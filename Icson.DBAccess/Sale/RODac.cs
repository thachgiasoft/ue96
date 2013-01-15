using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using Icson.Objects.Sale;
using Icson.Utils;


namespace Icson.DBAccess.Sale
{
	/// <summary>
	/// Summary description for RODac.
	/// </summary>
	public class RODac
	{
		public RODac()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        public int InsertMaster(ROInfo oParam)
        {
            string sql = @"INSERT INTO RO_Master
                           (
                           SysNo, ROID, RMASysNo, StockSysNo, Status,
                           OriginCashAmt, OriginPointAmt, RedeemAmt, CashAmt, 
                           PointAmt, CreateUserSysNo, AuditUserSysNo, 
                           AuditTime, ReturnUserSysNo, ReturnTime, ReceiveName, ReceiveAddress, 
                           ReceivePhone, Note
                           )
                           VALUES (
                           @SysNo, @ROID, @RMASysNo, @StockSysNo, @Status,
                           @OriginCashAmt, @OriginPointAmt, @RedeemAmt, @CashAmt, 
                           @PointAmt, @CreateUserSysNo, @AuditUserSysNo, 
                           @AuditTime, @ReturnUserSysNo, @ReturnTime, @ReceiveName, @ReceiveAddress, 
                           @ReceivePhone, @Note
                           )";

            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramROID = new SqlParameter("@ROID", SqlDbType.NVarChar, 20);
            SqlParameter paramRMASysNo = new SqlParameter("@RMASysNo", SqlDbType.Int, 4);
            SqlParameter paramStockSysNo = new SqlParameter("@StockSysNo", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramOriginCashAmt = new SqlParameter("@OriginCashAmt", SqlDbType.Decimal, 9);
            SqlParameter paramOriginPointAmt = new SqlParameter("@OriginPointAmt", SqlDbType.Int, 4);
            SqlParameter paramRedeemAmt = new SqlParameter("@RedeemAmt", SqlDbType.Decimal, 9);
            SqlParameter paramCashAmt = new SqlParameter("@CashAmt", SqlDbType.Decimal, 9);
            SqlParameter paramPointAmt = new SqlParameter("@PointAmt", SqlDbType.Int, 4);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramAuditUserSysNo = new SqlParameter("@AuditUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramAuditTime = new SqlParameter("@AuditTime", SqlDbType.DateTime);
            SqlParameter paramReturnUserSysNo = new SqlParameter("@ReturnUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramReturnTime = new SqlParameter("@ReturnTime", SqlDbType.DateTime);
            SqlParameter paramReceiveName = new SqlParameter("@ReceiveName", SqlDbType.NVarChar, 20);
            SqlParameter paramReceiveAddress = new SqlParameter("@ReceiveAddress", SqlDbType.NVarChar, 200);
            SqlParameter paramReceivePhone = new SqlParameter("@ReceivePhone", SqlDbType.NVarChar, 20);
            SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.NVarChar, 500);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.ROID != AppConst.StringNull)
                paramROID.Value = oParam.ROID;
            else
                paramROID.Value = System.DBNull.Value;
            if (oParam.RMASysNo != AppConst.IntNull)
                paramRMASysNo.Value = oParam.RMASysNo;
            else
                paramRMASysNo.Value = System.DBNull.Value;
            if (oParam.StockSysNo != AppConst.IntNull)
                paramStockSysNo.Value = oParam.StockSysNo;
            else
                paramStockSysNo.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;
            if (oParam.OriginCashAmt != AppConst.DecimalNull)
                paramOriginCashAmt.Value = oParam.OriginCashAmt;
            else
                paramOriginCashAmt.Value = System.DBNull.Value;
            if (oParam.OriginPointAmt != AppConst.IntNull)
                paramOriginPointAmt.Value = oParam.OriginPointAmt;
            else
                paramOriginPointAmt.Value = System.DBNull.Value;
            if (oParam.RedeemAmt != AppConst.DecimalNull)
                paramRedeemAmt.Value = oParam.RedeemAmt;
            else
                paramRedeemAmt.Value = System.DBNull.Value;
            if (oParam.CashAmt != AppConst.DecimalNull)
                paramCashAmt.Value = oParam.CashAmt;
            else
                paramCashAmt.Value = System.DBNull.Value;
            if (oParam.PointAmt != AppConst.IntNull)
                paramPointAmt.Value = oParam.PointAmt;
            else
                paramPointAmt.Value = System.DBNull.Value;
            if (oParam.CreateUserSysNo != AppConst.IntNull)
                paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
            else
                paramCreateUserSysNo.Value = System.DBNull.Value;
            if (oParam.AuditUserSysNo != AppConst.IntNull)
                paramAuditUserSysNo.Value = oParam.AuditUserSysNo;
            else
                paramAuditUserSysNo.Value = System.DBNull.Value;
            if (oParam.AuditTime != AppConst.DateTimeNull)
                paramAuditTime.Value = oParam.AuditTime;
            else
                paramAuditTime.Value = System.DBNull.Value;
            if (oParam.ReturnUserSysNo != AppConst.IntNull)
                paramReturnUserSysNo.Value = oParam.ReturnUserSysNo;
            else
                paramReturnUserSysNo.Value = System.DBNull.Value;
            if (oParam.ReturnTime != AppConst.DateTimeNull)
                paramReturnTime.Value = oParam.ReturnTime;
            else
                paramReturnTime.Value = System.DBNull.Value;
            if (oParam.ReceiveName != AppConst.StringNull)
                paramReceiveName.Value = oParam.ReceiveName;
            else
                paramReceiveName.Value = System.DBNull.Value;
            if (oParam.ReceiveAddress != AppConst.StringNull)
                paramReceiveAddress.Value = oParam.ReceiveAddress;
            else
                paramReceiveAddress.Value = System.DBNull.Value;
            if (oParam.ReceivePhone != AppConst.StringNull)
                paramReceivePhone.Value = oParam.ReceivePhone;
            else
                paramReceivePhone.Value = System.DBNull.Value;
            if (oParam.Note != AppConst.StringNull)
                paramNote.Value = oParam.Note;
            else
                paramNote.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramROID);
            cmd.Parameters.Add(paramRMASysNo);
            cmd.Parameters.Add(paramStockSysNo);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramOriginCashAmt);
            cmd.Parameters.Add(paramOriginPointAmt);
            cmd.Parameters.Add(paramRedeemAmt);
            cmd.Parameters.Add(paramCashAmt);
            cmd.Parameters.Add(paramPointAmt);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramAuditUserSysNo);
            cmd.Parameters.Add(paramAuditTime);
            cmd.Parameters.Add(paramReturnUserSysNo);
            cmd.Parameters.Add(paramReturnTime);
            cmd.Parameters.Add(paramReceiveName);
            cmd.Parameters.Add(paramReceiveAddress);
            cmd.Parameters.Add(paramReceivePhone);
            cmd.Parameters.Add(paramNote);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int UpdateMaster(Hashtable paramHash)
        {
            StringBuilder sb = new StringBuilder(200);
            sb.Append("UPDATE RO_Master SET ");

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
                        sb.Append(key).Append(" = cast(").Append(Util.ToSqlString(((DateTime)item).ToString(AppConst.DateFormatLong))).Append(" as DateTime)");
                    }
                }
            }

            sb.Append(" WHERE SysNo=").Append(paramHash["SysNo"]);

            return SqlHelper.ExecuteNonQuery(sb.ToString());
        }

        public int InsertItem(ROItemInfo oParam)
        {
            string sql = @"INSERT INTO RO_Item
                           (
                           ROSysNo, ProductSysNo, ReturnPriceType, ReturnType, 
                           ReturnSysNo, Quantity, Price, Cost, 
                           Weight, Point, RefundCash, RefundPoint
                           )
                           VALUES (
                           @ROSysNo, @ProductSysNo,@ReturnPriceType, @ReturnType, 
                           @ReturnSysNo, @Quantity, @Price, @Cost, 
                           @Weight, @Point, @RefundCash, @RefundPoint
                           )";

            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramROSysNo = new SqlParameter("@ROSysNo", SqlDbType.Int, 4);
            SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int, 4);
            SqlParameter paramReturnPriceType = new SqlParameter("@ReturnPriceType", SqlDbType.Int, 4);
            SqlParameter paramReturnType = new SqlParameter("@ReturnType", SqlDbType.Int, 4);
            SqlParameter paramReturnSysNo = new SqlParameter("@ReturnSysNo", SqlDbType.Int, 4);
            SqlParameter paramQuantity = new SqlParameter("@Quantity", SqlDbType.Int, 4);
            SqlParameter paramPrice = new SqlParameter("@Price", SqlDbType.Decimal, 9);
            SqlParameter paramCost = new SqlParameter("@Cost", SqlDbType.Decimal, 9);
            SqlParameter paramWeight = new SqlParameter("@Weight", SqlDbType.Int, 4);
            SqlParameter paramPoint = new SqlParameter("@Point", SqlDbType.Int, 4);
            SqlParameter paramRefundCash = new SqlParameter("@RefundCash", SqlDbType.Decimal, 9);
            SqlParameter paramRefundPoint = new SqlParameter("@RefundPoint", SqlDbType.Int, 4);

            if (oParam.ROSysNo != AppConst.IntNull)
                paramROSysNo.Value = oParam.ROSysNo;
            else
                paramROSysNo.Value = System.DBNull.Value;
            if (oParam.ProductSysNo != AppConst.IntNull)
                paramProductSysNo.Value = oParam.ProductSysNo;
            else
                paramProductSysNo.Value = System.DBNull.Value;
            if (oParam.ReturnPriceType != AppConst.IntNull)
                paramReturnPriceType.Value = oParam.ReturnPriceType;
            else
                paramReturnPriceType.Value = System.DBNull.Value;
            if (oParam.ReturnType != AppConst.IntNull)
                paramReturnType.Value = oParam.ReturnType;
            else
                paramReturnType.Value = System.DBNull.Value;
            if (oParam.ReturnSysNo != AppConst.IntNull)
                paramReturnSysNo.Value = oParam.ReturnSysNo;
            else
                paramReturnSysNo.Value = System.DBNull.Value;
            if (oParam.Quantity != AppConst.IntNull)
                paramQuantity.Value = oParam.Quantity;
            else
                paramQuantity.Value = System.DBNull.Value;
            if (oParam.Price != AppConst.DecimalNull)
                paramPrice.Value = oParam.Price;
            else
                paramPrice.Value = System.DBNull.Value;
            if (oParam.Cost != AppConst.DecimalNull)
                paramCost.Value = oParam.Cost;
            else
                paramCost.Value = System.DBNull.Value;
            if (oParam.Weight != AppConst.IntNull)
                paramWeight.Value = oParam.Weight;
            else
                paramWeight.Value = System.DBNull.Value;
            if (oParam.Point != AppConst.IntNull)
                paramPoint.Value = oParam.Point;
            else
                paramPoint.Value = System.DBNull.Value;
            if (oParam.RefundCash != AppConst.DecimalNull)
                paramRefundCash.Value = oParam.RefundCash;
            else
                paramRefundCash.Value = System.DBNull.Value;
            if (oParam.RefundPoint != AppConst.IntNull)
                paramRefundPoint.Value = oParam.RefundPoint;
            else
                paramRefundPoint.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramROSysNo);
            cmd.Parameters.Add(paramProductSysNo);
            cmd.Parameters.Add(paramReturnPriceType);
            cmd.Parameters.Add(paramReturnType);
            cmd.Parameters.Add(paramReturnSysNo);
            cmd.Parameters.Add(paramQuantity);
            cmd.Parameters.Add(paramPrice);
            cmd.Parameters.Add(paramCost);
            cmd.Parameters.Add(paramWeight);
            cmd.Parameters.Add(paramPoint);
            cmd.Parameters.Add(paramRefundCash);
            cmd.Parameters.Add(paramRefundPoint);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int UpdateItem(ROItemInfo oParam)
        {
            string sql = @"UPDATE RO_Item SET 
                           ROSysNo=@ROSysNo, 
                           ProductSysNo=@ProductSysNo,ReturnPriceType=@ReturnPriceType,ReturnType=@ReturnType, 
                           ReturnSysNo=@ReturnSysNo, Quantity=@Quantity, 
                           Price=@Price, Cost=@Cost, 
                           Weight=@Weight, Point=@Point, 
                           RefundCash=@RefundCash, RefundPoint=@RefundPoint
                           WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramROSysNo = new SqlParameter("@ROSysNo", SqlDbType.Int, 4);
            SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int, 4);
            SqlParameter paramReturnPriceType = new SqlParameter("@ReturnPriceType", SqlDbType.Int, 4);
            SqlParameter paramReturnType = new SqlParameter("@ReturnType", SqlDbType.Int, 4);
            SqlParameter paramReturnSysNo = new SqlParameter("@ReturnSysNo", SqlDbType.Int, 4);
            SqlParameter paramQuantity = new SqlParameter("@Quantity", SqlDbType.Int, 4);
            SqlParameter paramPrice = new SqlParameter("@Price", SqlDbType.Decimal, 9);
            SqlParameter paramCost = new SqlParameter("@Cost", SqlDbType.Decimal, 9);
            SqlParameter paramWeight = new SqlParameter("@Weight", SqlDbType.Int, 4);
            SqlParameter paramPoint = new SqlParameter("@Point", SqlDbType.Int, 4);
            SqlParameter paramRefundCash = new SqlParameter("@RefundCash", SqlDbType.Decimal, 9);
            SqlParameter paramRefundPoint = new SqlParameter("@RefundPoint", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.ROSysNo != AppConst.IntNull)
                paramROSysNo.Value = oParam.ROSysNo;
            else
                paramROSysNo.Value = System.DBNull.Value;
            if (oParam.ReturnPriceType != AppConst.IntNull)
                paramReturnPriceType.Value = oParam.ReturnPriceType;
            else
                paramReturnPriceType.Value = System.DBNull.Value;
            if (oParam.ProductSysNo != AppConst.IntNull)
                paramProductSysNo.Value = oParam.ProductSysNo;
            else
                paramProductSysNo.Value = System.DBNull.Value;
            if (oParam.ReturnType != AppConst.IntNull)
                paramReturnType.Value = oParam.ReturnType;
            else
                paramReturnType.Value = System.DBNull.Value;
            if (oParam.ReturnSysNo != AppConst.IntNull)
                paramReturnSysNo.Value = oParam.ReturnSysNo;
            else
                paramReturnSysNo.Value = System.DBNull.Value;
            if (oParam.Quantity != AppConst.IntNull)
                paramQuantity.Value = oParam.Quantity;
            else
                paramQuantity.Value = System.DBNull.Value;
            if (oParam.Price != AppConst.DecimalNull)
                paramPrice.Value = oParam.Price;
            else
                paramPrice.Value = System.DBNull.Value;
            if (oParam.Cost != AppConst.DecimalNull)
                paramCost.Value = oParam.Cost;
            else
                paramCost.Value = System.DBNull.Value;
            if (oParam.Weight != AppConst.IntNull)
                paramWeight.Value = oParam.Weight;
            else
                paramWeight.Value = System.DBNull.Value;
            if (oParam.Point != AppConst.IntNull)
                paramPoint.Value = oParam.Point;
            else
                paramPoint.Value = System.DBNull.Value;
            if (oParam.RefundCash != AppConst.DecimalNull)
                paramRefundCash.Value = oParam.RefundCash;
            else
                paramRefundCash.Value = System.DBNull.Value;
            if (oParam.RefundPoint != AppConst.IntNull)
                paramRefundPoint.Value = oParam.RefundPoint;
            else
                paramRefundPoint.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramROSysNo);
            cmd.Parameters.Add(paramProductSysNo);
            cmd.Parameters.Add(paramReturnPriceType);
            cmd.Parameters.Add(paramReturnType);
            cmd.Parameters.Add(paramReturnSysNo);
            cmd.Parameters.Add(paramQuantity);
            cmd.Parameters.Add(paramPrice);
            cmd.Parameters.Add(paramCost);
            cmd.Parameters.Add(paramWeight);
            cmd.Parameters.Add(paramPoint);
            cmd.Parameters.Add(paramRefundCash);
            cmd.Parameters.Add(paramRefundPoint);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
    }
}
