using System;
using Icson.Utils;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// Summary description for ProductStatusInfo.
	/// </summary>
	public class ProductStatusInfo
	{
		public ProductStatusInfo()
		{
			Init();
		}		
		public int SysNo;
		public int ProductSysNo;
		public int InfoStatus;
		public int InfoUserSysNo;
		public DateTime InfoTime;
		public int PicStatus;
		public int PicUserSysNo;
		public DateTime PicTime;
		public int WarrantyStatus;
		public int WarrantyUserSysNo;
		public DateTime WarrantyTime;
		public int PriceStatus;
		public int PriceUserSysNo;
		public DateTime PriceTime;
		public int WeightStatus;
		public int WeightUserSysNo;
		public DateTime WeightTime;
		public int AllowStatus;
		public int AllowUserSysNo;
		public DateTime AllowTime;
        public int PreviewStatus;
        public int PreviewUserSysNo;
        public DateTime PreviewTime;

		public void Init()
		{
			SysNo = AppConst.IntNull;
			ProductSysNo = AppConst.IntNull;
			InfoStatus = AppConst.IntNull;
			InfoUserSysNo = AppConst.IntNull;
			InfoTime = AppConst.DateTimeNull;
			PicStatus = AppConst.IntNull;
			PicUserSysNo = AppConst.IntNull;
			PicTime = AppConst.DateTimeNull;
			WarrantyStatus = AppConst.IntNull;
			WarrantyUserSysNo = AppConst.IntNull;
			WarrantyTime = AppConst.DateTimeNull;
			PriceStatus = AppConst.IntNull;
			PriceUserSysNo = AppConst.IntNull;
			PriceTime = AppConst.DateTimeNull;
			WeightStatus = AppConst.IntNull;
			WeightUserSysNo = AppConst.IntNull;
			WeightTime = AppConst.DateTimeNull;
			AllowStatus = AppConst.IntNull;
			AllowUserSysNo = AppConst.IntNull;
			AllowTime = AppConst.DateTimeNull;
            PreviewStatus = AppConst.IntNull;
            PreviewUserSysNo = AppConst.IntNull;
            PreviewTime = AppConst.DateTimeNull;
		}
	}
}