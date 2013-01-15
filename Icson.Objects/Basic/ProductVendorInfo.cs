using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Basic
{
    public class ProductVendorInfo
    {
        public ProductVendorInfo()
		{
			Init();
		}

        public int SysNo;
        public int ProductSysNo;
        public int VendorSysNo;
        public decimal PurchasePrice;
        public int IsDefault;
        public int UpdateUserSysNo;
        public DateTime UpdateTime;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            ProductSysNo = AppConst.IntNull;
            VendorSysNo = AppConst.IntNull;
            PurchasePrice = AppConst.DecimalNull;
            IsDefault = AppConst.IntNull;
            UpdateUserSysNo = AppConst.IntNull;
            UpdateTime = AppConst.DateTimeNull;
        }
    }
}
