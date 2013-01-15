using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Sale
{
    public class DSItemInfo
    {
        public int SysNo;
        public int DSSysNo;
        public string ItemID;
        public int ItemType;
        public int PayType;
        public decimal PayAmt;
        public int Status;
        public decimal PosFee;
        public int DLSysNo;
        public int IsPos;
        public string PosNo;
        public DateTime PosDate;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            DSSysNo = AppConst.IntNull;
            ItemID = AppConst.StringNull;
            ItemType = AppConst.IntNull;
            PayType = AppConst.IntNull;
            PayAmt = AppConst.DecimalNull;
            Status = AppConst.IntNull;
            PosFee = AppConst.DecimalNull;
            DLSysNo = AppConst.IntNull;
            IsPos = AppConst.IntNull;
            PosNo = AppConst.StringNull;
            PosDate = AppConst.DateTimeNull;
        }
    }
}
