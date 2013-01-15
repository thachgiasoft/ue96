using System;

using Icson.Utils;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// Summary description for UserRoleInfo.
	/// </summary>
	public class UserRoleInfo
	{
		public UserRoleInfo()
		{
			Init();
		}
		private int _sysno;
		private int _usersysno;
		private int _rolesysno;
		private string _roleid;
		private string _rolename;

		public void Init()
		{
			_sysno = AppConst.IntNull;
			_usersysno = AppConst.IntNull;
			_rolesysno = AppConst.IntNull;
			_roleid = AppConst.StringNull;
			_rolename = AppConst.StringNull;
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
		public int UserSysNo
		{
			get
			{
				return _usersysno;
			}
			set
			{
				_usersysno = value;
			}
		}
		public int RoleSysNo
		{
			get
			{
				return _rolesysno;
			}
			set
			{
				_rolesysno = value;
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
	}
}
