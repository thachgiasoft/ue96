using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;
namespace Icson.Objects.Promotion
{
   public class PromotionItemProductInfo
    {
        public int SysNo;
        public int PromotionItemGroupSysNo;
        public int ProductSysNo;
        public decimal PromotionDiscount;
        public int OrderNum;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            PromotionItemGroupSysNo = AppConst.IntNull;
            ProductSysNo = AppConst.IntNull;
            PromotionDiscount = AppConst.DecimalNull;
            OrderNum = AppConst.IntNull;
        }
    }
}
