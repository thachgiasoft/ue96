using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Text;

using Icson.Utils;
using Icson.Objects.Online;

namespace Icson.DBAccess.Online
{
	/// <summary>
	/// Summary description for SubscribeDac.
	/// </summary>
	public class SubscribeDac
	{
		
		public SubscribeDac()
		{
		}

		public int Insert(SubscribeInfo oParam)
		{
			string sql = @"INSERT INTO subscribe
                            (
                            Email, UserName, CreateTime
                            )
                            VALUES (
                            @Email, @UserName, @CreateTime
                            );set @SysNo = SCOPE_IDENTITY();";

			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramEmail = new SqlParameter("@Email", SqlDbType.NVarChar,50);
			SqlParameter paramUserName = new SqlParameter("@UserName", SqlDbType.NVarChar,50);
			SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);

			paramSysNo.Direction = ParameterDirection.Output;

			if ( oParam.Email != AppConst.StringNull)
				paramEmail.Value = oParam.Email;
			else
				paramEmail.Value = System.DBNull.Value;
			if ( oParam.UserName != AppConst.StringNull)
				paramUserName.Value = oParam.UserName;
			else
				paramUserName.Value = System.DBNull.Value;
			if ( oParam.CreateTime != AppConst.DateTimeNull)
				paramCreateTime.Value = oParam.CreateTime;
			else
				paramCreateTime.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramEmail);
			cmd.Parameters.Add(paramUserName);
			cmd.Parameters.Add(paramCreateTime);

			return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);

		}

		public int Delete(string email)
		{
			string sql = "delete from subscribe where email=" + Util.ToSqlString(email);
			SqlCommand cmd = new SqlCommand(sql);

			return SqlHelper.ExecuteNonQuery(cmd);

		}
	}
}
