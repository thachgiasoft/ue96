using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Basic
{
    public class PromotionItemProductInfo
    {

        public PromotionItemProductInfo()
        {
            Init();
        }

        public int SysNo;
        public int PromotionItemSysNo;
        public int ProductSysNo;
        public int ProductOrder;
        public int Status;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            PromotionItemSysNo = AppConst.IntNull;
            ProductSysNo = AppConst.IntNull;
            ProductOrder = AppConst.IntNull;
            Status = AppConst.IntNull;
        }
    }
}
