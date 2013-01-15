using System;
using Icson.Utils;

namespace Icson.Objects.Sale
{
	/// <summary>
	/// Summary description for SalePointDelayInfo.
	/// </summary>
	public class SalePointDelayInfo
	{
		public SalePointDelayInfo()
		{
			Init();
		}
		
		public int SysNo;
		public int SOSysNo;
		public DateTime CreateTime;
		public int Status;

		public void Init()
		{
			SysNo = AppConst.IntNull;
			SOSysNo = AppConst.IntNull;
			CreateTime = AppConst.DateTimeNull;
			Status = AppConst.IntNull;
		}
	}
}
