using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Text;

using Icson.Utils;
using Icson.Objects.Finance;

namespace Icson.DBAccess.Finance
{
    public class DailyPayAmtRestrictDac
    {
        public DailyPayAmtRestrictDac()
        {
        }

        public int Insert(DailyPayAmtRestrictInfo oParam)
        {
            string sql = @"INSERT INTO Fin_DailyPayAmtRestrict
                            (
                            TopPublicPayAmt, AllocatedPublicAmt, TopPrivatePayAmt, AllocatedPrivateAmt, 
                            CreateUserSysNo, CreateTime
                            )
                            VALUES (
                            @TopPublicPayAmt, @AllocatedPublicAmt, @TopPrivatePayAmt, @AllocatedPrivateAmt, 
                            @CreateUserSysNo, @CreateTime
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramTopPublicPayAmt = new SqlParameter("@TopPublicPayAmt", SqlDbType.Decimal, 9);
            SqlParameter paramAllocatedPublicAmt = new SqlParameter("@AllocatedPublicAmt", SqlDbType.Decimal, 9);
            SqlParameter paramTopPrivatePayAmt = new SqlParameter("@TopPrivatePayAmt", SqlDbType.Decimal, 9);
            SqlParameter paramAllocatedPrivateAmt = new SqlParameter("@AllocatedPrivateAmt", SqlDbType.Decimal, 9);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.TopPublicPayAmt != AppConst.DecimalNull)
                paramTopPublicPayAmt.Value = oParam.TopPublicPayAmt;
            else
                paramTopPublicPayAmt.Value = System.DBNull.Value;
            if (oParam.AllocatedPublicAmt != AppConst.DecimalNull)
                paramAllocatedPublicAmt.Value = oParam.AllocatedPublicAmt;
            else
                paramAllocatedPublicAmt.Value = System.DBNull.Value;
            if (oParam.TopPrivatePayAmt != AppConst.DecimalNull)
                paramTopPrivatePayAmt.Value = oParam.TopPrivatePayAmt;
            else
                paramTopPrivatePayAmt.Value = System.DBNull.Value;
            if (oParam.AllocatedPrivateAmt != AppConst.DecimalNull)
                paramAllocatedPrivateAmt.Value = oParam.AllocatedPrivateAmt;
            else
                paramAllocatedPrivateAmt.Value = System.DBNull.Value;
            if (oParam.CreateUserSysNo != AppConst.IntNull)
                paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
            else
                paramCreateUserSysNo.Value = System.DBNull.Value;
            if (oParam.CreateTime != AppConst.DateTimeNull)
                paramCreateTime.Value = oParam.CreateTime;
            else
                paramCreateTime.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramTopPublicPayAmt);
            cmd.Parameters.Add(paramAllocatedPublicAmt);
            cmd.Parameters.Add(paramTopPrivatePayAmt);
            cmd.Parameters.Add(paramAllocatedPrivateAmt);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramCreateTime);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Update(DailyPayAmtRestrictInfo oParam)
        {
            string sql = @"UPDATE Fin_DailyPayAmtRestrict SET 
                            TopPublicPayAmt=@TopPublicPayAmt, AllocatedPublicAmt=@AllocatedPublicAmt, 
                            TopPrivatePayAmt=@TopPrivatePayAmt, AllocatedPrivateAmt=@AllocatedPrivateAmt, 
                            CreateUserSysNo=@CreateUserSysNo, CreateTime=@CreateTime
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramTopPublicPayAmt = new SqlParameter("@TopPublicPayAmt", SqlDbType.Decimal, 9);
            SqlParameter paramAllocatedPublicAmt = new SqlParameter("@AllocatedPublicAmt", SqlDbType.Decimal, 9);
            SqlParameter paramTopPrivatePayAmt = new SqlParameter("@TopPrivatePayAmt", SqlDbType.Decimal, 9);
            SqlParameter paramAllocatedPrivateAmt = new SqlParameter("@AllocatedPrivateAmt", SqlDbType.Decimal, 9);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.TopPublicPayAmt != AppConst.DecimalNull)
                paramTopPublicPayAmt.Value = oParam.TopPublicPayAmt;
            else
                paramTopPublicPayAmt.Value = System.DBNull.Value;
            if (oParam.AllocatedPublicAmt != AppConst.DecimalNull)
                paramAllocatedPublicAmt.Value = oParam.AllocatedPublicAmt;
            else
                paramAllocatedPublicAmt.Value = System.DBNull.Value;
            if (oParam.TopPrivatePayAmt != AppConst.DecimalNull)
                paramTopPrivatePayAmt.Value = oParam.TopPrivatePayAmt;
            else
                paramTopPrivatePayAmt.Value = System.DBNull.Value;
            if (oParam.AllocatedPrivateAmt != AppConst.DecimalNull)
                paramAllocatedPrivateAmt.Value = oParam.AllocatedPrivateAmt;
            else
                paramAllocatedPrivateAmt.Value = System.DBNull.Value;
            if (oParam.CreateUserSysNo != AppConst.IntNull)
                paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
            else
                paramCreateUserSysNo.Value = System.DBNull.Value;
            if (oParam.CreateTime != AppConst.DateTimeNull)
                paramCreateTime.Value = oParam.CreateTime;
            else
                paramCreateTime.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramTopPublicPayAmt);
            cmd.Parameters.Add(paramAllocatedPublicAmt);
            cmd.Parameters.Add(paramTopPrivatePayAmt);
            cmd.Parameters.Add(paramAllocatedPrivateAmt);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramCreateTime);

            return SqlHelper.ExecuteNonQuery(cmd);
        }


    }
}
