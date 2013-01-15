using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;

using Icson.Utils;
using Icson.Objects.Basic;
using Icson.BLL;
using Icson.BLL.Basic;
using Icson.Objects;

namespace YoeJoyHelper
{
    public class GroupBuyUtility
    {
        /// <summary>
        /// 获取团购地区的城市
        /// </summary>
        /// <returns></returns>
        public static string GetGroupBuyLocationStr(int provinceSysNo, int citySysNo)
        {
            string locationStr = String.Empty;

            Hashtable ht = ASPManager.GetInstance().GetAreaHash();

            foreach (AreaInfo item in ht.Values)
            {
                if (item.ProvinceSysNo == provinceSysNo && item.CitySysNo == citySysNo)
                {
                    locationStr = item.CityName.Trim();
                }
            }

            return locationStr;
        }

        /// <summary>
        /// 获取合法的时间
        /// </summary>
        /// <param name="dateTimeStr"></param>
        /// <returns></returns>
        public static DateTime GetValidDate(string dateTimeStr)
        {
            string[] dateValue = dateTimeStr.Split('-');
            int year = int.Parse(dateValue[0]);
            int month = int.Parse(dateValue[1]);
            int day = int.Parse(dateValue[2]);
            return new DateTime(year, month, day);
        }

        /// <summary>
        /// 拼接多条件查询语句时的AND运算
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="conditionStr"></param>
        /// <returns></returns>
        public static string CombineMutlQueryCondtion(string condition, string conditionStr)
        {
            string andStr = String.Empty;
            if (condition != "WHERE")
            {
                andStr = " AND";
            }
            return String.Concat(andStr, conditionStr);
        }

    }
}