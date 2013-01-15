using System;

using Icson.Utils;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// Summary description for InventoryInfo.
	/// </summary>
	public class InventoryInfo
	{
		public InventoryInfo()
		{
			Init();
		}
		private int _sysno;
		private int _productsysno;
		private int _accountqty;
		private int _availableqty;
		private int _allocatedqty;
		private int _orderqty;
		private int _purchaseqty;
		private int _virtualqty;

		public void Init()
		{
			_sysno = AppConst.IntNull;
			_productsysno = AppConst.IntNull;
			_accountqty = AppConst.IntNull;
			_availableqty = AppConst.IntNull;
			_allocatedqty = AppConst.IntNull;
			_orderqty = AppConst.IntNull;
			_purchaseqty = AppConst.IntNull;
			_virtualqty = AppConst.IntNull;
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
		public int VirtualQty
		{
			get
			{
				return _virtualqty;
			}
			set
			{
				_virtualqty = value;
			}
		}

		public int GetOnlineQty()
		{
			return this._availableqty+this._virtualqty;
		}
	}
}
