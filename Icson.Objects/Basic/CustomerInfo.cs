using System;
using Icson.Utils;

namespace Icson.Objects.Basic
{
    /// <summary>
    /// Summary description for CustomerInfo.
    /// </summary>
    public class CustomerInfo
    {
        public CustomerInfo()
        {
            Init();
        }

        public int SysNo;
        public string CustomerID;
        public string Pwd;
        public int Status;
        public string CustomerName;
        public int Gender;
        public string Email;
        public string Phone;
        public string CellPhone;
        public string Fax;
        public int DwellAreaSysNo;
        public string DwellAddress;
        public string DwellZip;
        public string ReceiveName;
        public string ReceiveContact;
        public string ReceivePhone;
        public string ReceiveCellPhone;
        public string ReceiveFax;
        public int ReceiveAreaSysNo;
        public string ReceiveAddress;
        public string ReceiveZip;
        public int TotalScore;
        public int ValidScore;
        public string CardNo;
        public string Note;
        public int EmailStatus;
        public DateTime RegisterTime;
        public string LastLoginIP;
        public DateTime LastLoginTime;
        public int CustomerRank;
        public int IsManualRank;
        public int CustomerType;
        public decimal TotalFreeShipFee;
        public decimal ValidFreeShipFee;
        public int RefCustomerSysNo;
        public int VIPRank;
        public int DiscountPercent;
        public string BirthDay;
        public string NickName;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            CustomerID = AppConst.StringNull;
            Pwd = AppConst.StringNull;
            Status = AppConst.IntNull;
            CustomerName = AppConst.StringNull;
            Gender = AppConst.IntNull;
            Email = AppConst.StringNull;
            Phone = AppConst.StringNull;
            CellPhone = AppConst.StringNull;
            Fax = AppConst.StringNull;
            DwellAreaSysNo = AppConst.IntNull;
            DwellAddress = AppConst.StringNull;
            DwellZip = AppConst.StringNull;
            ReceiveName = AppConst.StringNull;
            ReceiveContact = AppConst.StringNull;
            ReceivePhone = AppConst.StringNull;
            ReceiveCellPhone = AppConst.StringNull;
            ReceiveFax = AppConst.StringNull;
            ReceiveAreaSysNo = AppConst.IntNull;
            ReceiveAddress = AppConst.StringNull;
            ReceiveZip = AppConst.StringNull;
            TotalScore = AppConst.IntNull;
            ValidScore = AppConst.IntNull;
            CardNo = AppConst.StringNull;
            Note = AppConst.StringNull;
            EmailStatus = AppConst.IntNull;
            RegisterTime = AppConst.DateTimeNull;
            LastLoginIP = AppConst.StringNull;
            LastLoginTime = AppConst.DateTimeNull;
            CustomerRank = AppConst.IntNull;
            IsManualRank = AppConst.IntNull;
            CustomerType = AppConst.IntNull;
            TotalFreeShipFee = AppConst.DecimalNull;
            ValidFreeShipFee = AppConst.DecimalNull;
            RefCustomerSysNo = AppConst.IntNull;
            VIPRank = AppConst.IntNull;
            DiscountPercent = AppConst.IntNull;
        }
    }
}