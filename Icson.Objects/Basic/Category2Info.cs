using System;

using Icson.Utils;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// Summary description for Category2Info.
	/// </summary>
	public class Category2Info : IComparable
	{
		public Category2Info()
		{
			Init();
		}

		private int _sysno;
		private int _c1sysno;
		private string _c2id;
		private string _c2name;
		private int _status;

		public void Init()
		{
			_sysno = AppConst.IntNull;
			_c1sysno = AppConst.IntNull;
			_c2id = AppConst.StringNull;
			_c2name = AppConst.StringNull;
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
		public int C1SysNo
		{
			get
			{
				return _c1sysno;
			}
			set
			{
				_c1sysno = value;
			}
		}
		public string C2ID
		{
			get
			{
				return _c2id;
			}
			set
			{
				_c2id = value;
			}
		}
		public string C2Name
		{
			get
			{
				return _c2name;
			}
			set
			{
				_c2name = value;
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
		public override string ToString()
		{
			return "[" + this._c2id +"] " + this._c2name;
		}

		#region IComparable Members

		public int CompareTo(object obj)
		{
			Category2Info b = obj as Category2Info;

			if ( this.Status < b.Status )
				return 1;
			else if ( this.Status > b.Status )
				return -1;
			else
			{
				int result = String.Compare(this.C2ID, b.C2ID);
				if ( result > 0)
					return 1;
				else
					return -1;
			}
				
		}

		#endregion
	}
}
