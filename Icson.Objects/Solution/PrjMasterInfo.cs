using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Solution
{
    public class PrjMasterInfo : IComparable
    {
        public PrjMasterInfo()
        {
            Init();
        }

        public int SysNo;
        public int SlnSysNo;
        public int PrjTypeSysNo;
        public string ID;
        public string Name;
        public string Title;
        public string Description;
        public int SysUserSysNo;
        public DateTime DateStamp;
        public int OrderNum;
        public int Status;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            SlnSysNo = AppConst.IntNull;
            PrjTypeSysNo = AppConst.IntNull;
            ID = AppConst.StringNull;
            Name = AppConst.StringNull;
            Title = AppConst.StringNull;
            Description = AppConst.StringNull;
            SysUserSysNo = AppConst.IntNull;
            DateStamp = AppConst.DateTimeNull;
            OrderNum = AppConst.IntNull;
            Status = AppConst.IntNull;
        }

        #region IComparable Members
        public int CompareTo(object obj)
        {
            PrjMasterInfo b = obj as PrjMasterInfo;
            if (this.OrderNum > b.OrderNum)
                return 1;
            else
                return -1;
        }
        #endregion
    }
}
