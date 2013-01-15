using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects.Online;

namespace Icson.DBAccess.Online
{
    public class OnlineListQADac
    {
        public OnlineListQADac()
        { }

        public int Insert(OnlineListQAInfo oParam)
        {
            string sql = @"INSERT INTO OnlineListQA
                            (
                            OnlineAreaType, CategorySysNo, QAType, QASysNo, CreateUserSysNo, 
                            CreateTime, ListOrder
                            )
                            VALUES (
                            @OnlineAreaType, @CategorySysNo, @QAType, @QASysNo, @CreateUserSysNo, 
                            @CreateTime, @ListOrder
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramOnlineAreaType = new SqlParameter("@OnlineAreaType", SqlDbType.Int, 4);
            SqlParameter paramCategorySysNo = new SqlParameter("@CategorySysNo", SqlDbType.Int, 4);
            SqlParameter paramQAType = new SqlParameter("@QAType",SqlDbType.Int,4);
            SqlParameter paramQASysNo = new SqlParameter("@QASysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
            SqlParameter paramListOrder = new SqlParameter("@ListOrder", SqlDbType.Int, 4);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.OnlineAreaType != AppConst.IntNull)
                paramOnlineAreaType.Value = oParam.OnlineAreaType;
            else
                paramOnlineAreaType.Value = System.DBNull.Value;
            if (oParam.CategorySysNo != AppConst.IntNull)
                paramCategorySysNo.Value = oParam.CategorySysNo;
            else
                paramCategorySysNo.Value = System.DBNull.Value;
            if (oParam.QAType != AppConst.IntNull)
                paramQAType.Value = oParam.QAType;
            else
                paramQAType.Value = System.DBNull.Value;
            if (oParam.QASysNo != AppConst.IntNull)
                paramQASysNo.Value = oParam.QASysNo;
            else
                paramQASysNo.Value = System.DBNull.Value;
            if (oParam.CreateUserSysNo != AppConst.IntNull)
                paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
            else
                paramCreateUserSysNo.Value = System.DBNull.Value;
            if (oParam.CreateTime != AppConst.DateTimeNull)
                paramCreateTime.Value = oParam.CreateTime;
            else
                paramCreateTime.Value = System.DBNull.Value;
            if (oParam.ListOrder != AppConst.IntNull)
                paramListOrder.Value = oParam.ListOrder;
            else
                paramListOrder.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramOnlineAreaType);
            cmd.Parameters.Add(paramCategorySysNo);
            cmd.Parameters.Add(paramQAType);
            cmd.Parameters.Add(paramQASysNo);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramCreateTime);
            cmd.Parameters.Add(paramListOrder);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Update(OnlineListQAInfo oParam)
        {
            string sql = @"UPDATE OnlineListQA SET 
                            OnlineAreaType=@OnlineAreaType, CategorySysNo=@CategorySysNo, 
                            QAType=@QAType, QASysNo=@QASysNo, CreateUserSysNo=@CreateUserSysNo, 
                            CreateTime=@CreateTime, ListOrder=@ListOrder
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramOnlineAreaType = new SqlParameter("@OnlineAreaType", SqlDbType.Int, 4);
            SqlParameter paramCategorySysNo = new SqlParameter("@CategorySysNo", SqlDbType.Int, 4);
            SqlParameter paramQAType = new SqlParameter("@QAType",SqlDbType.Int,4);
            SqlParameter paramQASysNo = new SqlParameter("@QASysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
            SqlParameter paramListOrder = new SqlParameter("@ListOrder", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.OnlineAreaType != AppConst.IntNull)
                paramOnlineAreaType.Value = oParam.OnlineAreaType;
            else
                paramOnlineAreaType.Value = System.DBNull.Value;
            if (oParam.CategorySysNo != AppConst.IntNull)
                paramCategorySysNo.Value = oParam.CategorySysNo;
            else
                paramCategorySysNo.Value = System.DBNull.Value;
            if (oParam.QAType != AppConst.IntNull)
                paramQAType.Value = oParam.QAType;
            else
                paramQAType.Value = System.DBNull.Value;
            if (oParam.QASysNo != AppConst.IntNull)
                paramQASysNo.Value = oParam.QASysNo;
            else
                paramQASysNo.Value = System.DBNull.Value;
            if (oParam.CreateUserSysNo != AppConst.IntNull)
                paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
            else
                paramCreateUserSysNo.Value = System.DBNull.Value;
            if (oParam.CreateTime != AppConst.DateTimeNull)
                paramCreateTime.Value = oParam.CreateTime;
            else
                paramCreateTime.Value = System.DBNull.Value;
            if (oParam.ListOrder != AppConst.IntNull)
                paramListOrder.Value = oParam.ListOrder;
            else
                paramListOrder.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramOnlineAreaType);
            cmd.Parameters.Add(paramCategorySysNo);
            cmd.Parameters.Add(paramQAType);
            cmd.Parameters.Add(paramQASysNo);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramCreateTime);
            cmd.Parameters.Add(paramListOrder);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int Delete(int sysno)
        {
            string sql = "delete from onlinelistqa where sysno = " + sysno;
            SqlCommand cmd = new SqlCommand(sql);
            return SqlHelper.ExecuteNonQuery(cmd);

        }
    }
}