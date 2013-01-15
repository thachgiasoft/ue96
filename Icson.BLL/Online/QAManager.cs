using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

using System.Transactions;
using Icson.Objects;
using Icson.Objects.Online;
using Icson.Utils;
using Icson.DBAccess;
using Icson.DBAccess.Online;


namespace Icson.BLL.Online
{
    public class QAManager
    {
        private QAManager()
		{
		}
        private static QAManager _instance;
        public static QAManager GetInstance()
		{
			if( _instance == null )
			{
                _instance = new QAManager();
			}
			return _instance;
		}

        private void Map(QAInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.Question = Util.TrimNull(tempdr["Question"]);
            oParam.Answer = Util.TrimNull(tempdr["Answer"]);
            oParam.SearchKey = Util.TrimNull(tempdr["SearchKey"]);
            oParam.Type = Util.TrimIntNull(tempdr["Type"]);
            oParam.CreateUserSysNo = Util.TrimIntNull(tempdr["CreateUserSysNo"]);
            oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
            oParam.ViewCount = Util.TrimIntNull(tempdr["ViewCount"]);
            oParam.OrderNum = Util.TrimIntNull(tempdr["OrderNum"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
        }

        public int Insert(QAInfo oParam)
        {
            return new QADac().Insert(oParam);
        }

        public int Update(QAInfo oParam)
        {
            return new QADac().Update(oParam);
        }

        public int Update(Hashtable htParam)
        {
            return new QADac().Update(htParam);
        }

        public QAInfo LoadQA(int SysNo)
        {
            string sql = "select * from QA where sysno=" + SysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                QAInfo oInfo = new QAInfo();
                Map(oInfo, ds.Tables[0].Rows[0]);
                return oInfo;
            }
            else
                return null;
        }

        public int GetQANewOrderNum(QAInfo oParam)
        {
            return new QADac().GetQANewOrderNum(oParam);
        }

        public SortedList GetQAListByType(int Type)
        {
            string sql = "select * from QA where type=" + Type + " order by ordernum";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                SortedList sl = new SortedList();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    QAInfo oInfo = new QAInfo();
                    Map(oInfo,dr);
                    sl.Add(oInfo,null);
                }
                return sl;
            }
            else
                return null;
        }

        public DataSet GetQAListDs(Hashtable paramHash)
        {
            string sql = "select qa.*,sys_user.username as createusername from qa inner join sys_user on qa.createusersysno=sys_user.sysno where 1=1 ";
            string sqlOrderBy = " order by qa.type,qa.ordernum ";
            if (paramHash != null && paramHash.Count != 0)
            {
                StringBuilder sb = new StringBuilder(100);
                foreach (string key in paramHash.Keys)
                {
                    object item = paramHash[key];
                    if (key == "OrderBy")
                    {
                        sqlOrderBy = " order by " + item.ToString();
                    }
                    else
                    {
                        sb.Append(" and ");
                        if (key == "SearchKey")
                        {
                            sb.Append("qa.searchkey like").Append(Util.ToSqlLikeString(item.ToString()));
                        }
                        else if (key == "Type")
                        {
                            sb.Append("qa.type=").Append(item.ToString());
                        }
                        else if (key == "Status")
                        {
                            sb.Append("qa.status=").Append(item.ToString());
                        }
                        else if (key == "SysNo")
                        {
                            sb.Append("qa.SysNo=").Append(item.ToString());
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
                }
                sql += sb.ToString();
            }
            sql += sqlOrderBy;

            return SqlHelper.ExecuteDataSet(sql);
        }

        public void MoveTop(QAInfo oParam)
        {
            if (oParam.OrderNum == 1)
            {
                throw new BizException("it's the top one already");
            }
            SortedList sl = GetQAListByType(oParam.Type);

            if (sl == null)
            {
                throw new BizException("no QA for this type");
            }

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                QADac o = new QADac();

                foreach (QAInfo item in sl.Keys)
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

        public void MoveUp(QAInfo oParam)
        {
            if (oParam.OrderNum == 1)
            {
                throw new BizException("it's the first one, can't be moved up");
            }
            SortedList sl = GetQAListByType(oParam.Type);
            if (sl == null)
            {
                throw new BizException("no QA");
            }

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                QADac o = new QADac();

                foreach (QAInfo item in sl.Keys)
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

        public void MoveDown(QAInfo oParam)
        {
            SortedList sl = GetQAListByType(oParam.Type);
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
                QADac o = new QADac();

                foreach (QAInfo item in sl.Keys)
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

        public void MoveBottom(QAInfo oParam)
        {
            SortedList sl = GetQAListByType(oParam.Type);
            if (sl == null)
            {
                throw new BizException("no qa");
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

                QADac o = new QADac();

                foreach (QAInfo item in sl.Keys)
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
    }
}