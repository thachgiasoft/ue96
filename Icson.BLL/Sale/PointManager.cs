using System;
using System.Data;
using System.Collections;
using System.Text;

using Icson.Utils;
using System.Transactions;
using Icson.Objects;
using Icson.Objects.Basic;
using Icson.Objects.Sale;
using Icson.DBAccess;
using Icson.DBAccess.Sale;
using Icson.BLL;
using Icson.BLL.Basic;


namespace Icson.BLL.Sale
{
	/// <summary>
	/// Summary description for PointManager.
	/// </summary>
	public class PointManager
	{
		public PointManager()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		private static PointManager _instance;
		public static PointManager GetInstance()
		{
			if(_instance==null)
				_instance = new PointManager();
			return _instance;
		}

		public DataSet GetPointLogDs(int customerSysNo)
		{
			string sql = "select * from customer_pointlog where customersysno=" + customerSysNo + " order by sysno DESC";
			return SqlHelper.ExecuteDataSet(sql);
		}

		private void map(CustomerPointLogInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.CustomerSysNo = Util.TrimIntNull(tempdr["CustomerSysNo"]);
			oParam.PointLogType = Util.TrimIntNull(tempdr["PointLogType"]);
			oParam.PointAmount = Util.TrimIntNull(tempdr["PointAmount"]);
			oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
			oParam.Memo = Util.TrimNull(tempdr["Memo"]);
			oParam.LogCheck = Util.TrimNull(tempdr["LogCheck"]);
		}

		private void map(SalePointDelayInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.SOSysNo = Util.TrimIntNull(tempdr["SOSysNo"]);
			oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
			oParam.Status = Util.TrimIntNull(tempdr["Status"]);
		}
        
        private void map(CustomerPointRequestInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.CustomerSysNo = Util.TrimIntNull(tempdr["CustomerSysNo"]);
            oParam.PointSourceType = Util.TrimIntNull(tempdr["PointSourceType"]);
            oParam.PointSourceSysNo = Util.TrimIntNull(tempdr["PointSourceSysNo"]);
            oParam.PointLogType = Util.TrimIntNull(tempdr["PointLogType"]);
            oParam.PointAmount = Util.TrimIntNull(tempdr["PointAmount"]);
            oParam.RequestUserType = Util.TrimIntNull(tempdr["RequestUserType"]);
            oParam.RequestUserSysNo = Util.TrimIntNull(tempdr["RequestUserSysNo"]);
            oParam.RequestTime = Util.TrimDateNull(tempdr["RequestTime"]);
            oParam.AuditUserSysNo = Util.TrimIntNull(tempdr["AuditUserSysNo"]);
            oParam.AuditTime = Util.TrimDateNull(tempdr["AuditTime"]);
            oParam.AddUserSysNo = Util.TrimIntNull(tempdr["AddUserSysNo"]);
            oParam.AddTime = Util.TrimDateNull(tempdr["AddTime"]);
            oParam.AbandonUserSysNo = Util.TrimIntNull(tempdr["AbandonUserSysNo"]);
            oParam.AbandonTime = Util.TrimDateNull(tempdr["AbandonTime"]);
            oParam.Memo = Util.TrimNull(tempdr["Memo"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
            oParam.PMUserSysNo = Util.TrimIntNull(tempdr["PMUserSysNo"]);
        }

		#region point delay
		public SalePointDelayInfo LoadValid(int soSysNo)
		{
			SalePointDelayInfo spInfo = null;
			string sql = @"select * from sale_pointdelay where status<>"+(int)AppEnum.TriStatus.Abandon+" and sosysno="+soSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if(Util.HasMoreRow(ds))
			{
				spInfo = new SalePointDelayInfo();
				this.map(spInfo,ds.Tables[0].Rows[0]);
			}
			return spInfo;
		}

		public void InsertPointDelay(SalePointDelayInfo oParam)
		{
			string sql = @"select * from sale_pointdelay where status>"+(int)AppEnum.TriStatus.Abandon+" and sosysno ="+oParam.SOSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if(Util.HasMoreRow(ds))
				throw new BizException("A valid delaypoint record of this SO already exists,can't add another.'");
			new PointDac().InsertPointDelay(oParam);
		}

		public void UpdatePointDelay(SalePointDelayInfo oParam)
		{
			new PointDac().UpdatePointDelay(oParam);
		}
		#endregion

		//2006-08-11
		public bool HasAddScoreForHF(int CustomerSysNo,int PointLogType)
		{
			string  sql ="Select * from Customer_PointLog Where CustomerSysNo="+ CustomerSysNo +" And PointLogType="+ PointLogType +"";
			
            DataSet ds = SqlHelper.ExecuteDataSet(sql);

			if(ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		//2006-08-11
		public void AddScore(int CustomerSysNo,int increment)
		{
			if(HasAddScoreForHF(CustomerSysNo,17))
			{
				return;
			}
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				int rowsAffected = new PointDac().SetScore(CustomerSysNo, increment);
				if(rowsAffected!=1)
					throw new BizException("客户积分更新失败，可能因为积分不足。");
				
				if ( increment != 0)
				{
					int pointLogType = 17;
					string poingLogMemo = "浩号送积分";
					CustomerPointLogInfo oPointLog = new CustomerPointLogInfo(CustomerSysNo, pointLogType, increment, poingLogMemo);
					oPointLog.LogCheck = oPointLog.CalcLogCheck();

					if ( 1!=new PointDac().InsertLog(oPointLog))
						throw new BizException("增加积分流水失败");
				}

				scope.Complete();
            }
		}

		//2006-10-31 给指定的送积分类型增加积分
		public void AddScore(int CustomerSysNo,int increment,int pointLogType,string poingLogMemo)
		{
            if (pointLogType == (int)AppEnum.PointLogType.forHF && HasAddScoreForHF(CustomerSysNo, pointLogType))
            {
                throw new BizException("浩号不能重复增加积分");
            }
            
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				int rowsAffected = new PointDac().SetScore(CustomerSysNo, increment);
				if(rowsAffected!=1)
					throw new BizException("客户积分更新失败，可能因为积分不足。");
				
				if ( increment != 0)
				{
					CustomerPointLogInfo oPointLog = new CustomerPointLogInfo(CustomerSysNo, pointLogType, increment, poingLogMemo);
					oPointLog.LogCheck = oPointLog.CalcLogCheck();

					if ( 1!=new PointDac().InsertLog(oPointLog))
						throw new BizException("增加积分流水失败");
				}

				scope.Complete();
            }
		}
		
		public void SetScore(int customerSysNo, int pointDelt, int pointLogType, string poingLogMemo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				int rowsAffected = new PointDac().SetScore(customerSysNo, pointDelt);
				if(rowsAffected!=1)
					throw new BizException("客户积分更新失败，可能因为积分不足。");
				
				if ( pointDelt != 0)
				{
					CustomerPointLogInfo oPointLog = new CustomerPointLogInfo(customerSysNo, pointLogType, pointDelt, poingLogMemo);
					oPointLog.LogCheck = oPointLog.CalcLogCheck();

					if ( 1!=new PointDac().InsertLog(oPointLog))
						throw new BizException("增加积分流水失败");
				}

				scope.Complete();
            }
		}

		private void doDelayPoint(SalePointDelayInfo spInfo,int pointDelt,int customerSysNo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				//更新point delay状态
				spInfo.Status = (int)AppEnum.TriStatus.Handled;
				this.UpdatePointDelay(spInfo);
				//设置用户积分
				PointManager.GetInstance().SetScore(customerSysNo, pointDelt, (int)AppEnum.PointLogType.AddPointLater, spInfo.SOSysNo.ToString());
				scope.Complete();
            }
		}

		public void DoDelayPoint()
		{
			string sql = @"select sp.* ,sm.pointamt,sm.customersysno from sale_pointdelay sp(nolock) 
						   inner join so_master sm(nolock) on sm.sysno = sp.sosysno 
                           inner join finance_soincome fs(nolock) on sm.sysno=fs.ordersysno 
						   where sm.pointamt <> 0 and sm.status = "+(int)AppEnum.SOStatus.OutStock+" and sp.status = "+(int)AppEnum.TriStatus.Origin+" and createtime<="+ Util.ToSqlString( DateTime.Now.AddDays(-3).ToString(AppConst.DateFormatLong))  + " and fs.ordertype=" + (int)AppEnum.SOIncomeOrderType.SO + " and fs.status=" + (int)AppEnum.SOIncomeStatus.Confirmed;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);

			if(Util.HasMoreRow(ds))
			{
				Hashtable failHash = new Hashtable();
				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					SalePointDelayInfo spInfo = new SalePointDelayInfo();
					this.map(spInfo,dr);
					try
					{
						this.doDelayPoint(spInfo,(int)dr["PointAmt"],(int)dr["CustomerSysNo"]);
					}
					catch
					{
						failHash.Add(spInfo.SysNo,spInfo);
					}
				}

				TCPMail oMail = new TCPMail();
				if(failHash.Count>0)
				{
					StringBuilder sb = new StringBuilder();
					sb.Append("<table align='center' border='1' cellpadding='0' cellspacing='0'>");
					sb.Append(" <tr>");
					sb.Append("  <td>SysNo</td>");
					sb.Append("  <td>SOSysNo</td>");
					sb.Append("  <td>CreateTime</td>");
					sb.Append(" </tr>");
					foreach(SalePointDelayInfo failInfo in failHash.Values)
					{
						sb.Append("<tr>");
						sb.Append(" <td>"+failInfo.SysNo+"</td>");
						sb.Append(" <td>"+failInfo.SOSysNo+"</td>");
						sb.Append(" <td>"+failInfo.CreateTime+"</td>");
						sb.Append("</tr>");
					}
					sb.Append("</table>");
					oMail.Send(AppConfig.AdminEmail,"Add Delay Point---Failed : "+DateTime.Now.ToString(AppConst.DateFormatLong),sb.ToString());
				}
				else
					oMail.Send(AppConfig.AdminEmail,"Add Delay Point---OK : "+DateTime.Now.ToString(AppConst.DateFormatLong), "");
			}
			else
			{
				TCPMail oMail = new TCPMail();
				oMail.Send(AppConfig.AdminEmail, "Add Delay Point None---: " + DateTime.Now.ToString(AppConst.DateFormatLong), "");
			}
			
		}
		
		/// <summary>
		/// 单独增加对应订单的积分
		/// </summary>
		/// <param name="soSysNo"></param>
		public void DoDelayPointSingle(int soSysNo)
		{
			string sql = @"select sp.* ,sm.pointamt,sm.customersysno from sale_pointdelay sp 
						   inner join so_master sm on sm.sysno = sp.sosysno
						   where sm.pointamt <> 0 and sm.status = "+(int)AppEnum.SOStatus.OutStock+" and sp.status = "+(int)AppEnum.TriStatus.Origin+" and sm.sysno ="+soSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if(Util.HasMoreRow(ds))
			{
				SalePointDelayInfo spInfo = new SalePointDelayInfo();
				DataRow dr = ds.Tables[0].Rows[0];
				this.map(spInfo,dr);
				this.doDelayPoint(spInfo,(int)dr["PointAmt"],(int)dr["CustomerSysNo"]);
			}
		}

		public void CheckUserScore()
		{
			string sql = @"select customersysno,sum(pointamount) as PointLogTotal ,validscore
						   from customer_pointlog cp
						   inner join customer c on c.sysno = cp.customersysno
						   where c.status ="+(int)AppEnum.BiStatus.Valid
						+@" group by customersysno,validscore
						   having sum(pointamount)<>validscore";
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			Hashtable errorHash = new Hashtable();
			Hashtable failHash = new Hashtable();
			TCPMail oMail = new TCPMail();
			if(Util.HasMoreRow(ds))
			{
				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					try
					{
						Hashtable paramHash = new Hashtable();
						paramHash.Add("SysNo",(int)dr["CustomerSysNo"]);
						paramHash.Add("Status",(int)AppEnum.BiStatus.InValid);
						CustomerManager.GetInstance().Update(paramHash);
						errorHash.Add((int)dr["CustomerSysNo"],dr);
					}
					catch
					{
						failHash.Add((int)dr["CustomerSysNo"],dr);
					}
				}
			}
			if(errorHash.Count>0)
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("<table align='center' border='0' cellpadding='0' cellspacing='0'>");
				sb.Append(" <tr>");
				sb.Append("  <td>CustomerSysNo</td>");
				sb.Append("  <td>PointLogTotal</td>");
				sb.Append("  <td>ValidScore</td>");
				sb.Append(" </tr>");
				foreach(DataRow drer in errorHash.Values)
				{
					sb.Append("<tr>");
					sb.Append("	<td>"+drer["CustomerSysNo"].ToString()+"</td>");
					sb.Append(" <td>"+drer["PointLogTotal"].ToString()+"</td>");
					sb.Append(" <td>"+drer["ValidScore"].ToString()+"</td>");
					sb.Append("</tr>");
				}
				sb.Append("</table>");
				oMail.Send(AppConfig.AdminEmail,"ScoreErrorUpdated:"+DateTime.Now.ToLongDateString(),sb.ToString());
			}
			if(failHash.Count>0)
			{
				StringBuilder sb1 = new StringBuilder();
				sb1.Append("<table align='center' border='0' cellpadding='0' cellspacing='0'>");
				sb1.Append(" <tr>");
				sb1.Append("  <td>CustomerSysNo</td>");
				sb1.Append("  <td>PointLogTotal</td>");
				sb1.Append("  <td>ValidScore</td>");
				sb1.Append(" </tr>");
				foreach(DataRow drer in errorHash.Values)
				{
					sb1.Append("<tr>");
					sb1.Append("	<td>"+drer["CustomerSysNo"].ToString()+"</td>");
					sb1.Append(" <td>"+drer["PointLogTotal"].ToString()+"</td>");
					sb1.Append(" <td>"+drer["ValidScore"].ToString()+"</td>");
					sb1.Append("</tr>");
				}
				sb1.Append("</table>");
				oMail.Send(AppConfig.AdminEmail,"ScoreErrorNotUpdated:"+DateTime.Now.ToLongDateString(),sb1.ToString());
			}
		}

		public void ImportPointLog()
		{
			if ( !AppConfig.IsImportable)
				throw new BizException("Is Importable is false");

			/*  do not  use the following code after Data Pour in */
			string sql = " select top 1 * from Customer_PointLog ";
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds) )
				throw new BizException("the table Point log is not empty");

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				//先清理ipp2003里面积分log amt为零的记录
				new PointDac().DeleteLog();

				string sql1 = @"select pkid as sysno, customersysno, logtype as pointlogtype,amount as pointamount, logtime as createtime, memo, '' as LogCheck
								from ipp2003..pointlog order by sysno";
				DataSet ds1 = SqlHelper.ExecuteDataSet(sql1);
				foreach(DataRow dr1 in ds1.Tables[0].Rows )
				{
					CustomerPointLogInfo oPoint = new CustomerPointLogInfo();
					map(oPoint, dr1);
					oPoint.LogCheck = oPoint.CalcLogCheck();
					new PointDac().InsertLog(oPoint);
				}
				scope.Complete();
            }
		}
		public void ImportPointDelay()
		{
			if ( !AppConfig.IsImportable)
				throw new BizException("Is Importable is false");
			string sql = " select top 1 * from Sale_PointDelay ";
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds) )
				throw new BizException("the table Point Delay is not empty");

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				string sqlOld = @"select * from ipp2003..PointPayDelay ";
				DataSet dsOld = SqlHelper.ExecuteDataSet(sqlOld);
				if(Util.HasMoreRow(dsOld))
				{
					foreach(DataRow dr in dsOld.Tables[0].Rows)
					{
						SalePointDelayInfo spInfo = new SalePointDelayInfo();
						this.map(spInfo,dr);
						new PointDac().InsertPointDelay(spInfo);
					}
				}
				scope.Complete();
            }
		}

        public int InsertCustomerPointRequest(CustomerPointRequestInfo oParam)
        {
            return new PointDac().InsertCustomerPointRequest(oParam);
        }

        public void UpdateCustomerPointRequest(Hashtable paramHash)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                if (paramHash.ContainsKey("Status"))
                {
                    int sysno = Int32.Parse(paramHash["SysNo"].ToString());
                    int newStatus = Int32.Parse(paramHash["Status"].ToString());
                    CustomerPointRequestInfo oInfo = LoadCustomerPointRequest(sysno);

                    int curStatus = oInfo.Status;
                    if(curStatus == (int)AppEnum.PointRequestStatus.Origin)
                    {
                        if (newStatus != (int)AppEnum.PointRequestStatus.Audited && newStatus != (int)AppEnum.PointRequestStatus.Abandon)
                            throw new BizException("非orign状态，无法进行该操作");
                    }
                    else if (curStatus == (int)AppEnum.PointRequestStatus.Audited)
                    {
                        if (newStatus != (int)AppEnum.PointRequestStatus.Added && newStatus != (int)AppEnum.PointRequestStatus.Origin)
                            throw new BizException("非audited状态，无法进行该操作");
                    }
                    else
                    {
                        throw new BizException("无法进行该操作");
                    }

                    new PointDac().UpdateCustomerPointRequest(paramHash);
                    if (newStatus == (int)AppEnum.PointRequestStatus.Added) //增加积分
                    {
                        this.SetScore(oInfo.CustomerSysNo,oInfo.PointAmount,oInfo.PointLogType,oInfo.SysNo + "_" + oInfo.Memo);
                    }
                }
                else
                {
                    new PointDac().UpdateCustomerPointRequest(paramHash);
                }
                scope.Complete();
            }
        }

        public CustomerPointRequestInfo LoadCustomerPointRequest(int sysno)
        {
            string sql = "select * from customer_pointrequest where sysno=" + sysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                CustomerPointRequestInfo oInfo = new CustomerPointRequestInfo();
                map(oInfo, ds.Tables[0].Rows[0]);
                return oInfo;
            }
            else
                return null;
        }

        public DataSet GetPointRequestList(Hashtable paramHash)
        {
            string sql = @"select cpr.*,
                            RequestUserName = case cpr.requestusertype when @employee then ru.username when @customer then customer.CustomerName end,
                            c.customerid,c.customername,au.username as AuditUserName,addu.username as AddUserName,abau.username as AbandonUserName 
                            from customer_pointrequest as cpr(NOLOCK) 
                            left join customer as c(NOLOCK) on cpr.customersysno=c.sysno 
                            left join customer(NOLOCK) on cpr.requestusersysno=customer.sysno 
                            left join sys_user as ru(NOLOCK) on cpr.requestusersysno=ru.sysno 
                            left join sys_user as au(NOLOCK) on cpr.auditusersysno=au.sysno
                            left join sys_user as addu(NOLOCK) on cpr.addusersysno=addu.sysno 
                            left join sys_user as abau(NOLOCK) on cpr.abandonusersysno=abau.sysno 
                            where  1=1 
                            @RequestTimeFrom @RequestTimeTo
                            @AuditTimeFrom @AuditTimeTo 
                            @AddTimeFrom @AddTimeTo
                            @PointSourceType @PointSourceSysNo @RequestSysNo @Status @CustomerSysNo order by cpr.requesttime desc";

            sql = sql.Replace("@employee", Util.ToSqlString(((int)AppEnum.CreateUserType.Employee).ToString()));
            sql = sql.Replace("@customer", Util.ToSqlString(((int)AppEnum.CreateUserType.Customer).ToString()));

            if (paramHash.ContainsKey("RequestTimeFrom"))
                sql = sql.Replace("@RequestTimeFrom", " and cpr.RequestTime >=" + Util.ToSqlString(paramHash["RequestTimeFrom"].ToString()));
            else
                sql = sql.Replace("@RequestTimeFrom", "");

            if (paramHash.ContainsKey("RequestTimeTo"))
                sql = sql.Replace("@RequestTimeTo", " and cpr.RequestTime <=" + Util.ToSqlEndDate(paramHash["RequestTimeTo"].ToString()));
            else
                sql = sql.Replace("@RequestTimeTo", "");

            if (paramHash.ContainsKey("AuditTimeFrom"))
                sql = sql.Replace("@AuditTimeFrom", " and cpr.AuditTime >=" + Util.ToSqlString(paramHash["AuditTimeFrom"].ToString()));
            else
                sql = sql.Replace("@AuditTimeFrom", "");
            if (paramHash.ContainsKey("AuditTimeTo"))
                sql = sql.Replace("@AuditTimeTo", " and cpr.AuditTime <=" + Util.ToSqlEndDate(paramHash["AuditTimeTo"].ToString()));
            else
                sql = sql.Replace("@AuditTimeTo", "");

            if (paramHash.ContainsKey("AddTimeFrom"))
                sql = sql.Replace("@AddTimeFrom", " and cpr.AddTime >=" + Util.ToSqlString(paramHash["AddTimeFrom"].ToString()));
            else
                sql = sql.Replace("@AddTimeFrom", "");
            if (paramHash.ContainsKey("AddTimeTo"))
                sql = sql.Replace("@AddTimeTo", " and cpr.AddTime <=" + Util.ToSqlEndDate(paramHash["AddTimeTo"].ToString()));
            else
                sql = sql.Replace("@AddTimeTo", "");

           if (paramHash.ContainsKey("PointSourceSysNo"))
               sql = sql.Replace("@PointSourceSysNo", " and cpr.PointSourceSysNo=" + Util.ToSqlString(paramHash["PointSourceSysNo"].ToString()));
            else
               sql = sql.Replace("@PointSourceSysNo", "");

           if (paramHash.ContainsKey("PointSourceType"))
               sql = sql.Replace("@PointSourceType", " and cpr.PointSourceType=" + Util.ToSqlString(paramHash["PointSourceType"].ToString()));
           else
               sql = sql.Replace("@PointSourceType", "");

            if (paramHash.ContainsKey("RequestSysNo"))
                sql = sql.Replace("@RequestSysNo", " and cpr.SysNo=" + Util.ToSqlString(paramHash["RequestSysNo"].ToString()));
            else
                sql = sql.Replace("@RequestSysNo", "");

            if (paramHash.ContainsKey("CustomerSysNo"))
            {
                sql = sql.Replace("@CustomerSysNo", "  and cpr.CustomerSysNo =" + Util.ToSqlString(paramHash["CustomerSysNo"].ToString()));
            }
            else
            {
                sql = sql.Replace("@CustomerSysNo", "");
            }

            if (paramHash.ContainsKey("Status"))
            {
                sql = sql.Replace("@Status", " and cpr.Status=" + Util.ToSqlString(paramHash["Status"].ToString()));
            }
            else
            {
                sql = sql.Replace("@Status", "");
            }

            if (paramHash == null || paramHash.Count == 1)
                sql = sql.Replace("select", "select top 50 ");

            return SqlHelper.ExecuteDataSet(sql);
        }
	}
}