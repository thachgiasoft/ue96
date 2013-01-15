using System;
using System.Data;
using System.Collections;
using System.Text;

using Icson.Utils;
using System.Transactions;

using Icson.DBAccess;
using Icson.DBAccess.Online;
using Icson.Objects;
using Icson.Objects.Basic;
using Icson.Objects.Online;

using Icson.BLL.Basic;


namespace Icson.BLL.Online
{
	/// <summary>
	/// Summary description for ProductNotifyManager.
	/// </summary>
	public class ProductNotifyManager
	{
		private ProductNotifyManager()
		{
		}
		private static ProductNotifyManager _instance;
		public static ProductNotifyManager GetInstance()
		{
			if( _instance == null )
			{
				_instance = new ProductNotifyManager();
			}
			return _instance;
		}
		public void Insert(ProductNotifyInfo oParam)
		{

			//如果存在相同的email和productsysno，就提醒登录
			string sql = "select top 1 sysno from product_notify where productsysno = " + oParam.ProductSysNo + " and email=" + Util.ToSqlString(oParam.Email);
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds))
			{
				throw new BizException("已经有相同email的重复记录，如果希望管理您的到货通知，请登录");
			}
			new ProductNotifyDac().Insert(oParam);
		}
		public void Update(Hashtable ht)
		{
			new ProductNotifyDac().Update(ht);
		}

		private void map(ProductNotifyInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.CustomerSysNo = Util.TrimIntNull(tempdr["CustomerSysNo"]);
			oParam.ProductSysNo = Util.TrimIntNull(tempdr["ProductSysNo"]);
			oParam.Email = Util.TrimNull(tempdr["Email"]);
			oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
			oParam.NotifyTime = Util.TrimDateNull(tempdr["NotifyTime"]);
			oParam.Status = Util.TrimIntNull(tempdr["Status"]);
		}

		public void Delete(int notifySysNo)
		{
			new ProductNotifyDac().Delete(notifySysNo);
		}

		public void Clear(int customerSysNo)
		{
			new ProductNotifyDac().Clear(customerSysNo);
		}

		public DataSet GetNotifyDs(Hashtable paramHash)
		{
			string sql = @" select
							customername, productname, product_notify.status,
							product_notify.email, product_notify.createTime, product_notify.notifyTime
						from 
							product_notify, customer, product
						where
							product_notify.customersysno = customer.sysno
						and product_notify.productsysno = product.sysno";

			if ( paramHash != null && paramHash.Count != 0 )
			{
				StringBuilder sb = new StringBuilder(100);
				foreach(string key in paramHash.Keys)
				{
					sb.Append(" and ");
					object item = paramHash[key];

					if ( key == "Email")
					{
						sb.Append("Product_Notify.Email like ").Append(Util.ToSqlLikeString(item.ToString()));
					}
					else if ( key == "Status")
					{
						sb.Append("Product_Notify.Status =").Append(item.ToString());
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

			sql += " order by product_notify.createtime desc";
			return SqlHelper.ExecuteDataSet(sql);
		}

		public DataSet GetNotifyDsSummary(Hashtable paramHash)
		{
			string sql = @" select a.sysno,a.productid,a.productname,count(a.sysno) as itemcount,b.status,c.sysno as pmusersysno,c.username,min(b.createtime) as mincreatetime 
							from product a,product_notify b,sys_user c 
							where a.status='0' and a.sysno=b.productsysno and a.pmusersysno=c.sysno ";

			if ( paramHash != null && paramHash.Count != 0 )
			{
				StringBuilder sb = new StringBuilder(100);
				foreach(string key in paramHash.Keys)
				{
					sb.Append(" and ");
					object item = paramHash[key];

					if(key=="StartDate")
					{
						sb.Append("b.createtime").Append(">=").Append(Util.ToSqlString(item.ToString()));
					}
					else if(key=="EndDate")
					{
						sb.Append("b.createtime").Append("<=").Append(Util.ToSqlEndDate(item.ToString()));
					}
					else if ( key == "Status")
					{
						sb.Append("b.status =").Append(item.ToString());
					}
					else if(key=="pmusersysno")
					{
						sb.Append("c.sysno").Append("=").Append(item.ToString());
					}
				}
				sql += sb.ToString();
			}
			else
			{
				sql = sql.Replace("select", "select top 50");
			}

			sql += "group by a.sysno,a.productid,a.productname,b.status,c.sysno,c.username";

			sql += " order by count(a.sysno) desc";
			return SqlHelper.ExecuteDataSet(sql);
		}

		public void DoNotify(int poSysNo)
		{
			// 以后增加90天删除的功能。
			string sql = @"select 
							product.sysno, productname
						from
							po_item, inventory, product, product_price
						where
							product.sysno = po_item.productsysno
						and product.sysno = inventory.productsysno
						and product.sysno = product_price.productsysno
						and availableqty+virtualqty>0
						@onlineShowLimit
						and po_item.posysno = @poSysNo";
			sql = sql.Replace("@onlineShowLimit", OnlineListManager.GetInstance().onlineShowLimit);
			sql = sql.Replace("@poSysNo", poSysNo.ToString());
			DataSet ds = SqlHelper.ExecuteDataSet(sql);

			if ( !Util.HasMoreRow( ds))
				return;

			
			foreach(DataRow dr in ds.Tables[0].Rows)
			{
				int productSysNo = Util.TrimIntNull(dr["sysno"]);
				string productName = Util.TrimNull(dr["productname"]);
				string sql2 = @"select 
									* 
								from product_notify where productsysno =" + productSysNo.ToString() + " and status=" + (int)AppEnum.ProductNotifyStatus.UnNotify;
				DataSet ds2 = SqlHelper.ExecuteDataSet(sql2);
				if ( !Util.HasMoreRow(ds2))
					continue;

				string mailBody = @"您好！<br>您关注的商品@productlink已经到货，请及时到ORS商城网订购。<br>如果您查看的时候，该商品又处于缺货
								状态，您可以选择该商品，点击重新提醒。<br>请更新您的到货通知列表，以确保您能得到及时，准确地信息。
								<br>所有到货通知，自生成或最后更新日起，保留90天。";
				string mailSubject = "ORS商城网到货通知-@productname";

				mailBody = mailBody.Replace("@productlink", "<a href='http://www.baby1one.com.cn/Items/ItemDetail.aspx?ItemID=" + productSysNo.ToString() + "'>" + productName + "</a>");
				mailSubject = mailSubject.Replace("@productname", productName);
				Hashtable ht = new Hashtable(2);
				foreach(DataRow dr2 in ds2.Tables[0].Rows)
				{
					ProductNotifyInfo o = new ProductNotifyInfo();
					map(o, dr2);
					EmailInfo oEmail = new EmailInfo(o.Email, mailSubject, mailBody);
					EmailManager.GetInstance().InsertEmail(oEmail);

					ht.Clear();
					ht.Add("SysNo", o.SysNo);
					ht.Add("NotifyTime", DateTime.Now);
					ht.Add("Status", (int)AppEnum.ProductNotifyStatus.Notified);

					this.Update(ht);
				}

			}

		}

		public void Import()
		{
			if ( !AppConfig.IsImportable)
				throw new BizException("Is Importable is false");

			/*  do not  use the following code after Data Pour in */
			string sql = " select top 1 * from product_notify ";
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds) )
				throw new BizException("the table product_notify is not empty");

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				string sql1 = @"select
								1 as sysno, null as notifytime,
								wp.webusersysno as customersysno, 
								con_product.newsysno as productsysno,
								notifytool as email, createtime,
								status
							from 
								ipp2003..wish_product as wp,
								ippconvert..productbasic as con_product
							where 
								wp.productsysno = con_product.oldsysno";
				DataSet ds1 = SqlHelper.ExecuteDataSet(sql1);
				/*
				 *	<option value="-1">Abandon</option>
					<option value="0">UnNotify</option>
					<option value="1">Notified</option>
				 * 
				 */
				foreach(DataRow dr1 in ds1.Tables[0].Rows)
				{
					ProductNotifyInfo oInfo = new ProductNotifyInfo();
					map(oInfo,dr1);
					new ProductNotifyDac().Insert(oInfo);
				}
				
				
			scope.Complete();
            }
		}
		
	}
}
