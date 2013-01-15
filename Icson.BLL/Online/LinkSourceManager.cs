using System;
using System.Data;
using System.Collections;
using System.Text;

using Icson.Utils;
using System.Transactions;

using Icson.DBAccess;
using Icson.DBAccess.Online;

using Icson.Objects.Online;

namespace Icson.BLL.Online
{
	/// <summary>
	/// Summary description for LinkSourceManager.
	/// </summary>
	public class LinkSourceManager
	{
		private LinkSourceManager()
		{
		}
		private static Hashtable ht = new Hashtable(1000);
		private static object locker = new object();
		private static LinkSourceManager _instance;
		public static LinkSourceManager GetInstance()
		{
			if( _instance == null )
			{
				_instance = new LinkSourceManager();
			}
			return _instance;
		}
		public void Count(string urlSource)
		{
			//使用这个hash缓存，将每次的urlsource更新数据库为50次一更新。
			//优点就是减少了数据库的负担
			//缺点	1 urlSource如果很多，就会比较占用内存, 如果达到1000条以上，要考虑更新算法，保持条目不要太多。
			//		2 会出现隔天计数的情况，也就是说50个点击才更新，所以会在第二天才因为日期原因，强制更新。如果需要解决，可以增加一个daemon。不过这个daemon 的具体策略还需要考虑。
			//		3 重启会使内存中的数据丢失，造成点击率偏差。 如果需要解决，可以考虑在application end 事件中处理。
			lock ( locker )
			{
				if ( !ht.Contains(urlSource))
				{
					//如果不包含这个url，就生成一个。
					LinkSourceInfo oLinkSource = new LinkSourceInfo();
					oLinkSource.CountDate = DateTime.Now.ToString(AppConst.DateFormat);
					oLinkSource.URLSource = urlSource;
					oLinkSource.VisitCount = 1;
					ht.Add(urlSource, oLinkSource);
				}
				else
				{
					LinkSourceInfo oLinkSource = ht[urlSource] as LinkSourceInfo;
					string nowTime = DateTime.Now.ToString(AppConst.DateFormat);

					//如果ht中的url不是今天的记录，就先更新昨天的记录数到数据库。并将今天这个url设置为1。
					if ( oLinkSource.CountDate != nowTime )
					{
						new LinkSourceDac().Count(oLinkSource);
						oLinkSource.CountDate = nowTime;
						oLinkSource.VisitCount = 1;
					}
					else 
					{
						//如果是今天的,计数加一
						oLinkSource.VisitCount++;
						//如果当前计数大于500，就更新到数据库。
						if ( oLinkSource.VisitCount >= 5)
						{
							new LinkSourceDac().Count(oLinkSource); //
							oLinkSource.VisitCount = 0;
						}
					}
				}
			}
		}

		public void map(LinkSourceInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.URLSource = Util.TrimNull(tempdr["URLSource"]);
			oParam.VisitCount = Util.TrimIntNull(tempdr["VisitCount"]);
			oParam.CountDate = Util.TrimNull(tempdr["CountDate"]);
		}

        /// <summary>
        /// 返回LinkSource的DataSet
        /// </summary>
        /// <param name="dateFrom">开始日期</param>
        /// <param name="dateTo">结束日期</param>
        /// <param name="url">网址</param>
        /// <returns></returns>
		public DataSet GetLinkSourceDs(string dateFrom, string dateTo, string url)
		{
			string sqlwhere = "where 1=1 ";
			if ( dateFrom != AppConst.StringNull )
				sqlwhere += " and countdate>=" + Util.ToSqlString(dateFrom);
			if ( dateTo != AppConst.StringNull)
				sqlwhere += " and countdate<=" + Util.ToSqlString(dateTo);

			if ( dateFrom == AppConst.StringNull && dateTo == AppConst.StringNull )
				sqlwhere += " and countdate = " + Util.ToSqlString(DateTime.Now.ToString(AppConst.DateFormat));

			if ( url !=  AppConst.StringNull )
				sqlwhere += " and urlsource like " + Util.ToSqlLikeString(url);

			string sql = @"select * from
							(
							select urlsource, visitcount, countdate from linksource @where
							union all
							select '合计' as urlsource,  sum(visitcount), countdate  from linksource @where
							group by countdate
							) as a
							order by countdate desc, urlsource";

			sql = sql.Replace("@where", sqlwhere);
			
			return SqlHelper.ExecuteDataSet(sql);
		}
		public void Import()
		{
			if ( !AppConfig.IsImportable)
				throw new BizException("Is Importable is false");

			/*  do not  use the following code after Data Pour in */
			string sql = " select top 1 * from linksource ";
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds) )
				throw new BizException("the table linksource is not empty");

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				string sql1 = @"select 
									*
								from 
									ipp2003..visitsourcelog";
				DataSet ds1 = SqlHelper.ExecuteDataSet(sql1);
				foreach(DataRow dr1 in ds1.Tables[0].Rows)
				{
					LinkSourceInfo oInfo = new LinkSourceInfo();
					map(oInfo, dr1);
					oInfo.CountDate = Util.TrimDateNull(oInfo.CountDate).ToString(AppConst.DateFormat);
					new LinkSourceDac().Insert(oInfo);
				}
				
			scope.Complete();
            }
		}
	}
}
