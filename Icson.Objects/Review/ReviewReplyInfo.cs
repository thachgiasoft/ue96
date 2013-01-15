using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Review
{
    public class ReviewReplyInfo
    {
        public int SysNo;
        public int ReviewSysNo;
        public string ReplyContent;
        public int Status;
        public int CreateUserType;
        public int CreateUserSysNo;
        public DateTime CreateDate;
        public int LastEditUserSysNo;
        public DateTime LastEditDate;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            ReviewSysNo = AppConst.IntNull;
            ReplyContent = AppConst.StringNull;
            Status = AppConst.IntNull;
            CreateUserType = AppConst.IntNull;
            CreateUserSysNo = AppConst.IntNull;
            CreateDate = AppConst.DateTimeNull;
            LastEditUserSysNo = AppConst.IntNull;
            LastEditDate = AppConst.DateTimeNull;
        }
    }
}
