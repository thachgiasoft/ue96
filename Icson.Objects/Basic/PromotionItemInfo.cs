using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;


namespace Icson.Objects.Basic
{
    public class PromotionItemInfo
    {

        public PromotionItemInfo()
        {
            Init();
        }

        public int SysNo;
        public int PromotionSysNo;
        public string ItemID;
        public string ItemName;
        public int ItemOrder;
        public int Status;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            PromotionSysNo = AppConst.IntNull;
            ItemID = AppConst.StringNull;
            ItemName = AppConst.StringNull;
            ItemOrder = AppConst.IntNull;
            Status = AppConst.IntNull;
        }
    }
}
