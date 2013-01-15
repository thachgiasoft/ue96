using System;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects.Online;

namespace Icson.DBAccess.Online
{
    public class QuestionnaireSurveyDac
    {
        public QuestionnaireSurveyDac()
        { }

        public int Insert(QuestionnaireSurveyInfo oParam)
        {
            string sql = @"INSERT INTO QuestionnaireSurvey
                            (
                            BatchNo, Subject1, Subject2, Subject3, 
                            Subject4, Subject5, Subject6, Subject7, 
                            Subject8, Subject9, Subject10, Subject11, 
                            Subject12, Subject13, Subject14, Subject15, 
                            Subject16, Subject17, Subject18, Subject19, 
                            Subject20, Suggest, SubmitCustomerSysNo, SubmitTime, 
                            ReplyNote, ReplyUserSysNo, ReplyTime
                            )
                            VALUES (
                            @BatchNo, @Subject1, @Subject2, @Subject3, 
                            @Subject4, @Subject5, @Subject6, @Subject7, 
                            @Subject8, @Subject9, @Subject10, @Subject11, 
                            @Subject12, @Subject13, @Subject14, @Subject15, 
                            @Subject16, @Subject17, @Subject18, @Subject19, 
                            @Subject20, @Suggest, @SubmitCustomerSysNo, @SubmitTime, 
                            @ReplyNote, @ReplyUserSysNo, @ReplyTime
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramBatchNo = new SqlParameter("@BatchNo", SqlDbType.Int, 4);
            SqlParameter paramSubject1 = new SqlParameter("@Subject1", SqlDbType.NVarChar, 20);
            SqlParameter paramSubject2 = new SqlParameter("@Subject2", SqlDbType.NVarChar, 20);
            SqlParameter paramSubject3 = new SqlParameter("@Subject3", SqlDbType.NVarChar, 20);
            SqlParameter paramSubject4 = new SqlParameter("@Subject4", SqlDbType.NVarChar, 20);
            SqlParameter paramSubject5 = new SqlParameter("@Subject5", SqlDbType.NVarChar, 20);
            SqlParameter paramSubject6 = new SqlParameter("@Subject6", SqlDbType.NVarChar, 20);
            SqlParameter paramSubject7 = new SqlParameter("@Subject7", SqlDbType.NVarChar, 20);
            SqlParameter paramSubject8 = new SqlParameter("@Subject8", SqlDbType.NVarChar, 20);
            SqlParameter paramSubject9 = new SqlParameter("@Subject9", SqlDbType.NVarChar, 20);
            SqlParameter paramSubject10 = new SqlParameter("@Subject10", SqlDbType.NVarChar, 20);
            SqlParameter paramSubject11 = new SqlParameter("@Subject11", SqlDbType.NVarChar, 20);
            SqlParameter paramSubject12 = new SqlParameter("@Subject12", SqlDbType.NVarChar, 20);
            SqlParameter paramSubject13 = new SqlParameter("@Subject13", SqlDbType.NVarChar, 20);
            SqlParameter paramSubject14 = new SqlParameter("@Subject14", SqlDbType.NVarChar, 20);
            SqlParameter paramSubject15 = new SqlParameter("@Subject15", SqlDbType.NVarChar, 20);
            SqlParameter paramSubject16 = new SqlParameter("@Subject16", SqlDbType.NVarChar, 20);
            SqlParameter paramSubject17 = new SqlParameter("@Subject17", SqlDbType.NVarChar, 20);
            SqlParameter paramSubject18 = new SqlParameter("@Subject18", SqlDbType.NVarChar, 20);
            SqlParameter paramSubject19 = new SqlParameter("@Subject19", SqlDbType.NVarChar, 20);
            SqlParameter paramSubject20 = new SqlParameter("@Subject20", SqlDbType.NVarChar, 20);
            SqlParameter paramSuggest = new SqlParameter("@Suggest", SqlDbType.NVarChar, 200);
            SqlParameter paramSubmitCustomerSysNo = new SqlParameter("@SubmitCustomerSysNo", SqlDbType.Int, 4);
            SqlParameter paramSubmitTime = new SqlParameter("@SubmitTime", SqlDbType.DateTime);
            SqlParameter paramReplyNote = new SqlParameter("@ReplyNote", SqlDbType.NVarChar, 200);
            SqlParameter paramReplyUserSysNo = new SqlParameter("@ReplyUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramReplyTime = new SqlParameter("@ReplyTime", SqlDbType.DateTime);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.BatchNo != AppConst.IntNull)
                paramBatchNo.Value = oParam.BatchNo;
            else
                paramBatchNo.Value = System.DBNull.Value;
            if (oParam.Subject1 != AppConst.StringNull)
                paramSubject1.Value = oParam.Subject1;
            else
                paramSubject1.Value = System.DBNull.Value;
            if (oParam.Subject2 != AppConst.StringNull)
                paramSubject2.Value = oParam.Subject2;
            else
                paramSubject2.Value = System.DBNull.Value;
            if (oParam.Subject3 != AppConst.StringNull)
                paramSubject3.Value = oParam.Subject3;
            else
                paramSubject3.Value = System.DBNull.Value;
            if (oParam.Subject4 != AppConst.StringNull)
                paramSubject4.Value = oParam.Subject4;
            else
                paramSubject4.Value = System.DBNull.Value;
            if (oParam.Subject5 != AppConst.StringNull)
                paramSubject5.Value = oParam.Subject5;
            else
                paramSubject5.Value = System.DBNull.Value;
            if (oParam.Subject6 != AppConst.StringNull)
                paramSubject6.Value = oParam.Subject6;
            else
                paramSubject6.Value = System.DBNull.Value;
            if (oParam.Subject7 != AppConst.StringNull)
                paramSubject7.Value = oParam.Subject7;
            else
                paramSubject7.Value = System.DBNull.Value;
            if (oParam.Subject8 != AppConst.StringNull)
                paramSubject8.Value = oParam.Subject8;
            else
                paramSubject8.Value = System.DBNull.Value;
            if (oParam.Subject9 != AppConst.StringNull)
                paramSubject9.Value = oParam.Subject9;
            else
                paramSubject9.Value = System.DBNull.Value;
            if (oParam.Subject10 != AppConst.StringNull)
                paramSubject10.Value = oParam.Subject10;
            else
                paramSubject10.Value = System.DBNull.Value;
            if (oParam.Subject11 != AppConst.StringNull)
                paramSubject11.Value = oParam.Subject11;
            else
                paramSubject11.Value = System.DBNull.Value;
            if (oParam.Subject12 != AppConst.StringNull)
                paramSubject12.Value = oParam.Subject12;
            else
                paramSubject12.Value = System.DBNull.Value;
            if (oParam.Subject13 != AppConst.StringNull)
                paramSubject13.Value = oParam.Subject13;
            else
                paramSubject13.Value = System.DBNull.Value;
            if (oParam.Subject14 != AppConst.StringNull)
                paramSubject14.Value = oParam.Subject14;
            else
                paramSubject14.Value = System.DBNull.Value;
            if (oParam.Subject15 != AppConst.StringNull)
                paramSubject15.Value = oParam.Subject15;
            else
                paramSubject15.Value = System.DBNull.Value;
            if (oParam.Subject16 != AppConst.StringNull)
                paramSubject16.Value = oParam.Subject16;
            else
                paramSubject16.Value = System.DBNull.Value;
            if (oParam.Subject17 != AppConst.StringNull)
                paramSubject17.Value = oParam.Subject17;
            else
                paramSubject17.Value = System.DBNull.Value;
            if (oParam.Subject18 != AppConst.StringNull)
                paramSubject18.Value = oParam.Subject18;
            else
                paramSubject18.Value = System.DBNull.Value;
            if (oParam.Subject19 != AppConst.StringNull)
                paramSubject19.Value = oParam.Subject19;
            else
                paramSubject19.Value = System.DBNull.Value;
            if (oParam.Subject20 != AppConst.StringNull)
                paramSubject20.Value = oParam.Subject20;
            else
                paramSubject20.Value = System.DBNull.Value;
            if (oParam.Suggest != AppConst.StringNull)
                paramSuggest.Value = oParam.Suggest;
            else
                paramSuggest.Value = System.DBNull.Value;
            if (oParam.SubmitCustomerSysNo != AppConst.IntNull)
                paramSubmitCustomerSysNo.Value = oParam.SubmitCustomerSysNo;
            else
                paramSubmitCustomerSysNo.Value = System.DBNull.Value;
            if (oParam.SubmitTime != AppConst.DateTimeNull)
                paramSubmitTime.Value = oParam.SubmitTime;
            else
                paramSubmitTime.Value = System.DBNull.Value;
            if (oParam.ReplyNote != AppConst.StringNull)
                paramReplyNote.Value = oParam.ReplyNote;
            else
                paramReplyNote.Value = System.DBNull.Value;
            if (oParam.ReplyUserSysNo != AppConst.IntNull)
                paramReplyUserSysNo.Value = oParam.ReplyUserSysNo;
            else
                paramReplyUserSysNo.Value = System.DBNull.Value;
            if (oParam.ReplyTime != AppConst.DateTimeNull)
                paramReplyTime.Value = oParam.ReplyTime;
            else
                paramReplyTime.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramBatchNo);
            cmd.Parameters.Add(paramSubject1);
            cmd.Parameters.Add(paramSubject2);
            cmd.Parameters.Add(paramSubject3);
            cmd.Parameters.Add(paramSubject4);
            cmd.Parameters.Add(paramSubject5);
            cmd.Parameters.Add(paramSubject6);
            cmd.Parameters.Add(paramSubject7);
            cmd.Parameters.Add(paramSubject8);
            cmd.Parameters.Add(paramSubject9);
            cmd.Parameters.Add(paramSubject10);
            cmd.Parameters.Add(paramSubject11);
            cmd.Parameters.Add(paramSubject12);
            cmd.Parameters.Add(paramSubject13);
            cmd.Parameters.Add(paramSubject14);
            cmd.Parameters.Add(paramSubject15);
            cmd.Parameters.Add(paramSubject16);
            cmd.Parameters.Add(paramSubject17);
            cmd.Parameters.Add(paramSubject18);
            cmd.Parameters.Add(paramSubject19);
            cmd.Parameters.Add(paramSubject20);
            cmd.Parameters.Add(paramSuggest);
            cmd.Parameters.Add(paramSubmitCustomerSysNo);
            cmd.Parameters.Add(paramSubmitTime);
            cmd.Parameters.Add(paramReplyNote);
            cmd.Parameters.Add(paramReplyUserSysNo);
            cmd.Parameters.Add(paramReplyTime);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Update(QuestionnaireSurveyInfo oParam)
        {
            string sql = @"UPDATE QuestionnaireSurvey SET 
                            BatchNo=@BatchNo, Subject1=@Subject1, 
                            Subject2=@Subject2, Subject3=@Subject3, 
                            Subject4=@Subject4, Subject5=@Subject5, 
                            Subject6=@Subject6, Subject7=@Subject7, 
                            Subject8=@Subject8, Subject9=@Subject9, 
                            Subject10=@Subject10, Subject11=@Subject11, 
                            Subject12=@Subject12, Subject13=@Subject13, 
                            Subject14=@Subject14, Subject15=@Subject15, 
                            Subject16=@Subject16, Subject17=@Subject17, 
                            Subject18=@Subject18, Subject19=@Subject19, 
                            Subject20=@Subject20, Suggest=@Suggest, 
                            SubmitCustomerSysNo=@SubmitCustomerSysNo, SubmitTime=@SubmitTime, 
                            ReplyNote=@ReplyNote, ReplyUserSysNo=@ReplyUserSysNo, 
                            ReplyTime=@ReplyTime
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramBatchNo = new SqlParameter("@BatchNo", SqlDbType.Int, 4);
            SqlParameter paramSubject1 = new SqlParameter("@Subject1", SqlDbType.NVarChar, 20);
            SqlParameter paramSubject2 = new SqlParameter("@Subject2", SqlDbType.NVarChar, 20);
            SqlParameter paramSubject3 = new SqlParameter("@Subject3", SqlDbType.NVarChar, 20);
            SqlParameter paramSubject4 = new SqlParameter("@Subject4", SqlDbType.NVarChar, 20);
            SqlParameter paramSubject5 = new SqlParameter("@Subject5", SqlDbType.NVarChar, 20);
            SqlParameter paramSubject6 = new SqlParameter("@Subject6", SqlDbType.NVarChar, 20);
            SqlParameter paramSubject7 = new SqlParameter("@Subject7", SqlDbType.NVarChar, 20);
            SqlParameter paramSubject8 = new SqlParameter("@Subject8", SqlDbType.NVarChar, 20);
            SqlParameter paramSubject9 = new SqlParameter("@Subject9", SqlDbType.NVarChar, 20);
            SqlParameter paramSubject10 = new SqlParameter("@Subject10", SqlDbType.NVarChar, 20);
            SqlParameter paramSubject11 = new SqlParameter("@Subject11", SqlDbType.NVarChar, 20);
            SqlParameter paramSubject12 = new SqlParameter("@Subject12", SqlDbType.NVarChar, 20);
            SqlParameter paramSubject13 = new SqlParameter("@Subject13", SqlDbType.NVarChar, 20);
            SqlParameter paramSubject14 = new SqlParameter("@Subject14", SqlDbType.NVarChar, 20);
            SqlParameter paramSubject15 = new SqlParameter("@Subject15", SqlDbType.NVarChar, 20);
            SqlParameter paramSubject16 = new SqlParameter("@Subject16", SqlDbType.NVarChar, 20);
            SqlParameter paramSubject17 = new SqlParameter("@Subject17", SqlDbType.NVarChar, 20);
            SqlParameter paramSubject18 = new SqlParameter("@Subject18", SqlDbType.NVarChar, 20);
            SqlParameter paramSubject19 = new SqlParameter("@Subject19", SqlDbType.NVarChar, 20);
            SqlParameter paramSubject20 = new SqlParameter("@Subject20", SqlDbType.NVarChar, 20);
            SqlParameter paramSuggest = new SqlParameter("@Suggest", SqlDbType.NVarChar, 200);
            SqlParameter paramSubmitCustomerSysNo = new SqlParameter("@SubmitCustomerSysNo", SqlDbType.Int, 4);
            SqlParameter paramSubmitTime = new SqlParameter("@SubmitTime", SqlDbType.DateTime);
            SqlParameter paramReplyNote = new SqlParameter("@ReplyNote", SqlDbType.NVarChar, 200);
            SqlParameter paramReplyUserSysNo = new SqlParameter("@ReplyUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramReplyTime = new SqlParameter("@ReplyTime", SqlDbType.DateTime);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.BatchNo != AppConst.IntNull)
                paramBatchNo.Value = oParam.BatchNo;
            else
                paramBatchNo.Value = System.DBNull.Value;
            if (oParam.Subject1 != AppConst.StringNull)
                paramSubject1.Value = oParam.Subject1;
            else
                paramSubject1.Value = System.DBNull.Value;
            if (oParam.Subject2 != AppConst.StringNull)
                paramSubject2.Value = oParam.Subject2;
            else
                paramSubject2.Value = System.DBNull.Value;
            if (oParam.Subject3 != AppConst.StringNull)
                paramSubject3.Value = oParam.Subject3;
            else
                paramSubject3.Value = System.DBNull.Value;
            if (oParam.Subject4 != AppConst.StringNull)
                paramSubject4.Value = oParam.Subject4;
            else
                paramSubject4.Value = System.DBNull.Value;
            if (oParam.Subject5 != AppConst.StringNull)
                paramSubject5.Value = oParam.Subject5;
            else
                paramSubject5.Value = System.DBNull.Value;
            if (oParam.Subject6 != AppConst.StringNull)
                paramSubject6.Value = oParam.Subject6;
            else
                paramSubject6.Value = System.DBNull.Value;
            if (oParam.Subject7 != AppConst.StringNull)
                paramSubject7.Value = oParam.Subject7;
            else
                paramSubject7.Value = System.DBNull.Value;
            if (oParam.Subject8 != AppConst.StringNull)
                paramSubject8.Value = oParam.Subject8;
            else
                paramSubject8.Value = System.DBNull.Value;
            if (oParam.Subject9 != AppConst.StringNull)
                paramSubject9.Value = oParam.Subject9;
            else
                paramSubject9.Value = System.DBNull.Value;
            if (oParam.Subject10 != AppConst.StringNull)
                paramSubject10.Value = oParam.Subject10;
            else
                paramSubject10.Value = System.DBNull.Value;
            if (oParam.Subject11 != AppConst.StringNull)
                paramSubject11.Value = oParam.Subject11;
            else
                paramSubject11.Value = System.DBNull.Value;
            if (oParam.Subject12 != AppConst.StringNull)
                paramSubject12.Value = oParam.Subject12;
            else
                paramSubject12.Value = System.DBNull.Value;
            if (oParam.Subject13 != AppConst.StringNull)
                paramSubject13.Value = oParam.Subject13;
            else
                paramSubject13.Value = System.DBNull.Value;
            if (oParam.Subject14 != AppConst.StringNull)
                paramSubject14.Value = oParam.Subject14;
            else
                paramSubject14.Value = System.DBNull.Value;
            if (oParam.Subject15 != AppConst.StringNull)
                paramSubject15.Value = oParam.Subject15;
            else
                paramSubject15.Value = System.DBNull.Value;
            if (oParam.Subject16 != AppConst.StringNull)
                paramSubject16.Value = oParam.Subject16;
            else
                paramSubject16.Value = System.DBNull.Value;
            if (oParam.Subject17 != AppConst.StringNull)
                paramSubject17.Value = oParam.Subject17;
            else
                paramSubject17.Value = System.DBNull.Value;
            if (oParam.Subject18 != AppConst.StringNull)
                paramSubject18.Value = oParam.Subject18;
            else
                paramSubject18.Value = System.DBNull.Value;
            if (oParam.Subject19 != AppConst.StringNull)
                paramSubject19.Value = oParam.Subject19;
            else
                paramSubject19.Value = System.DBNull.Value;
            if (oParam.Subject20 != AppConst.StringNull)
                paramSubject20.Value = oParam.Subject20;
            else
                paramSubject20.Value = System.DBNull.Value;
            if (oParam.Suggest != AppConst.StringNull)
                paramSuggest.Value = oParam.Suggest;
            else
                paramSuggest.Value = System.DBNull.Value;
            if (oParam.SubmitCustomerSysNo != AppConst.IntNull)
                paramSubmitCustomerSysNo.Value = oParam.SubmitCustomerSysNo;
            else
                paramSubmitCustomerSysNo.Value = System.DBNull.Value;
            if (oParam.SubmitTime != AppConst.DateTimeNull)
                paramSubmitTime.Value = oParam.SubmitTime;
            else
                paramSubmitTime.Value = System.DBNull.Value;
            if (oParam.ReplyNote != AppConst.StringNull)
                paramReplyNote.Value = oParam.ReplyNote;
            else
                paramReplyNote.Value = System.DBNull.Value;
            if (oParam.ReplyUserSysNo != AppConst.IntNull)
                paramReplyUserSysNo.Value = oParam.ReplyUserSysNo;
            else
                paramReplyUserSysNo.Value = System.DBNull.Value;
            if (oParam.ReplyTime != AppConst.DateTimeNull)
                paramReplyTime.Value = oParam.ReplyTime;
            else
                paramReplyTime.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramBatchNo);
            cmd.Parameters.Add(paramSubject1);
            cmd.Parameters.Add(paramSubject2);
            cmd.Parameters.Add(paramSubject3);
            cmd.Parameters.Add(paramSubject4);
            cmd.Parameters.Add(paramSubject5);
            cmd.Parameters.Add(paramSubject6);
            cmd.Parameters.Add(paramSubject7);
            cmd.Parameters.Add(paramSubject8);
            cmd.Parameters.Add(paramSubject9);
            cmd.Parameters.Add(paramSubject10);
            cmd.Parameters.Add(paramSubject11);
            cmd.Parameters.Add(paramSubject12);
            cmd.Parameters.Add(paramSubject13);
            cmd.Parameters.Add(paramSubject14);
            cmd.Parameters.Add(paramSubject15);
            cmd.Parameters.Add(paramSubject16);
            cmd.Parameters.Add(paramSubject17);
            cmd.Parameters.Add(paramSubject18);
            cmd.Parameters.Add(paramSubject19);
            cmd.Parameters.Add(paramSubject20);
            cmd.Parameters.Add(paramSuggest);
            cmd.Parameters.Add(paramSubmitCustomerSysNo);
            cmd.Parameters.Add(paramSubmitTime);
            cmd.Parameters.Add(paramReplyNote);
            cmd.Parameters.Add(paramReplyUserSysNo);
            cmd.Parameters.Add(paramReplyTime);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

    }
}
