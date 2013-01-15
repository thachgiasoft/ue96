using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects;
using Icson.Objects.Sale;

namespace Icson.DBAccess.Sale
{
    public class EqifaLogDac
    {
        public int Insert(EqifaLogInfo oParam)
        {
            string sql = @"INSERT INTO Eqifa_Log
                            (
                            SOSysNo, ProductSysNo, Quantity, Price, 
                            EqifaLog
                            )
                            VALUES (
                            @SOSysNo, @ProductSysNo, @Quantity, @Price, 
                            @EqifaLog
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int, 4);
            SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int, 4);
            SqlParameter paramQuantity = new SqlParameter("@Quantity", SqlDbType.Int, 4);
            SqlParameter paramPrice = new SqlParameter("@Price", SqlDbType.Decimal, 9);
            SqlParameter paramEqifaLog = new SqlParameter("@EqifaLog", SqlDbType.NVarChar, 2000);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.SOSysNo != AppConst.IntNull)
                paramSOSysNo.Value = oParam.SOSysNo;
            else
                paramSOSysNo.Value = System.DBNull.Value;
            if (oParam.ProductSysNo != AppConst.IntNull)
                paramProductSysNo.Value = oParam.ProductSysNo;
            else
                paramProductSysNo.Value = System.DBNull.Value;
            if (oParam.Quantity != AppConst.IntNull)
                paramQuantity.Value = oParam.Quantity;
            else
                paramQuantity.Value = System.DBNull.Value;
            if (oParam.Price != AppConst.DecimalNull)
                paramPrice.Value = oParam.Price;
            else
                paramPrice.Value = System.DBNull.Value;
            if (oParam.EqifaLog != AppConst.StringNull)
                paramEqifaLog.Value = oParam.EqifaLog;
            else
                paramEqifaLog.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramSOSysNo);
            cmd.Parameters.Add(paramProductSysNo);
            cmd.Parameters.Add(paramQuantity);
            cmd.Parameters.Add(paramPrice);
            cmd.Parameters.Add(paramEqifaLog);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Update(EqifaLogInfo oParam)
        {
            string sql = @"UPDATE Eqifa_Log SET 
                            SOSysNo=@SOSysNo, ProductSysNo=@ProductSysNo, 
                            Quantity=@Quantity, Price=@Price, 
                            EqifaLog=@EqifaLog
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int, 4);
            SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int, 4);
            SqlParameter paramQuantity = new SqlParameter("@Quantity", SqlDbType.Int, 4);
            SqlParameter paramPrice = new SqlParameter("@Price", SqlDbType.Decimal, 9);
            SqlParameter paramEqifaLog = new SqlParameter("@EqifaLog", SqlDbType.NVarChar, 2000);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.SOSysNo != AppConst.IntNull)
                paramSOSysNo.Value = oParam.SOSysNo;
            else
                paramSOSysNo.Value = System.DBNull.Value;
            if (oParam.ProductSysNo != AppConst.IntNull)
                paramProductSysNo.Value = oParam.ProductSysNo;
            else
                paramProductSysNo.Value = System.DBNull.Value;
            if (oParam.Quantity != AppConst.IntNull)
                paramQuantity.Value = oParam.Quantity;
            else
                paramQuantity.Value = System.DBNull.Value;
            if (oParam.Price != AppConst.DecimalNull)
                paramPrice.Value = oParam.Price;
            else
                paramPrice.Value = System.DBNull.Value;
            if (oParam.EqifaLog != AppConst.StringNull)
                paramEqifaLog.Value = oParam.EqifaLog;
            else
                paramEqifaLog.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramSOSysNo);
            cmd.Parameters.Add(paramProductSysNo);
            cmd.Parameters.Add(paramQuantity);
            cmd.Parameters.Add(paramPrice);
            cmd.Parameters.Add(paramEqifaLog);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
    }
}
