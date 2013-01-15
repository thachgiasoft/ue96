using System;
using System.Data;
using System.Collections;

using Icson.Utils;
using System.Transactions;
using Icson.Objects.Basic;
using Icson.Objects.ImportData;
using Icson.DBAccess;
using Icson.DBAccess.ImportData;
using Icson.DBAccess.Basic;
using Icson.Objects;


namespace Icson.BLL.Basic
{
	/// <summary>
	/// Summary description for CurrencyManager.
	/// </summary>
	public class CurrencyManager : IInitializable
	{
		private CurrencyManager()
		{
		}
		private static CurrencyManager _instance;
		public static CurrencyManager GetInstance()
		{
			if ( _instance == null)
			{
				_instance = new CurrencyManager();
				_instance.Init();
				SyncManager.GetInstance().RegisterLastVersion((int)AppEnum.Sync.Currency);
			}
			return _instance;
		}
		private static object currencyLocker = new object();
		private static Hashtable currencyHash = new Hashtable(5);
		public Hashtable GetCurrencyHash()
		{
			return currencyHash;
		}
		public SortedList GetCurrencyList()
		{ 
			SortedList sl = new SortedList(5);
			foreach(CurrencyInfo item in currencyHash.Values)
			{
				sl.Add(item,null);
			}
			return sl;
		}
		public CurrencyInfo Load(int sysno)
		{
			return currencyHash[sysno] as CurrencyInfo;
		}
		public CurrencyInfo LoadLocal()
		{
			foreach(CurrencyInfo item in currencyHash.Values)
			{
				if ( item.IsLocal == (int)AppEnum.YNStatus.Yes)
				{
					return item;
				}
			}
			return null;
		}

		private void map(CurrencyInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.CurrencyID = Util.TrimNull(tempdr["CurrencyID"]);
			oParam.CurrencyName = Util.TrimNull(tempdr["CurrencyName"]);
			oParam.CurrencySymbol = Util.TrimNull(tempdr["CurrencySymbol"]);
			oParam.IsLocal = Util.TrimIntNull(tempdr["IsLocal"]);
			oParam.ExchangeRate = Util.TrimDecimalNull(tempdr["ExchangeRate"]);
			oParam.ListOrder = Util.TrimNull(tempdr["ListOrder"]);
			oParam.Status = Util.TrimIntNull(tempdr["Status"]);
		}

		public void Init()
		{
			lock ( currencyLocker )
			{
				currencyHash.Clear();

				string sql = @" select * from currency ";
				DataSet ds = SqlHelper.ExecuteDataSet(sql);
				if ( !Util.HasMoreRow(ds))
					return;
				foreach( DataRow dr in ds.Tables[0].Rows )
				{
					CurrencyInfo item = new CurrencyInfo();
					map(item, dr);
					currencyHash.Add(item.SysNo, item);
				}
			}
		}

		public void Insert(CurrencyInfo oParam)
		{
			foreach(CurrencyInfo item in currencyHash.Values)
			{
				if ( item.CurrencyID == oParam.CurrencyID)
					throw new BizException("the same id exists");
			}

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				string sql = "select isnull(max(sysno),0) as maxsysno from currency";
				DataSet ds = SqlHelper.ExecuteDataSet(sql);

				int newSysNo = 0;
				if ( !Util.HasMoreRow(ds))
					newSysNo = 0;
				else
					newSysNo = Util.TrimIntNull(ds.Tables[0].Rows[0]["maxsysno"]);

				oParam.SysNo = newSysNo+1;

				new CurrencyDac().Insert(oParam);
				SyncManager.GetInstance().SetDbLastVersion( (int)AppEnum.Sync.Currency );

				currencyHash.Add(oParam.SysNo, oParam);

				scope.Complete();
            }
		}

		public void Update(CurrencyInfo oParam)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				new CurrencyDac().Update(oParam);
				SyncManager.GetInstance().SetDbLastVersion( (int)AppEnum.Sync.Currency );

				currencyHash.Remove(oParam.SysNo);
				currencyHash.Add(oParam.SysNo,oParam);

				scope.Complete();
            }
		}

		public CurrencyInfo GetLocalCurrency()
		{
			foreach(CurrencyInfo item in currencyHash.Values)
			{
				if ( item.IsLocal == (int)AppEnum.YNStatus.Yes )
					return item;
			}
			return null;
		}

		public void Import()
		{
			if ( !AppConfig.IsImportable)
				throw new BizException("Is Importable is false");

			/*  do not  use the following code after Data Pour in */
			string sql = " select top 1 * from currency ";
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds) )
				throw new BizException("the table currency is not empty");

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                try
                {

				    string sql1 = @"select a.*, isnull(b.exchangerate,0) as exchangerate from ipp2003..currency a, ipp2003..exchange_rate b
								    where a.sysno *=b.currencysysno";
				    DataSet ds1 = SqlHelper.ExecuteDataSet(sql1);
				    foreach(DataRow dr1 in ds1.Tables[0].Rows )
				    {
					    CurrencyInfo item = new CurrencyInfo();
    					
					    item.CurrencyID = Util.TrimNull(dr1["CurrencyID"]);
					    item.CurrencyName = Util.TrimNull(dr1["CurrencyName"]);
					    item.CurrencySymbol = Util.TrimNull(dr1["CurrencySymbol"]);
    					
					    if ( dr1["IfBasic"] == System.DBNull.Value )
						    item.IsLocal = (int)AppEnum.YNStatus.No;
					    else if ( Convert.ToBoolean(dr1["IfBasic"])==true)
						    item.IsLocal = (int)AppEnum.YNStatus.Yes;
					    else
						    item.IsLocal = (int)AppEnum.YNStatus.No;

					    item.ExchangeRate = Util.TrimDecimalNull(dr1["ExchangeRate"]);
					    item.ListOrder = Util.TrimNull(dr1["CurrencyID"]);
					    item.Status = Util.TrimIntNull(dr1["Status"]);

					    this.Insert(item);


					    //insert into convert table
					    ImportInfo oConvert = new ImportInfo();
					    oConvert.OldSysNo = Util.TrimIntNull(dr1["SysNo"]);
					    oConvert.NewSysNo = item.SysNo;
					    new ImportDac().Insert(oConvert, "Currency");
                    }
				}
				catch(Exception ex) 
	            {
		            currencyHash.Clear();
		            throw ex;
	            }
                scope.Complete();
			}
		}
		

	}
}
