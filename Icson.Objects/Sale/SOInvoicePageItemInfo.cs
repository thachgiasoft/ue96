using System;
using Icson.Utils;
using Icson.Objects;
namespace Icson.Objects.Sale
{
	/// <summary>
	/// Summary description for SOInvoicePageItemInfo.
	/// </summary>
	public class SOInvoicePageItemInfo
	{
		public SOInvoicePageItemInfo()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        public int ProductSysNo;

		public string ProductID;

		public string ProductName;

		public decimal Price;

		public int Quantity;

		public decimal SubTotal;

		public string Warranty;

		public int Weight;

		public bool IsSOItem;

		public bool IsPoint;

		public string GetPrintQuantity()
		{
			if(this.IsSOItem == true)
			{
				return this.Quantity.ToString();
			}
			else
			{
				return "";
			}
		}

		public int PriceInPoint
		{
			get
			{
				return Convert.ToInt32(Decimal.Round( Convert.ToDecimal(this.Price * AppConst.ExchangeRate),0));
			}
		}

		public string GetPrintPrice()
		{
			if(this.IsSOItem == true)
			{
				return this.Price.ToString(AppConst.DecimalFormat);
			}
			else
			{
				return "";
			}
		}

		public string GetPrintTotal()
		{
			if ( this.IsPoint)
			{
				return "P."+Convert.ToInt32(this.SubTotal).ToString();
			}
			else
			{
				return this.SubTotal.ToString(AppConst.DecimalFormat);
			}
		}
	}
}
