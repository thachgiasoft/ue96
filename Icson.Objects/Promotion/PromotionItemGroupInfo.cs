using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;
namespace Icson.Objects.Promotion
{
   public class PromotionItemGroupInfo
    {
        public int SysNo;
        public int PromotionSysNo;
        public string GroupName;
        public int OrderNum;
        public int Status;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            PromotionSysNo = AppConst.IntNull;
            GroupName = AppConst.StringNull;
            OrderNum = AppConst.IntNull;
            Status = AppConst.IntNull;
        }
    }
}
