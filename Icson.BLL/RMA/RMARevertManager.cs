using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Text;


using System.Transactions;
using Icson.Utils;
using Icson.DBAccess;

using Icson.Objects;
using Icson.DBAccess.RMA;
using Icson.Objects.RMA;

using Icson.BLL.Basic;
using Icson.Objects.Basic;
using Icson.BLL.Online;
using Icson.BLL.Finance;


namespace Icson.BLL.RMA
{
    /// <summary>
    /// Summary description for RMARevertManager.
    /// </summary>
    public class RMARevertManager
    {
        private RMARevertManager()
        {

        }
        private static RMARevertManager _instance;
        public static RMARevertManager GetInstance()
        {
            if (_instance == null)
                _instance = new RMARevertManager();
            return _instance;
        }

        public void InsertRevert(RMARevertInfo oInfo)
        {
            new RMARevertDac().InsertRevert(oInfo);
        }

        public void UpdateRevertCheckInfo(Hashtable paramHash)
        {
            new RMARevertDac().UpdateRevert(paramHash);
        }

        public void UpdateRevert(Hashtable ht)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                //如果状态Status改变，改变Register中相应的Status
                if (ht.ContainsKey("Status"))
                {
                    string sql = "select RegisterSysNo from RMA_Revert_Item (NOLOCK) where RevertSysNo = " + ht["SysNo"].ToString();
                    DataSet ds = SqlHelper.ExecuteDataSet(sql);
                    if (Util.HasMoreRow(ds))
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            Hashtable htRegister = new Hashtable();
                            htRegister.Add("SysNo", Util.TrimIntNull(dr["RegisterSysNo"]));
                            htRegister.Add("RevertStatus", Util.TrimIntNull(ht["Status"]));
                            RMARegisterManager.GetInstance().UpdateRegister(htRegister);
                        }
                    }
                }

                new RMARevertDac().UpdateRevert(ht);


                //如果Status是取消，对于调新品或者二手或者其他商品，对应商品所占库存应该恢复
                if (ht.ContainsKey("Status"))
                {
                    int status = Util.TrimIntNull(ht["Status"]);
                    if (status == (int)AppEnum.RMARevertStatus.Abandon)
                    {
                        string sql = @"select RMA_Revert_Item.StockSysNo , RMA_Register.ProductSysNo
                               ,RMA_Register.NewProductStatus,RMA_Register.RevertProductSysNo
                               from   RMA_Revert_Item (NOLOCK)
                               inner  join  RMA_Register (NOLOCK) on RMA_Register.SysNo = RMA_Revert_Item.RegisterSysNo
                               where  RMA_Revert_Item.StockSysNo is not null and RMA_Revert_Item.RevertSysNo =  " + ht["SysNo"].ToString();

                        DataSet ds = SqlHelper.ExecuteDataSet(sql);
                        int newProductStatusValue = (int)AppEnum.NewProductStatus.Origin;
                        int newProductSysNo = 0;
                        int stockSysNo = 0;

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            newProductStatusValue = Util.TrimIntNull(dr["NewProductStatus"]);
                            newProductSysNo = (int)dr["RevertProductSysNo"];
                            stockSysNo = (int)dr["StockSysNo"];
                            //if (newProductStatusValue == (int)AppEnum.NewProductStatus.SecondHand)
                            //{
                            //    newProductSysNo = RMARegisterManager.GetInstance().getSecondHandProductSysNo(newProductSysNo);
                            //}
                            //仅新品、二手、其他会产生占用库存，而原物退回不会占库存。
                            if (newProductStatusValue != (int)AppEnum.NewProductStatus.Origin)
                            {
                                InventoryManager.GetInstance().SetAvailableQty(stockSysNo, newProductSysNo, -1);
                            }
                        }
                    }
                }
                scope.Complete();
            }
        }

        /// <summary>
        /// 更新单件处理中心发出的商品序列号
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public int UpdateRevertRegister(Hashtable ht)
        {
            string sql = "";
            foreach (string key in ht.Keys)
            {
                sql += "update rma_register set newProductIDSysNo='" + ht[key].ToString() + "' where sysno=" + key + ";";
            }

            return SqlHelper.ExecuteNonQuery(sql);
        }

        public void InsertRevertItem(RMARevertItemInfo oInfo)
        {
            new RMARevertDac().InsertRevertItem(oInfo);
        }
        /// <summary>
        /// RMA Revert运送单输入时则发Mail给Customer
        /// </summary>
        /// <returns></returns>
        public void SendMail(Hashtable ht)
        {
            //get mail information
            string mailaddress;
            string mailsubject;
            string mailbodyinfo;

            RMARevertInfo revertInfo = Load(Util.TrimIntNull(ht["SysNo"]));
            string strAllProduct = null;
            string strProductInfo = null;

            string strPackageID = ht["PackageID"].ToString();

            CustomerInfo oCustomer = new CustomerInfo();

            oCustomer = CustomerManager.GetInstance().Load(revertInfo.CustomerSysNo);

            if (Util.TrimNull(oCustomer.Email) == AppConst.StringNull)
                return;

            mailaddress = oCustomer.Email.ToString();

            mailsubject = @"您申请的返修物品已经返还";
            mailbodyinfo = @"<span style='font-size:12.0pt;font-family:方正姚体'>尊敬的ORS商城网用户，
                            <br>
                            您好！
                            <br>
                            您在ORS商城网的申请的如下返修物品：<br>
                            @allProduct
                            已经处理完成，ORS商城网安排了申通快递帮您配送您的返还件。申通快递的运单号是@PackageID，您可在申通快递的网站<a href='http://www.sto.cn' target='_blank'>www.sto.cn</a>查询返还件的运输情况。您还可致电申通公司021-39206666获取信息。如有需要，可联系ORS商城网客服热线400-820-1878。 
                            <br>
                            感谢您在ORS商城网购物，谢谢支持！
                            <br>
                            中国ORS商城网</span>";

            DataSet dsProductInfo = RMARevertManager.GetInstance().GetProductByRevert(Util.TrimIntNull(ht["SysNo"]));

            foreach (DataRow row in dsProductInfo.Tables[0].Rows)
            {
                strProductInfo += "产品号:" + row["ProductID"].ToString() + "(" + row["ProductName"].ToString() + ")" + "<br>";
            }

            strAllProduct = strProductInfo;

            mailbodyinfo = mailbodyinfo.Replace("@allProduct", strAllProduct);
            mailbodyinfo = mailbodyinfo.Replace("@PackageID", strPackageID);

            //send mail
            EmailInfo oMail = new EmailInfo();
            oMail.MailAddress = mailaddress;
            oMail.MailSubject = mailsubject;

            oMail.MailBody = mailbodyinfo;
            oMail.Status = (int)AppEnum.TriStatus.Origin;

            EmailManager.GetInstance().InsertEmail(oMail);
        }
        public DataSet GetWaitingRevert()
        {
            Hashtable ht = new Hashtable();
            return GetWaitingRevert(ht);
        }

        public string GetMemoFromRMARequest(Hashtable ht)
        {
            string registSysNoCollection = "";
            int index = 0;
            foreach (int registSysNo in ht.Keys)
            {
                if (index != 0)
                    registSysNoCollection += ",";
                registSysNoCollection += registSysNo.ToString();
                index++;
            }

            if (registSysNoCollection == "")
                return "";

            string sql = @"select memo 
                            from 
	                            rma_request (NOLOCK) left join rma_request_item (NOLOCK) on rma_request.sysno = rma_request_item.requestsysno
                            where
	                            rma_request_item.registersysno in (@regS)";

            sql = sql.Replace("@regS", registSysNoCollection);
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return "";

            string memo = "";
            index = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Util.TrimNull(dr["memo"]) == "")
                    continue;

                if (index != 0)
                    memo += ",";

                memo += Util.TrimNull(dr["memo"]);
                index++;
            }
            return memo;
        }

        public DataSet GetShipTypeName(int shipTypeSysNo)
        {
            string sql = @"select * from ShipType (NOLOCK) where SysNo = " + shipTypeSysNo + "";
            return SqlHelper.ExecuteDataSet(sql);
        }


         
        /// <summary>
        /// for revert getting note value from request table
        /// added parameter as out
        /// 如果同一个订单有两个维修申请，每个申请有不同的地址信息，在发货的时候默认用最近的那个地址和地址区域。
        /// </summary>
        /// <param name="paramHash"></param>
        /// <returns></returns>
        public string GetNoteFromRMARequest(Hashtable ht, ref string address, ref int areaSysNo)
        {
            string registSysNoCollection = "";
            int index = 0;
            foreach (int registSysNo in ht.Keys)
            {
                if (index != 0)
                    registSysNoCollection += ",";
                registSysNoCollection += registSysNo.ToString();
                index++;
            }

            if (registSysNoCollection == "")
                return "";

            string sql = @"select Note, Address,AreaSysNo
                            from 
	                            rma_request (NOLOCK) left join rma_request_item (NOLOCK) on rma_request.sysno = rma_request_item.requestsysno
                            where
	                            rma_request_item.registersysno in (@regS)";

            sql = sql.Replace("@regS", registSysNoCollection);
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return "";

            string Note = "";
            index = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (address == "" && Util.TrimNull(dr["Address"]) != AppConst.StringNull) address = Util.TrimNull(dr["Address"]);
                if (areaSysNo == AppConst.IntNull && Util.TrimIntNull(dr["AreaSysNo"]) != AppConst.IntNull) areaSysNo = Util.TrimIntNull(dr["AreaSysNo"]);

                if (Util.TrimNull(dr["Note"]) == "")
                    continue;

                if (index != 0)
                    Note += ",";

                Note += Util.TrimNull(dr["Note"]);
                index++;
            }
            return Note;
        }

        public DataSet GetWaitingRevert(Hashtable paramHash)
        {
            string sql = @"select RMA_Register.sysno as registersysno , Product.productname , RequestType , CustomerDesc ,
                                  RMA_Request.contact , RMA_Request.address ,RMA_Request.Zip , RMA_Request.Phone , RMA_Register.NewProductStatus
                           from   RMA_Register (NOLOCK)
                           left  join RMA_Request_Item (NOLOCK) on RMA_Request_Item.RegisterSysNo = RMA_Register.sysno
                           left  join RMA_Request (NOLOCK) on RMA_Request.sysno = RMA_Request_Item.RequestSysNo 
                           left  join Product (NOLOCK) on Product.sysno = RMA_Register.productsysno 
                           left  join V_SO_Master SO_Master (NOLOCK) on SO_Master.sysno = RMA_Request.SOSysNo
                           where RMA_Register.RevertStatus = @RevertStatus
                           and RMA_Register.sysno not in 
                              (select  RMA_Revert_Item.registerSysNo 
                                from   RMA_Revert_Item (NOLOCK)
                                inner  join RMA_Revert (NOLOCK) on RMA_Revert_Item.RevertSysNo = RMA_Revert.sysno
                                where  RMA_Revert.status = @RevertStatus )
                           @SOSysNo @RequestSysNo @ReceiveType @sqlHost";
            string sqlHost = @"and not exists
				            (select   RMA_Request.sysno  from
                                          RMA_Request_Item (NOLOCK) 
		                                  left join RMA_Register(nolock) on RMA_Register.sysno=RMA_Request_Item.RegisterSysNo
                                          where ( isnull (RMA_Register.RevertStatus," + AppConst.IntNull + " )<>" + (int)AppEnum.RMARevertStatus.WaitingRevert + "and isnull (RMA_Register.RefundStatus," + AppConst.IntNull + " )<>" + (int)AppEnum.RMARefundStatus.Refunded + ") and RMA_Request_Item.RequestSysNo = RMA_Request.sysno)";


           
//            string sqlHost = @"and not exists
//				            (select   RMA_Request.sysno  from
//                                          RMA_Request_Item (NOLOCK) 
//		                                  left join RMA_Register(nolock) on RMA_Register.sysno=RMA_Request_Item.RegisterSysNo
//                                          where  isnull (RMA_Register.RevertStatus,"+AppConst.IntNull+" )<>" + (int)AppEnum.RMARevertStatus.WaitingRevert + "and  RMA_Request_Item.RequestSysNo = RMA_Request.sysno)";
            sql = sql.Replace("@RevertStatus", ((int)AppEnum.RMARevertStatus.WaitingRevert).ToString());

            if (paramHash.ContainsKey("SOSysNo"))
                sql = sql.Replace("@SOSysNo", "  and RMA_Request.SOSysNo = " + Util.ToSqlString(paramHash["SOSysNo"].ToString()));
            else
                sql = sql.Replace("@SOSysNo", "");

            if (paramHash.ContainsKey("RequestSysNo"))
                sql = sql.Replace("@RequestSysNo", "  and RMA_Request.SysNo = " + Util.ToSqlString(paramHash["RequestSysNo"].ToString()));
            else
                sql = sql.Replace("@RequestSysNo", "");
          
            if (Util.TrimIntNull(paramHash["RMAReceiveType"]) == (int)AppEnum.RMARecieveType.Host)
            {
                sql = sql.Replace("@ReceiveType", "and RMA_Request.ReceiveType=" + (int)AppEnum.RMARecieveType.Host);
                sql = sql.Replace("@sqlHost", sqlHost);
            }
            else  //除了整机的，剩下都是单件，之前的单件ReceiveType为null
            {
                sql = sql.Replace("@ReceiveType", "and isnull(RMA_Request.ReceiveType," + AppConst.IntNull + ")<>" + (int)AppEnum.RMARecieveType.Host);
                sql = sql.Replace("@sqlHost", "");
            }

            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetRevertItem(int sysno)
        {
            string sql = @"select RMA_Register.* , Product.ProductName ,p.ProductName as RevertProductName, Stock.StockName,RMA_Revert_Item.SysNo as RevertItemSysNo, product.ProductID ,Product.BriefName ,p.productID,p.BriefName,
                                  RMA_Revert.SOSysNo,RMA_Revert_Item.Cost ,Product.SysNo as ProductSysNo,RMA_Revert_Item.StockSysNo as StockSysNo,Po_Master.VendorSysNo as VendorSysNo
                            from   RMA_Register (NOLOCK) 
                            inner  join  RMA_Revert_Item (NOLOCK) on RMA_Register.SysNo = RMA_Revert_Item.RegisterSysNo
                            inner  join  RMA_Revert (NOLOCK) on RMA_Revert.SysNo = RMA_Revert_Item.RevertSysNo                            
                            inner  join  Product (NOLOCK) on Product.SysNo = RMA_Register.ProductSysNo
                            inner  join  Product p(NOLOCK)  on p.SysNo = RMA_Register.RevertProductSysNo
                            left   join  Stock (NOLOCK) on Stock.SysNo = RMA_Revert_Item.StockSysNo
                            left   join Product_ID on Product_ID.SysNo=RMA_Register.productidsysno
                            left   join Po_Master on Po_Master.sysno=Product_ID.POSysNo
                            where  RMA_Revert_Item.RevertSysNo = " + sysno; 

            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetProductByRevert(int revertSysNo)
        {
            string sql = @"select RMA_Register.* , Product.ProductID,Product.ProductName,Product.Sysno as ProductSysno
                           from RMA_Register (NOLOCK) 
                           inner join RMA_Revert_Item (NOLOCK)  on RMA_Register.SysNo = RMA_Revert_Item.RegisterSysNo
                           inner join Product (NOLOCK)  on Product.SysNo = RMA_Register.ProductSysNo
                           where RMA_Revert_Item.RevertSysNo=" + revertSysNo + "";
            return SqlHelper.ExecuteDataSet(sql);
        }
        public DataSet RevertSOList()
        {
            //增加SOID，由原来按照CustomerName生成Revert单改为由SOID来生成（显示为SOID，实际上还是按照Customer生成送货地址等信息）           
            string sql = @"select  distinct SO_Master.SOID, RMA_Request.sosysno
                            from  RMA_Register (NOLOCK)
                            left  join RMA_Request_Item (NOLOCK) on RMA_Register.sysno = RMA_Request_Item.RegisterSysNo
                            left  join RMA_Request (NOLOCK) on RMA_Request.sysno = RMA_Request_Item.RequestSysNo
                            left  join V_SO_Master SO_Master (NOLOCK) on RMA_Request.sosysno = SO_Master.SysNo                 
                            where RMA_Register.RevertStatus = @RevertStatus
                             and  RMA_Register.sysno not in
                             (select RMA_Revert_Item.registerSysNo
                              from   RMA_Revert_Item (NOLOCK)
                              inner join RMA_Revert (NOLOCK) on RMA_Revert_Item.RevertSysNo = RMA_Revert.sysno 
                              where  RMA_Revert.status = @RevertStatus ) ";

            sql = sql.Replace("@RevertStatus", ((int)AppEnum.RMARevertStatus.WaitingRevert).ToString());

            return SqlHelper.ExecuteDataSet(sql);
        }

        public int Create(RMARevertInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                oParam.SysNo = SequenceDac.GetInstance().Create("RMA_Revert_Sequence");
                oParam.RevertID = getRevertID(oParam.SysNo);

                new RMARevertDac().InsertRevert(oParam);
                foreach (int registerSysNo in oParam.ItemHash.Keys)
                {
                    RMARevertItemInfo oRevertItem = new RMARevertItemInfo();
                    oRevertItem.RegisterSysNo = registerSysNo;
                    oRevertItem.RevertSysNo = oParam.SysNo;
                    //把Register仓库号写入RMA_Revert_Item
                    int revertStockSysNo = AppConst.IntNull;
                    DataRow regdr =  RMARegisterManager.GetInstance().GetRegisterRow(registerSysNo);
                    revertStockSysNo = Util.TrimIntNull(regdr["RevertStockSysNo"]);
                    
                    oRevertItem.StockSysNo = Util.TrimIntNull(revertStockSysNo);

                    this.InsertRevertItem(oRevertItem);
                }

                scope.Complete();
                return oParam.SysNo;
            }
        }
        public int AddItem(RMARevertInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                foreach (int registerSysNo in oParam.ItemHash.Keys)
                {
                    RMARevertItemInfo oRevertItem = new RMARevertItemInfo();
                    oRevertItem.RegisterSysNo = registerSysNo;
                    oRevertItem.RevertSysNo = oParam.SysNo;
                    //把Register仓库号写入RMA_Revert_Item
                    int revertStockSysNo = AppConst.IntNull;
                    DataRow regdr = RMARegisterManager.GetInstance().GetRegisterRow(registerSysNo);
                    revertStockSysNo = Util.TrimIntNull(regdr["RevertStockSysNo"]);

                    oRevertItem.StockSysNo = Util.TrimIntNull(revertStockSysNo);

                    this.InsertRevertItem(oRevertItem);
                }

                scope.Complete();
                return oParam.SysNo;
            }
        }

        private string getRevertID(int sysno)
        {
            return "R2" + sysno.ToString().PadLeft(8, '0');
        }

        public DataSet GetRevertList(Hashtable paramHash)
        {
            string sql = @"select RMA_Revert.* ,customer.customername , createuser.username as createusername , 
                                  outuser.username as outusername ,so_master.sysno , so_master.soid ,                                  
                                  customer.VIPRank ,pt.isPayWhenRecv 
                           from   RMA_Revert (NOLOCK)
                           left   join  v_so_master so_master (NOLOCK) on so_master.sysno = rma_revert.sosysno
                           left   join  customer (NOLOCK) on customer.sysno = so_master.customersysno                           
                           left   join  Sys_User as createuser (NOLOCK) on createuser.sysno = RMA_Revert.CreateUserSysNo
                           left   join  Sys_User as outuser (NOLOCK) on outuser.sysno = RMA_Revert.OutUserSysNo
                           left   join paytype pt (NOLOCK) on pt.sysno = so_master.paytypesysno
                           where   1=1  @SysNo @DateFrom  @DateTo  @RevertID @ShipType @NewProductStatus @IsWithin7days 
                                  @SOSysNo @LocationStatus @IsVIP @RevertStatus @RegisterSysNo @ProductID @IsConfirmAddress @OutDateFrom @OutDateTo @IsOutCheck
                                   order by RMA_Revert.sysno desc ";

            if (paramHash.ContainsKey("SysNo"))
                sql = sql.Replace("@SysNo", " and RMA_Revert.SysNo =" + paramHash["SysNo"].ToString());
            else
                sql = sql.Replace("@SysNo", "");

            if (paramHash.ContainsKey("DateFrom"))
                sql = sql.Replace("@DateFrom", " and RMA_Revert.CreateTime >=" + Util.ToSqlString(paramHash["DateFrom"].ToString()));
            else
                sql = sql.Replace("@DateFrom", "");

            if (paramHash.ContainsKey("DateTo"))
                sql = sql.Replace("@DateTo", " and RMA_Revert.CreateTime <=" + Util.ToSqlEndDate(paramHash["DateTo"].ToString()));
            else
                sql = sql.Replace("@DateTo", "");

            if (paramHash.ContainsKey("RevertID"))
                sql = sql.Replace("@RevertID", " and RMA_Revert.RevertID like '%" + paramHash["RevertID"].ToString() + "%'");
            else
                sql = sql.Replace("@RevertID", "");

            if (paramHash.ContainsKey("ShipType"))
                sql = sql.Replace("@ShipType", " and RMA_Revert.ShipType = " + paramHash["ShipType"].ToString());
            else
                sql = sql.Replace("@ShipType", "");

            if (paramHash.ContainsKey("NewProductStatus"))
                sql = sql.Replace("@NewProductStatus", " and exists (select  rma_register.sysno from rma_revert_item  , rma_register  where  rma_revert_item.revertsysno = rma_revert.sysno and rma_revert_item.registersysno = rma_register.sysno and rma_register.newproductstatus = " + Util.TrimNull(paramHash["NewProductStatus"]) + ")");
            else
                sql = sql.Replace("@NewProductStatus", "");

            if (paramHash.ContainsKey("IsWithin7days"))
                sql = sql.Replace("@IsWithin7days", " and exists (select  rma_register.sysno from rma_revert_item  , rma_register  where  rma_revert_item.revertsysno = rma_revert.sysno and rma_revert_item.registersysno = rma_register.sysno and rma_register.IsWithin7days = " + Util.TrimNull(paramHash["IsWithin7days"]) + ")");
            else
                sql = sql.Replace("@IsWithin7days", "");

            if (paramHash.ContainsKey("SOSysNo"))
                sql = sql.Replace("@SOSysNo", " and rma_revert.sosysno=" + paramHash["SOSysNo"].ToString());
            else
                sql = sql.Replace("@SOSysNo", "");

            if (paramHash.ContainsKey("LocationStatus"))
                sql = sql.Replace("@LocationStatus", " and rma_revert.LocationStatus =" + paramHash["LocationStatus"].ToString());
            else
                sql = sql.Replace("@LocationStatus", "");

            if (paramHash.ContainsKey("IsVIP"))
            {
                if (Util.TrimIntNull(paramHash["IsVIP"]) == (int)AppEnum.YNStatus.Yes)
                {
                    sql = sql.Replace("@IsVIP", "and (customer.VIPRank=" + (int)AppEnum.CustomerVIPRank.AutoVIP + " or customer.VIPRank=" + (int)AppEnum.CustomerVIPRank.ManualVIP + ") ");
                }
                else
                {
                    sql = sql.Replace("@IsVIP", "and (customer.VIPRank=" + (int)AppEnum.CustomerVIPRank.AutoNonVIP + " or customer.VIPRank=" + (int)AppEnum.CustomerVIPRank.ManualNonVIP + ") ");
                }
            }
            else
            {
                sql = sql.Replace("@IsVIP", "");
            }

            if (paramHash.ContainsKey("RevertStatus"))
            {
                sql = sql.Replace("@RevertStatus", " and rma_revert.status=" + paramHash["RevertStatus"].ToString());
            }
            else
            {
                sql = sql.Replace("@RevertStatus", "");
            }

            if (paramHash.ContainsKey("RegisterSysNo"))
            {
                sql = sql.Replace("@RegisterSysNo", " and exists(select rma_revert_item.registersysno from rma_revert,rma_revert_item where rma_revert_item.revertsysno=rma_revert.sysno and rma_revert_item.registersysno = " + Util.TrimNull(paramHash["RegisterSysNo"]) + ")");
            }
            else
            {
                sql = sql.Replace("@RegisterSysNo","");
            }

            if (paramHash.ContainsKey("ProductID"))
            {
                sql = sql.Replace("@ProductID"," and exists(select product.productid from rma_revert,rma_revert_item,rma_register,product where rma_revert.sysno=rma_revert_item.revertsysno and rma_revert_item.registersysno=rma_register.sysno and rma_register.productsysno=product.sysno and product.productid = " + Util.ToSqlString(Util.TrimNull(paramHash["ProductID"])) + ")");
            }
            else
            {
                sql = sql.Replace("@ProductID","");
            }

            if (paramHash.ContainsKey("IsConfirmAddress"))
            {
                sql = sql.Replace("@IsConfirmAddress", " and rma_revert.IsConfirmAddress=" + paramHash["IsConfirmAddress"].ToString());
            }
            else 
            {
                sql = sql.Replace("@IsConfirmAddress", "");
            }
            if (paramHash.ContainsKey("IsOutCheck"))
            {
                if (Util.TrimIntNull(paramHash["IsOutCheck"])==(int)AppEnum.YNStatus.Yes)
                    sql = sql.Replace("@IsOutCheck", " and rma_revert.CheckQtyTime is not null");
                else
                    sql = sql.Replace("@IsOutCheck", " and rma_revert.CheckQtyTime is null");
            }
            else
            {
                sql = sql.Replace("@IsOutCheck", "");
            }
            if (paramHash.ContainsKey("OutDateFrom"))
            {
                sql = sql.Replace("@OutDateFrom", " and rma_revert.OutTime>=" +Util.ToSqlString(paramHash["OutDateFrom"].ToString()));
            }
            else
            {
                sql = sql.Replace("@OutDateFrom", "");
            }
            if (paramHash.ContainsKey("OutDateTo"))
            {
                sql = sql.Replace("@OutDateTo", " and rma_revert.OutTime<=" +Util.ToSqlEndDate(paramHash["OutDateFrom"].ToString()));
            }
            else
            {
                sql = sql.Replace("@OutDateTo", "");
            }

            if (paramHash == null || paramHash.Count == 0)
                sql = sql.Replace("select", "select top 50");

            return SqlHelper.ExecuteDataSet(sql);
        }

        public void RevertCancelOut(int sysno)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;
            int newProductStatusValue = (int)AppEnum.NewProductStatus.Origin;
            int newProductSysNo = 0;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                Hashtable ht = new Hashtable();
                ht.Add("SysNo", sysno);
                ht.Add("Status", (int)AppEnum.RMARevertStatus.WaitingRevert);

                this.UpdateRevert(ht);
                new RMARevertDac().RevertCancelOut(sysno);

                string sql = @"select RMA_Revert_Item.StockSysNo , RMA_Register.ProductSysNo ,RMA_Revert_Item.SysNo as revetItemSysNo ,RMA_Register.SysNo as RegisterSysNo
                               ,RMA_Register.NewProductStatus,RMA_Register.RevertProductSysNo,RMA_Revert_Item.Cost,RMA_Register.Status
                               from   RMA_Revert_Item (NOLOCK)
                               inner  join  RMA_Register (NOLOCK) on RMA_Register.SysNo = RMA_Revert_Item.RegisterSysNo
                               where  RMA_Revert_Item.StockSysNo is not null and RMA_Revert_Item.RevertSysNo =  " + sysno.ToString();

                DataSet ds = SqlHelper.ExecuteDataSet(sql);

                if (Util.HasMoreRow(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (Util.TrimIntNull(dr["Status"]) != (int)AppEnum.RMARequestStatus.Handling)
                        {
                            throw new BizException("单件状态不是处理中，无法进行取消出库发货");
                        }

                        Hashtable CostRevertItem = new Hashtable();

                        CostRevertItem.Add("SysNo", Util.TrimIntNull(dr["revetItemSysNo"]));
                        CostRevertItem.Add("Cost", Util.TrimDecimalNull(AppConst.DecimalNull));
                        new RMARevertDac().UpdateRevertItem(CostRevertItem);

                        //返回商品SysNo就是字段中保存的值。而二手、新品或者产品都要减库存 
                        newProductStatusValue = Util.TrimIntNull(dr["NewProductStatus"]);
                        newProductSysNo = (int)dr["RevertProductSysNo"];
                        if (newProductStatusValue != (int)AppEnum.NewProductStatus.Origin)
                        {   
                            UnitCostManager.GetInstance().SetCost(newProductSysNo, 1, Util.TrimDecimalNull(dr["Cost"]));
                            //InventoryManager.GetInstance().SetOutStockQty((int)dr["StockSysNo"], newProductSysNo, -1);
                        }
                        InventoryManager.GetInstance().SetOutStockQty((int)dr["StockSysNo"], newProductSysNo, -1);

                        Hashtable htRegisterNewProduct = new Hashtable();
                        htRegisterNewProduct.Add("SysNo", Util.TrimIntNull(dr["RegisterSysNo"]));
                        htRegisterNewProduct.Add("OwnBy", (int)AppEnum.RMAOwnBy.Customer);
                        htRegisterNewProduct.Add("Cost", AppConst.DecimalNull);

                        RMARegisterManager.GetInstance().UpdateRegister(htRegisterNewProduct);
                    }
                }

                //对于同一个发货单中非调货和调货情况下，以前代码会把跳过对于非调货单据设置状态。 //这里通过StockSysNo为空的情况限制到非调换情况 
                string sqlRegister = "select registersysno from rma_revert_item where StockSysNo is null and revertsysno=" + sysno.ToString();
                DataSet dsRegister = SqlHelper.ExecuteDataSet(sqlRegister);
                if (Util.HasMoreRow(dsRegister))
                {
                    foreach (DataRow drRegister in dsRegister.Tables[0].Rows)
                    {
                        Hashtable htRegisterNotNewProduct = new Hashtable();
                        htRegisterNotNewProduct.Add("SysNo", Util.TrimIntNull(drRegister["registersysno"]));
                        htRegisterNotNewProduct.Add("OwnBy", (int)AppEnum.RMAOwnBy.Customer);
                        htRegisterNotNewProduct.Add("Location", (int)AppEnum.RMALocation.Icson);
                        RMARegisterManager.GetInstance().UpdateRegister(htRegisterNotNewProduct);
                    }
                }

                scope.Complete();
            }
        }

        public void RevertOut(Hashtable paramHash)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;
            int newProductSysNo = -1;
            int newProductStatusValue = (int)AppEnum.NewProductStatus.Origin;
            decimal unitCost = AppConst.DecimalNull;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                this.UpdateRevert(paramHash);

                //调新品的情况：
                //用StockSysNo is not null来判断，而非RMA_Register.NewProductStatus，是为了进一步确保，已经选择了要退回的商品，有UnitCost值
                //RMA_Revert_Item.StockSysNo is not null 就已经排除了原物返还的情况
                string sql = @"select RMA_Revert_Item.StockSysNo , RMA_Register.ProductSysNo ,RMA_Revert_Item.SysNo as revetItemSysNo,Product_Price.UnitCost ,
                                      RMA_Register.SysNo as RegisterSysNo,RMA_Register.OwnBy ,RMA_Register.Location
                                      ,RMA_Register.NewProductStatus,RMA_Register.RevertProductSysNo
                               from   RMA_Revert_Item (NOLOCK)
                               inner  join  RMA_Register (NOLOCK) on RMA_Register.SysNo = RMA_Revert_Item.RegisterSysNo
                               inner  join Product_Price (NOLOCK) on Product_Price.ProductSysNo = RMA_Register.ProductSysNo
                               where  RMA_Revert_Item.StockSysNo is not null and RMA_Revert_Item.RevertSysNo =  " + paramHash["SysNo"].ToString();

                DataSet ds = SqlHelper.ExecuteDataSet(sql);

                if (Util.HasMoreRow(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (Util.TrimIntNull(dr["OwnBy"]) != (int)AppEnum.RMAOwnBy.Customer)
                        {
                            throw new BizException("该RMA单已经退款或者发货给客户，不能执行此操作！请与系统管理员联系。");
                        }

                        //判断是新品、二手，这里新品和返修退回都是原来商品编号，仅二手是新号码   //程序已经更改，见下面
                        //程序更改，当前返回产品都是RevertProductSysNo，而只有二手或者其他产品的unitcost需要从其他重新计算，新品、原物返还根据前面sql语句获取。
                        newProductStatusValue = Util.TrimIntNull(dr["NewProductStatus"]);
                        newProductSysNo = (int)dr["RevertProductSysNo"];
                        unitCost = Util.TrimDecimalNull(dr["UnitCost"]);   //这里已经获取了最新的成本，对于是新品的情况。
                        if (newProductStatusValue == (int)AppEnum.NewProductStatus.SecondHand || newProductStatusValue == (int)AppEnum.NewProductStatus.OtherProduct)
                        {
                            unitCost = ProductManager.GetInstance().LoadPrice(newProductSysNo).UnitCost;
                        }

                        Hashtable CostRevertItem = new Hashtable();

                        CostRevertItem.Add("SysNo", Util.TrimIntNull(dr["revetItemSysNo"]));
                        CostRevertItem.Add("Cost", unitCost);
                        new RMARevertDac().UpdateRevertItem(CostRevertItem);

                        Hashtable htRegisterNewProduct = new Hashtable();
                        htRegisterNewProduct.Add("SysNo", Util.TrimIntNull(dr["RegisterSysNo"]));
                        htRegisterNewProduct.Add("OwnBy", (int)AppEnum.RMAOwnBy.Icson);
                        htRegisterNewProduct.Add("Cost", unitCost);
                        RMARegisterManager.GetInstance().UpdateRegister(htRegisterNewProduct);

                        InventoryManager.GetInstance().SetOutStockQty(Util.TrimIntNull(dr["StockSysNo"]), newProductSysNo, 1);
                    }
                }

                //对于同一个发货单中非调货和调货情况下，以前代码会把跳过对于非调货单据设置状态。 //这里通过StockSysNo为空的情况限制到非调换情况
                string sqlRegister = @"select   registersysno ,location,ownby
                                            from rma_revert_item (NOLOCK) 
                                            inner join rma_register (NOLOCK) on rma_revert_item.registersysno = rma_register.sysno
                                            where  StockSysNo is null and revertsysno=" + paramHash["SysNo"].ToString();
                DataSet dsRegister = SqlHelper.ExecuteDataSet(sqlRegister);
                if (Util.HasMoreRow(dsRegister))
                {
                    foreach (DataRow drRegister in dsRegister.Tables[0].Rows)
                    {
                        if (Util.TrimIntNull(drRegister["location"]) != (int)AppEnum.RMALocation.Icson)
                        {
                            throw new BizException("该RMA单产品送修尚未返还，不能发还给客户，如果一定要现在发还，请选择调新品或良品，或与系统管理员联系！");
                        }

                        if (Util.TrimIntNull(drRegister["ownby"]) != (int)AppEnum.RMAOwnBy.Customer)
                        {
                            throw new BizException("该RMA单已经退款或者发货给客户，不能执行此操作！请与系统管理员联系。");
                        }

                        Hashtable htRegisterNotNewProduct = new Hashtable();
                        htRegisterNotNewProduct.Add("SysNo", Util.TrimIntNull(drRegister["RegisterSysNo"]));
                        htRegisterNotNewProduct.Add("OwnBy", (int)AppEnum.RMAOwnBy.Origin);
                        htRegisterNotNewProduct.Add("Location", (int)AppEnum.RMALocation.Origin);
                        RMARegisterManager.GetInstance().UpdateRegister(htRegisterNotNewProduct);
                    }
                }
                scope.Complete();
            }
        }

        public void SetRevertStock(int itemsysno, int stocksysno)
        {
            new RMARevertDac().SetRevertStock(itemsysno, stocksysno);
        }

        public DataSet CheckStock(int sysno)
        {
            string sql = @"select  StockSysNo 
                            from   RMA_Revert_Item (NOLOCK) 
                            inner  join RMA_Register (NOLOCK) on RMA_Revert_Item.registerSysNo = RMA_Register.SysNo
                            where  RMA_Register.NewProductStatus <> @NewProductStatus
                            and    RMA_Revert_Item.RevertSysNo = @RevertSysNo ";

            sql = sql.Replace("@NewProductStatus", ((int)AppEnum.NewProductStatus.Origin).ToString());

            sql = sql.Replace("@RevertSysNo", sysno.ToString());

            return SqlHelper.ExecuteDataSet(sql);

            //			DataSet ds = SqlHelper.ExecuteDataSet(sql);

            //			if (!Util.HasMoreRow(ds))
            //				return true;
            //
            //			bool result = true ;
            //
            //			foreach(DataRow dr in ds.Tables[0].Rows)
            //			{
            //			    if (dr["StockSysNo"].ToString() ==  AppConst.StringNull)
            //					result = false;
            //			}
            //			return result;
        }

        public RMARevertInfo Load(int sysno)
        {
            RMARevertInfo oRevert = new RMARevertInfo();
            string sql = "select * from RMA_Revert (NOLOCK) where sysno = " + sysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                MapRevert(oRevert, ds.Tables[0].Rows[0]);
            }
            return oRevert;
        }

        public RMARevertInfo GetOutBoundRevert(int sysno)
        {
            RMARevertInfo orderRevert = new RMARevertInfo();
            string sql = "select * from RMA_Revert (NOLOCK) where status = " + (int)AppEnum.RMARevertStatus.Reverted + " and sysno=" + sysno + "";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                MapRevert(orderRevert, ds.Tables[0].Rows[0]);
            }
            return orderRevert;
        }

        private void MapRevert(RMARevertInfo oInfo, DataRow dr)
        {
            oInfo.Address = dr["Address"].ToString();
            oInfo.Contact = dr["Contact"].ToString();
            oInfo.CreateTime = Util.TrimDateNull(dr["CreateTime"]);
            oInfo.CreateUserSysNo = Util.TrimIntNull(dr["CreateUserSysNo"]);
            oInfo.CustomerSysNo = Util.TrimIntNull(dr["CustomerSysNo"]);
            oInfo.Note = dr["Note"].ToString();
            oInfo.OutTime = Util.TrimDateNull(dr["OutTime"]);
            oInfo.OutUserSysNo = Util.TrimIntNull(dr["OutUserSysNo"]);
            oInfo.Phone = dr["Phone"].ToString();
            oInfo.RevertID = dr["RevertID"].ToString();
            oInfo.ShipType = Util.TrimIntNull(dr["ShipType"]);
            oInfo.Status = Util.TrimIntNull(dr["Status"]);
            oInfo.SysNo = Util.TrimIntNull(dr["SysNo"]);
            oInfo.ZIP = dr["ZIP"].ToString();
            oInfo.SOSysNo = Util.TrimIntNull(dr["SOSysNo"]);
            oInfo.LocationStatus = Util.TrimIntNull(dr["LocationStatus"]);
            oInfo.AddressAreaSysNo = Util.TrimIntNull(dr["AddressAreaSysNo"]);
            oInfo.PackageID = dr["PackageID"].ToString();
            oInfo.FreightUserSysNo = Util.TrimIntNull(dr["FreightUserSysNo"]);
            oInfo.SetDeliveryManTime = Util.TrimDateNull(dr["SetDeliveryManTime"]);
            oInfo.CheckQtyUserSysNo = Util.TrimIntNull(dr["CheckQtyUserSysNo"]);
            oInfo.CheckQtyTime = Util.TrimDateNull(dr["CheckQtyTime"]);
            oInfo.IsPrintPackageCover = Util.TrimIntNull(dr["IsPrintPackageCover"]);
            oInfo.IsConfirmAddress = Util.TrimIntNull(dr["IsConfirmAddress"]);
        }

        public int GetCurrentStatus(int sysno)
        {
            string sql = "select status from rma_revert (NOLOCK) where sysno = " + sysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
            {
                throw new BizException("Get Current Status Error!");
            }
            DataRow dr = ds.Tables[0].Rows[0];
            return Util.TrimIntNull(dr["status"]);
        }

        public string GetRevertPackage(int RegisterSysNo)
        {
            string sql = @"select packageid ,shiptypename
                           from  rma_register (nolock)
                           inner join rma_revert_item (nolock) on rma_register.sysno = rma_revert_item.registersysno
                           inner join rma_revert (nolock) on rma_revert_item.revertsysno=rma_revert.sysno
                           inner join shiptype on rma_revert.shiptype = shiptype.sysno
                           where rma_register.revertstatus = rma_revert.status and rma_revert_item.registersysno=" + RegisterSysNo;

            string packageID = AppConst.StringNull;

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                DataRow dr = ds.Tables[0].Rows[0];
                packageID = Util.TrimNull(dr["packageid"]);
                if (packageID != AppConst.StringNull)
                    packageID = "保修商品已返还<br>" + Util.TrimNull(dr["shiptypename"]) + ":" + packageID;
            }

            return packageID;
        }

        public bool GetRevertPackageByRevertSysNo(int revertSysNo)
        {
            string sql = @"select packageid 
                           from  rma_revert (nolock)
                           where sysno = " + revertSysNo;

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            bool s=false;
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

        public int  GetRevertSysNo(int RegisterSysNo)
        {
            string sql = @"select RevertSysNo
                           from  rma_revert_item (nolock)
                           where rma_revert_item.registersysno=" + RegisterSysNo;

             int  revertSysNo = AppConst.IntNull;

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                DataRow dr = ds.Tables[0].Rows[0];
                revertSysNo = Util.TrimIntNull(dr["RevertSysNo"]);
            }

            return revertSysNo;
        }

        public int GetRevertSysNoByID(string RevertID)
        {
            string sql = @"select SysNo
                           from  rma_revert (nolock)
                           where rma_revert.RevertID='" + RevertID+"'";

            int revertSysNo = AppConst.IntNull;

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                DataRow dr = ds.Tables[0].Rows[0];
                revertSysNo = Util.TrimIntNull(dr["SysNo"]);
            }

            return revertSysNo;
        }

        public DataSet GetRegisterSysNo(int sysno)
        {
            string sql = @"select registersysno   from rma_revert_item(nolock) inner join rma_register(nolock) on rma_register.sysno=rma_revert_item.registersysno  
                          where rma_register.newproductstatus=0 and status=1  and revertsysno=" + sysno + "order by Revertsysno desc ";//原物发还给客户
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

        public DataSet GetRMAProductCardDs(int StockSysNo,int productSysNo)
        {
            string sql = @"select a.*
						from 
						(
							--收货
							(
								select 
								'收货商品' as record_name,
								productsysno as record_product,  
								recvtime as record_time,
								rma_request.sysno as request_sysno,
								rma_register.sysno as register_sysno,
								1 as record_qty,
								1 as affect_Qty
								from rma_register
								inner join rma_request_item on rma_request_item.registerSysNo = rma_register.sysno
								inner join rma_request on rma_request.sysno = rma_request_item.requestsysno
								where 
								rma_register.status > @registerstatus 
								and rma_request.recvtime is not null  
								and rma_request.recvtime >= @Initdatetime
								and rma_register.productsysno=@productsysno
							)
							union all
							--送修
							(
							    select 
								'商品送修' as record_name,
								productsysno as record_product,  
								RMA_OutBound.OutTime as record_time,
								rma_request.sysno as request_sysno,
								rma_register.sysno as register_sysno,
								1 as record_qty,
								0 as affect_Qty
								from rma_register
								inner join rma_request_item on rma_request_item.registerSysNo = rma_register.sysno
								inner join rma_request on rma_request.sysno = rma_request_item.requestsysno
								left join RMA_OutBound_Item on RMA_OutBound_Item.registerSysNo = rma_register.sysno
								left join RMA_OutBound on RMA_OutBound.sysno = RMA_OutBound_Item.OutBoundSysNo
								where 
								rma_register.status > @registerstatus 
								and RMA_OutBound.OutTime is not null  
								and RMA_OutBound.OutTime >= @Initdatetime
								and rma_register.productsysno=@productsysno
							)
							union all
							--送修返回
							(
							    select 
								'送修返回' as record_name,
								productsysno as record_product,  
								ResponseTime as record_time,
								rma_request.sysno as request_sysno,
								rma_register.sysno as register_sysno,
								1 as record_qty,
								0 as affect_Qty
								from rma_register
								inner join rma_request_item on rma_request_item.registerSysNo = rma_register.sysno
								inner join rma_request on rma_request.sysno = rma_request_item.requestsysno
								where 
								rma_register.status > @registerstatus 
								and rma_register.ResponseTime is not null 
								and rma_register.ResponseTime >=  @Initdatetime
								and rma_register.productsysno=@productsysno
							)
							union all
							--发还
							(
							    select 
								'发货（正常）' as record_name,
								productsysno as record_product,  
								RMA_Revert.OutTime as record_time,
								rma_request.sysno as request_sysno,
								rma_register.sysno as register_sysno,
								1 as record_qty,
								-1 as affect_Qty
								from rma_register
								inner join rma_request_item on rma_request_item.registerSysNo = rma_register.sysno
								inner join rma_request on rma_request.sysno = rma_request_item.requestsysno
								left join RMA_Revert_Item on RMA_Revert_Item.RegisterSysNo = rma_register.sysno
								left join RMA_Revert on RMA_Revert.sysno = RMA_Revert_Item.RevertSysNo
								where 
								rma_register.status > @registerstatus 
								and RMA_Revert.OutTime is not null  
								and RMA_Revert.OutTime >=  @Initdatetime
								and newproductstatus = @NormalRevertStatus
								and rma_register.productsysno=@productsysno
							)
							union all
							--发新品
							(
							    select 
								'发货（新品）' as record_name,
								productsysno as record_product,  
								RMA_Revert.OutTime as record_time,
								rma_request.sysno as request_sysno,
								rma_register.sysno as register_sysno,
								1 as record_qty,
								0 as affect_Qty
								from rma_register
								inner join rma_request_item on rma_request_item.registerSysNo = rma_register.sysno
								inner join rma_request on rma_request.sysno = rma_request_item.requestsysno
								left join RMA_Revert_Item on RMA_Revert_Item.RegisterSysNo = rma_register.sysno
								left join RMA_Revert on RMA_Revert.sysno = RMA_Revert_Item.RevertSysNo
								where 
								rma_register.status > @registerstatus 
								and RMA_Revert.OutTime is not null  
								and RMA_Revert.OutTime >=  @Initdatetime
								and newproductstatus = @NewProductRevertStatus
								and rma_register.productsysno=@productsysno
							)
							union all
							--发二手
							(
							    select 
								'发货（二手）' as record_name,
								productsysno as record_product,  
								RMA_Revert.OutTime as record_time,
								rma_request.sysno as request_sysno,
								rma_register.sysno as register_sysno,
								1 as record_qty,
								0 as affect_Qty
								from rma_register
								inner join rma_request_item on rma_request_item.registerSysNo = rma_register.sysno
								inner join rma_request on rma_request.sysno = rma_request_item.requestsysno
								left join RMA_Revert_Item on RMA_Revert_Item.RegisterSysNo = rma_register.sysno
								left join RMA_Revert on RMA_Revert.sysno = RMA_Revert_Item.RevertSysNo
								where 
								rma_register.status > @registerstatus 
								and RMA_Revert.OutTime is not null  
								and RMA_Revert.OutTime >=  @Initdatetime
								and newproductstatus = @SecProductRevertStatus
								and rma_register.productsysno=@productsysno
							)
                            union all
							--Secproduct
							(
							    select 
								'发货（非当前RMACase）' as record_name,
								productsysno as record_product,  
								RMA_Revert.OutTime as record_time,
								rma_request.sysno as request_sysno,
								rma_register.sysno as register_sysno,
								1 as record_qty,
								0 as affect_Qty
								from rma_register
								inner join rma_request_item on rma_request_item.registerSysNo = rma_register.sysno
								inner join rma_request on rma_request.sysno = rma_request_item.requestsysno
								left join RMA_Revert_Item on RMA_Revert_Item.RegisterSysNo = rma_register.sysno
								left join RMA_Revert on RMA_Revert.sysno = RMA_Revert_Item.RevertSysNo
								where 
								rma_register.status > @registerstatus 
								and RMA_Revert.OutTime is not null  
								and RMA_Revert.OutTime >=  @Initdatetime
								and newproductstatus = @OtherProductRevertStatus
								and rma_register.productsysno=@productsysno
							)
							union all
							--退货入库
							(
							    select 
								'退货入库' as record_name,
								productsysno as record_product,  
								RMA_Return.ReturnTime as record_time,
								rma_request.sysno as request_sysno,
								rma_register.sysno as register_sysno,
								1 as record_qty,
								-1 as affect_Qty
								from rma_register 
								inner join rma_request_item on rma_request_item.registerSysNo = rma_register.sysno
								inner join rma_request on rma_request.sysno = rma_request_item.requestsysno
								left join RMA_Return_Item on RMA_Return_Item.registerSysNo = rma_register.sysno
								left join RMA_Return on RMA_Return.sysno = RMA_Return_Item.ReturnSysNo
								where 
								rma_register.status > @registerstatus 
								and RMA_Return.ReturnTime is not null  
								and RMA_Return.ReturnTime >=  @Initdatetime
								and rma_register.productsysno=@productsysno
							)
                            union all 
                            --移库移入
							(
								select 
								'移库移入' as record_name,
								productsysno as record_product,  
								intime as record_time,
								'' as request_sysno,
								'' as register_sysno,
								shiftqty as record_qty,
								shiftqty as affect_Qty
								from st_shift ss 
                                inner join st_shift_item si on ss.sysno=si.shiftsysno 
								where 
								si.productsysno=@productsysno and ss.status in(@instockstatus) and ss.stocksysnoB=@instocksysno 
							)
                            union all 
                            --移库移出
							(
								select 
								'移库移出' as record_name,
								productsysno as record_product,  
								outtime as record_time,
								'' as request_sysno,
								'' as register_sysno,
								shiftqty as record_qty,
								-shiftqty as affect_Qty
								from st_shift ss 
                                inner join st_shift_item si on ss.sysno=si.shiftsysno 
								where 
								si.productsysno=@productsysno and ss.status in(@outstockstatus) and ss.stocksysnoA=@outstocksysno 
							)
							--union all
							----ProductCard_InitialQty
							--(
							--	select 
							--	'2008-04-20库存应有起始值' as record_name,
							--	RMA_ProductCard_InitialQty.productsysno as record_product,
							--  '2008-04-20' as record_time,
							--	NULL as request_sysno,
							--	NULL as register_sysno,
							--	InitialQty as record_qty,
							--	InitialQty as affect_Qty
							--	from RMA_ProductCard_InitialQty (NOLOCK)
							--	where 
                            --  RMA_ProductCard_InitialQty.productsysno=@productsysno
							--)
						)
						as a 
						order by record_time";

            sql = sql.Replace("@productsysno", productSysNo.ToString());
            sql = sql.Replace("@registerstatus", ((int)AppEnum.RMARequestStatus.Abandon).ToString());
            sql = sql.Replace("@Initdatetime", Util.ToSqlString(AppConst.RMA_Initializtion_DateTime.ToString()));
            sql = sql.Replace("@NormalRevertStatus", ((int)AppEnum.NewProductStatus.Origin).ToString());
            sql = sql.Replace("@NewProductRevertStatus", ((int)AppEnum.NewProductStatus.NewProduct).ToString());
            sql = sql.Replace("@SecProductRevertStatus", ((int)AppEnum.NewProductStatus.SecondHand).ToString());
            sql = sql.Replace("@OtherProductRevertStatus", ((int)AppEnum.NewProductStatus.OtherProduct).ToString());

            sql = sql.Replace("@instocksysno", StockSysNo.ToString());
            sql = sql.Replace("@outstocksysno", StockSysNo.ToString());
            sql = sql.Replace("@instockstatus", ((int)AppEnum.ShiftStatus.InStock).ToString());
            sql = sql.Replace("@outstockstatus", ((int)AppEnum.ShiftStatus.InStock).ToString() + "," + ((int)AppEnum.ShiftStatus.OutStock).ToString());

            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetProductsToShift(int StockSysNo)
        {
            string sql = @"select rr.productsysno,productid,productname,count(rr.productsysno) as needRevertCount,inv_s.accountqty 
                            from RMA_Register rr 
                            inner join RMA_Revert_Item rri on rr.sysno=rri.RegisterSysNo 
                            inner join RMA_Revert rre on rre.sysno=rri.RevertSysNo and rre.status=@RevertStatus
                            inner join Product p on p.sysno=rr.productsysno and rr.NewProductStatus in (@NewProductStatus) 
                            inner join Inventory_Stock inv_s on inv_s.productsysno = p.sysno and inv_s.stocksysno=@stocksysno 
                            group by rr.productsysno,ProductID,ProductName,inv_s.accountqty
                            having inv_s.accountqty < (count(rr.productsysno)+1) ";

            sql = sql.Replace("@RevertStatus", ((int)AppEnum.RMARevertStatus.WaitingRevert).ToString());
            sql = sql.Replace("@NewProductStatus", ((int)AppEnum.NewProductStatus.NewProduct).ToString() + "," + ((int)AppEnum.NewProductStatus.SecondHand).ToString());
            sql = sql.Replace("@stocksysno",StockSysNo.ToString());

            return SqlHelper.ExecuteDataSet(sql);
        }


        public int GetRevertSysNofromID(string RevertID)
        {
            string sql = "select sysno from rma_revert (NOLOCK) where RevertID=" + Util.ToSqlString(RevertID.ToString());
            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (Util.HasMoreRow(ds))
                return (int)ds.Tables[0].Rows[0][0];
            else
                return AppConst.IntNull;

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
                new RMARequestDac().UpdateRequest(updateHash);
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
                new RMARequestDac().UpdateRequest(ht);
                scope.Complete();
            }
        }

        public int GetFreightUserSysNofromID(string RevertID)
        {
            string sql = "select FreightUserSysNo from rma_revert (NOLOCK) where RevertID=" + Util.ToSqlString(RevertID.ToString());
            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (Util.HasMoreRow(ds))
                return (int)ds.Tables[0].Rows[0][0];
            else
                return AppConst.IntNull;

        }

        public int GetDLSysNofromID(string RevertID)
        {
            string sql = "select dlsysno from rma_revert (NOLOCK) where RevertID=" + Util.ToSqlString(RevertID.ToString());
            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (Util.HasMoreRow(ds))
                return (int)ds.Tables[0].Rows[0][0];
            else
                return AppConst.IntNull;

        }

        public DataSet GetFreightMenDs(Hashtable paramHash)
        {
            string sql = @"select rma_revert.*,area.DistrictName,area.localcode,sys_user.username as freightusername
                          from rma_revert,area ,sys_user
                          where rma_revert.status=1 and AddressAreaSysNo=area.sysno and sys_user.sysno=FreightUserSysNo";


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
                    else if (item is int )
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

        public int DeleteItem(int registerSysNo, int revertSysNo)
        {
            int result;
          RMARevertInfo oRevertInfo=  RMARevertManager.GetInstance().Load(revertSysNo);
          if (oRevertInfo.Status != (int)AppEnum.RMARevertStatus.WaitingRevert)
          {
              throw new BizException("发货单不是待发货状态，不能删除相关Item!");
          }
          else
          {
              result = new RMARevertDac().DeleteItem(registerSysNo, revertSysNo);
          }
            return result;
        }

        public DataSet RevertRequestList(int ReceiveType)
        {
            //增加SOID，由原来按照CustomerName生成Revert单改为由SOID来生成（显示为SOID，实际上还是按照Customer生成送货地址等信息）  
            //增加RequestID， 按照申请单号生成，取申请单的地址
            string sql = @"select Distinct RMA_Request.SysNo as RequestSysNo, RMA_Request.RequestID
                            from  RMA_Register (NOLOCK)
                            left  join RMA_Request_Item (NOLOCK) on RMA_Register.sysno = RMA_Request_Item.RegisterSysNo
                            left  join RMA_Request (NOLOCK) on RMA_Request.sysno = RMA_Request_Item.RequestSysNo
                            where RMA_Register.RevertStatus = @RevertStatus
                             and  RMA_Register.sysno not in
                             (select RMA_Revert_Item.registerSysNo
                              from   RMA_Revert_Item (NOLOCK)
                              inner join RMA_Revert (NOLOCK) on RMA_Revert_Item.RevertSysNo = RMA_Revert.sysno 
                              where  RMA_Revert.status = @RevertStatus ) and @ReceiveType  @sqlHost";

            string sqlHost= @"and not exists
				            (select   RMA_Request.sysno  from
                                          RMA_Request_Item (NOLOCK) 
		                                  left join RMA_Register(nolock) on RMA_Register.sysno=RMA_Request_Item.RegisterSysNo
                                          where ( isnull (RMA_Register.RevertStatus," + AppConst.IntNull + " )<>" + (int)AppEnum.RMARevertStatus.WaitingRevert + "and isnull (RMA_Register.RefundStatus," + AppConst.IntNull + " )<>" + (int)AppEnum.RMARefundStatus.Refunded + ") and RMA_Request_Item.RequestSysNo = RMA_Request.sysno)";

            sql = sql.Replace("@RevertStatus", ((int)AppEnum.RMARevertStatus.WaitingRevert).ToString());
            if (ReceiveType == (int)AppEnum.RMARecieveType.Host)
            {
                sql = sql.Replace("@ReceiveType", "RMA_Request.ReceiveType=" + (int)AppEnum.RMARecieveType.Host);
                sql = sql.Replace("@sqlHost", sqlHost);
            }
            else  //除了整机的，剩下都是单件，之前的单件ReceiveType为null
            {
                sql = sql.Replace("@ReceiveType", "isnull(RMA_Request.ReceiveType,"+AppConst.IntNull+")<>" + (int)AppEnum.RMARecieveType.Host);
                sql = sql.Replace("@sqlHost", "");
            }
            return SqlHelper.ExecuteDataSet(sql);

           
        }
    }
}