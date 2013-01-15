using System;
using System.Collections;

using Icson.Utils;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// Summary description for ProductBasicInfo.
	/// </summary>
	public class ProductBasicInfo
	{
		public ProductBasicInfo()
		{
			Init();
		}
		
		public int SysNo;
		public string ProductID;
		public string ProductMode;
		public int ProductType;
		public string ProductName;
		public string ProductDesc;
		public string ProductDescLong;
		public string Performance;
		public string Warranty;
		public string PackageList;
		public int Weight;
        public int C1SysNo;
        public int C2SysNo;
		public int C3SysNo;
		public int ManufacturerSysNo;
		public string ProductLink;
		public int MultiPicNum;
		public int PMUserSysNo;
		public int PPMUserSysNo;
		public int CreateUserSysNo;
		public DateTime CreateTime;
		public string Attention;
		public string Note;
		public string BarCode;
		public int Status;
		public int IsLarge;
		public int LastVendorSysNo;

        public string PromotionWord;
        public string BriefName;
        public string AssessmentTitle;
        public string AssessmentLink;
        public int VirtualArriveTime;

        public int InventoryCycleTime;
        public decimal DMSWeight;
        public decimal DMS;
        public int OPL;

        public int OrderNum;
        public int APMUserSysNo;
        //baby1one
        public int DefaultVendorSysNo;
        public decimal DefaultPurchasePrice;
        public int Product2ndType;
        public int MasterProductSysNo;
        public int ProductColor;
        public int ProductSize;
        public string ProducingArea;
        public int PackQuantity;
        public int MinOrderQuantity;
        public int IsStoreFrontSale;
        public string SaleUnit;
        public int StorageDay;
        public int IsCanPurchase;
        public int IsCanDoVat;

        //baby1one

		public void Init()
		{
			SysNo = AppConst.IntNull;
			ProductID = AppConst.StringNull;
			ProductMode = AppConst.StringNull;
			ProductType = AppConst.IntNull;
			ProductName = AppConst.StringNull;
			ProductDesc = AppConst.StringNull;
			ProductDescLong = AppConst.StringNull;
			Performance = AppConst.StringNull;
			Warranty = AppConst.StringNull;
			PackageList = AppConst.StringNull;
			Weight = AppConst.IntNull;
            C1SysNo = AppConst.IntNull;
            C2SysNo = AppConst.IntNull;
			C3SysNo = AppConst.IntNull;
			ManufacturerSysNo = AppConst.IntNull;
			ProductLink = AppConst.StringNull;
			MultiPicNum = AppConst.IntNull;
			PMUserSysNo = AppConst.IntNull;
			PPMUserSysNo = AppConst.IntNull;
			CreateUserSysNo = AppConst.IntNull;
			CreateTime = AppConst.DateTimeNull;
			Attention = AppConst.StringNull;
			Note = AppConst.StringNull;
			BarCode = AppConst.StringNull;
			Status = AppConst.IntNull;
			IsLarge = AppConst.IntNull;
			LastVendorSysNo = AppConst.IntNull;

            PromotionWord = AppConst.StringNull;
            BriefName = AppConst.StringNull;
            AssessmentTitle = AppConst.StringNull;
            AssessmentLink = AppConst.StringNull;
            VirtualArriveTime = AppConst.IntNull;

            InventoryCycleTime = AppConst.IntNull;
            DMSWeight = AppConst.DecimalNull;
            DMS = AppConst.DecimalNull;
            OPL = AppConst.IntNull;

            OrderNum = AppConst.IntNull;
            APMUserSysNo = AppConst.IntNull;

            //baby1one
            DefaultVendorSysNo = AppConst.IntNull;
            DefaultPurchasePrice = AppConst.DecimalNull;
            Product2ndType = AppConst.IntNull;
            MasterProductSysNo = AppConst.IntNull;
            ProductColor = AppConst.IntNull;
            ProductSize = AppConst.IntNull;

            ProducingArea = AppConst.StringNull;
            PackQuantity = AppConst.IntNull;
            MinOrderQuantity = AppConst.IntNull;
            IsStoreFrontSale = AppConst.IntNull;
            SaleUnit = AppConst.StringNull;
            StorageDay = AppConst.IntNull;

            IsCanDoVat = AppConst.IntNull;
            IsCanPurchase = AppConst.IntNull;
            //baby1one
		}
		public static string GetSmallPic(string path, string productID)
		{
			//二手产品的图片和正常的一致
            if(productID.Length==7)
			    productID = productID.Substring(0,7);
            else if(productID.Length==10)
                productID = productID.Substring(0, 10);

			return path + "middle/" + productID + ".jpg";
		}

        public static string GetSmallPic(string path, string productID,bool small)
        {
            if (productID.Length == 7)
                productID = productID.Substring(0, 7);
            else if (productID.Length == 10)
                productID = productID.Substring(0, 10);

            if (small)
            {
                return path + "small/" + productID + ".jpg";
            }
            else
            {
                return path + "middle/" + productID + ".jpg";
            }
        }

		public static string GetMultiPic(string path, string productID, int index)
		{
            if (productID.Length == 7)
                productID = productID.Substring(0, 7);
            else if (productID.Length == 10)
                productID = productID.Substring(0, 10);

			string temp="";
			if ( index == 0)
				temp = path + "mpic/" + productID + ".jpg";
			else
				temp = path + "mpic/" + productID + "-" + index.ToString().PadLeft(2,'0') + ".jpg";
			return temp;
		}
		public ArrayList GetSmallPics(string path)
		{
            string productID = "";

            if (this.ProductID.Length == 7)
                productID = this.ProductID.Substring(0, 7);
            else if (this.ProductID.Length == 10)
                productID = this.ProductID.Substring(0, 10);

			ArrayList al = new ArrayList(MultiPicNum);
			for(int i=0; i<MultiPicNum; i++)
			{
				if ( i == 0)
					al.Add( path + "small/" + productID + ".jpg");
				else
					al.Add( path + "small/" + productID + "-" + i.ToString().PadLeft(2,'0') + ".jpg");
			}
			return al;
		}

		public ArrayList GeMiddlePics(string path)
		{
            string productID = "";

            if (this.ProductID.Length == 7)
                productID = this.ProductID.Substring(0, 7);
            else if (this.ProductID.Length == 10)
                productID = this.ProductID.Substring(0, 10);

			ArrayList al = new ArrayList(MultiPicNum);
			for(int i=0; i<MultiPicNum; i++)
			{
				if ( i == 0)
					al.Add( path + "middle/" + productID + ".jpg");
				else
					al.Add( path + "middle/" + productID + "-" + i.ToString().PadLeft(2,'0') + ".jpg");
			}
			return al;
		}

        public static string GetBigPic(string path, string productID)
        {
            //二手产品的图片和正常的一致
            productID = productID.Substring(0, 7);
            return path + "Big/" + productID + ".jpg";
        }

        public static string GetBiggestPic(string path, string productID)
        {
            //二手产品的图片和正常的一致
            productID = productID.Substring(0, 7);
            return path + "Biggest/" + productID + ".jpg";
        }
        public ArrayList GeBigPics(string path)
        {
            string productID = this.ProductID.Substring(0, 7);
            ArrayList al = new ArrayList(MultiPicNum);
            for (int i = 0; i < MultiPicNum; i++)
            {
                if (i == 0)
                    al.Add(path + "big/" + productID + ".jpg");
                else
                    al.Add(path + "big/" + productID + "-" + i.ToString().PadLeft(2, '0') + ".jpg");
            }
            return al;
        }

        public ArrayList GeBiggestPics(string path)
        {
            string productID = this.ProductID.Substring(0, 7);
            ArrayList al = new ArrayList(MultiPicNum);
            for (int i = 0; i < MultiPicNum; i++)
            {
                if (i == 0)
                    al.Add(path + "biggest/" + productID + ".jpg");
                else
                    al.Add(path + "biggest/" + productID + "-" + i.ToString().PadLeft(2, '0') + ".jpg");
            }
            return al;
        }

	}
}