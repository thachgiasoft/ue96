using System;

using Icson.Utils;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// Summary description for PayTypeInfo.
	/// </summary>
	public class PayTypeInfo : IComparable
	{
		public PayTypeInfo()
		{
			Init();
		}
		private int _sysno;
		private string _paytypeid;
		private string _paytypename;
		private string _paytypedesc;
		private string _period;
		private string _paymentpage;
		private decimal _payrate;
		private int _isnet;
		private int _ispaywhenrecv;
		private string _ordernumber;
		private int _isonlineshow;

		public void Init()
		{
			_sysno = AppConst.IntNull;
			_paytypeid = AppConst.StringNull;
			_paytypename = AppConst.StringNull;
			_paytypedesc = AppConst.StringNull;
			_period = AppConst.StringNull;
			_paymentpage = AppConst.StringNull;
			_payrate = AppConst.DecimalNull;
			_isnet = AppConst.IntNull;
			_ispaywhenrecv = AppConst.IntNull;
			_ordernumber = AppConst.StringNull;
			_isonlineshow = AppConst.IntNull;
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
		public string PayTypeID
		{
			get
			{
				return _paytypeid;
			}
			set
			{
				_paytypeid = value;
			}
		}
		public string PayTypeName
		{
			get
			{
				return _paytypename;
			}
			set
			{
				_paytypename = value;
			}
		}
		public string PayTypeDesc
		{
			get
			{
				return _paytypedesc;
			}
			set
			{
				_paytypedesc = value;
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
		public string PaymentPage
		{
			get
			{
				return _paymentpage;
			}
			set
			{
				_paymentpage = value;
			}
		}
		public decimal PayRate
		{
			get
			{
				return _payrate;
			}
			set
			{
				_payrate = value;
			}
		}
		public int IsNet
		{
			get
			{
				return _isnet;
			}
			set
			{
				_isnet = value;
			}
		}
		public int IsPayWhenRecv
		{
			get
			{
				return _ispaywhenrecv;
			}
			set
			{
				_ispaywhenrecv = value;
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
		#region IComparable Members

		public int CompareTo(object obj)
		{
			PayTypeInfo b = obj as PayTypeInfo;
			if ( String.Compare(this.OrderNumber, b.OrderNumber) >= 0 )
				return 1;
			else
				return -1;
		}

		#endregion
	}
}
