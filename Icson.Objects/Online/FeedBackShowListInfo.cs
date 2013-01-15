using System;
using Icson.Utils;


namespace Icson.Objects.Online
{
    public class FeedBackShowListInfo
    {
        public FeedBackShowListInfo()
        {
            Init();
        }
        public int SysNo;
        public int FeedBackSysNo;
        public int Type;
        public DateTime CreateTime;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            FeedBackSysNo = AppConst.IntNull;
            Type = AppConst.IntNull;
            CreateTime = AppConst.DateTimeNull;
        }
    }
}
