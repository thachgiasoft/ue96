using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;
using Icson.Objects;

namespace Icson.Objects.RMA
{
  public  class RMAContactCustomerInfo
    {
        public RMAContactCustomerInfo()
        {
            Init();
        }

        public int SysNo;
        public int RegisterSysNo;
        public string Content;
        public int ContactUserSysNo;
        public DateTime CreateTime;
        public DateTime NextContactTime;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            RegisterSysNo = AppConst.IntNull;
            Content = AppConst.StringNull;
            ContactUserSysNo = AppConst.IntNull;
            CreateTime = AppConst.DateTimeNull;
            NextContactTime = AppConst.DateTimeNull;
        }

    }
}
