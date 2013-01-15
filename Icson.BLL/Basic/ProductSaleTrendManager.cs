using System;

using Icson.Utils;
using System.Transactions;
using Icson.Objects;
using Icson.DBAccess.Basic;

namespace Icson.BLL.Basic
{
	/// <summary>
	/// Summary description for ProductSaleTrend.
	/// </summary>
	public class ProductSaleTrendManager
	{
		private ProductSaleTrendManager()
		{
		}
		private static ProductSaleTrendManager _instance;
		public static ProductSaleTrendManager GetInstance()
		{
			if ( _instance == null )
			{
				_instance = new ProductSaleTrendManager();
			}
			return _instance;
		}
		public void UpToDate()
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				new ProductSaleTrendDac().Exec("delete from product_saletrend");
				new ProductSaleTrendDac().Exec(BuildInsertSql());

				scope.Complete();
            }

		}

		private string BuildInsertSql()
		{
			//f: from t: to
			DateTime d1f,d1t, d2f,d2t, d3f,d3t, d4f,d4t, d5f,d5t, d6f,d6t, d7f,d7t;
			DateTime w1f,w1t, w2f,w2t, w3f,w3t, w4f,w4t, w5f,w5t, w6f,w6t, w7f,w7t;
			DateTime m1f,m1t, m2f,m2t, m3f,m3t, m4f,m4t, m5f,m5t, m6f,m6t;

			d1t = DateTime.Today;
			//d1t = new DateTime(2003,12,30);
			d1f = d2t = d1t.AddDays(-1);
			d2f = d3t = d2t.AddDays(-1);
			d3f = d4t = d3t.AddDays(-1);
			d4f = d5t = d4t.AddDays(-1);
			d5f = d6t = d5t.AddDays(-1);
			d6f = d7t = d6t.AddDays(-1);
			d7f = d7t.AddDays(-1);

			w1t = d1t;
			w1f = w2t = w1t.AddDays(-7);
			w2f = w3t = w2t.AddDays(-7);
			w3f = w4t = w3t.AddDays(-7);
			w4f = w5t = w4t.AddDays(-7);
			w5f = w6t = w5t.AddDays(-7);
			w6f = w7t = w6t.AddDays(-7);
			w7f = w7t.AddDays(-7);
			
			m1t = d1t;
			m1f = m2t = m1t.AddMonths(-1);
			m2f = m3t = m2t.AddMonths(-1);
			m3f = m4t = m3t.AddMonths(-1);
			m4f = m5t = m4t.AddMonths(-1);
			m5f = m6t = m5t.AddMonths(-1);
			m6f = m6t.AddMonths(-1);


			string sql = @"INSERT INTO Product_SaleTrend
							(ProductSysNo, d1, d2, d3, d4, d5, d6, d7, w1, w2, w3, w4, w5, w6, w7, m1, m2, m3, m4, m5, m6)
								select si.productsysno,
									sum(case when sm.OutTime between @d1f and @d1t then si.quantity else 0 end ) as d1,
									sum(case when sm.OutTime between @d2f and @d2t then si.quantity else 0 end ) as d2,
									sum(case when sm.OutTime between @d3f and @d3t then si.quantity else 0 end ) as d3,
									sum(case when sm.OutTime between @d4f and @d4t then si.quantity else 0 end ) as d4,
									sum(case when sm.OutTime between @d5f and @d5t then si.quantity else 0 end ) as d5,
									sum(case when sm.OutTime between @d6f and @d6t then si.quantity else 0 end ) as d6,
									sum(case when sm.OutTime between @d7f and @d7t then si.quantity else 0 end ) as d7,

									sum(case when sm.OutTime between @w1f and @w1t then si.quantity else 0 end ) as w1,
									sum(case when sm.OutTime between @w2f and @w2t then si.quantity else 0 end ) as w2,
									sum(case when sm.OutTime between @w3f and @w3t then si.quantity else 0 end ) as w3,
									sum(case when sm.OutTime between @w4f and @w4t then si.quantity else 0 end ) as w4,
									sum(case when sm.OutTime between @w5f and @w5t then si.quantity else 0 end ) as w5,
									sum(case when sm.OutTime between @w6f and @w6t then si.quantity else 0 end ) as w6,
									sum(case when sm.OutTime between @w7f and @w7t then si.quantity else 0 end ) as w7,

									sum(case when sm.OutTime between @m1f and @m1t then si.quantity else 0 end ) as m1,
									sum(case when sm.OutTime between @m2f and @m2t then si.quantity else 0 end ) as m2,
									sum(case when sm.OutTime between @m3f and @m3t then si.quantity else 0 end ) as m3,
									sum(case when sm.OutTime between @m4f and @m4t then si.quantity else 0 end ) as m4,
									sum(case when sm.OutTime between @m5f and @m5t then si.quantity else 0 end ) as m5,
									sum(case when sm.OutTime between @m6f and @m6t then si.quantity else 0 end ) as m6
								from so_master as sm, so_item as si 
									where 
									sm.sysno = si.sosysno and
									(sm.OutTime between @m6f and @d1t) and
									sm.status=@status
								group by si.productsysno
							";
			sql = sql.Replace("@d1f", "Cast('" + d1f.ToString(AppConst.DateFormatLong) + "' as datetime)");
			sql = sql.Replace("@d2f", "Cast('" + d2f.ToString(AppConst.DateFormatLong) + "' as datetime)");
			sql = sql.Replace("@d3f", "Cast('" + d3f.ToString(AppConst.DateFormatLong) + "' as datetime)");
			sql = sql.Replace("@d4f", "Cast('" + d4f.ToString(AppConst.DateFormatLong) + "' as datetime)");
			sql = sql.Replace("@d5f", "Cast('" + d5f.ToString(AppConst.DateFormatLong) + "' as datetime)");
			sql = sql.Replace("@d6f", "Cast('" + d6f.ToString(AppConst.DateFormatLong) + "' as datetime)");
			sql = sql.Replace("@d7f", "Cast('" + d7f.ToString(AppConst.DateFormatLong) + "' as datetime)");

			sql = sql.Replace("@d1t", "Cast('" + d1t.ToString(AppConst.DateFormatLong) + "' as datetime)");
			sql = sql.Replace("@d2t", "Cast('" + d2t.ToString(AppConst.DateFormatLong) + "' as datetime)");
			sql = sql.Replace("@d3t", "Cast('" + d3t.ToString(AppConst.DateFormatLong) + "' as datetime)");
			sql = sql.Replace("@d4t", "Cast('" + d4t.ToString(AppConst.DateFormatLong) + "' as datetime)");
			sql = sql.Replace("@d5t", "Cast('" + d5t.ToString(AppConst.DateFormatLong) + "' as datetime)");
			sql = sql.Replace("@d6t", "Cast('" + d6t.ToString(AppConst.DateFormatLong) + "' as datetime)");
			sql = sql.Replace("@d7t", "Cast('" + d7t.ToString(AppConst.DateFormatLong) + "' as datetime)");

			sql = sql.Replace("@w1f", "Cast('" + w1f.ToString(AppConst.DateFormatLong) + "' as datetime)");
			sql = sql.Replace("@w2f", "Cast('" + w2f.ToString(AppConst.DateFormatLong) + "' as datetime)");
			sql = sql.Replace("@w3f", "Cast('" + w3f.ToString(AppConst.DateFormatLong) + "' as datetime)");
			sql = sql.Replace("@w4f", "Cast('" + w4f.ToString(AppConst.DateFormatLong) + "' as datetime)");
			sql = sql.Replace("@w5f", "Cast('" + w5f.ToString(AppConst.DateFormatLong) + "' as datetime)");
			sql = sql.Replace("@w6f", "Cast('" + w6f.ToString(AppConst.DateFormatLong) + "' as datetime)");
			sql = sql.Replace("@w7f", "Cast('" + w7f.ToString(AppConst.DateFormatLong) + "' as datetime)");

			sql = sql.Replace("@w1t", "Cast('" + w1t.ToString(AppConst.DateFormatLong) + "' as datetime)");
			sql = sql.Replace("@w2t", "Cast('" + w2t.ToString(AppConst.DateFormatLong) + "' as datetime)");
			sql = sql.Replace("@w3t", "Cast('" + w3t.ToString(AppConst.DateFormatLong) + "' as datetime)");
			sql = sql.Replace("@w4t", "Cast('" + w4t.ToString(AppConst.DateFormatLong) + "' as datetime)");
			sql = sql.Replace("@w5t", "Cast('" + w5t.ToString(AppConst.DateFormatLong) + "' as datetime)");
			sql = sql.Replace("@w6t", "Cast('" + w6t.ToString(AppConst.DateFormatLong) + "' as datetime)");
			sql = sql.Replace("@w7t", "Cast('" + w7t.ToString(AppConst.DateFormatLong) + "' as datetime)");


			sql = sql.Replace("@m1f", "Cast('" + m1f.ToString(AppConst.DateFormatLong) + "' as datetime)");
			sql = sql.Replace("@m2f", "Cast('" + m2f.ToString(AppConst.DateFormatLong) + "' as datetime)");
			sql = sql.Replace("@m3f", "Cast('" + m3f.ToString(AppConst.DateFormatLong) + "' as datetime)");
			sql = sql.Replace("@m4f", "Cast('" + m4f.ToString(AppConst.DateFormatLong) + "' as datetime)");
			sql = sql.Replace("@m5f", "Cast('" + m5f.ToString(AppConst.DateFormatLong) + "' as datetime)");
			sql = sql.Replace("@m6f", "Cast('" + m6f.ToString(AppConst.DateFormatLong) + "' as datetime)");

			sql = sql.Replace("@m1t", "Cast('" + m1t.ToString(AppConst.DateFormatLong) + "' as datetime)");
			sql = sql.Replace("@m2t", "Cast('" + m2t.ToString(AppConst.DateFormatLong) + "' as datetime)");
			sql = sql.Replace("@m3t", "Cast('" + m3t.ToString(AppConst.DateFormatLong) + "' as datetime)");
			sql = sql.Replace("@m4t", "Cast('" + m4t.ToString(AppConst.DateFormatLong) + "' as datetime)");
			sql = sql.Replace("@m5t", "Cast('" + m5t.ToString(AppConst.DateFormatLong) + "' as datetime)");
			sql = sql.Replace("@m6t", "Cast('" + m6t.ToString(AppConst.DateFormatLong) + "' as datetime)");

			sql = sql.Replace("@status", ((int)AppEnum.SOStatus.OutStock).ToString());

			return sql;
		}
	}
}
