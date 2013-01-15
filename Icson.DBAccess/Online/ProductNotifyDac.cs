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
	/// Summary description for ProductNotifyDac.
	/// </summary>
	public class ProductNotifyDac
	{
		
		public ProductNotifyDac()
		{
		}
		public int Insert(ProductNotifyInfo oParam)
		{
			string sql = @"INSERT INTO Product_Notify
                            (
                            CustomerSysNo, ProductSysNo, Email, 
                            CreateTime, NotifyTime, Status
                            )
                            VALUES (
                            @CustomerSysNo, @ProductSysNo, @Email, 
                            @CreateTime, @NotifyTime, @Status
                            );set @SysNo = SCOPE_IDENTITY();";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramCustomerSysNo = new SqlParameter("@CustomerSysNo", SqlDbType.Int,4);
			SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int,4);
			SqlParameter paramEmail = new SqlParameter("@Email", SqlDbType.NVarChar,50);
			SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
			SqlParameter paramNotifyTime = new SqlParameter("@NotifyTime", SqlDbType.DateTime);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);

			paramSysNo.Direction = ParameterDirection.Output;

			if ( oParam.CustomerSysNo != AppConst.IntNull)
				paramCustomerSysNo.Value = oParam.CustomerSysNo;
			else
				paramCustomerSysNo.Value = System.DBNull.Value;
			if ( oParam.ProductSysNo != AppConst.IntNull)
				paramProductSysNo.Value = oParam.ProductSysNo;
			else
				paramProductSysNo.Value = System.DBNull.Value;
			if ( oParam.Email != AppConst.StringNull)
				paramEmail.Value = oParam.Email;
			else
				paramEmail.Value = System.DBNull.Value;
			if ( oParam.CreateTime != AppConst.DateTimeNull)
				paramCreateTime.Value = oParam.CreateTime;
			else
				paramCreateTime.Value = System.DBNull.Value;
			if ( oParam.NotifyTime != AppConst.DateTimeNull)
				paramNotifyTime.Value = oParam.NotifyTime;
			else
				paramNotifyTime.Value = System.DBNull.Value;
			if ( oParam.Status != AppConst.IntNull)
				paramStatus.Value = oParam.Status;
			else
				paramStatus.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramCustomerSysNo);
			cmd.Parameters.Add(paramProductSysNo);
			cmd.Parameters.Add(paramEmail);
			cmd.Parameters.Add(paramCreateTime);
			cmd.Parameters.Add(paramNotifyTime);
			cmd.Parameters.Add(paramStatus);

			return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);

		}

		public int Update(Hashtable paramHash)
		{
			StringBuilder sb = new StringBuilder(200);
			sb.Append("UPDATE Product_Notify SET ");

			if ( paramHash != null && paramHash.Count != 0 )
			{
				int index = 0;
				foreach(string key in paramHash.Keys)
				{
					object item = paramHash[key];
					if ( key.ToLower()=="sysno" )
						continue;

					if ( index != 0 )
						sb.Append(",");
					index++;

					
					if (item is int || item is decimal)
					{
						sb.Append(key).Append("=").Append(item.ToString());
					}
					else if (item is string)
					{
						sb.Append(key).Append(" = ").Append(Util.ToSqlString(item.ToString()));
					}
					else if (item is DateTime)
					{
						sb.Append(key).Append(" = cast(").Append( Util.ToSqlString(((DateTime)item).ToString(AppConst.DateFormatLong))).Append(" as DateTime)");
					}
				}
			}

			sb.Append(" WHERE SysNo=").Append(paramHash["SysNo"]);

			return SqlHelper.ExecuteNonQuery(sb.ToString());
		}

		public int Delete(int sysno)
		{
			string sql = "delete from product_notify where sysno=" + sysno;
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public int Clear(int customerySysNo)
		{
			string sql = "delete from product_notify where customerSysNo =" + customerySysNo;
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);
		}
	}
}
