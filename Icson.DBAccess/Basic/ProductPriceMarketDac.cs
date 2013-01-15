using System;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects.Basic;

namespace Icson.DBAccess.Basic
{
    public class ProductPriceMarketDac
    {
        public int Insert(ProductPriceMarketInfo oParam)
        {
            string sql = @"INSERT INTO Product_Price_Market
                            (
                            ProductSysNo, MarketLowestPrice, MarketUrl, CreateMemo, 
                            CreateUserSysNo, CreateTime, AuditMemo, AuditUserSysNo, 
                            AuditTime, Status
                            )
                            VALUES (
                            @ProductSysNo, @MarketLowestPrice, @MarketUrl, @CreateMemo, 
                            @CreateUserSysNo, @CreateTime, @AuditMemo, @AuditUserSysNo, 
                            @AuditTime, @Status
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int, 4);
            SqlParameter paramMarketLowestPrice = new SqlParameter("@MarketLowestPrice", SqlDbType.Decimal, 9);
            SqlParameter paramMarketUrl = new SqlParameter("@MarketUrl", SqlDbType.NVarChar, 500);
            SqlParameter paramCreateMemo = new SqlParameter("@CreateMemo", SqlDbType.NVarChar, 500);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
            SqlParameter paramAuditMemo = new SqlParameter("@AuditMemo", SqlDbType.NVarChar, 500);
            SqlParameter paramAuditUserSysNo = new SqlParameter("@AuditUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramAuditTime = new SqlParameter("@AuditTime", SqlDbType.DateTime);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.ProductSysNo != AppConst.IntNull)
                paramProductSysNo.Value = oParam.ProductSysNo;
            else
                paramProductSysNo.Value = System.DBNull.Value;
            if (oParam.MarketLowestPrice != AppConst.DecimalNull)
                paramMarketLowestPrice.Value = oParam.MarketLowestPrice;
            else
                paramMarketLowestPrice.Value = System.DBNull.Value;
            if (oParam.MarketUrl != AppConst.StringNull)
                paramMarketUrl.Value = oParam.MarketUrl;
            else
                paramMarketUrl.Value = System.DBNull.Value;
            if (oParam.CreateMemo != AppConst.StringNull)
                paramCreateMemo.Value = oParam.CreateMemo;
            else
                paramCreateMemo.Value = System.DBNull.Value;
            if (oParam.CreateUserSysNo != AppConst.IntNull)
                paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
            else
                paramCreateUserSysNo.Value = System.DBNull.Value;
            if (oParam.CreateTime != AppConst.DateTimeNull)
                paramCreateTime.Value = oParam.CreateTime;
            else
                paramCreateTime.Value = System.DBNull.Value;
            if (oParam.AuditMemo != AppConst.StringNull)
                paramAuditMemo.Value = oParam.AuditMemo;
            else
                paramAuditMemo.Value = System.DBNull.Value;
            if (oParam.AuditUserSysNo != AppConst.IntNull)
                paramAuditUserSysNo.Value = oParam.AuditUserSysNo;
            else
                paramAuditUserSysNo.Value = System.DBNull.Value;
            if (oParam.AuditTime != AppConst.DateTimeNull)
                paramAuditTime.Value = oParam.AuditTime;
            else
                paramAuditTime.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramProductSysNo);
            cmd.Parameters.Add(paramMarketLowestPrice);
            cmd.Parameters.Add(paramMarketUrl);
            cmd.Parameters.Add(paramCreateMemo);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramCreateTime);
            cmd.Parameters.Add(paramAuditMemo);
            cmd.Parameters.Add(paramAuditUserSysNo);
            cmd.Parameters.Add(paramAuditTime);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Update(ProductPriceMarketInfo oParam)
        {
            string sql = @"UPDATE Product_Price_Market SET 
                            ProductSysNo=@ProductSysNo, MarketLowestPrice=@MarketLowestPrice, 
                            MarketUrl=@MarketUrl, CreateMemo=@CreateMemo, 
                            CreateUserSysNo=@CreateUserSysNo, CreateTime=@CreateTime, 
                            AuditMemo=@AuditMemo, AuditUserSysNo=@AuditUserSysNo, 
                            AuditTime=@AuditTime, Status=@Status
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int, 4);
            SqlParameter paramMarketLowestPrice = new SqlParameter("@MarketLowestPrice", SqlDbType.Decimal, 9);
            SqlParameter paramMarketUrl = new SqlParameter("@MarketUrl", SqlDbType.NVarChar, 500);
            SqlParameter paramCreateMemo = new SqlParameter("@CreateMemo", SqlDbType.NVarChar, 500);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
            SqlParameter paramAuditMemo = new SqlParameter("@AuditMemo", SqlDbType.NVarChar, 500);
            SqlParameter paramAuditUserSysNo = new SqlParameter("@AuditUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramAuditTime = new SqlParameter("@AuditTime", SqlDbType.DateTime);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.ProductSysNo != AppConst.IntNull)
                paramProductSysNo.Value = oParam.ProductSysNo;
            else
                paramProductSysNo.Value = System.DBNull.Value;
            if (oParam.MarketLowestPrice != AppConst.DecimalNull)
                paramMarketLowestPrice.Value = oParam.MarketLowestPrice;
            else
                paramMarketLowestPrice.Value = System.DBNull.Value;
            if (oParam.MarketUrl != AppConst.StringNull)
                paramMarketUrl.Value = oParam.MarketUrl;
            else
                paramMarketUrl.Value = System.DBNull.Value;
            if (oParam.CreateMemo != AppConst.StringNull)
                paramCreateMemo.Value = oParam.CreateMemo;
            else
                paramCreateMemo.Value = System.DBNull.Value;
            if (oParam.CreateUserSysNo != AppConst.IntNull)
                paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
            else
                paramCreateUserSysNo.Value = System.DBNull.Value;
            if (oParam.CreateTime != AppConst.DateTimeNull)
                paramCreateTime.Value = oParam.CreateTime;
            else
                paramCreateTime.Value = System.DBNull.Value;
            if (oParam.AuditMemo != AppConst.StringNull)
                paramAuditMemo.Value = oParam.AuditMemo;
            else
                paramAuditMemo.Value = System.DBNull.Value;
            if (oParam.AuditUserSysNo != AppConst.IntNull)
                paramAuditUserSysNo.Value = oParam.AuditUserSysNo;
            else
                paramAuditUserSysNo.Value = System.DBNull.Value;
            if (oParam.AuditTime != AppConst.DateTimeNull)
                paramAuditTime.Value = oParam.AuditTime;
            else
                paramAuditTime.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramProductSysNo);
            cmd.Parameters.Add(paramMarketLowestPrice);
            cmd.Parameters.Add(paramMarketUrl);
            cmd.Parameters.Add(paramCreateMemo);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramCreateTime);
            cmd.Parameters.Add(paramAuditMemo);
            cmd.Parameters.Add(paramAuditUserSysNo);
            cmd.Parameters.Add(paramAuditTime);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int Deletes(string priceSysNos)
        {
            string sql = "delete from Product_Price_Market where SysNo in (" + priceSysNos + ")";
            return SqlHelper.ExecuteNonQuery(sql);
        }
    }
}
