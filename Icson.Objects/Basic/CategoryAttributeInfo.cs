using System;

using Icson.Utils;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// Summary description for CategoryAttributeInfo.
	/// </summary>
	public class CategoryAttributeInfo : IComparable
	{
		public CategoryAttributeInfo()
		{
			Init();
		}

		private int _sysno;
		private int _c3sysno;
		private string _attributeid;
		private string _attributename;
		private int _ordernum;
		private int _status;
		private int _attributetype;

		public void Init()
		{
			_sysno = AppConst.IntNull;
			_c3sysno = AppConst.IntNull;
			_attributeid = AppConst.StringNull;
			_attributename = AppConst.StringNull;
			_ordernum = AppConst.IntNull;
			_status = AppConst.IntNull;
			_attributetype = AppConst.IntNull;
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
		public string AttributeID
		{
			get
			{
				return _attributeid;
			}
			set
			{
				_attributeid = value;
			}
		}
		public string AttributeName
		{
			get
			{
				return _attributename;
			}
			set
			{
				_attributename = value;
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

		public int AttributeType
		{
			get
			{
				return _attributetype;
			}
			set
			{
				_attributetype = value;
			}
		}

		#region IComparable Members

		public int CompareTo(object obj)
		{
			CategoryAttributeInfo b = obj as CategoryAttributeInfo;
			if ( this._ordernum > b._ordernum )
				return 1;
			else
				return -1;
		}

		#endregion
	}
}
