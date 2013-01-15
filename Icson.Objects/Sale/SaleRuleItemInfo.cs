using System;
using Icson.Utils;

namespace Icson.Objects.Sale
{
	/// <summary>
	/// Summary description for SaleRuleItem.
	/// </summary>
	public class SaleRuleItemInfo
	{
		public SaleRuleItemInfo()
		{
			Init();
		}
		
		public int SysNo;
		public int SaleRuleSysNo;
		public int ProductSysNo;
		public int Quantity;
		public decimal Discount;
		public DateTime CreateTime;

		public void Init()
		{
			SysNo = AppConst.IntNull;
			SaleRuleSysNo = AppConst.IntNull;
			ProductSysNo = AppConst.IntNull;
			Quantity = AppConst.IntNull;
			Discount = AppConst.DecimalNull;
			CreateTime = AppConst.DateTimeNull;
		}
	}
}
