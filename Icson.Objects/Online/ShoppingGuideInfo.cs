using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Online
{
  public  class ShoppingGuideInfo
    {
        public int SysNo;
        public string Title;
        public string Url;
        public string Content;
        public int CreateUserSysNo;
        public DateTime CreateTime;
        public int Status;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            Title = AppConst.StringNull;
            Url = AppConst.StringNull;
            Content = AppConst.StringNull;
            CreateUserSysNo = AppConst.IntNull;
            CreateTime = AppConst.DateTimeNull;
            Status = AppConst.IntNull;
        }
    }
}
