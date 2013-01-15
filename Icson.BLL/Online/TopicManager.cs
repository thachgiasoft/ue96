using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Transactions;
using Icson.DBAccess;
using Icson.DBAccess.Online;
using Icson.Objects;
using Icson.Objects.Online;
using Icson.Utils;

namespace Icson.BLL.Online
{
    public class TopicManager
    {
        private TopicManager()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private static TopicManager _instance;

        public static TopicManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new TopicManager();
            }
            return _instance;
        }

        public static List<TopicInfo> GetProductTop3TopicList(int productSysNo)
        {
            int totalCount;

            return TopicDac.GetAllTopicByProductId(productSysNo, 0, 0, out totalCount, "Top3");
        }

        public static List<TopicInfo> GetProductTopicList(int productSysNo, int pageSize, int pageIndex, out int totalCount, string type)
        {
            return TopicDac.GetAllTopicByProductId(productSysNo, pageSize, pageIndex, out totalCount, type);
        }

        public static TopicInfo GetTopicById(int TopicId)
        {
            return TopicDac.GetTopicByTopicId(TopicId);
        }
        public static DataSet SearchProductTopics(TopicSearchCondition SearchCondition)
        {
            return TopicDac.SearchProductTopic(SearchCondition);
        }
        public static List<TopicReplyInfo> GetTopicReplyList(int topicSysNo)
        {
            return TopicReplyDac.GetTopicReplyByTopicId(topicSysNo);
        }
        public static List<TopicImageInfo> GetTopicImageList(int topicSysNo)
        {
            return TopicImageDac.GetTopicImagesByTopicId(topicSysNo);
        }
        public static List<TopicImageInfo> GetValidTopicImageByTopicId(int topicSysNo)
        {
            return TopicImageDac.GetValidTopicImageByTopicId(topicSysNo);
        }
        public static TopicImageInfo GetTopicImage(int SysNo)
        {
            return TopicImageDac.GetTopicImageBySysNo(SysNo);
        }

        public static DataSet GetTopicComplain(int topicSysNo)
        {
            return TopicComplainDac.LoadComplainByTopicSysNo(topicSysNo);
        }
        public static int AddTopicImageCount(int SysNo)
        {
            return TopicImageDac.AddTopicImageCount(SysNo);
        }

        #region  Remark Topic (评论是否有用) 管理

        /// <summary>
        /// 设置评论是否有用
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static int SetRemarkUseful(TopicRemarkInfo info)
        {
            if (TopicRemarkDac.IsExistRemark(info))
                return -1; // 已存在当前用户的评价

            TopicRemarkDac.InsertTopicRemark(info);

            #region 自动设为精华功能，（包括给客户添加积分）
            //// 达到一定分数之后，自动设置为精华。
            //if (Can_AutoSetDigest == true
            //    && CanAutoSetTopicDigest(info.TopicSysNo) == true)
            //{
            //    TopicSetDigset(info.TopicSysNo, 0);

            //    // 奖励积分，仅限于顾客
            //    if (PointBonus_AutoSetDigest > 0 && info.CreateUserType == CreateUserType.Customer)
            //    {
            //        string strLog = "评论ID:" + info.TopicSysNo + ", 顾客ID:" + info.CreateUserSysNo + ", 时间:" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            //        PointManager.GetInstance().SetScore(
            //            info.CreateUserSysNo,
            //            TopicManager.PointBonus_AutoSetDigest,
            //            (int)AppEnum.PointLogType.SetScoreAuto,
            //            strLog);
            //    }
            //}
            #endregion
            return 0;  // 更新评价成功                
        }

        #region 评论作废与取消作废
        public static void AbandonTopic(int topicSysNo, int userSysNo)
        {
            TopicDac.UpdateTopicByType(topicSysNo, userSysNo, AppEnum.TopicUpdateType.AbandonTopic);
        }

        public static void CancelAbandonTopic(int topicSysNo, int userSysNo)
        {
            TopicDac.UpdateTopicByType(topicSysNo, userSysNo, AppEnum.TopicUpdateType.CancelAbandonTopic);
        }
        #endregion
        #region
        public static void ConfirmTopic(int topicSysNo, int userSysNo)
        {
            TopicDac.UpdateTopicByType(topicSysNo, userSysNo, AppEnum.TopicUpdateType.ConfirmTopic);
        }
        public static void UnConfirmTopic(int topicSysNo, int userSysNo)
        {
            TopicDac.UpdateTopicByType(topicSysNo, userSysNo, AppEnum.TopicUpdateType.UnConfirmTopic);
        }
        #endregion

        #region 评论图片作废与取消作废
        public static void AbandonTopicImage(int topicImageSysNo)
        {
            TopicImageDac.AbandTopicImage(topicImageSysNo, AppEnum.TopicImageStatus.Abandon);
        }
        public static void CancelAbandonTopicImage(int topicImageSysNo)
        {
            TopicImageDac.AbandTopicImage(topicImageSysNo, AppEnum.TopicImageStatus.Normal);
        }

        #endregion
        #region 回复作废与取消作废
        public static void AbandonTopicReply(int topicReplySysNo, int userSysNo)
        {
            TopicReplyDac.AbandTopicReply(topicReplySysNo, userSysNo, AppEnum.TopicReplyStatus.Abandon);
        }

        public static void CancelAbandonTopicReply(int topicReplySysNo, int userSysNo)
        {
            TopicReplyDac.AbandTopicReply(topicReplySysNo, userSysNo, AppEnum.TopicReplyStatus.Normal);
        }
        #endregion
        #region 设置/取消 置顶或者精华

        #endregion
        public static void TopicSetTop(int topicSysNo, int userSysNo)
        {
            TopicDac.UpdateTopicByType(topicSysNo, userSysNo, AppEnum.TopicUpdateType.TopicSetTop);
        }

        public static void TopicSetDigset(int topicSysNo, int userSysNo)
        {
            TopicDac.UpdateTopicByType(topicSysNo, userSysNo, AppEnum.TopicUpdateType.TopicSetDigset);
        }

        public static void TopicCancelTop(int topicSysNo, int userSysNo)
        {
            TopicDac.UpdateTopicByType(topicSysNo, userSysNo, AppEnum.TopicUpdateType.TopicCancelTop);
        }

        public static void TopicCancelDigset(int topicSysNo, int userSysNo)
        {
            TopicDac.UpdateTopicByType(topicSysNo, userSysNo, AppEnum.TopicUpdateType.TopicCancelDigset);
        }
        #endregion

        #region 获得 Product 评论统计信息

        public static void GetTopicStatisticalInfo(int sysNo, out int topicCount, out int replyCount, out int digestCount, out int score, out int scoreCount_5, out int scoreCount_4, out int scoreCount_3, out int scoreCount_2, out int scoreCount_1)
        {
            string sql = @"SELECT 
                (SELECT COUNT(*) FROM Topic_Master (NOLOCK) WHERE ReferenceType = 0 AND ReferenceSysNo = " + sysNo + " AND Status > 0) AS TopicCount," +
                " (SELECT COUNT(*) FROM Topic_Reply (NOLOCK) WHERE Status <> 1 AND TopicSysNo IN (SELECT SysNo FROM Topic_Master (NOLOCK) " +
                " WHERE ReferenceType = 0 AND ReferenceSysNo = " + sysNo + " AND Status > 0)) AS ReplyCount," +
                " (SELECT COUNT(*) FROM Topic_Master (NOLOCK) WHERE IsDigest = 1 AND ReferenceType = 0 AND ReferenceSysNo = " + sysNo + " AND Status > 0) AS DigestCount," +
                " isnull((SELECT Product_Comment.ExpAvgScore FROM Product (NOLOCK) left join Product_Comment on Product_Comment.ProductSysNo = product.sysno WHERE product.SysNo = " + sysNo + "), 0) AS Score," +
                " (SELECT COUNT(*) FROM Topic_Master (NOLOCK) WHERE Score = 5 AND TopicType = 1 AND ReferenceType = 0 AND ReferenceSysNo = " + sysNo + " AND Status > 0) AS ScoreCount_5," +
                " (SELECT COUNT(*) FROM Topic_Master (NOLOCK) WHERE Score = 4 AND TopicType = 1 AND ReferenceType = 0 AND ReferenceSysNo = " + sysNo + " AND Status > 0) AS ScoreCount_4," +
                " (SELECT COUNT(*) FROM Topic_Master (NOLOCK) WHERE Score = 3 AND TopicType = 1 AND ReferenceType = 0 AND ReferenceSysNo = " + sysNo + " AND Status > 0) AS ScoreCount_3," +
                " (SELECT COUNT(*) FROM Topic_Master (NOLOCK) WHERE Score = 2 AND TopicType = 1 AND ReferenceType = 0 AND ReferenceSysNo = " + sysNo + " AND Status > 0) AS ScoreCount_2," +
                " (SELECT COUNT(*) FROM Topic_Master (NOLOCK) WHERE Score = 1 AND TopicType = 1 AND ReferenceType = 0 AND ReferenceSysNo = " + sysNo + " AND Status > 0) AS ScoreCount_1";

            DataTable dt = SqlHelper.ExecuteDataSet(sql).Tables[0];

            topicCount = Convert.ToInt32(dt.Rows[0]["topicCount"]);
            replyCount = Convert.ToInt32(dt.Rows[0]["replyCount"]);
            digestCount = Convert.ToInt32(dt.Rows[0]["digestCount"]);
            score = Convert.ToInt32(dt.Rows[0]["score"]);
            scoreCount_5 = Convert.ToInt32(dt.Rows[0]["scoreCount_5"]);
            scoreCount_4 = Convert.ToInt32(dt.Rows[0]["scoreCount_4"]);
            scoreCount_3 = Convert.ToInt32(dt.Rows[0]["scoreCount_3"]);
            scoreCount_2 = Convert.ToInt32(dt.Rows[0]["scoreCount_2"]);
            scoreCount_1 = Convert.ToInt32(dt.Rows[0]["scoreCount_1"]);
        }

        #endregion

        /// <summary>
        /// 检查该用户的当天主题数量是否达到了上限
        /// </summary>
        /// <param name="customerSysNo">Customer SysNo</param>
        /// <returns></returns>
        public static bool IsTopicCountLimitToday(int customerSysNo)
        {
            return TopicDac.ReturnNumOfTopicByCustomerId(customerSysNo) >= AppConfig.Topic.PostTopic_Everyday_CountLimit;
        }

        /// <summary>
        /// 检查该用户的当天回复数量是否达到了上限
        /// </summary>
        /// <param name="customerSysNo">Customer SysNo</param>
        /// <returns></returns>
        public static bool IsReplyCountLimitToday(int customerSysNo)
        {
            return TopicReplyDac.ReturnNumOfReplyByCustomerId(customerSysNo) >= AppConfig.Topic.PostReply_Everyday_CountLimit;
        }

        /// <summary>
        /// 根据用户权限获取用户可以上传图片的数量
        /// </summary>
        /// <param name="cusRank"></param>
        /// <returns></returns>
        public static int GetNumOfUpImageByCustomerRank(AppEnum.CustomerRank cusRank)
        {
            AppEnum.TopicCustomerRight topicCusRight = GetTopicCustomerRight(cusRank);
            int outNum = 0;
            switch (topicCusRight)
            {
                case AppEnum.TopicCustomerRight.A1:
                    outNum = 1;
                    break;

                case AppEnum.TopicCustomerRight.B2:
                    outNum = 3;
                    break;
                case AppEnum.TopicCustomerRight.C3:
                    outNum = 5;
                    break;

            }
            return outNum;
        }

        #region Topic / Reply Operation (Add New, Remark Userful, Complain 等)

        public static void AddNewTopic(TopicInfo topic)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                int topicSysNo = TopicDac.InsertTopic(topic);

                // 添加了新评论，
                // 同时更新 Product 表的 RemarkCount / RemarkScore 字段。
                TopicDac.UpdateProductRemarkCountAndScore(topic.ReferenceSysNo);

                if (topic.ImageList != null && topic.ImageList.Count > 0)
                    foreach (TopicImageInfo info in topic.ImageList)
                    {
                        info.TopicSysNo = topicSysNo;
                        TopicImageDac.InsertTopicImage(info);
                    }

                scope.Complete();
            }
        }

        public static void AddTopicReply(TopicReplyInfo reply)
        {
            TopicReplyDac.InsertTopicReply(reply);
            TopicDac.UpdateProductRemarkCountAndScoreByTopicSysNo(reply.TopicSysNo);
        }

        public static void AddTopicReplyBySysUser(TopicReplyInfo reply, AppEnum.TopicStatus ts)
        {
            TopicReplyDac.InsertTopicReply(reply);

            TopicDac.ChangeTopicStstus(reply.TopicSysNo, AppEnum.TopicStatus.Replyed);
        }
        public static void ComplainTopic(TopicComplainInfo complain)
        {
            TopicComplainDac.InsertTopicComplain(complain);
        }
        #endregion

        #region  TopicComplain管理

        // 投诉评论
        public static int SetComplain(TopicComplainInfo info)
        {
            if (TopicComplainDac.IsExistComplain(info))
                return -1; //已存在当前用户的投诉
            else
            {
                TopicComplainDac.InsertTopicComplain(info);
                return 0;  //投诉成功
            }

        }
        #endregion

        #region 用户权限管理

        /// <summary>
        /// MAP评论系统中客户的对应权限
        /// </summary>
        /// <param name="cRank"></param>
        /// <returns></returns>
        public static AppEnum.TopicCustomerRight GetTopicCustomerRight(AppEnum.CustomerRank cRank)
        {
            AppEnum.TopicCustomerRight oRes = AppEnum.TopicCustomerRight.A1;
            switch (cRank)
            {
                case AppEnum.CustomerRank.OneStar:
                case AppEnum.CustomerRank.TwoStar:
                case AppEnum.CustomerRank.ThreeStar:
                    oRes = AppEnum.TopicCustomerRight.A1;
                    break;
                case AppEnum.CustomerRank.FourStar:
                case AppEnum.CustomerRank.FiveStar:
                case AppEnum.CustomerRank.Golden:
                    oRes = AppEnum.TopicCustomerRight.B2;
                    break;
                case AppEnum.CustomerRank.Diamond:
                case AppEnum.CustomerRank.VIP:
                    oRes = AppEnum.TopicCustomerRight.C3;
                    break;
            }
            return oRes;
        }

        /// <summary>
        /// 根据用户sysno和产品sysno判断该用户是否可以对该商品进行评论：
        /// 购买商品次数大于评论次数，可以对商品进行评论(Type=经验)
        /// </summary>
        /// <param name="csysno">Customer SysNo</param>
        /// <param name="psysno">Product SysNo</param>
        /// <returns></returns>
        public static bool ExamProductRemark(int csysno, int psysno)
        {
            //Chris:TopicManager.cs--测试时，先直接返回True
            //return true;


            int sCount;   //购买商品次数
            int rCount;   //评论次数
            string sqls = @"SELECT *  FROM SO_Master (NOLOCK) INNER JOIN SO_Item (NOLOCK) ON SO_Item.SOSysNo = SO_Master.SysNo
                           WHERE SO_Master.CustomerSysNo = " + csysno + " AND SO_Item.ProductSysNo = " + psysno + " AND SO_Master.Status ="
                + (int)AppEnum.SOStatus.OutStock;

            DataSet ds = SqlHelper.ExecuteDataSet(sqls);
            if (!Util.HasMoreRow(ds))
                return false;
            else
                sCount = ds.Tables[0].Rows.Count;

            string sqlr = @"SELECT * FROM Topic_Master (NOLOCK) WHERE TopicType = " + (int)AppEnum.TopicType.Experience + " AND CreateCustomerSysNo = " + csysno
                + " AND ReferenceSysNo = " + psysno + " and ReferenceType = " + (int)AppEnum.TopicReferenceType.Product;
            ds = SqlHelper.ExecuteDataSet(sqlr);
            rCount = ds.Tables[0].Rows.Count;

            if (sCount <= rCount)
                return false;
            else
            {
                sqls = sqls + " and outtime >= cast('" + DateTime.Now.AddMonths(-3) + "' as datetime)";
                ds = SqlHelper.ExecuteDataSet(sqls);
                if (ds.Tables[0].Rows.Count > 0)
                    return true;
                else
                    return false;
            }
        }

        #endregion

        public static string GetScorePic(int score)
        {
            StringBuilder sb = new StringBuilder();
            if (score < 0 || score > 5)
                throw new Exception("评论得分不在0-5范围内。");
            return "<img src='../images/Topic/ays_star" + score + ".gif'/>";
            //for (int i = 0; i < score; i++)
            //{
            //    sb.Append("<img src='../images/Topic/ays_star.gif' width='13' height='14' align='absmiddle' />");
            //}
            //return sb.ToString();
        }
    }
}