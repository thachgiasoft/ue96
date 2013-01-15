using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Solution
{
    public class PrjItemInfo : IComparable
    {
        public PrjItemInfo()
        {
            Init();
        }

        public int SysNo;
        public int PrjSysNo;
        public int SlnItemSysNo;
        public int DefaultProductSysNo;
        public int DefaultQty;
        public int IsShowPic;
        public int Status;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            PrjSysNo = AppConst.IntNull;
            SlnItemSysNo = AppConst.IntNull;
            DefaultProductSysNo = AppConst.IntNull;
            DefaultQty = AppConst.IntNull;
            IsShowPic = AppConst.IntNull;
            Status = AppConst.IntNull;
        }

        #region IComparable Members
        public int CompareTo(object obj)
        {
            PrjItemInfo b = obj as PrjItemInfo;
            if (this.SysNo > b.SysNo)
                return 1;
            else
                return -1;
        }
        #endregion
    }
}
