using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Online
{
    public class TopicItemInfo
    {
        public int SysNo;
        public int TopicSysNo;
        public int C3ReviewItemSysNo;
        public int Score;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            TopicSysNo = AppConst.IntNull;
            C3ReviewItemSysNo = AppConst.IntNull;
            Score = AppConst.IntNull;
        }
    }
}