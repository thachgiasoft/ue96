using System;
using System.Collections.Generic;
using System.Text;
using Icson.Objects;
using Icson.Utils;
using Icson.Objects.Promotion;
using System.Data;
using System.Data.SqlClient;

namespace Icson.DBAccess.Promotion
{
    public class CouponDac
    {
        public CouponDac()
		{
		}

        public int Insert(CouponInfo oParam)
        {
            string sql = @"INSERT INTO Coupon
                            (
                            SysNo, CouponID, CouponName, CouponCode, 
                            CouponAmt, SaleAmt, CouponType, ValidTimeFrom, 
                            ValidTimeTo, MaxUseDegree, UsedDegree, CreateUserSysNo, 
                            CreateTime, AuditUserSysNo, AuditTime, UsedTime, 
                            BatchNo, Status, CategorySysNoCom, ProductSysNoCom, 
                            ManufactorySysNoCom, UseAreaSysNoCom, UseCustomerSysNo, UseCustomerGradeCom
                            )
                            VALUES (
                            @SysNo, @CouponID, @CouponName, @CouponCode, 
                            @CouponAmt, @SaleAmt, @CouponType, @ValidTimeFrom, 
                            @ValidTimeTo, @MaxUseDegree, @UsedDegree, @CreateUserSysNo, 
                            @CreateTime, @AuditUserSysNo, @AuditTime, @UsedTime, 
                            @BatchNo, @Status, @CategorySysNoCom, @ProductSysNoCom, 
                            @ManufactorySysNoCom, @UseAreaSysNoCom, @UseCustomerSysNo, @UseCustomerGradeCom
                            )";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramCouponID = new SqlParameter("@CouponID", SqlDbType.NVarChar, 20);
            SqlParameter paramCouponName = new SqlParameter("@CouponName", SqlDbType.NVarChar, 50);
            SqlParameter paramCouponCode = new SqlParameter("@CouponCode", SqlDbType.NVarChar, 20);
            SqlParameter paramCouponAmt = new SqlParameter("@CouponAmt", SqlDbType.Decimal, 9);
            SqlParameter paramSaleAmt = new SqlParameter("@SaleAmt", SqlDbType.Decimal, 9);
            SqlParameter paramCouponType = new SqlParameter("@CouponType", SqlDbType.Int, 4);
            SqlParameter paramValidTimeFrom = new SqlParameter("@ValidTimeFrom", SqlDbType.DateTime);
            SqlParameter paramValidTimeTo = new SqlParameter("@ValidTimeTo", SqlDbType.DateTime);
            SqlParameter paramMaxUseDegree = new SqlParameter("@MaxUseDegree", SqlDbType.Int, 4);
            SqlParameter paramUsedDegree = new SqlParameter("@UsedDegree", SqlDbType.Int, 4);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
            SqlParameter paramAuditUserSysNo = new SqlParameter("@AuditUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramAuditTime = new SqlParameter("@AuditTime", SqlDbType.DateTime);
            SqlParameter paramUsedTime = new SqlParameter("@UsedTime", SqlDbType.DateTime);
            SqlParameter paramBatchNo = new SqlParameter("@BatchNo", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramCategorySysNoCom = new SqlParameter("@CategorySysNoCom", SqlDbType.NVarChar, 100);
            SqlParameter paramProductSysNoCom = new SqlParameter("@ProductSysNoCom", SqlDbType.NVarChar, 100);
            SqlParameter paramManufactorySysNoCom = new SqlParameter("@ManufactorySysNoCom", SqlDbType.NVarChar, 100);
            SqlParameter paramUseAreaSysNoCom = new SqlParameter("@UseAreaSysNoCom", SqlDbType.NVarChar, 100);
            SqlParameter paramUseCustomerSysNo = new SqlParameter("@UseCustomerSysNo", SqlDbType.Int, 4);
            SqlParameter paramUseCustomerGradeCom = new SqlParameter("@UseCustomerGradeCom", SqlDbType.NVarChar, 100);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.CouponID != AppConst.StringNull)
                paramCouponID.Value = oParam.CouponID;
            else
                paramCouponID.Value = System.DBNull.Value;
            if (oParam.CouponName != AppConst.StringNull)
                paramCouponName.Value = oParam.CouponName;
            else
                paramCouponName.Value = System.DBNull.Value;
            if (oParam.CouponCode != AppConst.StringNull)
                paramCouponCode.Value = oParam.CouponCode;
            else
                paramCouponCode.Value = System.DBNull.Value;
            if (oParam.CouponAmt != AppConst.DecimalNull)
                paramCouponAmt.Value = oParam.CouponAmt;
            else
                paramCouponAmt.Value = System.DBNull.Value;
            if (oParam.SaleAmt != AppConst.DecimalNull)
                paramSaleAmt.Value = oParam.SaleAmt;
            else
                paramSaleAmt.Value = System.DBNull.Value;
            if (oParam.CouponType != AppConst.IntNull)
                paramCouponType.Value = oParam.CouponType;
            else
                paramCouponType.Value = System.DBNull.Value;
            if (oParam.ValidTimeFrom != AppConst.DateTimeNull)
                paramValidTimeFrom.Value = oParam.ValidTimeFrom;
            else
                paramValidTimeFrom.Value = System.DBNull.Value;
            if (oParam.ValidTimeTo != AppConst.DateTimeNull)
                paramValidTimeTo.Value = oParam.ValidTimeTo;
            else
                paramValidTimeTo.Value = System.DBNull.Value;
            if (oParam.MaxUseDegree != AppConst.IntNull)
                paramMaxUseDegree.Value = oParam.MaxUseDegree;
            else
                paramMaxUseDegree.Value = System.DBNull.Value;
            if (oParam.UsedDegree != AppConst.IntNull)
                paramUsedDegree.Value = oParam.UsedDegree;
            else
                paramUsedDegree.Value = System.DBNull.Value;
            if (oParam.CreateUserSysNo != AppConst.IntNull)
                paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
            else
                paramCreateUserSysNo.Value = System.DBNull.Value;
            if (oParam.CreateTime != AppConst.DateTimeNull)
                paramCreateTime.Value = oParam.CreateTime;
            else
                paramCreateTime.Value = System.DBNull.Value;
            if (oParam.AuditUserSysNo != AppConst.IntNull)
                paramAuditUserSysNo.Value = oParam.AuditUserSysNo;
            else
                paramAuditUserSysNo.Value = System.DBNull.Value;
            if (oParam.AuditTime != AppConst.DateTimeNull)
                paramAuditTime.Value = oParam.AuditTime;
            else
                paramAuditTime.Value = System.DBNull.Value;
            if (oParam.UsedTime != AppConst.DateTimeNull)
                paramUsedTime.Value = oParam.UsedTime;
            else
                paramUsedTime.Value = System.DBNull.Value;
            if (oParam.BatchNo != AppConst.IntNull)
                paramBatchNo.Value = oParam.BatchNo;
            else
                paramBatchNo.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;
            if (oParam.CategorySysNoCom != AppConst.StringNull)
                paramCategorySysNoCom.Value = oParam.CategorySysNoCom;
            else
                paramCategorySysNoCom.Value = System.DBNull.Value;
            if (oParam.ProductSysNoCom != AppConst.StringNull)
                paramProductSysNoCom.Value = oParam.ProductSysNoCom;
            else
                paramProductSysNoCom.Value = System.DBNull.Value;
            if (oParam.ManufactorySysNoCom != AppConst.StringNull)
                paramManufactorySysNoCom.Value = oParam.ManufactorySysNoCom;
            else
                paramManufactorySysNoCom.Value = System.DBNull.Value;
            if (oParam.UseAreaSysNoCom != AppConst.StringNull)
                paramUseAreaSysNoCom.Value = oParam.UseAreaSysNoCom;
            else
                paramUseAreaSysNoCom.Value = System.DBNull.Value;
            if (oParam.UseCustomerSysNo != AppConst.IntNull)
                paramUseCustomerSysNo.Value = oParam.UseCustomerSysNo;
            else
                paramUseCustomerSysNo.Value = System.DBNull.Value;
            if (oParam.UseCustomerGradeCom != AppConst.StringNull)
                paramUseCustomerGradeCom.Value = oParam.UseCustomerGradeCom;
            else
                paramUseCustomerGradeCom.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramCouponID);
            cmd.Parameters.Add(paramCouponName);
            cmd.Parameters.Add(paramCouponCode);
            cmd.Parameters.Add(paramCouponAmt);
            cmd.Parameters.Add(paramSaleAmt);
            cmd.Parameters.Add(paramCouponType);
            cmd.Parameters.Add(paramValidTimeFrom);
            cmd.Parameters.Add(paramValidTimeTo);
            cmd.Parameters.Add(paramMaxUseDegree);
            cmd.Parameters.Add(paramUsedDegree);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramCreateTime);
            cmd.Parameters.Add(paramAuditUserSysNo);
            cmd.Parameters.Add(paramAuditTime);
            cmd.Parameters.Add(paramUsedTime);
            cmd.Parameters.Add(paramBatchNo);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramCategorySysNoCom);
            cmd.Parameters.Add(paramProductSysNoCom);
            cmd.Parameters.Add(paramManufactorySysNoCom);
            cmd.Parameters.Add(paramUseAreaSysNoCom);
            cmd.Parameters.Add(paramUseCustomerSysNo);
            cmd.Parameters.Add(paramUseCustomerGradeCom);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int Update(CouponInfo oParam)
        {
            string sql = @"UPDATE Coupon SET 
                            CouponID=@CouponID, CouponName=@CouponName, 
                            CouponCode=@CouponCode, CouponAmt=@CouponAmt, 
                            SaleAmt=@SaleAmt, CouponType=@CouponType, 
                            ValidTimeFrom=@ValidTimeFrom, ValidTimeTo=@ValidTimeTo, 
                            MaxUseDegree=@MaxUseDegree, UsedDegree=@UsedDegree, 
                            CreateUserSysNo=@CreateUserSysNo, CreateTime=@CreateTime, 
                            AuditUserSysNo=@AuditUserSysNo, AuditTime=@AuditTime, 
                            UsedTime=@UsedTime, BatchNo=@BatchNo, 
                            Status=@Status, CategorySysNoCom=@CategorySysNoCom, 
                            ProductSysNoCom=@ProductSysNoCom, ManufactorySysNoCom=@ManufactorySysNoCom, 
                            UseAreaSysNoCom=@UseAreaSysNoCom, UseCustomerSysNo=@UseCustomerSysNo, 
                            UseCustomerGradeCom=@UseCustomerGradeCom
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramCouponID = new SqlParameter("@CouponID", SqlDbType.NVarChar, 20);
            SqlParameter paramCouponName = new SqlParameter("@CouponName", SqlDbType.NVarChar, 50);
            SqlParameter paramCouponCode = new SqlParameter("@CouponCode", SqlDbType.NVarChar, 20);
            SqlParameter paramCouponAmt = new SqlParameter("@CouponAmt", SqlDbType.Decimal, 9);
            SqlParameter paramSaleAmt = new SqlParameter("@SaleAmt", SqlDbType.Decimal, 9);
            SqlParameter paramCouponType = new SqlParameter("@CouponType", SqlDbType.Int, 4);
            SqlParameter paramValidTimeFrom = new SqlParameter("@ValidTimeFrom", SqlDbType.DateTime);
            SqlParameter paramValidTimeTo = new SqlParameter("@ValidTimeTo", SqlDbType.DateTime);
            SqlParameter paramMaxUseDegree = new SqlParameter("@MaxUseDegree", SqlDbType.Int, 4);
            SqlParameter paramUsedDegree = new SqlParameter("@UsedDegree", SqlDbType.Int, 4);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
            SqlParameter paramAuditUserSysNo = new SqlParameter("@AuditUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramAuditTime = new SqlParameter("@AuditTime", SqlDbType.DateTime);
            SqlParameter paramUsedTime = new SqlParameter("@UsedTime", SqlDbType.DateTime);
            SqlParameter paramBatchNo = new SqlParameter("@BatchNo", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramCategorySysNoCom = new SqlParameter("@CategorySysNoCom", SqlDbType.NVarChar, 100);
            SqlParameter paramProductSysNoCom = new SqlParameter("@ProductSysNoCom", SqlDbType.NVarChar, 100);
            SqlParameter paramManufactorySysNoCom = new SqlParameter("@ManufactorySysNoCom", SqlDbType.NVarChar, 100);
            SqlParameter paramUseAreaSysNoCom = new SqlParameter("@UseAreaSysNoCom", SqlDbType.NVarChar, 100);
            SqlParameter paramUseCustomerSysNo = new SqlParameter("@UseCustomerSysNo", SqlDbType.Int, 4);
            SqlParameter paramUseCustomerGradeCom = new SqlParameter("@UseCustomerGradeCom", SqlDbType.NVarChar, 100);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.CouponID != AppConst.StringNull)
                paramCouponID.Value = oParam.CouponID;
            else
                paramCouponID.Value = System.DBNull.Value;
            if (oParam.CouponName != AppConst.StringNull)
                paramCouponName.Value = oParam.CouponName;
            else
                paramCouponName.Value = System.DBNull.Value;
            if (oParam.CouponCode != AppConst.StringNull)
                paramCouponCode.Value = oParam.CouponCode;
            else
                paramCouponCode.Value = System.DBNull.Value;
            if (oParam.CouponAmt != AppConst.DecimalNull)
                paramCouponAmt.Value = oParam.CouponAmt;
            else
                paramCouponAmt.Value = System.DBNull.Value;
            if (oParam.SaleAmt != AppConst.DecimalNull)
                paramSaleAmt.Value = oParam.SaleAmt;
            else
                paramSaleAmt.Value = System.DBNull.Value;
            if (oParam.CouponType != AppConst.IntNull)
                paramCouponType.Value = oParam.CouponType;
            else
                paramCouponType.Value = System.DBNull.Value;
            if (oParam.ValidTimeFrom != AppConst.DateTimeNull)
                paramValidTimeFrom.Value = oParam.ValidTimeFrom;
            else
                paramValidTimeFrom.Value = System.DBNull.Value;
            if (oParam.ValidTimeTo != AppConst.DateTimeNull)
                paramValidTimeTo.Value = oParam.ValidTimeTo;
            else
                paramValidTimeTo.Value = System.DBNull.Value;
            if (oParam.MaxUseDegree != AppConst.IntNull)
                paramMaxUseDegree.Value = oParam.MaxUseDegree;
            else
                paramMaxUseDegree.Value = System.DBNull.Value;
            if (oParam.UsedDegree != AppConst.IntNull)
                paramUsedDegree.Value = oParam.UsedDegree;
            else
                paramUsedDegree.Value = System.DBNull.Value;
            if (oParam.CreateUserSysNo != AppConst.IntNull)
                paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
            else
                paramCreateUserSysNo.Value = System.DBNull.Value;
            if (oParam.CreateTime != AppConst.DateTimeNull)
                paramCreateTime.Value = oParam.CreateTime;
            else
                paramCreateTime.Value = System.DBNull.Value;
            if (oParam.AuditUserSysNo != AppConst.IntNull)
                paramAuditUserSysNo.Value = oParam.AuditUserSysNo;
            else
                paramAuditUserSysNo.Value = System.DBNull.Value;
            if (oParam.AuditTime != AppConst.DateTimeNull)
                paramAuditTime.Value = oParam.AuditTime;
            else
                paramAuditTime.Value = System.DBNull.Value;
            if (oParam.UsedTime != AppConst.DateTimeNull)
                paramUsedTime.Value = oParam.UsedTime;
            else
                paramUsedTime.Value = System.DBNull.Value;
            if (oParam.BatchNo != AppConst.IntNull)
                paramBatchNo.Value = oParam.BatchNo;
            else
                paramBatchNo.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;
            if (oParam.CategorySysNoCom != AppConst.StringNull)
                paramCategorySysNoCom.Value = oParam.CategorySysNoCom;
            else
                paramCategorySysNoCom.Value = System.DBNull.Value;
            if (oParam.ProductSysNoCom != AppConst.StringNull)
                paramProductSysNoCom.Value = oParam.ProductSysNoCom;
            else
                paramProductSysNoCom.Value = System.DBNull.Value;
            if (oParam.ManufactorySysNoCom != AppConst.StringNull)
                paramManufactorySysNoCom.Value = oParam.ManufactorySysNoCom;
            else
                paramManufactorySysNoCom.Value = System.DBNull.Value;
            if (oParam.UseAreaSysNoCom != AppConst.StringNull)
                paramUseAreaSysNoCom.Value = oParam.UseAreaSysNoCom;
            else
                paramUseAreaSysNoCom.Value = System.DBNull.Value;
            if (oParam.UseCustomerSysNo != AppConst.IntNull)
                paramUseCustomerSysNo.Value = oParam.UseCustomerSysNo;
            else
                paramUseCustomerSysNo.Value = System.DBNull.Value;
            if (oParam.UseCustomerGradeCom != AppConst.StringNull)
                paramUseCustomerGradeCom.Value = oParam.UseCustomerGradeCom;
            else
                paramUseCustomerGradeCom.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramCouponID);
            cmd.Parameters.Add(paramCouponName);
            cmd.Parameters.Add(paramCouponCode);
            cmd.Parameters.Add(paramCouponAmt);
            cmd.Parameters.Add(paramSaleAmt);
            cmd.Parameters.Add(paramCouponType);
            cmd.Parameters.Add(paramValidTimeFrom);
            cmd.Parameters.Add(paramValidTimeTo);
            cmd.Parameters.Add(paramMaxUseDegree);
            cmd.Parameters.Add(paramUsedDegree);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramCreateTime);
            cmd.Parameters.Add(paramAuditUserSysNo);
            cmd.Parameters.Add(paramAuditTime);
            cmd.Parameters.Add(paramUsedTime);
            cmd.Parameters.Add(paramBatchNo);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramCategorySysNoCom);
            cmd.Parameters.Add(paramProductSysNoCom);
            cmd.Parameters.Add(paramManufactorySysNoCom);
            cmd.Parameters.Add(paramUseAreaSysNoCom);
            cmd.Parameters.Add(paramUseCustomerSysNo);
            cmd.Parameters.Add(paramUseCustomerGradeCom);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

    }
}
