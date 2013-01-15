using System;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// Summary description for ProductInfo.
	/// </summary>
	public class ProductInfo
	{
		public ProductInfo()
		{
			Init();
		}
		public void Init()
		{
			BasicInfo = new ProductBasicInfo();
			PriceInfo = new ProductPriceInfo();
			AttributeInfo = new ProductAttributeInfo();
			StatusInfo = new ProductStatusInfo();
			BarcodeInfo = new ProductBarcodeInfo();
		}
		public ProductBasicInfo BasicInfo;
		public ProductPriceInfo PriceInfo;
		public ProductAttributeInfo AttributeInfo;
		public ProductStatusInfo StatusInfo;
		public ProductBarcodeInfo BarcodeInfo;
	}
}
