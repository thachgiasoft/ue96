using System;

using Icson.Utils;

namespace Icson.Objects.Stock
{
	/// <summary>
	/// Summary description for LendReturnInfo.
	/// </summary>
	public class LendReturnInfo
	{
		public LendReturnInfo()
		{
			Init();
		}

		public int SysNo;
		public int LendSysNo;
		public int ProductSysNo;
		public int ReturnQty;
		public DateTime ReturnTime;
		public void Init()
		{
			SysNo = AppConst.IntNull;
			LendSysNo = AppConst.IntNull;
			ProductSysNo = AppConst.IntNull;
			ReturnQty = AppConst.IntNull;
			ReturnTime = AppConst.DateTimeNull;
		}
	}
}
