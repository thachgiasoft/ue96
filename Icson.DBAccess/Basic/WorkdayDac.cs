using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects;
using Icson.Objects.Basic;

namespace Icson.DBAccess.Basic
{
    public class WorkdayDac
    {
        public WorkdayDac()
        { }

        public int Insert(WorkdayInfo oParam)
        {
            string sql = @"INSERT INTO Workday
                            (
                            Name, Date, TimeSpan, Week, 
                            Status
                            )
                            VALUES (
                            @Name, @Date, @TimeSpan, @Week, 
                            @Status
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramName = new SqlParameter("@Name", SqlDbType.NVarChar, 50);
            SqlParameter paramDate = new SqlParameter("@Date", SqlDbType.DateTime);
            SqlParameter paramTimeSpan = new SqlParameter("@TimeSpan", SqlDbType.Int, 4);
            SqlParameter paramWeek = new SqlParameter("@Week", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.Name != AppConst.StringNull)
                paramName.Value = oParam.Name;
            else
                paramName.Value = System.DBNull.Value;
            if (oParam.Date != AppConst.DateTimeNull)
                paramDate.Value = oParam.Date;
            else
                paramDate.Value = System.DBNull.Value;
            if (oParam.TimeSpan != AppConst.IntNull)
                paramTimeSpan.Value = oParam.TimeSpan;
            else
                paramTimeSpan.Value = System.DBNull.Value;
            if (oParam.Week != AppConst.IntNull)
                paramWeek.Value = oParam.Week;
            else
                paramWeek.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramName);
            cmd.Parameters.Add(paramDate);
            cmd.Parameters.Add(paramTimeSpan);
            cmd.Parameters.Add(paramWeek);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Update(WorkdayInfo oParam)
        {
            string sql = @"UPDATE Workday SET 
                            Name=@Name, Date=@Date, 
                            TimeSpan=@TimeSpan, Week=@Week, 
                            Status=@Status
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramName = new SqlParameter("@Name", SqlDbType.NVarChar, 50);
            SqlParameter paramDate = new SqlParameter("@Date", SqlDbType.DateTime);
            SqlParameter paramTimeSpan = new SqlParameter("@TimeSpan", SqlDbType.Int, 4);
            SqlParameter paramWeek = new SqlParameter("@Week", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.Name != AppConst.StringNull)
                paramName.Value = oParam.Name;
            else
                paramName.Value = System.DBNull.Value;
            if (oParam.Date != AppConst.DateTimeNull)
                paramDate.Value = oParam.Date;
            else
                paramDate.Value = System.DBNull.Value;
            if (oParam.TimeSpan != AppConst.IntNull)
                paramTimeSpan.Value = oParam.TimeSpan;
            else
                paramTimeSpan.Value = System.DBNull.Value;
            if (oParam.Week != AppConst.IntNull)
                paramWeek.Value = oParam.Week;
            else
                paramWeek.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramName);
            cmd.Parameters.Add(paramDate);
            cmd.Parameters.Add(paramTimeSpan);
            cmd.Parameters.Add(paramWeek);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
    }
}
