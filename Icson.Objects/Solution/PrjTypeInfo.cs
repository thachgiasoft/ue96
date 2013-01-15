using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Solution
{
    public class PrjTypeInfo : IComparable
    {
        public PrjTypeInfo()
        {
            Init();
        }

        public int SysNo;
        public int SlnSysNo;
        public string ID;
        public string Name;
        public string Title;
        public string Description;
        public int OrderNum;
        public int Status;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            SlnSysNo = AppConst.IntNull;
            ID = AppConst.StringNull;
            Name = AppConst.StringNull;
            Title = AppConst.StringNull;
            Description = AppConst.StringNull;
            OrderNum = AppConst.IntNull;
            Status = AppConst.IntNull;
        }

        #region IComparable Members
        public int CompareTo(object obj)
        {
            PrjTypeInfo b = obj as PrjTypeInfo;
            if (this.OrderNum > b.OrderNum)
                return 1;
            else
                return -1;
        }
        #endregion
    }
}