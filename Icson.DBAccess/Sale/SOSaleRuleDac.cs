using System;
using System.Data;
using System.Data.SqlClient;
using Icson.Utils;
using Icson.Objects.Sale;

namespace Icson.DBAccess.Sale
{
	/// <summary>
	/// Summary description for SOSaleRuleDac.
	/// </summary>
	public class SOSaleRuleDac
	{
		public SOSaleRuleDac()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		
		public int Insert(SOSaleRuleInfo oParam)
		{
			string sql = @"INSERT INTO SO_SaleRule
                            (
                            SOSysNo, SaleRuleSysNo, SaleRuleName, 
                            Discount, Times, Note
                            )
                            VALUES (
                            @SOSysNo, @SaleRuleSysNo, @SaleRuleName, 
                            @Discount, @Times, @Note
                            )";
			
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int,4);
			SqlParameter paramSaleRuleSysNo = new SqlParameter("@SaleRuleSysNo", SqlDbType.Int,4);
			SqlParameter paramSaleRuleName = new SqlParameter("@SaleRuleName", SqlDbType.NVarChar,500);
			SqlParameter paramDiscount = new SqlParameter("@Discount", SqlDbType.Decimal,9);
			SqlParameter paramTimes = new SqlParameter("@Times", SqlDbType.Int,4);
			SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.NVarChar,200);

			if ( oParam.SOSysNo != AppConst.IntNull)
				paramSOSysNo.Value = oParam.SOSysNo;
			else
				paramSOSysNo.Value = System.DBNull.Value;
			if ( oParam.SaleRuleSysNo != AppConst.IntNull)
				paramSaleRuleSysNo.Value = oParam.SaleRuleSysNo;
			else
				paramSaleRuleSysNo.Value = System.DBNull.Value;
			if ( oParam.SaleRuleName != AppConst.StringNull)
				paramSaleRuleName.Value = oParam.SaleRuleName;
			else
				paramSaleRuleName.Value = System.DBNull.Value;
			if ( oParam.Discount != AppConst.DecimalNull)
				paramDiscount.Value = oParam.Discount;
			else
				paramDiscount.Value = System.DBNull.Value;
			if ( oParam.Times != AppConst.IntNull)
				paramTimes.Value = oParam.Times;
			else
				paramTimes.Value = System.DBNull.Value;
			if ( oParam.Note != AppConst.StringNull)
				paramNote.Value = oParam.Note;
			else
				paramNote.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSOSysNo);
			cmd.Parameters.Add(paramSaleRuleSysNo);
			cmd.Parameters.Add(paramSaleRuleName);
			cmd.Parameters.Add(paramDiscount);
			cmd.Parameters.Add(paramTimes);
			cmd.Parameters.Add(paramNote);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public int Update(SOSaleRuleInfo oParam)
		{
			string sql = @"UPDATE SO_SaleRule SET 
                           SOSysNo=@SOSysNo, 
                           SaleRuleSysNo=@SaleRuleSysNo, SaleRuleName=@SaleRuleName, 
                           Discount=@Discount, Times=@Times, Memo=@Memo
                           WHERE SysNo=@SysNo";

			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int,4);
			SqlParameter paramSaleRuleSysNo = new SqlParameter("@SaleRuleSysNo", SqlDbType.Int,4);
			SqlParameter paramSaleRuleName = new SqlParameter("@SaleRuleName", SqlDbType.NVarChar,500);
			SqlParameter paramDiscount = new SqlParameter("@Discount", SqlDbType.Decimal,9);
			SqlParameter paramTimes = new SqlParameter("@Times", SqlDbType.Int,4);
			SqlParameter paramMemo = new SqlParameter("@Memo", SqlDbType.NVarChar,200);

			if ( oParam.SysNo != AppConst.IntNull)
				paramSysNo.Value = oParam.SysNo;
			else
				paramSysNo.Value = System.DBNull.Value;
			if ( oParam.SOSysNo != AppConst.IntNull)
				paramSOSysNo.Value = oParam.SOSysNo;
			else
				paramSOSysNo.Value = System.DBNull.Value;
			if ( oParam.SaleRuleSysNo != AppConst.IntNull)
				paramSaleRuleSysNo.Value = oParam.SaleRuleSysNo;
			else
				paramSaleRuleSysNo.Value = System.DBNull.Value;
			if ( oParam.SaleRuleName != AppConst.StringNull)
				paramSaleRuleName.Value = oParam.SaleRuleName;
			else
				paramSaleRuleName.Value = System.DBNull.Value;
			if ( oParam.Discount != AppConst.DecimalNull)
				paramDiscount.Value = oParam.Discount;
			else
				paramDiscount.Value = System.DBNull.Value;
			if ( oParam.Times != AppConst.IntNull)
				paramTimes.Value = oParam.Times;
			else
				paramTimes.Value = System.DBNull.Value;
			if ( oParam.Note != AppConst.StringNull)
				paramMemo.Value = oParam.Note;
			else
				paramMemo.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramSOSysNo);
			cmd.Parameters.Add(paramSaleRuleSysNo);
			cmd.Parameters.Add(paramSaleRuleName);
			cmd.Parameters.Add(paramDiscount);
			cmd.Parameters.Add(paramTimes);
			cmd.Parameters.Add(paramMemo);

			return SqlHelper.ExecuteNonQuery(cmd);
		}
	}
}
