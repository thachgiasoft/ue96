using System;
using System.Collections;

using Icson.Utils;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// ProductBarcodeInfo 的摘要说明。
	/// </summary>
	public class ProductBarcodeInfo
	{
		public ProductBarcodeInfo()
		{
			Init();
		}

		public int SysNo;
		public int ProductSysNo;
		public string Barcode;
		public DateTime DateStamp;

		public void Init()
		{
			SysNo = AppConst.IntNull;
			ProductSysNo = AppConst.IntNull;
			Barcode = AppConst.StringNull;
			DateStamp = AppConst.DateTimeNull;
		}
	}
}