using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects.Sale;
namespace Icson.DBAccess.Sale
{
    public class WHUserWorkRatioDac
    {
        public int Insert(WHUserWorkRatioInfo oParam)
        {
            string sql = @"INSERT INTO WH_User_WorkRatio
                            (
                            UserSysNo, Ratio, WorkType, BillType, 
                            WorkTimeSpan
                            )
                            VALUES (
                            @UserSysNo, @Ratio, @WorkType, @BillType, 
                            @WorkTimeSpan
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramUserSysNo = new SqlParameter("@UserSysNo", SqlDbType.Int, 4);
            SqlParameter paramRatio = new SqlParameter("@Ratio", SqlDbType.Int, 4);
            SqlParameter paramWorkType = new SqlParameter("@WorkType", SqlDbType.Int, 4);
            SqlParameter paramBillType = new SqlParameter("@BillType", SqlDbType.Int, 4);
            SqlParameter paramWorkTimeSpan = new SqlParameter("@WorkTimeSpan", SqlDbType.Int, 4);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.UserSysNo != AppConst.IntNull)
                paramUserSysNo.Value = oParam.UserSysNo;
            else
                paramUserSysNo.Value = System.DBNull.Value;
            if (oParam.Ratio != AppConst.IntNull)
                paramRatio.Value = oParam.Ratio;
            else
                paramRatio.Value = System.DBNull.Value;
            if (oParam.WorkType != AppConst.IntNull)
                paramWorkType.Value = oParam.WorkType;
            else
                paramWorkType.Value = System.DBNull.Value;
            if (oParam.BillType != AppConst.IntNull)
                paramBillType.Value = oParam.BillType;
            else
                paramBillType.Value = System.DBNull.Value;
            if (oParam.WorkTimeSpan != AppConst.IntNull)
                paramWorkTimeSpan.Value = oParam.WorkTimeSpan;
            else
                paramWorkTimeSpan.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramUserSysNo);
            cmd.Parameters.Add(paramRatio);
            cmd.Parameters.Add(paramWorkType);
            cmd.Parameters.Add(paramBillType);
            cmd.Parameters.Add(paramWorkTimeSpan);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }
        public int Update(WHUserWorkRatioInfo oParam)
        {
            string sql = @"UPDATE WH_User_WorkRatio SET 
                            UserSysNo=@UserSysNo, Ratio=@Ratio, 
                            WorkType=@WorkType, BillType=@BillType, 
                            WorkTimeSpan=@WorkTimeSpan
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramUserSysNo = new SqlParameter("@UserSysNo", SqlDbType.Int, 4);
            SqlParameter paramRatio = new SqlParameter("@Ratio", SqlDbType.Int, 4);
            SqlParameter paramWorkType = new SqlParameter("@WorkType", SqlDbType.Int, 4);
            SqlParameter paramBillType = new SqlParameter("@BillType", SqlDbType.Int, 4);
            SqlParameter paramWorkTimeSpan = new SqlParameter("@WorkTimeSpan", SqlDbType.Int, 4);

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
            if (oParam.WorkType != AppConst.IntNull)
                paramWorkType.Value = oParam.WorkType;
            else
                paramWorkType.Value = System.DBNull.Value;
            if (oParam.BillType != AppConst.IntNull)
                paramBillType.Value = oParam.BillType;
            else
                paramBillType.Value = System.DBNull.Value;
            if (oParam.WorkTimeSpan != AppConst.IntNull)
                paramWorkTimeSpan.Value = oParam.WorkTimeSpan;
            else
                paramWorkTimeSpan.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramUserSysNo);
            cmd.Parameters.Add(paramRatio);
            cmd.Parameters.Add(paramWorkType);
            cmd.Parameters.Add(paramBillType);
            cmd.Parameters.Add(paramWorkTimeSpan);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
    }
}
