using System;
using System.Collections.Generic;
using System.Text;

using Icson.Utils;

namespace Icson.Objects.Solution
{
    public class PrjItemFilterInfo
    {
        public PrjItemFilterInfo()
        {
            Init();
        }

        public int SysNo;
        public int PrjItemSysNo;
        public string Filter;
        public decimal PriceFrom;
        public decimal PriceTo;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            PrjItemSysNo = AppConst.IntNull;
            Filter = AppConst.StringNull;
            PriceFrom = AppConst.DecimalNull;
            PriceTo = AppConst.DecimalNull;
        }
    }
}
