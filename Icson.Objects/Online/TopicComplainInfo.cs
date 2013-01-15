using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Online
{
    public class TopicComplainInfo
    {
        public TopicComplainInfo()
        {
            Init();
        }
        public int SysNo;
        public int TopicSysNo;
        public string Memo;
        public AppEnum.CreateUserType CreateUserType;
        public int CreateUserSysNo;
        public DateTime CreateDate;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            TopicSysNo = AppConst.IntNull;
            Memo = AppConst.StringNull;
            CreateUserSysNo = AppConst.IntNull;
            CreateDate = AppConst.DateTimeNull;
        }
    }
}
