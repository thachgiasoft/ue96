using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Icson.Utils;
using Icson.Objects;
using Icson.Objects.Sale;
using Icson.DBAccess.Sale;

namespace Icson.BLL.Sale
{
    public class SearchKeyTrackManager
    {
        private SearchKeyTrackManager()
        {
        }
        private static SearchKeyTrackManager _instance;
        public static SearchKeyTrackManager GetInstance()
        {
            if (_instance == null)
                _instance = new SearchKeyTrackManager();
            return _instance;
        }

        public int Insert(SearchKeyTrackInfo oParam)
        {
            return new SearchKeyTrackDac().Insert(oParam);
        }

        public int Update(SearchKeyTrackInfo oParam)
        {
            return new SearchKeyTrackDac().Update(oParam);
        }

        private void map(SearchKeyTrackInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.CustomerID = Util.TrimNull(tempdr["CustomerID"]);
            oParam.Keyword = Util.TrimNull(tempdr["Keyword"]);
            oParam.SearchTime = Util.TrimDateNull(tempdr["SearchTime"]);
        }
    }
}