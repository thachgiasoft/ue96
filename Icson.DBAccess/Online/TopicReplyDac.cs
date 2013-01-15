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
    public class TopicReplyDac
    {
        #region Sql Script

        private const string SQL_GET_TOPICREPLY_BY_TOPICID
            = @"@select Topic_Reply.SysNo as SysNo, Topic_Reply.TopicSysNo as TopicSysNo, Topic_Reply.ReplyContent as ReplyContent,
                   Topic_Reply.Status as Status, Topic_Reply.CreateUserType CreateUserType, Topic_Reply.CreateUserSysNo as CreateUserSysNo,
                   Topic_Reply.CreateDate as CreateDate, Topic_Reply.LastEditUserSysNo as LastEditUserSysNo, Topic_Reply.LastEditDate as LastEditDate,
                   Sys_User.UserName as UpdateUserName,
		           CustomerName = CASE CreateUserType WHEN 0 THEN (SELECT NickName FROM Customer (NOLOCK) WHERE SysNo = Topic_Reply.CreateUserSysNo) WHEN 1 THEN (SELECT UserName FROM Sys_User (NOLOCK) WHERE SysNo = Topic_Reply.CreateUserSysNo) END,
                   CustomerID = CASE CreateUserType WHEN 0 THEN (SELECT CustomerID FROM Customer(NOLOCK) WHERE SysNo = Topic_Reply.CreateUserSysNo) WHEN 1 THEN (SELECT UserName FROM Sys_User (NOLOCK) WHERE SysNo = Topic_Reply.CreateUserSysNo) END,
                   CustomerRank = CASE CreateUserType WHEN 0 THEN (SELECT CustomerRank FROM Customer (NOLOCK) WHERE SysNo = Topic_Reply.CreateUserSysNo) WHEN 1 THEN 0 END
	               FROM Topic_Reply (NOLOCK)
                   left join Sys_User (NOLOCK) on Sys_User.SysNo = Topic_Reply.LastEditUserSysNo  
	               WHERE @sqlPar";

        private const string SQL_GET_COUNT_TOPICREPLY_BYID_USERTYPE
            = @"SELECT SysNo FROM Topic_Reply (NOLOCK) 
                WHERE TopicSysNo = @TopicSysNo AND CreateUserType = @CreateUserType";

        private const string SQL_INSERT_TOPICREPLY
            = @"INSERT INTO Topic_Reply (TopicSysNo,ReplyContent,CreateUserType,CreateUserSysNo) 
                VALUES (@TopicSysNo,@ReplyContent,@CreateUserType,@CreateUserSysNo) select @@Identity";

        private const string SQL_UPDATE_TOPICREPLY_STATUS
            = @"UPDATE Topic_Reply SET Status = @Status, LastEditUserSysNo = @LastEditUserSysNo 
                WHERE SysNo=@SysNo";

        private const string SQL_DELETE_TOPICREPLY
            = @"DELETE FROM Topic_Reply WHERE SysNo = @SysNo";
        #endregion

        protected static TopicReplyInfo Map(DataRow row)
        {
            TopicReplyInfo info = new TopicReplyInfo();

            info.SysNo = Convert.ToInt32(row["SysNo"]);
            info.TopicSysNo = Convert.ToInt32(row["TopicSysNo"]);
            info.ReplyContent = row["ReplyContent"].ToString();
            info.Status = (AppEnum.TopicReplyStatus)Convert.ToInt32(row["Status"]);
            info.CreateUserType = (AppEnum.CreateUserType)Convert.ToInt32(row["CreateUserType"]);
            info.CreateUserSysNo = Convert.ToInt32(row["CreateUserSysNo"]);
            info.CreateDate = Convert.ToDateTime(row["CreateDate"]);
            if (row["LastEditUserSysNo"] != DBNull.Value)
                info.LastEditUserSysNo = Convert.ToInt32(row["LastEditUserSysNo"]);
            else
                info.LastEditUserSysNo = 0;
            info.LastEditDate = Convert.ToDateTime(row["LastEditDate"]);
            info.CreateUserName = row["CustomerName"].ToString();
            info.LastEditUserName = row["UpdateUserName"].ToString();
            if (row["CustomerRank"] != DBNull.Value)
                info.CustomerRank = Convert.ToInt32(row["CustomerRank"]);
            if (row["CustomerID"] != DBNull.Value)
                info.CustomerID = row["CustomerID"].ToString();
            if (row["CustomerName"] != DBNull.Value)
                info.CustomerName = row["CustomerName"].ToString();
            return info;
        }

        public static int ReturnNumOfReplyByCustomerId(int customerID)
        {
            string sql = @"SELECT count(*) FROM Topic_Reply(NOLOCK) WHERE CreateUserType=0 AND CreateUserSysNo=" + customerID + " AND CreateDate >'" + DateTime.Today + "'";
            int topicNum = Convert.ToInt32(SqlHelper.ExecuteScalar(sql));
            return topicNum;
        }

        public static bool HasSysReply(int topicSysNo)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@TopicSysNo", SqlDbType.Int),
                new SqlParameter("@CreateUserType", SqlDbType.Int)
            };
            parms[0].Value = topicSysNo;
            parms[1].Value = (int)AppEnum.CreateUserType.Employee;

            DataTable dt = SqlHelper.ExecuteDataSet(SQL_GET_COUNT_TOPICREPLY_BYID_USERTYPE, parms).Tables[0];
            return dt.Rows.Count > 0;
        }

        public static List<TopicReplyInfo> GetTopicReplyByTopicIdList(string sysNoGroup)
        {
            string sql
                = @"SELECT Topic_Reply.SysNo as SysNo,Topic_Reply.TopicSysNo as TopicSysNo,Topic_Reply.ReplyContent as ReplyContent,
                       Topic_Reply.Status as Status,Topic_Reply.CreateUserType,Topic_Reply.CreateUserSysNo as CreateUserSysNo,
                       Topic_Reply.CreateDate as CreateDate, Topic_Reply.LastEditUserSysNo as LastEditUserSysNo,Topic_Reply.LastEditDate as LastEditDate,
                       Sys_User.UserName as UpdateUserName, CustomerName = CASE CreateUserType WHEN 0 THEN (SELECT NickName FROM Customer WHERE SysNo = Topic_Reply.CreateUserSysNo) WHEN 1 THEN '管理员' END,
				       CustomerRank = CASE CreateUserType WHEN 0 THEN (SELECT CustomerRank FROM Customer (NOLOCK) WHERE SysNo = Topic_Reply.CreateUserSysNo) WHEN 1 THEN 0 END,
                       CustomerID = CASE CreateUserType WHEN 0 THEN (SELECT CustomerID FROM Customer(NOLOCK) WHERE SysNo = Topic_Reply.CreateUserSysNo) WHEN 1 THEN '管理员' END,
					   CustomerName = CASE CreateUserType WHEN 0 THEN (SELECT NickName FROM Customer(NOLOCK) WHERE SysNo = Topic_Reply.CreateUserSysNo) WHEN 1 THEN '管理员' END
                       FROM  Topic_Reply (NOLOCK)
                       left join Sys_User on Sys_User.SysNo = Topic_Reply.LastEditUserSysNo  
                       WHERE Topic_Reply.Status <> " + (int)AppEnum.TopicReplyStatus.Abandon + " AND Topic_Reply.TopicSysNo in (" + sysNoGroup + ") ORDER BY Topic_Reply.CreateDate desc";


            DataTable dt = SqlHelper.ExecuteDataSet(sql).Tables[0];
            if (dt == null || dt.Rows.Count == 0)
                return null;

            List<TopicReplyInfo> list = new List<TopicReplyInfo>();

            foreach (DataRow row in dt.Rows)
                list.Add(Map(row));

            return list;
        }

        public static List<TopicReplyInfo> GetTopicReplyByTopicId(int topicSysNo)
        {
            TopicReplySearchCondition sc = new TopicReplySearchCondition();
            return GetTopicReplyByTopicId(topicSysNo, sc);
        }

        public static List<TopicReplyInfo> GetValidTopicReplyByTopicId(int topicSysNo)
        {
            TopicReplySearchCondition sc = new TopicReplySearchCondition();
            sc.Status = 0;
            return GetTopicReplyByTopicId(topicSysNo, sc);
        }

        public static List<TopicReplyInfo> GetAllTopicReplyBySearch(TopicReplySearchCondition rsc)
        {
            return GetTopicReplyByTopicId(0, rsc);
        }

        private static List<TopicReplyInfo> GetTopicReplyByTopicId(int topicSysNo, TopicReplySearchCondition rsc)
        {
            string sqlStr = SQL_GET_TOPICREPLY_BY_TOPICID;
            DataTable dt = new DataTable();

            if (topicSysNo != 0)
            {
                sqlStr = sqlStr.Replace("@select", "select ");
                sqlStr = sqlStr.Replace("@sqlPar", " Topic_Reply.TopicSysNo = @TopicSysNo @STATUS ORDER BY Topic_Reply.CreateDate desc");
                if (rsc != null && rsc.Status != null)
                    sqlStr = sqlStr.Replace("@STATUS", "AND Topic_Reply.Status = " + rsc.Status);
                else
                    sqlStr = sqlStr.Replace("@STATUS", "");

                SqlParameter[] parms = new SqlParameter[]
                {
                    new SqlParameter("@TopicSysNo", SqlDbType.Int)
                };
                parms[0].Value = topicSysNo;
                dt = SqlHelper.ExecuteDataSet(sqlStr, parms).Tables[0];
            }
            else
            {
                StringBuilder sb = new StringBuilder(" CreateUserType = 0 ");
                if (rsc.DateFrom != null)
                    sb.Append(" and Topic_Reply.CreateDate >= '" + rsc.DateFrom + "' ");

                if (rsc.DateTo != null)
                    sb.Append(" and Topic_Reply.CreateDate <= '" + rsc.DateTo.Value.AddDays(1).AddSeconds(-1) + "' ");

                if (rsc.Status != null)
                    sb.Append(" and Topic_Reply.Status = " + rsc.Status + "");

                if (rsc.CustomerId != string.Empty)
                    sb.Append(" and CreateUserSysNo IN (SELECT SysNo FROM Customer (NOLOCK) WHERE CustomerId LIKE '%" + rsc.CustomerId + "%') ");
                sb.Append(" ORDER BY Topic_Reply.CreateDate DESC");

                if (rsc.InEmpty == true)
                    sqlStr = sqlStr.Replace("@select", "select top 50 ");
                else
                    sqlStr = sqlStr.Replace("@select", "select ");

                sqlStr = sqlStr.Replace("@sqlPar", sb.ToString());
                dt = SqlHelper.ExecuteDataSet(sqlStr).Tables[0];
            }

            if (dt == null || dt.Rows.Count == 0)
                return null;
            List<TopicReplyInfo> list = new List<TopicReplyInfo>();
            foreach (DataRow row in dt.Rows)
                list.Add(Map(row));
            return list;
        }

        public static int InsertTopicReply(TopicReplyInfo info)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@TopicSysNo", SqlDbType.Int),
                new SqlParameter("@ReplyContent", SqlDbType.NVarChar),
                new SqlParameter("@CreateUserType", SqlDbType.Int),
                new SqlParameter("@CreateUserSysNo", SqlDbType.Int),
            };

            parms[0].Value = info.TopicSysNo;
            parms[1].Value = info.ReplyContent;
            parms[2].Value = (int)info.CreateUserType;
            parms[3].Value = info.CreateUserSysNo;

            SqlCommand cmd = new SqlCommand(SQL_INSERT_TOPICREPLY);
            cmd.Parameters.AddRange(parms);
            cmd.CommandTimeout = 180;
            object obj = SqlHelper.ExecuteScalar(cmd);

            return Convert.ToInt32(obj);
        }

        public static void AbandTopicReply(int topicReplySysNo, int userSysNo, AppEnum.TopicReplyStatus status)
        {
            AbandTopicReply(topicReplySysNo, userSysNo, status, string.Empty);
        }

        public static void AbandTopicReplys(string sysNos, int userSysNo, AppEnum.TopicReplyStatus status)
        {
            AbandTopicReply(0, userSysNo, status, sysNos);
        }

        private static void AbandTopicReply(int topicReplySysNo, int userSysNo, AppEnum.TopicReplyStatus status, string sysNos)
        {
            string sqlStr;
            if (sysNos == string.Empty)
                sqlStr = SQL_UPDATE_TOPICREPLY_STATUS.Replace("SysNo=@SysNo", "SysNo=" + topicReplySysNo);
            else
                sqlStr = SQL_UPDATE_TOPICREPLY_STATUS.Replace("SysNo=@SysNo", "SysNo in (" + sysNos + ")");

            SqlParameter[] parms = new SqlParameter[]
            {
       
              new  SqlParameter("@Status", SqlDbType.Int),
              new  SqlParameter("@LastEditUserSysNo", SqlDbType.Int),
            };

            parms[0].Value = (int)status;
            parms[1].Value = userSysNo;

            SqlCommand cmd = new SqlCommand(sqlStr);
            cmd.Parameters.AddRange(parms);
            cmd.CommandTimeout = 180;
            SqlHelper.ExecuteNonQuery(cmd);
        }

        public static void DeleteTopicReply(int SysNo)
        {
            SqlParameter[] parms = new SqlParameter[1];

            parms[0] = new SqlParameter("@SysNo", SqlDbType.Int);
            parms[0].Value = SysNo;

            SqlCommand cmd = new SqlCommand(SQL_DELETE_TOPICREPLY);
            cmd.Parameters.AddRange(parms);
            cmd.CommandTimeout = 180;
            SqlHelper.ExecuteNonQuery(cmd);
        }
    }
}