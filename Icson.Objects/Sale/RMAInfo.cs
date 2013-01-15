using System;
using System.Collections;
using Icson.Utils;

namespace Icson.Objects.Sale
{
	/// <summary>
	/// Summary description for RMAInfo.
	/// </summary>
	public class RMAInfo
	{
		public RMAInfo()
		{
			Init();
		}		

		public int SysNo;
		public string RMAID;
		public int SOSysNo;
		public int CustomerSysNo;
		public int Status;
		public int AuditUserSysNo;
		public DateTime AuditTime;
		public int ReceiveUserSysNo;
		public DateTime ReceiveTime;
		public int CloseUserSysNo;
		public DateTime CloseTime;
		public int RMAUserSysNo;
		public DateTime RMATime;
		public int LastUserSysNo;
		public DateTime UserChangedTime;
		public string RMANote;
		public string CCNote;
		public string SubmitInfo;
		public string ReceiveInfo;
		public int UserStatus;
		public DateTime CreateTime;

		public Hashtable ItemHash = new Hashtable();
		
		public void Init()
		{
			SysNo = AppConst.IntNull;
			RMAID = AppConst.StringNull;
			SOSysNo = AppConst.IntNull;
			CustomerSysNo = AppConst.IntNull;
			Status = AppConst.IntNull;
			AuditUserSysNo = AppConst.IntNull;
			AuditTime = AppConst.DateTimeNull;
			ReceiveUserSysNo = AppConst.IntNull;
			ReceiveTime = AppConst.DateTimeNull;
			CloseUserSysNo = AppConst.IntNull;
			CloseTime = AppConst.DateTimeNull;
			RMAUserSysNo = AppConst.IntNull;
			RMATime = AppConst.DateTimeNull;
			LastUserSysNo = AppConst.IntNull;
			UserChangedTime = AppConst.DateTimeNull;
			RMANote = AppConst.StringNull;
			CCNote = AppConst.StringNull;
			SubmitInfo = AppConst.StringNull;
			ReceiveInfo = AppConst.StringNull;
			UserStatus = AppConst.IntNull;
			CreateTime = AppConst.DateTimeNull;
		}
	}
}