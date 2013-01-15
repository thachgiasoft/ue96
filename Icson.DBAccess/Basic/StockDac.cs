using System;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects.Basic;

namespace Icson.DBAccess.Basic
{
	/// <summary>
	/// Summary description for StockDac.
	/// </summary>
	public class StockDac
	{
		
		public StockDac()
		{
		}
		public int Insert(StockInfo oParam)
		{
			string sql = @"INSERT INTO Stock
                            (
                            SysNo, StockID, StockName, Address, 
                            Contact, Phone, Status, StockType
                            )
                            VALUES (
                            @SysNo, @StockID, @StockName, @Address, 
                            @Contact, @Phone, @Status, @StockType
                            )";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramStockID = new SqlParameter("@StockID", SqlDbType.NVarChar,20);
			SqlParameter paramStockName = new SqlParameter("@StockName", SqlDbType.NVarChar,50);
			SqlParameter paramAddress = new SqlParameter("@Address", SqlDbType.NVarChar,100);
			SqlParameter paramContact = new SqlParameter("@Contact", SqlDbType.NVarChar,20);
			SqlParameter paramPhone = new SqlParameter("@Phone", SqlDbType.NVarChar,50);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);
            SqlParameter paramStockType = new SqlParameter("@StockType",SqlDbType.Int,4);

			paramSysNo.Value = oParam.SysNo;
			paramStockID.Value = oParam.StockID;
			paramStockName.Value = oParam.StockName;
			paramAddress.Value = oParam.Address;
			paramContact.Value = oParam.Contact;
			paramPhone.Value = oParam.Phone;
			paramStatus.Value = oParam.Status;
            paramStockType.Value = oParam.StockType;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramStockID);
			cmd.Parameters.Add(paramStockName);
			cmd.Parameters.Add(paramAddress);
			cmd.Parameters.Add(paramContact);
			cmd.Parameters.Add(paramPhone);
			cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramStockType);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public int Update(StockInfo oParam)  //StockType 不做调整
		{
			string sql = @"UPDATE Stock SET 
                            StockID=@StockID, 
                            StockName=@StockName, Address=@Address, 
                            Contact=@Contact, Phone=@Phone, 
                            Status=@Status
                            WHERE SysNo=@SysNo";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramStockID = new SqlParameter("@StockID", SqlDbType.NVarChar,20);
			SqlParameter paramStockName = new SqlParameter("@StockName", SqlDbType.NVarChar,50);
			SqlParameter paramAddress = new SqlParameter("@Address", SqlDbType.NVarChar,100);
			SqlParameter paramContact = new SqlParameter("@Contact", SqlDbType.NVarChar,20);
			SqlParameter paramPhone = new SqlParameter("@Phone", SqlDbType.NVarChar,50);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);

			paramSysNo.Value = oParam.SysNo;
			paramStockID.Value = oParam.StockID;
			paramStockName.Value = oParam.StockName;
			paramAddress.Value = oParam.Address;
			paramContact.Value = oParam.Contact;
			paramPhone.Value = oParam.Phone;
			paramStatus.Value = oParam.Status;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramStockID);
			cmd.Parameters.Add(paramStockName);
			cmd.Parameters.Add(paramAddress);
			cmd.Parameters.Add(paramContact);
			cmd.Parameters.Add(paramPhone);
			cmd.Parameters.Add(paramStatus);

			return SqlHelper.ExecuteNonQuery(cmd);
		}
	}
}
