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
	/// Summary description for WishListDac.
	/// </summary>
	public class WishListDac
	{
		
		public WishListDac()
		{
		}

		public int Insert(WishListInfo oParam)
		{
			string sql = @"INSERT INTO WishList
                            (
                            CustomerSysNo, ProductSysNo, CreateTime
                            )
                            VALUES (
                            @CustomerSysNo, @ProductSysNo, @CreateTime
                            );set @SysNo = SCOPE_IDENTITY();";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramCustomerSysNo = new SqlParameter("@CustomerSysNo", SqlDbType.Int,4);
			SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int,4);
			SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);

			paramSysNo.Direction = ParameterDirection.Output;

			if ( oParam.CustomerSysNo != AppConst.IntNull)
				paramCustomerSysNo.Value = oParam.CustomerSysNo;
			else
				paramCustomerSysNo.Value = System.DBNull.Value;
			if ( oParam.ProductSysNo != AppConst.IntNull)
				paramProductSysNo.Value = oParam.ProductSysNo;
			else
				paramProductSysNo.Value = System.DBNull.Value;
			if ( oParam.CreateTime != AppConst.DateTimeNull)
				paramCreateTime.Value = oParam.CreateTime;
			else
				paramCreateTime.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramCustomerSysNo);
			cmd.Parameters.Add(paramProductSysNo);
			cmd.Parameters.Add(paramCreateTime);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        
		}

		public int Delete(int customerSysNo, int productSysNo)
		{
			string sql = "delete from wishlist where customerSysNo = " + customerSysNo + " and productSysNo=" + productSysNo;
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);
		}
		public int Clear(int customerSysNo)
		{
			string sql = "delete from wishlist where customersysno =" + customerSysNo;
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);
		}
	}
}
