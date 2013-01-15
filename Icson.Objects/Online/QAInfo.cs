using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Online
{
    public class QAInfo : IComparable
    {
        public QAInfo()
        {
            Init();
        }

        public int SysNo;
        public string Question;
        public string Answer;
        public string SearchKey;
        public int Type;
        public int CreateUserSysNo;
        public DateTime CreateTime;
        public int ViewCount;
        public int OrderNum;
        public int Status;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            Question = AppConst.StringNull;
            Answer = AppConst.StringNull;
            SearchKey = AppConst.StringNull;
            Type = AppConst.IntNull;
            CreateUserSysNo = AppConst.IntNull;
            CreateTime = AppConst.DateTimeNull;
            ViewCount = AppConst.IntNull;
            OrderNum = AppConst.IntNull;
            Status = AppConst.IntNull;
        }

        #region IComparable Members
        public int CompareTo(object obj)
        {
            QAInfo b = obj as QAInfo;
            if (this.OrderNum > b.OrderNum)
                return 1;
            else
                return -1;
        }
        #endregion
    }
}