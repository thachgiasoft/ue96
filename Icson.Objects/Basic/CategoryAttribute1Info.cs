using System;

using Icson.Utils;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// Summary description for CategoryAttribute1Info.
	/// </summary>
	public class CategoryAttribute1Info : IComparable
	{
		public CategoryAttribute1Info()
		{
			Init();
		}

		private int _sysno;
		private int _c3sysno;
		private string _attribute1id;
		private string _attribute1name;
		private int _ordernum;
		private int _status;
		private int _attribute1type;

		public void Init()
		{
			_sysno = AppConst.IntNull;
			_c3sysno = AppConst.IntNull;
			_attribute1id = AppConst.StringNull;
			_attribute1name = AppConst.StringNull;
			_ordernum = AppConst.IntNull;
			_status = AppConst.IntNull;
			_attribute1type = AppConst.IntNull;
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
		public int C3SysNo
		{
			get
			{
				return _c3sysno;
			}
			set
			{
				_c3sysno = value;
			}
		}
		public string Attribute1ID
		{
			get
			{
				return _attribute1id;
			}
			set
			{
				_attribute1id = value;
			}
		}
		public string Attribute1Name
		{
			get
			{
				return _attribute1name;
			}
			set
			{
				_attribute1name = value;
			}
		}
		public int OrderNum
		{
			get
			{
				return _ordernum;
			}
			set
			{
				_ordernum = value;
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

		public int Attribute1Type
		{
			get
			{
				return _attribute1type;
			}
			set
			{
				_attribute1type = value;
			}
		}

		public override string ToString()
		{
			return "[" + this._attribute1id +"] " + this._attribute1name;
		}

		#region IComparable Members

		public int CompareTo(object obj)
		{
			CategoryAttribute1Info b = obj as CategoryAttribute1Info;
			if ( this._ordernum > b._ordernum )
				return 1;
			else
				return -1;
		}

		#endregion
	}
}
