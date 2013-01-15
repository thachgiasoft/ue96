using System;
using System.Collections;
using Icson.Utils;


namespace Icson.Objects.RMA
{
	/// <summary>
	/// Summary description for RMAReturnInfo.
	/// </summary>
	public class RMAReturnInfo
	{
		public RMAReturnInfo()
		{
		    Init();	
		}
		
		public int SysNo;
		public string ReturnID;
		public int StockSysNo;
		public DateTime CreateTime;
		public int CreateUserSysNo;
		public DateTime ReturnTime;
		public int ReturnUserSysNo;
		public string Note;
		public int Status;
		public Hashtable ItemHash = new Hashtable(); //key registersysno , value registerinfo;
		public void Init()
		{
			SysNo = AppConst.IntNull;
			ReturnID = AppConst.StringNull;
			StockSysNo = AppConst.IntNull;
			CreateTime = AppConst.DateTimeNull;
			CreateUserSysNo = AppConst.IntNull;
			ReturnTime = AppConst.DateTimeNull;
			ReturnUserSysNo = AppConst.IntNull;
			Note = AppConst.StringNull;
			Status = AppConst.IntNull;
		}

	}
}
