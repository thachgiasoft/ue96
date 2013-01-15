using System;
using System.Collections;

using Icson.Utils;

namespace Icson.Objects.RMA
{
	/// <summary>
	/// Summary description for RMARefundInfo.
	/// </summary>
	public class RMARefundInfo
	{
		public RMARefundInfo()
		{
			Init();
		}

		public Hashtable ItemHash = new Hashtable(5); //key registerSysNo, value rmaRufundItemInfo
		
		public int SysNo;
		public string RefundID;
		public int SOSysNo;
		public int CustomerSysNo;
		public DateTime CreateTime;
		public int CreateUserSysNo;
		public DateTime AuditTime;
		public int AuditUserSysNo;
		public DateTime RefundTime;
		public int RefundUserSysNo;
		public decimal CompensateShipPrice;
		public decimal SOCashPointRate;
		public decimal OrgCashAmt;
		public int OrgPointAmt;
		public int DeductPointFromAccount;
		public decimal DeductPointFromCurrentCash;
		public decimal CashAmt;
		public int PointAmt;
        public int RefundPayType;
		public string Note;
		public int Status;

		public void Init()
		{
			SysNo = AppConst.IntNull;
			RefundID = AppConst.StringNull;
			SOSysNo = AppConst.IntNull;
			CustomerSysNo = AppConst.IntNull;
			CreateTime = AppConst.DateTimeNull;
			CreateUserSysNo = AppConst.IntNull;
			AuditTime = AppConst.DateTimeNull;
			AuditUserSysNo = AppConst.IntNull;
			RefundTime = AppConst.DateTimeNull;
			RefundUserSysNo = AppConst.IntNull;
			CompensateShipPrice = AppConst.DecimalNull;
			SOCashPointRate = AppConst.DecimalNull;
			OrgCashAmt = AppConst.DecimalNull;
			OrgPointAmt = AppConst.IntNull;
			DeductPointFromAccount = AppConst.IntNull;
			DeductPointFromCurrentCash = AppConst.DecimalNull;
			CashAmt = AppConst.DecimalNull;
			PointAmt = AppConst.IntNull;
            RefundPayType = AppConst.IntNull;
			Note = AppConst.StringNull;
			Status = AppConst.IntNull;

			ItemHash.Clear();
		}


	}
}
