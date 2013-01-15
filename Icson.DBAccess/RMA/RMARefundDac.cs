using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Text;

using Icson.Utils;
using Icson.Objects.RMA;

namespace Icson.DBAccess.RMA
{
	/// <summary>
	/// Summary description for RMARefundDac.
	/// </summary>
	public class RMARefundDac
	{
		
		public RMARefundDac()
		{
		}

		public int InsertMaster(RMARefundInfo oParam)
		{
			string sql = @"INSERT INTO rma_refund
                            (
                            SysNo, RefundID, SOSysNo, CustomerSysNo, 
                            CreateTime, CreateUserSysNo, AuditTime, AuditUserSysNo, 
                            RefundTime, RefundUserSysNo, CompensateShipPrice, SOCashPointRate, 
                            OrgCashAmt, OrgPointAmt, DeductPointFromAccount, DeductPointFromCurrentCash, 
                            CashAmt, PointAmt, RefundPayType, Note, 
                            Status
                            )
                            VALUES (
                            @SysNo, @RefundID, @SOSysNo, @CustomerSysNo, 
                            @CreateTime, @CreateUserSysNo, @AuditTime, @AuditUserSysNo, 
                            @RefundTime, @RefundUserSysNo, @CompensateShipPrice, @SOCashPointRate, 
                            @OrgCashAmt, @OrgPointAmt, @DeductPointFromAccount, @DeductPointFromCurrentCash, 
                            @CashAmt, @PointAmt, @RefundPayType, @Note, 
                            @Status
                            )";
			
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramRefundID = new SqlParameter("@RefundID", SqlDbType.NVarChar,10);
			SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int,4);
			SqlParameter paramCustomerSysNo = new SqlParameter("@CustomerSysNo", SqlDbType.Int,4);
			SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
			SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int,4);
			SqlParameter paramAuditTime = new SqlParameter("@AuditTime", SqlDbType.DateTime);
			SqlParameter paramAuditUserSysNo = new SqlParameter("@AuditUserSysNo", SqlDbType.Int,4);
			SqlParameter paramRefundTime = new SqlParameter("@RefundTime", SqlDbType.DateTime);
			SqlParameter paramRefundUserSysNo = new SqlParameter("@RefundUserSysNo", SqlDbType.Int,4);
			SqlParameter paramCompensateShipPrice = new SqlParameter("@CompensateShipPrice", SqlDbType.Decimal,9);
			SqlParameter paramSOCashPointRate = new SqlParameter("@SOCashPointRate", SqlDbType.Decimal,9);
			SqlParameter paramOrgCashAmt = new SqlParameter("@OrgCashAmt", SqlDbType.Decimal,9);
			SqlParameter paramOrgPointAmt = new SqlParameter("@OrgPointAmt", SqlDbType.Int,4);
			SqlParameter paramDeductPointFromAccount = new SqlParameter("@DeductPointFromAccount", SqlDbType.Int,4);
			SqlParameter paramDeductPointFromCurrentCash = new SqlParameter("@DeductPointFromCurrentCash", SqlDbType.Decimal,9);
			SqlParameter paramCashAmt = new SqlParameter("@CashAmt", SqlDbType.Decimal,9);
			SqlParameter paramPointAmt = new SqlParameter("@PointAmt", SqlDbType.Int,4);
            SqlParameter paramRefundPayType = new SqlParameter("@RefundPayType", SqlDbType.Int, 4);
			SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.NVarChar,500);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);

			if ( oParam.SysNo != AppConst.IntNull)
				paramSysNo.Value = oParam.SysNo;
			else
				paramSysNo.Value = System.DBNull.Value;
			if ( oParam.RefundID != AppConst.StringNull)
				paramRefundID.Value = oParam.RefundID;
			else
				paramRefundID.Value = System.DBNull.Value;
			if ( oParam.SOSysNo != AppConst.IntNull)
				paramSOSysNo.Value = oParam.SOSysNo;
			else
				paramSOSysNo.Value = System.DBNull.Value;
			if ( oParam.CustomerSysNo != AppConst.IntNull)
				paramCustomerSysNo.Value = oParam.CustomerSysNo;
			else
				paramCustomerSysNo.Value = System.DBNull.Value;
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
			if ( oParam.RefundTime != AppConst.DateTimeNull)
				paramRefundTime.Value = oParam.RefundTime;
			else
				paramRefundTime.Value = System.DBNull.Value;
			if ( oParam.RefundUserSysNo != AppConst.IntNull)
				paramRefundUserSysNo.Value = oParam.RefundUserSysNo;
			else
				paramRefundUserSysNo.Value = System.DBNull.Value;
			if ( oParam.CompensateShipPrice != AppConst.DecimalNull)
				paramCompensateShipPrice.Value = oParam.CompensateShipPrice;
			else
				paramCompensateShipPrice.Value = System.DBNull.Value;
			if ( oParam.SOCashPointRate != AppConst.DecimalNull)
				paramSOCashPointRate.Value = oParam.SOCashPointRate;
			else
				paramSOCashPointRate.Value = System.DBNull.Value;
			if ( oParam.OrgCashAmt != AppConst.DecimalNull)
				paramOrgCashAmt.Value = oParam.OrgCashAmt;
			else
				paramOrgCashAmt.Value = System.DBNull.Value;
			if ( oParam.OrgPointAmt != AppConst.IntNull)
				paramOrgPointAmt.Value = oParam.OrgPointAmt;
			else
				paramOrgPointAmt.Value = System.DBNull.Value;
			if ( oParam.DeductPointFromAccount != AppConst.IntNull)
				paramDeductPointFromAccount.Value = oParam.DeductPointFromAccount;
			else
				paramDeductPointFromAccount.Value = System.DBNull.Value;
			if ( oParam.DeductPointFromCurrentCash != AppConst.DecimalNull)
				paramDeductPointFromCurrentCash.Value = oParam.DeductPointFromCurrentCash;
			else
				paramDeductPointFromCurrentCash.Value = System.DBNull.Value;
			if ( oParam.CashAmt != AppConst.DecimalNull)
				paramCashAmt.Value = oParam.CashAmt;
			else
				paramCashAmt.Value = System.DBNull.Value;
			if ( oParam.PointAmt != AppConst.IntNull)
				paramPointAmt.Value = oParam.PointAmt;
			else
				paramPointAmt.Value = System.DBNull.Value;
            if (oParam.RefundPayType != AppConst.IntNull)
                paramRefundPayType.Value = oParam.RefundPayType;
			else
                paramRefundPayType.Value = System.DBNull.Value;
			if ( oParam.Note != AppConst.StringNull)
				paramNote.Value = oParam.Note;
			else
				paramNote.Value = System.DBNull.Value;
			if ( oParam.Status != AppConst.IntNull)
				paramStatus.Value = oParam.Status;
			else
				paramStatus.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramRefundID);
			cmd.Parameters.Add(paramSOSysNo);
			cmd.Parameters.Add(paramCustomerSysNo);
			cmd.Parameters.Add(paramCreateTime);
			cmd.Parameters.Add(paramCreateUserSysNo);
			cmd.Parameters.Add(paramAuditTime);
			cmd.Parameters.Add(paramAuditUserSysNo);
			cmd.Parameters.Add(paramRefundTime);
			cmd.Parameters.Add(paramRefundUserSysNo);
			cmd.Parameters.Add(paramCompensateShipPrice);
			cmd.Parameters.Add(paramSOCashPointRate);
			cmd.Parameters.Add(paramOrgCashAmt);
			cmd.Parameters.Add(paramOrgPointAmt);
			cmd.Parameters.Add(paramDeductPointFromAccount);
			cmd.Parameters.Add(paramDeductPointFromCurrentCash);
			cmd.Parameters.Add(paramCashAmt);
			cmd.Parameters.Add(paramPointAmt);
            cmd.Parameters.Add(paramRefundPayType);
			cmd.Parameters.Add(paramNote);
			cmd.Parameters.Add(paramStatus);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public int UpdateMaster(Hashtable paramHash)
		{
			StringBuilder sb = new StringBuilder(200);
			sb.Append("UPDATE RMA_Refund SET ");

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
						if ( (int)item == AppConst.IntNull)
							sb.Append(key).Append("= null");
						else
							sb.Append(key).Append("=").Append(item.ToString());
					}
					else if ( item is decimal )
					{
						if ( (decimal)item == AppConst.DecimalNull )
							sb.Append(key).Append("= null");
						else
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

		public int InsertItem(RMARefundItemInfo oParam)
		{
			string sql = @"INSERT INTO rma_refund_item
                            (
                            RefundSysNo, RegisterSysNo, OrgPrice, 
                            UnitDiscount, ProductValue, OrgPoint, RefundPrice, 
                            PointType, RefundCash, RefundPoint , RefundPriceType , RefundCost , RefundCostPoint
                            )
                            VALUES (
                            @RefundSysNo, @RegisterSysNo, @OrgPrice, 
                            @UnitDiscount, @ProductValue, @OrgPoint, @RefundPrice, 
                            @PointType, @RefundCash, @RefundPoint , @RefundPriceType , @RefundCost , @RefundCostPoint
                            );set @SysNo = SCOPE_IDENTITY();";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramRefundSysNo = new SqlParameter("@RefundSysNo", SqlDbType.Int,4);
			SqlParameter paramRegisterSysNo = new SqlParameter("@RegisterSysNo", SqlDbType.Int,4);
			SqlParameter paramOrgPrice = new SqlParameter("@OrgPrice", SqlDbType.Decimal,9);
			SqlParameter paramUnitDiscount = new SqlParameter("@UnitDiscount", SqlDbType.Decimal,9);
			SqlParameter paramProductValue = new SqlParameter("@ProductValue", SqlDbType.Decimal,9);
			SqlParameter paramOrgPoint = new SqlParameter("@OrgPoint", SqlDbType.Int,4);
			SqlParameter paramRefundPrice = new SqlParameter("@RefundPrice", SqlDbType.Decimal,9);
			SqlParameter paramPointType = new SqlParameter("@PointType", SqlDbType.Int,4);
			SqlParameter paramRefundCash = new SqlParameter("@RefundCash", SqlDbType.Decimal,9);
			SqlParameter paramRefundPoint = new SqlParameter("@RefundPoint", SqlDbType.Int,4);
			SqlParameter paramRefundPriceType = new SqlParameter("@RefundPriceType", SqlDbType.Int,4);
			SqlParameter paramRefundCost = new SqlParameter("@RefundCost", SqlDbType.Decimal,9);
			SqlParameter paramRefundCostPoint = new SqlParameter("@RefundCostPoint", SqlDbType.Int,4);

			paramSysNo.Direction = ParameterDirection.Output;

			if ( oParam.RefundSysNo != AppConst.IntNull)
				paramRefundSysNo.Value = oParam.RefundSysNo;
			else
				paramRefundSysNo.Value = System.DBNull.Value;
			if ( oParam.RegisterSysNo != AppConst.IntNull)
				paramRegisterSysNo.Value = oParam.RegisterSysNo;
			else
				paramRegisterSysNo.Value = System.DBNull.Value;
			if ( oParam.OrgPrice != AppConst.DecimalNull)
				paramOrgPrice.Value = oParam.OrgPrice;
			else
				paramOrgPrice.Value = System.DBNull.Value;
			if ( oParam.UnitDiscount != AppConst.DecimalNull)
				paramUnitDiscount.Value = oParam.UnitDiscount;
			else
				paramUnitDiscount.Value = System.DBNull.Value;
			if ( oParam.ProductValue != AppConst.DecimalNull)
				paramProductValue.Value = oParam.ProductValue;
			else
				paramProductValue.Value = System.DBNull.Value;
			if ( oParam.OrgPoint != AppConst.IntNull)
				paramOrgPoint.Value = oParam.OrgPoint;
			else
				paramOrgPoint.Value = System.DBNull.Value;
			if ( oParam.RefundPrice != AppConst.DecimalNull)
				paramRefundPrice.Value = oParam.RefundPrice;
			else
				paramRefundPrice.Value = System.DBNull.Value;
			if ( oParam.PointType != AppConst.IntNull)
				paramPointType.Value = oParam.PointType;
			else
				paramPointType.Value = System.DBNull.Value;
			if ( oParam.RefundCash != AppConst.DecimalNull)
				paramRefundCash.Value = oParam.RefundCash;
			else
				paramRefundCash.Value = System.DBNull.Value;
			if ( oParam.RefundPoint != AppConst.IntNull)
				paramRefundPoint.Value = oParam.RefundPoint;
			else
				paramRefundPoint.Value = System.DBNull.Value;

			if ( oParam.RefundPriceType != AppConst.IntNull)
				paramRefundPriceType.Value = oParam.RefundPriceType;
			else
				paramRefundPriceType.Value = System.DBNull.Value;

			if ( oParam.RefundCost != AppConst.DecimalNull)
				paramRefundCost.Value = oParam.RefundCost;
			else
				paramRefundCost.Value = System.DBNull.Value;

			if ( oParam.RefundCostPoint != AppConst.IntNull)
				paramRefundCostPoint.Value = oParam.RefundCostPoint;
			else
				paramRefundCostPoint.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramRefundSysNo);
			cmd.Parameters.Add(paramRegisterSysNo);
			cmd.Parameters.Add(paramOrgPrice);
			cmd.Parameters.Add(paramUnitDiscount);
			cmd.Parameters.Add(paramProductValue);
			cmd.Parameters.Add(paramOrgPoint);
			cmd.Parameters.Add(paramRefundPrice);
			cmd.Parameters.Add(paramPointType);
			cmd.Parameters.Add(paramRefundCash);
			cmd.Parameters.Add(paramRefundPoint);
			cmd.Parameters.Add(paramRefundPriceType);
			cmd.Parameters.Add(paramRefundCost);
			cmd.Parameters.Add(paramRefundCostPoint);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public int UpdateItem(Hashtable paramHash)
		{
			StringBuilder sb = new StringBuilder(200);
			sb.Append("UPDATE RMA_Refund_Item SET ");

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
						if ( (int)item == AppConst.IntNull)
							sb.Append(key).Append("= null");
						else
							sb.Append(key).Append("=").Append(item.ToString());
					}
					else if ( item is decimal )
					{
						if ( (decimal)item == AppConst.DecimalNull )
							sb.Append(key).Append("= null");
						else
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

		public int DeleteItem(int sysno)
		{
			string sql = "delete from rma_refund_item where sysno = " + sysno;
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);
		}
	}
}
