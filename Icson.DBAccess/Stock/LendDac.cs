using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections;

using Icson.Utils;
using Icson.Objects.Stock;

namespace Icson.DBAccess.Stock
{
	/// <summary>
	/// Summary description for LendDac.
	/// </summary>
	public class LendDac
	{
		
		public LendDac()
		{
		}
		public int InsertMaster(LendInfo oParam)
		{
			string sql = @"INSERT INTO St_Lend
                            (
                            SysNo, LendID, StockSysNo, LendUserSysNo, 
                            CreateTime, CreateUserSysNo, Status, Note
                            )
                            VALUES (
                            @SysNo, @LendID, @StockSysNo, @LendUserSysNo, 
                            @CreateTime, @CreateUserSysNo, @Status, @Note
                            )";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramLendID = new SqlParameter("@LendID", SqlDbType.NVarChar,20);
			SqlParameter paramStockSysNo = new SqlParameter("@StockSysNo", SqlDbType.Int,4);
			SqlParameter paramLendUserSysNo = new SqlParameter("@LendUserSysNo", SqlDbType.Int,4);
			SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
			SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int,4);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);
			SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.NVarChar,200);

			paramSysNo.Value = oParam.SysNo;
			paramLendID.Value = oParam.LendID;
			paramStockSysNo.Value = oParam.StockSysNo;
			paramLendUserSysNo.Value = oParam.LendUserSysNo;
			paramCreateTime.Value = oParam.CreateTime;
			paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
			
			paramStatus.Value = oParam.Status;
			
			if ( oParam.Note != AppConst.StringNull)
				paramNote.Value = oParam.Note;
			else
				paramNote.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramLendID);
			cmd.Parameters.Add(paramStockSysNo);
			cmd.Parameters.Add(paramLendUserSysNo);
			cmd.Parameters.Add(paramCreateTime);
			cmd.Parameters.Add(paramCreateUserSysNo);
			cmd.Parameters.Add(paramStatus);
			cmd.Parameters.Add(paramNote);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public int UpdateMaster(Hashtable paramHash)
		{

			StringBuilder sb = new StringBuilder(200);
			sb.Append("UPDATE St_Lend SET ");

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

					
					if (item is int)
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

		public int InsertItem(LendItemInfo oParam)
		{
			string sql = @"INSERT INTO St_Lend_Item
                            (
                            LendSysNo, ProductSysNo, LendQty, 
                            ExpectReturnTime
                            )
                            VALUES (
                            @LendSysNo, @ProductSysNo, @LendQty, 
                            @ExpectReturnTime
                            );set @SysNo = SCOPE_IDENTITY();";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramLendSysNo = new SqlParameter("@LendSysNo", SqlDbType.Int,4);
			SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int,4);
			SqlParameter paramLendQty = new SqlParameter("@LendQty", SqlDbType.Int,4);
			SqlParameter paramExpectReturnTime = new SqlParameter("@ExpectReturnTime", SqlDbType.DateTime);

			paramSysNo.Direction = ParameterDirection.Output;
			paramLendSysNo.Value = oParam.LendSysNo;
			paramProductSysNo.Value = oParam.ProductSysNo;
			paramLendQty.Value = oParam.LendQty;

			if ( oParam.ExpectReturnTime != AppConst.DateTimeNull)
				paramExpectReturnTime.Value = oParam.ExpectReturnTime.ToString(AppConst.DateFormatLong);
			else
				paramExpectReturnTime.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramLendSysNo);
			cmd.Parameters.Add(paramProductSysNo);
			cmd.Parameters.Add(paramLendQty);
			cmd.Parameters.Add(paramExpectReturnTime);

			return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);

		}

		public int UpdateItem(int lendItemSysNo, int deltQty, DateTime expectReturnTime)
		{
			string sql = @"UPDATE St_Lend_Item SET 
                            LendQty=LendQty+@LendQty, ExpectReturnTime=@ExpectReturnTime
                            WHERE SysNo=@SysNo";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramLendQty = new SqlParameter("@LendQty", SqlDbType.Int,4);
			SqlParameter paramExpectReturnTime = new SqlParameter("@ExpectReturnTime", SqlDbType.DateTime);

			paramSysNo.Value = lendItemSysNo;
			paramLendQty.Value = deltQty;

			if ( expectReturnTime != AppConst.DateTimeNull)
				paramExpectReturnTime.Value = expectReturnTime;
			else
				paramExpectReturnTime.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramLendQty);
			cmd.Parameters.Add(paramExpectReturnTime);

			return SqlHelper.ExecuteNonQuery(cmd);
		}
		public int DeleteItem(int paramSysNo)
		{
			string sql = "delete from St_Lend_Item where sysno = " + paramSysNo;
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);
		}
		public int InsertReturn(LendReturnInfo oParam)
		{
			string sql = @"INSERT INTO St_Lend_Return
                            (
                            LendSysNo, ProductSysNo, ReturnQty, 
                            ReturnTime
                            )
                            VALUES (
                            @LendSysNo, @ProductSysNo, @ReturnQty, 
                            @ReturnTime
                            ); set @SysNo = SCOPE_IDENTITY();";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramLendSysNo = new SqlParameter("@LendSysNo", SqlDbType.Int,4);
			SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int,4);
			SqlParameter paramReturnQty = new SqlParameter("@ReturnQty", SqlDbType.Int,4);
			SqlParameter paramReturnTime = new SqlParameter("@ReturnTime", SqlDbType.DateTime);

			paramSysNo.Direction = ParameterDirection.Output;
			
			paramLendSysNo.Value = oParam.LendSysNo;
			paramProductSysNo.Value = oParam.ProductSysNo;
			paramReturnQty.Value = oParam.ReturnQty;
			paramReturnTime.Value = oParam.ReturnTime;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramLendSysNo);
			cmd.Parameters.Add(paramProductSysNo);
			cmd.Parameters.Add(paramReturnQty);
			cmd.Parameters.Add(paramReturnTime);

			return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);

		}
		public int DeleteReturn(int paramSysNo)
		{
			string sql = "delete from St_Lend_Return where sysno = " + paramSysNo;
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public int ImportMaster(LendInfo oParam)
		{
			string sql = @"INSERT INTO St_Lend
                            (
                            SysNo, LendID, StockSysNo, LendUserSysNo, 
                            CreateTime, CreateUserSysNo, AuditTime, AuditUserSysNo, 
                            OutTime, OutUserSysNo, Status, Note
                            )
                            VALUES (
                            @SysNo, @LendID, @StockSysNo, @LendUserSysNo, 
                            @CreateTime, @CreateUserSysNo, @AuditTime, @AuditUserSysNo, 
                            @OutTime, @OutUserSysNo, @Status, @Note
                            )";

			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramLendID = new SqlParameter("@LendID", SqlDbType.NVarChar,20);
			SqlParameter paramStockSysNo = new SqlParameter("@StockSysNo", SqlDbType.Int,4);
			SqlParameter paramLendUserSysNo = new SqlParameter("@LendUserSysNo", SqlDbType.Int,4);
			SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
			SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int,4);
			SqlParameter paramAuditTime = new SqlParameter("@AuditTime", SqlDbType.DateTime);
			SqlParameter paramAuditUserSysNo = new SqlParameter("@AuditUserSysNo", SqlDbType.Int,4);
			SqlParameter paramOutTime = new SqlParameter("@OutTime", SqlDbType.DateTime);
			SqlParameter paramOutUserSysNo = new SqlParameter("@OutUserSysNo", SqlDbType.Int,4);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);
			SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.NVarChar,200);

			if ( oParam.SysNo != AppConst.IntNull)
				paramSysNo.Value = oParam.SysNo;
			else
				paramSysNo.Value = System.DBNull.Value;
			if ( oParam.LendID != AppConst.StringNull)
				paramLendID.Value = oParam.LendID;
			else
				paramLendID.Value = System.DBNull.Value;
			if ( oParam.StockSysNo != AppConst.IntNull)
				paramStockSysNo.Value = oParam.StockSysNo;
			else
				paramStockSysNo.Value = System.DBNull.Value;
			if ( oParam.LendUserSysNo != AppConst.IntNull)
				paramLendUserSysNo.Value = oParam.LendUserSysNo;
			else
				paramLendUserSysNo.Value = System.DBNull.Value;
			if ( oParam.CreateTime != AppConst.DateTimeNull)
				paramCreateTime.Value = oParam.CreateTime;
			else
				paramCreateTime.Value = System.DBNull.Value;
			if ( oParam.CreateUserSysNo != AppConst.IntNull)
				paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
			else
				paramCreateUserSysNo.Value = System.DBNull.Value;
			if ( oParam.AuditTime != AppConst.DateTimeNull)
				paramAuditTime.Value = oParam.AuditTime;
			else
				paramAuditTime.Value = System.DBNull.Value;
			if ( oParam.AuditUserSysNo != AppConst.IntNull)
				paramAuditUserSysNo.Value = oParam.AuditUserSysNo;
			else
				paramAuditUserSysNo.Value = System.DBNull.Value;
			if ( oParam.OutTime != AppConst.DateTimeNull)
				paramOutTime.Value = oParam.OutTime;
			else
				paramOutTime.Value = System.DBNull.Value;
			if ( oParam.OutUserSysNo != AppConst.IntNull)
				paramOutUserSysNo.Value = oParam.OutUserSysNo;
			else
				paramOutUserSysNo.Value = System.DBNull.Value;
			if ( oParam.Status != AppConst.IntNull)
				paramStatus.Value = oParam.Status;
			else
				paramStatus.Value = System.DBNull.Value;
			if ( oParam.Note != AppConst.StringNull)
				paramNote.Value = oParam.Note;
			else
				paramNote.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramLendID);
			cmd.Parameters.Add(paramStockSysNo);
			cmd.Parameters.Add(paramLendUserSysNo);
			cmd.Parameters.Add(paramCreateTime);
			cmd.Parameters.Add(paramCreateUserSysNo);
			cmd.Parameters.Add(paramAuditTime);
			cmd.Parameters.Add(paramAuditUserSysNo);
			cmd.Parameters.Add(paramOutTime);
			cmd.Parameters.Add(paramOutUserSysNo);
			cmd.Parameters.Add(paramStatus);
			cmd.Parameters.Add(paramNote);

			return SqlHelper.ExecuteNonQuery(cmd);
		}
		

	}
}
