using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using Icson.Objects.RMA;
using Icson.Utils;

namespace Icson.DBAccess.RMA
{
   public class PreSaleRefundDac
    {
       public int Insert(PreSaleRefundInfo oParam)
       {
           string sql = @"INSERT INTO PreSale_Refund
                            (
                            SysNo, RefundID, SOSysNo, CustomerSysNo, 
                            CreateTime, CreateUserSysNo, AuditTime, AuditUserSysNo, 
                            RefundTime, RefundUserSysNo, UpdateUserSysNo, UpdateTime, 
                            RefundPayType, RefundAmt, RefundCause, RefundCauseNote, 
                            CustomerAccNote, RefundNote, Status, VoucherID, 
                            VoucherTime, ACCAuditTime, ACCAuditUserSysNo
                            )
                            VALUES (
                            @SysNo, @RefundID, @SOSysNo, @CustomerSysNo, 
                            @CreateTime, @CreateUserSysNo, @AuditTime, @AuditUserSysNo, 
                            @RefundTime, @RefundUserSysNo, @UpdateUserSysNo, @UpdateTime, 
                            @RefundPayType, @RefundAmt, @RefundCause, @RefundCauseNote, 
                            @CustomerAccNote, @RefundNote, @Status, @VoucherID, 
                            @VoucherTime, @ACCAuditTime, @ACCAuditUserSysNo
                            )";
           SqlCommand cmd = new SqlCommand(sql);

           SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
           SqlParameter paramRefundID = new SqlParameter("@RefundID", SqlDbType.NVarChar, 50);
           SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int, 4);
           SqlParameter paramCustomerSysNo = new SqlParameter("@CustomerSysNo", SqlDbType.Int, 4);
           SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
           SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
           SqlParameter paramAuditTime = new SqlParameter("@AuditTime", SqlDbType.DateTime);
           SqlParameter paramAuditUserSysNo = new SqlParameter("@AuditUserSysNo", SqlDbType.Int, 4);
           SqlParameter paramRefundTime = new SqlParameter("@RefundTime", SqlDbType.DateTime);
           SqlParameter paramRefundUserSysNo = new SqlParameter("@RefundUserSysNo", SqlDbType.Int, 4);
           SqlParameter paramUpdateUserSysNo = new SqlParameter("@UpdateUserSysNo", SqlDbType.Int, 4);
           SqlParameter paramUpdateTime = new SqlParameter("@UpdateTime", SqlDbType.DateTime);
           SqlParameter paramRefundPayType = new SqlParameter("@RefundPayType", SqlDbType.Int, 4);
           SqlParameter paramRefundAmt = new SqlParameter("@RefundAmt", SqlDbType.Decimal, 9);
           SqlParameter paramRefundCause = new SqlParameter("@RefundCause", SqlDbType.Int, 4);
           SqlParameter paramRefundCauseNote = new SqlParameter("@RefundCauseNote", SqlDbType.NVarChar, 500);
           SqlParameter paramCustomerAccNote = new SqlParameter("@CustomerAccNote", SqlDbType.NVarChar, 300);
           SqlParameter paramRefundNote = new SqlParameter("@RefundNote", SqlDbType.NVarChar, 500);
           SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
           SqlParameter paramVoucherID = new SqlParameter("@VoucherID", SqlDbType.NVarChar, 50);
           SqlParameter paramVoucherTime = new SqlParameter("@VoucherTime", SqlDbType.DateTime);
           SqlParameter paramACCAuditTime = new SqlParameter("@ACCAuditTime", SqlDbType.DateTime);
           SqlParameter paramACCAuditUserSysNo = new SqlParameter("@ACCAuditUserSysNo", SqlDbType.Int, 4);

           if (oParam.SysNo != AppConst.IntNull)
               paramSysNo.Value = oParam.SysNo;
           else
               paramSysNo.Value = System.DBNull.Value;
           if (oParam.RefundID != AppConst.StringNull)
               paramRefundID.Value = oParam.RefundID;
           else
               paramRefundID.Value = System.DBNull.Value;
           if (oParam.SOSysNo != AppConst.IntNull)
               paramSOSysNo.Value = oParam.SOSysNo;
           else
               paramSOSysNo.Value = System.DBNull.Value;
           if (oParam.CustomerSysNo != AppConst.IntNull)
               paramCustomerSysNo.Value = oParam.CustomerSysNo;
           else
               paramCustomerSysNo.Value = System.DBNull.Value;
           if (oParam.CreateTime != AppConst.DateTimeNull)
               paramCreateTime.Value = oParam.CreateTime;
           else
               paramCreateTime.Value = System.DBNull.Value;
           if (oParam.CreateUserSysNo != AppConst.IntNull)
               paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
           else
               paramCreateUserSysNo.Value = System.DBNull.Value;
           if (oParam.AuditTime != AppConst.DateTimeNull)
               paramAuditTime.Value = oParam.AuditTime;
           else
               paramAuditTime.Value = System.DBNull.Value;
           if (oParam.AuditUserSysNo != AppConst.IntNull)
               paramAuditUserSysNo.Value = oParam.AuditUserSysNo;
           else
               paramAuditUserSysNo.Value = System.DBNull.Value;
           if (oParam.RefundTime != AppConst.DateTimeNull)
               paramRefundTime.Value = oParam.RefundTime;
           else
               paramRefundTime.Value = System.DBNull.Value;
           if (oParam.RefundUserSysNo != AppConst.IntNull)
               paramRefundUserSysNo.Value = oParam.RefundUserSysNo;
           else
               paramRefundUserSysNo.Value = System.DBNull.Value;
           if (oParam.UpdateUserSysNo != AppConst.IntNull)
               paramUpdateUserSysNo.Value = oParam.UpdateUserSysNo;
           else
               paramUpdateUserSysNo.Value = System.DBNull.Value;
           if (oParam.UpdateTime != AppConst.DateTimeNull)
               paramUpdateTime.Value = oParam.UpdateTime;
           else
               paramUpdateTime.Value = System.DBNull.Value;
           if (oParam.RefundPayType != AppConst.IntNull)
               paramRefundPayType.Value = oParam.RefundPayType;
           else
               paramRefundPayType.Value = System.DBNull.Value;
           if (oParam.RefundAmt != AppConst.DecimalNull)
               paramRefundAmt.Value = oParam.RefundAmt;
           else
               paramRefundAmt.Value = System.DBNull.Value;
           if (oParam.RefundCause != AppConst.IntNull)
               paramRefundCause.Value = oParam.RefundCause;
           else
               paramRefundCause.Value = System.DBNull.Value;
           if (oParam.RefundCauseNote != AppConst.StringNull)
               paramRefundCauseNote.Value = oParam.RefundCauseNote;
           else
               paramRefundCauseNote.Value = System.DBNull.Value;
           if (oParam.CustomerAccNote != AppConst.StringNull)
               paramCustomerAccNote.Value = oParam.CustomerAccNote;
           else
               paramCustomerAccNote.Value = System.DBNull.Value;
           if (oParam.RefundNote != AppConst.StringNull)
               paramRefundNote.Value = oParam.RefundNote;
           else
               paramRefundNote.Value = System.DBNull.Value;
           if (oParam.Status != AppConst.IntNull)
               paramStatus.Value = oParam.Status;
           else
               paramStatus.Value = System.DBNull.Value;
           if (oParam.VoucherID != AppConst.StringNull)
               paramVoucherID.Value = oParam.VoucherID;
           else
               paramVoucherID.Value = System.DBNull.Value;
           if (oParam.VoucherTime != AppConst.DateTimeNull)
               paramVoucherTime.Value = oParam.VoucherTime;
           else
               paramVoucherTime.Value = System.DBNull.Value;
           if (oParam.ACCAuditTime != AppConst.DateTimeNull)
               paramACCAuditTime.Value = oParam.ACCAuditTime;
           else
               paramACCAuditTime.Value = System.DBNull.Value;
           if (oParam.ACCAuditUserSysNo != AppConst.IntNull)
               paramACCAuditUserSysNo.Value = oParam.ACCAuditUserSysNo;
           else
               paramACCAuditUserSysNo.Value = System.DBNull.Value;

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
           cmd.Parameters.Add(paramUpdateUserSysNo);
           cmd.Parameters.Add(paramUpdateTime);
           cmd.Parameters.Add(paramRefundPayType);
           cmd.Parameters.Add(paramRefundAmt);
           cmd.Parameters.Add(paramRefundCause);
           cmd.Parameters.Add(paramRefundCauseNote);
           cmd.Parameters.Add(paramCustomerAccNote);
           cmd.Parameters.Add(paramRefundNote);
           cmd.Parameters.Add(paramStatus);
           cmd.Parameters.Add(paramVoucherID);
           cmd.Parameters.Add(paramVoucherTime);
           cmd.Parameters.Add(paramACCAuditTime);
           cmd.Parameters.Add(paramACCAuditUserSysNo);

           return SqlHelper.ExecuteNonQuery(cmd);
       }
       public int Update(PreSaleRefundInfo oParam)
       {
           string sql = @"UPDATE PreSale_Refund SET 
                            RefundID=@RefundID, SOSysNo=@SOSysNo, 
                            CustomerSysNo=@CustomerSysNo, CreateTime=@CreateTime, 
                            CreateUserSysNo=@CreateUserSysNo, AuditTime=@AuditTime, 
                            AuditUserSysNo=@AuditUserSysNo, RefundTime=@RefundTime, 
                            RefundUserSysNo=@RefundUserSysNo, UpdateUserSysNo=@UpdateUserSysNo, 
                            UpdateTime=@UpdateTime, RefundPayType=@RefundPayType, 
                            RefundAmt=@RefundAmt, RefundCause=@RefundCause, 
                            RefundCauseNote=@RefundCauseNote, CustomerAccNote=@CustomerAccNote, 
                            RefundNote=@RefundNote, Status=@Status, 
                            VoucherID=@VoucherID, VoucherTime=@VoucherTime, 
                            ACCAuditTime=@ACCAuditTime, ACCAuditUserSysNo=@ACCAuditUserSysNo
                            WHERE SysNo=@SysNo";
           SqlCommand cmd = new SqlCommand(sql);

           SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
           SqlParameter paramRefundID = new SqlParameter("@RefundID", SqlDbType.NVarChar, 50);
           SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int, 4);
           SqlParameter paramCustomerSysNo = new SqlParameter("@CustomerSysNo", SqlDbType.Int, 4);
           SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
           SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
           SqlParameter paramAuditTime = new SqlParameter("@AuditTime", SqlDbType.DateTime);
           SqlParameter paramAuditUserSysNo = new SqlParameter("@AuditUserSysNo", SqlDbType.Int, 4);
           SqlParameter paramRefundTime = new SqlParameter("@RefundTime", SqlDbType.DateTime);
           SqlParameter paramRefundUserSysNo = new SqlParameter("@RefundUserSysNo", SqlDbType.Int, 4);
           SqlParameter paramUpdateUserSysNo = new SqlParameter("@UpdateUserSysNo", SqlDbType.Int, 4);
           SqlParameter paramUpdateTime = new SqlParameter("@UpdateTime", SqlDbType.DateTime);
           SqlParameter paramRefundPayType = new SqlParameter("@RefundPayType", SqlDbType.Int, 4);
           SqlParameter paramRefundAmt = new SqlParameter("@RefundAmt", SqlDbType.Decimal, 9);
           SqlParameter paramRefundCause = new SqlParameter("@RefundCause", SqlDbType.Int, 4);
           SqlParameter paramRefundCauseNote = new SqlParameter("@RefundCauseNote", SqlDbType.NVarChar, 500);
           SqlParameter paramCustomerAccNote = new SqlParameter("@CustomerAccNote", SqlDbType.NVarChar, 300);
           SqlParameter paramRefundNote = new SqlParameter("@RefundNote", SqlDbType.NVarChar, 500);
           SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
           SqlParameter paramVoucherID = new SqlParameter("@VoucherID", SqlDbType.NVarChar, 50);
           SqlParameter paramVoucherTime = new SqlParameter("@VoucherTime", SqlDbType.DateTime);
           SqlParameter paramACCAuditTime = new SqlParameter("@ACCAuditTime", SqlDbType.DateTime);
           SqlParameter paramACCAuditUserSysNo = new SqlParameter("@ACCAuditUserSysNo", SqlDbType.Int, 4);

           if (oParam.SysNo != AppConst.IntNull)
               paramSysNo.Value = oParam.SysNo;
           else
               paramSysNo.Value = System.DBNull.Value;
           if (oParam.RefundID != AppConst.StringNull)
               paramRefundID.Value = oParam.RefundID;
           else
               paramRefundID.Value = System.DBNull.Value;
           if (oParam.SOSysNo != AppConst.IntNull)
               paramSOSysNo.Value = oParam.SOSysNo;
           else
               paramSOSysNo.Value = System.DBNull.Value;
           if (oParam.CustomerSysNo != AppConst.IntNull)
               paramCustomerSysNo.Value = oParam.CustomerSysNo;
           else
               paramCustomerSysNo.Value = System.DBNull.Value;
           if (oParam.CreateTime != AppConst.DateTimeNull)
               paramCreateTime.Value = oParam.CreateTime;
           else
               paramCreateTime.Value = System.DBNull.Value;
           if (oParam.CreateUserSysNo != AppConst.IntNull)
               paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
           else
               paramCreateUserSysNo.Value = System.DBNull.Value;
           if (oParam.AuditTime != AppConst.DateTimeNull)
               paramAuditTime.Value = oParam.AuditTime;
           else
               paramAuditTime.Value = System.DBNull.Value;
           if (oParam.AuditUserSysNo != AppConst.IntNull)
               paramAuditUserSysNo.Value = oParam.AuditUserSysNo;
           else
               paramAuditUserSysNo.Value = System.DBNull.Value;
           if (oParam.RefundTime != AppConst.DateTimeNull)
               paramRefundTime.Value = oParam.RefundTime;
           else
               paramRefundTime.Value = System.DBNull.Value;
           if (oParam.RefundUserSysNo != AppConst.IntNull)
               paramRefundUserSysNo.Value = oParam.RefundUserSysNo;
           else
               paramRefundUserSysNo.Value = System.DBNull.Value;
           if (oParam.UpdateUserSysNo != AppConst.IntNull)
               paramUpdateUserSysNo.Value = oParam.UpdateUserSysNo;
           else
               paramUpdateUserSysNo.Value = System.DBNull.Value;
           if (oParam.UpdateTime != AppConst.DateTimeNull)
               paramUpdateTime.Value = oParam.UpdateTime;
           else
               paramUpdateTime.Value = System.DBNull.Value;
           if (oParam.RefundPayType != AppConst.IntNull)
               paramRefundPayType.Value = oParam.RefundPayType;
           else
               paramRefundPayType.Value = System.DBNull.Value;
           if (oParam.RefundAmt != AppConst.DecimalNull)
               paramRefundAmt.Value = oParam.RefundAmt;
           else
               paramRefundAmt.Value = System.DBNull.Value;
           if (oParam.RefundCause != AppConst.IntNull)
               paramRefundCause.Value = oParam.RefundCause;
           else
               paramRefundCause.Value = System.DBNull.Value;
           if (oParam.RefundCauseNote != AppConst.StringNull)
               paramRefundCauseNote.Value = oParam.RefundCauseNote;
           else
               paramRefundCauseNote.Value = System.DBNull.Value;
           if (oParam.CustomerAccNote != AppConst.StringNull)
               paramCustomerAccNote.Value = oParam.CustomerAccNote;
           else
               paramCustomerAccNote.Value = System.DBNull.Value;
           if (oParam.RefundNote != AppConst.StringNull)
               paramRefundNote.Value = oParam.RefundNote;
           else
               paramRefundNote.Value = System.DBNull.Value;
           if (oParam.Status != AppConst.IntNull)
               paramStatus.Value = oParam.Status;
           else
               paramStatus.Value = System.DBNull.Value;
           if (oParam.VoucherID != AppConst.StringNull)
               paramVoucherID.Value = oParam.VoucherID;
           else
               paramVoucherID.Value = System.DBNull.Value;
           if (oParam.VoucherTime != AppConst.DateTimeNull)
               paramVoucherTime.Value = oParam.VoucherTime;
           else
               paramVoucherTime.Value = System.DBNull.Value;
           if (oParam.ACCAuditTime != AppConst.DateTimeNull)
               paramACCAuditTime.Value = oParam.ACCAuditTime;
           else
               paramACCAuditTime.Value = System.DBNull.Value;
           if (oParam.ACCAuditUserSysNo != AppConst.IntNull)
               paramACCAuditUserSysNo.Value = oParam.ACCAuditUserSysNo;
           else
               paramACCAuditUserSysNo.Value = System.DBNull.Value;

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
           cmd.Parameters.Add(paramUpdateUserSysNo);
           cmd.Parameters.Add(paramUpdateTime);
           cmd.Parameters.Add(paramRefundPayType);
           cmd.Parameters.Add(paramRefundAmt);
           cmd.Parameters.Add(paramRefundCause);
           cmd.Parameters.Add(paramRefundCauseNote);
           cmd.Parameters.Add(paramCustomerAccNote);
           cmd.Parameters.Add(paramRefundNote);
           cmd.Parameters.Add(paramStatus);
           cmd.Parameters.Add(paramVoucherID);
           cmd.Parameters.Add(paramVoucherTime);
           cmd.Parameters.Add(paramACCAuditTime);
           cmd.Parameters.Add(paramACCAuditUserSysNo);

           return SqlHelper.ExecuteNonQuery(cmd);
       }
       public int Update(Hashtable paramHash)
       {
           StringBuilder sb = new StringBuilder(200);
           sb.Append("UPDATE PreSale_Refund SET ");

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
                       if ((DateTime)item == AppConst.DateTimeNull)
                           sb.Append(key).Append(" = null ");
                       else
                           sb.Append(key).Append(" = cast(").Append(Util.ToSqlString(((DateTime)item).ToString(AppConst.DateFormatLong))).Append(" as DateTime)");
                   }
               }
           }

           sb.Append(" WHERE SysNo=").Append(paramHash["SysNo"]);

           return SqlHelper.ExecuteNonQuery(sb.ToString());
       }

    }
}
