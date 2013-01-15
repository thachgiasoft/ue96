using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Sale
{
    public class SOVoucherInfo
    {
        public int SysNo;
        public int SOSysNo;
        public string VoucherID;
        public int Status;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            SOSysNo = AppConst.IntNull;
            VoucherID = AppConst.StringNull;
            Status = AppConst.IntNull;
        }
    }
}