using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Solution
{
    public class SlnMasterInfo : IComparable
    {
        public SlnMasterInfo()
		{
			Init();
		}

        public int SysNo;
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
            SlnMasterInfo b = obj as SlnMasterInfo;
            if (this.OrderNum > b.OrderNum)
                return 1;
            else
                return -1;
        }
        #endregion
    }   
}
