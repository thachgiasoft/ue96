using System;
using System.Data;
using System.Collections;
using System.Text;
using Icson.Objects;
using Icson.Utils;
using System.Transactions;

using Icson.DBAccess;
using Icson.DBAccess.Online;

using Icson.Objects.Online;

namespace Icson.BLL.Online
{
	/// <summary>
	/// Summary description for DailyClickManager.
	/// </summary>
	public class DailyClickManager
	{
		private DailyClickManager()
		{
		}
		private static Hashtable ht = new Hashtable(1000);
		private static object locker = new object();

		private static DailyClickManager _instance;
		public static DailyClickManager GetInstance()
		{
			if( _instance == null )
			{
				_instance = new DailyClickManager();
			}
			return _instance;
		}
		public void Click(int productSysNo)
		{
			//说明参见biz.online.dailyClickManager.Count()
			lock ( locker )
			{
				if ( !ht.Contains(productSysNo))
				{
					//如果不包含这个produtSysNo，就生成一个。
					DailyClickInfo oClick = new DailyClickInfo();

					oClick.ClickDate = DateTime.Now.ToString(AppConst.DateFormat);
					oClick.ClickCount = 1;
					oClick.ProductSysNo = productSysNo;
					ht.Add(productSysNo, oClick);
				}
				else
				{
					DailyClickInfo oClick = ht[productSysNo] as DailyClickInfo;
					string nowTime = DateTime.Now.ToString(AppConst.DateFormat);

					//如果ht中的productSysNo不是今天的记录，就先更新昨天的记录数到数据库。并将今天这个productSysNo设置为1。
					if ( oClick.ClickDate != nowTime )
					{
						new DailyClickDac().Click(oClick);
						oClick.ClickCount = 1;
						oClick.ClickDate = nowTime;
					}
					else 
					{
						//如果是今天的,计数加一
						oClick.ClickCount++;
						//如果当前计数大于500，就更新到数据库。
						if ( oClick.ClickCount >= 5)
						{
							new DailyClickDac().Click(oClick);
							oClick.ClickCount = 0;
						}
					}
				}
			}
		}
		public void map(DailyClickInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.ProductSysNo = Util.TrimIntNull(tempdr["ProductSysNo"]);
			oParam.ClickDate = Util.TrimNull(tempdr["ClickDate"]);
			oParam.ClickCount = Util.TrimIntNull(tempdr["ClickCount"]);
		}

        public void UpdateProductDailyClick(string fromDate)
        {
            string sql = @"update product set AvgDailyClick = pdc.clicknum from product p 
                            inner join (
                            select p.sysno,
                            case
                                when datediff(day,p.createtime,getdate()) = 0 then sum(dc.clickcount) 
                                when p.createtime <  @fromDate then sum(dc.clickcount)/(datediff(day,@fromDate,getdate()))  
                                when p.createtime >= @fromDate then sum(dc.clickcount)/(datediff(day,p.createtime,getdate()))
                            end as clicknum
                            from product_dailyclick dc 
                            inner join product p on dc.productsysno = p.sysno 
                            where dc.clickdate >= @fromDate
                            group by p.sysno,p.createtime) as pdc on p.sysno=pdc.sysno and p.status=@status";
            sql = sql.Replace("@fromDate",Util.ToSqlString(fromDate));
            sql = sql.Replace("@status", ((int) AppEnum.ProductStatus.Show).ToString());
            SqlHelper.ExecuteNonQuery(sql);
        }

	    public void Import()
		{
			if ( !AppConfig.IsImportable)
				throw new BizException("Is Importable is false");

			/*  do not  use the following code after Data Pour in */
			string sql = " select top 1 * from product_dailyclick ";
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds) )
				throw new BizException("the table product_dailyclick is not empty");

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				//仅导3月份以来的数据
				string sql1 = @"select 
								pdc.sysno, con_product.newsysno as productsysno, countdate as clickdate, countnum as clickcount
							from
								ipp2003..product_daily_count as pdc,
								ippconvert..productbasic as con_product
							where 
								pdc.productsysno = con_product.oldsysno
							and convert(datetime, pdc.countdate, 120) >= convert(datetime, '2005-03-01', 120)";
				DataSet ds1 = SqlHelper.ExecuteDataSet(sql1);
				foreach(DataRow dr1 in ds1.Tables[0].Rows)
				{
					DailyClickInfo oInfo = new DailyClickInfo();
					map(oInfo,dr1);
					oInfo.ClickDate = Util.TrimDateNull(oInfo.ClickDate).ToString(AppConst.DateFormat);
					new DailyClickDac().Insert(oInfo);
				}
				
			scope.Complete();
            }
		}
	}
}
