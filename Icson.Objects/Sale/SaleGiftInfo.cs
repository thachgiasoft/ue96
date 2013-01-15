using System;
using Icson.Utils;

namespace Icson.Objects.Sale
{
	/// <summary>
	/// Summary description for SaleGiftInfo.
	/// </summary>
	public class SaleGiftInfo
	{
		public SaleGiftInfo()
		{
			Init();
		}
		
		public int SysNo;
		public int ProductSysNo;
		public int GiftSysNo;
		public string ListOrder;
		public int Status;
		public int CreateUserSysNo;
		public DateTime CreateTime;
		public int AbandonUserSysNo;
		public DateTime AbandonTime;

		public void Init()
		{
			SysNo = AppConst.IntNull;
			ProductSysNo = AppConst.IntNull;
			GiftSysNo = AppConst.IntNull;
			ListOrder = AppConst.StringNull;
			Status = AppConst.IntNull;
			CreateUserSysNo = AppConst.IntNull;
			CreateTime = AppConst.DateTimeNull;
			AbandonUserSysNo = AppConst.IntNull;
			AbandonTime = AppConst.DateTimeNull;
		}
	}
}
