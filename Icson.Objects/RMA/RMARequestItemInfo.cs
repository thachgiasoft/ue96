using System;
using Icson.Utils;

namespace Icson.Objects.RMA
{
	/// <summary>
	/// Summary description for RMARequestItemInfo.
	/// </summary>
	public class RMARequestItemInfo
	{
		public RMARequestItemInfo()
		{
			Init();
		}

		public int SysNo;
		public int RequestSysNo;
		public int RegisterSysNo;
		public void Init()
		{
			SysNo = AppConst.IntNull;
			RequestSysNo = AppConst.IntNull;
			RegisterSysNo = AppConst.IntNull;
		}

	}
}
