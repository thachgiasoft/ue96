using System;

using Icson.Utils;

namespace Icson.Objects.Purchase
{
	/// <summary>
	/// Summary description for POApportionItemInfo.
	/// </summary>
	public class POApportionItemInfo
	{
		public POApportionItemInfo()
		{
			Init();
		}
		
		public int SysNo;
		public int ApportionSysNo;
		public int ProductSysNo;
		public void Init()
		{
			SysNo = AppConst.IntNull;
			ApportionSysNo = AppConst.IntNull;
			ProductSysNo = AppConst.IntNull;
		}


	}
}
