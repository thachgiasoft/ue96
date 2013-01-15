using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Basic
{
    public class ProductPriceMarketInfo
    {
        public ProductPriceMarketInfo()
        {
            Init();
        }

        public int SysNo;
        public int ProductSysNo;
        public decimal MarketLowestPrice;
        public string MarketUrl;
        public string CreateMemo;
        public int CreateUserSysNo;
        public DateTime CreateTime;
        public string AuditMemo;
        public int AuditUserSysNo;
        public DateTime AuditTime;
        public int Status;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            ProductSysNo = AppConst.IntNull;
            MarketLowestPrice = AppConst.DecimalNull;
            MarketUrl = AppConst.StringNull;
            CreateMemo = AppConst.StringNull;
            CreateUserSysNo = AppConst.IntNull;
            CreateTime = AppConst.DateTimeNull;
            AuditMemo = AppConst.StringNull;
            AuditUserSysNo = AppConst.IntNull;
            AuditTime = AppConst.DateTimeNull;
            Status = AppConst.IntNull;
        }
    }
}
