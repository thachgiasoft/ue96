using System;
using System.Collections.Generic;
using System.Text;

using Icson.Utils;

namespace Icson.Objects.Basic
{
    public class PriceReportInfo
    {
        public PriceReportInfo()
        {
            Init();
        }

        public int SysNo;
        public int ProductSysNo;
        public decimal CurrentPrice;
        public decimal UnitCost;
        public decimal LastOrderPrice;
        public int CompetitorSysNo;
        public decimal CompetitorPrice;
        public string CompetitorUrl;
        public int CustomerSysNo;
        public string NickName;
        public string Email;
        public string CustomerMemo;
        public DateTime ReportTime;
        public string CustomerIP;
        public int AuditUserSysNo;
        public string AuditMemo;
        public string AuditNote;
        public DateTime AuditTime;
        public int AbandonUserSysNo;
        public DateTime AbandonTime;
        public int Point;
        public int HandleType;
        public int Reason;
        public int Status;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            ProductSysNo = AppConst.IntNull;
            CurrentPrice = AppConst.DecimalNull;
            UnitCost = AppConst.DecimalNull;
            LastOrderPrice = AppConst.DecimalNull;
            CompetitorSysNo = AppConst.IntNull;
            CompetitorPrice = AppConst.DecimalNull;
            CompetitorUrl = AppConst.StringNull;
            CustomerSysNo = AppConst.IntNull;
            NickName = AppConst.StringNull;
            Email = AppConst.StringNull;
            CustomerMemo = AppConst.StringNull;
            ReportTime = AppConst.DateTimeNull;
            CustomerIP = AppConst.StringNull;
            AuditUserSysNo = AppConst.IntNull;
            AuditMemo = AppConst.StringNull;
            AuditNote = AppConst.StringNull;
            AuditTime = AppConst.DateTimeNull;
            AbandonUserSysNo = AppConst.IntNull;
            AbandonTime = AppConst.DateTimeNull;
            Point = AppConst.IntNull;
            HandleType = AppConst.IntNull;
            Reason = AppConst.IntNull;
            Status = AppConst.IntNull;
        }
    }
}