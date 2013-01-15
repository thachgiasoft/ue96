using System;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects.Basic;


namespace Icson.DBAccess.Basic
{
	/// <summary>
	/// Summary description for PrivilegeDac.
	/// </summary>
	public class PrivilegeDac
	{
		
		public PrivilegeDac()
		{
		}

		public int Insert(PrivilegeInfo oParam)
		{
			string sql = @"INSERT INTO Sys_Privilege
                            (
                            SysNo, PrivilegeID, PrivilegeName, Status
                            )
                            VALUES (
                            @SysNo, @PrivilegeID, @PrivilegeName, @Status
                            )";

			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramPrivilegeID = new SqlParameter("@PrivilegeID", SqlDbType.NVarChar,20);
			SqlParameter paramPrivilegeName = new SqlParameter("@PrivilegeName", SqlDbType.NVarChar,50);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);

			if ( oParam.SysNo != AppConst.IntNull)
				paramSysNo.Value = oParam.SysNo;
			else
				paramSysNo.Value = System.DBNull.Value;
			if ( oParam.PrivilegeID != AppConst.StringNull)
				paramPrivilegeID.Value = oParam.PrivilegeID;
			else
				paramPrivilegeID.Value = System.DBNull.Value;
			if ( oParam.PrivilegeName != AppConst.StringNull)
				paramPrivilegeName.Value = oParam.PrivilegeName;
			else
				paramPrivilegeName.Value = System.DBNull.Value;
			if ( oParam.Status != AppConst.IntNull)
				paramStatus.Value = oParam.Status;
			else
				paramStatus.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramPrivilegeID);
			cmd.Parameters.Add(paramPrivilegeName);
			cmd.Parameters.Add(paramStatus);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

	}
}
