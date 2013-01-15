using System;
using System.Collections.Generic;
using System.Text;

using Icson.Utils;

namespace Icson.Objects.RMA
{
    public class RMAShiftInfo
    {
        public RMAShiftInfo()
        {
            Init();
        }

        public int SysNo;
        public int RegisterSysNo;
        public int RMAShiftType;
        public int ShiftSysNo;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            RegisterSysNo = AppConst.IntNull;
            RMAShiftType = AppConst.IntNull;
            ShiftSysNo = AppConst.IntNull;
        }
    }
}
