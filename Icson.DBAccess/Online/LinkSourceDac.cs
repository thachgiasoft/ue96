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
	/// Summary description for LinkSourceDac.
	/// </summary>
	public class LinkSourceDac
	{
		
		public LinkSourceDac()
		{
		}
		public int Insert(LinkSourceInfo oParam)
		{
			string sql = @"INSERT INTO LinkSource
                            (
                            URLSource, VisitCount, CountDate
                            )
                            VALUES (
                            @URLSource, @VisitCount, @CountDate
                            );set @SysNo = SCOPE_IDENTITY();";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramURLSource = new SqlParameter("@URLSource", SqlDbType.NVarChar,300);
			SqlParameter paramVisitCount = new SqlParameter("@VisitCount", SqlDbType.Int,4);
			SqlParameter paramCountDate = new SqlParameter("@CountDate", SqlDbType.NVarChar,10);

			paramSysNo.Direction = ParameterDirection.Output;

			if ( oParam.URLSource != AppConst.StringNull)
				paramURLSource.Value = oParam.URLSource;
			else
				paramURLSource.Value = System.DBNull.Value;
			if ( oParam.VisitCount != AppConst.IntNull)
				paramVisitCount.Value = oParam.VisitCount;
			else
				paramVisitCount.Value = System.DBNull.Value;
			if ( oParam.CountDate != AppConst.StringNull)
				paramCountDate.Value = oParam.CountDate;
			else
				paramCountDate.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramURLSource);
			cmd.Parameters.Add(paramVisitCount);
			cmd.Parameters.Add(paramCountDate);

			return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);

		}

		public int Count(LinkSourceInfo oLink)
		{
			string sql = @"
					if exists( select top 1 * from linksource where urlsource=@urlsource and countdate=@countdate)
						update linksource set visitcount = visitcount + @visitcount where urlsource=@urlsource and countdate=@countdate
					else
						insert into linksource(urlsource, countdate, visitcount) values(@urlsource, @countdate, @visitcount)";
			sql = sql.Replace("@urlsource", Util.ToSqlString(oLink.URLSource));
			sql = sql.Replace("@countdate", Util.ToSqlString(oLink.CountDate));
			sql = sql.Replace("@visitcount",oLink.VisitCount.ToString());

			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);

		}
	}
}
