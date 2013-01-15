using System;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects.Online;

namespace Icson.DBAccess.Online
{
	/// <summary>
	/// Summary description for OnlineListDac.
	/// </summary>
	public class OnlineListDac
	{
		
		public OnlineListDac()
		{
		}
		public int Insert(OnlineListInfo oParam)
		{
			string sql = @"INSERT INTO OnlineList
                            (
                            ListArea, ProductSysNo, CreateUserSysNo, 
                            CreateTime, ListOrder
                            )
                            VALUES (
                            @ListArea, @ProductSysNo, @CreateUserSysNo, 
                            @CreateTime, @ListOrder
                            );set @SysNo = SCOPE_IDENTITY();";

			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramListArea = new SqlParameter("@ListArea", SqlDbType.Int,4);
			SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int,4);
			SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int,4);
			SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
			SqlParameter paramListOrder = new SqlParameter("@ListOrder", SqlDbType.NVarChar,20);

			paramSysNo.Direction = ParameterDirection.Output;

			if ( oParam.ListArea != AppConst.IntNull)
				paramListArea.Value = oParam.ListArea;
			else
				paramListArea.Value = System.DBNull.Value;
			if ( oParam.ProductSysNo != AppConst.IntNull)
				paramProductSysNo.Value = oParam.ProductSysNo;
			else
				paramProductSysNo.Value = System.DBNull.Value;
			if ( oParam.CreateUserSysNo != AppConst.IntNull)
				paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
			else
				paramCreateUserSysNo.Value = System.DBNull.Value;
			if ( oParam.CreateTime != AppConst.DateTimeNull)
				paramCreateTime.Value = oParam.CreateTime;
			else
				paramCreateTime.Value = System.DBNull.Value;
			if ( oParam.ListOrder != AppConst.StringNull)
				paramListOrder.Value = oParam.ListOrder;
			else
				paramListOrder.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramListArea);
			cmd.Parameters.Add(paramProductSysNo);
			cmd.Parameters.Add(paramCreateUserSysNo);
			cmd.Parameters.Add(paramCreateTime);
			cmd.Parameters.Add(paramListOrder);

			return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);

		}
		public int Delete(int sysno)
		{
			string sql = "delete from onlinelist where sysno = " + sysno;
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);

		}
	}
}
