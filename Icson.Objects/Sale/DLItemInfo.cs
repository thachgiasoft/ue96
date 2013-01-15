using System;
using System.Collections.Generic;
using System.Text;

using Icson.Utils;

namespace Icson.Objects.Sale
{
    public class DLItemInfo
    {
        public int SysNo;
        public int DLSysNo;
        public string ItemID;
        public int ItemType;
        public int PayType;
        public decimal PayAmt;
        public int Status;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            DLSysNo = AppConst.IntNull;
            ItemID = AppConst.StringNull;
            ItemType = AppConst.IntNull;
            PayType = AppConst.IntNull;
            PayAmt = AppConst.DecimalNull;
            Status = AppConst.IntNull;
        }
    }
}