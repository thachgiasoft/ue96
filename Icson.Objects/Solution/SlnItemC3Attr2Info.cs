using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Solution
{
    public class SlnItemC3Attr2Info
    {
        public SlnItemC3Attr2Info()
        {
            Init();
        }

        public int SysNo;
        public int SlnItemC3SysNo;
        public int C3Attr2SysNo;
        public int Status;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            SlnItemC3SysNo = AppConst.IntNull;
            C3Attr2SysNo = AppConst.IntNull;
            Status = AppConst.IntNull;
        }
    }
}
