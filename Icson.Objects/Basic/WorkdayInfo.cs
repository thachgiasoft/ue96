using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Basic
{
    public class WorkdayInfo : IComparable
    {
        public int SysNo;
        public string Name;
        public DateTime Date;
        public int TimeSpan;
        public int Week;
        public int Status;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            Name = AppConst.StringNull;
            Date = AppConst.DateTimeNull;
            TimeSpan = AppConst.IntNull;
            Week = AppConst.IntNull;
            Status = AppConst.IntNull;
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            WorkdayInfo b = obj as WorkdayInfo;
            if (this.SysNo > b.SysNo)
                return 1;
            else
                return -1;
        }
        #endregion
    }
}