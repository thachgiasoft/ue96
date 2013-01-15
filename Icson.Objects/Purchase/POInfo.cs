using System;
using System.Collections;

using Icson.Utils;

namespace Icson.Objects.Purchase
{
	/// <summary>
	/// Summary description for POInfo.
	/// </summary>
	public class POInfo
	{
		public POInfo()
		{
			Init();
		}

		//item µÄÃ÷Ï¸
		public Hashtable itemHash = new Hashtable(10); // key: productsysno, value: poiteminfo
		public Hashtable apportionHash = new Hashtable(10); //key: poapportioninfo

		
		public int SysNo;
		public string POID;
		public int VendorSysNo;
		public int StockSysNo;
		public int ShipTypeSysNo;
		public int PayTypeSysNo;
		public int CurrencySysNo;
		public decimal ExchangeRate;
		public decimal TotalAmt;
		public DateTime CreateTime;
		public int CreateUserSysNo;
		public DateTime AuditTime;
		public int AuditUserSysNo;
        public DateTime ReceiveTime;
        public int ReceiveUserSysNo;
		public DateTime InTime;
		public int InUserSysNo;
		public int IsApportion;
		public DateTime ApportionTime;
		public int ApportionUserSysNo;
        public DateTime PayDate;
		public string Memo;
		public string Note;
		public int Status;
	    public int POType;
	    public int POInvoiceType;
        public string ManagerAuditMemo;
        public int ManagerAuditStatus;
        public string POInvoiceDunDesc;
        public DateTime POInvoiceDunTime;
        public DateTime InvoiceExpectReceiveDate;
        public int POShipTypeSysNo;
        public DateTime DeliveryDate;
        public int CustomerSysNo;
        public int MinusPOType;

		public void Init()
		{
			SysNo = AppConst.IntNull;
			POID = AppConst.StringNull;
			VendorSysNo = AppConst.IntNull;
			StockSysNo = AppConst.IntNull;
			ShipTypeSysNo = AppConst.IntNull;
			PayTypeSysNo = AppConst.IntNull;
			CurrencySysNo = AppConst.IntNull;
			ExchangeRate = AppConst.DecimalNull;
			TotalAmt = AppConst.DecimalNull;
			CreateTime = AppConst.DateTimeNull;
			CreateUserSysNo = AppConst.IntNull;
			AuditTime = AppConst.DateTimeNull;
			AuditUserSysNo = AppConst.IntNull;
            ReceiveTime = AppConst.DateTimeNull;
            ReceiveUserSysNo = AppConst.IntNull;
			InTime = AppConst.DateTimeNull;
			InUserSysNo = AppConst.IntNull;
			IsApportion = AppConst.IntNull;
			ApportionTime = AppConst.DateTimeNull;
			ApportionUserSysNo = AppConst.IntNull;
            PayDate = AppConst.DateTimeNull;
			Memo = AppConst.StringNull;
			Note = AppConst.StringNull;
			Status = AppConst.IntNull;
		    POType = AppConst.IntNull;
		    POInvoiceType = AppConst.IntNull;
            ManagerAuditMemo = AppConst.StringNull;
            ManagerAuditStatus = AppConst.IntNull;
            POInvoiceDunDesc = AppConst.StringNull;
            InvoiceExpectReceiveDate = AppConst.DateTimeNull;
            POShipTypeSysNo = AppConst.IntNull;
            DeliveryDate = AppConst.DateTimeNull;
            CustomerSysNo = AppConst.IntNull;
            MinusPOType = AppConst.IntNull;
			itemHash.Clear();

			foreach(POApportionInfo item in apportionHash.Keys)
			{
				item.Init();
			}
			apportionHash.Clear();
		}

		public decimal GetTotalAmt()
		{
			decimal totalAmt = 0;
			foreach(POItemInfo item in itemHash.Values)
			{
				totalAmt += item.OrderPrice * item.Quantity;
			}
			return totalAmt;
		}

	}
}
