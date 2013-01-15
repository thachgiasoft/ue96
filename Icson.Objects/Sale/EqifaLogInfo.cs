using System;
using System.Collections.Generic;
using System.Text;

using Icson.Utils;

namespace Icson.Objects.Sale
{
    public class EqifaLogInfo
    {
        public int SysNo;
        public int SOSysNo;
        public int ProductSysNo;
        public int Quantity;
        public decimal Price;
        public string EqifaLog;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            SOSysNo = AppConst.IntNull;
            ProductSysNo = AppConst.IntNull;
            Quantity = AppConst.IntNull;
            Price = AppConst.DecimalNull;
            EqifaLog = AppConst.StringNull;
        }
    }
}