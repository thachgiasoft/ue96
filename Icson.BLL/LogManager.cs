/*
 * 记录事务日志	
 */
using System;
using System.Data;
using System.Collections;//add by Judy
using System.Text;//add by Judy

using Icson.Objects.Basic; //add by Judy
using Icson.DBAccess.Basic;
using Icson.DBAccess;//add by Judy
using Icson.Utils; //add by Judy

namespace Icson.BLL
{
    /// <summary>
    /// Summary description for Logger.
    /// </summary>
    public class LogManager
    {
        private LogManager()
        {
        }

        private static LogManager _instance;
        public static LogManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new LogManager();
            }
            return _instance;
        }

        public int Write(LogInfo oParam)
        {
            LogDac o = new LogDac();
            return o.Insert(oParam);
        }
        public DataSet Search(LogInfo oParam)
        {
            return null;
        }

        //add by Judy
        public DataSet GetSysLogDetail(Hashtable paramHash)
        {
            string sql = @"SELECT sl.TicketSysNo, sl.OptTime, sl.Note, sl.TicketType, sl.OptIP, su.UserName
                            FROM Sys_Log sl LEFT OUTER JOIN
                                  Sys_User su ON su.SysNo = sl.OptUserSysNo
            			    Where  1=1";
            if (paramHash.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string key in paramHash.Keys)
                {
                    object item = paramHash[key];
                    sb.Append(" and ");

                    if (key == "TicketSysNo")
                        sb.Append("sl.TicketSysNo").Append("=").Append(item.ToString());
                    else if (key == "DateFrom")
                    {
                        sb.Append("sl.OptTime").Append(">=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "DateTo")
                    {
                        sb.Append("sl.OptTime").Append("<=").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                }
                sb.Append("ORDER BY sl.OptTime DESC");
                sql += sb.ToString();
            }
            return SqlHelper.ExecuteDataSet(sql);

        }
    }
}
