using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Icson.Objects.Sale;
using Icson.Utils;

namespace Icson.DBAccess.Sale
{
    public class SOAlipayDac
    {
        public int Insert(SOAlipayInfo oParam)
        {
            string sql = @"INSERT INTO SO_Alipay
                            (
                            SOSysNo, AlipayTradeNo
                            )
                            VALUES (
                            @SOSysNo, @AlipayTradeNo
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int, 4);
            SqlParameter paramAlipayTradeNo = new SqlParameter("@AlipayTradeNo", SqlDbType.NVarChar, 50);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.SOSysNo != AppConst.IntNull)
                paramSOSysNo.Value = oParam.SOSysNo;
            else
                paramSOSysNo.Value = System.DBNull.Value;
            if (oParam.AlipayTradeNo != AppConst.StringNull)
                paramAlipayTradeNo.Value = oParam.AlipayTradeNo;
            else
                paramAlipayTradeNo.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramSOSysNo);
            cmd.Parameters.Add(paramAlipayTradeNo);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Update(SOAlipayInfo oParam)
        {
            string sql = @"UPDATE SO_Alipay SET 
                            SOSysNo=@SOSysNo, AlipayTradeNo=@AlipayTradeNo
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int, 4);
            SqlParameter paramAlipayTradeNo = new SqlParameter("@AlipayTradeNo", SqlDbType.NVarChar, 50);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.SOSysNo != AppConst.IntNull)
                paramSOSysNo.Value = oParam.SOSysNo;
            else
                paramSOSysNo.Value = System.DBNull.Value;
            if (oParam.AlipayTradeNo != AppConst.StringNull)
                paramAlipayTradeNo.Value = oParam.AlipayTradeNo;
            else
                paramAlipayTradeNo.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramSOSysNo);
            cmd.Parameters.Add(paramAlipayTradeNo);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
    }
}
