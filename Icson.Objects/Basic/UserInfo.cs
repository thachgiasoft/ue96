using System;

using Icson.Utils;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// Summary description for UserInfo.
	/// </summary>
	public class UserInfo : IComparable
	{
		public UserInfo()
		{
			Init();
		}

		private int _sysno;
		private string _userid;
		private string _username;
		private string _pwd;
		private string _email;
		private string _phone;
		private string _note;
		private int _status;
        private int _departmentsysno;
        private string _mobilephone;
        private int _branchsysno;
        private int _flag;
        private int _stationsysno;
        private int _pmgroupsysno;


		public void Init()
		{
			_sysno = AppConst.IntNull;
			_userid = AppConst.StringNull;
			_username = AppConst.StringNull;
			_pwd = AppConst.StringNull;
			_email = AppConst.StringNull;
			_phone = AppConst.StringNull;
			_note = AppConst.StringNull;
			_status = AppConst.IntNull;
            _departmentsysno = AppConst.IntNull;
            _mobilephone = AppConst.StringNull;
            _branchsysno = AppConst.IntNull;
            _flag = AppConst.IntNull;
            _stationsysno = AppConst.IntNull;
            _pmgroupsysno = AppConst.IntNull;
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
		public string UserID
		{
			get
			{
				return _userid;
			}
			set
			{
				_userid = value;
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
		public string Pwd
		{
			get
			{
				return _pwd;
			}
			set
			{
				_pwd = value;
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
		public string Phone
		{
			get
			{
				return _phone;
			}
			set
			{
				_phone = value;
			}
		}
		public string Note
		{
			get
			{
				return _note;
			}
			set
			{
				_note = value;
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
        public int DepartmentSysNo
        {
            get
            {
                return _departmentsysno;
            }
            set
            {
                _departmentsysno = value;
            }
        }

        public string MobilePhone
        {
            get
            {
                return _mobilephone;
            }
            set
            {
                _mobilephone = value;
            }
        }

        public int BranchSysNo
        {
            get
            {
                return _branchsysno;
            }
            set
            {
                _branchsysno = value;
            }
        }

        public int Flag
        {
            get
            {
                return _flag;
            }
            set
            {
                _flag = value;
            }
        }

        public int StationSysNo
        {
            get
            {
                return _stationsysno;
            }
            set
            {
                _stationsysno = value;
            }
        }

        public int PMGroupSysNo
        {
            get
            {
                return _pmgroupsysno;
            }
            set
            {
                _pmgroupsysno = value;
            }
        }

		public override string ToString()
		{
			return "[" + this.UserID +"] " + this.UserName;
		}

		#region IComparable Members

		public int CompareTo(object obj)
		{
			UserInfo b = obj as UserInfo;
			return String.Compare(this.UserID, b.UserID);
		}

		#endregion
	}
}
