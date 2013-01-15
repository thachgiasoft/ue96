using System;
using System.Collections.Generic;
using System.Text;

using Icson.Utils;
using Icson.Objects.Basic;
namespace Icson.Objects.Basic
{
  public class PaipaiResponseInfo
    {
        public int SysNo;
        public int ProductSysNo;
        public string PaipaiItemID;
        public DateTime CreateTime;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            ProductSysNo = AppConst.IntNull;
            PaipaiItemID = AppConst.StringNull;
            CreateTime = AppConst.DateTimeNull;
        }
    }
}
