using System;
using System.Collections;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Sale
{
    public class SRInfo
    {
        public SRInfo()
        {
            Init();
        }

        public int SysNo;
        public string SRID;
        public int SOSysNo;
        public int Status;
        public int ReturnType;
        public int StockSysNo;
        public int CreateUserSysNo;
        public DateTime CreateTime;
        public int ReceiveUserSysNo;
        public DateTime ReceiveTime;
        public DateTime InstockTime;
        public int InstockUserSysNo;
        public DateTime ShelveTime;
        public int ShelveUserSysNo;
        public int UpdateUserSysNo;
        public DateTime UpdateTime;
        public string Note;

        public Hashtable ItemHash = new Hashtable();

        public void Init()
        {
            SysNo = AppConst.IntNull;
            SRID = AppConst.StringNull;
            SOSysNo = AppConst.IntNull;
            Status = AppConst.IntNull;
            ReturnType = AppConst.IntNull;
            StockSysNo = AppConst.IntNull;
            CreateUserSysNo = AppConst.IntNull;
            CreateTime = AppConst.DateTimeNull;
            ReceiveUserSysNo = AppConst.IntNull;
            ReceiveTime = AppConst.DateTimeNull;
            InstockTime = AppConst.DateTimeNull;
            InstockUserSysNo = AppConst.IntNull;
            ShelveTime = AppConst.DateTimeNull;
            ShelveUserSysNo = AppConst.IntNull;
            UpdateUserSysNo = AppConst.IntNull;
            UpdateTime = AppConst.DateTimeNull;
            Note = AppConst.StringNull;
        }
    }
}
