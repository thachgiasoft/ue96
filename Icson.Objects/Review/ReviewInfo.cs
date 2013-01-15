using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Review
{
    public class ReviewInfo
    {
        public Hashtable ReviewC3ItemScoreHash = new Hashtable();

        public int SysNo;
        public int ReviewType;
        public string Title;
        public string Content1;
        public string Content2;
        public string Content3;
        public int Score;
        public int OwnedType;
        public int UnderstandingType;
        public string NickName;
        public int ReferenceType;
        public int ReferenceSysNo;
        public int IsTop;
        public int IsGood;
        public int TotalRemarkCount;
        public int TotalHelpfulRemarkCount;
        public int TotalComplainCount;
        public int Status;
        public int CreateCustomerSysNo;
        public DateTime CreateDate;
        public int LastEditUserSysNo;
        public DateTime LastEditDate;
        public string CustomerIP;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            ReviewType = AppConst.IntNull;
            Title = AppConst.StringNull;
            Content1 = AppConst.StringNull;
            Content2 = AppConst.StringNull;
            Content3 = AppConst.StringNull;
            Score = AppConst.IntNull;
            OwnedType = AppConst.IntNull;
            UnderstandingType = AppConst.IntNull;
            NickName = AppConst.StringNull;
            ReferenceType = AppConst.IntNull;
            ReferenceSysNo = AppConst.IntNull;
            IsTop = AppConst.IntNull;
            IsGood = AppConst.IntNull;
            TotalRemarkCount = AppConst.IntNull;
            TotalHelpfulRemarkCount = AppConst.IntNull;
            TotalComplainCount = AppConst.IntNull;
            Status = AppConst.IntNull;
            CreateCustomerSysNo = AppConst.IntNull;
            CreateDate = AppConst.DateTimeNull;
            LastEditUserSysNo = AppConst.IntNull;
            LastEditDate = AppConst.DateTimeNull;
            CustomerIP = AppConst.StringNull;
        }
    }
}
