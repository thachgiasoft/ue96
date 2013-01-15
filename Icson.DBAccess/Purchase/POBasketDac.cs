using System;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects.Purchase;

namespace Icson.DBAccess.Purchase
{
	/// <summary>
	/// Summary description for POBasketDac.
	/// </summary>
	public class POBasketDac
	{
		
		public POBasketDac()
		{
		}

		public int Insert(POBasketInfo oParam)
		{
			string sql = @"INSERT INTO PO_Basket
                            (
                            CreateUserSysNo, ProductSysNo, Quantity, 
                            OrderPrice
                            )
                            VALUES (
                            @CreateUserSysNo, @ProductSysNo, @Quantity, 
                            @OrderPrice
                            );set @SysNo = SCOPE_IDENTITY();";

			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int,4);
			SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int,4);
			SqlParameter paramQuantity = new SqlParameter("@Quantity", SqlDbType.Int,4);
			SqlParameter paramOrderPrice = new SqlParameter("@OrderPrice", SqlDbType.Decimal,9);

			paramSysNo.Direction = ParameterDirection.Output;

			if ( oParam.CreateUserSysNo != AppConst.IntNull)
				paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
			else
				paramCreateUserSysNo.Value = System.DBNull.Value;
			if ( oParam.ProductSysNo != AppConst.IntNull)
				paramProductSysNo.Value = oParam.ProductSysNo;
			else
				paramProductSysNo.Value = System.DBNull.Value;
			if ( oParam.Quantity != AppConst.IntNull)
				paramQuantity.Value = oParam.Quantity;
			else
				paramQuantity.Value = System.DBNull.Value;
			if ( oParam.OrderPrice != AppConst.DecimalNull)
				paramOrderPrice.Value = oParam.OrderPrice;
			else
				paramOrderPrice.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramCreateUserSysNo);
			cmd.Parameters.Add(paramProductSysNo);
			cmd.Parameters.Add(paramQuantity);
			cmd.Parameters.Add(paramOrderPrice);

			return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);

		}

		public int Update(POBasketInfo oParam)
		{
			string sql = @"UPDATE PO_Basket SET 
                            Quantity=@Quantity, OrderPrice = @OrderPrice
                            WHERE SysNo=@SysNo";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramQuantity = new SqlParameter("@Quantity", SqlDbType.Int,4);
			SqlParameter paramOrderPrice = new SqlParameter("@OrderPrice", SqlDbType.Decimal,9);

			if ( oParam.SysNo != AppConst.IntNull)
				paramSysNo.Value = oParam.SysNo;
			else
				paramSysNo.Value = System.DBNull.Value;
			if ( oParam.Quantity != AppConst.IntNull)
				paramQuantity.Value = oParam.Quantity;
			else
				paramQuantity.Value = System.DBNull.Value;
			if ( oParam.OrderPrice != AppConst.DecimalNull)
				paramOrderPrice.Value = oParam.OrderPrice;
			else
				paramOrderPrice.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramQuantity);
			cmd.Parameters.Add(paramOrderPrice);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public int Delete(int sysno)
		{
			string sql = "delete from po_basket where sysno = " + sysno;

			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);

		}

        //批量删除采购蓝信息
        public int Deletes(string SysNos)
        {
            string sql = "delete from po_basket where sysno in (" + SysNos+")";

            SqlCommand cmd = new SqlCommand(sql);
            return SqlHelper.ExecuteNonQuery(cmd);

        }

		public int Delete(int productSysNo, int userSysNo)
		{
			string sql = "delete from po_basket where productSysNo = " + productSysNo + " and createusersysno=" + userSysNo;

			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);

		}
	}
}
