using System;
using System.Data;
using System.Data.SqlClient;
using Icson.Utils;
using Icson.Objects.Sale;

namespace Icson.DBAccess.Sale
{
	/// <summary>
	/// Summary description for SOVATDac.
	/// </summary>
	public class SOVATDac
	{
		public SOVATDac()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		
		public int Insert(SOVATInfo oParam)
		{
			string sql = @"INSERT INTO SO_ValueAdded_Invoice
                            (
                            SOSysNo, CustomerSysNo, CompanyName, 
                            TaxNum, CompanyAddress, CompanyPhone, BankAccount, 
                            Memo,VATEMSFee
                            )
                            VALUES (
                            @SOSysNo, @CustomerSysNo, @CompanyName, 
                            @TaxNum, @CompanyAddress, @CompanyPhone, @BankAccount, 
                            @Memo,@VATEMSFee
                            )";
			
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int,4);
			SqlParameter paramCustomerSysNo = new SqlParameter("@CustomerSysNo", SqlDbType.Int,4);
			SqlParameter paramCompanyName = new SqlParameter("@CompanyName", SqlDbType.NVarChar,100);
			SqlParameter paramTaxNum = new SqlParameter("@TaxNum", SqlDbType.NVarChar,20);
			SqlParameter paramCompanyAddress = new SqlParameter("@CompanyAddress", SqlDbType.NVarChar,200);
			SqlParameter paramCompanyPhone = new SqlParameter("@CompanyPhone", SqlDbType.NVarChar,10);
			SqlParameter paramBankAccount = new SqlParameter("@BankAccount", SqlDbType.NVarChar,100);
			SqlParameter paramMemo = new SqlParameter("@Memo", SqlDbType.NVarChar,200);
			SqlParameter paramVATEMSFee = new SqlParameter("@VATEMSFee", SqlDbType.Decimal,9);
			

			if ( oParam.SOSysNo != AppConst.IntNull)
				paramSOSysNo.Value = oParam.SOSysNo;
			else
				paramSOSysNo.Value = System.DBNull.Value;
			if ( oParam.CustomerSysNo != AppConst.IntNull)
				paramCustomerSysNo.Value = oParam.CustomerSysNo;
			else
				paramCustomerSysNo.Value = System.DBNull.Value;
			if ( oParam.CompanyName != AppConst.StringNull)
				paramCompanyName.Value = oParam.CompanyName;
			else
				paramCompanyName.Value = System.DBNull.Value;
			if ( oParam.TaxNum != AppConst.StringNull)
				paramTaxNum.Value = oParam.TaxNum;
			else
				paramTaxNum.Value = System.DBNull.Value;
			if ( oParam.CompanyAddress != AppConst.StringNull)
				paramCompanyAddress.Value = oParam.CompanyAddress;
			else
				paramCompanyAddress.Value = System.DBNull.Value;
			if ( oParam.CompanyPhone != AppConst.StringNull)
				paramCompanyPhone.Value = oParam.CompanyPhone;
			else
				paramCompanyPhone.Value = System.DBNull.Value;
			if ( oParam.BankAccount != AppConst.StringNull)
				paramBankAccount.Value = oParam.BankAccount;
			else
				paramBankAccount.Value = System.DBNull.Value;
			if ( oParam.Memo != AppConst.StringNull)
				paramMemo.Value = oParam.Memo;
			else
				paramMemo.Value = System.DBNull.Value;
			if ( oParam.VATEMSFee != AppConst.DecimalNull)
				paramVATEMSFee.Value = oParam.VATEMSFee;
			else
				paramVATEMSFee.Value = System.DBNull.Value;


			cmd.Parameters.Add(paramSOSysNo);
			cmd.Parameters.Add(paramCustomerSysNo);
			cmd.Parameters.Add(paramCompanyName);
			cmd.Parameters.Add(paramTaxNum);
			cmd.Parameters.Add(paramCompanyAddress);
			cmd.Parameters.Add(paramCompanyPhone);
			cmd.Parameters.Add(paramBankAccount);
			cmd.Parameters.Add(paramMemo);
			cmd.Parameters.Add(paramVATEMSFee);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public int Update(SOVATInfo oParam)
		{
			string sql = @"UPDATE SO_ValueAdded_Invoice SET 
                           CustomerSysNo=@CustomerSysNo, CompanyName=@CompanyName, 
                           TaxNum=@TaxNum, CompanyAddress=@CompanyAddress, 
                           CompanyPhone=@CompanyPhone, BankAccount=@BankAccount, 
                           Memo=@Memo,VATEMSFee=@VATEMSFee
                           WHERE SOSysNo=@SOSysNo";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int,4);
			SqlParameter paramCustomerSysNo = new SqlParameter("@CustomerSysNo", SqlDbType.Int,4);
			SqlParameter paramCompanyName = new SqlParameter("@CompanyName", SqlDbType.NVarChar,100);
			SqlParameter paramTaxNum = new SqlParameter("@TaxNum", SqlDbType.NVarChar,20);
			SqlParameter paramCompanyAddress = new SqlParameter("@CompanyAddress", SqlDbType.NVarChar,200);
			SqlParameter paramCompanyPhone = new SqlParameter("@CompanyPhone", SqlDbType.NVarChar,10);
			SqlParameter paramBankAccount = new SqlParameter("@BankAccount", SqlDbType.NVarChar,100);
			SqlParameter paramMemo = new SqlParameter("@Memo", SqlDbType.NVarChar,200);
			SqlParameter paramVATEMSFee = new SqlParameter("@VATEMSFee", SqlDbType.Decimal,9);			

			if ( oParam.SOSysNo != AppConst.IntNull)
				paramSOSysNo.Value = oParam.SOSysNo;
			else
				paramSOSysNo.Value = System.DBNull.Value;
			if ( oParam.CustomerSysNo != AppConst.IntNull)
				paramCustomerSysNo.Value = oParam.CustomerSysNo;
			else
				paramCustomerSysNo.Value = System.DBNull.Value;
			if ( oParam.CompanyName != AppConst.StringNull)
				paramCompanyName.Value = oParam.CompanyName;
			else
				paramCompanyName.Value = System.DBNull.Value;
			if ( oParam.TaxNum != AppConst.StringNull)
				paramTaxNum.Value = oParam.TaxNum;
			else
				paramTaxNum.Value = System.DBNull.Value;
			if ( oParam.CompanyAddress != AppConst.StringNull)
				paramCompanyAddress.Value = oParam.CompanyAddress;
			else
				paramCompanyAddress.Value = System.DBNull.Value;
			if ( oParam.CompanyPhone != AppConst.StringNull)
				paramCompanyPhone.Value = oParam.CompanyPhone;
			else
				paramCompanyPhone.Value = System.DBNull.Value;
			if ( oParam.BankAccount != AppConst.StringNull)
				paramBankAccount.Value = oParam.BankAccount;
			else
				paramBankAccount.Value = System.DBNull.Value;
			if ( oParam.Memo != AppConst.StringNull)
				paramMemo.Value = oParam.Memo;
			else
				paramMemo.Value = System.DBNull.Value;
			if( oParam.VATEMSFee != AppConst.DecimalNull)
				paramVATEMSFee.Value = oParam.VATEMSFee;
			else
				paramVATEMSFee.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSOSysNo);
			cmd.Parameters.Add(paramCustomerSysNo);
			cmd.Parameters.Add(paramCompanyName);
			cmd.Parameters.Add(paramTaxNum);
			cmd.Parameters.Add(paramCompanyAddress);
			cmd.Parameters.Add(paramCompanyPhone);
			cmd.Parameters.Add(paramBankAccount);
			cmd.Parameters.Add(paramMemo);
			cmd.Parameters.Add(paramVATEMSFee);

			return SqlHelper.ExecuteNonQuery(cmd);
		}
	}
}
