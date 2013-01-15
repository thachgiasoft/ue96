using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using Icson.Objects.Complain;
using Icson.Utils;
namespace Icson.DBAccess.Complain
{
   public class ComplainDac
    {
       public int Insert(ComplainInfo oParam)
       {
           string sql = @"INSERT INTO Complain
                            (
                            SoSysNo, CustomerSysno, Contact, ContactPhone, 
                            AreaSysNo, Address, CreateUserSysNo, CreateTime, 
                            Status, LastUpdateUserSysNo, LastUpdateTime, CurrentHandleUserSysNo, 
                            AbnormalType, AbnormalCauseType, CustomerNote, EmployeeNote, 
                            AuditNote, AuditUserSysNo, AuditTime, Score, 
                            ReviewBackNote, LastReviewBackUserSysNo, LastReviewTime, CloseUserSysNo, 
                            CloseTime
                            )
                            VALUES (
                            @SoSysNo, @CustomerSysno, @Contact, @ContactPhone, 
                            @AreaSysNo, @Address, @CreateUserSysNo, @CreateTime, 
                            @Status, @LastUpdateUserSysNo, @LastUpdateTime, @CurrentHandleUserSysNo, 
                            @AbnormalType, @AbnormalCauseType, @CustomerNote, @EmployeeNote, 
                            @AuditNote, @AuditUserSysNo, @AuditTime, @Score, 
                            @ReviewBackNote, @LastReviewBackUserSysNo, @LastReviewTime, @CloseUserSysNo, 
                            @CloseTime
                            );set @SysNo = SCOPE_IDENTITY();";
           SqlCommand cmd = new SqlCommand(sql);

           SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
           SqlParameter paramSoSysNo = new SqlParameter("@SoSysNo", SqlDbType.Int, 4);
           SqlParameter paramCustomerSysno = new SqlParameter("@CustomerSysno", SqlDbType.Int, 4);
           SqlParameter paramContact = new SqlParameter("@Contact", SqlDbType.NVarChar, 50);
           SqlParameter paramContactPhone = new SqlParameter("@ContactPhone", SqlDbType.NVarChar, 50);
           SqlParameter paramAreaSysNo = new SqlParameter("@AreaSysNo", SqlDbType.Int, 4);
           SqlParameter paramAddress = new SqlParameter("@Address", SqlDbType.NVarChar, 200);
           SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
           SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
           SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
           SqlParameter paramLastUpdateUserSysNo = new SqlParameter("@LastUpdateUserSysNo", SqlDbType.Int, 4);
           SqlParameter paramLastUpdateTime = new SqlParameter("@LastUpdateTime", SqlDbType.DateTime);
           SqlParameter paramCurrentHandleUserSysNo = new SqlParameter("@CurrentHandleUserSysNo", SqlDbType.Int, 4);
           SqlParameter paramAbnormalType = new SqlParameter("@AbnormalType", SqlDbType.Int, 4);
           SqlParameter paramAbnormalCauseType = new SqlParameter("@AbnormalCauseType", SqlDbType.Int, 4);
           SqlParameter paramCustomerNote = new SqlParameter("@CustomerNote", SqlDbType.Text, 0);
           SqlParameter paramEmployeeNote = new SqlParameter("@EmployeeNote", SqlDbType.Text, 0);
           SqlParameter paramAuditNote = new SqlParameter("@AuditNote", SqlDbType.NVarChar, 200);
           SqlParameter paramAuditUserSysNo = new SqlParameter("@AuditUserSysNo", SqlDbType.Int, 4);
           SqlParameter paramAuditTime = new SqlParameter("@AuditTime", SqlDbType.DateTime);
           SqlParameter paramScore = new SqlParameter("@Score", SqlDbType.Int, 4);
           SqlParameter paramReviewBackNote = new SqlParameter("@ReviewBackNote", SqlDbType.Text, 0);
           SqlParameter paramLastReviewBackUserSysNo = new SqlParameter("@LastReviewBackUserSysNo", SqlDbType.Int, 4);
           SqlParameter paramLastReviewTime = new SqlParameter("@LastReviewTime", SqlDbType.DateTime);
           SqlParameter paramCloseUserSysNo = new SqlParameter("@CloseUserSysNo", SqlDbType.Int, 4);
           SqlParameter paramCloseTime = new SqlParameter("@CloseTime", SqlDbType.DateTime);
           paramSysNo.Direction = ParameterDirection.Output;
           if (oParam.SoSysNo != AppConst.IntNull)
               paramSoSysNo.Value = oParam.SoSysNo;
           else
               paramSoSysNo.Value = System.DBNull.Value;
           if (oParam.CustomerSysno != AppConst.IntNull)
               paramCustomerSysno.Value = oParam.CustomerSysno;
           else
               paramCustomerSysno.Value = System.DBNull.Value;
           if (oParam.Contact != AppConst.StringNull)
               paramContact.Value = oParam.Contact;
           else
               paramContact.Value = System.DBNull.Value;
           if (oParam.ContactPhone != AppConst.StringNull)
               paramContactPhone.Value = oParam.ContactPhone;
           else
               paramContactPhone.Value = System.DBNull.Value;
           if (oParam.AreaSysNo != AppConst.IntNull)
               paramAreaSysNo.Value = oParam.AreaSysNo;
           else
               paramAreaSysNo.Value = System.DBNull.Value;
           if (oParam.Address != AppConst.StringNull)
               paramAddress.Value = oParam.Address;
           else
               paramAddress.Value = System.DBNull.Value;
           if (oParam.CreateUserSysNo != AppConst.IntNull)
               paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
           else
               paramCreateUserSysNo.Value = System.DBNull.Value;
           if (oParam.CreateTime != AppConst.DateTimeNull)
               paramCreateTime.Value = oParam.CreateTime;
           else
               paramCreateTime.Value = System.DBNull.Value;
           if (oParam.Status != AppConst.IntNull)
               paramStatus.Value = oParam.Status;
           else
               paramStatus.Value = System.DBNull.Value;
           if (oParam.LastUpdateUserSysNo != AppConst.IntNull)
               paramLastUpdateUserSysNo.Value = oParam.LastUpdateUserSysNo;
           else
               paramLastUpdateUserSysNo.Value = System.DBNull.Value;
           if (oParam.LastUpdateTime != AppConst.DateTimeNull)
               paramLastUpdateTime.Value = oParam.LastUpdateTime;
           else
               paramLastUpdateTime.Value = System.DBNull.Value;
           if (oParam.CurrentHandleUserSysNo != AppConst.IntNull)
               paramCurrentHandleUserSysNo.Value = oParam.CurrentHandleUserSysNo;
           else
               paramCurrentHandleUserSysNo.Value = System.DBNull.Value;
           if (oParam.AbnormalType != AppConst.IntNull)
               paramAbnormalType.Value = oParam.AbnormalType;
           else
               paramAbnormalType.Value = System.DBNull.Value;
           if (oParam.AbnormalCauseType != AppConst.IntNull)
               paramAbnormalCauseType.Value = oParam.AbnormalCauseType;
           else
               paramAbnormalCauseType.Value = System.DBNull.Value;
           if (oParam.CustomerNote != AppConst.StringNull)
               paramCustomerNote.Value = oParam.CustomerNote;
           else
               paramCustomerNote.Value = System.DBNull.Value;
           if (oParam.EmployeeNote != AppConst.StringNull)
               paramEmployeeNote.Value = oParam.EmployeeNote;
           else
               paramEmployeeNote.Value = System.DBNull.Value;
           if (oParam.AuditNote != AppConst.StringNull)
               paramAuditNote.Value = oParam.AuditNote;
           else
               paramAuditNote.Value = System.DBNull.Value;
           if (oParam.AuditUserSysNo != AppConst.IntNull)
               paramAuditUserSysNo.Value = oParam.AuditUserSysNo;
           else
               paramAuditUserSysNo.Value = System.DBNull.Value;
           if (oParam.AuditTime != AppConst.DateTimeNull)
               paramAuditTime.Value = oParam.AuditTime;
           else
               paramAuditTime.Value = System.DBNull.Value;
           if (oParam.Score != AppConst.IntNull)
               paramScore.Value = oParam.Score;
           else
               paramScore.Value = System.DBNull.Value;
           if (oParam.ReviewBackNote != AppConst.StringNull)
               paramReviewBackNote.Value = oParam.ReviewBackNote;
           else
               paramReviewBackNote.Value = System.DBNull.Value;
           if (oParam.LastReviewBackUserSysNo != AppConst.IntNull)
               paramLastReviewBackUserSysNo.Value = oParam.LastReviewBackUserSysNo;
           else
               paramLastReviewBackUserSysNo.Value = System.DBNull.Value;
           if (oParam.LastReviewTime != AppConst.DateTimeNull)
               paramLastReviewTime.Value = oParam.LastReviewTime;
           else
               paramLastReviewTime.Value = System.DBNull.Value;
           if (oParam.CloseUserSysNo != AppConst.IntNull)
               paramCloseUserSysNo.Value = oParam.CloseUserSysNo;
           else
               paramCloseUserSysNo.Value = System.DBNull.Value;
           if (oParam.CloseTime != AppConst.DateTimeNull)
               paramCloseTime.Value = oParam.CloseTime;
           else
               paramCloseTime.Value = System.DBNull.Value;

           cmd.Parameters.Add(paramSysNo);
           cmd.Parameters.Add(paramSoSysNo);
           cmd.Parameters.Add(paramCustomerSysno);
           cmd.Parameters.Add(paramContact);
           cmd.Parameters.Add(paramContactPhone);
           cmd.Parameters.Add(paramAreaSysNo);
           cmd.Parameters.Add(paramAddress);
           cmd.Parameters.Add(paramCreateUserSysNo);
           cmd.Parameters.Add(paramCreateTime);
           cmd.Parameters.Add(paramStatus);
           cmd.Parameters.Add(paramLastUpdateUserSysNo);
           cmd.Parameters.Add(paramLastUpdateTime);
           cmd.Parameters.Add(paramCurrentHandleUserSysNo);
           cmd.Parameters.Add(paramAbnormalType);
           cmd.Parameters.Add(paramAbnormalCauseType);
           cmd.Parameters.Add(paramCustomerNote);
           cmd.Parameters.Add(paramEmployeeNote);
           cmd.Parameters.Add(paramAuditNote);
           cmd.Parameters.Add(paramAuditUserSysNo);
           cmd.Parameters.Add(paramAuditTime);
           cmd.Parameters.Add(paramScore);
           cmd.Parameters.Add(paramReviewBackNote);
           cmd.Parameters.Add(paramLastReviewBackUserSysNo);
           cmd.Parameters.Add(paramLastReviewTime);
           cmd.Parameters.Add(paramCloseUserSysNo);
           cmd.Parameters.Add(paramCloseTime);

           return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
       }
       public int Update(ComplainInfo oParam)
       {
           string sql = @"UPDATE Complain SET 
                            SoSysNo=@SoSysNo, CustomerSysno=@CustomerSysno, 
                            Contact=@Contact, ContactPhone=@ContactPhone, 
                            AreaSysNo=@AreaSysNo, Address=@Address, 
                            CreateUserSysNo=@CreateUserSysNo, CreateTime=@CreateTime, 
                            Status=@Status, LastUpdateUserSysNo=@LastUpdateUserSysNo, 
                            LastUpdateTime=@LastUpdateTime, CurrentHandleUserSysNo=@CurrentHandleUserSysNo, 
                            AbnormalType=@AbnormalType, AbnormalCauseType=@AbnormalCauseType, 
                            CustomerNote=@CustomerNote, EmployeeNote=@EmployeeNote, 
                            AuditNote=@AuditNote, AuditUserSysNo=@AuditUserSysNo, 
                            AuditTime=@AuditTime, Score=@Score, 
                            ReviewBackNote=@ReviewBackNote, LastReviewBackUserSysNo=@LastReviewBackUserSysNo, 
                            LastReviewTime=@LastReviewTime, CloseUserSysNo=@CloseUserSysNo, 
                            CloseTime=@CloseTime
                            WHERE SysNo=@SysNo";
           SqlCommand cmd = new SqlCommand(sql);

           SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
           SqlParameter paramSoSysNo = new SqlParameter("@SoSysNo", SqlDbType.Int, 4);
           SqlParameter paramCustomerSysno = new SqlParameter("@CustomerSysno", SqlDbType.Int, 4);
           SqlParameter paramContact = new SqlParameter("@Contact", SqlDbType.NVarChar, 50);
           SqlParameter paramContactPhone = new SqlParameter("@ContactPhone", SqlDbType.NVarChar, 50);
           SqlParameter paramAreaSysNo = new SqlParameter("@AreaSysNo", SqlDbType.Int, 4);
           SqlParameter paramAddress = new SqlParameter("@Address", SqlDbType.NVarChar, 200);
           SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
           SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
           SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
           SqlParameter paramLastUpdateUserSysNo = new SqlParameter("@LastUpdateUserSysNo", SqlDbType.Int, 4);
           SqlParameter paramLastUpdateTime = new SqlParameter("@LastUpdateTime", SqlDbType.DateTime);
           SqlParameter paramCurrentHandleUserSysNo = new SqlParameter("@CurrentHandleUserSysNo", SqlDbType.Int, 4);
           SqlParameter paramAbnormalType = new SqlParameter("@AbnormalType", SqlDbType.Int, 4);
           SqlParameter paramAbnormalCauseType = new SqlParameter("@AbnormalCauseType", SqlDbType.Int, 4);
           SqlParameter paramCustomerNote = new SqlParameter("@CustomerNote", SqlDbType.Text, 0);
           SqlParameter paramEmployeeNote = new SqlParameter("@EmployeeNote", SqlDbType.Text, 0);
           SqlParameter paramAuditNote = new SqlParameter("@AuditNote", SqlDbType.Text, 0);
           SqlParameter paramAuditUserSysNo = new SqlParameter("@AuditUserSysNo", SqlDbType.Int, 4);
           SqlParameter paramAuditTime = new SqlParameter("@AuditTime", SqlDbType.DateTime);
           SqlParameter paramScore = new SqlParameter("@Score", SqlDbType.Int, 4);
           SqlParameter paramReviewBackNote = new SqlParameter("@ReviewBackNote", SqlDbType.Text, 0);
           SqlParameter paramLastReviewBackUserSysNo = new SqlParameter("@LastReviewBackUserSysNo", SqlDbType.Int, 4);
           SqlParameter paramLastReviewTime = new SqlParameter("@LastReviewTime", SqlDbType.DateTime);
           SqlParameter paramCloseUserSysNo = new SqlParameter("@CloseUserSysNo", SqlDbType.Int, 4);
           SqlParameter paramCloseTime = new SqlParameter("@CloseTime", SqlDbType.DateTime);

           if (oParam.SysNo != AppConst.IntNull)
               paramSysNo.Value = oParam.SysNo;
           else
               paramSysNo.Value = System.DBNull.Value;
           if (oParam.SoSysNo != AppConst.IntNull)
               paramSoSysNo.Value = oParam.SoSysNo;
           else
               paramSoSysNo.Value = System.DBNull.Value;
           if (oParam.CustomerSysno != AppConst.IntNull)
               paramCustomerSysno.Value = oParam.CustomerSysno;
           else
               paramCustomerSysno.Value = System.DBNull.Value;
           if (oParam.Contact != AppConst.StringNull)
               paramContact.Value = oParam.Contact;
           else
               paramContact.Value = System.DBNull.Value;
           if (oParam.ContactPhone != AppConst.StringNull)
               paramContactPhone.Value = oParam.ContactPhone;
           else
               paramContactPhone.Value = System.DBNull.Value;
           if (oParam.AreaSysNo != AppConst.IntNull)
               paramAreaSysNo.Value = oParam.AreaSysNo;
           else
               paramAreaSysNo.Value = System.DBNull.Value;
           if (oParam.Address != AppConst.StringNull)
               paramAddress.Value = oParam.Address;
           else
               paramAddress.Value = System.DBNull.Value;
           if (oParam.CreateUserSysNo != AppConst.IntNull)
               paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
           else
               paramCreateUserSysNo.Value = System.DBNull.Value;
           if (oParam.CreateTime != AppConst.DateTimeNull)
               paramCreateTime.Value = oParam.CreateTime;
           else
               paramCreateTime.Value = System.DBNull.Value;
           if (oParam.Status != AppConst.IntNull)
               paramStatus.Value = oParam.Status;
           else
               paramStatus.Value = System.DBNull.Value;
           if (oParam.LastUpdateUserSysNo != AppConst.IntNull)
               paramLastUpdateUserSysNo.Value = oParam.LastUpdateUserSysNo;
           else
               paramLastUpdateUserSysNo.Value = System.DBNull.Value;
           if (oParam.LastUpdateTime != AppConst.DateTimeNull)
               paramLastUpdateTime.Value = oParam.LastUpdateTime;
           else
               paramLastUpdateTime.Value = System.DBNull.Value;
           if (oParam.CurrentHandleUserSysNo != AppConst.IntNull)
               paramCurrentHandleUserSysNo.Value = oParam.CurrentHandleUserSysNo;
           else
               paramCurrentHandleUserSysNo.Value = System.DBNull.Value;
           if (oParam.AbnormalType != AppConst.IntNull)
               paramAbnormalType.Value = oParam.AbnormalType;
           else
               paramAbnormalType.Value = System.DBNull.Value;
           if (oParam.AbnormalCauseType != AppConst.IntNull)
               paramAbnormalCauseType.Value = oParam.AbnormalCauseType;
           else
               paramAbnormalCauseType.Value = System.DBNull.Value;
           if (oParam.CustomerNote != AppConst.StringNull)
               paramCustomerNote.Value = oParam.CustomerNote;
           else
               paramCustomerNote.Value = System.DBNull.Value;
           if (oParam.EmployeeNote != AppConst.StringNull)
               paramEmployeeNote.Value = oParam.EmployeeNote;
           else
               paramEmployeeNote.Value = System.DBNull.Value;
           if (oParam.AuditNote != AppConst.StringNull)
               paramAuditNote.Value = oParam.AuditNote;
           else
               paramAuditNote.Value = System.DBNull.Value;
           if (oParam.AuditUserSysNo != AppConst.IntNull)
               paramAuditUserSysNo.Value = oParam.AuditUserSysNo;
           else
               paramAuditUserSysNo.Value = System.DBNull.Value;
           if (oParam.AuditTime != AppConst.DateTimeNull)
               paramAuditTime.Value = oParam.AuditTime;
           else
               paramAuditTime.Value = System.DBNull.Value;
           if (oParam.Score != AppConst.IntNull)
               paramScore.Value = oParam.Score;
           else
               paramScore.Value = System.DBNull.Value;
           if (oParam.ReviewBackNote != AppConst.StringNull)
               paramReviewBackNote.Value = oParam.ReviewBackNote;
           else
               paramReviewBackNote.Value = System.DBNull.Value;
           if (oParam.LastReviewBackUserSysNo != AppConst.IntNull)
               paramLastReviewBackUserSysNo.Value = oParam.LastReviewBackUserSysNo;
           else
               paramLastReviewBackUserSysNo.Value = System.DBNull.Value;
           if (oParam.LastReviewTime != AppConst.DateTimeNull)
               paramLastReviewTime.Value = oParam.LastReviewTime;
           else
               paramLastReviewTime.Value = System.DBNull.Value;
           if (oParam.CloseUserSysNo != AppConst.IntNull)
               paramCloseUserSysNo.Value = oParam.CloseUserSysNo;
           else
               paramCloseUserSysNo.Value = System.DBNull.Value;
           if (oParam.CloseTime != AppConst.DateTimeNull)
               paramCloseTime.Value = oParam.CloseTime;
           else
               paramCloseTime.Value = System.DBNull.Value;

           cmd.Parameters.Add(paramSysNo);
           cmd.Parameters.Add(paramSoSysNo);
           cmd.Parameters.Add(paramCustomerSysno);
           cmd.Parameters.Add(paramContact);
           cmd.Parameters.Add(paramContactPhone);
           cmd.Parameters.Add(paramAreaSysNo);
           cmd.Parameters.Add(paramAddress);
           cmd.Parameters.Add(paramCreateUserSysNo);
           cmd.Parameters.Add(paramCreateTime);
           cmd.Parameters.Add(paramStatus);
           cmd.Parameters.Add(paramLastUpdateUserSysNo);
           cmd.Parameters.Add(paramLastUpdateTime);
           cmd.Parameters.Add(paramCurrentHandleUserSysNo);
           cmd.Parameters.Add(paramAbnormalType);
           cmd.Parameters.Add(paramAbnormalCauseType);
           cmd.Parameters.Add(paramCustomerNote);
           cmd.Parameters.Add(paramEmployeeNote);
           cmd.Parameters.Add(paramAuditNote);
           cmd.Parameters.Add(paramAuditUserSysNo);
           cmd.Parameters.Add(paramAuditTime);
           cmd.Parameters.Add(paramScore);
           cmd.Parameters.Add(paramReviewBackNote);
           cmd.Parameters.Add(paramLastReviewBackUserSysNo);
           cmd.Parameters.Add(paramLastReviewTime);
           cmd.Parameters.Add(paramCloseUserSysNo);
           cmd.Parameters.Add(paramCloseTime);

           return SqlHelper.ExecuteNonQuery(cmd);
       }
    }
}
