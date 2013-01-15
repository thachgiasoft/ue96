using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Icson.Objects.Online;
using Icson.Utils;

namespace Icson.DBAccess.Online
{
    public class OnlineC1_ProductDac
    {
        public int Insert(OnlineC1_ProductInfo oParam)
        {
            string sql = @"INSERT INTO OnlineC1_Product
                            (
                            ProductSysNo,ProductBriefName, OrderNum
                            )
                            VALUES (
                            @ProductSysNo,@ProductBriefName, @OrderNum
                            )";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int, 4);
            SqlParameter paramProductBriefName = new SqlParameter("@ProductBriefName",SqlDbType.NVarChar,200);
            SqlParameter paramOrderNum = new SqlParameter("@OrderNum", SqlDbType.Int, 4);

            if (oParam.ProductSysNo != AppConst.IntNull)
                paramProductSysNo.Value = oParam.ProductSysNo;
            else
                paramProductSysNo.Value = System.DBNull.Value;
            if (oParam.ProductBriefName != AppConst.StringNull)
                paramProductBriefName.Value = oParam.ProductBriefName;
            else
                paramProductBriefName.Value = System.DBNull.Value;
            if (oParam.OrderNum != AppConst.IntNull)
                paramOrderNum.Value = oParam.OrderNum;
            else
                paramOrderNum.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramProductSysNo);
            cmd.Parameters.Add(paramProductBriefName);
            cmd.Parameters.Add(paramOrderNum);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int Update(OnlineC1_ProductInfo oParam)
        {
            string sql = @"UPDATE OnlineC1_Product SET 
                            ProductSysNo=@ProductSysNo,ProductBriefName=@ProductBriefName, OrderNum=@OrderNum
                            WHERE ProductSysNo=@ProductSysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int, 4);
            SqlParameter paramProductBriefName = new SqlParameter("@ProductBriefName",SqlDbType.NVarChar,200);
            SqlParameter paramOrderNum = new SqlParameter("@OrderNum", SqlDbType.Int, 4);

            if (oParam.ProductSysNo != AppConst.IntNull)
                paramProductSysNo.Value = oParam.ProductSysNo;
            else
                paramProductSysNo.Value = System.DBNull.Value;
            if (oParam.ProductBriefName != AppConst.StringNull)
                paramProductBriefName.Value = oParam.ProductBriefName;
            else
                paramProductBriefName.Value = System.DBNull.Value;
            if (oParam.OrderNum != AppConst.IntNull)
                paramOrderNum.Value = oParam.OrderNum;
            else
                paramOrderNum.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramProductSysNo);
            cmd.Parameters.Add(paramProductBriefName);
            cmd.Parameters.Add(paramOrderNum);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int Delete(OnlineC1_ProductInfo oParam)
        {
            string sql = "DELETE OnlineC1_Product where ProductSysNo=@ProductSysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int, 4);

            if (oParam.ProductSysNo != AppConst.IntNull)
                paramProductSysNo.Value = oParam.ProductSysNo;
            else
                paramProductSysNo.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramProductSysNo);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int SetOrderNum(OnlineC1_ProductInfo oParam)
        {
            string sql = "update onlineC1_Product set ordernum = " + oParam.OrderNum + " where productsysno = " + oParam.ProductSysNo;
            SqlCommand cmd = new SqlCommand(sql);
            return SqlHelper.ExecuteNonQuery(cmd);
        }
    }
}
