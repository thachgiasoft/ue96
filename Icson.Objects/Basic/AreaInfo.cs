using System;

using Icson.Utils;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// Summary description for AreaInfo.
	/// </summary>
	public class AreaInfo : IComparable
	{
		public AreaInfo()
		{
			Init();
		}

		private int _sysno;
		private int _provincesysno;
		private int _citysysno;
		private string _provincename;
		private string _cityname;
		private string _districtname;
		private string _ordernumber;
		private int _islocal;
		private int _status;
        private int _localcode;

		public void Init()
		{
			_sysno = AppConst.IntNull;
			_provincesysno = AppConst.IntNull;
			_citysysno = AppConst.IntNull;
			_provincename = AppConst.StringNull;
			_cityname = AppConst.StringNull;
			_districtname = AppConst.StringNull;
			_ordernumber = AppConst.StringNull;
			_islocal = AppConst.IntNull;
			_status = AppConst.IntNull;
		    _localcode = AppConst.IntNull;
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
		public int ProvinceSysNo
		{
			get
			{
				return _provincesysno;
			}
			set
			{
				_provincesysno = value;
			}
		}
		public int CitySysNo
		{
			get
			{
				return _citysysno;
			}
			set
			{
				_citysysno = value;
			}
		}
		public string ProvinceName
		{
			get
			{
				return _provincename;
			}
			set
			{
				_provincename = value;
			}
		}
		public string CityName
		{
			get
			{
				return _cityname;
			}
			set
			{
				_cityname = value;
			}
		}
		public string DistrictName
		{
			get
			{
				return _districtname;
			}
			set
			{
				_districtname = value;
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
		public int IsLocal
		{
			get
			{
				return _islocal;
			}
			set
			{
				_islocal = value;
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

	    public int LocalCode
	    {
	        get { return _localcode;}
            set { _localcode = value;}
	    }

		public string GetWholeName()
		{
			if ( CitySysNo != AppConst.IntNull )
				return ProvinceName + "," + CityName + "," + DistrictName;
			else if ( ProvinceSysNo != AppConst.IntNull )
				return ProvinceName + "," + CityName;
			else
				return ProvinceName;
		}	

		public string AreaName
		{
			get
			{
				if ( CitySysNo != AppConst.IntNull )
					return DistrictName;
				else if ( ProvinceSysNo != AppConst.IntNull )
					return CityName;
				else
					return ProvinceName;
			}
			set
			{
				if ( CitySysNo != AppConst.IntNull )
					DistrictName = value;
				else if ( ProvinceSysNo != AppConst.IntNull )
					CityName = value;
				else
					ProvinceName = value;
			}
		}

		#region IComparable Members

		public int CompareTo(object obj)
		{
			AreaInfo b = obj as AreaInfo;
            //if ( String.Compare(this.OrderNumber, b.OrderNumber) >= 0 )
            //    return 1;
            //else
            //    return -1;
            if (string.Compare(this.DistrictName, b.DistrictName) >= 0)
                return 1;
            else
                return -1;
		}

		#endregion
	}
}
