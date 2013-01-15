using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Sale
{
    public class AbnormalSOInfo
    {
        public AbnormalSOInfo()
        {
            Init();
        }
        public int SysNo;
        public int SOSysNo;
        public string Description;
        public int Status;
        public int Type;
        public DateTime Createtime;
        public int CreateUserSysNo;
        public DateTime UpdateTime;
        public int UpdateUserSysNo;
        public DateTime CloseTime;
        public int CloseUserSysNo;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            SOSysNo = AppConst.IntNull;
            Description = AppConst.StringNull;
            Status = AppConst.IntNull;
            Type = AppConst.IntNull;
            Createtime = AppConst.DateTimeNull;
            CreateUserSysNo = AppConst.IntNull;
            UpdateTime = AppConst.DateTimeNull;
            UpdateUserSysNo = AppConst.IntNull;
            CloseTime = AppConst.DateTimeNull;
            CloseUserSysNo = AppConst.IntNull;
        }
    }
}
