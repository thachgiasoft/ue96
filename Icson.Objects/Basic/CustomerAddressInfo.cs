using System;
using Icson.Utils;

namespace Icson.Objects.Basic
{
    public class CustomerAddressInfo
    {
        public CustomerAddressInfo()
		{
			Init();
		}

        public int SysNo;
        public int CustomerSysNo;
        public string Brief;
        public string Name;
        public string Contact;
        public string Phone;
        public string CellPhone;
        public string Fax;
        public string Address;
        public string Zip;
        public int AreaSysNo;
        public int IsDefault;
        public DateTime UpdateTime;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            CustomerSysNo = AppConst.IntNull;
            Brief = AppConst.StringNull;
            Name = AppConst.StringNull;
            Contact = AppConst.StringNull;
            Phone = AppConst.StringNull;
            CellPhone = AppConst.StringNull;
            Fax = AppConst.StringNull;
            Address = AppConst.StringNull;
            Zip = AppConst.StringNull;
            AreaSysNo = AppConst.IntNull;
            IsDefault = AppConst.IntNull;
            UpdateTime = AppConst.DateTimeNull;
        }
    }
}