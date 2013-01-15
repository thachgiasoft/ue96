using System;
using System.Collections.Generic;
using System.Text;
using Icson.Objects;
using Icson.Objects.Online;
using Icson.DBAccess;
using Icson.DBAccess.Online;
using System.Data;
using Icson.Utils;
using System.Collections;

namespace Icson.BLL.Online
{
    public class QuestionnaireSurveyManager
    {
        private QuestionnaireSurveyManager()
        {
        }
        private static QuestionnaireSurveyManager _instance;
        public static QuestionnaireSurveyManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new QuestionnaireSurveyManager();
            }
            return _instance;
        }

        private void map(QuestionnaireSurveyInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.BatchNo = Util.TrimIntNull(tempdr["BatchNo"]);
            oParam.Subject1 = Util.TrimNull(tempdr["Subject1"]);
            oParam.Subject2 = Util.TrimNull(tempdr["Subject2"]);
            oParam.Subject3 = Util.TrimNull(tempdr["Subject3"]);
            oParam.Subject4 = Util.TrimNull(tempdr["Subject4"]);
            oParam.Subject5 = Util.TrimNull(tempdr["Subject5"]);
            oParam.Subject6 = Util.TrimNull(tempdr["Subject6"]);
            oParam.Subject7 = Util.TrimNull(tempdr["Subject7"]);
            oParam.Subject8 = Util.TrimNull(tempdr["Subject8"]);
            oParam.Subject9 = Util.TrimNull(tempdr["Subject9"]);
            oParam.Subject10 = Util.TrimNull(tempdr["Subject10"]);
            oParam.Subject11 = Util.TrimNull(tempdr["Subject11"]);
            oParam.Subject12 = Util.TrimNull(tempdr["Subject12"]);
            oParam.Subject13 = Util.TrimNull(tempdr["Subject13"]);
            oParam.Subject14 = Util.TrimNull(tempdr["Subject14"]);
            oParam.Subject15 = Util.TrimNull(tempdr["Subject15"]);
            oParam.Subject16 = Util.TrimNull(tempdr["Subject16"]);
            oParam.Subject17 = Util.TrimNull(tempdr["Subject17"]);
            oParam.Subject18 = Util.TrimNull(tempdr["Subject18"]);
            oParam.Subject19 = Util.TrimNull(tempdr["Subject19"]);
            oParam.Subject20 = Util.TrimNull(tempdr["Subject20"]);
            oParam.Suggest = Util.TrimNull(tempdr["Suggest"]);
            oParam.SubmitCustomerSysNo = Util.TrimIntNull(tempdr["SubmitCustomerSysNo"]);
            oParam.SubmitTime = Util.TrimDateNull(tempdr["SubmitTime"]);
            oParam.ReplyNote = Util.TrimNull(tempdr["ReplyNote"]);
            oParam.ReplyUserSysNo = Util.TrimIntNull(tempdr["ReplyUserSysNo"]);
            oParam.ReplyTime = Util.TrimDateNull(tempdr["ReplyTime"]);
        }

        public void Insert(QuestionnaireSurveyInfo oParam)
        {
            string sql = "Select top 1 * from QuestionnaireSurvey Where SubmitCustomerSysNo = " + oParam.SubmitCustomerSysNo + " and BatchNo =" + oParam.BatchNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                throw new BizException("您已经提交了ORS商城用户结构调查，不能再次提交，感谢您对ORS商城的支持！");
            new QuestionnaireSurveyDac().Insert(oParam);
        }

        public DataSet GetQuestionnaireSurveyList(Hashtable paramHash)
        {
            string sql = @"
SELECT  dbo.QuestionnaireSurvey.*,
        dbo.Customer.CustomerName,
        dbo.Sys_User.UserName ReplyName
FROM    dbo.QuestionnaireSurvey 
        LEFT JOIN dbo.Customer (NOLOCK) ON dbo.Customer.SysNo = dbo.QuestionnaireSurvey.SubmitCustomerSysNo
        LEFT JOIN dbo.Sys_User (NOLOCK) ON dbo.Sys_User.SysNo = dbo.QuestionnaireSurvey.ReplyUserSysNo
";
            if (paramHash != null && paramHash.Count != 0)
            {
                if (!paramHash.ContainsKey("BatchNo"))
                    throw new BizException("缺少参数");
                else
                {
                    sql += " WHERE BatchNo =" + paramHash["BatchNo"].ToString();
                    if (paramHash.ContainsKey("SubmitCustomerSysNo"))
                        sql += " AND SubmitCustomerSysNo=" + paramHash["SubmitCustomerSysNo"].ToString();
                    if (paramHash.ContainsKey("SysNo"))
                        sql += " AND dbo.QuestionnaireSurvey.SysNo=" + paramHash["SysNo"].ToString();
                }
            }
            else
            {
                throw new BizException("缺少参数");
            }
            return SqlHelper.ExecuteDataSet(sql);
        }

        public QuestionnaireSurveyInfo LoadQuestionnaireSurvey(Hashtable paramHash)
        {
            DataSet ds = GetQuestionnaireSurveyList(paramHash);
            if (Util.HasMoreRow(ds))
            {
                QuestionnaireSurveyInfo oInfo = new QuestionnaireSurveyInfo();
                map(oInfo, ds.Tables[0].Rows[0]);
                return oInfo;
            }
            else
                return null;
        }

        public DataSet GetQuestionnaireSurveyCount(Hashtable paramHash)
        {//ckSubject3_A
            if (paramHash != null && paramHash.Count > 0)
            {
                if (!paramHash.ContainsKey("BatchNo"))
                    throw new BizException("缺少参数");
                string BatchNo = paramHash["BatchNo"].ToString();

                string sql = string.Empty;
                string sqlColumns = @"
SELECT Batch.BatchNo,
       ISNULL(Batch.CountNum,0) CountNum";

                string sqlJoin = @" 
       FROM (  SELECT COUNT(*) CountNum,
                      BatchNo 
               FROM   dbo.QuestionnaireSurvey
               WHERE  BatchNo=" + BatchNo + @"
               Group By BatchNo
            ) Batch ";

                foreach (string key in paramHash.Keys)
                {
                    if (key != "BatchNo")
                    {
                        sqlColumns += @",
       ISNULL(" + key + "." + key.Split('_')[1] + ",0) AS " + key;

                        sqlJoin += @"
        LEFT JOIN ( SELECT COUNT(*) AS " + key.Split('_')[1] + @",
                           BatchNo
                    FROM   dbo.QuestionnaireSurvey
                    WHERE  BatchNo =" + BatchNo + @"
                           AND " + key.Split('_')[0].Replace("ck", "") + " LIKE " + Util.ToSqlLikeString(key.Split('_')[1]) + @"
                    GROUP BY BatchNo
                   ) " + key + " ON " + key + ".BatchNo = Batch.BatchNo";
                    }
                }

                sql = sqlColumns + sqlJoin;
                return SqlHelper.ExecuteDataSet(sql);
            }
            else
                throw new BizException("缺少参数");
        }
    }
}
