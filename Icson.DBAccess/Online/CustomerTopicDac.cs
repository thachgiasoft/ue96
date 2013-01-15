using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;

using Icson.Utils;
using Icson.Objects.Online;

namespace Icson.DBAccess.Online
{
    public class CustomerTopicDac
    {
        #region Sql Script

        private const string SQL_GET_ALL_CUSTOMERTOPICINFO
            = @"SELECT SysNo,CustomerSysNo,TopicCount,DigestCount,ReplyCount,RemarkCount,LastEditUserSysNo,LastEditDate 
				FROM [AYS].[dbo].[Customer_Topic] (NOLOCK)";

        private const string SQL_GET_CUSTOMERTOPICINFO
            = @"SELECT SysNo,CustomerSysNo,TopicCount,DigestCount,ReplyCount,RemarkCount,LastEditUserSysNo,LastEditDate 
				FROM [AYS].[dbo].[Customer_Topic] (NOLOCK)
				WHERE SysNo=@SysNo";

        private const string SQL_GET_CUSTOMERTOPICINFO_BY_ID
            = @"SELECT count(*)
                FROM [AYS].[dbo].[Customer_Topic] (NOLOCK)
                WHERE CustomerSysNo=@CustomerSysNo";

        private const string SQL_INSERT_CUSTOMERTOPICINFO
        = @"INSERT INTO [AYS].[dbo].Customer_Topic(CustomerSysNo,TopicCount) 
                SELECT @CustomerSysNo,(SELECT COUNT(*) FROM Topic_Master (NOLOCK) WHERE CreateCustomerSysNo=@CustomerSysNo) 
                SELECT @@Identity";

        private const string SQL_DELETE_CUSTOMERTOPICINFO = "DELETE FROM [AYS].[dbo].[Customer_Topic] WHERE SysNo = @SysNo";
        #endregion

        protected static CustomerTopicInfo Map(DataRow row)
        {
            CustomerTopicInfo info = new CustomerTopicInfo();

            info.SysNo = Convert.ToInt32(row["SysNo"]);
            info.CustomerSysNo = Convert.ToInt32(row["CustomerSysNo"]);
            info.TopicCount = Convert.ToInt32(row["TopicCount"]);
            info.DigestCount = Convert.ToInt32(row["DigestCount"]);
            info.ReplyCount = Convert.ToInt32(row["ReplyCount"]);
            info.RemarkCount = Convert.ToInt32(row["RemarkCount"]);
            info.LastEditUserSysNo = Convert.ToInt32(row["LastEditUserSysNo"]);
            info.LastEditDate = Convert.ToDateTime(row["LastEditDate"]);
            return info;
        }

        /// <summary>
        /// 获取所有的CustomerTopicInfo
        /// </summary>
        /// <returns></returns>
        public static DataTable GetCustomerTopicInfo()
        {
            return SqlHelper.ExecuteDataSet(SQL_GET_ALL_CUSTOMERTOPICINFO).Tables[0];
        }

        /// <summary>
        /// 判断特定用户是否包含CustomerTopic数据
        /// </summary>
        /// <param name="customerSysNo"></param>
        /// <returns></returns>
        public static bool HasCustomerTopic(int customerSysNo)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                 new SqlParameter("@CustomerSysNo", SqlDbType.Int),
            };

            parms[0].Value = customerSysNo;

            SqlCommand cmd = new SqlCommand(SQL_GET_CUSTOMERTOPICINFO_BY_ID);
            cmd.Parameters.AddRange(parms);
            cmd.CommandTimeout = 180;
            object obj = SqlHelper.ExecuteScalar(cmd);

            return Convert.ToInt32(obj) > 0;
        }

        public static int InsertCustomerTopicInfo(CustomerTopicInfo info)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                         new SqlParameter("@CustomerSysNo", SqlDbType.Int),
             };

            parms[0].Value = info.CustomerSysNo;

            SqlCommand cmd = new SqlCommand(SQL_INSERT_CUSTOMERTOPICINFO);
            cmd.Parameters.AddRange(parms);
            cmd.CommandTimeout = 180;
            object obj = SqlHelper.ExecuteScalar(cmd);

            return Convert.ToInt32(obj);
        }

        public static void DeleteCustomerTopicInfo(int SysNo)
        {
            SqlParameter[] parms = new SqlParameter[1];

            parms[0] = new SqlParameter("@SysNo", SqlDbType.Int);
            parms[0].Value = SysNo;

            SqlCommand cmd = new SqlCommand(SQL_DELETE_CUSTOMERTOPICINFO);
            cmd.Parameters.AddRange(parms);
            cmd.CommandTimeout = 180;
            SqlHelper.ExecuteNonQuery(cmd);
        }

        #region Search
        public static string ReturnOperate(int num)
        {
            switch (num)
            {
                case 1:
                    return "<";
                case 2:
                    return ">";
                case 0:
                default:
                    return "=";
            }
        }

        public static DataSet SearchTopicCustomerRights(CustomerTopicSearchCondition SearchCondition)
        {
            DataSet ds = new DataSet();
            string sql = @"@select Customer_Topic.SysNo as SysNo,Customer_Topic.CustomerSysNo as CustomerSysNo,
                                   Customer_Topic.TopicCount as TopicCount,Customer_Topic.DigestCount as DigestCount,
                                   Customer_Topic.ReplyCount as ReplyCount,Customer_Topic.RemarkCount as RemarkCount,
                                   Customer_Topic.LastEditUserSysNo as LastEditUserSysNo,Customer_Topic.LastEditDate as LastEditDate,
                                   Customer.CustomerID as CustomerID,Customer.CustomerName as CustomerName,Sys_User.UserName as LastEditUserName
                           from  Customer_Topic (NOLOCK)
                                 inner join Customer (NOLOCK) on Customer.SysNo = Customer_Topic.CustomerSysNo
                                 left join Sys_User (NOLOCK) on Sys_User.SysNo = Customer_Topic.LastEditUserSysNo 
                           where 1=1 @sqlPar order by Customer_Topic.LastEditDate desc";

            SqlParameter[] paras = new SqlParameter[]
            {
                   new SqlParameter("@CustomerID ", SqlDbType.NVarChar),
                   new SqlParameter("@TopicCount", SqlDbType.Int),
                   new SqlParameter("@DigestCount", SqlDbType.Int),
                   new SqlParameter("@ReplyCount", SqlDbType.Int),
                   new SqlParameter("@RemarkCount", SqlDbType.Int),
                   new SqlParameter("@CustomerSysNo", SqlDbType.Int)
                
            };

            paras[0].Value = "";
            paras[1].Value = 0;
            paras[2].Value = 0;
            paras[3].Value = 0;
            paras[4].Value = 0;
            paras[5].Value = 0;

            StringBuilder sb = new StringBuilder();
            if (SearchCondition.CustomerID != string.Empty)
            {
                sb.Append(" and Customer.CustomerID =@CustomerID ");
                paras[0].Value = SearchCondition.CustomerID;
            }
            if (SearchCondition.TopicCount != null)
            {
                sb.Append(" and Customer_Topic.TopicCount " + ReturnOperate(SearchCondition.TopicCountSign) + " @TopicCount ");
                paras[1].Value = SearchCondition.TopicCount;
            }
            if (SearchCondition.DigestCount != null)
            {
                sb.Append(" and Customer_Topic.DigestCount " + ReturnOperate(SearchCondition.DigestCountSign) + " @DigestCount ");
                paras[2].Value = SearchCondition.DigestCount;
            }
            if (SearchCondition.ReplyCount != null)
            {
                sb.Append(" and Customer_Topic.ReplyCount " + ReturnOperate(SearchCondition.ReplyCountSign) + " @ReplyCount ");
                paras[3].Value = SearchCondition.ReplyCount;
            }
            if (SearchCondition.RemarkCount != null)
            {
                sb.Append(" and Customer_Topic.RemarkCount " + ReturnOperate(SearchCondition.RemarkCountSign) + " @RemarkCount ");
                paras[4].Value = SearchCondition.RemarkCount;
            }
            if (SearchCondition.CustomerSysNo != null)
            {
                sb.Append(" and Customer_Topic.CustomerSysNo = @CustomerSysNo ");
                paras[6].Value = SearchCondition.CustomerSysNo;
            }

            if (SearchCondition.IsEmpty == true)
                sql = sql.Replace("@select", "select top 50 ");
            else
                sql = sql.Replace("@select", "select  ");
            sql = sql.Replace("@sqlPar", sb.ToString());

            return SqlHelper.ExecuteDataSet(sql, paras);
        }
        #endregion
    }


}
