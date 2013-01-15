using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Review
{
    public class ReviewC3ItemScoreInfo
    {
        public int SysNo;
        public int ReviewSysNo;
        public int C3ReviewItemSysNo;
        public int Score;
        public int Weight;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            ReviewSysNo = AppConst.IntNull;
            C3ReviewItemSysNo = AppConst.IntNull;
            Score = AppConst.IntNull;
            Weight = AppConst.IntNull;
        }
    }
}