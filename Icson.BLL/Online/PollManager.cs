using System;
using System.Data;
using System.Collections;
using System.Text;

using Icson.Utils;
using System.Transactions;

using Icson.DBAccess;
using Icson.DBAccess.Online;
using Icson.Objects;
using Icson.Objects.Online;

namespace Icson.BLL.Online
{
	/// <summary>
	/// Summary description for PollManager.
	/// </summary>
	public class PollManager
	{
		private PollManager()
		{
		}
		private static PollManager _instance;
		public static PollManager GetInstance()
		{
			if( _instance == null )
			{
				_instance = new PollManager();
			}
			return _instance;
		}
		public void map(PollInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.PollName = Util.TrimNull(tempdr["PollName"]);
			oParam.PollCount = Util.TrimIntNull(tempdr["PollCount"]);
			oParam.Status = Util.TrimIntNull(tempdr["Status"]);
		}
		public void map(PollItemInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.PollSysNo = Util.TrimIntNull(tempdr["PollSysNo"]);
			oParam.ItemName = Util.TrimNull(tempdr["ItemName"]);
			oParam.ItemCount = Util.TrimIntNull(tempdr["ItemCount"]);
		}
		public void Insert(PollInfo oParam)
		{
			new PollDac().Insert(oParam);
		}
		public void InsertItem(PollItemInfo oParam)
		{
			new PollDac().InsertItem(oParam);
		}
		public void Update(Hashtable ht)
		{
			new PollDac().Update(ht);
		}
		public void UpdateItem(Hashtable ht)
		{
			new PollDac().UpdateItem(ht);
		}
		public void DeleteItem(int itemSysNo)
		{
			new PollDac().DeleteItem(itemSysNo);
		}
		public PollInfo Load(int sysno)
		{
			string sql = "select * from poll where sysno = " + sysno;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( !Util.HasMoreRow(ds))
				return null;
			PollInfo oPoll = new PollInfo();
			map(oPoll, ds.Tables[0].Rows[0]);

			string sqlItem = "select * from poll_item where pollsysno = " + sysno +" order by ItemName" ;
			DataSet dsItem = SqlHelper.ExecuteDataSet(sqlItem);
			if ( Util.HasMoreRow(dsItem))
			{
				foreach(DataRow dr in dsItem.Tables[0].Rows)
				{
					PollItemInfo oPollItem = new PollItemInfo();
					map(oPollItem, dr);
					oPoll.itemHash.Add(oPollItem.SysNo, oPollItem);
				}
			}
			return oPoll;
		}
		public void DoPoll(int pollSysNo, int itemSysNo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				new PollDac().DoPoll(pollSysNo, itemSysNo);

				scope.Complete();
            }
		}

		public DataSet GetPollDs(Hashtable paramHash)
		{
			string sql = @" select * from poll where 1=1 ";
			if ( paramHash != null && paramHash.Count != 0 )
			{
				StringBuilder sb = new StringBuilder(100);
				foreach(string key in paramHash.Keys)
				{
					sb.Append(" and ");
					object item = paramHash[key];
					
					if ( key == "some special key" )
					{
						//sb.Append("po.Status = ").Append(item.ToString());
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
			else
			{
				sql = sql.Replace("select", "select top 50");
			}

			sql += " order by sysno desc";
			return SqlHelper.ExecuteDataSet(sql);
		}

		public void Import()
		{
			if ( !AppConfig.IsImportable)
				throw new BizException("Is Importable is false");

			/*  do not  use the following code after Data Pour in */
			string sql = " select top 1 * from poll ";
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds) )
				throw new BizException("the table poll is not empty");

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				string sql1 = "select * from ipp2003..poll";
				DataSet ds1 = SqlHelper.ExecuteDataSet(sql1);
				foreach(DataRow dr1 in ds1.Tables[0].Rows)
				{
					PollInfo oPoll = new PollInfo();
					map(oPoll,dr1);
					switch( oPoll.Status) {
						case -1:
						case 0:
							oPoll.Status = (int)AppEnum.BiStatus.InValid;
							break;
						case 1:
							oPoll.Status = (int)AppEnum.BiStatus.Valid;
							break;
						default:
							throw new BizException("no such status");
					}
					//在sysnono还没有改变以前构造一下item的字符串:）
					string sql2 = "select * from ipp2003..poll_item where pollsysno=" + oPoll.SysNo;
					new PollDac().Insert(oPoll);

					DataSet ds2 = SqlHelper.ExecuteDataSet(sql2);
					foreach(DataRow dr2 in ds2.Tables[0].Rows)
					{
						PollItemInfo oPollItem = new PollItemInfo();
						map(oPollItem, dr2);
						oPollItem.PollSysNo = oPoll.SysNo;
						new PollDac().InsertItem(oPollItem);
					}
				}
				
				
			scope.Complete();
            }
		}
	}
}
