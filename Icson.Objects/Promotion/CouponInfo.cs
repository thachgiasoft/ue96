using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Promotion
{
    public class CouponInfo
    {
        public CouponInfo()
        {
            Init();
        }

        public int SysNo;
        public string CouponID;
        public string CouponName;
        public string CouponCode;
        public decimal CouponAmt;
        public decimal SaleAmt;
        public int CouponType;
        public DateTime ValidTimeFrom;
        public DateTime ValidTimeTo;
        public int MaxUseDegree;
        public int UsedDegree;
        public int CreateUserSysNo;
        public DateTime CreateTime;
        public int AuditUserSysNo;
        public DateTime AuditTime;
        public DateTime UsedTime;
        public int BatchNo;
        public int Status;
        public string CategorySysNoCom;
        public string ProductSysNoCom;
        public string ManufactorySysNoCom;
        public string UseAreaSysNoCom;
        public int UseCustomerSysNo;
        public string UseCustomerGradeCom;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            CouponID = AppConst.StringNull;
            CouponName = AppConst.StringNull;
            CouponCode = AppConst.StringNull;
            CouponAmt = AppConst.DecimalNull;
            SaleAmt = AppConst.DecimalNull;
            CouponType = AppConst.IntNull;
            ValidTimeFrom = AppConst.DateTimeNull;
            ValidTimeTo = AppConst.DateTimeNull;
            MaxUseDegree = AppConst.IntNull;
            UsedDegree = AppConst.IntNull;
            CreateUserSysNo = AppConst.IntNull;
            CreateTime = AppConst.DateTimeNull;
            AuditUserSysNo = AppConst.IntNull;
            AuditTime = AppConst.DateTimeNull;
            UsedTime = AppConst.DateTimeNull;
            BatchNo = AppConst.IntNull;
            Status = AppConst.IntNull;
            CategorySysNoCom = AppConst.StringNull;
            ProductSysNoCom = AppConst.StringNull;
            ManufactorySysNoCom = AppConst.StringNull;
            UseAreaSysNoCom = AppConst.StringNull;
            UseCustomerSysNo = AppConst.IntNull;
            UseCustomerGradeCom = AppConst.StringNull;
        }
    }
}
