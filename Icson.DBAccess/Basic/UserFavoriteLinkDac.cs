using System;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects.Basic;

namespace Icson.DBAccess.Basic
{
	/// <summary>
	/// Summary description for UserFavoriteLinkDac.
	/// </summary>
	public class UserFavoriteLinkDac
	{
		
		public UserFavoriteLinkDac()
		{
		}

		public int Insert(UserFavoriteLinkInfo oParam)
		{
			string sql = @"INSERT INTO Sys_User_FavoriteLink
                            (
                            UserSysNo, LinkName, LinkAhref, 
                            CreateTime
                            )
                            VALUES (
                            @UserSysNo, @LinkName, @LinkAhref, 
                            @CreateTime
                            );set @SysNo = SCOPE_IDENTITY();";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramUserSysNo = new SqlParameter("@UserSysNo", SqlDbType.Int,4);
			SqlParameter paramLinkName = new SqlParameter("@LinkName", SqlDbType.NVarChar,100);
			SqlParameter paramLinkAhref = new SqlParameter("@LinkAhref", SqlDbType.NVarChar,500);
			SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);

			paramSysNo.Direction = ParameterDirection.Output;

			if ( oParam.UserSysNo != AppConst.IntNull)
				paramUserSysNo.Value = oParam.UserSysNo;
			else
				paramUserSysNo.Value = System.DBNull.Value;
			if ( oParam.LinkName != AppConst.StringNull)
				paramLinkName.Value = oParam.LinkName;
			else
				paramLinkName.Value = System.DBNull.Value;
			if ( oParam.LinkAhref != AppConst.StringNull)
				paramLinkAhref.Value = oParam.LinkAhref;
			else
				paramLinkAhref.Value = System.DBNull.Value;
			if ( oParam.CreateTime != AppConst.DateTimeNull)
				paramCreateTime.Value = oParam.CreateTime;
			else
				paramCreateTime.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramUserSysNo);
			cmd.Parameters.Add(paramLinkName);
			cmd.Parameters.Add(paramLinkAhref);
			cmd.Parameters.Add(paramCreateTime);

			return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);

		}

		public int Delete(int sysno)
		{
			string sql = " delete from Sys_User_FavoriteLink where sysno = " + sysno;
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);

		}

	}
}
