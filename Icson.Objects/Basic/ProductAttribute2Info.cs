using System;

using Icson.Utils;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// ProductAttribute2Info 的摘要说明。
	/// </summary>
	public class ProductAttribute2Info
	{
		public ProductAttribute2Info()
		{
			Init();
		}
		private int _sysno;
		private int _productsysno;
		private int _attribute2sysno;
		private int _attribute2optionsysno;
		private string _attribute2value;

		public void Init()
		{
			_sysno = AppConst.IntNull;
			_productsysno = AppConst.IntNull;
			_attribute2sysno = AppConst.IntNull;
			_attribute2optionsysno = AppConst.IntNull;
			_attribute2value = AppConst.StringNull;
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
		public int ProductSysNo
		{
			get
			{
				return _productsysno;
			}
			set
			{
				_productsysno = value;
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
		public int Attribute2OptionSysNo
		{
			get
			{
				return _attribute2optionsysno;
			}
			set
			{
				_attribute2optionsysno = value;
			}
		}
		public string Attribute2Value
		{
			get
			{
				return _attribute2value;
			}
			set
			{
				_attribute2value = value;
			}
		}		
	}
}
