using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Basic
{
    public class CustomerCommendInfo
    {
        public int SysNo;
        public int CustomerSysNo;
        public string FriendName;
        public string CommendEmail;
        public DateTime CommendTime;
        public int CommendStatus;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            CustomerSysNo = AppConst.IntNull;
            FriendName = AppConst.StringNull;
            CommendEmail = AppConst.StringNull;
            CommendTime = AppConst.DateTimeNull;
            CommendStatus = AppConst.IntNull;
        }
    }
}
