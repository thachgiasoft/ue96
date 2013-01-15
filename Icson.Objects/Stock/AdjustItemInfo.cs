using System;

using Icson.Utils;

namespace Icson.Objects.Stock
{
	/// <summary>
	/// Summary description for AdjustItemInfo.
	/// </summary>
	public class AdjustItemInfo
	{
		public AdjustItemInfo()
		{
			Init();
		}
		
		public int SysNo;
		public int AdjustSysNo;
		public int ProductSysNo;
		public int AdjustQty;
		public decimal AdjustCost;
		public void Init()
		{
			SysNo = AppConst.IntNull;
			AdjustSysNo = AppConst.IntNull;
			ProductSysNo = AppConst.IntNull;
			AdjustQty = AppConst.IntNull;
			AdjustCost = AppConst.DecimalNull;
		}

	}
}
