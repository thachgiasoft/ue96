using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects;
using Icson.Objects.Online;

namespace Icson.DBAccess.Online
{
    public class QADac
    {
        public QADac()
        {}

        public int Insert(QAInfo oParam)
        {
            string sql = @"INSERT INTO QA
                            (
                            Question, Answer, SearchKey, Type, 
                            CreateUserSysNo, CreateTime, ViewCount, OrderNum, 
                            Status
                            )
                            VALUES (
                            @Question, @Answer, @SearchKey, @Type, 
                            @CreateUserSysNo, @CreateTime, @ViewCount, @OrderNum, 
                            @Status
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramQuestion = new SqlParameter("@Question", SqlDbType.NVarChar, 500);
            SqlParameter paramAnswer = new SqlParameter("@Answer", SqlDbType.Text, 0);
            SqlParameter paramSearchKey = new SqlParameter("@SearchKey", SqlDbType.NVarChar, 500);
            SqlParameter paramType = new SqlParameter("@Type", SqlDbType.Int, 4);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
            SqlParameter paramViewCount = new SqlParameter("@ViewCount", SqlDbType.Int, 4);
            SqlParameter paramOrderNum = new SqlParameter("@OrderNum", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.Question != AppConst.StringNull)
                paramQuestion.Value = oParam.Question;
            else
                paramQuestion.Value = System.DBNull.Value;
            if (oParam.Answer != AppConst.StringNull)
                paramAnswer.Value = oParam.Answer;
            else
                paramAnswer.Value = System.DBNull.Value;
            if (oParam.SearchKey != AppConst.StringNull)
                paramSearchKey.Value = oParam.SearchKey;
            else
                paramSearchKey.Value = System.DBNull.Value;
            if (oParam.Type != AppConst.IntNull)
                paramType.Value = oParam.Type;
            else
                paramType.Value = System.DBNull.Value;
            if (oParam.CreateUserSysNo != AppConst.IntNull)
                paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
            else
                paramCreateUserSysNo.Value = System.DBNull.Value;
            if (oParam.CreateTime != AppConst.DateTimeNull)
                paramCreateTime.Value = oParam.CreateTime;
            else
                paramCreateTime.Value = System.DBNull.Value;
            if (oParam.ViewCount != AppConst.IntNull)
                paramViewCount.Value = oParam.ViewCount;
            else
                paramViewCount.Value = System.DBNull.Value;
            if (oParam.OrderNum != AppConst.IntNull)
                paramOrderNum.Value = oParam.OrderNum;
            else
                paramOrderNum.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramQuestion);
            cmd.Parameters.Add(paramAnswer);
            cmd.Parameters.Add(paramSearchKey);
            cmd.Parameters.Add(paramType);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramCreateTime);
            cmd.Parameters.Add(paramViewCount);
            cmd.Parameters.Add(paramOrderNum);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Update(QAInfo oParam)
        {
            string sql = @"UPDATE QA SET 
                            Question=@Question, Answer=@Answer, 
                            SearchKey=@SearchKey, Type=@Type, 
                            CreateUserSysNo=@CreateUserSysNo, CreateTime=@CreateTime, 
                            ViewCount=@ViewCount, OrderNum=@OrderNum, 
                            Status=@Status
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramQuestion = new SqlParameter("@Question", SqlDbType.NVarChar, 500);
            SqlParameter paramAnswer = new SqlParameter("@Answer", SqlDbType.Text, 0);
            SqlParameter paramSearchKey = new SqlParameter("@SearchKey", SqlDbType.NVarChar, 500);
            SqlParameter paramType = new SqlParameter("@Type", SqlDbType.Int, 4);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
            SqlParameter paramViewCount = new SqlParameter("@ViewCount", SqlDbType.Int, 4);
            SqlParameter paramOrderNum = new SqlParameter("@OrderNum", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.Question != AppConst.StringNull)
                paramQuestion.Value = oParam.Question;
            else
                paramQuestion.Value = System.DBNull.Value;
            if (oParam.Answer != AppConst.StringNull)
                paramAnswer.Value = oParam.Answer;
            else
                paramAnswer.Value = System.DBNull.Value;
            if (oParam.SearchKey != AppConst.StringNull)
                paramSearchKey.Value = oParam.SearchKey;
            else
                paramSearchKey.Value = System.DBNull.Value;
            if (oParam.Type != AppConst.IntNull)
                paramType.Value = oParam.Type;
            else
                paramType.Value = System.DBNull.Value;
            if (oParam.CreateUserSysNo != AppConst.IntNull)
                paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
            else
                paramCreateUserSysNo.Value = System.DBNull.Value;
            if (oParam.CreateTime != AppConst.DateTimeNull)
                paramCreateTime.Value = oParam.CreateTime;
            else
                paramCreateTime.Value = System.DBNull.Value;
            if (oParam.ViewCount != AppConst.IntNull)
                paramViewCount.Value = oParam.ViewCount;
            else
                paramViewCount.Value = System.DBNull.Value;
            if (oParam.OrderNum != AppConst.IntNull)
                paramOrderNum.Value = oParam.OrderNum;
            else
                paramOrderNum.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramQuestion);
            cmd.Parameters.Add(paramAnswer);
            cmd.Parameters.Add(paramSearchKey);
            cmd.Parameters.Add(paramType);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramCreateTime);
            cmd.Parameters.Add(paramViewCount);
            cmd.Parameters.Add(paramOrderNum);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int Update(Hashtable paramHash)
        {
            StringBuilder sb = new StringBuilder(200);
            sb.Append("UPDATE QA SET ");

            if (paramHash != null && paramHash.Count != 0)
            {
                int index = 0;
                foreach (string key in paramHash.Keys)
                {
                    object item = paramHash[key];
                    if (key.ToLower() == "sysno")
                        continue;

                    if (index != 0)
                        sb.Append(",");
                    index++;

                    if (item is int || item is decimal)
                    {
                        sb.Append(key).Append("=").Append(item.ToString());
                    }
                    else if (item is string)
                    {
                        sb.Append(key).Append(" = ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (item is DateTime)
                    {
                        sb.Append(key).Append(" = cast(").Append(Util.ToSqlString(((DateTime)item).ToString(AppConst.DateFormatLong))).Append(" as DateTime)");
                    }
                }
            }

            sb.Append(" WHERE SysNo=").Append(paramHash["SysNo"]);

            return SqlHelper.ExecuteNonQuery(sb.ToString());
        }

        public int SetOrderNum(QAInfo oParam)
        {
            string sql = "update qa set ordernum = " + oParam.OrderNum + " where sysno = " + oParam.SysNo;
            SqlCommand cmd = new SqlCommand(sql);
            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int GetQANewOrderNum(QAInfo oParam)
        {
            string sql = "select isnull(max(OrderNum),0)+1 as newOrderNum from qa where type=" + oParam.Type;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                return Int32.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 1;
            }
        }
    }
}