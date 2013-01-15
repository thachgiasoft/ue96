using System;

using Icson.Utils;

namespace Icson.Objects.Online
{
	/// <summary>
	/// Summary description for BulletinInfo.
	/// </summary>
	public class BulletinInfo
	{
		public BulletinInfo()
		{
			Init();
		}

		public int SysNo;
		public string Message;
		public int Status;
		public void Init()
		{
			SysNo = AppConst.IntNull;
			Message = AppConst.StringNull;
			Status = AppConst.IntNull;
		}
	}
}
