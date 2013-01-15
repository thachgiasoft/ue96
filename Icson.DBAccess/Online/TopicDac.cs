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
    public class TopicDac
    {
        #region Sql Script

        private const string SQL_GET_TOPIC_BY_ID
            = @"SELECT Topic_Master.SysNo as SysNo,
					   TopicType,Title, TopicContent, IsTop, IsDigest,ReferenceType,ReferenceSysNo,
					   TotalRemarkCount,TotalUsefulRemarkCount,
					   Score,TotalComplainCount,Topic_Master.Status as Status,
					   Topic_Master.CreateCustomerSysNo as CreateCustomerSysNo,
					   Topic_Master.CreateDate as CreateDate,Topic_Master.LastEditUserSysNo as LastEditUserSysNo,
					   Topic_Master.LastEditDate as LastEditDate, 
					   CASE ReferenceType WHEN 0 THEN 
							(SELECT ProductName FROM Product (NOLOCK) WHERE SysNo = ReferenceSysNo) 
							ELSE '-ReferenceName-' END as ReferenceName,
					   Customer.CustomerRank as CustomerRank,Customer.CustomerName as CustomerName,Customer.CustomerID as CustomerID
				FROM Topic_Master (NOLOCK)
                INNER JOIN Customer (NOLOCK) ON  Topic_Master.CreateCustomerSysNo=Customer.SysNo
                WHERE Topic_Master.SysNo=@SysNo ";

        private const string SQL_GET_TOPIC_BY_PRODUCTID
            = @"@select Topic_Master.SysNo,TopicType,Title,TopicContent,IsTop,IsDigest,
						ReferenceType,ReferenceSysNo,TotalRemarkCount,TotalUsefulRemarkCount,Score,
						TotalComplainCount,Topic_Master.Status as Status,CreateCustomerSysNo,CreateDate,LastEditUserSysNo,
						LastEditDate, '-ReferenceName-' as ReferenceName,
						Customer.CustomerRank as CustomerRank,Customer.CustomerName as CustomerName,Customer.CustomerID as CustomerID
				FROM Topic_Master (NOLOCK)
                INNER JOIN Customer (NOLOCK) ON Topic_Master.CreateCustomerSysNo = Customer.SysNo
				WHERE ReferenceSysNo = @ReferenceSysNo AND @Value 
                AND ReferenceType = @ReferenceType
                AND Topic_Master.Status > 0 
                ORDER BY Topic_Master.IsTop DESC, Topic_Master.CreateDate DESC";

        private const string SQL_CHANGE_TOPIC_STATUS
            = @"UPDATE Topic_Master 
                SET Status = @Status WHERE SysNo = @SysNo";

        private const string SQL_INSERT_TOPIC
            = @"INSERT INTO Topic_Master (TopicType,Title,TopicContent,ReferenceType,ReferenceSysNo,Score,Status,CreateCustomerSysNo) 
            VALUES (@TopicType,@Title,@TopicContent,@ReferenceType,@ReferenceSysNo,@Score,@Status,@CreateCustomerSysNo) select @@Identity";

        private const string SQL_UPDATE_TOPIC = "UPDATE Topic_Master SET TopicType=@TopicType,Title=@Title,TopicContent=@TopicContent,IsTop=@IsTop,IsDigest=@IsDigest,ReferenceType=@ReferenceType,ReferenceSysNo=@ReferenceSysNo,TotalRemarkCount=@TotalRemarkCount,TotalUsefulRemarkCount=@TotalUsefulRemarkCount,Score=@Score,TotalComplainCount=@TotalComplainCount,Status=@Status,CreateCustomerSysNo=@CreateCustomerSysNo,CreateDate=@CreateDate,LastEditUserSysNo=@LastEditUserSysNo,LastEditDate=getdate() WHERE SysNo=@SysNo";
        private const string SQL_UPDATE_TOPIC_BYTYPE = "UPDATE Topic_Master SET LastEditUserSysNo=@LastEditUserSysNo,@Key=@Value WHERE @SysNo";
        private const string SQL_DELETE_TOPIC = "DELETE FROM Topic_Master WHERE SysNo = @SysNo";
        #endregion

        protected static TopicInfo Map(DataRow row)
        {
            TopicInfo info = new TopicInfo();

            info.SysNo = Convert.ToInt32(row["SysNo"]);
            info.TopicType = (AppEnum.TopicType)Convert.ToInt32(row["TopicType"]);
            info.Title = row["Title"].ToString();
            info.TopicContent = row["TopicContent"].ToString();
            info.IsTop = Convert.ToInt32(row["IsTop"]) == 1;
            info.IsDigest = Convert.ToInt32(row["IsDigest"]) == 1;
            info.ReferenceType = Convert.ToInt32(row["ReferenceType"]);
            info.ReferenceSysNo = Convert.ToInt32(row["ReferenceSysNo"]);
            info.TotalRemarkCount = Convert.ToInt32(row["TotalRemarkCount"]);
            info.TotalUsefulRemarkCount = Convert.ToInt32(row["TotalUsefulRemarkCount"]);
            info.Score = Convert.ToInt32(row["Score"]);
            info.TotalComplainCount = Convert.ToInt32(row["TotalComplainCount"]);
            info.Status = (AppEnum.TopicStatus)Convert.ToInt32(row["Status"]);
            info.CreateCustomerSysNo = Convert.ToInt32(row["CreateCustomerSysNo"]);
            info.CreateDate = Convert.ToDateTime(row["CreateDate"]);
            info.LastEditDate = Convert.ToDateTime(row["LastEditDate"]);
            info.CustomerRank = Convert.ToInt32(row["CustomerRank"]);

            if (row["CustomerName"] != DBNull.Value)
                info.CreateUsername = row["CustomerName"].ToString();

            if (row["LastEditUserSysNo"] != DBNull.Value)
                info.LastEditUserSysNo = Convert.ToInt32(row["LastEditUserSysNo"]);
            else
                info.LastEditUserSysNo = 0;

            if (row["CustomerId"] != DBNull.Value)
                info.CustomerID = row["CustomerId"].ToString();

            if (row["CustomerName"] != DBNull.Value)
                info.CustomerName = row["CustomerName"].ToString();

            info.ReferenceName = row["ReferenceName"].ToString();
            return info;
        }

        /// <summary>
        /// 根据顾客ID，返回其当天的发表"参考"数量
        /// </summary>
        /// <param name="custSysNo">custSysNo</param>
        /// <returns></returns>
        public static int ReturnNumOfTopicByCustomerId(int custSysNo)
        {
            string sql = @"SELECT count(*) FROM Topic_Master (NOLOCK) 
                WHERE TopicType = 2 AND CreateCustomerSysNo=" + custSysNo
                + " AND CreateDate >'" + DateTime.Today.ToString() + "'";

            return Convert.ToInt32(SqlHelper.ExecuteScalar(sql));
        }

        public static TopicInfo GetTopicByTopicId(int TopicId)
        {
            SqlParameter[] parms = new SqlParameter[]
                 {
                   new SqlParameter("@SysNo", SqlDbType.Int),
                 };
            parms[0].Value = TopicId;

            DataTable dt = SqlHelper.ExecuteDataSet(SQL_GET_TOPIC_BY_ID, parms).Tables[0];
            if (dt == null || dt.Rows.Count == 0)
                return null;

            return Map(dt.Rows[0]);
        }

        /// <summary>
        /// 根据商品ID返回所有评论
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalCount"></param>
        /// <param name="type"> "Top3","All","Digest","Discuss","Exp"</param>
        /// <returns></returns>
        public static List<TopicInfo> GetAllTopicByProductId(int productId, int pageSize, int pageIndex, out int totalCount, string type)
        {
            SqlParameter[] parms = new SqlParameter[]
             {
                 new SqlParameter("@ReferenceSysNo", SqlDbType.Int),
                 new SqlParameter("@ReferenceType", SqlDbType.Int),
                 new SqlParameter("@Status", SqlDbType.Int)
             };
            parms[0].Value = productId;
            parms[1].Value = (int)AppEnum.TopicReferenceType.Product;
            parms[2].Value = (int)AppEnum.TopicStatus.Abandon;

            string sqlStr = SQL_GET_TOPIC_BY_PRODUCTID;

            if (type == "Top3")
            {
                sqlStr = SQL_GET_TOPIC_BY_PRODUCTID.Replace("@select", "SELECT TOP 3");
                sqlStr = sqlStr.Replace("@Value", "1=1");
            }
            else
                sqlStr = sqlStr.Replace("@select", "SELECT");

            if (type == "All")
                sqlStr = sqlStr.Replace("@Value", "1=1");
            if (type == "Digest")
                sqlStr = sqlStr.Replace("@Value", "Topic_Master.IsDigest = 1");
            if (type == "Discuss")
                sqlStr = sqlStr.Replace("@Value", "Topic_Master.TopicType = " + (int)AppEnum.TopicType.Discuss);
            if (type == "Exp")
                sqlStr = sqlStr.Replace("@Value", "Topic_Master.TopicType = " + (int)AppEnum.TopicType.Experience);

            DataTable dt = SqlHelper.ExecuteDataSet(sqlStr, parms).Tables[0];

            if (dt == null || dt.Rows.Count == 0)
            {
                totalCount = 0;
                return null;
            }

            totalCount = dt.Rows.Count;

            List<TopicInfo> list = new List<TopicInfo>();
            string ids = string.Empty;

            if (pageSize > 0)
            {
                // 需要分页的
                int from = pageSize * pageIndex;
                int to = (totalCount >= (pageSize * pageIndex + pageSize)) ? (pageSize * pageIndex + pageSize) : totalCount;
                for (int i = from; i < to; i++)
                {
                    DataRow row = dt.Rows[i];
                    ids += (ids.Length == 0 ? "" : ",") + row["SysNo"].ToString();
                    list.Add(Map(row));
                }
            }
            else
            {
                foreach (DataRow row in dt.Rows)
                {
                    ids += (ids.Length == 0 ? "" : ",") + row["SysNo"].ToString();
                    list.Add(Map(row));
                }
            }

            // 添加 TopicReply 信息
            List<TopicReplyInfo> replyList = new List<TopicReplyInfo>();
            replyList = TopicReplyDac.GetTopicReplyByTopicIdList(ids);


            //添加TopicImage信息
            List<TopicImageInfo> imageList = new List<TopicImageInfo>();
            imageList = TopicImageDac.GetTopicImageByTopicIdList(ids);
            //添加Imagedac时补充读取Image信息
            FormatTopicInfoList(list, replyList, imageList);

            return list;
        }

        /// <summary>
        /// 把 TopicTeply 附加到 Topic 列表中
        /// </summary>
        /// <param name="list"></param>
        /// <param name="replyList"></param>
        private static void FormatTopicInfoList(List<TopicInfo> list, List<TopicReplyInfo> replyList, List<TopicImageInfo> imageList)
        {
            if (list == null || (replyList == null && imageList == null))
                return;

            foreach (TopicInfo info in list)
            {
                if (replyList != null)
                {
                    foreach (TopicReplyInfo replyInfo in replyList)
                        if (info.SysNo == replyInfo.TopicSysNo)
                        {
                            if (info.ReplayList == null)
                                info.ReplayList = new List<TopicReplyInfo>();
                            List<TopicReplyInfo> t = info.ReplayList;
                            t.Add(replyInfo);
                            info.ReplayList = t;
                        }
                }
                if (imageList != null)
                {
                    foreach (TopicImageInfo imageInfo in imageList)
                        if (info.SysNo == imageInfo.TopicSysNo)
                        {
                            if (info.ImageList == null)
                                info.ImageList = new List<TopicImageInfo>();
                            List<TopicImageInfo> t = info.ImageList;
                            t.Add(imageInfo);
                            info.ImageList = t;
                            //info.ImageList.Add(imageInfo);
                        }
                }
            }
        }

        #region Insert / Update / Delete / Query

        public static int InsertTopic(TopicInfo info)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                   new SqlParameter("@TopicType", SqlDbType.Int),
                   new SqlParameter("@Title", SqlDbType.NVarChar),
                   new SqlParameter("@TopicContent", SqlDbType.NText),
                   new SqlParameter("@ReferenceType", SqlDbType.Int),
                   new SqlParameter("@ReferenceSysNo", SqlDbType.Int),
                   new SqlParameter("@Score", SqlDbType.Int),
					  new SqlParameter("@Status", SqlDbType.Int),
                   new SqlParameter("@CreateCustomerSysNo", SqlDbType.Int),
             };

            parms[0].Value = (int)info.TopicType;
            parms[1].Value = info.Title;
            parms[2].Value = info.TopicContent;
            parms[3].Value = info.ReferenceType;
            parms[4].Value = info.ReferenceSysNo;
            parms[5].Value = info.Score;
            parms[6].Value = (int)info.Status;
            parms[7].Value = info.CreateCustomerSysNo;

            SqlCommand cmd = new SqlCommand(SQL_INSERT_TOPIC);
            cmd.Parameters.AddRange(parms);
            cmd.CommandTimeout = 180;
            object obj = SqlHelper.ExecuteScalar(cmd);

            return Convert.ToInt32(obj);
        }

        public static void UpdateTopicByType(int topicSysNo, int userSysNo, AppEnum.TopicUpdateType type)
        {
            UpdateTopicByType(topicSysNo, userSysNo, type, string.Empty);
        }
        public static void UpdateTopicsByType(string topicSysNos, int userSysNo, AppEnum.TopicUpdateType type)
        {
            UpdateTopicByType(0, userSysNo, type, topicSysNos);
        }
        private static void UpdateTopicByType(int topicSysNo, int userSysNo, AppEnum.TopicUpdateType type, string topicSysNos)
        {
            string sqlStr;

            if (topicSysNos == string.Empty)
                sqlStr = SQL_UPDATE_TOPIC_BYTYPE.Replace("@SysNo", "SysNo=" + topicSysNo);
            else
                sqlStr = SQL_UPDATE_TOPIC_BYTYPE.Replace("@SysNo", "SysNo in (" + topicSysNos + ")");


            int value;

            SqlParameter[] parms = new SqlParameter[]
                 {
                  
                   new SqlParameter("@LastEditUserSysNo", SqlDbType.Int),
                   new SqlParameter("@Value", SqlDbType.Int)
                 };

            switch (type)
            {
                case AppEnum.TopicUpdateType.AbandonTopic:
                    sqlStr = sqlStr.Replace("@Key", "Status");
                    value = (int)AppEnum.TopicStatus.Abandon;
                    break;
                case AppEnum.TopicUpdateType.CancelAbandonTopic:
                    sqlStr = sqlStr.Replace("@Key", "Status");
                    if (TopicReplyDac.HasSysReply(topicSysNo))
                        value = (int)AppEnum.TopicStatus.Replyed;// 2;
                    else
                        value = (int)AppEnum.TopicStatus.confirmed;//1;
                    break;
                case AppEnum.TopicUpdateType.TopicCancelDigset:
                    sqlStr = sqlStr.Replace("@Key", "IsDigest");
                    value = 0;
                    break;
                case AppEnum.TopicUpdateType.TopicSetDigset:
                    sqlStr = sqlStr.Replace("@Key", "IsDigest");
                    value = 1;
                    break;
                case AppEnum.TopicUpdateType.TopicCancelTop:
                    sqlStr = sqlStr.Replace("@Key", "IsTop");
                    value = 0;
                    break;
                case AppEnum.TopicUpdateType.TopicSetTop:
                    sqlStr = sqlStr.Replace("@Key", "IsTop");
                    value = 1;
                    break;
                case AppEnum.TopicUpdateType.ConfirmTopic:
                    sqlStr = sqlStr.Replace("@Key", "Status");
                    if (TopicReplyDac.HasSysReply(topicSysNo))
                        value = (int)AppEnum.TopicStatus.Replyed;// 2;
                    else
                        value = (int)AppEnum.TopicStatus.confirmed;//1;
                    break;
                case AppEnum.TopicUpdateType.UnConfirmTopic:
                    sqlStr = sqlStr.Replace("@Key", "Status");
                    value = (int)AppEnum.TopicStatus.unconfirmed; ;
                    break;
                default:
                    throw new ArgumentException("Topic Update Type is invalid.");
            }

            parms[0].Value = userSysNo;
            parms[1].Value = value;

            SqlCommand cmd = new SqlCommand(sqlStr);
            cmd.Parameters.AddRange(parms);
            cmd.CommandTimeout = 180;
            SqlHelper.ExecuteNonQuery(cmd);

            if (topicSysNos == string.Empty)
                UpdateProductRemarkCountAndScoreByTopicSysNo(topicSysNo);
            else
            {
                foreach (string s in topicSysNos.Split(new char[] { ',' }))
                {
                    UpdateProductRemarkCountAndScoreByTopicSysNo(int.Parse(s.Trim()));
                }
            }
        }


        public static void UpdateTopic(TopicInfo info)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                   new SqlParameter("@SysNo", SqlDbType.Int),
                   new SqlParameter("@TopicType", SqlDbType.Int),
                   new SqlParameter("@Title", SqlDbType.NVarChar),
                   new SqlParameter("@TopicContent", SqlDbType.NText),
                   new SqlParameter("@IsTop", SqlDbType.Int),
                   new SqlParameter("@IsDigest", SqlDbType.Int),
                   new SqlParameter("@ReferenceType", SqlDbType.Int),
                   new SqlParameter("@ReferenceSysNo", SqlDbType.Int),
                   new SqlParameter("@TotalRemarkCount", SqlDbType.Int),
                   new SqlParameter("@TotalUsefulRemarkCount", SqlDbType.Int),
                   new SqlParameter("@Score", SqlDbType.Int),
                   new SqlParameter("@TotalComplainCount", SqlDbType.Int),
                   new SqlParameter("@Status", SqlDbType.Int),
                   new SqlParameter("@CreateCustomerSysNo", SqlDbType.Int),
                   new SqlParameter("@CreateDate", SqlDbType.DateTime),
                   new SqlParameter("@LastEditUserSysNo", SqlDbType.Int),
                   new SqlParameter("@LastEditDate", SqlDbType.DateTime)
             };

            parms[0].Value = info.SysNo;
            parms[1].Value = info.TopicType;
            parms[2].Value = info.Title;
            parms[3].Value = info.TopicContent;
            parms[4].Value = info.IsTop;
            parms[5].Value = info.IsDigest;
            parms[6].Value = info.ReferenceType;
            parms[7].Value = info.ReferenceSysNo;
            parms[8].Value = info.TotalRemarkCount;
            parms[9].Value = info.TotalUsefulRemarkCount;
            parms[10].Value = info.Score;
            parms[11].Value = info.TotalComplainCount;
            parms[12].Value = info.Status;
            parms[13].Value = info.CreateCustomerSysNo;
            parms[14].Value = info.CreateDate;
            parms[15].Value = info.LastEditUserSysNo;
            parms[16].Value = info.LastEditDate;

            SqlCommand cmd = new SqlCommand(SQL_UPDATE_TOPIC);
            cmd.Parameters.AddRange(parms);
            cmd.CommandTimeout = 180;
            SqlHelper.ExecuteNonQuery(cmd);
        }

        public static void DeleteTopic(int SysNo)
        {
            SqlParameter[] parms = new SqlParameter[1];

            parms[0] = new SqlParameter("@SysNo", SqlDbType.Int);
            parms[0].Value = SysNo;

            SqlCommand cmd = new SqlCommand(SQL_DELETE_TOPIC);
            cmd.Parameters.AddRange(parms);
            cmd.CommandTimeout = 180;
            SqlHelper.ExecuteNonQuery(cmd);

        }

        public static void ChangeTopicStstus(int sysNo, AppEnum.TopicStatus st)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                   new SqlParameter("@Status", SqlDbType.Int),
                   new SqlParameter("@SysNo", SqlDbType.Int)
             };
            parms[0].Value = (int)st;
            parms[1].Value = sysNo;

            SqlCommand cmd = new SqlCommand(SQL_CHANGE_TOPIC_STATUS);
            cmd.Parameters.AddRange(parms);
            cmd.CommandTimeout = 180;
            SqlHelper.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// 获得 Topic 总数
        /// </summary>
        /// <returns></returns>
        public static int GetTotalTopicCount()
        {
            string sql = "SELECT COUNT(*) AS TotalCount FROM Topic_Master (NOLOCK) WHERE Status <> "
                + (int)AppEnum.TopicStatus.Abandon;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(sql));
        }

        public static DataSet SearchProductTopic(TopicSearchCondition SearchCondition)
        {
            //DataSet ds = new DataSet();
            string sql = @"@select Topic_Master.SysNo as SysNo ,Topic_Master.ReferenceType as ReferenceType,
								  Topic_Master.ReferenceSysNo as ReferenceSysNo,
                                  Product.ProductName as ProductName, Customer.CustomerName as CustomerName,Customer.CustomerID as CustomerID,							
                                  Topic_Master.TotalRemarkCount as TotalRemarkCount,Topic_Master.TotalUsefulRemarkCount as TotalUsefulRemarkCount,Topic_Master.TotalComplainCount as TotalComplainCount,
                                  Topic_Master.CreateDate as CreateDate , Topic_Master.Title as Title , Topic_Master.TopicContent as TopicContent ,
                                  Topic_Master.Status as Status , Sys_User.UserName as UpdateUserName , Topic_Master.LastEditDate as LastEditDate , Product.ProductID as ProductID,
                                  Topic_Master.TopicType as TopicType,Topic_Master.IsTop as IsTop,Topic_Master.IsDigest as IsDigest,Topic_Master.Score as Score,
                                  Topic_Master.CreateCustomerSysNo as CreateCustomerSysNo,Topic_Master.LastEditUserSysNo as LastEditUserSysNo,Topic_Master.LastEditDate as LastEditDate,
                                  (select count(*) FROM Topic_Reply (NOLOCK) WHERE Topic_Reply.TopicSysNo=Topic_Master.SysNo) as ReplyCount,
								  (select count(*) FROM Topic_Image (NOLOCK) WHERE Topic_Image.TopicSysNo=Topic_Master.SysNo) as ImageCount
                           from  Topic_Master (NOLOCK) 
                                 inner join Customer (NOLOCK) on Customer.SysNo = Topic_Master.CreateCustomerSysNo
                                 inner join Product (NOLOCK) on Product.SysNo = Topic_Master.ReferenceSysNo                                
                                 left join Sys_User (NOLOCK) on Sys_User.SysNo = Topic_Master.LastEditUserSysNo 
                           where 1=1 @sqlPar order by Topic_Master.CreateDate desc";

            SqlParameter[] paras = new SqlParameter[]
            {
                   new SqlParameter("@DateFrom", SqlDbType.DateTime),
                   new SqlParameter("@DateTo", SqlDbType.DateTime),
                   new SqlParameter("@ProductSysNo", SqlDbType.Int),
                   new SqlParameter("@Status", SqlDbType.Int),
                   new SqlParameter("@PMSysNo", SqlDbType.Int),
                   new SqlParameter("@ProductStatus", SqlDbType.Int),
                   new SqlParameter("@IsDigest", SqlDbType.Int),
                   new SqlParameter("@IsTop", SqlDbType.Int),
                   new SqlParameter("@TopicType", SqlDbType.Int)
            };

            paras[0].Value = DateTime.Today;
            paras[1].Value = DateTime.Today;
            paras[2].Value = 0;
            paras[3].Value = 0;
            paras[4].Value = 0;
            paras[5].Value = 0;
            paras[6].Value = 0;
            paras[7].Value = 0;
            paras[8].Value = 0;


            StringBuilder sb = new StringBuilder();
            if (SearchCondition.DateFrom != null)
            {
                sb.Append(" and Topic_Master.CreateDate>= @DateFrom ");
                paras[0].Value = SearchCondition.DateFrom;
            }
            if (SearchCondition.DateTo != null)
            {
                sb.Append(" and Topic_Master.CreateDate<= @DateTo ");
                paras[1].Value = SearchCondition.DateTo.Value.AddDays(1).AddSeconds(-1);
            }
            if (SearchCondition.ProductSysNo != null)
            {
                sb.Append(" and Topic_Master.ReferenceSysNo=@ProductSysNo ");
                paras[2].Value = SearchCondition.ProductSysNo;
            }
            if (SearchCondition.Status != null)
            {
                sb.Append(" and Topic_Master.Status=@Status ");
                paras[3].Value = SearchCondition.Status;
            }
            if (SearchCondition.ProductStatus != null)
            {
                sb.Append(" and Product.Status = @ProductStatus ");
                paras[5].Value = SearchCondition.ProductStatus;
            }
            if (SearchCondition.IsDigest != null)
            {
                sb.Append(" and Topic_Master.IsDigest=@IsDigest ");
                paras[6].Value = (SearchCondition.IsDigest == true) ? 1 : 0;
            }
            if (SearchCondition.IsTop != null)
            {
                sb.Append(" and Topic_Master.IsTop=@IsTop ");
                paras[7].Value = (SearchCondition.IsTop == true) ? 1 : 0;
            }

            if (SearchCondition.TypeOfTopic != null)
            {
                sb.Append(" and Topic_Master.TopicType=@TopicType ");
                paras[8].Value = SearchCondition.TypeOfTopic;
            }


            if (SearchCondition.CustomerId != string.Empty)
                sb.Append(" and Customer.CustomerID='" + SearchCondition.CustomerId + "'");

            if (SearchCondition.ScoreSign != string.Empty)
                sb.Append(" and Topic_Master.Score" + SearchCondition.ScoreSign);
            if (SearchCondition.IsComplain != null)
            {
                if (SearchCondition.IsComplain == true)
                    sb.Append(" and Topic_Master.TotalComplainCount>0 ");
                if (SearchCondition.IsComplain == false)
                    sb.Append(" and Topic_Master.TotalComplainCount=0 ");
            }

            if (SearchCondition.InEmpty == true)
                sql = sql.Replace("@select", "select top 50 ");
            else
                sql = sql.Replace("@select", "select ");

            sql = sql.Replace("@sqlPar", sb.ToString());

            return SqlHelper.ExecuteDataSet(sql, paras);
        }

        #endregion

        //        #region ＂我的话题＂列表

        //        /// <summary>
        //        /// 获得注册用户的 Topic 列表，不包括回复具体内容。
        //        /// 用于“我的主题”列表。
        //        /// </summary>
        //        /// <param name="customerSysNo">Customer SysNo</param>
        //        /// <returns></returns>
        //        public static DataTable GetCustomerTopicList(int customerSysNo)
        //        {
        //            string sql = @"SELECT TopicType, Title, 
        //                CASE ReferenceType WHEN 0 THEN (SELECT ProductName FROM Product (NOLOCK) WHERE SysNo = ReferenceSysNo) ELSE 'N/A' END as ReferenceName,
        //                ReferenceSysNo, Topic_Master.Status, CreateDate,
        //                (SELECT COUNT(*) FROM dbo.Topic_Reply (NOLOCK) WHERE TopicSysNo = Topic_Master.SysNo AND Status <> 1) AS ReplyCount
        //                FROM dbo.Topic_Master (NOLOCK), dbo.Product (NOLOCK) 
        //                WHERE Topic_Master.ReferenceSysNo = Product.SysNo AND Topic_Master.ReferenceType = 0
        //                AND Product.Status = 1 
        //				AND CreateCustomerSysNo = @CreateCustomerSysNo
        //                ORDER BY CreateDate DESC";

        //            SqlParameter[] parms = new SqlParameter[1];
        //            parms[0] = new SqlParameter("@CreateCustomerSysNo", SqlDbType.Int);
        //            parms[0].Value = customerSysNo;

        //            return SqlHelper.ExecuteDataSet(sql, parms).Tables[0];
        //        }

        //        /// <summary>
        //        /// 获得注册用户的 Topic Reply 列表，
        //        /// 用于“我的回复”列表。
        //        /// </summary>
        //        /// <param name="customerSysNo">Customer SysNo</param>
        //        /// <returns></returns>
        //        public static DataTable GetCustomerTopicReplyList(int customerSysNo)
        //        {
        //            string sql = @"SELECT Topic_Master.TopicType, Topic_Master.Title,
        //                CASE Topic_Master.ReferenceType WHEN 0 THEN (SELECT ProductName FROM Product (NOLOCK) WHERE SysNo = ReferenceSysNo) ELSE 'N/A' END as ReferenceName,
        //                Topic_Master.ReferenceSysNo, Topic_Master.Status, Topic_Master.CreateDate,
        //                (SELECT COUNT(*) FROM dbo.Topic_Reply WHERE TopicSysNo = Topic_Master.SysNo AND Status <> 1) AS ReplyCount
        //                FROM dbo.Topic_Master (NOLOCK), IPP3.dbo.Product (NOLOCK)
        //                WHERE Topic_Master.ReferenceSysNo = Product.SysNo AND Topic_Master.ReferenceType = 0
        //                AND Product.Status = 1 
        //                AND Topic_Master.SysNo IN (SELECT Topic_Master FROM dbo.Topic_Reply (NOLOCK) WHERE CreateUserType = 0 AND CreateUserSysNo = @CreateCustomerSysNo AND Status <> 1)
        //                ORDER BY Topic_Master.CreateDate DESC";

        //            SqlParameter[] parms = new SqlParameter[1];
        //            parms[0] = new SqlParameter("@CreateCustomerSysNo", SqlDbType.Int);
        //            parms[0].Value = customerSysNo;

        //            return SqlHelper.ExecuteDataSet(sql, parms).Tables[0];
        //        }

        //        #endregion

        //        #region 随机获得精华Topic，用于在 Homepage，Category 3 Index，Product Detail 页面显示。

        //        /// <summary>
        //        /// 随机获得精华 Topic 列表，
        //        /// （限制条件: 一个月内，）
        //        /// 用于在 Homepage，Product Detail 页面显示
        //        /// </summary>
        //        /// <param name="count">Topic 数目</param>
        //        /// <param name="place">显示随机精华 Topic 的页面类型（位置）</param>
        //        /// <param name="refSysNo">关联的 SysNo （ ProductSysNo）</param>
        //        /// <returns></returns>
        //        public static DataTable GetRandomDigestTopicList(int count, AppEnum.RandomDigestTopicPage place, int refSysNo)
        //        {
        //            if (count > 100 || count <= 0)
        //                count = 1;

        //            string sql = @"SELECT TOP @TOP_COUNT topic.*, cust.CustomerId, cust.CustomerRank AS CustomerRank,
        //                CASE ReferenceType WHEN 0 THEN (SELECT ProductName FROM Product (NOLOCK) WHERE SysNo = ReferenceSysNo) ELSE 'N/A' END AS ReferenceName,
        //                CASE ReferenceType WHEN 0 THEN (SELECT ProductId FROM Product (NOLOCK) WHERE SysNo = ReferenceSysNo) ELSE 'N/A' END AS ProductId  
        //                FROM Topic_Master topic (NOLOCK) 
        //                LEFT JOIN IPP3.dbo.Customer cust (NOLOCK) ON cust.SysNo = CreateCustomerSysNo
        //                LEFT JOIN IPP3.dbo.Product product (NOLOCK) ON product.SysNo = topic.ReferenceSysNo
        //                WHERE IsDigest = 1 AND CanRandomSelected = 1 AND topic.Status <> 2
        //                AND Product.Status = 1
        //                AND CreateDate > DATEADD(MONTH,-3,GETDATE())
        //                @QUERY_CONDITION
        //                ORDER BY NEWID()";

        //            sql = sql.Replace("@TOP_COUNT", count.ToString());

        //            switch (place)
        //            {
        //                case AppEnum.RandomDigestTopicPage.Homepage:
        //                    sql = sql.Replace("@QUERY_CONDITION", "");
        //                    break;
        //                case AppEnum.RandomDigestTopicPage.ProductDetail:
        //                    sql = sql.Replace("@QUERY_CONDITION", "AND ReferenceType = 0 AND ReferenceSysNo = @RefSysNo");
        //                    break;
        //                default:
        //                    throw new ArgumentException("Place is invalid.");
        //            }

        //            SqlParameter[] parms = new SqlParameter[1];
        //            parms[0] = new SqlParameter("@RefSysNo", SqlDbType.Int);
        //            parms[0].Value = refSysNo;

        //            DataTable dt = SqlHelper.ExecuteDataSet(sql, parms).Tables[0];

        //            if (dt == null || dt.Rows.Count == 0)
        //                return null;

        //            return dt;
        //        }

        //        #endregion

        /// <summary>
        /// 提交回复的时候更新商品表
        /// </summary>
        /// <param name="topicSysNo"></param>
        public static void UpdateProductRemarkCountAndScoreByTopicSysNo(int topicSysNo)
        {
            string sql =
                @"SELECT ReferenceSysNo FROM Topic_Master (NOLOCK) WHERE ReferenceType = 0 AND SysNo = @SysNo";
            SqlParameter[] parms = new SqlParameter[1];
            parms[0] = new SqlParameter("@SysNo", SqlDbType.Int);
            parms[0].Value = topicSysNo;

            DataTable dt = SqlHelper.ExecuteDataSet(sql, parms).Tables[0];
            if (dt == null || dt.Rows.Count != 1)
                return;
            UpdateProductRemarkCountAndScore(Convert.ToInt32(dt.Rows[0]["ReferenceSysNo"]));
        }

        /// <summary>
        /// 如果 Topic 类型 = 体验，
        /// 还要更新 Product 表的 Remark Score / Count 字段。
        /// </summary>
        /// <param name="productSysNo">Product SysNo</param>
        public static void UpdateProductRemarkCountAndScore(int productSysNo)
        {
            string sql = @"SELECT
                TopicCount = (SELECT COUNT(*) FROM dbo.Topic_Master (NOLOCK) WHERE ReferenceSysNo = @ProductSysNo AND ReferenceType = 0 AND Status > 0),
                ExpCount = (SELECT COUNT(*) FROM dbo.Topic_Master (NOLOCK) WHERE ReferenceSysNo = @ProductSysNo AND ReferenceType = 0 AND Status > 0 AND TopicType = 1),
				ReplyCount = (SELECT COUNT(tr.[SysNo]) FROM [dbo].[Topic_Reply] AS tr RIGHT JOIN [Topic_Master] AS tm ON [tm].[SysNo]=[tr].[TopicSysNo]
								WHERE tm.ReferenceSysNo = 1 AND tm.ReferenceType = 0 AND tm.Status > 0 AND tm.TopicType = 2),
				TotalTopicScore = (SELECT SUM(Score) FROM dbo.Topic_Master (NOLOCK) WHERE ReferenceSysNo = @ProductSysNo AND ReferenceType = 0 AND Status > 0 AND TopicType = 1 ) ";

            SqlParameter[] parms = new SqlParameter[1];
            parms[0] = new SqlParameter("@ProductSysNo", SqlDbType.Int);
            parms[0].Value = productSysNo;

            DataTable dt = SqlHelper.ExecuteDataSet(sql, parms).Tables[0];

            if (dt == null || dt.Rows.Count != 1)
                throw new ApplicationException("无法获得相关商品的评论统计信息。");

            int topicCount = Convert.ToInt32(dt.Rows[0]["TopicCount"]);
            int expCount = Convert.ToInt32(dt.Rows[0]["ExpCount"]);
            int ReplyCount = Convert.ToInt32(dt.Rows[0]["ReplyCount"]);
            int score = 0;
            if (expCount > 0)
                score = (int)(Convert.ToInt32(dt.Rows[0]["TotalTopicScore"]) / expCount);


            string update = "UPDATE dbo.Product_Comment SET TopicCount = @Count, ReplyCount=@ReplyCount, ExpCount=@ExpCount, ExpAvgScore = @Score WHERE ProductSysNo = @ProductSysNo";

            SqlParameter[] parms2 = new SqlParameter[5];
            parms2[0] = new SqlParameter("@Count", SqlDbType.Int);
            parms2[1] = new SqlParameter("@Score", SqlDbType.Int);
            parms2[2] = new SqlParameter("@ProductSysNo", SqlDbType.Int);
            parms2[3] = new SqlParameter("@ReplyCount", SqlDbType.Int);
            parms2[4] = new SqlParameter("@ExpCount", SqlDbType.Int);

            parms2[0].Value = topicCount;
            parms2[1].Value = score;
            parms2[2].Value = productSysNo;
            parms2[3].Value = ReplyCount;
            parms2[4].Value = expCount;

            SqlCommand cmd = new SqlCommand(update);
            cmd.Parameters.AddRange(parms2);
            cmd.CommandTimeout = 180;
            SqlHelper.ExecuteNonQuery(cmd);
        }
    }
}