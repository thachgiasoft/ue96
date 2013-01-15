using System;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects.Basic;

namespace Icson.DBAccess.Basic
{
	/// <summary>
	/// Summary description for SMSDac.
	/// </summary>
	public class SMSDac
	{
		public SMSDac()
		{
			//
			// TODO: Add constructor logic here
			//
		}
        
		public int Insert(SMSInfo oParam)
		{
			string sql = @"INSERT INTO SMS
                            (
                            CellNumber, SMSContent, Priority, 
                            RetryCount, CreateUserSysNo, CreateTime, ExpectSendTime,HandleTime, Status
                            )
                            VALUES (
                            @CellNumber, @SMSContent, @Priority, 
                            @RetryCount, @CreateUserSysNo, @CreateTime, @ExpectSendTime, @HandleTime, @Status
                            );set @SysNo = SCOPE_IDENTITY();";

			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramCellNumber = new SqlParameter("@CellNumber", SqlDbType.NVarChar,11);
			SqlParameter paramSMSContent = new SqlParameter("@SMSContent", SqlDbType.NVarChar,70);
			SqlParameter paramPriority = new SqlParameter("@Priority", SqlDbType.Int,4);
			SqlParameter paramRetryCount = new SqlParameter("@RetryCount", SqlDbType.Int,4);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
			SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
            SqlParameter paramExpectSendTime = new SqlParameter("@ExpectSendTime", SqlDbType.DateTime);
			SqlParameter paramHandleTime = new SqlParameter("@HandleTime", SqlDbType.DateTime);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);

			paramSysNo.Direction = ParameterDirection.Output;

			if ( oParam.CellNumber != AppConst.StringNull)
				paramCellNumber.Value = oParam.CellNumber;
			else
				paramCellNumber.Value = System.DBNull.Value;
			if ( oParam.SMSContent != AppConst.StringNull)
				paramSMSContent.Value = oParam.SMSContent;
			else
				paramSMSContent.Value = System.DBNull.Value;
			if ( oParam.Priority != AppConst.IntNull)
				paramPriority.Value = oParam.Priority;
			else
				paramPriority.Value = System.DBNull.Value;
			if ( oParam.RetryCount != AppConst.IntNull)
				paramRetryCount.Value = oParam.RetryCount;
			else
				paramRetryCount.Value = System.DBNull.Value;
            if (oParam.CreateUserSysNo != AppConst.IntNull)
                paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
            else
                paramCreateUserSysNo.Value = System.DBNull.Value;
			if ( oParam.CreateTime != AppConst.DateTimeNull)
				paramCreateTime.Value = oParam.CreateTime;
			else
				paramCreateTime.Value = System.DBNull.Value;
            if (oParam.ExpectSendTime != AppConst.DateTimeNull)
                paramExpectSendTime.Value = oParam.ExpectSendTime;
            else
                paramExpectSendTime.Value = System.DBNull.Value;
			if ( oParam.HandleTime != AppConst.DateTimeNull)
				paramHandleTime.Value = oParam.HandleTime;
			else
				paramHandleTime.Value = System.DBNull.Value;
			if ( oParam.Status != AppConst.IntNull)
				paramStatus.Value = oParam.Status;
			else
				paramStatus.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramCellNumber);
			cmd.Parameters.Add(paramSMSContent);
			cmd.Parameters.Add(paramPriority);
			cmd.Parameters.Add(paramRetryCount);
            cmd.Parameters.Add(paramCreateUserSysNo);
			cmd.Parameters.Add(paramCreateTime);
            cmd.Parameters.Add(paramExpectSendTime);
			cmd.Parameters.Add(paramHandleTime);
			cmd.Parameters.Add(paramStatus);

			return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
		}

        public int UpdateSMSStatus(SMSInfo oParam)
        {
            string sql = @"UPDATE sms SET 
                           Status=@Status,HandleTime=@HandleTime 
                           WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramHandleTime = new SqlParameter("@HandleTime", SqlDbType.DateTime);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;
            if (oParam.HandleTime != AppConst.DateTimeNull)
                paramHandleTime.Value = oParam.HandleTime;
            else
                paramHandleTime.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramHandleTime);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
	}
}