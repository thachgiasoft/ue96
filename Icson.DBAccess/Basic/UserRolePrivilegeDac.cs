using System;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects.Basic;

namespace Icson.DBAccess.Basic
{
	/// <summary>
	/// Summary description for UserRolePrivilegeDac.
	/// </summary>
	public class UserRolePrivilegeDac
	{
		public UserRolePrivilegeDac()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		

		public int RolePrivilegeInsert(int roleSysNo, int privilegeSysNo)
		{
			string sql = @"INSERT INTO Sys_Role_Privilege
                            (
                            RoleSysNo, PrivilegeSysNo
                            )
                            VALUES (
                            @RoleSysNo, @PrivilegeSysNo
                            )";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramRoleSysNo = new SqlParameter("@RoleSysNo", SqlDbType.Int,4);
			SqlParameter paramPrivilegeSysNo = new SqlParameter("@PrivilegeSysNo", SqlDbType.Int,4);

			paramRoleSysNo.Value = roleSysNo;
			paramPrivilegeSysNo.Value = privilegeSysNo;

			cmd.Parameters.Add(paramRoleSysNo);
			cmd.Parameters.Add(paramPrivilegeSysNo);

			return SqlHelper.ExecuteNonQuery(cmd);
		}
		public int RolePrivilegeDelete(int rolePirvilegeSysNo)
		{
			string sql = "delete from sys_role_privilege where sysno =" + rolePirvilegeSysNo;

			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);
		}
		public int UserRoleInsert(int userSysNo, int roleSysNo)
		{
			string sql = "insert into sys_user_role(usersysno, rolesysno) values( " + userSysNo + "," + roleSysNo + ")";
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);
		}
		public int UserRoleDelete(int userRoleSysNo)
		{
			string sql = "delete from sys_user_role where sysno =" + userRoleSysNo;
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);
		}

        /// <summary>
        /// 删除某用户的所拥有的所有角色
        /// </summary>
        /// <param name="userSysNo"></param>
        /// <returns></returns>
        public int TheUserRoleDelete(int userSysNo)
        {
            string sql = "delete from sys_user_role where usersysno =" + userSysNo.ToString();
            SqlCommand cmd = new SqlCommand(sql);
            return SqlHelper.ExecuteNonQuery(cmd);
        }
	}
}
