using System;
using Icson.Utils;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// ProductPriceRangeInfo ��ժҪ˵����
	/// </summary>
	public class ProductPriceRangeInfo
	{
		public ProductPriceRangeInfo()
		{
			Init();
		}

		public int SysNo;
		public int RangeID;
		public string RangeName;
		public int RangeFrom;
		public int RangeTo;
		public void Init()
		{
			SysNo = AppConst.IntNull;
			RangeID = AppConst.IntNull;
			RangeName = AppConst.StringNull;
			RangeFrom = AppConst.IntNull;
			RangeTo = AppConst.IntNull;
		}
	}
}