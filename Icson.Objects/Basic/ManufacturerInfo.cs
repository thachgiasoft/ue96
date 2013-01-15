using System;

using Icson.Utils;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// Summary description for Manufacturer.
	/// </summary>
	public class ManufacturerInfo : IComparable
	{
		public ManufacturerInfo()
		{
			Init();
		}

		private int _sysno;
		private string _manufacturerid;
		private string _manufacturername;
		private string _briefname;
		private string _note;
		private int _status;

		public void Init()
		{
			_sysno = AppConst.IntNull;
			_manufacturerid = AppConst.StringNull;
			_manufacturername = AppConst.StringNull;
			_briefname = AppConst.StringNull;
			_note = AppConst.StringNull;
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
		public string ManufacturerID
		{
			get
			{
				return _manufacturerid;
			}
			set
			{
				_manufacturerid = value;
			}
		}
		public string ManufacturerName
		{
			get
			{
				return _manufacturername;
			}
			set
			{
				_manufacturername = value;
			}
		}
		public string BriefName
		{
			get
			{
				return _briefname;
			}
			set
			{
				_briefname = value;
			}
		}
		public string Note
		{
			get
			{
				return _note;
			}
			set
			{
				_note = value;
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
			ManufacturerInfo b = obj as ManufacturerInfo;

			int compare = String.Compare(this.BriefName, b.BriefName);
			if ( compare == 0 )
				compare = 1;

			return compare;
		}

		#endregion
	}
}
