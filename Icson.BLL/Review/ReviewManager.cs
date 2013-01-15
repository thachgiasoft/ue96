using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Transactions;
using Icson.BLL.Online;
using Icson.DBAccess;
using Icson.DBAccess.Review;
using Icson.Objects;
using Icson.Objects.Review;
using Icson.Utils;

namespace Icson.BLL.Review
{
    public class ReviewManager
    {
        private ReviewManager()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private static ReviewManager _instance;

        public static ReviewManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ReviewManager();
            }
            return _instance;
        }

        public int InsertReviewMaster(ReviewInfo oParam)
        {
            oParam.SysNo = SequenceDac.GetInstance().Create("Review_Sequence");
            return new ReviewDac().Insert(oParam);
        }

        public int UpdateReviewMaster(ReviewInfo oParam)
        {
            return new ReviewDac().Update(oParam);
        }

        public ReviewInfo LoadReviewMaster(int ReviewSysNo)
        {
            string sql = "select * from review_master where sysno=" + ReviewSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                ReviewInfo o = new ReviewInfo();
                map(o, ds.Tables[0].Rows[0]);
                return o;
            }
            else
                return null;
        }

        public bool HasReviewRemarkedByUser(int reviewSysNo, int createUserType, int createUserSysNo)
        {
            string sql = @"select * from review_remark where reviewSysNo=" + reviewSysNo + " and createUserType=" + createUserType + " and createUserSysNo=" + createUserSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                return true;
            else
                return false;
        }

        public void AddNewReviewRemark(ReviewRemarkInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                string sql = @"select * from review_remark where reviewSysNo=" + oParam.ReviewSysNo + " and createUserType=" + oParam.CreateUserType + " and createUserSysNo=" + oParam.CreateUserSysNo;
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (Util.HasMoreRow(ds))
                    throw new BizException("请勿重复提交");

                int iInsert = InsertReviewRemark(oParam);
                if (iInsert == 0)
                    throw new BizException("提交失败");

                if (oParam.IsHelpful == (int)AppEnum.YNStatus.Yes)
                {
                    sql = "update review_master set totalremarkcount=totalremarkcount+1,totalhelpfulremarkcount=totalhelpfulremarkcount+1 where sysno=" + oParam.ReviewSysNo;
                }
                else
                {
                    sql = "update review_master set totalremarkcount=totalremarkcount+1 where sysno=" + oParam.ReviewSysNo;
                }
                SqlHelper.ExecuteNonQuery(sql);

                scope.Complete();
            }
        }

        public int InsertReviewRemark(ReviewRemarkInfo oParam)
        {
            return new ReviewRemarkDac().Insert(oParam);
        }

        public int UpdateReviewRemark(ReviewRemarkInfo oParam)
        {
            return new ReviewRemarkDac().Update(oParam);
        }

        public int InsertReviewComplain(ReviewComplainInfo oParam)
        {
            return new ReviewComplainDac().Insert(oParam);
        }

        public int UpdateReviewComplain(ReviewComplainInfo oParam)
        {
            return new ReviewComplainDac().Update(oParam);
        }

        public void AddNewReviewComplain(ReviewComplainInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                string sql = @"select * from review_complain where reviewSysNo=" + oParam.ReviewSysNo + " and createUserType=" + oParam.CreateUserType + " and createUserSysNo=" + oParam.CreateUserSysNo;
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (Util.HasMoreRow(ds))
                    throw new BizException("请勿重复提交");

                int iInsert = InsertReviewComplain(oParam);
                if (iInsert == 0)
                    throw new BizException("提交失败");

                sql = "update review_master set totalcomplaincount=totalcomplaincount+1 where sysno=" + oParam.ReviewSysNo;
                SqlHelper.ExecuteNonQuery(sql);

                scope.Complete();
            }
        }

        public int InsertReviewReply(ReviewReplyInfo oParam)
        {
            return new ReviewReplyDac().Insert(oParam);
        }

        public int UpdateReviewReply(ReviewReplyInfo oParam)
        {
            return new ReviewReplyDac().Update(oParam);
        }

        public ReviewReplyInfo LoadReviewReply(int ReviewReplySysNo)
        {
            string sql = "select * from review_reply where sysno=" + ReviewReplySysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                ReviewReplyInfo o = new ReviewReplyInfo();
                map(o, ds.Tables[0].Rows[0]);
                return o;
            }
            else
                return null;
        }

        public int InsertReviewC3ItemScore(ReviewC3ItemScoreInfo oParam)
        {
            return new ReviewC3ItemScoreDac().Insert(oParam);
        }

        public int UpdateReviewC3ItemScore(ReviewC3ItemScoreInfo oParam)
        {
            return new ReviewC3ItemScoreDac().Update(oParam);
        }

        private void map(ReviewInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.ReviewType = Util.TrimIntNull(tempdr["ReviewType"]);
            oParam.Title = Util.TrimNull(tempdr["Title"]);
            oParam.Content1 = Util.TrimNull(tempdr["Content1"]);
            oParam.Content2 = Util.TrimNull(tempdr["Content2"]);
            oParam.Content3 = Util.TrimNull(tempdr["Content3"]);
            oParam.Score = Util.TrimIntNull(tempdr["Score"]);
            oParam.OwnedType = Util.TrimIntNull(tempdr["OwnedType"]);
            oParam.UnderstandingType = Util.TrimIntNull(tempdr["UnderstandingType"]);
            oParam.NickName = Util.TrimNull(tempdr["NickName"]);
            oParam.ReferenceType = Util.TrimIntNull(tempdr["ReferenceType"]);
            oParam.ReferenceSysNo = Util.TrimIntNull(tempdr["ReferenceSysNo"]);
            oParam.IsTop = Util.TrimIntNull(tempdr["IsTop"]);
            oParam.IsGood = Util.TrimIntNull(tempdr["IsGood"]);
            oParam.TotalRemarkCount = Util.TrimIntNull(tempdr["TotalRemarkCount"]);
            oParam.TotalHelpfulRemarkCount = Util.TrimIntNull(tempdr["TotalHelpfulRemarkCount"]);
            oParam.TotalComplainCount = Util.TrimIntNull(tempdr["TotalComplainCount"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
            oParam.CreateCustomerSysNo = Util.TrimIntNull(tempdr["CreateCustomerSysNo"]);
            oParam.CreateDate = Util.TrimDateNull(tempdr["CreateDate"]);
            oParam.LastEditUserSysNo = Util.TrimIntNull(tempdr["LastEditUserSysNo"]);
            oParam.LastEditDate = Util.TrimDateNull(tempdr["LastEditDate"]);
            oParam.CustomerIP = Util.TrimNull(tempdr["CustomerIP"]);
        }

        private void map(ReviewRemarkInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.ReviewSysNo = Util.TrimIntNull(tempdr["ReviewSysNo"]);
            oParam.IsHelpful = Util.TrimIntNull(tempdr["IsHelpful"]);
            oParam.CreateUserType = Util.TrimIntNull(tempdr["CreateUserType"]);
            oParam.CreateUserSysNo = Util.TrimIntNull(tempdr["CreateUserSysNo"]);
            oParam.CreateDate = Util.TrimDateNull(tempdr["CreateDate"]);
        }

        private void map(ReviewComplainInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.ReviewSysNo = Util.TrimIntNull(tempdr["ReviewSysNo"]);
            oParam.ComplainContent = Util.TrimNull(tempdr["ComplainContent"]);
            oParam.CreateUserType = Util.TrimIntNull(tempdr["CreateUserType"]);
            oParam.CreateUserSysNo = Util.TrimIntNull(tempdr["CreateUserSysNo"]);
            oParam.CreateDate = Util.TrimDateNull(tempdr["CreateDate"]);
        }

        private void map(ReviewReplyInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.ReviewSysNo = Util.TrimIntNull(tempdr["ReviewSysNo"]);
            oParam.ReplyContent = Util.TrimNull(tempdr["ReplyContent"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
            oParam.CreateUserType = Util.TrimIntNull(tempdr["CreateUserType"]);
            oParam.CreateUserSysNo = Util.TrimIntNull(tempdr["CreateUserSysNo"]);
            oParam.CreateDate = Util.TrimDateNull(tempdr["CreateDate"]);
            oParam.LastEditUserSysNo = Util.TrimIntNull(tempdr["LastEditUserSysNo"]);
            oParam.LastEditDate = Util.TrimDateNull(tempdr["LastEditDate"]);
        }

        private void map(ReviewC3ItemScoreInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.ReviewSysNo = Util.TrimIntNull(tempdr["ReviewSysNo"]);
            oParam.C3ReviewItemSysNo = Util.TrimIntNull(tempdr["C3ReviewItemSysNo"]);
            oParam.Score = Util.TrimIntNull(tempdr["Score"]);
            oParam.Weight = Util.TrimIntNull(tempdr["Weight"]);
        }

        public string GetItemReviewScoreDiv(int productSysNo)
        {
            string sql = @"select score,count(score) as qty from review_master 
                           where reviewtype=@reviewtype 
                           and referencetype=@referencetype
                           and referencesysno=@productsysno
                           and @status and score > 0 
                           group by score order by score desc";
            sql = sql.Replace("@reviewtype", ((int)AppEnum.ReviewType.Experience).ToString());
            sql = sql.Replace("@referencetype", ((int)AppEnum.ReviewReferenceType.Product).ToString());
            sql = sql.Replace("@productsysno", productSysNo.ToString());
            sql = sql.Replace("@status", "( status=" + (int)AppEnum.ReviewStatus.Confirmed + " or status=" + (int)AppEnum.ReviewStatus.Replyed + ")");

            string sql2 = @"select c3ri.name as ItemName,sum(c3is.score)/count(*) as AvgScore  from Category3_ReviewItem c3ri 
                            inner join Review_C3ItemScore c3is on c3ri.sysno=c3is.c3reviewitemsysno
                            inner join review_master rm on rm.sysno=c3is.reviewsysno 
                            where  rm.reviewtype=@reviewtype
                            and rm.referencetype=@referencetype 
                            and rm.referencesysno=@productsysno
                            and @status
                            and c3ri.status=@validStatus
                            group by c3ri.name,c3ri.ordernum
                            order by c3ri.ordernum";
            sql2 = sql2.Replace("@reviewtype", ((int)AppEnum.ReviewType.Experience).ToString());
            sql2 = sql2.Replace("@referencetype", ((int)AppEnum.ReviewReferenceType.Product).ToString());
            sql2 = sql2.Replace("@productsysno", productSysNo.ToString());
            sql2 = sql2.Replace("@status", "( rm.status=" + (int)AppEnum.ReviewStatus.Confirmed + " or rm.status=" + (int)AppEnum.ReviewStatus.Replyed + ")");
            sql2 = sql2.Replace("@validStatus", ((int)AppEnum.BiStatus.Valid).ToString());

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return "";

            DataSet ds2 = SqlHelper.ExecuteDataSet(sql2);

            int Score_5_Count = 0;  //5星
            int Score_4_Count = 0;  //4星
            int Score_3_Count = 0;  //3星
            int Score_2_Count = 0;  //2星
            int Score_1_Count = 0;  //1星
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (Util.TrimIntNull(dr["Score"]))
                {
                    case 10:
                    case 9:
                        Score_5_Count += Util.TrimIntNull(dr["qty"]);
                        break;
                    case 8:
                    case 7:
                        Score_4_Count += Util.TrimIntNull(dr["qty"]);
                        break;
                    case 6:
                    case 5:
                        Score_3_Count += Util.TrimIntNull(dr["qty"]);
                        break;
                    case 4:
                    case 3:
                        Score_2_Count += Util.TrimIntNull(dr["qty"]);
                        break;
                    case 2:
                    case 1:
                        Score_1_Count += Util.TrimIntNull(dr["qty"]);
                        break;
                }
            }
            int Score_All_Count = Score_5_Count + Score_4_Count + Score_3_Count + Score_2_Count + Score_1_Count;
            string Score_5_Per = "0";
            string Score_4_Per = "0";
            string Score_3_Per = "0";
            string Score_2_Per = "0";
            string Score_1_Per = "0";
            decimal Score_Avg = 0m;
            if (Score_All_Count != 0)
            {
                Score_5_Per = (100 * Convert.ToDecimal(Score_5_Count) / Convert.ToDecimal(Score_All_Count)).ToString(AppConst.DecimalToInt);
                Score_4_Per = (100 * Convert.ToDecimal(Score_4_Count) / Convert.ToDecimal(Score_All_Count)).ToString(AppConst.DecimalToInt);
                Score_3_Per = (100 * Convert.ToDecimal(Score_3_Count) / Convert.ToDecimal(Score_All_Count)).ToString(AppConst.DecimalToInt);
                Score_2_Per = (100 * Convert.ToDecimal(Score_2_Count) / Convert.ToDecimal(Score_All_Count)).ToString(AppConst.DecimalToInt);
                Score_1_Per = (100 * Convert.ToDecimal(Score_1_Count) / Convert.ToDecimal(Score_All_Count)).ToString(AppConst.DecimalToInt);
                Score_Avg = Convert.ToDecimal(10 * Score_5_Count + 8 * Score_4_Count + 6 * Score_3_Count + 4 * Score_2_Count + 2 * Score_1_Count) / Convert.ToDecimal(Score_All_Count);
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=panelr>");
            sb.Append("<div class=panelr_title>");
            sb.Append("<img src='../images/site/main/center/tt_cfpj.png' alt='客户评价' />");
            if (Score_Avg > 9m)
            {
                sb.Append("<img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' />");
            }
            else if (Score_Avg > 7m)
            {
                sb.Append("<img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' />");
            }
            else if (Score_Avg > 5m)
            {
                sb.Append("<img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' />");
            }
            else if (Score_Avg > 3m)
            {
                sb.Append("<img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' />");
            }
            else if (Score_Avg >= 1m)
            {
                sb.Append("<img src='../images/site/main/center/star1.gif' />");
            }

            sb.Append("</div>");
            sb.Append("<div class=panelr_content>");
            sb.Append("<div id=dr_sc_content>");
            sb.Append("<table width='100%' border=0 cellspacing=0 cellpadding=0>");
            sb.Append("<tr>");
            sb.Append("<td valign=top>");
            if (Util.HasMoreRow(ds2))
            {
                sb.Append("<table valign=top>");
                foreach (DataRow dr in ds2.Tables[0].Rows)
                {
                    decimal avgScore = decimal.Round(decimal.Parse(dr["AvgScore"].ToString()), 0);
                    sb.Append("<tr><td height=16>" + Util.TrimNull(dr["ItemName"]) + "</td><td><img src='../images/site/score_bar.gif' width=" + (avgScore * 10) + " height=9 /></td><td>" + avgScore + "分</td></tr>");
                }
                sb.Append("</table>");
            }

            sb.Append("</td>");
            sb.Append("<td><div style='layout-flow:vertical-ideographic'><hr size=1 noshade width=120></div></td>");
            sb.Append("<td valign=top>");
            sb.Append("<table valign=top>");
            sb.Append("<tr>");
            sb.Append("<td width=30 height=16>很好</td>");
            sb.Append("<td width=105><img src='../images/site/score_bar.gif' width=" + Score_5_Per + " height=9 /></td>");
            sb.Append("<td width=33 class=dr_sc_td_b>" + Score_5_Per + "%</td>");
            sb.Append("<td width=35>" + Score_5_Count + "票</td>");
            sb.Append("</tr>");

            sb.Append("<tr>");
            sb.Append("<td>还好</td>");
            sb.Append("<td><img src='../images/site/score_bar.gif' width=" + Score_4_Per + " height=9 /></td>");
            sb.Append("<td class=dr_sc_td_b>" + Score_4_Per + "%</td>");
            sb.Append("<td>" + Score_4_Count + "票</td>");
            sb.Append("</tr>");

            sb.Append("<tr>");
            sb.Append("<td>一般</td>");
            sb.Append("<td><img src='../images/site/score_bar.gif' width=" + Score_3_Per + " height=9 /></td>");
            sb.Append("<td class=dr_sc_td_b>" + Score_3_Per + "%</td>");
            sb.Append("<td>" + Score_3_Count + "票</td>");
            sb.Append("</tr>");

            sb.Append("<tr>");
            sb.Append("<td>不好</td>");
            sb.Append("<td><img src='../images/site/score_bar.gif' width=" + Score_2_Per + " height=9 /></td>");
            sb.Append("<td class=dr_sc_td_b>" + Score_2_Per + "%</td>");
            sb.Append("<td>" + Score_2_Count + "票</td>");
            sb.Append("</tr>");

            sb.Append("<tr>");
            sb.Append("<td>极差</td>");
            sb.Append("<td><img src='../images/site/score_bar.gif' width=" + Score_1_Per + " height=9 /></td>");
            sb.Append("<td class=dr_sc_td_b>" + Score_1_Per + "%</td>");
            sb.Append("<td>" + Score_1_Count + "票</td>");
            sb.Append("</tr>");
            sb.Append("</table>");

            sb.Append("</td></tr>");
            sb.Append("</table>");
            sb.Append("<div id=dr_sc_total>");
            sb.Append("<table width='100%' cellpadding=0 cellspacing=0>");
            sb.Append("<tr>");
            sb.Append("<td>共有" + Score_All_Count + "人评价</td>");
            sb.Append("<td width=90><a href='../Review/Remark.aspx?ItemID=" + productSysNo + "'><img src='../images/site/main/center/btn_pingj.gif' alt='我要评价' width=83 height=34 border=0 /></a></td>");
            sb.Append("</tr>");
            sb.Append("</table>");


            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");

            return sb.ToString();
        }

        /// <summary>
        /// 此方法经过修改，只用于查询 体验、讨论、推荐 (排除：提问)
        /// </summary>
        /// <param name="productSysNo"></param>
        /// <returns></returns>
        public string GetItemReviewDiv(int productSysNo)
        {  //ReviewType <> "+(int)AppEnum.ReviewType.Inquiry + @"
            string sql = @"select top 8 *
								from 
									review_master
								where ReviewType <> " + (int)AppEnum.ReviewType.Inquiry + @"
									and ReferenceSysNo = @productsysno and ReferenceType=@referencetype  
								    and @status
								order by istop desc,createdate desc";

            string sql2 = @"select count(*)
								from 
									review_master
								where ReviewType <> " + (int)AppEnum.ReviewType.Inquiry + @"
									 and ReferenceSysNo = @productsysno and ReferenceType=@referencetype  
								     and @status";

            sql = sql.Replace("@productsysno", productSysNo.ToString());
            sql = sql.Replace("@referencetype", ((int)AppEnum.ReviewReferenceType.Product).ToString());
            sql = sql.Replace("@status", " status<>" + (int)AppEnum.ReviewStatus.Abandon);

            sql2 = sql2.Replace("@productsysno", productSysNo.ToString());
            sql2 = sql2.Replace("@referencetype", ((int)AppEnum.ReviewReferenceType.Product).ToString());
            sql2 = sql2.Replace("@status", " status<>" + (int)AppEnum.ReviewStatus.Abandon);

            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return "";
            int iReviewAllCount = Util.TrimIntNull(SqlHelper.ExecuteScalar(sql2).ToString());
            StringBuilder sbReply = new StringBuilder();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sbReply.Append("select rr.*,c.customerid from review_reply rr left join customer c on rr.createusersysno=c.sysno where rr.status=" + (int)AppEnum.ReviewReplyStatus.Normal + " and reviewSysNo=" + Util.TrimIntNull(dr["sysno"]) + ";");
            }
            DataSet dsReply = new DataSet();
            if (sbReply.Length > 0)
            {
                dsReply = SqlHelper.ExecuteDataSet(sbReply.ToString());
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("<div class=panelr>");
            sb.Append("<div class=panelr_title>");
            sb.Append("<div class=panelr_more><a href='../Review/AllReview.aspx?ItemID=" + productSysNo + "&Type=All'>查看全部" + iReviewAllCount + "条评论</a></div>");
            sb.Append("<img src='../images/site/main/center/tt_khpr.png' alt='客户评论' /></div>");
            sb.Append("<div class=panelr_content>");
            sb.Append("<div id=dr_commentary>");
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sb.Append("<div class=dr_com_block>"); //<!--单个评论区块-->
                sb.Append("<div class=dr_com_b_q>");
                if (Util.TrimIntNull(dr["ReviewType"].ToString()) == (int)AppEnum.ReviewType.Experience)
                {
                    sb.Append("<div class=dr_com_b_q_score>体验评分:");
                    //for (int iStar = 1; iStar <= Util.TrimIntNull(dr["Score"]); iStar++)
                    //{
                    //    sb.Append("<img src='../images/site/main/center/star1.gif' />");
                    //}
                    int Score_Avg = Util.TrimIntNull(dr["Score"]);
                    if (Score_Avg >= 9)
                    {
                        sb.Append(
                            "<img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' />");
                    }
                    else if (Score_Avg >= 7)
                    {
                        sb.Append(
                            "<img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' />");
                    }
                    else if (Score_Avg >= 5)
                    {
                        sb.Append(
                            "<img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' />");
                    }
                    else if (Score_Avg >= 3)
                    {
                        sb.Append(
                            "<img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' />");
                    }
                    else if (Score_Avg >= 1)
                    {
                        sb.Append("<img src='../images/site/main/center/star1.gif' />");
                    }
                }
                else
                {
                    sb.Append("<div class=dr_com_b_q_score>" + AppEnum.GetReviewType(Util.TrimIntNull(dr["ReviewType"].ToString())));
                }
                sb.Append("</div>");
                sb.Append("<div class=dr_com_b_q_title>" + Util.FilterCompetitorKeyword(Util.TrimNull(dr["Title"])) + "</div>");
                sb.Append("<div class=dr_com_b_q_content>" + Util.FilterCompetitorKeyword(Util.TrimNull(dr["Content1"])) + "</div>");

                sb.Append("<div class=dr_com_b_q_time>" + Util.TrimNull(dr["NickName"]) + " 评论于: " + Util.TrimNull(dr["CreateDate"]) + "</div>");
                sb.Append("</div>");
                if (Util.HasMoreRow(dsReply.Tables[i]))
                {
                    foreach (DataRow drReply in dsReply.Tables[i].Rows)
                    {
                        sb.Append("<div class=dr_com_b_r>");
                        sb.Append("<div class=dr_com_b_r_title>[回复]</div>");
                        sb.Append("<div class=dr_com_b_r_content>" + Util.FilterCompetitorKeyword(Util.TrimNull(drReply["ReplyContent"])) + "</div>");
                        if (Util.TrimIntNull(drReply["CreateUserType"]) == (int)AppEnum.CreateUserType.Customer)
                        {
                            sb.Append("<div class=dr_com_b_r_time>" + Util.TrimNull(drReply["customerid"]) + " 回复于:" + Util.TrimNull(drReply["createdate"]) + "</div>");
                        }
                        else
                        {
                            sb.Append("<div class=dr_com_b_r_time> ORS商城 回复于:" + Util.TrimNull(drReply["createdate"]) + "</div>");
                        }
                        sb.Append("</div>");
                    }
                }
                sb.Append("</div>");

                if (Util.TrimIntNull(dr["TotalHelpfulRemarkCount"]) == 0)
                    sb.Append("<div class=dr_com_b_foot>该评论对您是否有所帮助？  <a href=\"javascript:openDialog('../Review/IsHelpful.aspx?Type=Yes&ID=" + Util.TrimIntNull(dr["SysNo"]) + "')\">是</a> <a href=\"javascript:openDialog('../Review/IsHelpful.aspx?Type=No&ID=" + Util.TrimIntNull(dr["SysNo"]) + "')\">否</a>");
                else
                    sb.Append("<div class=dr_com_b_foot>" + Util.TrimIntNull(dr["TotalRemarkCount"]) + "位顾客中有 <font color=red>" + Util.TrimIntNull(dr["TotalHelpfulRemarkCount"]) + "</font> 位认为该评论有帮助，你呢？  <a href=\"javascript:openDialog('../Review/IsHelpful.aspx?Type=Yes&ID=" + Util.TrimIntNull(dr["SysNo"]) + "')\">是</a> <a href=\"javascript:openDialog('../Review/IsHelpful.aspx?Type=No&ID=" + Util.TrimIntNull(dr["SysNo"]) + "')\">否</a>");
                sb.Append("&nbsp;&nbsp;<a href=\"javascript:openDialog('../Review/Reply.aspx?Type=1&ID=" + Util.TrimIntNull(dr["SysNo"]) + "')\"> 回 复 </a> " + "<a href=\"javascript:openDialog('../Review/Reply.aspx?Type=2&ID=" + Util.TrimIntNull(dr["SysNo"]) + "')\"> 举 报 </a></div>");
                i++;
            }

            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");

            return sb.ToString();
        }

        /// <summary>
        /// 此方法用于查询商品询问
        /// </summary>
        /// <param name="productSysNo"></param>
        /// <returns></returns>
        public string GetItemReviewDivByInquiry(int productSysNo)
        {
            string sql = @"select top 5 *
								from 
									review_master
								where ReviewType = " + (int)AppEnum.ReviewType.Inquiry + @"
									and ReferenceSysNo = @productsysno and ReferenceType=@referencetype  
								    and @status
								order by istop desc,createdate desc";

            string sql2 = @"select count(*)
								from 
									review_master
								where ReviewType = " + (int)AppEnum.ReviewType.Inquiry + @"
									 and ReferenceSysNo = @productsysno and ReferenceType=@referencetype  
								     and @status";

            sql = sql.Replace("@productsysno", productSysNo.ToString());
            sql = sql.Replace("@referencetype", ((int)AppEnum.ReviewReferenceType.Product).ToString());
            sql = sql.Replace("@status", " status<>" + (int)AppEnum.ReviewStatus.Abandon);

            sql2 = sql2.Replace("@productsysno", productSysNo.ToString());
            sql2 = sql2.Replace("@referencetype", ((int)AppEnum.ReviewReferenceType.Product).ToString());
            sql2 = sql2.Replace("@status", " status<>" + (int)AppEnum.ReviewStatus.Abandon);

            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return "";
            int iReviewAllCount = Util.TrimIntNull(SqlHelper.ExecuteScalar(sql2).ToString());
            StringBuilder sbReply = new StringBuilder();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sbReply.Append("select rr.*,c.customerid from review_reply rr left join customer c on rr.createusersysno=c.sysno where rr.status=" + (int)AppEnum.ReviewReplyStatus.Normal + " and reviewSysNo=" + Util.TrimIntNull(dr["sysno"]) + ";");
            }
            DataSet dsReply = new DataSet();
            if (sbReply.Length > 0)
            {
                dsReply = SqlHelper.ExecuteDataSet(sbReply.ToString());
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("<div>");
            sb.Append("<div class=panelr_content>");
            sb.Append("<div id=dr_commentary>");
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sb.Append("<div class=dr_com_block>"); //<!--单个评论区块-->
                sb.Append("<div class=dr_com_b_q>");
                if (Util.TrimIntNull(dr["ReviewType"].ToString()) == (int)AppEnum.ReviewType.Experience)
                {
                    sb.Append("<div class=dr_com_b_q_score>体验评分:");
                    //for (int iStar = 1; iStar <= Util.TrimIntNull(dr["Score"]); iStar++)
                    //{
                    //    sb.Append("<img src='../images/site/main/center/star1.gif' />");
                    //}
                    int Score_Avg = Util.TrimIntNull(dr["Score"]);
                    if (Score_Avg >= 9)
                    {
                        sb.Append(
                            "<img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' />");
                    }
                    else if (Score_Avg >= 7)
                    {
                        sb.Append(
                            "<img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' />");
                    }
                    else if (Score_Avg >= 5)
                    {
                        sb.Append(
                            "<img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' />");
                    }
                    else if (Score_Avg >= 3)
                    {
                        sb.Append(
                            "<img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' />");
                    }
                    else if (Score_Avg >= 1)
                    {
                        sb.Append("<img src='../images/site/main/center/star1.gif' />");
                    }
                }
                else
                {
                    sb.Append("<div class=dr_com_b_q_score>");
                }
                sb.Append("</div>");
                sb.Append("<div class=dr_com_b_q_title>" + Util.FilterCompetitorKeyword(Util.TrimNull(dr["Title"])) + "</div>");
                sb.Append("<div class=dr_com_b_q_content>" + Util.FilterCompetitorKeyword(Util.TrimNull(dr["Content1"])) + "</div>");

                sb.Append("<div class=dr_com_b_q_time>" + Util.TrimNull(dr["NickName"]) + " 询问于: " + Util.TrimNull(dr["CreateDate"]) + "</div>");
                sb.Append("</div>");
                if (Util.HasMoreRow(dsReply.Tables[i]))
                {
                    foreach (DataRow drReply in dsReply.Tables[i].Rows)
                    {
                        sb.Append("<div class=dr_com_b_r>");
                        sb.Append("<div class=dr_com_b_r_title>[回复]</div>");
                        sb.Append("<div class=dr_com_b_r_content>" + Util.FilterCompetitorKeyword(Util.TrimNull(drReply["ReplyContent"])) + "</div>");
                        if (Util.TrimIntNull(drReply["CreateUserType"]) == (int)AppEnum.CreateUserType.Customer)
                        {
                            sb.Append("<div class=dr_com_b_r_time>" + Util.TrimNull(drReply["customerid"]) + " 回复于:" + Util.TrimNull(drReply["createdate"]) + "</div>");
                        }
                        else
                        {
                            sb.Append("<div class=dr_com_b_r_time> ORS商城 回复于:" + Util.TrimNull(drReply["createdate"]) + "</div>");
                        }
                        sb.Append("</div>");
                    }
                }
                sb.Append("</div>");

                if (Util.TrimIntNull(dr["TotalHelpfulRemarkCount"]) == 0)
                    sb.Append("<div class=dr_com_b_foot>该询问对您是否有所帮助？  <a href=\"javascript:openDialog('../Review/IsHelpful.aspx?Type=Yes&ID=" + Util.TrimIntNull(dr["SysNo"]) + "')\">是</a> <a href=\"javascript:openDialog('../Review/IsHelpful.aspx?Type=No&ID=" + Util.TrimIntNull(dr["SysNo"]) + "')\">否</a>");
                else
                    sb.Append("<div class=dr_com_b_foot>" + Util.TrimIntNull(dr["TotalRemarkCount"]) + "位顾客中有 <font color=red>" + Util.TrimIntNull(dr["TotalHelpfulRemarkCount"]) + "</font> 位认为该询问有帮助，你呢？  <a href=\"javascript:openDialog('../Review/IsHelpful.aspx?Type=Yes&ID=" + Util.TrimIntNull(dr["SysNo"]) + "')\">是</a> <a href=\"javascript:openDialog('../Review/IsHelpful.aspx?Type=No&ID=" + Util.TrimIntNull(dr["SysNo"]) + "')\">否</a>");
                sb.Append("&nbsp;&nbsp;<a href=\"javascript:openDialog('../Review/Reply.aspx?Type=1&ID=" + Util.TrimIntNull(dr["SysNo"]) + "')\"> 回 复 </a> " + "<a href=\"javascript:openDialog('../Review/Reply.aspx?Type=2&ID=" + Util.TrimIntNull(dr["SysNo"]) + "')\"> 举 报 </a></div>");
                i++;
            }
            sb.Append("<div align=right><a href='../Review/AllReview.aspx?ItemID=" + productSysNo + "&Type=Question' style='color:#f08110'>查看全部" + iReviewAllCount + "条询问</a><br /></div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");

            return sb.ToString();
        }

        public Hashtable GetItemReviewSysNoHash(int ReferenceSysNo, int Type, int referencetype)
        {
            string sql = @"select sysno
								from 
									review_master
								where
									ReferenceSysNo = @ReferenceSysNo and ReferenceType=@referencetype 
                                and @reviewtype and @status
								order by istop desc,createdate desc";
            sql = sql.Replace("@ReferenceSysNo", ReferenceSysNo.ToString());
            sql = sql.Replace("@referencetype", referencetype.ToString());
            sql = sql.Replace("@status", "( status<>" + (int)AppEnum.ReviewStatus.Abandon + ")");
            if (Type != AppConst.IntNull)
            {
                sql = sql.Replace("@reviewtype", " reviewtype=" + Type);
            }
            else
            {
                sql = sql.Replace("@reviewtype", " 1=1 ");
            }
            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return null;

            Hashtable ht = new Hashtable(30);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ht.Add(Util.TrimIntNull(dr["sysno"]), null);
            }
            return ht;
        }

        public Hashtable GetPromotionReviewSysNoHash(int ReferenceSysNo, int Type, int referencetype)
        {
            string sql = @"select sysno
								from 
									review_master
								where
									ReferenceSysNo = @ReferenceSysNo and ReferenceType=@referencetype 
                                and @reviewtype and @status
								order by istop desc,createdate desc";
            sql = sql.Replace("@ReferenceSysNo", ReferenceSysNo.ToString());
            sql = sql.Replace("@referencetype", referencetype.ToString());
            sql = sql.Replace("@status", "( status<>" + (int)AppEnum.ReviewStatus.Abandon + ")");
            if (Type != AppConst.IntNull)
            {
                sql = sql.Replace("@reviewtype", " reviewtype=" + Type);
            }
            else
            {
                sql = sql.Replace("@reviewtype", " 1=1 ");
            }
            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return null;

            Hashtable ht = new Hashtable(30);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ht.Add(Util.TrimIntNull(dr["sysno"]), null);
            }
            return ht;
        }

        public int GetReviewCount(int ReferenceSysNo, int Type, int referencetype, int currentPage)
        {
            string sql = @"select *
								from 
									review_master
								where
									ReferenceSysNo = @productsysno and ReferenceType=@referencetype 
                                and @reviewtype and @status
								";
            sql = sql.Replace("@productsysno", ReferenceSysNo.ToString());
            sql = sql.Replace("@referencetype", referencetype.ToString());
            sql = sql.Replace("@status", " status<>" + (int)AppEnum.ReviewStatus.Abandon);
            if (Type != AppConst.IntNull)
            {
                sql = sql.Replace("@reviewtype", " reviewtype=" + Type);
            }
            else
            {
                sql = sql.Replace("@reviewtype", " 1=1 ");
            }
            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            int ReviewAllCount = ds.Tables[0].Rows.Count;

            return ReviewAllCount;


        }
        public string GetItemAllReviewDiv(int referenceSysNo, int Type, int referencetype, int currentPage)
        {
            string sql = @"select *
								from 
									review_master
								where
									ReferenceSysNo = @referenceSysNo and ReferenceType=@referencetype 
                                and @reviewtype and @status
								order by istop desc,createdate desc";
            sql = sql.Replace("@referenceSysNo", referenceSysNo.ToString());
            sql = sql.Replace("@referencetype", referencetype.ToString());
            sql = sql.Replace("@status", " status<>" + (int)AppEnum.ReviewStatus.Abandon);
            if (Type != AppConst.IntNull)
            {
                sql = sql.Replace("@reviewtype", " reviewtype=" + Type);
            }
            else
            {
                sql = sql.Replace("@reviewtype", " 1=1 ");
            }
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            StringBuilder sb = new StringBuilder();
            if (!Util.HasMoreRow(ds))
            {
                sb.Append("<font color='red'>暂时没有对应的主题</font>");

            }


            int pageSize = 10;
            int totalRowCount = ds.Tables[0].Rows.Count;
            int totalPage = totalRowCount / pageSize;
            if (ds.Tables[0].Rows.Count % pageSize != 0)
                totalPage += 1;

            if (currentPage > totalPage)
                currentPage = 1;
            int rowindex = 0;


            sb.Append("<div class=panelr_content>");
            sb.Append("<div id=dr_commentary>");

            int i = 0;

            StringBuilder sbReply = new StringBuilder();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (rowindex >= (currentPage - 1) * pageSize && rowindex < currentPage * pageSize)
                {
                    sbReply.Append("select rr.*,c.customerid from review_reply rr left join customer c on rr.createusersysno=c.sysno where rr.status=" + (int)AppEnum.ReviewReplyStatus.Normal + " and reviewSysNo=" + Util.TrimIntNull(dr["sysno"]) + ";");
                }
                rowindex++;
            }
            DataSet dsReply = new DataSet();
            if (sbReply.Length > 0)
            {
                dsReply = SqlHelper.ExecuteDataSet(sbReply.ToString());
            }

            rowindex = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (rowindex >= (currentPage - 1) * pageSize && rowindex < currentPage * pageSize)
                {
                    sb.Append("<div class=dr_com_block>"); //<!--单个评论区块-->
                    sb.Append("<div class=dr_com_b_q>");
                    if (dr["ReviewType"].ToString() != AppConst.StringNull)
                    {
                        if (Util.TrimIntNull(dr["ReviewType"].ToString()) == (int)AppEnum.ReviewType.Experience)
                        {
                            sb.Append("<div class=dr_com_b_q_score>体验评分:");
                            //for (int iStar = 1; iStar <= Util.TrimIntNull(dr["Score"]); iStar++)
                            //{
                            //    sb.Append("<img src='../images/site/main/center/star1.gif' />");
                            //}
                            int Score_Avg = Util.TrimIntNull(dr["Score"]);
                            if (Score_Avg >= 9)
                            {
                                sb.Append(
                                    "<img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' />");
                            }
                            else if (Score_Avg >= 7)
                            {
                                sb.Append(
                                    "<img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' />");
                            }
                            else if (Score_Avg >= 5)
                            {
                                sb.Append(
                                    "<img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' />");
                            }
                            else if (Score_Avg >= 3)
                            {
                                sb.Append(
                                    "<img src='../images/site/main/center/star1.gif' /><img src='../images/site/main/center/star1.gif' />");
                            }
                            else if (Score_Avg >= 1)
                            {
                                sb.Append("<img src='../images/site/main/center/star1.gif' />");
                            }
                        }
                        else
                        {
                            sb.Append("<div class=dr_com_b_q_score>" + AppEnum.GetReviewType(Util.TrimIntNull(dr["ReviewType"].ToString())));
                        }
                        sb.Append("</div>");
                    }


                    sb.Append("<div class=dr_com_b_q_title>" + Util.FilterCompetitorKeyword(Util.TrimNull(dr["Title"])) + "</div>");
                    sb.Append("<div class=dr_com_b_q_content>" + Util.FilterCompetitorKeyword(Util.TrimNull(dr["Content1"])) + "</div>");

                    sb.Append("<div class=dr_com_b_q_time>" + Util.TrimNull(dr["NickName"]) + " 评论于: " + Util.TrimNull(dr["CreateDate"]) + "</div>");
                    sb.Append("</div>");
                    if (Util.HasMoreRow(dsReply.Tables[i]))
                    {
                        foreach (DataRow drReply in dsReply.Tables[i].Rows)
                        {
                            sb.Append("<div class=dr_com_b_r>");
                            sb.Append("<div class=dr_com_b_r_title>[回复]</div>");
                            sb.Append("<div class=dr_com_b_r_content>" + Util.FilterCompetitorKeyword(Util.TrimNull(drReply["ReplyContent"])) + "</div>");
                            if (Util.TrimIntNull(drReply["CreateUserType"]) == (int)AppEnum.CreateUserType.Customer)
                            {
                                sb.Append("<div class=dr_com_b_r_time>" + Util.TrimNull(drReply["customerid"]) + " 回复于:" + Util.TrimNull(drReply["createdate"]) + "</div>");
                            }
                            else
                            {
                                sb.Append("<div class=dr_com_b_r_time> ORS商城 回复于:" + Util.TrimNull(drReply["createdate"]) + "</div>");
                            }
                            sb.Append("</div>");
                        }
                    }
                    sb.Append("</div>");

                    if (Util.TrimIntNull(dr["TotalHelpfulRemarkCount"]) == 0)
                        sb.Append("<div class=dr_com_b_foot>该评论对您是否有所帮助？  <a href=\"javascript:openDialog('../Review/IsHelpful.aspx?Type=Yes&ID=" + Util.TrimIntNull(dr["SysNo"]) + "')\">是</a> <a href=\"javascript:openDialog('../Review/IsHelpful.aspx?Type=No&ID=" + Util.TrimIntNull(dr["SysNo"]) + "')\">否</a>");
                    else
                        sb.Append("<div class=dr_com_b_foot>" + Util.TrimIntNull(dr["TotalRemarkCount"]) + "位顾客中有 <font color=red>" + Util.TrimIntNull(dr["TotalHelpfulRemarkCount"]) + "</font> 位认为该评论有帮助，你呢？  <a href=\"javascript:openDialog('../Review/IsHelpful.aspx?Type=Yes&ID=" + Util.TrimIntNull(dr["SysNo"]) + "')\">是</a> <a href=\"javascript:openDialog('../Review/IsHelpful.aspx?Type=No&ID=" + Util.TrimIntNull(dr["SysNo"]) + "')\">否</a>");
                    sb.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href=\"javascript:openDialog('../Review/Reply.aspx?Type=1&ID=" + Util.TrimIntNull(dr["SysNo"]) + "')\"> 回 复 </a> " + "<a href=\"javascript:openDialog('../Review/Reply.aspx?Type=2&ID=" + Util.TrimIntNull(dr["SysNo"]) + "')\"> 举 报 </a></div>");
                    i++;
                }
                rowindex++;
            }
            sb.Append("</div>");
            sb.Append("</div>");

            return sb.ToString();
        }

        public string GetPromotionAllReviewDiv(int referenceSysNo, int Type, int referencetype, int currentPage)
        {
            string sql = @"select *
								from 
									review_master
								where
									ReferenceSysNo = @referenceSysNo and ReferenceType=@referencetype 
                                and @reviewtype and @status
								order by istop desc,createdate desc";
            sql = sql.Replace("@referenceSysNo", referenceSysNo.ToString());
            sql = sql.Replace("@referencetype", referencetype.ToString());
            if (referenceSysNo != 72)
            {
                sql = sql.Replace("@status", " status<>" + (int)AppEnum.ReviewStatus.Abandon);
            }
            else
            {
                sql = sql.Replace("@status", " 1=1 ");
            }

            if (Type != AppConst.IntNull)
            {
                sql = sql.Replace("@reviewtype", " reviewtype=" + Type);
            }
            else
            {
                sql = sql.Replace("@reviewtype", " 1=1 ");
            }
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            StringBuilder sb = new StringBuilder();
            if (!Util.HasMoreRow(ds))
            {
                sb.Append("<font color='red'>暂时没有对应的主题</font>");

            }


            int i = 0;

            if (referenceSysNo == 72)
            {

                int pageSize = 20;
                int totalRowCount = ds.Tables[0].Rows.Count;
                int totalPage = totalRowCount / pageSize;
                if (ds.Tables[0].Rows.Count % pageSize != 0)
                    totalPage += 1;

                if (currentPage > totalPage)
                    currentPage = 1;
                int rowindex = 0;
                int j = totalRowCount;

                sb.Append("<div class=panelr_content>");
                sb.Append("<div id=dr_commentary>");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (rowindex >= (currentPage - 1) * pageSize && rowindex < currentPage * pageSize)
                    {
                        if (i % 2 == 0)
                        {
                            sb.Append("<div class=promotionReview1>");
                        }
                        else
                        {
                            sb.Append("<div class=promotionReview2>");
                        }
                        if (Util.TrimIntNull(dr["status"]) == (int)AppEnum.ReviewStatus.Abandon)
                        {
                            sb.Append("<div class=ReviewTitle><font color=red>不符合ORS商城的祝福规则，已删除！<br/></font>ORS商城祝福大家牛年吉祥、阖家幸福、财源广进、万事顺心！</div>");
                            sb.Append("<div class=ReviewTime>" + Util.TrimNull(dr["NickName"]) + " 发表于: " + Util.TrimNull(dr["CreateDate"]) + "<span class=floor>【" + (totalRowCount - rowindex) + "楼】</span></div>");

                        }
                        else
                        {
                            sb.Append("<div class=ReviewTitle>" + Util.FilterCompetitorKeyword(Util.TrimNull(dr["Content1"])) + "</div>");
                            sb.Append("<div class=ReviewTime>" + Util.TrimNull(dr["NickName"]) + " 发表于: " + Util.TrimNull(dr["CreateDate"]) + "<span class=floor>【" + (totalRowCount - rowindex) + "楼】</span></div>");
                        }
                        sb.Append("<div style='clear:both'></div>");
                        sb.Append("</div>");


                        i++;
                        j--;
                    }
                    rowindex++;
                }
                sb.Append("</div>");
                sb.Append("</div>");

            }
            else
            {

                int pageSize = 10;
                int totalRowCount = ds.Tables[0].Rows.Count;
                int totalPage = totalRowCount / pageSize;
                if (ds.Tables[0].Rows.Count % pageSize != 0)
                    totalPage += 1;

                if (currentPage > totalPage)
                    currentPage = 1;
                int rowindex = 0;


                sb.Append("<div class=panelr_content>");
                sb.Append("<div id=dr_commentary>");



                StringBuilder sbReply = new StringBuilder();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (rowindex >= (currentPage - 1) * pageSize && rowindex < currentPage * pageSize)
                    {
                        sbReply.Append("select rr.*,c.customerid from review_reply rr left join customer c on rr.createusersysno=c.sysno where rr.status=" + (int)AppEnum.ReviewReplyStatus.Normal + " and reviewSysNo=" + Util.TrimIntNull(dr["sysno"]) + ";");
                    }
                    rowindex++;
                }
                DataSet dsReply = new DataSet();
                if (sbReply.Length > 0)
                {
                    dsReply = SqlHelper.ExecuteDataSet(sbReply.ToString());
                }

                rowindex = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (rowindex >= (currentPage - 1) * pageSize && rowindex < currentPage * pageSize)
                    {
                        sb.Append("<div class=dr_com_block>"); //<!--单个评论区块-->
                        sb.Append("<div class=dr_com_b_q>");

                        //sb.Append("<div class=dr_com_b_q_score>";


                        //sb.Append("</div>");
                        sb.Append("<div class=dr_com_b_q_title>" + Util.FilterCompetitorKeyword(Util.TrimNull(dr["Title"])) + "</div>");
                        sb.Append("<div class=dr_com_b_q_content>" + Util.FilterCompetitorKeyword(Util.TrimNull(dr["Content1"])) + "</div>");

                        sb.Append("<div class=dr_com_b_q_time>" + Util.TrimNull(dr["NickName"]) + " 评论于: " + Util.TrimNull(dr["CreateDate"]) + "</div>");
                        sb.Append("</div>");
                        if (Util.HasMoreRow(dsReply.Tables[i]))
                        {
                            foreach (DataRow drReply in dsReply.Tables[i].Rows)
                            {
                                sb.Append("<div class=dr_com_b_r>");
                                sb.Append("<div class=dr_com_b_r_title>[回复]</div>");
                                sb.Append("<div class=dr_com_b_r_content>" + Util.FilterCompetitorKeyword(Util.TrimNull(drReply["ReplyContent"])) + "</div>");
                                if (Util.TrimIntNull(drReply["CreateUserType"]) == (int)AppEnum.CreateUserType.Customer)
                                {
                                    sb.Append("<div class=dr_com_b_r_time>" + Util.TrimNull(drReply["customerid"]) + " 回复于:" + Util.TrimNull(drReply["createdate"]) + "</div>");
                                }
                                else
                                {
                                    sb.Append("<div class=dr_com_b_r_time> ORS商城 回复于:" + Util.TrimNull(drReply["createdate"]) + "</div>");
                                }
                                sb.Append("</div>");
                            }
                        }
                        sb.Append("</div>");

                        if (Util.TrimIntNull(dr["TotalHelpfulRemarkCount"]) == 0)
                            sb.Append("<div class=dr_com_b_foot>该评论对您是否有所帮助？  <a href=\"javascript:openDialog('../Review/IsHelpful.aspx?Type=Yes&ID=" + Util.TrimIntNull(dr["SysNo"]) + "')\">是</a> <a href=\"javascript:openDialog('../Review/IsHelpful.aspx?Type=No&ID=" + Util.TrimIntNull(dr["SysNo"]) + "')\">否</a>");
                        else
                            sb.Append("<div class=dr_com_b_foot>" + Util.TrimIntNull(dr["TotalRemarkCount"]) + "位顾客中有 <font color=red>" + Util.TrimIntNull(dr["TotalHelpfulRemarkCount"]) + "</font> 位认为该评论有帮助，你呢？  <a href=\"javascript:openDialog('../Review/IsHelpful.aspx?Type=Yes&ID=" + Util.TrimIntNull(dr["SysNo"]) + "')\">是</a> <a href=\"javascript:openDialog('../Review/IsHelpful.aspx?Type=No&ID=" + Util.TrimIntNull(dr["SysNo"]) + "')\">否</a>");
                        sb.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href=\"javascript:openDialog('../Review/Reply.aspx?Type=1&ID=" + Util.TrimIntNull(dr["SysNo"]) + "')\"> 回 复 </a> " + "<a href=\"javascript:openDialog('../Review/Reply.aspx?Type=2&ID=" + Util.TrimIntNull(dr["SysNo"]) + "')\"> 举 报 </a></div>");
                        i++;
                    }
                    rowindex++;
                }
                sb.Append("</div>");
                sb.Append("</div>");
            }

            return sb.ToString();
        }
//        public DataSet GetItemReviewDs(Hashtable paramHash)
//        {
//            string sql = @"select rm.*,p.productID,p.productName,pm.userName as PMUserName,su.UserName as editUserName,reply.totalReplyCount
//                           from review_master rm 
//                           inner join product p on rm.referenceSysNo = p.sysNo
//                           inner join category3 c3 on p.c3sysno=c3.sysno 
//                           inner join category2 c2 on c3.c2sysno=c2.sysno
//                           inner join category1 c1 on c2.c1sysno=c1.sysno 
//                           inner join sys_user pm on p.pmusersysno=pm.sysno 
//                           left join (select ReviewSysNo,count(*) as totalReplyCount from review_reply group by reviewsysno) as reply on rm.sysno=reply.reviewsysno
//                           left join sys_user su on rm.lastEditUserSysNo=su.sysno 
//                           @reviewreplystatus 
//                           where 1=1 @createusertype  and rm.referenceType=" + (int)AppEnum.ReviewReferenceType.Product;
//            if (paramHash != null && paramHash.Count > 0)
//            {
//                StringBuilder sb = new StringBuilder();
//                foreach (string key in paramHash.Keys)
//                {
//                    object item = paramHash[key];
//                    sb.Append(" and ");
//                    if (key == "DateFrom")
//                    {
//                        sb.Append("rm.CreateDate >=").Append(Util.ToSqlString(item.ToString()));
//                    }
//                    else if (key == "DateTo")
//                    {
//                        sb.Append("rm.CreateDate <").Append(Util.ToSqlString(item.ToString()));
//                    }
//                    else if (key == "HasComplain")
//                    {
//                        if (Int32.Parse(item.ToString()) == (int)AppEnum.YNStatus.Yes)
//                            sb.Append("rm.TotalComplainCount > 0");
//                        else
//                            sb.Append("rm.TotalComplainCount = 0");
//                    }
//                    else if (key == "PMUserSysNo")
//                    {
//                        sb.Append("pm.sysno=" + item.ToString());
//                    }
//                    else if (key == "APMUserSysNo")
//                    {
//                        sb.Append("p.apmusersysno=" + item.ToString());
//                    }
//                    else if (key == "C1SysNo")
//                    {
//                        sb.Append("c1.sysno=" + item.ToString());
//                    }
//                    else if (key == "C2SysNo")
//                    {
//                        sb.Append("c2.sysno=" + item.ToString());
//                    }
//                    else if (key == "C3SysNo")
//                    {
//                        sb.Append("c3.sysno=" + item.ToString());
//                    }
//                    else if (key == "CreateUserType")
//                    {
//                        sb.Append(" 1=1 ");
//                        //sql = sql.Replace("@createusertype", " and exists(select review_reply.sysno from review_reply where review_reply.reviewsysno=rm.sysno and review_reply.createusertype=" + item.ToString() + ")");
//                        sql = sql.Replace("@createusertype", " and (select top 1 review_reply.createusertype from review_reply where review_reply.reviewsysno=rm.sysno order by review_reply.sysno desc)=" + item.ToString());
//                    }
//                    else if (key == "ReviewReplyStatus")
//                    {
//                        sb.Append(" 1=1 ");
//                        sql = sql.Replace("@reviewreplystatus", " inner join (select distinct reviewsysno from review_reply where status=" + item.ToString() + ") as rr on rm.sysno=rr.reviewsysno ");
//                    }
//                    else if (key == "ReplyContentKeys")
//                    {
//                        string[] keys = (paramHash["ReplyContentKeys"].ToString()).Split(' ');
//                        if (keys.Length > 0)
//                        {
//                            string t = "exists (select * from Review_Reply where Review_Reply.ReviewSysNo=rm.sysno";
//                            for (int i = 0; i < keys.Length; i++)
//                            {
//                                t += " and ReplyContent like " + Util.ToSqlLikeString(keys[i]);
//                            }
//                            t += ")";
//                            sb.Append(t);
//                        }
//                    }
//                    else if (key == "NickName")
//                    {
//                        sb.Append(" rm.NickName like ").Append(Util.ToSqlLikeString(item.ToString()));
//                    }
//                    else if (item is int)
//                    {
//                        sb.Append(key).Append("=").Append(item.ToString());
//                    }
//                    else if (item is string)
//                    {
//                        sb.Append(key).Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
//                    }
//                }
//                sql = sql.Replace("@createusertype", "");
//                sql = sql.Replace("@reviewreplystatus", " ");

//                sql += sb.ToString() + " order by rm.sysno desc";
//            }
//            return SqlHelper.ExecuteDataSet(sql);
//        }
        public DataSet GetItemReviewDs(Hashtable paramHash)
        {
            string sql = @"select rm.*,p.productID,p.productName,pm.userName as PMUserName,su.UserName as editUserName,reply.totalReplyCount
                           from review_master rm 
                           inner join product p on rm.referenceSysNo = p.sysNo
                           inner join category3 c3 on p.c3sysno=c3.sysno 
                           inner join category2 c2 on c3.c2sysno=c2.sysno
                           inner join category1 c1 on c2.c1sysno=c1.sysno 
                           inner join sys_user pm on p.pmusersysno=pm.sysno 
                           left join (select ReviewSysNo,count(*) as totalReplyCount from review_reply group by reviewsysno) as reply on rm.sysno=reply.reviewsysno
                           left join sys_user su on rm.lastEditUserSysNo=su.sysno 
                           @reviewreplystatus 
                           where 1=1 @createusertype  and rm.referenceType=" + (int)AppEnum.ReviewReferenceType.Product;
            if (paramHash != null && paramHash.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string key in paramHash.Keys)
                {
                    object item = paramHash[key];
                    sb.Append(" and ");
                    if (key == "DateFrom")
                    {
                        sb.Append("rm.CreateDate >=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "DateTo")
                    {
                        sb.Append("rm.CreateDate <").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "HasComplain")
                    {
                        if (Int32.Parse(item.ToString()) == (int)AppEnum.YNStatus.Yes)
                            sb.Append("rm.TotalComplainCount > 0");
                        else
                            sb.Append("rm.TotalComplainCount = 0");
                    }
                    else if (key == "PMUserSysNo")
                    {
                        sb.Append("pm.sysno=" + item.ToString());
                    }
                    else if (key == "APMUserSysNo")
                    {
                        sb.Append("p.apmusersysno=" + item.ToString());
                    }
                    else if (key == "C1SysNo")
                    {
                        sb.Append("c1.sysno=" + item.ToString());
                    }
                    else if (key == "C2SysNo")
                    {
                        sb.Append("c2.sysno=" + item.ToString());
                    }
                    else if (key == "C3SysNo")
                    {
                        sb.Append("c3.sysno=" + item.ToString());
                    }
                    else if (key == "CreateUserType")
                    {
                        sb.Append(" 1=1 ");
                        //sql = sql.Replace("@createusertype", " and exists(select review_reply.sysno from review_reply where review_reply.reviewsysno=rm.sysno and review_reply.createusertype=" + item.ToString() + ")");
                        sql = sql.Replace("@createusertype", " and (select top 1 review_reply.createusertype from review_reply where review_reply.reviewsysno=rm.sysno order by review_reply.sysno desc)=" + item.ToString());
                    }
                    else if (key == "ReviewReplyStatus")
                    {
                        sb.Append(" 1=1 ");
                        sql = sql.Replace("@reviewreplystatus", " inner join (select distinct reviewsysno from review_reply where status=" + item.ToString() + ") as rr on rm.sysno=rr.reviewsysno ");
                    }
                    else if (key == "ReplyContentKeys")
                    {
                        string[] keys = (paramHash["ReplyContentKeys"].ToString()).Split(' ');
                        if (keys.Length > 0)
                        {
                            string t = "exists (select * from Review_Reply where Review_Reply.ReviewSysNo=rm.sysno";
                            for (int i = 0; i < keys.Length; i++)
                            {
                                t += " and ReplyContent like " + Util.ToSqlLikeString(keys[i]);
                            }
                            t += ")";
                            sb.Append(t);
                        }
                    }
                    else if (item is int)
                    {
                        sb.Append(key).Append("=").Append(item.ToString());
                    }
                    else if (item is string)
                    {
                        sb.Append(key).Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                    }
                }
                sql = sql.Replace("@createusertype", "");
                sql = sql.Replace("@reviewreplystatus", " ");

                sql += sb.ToString() + " order by rm.sysno desc";
            }
            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetItemReviewDs(int productSysNo)
        {
            string sql = @"select *
								from 
									review_master
								where
									ReferenceSysNo = @productsysno and ReferenceType=@referencetype  
								and @status and @ReviewType
								order by istop desc,createdate desc";

            //            string sql2 = @"select count(*)
            //								from 
            //									review_master
            //								where
            //									ReferenceSysNo = @productsysno and ReferenceType=@referencetype  
            //								and @status and @ReviewType";

            sql = sql.Replace("@productsysno", productSysNo.ToString());
            sql = sql.Replace("@referencetype", ((int)AppEnum.ReviewReferenceType.Product).ToString());
            sql = sql.Replace("@status", " status<>" + (int)AppEnum.ReviewStatus.Abandon);
            sql = sql.Replace("@ReviewType", "ReviewType=" + (int)AppEnum.ReviewType.Inquiry);

            //sql2 = sql2.Replace("@productsysno", productSysNo.ToString());
            //sql2 = sql2.Replace("@referencetype", ((int)AppEnum.ReviewReferenceType.Product).ToString());
            //sql2 = sql2.Replace("@status", " status<>" + (int)AppEnum.ReviewStatus.Abandon);
            //sql2 = sql2.Replace("@ReviewType", "ReviewType=" + (int)AppEnum.ReviewType.Inquiry);


            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return null;

            return ds;
        }

        public DataSet GetItemReviewReplyDs(int reviewSysNo)
        {
            string sql = @"select rr.*,c.customerid 
                                from review_reply rr 
                                left join customer c on rr.createusersysno=c.sysno 
                                where  @Status and  @ReviewSysNo;";

            sql = sql.Replace("@Status", "rr.status=" + (int)AppEnum.ReviewReplyStatus.Normal);
            sql = sql.Replace("@ReviewSysNo", "reviewSysNo=" + reviewSysNo.ToString());

            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return null;

            return ds;

        }

        public DataSet GetPromotionReviewDs(Hashtable paramHash)
        {
            string sql = @"select rm.*,pm.PromotionName ,su.UserName as editUserName,reply.totalReplyCount
                           from review_master rm 
                           inner join Promotion_Master pm on rm.referenceSysNo = pm.sysNo
                           left join (select ReviewSysNo,count(*) as totalReplyCount from review_reply group by reviewsysno) as reply on rm.sysno=reply.reviewsysno
                           left join sys_user su on rm.lastEditUserSysNo=su.sysno 
                           @reviewreplystatus 
                           where 1=1 @createusertype  and rm.referenceType=" + (int)AppEnum.ReviewReferenceType.Promotion;
            if (paramHash != null && paramHash.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string key in paramHash.Keys)
                {
                    object item = paramHash[key];
                    sb.Append(" and ");
                    if (key == "DateFrom")
                    {
                        sb.Append("rm.CreateDate >=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "DateTo")
                    {
                        sb.Append("rm.CreateDate <").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "HasComplain")
                    {
                        if (Int32.Parse(item.ToString()) == (int)AppEnum.YNStatus.Yes)
                            sb.Append("rm.TotalComplainCount > 0");
                        else
                            sb.Append("rm.TotalComplainCount = 0");
                    }

                    else if (key == "CreateUserType")
                    {
                        sb.Append(" 1=1 ");
                        sql = sql.Replace("@createusertype", " and (select top 1 review_reply.createusertype from review_reply where review_reply.reviewsysno=rm.sysno order by review_reply.sysno desc)=" + item.ToString());
                    }
                    else if (key == "ReviewReplyStatus")
                    {
                        sb.Append(" 1=1 ");
                        sql = sql.Replace("@reviewreplystatus", " inner join (select distinct reviewsysno from review_reply where status=" + item.ToString() + ") as rr on rm.sysno=rr.reviewsysno ");
                    }
                    else if (key == "ReplyContentKeys")
                    {
                        string[] keys = (paramHash["ReplyContentKeys"].ToString()).Split(' ');
                        if (keys.Length > 0)
                        {
                            string t = "exists (select * from Review_Reply where Review_Reply.ReviewSysNo=rm.sysno";
                            for (int i = 0; i < keys.Length; i++)
                            {
                                t += " and ReplyContent like " + Util.ToSqlLikeString(keys[i]);
                            }
                            t += ")";
                            sb.Append(t);
                        }
                    }
                    else if (key == "NickName")
                    {
                        sb.Append("  rm.NickName like ").Append(Util.ToSqlLikeString(item.ToString()));
                    }
                    else if (item is int)
                    {
                        sb.Append(key).Append("=").Append(item.ToString());
                    }
                    else if (item is string)
                    {
                        sb.Append(key).Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                    }
                }
                sql = sql.Replace("@createusertype", "");
                sql = sql.Replace("@reviewreplystatus", " ");

                sql += sb.ToString() + " order by rm.sysno desc";
            }
            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetDIYReviewDs(Hashtable paramHash)
        {
            string sql = @"select rm.*,pm.Title as diytitle,su.UserName as editUserName,reply.totalReplyCount
                           from review_master rm 
                           inner join Prj_Master pm on rm.referenceSysNo = pm.sysNo
                           left join (select ReviewSysNo,count(*) as totalReplyCount from review_reply group by reviewsysno) as reply on rm.sysno=reply.reviewsysno
                           left join sys_user su on rm.lastEditUserSysNo=su.sysno 
                           @reviewreplystatus 
                           where 1=1 @createusertype  and rm.referenceType=" + (int)AppEnum.ReviewReferenceType.DIY;
            if (paramHash != null && paramHash.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string key in paramHash.Keys)
                {
                    object item = paramHash[key];
                    sb.Append(" and ");
                    if (key == "DateFrom")
                    {
                        sb.Append("rm.CreateDate >=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "DateTo")
                    {
                        sb.Append("rm.CreateDate <").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "HasComplain")
                    {
                        if (Int32.Parse(item.ToString()) == (int)AppEnum.YNStatus.Yes)
                            sb.Append("rm.TotalComplainCount > 0");
                        else
                            sb.Append("rm.TotalComplainCount = 0");
                    }

                    else if (key == "CreateUserType")
                    {
                        sb.Append(" 1=1 ");
                        sql = sql.Replace("@createusertype", " and (select top 1 review_reply.createusertype from review_reply where review_reply.reviewsysno=rm.sysno order by review_reply.sysno desc)=" + item.ToString());
                    }
                    else if (key == "ReviewReplyStatus")
                    {
                        sb.Append(" 1=1 ");
                        sql = sql.Replace("@reviewreplystatus", " inner join (select distinct reviewsysno from review_reply where status=" + item.ToString() + ") as rr on rm.sysno=rr.reviewsysno ");
                    }
                    else if (key == "ReplyContentKeys")
                    {
                        string[] keys = (paramHash["ReplyContentKeys"].ToString()).Split(' ');
                        if (keys.Length > 0)
                        {
                            string t = "exists (select * from Review_Reply where Review_Reply.ReviewSysNo=rm.sysno";
                            for (int i = 0; i < keys.Length; i++)
                            {
                                t += " and ReplyContent like " + Util.ToSqlLikeString(keys[i]);
                            }
                            t += ")";
                            sb.Append(t);
                        }
                    }
                    else if (key == "NickName")
                    {
                        sb.Append("  rm.NickName like ").Append(Util.ToSqlLikeString(item.ToString()));
                    }
                    else if (item is int)
                    {
                        sb.Append(key).Append("=").Append(item.ToString());
                    }
                    else if (item is string)
                    {
                        sb.Append(key).Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                    }
                }
                sql = sql.Replace("@createusertype", "");
                sql = sql.Replace("@reviewreplystatus", " ");

                sql += sb.ToString() + " order by rm.sysno desc";
            }
            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetReviewReplyDs(int reviewSysNo)
        {
            string sql = "select rr.*,c.customerid as createusername,su.username as lasteditusername from review_reply rr inner join customer c on rr.createusersysno=c.sysno left join sys_user su on rr.lasteditusersysno=su.sysno where createusertype=" + (int)AppEnum.CreateUserType.Customer + " and reviewsysno=" + reviewSysNo;
            sql += " union all select rr.*,s.username as createusername,su.username as lasteditusername from review_reply rr inner join sys_user s on rr.createusersysno=s.sysno left join sys_user su on rr.lasteditusersysno=su.sysno where createusertype=" + (int)AppEnum.CreateUserType.Employee + " and reviewsysno=" + reviewSysNo;
            sql = "select * from (" + sql + ") v order by createdate desc";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                return ds;
            else
                return null;
        }

        public DataSet GetReviewComplainDs(int reviewSysNo)
        {
            string sql = "select *,c.customerid as createusername from review_complain rc left join customer c on rc.createusersysno=c.sysno where reviewsysno=" + reviewSysNo + " order by createdate desc";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                return ds;
            else
                return null;
        }

        public int AbandonReview(int reviewSysNo, int userSysNo)
        {
            string sql = "update review_master set status=" + (int)AppEnum.ReviewStatus.Abandon + ",lasteditusersysno=@usersysno,lasteditdate=@lasteditdate where sysno=" + reviewSysNo;
            sql = sql.Replace("@usersysno", userSysNo.ToString());
            sql = sql.Replace("@lasteditdate", Util.ToSqlString(DateTime.Now.ToString()));
            return SqlHelper.ExecuteNonQuery(sql);
        }

        public int CancelAbandonReview(int reviewSysNo, int userSysNo)
        {
            string sql = "update review_master set status=" + (int)AppEnum.ReviewStatus.UnConfirmed + ",lasteditusersysno=@usersysno,lasteditdate=@lasteditdate where sysno=" + reviewSysNo;
            sql = sql.Replace("@usersysno", userSysNo.ToString());
            sql = sql.Replace("@lasteditdate", Util.ToSqlString(DateTime.Now.ToString()));
            return SqlHelper.ExecuteNonQuery(sql);
        }

        public int ConfirmReview(int reviewSysNo, int userSysNo)
        {
            string sql = "update review_master set status=" + (int)AppEnum.ReviewStatus.Confirmed + ",lasteditusersysno=@usersysno,lasteditdate=@lasteditdate where sysno=" + reviewSysNo;
            sql = sql.Replace("@usersysno", userSysNo.ToString());
            sql = sql.Replace("@lasteditdate", Util.ToSqlString(DateTime.Now.ToString()));
            return SqlHelper.ExecuteNonQuery(sql);
        }

        public int UnConfirmReview(int reviewSysNo, int userSysNo)
        {
            string sql = "update review_master set status=" + (int)AppEnum.ReviewStatus.UnConfirmed + ",lasteditusersysno=@usersysno,lasteditdate=@lasteditdate where sysno=" + reviewSysNo;
            sql = sql.Replace("@usersysno", userSysNo.ToString());
            sql = sql.Replace("@lasteditdate", Util.ToSqlString(DateTime.Now.ToString()));
            return SqlHelper.ExecuteNonQuery(sql);
        }

        public int ReviewSetTop(int reviewSysNo, int userSysNo)
        {
            string sql = "update review_master set istop=" + (int)AppEnum.YNStatus.Yes + ",lasteditusersysno=@usersysno,lasteditdate=@lasteditdate where sysno=" + reviewSysNo;
            sql = sql.Replace("@usersysno", userSysNo.ToString());
            sql = sql.Replace("@lasteditdate", Util.ToSqlString(DateTime.Now.ToString()));
            return SqlHelper.ExecuteNonQuery(sql);
        }

        public int ReviewCancelTop(int reviewSysNo, int userSysNo)
        {
            string sql = "update review_master set istop=" + (int)AppEnum.YNStatus.No + ",lasteditusersysno=@usersysno,lasteditdate=@lasteditdate where sysno=" + reviewSysNo;
            sql = sql.Replace("@usersysno", userSysNo.ToString());
            sql = sql.Replace("@lasteditdate", Util.ToSqlString(DateTime.Now.ToString()));
            return SqlHelper.ExecuteNonQuery(sql);
        }

        public int ReviewSetGood(int reviewSysNo, int userSysNo)
        {
            string sql = "update review_master set isgood=" + (int)AppEnum.YNStatus.Yes + ",lasteditusersysno=@usersysno,lasteditdate=@lasteditdate where sysno=" + reviewSysNo;
            sql = sql.Replace("@usersysno", userSysNo.ToString());
            sql = sql.Replace("@lasteditdate", Util.ToSqlString(DateTime.Now.ToString()));
            return SqlHelper.ExecuteNonQuery(sql);
        }

        public int ReviewCancelGood(int reviewSysNo, int userSysNo)
        {
            string sql = "update review_master set isgood=" + (int)AppEnum.YNStatus.No + ",lasteditusersysno=@usersysno,lasteditdate=@lasteditdate where sysno=" + reviewSysNo;
            sql = sql.Replace("@usersysno", userSysNo.ToString());
            sql = sql.Replace("@lasteditdate", Util.ToSqlString(DateTime.Now.ToString()));
            return SqlHelper.ExecuteNonQuery(sql);
        }

        public int AbandonReviewReply(int replySysNo, int userSysNo)
        {
            string sql = "update review_reply set status=" + (int)AppEnum.ReviewReplyStatus.Abandon + ",lasteditusersysno=@usersysno,lasteditdate=@lasteditdate where sysno=" + replySysNo;
            sql = sql.Replace("@usersysno", userSysNo.ToString());
            sql = sql.Replace("@lasteditdate", Util.ToSqlString(DateTime.Now.ToString()));
            return SqlHelper.ExecuteNonQuery(sql);
        }

        public int CancelAbandonReviewReply(int replySysNo, int userSysNo)
        {
            string sql = "update review_reply set status=@status,lasteditusersysno=@usersysno,lasteditdate=@lasteditdate where sysno=" + replySysNo;
            sql = sql.Replace("@usersysno", userSysNo.ToString());
            sql = sql.Replace("@lasteditdate", Util.ToSqlString(DateTime.Now.ToString()));

            ReviewReplyInfo rrInfo = LoadReviewReply(replySysNo);
            if (rrInfo.CreateUserType == (int)AppEnum.CreateUserType.Customer)
            {
                sql = sql.Replace("@status", ((int)AppEnum.ReviewReplyStatus.Normal).ToString());
            }
            else
            {
                sql = sql.Replace("@status", ((int)AppEnum.ReviewReplyStatus.WaitingAudit).ToString());
            }

            return SqlHelper.ExecuteNonQuery(sql);
        }

        public int GetCustomerReviewCountByDate(string CustomerIP)
        {
            int reviewCount = AppConst.IntNull;
            string sql = @"select count(*) as reviewCount from Review_Master where CustomerIP=" + Util.ToSqlString(CustomerIP) + " and CreateDate>=" + Util.ToSqlString(DateTime.Today.ToString());
            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (Util.HasMoreRow(ds))
            {
                DataRow dr = ds.Tables[0].Rows[0];
                reviewCount = Util.TrimIntNull(dr["reviewCount"]);
            }
            return reviewCount;


        }

        public void CreateReview(ReviewInfo oInfo)
        {
            try
            {
                TransactionOptions options = new TransactionOptions();
                options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                options.Timeout = TransactionManager.DefaultTimeout;

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    //一个用户每天最多只能发表5条评论
                    int reviewCount = GetCustomerReviewCountByDate(oInfo.CustomerIP);
                    if (reviewCount >= 5)
                        throw new BizException("同一个IP每天最多只能发表5次评论！");

                    this.InsertReviewMaster(oInfo);

                    //加入订单商品明细
                    if (oInfo.ReferenceType == (int)AppEnum.ReviewReferenceType.Product)
                    {
                        foreach (ReviewC3ItemScoreInfo item in oInfo.ReviewC3ItemScoreHash.Values)
                        {
                            item.ReviewSysNo = oInfo.SysNo;
                            this.InsertReviewC3ItemScore(item);
                        }
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                oInfo.SysNo = AppConst.IntNull;
                throw ex;
            }
        }

        /// <summary>
        /// 检查客户是否购买过该商品
        /// </summary>
        /// <param name="customerSysNo"></param>
        /// <param name="productSysNo"></param>
        /// <returns></returns>
        public bool CheckCustomerHasBuyProduct(int customerSysNo, int productSysNo)
        {
            string sql = @"select si.productsysno from so_master sm inner join so_item si on sm.sysno=si.sosysno 
                        inner join product p on si.productsysno=p.sysno
                        where sm.status=@status and sm.customersysno=@customerSysNo and si.productsysno=@productSysNo";
            sql = sql.Replace("@status", ((int)AppEnum.SOStatus.OutStock).ToString());
            sql = sql.Replace("@customerSysNo", customerSysNo.ToString());
            sql = sql.Replace("@productSysNo", productSysNo.ToString());

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 检查客户是否评论过该商品
        /// </summary>
        /// <param name="customerSysNo"></param>
        /// <param name="productSysNo"></param>
        /// <param name="reviewType"></param>
        /// <returns></returns>
        public bool CheckCustomerHasReviewProduct(int customerSysNo, int productSysNo, int reviewType)
        {
            string sql = @"select sysno from review_master where CreateCustomerSysNo=@customerSysNo and ReviewType=@reviewType
                        and ReferenceType=@referenceType and ReferenceSysNo=@productSysNo";
            sql = sql.Replace("@customerSysNo", customerSysNo.ToString());
            sql = sql.Replace("@productSysNo", productSysNo.ToString());
            sql = sql.Replace("@reviewType", reviewType.ToString());
            sql = sql.Replace("@referenceType", ((int)AppEnum.ReviewReferenceType.Product).ToString());

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                return true;
            else
                return false;
        }

        //add by Judy
        public string GetSecondCategoryReviewDiv(int c2sysno)
        {
            string sql = @"select top 2 product.productname,product.sysno as psysno,rm.Title,rm.content1,rm.NickName,rm.CreateDate from Review_master rm
                            left join product on ReferenceSysno=product.sysno
                            left join category3 c3 on c3.sysno=product.c3sysno
                            where c3.c2sysno= " + c2sysno + "and rm.isgood=1 and rm.status<> -2 order by rm.CreateDate desc";

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return "";
            StringBuilder sb = new StringBuilder();
            sb.Append("<div id='fc_hy' class='panelc'>");
            sb.Append("<div class='panelc_title'>");
            sb.Append("<img src='../images/site/main/center/tt_jcpr.png'alt='精彩评论'/></div>");
            sb.Append("<div class='panelc_content' width=''>");
            sb.Append("<div class='c_hy'>");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sb.Append("<div style='padding-left:10px;padding-right:10px;' >关于商品：<a href=../Items/ItemDetail.aspx?ItemID=" + Util.TrimNull(dr["psysno"]) + ">" + Util.TrimNull(dr["productname"]) + "</a> 的评论</div>");
                sb.Append("<div class='c_hy_p' style='padding-left:10px;padding-right:10px;'><img src=../../images/site/main/center/li2.png /> <font color='#f76d04'>" + Util.TrimNull(dr["Title"]) + "</font></div>");
                sb.Append("<div class='c_hy_p' style='padding-left:24px;padding-right:10px;'>" + Util.TrimNull(dr["Content1"]) + "</div>");
                sb.Append("<div class='c_hy_ath' style='padding-right:10px'>" + Util.TrimNull(dr["nickName"]) + "&nbsp;&nbsp;&nbsp;&nbsp;" + Util.TrimDateNull(dr["createDate"]) + "</div></br>");
            }
            sb.Append("</div>");
            sb.Append("<br clear='all'>");
            sb.Append("</div>");
            sb.Append("</div>");
            //sb.Append("</div>");
            return sb.ToString();

        }

        //add by Judy
        public string GetFirstCategoryReviewDiv(int c1sysno)
        {
            string sql = @"select top 2 product.productname,product.sysno as psysno,rm.Title,rm.content1,rm.NickName,rm.CreateDate from Review_master rm
                            left join product on ReferenceSysno=product.sysno
                            left join category3 c3 on c3.sysno=product.c3sysno
                            left join category2 c2 on c2.sysno=c3.c2sysno
                            where c2.c1sysno= " + c1sysno + " and rm.isgood=1 and rm.status<> -2 order by rm.CreateDate desc";

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return "";
            StringBuilder sb = new StringBuilder();
            sb.Append("<div id='fc_hy' class='panelc'>");
            sb.Append("<div class='panelc_title'>");
            sb.Append("<img src='../images/site/main/center/tt_jcpr.png'alt='精彩评论'/></div>");
            sb.Append("<div class='panelc_content' width=''>");
            sb.Append("<div class='c_hy'>");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sb.Append("<div style='padding-left:10px;padding-right:10px;' >关于商品：<a href=../Items/ItemDetail.aspx?ItemID=" + Util.TrimNull(dr["psysno"]) + ">" + Util.TrimNull(dr["productname"]) + "</a> 的评论</div>");
                sb.Append("<div class='c_hy_p' style='padding-left:10px;padding-right:10px;'><img src=../../images/site/main/center/li2.png /> <font color='#f76d04'>" + Util.TrimNull(dr["Title"]) + "</font></div>");
                sb.Append("<div class='c_hy_p' style='padding-left:24px;padding-right:10px;'>" + Util.TrimNull(dr["Content1"]) + "</div>");
                sb.Append("<div class='c_hy_ath' style='padding-right:10px'>" + Util.TrimNull(dr["nickName"]) + "&nbsp;&nbsp;&nbsp;&nbsp;" + Util.TrimDateNull(dr["createDate"]) + "</div></br>");
            }
            sb.Append("</div>");
            sb.Append("<br clear='all'>");
            sb.Append("</div>");
            sb.Append("</div>");
            //sb.Append("</div>");
            return sb.ToString();

        }

        public DataSet GetFirstCategoryReviewDataSet(int c1sysno)
        {
            string sql = @"select top 2 product.productID,product.productname,product.sysno as psysno,rm.Title,rm.content1,rm.NickName,rm.CreateDate from Review_master rm
                            left join product on ReferenceSysno=product.sysno
                            left join category3 c3 on c3.sysno=product.c3sysno
                            left join category2 c2 on c2.sysno=c3.c2sysno
                            where c2.c1sysno= " + c1sysno + " and rm.isgood=1 and rm.status<> -2 order by rm.CreateDate desc";

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;
            else
                return ds;
        }

        public bool CheckReviewExist(int CreateCustomerSysNo, int ReferenceType, int ReferenceSysNo)
        {
            string sql = @"select *
								from 
									review_master
								where
									ReferenceSysNo = @referenceSysNo and ReferenceType=@referencetype 
                                    and CreateCustomerSysNo=@CreateCustomerSysNo and CreateDate>=" + Util.ToSqlString(DateTime.Today.ToString());
            sql = sql.Replace("@referenceSysNo", ReferenceSysNo.ToString());
            sql = sql.Replace("@referencetype", ReferenceType.ToString());
            sql = sql.Replace("@CreateCustomerSysNo", CreateCustomerSysNo.ToString());
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            bool result = false;

            if (Util.HasMoreRow(ds) && ds.Tables[0].Rows.Count != 0)
                result = true;

            return result;

        }

        public DataSet GetMasterReviewDs(Hashtable paramHash)
        {
            string sql = @"select * from review_master rm	where 1=1";

            if (paramHash != null && paramHash.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string key in paramHash.Keys)
                {
                    object item = paramHash[key];
                    sb.Append(" and ");
                    if (key == "ReferenceSysNo")
                    {
                        sb.Append("rm.ReferenceSysNo =").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "IsGood")
                    {
                        sb.Append("rm.IsGood =").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "ReferenceType")
                    {
                        sb.Append("rm.ReferenceType =").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "Status")
                    {
                        sb.Append("rm.Status =").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "ReviewType")
                    {
                        sb.Append("rm.ReviewType =").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "Top")
                    {
                        sb.Append("1=1");
                        sql = sql.Replace("select", "select " + paramHash["Top"].ToString());
                    }
                    else if (item is int)
                    {
                        sb.Append(key).Append("=").Append(item.ToString());
                    }
                    else if (item is string)
                    {
                        sb.Append(key).Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                    }
                }

                sql += sb.ToString();
            }

            sql += " order by rm.CreateDate desc";

            return SqlHelper.ExecuteDataSet(sql);

        }



        /// <summary>
        /// 此方法用于查询装机宝的评论，装机宝评论不分评论类型
        /// </summary>
        /// <param name="prjSysNo"></param>
        /// <returns></returns>
        public string GetDIYReviewDiv(int prjSysNo)
        {
            string sql = @"select top 8 *
								from 
									review_master
								where 1=1
									and ReferenceSysNo = @prjSysNo and ReferenceType=@referencetype  
								    and @status
								order by istop desc,createdate desc";

            string sql2 = @"select count(*)
								from 
									review_master
								where 1=1
									 and ReferenceSysNo = @prjSysNo and ReferenceType=@referencetype  
								     and @status";

            sql = sql.Replace("@prjSysNo", prjSysNo.ToString());
            sql = sql.Replace("@referencetype", ((int)AppEnum.ReviewReferenceType.DIY).ToString());
            sql = sql.Replace("@status", " status<>" + (int)AppEnum.ReviewStatus.Abandon);

            sql2 = sql2.Replace("@prjSysNo", prjSysNo.ToString());
            sql2 = sql2.Replace("@referencetype", ((int)AppEnum.ReviewReferenceType.DIY).ToString());
            sql2 = sql2.Replace("@status", " status<>" + (int)AppEnum.ReviewStatus.Abandon);

            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return "";
            int iReviewAllCount = Util.TrimIntNull(SqlHelper.ExecuteScalar(sql2).ToString());
            StringBuilder sbReply = new StringBuilder();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sbReply.Append("select rr.*,c.customerid from review_reply rr left join customer c on rr.createusersysno=c.sysno where rr.status=" + (int)AppEnum.ReviewReplyStatus.Normal + " and reviewSysNo=" + Util.TrimIntNull(dr["sysno"]) + ";");
            }
            DataSet dsReply = new DataSet();
            if (sbReply.Length > 0)
            {
                dsReply = SqlHelper.ExecuteDataSet(sbReply.ToString());
            }

            StringBuilder sb = new StringBuilder();

            //sb.Append("<div class=panelr>");
            //sb.Append("<div class=panelr_title>");
            //sb.Append("<div class=panelr_more><a href='../Review/AllReview.aspx?ItemID=" + prjSysNo + "&Type=All'>查看全部" + iReviewAllCount + "条评论</a></div>");
            //sb.Append("<img src='../images/site/main/center/tt_khpr.png' alt='客户评论' /></div>");
            //sb.Append("<div class=panelr_content>");
            //sb.Append("<div id=dr_commentary>");
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sb.Append("<div class=dr_com_block>"); //<!--单个评论区块-->
                sb.Append("<div class=dr_com_b_q>");
                sb.Append("</div>");
                sb.Append("<div class=dr_com_b_q_title>" + Util.FilterCompetitorKeyword(Util.TrimNull(dr["Title"])) + "</div>");
                sb.Append("<div class=dr_com_b_q_content>" + Util.FilterCompetitorKeyword(Util.TrimNull(dr["Content1"])) + "</div>");

                sb.Append("<div class=dr_com_b_q_time>" + Util.TrimNull(dr["NickName"]) + " 评论于: " + Util.TrimNull(dr["CreateDate"]) + "</div>");
                sb.Append("</div>");
                if (Util.HasMoreRow(dsReply.Tables[i]))
                {
                    foreach (DataRow drReply in dsReply.Tables[i].Rows)
                    {
                        sb.Append("<div class=dr_com_b_r>");
                        sb.Append("<div class=dr_com_b_r_title>[回复]</div>");
                        sb.Append("<div class=dr_com_b_r_content>" + Util.FilterCompetitorKeyword(Util.TrimNull(drReply["ReplyContent"])) + "</div>");
                        if (Util.TrimIntNull(drReply["CreateUserType"]) == (int)AppEnum.CreateUserType.Customer)
                        {
                            sb.Append("<div class=dr_com_b_r_time>" + Util.TrimNull(drReply["customerid"]) + " 回复于:" + Util.TrimNull(drReply["createdate"]) + "</div>");
                        }
                        else
                        {
                            sb.Append("<div class=dr_com_b_r_time> ORS商城 回复于:" + Util.TrimNull(drReply["createdate"]) + "</div>");
                        }
                        sb.Append("</div>");
                    }
                }
                sb.Append("</div>");

                if (Util.TrimIntNull(dr["TotalHelpfulRemarkCount"]) == 0)
                    sb.Append("<div class=dr_com_b_foot>该评论对您是否有所帮助？  <a href=\"javascript:openDialog('../Review/IsHelpful.aspx?Type=Yes&ID=" + Util.TrimIntNull(dr["SysNo"]) + "')\">是</a> <a href=\"javascript:openDialog('../Review/IsHelpful.aspx?Type=No&ID=" + Util.TrimIntNull(dr["SysNo"]) + "')\">否</a>");
                else
                    sb.Append("<div class=dr_com_b_foot>" + Util.TrimIntNull(dr["TotalRemarkCount"]) + "位顾客中有 <font color=red>" + Util.TrimIntNull(dr["TotalHelpfulRemarkCount"]) + "</font> 位认为该评论有帮助，你呢？  <a href=\"javascript:openDialog('../Review/IsHelpful.aspx?Type=Yes&ID=" + Util.TrimIntNull(dr["SysNo"]) + "')\">是</a> <a href=\"javascript:openDialog('../Review/IsHelpful.aspx?Type=No&ID=" + Util.TrimIntNull(dr["SysNo"]) + "')\">否</a>");
                sb.Append("&nbsp;&nbsp;<a href=\"javascript:openDialog('../Review/Reply.aspx?Type=1&ID=" + Util.TrimIntNull(dr["SysNo"]) + "')\"> 回 复 </a> " + "<a href=\"javascript:openDialog('../Review/Reply.aspx?Type=2&ID=" + Util.TrimIntNull(dr["SysNo"]) + "')\"> 举 报 </a></div>");
                i++;
            }

            //sb.Append("</div>");
            //sb.Append("</div>");
            //sb.Append("</div>");

            sb.Append("<div style='text-align:center'><br/><a href='../Review/AllReview.aspx?PID=" + prjSysNo + "&Type=All'>查看所有评论</a></div>");
            return sb.ToString();
        }
    }
}