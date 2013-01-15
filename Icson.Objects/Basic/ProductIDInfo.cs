using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Basic
{
    public class ProductIDInfo
    {
        public ProductIDInfo()
        {
            Init();
        }

        public int SysNo;
        public int ProductSysNo;
        public int POSysNo;
        public int OrderNum;
        public int Status;
        public string ProductSN;
        public string ProductTrackSN;
        public string Note;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            ProductSysNo = AppConst.IntNull;
            POSysNo = AppConst.IntNull;
            OrderNum = AppConst.IntNull;
            Status = AppConst.IntNull;
            ProductSN = AppConst.StringNull;
            ProductTrackSN = AppConst.StringNull;
            Note = AppConst.StringNull;
        }
    }
}