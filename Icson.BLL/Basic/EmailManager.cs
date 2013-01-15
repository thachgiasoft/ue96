using System;
using System.Data;
using System.Text;
using System.Collections;

using Icson.Utils;
using Icson.Objects.Basic;
using Icson.DBAccess;
using Icson.DBAccess.Basic;
using Icson.BLL;

namespace Icson.BLL.Basic
{
	/// <summary>
	/// Summary description for EmailManager.
	/// </summary>
	public class EmailManager
	{
		public EmailManager()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		private static EmailManager _instance;
		public static EmailManager GetInstance()
		{
			if ( _instance == null )
			{
				_instance = new EmailManager();
			}
			return _instance;
		}

		private void map(EmailInfo oParam,DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.MailAddress = Util.TrimNull(tempdr["MailAddress"]);
			oParam.MailSubject = Util.TrimNull(tempdr["MailSubject"]);
			oParam.MailBody = Util.TrimNull(tempdr["MailBody"]);
			oParam.Status = Util.TrimIntNull(tempdr["Status"]);
		}

		public void InsertEmail(EmailInfo oParam)
		{
			new EmailDac().InsertEmail(oParam);
		}

		/// <summary>
		/// Search for the emails to be send
		/// </summary>
		/// <returns></returns>
		public Hashtable SearchAsyncEmails()
		{
			string sql = @"select * 
							from asyncemail
							where status = 0";
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			Hashtable emailHash = new Hashtable(5);
			if(Util.HasMoreRow(ds))
			{
				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					EmailInfo oInfo = new EmailInfo();
					this.map(oInfo,dr);
					emailHash.Add(oInfo,null);
				}
			}
			return emailHash;
		}

		public void UpdateEmailStatus(EmailInfo oInfo)
		{
			new EmailDac().UpdateEmailStatus(oInfo);
		}
	}
}
