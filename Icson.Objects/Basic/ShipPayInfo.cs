using System;

using Icson.Utils;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// Summary description for ShipPayInfo.
	/// </summary>
	public class ShipPayInfo
	{
		public ShipPayInfo()
		{
			Init();
		}
        public int SysNo;
        public int ShipTypeSysNo;
        public int PayTypeSysNo;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            ShipTypeSysNo = AppConst.IntNull;
            PayTypeSysNo = AppConst.IntNull;
        }
	}
}
