using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Review
{
    public class ReviewRemarkInfo
    {
        public int SysNo;
        public int ReviewSysNo;
        public int IsHelpful;
        public int CreateUserType;
        public int CreateUserSysNo;
        public DateTime CreateDate;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            ReviewSysNo = AppConst.IntNull;
            IsHelpful = AppConst.IntNull;
            CreateUserType = AppConst.IntNull;
            CreateUserSysNo = AppConst.IntNull;
            CreateDate = AppConst.DateTimeNull;
        }
    }
}
