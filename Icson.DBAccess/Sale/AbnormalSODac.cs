using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Icson.Objects.Sale;
using Icson.Utils;

namespace Icson.DBAccess.Sale
{
    public class AbnormalSODac
    {
        public int Insert(AbnormalSOInfo oParam)
        {
            string sql = @"INSERT INTO AbnormalSO
                            (
                            SOSysNo, Description, Status, Type, 
                            Createtime, CreateUserSysNo, UpdateTime, UpdateUserSysNo, 
                            CloseTime, CloseUserSysNo
                            )
                            VALUES (
                            @SOSysNo, @Description, @Status, @Type, 
                            @Createtime, @CreateUserSysNo, @UpdateTime, @UpdateUserSysNo, 
                            @CloseTime, @CloseUserSysNo
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int, 4);
            SqlParameter paramDescription = new SqlParameter("@Description", SqlDbType.NVarChar, 1000);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramType = new SqlParameter("@Type", SqlDbType.Int, 4);
            SqlParameter paramCreatetime = new SqlParameter("@Createtime", SqlDbType.DateTime);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramUpdateTime = new SqlParameter("@UpdateTime", SqlDbType.DateTime);
            SqlParameter paramUpdateUserSysNo = new SqlParameter("@UpdateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCloseTime = new SqlParameter("@CloseTime", SqlDbType.DateTime);
            SqlParameter paramCloseUserSysNo = new SqlParameter("@CloseUserSysNo", SqlDbType.Int, 4);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.SOSysNo != AppConst.IntNull)
                paramSOSysNo.Value = oParam.SOSysNo;
            else
                paramSOSysNo.Value = System.DBNull.Value;
            if (oParam.Description != AppConst.StringNull)
                paramDescription.Value = oParam.Description;
            else
                paramDescription.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;
            if (oParam.Type != AppConst.IntNull)
                paramType.Value = oParam.Type;
            else
                paramType.Value = System.DBNull.Value;
            if (oParam.Createtime != AppConst.DateTimeNull)
                paramCreatetime.Value = oParam.Createtime;
            else
                paramCreatetime.Value = System.DBNull.Value;
            if (oParam.CreateUserSysNo != AppConst.IntNull)
                paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
            else
                paramCreateUserSysNo.Value = System.DBNull.Value;
            if (oParam.UpdateTime != AppConst.DateTimeNull)
                paramUpdateTime.Value = oParam.UpdateTime;
            else
                paramUpdateTime.Value = System.DBNull.Value;
            if (oParam.UpdateUserSysNo != AppConst.IntNull)
                paramUpdateUserSysNo.Value = oParam.UpdateUserSysNo;
            else
                paramUpdateUserSysNo.Value = System.DBNull.Value;
            if (oParam.CloseTime != AppConst.DateTimeNull)
                paramCloseTime.Value = oParam.CloseTime;
            else
                paramCloseTime.Value = System.DBNull.Value;
            if (oParam.CloseUserSysNo != AppConst.IntNull)
                paramCloseUserSysNo.Value = oParam.CloseUserSysNo;
            else
                paramCloseUserSysNo.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramSOSysNo);
            cmd.Parameters.Add(paramDescription);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramType);
            cmd.Parameters.Add(paramCreatetime);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramUpdateTime);
            cmd.Parameters.Add(paramUpdateUserSysNo);
            cmd.Parameters.Add(paramCloseTime);
            cmd.Parameters.Add(paramCloseUserSysNo);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }
        public int Update(AbnormalSOInfo oParam)
        {
            string sql = @"UPDATE AbnormalSO SET 
                            SOSysNo=@SOSysNo, Description=@Description, 
                            Status=@Status, Type=@Type, 
                            Createtime=@Createtime, CreateUserSysNo=@CreateUserSysNo, 
                            UpdateTime=@UpdateTime, UpdateUserSysNo=@UpdateUserSysNo, 
                            CloseTime=@CloseTime, CloseUserSysNo=@CloseUserSysNo
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int, 4);
            SqlParameter paramDescription = new SqlParameter("@Description", SqlDbType.NVarChar, 1000);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramType = new SqlParameter("@Type", SqlDbType.Int, 4);
            SqlParameter paramCreatetime = new SqlParameter("@Createtime", SqlDbType.DateTime);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramUpdateTime = new SqlParameter("@UpdateTime", SqlDbType.DateTime);
            SqlParameter paramUpdateUserSysNo = new SqlParameter("@UpdateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCloseTime = new SqlParameter("@CloseTime", SqlDbType.DateTime);
            SqlParameter paramCloseUserSysNo = new SqlParameter("@CloseUserSysNo", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.SOSysNo != AppConst.IntNull)
                paramSOSysNo.Value = oParam.SOSysNo;
            else
                paramSOSysNo.Value = System.DBNull.Value;
            if (oParam.Description != AppConst.StringNull)
                paramDescription.Value = oParam.Description;
            else
                paramDescription.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;
            if (oParam.Type != AppConst.IntNull)
                paramType.Value = oParam.Type;
            else
                paramType.Value = System.DBNull.Value;
            if (oParam.Createtime != AppConst.DateTimeNull)
                paramCreatetime.Value = oParam.Createtime;
            else
                paramCreatetime.Value = System.DBNull.Value;
            if (oParam.CreateUserSysNo != AppConst.IntNull)
                paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
            else
                paramCreateUserSysNo.Value = System.DBNull.Value;
            if (oParam.UpdateTime != AppConst.DateTimeNull)
                paramUpdateTime.Value = oParam.UpdateTime;
            else
                paramUpdateTime.Value = System.DBNull.Value;
            if (oParam.UpdateUserSysNo != AppConst.IntNull)
                paramUpdateUserSysNo.Value = oParam.UpdateUserSysNo;
            else
                paramUpdateUserSysNo.Value = System.DBNull.Value;
            if (oParam.CloseTime != AppConst.DateTimeNull)
                paramCloseTime.Value = oParam.CloseTime;
            else
                paramCloseTime.Value = System.DBNull.Value;
            if (oParam.CloseUserSysNo != AppConst.IntNull)
                paramCloseUserSysNo.Value = oParam.CloseUserSysNo;
            else
                paramCloseUserSysNo.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramSOSysNo);
            cmd.Parameters.Add(paramDescription);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramType);
            cmd.Parameters.Add(paramCreatetime);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramUpdateTime);
            cmd.Parameters.Add(paramUpdateUserSysNo);
            cmd.Parameters.Add(paramCloseTime);
            cmd.Parameters.Add(paramCloseUserSysNo);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
    }
}
