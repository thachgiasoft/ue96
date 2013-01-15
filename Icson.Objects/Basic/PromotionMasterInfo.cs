using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Basic
{
    public class PromotionMasterInfo
    {

        public PromotionMasterInfo()
        {
            Init();
        }

        public int SysNo;
        public string PromotionID;
        public string PromotionName;
        public string PromotionDesc;
        public int CreateUserSysNo;
        public DateTime CreateTime;
        public int Status;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            PromotionID = AppConst.StringNull;
            PromotionName = AppConst.StringNull;
            PromotionDesc = AppConst.StringNull;
            CreateUserSysNo = AppConst.IntNull;
            CreateTime = AppConst.DateTimeNull;
            Status = AppConst.IntNull;
        }
    }
}