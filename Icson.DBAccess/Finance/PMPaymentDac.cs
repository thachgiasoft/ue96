using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Text;

using Icson.Utils;
using Icson.Objects.Finance;

namespace Icson.DBAccess.Finance
{
    public class PMPaymentDac
    {
        public PMPaymentDac()
        { }

        public int Insert(PMPaymentInfo oParam)
        {
            string sql = @"INSERT INTO PMPayment
                            (
                            PayDate, PMSysNo, PayAmt, StockAmt, 
                            DateStamp
                            )
                            VALUES (
                            @PayDate, @PMSysNo, @PayAmt, @StockAmt, 
                            @DateStamp
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramPayDate = new SqlParameter("@PayDate", SqlDbType.DateTime);
            SqlParameter paramPMSysNo = new SqlParameter("@PMSysNo", SqlDbType.Int, 4);
            SqlParameter paramPayAmt = new SqlParameter("@PayAmt", SqlDbType.Decimal, 9);
            SqlParameter paramStockAmt = new SqlParameter("@StockAmt", SqlDbType.Decimal, 9);
            SqlParameter paramDateStamp = new SqlParameter("@DateStamp", SqlDbType.DateTime);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.PayDate != AppConst.DateTimeNull)
                paramPayDate.Value = oParam.PayDate;
            else
                paramPayDate.Value = System.DBNull.Value;
            if (oParam.PMSysNo != AppConst.IntNull)
                paramPMSysNo.Value = oParam.PMSysNo;
            else
                paramPMSysNo.Value = System.DBNull.Value;
            if (oParam.PayAmt != AppConst.DecimalNull)
                paramPayAmt.Value = oParam.PayAmt;
            else
                paramPayAmt.Value = System.DBNull.Value;
            if (oParam.StockAmt != AppConst.DecimalNull)
                paramStockAmt.Value = oParam.StockAmt;
            else
                paramStockAmt.Value = System.DBNull.Value;
            if (oParam.DateStamp != AppConst.DateTimeNull)
                paramDateStamp.Value = oParam.DateStamp;
            else
                paramDateStamp.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramPayDate);
            cmd.Parameters.Add(paramPMSysNo);
            cmd.Parameters.Add(paramPayAmt);
            cmd.Parameters.Add(paramStockAmt);
            cmd.Parameters.Add(paramDateStamp);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }
    }
}
