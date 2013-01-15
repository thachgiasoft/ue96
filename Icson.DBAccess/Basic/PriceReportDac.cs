using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

using Icson.Objects.Basic;
using Icson.Utils;

namespace Icson.DBAccess.Basic
{
    public class PriceReportDac
    {
        public int Insert(PriceReportInfo oParam)
        {
            string sql = @"INSERT INTO Price_Report
                            (
                            ProductSysNo, CurrentPrice, UnitCost, LastOrderPrice, 
                            CompetitorSysNo, CompetitorPrice, CompetitorUrl, CustomerSysNo, 
                            NickName, Email, CustomerMemo, ReportTime, 
                            CustomerIP, AuditUserSysNo, AuditMemo, AuditNote, 
                            AuditTime, AbandonUserSysNo, AbandonTime, Point, 
                            HandleType, Reason, Status
                            )
                            VALUES (
                            @ProductSysNo, @CurrentPrice, @UnitCost, @LastOrderPrice, 
                            @CompetitorSysNo, @CompetitorPrice, @CompetitorUrl, @CustomerSysNo, 
                            @NickName, @Email, @CustomerMemo, @ReportTime, 
                            @CustomerIP, @AuditUserSysNo, @AuditMemo, @AuditNote, 
                            @AuditTime, @AbandonUserSysNo, @AbandonTime, @Point, 
                            @HandleType, @Reason, @Status
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int, 4);
            SqlParameter paramCurrentPrice = new SqlParameter("@CurrentPrice", SqlDbType.Decimal, 9);
            SqlParameter paramUnitCost = new SqlParameter("@UnitCost", SqlDbType.Decimal, 9);
            SqlParameter paramLastOrderPrice = new SqlParameter("@LastOrderPrice", SqlDbType.Decimal, 9);
            SqlParameter paramCompetitorSysNo = new SqlParameter("@CompetitorSysNo", SqlDbType.Int, 4);
            SqlParameter paramCompetitorPrice = new SqlParameter("@CompetitorPrice", SqlDbType.Decimal, 9);
            SqlParameter paramCompetitorUrl = new SqlParameter("@CompetitorUrl", SqlDbType.NVarChar, 500);
            SqlParameter paramCustomerSysNo = new SqlParameter("@CustomerSysNo", SqlDbType.Int, 4);
            SqlParameter paramNickName = new SqlParameter("@NickName", SqlDbType.NVarChar, 100);
            SqlParameter paramEmail = new SqlParameter("@Email", SqlDbType.NVarChar, 100);
            SqlParameter paramCustomerMemo = new SqlParameter("@CustomerMemo", SqlDbType.NVarChar, 500);
            SqlParameter paramReportTime = new SqlParameter("@ReportTime", SqlDbType.DateTime);
            SqlParameter paramCustomerIP = new SqlParameter("@CustomerIP", SqlDbType.NVarChar, 50);
            SqlParameter paramAuditUserSysNo = new SqlParameter("@AuditUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramAuditMemo = new SqlParameter("@AuditMemo", SqlDbType.NVarChar, 200);
            SqlParameter paramAuditNote = new SqlParameter("@AuditNote", SqlDbType.NVarChar, 200);
            SqlParameter paramAuditTime = new SqlParameter("@AuditTime", SqlDbType.DateTime);
            SqlParameter paramAbandonUserSysNo = new SqlParameter("@AbandonUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramAbandonTime = new SqlParameter("@AbandonTime", SqlDbType.DateTime);
            SqlParameter paramPoint = new SqlParameter("@Point", SqlDbType.Int, 4);
            SqlParameter paramHandleType = new SqlParameter("@HandleType", SqlDbType.Int, 4);
            SqlParameter paramReason = new SqlParameter("@Reason", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.ProductSysNo != AppConst.IntNull)
                paramProductSysNo.Value = oParam.ProductSysNo;
            else
                paramProductSysNo.Value = System.DBNull.Value;
            if (oParam.CurrentPrice != AppConst.DecimalNull)
                paramCurrentPrice.Value = oParam.CurrentPrice;
            else
                paramCurrentPrice.Value = System.DBNull.Value;
            if (oParam.UnitCost != AppConst.DecimalNull)
                paramUnitCost.Value = oParam.UnitCost;
            else
                paramUnitCost.Value = System.DBNull.Value;
            if (oParam.LastOrderPrice != AppConst.DecimalNull)
                paramLastOrderPrice.Value = oParam.LastOrderPrice;
            else
                paramLastOrderPrice.Value = System.DBNull.Value;
            if (oParam.CompetitorSysNo != AppConst.IntNull)
                paramCompetitorSysNo.Value = oParam.CompetitorSysNo;
            else
                paramCompetitorSysNo.Value = System.DBNull.Value;
            if (oParam.CompetitorPrice != AppConst.DecimalNull)
                paramCompetitorPrice.Value = oParam.CompetitorPrice;
            else
                paramCompetitorPrice.Value = System.DBNull.Value;
            if (oParam.CompetitorUrl != AppConst.StringNull)
                paramCompetitorUrl.Value = oParam.CompetitorUrl;
            else
                paramCompetitorUrl.Value = System.DBNull.Value;
            if (oParam.CustomerSysNo != AppConst.IntNull)
                paramCustomerSysNo.Value = oParam.CustomerSysNo;
            else
                paramCustomerSysNo.Value = System.DBNull.Value;
            if (oParam.NickName != AppConst.StringNull)
                paramNickName.Value = oParam.NickName;
            else
                paramNickName.Value = System.DBNull.Value;
            if (oParam.Email != AppConst.StringNull)
                paramEmail.Value = oParam.Email;
            else
                paramEmail.Value = System.DBNull.Value;
            if (oParam.CustomerMemo != AppConst.StringNull)
                paramCustomerMemo.Value = oParam.CustomerMemo;
            else
                paramCustomerMemo.Value = System.DBNull.Value;
            if (oParam.ReportTime != AppConst.DateTimeNull)
                paramReportTime.Value = oParam.ReportTime;
            else
                paramReportTime.Value = System.DBNull.Value;
            if (oParam.CustomerIP != AppConst.StringNull)
                paramCustomerIP.Value = oParam.CustomerIP;
            else
                paramCustomerIP.Value = System.DBNull.Value;
            if (oParam.AuditUserSysNo != AppConst.IntNull)
                paramAuditUserSysNo.Value = oParam.AuditUserSysNo;
            else
                paramAuditUserSysNo.Value = System.DBNull.Value;
            if (oParam.AuditMemo != AppConst.StringNull)
                paramAuditMemo.Value = oParam.AuditMemo;
            else
                paramAuditMemo.Value = System.DBNull.Value;
            if (oParam.AuditNote != AppConst.StringNull)
                paramAuditNote.Value = oParam.AuditNote;
            else
                paramAuditNote.Value = System.DBNull.Value;
            if (oParam.AuditTime != AppConst.DateTimeNull)
                paramAuditTime.Value = oParam.AuditTime;
            else
                paramAuditTime.Value = System.DBNull.Value;
            if (oParam.AbandonUserSysNo != AppConst.IntNull)
                paramAbandonUserSysNo.Value = oParam.AbandonUserSysNo;
            else
                paramAbandonUserSysNo.Value = System.DBNull.Value;
            if (oParam.AbandonTime != AppConst.DateTimeNull)
                paramAbandonTime.Value = oParam.AbandonTime;
            else
                paramAbandonTime.Value = System.DBNull.Value;
            if (oParam.Point != AppConst.IntNull)
                paramPoint.Value = oParam.Point;
            else
                paramPoint.Value = System.DBNull.Value;
            if (oParam.HandleType != AppConst.IntNull)
                paramHandleType.Value = oParam.HandleType;
            else
                paramHandleType.Value = System.DBNull.Value;
            if (oParam.Reason != AppConst.IntNull)
                paramReason.Value = oParam.Reason;
            else
                paramReason.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramProductSysNo);
            cmd.Parameters.Add(paramCurrentPrice);
            cmd.Parameters.Add(paramUnitCost);
            cmd.Parameters.Add(paramLastOrderPrice);
            cmd.Parameters.Add(paramCompetitorSysNo);
            cmd.Parameters.Add(paramCompetitorPrice);
            cmd.Parameters.Add(paramCompetitorUrl);
            cmd.Parameters.Add(paramCustomerSysNo);
            cmd.Parameters.Add(paramNickName);
            cmd.Parameters.Add(paramEmail);
            cmd.Parameters.Add(paramCustomerMemo);
            cmd.Parameters.Add(paramReportTime);
            cmd.Parameters.Add(paramCustomerIP);
            cmd.Parameters.Add(paramAuditUserSysNo);
            cmd.Parameters.Add(paramAuditMemo);
            cmd.Parameters.Add(paramAuditNote);
            cmd.Parameters.Add(paramAuditTime);
            cmd.Parameters.Add(paramAbandonUserSysNo);
            cmd.Parameters.Add(paramAbandonTime);
            cmd.Parameters.Add(paramPoint);
            cmd.Parameters.Add(paramHandleType);
            cmd.Parameters.Add(paramReason);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Update(Hashtable paramHash)
        {
            StringBuilder sb = new StringBuilder(200);
            sb.Append("UPDATE price_report SET ");

            if (paramHash != null && paramHash.Count != 0)
            {
                int index = 0;
                foreach (string key in paramHash.Keys)
                {
                    object item = paramHash[key];
                    if (key.ToLower() == "sysno")
                        continue;

                    if (index != 0)
                        sb.Append(",");
                    index++;

                    if (item is int || item is decimal)
                    {
                        sb.Append(key).Append("=").Append(item.ToString());
                    }
                    else if (item is string)
                    {
                        sb.Append(key).Append(" = ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (item is DateTime)
                    {
                        sb.Append(key).Append(" = cast(").Append(Util.ToSqlString(((DateTime)item).ToString(AppConst.DateFormatLong))).Append(" as DateTime)");
                    }
                }
            }

            sb.Append(" WHERE SysNo=").Append(paramHash["SysNo"]);

            return SqlHelper.ExecuteNonQuery(sb.ToString());
        }
    }
}