using System;
using System.Collections.Generic;
using System.Text;

using Icson.Utils;

namespace Icson.Objects.Stock
{
    public class ShiftItemProductIDInfo
    {
        public int SysNo;
        public int StShiftItemSysNo;
        public int ProductIDSysNo;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            StShiftItemSysNo = AppConst.IntNull;
            ProductIDSysNo = AppConst.IntNull;
        }
    }
}