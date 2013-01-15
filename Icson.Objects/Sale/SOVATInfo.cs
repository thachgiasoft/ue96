using System;
using Icson.Utils;

namespace Icson.Objects.Sale
{
	/// <summary>
	/// Summary description for SOVATInfo.
	/// </summary>
	[Serializable] 
	public class SOVATInfo
	{
		public SOVATInfo()
		{
			Init();
		}
	
		public int SysNo;
		public int SOSysNo;
		public int CustomerSysNo;
		public string CompanyName;
		public string TaxNum;
		public string CompanyAddress;
		public string CompanyPhone;
		public string BankAccount;
		public string Memo;
		public DateTime CreateTime;
		public decimal VATEMSFee;

		public void Init()
		{
			SysNo = AppConst.IntNull;
			SOSysNo = AppConst.IntNull;
			CustomerSysNo = AppConst.IntNull;
			CompanyName = AppConst.StringNull;
			TaxNum = AppConst.StringNull;
			CompanyAddress = AppConst.StringNull;
			CompanyPhone = AppConst.StringNull;
			BankAccount = AppConst.StringNull;
			Memo = AppConst.StringNull;
			CreateTime = AppConst.DateTimeNull;
			VATEMSFee = AppConst.DecimalNull;
		}
	}
}
