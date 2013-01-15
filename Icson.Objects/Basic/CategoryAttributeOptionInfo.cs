using System;
using Icson.Utils;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// CategoryAttributeOptionInfo 的摘要说明。
	/// </summary>
	public class CategoryAttributeOptionInfo : IComparable
	{
		public CategoryAttributeOptionInfo()
		{
			Init();
		}

		private int _sysno;
		private int _attributesysno;
		private string _attributeoptionname;		
		private int _ordernum;
		private int _isrecommend;
		private int _status;

		public void Init()
		{
			_sysno = AppConst.IntNull;
			_attributesysno = AppConst.IntNull;
			_attributeoptionname = AppConst.StringNull;
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
		public int AttributeSysNo
		{
			get
			{
				return _attributesysno;
			}
			set
			{
				_attributesysno = value;
			}
		}
		public string AttributeOptionName
		{
			get
			{
				return _attributeoptionname;
			}
			set
			{
				_attributeoptionname = value;
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
			CategoryAttributeOptionInfo b = obj as CategoryAttributeOptionInfo;
			if ( this._ordernum > b._ordernum )
				return 1;
			else
				return -1;
		}

		#endregion
	}
}
