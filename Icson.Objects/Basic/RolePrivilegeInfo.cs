using System;

using Icson.Utils;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// Summary description for RolePrivilegeInfo.
	/// </summary>
	public class RolePrivilegeInfo
	{
		public RolePrivilegeInfo()
		{
			Init();
		}
		private int _sysno;
		private int _rolesysno;
		private int _privilegesysno;
		private string _privilegeid;
		private string _privilegename;

		public void Init()
		{
			_sysno = AppConst.IntNull;
			_rolesysno = AppConst.IntNull;
			_privilegesysno = AppConst.IntNull;
			_privilegeid = AppConst.StringNull;
			_privilegename = AppConst.StringNull;
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
		public int PrivilegeSysNo
		{
			get
			{
				return _privilegesysno;
			}
			set
			{
				_privilegesysno = value;
			}
		}
		public string PrivilegeID
		{
			get
			{
				return _privilegeid;
			}
			set
			{
				_privilegeid = value;
			}
		}
		public string PrivilegeName
		{
			get
			{
				return _privilegename;
			}
			set
			{
				_privilegename = value;
			}
		}
	}
}
