using System;
using System.Data;
using System.Collections;
using System.Text;

using Icson.Utils;
using System.Transactions;
using Icson.Objects;
using Icson.Objects.Finance;

using Icson.DBAccess;
using Icson.DBAccess.Finance;

using Icson.BLL.Sale;

namespace Icson.BLL.Finance
{
	/// <summary>
	/// Summary description for NetPayManager.
	/// </summary>
	public class NetPayManager
	{
		private NetPayManager()
		{
		}
		private static NetPayManager _instance;
		public static NetPayManager GetInstance()
		{
			if( _instance == null )
			{
				_instance = new NetPayManager();
			}
			return _instance;
		}
		private void map(NetPayInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.SOSysNo = Util.TrimIntNull(tempdr["SOSysNo"]);
			oParam.PayTypeSysNo = Util.TrimIntNull(tempdr["PayTypeSysNo"]);
			oParam.PayAmount = Util.TrimDecimalNull(tempdr["PayAmount"]);
			oParam.Source = Util.TrimIntNull(tempdr["Source"]);
			oParam.InputTime = Util.TrimDateNull(tempdr["InputTime"]);
			oParam.InputUserSysNo = Util.TrimIntNull(tempdr["InputUserSysNo"]);
			oParam.ApproveUserSysNo = Util.TrimIntNull(tempdr["ApproveUserSysNo"]);
			oParam.ApproveTime = Util.TrimDateNull(tempdr["ApproveTime"]);
			oParam.Note = Util.TrimNull(tempdr["Note"]);
			oParam.Status = Util.TrimIntNull(tempdr["Status"]);
		}

		public DataSet GetNetPayDs(Hashtable paramHash)
		{
//            string sql = @"select netpay.*, paytype.paytypename, inputuser.username as InputUserName, 
//									approveuser.username as ApproveUserName
//							from 
//								finance_netpay netpay
//								paytype,
//								sys_user inputuser,
//								sys_user approveuser 
//							where 
//								netpay.paytypesysno = paytype.sysno
//								and netpay.InputUserSysno *= inputuser.sysno
//								and netpay.ApproveUserSysno *= approveuser.sysno ";
            string sql = @"select netpay.*, paytype.paytypename, inputuser.username as InputUserName, 
									approveuser.username as ApproveUserName,so_alipay.alipaytradeno
							from 
								finance_netpay netpay inner join paytype on netpay.paytypesysno = paytype.sysno
								left join sys_user inputuser on netpay.InputUserSysno = inputuser.sysno
                                left join sys_user approveuser on netpay.ApproveUserSysno = approveuser.sysno 
                                left join so_alipay on netpay.sosysno = so_alipay.sosysno 
							where 1=1 ";
			if ( paramHash != null && paramHash.Count != 0 )
			{
				StringBuilder sb = new StringBuilder(100);
				foreach(string key in paramHash.Keys)
				{
					sb.Append(" and ");
					object item = paramHash[key];
					if ( key == "DateFrom")
					{
						sb.Append("InputTime >= ").Append(Util.ToSqlString(item.ToString()));
					}
					else if ( key == "DateTo")
					{
						sb.Append("InputTime <= ").Append(Util.ToSqlEndDate(item.ToString()));
					}
					else if ( key == "Status" )
					{
						sb.Append("netpay.Status = ").Append(item.ToString());
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

			sql += " order by netpay.inputtime desc";
			return SqlHelper.ExecuteDataSet(sql);
		}

		public NetPayInfo Load(int sysno)
		{
			NetPayInfo oInfo = null;
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				
				string sql = "select * from finance_netpay where sysno =" + sysno;
				DataSet ds = SqlHelper.ExecuteDataSet(sql);
				if ( Util.HasMoreRow(ds))
				{
					oInfo = new NetPayInfo();
					map(oInfo, ds.Tables[0].Rows[0]);
				}
				scope.Complete();
            }
			return oInfo;			
		}

		public bool IsPayed(int soSysNo)
		{
			string sql = "select top 1 sysno from finance_netpay where status <>" + (int)AppEnum.NetPayStatus.Abandon + " and sosysno = " + soSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds))
				return true;
			else
				return false;
		}

		public bool IsPayedForAlipay(int soSysNo)
		{
			string sql = "select top 1 sysno from finance_netpay where sosysno = " + soSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds))
				return true;
			else
				return false;
		}

		public bool IsPayedFor99bill(int soSysNo)
		{
			string sql = "select top 1 sysno from finance_netpay where sosysno = " + soSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds))
				return true;
			else
				return false;
		}

		public NetPayInfo LoadVerified(int soSysNo)
		{
			NetPayInfo oInfo = null;
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				
				string sql = "select * from finance_netpay where sosysno =" + soSysNo + " and status=" + (int)AppEnum.NetPayStatus.Verified;
				DataSet ds = SqlHelper.ExecuteDataSet(sql);
				if ( Util.HasMoreRow(ds))
				{
					oInfo = new NetPayInfo();
					map(oInfo, ds.Tables[0].Rows[0]);
				}
				scope.Complete();
            }
			return oInfo;			
		}

		public void AddVerified(NetPayInfo oParam, int userSysNo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				this.Insert(oParam);
				this.Verify(oParam.SysNo, userSysNo);

				scope.Complete();
            }
		}
		public void Insert(NetPayInfo oParam)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				new NetPayDac().Insert(oParam);

				scope.Complete();
            }
		}
		public void ManualAbandon(int netpaySysNo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				NetPayInfo dbInfo = Load(netpaySysNo);

				if ( dbInfo.Status != (int)AppEnum.NetPayStatus.Origin)
					throw new BizException("netpay: not origin, can't abandon");

				

				Abandon(dbInfo);

				scope.Complete();
            }
		}
		public void SOIncomeAbandon(int soSysNo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				NetPayInfo dbInfo = LoadVerified(soSysNo);

				if ( dbInfo != null)
				{
					if ( dbInfo.Status != (int)AppEnum.NetPayStatus.Verified)
						throw new BizException("netpay: not verified, can't abandon");

					Abandon(dbInfo);
				}
				scope.Complete();
            }
		}
		private void Abandon(NetPayInfo dbInfo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				Hashtable ht = new Hashtable(5);
				ht.Add("SysNo", dbInfo.SysNo);
				ht.Add("Status", (int)AppEnum.NetPayStatus.Abandon);

				new NetPayDac().Update(ht);

				scope.Complete();
            }

		}
		public void Verify(int netpaySysNo, int userSysNo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				NetPayInfo dbInfo = Load(netpaySysNo);
				if ( dbInfo.Status != (int)AppEnum.NetPayStatus.Origin)
					throw new BizException("netpay: not origin, can't verify");

				dbInfo.Status = (int)AppEnum.NetPayStatus.Verified;

				Hashtable ht = new Hashtable(5);
				ht.Add("SysNo", dbInfo.SysNo);
				ht.Add("ApproveUserSysNo", userSysNo);
				ht.Add("ApproveTime", DateTime.Now);
				ht.Add("Status", dbInfo.Status);

				new NetPayDac().Update(ht);

				//so income 的唯一性那边会判断的
				SOIncomeInfo oIncome = new SOIncomeInfo();
				oIncome.OrderType = (int)AppEnum.SOIncomeOrderType.SO;
				oIncome.OrderSysNo = dbInfo.SOSysNo;
				oIncome.OrderAmt = dbInfo.PayAmount;
				oIncome.IncomeStyle = (int)AppEnum.SOIncomeStyle.Advanced;
				oIncome.IncomeAmt = dbInfo.PayAmount;
				oIncome.IncomeUserSysNo = userSysNo;
				oIncome.IncomeTime = DateTime.Now;
				oIncome.Status = (int)AppEnum.SOIncomeStatus.Origin;

				SOIncomeManager.GetInstance().Insert(oIncome);
                
				scope.Complete();
            }
		}
//		public void UnVerify(NetPayInfo oParam)
//		{
//			TransactionContext ctx = 
//				TransactionContextFactory.GetContext(TransactionAffinity.Required);
//			try
//			{
//				ctx.Enter();
//
//				NetPayInfo dbInfo = Load(oParam.SysNo);
//				if ( dbInfo.Status != (int)AppEnum.NetPayStatus.Verified)
//					throw new BizException("netpay: not verified, can't cancel verify");
//
//				oParam.Status = (int)AppEnum.NetPayStatus.Origin;
//
//				Hashtable ht = new Hashtable(5);
//				ht.Add("SysNo", oParam.SysNo);
//				ht.Add("ApproveUserSysNo", oParam.ApproveUserSysNo);
//				ht.Add("ApproveTime", oParam.ApproveTime);
//				ht.Add("Status", oParam.Status);
//
//				new NetPayDac().Update(ht);
//
//				//???????????????????????
//				//SOIncomeManager.GetInstance().Delete((int)AppEnum.SOIncomeOrderType.SO, oParam.SOSysNo);
//
//				
//			}
//			catch(Exception ex) 
//			{
//				ctx.VoteRollback();
//				throw ex;
//			}
//			finally
//			{
//				ctx.Exit();
//			}
//		}

		public void Import()
		{
			/* 涉及的问题 
			 * 1 还货记录的处理
			 * 2 库存的处理
			 * 3 状态的处理
			 * 4 还有单据id的对应，这个要特别注意
			 */
			if ( !AppConfig.IsImportable)
				throw new BizException("Is Importable is false");
			
			string sql = " select top 1 sysno from finance_netpay";
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds) )
				throw new BizException("the table finance_netpay is not empty");

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				//因为原来没有inputuser，所以用1
				string sql_old = @"select 
									a.sosysno, a.payamount,
									a.source, a.inputtime,
									con_paytype.newsysno as paytypesysno,
									con_user.newsysno as approveUserSysno,
									a.approveTime,
									a.note,
									a.status,
									'1' as inputusersysno,
									'1' as sysno
								from 
									ipp2003..net_pay as a,
									ippconvert..paytype as con_paytype,
									ippconvert..sys_user as con_user
								where
									a.paytypesysno = con_paytype.oldsysno
									and a.approveusersysno *= con_user.oldsysno
									--and status <>-1
								order by sosysno, a.status desc";

				DataSet ds_old = SqlHelper.ExecuteDataSet(sql_old);
				if ( !Util.HasMoreRow(ds_old) )
					return;

				//int currentSoSysNo = AppConst.IntNull;
				foreach(DataRow dr in ds_old.Tables[0].Rows)
				{

					/* oldstatus
					 * -1	deleted		
					 * 0	origin			
					 * 1	aduited
					 * 
					 * oldsource
					 * 0 bank insert
					 * 1 employee insert	
					 * 导数据的规则是：只插入遇到相同的sosysno的第一个		
					 */
					NetPayInfo oNetPay = new NetPayInfo();
					map(oNetPay, dr);
//					if ( currenSoSysNo != oNetPay.SOSysNo )
//					{
//						currenSoSysNo = oNetPay.SOSysNo;
//						this.Insert(oNetPay);
//					}
					this.Insert(oNetPay);
				}				

				
			scope.Complete();
            }

		}


	}
}
