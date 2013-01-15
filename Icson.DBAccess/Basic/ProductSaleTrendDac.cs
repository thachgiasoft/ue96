using System;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;

namespace Icson.DBAccess.Basic
{
	/// <summary>
	/// Summary description for ProductSaleTrendDac.
	/// </summary>
	public class ProductSaleTrendDac
	{
		

		public ProductSaleTrendDac()
		{
		}

		public int Exec(string sql)
		{
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);
		}
	}
}
