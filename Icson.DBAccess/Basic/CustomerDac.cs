using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Text;

using Icson.Utils;
using Icson.Objects.Basic;
using Icson.DBAccess;

namespace Icson.DBAccess.Basic
{
    /// <summary>
    /// Summary description for CustomerDac.
    /// </summary>
    public class CustomerDac
    {
        public CustomerDac()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public int Insert(CustomerInfo oParam)
        {
            string sql = @"INSERT INTO Customer
                            (
                            SysNo,CustomerID, Pwd, Status, 
                            CustomerName, Gender, Email, Phone, 
                            CellPhone, Fax, DwellAreaSysNo, DwellAddress, 
                            DwellZip, ReceiveName, ReceiveContact, ReceivePhone, 
                            ReceiveCellPhone, ReceiveFax, ReceiveAreaSysNo, ReceiveAddress, 
                            ReceiveZip, TotalScore, ValidScore, CardNo, 
                            Note, EmailStatus, RegisterTime,
                            LastLoginIP, LastLoginTime,
                            CustomerRank, IsManualRank, CustomerType,
                            TotalFreeShipFee,ValidFreeShipFee,RefCustomerSysNo
                            )
                            VALUES (
                            @SysNo,@CustomerID, @Pwd, @Status, 
                            @CustomerName, @Gender, @Email, @Phone, 
                            @CellPhone, @Fax, @DwellAreaSysNo, @DwellAddress, 
                            @DwellZip, @ReceiveName, @ReceiveContact, @ReceivePhone, 
                            @ReceiveCellPhone, @ReceiveFax, @ReceiveAreaSysNo, @ReceiveAddress, 
                            @ReceiveZip, @TotalScore, @ValidScore, @CardNo, 
                            @Note, @EmailStatus, @RegisterTime,
                            @LastLoginIP, @LastLoginTime,
                            @CustomerRank, @IsManualRank, @CustomerType,
                            @TotalFreeShipFee,@ValidFreeShipFee,@RefCustomerSysNo
                            )";

            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramCustomerID = new SqlParameter("@CustomerID", SqlDbType.NVarChar, 50);
            SqlParameter paramPwd = new SqlParameter("@Pwd", SqlDbType.NVarChar, 50);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramCustomerName = new SqlParameter("@CustomerName", SqlDbType.NVarChar, 50);
            SqlParameter paramGender = new SqlParameter("@Gender", SqlDbType.Int, 4);
            SqlParameter paramEmail = new SqlParameter("@Email", SqlDbType.NVarChar, 50);
            SqlParameter paramPhone = new SqlParameter("@Phone", SqlDbType.NVarChar, 50);
            SqlParameter paramCellPhone = new SqlParameter("@CellPhone", SqlDbType.NVarChar, 50);
            SqlParameter paramFax = new SqlParameter("@Fax", SqlDbType.NVarChar, 50);
            SqlParameter paramDwellAreaSysNo = new SqlParameter("@DwellAreaSysNo", SqlDbType.Int, 4);
            SqlParameter paramDwellAddress = new SqlParameter("@DwellAddress", SqlDbType.NVarChar, 200);
            SqlParameter paramDwellZip = new SqlParameter("@DwellZip", SqlDbType.NVarChar, 50);
            SqlParameter paramReceiveName = new SqlParameter("@ReceiveName", SqlDbType.NVarChar, 50);
            SqlParameter paramReceiveContact = new SqlParameter("@ReceiveContact", SqlDbType.NVarChar, 50);
            SqlParameter paramReceivePhone = new SqlParameter("@ReceivePhone", SqlDbType.NVarChar, 50);
            SqlParameter paramReceiveCellPhone = new SqlParameter("@ReceiveCellPhone", SqlDbType.NVarChar, 50);
            SqlParameter paramReceiveFax = new SqlParameter("@ReceiveFax", SqlDbType.NVarChar, 50);
            SqlParameter paramReceiveAreaSysNo = new SqlParameter("@ReceiveAreaSysNo", SqlDbType.Int, 4);
            SqlParameter paramReceiveAddress = new SqlParameter("@ReceiveAddress", SqlDbType.NVarChar, 200);
            SqlParameter paramReceiveZip = new SqlParameter("@ReceiveZip", SqlDbType.NVarChar, 50);
            SqlParameter paramTotalScore = new SqlParameter("@TotalScore", SqlDbType.Int, 4);
            SqlParameter paramValidScore = new SqlParameter("@ValidScore", SqlDbType.Int, 4);
            SqlParameter paramCardNo = new SqlParameter("@CardNo", SqlDbType.NVarChar, 50);
            SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.NVarChar, 500);
            SqlParameter paramEmailStatus = new SqlParameter("@EmailStatus", SqlDbType.Int, 4);
            SqlParameter paramRegisterTime = new SqlParameter("@RegisterTime", SqlDbType.DateTime);

            SqlParameter paramLastLoginIP = new SqlParameter("@LastLoginIP", SqlDbType.NVarChar, 50);
            SqlParameter paramLastLoginTime = new SqlParameter("@LastLoginTime", SqlDbType.DateTime);
            SqlParameter paramCustomerRank = new SqlParameter("@CustomerRank", SqlDbType.Int, 4);
            SqlParameter paramIsManualRank = new SqlParameter("@IsManualRank", SqlDbType.Int, 4);
            SqlParameter paramCustomerType = new SqlParameter("@CustomerType", SqlDbType.Int, 4);

            SqlParameter paramTotalFreeShipFee = new SqlParameter("@TotalFreeShipFee", SqlDbType.Decimal, 9);
            SqlParameter paramValidFreeShipFee = new SqlParameter("@ValidFreeShipFee", SqlDbType.Decimal, 9);
            SqlParameter paramRefCustomerSysNo = new SqlParameter("@RefCustomerSysNo", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.CustomerID != AppConst.StringNull)
                paramCustomerID.Value = oParam.CustomerID;
            else
                paramCustomerID.Value = System.DBNull.Value;
            if (oParam.Pwd != AppConst.StringNull)
                paramPwd.Value = oParam.Pwd;
            else
                paramPwd.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;
            if (oParam.CustomerName != AppConst.StringNull)
                paramCustomerName.Value = oParam.CustomerName;
            else
                paramCustomerName.Value = System.DBNull.Value;
            if (oParam.Gender != AppConst.IntNull)
                paramGender.Value = oParam.Gender;
            else
                paramGender.Value = System.DBNull.Value;
            if (oParam.Email != AppConst.StringNull)
                paramEmail.Value = oParam.Email;
            else
                paramEmail.Value = System.DBNull.Value;
            if (oParam.Phone != AppConst.StringNull)
                paramPhone.Value = oParam.Phone;
            else
                paramPhone.Value = System.DBNull.Value;
            if (oParam.CellPhone != AppConst.StringNull)
                paramCellPhone.Value = oParam.CellPhone;
            else
                paramCellPhone.Value = System.DBNull.Value;
            if (oParam.Fax != AppConst.StringNull)
                paramFax.Value = oParam.Fax;
            else
                paramFax.Value = System.DBNull.Value;
            paramDwellAreaSysNo.Value = oParam.DwellAreaSysNo;
            if (oParam.DwellAddress != AppConst.StringNull)
                paramDwellAddress.Value = oParam.DwellAddress;
            else
                paramDwellAddress.Value = System.DBNull.Value;
            if (oParam.DwellZip != AppConst.StringNull)
                paramDwellZip.Value = oParam.DwellZip;
            else
                paramDwellZip.Value = System.DBNull.Value;
            if (oParam.ReceiveName != AppConst.StringNull)
                paramReceiveName.Value = oParam.ReceiveName;
            else
                paramReceiveName.Value = System.DBNull.Value;
            if (oParam.ReceiveContact != AppConst.StringNull)
                paramReceiveContact.Value = oParam.ReceiveContact;
            else
                paramReceiveContact.Value = System.DBNull.Value;
            if (oParam.ReceivePhone != AppConst.StringNull)
                paramReceivePhone.Value = oParam.ReceivePhone;
            else
                paramReceivePhone.Value = System.DBNull.Value;
            if (oParam.ReceiveCellPhone != AppConst.StringNull)
                paramReceiveCellPhone.Value = oParam.ReceiveCellPhone;
            else
                paramReceiveCellPhone.Value = System.DBNull.Value;
            if (oParam.ReceiveFax != AppConst.StringNull)
                paramReceiveFax.Value = oParam.ReceiveFax;
            else
                paramReceiveFax.Value = System.DBNull.Value;
            paramReceiveAreaSysNo.Value = oParam.ReceiveAreaSysNo;
            if (oParam.ReceiveAddress != AppConst.StringNull)
                paramReceiveAddress.Value = oParam.ReceiveAddress;
            else
                paramReceiveAddress.Value = System.DBNull.Value;
            if (oParam.ReceiveZip != AppConst.StringNull)
                paramReceiveZip.Value = oParam.ReceiveZip;
            else
                paramReceiveZip.Value = System.DBNull.Value;
            if (oParam.TotalScore != AppConst.IntNull)
                paramTotalScore.Value = oParam.TotalScore;
            else
                paramTotalScore.Value = System.DBNull.Value;
            if (oParam.ValidScore != AppConst.IntNull)
                paramValidScore.Value = oParam.ValidScore;
            else
                paramValidScore.Value = System.DBNull.Value;
            if (oParam.CardNo != AppConst.StringNull)
                paramCardNo.Value = oParam.CardNo;
            else
                paramCardNo.Value = System.DBNull.Value;
            if (oParam.Note != AppConst.StringNull)
                paramNote.Value = oParam.Note;
            else
                paramNote.Value = System.DBNull.Value;
            if (oParam.EmailStatus != AppConst.IntNull)
                paramEmailStatus.Value = oParam.EmailStatus;
            else
                paramEmailStatus.Value = System.DBNull.Value;
            if (oParam.RegisterTime != AppConst.DateTimeNull)
                paramRegisterTime.Value = oParam.RegisterTime;
            else
                paramRegisterTime.Value = System.DBNull.Value;

            if (oParam.LastLoginIP != AppConst.StringNull)
                paramLastLoginIP.Value = oParam.LastLoginIP;
            else
                paramLastLoginIP.Value = System.DBNull.Value;
            if (oParam.LastLoginTime != AppConst.DateTimeNull)
                paramLastLoginTime.Value = oParam.LastLoginTime;
            else
                paramLastLoginTime.Value = System.DBNull.Value;
            if (oParam.CustomerRank != AppConst.IntNull)
                paramCustomerRank.Value = oParam.CustomerRank;
            else
                paramCustomerRank.Value = System.DBNull.Value;
            if (oParam.IsManualRank != AppConst.IntNull)
                paramIsManualRank.Value = oParam.IsManualRank;
            else
                paramIsManualRank.Value = System.DBNull.Value;
            if (oParam.CustomerType != AppConst.IntNull)
                paramCustomerType.Value = oParam.CustomerType;
            else
                paramCustomerType.Value = System.DBNull.Value;

            if (oParam.TotalFreeShipFee != AppConst.DecimalNull)
                paramTotalFreeShipFee.Value = oParam.TotalFreeShipFee;
            else
                paramTotalFreeShipFee.Value = System.DBNull.Value;
            if (oParam.ValidFreeShipFee != AppConst.DecimalNull)
                paramValidFreeShipFee.Value = oParam.ValidFreeShipFee;
            else
                paramValidFreeShipFee.Value = System.DBNull.Value;
            if (oParam.RefCustomerSysNo != AppConst.IntNull)
                paramRefCustomerSysNo.Value = oParam.RefCustomerSysNo;
            else
                paramRefCustomerSysNo.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramCustomerID);
            cmd.Parameters.Add(paramPwd);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramCustomerName);
            cmd.Parameters.Add(paramGender);
            cmd.Parameters.Add(paramEmail);
            cmd.Parameters.Add(paramPhone);
            cmd.Parameters.Add(paramCellPhone);
            cmd.Parameters.Add(paramFax);
            cmd.Parameters.Add(paramDwellAreaSysNo);
            cmd.Parameters.Add(paramDwellAddress);
            cmd.Parameters.Add(paramDwellZip);
            cmd.Parameters.Add(paramReceiveName);
            cmd.Parameters.Add(paramReceiveContact);
            cmd.Parameters.Add(paramReceivePhone);
            cmd.Parameters.Add(paramReceiveCellPhone);
            cmd.Parameters.Add(paramReceiveFax);
            cmd.Parameters.Add(paramReceiveAreaSysNo);
            cmd.Parameters.Add(paramReceiveAddress);
            cmd.Parameters.Add(paramReceiveZip);
            cmd.Parameters.Add(paramTotalScore);
            cmd.Parameters.Add(paramValidScore);
            cmd.Parameters.Add(paramCardNo);
            cmd.Parameters.Add(paramNote);
            cmd.Parameters.Add(paramEmailStatus);
            cmd.Parameters.Add(paramRegisterTime);

            cmd.Parameters.Add(paramLastLoginIP);
            cmd.Parameters.Add(paramLastLoginTime);
            cmd.Parameters.Add(paramCustomerRank);
            cmd.Parameters.Add(paramIsManualRank);
            cmd.Parameters.Add(paramCustomerType);

            cmd.Parameters.Add(paramTotalFreeShipFee);
            cmd.Parameters.Add(paramValidFreeShipFee);
            cmd.Parameters.Add(paramRefCustomerSysNo);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        //public int Update(Hashtable paramHash)
        //{
        //    StringBuilder sb = new StringBuilder(200);
        //    sb.Append("UPDATE Customer SET ");

        //    if ( paramHash != null && paramHash.Count != 0 )
        //    {
        //        int index = 0;
        //        foreach(string key in paramHash.Keys)
        //        {
        //            object item = paramHash[key];
        //            if ( key.ToLower()=="sysno" )
        //                continue;

        //            if ( index != 0 )
        //                sb.Append(",");
        //            index++;


        //            if (item is int || item is decimal)
        //            {
        //                sb.Append(key).Append("=").Append(item.ToString());
        //            }
        //            else if (item is string)
        //            {
        //                sb.Append(key).Append(" = ").Append(Util.ToSqlString(item.ToString()));
        //            }
        //            else if (item is DateTime)
        //            {
        //                sb.Append(key).Append(" = cast(").Append( Util.ToSqlString(((DateTime)item).ToString(AppConst.DateFormatLong))).Append(" as DateTime)");
        //            }
        //        }
        //    }

        //    sb.Append(" WHERE SysNo=").Append(paramHash["SysNo"]);

        //    return SqlHelper.ExecuteNonQuery(sb.ToString());
        //}

        public int Insert(CustomerAddressInfo oParam)
        {
            string sql = @"INSERT INTO Customer_Address
                            (
                            CustomerSysNo, Brief, Name, Contact, 
                            Phone, CellPhone, Fax, Address, 
                            Zip, AreaSysNo, IsDefault, UpdateTime
                            )
                            VALUES (
                            @CustomerSysNo, @Brief, @Name, @Contact, 
                            @Phone, @CellPhone, @Fax, @Address, 
                            @Zip, @AreaSysNo, @IsDefault, @UpdateTime
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramCustomerSysNo = new SqlParameter("@CustomerSysNo", SqlDbType.Int, 4);
            SqlParameter paramBrief = new SqlParameter("@Brief", SqlDbType.NVarChar, 50);
            SqlParameter paramName = new SqlParameter("@Name", SqlDbType.NVarChar, 50);
            SqlParameter paramContact = new SqlParameter("@Contact", SqlDbType.NVarChar, 50);
            SqlParameter paramPhone = new SqlParameter("@Phone", SqlDbType.NVarChar, 50);
            SqlParameter paramCellPhone = new SqlParameter("@CellPhone", SqlDbType.NVarChar, 50);
            SqlParameter paramFax = new SqlParameter("@Fax", SqlDbType.NVarChar, 50);
            SqlParameter paramAddress = new SqlParameter("@Address", SqlDbType.NVarChar, 200);
            SqlParameter paramZip = new SqlParameter("@Zip", SqlDbType.NVarChar, 50);
            SqlParameter paramAreaSysNo = new SqlParameter("@AreaSysNo", SqlDbType.Int, 4);
            SqlParameter paramIsDefault = new SqlParameter("@IsDefault", SqlDbType.Int, 4);
            SqlParameter paramUpdateTime = new SqlParameter("@UpdateTime", SqlDbType.DateTime);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.CustomerSysNo != AppConst.IntNull)
                paramCustomerSysNo.Value = oParam.CustomerSysNo;
            else
                paramCustomerSysNo.Value = System.DBNull.Value;
            if (oParam.Brief != AppConst.StringNull)
                paramBrief.Value = oParam.Brief;
            else
                paramBrief.Value = System.DBNull.Value;
            if (oParam.Name != AppConst.StringNull)
                paramName.Value = oParam.Name;
            else
                paramName.Value = System.DBNull.Value;
            if (oParam.Contact != AppConst.StringNull)
                paramContact.Value = oParam.Contact;
            else
                paramContact.Value = System.DBNull.Value;
            if (oParam.Phone != AppConst.StringNull)
                paramPhone.Value = oParam.Phone;
            else
                paramPhone.Value = System.DBNull.Value;
            if (oParam.CellPhone != AppConst.StringNull)
                paramCellPhone.Value = oParam.CellPhone;
            else
                paramCellPhone.Value = System.DBNull.Value;
            if (oParam.Fax != AppConst.StringNull)
                paramFax.Value = oParam.Fax;
            else
                paramFax.Value = System.DBNull.Value;
            if (oParam.Address != AppConst.StringNull)
                paramAddress.Value = oParam.Address;
            else
                paramAddress.Value = System.DBNull.Value;
            if (oParam.Zip != AppConst.StringNull)
                paramZip.Value = oParam.Zip;
            else
                paramZip.Value = System.DBNull.Value;
            if (oParam.AreaSysNo != AppConst.IntNull)
                paramAreaSysNo.Value = oParam.AreaSysNo;
            else
                paramAreaSysNo.Value = System.DBNull.Value;
            if (oParam.IsDefault != AppConst.IntNull)
                paramIsDefault.Value = oParam.IsDefault;
            else
                paramIsDefault.Value = System.DBNull.Value;
            if (oParam.UpdateTime != AppConst.DateTimeNull)
                paramUpdateTime.Value = oParam.UpdateTime;
            else
                paramUpdateTime.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramCustomerSysNo);
            cmd.Parameters.Add(paramBrief);
            cmd.Parameters.Add(paramName);
            cmd.Parameters.Add(paramContact);
            cmd.Parameters.Add(paramPhone);
            cmd.Parameters.Add(paramCellPhone);
            cmd.Parameters.Add(paramFax);
            cmd.Parameters.Add(paramAddress);
            cmd.Parameters.Add(paramZip);
            cmd.Parameters.Add(paramAreaSysNo);
            cmd.Parameters.Add(paramIsDefault);
            cmd.Parameters.Add(paramUpdateTime);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Update(CustomerAddressInfo oParam)
        {
            string sql = @"UPDATE Customer_Address SET 
                            CustomerSysNo=@CustomerSysNo, Brief=@Brief, 
                            Name=@Name, Contact=@Contact, 
                            Phone=@Phone, CellPhone=@CellPhone, 
                            Fax=@Fax, Address=@Address, 
                            Zip=@Zip, AreaSysNo=@AreaSysNo, 
                            IsDefault=@IsDefault, UpdateTime=@UpdateTime
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramCustomerSysNo = new SqlParameter("@CustomerSysNo", SqlDbType.Int, 4);
            SqlParameter paramBrief = new SqlParameter("@Brief", SqlDbType.NVarChar, 50);
            SqlParameter paramName = new SqlParameter("@Name", SqlDbType.NVarChar, 50);
            SqlParameter paramContact = new SqlParameter("@Contact", SqlDbType.NVarChar, 50);
            SqlParameter paramPhone = new SqlParameter("@Phone", SqlDbType.NVarChar, 50);
            SqlParameter paramCellPhone = new SqlParameter("@CellPhone", SqlDbType.NVarChar, 50);
            SqlParameter paramFax = new SqlParameter("@Fax", SqlDbType.NVarChar, 50);
            SqlParameter paramAddress = new SqlParameter("@Address", SqlDbType.NVarChar, 200);
            SqlParameter paramZip = new SqlParameter("@Zip", SqlDbType.NVarChar, 50);
            SqlParameter paramAreaSysNo = new SqlParameter("@AreaSysNo", SqlDbType.Int, 4);
            SqlParameter paramIsDefault = new SqlParameter("@IsDefault", SqlDbType.Int, 4);
            SqlParameter paramUpdateTime = new SqlParameter("@UpdateTime", SqlDbType.DateTime);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.CustomerSysNo != AppConst.IntNull)
                paramCustomerSysNo.Value = oParam.CustomerSysNo;
            else
                paramCustomerSysNo.Value = System.DBNull.Value;
            if (oParam.Brief != AppConst.StringNull)
                paramBrief.Value = oParam.Brief;
            else
                paramBrief.Value = System.DBNull.Value;
            if (oParam.Name != AppConst.StringNull)
                paramName.Value = oParam.Name;
            else
                paramName.Value = System.DBNull.Value;
            if (oParam.Contact != AppConst.StringNull)
                paramContact.Value = oParam.Contact;
            else
                paramContact.Value = System.DBNull.Value;
            if (oParam.Phone != AppConst.StringNull)
                paramPhone.Value = oParam.Phone;
            else
                paramPhone.Value = System.DBNull.Value;
            if (oParam.CellPhone != AppConst.StringNull)
                paramCellPhone.Value = oParam.CellPhone;
            else
                paramCellPhone.Value = System.DBNull.Value;
            if (oParam.Fax != AppConst.StringNull)
                paramFax.Value = oParam.Fax;
            else
                paramFax.Value = System.DBNull.Value;
            if (oParam.Address != AppConst.StringNull)
                paramAddress.Value = oParam.Address;
            else
                paramAddress.Value = System.DBNull.Value;
            if (oParam.Zip != AppConst.StringNull)
                paramZip.Value = oParam.Zip;
            else
                paramZip.Value = System.DBNull.Value;
            if (oParam.AreaSysNo != AppConst.IntNull)
                paramAreaSysNo.Value = oParam.AreaSysNo;
            else
                paramAreaSysNo.Value = System.DBNull.Value;
            if (oParam.IsDefault != AppConst.IntNull)
                paramIsDefault.Value = oParam.IsDefault;
            else
                paramIsDefault.Value = System.DBNull.Value;
            if (oParam.UpdateTime != AppConst.DateTimeNull)
                paramUpdateTime.Value = oParam.UpdateTime;
            else
                paramUpdateTime.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramCustomerSysNo);
            cmd.Parameters.Add(paramBrief);
            cmd.Parameters.Add(paramName);
            cmd.Parameters.Add(paramContact);
            cmd.Parameters.Add(paramPhone);
            cmd.Parameters.Add(paramCellPhone);
            cmd.Parameters.Add(paramFax);
            cmd.Parameters.Add(paramAddress);
            cmd.Parameters.Add(paramZip);
            cmd.Parameters.Add(paramAreaSysNo);
            cmd.Parameters.Add(paramIsDefault);
            cmd.Parameters.Add(paramUpdateTime);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int Insert(CustomerVATInfo oParam)
        {
            string sql = @"INSERT INTO Customer_VATInfo
                            (
                            CustomerSysNo, CompanyName, TaxNum, CompanyAddress, 
                            CompanyPhone, BankInfo, BankAccount, Image1, 
                            Image2, Image3, Image4, Memo, 
                            CreateTime, IsDefault, Status
                            )
                            VALUES (
                            @CustomerSysNo, @CompanyName, @TaxNum, @CompanyAddress, 
                            @CompanyPhone, @BankInfo, @BankAccount, @Image1, 
                            @Image2, @Image3, @Image4, @Memo, 
                            @CreateTime, @IsDefault, @Status
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramCustomerSysNo = new SqlParameter("@CustomerSysNo", SqlDbType.Int, 4);
            SqlParameter paramCompanyName = new SqlParameter("@CompanyName", SqlDbType.NVarChar, 100);
            SqlParameter paramTaxNum = new SqlParameter("@TaxNum", SqlDbType.NVarChar, 20);
            SqlParameter paramCompanyAddress = new SqlParameter("@CompanyAddress", SqlDbType.NVarChar, 200);
            SqlParameter paramCompanyPhone = new SqlParameter("@CompanyPhone", SqlDbType.NVarChar, 50);
            SqlParameter paramBankInfo = new SqlParameter("@BankInfo", SqlDbType.NVarChar, 100);
            SqlParameter paramBankAccount = new SqlParameter("@BankAccount", SqlDbType.NVarChar, 100);
            SqlParameter paramImage1 = new SqlParameter("@Image1", SqlDbType.NVarChar, 100);
            SqlParameter paramImage2 = new SqlParameter("@Image2", SqlDbType.NVarChar, 100);
            SqlParameter paramImage3 = new SqlParameter("@Image3", SqlDbType.NVarChar, 100);
            SqlParameter paramImage4 = new SqlParameter("@Image4", SqlDbType.NVarChar, 100);
            SqlParameter paramMemo = new SqlParameter("@Memo", SqlDbType.NVarChar, 200);
            SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
            SqlParameter paramIsDefault = new SqlParameter("@IsDefault", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.CustomerSysNo != AppConst.IntNull)
                paramCustomerSysNo.Value = oParam.CustomerSysNo;
            else
                paramCustomerSysNo.Value = System.DBNull.Value;
            if (oParam.CompanyName != AppConst.StringNull)
                paramCompanyName.Value = oParam.CompanyName;
            else
                paramCompanyName.Value = System.DBNull.Value;
            if (oParam.TaxNum != AppConst.StringNull)
                paramTaxNum.Value = oParam.TaxNum;
            else
                paramTaxNum.Value = System.DBNull.Value;
            if (oParam.CompanyAddress != AppConst.StringNull)
                paramCompanyAddress.Value = oParam.CompanyAddress;
            else
                paramCompanyAddress.Value = System.DBNull.Value;
            if (oParam.CompanyPhone != AppConst.StringNull)
                paramCompanyPhone.Value = oParam.CompanyPhone;
            else
                paramCompanyPhone.Value = System.DBNull.Value;
            if (oParam.BankInfo != AppConst.StringNull)
                paramBankInfo.Value = oParam.BankInfo;
            else
                paramBankInfo.Value = System.DBNull.Value;
            if (oParam.BankAccount != AppConst.StringNull)
                paramBankAccount.Value = oParam.BankAccount;
            else
                paramBankAccount.Value = System.DBNull.Value;
            if (oParam.Image1 != AppConst.StringNull)
                paramImage1.Value = oParam.Image1;
            else
                paramImage1.Value = System.DBNull.Value;
            if (oParam.Image2 != AppConst.StringNull)
                paramImage2.Value = oParam.Image2;
            else
                paramImage2.Value = System.DBNull.Value;
            if (oParam.Image3 != AppConst.StringNull)
                paramImage3.Value = oParam.Image3;
            else
                paramImage3.Value = System.DBNull.Value;
            if (oParam.Image4 != AppConst.StringNull)
                paramImage4.Value = oParam.Image4;
            else
                paramImage4.Value = System.DBNull.Value;
            if (oParam.Memo != AppConst.StringNull)
                paramMemo.Value = oParam.Memo;
            else
                paramMemo.Value = System.DBNull.Value;
            if (oParam.CreateTime != AppConst.DateTimeNull)
                paramCreateTime.Value = oParam.CreateTime;
            else
                paramCreateTime.Value = System.DBNull.Value;
            if (oParam.IsDefault != AppConst.IntNull)
                paramIsDefault.Value = oParam.IsDefault;
            else
                paramIsDefault.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramCustomerSysNo);
            cmd.Parameters.Add(paramCompanyName);
            cmd.Parameters.Add(paramTaxNum);
            cmd.Parameters.Add(paramCompanyAddress);
            cmd.Parameters.Add(paramCompanyPhone);
            cmd.Parameters.Add(paramBankInfo);
            cmd.Parameters.Add(paramBankAccount);
            cmd.Parameters.Add(paramImage1);
            cmd.Parameters.Add(paramImage2);
            cmd.Parameters.Add(paramImage3);
            cmd.Parameters.Add(paramImage4);
            cmd.Parameters.Add(paramMemo);
            cmd.Parameters.Add(paramCreateTime);
            cmd.Parameters.Add(paramIsDefault);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Update(CustomerVATInfo oParam)
        {
            string sql = @"UPDATE Customer_VATInfo SET 
                            CustomerSysNo=@CustomerSysNo, CompanyName=@CompanyName, 
                            TaxNum=@TaxNum, CompanyAddress=@CompanyAddress, 
                            CompanyPhone=@CompanyPhone, BankInfo=@BankInfo, 
                            BankAccount=@BankAccount, Image1=@Image1, 
                            Image2=@Image2, Image3=@Image3, 
                            Image4=@Image4, Memo=@Memo, 
                            CreateTime=@CreateTime, IsDefault=@IsDefault, 
                            Status=@Status
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramCustomerSysNo = new SqlParameter("@CustomerSysNo", SqlDbType.Int, 4);
            SqlParameter paramCompanyName = new SqlParameter("@CompanyName", SqlDbType.NVarChar, 100);
            SqlParameter paramTaxNum = new SqlParameter("@TaxNum", SqlDbType.NVarChar, 20);
            SqlParameter paramCompanyAddress = new SqlParameter("@CompanyAddress", SqlDbType.NVarChar, 200);
            SqlParameter paramCompanyPhone = new SqlParameter("@CompanyPhone", SqlDbType.NVarChar, 50);
            SqlParameter paramBankInfo = new SqlParameter("@BankInfo", SqlDbType.NVarChar, 100);
            SqlParameter paramBankAccount = new SqlParameter("@BankAccount", SqlDbType.NVarChar, 100);
            SqlParameter paramImage1 = new SqlParameter("@Image1", SqlDbType.NVarChar, 100);
            SqlParameter paramImage2 = new SqlParameter("@Image2", SqlDbType.NVarChar, 100);
            SqlParameter paramImage3 = new SqlParameter("@Image3", SqlDbType.NVarChar, 100);
            SqlParameter paramImage4 = new SqlParameter("@Image4", SqlDbType.NVarChar, 100);
            SqlParameter paramMemo = new SqlParameter("@Memo", SqlDbType.NVarChar, 200);
            SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
            SqlParameter paramIsDefault = new SqlParameter("@IsDefault", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.CustomerSysNo != AppConst.IntNull)
                paramCustomerSysNo.Value = oParam.CustomerSysNo;
            else
                paramCustomerSysNo.Value = System.DBNull.Value;
            if (oParam.CompanyName != AppConst.StringNull)
                paramCompanyName.Value = oParam.CompanyName;
            else
                paramCompanyName.Value = System.DBNull.Value;
            if (oParam.TaxNum != AppConst.StringNull)
                paramTaxNum.Value = oParam.TaxNum;
            else
                paramTaxNum.Value = System.DBNull.Value;
            if (oParam.CompanyAddress != AppConst.StringNull)
                paramCompanyAddress.Value = oParam.CompanyAddress;
            else
                paramCompanyAddress.Value = System.DBNull.Value;
            if (oParam.CompanyPhone != AppConst.StringNull)
                paramCompanyPhone.Value = oParam.CompanyPhone;
            else
                paramCompanyPhone.Value = System.DBNull.Value;
            if (oParam.BankInfo != AppConst.StringNull)
                paramBankInfo.Value = oParam.BankInfo;
            else
                paramBankInfo.Value = System.DBNull.Value;
            if (oParam.BankAccount != AppConst.StringNull)
                paramBankAccount.Value = oParam.BankAccount;
            else
                paramBankAccount.Value = System.DBNull.Value;
            if (oParam.Image1 != AppConst.StringNull)
                paramImage1.Value = oParam.Image1;
            else
                paramImage1.Value = System.DBNull.Value;
            if (oParam.Image2 != AppConst.StringNull)
                paramImage2.Value = oParam.Image2;
            else
                paramImage2.Value = System.DBNull.Value;
            if (oParam.Image3 != AppConst.StringNull)
                paramImage3.Value = oParam.Image3;
            else
                paramImage3.Value = System.DBNull.Value;
            if (oParam.Image4 != AppConst.StringNull)
                paramImage4.Value = oParam.Image4;
            else
                paramImage4.Value = System.DBNull.Value;
            if (oParam.Memo != AppConst.StringNull)
                paramMemo.Value = oParam.Memo;
            else
                paramMemo.Value = System.DBNull.Value;
            if (oParam.CreateTime != AppConst.DateTimeNull)
                paramCreateTime.Value = oParam.CreateTime;
            else
                paramCreateTime.Value = System.DBNull.Value;
            if (oParam.IsDefault != AppConst.IntNull)
                paramIsDefault.Value = oParam.IsDefault;
            else
                paramIsDefault.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramCustomerSysNo);
            cmd.Parameters.Add(paramCompanyName);
            cmd.Parameters.Add(paramTaxNum);
            cmd.Parameters.Add(paramCompanyAddress);
            cmd.Parameters.Add(paramCompanyPhone);
            cmd.Parameters.Add(paramBankInfo);
            cmd.Parameters.Add(paramBankAccount);
            cmd.Parameters.Add(paramImage1);
            cmd.Parameters.Add(paramImage2);
            cmd.Parameters.Add(paramImage3);
            cmd.Parameters.Add(paramImage4);
            cmd.Parameters.Add(paramMemo);
            cmd.Parameters.Add(paramCreateTime);
            cmd.Parameters.Add(paramIsDefault);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int UpdateCustomer(Hashtable paramHash)
        {
            return UtilDac.GetInstance().Update(paramHash, "Customer");
        }

        public int UpdateCustomerVATInfo(Hashtable paramHash)
        {
            return UtilDac.GetInstance().Update(paramHash, "Customer_VATInfo");
        }
    }
}