using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Transactions;
using System.Data;
using Icson.DBAccess;
using Icson.DBAccess.Basic;
using Icson.Utils;
using Icson.Objects;
using Icson.Objects.Basic;
using Icson.BLL.Sale;

namespace Icson.BLL.Basic
{
    public class PriceReportManager
    {
        private PriceReportManager()
        {
        }
        private static PriceReportManager _instance;
        public static PriceReportManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new PriceReportManager();
            }
            return _instance;
        }

        private void map(PriceReportInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.ProductSysNo = Util.TrimIntNull(tempdr["ProductSysNo"]);
            oParam.CurrentPrice = Util.TrimDecimalNull(tempdr["CurrentPrice"]);
            oParam.UnitCost = Util.TrimDecimalNull(tempdr["UnitCost"]);
            oParam.LastOrderPrice = Util.TrimDecimalNull(tempdr["LastOrderPrice"]);
            oParam.CompetitorSysNo = Util.TrimIntNull(tempdr["CompetitorSysNo"]);
            oParam.CompetitorPrice = Util.TrimDecimalNull(tempdr["CompetitorPrice"]);
            oParam.CompetitorUrl = Util.TrimNull(tempdr["CompetitorUrl"]);
            oParam.CustomerSysNo = Util.TrimIntNull(tempdr["CustomerSysNo"]);
            oParam.NickName = Util.TrimNull(tempdr["NickName"]);
            oParam.Email = Util.TrimNull(tempdr["Email"]);
            oParam.CustomerMemo = Util.TrimNull(tempdr["CustomerMemo"]);
            oParam.ReportTime = Util.TrimDateNull(tempdr["ReportTime"]);
            oParam.CustomerIP = Util.TrimNull(tempdr["CustomerIP"]);
            oParam.AuditUserSysNo = Util.TrimIntNull(tempdr["AuditUserSysNo"]);
            oParam.AuditMemo = Util.TrimNull(tempdr["AuditMemo"]);
            oParam.AuditNote = Util.TrimNull(tempdr["AuditNote"]);
            oParam.AuditTime = Util.TrimDateNull(tempdr["AuditTime"]);
            oParam.AbandonUserSysNo = Util.TrimIntNull(tempdr["AbandonUserSysNo"]);
            oParam.AbandonTime = Util.TrimDateNull(tempdr["AbandonTime"]);
            oParam.Point = Util.TrimIntNull(tempdr["Point"]);
            oParam.HandleType = Util.TrimIntNull(tempdr["HandleType"]);
            oParam.Reason = Util.TrimIntNull(tempdr["Reason"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
        }

        public PriceReportInfo Load(int SysNo)
        {
            string sql = "select * from price_report where sysno=" + SysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                PriceReportInfo oInfo = new PriceReportInfo();
                map(oInfo, ds.Tables[0].Rows[0]);
                return oInfo;
            }
            else
                return null;
        }

        public PriceReportInfo Load(int ProductSysNo,int CustomerSysNo)
        {
            string sql = "select * from price_report where productsysno=" + ProductSysNo + " and customersysno=" + CustomerSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                PriceReportInfo oInfo = new PriceReportInfo();
                map(oInfo, ds.Tables[0].Rows[0]);
                return oInfo;
            }
            else
                return null;
        }

        public int Insert(PriceReportInfo oParam)
        {
            PriceReportInfo pr = Load(oParam.ProductSysNo, oParam.CustomerSysNo);
            if (pr != null && pr.ReportTime.ToString(AppConst.DateFormat) == DateTime.Now.ToString(AppConst.DateFormat))
            {
                throw new BizException("请勿重复提交价格举报信息！");
            }
            return new PriceReportDac().Insert(oParam);
        }

        public void Update(Hashtable paramHash)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                if (paramHash.ContainsKey("Status"))
                {
                    bool NeedAddPoint = false;

                    int newStatus = Int32.Parse(paramHash["Status"].ToString());
                    int SysNo = Int32.Parse(paramHash["SysNo"].ToString());
                    PriceReportInfo oInfo = Load(SysNo);

                    if (newStatus == (int)AppEnum.PriceReportStatus.Abandon)
                    {
                        if (oInfo.Status != (int)AppEnum.PriceReportStatus.Origin&&oInfo.Status!=(int)AppEnum.PriceReportStatus.AuditWaitingChangePrice)
                            throw new BizException("非初始状态，不能作废");
                    }
                    else if (newStatus == (int)AppEnum.PriceReportStatus.Origin)
                    {
                        if (oInfo.Status != (int)AppEnum.PriceReportStatus.Abandon)
                            throw new BizException("非作废状态，不能取消作废");
                    }
                    else if (newStatus == (int)AppEnum.PriceReportStatus.AuditedNotChangePrice)
                    {
                        NeedAddPoint = true;
                        if (oInfo.Status != (int)AppEnum.PriceReportStatus.Origin && oInfo.Status != (int)AppEnum.PriceReportStatus.AuditWaitingChangePrice)
                            throw new BizException("非初始状态，不能审核不修改价格");
                    }
                    else if (newStatus == (int)AppEnum.PriceReportStatus.AuditChangePrice)
                    {
                        NeedAddPoint = true;
                        if (oInfo.Status != (int)AppEnum.PriceReportStatus.Origin && oInfo.Status != (int)AppEnum.PriceReportStatus.AuditWaitingChangePrice)
                            throw new BizException("非初始状态，不能审核并修改价格");
                    }

                    new PriceReportDac().Update(paramHash);

                    if (NeedAddPoint)
                    {
                        oInfo = Load(SysNo);
                        CustomerPointRequestInfo cprInfo = new CustomerPointRequestInfo();
                        cprInfo.CustomerSysNo = oInfo.CustomerSysNo;
                        cprInfo.PointSourceType = (int)AppEnum.PointSourceType.PriceReport;
                        cprInfo.PointSourceSysNo = SysNo;
                        cprInfo.PointLogType = (int)AppEnum.PointLogType.award;
                        cprInfo.PointAmount = oInfo.Point;
                        cprInfo.Memo = oInfo.AuditMemo;
                        cprInfo.RequestTime = DateTime.Now;
                        cprInfo.RequestUserSysNo = oInfo.AuditUserSysNo;
                        cprInfo.RequestUserType = (int)AppEnum.CreateUserType.Employee;
                        cprInfo.Status = (int)AppEnum.PointRequestStatus.Origin;

                        PointManager.GetInstance().InsertCustomerPointRequest(cprInfo);
                    }
                }
                else
                {
                    new PriceReportDac().Update(paramHash);
                }

                scope.Complete();
            }
        }

        public DataSet GetPriceReportDs(Hashtable paramHash)
        {
            string sql = @"select pr.*,p.productID,p.productName,pm.userName as PMUserName,su.UserName as auditUserName,
                           c.customerName,pp.currentprice as newCurrentPrice
                           from price_report pr 
                           inner join product p on pr.productsysno = p.sysNo 
                           inner join product_price pp on p.sysno = pp.productsysno 
                           inner join category3 c3 on p.c3sysno=c3.sysno 
                           inner join category2 c2 on c3.c2sysno=c2.sysno 
                           inner join category1 c1 on c2.c1sysno=c1.sysno 
                           inner join sys_user pm on p.pmusersysno=pm.sysno 
                           inner join customer c on c.sysno=pr.customersysno 
                           left join sys_user su on pr.auditusersysno=su.sysno 
                           where 1=1";
            if (paramHash != null && paramHash.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string key in paramHash.Keys)
                {
                    object item = paramHash[key];
                    sb.Append(" and ");
                    if (key == "DateFrom")
                    {
                        sb.Append("pr.ReportTime >=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "DateTo")
                    {
                        sb.Append("pr.ReportTime <").Append(Util.ToSqlEndDate(item.ToString()));
                    }                    
                    else if (key == "PMUserSysNo")
                    {
                        sb.Append("pm.sysno=" + item.ToString());
                    }
                    else if (key == "APMUserSysNo")
                    {
                        sb.Append("p.apmusersysno=" + item.ToString());
                    }
                    else if (key == "C1SysNo")
                    {
                        sb.Append("c1.sysno=" + item.ToString());
                    }
                    else if (key == "C2SysNo")
                    {
                        sb.Append("c2.sysno=" + item.ToString());
                    }
                    else if (key == "C3SysNo")
                    {
                        sb.Append("c3.sysno=" + item.ToString());
                    }
                    else if (key == "ProductSysNo")
                    {
                        sb.Append("pr.productsysno=" + item.ToString());
                    }
                    else if (key == "CompetitorSysNo")
                    {
                        sb.Append("pr.CompetitorSysNo=" + item.ToString());
                    }
                    else if (key == "PriceReportStatus")
                    {
                        sb.Append("pr.Status=" + item.ToString());
                    }
                    else if (key == "Reason")
                    {
                        sb.Append("pr.Reason=" + item.ToString());
                    }
                    else if (key == "HandleType")
                    {
                        sb.Append("pr.HandleType" + item.ToString());
                    }
                    else if (key == "sysno")
                    {
                        sb.Append("pr.sysno=" + item.ToString());
                    }
                    else if (item is int)
                    {
                        sb.Append(key).Append("=").Append(item.ToString());
                    }
                    else if (item is string)
                    {
                        sb.Append(key).Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                    }
                    else if (key == "CustomerSysNo")
                    {
                        sb.Append("pr.CustomerSysNo = " + item.ToString());
                    }
                }
                sql += sb.ToString() + " order by pr.sysno desc";
            }
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                return ds;
            else
                return null;
        }
    }
}