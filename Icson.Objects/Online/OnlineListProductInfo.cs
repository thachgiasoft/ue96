using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Online
{
    public class OnlineListProductInfo
    {
        public int SysNo;
        public int OnlineAreaType;
        public int OnlineRecommendType;
        public int CategorySysNo;
        public int ProductSysNo;
        public int CreateUserSysNo;
        public DateTime CreateTime;
        public int ListOrder;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            OnlineAreaType = AppConst.IntNull;
            OnlineRecommendType = AppConst.IntNull;
            CategorySysNo = AppConst.IntNull;
            ProductSysNo = AppConst.IntNull;
            CreateUserSysNo = AppConst.IntNull;
            CreateTime = AppConst.DateTimeNull;
            ListOrder = AppConst.IntNull;
        }
    }
}
