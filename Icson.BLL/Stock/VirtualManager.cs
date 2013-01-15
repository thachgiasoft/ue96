using System;
using System.Data;
using System.Collections;
using System.Text;

using Icson.Utils;
using System.Transactions;
using Icson.Objects.Stock;
using Icson.DBAccess;
using Icson.DBAccess.Stock;
using Icson.BLL.Basic;
using Icson.BLL.Finance;

namespace Icson.BLL.Stock
{
	/// <summary>
	/// Summary description for VirtualManager.
	/// </summary>
	public class VirtualManager
	{
		private VirtualManager()
		{
		}

		private static VirtualManager _instance;
		public static VirtualManager GetInstance()
		{
			if( _instance == null )
			{
				_instance = new VirtualManager();
			}
			return _instance;
		}

		public DataSet GetVirtualListDs(Hashtable paramHash)
		{
			string sql = @" select st.*, a.username as CreateUserName, productid, productname
							from st_virtual st, sys_user a, Product 
							where 
								st.createusersysno *= a.sysno
								and st.productsysno = product.sysno";
			if ( paramHash != null && paramHash.Count != 0 )
			{
				StringBuilder sb = new StringBuilder(100);
				foreach(string key in paramHash.Keys)
				{
					sb.Append(" and ");
					object item = paramHash[key];
					if ( key == "DateFrom")
					{
						sb.Append("st.CreateTime >= ").Append(Util.ToSqlString(item.ToString()));
					}
					else if ( key == "DateTo")
					{
						sb.Append("st.CreateTime <= ").Append(Util.ToSqlEndDate(item.ToString()));
					}
                    else if (key == "SysNo")
                    {
                        sb.Append("st.SysNo = ").Append(item.ToString());
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

			sql += " order by st.sysno desc";
			return SqlHelper.ExecuteDataSet(sql);
		}

		#region map
		private void map(VirtualInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.ProductSysNo = Util.TrimIntNull(tempdr["ProductSysNo"]);
			oParam.VirtualQty = Util.TrimIntNull(tempdr["VirtualQty"]);
			oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
			oParam.CreateUserSysNo = Util.TrimIntNull(tempdr["CreateUserSysNo"]);
		}
		#endregion

		public void Create(VirtualInfo oParam)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				//½¨Á¢¼ÇÂ¼
				int rowsAffected = new VirtualDac().Insert(oParam);
				if(rowsAffected != 1)
					throw new BizException("insert virtual error");

				InventoryManager.GetInstance().SetVirtualQty(oParam.ProductSysNo, oParam.VirtualQty);

				scope.Complete();
            }
		}
	}
}
