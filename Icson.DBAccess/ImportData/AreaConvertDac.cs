using System;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects.ImportData;

namespace Icson.DBAccess.ImportData
{
	/// <summary>
	/// Summary description for AreaConvertDac.
	/// </summary>
	public class AreaConvertDac
	{
		
		public AreaConvertDac()
		{
		}
		public int Insert(AreaConvertInfo oParam)
		{
			string sql = @"INSERT INTO ippconvert..Area
                            (
                            oldSysNo, newSysNo
                            )
                            VALUES (
                            @oldSysNo, @newSysNo
                            )";

			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramoldSysNo = new SqlParameter("@oldSysNo", SqlDbType.Int,4);
			SqlParameter paramnewSysNo = new SqlParameter("@newSysNo", SqlDbType.Int,4);

			paramoldSysNo.Value = oParam.OldSysNo;
			paramnewSysNo.Value = oParam.NewSysNo;

			cmd.Parameters.Add(paramoldSysNo);
			cmd.Parameters.Add(paramnewSysNo);

			return SqlHelper.ExecuteNonQuery(cmd);
		}
		public int Delete()
		{
			string sql = "delete from ippconvert..area";
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);
		}
	}
}
