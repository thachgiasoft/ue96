using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Sale
{
    public class SearchKeyTrackInfo
    {
        public SearchKeyTrackInfo()
        {
            Init();
        }

        public int SysNo;
        public string CustomerID;
        public string Keyword;
        public DateTime SearchTime;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            CustomerID = AppConst.StringNull;
            Keyword = AppConst.StringNull;
            SearchTime = AppConst.DateTimeNull;
        }
    }
}