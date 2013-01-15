using System;
using Icson.Utils;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// Summary description for EmailInfo.
	/// </summary>
	public class EmailInfo
	{
		public EmailInfo()
		{
			Init();
		}

		public EmailInfo(string address, string subject, string body)
		{
			MailAddress = address;
			MailSubject = subject;
			MailBody = body;
			Status = 0; // 0 origin.
		}
		
		public int SysNo;
		public string MailAddress;
		public string MailSubject;
		public string MailBody;
		public int Status;

		public void Init()
		{
			SysNo = AppConst.IntNull;
			MailAddress = AppConst.StringNull;
			MailSubject = AppConst.StringNull;
			MailBody = AppConst.StringNull;
			Status = AppConst.IntNull;
		}
	}
}
