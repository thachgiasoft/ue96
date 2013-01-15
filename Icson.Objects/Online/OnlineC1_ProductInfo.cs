using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Online
{
    public class OnlineC1_ProductInfo : IComparable
    {
        public OnlineC1_ProductInfo()
        {
            Init();
        }
        public int ProductSysNo;
        public string ProductBriefName;
        public int OrderNum;

        public void Init()
        {
            ProductSysNo = AppConst.IntNull;
            ProductBriefName = AppConst.StringNull;
            OrderNum = AppConst.IntNull;
        }

        #region IComparable Members
        public int CompareTo(object obj)
        {
            OnlineC1_ProductInfo b = obj as OnlineC1_ProductInfo;
            if (this.OrderNum > b.OrderNum)
                return 1;
            else
                return -1;
        }
        #endregion
    }
}
