using System;
using System.Data;
using System.Collections;
using System.Text;

using Icson.Utils;
using System.Transactions;

using Icson.DBAccess;
using Icson.DBAccess.Online;

using Icson.Objects;
using Icson.Objects.Online;

namespace Icson.BLL.Online
{
	/// <summary>
	/// Summary description for FeedBackManager.
	/// </summary>
	public class FeedBackManager
	{
		private FeedBackManager()
		{
		}
		private static FeedBackManager _instance;
		public static FeedBackManager GetInstance()
		{
			if( _instance == null )
			{
				_instance = new FeedBackManager();
			}
			return _instance;
		}
		public void Insert(FeedBackInfo oParam)
		{
			new FeedBackDac().Insert(oParam);
		}
		public void Update(Hashtable ht)
		{
			new FeedBackDac().Update(ht);
		}
		private void map(FeedBackInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.CustomerSysNo = Util.TrimIntNull(tempdr["CustomerSysNo"]);
			oParam.Subject = Util.TrimNull(tempdr["Subject"]);
			oParam.Suggest = Util.TrimNull(tempdr["Suggest"]);
			oParam.NickName = Util.TrimNull(tempdr["NickName"]);
			oParam.Email = Util.TrimNull(tempdr["Email"]);
			oParam.Phone = Util.TrimNull(tempdr["Phone"]);
			oParam.Memo = Util.TrimNull(tempdr["Memo"]);
			oParam.Note = Util.TrimNull(tempdr["Note"]);
			oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
			oParam.UpdateTime = Util.TrimDateNull(tempdr["UpdateTime"]);
            oParam.UpdateUserSysNo = Util.TrimIntNull(tempdr["UpdateUserSysNo"]);
			oParam.Status = Util.TrimIntNull(tempdr["Status"]);
            oParam.Sosysno = Util.TrimIntNull(tempdr["Sosysno"]);
		}
		public FeedBackInfo Load(int sysno)
		{
			string sql = "select * from feedback where sysno = " + sysno;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( !Util.HasMoreRow(ds))
				return null;
			FeedBackInfo oInfo = new FeedBackInfo();
			map(oInfo, ds.Tables[0].Rows[0]);
			return oInfo;
		}
		public DataSet GetFeedBackDs(Hashtable paramHash)
		{
            string sql = @" select feedback.*,su.username from feedback left join sys_user su on su.sysno=feedback.UpdateUserSysNo  where 1=1";
			if ( paramHash != null && paramHash.Count != 0 )
			{
				StringBuilder sb = new StringBuilder(100);
				foreach(string key in paramHash.Keys)
				{
					sb.Append(" and ");
					object item = paramHash[key];
					if ( key == "DateFrom")
					{
                        sb.Append("feedback.CreateTime >= ").Append(Util.ToSqlString(item.ToString()));
					}
					else if ( key == "DateTo")
					{
                        sb.Append("feedback.CreateTime <= ").Append(Util.ToSqlEndDate(item.ToString()));
					}
                    else if (key == "CustomerSysNo")
                    {
                        sb.Append("feedback.CustomerSysNo = ").Append(Util.TrimIntNull(item.ToString()));
                    }
                    else if (key == "Createtime")
                    {
                        sb.Append("feedback.CreateTime >= ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "Status")
                    {
                        sb.Append("feedback.Status =").Append(item.ToString());
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
			else
			{
				sql = sql.Replace("select", "select top 50");
			}

            sql += " order by feedback.sysno desc";
			return SqlHelper.ExecuteDataSet(sql);
		}

		public string GetFeedBackOnlineString(DataSet ds, int currentPage, int pageSize)
		{
			StringBuilder sb = new StringBuilder(1000);

			if ( !Util.HasMoreRow(ds))
			{
				sb.Append("<tr>");
				sb.Append("<td bgcolor=#FFFFFF colspan=4 valign='top'>");
				sb.Append("还没搜索到相关反馈，您有机会成为我们第一个给ORS商城网提供反馈意见的客人！！！");
				sb.Append("</td>");
				sb.Append("</tr>");
			}
			else
			{
				int totalPage = ds.Tables[0].Rows.Count / pageSize;
				if ( ds.Tables[0].Rows.Count % pageSize != 0)
					totalPage++;
				if ( currentPage > totalPage)
					currentPage = 1;
				int i = 1;
				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					if ( i > (currentPage-1)*pageSize && i<= currentPage*pageSize)
					{
						sb.Append("<tr bgcolor='#f6f6f6'><td colspan='4'><table width='100%'><tr>");
						sb.Append("<td align='left'><img border='0' src='../Images/FeedBack/man.ico' >");
						sb.Append("<font  width='3'>|</font>");
						sb.Append("<b>" + Util.FilterCompetitorKeyword(Util.TrimNull(dr["Subject"])) + "</b></td>");
						sb.Append("<td align='right'>" + Util.TrimNull(dr["NickName"]) + Util.TrimDateNull(dr["CreateTime"]).ToString(AppConst.DateFormatLong) + "</td>");
						sb.Append("</tr></table></td></tr>");
						sb.Append("<tr>");
						sb.Append("<td colspan='3' bgcolor='#ececec' height='1' style='PADDING-RIGHT:0px; PADDING-LEFT:0px; PADDING-BOTTOM:0px; OVERFLOW:hidden; PADDING-TOP:0px'></td>");
						sb.Append("</tr>");
						sb.Append("<tr>");
				
						sb.Append("<td></td>");
						sb.Append("<td >");
						sb.Append("<table border='0' width='100%'>");
						sb.Append("<tr>");
						sb.Append("<td width='100%'>" + Util.FilterCompetitorKeyword(Util.TrimNull(dr["Suggest"])) + "</td>");
						sb.Append("</tr>");
						sb.Append("<tr>");
                        if (Util.TrimIntNull(dr["status"])==(int)AppEnum.FeedBackStatus.Orgin)
                        {
                            sb.Append("<td width='100%' bgcolor='#fffde1' style='BORDER-RIGHT: #878787 1px solid; BORDER-TOP: #878787 1px solid; BORDER-LEFT: #878787 1px solid; BORDER-BOTTOM: #878787 1px solid'>&nbsp;<img border=0 src='../Images/FeedBack/star.ico' >");
                            sb.Append("<b>状态</b>： 待处理！</td>");
                        }
                        else
                        {
                            sb.Append("<td width='100%' bgcolor='#fffde1' style='BORDER-RIGHT: #878787 1px solid; BORDER-TOP: #878787 1px solid; BORDER-LEFT: #878787 1px solid; BORDER-BOTTOM: #878787 1px solid'>&nbsp;<img border=0 src='../Images/FeedBack/star.ico' >");
                            sb.Append("<b>ORS商城</b>：" + Util.FilterCompetitorKeyword(Util.TrimNull(dr["Memo"])) + "</td>");
                        }
						sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td width='100%' height='10'><img border='0' src='../Images/FeedBack/feedbacklist.blank.gif' align='left'></td>");
                        sb.Append("</tr>");
						sb.Append("</table>");
						sb.Append("</td>");
						sb.Append("</tr>");
						sb.Append("<tr>");
						sb.Append("<td colspan='3' height='1' style='PADDING-RIGHT:0px; PADDING-LEFT:0px; PADDING-BOTTOM:0px; OVERFLOW:hidden; PADDING-TOP:0px' background='../Images/FeedBack/rank_top_dotline.gif'><img border=0 src='../Images/FeedBack/feedbacklist.blank.gif' align=left width=1 height=1></td>");
						sb.Append("</tr>");
					}
					i++;
				}
			}
			return sb.ToString();
		}

		public void Import()
		{
			if ( !AppConfig.IsImportable)
				throw new BizException("Is Importable is false");

			/*  do not  use the following code after Data Pour in */
			string sql = " select top 1 * from feedback ";
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds) )
				throw new BizException("the table feedback is not empty");

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				string sql1 = @"select
									1 as sysno,
									subject,
									suggest,
									usercode as nickname,
									usermail as email,
									userphone as phone,
									Icsonoutfeed as memo,
									Icsoninfeed as note,
									createtime,
									status,
									null as updatetime
									
								from ipp2003..feedback feedback";
				DataSet ds1 = SqlHelper.ExecuteDataSet(sql1);

				/*
				 *	<option selected="selected" value="0">未处理</option>
					<option value="1">已处理</option>
					<option value="2">Icson显示</option>
					<option value="3">删除</option>
					ipp2和3采用的状态是一样的，所以不需要转换
				 */
				foreach(DataRow dr1 in ds1.Tables[0].Rows)
				{
					FeedBackInfo oFeedBack = new FeedBackInfo();
					map(oFeedBack,dr1);
					if ( oFeedBack.Subject == AppConst.StringNull)
						oFeedBack.Subject = "无";
					if ( oFeedBack.Suggest == AppConst.StringNull)
						oFeedBack.Suggest = "无";
					if ( oFeedBack.NickName == AppConst.StringNull )
						oFeedBack.NickName = "无";
					new FeedBackDac().Insert(oFeedBack);
				}
				
				
			scope.Complete();
            }
		}

        public DataSet GetFeedBackUpdateUserListDs()
        {
            string sql = @"select distinct feedback.UpdateUserSysNo,su.username as UpdateUserName 
                            from feedback 
                            inner join sys_user su on feedback.UpdateUserSysNo=su.sysno
                            where feedback.UpdateTime >=" + Util.ToSqlString(DateTime.Now.AddMonths(-3).ToString());
           return SqlHelper.ExecuteDataSet(sql);
        }
	}
}
