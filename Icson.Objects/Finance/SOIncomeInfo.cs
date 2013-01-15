using System;

using Icson.Utils;

namespace Icson.Objects.Finance
{
	/// <summary>
	/// Summary description for SOIncomeInfo.
	/// </summary>
	public class SOIncomeInfo
	{
		public SOIncomeInfo()
		{
			Init();
		}
		public int SysNo;
		public int OrderType;
		public int OrderSysNo;
		public decimal OrderAmt;
		public int IncomeStyle;
		public decimal IncomeAmt;
		public DateTime IncomeTime;
		public int IncomeUserSysNo;
		public DateTime ConfirmTime;
		public int ConfirmUserSysNo;
		public string Note;
		public int Status;
		public void Init()
		{
			SysNo = AppConst.IntNull;
			OrderType = AppConst.IntNull;
			OrderSysNo = AppConst.IntNull;
			OrderAmt = AppConst.DecimalNull;
			IncomeStyle = AppConst.IntNull;
			IncomeAmt = AppConst.DecimalNull;
			IncomeTime = AppConst.DateTimeNull;
			IncomeUserSysNo = AppConst.IntNull;
			ConfirmTime = AppConst.DateTimeNull;
			ConfirmUserSysNo = AppConst.IntNull;
			Note = AppConst.StringNull;
			Status = AppConst.IntNull;
		}
	}
}
