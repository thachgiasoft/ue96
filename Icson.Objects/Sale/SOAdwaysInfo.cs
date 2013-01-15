using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Sale
{
    public class SOAdwaysInfo
    {
        public SOAdwaysInfo()
        {
            Init();
        }

        public int SysNo;
        public int CustomerSysNo;
        public string AdwaysID;
        public string AdwaysEmail;
        public int SOSysNo;
        public decimal ShipPrice;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            CustomerSysNo = AppConst.IntNull;
            AdwaysID = AppConst.StringNull;
            AdwaysEmail = AppConst.StringNull;
            SOSysNo = AppConst.IntNull;
            ShipPrice = AppConst.DecimalNull;
        }
    }
}
