using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Collections;

namespace YoeJoyHelper
{
    /// <summary>
    /// 
    /// </summary>
    public class SqlDBHelper
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private static readonly string connStr = ConfigurationManager.ConnectionStrings["ConnStr"].ToString();

        public SqlDBHelper()
        {

        }

        #region 数据库DBHelper类的实例方法

        /// <summary>
        /// 查询数据库
        /// 返回DataTable类型的结果集合
        /// </summary>
        /// <param name="sqlCmdTxt">String类型，数据库查询命令</param>
        /// <returns></returns>
        public DataTable ExecuteQuery(string sqlCmdTxt)
        {
            DataTable data = new DataTable();

            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(sqlCmdTxt, conn))
                    {
                        try
                        {
                            conn.Open();
                            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                            dataAdapter.Fill(data);
                        }
                        catch (SqlException sqlEx)
                        {
                            throw new Exception("SqlDBHelper throws SqlException for ExecuteQuery method: ", sqlEx);
                        }
                        catch (InvalidOperationException invaildOpEx)
                        {
                            throw new InvalidOperationException("SqlDBHelper throws InvalidOperationException for ExecuteQuery method: ", invaildOpEx);
                        }
                        catch (Exception commonEx)
                        {
                            throw new Exception("SqlDBHelper throws Common Exception for ExecuteQuery method: ", commonEx);
                        }
                    }
                }
            }

            return data;
        }

        /// <summary>
        /// 查询数据库
        /// 返回DataTable类型的结果集合
        /// </summary>
        /// <param name="sqlCmdTxt">String类型，数据库查询命令</param>
        /// <param name="paras">SqlParameter[]数组类型，SqlParameter类型数组</param>
        /// <returns></returns>
        public DataTable ExecuteQuery(string sqlCmdTxt, SqlParameter[] paras)
        {
            DataTable data = new DataTable();

            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand command = new SqlCommand(sqlCmdTxt, conn))
                    {
                        try
                        {
                            foreach (SqlParameter para in paras)
                            {
                                command.Parameters.Add(para);
                            }
                            conn.Open();
                            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                            dataAdapter.Fill(data);
                        }
                        catch (SqlException sqlEx)
                        {
                            throw new Exception("SqlDBHelper throws SqlException for ExecuteQuery method: ", sqlEx);
                        }
                        catch (InvalidOperationException invaildOpEx)
                        {
                            throw new InvalidOperationException("SqlDBHelper throws InvalidOperationException for ExecuteQuery method: ", invaildOpEx);
                        }
                        catch (NullReferenceException nullReferEx)
                        {
                            throw new InvalidOperationException("SqlDBHelper throws NullReferenceException for ExecuteQuery method: ", nullReferEx);
                        }
                        catch (Exception commonEx)
                        {
                            throw new Exception("SqlDBHelper throws Common Exception for ExecuteQuery method: ", commonEx);
                        }
                    }
                }
            }

            return data;
        }

        /// <summary>
        ///数据库的添加，修改，删除操作
        ///返回数据库表中受影响的行数
        ///当INSERTt命令中包含Select @@indentify时
        ///返回所插入表的记录主键
        /// </summary>
        /// <param name="sqlCmdTxt">String类型，数据库操作命令</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sqlCmdTxt)
        {
            int effectedRowsCount = 0;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand command = new SqlCommand(sqlCmdTxt, conn))
                {
                    try
                    {
                        conn.Open();
                        if (sqlCmdTxt.Contains("SELECT @@IDENTITY"))
                        {
                            effectedRowsCount = int.Parse(command.ExecuteScalar().ToString().Trim());
                        }
                        else
                        {
                            effectedRowsCount = command.ExecuteNonQuery();
                        }
                    }
                    catch (SqlException sqlEx)
                    {
                        throw new Exception("SqlDBHelper throws SqlException for ExecuteNonQuery method: ", sqlEx);
                    }
                    catch (ArgumentNullException argsNullEx)
                    {
                        throw new ArgumentNullException("SqlDBHelper throws ArgumentNullException for ExecuteNonQuery method: ", argsNullEx);
                    }
                    catch (FormatException formatEx)
                    {
                        throw new FormatException("SqlDBHelper throws FormatException for ExecuteNonQuery method: ", formatEx);
                    }
                    catch (Exception commonEx)
                    {
                        throw new Exception("SqlDBHelper throws Common Exception for ExecuteNonQuery method: ", commonEx);
                    }
                }
            }
            return effectedRowsCount;
        }

        /// <summary>
        ///数据库的添加，修改，删除操作
        ///返回数据库表中受影响的行数
        ///当INSERTt命令中包含Select @@indentify时
        ///返回所插入表的记录主键
        /// </summary>
        /// <param name="sqlCmdTxt">String类型，数据库操作命令</param>
        /// <param name="paras">SqlParameter[]数组类型，SqlParameter类型数组</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sqlCmdTxt, SqlParameter[] paras)
        {
            int effectedRowsCount = 0;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand command = new SqlCommand(sqlCmdTxt, conn))
                {
                    try
                    {

                        foreach (SqlParameter para in paras)
                        {
                            command.Parameters.Add(para);
                        }

                        conn.Open();
                        if (sqlCmdTxt.Contains("SELECT @@IDENTITY"))
                        {
                            effectedRowsCount = int.Parse(command.ExecuteScalar().ToString().Trim());
                        }
                        else
                        {
                            effectedRowsCount = command.ExecuteNonQuery();
                        }
                    }
                    catch (SqlException sqlEx)
                    {
                        throw new Exception("SqlDBHelper throws SqlException for ExecuteNonQuery method: ", sqlEx);
                    }
                    catch (ArgumentNullException argsNullEx)
                    {
                        throw new ArgumentNullException("SqlDBHelper throws ArgumentNullException for ExecuteNonQuery method: ", argsNullEx);
                    }
                    catch (FormatException formatEx)
                    {
                        throw new FormatException("SqlDBHelper throws FormatException for ExecuteNonQuery method: ", formatEx);
                    }
                    catch (NullReferenceException nullReferEx)
                    {
                        throw new NullReferenceException("SqlDBHelper throws NullReferenceException for ExecuteNonQuery method: ", nullReferEx);
                    }
                    catch (Exception commonEx)
                    {
                        throw new Exception("SqlDBHelper throws Common Exception for ExecuteNonQuery method: ", commonEx);
                    }
                }
            }
            return effectedRowsCount;
        }

        /// <summary>
        /// 数据库执行事物的方法
        /// </summary>
        /// <param name="sqlCmdTxts">String[]类型，SqlCommand类型数组</param>
        /// <returns></returns>
        public bool ExecuteTransaction(string[] sqlCmdTxts)
        {
            bool sqlTranscationResult = false;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlTransaction sqlTransaction;
                using (SqlCommand command = new SqlCommand())
                {
                    conn.Open();
                    sqlTransaction = conn.BeginTransaction();
                    command.Connection = conn;
                    command.Transaction = sqlTransaction;

                    try
                    {
                        foreach (string sqlCmdTxt in sqlCmdTxts)
                        {
                            command.CommandText = sqlCmdTxt;
                            command.ExecuteNonQuery();
                        }
                        sqlTransaction.Commit();
                        sqlTranscationResult = true;
                    }
                    catch (SqlException sqlEx)
                    {
                        sqlTransaction.Rollback();
                        throw new Exception("SqlDBHelper throws SqlException for ExecuteTransaction method: ", sqlEx);
                    }
                    catch (Exception CommonEx)
                    {
                        sqlTransaction.Rollback();
                        throw new Exception("SqlDBHelper throws Common Exception for ExecuteTransaction method: ", CommonEx);
                    }
                }
            }
            return sqlTranscationResult;
        }

        /// <summary>
        /// 数据库执行存储过程的方法
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="paras"></param>
        /// <param name="outputName"></param>
        /// <param name="outputValue"></param>
        /// <returns></returns>
        public int ExecuteStoredProcedure(string procedureName, SqlParameter[] paras, string outputName, out object outputValue)
        {
            int effectedRowsCount = 0;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand command = new SqlCommand(procedureName, conn))
                {
                    try
                    {

                        foreach (SqlParameter para in paras)
                        {
                            command.Parameters.Add(para);
                        }

                        command.CommandType = CommandType.StoredProcedure;
                        conn.Open();
                        effectedRowsCount = command.ExecuteNonQuery();
                        outputValue = command.Parameters[outputName].Value;
                    }
                    catch (SqlException sqlEx)
                    {
                        throw new Exception("SqlDBHelper throws SqlException for ExecuteNonQuery method: ", sqlEx);
                    }
                    catch (ArgumentNullException argsNullEx)
                    {
                        throw new ArgumentNullException("SqlDBHelper throws ArgumentNullException for ExecuteNonQuery method: ", argsNullEx);
                    }
                    catch (FormatException formatEx)
                    {
                        throw new FormatException("SqlDBHelper throws FormatException for ExecuteNonQuery method: ", formatEx);
                    }
                    catch (NullReferenceException nullReferEx)
                    {
                        throw new NullReferenceException("SqlDBHelper throws NullReferenceException for ExecuteNonQuery method: ", nullReferEx);
                    }
                    catch (Exception commonEx)
                    {
                        throw new Exception("SqlDBHelper throws Common Exception for ExecuteNonQuery method: ", commonEx);
                    }
                }
            }
            return effectedRowsCount;
        }

        #endregion

    }
}