using System;
using System.Data;
using System.Collections;
using System.Text;

using Icson.Utils;
using System.Transactions;
using Icson.Objects;
using Icson.Objects.RMA;

using Icson.DBAccess;
using Icson.DBAccess.RMA;

using Icson.BLL.Basic;

namespace Icson.BLL.RMA
{
    /// <summary>
    /// Summary description for OutBoundManager.
    /// </summary>
    public class RMAOutBoundManager
    {
        private RMAOutBoundManager()
        {
        }

        private static RMAOutBoundManager _instance;
        public static RMAOutBoundManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new RMAOutBoundManager();
            }
            return _instance;
        }

        private string getOutBoundID(int sysno)
        {
            return "R1" + sysno.ToString().PadLeft(8, '0');
        }

        private int getCurrentStatus(int sysno)
        {
            string sql = "select status from rma_outbound where sysno = " + sysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
            {
                throw new BizException("no such sysno");
            }
            return Util.TrimIntNull(ds.Tables[0].Rows[0]["Status"]);
        }

        private void map(RMAOutBoundInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.OutBoundID = Util.TrimNull(tempdr["OutBoundID"]);
            oParam.VendorSysNo = Util.TrimIntNull(tempdr["VendorSysNo"]);
            oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
            oParam.CreateUserSysNo = Util.TrimIntNull(tempdr["CreateUserSysNo"]);
            oParam.VendorName = Util.TrimNull(tempdr["VendorName"]);
            oParam.ZIP = Util.TrimNull(tempdr["Zip"]);
            oParam.Address = Util.TrimNull(tempdr["Address"]);
            oParam.Contact = Util.TrimNull(tempdr["Contact"]);
            oParam.Phone = Util.TrimNull(tempdr["Phone"]);
            oParam.OutTime = Util.TrimDateNull(tempdr["OutTime"]);
            oParam.OutUserSysNo = Util.TrimIntNull(tempdr["OutUserSysNo"]);
            oParam.Note = Util.TrimNull(tempdr["Note"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
            oParam.EOutBoundDate = Util.TrimDateNull(tempdr["EOutBoundDate"]);
            oParam.EResponseDate = Util.TrimDateNull(tempdr["EResponseDate"]);
            oParam.AreaSysNo = Util.TrimIntNull(tempdr["AreaSysNo"]);
            oParam.OutBoundInvoiceQty = Util.TrimIntNull(tempdr["OutBoundInvoiceQty"]);
            oParam.ShipType = Util.TrimIntNull(tempdr["ShipType"]);
            oParam.PackageID = Util.TrimNull(tempdr["PackageID"]);
            oParam.CheckQtyUserSysNo = Util.TrimIntNull(tempdr["CheckQtyUserSysNo"]);
            oParam.CheckQtyTime = Util.TrimDateNull(tempdr["CheckQtyTime"]);
            oParam.FreightUserSysNo = Util.TrimIntNull(tempdr["FreightUserSysNo"]);
            oParam.SetDeliveryManTime = Util.TrimDateNull(tempdr["SetDeliveryManTime"]);
        }

        public RMAOutBoundInfo Load(int sysno)
        {
            string sql = "select * from rma_outbound (NOLOCK) where sysno = " + sysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;

            RMAOutBoundInfo oOut = new RMAOutBoundInfo();
            map(oOut, ds.Tables[0].Rows[0]);

            string sql_item = "select registersysno from rma_outbound_item (NOLOCK) where outboundsysno = " + sysno;
            DataSet ds_item = SqlHelper.ExecuteDataSet(sql_item);
            foreach (DataRow dr in ds_item.Tables[0].Rows)
            {
                oOut.itemHash.Add(Util.TrimIntNull(dr["registerSysNo"]), null);
            }
            RMARegisterManager.GetInstance().ConvertRegisterBoundleHash(oOut.itemHash);

            return oOut;
        }

        public void Create(RMAOutBoundInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                oParam.SysNo = SequenceDac.GetInstance().Create("RMA_OutBound_Sequence");
                oParam.OutBoundID = getOutBoundID(oParam.SysNo);

                new RMAOutBoundDac().InsertMaster(oParam);
                int OutBoundInvoiceQty = 0;
                foreach (int registerSysNo in oParam.itemHash.Keys)
                {
                    object item = oParam.itemHash[registerSysNo];
                    this.InsertItem(oParam.SysNo, registerSysNo);
                    Hashtable htRegister = new Hashtable();
                    htRegister.Add("SysNo", registerSysNo);
                    htRegister.Add("OutBoundWithInvoice", Util.TrimIntNull(item.ToString()));

                    RMARegisterManager.GetInstance().UpdateRegister(htRegister);
                    if (Util.TrimIntNull(item.ToString()) == (int)AppEnum.OutBoundWithInvoice.SendWithInvoice)
                    {
                        OutBoundInvoiceQty = OutBoundInvoiceQty + 1;
                    }
                    else
                    {
                        OutBoundInvoiceQty = OutBoundInvoiceQty + 0;
                    }
                }
                Hashtable htOutBound = new Hashtable();
                htOutBound.Add("SysNo", oParam.SysNo);
                htOutBound.Add("OutBoundInvoiceQty", OutBoundInvoiceQty);
                RMAOutBoundManager.GetInstance().UpdateMaster(htOutBound);
                scope.Complete();
            }

        }

        public void UpdateMaster(Hashtable paramHt)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                //必须是初始状态
                if (getCurrentStatus((int)paramHt["SysNo"]) != (int)AppEnum.RMAOutBoundStatus.Origin)
                    throw new BizException("status is not origin now,  update failed");

                if (1 != new RMAOutBoundDac().UpdateMaster(paramHt))
                    throw new BizException("expected one-row update failed, update failed ");

                scope.Complete();
            }
        }

        public void UpdatePrepareTime(Hashtable paramHt)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                if (1 != new RMAOutBoundDac().UpdateMaster(paramHt))
                    throw new BizException("expected one-row update failed, update failed ");

                scope.Complete();
            }
        }

        public void Abandon(int masterSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                //必须是初始状态
                if (getCurrentStatus(masterSysNo) != (int)AppEnum.RMAOutBoundStatus.Origin)
                    throw new BizException("status is not origin now,  abandon failed");

                Hashtable ht = new Hashtable(5);
                ht.Add("SysNo", masterSysNo);
                ht.Add("Status", (int)AppEnum.RMAOutBoundStatus.Abandon);

                if (1 != new RMAOutBoundDac().UpdateMaster(ht))
                    throw new BizException("expected one-row update failed, update failed ");

                scope.Complete();
            }

        }

        //cancel abandon 还要检查明细中的Register是否加入到其他有效的单据中，所以不提供。
        //		public void CancelAbandon(int masterSysNo)
        //		{
        //		}

        public void OutStock(int masterSysNo, int userSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                //必须是初始状态
                if (getCurrentStatus(masterSysNo) != (int)AppEnum.RMAOutBoundStatus.Origin)
                    throw new BizException("status is not origin now,  outstock failed");

                Hashtable ht = new Hashtable(5);
                ht.Add("SysNo", masterSysNo);
                ht.Add("OutTime", DateTime.Now);
                ht.Add("OutUserSysNo", userSysNo);
                ht.Add("Status", (int)AppEnum.RMAOutBoundStatus.SendAlready);

                if (1 != new RMAOutBoundDac().UpdateMaster(ht))
                    throw new BizException("expected one-row update failed, update failed ");

                string sql = @"select registersysno , ownby , location,rma_register.receivestocksysno,rma_register.productsysno 
                                from
                                    rma_outbound_item (nolock)
                                    inner join rma_register (nolock) on rma_register.sysno = rma_outbound_item.registersysno
                                where
                                    rma_outbound_item.outboundsysno=" + masterSysNo;
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (Util.HasMoreRow(ds))
                {
                    //调用Register的更新
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (Util.TrimIntNull(dr["ownby"]) == (int)AppEnum.RMAOwnBy.Origin || Util.TrimIntNull(dr["location"]) == (int)AppEnum.RMALocation.Origin)
                        {
                            throw new BizException("非法操作！该产品已经退货入库或者已经原物发还给客户，无法继续送修！");
                        }

                        Hashtable htRegister = new Hashtable(2);
                        htRegister.Add("SysNo", Util.TrimIntNull(dr["registersysno"]));
                        htRegister.Add("OutBoundStatus", (int)AppEnum.RMAOutBoundStatus.SendAlready);
                        htRegister.Add("Location", (int)AppEnum.RMALocation.Vendor);
                        RMARegisterManager.GetInstance().UpdateRegister(htRegister);

                        InventoryManager.GetInstance().SetOutStockQty(Util.TrimIntNull(dr["receivestocksysno"]),Util.TrimIntNull(dr["productsysno"]),1);
                    }
                }

                scope.Complete();
            }

        }

        public void CancelOutStock(int masterSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                //必须是初始状态
                if (getCurrentStatus(masterSysNo) != (int)AppEnum.RMAOutBoundStatus.SendAlready)
                    throw new BizException("status is not send already now,  cancel outstock failed");

                Hashtable ht = new Hashtable(5);
                ht.Add("SysNo", masterSysNo);
                ht.Add("Status", (int)AppEnum.RMAOutBoundStatus.Origin);

                if (1 != new RMAOutBoundDac().UpdateMaster(ht))
                    throw new BizException("expected one-row update failed, update failed ");

                string sql = @"select
                                registersysno,rma_register.receivestocksysno,rma_register.productsysno 
                                    from rma_outbound_item
                                inner join rma_register (nolock) on rma_register.sysno = rma_outbound_item.registersysno
                                    where
                                rma_outbound_item.outboundsysno =" + masterSysNo;
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (Util.HasMoreRow(ds))
                {
                    //调用Register的更新
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {

                        Hashtable htRegister = new Hashtable(2);
                        htRegister.Add("SysNo", Util.TrimIntNull(dr["registersysno"]));
                        htRegister.Add("OutBoundStatus", (int)AppEnum.RMAOutBoundStatus.Origin);
                        htRegister.Add("Location", (int)AppEnum.RMALocation.Icson);
                        RMARegisterManager.GetInstance().UpdateRegister(htRegister);

                        InventoryManager.GetInstance().SetOutStockQty(Util.TrimIntNull(dr["receivestocksysno"]), Util.TrimIntNull(dr["productsysno"]), -1);
                    }
                }
                scope.Complete();
            }
        }


        public void InsertItem(int outBoundSysNo, int registerSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                int status = getCurrentStatus(outBoundSysNo);
                if (status != (int)AppEnum.RMAOutBoundStatus.Origin)
                    throw new BizException("the status is not origin");

                string sql = @"select 
									top 1 rma_outbound.sysno
								from
									rma_outbound (NOLOCK), rma_outbound_item (NOLOCK)
								where
									rma_outbound.sysno = rma_outbound_item.outboundsysno
								and rma_outbound.status <> @1
								and registersysno = @2";

                sql = sql.Replace("@1", ((int)AppEnum.RMAOutBoundStatus.Abandon).ToString());
                sql = sql.Replace("@2", registerSysNo.ToString());

                DataSet ds = SqlHelper.ExecuteDataSet(sql);

                if (Util.HasMoreRow(ds))
                    throw new BizException("this register has been add to a valid outbound sheet");

                new RMAOutBoundDac().InsertItem(outBoundSysNo, registerSysNo);

                scope.Complete();
            }
        }

        public void DeleteItem(int outBoundSysNo, int registerSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                int status = getCurrentStatus(outBoundSysNo);
                if (status != (int)AppEnum.RMAOutBoundStatus.Origin)
                    throw new BizException("the status is not origin");

                new RMAOutBoundDac().DeleteItem(outBoundSysNo, registerSysNo);

                scope.Complete();
            }
        }

        public DataSet GetWaitingOutBoundDs(Hashtable paramHash)
        {
//            string sql = @"select 
//                             rr.sysno as registersysno,rma_request.sosysno,product.productid,product.productname,
//							customerdesc,checkdesc,vendorname,lastvendorsysno 
//                            from rma_register as rr (NOLOCK)
//                            left join product (NOLOCK) on rr.productsysno = product.sysno
//                            left join vendor (NOLOCK) on vendor.sysno = lastvendorsysno
//                            left join rma_request_item (NOLOCK) on rr.sysno = rma_request_item.registersysno
//                            left join rma_request (NOLOCK) on rma_request.sysno = rma_request_item.requestsysno
//                           where
//                             rr.outboundstatus = @registerOutBoundStatus 
//                             and  rr.sysno not in 
//                             (
//                      		    select registersysno from rma_outbound (NOLOCK), rma_outbound_item (NOLOCK)
//							    where
//								    rma_outbound.sysno = rma_outbound_item.outboundsysno
//							    and rma_outbound.status <> @masterOutBoundStatus
//                             )
//                            @vendor
//                            order by registersysno desc
//                          ";
            string sql = @"select 
                             rr.sysno as registersysno,rma_request.sosysno,product.productid,product.productname,
							customerdesc,checkdesc,vendorname,vendor.sysno as lastvendorsysno 
                            from rma_register as rr (NOLOCK)
                            left join product (NOLOCK) on rr.productsysno = product.sysno 
                            left join product_id as pid (NOLOCK) on rr.productidsysno = pid.sysno
                            left join po_master as pm (NOLOCK) on pid.posysno = pm.sysno 
                            left join vendor (NOLOCK) on vendor.sysno = pm.vendorsysno
                            left join rma_request_item (NOLOCK) on rr.sysno = rma_request_item.registersysno
                            left join rma_request (NOLOCK) on rma_request.sysno = rma_request_item.requestsysno
                           where
                             rr.outboundstatus = @registerOutBoundStatus 
                             and  rr.sysno not in 
                             (
                      		    select registersysno from rma_outbound (NOLOCK), rma_outbound_item (NOLOCK)
							    where
								    rma_outbound.sysno = rma_outbound_item.outboundsysno
							    and rma_outbound.status <> @masterOutBoundStatus
                             )
                            @vendor
                            order by registersysno desc
                          ";

            sql = sql.Replace("@registerOutBoundStatus", ((int)AppEnum.RMAOutBoundStatus.Origin).ToString());
            sql = sql.Replace("@masterOutBoundStatus", ((int)AppEnum.RMAOutBoundStatus.Abandon).ToString());
            if (paramHash.ContainsKey("Vendor"))
            {
                if (paramHash["Vendor"].ToString().Trim().Length > 0)
                {
                    if (paramHash["Vendor"].ToString().Trim() == "all")
                        sql = sql.Replace("@vendor", "");
                    else
                        sql = sql.Replace("@vendor", " and lastvendorsysno=" + Util.TrimIntNull(paramHash["Vendor"]));
                }
                else
                    sql = sql.Replace("@vendor", " and  lastvendorsysno is null");
            }
            else
                sql = sql.Replace("@vendor", "");

            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetOutBoundDs(Hashtable paramHash)
        {
            string sql = @" select rma.*, b.username as CreateName, c.username as OutName,d.VendorName,d.WarrantyAddress,d.WarrantyContact,d.WarrantyPhone  
							from rma_outbound rma (NOLOCK), sys_user b (NOLOCK), sys_user c (NOLOCK),vendor d (NOLOCK)
							where 
								rma.createusersysno *= b.sysno
								and rma.outusersysno *= c.sysno 
                                and rma.vendorsysno = d.sysno ";
            if (paramHash != null && paramHash.Count != 0)
            {
                StringBuilder sb = new StringBuilder(100);
                foreach (string key in paramHash.Keys)
                {
                    sb.Append(" and ");
                    object item = paramHash[key];
                    if (key == "DateFrom")
                    {
                        sb.Append("rma.CreateTime >= ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "DateTo")
                    {
                        sb.Append("rma.CreateTime <= ").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "ProductSysNo")
                    {
                        sb.Append(" exists ( select top 1 rma_register.sysno from rma_outbound_item rma_item, rma_register where rma.sysno=rma_item.outboundsysno and rma_item.registersysno = rma_register.sysno and productsysno = ").Append(Util.SafeFormat(item.ToString())).Append(" ) ");
                    }
                    else if (key == "Status")
                    {
                        sb.Append("rma.Status = ").Append(item.ToString());
                    }
                    else if (key == "CreateFrom")
                    {
                        sb.Append(" rma.CreateTime >=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "CreateTo")
                    {
                        sb.Append(" rma.CreateTime <=").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "WarrantySelfSend")
                    {
                        sb.Append("d.WarrantySelfSend = ").Append(Util.TrimNull(item.ToString()));
                    }
                    else if (key == "VendorName")
                    {
                        sb.Append("d.VendorName like ").Append(Util.ToSqlLikeString(item.ToString()));
                    }
                    else if (key == "SOSysNo")
                    {
                        sb.Append(" exists (select top 1 rma_request.sysno from rma_request,rma_request_item,rma_outbound_item rma_item where rma.sysno=rma_item.outboundsysno and rma_item.registersysno=rma_request_item.registersysno and rma_request_item.requestsysno=rma_request.sysno and rma_request.sosysno=").Append(Util.SafeFormat(item.ToString())).Append(")");
                    }
                    else if (item is int)
                    {
                        sb.Append(key).Append("=").Append(item.ToString());
                    }
                    else if (item is string)
                    {
                        sb.Append(key).Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                    }
                }
                sql += sb.ToString();
            }
            else
            {
                sql = sql.Replace("select", "select top 50");
            }
            sql += " order by rma.sysno desc";
            return SqlHelper.ExecuteDataSet(sql);
        }
        
        /// <summary>
        /// destination:获取送修但未返还物品清单
        /// </summary>
        /// <returns></returns>

        public DataSet GetOutBoundNoReturnDS(Hashtable paramHash)
        {
            string sql = @"select 
                            RMA_OutBound.VendorSysNo as VendorSysNo,RMA_OutBound.VendorName as VendorName,Product.PMUserSysNo as PMUserSysNo,
                            Product.ProductID as ProductID,Product.ProductName as ProductName,
                            RMA_Register.Cost as ProductCost,RMA_Register.SysNo as RMASysNo,
                            V_SO_Master.SysNo as SOSysNo,V_SO_Master.AuditTime as SODate,V_SO_Item.Warranty as Warranty,
                            RMA_OutBound.SysNo as OutBoundSysNo,RMA_OutBound.OutTime as OutTime ,RMA_Register.IsWithin7Days as IsWithin7Days,
                            RMA_Register.PMDunDesc as PMDunDesc, RMA_Register.Memo as Memo
                            from dbo.RMA_OutBound (NOLOCK)  
                            inner join dbo.RMA_OutBound_Item (NOLOCK) on RMA_OutBound_Item.OutBoundSysNo = RMA_OutBound.SysNo
                            inner join dbo.RMA_Register (NOLOCK) on RMA_Register.SysNo = RMA_OutBound_Item.RegisterSysNo
                            inner join dbo.Product (NOLOCK) on Product.SysNo = RMA_Register.ProductSysNo
                            inner join dbo.RMA_Request_Item (NOLOCK) on RMA_Request_Item.RegisterSysNo = RMA_Register.Sysno
                            inner join dbo.RMA_Request (NOLOCK) on RMA_Request.SysNo = RMA_Request_Item.RequestSysNo
                            inner join dbo.V_SO_Master (NOLOCK) on V_SO_Master.SysNo = RMA_Request.SoSysNo
                            inner join dbo.V_SO_Item (NOLOCK) on (V_SO_Item.SOSysNo = V_SO_Master.SysNo and V_SO_Item.ProductSysNo = Product.SysNo)
							inner join dbo.category3 (NOLOCK) on product.c3sysno = category3.sysno
							inner join dbo.category2 (NOLOCK) on category3.c2sysno = category2.sysno
							inner join dbo.category1 (NOLOCK) on category2.c1sysno = category1.sysno

                           where RMA_Register.Status=@RegisterStatus and RMA_Register.OutBoundStatus =@RegisterOutBoundStatus and RMA_OutBound.Status <> @RMAOutBoundStatus 
                                 @OutBoundTimeFrom  @OutBoundTimeTo  @VendorSysNo  @SOSysNo @ProductSysNo  @PMUserSysNo @CSysNo ";
            sql += "order by RMA_OutBound.VendorName,RMA_OutBound.OutTime,Product.PMUserSysNo ";

            sql = sql.Replace("@RegisterStatus", ((int)AppEnum.RMARequestStatus.Handling).ToString());
            sql = sql.Replace("@RegisterOutBoundStatus", ((int)AppEnum.RMAOutBoundStatus.SendAlready).ToString());
            sql = sql.Replace("@RMAOutBoundStatus", ((int)AppEnum.RMAOutBoundStatus.Abandon).ToString());

            if (paramHash.ContainsKey("OutBoundTimeFrom"))
                sql = sql.Replace("@OutBoundTimeFrom", " and RMA_OutBound.OutTime>='" + paramHash["OutBoundTimeFrom"] + "'");
            else
                sql = sql.Replace("@OutBoundTimeFrom", "");

            if (paramHash.ContainsKey("OutBoundTimeTo"))
                sql = sql.Replace("@OutBoundTimeTo", "  and RMA_OutBound.OutTime<=" + Util.ToSqlEndDate(paramHash["OutBoundTimeTo"].ToString()) + "");
            else
                sql = sql.Replace("@OutBoundTimeTo", "");

            if (paramHash.ContainsKey("VendorSysNo"))
                sql = sql.Replace("@VendorSysNo", " and RMA_OutBound.VendorSysNo=" + paramHash["VendorSysNo"] + "");
            else
                sql = sql.Replace("@VendorSysNo", "");

            if (paramHash.ContainsKey("SoSysNo"))
                sql = sql.Replace("@SOSysNo", " and  V_SO_Master.SysNo=" + paramHash["SoSysNo"] + "");
            else
                sql = sql.Replace("@SOSysNo", "");

            if (paramHash.ContainsKey("ProductSysNo"))
                sql = sql.Replace("@ProductSysNo", " and Product.SysNo=" + paramHash["ProductSysNo"] + "");
            else
                sql = sql.Replace("@ProductSysNo", "");

            if (paramHash.ContainsKey("PMUserSysNo"))
                sql = sql.Replace("@PMUserSysNo", " and Product.PMUserSysNo=" + paramHash["PMUserSysNo"] + "");
            else
                sql = sql.Replace("@PMUserSysNo", "");

            if (paramHash.ContainsKey("Category"))
                sql = sql.Replace("@CSysNo", " and " + paramHash["Category"].ToString().Trim());
            else
                sql = sql.Replace("@CSysNo", "");

            return SqlHelper.ExecuteDataSet(sql);

        }


        public int GetFreightUserSysNoByID(string outBoundID)
        {
            string sql = @"select FreightUserSysNo
                           from  rma_outbound (nolock)
                           where rma_outbound.outBoundID='" + outBoundID + "'";

            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (Util.HasMoreRow(ds))
                return (int)ds.Tables[0].Rows[0][0];
            else
                return AppConst.IntNull;

        }

        public int GetDLSysNoSysNoByID(string outBoundID)
        {
            string sql = @"select dlsysno
                           from  rma_outbound (nolock)
                           where rma_outbound.outBoundID='" + outBoundID + "'";

            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (Util.HasMoreRow(ds))
                return (int)ds.Tables[0].Rows[0][0];
            else
                return AppConst.IntNull;

        }

        public int GetOutBoundSysNoByID(string outBoundID)
        {
            string sql = @"select SysNo
                           from  rma_outbound (nolock)
                           where rma_outbound.outBoundID='" + outBoundID + "'";

            int outBoundSysNo = AppConst.IntNull;

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                DataRow dr = ds.Tables[0].Rows[0];
                outBoundSysNo = Util.TrimIntNull(dr["SysNo"]);
            }

            return outBoundSysNo;
        }
        public DataSet GetProductByOutbBound(int outBoundSysNo)
        {
            string sql = @"select RMA_Register.sysno as RegisterSysno, Product.ProductID,Product.ProductName,Product.Sysno as ProductSysno
                           from RMA_Register (NOLOCK) 
                           inner join RMA_outBound_Item (NOLOCK)  on RMA_Register.SysNo = RMA_outBound_Item.RegisterSysNo
                           inner join Product (NOLOCK)  on Product.SysNo = RMA_Register.ProductSysNo
                           where RMA_outBound_Item.OutBoundSysNo=" + outBoundSysNo + "";
            return SqlHelper.ExecuteDataSet(sql);
        }


        public void UpdateOutBoundCheckInfo(Hashtable paramHash)
        {
            new RMAOutBoundDac().UpdateMaster(paramHash);
        }

        public bool GetRevertPackageByOutBoundSysNo(int outBoundSysNo)
        {
            string sql = @"select PackageID 
                           from  rma_outBound(nolock)
                           where sysno = " + outBoundSysNo;

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            bool s = false;
            if (Util.HasMoreRow(ds))
            {
                DataRow dr = ds.Tables[0].Rows[0];
                string packageID = Util.TrimNull(dr["packageid"]);
                if (packageID != AppConst.StringNull)
                    s = true;
                else
                    s = false;
            }
            return s;
        }

        public void SetDeliveryMen(int sysno, int freightManSysNo, int dlsysno)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                Hashtable updateHash = new Hashtable();
                updateHash.Add("SysNo", sysno);
                updateHash.Add("freightusersysno", freightManSysNo);
                updateHash.Add("SetDeliveryManTime", System.DateTime.Now);
                updateHash.Add("DLSysNo", dlsysno);
                new RMAOutBoundDac().UpdateMaster(updateHash);
                scope.Complete();
            }
        }

        public void SetDeliveryMen(Hashtable ht)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                new RMAOutBoundDac().UpdateMaster(ht);
                scope.Complete();
            }
        }

        public DataSet GetFreightMenDs(Hashtable paramHash)
        {
            string sql = @"select RMA_OutBound.*,area.DistrictName,area.localcode,sys_user.username as freightusername
                          from RMA_OutBound,area ,sys_user
                          where RMA_OutBound.status=1 and AreaSysNo=area.sysno and sys_user.sysno=FreightUserSysNo";


            string sql1 = "";

            if (paramHash != null && paramHash.Count != 0)
            {
                StringBuilder sb = new StringBuilder(100);
                foreach (string key in paramHash.Keys)
                {
                    object item = paramHash[key];
                    if (key == "DateFrom")
                    {
                        sb.Append(" and");
                        sb.Append(" SetDeliveryManTime >=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "DateTo")
                    {
                        sb.Append(" and");
                        sb.Append(" SetDeliveryManTime <=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (item is int)
                    {
                        sb.Append(" and ");
                        sb.Append(key).Append("=").Append(item.ToString());
                    }
                    else if (key == "orderby")
                    {
                        sql1 = " order by SetDeliveryManTime desc ";
                    }
                    sql += sb.ToString();
                }
            }
            sql += sql1;
            return SqlHelper.ExecuteDataSet(sql);
        }
    }
}
