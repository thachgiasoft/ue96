using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Icson.Objects.Basic;
using Icson.Utils;

namespace Icson.DBAccess.Sale
{
    public class FreeShipFeeDac
    {
        public int Insert(CustomerFreeShipFeeLogInfo oParam)
        {
            string sql = @"INSERT INTO Customer_FreeShipFeeLog
                            (
                            CustomerSysNo, FreeShipFeeLogType, FreeShipFeeAmount, CreateTime, 
                            Memo, LogCheck
                            )
                            VALUES 
                            (
                            @CustomerSysNo, @FreeShipFeeLogType, @FreeShipFeeAmount, @CreateTime, 
                            @Memo, @LogCheck
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramCustomerSysNo = new SqlParameter("@CustomerSysNo", SqlDbType.Int, 4);
            SqlParameter paramFreeShipFeeLogType = new SqlParameter("@FreeShipFeeLogType", SqlDbType.Int, 4);
            SqlParameter paramFreeShipFeeAmount = new SqlParameter("@FreeShipFeeAmount", SqlDbType.Decimal, 9);
            SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
            SqlParameter paramMemo = new SqlParameter("@Memo", SqlDbType.NVarChar, 200);
            SqlParameter paramLogCheck = new SqlParameter("@LogCheck", SqlDbType.NVarChar, 200);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.CustomerSysNo != AppConst.IntNull)
                paramCustomerSysNo.Value = oParam.CustomerSysNo;
            else
                paramCustomerSysNo.Value = System.DBNull.Value;
            if (oParam.FreeShipFeeLogType != AppConst.IntNull)
                paramFreeShipFeeLogType.Value = oParam.FreeShipFeeLogType;
            else
                paramFreeShipFeeLogType.Value = System.DBNull.Value;
            if (oParam.FreeShipFeeAmount != AppConst.DecimalNull)
                paramFreeShipFeeAmount.Value = oParam.FreeShipFeeAmount;
            else
                paramFreeShipFeeAmount.Value = System.DBNull.Value;
            if (oParam.CreateTime != AppConst.DateTimeNull)
                paramCreateTime.Value = oParam.CreateTime;
            else
                paramCreateTime.Value = System.DBNull.Value;
            if (oParam.Memo != AppConst.StringNull)
                paramMemo.Value = oParam.Memo;
            else
                paramMemo.Value = System.DBNull.Value;
            if (oParam.LogCheck != AppConst.StringNull)
                paramLogCheck.Value = oParam.LogCheck;
            else
                paramLogCheck.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramCustomerSysNo);
            cmd.Parameters.Add(paramFreeShipFeeLogType);
            cmd.Parameters.Add(paramFreeShipFeeAmount);
            cmd.Parameters.Add(paramCreateTime);
            cmd.Parameters.Add(paramMemo);
            cmd.Parameters.Add(paramLogCheck);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Insert(CustomerCommendInfo oParam)
        {
            string sql = @"INSERT INTO Customer_Commend
                            (
                            CustomerSysNo, FriendName, CommendEmail, CommendTime, CommendStatus
                            )
                            VALUES (
                            @CustomerSysNo, @FriendName, @CommendEmail, @CommendTime, @CommendStatus
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramCustomerSysNo = new SqlParameter("@CustomerSysNo", SqlDbType.Int, 4);
            SqlParameter paramFriendName = new SqlParameter("@FriendName", SqlDbType.NVarChar, 200);
            SqlParameter paramCommendEmail = new SqlParameter("@CommendEmail", SqlDbType.NVarChar, 200);
            SqlParameter paramCommendTime = new SqlParameter("@CommendTime", SqlDbType.DateTime);
            SqlParameter paramCommendStatus = new SqlParameter("@CommendStatus", SqlDbType.Int, 4);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.CustomerSysNo != AppConst.IntNull)
                paramCustomerSysNo.Value = oParam.CustomerSysNo;
            else
                paramCustomerSysNo.Value = System.DBNull.Value;
            if (oParam.FriendName != AppConst.StringNull)
                paramFriendName.Value = oParam.FriendName;
            else
                paramFriendName.Value = System.DBNull.Value;
            if (oParam.CommendEmail != AppConst.StringNull)
                paramCommendEmail.Value = oParam.CommendEmail;
            else
                paramCommendEmail.Value = System.DBNull.Value;
            if (oParam.CommendTime != AppConst.DateTimeNull)
                paramCommendTime.Value = oParam.CommendTime;
            else
                paramCommendTime.Value = System.DBNull.Value;
            if (oParam.CommendStatus != AppConst.IntNull)
                paramCommendStatus.Value = oParam.CommendStatus;
            else
                paramCommendStatus.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramCustomerSysNo);
            cmd.Parameters.Add(paramFriendName);
            cmd.Parameters.Add(paramCommendEmail);
            cmd.Parameters.Add(paramCommendTime);
            cmd.Parameters.Add(paramCommendStatus);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Update(CustomerCommendInfo oParam)
        {
            string sql = @"UPDATE Customer_Commend SET 
                            CustomerSysNo=@CustomerSysNo, FriendName=@FriendName, 
                            CommendEmail=@CommendEmail, CommendTime=@CommendTime, 
                            CommendStatus=@CommendStatus
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramCustomerSysNo = new SqlParameter("@CustomerSysNo", SqlDbType.Int, 4);
            SqlParameter paramFriendName = new SqlParameter("@FriendName", SqlDbType.NVarChar, 200);
            SqlParameter paramCommendEmail = new SqlParameter("@CommendEmail", SqlDbType.NVarChar, 200);
            SqlParameter paramCommendTime = new SqlParameter("@CommendTime", SqlDbType.DateTime);
            SqlParameter paramCommendStatus = new SqlParameter("@CommendStatus", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.CustomerSysNo != AppConst.IntNull)
                paramCustomerSysNo.Value = oParam.CustomerSysNo;
            else
                paramCustomerSysNo.Value = System.DBNull.Value;
            if (oParam.FriendName != AppConst.StringNull)
                paramFriendName.Value = oParam.FriendName;
            else
                paramFriendName.Value = System.DBNull.Value;
            if (oParam.CommendEmail != AppConst.StringNull)
                paramCommendEmail.Value = oParam.CommendEmail;
            else
                paramCommendEmail.Value = System.DBNull.Value;
            if (oParam.CommendTime != AppConst.DateTimeNull)
                paramCommendTime.Value = oParam.CommendTime;
            else
                paramCommendTime.Value = System.DBNull.Value;
            if (oParam.CommendStatus != AppConst.IntNull)
                paramCommendStatus.Value = oParam.CommendStatus;
            else
                paramCommendStatus.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramCustomerSysNo);
            cmd.Parameters.Add(paramFriendName);
            cmd.Parameters.Add(paramCommendEmail);
            cmd.Parameters.Add(paramCommendTime);
            cmd.Parameters.Add(paramCommendStatus);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int SetFreeShipFee(int customerSysNo, decimal increment)
        {
            string sql = @"update customer set validFreeShipFee = validFreeShipFee + @increment";
            if (increment > 0)
                sql += " ,totalFreeShipFee = totalFreeShipFee + @increment";
            sql += " where sysno=@SysNo and validFreeShipFee + @increment>=0";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramIncrement = new SqlParameter("@Increment", SqlDbType.Decimal, 9);

            paramSysNo.Value = customerSysNo;
            paramIncrement.Value = increment;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramIncrement);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
    }
}
