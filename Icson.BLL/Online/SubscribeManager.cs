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
	/// Summary description for SubscribeManager.
	/// </summary>
	public class SubscribeManager
	{
		private SubscribeManager()
		{
		}
		private static SubscribeManager _instance;
		public static SubscribeManager GetInstance()
		{
			if( _instance == null )
			{
				_instance = new SubscribeManager();
			}
			return _instance;
		}
		public bool IsSubscribed(string email)
		{
			
			bool b = false;
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				string sql = "select top 1 sysno from subscribe where email=" + Util.ToSqlString(email);
				DataSet ds = SqlHelper.ExecuteDataSet(sql);
				if ( Util.HasMoreRow(ds))
					b = true;
				else
					b = false;

				scope.Complete();
            }
			return b;
		}

		public void Insert(SubscribeInfo oParam)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				if ( IsSubscribed(oParam.Email))
					throw new BizException("ÒÑ¾­¶©ÔÄ");

				new SubscribeDac().Insert(oParam);

				scope.Complete();
            }
			
		}
		public void Delete(string email)
		{
			new SubscribeDac().Delete(email);
		}

		public void Import()
		{
			if ( !AppConfig.IsImportable)
				throw new BizException("Is Importable is false");

			/*  do not  use the following code after Data Pour in */
			string sql = " select top 1 * from subscribe ";
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds) )
				throw new BizException("the table subscribe is not empty");

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				string sql1 = @"select * from 
								(
								select customername as username, email, registertime as createtime from ipp3..customer where emailstatus = 1
								union all
								select username, email, createtime from ipp2003..EmailSubscribe
								) as a
								where a.username is not null
								order by createtime";

				Hashtable ht = new Hashtable(20000);
				DataSet ds1 = SqlHelper.ExecuteDataSet(sql1);
				foreach(DataRow dr1 in ds1.Tables[0].Rows)
				{
					if ( ht.ContainsKey(Util.TrimNull(dr1["email"])) )
						continue;

					SubscribeInfo  oInfo = new SubscribeInfo();
					oInfo.Email = Util.TrimNull(dr1["email"]);
					oInfo.UserName = Util.TrimNull(dr1["username"]);
					oInfo.CreateTime = Util.TrimDateNull(dr1["createtime"]);

					new SubscribeDac().Insert(oInfo);

					ht.Add(oInfo.Email, null);
				}
				
			scope.Complete();
            }
		}
	}
}
