using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Basic
{
    public class ProductRelatedInfo
    {
        public ProductRelatedInfo()
        {
            Init();
        }

        public int SysNo;
        public int MasterProductSysNo;
        public int RelatedProductSysNo;
        public int CreateUserSysNo;
        public DateTime CreateTime;
        public int Status;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            MasterProductSysNo = AppConst.IntNull;
            RelatedProductSysNo = AppConst.IntNull;
            CreateUserSysNo = AppConst.IntNull;
            CreateTime = AppConst.DateTimeNull;
            Status = AppConst.IntNull;
        }
    }
}
