using System;
using System.Collections;
using System.Data;
using System.Text;
using Icson.DBAccess;
using Icson.DBAccess.Sale;
using Icson.Objects.Sale;
using System.Transactions;
using Icson.Utils;

namespace Icson.BLL.Sale
{
	/// <summary>
	/// Summary description for DOManager.
	/// </summary>
	public class DOManager
	{
		public DOManager()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		
		private static DOManager _instance = new DOManager();

		public static DOManager GetInstance()
		{
			if(_instance == null)
			{
				_instance =  new DOManager();
			}
			return _instance;
		}
		
		private void map(DOInfo oInfo,DataRow tempdr)
		{
			oInfo.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oInfo.SOSysNo = Util.TrimIntNull(tempdr["SOSysNo"]);
			oInfo.DONo = Util.TrimNull(tempdr["DONo"]);
			oInfo.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
		}
		
		public DOInfo LoadDO(int sysNo)
		{
			string sql = "select * from do_master where sysno = @SysNo";
            sql = sql.Replace("@SysNo", sysNo.ToString());
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( !Util.HasMoreRow(ds))
				return null;
			DataRow tempdr = ds.Tables[0].Rows[0];
			DOInfo oInfo = new DOInfo();
			this.map(oInfo,tempdr);
			return oInfo;
		}

        public DOInfo LoadDOBySO(int SOSysNo)
        {
            string sql = "select * from do_master where sosysno = @SOSysNo";
            sql = sql.Replace("@SOSysNo",SOSysNo.ToString());
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;
            DataRow tempdr = ds.Tables[0].Rows[0];
            DOInfo oInfo = new DOInfo();
            this.map(oInfo, tempdr);
            return oInfo;
        }
		
		public DataSet GetDOList(Hashtable paramHash)
		{
			string sql = @"select dm.*,sm.soid,st.ShipTypeName,sys_user.username as checkqtyusername,sm.checkqtytime 
                            from DO_Master dm
							left join SO_Master sm on sm.sysno =  dm.sosysno
                            left join sys_user on sys_user.sysno = sm.checkqtyusersysno 
							left join ShipType st on st.sysno = sm.shiptypesysno";
			if(paramHash.Count>0)
			{
				StringBuilder sb = new StringBuilder();
				sb.Append(" where 1=1 ");
				foreach(string key in paramHash.Keys)
				{
					sb.Append(" and ");
					object item = paramHash[key];
					if (key=="DOSysNo")
					{
						sb.Append("dm.sysno = ").Append(item.ToString());
					}
					else if(key=="SOSysNo")
					{
						sb.Append("sm.Sysno = ").Append(item.ToString());
					}
                    else if (key == "StartDate")
                    {
                        sb.Append("dm.createtime").Append(">=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "EndDate")
                    {
                        sb.Append("dm.createtime").Append("<=").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "ShipTypeSysNo")
                    {
                        sb.Append("st.sysno").Append("=").Append(item.ToString());
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
				sql.Replace("select","select top 50");
			}
			sql += " order by dm.sysno desc";
			return SqlHelper.ExecuteDataSet(sql);
		}
		
		public void AddDO(DOInfo oInfo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {	
				new DODac().Insert(oInfo);
				scope.Complete();
            }
		}
		

		public void UpdateDO(DOInfo oInfo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {	
				new DODac().Update(oInfo);
				scope.Complete();
            }
		}
	}
}
