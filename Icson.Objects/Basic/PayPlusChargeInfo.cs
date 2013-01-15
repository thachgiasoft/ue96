using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Basic
{
    public class PayPlusChargeInfo : IComparable
    {
        public PayPlusChargeInfo()
        {
            Init();
        }

        public int SysNo;
        public string BankName;
        public int InstallmentNum;
        public decimal PlusRate;
        public int Status;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            BankName = AppConst.StringNull;
            InstallmentNum = AppConst.IntNull;
            PlusRate = AppConst.DecimalNull;
            Status = AppConst.IntNull;
        }

        public int CompareTo(object obj)
        {
            PayPlusChargeInfo b = obj as PayPlusChargeInfo;
            if (string.Compare(this.BankName, b.BankName) >= 0)
                return 1;
            else
                return -1;
        }
    }
}
