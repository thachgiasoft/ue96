using System;
using Icson.Utils;

namespace Icson.Objects.Sale
{
	/// <summary>
	/// Summary description for DOInfo.
	/// </summary>
	public class DOInfo
	{
		public DOInfo()
		{
			Init();
		}
		
		public int SysNo;
		public int SOSysNo;
		public string DONo;
		public DateTime CreateTime;
		
		public void Init()
		{
			SysNo = AppConst.IntNull;
			SOSysNo = AppConst.IntNull;
			DONo = AppConst.StringNull;
			CreateTime = AppConst.DateTimeNull;
		}
	}
}
