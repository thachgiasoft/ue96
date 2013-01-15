using System;
using Icson.Utils;

namespace Icson.Objects.Online
{
	/// <summary>
	/// Summary description for ProductRemarkInfo.
	/// </summary>
	public class ProductRemarkInfo
	{
		public ProductRemarkInfo()
		{
			Init();
		}

		public int SysNo;
		public int CustomerSysNo;
		public int ProductSysNo;
		public DateTime CreateTime;
		public string Title;
		public string Remark;
		public int Score;
		public int Status;
	    public string OptIP;
		public void Init()
		{
			SysNo = AppConst.IntNull;
			CustomerSysNo = AppConst.IntNull;
			ProductSysNo = AppConst.IntNull;
			CreateTime = AppConst.DateTimeNull;
			Title = AppConst.StringNull;
			Remark = AppConst.StringNull;
			Score = AppConst.IntNull;
			Status = AppConst.IntNull;
		    OptIP = AppConst.StringNull;
		}
	}
}
