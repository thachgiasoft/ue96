using System;

using Icson.Utils;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// Summary description for Category1Info.
	/// </summary>
	public class Category1Info : IComparable
	{
		public Category1Info()
		{
			Init();
		}

		private int _sysno;
		private string _c1id;
		private string _c1name;
		private int _status;

		public void Init()
		{
			_sysno = AppConst.IntNull;
			_c1id = AppConst.StringNull;
			_c1name = AppConst.StringNull;
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
		public string C1ID
		{
			get
			{
				return _c1id;
			}
			set
			{
				_c1id = value;
			}
		}
		public string C1Name
		{
			get
			{
				return _c1name;
			}
			set
			{
				_c1name = value;
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
			return "[" + this._c1id +"] " + this._c1name;
		}

		#region IComparable Members

		public int CompareTo(object obj)
		{
			Category1Info b = obj as Category1Info;
			if ( this.Status < b.Status )
				return 1;
			else if ( this.Status > b.Status )
				return -1;
			else
			{
				int result = String.Compare(this.C1ID, b.C1ID);
				if ( result > 0)
					return 1;
				else
					return -1;
			}
		}

		#endregion
	}
}
