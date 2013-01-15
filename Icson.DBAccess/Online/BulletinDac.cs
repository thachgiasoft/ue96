using System;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects.Online;

namespace Icson.DBAccess.Online
{
	/// <summary>
	/// Summary description for BulletinDac.
	/// </summary>
	public class BulletinDac
	{
		
		public BulletinDac()
		{
		}
		
		public int Insert(BulletinInfo oParam)
		{
			string sql = @"INSERT INTO Bulletin
                            (
                            Message, Status
                            )
                            VALUES (
                            @Message, @Status
                            );set @SysNo = SCOPE_IDENTITY();";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramMessage = new SqlParameter("@Message", SqlDbType.NVarChar,2000);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);

			paramSysNo.Direction = ParameterDirection.Output;

			if ( oParam.Message != AppConst.StringNull)
				paramMessage.Value = oParam.Message;
			else
				paramMessage.Value = System.DBNull.Value;
			if ( oParam.Status != AppConst.IntNull)
				paramStatus.Value = oParam.Status;
			else
				paramStatus.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramMessage);
			cmd.Parameters.Add(paramStatus);

			return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);

		}

		public int Update(BulletinInfo oParam)
		{
			//因为只有一条记录，所以update就不指定了。
			string sql = @"UPDATE Bulletin SET 
                            Message=@Message, 
                            Status=@Status";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramMessage = new SqlParameter("@Message", SqlDbType.NVarChar,2000);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);

			if ( oParam.Message != AppConst.StringNull)
				paramMessage.Value = oParam.Message;
			else
				paramMessage.Value = System.DBNull.Value;
			if ( oParam.Status != AppConst.IntNull)
				paramStatus.Value = oParam.Status;
			else
				paramStatus.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramMessage);
			cmd.Parameters.Add(paramStatus);

			return SqlHelper.ExecuteNonQuery(cmd);
		}
	}
}
