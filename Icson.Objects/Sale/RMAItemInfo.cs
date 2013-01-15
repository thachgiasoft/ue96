using System;
using Icson.Utils;

namespace Icson.Objects.Sale
{
	/// <summary>
	/// Summary description for RMAItemInfo.
	/// </summary>
	public class RMAItemInfo
	{
		public RMAItemInfo()
		{
			Init();
		}
		
		public int SysNo;
		public int RMASysNo;
		public int ProductSysNo;
		public int RMAType;
		public int RMAQty;
		public string RMADesc;
	    public string SOItemPODesc;

		public void Init()
		{
			SysNo = AppConst.IntNull;
			RMASysNo = AppConst.IntNull;
			ProductSysNo = AppConst.IntNull;
			RMAType = AppConst.IntNull;
			RMAQty = AppConst.IntNull;
			RMADesc = AppConst.StringNull;
		    SOItemPODesc = AppConst.StringNull;
		}
	}
}
