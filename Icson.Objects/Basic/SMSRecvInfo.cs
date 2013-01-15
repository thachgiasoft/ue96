using System;
using Icson.Utils;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// Summary description for SMSRecvInfo.
	/// </summary>
	public class SMSRecvInfo
	{
		public SMSRecvInfo()
		{
			Init();
		}
		
		public int SysNo;
		public string CellNumber;
		public string SMSContent;
		public DateTime SendTime;
		public DateTime HandleTime;
		public void Init()
		{
			SysNo = AppConst.IntNull;
			CellNumber = AppConst.StringNull;
			SMSContent = AppConst.StringNull;
			SendTime = AppConst.DateTimeNull;
			HandleTime = AppConst.DateTimeNull;
		}

	}
}
