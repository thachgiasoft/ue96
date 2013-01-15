using System;

using Icson.Utils;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// Summary description for ShipAreaPriceInfo.
	/// </summary>
	public class ShipAreaPriceInfo
	{
		public ShipAreaPriceInfo()
		{
			Init();
		}

        public int SysNo;
        public int ShipTypeSysNo;
        public int AreaSysNo;
        public int BaseWeight;
        public int TopWeight;
        public int UnitWeight;
        public decimal UnitPrice;
        public decimal MaxPrice;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            ShipTypeSysNo = AppConst.IntNull;
            AreaSysNo = AppConst.IntNull;
            BaseWeight = AppConst.IntNull;
            TopWeight = AppConst.IntNull;
            UnitWeight = AppConst.IntNull;
            UnitPrice = AppConst.DecimalNull;
            MaxPrice = AppConst.DecimalNull;
        }
	}
}
