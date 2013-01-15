using System;
using System.Collections;
using Icson.Utils;

namespace Icson.Objects.RMA
{
	/// <summary>
	/// Summary description for RMARequestInfo.
	/// </summary>
	public class RMARequestInfo
	{
		public RMARequestInfo()
		{
			Init();
		}

        public int SysNo;
        public int SOSysNo;
        public string RequestID;
        public DateTime CreateTime;
        public int CustomerSysNo;
        public int CreateUserSysNo;
        public int CanDoorGet;
        public decimal DoorGetFee;
        public string Address;
        public string Contact;
        public string Phone;
        public string Zip;
        public DateTime RecvTime;
        public int RecvUserSysNo;
        public string Note;
        public string Memo;
        public int Status;
        public DateTime ETakeDate;
        public int AreaSysNo;
        public DateTime CustomerSendTime;
        public int IsRejectRMA;
        public int FreightUserSysNo;
        public DateTime SetDeliveryManTime;
        public int IsRevertAddress;
        public string RevertAddress;
        public int RevertAreaSysNo;
        public string RevertZip;
        public string RevertContact;
        public string RevertContactPhone;
        public int ReceiveType;

        
		public Hashtable ItemHash = new Hashtable(); //key registersysno , value registerinfo;

		public void Init()
		{
            SysNo = AppConst.IntNull;
            SOSysNo = AppConst.IntNull;
            RequestID = AppConst.StringNull;
            CreateTime = AppConst.DateTimeNull;
            CustomerSysNo = AppConst.IntNull;
            CreateUserSysNo = AppConst.IntNull;
            CanDoorGet = AppConst.IntNull;
            DoorGetFee = AppConst.DecimalNull;
            Address = AppConst.StringNull;
            Contact = AppConst.StringNull;
            Phone = AppConst.StringNull;
            Zip = AppConst.StringNull;
            RecvTime = AppConst.DateTimeNull;
            RecvUserSysNo = AppConst.IntNull;
            Note = AppConst.StringNull;
            Memo = AppConst.StringNull;
            Status = AppConst.IntNull;
            ETakeDate = AppConst.DateTimeNull;
            AreaSysNo = AppConst.IntNull;
            CustomerSendTime = AppConst.DateTimeNull;
            IsRejectRMA = AppConst.IntNull;
            FreightUserSysNo = AppConst.IntNull;
            SetDeliveryManTime = AppConst.DateTimeNull;
            IsRevertAddress = AppConst.IntNull;
            RevertAddress = AppConst.StringNull;
            RevertAreaSysNo = AppConst.IntNull;
            RevertZip = AppConst.StringNull;
            RevertContact = AppConst.StringNull;
            RevertContactPhone = AppConst.StringNull;
            ReceiveType = AppConst.IntNull;
        }
	}
}
