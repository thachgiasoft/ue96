using System;

using Icson.Utils;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// Summary description for StockInfo.
	/// </summary>
	public class StockInfo : IComparable
	{
		public StockInfo()
		{
			Init();
		}
		private int _sysno;
		private string _stockid;
		private string _stockname;
		private string _address;
		private string _contact;
		private string _phone;
		private int _status;
        private int _stocktype;

		public void Init()
		{
			_sysno = AppConst.IntNull;
			_stockid = AppConst.StringNull;
			_stockname = AppConst.StringNull;
			_address = AppConst.StringNull;
			_contact = AppConst.StringNull;
			_phone = AppConst.StringNull;
			_status = AppConst.IntNull;
            _stocktype = AppConst.IntNull;
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
		public string StockID
		{
			get
			{
				return _stockid;
			}
			set
			{
				_stockid = value;
			}
		}
		public string StockName
		{
			get
			{
				return _stockname;
			}
			set
			{
				_stockname = value;
			}
		}
		public string Address
		{
			get
			{
				return _address;
			}
			set
			{
				_address = value;
			}
		}
		public string Contact
		{
			get
			{
				return _contact;
			}
			set
			{
				_contact = value;
			}
		}
		public string Phone
		{
			get
			{
				return _phone;
			}
			set
			{
				_phone = value;
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
        public int StockType
        {
            get
            {
                return _stocktype;
            }
            set
            {
                _stocktype = value;
            }
        }

		#region IComparable Members

		public int CompareTo(object obj)
		{
			StockInfo b = obj as StockInfo;
			return String.Compare(this.StockID, b.StockID);
		}

		#endregion
	}
}
