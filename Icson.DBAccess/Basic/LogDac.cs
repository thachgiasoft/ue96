using System;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects.Basic;


namespace Icson.DBAccess.Basic
{
	/// <summary>
	/// Summary description for LogDac.
	/// </summary>
	public class LogDac
	{
		

		public LogDac()
		{
		}
		public int Insert(LogInfo oParam)
		{
			string sql = @"INSERT INTO Sys_Log
						(
						OptTime,OptUserSysNo,OptIP,
						TicketType,TicketSysno,Note
						)
						VALUES (
						GetDate(),@OptUserSysNo,@OptIP,
						@TicketType,@TicketSysno,@Note
						)";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramOptUserSysNo = new SqlParameter("@OptUserSysNo", SqlDbType.Int,4);
			SqlParameter paramOptIP = new SqlParameter("@OptIP", SqlDbType.NVarChar,20);
			SqlParameter paramTicketType = new SqlParameter("@TicketType", SqlDbType.Int,4);
			SqlParameter paramTicketSysno = new SqlParameter("@TicketSysno", SqlDbType.Int,4);
			SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.NVarChar,500);

			paramOptUserSysNo.Value = oParam.OptUserSysNo;
			paramOptIP.Value = oParam.OptIP;
			paramTicketType.Value = oParam.TicketType;
			paramTicketSysno.Value = oParam.TicketSysNo;
			if ( oParam.Note != AppConst.StringNull )
				paramNote.Value = oParam.Note;
			else
				paramNote.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramOptUserSysNo);
			cmd.Parameters.Add(paramOptIP);
			cmd.Parameters.Add(paramTicketType);
			cmd.Parameters.Add(paramTicketSysno);
			cmd.Parameters.Add(paramNote);

			return SqlHelper.ExecuteNonQuery(cmd);
		}
	}
}
