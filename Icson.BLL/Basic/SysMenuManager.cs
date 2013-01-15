using System;
using System.Data;
using System.Text;
using System.Collections;

using Icson.Utils;
using Icson.Objects.Basic;
using Icson.DBAccess;
using Icson.DBAccess.Basic;

namespace Icson.BLL.Basic
{
    public class SysMenuManager
    {
        private static SysMenuManager _instance;

        public static SysMenuManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SysMenuManager();
            }
            return _instance;
        }

        private void map(SysMenuInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.MenuID = Util.TrimIntNull(tempdr["MenuID"]);
            oParam.ParentID = Util.TrimIntNull(tempdr["ParentID"]);
            oParam.OrderNum = Util.TrimIntNull(tempdr["OrderNum"]);
            oParam.SubOrder = Util.TrimIntNull(tempdr["SubOrder"]);
            oParam.Name = Util.TrimNull(tempdr["Name"]);
            oParam.Description = Util.TrimNull(tempdr["Description"]);
            oParam.href = Util.TrimNull(tempdr["href"]);
            oParam.help = Util.TrimNull(tempdr["help"]);
            oParam.Icon = Util.TrimNull(tempdr["Icon"]);
            oParam.Privilege = Util.TrimNull(tempdr["Privilege"]);
        }

        public DataSet LoadDs(Hashtable paramHash)
        {
            string sql = @"select *
                            FROM Sys_Menu
						  where 1=1	";
            if (paramHash != null && paramHash.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string key in paramHash.Keys)
                {
                    sb.Append(" and ");
                    object item = paramHash[key];
                    if (key == "MenuID")
                    {
                        sb.Append("MenuID = ").Append(item.ToString());
                    }
                    else if (key == "Status")
                    {
                        sb.Append("Status = ").Append(item.ToString());
                    }
                    else if (key == "SysNo")
                    {
                        sb.Append("SysNo = ").Append(item.ToString());
                    }
                    else if (key == "ParentID")
                    {
                        sb.Append("ParentID = ").Append(item.ToString());
                    }
                    else if (item is int)
                    {
                        sb.Append(key).Append("=").Append(item.ToString());
                    }
                    else if (item is string)
                    {
                        sb.Append(key).Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                    }
                }
                sql += sb.ToString();
            }
            sql += " order by ordernum,suborder ";
            return SqlHelper.ExecuteDataSet(sql);
        }

        public void Insert(SysMenuInfo oParam)
        {
            new SysMenuDac().Insert(oParam);

        }

        public void Update(SysMenuInfo oParam)
        {
            new SysMenuDac().Update(oParam);
        }
    }
}
