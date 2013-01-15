using System;

using Icson.Utils;

namespace Icson.Objects.Stock
{
	/// <summary>
	/// Summary description for ShiftItemInfo.
	/// </summary>
	public class ShiftItemInfo
	{
		public ShiftItemInfo()
		{
			Init();
		}
		public int SysNo;
		public int ShiftSysNo;
		public int ProductSysNo;
		public int ShiftQty;
		public void Init()
		{
			SysNo = AppConst.IntNull;
			ShiftSysNo = AppConst.IntNull;
			ProductSysNo = AppConst.IntNull;
			ShiftQty = AppConst.IntNull;
		}

	}
}
