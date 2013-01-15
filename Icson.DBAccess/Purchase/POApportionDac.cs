using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Text;

using Icson.Utils;
using Icson.Objects.Purchase;

namespace Icson.DBAccess.Purchase
{
	/// <summary>
	/// Summary description for POApportionDac.
	/// </summary>
	public class POApportionDac
	{
		
		public POApportionDac()
		{
		}
		public int Insert(POApportionInfo oParam)
		{
			string sql = @"INSERT INTO PO_Apportion
                            (
                            SysNo,POSysNo, ApportionSubjectSysNo, ApportionType, 
                            ExpenseAmt
                            )
                            VALUES (
                            @SysNo,@POSysNo, @ApportionSubjectSysNo, @ApportionType, 
                            @ExpenseAmt
                            )";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramPOSysNo = new SqlParameter("@POSysNo", SqlDbType.Int,4);
			SqlParameter paramApportionSubjectSysNo = new SqlParameter("@ApportionSubjectSysNo", SqlDbType.Int,4);
			SqlParameter paramApportionType = new SqlParameter("@ApportionType", SqlDbType.Int,4);
			SqlParameter paramExpenseAmt = new SqlParameter("@ExpenseAmt", SqlDbType.Decimal,9);

			if ( oParam.SysNo != AppConst.IntNull)
				paramSysNo.Value = oParam.SysNo;
			else
				paramSysNo.Value = System.DBNull.Value;
			if ( oParam.POSysNo != AppConst.IntNull)
				paramPOSysNo.Value = oParam.POSysNo;
			else
				paramPOSysNo.Value = System.DBNull.Value;
			if ( oParam.ApportionSubjectSysNo != AppConst.IntNull)
				paramApportionSubjectSysNo.Value = oParam.ApportionSubjectSysNo;
			else
				paramApportionSubjectSysNo.Value = System.DBNull.Value;
			if ( oParam.ApportionType != AppConst.IntNull)
				paramApportionType.Value = oParam.ApportionType;
			else
				paramApportionType.Value = System.DBNull.Value;
			if ( oParam.ExpenseAmt != AppConst.DecimalNull)
				paramExpenseAmt.Value = oParam.ExpenseAmt;
			else
				paramExpenseAmt.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramPOSysNo);
			cmd.Parameters.Add(paramApportionSubjectSysNo);
			cmd.Parameters.Add(paramApportionType);
			cmd.Parameters.Add(paramExpenseAmt);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public int UpdateMaster(Hashtable paramHash)
		{
			StringBuilder sb = new StringBuilder(200);
			sb.Append("UPDATE PO_Apportion SET ");

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
		public int InsertItem(POApportionItemInfo oParam)
		{
			string sql = @"INSERT INTO PO_Apportion_Item
                            (
                            ApportionSysNo, ProductSysNo
                            )
                            VALUES (
                            @ApportionSysNo, @ProductSysNo
                            );set @SysNo = SCOPE_IDENTITY();";

			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramApportionSysNo = new SqlParameter("@ApportionSysNo", SqlDbType.Int,4);
			SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int,4);

			paramSysNo.Direction = ParameterDirection.Output;

			if ( oParam.ApportionSysNo != AppConst.IntNull)
				paramApportionSysNo.Value = oParam.ApportionSysNo;
			else
				paramApportionSysNo.Value = System.DBNull.Value;
			if ( oParam.ProductSysNo != AppConst.IntNull)
				paramProductSysNo.Value = oParam.ProductSysNo;
			else
				paramProductSysNo.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramApportionSysNo);
			cmd.Parameters.Add(paramProductSysNo);

			return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);

		}
		public int DeleteItem(int apportionSysNo, int productSysNo)
		{
			string sql = "delete from po_apportion_item where apportionsysno = " + apportionSysNo 
				+ " and productSysNo = " + productSysNo;
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);
		}
		public int DeleteMaster(int sysno)
		{
			string sql = "delete from po_apportion where sysno =" + sysno;
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);
		}
	}
}
