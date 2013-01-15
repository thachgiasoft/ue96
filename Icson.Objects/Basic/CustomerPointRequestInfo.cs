using System;
using System.Collections.Generic;
using System.Text;

using Icson.Utils;

namespace Icson.Objects.Basic
{
    public class CustomerPointRequestInfo
    {
        public CustomerPointRequestInfo()
		{
			Init();
		}

        public int SysNo;
        public int CustomerSysNo;
        public int PointSourceType;
        public int PointSourceSysNo;
        public int PointLogType;
        public int PointAmount;
        public int RequestUserType;
        public int RequestUserSysNo;
        public DateTime RequestTime;
        public int AuditUserSysNo;
        public DateTime AuditTime;
        public int AddUserSysNo;
        public DateTime AddTime;
        public int AbandonUserSysNo;
        public DateTime AbandonTime;
        public string Memo;
        public int Status;
        public int PMUserSysNo;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            CustomerSysNo = AppConst.IntNull;
            PointSourceType = AppConst.IntNull;
            PointSourceSysNo = AppConst.IntNull;
            PointLogType = AppConst.IntNull;
            PointAmount = AppConst.IntNull;
            RequestUserType = AppConst.IntNull;
            RequestUserSysNo = AppConst.IntNull;
            RequestTime = AppConst.DateTimeNull;
            AuditUserSysNo = AppConst.IntNull;
            AuditTime = AppConst.DateTimeNull;
            AddUserSysNo = AppConst.IntNull;
            AddTime = AppConst.DateTimeNull;
            AbandonUserSysNo = AppConst.IntNull;
            AbandonTime = AppConst.DateTimeNull;
            Memo = AppConst.StringNull;
            Status = AppConst.IntNull;
            PMUserSysNo = AppConst.IntNull;
        }
    }
}