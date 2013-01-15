using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Icson.Objects.Sale;
using Icson.Utils;


namespace Icson.DBAccess.Sale
{
    public class DeliveryManArrearageLogDac
    {
        public int Insert(DeliveryManArrearageLogInfo oParam)
        {
            string sql = @"INSERT INTO DeliveryMan_ArrearageLog
                            (
                            UserSysNo, DSSysNo, Arrearage, ArrearageLogType, 
                            ArrearageChange, Memo, CreateTime, CreateUserSysNo
                            )
                            VALUES (
                            @UserSysNo, @DSSysNo, @Arrearage, @ArrearageLogType, 
                            @ArrearageChange, @Memo, @CreateTime, @CreateUserSysNo
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramUserSysNo = new SqlParameter("@UserSysNo", SqlDbType.Int, 4);
            SqlParameter paramDSSysNo = new SqlParameter("@DSSysNo", SqlDbType.Int, 4);
            SqlParameter paramArrearage = new SqlParameter("@Arrearage", SqlDbType.Decimal, 9);
            SqlParameter paramArrearageLogType = new SqlParameter("@ArrearageLogType", SqlDbType.Int, 4);
            SqlParameter paramArrearageChange = new SqlParameter("@ArrearageChange", SqlDbType.Decimal, 9);
            SqlParameter paramMemo = new SqlParameter("@Memo", SqlDbType.NVarChar, 300);
            SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.UserSysNo != AppConst.IntNull)
                paramUserSysNo.Value = oParam.UserSysNo;
            else
                paramUserSysNo.Value = System.DBNull.Value;
            if (oParam.DSSysNo != AppConst.IntNull)
                paramDSSysNo.Value = oParam.DSSysNo;
            else
                paramDSSysNo.Value = System.DBNull.Value;
            if (oParam.Arrearage != AppConst.DecimalNull)
                paramArrearage.Value = oParam.Arrearage;
            else
                paramArrearage.Value = System.DBNull.Value;
            if (oParam.ArrearageLogType != AppConst.IntNull)
                paramArrearageLogType.Value = oParam.ArrearageLogType;
            else
                paramArrearageLogType.Value = System.DBNull.Value;
            if (oParam.ArrearageChange != AppConst.DecimalNull)
                paramArrearageChange.Value = oParam.ArrearageChange;
            else
                paramArrearageChange.Value = System.DBNull.Value;
            if (oParam.Memo != AppConst.StringNull)
                paramMemo.Value = oParam.Memo;
            else
                paramMemo.Value = System.DBNull.Value;
            if (oParam.CreateTime != AppConst.DateTimeNull)
                paramCreateTime.Value = oParam.CreateTime;
            else
                paramCreateTime.Value = System.DBNull.Value;
            if (oParam.CreateUserSysNo != AppConst.IntNull)
                paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
            else
                paramCreateUserSysNo.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramUserSysNo);
            cmd.Parameters.Add(paramDSSysNo);
            cmd.Parameters.Add(paramArrearage);
            cmd.Parameters.Add(paramArrearageLogType);
            cmd.Parameters.Add(paramArrearageChange);
            cmd.Parameters.Add(paramMemo);
            cmd.Parameters.Add(paramCreateTime);
            cmd.Parameters.Add(paramCreateUserSysNo);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }
        public int Update(DeliveryManArrearageLogInfo oParam)
        {
            string sql = @"UPDATE DeliveryMan_ArrearageLog SET 
                            UserSysNo=@UserSysNo, DSSysNo=@DSSysNo, 
                            Arrearage=@Arrearage, ArrearageLogType=@ArrearageLogType, 
                            ArrearageChange=@ArrearageChange, Memo=@Memo, 
                            CreateTime=@CreateTime, CreateUserSysNo=@CreateUserSysNo
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramUserSysNo = new SqlParameter("@UserSysNo", SqlDbType.Int, 4);
            SqlParameter paramDSSysNo = new SqlParameter("@DSSysNo", SqlDbType.Int, 4);
            SqlParameter paramArrearage = new SqlParameter("@Arrearage", SqlDbType.Decimal, 9);
            SqlParameter paramArrearageLogType = new SqlParameter("@ArrearageLogType", SqlDbType.Int, 4);
            SqlParameter paramArrearageChange = new SqlParameter("@ArrearageChange", SqlDbType.Decimal, 9);
            SqlParameter paramMemo = new SqlParameter("@Memo", SqlDbType.NVarChar, 300);
            SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.UserSysNo != AppConst.IntNull)
                paramUserSysNo.Value = oParam.UserSysNo;
            else
                paramUserSysNo.Value = System.DBNull.Value;
            if (oParam.DSSysNo != AppConst.IntNull)
                paramDSSysNo.Value = oParam.DSSysNo;
            else
                paramDSSysNo.Value = System.DBNull.Value;
            if (oParam.Arrearage != AppConst.DecimalNull)
                paramArrearage.Value = oParam.Arrearage;
            else
                paramArrearage.Value = System.DBNull.Value;
            if (oParam.ArrearageLogType != AppConst.IntNull)
                paramArrearageLogType.Value = oParam.ArrearageLogType;
            else
                paramArrearageLogType.Value = System.DBNull.Value;
            if (oParam.ArrearageChange != AppConst.DecimalNull)
                paramArrearageChange.Value = oParam.ArrearageChange;
            else
                paramArrearageChange.Value = System.DBNull.Value;
            if (oParam.Memo != AppConst.StringNull)
                paramMemo.Value = oParam.Memo;
            else
                paramMemo.Value = System.DBNull.Value;
            if (oParam.CreateTime != AppConst.DateTimeNull)
                paramCreateTime.Value = oParam.CreateTime;
            else
                paramCreateTime.Value = System.DBNull.Value;
            if (oParam.CreateUserSysNo != AppConst.IntNull)
                paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
            else
                paramCreateUserSysNo.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramUserSysNo);
            cmd.Parameters.Add(paramDSSysNo);
            cmd.Parameters.Add(paramArrearage);
            cmd.Parameters.Add(paramArrearageLogType);
            cmd.Parameters.Add(paramArrearageChange);
            cmd.Parameters.Add(paramMemo);
            cmd.Parameters.Add(paramCreateTime);
            cmd.Parameters.Add(paramCreateUserSysNo);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
    }
}
