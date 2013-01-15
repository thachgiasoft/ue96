using System;

using Icson.Utils;

namespace Icson.Objects.Stock
{
	/// <summary>
	/// Summary description for TransferItemInfo.
	/// </summary>
	public class TransferItemInfo
	{
		public TransferItemInfo()
		{
			Init();
		}
		public int SysNo;
		public int TransferSysNo;
		public int ProductSysNo;
		public int TransferType;
		public int TransferQty;
		public decimal TransferCost;
		public void Init()
		{
			SysNo = AppConst.IntNull;
			TransferSysNo = AppConst.IntNull;
			ProductSysNo = AppConst.IntNull;
			TransferType = AppConst.IntNull;
			TransferQty = AppConst.IntNull;
			TransferCost = AppConst.DecimalNull;
		}
	}
}
