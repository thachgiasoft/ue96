using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Transactions;
using Icson.DBAccess;
using Icson.DBAccess.Online;
using Icson.Objects.Online;
using Icson.Utils;

namespace Icson.BLL.Online
{
    public class WebBulletinManager
    {
        private WebBulletinManager()
        {
        }
        private static WebBulletinManager _instance;
        public static WebBulletinManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new WebBulletinManager();
            }
            return _instance;
        }

        private void map(WebBulletinInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.Title = Util.TrimNull(tempdr["Title"]);
            oParam.Content = Util.TrimNull(tempdr["Content"]);
            oParam.CreateDate = Util.TrimDateNull(tempdr["CreateDate"]);
            oParam.OrderNum = Util.TrimIntNull(tempdr["OrderNum"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
        }

        public WebBulletinInfo LoadWebBulletin(int SysNo)
        {
            string sql = @"select * from WebBulletin where sysno=" + SysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;
            WebBulletinInfo o = new WebBulletinInfo();
            map(o, ds.Tables[0].Rows[0]);
            return o;
        }

        public int GetWebBulletinNewOrderNum()
        {
            string sql = "select (IsNull(max(ordernum),0)+1) from WebBulletin";
            return Int32.Parse(SqlHelper.ExecuteScalar(sql).ToString());
        }

        public void Insert(WebBulletinInfo oParam)
        {
            new WebBulletinDac().Insert(oParam);
        }
        public void Update(WebBulletinInfo oParam)
        {
            new WebBulletinDac().Update(oParam);
        }

        public DataSet GetWebBulletinDs(Hashtable paramHash)
        {
            string sql = "select @top * from WebBulletin Where 1=1 ";
            if (paramHash != null && paramHash.Count != 0)
            {
                StringBuilder sb = new StringBuilder(100);
                foreach (string key in paramHash.Keys)
                {
                    object item = paramHash[key];
                    sb.Append(" and ");
                    if (key == "Title")
                    {
                        sb.Append("Title like ").Append(Util.ToSqlLikeString(item.ToString()));
                    }
                    else if (key == "Status")
                    {
                        sb.Append("Status=").Append(item.ToString());
                    }
                    else if (key == "Top")
                    {
                        sb.Append(" 1=1 ");
                        sql = sql.Replace("@top", " top " + item.ToString());
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
            sql += " order by ordernum";
            sql = sql.Replace("@top", " ");

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;
            return ds;
        }

        public DataSet GetWebBulletinDsForWeb(Hashtable paramHash)
        {
            string sql = "select @top sysno,title,ordernum,status from WebBulletin Where 1=1 ";
            if (paramHash != null && paramHash.Count != 0)
            {
                StringBuilder sb = new StringBuilder(100);
                foreach (string key in paramHash.Keys)
                {
                    object item = paramHash[key];
                    sb.Append(" and ");
                    if (key == "Title")
                    {
                        sb.Append("Title like ").Append(Util.ToSqlLikeString(item.ToString()));
                    }
                    else if (key == "Status")
                    {
                        sb.Append("Status=").Append(item.ToString());
                    }
                    else if (key == "Top")
                    {
                        sb.Append(" 1=1 ");
                        sql = sql.Replace("@top", " top " + item.ToString());
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
            sql += " order by ordernum";
            sql = sql.Replace("@top", " ");

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;
            return ds;
        }

        public Hashtable GetWebBulletinSysNoHash()
        {
            string sql = @"select sysno
								from 
									webbulletin
								where status = 0 
								order by ordernum,createdate desc ";
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

        public SortedList GetWebBulletinList()
        {
            string sql = @"select * from WebBulletin order by ordernum";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;

            SortedList sl = new SortedList(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                WebBulletinInfo item = new WebBulletinInfo();
                map(item, dr);
                sl.Add(item, null);
            }
            return sl;
        }

        public SortedList GetWebBulletinList(int Top)
        {
            string sql = @"select top " + Top + " * from WebBulletin order by ordernum";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;

            SortedList sl = new SortedList(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                WebBulletinInfo item = new WebBulletinInfo();
                map(item, dr);
                sl.Add(item, null);
            }
            return sl;
        }

        public void MoveTop(WebBulletinInfo oParam)
        {
            SortedList sl = GetWebBulletinList();

            if (sl == null)
            {
                throw new BizException("no item for this solution");
            }

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                WebBulletinDac o = new WebBulletinDac();

                foreach (WebBulletinInfo item in sl.Keys)
                {
                    if (item.OrderNum < oParam.OrderNum)
                    {
                        item.OrderNum = item.OrderNum + 1;
                        o.SetOrderNum(item);
                    }
                }
                oParam.OrderNum = 1;
                o.SetOrderNum(oParam);

                scope.Complete();
            }
        }

        public void MoveUp(WebBulletinInfo oParam)
        {
            SortedList sl = GetWebBulletinList();
            if (sl == null)
            {
                throw new BizException("no items");
            }

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                WebBulletinDac o = new WebBulletinDac();

                foreach (WebBulletinInfo item in sl.Keys)
                {
                    if (item.OrderNum == oParam.OrderNum - 1)
                    {
                        item.OrderNum += 1;
                        o.SetOrderNum(item);
                    }
                }
                oParam.OrderNum -= 1;
                o.SetOrderNum(oParam);

                scope.Complete();
            }
        }

        public void MoveDown(WebBulletinInfo oParam)
        {
            SortedList sl = GetWebBulletinList();
            if (sl == null)
            {
                throw new BizException("no items");
            }

            if (oParam.OrderNum == sl.Count)
            {
                throw new BizException("it's the last one, can't be moved down");
            }

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                WebBulletinDac o = new WebBulletinDac();

                foreach (WebBulletinInfo item in sl.Keys)
                {
                    if (item.OrderNum == oParam.OrderNum + 1)
                    {
                        item.OrderNum -= 1;
                        o.SetOrderNum(item);
                    }
                }
                oParam.OrderNum += 1;
                o.SetOrderNum(oParam);

                scope.Complete();
            }
        }

        public void MoveBottom(WebBulletinInfo oParam)
        {
            SortedList sl = GetWebBulletinList();
            if (sl == null)
            {
                throw new BizException("no items");
            }

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                WebBulletinDac o = new WebBulletinDac();

                foreach (WebBulletinInfo item in sl.Keys)
                {
                    if (item.OrderNum > oParam.OrderNum)
                    {
                        item.OrderNum = item.OrderNum - 1;
                        o.SetOrderNum(item);
                    }
                }
                oParam.OrderNum = sl.Count;
                o.SetOrderNum(oParam);

                scope.Complete();
            }
        }

        public string GetWebBulletinPageDiv(int currentPage)
        {
            string sql = @"select sysno,title,createdate
								from 
									webbulletin
								where status = 0 
								order by ordernum,createdate desc";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return "";

            int pageSize = 10;
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
                    sb.Append("<div class=xwgg_li><a href='../Service/NewsDetail.aspx?Type=Bulletin&ID=" + Util.TrimNull(dr["sysno"]) + "' class=acolor>" + Util.TrimNull(dr["title"]) + "</a><span class=time>" + Util.TrimNull(dr["createdate"]) + "</span></div>");
                }
                rowindex++;
            }

            return sb.ToString();
        }
        public DataSet GetWebBulletinPageDs(int currentPage)
        {
            string sql = @"select sysno,title,createdate
								from 
									webbulletin
								where status = 0 
								order by ordernum,createdate desc";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            //if (!Util.HasMoreRow(ds))
            //    return "";

            //int pageSize = 10;
            //int totalRowCount = ds.Tables[0].Rows.Count;
            //int totalPage = totalRowCount / pageSize;
            //if (ds.Tables[0].Rows.Count % pageSize != 0)
            //    totalPage += 1;

            //if (currentPage > totalPage)
            //    currentPage = 1;
            //int rowindex = 0;
            //StringBuilder sb = new StringBuilder();

            //foreach (DataRow dr in ds.Tables[0].Rows)
            //{
            //    if (rowindex >= (currentPage - 1) * pageSize && rowindex < currentPage * pageSize)
            //    {
            //        sb.Append("<div class=xwgg_li><a href='../Service/NewsDetail.aspx?Type=Bulletin&ID=" + Util.TrimNull(dr["sysno"]) + "' class=acolor>" + Util.TrimNull(dr["title"]) + "</a><span class=time>" + Util.TrimNull(dr["createdate"]) + "</span></div>");
            //    }
            //    rowindex++;
            //}

            return ds;
        }
    }
}