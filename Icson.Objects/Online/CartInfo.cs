using System;

using Icson.Utils;

namespace Icson.Objects.Online
{
	/// <summary>
	/// Summary description for CartInfo.
	/// </summary>
	public class CartInfo
	{
		public CartInfo()
		{
			Init();
		}		
		public int ProductSysNo;
		public int Quantity;
        public int ExpectQty;

		public void Init()
		{
			ProductSysNo = AppConst.IntNull;
			Quantity = AppConst.IntNull;
            ExpectQty = AppConst.IntNull;
		}
	}
}
