using System;

using Icson.Utils;

namespace Icson.Objects.Online
{
	/// <summary>
	/// Summary description for LinkSourceInfo.
	/// </summary>
	public class LinkSourceInfo
	{
		public LinkSourceInfo()
		{
			Init();
		}
		public int SysNo;
		public string URLSource;
		public int VisitCount;
		public string CountDate;
		public void Init()
		{
			SysNo = AppConst.IntNull;
			URLSource = AppConst.StringNull;
			VisitCount = AppConst.IntNull;
			CountDate = AppConst.StringNull;
		}
	}
}
