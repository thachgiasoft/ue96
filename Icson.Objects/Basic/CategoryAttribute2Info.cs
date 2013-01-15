using System;

using Icson.Utils;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// Summary description for CategoryAttribute2Info.
	/// </summary>
	public class CategoryAttribute2Info : IComparable
	{
		public CategoryAttribute2Info()
		{
			Init();
		}

		private int _sysno;
		private int _a1sysno;
		private string _attribute2id;
		private string _attribute2name;
		private int _ordernum;
		private int _status;
		private int _attribute2type;

		public void Init()
		{
			_sysno = AppConst.IntNull;
			_a1sysno = AppConst.IntNull;
			_attribute2id = AppConst.StringNull;
			_attribute2name = AppConst.StringNull;
			_ordernum = AppConst.IntNull;
			_status = AppConst.IntNull;
			_attribute2type = AppConst.IntNull;
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
		public int A1SysNo
		{
			get
			{
				return _a1sysno;
			}
			set
			{
				_a1sysno = value;
			}
		}
		public string Attribute2ID
		{
			get
			{
				return _attribute2id;
			}
			set
			{
				_attribute2id = value;
			}
		}
		public string Attribute2Name
		{
			get
			{
				return _attribute2name;
			}
			set
			{
				_attribute2name = value;
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

		public int Attribute2Type
		{
			get
			{
				return _attribute2type;
			}
			set
			{
				_attribute2type = value;
			}
		}

		public override string ToString()
		{
			return "[" + this._attribute2id +"] " + this._attribute2name;
		}

		#region IComparable Members

		public int CompareTo(object obj)
		{
			CategoryAttribute2Info b = obj as CategoryAttribute2Info;
			if ( this._ordernum > b._ordernum )
				return 2;
			else
				return -2;
		}

		#endregion
	}
}
