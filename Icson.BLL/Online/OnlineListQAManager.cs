using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

using Icson.Utils;
using Icson.Objects;
using Icson.Objects.Online;
using Icson.DBAccess;
using Icson.DBAccess.Online;

namespace Icson.BLL.Online
{
    public class OnlineListQAManager
    {
        private OnlineListQAManager()
		{
		}
        private static OnlineListQAManager _instance;
        public static OnlineListQAManager GetInstance()
		{
			if( _instance == null )
			{
                _instance = new OnlineListQAManager();
			}
			return _instance;
		}

        private void map(OnlineListQAInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.OnlineAreaType = Util.TrimIntNull(tempdr["OnlineAreaType"]);
            oParam.CategorySysNo = Util.TrimIntNull(tempdr["CategorySysNo"]);
            oParam.QAType = Util.TrimIntNull(tempdr["QAType"]);
            oParam.QASysNo = Util.TrimIntNull(tempdr["QASysNo"]);
            oParam.CreateUserSysNo = Util.TrimIntNull(tempdr["CreateUserSysNo"]);
            oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
            oParam.ListOrder = Util.TrimIntNull(tempdr["ListOrder"]);
        }

        public int Insert(OnlineListQAInfo oParam)
        {
            if (LoadOnlineListQA(oParam.OnlineAreaType, oParam.CategorySysNo, oParam.QASysNo) != null)
            {
                throw new BizException("duplicated!");
            }
            return new OnlineListQADac().Insert(oParam);
        }

        public int Update(OnlineListQAInfo oParam)
        {
            return new OnlineListQADac().Update(oParam);
        }

        public OnlineListQAInfo LoadOnlineListQA(int OnlineAreaType, int CategorySysNo, int QASysNo)
        {
            string sql = "select * from onlinelistQA where onlineareatype=" + OnlineAreaType + " and categorysysno=" + CategorySysNo + " and QASysNo=" + QASysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                OnlineListQAInfo oInfo = new OnlineListQAInfo();
                map(oInfo, ds.Tables[0].Rows[0]);
                return oInfo;
            }
            else
            {
                return null;
            }
        }

        public void Delete(int sysno)
        {
            new OnlineListQADac().Delete(sysno);
        }

        public DataSet GetOnlineListQADs(Hashtable paramHash, int topNumber)
        {
            string sqlHomePage = @" select @top
								OnlineListQA.SysNo as OnlineListQASysNo,OnlineListQA.QAType, OnlineListQA.OnlineAreaType,'首页' as categoryname,
                                QA.SysNo as SysNo,Question,Answer,SearchKey, 
                                ListOrder, QA.Status, UserName as CreateUserName,
								OnlineListQA.CreateTime,QA.OrderNum
							from 
								OnlineListQA,
								QA,
								Sys_User
							where OnlineListQA.OnlineAreaType = " + (int)AppEnum.OnlineAreaType.HomePage;
					sqlHomePage += " and OnlineListQA.QASysNo = QA.sysno and OnlineListQA.CreateUserSysNo = Sys_User.SysNo ";
            string sqlCategory1 = @" select @top
						OnlineListQA.SysNo as OnlineListQASysNo,OnlineListQA.QAType, OnlineListQA.OnlineAreaType,c1.c1name categoryname,";
                   sqlCategory1 += @"QA.SysNo as SysNo,Question,Answer,SearchKey,
                                ListOrder,QA.Status,UserName as CreateUserName,
						        OnlineListQA.CreateTime,QA.OrderNum
					        from 
						        OnlineListQA,
						        QA,
						        Sys_User, 
                                category1 c1
                            where c1.sysno = onlinelistqa.categorysysno";
				   sqlCategory1 += " and OnlineListQA.OnlineAreaType = " + (int)AppEnum.OnlineAreaType.FirstCategory;
                   sqlCategory1 += " and OnlineListQA.QASysNo = QA.sysno and OnlineListQA.CreateUserSysNo = Sys_User.SysNo ";

            string sqlCategory2 = @" select @top
						OnlineListQA.SysNo as OnlineListQASysNo,OnlineListQA.QAType, OnlineListQA.OnlineAreaType,c2.c2name categoryname,";
                   sqlCategory2 += @"QA.SysNo as SysNo,Question,Answer,SearchKey,
                                ListOrder,QA.Status,UserName as CreateUserName,
						        OnlineListQA.CreateTime,QA.OrderNum
					        from 
						        OnlineListQA,
						        QA,
						        Sys_User, 
                                category2 c2
                            where c2.sysno = onlinelistqa.categorysysno";
                   sqlCategory2 += " and OnlineListQA.OnlineAreaType = " + (int)AppEnum.OnlineAreaType.SecondCategory;
                   sqlCategory2 += " and OnlineListQA.QASysNo = QA.sysno and OnlineListQA.CreateUserSysNo = Sys_User.SysNo ";

            string sqlCategory3 = @" select @top
						OnlineListQA.SysNo as OnlineListQASysNo,OnlineListQA.QAType, OnlineListQA.OnlineAreaType,c3.c3name categoryname,";
                   sqlCategory3 += @"QA.SysNo as SysNo,Question,Answer,SearchKey,
                                ListOrder,QA.Status,UserName as CreateUserName,
						        OnlineListQA.CreateTime,QA.OrderNum
					        from 
						        OnlineListQA,
						        QA,
						        Sys_User, 
                                category3 c3
                            where c3.sysno = onlinelistqa.categorysysno";
                   sqlCategory3 += " and OnlineListQA.OnlineAreaType = " + (int)AppEnum.OnlineAreaType.ThirdCategory;
                   sqlCategory3 += " and OnlineListQA.QASysNo = QA.sysno and OnlineListQA.CreateUserSysNo = Sys_User.SysNo ";

            string sqlItemDetail = @" select @top
						OnlineListQA.SysNo as OnlineListQASysNo,OnlineListQA.QAType, OnlineListQA.OnlineAreaType,p.productname categoryname,";
                   sqlItemDetail += @"QA.SysNo as SysNo,Question,Answer,SearchKey,
                                ListOrder,QA.Status,UserName as CreateUserName,
						        OnlineListQA.CreateTime,QA.OrderNum
					        from 
						        OnlineListQA,
						        QA,
						        Sys_User, 
                                product p
                            where p.sysno = onlinelistqa.categorysysno";
                   sqlItemDetail += " and OnlineListQA.OnlineAreaType = " + (int)AppEnum.OnlineAreaType.ItemDetail;
                   sqlItemDetail += " and OnlineListQA.QASysNo = QA.sysno and OnlineListQA.CreateUserSysNo = Sys_User.SysNo ";

            string sqlOrderBy = "";

            if (paramHash != null && paramHash.Count != 0)
            {
                StringBuilder sb = new StringBuilder(100);
                foreach (string key in paramHash.Keys)
                {
                    object item = paramHash[key];
                    if (key == "OrderBy")
                    {
                        sqlOrderBy = " order by " + item.ToString();
                    }
                    else
                    {
                        sb.Append(" and ");
                        if (key == "QASysNo")
                        {
                            sb.Append("OnlineListQA.QASysNo = ").Append(item.ToString());
                        }
                        else if(key == "QAType")
                        {
                            sb.Append("OnlineListQA.QAType = ").Append(item.ToString());
                        }
                        else if (key == "OnlineAreaType")
                        {
                            sb.Append("OnlineListQA.OnlineAreaType=").Append(item.ToString());
                        }
                        else if (key == "CategorySysNo")
                        {
                            sb.Append("OnlineListQA.CategorySysNo=").Append(item.ToString());
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
                }
                sqlHomePage += sb.ToString();
                sqlCategory1 += sb.ToString();
                sqlCategory2 += sb.ToString();
                sqlCategory3 += sb.ToString();
                sqlItemDetail += sb.ToString();
            }
            
            if (topNumber != AppConst.IntNull)
            {
                sqlHomePage = sqlHomePage.Replace("@top", " top " + topNumber);
                sqlCategory1 = sqlCategory1.Replace("@top", " top " + topNumber);
                sqlCategory2 = sqlCategory2.Replace("@top", " top " + topNumber);
                sqlCategory3 = sqlCategory3.Replace("@top", " top " + topNumber);
                sqlItemDetail = sqlItemDetail.Replace("@top", " top " + topNumber);
            }
            else
            {
                sqlHomePage = sqlHomePage.Replace("@top", "");
                sqlCategory1 = sqlCategory1.Replace("@top", "");
                sqlCategory2 = sqlCategory2.Replace("@top", "");
                sqlCategory3 = sqlCategory3.Replace("@top", "");
                sqlItemDetail = sqlItemDetail.Replace("@top", "");
            }
            if (sqlOrderBy.Length == 0)
                sqlOrderBy = " order by SysNo desc";
            string sql = "(" + sqlHomePage + ") union all (" + sqlCategory1 + ") union all (" + sqlCategory2 + ") union all (" + sqlCategory3 + ") union all (" + sqlItemDetail + ") " + sqlOrderBy;
            return SqlHelper.ExecuteDataSet(sql);
        }

        public string GetHomePageQA()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div id=mr_faq_b2><img src='../images/site/mr_faq_title2.gif' />");
            sb.Append(    "<ul>");

            Hashtable ht = new Hashtable();
            ht.Add("OnlineAreaType",(int)AppEnum.OnlineAreaType.HomePage);
            DataSet ds = GetOnlineListQADs(ht, 4);
            if (!Util.HasMoreRow(ds))
                return "";

            foreach(DataRow dr in ds.Tables[0].Rows)
            {
                string Question = Util.TrimNull(dr["Question"]);
                Question = Question.Length < 30?Question:Question.Substring(0,30) + "..";
                sb.Append("<li><a href='../Items/QADetail.aspx?ID="+ Util.TrimIntNull(dr["sysno"].ToString()) +"' target='_blank'>"+ Question +"</a></li>");
            }
            sb.Append(    "</ul>");
            sb.Append("<span><img src='../images/cat/list_1.gif' /><a href='#'>更多..</a></span>");
            sb.Append("</div>");

            return sb.ToString();
        }

        public string GetC1RightQA(int c1SysNo)
        {
            Hashtable ht = new Hashtable(5);
            ht.Add("OnlineAreaType", (int)AppEnum.OnlineAreaType.FirstCategory);
            ht.Add("CategorySysNo", c1SysNo);

            DataSet ds = OnlineListQAManager.GetInstance().GetOnlineListQADs(ht, AppConst.IntNull);
            if (!Util.HasMoreRow(ds))
                return "";

            StringBuilder sb = new StringBuilder();
            sb.Append("<div id=fl_ztlb class=panelr>");
            sb.Append(  "<div class=panelr_title>");
            //sb.Append(    "<div class=panelr_more><a href=#><img src='../images/site/main/left/more.png' /></a></div>");
            sb.Append(    "<div class=panelr_more></div>");
            sb.Append(    "<img src='../images/site/main/right/tt_zjlb.png' alt='专题列表' /></div>");
            sb.Append(    "<div class=panelr_content>");
            sb.Append(      "<div class=c_zxdc>");

            foreach(DataRow dr in ds.Tables[0].Rows)
            {
                if (Util.TrimNull(dr["Question"]).Length > 20)
                {
                    sb.Append("<div class=c_zxgg_li><a href='../Items/QADetail.aspx?ID=" + Util.TrimIntNull(dr["SysNo"]) + "'>" + Util.TrimNull(dr["Question"]).Substring(0, 20) + "..." + "</a></div>");
                }
                else
                {
                    sb.Append("<div class=c_zxgg_li><a href='../Items/QADetail.aspx?ID="+ Util.TrimIntNull(dr["SysNo"]) +"'>" + Util.TrimNull(dr["Question"]) + "</a></div>");
                }
            }
            sb.Append(    "</div>");
            sb.Append(  "</div>");
            sb.Append("</div>");

            return sb.ToString();
        }

        public string GetAreaQADiv(Hashtable ht,string Position,int Top)
        {
            DataSet ds = OnlineListQAManager.GetInstance().GetOnlineListQADs(ht, Top);
            int QAType = Int32.Parse(ht["QAType"].ToString());
            string QATypeName = AppEnum.GetQAType(QAType);
            if (!Util.HasMoreRow(ds))
                return "";

            StringBuilder sb = new StringBuilder();
            if(Position == "Left")
            {
                sb.Append("<div id=fl_yxxt class=panel>");
                sb.Append("<div class=panel_title>");
                sb.Append("<div class=panel_more><a href='../Service/News.aspx?Type=Article&Type2=" + QAType + "' style='color:#205E8A;' target='_blank'>更多...</a></div>");
                sb.Append(QATypeName + "</div>");
                sb.Append("<div class=panel_content>");
                sb.Append("<div class=c_yxxt>");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (Util.TrimNull(dr["Question"]).Length > 20)
                    {
                        sb.Append("<span><a href='../Service/NewsDetail.aspx?Type=Article&ID=" + Util.TrimIntNull(dr["SysNo"]) + "' target='_blank'>" + Util.TrimNull(dr["Question"]).Substring(0, 20) + "..." + "</a></span>");
                    }
                    else
                    {
                        sb.Append("<span><a href='../Service/NewsDetail.aspx?Type=Article&ID=" + Util.TrimIntNull(dr["SysNo"]) + "' target='_blank'>" + Util.TrimNull(dr["Question"]) + "</a></span>");
                    }
                }
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</div>");
            }
            else if(Position == "Right")
            {
                sb.Append("<div id=fl_ztlb class=panelr>");
                sb.Append("<div class=panelr_title>");
                sb.Append("<div class=panelr_more><a href='../Service/News.aspx?Type=Article&Type2=" + QAType + "' style='color:#205E8A;' target='_blank'>更多...</a></div>");
                sb.Append(QATypeName + "</div>");
                sb.Append("<div class=panelr_content>");
                sb.Append("<div class=c_zxdc>");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (Util.TrimNull(dr["Question"]).Length > 20)
                    {
                        sb.Append("<div class=c_zxgg_li><a href='../Service/NewsDetail.aspx?Type=Article&ID=" + Util.TrimIntNull(dr["SysNo"]) + "' target='_blank'>" + Util.TrimNull(dr["Question"]).Substring(0, 20) + "..." + "</a></div>");
                    }
                    else
                    {
                        sb.Append("<div class=c_zxgg_li><a href='../Service/NewsDetail.aspx?Type=Article&ID=" + Util.TrimIntNull(dr["SysNo"]) + "' target='_blank'>" + Util.TrimNull(dr["Question"]) + "</a></div>");
                    }
                }
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</div>");
            }

            return sb.ToString();
        }

        public string GetLatestArticleQADiv(int Type,int Top)
        {
            string sql = "select top "+ Top +" sysno,question,createtime from qa where status=0 and type=" + Type + " order by createtime desc";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return "";

            StringBuilder sb = new StringBuilder();
            sb.Append("<div id=zxzt class=panelr>");
            sb.Append("<div class=panelr_title1>");
            sb.Append(     "<div class=title>"+ AppEnum.GetQAType(Type) +"&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href='../Service/News.aspx?Type=Article&Type2="+ Type +"' style='color:#205E8A; padding-right:3px;'>更多...</a></div>");
            sb.Append(     "</div>");
            sb.Append("<div class=cxgg_content>");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sb.Append("<div class=cxgg_li><a href='../Service/NewsDetail.aspx?Type=Article&ID=" + Util.TrimIntNull(dr["sysno"].ToString()) + "'>" + Util.TrimNull(dr["question"]) + "</a></div>");
            }
            sb.Append(  "</div>");
            sb.Append("</div>");
            return sb.ToString();
        }

        public Hashtable GetOnlineListQASysNo(int Type)
        {
            string sql = @"select sysno from qa where status=0 and type=" + Type + " order by ordernum, createtime desc";
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

        public string GetOnlineListQAPageDiv(int Type,int currentPage)
        {
            string sql = @"select sysno,question,createtime
								from 
									qa
								where status = 0 and type=@type
								order by ordernum desc,createtime desc";
            sql = sql.Replace("@type", Type.ToString());
            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return "";

            int pageSize = 10;
            int totalRowCount = ds.Tables[0].Rows.Count;
            int totalPage = totalRowCount / pageSize;
            if (ds.Tables[0].Rows.Count % pageSize != 0)
                totalPage += 1;

            if (currentPage > totalPage)
                currentPage = 1;
            int rowindex = 0;
            StringBuilder sb = new StringBuilder();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (rowindex >= (currentPage - 1) * pageSize && rowindex < currentPage * pageSize)
                {
                    sb.Append("<div class=xwgg_li><a href='../Service/NewsDetail.aspx?Type=Article&ID="+ Util.TrimNull(dr["sysno"]) +"' class=acolor>"+ Util.TrimNull(dr["question"]) +"</a><span class=time>"+ Util.TrimNull(dr["createtime"]) +"</span></div>");
                }
                rowindex++;
            }

            return sb.ToString();
        }
    }
}