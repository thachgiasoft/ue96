using System;

using Icson.Utils;

namespace Icson.Objects.Online
{
	/// <summary>
	/// Summary description for DailyClickInfo.
	/// </summary>
	public class DailyClickInfo
	{
		public DailyClickInfo()
		{
			Init();
		}
		public int SysNo;
		public int ProductSysNo;
		public string ClickDate;
		public int ClickCount;
		public void Init()
		{
			SysNo = AppConst.IntNull;
			ProductSysNo = AppConst.IntNull;
			ClickDate = AppConst.StringNull;
			ClickCount = AppConst.IntNull;
		}
	}
}
