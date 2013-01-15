using System;
using Icson.Utils;

namespace Icson.Objects.Sale
{
	/// <summary>
	/// Summary description for SOSaleRuleInfo.
	/// </summary>
	public class SOSaleRuleInfo
	{
		public SOSaleRuleInfo()
		{
			Init();
		}		
		
		public int SysNo;
		public int SOSysNo;
		public int SaleRuleSysNo;
		public string SaleRuleName;
		public decimal Discount;
		public int Times;
		public string Note;
		public DateTime CreateTime;

		public void Init()
		{
			SysNo = AppConst.IntNull;
			SOSysNo = AppConst.IntNull;
			SaleRuleSysNo = AppConst.IntNull;
			SaleRuleName = AppConst.StringNull;
			Discount = AppConst.DecimalNull;
			Times = AppConst.IntNull;
			Note = AppConst.StringNull;
			CreateTime = AppConst.DateTimeNull;
		}
	}
}
