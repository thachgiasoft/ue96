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
	/// Summary description for ProductRemarkManager.
	/// </summary>
	public class ProductRemarkManager
	{
		private ProductRemarkManager()
		{
		}
		private static ProductRemarkManager _instance;
		public static ProductRemarkManager GetInstance()
		{
			if( _instance == null )
			{
				_instance = new ProductRemarkManager();
			}
			return _instance;
		}

		public void map(ProductRemarkInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.CustomerSysNo = Util.TrimIntNull(tempdr["CustomerSysNo"]);
			oParam.ProductSysNo = Util.TrimIntNull(tempdr["ProductSysNo"]);
			oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
			oParam.Title = Util.TrimNull(tempdr["Title"]);
			oParam.Remark = Util.TrimNull(tempdr["Remark"]);;
			oParam.Score = Util.TrimIntNull(tempdr["Score"]);
			oParam.Status = Util.TrimIntNull(tempdr["Status"]);
		    oParam.OptIP = Util.TrimNull(tempdr["OptIP"]);
		}

		public void Insert(ProductRemarkInfo oParam)
		{
			new ProductRemarkDac().Insert(oParam);
		}
		public void Delete(int remarkSysNo)
		{
			Hashtable ht = new Hashtable(5);
			ht.Add("SysNo", remarkSysNo);
			ht.Add("Status", (int)AppEnum.BiStatus.InValid);
			new ProductRemarkDac().Update(ht);
		}

		public void GetRemarkStat(out int countTotal, out int scoreTotal, int productSysNo)
		{
			string sql = @"SELECT 
							Count(*) as countTotal, Isnull(Sum(Score),0) as scoreTotal
						 FROM 
							Product_Remark
						WHERE
							productSysNo = @productSysNo
						and status = @status";
			sql = sql.Replace("@productSysNo", productSysNo.ToString());
			sql = sql.Replace("@status", ((int)AppEnum.BiStatus.Valid).ToString());

			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			countTotal = Util.TrimIntNull(ds.Tables[0].Rows[0]["countTotal"]);
			scoreTotal = Util.TrimIntNull(ds.Tables[0].Rows[0]["scoreTotal"]);
			
		}

		public DataSet GetRemarkList(int productSysNo, int currentPage, int pageSize)
		{
			string sql = @"select
							product_remark.*, customer.customerid
						from 
							product_remark, customer
						where 
							product_remark.customersysno = customer.sysno
						and product_remark.productsysno = @productSysNo
						and product_remark.status = @status
						order by product_remark.sysno desc";

			sql = sql.Replace("@productSysNo", productSysNo.ToString());
			sql = sql.Replace("@status", ((int)AppEnum.BiStatus.Valid).ToString());

			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( !Util.HasMoreRow(ds))
				return ds;

			int i=0;
			DataSet ds2 = ds.Clone();
			foreach(DataRow dr in ds.Tables[0].Rows)
			{
				if ( i>= currentPage*pageSize && i < (currentPage+1)*pageSize )
					ds2.Tables[0].ImportRow(dr);
				i++;
				if ( i >= (currentPage+1)*pageSize)
					break;
			}
			return ds2;
		}

		public void Import()
		{
			if ( !AppConfig.IsImportable)
				throw new BizException("Is Importable is false");

			/*  do not  use the following code after Data Pour in */
			string sql = " select top 1 * from product_remark ";
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds) )
				throw new BizException("the table product_remark is not empty");

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				string sql1 = @"select 
									1 as sysno, WebUserSysNo as CustomerSysNo,
									convert_1.newsysno as ProductSysNo,
									createtime, title, isnull(remark, '') as remark, score, status 
								from 
									ipp2003..product_remark as pr, ippconvert..productbasic convert_1
								where 
									pr.productsysno = convert_1.oldsysno";
				DataSet ds1 = SqlHelper.ExecuteDataSet(sql1);
				foreach(DataRow dr1 in ds1.Tables[0].Rows)
				{
					ProductRemarkInfo oRemark = new ProductRemarkInfo();
					map(oRemark,dr1);
					if ( oRemark.Remark == "")
						oRemark.Remark = "нч";
					if ( oRemark.Title == "")
						oRemark.Title = "нч";
					new ProductRemarkDac().Insert(oRemark);
				}
				
				
			scope.Complete();
            }
		}

        public string GetPMEmailByProduct(int productSysNo)
        {
            ProductBasicInfo oProductBasic = ProductManager.GetInstance().LoadBasic(productSysNo);
            if (oProductBasic != null)
            {
                string sql = "select Email from Sys_User where SysNo = " + oProductBasic.PMUserSysNo;
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (!Util.HasMoreRow(ds))
                    return AppConfig.ReviewBox;
                DataRow dr = ds.Tables[0].Rows[0];
                return dr["Email"].ToString().Trim();
            }
            else
                return AppConfig.ReviewBox;
        }
	}
}
