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
	public class ImportDac
	{
		
		public ImportDac()
		{
		}
		public int Insert(ImportInfo oParam, string tableName)
		{
			string sql = "INSERT INTO ippconvert.." + tableName +
                           @"(
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
	}
}
