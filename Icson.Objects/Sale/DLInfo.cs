using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

using Icson.Utils;

namespace Icson.Objects.Sale
{
    public class DLInfo
    {
        public DLInfo()
        {
            Init();
        }

        public Hashtable ItemHash = new Hashtable();

        public int SysNo;
        public int FreightUserSysNo;
        public int CreateUserSysNo;
        public DateTime CreateTime;
        public int ConfirmUserSysNo;
        public DateTime ConfirmTime;
        public int IncomeUserSysNo;
        public DateTime IncomeTime;
        public int UpdateFreightManUserSysNo;
        public DateTime UpdateFreightManTime;
        public int VoucherUserSysNo;
        public string VoucherID;
        public DateTime VoucherTime;
        public int HasPaidQty;
        public decimal HasPaidAmt;
        public int CODQty;
        public decimal CODAmt;
        public int Type;
        public int Status;
        public int IsSendSMS;
        public int IsAllow;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            FreightUserSysNo = AppConst.IntNull;
            CreateUserSysNo = AppConst.IntNull;
            CreateTime = AppConst.DateTimeNull;
            ConfirmUserSysNo = AppConst.IntNull;
            ConfirmTime = AppConst.DateTimeNull;
            IncomeUserSysNo = AppConst.IntNull;
            IncomeTime = AppConst.DateTimeNull;
            UpdateFreightManUserSysNo = AppConst.IntNull;
            UpdateFreightManTime = AppConst.DateTimeNull;
            VoucherUserSysNo = AppConst.IntNull;
            VoucherID = AppConst.StringNull;
            VoucherTime = AppConst.DateTimeNull;
            HasPaidQty = AppConst.IntNull;
            HasPaidAmt = AppConst.DecimalNull;
            CODQty = AppConst.IntNull;
            CODAmt = AppConst.DecimalNull;
            Type = AppConst.IntNull;
            Status = AppConst.IntNull;
            IsSendSMS = AppConst.IntNull;
            IsAllow = AppConst.IntNull;
        }
    }
}