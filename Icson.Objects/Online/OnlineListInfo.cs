using System;
using Icson.Utils;

namespace Icson.Objects.Online
{
	/// <summary>
	/// Summary description for OnlineListInfo.
	/// </summary>
	public class OnlineListInfo
	{
		public OnlineListInfo()
		{
			Init();
		}

		public int SysNo;
		public int ListArea;
		public int ProductSysNo;
		public int CreateUserSysNo;
		public DateTime CreateTime;
		public string ListOrder;
		public void Init()
		{
			SysNo = AppConst.IntNull;
			ListArea = AppConst.IntNull;
			ProductSysNo = AppConst.IntNull;
			CreateUserSysNo = AppConst.IntNull;
			CreateTime = AppConst.DateTimeNull;
			ListOrder = AppConst.StringNull;
		}
	}
}