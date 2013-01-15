using System;
using System.Collections;

using Icson.Utils;

namespace Icson.Objects.Stock
{
	/// <summary>
	/// Summary description for TransferInfo.
	/// </summary>
	public class TransferInfo
	{
		public TransferInfo()
		{
			Init();
		}

		public Hashtable itemHash = new Hashtable(5);

		public int SysNo;
		public string TransferID;
		public int StockSysNo;
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
			TransferID = AppConst.StringNull;
			StockSysNo = AppConst.IntNull;
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
