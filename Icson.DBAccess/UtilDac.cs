using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Icson.Utils;

namespace Icson.DBAccess
{
    public class UtilDac
    {
        private UtilDac()
        {
        }
        private static UtilDac _instance;
        public static UtilDac GetInstance()
        {
            if (_instance == null)
            {
                _instance = new UtilDac();
            }
            return _instance;
        }

        public int Update(Hashtable paramHash, string paramTable)
        {
            if (paramHash != null && paramHash.Count > 0 && paramTable.Trim() != string.Empty)
            {
                StringBuilder sb = new StringBuilder(200);
                sb.Append("UPDATE ").Append(paramTable).Append(" SET ");
                int index = 0;
                foreach (string key in paramHash.Keys)
                {
                    object item = paramHash[key];
                    if (key.ToLower() == "sysno")
                        continue;
                    if (index != 0)
                        sb.Append(",");
                    index++;
                    if (item is int || item is decimal)
                    {
                        if (item.ToString() == AppConst.IntNull.ToString() || item.ToString() == AppConst.DecimalNull.ToString())
                            sb.Append(key).Append(" = null");
                        else
                            sb.Append(key).Append(" = ").Append(item.ToString());
                    }

                    else if (item is string)
                    {
                        sb.Append(key).Append(" = ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (item is DateTime)
                    {
                        sb.Append(key).Append(" = cast(").Append(Util.ToSqlString(((DateTime)item).ToString(AppConst.DateFormatLong))).Append(" as DateTime )");
                    }
                    else if (item is DBNull)
                    {
                        sb.Append(key).Append(" = null ");
                    }
                }
                sb.Append(" Where SysNo =").Append(paramHash["SysNo"]);
                return SqlHelper.ExecuteNonQuery(sb.ToString());
            }
            else
            {
                return 0;
            }
        }
    }
}
