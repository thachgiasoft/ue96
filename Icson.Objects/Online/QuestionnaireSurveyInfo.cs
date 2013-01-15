using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Online
{
    public class QuestionnaireSurveyInfo
    {
        public QuestionnaireSurveyInfo()
        {
            Init();
        }

        public int SysNo;
        public int BatchNo;
        public string Subject1;
        public string Subject2;
        public string Subject3;
        public string Subject4;
        public string Subject5;
        public string Subject6;
        public string Subject7;
        public string Subject8;
        public string Subject9;
        public string Subject10;
        public string Subject11;
        public string Subject12;
        public string Subject13;
        public string Subject14;
        public string Subject15;
        public string Subject16;
        public string Subject17;
        public string Subject18;
        public string Subject19;
        public string Subject20;
        public string Suggest;
        public int SubmitCustomerSysNo;
        public DateTime SubmitTime;
        public string ReplyNote;
        public int ReplyUserSysNo;
        public DateTime ReplyTime;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            BatchNo = AppConst.IntNull;
            Subject1 = AppConst.StringNull;
            Subject2 = AppConst.StringNull;
            Subject3 = AppConst.StringNull;
            Subject4 = AppConst.StringNull;
            Subject5 = AppConst.StringNull;
            Subject6 = AppConst.StringNull;
            Subject7 = AppConst.StringNull;
            Subject8 = AppConst.StringNull;
            Subject9 = AppConst.StringNull;
            Subject10 = AppConst.StringNull;
            Subject11 = AppConst.StringNull;
            Subject12 = AppConst.StringNull;
            Subject13 = AppConst.StringNull;
            Subject14 = AppConst.StringNull;
            Subject15 = AppConst.StringNull;
            Subject16 = AppConst.StringNull;
            Subject17 = AppConst.StringNull;
            Subject18 = AppConst.StringNull;
            Subject19 = AppConst.StringNull;
            Subject20 = AppConst.StringNull;
            Suggest = AppConst.StringNull;
            SubmitCustomerSysNo = AppConst.IntNull;
            SubmitTime = AppConst.DateTimeNull;
            ReplyNote = AppConst.StringNull;
            ReplyUserSysNo = AppConst.IntNull;
            ReplyTime = AppConst.DateTimeNull;
        }
    }
}
