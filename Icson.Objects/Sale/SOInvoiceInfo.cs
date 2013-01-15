using System;
using System.Collections;

using Icson.Utils;
using Icson.Objects.Basic;

namespace Icson.Objects.Sale
{
	/// <summary>
	/// Summary description for SOInvoiceInfo.
	/// </summary>
	public class SOInvoiceInfo
	{
		public SOInvoiceInfo()
		{
		}

		#region Define Fields And Properties 
		//Warehouse User Code
		public string WarehouseUserCode;

		//User Name
		public string CustomerName;

		public int ApproverSysNo;

		//Receive Name
		public string ReceiveName;

		//Receive Address
		public string ReceiveAddress;

		//Receive Phone
		public string ReceivePhone;

		//Receive CellPhone
		public string ReceiveCellPhone;

		//Note
		public string InvoiceNote;

		//User SysNo
		public int CustomerSysNo;

		//Pay Type Name
		public string PayTypeName;

		//SO ID
		public string SOID;

		//SO SysNo
		public int SOSysNo;

		//Out Stock ID
		public string OutStockID;

		//Ship Type
		public string ShipTypeName;

		//TotalPage int 
		public int TotalPage;

		//vat
		public int IsVAT;

		//receiveContact
		public string ReceiveContact;

		//totalWeight
		public int TotalWeight;

	    public string AuditDeliveryDateTime;

        public bool HasServiceProduct;

		public string CompanyAddress
		{
			get
			{
				return new IcsonInfo().CompanyAddress;
			}
		}

		//Delivery Memo ---Icson Change
		public string DeliveryMemo;

		#endregion

		public Hashtable ItemPageHash = new Hashtable();
	}
}
