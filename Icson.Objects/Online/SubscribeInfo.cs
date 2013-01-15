using System;

using Icson.Utils;

namespace Icson.Objects.Online
{
	/// <summary>
	/// Summary description for SubscribeInfo.
	/// </summary>
	public class SubscribeInfo
	{
		public SubscribeInfo()
		{
			Init();
		}
		public int SysNo;
		public string Email;
		public string UserName;
		public DateTime CreateTime;
		public void Init()
		{
			SysNo = AppConst.IntNull;
			Email = AppConst.StringNull;
			UserName = AppConst.StringNull;
			CreateTime = AppConst.DateTimeNull;
		}
	}
}
