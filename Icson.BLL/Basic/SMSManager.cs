using System;
using System.Data;
using System.Collections;
using System.Text;

using Icson.Utils;
using System.Transactions;
using Icson.Objects.Basic;
using Icson.Objects;
using Icson.DBAccess;
using Icson.DBAccess.Basic;

using Icson.Objects.ImportData;
using Icson.DBAccess.ImportData;

namespace Icson.BLL.Basic
{
	/// <summary>
	/// Summary description for SMSManager.
	/// </summary>
	public class SMSManager
	{
		private SMSManager()
		{
		}

		private static SMSManager _instance;
		
        public static SMSManager GetInstance()
		{
			if ( _instance == null )
			{
				_instance = new SMSManager();
			}
			return _instance;
		}

		public void Insert(SMSInfo oInfo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                //防止重复扫描，相同的短信只发送一次
                if(!IsExistsSMS(oInfo))
                    new SMSDac().Insert(oInfo);
			    scope.Complete();
            }
		}

        public int InsertSMS(SMSInfo oInfo)
        {
            return new SMSDac().Insert(oInfo);
        }

        private bool IsExistsSMS(SMSInfo oInfo)
        {
            string sql = "select * from sms where cellnumber=" + oInfo.CellNumber + " and smscontent=" + Util.ToSqlString(Util.SafeFormat(oInfo.SMSContent.Trim()));
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                return true;
            else 
                return false;
        }

        private void map(SMSInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.CellNumber = Util.TrimNull(tempdr["CellNumber"]);
            oParam.SMSContent = Util.TrimNull(tempdr["SMSContent"]);
            oParam.Priority = Util.TrimIntNull(tempdr["Priority"]);
            oParam.RetryCount = Util.TrimIntNull(tempdr["RetryCount"]);
            oParam.CreateUserSysNo = Util.TrimIntNull(tempdr["CreateUserSysNo"]);
            oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
            oParam.ExpectSendTime = Util.TrimDateNull(tempdr["ExpectSendTime"]);
            oParam.HandleTime = Util.TrimDateNull(tempdr["HandleTime"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
        }

        /// <summary>
        /// Search for the emails to be send
        /// </summary>
        /// <returns></returns>
        public Hashtable SearchAsyncSMS()
        {
            string sql = @"select * 
							from sms
							where expectsendtime < getdate() and status = 0";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            Hashtable smsHash = new Hashtable(5);
            if (Util.HasMoreRow(ds))
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    SMSInfo oInfo = new SMSInfo();
                    this.map(oInfo, dr);
                    smsHash.Add(oInfo, null);
                }
            }
            return smsHash;
        }

        public void UpdateSMSStatus(SMSInfo oInfo)
        {
            new SMSDac().UpdateSMSStatus(oInfo);
        }

        public void UpdateSMSRetryCount(SMSInfo oInfo)
        {
            string sql = "";
            if (oInfo.RetryCount == 0) //1小时后再补发一次
            {
                sql = "update sms set retrycount=1,expectsendtime=DATEADD(hour, 1, getdate()) where sysno=" + oInfo.SysNo;
            }
            else if (oInfo.RetryCount == 1) //3小时后再补发一次
            {
                sql = "update sms set retrycount=2,expectsendtime=DATEADD(hour, 2, getdate()) where sysno=" + oInfo.SysNo;
            }
            else if(oInfo.RetryCount == 2)  //24小时后再补发一次
            {
                sql = "update sms set retrycount=3,expectsendtime=DATEADD(hour, 21, getdate()) where sysno=" + oInfo.SysNo;
            }
            else if (oInfo.RetryCount == 3) //作废掉
            {
                sql = "update sms set status=" + (int)AppEnum.TriStatus.Abandon + " where sysno=" + oInfo.SysNo;
            }
            SqlHelper.ExecuteNonQuery(sql);
        }

        public DataSet GetUserSendMessage(Hashtable paramHash)
        {
            string sql = @"select sms.*,su.username from sms(nolock) inner join sys_user su(nolock) on sms.createusersysno=su.sysno where 1=1 ";
            StringBuilder sb = new StringBuilder();
            if (paramHash.Count > 0)
            {
                foreach (string key in paramHash.Keys)
                {
                    object item = paramHash[key];
                    sb.Append(" and ");
                    if (key == "datefrom")
                    {
                        sb.Append("sms.handletime").Append(">=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "dateto")
                    {
                        sb.Append("sms.handletime").Append("<=").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "usersysno")
                    {
                        sb.Append("sms.createusersysno").Append("=").Append(item.ToString());
                    }
                    else if (item is int)
                    {
                        sb.Append(key).Append("=").Append(item.ToString());
                    }
                    else if (item is string)
                    {
                        sb.Append(key).Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                    }
                }
            }
            sql += sb.ToString() + " order by handletime desc";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                return ds;
            else
                return null;
        }
	}
}