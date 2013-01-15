using System;
using Icson.Utils;

namespace Icson.Objects.RMA
{
	/// <summary>
	/// Summary description for RMARevertItemInfo.
	/// </summary>
	public class RMARevertItemInfo
	{
		public RMARevertItemInfo()
		{
		    Init();	
		}

		
		public int SysNo;
		public int RevertSysNo;
		public int RegisterSysNo;
		public int StockSysNo;
        public decimal Cost;
		public void Init()
		{
			SysNo         = AppConst.IntNull;
			RevertSysNo   = AppConst.IntNull;
			RegisterSysNo = AppConst.IntNull;
			StockSysNo = AppConst.IntNull;
            Cost       = AppConst.DecimalNull;
		}

	}
}
