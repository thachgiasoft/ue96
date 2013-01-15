using System;

using Icson.Utils;

namespace Icson.Objects.Online
{
	/// <summary>
	/// Summary description for ProductNotifyInfo.
	/// </summary>
	public class ProductNotifyInfo
	{
		public ProductNotifyInfo()
		{
			Init();
		}
		public int SysNo;
		public int CustomerSysNo;
		public int ProductSysNo;
		public string Email;
		public DateTime CreateTime;
		public DateTime NotifyTime;
		public int Status;
		public void Init()
		{
			SysNo = AppConst.IntNull;
			CustomerSysNo = AppConst.IntNull;
			ProductSysNo = AppConst.IntNull;
			Email = AppConst.StringNull;
			CreateTime = AppConst.DateTimeNull;
			NotifyTime = AppConst.DateTimeNull;
			Status = AppConst.IntNull;
		}
	}
}
