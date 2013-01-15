using System;

using Icson.Utils;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// Summary description for RoleInfo.
	/// </summary>
	public class RoleInfo
	{
		public RoleInfo()
		{
			Init();
		}
		private int _sysno;
		private string _roleid;
		private string _rolename;
		private int _status;
        private int _operationtypeid;


		public void Init()
		{
			_sysno = AppConst.IntNull;
			_roleid = AppConst.StringNull;
			_rolename = AppConst.StringNull;
			_status = AppConst.IntNull;
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
		public string RoleID
		{
			get
			{
				return _roleid;
			}
			set
			{
				_roleid = value;
			}
		}
		public string RoleName
		{
			get
			{
				return _rolename;
			}
			set
			{
				_rolename = value;
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
        public int OperationTypeID
        {
            get
            {
                return _operationtypeid;
            }
            set
            {
                _operationtypeid = value;
            }
        }

	}
}
