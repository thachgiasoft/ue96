using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects.Sale;

namespace Icson.DBAccess.Sale
{
    public class SOUserAuditRatioDac
    {

        public int Insert(SOUserAuditRatioInfo oParam)
        {
            string sql = @"INSERT INTO SO_User_AuditRatio
                            (
                            UserSysNo, Ratio, AuditTimeSpan
                            )
                            VALUES (
                            @UserSysNo, @Ratio, @AuditTimeSpan
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramUserSysNo = new SqlParameter("@UserSysNo", SqlDbType.Int, 4);
            SqlParameter paramRatio = new SqlParameter("@Ratio", SqlDbType.Int, 4);
            SqlParameter paramAuditTimeSpan = new SqlParameter("@AuditTimeSpan", SqlDbType.Int, 4);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.UserSysNo != AppConst.IntNull)
                paramUserSysNo.Value = oParam.UserSysNo;
            else
                paramUserSysNo.Value = System.DBNull.Value;
            if (oParam.Ratio != AppConst.IntNull)
                paramRatio.Value = oParam.Ratio;
            else
                paramRatio.Value = System.DBNull.Value;
            if (oParam.AuditTimeSpan != AppConst.IntNull)
                paramAuditTimeSpan.Value = oParam.AuditTimeSpan;
            else
                paramAuditTimeSpan.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramUserSysNo);
            cmd.Parameters.Add(paramRatio);
            cmd.Parameters.Add(paramAuditTimeSpan);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Update(SOUserAuditRatioInfo oParam)
        {
            string sql = @"UPDATE SO_User_AuditRatio SET 
                            UserSysNo=@UserSysNo, Ratio=@Ratio, 
                            AuditTimeSpan=@AuditTimeSpan
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramUserSysNo = new SqlParameter("@UserSysNo", SqlDbType.Int, 4);
            SqlParameter paramRatio = new SqlParameter("@Ratio", SqlDbType.Int, 4);
            SqlParameter paramAuditTimeSpan = new SqlParameter("@AuditTimeSpan", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.UserSysNo != AppConst.IntNull)
                paramUserSysNo.Value = oParam.UserSysNo;
            else
                paramUserSysNo.Value = System.DBNull.Value;
            if (oParam.Ratio != AppConst.IntNull)
                paramRatio.Value = oParam.Ratio;
            else
                paramRatio.Value = System.DBNull.Value;
            if (oParam.AuditTimeSpan != AppConst.IntNull)
                paramAuditTimeSpan.Value = oParam.AuditTimeSpan;
            else
                paramAuditTimeSpan.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramUserSysNo);
            cmd.Parameters.Add(paramRatio);
            cmd.Parameters.Add(paramAuditTimeSpan);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
    }
}