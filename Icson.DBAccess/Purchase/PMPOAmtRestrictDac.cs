using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Text;

using Icson.Utils;
using Icson.Objects.Purchase;

namespace Icson.DBAccess.Purchase
{
    public class PMPOAmtRestrictDac
    {
        public PMPOAmtRestrictDac()
        {
        }

        public int Insert(PMPOAmtRestrictInfo oParam)
        {
            string sql = @"INSERT INTO PM_POAmtRestrict
                            (
                            PMUserSysNo, PMGroupNo, IsPMD, DailyPOAmtMax, 
                            EachPOAmtMax
                            )
                            VALUES (
                            @PMUserSysNo, @PMGroupNo, @IsPMD, @DailyPOAmtMax, 
                            @EachPOAmtMax
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramPMUserSysNo = new SqlParameter("@PMUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramPMGroupNo = new SqlParameter("@PMGroupNo", SqlDbType.Int, 4);
            SqlParameter paramIsPMD = new SqlParameter("@IsPMD", SqlDbType.Bit, 1);
            SqlParameter paramDailyPOAmtMax = new SqlParameter("@DailyPOAmtMax", SqlDbType.Decimal, 9);
            SqlParameter paramEachPOAmtMax = new SqlParameter("@EachPOAmtMax", SqlDbType.Decimal, 9);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.PMUserSysNo != AppConst.IntNull)
                paramPMUserSysNo.Value = oParam.PMUserSysNo;
            else
                paramPMUserSysNo.Value = System.DBNull.Value;
            if (oParam.PMGroupNo != AppConst.IntNull)
                paramPMGroupNo.Value = oParam.PMGroupNo;
            else
                paramPMGroupNo.Value = System.DBNull.Value;
            if (oParam.IsPMD != AppConst.IntNull)
                paramIsPMD.Value = oParam.IsPMD;
            else
                paramIsPMD.Value = System.DBNull.Value;
            if (oParam.DailyPOAmtMax != AppConst.DecimalNull)
                paramDailyPOAmtMax.Value = oParam.DailyPOAmtMax;
            else
                paramDailyPOAmtMax.Value = System.DBNull.Value;
            if (oParam.EachPOAmtMax != AppConst.DecimalNull)
                paramEachPOAmtMax.Value = oParam.EachPOAmtMax;
            else
                paramEachPOAmtMax.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramPMUserSysNo);
            cmd.Parameters.Add(paramPMGroupNo);
            cmd.Parameters.Add(paramIsPMD);
            cmd.Parameters.Add(paramDailyPOAmtMax);
            cmd.Parameters.Add(paramEachPOAmtMax);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Update(PMPOAmtRestrictInfo oParam)
        {
            string sql = @"UPDATE PM_POAmtRestrict SET 
                            PMUserSysNo=@PMUserSysNo, PMGroupNo=@PMGroupNo, 
                            IsPMD=@IsPMD, DailyPOAmtMax=@DailyPOAmtMax, 
                            EachPOAmtMax=@EachPOAmtMax
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramPMUserSysNo = new SqlParameter("@PMUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramPMGroupNo = new SqlParameter("@PMGroupNo", SqlDbType.Int, 4);
            SqlParameter paramIsPMD = new SqlParameter("@IsPMD", SqlDbType.Bit, 1);
            SqlParameter paramDailyPOAmtMax = new SqlParameter("@DailyPOAmtMax", SqlDbType.Decimal, 9);
            SqlParameter paramEachPOAmtMax = new SqlParameter("@EachPOAmtMax", SqlDbType.Decimal, 9);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.PMUserSysNo != AppConst.IntNull)
                paramPMUserSysNo.Value = oParam.PMUserSysNo;
            else
                paramPMUserSysNo.Value = System.DBNull.Value;
            if (oParam.PMGroupNo != AppConst.IntNull)
                paramPMGroupNo.Value = oParam.PMGroupNo;
            else
                paramPMGroupNo.Value = System.DBNull.Value;
            if (oParam.IsPMD != AppConst.IntNull)
                paramIsPMD.Value = oParam.IsPMD;
            else
                paramIsPMD.Value = System.DBNull.Value;
            if (oParam.DailyPOAmtMax != AppConst.DecimalNull)
                paramDailyPOAmtMax.Value = oParam.DailyPOAmtMax;
            else
                paramDailyPOAmtMax.Value = System.DBNull.Value;
            if (oParam.EachPOAmtMax != AppConst.DecimalNull)
                paramEachPOAmtMax.Value = oParam.EachPOAmtMax;
            else
                paramEachPOAmtMax.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramPMUserSysNo);
            cmd.Parameters.Add(paramPMGroupNo);
            cmd.Parameters.Add(paramIsPMD);
            cmd.Parameters.Add(paramDailyPOAmtMax);
            cmd.Parameters.Add(paramEachPOAmtMax);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

    }
}
