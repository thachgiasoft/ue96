using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Sale
{
    public class DeliveryManArrearageLogInfo
    {
        public DeliveryManArrearageLogInfo()
        {
            Init();
        }
        public int SysNo;
        public int UserSysNo;
        public int DSSysNo;
        public decimal Arrearage;
        public int ArrearageLogType;
        public decimal ArrearageChange;
        public string Memo;
        public DateTime CreateTime;
        public int CreateUserSysNo;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            UserSysNo = AppConst.IntNull;
            DSSysNo = AppConst.IntNull;
            Arrearage = AppConst.DecimalNull;
            ArrearageLogType = AppConst.IntNull;
            ArrearageChange = AppConst.DecimalNull;
            Memo = AppConst.StringNull;
            CreateTime = AppConst.DateTimeNull;
            CreateUserSysNo = AppConst.IntNull;
        }
    }
}
