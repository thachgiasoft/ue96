using System;
using System.Collections;

using Icson.Utils;
namespace Icson.Objects.Finance
{
    public class POPayItemErrRequestInfo
    {
        public POPayItemErrRequestInfo()
        {
            Init();
        }

        public int SysNo;
        public int POPayItemSysNo;
        public int RequestUserSysNo;
        public DateTime RequestTime;
        public string RequestUserNote;
        public string ErrMsgNote;
        public int TLAuditUserSysNo;
        public DateTime TLAuditTime;
        public string TLNote;
        public int LastAuditUserSysNo;
        public DateTime LastAuditTime;
        public string LastAuditNote;
        public int Status;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            POPayItemSysNo = AppConst.IntNull;
            RequestUserSysNo = AppConst.IntNull;
            RequestTime = AppConst.DateTimeNull;
            RequestUserNote = AppConst.StringNull;
            ErrMsgNote = AppConst.StringNull;
            TLAuditUserSysNo = AppConst.IntNull;
            TLAuditTime = AppConst.DateTimeNull;
            TLNote = AppConst.StringNull;
            LastAuditUserSysNo = AppConst.IntNull;
            LastAuditTime = AppConst.DateTimeNull;
            LastAuditNote = AppConst.StringNull;
            Status = AppConst.IntNull;
        }
    }
}
