using System;
using System.Globalization;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Icson.DBAccess;
using Icson.DBAccess.Online;
using Icson.Utils;
using Icson.Objects.Online;

namespace Icson.BLL.Online
{
   public class FeedBackShowListManager
    {
       private FeedBackShowListManager()
       { 
       }
       private static FeedBackShowListManager _instance;
       public static FeedBackShowListManager GetInstance()
       {
           if (_instance == null)
           {
               _instance = new FeedBackShowListManager();
           }
           return _instance;
       }

       public int Insert(FeedBackShowListInfo oParam)
       {
           string sql = " select * from FeedBackShowList where FeedBackSysNo = " + oParam.FeedBackSysNo;
           DataSet ds = SqlHelper.ExecuteDataSet(sql);
           if (Util.HasMoreRow(ds))
               throw new BizException("the same FeedBack exists already");
           
           int result=new FeedBackShowListDac().Insert(oParam);
           return result;
       }

       private void map(FeedBackShowListInfo oParam, DataRow tempdr)
       {
           oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
           oParam.FeedBackSysNo = Util.TrimIntNull(tempdr["FeedBackSysNo"]);
           oParam.Type = Util.TrimIntNull(tempdr["Type"]);
           oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
       }
       public FeedBackShowListInfo Load(int FeedBacksysno,int Type)
       {
           string sql = "select * from FeedBackShowList where FeedBackSysNo = " + FeedBacksysno + "and Type=" + Type;
           DataSet ds = SqlHelper.ExecuteDataSet(sql);
           if (!Util.HasMoreRow(ds))
               return null;
           FeedBackShowListInfo oInfo = new FeedBackShowListInfo();
           map(oInfo, ds.Tables[0].Rows[0]);
           return oInfo;
       }
       public void Delete(int FeedBacksysno)
       {
           new FeedBackShowListDac().Delete(FeedBacksysno);
       }

       public string GetFeedBackShowDiv()
       {
           string sql = @"select top 1 fb.subject,fb.suggest,fb.nickName,fb.createTime  from feedbackshowlist fbsl
                          left join feedback  fb on fbsl.feedbacksysno=fb.sysno
                          order by fb.createtime desc";
           DataSet ds = SqlHelper.ExecuteDataSet(sql);
           if (!Util.HasMoreRow(ds))
               return "";
          StringBuilder sb = new StringBuilder();
          foreach(DataRow dr in ds.Tables[0].Rows )
          {
               sb.Append("<div id='fc_hy' class='panelc'>");
               sb.Append("<div class='panelc_title'>");
               sb.Append("<div class='panelc_more'><a href='../Service/Feedback.aspx' style='color:#205E8A;'>更多...</a></div>");
               sb.Append("<img src='../images/site/main/center/tt_hy.png'alt='慧眼看ORS商城'/></div>");
               sb.Append("<div class='panelc_content'>");
               sb.Append("<div class='c_hy'>");
               sb.Append("<div class='c_hy_li'>");
               sb.Append("<div class='c_hy_title'>"+Util.FilterCompetitorKeyword(Util.TrimNull(dr["subject"]))+"</div>");
               sb.Append("<div class='c_hy_p'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + Util.FilterCompetitorKeyword(Util.TrimNull(dr["Suggest"])) + "</div>");
               sb.Append("<div class='c_hy_ath'>"+Util.TrimNull(dr["nickName"])+"&nbsp;&nbsp;&nbsp;&nbsp;"+Util.TrimDateNull(dr["createtime"])+"</div>");
               sb.Append("</div>");
               sb.Append("<br clear='all'>");
               sb.Append("</div>");
               sb.Append("</div>");
               sb.Append("</div>");
          }
           return sb.ToString();

         }
          
       
    }
}
