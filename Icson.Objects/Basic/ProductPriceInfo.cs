using System;

using Icson.Utils;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// Summary description for ProductPriceInfo.
	/// </summary>
	public class ProductPriceInfo
	{
		public ProductPriceInfo()
		{
			Init();
		}		
		public int SysNo;
		public int ProductSysNo;
		public decimal BasicPrice;
		public decimal CurrentPrice;
		public decimal UnitCost;
		public decimal LastOrderPrice;
        public decimal LastMarketLowestPrice;
		public decimal Discount;
		public int PointType;
		public int IsWholeSale;
		public int Q1;
		public decimal P1;
		public int Q2;
		public decimal P2;
		public int Q3;
		public decimal P3;
		public decimal CashRebate;
		public int Point;
		public int ClearanceSale;
		public DateTime CreateTime;
        public int LimitedQty;
		
		public void Init()
		{
			SysNo = AppConst.IntNull;
			ProductSysNo = AppConst.IntNull;
			BasicPrice = AppConst.DecimalNull;
			CurrentPrice = AppConst.DecimalNull;
			UnitCost = AppConst.DecimalNull;
			LastOrderPrice = AppConst.DecimalNull;
            LastMarketLowestPrice = AppConst.DecimalNull;
			Discount = AppConst.DecimalNull;
			PointType = AppConst.IntNull;
			IsWholeSale = AppConst.IntNull;
			Q1 = AppConst.IntNull;
			P1 = AppConst.DecimalNull;
			Q2 = AppConst.IntNull;
			P2 = AppConst.DecimalNull;
			Q3 = AppConst.IntNull;
			P3 = AppConst.DecimalNull;
			CashRebate = AppConst.DecimalNull;
			Point = AppConst.IntNull;
			ClearanceSale = AppConst.IntNull;
			CreateTime = AppConst.DateTimeNull;
            LimitedQty = AppConst.IntNull;
		}

		public string GetLogMemo()
		{
			string str = @"Update:BasicP-"+BasicPrice.ToString(AppConst.DecimalFormat)+",CurrentP-"+CurrentPrice.ToString(AppConst.DecimalFormat)
						+",Discount-"+Discount.ToString(AppConst.DecimalFormat)+",UnitC-"+UnitCost.ToString(AppConst.DecimalFormat)+",CR-"+CashRebate.ToString(AppConst.DecimalFormat)
						+",Point-"+Point.ToString()+",Q1-"+Q1.ToString()+",P1-"+P1.ToString(AppConst.DecimalFormat)+",Q2-"+Q2.ToString()
						+",P2-"+P2.ToString(AppConst.DecimalFormat)+",Q3-"+Q3.ToString()+",P3-"+P3.ToString(AppConst.DecimalFormat)
						+",LastOP-"+LastOrderPrice.ToString(AppConst.DecimalFormat)+",LimitedQty-"+LimitedQty.ToString();
			return str;
		}
		public decimal GetRealPrice(int orderQty)
		{
			if ( IsWholeSale == 0) //AppEnum.YNStatus.NO
				return CurrentPrice;
			else if ( orderQty >=1 && orderQty < Q1)
				return CurrentPrice;
			else if ( orderQty >= Q1 && orderQty < Q2)
				return P1;
			else if ( orderQty >= Q2 && orderQty < Q3)
				return P2;
			else if ( orderQty >=Q3)
				return P3;
			else
				return AppConst.DefaultPrice;
		}
	}
}
