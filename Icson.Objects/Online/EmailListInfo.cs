using System;
using Icson.Utils;

namespace Icson.Objects.Online
{
	/// <summary>
	/// Summary description for EmailListInfo.
	/// </summary>
	public class EmailListInfo_
	{
		public EmailListInfo_()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		private int _sysno;
		private string _username;
		private string _email;
		private int _status;
		private DateTime _createtime;

		public void Init()
		{
			_sysno = AppConst.IntNull;
			_username = AppConst.StringNull;
			_email = AppConst.StringNull;
			_status = AppConst.IntNull;
			_createtime = AppConst.DateTimeNull;
		}

		public int SysNo
		{
			get
			{
				return _sysno;
			}
			set
			{
				_sysno = value;
			}
		}
		public string UserName
		{
			get
			{
				return _username;
			}
			set
			{
				_username = value;
			}
		}
		public string Email
		{
			get
			{
				return _email;
			}
			set
			{
				_email = value;
			}
		}
		public int Status
		{
			get
			{
				return _status;
			}
			set
			{
				_status = value;
			}
		}
		public DateTime CreateTime
		{
			get
			{
				return _createtime;
			}
			set
			{
				_createtime = value;
			}
		}
	}
}
