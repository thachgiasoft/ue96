using System;

using Icson.Utils;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// Summary description for CurrencyInfo.
	/// </summary>
	public class CurrencyInfo : IComparable
	{
		public CurrencyInfo()
		{
			Init();
		}
		
		
		public int SysNo;
		public string CurrencyID;
		public string CurrencyName;
		public string CurrencySymbol;
		public int IsLocal;
		public decimal ExchangeRate;
		public string ListOrder;
		public int Status;
		public void Init()
		{
			SysNo = AppConst.IntNull;
			CurrencyID = AppConst.StringNull;
			CurrencyName = AppConst.StringNull;
			CurrencySymbol = AppConst.StringNull;
			IsLocal = AppConst.IntNull;
			ExchangeRate = AppConst.DecimalNull;
			ListOrder = AppConst.StringNull;
			Status = AppConst.IntNull;
		}
		public override string ToString()
		{
			return CurrencySymbol + CurrencyName;
		}


		#region IComparable Members

		public int CompareTo(object obj)
		{
			CurrencyInfo b = obj as CurrencyInfo;
			if (this.ListOrder == b.ListOrder)
				return String.Compare(this.CurrencyID, b.CurrencyID);
			else
				return String.Compare(this.ListOrder, b.ListOrder);
		}

		#endregion
	}
}
