using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;

using Icson.Utils;
using Icson.Objects;
using Icson.Objects.Online;

namespace Icson.DBAccess.Online
{
    public class TopicComplainDac
    {
        #region Sql Script

        private const string SQL_GET_TOPICCOMPLAIN
            = @"SELECT SysNo, TopicSysNo, Memo,CreateUserType, CreateUserSysNo, CreateDate 
				FROM Topic_Complain (NOLOCK)
				WHERE SysNo = @SysNo";

        private const string SQL_INSERT_TOPICCOMPLAIN
            = @"INSERT INTO Topic_Complain (TopicSysNo,Memo,CreateUserType,CreateUserSysNo) 
                VALUES (@TopicSysNo,@Memo,@CreateUserType,@CreateUserSysNo) SELECT @@Identity";

        private const string SQL_CHECK_EXITCOMPLAIN
            = @"SELECT count(*) FROM Topic_Complain (NOLOCK) 
                WHERE TopicSysNo = @TopicSysNo AND CreateUserSysNo = @CreateUserSysNo AND CreateUserType = @CreateUserType";

        #endregion
        protected static TopicComplainInfo Map(DataRow row)
        {
            TopicComplainInfo info = new TopicComplainInfo();

            info.SysNo = Convert.ToInt32(row["SysNo"]);
            info.TopicSysNo = Convert.ToInt32(row["TopicSysNo"]);
            info.Memo = row["Memo"].ToString();
            info.CreateUserType = (AppEnum.CreateUserType)Convert.ToInt32(row["CreateUserType"]);
            info.CreateUserSysNo = Convert.ToInt32(row["CreateUserSysNo"]);
            info.CreateDate = Convert.ToDateTime(row["CreateDate"]);

            return info;
        }
        public static int InsertTopicComplain(TopicComplainInfo info)
        {
            SqlParameter[] parms = new SqlParameter[]
                {
                     new SqlParameter("@TopicSysNo", SqlDbType.Int),
                     new SqlParameter("@Memo", SqlDbType.NVarChar),
                     new SqlParameter("@CreateUserType", SqlDbType.Int),
                     new SqlParameter("@CreateUserSysNo", SqlDbType.Int)
                };

            parms[0].Value = info.TopicSysNo;
            parms[1].Value = info.Memo;
            parms[2].Value = info.CreateUserType;
            parms[3].Value = info.CreateUserSysNo;

            UpdateTopicRemarkCount(info);

            SqlCommand cmd = new SqlCommand(SQL_INSERT_TOPICCOMPLAIN);
            cmd.Parameters.AddRange(parms);
            cmd.CommandTimeout = 180;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(cmd));
        }

        /// <summary>
        /// 检查是否已经存在当前用户得Complain
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool IsExistComplain(TopicComplainInfo info)
        {
            SqlParameter[] parms = new SqlParameter[]
                {
                   new  SqlParameter("@TopicSysNo", SqlDbType.Int),
                   new  SqlParameter("@CreateUserType", SqlDbType.Int),
                   new  SqlParameter("@CreateUserSysNo", SqlDbType.Int)
                 };

            parms[0].Value = info.TopicSysNo;
            parms[1].Value = (int)info.CreateUserType;
            parms[2].Value = info.CreateUserSysNo;

            SqlCommand cmd = new SqlCommand(SQL_CHECK_EXITCOMPLAIN);
            cmd.Parameters.AddRange(parms);
            cmd.CommandTimeout = 180;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(cmd)) > 0;
        }


        /// <summary>
        /// 更新主表中的 TotalComplainCount, 使得保持一致
        /// </summary>
        /// <param name="info"></param>
        private static void UpdateTopicRemarkCount(TopicComplainInfo info)
        {
            string sql = @"UPDATE Topic_Master 
                SET TotalComplainCount = TotalComplainCount+1 WHERE SysNo = " + info.TopicSysNo;

            SqlHelper.ExecuteScalar(sql);
        }

        public static DataSet LoadComplainByTopicSysNo(int topicSysNo)
        {
            string sql = @"select Topic_Complain.SysNo, 
									Topic_Complain.TopicSysNo, 
									Topic_Complain.Memo,
									Topic_Complain.CreateUserType,
									Topic_Complain.CreateUserSysNo, 
									Topic_Complain.CreateDate ,
									customer.customerid as CreateUserName 
							from Topic_Complain (nolock) 
								left join customer (nolock) on customer.sysno =	Topic_Complain.CreateUserSysNo	
							where TopicSysNo = " + topicSysNo;
            return SqlHelper.ExecuteDataSet(sql);
        }
    }
}