using System;

using Icson.Utils;

namespace Icson.Objects.Basic
{
    /// <summary>
    /// Summary description for CategoryAttribute1Info.
    /// </summary>
    public class Category3ReviewItemInfo : IComparable
    {
        public Category3ReviewItemInfo()
        {
            Init();
        }

        public int SysNo;
        public int C3SysNo;
        public string ID;
        public string Name;
        public string Description;
        public int Weight;
        public int OrderNum;
        public int Status;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            C3SysNo = AppConst.IntNull;
            ID = AppConst.StringNull;
            Name = AppConst.StringNull;
            Description = AppConst.StringNull;
            Weight = AppConst.IntNull;
            OrderNum = AppConst.IntNull;
            Status = AppConst.IntNull;
        }
        #region IComparable Members

        public int CompareTo(object obj)
        {
            Category3ReviewItemInfo b = obj as Category3ReviewItemInfo;
            if (this.OrderNum > b.OrderNum)
                return 1;
            else
                return -1;
        }

        #endregion
    }
}
