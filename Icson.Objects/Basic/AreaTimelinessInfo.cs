using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Basic
{
    public class AreaTimelinessInfo
    {
        public int SysNo;
        public int ShipTypeSysNo;
        public int AreaSysNo;
        public int Status;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            ShipTypeSysNo = AppConst.IntNull;
            AreaSysNo = AppConst.IntNull;
            Status = AppConst.IntNull;
        }
    }
}