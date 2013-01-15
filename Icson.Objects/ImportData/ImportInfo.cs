using System;

using Icson.Utils;

namespace Icson.Objects.ImportData
{
	/// <summary>
	/// Summary description for ImportInfo.
	/// </summary>
	public class ImportInfo
	{
		public ImportInfo()
		{
			Init();
		}
		private int _sysno;
		private int _oldsysno;
		private int _newsysno;

		public void Init()
		{
			_sysno = AppConst.IntNull;
			_oldsysno = AppConst.IntNull;
			_newsysno = AppConst.IntNull;
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
		public int OldSysNo
		{
			get
			{
				return _oldsysno;
			}
			set
			{
				_oldsysno = value;
			}
		}
		public int NewSysNo
		{
			get
			{
				return _newsysno;
			}
			set
			{
				_newsysno = value;
			}
		}
	}
}
