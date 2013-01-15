using System;
using System.Collections;
using System.Reflection;
using System.Threading;
using System.Data;


using Icson.Utils;
using System.Transactions;
using Icson.Objects;
using Icson.Objects.Basic;

using Icson.DBAccess;
using Icson.DBAccess.Basic;

using Icson.Objects.ImportData;
using Icson.DBAccess.ImportData;

using Icson.BLL;

namespace Icson.BLL.Basic
{
	/// <summary>
	/// Summary description for SyncManager.
	/// </summary>
	public class SyncManager
	{
		private SyncManager()
		{
		}
		private static SyncManager _instance;
		private static object instanceLock = new object();
		public static SyncManager GetInstance()
		{
			lock(instanceLock)
			{
				if ( _instance == null )
				{
					_instance = new SyncManager();
				}
			}
			return _instance;
		}

		private static Hashtable syncHash = new Hashtable(5); //key syncType, value DateTime(lastVersionTime)

		public void RegisterLastVersion(int syncType)
		{
			if ( syncHash.Contains(syncType))
				syncHash.Remove(syncType);
			
			syncHash.Add(syncType, DateTime.Now);
		}

		private void DoSync(int syncType)
		{
			string className = AppEnum.GetSync(syncType);
			Type type = Type.GetType(className);
			if ( type == null ) //如果添加新的需要同步的类，就先在数据库里增加，在没有更新程序以前，className 应该是Unknown, Type.GetType的返回就会是null。
				return;
			MethodInfo method = type.GetMethod("GetInstance");
			IInitializable iInit = (IInitializable)method.Invoke(null,null);
			iInit.Init();
			RegisterLastVersion(syncType);
		}

		public void SetDbLastVersion(int syncType)
		{
			SyncInfo oInfo = new SyncInfo();
			oInfo.SyncType = syncType;
			oInfo.LastVersionTime = DateTime.Now;
			new SyncDac().Update(oInfo);
		}
		private Hashtable GetDbLastVersion()
		{
			string sql = "select * from sys_sync";
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( !Util.HasMoreRow(ds))
				return null;
			Hashtable ht = new Hashtable(5);
			foreach(DataRow dr in ds.Tables[0].Rows)
			{
				SyncInfo oInfo = new SyncInfo();
				map(oInfo, dr);
				ht.Add(oInfo.SyncType, oInfo);
			}
			return ht;
		}
		private void map(SyncInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.SyncType = Util.TrimIntNull(tempdr["SyncType"]);
			oParam.LastVersionTime = Util.TrimDateNull(tempdr["LastVersionTime"]);
		}

		public void Run()
		{
			try
			{
				Hashtable ht = GetDbLastVersion();
				if (ht == null )
					return;
				foreach(SyncInfo item in ht.Values)
				{
					DateTime localLastTime = DateTime.MinValue;

					try
					{
						localLastTime = (DateTime)syncHash[item.SyncType];
					}
					catch
					{}

					if ( localLastTime == DateTime.MinValue || localLastTime < item.LastVersionTime)
						DoSync(item.SyncType);
				}
			}
			catch(Exception exp)
			{
				ErrorLog.GetInstance().Write(exp.ToString());
			}
		}

		public void Import()
		{
			if ( !AppConfig.IsImportable)
				throw new BizException("Is Importable is false");

			string sql = " select top 1 * from Sys_Sync ";
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds) )
				throw new BizException("the table Sync is not empty");

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				SortedList sl = AppEnum.GetSync();
				foreach(int syncType in sl.Keys)
				{
					SyncInfo oInfo = new SyncInfo();
					oInfo.SyncType = syncType;
					oInfo.LastVersionTime = DateTime.Now;

					new SyncDac().Insert(oInfo);
				}
                				
			scope.Complete();
            }
		}
	}
}