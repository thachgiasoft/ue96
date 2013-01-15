using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Text;

using Icson.Utils;
using Icson.Objects.Finance;

namespace Icson.DBAccess.Finance
{
	/// <summary>
	/// Summary description for SOIncomeDac.
	/// </summary>
	public class SOIncomeDac
	{
		
		public SOIncomeDac()
		{
		}
		public int Insert(SOIncomeInfo oParam)
		{
			string sql = @"INSERT INTO Finance_SOIncome
                            (
                            OrderType, OrderSysNo,  
                            OrderAmt, IncomeStyle, IncomeAmt, IncomeTime, 
                            IncomeUserSysNo, ConfirmTime, ConfirmUserSysNo, Note, 
                            Status
                            )
                            VALUES (
                            @OrderType, @OrderSysNo,  
                            @OrderAmt, @IncomeStyle, @IncomeAmt, @IncomeTime, 
                            @IncomeUserSysNo, @ConfirmTime, @ConfirmUserSysNo, @Note, 
                            @Status
                            );set @SysNo = SCOPE_IDENTITY();";

			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramOrderType = new SqlParameter("@OrderType", SqlDbType.Int,4);
			SqlParameter paramOrderSysNo = new SqlParameter("@OrderSysNo", SqlDbType.Int,4);
			SqlParameter paramOrderAmt = new SqlParameter("@OrderAmt", SqlDbType.Decimal,9);
			SqlParameter paramIncomeStyle = new SqlParameter("@IncomeStyle", SqlDbType.Int,4);
			SqlParameter paramIncomeAmt = new SqlParameter("@IncomeAmt", SqlDbType.Decimal,9);
			SqlParameter paramIncomeTime = new SqlParameter("@IncomeTime", SqlDbType.DateTime);
			SqlParameter paramIncomeUserSysNo = new SqlParameter("@IncomeUserSysNo", SqlDbType.Int,4);
			SqlParameter paramConfirmTime = new SqlParameter("@ConfirmTime", SqlDbType.DateTime);
			SqlParameter paramConfirmUserSysNo = new SqlParameter("@ConfirmUserSysNo", SqlDbType.Int,4);
			SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.NVarChar,200);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);

			paramSysNo.Direction = ParameterDirection.Output;

			if ( oParam.OrderType != AppConst.IntNull)
				paramOrderType.Value = oParam.OrderType;
			else
				paramOrderType.Value = System.DBNull.Value;
			if ( oParam.OrderSysNo != AppConst.IntNull)
				paramOrderSysNo.Value = oParam.OrderSysNo;
			else
				paramOrderSysNo.Value = System.DBNull.Value;
			if ( oParam.OrderAmt != AppConst.DecimalNull)
				paramOrderAmt.Value = oParam.OrderAmt;
			else
				paramOrderAmt.Value = System.DBNull.Value;
			if ( oParam.IncomeStyle != AppConst.IntNull)
				paramIncomeStyle.Value = oParam.IncomeStyle;
			else
				paramIncomeStyle.Value = System.DBNull.Value;
			if ( oParam.IncomeAmt != AppConst.DecimalNull)
				paramIncomeAmt.Value = oParam.IncomeAmt;
			else
				paramIncomeAmt.Value = System.DBNull.Value;
			if ( oParam.IncomeTime != AppConst.DateTimeNull)
				paramIncomeTime.Value = oParam.IncomeTime;
			else
				paramIncomeTime.Value = System.DBNull.Value;
			if ( oParam.IncomeUserSysNo != AppConst.IntNull)
				paramIncomeUserSysNo.Value = oParam.IncomeUserSysNo;
			else
				paramIncomeUserSysNo.Value = System.DBNull.Value;
			if ( oParam.ConfirmTime != AppConst.DateTimeNull)
				paramConfirmTime.Value = oParam.ConfirmTime;
			else
				paramConfirmTime.Value = System.DBNull.Value;
			if ( oParam.ConfirmUserSysNo != AppConst.IntNull)
				paramConfirmUserSysNo.Value = oParam.ConfirmUserSysNo;
			else
				paramConfirmUserSysNo.Value = System.DBNull.Value;
			if ( oParam.Note != AppConst.StringNull)
				paramNote.Value = oParam.Note;
			else
				paramNote.Value = System.DBNull.Value;
			if ( oParam.Status != AppConst.IntNull)
				paramStatus.Value = oParam.Status;
			else
				paramStatus.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramOrderType);
			cmd.Parameters.Add(paramOrderSysNo);
			cmd.Parameters.Add(paramOrderAmt);
			cmd.Parameters.Add(paramIncomeStyle);
			cmd.Parameters.Add(paramIncomeAmt);
			cmd.Parameters.Add(paramIncomeTime);
			cmd.Parameters.Add(paramIncomeUserSysNo);
			cmd.Parameters.Add(paramConfirmTime);
			cmd.Parameters.Add(paramConfirmUserSysNo);
			cmd.Parameters.Add(paramNote);
			cmd.Parameters.Add(paramStatus);

			return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);

		}

		public int Update(Hashtable paramHash)
		{
			StringBuilder sb = new StringBuilder(200);
			sb.Append("UPDATE Finance_SOIncome SET ");

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
		public int Delete(int sysno)
		{
			string sql = "delete from finance_soincome where sysno = " + sysno;
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);

		}
	}
}
