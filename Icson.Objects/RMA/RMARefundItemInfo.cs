using System;

using Icson.Utils;

namespace Icson.Objects.RMA
{
	/// <summary>
	/// Summary description for RMARefundItemInfo.
	/// </summary>
	public class RMARefundItemInfo
	{
		public RMARefundItemInfo()
		{
			Init();
		}

		public int SysNo;
		public int RefundSysNo;
		public int RegisterSysNo;
		public decimal OrgPrice;
		public decimal UnitDiscount;
		public decimal ProductValue;
		public int OrgPoint;
		public decimal RefundPrice;
		public int PointType;
		public decimal RefundCash;
		public int RefundPoint;
		public int RefundPriceType;
		public decimal RefundCost ;
		public int RefundCostPoint ;

		public void Init()
		{
			SysNo = AppConst.IntNull;
			RefundSysNo = AppConst.IntNull;
			RegisterSysNo = AppConst.IntNull;
			OrgPrice = AppConst.DecimalNull;
			UnitDiscount = AppConst.DecimalNull;
			ProductValue = AppConst.DecimalNull;
			OrgPoint = AppConst.IntNull;
			RefundPrice = AppConst.DecimalNull;
			PointType = AppConst.IntNull;
			RefundCash = AppConst.DecimalNull;
			RefundPoint = AppConst.IntNull;
			RefundPriceType = AppConst.IntNull;
			RefundCost = AppConst.DecimalNull ;
			RefundCostPoint = AppConst.IntNull;
		}

	}
}
