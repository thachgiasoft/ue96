using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Online
{
    public class OnlineC1_C2Info : IComparable
    {
        public OnlineC1_C2Info()
        {
            Init();
        }

        public int C2SysNo;
        public int OrderNum;

        public void Init()
        {
            C2SysNo = AppConst.IntNull;
            OrderNum = AppConst.IntNull;
        }

        #region IComparable Members
        public int CompareTo(object obj)
        {
            OnlineC1_C2Info b = obj as OnlineC1_C2Info;
            if (this.OrderNum > b.OrderNum)
                return 1;
            else
                return -1;
        }
        #endregion
    }
}