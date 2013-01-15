using System;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects.Basic;

namespace Icson.DBAccess.Basic
{
	/// <summary>
	/// Summary description for SyncDac.
	/// </summary>
	public class SyncDac
	{
		

		public SyncDac()
		{
		}

		public int Insert(SyncInfo oParam)
		{
			string sql = @"INSERT INTO Sys_Sync
                            (
                            SyncType, LastVersionTime
                            )
                            VALUES (
                            @SyncType, @LastVersionTime
                            );set @SysNo = SCOPE_IDENTITY();";

			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramSyncType = new SqlParameter("@SyncType", SqlDbType.Int,4);
			SqlParameter paramLastVersionTime = new SqlParameter("@LastVersionTime", SqlDbType.DateTime);

			paramSysNo.Direction = ParameterDirection.Output;

			if ( oParam.SyncType != AppConst.IntNull)
				paramSyncType.Value = oParam.SyncType;
			else
				paramSyncType.Value = System.DBNull.Value;
			if ( oParam.LastVersionTime != AppConst.DateTimeNull)
				paramLastVersionTime.Value = oParam.LastVersionTime;
			else
				paramLastVersionTime.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramSyncType);
			cmd.Parameters.Add(paramLastVersionTime);

			return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);

		}

		public int Update(SyncInfo oParam)
		{
			string sql = @"UPDATE Sys_Sync SET 
                            LastVersionTime=@LastVersionTime
                            WHERE SyncType=@SyncType";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSyncType = new SqlParameter("@SyncType", SqlDbType.Int,4);
			SqlParameter paramLastVersionTime = new SqlParameter("@LastVersionTime", SqlDbType.DateTime);

			if ( oParam.SyncType != AppConst.IntNull)
				paramSyncType.Value = oParam.SyncType;
			else
				paramSyncType.Value = System.DBNull.Value;
			if ( oParam.LastVersionTime != AppConst.DateTimeNull)
				paramLastVersionTime.Value = oParam.LastVersionTime;
			else
				paramLastVersionTime.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSyncType);
			cmd.Parameters.Add(paramLastVersionTime);

			return SqlHelper.ExecuteNonQuery(cmd);
		}
	}
}
