using System;
using System.Collections.Generic;
using System.Text;

using Icson.Utils;

namespace Icson.Objects.RMA
{
    public class RMARevertItemProductIDInfo
    {
        public int SysNo;
        public int RevertItemSysNo;
        public int ProductIDSysNo;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            RevertItemSysNo = AppConst.IntNull;
            ProductIDSysNo = AppConst.IntNull;
        }
    }
}