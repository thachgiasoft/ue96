using System;

using Icson.Utils;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// ProductAttribute2Summary 的摘要说明。
	/// </summary>
	public class ProductAttribute2Summary
	{
		public ProductAttribute2Summary()
		{
			Init();
		}
		private int _sysno;
		private int _productsysno;
		private string _summary;
	    private string _summarymain;

		public void Init()
		{
			_sysno = AppConst.IntNull;
			_productsysno = AppConst.IntNull;
			_summary = AppConst.StringNull;
		    _summarymain = AppConst.StringNull;
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
		public string Summary
		{
			get
			{
				return _summary;
			}
			set
			{
				_summary = value;
			}
		}
	    public string SummaryMain
	    {
            get
            {
                 return _summarymain;
            }
            set
            {
                 _summarymain = value;
            }
	    }
	}
}
