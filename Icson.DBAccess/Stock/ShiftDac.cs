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
	/// Summary description for ShiftDac.
	/// </summary>
	public class ShiftDac
	{
		
		public ShiftDac()
		{
		}

		public int InsertMaster(ShiftInfo oParam)
		{
			string sql = @"INSERT INTO St_Shift
                            (
                            SysNo, ShiftID, StockSysNoA, StockSysNoB, 
                            CreateTime, CreateUserSysNo, AuditTime, AuditUserSysNo, 
                            OutTime, OutUserSysNo, InTime, InUserSysNo, 
                            Status, Note
                            )
                            VALUES (
                            @SysNo, @ShiftID, @StockSysNoA, @StockSysNoB, 
                            @CreateTime, @CreateUserSysNo, @AuditTime, @AuditUserSysNo, 
                            @OutTime, @OutUserSysNo, @InTime, @InUserSysNo, 
                            @Status, @Note
                            )";

			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramShiftID = new SqlParameter("@ShiftID", SqlDbType.NVarChar,20);
			SqlParameter paramStockSysNoA = new SqlParameter("@StockSysNoA", SqlDbType.Int,4);
			SqlParameter paramStockSysNoB = new SqlParameter("@StockSysNoB", SqlDbType.Int,4);
			SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
			SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int,4);
			SqlParameter paramAuditTime = new SqlParameter("@AuditTime", SqlDbType.DateTime);
			SqlParameter paramAuditUserSysNo = new SqlParameter("@AuditUserSysNo", SqlDbType.Int,4);
			SqlParameter paramOutTime = new SqlParameter("@OutTime", SqlDbType.DateTime);
			SqlParameter paramOutUserSysNo = new SqlParameter("@OutUserSysNo", SqlDbType.Int,4);
			SqlParameter paramInTime = new SqlParameter("@InTime", SqlDbType.DateTime);
			SqlParameter paramInUserSysNo = new SqlParameter("@InUserSysNo", SqlDbType.Int,4);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);
			SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.NVarChar,200);

			if ( oParam.SysNo != AppConst.IntNull)
				paramSysNo.Value = oParam.SysNo;
			else
				paramSysNo.Value = System.DBNull.Value;
			if ( oParam.ShiftID != AppConst.StringNull)
				paramShiftID.Value = oParam.ShiftID;
			else
				paramShiftID.Value = System.DBNull.Value;
			if ( oParam.StockSysNoA != AppConst.IntNull)
				paramStockSysNoA.Value = oParam.StockSysNoA;
			else
				paramStockSysNoA.Value = System.DBNull.Value;
			if ( oParam.StockSysNoB != AppConst.IntNull)
				paramStockSysNoB.Value = oParam.StockSysNoB;
			else
				paramStockSysNoB.Value = System.DBNull.Value;
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
			if ( oParam.InTime != AppConst.DateTimeNull)
				paramInTime.Value = oParam.InTime;
			else
				paramInTime.Value = System.DBNull.Value;
			if ( oParam.InUserSysNo != AppConst.IntNull)
				paramInUserSysNo.Value = oParam.InUserSysNo;
			else
				paramInUserSysNo.Value = System.DBNull.Value;
			if ( oParam.Status != AppConst.IntNull)
				paramStatus.Value = oParam.Status;
			else
				paramStatus.Value = System.DBNull.Value;
			if ( oParam.Note != AppConst.StringNull)
				paramNote.Value = oParam.Note;
			else
				paramNote.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramShiftID);
			cmd.Parameters.Add(paramStockSysNoA);
			cmd.Parameters.Add(paramStockSysNoB);
			cmd.Parameters.Add(paramCreateTime);
			cmd.Parameters.Add(paramCreateUserSysNo);
			cmd.Parameters.Add(paramAuditTime);
			cmd.Parameters.Add(paramAuditUserSysNo);
			cmd.Parameters.Add(paramOutTime);
			cmd.Parameters.Add(paramOutUserSysNo);
			cmd.Parameters.Add(paramInTime);
			cmd.Parameters.Add(paramInUserSysNo);
			cmd.Parameters.Add(paramStatus);
			cmd.Parameters.Add(paramNote);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public int UpdateMaster(Hashtable paramHash)
		{
			StringBuilder sb = new StringBuilder(200);
			sb.Append("UPDATE St_Shift SET ");

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

		public int InsertItem(ShiftItemInfo oParam)
		{
			string sql = @"INSERT INTO St_Shift_Item
                            (
                            ShiftSysNo, ProductSysNo, ShiftQty
                            )
                            VALUES (
                            @ShiftSysNo, @ProductSysNo, @ShiftQty
                            );set @SysNo = SCOPE_IDENTITY();";

			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramShiftSysNo = new SqlParameter("@ShiftSysNo", SqlDbType.Int,4);
			SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int,4);
			SqlParameter paramShiftQty = new SqlParameter("@ShiftQty", SqlDbType.Int,4);

			paramSysNo.Direction = ParameterDirection.Output;

			if ( oParam.ShiftSysNo != AppConst.IntNull)
				paramShiftSysNo.Value = oParam.ShiftSysNo;
			else
				paramShiftSysNo.Value = System.DBNull.Value;
			if ( oParam.ProductSysNo != AppConst.IntNull)
				paramProductSysNo.Value = oParam.ProductSysNo;
			else
				paramProductSysNo.Value = System.DBNull.Value;
			if ( oParam.ShiftQty != AppConst.IntNull)
				paramShiftQty.Value = oParam.ShiftQty;
			else
				paramShiftQty.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramShiftSysNo);
			cmd.Parameters.Add(paramProductSysNo);
			cmd.Parameters.Add(paramShiftQty);

			return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);

		}

		public int UpdateItemQty(int shiftItemSysNo, int deltQty)
		{
			string sql = @"UPDATE St_Shift_Item SET 
                            ShiftQty=ShiftQty+@ShiftQty
                            WHERE SysNo=@SysNo";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramShiftQty = new SqlParameter("@ShiftQty", SqlDbType.Int,4);

			paramSysNo.Value = shiftItemSysNo;
			paramShiftQty.Value = deltQty;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramShiftQty);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public int DeleteItem(int paramSysNo)
		{
			string sql = "delete from St_Shift_Item where sysno = " + paramSysNo;
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);
		}

        public int InsertShiftItemProductID(ShiftItemProductIDInfo oParam)
        {
            string sql = @"INSERT INTO St_Shift_Item_ProductID
                            (
                            StShiftItemSysNo, ProductIDSysNo
                            )
                            VALUES (
                            @StShiftItemSysNo, @ProductIDSysNo
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramStShiftItemSysNo = new SqlParameter("@StShiftItemSysNo", SqlDbType.Int, 4);
            SqlParameter paramProductIDSysNo = new SqlParameter("@ProductIDSysNo", SqlDbType.Int, 4);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.StShiftItemSysNo != AppConst.IntNull)
                paramStShiftItemSysNo.Value = oParam.StShiftItemSysNo;
            else
                paramStShiftItemSysNo.Value = System.DBNull.Value;
            if (oParam.ProductIDSysNo != AppConst.IntNull)
                paramProductIDSysNo.Value = oParam.ProductIDSysNo;
            else
                paramProductIDSysNo.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramStShiftItemSysNo);
            cmd.Parameters.Add(paramProductIDSysNo);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int UpdateShiftItemProductID(ShiftItemProductIDInfo oParam)
        {
            string sql = @"UPDATE St_Shift_Item_ProductID SET 
                            StShiftItemSysNo=@StShiftItemSysNo, ProductIDSysNo=@ProductIDSysNo
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramStShiftItemSysNo = new SqlParameter("@StShiftItemSysNo", SqlDbType.Int, 4);
            SqlParameter paramProductIDSysNo = new SqlParameter("@ProductIDSysNo", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.StShiftItemSysNo != AppConst.IntNull)
                paramStShiftItemSysNo.Value = oParam.StShiftItemSysNo;
            else
                paramStShiftItemSysNo.Value = System.DBNull.Value;
            if (oParam.ProductIDSysNo != AppConst.IntNull)
                paramProductIDSysNo.Value = oParam.ProductIDSysNo;
            else
                paramProductIDSysNo.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramStShiftItemSysNo);
            cmd.Parameters.Add(paramProductIDSysNo);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
	}
}