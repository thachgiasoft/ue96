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
	/// Summary description for AdjustDac.
	/// </summary>
	public class AdjustDac
	{
		
		public AdjustDac()
		{
		}

		public int InsertMaster(AdjustInfo oParam)
		{
			string sql = @"INSERT INTO St_Adjust
                            (
                            SysNo, AdjustID, StockSysNo, CreateTime, 
                            CreateUserSysNo, Status, Note
                            )
                            VALUES (
                            @SysNo, @AdjustID, @StockSysNo, @CreateTime, 
                            @CreateUserSysNo, @Status, @Note
                            )";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramAdjustID = new SqlParameter("@AdjustID", SqlDbType.NVarChar,20);
			SqlParameter paramStockSysNo = new SqlParameter("@StockSysNo", SqlDbType.Int,4);
			SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
			SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int,4);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);
			SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.NVarChar,200);

			if ( oParam.SysNo != AppConst.IntNull)
				paramSysNo.Value = oParam.SysNo;
			else
				paramSysNo.Value = System.DBNull.Value;
			if ( oParam.AdjustID != AppConst.StringNull)
				paramAdjustID.Value = oParam.AdjustID;
			else
				paramAdjustID.Value = System.DBNull.Value;
			if ( oParam.StockSysNo != AppConst.IntNull)
				paramStockSysNo.Value = oParam.StockSysNo;
			else
				paramStockSysNo.Value = System.DBNull.Value;
			if ( oParam.CreateTime != AppConst.DateTimeNull)
				paramCreateTime.Value = oParam.CreateTime;
			else
				paramCreateTime.Value = System.DBNull.Value;
			if ( oParam.CreateUserSysNo != AppConst.IntNull)
				paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
			else
				paramCreateUserSysNo.Value = System.DBNull.Value;
			if ( oParam.Status != AppConst.IntNull)
				paramStatus.Value = oParam.Status;
			else
				paramStatus.Value = System.DBNull.Value;
			if ( oParam.Note != AppConst.StringNull)
				paramNote.Value = oParam.Note;
			else
				paramNote.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramAdjustID);
			cmd.Parameters.Add(paramStockSysNo);
			cmd.Parameters.Add(paramCreateTime);
			cmd.Parameters.Add(paramCreateUserSysNo);
			cmd.Parameters.Add(paramStatus);
			cmd.Parameters.Add(paramNote);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public int UpdateMaster(Hashtable paramHash)
		{
			StringBuilder sb = new StringBuilder(200);
			sb.Append("UPDATE St_Adjust SET ");

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
		public int InsertItem(AdjustItemInfo oParam)
		{
			string sql = @"INSERT INTO St_Adjust_Item
                            (
                            AdjustSysNo, ProductSysNo, AdjustQty, 
                            AdjustCost
                            )
                            VALUES (
                            @AdjustSysNo, @ProductSysNo, @AdjustQty, 
                            @AdjustCost
                            );set @SysNo = SCOPE_IDENTITY();";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramAdjustSysNo = new SqlParameter("@AdjustSysNo", SqlDbType.Int,4);
			SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int,4);
			SqlParameter paramAdjustQty = new SqlParameter("@AdjustQty", SqlDbType.Int,4);
			SqlParameter paramAdjustCost = new SqlParameter("@AdjustCost", SqlDbType.Decimal,9);

			paramSysNo.Direction = ParameterDirection.Output;

			if ( oParam.AdjustSysNo != AppConst.IntNull)
				paramAdjustSysNo.Value = oParam.AdjustSysNo;
			else
				paramAdjustSysNo.Value = System.DBNull.Value;
			if ( oParam.ProductSysNo != AppConst.IntNull)
				paramProductSysNo.Value = oParam.ProductSysNo;
			else
				paramProductSysNo.Value = System.DBNull.Value;
			if ( oParam.AdjustQty != AppConst.IntNull)
				paramAdjustQty.Value = oParam.AdjustQty;
			else
				paramAdjustQty.Value = System.DBNull.Value;
			if ( oParam.AdjustCost != AppConst.DecimalNull)
				paramAdjustCost.Value = oParam.AdjustCost;
			else
				paramAdjustCost.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramAdjustSysNo);
			cmd.Parameters.Add(paramProductSysNo);
			cmd.Parameters.Add(paramAdjustQty);
			cmd.Parameters.Add(paramAdjustCost);

			return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);

		}

		public int UpdateItemQtyCost(int adjustItemSysNo, int deltQty, decimal adjustCost)
		{
			string sql = @"UPDATE St_Adjust_Item SET 
                            AdjustQty=AdjustQty+@AdjustQty, AdjustCost=@AdjustCost
                            WHERE SysNo=@SysNo";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramAdjustQty = new SqlParameter("@AdjustQty", SqlDbType.Int,4);
			SqlParameter paramAdjustCost = new SqlParameter("@AdjustCost", SqlDbType.Decimal,19);

			paramSysNo.Value = adjustItemSysNo;
			paramAdjustQty.Value = deltQty;
			paramAdjustCost.Value = adjustCost;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramAdjustQty);
			cmd.Parameters.Add(paramAdjustCost);

			return SqlHelper.ExecuteNonQuery(cmd);
		}
		public int UpdateItemCost(int adjustItemSysNo, decimal productCost)
		{
			string sql = @"UPDATE St_Adjust_Item SET 
                            AdjustCost=@AdjustCost
                            WHERE SysNo=@SysNo";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramAdjustCost = new SqlParameter("@AdjustCost", SqlDbType.Decimal,9);

			paramSysNo.Value = adjustItemSysNo;
			paramAdjustCost.Value = productCost;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramAdjustCost);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public int DeleteItem(int paramSysNo)
		{
			string sql = "delete from St_Adjust_Item where sysno = " + paramSysNo;
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public int ImportMaster(AdjustInfo oParam)
		{
			string sql = @"INSERT INTO St_Adjust
                            (
                            SysNo, AdjustID, StockSysNo, CreateTime, 
                            CreateUserSysNo, AuditTime, AuditUserSysNo, OutTime, 
                            OutUserSysNo, Status, Note
                            )
                            VALUES (
                            @SysNo, @AdjustID, @StockSysNo, @CreateTime, 
                            @CreateUserSysNo, @AuditTime, @AuditUserSysNo, @OutTime, 
                            @OutUserSysNo, @Status, @Note
                            )";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramAdjustID = new SqlParameter("@AdjustID", SqlDbType.NVarChar,20);
			SqlParameter paramStockSysNo = new SqlParameter("@StockSysNo", SqlDbType.Int,4);
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
			if ( oParam.AdjustID != AppConst.StringNull)
				paramAdjustID.Value = oParam.AdjustID;
			else
				paramAdjustID.Value = System.DBNull.Value;
			if ( oParam.StockSysNo != AppConst.IntNull)
				paramStockSysNo.Value = oParam.StockSysNo;
			else
				paramStockSysNo.Value = System.DBNull.Value;
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
			cmd.Parameters.Add(paramAdjustID);
			cmd.Parameters.Add(paramStockSysNo);
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
