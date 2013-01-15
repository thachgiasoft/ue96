using System;
using System.Data;
using System.Collections;
using System.Text;

using Icson.Utils;
using System.Transactions;
using Icson.Objects.Basic;
using Icson.DBAccess;
using Icson.DBAccess.Basic;

using Icson.Objects;
using Icson.Objects.ImportData;
using Icson.DBAccess.ImportData;

namespace Icson.BLL.Basic
{
	/// <summary>
	/// Summary description for StockManager.
	/// </summary>
	public class StockManager : IInitializable
	{
		private StockManager()
		{
		}
		private static StockManager _instance;
		public static StockManager GetInstance()
		{
			if ( _instance == null )
			{
				_instance = new StockManager();
				_instance.Init();
				SyncManager.GetInstance().RegisterLastVersion((int)AppEnum.Sync.Stock);
			}
			return _instance;
		}

		private static object stockLock = new object();
		private static Hashtable stockHash = new Hashtable(5);
		public Hashtable GetStockHash(bool isValidOnly)
		{
			Hashtable ht=null;
			foreach(StockInfo item in stockHash.Values)
			{
				if ( !isValidOnly || item.Status == (int)AppEnum.BiStatus.Valid )
				{
					if ( ht == null )
					{
						ht = new Hashtable(5);
					}
					ht.Add( item.SysNo, item);
				}

			}
			return ht;
		}
		public SortedList GetStockList(bool isValidOnly)
		{
			SortedList sl = null;
			foreach(StockInfo item in stockHash.Values)
			{
				if ( !isValidOnly || item.Status == (int)AppEnum.BiStatus.Valid)
				{
					if ( sl == null )
					{
						sl = new SortedList(5);
					}
					sl.Add(item, null);
				}
			}
			return sl;
		}

		public void Init()
		{
			lock( stockLock )
			{
				if ( stockHash != null )
					stockHash.Clear();
				string sql = "select * from Stock";
				DataSet ds = SqlHelper.ExecuteDataSet(sql);
				if ( !Util.HasMoreRow(ds) )
					return;
				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					StockInfo item = new StockInfo();
					map(item, dr);
					if( stockHash == null)
					{
						stockHash = new Hashtable(5);
					}
					stockHash.Add(item.SysNo, item);
				}
			}
		}

		public void ImportStock()
		{
			if ( !AppConfig.IsImportable)
				throw new BizException("Is Importable is false");

			/*  do not  use the following code after Data Pour in */
			string sql = " select top 1 * from Stock ";
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds) )
				throw new BizException("the table Stock is not empty");

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				string sql1 = "select * from ipp2003..Stock";
				DataSet ds1 = SqlHelper.ExecuteDataSet(sql1);
				foreach(DataRow dr1 in ds1.Tables[0].Rows )
				{
					StockInfo oInfo = new StockInfo();

					oInfo.StockID = Util.TrimNull(dr1["StockID"]);
					oInfo.StockName = Util.TrimNull(dr1["StockName"]);
					oInfo.Address = Util.TrimNull(dr1["StockAddress"]);
					oInfo.Contact =Util.TrimNull(dr1["Contact"]);
					oInfo.Phone = Util.TrimNull(dr1["Tel"]);
					oInfo.Status = Util.TrimIntNull(dr1["Status"]);

					this.Insert(oInfo);

					//insert into convert table
					ImportInfo oVendorConvert = new ImportInfo();
					oVendorConvert.OldSysNo = Util.TrimIntNull(dr1["SysNo"]);
					oVendorConvert.NewSysNo = oInfo.SysNo;
					new ImportDac().Insert(oVendorConvert, "Stock");
					
				}
				scope.Complete();
            }
		}

		public int Insert(StockInfo oParam)
		{
			string sql = " select top 1 sysno from Stock where StockID = " + Util.ToSqlString(oParam.StockID);
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds) )
				throw new BizException("the same Stock ID exists already");

			oParam.SysNo = SequenceDac.GetInstance().Create("Stock_Sequence");
			int result = new StockDac().Insert(oParam);
			SyncManager.GetInstance().SetDbLastVersion( (int)AppEnum.Sync.Stock );

			stockHash.Add(oParam.SysNo, oParam);
			return result;
		}
		public int Update(StockInfo oParam)
		{
			string sql = " select top 1 sysno from Stock where StockID = " + Util.ToSqlString(oParam.StockID) + " and sysno <> " + oParam.SysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds) )
				throw new BizException("the same Stock ID exists already");

			int result = new StockDac().Update(oParam);
			SyncManager.GetInstance().SetDbLastVersion( (int)AppEnum.Sync.Stock );

			stockHash.Remove(oParam.SysNo);
			stockHash.Add(oParam.SysNo, oParam);
			return result;
		}
		private void map(StockInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.StockID = Util.TrimNull(tempdr["StockID"]);
			oParam.StockName = Util.TrimNull(tempdr["StockName"]);
			oParam.Address = Util.TrimNull(tempdr["Address"]);
			oParam.Contact = Util.TrimNull(tempdr["Contact"]);
			oParam.Phone = Util.TrimNull(tempdr["Phone"]);
			oParam.Status = Util.TrimIntNull(tempdr["Status"]);
		    oParam.StockType = Util.TrimIntNull(tempdr["StockType"]);
		}

		public StockInfo Load(int paramSysno)
		{
			return stockHash[paramSysno] as StockInfo;
		}

		public StockInfo Load(string paramStockID)
		{
			StockInfo tempStock = null;
			foreach(StockInfo stock in stockHash.Values)
			{
				if(stock.StockID==paramStockID)
				{
					tempStock = stock;
					break;
				}
			}
			return tempStock;
		}

		public DataSet GetStockDs(Hashtable paramHash)
		{
			string sql = " select * from Stock ";
			if ( paramHash != null && paramHash.Count != 0 )
			{
				StringBuilder sb = new StringBuilder(100);
				sb.Append(" where 1=1 " );
				foreach(string key in paramHash.Keys)
				{
					sb.Append(" and ");
					object item = paramHash[key];
					if ( key == "some key special")
					{
						//special deal
					}
					else if ( item is int)
					{
						sb.Append(key).Append("=" ).Append(item.ToString());
					}
					else if ( item is string)
					{
						sb.Append(key).Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
					}
				}
				sql += sb.ToString();
			}
			return SqlHelper.ExecuteDataSet(sql);
		}
	}
}
