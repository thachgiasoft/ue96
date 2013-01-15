using System;
using System.Collections.Generic;
using System.Text;

using Icson.Utils;

namespace Icson.Objects.Sale
{
    public class DeliveryManSetListInfo
    {
        public int SysNo;
        public string ItemID;
        public int SetUserSysNo;
        public int FreightUserSysNo;
        public DateTime CreateTime;
        public int DLSysNo;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            ItemID = AppConst.StringNull;
            SetUserSysNo = AppConst.IntNull;
            FreightUserSysNo = AppConst.IntNull;
            CreateTime = AppConst.DateTimeNull;
            DLSysNo = AppConst.IntNull;
        }
    }
}