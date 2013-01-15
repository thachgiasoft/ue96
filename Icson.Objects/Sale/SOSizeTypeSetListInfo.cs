using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;


namespace Icson.Objects.Sale
{
    public class SOSizeTypeSetListInfo
    {
        public int SysNo;
        public string ItemID;
        public int CreateUserSysNo;
        public DateTime CreateTime;
        public int SizeType;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            ItemID = AppConst.StringNull;
            CreateUserSysNo = AppConst.IntNull;
            CreateTime = AppConst.DateTimeNull;
            SizeType = AppConst.IntNull;
        }
    }
}
