using System;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects.Stock;

namespace Icson.DBAccess.Stock
{
	/// <summary>
	/// Summary description for VirtualDac.
	/// </summary>
	public class VirtualDac
	{
		
		public VirtualDac()
		{
		}
		public int Insert(VirtualInfo oParam)
		{
			string sql = @"INSERT INTO St_Virtual
                            (
                            ProductSysNo, VirtualQty, CreateTime, 
                            CreateUserSysNo
                            )
                            VALUES (
                            @ProductSysNo, @VirtualQty, @CreateTime, 
                            @CreateUserSysNo
                            );set @SysNo = SCOPE_IDENTITY();";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int,4);
			SqlParameter paramVirtualQty = new SqlParameter("@VirtualQty", SqlDbType.Int,4);
			SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
			SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int,4);

			paramSysNo.Direction = ParameterDirection.Output;

			if ( oParam.ProductSysNo != AppConst.IntNull)
				paramProductSysNo.Value = oParam.ProductSysNo;
			else
				paramProductSysNo.Value = System.DBNull.Value;
			if ( oParam.VirtualQty != AppConst.IntNull)
				paramVirtualQty.Value = oParam.VirtualQty;
			else
				paramVirtualQty.Value = System.DBNull.Value;
			if ( oParam.CreateTime != AppConst.DateTimeNull)
				paramCreateTime.Value = oParam.CreateTime;
			else
				paramCreateTime.Value = System.DBNull.Value;
			if ( oParam.CreateUserSysNo != AppConst.IntNull)
				paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
			else
				paramCreateUserSysNo.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramProductSysNo);
			cmd.Parameters.Add(paramVirtualQty);
			cmd.Parameters.Add(paramCreateTime);
			cmd.Parameters.Add(paramCreateUserSysNo);

			return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);

		}
	}
}
