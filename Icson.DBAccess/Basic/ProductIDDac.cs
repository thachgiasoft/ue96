using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Icson.Objects.Basic;
using Icson.Utils;

namespace Icson.DBAccess.Basic
{
    public class ProductIDDac
    {
        public int Insert(ProductIDInfo oParam)
        {
            string sql = @"INSERT INTO Product_ID
                            (
                            ProductSysNo, POSysNo, OrderNum, Status, 
                            ProductSN, ProductTrackSN, Note
                            )
                            VALUES (
                            @ProductSysNo, @POSysNo, @OrderNum, @Status, 
                            @ProductSN, @ProductTrackSN, @Note
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int, 4);
            SqlParameter paramPOSysNo = new SqlParameter("@POSysNo", SqlDbType.Int, 4);
            SqlParameter paramOrderNum = new SqlParameter("@OrderNum", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramProductSN = new SqlParameter("@ProductSN", SqlDbType.NVarChar, 50);
            SqlParameter paramProductTrackSN = new SqlParameter("@ProductTrackSN", SqlDbType.NVarChar, 50);
            SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.NVarChar, 500);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.ProductSysNo != AppConst.IntNull)
                paramProductSysNo.Value = oParam.ProductSysNo;
            else
                paramProductSysNo.Value = System.DBNull.Value;
            if (oParam.POSysNo != AppConst.IntNull)
                paramPOSysNo.Value = oParam.POSysNo;
            else
                paramPOSysNo.Value = System.DBNull.Value;
            if (oParam.OrderNum != AppConst.IntNull)
                paramOrderNum.Value = oParam.OrderNum;
            else
                paramOrderNum.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;
            if (oParam.ProductSN != AppConst.StringNull)
                paramProductSN.Value = oParam.ProductSN;
            else
                paramProductSN.Value = System.DBNull.Value;
            if (oParam.ProductTrackSN != AppConst.StringNull)
                paramProductTrackSN.Value = oParam.ProductTrackSN;
            else
                paramProductTrackSN.Value = System.DBNull.Value;
            if (oParam.Note != AppConst.StringNull)
                paramNote.Value = oParam.Note;
            else
                paramNote.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramProductSysNo);
            cmd.Parameters.Add(paramPOSysNo);
            cmd.Parameters.Add(paramOrderNum);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramProductSN);
            cmd.Parameters.Add(paramProductTrackSN);
            cmd.Parameters.Add(paramNote);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Update(ProductIDInfo oParam)
        {
            string sql = @"UPDATE Product_ID SET 
                            ProductSysNo=@ProductSysNo, POSysNo=@POSysNo, 
                            OrderNum=@OrderNum, Status=@Status, 
                            ProductSN=@ProductSN, ProductTrackSN=@ProductTrackSN, 
                            Note=@Note
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int, 4);
            SqlParameter paramPOSysNo = new SqlParameter("@POSysNo", SqlDbType.Int, 4);
            SqlParameter paramOrderNum = new SqlParameter("@OrderNum", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramProductSN = new SqlParameter("@ProductSN", SqlDbType.NVarChar, 50);
            SqlParameter paramProductTrackSN = new SqlParameter("@ProductTrackSN", SqlDbType.NVarChar, 50);
            SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.NVarChar, 500);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.ProductSysNo != AppConst.IntNull)
                paramProductSysNo.Value = oParam.ProductSysNo;
            else
                paramProductSysNo.Value = System.DBNull.Value;
            if (oParam.POSysNo != AppConst.IntNull)
                paramPOSysNo.Value = oParam.POSysNo;
            else
                paramPOSysNo.Value = System.DBNull.Value;
            if (oParam.OrderNum != AppConst.IntNull)
                paramOrderNum.Value = oParam.OrderNum;
            else
                paramOrderNum.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;
            if (oParam.ProductSN != AppConst.StringNull)
                paramProductSN.Value = oParam.ProductSN;
            else
                paramProductSN.Value = System.DBNull.Value;
            if (oParam.ProductTrackSN != AppConst.StringNull)
                paramProductTrackSN.Value = oParam.ProductTrackSN;
            else
                paramProductTrackSN.Value = System.DBNull.Value;
            if (oParam.Note != AppConst.StringNull)
                paramNote.Value = oParam.Note;
            else
                paramNote.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramProductSysNo);
            cmd.Parameters.Add(paramPOSysNo);
            cmd.Parameters.Add(paramOrderNum);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramProductSN);
            cmd.Parameters.Add(paramProductTrackSN);
            cmd.Parameters.Add(paramNote);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
    }
}