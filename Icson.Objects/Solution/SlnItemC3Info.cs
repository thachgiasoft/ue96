using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Solution
{
    public class SlnItemC3Info
    {
        public SlnItemC3Info()
        {
            Init();
        }

        public int SysNo;
        public int SlnItemSysNo;
        public int C3SysNo;
        public int Status;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            SlnItemSysNo = AppConst.IntNull;
            C3SysNo = AppConst.IntNull;
            Status = AppConst.IntNull;
        }
    }
}
