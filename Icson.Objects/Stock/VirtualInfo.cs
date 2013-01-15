using System;

using Icson.Utils;

namespace Icson.Objects.Stock
{
	/// <summary>
	/// Summary description for VirtualInfo.
	/// </summary>
	public class VirtualInfo
	{
		public VirtualInfo()
		{
			Init();
		}

		public int SysNo;
		public int ProductSysNo;
		public int VirtualQty;
		public DateTime CreateTime;
		public int CreateUserSysNo;
		public void Init()
		{
			SysNo = AppConst.IntNull;
			ProductSysNo = AppConst.IntNull;
			VirtualQty = AppConst.IntNull;
			CreateTime = AppConst.DateTimeNull;
			CreateUserSysNo = AppConst.IntNull;
		}
	}
}
