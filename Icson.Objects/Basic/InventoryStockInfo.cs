using System;

using Icson.Utils;

namespace Icson.Objects.Basic
{
    /// <summary>
    /// Summary description for InventoryStockInfo.
    /// </summary>
    public class InventoryStockInfo
    {
        public InventoryStockInfo()
        {
            Init();
        }
        private int _sysno;
        private int _stocksysno;
        private int _productsysno;
        private int _accountqty;
        private int _availableqty;
        private int _allocatedqty;
        private int _orderqty;
        private int _purchaseqty;
        private int _shiftinqty;
        private int _shiftoutqty;
        private int _safeqty;
        private string _position1;
        private string _position2;
        private DateTime _poslastupdatetime;

        public void Init()
        {
            _sysno = AppConst.IntNull;
            _stocksysno = AppConst.IntNull;
            _productsysno = AppConst.IntNull;
            _accountqty = AppConst.IntNull;
            _availableqty = AppConst.IntNull;
            _allocatedqty = AppConst.IntNull;
            _orderqty = AppConst.IntNull;
            _purchaseqty = AppConst.IntNull;
            _shiftinqty = AppConst.IntNull;
            _shiftoutqty = AppConst.IntNull;
            _safeqty = AppConst.IntNull;
            _position1 = AppConst.StringNull;
            _position2 = AppConst.StringNull;
            _poslastupdatetime = AppConst.DateTimeNull;
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
        public int StockSysNo
        {
            get
            {
                return _stocksysno;
            }
            set
            {
                _stocksysno = value;
            }
        }
        public int ProductSysNo
        {
            get
            {
                return _productsysno;
            }
            set
            {
                _productsysno = value;
            }
        }
        public int AccountQty
        {
            get
            {
                return _accountqty;
            }
            set
            {
                _accountqty = value;
            }
        }
        public int AvailableQty
        {
            get
            {
                return _availableqty;
            }
            set
            {
                _availableqty = value;
            }
        }
        public int AllocatedQty
        {
            get
            {
                return _allocatedqty;
            }
            set
            {
                _allocatedqty = value;
            }
        }
        public int OrderQty
        {
            get
            {
                return _orderqty;
            }
            set
            {
                _orderqty = value;
            }
        }
        public int PurchaseQty
        {
            get
            {
                return _purchaseqty;
            }
            set
            {
                _purchaseqty = value;
            }
        }
        public int ShiftInQty
        {
            get
            {
                return _shiftinqty;
            }
            set
            {
                _shiftinqty = value;
            }
        }
        public int ShiftOutQty
        {
            get
            {
                return _shiftoutqty;
            }
            set
            {
                _shiftoutqty = value;
            }
        }
        public int SafeQty
        {
            get
            {
                return _safeqty;
            }
            set
            {
                _safeqty = value;
            }
        }
        public string Position1
        {
            get
            {
                return _position1;
            }
            set
            {
                _position1 = value;
            }
        }
        public string Position2
        {
            get
            {
                return _position2;
            }
            set
            {
                _position2 = value;
            }
        }

        public DateTime PosLastUpdateTime
        {
            get
            {
                return _poslastupdatetime;
            }
            set
            {
                _poslastupdatetime = value;
            }
        }
    }
}
