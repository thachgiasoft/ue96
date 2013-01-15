using System;
using Icson.Utils;

namespace Icson.Objects.Basic
{
    public class MenuInfo
    {
        public MenuInfo()
        {
            Init();
        }

        private int _sysno;
        private int _menuid;
        private int _parentid;
        private string _name;
        private string _description;

        private void Init()
        {
            _sysno = AppConst.IntNull;
            _menuid = AppConst.IntNull;
            _parentid = AppConst.IntNull;
            _name = AppConst.StringNull;
            _description = AppConst.StringNull;
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

        public int MenuID
        {
            get
            {
                return _menuid;
            }
            set
            {
                _menuid = value;
            }
        }

        public int ParentID
        {
            get
            {
                return _parentid;
            }
            set
            {
                _parentid = value;
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

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

    }
}
