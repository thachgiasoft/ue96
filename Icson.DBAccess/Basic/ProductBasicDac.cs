using System;
using System.Data;
using System.Data.SqlClient;
using Icson.Objects.Basic;
using Icson.Utils;

namespace Icson.DBAccess.Basic
{
    /// <summary>
    /// Summary description for ProductBasicDac.
    /// </summary>
    public class ProductBasicDac
    {

        public ProductBasicDac()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public int Insert(ProductBasicInfo oParam)
        {
            string sql = @"INSERT INTO product
                            (
                            SysNo,ProductID, ProductMode, ProductName, ProductType,
                            ProductDesc, ProductDescLong, Performance, Warranty, 
                            PackageList, Weight, C1SysNo, C2SysNo, C3SysNo, ManufacturerSysNo, 
                            ProductLink, MultiPicNum, PMUserSysNo, PPMUserSysNo, 
                            CreateUserSysNo, Attention, Note, BarCode, Status, IsLarge,
							LastVendorSysNo,PromotionWord,BriefName,AssessmentTitle,AssessmentLink,VirtualArriveTime,
                            InventoryCycleTime,DMSWeight, DMS, OPL,OrderNum,APMUserSysNo,
                            DefaultVendorSysNo, DefaultPurchasePrice, Product2ndType, MasterProductSysNo, 
                            ProductColor, ProductSize, ProducingArea, PackQuantity,
                            MinOrderQuantity, IsStoreFrontSale, SaleUnit,StorageDay
                            )
                            VALUES (
                            @SysNo,@ProductID, @ProductMode, @ProductName, @ProductType,
                            @ProductDesc, @ProductDescLong, @Performance, @Warranty, 
                            @PackageList, @Weight,@C1SysNo, @C2SysNo, @C3SysNo, @ManufacturerSysNo, 
                            @ProductLink, @MultiPicNum, @PMUserSysNo, @PPMUserSysNo, 
                            @CreateUserSysNo, @Attention, @Note, @BarCode, @Status, @IsLarge,
							@LastVendorSysNo,@PromotionWord,@BriefName,@AssessmentTitle,@AssessmentLink,@VirtualArriveTime,
                            @InventoryCycleTime, @DMSWeight, @DMS, @OPL,@OrderNum,@APMUserSysNo, 
                            @DefaultVendorSysNo, @DefaultPurchasePrice, @Product2ndType, @MasterProductSysNo, 
                            @ProductColor, @ProductSize, @ProducingArea, @PackQuantity,
                            @MinOrderQuantity, @IsStoreFrontSale, @SaleUnit,@StorageDay
                            )";

            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramProductID = new SqlParameter("@ProductID", SqlDbType.NVarChar, 20);
            SqlParameter paramProductMode = new SqlParameter("@ProductMode", SqlDbType.NVarChar, 100);
            SqlParameter paramProductName = new SqlParameter("@ProductName", SqlDbType.NVarChar, 300);
            SqlParameter paramProductType = new SqlParameter("@ProductType", SqlDbType.Int, 4);
            SqlParameter paramProductDesc = new SqlParameter("@ProductDesc", SqlDbType.NVarChar, 2000);
            SqlParameter paramProductDescLong = new SqlParameter("@ProductDescLong", SqlDbType.NText);
            SqlParameter paramPerformance = new SqlParameter("@Performance", SqlDbType.NText);
            SqlParameter paramWarranty = new SqlParameter("@Warranty", SqlDbType.NVarChar, 200);
            SqlParameter paramPackageList = new SqlParameter("@PackageList", SqlDbType.NVarChar, 500);
            SqlParameter paramWeight = new SqlParameter("@Weight", SqlDbType.Int, 4);

            SqlParameter paramC1SysNo = new SqlParameter("@C1SysNo", SqlDbType.Int, 4);
            SqlParameter paramC2SysNo = new SqlParameter("@C2SysNo", SqlDbType.Int, 4);
            SqlParameter paramC3SysNo = new SqlParameter("@C3SysNo", SqlDbType.Int, 4);

            SqlParameter paramManufacturerSysNo = new SqlParameter("@ManufacturerSysNo", SqlDbType.Int, 4);
            SqlParameter paramProductLink = new SqlParameter("@ProductLink", SqlDbType.NVarChar, 200);
            SqlParameter paramMultiPicNum = new SqlParameter("@MultiPicNum", SqlDbType.Int, 4);
            SqlParameter paramPMUserSysNo = new SqlParameter("@PMUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramPPMUserSysNo = new SqlParameter("@PPMUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramAttention = new SqlParameter("@Attention", SqlDbType.NVarChar, 500);
            SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.NVarChar, 500);
            SqlParameter paramBarCode = new SqlParameter("@BarCode", SqlDbType.NVarChar, 30);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramIsLarge = new SqlParameter("@IsLarge", SqlDbType.Int, 4);
            SqlParameter paramLastVendorSysNo = new SqlParameter("@LastVendorSysNo", SqlDbType.Int, 4);

            SqlParameter paramPromotionWord = new SqlParameter("@PromotionWord", SqlDbType.NVarChar, 30);
            SqlParameter paramBriefName = new SqlParameter("@BriefName", SqlDbType.NVarChar, 300);
            SqlParameter paramAssessmentTitle = new SqlParameter("@AssessmentTitle", SqlDbType.NVarChar, 200);
            SqlParameter paramAssessmentLink = new SqlParameter("@AssessmentLink", SqlDbType.NVarChar, 200);
            SqlParameter paramVirtualArriveTime = new SqlParameter("@VirtualArriveTime", SqlDbType.Int, 4);

            SqlParameter paramInventoryCycleTime = new SqlParameter("@InventoryCycleTime", SqlDbType.Int, 4);
            SqlParameter paramDMSWeight = new SqlParameter("@DMSWeight", SqlDbType.Decimal, 2);
            SqlParameter paramDMS = new SqlParameter("@DMS", SqlDbType.Decimal, 2);
            SqlParameter paramOPL = new SqlParameter("@OPL", SqlDbType.Int, 4);
            SqlParameter paramOrderNum = new SqlParameter("@OrderNum", SqlDbType.Int, 4);
            SqlParameter paramAPMUserSysNo = new SqlParameter("@APMUserSysNo", SqlDbType.Int, 4);

            SqlParameter paramDefaultVendorSysNo = new SqlParameter("@DefaultVendorSysNo", SqlDbType.Int, 4);
            SqlParameter paramDefaultPurchasePrice = new SqlParameter("@DefaultPurchasePrice", SqlDbType.Decimal, 9);
            SqlParameter paramProduct2ndType = new SqlParameter("@Product2ndType", SqlDbType.Int, 4);
            SqlParameter paramMasterProductSysNo = new SqlParameter("@MasterProductSysNo", SqlDbType.Int, 4);
            SqlParameter paramProductColor = new SqlParameter("@ProductColor", SqlDbType.Int, 4);
            SqlParameter paramProductSize = new SqlParameter("@ProductSize", SqlDbType.Int, 4);
            SqlParameter paramProducingArea = new SqlParameter("@ProducingArea", SqlDbType.NVarChar, 50);
            SqlParameter paramPackQuantity = new SqlParameter("@PackQuantity", SqlDbType.Int, 4);
            SqlParameter paramMinOrderQuantity = new SqlParameter("@MinOrderQuantity", SqlDbType.Int, 4);
            SqlParameter paramIsStoreFrontSale = new SqlParameter("@IsStoreFrontSale", SqlDbType.Int, 4);
            SqlParameter paramSaleUnit = new SqlParameter("@SaleUnit", SqlDbType.NVarChar, 50);
            SqlParameter paramStorageDay = new SqlParameter("@StorageDay", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.ProductID != AppConst.StringNull)
                paramProductID.Value = oParam.ProductID;
            else
                paramProductID.Value = System.DBNull.Value;
            if (oParam.ProductMode != AppConst.StringNull)
                paramProductMode.Value = oParam.ProductMode;
            else
                paramProductMode.Value = System.DBNull.Value;
            if (oParam.ProductName != AppConst.StringNull)
                paramProductName.Value = oParam.ProductName;
            else
                paramProductName.Value = "Lack of name";
            if (oParam.ProductType != AppConst.IntNull)
                paramProductType.Value = oParam.ProductType;
            else
                paramProductType.Value = System.DBNull.Value;
            if (oParam.ProductDesc != AppConst.StringNull)
                paramProductDesc.Value = oParam.ProductDesc;
            else
                paramProductDesc.Value = System.DBNull.Value;
            if (oParam.ProductDescLong != AppConst.StringNull)
                paramProductDescLong.Value = oParam.ProductDescLong;
            else
                paramProductDescLong.Value = System.DBNull.Value;
            if (oParam.Performance != AppConst.StringNull)
                paramPerformance.Value = oParam.Performance;
            else
                paramPerformance.Value = System.DBNull.Value;
            if (oParam.Warranty != AppConst.StringNull)
                paramWarranty.Value = oParam.Warranty;
            else
                paramWarranty.Value = "无保修";
            if (oParam.PackageList != AppConst.StringNull)
                paramPackageList.Value = oParam.PackageList;
            else
                paramPackageList.Value = System.DBNull.Value;
            if (oParam.Weight != AppConst.IntNull)
                paramWeight.Value = oParam.Weight;
            else
                paramWeight.Value = System.DBNull.Value;
            if (oParam.C1SysNo != AppConst.IntNull)
            {
                paramC1SysNo.Value = oParam.C1SysNo;
            }
            else
            {
                paramC1SysNo.Value = System.DBNull.Value;
            }
            if (oParam.C2SysNo != AppConst.IntNull)
            {
                paramC2SysNo.Value = oParam.C2SysNo;
            }
            else
            {
                paramC2SysNo.Value = System.DBNull.Value;
            }
            if (oParam.C3SysNo != AppConst.IntNull)
            {
                paramC3SysNo.Value = oParam.C3SysNo;
            }
            else
            {
                paramC3SysNo.Value = System.DBNull.Value;
            }
            if (oParam.ManufacturerSysNo != AppConst.IntNull)
                paramManufacturerSysNo.Value = oParam.ManufacturerSysNo;
            else
                paramManufacturerSysNo.Value = System.DBNull.Value;
            if (oParam.ProductLink != AppConst.StringNull)
                paramProductLink.Value = oParam.ProductLink;
            else
                paramProductLink.Value = System.DBNull.Value;
            if (oParam.MultiPicNum != AppConst.IntNull)
                paramMultiPicNum.Value = oParam.MultiPicNum;
            else
                paramMultiPicNum.Value = System.DBNull.Value;
            if (oParam.PMUserSysNo != AppConst.IntNull)
                paramPMUserSysNo.Value = oParam.PMUserSysNo;
            else
                paramPMUserSysNo.Value = System.DBNull.Value;
            if (oParam.PPMUserSysNo != AppConst.IntNull)
                paramPPMUserSysNo.Value = oParam.PPMUserSysNo;
            else
                paramPPMUserSysNo.Value = System.DBNull.Value;
            if (oParam.CreateUserSysNo != AppConst.IntNull)
                paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
            else
                paramCreateUserSysNo.Value = System.DBNull.Value;
            if (oParam.Attention != AppConst.StringNull)
                paramAttention.Value = oParam.Attention;
            else
                paramAttention.Value = System.DBNull.Value;
            if (oParam.Note != AppConst.StringNull)
                paramNote.Value = oParam.Note;
            else
                paramNote.Value = System.DBNull.Value;
            if (oParam.BarCode != AppConst.StringNull)
                paramBarCode.Value = oParam.BarCode;
            else
                paramBarCode.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;
            if (oParam.IsLarge != AppConst.IntNull)
                paramIsLarge.Value = oParam.IsLarge;
            else
                paramIsLarge.Value = System.DBNull.Value;
            if (oParam.LastVendorSysNo != AppConst.IntNull)
                paramLastVendorSysNo.Value = oParam.LastVendorSysNo;
            else
                paramLastVendorSysNo.Value = System.DBNull.Value;

            if (oParam.PromotionWord != AppConst.StringNull)
                paramPromotionWord.Value = oParam.PromotionWord;
            else
                paramPromotionWord.Value = System.DBNull.Value;
            if (oParam.BriefName != AppConst.StringNull)
                paramBriefName.Value = oParam.BriefName;
            else
                paramBriefName.Value = System.DBNull.Value;

            if (oParam.AssessmentTitle != AppConst.StringNull)
                paramAssessmentTitle.Value = oParam.AssessmentTitle;
            else
                paramAssessmentTitle.Value = System.DBNull.Value;
            if (oParam.AssessmentLink != AppConst.StringNull)
                paramAssessmentLink.Value = oParam.AssessmentLink;
            else
                paramAssessmentLink.Value = System.DBNull.Value;
            if (oParam.VirtualArriveTime != AppConst.IntNull)
                paramVirtualArriveTime.Value = oParam.VirtualArriveTime;
            else
                paramVirtualArriveTime.Value = System.DBNull.Value;

            if (oParam.InventoryCycleTime != AppConst.IntNull)
                paramInventoryCycleTime.Value = oParam.InventoryCycleTime;
            else
                paramInventoryCycleTime.Value = System.DBNull.Value;
            if (oParam.DMSWeight != AppConst.DecimalNull)
                paramDMSWeight.Value = oParam.DMSWeight;
            else
                paramDMSWeight.Value = System.DBNull.Value;
            if (oParam.DMS != AppConst.DecimalNull)
                paramDMS.Value = oParam.DMS;
            else
                paramDMS.Value = System.DBNull.Value;
            if (oParam.OPL != AppConst.IntNull)
                paramOPL.Value = oParam.OPL;
            else
                paramOPL.Value = System.DBNull.Value;
            if (oParam.OrderNum != AppConst.IntNull)
                paramOrderNum.Value = oParam.OrderNum;
            else
                paramOrderNum.Value = System.DBNull.Value;
            if (oParam.APMUserSysNo != AppConst.IntNull)
                paramAPMUserSysNo.Value = oParam.APMUserSysNo;
            else
                paramAPMUserSysNo.Value = System.DBNull.Value;

            if (oParam.DefaultVendorSysNo != AppConst.IntNull)
                paramDefaultVendorSysNo.Value = oParam.DefaultVendorSysNo;
            else
                paramDefaultVendorSysNo.Value = System.DBNull.Value;
            if (oParam.DefaultPurchasePrice != AppConst.DecimalNull)
                paramDefaultPurchasePrice.Value = oParam.DefaultPurchasePrice;
            else
                paramDefaultPurchasePrice.Value = System.DBNull.Value;
            if (oParam.Product2ndType != AppConst.IntNull)
                paramProduct2ndType.Value = oParam.Product2ndType;
            else
                paramProduct2ndType.Value = System.DBNull.Value;
            if (oParam.MasterProductSysNo != AppConst.IntNull)
                paramMasterProductSysNo.Value = oParam.MasterProductSysNo;
            else
                paramMasterProductSysNo.Value = System.DBNull.Value;
            if (oParam.ProductColor != AppConst.IntNull)
                paramProductColor.Value = oParam.ProductColor;
            else
                paramProductColor.Value = System.DBNull.Value;
            if (oParam.ProductSize != AppConst.IntNull)
                paramProductSize.Value = oParam.ProductSize;
            else
                paramProductSize.Value = System.DBNull.Value;
            if (oParam.ProducingArea != AppConst.StringNull)
                paramProducingArea.Value = oParam.ProducingArea;
            else
                paramProducingArea.Value = System.DBNull.Value;
            if (oParam.PackQuantity != AppConst.IntNull)
                paramPackQuantity.Value = oParam.PackQuantity;
            else
                paramPackQuantity.Value = System.DBNull.Value;
            if (oParam.MinOrderQuantity != AppConst.IntNull)
                paramMinOrderQuantity.Value = oParam.MinOrderQuantity;
            else
                paramMinOrderQuantity.Value = System.DBNull.Value;
            if (oParam.IsStoreFrontSale != AppConst.IntNull)
                paramIsStoreFrontSale.Value = oParam.IsStoreFrontSale;
            else
                paramIsStoreFrontSale.Value = System.DBNull.Value;
            if (oParam.SaleUnit != AppConst.StringNull)
                paramSaleUnit.Value = oParam.SaleUnit;
            else
                paramSaleUnit.Value = System.DBNull.Value;
            if (oParam.StorageDay != AppConst.IntNull)
                paramStorageDay.Value = oParam.StorageDay;
            else
                paramStorageDay.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramProductID);
            cmd.Parameters.Add(paramProductMode);
            cmd.Parameters.Add(paramProductName);
            cmd.Parameters.Add(paramProductType);
            cmd.Parameters.Add(paramProductDesc);
            cmd.Parameters.Add(paramProductDescLong);
            cmd.Parameters.Add(paramPerformance);
            cmd.Parameters.Add(paramWarranty);
            cmd.Parameters.Add(paramPackageList);
            cmd.Parameters.Add(paramWeight);
            cmd.Parameters.Add(paramC1SysNo);
            cmd.Parameters.Add(paramC2SysNo);
            cmd.Parameters.Add(paramC3SysNo);
            cmd.Parameters.Add(paramManufacturerSysNo);
            cmd.Parameters.Add(paramProductLink);
            cmd.Parameters.Add(paramMultiPicNum);
            cmd.Parameters.Add(paramPMUserSysNo);
            cmd.Parameters.Add(paramPPMUserSysNo);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramAttention);
            cmd.Parameters.Add(paramNote);
            cmd.Parameters.Add(paramBarCode);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramIsLarge);
            cmd.Parameters.Add(paramLastVendorSysNo);

            cmd.Parameters.Add(paramPromotionWord);
            cmd.Parameters.Add(paramBriefName);
            cmd.Parameters.Add(paramAssessmentTitle);
            cmd.Parameters.Add(paramAssessmentLink);
            cmd.Parameters.Add(paramVirtualArriveTime);

            cmd.Parameters.Add(paramInventoryCycleTime);
            cmd.Parameters.Add(paramDMSWeight);
            cmd.Parameters.Add(paramDMS);
            cmd.Parameters.Add(paramOPL);

            cmd.Parameters.Add(paramOrderNum);
            cmd.Parameters.Add(paramAPMUserSysNo);

            cmd.Parameters.Add(paramDefaultVendorSysNo);
            cmd.Parameters.Add(paramDefaultPurchasePrice);
            cmd.Parameters.Add(paramProduct2ndType);
            cmd.Parameters.Add(paramMasterProductSysNo);
            cmd.Parameters.Add(paramProductColor);
            cmd.Parameters.Add(paramProductSize);
            cmd.Parameters.Add(paramProducingArea);
            cmd.Parameters.Add(paramPackQuantity);
            cmd.Parameters.Add(paramMinOrderQuantity);
            cmd.Parameters.Add(paramIsStoreFrontSale);
            cmd.Parameters.Add(paramSaleUnit);
            cmd.Parameters.Add(paramStorageDay);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
        /// <summary>
        /// 更新商品基本信息(商品状态另外单独更新)
        /// </summary>
        /// <param name="oParam"></param>
        /// <returns></returns>
        public int Update(ProductBasicInfo oParam)
        {
            string sql = @"UPDATE product SET 
                            ProductID=@ProductID, 
                            ProductMode=@ProductMode, ProductName=@ProductName, ProductType=@ProductType,
                            ProductDesc=@ProductDesc, ProductDescLong=@ProductDescLong, 
                            Performance=@Performance, Warranty=@Warranty, 
                            PackageList=@PackageList, Weight=@Weight,C1SysNo=@C1SysNO, 
                            C2SysNo=@C2SysNo, C3SysNo=@C3SysNo, ManufacturerSysNo=@ManufacturerSysNo, 
                            ProductLink=@ProductLink, MultiPicNum=@MultiPicNum, 
                            PMUserSysNo=@PMUserSysNo, PPMUserSysNo=@PPMUserSysNo, 
                            Attention=@Attention, Note=@Note, BarCode=@BarCode, IsLarge=@IsLarge,
							LastVendorSysNo = @LastVendorSysNo, Status=@Status, 
                            PromotionWord=@PromotionWord,BriefName=@BriefName,AssessmentTitle=@AssessmentTitle,
                            AssessmentLink=@AssessmentLink,VirtualArriveTime=@VirtualArriveTime,
                            InventoryCycleTime=@InventoryCycleTime, DMSWeight=@DMSWeight, 
                            DMS=@DMS, OPL=@OPL ,OrderNum=@OrderNum,APMUserSysNo=@APMUserSysNo, 
                            DefaultVendorSysNo=@DefaultVendorSysNo, DefaultPurchasePrice=@DefaultPurchasePrice, 
                            Product2ndType=@Product2ndType, MasterProductSysNo=@MasterProductSysNo, 
                            ProductColor=@ProductColor, ProductSize=@ProductSize,
                            ProducingArea=@ProducingArea, PackQuantity=@PackQuantity,
                            MinOrderQuantity=@MinOrderQuantity, IsStoreFrontSale=@IsStoreFrontSale,
                            SaleUnit=@SaleUnit, StorageDay=@StorageDay, IsCanPurchase=@IsCanPurchase
                            WHERE SysNo = @SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramProductID = new SqlParameter("@ProductID", SqlDbType.NVarChar, 20);
            SqlParameter paramProductMode = new SqlParameter("@ProductMode", SqlDbType.NVarChar, 100);
            SqlParameter paramProductName = new SqlParameter("@ProductName", SqlDbType.NVarChar, 300);
            SqlParameter paramProductType = new SqlParameter("@ProductType", SqlDbType.Int, 4);
            SqlParameter paramProductDesc = new SqlParameter("@ProductDesc", SqlDbType.NVarChar, 2000);
            SqlParameter paramProductDescLong = new SqlParameter("@ProductDescLong", SqlDbType.NText);
            SqlParameter paramPerformance = new SqlParameter("@Performance", SqlDbType.NText);
            SqlParameter paramWarranty = new SqlParameter("@Warranty", SqlDbType.NVarChar, 200);
            SqlParameter paramPackageList = new SqlParameter("@PackageList", SqlDbType.NVarChar, 500);
            SqlParameter paramWeight = new SqlParameter("@Weight", SqlDbType.Int, 4);

            SqlParameter paramC1SysNo = new SqlParameter("@C1SysNo", SqlDbType.Int, 4);
            SqlParameter paramC2SysNo = new SqlParameter("@C2SysNo", SqlDbType.Int, 4);
            SqlParameter paramC3SysNo = new SqlParameter("@C3SysNo", SqlDbType.Int, 4);

            SqlParameter paramManufacturerSysNo = new SqlParameter("@ManufacturerSysNo", SqlDbType.Int, 4);
            SqlParameter paramProductLink = new SqlParameter("@ProductLink", SqlDbType.NVarChar, 200);
            SqlParameter paramMultiPicNum = new SqlParameter("@MultiPicNum", SqlDbType.Int, 4);
            SqlParameter paramPMUserSysNo = new SqlParameter("@PMUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramPPMUserSysNo = new SqlParameter("@PPMUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramAttention = new SqlParameter("@Attention", SqlDbType.NVarChar, 500);
            SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.NVarChar, 500);
            SqlParameter paramBarCode = new SqlParameter("@BarCode", SqlDbType.NVarChar, 30);
            SqlParameter paramIsLarge = new SqlParameter("@IsLarge", SqlDbType.Int, 4);
            SqlParameter paramLastVendorSysNo = new SqlParameter("@LastVendorSysNo", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);

            SqlParameter paramPromotionWord = new SqlParameter("@PromotionWord", SqlDbType.NVarChar, 30);
            SqlParameter paramBriefName = new SqlParameter("@BriefName", SqlDbType.NVarChar, 300);
            SqlParameter paramAssessmentTitle = new SqlParameter("@AssessmentTitle", SqlDbType.NVarChar, 200);
            SqlParameter paramAssessmentLink = new SqlParameter("@AssessmentLink", SqlDbType.NVarChar, 200);
            SqlParameter paramVirtualArriveTime = new SqlParameter("@VirtualArriveTime", SqlDbType.Int, 4);

            SqlParameter paramInventoryCycleTime = new SqlParameter("@InventoryCycleTime", SqlDbType.Int, 4);
            SqlParameter paramDMSWeight = new SqlParameter("@DMSWeight", SqlDbType.Decimal, 2);
            SqlParameter paramDMS = new SqlParameter("@DMS", SqlDbType.Decimal, 2);
            SqlParameter paramOPL = new SqlParameter("@OPL", SqlDbType.Int, 4);

            SqlParameter paramOrderNum = new SqlParameter("@OrderNum", SqlDbType.Int, 4);
            SqlParameter paramAPMUserSysNo = new SqlParameter("@APMUserSysNo", SqlDbType.Int, 4);

            SqlParameter paramDefaultVendorSysNo = new SqlParameter("@DefaultVendorSysNo", SqlDbType.Int, 4);
            SqlParameter paramDefaultPurchasePrice = new SqlParameter("@DefaultPurchasePrice", SqlDbType.Decimal, 9);
            SqlParameter paramProduct2ndType = new SqlParameter("@Product2ndType", SqlDbType.Int, 4);
            SqlParameter paramMasterProductSysNo = new SqlParameter("@MasterProductSysNo", SqlDbType.Int, 4);
            SqlParameter paramProductColor = new SqlParameter("@ProductColor", SqlDbType.Int, 4);
            SqlParameter paramProductSize = new SqlParameter("@ProductSize", SqlDbType.Int, 4);
            SqlParameter paramProducingArea = new SqlParameter("@ProducingArea", SqlDbType.NVarChar, 50);
            SqlParameter paramPackQuantity = new SqlParameter("@PackQuantity", SqlDbType.Int, 4);
            SqlParameter paramMinOrderQuantity = new SqlParameter("@MinOrderQuantity", SqlDbType.Int, 4);
            SqlParameter paramIsStoreFrontSale = new SqlParameter("@IsStoreFrontSale", SqlDbType.Int, 4);
            SqlParameter paramSaleUnit = new SqlParameter("@SaleUnit", SqlDbType.NVarChar, 50);
            SqlParameter paramStorageDay = new SqlParameter("@StorageDay", SqlDbType.Int, 4);
            SqlParameter paramIsCanPurchase = new SqlParameter("@IsCanPurchase", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.ProductID != AppConst.StringNull)
                paramProductID.Value = oParam.ProductID;
            else
                paramProductID.Value = System.DBNull.Value;
            if (oParam.ProductMode != AppConst.StringNull)
                paramProductMode.Value = oParam.ProductMode;
            else
                paramProductMode.Value = System.DBNull.Value;
            if (oParam.ProductName != AppConst.StringNull)
                paramProductName.Value = oParam.ProductName;
            else
                paramProductName.Value = System.DBNull.Value;
            if (oParam.ProductType != AppConst.IntNull)
                paramProductType.Value = oParam.ProductType;
            else
                paramProductType.Value = System.DBNull.Value;
            if (oParam.ProductDesc != AppConst.StringNull)
                paramProductDesc.Value = oParam.ProductDesc;
            else
                paramProductDesc.Value = System.DBNull.Value;
            if (oParam.ProductDescLong != AppConst.StringNull)
                paramProductDescLong.Value = oParam.ProductDescLong;
            else
                paramProductDescLong.Value = System.DBNull.Value;
            if (oParam.Performance != AppConst.StringNull)
                paramPerformance.Value = oParam.Performance;
            else
                paramPerformance.Value = System.DBNull.Value;
            if (oParam.Warranty != AppConst.StringNull)
                paramWarranty.Value = oParam.Warranty;
            else
                paramWarranty.Value = System.DBNull.Value;
            if (oParam.PackageList != AppConst.StringNull)
                paramPackageList.Value = oParam.PackageList;
            else
                paramPackageList.Value = System.DBNull.Value;
            if (oParam.Weight != AppConst.IntNull)
                paramWeight.Value = oParam.Weight;
            else
                paramWeight.Value = System.DBNull.Value;
            if (oParam.C1SysNo != AppConst.IntNull)
            {
                paramC1SysNo.Value = oParam.C1SysNo;
            }
            else
            {
                paramC1SysNo.Value = System.DBNull.Value;
            }
            if (oParam.C2SysNo != AppConst.IntNull)
            {
                paramC2SysNo.Value = oParam.C2SysNo;
            }
            else
            {
                paramC2SysNo.Value = System.DBNull.Value;
            }
            if (oParam.C3SysNo != AppConst.IntNull)
                paramC3SysNo.Value = oParam.C3SysNo;
            else
                paramC3SysNo.Value = System.DBNull.Value;
            if (oParam.ManufacturerSysNo != AppConst.IntNull)
                paramManufacturerSysNo.Value = oParam.ManufacturerSysNo;
            else
                paramManufacturerSysNo.Value = System.DBNull.Value;
            if (oParam.ProductLink != AppConst.StringNull)
                paramProductLink.Value = oParam.ProductLink;
            else
                paramProductLink.Value = System.DBNull.Value;
            if (oParam.MultiPicNum != AppConst.IntNull)
                paramMultiPicNum.Value = oParam.MultiPicNum;
            else
                paramMultiPicNum.Value = System.DBNull.Value;
            if (oParam.PMUserSysNo != AppConst.IntNull)
                paramPMUserSysNo.Value = oParam.PMUserSysNo;
            else
                paramPMUserSysNo.Value = System.DBNull.Value;
            if (oParam.PPMUserSysNo != AppConst.IntNull)
                paramPPMUserSysNo.Value = oParam.PPMUserSysNo;
            else
                paramPPMUserSysNo.Value = System.DBNull.Value;
            if (oParam.Attention != AppConst.StringNull)
                paramAttention.Value = oParam.Attention;
            else
                paramAttention.Value = System.DBNull.Value;
            if (oParam.Note != AppConst.StringNull)
                paramNote.Value = oParam.Note;
            else
                paramNote.Value = System.DBNull.Value;
            if (oParam.BarCode != AppConst.StringNull)
                paramBarCode.Value = oParam.BarCode;
            else
                paramBarCode.Value = System.DBNull.Value;
            if (oParam.IsLarge != AppConst.IntNull)
                paramIsLarge.Value = oParam.IsLarge;
            else
                paramIsLarge.Value = System.DBNull.Value;
            if (oParam.LastVendorSysNo != AppConst.IntNull)
                paramLastVendorSysNo.Value = oParam.LastVendorSysNo;
            else
                paramLastVendorSysNo.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;

            if (oParam.PromotionWord != AppConst.StringNull)
                paramPromotionWord.Value = oParam.PromotionWord;
            else
                paramPromotionWord.Value = System.DBNull.Value;
            if (oParam.BriefName != AppConst.StringNull)
                paramBriefName.Value = oParam.BriefName;
            else
                paramBriefName.Value = System.DBNull.Value;
            if (oParam.AssessmentTitle != AppConst.StringNull)
                paramAssessmentTitle.Value = oParam.AssessmentTitle;
            else
                paramAssessmentTitle.Value = System.DBNull.Value;
            if (oParam.AssessmentLink != AppConst.StringNull)
                paramAssessmentLink.Value = oParam.AssessmentLink;
            else
                paramAssessmentLink.Value = System.DBNull.Value;
            if (oParam.VirtualArriveTime != AppConst.IntNull)
                paramVirtualArriveTime.Value = oParam.VirtualArriveTime;
            else
                paramVirtualArriveTime.Value = System.DBNull.Value;

            if (oParam.InventoryCycleTime != AppConst.IntNull)
                paramInventoryCycleTime.Value = oParam.InventoryCycleTime;
            else
                paramInventoryCycleTime.Value = System.DBNull.Value;
            if (oParam.DMSWeight != AppConst.DecimalNull)
                paramDMSWeight.Value = oParam.DMSWeight;
            else
                paramDMSWeight.Value = System.DBNull.Value;
            if (oParam.DMS != AppConst.DecimalNull)
                paramDMS.Value = oParam.DMS;
            else
                paramDMS.Value = System.DBNull.Value;
            if (oParam.OPL != AppConst.IntNull)
                paramOPL.Value = oParam.OPL;
            else
                paramOPL.Value = System.DBNull.Value;
            if (oParam.OrderNum != AppConst.IntNull)
                paramOrderNum.Value = oParam.OrderNum;
            else
                paramOrderNum.Value = System.DBNull.Value;
            if (oParam.APMUserSysNo != AppConst.IntNull)
                paramAPMUserSysNo.Value = oParam.APMUserSysNo;
            else
                paramAPMUserSysNo.Value = System.DBNull.Value;

            if (oParam.DefaultVendorSysNo != AppConst.IntNull)
                paramDefaultVendorSysNo.Value = oParam.DefaultVendorSysNo;
            else
                paramDefaultVendorSysNo.Value = System.DBNull.Value;
            if (oParam.DefaultPurchasePrice != AppConst.DecimalNull)
                paramDefaultPurchasePrice.Value = oParam.DefaultPurchasePrice;
            else
                paramDefaultPurchasePrice.Value = System.DBNull.Value;
            if (oParam.Product2ndType != AppConst.IntNull)
                paramProduct2ndType.Value = oParam.Product2ndType;
            else
                paramProduct2ndType.Value = System.DBNull.Value;
            if (oParam.MasterProductSysNo != AppConst.IntNull)
                paramMasterProductSysNo.Value = oParam.MasterProductSysNo;
            else
                paramMasterProductSysNo.Value = System.DBNull.Value;
            if (oParam.ProductColor != AppConst.IntNull)
                paramProductColor.Value = oParam.ProductColor;
            else
                paramProductColor.Value = System.DBNull.Value;
            if (oParam.ProductSize != AppConst.IntNull)
                paramProductSize.Value = oParam.ProductSize;
            else
                paramProductSize.Value = System.DBNull.Value;
            if (oParam.ProducingArea != AppConst.StringNull)
                paramProducingArea.Value = oParam.ProducingArea;
            else
                paramProducingArea.Value = System.DBNull.Value;
            if (oParam.PackQuantity != AppConst.IntNull)
                paramPackQuantity.Value = oParam.PackQuantity;
            else
                paramPackQuantity.Value = System.DBNull.Value;
            if (oParam.MinOrderQuantity != AppConst.IntNull)
                paramMinOrderQuantity.Value = oParam.MinOrderQuantity;
            else
                paramMinOrderQuantity.Value = System.DBNull.Value;
            if (oParam.IsStoreFrontSale != AppConst.IntNull)
                paramIsStoreFrontSale.Value = oParam.IsStoreFrontSale;
            else
                paramIsStoreFrontSale.Value = System.DBNull.Value;
            if (oParam.SaleUnit != AppConst.StringNull)
                paramSaleUnit.Value = oParam.SaleUnit;
            else
                paramSaleUnit.Value = System.DBNull.Value;
            if (oParam.StorageDay != AppConst.IntNull)
                paramStorageDay.Value = oParam.StorageDay;
            else
                paramStorageDay.Value = System.DBNull.Value;
            if (oParam.IsCanPurchase !=AppConst.IntNull)
            {
                paramIsCanPurchase.Value=oParam.IsCanPurchase;
            }
            else
            {
                paramIsCanPurchase.Value = System.DBNull.Value;
            }

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramProductID);
            cmd.Parameters.Add(paramProductMode);
            cmd.Parameters.Add(paramProductName);
            cmd.Parameters.Add(paramProductType);
            cmd.Parameters.Add(paramProductDesc);
            cmd.Parameters.Add(paramProductDescLong);
            cmd.Parameters.Add(paramPerformance);
            cmd.Parameters.Add(paramWarranty);
            cmd.Parameters.Add(paramPackageList);
            cmd.Parameters.Add(paramWeight);
            cmd.Parameters.Add(paramC1SysNo);
            cmd.Parameters.Add(paramC2SysNo);
            cmd.Parameters.Add(paramC3SysNo);
            cmd.Parameters.Add(paramManufacturerSysNo);
            cmd.Parameters.Add(paramProductLink);
            cmd.Parameters.Add(paramMultiPicNum);
            cmd.Parameters.Add(paramPMUserSysNo);
            cmd.Parameters.Add(paramPPMUserSysNo);
            cmd.Parameters.Add(paramAttention);
            cmd.Parameters.Add(paramNote);
            cmd.Parameters.Add(paramBarCode);
            cmd.Parameters.Add(paramIsLarge);
            cmd.Parameters.Add(paramLastVendorSysNo);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramPromotionWord);
            cmd.Parameters.Add(paramBriefName);
            cmd.Parameters.Add(paramAssessmentTitle);
            cmd.Parameters.Add(paramAssessmentLink);
            cmd.Parameters.Add(paramVirtualArriveTime);

            cmd.Parameters.Add(paramInventoryCycleTime);
            cmd.Parameters.Add(paramDMSWeight);
            cmd.Parameters.Add(paramDMS);
            cmd.Parameters.Add(paramOPL);

            cmd.Parameters.Add(paramOrderNum);
            cmd.Parameters.Add(paramAPMUserSysNo);

            cmd.Parameters.Add(paramDefaultVendorSysNo);
            cmd.Parameters.Add(paramDefaultPurchasePrice);
            cmd.Parameters.Add(paramProduct2ndType);
            cmd.Parameters.Add(paramMasterProductSysNo);
            cmd.Parameters.Add(paramProductColor);
            cmd.Parameters.Add(paramProductSize);
            cmd.Parameters.Add(paramProducingArea);
            cmd.Parameters.Add(paramPackQuantity);
            cmd.Parameters.Add(paramMinOrderQuantity);
            cmd.Parameters.Add(paramIsStoreFrontSale);
            cmd.Parameters.Add(paramSaleUnit);
            cmd.Parameters.Add(paramStorageDay);

            cmd.Parameters.Add(paramIsCanPurchase);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
        /// <summary>
        /// 商品状态单独更新
        /// </summary>
        /// <param name="ProductSysNo"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public int UpdateStatus(int productSysNo, int productStatus)
        {
            string sql = @" Update Product
							set Status = @Status
							Where SysNo = @SysNo";
            SqlCommand cmd = new SqlCommand(sql);
            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            if (productSysNo != AppConst.IntNull)
                paramSysNo.Value = productSysNo;
            if (productStatus != AppConst.IntNull)
                paramStatus.Value = productStatus;
            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
        public int UpdateLastVendor(int productSysNo, int lastVendorSysNo)
        {
            string sql = "update product set LastVendorSysNo=" + lastVendorSysNo + " where sysno = " + productSysNo;
            SqlCommand cmd = new SqlCommand(sql);
            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int SetOrderNum( ProductBasicInfo oParam)
        {
            string sql = "update product set ordernum = " + oParam.OrderNum + " where sysno = " + oParam.SysNo;
            SqlCommand cmd = new SqlCommand(sql);
            return SqlHelper.ExecuteNonQuery(cmd);
        }
    }
}