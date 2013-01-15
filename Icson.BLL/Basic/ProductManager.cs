using System;
using System.Data;
using System.Collections;
using System.Text;
using System.Transactions;
using System.Web;

using Icson.Objects;
using Icson.Objects.Basic;
using Icson.Objects.Sale;
using Icson.Objects.Online;
using Icson.Utils;
using Icson.DBAccess;
using Icson.DBAccess.Basic;
using Icson.Objects.ImportData;
using Icson.DBAccess.ImportData;
using Icson.BLL.Online;

namespace Icson.BLL.Basic
{
    /// <summary>
    /// Summary description for ProductManager.
    /// </summary>
    public class ProductManager
    {
        private ProductManager()
        {
        }
        private static ProductManager _instance;
        public static ProductManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ProductManager();
            }
            return _instance;
        }

        private void mapBasic(ProductBasicInfo oParam, DataRow tempdr, bool isFull)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.ProductID = Util.TrimNull(tempdr["ProductID"]);
            oParam.ProductName = Util.TrimNull(tempdr["ProductName"]);
            oParam.PromotionWord = Util.TrimNull(tempdr["PromotionWord"]);
            oParam.BriefName = Util.TrimNull(tempdr["BriefName"]);
            oParam.MultiPicNum = Util.TrimIntNull(tempdr["MultiPicNum"]);
            oParam.ProductMode = Util.TrimNull(tempdr["ProductMode"]);
            oParam.DMS = Util.TrimDecimalNull(tempdr["DMS"]);
            oParam.OPL = Util.TrimIntNull(tempdr["OPL"]);
            oParam.BarCode = Util.TrimNull(tempdr["Barcode"]);
            oParam.DefaultVendorSysNo = Util.TrimIntNull(tempdr["DefaultVendorSysNo"]);

            if (isFull)
            {
                oParam.ProductType = Util.TrimIntNull(tempdr["ProductType"]);
                oParam.ProductDesc = Util.TrimNull(tempdr["ProductDesc"]);
                oParam.ProductDescLong = Util.TrimNull(tempdr["ProductDescLong"]);
                oParam.Performance = Util.TrimNull(tempdr["Performance"]);
                oParam.Warranty = Util.TrimNull(tempdr["Warranty"]);
                oParam.PackageList = Util.TrimNull(tempdr["PackageList"]);
                oParam.Weight = Util.TrimIntNull(tempdr["Weight"]);
                oParam.C1SysNo = Util.TrimIntNull(tempdr["C1SysNo"]);
                oParam.C2SysNo = Util.TrimIntNull(tempdr["C2SysNo"]);
                oParam.C3SysNo = Util.TrimIntNull(tempdr["C3SysNo"]);
                
                oParam.ManufacturerSysNo = Util.TrimIntNull(tempdr["ManufacturerSysNo"]);
                oParam.ProductLink = Util.TrimNull(tempdr["ProductLink"]);
                //oParam.MultiPicNum = Util.TrimIntNull(tempdr["MultiPicNum"]);
                oParam.PMUserSysNo = Util.TrimIntNull(tempdr["PMUserSysNo"]);
                oParam.PPMUserSysNo = Util.TrimIntNull(tempdr["PPMUserSysNo"]);
                oParam.CreateUserSysNo = Util.TrimIntNull(tempdr["CreateUserSysNo"]);
                oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
                oParam.Attention = Util.TrimNull(tempdr["Attention"]);
                oParam.Note = Util.TrimNull(tempdr["Note"]);
                oParam.Status = Util.TrimIntNull(tempdr["Status"]);
                oParam.IsLarge = Util.TrimIntNull(tempdr["IsLarge"]);
                oParam.LastVendorSysNo = Util.TrimIntNull(tempdr["LastVendorSysNo"]);
                oParam.PromotionWord = Util.TrimNull(tempdr["PromotionWord"]);
                oParam.BriefName = Util.TrimNull(tempdr["BriefName"]);
                oParam.AssessmentTitle = Util.TrimNull(tempdr["AssessmentTitle"]);
                oParam.AssessmentLink = Util.TrimNull(tempdr["AssessmentLink"]);
                oParam.VirtualArriveTime = Util.TrimIntNull(tempdr["VirtualArriveTime"]);

                oParam.InventoryCycleTime = Util.TrimIntNull(tempdr["InventoryCycleTime"]);
                oParam.DMSWeight = Util.TrimDecimalNull(tempdr["DMSWeight"]);

                oParam.OrderNum = Util.TrimIntNull(tempdr["OrderNum"]);
                oParam.APMUserSysNo = Util.TrimIntNull(tempdr["APMUserSysNo"]);
                oParam.DefaultPurchasePrice = Util.TrimDecimalNull(tempdr["DefaultPurchasePrice"]);
                oParam.Product2ndType = Util.TrimIntNull(tempdr["Product2ndType"]);
                oParam.MasterProductSysNo = Util.TrimIntNull(tempdr["MasterProductSysNo"]);
                oParam.ProductColor = Util.TrimIntNull(tempdr["ProductColor"]);
                oParam.ProductSize = Util.TrimIntNull(tempdr["ProductSize"]);
                oParam.ProducingArea = Util.TrimNull(tempdr["ProducingArea"]);
                oParam.PackQuantity = Util.TrimIntNull(tempdr["PackQuantity"]);
                oParam.MinOrderQuantity = Util.TrimIntNull(tempdr["MinOrderQuantity"]);
                oParam.IsStoreFrontSale = Util.TrimIntNull(tempdr["IsStoreFrontSale"]);
                oParam.SaleUnit = Util.TrimNull(tempdr["SaleUnit"]);
                oParam.StorageDay = Util.TrimIntNull(tempdr["StorageDay"]);

                oParam.IsCanPurchase = Util.TrimIntNull(tempdr["IsCanPurchase"]);
            }
        }
        private void mapPrice(ProductPriceInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.ProductSysNo = Util.TrimIntNull(tempdr["ProductSysNo"]);
            oParam.BasicPrice = Util.TrimDecimalNull(tempdr["BasicPrice"]);
            oParam.CurrentPrice = Util.TrimDecimalNull(tempdr["CurrentPrice"]);
            oParam.Discount = Util.TrimDecimalNull(tempdr["Discount"]);
            oParam.PointType = Util.TrimIntNull(tempdr["PointType"]);
            oParam.IsWholeSale = Util.TrimIntNull(tempdr["IsWholeSale"]);
            oParam.Q1 = Util.TrimIntNull(tempdr["Q1"]);
            oParam.P1 = Util.TrimDecimalNull(tempdr["P1"]);
            oParam.Q2 = Util.TrimIntNull(tempdr["Q2"]);
            oParam.P2 = Util.TrimDecimalNull(tempdr["P2"]);
            oParam.Q3 = Util.TrimIntNull(tempdr["Q3"]);
            oParam.P3 = Util.TrimDecimalNull(tempdr["P3"]);
            oParam.CashRebate = Util.TrimDecimalNull(tempdr["CashRebate"]);
            oParam.Point = Util.TrimIntNull(tempdr["Point"]);
            oParam.UnitCost = Util.TrimDecimalNull(tempdr["UnitCost"]);
            oParam.LastOrderPrice = Util.TrimDecimalNull(tempdr["LastOrderPrice"]);
            oParam.LastMarketLowestPrice = Util.TrimDecimalNull(tempdr["LastMarketLowestPrice"]);
            oParam.ClearanceSale = Util.TrimIntNull(tempdr["ClearanceSale"]);
            oParam.LimitedQty = Util.TrimIntNull(tempdr["LimitedQty"]);
        }
        private void mapAttribute(ProductAttributeInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.ProductSysNo = Util.TrimIntNull(tempdr["ProductSysNo"]);
            oParam.A1 = Util.TrimNull(tempdr["A1"]);
            oParam.A2 = Util.TrimNull(tempdr["A2"]);
            oParam.A3 = Util.TrimNull(tempdr["A3"]);
            oParam.A4 = Util.TrimNull(tempdr["A4"]);
            oParam.A5 = Util.TrimNull(tempdr["A5"]);
            oParam.A6 = Util.TrimNull(tempdr["A6"]);
            oParam.A7 = Util.TrimNull(tempdr["A7"]);
            oParam.A8 = Util.TrimNull(tempdr["A8"]);
            oParam.A9 = Util.TrimNull(tempdr["A9"]);
            oParam.A10 = Util.TrimNull(tempdr["A10"]);
            oParam.A11 = Util.TrimNull(tempdr["A11"]);
            oParam.A12 = Util.TrimNull(tempdr["A12"]);
            oParam.A13 = Util.TrimNull(tempdr["A13"]);
            oParam.A14 = Util.TrimNull(tempdr["A14"]);
            oParam.A15 = Util.TrimNull(tempdr["A15"]);
            oParam.A16 = Util.TrimNull(tempdr["A16"]);
            oParam.A17 = Util.TrimNull(tempdr["A17"]);
            oParam.A18 = Util.TrimNull(tempdr["A18"]);
            oParam.A19 = Util.TrimNull(tempdr["A19"]);
            oParam.A20 = Util.TrimNull(tempdr["A20"]);
            oParam.A21 = Util.TrimNull(tempdr["A21"]);
            oParam.A22 = Util.TrimNull(tempdr["A22"]);
            oParam.A23 = Util.TrimNull(tempdr["A23"]);
            oParam.A24 = Util.TrimNull(tempdr["A24"]);
            oParam.A25 = Util.TrimNull(tempdr["A25"]);
            oParam.A26 = Util.TrimNull(tempdr["A26"]);
            oParam.A27 = Util.TrimNull(tempdr["A27"]);
            oParam.A28 = Util.TrimNull(tempdr["A28"]);
            oParam.A29 = Util.TrimNull(tempdr["A29"]);
            oParam.A30 = Util.TrimNull(tempdr["A30"]);
        }
        private void mapStatus(ProductStatusInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.ProductSysNo = Util.TrimIntNull(tempdr["ProductSysNo"]);
            oParam.InfoStatus = Util.TrimIntNull(tempdr["InfoStatus"]);
            oParam.InfoUserSysNo = Util.TrimIntNull(tempdr["InfoUserSysNo"]);
            oParam.InfoTime = Util.TrimDateNull(tempdr["InfoTime"]);
            oParam.PriceStatus = Util.TrimIntNull(tempdr["PriceStatus"]);
            oParam.PriceUserSysNo = Util.TrimIntNull(tempdr["PriceUserSysNo"]);
            oParam.PriceTime = Util.TrimDateNull(tempdr["PriceTime"]);
            oParam.PicStatus = Util.TrimIntNull(tempdr["PicStatus"]);
            oParam.PicUserSysNo = Util.TrimIntNull(tempdr["PicUserSysNo"]);
            oParam.PicTime = Util.TrimDateNull(tempdr["PicTime"]);
            oParam.WarrantyStatus = Util.TrimIntNull(tempdr["WarrantyStatus"]);
            oParam.WarrantyUserSysNo = Util.TrimIntNull(tempdr["WarrantyUserSysNo"]);
            oParam.WarrantyTime = Util.TrimDateNull(tempdr["WarrantyTime"]);
            oParam.WeightStatus = Util.TrimIntNull(tempdr["WeightStatus"]);
            oParam.WeightUserSysNo = Util.TrimIntNull(tempdr["WeightUserSysNo"]);
            oParam.WeightTime = Util.TrimDateNull(tempdr["WeightTime"]);
            oParam.AllowStatus = Util.TrimIntNull(tempdr["AllowStatus"]);
            oParam.AllowUserSysNo = Util.TrimIntNull(tempdr["AllowUserSysNo"]);
            oParam.AllowTime = Util.TrimDateNull(tempdr["AllowTime"]);
            oParam.PreviewStatus = Util.TrimIntNull(tempdr["PreviewStatus"]);
            oParam.PreviewUserSysNo = Util.TrimIntNull(tempdr["PreviewUserSysNo"]);
            oParam.PreviewTime = Util.TrimDateNull(tempdr["PreviewTime"]);
        }

        public ProductBasicInfo LoadBasicBrief(int productSysNo)
        {
            string sql = "select sysno, productid, productname,PromotionWord,briefname, MultiPicNum, ProductMode,DMS,OPL,barcode,DefaultVendorSysNo from Product where SysNo = " + productSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                ProductBasicInfo oBasic = new ProductBasicInfo();
                mapBasic(oBasic, ds.Tables[0].Rows[0], false);
                return oBasic;
            }
            else
                return null;
        }
        public ProductBasicInfo LoadBasicBrief(string productID)
        {
            string sql = "select sysno, productid, productname,PromotionWord,briefname, MultiPicNum, ProductMode,DMS,OPL,barcode,DefaultVendorSysNo from Product where ProductID = " + Util.ToSqlString(productID);
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                ProductBasicInfo oBasic = new ProductBasicInfo();
                mapBasic(oBasic, ds.Tables[0].Rows[0], false);
                return oBasic;
            }
            else
                return null;
        }
        public ProductBasicInfo LoadBasic(int productSysNo)
        {
            string sql = "select * from product where sysno=" + productSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            ProductBasicInfo oBasic = new ProductBasicInfo();
            if (Util.HasMoreRow(ds))
                mapBasic(oBasic, ds.Tables[0].Rows[0], true);
            else
                oBasic = null;
            return oBasic;
        }
        public ProductBasicInfo LoadBasic(string productID)
        {
            string sql = "select * from product where productid =" + Util.ToSqlString(productID);
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            ProductBasicInfo oBasic = new ProductBasicInfo();
            if (Util.HasMoreRow(ds))
                mapBasic(oBasic, ds.Tables[0].Rows[0], true);
            else
                oBasic = null;
            return oBasic;
        }
        public ProductPriceInfo LoadPrice(int productSysNo)
        {
            string sql = "select * from product_price where productsysno=" + productSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            ProductPriceInfo oPrice = new ProductPriceInfo();
            if (Util.HasMoreRow(ds))
                mapPrice(oPrice, ds.Tables[0].Rows[0]);
            else
                oPrice = null;
            return oPrice;
        }
        public ProductAttributeInfo LoadAttribute(int productSysNo)
        {
            string sql = " select * from product_attribute where productsysno=" + productSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            ProductAttributeInfo oAttribute = new ProductAttributeInfo();
            if (Util.HasMoreRow(ds))
                mapAttribute(oAttribute, ds.Tables[0].Rows[0]);
            return oAttribute;
        }
        public void CloneAttribute2(int TargetProductSysno, int SourceProductSysno)
        {
            string sql1 = "INSERT INTO Product_Attribute2 ";
            string sql2 = "select " + TargetProductSysno + " AS productsysno,Attribute2SysNo,Attribute2OptionSysNo,Attribute2Value  from Product_Attribute2  where ProductSysNo = " + SourceProductSysno;
            string sql = sql1 + sql2;
            SqlHelper.ExecuteDataSet(sql);

        }

        public ProductStatusInfo LoadStatus(int productSysNo)
        {
            string sql = " select * from product_status where productsysno=" + productSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            ProductStatusInfo oStatus = new ProductStatusInfo();
            if (Util.HasMoreRow(ds))
                mapStatus(oStatus, ds.Tables[0].Rows[0]);
            return oStatus;
        }
        public ProductInfo LoadProduct(int productSysNo)
        {
            ProductInfo oProduct = new ProductInfo();
            oProduct.BasicInfo = this.LoadBasic(productSysNo);
            oProduct.PriceInfo = this.LoadPrice(productSysNo);
            oProduct.AttributeInfo = this.LoadAttribute(productSysNo);
            oProduct.StatusInfo = this.LoadStatus(productSysNo);
            return oProduct;
        }
        public ProductInfo LoadProduct(string productID)
        {
            ProductInfo oProduct = new ProductInfo();
            oProduct.BasicInfo = this.LoadBasic(productID);
            oProduct.PriceInfo = this.LoadPrice(oProduct.BasicInfo.SysNo);
            oProduct.AttributeInfo = this.LoadAttribute(oProduct.BasicInfo.SysNo);
            oProduct.StatusInfo = this.LoadStatus(oProduct.BasicInfo.SysNo);
            return oProduct;
        }

        public Hashtable LoadProducts(Hashtable sysNoHash)
        {
            Hashtable productHash = new Hashtable(5);
            if (sysNoHash.Count != 0)
            {
                string strSysNo = "(";
                foreach (int sysNo in sysNoHash.Keys)
                {
                    strSysNo += "'" + sysNo + "',";
                }
                strSysNo = strSysNo.TrimEnd(',');
                strSysNo += ")";
                string sql = @"select * from product p where p.sysno in " + strSysNo;

                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (Util.HasMoreRow(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ProductBasicInfo oInfo = new ProductBasicInfo();
                        mapBasic(oInfo, dr, true);
                        productHash.Add(oInfo.SysNo, oInfo);
                    }
                }
            }
            return productHash;
        }

        public DataSet GetBasicBriefDs(string paramMisc, int stockSysNo) //Misc is sysno, productid, product name
        {
            string sql = " select Product.SysNo, ProductID, ProductName,PromotionWord,BriefName, AccountQty, AvailableQty, AllocatedQty, OrderQty";
            if (stockSysNo == AppConst.IntNull)
            {
                //写stock显示的控制
                sql += " from  Product, Inventory where Product.sysno = Inventory.productsysno ";
            }
            else
            {
                sql += " from Product, Inventory_Stock where Product.sysno= Inventory_Stock.ProductSysno and StockSysNo=" + stockSysNo;
            }

            //int productSysNo = AppConst.IntNull;
            //try
            //{
            //    productSysNo = Convert.ToInt32(paramMisc);
            //}
            //catch
            //{
            //}
            //sql += " and (";
            //if (productSysNo != AppConst.IntNull)
            //    sql += " Product.sysno =" + productSysNo + " or ";

            //sql += " ProductID like" + Util.ToSqlLikeString(paramMisc);
            //sql += " or ProductName like" + Util.ToSqlLikeString(paramMisc);
            //sql += " )";

            string[] keys = paramMisc.Split(' ');
            if (keys.Length == 1)
            {
                if (Util.IsInteger(paramMisc))  // productsysno
                {
                    sql += " and (product.sysno=" + paramMisc + " or productid like " + Util.ToSqlLikeString(paramMisc) + " or productname like " + Util.ToSqlLikeString(paramMisc) + ")";
                }
                else  //productid, productname
                {
                    sql += " and (productid like " + Util.ToSqlLikeString(paramMisc) + " or productname like " + Util.ToSqlLikeString(paramMisc) + ")";
                }
            }
            else
            {
                sql += " and (productid like " + Util.ToSqlLikeString(paramMisc) + " or ( 1=1 ";
                for (int i = 0; i < keys.Length; i++)
                {
                    sql += " and productname like " + Util.ToSqlLikeString(keys[i]);
                }
                sql += "))";
            }

            sql += " order by Product.sysno ";
            return SqlHelper.ExecuteDataSet(sql);
        }

        /// <summary>
        /// 带商品价钱的GetBasicBriefDs版本
        /// </summary>
        /// <param name="paramMisc"></param>
        /// <param name="stockSysNo"></param>
        /// <returns></returns>
        public DataSet GetBasicBriefDsWithPrice(string paramMisc, int stockSysNo) //Misc is sysno, productid, product name
        {
            string sql = " select Product.SysNo, ProductID, ProductName,PromotionWord,BriefName, AccountQty, AvailableQty, AllocatedQty, OrderQty, BasicPrice, CurrentPrice, LastOrderPrice, Discount";
            if (stockSysNo == AppConst.IntNull)
            {
                //写stock显示的控制
                sql += " from  Product, Inventory, Product_Price where Product.sysno = Inventory.productsysno ";
            }
            else
            {
                sql += " from Product, Inventory_Stock, Product_Price where Product.sysno= Inventory_Stock.ProductSysno and StockSysNo=" + stockSysNo;
            }

            //int productSysNo = AppConst.IntNull;
            //try
            //{
            //    productSysNo = Convert.ToInt32(paramMisc);
            //}
            //catch
            //{
            //}
            //sql += " and (";
            //if (productSysNo != AppConst.IntNull)
            //    sql += " Product.sysno =" + productSysNo + " or ";

            //sql += " ProductID like" + Util.ToSqlLikeString(paramMisc);
            //sql += " or ProductName like" + Util.ToSqlLikeString(paramMisc);
            //sql += " )";

            string[] keys = paramMisc.Split(' ');
            if (keys.Length == 1)
            {
                if (Util.IsInteger(paramMisc))  // productsysno
                {
                    sql += " and (product.sysno=" + paramMisc + " or productid like " + Util.ToSqlLikeString(paramMisc) + " or productname like " + Util.ToSqlLikeString(paramMisc) + ")";
                }
                else  //productid, productname
                {
                    sql += " and (productid like " + Util.ToSqlLikeString(paramMisc) + " or productname like " + Util.ToSqlLikeString(paramMisc) + ")";
                }
            }
            else
            {
                sql += " and (productid like " + Util.ToSqlLikeString(paramMisc) + " or ( 1=1 ";
                for (int i = 0; i < keys.Length; i++)
                {
                    sql += " and productname like " + Util.ToSqlLikeString(keys[i]);
                }
                sql += "))";
            }

            sql += "and Product.SysNo=Product_Price.ProductSysNo order by Product.sysno ";
            return SqlHelper.ExecuteDataSet(sql);
        }

        public void ImportBasic()
        {
            if (!AppConfig.IsImportable)
                throw new BizException("IsImportable is false");
            string sql = "select top 1 * from product";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                throw new BizException("the table Product is not empty");

            string sql_product_distinct = "select productid from ipp2003..product group by productid having count(productid)>=2";
            DataSet ds_product_distinct = SqlHelper.ExecuteDataSet(sql_product_distinct);
            if (Util.HasMoreRow(ds_product_distinct))
                throw new BizException("the ipp2003..product has item using the same id");

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                string sqli = @"select p.sysno,p.productid,pl.productmode,pl.productname,pl.productdescription as productdesc, producttype,
								pl.otherinfo as productdesclong,pl.performance,isnull(pl.warrantyclause,'无保修') as warranty,pl.packlist as packagelist,
								isnull(p.netweight,0) as weight,c3.newsysno as C3SysNo,m.newsysno as ManufacturerSysNo,su.newsysno as CreateUserSysNo,
								su1.newsysno as PMUsersysno,su2.newsysno as ppmusersysno,isnull(p.mpicnum,0) as MultiPicNum,p.ProductLink,p.createtime,
								pl.note as Attention ,p.barcode,p.status,p.memo as note,p.islarge, con_vendor.newsysno as lastvendorsysno
								from ipp2003..product p 
								left join ipp2003..product_language pl on p.sysno = pl.productsysno 
								left join ippConvert..category3 c3 on c3.oldsysno = p.categorysysno
								left join ippConvert..manufacturer m on m.oldsysno = p.producersysno
								left join ippConvert..Sys_User su on su.oldsysno = p.createusersysno
								left join ippConvert..Sys_User su1 on su1.oldsysno = p.pmusersysno
								left join ippConvert..Sys_User su2 on su2.oldsysno = p.ppmusersysno
								left join ippConvert..vendor con_vendor on con_vendor.oldsysno = p.lastvendorsysno
								where pl.languageid='cn'";
                DataSet dsi = SqlHelper.ExecuteDataSet(sqli);
                foreach (DataRow dr in dsi.Tables[0].Rows)
                {
                    ProductBasicInfo oBasic = new ProductBasicInfo();
                    oBasic.ProductID = Util.TrimNull(dr["ProductID"]);
                    oBasic.ProductMode = Util.TrimNull(dr["ProductMode"]);
                    oBasic.ProductType = Util.TrimIntNull(dr["ProductType"]); //old type == new type
                    oBasic.ProductName = Util.TrimNull(dr["ProductName"]);
                    oBasic.ProductDesc = Util.TrimNull(dr["ProductDesc"]);
                    oBasic.ProductDescLong = Util.TrimNull(dr["ProductDescLong"]);
                    oBasic.Performance = Util.TrimNull(dr["Performance"]);
                    oBasic.Warranty = Util.TrimNull(dr["Warranty"]);
                    oBasic.PackageList = Util.TrimNull(dr["PackageList"]);
                    oBasic.Weight = Convert.ToInt32(Util.TrimDecimalNull(dr["Weight"]));
                    oBasic.C3SysNo = Util.TrimIntNull(dr["C3SysNo"]);
                    if (oBasic.C3SysNo == AppConst.IntNull)
                        oBasic.C3SysNo = 3810;
                    oBasic.ManufacturerSysNo = Util.TrimIntNull(dr["ManufacturerSysNo"]);
                    oBasic.ProductLink = Util.TrimNull(dr["ProductLink"]);
                    oBasic.MultiPicNum = Util.TrimIntNull(dr["MultiPicNum"]);
                    oBasic.PMUserSysNo = Util.TrimIntNull(dr["PMUserSysNo"]);
                    oBasic.PPMUserSysNo = Util.TrimIntNull(dr["PPMUserSysNo"]);
                    oBasic.CreateUserSysNo = Util.TrimIntNull(dr["CreateUserSysNo"]);
                    if (oBasic.CreateUserSysNo == AppConst.IntNull)
                        oBasic.CreateUserSysNo = -1;
                    oBasic.CreateTime = Util.TrimDateNull(dr["CreateTime"]);
                    oBasic.Attention = Util.TrimNull(dr["Attention"]);
                    oBasic.Note = Util.TrimNull(dr["Note"]);
                    oBasic.BarCode = Util.TrimNull(dr["BarCode"]);
                    oBasic.Status = Util.TrimIntNull(dr["Status"]);
                    oBasic.IsLarge = Util.TrimIntNull(dr["IsLarge"]);
                    oBasic.LastVendorSysNo = Util.TrimIntNull(dr["LastVendorSysNo"]);

                    oBasic.SysNo = SequenceDac.GetInstance().Create("Product_Sequence");
                    new ProductBasicDac().Insert(oBasic);
                    //this.InsertBasic(oBasic); productid 唯一的check在前面统一完成，以加快速度。

                    //insert into convert table
                    ImportInfo oProductBasicConvert = new ImportInfo();
                    oProductBasicConvert.OldSysNo = Util.TrimIntNull(dr["SysNo"]);
                    oProductBasicConvert.NewSysNo = oBasic.SysNo;
                    new ImportDac().Insert(oProductBasicConvert, "ProductBasic");
                }
                scope.Complete();
            }
        }
        public void ImportPrice()
        {
            if (!AppConfig.IsImportable)
                throw new BizException("IsImportable is false");
            string sql = "select top 1 * from product_price";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                throw new BizException("the table Product_Price is not empty");

            string sql_product_distinct = "select productsysno from ipp2003..product_price group by productsysno having count(productsysno)>=2";
            DataSet ds_product_distinct = SqlHelper.ExecuteDataSet(sql_product_distinct);
            if (Util.HasMoreRow(ds_product_distinct))
                throw new BizException("the ipp2003..product_price has item of 2 rows price info");

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                string sqli = @"select pb.newsysno as productsysno,basicprice,discount,currentprice,Point,
								unitcost,cashrebate,pointstatus as pointtype,clearancesale,createtime, lastOrderPrice
								from ipp2003..product_price pp
								left join ippConvert..productbasic pb on pb.oldsysno = pp.productsysno";
                DataSet dsi = SqlHelper.ExecuteDataSet(sqli);
                foreach (DataRow dr in dsi.Tables[0].Rows)
                {
                    ProductPriceInfo oPrice = new ProductPriceInfo();
                    oPrice.ProductSysNo = Util.TrimIntNull(dr["ProductSysNo"]);
                    oPrice.BasicPrice = Util.TrimDecimalNull(dr["BasicPrice"]);
                    if (oPrice.BasicPrice == AppConst.DecimalNull)
                        oPrice.BasicPrice = 999999m;
                    oPrice.CurrentPrice = Util.TrimDecimalNull(dr["CurrentPrice"]);
                    if (oPrice.CurrentPrice == AppConst.DecimalNull)
                        oPrice.CurrentPrice = 999999m;
                    oPrice.UnitCost = Util.TrimDecimalNull(dr["UnitCost"]);
                    if (oPrice.UnitCost == AppConst.DecimalNull)
                        oPrice.UnitCost = 0m;
                    oPrice.LastOrderPrice = Util.TrimDecimalNull(dr["LastOrderPrice"]);
                    oPrice.Discount = Util.TrimDecimalNull(dr["Discount"]);
                    oPrice.PointType = Util.TrimIntNull(dr["PointType"]);
                    oPrice.CashRebate = Util.TrimDecimalNull(dr["CashRebate"]);
                    oPrice.Point = Util.TrimIntNull(dr["Point"]);
                    oPrice.ClearanceSale = Util.TrimIntNull(dr["ClearanceSale"]);
                    //this.InsertPrice(oPrice);					
                    new ProductPriceDac().Insert(oPrice);
                }
                scope.Complete();
            }
        }
        public void ImportAttribute()
        {
            if (!AppConfig.IsImportable)
                throw new BizException("IsImportable is false");
            string sql = "select top 1 * from product_attribute";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                throw new BizException("the table Product_attribute is not empty");

            string sql_product_distinct = "select productsysno from ipp2003..product_attribute group by productsysno having count(productsysno)>=2";
            DataSet ds_product_distinct = SqlHelper.ExecuteDataSet(sql_product_distinct);
            if (Util.HasMoreRow(ds_product_distinct))
                throw new BizException("the ipp2003..product_attribute has item of 2 rows attribute info");

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                string sqli = @"select pb.newsysno as productsysno,A1,A2,A3,A4,A5,A6,A7,A8,A9,A10,A11,A12,
								A13,A14,A15,A16,A17,A18,A19,A20,A21,A22,A23,A24,A25,A26,A27,A28,A29,A30
								from ipp2003..product_attribute pa
								left join ippConvert..productbasic pb on pb.oldsysno = pa.productsysno
								where languageid = 'cn'";
                DataSet dsi = SqlHelper.ExecuteDataSet(sqli);
                foreach (DataRow dr in dsi.Tables[0].Rows)
                {
                    ProductAttributeInfo oAttribute = new ProductAttributeInfo();
                    oAttribute.ProductSysNo = Util.TrimIntNull(dr["ProductSysNo"]);
                    oAttribute.A1 = Util.TrimNull(dr["A1"]);
                    oAttribute.A2 = Util.TrimNull(dr["A2"]);
                    oAttribute.A3 = Util.TrimNull(dr["A3"]);
                    oAttribute.A4 = Util.TrimNull(dr["A4"]);
                    oAttribute.A5 = Util.TrimNull(dr["A5"]);
                    oAttribute.A6 = Util.TrimNull(dr["A6"]);
                    oAttribute.A7 = Util.TrimNull(dr["A7"]);
                    oAttribute.A8 = Util.TrimNull(dr["A8"]);
                    oAttribute.A9 = Util.TrimNull(dr["A9"]);
                    oAttribute.A10 = Util.TrimNull(dr["A10"]);
                    oAttribute.A11 = Util.TrimNull(dr["A11"]);
                    oAttribute.A12 = Util.TrimNull(dr["A12"]);
                    oAttribute.A13 = Util.TrimNull(dr["A13"]);
                    oAttribute.A14 = Util.TrimNull(dr["A14"]);
                    oAttribute.A15 = Util.TrimNull(dr["A15"]);
                    oAttribute.A16 = Util.TrimNull(dr["A16"]);
                    oAttribute.A17 = Util.TrimNull(dr["A17"]);
                    oAttribute.A18 = Util.TrimNull(dr["A18"]);
                    oAttribute.A19 = Util.TrimNull(dr["A19"]);
                    oAttribute.A20 = Util.TrimNull(dr["A20"]);
                    oAttribute.A21 = Util.TrimNull(dr["A21"]);
                    oAttribute.A22 = Util.TrimNull(dr["A22"]);
                    oAttribute.A23 = Util.TrimNull(dr["A23"]);
                    oAttribute.A24 = Util.TrimNull(dr["A24"]);
                    oAttribute.A25 = Util.TrimNull(dr["A25"]);
                    oAttribute.A26 = Util.TrimNull(dr["A26"]);
                    oAttribute.A27 = Util.TrimNull(dr["A27"]);
                    oAttribute.A28 = Util.TrimNull(dr["A28"]);
                    oAttribute.A29 = Util.TrimNull(dr["A29"]);
                    oAttribute.A30 = Util.TrimNull(dr["A30"]);

                    //this.InsertAttribute(oAttribute);
                    new ProductAttributeDac().Insert(oAttribute);
                }
                scope.Complete();
            }
        }
        public void ImportStatus()
        {
            if (!AppConfig.IsImportable)
                throw new BizException("IsImportable is false");
            string sql = "select top 1 * from product_status";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                throw new BizException("the table Product_Status is not empty");

            string sql_product_distinct = "select productsysno from ipp2003..product_status group by productsysno having count(productsysno)>=2";
            DataSet ds_product_distinct = SqlHelper.ExecuteDataSet(sql_product_distinct);
            if (Util.HasMoreRow(ds_product_distinct))
                throw new BizException("the ipp2003..product_status has item of 2 rows status info");

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                string sqli = @"select pb.newsysno as productsysno,ps.a as AllowStatus,ps.A_usersysno as allowusersysno,
								ps.a_time as allowtime,ps.w as warrantystatus,ps.w_usersysno as warrantyusersysno,ps.w_time as warrantytime,
								ps.p as pricestatus,ps.p_usersysno as priceusersysno,ps.p_time as pricetime,
								ps.i as infostatus,ps.i_usersysno as infousersysno,ps.i_time as infotime,
								ps.g as picstatus,ps.g_usersysno as picusersysno,ps.g_time as pictime
								from ipp2003..product_Status ps
								left join ippConvert..productbasic pb on pb.oldsysno = ps.productsysno
								where ps.productsysno>0 and ps.productsysno is not null and pb.newsysno is not null";
                DataSet dsi = SqlHelper.ExecuteDataSet(sqli);
                foreach (DataRow dr in dsi.Tables[0].Rows)
                {
                    ProductStatusInfo oStatus = new ProductStatusInfo();
                    oStatus.ProductSysNo = Util.TrimIntNull(dr["ProductSysNo"]);
                    if ((bool)dr["AllowStatus"])
                        oStatus.AllowStatus = 1;
                    else
                        oStatus.AllowStatus = 0;
                    oStatus.AllowUserSysNo = Util.TrimIntNull(dr["AllowUserSysNo"]);
                    oStatus.AllowTime = Util.TrimDateNull(dr["AllowTime"]);
                    if ((bool)dr["InfoStatus"])
                        oStatus.InfoStatus = 1;
                    else
                        oStatus.InfoStatus = 0;
                    oStatus.InfoUserSysNo = Util.TrimIntNull(dr["InfoUserSysNo"]);
                    oStatus.InfoTime = Util.TrimDateNull(dr["InfoTime"]);
                    if ((bool)dr["PicStatus"])
                        oStatus.PicStatus = 1;
                    else
                        oStatus.PicStatus = 0;
                    oStatus.PicUserSysNo = Util.TrimIntNull(dr["PicUserSysNo"]);
                    oStatus.PicTime = Util.TrimDateNull(dr["PicTime"]);
                    if ((bool)dr["PriceStatus"])
                        oStatus.PriceStatus = 1;
                    else
                        oStatus.PriceStatus = 0;
                    oStatus.PriceUserSysNo = Util.TrimIntNull(dr["PriceUserSysNo"]);
                    oStatus.PriceTime = Util.TrimDateNull(dr["PriceTime"]);
                    if ((bool)dr["WarrantyStatus"])
                        oStatus.WarrantyStatus = 1;
                    else
                        oStatus.WarrantyStatus = 0;
                    oStatus.WarrantyUserSysNo = Util.TrimIntNull(dr["WarrantyUserSysNo"]);
                    oStatus.WarrantyTime = Util.TrimDateNull(dr["WarrantyTime"]);
                    oStatus.WeightStatus = 0;
                    //this.InsertStatus(oStatus);					
                    new ProductStatusDac().Insert(oStatus);
                }
                scope.Complete();
            }
        }
        public int InsertBasic(ProductBasicInfo oParam)
        {
            string sql = "select * from product where ProductID=" + Util.ToSqlString(oParam.ProductID);
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                throw new BizException("the same ProductID exists already");
            oParam.SysNo = SequenceDac.GetInstance().Create("Product_Sequence");
            return new ProductBasicDac().Insert(oParam);
        }
        public int InsertPrice(ProductPriceInfo oParam)
        {
            string sql = "select * from product_price where productsysno =" + oParam.ProductSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                throw new BizException("the same ProductSysNo exists already");
            return new ProductPriceDac().Insert(oParam);
        }
        public int InsertAttribute(ProductAttributeInfo oParam)
        {
            string sql = "select * from product_attribute where productsysno =" + oParam.ProductSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                throw new BizException("the same ProductSysNo exists already");
            return new ProductAttributeDac().Insert(oParam);
        }
        public int InsertStatus(ProductStatusInfo oParam)
        {
            string sql = "select * from product_status where productsysno =" + oParam.ProductSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                throw new BizException("the same ProductSysNo exists already");
            return new ProductStatusDac().Insert(oParam);
        }

        public int InsertBarcode(ProductBarcodeInfo oParam)
        {
            string sql = "select * from product_barcode where productsysno=" + oParam.ProductSysNo + " and barcode='" + oParam.Barcode + "'";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                throw new BizException("the same ProductSysNo and Barcode exists already");
            return new ProductBarcodeDac().Insert(oParam);
        }

        public int DeleteBarcode(ProductBarcodeInfo oParam)
        {
            string sql = "delete product_barcode where productsysno=" + oParam.ProductSysNo + " and barcode='" + oParam.Barcode + "'";
            return SqlHelper.ExecuteNonQuery(sql);
        }

        public Hashtable GetProductBoundle(Hashtable sysnoHt)
        {
            if (sysnoHt == null || sysnoHt.Count == 0)
                return null;

            string sql = "select sysno, productid, productname,PromotionWord,BriefName,MultiPicNum,ProductMode,DMS,OPL,barcode,DefaultVendorSysNo from product where sysno in (";
            int index = 0;
            foreach (int item in sysnoHt.Keys)
            {
                if (index != 0)
                    sql += ",";

                sql += item.ToString();

                index++;
            }
            sql += ")";

            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return null;

            Hashtable ht = new Hashtable(5);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ProductBasicInfo item = new ProductBasicInfo();
                mapBasic(item, dr, false);
                ht.Add(item.SysNo, item);
            }
            return ht;
        }

        public Hashtable GetPriceBoundle(Hashtable sysnoHt)
        {
            if (sysnoHt == null || sysnoHt.Count == 0)
                return null;

            string sql = "select * from product_price where productsysno in (";
            int index = 0;
            foreach (int item in sysnoHt.Keys)
            {
                if (index != 0)
                    sql += ",";

                sql += item.ToString();

                index++;
            }
            sql += ")";

            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return null;

            Hashtable ht = new Hashtable(5);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ProductPriceInfo item = new ProductPriceInfo();
                mapPrice(item, dr);
                ht.Add(item.ProductSysNo, item);
            }
            return ht;
        }

        public Hashtable GetProductBoundleWithStockPosition1(int stockSysNo, Hashtable sysnoHt)
        {
            if (sysnoHt == null || sysnoHt.Count == 0)
                return null;

            string sql = "select product.sysno,product.barcode,product.DefaultVendorSysNo, product.productid, product.productname,DMS,OPL,product.MultiPicNum,product.ProductMode,inventory_stock.position1 as stockposition1 from product inner join inventory_stock on product.sysno=inventory_stock.productsysno where inventory_stock.stocksysno=@stocksysno and product.sysno in (";
            sql = sql.Replace("@stocksysno", stockSysNo.ToString());
            int index = 0;
            foreach (int item in sysnoHt.Keys)
            {
                if (index != 0)
                    sql += ",";

                sql += item.ToString();

                index++;
            }
            sql += ")";

            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return null;

            Hashtable ht = new Hashtable(5);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ProductBasicInfo item = new ProductBasicInfo();
                mapBasic(item, dr, false);
                ht.Add(item.SysNo, item);
            }
            return ht;
        }

        public DataSet GetProductBoundleWithInventory(Hashtable sysNoHash)
        {
            DataSet ds = new DataSet();
            if (sysNoHash.Count != 0)
            {
                string strSysNo = "(";
                foreach (int key in sysNoHash.Keys)
                {
                    strSysNo += "'";
                    strSysNo += key.ToString();
                    strSysNo += "',";
                }
                strSysNo = strSysNo.TrimEnd(',');
                strSysNo += ")";
                string sql = @"select 
									product.sysno,productid,productname,PromotionWord,briefname,availableqty+virtualqty as onlineqty,accountQty,availableqty,VirtualArriveTime,pp.LimitedQty as LimitedQty,size2name,iscandovat as iscanvat 
								from 
									product
								inner join inventory on inventory.productsysno = product.sysno
                                inner join Product_Price pp on pp.productsysno=product.sysno
                                left join size2 on size2.sysno=product.ProductSize 
								where
									product.sysno in " + strSysNo;
                ds = SqlHelper.ExecuteDataSet(sql);
            }
            return ds;
        }

        public DataSet GetProductBoundleWithInventoryPrice(Hashtable sysNoHash)
        {
            DataSet ds = new DataSet();
            if (sysNoHash.Count != 0)
            {
                string strSysNo = "(";
                foreach (int key in sysNoHash.Keys)
                {
                    strSysNo += "'";
                    strSysNo += key.ToString();
                    strSysNo += "',";
                }
                strSysNo = strSysNo.TrimEnd(',');
                strSysNo += ")";
                string sql = @"select 
									product.sysno,productid,productname,PromotionWord,briefname,availableqty+virtualqty as onlineqty,accountQty,availableqty,isnull(product_price.limitedqty,999) as limitedqty
								from 
									product
								inner join inventory on inventory.productsysno = product.sysno 
                                inner join product_price on product_price.productsysno = product.sysno 
								where
									product.sysno in " + strSysNo;
                ds = SqlHelper.ExecuteDataSet(sql);
            }
            return ds;
        }

        public void UpdateBasicInfo(ProductBasicInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                ProductBasicDac pb = new ProductBasicDac();
                pb.Update(oParam);
                scope.Complete();
            }
        }
        /// <summary>
        /// 根据商品各状态，更新商品基本状态
        /// </summary>
        /// <param name="oParam"></param>
        /// <param name="productStatus"></param>
        public void RefreshStatus(ProductStatusInfo oParam, int productSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                if (oParam.SysNo == AppConst.IntNull)
                {
                    oParam.ProductSysNo = productSysNo;
                    if (oParam.AllowStatus == AppConst.IntNull)
                        oParam.AllowStatus = (int)AppEnum.ProductStatusInfo.Valid;
                    if (oParam.InfoStatus == AppConst.IntNull)
                        oParam.InfoStatus = (int)AppEnum.ProductStatusInfo.Valid;
                    if (oParam.PicStatus == AppConst.IntNull)
                        oParam.PicStatus = (int)AppEnum.ProductStatusInfo.Valid;
                    if (oParam.PriceStatus == AppConst.IntNull)
                        oParam.PriceStatus = (int)AppEnum.ProductStatusInfo.Valid;
                    if (oParam.WarrantyStatus == AppConst.IntNull)
                        oParam.WarrantyStatus = (int)AppEnum.ProductStatusInfo.Valid;
                    if (oParam.WeightStatus == AppConst.IntNull)
                        oParam.WeightStatus = (int)AppEnum.ProductStatusInfo.Valid;
                    if (oParam.PreviewStatus == AppConst.IntNull)
                        oParam.PreviewStatus = (int)AppEnum.ProductStatusInfo.Valid;
                    this.InsertStatus(oParam);
                }
                else
                {
                    this.UpdateStatusInfo(oParam);
                }
                ProductBasicInfo pb = ProductManager.GetInstance().LoadBasic(oParam.ProductSysNo);
                //作废时，无需判断StatusInfo
                if (pb.Status != (int)AppEnum.ProductStatus.Abandon)
                {
                    if (oParam.AllowStatus == (int)AppEnum.ProductStatusInfo.Show && oParam.InfoStatus == (int)AppEnum.ProductStatusInfo.Show
                        && oParam.PicStatus == (int)AppEnum.ProductStatusInfo.Show && oParam.PriceStatus == (int)AppEnum.ProductStatusInfo.Show
                        && oParam.WarrantyStatus == (int)AppEnum.ProductStatusInfo.Show && oParam.WeightStatus == (int)AppEnum.ProductStatusInfo.Show
                        && oParam.PreviewStatus == (int)AppEnum.ProductStatusInfo.Show)  //增加预览状态
                        pb.Status = (int)AppEnum.ProductStatus.Show;
                    else
                        pb.Status = (int)AppEnum.ProductStatus.Valid;
                }
                new ProductBasicDac().UpdateStatus(oParam.ProductSysNo, pb.Status);
                scope.Complete();
            }
        }

        public int UpdateStatusInfo(ProductStatusInfo oParam)
        {
            return new ProductStatusDac().Update(oParam);
        }

        public void AddProduct(ProductBasicInfo oBasic)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                //Insert Basic Info
                oBasic.ProductID = GenerateNewProductID(oBasic.C3SysNo);
                ProductManager.GetInstance().InsertBasic(oBasic);
                //Insert Status Info
                ProductStatusInfo oStatus = new ProductStatusInfo();
                oStatus.ProductSysNo = oBasic.SysNo;
                oStatus.AllowStatus = (int)AppEnum.ProductStatusInfo.Valid;
                oStatus.InfoStatus = (int)AppEnum.ProductStatusInfo.Valid;
                oStatus.PicStatus = (int)AppEnum.ProductStatusInfo.Valid;
                oStatus.PriceStatus = (int)AppEnum.ProductStatusInfo.Valid;
                oStatus.WarrantyStatus = (int)AppEnum.ProductStatusInfo.Valid;
                oStatus.WeightStatus = (int)AppEnum.ProductStatusInfo.Valid;
                oStatus.PreviewStatus = (int)AppEnum.ProductStatusInfo.Valid;
                ProductManager.GetInstance().InsertStatus(oStatus);
                //Insert Price Info
                ProductPriceInfo oPrice = new ProductPriceInfo();
                oPrice.ProductSysNo = oBasic.SysNo;
                oPrice.ClearanceSale = (int)AppEnum.YNStatus.Yes;
                oPrice.IsWholeSale = (int)AppEnum.YNStatus.No;
                oPrice.Q1 = 0;
                oPrice.Q2 = 0;
                oPrice.Q3 = 0;
                oPrice.P1 = AppConst.DefaultPrice;
                oPrice.P2 = AppConst.DefaultPrice;
                oPrice.P3 = AppConst.DefaultPrice;
                oPrice.PointType = (int)AppEnum.ProductPayType.BothSupported;
                oPrice.Point = 0;
                oPrice.CashRebate = 0m;
                oPrice.UnitCost = 0;
                oPrice.BasicPrice = AppConst.DefaultPrice;
                oPrice.CurrentPrice = AppConst.DefaultPrice;
                oPrice.Discount = oPrice.CurrentPrice / oPrice.BasicPrice;
                ProductManager.GetInstance().InsertPrice(oPrice);

                //初始化库存
                InventoryManager.GetInstance().InitInventory(oBasic.SysNo);

                scope.Complete();
            }
        }

        public void AddMMbuyProduct(ProductBasicInfo oBasic)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                //Insert Basic Info
                //oBasic.ProductID = GenerateNewProductID(oBasic.C3SysNo);
                oBasic.ProductID = oBasic.ProductID;
                ProductManager.GetInstance().InsertBasic(oBasic);
                //Insert Status Info
                ProductStatusInfo oStatus = new ProductStatusInfo();
                oStatus.ProductSysNo = oBasic.SysNo;
                oStatus.AllowStatus = (int)AppEnum.ProductStatusInfo.Valid;
                oStatus.InfoStatus = (int)AppEnum.ProductStatusInfo.Valid;
                oStatus.PicStatus = (int)AppEnum.ProductStatusInfo.Valid;
                oStatus.PriceStatus = (int)AppEnum.ProductStatusInfo.Valid;
                oStatus.WarrantyStatus = (int)AppEnum.ProductStatusInfo.Valid;
                oStatus.WeightStatus = (int)AppEnum.ProductStatusInfo.Valid;
                oStatus.PreviewStatus = (int)AppEnum.ProductStatusInfo.Valid;
                ProductManager.GetInstance().InsertStatus(oStatus);
                //Insert Price Info
                ProductPriceInfo oPrice = new ProductPriceInfo();
                oPrice.ProductSysNo = oBasic.SysNo;
                oPrice.ClearanceSale = (int)AppEnum.YNStatus.Yes;
                oPrice.IsWholeSale = (int)AppEnum.YNStatus.No;
                oPrice.Q1 = 0;
                oPrice.Q2 = 0;
                oPrice.Q3 = 0;
                oPrice.P1 = AppConst.DefaultPrice;
                oPrice.P2 = AppConst.DefaultPrice;
                oPrice.P3 = AppConst.DefaultPrice;
                oPrice.PointType = (int)AppEnum.ProductPayType.BothSupported;
                oPrice.Point = 0;
                oPrice.CashRebate = 0m;
                oPrice.UnitCost = 0;
                oPrice.BasicPrice = AppConst.DefaultPrice;
                oPrice.CurrentPrice = AppConst.DefaultPrice;
                oPrice.Discount = oPrice.CurrentPrice / oPrice.BasicPrice;
                ProductManager.GetInstance().InsertPrice(oPrice);

                //初始化库存
                InventoryManager.GetInstance().InitInventory(oBasic.SysNo);

                scope.Complete();
            }
        }

        //productid, 7位字符串组成，前两位是大类的id，后五位是该大类下的流水号
        private string GenerateNewProductID(int c3SysNo)
        {
            string sql = @"select c1.sysno,c1.c1ID
                           from category1 c1 
                           inner join category2 c2 on c2.c1sysno=c1.sysno
                           inner join category3 c3 on c3.c2sysno=c2.sysno
                           where c3.sysno=" + c3SysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            int c1SysNo = Util.TrimIntNull(ds.Tables[0].Rows[0]["sysno"]);
            string c1ID = Util.TrimNull(ds.Tables[0].Rows[0]["c1ID"]);

            sql = @"select c1.c1ID, (max(right(productid,5)) + 1) as newProductID 
                           from category1 c1 
                           inner join category2 c2 on c2.c1sysno=c1.sysno
                           inner join category3 c3 on c3.c2sysno=c2.sysno
                           inner join product p on c3.sysno=p.c3sysno 
                           where c1.sysno=" + c1SysNo + " group by c1.c1ID";

            ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                return ds.Tables[0].Rows[0]["c1ID"].ToString() + ds.Tables[0].Rows[0]["newProductID"].ToString().PadLeft(5, '0');
            }
            else
            {
                return c1ID + "00001";
            }
        }

        public void EditProduct(ProductBasicInfo oBasic, ProductStatusInfo oStatus)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                ProductManager.GetInstance().UpdateBasicInfo(oBasic);
                ProductManager.GetInstance().RefreshStatus(oStatus, oBasic.SysNo);
                scope.Complete();
            }
        }

        public void EditPrice(ProductPriceInfo oPrice, ProductStatusInfo oStatus)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                //如果原先没有priceInfo，Add
                if (oPrice.SysNo == AppConst.IntNull)
                    ProductManager.GetInstance().InsertPrice(oPrice);
                else
                    ProductManager.GetInstance().UpdatePriceInfo(oPrice);
                ProductManager.GetInstance().RefreshStatus(oStatus, oPrice.ProductSysNo);
                scope.Complete();
            }
        }
        public DataTable GetProductAddedList()
        {
            string sql = @"select top 20 sysno,productid,productname,PromotionWord,briefname from product where status>=0 and createtime>='" + DateTime.Now.ToString(AppConst.DateFormat) + "' order by sysno desc";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            return ds.Tables[0];
        }
        /// <summary>
        /// 取出单个商品所有的属性
        /// </summary>
        /// <param name="paramProductSysNo"></param>
        /// <returns></returns>
        public SortedList GetProductAttributes(int paramProductSysNo)
        {
            string sql = @"select ca.attributeid,ca.attributename,ca.ordernum,ca.status
						  from category_attribute ca
						  inner join product p on p.c3sysno = ca.c3sysno
						  where ca.status=0 and p.sysno = " + paramProductSysNo
                        + @"; select *
						  from product_attribute
						  where productsysno = " + paramProductSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            DataTable TName = ds.Tables[0];
            DataTable TValue = ds.Tables[1];
            SortedList st = new SortedList();
            if (!Util.HasMoreRow(TName))
                return st;

            foreach (DataRow dr in TName.Rows)
            {
                AttributeInfo ai = new AttributeInfo();
                ai.AttributeId = dr["attributeid"].ToString();
                ai.AttributeName = dr["attributename"].ToString();
                ai.OrderNum = (int)dr["ordernum"];
                ai.Status = (int)dr["status"];
                if (Util.HasMoreRow(TValue))
                {
                    ai.AttributeValue = ds.Tables[1].Rows[0][ai.AttributeId].ToString();
                }
                st.Add(ai, null);
            }
            return st;
        }

        /// <summary>
        /// 取出单个商品所有的属性, 属性值为选项
        /// </summary>
        /// <param name="paramProductSysNo"></param>
        /// <returns></returns>
        public SortedList GetProductAttributes(int paramProductSysNo, bool Option)
        {
            string sql = @"select ca.attributeid,ca.attributename,ca.ordernum,ca.status
						  from category_attribute ca
						  inner join product p on p.c3sysno = ca.c3sysno
						  where ca.status=0 and p.sysno = " + paramProductSysNo
                + @"; select *
						  from product_attribute
						  where productsysno = " + paramProductSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            DataTable TName = ds.Tables[0];
            DataTable TValue = ds.Tables[1];
            SortedList st = new SortedList();
            if (!Util.HasMoreRow(TName))
                return st;

            foreach (DataRow dr in TName.Rows)
            {
                AttributeInfo ai = new AttributeInfo();
                ai.AttributeId = dr["attributeid"].ToString();
                ai.AttributeName = dr["attributename"].ToString();
                ai.OrderNum = (int)dr["ordernum"];
                ai.Status = (int)dr["status"];
                if (Util.HasMoreRow(TValue))
                {
                    ai.AttributeValue = ds.Tables[1].Rows[0][ai.AttributeId + "_AttributeOptionSysNo"].ToString();
                }
                st.Add(ai, null);
            }
            return st;
        }

        /// <summary>
        /// 取出单个商品所有的属性Value
        /// </summary>
        /// <param name="paramProductSysNo"></param>
        /// <param name="paramAttribute2Type">Attribute2Type</param>
        /// <returns></returns>
        public Hashtable GetProductAttribute2Value(int paramProductSysNo, int paramAttribute2Type)
        {
            string sql = "";
            if (paramAttribute2Type.Equals((int)AppEnum.AttributeType.Text))
            {
                sql = "select a.ProductSysNo,a.Attribute2SysNo,a.Attribute2OptionSysNo,a.Attribute2Value,b.Attribute2Type from product_attribute2 a, category_attribute2 b where a.attribute2sysno = b.sysno and a.productsysno = " + paramProductSysNo.ToString() + " and b.attribute2type=" + paramAttribute2Type.ToString();
            }
            else
            {
                sql = @"SELECT distinct Product_Attribute2.ProductSysNo,  Product_Attribute2.Attribute2SysNo, 
							Product_Attribute2.Attribute2OptionSysNo,  Product_Attribute2.Attribute2Value, 
							Category_Attribute2.Attribute2Type
							FROM  Category_Attribute2_Option INNER JOIN
							Product_Attribute2 ON 
							Category_Attribute2_Option.Attribute2SysNo =  Product_Attribute2.Attribute2SysNo
							INNER JOIN
							Category_Attribute2 ON 
							Category_Attribute2_Option.Attribute2SysNo =  Category_Attribute2.SysNo 
							where productsysno = " + paramProductSysNo + " and attribute2type=" + paramAttribute2Type;
            }
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;

            Hashtable ht = new Hashtable(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (paramAttribute2Type.Equals((int)AppEnum.AttributeType.Text))
                {
                    ht.Add(dr["attribute2sysno"].ToString(), dr["attribute2value"].ToString());
                }
                else
                {
                    ht.Add(dr["attribute2sysno"].ToString(), dr["attribute2optionsysno"].ToString());
                }
            }
            return ht;
        }

        /// <summary>
        /// 取出单个商品所有的属性Value
        /// </summary>
        /// <param name="paramProductSysNo"></param>
        /// <param name="paramIsText">IsText:true, IsOption:false</param>
        /// <returns></returns>
        public Hashtable GetProductAttribute2Value(int paramProductSysNo, bool paramIsText)
        {
            string sql = "";
            int iText = (int)AppEnum.AttributeType.Text;
            if (paramIsText)
            {
                sql = "select a.ProductSysNo,a.Attribute2SysNo,a.Attribute2OptionSysNo,a.Attribute2Value,b.Attribute2Type from product_attribute2 a, category_attribute2 b where a.attribute2sysno = b.sysno and a.productsysno = " + paramProductSysNo.ToString() + " and b.attribute2type=" + iText.ToString();
            }
            else
            {
                sql = @"SELECT distinct Product_Attribute2.ProductSysNo,  Product_Attribute2.Attribute2SysNo, 
							Product_Attribute2.Attribute2OptionSysNo,  Product_Attribute2.Attribute2Value, 
							Category_Attribute2.Attribute2Type
							FROM  Category_Attribute2_Option INNER JOIN
							Product_Attribute2 ON 
							Category_Attribute2_Option.Attribute2SysNo =  Product_Attribute2.Attribute2SysNo
							INNER JOIN
							Category_Attribute2 ON 
							Category_Attribute2_Option.Attribute2SysNo =  Category_Attribute2.SysNo 
							where productsysno = " + paramProductSysNo + " and attribute2type<>" + iText.ToString();
            }
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;

            Hashtable ht = new Hashtable(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (paramIsText)
                {
                    ht.Add(dr["attribute2sysno"].ToString(), dr["attribute2value"].ToString());
                }
                else
                {
                    ht.Add(dr["attribute2sysno"].ToString(), dr["attribute2optionsysno"].ToString());
                }
            }
            return ht;
        }

        /// <summary>
        /// 取出单个商品所有的属性Value
        /// </summary>
        /// <param name="paramProductSysNo"></param>
        /// <returns></returns>
        public Hashtable GetProductAttribute2Value(int paramProductSysNo)
        {
            string sql = "select a.ProductSysNo,a.Attribute2SysNo,a.Attribute2OptionSysNo,a.Attribute2Value,b.Attribute2Type from product_attribute2 a, category_attribute2 b where a.attribute2sysno = b.sysno and a.productsysno = " + paramProductSysNo.ToString();

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;

            Hashtable ht = new Hashtable(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (int.Parse(dr["attribute2type"].ToString()).Equals((int)AppEnum.AttributeType.Text))
                {
                    ht.Add(dr["attribute2sysno"].ToString(), dr["attribute2value"].ToString());
                }
                else
                {
                    ht.Add(dr["attribute2sysno"].ToString(), dr["attribute2optionsysno"].ToString());
                }
            }
            return ht;
        }

        /// <summary>
        /// 取出单个商品所有的属性选项
        /// </summary>
        /// <param name="paramProductSysNo"></param>
        /// <returns></returns>
        public SortedList GetProductOptionAttributes(int paramProductSysNo)
        {
            string sql = @"select ca.attributeid,ca.attributename,ca.ordernum,ca.status
						  from category_attribute ca
						  inner join product p on p.c3sysno = ca.c3sysno
						  where ca.status=0 and p.sysno = " + paramProductSysNo
                + @"; select *
						  from product_attribute
						  where productsysno = " + paramProductSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            DataTable TName = ds.Tables[0];
            DataTable TValue = ds.Tables[1];
            SortedList st = new SortedList();
            if (!Util.HasMoreRow(TName))
                return st;

            foreach (DataRow dr in TName.Rows)
            {
                AttributeInfo ai = new AttributeInfo();
                ai.AttributeId = dr["attributeid"].ToString();
                ai.AttributeName = dr["attributename"].ToString();
                ai.OrderNum = (int)dr["ordernum"];
                ai.Status = (int)dr["status"];
                if (Util.HasMoreRow(TValue))
                {
                    ai.AttributeValue = ds.Tables[1].Rows[0][ai.AttributeId + "_AttributeOptionSysNo"].ToString();
                }
                st.Add(ai, null);
            }
            return st;
        }

        //        /// <summary>
        //        /// 取出单个商品所有的属性选项value  For ItemDetail.aspx
        //        /// </summary>
        //        /// <param name="paramProductSysNo"></param>
        //        /// <returns></returns>
        //        public SortedList GetProductOptionAttributeValues(int paramProductSysNo)
        //        {
        //            string sql = @"SELECT ca.AttributeName as attributename,cao.AttributeOptionName as attributevalue,pa.ProductSysNo as productsysno 
        //						   FROM Category_Attribute ca INNER JOIN Category_Attribute_Option cao
        //						   ON ca.Status = 0 AND cao.Status = 0 AND ca.SysNo = cao.AttributeSysNo 
        //						   INNER JOIN Product_Attribute pa ON pa.productsysno=@ProductSysNo and 
        //						(
        //						      cao.SysNo = pa.A1_AttributeOptionSysNo OR cao.SysNo = pa.A2_AttributeOptionSysNo
        //						   OR cao.SysNo = pa.A3_AttributeOptionSysNo OR cao.SysNo = pa.A4_AttributeOptionSysNo
        //						   OR cao.SysNo = pa.A5_AttributeOptionSysNo OR cao.SysNo = pa.A6_AttributeOptionSysNo
        //						   OR cao.SysNo = pa.A7_AttributeOptionSysNo OR cao.SysNo = pa.A8_AttributeOptionSysNo
        //						   OR cao.SysNo = pa.A9_AttributeOptionSysNo OR cao.SysNo = pa.A10_AttributeOptionSysNo
        //						   OR cao.SysNo = pa.A11_AttributeOptionSysNo OR cao.SysNo = pa.A12_AttributeOptionSysNo
        //						   OR cao.SysNo = pa.A13_AttributeOptionSysNo OR cao.SysNo = pa.A14_AttributeOptionSysNo
        //						   OR cao.SysNo = pa.A15_AttributeOptionSysNo OR cao.SysNo = pa.A16_AttributeOptionSysNo
        //						   OR cao.SysNo = pa.A17_AttributeOptionSysNo OR cao.SysNo = pa.A18_AttributeOptionSysNo
        //						   OR cao.SysNo = pa.A19_AttributeOptionSysNo OR cao.SysNo = pa.A20_AttributeOptionSysNo
        //						   OR cao.SysNo = pa.A21_AttributeOptionSysNo OR cao.SysNo = pa.A22_AttributeOptionSysNo
        //						   OR cao.SysNo = pa.A23_AttributeOptionSysNo OR cao.SysNo = pa.A24_AttributeOptionSysNo
        //						   OR cao.SysNo = pa.A25_AttributeOptionSysNo OR cao.SysNo = pa.A26_AttributeOptionSysNo
        //						   OR cao.SysNo = pa.A27_AttributeOptionSysNo OR cao.SysNo = pa.A28_AttributeOptionSysNo
        //						   OR cao.SysNo = pa.A29_AttributeOptionSysNo OR cao.SysNo = pa.A30_AttributeOptionSysNo
        //						)
        //						   ORDER BY ca.OrderNum";
        //            sql = sql.Replace("@ProductSysNo",paramProductSysNo.ToString());

        //            DataSet ds = SqlHelper.ExecuteDataSet(sql);
        //            SortedList st = new SortedList();
        //            if(!Util.HasMoreRow(ds))
        //                return st;

        //            foreach(DataRow dr in ds.Tables[0].Rows)
        //            {
        //                AttributeInfo ai = new AttributeInfo();
        //                ai.AttributeName = dr["attributename"].ToString().Trim();
        //                ai.AttributeValue = dr["attributevalue"].ToString().Trim();
        //                st.Add(ai,null);
        //            }
        //            return st;
        //        }

        /// <summary>
        /// 取出单个商品所有的属性选项value  For ItemDetail.aspx
        /// </summary>
        /// <param name="paramProductSysNo"></param>
        /// <returns></returns>
        public string GetProductOptionAttributeValues(int paramProductSysNo)
        {
            string sql = @"SELECT sysno,productsysno,summary from Product_Attribute2_Summary where productsysno=@ProductSysNo";
            sql = sql.Replace("@ProductSysNo", paramProductSysNo.ToString());

            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return "";
            else
            {
                //IAS更新后，此处的replace可以去掉
                string summary = ds.Tables[0].Rows[0]["summary"].ToString().Trim();
                summary = summary.Replace("<TABLE cellpadding=0 cellspacing=0 class=specification width=300>", "<TABLE cellpadding=0 cellspacing=0 class=specification>");
                summary = summary.Replace("<TD align=left colspan=2 class=name>", "<TD align=left colspan=2 class=title>");
                return summary;
            }
        }

        /// <summary>
        /// 取出单个Category3所有的valid属性选项
        /// </summary>
        /// <param name="paramC3SysNo"></param>
        /// <returns></returns>
        public DataSet GetC3ProductAttributeOptions(int paramC3SysNo)
        {
            string sql = @"select ca.sysno as attributesysno,ca.attributeid,ca.attributename,ca.ordernum,ca.status 
						   from category_attribute ca where ca.c3sysno=" + paramC3SysNo + " and ca.status=0 order by ca.ordernum";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;

            StringBuilder sb = new StringBuilder();
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sb.Append("select '" + dr["attributesysno"].ToString().Trim() + "' as attributesysno,'" + dr["attributeid"].ToString().Trim() + "' as attributeid,'" + dr["attributename"].ToString().Trim() + "' as attributename,sysno as attributeoptionsysno,attributeoptionname from Category_Attribute_Option where AttributeSysNo=" + dr["attributesysno"].ToString() + " and Status=0 order by ordernum;");
            }

            if (sb.Length > 0)
            {
                sql += ";" + sb.ToString().Substring(0, sb.Length - 1);
            }

            ds = SqlHelper.ExecuteDataSet(sql);
            return ds;
        }

        /// <summary>
        /// 取出单个Category3所有的valid属性选项
        /// </summary>
        /// <param name="paramC3SysNo"></param>
        /// <returns></returns>
        public DataSet GetC3ProductAttribute2Options(int paramC3SysNo)
        {
            string sql = @"select ca.sysno as attribute1sysno,ca.attribute1id,ca.attribute1name,ca.ordernum,ca.status 
						   from category_attribute1 ca where ca.c3sysno=" + paramC3SysNo + " and ca.status=0 order by ca.ordernum";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;

            StringBuilder sb = new StringBuilder();
            DataTable dt1 = ds.Tables[0];
            foreach (DataRow dr in dt1.Rows)
            {
                sb.Append("select sysno as attribute2sysno,attribute2name,attribute2type from Category_Attribute2 where A1SysNo=" + dr["attribute1sysno"].ToString() + " and Status=0 order by ordernum;");
            }

            ds = SqlHelper.ExecuteDataSet(sb.ToString().Substring(0, sb.Length - 1));
            if (!Util.HasMoreRow(ds))
                return null;

            foreach (DataTable dt in ds.Tables)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    sb.Append("select sysno as attribute2optionsysno,attribute2optionname from Category_attribute2_option where attribute2sysno=" + dr["attribute2sysno"].ToString() + " and status=0 order by ordernum;");
                }
            }

            if (sb.Length > 0)
            {
                sql += ";" + sb.ToString().Substring(0, sb.Length - 1);
            }

            ds = SqlHelper.ExecuteDataSet(sql);
            return ds;
        }

        //		/// <summary>
        //		/// 取出单个商品所有的属性
        //		/// </summary>
        //		/// <param name="paramProductSysNo"></param>
        //		/// <returns></returns>
        //		public SortedList GetProductAttributes(int paramProductSysNo)
        //		{
        //			string sql = @"select ca.attributeid,ca.attributename,ca.ordernum,ca.status,ca.attributetype,ca.sysno 
        //						  from category_attribute ca
        //						  inner join product p on p.c3sysno = ca.c3sysno
        //						  where ca.status=0 and p.sysno = "+paramProductSysNo
        //				+@"; select *
        //						  from product_attribute
        //						  where productsysno = "+paramProductSysNo;
        //			DataSet ds = SqlHelper.ExecuteDataSet(sql);
        //			DataTable TName = ds.Tables[0];
        //			DataTable TValue = ds.Tables[1];
        //			SortedList st = new SortedList();
        //			if(!Util.HasMoreRow(TName))
        //				return st;
        //
        //			foreach(DataRow dr in TName.Rows)
        //			{
        //				AttributeInfo ai = new AttributeInfo();
        //				ai.SysNo = (int)dr["sysno"];
        //				ai.AttributeId = dr["attributeid"].ToString();
        //				ai.AttributeName = dr["attributename"].ToString();
        //				ai.OrderNum = (int)dr["ordernum"];
        //				ai.Status = (int)dr["status"];
        //				ai.AttributeType = dr["attributetype"].ToString();
        //				if ( Util.HasMoreRow(TValue))
        //				{
        //					ai.AttributeValue = ds.Tables[1].Rows[0][ai.AttributeId].ToString();
        //				}
        //				st.Add(ai,null);
        //			}
        //			return st;
        //		}

        /// <summary>
        /// 更新商品属性
        /// </summary>
        /// <param name="oParam"></param>
        public void UpdateAttributeInfo(ProductAttributeInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                ProductAttributeDac pa = new ProductAttributeDac();
                //pa.Update(oParam);
                pa.Update2(oParam);
                scope.Complete();
            }
        }

        /// <summary>
        /// 更新商品属性
        /// </summary>
        /// <param name="oParam"></param>
        public void UpdateAttribute2(int paramProductSysNo, Hashtable htText, Hashtable htOption)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                Hashtable ht_Old = GetProductAttribute2Value(paramProductSysNo);
                ProductAttributeDac pa = new ProductAttributeDac();

                foreach (string key in htText.Keys)
                {
                    ProductAttribute2Info oParam = new ProductAttribute2Info();
                    oParam.ProductSysNo = paramProductSysNo;
                    oParam.Attribute2SysNo = int.Parse(key);
                    oParam.Attribute2Value = htText[key].ToString();
                    oParam.Attribute2OptionSysNo = 0;  //default value is 0

                    if (ht_Old != null && ht_Old.Count > 0 && ht_Old.ContainsKey(key))
                    {
                        pa.Update(oParam);
                    }
                    else
                    {
                        pa.Insert(oParam);
                    }
                }

                foreach (string key in htOption.Keys)
                {
                    ProductAttribute2Info oParam = new ProductAttribute2Info();
                    oParam.ProductSysNo = paramProductSysNo;
                    oParam.Attribute2SysNo = int.Parse(key);
                    oParam.Attribute2OptionSysNo = int.Parse(htOption[key].ToString());
                    oParam.Attribute2Value = ""; // default value is ""

                    if (ht_Old != null && ht_Old.Count > 0 && ht_Old.ContainsKey(key))
                    {
                        pa.Update(oParam);
                    }
                    else
                    {
                        pa.Insert(oParam);
                    }
                }

                if (ht_Old != null && ht_Old.Count > 0)
                {
                    foreach (string key in ht_Old.Keys)
                    {
                        if (!htText.ContainsKey(key) && !htOption.ContainsKey(key))
                        {
                            pa.DeleteProductAttribute2(paramProductSysNo, Util.TrimIntNull(key));
                        }
                    }
                }

                scope.Complete();
            }
        }

        /// <summary>
        /// 更新产品属性summary
        /// </summary>
        /// <param name="paramProductSysNo"></param>
        /// <returns></returns>
        public void UpdateProductAttribute2Summary(int paramProductSysNo)
        {
            try
            {
                string sql = @"SELECT 
                        category_attribute1.ordernum,
                        category_attribute2.ordernum, 
                        product_attribute2.productsysno,
                        category_attribute1.sysno as attribute1sysno,
                        attribute1name,
                        attribute2name,
                        attribute2type,
                        case category_attribute2.attribute2type when '0' then product_attribute2.attribute2value else category_attribute2_option.attribute2optionname end as attribute2value
                        FROM 
                         Product_Attribute2 
                        INNER JOIN Category_Attribute2 
                         ON Product_Attribute2.Attribute2SysNo = Category_Attribute2.SysNo
                        INNER JOIN Category_Attribute1 
                         ON Category_Attribute2.A1SysNo = Category_Attribute1.SysNo
                        LEFT JOIN Category_Attribute2_Option 
                         ON Product_Attribute2.Attribute2OptionSysNo = Category_Attribute2_Option.SysNo
                        WHERE category_attribute1.status=0 and category_attribute2.status=0 and isnull(category_attribute2_option.status,0)=0 and ProductSysNo = @productsysno 
                        ORDER BY category_attribute1.ordernum,category_attribute2.ordernum ";
                sql = sql.Replace("@productsysno", paramProductSysNo.ToString());
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (Util.HasMoreRow(ds))
                {
                    DataTable dt = ds.Tables[0];
                    StringBuilder sb = new StringBuilder();
                    StringBuilder sbMain = new StringBuilder();

                    int attribute1sysno = 0; int.Parse(dt.Rows[0]["attribute1sysno"].ToString());
                    sb.Append("<TABLE cellpadding=0 cellspacing=0 class=specification>");
                    sb.Append("<TBODY>");

                    sbMain.Append("<TABLE cellpadding=0 cellspacing=0 class=specification2>");
                    sbMain.Append("<TBODY>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (!int.Parse(dt.Rows[i]["attribute1sysno"].ToString()).Equals(attribute1sysno))  //attribute1name
                        {
                            sb.Append("<TR><TD colspan=2 class=title>" + dt.Rows[i]["attribute1name"].ToString().Trim() + "</TD></TR>");
                            attribute1sysno = int.Parse(dt.Rows[i]["attribute1sysno"].ToString());
                            i--;
                        }
                        else //attribute2name + attribute2value
                        {
                            sb.Append("<TR><TD class=name>" + dt.Rows[i]["attribute2name"].ToString().Trim() + "</TD><TD class=desc>" + dt.Rows[i]["attribute2value"].ToString().Trim() + "</TD></TR>");
                            if (Util.TrimIntNull(dt.Rows[i]["attribute2type"]) == 1)
                                sbMain.Append("<TR><TD class=name2>" + dt.Rows[i]["attribute2name"].ToString().Trim() + ": " + dt.Rows[i]["attribute2value"].ToString().Trim() + "</TD></TR>");
                        }
                    }
                    sb.Append("</TBODY></TABLE>");
                    sbMain.Append("</TBODY></TABLE>");

                    sql = "select productsysno from product_attribute2_summary where productsysno=" + paramProductSysNo;
                    DataSet ds2 = SqlHelper.ExecuteDataSet(sql);
                    ProductAttribute2Summary oParam = new ProductAttribute2Summary();
                    oParam.ProductSysNo = paramProductSysNo;
                    oParam.Summary = sb.ToString();
                    oParam.SummaryMain = sbMain.ToString();

                    if (Util.HasMoreRow(ds2))
                    {
                        new ProductAttributeDac().Update(oParam);
                    }
                    else
                    {
                        new ProductAttributeDac().Insert(oParam);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据c3sysno更新相关产品属性summary
        /// </summary>
        /// <param name="paramC3SysNo"></param>
        public void UpdateProductAttribute2SummarysByC3SysNO(int paramC3SysNo)
        {
            string sql = "select product.sysno from product,Product_Attribute2_Summary where product.sysno=Product_Attribute2_Summary.productsysno and product.c3sysno=" + paramC3SysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                TransactionOptions options = new TransactionOptions();
                options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                options.Timeout = TransactionManager.DefaultTimeout;

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        this.UpdateProductAttribute2Summary(Int32.Parse(dr["sysno"].ToString()));
                    }

                    scope.Complete();
                }
            }
        }

        public string GetProductStatusDesc(int paramStatus)
        {
            string Desc = "";
            switch (paramStatus)
            {
                case 0:
                    Desc = "Valid";
                    break;
                case 1:
                    Desc = "Show";
                    break;
                case -1:
                    Desc = "Abandon";
                    break;
                default:
                    Desc = "Unknown";
                    break;
            }
            return Desc;
        }
        public void UpdatePriceInfo(ProductPriceInfo oPrice)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                ProductPriceDac pp = new ProductPriceDac();
                pp.Update(oPrice);
                scope.Complete();
            }
        }
        public void ProductClone(string SourceID, string TargetID, int createUserSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                //复制商品基本信息
                ProductBasicInfo TBasic = ProductManager.GetInstance().LoadBasic(SourceID);
                if (TBasic.SysNo == AppConst.IntNull)
                    throw new BizException("Source Product does not exist");
                int SourceSysno = TBasic.SysNo;
                if (TargetID != string.Empty)
                    TBasic.ProductID = TargetID;
                else
                {
                    if (SourceID.Substring(SourceID.Length - 1, 1).ToUpper() != "R" && SourceID.Substring(SourceID.Length - 1, 1).ToUpper() != "B")//新品默认复制为二手
                    {
                        TBasic.ProductID = SourceID + "R";
                    }
                    else if (SourceID.Substring(SourceID.Length - 1, 1).ToUpper() == "R")//二手默认复制为坏品
                    {
                        TBasic.ProductID = SourceID.Substring(0, SourceID.Length - 1) + "B";
                    }
                    else if (SourceID.Substring(SourceID.Length - 1, 1).ToUpper() == "B")//坏品不提供默认复制
                    {
                        throw new BizException("Bad Product has no default target");
                    }
                }
                if (TBasic.ProductID.ToUpper().EndsWith("R") || TBasic.ProductID.ToUpper().Substring(0, TBasic.ProductID.Length - 1).EndsWith("R"))
                {
                    TBasic.ProductName = "（二手）" + TBasic.ProductName.Replace("（二手）", string.Empty).Replace("（坏品）", string.Empty);
                    TBasic.ProductType = (int)AppEnum.ProductType.SecondHand;
                }
                else if (TBasic.ProductID.ToUpper().EndsWith("B"))
                {
                    TBasic.ProductName = "（坏品）" + TBasic.ProductName.Replace("（二手）", string.Empty).Replace("（坏品）", string.Empty);
                    TBasic.ProductType = (int)AppEnum.ProductType.Bad;
                }
                TBasic.CreateUserSysNo = createUserSysNo;
                TBasic.Status = (int)AppEnum.ProductStatus.Valid;
                TBasic.MultiPicNum = 0;
                ProductManager.GetInstance().InsertBasic(TBasic);
                //复制价格信息
                ProductPriceInfo TPrice = ProductManager.GetInstance().LoadPrice(SourceSysno);
                if (TPrice.ProductSysNo != AppConst.IntNull)
                {
                    TPrice.ProductSysNo = TBasic.SysNo;
                    ProductManager.GetInstance().InsertPrice(TPrice);
                }
                //状态不复制，插入新记录
                ProductStatusInfo TStatus = new ProductStatusInfo();
                TStatus.ProductSysNo = TBasic.SysNo;
                TStatus.AllowStatus = (int)AppEnum.ProductStatusInfo.Valid;
                TStatus.InfoStatus = (int)AppEnum.ProductStatusInfo.Valid;
                TStatus.PicStatus = (int)AppEnum.ProductStatusInfo.Valid;
                TStatus.PriceStatus = (int)AppEnum.ProductStatusInfo.Valid;
                TStatus.WarrantyStatus = (int)AppEnum.ProductStatusInfo.Valid;
                TStatus.WeightStatus = (int)AppEnum.ProductStatusInfo.Valid;
                ProductManager.GetInstance().InsertStatus(TStatus);
                //复制商品属性  add by judy
                int TargetSysno = TBasic.SysNo;
                ProductManager.GetInstance().CloneAttribute2(TargetSysno, SourceSysno);

                //初始化目标商品库存
                InventoryManager.GetInstance().InitInventory(TBasic.SysNo);

                //复制安全库存 add by judy  //安全库存暂不复制
                //InventoryStockInfo TStock = InventoryManager.GetInstance().LoadInventoryStock2(SourceSysno);
                //InventoryManager.GetInstance().SetSafeQty(TargetSysno, TStock.SafeQty, TStock.StockSysNo);

                scope.Complete();
            }
        }

        public DataSet PowerSearch(Hashtable paramHash)
        {
            //select 出来的sysno默认是ProductSysNo, 不能改

            string sql0 = @" select distinct
								product.sysno, productid, productname,PromotionWord, productmode as productmodel,product.createtime,
                                product.DMS,product.OPL,product.InventoryCycleTime,product.DMSWeight,DefaultPurchasePrice,
								unitcost, currentprice, point, lastorderprice,inventory_stock.safeqty,
								inventory.orderqty, inventory.accountqty, inventory.allocatedqty,inventory_stock.accountqty as stockaccountqty ,
								inventory.availableqty, inventory.virtualqty, inventory.availableqty+inventory.virtualqty onlineqty, 
								inventory.purchaseqty, product_saletrend.*,product_saletrend.w1+product_saletrend.w2+0 as monitorqty,
								product.status, sys_user.username as pmUserName, VendorName,product_rmapercent.rmapercent,product.barcode";  //modify by Judy
            //string sql00 = ", '0' as stockaccountqty"; Uncomment by Judy

            string sql1 = @" from 
								product inner join category3 on product.c3sysno = category3.sysno 
								inner join category2 on category3.c2sysno = category2.sysno 
								inner join category1 on category2.c1sysno = category1.sysno
								left join manufacturer on product.manufacturersysno = manufacturer.sysno
								left join product_price on product.sysno = product_price.productsysno
								left join product_status on product.sysno = product_status.productsysno
								left join sys_user on product.pmusersysno = sys_user.sysno
								left join inventory on product.sysno = inventory.productsysno
								left join product_saletrend on product.sysno = product_saletrend.productsysno
								left join vendor on product.lastvendorsysno = vendor.sysno
                                left join inventory_stock on product.sysno = inventory_stock.productsysno 
                                left join product_rmapercent on product.sysno=product_rmapercent.productsysno ";
            string sql2 = @"
							where 
								1=1
							";

            string sql = sql0 + sql1 + sql2;

            if (paramHash != null && paramHash.Count != 0)
            {
                // Uncomment by Judy
                //if ( paramHash.ContainsKey("StockSysNo") )
                //{
                //    sql0 = sql0 + ", inventory_stock.accountqty as stockaccountqty";
                ////  sql1 = sql1 + " left join inventory_stock on product.sysno = inventory_stock.productsysno"; 
                //    sql = sql0 + sql1 + sql2;
                //}

                StringBuilder sb = new StringBuilder(100);
                string sqlSaleTime = "";
                string sqlSaleNull = "";
                int isBadInventory = 0;
                int isSaleNull = 0;
                foreach (string key in paramHash.Keys)
                {
                    sb.Append(" and ");
                    object item = paramHash[key];
                    if (key == "SysNo")
                    {
                        sb.Append("product.").Append(key).Append("=").Append(item.ToString());
                    }
                    else if (key == "Status")
                    {
                        sb.Append("product.").Append(key).Append(item.ToString());
                    }
                    else if (key == "DateFrom")
                    {
                        sb.Append("product.CreateTime >= ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "DateTo")
                    {

                        sb.Append("product.CreateTime <= ").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "StockSysNo")
                    {
                        sb.Append("inventory_stock.stocksysno").Append("=").Append(item.ToString());
                    }
                    else if (key == "APMUserSysNo")
                    {
                        sb.Append("product.apmUserSysNo").Append("=").Append(item.ToString());
                    }
                    else if (key == "VendorSysNo")
                    {
                        sb.Append("product.defaultvendorsysno").Append("=").Append(item.ToString());
                    }
                    else if (key == "Product2ndType")
                    {
                        sb.Append("product.product2ndtype").Append("=").Append(item.ToString());
                    }
                    else if (key == "VendorName")		//如果查询慢，可以改为用直接在product.lastvendorsysno
                    {
                        sb.Append("vendor.").Append(key).Append(" like ").Append(Util.ToSqlLikeString(item.ToString())).Append(" and ");   //changed 20070411
                        string vendor = @"
										exists
										(
										select 
											top 1 po_master.sysno
										from 
											po_master, po_item, vendor
										where 
											po_master.sysno = po_item.posysno
										and	po_master.vendorsysno = vendor.sysno and vendorname like @vendorname
										and productsysno = product.sysno 
										)";
                        vendor = vendor.Replace("@vendorname", Util.ToSqlLikeString(item.ToString()));
                        sb.Append(vendor);
                    }
                    else if (key == "UnitCost" || key == "CurrentPrice" || key == "Point" || key == "Weight")
                    {
                        sb.Append(key).Append(item.ToString());
                    }
                    else if (key == "OnlineQty")
                    {
                        sb.Append("inventory.AvailableQty+inventory.VirtualQty").Append(item.ToString());
                    }
                    else if (key == "SafeQty")
                    {
                        sb.Append(key).Append(item.ToString());
                    }
                    else if (key == "AccountQtySafeQty")
                    {
                        sb.Append("Inventory_stock.AccountQty").Append(item.ToString()).Append("Inventory_stock.SafeQty");

                    }
                    else if (key == "MultiPicNum")
                    {
                        sb.Append("Product.MultiPicNum").Append(item.ToString());
                    }
                    else if (key == "AccountQty" || key == "AvailableQty" || key == "AllocatedQty"
                        || key == "PurchaseQty" || key == "OnlineQty" || key == "OrderQty" || key == "VirtualQty")
                    {
                        sb.Append("inventory.").Append(key).Append(item.ToString());
                    }
                    else if (key == "StockAccountQty")
                    {
                        sb.Append("inventory_stock.accountqty").Append(item.ToString());
                    }
                    else if (key == "monitorqty")
                    {
                        isBadInventory = 1;
                        sb.Append(" 1=1 ");
                    }

                    else if (key == "SaleTime")
                    {
                        if (paramHash.ContainsKey("StockSysNo"))
                        {
                            switch (item.ToString())
                            {
                                case "2d":
                                    //sqlSaleTime=" (product_saletrend.d1+product_saletrend.d2)<=0 and AccountQty>0";
                                    sqlSaleTime = " (product_saletrend.d1+product_saletrend.d2)<Inventory_Stock.AccountQty ";
                                    break;
                                case "3d":
                                    //sqlSaleTime=" (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3)<=0  and AccountQty>0";
                                    sqlSaleTime = " (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3)< Inventory_Stock.AccountQty";
                                    break;
                                case "4d":
                                    //sqlSaleTime=" (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3+product_saletrend.d4)<=0  and AccountQty>0";
                                    sqlSaleTime = " (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3+product_saletrend.d4)<Inventory_Stock.AccountQty";
                                    break;
                                case "5d":
                                    //sqlSaleTime=" (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3+product_saletrend.d4+product_saletrend.d5)<=0  and AccountQty>0";
                                    sqlSaleTime = " (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3+product_saletrend.d4+product_saletrend.d5)<Inventory_Stock.AccountQty";
                                    break;
                                case "6d":
                                    //sqlSaleTime=" (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3+product_saletrend.d4+product_saletrend.d5+product_saletrend.d6)<=0  and AccountQty>0";
                                    sqlSaleTime = " (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3+product_saletrend.d4+product_saletrend.d5+product_saletrend.d6)< Inventory_Stock.AccountQty";
                                    break;
                                case "1w":
                                    //sqlSaleTime=" (product_saletrend.w1<=0 and AccountQty)>0";
                                    sqlSaleTime = " product_saletrend.w1< Inventory_Stock.AccountQty";
                                    break;
                                case "2w":
                                    //sqlSaleTime=" (product_saletrend.w1+product_saletrend.w2)<=0 and AccountQty>0";
                                    sqlSaleTime = " (product_saletrend.w1+product_saletrend.w2)<Inventory_Stock.AccountQty";
                                    break;
                                case "3w":
                                    //sqlSaleTime=" (product_saletrend.w1+product_saletrend.w2+product_saletrend.w3)<=0 and AccountQty>0";
                                    sqlSaleTime = " (product_saletrend.w1+product_saletrend.w2+product_saletrend.w3)<Inventory_Stock.AccountQty";
                                    break;
                                case "1m":
                                    //sqlSaleTime=" product_saletrend.m1<=0 and AccountQty>0";
                                    sqlSaleTime = " product_saletrend.m1<Inventory_Stock.AccountQty";
                                    break;
                                case "2m":
                                    //sqlSaleTime=" (product_saletrend.m1+product_saletrend.m2)<=0 and AccountQty>0";
                                    sqlSaleTime = " (product_saletrend.m1+product_saletrend.m2)<Inventory_Stock.AccountQty";
                                    break;
                                default:
                                    //sqlSaleTime=" (product_saletrend.w1+product_saletrend.w2)<=0 and AccountQty>0";
                                    sqlSaleTime = " (product_saletrend.w1+product_saletrend.w2)<Inventory_Stock. AccountQty";
                                    break;
                            }
                        }
                        else
                        {
                            switch (item.ToString())
                            {
                                case "2d":
                                    //sqlSaleTime=" (product_saletrend.d1+product_saletrend.d2)<=0 and AccountQty>0";
                                    sqlSaleTime = " (product_saletrend.d1+product_saletrend.d2)<Inventory.AccountQty ";
                                    break;
                                case "3d":
                                    //sqlSaleTime=" (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3)<=0  and AccountQty>0";
                                    sqlSaleTime = " (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3)< Inventory.AccountQty";
                                    break;
                                case "4d":
                                    //sqlSaleTime=" (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3+product_saletrend.d4)<=0  and AccountQty>0";
                                    sqlSaleTime = " (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3+product_saletrend.d4)<Inventory. AccountQty";
                                    break;
                                case "5d":
                                    //sqlSaleTime=" (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3+product_saletrend.d4+product_saletrend.d5)<=0  and AccountQty>0";
                                    sqlSaleTime = " (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3+product_saletrend.d4+product_saletrend.d5)<Inventory.AccountQty";
                                    break;
                                case "6d":
                                    //sqlSaleTime=" (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3+product_saletrend.d4+product_saletrend.d5+product_saletrend.d6)<=0  and AccountQty>0";
                                    sqlSaleTime = " (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3+product_saletrend.d4+product_saletrend.d5+product_saletrend.d6)< Inventory.AccountQty";
                                    break;
                                case "1w":
                                    //sqlSaleTime=" (product_saletrend.w1<=0 and AccountQty)>0";
                                    sqlSaleTime = " product_saletrend.w1<Inventory. AccountQty";
                                    break;
                                case "2w":
                                    //sqlSaleTime=" (product_saletrend.w1+product_saletrend.w2)<=0 and AccountQty>0";
                                    sqlSaleTime = " (product_saletrend.w1+product_saletrend.w2)<Inventory.AccountQty";
                                    break;
                                case "3w":
                                    //sqlSaleTime=" (product_saletrend.w1+product_saletrend.w2+product_saletrend.w3)<=0 and AccountQty>0";
                                    sqlSaleTime = " (product_saletrend.w1+product_saletrend.w2+product_saletrend.w3)<Inventory.AccountQty";
                                    break;
                                case "1m":
                                    //sqlSaleTime=" product_saletrend.m1<=0 and AccountQty>0";
                                    sqlSaleTime = " product_saletrend.m1<Inventory.AccountQty";
                                    break;
                                case "2m":
                                    //sqlSaleTime=" (product_saletrend.m1+product_saletrend.m2)<=0 and AccountQty>0";
                                    sqlSaleTime = " (product_saletrend.m1+product_saletrend.m2)<Inventory.AccountQty";
                                    break;
                                default:
                                    //sqlSaleTime=" (product_saletrend.w1+product_saletrend.w2)<=0 and AccountQty>0";
                                    sqlSaleTime = " (product_saletrend.w1+product_saletrend.w2)<Inventory. AccountQty";
                                    break;
                            }
                        }
                        sb.Append("1=1");

                    }
                    else if (key == "SaleNull")
                    {
                        if (paramHash.ContainsKey("StockSysNo"))
                        {
                            isSaleNull = 1;
                            switch (item.ToString())
                            {
                                case "2d":
                                    sqlSaleNull = " (product_saletrend.d1+product_saletrend.d2)<=0 and Inventory_Stock.AccountQty>0";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddDays(-2) + "' and v.productsysno=product.sysno) ";
                                    break;
                                case "3d":
                                    sqlSaleNull = " (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3)<=0  and Inventory_Stock.AccountQty>0";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddDays(-3) + "' and v.productsysno=product.sysno) ";
                                    break;
                                case "4d":
                                    sqlSaleNull = " (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3+product_saletrend.d4)<=0  and Inventory_Stock.AccountQty>0";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddDays(-4) + "' and v.productsysno=product.sysno) ";
                                    break;
                                case "5d":
                                    sqlSaleNull = " (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3+product_saletrend.d4+product_saletrend.d5)<=0  and Inventory_Stock.AccountQty>0";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddDays(-5) + "' and v.productsysno=product.sysno) ";
                                    break;
                                case "6d":
                                    sqlSaleNull = " (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3+product_saletrend.d4+product_saletrend.d5+product_saletrend.d6)<=0  and Inventory_Stock.AccountQty>0";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddDays(-6) + "' and v.productsysno=product.sysno) ";
                                    break;
                                case "1w":
                                    sqlSaleNull = " (product_saletrend.w1<=0 and Inventory_Stock.AccountQty)>0";
                                    sqlSaleNull += " and exists(select po_item.productsysno from po_master,po_item where po_master.sysno=po_item.posysno and po_item.productsysno=product.sysno and po_master.intime < '" + DateTime.Now.AddDays(-7) + "') ";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddDays(-7) + "' and v.productsysno=product.sysno) ";
                                    break;
                                case "2w":
                                    sqlSaleNull = " (product_saletrend.w1+product_saletrend.w2)<=0 and Inventory_Stock.AccountQty>0";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddDays(-14) + "' and v.productsysno=product.sysno) ";
                                    break;
                                case "3w":
                                    sqlSaleNull = " (product_saletrend.w1+product_saletrend.w2+product_saletrend.w3)<=0 and Inventory_Stock.AccountQty>0";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddDays(-21) + "' and v.productsysno=product.sysno) ";
                                    break;
                                case "1m":
                                    sqlSaleNull = " product_saletrend.m1<=0 and Inventory_Stock.AccountQty>0";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddMonths(-1) + "' and v.productsysno=product.sysno) ";
                                    break;
                                case "2m":
                                    sqlSaleNull = " (product_saletrend.m1+product_saletrend.m2)<=0 and Inventory_Stock.AccountQty>0";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddMonths(-2) + "' and v.productsysno=product.sysno) ";
                                    break;
                                default:
                                    sqlSaleNull = " (product_saletrend.w1+product_saletrend.w2)<=0 and Inventory_Stock.AccountQty>0";
                                    break;
                            }
                        }

                        else
                        {
                            isSaleNull = 1;
                            switch (item.ToString())
                            {
                                case "2d":
                                    sqlSaleNull = " (product_saletrend.d1+product_saletrend.d2)<=0 and Inventory.AccountQty>0";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddDays(-2) + "' and v.productsysno=product.sysno) ";
                                    break;
                                case "3d":
                                    sqlSaleNull = " (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3)<=0  and Inventory.AccountQty>0";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddDays(-3) + "' and v.productsysno=product.sysno) ";
                                    break;
                                case "4d":
                                    sqlSaleNull = " (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3+product_saletrend.d4)<=0  and Inventory.AccountQty>0";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddDays(-4) + "' and v.productsysno=product.sysno) ";
                                    break;
                                case "5d":
                                    sqlSaleNull = " (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3+product_saletrend.d4+product_saletrend.d5)<=0  and Inventory.AccountQty>0";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddDays(-5) + "' and v.productsysno=product.sysno) ";
                                    break;
                                case "6d":
                                    sqlSaleNull = " (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3+product_saletrend.d4+product_saletrend.d5+product_saletrend.d6)<=0  and Inventory.AccountQty>0";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddDays(-6) + "' and v.productsysno=product.sysno) ";
                                    break;
                                case "1w":
                                    sqlSaleNull = " (product_saletrend.w1<=0 and Inventory.AccountQty)>0";
                                    sqlSaleNull += " and exists(select po_item.productsysno from po_master,po_item where po_master.sysno=po_item.posysno and po_item.productsysno=product.sysno and po_master.intime < '" + DateTime.Now.AddDays(-7) + "') ";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddDays(-7) + "' and v.productsysno=product.sysno) ";
                                    break;
                                case "2w":
                                    sqlSaleNull = " (product_saletrend.w1+product_saletrend.w2)<=0 and Inventory.AccountQty>0";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddDays(-14) + "' and v.productsysno=product.sysno) ";
                                    break;
                                case "3w":
                                    sqlSaleNull = " (product_saletrend.w1+product_saletrend.w2+product_saletrend.w3)<=0 and Inventory.AccountQty>0";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddDays(-21) + "' and v.productsysno=product.sysno) ";
                                    break;
                                case "1m":
                                    sqlSaleNull = " product_saletrend.m1<=0 and Inventory.AccountQty>0";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddMonths(-1) + "' and v.productsysno=product.sysno) ";
                                    break;
                                case "2m":
                                    sqlSaleNull = " (product_saletrend.m1+product_saletrend.m2)<=0 and Inventory.AccountQty>0";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddMonths(-2) + "' and v.productsysno=product.sysno) ";
                                    break;
                                default:
                                    sqlSaleNull = " (product_saletrend.w1+product_saletrend.w2)<=0 and Inventory.AccountQty>0";
                                    break;
                            }
                        }

                        sb.Append("1=1");
                    }
                    else if (item is int)
                    {
                        sb.Append(key).Append("=").Append(item.ToString());
                    }
                    else if (item is string)
                    {
                        sb.Append(key).Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                    }
                }
                if (isBadInventory == 1)
                    sb.Append(" and " + sqlSaleTime);
                if (isSaleNull == 1)
                {
                    sb.Append(" and " + sqlSaleNull);
                }
                sql += sb.ToString();
            }
            else
            {
                sql = sql.Replace("select", "select top 50");
            }

            //sql += " order by productid"; 
            sql += " order by  monitorqty DESC";
            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet ProductDetailExcell(Hashtable paramHash)
        {
            //select 出来的sysno默认是ProductSysNo, 不能改

            string sql0 = @"    select 
                                ProductDescLong,weight,MultiPicNum, pas.summary as Attribute ,product.createtime,
                                DefaultVendorSysNo,DefaultPurchasePrice,Product2ndType,MasterProductSysNo,
                                ProductColor,ProductSize,ProducingArea,PackQuantity,
                                MinOrderQuantity,IsStoreFrontSale,SaleUnit, StorageDay,
                                product.sysno, productid, productname, productmode as productmodel,promotionword,
								unitcost, currentprice, point, lastorderprice,inventory_stock.safeqty,
								inventory.orderqty, inventory.accountqty, inventory.allocatedqty, 
								inventory.availableqty, inventory.virtualqty, inventory.availableqty+inventory.virtualqty onlineqty, 
								inventory.purchaseqty, product_saletrend.*,product_saletrend.w1+product_saletrend.w2+0 as monitorqty,
								product.status, sys_user.username as pmUserName, VendorName , product_rmapercent.rmapercent,
                                category3.c3id,category3.c3name,manufacturer.manufacturerid,manufacturer.manufacturername,
                                product.barcode,vendor.sysno as vendorsysno,vendor.vendorname";
            //string sql00 = ", '0' as stockaccountqty, '0' as safeqty";

            string sql1 = @" from product 
                                inner join category3 on product.c3sysno = category3.sysno 
								inner join category2 on category3.c2sysno = category2.sysno 
								inner join category1 on category2.c1sysno = category1.sysno
                                left join Product_Attribute2_Summary pas on pas.productsysno=product.sysno 
								left join manufacturer on product.manufacturersysno = manufacturer.sysno
								left join product_price on product.sysno = product_price.productsysno
								left join product_status on product.sysno = product_status.productsysno
								left join sys_user on product.pmusersysno = sys_user.sysno
								left join inventory on product.sysno = inventory.productsysno
								left join product_saletrend on product.sysno = product_saletrend.productsysno
								left join vendor on product.DefaultVendorSysNo = vendor.sysno
                                left join inventory_stock on product.sysno = inventory_stock.productsysno 
                                left join product_rmapercent on product.sysno = product_rmapercent.rmapercent ";
            string sql2 = @"
							where 
								1=1
							";

            string sql = sql0 + sql1 + sql2;

            if (paramHash != null && paramHash.Count != 0)
            {
                // Uncomment by Judy
                //if ( paramHash.ContainsKey("StockSysNo") )
                //{
                //    sql0 = sql0 + ", inventory_stock.accountqty as stockaccountqty";
                ////  sql1 = sql1 + " left join inventory_stock on product.sysno = inventory_stock.productsysno"; 
                //    sql = sql0 + sql1 + sql2;
                //}

                StringBuilder sb = new StringBuilder(100);
                string sqlSaleTime = "";
                string sqlSaleNull = "";
                int isBadInventory = 0;
                int isSaleNull = 0;
                foreach (string key in paramHash.Keys)
                {
                    sb.Append(" and ");
                    object item = paramHash[key];
                    if (key == "SysNo")
                    {
                        sb.Append("product.").Append(key).Append("=").Append(item.ToString());
                    }
                    else if (key == "Status")
                    {
                        sb.Append("product.").Append(key).Append(item.ToString());
                    }
                    else if (key == "DateFrom")
                    {
                        sb.Append("product.CreateTime >= ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "DateTo")
                    {

                        sb.Append("product.CreateTime <= ").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    //Add by Judy
                    //-------------------------------------------------------------------------------------
                    else if (key == "StockSysNo")
                    {
                        sb.Append("inventory_stock.stocksysno").Append("=").Append(item.ToString());
                    }
                    //-------------------------------------------------------------------------------------

                    else if (key == "VendorName")		//如果查询慢，可以改为用直接在product.lastvendorsysno
                    {
                        sb.Append("vendor.").Append(key).Append(" like ").Append(Util.ToSqlLikeString(item.ToString())).Append(" and ");   //changed 20070411
                        string vendor = @"
										exists
										(
										select 
											top 1 po_master.sysno
										from 
											po_master, po_item, vendor
										where 
											po_master.sysno = po_item.posysno
										and	po_master.vendorsysno = vendor.sysno and vendorname like @vendorname
										and productsysno = product.sysno 
										)";
                        vendor = vendor.Replace("@vendorname", Util.ToSqlLikeString(item.ToString()));
                        sb.Append(vendor);
                    }
                    else if (key == "UnitCost" || key == "CurrentPrice" || key == "Point" || key == "Weight")
                    {
                        sb.Append(key).Append(item.ToString());
                    }
                    else if (key == "OnlineQty")
                    {
                        sb.Append("inventory.AvailableQty+inventory.VirtualQty").Append(item.ToString());
                    }
                    else if (key == "SafeQty")
                    {
                        sb.Append(key).Append(item.ToString());
                    }
                    else if (key == "AccountQtySafeQty")
                    {
                        sb.Append("Inventory_stock.AccountQty").Append(item.ToString()).Append("Inventory_stock.SafeQty");

                    }
                    else if (key == "MultiPicNum")
                    {
                        sb.Append("Product.MultiPicNum").Append(item.ToString());
                    }
                    else if (key == "AccountQty" || key == "AvailableQty" || key == "AllocatedQty"
                        || key == "PurchaseQty" || key == "OnlineQty" || key == "OrderQty" || key == "VirtualQty")
                    {
                        sb.Append("inventory.").Append(key).Append(item.ToString());
                    }
                    else if (key == "StockAccountQty")
                    {
                        sb.Append("inventory_stock.accountqty").Append(item.ToString());
                    }
                    else if (key == "monitorqty")
                    {
                        isBadInventory = 1;
                        sb.Append(" 1=1 ");
                    }
                    else if (key == "SaleTime")
                    {
                        if (paramHash.ContainsKey("StockSysNo"))
                        {
                            switch (item.ToString())
                            {
                                case "2d":
                                    //sqlSaleTime=" (product_saletrend.d1+product_saletrend.d2)<=0 and AccountQty>0";
                                    sqlSaleTime = " (product_saletrend.d1+product_saletrend.d2)<Inventory_Stock.AccountQty ";
                                    break;
                                case "3d":
                                    //sqlSaleTime=" (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3)<=0  and AccountQty>0";
                                    sqlSaleTime = " (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3)< Inventory_Stock.AccountQty";
                                    break;
                                case "4d":
                                    //sqlSaleTime=" (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3+product_saletrend.d4)<=0  and AccountQty>0";
                                    sqlSaleTime = " (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3+product_saletrend.d4)<Inventory_Stock.AccountQty";
                                    break;
                                case "5d":
                                    //sqlSaleTime=" (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3+product_saletrend.d4+product_saletrend.d5)<=0  and AccountQty>0";
                                    sqlSaleTime = " (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3+product_saletrend.d4+product_saletrend.d5)<Inventory_Stock.AccountQty";
                                    break;
                                case "6d":
                                    //sqlSaleTime=" (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3+product_saletrend.d4+product_saletrend.d5+product_saletrend.d6)<=0  and AccountQty>0";
                                    sqlSaleTime = " (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3+product_saletrend.d4+product_saletrend.d5+product_saletrend.d6)< Inventory_Stock.AccountQty";
                                    break;
                                case "1w":
                                    //sqlSaleTime=" (product_saletrend.w1<=0 and AccountQty)>0";
                                    sqlSaleTime = " product_saletrend.w1< Inventory_Stock.AccountQty";
                                    break;
                                case "2w":
                                    //sqlSaleTime=" (product_saletrend.w1+product_saletrend.w2)<=0 and AccountQty>0";
                                    sqlSaleTime = " (product_saletrend.w1+product_saletrend.w2)<Inventory_Stock.AccountQty";
                                    break;
                                case "3w":
                                    //sqlSaleTime=" (product_saletrend.w1+product_saletrend.w2+product_saletrend.w3)<=0 and AccountQty>0";
                                    sqlSaleTime = " (product_saletrend.w1+product_saletrend.w2+product_saletrend.w3)<Inventory_Stock.AccountQty";
                                    break;
                                case "1m":
                                    //sqlSaleTime=" product_saletrend.m1<=0 and AccountQty>0";
                                    sqlSaleTime = " product_saletrend.m1<Inventory_Stock.AccountQty";
                                    break;
                                case "2m":
                                    //sqlSaleTime=" (product_saletrend.m1+product_saletrend.m2)<=0 and AccountQty>0";
                                    sqlSaleTime = " (product_saletrend.m1+product_saletrend.m2)<Inventory_Stock.AccountQty";
                                    break;
                                default:
                                    //sqlSaleTime=" (product_saletrend.w1+product_saletrend.w2)<=0 and AccountQty>0";
                                    sqlSaleTime = " (product_saletrend.w1+product_saletrend.w2)<Inventory_Stock. AccountQty";
                                    break;
                            }
                        }
                        else
                        {
                            switch (item.ToString())
                            {
                                case "2d":
                                    //sqlSaleTime=" (product_saletrend.d1+product_saletrend.d2)<=0 and AccountQty>0";
                                    sqlSaleTime = " (product_saletrend.d1+product_saletrend.d2)<Inventory.AccountQty ";
                                    break;
                                case "3d":
                                    //sqlSaleTime=" (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3)<=0  and AccountQty>0";
                                    sqlSaleTime = " (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3)< Inventory.AccountQty";
                                    break;
                                case "4d":
                                    //sqlSaleTime=" (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3+product_saletrend.d4)<=0  and AccountQty>0";
                                    sqlSaleTime = " (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3+product_saletrend.d4)<Inventory. AccountQty";
                                    break;
                                case "5d":
                                    //sqlSaleTime=" (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3+product_saletrend.d4+product_saletrend.d5)<=0  and AccountQty>0";
                                    sqlSaleTime = " (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3+product_saletrend.d4+product_saletrend.d5)<Inventory.AccountQty";
                                    break;
                                case "6d":
                                    //sqlSaleTime=" (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3+product_saletrend.d4+product_saletrend.d5+product_saletrend.d6)<=0  and AccountQty>0";
                                    sqlSaleTime = " (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3+product_saletrend.d4+product_saletrend.d5+product_saletrend.d6)< Inventory.AccountQty";
                                    break;
                                case "1w":
                                    //sqlSaleTime=" (product_saletrend.w1<=0 and AccountQty)>0";
                                    sqlSaleTime = " product_saletrend.w1<Inventory. AccountQty";
                                    break;
                                case "2w":
                                    //sqlSaleTime=" (product_saletrend.w1+product_saletrend.w2)<=0 and AccountQty>0";
                                    sqlSaleTime = " (product_saletrend.w1+product_saletrend.w2)<Inventory.AccountQty";
                                    break;
                                case "3w":
                                    //sqlSaleTime=" (product_saletrend.w1+product_saletrend.w2+product_saletrend.w3)<=0 and AccountQty>0";
                                    sqlSaleTime = " (product_saletrend.w1+product_saletrend.w2+product_saletrend.w3)<Inventory.AccountQty";
                                    break;
                                case "1m":
                                    //sqlSaleTime=" product_saletrend.m1<=0 and AccountQty>0";
                                    sqlSaleTime = " product_saletrend.m1<Inventory.AccountQty";
                                    break;
                                case "2m":
                                    //sqlSaleTime=" (product_saletrend.m1+product_saletrend.m2)<=0 and AccountQty>0";
                                    sqlSaleTime = " (product_saletrend.m1+product_saletrend.m2)<Inventory.AccountQty";
                                    break;
                                default:
                                    //sqlSaleTime=" (product_saletrend.w1+product_saletrend.w2)<=0 and AccountQty>0";
                                    sqlSaleTime = " (product_saletrend.w1+product_saletrend.w2)<Inventory. AccountQty";
                                    break;
                            }
                        }
                        sb.Append("1=1");

                    }
                    else if (key == "SaleNull")
                    {
                        if (paramHash.ContainsKey("StockSysNo"))
                        {
                            isSaleNull = 1;
                            switch (item.ToString())
                            {
                                case "2d":
                                    sqlSaleNull = " (product_saletrend.d1+product_saletrend.d2)<=0 and Inventory_Stock.AccountQty>0";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddDays(-2) + "' and v.productsysno=product.sysno) ";
                                    break;
                                case "3d":
                                    sqlSaleNull = " (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3)<=0  and Inventory_Stock.AccountQty>0";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddDays(-3) + "' and v.productsysno=product.sysno) ";
                                    break;
                                case "4d":
                                    sqlSaleNull = " (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3+product_saletrend.d4)<=0  and Inventory_Stock.AccountQty>0";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddDays(-4) + "' and v.productsysno=product.sysno) ";
                                    break;
                                case "5d":
                                    sqlSaleNull = " (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3+product_saletrend.d4+product_saletrend.d5)<=0  and Inventory_Stock.AccountQty>0";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddDays(-5) + "' and v.productsysno=product.sysno) ";
                                    break;
                                case "6d":
                                    sqlSaleNull = " (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3+product_saletrend.d4+product_saletrend.d5+product_saletrend.d6)<=0  and Inventory_Stock.AccountQty>0";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddDays(-6) + "' and v.productsysno=product.sysno) ";
                                    break;
                                case "1w":
                                    sqlSaleNull = " (product_saletrend.w1<=0 and Inventory_Stock.AccountQty)>0";
                                    sqlSaleNull += " and exists(select po_item.productsysno from po_master,po_item where po_master.sysno=po_item.posysno and po_item.productsysno=product.sysno and po_master.intime < '" + DateTime.Now.AddDays(-7) + "') ";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddDays(-7) + "' and v.productsysno=product.sysno) ";
                                    break;
                                case "2w":
                                    sqlSaleNull = " (product_saletrend.w1+product_saletrend.w2)<=0 and Inventory_Stock.AccountQty>0";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddDays(-14) + "' and v.productsysno=product.sysno) ";
                                    break;
                                case "3w":
                                    sqlSaleNull = " (product_saletrend.w1+product_saletrend.w2+product_saletrend.w3)<=0 and Inventory_Stock.AccountQty>0";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddDays(-21) + "' and v.productsysno=product.sysno) ";
                                    break;
                                case "1m":
                                    sqlSaleNull = " product_saletrend.m1<=0 and Inventory_Stock.AccountQty>0";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddMonths(-1) + "' and v.productsysno=product.sysno) ";
                                    break;
                                case "2m":
                                    sqlSaleNull = " (product_saletrend.m1+product_saletrend.m2)<=0 and Inventory_Stock.AccountQty>0";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddMonths(-2) + "' and v.productsysno=product.sysno) ";
                                    break;
                                default:
                                    sqlSaleNull = " (product_saletrend.w1+product_saletrend.w2)<=0 and Inventory_Stock.AccountQty>0";
                                    break;
                            }
                        }

                        else
                        {
                            isSaleNull = 1;
                            switch (item.ToString())
                            {
                                case "2d":
                                    sqlSaleNull = " (product_saletrend.d1+product_saletrend.d2)<=0 and Inventory.AccountQty>0";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddDays(-2) + "' and v.productsysno=product.sysno) ";
                                    break;
                                case "3d":
                                    sqlSaleNull = " (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3)<=0  and Inventory.AccountQty>0";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddDays(-3) + "' and v.productsysno=product.sysno) ";
                                    break;
                                case "4d":
                                    sqlSaleNull = " (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3+product_saletrend.d4)<=0  and Inventory.AccountQty>0";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddDays(-4) + "' and v.productsysno=product.sysno) ";
                                    break;
                                case "5d":
                                    sqlSaleNull = " (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3+product_saletrend.d4+product_saletrend.d5)<=0  and Inventory.AccountQty>0";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddDays(-5) + "' and v.productsysno=product.sysno) ";
                                    break;
                                case "6d":
                                    sqlSaleNull = " (product_saletrend.d1+product_saletrend.d2+product_saletrend.d3+product_saletrend.d4+product_saletrend.d5+product_saletrend.d6)<=0  and Inventory.AccountQty>0";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddDays(-6) + "' and v.productsysno=product.sysno) ";
                                    break;
                                case "1w":
                                    sqlSaleNull = " (product_saletrend.w1<=0 and Inventory.AccountQty)>0";
                                    sqlSaleNull += " and exists(select po_item.productsysno from po_master,po_item where po_master.sysno=po_item.posysno and po_item.productsysno=product.sysno and po_master.intime < '" + DateTime.Now.AddDays(-7) + "') ";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddDays(-7) + "' and v.productsysno=product.sysno) ";
                                    break;
                                case "2w":
                                    sqlSaleNull = " (product_saletrend.w1+product_saletrend.w2)<=0 and Inventory.AccountQty>0";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddDays(-14) + "' and v.productsysno=product.sysno) ";
                                    break;
                                case "3w":
                                    sqlSaleNull = " (product_saletrend.w1+product_saletrend.w2+product_saletrend.w3)<=0 and Inventory.AccountQty>0";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddDays(-21) + "' and v.productsysno=product.sysno) ";
                                    break;
                                case "1m":
                                    sqlSaleNull = " product_saletrend.m1<=0 and Inventory.AccountQty>0";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddMonths(-1) + "' and v.productsysno=product.sysno) ";
                                    break;
                                case "2m":
                                    sqlSaleNull = " (product_saletrend.m1+product_saletrend.m2)<=0 and Inventory.AccountQty>0";
                                    sqlSaleNull += " and exists(select v.productsysno from (select po_item.productsysno,max(po_master.intime) as LastInTime from po_master,po_item where po_master.sysno=po_item.posysno group by po_item.productsysno) v where v.lastintime < '" + DateTime.Now.AddMonths(-2) + "' and v.productsysno=product.sysno) ";
                                    break;
                                default:
                                    sqlSaleNull = " (product_saletrend.w1+product_saletrend.w2)<=0 and Inventory.AccountQty>0";
                                    break;
                            }
                        }

                        sb.Append("1=1");
                    }
                    else if (item is int)
                    {
                        sb.Append(key).Append("=").Append(item.ToString());
                    }
                    else if (item is string)
                    {
                        sb.Append(key).Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                    }
                }
                if (isBadInventory == 1)
                    sb.Append(" and " + sqlSaleTime);
                if (isSaleNull == 1)
                {
                    sb.Append(" and " + sqlSaleNull);
                }
                sql += sb.ToString();

            }
            else
            {
                sql = sql.Replace("select", "select top 50");
            }

            //sql += " order by productid"; 
            sql += " order by  monitorqty DESC";
            DataSet productdetail = SqlHelper.ExecuteDataSet(sql);
            return productdetail;

            //            string sqlExcell = @"select 
            //                                productid,productname,productDesc,weight,MultiPicNum Product_Attribute2_Summary.summary as Attribute 
            //                         from   
            //                                product,Product_Attribute2_Summary
            //                         where  product.sysno=Product_Attribute2_Summary.productsysno and product.sysno in productdetail";
            //            return SqlHelper.ExecuteDataSet(sqlExcell);
        }

        public Hashtable GetSOItemPriceList(SOInfo soInfo)
        {
            Hashtable ht = new Hashtable();
            string sql = @"select pp.* 
						   from product_price pp 
						   inner join so_item si on si.productSysNo = pp.productSysNo
						   where si.sosysno=" + soInfo.SysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ProductPriceInfo ppInfo = new ProductPriceInfo();
                    this.mapPrice(ppInfo, dr);
                    ht.Add(ppInfo.ProductSysNo, ppInfo);
                }
            }
            return ht;
        }

        public void SetLastOrderPrice(int productSysNo, decimal lastOrderPrice)
        {
            new ProductPriceDac().UpdateLastOrderPrice(productSysNo, lastOrderPrice);
        }
        public void SetLastVendor(int productSysNo, int lastVendorSysNo)
        {
            new ProductBasicDac().UpdateLastVendor(productSysNo, lastVendorSysNo);
        }

        public void SetLastMarketLowestPrice(int productSysNo, decimal lastMarketLowestPrice)
        {
            new ProductPriceDac().UpdateLastMarketLowestPrice(productSysNo, lastMarketLowestPrice);
        }

        /// <summary>
        /// 获取商品对应的新品、二手、坏品的系统编号
        /// </summary>
        /// <param name="productSysNo"></param>
        /// <param name="relatedType"></param>
        /// <returns></returns>
        public int GetRelatedProductSysNo(int productSysNo, int relatedType)
        {
            int relatedSysNo = AppConst.IntNull;
            ProductBasicInfo pbInfo = this.LoadBasicBrief(productSysNo);
            if (pbInfo != null)
            {
                string normalID = pbInfo.ProductID.ToUpper().TrimEnd('R').TrimEnd('B');
                string relatedID = AppConst.StringNull;
                if (relatedType == (int)AppEnum.ProductType.Normal)
                    relatedID = normalID;
                else if (relatedType == (int)AppEnum.ProductType.SecondHand)
                    relatedID = normalID + "R";
                else if (relatedType == (int)AppEnum.ProductType.Bad)
                    relatedID = normalID + "B";
                ProductBasicInfo relatedInfo = this.LoadBasicBrief(relatedID);
                if (relatedInfo != null)
                    relatedSysNo = relatedInfo.SysNo;
            }
            return relatedSysNo;
        }

        /// <summary>
        /// 获取指定商品的信息
        /// </summary>
        /// <param name="ProductSysNos"></param>
        /// <returns></returns>
        public DataTable GetProductsInfo(string ProductSysNos)
        {
            string[] PBs = ProductSysNos.Split(',');
            string sql = @"select sysno,productid,productname,PromotionWord,briefname from product where 1=1 ";
            for (int i = 0; i < PBs.Length; i++)
            {
                sql += " or sysno = " + PBs[i];
            }
            sql += " order by sysno desc";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            return ds.Tables[0];
        }

        private void mapPriceRange(ProductPriceRangeInfo oParam, DataRow dr)
        {
            oParam.SysNo = int.Parse(dr["SysNo"].ToString().Trim());
            oParam.RangeID = int.Parse(dr["RangeID"].ToString().Trim());
            oParam.RangeName = dr["RangeName"].ToString().Trim();
            oParam.RangeFrom = int.Parse(dr["RangeTo"].ToString().Trim());
            oParam.RangeTo = int.Parse(dr["RangeTo"].ToString().Trim());
        }

        public ProductPriceRangeInfo LoadPriceRange(int sysno)
        {
            string sql = "select sysno,rangeid,rangename,rangefrom,rangeto from product_price_range where sysno=" + sysno.ToString();
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            ProductPriceRangeInfo ppr = new ProductPriceRangeInfo();
            if (Util.HasMoreRow(ds))
            {
                mapPriceRange(ppr, ds.Tables[0].Rows[0]);
            }
            else
            {
                ppr = null;
            }
            return ppr;
        }

        //查询市场低价信息
        public ProductPriceMarketInfo LoadPriceMarket(int sysno)
        {
            string sql = "select * from Product_Price_Market where sysno=" + sysno.ToString();
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            ProductPriceMarketInfo ppr = new ProductPriceMarketInfo();
            if (Util.HasMoreRow(ds))
            {
                mapProductPriceMarket(ppr, ds.Tables[0].Rows[0]);
            }
            else
            {
                ppr = null;
            }
            return ppr;
        }

        /// <summary>
        /// 取出所有价格区间信息
        /// </summary>
        /// <returns></returns>
        public SortedList GetProductPriceRanges()
        {
            string sql = "select sysno,rangeid,rangename,rangefrom,rangeto from product_price_range order by rangeid";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            SortedList st = new SortedList();
            if (!Util.HasMoreRow(ds))
                return st;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ProductPriceRangeInfo ppr = new ProductPriceRangeInfo();
                mapPriceRange(ppr, dr);
                st.Add(ppr, null);
            }
            return st;
        }

        public int ExecSql(string sql)
        {
            return SqlHelper.ExecuteNonQuery(sql);
        }

        public void ExecSql(ArrayList al)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                for (int i = 0; i < al.Count; i++)
                {
                    SqlHelper.ExecuteNonQuery(al[i].ToString());
                }
                scope.Complete();
            }
        }

        /// <summary>
        /// 根据SysNo列表获取状态为Show的商品信息
        /// </summary>
        /// <param name="sysNoList"></param>
        /// <returns></returns>
        public ArrayList GetBrowseHistoryList(string sysNoList)
        {
            if (sysNoList == null || sysNoList == string.Empty)
            {
                return null;
            }
            string sql = @"SELECT  sysNo, ProductID, ProductName,PromotionWord,briefname  
									FROM product (NOLOCK) 
									WHERE Status = " + (int)AppEnum.ProductStatus.Show + " AND sysno in (" + sysNoList + ")";

            try
            {
                DataSet ds = SqlHelper.ExecuteDataSet(sql);

                if (!Util.HasMoreRow(ds))
                    return null;

                //将数据库获取的数据放入Hashtable
                Hashtable ht = new Hashtable(10);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ProductBasicInfo oP = new ProductBasicInfo();
                    oP.SysNo = Util.TrimIntNull(dr["SysNo"]);
                    oP.ProductID = Util.TrimNull(dr["ProductID"]);
                    oP.ProductName = Util.TrimNull(dr["ProductName"]);
                    ht.Add(oP.SysNo.ToString(), oP);
                }

                //根据次序进行排序
                ArrayList al = new ArrayList(10);
                string[] sysArr = sysNoList.Split(',');
                for (int i = 0; i < sysArr.Length; i++)
                {
                    if (ht.Contains(sysArr[i])) //有可能商品Status变化,所以ht会比cookie中的少
                        al.Add(ht[sysArr[i]]);
                }

                return al;
            }
            catch
            {
                return null;
            }
        }

        public DataSet GetBrowseHistoryListDs(string sysNoList)
        {
            if (sysNoList == null || sysNoList == string.Empty)
            {
                return null;
            }
            string sql = @"SELECT  sysNo, ProductID, ProductName,PromotionWord,briefname  
									FROM product (NOLOCK) 
									WHERE Status = " + (int)AppEnum.ProductStatus.Show + " and sysno in (" + sysNoList + ")";

            try
            {
                DataSet ds = SqlHelper.ExecuteDataSet(sql);

                if (!Util.HasMoreRow(ds))
                    return ds;
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }

        #region 相关商品
        public int InsertProductRelated(ProductRelatedInfo oParam)
        {
            return new ProductRelatedDac().Insert(oParam);
        }

        public int UpdateProductRelated(ProductRelatedInfo oParam)
        {
            return new ProductRelatedDac().Update(oParam);
        }

        private void map(ProductRelatedInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.MasterProductSysNo = Util.TrimIntNull(tempdr["MasterProductSysNo"]);
            oParam.RelatedProductSysNo = Util.TrimIntNull(tempdr["RelatedProductSysNo"]);
            oParam.CreateUserSysNo = Util.TrimIntNull(tempdr["CreateUserSysNo"]);
            oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
        }
        #endregion

        public DataSet GetProductCompare(string[] ssysno)
        {
            StringBuilder sb = new StringBuilder();
            int nn = ssysno.Length;

            for (int i = 0; i < nn; i++)
            {
                int sysno = Int32.Parse(ssysno[i]);
                sb.Append(@"SELECT 
                            category_attribute1.ordernum,
                            category_attribute2.ordernum, 
                            product_attribute2.productsysno,
                            category_attribute1.sysno as attribute1sysno,
                            attribute1name,
                            attribute2name,
                            attribute2type,
                            case category_attribute2.attribute2type when '0' then product_attribute2.attribute2value else category_attribute2_option.attribute2optionname end as attribute2value
                            FROM 
                             Product_Attribute2 
                            INNER JOIN Category_Attribute2 
                             ON Product_Attribute2.Attribute2SysNo = Category_Attribute2.SysNo
                            INNER JOIN Category_Attribute1 
                             ON Category_Attribute2.A1SysNo = Category_Attribute1.SysNo
                            LEFT JOIN Category_Attribute2_Option 
                             ON Product_Attribute2.Attribute2OptionSysNo = Category_Attribute2_Option.SysNo
                            WHERE category_attribute1.status=0 and category_attribute2.status=0 and isnull(category_attribute2_option.status,0)=0 and ProductSysNo = @sysno  
                            ORDER BY category_attribute1.ordernum,category_attribute2.ordernum;");
                sb = sb.Replace("@sysno", sysno.ToString());
            }
            return SqlHelper.ExecuteDataSet(sb.ToString());
        }

        private void mapProductPriceMarket(ProductPriceMarketInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.ProductSysNo = Util.TrimIntNull(tempdr["ProductSysNo"]);
            oParam.MarketLowestPrice = Util.TrimDecimalNull(tempdr["MarketLowestPrice"]);
            oParam.MarketUrl = Util.TrimNull(tempdr["MarketUrl"]);
            oParam.CreateMemo = Util.TrimNull(tempdr["CreateMemo"]);
            oParam.CreateUserSysNo = Util.TrimIntNull(tempdr["CreateUserSysNo"]);
            oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
            oParam.AuditMemo = Util.TrimNull(tempdr["AuditMemo"]);
            oParam.AuditUserSysNo = Util.TrimIntNull(tempdr["AuditUserSysNo"]);
            oParam.AuditTime = Util.TrimDateNull(tempdr["AuditTime"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
        }

        public void InsertProductPriceMarket(ProductPriceMarketInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                new ProductPriceMarketDac().Insert(oParam);

                SetLastMarketLowestPrice(oParam.ProductSysNo, oParam.MarketLowestPrice);

                scope.Complete();
            }
        }

        public int UpdateProductPriceMarket(ProductPriceMarketInfo oParam)
        {
            return new ProductPriceMarketDac().Update(oParam);
        }

        //批量删除低价信息
        public int DeleteProductPriceMarket(string priceSysNos)
        {
            return new ProductPriceMarketDac().Deletes(priceSysNos);
        }

        public DataSet GetProductPriceMarketDs(int productSysNo)
        {
            string sql = @"select ppm.*,u.username as createusername from product_price_market ppm 
                           left join sys_user u on ppm.createusersysno=u.sysno where ppm.productsysno=" + productSysNo + " order by ppm.sysno desc";
            return SqlHelper.ExecuteDataSet(sql);
        }

        //[AjaxPro.AjaxMethod]
        //public static string GetProductNameBySysNo(int SysNo)
        //{
        //    string sql = "select productname from product where sysno=" + SysNo;
        //    return SqlHelper.ExecuteScalar(sql).ToString();
        //}
        public DataSet GetProductBarcodeDs(int productsysno, string barcode)
        {
            string sql = @"select * from product_barcode where productsysno=" + productsysno + " and barcode='" + barcode + "'";
            return SqlHelper.ExecuteDataSet(sql);
        }
        public DataSet GetProductBarcodeDs(string productid, string barcode)
        {
            string sql = @"select * from product_barcode left join product on product.productid=" + productid + " where productsysno=" + productid + " and product_barcode.barcode='" + barcode + "'";
            return SqlHelper.ExecuteDataSet(sql);
        }

        public void AutoUpdateSecondHandProductPrice()
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                string sql = @" select distinct product.sysno as productsysno,username,productname,currentprice,FLOOR(currentprice*0.99) as newprice from 
							    product 
                                left join sys_user on product.pmusersysno = sys_user.sysno 
							    left join product_price on product.sysno = product_price.productsysno
							    left join product_status on product.sysno = product_status.productsysno
							    left join inventory on product.sysno = inventory.productsysno
			                    left join inventory_stock on product.sysno = inventory_stock.productsysno
						        where inventory_stock.stocksysno=1 and inventory.AccountQty>=1 
                                and ProductType=@producttype and product.Status=@productstatus 
                                order by sys_user.username,product.sysno";
                sql = sql.Replace("@producttype", ((int)AppEnum.ProductType.SecondHand).ToString());
                sql = sql.Replace("@productstatus", ((int)AppEnum.ProductStatus.Show).ToString());

                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (Util.HasMoreRow(ds))
                {
                    StringBuilder sb = new StringBuilder();
                    StringBuilder sbBody = new StringBuilder();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        sb.Append(Util.TrimNull(dr["productsysno"]) + ",");
                        sbBody.Append(Util.TrimNull(dr["username"]) + " " + "<a target='_blank' href='http://www.baby1one.com.cn/Items/ItemDetail.aspx?type=1&ItemID=" + Util.TrimNull(dr["productsysno"]) + "'>" + Util.TrimNull(dr["productname"]) + "</a>" + " " + Util.TrimDecimalNull(dr["currentprice"]).ToString(AppConst.DecimalFormat) + " " + Util.TrimDecimalNull(dr["newprice"]).ToString(AppConst.DecimalFormat) + "<br>");
                    }
                    string productsysnoList = sb.ToString().Substring(0, sb.Length - 1);
                    string sql1 = "update product_price set currentprice = FLOOR(currentprice*0.99) where productsysno in(" + productsysnoList + ")";
                    int result = SqlHelper.ExecuteNonQuery(sql1);

                    if (result > 0)
                    {
                        EmailInfo oEmail = new EmailInfo();
                        oEmail.MailAddress = "";
                        oEmail.MailSubject = "系统于 " + DateTime.Now.ToString() + " 自动更新了 " + result.ToString() + " 个商品的价格（二手在线商品每天自动降价1%）";
                        oEmail.MailBody = sbBody.ToString();
                        oEmail.Status = (int)AppEnum.YNStatus.No;
                        EmailManager.GetInstance().InsertEmail(oEmail);
                    }

                    string sql2 = @"update product set warranty = '超低价二手商品，无保修' from product_price where product.sysno=product_price.productsysno 
                                    and currentprice < unitcost*0.5 and product.sysno in(" + productsysnoList + ")";
                    SqlHelper.ExecuteNonQuery(sql2);
                }
                scope.Complete();
            }
        }

        public void UpdateVirtualArriveTime(int c3sysno, int VirtualArriveTime)
        {
            string sql = @"update product set VirtualArriveTime=" + VirtualArriveTime + "where c3sysno=" + c3sysno;
            SqlHelper.ExecuteDataSet(sql);
        }
        public int GetProductSysNoByID(string productid)
        {
            string sql = @"select sysno from product where productid=" + productid;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return AppConst.IntNull;
            return Util.TrimIntNull(ds.Tables[0].Rows[0]);
        }

        /// <summary>
        /// 更新商品InventoryCycleTime，DMSWeight
        /// </summary>
        /// <param name="oInfo"></param>
        /// <param name="isAll"></param>
        /// <returns></returns>
        public int UpdateProductDMSBasicByC3(Category3Info oInfo, bool isAll)
        {
            string sql = "update product set InventoryCycleTime='" + oInfo.C3InventoryCycleTime + "',DMSWeight='" + oInfo.C3DMSWeight + "' where c3sysno=" + oInfo.SysNo;
            if (!isAll)
            {
                sql += " and (InventoryCycleTime is null or len(InventoryCycleTime) = 0 or DMSWeight is null or len(DMSWeight) = 0)";
            }
            return SqlHelper.ExecuteNonQuery(sql);
        }



        /// <summary>
        /// 调整小类中商品的显示顺序
        /// </summary>
        /// <param name="oParam"></param>
        /// 

        public void MoveTop(ProductBasicInfo oParam)
        {
            if (oParam.OrderNum == 1)
            {
                throw new BizException("it's the top one already");
            }
            SortedList sl = GetC3Product(oParam.C3SysNo);
            if (sl == null)
            {
                throw new BizException("no attribute for this third category");
            }

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                ProductBasicDac o = new ProductBasicDac();

                foreach (ProductBasicInfo item in sl.Values)
                {
                    if (oParam.OrderNum == AppConst.IntNull || oParam.OrderNum <= 0 || item.OrderNum < oParam.OrderNum)
                    {
                        if (item.OrderNum != AppConst.IntNull && item.OrderNum > 0)
                        {
                            item.OrderNum = item.OrderNum + 1;
                            o.SetOrderNum(item);
                        }
                    }
                }
                oParam.OrderNum = 1;
                o.SetOrderNum(oParam);

                scope.Complete();
            }
        }
        public void MoveUp(ProductBasicInfo oParam)
        {
            if (oParam.OrderNum == 1)
            {
                throw new BizException("it's the first one, can't be moved up");
            }
            SortedList sl = GetC3Product(oParam.C3SysNo);
            if (sl == null)
            {
                throw new BizException("no attributes");
            }

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                ProductBasicDac o = new ProductBasicDac();
                foreach (ProductBasicInfo item in sl.Values)
                {
                    if (item.OrderNum == oParam.OrderNum - 1)
                    {
                        item.OrderNum += 1;
                        o.SetOrderNum(item);
                    }
                }
                oParam.OrderNum -= 1;
                o.SetOrderNum(oParam);

                scope.Complete();
            }
        }
        public void MoveDown(ProductBasicInfo oParam)
        {
            SortedList sl = GetC3Product(oParam.C3SysNo);
            if (sl == null)
            {
                throw new BizException("no attributes");
            }

            if (oParam.OrderNum == sl.Count)
            {
                throw new BizException("it's the last one, can't be moved down");
            }

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                ProductBasicDac o = new ProductBasicDac();

                foreach (ProductBasicInfo item in sl.Values)
                {
                    if (item.OrderNum == oParam.OrderNum + 1)
                    {
                        item.OrderNum -= 1;
                        o.SetOrderNum(item);
                    }
                }
                oParam.OrderNum += 1;
                o.SetOrderNum(oParam);

                scope.Complete();
            }
        }
        public void MoveBottom(ProductBasicInfo oParam)
        {
            SortedList sl = GetC3Product(oParam.C3SysNo);
            if (sl == null)
            {
                throw new BizException("no attributes");
            }

            if (oParam.OrderNum == sl.Count)
            {
                throw new BizException("it's the last one, can't be moved down");
            }

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                ProductBasicDac o = new ProductBasicDac();

                foreach (ProductBasicInfo item in sl.Values)
                {
                    if (item.OrderNum > oParam.OrderNum)
                    {
                        item.OrderNum = item.OrderNum - 1;
                        o.SetOrderNum(item);
                    }
                }
                oParam.OrderNum = sl.Count;
                o.SetOrderNum(oParam);

                scope.Complete();
            }
        }


        public SortedList GetC3Product(int paramC3SysNo)
        {
            Hashtable ht = new Hashtable();
            ht.Add("c3sysno", paramC3SysNo);
            DataSet ds = OnlineListManager.GetInstance().GetC3ProductShowList(ht);
            if (!Util.HasMoreRow(ds))
                return null;
            SortedList sl = new SortedList(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ProductBasicInfo item = new ProductBasicInfo();
                mapBasic(item, dr, true);
                sl.Add(item.SysNo, item);
            }
            return sl;
        }

        public void CompetitorPriceTotxtFile(Hashtable paramHash)
        {
            //            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=CompetitorPrice.txt");
            //            HttpContext.Current.Response.Charset = "GB2312";
            //            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            //            HttpContext.Current.Response.ContentType = "application/vnd.ms-txt";

            //            System.IO.StringWriter tw = new System.IO.StringWriter();

            //            string sql = @"SELECT productmap.pid as productsysno, pricelist.price as competitorprice
            //                         FROM pricelist 
            //                         INNER JOIN productmap ON pricelist.id = productmap.itemid
            //                         WHERE pricelist.storeid = 3 @DateFrom @DateTo";

            //            if (paramHash.Contains("DateFrom"))
            //            {
            //                sql = sql.Replace("@DateFrom", "and pricelist.extracttime>=" + Util.ToSqlString(paramHash["DateFrom"].ToString()));
            //            }
            //            else
            //            {
            //                sql = sql.Replace("@DateFrom", "");
            //            }
            //            if (paramHash.Contains("DateTo"))
            //            {
            //                sql = sql.Replace("@DateTo", "and pricelist.extracttime<=" + Util.ToSqlEndDate(paramHash["DateTo"].ToString()));
            //            }
            //            else
            //            {
            //                sql = sql.Replace("@DateTo", "");
            //            }

            //            DataSet ds = SqlHelper.ExecuteDataSetPWDB(sql);

            //            string sLine = "";
            //            int i = 1;
            //            foreach (DataRow dr in ds.Tables[0].Rows)
            //            {
            //                sLine = Util.TrimIntNull(dr["productsysno"]) + "," + Util.TrimNull(dr["competitorprice"]) ;
            //                tw.WriteLine(sLine);
            //                i++;
            //            }

            //            tw.Flush();
            //            tw.Close();
            //            HttpContext.Current.Response.Write(tw.ToString()); 
            //            HttpContext.Current.Response.End();
        }

        public void CompetitorPriceToDB(ArrayList al)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                for (int i = 0; i < al.Count; i++)
                {
                    ProductPriceCompetitorInfo oInfo = (ProductPriceCompetitorInfo)al[i];
                    string sql = "select * from Product_Price_Competitor where productsysno=" + oInfo.ProductSysNo;
                    DataSet ds = SqlHelper.ExecuteDataSet(sql);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string updatesql = "update Product_Price_Competitor set CompetitorPrice1=" + oInfo.CompetitorPrice1 + ",ImportTime1=" + Util.ToSqlString(oInfo.ImportTime1.ToString()) + " where productsysno =" + oInfo.ProductSysNo;
                        SqlHelper.ExecuteDataSet(updatesql);
                    }
                    else
                    {
                        new ProductPriceCompetitorDac().Insert(oInfo);
                    }
                }
                scope.Complete();
            }
        }


        private void mapProductVendor(ProductVendorInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.ProductSysNo = Util.TrimIntNull(tempdr["ProductSysNo"]);
            oParam.VendorSysNo = Util.TrimIntNull(tempdr["VendorSysNo"]);
            oParam.PurchasePrice = Util.TrimDecimalNull(tempdr["PurchasePrice"]);
            oParam.IsDefault = Util.TrimIntNull(tempdr["IsDefault"]);
            oParam.UpdateUserSysNo = Util.TrimIntNull(tempdr["UpdateUserSysNo"]);
            oParam.UpdateTime = Util.TrimDateNull(tempdr["UpdateTime"]);
        }

        public int InsertProductVendor(ProductVendorInfo oParam)
        {
            return new ProductVendorDac().Insert(oParam);
        }

        public int UpdateProductVendor(ProductVendorInfo oParam)
        {
            return new ProductVendorDac().Update(oParam);
        }


        //获取某主商品的所有子商品,含尺码信息
        public DataSet GetSubProductList(int MasterProductSysno)
        {
            string sql = "select a.sysno,a.productid,a.productname,a.productname,b.size2name as sizename from product as a,size2 as b where a.masterproductsysno=" + MasterProductSysno.ToString() + " and a.product2ndtype=" + ((int)AppEnum.Product2ndType.Child).ToString() + " and a.productsize=b.sysno order by a.productsize,a.sysno";

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;

            return ds;
        }

        //获取所有子商品信息，如果主商品则获取子商品信息，如果子商品则获取同系列子商品信息
        public DataSet GetChildProductSysNoList(int masterProductSysNo, int productSysNo, int product2ndType)
        {
            string sql = "";
            if (product2ndType == (int)AppEnum.Product2ndType.Master)
                sql = "select sysno,productid from product where masterproductsysno=" + masterProductSysNo.ToString() + " and product2ndtype=" + ((int)AppEnum.Product2ndType.Child).ToString();
            else
                sql = "select sysno,productid from product where (masterproductsysno=" + masterProductSysNo.ToString() + " and product2ndtype=" + ((int)AppEnum.Product2ndType.Child).ToString() + " and sysno<>" + productSysNo.ToString() + ") or (sysno=" + masterProductSysNo.ToString() + ")";

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;

            return ds;
        }

        //判断某尺码是否已存在
        public bool HasSize(int MasterProductSysNo, int SizeSysNo)
        {
            string sql = "select sysno from product where (productsize=" + SizeSysNo.ToString() + ") and (SysNo=" + MasterProductSysNo.ToString() + " or (product2ndtype=" + ((int)AppEnum.Product2ndType.Child).ToString() + " and masterproductsysno=" + MasterProductSysNo.ToString() + "))";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return false;
            else
                return true;
        }

        //批量添加其他尺码商品时，复制主商品信息
        public void ProductCloneSize(int MasterProductSysno, int[] arrSizeSysNo, int CreateUser)
        {
            int SizeSysNo = AppConst.IntNull;
            string sizeName = "";
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                for (int i = 0; i < arrSizeSysNo.Length; i++)
                {
                    SizeSysNo = arrSizeSysNo[i];
                    //复制基本信息
                    ProductBasicInfo TBasic = ProductManager.GetInstance().LoadBasic(MasterProductSysno);
                    if (TBasic.SysNo == AppConst.IntNull)
                        throw new BizException("Source Product does not exist");

                    sizeName = GetSizeName(SizeSysNo);

                    if (HasSize(MasterProductSysno, SizeSysNo))
                        throw new BizException("对不起，尺码“" + sizeName + "”已经存在，不能重复添加！");

                    TBasic.ProductID = GenerateNewProductID(TBasic.C3SysNo);
                    TBasic.Product2ndType = (int)AppEnum.Product2ndType.Child;
                    TBasic.MasterProductSysNo = MasterProductSysno;
                    TBasic.ProductSize = SizeSysNo;
                    TBasic.CreateUserSysNo = CreateUser;

                    ProductManager.GetInstance().InsertBasic(TBasic);

                    //复制价格信息
                    ProductPriceInfo TPrice = ProductManager.GetInstance().LoadPrice(MasterProductSysno);
                    if (TPrice.ProductSysNo != AppConst.IntNull)
                    {
                        TPrice.ProductSysNo = TBasic.SysNo;
                        ProductManager.GetInstance().InsertPrice(TPrice);
                    }

                    //状态不复制，插入新记录
                    //ProductStatusInfo TStatus = new ProductStatusInfo();
                    ProductStatusInfo TStatus = ProductManager.GetInstance().LoadStatus(MasterProductSysno);
                    TStatus.ProductSysNo = TBasic.SysNo;
                    //TStatus.AllowStatus = (int)AppEnum.ProductStatusInfo.Valid;
                    //TStatus.InfoStatus = (int)AppEnum.ProductStatusInfo.Valid;
                    //TStatus.PicStatus = (int)AppEnum.ProductStatusInfo.Valid;
                    //TStatus.PriceStatus = (int)AppEnum.ProductStatusInfo.Valid;
                    //TStatus.WarrantyStatus = (int)AppEnum.ProductStatusInfo.Valid;
                    //TStatus.WeightStatus = (int)AppEnum.ProductStatusInfo.Valid;
                    //TStatus.RMAStatus = (int)AppEnum.ProductStatusInfo.Show;
                    ProductManager.GetInstance().InsertStatus(TStatus);

                    //复制商品属性  
                    int TargetSysno = TBasic.SysNo;
                    ProductManager.GetInstance().CloneAttribute2(TargetSysno, MasterProductSysno);

                    //初始化目标商品库存
                    InventoryManager.GetInstance().InitInventory(TBasic.SysNo);

                    //复制安全库存 
                    //InventoryStockInfo TStock = InventoryManager.GetInstance().LoadInventoryStock2(MasterProductSysno);
                    //InventoryManager.GetInstance().SetSafeQty(TargetSysno, TStock.SafeQty, TStock.StockSysNo);

                    SetProductSizeWhenClone(TargetSysno, sizeName);
                }

                scope.Complete();
            }
        }
        public DataSet GetUnSetProductSize(int productSysNo)
        {
            string sql = @" select sysno,size2name from size2 where size1sysno = (select size1sysno from size2 where sysno = (select productsize from product where sysno=" + productSysNo.ToString() + ")) and (sysno not in (select productsize from product where (product2ndtype=" + ((int)AppEnum.Product2ndType.Child).ToString() + " and masterproductsysno=" + productSysNo.ToString() + ") or sysno=" + productSysNo.ToString() + "))";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;

            return ds;
        }
        public string GetSizeName(int SizeSysNo)
        {
            string strName = "";
            string sql = "select size2name from size2 where sysno=" + SizeSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                strName = ds.Tables[0].Rows[0]["size2name"].ToString();
            }
            return strName;
        }
        //当批量添加子商品时，同步尺码属性
        private void SetProductSizeWhenClone(int ProductSysNo, string SizeName)
        {
            int attribute2Sysno = 0;
            int productAttribute2SysNo = 0;
            string sql = @"select a.*,b.sysno as attribute2sysno, b.attribute2name from product_attribute2 as a 
                           left join category_attribute2 as b on b.sysno=a.attribute2sysno 

                           where a.productsysno=@productsysno and b.attribute2name='尺码'";
            sql = sql.Replace("@productsysno", ProductSysNo.ToString());
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                attribute2Sysno = Convert.ToInt32(ds.Tables[0].Rows[0]["attribute2sysno"].ToString());
                productAttribute2SysNo = Convert.ToInt32(ds.Tables[0].Rows[0]["sysno"].ToString());

                if (ds.Tables[0].Rows[0]["attribute2optionsysno"].ToString().Trim() != "0")
                {
                    //如果属性是选项，先找寻选项中是否有对于尺码
                    sql = "select * from category_attribute2_option where attribute2sysno=" + attribute2Sysno.ToString() + " and attribute2optionname='" + SizeName + "'";
                    DataSet ds1 = SqlHelper.ExecuteDataSet(sql);
                    if (Util.HasMoreRow(ds1))
                    {
                        sql = "update product_attribute2 set attribute2optionsysno=" + ds1.Tables[0].Rows[0]["sysno"] + " where sysno=" + productAttribute2SysNo;
                        SqlHelper.ExecuteNonQuery(sql);
                    }
                }
                else
                {
                    //如果属性是文本，则直接输入尺码内容
                    sql = "update product_attribute2 set Attribute2Value='" + SizeName + "' where sysno=" + productAttribute2SysNo;
                    SqlHelper.ExecuteNonQuery(sql);
                }

            }
        }


    }
}