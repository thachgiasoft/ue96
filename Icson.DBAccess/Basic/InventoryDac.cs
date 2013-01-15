using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Text;

using Icson.Utils;
using Icson.Objects.Basic;

namespace Icson.DBAccess.Basic
{
	/// <summary>
	/// Summary description for InventoryDac.
	/// </summary>
	public class InventoryDac
	{
		
		public InventoryDac()
		{
		}

		public int InitInventory(int productSysNo)
		{
			string sql = "insert into inventory(productsysno) values(" + productSysNo + ")";
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);
		}
		public int InitInventoryStock(int productSysNo, int stockSysNo)
		{
			string sql = "insert into inventory_stock(productsysno, stocksysno) values(" + productSysNo + "," + stockSysNo + ")";
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);
		}
		
		public int UpdateQty(string sql)
		{
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);
		}

        public int SetPosition(int sysno, string pos1, string pos2)
        {
            string sql = "update inventory_stock set position1=" + Util.ToSqlString(pos1) + ", position2=" + Util.ToSqlString(pos2) + ",poslastupdatetime='" + DateTime.Now.ToString() + "' where sysno = " + sysno;
            SqlCommand cmd = new SqlCommand(sql);
            return SqlHelper.ExecuteNonQuery(cmd);
        }



		#region 下面两个insert 目前仅仅用在初始化部分
		public int Insert(InventoryInfo oParam)
		{
			string sql = @"INSERT INTO Inventory
                            (
                            ProductSysNo, AccountQty, AvailableQty, 
                            AllocatedQty, OrderQty, PurchaseQty, VirtualQty
                            )
                            VALUES (
                            @ProductSysNo, @AccountQty, @AvailableQty, 
                            @AllocatedQty, @OrderQty, @PurchaseQty, @VirtualQty
                            )";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int,4);
			SqlParameter paramAccountQty = new SqlParameter("@AccountQty", SqlDbType.Int,4);
			SqlParameter paramAvailableQty = new SqlParameter("@AvailableQty", SqlDbType.Int,4);
			SqlParameter paramAllocatedQty = new SqlParameter("@AllocatedQty", SqlDbType.Int,4);
			SqlParameter paramOrderQty = new SqlParameter("@OrderQty", SqlDbType.Int,4);
			SqlParameter paramPurchaseQty = new SqlParameter("@PurchaseQty", SqlDbType.Int,4);
			SqlParameter paramVirtualQty = new SqlParameter("@VirtualQty", SqlDbType.Int,4);

			paramProductSysNo.Value = oParam.ProductSysNo;

			if ( oParam.AccountQty != AppConst.IntNull)
				paramAccountQty.Value = oParam.AccountQty;
			else
				paramAccountQty.Value = System.DBNull.Value;
			if ( oParam.AvailableQty != AppConst.IntNull)
				paramAvailableQty.Value = oParam.AvailableQty;
			else
				paramAvailableQty.Value = System.DBNull.Value;
			if ( oParam.AllocatedQty != AppConst.IntNull)
				paramAllocatedQty.Value = oParam.AllocatedQty;
			else
				paramAllocatedQty.Value = System.DBNull.Value;
			if ( oParam.OrderQty != AppConst.IntNull)
				paramOrderQty.Value = oParam.OrderQty;
			else
				paramOrderQty.Value = System.DBNull.Value;
			if ( oParam.PurchaseQty != AppConst.IntNull)
				paramPurchaseQty.Value = oParam.PurchaseQty;
			else
				paramPurchaseQty.Value = System.DBNull.Value;
			if ( oParam.VirtualQty != AppConst.IntNull)
				paramVirtualQty.Value = oParam.VirtualQty;
			else
				paramVirtualQty.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramProductSysNo);
			cmd.Parameters.Add(paramAccountQty);
			cmd.Parameters.Add(paramAvailableQty);
			cmd.Parameters.Add(paramAllocatedQty);
			cmd.Parameters.Add(paramOrderQty);
			cmd.Parameters.Add(paramPurchaseQty);
			cmd.Parameters.Add(paramVirtualQty);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

        public int Insert(InventoryStockInfo oParam)
        {
            string sql = @"INSERT INTO Inventory_Stock
                            (
                            StockSysNo, ProductSysNo, AccountQty, 
                            AvailableQty, AllocatedQty, OrderQty, PurchaseQty, 
                            ShiftInQty, ShiftOutQty, SafeQty, Position1, 
                            Position2,PosLastUpdateTime
                            )
                            VALUES (
                            @StockSysNo, @ProductSysNo, @AccountQty, 
                            @AvailableQty, @AllocatedQty, @OrderQty, @PurchaseQty, 
                            @ShiftInQty, @ShiftOutQty, @SafeQty, @Position1, 
                            @Position2,@PosLastUpdateTime
                            )";

            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramStockSysNo = new SqlParameter("@StockSysNo", SqlDbType.Int, 4);
            SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int, 4);
            SqlParameter paramAccountQty = new SqlParameter("@AccountQty", SqlDbType.Int, 4);
            SqlParameter paramAvailableQty = new SqlParameter("@AvailableQty", SqlDbType.Int, 4);
            SqlParameter paramAllocatedQty = new SqlParameter("@AllocatedQty", SqlDbType.Int, 4);
            SqlParameter paramOrderQty = new SqlParameter("@OrderQty", SqlDbType.Int, 4);
            SqlParameter paramPurchaseQty = new SqlParameter("@PurchaseQty", SqlDbType.Int, 4);
            SqlParameter paramShiftInQty = new SqlParameter("@ShiftInQty", SqlDbType.Int, 4);
            SqlParameter paramShiftOutQty = new SqlParameter("@ShiftOutQty", SqlDbType.Int, 4);
            SqlParameter paramSafeQty = new SqlParameter("@SafeQty", SqlDbType.Int, 4);
            SqlParameter paramPosition1 = new SqlParameter("@Position1", SqlDbType.NVarChar, 20);
            SqlParameter paramPosition2 = new SqlParameter("@Position2", SqlDbType.NVarChar, 20);
            SqlParameter paramPosLastUpdateTime = new SqlParameter("@PosLastUpdateTime", SqlDbType.DateTime);

            paramStockSysNo.Value = oParam.StockSysNo;
            paramProductSysNo.Value = oParam.ProductSysNo;

            if (oParam.AccountQty != AppConst.IntNull)
                paramAccountQty.Value = oParam.AccountQty;
            else
                paramAccountQty.Value = System.DBNull.Value;
            if (oParam.AvailableQty != AppConst.IntNull)
                paramAvailableQty.Value = oParam.AvailableQty;
            else
                paramAvailableQty.Value = System.DBNull.Value;
            if (oParam.AllocatedQty != AppConst.IntNull)
                paramAllocatedQty.Value = oParam.AllocatedQty;
            else
                paramAllocatedQty.Value = System.DBNull.Value;
            if (oParam.OrderQty != AppConst.IntNull)
                paramOrderQty.Value = oParam.OrderQty;
            else
                paramOrderQty.Value = System.DBNull.Value;
            if (oParam.PurchaseQty != AppConst.IntNull)
                paramPurchaseQty.Value = oParam.PurchaseQty;
            else
                paramPurchaseQty.Value = System.DBNull.Value;
            if (oParam.ShiftInQty != AppConst.IntNull)
                paramShiftInQty.Value = oParam.ShiftInQty;
            else
                paramShiftInQty.Value = System.DBNull.Value;
            if (oParam.ShiftOutQty != AppConst.IntNull)
                paramShiftOutQty.Value = oParam.ShiftOutQty;
            else
                paramShiftOutQty.Value = System.DBNull.Value;
            if (oParam.SafeQty != AppConst.IntNull)
                paramSafeQty.Value = oParam.SafeQty;
            else
                paramSafeQty.Value = System.DBNull.Value;
            if (oParam.Position1 != AppConst.StringNull)
                paramPosition1.Value = oParam.Position1;
            else
                paramPosition1.Value = System.DBNull.Value;
            if (oParam.Position2 != AppConst.StringNull)
                paramPosition2.Value = oParam.Position2;
            else
                paramPosition2.Value = System.DBNull.Value;
            if (oParam.PosLastUpdateTime != AppConst.DateTimeNull)
                paramPosLastUpdateTime.Value = oParam.PosLastUpdateTime;
            else
                paramPosLastUpdateTime.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramStockSysNo);
            cmd.Parameters.Add(paramProductSysNo);
            cmd.Parameters.Add(paramAccountQty);
            cmd.Parameters.Add(paramAvailableQty);
            cmd.Parameters.Add(paramAllocatedQty);
            cmd.Parameters.Add(paramOrderQty);
            cmd.Parameters.Add(paramPurchaseQty);
            cmd.Parameters.Add(paramShiftInQty);
            cmd.Parameters.Add(paramShiftOutQty);
            cmd.Parameters.Add(paramSafeQty);
            cmd.Parameters.Add(paramPosition1);
            cmd.Parameters.Add(paramPosition2);
            cmd.Parameters.Add(paramPosLastUpdateTime);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
		#endregion
	}
}
