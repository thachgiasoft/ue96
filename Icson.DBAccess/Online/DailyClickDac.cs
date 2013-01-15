using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Text;

using Icson.Utils;
using Icson.Objects.Online;

namespace Icson.DBAccess.Online
{
	/// <summary>
	/// Summary description for DailyClickDac.
	/// </summary>
	public class DailyClickDac
	{
		
		public DailyClickDac()
		{
		}

		public int Insert(DailyClickInfo oParam)
		{
			string sql = @"INSERT INTO Product_DailyClick
                            (
                            ProductSysNo, ClickDate, ClickCount
                            )
                            VALUES (
                            @ProductSysNo, @ClickDate, @ClickCount
                            );set @SysNo = SCOPE_IDENTITY();";

			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int,4);
			SqlParameter paramClickDate = new SqlParameter("@ClickDate", SqlDbType.NVarChar,10);
			SqlParameter paramClickCount = new SqlParameter("@ClickCount", SqlDbType.Int,4);

			paramSysNo.Direction = ParameterDirection.Output;

			if ( oParam.ProductSysNo != AppConst.IntNull)
				paramProductSysNo.Value = oParam.ProductSysNo;
			else
				paramProductSysNo.Value = System.DBNull.Value;
			if ( oParam.ClickDate != AppConst.StringNull)
				paramClickDate.Value = oParam.ClickDate;
			else
				paramClickDate.Value = System.DBNull.Value;
			if ( oParam.ClickCount != AppConst.IntNull)
				paramClickCount.Value = oParam.ClickCount;
			else
				paramClickCount.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramProductSysNo);
			cmd.Parameters.Add(paramClickDate);
			cmd.Parameters.Add(paramClickCount);

			return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);

		}
		public int Click(DailyClickInfo oClick)
		{
			string sql = @"
					if exists( select top 1 * from product_dailyclick where productsysno=@productsysno and clickdate=@clickdate)
						update product_dailyclick set clickcount = clickcount + @clickcount where productsysno=@productsysno and clickdate=@clickdate
					else
						insert into product_dailyclick(productsysno, clickdate, clickcount) values(@productsysno, @clickdate, @clickcount)";
			sql = sql.Replace("@productsysno", oClick.ProductSysNo.ToString());
			sql = sql.Replace("@clickdate", Util.ToSqlString(oClick.ClickDate));
			sql = sql.Replace("@clickcount", oClick.ClickCount.ToString());

			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);

		}
	}
}
