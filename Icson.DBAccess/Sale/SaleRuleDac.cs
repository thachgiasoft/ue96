using System;
using System.Data;
using System.Data.SqlClient;
using Icson.Utils;
using Icson.Objects.Sale;

namespace Icson.DBAccess.Sale
{
	/// <summary>
	/// Summary description for SaleRuleDac.
	/// </summary>
	public class SaleRuleDac
	{
		public SaleRuleDac()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		
		public int InsertMaster(SaleRuleInfo oParam)
		{
			string sql = @"INSERT INTO SaleRule_Master
                            (SaleRuleName, Status ,CreateUserSysNo)
                            VALUES (@SaleRuleName, @Status, @CreateUserSysNo)
							set @SysNo = SCOPE_IDENTITY()";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramSaleRuleName = new SqlParameter("@SaleRuleName", SqlDbType.NVarChar,500);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);
			SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int,4);

			paramSysNo.Direction = ParameterDirection.Output;
			if ( oParam.SaleRuleName != AppConst.StringNull)
				paramSaleRuleName.Value = oParam.SaleRuleName;
			else
				paramSaleRuleName.Value = System.DBNull.Value;
			if ( oParam.Status != AppConst.IntNull)
				paramStatus.Value = oParam.Status;
			else
				paramStatus.Value = System.DBNull.Value;		
			if ( oParam.CreateUserSysNo != AppConst.IntNull)
				paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
			else
				paramCreateUserSysNo.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramSaleRuleName);
			cmd.Parameters.Add(paramStatus);
			cmd.Parameters.Add(paramCreateUserSysNo);

			return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);

		}

		public int InsertItem(SaleRuleItemInfo oParam)
		{
			string sql = @"INSERT INTO SaleRule_Item
                            (
                            SaleRuleSysNo, ProductSysNo, Quantity, 
                            Discount
                            )
                            VALUES (
                            @SaleRuleSysNo, @ProductSysNo, @Quantity, 
                            @Discount
                            )";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramSaleRuleSysNo = new SqlParameter("@SaleRuleSysNo", SqlDbType.Int,4);
			SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int,4);
			SqlParameter paramQuantity = new SqlParameter("@Quantity", SqlDbType.Int,4);
			SqlParameter paramDiscount = new SqlParameter("@Discount", SqlDbType.Decimal,9);
			SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);

			if ( oParam.SaleRuleSysNo != AppConst.IntNull)
				paramSaleRuleSysNo.Value = oParam.SaleRuleSysNo;
			else
				paramSaleRuleSysNo.Value = System.DBNull.Value;
			if ( oParam.ProductSysNo != AppConst.IntNull)
				paramProductSysNo.Value = oParam.ProductSysNo;
			else
				paramProductSysNo.Value = System.DBNull.Value;
			if ( oParam.Quantity != AppConst.IntNull)
				paramQuantity.Value = oParam.Quantity;
			else
				paramQuantity.Value = System.DBNull.Value;
			if ( oParam.Discount != AppConst.DecimalNull)
				paramDiscount.Value = oParam.Discount;
			else
				paramDiscount.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSaleRuleSysNo);
			cmd.Parameters.Add(paramProductSysNo);
			cmd.Parameters.Add(paramQuantity);
			cmd.Parameters.Add(paramDiscount);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

        public int UpdateItem(SaleRuleItemInfo oParam)
        {

            string sql = @"UPDATE SaleRule_Item SET ProductSysNo=@ProductSysNo,Quantity=@Quantity,Discount=@Discount WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int, 4);
            SqlParameter paramQuantity = new SqlParameter("@Quantity", SqlDbType.Int, 4);
            SqlParameter paramDiscount = new SqlParameter("@Discount", SqlDbType.Decimal, 9);
            //SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.ProductSysNo != AppConst.IntNull)
                paramProductSysNo.Value = oParam.ProductSysNo;
            else
                paramProductSysNo.Value = System.DBNull.Value;
            if (oParam.Quantity != AppConst.IntNull)
                paramQuantity.Value = oParam.Quantity;
            else
                paramQuantity.Value = System.DBNull.Value;
            if (oParam.Discount != AppConst.DecimalNull)
                paramDiscount.Value = oParam.Discount;
            else
                paramDiscount.Value = System.DBNull.Value;
            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramProductSysNo);
            cmd.Parameters.Add(paramQuantity);
            cmd.Parameters.Add(paramDiscount);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        //批量删除促销商品
        public int DeleteItemBatch(string sysNos)
        {
            string sql = "delete from SaleRule_Item where SysNo in(" + sysNos + ")";
            return SqlHelper.ExecuteNonQuery(sql);
        }

		public int UpdateMaster(SaleRuleInfo oParam)
		{
			string sql = @"UPDATE SaleRule_Master SET 
                           SaleRuleName=@SaleRuleName, 
                           Status=@Status
                           WHERE SysNo=@SysNo";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramSaleRuleName = new SqlParameter("@SaleRuleName", SqlDbType.NVarChar,500);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);

			if ( oParam.SysNo != AppConst.IntNull)
				paramSysNo.Value = oParam.SysNo;
			else
				throw new Exception("SysNo can't be null");
			if ( oParam.SaleRuleName != AppConst.StringNull)
				paramSaleRuleName.Value = oParam.SaleRuleName;
			else
				paramSaleRuleName.Value = System.DBNull.Value;
			if ( oParam.Status != AppConst.IntNull)
				paramStatus.Value = oParam.Status;
			else
				paramStatus.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramSaleRuleName);
			cmd.Parameters.Add(paramStatus);

			return SqlHelper.ExecuteNonQuery(cmd);
		}
	
		public int DeleteItem(int paramSysNo)
		{
			string sql = @"Delete from SaleRule_Item where SysNo ="+paramSysNo;
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);
		}
	}
}
