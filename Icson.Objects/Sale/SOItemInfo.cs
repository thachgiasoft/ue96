using System;
using Icson.Utils;

namespace Icson.Objects.Sale
{
    /// <summary>
    /// Summary description for SOItemInfo.
    /// </summary>
    public class SOItemInfo
    {
        public SOItemInfo()
        {
            Init();
        }

        public int SysNo;
        public int SOSysNo;
        public int ProductSysNo;
        public int Quantity;
        public int AccountQty;
        public int Weight;
        public decimal OrderPrice;
        public decimal Price;
        public decimal Cost;
        public int Point;
        public int PointType;
        public decimal DiscountAmt;
        public string Warranty;
        public int ProductType; //主产品，赠品
        public int GiftSysNo;
        public int BaseProductType; //新品，二手，坏品，服务
        public int ExpectQty; //期望订购数量
        public int ReturnQty;//部分拒收的数量
        public int IsCanVat; //是否能开增票

        public void Init()
        {
            SysNo = AppConst.IntNull;
            SOSysNo = AppConst.IntNull;
            ProductSysNo = AppConst.IntNull;
            Quantity = AppConst.IntNull;
            Weight = AppConst.IntNull;
            OrderPrice = AppConst.DecimalNull;
            Price = AppConst.DecimalNull;
            Cost = AppConst.DecimalNull;
            Point = AppConst.IntNull;
            PointType = AppConst.IntNull;
            DiscountAmt = AppConst.DecimalNull;
            Warranty = AppConst.StringNull;
            ProductType = AppConst.IntNull;
            GiftSysNo = AppConst.IntNull;
            BaseProductType = AppConst.IntNull;
            ExpectQty = AppConst.IntNull;
            ReturnQty = AppConst.IntNull;
            IsCanVat = AppConst.IntNull;
        }
    }
}
