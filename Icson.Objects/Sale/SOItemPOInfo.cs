using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Sale
{
    public class SOItemPOInfo
    {
        public SOItemPOInfo()
        {
            Init();
        }

        public int SysNo;
        public int SOItemSysNo;
        public int POSysNo;
        public int ProductIDSysNo;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            SOItemSysNo = AppConst.IntNull;
            POSysNo = AppConst.IntNull;
            ProductIDSysNo = AppConst.IntNull;
        }
    }
}