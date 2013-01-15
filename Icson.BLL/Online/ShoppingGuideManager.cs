using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections;

using Icson.Utils;
using Icson.DBAccess;
using Icson.DBAccess.Online;
using Icson.Objects;
using Icson.Objects.Online;
namespace Icson.BLL.Online
{
    public class ShoppingGuideManager
    {

        private ShoppingGuideManager()
        {
        }
        private static ShoppingGuideManager _instance;
        public static ShoppingGuideManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ShoppingGuideManager();
            }
            return _instance;
        }

        private void map(ShoppingGuideInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.Title = Util.TrimNull(tempdr["Title"]);
            oParam.Url = Util.TrimNull(tempdr["Url"]);
            oParam.Content = Util.TrimNull(tempdr["Content"]);
            oParam.CreateUserSysNo = Util.TrimIntNull(tempdr["CreateUserSysNo"]);
            oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
        }

        public ShoppingGuideInfo Load(int sysno)
        {
            string sql = "select * from ShoppingGuide where sysno=" + sysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;
            ShoppingGuideInfo oInfo = new ShoppingGuideInfo();
            map(oInfo, ds.Tables[0].Rows[0]);
            return oInfo;
        }

        public void Insert(ShoppingGuideInfo oParam)
        {
            new ShoppingGuideDac().Insert(oParam);
        }
        public void Update(ShoppingGuideInfo oParam)
        {
            new ShoppingGuideDac().Update(oParam);
        }

        public DataSet GetShoppingGuideList(Hashtable paramHash)
        {
            string sql = @"select @Top ShoppingGuide.*,su.username as createusername from ShoppingGuide
                        left join sys_user su on ShoppingGuide.createusersysno=su.sysno
                        where 1=1 @DateFrom @DateTo @Title @Status order by ShoppingGuide.sysno desc";

            if (paramHash.ContainsKey("DateFrom"))
            {
                sql = sql.Replace("@DateFrom", "and ShoppingGuide.CreateTime>=" + Util.ToSqlString(paramHash["DateFrom"].ToString()));
            }
            else
            {
                sql = sql.Replace("@DateFrom", "");
            }

            if (paramHash.ContainsKey("DateTo"))
            {
                sql = sql.Replace("@DateTo", "and ShoppingGuide.CreateTime<=" + Util.ToSqlEndDate(paramHash["DateTo"].ToString()));
            }
            else
            {
                sql = sql.Replace("@DateTo", "");
            }
            if (paramHash.ContainsKey("Status"))
            {
                sql = sql.Replace("@Status", "and ShoppingGuide.Status=" + Util.ToSqlString(paramHash["Status"].ToString()));
            }
            else
            {
                sql = sql.Replace("@Status", "");
            }
            if (paramHash.ContainsKey("Title"))
            {
                string t = "";
                string[] Keys = Util.TrimNull((paramHash["Title"].ToString())).Split(' ');
                if (Keys.Length == 1)
                {
                    sql = sql.Replace("@Title", "and ShoppingGuide.Title like" + Util.ToSqlLikeString(paramHash["Title"].ToString()));
                }
                else
                {
                    for (int i = 0; i < Keys.Length; i++)
                    {
                        t += "and ShoppingGuide.Title like" + Util.ToSqlLikeString(Keys[i]);
                    }
                    sql = sql.Replace("@Title", t);
                }

            }
            else
            {
                sql = sql.Replace("@Title", "");
            }
            if (paramHash.ContainsKey("Top"))
            {
                sql = sql.Replace("@Top", "top " + Util.TrimIntNull(paramHash["Top"].ToString()));
            }
            else
            {
                sql = sql.Replace("@Top", "");
            }
            if (paramHash == null || paramHash.Count == 0)
                sql = sql.Replace("select", "select top 50");
            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetShoppingGuideListForWeb(Hashtable paramHash)
        {
            string sql = @"select @Top sysno,title,url from ShoppingGuide                        
                        where 1=1  @Status order by sysno desc";

            if (paramHash.ContainsKey("Status"))
            {
                sql = sql.Replace("@Status", "and Status=" + Util.ToSqlString(paramHash["Status"].ToString()));
            }
            else
            {
                sql = sql.Replace("@Status", "");
            }

            if (paramHash.ContainsKey("Top"))
            {
                sql = sql.Replace("@Top", "top " + Util.TrimIntNull(paramHash["Top"].ToString()));
            }
            else
            {
                sql = sql.Replace("@Top", "");
            }
            if (paramHash == null || paramHash.Count == 0)
                sql = sql.Replace("select", "select top 50");
            return SqlHelper.ExecuteDataSet(sql);
        }

        public string GetShoppingGuideShowListPageDiv(int currentPage)
        {
            string sql = @"select  ShoppingGuide.*,su.username as createusername from ShoppingGuide
                        left join sys_user su on ShoppingGuide.createusersysno=su.sysno
                        where ShoppingGuide.status=" + (int)AppEnum.BiStatus.Valid + " order by ShoppingGuide.sysno desc";


            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return "";

            int pageSize = 15;
            int totalRowCount = ds.Tables[0].Rows.Count;
            int totalPage = totalRowCount / pageSize;
            if (ds.Tables[0].Rows.Count % pageSize != 0)
                totalPage += 1;

            if (currentPage > totalPage)
                currentPage = 1;
            int rowindex = 0;
            StringBuilder sb = new StringBuilder();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (rowindex >= (currentPage - 1) * pageSize && rowindex < currentPage * pageSize)
                {
                    if (Util.TrimNull(dr["url"]) == AppConst.StringNull)
                    {
                        sb.Append("<div class=xwgg_li><a target='_blank' href='../Service/ShoppingGuideDetail.aspx?ID=" + Util.TrimNull(dr["sysno"]) + "' class=acolor>" + Util.TrimNull(dr["title"]) + "</a><span class=time>" + Util.TrimNull(dr["createtime"]) + "</span></div>");
                    }
                    else
                    {
                        sb.Append("<div class=xwgg_li><a target='_blank' href='" + Util.TrimNull(dr["url"]) + "' class=acolor>" + Util.TrimNull(dr["title"]) + "</a><span class=time>" + Util.TrimNull(dr["createtime"]) + "</span></div>");
                    }
                }
                rowindex++;
            }

            return sb.ToString();
        }

        public Hashtable GetShoppingGuideShowListSysNoHash()
        {
            string sql = @"select ShoppingGuide.sysno from ShoppingGuide
                        left join sys_user su on ShoppingGuide.createusersysno=su.sysno
                        where ShoppingGuide.status=" + (int)AppEnum.BiStatus.Valid + " order by ShoppingGuide.sysno desc";

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;

            Hashtable ht = new Hashtable(30);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ht.Add(Util.TrimIntNull(dr["sysno"]), null);
            }
            return ht;
        }


    }
}
