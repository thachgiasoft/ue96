using System;
using System.Collections;

using Icson.Utils;

namespace Icson.Objects.Purchase
{
	/// <summary>
	/// Summary description for POApportionInfo.
	/// </summary>
	public class POApportionInfo
	{
		public POApportionInfo()
		{
			Init();
		}

		public Hashtable itemHash = new Hashtable(5); //key=productsysno value = null
		
		public int SysNo;
		public int POSysNo;
		public int ApportionSubjectSysNo;
		public int ApportionType;
		public decimal ExpenseAmt;
		public void Init()
		{
			SysNo = AppConst.IntNull;
			POSysNo = AppConst.IntNull;
			ApportionSubjectSysNo = AppConst.IntNull;
			ApportionType = AppConst.IntNull;
			ExpenseAmt = AppConst.DecimalNull;
			itemHash.Clear();
		}
	}
}
