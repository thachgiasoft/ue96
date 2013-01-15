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
        public int ProductType; //����Ʒ����Ʒ
        public int GiftSysNo;
        public int BaseProductType; //��Ʒ�����֣���Ʒ������
        public int ExpectQty; //������������
        public int ReturnQty;//���־��յ�����
        public int IsCanVat; //�Ƿ��ܿ���Ʊ

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
