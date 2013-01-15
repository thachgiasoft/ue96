using System;
using System.Collections;
using Icson.Utils;

namespace Icson.Objects.Sale
{
	/// <summary>
	/// Summary description for ROInfo.
	/// </summary>
	public class ROInfo
	{
		public ROInfo()
		{
			Init();
		}
		
		public int SysNo;
		public string ROID;
		public int RMASysNo;
		public int StockSysNo;
		public int Status;
		public decimal OriginCashAmt;
		public int OriginPointAmt;
		public decimal RedeemAmt;
		public decimal CashAmt;
		public int PointAmt;
		public int CreateUserSysNo;
		public DateTime CreateTime;
		public int AuditUserSysNo;
		public DateTime AuditTime;
		public int ReturnUserSysNo;
		public DateTime ReturnTime;
		public string ReceiveName;
		public string ReceiveAddress;
		public string ReceivePhone;
		public string Note;

		public Hashtable ItemHash = new Hashtable();

		public void Init()
		{
			SysNo = AppConst.IntNull;
			ROID = AppConst.StringNull;
			RMASysNo = AppConst.IntNull;
			StockSysNo = AppConst.IntNull;
			Status = AppConst.IntNull;
			OriginCashAmt = AppConst.DecimalNull;
			OriginPointAmt = AppConst.IntNull;
			RedeemAmt = AppConst.DecimalNull;
			CashAmt = AppConst.DecimalNull;
			PointAmt = AppConst.IntNull;
			CreateUserSysNo = AppConst.IntNull;
			CreateTime = AppConst.DateTimeNull;
			AuditUserSysNo = AppConst.IntNull;
			AuditTime = AppConst.DateTimeNull;
			ReturnUserSysNo = AppConst.IntNull;
			ReturnTime = AppConst.DateTimeNull;
			ReceiveName = AppConst.StringNull;
			ReceiveAddress = AppConst.StringNull;
			ReceivePhone = AppConst.StringNull;
			Note = AppConst.StringNull;
		}

		public int GetTotalWeight()
		{
			int totalWeight = 0;
			if(ItemHash.Count!=0)
			{
				foreach(ROItemInfo item in this.ItemHash.Values)
				{
					totalWeight += item.Weight*item.Quantity;
				}
			}
			return totalWeight;
		}
	}
}
