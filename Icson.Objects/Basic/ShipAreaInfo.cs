using System;

using Icson.Utils;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// Summary description for ShipAreaInfo.
	/// </summary>
	public class ShipAreaInfo
	{
		public ShipAreaInfo()
		{
			Init();
		}
        public int SysNo;
        public int ShipTypeSysNo;
        public int AreaSysNo;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            ShipTypeSysNo = AppConst.IntNull;
            AreaSysNo = AppConst.IntNull;
        }
	}
}
