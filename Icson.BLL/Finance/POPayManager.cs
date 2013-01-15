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
        /// ��ȡӦ������б�
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
        /// ���븶�
        /// 1 waiting apportion �� waiting instock ��poֻ������Ԥ���ĸ��
        /// 2 InStock��poֻ�ܼ�������֧���ĸ��
        /// 3 ���������ĸ���������г�ʼ״̬��Ԥ�����
        /// 4 �Ƿ���Ӧ���a���û�У�����֮��b��������ϵ�Ӧ�������Ϊunpay״̬��c���Ӧ�����Ѿ���ȫ֧�������ܼ��븶��ˡ�
        /// 5 �����븶�
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
        /// �����޸ĳ�ʼ״̬�ĸ��
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
        /// 1ֻ�����ϳ�ʼ״̬�ĸ��
        /// 3���ϸ��
        /// 2������û����Ч�ĸ���ˣ�������Ӧ�տ�
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

                //����ø�������쳣�����¼����ȡ�����쳣�����¼
                POPayItemErrRequestInfo errRequestInfo = LoadPOPayItemErrRequest(payItemSysNo, true);
                if (errRequestInfo != null)
                    CancelErrRequest(errRequestInfo.SysNo);

                //���û����Ч�ĸ���� ������Ӧ���� 
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
        /// 1 ֻ��ȡ�������Ѿ����ϵĸ��
        /// 2 ���po�ĵ�ǰ״̬��waiting apportion, waiting in stock; ֻ�ָܻ�advanced�ĸ��
        /// 3 ���po�ĵ�ǰ״̬��instock, ֻ�ָܻ�normal�ĸ��
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
        /// ��PO������,��Ʊ�Ƿ�,�Ƿ���ڸ��ɹ�,�Ƿ���ڳ��ڷ��޼�,֧������Ƿ���ڿ����,�Ƿ������һ�ʸ�������ж�
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
                ErrorMsg += "�Ѿ�ȫ��֧����������֧�� " + " <br />";
            }

            //�������ɹ���������ں���Ʊ���ж�
            POInfo poInfo = null;
            if (oPayItem.PayStyle == (int)AppEnum.POPayItemStyle.Advanced)
            {
                ErrorMsg += "����һ��Ԥ���� " + " <br />";
            }

            if (oPayItem.PayStyle == (int)AppEnum.POPayItemStyle.Normal)
            {
                poInfo = PurchaseManager.GetInstance().LoadPO(oPayItem.POSysNo);
                if (poInfo != null && poInfo.InTime != null)
                {
                    if (DateTime.Parse(poInfo.PayDate.AddDays(-1).ToString(AppConst.DateFormat) + " 00:00:01") >= DateTime.Now) //��ǰһ�������ӡ����
                    {
                        //throw new BizException("����δ�������ܸ���");
                        ErrorMsg += "����δ�� " + " <br />";
                    }
                    if (poInfo.POInvoiceType == (int)AppEnum.POInvoiceType.ValueAddedInvoice)
                    {
                        if (oPay.InvoiceStatus != (int)AppEnum.POPayInvoiceStatus.Complete)
                        {
                            //throw new BizException("��Ʊδ�������ܸ���");
                            ErrorMsg += "��Ʊδ�� " + " <br />";
                        }
                    }
                }
            }


            //����ǿ�ƿ��Ʋ����Ƿ��ܸ��� - ���RMA
            //��δ��˵ĸ��ɹ��ģ����ܸ���
            //�����ڷ��޼��ģ����ܸ���
            //֧�������ڿ����Ĳ��ܸ���

            //POInfo po = PurchaseManager.GetInstance().LoadPO(oPayItem.POSysNo);
            if (poInfo == null)
                poInfo = PurchaseManager.GetInstance().LoadPO(oPayItem.POSysNo);
            if (poInfo != null && poInfo.InTime != null)
            {
                //�жϸ�PO�Ƿ���ڸ��ɹ������ڵĻ����ܸ���
                if (IsExistsUnRequestPO(poInfo.VendorSysNo, -1, oPayItem.SysNo))
                {
                    //throw new BizException("����δ��˵ĸ��ɹ������ܸ��");
                    ErrorMsg += "���ڶ�Ӧ��Ӧ�̵�δ����ĸ��ɹ� <br />";
                }
                Hashtable ht = new Hashtable();
                ht.Add("OutBoundTimeTo", DateTime.Now.AddMonths(-1).ToString(AppConst.DateFormat));
                ht.Add("VendorSysNo", poInfo.VendorSysNo.ToString());
                DataSet dsOutBoundNoReturn = RMA.RMAOutBoundManager.GetInstance().GetOutBoundNoReturnDS(ht);

                //���ڷ��޼��ģ����ܸ���
                if (Util.HasMoreRow(dsOutBoundNoReturn))
                {
                    //throw new BizException("����һ������������δ������Ʒ�����ܸ��");
                    ErrorMsg += "����һ������������δ������Ʒ " + " <br />";
                }

                //֧�������ڿ����Ĳ��ܸ���
                if (GetVendorPayableAmt(poInfo.VendorSysNo) - oPay.POAmt < GetVendorInventoryAmt(AppConfig.DefaultStockSysNo, poInfo.VendorSysNo))
                {
                    ErrorMsg += "�Թ�Ӧ�̵�Ӧ���ܶ�-��ǰӦ����С�ڿ���� " + " <br />";
                }
            }

            //else if (oPay.POAmt < 0) //���ɹ���֧������Ӧ�̽���ܺͲ���С��0
            //{
            //    if (poInfo == null)
            //        poInfo = PurchaseManager.GetInstance().LoadPO(oPayItem.POSysNo);
            //    if (poInfo != null)
            //    {
            //        if (IsPayTotalAmtLessThanZero(poInfo.VendorSysNo))
            //        {
            //            ErrorMsg += "���ɹ����ܴ������ɹ����ĺ� " + " <br />";
            //        }
            //    }
            //}

            //�Ƿ������һ�ʸ���
            if (IsLastPayForVendor(oPay, poInfo))
            {
                ErrorMsg += "�������һ�ʸ���" + " <br />";
            }

            return ErrorMsg;
        }

        /// <summary>
        /// ���������ˣ��Ƿ�Ӧ��Ӧ�����ܺ�С��0
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
        /// ���������ˣ��Ƿ�����쳣��
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
        /// ���������ˣ��Ƿ����δ��˵ĸ��ɹ�
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
        /// ֧�����
        /// 1 ֻ��֧����ʼ״̬�ĸ��
        /// 2 advance�ĸ��������po��waiting apportion ��waiting in stock ״̬��֧��
        /// 3 normal�ĸ��ֻ����po��instock״̬��֧��
        /// 4 ��ȡӦ�տӦ�տ�״̬����������ȫ֧����������
        /// 5 ���ݽ���֧������ж���ȫ�������֣�����δ֧��������֧��������
        /// 6 ����Ӧ�տ�͸����״̬
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

                //if (oPayItem.PayStyle == (int)AppEnum.POPayItemStyle.Normal && oPayItem.PayAmt > 0)  //�������ɹ���������ں���Ʊ���ж�
                //{
                //    POInfo poInfo = PurchaseManager.GetInstance().LoadPO(oPayItem.POSysNo);
                //    if (poInfo != null && poInfo.InTime != null && poInfo.InTime >= DateTime.Parse("2008-09-01"))  //9.1֮�����Ĳɹ������п���
                //    {
                //        if (DateTime.Parse(poInfo.PayDate.ToString(AppConst.DateFormat) + " 00:00:01") >= DateTime.Now)
                //        {
                //            throw new BizException("����δ�������ܸ���");
                //        }
                //        if (poInfo.POInvoiceType == (int)AppEnum.POInvoiceType.ValueAddedInvoice)
                //        {
                //            if (oPay.InvoiceStatus != (int)AppEnum.POPayInvoiceStatus.Complete)
                //            {
                //                throw new BizException("��Ʊδ�������ܸ���");
                //            }
                //        }
                //    }
                //}
                //����ǿ�ƿ��Ʋ����Ƿ��ܸ��� - ���RMA
                //if (oPay.POAmt > 0)
                //{
                //    POInfo po = PurchaseManager.GetInstance().LoadPO(oPayItem.POSysNo);
                //    Hashtable ht = new Hashtable();
                //    ht.Add("OutBoundTimeTo", DateTime.Now.AddMonths(-1).ToString(AppConst.DateFormat));
                //    ht.Add("VendorSysNo", po.VendorSysNo.ToString());
                //    DataSet dsOutBoundNoReturn = RMA.RMAOutBoundManager.GetInstance().GetOutBoundNoReturnDS(ht);
                //    if (Util.HasMoreRow(dsOutBoundNoReturn))
                //    {
                //        throw new BizException("����һ������������δ������Ʒ�����ܸ��");
                //    }
                //}

                //if (oPay.POAmt > 0)  //����Ƿ���ڸ��ɹ�δ֧����
                //{
                //    POInfo po = PurchaseManager.GetInstance().LoadPO(oPayItem.POSysNo);
                //    if (IsExistsUnPayPO(po.VendorSysNo,-1))
                //    {
                //        throw new BizException("����δ֧���ĸ��ɹ������ܸ��");
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
                //������ɹ�ʱ��POAmtΪ���������⣬���ж�POAmt �Ƿ���� ��֧��+����֧�� changed 20070413
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
        /// 1���״̬�������Ѿ�֧��
        /// 2waiting apportion �� waiting instock״̬�¿���ȡ��Ԥ���ĸ��
        /// 3instock״̬�¿���ȡ���������տ��po��������3��״̬���׳��쳣��
        /// 4Ӧ�տ��״̬��������unpay��abandon
        /// 5����Ӧ�տ��״̬Ϊunpay����partlypay
        /// 6���¸����Ӧ�տ��״̬
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
        /// �жϹ�Ӧ���Ƿ����δ֧���Ĳɹ���
        /// </summary>
        /// <param name="vendorSysNo">��Ӧ��sysno</param>
        /// <param name="poType">�����ɹ�:1Ϊ���ɹ�,-1Ϊ���ɹ� </param>
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
        /// �жϹ�Ӧ���Ƿ���ڲ��񸶿�δ��˵Ĳɹ���
        /// </summary>
        /// <param name="vendorSysNo">��Ӧ��sysno</param>
        /// <param name="poType">�����ɹ�:1Ϊ���ɹ�,-1Ϊ���ɹ� </param>
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
        /// �жϹ�Ӧ���Ƿ���ڲ��񸶿�δ����ĸ��ɹ���(�������͸��ɹ��������ɹ�)
        /// </summary>
        /// <param name="vendorSysNo">��Ӧ��sysno</param>
        /// <param name="poType">�����ɹ�:1Ϊ���ɹ�,-1Ϊ���ɹ� </param>
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

                #region �Ȳ���Ӧ�տ��Ϊ�ϵļ�¼���ж����⣬����Ҫ�ϲ���
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

                #region �ٲ����տ
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
                     * ����Ҫת��
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
                //����Ƿ���ڸ��ɹ�δ��˵�
                if (oPay.POAmt > 0)
                {
                    POInfo po = PurchaseManager.GetInstance().LoadPO(oInfo.POSysNo);
                    if (IsExistsUnAuditPO(po.VendorSysNo, -1, oInfo.SysNo))
                    {
                        throw new BizException("����δ��˵ĸ��ɹ������ܸ��");
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
        /// ��˸��
        /// </summary>
        /// <param name="oInfo">���</param>
        /// <param name="isIgnoreErr">�Ƿ�����쳣���</param>
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
                        errmsg += "���ζԵ�ǰ��Ӧ�̵�֧���ܺ�С��0 <br />";
                    }
                    POInfo poInfo = PurchaseManager.GetInstance().LoadPO(oInfo.POSysNo);
                    if (poInfo != null)
                    {
                        if (IsExistsUnAuditPO(poInfo.VendorSysNo, -1, oInfo.SysNo))
                        {
                            errmsg += "���ڶ�Ӧ��Ӧ�̵�δ��˵ĸ��ɹ�";
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
        /// ���ݸ��֧��������ÿ���ѷ�����
        /// </summary>
        /// <param name="paramInfo">���</param>
        /// <param name="operation">������("+"��"-")</param>
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
        /// ���û��ͨ����������
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
                    errRequestInfo.LastAuditNote = "���û��ͨ����ϵͳ�Զ�����";
                    ReturnErrRequest(errRequestInfo);
                }

                scope.Complete();
            }
        }

        /// <summary>
        /// ���Լ��㹩Ӧ�̵Ŀ����
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
        /// ����Ӧ�ø��ù�Ӧ�̵�֧���ܶ�
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
        /// ����Ƿ��Ǹù�Ӧ�̵����һ�ʸ���
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
                        throw new BizException("���ݲ����ڣ���ȷ��֧����Ϣ�Ƿ���ȷ");
                }
                else
                {
                    throw new BizException("���ݲ����ڣ���ȷ��֧����Ϣ�Ƿ���ȷ");
                }
            }

            return result;
        }

        #region PO����쳣����
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
        /// ����ϵͳ��ż���PO����쳣����
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
        /// ���ݸ���ż���PO����쳣����      
        /// </summary>
        /// <param name="POPayItemSysNo">������</param>
        /// <param name="IsOnlyLoadValid">�Ƿ�ֻ������Ч��¼</param>
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
        /// ����쳣�����ѯ
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
        /// ���������ύ
        /// </summary>
        /// <param name="paramHash">����POPayItem�Ĳ���</param>
        /// <param name="errRequest">�쳣�����¼</param>
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
                    throw new BizException("ȱ�ٲ���");

                if (errRequest != null)
                    new POPayDac().UpdatePOPayItemErrRequest(errRequest);

                scope.Complete();
            }
        }

        /// <summary>
        /// PM�쳣��������¼
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
                    throw new BizException("�����ظ��ύ�쳣����");
                }
                //else
                //{
                //    #region ����Ǹ��ɹ������жϣ����ɹ����ܴ������ɹ����ĺ�
                //    POPayItemInfo oPayItem = LoadPayItem(errRequestInfo.POPayItemSysNo);
                //    POPayInfo oPay = LoadPay(oPayItem.POSysNo);
                //    if (oPay.POAmt < 0)
                //    {
                //        POInfo poInfo = PurchaseManager.GetInstance().LoadPO(oPayItem.POSysNo);
                //        if (poInfo != null)
                //        {
                //            if (IsPayTotalAmtLessThanZero(poInfo.VendorSysNo))
                //            {
                //                throw new BizException("���ɹ����ܴ������ɹ����ĺͣ����ܸ���");
                //            }
                //        }
                //    }
                //#endregion

                if (1 != new POPayDac().InsertPOPayItemErrRequest(errRequestInfo))
                {
                    throw new BizException("�ύ�쳣����ʧ��");
                }

                scope.Complete();
            }
        }

        /// <summary>
        /// PMȡ���쳣�������
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
                throw new BizException("û����Ч���쳣�����¼");
            }
            else
            {
                Hashtable hs = new Hashtable(2);
                hs.Add("SysNo", SysNo);
                hs.Add("Status", (int)AppEnum.POPayItemErrRequestStatus.PMCancelRequest);

                if (1 != new POPayDac().UpdatePOPayItemErrRequest(hs))
                {
                    throw new BizException("ȡ���쳣����ʧ��");
                }
            }
        }

        /// <summary>
        /// �����쳣�������
        /// </summary>
        /// <param name="errRequest"></param>
        public void ReturnErrRequest(POPayItemErrRequestInfo errRequest)
        {
            if (1 != new POPayDac().UpdatePOPayItemErrRequest(errRequest))
                throw new BizException("���ز���ʧ��");
        }

        /// <summary>
        /// TL�ύ������˸��
        /// </summary>
        /// <param name="POPayItemSysNo">������</param>
        /// <param name="TLUserSysNo">TL�ύ������</param>
        /// <param name="Note">TL��ע</param>
        public void RequestErrPayItemByTL(int POPayItemSysNo, int TLUserSysNo, string Note)
        {
            POPayItemErrRequestInfo errRequestInfo = LoadPOPayItemErrRequest(POPayItemSysNo, true);
            if (errRequestInfo == null)
                throw new BizException("��������¼������");
            if (errRequestInfo.Status != (int)AppEnum.POPayItemErrRequestStatus.PMRequest)
                throw new BizException("��ǰ��������¼״̬���ǡ�PM����״̬�������ύ��PMD���");
            //if (errRequestInfo.TLAuditUserSysNo != TLUserSysNo && TLUserSysNo != AppConst.PMDUserSysNo)
            if (errRequestInfo.TLAuditUserSysNo != TLUserSysNo && TLUserSysNo != PurchaseManager.GetInstance().GetPMDUserSysNo())
                throw new BizException("����Ȩ����TL�쳣����");


            errRequestInfo.Status = (int)AppEnum.POPayItemErrRequestStatus.TLRequst;
            errRequestInfo.TLAuditTime = DateTime.Now;
            errRequestInfo.TLNote = Note;
            //errRequestInfo.LastAuditUserSysNo = AppConst.PMDUserSysNo;
            errRequestInfo.LastAuditUserSysNo = PurchaseManager.GetInstance().GetPMDUserSysNo();

            if (1 != new POPayDac().UpdatePOPayItemErrRequest(errRequestInfo))
                throw new BizException("TL�ύ�쳣����ʧ��");
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
                throw new BizException("ȱ�ٲ�������ѯʧ��");

            return SqlHelper.ExecuteDataSet(sql);
        }

        #endregion


        #region ÿ�ղ���֧������ܶ�����
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
        /// ����ÿ��֧��������� ������Ҫһ������
        /// </summary>
        /// <param name="SysNo">��֪������ֵ���Դ�AppConst.IntNull</param>
        /// <param name="date">��֪������ֵ���Դ�AppConst.DateTimeNull</param>
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
                throw new BizException("�Ѿ����ڽ��յ�֧��������ƣ�����Ҫ�����");
            }
            else if (1 != new DailyPayAmtRestrictDac().Insert(oParam))
            {
                throw new BizException("����ʧ��");
            }
        }

        public void UpdateDailyPayAmtRestrict(DailyPayAmtRestrictInfo oParam)
        {
            if (1 != new DailyPayAmtRestrictDac().Update(oParam))
            {
                throw new BizException("����ʧ��");
            }
        }
        #endregion
    }
}