using System;

using Icson.Utils;

namespace Icson.Objects.Finance
{
    /// <summary>
    /// Summary description for POPayItemInfo.
    /// </summary>
    public class POPayItemInfo
    {
        public POPayItemInfo()
        {
            Init();
        }
        public int SysNo;
        public int POSysNo;
        public int PayStyle;
        public decimal PayAmt;
        public DateTime CreateTime;
        public int CreateUserSysNo;
        public DateTime EstimatePayTime;
        public string ReferenceID;
        public DateTime PayTime;
        public int PayUserSysNo;
        public string Note;
        public int Status;
        public int IsPrintPOPayBill;
        public int RequestUserSysNo;
        public DateTime RequestTime;
        public int AuditUserSysNo;
        public DateTime AuditTime;
        public DateTime VoucherTime;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            POSysNo = AppConst.IntNull;
            PayStyle = AppConst.IntNull;
            PayAmt = AppConst.DecimalNull;
            CreateTime = AppConst.DateTimeNull;
            CreateUserSysNo = AppConst.IntNull;
            EstimatePayTime = AppConst.DateTimeNull;
            ReferenceID = AppConst.StringNull;
            PayTime = AppConst.DateTimeNull;
            PayUserSysNo = AppConst.IntNull;
            Note = AppConst.StringNull;
            Status = AppConst.IntNull;
            IsPrintPOPayBill = AppConst.IntNull;
            RequestUserSysNo = AppConst.IntNull;
            RequestTime = AppConst.DateTimeNull;
            AuditUserSysNo = AppConst.IntNull;
            AuditTime = AppConst.DateTimeNull;
            VoucherTime = AppConst.DateTimeNull;
        }

    }
}
