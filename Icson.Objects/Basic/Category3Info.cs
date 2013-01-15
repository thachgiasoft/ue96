using System;

using Icson.Utils;

namespace Icson.Objects.Basic
{
    /// <summary>
    /// Summary description for Category3Info.
    /// </summary>
    public class Category3Info : IComparable
    {
        public Category3Info()
        {
            Init();
        }

        private int _sysno;
        private int _c2sysno;
        private string _c3id;
        private string _c3name;
        private int _status;
        private int _online;
        private int _c3InventoryCycleTime;
        private decimal _c3DMSWeight;
        private int _c3Type;
        private int _mustHasInvoice;
        private int _maxSafeQty;
        private int _minSafeQty;

        public void Init()
        {
            _sysno = AppConst.IntNull;
            _c2sysno = AppConst.IntNull;
            _c3id = AppConst.StringNull;
            _c3name = AppConst.StringNull;
            _status = AppConst.IntNull;
            _online = AppConst.IntNull;
            _c3InventoryCycleTime = AppConst.IntNull;
            _c3DMSWeight = AppConst.DecimalNull;
            _c3Type = AppConst.IntNull;
            _mustHasInvoice = AppConst.IntNull;
            _maxSafeQty = AppConst.IntNull;
            _minSafeQty = AppConst.IntNull;
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
        public int C2SysNo
        {
            get
            {
                return _c2sysno;
            }
            set
            {
                _c2sysno = value;
            }
        }
        public string C3ID
        {
            get
            {
                return _c3id;
            }
            set
            {
                _c3id = value;
            }
        }
        public string C3Name
        {
            get
            {
                return _c3name;
            }
            set
            {
                _c3name = value;
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
        public int Online
        {
            get
            {
                return _online;
            }
            set
            {
                _online = value;
            }
        }

        public int C3InventoryCycleTime
        {
            get
            {
                return _c3InventoryCycleTime;
            }
            set
            {
                _c3InventoryCycleTime = value;
            }
        }

        public decimal C3DMSWeight
        {
            get
            {
                return _c3DMSWeight;
            }
            set
            {
                _c3DMSWeight = value;
            }
        }

        public int C3Type
        {
            get
            {
                return _c3Type;
            }
            set
            {
                _c3Type = value;
            }
        }

        public int MustHasInvoice
        {
            get
            {
                return _mustHasInvoice;
            }
            set
            {
                _mustHasInvoice = value;
            }
        }

        public override string ToString()
        {
            return "[" + this._c3id + "] " + this._c3name;
        }

        public int MaxSafeQty
        {
            get { return _maxSafeQty; }
            set { _maxSafeQty = value; }
        }
        public int MinSafeQty
        {
            get { return _minSafeQty; }
            set { _minSafeQty = value; }
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            Category3Info b = obj as Category3Info;

            if (this.Status < b.Status)
                return 1;
            else if (this.Status > b.Status)
                return -1;
            else
            {
                int result = String.Compare(this.C3ID, b.C3ID);
                if (result > 0)
                    return 1;
                else
                    return -1;
            }
        }

        #endregion
    }
}
