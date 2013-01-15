using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Icson.Utils;
using Icson.Objects;
using Icson.Objects.Basic;

namespace Icson.DBAccess.Basic
{
    public class DeliveryManDepositDac
    {
        public int Insert(DeliveryManDepositInfo oParam)
        {
            string sql = @"INSERT INTO DeliveryMan_Deposit
                            (
                            UserSysNo, Deposit, Arrearage, PayDate, 
                            IsAllow, CreateUserSysNo, CreateTime, UpdateUserSysNo, 
                            UpdateTime
                            )
                            VALUES (
                            @UserSysNo, @Deposit, @Arrearage, @PayDate, 
                            @IsAllow, @CreateUserSysNo, @CreateTime, @UpdateUserSysNo, 
                            @UpdateTime
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramUserSysNo = new SqlParameter("@UserSysNo", SqlDbType.Int, 4);
            SqlParameter paramDeposit = new SqlParameter("@Deposit", SqlDbType.Decimal, 9);
            SqlParameter paramArrearage = new SqlParameter("@Arrearage", SqlDbType.Decimal, 9);
            SqlParameter paramPayDate = new SqlParameter("@PayDate", SqlDbType.DateTime);
            SqlParameter paramIsAllow = new SqlParameter("@IsAllow", SqlDbType.Int, 4);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
            SqlParameter paramUpdateUserSysNo = new SqlParameter("@UpdateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramUpdateTime = new SqlParameter("@UpdateTime", SqlDbType.DateTime);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.UserSysNo != AppConst.IntNull)
                paramUserSysNo.Value = oParam.UserSysNo;
            else
                paramUserSysNo.Value = System.DBNull.Value;
            if (oParam.Deposit != AppConst.DecimalNull)
                paramDeposit.Value = oParam.Deposit;
            else
                paramDeposit.Value = System.DBNull.Value;
            if (oParam.Arrearage != AppConst.DecimalNull)
                paramArrearage.Value = oParam.Arrearage;
            else
                paramArrearage.Value = System.DBNull.Value;
            if (oParam.PayDate != AppConst.DateTimeNull)
                paramPayDate.Value = oParam.PayDate;
            else
                paramPayDate.Value = System.DBNull.Value;
            if (oParam.IsAllow != AppConst.IntNull)
                paramIsAllow.Value = oParam.IsAllow;
            else
                paramIsAllow.Value = System.DBNull.Value;
            if (oParam.CreateUserSysNo != AppConst.IntNull)
                paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
            else
                paramCreateUserSysNo.Value = System.DBNull.Value;
            if (oParam.CreateTime != AppConst.DateTimeNull)
                paramCreateTime.Value = oParam.CreateTime;
            else
                paramCreateTime.Value = System.DBNull.Value;
            if (oParam.UpdateUserSysNo != AppConst.IntNull)
                paramUpdateUserSysNo.Value = oParam.UpdateUserSysNo;
            else
                paramUpdateUserSysNo.Value = System.DBNull.Value;
            if (oParam.UpdateTime != AppConst.DateTimeNull)
                paramUpdateTime.Value = oParam.UpdateTime;
            else
                paramUpdateTime.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramUserSysNo);
            cmd.Parameters.Add(paramDeposit);
            cmd.Parameters.Add(paramArrearage);
            cmd.Parameters.Add(paramPayDate);
            cmd.Parameters.Add(paramIsAllow);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramCreateTime);
            cmd.Parameters.Add(paramUpdateUserSysNo);
            cmd.Parameters.Add(paramUpdateTime);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }
        public int Update(DeliveryManDepositInfo oParam)
        {
            string sql = @"UPDATE DeliveryMan_Deposit SET 
                            UserSysNo=@UserSysNo, Deposit=@Deposit, 
                            Arrearage=@Arrearage, PayDate=@PayDate, 
                            IsAllow=@IsAllow, CreateUserSysNo=@CreateUserSysNo, 
                            CreateTime=@CreateTime, UpdateUserSysNo=@UpdateUserSysNo, 
                            UpdateTime=@UpdateTime
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramUserSysNo = new SqlParameter("@UserSysNo", SqlDbType.Int, 4);
            SqlParameter paramDeposit = new SqlParameter("@Deposit", SqlDbType.Decimal, 9);
            SqlParameter paramArrearage = new SqlParameter("@Arrearage", SqlDbType.Decimal, 9);
            SqlParameter paramPayDate = new SqlParameter("@PayDate", SqlDbType.DateTime);
            SqlParameter paramIsAllow = new SqlParameter("@IsAllow", SqlDbType.Int, 4);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
            SqlParameter paramUpdateUserSysNo = new SqlParameter("@UpdateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramUpdateTime = new SqlParameter("@UpdateTime", SqlDbType.DateTime);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.UserSysNo != AppConst.IntNull)
                paramUserSysNo.Value = oParam.UserSysNo;
            else
                paramUserSysNo.Value = System.DBNull.Value;
            if (oParam.Deposit != AppConst.DecimalNull)
                paramDeposit.Value = oParam.Deposit;
            else
                paramDeposit.Value = System.DBNull.Value;
            if (oParam.Arrearage != AppConst.DecimalNull)
                paramArrearage.Value = oParam.Arrearage;
            else
                paramArrearage.Value = System.DBNull.Value;
            if (oParam.PayDate != AppConst.DateTimeNull)
                paramPayDate.Value = oParam.PayDate;
            else
                paramPayDate.Value = System.DBNull.Value;
            if (oParam.IsAllow != AppConst.IntNull)
                paramIsAllow.Value = oParam.IsAllow;
            else
                paramIsAllow.Value = System.DBNull.Value;
            if (oParam.CreateUserSysNo != AppConst.IntNull)
                paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
            else
                paramCreateUserSysNo.Value = System.DBNull.Value;
            if (oParam.CreateTime != AppConst.DateTimeNull)
                paramCreateTime.Value = oParam.CreateTime;
            else
                paramCreateTime.Value = System.DBNull.Value;
            if (oParam.UpdateUserSysNo != AppConst.IntNull)
                paramUpdateUserSysNo.Value = oParam.UpdateUserSysNo;
            else
                paramUpdateUserSysNo.Value = System.DBNull.Value;
            if (oParam.UpdateTime != AppConst.DateTimeNull)
                paramUpdateTime.Value = oParam.UpdateTime;
            else
                paramUpdateTime.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramUserSysNo);
            cmd.Parameters.Add(paramDeposit);
            cmd.Parameters.Add(paramArrearage);
            cmd.Parameters.Add(paramPayDate);
            cmd.Parameters.Add(paramIsAllow);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramCreateTime);
            cmd.Parameters.Add(paramUpdateUserSysNo);
            cmd.Parameters.Add(paramUpdateTime);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
    }
}
