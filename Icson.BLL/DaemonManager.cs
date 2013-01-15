using System;
using System.Timers;
using System.Text;
using System.Collections;
using Icson.BLL.Online;
using Icson.Utils;

using Icson.Objects;
using Icson.Objects.Basic;
using Icson.BLL.Basic;
using Icson.BLL.Sale;
using Icson.BLL.Finance;

namespace Icson.BLL
{
	/// <summary>
	/// Summary description for DaemonManager.
	/// </summary>
	public class DaemonManager
	{
		private DaemonManager()
		{
		}
		private static object daemonLock = new object();
		private static DaemonManager _instance;
		public static DaemonManager GetInstance()
		{
			lock( daemonLock )
			{
				if ( _instance == null )
				{
					_instance = new DaemonManager();
					_instance.Go();
				}
			}
			return _instance;
		}

		private void Go()
		{
			if ( !AppConfig.IsDaemon)
				return;
			// 每4个小时汇报是否在工作
			System.Timers.Timer oTimer = new System.Timers.Timer(4*60*60*1000);
			oTimer.Elapsed+=new ElapsedEventHandler(AmWork);
			// Only raise the event the first time Interval elapses.
			oTimer.AutoReset = true;
			oTimer.Enabled = true;

            // 每1小时检查一次导入PM帐期
            System.Timers.Timer pmTimer = new Timer(1*60*60*1000);
            pmTimer.Elapsed += new ElapsedEventHandler(ImportPMPayment);
            pmTimer.AutoReset = true;
            pmTimer.Enabled = true;

			// 每5分钟同步一次 -------------------------------------------------------------------------------
			System.Timers.Timer aTimer = new System.Timers.Timer(5*60*1000);
			aTimer.Elapsed+=new ElapsedEventHandler(DoSync);
			// Only raise the event the first time Interval elapses.
			aTimer.AutoReset = true;
			aTimer.Enabled = true;

			if ( !AppConfig.IsIAS)
				return;

			// 每30分钟检查是否在更新时段内 - 购物送积分
			System.Timers.Timer bTimer = new System.Timers.Timer(30*60*1000);
			bTimer.Elapsed+=new ElapsedEventHandler(DoPointDelay);
			// Only raise the event the first time Interval elapses.
			bTimer.AutoReset = true;
			bTimer.Enabled = true;

			// 每30分钟检查是否在更新时段内 - 销售趋势
			System.Timers.Timer cTimer = new System.Timers.Timer(30*60*1000);
			cTimer.Elapsed+=new ElapsedEventHandler(DoProductSaleTrend);
			// Only raise the event the first time Interval elapses.
			cTimer.AutoReset = true;
			cTimer.Enabled = true;

			// 每30秒钟检索待发邮件发送
			System.Timers.Timer eTimer = new System.Timers.Timer(1*30*1000);
			eTimer.Elapsed+=new ElapsedEventHandler(SendAsyncEmail);
			// Only raise the event the first time Interval elapses.
			eTimer.AutoReset = true;
			eTimer.Enabled = true;

            //限时抢购
            //每5分钟检查是否在更新时段内
            System.Timers.Timer hTimer = new System.Timers.Timer(5 * 60 * 1000);
            hTimer.Elapsed += new ElapsedEventHandler(CountdownJob);
            // Only raise the event the first time Interval elapses.
            hTimer.AutoReset = true;
            hTimer.Enabled = true;

            //每n分钟增加一系列订单空号
            //System.Timers.Timer sTimer = new System.Timers.Timer(AppConfig.AutoCreateSOTimer * 60 * 1000);
            //sTimer.Elapsed += new ElapsedEventHandler(AutoCreateNullSO);
            //// Only raise the event the first time Interval elapses.
            //sTimer.AutoReset = true;
            //sTimer.Enabled = true;

            //商品点击率
            //每小时更新一次
            System.Timers.Timer dTimer = new System.Timers.Timer(60 * 60 * 1000);
            dTimer.Elapsed += new ElapsedEventHandler(UpdateProductDailyClick);
            // Only raise the event the first time Interval elapses.
            dTimer.AutoReset = true;
            dTimer.Enabled = true;

            //每小时检查一次更新二手商品价格，每天降1%（每天发生在0点到1点之间）
            System.Timers.Timer shTimer = new System.Timers.Timer(60 * 60 * 1000);
            shTimer.Elapsed += new ElapsedEventHandler(UpdateSecondHandProductPrice);
            // Only raise the event the first time Interval elapses.
            shTimer.AutoReset = true;
            shTimer.Enabled = true;

            // 每30秒钟检索待发手机短信
            System.Timers.Timer smsTimer = new System.Timers.Timer(1 * 30 * 1000);
            smsTimer.Elapsed += new ElapsedEventHandler(SendSMS);
            // Only raise the event the first time Interval elapses.
            smsTimer.AutoReset = true;
            smsTimer.Enabled = true;
		}

        /// <summary>
        /// 22:00 - 23:59之间执行一次
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ImportPMPayment(object sender, ElapsedEventArgs e)
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime today23hour = Convert.ToDateTime(today + " 22:00:00");
            DateTime today24hour = Convert.ToDateTime(today + " 23:59:59");

            if (today23hour <= DateTime.Now && DateTime.Now <= today24hour)
            {                
                if (!PMPaymentManager.GetInstance().IsHasImportedPMPayment(today))
                {
                    PMPaymentManager.GetInstance().ImportPMPayment(today);
                }
            }
        }

		public void DoSync(object source, ElapsedEventArgs e)
		{
			SyncManager.GetInstance().Run();
			
		}
		public void DoPointDelay(object source, ElapsedEventArgs e)
		{
			if ( DateTime.Now.Hour >= 3 && DateTime.Now.Hour < 4 )
			{
				PointManager.GetInstance().DoDelayPoint();
			}
		}
		public void DoProductSaleTrend(object source, ElapsedEventArgs e)
		{
            //if ( DateTime.Now.Hour >= 22 && DateTime.Now.Hour < 23)
            if (DateTime.Now.Hour >= 9 && DateTime.Now.Hour < 10)
			{
				ProductSaleTrendManager.GetInstance().UpToDate();

				TCPMail oMail = new TCPMail();
				string mailHead = "更新销售趋势---" + DateTime.Now.ToString(AppConst.DateFormatLong);
				if ( oMail.Send(AppConfig.AdminEmail, mailHead, "OK") )
				{
				}
				else
				{
					ErrorLog.GetInstance().Write("发送email失败:" + mailHead);
				}
			}
		}
		public void CheckUserPoint()
		{
			if( DateTime.Now.Hour>=5&&DateTime.Now.Hour<6)
			{
				PointManager.GetInstance().CheckUserScore();
			}
		}
		public void AmWork(object source, ElapsedEventArgs e)
		{
			TCPMail oMail = new TCPMail();
			string mailHead =  "ICSON Daemon Running---" + DateTime.Now.ToString(AppConst.DateFormatLong);
			if ( oMail.Send(AppConfig.AdminEmail, mailHead, "OK") )
			{
			}
			else
			{
				ErrorLog.GetInstance().Write("发送email失败:" + mailHead);
			}			
		}

		public void SendAsyncEmail(object source, ElapsedEventArgs e)
		{
			Hashtable emailHash = EmailManager.GetInstance().SearchAsyncEmails();
			if(emailHash.Count>0)
			{
				StringBuilder sb = new StringBuilder();
				foreach(EmailInfo oInfo in emailHash.Keys)
				{
					if(Util.SendMail(oInfo.MailAddress,oInfo.MailSubject,oInfo.MailBody))
					{
						oInfo.Status = (int)AppEnum.TriStatus.Handled;
						EmailManager.GetInstance().UpdateEmailStatus(oInfo);
					}
					else
					{
						sb.Append(oInfo.SysNo+",");
					}
				}
				if(sb.Length>0)
				{
					ErrorLog.GetInstance().Write("发送AsyncEmail失败："+sb.ToString().Trim(','));
				}
			}
		}

        public void CountdownJob(object source, ElapsedEventArgs e)
        {
            CountdownManager.GetInstance().CountdownJob();
        }

        public void AutoCreateNullSO(object source, ElapsedEventArgs e)
        {
            SaleManager.GetInstance().AutoCreateNullSO();
        }

        public void UpdateProductDailyClick(object source,ElapsedEventArgs e)
        {
            DailyClickManager.GetInstance().UpdateProductDailyClick(DateTime.Now.AddMonths(-2).ToString(AppConst.DateFormat));
        }

        public void UpdateSecondHandProductPrice(object sender, ElapsedEventArgs e)
        {
            if (!AppConfig.IsAutoUpdateSecondHandProductPrice)
            {
                return;
            }
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime today0 = Convert.ToDateTime(today + " 01:00:00");
            DateTime today1 = Convert.ToDateTime(today + " 02:00:00");

            if (today0 <= DateTime.Now && DateTime.Now < today1)
            {
                ProductManager.GetInstance().AutoUpdateSecondHandProductPrice();
            }
        }

        public void SendSMS(object source, ElapsedEventArgs e)
        {
            if (!AppConfig.IsSendSMS)
            {
                return;
            }

            Hashtable smsHash = SMSManager.GetInstance().SearchAsyncSMS();
            if (smsHash.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (SMSInfo oInfo in smsHash.Keys)
                {
                    if (Util.SendSMSMessage(oInfo.CellNumber,oInfo.SMSContent) == 1)
                    {
                        oInfo.Status = (int)AppEnum.TriStatus.Handled;
                        oInfo.HandleTime = DateTime.Now;
                        SMSManager.GetInstance().UpdateSMSStatus(oInfo);
                    }
                    else
                    {
                        sb.Append(oInfo.SysNo + ",");
                        SMSManager.GetInstance().UpdateSMSRetryCount(oInfo);
                    }
                }
                if (sb.Length > 0)
                {
                    ErrorLog.GetInstance().Write("发送SMS失败：" + sb.ToString().Trim(','));
                }
            }
        }
	}
}