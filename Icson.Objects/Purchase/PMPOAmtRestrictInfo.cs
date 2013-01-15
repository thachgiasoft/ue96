using System;

using Icson.Utils;


namespace Icson.Objects.Purchase
{
    public class PMPOAmtRestrictInfo
    {
        public PMPOAmtRestrictInfo()
        {
            Init();
        }

        public int SysNo;
        public int PMUserSysNo;
        public int PMGroupNo;
        public int IsPMD;
        public decimal DailyPOAmtMax;
        public decimal EachPOAmtMax;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            PMUserSysNo = AppConst.IntNull;
            PMGroupNo = AppConst.IntNull;
            IsPMD = AppConst.IntNull;
            DailyPOAmtMax = AppConst.DecimalNull;
            EachPOAmtMax = AppConst.DecimalNull;
        }
    }
}
