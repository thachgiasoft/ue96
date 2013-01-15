using System;
using Icson.Utils;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// Summary description for SMSInfo.
	/// </summary>
	public class SMSInfo
	{
		public SMSInfo()
		{
			Init();
		}

        public SMSInfo(string cellNumber, string smsContent, int priority, DateTime expectSendTime)
		{
			Init();
			CellNumber = cellNumber;
			SMSContent = smsContent;
			RetryCount = 0;
			Priority = priority;
			CreateTime = DateTime.Now;
            ExpectSendTime = expectSendTime;
			Status = 0; //orgion
		}

        public SMSInfo(string cellNumber, string smsContent, int createUserSysNo, int priority, DateTime expectSendTime,int status)
        {
            Init();
            CellNumber = cellNumber;
            SMSContent = smsContent;
            CreateUserSysNo = createUserSysNo;
            RetryCount = 0;
            Priority = priority;
            CreateTime = DateTime.Now;
            ExpectSendTime = expectSendTime;
            Status = status;
        }

		public int SysNo;
		public string CellNumber;
		public string SMSContent;
		public int Priority;
		public int RetryCount;
        public int CreateUserSysNo;
		public DateTime CreateTime;
        public DateTime ExpectSendTime;
		public DateTime HandleTime;
		public int Status;

		public void Init()
		{
			SysNo = AppConst.IntNull;
			CellNumber = AppConst.StringNull;
			SMSContent = AppConst.StringNull;
			Priority = AppConst.IntNull;
			RetryCount = AppConst.IntNull;
            CreateUserSysNo = AppConst.IntNull;
			CreateTime = AppConst.DateTimeNull;
            ExpectSendTime = AppConst.DateTimeNull;
			HandleTime = AppConst.DateTimeNull;
			Status = AppConst.IntNull;
		}
	}
}