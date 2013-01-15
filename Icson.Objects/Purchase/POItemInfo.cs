using System;

using Icson.Utils;

namespace Icson.Objects.Purchase
{
	/// <summary>
	/// Summary description for POItemInfo.
	/// </summary>
	public class POItemInfo
	{
		public POItemInfo()
		{
			Init();
		}
		
		public int SysNo;
		public int POSysNo;
		public int ProductSysNo;
		public int Quantity;
		public int Weight;
		public decimal OrderPrice;
		public decimal ApportionAddOn;
		public decimal UnitCost;
        public int OrderQty;

		public void Init()
		{
			SysNo = AppConst.IntNull;
			POSysNo = AppConst.IntNull;
			ProductSysNo = AppConst.IntNull;
			Quantity = AppConst.IntNull;
			Weight = AppConst.IntNull;
			OrderPrice = AppConst.DecimalNull;
			ApportionAddOn = AppConst.DecimalNull;
			UnitCost = AppConst.DecimalNull;
            OrderQty = AppConst.IntNull;
		}
	}
}
