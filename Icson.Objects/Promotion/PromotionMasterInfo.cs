using System;
using System.Collections.Generic;
using System.Text;

using Icson.Utils;
namespace Icson.Objects.Promotion
{
  public  class PromotionMasterInfo
    {
        public int SysNo;
        public string PromotionName;
        public string PromotionNote;
        public int CreateUserSysNo;
        public DateTime CreateTime;
        public int Status;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            PromotionName = AppConst.StringNull;
            PromotionNote = AppConst.StringNull;
            CreateUserSysNo = AppConst.IntNull;
            CreateTime = AppConst.DateTimeNull;
            Status = AppConst.IntNull;
        }
    }
}
