using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Icson.Objects;
using Icson.Objects.RMA;
using Icson.Objects.Sale;
using Icson.Objects.Basic;
using Icson.Objects.ImportData;

using Icson.DBAccess.RMA;
using Icson.DBAccess.ImportData;
using Icson.DBAccess;

using Icson.Utils;

using Icson.BLL.Basic;
using Icson.BLL.Sale;
using Icson.BLL.RMA;

using System.Transactions;

namespace Icson.BLL.RMA
{
    /// <summary>
    /// Summary description for RMARequestManager.
    /// </summary>
    public class RMARequestManager
    {
        private RMARequestManager()
        {

        }
        private static RMARequestManager _instance;
        public static RMARequestManager GetInstance()
        {
            if (_instance == null)
                _instance = new RMARequestManager();
            return _instance;
        }
        /// <summary>
        /// Insert Request
        /// </summary>
        /// <param name="oInfo"></param>
        public void InsertRequest(RMARequestInfo oInfo)
        {
            new RMARequestDac().InsertRequest(oInfo);
        }

        public void RequestReceived(Hashtable ht)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;
            int sysno = (int)ht["SysNo"];
            int intWithin7Days = 8;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                this.UpdateRequest(ht);
                //更新Request中的Register，设置其Ownby = customer；Location = Icson
                string sqlRegister = @"select rma_request_item.RegisterSysNo as registersysno ,V_SO_Item.Cost as Cost,rma_request.CustomerSendTime,v_so_master.OutTime,
                                       rma_register.productsysno,rma_register.receivestocksysno 
                                       from rma_request_item (nolock) 
                                          inner join rma_request (nolock) on rma_request_item.requestsysno=rma_request.sysno
                                          inner join v_so_master (nolock) on v_so_master.sysno=rma_request.sosysno 
                                          inner join v_so_item (nolock) on v_so_item.sosysno=v_so_master.sysno 
                                          inner join rma_register (nolock) on (rma_register.productsysno=v_so_item.productsysno and rma_register.sysno=rma_request_item.registersysno)   
                                      where requestsysno = " + sysno;
                DataSet dsRegister = SqlHelper.ExecuteDataSet(sqlRegister);

                if (Util.TrimDateNull(ht["CustomerSendTime"]).AddDays(-7) <= Util.TrimDateNull(dsRegister.Tables[0].Rows[0]["OutTime"]))
                    intWithin7Days = (int)AppEnum.YNStatus.Yes;
                else if (Util.TrimDateNull(ht["CustomerSendTime"]).AddDays(-7) > Util.TrimDateNull(dsRegister.Tables[0].Rows[0]["OutTime"]))
                    intWithin7Days = (int)AppEnum.YNStatus.No;

                foreach (DataRow dr in dsRegister.Tables[0].Rows)
                {
                    Hashtable htRegister = new Hashtable();
                    htRegister.Add("SysNo", Util.TrimIntNull(dr["registersysno"]));
                    htRegister.Add("OwnBy", (int)AppEnum.RMAOwnBy.Customer);
                    htRegister.Add("Location", (int)AppEnum.RMALocation.Icson);
                    htRegister.Add("Cost", Util.TrimDecimalNull(dr["Cost"]));
                    if (intWithin7Days == (int)AppEnum.YNStatus.Yes || intWithin7Days == (int)AppEnum.YNStatus.No)
                    {
                        htRegister.Add("IsWithin7Days", intWithin7Days);
                    }
                    RMARegisterManager.GetInstance().UpdateRegister(htRegister);

                    //收货更新库存 -- lucky 03/13/2008
                    InventoryManager.GetInstance().SetInStockQty(Util.TrimIntNull(dr["receivestocksysno"]),Util.TrimIntNull(dr["productsysno"]),1);
                }
                scope.Complete();
            }
            //收到货物后，系统发mail给客户                
//            string sql = @"select email, requestid
//                                from customer (NOLOCK) inner join rma_request (NOLOCK) on customer.sysno = rma_request.customersysno
//                                where rma_request.sysno = " + sysno;
//            DataSet ds = SqlHelper.ExecuteDataSet(sql);
//            if (Util.HasMoreRow(ds))
//            {
//                string emailAddress = Util.TrimNull(ds.Tables[0].Rows[0]["Email"]);
//                if (emailAddress == AppConst.StringNull)
//                    return;

//                string requestID = ds.Tables[0].Rows[0]["requestID"].ToString();

//                string mailBody = @"<span style='font-size:12.0pt;font-family:华文细黑'>尊敬的ORS商城网用户:<br><br>您好!<br>您申请的返修单@requestIDORS商城网已经收到,现已进入检测维修阶段,
//                                        ORS商城网将严格按照国家三包法规进行处理。返修单号处理完毕后，ORS商城网会有专人与您取得联系并告知相应事宜。
//                                        您也可以点击此链接<a href='http://www.baby1one.com.cn/Login/WebLogin.aspx?url=http://www.baby1one.com.cn/RMA/RMARule.aspx'><font color=blue>保修中心</font></a>
//                                        在ORS商城网上查询退换货品的状态。<br><br>
//                                        如您有任何疑问，请拨打ORS商城网客服热线 021- 34241690  周一至周六 8:30 ~ 5:30，我们的客户服务人员将竭诚为您服务。<br><br>
//                                        (此邮件为系统自动生成邮件，请不要直接回复！)<br><br>
//                                        感谢您一直以来对ORS商城网的支持！<br><br></span>";
//                mailBody = mailBody.Replace("@requestID", requestID);

//                TCPMail oMail = new TCPMail();
//                oMail.Send(emailAddress, "您申请的返修单:" + requestID + "ORS商城网已经收到", mailBody);
//            }
        }
        /// <summary>
        ///   Update Request
        /// </summary>										  
        /// <param name="paramHash"></param>
        public void UpdateRequest(Hashtable paramHash)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;
            int intWithin7Days = 8;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                new RMARequestDac().UpdateRequest(paramHash);
                //如果包含状态Status的改变，需要相应改变Register中的Status
                if (paramHash.ContainsKey("Status") || paramHash.ContainsKey("CustomerSendTime"))
                {
                    //                    string sql = @"select RegisterSysNo
                    //                                    from RMA_Request_Item (NOLOCK)                                     
                    //                                    where RequestSysNo = " + paramHash["SysNo"].ToString();
                    string sql = @"select rma_request_item.RegisterSysNo as RegisterSysNo,rma_request.CustomerSendTime,v_so_master.OutTime
                                       from rma_request_item (nolock) 
                                          inner join rma_request (nolock) on rma_request_item.requestsysno=rma_request.sysno
                                          inner join v_so_master (nolock) on v_so_master.sysno=rma_request.sosysno 
                                          inner join v_so_item (nolock) on v_so_item.sosysno=v_so_master.sysno 
                                          inner join rma_register (nolock) on (rma_register.productsysno=v_so_item.productsysno and rma_register.sysno=rma_request_item.registersysno)   
                                      where RequestSysNo = " + paramHash["SysNo"].ToString();

                    DataSet ds = SqlHelper.ExecuteDataSet(sql);
                    if (paramHash.ContainsKey("CustomerSendTime"))
                    {
                        if (Util.TrimDateNull(paramHash["CustomerSendTime"]).AddDays(-7) <= Util.TrimDateNull(ds.Tables[0].Rows[0]["OutTime"]))
                            intWithin7Days = (int)AppEnum.YNStatus.Yes;
                        else if (Util.TrimDateNull(paramHash["CustomerSendTime"]).AddDays(-7) > Util.TrimDateNull(ds.Tables[0].Rows[0]["OutTime"]))
                            intWithin7Days = (int)AppEnum.YNStatus.No;
                    }
                    if (Util.HasMoreRow(ds))
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            Hashtable htRegister = new Hashtable();
                            htRegister.Add("SysNo", Util.TrimIntNull(dr["RegisterSysNo"]));
                            if (paramHash.ContainsKey("Status"))
                            {
                                htRegister.Add("Status", Util.TrimIntNull(paramHash["Status"]));
                            }
                            else if (paramHash.ContainsKey("CustomerSendTime"))
                            {
                                if (intWithin7Days == (int)AppEnum.YNStatus.Yes || intWithin7Days == (int)AppEnum.YNStatus.No)
                                {
                                    htRegister.Add("IsWithin7Days", intWithin7Days);
                                }
                            }
                            RMARegisterManager.GetInstance().UpdateRegister(htRegister);
                        }
                    }
                }
                scope.Complete();
            }
        }
        /// <summary>
        /// Insert Request_Item
        /// </summary>
        /// <param name="oInfo"></param>
        public void InsertRequestItem(RMARequestItemInfo oInfo)
        {
            new RMARequestDac().InsertRequestItem(oInfo);
        }
        /// <summary>
        /// Delete Request Item
        /// </summary>
        /// <param name="RequestItemID"></param>
        public void DeleteRquestItem(int RequestItemID)
        {
            new RMARequestDac().DeleteItem(RequestItemID);
        }

        public void GetRegisterHash(out Hashtable RegisterInfoHash, out Hashtable ProductHash, Hashtable sysnoHash)
        {
            RegisterInfoHash = new Hashtable(5);
            ProductHash = new Hashtable(5);
            if (sysnoHash == null || sysnoHash.Count == 0)
            {
                RegisterInfoHash = null;
                ProductHash = null;
                return;
            }
            string sql = "select * from RMA_Register (NOLOCK) where sysno in (";
            int index = 0;
            foreach (int item in sysnoHash.Keys)
            {
                if (index != 0)
                    sql += ",";

                sql += item.ToString();

                index++;
            }
            sql += ")";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                RMARegisterInfo paramInfo = new RMARegisterInfo();
                mapRegister(paramInfo, dr);
                RegisterInfoHash.Add(paramInfo.SysNo, paramInfo);
            }
            sql = "select distinct productsysno from RMA_Register (NOLOCK) where sysno in (";
            index = 0;
            foreach (int item in sysnoHash.Keys)
            {
                if (index != 0)
                    sql += ",";

                sql += item.ToString();

                index++;
            }
            sql += ")";
            ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return;
            Hashtable tempht = new Hashtable(5);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                tempht.Add(dr["ProductSysNo"].ToString(), null);
            }
            ProductHash = ProductManager.GetInstance().GetProductBoundle(tempht);
        }

        public void mapRegister(RMARegisterInfo oInfo, DataRow dr)
        {
            oInfo.SysNo = Util.TrimIntNull(dr["SysNo"]);
            oInfo.ProductSysNo = Util.TrimIntNull(dr["ProductSysNo"]);
            oInfo.RequestType = Util.TrimIntNull(dr["RequestType"]);
            oInfo.CustomerDesc = Util.TrimNull(dr["CustomerDesc"]);
            //oInfo.IsChecked = Util.TrimIntNull(dr["IsChecked"]);
            oInfo.CheckDesc = Util.TrimNull(dr["CheckDesc"]);
            oInfo.NewProductStatus = Util.TrimIntNull(dr["NewProductStatus"]);
            oInfo.RevertStatus = Util.TrimIntNull(dr["RevertStatus"]);
            oInfo.OutBoundStatus = Util.TrimIntNull(dr["OutBoundStatus"]);
            oInfo.ReturnStatus = Util.TrimIntNull(dr["ReturnStatus"]);
            oInfo.ResponseDesc = Util.TrimNull(dr["ResponseDesc"]);
            oInfo.ResponseTime = Util.TrimDateNull(dr["ResponseTime"]);
            oInfo.NextHandler = Util.TrimIntNull(dr["NextHandler"]);
            oInfo.Memo = Util.TrimNull(dr["Memo"]);
            oInfo.Status = Util.TrimIntNull(dr["Status"]);
            oInfo.ProductNo = Util.TrimNull(dr["ProductNo"]);
        }

        public DataTable GetSOforRMA(int customerSysNo)
        {
            //			string sql = @"select 
            //								sm.sysno,sm.soid
            //							from 
            //								so_master sm
            //							left join RMA_Request rm on rm.sosysno = sm.sysno and rm.status<>"+(int)AppEnum.RMARequestStatus.Abandon+" and rm.status<>"+(int)AppEnum.RMARequestStatus.Closed
            //				+@"	where
            //								sm.status="+(int)AppEnum.SOStatus.OutStock+" and sm.customersysno= "+customerSysNo+" and rm.sosysno is null";

            string sql = @" select sysno, soid from v_so_master (NOLOCK)
                            where status = " + (int)AppEnum.SOStatus.OutStock + " and customersysno=" + customerSysNo
                       + " ORDER BY OrderDate DESC"; // 根据SO下单时间降序排序
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            List<string> list = new List<string>();
            if (Util.HasMoreRow(ds))
                return ds.Tables[0];
            else
                return null;
        }
        public DataTable GetRMABySO(int SOSysNo)
        {
            string sql = @"select RMA_Request.*
                            from RMA_Request (NOLOCK) 
							where RMA_Request.SOSysNo = " + SOSysNo
                             +"order by CreateTime desc";

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            List<string> list = new List<string>();
            if (Util.HasMoreRow(ds))
                return ds.Tables[0];
            else
                return null;
        }
        public DataTable GetRMAByCustomerSysNo(int CustomerSysNo)
        {
            string sql = @"SELECT RMA_Request.*
                           FROM RMA_Request WITH (NOLOCK) 
                           LEFT  JOIN SO_Master ON RMA_Request.SOSysNo = SO_Master.SysNo
                           WHERE SO_Master.CustomerSysNo =" + CustomerSysNo + "Order by CreateTime desc";
                           

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            List<string> list = new List<string>();
            if (Util.HasMoreRow(ds))
                return ds.Tables[0];
            else
                return null;
        }

        public int GetRegisterSysnoByRequestSysno(int RMARequestSysNo)
        {
            string sql = "select registersysno from rma_request_item (NOLOCK) where requestsysno=" + RMARequestSysNo;

            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return AppConst.IntNull;

            DataRow dr = ds.Tables[0].Rows[0];

            return Util.TrimIntNull(dr["registersysno"]);
        }

        public DataTable GetRegisterSysnoBySOSysno(int SOSysNo,int RequestSysNo)
        {
            string sql = @"select RMA_Register.* ,Product.ProductName ,Product.ProductID,Product.SysNo as ProductSysNo,RMA_Request.RecvTime as RMARecvTime,
                            RMA_Request.Sysno as RequestSysno
                            from RMA_Register (NOLOCK) 
							inner join Product (NOLOCK) on Product.sysno = RMA_Register.ProductSysNo 
							inner join RMA_Request_Item (NOLOCK) on RMA_Register.SysNo = RMA_Request_Item.RegisterSysNo
							inner join RMA_Request (NOLOCK) on RMA_Request_Item.RequestSysNo = RMA_Request.SysNo
							where RMA_Request.SOSysNo = " + SOSysNo + "and RMA_Request.sysno=" + RequestSysNo;

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            List<string> list = new List<string>();
            if (Util.HasMoreRow(ds))
                return ds.Tables[0];
            else
                return null;
        }

        public RMARequestInfo BuildRMAfromSO(int soSysNo)
        {
            //			if(!this.IfExistOpenedRMA(soSysNo))
            //			{
            RMARequestInfo rmaInfo = new RMARequestInfo();
            SOInfo soInfo = SaleManager.GetInstance().LoadSO(soSysNo);
            if (soInfo.PositiveSOSysNo != AppConst.IntNull)
                throw new BizException("该SO为隔月取消出库的负销售单，RMA申请失败！");
            if (soInfo != null && soInfo.Status == (int)AppEnum.SOStatus.OutStock)
            {
                rmaInfo.SOSysNo = soInfo.SysNo;
                rmaInfo.CustomerSysNo = soInfo.CustomerSysNo;
                CustomerInfo oCustomer = CustomerManager.GetInstance().Load(soInfo.CustomerSysNo);
                rmaInfo.Address = soInfo.ReceiveAddress != AppConst.StringNull ? soInfo.ReceiveAddress : (oCustomer.ReceiveAddress != AppConst.StringNull ? oCustomer.ReceiveAddress : (oCustomer.DwellAddress != AppConst.StringNull ? oCustomer.DwellAddress : "无"));
                rmaInfo.Contact = soInfo.ReceiveContact != AppConst.StringNull ? soInfo.ReceiveContact : (oCustomer.ReceiveContact != AppConst.StringNull ? oCustomer.ReceiveContact : (oCustomer.CustomerName != AppConst.StringNull ? oCustomer.CustomerName : "无"));
                rmaInfo.CreateTime = DateTime.Now;
                if (soInfo.ReceivePhone != AppConst.StringNull)
                    rmaInfo.Phone = soInfo.ReceivePhone;
                if (soInfo.ReceiveCellPhone != AppConst.StringNull)
                    rmaInfo.Phone += "  " + soInfo.ReceiveCellPhone;
                //rmaInfo.Phone = soInfo.ReceivePhone != AppConst.StringNull ? soInfo.ReceivePhone : (oCustomer.ReceiveCellPhone != AppConst.StringNull ? oCustomer.ReceiveCellPhone : (oCustomer.ReceivePhone != AppConst.StringNull ? oCustomer.ReceivePhone : (oCustomer.CellPhone != AppConst.StringNull ? oCustomer.CellPhone : (oCustomer.Phone != AppConst.StringNull ? oCustomer.Phone : "无"))));

                rmaInfo.AreaSysNo = soInfo.ReceiveAreaSysNo;   //this AreaSysNo field is added lastest.  
                rmaInfo.Zip = soInfo.ReceiveZip;

                if (soInfo.ItemHash.Count > 0)
                {
                    int j = 0;
                    foreach (SOItemInfo soItem in soInfo.ItemHash.Values)
                    {
                        if (soItem.ProductType == (int)AppEnum.SOItemType.Promotion)
                            continue;  //这里排除优惠券。

                        if (soItem.Quantity > 0)
                        {
                            Hashtable htTemp = new Hashtable(1);
                            htTemp.Add("SOItemSysNo",soItem.SysNo.ToString());
                            DataSet dsTemp = SaleManager.GetInstance().GetSOProductIDSysNoList(htTemp);

                            int rowCount = 0;
                            if(Util.HasMoreRow(dsTemp))
                                rowCount = dsTemp.Tables[0].Rows.Count;

                            for (int i = 1; i <= soItem.Quantity; i++)
                            {
                                RMARegisterInfo rmaItem = new RMARegisterInfo();
                                rmaItem.ProductSysNo = soItem.ProductSysNo;
                                rmaItem.RequestType = (int)AppEnum.RMARequestType.Maintain;

                                if (i <= rowCount)
                                {
                                    int POSysNo = Util.TrimIntNull(dsTemp.Tables[0].Rows[i-1]["posysno"].ToString());
                                    int ProductIDSysNo = Util.TrimIntNull(dsTemp.Tables[0].Rows[i - 1]["ProductIDSysNo"]);
                                    rmaItem.ProductIDSysNo = ProductIDSysNo;
                                    
                                    if (POSysNo > 0)
                                    {
                                        rmaItem.SOItemPODesc = "采购单号:<a href=\"javascript:openWindowS2('../Purchase/POSheet.aspx?sysno=" + POSysNo + "&opt=view')" + "\" >" + POSysNo + "</a><br>";
                                    }
                                    else if (ProductIDSysNo > 0)
                                    {
                                        rmaItem.SOItemPODesc = "商品序列号:<a href=\"javascript:openWindowS2('../Basic/ProductID.aspx?sysno=" + ProductIDSysNo + "')" + "\" >" + ProductIDSysNo + "</a><br>";
                                    }
                                }
                                rmaInfo.ItemHash.Add(j, rmaItem);
                                j = j + 1;
                            }
                        }
                    }
                }
            }
            else
                rmaInfo = null;
            return rmaInfo;
            //			}
            //			else 
            //				throw new BizException("本销售单已经存在一张保修单在处理中，其间您不能再提交新的保修申请，请联系ORS商城客服");
        }
        public RMARequestInfo BuildRMAfromSO(string soID)
        {
            int sysno = SaleManager.GetInstance().GetSOSysNofromID(soID.ToString());
            if (sysno != AppConst.IntNull)
                return this.BuildRMAfromSO(sysno);
            else
            {
                try
                {
                    return this.BuildRMAfromSO(Util.TrimIntNull(soID));
                }
                catch
                {
                    return null;
                }
            }
        }

        private bool IfExistOpenedRMA(int soSysNo)
        {
            bool ifExist;
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                string sql = "select sysno from RMA_Request (NOLOCK) where status>=" + (int)AppEnum.RMARequestStatus.Orgin + " and status<" + (int)AppEnum.RMARequestStatus.Closed + " and sosysno=" + soSysNo;
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (Util.HasMoreRow(ds))
                    ifExist = true;
                else
                    ifExist = false;
                scope.Complete();
            }
            return ifExist;
        }
        /// <summary>
        /// 新增rma单
        /// </summary>
        /// <param name="rmaInfo"></param>
        public void AddRMA(RMARequestInfo rmaInfo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                rmaInfo.SysNo = SequenceDac.GetInstance().Create("RMA_Request_Sequence");
                rmaInfo.RequestID = this.BuildRMAID(rmaInfo.SysNo);
                rmaInfo.Status = (int)AppEnum.RMAStatus.Origin;
                this.InsertRequest(rmaInfo);
                foreach (RMARegisterInfo rmaItem in rmaInfo.ItemHash.Values)
                {
                    rmaItem.SysNo = SequenceDac.GetInstance().Create("RMA_Register_Sequence");
                    rmaItem.Status = (int)AppEnum.RMARequestStatus.Orgin;
                    rmaItem.OwnBy = (int)AppEnum.RMAOwnBy.Origin;
                    rmaItem.Location = (int)AppEnum.RMALocation.Origin;
                    RMARegisterManager.GetInstance().InsertRegister(rmaItem);
                    RMARequestItemInfo rmaiteminfo = new RMARequestItemInfo();
                    rmaiteminfo.RegisterSysNo = rmaItem.SysNo;
                    rmaiteminfo.RequestSysNo = rmaInfo.SysNo;
                    this.InsertRequestItem(rmaiteminfo);
                }

                scope.Complete();
            }
            //SendMail(rmaInfo);
        }

        /// <summary>
        /// SendMail to customer when a rma request order is created successfully
        /// </summary>
        /// <param name="sysno"></param>
        /// <returns></returns>
        public void SendMail(RMARequestInfo rmaInfo)
        {
            //get mail information
            string mailaddress;
            string mailsubject;
            string mailbodyinfo;

            string requestId;
            int sosysno;
            string strSOID;

            string requestInfo;
            string allRequestInfo = null;


            CustomerInfo oCustomer = new CustomerInfo();
            oCustomer = CustomerManager.GetInstance().Load(rmaInfo.CustomerSysNo);
            if (Util.TrimNull(oCustomer.Email) == AppConst.StringNull)
                return;

            mailaddress = oCustomer.Email;

            requestId = rmaInfo.RequestID.ToString();

            sosysno = Util.TrimIntNull(rmaInfo.SOSysNo);
            strSOID = GetSOIDFromSOMaster(sosysno);

            mailsubject = "您已成功申请返修单" + requestId + " ";

            mailbodyinfo = @"<span style='font-size:12.0pt;font-family:方正姚体'>尊敬的ORS商城网用户，<br>
                                        <br>
                                        您好！
                                        <br>
                                        <br>                                        
                                        您已成功的在线申请了RMA返还单号@requestID。当您返还货品时请您将对应产品的发票（复印件无效），同时保证产品完整，包括原来的包装物、用户手册，和其他由厂商提供的一切附件。如果产品某一部分丢失，则不符合ORS商城的保修条款，在您补全所有发票及附件之前，我们无法将产品送交厂家进行保修。ORS商城网无法承担用户补寄附件或发票所产生的费用；由于产品不全而造成的保修延误，ORS商城网不承担责任。 
                                        <br><br>
                                        RMA 返还单详细目录<br>
                                        <br>
                                        订单号： SO @SOID；<br> @allRequestInfo
                                        <br>                                
                                        如您选择保修，在ORS商城完成保修过程后将以原订单的地址帮您返还。 
                                        如您的地址有变动，请及时与我们的客服热线 400-820-1878 
                                        取得联系更新返还地址。因地址未及时的更新而导致的延误等后果，ORS商城网概不负责。<br>
                                                                                <br>
                                                                                
                                        请您及时的将返还件以邮局普通包裹*或其他方式交至ORS商城网。邮政普通包裹费用**由ORS商城负责，我们会在收到货后以积分的形式将邮寄费用存入您在ORS商城网的帐户中。<br>
                                                                                * 
                                        即指平邮包裹形式，而非快递包裹或EMS。如果客户采用其他运输方式，则运费由客户自己负责。<br>
                                                                                ** 仅包含邮局资费，不包含包装费、 
                                        挂号费、保价费；并需要提供邮局发票，收据不能作为ORS商城承担运费的凭证。 
                                        产品维修好后，由我们将货物交还客户。<br>
                                                                             <br>                                                                                
                                        ORS商城在收到您的返还件后,ORS商城网将严格按照国家三包法规进行处理。如有需要，可联系ORS商城网客服热线 400-820-1878
                                        周一至周六 上午8：30 － 下午17：30。 <br>
                                        <br>
                                        <br>
                                        谢谢支持！<br>
                                        <br>
                                        <br>
                                        中国ORS商城网<br><br></span>";



            DataSet registerInfo = RMARequestManager.GetInstance().GetRegisterByRequest(Util.TrimIntNull(rmaInfo.SysNo));

            foreach (DataRow row in registerInfo.Tables[0].Rows)
            {
                requestInfo = @"产品号： @ProductId （@ProductName）<br>
                                返还件种类：@RequestType<br>
                                返还原因/故障描述：@reason<br> ";
                string requesttypeDesc = AppEnum.GetRMARequestType(row["RequestType"]);
                requestInfo = requestInfo.Replace("@ProductId", row["ProductId"].ToString()).Replace("@ProductName", row["ProductName"].ToString()).Replace("@RequestType", requesttypeDesc.ToString()).Replace("@reason", row["CustomerDesc"].ToString());
                allRequestInfo = allRequestInfo + requestInfo;
            }

            mailbodyinfo = mailbodyinfo.Replace("@requestID", requestId);
            mailbodyinfo = mailbodyinfo.Replace("@SOID", strSOID);
            mailbodyinfo = mailbodyinfo.Replace("@allRequestInfo", allRequestInfo.ToString());


            //send mail
            EmailInfo oMail = new EmailInfo();

            oMail.MailAddress = mailaddress;
            oMail.MailSubject = mailsubject;

            oMail.MailBody = mailbodyinfo;
            oMail.Status = (int)AppEnum.TriStatus.Origin;

            //EmailManager.GetInstance().InsertEmail(oMail);
        }

        public string GetSOIDFromSOMaster(int sosysno)
        {
            string soid = null;
            string sql = @"select SOID from V_SO_Master [NOLOCK] where SysNo=" + sosysno + " ";

            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (Util.HasMoreRow(ds))
                soid = ds.Tables[0].Rows[0]["SOID"].ToString();

            return soid;
        }


        public string BuildRMAID(int rmarequestSysNo)
        {
            return "R0" + rmarequestSysNo.ToString().PadLeft(8, '0');
        }

        public DataSet GetRequestList(Hashtable paramHash)
        {
            string sql = @"select RMA_Request.* , SO_Master.SOID , Customer.CustomerID ,Customer.CustomerName ,su1.UserName as RecvUser ,su2.UserName as CreateUser ,Customer.VIPRank 
                           from   RMA_Request (NOLOCK)
                                  left join V_SO_Master SO_master (NOLOCK) on SO_Master.sysno = RMA_Request.SOSysNo
                                  left join Customer (NOLOCK) on RMA_Request.CustomerSysNo = Customer.SysNo                                  
                                  left join Sys_User as su1 (NOLOCK) on RMA_Request.RecvUserSysNo = su1.SysNo
                                  left join Sys_User as su2 (NOLOCK) on RMA_Request.CreateUserSysNo = su2.SysNo
                           where  1=1  @DateTo  @DateFrom @RecvFrom @RecvTo  @SOID @RequestID @SysNo @ETakeFrom @ETakeTo @IsVIP @RevertSysNo
                                  @CustomerSysNo @CustomerID @CanDoorGet @CreateUserSysNo @Status @RMAReceiveType order by @GroupBy desc";

            if (paramHash.ContainsKey("DateTo"))
                sql = sql.Replace("@DateTo", " and RMA_Request.CreateTime <=" + Util.ToSqlEndDate(paramHash["DateTo"].ToString()));
            else
                sql = sql.Replace("@DateTo", "");

            if (paramHash.ContainsKey("DateFrom"))
                sql = sql.Replace("@DateFrom", " and RMA_Request.CreateTime >=" + Util.ToSqlString(paramHash["DateFrom"].ToString()));
            else
                sql = sql.Replace("@DateFrom", "");

            if (paramHash.ContainsKey("RecvFrom"))
                sql = sql.Replace("@RecvFrom", " and RMA_Request.RecvTime >=" + Util.ToSqlString(paramHash["RecvFrom"].ToString()));
            else
                sql = sql.Replace("@RecvFrom", "");

            if (paramHash.ContainsKey("RecvTo"))
                sql = sql.Replace("@RecvTo", " and RMA_Request.RecvTime <=" + Util.ToSqlEndDate(paramHash["RecvTo"].ToString()));
            else
                sql = sql.Replace("@RecvTo", "");

            if (paramHash.ContainsKey("SOID"))
                sql = sql.Replace("@SOID", " and SO_Master.SOID like " + Util.ToSqlLikeString(paramHash["SOID"].ToString()));
            else
                sql = sql.Replace("@SOID", "");

            if (paramHash.ContainsKey("RequestID"))
                sql = sql.Replace("@RequestID", " and RMA_Request.RequestID like " + Util.ToSqlLikeString(paramHash["RequestID"].ToString()));
            else
                sql = sql.Replace("@RequestID", "");

            if (paramHash.ContainsKey("SysNo"))
                sql = sql.Replace("@SysNo", " and RMA_Request.SysNo = " + Util.ToSqlString(paramHash["SysNo"].ToString()));
            else
                sql = sql.Replace("@SysNo", "");

            if (paramHash.ContainsKey("ETakeFrom"))
                sql = sql.Replace("@ETakeFrom", " and RMA_Request.ETakeDate >=" + Util.ToSqlString(paramHash["ETakeFrom"].ToString()));
            else
                sql = sql.Replace("@ETakeFrom", "");

            if (paramHash.ContainsKey("ETakeTo"))
                sql = sql.Replace("@ETakeTo", " and RMA_Request.ETakeDate <=" + Util.ToSqlEndDate(paramHash["ETakeTo"].ToString()));
            else
                sql = sql.Replace("@ETakeTo", "");

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

            if (paramHash.ContainsKey("CustomerSysNo"))
            {
                sql = sql.Replace("@CustomerSysNo", "  and customer.SysNo =" + Util.ToSqlString(paramHash["CustomerSysNo"].ToString()));
            }
            else
            {
                sql = sql.Replace("@CustomerSysNo", "");
            }

            if (paramHash.ContainsKey("CustomerID"))
            {
                sql = sql.Replace("@CustomerID", "  and customer.CustomerID =" + Util.ToSqlString(paramHash["CustomerID"].ToString()));
            }
            else
            {
                sql = sql.Replace("@CustomerID", "");
            }

            if (paramHash.ContainsKey("CanDoorGet"))
            {
                sql = sql.Replace("@CanDoorGet", " and RMA_Request.CanDoorGet =" + Util.ToSqlString(paramHash["CanDoorGet"].ToString()));
            }
            else
            {
                sql = sql.Replace("@CanDoorGet", "");
            }

            if (paramHash.ContainsKey("CreateUserSysNo"))
            {
                sql = sql.Replace("@CreateUserSysNo", " and RMA_Request.CreateUserSysNo=" + Util.ToSqlString(paramHash["CreateUserSysNo"].ToString()));
            }
            else
            {
                sql = sql.Replace("@CreateUserSysNo","");
            }

            if (paramHash.ContainsKey("GroupBy"))
            {
                if (paramHash["GroupBy"].ToString() == "0")
                    sql = sql.Replace("@GroupBy", "RMA_Request.CreateTime");
                else
                    sql = sql.Replace("@GroupBy", "RMA_Request.RecvTime");
            }
            else
            {
                sql = sql.Replace("@GroupBy", "RMA_Request.CreateTime");
            }

            if (paramHash.ContainsKey("Status"))
            {
                sql = sql.Replace("@Status", " and RMA_Request.Status=" + Util.ToSqlString(paramHash["Status"].ToString()));
            }
            else
            {
                sql = sql.Replace("@Status", "");
            }
            if (paramHash.ContainsKey("RevertSysNo"))
            {
                sql = sql.Replace("@RevertSysNo", " and not exists (select RMA_Request_Item.RegisterSysNo from RMA_Request_Item where RMA_Request_Item.RequestSysNo =RMA_Request.sysno and RMA_Request_Item.RegisterSysNo  in (select RMA_Revert_Item.registersysno from RMA_Revert_Item where RevertSysNo=" + Util.ToSqlString(paramHash["RevertSysNo"].ToString()) + "))");
            }
            else
            {
                sql = sql.Replace("@RevertSysNo", "");
            }
            if (paramHash.ContainsKey("RMAReceiveType"))
            {
                sql = sql.Replace("@RMAReceiveType", "and RMA_Request.ReceiveType= " + Util.ToSqlString(paramHash["RMAReceiveType"].ToString()));
            }
            else
            {
                sql = sql.Replace("@RMAReceiveType", "");
            }

            if (paramHash == null || paramHash.Count == 1)
                sql = sql.Replace("select", "select top 50 ");

            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetRMARequestCreateUserList(DateTime dtCreateFrom, int CreateUserStatus)
        {
            string sql = "select distinct su.sysno as createusersysno,su.username as createusername from sys_user su inner join rma_request rr on su.sysno=rr.createusersysno where 1=1";
            if (dtCreateFrom != AppConst.DateTimeNull)
            {
                sql += " and rr.createtime >= " + Util.ToSqlString(dtCreateFrom.ToString(AppConst.DateFormat));
            }
            if (CreateUserStatus != AppConst.IntNull)
            {
                sql += " and su.status=" + Util.ToSqlString(CreateUserStatus.ToString());
            }
            sql += " order by su.username";
            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetRegisterByRequest(int sysno)
        {
            string sql = @"select RMA_Register.* ,Product.ProductName ,Product.ProductID,Product.SysNo as ProductSysNo
                            from RMA_Register (NOLOCK) 
							inner join Product (NOLOCK) on Product.sysno = RMA_Register.ProductSysNo 
							inner join RMA_Request_Item (NOLOCK) on RMA_Register.SysNo = RMA_Request_Item.RegisterSysNo
							inner join RMA_Request (NOLOCK) on RMA_Request_Item.RequestSysNo = RMA_Request.SysNo
							where RMA_Request.SysNo = " + sysno;
            return SqlHelper.ExecuteDataSet(sql);
        }



        //该方法会抛出异常，如果检查不通过的话。
        //CancelReceive前的检查，这里参考RMARegisterManager.UpdateRegister方法，取消申请单前的检查,暂时有这些逻辑。
        //目前逻辑更改如此：下面的条件都满足的情况才允许取消。
        //1、没有送修单或者有送修单且是作废的状态；
        //2、没有退款单或者有退款单且是作废状态；
        //3、没有发货单或者有发货单且是作废状态；
        //4、没有退货入库单或者有退货入库单且是作废状态。

        public bool CancelReceiveCheck(int requestSysNo)
        {
            string alarm = "";
            string sql = "";
            DataTable table = null;

            //送修单检查
            sql = @"select I.RegisterSysNo from rma_outbound_item I (nolock)
                    left join rma_outbound M (nolock) on M.SysNo=I.OutBoundSysNo 
                    left join rma_request_item Q (nolock) on Q.RegisterSysNo=I.RegisterSysNo
                    where Q.RequestSysNo={0} and M.Status!={1}";
            table = SqlHelper.ExecuteDataSet(String.Format(sql, requestSysNo, (int)AppEnum.RMAOutBoundStatus.Abandon)).Tables[0];
            if (table.Rows.Count > 0)
            {
                alarm += "下面单据存在送修单：<br>&nbsp;&nbsp;";
                foreach (DataRow row in table.Rows)
                {
                    alarm += Util.TrimIntNull(row["RegisterSysNo"]).ToString() + ",";
                }
                alarm += "<br>";
            }
            //发货单检查
            sql = @"select I.RegisterSysNo from rma_revert_item I (nolock)
                    left join rma_revert M (nolock) on M.SysNo=I.RevertSysNo 
                    left join rma_request_item Q (nolock) on Q.RegisterSysNo=I.RegisterSysNo
                    where Q.RequestSysNo={0} and M.Status!={1}";
            table = SqlHelper.ExecuteDataSet(String.Format(sql, requestSysNo, (int)AppEnum.RMARevertStatus.Abandon)).Tables[0];
            if (table.Rows.Count > 0)
            {
                alarm += "下面单据存在发货单：<br>&nbsp;&nbsp;";
                foreach (DataRow row in table.Rows)
                {
                    alarm += Util.TrimIntNull(row["RegisterSysNo"]).ToString() + ",";
                }
                alarm += "<br>";
            }
            //退款单检查
            sql = @"select I.RegisterSysNo from rma_refund_item I (nolock)
                    left join rma_refund M (nolock) on M.SysNo=I.RefundSysNo 
                    left join rma_request_item Q (nolock) on Q.RegisterSysNo=I.RegisterSysNo
                    where Q.RequestSysNo={0} and M.Status!={1}";
            table = SqlHelper.ExecuteDataSet(String.Format(sql, requestSysNo, (int)AppEnum.RMARefundStatus.Abandon)).Tables[0];
            if (table.Rows.Count > 0)
            {
                alarm += "下面单据存在退款单：<br>&nbsp;&nbsp;";
                foreach (DataRow row in table.Rows)
                {
                    alarm += Util.TrimIntNull(row["RegisterSysNo"]).ToString() + ",";
                }
                alarm += "<br>";
            }
            //退货入库
            sql = @"select I.RegisterSysNo from rma_return_item I (nolock)
                    left join rma_return M (nolock) on M.SysNo=I.ReturnSysNo 
                    left join rma_request_item Q (nolock) on Q.RegisterSysNo=I.RegisterSysNo
                    where Q.RequestSysNo={0} and M.Status!={1}";
            table = SqlHelper.ExecuteDataSet(String.Format(sql, requestSysNo, (int)AppEnum.RMAReturnStatus.Abandon)).Tables[0];
            if (table.Rows.Count > 0)
            {
                alarm += "下面单据存在退货入库单：<br>&nbsp;&nbsp;";
                foreach (DataRow row in table.Rows)
                {
                    alarm += Util.TrimIntNull(row["RegisterSysNo"]).ToString() + ",";
                }
                alarm += "<br>";
            }
            if (alarm != "") throw new BizException(alarm + "当前状态不允许进行Cancel Receive操作！");

            //throw new BizException("允许取消。这里以后要取消的代码。");
            return true;
        }
        public void CancelReceive(int sysno)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                Hashtable ht = new Hashtable();
                ht.Add("SysNo", sysno);
                ht.Add("Status", (int)AppEnum.RMARequestStatus.Orgin);
                this.UpdateRequest(ht);

                new RMARequestDac().CancelReceive(sysno);

                string sql = @"select rma_request_item.RegisterSysNo as registersysno ,V_SO_Item.Cost as Cost,
                                       rma_register.receivestocksysno,rma_register.productsysno 
                                       from rma_request_item (nolock) 
                                          inner join rma_request (nolock) on rma_request_item.requestsysno=rma_request.sysno
                                          inner join v_so_master (nolock) on v_so_master.sysno=rma_request.sosysno 
                                          inner join v_so_item (nolock) on v_so_item.sosysno=v_so_master.sysno
                                          inner join rma_register (nolock) on (rma_register.productsysno=v_so_item.productsysno and rma_register.sysno=rma_request_item.registersysno)   
                                        where requestsysno = " + sysno;
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (Util.HasMoreRow(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Hashtable htRegister = new Hashtable();
                        htRegister.Add("SysNo", Util.TrimIntNull(dr["registersysno"]));
                        htRegister.Add("OwnBy", (int)AppEnum.RMAOwnBy.Origin);
                        htRegister.Add("Location", (int)AppEnum.RMALocation.Origin);
                        htRegister.Add("Cost", AppConst.DecimalNull);
                        RMARegisterManager.GetInstance().UpdateRegister(htRegister);

                        //收货更新库存 -- lucky 03/13/2008
                        InventoryManager.GetInstance().SetInStockQty(Util.TrimIntNull(dr["receivestocksysno"]), Util.TrimIntNull(dr["productsysno"]), -1);
                    }
                }

                scope.Complete();
            }
        }

        public RMARequestInfo Load(int sysno)
        {
            try
            {
                RMARequestInfo oInfo = null;
                string sql = "select * from RMA_Request (NOLOCK) where sysno=" + sysno;
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (Util.HasMoreRow(ds))
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    oInfo = MapRequest(dr);
                    string itemSql = @"select RMA_Register.* 
                                       from   RMA_Request_Item (NOLOCK)
                                       inner join  RMA_Register (NOLOCK) on RMA_Register.sysno = RMA_Request_Item.RegisterSysNO 
                                       where RMA_Request_Item.RequestSysNo=" + sysno;
                    DataSet itemds = SqlHelper.ExecuteDataSet(itemSql);
                    if (Util.HasMoreRow(itemds))
                    {
                        foreach (DataRow itemdr in itemds.Tables[0].Rows)
                        {
                            RMARegisterInfo oRegister = new RMARegisterInfo();
                            mapRegister(oRegister, itemdr);
                            oInfo.ItemHash.Add(Util.TrimIntNull(itemdr["SysNo"]), oRegister);
                        }
                    }
                }
                return oInfo;
            }
            catch
            {
                throw new BizException("Load RMARequestInfo Error!");
            }

        }
        /// <summary>
        /// 用于前台网站RMA查询
        /// </summary>
        /// <param name="sysno"></param>
        /// <returns></returns>
        public RMARequestInfo GetRequestInfo(int sysno)
        {
            try
            {
                RMARequestInfo oInfo = null;
                string sql = "select * from RMA_Request (NOLOCK) where sysno=" + sysno;
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (Util.HasMoreRow(ds))
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    oInfo = MapRequest(dr);
                    string itemSql = @"select RMA_Register.* 
                                       from   RMA_Request_Item (NOLOCK)
                                       inner join  RMA_Register (NOLOCK) on RMA_Register.sysno = RMA_Request_Item.RegisterSysNO 
                                       where RMA_Request_Item.RequestSysNo=" + sysno;
                    DataSet itemds = SqlHelper.ExecuteDataSet(itemSql);
                    if (Util.HasMoreRow(itemds))
                    {
                        foreach (DataRow itemdr in itemds.Tables[0].Rows)
                        {
                            RMARegisterInfo oRegister = new RMARegisterInfo();
                            mapReg(oRegister, itemdr);
                            oInfo.ItemHash.Add(Util.TrimIntNull(itemdr["SysNo"]), oRegister);
                        }
                    }
                }
                return oInfo;
            }
            catch
            {
                throw new BizException("Load RMARequestInfo Error!");
            }

        }
        public void mapReg(RMARegisterInfo oInfo, DataRow dr)
        {
            oInfo.SysNo = Util.TrimIntNull(dr["SysNo"]);
            oInfo.ProductSysNo = Util.TrimIntNull(dr["ProductSysNo"]);
            oInfo.RequestType = Util.TrimIntNull(dr["RequestType"]);
            oInfo.CustomerDesc = Util.TrimNull(dr["CustomerDesc"]);
            oInfo.CheckDesc = Util.TrimNull(dr["CheckDesc"]);
            oInfo.NewProductStatus = Util.TrimIntNull(dr["NewProductStatus"]);
            oInfo.RevertStatus = Util.TrimIntNull(dr["RevertStatus"]);
            oInfo.OutBoundStatus = Util.TrimIntNull(dr["OutBoundStatus"]);
            oInfo.ReturnStatus = Util.TrimIntNull(dr["ReturnStatus"]);
            oInfo.ResponseDesc = Util.TrimNull(dr["ResponseDesc"]);
            oInfo.ResponseTime = Util.TrimDateNull(dr["ResponseTime"]);
            oInfo.NextHandler = Util.TrimIntNull(dr["NextHandler"]);
            oInfo.Memo = Util.TrimNull(dr["Memo"]);
            oInfo.RMAReason = Util.TrimIntNull(dr["RMAReason"]);
            oInfo.Status = RMARegisterManager.GetInstance().GetStatusValue(Util.TrimIntNull(dr["SysNo"]));
            oInfo.ProductNo = Util.TrimNull(dr["ProductNo"]);
        }

        private RMARequestInfo MapRequest(DataRow dr)
        {
            RMARequestInfo oInfo = new RMARequestInfo();
            oInfo.Address = dr["Address"].ToString();
            oInfo.Contact = dr["Contact"].ToString();
            oInfo.CreateTime = Util.TrimDateNull(dr["CreateTime"]);
            oInfo.CreateUserSysNo = Util.TrimIntNull(dr["CreateUserSysNo"]);
            oInfo.CanDoorGet = Util.TrimIntNull(dr["CanDoorGet"]);
            oInfo.DoorGetFee = Util.TrimDecimalNull(dr["DoorGetFee"]);
            oInfo.CustomerSysNo = Util.TrimIntNull(dr["CustomerSysNo"]);
            oInfo.Memo = dr["Memo"].ToString();
            oInfo.Note = dr["Note"].ToString();
            oInfo.Phone = dr["Phone"].ToString();
            oInfo.RecvTime = Util.TrimDateNull(dr["RecvTime"]);
            oInfo.RecvUserSysNo = Util.TrimIntNull(dr["RecvUserSysNo"]);
            oInfo.RequestID = dr["RequestID"].ToString();
            oInfo.SOSysNo = Util.TrimIntNull(dr["SOSysNo"]);
            oInfo.Status = Util.TrimIntNull(dr["Status"]);
            oInfo.SysNo = Util.TrimIntNull(dr["SysNo"]);
            oInfo.ETakeDate = Util.TrimDateNull(dr["ETakeDate"]);
            oInfo.AreaSysNo = Util.TrimIntNull(dr["AreaSysNo"]);

            oInfo.FreightUserSysNo = Util.TrimIntNull(dr["FreightUserSysNo"]);
            oInfo.SetDeliveryManTime = Util.TrimDateNull(dr["SetDeliveryManTime"]);

            oInfo.IsRevertAddress = Util.TrimIntNull(dr["IsRevertAddress"]);
            oInfo.RevertAddress = Util.TrimNull(dr["RevertAddress"]);
            oInfo.RevertAreaSysNo = Util.TrimIntNull(dr["RevertAreaSysNo"]);
            oInfo.RevertZip = Util.TrimNull(dr["RevertZiP"]);

            oInfo.Zip = Util.TrimNull(dr["Zip"]);
            oInfo.RevertContact = Util.TrimNull(dr["RevertContact"]);
            oInfo.RevertContactPhone = Util.TrimNull(dr["RevertContactPhone"]);
            oInfo.ReceiveType = Util.TrimIntNull(dr["ReceiveType"]);
            return oInfo;
        }

        public int GetCurrentStatus(int sysno)
        {
            string sql = "select status from rma_request (NOLOCK) where sysno = " + sysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
            {
                throw new BizException("Get Current Status Error!");
            }
            DataRow dr = ds.Tables[0].Rows[0];
            return Util.TrimIntNull(dr["status"]);
        }
        /// <summary>
        /// 判断是否有收获情况选项全部选中
        /// </summary>
        /// <param name="oInfo"></param>
        /// <returns></returns>
        public bool GetSetRecInfo(int sysno)
        {
            int countNum = 0;
            string sql = @"select IsHaveInvoice,IsFullAccessory,IsFullPackage
                                       from rma_request_item (nolock) 
                                          inner join rma_request (nolock) on rma_request_item.requestsysno=rma_request.sysno
                                          inner join rma_register (nolock) on  rma_register.sysno=rma_request_item.registersysno  
                                      where requestsysno = " + sysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
            {
                throw new BizException("当前记录错误");
            }
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Util.TrimIntNull(dr["IsHaveInvoice"]) == AppConst.IntNull || Util.TrimIntNull(dr["IsFullAccessory"]) == AppConst.IntNull || Util.TrimIntNull(dr["IsFullPackage"]) == AppConst.IntNull)
                    countNum = countNum + 1;
            }
            if (countNum > 0)
                return true;
            else
                return false;
        }

        public string CheckSO(RMARequestInfo oInfo)
        {
            //是否已存在该SO中该商品的RMA单
            //注意：考虑到SO中多个商品的情况，判断已存在的在RMA处理的商品数量，如果小于SO中商品数量，则仍旧可以添加RMA申请
            string result = AppConst.StringNull;
            foreach (RMARegisterInfo oRegister in oInfo.ItemHash.Values)
            {
                string sql = @"select count(*) as rmaqty
                               from  rma_request (NOLOCK) 
                               inner join rma_request_item (NOLOCK) on rma_request.sysno = rma_request_item.requestsysno
                               inner join rma_register (NOLOCK) on rma_register.sysno = rma_request_item.registersysno
                               where rma_request.sosysno = " + oInfo.SOSysNo + " and rma_register.productsysno=" + oRegister.ProductSysNo
                    + " and (rma_register.status = " + (int)AppEnum.RMARequestStatus.Orgin + " or rma_register.status = " + (int)AppEnum.RMARequestStatus.Handling + ")";

                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                DataRow dr = ds.Tables[0].Rows[0];

                int rmaqty = Util.TrimIntNull(dr["rmaqty"]);
                string sqlso = @"select quantity 
                                 from v_so_item (NOLOCK) 
                                 where v_so_item.sosysno = " + oInfo.SOSysNo + " and productsysno=" + oRegister.ProductSysNo; 
                DataSet dsso = SqlHelper.ExecuteDataSet(sqlso);

                int soqty = 0;
                if (Util.HasMoreRow(dsso))
                {
                    DataRow drso = dsso.Tables[0].Rows[0];
                    soqty = Util.TrimIntNull(drso["quantity"]);
                }

                if (rmaqty >= soqty)
                {
                    ProductBasicInfo oProduct = ProductManager.GetInstance().LoadBasic(oRegister.ProductSysNo);
                    if (result != AppConst.StringNull)
                        result += "<br>";
                    result += oProduct.ProductName + "(" + oProduct.ProductID + ")";
                }
            }
            return result;
        }
        /// <summary>
        /// 如果是VIP客户申请RMARequest则，第3天第7天第12天提醒RMA和CC优先处理
        /// </summary>
        public void SendVipRMARequestToCCorRMA()
        {
            try
            {
                DateTime todayDate = DateTime.Today;
                //3 day ago
                string sql3 = @"select RMA_Register.* ,RMA_Request.CustomerSysNo,RMA_Request.SysNo as RequestSysNo,Product.ProductName ,Product.ProductID
                          from RMA_Register (NOLOCK) 
                          inner join Product (NOLOCK) on Product.sysno = RMA_Register.ProductSysNo
                          inner join RMA_Request_Item (NOLOCK) on RMA_Register.SysNo = RMA_Request_Item.RegisterSysNo
						  inner join RMA_Request (NOLOCK) on RMA_Request_Item.RequestSysNo = RMA_Request.SysNo
                          inner join Customer (NOLOCK)  on RMA_Request.CustomerSysNo = Customer.SysNo 
                           where (Customer.VIPRank=" + (int)AppEnum.CustomerVIPRank.AutoVIP + " or Customer.VIPRank=" + (int)AppEnum.CustomerVIPRank.ManualVIP + ")   and   RMA_Request.status=" + (int)AppEnum.RMARequestStatus.Handling + " and (RMA_Request.RecvTime between '" + todayDate.AddDays(-3).ToString() + "' and '" + todayDate.AddDays(-2).ToString() + "') order by RequestSysNo desc ";

                DataSet ds3 = SqlHelper.ExecuteDataSet(sql3);

                StringBuilder sb3 = new StringBuilder(1000);
                sb3.Append("<table align=center>３天前的ＶＩＰ客户申请单列表：</table>");
                sb3.Append("<br>");
                sb3.Append("<table align=center border=1 cellspacing=0 cellspadding=0>");
                if (!Util.HasMoreRow(ds3))
                {
                    sb3.Append("<tr>");
                    sb3.Append("<td width=100%><font color=#ff0000>记录为空！</font></td>");
                    sb3.Append("</tr>");
                }
                else if (Util.HasMoreRow(ds3))
                {
                    sb3.Append("<tr>");
                    sb3.Append("<td>客户号</td>");
                    sb3.Append("<td>申请单号</td>");
                    sb3.Append("<td>单件号</td>");
                    sb3.Append("<td>产品号</td>");
                    sb3.Append("<td>产品名称</td>");
                    sb3.Append("<td>返修/返回原因");
                    sb3.Append("<td>备忘录</td>");
                    sb3.Append("</tr>");

                }
                foreach (DataRow dr in ds3.Tables[0].Rows)
                {
                    sb3.Append("<tr>");
                    sb3.Append("<td>" + dr["CustomerSysNo"] + "</td>");
                    sb3.Append("<td>" + dr["RequestSysNo"] + "</td>");
                    sb3.Append("<td>" + dr["SysNo"] + "</td>");
                    sb3.Append("<td>" + dr["ProductID"] + "</td>");
                    sb3.Append("<td>" + dr["ProductName"] + "</td>");
                    sb3.Append("<td>" + dr["CustomerDesc"] + "</td>");
                    sb3.Append("<td>" + dr["Memo"] + "</td>");
                    sb3.Append("</tr>");
                }
                sb3.Append("</table>");
                sb3.Append("<br>");
                //7 day ago
                string sql7 = @"select RMA_Register.* ,RMA_Request.CustomerSysNo,RMA_Request.SysNo as RequestSysNo,Product.ProductName ,Product.ProductID
                          from RMA_Register (NOLOCK) 
                          inner join Product (NOLOCK) on Product.sysno = RMA_Register.ProductSysNo
                          inner join RMA_Request_Item (NOLOCK) on RMA_Register.SysNo = RMA_Request_Item.RegisterSysNo
						  inner join RMA_Request (NOLOCK) on RMA_Request_Item.RequestSysNo = RMA_Request.SysNo
                          inner join Customer (NOLOCK)  on RMA_Request.CustomerSysNo = Customer.SysNo 
                           where (Customer.VIPRank=" + (int)AppEnum.CustomerVIPRank.AutoVIP + " or Customer.VIPRank=" + (int)AppEnum.CustomerVIPRank.ManualVIP + ")   and   RMA_Request.status=" + (int)AppEnum.RMARequestStatus.Handling + " and  (RMA_Request.RecvTime between '" + todayDate.AddDays(-7).ToString() + "' and '" + todayDate.AddDays(-6).ToString() + "') order by RequestSysNo desc ";

                DataSet ds7 = SqlHelper.ExecuteDataSet(sql7);

                StringBuilder sb7 = new StringBuilder(1000);
                sb7.Append("<table align=center>7天前的ＶＩＰ客户申请单列表：</table>");
                sb7.Append("<br>");
                sb7.Append("<table align=center border=1 cellspacing=0 cellspadding=0>");
                if (!Util.HasMoreRow(ds7))
                {
                    sb7.Append("<tr>");
                    sb7.Append("<td width=100%><font color=#ff0000>记录为空！</font></td>");
                    sb7.Append("</tr>");
                }
                else if (Util.HasMoreRow(ds7))
                {
                    sb7.Append("<tr>");
                    sb7.Append("<td>客户号</td>");
                    sb7.Append("<td>申请单号</td>");
                    sb7.Append("<td>单件号</td>");
                    sb7.Append("<td>产品号</td>");
                    sb7.Append("<td>产品名称</td>");
                    sb7.Append("<td>返修/返回原因");
                    sb7.Append("<td>备忘录</td>");
                    sb7.Append("</tr>");

                }
                foreach (DataRow dr in ds7.Tables[0].Rows)
                {
                    sb7.Append("<tr>");
                    sb7.Append("<td>" + dr["CustomerSysNo"] + "</td>");
                    sb7.Append("<td>" + dr["RequestSysNo"] + "</td>");
                    sb7.Append("<td>" + dr["SysNo"] + "</td>");
                    sb7.Append("<td>" + dr["ProductID"] + "</td>");
                    sb7.Append("<td>" + dr["ProductName"] + "</td>");
                    sb7.Append("<td>" + dr["CustomerDesc"] + "</td>");
                    sb7.Append("<td>" + dr["Memo"] + "</td>");
                    sb7.Append("</tr>");
                }
                sb7.Append("</table>");
                sb7.Append("<br>");

                //12 day ago

                string sql12 = @"select RMA_Register.* ,RMA_Request.CustomerSysNo,RMA_Request.SysNo as RequestSysNo,Product.ProductName ,Product.ProductID
                          from RMA_Register (NOLOCK) 
                          inner join Product (NOLOCK) on Product.sysno = RMA_Register.ProductSysNo
                          inner join RMA_Request_Item (NOLOCK) on RMA_Register.SysNo = RMA_Request_Item.RegisterSysNo
						  inner join RMA_Request (NOLOCK) on RMA_Request_Item.RequestSysNo = RMA_Request.SysNo
                          inner join Customer (NOLOCK)  on RMA_Request.CustomerSysNo = Customer.SysNo 
                           where (Customer.VIPRank=" + (int)AppEnum.CustomerVIPRank.AutoVIP + " or Customer.VIPRank=" + (int)AppEnum.CustomerVIPRank.ManualVIP + ")   and   RMA_Request.status=" + (int)AppEnum.RMARequestStatus.Handling + " and (RMA_Request.RecvTime between '" + todayDate.AddDays(-12).ToString() + "' and '" + todayDate.AddDays(-11).ToString() + "')  order by RequestSysNo desc ";

                DataSet ds12 = SqlHelper.ExecuteDataSet(sql12);

                StringBuilder sb12 = new StringBuilder(1000);
                sb12.Append("<table align=center>12天前的ＶＩＰ客户申请单列表：</table>");
                sb12.Append("<br>");
                sb12.Append("<table align=center border=1 cellspacing=0 cellspadding=0>");

                if (!Util.HasMoreRow(ds12))
                {
                    sb12.Append("<tr>");
                    sb12.Append("<td width=100%><font color=#ff0000>记录为空！</font></td>");
                    sb12.Append("</tr>");
                }
                else if (Util.HasMoreRow(ds12))
                {
                    sb12.Append("<tr>");
                    sb12.Append("<td>客户号</td>");
                    sb12.Append("<td>申请单号</td>");
                    sb12.Append("<td>单件号</td>");
                    sb12.Append("<td>产品号</td>");
                    sb12.Append("<td>产品名称</td>");
                    sb12.Append("<td>返修/返回原因");
                    sb12.Append("<td>备忘录</td>");
                    sb12.Append("</tr>");

                }
                foreach (DataRow dr in ds12.Tables[0].Rows)
                {
                    sb12.Append("<tr>");
                    sb12.Append("<td>" + dr["CustomerSysNo"] + "</td>");
                    sb12.Append("<td>" + dr["RequestSysNo"] + "</td>");
                    sb12.Append("<td>" + dr["SysNo"] + "</td>");
                    sb12.Append("<td>" + dr["ProductID"] + "</td>");
                    sb12.Append("<td>" + dr["ProductName"] + "</td>");
                    sb12.Append("<td>" + dr["CustomerDesc"] + "</td>");
                    sb12.Append("<td>" + dr["Memo"] + "</td>");
                    sb12.Append("</tr>");
                }
                sb12.Append("</table>");
                sb12.Append("<br>");


                //send mail
                //if (AppConfig.RMAandCCMail == AppConst.StringNull)
                //    return;

                //TCPMail oMail = new TCPMail();
                //string mailaddress = AppConfig.RMAandCCMail;
                //string mailSubject = "需要优先处理的VIP客户RMA申请单列表";
                //string mailBody = sb3.ToString() + sb7.ToString() + sb12.ToString();
                //oMail.Send(mailaddress, mailSubject, mailBody);
            }
            catch
            {
            }

        }
        /// <summary>
        /// </summary>
        /// <param name="soSysNo"></param>
        /// <returns></returns>
        public void SendRMAEveryDayList()
        {

            //今日ＲＭＡ情况汇总表

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int day = DateTime.Now.AddDays(-1).Day;
            DateTime today = DateTime.Today;

            //今日接受件
            string listsql1 = @"select  rma_register.SysNo,product.productid,product.productname,rma_register.CustomerDesc,rma_register.status,product.PMUserSysNo,vendor.VendorName,lpi.LastVendorSysNo,rma_register.Cost from rma_request (NOLOCK) 
                             inner join rma_request_item (NOLOCK) on rma_request.sysno = rma_request_item.requestsysno
                             inner join rma_register (NOLOCK) on rma_register.sysno = rma_request_item.registersysno 
                             inner join product (NOLOCK) on rma_register.productsysno = product.sysno
                             left join product_lastpoinfo as lpi (NOLOCK) on product.sysno = lpi.productsysno
                             left join vendor (NOLOCK) on vendor.sysno = lpi.lastvendorsysno
                             where rma_register.status=" + (int)AppEnum.RMARequestStatus.Handling + " and rma_request.recvtime between '" + today.AddDays(-1) + "' and '" + today + "' order by rma_register.SysNo desc";

            DataSet listds1 = SqlHelper.ExecuteDataSet(listsql1);
            int RecRegisterNum = (int)listds1.Tables[0].Rows.Count;

            decimal RecRegisterCost = 0;
            foreach (DataRow dr in listds1.Tables[0].Rows)
            {
                if (Util.TrimDecimalNull(dr["Cost"]) != AppConst.DecimalNull)
                    RecRegisterCost = RecRegisterCost + Util.TrimDecimalNull(dr["Cost"]);
            }

            StringBuilder sb1 = new StringBuilder(1000);
            sb1.Append("<table><font color=blue>当日接受件</font></table>");
            sb1.Append("<br>");
            sb1.Append("共" + RecRegisterNum + "件,共" + RecRegisterCost.ToString(AppConst.DecimalFormatWithGroup) + "元");
            sb1.Append("<br>");
            sb1.Append("<table border=1 cellspacing=0 cellspadding=0>");
            if (!Util.HasMoreRow(listds1))
            {
                sb1.Append("<tr>");
                sb1.Append("<td width=100%><font color=#ff0000>记录为空！</font></td>");
                sb1.Append("</tr>");
            }
            else if (Util.HasMoreRow(listds1))
            {
                sb1.Append("<tr>");
                sb1.Append("<td>单号</td>");
                sb1.Append("<td>产品号</td>");
                sb1.Append("<td>商品名称</td>");
                sb1.Append("<td>故障描述</td>");
                sb1.Append("<td>处理状态</td>");
                sb1.Append("<td>PM</td>");
                sb1.Append("<td>供应商</td>");
                sb1.Append("<td>金额</td>");
                sb1.Append("</tr>");
            }
            foreach (DataRow dr in listds1.Tables[0].Rows)
            {
                string statusName = AppEnum.GetRMAStatus(Util.TrimIntNull(dr["status"]));
                string pmName = null;
                int PMSysNo = Util.TrimIntNull(dr["PMUserSysNo"]);

                UserInfo userinfo = SysManager.GetInstance().LoadUser(PMSysNo);

                if (userinfo != null)
                    pmName = Util.TrimNull(userinfo.UserName);
                else
                    pmName = AppConst.StringNull;

                string VendorName = null;
                if (Util.TrimNull(dr["VendorName"]) != AppConst.StringNull)
                    VendorName = Util.TrimNull(dr["VendorName"]).ToString();
                else
                    VendorName = "&nbsp;&nbsp;";

                string LastVendorSysNo = null;
                if (Util.TrimIntNull(dr["LastVendorSysNo"]) != AppConst.IntNull)
                    LastVendorSysNo = "(" + Util.TrimIntNull(dr["LastVendorSysNo"]).ToString() + ")";
                else
                    LastVendorSysNo = "&nbsp;&nbsp;";

                string Cost = null;
                if (Util.TrimDecimalNull(dr["Cost"]) != AppConst.DecimalNull)
                    Cost = Util.TrimDecimalNull(dr["Cost"]).ToString(AppConst.DecimalFormat);
                else
                    Cost = "&nbsp;&nbsp;";

                sb1.Append("<tr>");
                sb1.Append("<td>" + dr["SysNo"] + "</td>");
                sb1.Append("<td>" + dr["productid"] + "</td>");
                sb1.Append("<td width='250'>" + dr["productname"] + "</td>");
                sb1.Append("<td width='350'>" + dr["CustomerDesc"] + "</td>");
                sb1.Append("<td>" + statusName + "</td>");
                sb1.Append("<td>" + pmName + "</td>");
                sb1.Append("<td>" + VendorName + LastVendorSysNo + "</td>");
                sb1.Append("<td>" + Cost + "</td>");
                sb1.Append("</tr>");
            }

            sb1.Append("</table>");
            sb1.Append("<br>");

            //目前处理中的保修记录
            string listsql2 = @"select rma_register.SysNo,product.productid,product.productname,rma_register.CustomerDesc,rma_register.status ,product.PMUserSysNo,vendor.VendorName,lpi.LastVendorSysNo,rma_register.Cost from rma_request (NOLOCK) 
                             inner join rma_request_item (NOLOCK) on rma_request.sysno = rma_request_item.requestsysno
                             inner join rma_register (NOLOCK) on rma_register.sysno = rma_request_item.registersysno 
                             inner join product (NOLOCK) on rma_register.productsysno = product.sysno
                             left join product_lastpoinfo  as lpi (NOLOCK)  on product.sysno = lpi.productsysno
                             left join vendor (NOLOCK) on vendor.sysno = lpi.lastvendorsysno
                             where rma_register.status=" + (int)AppEnum.RMARequestStatus.Handling + " and rma_request.recvtime <='" + today + "' order by rma_register.SysNo desc";
            DataSet listds2 = SqlHelper.ExecuteDataSet(listsql2);

            int HandlingRegisterNum = (int)listds2.Tables[0].Rows.Count;

            decimal HandlingRegisterCost = 0;
            foreach (DataRow dr in listds2.Tables[0].Rows)
            {
                if (Util.TrimDecimalNull(dr["Cost"]) != AppConst.DecimalNull)
                    HandlingRegisterCost = HandlingRegisterCost + Util.TrimDecimalNull(dr["Cost"]);
            }

            StringBuilder sb2 = new StringBuilder(1000);
            sb2.Append("<table><font color=blue>目前在处理中的保修记录</font></table>");
            sb2.Append("<br>");
            sb2.Append("共" + HandlingRegisterNum + "件,共" + HandlingRegisterCost.ToString(AppConst.DecimalFormatWithGroup) + "元");
            sb2.Append("<br>");
            sb2.Append("<table  border=1 cellspacing=0 cellspadding=0>");
            if (!Util.HasMoreRow(listds2))
            {
                sb2.Append("<tr>");
                sb2.Append("<td width=100%><font color=#ff0000>记录为空！</font></td>");
                sb2.Append("</tr>");
            }
            else if (Util.HasMoreRow(listds2))
            {
                sb2.Append("<tr>");
                sb2.Append("<td>单号</td>");
                sb2.Append("<td>产品号</td>");
                sb2.Append("<td>商品名称</td>");
                sb2.Append("<td>故障描述</td>");
                sb2.Append("<td>处理状态</td>");
                sb2.Append("<td>PM</td>");
                sb2.Append("<td>供应商</td>");
                sb2.Append("<td>金额</td>");
                sb2.Append("</tr>");
            }
            foreach (DataRow dr in listds2.Tables[0].Rows)
            {
                string statusName = AppEnum.GetRMAStatus(Util.TrimIntNull(dr["status"]));
                string pmName = null;
                int PMSysNo = Util.TrimIntNull(dr["PMUserSysNo"]);

                UserInfo userinfo = SysManager.GetInstance().LoadUser(PMSysNo);

                if (userinfo != null)
                    pmName = Util.TrimNull(userinfo.UserName);
                else
                    pmName = AppConst.StringNull;

                string VendorName = null;
                if (Util.TrimNull(dr["VendorName"]) != AppConst.StringNull)
                    VendorName = Util.TrimNull(dr["VendorName"]).ToString();
                else
                    VendorName = "&nbsp;&nbsp;";

                string LastVendorSysNo = null;
                if (Util.TrimIntNull(dr["LastVendorSysNo"]) != AppConst.IntNull)
                    LastVendorSysNo = "(" + Util.TrimIntNull(dr["LastVendorSysNo"]).ToString() + ")";
                else
                    LastVendorSysNo = "&nbsp;&nbsp;";

                string Cost = null;
                if (Util.TrimDecimalNull(dr["Cost"]) != AppConst.DecimalNull)
                    Cost = Util.TrimDecimalNull(dr["Cost"]).ToString(AppConst.DecimalFormat);
                else
                    Cost = "&nbsp;&nbsp;";

                sb2.Append("<tr>");
                sb2.Append("<td>" + dr["SysNo"] + "</td>");
                sb2.Append("<td>" + dr["productid"] + "</td>");
                sb2.Append("<td width='250'>" + dr["productname"] + "</td>");
                sb2.Append("<td width='350'>" + dr["CustomerDesc"] + "</td>");
                sb2.Append("<td>" + statusName + "</td>");
                sb2.Append("<td>" + pmName + "</td>");
                sb2.Append("<td>" + VendorName + LastVendorSysNo + "</td>");
                sb2.Append("<td>" + Cost + "</td>");
                sb2.Append("</tr>");
            }

            sb2.Append("</table>");
            sb2.Append("<br>");

            //当日返还物品清单
            string listsql3 = @"select rma_register.SysNo,product.productid,product.productname,rma_register.CustomerDesc,rma_register.status ,product.PMUserSysNo,vendor.VendorName,lpi.LastVendorSysNo,rma_register.Cost 
                            from rma_register (NOLOCK) 
                            inner join product (NOLOCK) on rma_register.productsysno = product.sysno
                             left join product_lastpoinfo as lpi on product.sysno = lpi.productsysno
                             left join vendor (NOLOCK) on vendor.sysno = lpi.lastvendorsysno
                            where rma_register.outboundstatus=" + (int)AppEnum.RMAOutBoundStatus.Responsed + " and rma_register.ResponseTime between '" + today.AddDays(-1) + "' and '" + today + "' order by rma_register.SysNo desc";

            DataSet listds3 = SqlHelper.ExecuteDataSet(listsql3);

            int RevertedNum = (int)listds3.Tables[0].Rows.Count;

            decimal RevertedCost = 0;
            foreach (DataRow dr in listds3.Tables[0].Rows)
            {
                if (Util.TrimDecimalNull(dr["Cost"]) != AppConst.DecimalNull)
                    RevertedCost = RevertedCost + Util.TrimDecimalNull(dr["Cost"]);
            }

            StringBuilder sb3 = new StringBuilder(1000);
            sb3.Append("<table><font color=blue>当日返还物品清单</font></table>");
            sb3.Append("<br>");
            sb3.Append("共" + RevertedNum + "件,共" + RevertedCost.ToString(AppConst.DecimalFormatWithGroup) + "元");
            sb3.Append("<br>");
            sb3.Append("<table  border=1 cellspacing=0 cellspadding=0>");
            if (!Util.HasMoreRow(listds3))
            {
                sb3.Append("<tr>");
                sb3.Append("<td width=100%><font color=#ff0000>记录为空！</font></td>");
                sb3.Append("</tr>");
            }
            else if (Util.HasMoreRow(listds3))
            {
                sb3.Append("<tr>");
                sb3.Append("<td>单号</td>");
                sb3.Append("<td>产品号</td>");
                sb3.Append("<td>商品名称</td>");
                sb3.Append("<td>故障描述</td>");
                sb3.Append("<td>处理状态</td>");
                sb3.Append("<td>PM</td>");
                sb3.Append("<td>供应商</td>");
                sb3.Append("<td>金额</td>");
                sb3.Append("</tr>");
            }
            foreach (DataRow dr in listds3.Tables[0].Rows)
            {
                string statusName = AppEnum.GetRMAStatus(Util.TrimIntNull(dr["status"]));
                string pmName = null;
                int PMSysNo = Util.TrimIntNull(dr["PMUserSysNo"]);

                UserInfo userinfo = SysManager.GetInstance().LoadUser(PMSysNo);

                if (userinfo != null)
                    pmName = Util.TrimNull(userinfo.UserName);
                else
                    pmName = AppConst.StringNull;

                string VendorName = null;
                if (Util.TrimNull(dr["VendorName"]) != AppConst.StringNull)
                    VendorName = Util.TrimNull(dr["VendorName"]).ToString();
                else
                    VendorName = "&nbsp;&nbsp;";

                string LastVendorSysNo = null;
                if (Util.TrimIntNull(dr["LastVendorSysNo"]) != AppConst.IntNull)
                    LastVendorSysNo = "(" + Util.TrimIntNull(dr["LastVendorSysNo"]).ToString() + ")";
                else
                    LastVendorSysNo = "&nbsp;&nbsp;";

                string Cost = null;
                if (Util.TrimDecimalNull(dr["Cost"]) != AppConst.DecimalNull)
                    Cost = Util.TrimDecimalNull(dr["Cost"]).ToString(AppConst.DecimalFormat);
                else
                    Cost = "&nbsp;&nbsp;";

                sb3.Append("<tr>");
                sb3.Append("<td>" + dr["SysNo"] + "</td>");
                sb3.Append("<td>" + dr["productid"] + "</td>");
                sb3.Append("<td width='250'>" + dr["productname"] + "</td>");
                sb3.Append("<td width='350'>" + dr["CustomerDesc"] + "</td>");
                sb3.Append("<td>" + statusName + "</td>");
                sb3.Append("<td>" + pmName + "</td>");
                sb3.Append("<td>" + VendorName + LastVendorSysNo + "</td>");
                sb3.Append("<td>" + Cost + "</td>");
                sb3.Append("</tr>");
            }

            sb3.Append("</table>");
            sb3.Append("<br>");

            //当日退款物品清单
            string listsql4 = @"select rma_register.SysNo,product.productid,product.productname,rma_register.CustomerDesc,rma_register.status  ,product.PMUserSysNo,vendor.VendorName,lpi.LastVendorSysNo,rma_register.Cost from rma_refund (NOLOCK)
                            inner join rma_refund_item (NOLOCK) on rma_refund.sysno=rma_refund_item.refundsysno
                            inner join rma_register (NOLOCK) on rma_register.sysno = rma_refund_item.registersysno 
                            inner join product (NOLOCK) on rma_register.productsysno = product.sysno
                             left join product_lastpoinfo as lpi  on product.sysno = lpi.productsysno
                             left join vendor (NOLOCK) on vendor.sysno = lpi.lastvendorsysno
                            where rma_refund.status=" + (int)AppEnum.RMARefundStatus.Refunded + " and rma_refund.refundtime between '" + today.AddDays(-1) + "' and '" + today + "' order by rma_register.SysNo desc";
            DataSet listds4 = SqlHelper.ExecuteDataSet(listsql4);

            int RefundedNum = (int)listds4.Tables[0].Rows.Count;

            decimal RefundedCost = 0;
            foreach (DataRow dr in listds4.Tables[0].Rows)
            {
                if (Util.TrimDecimalNull(dr["Cost"]) != AppConst.DecimalNull)
                    RefundedCost = RefundedCost + Util.TrimDecimalNull(dr["Cost"]);
            }

            StringBuilder sb4 = new StringBuilder(1000);
            sb4.Append("<table><font color=blue>当日退款物品清单</font></table>");
            sb4.Append("<br>");
            sb4.Append("共" + RefundedNum + "件,共" + RefundedCost.ToString(AppConst.DecimalFormatWithGroup) + "元");
            sb4.Append("<br>");
            sb4.Append("<table  border=1 cellspacing=0 cellspadding=0>");
            if (!Util.HasMoreRow(listds4))
            {
                sb4.Append("<tr>");
                sb4.Append("<td width=100%><font color=#ff0000>记录为空！</font></td>");
                sb4.Append("</tr>");
            }
            else if (Util.HasMoreRow(listds4))
            {
                sb4.Append("<tr>");
                sb4.Append("<td>单号</td>");
                sb4.Append("<td>产品号</td>");
                sb4.Append("<td>商品名称</td>");
                sb4.Append("<td>故障描述</td>");
                sb4.Append("<td>处理状态</td>");
                sb4.Append("<td>PM</td>");
                sb4.Append("<td>供应商</td>");
                sb4.Append("<td>金额</td>");
                sb4.Append("</tr>");
            }
            foreach (DataRow dr in listds4.Tables[0].Rows)
            {
                string statusName = AppEnum.GetRMAStatus(Util.TrimIntNull(dr["status"]));
                string pmName = null;
                int PMSysNo = Util.TrimIntNull(dr["PMUserSysNo"]);

                UserInfo userinfo = SysManager.GetInstance().LoadUser(PMSysNo);

                if (userinfo != null)
                    pmName = Util.TrimNull(userinfo.UserName);
                else
                    pmName = AppConst.StringNull;

                string VendorName = null;
                if (Util.TrimNull(dr["VendorName"]) != AppConst.StringNull)
                    VendorName = Util.TrimNull(dr["VendorName"]).ToString();
                else
                    VendorName = "&nbsp;&nbsp;";

                string LastVendorSysNo = null;
                if (Util.TrimIntNull(dr["LastVendorSysNo"]) != AppConst.IntNull)
                    LastVendorSysNo = "(" + Util.TrimIntNull(dr["LastVendorSysNo"]).ToString() + ")";
                else
                    LastVendorSysNo = "&nbsp;&nbsp;";

                string Cost = null;
                if (Util.TrimDecimalNull(dr["Cost"]) != AppConst.DecimalNull)
                    Cost = Util.TrimDecimalNull(dr["Cost"]).ToString(AppConst.DecimalFormat);
                else
                    Cost = "&nbsp;&nbsp;";

                sb4.Append("<tr>");
                sb4.Append("<td>" + dr["SysNo"] + "</td>");
                sb4.Append("<td>" + dr["productid"] + "</td>");
                sb4.Append("<td width='250'>" + dr["productname"] + "</td>");
                sb4.Append("<td width='350'>" + dr["CustomerDesc"] + "</td>");
                sb4.Append("<td>" + statusName + "</td>");
                sb4.Append("<td>" + pmName + "</td>");
                sb4.Append("<td>" + VendorName + LastVendorSysNo + "</td>");
                sb4.Append("<td>" + Cost + "</td>");
                sb4.Append("</tr>");
            }

            sb4.Append("</table>");
            sb4.Append("<br>");

            //当日发新物品清单
            string listsql5 = @"select rma_register.SysNo,product.productid,product.productname,rma_register.CustomerDesc,rma_register.status ,product.PMUserSysNo,vendor.VendorName,lpi.LastVendorSysNo,rma_register.Cost  from rma_revert (NOLOCK)
                            inner join rma_revert_item (NOLOCK) on rma_revert.sysno=rma_revert_item.revertsysno
                            inner join rma_register (NOLOCK) on rma_revert_item.registersysno = rma_register.sysno
                            inner join product (NOLOCK) on rma_register.productsysno = product.sysno
                             left join product_lastpoinfo  as lpi  on product.sysno = lpi.productsysno
                             left join vendor (NOLOCK) on vendor.sysno = lpi.lastvendorsysno
                            where rma_revert.status=" + (int)AppEnum.RMARevertStatus.Reverted + " and rma_register.NewProductStatus<> " + (int)AppEnum.NewProductStatus.Origin + " and rma_revert.outtime between '" + today.AddDays(-1) + "' and '" + today + "' order by rma_register.SysNo desc";
            DataSet listds5 = SqlHelper.ExecuteDataSet(listsql5);

            int RevertedNewNum = (int)listds5.Tables[0].Rows.Count;

            decimal RevertedNewCost = 0;
            foreach (DataRow dr in listds5.Tables[0].Rows)
            {
                if (Util.TrimDecimalNull(dr["Cost"]) != AppConst.DecimalNull)
                    RevertedNewCost = RevertedNewCost + Util.TrimDecimalNull(dr["Cost"]);
            }

            StringBuilder sb5 = new StringBuilder(1000);
            sb5.Append("<table><font color=blue>当日发新物品清单</font></table>");
            sb5.Append("<br>");
            sb5.Append("共" + RevertedNewNum + "件,共" + RevertedNewCost.ToString(AppConst.DecimalFormatWithGroup) + "元");
            sb5.Append("<br>");
            sb5.Append("<table  border=1 cellspacing=0 cellspadding=0>");
            if (!Util.HasMoreRow(listds5))
            {
                sb5.Append("<tr>");
                sb5.Append("<td width=100%><font color=#ff0000>记录为空！</font></td>");
                sb5.Append("</tr>");
            }
            else if (Util.HasMoreRow(listds5))
            {
                sb5.Append("<tr>");
                sb5.Append("<td>单号</td>");
                sb5.Append("<td>产品号</td>");
                sb5.Append("<td>商品名称</td>");
                sb5.Append("<td>故障描述</td>");
                sb5.Append("<td>处理状态</td>");
                sb5.Append("<td>PM</td>");
                sb5.Append("<td>供应商</td>");
                sb5.Append("<td>金额</td>");
                sb5.Append("</tr>");
            }
            foreach (DataRow dr in listds5.Tables[0].Rows)
            {
                string statusName = AppEnum.GetRMAStatus(Util.TrimIntNull(dr["status"]));
                string pmName = null;
                int PMSysNo = Util.TrimIntNull(dr["PMUserSysNo"]);

                UserInfo userinfo = SysManager.GetInstance().LoadUser(PMSysNo);

                if (userinfo != null)
                    pmName = Util.TrimNull(userinfo.UserName);
                else
                    pmName = AppConst.StringNull;

                string VendorName = null;
                if (Util.TrimNull(dr["VendorName"]) != AppConst.StringNull)
                    VendorName = Util.TrimNull(dr["VendorName"]).ToString();
                else
                    VendorName = "&nbsp;&nbsp;";

                string LastVendorSysNo = null;
                if (Util.TrimIntNull(dr["LastVendorSysNo"]) != AppConst.IntNull)
                    LastVendorSysNo = "(" + Util.TrimIntNull(dr["LastVendorSysNo"]).ToString() + ")";
                else
                    LastVendorSysNo = "&nbsp;&nbsp;";

                string Cost = null;
                if (Util.TrimDecimalNull(dr["Cost"]) != AppConst.DecimalNull)
                    Cost = Util.TrimDecimalNull(dr["Cost"]).ToString(AppConst.DecimalFormat);
                else
                    Cost = "&nbsp;&nbsp;";

                sb5.Append("<tr>");
                sb5.Append("<td>" + dr["SysNo"] + "</td>");
                sb5.Append("<td>" + dr["productid"] + "</td>");
                sb5.Append("<td width='250'>" + dr["productname"] + "</td>");
                sb5.Append("<td width='350'>" + dr["CustomerDesc"] + "</td>");
                sb5.Append("<td>" + statusName + "</td>");
                sb5.Append("<td>" + pmName + "</td>");
                sb5.Append("<td>" + VendorName + LastVendorSysNo + "</td>");
                sb5.Append("<td>" + Cost + "</td>");
                sb5.Append("</tr>");
            }

            sb5.Append("</table>");
            sb5.Append("<br>");

            //当日退货入库清单
            string listsql6 = @"select rma_register.SysNo,product.productid,product.productname,rma_register.CustomerDesc,rma_register.status,product.PMUserSysNo,vendor.VendorName,lpi.LastVendorSysNo,rma_register.Cost  from rma_return (NOLOCK)
                            inner join rma_return_item (NOLOCK) on rma_return.sysno=rma_return_item.returnsysno
                            inner join rma_register (NOLOCK) on rma_register.sysno = rma_return_item.registersysno 
                            inner join product (NOLOCK) on rma_register.productsysno = product.sysno
                             left join product_lastpoinfo as lpi  on product.sysno = lpi.productsysno
                             left join vendor (NOLOCK) on vendor.sysno = lpi.lastvendorsysno
                            where rma_return.status = " + (int)AppEnum.RMAReturnStatus.Returned + " and rma_return.returntime between '" + today.AddDays(-1) + "' and '" + today + "' order by rma_register.SysNo desc";
            DataSet listds6 = SqlHelper.ExecuteDataSet(listsql6);

            int ReturnedNum = (int)listds6.Tables[0].Rows.Count;

            decimal ReturnedCost = 0;
            foreach (DataRow dr in listds6.Tables[0].Rows)
            {
                if (Util.TrimDecimalNull(dr["Cost"]) != AppConst.DecimalNull)
                    ReturnedCost = ReturnedCost + Util.TrimDecimalNull(dr["Cost"]);
            }

            StringBuilder sb6 = new StringBuilder(1000);
            sb6.Append("<table><font color=blue>当日退货入库清单</font></table>");
            sb6.Append("<br>");
            sb6.Append("共" + ReturnedNum + "件,共" + ReturnedCost.ToString(AppConst.DecimalFormatWithGroup) + "元");
            sb6.Append("<br>");
            sb6.Append("<table border=1 cellspacing=0 cellspadding=0>");
            if (!Util.HasMoreRow(listds6))
            {
                sb6.Append("<tr>");
                sb6.Append("<td width=100%><font color=#ff0000>记录为空！</font></td>");
                sb6.Append("</tr>");
            }
            else if (Util.HasMoreRow(listds6))
            {
                sb6.Append("<tr>");
                sb6.Append("<td>单号</td>");
                sb6.Append("<td>产品号</td>");
                sb6.Append("<td>商品名称</td>");
                sb6.Append("<td>故障描述</td>");
                sb6.Append("<td>处理状态</td>");
                sb6.Append("<td>PM</td>");
                sb6.Append("<td>供应商</td>");
                sb6.Append("<td>金额</td>");
                sb6.Append("</tr>");
            }
            foreach (DataRow dr in listds6.Tables[0].Rows)
            {
                string statusName = AppEnum.GetRMAStatus(Util.TrimIntNull(dr["status"]));
                string pmName = null;
                int PMSysNo = Util.TrimIntNull(dr["PMUserSysNo"]);

                UserInfo userinfo = SysManager.GetInstance().LoadUser(PMSysNo);

                if (userinfo != null)
                    pmName = Util.TrimNull(userinfo.UserName);
                else
                    pmName = AppConst.StringNull;

                string VendorName = null;
                if (Util.TrimNull(dr["VendorName"]) != AppConst.StringNull)
                    VendorName = Util.TrimNull(dr["VendorName"]).ToString();
                else
                    VendorName = "&nbsp;&nbsp;";

                string LastVendorSysNo = null;
                if (Util.TrimIntNull(dr["LastVendorSysNo"]) != AppConst.IntNull)
                    LastVendorSysNo = "(" + Util.TrimIntNull(dr["LastVendorSysNo"]).ToString() + ")";
                else
                    LastVendorSysNo = "&nbsp;&nbsp;";

                string Cost = null;
                if (Util.TrimDecimalNull(dr["Cost"]) != AppConst.DecimalNull)
                    Cost = Util.TrimDecimalNull(dr["Cost"]).ToString(AppConst.DecimalFormat);
                else
                    Cost = "&nbsp;&nbsp;";

                sb6.Append("<tr>");
                sb6.Append("<td>" + dr["SysNo"] + "</td>");
                sb6.Append("<td>" + dr["productid"] + "</td>");
                sb6.Append("<td width='250'>" + dr["productname"] + "</td>");
                sb6.Append("<td width='350'>" + dr["CustomerDesc"] + "</td>");
                sb6.Append("<td>" + statusName + "</td>");
                sb6.Append("<td>" + pmName + "</td>");
                sb6.Append("<td>" + VendorName + LastVendorSysNo + "</td>");
                sb6.Append("<td>" + Cost + "</td>");
                sb6.Append("</tr>");
            }

            sb6.Append("</table>");
            sb6.Append("<br>");

            //超时未结束物品清单
            string listsql7 = @"select rma_register.SysNo,product.productid,product.productname,rma_register.CustomerDesc,rma_register.status,product.PMUserSysNo,vendor.VendorName,lpi.LastVendorSysNo,rma_register.Cost  from rma_request (NOLOCK) 
                             inner join rma_request_item (NOLOCK) on rma_request.sysno = rma_request_item.requestsysno
                             inner join rma_register (NOLOCK) on rma_register.sysno = rma_request_item.registersysno 
                             inner join product (NOLOCK) on rma_register.productsysno = product.sysno
                             left join product_lastpoinfo  as lpi on product.sysno = lpi.productsysno
                             left join vendor (NOLOCK) on vendor.sysno = lpi.lastvendorsysno
                             where rma_register.status=" + (int)AppEnum.RMARequestStatus.Handling + " and rma_request.recvtime <= '" + today.AddDays(-21) + "' order by rma_register.SysNo desc";
            DataSet listds7 = SqlHelper.ExecuteDataSet(listsql7);

            int OverTimeNum = (int)listds7.Tables[0].Rows.Count;

            decimal OverTimeCost = 0;
            foreach (DataRow dr in listds7.Tables[0].Rows)
            {
                if (Util.TrimDecimalNull(dr["Cost"]) != AppConst.DecimalNull)
                    OverTimeCost = OverTimeCost + Util.TrimDecimalNull(dr["Cost"]);
            }

            StringBuilder sb7 = new StringBuilder(1000);
            sb7.Append("<table><font color=blue>超时未结束物品清单</font></table>");
            sb7.Append("<br>");
            sb7.Append("共" + OverTimeNum + "件,共" + OverTimeCost.ToString(AppConst.DecimalFormatWithGroup) + "元");
            sb7.Append("<br>");
            sb7.Append("<table border=1 cellspacing=0 cellspadding=0>");
            if (!Util.HasMoreRow(listds7))
            {
                sb7.Append("<tr>");
                sb7.Append("<td width=100%><font color=#ff0000>记录为空！</font></td>");
                sb7.Append("</tr>");
            }
            else if (Util.HasMoreRow(listds7))
            {
                sb7.Append("<tr>");
                sb7.Append("<td>单号</td>");
                sb7.Append("<td>产品号</td>");
                sb7.Append("<td>商品名称</td>");
                sb7.Append("<td>故障描述</td>");
                sb7.Append("<td>处理状态</td>");
                sb7.Append("<td>PM</td>");
                sb7.Append("<td>供应商</td>");
                sb7.Append("<td>金额</td>");
                sb7.Append("</tr>");
            }
            foreach (DataRow dr in listds7.Tables[0].Rows)
            {
                string statusName = AppEnum.GetRMAStatus(Util.TrimIntNull(dr["status"]));
                string pmName = null;
                int PMSysNo = Util.TrimIntNull(dr["PMUserSysNo"]);

                UserInfo userinfo = SysManager.GetInstance().LoadUser(PMSysNo);

                if (userinfo != null)
                    pmName = Util.TrimNull(userinfo.UserName);
                else
                    pmName = AppConst.StringNull;

                string VendorName = null;
                if (Util.TrimNull(dr["VendorName"]) != AppConst.StringNull)
                    VendorName = Util.TrimNull(dr["VendorName"]).ToString();
                else
                    VendorName = "&nbsp;&nbsp;";

                string LastVendorSysNo = null;
                if (Util.TrimIntNull(dr["LastVendorSysNo"]) != AppConst.IntNull)
                    LastVendorSysNo = "(" + Util.TrimIntNull(dr["LastVendorSysNo"]).ToString() + ")";
                else
                    LastVendorSysNo = "&nbsp;&nbsp";

                string Cost = null;
                if (Util.TrimDecimalNull(dr["Cost"]) != AppConst.DecimalNull)
                    Cost = Util.TrimDecimalNull(dr["Cost"]).ToString(AppConst.DecimalFormat);
                else
                    Cost = "&nbsp;&nbsp;";

                sb7.Append("<tr>");
                sb7.Append("<td>" + dr["SysNo"] + "</td>");
                sb7.Append("<td>" + dr["productid"] + "</td>");
                sb7.Append("<td width='250'>" + dr["productname"] + "</td>");
                sb7.Append("<td width='350'>" + dr["CustomerDesc"] + "</td>");
                sb7.Append("<td>" + statusName + "</td>");
                sb7.Append("<td>" + pmName + "</td>");
                sb7.Append("<td>" + VendorName + LastVendorSysNo + "</td>");
                sb7.Append("<td>" + Cost + "</td>");
                sb7.Append("</tr>");
            }

            sb7.Append("</table>");
            sb7.Append("<br>");
            //
            int allRegisterNum = 0;
            decimal allRegisterCost = 0;
            allRegisterNum = RecRegisterNum + HandlingRegisterNum + RevertedNum + RefundedNum + RevertedNewNum + ReturnedNum + OverTimeNum;
            allRegisterCost = RecRegisterCost + HandlingRegisterCost + RevertedCost + RefundedCost + RevertedNewCost + ReturnedCost + OverTimeCost;
            //
            StringBuilder sbAll = new StringBuilder(1000);
            sbAll.Append("<table border=1 width='30%'><tr width='100%'><font color=blue>" + year.ToString() + "年" + month.ToString() + "月" + day.ToString() + "日RMA情况汇总表</font></tr>");
            sbAll.Append("<tr><td>分类</td><td>数量</td><td>金额</td></tr>");
            sbAll.Append("<tr><td>当日接受件</td><td>" + RecRegisterNum.ToString() + "</td><td>" + RecRegisterCost.ToString(AppConst.DecimalFormatWithGroup) + "</td></tr>");
            sbAll.Append("<tr><td>目前处理中的保修记录</td><td>" + HandlingRegisterNum.ToString() + "</td><td>" + HandlingRegisterCost.ToString(AppConst.DecimalFormatWithGroup) + "</td></tr>");
            sbAll.Append("<tr><td>当日返还物品清单</td><td>" + RevertedNum.ToString() + "</td><td>" + RevertedCost.ToString(AppConst.DecimalFormatWithGroup) + "</td></tr>");
            sbAll.Append("<tr><td>当日退款物品清单</td><td>" + RefundedNum.ToString() + "</td><td>" + RefundedCost.ToString(AppConst.DecimalFormatWithGroup) + "</td></tr>");
            sbAll.Append("<tr><td>当日发新物品清单</td><td>" + RevertedNewNum.ToString() + "</td><td>" + RevertedNewCost.ToString(AppConst.DecimalFormatWithGroup) + "</td></tr>");
            sbAll.Append("<tr><td>当日退货入库清单</td><td>" + ReturnedNum.ToString() + "</td><td>" + ReturnedCost.ToString(AppConst.DecimalFormatWithGroup) + "</td></tr>");
            sbAll.Append("<tr><td>超时未结束物品清单</td><td>" + OverTimeNum.ToString() + "</td><td>" + OverTimeCost.ToString(AppConst.DecimalFormatWithGroup) + "</td></tr>");
            sbAll.Append("<tr><td>总计</td><td>" + allRegisterNum.ToString() + "</td><td>" + allRegisterCost.ToString(AppConst.DecimalFormatWithGroup) + "</td></tr>");
            sbAll.Append("</table>");
            sbAll.Append("<br>");


            //send mail
            //if (AppConfig.DayReportMail == AppConst.StringNull)
            //    return;

            //TCPMail oMail = new TCPMail();
            //string mailaddress = AppConfig.DayReportMail;
            //string mailSubject = "每日RMA情况表";
            //string mailBody = sbAll.ToString() + sb1.ToString() + sb2.ToString() + sb3.ToString() + sb4.ToString() + sb5.ToString() + sb6.ToString() + sb7.ToString();
            //oMail.Send(mailaddress, mailSubject, mailBody);
        }
        #region RMA每日报表(Special for PM)
        public void mailSpecialforPM(int sysno, string emailAddress)
        {

            //今日RMA情况汇总表

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int day = DateTime.Now.AddDays(-1).Day;
            DateTime today = DateTime.Today;
            //Begin

            //今日接受件
            string listsql1 = @"select  rma_register.SysNo,product.productid,product.productname,rma_register.CustomerDesc,rma_register.status,product.PMUserSysNo,vendor.VendorName,lpi.LastVendorSysNo,rma_register.Cost from rma_request (NOLOCK) 
                             inner join rma_request_item (NOLOCK) on rma_request.sysno = rma_request_item.requestsysno
                             inner join rma_register (NOLOCK) on rma_register.sysno = rma_request_item.registersysno 
                             inner join product (NOLOCK) on rma_register.productsysno = product.sysno
                             left join product_lastpoinfo as lpi (NOLOCK) on product.sysno = lpi.productsysno
                             left join vendor (NOLOCK) on vendor.sysno = lpi.lastvendorsysno
                             where   rma_register.status=" + (int)AppEnum.RMARequestStatus.Handling + " and rma_request.recvtime between '" + today.AddDays(-1) + "' and '" + today + "'and product.PMUserSysNo=" + sysno + " order by rma_register.SysNo desc";

            DataSet listds1 = SqlHelper.ExecuteDataSet(listsql1);
            int RecRegisterNum = (int)listds1.Tables[0].Rows.Count;

            decimal RecRegisterCost = 0;
            foreach (DataRow dr in listds1.Tables[0].Rows)
            {
                if (Util.TrimDecimalNull(dr["Cost"]) != AppConst.DecimalNull)
                    RecRegisterCost = RecRegisterCost + Util.TrimDecimalNull(dr["Cost"]);
            }

            StringBuilder sb1 = new StringBuilder(1000);
            sb1.Append("<table><font color=blue>当日接受件</font></table>");
            sb1.Append("<br>");
            sb1.Append("共" + RecRegisterNum + "件,共" + RecRegisterCost.ToString(AppConst.DecimalFormatWithGroup) + "元");
            sb1.Append("<br>");
            sb1.Append("<table border=1 cellspacing=0 cellspadding=0>");
            if (!Util.HasMoreRow(listds1))
            {
                sb1.Append("<tr>");
                sb1.Append("<td width=100%><font color=#ff0000>记录为空！</font></td>");
                sb1.Append("</tr>");
            }
            else if (Util.HasMoreRow(listds1))
            {
                sb1.Append("<tr>");
                sb1.Append("<td>单号</td>");
                sb1.Append("<td>产品号</td>");
                sb1.Append("<td>商品名称</td>");
                sb1.Append("<td>故障描述</td>");
                sb1.Append("<td>处理状态</td>");
                sb1.Append("<td>PM</td>");
                sb1.Append("<td>供应商</td>");
                sb1.Append("<td>金额</td>");
                sb1.Append("</tr>");
            }
            foreach (DataRow dr in listds1.Tables[0].Rows)
            {
                string statusName = AppEnum.GetRMAStatus(Util.TrimIntNull(dr["status"]));
                string pmName = null;
                int PMSysNo = Util.TrimIntNull(dr["PMUserSysNo"]);

                UserInfo userinfo = SysManager.GetInstance().LoadUser(PMSysNo);

                if (userinfo != null)
                    pmName = Util.TrimNull(userinfo.UserName);
                else
                    pmName = AppConst.StringNull;

                string VendorName = null;
                if (Util.TrimNull(dr["VendorName"]) != AppConst.StringNull)
                    VendorName = Util.TrimNull(dr["VendorName"]).ToString();
                else
                    VendorName = "&nbsp;&nbsp;";

                string LastVendorSysNo = null;
                if (Util.TrimIntNull(dr["LastVendorSysNo"]) != AppConst.IntNull)
                    LastVendorSysNo = "(" + Util.TrimIntNull(dr["LastVendorSysNo"]).ToString() + ")";
                else
                    LastVendorSysNo = "&nbsp;&nbsp;";

                string Cost = null;
                if (Util.TrimDecimalNull(dr["Cost"]) != AppConst.DecimalNull)
                    Cost = Util.TrimDecimalNull(dr["Cost"]).ToString(AppConst.DecimalFormat);
                else
                    Cost = "&nbsp;&nbsp;";

                sb1.Append("<tr>");
                sb1.Append("<td>" + dr["SysNo"] + "</td>");
                sb1.Append("<td>" + dr["productid"] + "</td>");
                sb1.Append("<td width='250'>" + dr["productname"] + "</td>");
                sb1.Append("<td width='350'>" + dr["CustomerDesc"] + "</td>");
                sb1.Append("<td>" + statusName + "</td>");
                sb1.Append("<td>" + pmName + "</td>");
                sb1.Append("<td>" + VendorName + LastVendorSysNo + "</td>");
                sb1.Append("<td>" + Cost + "</td>");
                sb1.Append("</tr>");
            }

            sb1.Append("</table>");
            sb1.Append("<br>");
            //End
            //Begin

            //当日返还物品清单
            string listsql3 = @"select rma_register.SysNo,product.productid,product.productname,rma_register.CustomerDesc,rma_register.status ,product.PMUserSysNo,vendor.VendorName,lpi.LastVendorSysNo,rma_register.Cost 
                            from rma_register (NOLOCK) 
                            inner join product (NOLOCK) on rma_register.productsysno = product.sysno
                             left join product_lastpoinfo as lpi on product.sysno = lpi.productsysno
                             left join vendor (NOLOCK) on vendor.sysno = lpi.lastvendorsysno
                            where rma_register.outboundstatus=" + (int)AppEnum.RMAOutBoundStatus.Responsed + " and rma_register.ResponseTime between '" + today.AddDays(-1) + "' and '" + today + "'and product.PMUserSysNo=" + sysno + "order by rma_register.SysNo desc";

            DataSet listds3 = SqlHelper.ExecuteDataSet(listsql3);

            int RevertedNum = (int)listds3.Tables[0].Rows.Count;

            decimal RevertedCost = 0;
            foreach (DataRow dr in listds3.Tables[0].Rows)
            {
                if (Util.TrimDecimalNull(dr["Cost"]) != AppConst.DecimalNull)
                    RevertedCost = RevertedCost + Util.TrimDecimalNull(dr["Cost"]);
            }

            StringBuilder sb3 = new StringBuilder(1000);
            sb3.Append("<table><font color=blue>当日返还物品清单</font></table>");
            sb3.Append("<br>");
            sb3.Append("共" + RevertedNum + "件,共" + RevertedCost.ToString(AppConst.DecimalFormatWithGroup) + "元");
            sb3.Append("<br>");
            sb3.Append("<table  border=1 cellspacing=0 cellspadding=0>");
            if (!Util.HasMoreRow(listds3))
            {
                sb3.Append("<tr>");
                sb3.Append("<td width=100%><font color=#ff0000>记录为空！</font></td>");
                sb3.Append("</tr>");
            }
            else if (Util.HasMoreRow(listds3))
            {
                sb3.Append("<tr>");
                sb3.Append("<td>单号</td>");
                sb3.Append("<td>产品号</td>");
                sb3.Append("<td>商品名称</td>");
                sb3.Append("<td>故障描述</td>");
                sb3.Append("<td>处理状态</td>");
                sb3.Append("<td>PM</td>");
                sb3.Append("<td>供应商</td>");
                sb3.Append("<td>金额</td>");
                sb3.Append("</tr>");
            }
            foreach (DataRow dr in listds3.Tables[0].Rows)
            {
                string statusName = AppEnum.GetRMAStatus(Util.TrimIntNull(dr["status"]));
                string pmName = null;
                int PMSysNo = Util.TrimIntNull(dr["PMUserSysNo"]);

                UserInfo userinfo = SysManager.GetInstance().LoadUser(PMSysNo);

                if (userinfo != null)
                    pmName = Util.TrimNull(userinfo.UserName);
                else
                    pmName = AppConst.StringNull;

                string VendorName = null;
                if (Util.TrimNull(dr["VendorName"]) != AppConst.StringNull)
                    VendorName = Util.TrimNull(dr["VendorName"]).ToString();
                else
                    VendorName = "&nbsp;&nbsp;";

                string LastVendorSysNo = null;
                if (Util.TrimIntNull(dr["LastVendorSysNo"]) != AppConst.IntNull)
                    LastVendorSysNo = "(" + Util.TrimIntNull(dr["LastVendorSysNo"]).ToString() + ")";
                else
                    LastVendorSysNo = "&nbsp;&nbsp;";

                string Cost = null;
                if (Util.TrimDecimalNull(dr["Cost"]) != AppConst.DecimalNull)
                    Cost = Util.TrimDecimalNull(dr["Cost"]).ToString(AppConst.DecimalFormat);
                else
                    Cost = "&nbsp;&nbsp;";

                sb3.Append("<tr>");
                sb3.Append("<td>" + dr["SysNo"] + "</td>");
                sb3.Append("<td>" + dr["productid"] + "</td>");
                sb3.Append("<td width='250'>" + dr["productname"] + "</td>");
                sb3.Append("<td width='350'>" + dr["CustomerDesc"] + "</td>");
                sb3.Append("<td>" + statusName + "</td>");
                sb3.Append("<td>" + pmName + "</td>");
                sb3.Append("<td>" + VendorName + LastVendorSysNo + "</td>");
                sb3.Append("<td>" + Cost + "</td>");
                sb3.Append("</tr>");
            }

            sb3.Append("</table>");
            sb3.Append("<br>");
            //End
            //Begin

            //当日退款物品清单
            string listsql4 = @"select rma_register.SysNo,product.productid,product.productname,rma_register.CustomerDesc,rma_register.status  ,product.PMUserSysNo,vendor.VendorName,lpi.LastVendorSysNo,rma_register.Cost from rma_refund (NOLOCK)
                            inner join rma_refund_item (NOLOCK) on rma_refund.sysno=rma_refund_item.refundsysno
                            inner join rma_register (NOLOCK) on rma_register.sysno = rma_refund_item.registersysno 
                            inner join product (NOLOCK) on rma_register.productsysno = product.sysno
                             left join product_lastpoinfo as lpi  on product.sysno = lpi.productsysno
                             left join vendor (NOLOCK) on vendor.sysno = lpi.lastvendorsysno
                            where rma_refund.status=" + (int)AppEnum.RMARefundStatus.Refunded + " and rma_refund.refundtime between '" + today.AddDays(-1) + "' and '" + today + "'and product.PMUserSysNo=" + sysno + " order by rma_register.SysNo desc";
            DataSet listds4 = SqlHelper.ExecuteDataSet(listsql4);

            int RefundedNum = (int)listds4.Tables[0].Rows.Count;

            decimal RefundedCost = 0;
            foreach (DataRow dr in listds4.Tables[0].Rows)
            {
                if (Util.TrimDecimalNull(dr["Cost"]) != AppConst.DecimalNull)
                    RefundedCost = RefundedCost + Util.TrimDecimalNull(dr["Cost"]);
            }

            StringBuilder sb4 = new StringBuilder(1000);
            sb4.Append("<table><font color=blue>当日退款物品清单</font></table>");
            sb4.Append("<br>");
            sb4.Append("共" + RefundedNum + "件,共" + RefundedCost.ToString(AppConst.DecimalFormatWithGroup) + "元");
            sb4.Append("<br>");
            sb4.Append("<table  border=1 cellspacing=0 cellspadding=0>");
            if (!Util.HasMoreRow(listds4))
            {
                sb4.Append("<tr>");
                sb4.Append("<td width=100%><font color=#ff0000>记录为空！</font></td>");
                sb4.Append("</tr>");
            }
            else if (Util.HasMoreRow(listds4))
            {
                sb4.Append("<tr>");
                sb4.Append("<td>单号</td>");
                sb4.Append("<td>产品号</td>");
                sb4.Append("<td>商品名称</td>");
                sb4.Append("<td>故障描述</td>");
                sb4.Append("<td>处理状态</td>");
                sb4.Append("<td>PM</td>");
                sb4.Append("<td>供应商</td>");
                sb4.Append("<td>金额</td>");
                sb4.Append("</tr>");
            }
            foreach (DataRow dr in listds4.Tables[0].Rows)
            {
                string statusName = AppEnum.GetRMAStatus(Util.TrimIntNull(dr["status"]));
                string pmName = null;
                int PMSysNo = Util.TrimIntNull(dr["PMUserSysNo"]);

                UserInfo userinfo = SysManager.GetInstance().LoadUser(PMSysNo);

                if (userinfo != null)
                    pmName = Util.TrimNull(userinfo.UserName);
                else
                    pmName = AppConst.StringNull;

                string VendorName = null;
                if (Util.TrimNull(dr["VendorName"]) != AppConst.StringNull)
                    VendorName = Util.TrimNull(dr["VendorName"]).ToString();
                else
                    VendorName = "&nbsp;&nbsp;";

                string LastVendorSysNo = null;
                if (Util.TrimIntNull(dr["LastVendorSysNo"]) != AppConst.IntNull)
                    LastVendorSysNo = "(" + Util.TrimIntNull(dr["LastVendorSysNo"]).ToString() + ")";
                else
                    LastVendorSysNo = "&nbsp;&nbsp;";

                string Cost = null;
                if (Util.TrimDecimalNull(dr["Cost"]) != AppConst.DecimalNull)
                    Cost = Util.TrimDecimalNull(dr["Cost"]).ToString(AppConst.DecimalFormat);
                else
                    Cost = "&nbsp;&nbsp;";

                sb4.Append("<tr>");
                sb4.Append("<td>" + dr["SysNo"] + "</td>");
                sb4.Append("<td>" + dr["productid"] + "</td>");
                sb4.Append("<td width='250'>" + dr["productname"] + "</td>");
                sb4.Append("<td width='350'>" + dr["CustomerDesc"] + "</td>");
                sb4.Append("<td>" + statusName + "</td>");
                sb4.Append("<td>" + pmName + "</td>");
                sb4.Append("<td>" + VendorName + LastVendorSysNo + "</td>");
                sb4.Append("<td>" + Cost + "</td>");
                sb4.Append("</tr>");
            }

            sb4.Append("</table>");
            sb4.Append("<br>");
            //End
            //Begin
            //当日发新物品清单
            string listsql5 = @"select rma_register.SysNo,product.productid,product.productname,rma_register.CustomerDesc,rma_register.status ,product.PMUserSysNo,vendor.VendorName,lpi.LastVendorSysNo,rma_register.Cost  from rma_revert (NOLOCK)
                            inner join rma_revert_item (NOLOCK) on rma_revert.sysno=rma_revert_item.revertsysno
                            inner join rma_register (NOLOCK) on rma_revert_item.registersysno = rma_register.sysno
                            inner join product (NOLOCK) on rma_register.productsysno = product.sysno
                             left join product_lastpoinfo  as lpi  on product.sysno = lpi.productsysno
                             left join vendor (NOLOCK) on vendor.sysno = lpi.lastvendorsysno
                            where rma_revert.status=" + (int)AppEnum.RMARevertStatus.Reverted + " and rma_register.NewProductStatus<> " + (int)AppEnum.NewProductStatus.Origin + " and rma_revert.outtime between '" + today.AddDays(-1) + "' and '" + today + "'and product.PMUserSysNo=" + sysno + " order by rma_register.SysNo desc";
            DataSet listds5 = SqlHelper.ExecuteDataSet(listsql5);

            int RevertedNewNum = (int)listds5.Tables[0].Rows.Count;

            decimal RevertedNewCost = 0;
            foreach (DataRow dr in listds5.Tables[0].Rows)
            {
                if (Util.TrimDecimalNull(dr["Cost"]) != AppConst.DecimalNull)
                    RevertedNewCost = RevertedNewCost + Util.TrimDecimalNull(dr["Cost"]);
            }

            StringBuilder sb5 = new StringBuilder(1000);
            sb5.Append("<table><font color=blue>当日发新物品清单</font></table>");
            sb5.Append("<br>");
            sb5.Append("共" + RevertedNewNum + "件,共" + RevertedNewCost.ToString(AppConst.DecimalFormatWithGroup) + "元");
            sb5.Append("<br>");
            sb5.Append("<table  border=1 cellspacing=0 cellspadding=0>");
            if (!Util.HasMoreRow(listds5))
            {
                sb5.Append("<tr>");
                sb5.Append("<td width=100%><font color=#ff0000>记录为空！</font></td>");
                sb5.Append("</tr>");
            }
            else if (Util.HasMoreRow(listds5))
            {
                sb5.Append("<tr>");
                sb5.Append("<td>单号</td>");
                sb5.Append("<td>产品号</td>");
                sb5.Append("<td>商品名称</td>");
                sb5.Append("<td>故障描述</td>");
                sb5.Append("<td>处理状态</td>");
                sb5.Append("<td>PM</td>");
                sb5.Append("<td>供应商</td>");
                sb5.Append("<td>金额</td>");
                sb5.Append("</tr>");
            }
            foreach (DataRow dr in listds5.Tables[0].Rows)
            {
                string statusName = AppEnum.GetRMAStatus(Util.TrimIntNull(dr["status"]));
                string pmName = null;
                int PMSysNo = Util.TrimIntNull(dr["PMUserSysNo"]);

                UserInfo userinfo = SysManager.GetInstance().LoadUser(PMSysNo);

                if (userinfo != null)
                    pmName = Util.TrimNull(userinfo.UserName);
                else
                    pmName = AppConst.StringNull;

                string VendorName = null;
                if (Util.TrimNull(dr["VendorName"]) != AppConst.StringNull)
                    VendorName = Util.TrimNull(dr["VendorName"]).ToString();
                else
                    VendorName = "&nbsp;&nbsp;";

                string LastVendorSysNo = null;
                if (Util.TrimIntNull(dr["LastVendorSysNo"]) != AppConst.IntNull)
                    LastVendorSysNo = "(" + Util.TrimIntNull(dr["LastVendorSysNo"]).ToString() + ")";
                else
                    LastVendorSysNo = "&nbsp;&nbsp;";

                string Cost = null;
                if (Util.TrimDecimalNull(dr["Cost"]) != AppConst.DecimalNull)
                    Cost = Util.TrimDecimalNull(dr["Cost"]).ToString(AppConst.DecimalFormat);
                else
                    Cost = "&nbsp;&nbsp;";

                sb5.Append("<tr>");
                sb5.Append("<td>" + dr["SysNo"] + "</td>");
                sb5.Append("<td>" + dr["productid"] + "</td>");
                sb5.Append("<td width='250'>" + dr["productname"] + "</td>");
                sb5.Append("<td width='350'>" + dr["CustomerDesc"] + "</td>");
                sb5.Append("<td>" + statusName + "</td>");
                sb5.Append("<td>" + pmName + "</td>");
                sb5.Append("<td>" + VendorName + LastVendorSysNo + "</td>");
                sb5.Append("<td>" + Cost + "</td>");
                sb5.Append("</tr>");
            }

            sb5.Append("</table>");
            sb5.Append("<br>");
            //End
            //Begin
            //当日退货入库清单
            string listsql6 = @"select rma_register.SysNo,product.productid,product.productname,rma_register.CustomerDesc,rma_register.status,product.PMUserSysNo,vendor.VendorName,lpi.LastVendorSysNo,rma_register.Cost  from rma_return (NOLOCK)
                            inner join rma_return_item (NOLOCK) on rma_return.sysno=rma_return_item.returnsysno
                            inner join rma_register (NOLOCK) on rma_register.sysno = rma_return_item.registersysno 
                            inner join product (NOLOCK) on rma_register.productsysno = product.sysno
                             left join product_lastpoinfo as lpi  on product.sysno = lpi.productsysno
                             left join vendor (NOLOCK) on vendor.sysno = lpi.lastvendorsysno
                            where rma_return.status = " + (int)AppEnum.RMAReturnStatus.Returned + " and rma_return.returntime between '" + today.AddDays(-1) + "' and '" + today + "'and product.PMUserSysNo=" + sysno + " order by rma_register.SysNo desc";
            DataSet listds6 = SqlHelper.ExecuteDataSet(listsql6);

            int ReturnedNum = (int)listds6.Tables[0].Rows.Count;

            decimal ReturnedCost = 0;
            foreach (DataRow dr in listds6.Tables[0].Rows)
            {
                if (Util.TrimDecimalNull(dr["Cost"]) != AppConst.DecimalNull)
                    ReturnedCost = ReturnedCost + Util.TrimDecimalNull(dr["Cost"]);
            }

            StringBuilder sb6 = new StringBuilder(1000);
            sb6.Append("<table><font color=blue>当日退货入库清单</font></table>");
            sb6.Append("<br>");
            sb6.Append("共" + ReturnedNum + "件,共" + ReturnedCost.ToString(AppConst.DecimalFormatWithGroup) + "元");
            sb6.Append("<br>");
            sb6.Append("<table border=1 cellspacing=0 cellspadding=0>");
            if (!Util.HasMoreRow(listds6))
            {
                sb6.Append("<tr>");
                sb6.Append("<td width=100%><font color=#ff0000>记录为空！</font></td>");
                sb6.Append("</tr>");
            }
            else if (Util.HasMoreRow(listds6))
            {
                sb6.Append("<tr>");
                sb6.Append("<td>单号</td>");
                sb6.Append("<td>产品号</td>");
                sb6.Append("<td>商品名称</td>");
                sb6.Append("<td>故障描述</td>");
                sb6.Append("<td>处理状态</td>");
                sb6.Append("<td>PM</td>");
                sb6.Append("<td>供应商</td>");
                sb6.Append("<td>金额</td>");
                sb6.Append("</tr>");
            }
            foreach (DataRow dr in listds6.Tables[0].Rows)
            {
                string statusName = AppEnum.GetRMAStatus(Util.TrimIntNull(dr["status"]));
                string pmName = null;
                int PMSysNo = Util.TrimIntNull(dr["PMUserSysNo"]);

                UserInfo userinfo = SysManager.GetInstance().LoadUser(PMSysNo);

                if (userinfo != null)
                    pmName = Util.TrimNull(userinfo.UserName);
                else
                    pmName = AppConst.StringNull;

                string VendorName = null;
                if (Util.TrimNull(dr["VendorName"]) != AppConst.StringNull)
                    VendorName = Util.TrimNull(dr["VendorName"]).ToString();
                else
                    VendorName = "&nbsp;&nbsp;";

                string LastVendorSysNo = null;
                if (Util.TrimIntNull(dr["LastVendorSysNo"]) != AppConst.IntNull)
                    LastVendorSysNo = "(" + Util.TrimIntNull(dr["LastVendorSysNo"]).ToString() + ")";
                else
                    LastVendorSysNo = "&nbsp;&nbsp;";

                string Cost = null;
                if (Util.TrimDecimalNull(dr["Cost"]) != AppConst.DecimalNull)
                    Cost = Util.TrimDecimalNull(dr["Cost"]).ToString(AppConst.DecimalFormat);
                else
                    Cost = "&nbsp;&nbsp;";

                sb6.Append("<tr>");
                sb6.Append("<td>" + dr["SysNo"] + "</td>");
                sb6.Append("<td>" + dr["productid"] + "</td>");
                sb6.Append("<td width='250'>" + dr["productname"] + "</td>");
                sb6.Append("<td width='350'>" + dr["CustomerDesc"] + "</td>");
                sb6.Append("<td>" + statusName + "</td>");
                sb6.Append("<td>" + pmName + "</td>");
                sb6.Append("<td>" + VendorName + LastVendorSysNo + "</td>");
                sb6.Append("<td>" + Cost + "</td>");
                sb6.Append("</tr>");
            }

            sb6.Append("</table>");
            sb6.Append("<br>");
            //End
            //Begin
            //超时未结束物品清单
            string listsql7 = @"select rma_register.SysNo,product.productid,product.productname,rma_register.CustomerDesc,rma_register.status,product.PMUserSysNo,vendor.VendorName,lpi.LastVendorSysNo,rma_register.Cost  from rma_request (NOLOCK) 
                             inner join rma_request_item (NOLOCK) on rma_request.sysno = rma_request_item.requestsysno
                             inner join rma_register (NOLOCK) on rma_register.sysno = rma_request_item.registersysno 
                             inner join product (NOLOCK) on rma_register.productsysno = product.sysno
                             left join product_lastpoinfo  as lpi on product.sysno = lpi.productsysno
                             left join vendor (NOLOCK) on vendor.sysno = lpi.lastvendorsysno
                             where rma_register.status=" + (int)AppEnum.RMARequestStatus.Handling + " and rma_request.recvtime <= '" + today.AddDays(-21) + "'and product.PMUserSysNo=" + sysno + " order by rma_register.SysNo desc";
            DataSet listds7 = SqlHelper.ExecuteDataSet(listsql7);

            int OverTimeNum = (int)listds7.Tables[0].Rows.Count;

            decimal OverTimeCost = 0;
            foreach (DataRow dr in listds7.Tables[0].Rows)
            {
                if (Util.TrimDecimalNull(dr["Cost"]) != AppConst.DecimalNull)
                    OverTimeCost = OverTimeCost + Util.TrimDecimalNull(dr["Cost"]);
            }

            StringBuilder sb7 = new StringBuilder(1000);
            sb7.Append("<table><font color=blue>超时未结束物品清单</font></table>");
            sb7.Append("<br>");
            sb7.Append("共" + OverTimeNum + "件,共" + OverTimeCost.ToString(AppConst.DecimalFormatWithGroup) + "元");
            sb7.Append("<br>");
            sb7.Append("<table border=1 cellspacing=0 cellspadding=0>");
            if (!Util.HasMoreRow(listds7))
            {
                sb7.Append("<tr>");
                sb7.Append("<td width=100%><font color=#ff0000>记录为空！</font></td>");
                sb7.Append("</tr>");
            }
            else if (Util.HasMoreRow(listds7))
            {
                sb7.Append("<tr>");
                sb7.Append("<td>单号</td>");
                sb7.Append("<td>产品号</td>");
                sb7.Append("<td>商品名称</td>");
                sb7.Append("<td>故障描述</td>");
                sb7.Append("<td>处理状态</td>");
                sb7.Append("<td>PM</td>");
                sb7.Append("<td>供应商</td>");
                sb7.Append("<td>金额</td>");
                sb7.Append("</tr>");
            }
            foreach (DataRow dr in listds7.Tables[0].Rows)
            {
                string statusName = AppEnum.GetRMAStatus(Util.TrimIntNull(dr["status"]));
                string pmName = null;
                int PMSysNo = Util.TrimIntNull(dr["PMUserSysNo"]);

                UserInfo userinfo = SysManager.GetInstance().LoadUser(PMSysNo);

                if (userinfo != null)
                    pmName = Util.TrimNull(userinfo.UserName);
                else
                    pmName = AppConst.StringNull;

                string VendorName = null;
                if (Util.TrimNull(dr["VendorName"]) != AppConst.StringNull)
                    VendorName = Util.TrimNull(dr["VendorName"]).ToString();
                else
                    VendorName = "&nbsp;&nbsp;";

                string LastVendorSysNo = null;
                if (Util.TrimIntNull(dr["LastVendorSysNo"]) != AppConst.IntNull)
                    LastVendorSysNo = "(" + Util.TrimIntNull(dr["LastVendorSysNo"]).ToString() + ")";
                else
                    LastVendorSysNo = "&nbsp;&nbsp";

                string Cost = null;
                if (Util.TrimDecimalNull(dr["Cost"]) != AppConst.DecimalNull)
                    Cost = Util.TrimDecimalNull(dr["Cost"]).ToString(AppConst.DecimalFormat);
                else
                    Cost = "&nbsp;&nbsp;";

                sb7.Append("<tr>");
                sb7.Append("<td>" + dr["SysNo"] + "</td>");
                sb7.Append("<td>" + dr["productid"] + "</td>");
                sb7.Append("<td width='250'>" + dr["productname"] + "</td>");
                sb7.Append("<td width='350'>" + dr["CustomerDesc"] + "</td>");
                sb7.Append("<td>" + statusName + "</td>");
                sb7.Append("<td>" + pmName + "</td>");
                sb7.Append("<td>" + VendorName + LastVendorSysNo + "</td>");
                sb7.Append("<td>" + Cost + "</td>");
                sb7.Append("</tr>");
            }

            sb7.Append("</table>");
            sb7.Append("<br>");
            //End
            //
            int allRegisterNum = 0;
            decimal allRegisterCost = 0;
            //
            StringBuilder sbAll = new StringBuilder(1000);
            sbAll.Append("<table border=1 width='30%'><tr width='100%'><font color=blue>" + year.ToString() + "年" + month.ToString() + "月" + day.ToString() + "日RMA情况汇总表</font></tr>");
            sbAll.Append("<tr><td>分类</td><td>数量</td><td>金额</td></tr>");
            sbAll.Append("<tr><td>当日接受件</td><td>" + RecRegisterNum.ToString() + "</td><td>" + RecRegisterCost.ToString(AppConst.DecimalFormatWithGroup) + "</td></tr>");
            sbAll.Append("<tr><td>当日返还物品清单</td><td>" + RevertedNum.ToString() + "</td><td>" + RevertedCost.ToString(AppConst.DecimalFormatWithGroup) + "</td></tr>");
            sbAll.Append("<tr><td>当日退款物品清单</td><td>" + RefundedNum.ToString() + "</td><td>" + RefundedCost.ToString(AppConst.DecimalFormatWithGroup) + "</td></tr>");
            sbAll.Append("<tr><td>当日发新物品清单</td><td>" + RevertedNewNum.ToString() + "</td><td>" + RevertedNewCost.ToString(AppConst.DecimalFormatWithGroup) + "</td></tr>");
            sbAll.Append("<tr><td>当日退货入库清单</td><td>" + ReturnedNum.ToString() + "</td><td>" + ReturnedCost.ToString(AppConst.DecimalFormatWithGroup) + "</td></tr>");
            sbAll.Append("<tr><td>超时未结束物品清单</td><td>" + OverTimeNum.ToString() + "</td><td>" + OverTimeCost.ToString(AppConst.DecimalFormatWithGroup) + "</td></tr>");
            sbAll.Append("<tr><td>总计</td><td>" + allRegisterNum.ToString() + "</td><td>" + allRegisterCost.ToString(AppConst.DecimalFormatWithGroup) + "</td></tr>");
            sbAll.Append("</table>");
            sbAll.Append("<br>");
            //send mail
            if (emailAddress == AppConst.StringNull)
                return;
            
            string mailBody = sbAll.ToString() + sb1.ToString() + sb3.ToString() + sb4.ToString() + sb5.ToString() + sb6.ToString() + sb7.ToString();

            //Email_InternalInfo oEmail = new Email_InternalInfo(emailAddress, "", "", "每日RMA情况表", mailBody);
            //Email_InternalManager.GetInstance().InsertEmail(oEmail);
        }
        public void SendMailToPm()
        {
            string sql = @"select distinct pmusersysno,email from pm_master(nolock) inner join  sys_user(nolock) on sys_user.sysno=pm_master.pmusersysno  and pm_master.status=0 ";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            DataTable dt = ds.Tables[0];
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    mailSpecialforPM(Convert.ToInt32(dr["pmusersysno"]), dr["email"].ToString());
                }
            }

        }
        #endregion


        public bool IfExistValidRMA(int soSysNo)
        {
            bool ifExist;
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                string sql = "select sysno from rma_request (NOLOCK) where (status=" + (int)AppEnum.RMARequestStatus.Orgin + " or status=" + (int)AppEnum.RMARequestStatus.Handling + ") and sosysno=" + soSysNo;
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (Util.HasMoreRow(ds))
                    ifExist = true;
                else
                    ifExist = false;
                scope.Complete();
            }
            return ifExist;
        }

        public int GetRequestSysNofromID(string RequestID)
        {
            string sql = "select sysno from rma_request (NOLOCK) where RequestID=" + Util.ToSqlString(RequestID.ToString());
            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (Util.HasMoreRow(ds))
                return (int)ds.Tables[0].Rows[0][0];
            else
                return AppConst.IntNull;
          
        }

        public int GetFreightUserSysNofromID(string RequestID)
        {
            string sql = "select FreightUserSysNo from rma_request (NOLOCK) where RequestID=" + Util.ToSqlString(RequestID.ToString());
            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (Util.HasMoreRow(ds))
                return (int)ds.Tables[0].Rows[0][0];
            else
                return AppConst.IntNull;

        }

        public int GetDLSysNofromID(string RequestID)
        {
            string sql = "select DLSysNo from rma_request (NOLOCK) where RequestID=" + Util.ToSqlString(RequestID.ToString());
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

        public DataSet GetFreightMenDs(Hashtable paramHash)
        {
            string sql = @"select rma_request.*,area.DistrictName,area.localcode,sys_user.username as freightusername
                          from rma_request,area ,sys_user
                          where  AreaSysNo=area.sysno and sys_user.sysno=FreightUserSysNo";


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
                }
                sql += sb.ToString();
            }
            sql += sql1;
            return SqlHelper.ExecuteDataSet(sql);
        }

        #region  Import

        //public void ImportRequest()
        //{
        //    string sqlExamRequest = "select top 1 * from RMA_Request";
        //    DataSet dsExamRequest = SqlHelper.ExecuteDataSet(sqlExamRequest);
        //    if(Util.HasMoreRow(dsExamRequest))
        //        throw new BizException("The RMA_Request is not empty!");

        //    string sqlExamRegister = "select top 1 * from RMA_Register";
        //    DataSet dsExamRegister = SqlHelper.ExecuteDataSet(sqlExamRegister);
        //    if(Util.HasMoreRow(dsExamRegister))
        //        throw new BizException("The RMA_Register is not empty!");



        //    TransactionOptions options = new TransactionOptions();
        //    options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
        //    options.Timeout = TransactionManager.DefaultTimeout;

        //    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
        //    {
        //        string sqlMaster = "select * from ipp3..RMA_Master";
        //        DataSet dsMaster = SqlHelper.ExecuteDataSet(sqlMaster);
        //        foreach(DataRow drMaster in dsMaster.Tables[0].Rows)
        //        {						
        //            RMARequestInfo oRequest = new RMARequestInfo();
        //            CustomerInfo oCustomer = CustomerManager.GetInstance().Load(Util.TrimIntNull(drMaster["CustomerSysNo"]));
        //            try
        //            {
        //                if(oCustomer != null)
        //                {
        //                    if(oCustomer.ReceiveAddress != AppConst.StringNull)
        //                        oRequest.Address = oCustomer.ReceiveAddress;
        //                    else
        //                        oRequest.Address = "无";
        //                    if(oCustomer.ReceiveContact != AppConst.StringNull)
        //                        oRequest.Contact = oCustomer.ReceiveContact;
        //                    else
        //                        oRequest.Contact = "无";
        //                    if(oCustomer.ReceivePhone != AppConst.StringNull)
        //                        oRequest.Phone = oCustomer.ReceivePhone ;
        //                    else
        //                        oRequest.Phone = "无";
        //                }						
        //                else
        //                {
        //                    oRequest.Address = "无";
        //                    oRequest.Contact = "无";
        //                    oRequest.Phone = "无";
        //                }						
        //                oRequest.CreateTime = Util.TrimDateNull(drMaster["CreateTime"]);
        //                oRequest.CustomerSysNo = Util.TrimIntNull(drMaster["CustomerSysNo"]);
        //                oRequest.Note = drMaster["RMANote"].ToString() + drMaster["CCNote"].ToString();

        //                oRequest.RecvTime = Util.TrimDateNull(drMaster["ReceiveTime"]);
        //                oRequest.RecvUserSysNo = Util.TrimIntNull(drMaster["ReceiveUserSysNo"]);
        //                oRequest.RequestID = drMaster["RMAID"].ToString();
        //                oRequest.SysNo = SequenceDac.GetInstance().Create("RMA_Request_Sequence");
        //                oRequest.SOSysNo = Util.TrimIntNull(drMaster["SOSysNo"]);
        //                int status = Util.TrimIntNull(drMaster["Status"]);
        //                if(status == -1)
        //                    oRequest.Status = -1;
        //                else if(status == 0)
        //                    oRequest.Status = 0;
        //                else if(status == 4)
        //                    oRequest.Status = 2 ;
        //                else
        //                    oRequest.Status = 1;

        //                this.InsertRequest(oRequest);
        //            }					
        //            catch
        //            {
        //                throw new BizException("Import RMA_Master error! RMAID =" + drMaster["RMAID"].ToString());
        //            }

        //            string sqlItem = "select * from ipp3..RMA_Item where RMASysNo=" + drMaster["SysNo"].ToString();
        //            DataSet dsItem = SqlHelper.ExecuteDataSet(sqlItem);
        //            if(Util.HasMoreRow(dsItem))
        //            {
        //                foreach(DataRow drItem in dsItem.Tables[0].Rows)
        //                {
        //                    for(int i =1 ; i <= Util.TrimIntNull(drItem["RMAQty"]) ; i ++)
        //                    {
        //                        try
        //                        {
        //                            RMARegisterInfo oRegister = new RMARegisterInfo();
        //                            oRegister.CheckTime = Util.TrimDateNull(drMaster["ReceiveTime"]);									
        //                            if(drItem["RMADesc"].ToString() != AppConst.StringNull)
        //                                oRegister.CustomerDesc = drItem["RMADesc"].ToString();
        //                            else
        //                                oRegister.CustomerDesc = "无";
        //                            oRegister.ProductSysNo = Util.TrimIntNull(drItem["ProductSysNo"]);
        //                            oRegister.RequestType = Util.TrimIntNull(drItem["RMAType"]);
        //                            oRegister.Status = oRequest.Status;
        //                            oRegister.SysNo = SequenceDac.GetInstance().Create("RMA_Register_Sequence");
        //                            RMARegisterManager.GetInstance().InsertRegister(oRegister);

        //                            RMARequestItemInfo oRequestItem = new RMARequestItemInfo();
        //                            oRequestItem.RegisterSysNo = oRegister.SysNo;
        //                            oRequestItem.RequestSysNo = oRequest.SysNo;
        //                            RMARequestManager.GetInstance().InsertRequestItem(oRequestItem);

        //                            RMAConvertInfo oConvert = new RMAConvertInfo();
        //                            oConvert.ProductSysNo = Util.TrimIntNull(drItem["ProductSysNo"]);
        //                            oConvert.RegisterSysNo = oRegister.SysNo;
        //                            oConvert.RMASysNo = Util.TrimIntNull(drMaster["SysNo"]);
        //                            new RMAConvertDac().Insert(oConvert);
        //                        }
        //                        catch
        //                        {
        //                            throw new BizException("Import RMA_Item Error! RMA_Item.SysNo = " + drItem["SysNo"].ToString() );
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        scope.Complete();
        //    }	
        //}

        #endregion

    }
}
