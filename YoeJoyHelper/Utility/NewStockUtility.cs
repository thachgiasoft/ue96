using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Sql;

using Icson.Utils;
using Icson.Objects.Basic;
using Icson.BLL.Basic;

namespace YoeJoyHelper
{
    public class NewStockUtility
    {
        /// <summary>
        /// 根据输入的关键字(系统编号/编号/名称)
        /// 在相应的仓库中搜索入库的商品
        /// </summary>
        /// <param name="keyword">搜索关键字</param>
        /// <param name="stockSysNo">仓库SysNo</param>
        /// <returns></returns>
        public static DataSet GetStockProductByKeyword(string keyword, int stockSysNo)
        {
            DataSet ds = ProductManager.GetInstance().GetBasicBriefDsWithPrice(keyword, stockSysNo);
            return ds;
        }

    }
}