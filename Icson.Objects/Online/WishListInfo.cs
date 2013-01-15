using System;

using Icson.Utils;

namespace Icson.Objects.Online
{
	/// <summary>
	/// Summary description for WishListInfo.
	/// </summary>
	public class WishListInfo
	{
		public WishListInfo()
		{
			Init();
		}
		public int SysNo;
		public int CustomerSysNo;
		public int ProductSysNo;
		public DateTime CreateTime;
		public void Init()
		{
			SysNo = AppConst.IntNull;
			CustomerSysNo = AppConst.IntNull;
			ProductSysNo = AppConst.IntNull;
			CreateTime = AppConst.DateTimeNull;
		}
	}
}
