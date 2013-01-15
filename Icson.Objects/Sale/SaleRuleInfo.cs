using System;
using System.Collections;
using Icson.Utils;

namespace Icson.Objects.Sale
{
	/// <summary>
	/// Summary description for SaleRule.
	/// </summary>
	public class SaleRuleInfo
	{
		public SaleRuleInfo()
		{
			Init();
		}
				
		public Hashtable ItemHash = new Hashtable();
		
		public int SysNo;
		public string SaleRuleName;
		public int Status;
		public int CreateUserSysNo;
		public DateTime CreateTime;

		public void Init()
		{
			SysNo = AppConst.IntNull;
			SaleRuleName = AppConst.StringNull;
			Status = AppConst.IntNull;
			CreateUserSysNo = AppConst.IntNull;
			CreateTime = AppConst.DateTimeNull;
		}

		public decimal GetTotalDiscount()
		{
			decimal totalDiscount = 0;
			if(this.ItemHash.Count>0)
			{
				foreach(SaleRuleItemInfo sri in this.ItemHash.Values)
				{
					totalDiscount += sri.Discount*sri.Quantity;
				}
			}
			return totalDiscount;
		}

		public string GetSRNote()
		{
			string note = "";
			if(this.ItemHash.Count>0)
			{
				foreach(SaleRuleItemInfo sri in this.ItemHash.Values)
				{
					note += sri.Quantity.ToString()+","+sri.ProductSysNo.ToString()+","+sri.Discount.ToString()+";";
				}
			}
			return note;
		}
	}
}
