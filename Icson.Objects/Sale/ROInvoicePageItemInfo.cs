using System;

using Icson.Utils;

namespace Icson.Objects.Sale
{
	/// <summary>
	/// Summary description for ROInvoicePageItemInfo.
	/// </summary>
	public class ROInvoicePageItemInfo
	{
		public ROInvoicePageItemInfo()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#region Define Fields And Properties 

        public string pk; //为了解决productid 在退款单中不是pk的问题。对于其他例如退积分就自定义

		//Product Name or Service Name
		private string productID;

		//Product Name
		private string productName;

		//Product Price
		private decimal price;

		//Quantity
		private int quantity;

		//The Total Money
		private decimal total;

		// Weight
		public int weight;

		// is ro item
		public bool isRoItem;

		public bool isPoint = false;

		//Product Name or Service Name
		public string ProductID
		{
			set
			{
				this.productID = value;
			}
			get
			{
				return this.productID;
			}
		}

		//Product Name
		public string ProductName
		{
			set
			{
				this.productName = value;
			}
			get
			{
				if(this.productName == null)
				{
					return "";
				}
				return this.productName;
			}
		}

		//Product Price
		public decimal Price
		{
			set
			{
				this.price = value;
			}
			get
			{
				return this.price;
			}
		}

		//Quantity
		public int Quantity
		{
			set
			{
				this.quantity = value;
			}
			get
			{
				return this.quantity;
			}
		}

		//The Total Money
		public decimal Total
		{
			set
			{
				this.total = value;
			}
			get
			{
				return this.total;
			}
		}


		// Weight
		public int Weight
		{
			set
			{
				this.weight = value;
			}
			get
			{
				return this.weight;
			}
		}


		#endregion        



		public string GetPrintPrice()
		{
			if(this.isRoItem == true)
				return this.Price.ToString(AppConst.DecimalFormat);
			else
				return "";
		}

		public string GetPrintQuantity()
		{
			if(this.isRoItem == true)
				return this.Quantity.ToString();
			else
				return "";
		}

		public string GetPrintTotal()
		{
			if( !this.isPoint)
				return this.Total.ToString(AppConst.DecimalFormat);
			else
				return "P."+Convert.ToInt32(this.Total).ToString();
		}
	}
}
