using System;
using Icson.Utils;

namespace Icson.Objects.Basic
{
    public class CustomerVATInfo
    {
        public CustomerVATInfo()
        {
            Init();
        }

        public int SysNo;
        public int CustomerSysNo;
        public string CompanyName;
        public string TaxNum;
        public string CompanyAddress;
        public string CompanyPhone;
        public string BankInfo;
        public string BankAccount;
        public string Image1;
        public string Image2;
        public string Image3;
        public string Image4;
        public string Memo;
        public DateTime CreateTime;
        public int IsDefault;
        public int Status;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            CustomerSysNo = AppConst.IntNull;
            CompanyName = AppConst.StringNull;
            TaxNum = AppConst.StringNull;
            CompanyAddress = AppConst.StringNull;
            CompanyPhone = AppConst.StringNull;
            BankInfo = AppConst.StringNull;
            BankAccount = AppConst.StringNull;
            Image1 = AppConst.StringNull;
            Image2 = AppConst.StringNull;
            Image3 = AppConst.StringNull;
            Image4 = AppConst.StringNull;
            Memo = AppConst.StringNull;
            CreateTime = AppConst.DateTimeNull;
            IsDefault = AppConst.IntNull;
            Status = AppConst.IntNull;
        }
    }
}
