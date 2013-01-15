using System;
using System.Data;

using Icson.Utils;
using System.Transactions;

using Icson.DBAccess;
using Icson.DBAccess.Online;

using Icson.Objects.Online;


namespace Icson.BLL.Online
{
	/// <summary>
	/// Summary description for BulletinManager.
	/// </summary>
	public class BulletinManager
	{
		private BulletinManager()
		{
		}
		private static BulletinManager _instance;
		public static BulletinManager GetInstance()
		{
			if( _instance == null )
			{
				_instance = new BulletinManager();
			}
			return _instance;
		}

		public BulletinInfo Load()
		{
			string sql = "select top 1 * from bulletin";
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( !Util.HasMoreRow(ds))
				return null;
			BulletinInfo oInfo = new BulletinInfo();
			map(oInfo, ds.Tables[0].Rows[0]);
			return oInfo;
		}

		private void map(BulletinInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.Message = Util.TrimNull(tempdr["Message"]);
			oParam.Status = Util.TrimIntNull(tempdr["Status"]);
		}
		private void Insert(BulletinInfo oParam)
		{
			new BulletinDac().Insert(oParam);

		}
		public void Update(BulletinInfo oParam)
		{
			new BulletinDac().Update(oParam);
		}
		public void Import()
		{
			if ( !AppConfig.IsImportable)
				throw new BizException("Is Importable is false");

			/*  do not  use the following code after Data Pour in */
			string sql = " select top 1 * from bulletin ";
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds) )
				throw new BizException("the table bulletin is not empty");

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				string sql1 = @"select 1 as sysno, message, status from ipp2003..messagebar";
				DataSet ds1 = SqlHelper.ExecuteDataSet(sql1);
				foreach(DataRow dr1 in ds1.Tables[0].Rows )
				{
					BulletinInfo item = new BulletinInfo();
					map(item, dr1);
					new BulletinDac().Insert(item);
					
				}
				
			scope.Complete();
            }
		}
	}
}
