using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Icson.BLL;
using Icson.BLL.Online;
using Icson.Objects;
using Icson.Objects.Online;
using Icson.Utils;
using System.Data;

namespace YoeJoyHelper
{
    /// <summary>
    /// 通知栏的Utility类
    /// </summary>
    public class WebBulletinUtility
    {
        private static readonly string GetWebBulletinListSqlCmdTemplate = @"SELECT TOP {0} [SysNo]
      ,[Title]
  FROM [mmbuy].[dbo].[WebBulletin] WHERE [Status]=0 ORDER BY [OrderNum] ASC";

        /// <summary>
        /// 获得通知栏文章列表
        /// </summary>
        /// <param name="topNum"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetWebBulletinList(string topNum)
        {
            string sqlCmd = String.Format(GetWebBulletinListSqlCmdTemplate, topNum);
            DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
            int count = data.Rows.Count;
            if (count > 0)
            {
                Dictionary<string, string> list = new Dictionary<string, string>();
                for (int i = 0; i < count; i++)
                {
                    string sysNo = data.Rows[i]["SysNo"].ToString().Trim();
                    string title = data.Rows[i]["Title"].ToString().Trim();
                    list.Add(sysNo, title);
                }
                return list;
            }
            else
            {
                return null;
            }
        }
    }
}
