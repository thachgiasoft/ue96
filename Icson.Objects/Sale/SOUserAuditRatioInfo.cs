using System;
using System.Collections.Generic;
using System.Text;

using Icson.Utils;

namespace Icson.Objects.Sale
{
    public class SOUserAuditRatioInfo
    {
        public int SysNo;
        public int UserSysNo;
        public int Ratio;
        public int AuditTimeSpan;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            UserSysNo = AppConst.IntNull;
            Ratio = AppConst.IntNull;
            AuditTimeSpan = AppConst.IntNull;
        }
    }
}