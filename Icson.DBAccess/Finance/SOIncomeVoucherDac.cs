using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Text;

using Icson.Utils;
using Icson.Objects.Finance;

namespace Icson.DBAccess.Finance
{
    public class SOIncomeVoucherDac
    {
        public SOIncomeVoucherDac()
        { }

        public int Insert(SOIncomeVoucherInfo oParam)
        {
            string sql = @"INSERT INTO Finance_SoIncome_Voucher
                            (
                            FSISysNo, VoucherID, VoucherTime, SysUserSysNo, 
                            DateStamp
                            )
                            VALUES (
                            @FSISysNo, @VoucherID, @VoucherTime, @SysUserSysNo, 
                            @DateStamp
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramFSISysNo = new SqlParameter("@FSISysNo", SqlDbType.Int, 4);
            SqlParameter paramVoucherID = new SqlParameter("@VoucherID", SqlDbType.NVarChar, 50);
            SqlParameter paramVoucherTime = new SqlParameter("@VoucherTime", SqlDbType.DateTime);
            SqlParameter paramSysUserSysNo = new SqlParameter("@SysUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramDateStamp = new SqlParameter("@DateStamp", SqlDbType.DateTime);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.FSISysNo != AppConst.IntNull)
                paramFSISysNo.Value = oParam.FSISysNo;
            else
                paramFSISysNo.Value = System.DBNull.Value;
            if (oParam.VoucherID != AppConst.StringNull)
                paramVoucherID.Value = oParam.VoucherID;
            else
                paramVoucherID.Value = System.DBNull.Value;
            if (oParam.VoucherTime != AppConst.DateTimeNull)
                paramVoucherTime.Value = oParam.VoucherTime;
            else
                paramVoucherTime.Value = System.DBNull.Value;
            if (oParam.SysUserSysNo != AppConst.IntNull)
                paramSysUserSysNo.Value = oParam.SysUserSysNo;
            else
                paramSysUserSysNo.Value = System.DBNull.Value;
            if (oParam.DateStamp != AppConst.DateTimeNull)
                paramDateStamp.Value = oParam.DateStamp;
            else
                paramDateStamp.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramFSISysNo);
            cmd.Parameters.Add(paramVoucherID);
            cmd.Parameters.Add(paramVoucherTime);
            cmd.Parameters.Add(paramSysUserSysNo);
            cmd.Parameters.Add(paramDateStamp);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }


        public int Update(SOIncomeVoucherInfo oParam)
        {
            string sql = @"UPDATE Finance_SoIncome_Voucher SET 
                            FSISysNo=@FSISysNo, VoucherID=@VoucherID, 
                            VoucherTime=@VoucherTime, SysUserSysNo=@SysUserSysNo, 
                            DateStamp=@DateStamp
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramFSISysNo = new SqlParameter("@FSISysNo", SqlDbType.Int, 4);
            SqlParameter paramVoucherID = new SqlParameter("@VoucherID", SqlDbType.NVarChar, 50);
            SqlParameter paramVoucherTime = new SqlParameter("@VoucherTime", SqlDbType.DateTime);
            SqlParameter paramSysUserSysNo = new SqlParameter("@SysUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramDateStamp = new SqlParameter("@DateStamp", SqlDbType.DateTime);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.FSISysNo != AppConst.IntNull)
                paramFSISysNo.Value = oParam.FSISysNo;
            else
                paramFSISysNo.Value = System.DBNull.Value;
            if (oParam.VoucherID != AppConst.StringNull)
                paramVoucherID.Value = oParam.VoucherID;
            else
                paramVoucherID.Value = System.DBNull.Value;
            if (oParam.VoucherTime != AppConst.DateTimeNull)
                paramVoucherTime.Value = oParam.VoucherTime;
            else
                paramVoucherTime.Value = System.DBNull.Value;
            if (oParam.SysUserSysNo != AppConst.IntNull)
                paramSysUserSysNo.Value = oParam.SysUserSysNo;
            else
                paramSysUserSysNo.Value = System.DBNull.Value;
            if (oParam.DateStamp != AppConst.DateTimeNull)
                paramDateStamp.Value = oParam.DateStamp;
            else
                paramDateStamp.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramFSISysNo);
            cmd.Parameters.Add(paramVoucherID);
            cmd.Parameters.Add(paramVoucherTime);
            cmd.Parameters.Add(paramSysUserSysNo);
            cmd.Parameters.Add(paramDateStamp);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int Delete(int FSISysNo)
        {
            string sql = "delete from finance_soincome_voucher where fsisysno = " + FSISysNo;
            SqlCommand cmd = new SqlCommand(sql);
            return SqlHelper.ExecuteNonQuery(cmd);
        }
    }
}
