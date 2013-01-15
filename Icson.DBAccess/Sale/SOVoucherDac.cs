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
    public class SOVoucherDac
    {
        public SOVoucherDac()
        { }

        public int Insert(SOVoucherInfo oParam)
        {
            string sql = @"INSERT INTO SO_Voucher
                            (
                            SOSysNo, VoucherID, Status
                            )
                            VALUES (
                            @SOSysNo, @VoucherID, @Status
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int, 4);
            SqlParameter paramVoucherID = new SqlParameter("@VoucherID", SqlDbType.NVarChar, 50);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.SOSysNo != AppConst.IntNull)
                paramSOSysNo.Value = oParam.SOSysNo;
            else
                paramSOSysNo.Value = System.DBNull.Value;
            if (oParam.VoucherID != AppConst.StringNull)
                paramVoucherID.Value = oParam.VoucherID;
            else
                paramVoucherID.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramSOSysNo);
            cmd.Parameters.Add(paramVoucherID);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Update(SOVoucherInfo oParam)
        {
            string sql = @"UPDATE SO_Voucher SET 
                            SOSysNo=@SOSysNo, VoucherID=@VoucherID, 
                            Status=@Status
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int, 4);
            SqlParameter paramVoucherID = new SqlParameter("@VoucherID", SqlDbType.NVarChar, 50);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.SOSysNo != AppConst.IntNull)
                paramSOSysNo.Value = oParam.SOSysNo;
            else
                paramSOSysNo.Value = System.DBNull.Value;
            if (oParam.VoucherID != AppConst.StringNull)
                paramVoucherID.Value = oParam.VoucherID;
            else
                paramVoucherID.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramSOSysNo);
            cmd.Parameters.Add(paramVoucherID);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
    }
}
