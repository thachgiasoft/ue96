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
	/// Summary description for TransferDac.
	/// </summary>
	public class TransferDac
	{
		
		public TransferDac()
		{
		}

		public int InsertMaster(TransferInfo oParam)
		{
			string sql = @"INSERT INTO St_Transfer
                            (
                            SysNo, TransferID, StockSysNo, CreateTime, 
                            CreateUserSysNo, AuditTime, AuditUserSysNo, OutTime, 
                            OutUserSysNo, Status, Note
                            )
                            VALUES (
                            @SysNo, @TransferID, @StockSysNo, @CreateTime, 
                            @CreateUserSysNo, @AuditTime, @AuditUserSysNo, @OutTime, 
                            @OutUserSysNo, @Status, @Note
                            )";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramTransferID = new SqlParameter("@TransferID", SqlDbType.NVarChar,20);
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
			if ( oParam.TransferID != AppConst.StringNull)
				paramTransferID.Value = oParam.TransferID;
			else
				paramTransferID.Value = System.DBNull.Value;
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
			cmd.Parameters.Add(paramTransferID);
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

		public int UpdateMaster(Hashtable paramHash)
		{
			StringBuilder sb = new StringBuilder(200);
			sb.Append("UPDATE St_Transfer SET ");

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

		public int InsertItem(TransferItemInfo oParam)
		{
			string sql = @"INSERT INTO St_Transfer_Item
                            (
                            TransferSysNo, ProductSysNo, TransferType, 
                            TransferQty, TransferCost
                            )
                            VALUES (
                            @TransferSysNo, @ProductSysNo, @TransferType, 
                            @TransferQty, @TransferCost
                            );set @SysNo = SCOPE_IDENTITY();";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramTransferSysNo = new SqlParameter("@TransferSysNo", SqlDbType.Int,4);
			SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int,4);
			SqlParameter paramTransferType = new SqlParameter("@TransferType", SqlDbType.Int,4);
			SqlParameter paramTransferQty = new SqlParameter("@TransferQty", SqlDbType.Int,4);
			SqlParameter paramTransferCost = new SqlParameter("@TransferCost", SqlDbType.Decimal,9);

			paramSysNo.Direction = ParameterDirection.Output;

			if ( oParam.TransferSysNo != AppConst.IntNull)
				paramTransferSysNo.Value = oParam.TransferSysNo;
			else
				paramTransferSysNo.Value = System.DBNull.Value;
			if ( oParam.ProductSysNo != AppConst.IntNull)
				paramProductSysNo.Value = oParam.ProductSysNo;
			else
				paramProductSysNo.Value = System.DBNull.Value;
			if ( oParam.TransferType != AppConst.IntNull)
				paramTransferType.Value = oParam.TransferType;
			else
				paramTransferType.Value = System.DBNull.Value;
			if ( oParam.TransferQty != AppConst.IntNull)
				paramTransferQty.Value = oParam.TransferQty;
			else
				paramTransferQty.Value = System.DBNull.Value;
			if ( oParam.TransferCost != AppConst.DecimalNull)
				paramTransferCost.Value = oParam.TransferCost;
			else
				paramTransferCost.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramTransferSysNo);
			cmd.Parameters.Add(paramProductSysNo);
			cmd.Parameters.Add(paramTransferType);
			cmd.Parameters.Add(paramTransferQty);
			cmd.Parameters.Add(paramTransferCost);

			return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);

		}

		public int UpdateItem(int transferItemSysNo, int deltQty, decimal productCost)
		{
			string sql = @"UPDATE St_Transfer_Item SET 
                            TransferQty=TransferQty+@TransferQty, TransferCost = @TransferCost
                            WHERE SysNo=@SysNo";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramTransferQty = new SqlParameter("@TransferQty", SqlDbType.Int,4);
			SqlParameter paramTransferCost = new SqlParameter("@TransferCost", SqlDbType.Decimal,9);

			paramSysNo.Value = transferItemSysNo;
			paramTransferQty.Value = deltQty;
			paramTransferCost.Value = productCost;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramTransferQty);
			cmd.Parameters.Add(paramTransferCost);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public int DeleteItem(int paramSysNo)
		{
			string sql = "delete from St_Transfer_Item where sysno = " + paramSysNo;
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);
		}
	}
}
