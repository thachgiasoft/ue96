using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Online
{
    public class TopicRemarkInfo
    {
        public TopicRemarkInfo()
        {
            Init();
        }
        public int SysNo;
        public int TopicSysNo;
        public bool IsUseful;
        public AppEnum.CreateUserType CreateUserType;
        public int CreateUserSysNo;
        public DateTime CreateDate;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            TopicSysNo = AppConst.IntNull;
            CreateUserSysNo = AppConst.IntNull;
            CreateDate = AppConst.DateTimeNull;
        }
    }
}