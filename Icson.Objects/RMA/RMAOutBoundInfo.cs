using System;
using System.Collections;

using Icson.Utils;

namespace Icson.Objects.RMA
{
	/// <summary>
	/// Summary description for RMAOutBoundInfo.
	/// </summary>
	public class RMAOutBoundInfo
	{
		public RMAOutBoundInfo()
		{
			Init();
		}

		public int SysNo;
		public string OutBoundID;
		public int VendorSysNo;
		public DateTime CreateTime;
		public int CreateUserSysNo;
		public string VendorName;
		public string ZIP;
		public string Address;
		public string Contact;
		public string Phone;
		public DateTime OutTime;
		public int OutUserSysNo;
		public string Note;
		public int Status;
        public DateTime EOutBoundDate;
        public DateTime EResponseDate;
        public int AreaSysNo;
        public int OutBoundInvoiceQty;
        public int ShipType;
        public string PackageID;
        public int CheckQtyUserSysNo;
        public DateTime CheckQtyTime;
        public int FreightUserSysNo;
        public DateTime SetDeliveryManTime;

		public Hashtable itemHash = new Hashtable(5); //key registerSysNo, value regiserInfo;

		public void Init()
		{
			SysNo = AppConst.IntNull;
			OutBoundID = AppConst.StringNull;
			VendorSysNo = AppConst.IntNull;
			CreateTime = AppConst.DateTimeNull;
			CreateUserSysNo = AppConst.IntNull;
			VendorName = AppConst.StringNull;
            ZIP = AppConst.StringNull;
			Address = AppConst.StringNull;
			Contact = AppConst.StringNull;
			Phone = AppConst.StringNull;
			OutTime = AppConst.DateTimeNull;
			OutUserSysNo = AppConst.IntNull;
			Note = AppConst.StringNull;
			Status = AppConst.IntNull;
            EOutBoundDate = AppConst.DateTimeNull;
            EResponseDate = AppConst.DateTimeNull;
            AreaSysNo = AppConst.IntNull;
            OutBoundInvoiceQty = AppConst.IntNull;
            ShipType = AppConst.IntNull;
            PackageID = AppConst.StringNull;
            CheckQtyUserSysNo = AppConst.IntNull;
            CheckQtyTime = AppConst.DateTimeNull;
            FreightUserSysNo = AppConst.IntNull;
            SetDeliveryManTime = AppConst.DateTimeNull;
			itemHash.Clear();
		}
	}
}
