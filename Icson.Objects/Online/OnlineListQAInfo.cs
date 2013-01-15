using System;
using System.Collections.Generic;
using System.Text;

using Icson.Utils;

namespace Icson.Objects.Online
{
    public class OnlineListQAInfo : IComparable
    {
        public OnlineListQAInfo()
        {
            Init();
        }

        public int SysNo;
        public int OnlineAreaType;
        public int CategorySysNo;
        public int QAType;
        public int QASysNo;
        public int CreateUserSysNo;
        public DateTime CreateTime;
        public int ListOrder;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            OnlineAreaType = AppConst.IntNull;
            CategorySysNo = AppConst.IntNull;
            QAType = AppConst.IntNull;
            QASysNo = AppConst.IntNull;
            CreateUserSysNo = AppConst.IntNull;
            CreateTime = AppConst.DateTimeNull;
            ListOrder = AppConst.IntNull;
        }

        #region IComparable Members
        public int CompareTo(object obj)
        {
            OnlineListQAInfo b = obj as OnlineListQAInfo;
            if (this.ListOrder > b.ListOrder)
                return 1;
            else
                return -1;
        }
        #endregion
    }
}
