using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Review
{
    public class ReviewComplainInfo
    {
        public int SysNo;
        public int ReviewSysNo;
        public string ComplainContent;
        public int CreateUserType;
        public int CreateUserSysNo;
        public DateTime CreateDate;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            ReviewSysNo = AppConst.IntNull;
            ComplainContent = AppConst.StringNull;
            CreateUserType = AppConst.IntNull;
            CreateUserSysNo = AppConst.IntNull;
            CreateDate = AppConst.DateTimeNull;
        }
    }
}
