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
			//ʹ�����hash���棬��ÿ�ε�urlsource�������ݿ�Ϊ50��һ���¡�
			//�ŵ���Ǽ��������ݿ�ĸ���
			//ȱ��	1 urlSource����ܶ࣬�ͻ�Ƚ�ռ���ڴ�, ����ﵽ1000�����ϣ�Ҫ���Ǹ����㷨��������Ŀ��Ҫ̫�ࡣ
			//		2 ����ָ�������������Ҳ����˵50������Ÿ��£����Ի��ڵڶ������Ϊ����ԭ��ǿ�Ƹ��¡������Ҫ�������������һ��daemon���������daemon �ľ�����Ի���Ҫ���ǡ�
			//		3 ������ʹ�ڴ��е����ݶ�ʧ����ɵ����ƫ� �����Ҫ��������Կ�����application end �¼��д���
			lock ( locker )
			{
				if ( !ht.Contains(urlSource))
				{
					//������������url��������һ����
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

					//���ht�е�url���ǽ���ļ�¼�����ȸ�������ļ�¼�������ݿ⡣�����������url����Ϊ1��
					if ( oLinkSource.CountDate != nowTime )
					{
						new LinkSourceDac().Count(oLinkSource);
						oLinkSource.CountDate = nowTime;
						oLinkSource.VisitCount = 1;
					}
					else 
					{
						//����ǽ����,������һ
						oLinkSource.VisitCount++;
						//�����ǰ��������500���͸��µ����ݿ⡣
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
        /// ����LinkSource��DataSet
        /// </summary>
        /// <param name="dateFrom">��ʼ����</param>
        /// <param name="dateTo">��������</param>
        /// <param name="url">��ַ</param>
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
							select '�ϼ�' as urlsource,  sum(visitcount), countdate  from linksource @where
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
