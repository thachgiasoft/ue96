using System;

using Icson.Utils;

namespace Icson.Objects.Basic
{
    /// <summary>
    /// Summary description for RequestAccountInfo.
    /// </summary>
    public class RequestAccountInfo 
    {

        public RequestAccountInfo()
        {
            Init();
        }

        private int _sysno;
        private string _name;
        private string _account;
        private string _department;
        private string _phone;
        private string _mobilephone;
        private string _email;
        private string _note;

        public void Init()
        {
            _sysno = AppConst.IntNull;
            _name = AppConst.StringNull;
            _account = AppConst.StringNull;
            _department = AppConst.StringNull;
            _phone = AppConst.StringNull;
            _mobilephone = AppConst.StringNull;
            _email = AppConst.StringNull;
            _note = AppConst.StringNull;
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

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
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

        public string MobilePhone
        {
            get
            {
                return _mobilephone;
            }
            set
            {
                _mobilephone = value;
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

        public string DepartMent
        {
            get
            {
                return _department;
            }
            set
            {
                _department = value;
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
    }
}
