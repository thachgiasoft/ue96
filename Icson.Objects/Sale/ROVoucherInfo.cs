using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Sale
{
    public class ROVoucherInfo
    {
        public int SysNo;
        public int ROSysNo;
        public string VoucherID;
        public int Status;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            ROSysNo = AppConst.IntNull;
            VoucherID = AppConst.StringNull;
            Status = AppConst.IntNull;
        }
    }
}