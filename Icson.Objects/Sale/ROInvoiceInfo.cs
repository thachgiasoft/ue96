using System;
using System.Collections;

namespace Icson.Objects.Sale
{
	/// <summary>
	/// Summary description for ROInvoiceInfo.
	/// </summary>
	public class ROInvoiceInfo
	{
		public ROInvoiceInfo()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public Hashtable ItemHash = new Hashtable(5);

		public string CustomerName;
	
		public string ROID;
		
		public string CompanyAddress;
		
		public string ShipTypeName;
		
		public string SOID;
		
		public string PayTypeName;
		
		public int CustomerSysNo;
		
		public string InvoiceNote;
		
		public string ReceivePhone;
		
		public string ReceiveAddress;
		
		public string ReceiveName;
		
		public int AuditUserSysNo;
		
		public string WarehouseUserCode;

		public int ROSysNo;

		public int TotalPage;

		public int TotalWeight;


		public ROInvoicePageInfo GetPage(int index)
		{
			if(index >= ItemHash.Count || index < 0)
			{
				return null;
			}
			return (ROInvoicePageInfo)ItemHash[index];
		}
	}
}
