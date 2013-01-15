using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Sale
{
    public class SRItemInfo
    {
        public SRItemInfo()
        {
            Init();
        }
        public int SysNo;
        public int SRSysNo;
        public int ProductSysNo;
        public int Quantity;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            SRSysNo = AppConst.IntNull;
            ProductSysNo = AppConst.IntNull;
            Quantity = AppConst.IntNull;
        }
    }
}
