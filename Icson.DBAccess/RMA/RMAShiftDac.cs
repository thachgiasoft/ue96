using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Icson.Objects.RMA;
using Icson.Utils;
namespace Icson.DBAccess.RMA
{
    public class RMAShiftDac
    {
        public int Insert(RMAShiftInfo oParam)
        {
            string sql = @"INSERT INTO RMA_Shift
                            (
                            RegisterSysNo, RMAShiftType, ShiftSysNo
                            )
                            VALUES (
                            @RegisterSysNo, @RMAShiftType, @ShiftSysNo
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramRegisterSysNo = new SqlParameter("@RegisterSysNo", SqlDbType.Int, 4);
            SqlParameter paramRMAShiftType = new SqlParameter("@RMAShiftType", SqlDbType.Int, 4);
            SqlParameter paramShiftSysNo = new SqlParameter("@ShiftSysNo", SqlDbType.Int, 4);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.RegisterSysNo != AppConst.IntNull)
                paramRegisterSysNo.Value = oParam.RegisterSysNo;
            else
                paramRegisterSysNo.Value = System.DBNull.Value;
            if (oParam.RMAShiftType != AppConst.IntNull)
                paramRMAShiftType.Value = oParam.RMAShiftType;
            else
                paramRMAShiftType.Value = System.DBNull.Value;
            if (oParam.ShiftSysNo != AppConst.IntNull)
                paramShiftSysNo.Value = oParam.ShiftSysNo;
            else
                paramShiftSysNo.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramRegisterSysNo);
            cmd.Parameters.Add(paramRMAShiftType);
            cmd.Parameters.Add(paramShiftSysNo);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }
    }
}
