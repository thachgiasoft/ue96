using System;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects.Basic;

namespace Icson.DBAccess.Basic
{
	/// <summary>
	/// Summary description for UserDac.
	/// </summary>
	public class UserDac
	{
		public UserDac()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        public int InsertRequestAccount(RequestAccountInfo oParam)
        {
            string sql = @"INSERT INTO Sys_RequestAccount
                            (
                            Name,Account,
                            DepartMent,Phone,MobilePhone,Email,Note
                            )
                            VALUES (
                            @Name,@Account,
                            @DepartMent,@Phone,@MobilePhone,@Email,@Note
                            )";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramName = new SqlParameter("@Name", SqlDbType.VarChar, 50);
            SqlParameter paramAccount = new SqlParameter("@Account", SqlDbType.VarChar, 50);
            SqlParameter paramDepartMent = new SqlParameter("@DepartMent", SqlDbType.VarChar, 100);
            SqlParameter paramPhone = new SqlParameter("@Phone", SqlDbType.VarChar, 50);
            SqlParameter paramMobilePhone = new SqlParameter("@MobilePhone", SqlDbType.VarChar, 50);
            SqlParameter paramEmail = new SqlParameter("@Email", SqlDbType.VarChar, 50);
            SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.VarChar, 200);

            paramName.Value = oParam.Name; ;
            paramAccount.Value = oParam.Account;
            paramDepartMent.Value = oParam.DepartMent;
            if (oParam.Phone != AppConst.StringNull)
                paramPhone.Value = oParam.Phone;
            else
                paramPhone.Value = System.DBNull.Value;
            paramMobilePhone.Value = oParam.MobilePhone;
            paramEmail.Value = oParam.Email;

            if (oParam.Note != AppConst.StringNull)
                paramNote.Value = oParam.Note;
            else
                paramNote.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramName);
            cmd.Parameters.Add(paramAccount);
            cmd.Parameters.Add(paramDepartMent);
            cmd.Parameters.Add(paramPhone);
            cmd.Parameters.Add(paramMobilePhone);
            cmd.Parameters.Add(paramEmail);
            cmd.Parameters.Add(paramNote);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        //删除用户账号申请
        public int DeleteRequestAccount(int requestSysNo)
        {
            string sql = "delete from Sys_RequestAccount where SysNo="+requestSysNo.ToString();
            return SqlHelper.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 标记已处理
        /// </summary>
        /// <param name="paramSysNo"></param>
        /// <returns></returns>
        public int MarkDispose(int paramSysNo)
        {
            string sql = "UPDATE Sys_RequestAccount SET Status=1 WHERE SysNo=" + paramSysNo;
            return SqlHelper.ExecuteNonQuery(sql);
        }

		public int Insert(UserInfo oParam)
		{
			string sql = @"INSERT INTO Sys_User
                            (
                            SysNo,UserID,UserName,
                            Pwd,Email,Phone,Note,
                            Status,DepartmentSysNo,MobilePhone,Flag,PMGroupSysNo,StationSysNo
                            )
                            VALUES (
                            @SysNo,@UserID,@UserName,
                            @Pwd,@Email,@Phone,@Note,
                            @Status,@DepartmentSysNo,@MobilePhone,@Flag,@PMGroupSysNo,@StationSysNo
                            )";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramUserID = new SqlParameter("@UserID", SqlDbType.NVarChar,20);
			SqlParameter paramUserName = new SqlParameter("@UserName", SqlDbType.NVarChar,20);
			SqlParameter paramPwd = new SqlParameter("@Pwd", SqlDbType.NVarChar,50);
			SqlParameter paramEmail = new SqlParameter("@Email", SqlDbType.NVarChar,50);
			SqlParameter paramPhone = new SqlParameter("@Phone", SqlDbType.NVarChar,100);
			SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.NVarChar,200);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);
            SqlParameter paramDepartmentSysNo = new SqlParameter("@DepartmentSysNo", SqlDbType.Int, 4);
            SqlParameter paramMobilePhone = new SqlParameter("@MobilePhone", SqlDbType.VarChar, 30);
            SqlParameter paramFlag = new SqlParameter("@Flag", SqlDbType.Int, 4);
            SqlParameter paramPMGroupSysNo = new SqlParameter("@PMGroupSysNo", SqlDbType.Int, 4);
            SqlParameter paramStationSysNo = new SqlParameter("@StationSysNo", SqlDbType.Int, 4);

			paramSysNo.Value = oParam.SysNo;
			paramUserID.Value = oParam.UserID;
			paramUserName.Value = oParam.UserName;
			paramPwd.Value = oParam.Pwd;
			paramEmail.Value = oParam.Email;
            paramDepartmentSysNo.Value = oParam.DepartmentSysNo;
            paramMobilePhone.Value = oParam.MobilePhone;
			if ( oParam.Phone != AppConst.StringNull)
				paramPhone.Value = oParam.Phone;
			else
				paramPhone.Value = System.DBNull.Value;
			if ( oParam.Note != AppConst.StringNull )
				paramNote.Value = oParam.Note;
			else
				paramNote.Value = System.DBNull.Value;
			paramStatus.Value = oParam.Status;
            if (oParam.Flag != AppConst.IntNull)
                paramFlag.Value = oParam.Flag;
            else
                paramFlag.Value = System.DBNull.Value;
            if (oParam.PMGroupSysNo != AppConst.IntNull)
                paramPMGroupSysNo.Value = oParam.PMGroupSysNo;
            else
                paramPMGroupSysNo.Value = System.DBNull.Value;
            if (oParam.StationSysNo != AppConst.IntNull)
                paramStationSysNo.Value = oParam.StationSysNo;
            else
                paramStationSysNo.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramUserID);
			cmd.Parameters.Add(paramUserName);
			cmd.Parameters.Add(paramPwd);
			cmd.Parameters.Add(paramEmail);
			cmd.Parameters.Add(paramPhone);
			cmd.Parameters.Add(paramNote);
			cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramDepartmentSysNo);
            cmd.Parameters.Add(paramMobilePhone);
            cmd.Parameters.Add(paramFlag);
            cmd.Parameters.Add(paramPMGroupSysNo);
            cmd.Parameters.Add(paramStationSysNo);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public int Update(UserInfo oParam)
		{

			string sql = @"UPDATE Sys_User SET 
                            UserID=@UserID, 
                            UserName=@UserName, 
                            Pwd=@Pwd, Email=@Email, 
                            Phone=@Phone, Note=@Note,
                            Status=@Status,DepartmentSysNo=@DepartmentSysNo,MobilePhone=@MobilePhone,Flag=@Flag,PMGroupSysNo=@PMGroupSysNo,StationSysNo=@StationSysNO
                            WHERE SysNo=@SysNo";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramUserID = new SqlParameter("@UserID", SqlDbType.NVarChar,20);
			SqlParameter paramUserName = new SqlParameter("@UserName", SqlDbType.NVarChar,20);
			SqlParameter paramPwd = new SqlParameter("@Pwd", SqlDbType.NVarChar,50);
			SqlParameter paramEmail = new SqlParameter("@Email", SqlDbType.NVarChar,50);
			SqlParameter paramPhone = new SqlParameter("@Phone", SqlDbType.NVarChar,100);
			SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.NVarChar,200);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);
            SqlParameter paramDepartmentSysNo = new SqlParameter("@DepartmentSysNo", SqlDbType.Int, 4);
            SqlParameter paramMobilePhone = new SqlParameter("@MobilePhone", SqlDbType.VarChar, 30);
            SqlParameter paramFlag = new SqlParameter("@Flag", SqlDbType.Int, 4);
            SqlParameter paramPMGroupSysNo = new SqlParameter("@PMGroupSysNo", SqlDbType.Int, 4);
            SqlParameter paramStationSysNo = new SqlParameter("@StationSysNo", SqlDbType.Int, 4);

			paramSysNo.Value = oParam.SysNo;
			paramUserID.Value = oParam.UserID;
			paramUserName.Value = oParam.UserName;
			paramPwd.Value = oParam.Pwd;
			paramEmail.Value = oParam.Email;
			paramPhone.Value = oParam.Phone;
			paramNote.Value = oParam.Note;
			paramStatus.Value = oParam.Status;
            paramDepartmentSysNo.Value = oParam.DepartmentSysNo;
            paramMobilePhone.Value = oParam.MobilePhone;
            paramFlag.Value = oParam.Flag;
            paramPMGroupSysNo.Value = oParam.PMGroupSysNo;
            paramStationSysNo.Value = oParam.StationSysNo;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramUserID);
			cmd.Parameters.Add(paramUserName);
			cmd.Parameters.Add(paramPwd);
			cmd.Parameters.Add(paramEmail);
			cmd.Parameters.Add(paramPhone);
			cmd.Parameters.Add(paramNote);
			cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramDepartmentSysNo);
            cmd.Parameters.Add(paramMobilePhone);
            cmd.Parameters.Add(paramFlag);
            cmd.Parameters.Add(paramPMGroupSysNo);
            cmd.Parameters.Add(paramStationSysNo);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

        public int DeleteUser(int userSysNo)
        {
            string sql = "delete from sln_master where SysUserSysNo="+userSysNo.ToString();
            SqlHelper.ExecuteNonQuery(sql);

            sql = "delete from Sys_User_Role where UserSysNo=" + userSysNo.ToString();
            SqlHelper.ExecuteNonQuery(sql);

            sql = "delete from Sys_User where SysNo=" + userSysNo.ToString();
            return SqlHelper.ExecuteNonQuery(sql);
        }
	}
}
