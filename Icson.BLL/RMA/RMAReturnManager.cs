using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections;

using Icson.Objects;
using Icson.Objects.RMA;
using Icson.Objects.Basic;
using Icson.Objects.ImportData;

using Icson.Utils;

using Icson.DBAccess;
using Icson.DBAccess.RMA;
using Icson.DBAccess.ImportData;

using System.Transactions;

using Icson.BLL.Basic;
using Icson.BLL.Online;
using Icson.BLL.Finance;
using Icson.BLL;

namespace Icson.BLL.RMA
{
    /// <summary>
    /// Summary description for RMAReturnManager.
    /// </summary>
    public class RMAReturnManager
    {
        private RMAReturnManager()
        {

        }
        private static RMAReturnManager _instance;
        public static RMAReturnManager GetInstance()
        {
            if (_instance == null)
                _instance = new RMAReturnManager();
            return _instance;
        }

        public void InsertReturn(RMAReturnInfo oInfo)
        {
            new RMAReturnDac().Insert(oInfo);
        }

        public void InsertReturnItem(RMAReturnItemInfo oInfo)
        {
            new RMAReturnDac().InsertReturnItem(oInfo);
        }

        public DataSet ReturnCustomerList()
        {
            string sql = @"select  distinct customer.sysno , customer.customername   
                            from  RMA_Register (NOLOCK)
                            left  join RMA_Request_Item (NOLOCK) on RMA_Register.sysno = RMA_Request_Item.RegisterSysNo
                            left  join RMA_Request (NOLOCK) on RMA_Request.sysno = RMA_Request_Item.RequestSysNo
                            left  join customer (NOLOCK) on RMA_Request.CustomerSysNo = customer.sysno 
                            where RMA_Register.ReturnStatus = @ReturnStatus
                             and  RMA_Register.sysno not in
                             (select RMA_Return_Item.registerSysNo
                              from   RMA_Return_Item (NOLOCK)
                              inner join RMA_Return (NOLOCK) on RMA_Return_Item.ReturnSysNo = RMA_Return.sysno 
                              where  RMA_Return.status = @ReturnStatus ) ";
            sql = sql.Replace("@ReturnStatus", ((int)AppEnum.RMAReturnStatus.WaitingReturn).ToString());
            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetWaitingReturn()
        {
            string sql = @"select RMA_Register.sysno as registersysno , Product.productname , RequestType , CustomerDesc ,
                                  RMA_Request.contact , RMA_Request.address ,SO_Master.ReceiveZip , RMA_Request.Phone , RMA_Register.NewProductStatus
                           from   RMA_Register (NOLOCK)
                           left  join RMA_Request_Item (NOLOCK) on RMA_Request_Item.RegisterSysNo = RMA_Register.sysno
                           left  join RMA_Request (NOLOCK) on RMA_Request.sysno = RMA_Request_Item.RequestSysNo 
                           left  join Product (NOLOCK) on Product.sysno = RMA_Register.productsysno 
                           left  join V_SO_Master SO_Master (NOLOCK) on SO_Master.sysno = RMA_Request.SOSysNo
                           where RMA_Register.ReturnStatus = @ReturnStatus
                           and RMA_Register.sysno not in 
                              (select  RMA_Return_Item.registerSysNo 
                                from   RMA_Return_Item (NOLOCK)
                                inner  join RMA_Return (NOLOCK) on RMA_Return_Item.ReturnSysNo = RMA_Return.sysno
                                where  RMA_Return.status = @ReturnStatus )  ";

            sql = sql.Replace("@ReturnStatus", ((int)AppEnum.RMAReturnStatus.WaitingReturn).ToString());

            return SqlHelper.ExecuteDataSet(sql);
        }

        public void Create(RMAReturnInfo returnInfo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                returnInfo.SysNo = SequenceDac.GetInstance().Create("RMA_Return_Sequence");
                returnInfo.ReturnID = getRevertID(returnInfo.SysNo);

                new RMAReturnDac().Insert(returnInfo);
                foreach (int registerSysNo in returnInfo.ItemHash.Keys)
                {
                    RMAReturnItemInfo oReturnItem = new RMAReturnItemInfo();
                    oReturnItem.RegisterSysNo = registerSysNo;
                    oReturnItem.ReturnSysNo = returnInfo.SysNo;

                    this.InsertReturnItem(oReturnItem);
                }

                scope.Complete();
            }
        }

        private string getRevertID(int sysno)
        {
            return "R4" + sysno.ToString().PadLeft(8, '0');
        }

        private int GetPSysNobyRegiSysNo(int regiSysNo)
        {
            string sql = "select productsysno from RMA_Register (NOLOCK) where sysno=" + regiSysNo;

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                return 0;
            DataRow dr = ds.Tables[0].Rows[0];
            return (int)dr["productsysno"];

        }

        public DataSet GetReturnList(Hashtable paramHash)
        {
            string sql = @"select distinct RMA_Return.*  , createuser.username as createusername , 
                                  returnuser.username as returnusername ,Stock.StockName
                           from   RMA_Return (NOLOCK)                         
                           left   join  Sys_User as createuser  (NOLOCK)  on createuser.sysno = RMA_Return.CreateUserSysNo
                           left   join  Sys_User as returnuser  (NOLOCK)  on returnuser.sysno = RMA_Return.ReturnUserSysNo
                           left   join  Stock  (NOLOCK)  on Stock.sysno = RMA_Return.StockSysNo
                           inner  join  RMA_Return_Item   (NOLOCK) on RMA_Return.sysno = RMA_Return_Item.ReturnSysNo
                           inner  join RMA_Register  (NOLOCK)  on RMA_Register.SysNo = RMA_Return_Item.RegisterSysNo
                           inner  join RMA_Request_Item   (NOLOCK) on RMA_Request_Item.registerSysNo = RMA_Register.SysNo
                           inner  join RMA_Request (NOLOCK)  on RMA_Request.SysNo = RMA_Request_Item.RequestSysNo  
                           left  join  Product as TargetProduct (NOLOCK)  on TargetProduct.SysNo = RMA_Return_Item.TargetProductSysNO                          
                           where   1=1  @SysNo @DateFrom  @DateTo  @ReturnID @RegisterSysNo  @SoSysNo  @ProductSysNo @WaitingAudit @OtherProduct  @TargetProductType @ReturnedStatus order by RMA_Return.SysNo desc";

            if (paramHash.ContainsKey("SysNo"))
                sql = sql.Replace("@SysNo", " and RMA_Return.SysNo =" + paramHash["SysNo"].ToString());
            else
                sql = sql.Replace("@SysNo", "");

            if (paramHash.ContainsKey("DateFrom"))
                sql = sql.Replace("@DateFrom", " and RMA_Return.CreateTime >=" + Util.ToSqlString(paramHash["DateFrom"].ToString()));
            else
                sql = sql.Replace("@DateFrom", "");

            if (paramHash.ContainsKey("DateTo"))
                sql = sql.Replace("@DateTo", " and RMA_Return.CreateTime <=" + Util.ToSqlString(paramHash["DateTo"].ToString()));
            else
                sql = sql.Replace("@DateTo", "");

            if (paramHash.ContainsKey("ReturnID"))
                sql = sql.Replace("@ReturnID", " and RMA_Return.ReturnID like '%" + paramHash["ReturnID"].ToString() + "%'");
            else
                sql = sql.Replace("@ReturnID", "");

            if (paramHash.ContainsKey("RegisterSysNo"))
                sql = sql.Replace("@RegisterSysNo", " and RMA_Register.SysNo = " + paramHash["RegisterSysNo"].ToString());
            else
                sql = sql.Replace("@RegisterSysNo", "");

            if (paramHash.ContainsKey("SoSysNo"))
                sql = sql.Replace("@SoSysNo", " and RMA_Request.SoSysNo = " + paramHash["SoSysNo"].ToString());
            else
                sql = sql.Replace("@SoSysNo", "");

            if (paramHash.ContainsKey("ProductSysNo"))
                sql = sql.Replace("@ProductSysNo", " and RMA_Register.ProductSysNo = " + paramHash["ProductSysNo"].ToString());
            else
                sql = sql.Replace("@ProductSysNo", "");

            //只显示待审核的
            if (paramHash.ContainsKey("onlyShowWaitingAudit"))
                sql = sql.Replace("@WaitingAudit", " and RMA_Return_Item.AuditStatus = " + ((int)AppEnum.TriStatus.Origin).ToString());
            else
                sql = sql.Replace("@WaitingAudit", "");
            //只显示入其他产品的
            if (paramHash.ContainsKey("onlyShowOtherProduct"))
                sql = sql.Replace("@OtherProduct", " and RMA_Return_Item.ReturnProductType = " + ((int)AppEnum.ProductType.OtherProduct).ToString());
            else
                sql = sql.Replace("@OtherProduct", "");

            //入库产品类型
            if (paramHash.ContainsKey("TargetProductType"))
            {
                sql = sql.Replace("@TargetProductType", " and RMA_Return_Item.ReturnProductType = " + paramHash["TargetProductType"]);
                sql = sql.Replace("@ReturnedStatus", " and RMA_Register.ReturnStatus = " + (int)AppEnum.RMAReturnStatus.Returned);
            }
            else
            {
                sql = sql.Replace("@TargetProductType", "");
                sql = sql.Replace("@ReturnedStatus", "");
            }


            if ((paramHash == null || paramHash.Count == 0) || (paramHash.Count == 1 && paramHash.ContainsKey("TargetProductType")))
                sql = sql.Replace("select distinct", "select distinct top 50");

            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetReturnItem(int sysno)
        {
            string sql = @"select RMA_Register.SysNO ,RMA_Register.ReturnStatus,RMA_Register.Status,RMA_Register.Memo, Product.ProductName ,Product.ProductId, Product.sysno as ProductSysNo,TargetProduct.ProductID as TargetProductID, RMA_Return_Item.TargetProductSysNO,
                                  TargetProduct.ProductName as TargetProductName,RMA_Return_Item.SysNo as ReturnItemSysNo
                                  ,RMA_Return_Item.Cost,RMA_Return_Item.AuditStatus
                            from   RMA_Register (NOLOCK) 
                            inner  join  RMA_Return_Item (NOLOCK) on RMA_Register.SysNo = RMA_Return_Item.RegisterSysNo
                            inner  join  Product (NOLOCK) on Product.SysNo = RMA_Register.ProductSysNo
                            left  join  Product as TargetProduct (NOLOCK) on TargetProduct.SysNo = RMA_Return_Item.TargetProductSysNO
                            where  RMA_Return_Item.ReturnSysNo = " + sysno;

            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetSOByReturn(int returnSysNo)
        {
            string sql = @"select rma_request.sosysno from rma_return inner join rma_return_item on rma_return.sysno=rma_return_item.returnsysno 
                            inner join rma_request_item on rma_return_item.registersysno=rma_request_item.registersysno 
                            inner join rma_request on rma_request.sysno=rma_request_item.requestsysno 
                            where rma_return.sysno=" + returnSysNo;
            return SqlHelper.ExecuteDataSet(sql);
        }

        public bool CheckTarget(int sysno)
        {
            string sql = "select TargetProductSysNo  from   RMA_Return_Item (NOLOCK)   where  ReturnSysNo =" + sysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return true;

            bool result = true;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Util.TrimIntNull(dr["TargetProductSysNo"]) == AppConst.IntNull)
                    result = false;
            }
            return result;
        }

        public bool CheckAudit(int sysno)
        {
            //是返回其他产品的类型且不是审核通过的，包括未审核或者审核失败的。
            string sql = "select count(*)  from   RMA_Return_Item (NOLOCK)   where  ReturnProductType=" + ((int)AppEnum.ProductType.OtherProduct).ToString() + " and AuditStatus!=" + ((int)AppEnum.TriStatus.Handled).ToString()
               + " and ReturnSysNo =" + sysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            bool result = true;
            if ((int)ds.Tables[0].Rows[0][0] > 0) result = false;
            else result = true;
            return result;
        }

        public void Return(Hashtable paramHash)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                this.UpdateReturn(paramHash);
                string sql = @"select RMA_Return.StockSysNo , RMA_Return_Item.TargetProductSysNo , RMA_Return_Item.Cost ,RMA_Return.Note ,
                               RMA_Return_Item.RegisterSysNo ,RMA_Register.Ownby , RMA_Register.Location
                               from   RMA_Return_Item (NOLOCK)
                               inner  join RMA_Return (NOLOCK) on RMA_Return_Item.ReturnSysNo = RMA_Return.SysNo
                               inner  join RMA_Register (NOLOCK) on RMA_Register.sysno = rma_return_item.registersysno 
                               where  RMA_Return_Item.ReturnSysNo =" + paramHash["SysNo"].ToString();

                DataSet ds = SqlHelper.ExecuteDataSet(sql);

                if (Util.HasMoreRow(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        //if (Util.TrimIntNull(dr["OwnBy"]) != (int)AppEnum.RMAOwnBy.Icson || Util.TrimIntNull(dr["Location"]) != (int)AppEnum.RMALocation.Icson)
                        //    throw new BizException("操作失败！满足如下条件的RMA单，才可以进行Return操作：<br>已经退款的商品，未送修或者送修已返还；<br>已经换货给客户，未送修或者送修已返还。");

                        UnitCostManager.GetInstance().SetCost(Util.TrimIntNull(dr["TargetProductSysNo"]), 1, Util.TrimDecimalNull(dr["Cost"]));

                        DataRow drRR = RMARegisterManager.GetInstance().GetRegisterRow(Util.TrimIntNull(dr["RegisterSysNo"]));

                        InventoryManager.GetInstance().SetOutStockQty(Util.TrimIntNull(dr["StockSysNo"]), Util.TrimIntNull(drRR["ProductSysNo"]), 1);
                        InventoryManager.GetInstance().SetInStockQty(Util.TrimIntNull(dr["StockSysNo"]), Util.TrimIntNull(dr["TargetProductSysNo"]), 1);

                        Hashtable htRegister = new Hashtable();
                        htRegister.Add("SysNo", Util.TrimIntNull(dr["RegisterSysNo"]));
                        htRegister.Add("OwnBy", (int)AppEnum.RMAOwnBy.Origin);
                        htRegister.Add("Location", (int)AppEnum.RMALocation.Origin);
                        RMARegisterManager.GetInstance().UpdateRegister(htRegister);

                        //如果是商品入库，mail通知
                        //ProductBasicInfo oTargetProduct = ProductManager.GetInstance().LoadBasic(Util.TrimIntNull(dr["TargetProductSysNo"]));

                        //TCPMail oMail = new TCPMail();
                        //string mailadd = ProductRemarkManager.GetInstance().GetPMEmailByProduct(oTargetProduct.SysNo);
                        //string mailSubject = "[" + oTargetProduct.ProductID + "]" + oTargetProduct.ProductName + "入库通知";
                        //string mailBody = "[" + oTargetProduct.ProductID + "]" + oTargetProduct.ProductName + ",客户退货，已经入库。<br>Note:" + dr["Note"].ToString();
                        //oMail.Send(mailadd, mailSubject, mailBody);

                        //代销产品取消出库直接转为代销库存
                        ////if (oTargetProduct.IsConsign == (int)AppEnum.ConsignmentAttribute.Consign && oTargetProduct.ProductID.Substring(oTargetProduct.ProductID.Length - 1, 1).ToUpper() != "R")
                        //if (oTargetProduct.IsConsign == (int)AppEnum.ConsignmentAttribute.Consign && oTargetProduct.ProductType != (int)AppEnum.ProductType.SecondHand)
                        //{
                        //    InventoryManager.GetInstance().TransferConsignToAccQty
                        //        (Util.TrimIntNull(dr["StockSysNo"]), Util.TrimIntNull(dr["TargetProductSysNo"]), (-1) * 1, (int)AppEnum.ConsignToAccType.Manual);
                        //}
                        //代销产品取消出库直接转为代销库存
                    }
                }

                scope.Complete();
            }
        }

        public void CancelReturn(int sysno)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                Hashtable ht = new Hashtable();
                ht.Add("SysNo", sysno);
                ht.Add("Status", (int)AppEnum.RMAReturnStatus.WaitingReturn);

                this.UpdateReturn(ht);

                new RMAReturnDac().CancelReturn(sysno);

                string sql = @"select RMA_Return.StockSysNo , RMA_Return_Item.TargetProductSysNo , RMA_Return_Item.Cost ,RMA_Return_Item.RegisterSysNo
                               from   RMA_Return_Item (NOLOCK)
                               inner  join RMA_Return (NOLOCK) on RMA_Return_Item.ReturnSysNo = RMA_Return.SysNo
                               where  RMA_Return_Item.ReturnSysNo =" + sysno;

                DataSet ds = SqlHelper.ExecuteDataSet(sql);

                if (Util.HasMoreRow(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        //RMA取消退货入库：取消成本加权
                        //UnitCostManager.GetInstance().SetCost(Util.TrimIntNull(dr["TargetProductSysNo"]), -1, Util.TrimDecimalNull(dr["Cost"]));
                        DataRow drRR = RMARegisterManager.GetInstance().GetRegisterRow(Util.TrimIntNull(dr["RegisterSysNo"]));

                        if (Util.TrimIntNull(drRR["Status"]) != (int)AppEnum.RMARequestStatus.Handling)
                        {
                            throw new BizException("单件状态不是处理中，无法进行取消退货");
                        }

                        InventoryManager.GetInstance().SetInStockQty(Util.TrimIntNull(dr["StockSysNo"]), Util.TrimIntNull(drRR["ProductSysNo"]), 1);
                        InventoryManager.GetInstance().SetInStockQty(Util.TrimIntNull(dr["StockSysNo"]), Util.TrimIntNull(dr["TargetProductSysNo"]), -1);

                        Hashtable htRegister = new Hashtable();
                        htRegister.Add("SysNo", Util.TrimIntNull(dr["RegisterSysNo"]));
                        htRegister.Add("OwnBy", (int)AppEnum.RMAOwnBy.Icson);
                        htRegister.Add("Location", (int)AppEnum.RMALocation.Icson);
                        RMARegisterManager.GetInstance().UpdateRegister(htRegister);
                    }
                }

                scope.Complete();
            }
        }

        public void UpdateReturn(Hashtable paramHash)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                new RMAReturnDac().UpdateReturn(paramHash);

                //如果状态Status改变，改变Register中相应的Status
                if (paramHash.ContainsKey("Status"))
                {
                    string sql = "select RegisterSysNo from RMA_Return_Item (NOLOCK) where ReturnSysNo = " + paramHash["SysNo"].ToString();
                    DataSet ds = SqlHelper.ExecuteDataSet(sql);
                    if (Util.HasMoreRow(ds))
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            Hashtable htRegister = new Hashtable();
                            htRegister.Add("SysNo", Util.TrimIntNull(dr["RegisterSysNo"]));
                            htRegister.Add("ReturnStatus", Util.TrimIntNull(paramHash["Status"]));
                            RMARegisterManager.GetInstance().UpdateRegister(htRegister);
                        }
                    }
                }
                scope.Complete();
            }
        }

        /// <summary>
        /// 选择目标仓库时更新RMA_Return_Item中的Cost ,date:2006-10-12
        /// </summary>
        /// <param name="paramHash"></param>
        /// <returns></returns>
        public void UpdateReturnItemCost(Hashtable paramHash)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                string sql = @"select RMA_Register.RefundStatus ,RMA_Register.SysNo as registerSysNo,
                              RMA_Register.NewProductStatus,RMA_Register.RevertStatus
                              from RMA_Register (NOLOCK) 
                              inner join RMA_Return_Item (NOLOCK) on RMA_Register.SysNo  = RMA_Return_Item.RegisterSysNo  
                              where RMA_Return_Item.SysNo = " + paramHash["SysNo"] + " ";

                DataSet ReturnRegisterDs = SqlHelper.ExecuteDataSet(sql);
                if (Util.HasMoreRow(ReturnRegisterDs))
                {
                    foreach (DataRow rrdr in ReturnRegisterDs.Tables[0].Rows)
                    {
                        if (Util.TrimIntNull(rrdr["RefundStatus"]) == (int)AppEnum.RMARefundStatus.Audited || Util.TrimIntNull(rrdr["RefundStatus"]) == (int)AppEnum.RMARefundStatus.Refunded)
                        {
                            updateReturnItemCostForRefund(Util.TrimIntNull(rrdr["registerSysNo"]));
                        }
                        else if ((Util.TrimIntNull(rrdr["NewProductStatus"]) != (int)AppEnum.NewProductStatus.Origin) && (Util.TrimIntNull(rrdr["RevertStatus"]) == (int)AppEnum.RMARevertStatus.Reverted))
                        {
                            updateReturnItemCostNotRefund(Util.TrimIntNull(rrdr["registerSysNo"]), Util.TrimIntNull(paramHash["SysNo"]));
                        }
                        else
                        {
                            throw new BizException("操作失败！只有已经审核退款或者换货给客户的情况，才可以进行次项操作！");
                        }
                    }
                }

                scope.Complete();
            }
        }
        /// <summary>
        /// For 些单件已经退款
        /// </summary>
        public void updateReturnItemCostForRefund(int registerSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                string sql = @"select RMA_Refund_Item.RefundCost
                               from RMA_Register (NOLOCK) inner join RMA_Refund_Item (NOLOCK) on RMA_Register.SysNo = RMA_Refund_Item.RegisterSysNo
                               where RMA_Register.SysNo = " + registerSysNo + "";

                DataSet ReturnCostDs = SqlHelper.ExecuteDataSet(sql);
                if (Util.HasMoreRow(ReturnCostDs))
                {
                    foreach (DataRow dr in ReturnCostDs.Tables[0].Rows)
                    {
                        Hashtable CostReturnItem = new Hashtable();
                        CostReturnItem.Add("SysNo", registerSysNo);
                        CostReturnItem.Add("Cost", Util.TrimDecimalNull(dr["RefundCost"]));
                        new RMAReturnDac().UpdateReturnItem(CostReturnItem);
                    }
                }
                scope.Complete();
            }
        }
        
        /// <summary>
        /// for 单件不是退款，而是退新品
        /// </summary>
        /// <param name="registerSysNo"></param>
        /// <param name="returnItemSysNo"></param>
        public void updateReturnItemCostNotRefund(int registerSysNo, int returnItemSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                string sql = @"select RMA_Revert_Item.Cost
                               from RMA_Revert_Item (NOLOCK) inner join RMA_Register (NOLOCK) on  RMA_Register.SysNo = RMA_Revert_Item.RegisterSysNo
                               where  RMA_Register.SysNo = " + registerSysNo + " and RMA_Register.RevertStatus=" + (int)AppEnum.RMARevertStatus.Reverted + " and RMA_Register.NewProductStatus<>" + (int)AppEnum.NewProductStatus.Origin + "";

                DataSet ReturnCostDs = SqlHelper.ExecuteDataSet(sql);
                if (Util.HasMoreRow(ReturnCostDs))
                {
                    foreach (DataRow dr in ReturnCostDs.Tables[0].Rows)
                    {
                        Hashtable CostReturnItem = new Hashtable();
                        CostReturnItem.Add("SysNo", registerSysNo);
                        if (dr["Cost"].ToString() != string.Empty || Util.TrimDecimalNull(dr["Cost"]) != AppConst.DecimalNull)
                            CostReturnItem.Add("Cost", Util.TrimDecimalNull(dr["Cost"]));
                        else
                            CostReturnItem.Add("Cost", RMAReturnManager.GetInstance().GetReturnItemCost(returnItemSysNo));

                        new RMAReturnDac().UpdateReturnItem(CostReturnItem);
                    }
                }

                scope.Complete();
            }
        }

        public Hashtable GetReturnProduct(int sysno)
        {
            string sql = @"select Product.ProductID , Product.ProductName , RMA_Register.ProductSysNo
                           ,I.TargetProductSysNo,I.ReturnProductType,I.AuditStatus,I.AuditUserSysNo,I.AuditTime,I.AuditMemo
                           from   RMA_Return_Item I (NOLOCK)
                           inner  join RMA_Register (NOLOCK) on RMA_Register.SysNo = I.RegisterSysNo
                           inner  join Product (NOLOCK) on Product.SysNo = RMA_Register.ProductSysNo
                           where  I.SysNo=" + sysno;

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            Hashtable ht = new Hashtable();
            if (Util.HasMoreRow(ds))
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ht.Add("ProductSysNo", Util.TrimIntNull(dr["ProductSysNo"]));
                ht.Add("ProductID", dr["ProductID"].ToString());
                ht.Add("ProductName", dr["ProductName"].ToString());

                ht.Add("TargetProductSysNo", Util.TrimIntNull(dr["TargetProductSysNo"]));
                ht.Add("ReturnProductType", Util.TrimIntNull(dr["ReturnProductType"]));
                ht.Add("AuditStatus", Util.TrimIntNull(dr["AuditStatus"]));
                ht.Add("AuditUserSysNo", Util.TrimIntNull(dr["AuditUserSysNo"]));
                ht.Add("AuditTime", Util.TrimDateNull(dr["AuditTime"]));
                ht.Add("AuditMemo", Util.TrimNull(dr["AuditMemo"]));
            }
            return ht;
        }

        //退货入库，调其他产品的审核，同意或者拒绝
        public void AuditReturnTarget(Hashtable ht)
        {
            new RMAReturnDac().UpdateReturnTarget(ht);
        }

        public DataSet GetRegisterSysNo(int sysno)
        {
            string sql = "select registersysno from rma_return_item(nolock) where returnsysno=" + sysno + "and returnproducttype is not null order by returnsysno desc"; //调新品(二手),其他非case商品,退款,原物品入库.
            DataSet dsRegisterSysno = SqlHelper.ExecuteDataSet(sql);
            if (dsRegisterSysno != null)
            {
                return dsRegisterSysno;
            }
            else
            {
                return null;
            }
        }

        public void SetReturnTarget(Hashtable ht)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                if (ht.ContainsKey("ProductID") && ht.ContainsKey("ProductType"))
                {

                    if (Util.TrimIntNull(ht["ProductType"]) == AppConst.IntNull)
                    {
                        Hashtable htReturn = new Hashtable();
                        htReturn.Add("SysNo", Util.TrimIntNull(ht["SysNo"]));
                        htReturn.Add("TargetProductSysNo", AppConst.IntNull);
                        htReturn.Add("ReturnProductType", AppConst.IntNull);
                        htReturn.Add("AuditStatus", AppConst.IntNull);
                        htReturn.Add("AuditMemo", AppConst.StringNull);
                        htReturn.Add("AuditUserSysNo", AppConst.IntNull);
                        htReturn.Add("AuditTime", AppConst.DateTimeNull);
                        new RMAReturnDac().UpdateReturnTarget(htReturn);
                    }
                    else
                    {
                        string ProductID = AppConst.StringNull;
                        //根据ProductType生成对应的ProductID
                        if (Util.TrimIntNull(ht["ProductType"]) == (int)AppEnum.ProductType.Normal)
                        {
                            ProductID = GetInitialProductID(ht["ProductID"].ToString());
                        }
                        else if (Util.TrimIntNull(ht["ProductType"]) == (int)AppEnum.ProductType.SecondHand)
                        {
                            //ProductID = GetInitialProductID(ht["ProductID"].ToString());
                            //ProductID = ProductID + "R";
                            ProductID = ht["NewProductID"].ToString();   //退货入库时退为二手时产品id已经固定
                        }
                        else if (Util.TrimIntNull(ht["ProductType"]) == (int)AppEnum.ProductType.Bad)
                        {
                            ProductID = GetInitialProductID(ht["ProductID"].ToString());
                            ProductID = ProductID + "B";
                        }
                        else if (Util.TrimIntNull(ht["ProductType"]) == (int)AppEnum.ProductType.OtherProduct)
                        {
                            ProductID = ProductManager.GetInstance().LoadBasic((int)ht["TargetProductSysNo"]).ProductID;
                        }

                        ProductBasicInfo oProduct = new ProductBasicInfo();
                        oProduct = ProductManager.GetInstance().LoadBasic(ProductID);
                        if (oProduct == null) //没有相应商品,Clone 商品
                        {
                            ProductManager.GetInstance().ProductClone(ht["ProductID"].ToString(), ProductID, Util.TrimIntNull(ht["UserSysNo"]));
                            oProduct = ProductManager.GetInstance().LoadBasic(ProductID);
                        }
                        //else     //有相应商品，则更新相应TargetProductSyNo字段                        
                        Hashtable htSetTarget = new Hashtable();
                        htSetTarget.Add("SysNo", Util.TrimIntNull(ht["SysNo"]));
                        htSetTarget.Add("TargetProductSysNo", oProduct.SysNo);
                        htSetTarget.Add("ReturnProductType", (int)ht["ProductType"]);
                        if (Util.TrimIntNull(ht["ProductType"]) == (int)AppEnum.ProductType.OtherProduct)
                        {
                            htSetTarget.Add("AuditStatus", (int)AppEnum.TriStatus.Origin);
                        }
                        else
                        {
                            htSetTarget.Add("AuditStatus", AppConst.IntNull);
                        }

                        htSetTarget.Add("AuditMemo", AppConst.StringNull);
                        htSetTarget.Add("AuditUserSysNo", AppConst.IntNull);
                        htSetTarget.Add("AuditTime", AppConst.DateTimeNull);

                        new RMAReturnDac().UpdateReturnTarget(htSetTarget);

                    }
                }

                UpdateReturnItemCost(ht); //更新入库成本  

                scope.Complete();
            }
        }

        public RMAReturnInfo Load(int sysno)
        {
            RMAReturnInfo oReturn = new RMAReturnInfo();
            string sql = "select * from RMA_Return where sysno=" + sysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                DataRow dr = ds.Tables[0].Rows[0];
                MapReturn(oReturn, dr);
                string itemsql = @"select RMA_Register.* ,RMA_Return_Item.SysNo as ItemSysNo
					               from   RMA_Return_Item (NOLOCK)
                                   inner  join  RMA_Register (NOLOCK) on RMA_Register.SysNo = RMA_Return_Item.RegisterSysNo 
                                   where  RMA_Return_Item.ReturnSysNo = " + sysno;
                DataSet itemds = SqlHelper.ExecuteDataSet(itemsql);
                if (Util.HasMoreRow(itemds))
                {
                    foreach (DataRow itemdr in itemds.Tables[0].Rows)
                    {
                        RMARegisterInfo oRegister = new RMARegisterInfo();
                        RMARequestManager.GetInstance().mapRegister(oRegister, itemdr);
                        oReturn.ItemHash.Add(itemdr["ItemSysNo"], oRegister);
                    }
                }
            }
            return oReturn;
        }

        private void MapReturn(RMAReturnInfo oInfo, DataRow dr)
        {
            oInfo.CreateTime = Util.TrimDateNull(dr["CreateTime"]);
            oInfo.CreateUserSysNo = Util.TrimIntNull(dr["CreateUserSysNo"]);
            oInfo.Note = dr["Note"].ToString();
            oInfo.ReturnID = dr["ReturnID"].ToString();
            oInfo.ReturnTime = Util.TrimDateNull(dr["ReturnTime"]);
            oInfo.ReturnUserSysNo = Util.TrimIntNull(dr["ReturnUserSysNo"]);
            oInfo.Status = Util.TrimIntNull(dr["Status"]);
            oInfo.StockSysNo = Util.TrimIntNull(dr["StockSysNo"]);
            oInfo.SysNo = Util.TrimIntNull(dr["SysNo"]);
        }

        private void MapReturnItem(RMAReturnItemInfo info, DataRow row)
        {
            info.SysNo = Util.TrimIntNull(row["SysNo"]);
            info.ReturnSysNo = Util.TrimIntNull(row["ReturnSysNo"]);
            info.RegisterSysNo = Util.TrimIntNull(row["RegisterSysNo"]);
            info.TargetProductSysNo = Util.TrimIntNull(row["TargetProductSysNo"]);
            info.Cost = Util.TrimDecimalNull(row["Cost"]);
            info.ReturnProductType = Util.TrimIntNull(row["ReturnProductType"]);
            info.AuditStatus = Util.TrimIntNull(row["AuditStatus"]);
            info.AuditUserSysNo = Util.TrimIntNull(row["AuditUserSysNo"]);
            info.AuditTime = Util.TrimDateNull(row["AuditTime"]);
            info.AuditMemo = Util.TrimNull(row["AuditMemo"]);
        }

        public ProductBasicInfo LoadTargetbyRegisterSysNo(int sysno)
        {
            ProductBasicInfo oProduct = new ProductBasicInfo();
            string sql = "select TargetProductSysNo from RMA_Return_Item (NOLOCK)  where RegisterSysNo=" + sysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                DataRow dr = ds.Tables[0].Rows[0];
                oProduct = ProductManager.GetInstance().LoadBasic(Util.TrimIntNull(dr["TargetProductSysNo"]));
            }
            return oProduct;
        }

        public int GetCurrentStatus(int sysno)
        {
            string sql = "select status from rma_return (NOLOCK) where sysno = " + sysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
            {
                throw new BizException("Get Current Status Error!");
            }
            DataRow dr = ds.Tables[0].Rows[0];
            return Util.TrimIntNull(dr["status"]);
        }

        public decimal GetReturnItemCost(int sysno)
        {
            decimal cost;
            string sql = @"select vi.Cost , vi.Point
                           from  RMA_Return_Item (NOLOCK)
                           inner join RMA_Register (NOLOCK) on RMA_Register.SysNo = RMA_Return_Item.RegisterSysNo  
                           inner join RMA_Request_Item (NOLOCK) on RMA_Request_Item.RegisterSysNo = RMA_Register.SysNo
                           inner join RMA_Request (NOLOCK) on RMA_Request_Item.RequestSysNo = RMA_Request.SysNo  
                           inner join V_SO_Item vi (NOLOCK) on (vi.sosysno = RMA_Request.sosysno and vi.ProductSysNo = RMA_Register.ProductSysNo)
                           where RMA_Return_Item.SysNo = " + sysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            DataRow dr = ds.Tables[0].Rows[0];
            cost = Util.TrimDecimalNull(dr["Cost"]);
            if (cost == AppConst.IntNull)
            {
                throw new BizException("Calculate Cost Error!");
            }
            return cost;
        }
        /// <summary>
        /// destination:采用递归获取初始产品号
        /// </summary>
        /// <param name="strProductID"></param>
        /// <returns></returns> 
        public string GetInitialProductID(string strProductID)
        {
            //int length = strProductID.Length;
            //if ((strProductID.Substring(length - 1, 1).ToUpper() == "R") || (strProductID.Substring(length - 1, 1).ToUpper() == "B"))
            //{
            //    strProductID = strProductID.Substring(0, length - 1);
            //    GetInitialProductID(strProductID);
            //}
            return strProductID.Substring(0, 10);// productID 格式是2-3-3，因此，这里只要取前10位就可以了。
        }
        public DataSet GetProductBarcodeInfo(int ReturnItemSysNo)
        {
            string sql = @"select RMA_Return_Item.TargetProductSysNo,RMA_Register.ProductIDSysNo,RMA_Register.sysno as registersysno,Inventory_Stock.Position1,Inventory_Stock.Position1 from RMA_Return_Item 
                          left join RMA_Register on RMA_Return_Item.registersysno=RMA_Register.sysno
                          left join Inventory_Stock on RMA_Return_Item.TargetProductSysNo=Inventory_Stock.ProductSysNo
                          where RMA_Return_Item.sysno=" + ReturnItemSysNo;
            return SqlHelper.ExecuteDataSet(sql);

        }

        #region  Import

        //        public void ImportReturn()
        //        {
        //            string sqlExamReturn="select top 1 * from RMA_Return";
        //            DataSet dsExamReturn = SqlHelper.ExecuteDataSet(sqlExamReturn);
        //            if(Util.HasMoreRow(dsExamReturn))
        //                throw new BizException("The RMA_Return is not Empty!");
        //            string sqlExamReturnItem = "select top 1 * from RMA_Return_Item";
        //            DataSet dsExamReturnItem = SqlHelper.ExecuteDataSet(sqlExamReturnItem);
        //            if(Util.HasMoreRow(dsExamReturnItem))
        //                throw new BizException("The RMA_Return_Item is not Empty!");

        //            TransactionOptions options = new TransactionOptions();
        //            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
        //            options.Timeout = TransactionManager.DefaultTimeout;

        //            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
        //            {

        //                string sqlMaster = "select * from IPP3..RO_Master";
        //                DataSet dsMaster = SqlHelper.ExecuteDataSet(sqlMaster);
        //                foreach(DataRow drMaster in dsMaster.Tables[0].Rows)
        //                {
        //                    //如果RO数据中没有RMASysNo（IPP2003中的数据），则从IPP2003中查询相应的数据，生成Request 和 register
        //                    if(drMaster["RMASysNo"].ToString() == AppConst.StringNull)
        //                    {
        //                        string sqlLoadRequest = @"select ipp3..so_master.sysno, ipp3..so_master.customersysno , ipp3..so_master.ReceiveAddress , ipp3..so_master.ReceiveContact,
        //                                                      ipp3..so_master.ReceivePhone 
        //                                               from  ipp3..ro_master  
        //                                               inner join ipp2003..ut_master on ipp3..ro_master.roid = ipp2003..ut_master.utid
        //                                               inner join ipp3..so_master on ipp2003..ut_master.sosysno = ipp3..so_master.sysno
        //                                               where ipp3..ro_master.sysno=" + drMaster["SysNo"].ToString();

        //                        DataSet dsLoadRequest = SqlHelper.ExecuteDataSet(sqlLoadRequest);                                                                      
        //                        if(Util.HasMoreRow(dsLoadRequest))
        //                        {
        //                             DataRow drLoadRequest = dsLoadRequest.Tables[0].Rows[0];
        //                            RMARequestInfo oRequest = new RMARequestInfo();
        //                            oRequest.SysNo = SequenceDac.GetInstance().Create("RMA_Request_Sequence");
        //                            oRequest.RequestID = RMARequestManager.GetInstance().BuildRMAID(oRequest.SysNo);
        //                            if(drLoadRequest["ReceiveAddress"].ToString() != AppConst.StringNull)
        //                                oRequest.Address = drLoadRequest["ReceiveAddress"].ToString();
        //                            else
        //                                oRequest.Address = "无";
        //                            if(drLoadRequest["ReceiveContact"].ToString() != AppConst.StringNull)
        //                                oRequest.Contact = drLoadRequest["ReceiveContact"].ToString();
        //                            else
        //                                oRequest.Contact = "无";
        //                            if(drLoadRequest["ReceivePhone"].ToString() != AppConst.StringNull)
        //                                oRequest.Phone = drLoadRequest["ReceivePhone"].ToString();
        //                            else
        //                                oRequest.Phone = "无";
        //                            oRequest.CreateTime = Util.TrimDateNull(drMaster["CreateTime"]);
        //                            oRequest.CustomerSysNo = Util.TrimIntNull(drLoadRequest["customersysno"]);
        //                            oRequest.Memo = "导入数据时系统自动生成！";
        //                            oRequest.Note = drMaster["Note"].ToString();							
        //                            oRequest.SOSysNo = Util.TrimIntNull(drLoadRequest["sysno"]);							
        //                            oRequest.Status = (int)AppEnum.RMARequestStatus.Closed;
        //                            RMARequestManager.GetInstance().InsertRequest(oRequest);

        //                            string sqlLoadRegister = "select * from RO_Item where ROSysNo = " + drMaster["SysNo"].ToString();
        //                            DataSet dsLoadRegister = SqlHelper.ExecuteDataSet(sqlLoadRegister);
        //                            if(Util.HasMoreRow(dsLoadRegister))
        //                            {							    
        //                                foreach(DataRow drLoadRegister in dsLoadRegister.Tables[0].Rows)
        //                                {
        //                                    for(int i=1 ; i<= Util.TrimIntNull(drLoadRegister["Quantity"]); i++)
        //                                    {
        //                                        RMARegisterInfo oRegister = new RMARegisterInfo();	
        //                                        oRegister.SysNo = SequenceDac.GetInstance().Create("RMA_Register_Sequence");
        //                                        oRegister.ProductSysNo = Util.TrimIntNull(drLoadRegister["ProductSysNo"]);
        //                                        oRegister.RequestType = (int)AppEnum.RMARequestType.Return;
        //                                        oRegister.CustomerDesc = "无";
        //                                        oRegister.CheckTime = Util.TrimDateNull(drMaster["AuditTime"]);
        //                                        oRegister.Memo = "系统自动生成！";
        //                                        oRegister.Status = oRequest.Status;
        //                                        RMARegisterManager.GetInstance().InsertRegister(oRegister);

        //                                        RMARequestItemInfo oRequestItem = new RMARequestItemInfo();
        //                                        oRequestItem.RequestSysNo = oRequest.SysNo;
        //                                        oRequestItem.RegisterSysNo = oRegister.SysNo;
        //                                        RMARequestManager.GetInstance().InsertRequestItem(oRequestItem);

        //                                        RMAConvertInfo oRMAConvert = new RMAConvertInfo();
        //                                        //对于这些没有相关RMASysNo信息的RO记录，根据ROSysNo和ProductSysNo确定Register
        //                                        oRMAConvert.RMASysNo = Util.TrimIntNull(drMaster["SysNo"]);
        //                                        oRMAConvert.ProductSysNo = Util.TrimIntNull(drLoadRegister["ProductSysNo"]);
        //                                        oRMAConvert.RegisterSysNo = oRegister.SysNo;
        //                                        new RMAConvertDac().Insert(oRMAConvert);
        //                                    }								

        //                                }
        //                            }
        //                        }

        //                    }
        //                    RMAReturnInfo oReturn = new RMAReturnInfo();
        //                    try
        //                    {						
        //                        oReturn.SysNo = SequenceDac.GetInstance().Create("RMA_Return_Sequence");
        //                        oReturn.ReturnID = getRevertID(oReturn.SysNo);
        //                        oReturn.StockSysNo = Util.TrimIntNull(drMaster["StockSysNo"]);
        //                        oReturn.CreateTime = Util.TrimDateNull(drMaster["CreateTime"]);
        //                        oReturn.CreateUserSysNo = Util.TrimIntNull(drMaster["CreateUserSysNo"]);
        //                        oReturn.ReturnTime = Util.TrimDateNull(drMaster["ReturnTime"]);
        //                        oReturn.ReturnUserSysNo = Util.TrimIntNull(drMaster["ReturnUserSysNo"]);
        //                        oReturn.Note = drMaster["Note"].ToString();
        //                        if(Util.TrimIntNull(drMaster["Status"]) == -1)
        //                            oReturn.Status = -1;
        //                        else if(Util.TrimIntNull(drMaster["Status"]) == 2)
        //                            oReturn.Status = 1 ;
        //                        else
        //                            oReturn.Status = 0;
        //                        this.InsertReturn(oReturn);
        //                    }			    
        //                    catch
        //                    {
        //                        throw new BizException("Import RMA_Return Error! RO_Master.SysNo=" + drMaster["SysNo"].ToString());
        //                    }
        //                    string sqlItem = "select * from IPP3..RO_Item where ROSysNo =" + drMaster["SysNo"].ToString();
        //                    DataSet dsItem = SqlHelper.ExecuteDataSet(sqlItem);
        //                    foreach(DataRow drItem in dsItem.Tables[0].Rows)
        //                    {	
        //                        try
        //                        {
        //                            string RMASysNo = drMaster["RMASysNo"].ToString() ;
        //                            if(drMaster["RMASysNo"].ToString() == AppConst.StringNull)
        //                                RMASysNo = drMaster["SysNo"].ToString();
        //                            string sqlRegister = "select RegisterSysNo from ippconvert..RMA_Import where RMASysNo = " + RMASysNo + " and ProductSysNo=" + drItem["ProductSysNo"].ToString();
        //                            DataSet dsRegister = SqlHelper.ExecuteDataSet(sqlRegister);
        //                            if(Util.HasMoreRow(dsRegister))
        //                            {
        //                                foreach(DataRow drRegister in dsRegister.Tables[0].Rows)
        //                                {
        //                                    RMAReturnItemInfo oReturnItem = new RMAReturnItemInfo();
        //                                    oReturnItem.ReturnSysNo = oReturn.SysNo;
        //                                    oReturnItem.RegisterSysNo = Util.TrimIntNull(drRegister["RegisterSysNo"]);
        //                                    oReturnItem.TargetProductSysNo = Util.TrimIntNull(drItem["ReturnSysNo"]);
        //                                    oReturnItem.Cost = Util.TrimDecimalNull(drItem["Cost"]);
        //                                    this.InsertReturnItem(oReturnItem);

        //                                    //更新相应的Register中的ReturnStatus
        //                                    Hashtable htRegister = new Hashtable();
        //                                    htRegister.Add("SysNo" , Util.TrimIntNull(drRegister["RegisterSysNo"]));
        //                                    htRegister.Add("ReturnStatus" , oReturn.Status);
        //                                    RMARegisterManager.GetInstance().UpdateRegister(htRegister);
        //                                }					       
        //                            }

        //                        }	
        //                        catch
        //                        {
        //                            throw new BizException("Import ReturnItem Error! RO_Item.SysNo=" + drItem["SysNo"].ToString());
        //                        }						
        //                    }				

        //                }


        //                scope.Complete();
        //            }		
        //        }

        #endregion

    }
}