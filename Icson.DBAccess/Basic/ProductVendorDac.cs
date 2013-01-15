using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Icson.Objects.Basic;
using Icson.Utils;

namespace Icson.DBAccess.Basic
{
    public class ProductVendorDac
    {
        public int Insert(ProductVendorInfo oParam)
        {
            string sql = @"INSERT INTO Product_Vendor
                            (
                            ProductSysNo, VendorSysNo, PurchasePrice, IsDefault, 
                            UpdateUserSysNo, UpdateTime
                            )
                            VALUES (
                            @ProductSysNo, @VendorSysNo, @PurchasePrice, @IsDefault, 
                            @UpdateUserSysNo, @UpdateTime
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int, 4);
            SqlParameter paramVendorSysNo = new SqlParameter("@VendorSysNo", SqlDbType.Int, 4);
            SqlParameter paramPurchasePrice = new SqlParameter("@PurchasePrice", SqlDbType.Decimal, 9);
            SqlParameter paramIsDefault = new SqlParameter("@IsDefault", SqlDbType.Int, 4);
            SqlParameter paramUpdateUserSysNo = new SqlParameter("@UpdateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramUpdateTime = new SqlParameter("@UpdateTime", SqlDbType.DateTime);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.ProductSysNo != AppConst.IntNull)
                paramProductSysNo.Value = oParam.ProductSysNo;
            else
                paramProductSysNo.Value = System.DBNull.Value;
            if (oParam.VendorSysNo != AppConst.IntNull)
                paramVendorSysNo.Value = oParam.VendorSysNo;
            else
                paramVendorSysNo.Value = System.DBNull.Value;
            if (oParam.PurchasePrice != AppConst.DecimalNull)
                paramPurchasePrice.Value = oParam.PurchasePrice;
            else
                paramPurchasePrice.Value = System.DBNull.Value;
            if (oParam.IsDefault != AppConst.IntNull)
                paramIsDefault.Value = oParam.IsDefault;
            else
                paramIsDefault.Value = System.DBNull.Value;
            if (oParam.UpdateUserSysNo != AppConst.IntNull)
                paramUpdateUserSysNo.Value = oParam.UpdateUserSysNo;
            else
                paramUpdateUserSysNo.Value = System.DBNull.Value;
            if (oParam.UpdateTime != AppConst.DateTimeNull)
                paramUpdateTime.Value = oParam.UpdateTime;
            else
                paramUpdateTime.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramProductSysNo);
            cmd.Parameters.Add(paramVendorSysNo);
            cmd.Parameters.Add(paramPurchasePrice);
            cmd.Parameters.Add(paramIsDefault);
            cmd.Parameters.Add(paramUpdateUserSysNo);
            cmd.Parameters.Add(paramUpdateTime);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Update(ProductVendorInfo oParam)
        {
            string sql = @"UPDATE Product_Vendor SET 
                            ProductSysNo=@ProductSysNo, VendorSysNo=@VendorSysNo, 
                            PurchasePrice=@PurchasePrice, IsDefault=@IsDefault, 
                            UpdateUserSysNo=@UpdateUserSysNo, UpdateTime=@UpdateTime
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int, 4);
            SqlParameter paramVendorSysNo = new SqlParameter("@VendorSysNo", SqlDbType.Int, 4);
            SqlParameter paramPurchasePrice = new SqlParameter("@PurchasePrice", SqlDbType.Decimal, 9);
            SqlParameter paramIsDefault = new SqlParameter("@IsDefault", SqlDbType.Int, 4);
            SqlParameter paramUpdateUserSysNo = new SqlParameter("@UpdateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramUpdateTime = new SqlParameter("@UpdateTime", SqlDbType.DateTime);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.ProductSysNo != AppConst.IntNull)
                paramProductSysNo.Value = oParam.ProductSysNo;
            else
                paramProductSysNo.Value = System.DBNull.Value;
            if (oParam.VendorSysNo != AppConst.IntNull)
                paramVendorSysNo.Value = oParam.VendorSysNo;
            else
                paramVendorSysNo.Value = System.DBNull.Value;
            if (oParam.PurchasePrice != AppConst.DecimalNull)
                paramPurchasePrice.Value = oParam.PurchasePrice;
            else
                paramPurchasePrice.Value = System.DBNull.Value;
            if (oParam.IsDefault != AppConst.IntNull)
                paramIsDefault.Value = oParam.IsDefault;
            else
                paramIsDefault.Value = System.DBNull.Value;
            if (oParam.UpdateUserSysNo != AppConst.IntNull)
                paramUpdateUserSysNo.Value = oParam.UpdateUserSysNo;
            else
                paramUpdateUserSysNo.Value = System.DBNull.Value;
            if (oParam.UpdateTime != AppConst.DateTimeNull)
                paramUpdateTime.Value = oParam.UpdateTime;
            else
                paramUpdateTime.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramProductSysNo);
            cmd.Parameters.Add(paramVendorSysNo);
            cmd.Parameters.Add(paramPurchasePrice);
            cmd.Parameters.Add(paramIsDefault);
            cmd.Parameters.Add(paramUpdateUserSysNo);
            cmd.Parameters.Add(paramUpdateTime);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
    }
}
