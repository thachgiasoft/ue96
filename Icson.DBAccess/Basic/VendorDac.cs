using System;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects.Basic;


namespace Icson.DBAccess.Basic
{
    /// <summary>
    /// Summary description for VendorDac.
    /// </summary>
    public class VendorDac
    {

        public VendorDac()
        {
        }

        public int Insert(VendorInfo oParam)
        {
            string sql = @"INSERT INTO Vendor
                            (
                            SysNo, VendorID, VendorName, EnglishName, 
                            BriefName, VendorType, District, Address, 
                            Zip, Contact, Phone, Fax, 
                            Email, Site, Bank, Account, 
                            TaxNo, Comment, Note, WarrantyAreaSysNo, 
                            WarrantyAddress, WarrantyZip, WarrantyContact, WarrantyPhone, 
                            WarrantyFax, WarrantyEmail, WarrantySite, WarrantySelfSend, 
                            Status, RMAPosition, APType, CooperateType
                            )
                            VALUES (
                            @SysNo, @VendorID, @VendorName, @EnglishName, 
                            @BriefName, @VendorType, @District, @Address, 
                            @Zip, @Contact, @Phone, @Fax, 
                            @Email, @Site, @Bank, @Account, 
                            @TaxNo, @Comment, @Note, @WarrantyAreaSysNo, 
                            @WarrantyAddress, @WarrantyZip, @WarrantyContact, @WarrantyPhone, 
                            @WarrantyFax, @WarrantyEmail, @WarrantySite, @WarrantySelfSend, 
                            @Status, @RMAPosition, @APType, @CooperateType
                            )";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramVendorID = new SqlParameter("@VendorID", SqlDbType.NVarChar, 20);
            SqlParameter paramVendorName = new SqlParameter("@VendorName", SqlDbType.NVarChar, 100);
            SqlParameter paramEnglishName = new SqlParameter("@EnglishName", SqlDbType.NVarChar, 100);
            SqlParameter paramBriefName = new SqlParameter("@BriefName", SqlDbType.NVarChar, 50);
            SqlParameter paramVendorType = new SqlParameter("@VendorType", SqlDbType.Int, 4);
            SqlParameter paramDistrict = new SqlParameter("@District", SqlDbType.NVarChar, 100);
            SqlParameter paramAddress = new SqlParameter("@Address", SqlDbType.NVarChar, 100);
            SqlParameter paramZip = new SqlParameter("@Zip", SqlDbType.NVarChar, 10);
            SqlParameter paramContact = new SqlParameter("@Contact", SqlDbType.NVarChar, 20);
            SqlParameter paramPhone = new SqlParameter("@Phone", SqlDbType.NVarChar, 50);
            SqlParameter paramFax = new SqlParameter("@Fax", SqlDbType.NVarChar, 50);
            SqlParameter paramEmail = new SqlParameter("@Email", SqlDbType.NVarChar, 50);
            SqlParameter paramSite = new SqlParameter("@Site", SqlDbType.NVarChar, 100);
            SqlParameter paramBank = new SqlParameter("@Bank", SqlDbType.NVarChar, 50);
            SqlParameter paramAccount = new SqlParameter("@Account", SqlDbType.NVarChar, 50);
            SqlParameter paramTaxNo = new SqlParameter("@TaxNo", SqlDbType.NVarChar, 50);
            SqlParameter paramComment = new SqlParameter("@Comment", SqlDbType.NVarChar, 20);
            SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.NVarChar, 500);
            SqlParameter paramWarrantyAreaSysNo = new SqlParameter("@WarrantyAreaSysNo", SqlDbType.Int, 4);
            SqlParameter paramWarrantyAddress = new SqlParameter("@WarrantyAddress", SqlDbType.NVarChar, 100);
            SqlParameter paramWarrantyZip = new SqlParameter("@WarrantyZip", SqlDbType.NVarChar, 10);
            SqlParameter paramWarrantyContact = new SqlParameter("@WarrantyContact", SqlDbType.NVarChar, 20);
            SqlParameter paramWarrantyPhone = new SqlParameter("@WarrantyPhone", SqlDbType.NVarChar, 50);
            SqlParameter paramWarrantyFax = new SqlParameter("@WarrantyFax", SqlDbType.NVarChar, 50);
            SqlParameter paramWarrantyEmail = new SqlParameter("@WarrantyEmail", SqlDbType.NVarChar, 50);
            SqlParameter paramWarrantySite = new SqlParameter("@WarrantySite", SqlDbType.NVarChar, 100);
            SqlParameter paramWarrantySelfSend = new SqlParameter("@WarrantySelfSend", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramRMAPosition = new SqlParameter("@RMAPosition", SqlDbType.NVarChar, 50);
            SqlParameter paramAPType = new SqlParameter("@APType", SqlDbType.Int, 4);
            SqlParameter paramCooperateType = new SqlParameter("@CooperateType", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.VendorID != AppConst.StringNull)
                paramVendorID.Value = oParam.VendorID;
            else
                paramVendorID.Value = System.DBNull.Value;
            if (oParam.VendorName != AppConst.StringNull)
                paramVendorName.Value = oParam.VendorName;
            else
                paramVendorName.Value = System.DBNull.Value;
            if (oParam.EnglishName != AppConst.StringNull)
                paramEnglishName.Value = oParam.EnglishName;
            else
                paramEnglishName.Value = System.DBNull.Value;
            if (oParam.BriefName != AppConst.StringNull)
                paramBriefName.Value = oParam.BriefName;
            else
                paramBriefName.Value = System.DBNull.Value;
            if (oParam.VendorType != AppConst.IntNull)
                paramVendorType.Value = oParam.VendorType;
            else
                paramVendorType.Value = System.DBNull.Value;
            if (oParam.District != AppConst.StringNull)
                paramDistrict.Value = oParam.District;
            else
                paramDistrict.Value = System.DBNull.Value;
            if (oParam.Address != AppConst.StringNull)
                paramAddress.Value = oParam.Address;
            else
                paramAddress.Value = System.DBNull.Value;
            if (oParam.Zip != AppConst.StringNull)
                paramZip.Value = oParam.Zip;
            else
                paramZip.Value = System.DBNull.Value;
            if (oParam.Contact != AppConst.StringNull)
                paramContact.Value = oParam.Contact;
            else
                paramContact.Value = System.DBNull.Value;
            if (oParam.Phone != AppConst.StringNull)
                paramPhone.Value = oParam.Phone;
            else
                paramPhone.Value = System.DBNull.Value;
            if (oParam.Fax != AppConst.StringNull)
                paramFax.Value = oParam.Fax;
            else
                paramFax.Value = System.DBNull.Value;
            if (oParam.Email != AppConst.StringNull)
                paramEmail.Value = oParam.Email;
            else
                paramEmail.Value = System.DBNull.Value;
            if (oParam.Site != AppConst.StringNull)
                paramSite.Value = oParam.Site;
            else
                paramSite.Value = System.DBNull.Value;
            if (oParam.Bank != AppConst.StringNull)
                paramBank.Value = oParam.Bank;
            else
                paramBank.Value = System.DBNull.Value;
            if (oParam.Account != AppConst.StringNull)
                paramAccount.Value = oParam.Account;
            else
                paramAccount.Value = System.DBNull.Value;
            if (oParam.TaxNo != AppConst.StringNull)
                paramTaxNo.Value = oParam.TaxNo;
            else
                paramTaxNo.Value = System.DBNull.Value;
            if (oParam.Comment != AppConst.StringNull)
                paramComment.Value = oParam.Comment;
            else
                paramComment.Value = System.DBNull.Value;
            if (oParam.Note != AppConst.StringNull)
                paramNote.Value = oParam.Note;
            else
                paramNote.Value = System.DBNull.Value;
            if (oParam.WarrantyAreaSysNo != AppConst.IntNull)
                paramWarrantyAreaSysNo.Value = oParam.WarrantyAreaSysNo;
            else
                paramWarrantyAreaSysNo.Value = System.DBNull.Value;
            if (oParam.WarrantyAddress != AppConst.StringNull)
                paramWarrantyAddress.Value = oParam.WarrantyAddress;
            else
                paramWarrantyAddress.Value = System.DBNull.Value;
            if (oParam.WarrantyZip != AppConst.StringNull)
                paramWarrantyZip.Value = oParam.WarrantyZip;
            else
                paramWarrantyZip.Value = System.DBNull.Value;
            if (oParam.WarrantyContact != AppConst.StringNull)
                paramWarrantyContact.Value = oParam.WarrantyContact;
            else
                paramWarrantyContact.Value = System.DBNull.Value;
            if (oParam.WarrantyPhone != AppConst.StringNull)
                paramWarrantyPhone.Value = oParam.WarrantyPhone;
            else
                paramWarrantyPhone.Value = System.DBNull.Value;
            if (oParam.WarrantyFax != AppConst.StringNull)
                paramWarrantyFax.Value = oParam.WarrantyFax;
            else
                paramWarrantyFax.Value = System.DBNull.Value;
            if (oParam.WarrantyEmail != AppConst.StringNull)
                paramWarrantyEmail.Value = oParam.WarrantyEmail;
            else
                paramWarrantyEmail.Value = System.DBNull.Value;
            if (oParam.WarrantySite != AppConst.StringNull)
                paramWarrantySite.Value = oParam.WarrantySite;
            else
                paramWarrantySite.Value = System.DBNull.Value;
            if (oParam.WarrantySelfSend != AppConst.IntNull)
                paramWarrantySelfSend.Value = oParam.WarrantySelfSend;
            else
                paramWarrantySelfSend.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;
            if (oParam.RMAPosition != AppConst.StringNull)
                paramRMAPosition.Value = oParam.RMAPosition;
            else
                paramRMAPosition.Value = System.DBNull.Value;
            if (oParam.APType != AppConst.IntNull)
                paramAPType.Value = oParam.APType;
            else
                paramAPType.Value = System.DBNull.Value;
            if (oParam.CooperateType != AppConst.IntNull)
                paramCooperateType.Value = oParam.CooperateType;
            else
                paramCooperateType.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramVendorID);
            cmd.Parameters.Add(paramVendorName);
            cmd.Parameters.Add(paramEnglishName);
            cmd.Parameters.Add(paramBriefName);
            cmd.Parameters.Add(paramVendorType);
            cmd.Parameters.Add(paramDistrict);
            cmd.Parameters.Add(paramAddress);
            cmd.Parameters.Add(paramZip);
            cmd.Parameters.Add(paramContact);
            cmd.Parameters.Add(paramPhone);
            cmd.Parameters.Add(paramFax);
            cmd.Parameters.Add(paramEmail);
            cmd.Parameters.Add(paramSite);
            cmd.Parameters.Add(paramBank);
            cmd.Parameters.Add(paramAccount);
            cmd.Parameters.Add(paramTaxNo);
            cmd.Parameters.Add(paramComment);
            cmd.Parameters.Add(paramNote);
            cmd.Parameters.Add(paramWarrantyAreaSysNo);
            cmd.Parameters.Add(paramWarrantyAddress);
            cmd.Parameters.Add(paramWarrantyZip);
            cmd.Parameters.Add(paramWarrantyContact);
            cmd.Parameters.Add(paramWarrantyPhone);
            cmd.Parameters.Add(paramWarrantyFax);
            cmd.Parameters.Add(paramWarrantyEmail);
            cmd.Parameters.Add(paramWarrantySite);
            cmd.Parameters.Add(paramWarrantySelfSend);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramRMAPosition);
            cmd.Parameters.Add(paramAPType);
            cmd.Parameters.Add(paramCooperateType);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int Update(VendorInfo oParam)
        {
            string sql = @"UPDATE Vendor SET 
                            VendorID=@VendorID, VendorName=@VendorName, 
                            EnglishName=@EnglishName, BriefName=@BriefName, 
                            VendorType=@VendorType, District=@District, 
                            Address=@Address, Zip=@Zip, 
                            Contact=@Contact, Phone=@Phone, 
                            Fax=@Fax, Email=@Email, 
                            Site=@Site, Bank=@Bank, 
                            Account=@Account, TaxNo=@TaxNo, 
                            Comment=@Comment, Note=@Note, 
                            WarrantyAreaSysNo=@WarrantyAreaSysNo, WarrantyAddress=@WarrantyAddress, 
                            WarrantyZip=@WarrantyZip, WarrantyContact=@WarrantyContact, 
                            WarrantyPhone=@WarrantyPhone, WarrantyFax=@WarrantyFax, 
                            WarrantyEmail=@WarrantyEmail, WarrantySite=@WarrantySite, 
                            WarrantySelfSend=@WarrantySelfSend, Status=@Status, 
                            RMAPosition=@RMAPosition, APType=@APType, 
                            CooperateType=@CooperateType
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramVendorID = new SqlParameter("@VendorID", SqlDbType.NVarChar, 20);
            SqlParameter paramVendorName = new SqlParameter("@VendorName", SqlDbType.NVarChar, 100);
            SqlParameter paramEnglishName = new SqlParameter("@EnglishName", SqlDbType.NVarChar, 100);
            SqlParameter paramBriefName = new SqlParameter("@BriefName", SqlDbType.NVarChar, 50);
            SqlParameter paramVendorType = new SqlParameter("@VendorType", SqlDbType.Int, 4);
            SqlParameter paramDistrict = new SqlParameter("@District", SqlDbType.NVarChar, 100);
            SqlParameter paramAddress = new SqlParameter("@Address", SqlDbType.NVarChar, 100);
            SqlParameter paramZip = new SqlParameter("@Zip", SqlDbType.NVarChar, 10);
            SqlParameter paramContact = new SqlParameter("@Contact", SqlDbType.NVarChar, 20);
            SqlParameter paramPhone = new SqlParameter("@Phone", SqlDbType.NVarChar, 50);
            SqlParameter paramFax = new SqlParameter("@Fax", SqlDbType.NVarChar, 50);
            SqlParameter paramEmail = new SqlParameter("@Email", SqlDbType.NVarChar, 50);
            SqlParameter paramSite = new SqlParameter("@Site", SqlDbType.NVarChar, 100);
            SqlParameter paramBank = new SqlParameter("@Bank", SqlDbType.NVarChar, 50);
            SqlParameter paramAccount = new SqlParameter("@Account", SqlDbType.NVarChar, 50);
            SqlParameter paramTaxNo = new SqlParameter("@TaxNo", SqlDbType.NVarChar, 50);
            SqlParameter paramComment = new SqlParameter("@Comment", SqlDbType.NVarChar, 20);
            SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.NVarChar, 500);
            SqlParameter paramWarrantyAreaSysNo = new SqlParameter("@WarrantyAreaSysNo", SqlDbType.Int, 4);
            SqlParameter paramWarrantyAddress = new SqlParameter("@WarrantyAddress", SqlDbType.NVarChar, 100);
            SqlParameter paramWarrantyZip = new SqlParameter("@WarrantyZip", SqlDbType.NVarChar, 10);
            SqlParameter paramWarrantyContact = new SqlParameter("@WarrantyContact", SqlDbType.NVarChar, 20);
            SqlParameter paramWarrantyPhone = new SqlParameter("@WarrantyPhone", SqlDbType.NVarChar, 50);
            SqlParameter paramWarrantyFax = new SqlParameter("@WarrantyFax", SqlDbType.NVarChar, 50);
            SqlParameter paramWarrantyEmail = new SqlParameter("@WarrantyEmail", SqlDbType.NVarChar, 50);
            SqlParameter paramWarrantySite = new SqlParameter("@WarrantySite", SqlDbType.NVarChar, 100);
            SqlParameter paramWarrantySelfSend = new SqlParameter("@WarrantySelfSend", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramRMAPosition = new SqlParameter("@RMAPosition", SqlDbType.NVarChar, 50);
            SqlParameter paramAPType = new SqlParameter("@APType", SqlDbType.Int, 4);
            SqlParameter paramCooperateType = new SqlParameter("@CooperateType", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.VendorID != AppConst.StringNull)
                paramVendorID.Value = oParam.VendorID;
            else
                paramVendorID.Value = System.DBNull.Value;
            if (oParam.VendorName != AppConst.StringNull)
                paramVendorName.Value = oParam.VendorName;
            else
                paramVendorName.Value = System.DBNull.Value;
            if (oParam.EnglishName != AppConst.StringNull)
                paramEnglishName.Value = oParam.EnglishName;
            else
                paramEnglishName.Value = System.DBNull.Value;
            if (oParam.BriefName != AppConst.StringNull)
                paramBriefName.Value = oParam.BriefName;
            else
                paramBriefName.Value = System.DBNull.Value;
            if (oParam.VendorType != AppConst.IntNull)
                paramVendorType.Value = oParam.VendorType;
            else
                paramVendorType.Value = System.DBNull.Value;
            if (oParam.District != AppConst.StringNull)
                paramDistrict.Value = oParam.District;
            else
                paramDistrict.Value = System.DBNull.Value;
            if (oParam.Address != AppConst.StringNull)
                paramAddress.Value = oParam.Address;
            else
                paramAddress.Value = System.DBNull.Value;
            if (oParam.Zip != AppConst.StringNull)
                paramZip.Value = oParam.Zip;
            else
                paramZip.Value = System.DBNull.Value;
            if (oParam.Contact != AppConst.StringNull)
                paramContact.Value = oParam.Contact;
            else
                paramContact.Value = System.DBNull.Value;
            if (oParam.Phone != AppConst.StringNull)
                paramPhone.Value = oParam.Phone;
            else
                paramPhone.Value = System.DBNull.Value;
            if (oParam.Fax != AppConst.StringNull)
                paramFax.Value = oParam.Fax;
            else
                paramFax.Value = System.DBNull.Value;
            if (oParam.Email != AppConst.StringNull)
                paramEmail.Value = oParam.Email;
            else
                paramEmail.Value = System.DBNull.Value;
            if (oParam.Site != AppConst.StringNull)
                paramSite.Value = oParam.Site;
            else
                paramSite.Value = System.DBNull.Value;
            if (oParam.Bank != AppConst.StringNull)
                paramBank.Value = oParam.Bank;
            else
                paramBank.Value = System.DBNull.Value;
            if (oParam.Account != AppConst.StringNull)
                paramAccount.Value = oParam.Account;
            else
                paramAccount.Value = System.DBNull.Value;
            if (oParam.TaxNo != AppConst.StringNull)
                paramTaxNo.Value = oParam.TaxNo;
            else
                paramTaxNo.Value = System.DBNull.Value;
            if (oParam.Comment != AppConst.StringNull)
                paramComment.Value = oParam.Comment;
            else
                paramComment.Value = System.DBNull.Value;
            if (oParam.Note != AppConst.StringNull)
                paramNote.Value = oParam.Note;
            else
                paramNote.Value = System.DBNull.Value;
            if (oParam.WarrantyAreaSysNo != AppConst.IntNull)
                paramWarrantyAreaSysNo.Value = oParam.WarrantyAreaSysNo;
            else
                paramWarrantyAreaSysNo.Value = System.DBNull.Value;
            if (oParam.WarrantyAddress != AppConst.StringNull)
                paramWarrantyAddress.Value = oParam.WarrantyAddress;
            else
                paramWarrantyAddress.Value = System.DBNull.Value;
            if (oParam.WarrantyZip != AppConst.StringNull)
                paramWarrantyZip.Value = oParam.WarrantyZip;
            else
                paramWarrantyZip.Value = System.DBNull.Value;
            if (oParam.WarrantyContact != AppConst.StringNull)
                paramWarrantyContact.Value = oParam.WarrantyContact;
            else
                paramWarrantyContact.Value = System.DBNull.Value;
            if (oParam.WarrantyPhone != AppConst.StringNull)
                paramWarrantyPhone.Value = oParam.WarrantyPhone;
            else
                paramWarrantyPhone.Value = System.DBNull.Value;
            if (oParam.WarrantyFax != AppConst.StringNull)
                paramWarrantyFax.Value = oParam.WarrantyFax;
            else
                paramWarrantyFax.Value = System.DBNull.Value;
            if (oParam.WarrantyEmail != AppConst.StringNull)
                paramWarrantyEmail.Value = oParam.WarrantyEmail;
            else
                paramWarrantyEmail.Value = System.DBNull.Value;
            if (oParam.WarrantySite != AppConst.StringNull)
                paramWarrantySite.Value = oParam.WarrantySite;
            else
                paramWarrantySite.Value = System.DBNull.Value;
            if (oParam.WarrantySelfSend != AppConst.IntNull)
                paramWarrantySelfSend.Value = oParam.WarrantySelfSend;
            else
                paramWarrantySelfSend.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;
            if (oParam.RMAPosition != AppConst.StringNull)
                paramRMAPosition.Value = oParam.RMAPosition;
            else
                paramRMAPosition.Value = System.DBNull.Value;
            if (oParam.APType != AppConst.IntNull)
                paramAPType.Value = oParam.APType;
            else
                paramAPType.Value = System.DBNull.Value;
            if (oParam.CooperateType != AppConst.IntNull)
                paramCooperateType.Value = oParam.CooperateType;
            else
                paramCooperateType.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramVendorID);
            cmd.Parameters.Add(paramVendorName);
            cmd.Parameters.Add(paramEnglishName);
            cmd.Parameters.Add(paramBriefName);
            cmd.Parameters.Add(paramVendorType);
            cmd.Parameters.Add(paramDistrict);
            cmd.Parameters.Add(paramAddress);
            cmd.Parameters.Add(paramZip);
            cmd.Parameters.Add(paramContact);
            cmd.Parameters.Add(paramPhone);
            cmd.Parameters.Add(paramFax);
            cmd.Parameters.Add(paramEmail);
            cmd.Parameters.Add(paramSite);
            cmd.Parameters.Add(paramBank);
            cmd.Parameters.Add(paramAccount);
            cmd.Parameters.Add(paramTaxNo);
            cmd.Parameters.Add(paramComment);
            cmd.Parameters.Add(paramNote);
            cmd.Parameters.Add(paramWarrantyAreaSysNo);
            cmd.Parameters.Add(paramWarrantyAddress);
            cmd.Parameters.Add(paramWarrantyZip);
            cmd.Parameters.Add(paramWarrantyContact);
            cmd.Parameters.Add(paramWarrantyPhone);
            cmd.Parameters.Add(paramWarrantyFax);
            cmd.Parameters.Add(paramWarrantyEmail);
            cmd.Parameters.Add(paramWarrantySite);
            cmd.Parameters.Add(paramWarrantySelfSend);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramRMAPosition);
            cmd.Parameters.Add(paramAPType);
            cmd.Parameters.Add(paramCooperateType);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int Deletes(string SysNos)
        {
            string sql = "delete from Vendor where sysno in ("+SysNos+")";
            return SqlHelper.ExecuteNonQuery(sql);
        }
    }
}
