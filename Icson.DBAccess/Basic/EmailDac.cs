using System;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects.Basic;

namespace Icson.DBAccess.Basic
{
	/// <summary>
	/// Summary description for EmailDac.
	/// </summary>
	public class EmailDac
	{
		

		public EmailDac()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public int InsertEmail(EmailInfo oParam)
		{
			string sql = @"INSERT INTO AsyncEmail
                           (
                           MailAddress, MailSubject, MailBody, 
                           Status
                           )
                           VALUES (
                           @MailAddress, @MailSubject, @MailBody, 
                           @Status
                           )";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramMailAddress = new SqlParameter("@MailAddress", SqlDbType.NVarChar,200);
			SqlParameter paramMailSubject = new SqlParameter("@MailSubject", SqlDbType.NVarChar,500);
			SqlParameter paramMailBody = new SqlParameter("@MailBody", SqlDbType.Text,0);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);

			if ( oParam.MailAddress != AppConst.StringNull)
				paramMailAddress.Value = oParam.MailAddress;
			else
				paramMailAddress.Value = System.DBNull.Value;
			if ( oParam.MailSubject != AppConst.StringNull)
				paramMailSubject.Value = oParam.MailSubject;
			else
				paramMailSubject.Value = System.DBNull.Value;
			if ( oParam.MailBody != AppConst.StringNull)
				paramMailBody.Value = oParam.MailBody;
			else
				paramMailBody.Value = System.DBNull.Value;
			if ( oParam.Status != AppConst.IntNull)
				paramStatus.Value = oParam.Status;
			else
				paramStatus.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramMailAddress);
			cmd.Parameters.Add(paramMailSubject);
			cmd.Parameters.Add(paramMailBody);
			cmd.Parameters.Add(paramStatus);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public int UpdateEmailStatus(EmailInfo oParam)
		{
			string sql = @"UPDATE AsyncEmail SET 
                           Status=@Status
                           WHERE SysNo=@SysNo";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);

			if ( oParam.SysNo != AppConst.IntNull)
				paramSysNo.Value = oParam.SysNo;
			else
				paramSysNo.Value = System.DBNull.Value;
			if ( oParam.Status != AppConst.IntNull)
				paramStatus.Value = oParam.Status;
			else
				paramStatus.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramStatus);

			return SqlHelper.ExecuteNonQuery(cmd);
		}
	}
}
