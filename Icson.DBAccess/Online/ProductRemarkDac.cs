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
	/// Summary description for ProductRemarkDac.
	/// </summary>
	public class ProductRemarkDac
	{
		
		public ProductRemarkDac()
		{
		}

		public int Insert(ProductRemarkInfo oParam)
		{
			string sql = @"INSERT INTO Product_Remark
                            (
                            CustomerSysNo, ProductSysNo, CreateTime, 
                            Title, Remark, Score, Status,OptIP
                            )
                            VALUES (
                            @CustomerSysNo, @ProductSysNo, @CreateTime, 
                            @Title, @Remark, @Score, @Status,@OptIP
                            );set @SysNo = SCOPE_IDENTITY();";

			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramCustomerSysNo = new SqlParameter("@CustomerSysNo", SqlDbType.Int,4);
			SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int,4);
			SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
			SqlParameter paramTitle = new SqlParameter("@Title", SqlDbType.NVarChar,200);
			SqlParameter paramRemark = new SqlParameter("@Remark", SqlDbType.NText);
			SqlParameter paramScore = new SqlParameter("@Score", SqlDbType.Int,4);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);
            SqlParameter paramOptIP = new SqlParameter("@OptIP",SqlDbType.NVarChar,50);

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
			if ( oParam.Title != AppConst.StringNull)
				paramTitle.Value = oParam.Title;
			else
				paramTitle.Value = System.DBNull.Value;
			if ( oParam.Remark != AppConst.StringNull)
				paramRemark.Value = oParam.Remark;
			else
				paramRemark.Value = System.DBNull.Value;
			if ( oParam.Score != AppConst.IntNull)
				paramScore.Value = oParam.Score;
			else
				paramScore.Value = System.DBNull.Value;
			if ( oParam.Status != AppConst.IntNull)
				paramStatus.Value = oParam.Status;
			else
				paramStatus.Value = System.DBNull.Value;
            if (oParam.OptIP != AppConst.StringNull)
                paramOptIP.Value = oParam.OptIP;
            else
                paramOptIP.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramCustomerSysNo);
			cmd.Parameters.Add(paramProductSysNo);
			cmd.Parameters.Add(paramCreateTime);
			cmd.Parameters.Add(paramTitle);
			cmd.Parameters.Add(paramRemark);
			cmd.Parameters.Add(paramScore);
			cmd.Parameters.Add(paramStatus);
		    cmd.Parameters.Add(paramOptIP);
			return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);

		}

		public int Update(Hashtable paramHash)
		{
			StringBuilder sb = new StringBuilder(200);
			sb.Append("UPDATE Product_Remark SET ");

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
	}
}
