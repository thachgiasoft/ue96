using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Promotion
{
    public class CouponRequestInfo
    {
        public CouponRequestInfo()
        {
            Init();
        }

        public int SysNo;
        public int CustomerSysNo;
        public string CouponCode;
        public int RequestUserSysNo;
        public DateTime RequestTime;
        public int AuditUserSysNo;
        public DateTime AuditTime;
        public int SOSysNo;
        public int BatchNo;
        public string Note;
        public int Status;
        public string EXECSql;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            CustomerSysNo = AppConst.IntNull;
            CouponCode = AppConst.StringNull;
            RequestUserSysNo = AppConst.IntNull;
            RequestTime = AppConst.DateTimeNull;
            AuditUserSysNo = AppConst.IntNull;
            AuditTime = AppConst.DateTimeNull;
            SOSysNo = AppConst.IntNull;
            BatchNo = AppConst.IntNull;
            Note = AppConst.StringNull;
            Status = AppConst.IntNull;
            EXECSql = AppConst.StringNull;
        }
    }
}
