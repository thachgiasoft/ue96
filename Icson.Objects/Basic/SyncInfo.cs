using System;
using Icson.Utils;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// Summary description for SyncInfo.
	/// </summary>
	public class SyncInfo
	{
		public SyncInfo()
		{
			Init();
		}

		
		public int SysNo;
		public int SyncType;
		public DateTime LastVersionTime;
		public void Init()
		{
			SysNo = AppConst.IntNull;
			SyncType = AppConst.IntNull;
			LastVersionTime = AppConst.DateTimeNull;
		}


	}
}
