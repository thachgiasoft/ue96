using System;

using Icson.Utils;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// Summary description for PrivilegeInfo.
	/// </summary>
	public class PrivilegeInfo : IComparable
	{
		public PrivilegeInfo()
		{
			Init();
		}
		private int _sysno;
		private string _privilegeid;
		private string _privilegename;
		private int _status;

		public void Init()
		{
			_sysno = AppConst.IntNull;
			_privilegeid = AppConst.StringNull;
			_privilegename = AppConst.StringNull;
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
		#region IComparable Members

		public int CompareTo(object obj)
		{
			PrivilegeInfo o = obj as PrivilegeInfo;
			return String.Compare(this.PrivilegeID, o.PrivilegeID );
		}

		#endregion
	}
}
