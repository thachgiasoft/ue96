using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using Icson.Utils;
using Icson.Objects.Basic;

namespace Icson.DBAccess.Basic
{
	/// <summary>
	/// Summary description for RoleDac.
	/// </summary>
	public class RoleDac
	{
		public RoleDac()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		

		public int Insert(RoleInfo oParam)
		{
			string sql = @"INSERT INTO Sys_Role
                            (
                            SysNo, RoleID, RoleName, Status
                            )
                            VALUES (
                            @SysNo, @RoleID, @RoleName, @Status
                            )";

			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramRoleID = new SqlParameter("@RoleID", SqlDbType.NVarChar,20);
			SqlParameter paramRoleName = new SqlParameter("@RoleName", SqlDbType.NVarChar,50);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);

			paramSysNo.Value = oParam.SysNo;
			paramRoleID.Value = oParam.RoleID;
			paramRoleName.Value = oParam.RoleName;
			paramStatus.Value = oParam.Status;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramRoleID);
			cmd.Parameters.Add(paramRoleName);
			cmd.Parameters.Add(paramStatus);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

        public int Insert_DBC(RoleInfo oParam)
        {
            string sql = @"INSERT INTO Sys_Role
                            (
                            SysNo, RoleID, RoleName, Status, OperationTypeID
                            )
                            VALUES (
                            @SysNo, @RoleID, @RoleName, @Status, @OperationTypeID
                            )";

            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramRoleID = new SqlParameter("@RoleID", SqlDbType.NVarChar, 20);
            SqlParameter paramRoleName = new SqlParameter("@RoleName", SqlDbType.NVarChar, 50);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramOperationTypeID = new SqlParameter("@OperationTypeID", SqlDbType.Int, 4);

            paramSysNo.Value = oParam.SysNo;
            paramRoleID.Value = oParam.RoleID;
            paramRoleName.Value = oParam.RoleName;
            paramStatus.Value = oParam.Status;
            if (oParam.OperationTypeID == -2)
                paramOperationTypeID.Value = System.DBNull.Value;
            else
                paramOperationTypeID.Value = oParam.OperationTypeID;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramRoleID);
            cmd.Parameters.Add(paramRoleName);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramOperationTypeID);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

		public int Update(RoleInfo oParam)
		{
			string sql = @"UPDATE Sys_Role SET 
                            RoleID=@RoleID, 
                            RoleName=@RoleName, Status=@Status
                            WHERE SysNo=@SysNo";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramRoleID = new SqlParameter("@RoleID", SqlDbType.NVarChar,20);
			SqlParameter paramRoleName = new SqlParameter("@RoleName", SqlDbType.NVarChar,50);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);

			paramSysNo.Value = oParam.SysNo;
			paramRoleID.Value = oParam.RoleID;
			paramRoleName.Value = oParam.RoleName;
			paramStatus.Value = oParam.Status;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramRoleID);
			cmd.Parameters.Add(paramRoleName);
			cmd.Parameters.Add(paramStatus);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

        public int Update_DBC(RoleInfo oParam)
        {
            string sql = @"UPDATE Sys_Role SET 
                            RoleID=@RoleID, 
                            RoleName=@RoleName, Status=@Status, OperationTypeID=@OperationTypeID
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramRoleID = new SqlParameter("@RoleID", SqlDbType.NVarChar, 20);
            SqlParameter paramRoleName = new SqlParameter("@RoleName", SqlDbType.NVarChar, 50);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramOperationTypeID = new SqlParameter("@OperationTypeID", SqlDbType.Int, 4);

            paramSysNo.Value = oParam.SysNo;
            paramRoleID.Value = oParam.RoleID;
            paramRoleName.Value = oParam.RoleName;
            paramStatus.Value = oParam.Status;
            if (oParam.OperationTypeID == -2)
                paramOperationTypeID.Value = System.DBNull.Value;
            else
                paramOperationTypeID.Value = oParam.OperationTypeID;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramRoleID);
            cmd.Parameters.Add(paramRoleName);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramOperationTypeID);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int DeleteRole(int SysNo)
        {
            string sql = "delete from sys_role_privilege where rolesysno=" + SysNo.ToString();
            SqlHelper.ExecuteNonQuery(sql);

            sql = "delete from sys_user_role where rolesysno=" + SysNo.ToString();
            SqlHelper.ExecuteNonQuery(sql);

            sql = "delete from sys_role where SysNo=" + SysNo.ToString();
            return SqlHelper.ExecuteNonQuery(sql);

        }

        public int DelPrivilegeRole(string strDelSql, int SysNo)
        {
            string sql = "delete from sys_role_privilege where RoleSysNo=" + SysNo;
            return SqlHelper.ExecuteNonQuery(sql);
        }

        public int InsertPrivilegeRole(int RoleSysNo, int PrivilegeSysNo)
        {
            string sql = @"INSERT INTO sys_Role_Privilege
                            (
                            RoleSysNo, PrivilegeSysNo
                            )
                            VALUES (
                            @RoleSysNo, @PrivilegeSysNo
                            )";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramRoleSysNo = new SqlParameter("@RoleSysNo", SqlDbType.Int, 4);
            SqlParameter paramPrivilegeSysNo = new SqlParameter("@PrivilegeSysNo", SqlDbType.Int, 4);


            if (RoleSysNo != AppConst.IntNull)
                paramRoleSysNo.Value = RoleSysNo;
            else
                paramRoleSysNo.Value = System.DBNull.Value;
            if (PrivilegeSysNo != AppConst.IntNull)
                paramPrivilegeSysNo.Value = PrivilegeSysNo;
            else
                paramPrivilegeSysNo.Value = System.DBNull.Value;


            cmd.Parameters.Add(paramRoleSysNo);
            cmd.Parameters.Add(paramPrivilegeSysNo);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

	}
}
