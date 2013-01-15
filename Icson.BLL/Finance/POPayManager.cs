using System;
using System.Data;
using System.Collections;
using System.Text;

using Icson.Utils;
using System.Transactions;

using Icson.Objects.Purchase;
using Icson.Objects.Finance;

using Icson.DBAccess;
using Icson.DBAccess.Finance;
using Icson.Objects;
using Icson.BLL.Purchase;
using Icson.BLL;
using Icson.BLL.Sale;


namespace Icson.BLL.Finance
{
    /// <summary>
    /// Summary description for POPayManager.
    /// </summary>
    public class POPayManager
    {
        private POPayManager()
        {
        }
        private static POPayManager _instance;
        public static POPayManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new POPayManager();
            }
            return _instance;
        }
        private void map(POPayInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.POSysNo = Util.TrimIntNull(tempdr["POSysNo"]);
            oParam.CurrencySysNo = Util.TrimIntNull(tempdr["CurrencySysNo"]);
            oParam.POAmt = Util.TrimDecimalNull(tempdr["POAmt"]);
            oParam.AlreadyPayAmt = Util.TrimDecimalNull(tempdr["AlreadyPayAmt"]);
            oParam.PayStatus = Util.TrimIntNull(tempdr["PayStatus"]);
            oParam.InvoiceStatus = Util.TrimIntNull(tempdr["InvoiceStatus"]);
            oParam.InvoiceTime = Util.TrimDateNull(tempdr["InvoiceTime"]);
            oParam.Note = Util.TrimNull(tempdr["Note"]);
        }


        private void map(POPayItemInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.POSysNo = Util.TrimIntNull(tempdr["POSysNo"]);
            oParam.PayStyle = Util.TrimIntNull(tempdr["PayStyle"]);
            oParam.PayAmt = Util.TrimDecimalNull(tempdr["PayAmt"]);
            oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
            oParam.CreateUserSysNo = Util.TrimIntNull(tempdr["CreateUserSysNo"]);
            oParam.EstimatePayTime = Util.TrimDateNull(tempdr["EstimatePayTime"]);
            oParam.ReferenceID = Util.TrimNull(tempdr["ReferenceID"]);
            oParam.PayTime = Util.TrimDateNull(tempdr["PayTime"]);
            oParam.PayUserSysNo = Util.TrimIntNull(tempdr["PayUserSysNo"]);
            oParam.Note = Util.TrimNull(tempdr["Note"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
            oParam.IsPrintPOPayBill = Util.TrimIntNull(tempdr["IsPrintPOPayBill"]);
            oParam.RequestUserSysNo = Util.TrimIntNull(tempdr["RequestUserSysNo"]);
            oParam.RequestTime = Util.TrimDateNull(tempdr["RequestTime"]);
            oParam.AuditUserSysNo = Util.TrimIntNull(tempdr["AuditUserSysNo"]);
            oParam.AuditTime = Util.TrimDateNull(tempdr["AuditTime"]);
            oParam.VoucherTime = Util.TrimDateNull(tempdr["VoucherTime"]);
        }
        public POPayItemInfo LoadPayItem(int sysno)
        {
            string sql = "select * from finance_popay_item where sysno =" + sysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;
            POPayItemInfo oPayItem = new POPayItemInfo();
            map(oPayItem, ds.Tables[0].Rows[0]);
            return oPayItem;
        }
        public POPayInfo LoadPay(int posysno)
        {
            string sql = "select * from finance_popay where posysno =" + posysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;
            POPayInfo oPay = new POPayInfo();
            map(oPay, ds.Tables[0].Rows[0]);
            return oPay;
        }


        public DataSet GetPOCanbePayDs(Hashtable paramHash)
        {
            string sql = @"
							select 
								a.sysno, a.poid, a.status 
							from 
								po_master a
							left join finance_popay b on a.sysno = b.posysno
							where
								((a.status = " + (int)AppEnum.POStatus.WaitingApportion + " or a.status=" + (int)AppEnum.POStatus.WaitingReceive + " or a.status = " + (int)AppEnum.POStatus.WaitingInStock + ") "
                        + "or	(a.status = " + (int)AppEnum.POStatus.InStock + " and b.paystatus <> " + (int)AppEnum.POPayStatus.FullPay + ") )";
            if (paramHash != null && paramHash.Count != 0)
            {
                StringBuilder sb = new StringBuilder(100);
                foreach (string key in paramHash.Keys)
                {

                    sb.Append(" and ");
                    object item = paramHash[key];
                    if (key == "POStatus")
                    {
                        sb.Append("a.status=").Append(item.ToString());
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

            sql += " order by a.sysno desc";
            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetPOPayItemDs(Hashtable paramHash)
        {
            string sql = @"select 
								a.sysno, a.posysno, po.poid,
								po.currencysysno, a.payamt, a.paystyle,vendor.vendorname,
								a.status, a.createtime, a.estimatepaytime, a.paytime,a.AuditTime,a.RequestTime,
                                a.VoucherTime,a.ReferenceID,po.MinusPOType
							from 
								finance_popay_item a,
								po_master po,
								vendor
							where
								a.posysno = po.sysno
							and po.vendorsysno = vendor.sysno
							";
            if (paramHash != null && paramHash.Count != 0)
            {
                StringBuilder sb = new StringBuilder(100);
                foreach (string key in paramHash.Keys)
                {

                    sb.Append(" and ");
                    object item = paramHash[key];
                    if (key == "Status")
                    {
                        sb.Append("a.status=").Append(item.ToString());
                    }
                    else if (key == "CurrencySysNo")
                    {
                        sb.Append("po.currencysysno=").Append(item.ToString());
                    }
                    else if (key == "CreateUserSysNo")
                    {
                        sb.Append("po.createusersysno=").Append(item.ToString());
                    }
                    else if (key == "DateFromCreate")
                    {
                        sb.Append("a.CreateTime >= ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "DateToCreate")
                    {
                        sb.Append("a.CreateTime <= ").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "DateFromEstimate")
                    {
                        sb.Append("a.EstimatePayTime >= ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "DateToEstimate")
                    {
                        sb.Append("a.EstimatePayTime <= ").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "DateFromPay")
                    {
                        sb.Append("a.PayTime >= ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "DateToPay")
                    {
                        sb.Append("a.PayTime <= ").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "DateFromPayDate")
                    {
                        sb.Append("po.paydate >= ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "DateToPayDate")
                    {
                        sb.Append("po.paydate <= ").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "VoucherTimeFrom")
                    {
                        sb.Append("a.VoucherTime>=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "VoucherTimeTo")
                    {
                        sb.Append("a.VoucherTime <=").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "AuditTimeFrom")
                    {
                        sb.Append(" a.AuditTime >=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "AuditTimeTo")
                    {
                        sb.Append(" a.AuditTime <=").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "RequestTimeFrom")
                    {
                        sb.Append(" a.RequestTime >=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "RequestTimeTo")
                    {
                        sb.Append(" a.RequestTime <=").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "Note")
                    {
                        sb.Append("a.Note like ").Append(Util.ToSqlLikeString(item.ToString()));
                    }
                    else if (key == "VendorName")
                    {
                        sb.Append("vendor.VendorName like ").Append(Util.ToSqlLikeString(item.ToString()));
                    }
                    else if (key == "POInvoiceType")
                    {
                        sb.Append("po.POInvoiceType = ").Append(item.ToString());
                    }
                    else if (key == "POItemSysNo")
                    {
                        sb.Append(" a.SysNo like ").Append(Util.ToSqlLikeString(item.ToString()));
                    }
                    else if (key == "OrderBy")
                    {
                        sb.Append(" 1=1 ");
                    }
                    else if (key == "IsOnlyShowErrRequestItem")
                    {
                        sb.Append("a.SysNo IN(").Append("SELECT  POPayItemSysNo FROM dbo.Fin_POPay_Item_ErrRequest WHERE Status IN (" + (int)AppEnum.POPayItemErrRequestStatus.PMRequest + "," + (int)AppEnum.POPayItemErrRequestStatus.TLRequst).Append("))");
                    }
                    else if (key == "NeedAuditBySelf")
                    {
                        sb.Append("a.SysNo IN(").Append("SELECT POPayItemSysNo FROM  dbo.Fin_POPay_Item_ErrRequest WHERE TLAuditUserSysNo=").Append(item.ToString()).Append(" OR LastAuditUserSysNo = ").Append(item.ToString());
                        sb.Append("                      UNION ");
                        sb.Append("                      SELECT dbo.Finance_POPay_Item.SysNo FROM dbo.Finance_POPay_Item JOIN dbo.PO_Master ON dbo.PO_Master.SysNo = dbo.Finance_POPay_Item.POSysNo WHERE dbo.PO_Master.CreateUserSysNo=").Append(item.ToString()).Append(")");
                    }
                    else if (key == "OnlyShowReturnErr")
                    {
                        sb.Append("a.SysNo IN(").Append(@"SELECT POPayItemSysNo FROM (SELECT MAX(Status) Status,POPayItemSysNo FROM (SELECT * FROM dbo.Fin_POPay_Item_ErrRequest WHERE Status NOT IN (-1)) t GROUP BY t.POPayItemSysNo)t1 WHERE t1.Status<-1").Append(")");
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

            if (paramHash != null && paramHash.Count > 0)
            {
                if (paramHash.ContainsKey("OrderBy"))
                    sql += " order by " + paramHash["OrderBy"].ToString();
                else
                    sql += " order by posysno desc, a.sysno desc";
            }
            else
                sql += " order by posysno desc, a.sysno desc";
            return SqlHelper.ExecuteDataSet(sql);
        }
        /// <summary>
        /// 获取应付款的列表
        /// </summary>
        /// <param name="paramHash"></param>
        /// <returns></returns>
        public DataSet GetPOPayDs(Hashtable paramHash)
        {
            string sql = @"select 
								pay.posysno, poid, po.currencysysno,
								vendorname, po.status as postatus,
								pay.paystatus, pay.invoicestatus,po.POInvoiceDunDesc,PO.InvoiceExpectReceiveDate,po.POInvoiceType,po.CreateUserSysNo,
								pay.poamt, pay.alreadypayamt, po.createtime,po.intime,payitem.paytime,pay.invoiceTime
							from 
								finance_popay pay,
								vendor,
								po_master po left join Finance_POPay_Item payitem on payitem.posysno = po.sysno 
							where
								pay.posysno = po.sysno 
							and po.vendorsysno = vendor.sysno and payitem.status <> " + (int)AppEnum.POPayItemStatus.Abandon;
            if (paramHash != null && paramHash.Count != 0)
            {
                StringBuilder sb = new StringBuilder(100);
                foreach (string key in paramHash.Keys)
                {
                    sb.Append(" and ");
                    object item = paramHash[key];
                    if (key == "POStatus")
                    {
                        sb.Append("po.status=").Append(item.ToString());
                    }
                    else if (key == "CurrencySysNo")
                    {
                        sb.Append("pay.currencysysno=").Append(item.ToString());
                    }
                    else if (key == "DateFromCreate")
                    {
                        sb.Append("po.CreateTime >= ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "DateToCreate")
                    {
                        sb.Append("po.CreateTime <= ").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "InTimeFrom")
                    {
                        sb.Append("po.InTime >= ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "InTimeTo")
                    {
                        sb.Append("po.InTime <= ").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "PayTimeFrom")
                    {
                        sb.Append("payitem.PayTime >= ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "PayTimeTo")
                    {
                        sb.Append("payitem.PayTime <= ").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "InvoiceNo")
                    {
                        sb.Append("pay.note like ").Append(Util.ToSqlLikeString(item.ToString()));
                    }
                    else if (key == "InvoiceTimeFrom")
                    {
                        sb.Append("pay.InvoiceTime >=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "InvoiceTimeTo")
                    {
                        sb.Append("pay.InvoiceTime <= ").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "IsInvoiceNeedDun")
                    {
                        if (Util.TrimIntNull(paramHash["IsInvoiceNeedDun"]) == (int)AppEnum.YNStatus.Yes)
                        {
                            sb.Append("exists(select * from PO_Master pom left join Finance_POPay popay on pom.sysno=popay.posysno where isnull( pom.InvoiceExpectReceiveDate," + Util.ToSqlString(AppConst.DateTimeNull.ToString()) + ")<" + Util.ToSqlString(DateTime.Now.AddDays(1).ToString()) + " and pom.POInvoiceType=" + (int)AppEnum.POInvoiceType.ValueAddedInvoice + "and popay.InvoiceStatus!=" + (int)AppEnum.POPayInvoiceStatus.Complete + "and popay.PayStatus=" + (int)AppEnum.POPayStatus.FullPay + " and po.sysno=pom.sysno)");
                        }
                        else
                        {
                            sb.Append("not exists(select * from PO_Master pom left join Finance_POPay popay on pom.sysno=popay.posysno where isnull( pom.InvoiceExpectReceiveDate," + Util.ToSqlString(AppConst.DateTimeNull.ToString()) + ")<" + Util.ToSqlString(DateTime.Now.AddDays(1).ToString()) + " and pom.POInvoiceType=" + (int)AppEnum.POInvoiceType.ValueAddedInvoice + "and popay.InvoiceStatus!=" + (int)AppEnum.POPayInvoiceStatus.Complete + "and popay.PayStatus=" + (int)AppEnum.POPayStatus.FullPay + " and po.sysno=pom.sysno)");
                        }
                    }
                    else if (key == "CreateUserSysNo")
                    {
                        sb.Append("po.CreateUserSysNo = ").Append(Util.ToSqlString(item.ToString()));
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

            sql += " order by pay.posysno desc";
            return SqlHelper.ExecuteDataSet(sql);
        }
        /// <summary>
        /// 插入付款单
        /// 1 waiting apportion 和 waiting instock 的po只能增加预付的付款单
        /// 2 InStock的po只能加入正常支付的付款单
        /// 3 加入正常的付款单，不能有初始状态的预付付款单
        /// 4 是否有应付款，a如果没有，增加之；b如果有作废的应付款，更新为unpay状态；c如果应付款已经完全支付，不能加入付款单了。
        /// 5 最后加入付款单
        /// </summary>
        /// <param name="oParam"></param>
        public void InsertPOPayItem(POPayItemInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                POInfo oPO = PurchaseManager.GetInstance().LoadPO(oParam.POSysNo);
                if (oPO.Status == (int)AppEnum.POStatus.WaitingApportion || oPO.Status == (int)AppEnum.POStatus.WaitingReceive || oPO.Status == (int)AppEnum.POStatus.WaitingInStock)
                {
                    if (oParam.PayStyle != (int)AppEnum.POPayItemStyle.Advanced)
                        throw new BizException("this po status allow only advanced pay");
                }
                else if (oPO.Status == (int)AppEnum.POStatus.InStock)
                {
                    if (oParam.PayStyle != (int)AppEnum.POPayItemStyle.Normal)
                        throw new BizException("this po status allow only normal pay");
                }
                else
                {
                    throw new BizException("this po status allow no pay");
                }

                if (oParam.PayStyle == (int)AppEnum.POPayItemStyle.Normal)
                {
                    string sqlExist = "select top 1 sysno from finance_popay_item where posysno=" + oParam.POSysNo
                        + " and status=" + (int)AppEnum.POPayItemStatus.Origin
                        + " and paystyle=" + (int)AppEnum.POPayItemStyle.Advanced;
                    DataSet ds = SqlHelper.ExecuteDataSet(sqlExist);
                    if (Util.HasMoreRow(ds))
                        throw new BizException("there is an bill - status(origin), paystyle(advanced), insert normal pay failed");
                }


                POPayInfo oPay = LoadPay(oParam.POSysNo);
                if (oPay == null)
                {
                    oPay = new POPayInfo();
                    oPay.POSysNo = oPO.SysNo;
                    oPay.CurrencySysNo = oPO.CurrencySysNo;
                    oPay.POAmt = oPO.TotalAmt;
                    oPay.AlreadyPayAmt = 0;
                    oPay.PayStatus = (int)AppEnum.POPayStatus.UnPay;
                    oPay.InvoiceStatus = (int)AppEnum.POPayInvoiceStatus.Absent;
                    oPay.Note = "auto create";
                    new POPayDac().InsertMaster(oPay);
                }
                else if (oPay.PayStatus == (int)AppEnum.POPayStatus.FullPay)
                {
                    throw new BizException("Full Pay already, need no pay bill");
                }
                else if (oPay.PayStatus == (int)AppEnum.POPayStatus.Abandon)
                {
                    Hashtable ht = new Hashtable(2);
                    ht.Add("SysNo", oPay.SysNo);
                    ht.Add("PayStatus", (int)AppEnum.POPayStatus.UnPay);
                    new POPayDac().UpdateMaster(ht);
                }

                new POPayDac().InsertItem(oParam);



                scope.Complete();
            }
        }
        /// <summary>
        /// 可以修改初始状态的付款单
        /// </summary>
        /// <param name="payItemSysNo"></param>
        /// <param name="payAmt"></param>
        /// <param name="estimatePayTime"></param>
        /// <param name="note"></param>
        public void UpdatePOPayItem(int payItemSysNo, decimal payAmt, DateTime estimatePayTime, string note, string referenceID)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                POPayItemInfo oPayItem = LoadPayItem(payItemSysNo);
                if (oPayItem.Status != (int)AppEnum.POPayItemStatus.Origin)
                    throw new BizException("pay bill is not origin status now, abandon failed");

                Hashtable ht = new Hashtable(5);
                ht.Add("SysNo", payItemSysNo);
                ht.Add("payAmt", payAmt);
                ht.Add("EstimatePayTime", estimatePayTime);
                ht.Add("ReferenceID", referenceID);
                ht.Add("Note", note);

                new POPayDac().UpdateItem(ht);

                scope.Complete();
            }
        }

        /// <summary>
        /// 1只能作废初始状态的付款单
        /// 3作废付款单
        /// 2如果如果没有有效的付款单了，就作废应收款
        /// </summary>
        /// <param name="payItemSysNo"></param>
        public void AbandonPOPayItem(int payItemSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                POPayItemInfo oPayItem = LoadPayItem(payItemSysNo);
                if (oPayItem.Status != (int)AppEnum.POPayItemStatus.WaitingAudit && oPayItem.Status != (int)AppEnum.POPayItemStatus.Origin)
                    throw new BizException("pay bill is not WaitingAudit or Origin status now, abandon failed");

                Hashtable ht = new Hashtable(5);
                ht.Add("SysNo", payItemSysNo);
                ht.Add("Status", (int)AppEnum.POPayItemStatus.Abandon);

                new POPayDac().UpdateItem(ht);

                //如果该付款单存在异常申请记录，则取消该异常申请记录
                POPayItemErrRequestInfo errRequestInfo = LoadPOPayItemErrRequest(payItemSysNo, true);
                if (errRequestInfo != null)
                    CancelErrRequest(errRequestInfo.SysNo);

                //如果没有有效的付款单， 就作废应付款 
                string sql = "select * from finance_popay_item where posysno= " + oPayItem.POSysNo + " and status <>" + (int)AppEnum.POPayItemStatus.Abandon;
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (!Util.HasMoreRow(ds))
                {
                    POPayInfo oPay = LoadPay(oPayItem.POSysNo);
                    Hashtable htPay = new Hashtable(2);
                    htPay.Add("SysNo", oPay.SysNo);
                    htPay.Add("PayStatus", (int)AppEnum.POPayStatus.Abandon);
                    new POPayDac().UpdateMaster(htPay);
                }


                scope.Complete();
            }

            return;
        }
        /// <summary>
        /// 1 只能取消作废已经作废的付款单
        /// 2 如果po的当前状态是waiting apportion, waiting in stock; 只能恢复advanced的付款单
        /// 3 如果po的当前状态是instock, 只能恢复normal的付款单
        /// </summary>
        /// <param name="payItemSysNo"></param>
        public void CancelAbandonPOPayItem(int payItemSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                POPayItemInfo oPayItem = LoadPayItem(payItemSysNo);
                if (oPayItem.Status != (int)AppEnum.POPayItemStatus.Abandon)
                    throw new BizException("pay bill is not abandon status now, cancel abandon failed");

                POInfo oPO = PurchaseManager.GetInstance().LoadPO(oPayItem.POSysNo);
                if (oPO.Status == (int)AppEnum.POStatus.WaitingApportion || oPO.Status == (int)AppEnum.POStatus.WaitingReceive || oPO.Status == (int)AppEnum.POStatus.WaitingInStock)
                {
                    if (oPayItem.PayStyle != (int)AppEnum.POPayItemStyle.Advanced)
                        throw new BizException("this po status allow only advanced pay");
                }
                else if (oPO.Status == (int)AppEnum.POStatus.InStock)
                {
                    if (oPayItem.PayStyle != (int)AppEnum.POPayItemStyle.Normal)
                        throw new BizException("this po status allow only normal pay");
                }
                else
                {
                    throw new BizException("this po status allow no pay");
                }

                POPayInfo oPay = LoadPay(oPayItem.POSysNo);
                if (oPay.PayStatus == (int)AppEnum.POPayStatus.FullPay)
                {
                    throw new BizException("Full Pay already, need no pay bill");
                }
                else if (oPay.PayStatus == (int)AppEnum.POPayItemStatus.Abandon)
                {
                    Hashtable htPay = new Hashtable(3);
                    htPay.Add("SysNo", oPay.SysNo);
                    htPay.Add("POAmt", oPO.TotalAmt);
                    htPay.Add("AlreadyPayAmt", 0M);
                    htPay.Add("PayStatus", (int)AppEnum.POPayStatus.UnPay);
                    new POPayDac().UpdateMaster(htPay);
                }

                Hashtable ht = new Hashtable(2);
                ht.Add("SysNo", payItemSysNo);
                if (oPayItem.IsPrintPOPayBill == (int)AppEnum.YNStatus.Yes)
                {
                    ht.Add("Status", (int)AppEnum.POPayItemStatus.WaitingAudit);
                }
                else
                    ht.Add("Status", (int)AppEnum.POPayItemStatus.Origin);


                new POPayDac().UpdateItem(ht);


                scope.Complete();
            }
        }

        /// <summary>
        /// 对PO的账期,增票是否到,是否存在负采购,是否存在超期返修件,支付金额是否大于库存金额,是否是最后一笔付款进行判断
        /// </summary>
        /// <param name="payItemSysNo"></param>
        /// <returns></returns>
        public string IsCanPayPOPayItem(int payItemSysNo)
        {
            string ErrorMsg = string.Empty;
            POPayItemInfo oPayItem = LoadPayItem(payItemSysNo);
            POPayInfo oPay = LoadPay(oPayItem.POSysNo);
            if (oPay.PayStatus == (int)AppEnum.POPayStatus.FullPay || oPay.PayStatus == (int)AppEnum.POPayStatus.Abandon)
            {
                //throw new BizException("Full Pay already, need no pay bill");
                ErrorMsg += "已经全部支付，无需再支付 " + " <br />";
            }

            //正常正采购付款对帐期和增票做判断
            POInfo poInfo = null;
            if (oPayItem.PayStyle == (int)AppEnum.POPayItemStyle.Advanced)
            {
                ErrorMsg += "这是一笔预付款 " + " <br />";
            }

            if (oPayItem.PayStyle == (int)AppEnum.POPayItemStyle.Normal)
            {
                poInfo = PurchaseManager.GetInstance().LoadPO(oPayItem.POSysNo);
                if (poInfo != null && poInfo.InTime != null)
                {
                    if (DateTime.Parse(poInfo.PayDate.AddDays(-1).ToString(AppConst.DateFormat) + " 00:00:01") >= DateTime.Now) //提前一天申请打印出来
                    {
                        //throw new BizException("帐期未到，不能付款");
                        ErrorMsg += "帐期未到 " + " <br />";
                    }
                    if (poInfo.POInvoiceType == (int)AppEnum.POInvoiceType.ValueAddedInvoice)
                    {
                        if (oPay.InvoiceStatus != (int)AppEnum.POPayInvoiceStatus.Complete)
                        {
                            //throw new BizException("增票未到，不能付款");
                            ErrorMsg += "增票未到 " + " <br />";
                        }
                    }
                }
            }


            //用于强制控制财务是否能付款 - 针对RMA
            //含未审核的负采购的，不能付款
            //含超期返修件的，不能付款
            //支付金额大于库存金额的不能付款

            //POInfo po = PurchaseManager.GetInstance().LoadPO(oPayItem.POSysNo);
            if (poInfo == null)
                poInfo = PurchaseManager.GetInstance().LoadPO(oPayItem.POSysNo);
            if (poInfo != null && poInfo.InTime != null)
            {
                //判断该PO是否存在负采购，存在的话不能付款
                if (IsExistsUnRequestPO(poInfo.VendorSysNo, -1, oPayItem.SysNo))
                {
                    //throw new BizException("存在未审核的负采购，不能付款！");
                    ErrorMsg += "存在对应供应商的未申请的负采购 <br />";
                }
                Hashtable ht = new Hashtable();
                ht.Add("OutBoundTimeTo", DateTime.Now.AddMonths(-1).ToString(AppConst.DateFormat));
                ht.Add("VendorSysNo", poInfo.VendorSysNo.ToString());
                DataSet dsOutBoundNoReturn = RMA.RMAOutBoundManager.GetInstance().GetOutBoundNoReturnDS(ht);

                //超期返修件的，不能付款
                if (Util.HasMoreRow(dsOutBoundNoReturn))
                {
                    //throw new BizException("存在一个月以上送修未返还商品，不能付款！");
                    ErrorMsg += "存在一个月以上送修未返还商品 " + " <br />";
                }

                //支付金额大于库存金额的不能付款
                if (GetVendorPayableAmt(poInfo.VendorSysNo) - oPay.POAmt < GetVendorInventoryAmt(AppConfig.DefaultStockSysNo, poInfo.VendorSysNo))
                {
                    ErrorMsg += "对供应商的应付总额-当前应付款小于库存金额 " + " <br />";
                }
            }

            //else if (oPay.POAmt < 0) //负采购，支付给供应商金额总和不能小于0
            //{
            //    if (poInfo == null)
            //        poInfo = PurchaseManager.GetInstance().LoadPO(oPayItem.POSysNo);
            //    if (poInfo != null)
            //    {
            //        if (IsPayTotalAmtLessThanZero(poInfo.VendorSysNo))
            //        {
            //            ErrorMsg += "负采购金额不能大于正采购金额的和 " + " <br />";
            //        }
            //    }
            //}

            //是否是最后一笔付款
            if (IsLastPayForVendor(oPay, poInfo))
            {
                ErrorMsg += "这是最后一笔付款" + " <br />";
            }

            return ErrorMsg;
        }

        /// <summary>
        /// 检测批量审核，是否供应商应付款总和小于0
        /// </summary>
        /// <param name="poPayItemList"></param>
        /// <returns></returns>
        public string IsCanAuditPOPayItem(ArrayList poPayItemList)
        {
            string isnotcanstr = "";
            for (int i = 0; i < poPayItemList.Count; i++)
            {
                int sysno = Util.TrimIntNull(poPayItemList[i]);
                POPayItemInfo oInfo = LoadPayItem(sysno);
                if (IsPayTotalAmtLessThanZero(PurchaseManager.GetInstance().LoadPO(oInfo.POSysNo).VendorSysNo))
                {
                    isnotcanstr += sysno.ToString() + ",";
                }
            }
            isnotcanstr = isnotcanstr.TrimEnd(',');
            return isnotcanstr;
        }

        /// <summary>
        /// 检测批量审核，是否存在异常单
        /// </summary>
        /// <param name="poPayItemList"></param>
        /// <returns></returns>
        public string IsErrRequest(ArrayList poPayItemList)
        {
            string isErrPOPayItemStr = "";

            string sql = @"
SELECT DISTINCT POPayItemSysNo FROM dbo.Fin_POPay_Item_ErrRequest
WHERE Status IN(" + (int)AppEnum.POPayItemErrRequestStatus.PMRequest + "," + (int)AppEnum.POPayItemErrRequestStatus.TLRequst + ") AND POPayItemSysNo IN " + Util.ToSqlInString(poPayItemList);
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    isErrPOPayItemStr += dr["POPayItemSysNo"].ToString() + ",";
                }
            }
            isErrPOPayItemStr = isErrPOPayItemStr.TrimEnd(',');
            return isErrPOPayItemStr;
        }

        /// <summary>
        /// 检测批量审核，是否存在未审核的负采购
        /// </summary>
        /// <param name="poPayItemList"></param>
        /// <returns></returns>
        public string IsExistsCanotAudit(ArrayList poPayItemList)
        {
            string s = "";

            for (int i = 0; i < poPayItemList.Count; i++)
            {
                int sysno = Util.TrimIntNull(poPayItemList[i]);
                POPayItemInfo oInfo = LoadPayItem(sysno);
                POInfo poInfo = PurchaseManager.GetInstance().LoadPO(oInfo.POSysNo);
                if (poInfo != null)
                {
                    if (IsExistsUnAuditPO(poInfo.VendorSysNo, -1, oInfo.SysNo))
                    {
                        s += sysno.ToString() + ",";
                    }
                }
            }
            s = s.TrimEnd(',');
            return s;
        }
        /// <summary>
        /// 支付付款单
        /// 1 只能支付初始状态的付款单
        /// 2 advance的付款单必须在po的waiting apportion 和waiting in stock 状态下支付
        /// 3 normal的付款单只能在po的instock状态下支付
        /// 4 获取应收款，应收款状态不可以是完全支付或者作废
        /// 5 根据金额的支付情况判断是全部，部分，还是未支付（可以支付负数）
        /// 6 更新应收款和付款单的状态
        /// </summary>
        /// <param name="payItemSysNo"></param>
        /// <param name="isForcePayMore"></param>
        /// <param name="userSysNo"></param>
        public void PayPOPayItem(int payItemSysNo, bool isForcePayMore, int userSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                POPayItemInfo oPayItem = LoadPayItem(payItemSysNo);
                if (oPayItem.Status != (int)AppEnum.POPayItemStatus.WaitingPay)
                    throw new BizException("pay bill is not WaitingPay status now, pay failed");

                int poStatus = PurchaseManager.GetInstance().getCurrentStatus(oPayItem.POSysNo);
                if (poStatus == (int)AppEnum.POStatus.WaitingApportion || poStatus == (int)AppEnum.POStatus.WaitingReceive || poStatus == (int)AppEnum.POStatus.WaitingInStock)
                {
                    if (oPayItem.PayStyle != (int)AppEnum.POPayItemStyle.Advanced)
                        throw new BizException("this po status allow only advanced pay");
                }
                else if (poStatus == (int)AppEnum.POStatus.InStock)
                {
                    if (oPayItem.PayStyle != (int)AppEnum.POPayItemStyle.Normal)
                        throw new BizException("this po status allow only normal pay");
                }
                else
                {
                    throw new BizException("this po status allow no pay");
                }

                POPayInfo oPay = LoadPay(oPayItem.POSysNo);
                if (oPay.PayStatus == (int)AppEnum.POPayStatus.FullPay || oPay.PayStatus == (int)AppEnum.POPayStatus.Abandon)
                {
                    throw new BizException("Full Pay already, need no pay bill");
                }

                //if (oPayItem.PayStyle == (int)AppEnum.POPayItemStyle.Normal && oPayItem.PayAmt > 0)  //正常正采购付款对帐期和增票做判断
                //{
                //    POInfo poInfo = PurchaseManager.GetInstance().LoadPO(oPayItem.POSysNo);
                //    if (poInfo != null && poInfo.InTime != null && poInfo.InTime >= DateTime.Parse("2008-09-01"))  //9.1之后入库的采购单进行控制
                //    {
                //        if (DateTime.Parse(poInfo.PayDate.ToString(AppConst.DateFormat) + " 00:00:01") >= DateTime.Now)
                //        {
                //            throw new BizException("帐期未到，不能付款");
                //        }
                //        if (poInfo.POInvoiceType == (int)AppEnum.POInvoiceType.ValueAddedInvoice)
                //        {
                //            if (oPay.InvoiceStatus != (int)AppEnum.POPayInvoiceStatus.Complete)
                //            {
                //                throw new BizException("增票未到，不能付款");
                //            }
                //        }
                //    }
                //}
                //用于强制控制财务是否能付款 - 针对RMA
                //if (oPay.POAmt > 0)
                //{
                //    POInfo po = PurchaseManager.GetInstance().LoadPO(oPayItem.POSysNo);
                //    Hashtable ht = new Hashtable();
                //    ht.Add("OutBoundTimeTo", DateTime.Now.AddMonths(-1).ToString(AppConst.DateFormat));
                //    ht.Add("VendorSysNo", po.VendorSysNo.ToString());
                //    DataSet dsOutBoundNoReturn = RMA.RMAOutBoundManager.GetInstance().GetOutBoundNoReturnDS(ht);
                //    if (Util.HasMoreRow(dsOutBoundNoReturn))
                //    {
                //        throw new BizException("存在一个月以上送修未返还商品，不能付款！");
                //    }
                //}

                //if (oPay.POAmt > 0)  //针对是否存在负采购未支付的
                //{
                //    POInfo po = PurchaseManager.GetInstance().LoadPO(oPayItem.POSysNo);
                //    if (IsExistsUnPayPO(po.VendorSysNo,-1))
                //    {
                //        throw new BizException("存在未支付的负采购，不能付款！");
                //    }
                //}

                //				if ( oPay.AlreadyPayAmt + oPayItem.PayAmt <= 0)
                //				{
                //					oPay.PayStatus = (int)AppEnum.POPayStatus.UnPay;
                //				}
                //				else if ( oPay.POAmt > oPay.AlreadyPayAmt + oPayItem.PayAmt)
                //				{
                //					oPay.PayStatus = (int)AppEnum.POPayStatus.PartlyPay;
                //				}
                //				else if ( oPay.POAmt == oPay.AlreadyPayAmt + oPayItem.PayAmt)
                //				{
                //					oPay.PayStatus = (int)AppEnum.POPayStatus.FullPay;
                //				}
                //				else if ( oPay.POAmt < oPay.AlreadyPayAmt + oPayItem.PayAmt )
                //				{
                //					if ( isForcePayMore)
                //						oPay.PayStatus = (int)AppEnum.POPayStatus.FullPay;
                //					else
                //						throw new BizException("please ensure 'force pay more' ");
                //				}
                //解决负采购时，POAmt为负数的问题，先判断POAmt 是否等于 已支付+本次支付 changed 20070413
                if (oPay.POAmt == oPay.AlreadyPayAmt + oPayItem.PayAmt)
                {
                    oPay.PayStatus = (int)AppEnum.POPayStatus.FullPay;
                }
                else if (oPay.AlreadyPayAmt + oPayItem.PayAmt <= 0)
                {
                    oPay.PayStatus = (int)AppEnum.POPayStatus.UnPay;
                }
                else if (oPay.POAmt > oPay.AlreadyPayAmt + oPayItem.PayAmt)
                {
                    oPay.PayStatus = (int)AppEnum.POPayStatus.PartlyPay;
                }
                else if (oPay.POAmt < oPay.AlreadyPayAmt + oPayItem.PayAmt)
                {
                    if (isForcePayMore)
                        oPay.PayStatus = (int)AppEnum.POPayStatus.FullPay;
                    else
                        throw new BizException("please ensure 'force pay more' ");
                }
                oPay.AlreadyPayAmt += oPayItem.PayAmt;

                Hashtable htPay = new Hashtable(3);
                htPay.Add("SysNo", oPay.SysNo);
                htPay.Add("AlreadyPayAmt", oPay.AlreadyPayAmt);
                htPay.Add("PayStatus", oPay.PayStatus);
                new POPayDac().UpdateMaster(htPay);

                Hashtable htPayItem = new Hashtable(2);
                htPayItem.Add("SysNo", payItemSysNo);
                htPayItem.Add("Status", (int)AppEnum.POPayItemStatus.Paid);
                htPayItem.Add("PayUserSysNo", userSysNo);
                htPayItem.Add("PayTime", DateTime.Now);
                if (oPayItem.VoucherTime == AppConst.DateTimeNull)
                {
                    htPayItem.Add("VoucherTime", DateTime.Now);
                }

                new POPayDac().UpdateItem(htPayItem);

                scope.Complete();
            }
        }
        /// <summary>
        /// 1付款单状态必须是已经支付
        /// 2waiting apportion 和 waiting instock状态下可以取消预付的付款单
        /// 3instock状态下可以取消正常的收款单；po不在上述3种状态的抛出异常。
        /// 4应收款的状态不可以是unpay和abandon
        /// 5更新应收款的状态为unpay或者partlypay
        /// 6更新付款单和应收款的状态
        /// </summary>
        /// <param name="payItemSysNo"></param>
        /// <param name="userSysNo"></param>
        public void CancelPayPOPayItem(int payItemSysNo, int userSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                POPayItemInfo oPayItem = LoadPayItem(payItemSysNo);
                if (oPayItem.Status != (int)AppEnum.POPayItemStatus.Paid)
                    throw new BizException("pay bill is not paid status now, cancel pay failed");

                int poStatus = PurchaseManager.GetInstance().getCurrentStatus(oPayItem.POSysNo);
                if (poStatus == (int)AppEnum.POStatus.WaitingApportion || poStatus == (int)AppEnum.POStatus.WaitingReceive || poStatus == (int)AppEnum.POStatus.WaitingInStock)
                {
                    if (oPayItem.PayStyle != (int)AppEnum.POPayItemStyle.Advanced)
                        throw new BizException("this po status allow only advanced pay");
                }
                else if (poStatus == (int)AppEnum.POStatus.InStock)
                {
                    if (oPayItem.PayStyle != (int)AppEnum.POPayItemStyle.Normal)
                        throw new BizException("this po status allow only normal pay");
                }
                else
                {
                    throw new BizException("this po status allow no pay");
                }

                POPayInfo oPay = LoadPay(oPayItem.POSysNo);
                if (oPay.PayStatus == (int)AppEnum.POPayStatus.UnPay || oPay.PayStatus == (int)AppEnum.POPayStatus.Abandon)
                {
                    throw new BizException("UnPay! Impossible, tell me");
                }

                if (oPay.AlreadyPayAmt - oPayItem.PayAmt == 0)
                {
                    oPay.PayStatus = (int)AppEnum.POPayStatus.UnPay;
                }
                else if (oPay.POAmt > oPay.AlreadyPayAmt - oPayItem.PayAmt)
                {
                    oPay.PayStatus = (int)AppEnum.POPayStatus.PartlyPay;
                }

                oPay.AlreadyPayAmt -= oPayItem.PayAmt;

                Hashtable htPay = new Hashtable(3);
                htPay.Add("SysNo", oPay.SysNo);
                htPay.Add("AlreadyPayAmt", oPay.AlreadyPayAmt);
                htPay.Add("PayStatus", oPay.PayStatus);
                new POPayDac().UpdateMaster(htPay);

                Hashtable htPayItem = new Hashtable(2);
                htPayItem.Add("SysNo", payItemSysNo);
                htPayItem.Add("Status", (int)AppEnum.POPayItemStatus.WaitingPay);
                htPayItem.Add("PayUserSysNo", userSysNo);
                htPayItem.Add("PayTime", DateTime.Now);

                new POPayDac().UpdateItem(htPayItem);

                scope.Complete();
            }
        }

        public void UpdatePOPay(int paySysNo, int invoiceStatus, string note, string invoiceTime)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                Hashtable ht = new Hashtable(5);
                ht.Add("SysNo", paySysNo);
                ht.Add("InvoiceStatus", invoiceStatus);
                ht.Add("Note", note);

                if (invoiceStatus == (int)AppEnum.POPayInvoiceStatus.Complete)
                    ht.Add("InvoiceTime", invoiceTime);
                else
                    ht.Add("InvoiceTime", AppConst.DateTimeNull);

                new POPayDac().UpdateMaster(ht);

                scope.Complete();
            }
        }

        /// <summary>
        /// 判断供应商是否存在未支付的采购单
        /// </summary>
        /// <param name="vendorSysNo">供应商sysno</param>
        /// <param name="poType">正负采购:1为正采购,-1为负采购 </param>
        /// <returns>true,false</returns>
        public bool IsExistsUnPayPO(int vendorSysNo, int poType)
        {
            string sql = @"select 
								pay.posysno
							from 
								finance_popay pay,
								po_master po,
                                Finance_POPay_Item fi  
							where 
								pay.posysno = po.sysno 
                            and po.sysno = fi.posysno 
                            and po.vendorsysno = @vendorsysno
                            and fi.status = @fistatus
                            and pay.paystatus = @paystatus";
            sql = sql.Replace("@fistatus", ((int)AppEnum.POPayItemStatus.Origin).ToString());
            sql = sql.Replace("@paystatus", ((int)AppEnum.POPayStatus.UnPay).ToString());
            sql = sql.Replace("@vendorsysno", vendorSysNo.ToString());
            if (poType > 0)
            {
                sql += " and pay.poamt > 0";
            }
            else if (poType < 0)
            {
                sql += " and pay.poamt < 0";
            }
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 判断供应商是否存在财务付款未审核的采购单
        /// </summary>
        /// <param name="vendorSysNo">供应商sysno</param>
        /// <param name="poType">正负采购:1为正采购,-1为负采购 </param>
        /// <returns>true,false</returns>
        public bool IsExistsUnAuditPO(int vendorSysNo, int poType, int poPayItemSysNo)
        {
            string sql = @"select 
								pay.posysno
							from 
								finance_popay pay,
								po_master po,
                                Finance_POPay_Item fi  
							where 
								pay.posysno = po.sysno 
                            and po.sysno = fi.posysno 
                            and po.vendorsysno = @vendorsysno
                            and fi.status in(@fistatus)
                            and pay.paystatus = @paystatus
                            and fi.sysno <> " + poPayItemSysNo + " ";
            sql = sql.Replace("@fistatus", ((int)AppEnum.POPayItemStatus.Origin).ToString() + "," + ((int)AppEnum.POPayItemStatus.WaitingAudit).ToString());
            sql = sql.Replace("@paystatus", ((int)AppEnum.POPayStatus.UnPay).ToString());
            sql = sql.Replace("@vendorsysno", vendorSysNo.ToString());
            if (poType > 0)
            {
                sql += " and pay.poamt > 0";
            }
            else if (poType < 0)
            {
                sql += " and pay.poamt < 0" + " and (po.MinusPOType IS NULL OR po.MinusPOType<>" + (int)AppEnum.MinusPOType.Rectify + ")";
            }
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 判断供应商是否存在财务付款未申请的负采购单(调价类型负采购当成正采购)
        /// </summary>
        /// <param name="vendorSysNo">供应商sysno</param>
        /// <param name="poType">正负采购:1为正采购,-1为负采购 </param>
        /// <returns>true,false</returns>
        public bool IsExistsUnRequestPO(int vendorSysNo, int poType, int poPayItemSysNo)
        {
            string sql = @"select 
								pay.posysno
							from 
								finance_popay pay,
								po_master po,
                                Finance_POPay_Item fi  
							where 
								pay.posysno = po.sysno 
                            and po.sysno = fi.posysno 
                            and po.vendorsysno = @vendorsysno
                            and fi.status = @fistatus
                            and pay.paystatus = @paystatus
                            and fi.sysno <> " + poPayItemSysNo + " ";
            sql = sql.Replace("@fistatus", ((int)AppEnum.POPayItemStatus.Origin).ToString());
            sql = sql.Replace("@paystatus", ((int)AppEnum.POPayStatus.UnPay).ToString());
            sql = sql.Replace("@vendorsysno", vendorSysNo.ToString());
            if (poType > 0)
            {
                sql += " and pay.poamt > 0";
            }
            else if (poType < 0)
            {
                sql += " and pay.poamt < 0" + " and (po.MinusPOType IS NULL OR po.MinusPOType<>" + (int)AppEnum.MinusPOType.Rectify + ")";
            }
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                return true;
            else
                return false;
        }

        public bool IsPayTotalAmtLessThanZero(int vendorSysNo)
        {
            string sql = @"
SELECT  ISNULL(SUM(dbo.Finance_POPay_Item.PayAmt),0) amt
FROM    dbo.Finance_POPay
        JOIN dbo.PO_Master ON dbo.PO_Master.SysNo = dbo.Finance_POPay.POSysNo
        JOIN dbo.Finance_POPay_Item ON dbo.Finance_POPay_Item.POSysNo = dbo.PO_Master.SysNo
WHERE   dbo.Finance_POPay.PayStatus = " + (int)AppEnum.POPayStatus.UnPay + @"
        AND dbo.Finance_POPay_Item.Status IN ( " + (int)AppEnum.POPayItemStatus.Origin + "," + (int)AppEnum.POPayItemStatus.WaitingAudit + "," + (int)AppEnum.POPayItemStatus.WaitingPay + @" )
        AND VendorSysNo = " + vendorSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                decimal totalamt = Util.TrimDecimalNull(ds.Tables[0].Rows[0]["amt"]);
                if (totalamt < 0)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public void Import()
        {
            if (!AppConfig.IsImportable)
                throw new BizException("Is Importable is false");

            string sql = " select top 1 sysno from finance_popay";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                throw new BizException("the table finance_popay is not empty");

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                #region 先插入应收款，因为老的记录里有多次入库，所以要合并。
                string sqlPOPay = @"select 
										ordersysno as posysno, foreignamt as poamt,
										foreignalreadypay as alreadypayamt,
										invoicestatus, a.status as paystatus, instocksysno,
										b.newsysno as currencysysno, '1' as sysno, '' as note
									from 
										ipp2003..finance_will_pay as a,
										ippconvert..currency as b
									where
										a.currencysysno = b.oldsysno
									and a.status <> -1
									order by ordersysno";

                DataSet dsPOPay = SqlHelper.ExecuteDataSet(sqlPOPay);
                if (!Util.HasMoreRow(dsPOPay))
                    return;


                POPayInfo oLastPOPay = new POPayInfo();

                foreach (DataRow drPOPay in dsPOPay.Tables[0].Rows)
                {
                    /* old paystatus
                     * -1 abandan
                     * 0 unpay
                     * 1 partly pay
                     * 2 pay
                     * 3 close
                     */
                    /*old invoice status
                     * 0 absent
                     * 1 incomplete
                     * 2 complete
                     */
                    POPayInfo oCurrentPOPay = new POPayInfo();
                    map(oCurrentPOPay, drPOPay);
                    switch (oCurrentPOPay.PayStatus)
                    {
                        case -1:
                            oCurrentPOPay.PayStatus = (int)AppEnum.POPayStatus.Abandon;
                            break;
                        case 0:
                            oCurrentPOPay.PayStatus = (int)AppEnum.POPayStatus.UnPay;
                            break;
                        case 1:
                            oCurrentPOPay.PayStatus = (int)AppEnum.POPayStatus.PartlyPay;
                            break;
                        case 2:
                        case 3:
                            oCurrentPOPay.PayStatus = (int)AppEnum.POPayStatus.FullPay;
                            break;
                        default:
                            break;
                    }
                    switch (oCurrentPOPay.InvoiceStatus)
                    {
                        case 0:
                            oCurrentPOPay.InvoiceStatus = (int)AppEnum.POPayInvoiceStatus.Absent;
                            break;
                        case 1:
                            oCurrentPOPay.InvoiceStatus = (int)AppEnum.POPayInvoiceStatus.Incomplete;
                            break;
                        case 2:
                            oCurrentPOPay.InvoiceStatus = (int)AppEnum.POPayInvoiceStatus.Complete;
                            break;
                        default:
                            break;
                    }

                    if (oLastPOPay.POSysNo == AppConst.IntNull)
                    {

                        oLastPOPay = oCurrentPOPay;
                    }
                    else if (oLastPOPay.POSysNo != oCurrentPOPay.POSysNo)
                    {
                        new POPayDac().InsertMaster(oLastPOPay);
                        oLastPOPay = oCurrentPOPay;
                    }
                    else
                    {
                        oLastPOPay.POAmt += oCurrentPOPay.POAmt;
                        oLastPOPay.AlreadyPayAmt += oCurrentPOPay.AlreadyPayAmt;
                        oLastPOPay.PayStatus = oCurrentPOPay.PayStatus;
                        oLastPOPay.InvoiceStatus = oCurrentPOPay.InvoiceStatus;
                    }
                }
                if (oLastPOPay != null)
                {
                    new POPayDac().InsertMaster(oLastPOPay);
                }
                #endregion

                #region 再插入收款单
                string sqlPOPayItem = @"select
											posysno, paytype, foreignamt,
											paytime, note, b.newsysno as accountsysno, a.status
										from
											ipp2003..purchase_order_pay as a,
											ippconvert..sys_user as b
										where a.accountsysno = b.oldsysno";
                DataSet dsPOPayItem = SqlHelper.ExecuteDataSet(sqlPOPayItem);
                if (!Util.HasMoreRow(dsPOPayItem))
                    throw new BizException("no pay item records in ipp2003 db");
                foreach (DataRow drPOPayItem in dsPOPayItem.Tables[0].Rows)
                {
                    /* old paytype 
                     * 0 normal
                     * 1 advanced 
                     * 不需要转换
                     */
                    POPayItemInfo oPOPayItem = new POPayItemInfo();
                    oPOPayItem.POSysNo = Util.TrimIntNull(drPOPayItem["posysno"]);
                    oPOPayItem.PayStyle = Util.TrimIntNull(drPOPayItem["paytype"]);
                    oPOPayItem.PayAmt = Util.TrimDecimalNull(drPOPayItem["foreignamt"]);
                    oPOPayItem.CreateUserSysNo = Util.TrimIntNull(drPOPayItem["accountsysno"]);
                    oPOPayItem.CreateTime = Util.TrimDateNull(drPOPayItem["paytime"]);
                    oPOPayItem.PayTime = Util.TrimDateNull(drPOPayItem["paytime"]);
                    oPOPayItem.PayUserSysNo = Util.TrimIntNull(drPOPayItem["accountsysno"]);
                    oPOPayItem.Status = Util.TrimIntNull(drPOPayItem["status"]);
                    switch (oPOPayItem.Status)
                    {
                        case 0:
                            oPOPayItem.Status = (int)AppEnum.POPayItemStatus.Paid;
                            break;
                        case -1:
                            oPOPayItem.Status = (int)AppEnum.POPayItemStatus.Abandon;
                            break;
                        default:
                            throw new BizException("unknown popayitem status");
                    }
                    new POPayDac().InsertItem(oPOPayItem);
                }
                #endregion


                scope.Complete();
            }
        }


        public void UpdatePOPayItem(Hashtable paramHash)
        {
            new POPayDac().UpdateItem(paramHash);
        }

        public void AuditPoPayItem(POPayItemInfo oInfo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                if (oInfo.Status != (int)AppEnum.POPayItemStatus.WaitingAudit)
                    throw new BizException("pay bill is not WaitingAudit status now, Audit failed");


                POPayInfo oPay = LoadPay(oInfo.POSysNo);
                //针对是否存在负采购未审核的
                if (oPay.POAmt > 0)
                {
                    POInfo po = PurchaseManager.GetInstance().LoadPO(oInfo.POSysNo);
                    if (IsExistsUnAuditPO(po.VendorSysNo, -1, oInfo.SysNo))
                    {
                        throw new BizException("存在未审核的负采购，不能付款！");
                    }
                }

                //POPayInfo oPay = LoadPay(oPayItem.POSysNo);

                //Hashtable htPay = new Hashtable(3);
                //htPay.Add("SysNo", oPay.SysNo);
                //htPay.Add("AlreadyPayAmt", oPay.AlreadyPayAmt);
                //htPay.Add("PayStatus", oPay.PayStatus);
                //new POPayDac().UpdateMaster(htPay);

                Hashtable htPayItem = new Hashtable(2);
                htPayItem.Add("SysNo", oInfo.SysNo);
                htPayItem.Add("Status", (int)AppEnum.POPayItemStatus.WaitingPay);
                htPayItem.Add("AuditUserSysNo", oInfo.AuditUserSysNo);
                htPayItem.Add("AuditTime", oInfo.AuditTime);
                new POPayDac().UpdateItem(htPayItem);

                scope.Complete();
            }
        }

        /// <summary>
        /// 审核付款单
        /// </summary>
        /// <param name="oInfo">付款单</param>
        /// <param name="isIgnoreErr">是否忽略异常检测</param>
        public void AuditPoPayItem(POPayItemInfo oInfo, bool isIgnoreErr, int auditUserSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                if (oInfo.Status != (int)AppEnum.POPayItemStatus.WaitingAudit)
                    throw new BizException("pay bill is not WaitingAudit status now, Audit failed");

                if (!isIgnoreErr)
                {
                    string errmsg = "";
                    if (IsPayTotalAmtLessThanZero(PurchaseManager.GetInstance().LoadPO(oInfo.POSysNo).VendorSysNo))
                    {
                        errmsg += "本次对当前供应商的支付总和小于0 <br />";
                    }
                    POInfo poInfo = PurchaseManager.GetInstance().LoadPO(oInfo.POSysNo);
                    if (poInfo != null)
                    {
                        if (IsExistsUnAuditPO(poInfo.VendorSysNo, -1, oInfo.SysNo))
                        {
                            errmsg += "存在对应供应商的未审核的负采购";
                        }
                    }
                    if (errmsg != "")
                        throw new BizException(errmsg);
                }


                Hashtable htPayItem = new Hashtable(2);
                htPayItem.Add("SysNo", oInfo.SysNo);
                htPayItem.Add("Status", (int)AppEnum.POPayItemStatus.WaitingPay);
                htPayItem.Add("AuditUserSysNo", auditUserSysNo);
                htPayItem.Add("AuditTime", DateTime.Now);
                new POPayDac().UpdateItem(htPayItem);
                UpdateDailyPayAmtRestrict(oInfo, "+");

                scope.Complete();
            }
        }

        /// <summary>
        /// 根据付款单支付金额更新每日已分配金额
        /// </summary>
        /// <param name="paramInfo">付款单</param>
        /// <param name="operation">操作符("+"或"-")</param>
        public void UpdateDailyPayAmtRestrict(POPayItemInfo paramInfo, string operation)
        {
            POInfo oPO = PurchaseManager.GetInstance().LoadPO(paramInfo.POSysNo);
            if (oPO != null)
            {
                DailyPayAmtRestrictInfo restrictInfo = POPayManager.GetInstance().LoadDailyPayAmtRestrict(AppConst.IntNull, DateTime.Now);
                if (oPO.POInvoiceType == (int)AppEnum.POInvoiceType.ValueAddedInvoice)
                {
                    if (operation == "+")
                        restrictInfo.AllocatedPublicAmt += paramInfo.PayAmt;
                    else if (operation == "-")
                        restrictInfo.AllocatedPublicAmt -= paramInfo.PayAmt;
                }
                else
                {
                    if (operation == "+")
                        restrictInfo.AllocatedPrivateAmt += paramInfo.PayAmt;
                    else if (operation == "-")
                        restrictInfo.AllocatedPrivateAmt -= paramInfo.PayAmt;
                }
                UpdateDailyPayAmtRestrict(restrictInfo);
            }
        }

        public void CancelAuditPoPayItem(POPayItemInfo oInfo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                if (oInfo.Status != (int)AppEnum.POPayItemStatus.WaitingPay)
                    throw new BizException("pay bill is not WaitingPay status now, CancelAudit failed");

                //POPayInfo oPay = LoadPay(oPayItem.POSysNo);

                //Hashtable htPay = new Hashtable(3);
                //htPay.Add("SysNo", oPay.SysNo);
                //htPay.Add("AlreadyPayAmt", oPay.AlreadyPayAmt);
                //htPay.Add("PayStatus", oPay.PayStatus);
                //new POPayDac().UpdateMaster(htPay);

                Hashtable htPayItem = new Hashtable(2);
                htPayItem.Add("SysNo", oInfo.SysNo);
                htPayItem.Add("Status", (int)AppEnum.POPayItemStatus.WaitingAudit);
                new POPayDac().UpdateItem(htPayItem);
                UpdateDailyPayAmtRestrict(oInfo, "-");
                scope.Complete();
            }
        }

        /// <summary>
        /// 审核没有通过驳回申请
        /// </summary>
        /// <param name="oInfo"></param>
        public void CancelRequestPoPayItem(POPayItemInfo oInfo, int userSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                if (oInfo.Status != (int)AppEnum.POPayItemStatus.WaitingAudit)
                    throw new BizException("pay bill is not WaitingAudit status now, CancelRequest failed");

                Hashtable htPayItem = new Hashtable(2);
                htPayItem.Add("SysNo", oInfo.SysNo);
                htPayItem.Add("Status", (int)AppEnum.POPayItemStatus.Origin);
                new POPayDac().UpdateItem(htPayItem);

                POPayItemErrRequestInfo errRequestInfo = POPayManager.GetInstance().LoadPOPayItemErrRequest(oInfo.SysNo, true);
                if (errRequestInfo != null)
                {
                    errRequestInfo.LastAuditUserSysNo = userSysNo;
                    errRequestInfo.LastAuditTime = DateTime.Now;
                    errRequestInfo.Status = (int)AppEnum.POPayItemErrRequestStatus.PMDReturn;
                    errRequestInfo.LastAuditNote = "审核没有通过，系统自动驳回";
                    ReturnErrRequest(errRequestInfo);
                }

                scope.Complete();
            }
        }

        /// <summary>
        /// 初略计算供应商的库存金额
        /// </summary>
        /// <param name="stocksysno"></param>
        /// <param name="vendorsysno"></param>
        /// <returns></returns>
        public decimal GetVendorInventoryAmt(int stocksysno, int vendorsysno)
        {
            string sql = @"
SELECT  ISNULL(SUM(dbo.Inventory_Stock.AccountQty * dbo.Product_Price.UnitCost),0) InventoryAmt
FROM    dbo.Product
        JOIN dbo.Inventory_Stock ON dbo.Product.SysNo = dbo.Inventory_Stock.ProductSysNo
        JOIN dbo.Product_Price ON dbo.Product.SysNo = dbo.Product_Price.ProductSysNo
WHERE   LastVendorSysNo = " + vendorsysno + @"
        AND dbo.Inventory_Stock.StockSysNo =" + stocksysno;

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            return Util.TrimDecimalNull(ds.Tables[0].Rows[0]["InventoryAmt"]);
        }

        /// <summary>
        /// 计算应该给该供应商的支付总额
        /// </summary>
        /// <returns></returns>
        public decimal GetVendorPayableAmt(int vendorsysno)
        {
            string sql = @"
SELECT  ISNULL(SUM(dbo.Finance_POPay_Item.PayAmt),0) amt
FROM    dbo.Finance_POPay
        JOIN dbo.PO_Master ON dbo.PO_Master.SysNo = dbo.Finance_POPay.POSysNo
        JOIN dbo.Finance_POPay_Item ON dbo.Finance_POPay_Item.POSysNo = dbo.PO_Master.SysNo
WHERE   dbo.Finance_POPay.PayStatus = " + (int)AppEnum.POPayStatus.UnPay + @"
        AND dbo.Finance_POPay_Item.Status IN ( " + (int)AppEnum.POPayItemStatus.Origin + "," + (int)AppEnum.POPayItemStatus.WaitingAudit + "," + (int)AppEnum.POPayItemStatus.WaitingPay + @" )
        AND VendorSysNo = " + vendorsysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            return Util.TrimDecimalNull(ds.Tables[0].Rows[0]["amt"]);
        }

        /// <summary>
        /// 付款单是否是该供应商的最后一笔付款
        /// </summary>
        /// <param name="oPay"></param>
        /// <param name="po"></param>
        /// <returns></returns>
        public bool IsLastPayForVendor(POPayInfo oPay, POInfo po)
        {
            bool result = false;
            if (oPay != null)
            {
                if (po == null)
                    po = PurchaseManager.GetInstance().LoadPO(oPay.POSysNo);
                if (po != null)
                {
                    string sql = @"
SELECT  dbo.PO_Master.VendorSysNo,
        Finance_POPay_Item.*
FROM    dbo.Finance_POPay_Item
        JOIN dbo.PO_Master ON dbo.PO_Master.SysNo = dbo.Finance_POPay_Item.POSysNo
WHERE   dbo.PO_Master.VendorSysNo = " + po.VendorSysNo + @"
        AND dbo.Finance_POPay_Item.Status IN ( " + (int)AppEnum.POPayItemStatus.Origin + "," + (int)AppEnum.POPayItemStatus.WaitingAudit + "," + (int)AppEnum.POPayItemStatus.WaitingPay + @")
        AND dbo.PO_Master.Status <>" + (int)AppEnum.POStatus.Abandon;

                    DataSet ds = SqlHelper.ExecuteDataSet(sql);
                    if (Util.HasMoreRow(ds))
                    {
                        if (ds.Tables[0].Rows.Count > 1)
                            result = false;
                        else
                            return true;
                    }
                    else
                        throw new BizException("数据不存在，请确认支付信息是否正确");
                }
                else
                {
                    throw new BizException("数据不存在，请确认支付信息是否正确");
                }
            }

            return result;
        }

        #region PO付款单异常申请
        private void map(POPayItemErrRequestInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.POPayItemSysNo = Util.TrimIntNull(tempdr["POPayItemSysNo"]);
            oParam.RequestUserSysNo = Util.TrimIntNull(tempdr["RequestUserSysNo"]);
            oParam.RequestTime = Util.TrimDateNull(tempdr["RequestTime"]);
            oParam.RequestUserNote = Util.TrimNull(tempdr["RequestUserNote"]);
            oParam.ErrMsgNote = Util.TrimNull(tempdr["ErrMsgNote"]);
            oParam.TLAuditUserSysNo = Util.TrimIntNull(tempdr["TLAuditUserSysNo"]);
            oParam.TLAuditTime = Util.TrimDateNull(tempdr["TLAuditTime"]);
            oParam.TLNote = Util.TrimNull(tempdr["TLNote"]);
            oParam.LastAuditUserSysNo = Util.TrimIntNull(tempdr["LastAuditUserSysNo"]);
            oParam.LastAuditTime = Util.TrimDateNull(tempdr["LastAuditTime"]);
            oParam.LastAuditNote = Util.TrimNull(tempdr["LastAuditNote"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
        }

        /// <summary>
        /// 根据系统编号加载PO付款单异常申请
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public POPayItemErrRequestInfo LoadPOPayItemErrRequest(int SysNo)
        {
            string sql = @"
SELECT  *
FROM    dbo.Fin_POPay_Item_ErrRequest Where SysNo =" + SysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;
            else
            {
                POPayItemErrRequestInfo newInfo = new POPayItemErrRequestInfo();
                map(newInfo, ds.Tables[0].Rows[0]);
                return newInfo;
            }
        }

        /// <summary>
        /// 根据付款单号加载PO付款单异常申请      
        /// </summary>
        /// <param name="POPayItemSysNo">付款单编号</param>
        /// <param name="IsOnlyLoadValid">是否只加载有效记录</param>
        /// <returns></returns>
        public POPayItemErrRequestInfo LoadPOPayItemErrRequest(int POPayItemSysNo, bool IsOnlyLoadValid)
        {
            string sql = @"
SELECT  *
FROM    dbo.Fin_POPay_Item_ErrRequest Where POPayItemSysNo =" + POPayItemSysNo;
            if (IsOnlyLoadValid)
                sql += " AND Status >" + (int)AppEnum.POPayItemErrRequestStatus.PMCancelRequest;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;
            else
            {
                POPayItemErrRequestInfo newInfo = new POPayItemErrRequestInfo();
                map(newInfo, ds.Tables[0].Rows[0]);
                return newInfo;
            }
        }

        /// <summary>
        /// 付款单异常申请查询
        /// </summary>
        /// <param name="paramHash"></param>
        /// <returns></returns>
        public DataSet GetPOPayItemErrRequest(Hashtable paramHash)
        {
            string sql = @"
SELECT  dbo.Fin_POPay_Item_ErrRequest.*,
        RequestUser.UserName RequestUserName,
        TLAuditUser.UserName TLAuditUserName,
        LastAuditUser.UserName LastAuditUserName
FROM    dbo.Fin_POPay_Item_ErrRequest 
        LEFT JOIN dbo.Sys_User RequestUser ON RequestUser.SysNo = dbo.Fin_POPay_Item_ErrRequest.RequestUserSysNo
        LEFT JOIN dbo.Sys_User TLAuditUser ON TLAuditUser.SysNo = dbo.Fin_POPay_Item_ErrRequest.TLAuditUserSysNo
        LEFT JOIN dbo.Sys_User LastAuditUser ON LastAuditUser.SysNo = dbo.Fin_POPay_Item_ErrRequest.LastAuditUserSysNo
";
            if (paramHash != null && paramHash.Count > 0)
            {
                StringBuilder sb = new StringBuilder(200);
                sb.Append("WHERE 1=1 ");
                foreach (string key in paramHash.Keys)
                {
                    sb.Append(" AND ");
                    object item = paramHash[key];
                    if (key == "SysNo")
                    {
                        sb.Append(" dbo.Fin_POPay_Item_ErrRequest.SysNo ").Append(" = ").Append(item.ToString());
                    }
                    else if (key == "POPayItemSysNo")
                    {
                        sb.Append(" dbo.Fin_POPay_Item_ErrRequest.POPayItemSysNo ").Append(" = ").Append(item.ToString());
                    }
                    else if (key == "RequestUserSysNo")
                    {
                        sb.Append(" dbo.Fin_POPay_Item_ErrRequest.RequestUserSysNo ").Append(" = ").Append(item.ToString());
                    }
                    else if (key == "TLAuditUserSysNo")
                    {
                        sb.Append(" dbo.Fin_POPay_Item_ErrRequest.TLAuditUserSysNo ").Append(" = ").Append(item.ToString());
                    }
                    else if (key == "LastAuditUserSysNo")
                    {
                        sb.Append(" dbo.Fin_POPay_Item_ErrRequest.LastAuditUserSysNo ").Append(" = ").Append(item.ToString());
                    }
                    else if (item is int)
                    {
                        sb.Append(" dbo.Fin_POPay_Item_ErrRequest." + key).Append(" = ").Append(item.ToString());
                    }
                    else if (item is string)
                    {
                        sb.Append(" dbo.Fin_POPay_Item_ErrRequest." + key).Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                    }
                }
                sql += sb.ToString();
            }

            return SqlHelper.ExecuteDataSet(sql);
        }

        /// <summary>
        /// 付款申请提交
        /// </summary>
        /// <param name="paramHash">更新POPayItem的参数</param>
        /// <param name="errRequest">异常申请记录</param>
        public void RequestPay(Hashtable paramHash, POPayItemErrRequestInfo errRequest)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                if (paramHash != null && paramHash.Count > 0)
                    UpdatePOPayItem(paramHash);
                else
                    throw new BizException("缺少参数");

                if (errRequest != null)
                    new POPayDac().UpdatePOPayItemErrRequest(errRequest);

                scope.Complete();
            }
        }

        /// <summary>
        /// PM异常付款单申请记录
        /// </summary>
        /// <param name="errRequestInfo"></param>
        public void RequestErrPayItem(POPayItemErrRequestInfo errRequestInfo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                string sql = @"
SELECT  TOP 1 *
FROM    dbo.Fin_POPay_Item_ErrRequest
WHERE   POPayItemSysNo = " + errRequestInfo.POPayItemSysNo + @"
        AND Status>" + (int)AppEnum.POPayItemErrRequestStatus.PMCancelRequest;

                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (Util.HasMoreRow(ds))
                {
                    throw new BizException("不能重复提交异常申请");
                }
                //else
                //{
                //    #region 如果是负采购，则判断：负采购金额不能大于正采购金额的和
                //    POPayItemInfo oPayItem = LoadPayItem(errRequestInfo.POPayItemSysNo);
                //    POPayInfo oPay = LoadPay(oPayItem.POSysNo);
                //    if (oPay.POAmt < 0)
                //    {
                //        POInfo poInfo = PurchaseManager.GetInstance().LoadPO(oPayItem.POSysNo);
                //        if (poInfo != null)
                //        {
                //            if (IsPayTotalAmtLessThanZero(poInfo.VendorSysNo))
                //            {
                //                throw new BizException("负采购金额不能大于正采购金额的和，不能付款");
                //            }
                //        }
                //    }
                //#endregion

                if (1 != new POPayDac().InsertPOPayItemErrRequest(errRequestInfo))
                {
                    throw new BizException("提交异常申请失败");
                }

                scope.Complete();
            }
        }

        /// <summary>
        /// PM取消异常付款单申请
        /// </summary>
        /// <param name="SysNo"></param>
        public void CancelErrRequest(int SysNo)
        {
            string sql = @"
SELECT  *
FROM    dbo.Fin_POPay_Item_ErrRequest
WHERE   SysNo = " + SysNo + @"
        AND Status >" + (int)AppEnum.POPayItemErrRequestStatus.PMCancelRequest;

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
            {
                throw new BizException("没有有效的异常申请记录");
            }
            else
            {
                Hashtable hs = new Hashtable(2);
                hs.Add("SysNo", SysNo);
                hs.Add("Status", (int)AppEnum.POPayItemErrRequestStatus.PMCancelRequest);

                if (1 != new POPayDac().UpdatePOPayItemErrRequest(hs))
                {
                    throw new BizException("取消异常申请失败");
                }
            }
        }

        /// <summary>
        /// 驳回异常付款单申请
        /// </summary>
        /// <param name="errRequest"></param>
        public void ReturnErrRequest(POPayItemErrRequestInfo errRequest)
        {
            if (1 != new POPayDac().UpdatePOPayItemErrRequest(errRequest))
                throw new BizException("驳回操作失败");
        }

        /// <summary>
        /// TL提交申请审核付款单
        /// </summary>
        /// <param name="POPayItemSysNo">付款单编号</param>
        /// <param name="TLUserSysNo">TL提交申请人</param>
        /// <param name="Note">TL备注</param>
        public void RequestErrPayItemByTL(int POPayItemSysNo, int TLUserSysNo, string Note)
        {
            POPayItemErrRequestInfo errRequestInfo = LoadPOPayItemErrRequest(POPayItemSysNo, true);
            if (errRequestInfo == null)
                throw new BizException("付款单申请记录不存在");
            if (errRequestInfo.Status != (int)AppEnum.POPayItemErrRequestStatus.PMRequest)
                throw new BizException("当前付款单申请记录状态不是“PM申请状态”不能提交到PMD审核");
            //if (errRequestInfo.TLAuditUserSysNo != TLUserSysNo && TLUserSysNo != AppConst.PMDUserSysNo)
            if (errRequestInfo.TLAuditUserSysNo != TLUserSysNo && TLUserSysNo != PurchaseManager.GetInstance().GetPMDUserSysNo())
                throw new BizException("您无权进行TL异常申请");


            errRequestInfo.Status = (int)AppEnum.POPayItemErrRequestStatus.TLRequst;
            errRequestInfo.TLAuditTime = DateTime.Now;
            errRequestInfo.TLNote = Note;
            //errRequestInfo.LastAuditUserSysNo = AppConst.PMDUserSysNo;
            errRequestInfo.LastAuditUserSysNo = PurchaseManager.GetInstance().GetPMDUserSysNo();

            if (1 != new POPayDac().UpdatePOPayItemErrRequest(errRequestInfo))
                throw new BizException("TL提交异常申请失败");
        }


        public DataSet GetPOPayItemReportByWaitPay(Hashtable paramHash)
        {
            string sql = @"
SELECT  dbo.Finance_POPay_Item.SysNo POPayItemSysNo,
        dbo.PO_Master.POID,
        PMUser.UserName PMName,
        RequestUser.UserName RequestName,
        dbo.Vendor.SysNo VendorSysNo,
        dbo.Vendor.VendorName,
        dbo.Finance_POPay_Item.PayAmt,
        dbo.PO_Master.POInvoiceType,
        dbo.Fin_POPay_Item_ErrRequest.ErrMsgNote
FROM    dbo.Finance_POPay_Item
        JOIN dbo.PO_Master ON dbo.PO_Master.SysNo = dbo.Finance_POPay_Item.POSysNo
        JOIN dbo.Vendor ON dbo.Vendor.SysNo = dbo.PO_Master.VendorSysNo
        JOIN dbo.Sys_User PMUser ON PMUser.SysNo = dbo.PO_Master.CreateUserSysNo
        JOIN dbo.Sys_User RequestUser ON RequestUser.SysNo = dbo.Finance_POPay_Item.RequestUserSysNo
        LEFT JOIN dbo.Fin_POPay_Item_ErrRequest ON dbo.Fin_POPay_Item_ErrRequest.POPayItemSysNo =dbo.Finance_POPay_Item.SysNo AND dbo.Fin_POPay_Item_ErrRequest.Status>" + (int)AppEnum.POPayItemErrRequestStatus.TLRequst + @"
";
            if (paramHash != null && paramHash.Count > 0)
            {
                StringBuilder sb = new StringBuilder(100);
                sb.Append(" WHERE   1=1 ");
                foreach (string key in paramHash.Keys)
                {
                    sb.Append(" AND ");
                    object item = paramHash[key];
                    if (key == "PMSysNo")
                    {
                        sb.Append(" dbo.PO_Master.CreateUserSysNo ").Append(" = ").Append(item.ToString());
                    }
                    else if (key == "Status")
                    {
                        sb.Append(" dbo.Finance_POPay_Item.Status ").Append(" = ").Append(item.ToString());
                    }
                    else if (key == "POInvoiceType")
                    {
                        sb.Append(" dbo.PO_Master.POInvoiceType = ").Append(item.ToString());
                    }
                    else if (key == "RequestTimeFrom")
                    {
                        sb.Append(" dbo.Finance_POPay_Item.RequestTime ").Append(" >= ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "RequestTimeTo")
                    {
                        sb.Append(" dbo.Finance_POPay_Item.RequestTime ").Append(" <= ").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "AuditTimeFrom")
                    {
                        sb.Append(" dbo.Finance_POPay_Item.AuditTime ").Append(" >= ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "AuditTimeTo")
                    {
                        sb.Append(" dbo.Finance_POPay_Item.AuditTime ").Append(" <= ").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "DateFromPay")
                    {
                        sb.Append(" dbo.Finance_POPay_Item.PayTime ").Append(" >= ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "DateToPay")
                    {
                        sb.Append(" dbo.Finance_POPay_Item.PayTime ").Append(" <= ").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "SysNoINStr")
                    {
                        sb.Append(" dbo.Finance_POPay_Item.SysNo IN ").Append(item.ToString());
                    }
                    else
                    {
                        sb.Append(" 1=1 ");
                    }
                }
                sql += sb.ToString() + " Order By dbo.Finance_POPay_Item.PayStyle desc,dbo.PO_Master.VendorSysNo,dbo.Finance_POPay_Item.SysNo,dbo.Finance_POPay_Item.PayAmt desc";
            }
            else
                throw new BizException("缺少参数，查询失败");

            return SqlHelper.ExecuteDataSet(sql);
        }

        #endregion


        #region 每日财务支付金额总额限制
        private void map(DailyPayAmtRestrictInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.TopPublicPayAmt = Util.TrimDecimalNull(tempdr["TopPublicPayAmt"]);
            oParam.AllocatedPublicAmt = Util.TrimDecimalNull(tempdr["AllocatedPublicAmt"]);
            oParam.TopPrivatePayAmt = Util.TrimDecimalNull(tempdr["TopPrivatePayAmt"]);
            oParam.AllocatedPrivateAmt = Util.TrimDecimalNull(tempdr["AllocatedPrivateAmt"]);
            oParam.CreateUserSysNo = Util.TrimIntNull(tempdr["CreateUserSysNo"]);
            oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
        }

        /// <summary>
        /// 加载每日支付金额限制 至少需要一个参数
        /// </summary>
        /// <param name="SysNo">不知道参数值可以传AppConst.IntNull</param>
        /// <param name="date">不知道参数值可以传AppConst.DateTimeNull</param>
        /// <returns></returns>
        public DailyPayAmtRestrictInfo LoadDailyPayAmtRestrict(int SysNo, DateTime date)
        {
            string sql = "SELECT * FROM dbo.Fin_DailyPayAmtRestrict WHERE 1=1 ";
            if (SysNo != AppConst.IntNull)
                sql += " AND SysNo =" + SysNo;
            if (date != AppConst.DateTimeNull)
            {
                sql += " AND CreateTime >=" + Util.ToSqlString(Util.TrimDateNull(date.ToString(AppConst.DateFormat)).ToString(AppConst.DateFormatLong));
                sql += " AND CreateTime <=" + Util.ToSqlEndDate(date.ToString(AppConst.DateFormat));
            }
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;
            else
            {
                DailyPayAmtRestrictInfo oInfo = new DailyPayAmtRestrictInfo();
                map(oInfo, ds.Tables[0].Rows[0]);
                return oInfo;
            }
        }

        public DataSet GetDailyPayAmtRestrictDs(Hashtable paramHash)
        {
            string sql = @"
SELECT  dbo.Fin_DailyPayAmtRestrict.*,
        dbo.Sys_User.UserName
FROM    dbo.Fin_DailyPayAmtRestrict
        LEFT JOIN dbo.Sys_User ON dbo.Sys_User.SysNo = dbo.Fin_DailyPayAmtRestrict.CreateUserSysNo
";
            if (paramHash != null && paramHash.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" WHERE 1=1 ");
                foreach (string key in paramHash.Keys)
                {
                    sb.Append(" AND ");
                    object item = paramHash[key];

                    if (key == "SysNo")
                    {
                        sb.Append(" dbo.Fin_DailyPayAmtRestrict.SysNo ").Append(" = ").Append(item.ToString());
                    }
                    else if (key == "CreateUserSysNo")
                    {
                        sb.Append(" dbo.Fin_DailyPayAmtRestrict.CreateUserSysNo ").Append(" = ").Append(item.ToString());
                    }
                    else if (key == "CreateTimeFrom")
                    {
                        sb.Append(" dbo.Fin_DailyPayAmtRestrict.CreateTime ").Append(" >= ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "CreateTimeTo")
                    {
                        sb.Append(" dbo.Fin_DailyPayAmtRestrict.CreateTime ").Append(" <= ").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else
                        sb.Append(" 1=1 ");
                }
                sql += sb.ToString();
            }
            sql += " ORDER BY dbo.Fin_DailyPayAmtRestrict.SysNo DESC";

            return SqlHelper.ExecuteDataSet(sql);
        }

        public void InsertDailyPayAmtRestrict(DailyPayAmtRestrictInfo oParam)
        {
            string sql = "SELECT * FROM dbo.Fin_DailyPayAmtRestrict";
            sql += " WHERE CreateTime >= " + Util.ToSqlString(oParam.CreateTime.ToString(AppConst.DateFormat));
            sql += " AND CreateTime <= " + Util.ToSqlEndDate(oParam.CreateTime.ToString(AppConst.DateFormat));
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                throw new BizException("已经存在今日的支付金额限制，不需要再添加");
            }
            else if (1 != new DailyPayAmtRestrictDac().Insert(oParam))
            {
                throw new BizException("插入失败");
            }
        }

        public void UpdateDailyPayAmtRestrict(DailyPayAmtRestrictInfo oParam)
        {
            if (1 != new DailyPayAmtRestrictDac().Update(oParam))
            {
                throw new BizException("更新失败");
            }
        }
        #endregion
    }
}