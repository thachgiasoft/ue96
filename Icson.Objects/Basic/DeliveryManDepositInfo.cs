using System;
using System.Collections.Generic;
using System.Text;

using Icson.Utils;

namespace Icson.Objects.Basic
{
    public class DeliveryManDepositInfo
    {
        public DeliveryManDepositInfo()
        {
            Init();
        }

        public int SysNo;
        public int UserSysNo;
        public decimal Deposit;
        public decimal Arrearage;
        public DateTime PayDate;
        public int IsAllow;
        public int CreateUserSysNo;
        public DateTime CreateTime;
        public int UpdateUserSysNo;
        public DateTime UpdateTime;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            UserSysNo = AppConst.IntNull;
            Deposit = AppConst.DecimalNull;
            Arrearage = AppConst.DecimalNull;
            PayDate = AppConst.DateTimeNull;
            IsAllow = AppConst.IntNull;
            CreateUserSysNo = AppConst.IntNull;
            CreateTime = AppConst.DateTimeNull;
            UpdateUserSysNo = AppConst.IntNull;
            UpdateTime = AppConst.DateTimeNull;
        }
    }
}
