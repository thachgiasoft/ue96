using System;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects.Basic;

namespace Icson.DBAccess.Basic
{
	/// <summary>
	/// Summary description for ProductPriceDac.
	/// </summary>
	public class ProductPriceDac
	{
		
		public ProductPriceDac()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public int Insert(ProductPriceInfo oParam)
		{
			string sql = @"INSERT INTO product_price
                            (
                            ProductSysNo, BasicPrice, CurrentPrice, UnitCost,
                            Discount, PointType, IsWholeSale, Q1, 
                            P1, Q2, P2, Q3, 
                            P3, CashRebate, Point, ClearanceSale, LastOrderPrice, LastMarketLowestPrice, LimitedQty
                            )
                            VALUES (
                            @ProductSysNo, @BasicPrice, @CurrentPrice, @UnitCost,
                            @Discount, @PointType, @IsWholeSale, @Q1, 
                            @P1, @Q2, @P2, @Q3, 
                            @P3, @CashRebate, @Point, @ClearanceSale, @LastOrderPrice, @LastMarketLowestPrice, @LimitedQty
                            )";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int,4);
			SqlParameter paramBasicPrice = new SqlParameter("@BasicPrice", SqlDbType.Decimal,9);
			SqlParameter paramCurrentPrice = new SqlParameter("@CurrentPrice", SqlDbType.Decimal,9);
			SqlParameter paramUnitCost = new SqlParameter("@UnitCost", SqlDbType.Decimal,9);
			SqlParameter paramDiscount = new SqlParameter("@Discount", SqlDbType.Decimal,9);
			SqlParameter paramPointType = new SqlParameter("@PointType", SqlDbType.Int,4);
			SqlParameter paramIsWholeSale = new SqlParameter("@IsWholeSale", SqlDbType.Int,4);
			SqlParameter paramQ1 = new SqlParameter("@Q1", SqlDbType.Int,4);
			SqlParameter paramP1 = new SqlParameter("@P1", SqlDbType.Decimal,9);
			SqlParameter paramQ2 = new SqlParameter("@Q2", SqlDbType.Int,4);
			SqlParameter paramP2 = new SqlParameter("@P2", SqlDbType.Decimal,9);
			SqlParameter paramQ3 = new SqlParameter("@Q3", SqlDbType.Int,4);
			SqlParameter paramP3 = new SqlParameter("@P3", SqlDbType.Decimal,9);
			SqlParameter paramCashRebate = new SqlParameter("@CashRebate", SqlDbType.Decimal,9);
			SqlParameter paramPoint = new SqlParameter("@Point", SqlDbType.Int,4);
			SqlParameter paramClearanceSale = new SqlParameter("@ClearanceSale", SqlDbType.Int,4);
			SqlParameter paramLastOrderPrice = new SqlParameter("@LastOrderPrice", SqlDbType.Decimal,9);
            SqlParameter paramLastMarketLowestPrice = new SqlParameter("@LastMarketLowestPrice", SqlDbType.Decimal, 9);
            SqlParameter paramLimitedQty = new SqlParameter("@LimitedQty", SqlDbType.Int, 4);

			if ( oParam.ProductSysNo != AppConst.IntNull)
				paramProductSysNo.Value = oParam.ProductSysNo;
			else
				paramProductSysNo.Value = System.DBNull.Value;
			if ( oParam.BasicPrice != AppConst.DecimalNull)
				paramBasicPrice.Value = oParam.BasicPrice;
			else
				paramBasicPrice.Value = System.DBNull.Value;
			if ( oParam.CurrentPrice != AppConst.DecimalNull)
				paramCurrentPrice.Value = oParam.CurrentPrice;
			else
				paramCurrentPrice.Value = System.DBNull.Value;
			if ( oParam.UnitCost != AppConst.DecimalNull)
				paramUnitCost.Value = oParam.UnitCost;
			else
				paramUnitCost.Value = System.DBNull.Value;
			if ( oParam.Discount != AppConst.DecimalNull)
				paramDiscount.Value = oParam.Discount;
			else
				paramDiscount.Value = System.DBNull.Value;
			if ( oParam.PointType != AppConst.IntNull)
				paramPointType.Value = oParam.PointType;
			else
				paramPointType.Value = System.DBNull.Value;
			if ( oParam.IsWholeSale != AppConst.IntNull)
				paramIsWholeSale.Value = oParam.IsWholeSale;
			else
				paramIsWholeSale.Value = 0;
			if ( oParam.Q1 != AppConst.IntNull)
				paramQ1.Value = oParam.Q1;
			else
				paramQ1.Value = System.DBNull.Value;
			if ( oParam.P1 != AppConst.DecimalNull)
				paramP1.Value = oParam.P1;
			else
				paramP1.Value = System.DBNull.Value;
			if ( oParam.Q2 != AppConst.IntNull)
				paramQ2.Value = oParam.Q2;
			else
				paramQ2.Value = System.DBNull.Value;
			if ( oParam.P2 != AppConst.DecimalNull)
				paramP2.Value = oParam.P2;
			else
				paramP2.Value = System.DBNull.Value;
			if ( oParam.Q3 != AppConst.IntNull)
				paramQ3.Value = oParam.Q3;
			else
				paramQ3.Value = System.DBNull.Value;
			if ( oParam.P3 != AppConst.DecimalNull)
				paramP3.Value = oParam.P3;
			else
				paramP3.Value = System.DBNull.Value;
			if ( oParam.CashRebate != AppConst.DecimalNull)
				paramCashRebate.Value = oParam.CashRebate;
			else
				paramCashRebate.Value = System.DBNull.Value;
			if ( oParam.Point != AppConst.IntNull)
				paramPoint.Value = oParam.Point;
			else
				paramPoint.Value = System.DBNull.Value;
			if ( oParam.ClearanceSale != AppConst.IntNull)
				paramClearanceSale.Value = oParam.ClearanceSale;
			else
				paramClearanceSale.Value = System.DBNull.Value;

			if ( oParam.LastOrderPrice != AppConst.IntNull)
				paramLastOrderPrice.Value = oParam.LastOrderPrice;
			else
				paramLastOrderPrice.Value = 0;

            if (oParam.LastMarketLowestPrice != AppConst.IntNull)
                paramLastMarketLowestPrice.Value = oParam.LastMarketLowestPrice;
            else
                paramLastMarketLowestPrice.Value = 0;

            if (oParam.LimitedQty != AppConst.IntNull)
                paramLimitedQty.Value = oParam.LimitedQty;
            else
                paramLimitedQty.Value = 999;
			cmd.Parameters.Add(paramProductSysNo);
			cmd.Parameters.Add(paramBasicPrice);
			cmd.Parameters.Add(paramCurrentPrice);
			cmd.Parameters.Add(paramUnitCost);
			cmd.Parameters.Add(paramDiscount);
			cmd.Parameters.Add(paramPointType);
			cmd.Parameters.Add(paramIsWholeSale);
			cmd.Parameters.Add(paramQ1);
			cmd.Parameters.Add(paramP1);
			cmd.Parameters.Add(paramQ2);
			cmd.Parameters.Add(paramP2);
			cmd.Parameters.Add(paramQ3);
			cmd.Parameters.Add(paramP3);
			cmd.Parameters.Add(paramCashRebate);
			cmd.Parameters.Add(paramPoint);
			cmd.Parameters.Add(paramClearanceSale);
			cmd.Parameters.Add(paramLastOrderPrice);
            cmd.Parameters.Add(paramLastMarketLowestPrice);
            cmd.Parameters.Add(paramLimitedQty);

			return SqlHelper.ExecuteNonQuery(cmd);
		}
		public int Update(ProductPriceInfo oParam)
		{
			string sql = @"UPDATE product_price SET 
                            BasicPrice=@BasicPrice, CurrentPrice=@CurrentPrice, 
                            Discount=@Discount, PointType=@PointType, 
                            IsWholeSale=@IsWholeSale, Q1=@Q1, 
                            P1=@P1, Q2=@Q2, 
                            P2=@P2, Q3=@Q3, 
                            P3=@P3, CashRebate=@CashRebate, Point=@Point ,
                            ClearanceSale=@ClearanceSale, LastOrderPrice=@LastOrderPrice, LastMarketLowestPrice=@LastMarketLowestPrice,
                            LimitedQty=@LimitedQty 
                            WHERE SysNo=@SysNo";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramBasicPrice = new SqlParameter("@BasicPrice", SqlDbType.Decimal,9);
			SqlParameter paramCurrentPrice = new SqlParameter("@CurrentPrice", SqlDbType.Decimal,9);
			SqlParameter paramDiscount = new SqlParameter("@Discount", SqlDbType.Decimal,9);
			SqlParameter paramPointType = new SqlParameter("@PointType", SqlDbType.Int,4);
			SqlParameter paramIsWholeSale = new SqlParameter("@IsWholeSale", SqlDbType.Int,4);
			SqlParameter paramQ1 = new SqlParameter("@Q1", SqlDbType.Int,4);
			SqlParameter paramP1 = new SqlParameter("@P1", SqlDbType.Decimal,9);
			SqlParameter paramQ2 = new SqlParameter("@Q2", SqlDbType.Int,4);
			SqlParameter paramP2 = new SqlParameter("@P2", SqlDbType.Decimal,9);
			SqlParameter paramQ3 = new SqlParameter("@Q3", SqlDbType.Int,4);
			SqlParameter paramP3 = new SqlParameter("@P3", SqlDbType.Decimal,9);
			SqlParameter paramCashRebate = new SqlParameter("@CashRebate", SqlDbType.Decimal,9);
			SqlParameter paramPoint = new SqlParameter("@Point", SqlDbType.Int,4);
			SqlParameter paramClearanceSale = new SqlParameter("@ClearanceSale", SqlDbType.Int,4);
			SqlParameter paramLastOrderPrice = new SqlParameter("@LastOrderPrice", SqlDbType.Decimal,9);
            SqlParameter paramLastMarketLowestPrice = new SqlParameter("@LastMarketLowestPrice", SqlDbType.Decimal, 9);
            SqlParameter paramLimitedQty = new SqlParameter("@LimitedQty", SqlDbType.Int, 4);

			if ( oParam.SysNo != AppConst.IntNull)
				paramSysNo.Value = oParam.SysNo;
			else
				paramSysNo.Value = System.DBNull.Value;
			if ( oParam.BasicPrice != AppConst.DecimalNull)
				paramBasicPrice.Value = oParam.BasicPrice;
			else
				paramBasicPrice.Value = System.DBNull.Value;
			if ( oParam.CurrentPrice != AppConst.DecimalNull)
				paramCurrentPrice.Value = oParam.CurrentPrice;
			else
				paramCurrentPrice.Value = System.DBNull.Value;
			if ( oParam.Discount != AppConst.DecimalNull)
				paramDiscount.Value = oParam.Discount;
			else
				paramDiscount.Value = System.DBNull.Value;
			if ( oParam.PointType != AppConst.IntNull)
				paramPointType.Value = oParam.PointType;
			else
				paramPointType.Value = System.DBNull.Value;
			if ( oParam.IsWholeSale != AppConst.IntNull)
				paramIsWholeSale.Value = oParam.IsWholeSale;
			else
				paramIsWholeSale.Value = System.DBNull.Value;
			if ( oParam.Q1 != AppConst.IntNull)
				paramQ1.Value = oParam.Q1;
			else
				paramQ1.Value = System.DBNull.Value;
			if ( oParam.P1 != AppConst.DecimalNull)
				paramP1.Value = oParam.P1;
			else
				paramP1.Value = System.DBNull.Value;
			if ( oParam.Q2 != AppConst.IntNull)
				paramQ2.Value = oParam.Q2;
			else
				paramQ2.Value = System.DBNull.Value;
			if ( oParam.P2 != AppConst.DecimalNull)
				paramP2.Value = oParam.P2;
			else
				paramP2.Value = System.DBNull.Value;
			if ( oParam.Q3 != AppConst.IntNull)
				paramQ3.Value = oParam.Q3;
			else
				paramQ3.Value = System.DBNull.Value;
			if ( oParam.P3 != AppConst.DecimalNull)
				paramP3.Value = oParam.P3;
			else
				paramP3.Value = System.DBNull.Value;
			if ( oParam.CashRebate != AppConst.DecimalNull)
				paramCashRebate.Value = oParam.CashRebate;
			else
				paramCashRebate.Value = System.DBNull.Value;
			if ( oParam.PointType != AppConst.IntNull)
				paramPoint.Value = oParam.Point;
			else
				paramPoint.Value = System.DBNull.Value;
			if ( oParam.ClearanceSale != AppConst.IntNull)
				paramClearanceSale.Value = oParam.ClearanceSale;
			else
				paramClearanceSale.Value = System.DBNull.Value;

			if ( oParam.LastOrderPrice != AppConst.IntNull)
				paramLastOrderPrice.Value = oParam.LastOrderPrice;
			else
				paramLastOrderPrice.Value = 0;

            if (oParam.LastMarketLowestPrice != AppConst.IntNull)
                paramLastMarketLowestPrice.Value = oParam.LastMarketLowestPrice;
            else
                paramLastMarketLowestPrice.Value = 0;

            if (oParam.LimitedQty != AppConst.IntNull)
                paramLimitedQty.Value = oParam.LimitedQty;
            else
                paramLimitedQty.Value = 999;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramBasicPrice);
			cmd.Parameters.Add(paramCurrentPrice);
			cmd.Parameters.Add(paramDiscount);
			cmd.Parameters.Add(paramPointType);
			cmd.Parameters.Add(paramIsWholeSale);
			cmd.Parameters.Add(paramQ1);
			cmd.Parameters.Add(paramP1);
			cmd.Parameters.Add(paramQ2);
			cmd.Parameters.Add(paramP2);
			cmd.Parameters.Add(paramQ3);
			cmd.Parameters.Add(paramP3);
			cmd.Parameters.Add(paramCashRebate);
			cmd.Parameters.Add(paramPoint);
			cmd.Parameters.Add(paramClearanceSale);
			cmd.Parameters.Add(paramLastOrderPrice);
            cmd.Parameters.Add(paramLastMarketLowestPrice);
            cmd.Parameters.Add(paramLimitedQty);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public int UpdateCost(int productSysNo, decimal cost)
		{
			string sql = @"UPDATE product_price SET UnitCost= " + Decimal.Round(cost,2).ToString() + " WHERE ProductSysNo=" + productSysNo;
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);
		}
		public int UpdateLastOrderPrice(int productSysNo, decimal lastOrderPrice)
		{
			string sql = @"update product_price set lastorderprice=" + Decimal.Round(lastOrderPrice,2).ToString() + " where productsysno=" + productSysNo;
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);
		}

        public int UpdateLastMarketLowestPrice(int productSysNo, decimal lastMarketLowestPrice)
        {
            string sql = @"update product_price set lastMarketLowestPrice=" + Decimal.Round(lastMarketLowestPrice, 2).ToString() + " where productsysno=" + productSysNo;
            SqlCommand cmd = new SqlCommand(sql);
            return SqlHelper.ExecuteNonQuery(cmd);
        }
	}
}
