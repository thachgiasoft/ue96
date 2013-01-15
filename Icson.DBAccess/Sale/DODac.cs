using System;
using System.Data;
using System.Data.SqlClient;
using Icson.Objects.Sale;
using Icson.Utils;

namespace Icson.DBAccess.Sale
{
	/// <summary>
	/// Summary description for DODac.
	/// </summary>
	public class DODac
	{
		public DODac()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		
		
		
		public int Insert(DOInfo oParam)
		{
			string sql = @"INSERT INTO DO_master
                            (
                             SOSysNo, DONo
                            )
                            VALUES (
                             @SOSysNo, @DONo
                            )";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int,4);
			SqlParameter paramDONo = new SqlParameter("@DONo", SqlDbType.NVarChar,50);

			if ( oParam.SysNo != AppConst.IntNull)
				paramSysNo.Value = oParam.SysNo;
			else
				paramSysNo.Value = System.DBNull.Value;
			if ( oParam.SOSysNo != AppConst.IntNull)
				paramSOSysNo.Value = oParam.SOSysNo;
			else
				paramSOSysNo.Value = System.DBNull.Value;
			if ( oParam.DONo != AppConst.StringNull)
				paramDONo.Value = oParam.DONo;
			else
				paramDONo.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramSOSysNo);
			cmd.Parameters.Add(paramDONo);

			return SqlHelper.ExecuteNonQuery(cmd);
		}
		
		public int Update(DOInfo oParam)
		{
			string sql = @"UPDATE DO_master SET 
                            DONo=@DONo
                            WHERE SysNo=@SysNo";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramDONo = new SqlParameter("@DONo", SqlDbType.NVarChar,50);

			if ( oParam.SysNo != AppConst.IntNull)
				paramSysNo.Value = oParam.SysNo;
			else
				paramSysNo.Value = System.DBNull.Value;
			if ( oParam.DONo != AppConst.StringNull)
				paramDONo.Value = oParam.DONo;
			else
				paramDONo.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramDONo);

			return SqlHelper.ExecuteNonQuery(cmd);
		}
	}
}
