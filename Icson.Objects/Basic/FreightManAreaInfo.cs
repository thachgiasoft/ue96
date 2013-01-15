using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Basic
{
    public class FreightManAreaInfo
    {
        public FreightManAreaInfo()
        {
            Init();
        }
        public int SysNo;
        public int FreightUserSysNo;
        public int AreaSysNo;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            FreightUserSysNo = AppConst.IntNull;
            AreaSysNo = AppConst.IntNull;
        }
    }
}
