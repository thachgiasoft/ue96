using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Online
{
    public class WebBulletinInfo : IComparable
    {
        public WebBulletinInfo()
        {
            Init();
        }

        public int SysNo;
        public string Title;
        public string Content;
        public DateTime CreateDate;
        public int OrderNum;
        public int Status;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            Title = AppConst.StringNull;
            Content = AppConst.StringNull;
            CreateDate = AppConst.DateTimeNull;
            OrderNum = AppConst.IntNull;
            Status = AppConst.IntNull;
        }

        #region IComparable Members
        public int CompareTo(object obj)
        {
            WebBulletinInfo b = obj as WebBulletinInfo;
            if (this.OrderNum > b.OrderNum)
                return 1;
            else
                return -1;
        }
        #endregion
    }
}
