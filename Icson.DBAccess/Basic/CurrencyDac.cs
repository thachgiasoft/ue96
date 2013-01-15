using System;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects.Basic;

namespace Icson.DBAccess.Basic
{
	/// <summary>
	/// Summary description for CurrencyDac.
	/// </summary>
	public class CurrencyDac
	{
		public CurrencyDac()
		{
		}

		
		public int Insert(CurrencyInfo oParam)
		{
			string sql = @"INSERT INTO Currency
                            (
                            SysNo, CurrencyID, CurrencyName, CurrencySymbol, 
                            IsLocal, ExchangeRate, ListOrder, Status
                            )
                            VALUES (
                            @SysNo, @CurrencyID, @CurrencyName, @CurrencySymbol, 
                            @IsLocal, @ExchangeRate, @ListOrder, @Status
                            )";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramCurrencyID = new SqlParameter("@CurrencyID", SqlDbType.NVarChar,20);
			SqlParameter paramCurrencyName = new SqlParameter("@CurrencyName", SqlDbType.NVarChar,50);
			SqlParameter paramCurrencySymbol = new SqlParameter("@CurrencySymbol", SqlDbType.NVarChar,20);
			SqlParameter paramIsLocal = new SqlParameter("@IsLocal", SqlDbType.Int,4);
			SqlParameter paramExchangeRate = new SqlParameter("@ExchangeRate", SqlDbType.Decimal,9);
			SqlParameter paramListOrder = new SqlParameter("@ListOrder", SqlDbType.NVarChar,10);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);

			if ( oParam.SysNo != AppConst.IntNull)
				paramSysNo.Value = oParam.SysNo;
			else
				paramSysNo.Value = System.DBNull.Value;
			if ( oParam.CurrencyID != AppConst.StringNull)
				paramCurrencyID.Value = oParam.CurrencyID;
			else
				paramCurrencyID.Value = System.DBNull.Value;
			if ( oParam.CurrencyName != AppConst.StringNull)
				paramCurrencyName.Value = oParam.CurrencyName;
			else
				paramCurrencyName.Value = System.DBNull.Value;
			if ( oParam.CurrencySymbol != AppConst.StringNull)
				paramCurrencySymbol.Value = oParam.CurrencySymbol;
			else
				paramCurrencySymbol.Value = System.DBNull.Value;
			if ( oParam.IsLocal != AppConst.IntNull)
				paramIsLocal.Value = oParam.IsLocal;
			else
				paramIsLocal.Value = System.DBNull.Value;
			if ( oParam.ExchangeRate != AppConst.DecimalNull)
				paramExchangeRate.Value = oParam.ExchangeRate;
			else
				paramExchangeRate.Value = System.DBNull.Value;
			if ( oParam.ListOrder != AppConst.StringNull)
				paramListOrder.Value = oParam.ListOrder;
			else
				paramListOrder.Value = System.DBNull.Value;
			if ( oParam.Status != AppConst.IntNull)
				paramStatus.Value = oParam.Status;
			else
				paramStatus.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramCurrencyID);
			cmd.Parameters.Add(paramCurrencyName);
			cmd.Parameters.Add(paramCurrencySymbol);
			cmd.Parameters.Add(paramIsLocal);
			cmd.Parameters.Add(paramExchangeRate);
			cmd.Parameters.Add(paramListOrder);
			cmd.Parameters.Add(paramStatus);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public int Update(CurrencyInfo oParam)
		{
			string sql = @"UPDATE Currency SET 
                            CurrencyID=@CurrencyID, 
                            CurrencyName=@CurrencyName, CurrencySymbol=@CurrencySymbol, 
                            IsLocal=@IsLocal, ExchangeRate=@ExchangeRate, 
                            ListOrder=@ListOrder, Status=@Status
                            WHERE SysNo=@SysNo";

			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramCurrencyID = new SqlParameter("@CurrencyID", SqlDbType.NVarChar,20);
			SqlParameter paramCurrencyName = new SqlParameter("@CurrencyName", SqlDbType.NVarChar,50);
			SqlParameter paramCurrencySymbol = new SqlParameter("@CurrencySymbol", SqlDbType.NVarChar,20);
			SqlParameter paramIsLocal = new SqlParameter("@IsLocal", SqlDbType.Int,4);
			SqlParameter paramExchangeRate = new SqlParameter("@ExchangeRate", SqlDbType.Decimal,9);
			SqlParameter paramListOrder = new SqlParameter("@ListOrder", SqlDbType.NVarChar,10);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);

			if ( oParam.SysNo != AppConst.IntNull)
				paramSysNo.Value = oParam.SysNo;
			else
				paramSysNo.Value = System.DBNull.Value;
			if ( oParam.CurrencyID != AppConst.StringNull)
				paramCurrencyID.Value = oParam.CurrencyID;
			else
				paramCurrencyID.Value = System.DBNull.Value;
			if ( oParam.CurrencyName != AppConst.StringNull)
				paramCurrencyName.Value = oParam.CurrencyName;
			else
				paramCurrencyName.Value = System.DBNull.Value;
			if ( oParam.CurrencySymbol != AppConst.StringNull)
				paramCurrencySymbol.Value = oParam.CurrencySymbol;
			else
				paramCurrencySymbol.Value = System.DBNull.Value;
			if ( oParam.IsLocal != AppConst.IntNull)
				paramIsLocal.Value = oParam.IsLocal;
			else
				paramIsLocal.Value = System.DBNull.Value;
			if ( oParam.ExchangeRate != AppConst.DecimalNull)
				paramExchangeRate.Value = oParam.ExchangeRate;
			else
				paramExchangeRate.Value = System.DBNull.Value;
			if ( oParam.ListOrder != AppConst.StringNull)
				paramListOrder.Value = oParam.ListOrder;
			else
				paramListOrder.Value = System.DBNull.Value;
			if ( oParam.Status != AppConst.IntNull)
				paramStatus.Value = oParam.Status;
			else
				paramStatus.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramCurrencyID);
			cmd.Parameters.Add(paramCurrencyName);
			cmd.Parameters.Add(paramCurrencySymbol);
			cmd.Parameters.Add(paramIsLocal);
			cmd.Parameters.Add(paramExchangeRate);
			cmd.Parameters.Add(paramListOrder);
			cmd.Parameters.Add(paramStatus);

			return SqlHelper.ExecuteNonQuery(cmd);
		}
	}
}
