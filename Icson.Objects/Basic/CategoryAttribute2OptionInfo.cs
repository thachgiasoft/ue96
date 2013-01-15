using System;
using Icson.Utils;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// CategoryAttributeOptionInfo 的摘要说明。
	/// </summary>
	public class CategoryAttribute2OptionInfo : IComparable
	{
		public CategoryAttribute2OptionInfo()
		{
			Init();
		}

		private int _sysno;
		private int _attribute2sysno;
		private string _attribute2optionname;		
		private int _ordernum;
		private int _isrecommend;
		private int _status;

		public void Init()
		{
			_sysno = AppConst.IntNull;
			_attribute2sysno = AppConst.IntNull;
			_attribute2optionname = AppConst.StringNull;
			_ordernum = AppConst.IntNull;
			_isrecommend = AppConst.IntNull;
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
		public int Attribute2SysNo
		{
			get
			{
				return _attribute2sysno;
			}
			set
			{
				_attribute2sysno = value;
			}
		}
		public string Attribute2OptionName
		{
			get
			{
				return _attribute2optionname;
			}
			set
			{
				_attribute2optionname = value;
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
		public int IsRecommend
		{
			get
			{
				return _isrecommend;
			}
			set
			{
				_isrecommend = value;
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
			CategoryAttribute2OptionInfo b = obj as CategoryAttribute2OptionInfo;
			if ( this._ordernum > b._ordernum )
				return 1;
			else
				return -1;
		}

		#endregion
	}
}
