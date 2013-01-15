using System;

using Icson.Utils;

namespace Icson.Objects.Stock
{
	/// <summary>
	/// Summary description for LendItemInfo.
	/// </summary>
	public class LendItemInfo
	{
		public LendItemInfo()
		{
			Init();
		}

		
		public int SysNo;
		public int LendSysNo;
		public int ProductSysNo;
		public int LendQty;
		public DateTime ExpectReturnTime;
		public void Init()
		{
			SysNo = AppConst.IntNull;
			LendSysNo = AppConst.IntNull;
			ProductSysNo = AppConst.IntNull;
			LendQty = AppConst.IntNull;
			ExpectReturnTime = AppConst.DateTimeNull;
		}
	}
}
