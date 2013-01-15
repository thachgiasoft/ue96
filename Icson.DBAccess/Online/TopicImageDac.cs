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
    public class TopicImageDac
    {
        #region Sql Script

        private const string SQL_GET_IMAGE_BY_TOPICID
            = @"@select [SysNo]
					  ,[TopicSysNo]
					  ,[ImageLink]
					  ,[ThumbnailLink]
					  ,[HitCount]
					  ,[Status]
					  ,[CreateUserSysNo]
					  ,[CreateDate]
				  FROM [Topic_Image]
				  WHERE @sqlPar";

        private const string SQL_GET_COUNT_TOPICIMAGE_BYID
            = @"SELECT SysNo     
				FROM Topic_Image(NOLOCK) 
				WHERE TopicSysNo = @TopicSysNo ";

        private const string SQL_INSERT_TOPICIMAGE
            = @"INSERT INTO Topic_Image
						   (TopicSysNo
						   ,ImageLink
						   ,ThumbnailLink       
						   ,CreateUserSysNo)
				VALUES
							(@TopicSysNo
						   ,@ImageLink
						   ,@ThumbnailLink       
						   ,@CreateUserSysNo)
				select @@Identity";

        private const string SQL_UPDATE_TOPICIMAGE_STATUS
        = @"UPDATE Topic_Image SET Status = @Status
                WHERE SysNo=@SysNo";

        private const string SQL_DELETE_TOPICIMAGE
            = @"DELETE FROM Topic_Image WHERE SysNo = @SysNo";
        #endregion

        protected static TopicImageInfo Map(DataRow row)
        {
            TopicImageInfo info = new TopicImageInfo();

            info.SysNo = Convert.ToInt32(row["SysNo"]);
            info.TopicSysNo = Convert.ToInt32(row["TopicSysNo"]);
            info.ImageLink = row["ImageLink"].ToString();
            info.ThumbnailLink = row["ThumbnailLink"].ToString();
            info.HitCount = Convert.ToInt32(row["HitCount"]);
            info.Status = (AppEnum.TopicImageStatus)Convert.ToInt32(row["Status"]);
            info.CreateUserSysNo = Convert.ToInt32(row["CreateUserSysNo"]);
            info.CreateDate = Convert.ToDateTime(row["CreateDate"]);
            return info;
        }


        public static int ReturnNumOfImageByCustomerId(int customerID)
        {
            string sql = @"SELECT count(*) FROM Topic_Image(NOLOCK) WHERE CreateUserSysNo=" + customerID + " AND CreateDate >'" + DateTime.Today + "'";
            int topicNum = Convert.ToInt32(SqlHelper.ExecuteScalar(sql));
            return topicNum;
        }

        public static List<TopicImageInfo> GetTopicImageByTopicIdList(string sysNoGroup)
        {
            string sql
                = @"SELECT 
						Topic_Image.SysNo as SysNo,
						Topic_Image.TopicSysNo as TopicSysNo,
						Topic_Image.ImageLink as ImageLink,
						Topic_Image.ThumbnailLink as ThumbnailLink,
						Topic_Image.HitCount as HitCount,
						Topic_Image.Status as Status,
						Topic_Image.CreateUserSysNo as CreateUserSysNo,
						Topic_Image.CreateDate as CreateDate 
					FROM  Topic_Image (NOLOCK)
					WHERE Topic_Image.Status <> " + (int)AppEnum.TopicImageStatus.Abandon + " AND Topic_Image.TopicSysNo in (" + sysNoGroup + ") ORDER BY Topic_Image.CreateDate desc";

            DataTable dt = SqlHelper.ExecuteDataSet(sql).Tables[0];
            if (dt == null || dt.Rows.Count == 0)
                return null;

            List<TopicImageInfo> list = new List<TopicImageInfo>();

            foreach (DataRow row in dt.Rows)
                list.Add(Map(row));

            return list;
        }

        public static List<TopicImageInfo> GetTopicImagesByTopicId(int topicSysNo)
        {
            TopicImageSearchCondition sc = new TopicImageSearchCondition();
            return GetTopicImageByTopicId(topicSysNo, sc);
        }

        public static List<TopicImageInfo> GetValidTopicImageByTopicId(int topicSysNo)
        {
            TopicImageSearchCondition sc = new TopicImageSearchCondition();
            sc.Status = (int)AppEnum.TopicImageStatus.Normal;
            return GetTopicImageByTopicId(topicSysNo, sc);
        }

        public static List<TopicImageInfo> GetAllTopicImageBySearch(TopicImageSearchCondition rsc)
        {
            return GetTopicImageByTopicId(0, rsc);
        }

        public static int AddTopicImageCount(int SysNo)
        {
            string sql = @"UPDATE Topic_Image
							 SET hitCount = hitCount + 1
							WHERE Topic_Image.SysNo = " + SysNo;
            return SqlHelper.ExecuteNonQuery(sql);
        }

        private static List<TopicImageInfo> GetTopicImageByTopicId(int topicSysNo, TopicImageSearchCondition rsc)
        {
            string sqlStr = SQL_GET_IMAGE_BY_TOPICID;
            DataTable dt = new DataTable();

            if (topicSysNo != 0)
            {
                sqlStr = sqlStr.Replace("@select", "select ");
                sqlStr = sqlStr.Replace("@sqlPar", " Topic_Image.TopicSysNo = @TopicSysNo @STATUS ORDER BY Topic_Image.CreateDate desc");
                if (rsc != null && rsc.Status != null)
                    sqlStr = sqlStr.Replace("@STATUS", "AND Topic_Image.Status = " + rsc.Status);
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
                    sb.Append(" and Topic_Image.CreateDate >= '" + rsc.DateFrom + "' ");

                if (rsc.DateTo != null)
                    sb.Append(" and Topic_Image.CreateDate <= '" + rsc.DateTo.Value.AddDays(1).AddSeconds(-1) + "' ");

                if (rsc.Status != null)
                    sb.Append(" and Topic_Image.Status = " + rsc.Status + "");

                if (rsc.CustomerId != string.Empty)
                    sb.Append(" and CreateUserSysNo IN (SELECT SysNo FROM Customer (NOLOCK) WHERE CustomerId LIKE '%" + rsc.CustomerId + "%') ");
                sb.Append(" ORDER BY Topic_Image.CreateDate DESC");

                if (rsc.InEmpty == true)
                    sqlStr = sqlStr.Replace("@select", "select top 50 ");
                else
                    sqlStr = sqlStr.Replace("@select", "select ");

                sqlStr = sqlStr.Replace("@sqlPar", sb.ToString());
                dt = SqlHelper.ExecuteDataSet(sqlStr).Tables[0];
            }

            if (dt == null || dt.Rows.Count == 0)
                return null;
            List<TopicImageInfo> list = new List<TopicImageInfo>();
            foreach (DataRow row in dt.Rows)
                list.Add(Map(row));
            return list;
        }

        public static TopicImageInfo GetTopicImageBySysNo(int SysNo)
        {
            string sql = @"SELECT * FROM Topic_Image (NOLOCK) WHERE sysno = " + SysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            TopicImageInfo i = new TopicImageInfo();
            if (Util.HasMoreRow(ds) == false)
                return i;
            if (ds.Tables[0].Rows.Count > 1)
                throw new Exception("错误：同一ID对应一条以上的数据");
            i = Map(ds.Tables[0].Rows[0]);
            return i;
        }

        public static int InsertTopicImage(TopicImageInfo info)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@TopicSysNo", SqlDbType.Int),
                new SqlParameter("@ImageLink", SqlDbType.NVarChar),
				new SqlParameter("@ThumbnailLink", SqlDbType.NVarChar),
                new SqlParameter("@CreateUserSysNo", SqlDbType.Int),
            };

            parms[0].Value = info.TopicSysNo;
            parms[1].Value = info.ImageLink;
            parms[2].Value = info.ThumbnailLink;
            parms[3].Value = info.CreateUserSysNo;

            SqlCommand cmd = new SqlCommand(SQL_INSERT_TOPICIMAGE);
            cmd.Parameters.AddRange(parms);
            cmd.CommandTimeout = 180;
            object obj = SqlHelper.ExecuteScalar(cmd);

            return Convert.ToInt32(obj);
        }

        public static void AbandTopicImage(int topicImageSysNo, AppEnum.TopicImageStatus status)
        {
            AbandTopicImage(topicImageSysNo, status, string.Empty);
        }

        public static void AbandTopicImages(string sysNos, AppEnum.TopicImageStatus status)
        {
            AbandTopicImage(0, status, sysNos);
        }

        private static void AbandTopicImage(int topicImageSysNo, AppEnum.TopicImageStatus status, string sysNos)
        {
            string sqlStr;
            if (sysNos == string.Empty)
                sqlStr = SQL_UPDATE_TOPICIMAGE_STATUS.Replace("SysNo=@SysNo", "SysNo=" + topicImageSysNo);
            else
                sqlStr = SQL_UPDATE_TOPICIMAGE_STATUS.Replace("SysNo=@SysNo", "SysNo in (" + sysNos + ")");

            SqlParameter[] parms = new SqlParameter[]
            {
       
              new  SqlParameter("@Status", SqlDbType.Int),
            };

            parms[0].Value = (int)status;

            SqlCommand cmd = new SqlCommand(sqlStr);
            cmd.Parameters.AddRange(parms);
            cmd.CommandTimeout = 180;
            SqlHelper.ExecuteNonQuery(cmd);
        }

        public static void DeleteTopicImage(int SysNo)
        {
            SqlParameter[] parms = new SqlParameter[1];

            parms[0] = new SqlParameter("@SysNo", SqlDbType.Int);
            parms[0].Value = SysNo;

            SqlCommand cmd = new SqlCommand(SQL_DELETE_TOPICIMAGE);
            cmd.Parameters.AddRange(parms);
            cmd.CommandTimeout = 180;
            SqlHelper.ExecuteNonQuery(cmd);
        }
    }
}