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
    public class ROVoucherDac
    {
        public ROVoucherDac()
        { }

        public int Insert(ROVoucherInfo oParam)
        {
            string sql = @"INSERT INTO RO_Voucher
                            (
                            ROSysNo, VoucherID, Status
                            )
                            VALUES (
                            @ROSysNo, @VoucherID, @Status
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramROSysNo = new SqlParameter("@ROSysNo", SqlDbType.Int, 4);
            SqlParameter paramVoucherID = new SqlParameter("@VoucherID", SqlDbType.NVarChar, 50);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.ROSysNo != AppConst.IntNull)
                paramROSysNo.Value = oParam.ROSysNo;
            else
                paramROSysNo.Value = System.DBNull.Value;
            if (oParam.VoucherID != AppConst.StringNull)
                paramVoucherID.Value = oParam.VoucherID;
            else
                paramVoucherID.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramROSysNo);
            cmd.Parameters.Add(paramVoucherID);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Update(ROVoucherInfo oParam)
        {
            string sql = @"UPDATE RO_Voucher SET 
                            ROSysNo=@ROSysNo, VoucherID=@VoucherID, 
                            Status=@Status
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramROSysNo = new SqlParameter("@ROSysNo", SqlDbType.Int, 4);
            SqlParameter paramVoucherID = new SqlParameter("@VoucherID", SqlDbType.NVarChar, 50);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.ROSysNo != AppConst.IntNull)
                paramROSysNo.Value = oParam.ROSysNo;
            else
                paramROSysNo.Value = System.DBNull.Value;
            if (oParam.VoucherID != AppConst.StringNull)
                paramVoucherID.Value = oParam.VoucherID;
            else
                paramVoucherID.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramROSysNo);
            cmd.Parameters.Add(paramVoucherID);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
    }
}
