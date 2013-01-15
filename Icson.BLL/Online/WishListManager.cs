using System;
using System.Data;
using System.Collections;
using System.Text;

using Icson.Utils;
using System.Transactions;

using Icson.DBAccess;
using Icson.DBAccess.Online;

using Icson.Objects.Online;

namespace Icson.BLL.Online
{
	/// <summary>
	/// Summary description for WishListManager.
	/// </summary>
	public class WishListManager
	{
		private WishListManager()
		{
		}
		private static WishListManager _instance;
		public static WishListManager GetInstance()
		{
			if( _instance == null )
			{
				_instance = new WishListManager();
			}
			return _instance;
		}

		public void Insert(WishListInfo oParam)
		{
			//如果已经存在，就不添加了
			string sql = "select * from wishlist where customersysno=" + oParam.CustomerSysNo + " and productSysNo=" + oParam.ProductSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds))
				return;
			new WishListDac().Insert(oParam);
		}

		public void Delete(int customerSysNo, int productSysNo)
		{
			new WishListDac().Delete(customerSysNo, productSysNo);
		}

		public void Clear(int customerSysNo)
		{
			new WishListDac().Clear(customerSysNo);
		}

		private void map(WishListInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.CustomerSysNo = Util.TrimIntNull(tempdr["CustomerSysNo"]);
			oParam.ProductSysNo = Util.TrimIntNull(tempdr["ProductSysNo"]);
			oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
		}

		public void Import()
		{
			if ( !AppConfig.IsImportable)
				throw new BizException("Is Importable is false");

			/*  do not  use the following code after Data Pour in */
			string sql = " select top 1 * from wishlist ";
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds) )
				throw new BizException("the table wishlist is not empty");

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				string sql1 = @"select
								1 as sysno, wl.webusersysno as customersysno, con_product.newsysno as productsysno, null as createtime
							from
								ipp2003..myfavorite wl, ippconvert..productbasic con_product
							where
								wl.productsysno = con_product.oldsysno";
				DataSet ds1 = SqlHelper.ExecuteDataSet(sql1);
				foreach(DataRow dr1 in ds1.Tables[0].Rows)
				{
					WishListInfo oWishList = new WishListInfo();
					map(oWishList,dr1);
					new WishListDac().Insert(oWishList);
				}
				
				
			scope.Complete();
            }
		}
	}
}
