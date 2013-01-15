using System;

using Icson.Utils;

namespace Icson.Objects.Online
{
	/// <summary>
	/// Summary description for PollItemInfo.
	/// </summary>
	public class PollItemInfo
	{
		public PollItemInfo()
		{
			Init();
		}
		public int SysNo;
		public int PollSysNo;
		public string ItemName;
		public int ItemCount;
		public void Init()
		{
			SysNo = AppConst.IntNull;
			PollSysNo = AppConst.IntNull;
			ItemName = AppConst.StringNull;
			ItemCount = AppConst.IntNull;
		}
	}
}
