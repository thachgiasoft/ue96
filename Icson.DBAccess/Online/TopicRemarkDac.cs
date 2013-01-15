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
    public class TopicRemarkDac
    {
        #region Sql Script

        private const string SQL_GET_ALL_TOPICREMARK
            = @"SELECT SysNo,TopicSysNo, IsUseful, CreateUserType, CreateUserSysNo, CreateDate 
				FROM Topic_Remark (NOLOCK)";

        private const string SQL_GET_TOPICREMARK
            = @"SELECT SysNo,TopicSysNo, IsUseful, CreateUserType, CreateUserSysNo, CreateDate 
				FROM Topic_Remark (NOLOCK)
				WHERE SysNo = @SysNo";

        private const string SQL_INSERT_TOPICREMARK
            = @"INSERT INTO Topic_Remark (TopicSysNo,IsUseful,CreateUserType,CreateUserSysNo) 
                VALUES (@TopicSysNo,@IsUseful,@CreateUserType,@CreateUserSysNo) select @@Identity";

        private const string SQL_CHECK_EXITREMARK
            = @"SELECT count(*) FROM Topic_Remark (NOLOCK) 
                WHERE TopicSysNo = @TopicSysNo AND CreateUserSysNo = @CreateUserSysNo AND CreateUserType = @CreateUserType";
        #endregion
        protected static TopicRemarkInfo Map(DataRow row)
        {
            TopicRemarkInfo info = new TopicRemarkInfo();

            info.SysNo = Convert.ToInt32(row["SysNo"]);
            info.TopicSysNo = Convert.ToInt32(row["TopicSysNo"]);
            info.IsUseful = Convert.ToInt32(row["IsUseful"]) == 1;
            info.CreateUserType = (AppEnum.CreateUserType)Convert.ToInt32(row["CreateUserType"]);
            info.CreateUserSysNo = Convert.ToInt32(row["CreateUserSysNo"]);
            info.CreateDate = Convert.ToDateTime(row["CreateDate"]);

            return info;
        }
        public static DataTable GetTopicRemark()
        {
            return SqlHelper.ExecuteDataSet(SQL_GET_ALL_TOPICREMARK).Tables[0];
        }

        #region 设置评论是否有用

        /// <summary>
        /// 添加Remark记录
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static int InsertTopicRemark(TopicRemarkInfo info)
        {
            SqlParameter[] parms = new SqlParameter[]
                {
                   new  SqlParameter("@TopicSysNo", SqlDbType.Int),
                   new  SqlParameter("@IsUseful", SqlDbType.Int),
                   new  SqlParameter("@CreateUserType", SqlDbType.Int),
                   new  SqlParameter("@CreateUserSysNo", SqlDbType.Int)
                 };

            parms[0].Value = info.TopicSysNo;
            parms[1].Value = info.IsUseful ? 1 : 0;
            parms[2].Value = (int)info.CreateUserType;
            parms[3].Value = info.CreateUserSysNo;

            UpdateTopicRemarkCount(info);

            SqlCommand cmd = new SqlCommand(SQL_INSERT_TOPICREMARK);
            cmd.Parameters.AddRange(parms);
            cmd.CommandTimeout = 180;
            object obj = SqlHelper.ExecuteScalar(cmd);
            return Convert.ToInt32(obj);
        }
        /// <summary>
        /// 检查是否已经存在当前用户的 Remark
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool IsExistRemark(TopicRemarkInfo info)
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


            SqlCommand cmd = new SqlCommand(SQL_CHECK_EXITREMARK);
            cmd.Parameters.AddRange(parms);
            cmd.CommandTimeout = 180;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(cmd)) > 0;
        }

        /// <summary>
        /// 更新主表中的 Remark Count, 使得保持一致
        /// </summary>
        /// <param name="info"></param>
        private static void UpdateTopicRemarkCount(TopicRemarkInfo info)
        {
            string sql = @"UPDATE Topic_Master 
                SET TotalRemarkCount = TotalRemarkCount+1, TotalUsefulRemarkCount = @NEW_TotalUsefulRemarkCount 
                WHERE SysNo=" + info.TopicSysNo;

            if (info.IsUseful)
                sql = sql.Replace("@NEW_TotalUsefulRemarkCount", "TotalUsefulRemarkCount + 1");
            else
                sql = sql.Replace("@NEW_TotalUsefulRemarkCount", "TotalUsefulRemarkCount");

            SqlHelper.ExecuteScalar(sql);
        }
        #endregion
    }
}