using System;

using Icson.Utils;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// Summary description for ShipTypeInfo.
	/// </summary>
	public class ShipTypeInfo : IComparable
	{
		public ShipTypeInfo()
		{
			Init();
		}
		private int _sysno;
		private string _shiptypeid;
		private string _shiptypename;
		private string _shiptypedesc;
		private string _period;
		private string _provider;
		private decimal _premiumrate;
		private decimal _premiumbase;
		private decimal _freeshipbase;
		private string _ordernumber;
		private int _isonlineshow;
		private int _iswithpackfee;
        private int _statusquerytype;
        private string _statusqueryurl;

		public void Init()
		{
			_sysno = AppConst.IntNull;
			_shiptypeid = AppConst.StringNull;
			_shiptypename = AppConst.StringNull;
			_shiptypedesc = AppConst.StringNull;
			_period = AppConst.StringNull;
			_provider = AppConst.StringNull;
			_premiumrate = AppConst.DecimalNull;
			_premiumbase = AppConst.DecimalNull;
			_freeshipbase = AppConst.DecimalNull;
			_ordernumber = AppConst.StringNull;
			_isonlineshow = AppConst.IntNull;
			_iswithpackfee = AppConst.IntNull;
            _statusquerytype = AppConst.IntNull;
            _statusqueryurl = AppConst.StringNull;
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
		public string ShipTypeID
		{
			get
			{
				return _shiptypeid;
			}
			set
			{
				_shiptypeid = value;
			}
		}
		public string ShipTypeName
		{
			get
			{
				return _shiptypename;
			}
			set
			{
				_shiptypename = value;
			}
		}
		public string ShipTypeDesc
		{
			get
			{
				return _shiptypedesc;
			}
			set
			{
				_shiptypedesc = value;
			}
		}
		public string Period
		{
			get
			{
				return _period;
			}
			set
			{
				_period = value;
			}
		}
		public string Provider
		{
			get
			{
				return _provider;
			}
			set
			{
				_provider = value;
			}
		}
		public decimal PremiumRate
		{
			get
			{
				return _premiumrate;
			}
			set
			{
				_premiumrate = value;
			}
		}
		public decimal PremiumBase
		{
			get
			{
				return _premiumbase;
			}
			set
			{
				_premiumbase = value;
			}
		}
		public decimal FreeShipBase
		{
			get
			{
				return _freeshipbase;
			}
			set
			{
				_freeshipbase = value;
			}
		}
		public string OrderNumber
		{
			get
			{
				return _ordernumber;
			}
			set
			{
				_ordernumber = value;
			}
		}
		public int IsOnlineShow
		{
			get
			{
				return _isonlineshow;
			}
			set
			{
				_isonlineshow = value;
			}
		}
		public int IsWithPackFee
		{
			get
			{
				return _iswithpackfee;
			}
			set
			{
				_iswithpackfee = value;
			}
		}

        public int StatusQueryType
        {
            get
            {
                return _statusquerytype;
            }
            set
            {
                _statusquerytype = value;
            }
        }

        public string StatusQueryUrl
        {
            get
            {
                return _statusqueryurl;
            }
            set
            {
                _statusqueryurl = value;
            }
        }

		#region IComparable Members

		public int CompareTo(object obj)
		{
			ShipTypeInfo b = obj as ShipTypeInfo;
			if ( String.Compare(this.OrderNumber, b.OrderNumber) >= 0 )
				return 1;
			else
				return -1;
		}

		#endregion
	}
}
