using System;

using Icson.Utils;

namespace Icson.Objects.Basic
{
    /// <summary>
    /// Summary description for VendorInfo.
    /// </summary>
    public class VendorInfo
    {
        public VendorInfo()
        {
            Init();
        }

        private int _sysno;
        private string _vendorid;
        private string _vendorname;
        private string _englishname;
        private string _briefname;
        private int _vendortype;
        private string _district;
        private string _address;
        private string _zip;
        private string _phone;
        private string _fax;
        private string _email;
        private string _site;
        private string _bank;
        private string _account;
        private string _taxno;
        private int _aptype;

        private string _contact;
        private string _comment;
        private string _note;
        private int _status;

     
        private int _warrantyareasysno;
        private string _warrantyaddress;
        private string _warrantyzip;
        private string _warrantycontact;
        private string _warrantyphone;
        private string _warrantyfax;
        private string _warrantyemail;
        private string _warrantysite;
    
        private int _warrantyselfsend;

        private int _repairareasysno;
        private string _repaircontactphone;
        private string _repairaddress;
        private string _repaircontact;
        private string _rmaposition;
        private int _cooperatetype;

        public void Init()
        {
            _sysno = AppConst.IntNull;
            _vendorid = AppConst.StringNull;
            _vendorname = AppConst.StringNull;
            _englishname = AppConst.StringNull;
            _briefname = AppConst.StringNull;
            _vendortype = AppConst.IntNull;
            _district = AppConst.StringNull;
            _address = AppConst.StringNull;
            _zip = AppConst.StringNull;
            _phone = AppConst.StringNull;
            _fax = AppConst.StringNull;
            _email = AppConst.StringNull;
            _site = AppConst.StringNull;
            _bank = AppConst.StringNull;
            _account = AppConst.StringNull;
            _taxno = AppConst.StringNull;
            _aptype = AppConst.IntNull;
            _contact = AppConst.StringNull;
            _comment = AppConst.StringNull;
            _note = AppConst.StringNull;
            _status = AppConst.IntNull;

            //add by Judy
            //------------------------------------
            _warrantyareasysno = AppConst.IntNull;
            _warrantyaddress = AppConst.StringNull;
            _warrantyzip = AppConst.StringNull;
            _warrantycontact = AppConst.StringNull;
            _warrantyphone = AppConst.StringNull;
            _warrantyfax = AppConst.StringNull;
            _warrantyemail = AppConst.StringNull;
            _warrantysite = AppConst.StringNull;
            //------------------------------------
            _warrantyselfsend = AppConst.IntNull;

            _repairareasysno = AppConst.IntNull;
            _repaircontactphone = AppConst.StringNull;
            _repairaddress = AppConst.StringNull;
            _repaircontact = AppConst.StringNull;
            _rmaposition = AppConst.StringNull;

            _cooperatetype = AppConst.IntNull;
        }

        public int SysNo
        {
            get
            {
                return _sysno;
            }
            set
            {
                _sysno = value;
            }
        }
        public string VendorID
        {
            get
            {
                return _vendorid;
            }
            set
            {
                _vendorid = value;
            }
        }
        public string VendorName
        {
            get
            {
                return _vendorname;
            }
            set
            {
                _vendorname = value;
            }
        }
        public string EnglishName
        {
            get
            {
                return _englishname;
            }
            set
            {
                _englishname = value;
            }
        }
        public string BriefName
        {
            get
            {
                return _briefname;
            }
            set
            {
                _briefname = value;
            }
        }
        public int VendorType
        {
            get
            {
                return _vendortype;
            }
            set
            {
                _vendortype = value;
            }
        }
        public string District
        {
            get
            {
                return _district;
            }
            set
            {
                _district = value;
            }
        }
        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
            }
        }
        public string Zip
        {
            get
            {
                return _zip;
            }
            set
            {
                _zip = value;
            }
        }
        public string Phone
        {
            get
            {
                return _phone;
            }
            set
            {
                _phone = value;
            }
        }
        public string Fax
        {
            get
            {
                return _fax;
            }
            set
            {
                _fax = value;
            }
        }
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
            }
        }
        public string Site
        {
            get
            {
                return _site;
            }
            set
            {
                _site = value;
            }
        }
        public string Bank
        {
            get
            {
                return _bank;
            }
            set
            {
                _bank = value;
            }
        }
        public string Account
        {
            get
            {
                return _account;
            }
            set
            {
                _account = value;
            }
        }
        public string TaxNo
        {
            get
            {
                return _taxno;
            }
            set
            {
                _taxno = value;
            }
        }
        public int APType
        {
            get
            {
                return _aptype;
            }
            set
            {
                _aptype = value;
            }
        }
        public string Contact
        {
            get
            {
                return _contact;
            }
            set
            {
                _contact = value;
            }
        }
        public string Comment
        {
            get
            {
                return _comment;
            }
            set
            {
                _comment = value;
            }
        }
        public string Note
        {
            get
            {
                return _note;
            }
            set
            {
                _note = value;
            }
        }
        public int Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }

        public int RepairAreaSysNo
        {
            get { return _repairareasysno; }
            set { _repairareasysno = value; }
        }

        public string RepairContactPhone
        {
            get { return _repaircontactphone; }
            set { _repaircontactphone = value; }
        }

        public string RepairAddress
        {
            get { return _repairaddress; }
            set { _repairaddress = value; }
        }

        public string RepairContact
        {
            get { return _repaircontact; }
            set { _repaircontact = value; }
        }

        //add by judy
        //-------------------------------
        public int WarrantyAreaSysNo
        {
            get { return _warrantyareasysno; }
            set { _warrantyareasysno = value; }
        }

        public string WarrantyAddress
        {
            get { return _warrantyaddress; }
            set { _warrantyaddress = value; }
        }

        public string WarrantyZip
        {
            get { return _warrantyzip; }
            set { _warrantyzip = value; }
        }

        public string WarrantyContact
        {
            get { return _warrantycontact; }
            set { _warrantycontact = value; }
        }
        public string WarrantyPhone
        {
            get { return _warrantyphone; }
            set { _warrantyphone = value; }
        }
        public string WarrantyFax
        {
            get { return _warrantyfax; }
            set { _warrantyfax = value; }
        }
        public string WarrantyEmail
        {
            get { return _warrantyemail; }
            set { _warrantyemail = value; }
        }
        public string WarrantySite
        {
            get { return _warrantysite; }
            set { _warrantysite = value; }
        }
        //-------------------------------
        public int WarrantySelfSend
        {
            get { return _warrantyselfsend; }
            set { _warrantyselfsend = value; }
        }
        public string RMAPosition
        {
            get { return _rmaposition; }
            set {_rmaposition=value;}
        }

        public int CooperateType
        {
            get { return _cooperatetype; }
            set { _cooperatetype = value; }
        }
    }
}