using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Sale
{
    public class SOAlipayInfo
    {
        public int SysNo;
        public int SOSysNo;
        public string AlipayTradeNo;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            SOSysNo = AppConst.IntNull;
            AlipayTradeNo = AppConst.StringNull;
        }
    }
}
