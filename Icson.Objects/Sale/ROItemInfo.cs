using System;
using Icson.Utils;

namespace Icson.Objects.Sale
{
	/// <summary>
	/// Summary description for ROItemInfo.
	/// </summary>
	public class ROItemInfo
	{
        public ROItemInfo()
        {
            Init();
        }

        public int SysNo;
        public int ROSysNo;
        public int ProductSysNo;
        public int ReturnPriceType;
        public int ReturnType;
        public int ReturnSysNo;
        public int Quantity;
        public decimal Price;
        public decimal Cost;
        public int Weight;
        public int Point;
        public decimal RefundCash;
        public int RefundPoint;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            ROSysNo = AppConst.IntNull;
            ProductSysNo = AppConst.IntNull;
            ReturnPriceType = AppConst.IntNull;
            ReturnType = AppConst.IntNull;
            ReturnSysNo = AppConst.IntNull;
            Quantity = AppConst.IntNull;
            Price = AppConst.DecimalNull;
            Cost = AppConst.DecimalNull;
            Weight = AppConst.IntNull;
            Point = AppConst.IntNull;
            RefundCash = AppConst.DecimalNull;
            RefundPoint = AppConst.IntNull;
        }
	}
}
