using System;
using System.Collections;

using Icson.Utils;

namespace Icson.Objects.RMA
{
	/// <summary>
	/// Summary description for RMARevertInfo.
	/// </summary>
	public class RMARevertInfo
	{
		public RMARevertInfo()
		{
		    Init();	
		}

		
		public int SysNo;
		public string RevertID;
		public int CustomerSysNo;
		public DateTime CreateTime;
		public int CreateUserSysNo;
		public string ZIP;
		public string Address;
		public string Contact;
		public string Phone;
		public int ShipType;
		public DateTime OutTime;
		public int OutUserSysNo;
		public string Note;
		public int Status;
		public int SOSysNo ;
        public int LocationStatus;
        public int AddressAreaSysNo;
        public string PackageID;
        public int FreightUserSysNo;
        public DateTime SetDeliveryManTime;
        public int CheckQtyUserSysNo;
        public DateTime CheckQtyTime;
        public int IsPrintPackageCover;
        public int IsConfirmAddress;

        public Hashtable ItemHash = new Hashtable(); //key registersysno , value registerinfo;
		public void Init()
		{
			SysNo = AppConst.IntNull;
			RevertID = AppConst.StringNull;
			CustomerSysNo = AppConst.IntNull;
			CreateTime = AppConst.DateTimeNull;
			CreateUserSysNo = AppConst.IntNull;
			ZIP = AppConst.StringNull;
			Address = AppConst.StringNull;
			Contact = AppConst.StringNull;
			Phone = AppConst.StringNull;
			ShipType = AppConst.IntNull;
			OutTime = AppConst.DateTimeNull;
			OutUserSysNo = AppConst.IntNull;
			Note = AppConst.StringNull;
			Status = AppConst.IntNull;
			SOSysNo = AppConst.IntNull ;
            LocationStatus = AppConst.IntNull;
            AddressAreaSysNo = AppConst.IntNull;
            PackageID = AppConst.StringNull;
            FreightUserSysNo = AppConst.IntNull;
            SetDeliveryManTime = AppConst.DateTimeNull;
            CheckQtyUserSysNo = AppConst.IntNull;
            CheckQtyTime = AppConst.DateTimeNull;
            IsPrintPackageCover = AppConst.IntNull;
            IsConfirmAddress = AppConst.IntNull;
		}
	}
}