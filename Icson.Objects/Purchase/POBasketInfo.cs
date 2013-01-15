using System;

using Icson.Utils;

namespace Icson.Objects.Purchase
{
	/// <summary>
	/// Summary description for POBasketInfo.
	/// </summary>
	public class POBasketInfo
	{
		public POBasketInfo()
		{
			Init();
		}

		
		public int SysNo;
		public int CreateUserSysNo;
		public int ProductSysNo;
		public int Quantity;
		public decimal OrderPrice;
		public void Init()
		{
			SysNo = AppConst.IntNull;
			CreateUserSysNo = AppConst.IntNull;
			ProductSysNo = AppConst.IntNull;
			Quantity = AppConst.IntNull;
			OrderPrice = AppConst.DecimalNull;
		}
	}
}
