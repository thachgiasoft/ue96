using System;
using System.Collections.Generic;
using System.Text;

using Icson.Utils;

namespace Icson.Objects.Sale
{
    public class SOWeightInfo
    {
        public int SysNo;
        public int SOSysNo;
        public decimal Weight;
        public decimal ShipPriceCustomer;
        public decimal ShipPriceVendor;
        public int CreateUserSysNo;
        public DateTime CreateTime;
        public int Status;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            SOSysNo = AppConst.IntNull;
            Weight = AppConst.DecimalNull;
            ShipPriceCustomer = AppConst.DecimalNull;
            ShipPriceVendor = AppConst.DecimalNull;
            CreateUserSysNo = AppConst.IntNull;
            CreateTime = AppConst.DateTimeNull;
            Status = AppConst.IntNull;
        }
    }
}