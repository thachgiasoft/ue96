using System;
using System.Collections;

using Icson.Utils;

namespace Icson.Objects.Stock
{
	/// <summary>
	/// Summary description for LendInfo.
	/// </summary>
	public class LendInfo
	{
		public LendInfo()
		{
			Init();
		}

		public Hashtable itemHash = new Hashtable(5);
		public Hashtable returnHash = new Hashtable(5);
		
		public int SysNo;
		public string LendID;
		public int StockSysNo;
		public int LendUserSysNo;
		public DateTime CreateTime;
		public int CreateUserSysNo;
		public DateTime AuditTime;
		public int AuditUserSysNo;
		public DateTime OutTime;
		public int OutUserSysNo;
		public int Status;
		public string Note;
		public void Init()
		{
			SysNo = AppConst.IntNull;
			LendID = AppConst.StringNull;
			StockSysNo = AppConst.IntNull;
			LendUserSysNo = AppConst.IntNull;
			CreateTime = AppConst.DateTimeNull;
			CreateUserSysNo = AppConst.IntNull;
			AuditTime = AppConst.DateTimeNull;
			AuditUserSysNo = AppConst.IntNull;
			OutTime = AppConst.DateTimeNull;
			OutUserSysNo = AppConst.IntNull;
			Status = AppConst.IntNull;
			Note = AppConst.StringNull;
		}

	}
}
