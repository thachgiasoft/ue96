using System;
using System.Data;
using System.Collections;
using System.Text;

using Icson.Utils;
using System.Transactions;

using Icson.Objects;
using Icson.Objects.Basic;
using Icson.Objects.Finance;
using Icson.Objects.RMA;

using Icson.DBAccess;
using Icson.DBAccess.Finance;
using Icson.BLL.Basic;
using Icson.BLL.RMA;

using Icson.BLL.Sale;

namespace Icson.BLL.Finance
{
    /// <summary>
    /// 提供插入、修改note、和confirm, cancel confirm, abandon, cancel abandon
    /// 一个so/rma 只能有一个soincome的记录
    /// </summary>
    public class SOIncomeManager
    {
        private SOIncomeManager()
        {
        }
        private static SOIncomeManager _instance;
        public static SOIncomeManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SOIncomeManager();
            }
            return _instance;
        }
        private void map(SOIncomeInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.OrderType = Util.TrimIntNull(tempdr["OrderType"]);
            oParam.OrderSysNo = Util.TrimIntNull(tempdr["OrderSysNo"]);
            oParam.OrderAmt = Util.TrimDecimalNull(tempdr["OrderAmt"]);
            oParam.IncomeStyle = Util.TrimIntNull(tempdr["IncomeStyle"]);
            oParam.IncomeAmt = Util.TrimDecimalNull(tempdr["IncomeAmt"]);
            oParam.IncomeTime = Util.TrimDateNull(tempdr["IncomeTime"]);
            oParam.IncomeUserSysNo = Util.TrimIntNull(tempdr["IncomeUserSysNo"]);
            oParam.ConfirmTime = Util.TrimDateNull(tempdr["ConfirmTime"]);
            oParam.ConfirmUserSysNo = Util.TrimIntNull(tempdr["ConfirmUserSysNo"]);
            oParam.Note = Util.TrimNull(tempdr["Note"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
        }
        public SOIncomeInfo Load(int sysno)
        {
            string sql = "select * from finance_soincome where sysno =" + sysno;
            return load(sql);
        }
        public SOIncomeInfo Load(int orderType, int orderSysNo)
        {
            string sql = "select * from finance_soincome where ordertype=" + orderType + " and ordersysno=" + orderSysNo;
            return load(sql);
        }
        public SOIncomeInfo LoadValid(int orderType, int orderSysNo)
        {
            string sql = "select * from finance_soincome where ordersysno=" + orderSysNo + " and status>" + (int)AppEnum.SOIncomeStatus.Abandon + " and ordertype=" + orderType;
            return load(sql);
        }

        public SOIncomeInfo load(string sql)
        {
            SOIncomeInfo oInfo = null;
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (Util.HasMoreRow(ds))
                {
                    oInfo = new SOIncomeInfo();
                    map(oInfo, ds.Tables[0].Rows[0]);
                }
                scope.Complete();
            }
            return oInfo;
        }
        public void Insert(SOIncomeInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                SOIncomeInfo dbInfo = LoadValid(oParam.OrderType, oParam.OrderSysNo);
                if (dbInfo != null)
                    throw new BizException("soincome: record exist already, insert failed");

                new SOIncomeDac().Insert(oParam);

                if (oParam.OrderType == (int)AppEnum.SOIncomeOrderType.SO)
                    SaleManager.GetInstance().PaySO(oParam.OrderSysNo);

                scope.Complete();
            }
        }
        public void Update(SOIncomeInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                SOIncomeInfo dbInfo = Load(oParam.SysNo);
                if (dbInfo.Status != (int)AppEnum.SOIncomeStatus.Origin)
                    throw new BizException("soincome: only origin status can update");

                Hashtable ht = new Hashtable(2);
                ht.Add("SysNo", oParam.SysNo);
                ht.Add("Note", oParam.Note);
                new SOIncomeDac().Update(ht);

                scope.Complete();
            }
        }

        public void Confirm(int soIncomeSysNo, int userSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                SOIncomeInfo dbInfo = Load(soIncomeSysNo);
                if (dbInfo.Status != (int)AppEnum.SOIncomeStatus.Origin)
                    throw new BizException("soincome: only origin status can be confirmed");
                dbInfo.Status = (int)AppEnum.SOIncomeStatus.Confirmed;

                Hashtable ht = new Hashtable(5);
                ht.Add("SysNo", soIncomeSysNo);
                ht.Add("ConfirmTime", DateTime.Now);
                ht.Add("ConfirmUserSysNo", userSysNo);
                ht.Add("Status", dbInfo.Status);

                new SOIncomeDac().Update(ht);

                scope.Complete();
            }
        }

        public void ConfirmList(Hashtable htList, int userSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                foreach (int soIncomeSysNo in htList.Keys)
                {
                    SOIncomeInfo dbInfo = Load(soIncomeSysNo);
                    if (dbInfo.Status != (int)AppEnum.SOIncomeStatus.Origin)
                        throw new BizException("soincome: only origin status can be confirmed");
                    dbInfo.Status = (int)AppEnum.SOIncomeStatus.Confirmed;
                    Hashtable ht = new Hashtable(5);
                    ht.Add("SysNo", soIncomeSysNo);
                    ht.Add("ConfirmTime", DateTime.Now);
                    ht.Add("ConfirmUserSysNo", userSysNo);
                    ht.Add("Status", dbInfo.Status);

                    new SOIncomeDac().Update(ht);
                }
                scope.Complete();
            }
        }

        public void UnConfirm(int soIncomeSysNo, int userSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                SOIncomeInfo dbInfo = Load(soIncomeSysNo);
                if (dbInfo.Status != (int)AppEnum.SOIncomeStatus.Confirmed)
                    throw new BizException("soincome: only confirmed status can be unconfirmed");

                dbInfo.Status = (int)AppEnum.SOIncomeStatus.Origin;

                Hashtable ht = new Hashtable(5);
                ht.Add("SysNo", soIncomeSysNo);
                ht.Add("ConfirmTime", DateTime.Now);
                ht.Add("ConfirmUserSysNo", userSysNo);
                ht.Add("Status", dbInfo.Status);

                new SOIncomeDac().Update(ht);

                DeleteSOIncomeVoucher(soIncomeSysNo);

                scope.Complete();
            }
        }

        public void ManualAbandon(int soIncomeSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                SOIncomeInfo dbInfo = Load(soIncomeSysNo);
                if (dbInfo.Status != (int)AppEnum.SOIncomeStatus.Origin)
                    throw new BizException("soincome: the status is not origin, abandon failed.");

                if (dbInfo.IncomeStyle != (int)AppEnum.SOIncomeStyle.Advanced)
                    throw new BizException("Only advanced income can be manually abandon");

                Hashtable ht = new Hashtable(5);
                ht.Add("SysNo", dbInfo.SysNo);
                ht.Add("Status", (int)AppEnum.SOIncomeStatus.Abandon);
                new SOIncomeDac().Update(ht);

                NetPayManager.GetInstance().SOIncomeAbandon(dbInfo.OrderSysNo);

                //如果Manual Abandon 
                SaleManager.GetInstance().UnPaySO(dbInfo.OrderSysNo);

                DeleteSOIncomeVoucher(soIncomeSysNo);

                scope.Complete();
            }
        }
        public void SOAbandon(int soSysNo)
        {
            /*
                这时候只能存在Advanced收款单，Normal的在取消出库的时候已经处理了
                如果soincome(advanced,confirmed) 抛出异常：提示必须先手动取消财务确认
                如果soincome(advanced, origin) 抛出异常：必须先手动作废收款单.
             */
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                SOIncomeInfo dbInfo = LoadValid((int)AppEnum.SOIncomeOrderType.SO, soSysNo);
                if (dbInfo != null)
                {
                    if (dbInfo.Status == (int)AppEnum.SOIncomeStatus.Confirmed)
                        throw new BizException(soSysNo + "收款单财务已经确认，请通知财务取消确认");
                    if (dbInfo.Status == (int)AppEnum.SOIncomeStatus.Origin)
                        throw new BizException(soSysNo + "存在有效收款单，请手动作废收款单");
                }
                scope.Complete();
            }
        }
        public void SOCancelOutStock(int soSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                SOIncomeInfo dbInfo = LoadValid((int)AppEnum.SOIncomeOrderType.SO, soSysNo);

                if (dbInfo.IncomeStyle == (int)AppEnum.SOIncomeStyle.Normal)
                {
                    if (dbInfo.Status == (int)AppEnum.SOIncomeStatus.Confirmed)
                        throw new BizException("收款单财务已经确认，请通知财务取消确认");

                    Hashtable ht = new Hashtable(5);
                    ht.Add("SysNo", dbInfo.SysNo);
                    ht.Add("Status", (int)AppEnum.SOIncomeStatus.Abandon);

                    new SOIncomeDac().Update(ht);

                    LogInfo log = new LogInfo();
                    log.OptIP = AppConst.SysIP;
                    log.OptUserSysNo = AppConst.SysUser;
                    log.OptTime = DateTime.Now;
                    log.TicketType = (int)AppEnum.LogType.Finance_SOIncome_Abandon;
                    log.TicketSysNo = dbInfo.SysNo;
                    LogManager.GetInstance().Write(log);
                }

                scope.Complete();
            }

        }
        public void ROCancelReturn(int roSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                SOIncomeInfo dbInfo = LoadValid((int)AppEnum.SOIncomeOrderType.RO, roSysNo);
                if (dbInfo.Status == (int)AppEnum.SOIncomeStatus.Confirmed)
                    throw new BizException("收款单财务已经确认，请通知财务取消确认");

                Hashtable ht = new Hashtable(5);
                ht.Add("SysNo", dbInfo.SysNo);
                ht.Add("Status", (int)AppEnum.SOIncomeStatus.Abandon);

                new SOIncomeDac().Update(ht);
                scope.Complete();
            }
        }

        public DataSet SOIncomeSearch(Hashtable paramHash)
        {
            bool HasFreightFilter = false;
            string sql = "";
            string sqlSO = @"select fs.sysno,fs.incomeStyle,fs.ordertype,fs.orderamt,fs.incomeamt,suiu.username as incomeuser,
                           fs.incometime,sucu.username as confirmuser,fs.confirmtime,
						   sm.soid as orderid,fs.status as incomestatus,sm.receiveareasysno,sm.sysno as sosysno,fsv.voucherid,fsv.vouchertime,sm.status,-isnull(vwPreSaleRefundAmt.presalerefundamt,0) as presalerefundamt  
						   from finance_soincome fs
						   left join sys_user suiu on suiu.sysno = fs.incomeusersysno
						   left join sys_user sucu on sucu.sysno = fs.confirmusersysno 
                           left join finance_soincome_voucher fsv on fsv.fsisysno = fs.sysno                            
						   inner join so_master sm on sm.sysno = fs.ordersysno and fs.ordertype = " + (int)AppEnum.SOIncomeOrderType.SO
                        + " left join vwPreSaleRefundAmt on sm.sysno=vwPreSaleRefundAmt.sosysno "
                        + " where 1=1 ";
            string sqlRO =
                @"select fs.sysno,fs.incomeStyle,fs.ordertype,fs.orderamt,fs.incomeamt,suiu.username as incomeuser,
                           fs.incometime,sucu.username as confirmuser,fs.confirmtime,
						   rm.roid as orderid,fs.status as incomestatus, sm.receiveareasysno,fs.ordersysno as sosysno,fsv.voucherid,fsv.vouchertime,sm.status,'0' as presalerefundamt 
						   from finance_soincome fs                            
						   left join sys_user suiu on suiu.sysno = fs.incomeusersysno
						   left join sys_user sucu on sucu.sysno = fs.confirmusersysno
                           left join finance_soincome_voucher fsv on fsv.fsisysno = fs.sysno
						   inner join ro_master rm on rm.sysno = fs.ordersysno and fs.ordertype = " + (int)AppEnum.SOIncomeOrderType.RO
                        + @" inner join rma_master rma on rm.rmasysno = rma.sysno
		                    inner join so_master sm on rma.sosysno = sm.sysno 
						    where 1=1 ";
            string sqlRR =  //退款单RMA_Refund
                @"select fs.sysno,fs.incomeStyle,fs.ordertype,fs.orderamt,fs.incomeamt,suiu.username as incomeuser,
                           fs.incometime,sucu.username as confirmuser,fs.confirmtime,
						   rr.refundid as orderid,fs.status as incomestatus, sm.receiveareasysno,fs.ordersysno as sosysno,fsv.voucherid,fsv.vouchertime,sm.status , '0' as presalerefundamt 
						   from finance_soincome fs                            
						   left join sys_user suiu on suiu.sysno = fs.incomeusersysno
						   left join sys_user sucu on sucu.sysno = fs.confirmusersysno
                           left join finance_soincome_voucher fsv on fsv.fsisysno = fs.sysno
						   inner join rma_refund rr on rr.sysno = fs.ordersysno and fs.ordertype = " + (int)AppEnum.SOIncomeOrderType.RO
                        + @" inner join so_master sm on rr.sosysno = sm.sysno 
						    where 1=1 ";
            if (paramHash.Count > 0)
            {
                StringBuilder sbSO = new StringBuilder();
                StringBuilder sbRO = new StringBuilder();
                StringBuilder sbRR = new StringBuilder();

                foreach (string key in paramHash.Keys)
                {
                    Object item = paramHash[key];
                    sbSO.Append(" and ");
                    sbRO.Append(" and ");
                    sbRR.Append(" and ");
                    if (key == "OrderID")
                    {
                        sbSO.Append("sm.soid").Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                        sbRO.Append("( rm.roid like " + Util.ToSqlLikeString(item.ToString()) + " or sm.sysno=" + Util.ToSqlString(item.ToString()) + ")");      //可以根据销售单号查出相关的退款
                        sbRR.Append("( rr.refundid like " + Util.ToSqlLikeString(item.ToString()) + " or sm.sysno=" + Util.ToSqlString(item.ToString()) + ")");   //可以根据销售单号查出相关的退款
                    }
                    else if (key == "StartDate")
                    {
                        sbSO.Append("fs.incometime").Append(">=").Append(Util.ToSqlString(item.ToString()));
                        sbRO.Append("fs.incometime").Append(">=").Append(Util.ToSqlString(item.ToString()));
                        sbRR.Append("fs.incometime").Append(">=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "EndDate")
                    {
                        sbSO.Append("fs.incometime").Append("<=").Append(Util.ToSqlEndDate(item.ToString()));
                        sbRO.Append("fs.incometime").Append("<=").Append(Util.ToSqlEndDate(item.ToString()));
                        sbRR.Append("fs.incometime").Append("<=").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "ConfirmTimeFrom")
                    {
                        sbSO.Append("fs.confirmtime").Append(">=").Append(Util.ToSqlString(item.ToString()));
                        sbRO.Append("fs.confirmtime").Append(">=").Append(Util.ToSqlString(item.ToString()));
                        sbRR.Append("fs.confirmtime").Append(">=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "ConfirmTimeTo")
                    {
                        sbSO.Append("fs.confirmtime").Append("<=").Append(Util.ToSqlEndDate(item.ToString()));
                        sbRO.Append("fs.confirmtime").Append("<=").Append(Util.ToSqlEndDate(item.ToString()));
                        sbRR.Append("fs.confirmtime").Append("<=").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "IncomeStatus")
                    {
                        sbSO.Append("fs.status").Append("=").Append(item.ToString());
                        sbRO.Append("fs.status").Append("=").Append(item.ToString());
                        sbRR.Append("fs.status").Append("=").Append(item.ToString());
                    }
                    else if (key == "PayTypeSysNo")
                    {
                        sbSO.Append("payTypeSysNo").Append("=").Append(item.ToString());
                        //ro 不包含paytype
                    }
                    else if (key == "DateFrom")
                    {
                        sbSO.Append("sm.SetdeliveryManTime >= ").Append(Util.ToSqlString(item.ToString()));
                        sbRO.Append("sm.SetdeliveryManTime >= ").Append(Util.ToSqlString(item.ToString()));
                        sbRR.Append("sm.SetdeliveryManTime >= ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "DateTo")
                    {
                        sbSO.Append("sm.SetdeliveryManTime <= ").Append(Util.ToSqlString(item.ToString()));
                        sbRO.Append("sm.SetdeliveryManTime <= ").Append(Util.ToSqlString(item.ToString()));
                        sbRR.Append("sm.SetdeliveryManTime <= ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "FreightUserSysNo")
                    {
                        HasFreightFilter = true;
                        sbSO.Append("sm.FreightUserSysNo").Append("=").Append(item.ToString());
                        sbRO.Append("sm.FreightUserSysNo").Append("=").Append(item.ToString());
                        sbRR.Append("sm.FreightUserSysNo").Append("=").Append(item.ToString());
                    }
                    else if (key == "VoucherID")
                    {
                        sbSO.Append(" fsv.voucherid ").Append("=").Append(Util.ToSqlString(item.ToString()));
                        sbRO.Append(" fsv.voucherid ").Append("=").Append(Util.ToSqlString(item.ToString()));
                        sbRR.Append(" fsv.voucherid ").Append("=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "VoucherTimeFrom")
                    {
                        sbSO.Append("fsv.VoucherTime >= ").Append(Util.ToSqlString(item.ToString()));
                        sbRO.Append("fsv.VoucherTime >= ").Append(Util.ToSqlString(item.ToString()));
                        sbRR.Append("fsv.VoucherTime >= ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "VoucherTimeTo")
                    {
                        sbSO.Append("fsv.VoucherTime <= ").Append(Util.ToSqlEndDate(item.ToString()));
                        sbRO.Append("fsv.VoucherTime <= ").Append(Util.ToSqlEndDate(item.ToString()));
                        sbRR.Append("fsv.VoucherTime <= ").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "SOStatus")
                    {
                        sbSO.Append("sm.Status").Append("=").Append(item.ToString());
                        sbRO.Append("sm.Status").Append("=").Append(item.ToString());
                        sbRR.Append("sm.Status").Append("=").Append(item.ToString());
                    }
                    else if (key == "DSSysNoList")
                    {
                        HasFreightFilter = true;
                        string dsSysNoList = item.ToString().Replace("，", ",");
                        string filtersql = "select ItemID from ds_item where dsSysNo in(" + dsSysNoList + ")";
                        sbSO.Append("sm.SOID").Append(" in(").Append(filtersql).Append(")");
                        sbRO.Append(" 1<>1 ");
                        sbRR.Append(" 1<>1 ");
                    }
                    else if (item is int)
                    {
                        sbSO.Append(key).Append("=").Append(item.ToString());
                        sbRO.Append(key).Append("=").Append(item.ToString());
                        sbRR.Append(key).Append("=").Append(item.ToString());
                    }
                    else if (item is string)
                    {
                        sbSO.Append(key).Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                        sbRO.Append(key).Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                        sbRR.Append(key).Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                    }
                }
                sqlSO += sbSO.ToString();
                sqlRO += sbRO.ToString();
                sqlRR += sbRR.ToString();
            }
            if (paramHash.ContainsKey("PayTypeSysNo"))
            {
                paramHash.Remove("OrderType");
                paramHash.Add("OrderType", (int)AppEnum.SOIncomeOrderType.SO);
            }
            if (paramHash.ContainsKey("OrderType"))
            {
                if ((int)paramHash["OrderType"] == (int)AppEnum.SOIncomeOrderType.SO)
                    sql = sqlSO;
                else if ((int)paramHash["OrderType"] == (int)AppEnum.SOIncomeOrderType.RO)
                    sql = sqlRO + " union all " + sqlRR;
            }
            else
            {
                if (!HasFreightFilter)
                    sql = sqlSO + " union all " + sqlRO + " union all " + sqlRR;
                else
                    sql = sqlSO;
            }
            //if (HasVoucherFilter)
            //{
            //    sql = sql.Replace("@joinsovoucher", " inner join so_voucher ");
            //    sql = sql.Replace("@joinrovoucher", " inner join ro_voucher ");

            //}
            //else
            //{
            //    sql = sql.Replace("@joinsovoucher", " left join so_voucher ");
            //    sql = sql.Replace("@joinrovoucher", " left join ro_voucher ");
            //}

            if (paramHash.Count != 0)
                sql = "select * from (" + sql + ") as a ";//"order by sysno desc";
            else
                sql = "select top 50 * from (" + sql + ") as a ";// "order by sysno desc";

            if (HasFreightFilter)
            {
                sql += " order by receiveareasysno,orderid";
            }
            else
            {
                sql += " order by sysno desc";
            }

            return SqlHelper.ExecuteDataSet(sql);
        }

        public void Import()
        {
            if (!AppConfig.IsImportable)
                throw new BizException("Is Importable is false");

            string sql = " select top 1 sysno from finance_soincome";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                throw new BizException("the table finance_soincome is not empty");

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                //1 将原来的收款单导入， 
                //2 对于有应收而没有收款单的，因为原来的早一点的应收记录不准确的，
                //  所以这部分的来源用 so已经出库而没有收款单的，生成收款单
                // ???
                UserInfo oUser = SysManager.GetInstance().LoadUser("0061");

                string sql2 = @"select 
									0 as sysno, ordertype, SOorUTSysNo as ordersysno,
									incomeamt as orderamt,
									incomeamt_local as incomeamt,
									incometype as incomestyle,
									createtime as incometime,
									con_user1.newsysno as incomeUserSysNo,
									con_user2.newsysno as confirmUserSysNo,
									null as confirmTime,
									note,
									status
								from 
									ipp2003..so_income as so_income, 
									ippConvert..sys_user as con_user1,
									ippConvert..sys_user as con_user2
								where
									so_income.createusersysno *= con_user1.oldsysno
								and	so_income.accountsysno *= con_user2.oldsysno
								order by so_income.ordersysno asc, so_income.status desc";
                DataSet ds2 = SqlHelper.ExecuteDataSet(sql2);

                int lastsosysno = AppConst.IntNull;
                bool gotvalid = false; //如果有多条valid（orign 和 confirm）只能导入一条
                foreach (DataRow dr in ds2.Tables[0].Rows)
                {
                    SOIncomeInfo item = new SOIncomeInfo();
                    map(item, dr);

                    //当前的是一个新的sosysno
                    if (lastsosysno != item.OrderSysNo)
                    {
                        lastsosysno = item.OrderSysNo;
                        gotvalid = false;
                    }

                    /*old
                     * IncomeType 0:正常 1:预收 2:退款
                     * ordertype 0: so, 1: ro
                     * status 0: 已确认 -1:作废 -2:未确认
                     */

                    if (item.OrderType == 0)
                        item.OrderType = (int)AppEnum.SOIncomeOrderType.SO;
                    else if (item.OrderType == 1)
                        item.OrderType = (int)AppEnum.SOIncomeOrderType.RO;
                    else
                        throw new BizException("no such order type");

                    if (item.IncomeStyle == 0)
                        item.IncomeStyle = (int)AppEnum.SOIncomeStyle.Normal;
                    else if (item.IncomeStyle == 1)
                        item.IncomeStyle = (int)AppEnum.SOIncomeStyle.Advanced;
                    else if (item.IncomeStyle == 2)
                        item.IncomeStyle = (int)AppEnum.SOIncomeStyle.RO;
                    else
                        throw new BizException("no such imcometype");

                    if (item.Status == -1)
                        item.Status = (int)AppEnum.SOIncomeStatus.Abandon;
                    else if (item.Status == -2)
                    {
                        item.Status = (int)AppEnum.SOIncomeStatus.Origin;
                    }
                    else if (item.Status == 0)
                    {
                        item.Status = (int)AppEnum.SOIncomeStatus.Confirmed;
                    }
                    else
                        throw new BizException("no such status");

                    if (item.IncomeUserSysNo == AppConst.IntNull)
                        item.IncomeUserSysNo = oUser.SysNo;
                    if (item.ConfirmUserSysNo != AppConst.IntNull)
                        item.ConfirmTime = DateTime.Now;

                    if (item.Status == (int)AppEnum.SOIncomeStatus.Abandon
                        || !gotvalid)
                    {
                        new SOIncomeDac().Insert(item);
                        if (item.Status == (int)AppEnum.SOIncomeStatus.Origin
                            || item.Status == (int)AppEnum.SOIncomeStatus.Confirmed)
                            gotvalid = true;
                    }
                }

                /*
                 *  这种情况下导入的数据有问题，sql1里面用的是cashamt, 这个仅仅是so的货款现金支付部分。
                 *  用下面的语句更新如下 2005.05.18 18:42 by marco
                 * update finance_soincome
                    set 
                        orderamt = round(cashamt+shipprice+premiumamt+payprice,2),
                        incomeamt = round(cashamt+shipprice+premiumamt+payprice,2)
					 
                                                    from 
                                                        ipp2003..so_master so,
                                                        ippConvert..sys_user as con_user
                                                    where
                                                        so.warehouseUserSysNo =con_user.oldsysno
                                                    and	so.status = 4
                                                    and so.soid not in
                                                        ( select soOrUtid from ipp2003..so_income where ordertype = 0)
                                                    and outtime >= '2003-08-07'
                        and so.sysno = ordersysno and ordertype = 1
                 */
                string sql1 = @"select
									0 as sysno, 0 as orderType, so.sysno as orderSysNo,
									cashamt as orderamt,
									0 as incomestyle,
									cashamt as incomeamt,
									outtime as incometime,
									con_user.newsysno as incomeusersysno,
									null as confirmtime,
									null as confirmusersysno,
									'ipp3导入' as note,
									0 as status
								from 
									ipp2003..so_master so,
									ippConvert..sys_user as con_user
								where
									so.warehouseUserSysNo =con_user.oldsysno
								and	so.status = 4
								and so.soid not in
									( select soOrUtid from ipp2003..so_income where ordertype = 0)
								and outtime >= '2003-08-07'";
                DataSet ds1 = SqlHelper.ExecuteDataSet(sql1);
                if (!Util.HasMoreRow(ds1))
                    return;
                foreach (DataRow dr in ds1.Tables[0].Rows)
                {
                    SOIncomeInfo item = new SOIncomeInfo();
                    map(item, dr);
                    item.OrderType = (int)AppEnum.SOIncomeOrderType.SO;
                    item.IncomeStyle = (int)AppEnum.SOIncomeStyle.Normal;
                    item.Status = (int)AppEnum.SOIncomeStatus.Origin;
                    new SOIncomeDac().Insert(item);
                }




                scope.Complete();
            }

        }

        #region voucher
        public int InsertSOIncomeVoucher(SOIncomeVoucherInfo oParam)
        {
            return new SOIncomeVoucherDac().Insert(oParam);
        }

        public int UpdateSOIncomeVoucher(SOIncomeVoucherInfo oParam)
        {
            return new SOIncomeVoucherDac().Update(oParam);
        }

        public int DeleteSOIncomeVoucher(int FSISysNo)
        {
            return new SOIncomeVoucherDac().Delete(FSISysNo);
        }

        public SOIncomeVoucherInfo LoadSOIncomeVoucher(SOIncomeVoucherInfo oParam)
        {
            string sql = "select * from finance_soincome_voucher where fsisysno=" + oParam.FSISysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                SOIncomeVoucherInfo newInfo = new SOIncomeVoucherInfo();
                map(newInfo, ds.Tables[0].Rows[0]);
                return newInfo;
            }
            else
            {
                return null;
            }
        }

        public Hashtable LoadSOIncomeVoucherList(string VoucherID)
        {
            string sql = "select * from finance_soincome_voucher where voucherid = " + Util.ToSqlString(VoucherID);
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                Hashtable ht = new Hashtable();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    SOIncomeVoucherInfo oInfo = new SOIncomeVoucherInfo();
                    map(oInfo, dr);
                    ht.Add(oInfo, null);
                }
                return ht;
            }
            else
                return null;
        }

        private void map(SOIncomeVoucherInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.FSISysNo = Util.TrimIntNull(tempdr["FSISysNo"]);
            oParam.VoucherID = Util.TrimNull(tempdr["VoucherID"]);
            oParam.VoucherTime = Util.TrimDateNull(tempdr["VoucherTime"]);
            oParam.SysUserSysNo = Util.TrimIntNull(tempdr["SysUserSysNo"]);
            oParam.DateStamp = Util.TrimDateNull(tempdr["DateStamp"]);
        }

        public void InsertSOIncomeVoucherList(Hashtable ht)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                foreach (SOIncomeVoucherInfo oParam in ht.Keys)
                {
                    SOIncomeVoucherInfo newInfo = LoadSOIncomeVoucher(oParam);
                    if (newInfo == null)
                        InsertSOIncomeVoucher(oParam);
                    else
                    {
                        newInfo.VoucherID = oParam.VoucherID;
                        newInfo.VoucherTime = oParam.VoucherTime;
                        UpdateSOIncomeVoucher(newInfo);
                    }

                    SOIncomeInfo oInfo = SOIncomeManager.GetInstance().Load(oParam.FSISysNo);
                    if (oInfo.OrderType == (int)AppEnum.SOIncomeOrderType.RO)
                    {
                        RMARefundInfo oRefundInfo = RMARefundManager.GetInstance().LoadRMARefund(oInfo.OrderSysNo);
                        if (oRefundInfo != null)
                        {
                            Hashtable rmaht = new Hashtable();
                            rmaht.Add("SysNo", oRefundInfo.SysNo);
                            rmaht.Add("VoucherID", oParam.VoucherID);
                            rmaht.Add("VoucherTime", oParam.VoucherTime);
                            RMARefundManager.GetInstance().UpdateMasterMemo(rmaht);
                        }

                    }
                }
                scope.Complete();
            }
        }

        public void EnterVoucherAndConfirm(Hashtable htVoucher, Hashtable htConfirm, int userSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                InsertSOIncomeVoucherList(htVoucher);
                ConfirmList(htConfirm, userSysNo);
                scope.Complete();
            }
        }
        #endregion
    }
}
