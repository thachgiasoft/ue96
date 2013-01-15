using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Icson.Utils;
using System.Transactions;
using Icson.Objects;
using Icson.DBAccess;
using Icson.Objects.Finance;
using Icson.DBAccess.Finance;

namespace Icson.BLL.Finance
{
    public class PMPaymentManager
    {
        private PMPaymentManager()
        {
        }
        private static PMPaymentManager _instance;
        public static PMPaymentManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new PMPaymentManager();
            }
            return _instance;
        }

        private void map(PMPaymentInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.PayDate = Util.TrimDateNull(tempdr["PayDate"]);
            oParam.PMSysNo = Util.TrimIntNull(tempdr["PMSysNo"]);
            oParam.PayAmt = Util.TrimDecimalNull(tempdr["PayAmt"]);
            oParam.StockAmt = Util.TrimDecimalNull(tempdr["StockAmt"]);
            oParam.DateStamp = Util.TrimDateNull(tempdr["DateStamp"]);
        }

        public void Insert(PMPaymentInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                new PMPaymentDac().Insert(oParam);
                scope.Complete();
            }
        }

        public bool IsHasImportedPMPayment(string Today)
        {
            string sql = "select * from pmpayment where paydate='@today'";
            sql = sql.Replace("@today", Today);
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 成批生成pm当天的资金占用情况
        /// </summary>
        /// <param name="Today"></param>
        public void ImportPMPayment(string Today)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                //                //帐期顺延一天
                //                int UnPay = (int)AppEnum.POPayStatus.UnPay;
                //                int PartlyPay = (int)AppEnum.POPayStatus.PartlyPay;
                //                string Tomorrow = Convert.ToDateTime(Today).AddDays(1).ToString("yyyy-MM-dd");
                //                string sql1 = @"update po_master set po_master.paydate = '@tomorrow' 
                //                                from finance_popay_item 
                //                                where po_master.sysno=finance_popay_item.posysno 
                //                                and po_master.paydate='@today' 
                //                                and (finance_popay_item.status=@unpay or finance_popay_item.status=@partlypay)";
                //                sql1 = sql1.Replace("@today", Today);
                //                sql1 = sql1.Replace("@tomorrow", Tomorrow);
                //                sql1 = sql1.Replace("@unpay", UnPay.ToString());
                //                sql1 = sql1.Replace("@partlypay",PartlyPay.ToString());
                //                SqlHelper.ExecuteNonQuery(sql1);

                //当天的资金占用数据存入数据库
                string sql2 = @"insert into pmpayment(paydate,pmsysno,payamt,stockamt,datestamp) 
                                select '@today' as today, v2.sysno as pmsysno, 
                                isnull(v1.pmtotalamt,0) as pmamt,isnull(v2.pmstockfunds,0) as stockamt,getdate() as datestamp 
                                from 
                                (select sys_user.sysno,sys_user.username, 
                                sum( TotalAmt * (cast(datediff(day,getdate(),po_master.paydate) as decimal(5,2)) / cast(datediff(day,po_master.intime,po_master.paydate) as decimal(5,2)))) as PMTotalAmt 
                                from po_master inner join sys_user on sys_user.sysno=po_master.createusersysno and sys_user.status=0 
                                inner join finance_popay on po_master.sysno=finance_popay.posysno @Filterpaystatus 
                                and po_master.paydate > po_master.intime and po_master.paydate > '@today' 
                                and po_master.paydate is not null and po_master.intime is not null 
                                group by sys_user.sysno,sys_user.username)  v1 right outer join 
                                (select sys_user.sysno,sys_user.username, sum((isnull(product_price.unitcost,0)*isnull(inventory.accountQty,0))) as PMStockFunds 
                                from inventory inner join product_price on inventory.productsysno=product_price.productsysno 
                                inner join product on inventory.productsysno=product.sysno inner join sys_user on product.PMUsersysno=sys_user.sysno and sys_user.status=0 
                                group by sys_user.sysno,sys_user.username) v2 
                                on v1.sysno = v2.sysno ";
                int UnPay = (int)AppEnum.POPayStatus.UnPay;
                sql2 = sql2.Replace("@Filterpaystatus", " and finance_popay.paystatus=" + UnPay.ToString());
                sql2 = sql2.Replace("@today", Today);
                SqlHelper.ExecuteNonQuery(sql2);
                scope.Complete();
            }
        }
    }
}