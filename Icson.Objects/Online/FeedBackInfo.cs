using System;

using Icson.Utils;

namespace Icson.Objects.Online
{
	/// <summary>
	/// Summary description for FeedBackInfo.
	/// </summary>
	public class FeedBackInfo
	{
		public FeedBackInfo()
		{
			Init();
		}
		public int SysNo;
        public int CustomerSysNo;
		public string Subject;
		public string Suggest;
		public string NickName;
		public string Email;
		public string Phone;
		public string Memo;
		public string Note;
		public DateTime CreateTime;
		public DateTime UpdateTime;
        public int UpdateUserSysNo;
		public int Status;
        public int Sosysno;
		public void Init()
		{
			SysNo = AppConst.IntNull;
            CustomerSysNo = AppConst.IntNull;
			Subject = AppConst.StringNull;
			Suggest = AppConst.StringNull;
			NickName = AppConst.StringNull;
			Email = AppConst.StringNull;
			Phone = AppConst.StringNull;
			Memo = AppConst.StringNull;
			Note = AppConst.StringNull;
			CreateTime = AppConst.DateTimeNull;
			UpdateTime = AppConst.DateTimeNull;
			Status = AppConst.IntNull;
            Sosysno = AppConst.IntNull;
            UpdateUserSysNo = AppConst.IntNull;
		}
	}
}
