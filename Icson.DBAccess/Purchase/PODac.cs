using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Text;

using Icson.Utils;
using Icson.Objects.Purchase;

namespace Icson.DBAccess.Purchase
{
	/// <summary>
	/// Summary description for PODac.
	/// </summary>
	public class PODac
	{
		
		public PODac()
		{
		}

        public int InsertMaster(POInfo oParam)
        {
            string sql = @"INSERT INTO PO_Master
                                    (
                                    SysNo, POID, VendorSysNo, StockSysNo, 
                                    ShipTypeSysNo, PayTypeSysNo, CurrencySysNo, ExchangeRate, 
                                    TotalAmt, CreateTime, CreateUserSysNo, AuditTime, 
                                    AuditUserSysNo, ReceiveTime, ReceiveUserSysNo, InTime, 
                                    InUserSysNo, IsApportion, ApportionTime, ApportionUserSysNo, 
                                    PayDate, Memo, Note, Status, 
                                    POType, POInvoiceType, ManagerAuditMemo, ManagerAuditStatus, 
                                    POInvoiceDunDesc, InvoiceExpectReceiveDate, POShipTypeSysNo, DeliveryDate, 
                                    CustomerSysNo
                                    )
                                    VALUES (
                                    @SysNo, @POID, @VendorSysNo, @StockSysNo, 
                                    @ShipTypeSysNo, @PayTypeSysNo, @CurrencySysNo, @ExchangeRate, 
                                    @TotalAmt, @CreateTime, @CreateUserSysNo, @AuditTime, 
                                    @AuditUserSysNo, @ReceiveTime, @ReceiveUserSysNo, @InTime, 
                                    @InUserSysNo, @IsApportion, @ApportionTime, @ApportionUserSysNo, 
                                    @PayDate, @Memo, @Note, @Status, 
                                    @POType, @POInvoiceType, @ManagerAuditMemo, @ManagerAuditStatus, 
                                    @POInvoiceDunDesc, @InvoiceExpectReceiveDate, @POShipTypeSysNo, @DeliveryDate, 
                                    @CustomerSysNo
                                    )";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
            SqlParameter paramPOID = new SqlParameter("@POID", SqlDbType.NVarChar,20);
            SqlParameter paramVendorSysNo = new SqlParameter("@VendorSysNo", SqlDbType.Int,4);
            SqlParameter paramStockSysNo = new SqlParameter("@StockSysNo", SqlDbType.Int,4);
            SqlParameter paramShipTypeSysNo = new SqlParameter("@ShipTypeSysNo", SqlDbType.Int,4);
            SqlParameter paramPayTypeSysNo = new SqlParameter("@PayTypeSysNo", SqlDbType.Int,4);
            SqlParameter paramCurrencySysNo = new SqlParameter("@CurrencySysNo", SqlDbType.Int,4);
            SqlParameter paramExchangeRate = new SqlParameter("@ExchangeRate", SqlDbType.Decimal,9);
            SqlParameter paramTotalAmt = new SqlParameter("@TotalAmt", SqlDbType.Decimal,9);
            SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int,4);
            SqlParameter paramAuditTime = new SqlParameter("@AuditTime", SqlDbType.DateTime);
            SqlParameter paramAuditUserSysNo = new SqlParameter("@AuditUserSysNo", SqlDbType.Int,4);
            SqlParameter paramReceiveTime = new SqlParameter("@ReceiveTime", SqlDbType.DateTime);
            SqlParameter paramReceiveUserSysNo = new SqlParameter("@ReceiveUserSysNo", SqlDbType.Int,4);
            SqlParameter paramInTime = new SqlParameter("@InTime", SqlDbType.DateTime);
            SqlParameter paramInUserSysNo = new SqlParameter("@InUserSysNo", SqlDbType.Int,4);
            SqlParameter paramIsApportion = new SqlParameter("@IsApportion", SqlDbType.Int,4);
            SqlParameter paramApportionTime = new SqlParameter("@ApportionTime", SqlDbType.DateTime);
            SqlParameter paramApportionUserSysNo = new SqlParameter("@ApportionUserSysNo", SqlDbType.Int,4);
            SqlParameter paramPayDate = new SqlParameter("@PayDate", SqlDbType.DateTime);
            SqlParameter paramMemo = new SqlParameter("@Memo", SqlDbType.NVarChar,500);
            SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.NVarChar,500);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);
            SqlParameter paramPOType = new SqlParameter("@POType", SqlDbType.Int,4);
            SqlParameter paramPOInvoiceType = new SqlParameter("@POInvoiceType", SqlDbType.Int,4);
            SqlParameter paramManagerAuditMemo = new SqlParameter("@ManagerAuditMemo", SqlDbType.Text,0);
            SqlParameter paramManagerAuditStatus = new SqlParameter("@ManagerAuditStatus", SqlDbType.Int,4);
            SqlParameter paramPOInvoiceDunDesc = new SqlParameter("@POInvoiceDunDesc", SqlDbType.Text,0);
            SqlParameter paramInvoiceExpectReceiveDate = new SqlParameter("@InvoiceExpectReceiveDate", SqlDbType.DateTime);
            SqlParameter paramPOShipTypeSysNo = new SqlParameter("@POShipTypeSysNo", SqlDbType.Int,4);
            SqlParameter paramDeliveryDate = new SqlParameter("@DeliveryDate", SqlDbType.DateTime);
            SqlParameter paramCustomerSysNo = new SqlParameter("@CustomerSysNo", SqlDbType.Int,4);

            if ( oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if ( oParam.POID != AppConst.StringNull)
                paramPOID.Value = oParam.POID;
            else
                paramPOID.Value = System.DBNull.Value;
            if ( oParam.VendorSysNo != AppConst.IntNull)
                paramVendorSysNo.Value = oParam.VendorSysNo;
            else
                paramVendorSysNo.Value = System.DBNull.Value;
            if ( oParam.StockSysNo != AppConst.IntNull)
                paramStockSysNo.Value = oParam.StockSysNo;
            else
                paramStockSysNo.Value = System.DBNull.Value;
            if ( oParam.ShipTypeSysNo != AppConst.IntNull)
                paramShipTypeSysNo.Value = oParam.ShipTypeSysNo;
            else
                paramShipTypeSysNo.Value = System.DBNull.Value;
            if ( oParam.PayTypeSysNo != AppConst.IntNull)
                paramPayTypeSysNo.Value = oParam.PayTypeSysNo;
            else
                paramPayTypeSysNo.Value = System.DBNull.Value;
            if ( oParam.CurrencySysNo != AppConst.IntNull)
                paramCurrencySysNo.Value = oParam.CurrencySysNo;
            else
                paramCurrencySysNo.Value = System.DBNull.Value;
            if ( oParam.ExchangeRate != AppConst.DecimalNull)
                paramExchangeRate.Value = oParam.ExchangeRate;
            else
                paramExchangeRate.Value = System.DBNull.Value;
            if ( oParam.TotalAmt != AppConst.DecimalNull)
                paramTotalAmt.Value = oParam.TotalAmt;
            else
                paramTotalAmt.Value = System.DBNull.Value;
            if ( oParam.CreateTime != AppConst.DateTimeNull)
                paramCreateTime.Value = oParam.CreateTime;
            else
                paramCreateTime.Value = System.DBNull.Value;
            if ( oParam.CreateUserSysNo != AppConst.IntNull)
                paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
            else
                paramCreateUserSysNo.Value = System.DBNull.Value;
            if ( oParam.AuditTime != AppConst.DateTimeNull)
                paramAuditTime.Value = oParam.AuditTime;
            else
                paramAuditTime.Value = System.DBNull.Value;
            if ( oParam.AuditUserSysNo != AppConst.IntNull)
                paramAuditUserSysNo.Value = oParam.AuditUserSysNo;
            else
                paramAuditUserSysNo.Value = System.DBNull.Value;
            if ( oParam.ReceiveTime != AppConst.DateTimeNull)
                paramReceiveTime.Value = oParam.ReceiveTime;
            else
                paramReceiveTime.Value = System.DBNull.Value;
            if ( oParam.ReceiveUserSysNo != AppConst.IntNull)
                paramReceiveUserSysNo.Value = oParam.ReceiveUserSysNo;
            else
                paramReceiveUserSysNo.Value = System.DBNull.Value;
            if ( oParam.InTime != AppConst.DateTimeNull)
                paramInTime.Value = oParam.InTime;
            else
                paramInTime.Value = System.DBNull.Value;
            if ( oParam.InUserSysNo != AppConst.IntNull)
                paramInUserSysNo.Value = oParam.InUserSysNo;
            else
                paramInUserSysNo.Value = System.DBNull.Value;
            if ( oParam.IsApportion != AppConst.IntNull)
                paramIsApportion.Value = oParam.IsApportion;
            else
                paramIsApportion.Value = System.DBNull.Value;
            if ( oParam.ApportionTime != AppConst.DateTimeNull)
                paramApportionTime.Value = oParam.ApportionTime;
            else
                paramApportionTime.Value = System.DBNull.Value;
            if ( oParam.ApportionUserSysNo != AppConst.IntNull)
                paramApportionUserSysNo.Value = oParam.ApportionUserSysNo;
            else
                paramApportionUserSysNo.Value = System.DBNull.Value;
            if ( oParam.PayDate != AppConst.DateTimeNull)
                paramPayDate.Value = oParam.PayDate;
            else
                paramPayDate.Value = System.DBNull.Value;
            if ( oParam.Memo != AppConst.StringNull)
                paramMemo.Value = oParam.Memo;
            else
                paramMemo.Value = System.DBNull.Value;
            if ( oParam.Note != AppConst.StringNull)
                paramNote.Value = oParam.Note;
            else
                paramNote.Value = System.DBNull.Value;
            if ( oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;
            if ( oParam.POType != AppConst.IntNull)
                paramPOType.Value = oParam.POType;
            else
                paramPOType.Value = System.DBNull.Value;
            if ( oParam.POInvoiceType != AppConst.IntNull)
                paramPOInvoiceType.Value = oParam.POInvoiceType;
            else
                paramPOInvoiceType.Value = System.DBNull.Value;
            if ( oParam.ManagerAuditMemo != AppConst.StringNull)
                paramManagerAuditMemo.Value = oParam.ManagerAuditMemo;
            else
                paramManagerAuditMemo.Value = System.DBNull.Value;
            if ( oParam.ManagerAuditStatus != AppConst.IntNull)
                paramManagerAuditStatus.Value = oParam.ManagerAuditStatus;
            else
                paramManagerAuditStatus.Value = System.DBNull.Value;
            if ( oParam.POInvoiceDunDesc != AppConst.StringNull)
                paramPOInvoiceDunDesc.Value = oParam.POInvoiceDunDesc;
            else
                paramPOInvoiceDunDesc.Value = System.DBNull.Value;
            if ( oParam.InvoiceExpectReceiveDate != AppConst.DateTimeNull)
                paramInvoiceExpectReceiveDate.Value = oParam.InvoiceExpectReceiveDate;
            else
                paramInvoiceExpectReceiveDate.Value = System.DBNull.Value;
            if ( oParam.POShipTypeSysNo != AppConst.IntNull)
                paramPOShipTypeSysNo.Value = oParam.POShipTypeSysNo;
            else
                paramPOShipTypeSysNo.Value = System.DBNull.Value;
            if ( oParam.DeliveryDate != AppConst.DateTimeNull)
                paramDeliveryDate.Value = oParam.DeliveryDate;
            else
                paramDeliveryDate.Value = System.DBNull.Value;
            if ( oParam.CustomerSysNo != AppConst.IntNull)
                paramCustomerSysNo.Value = oParam.CustomerSysNo;
            else
                paramCustomerSysNo.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramPOID);
            cmd.Parameters.Add(paramVendorSysNo);
            cmd.Parameters.Add(paramStockSysNo);
            cmd.Parameters.Add(paramShipTypeSysNo);
            cmd.Parameters.Add(paramPayTypeSysNo);
            cmd.Parameters.Add(paramCurrencySysNo);
            cmd.Parameters.Add(paramExchangeRate);
            cmd.Parameters.Add(paramTotalAmt);
            cmd.Parameters.Add(paramCreateTime);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramAuditTime);
            cmd.Parameters.Add(paramAuditUserSysNo);
            cmd.Parameters.Add(paramReceiveTime);
            cmd.Parameters.Add(paramReceiveUserSysNo);
            cmd.Parameters.Add(paramInTime);
            cmd.Parameters.Add(paramInUserSysNo);
            cmd.Parameters.Add(paramIsApportion);
            cmd.Parameters.Add(paramApportionTime);
            cmd.Parameters.Add(paramApportionUserSysNo);
            cmd.Parameters.Add(paramPayDate);
            cmd.Parameters.Add(paramMemo);
            cmd.Parameters.Add(paramNote);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramPOType);
            cmd.Parameters.Add(paramPOInvoiceType);
            cmd.Parameters.Add(paramManagerAuditMemo);
            cmd.Parameters.Add(paramManagerAuditStatus);
            cmd.Parameters.Add(paramPOInvoiceDunDesc);
            cmd.Parameters.Add(paramInvoiceExpectReceiveDate);
            cmd.Parameters.Add(paramPOShipTypeSysNo);
            cmd.Parameters.Add(paramDeliveryDate);
            cmd.Parameters.Add(paramCustomerSysNo);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

		public int UpdateMaster(Hashtable paramHash)
		{
			StringBuilder sb = new StringBuilder(200);
			sb.Append("UPDATE PO_Master SET ");

			if ( paramHash != null && paramHash.Count != 0 )
			{
				int index = 0;
				foreach(string key in paramHash.Keys)
				{
					object item = paramHash[key];
					if ( key.ToLower()=="sysno" )
						continue;

					if ( index != 0 )
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
						sb.Append(key).Append(" = cast(").Append( Util.ToSqlString(((DateTime)item).ToString(AppConst.DateFormatLong))).Append(" as DateTime)");
					}
				}
			}

			sb.Append(" WHERE SysNo=").Append(paramHash["SysNo"]);

			return SqlHelper.ExecuteNonQuery(sb.ToString());
		}

		public int InsertItem(POItemInfo oParam)
		{
            string sql = @"INSERT INTO PO_Item
                            (
                            POSysNo, ProductSysNo, Quantity, Weight, 
                            OrderPrice, ApportionAddOn, UnitCost, OrderQty
                            )
                            VALUES (
                            @POSysNo, @ProductSysNo, @Quantity, @Weight, 
                            @OrderPrice, @ApportionAddOn, @UnitCost, @OrderQty
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramPOSysNo = new SqlParameter("@POSysNo", SqlDbType.Int, 4);
            SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int, 4);
            SqlParameter paramQuantity = new SqlParameter("@Quantity", SqlDbType.Int, 4);
            SqlParameter paramWeight = new SqlParameter("@Weight", SqlDbType.Int, 4);
            SqlParameter paramOrderPrice = new SqlParameter("@OrderPrice", SqlDbType.Decimal, 9);
            SqlParameter paramApportionAddOn = new SqlParameter("@ApportionAddOn", SqlDbType.Decimal, 9);
            SqlParameter paramUnitCost = new SqlParameter("@UnitCost", SqlDbType.Decimal, 9);
            SqlParameter paramOrderQty = new SqlParameter("@OrderQty", SqlDbType.Int, 4);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.POSysNo != AppConst.IntNull)
                paramPOSysNo.Value = oParam.POSysNo;
            else
                paramPOSysNo.Value = System.DBNull.Value;
            if (oParam.ProductSysNo != AppConst.IntNull)
                paramProductSysNo.Value = oParam.ProductSysNo;
            else
                paramProductSysNo.Value = System.DBNull.Value;
            if (oParam.Quantity != AppConst.IntNull)
                paramQuantity.Value = oParam.Quantity;
            else
                paramQuantity.Value = System.DBNull.Value;
            if (oParam.Weight != AppConst.IntNull)
                paramWeight.Value = oParam.Weight;
            else
                paramWeight.Value = System.DBNull.Value;
            if (oParam.OrderPrice != AppConst.DecimalNull)
                paramOrderPrice.Value = oParam.OrderPrice;
            else
                paramOrderPrice.Value = System.DBNull.Value;
            if (oParam.ApportionAddOn != AppConst.DecimalNull)
                paramApportionAddOn.Value = oParam.ApportionAddOn;
            else
                paramApportionAddOn.Value = System.DBNull.Value;
            if (oParam.UnitCost != AppConst.DecimalNull)
                paramUnitCost.Value = oParam.UnitCost;
            else
                paramUnitCost.Value = System.DBNull.Value;
            if (oParam.OrderQty != AppConst.IntNull)
                paramOrderQty.Value = oParam.OrderQty;
            else
                paramOrderQty.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramPOSysNo);
            cmd.Parameters.Add(paramProductSysNo);
            cmd.Parameters.Add(paramQuantity);
            cmd.Parameters.Add(paramWeight);
            cmd.Parameters.Add(paramOrderPrice);
            cmd.Parameters.Add(paramApportionAddOn);
            cmd.Parameters.Add(paramUnitCost);
            cmd.Parameters.Add(paramOrderQty);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
		}
		public int UpdateItem(POItemInfo oParam)
		{
            string sql = @"UPDATE PO_Item SET 
                            POSysNo=@POSysNo, ProductSysNo=@ProductSysNo, 
                            Quantity=@Quantity, Weight=@Weight, 
                            OrderPrice=@OrderPrice, ApportionAddOn=@ApportionAddOn, 
                            UnitCost=@UnitCost, OrderQty=@OrderQty
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramPOSysNo = new SqlParameter("@POSysNo", SqlDbType.Int, 4);
            SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int, 4);
            SqlParameter paramQuantity = new SqlParameter("@Quantity", SqlDbType.Int, 4);
            SqlParameter paramWeight = new SqlParameter("@Weight", SqlDbType.Int, 4);
            SqlParameter paramOrderPrice = new SqlParameter("@OrderPrice", SqlDbType.Decimal, 9);
            SqlParameter paramApportionAddOn = new SqlParameter("@ApportionAddOn", SqlDbType.Decimal, 9);
            SqlParameter paramUnitCost = new SqlParameter("@UnitCost", SqlDbType.Decimal, 9);
            SqlParameter paramOrderQty = new SqlParameter("@OrderQty", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.POSysNo != AppConst.IntNull)
                paramPOSysNo.Value = oParam.POSysNo;
            else
                paramPOSysNo.Value = System.DBNull.Value;
            if (oParam.ProductSysNo != AppConst.IntNull)
                paramProductSysNo.Value = oParam.ProductSysNo;
            else
                paramProductSysNo.Value = System.DBNull.Value;
            if (oParam.Quantity != AppConst.IntNull)
                paramQuantity.Value = oParam.Quantity;
            else
                paramQuantity.Value = System.DBNull.Value;
            if (oParam.Weight != AppConst.IntNull)
                paramWeight.Value = oParam.Weight;
            else
                paramWeight.Value = System.DBNull.Value;
            if (oParam.OrderPrice != AppConst.DecimalNull)
                paramOrderPrice.Value = oParam.OrderPrice;
            else
                paramOrderPrice.Value = System.DBNull.Value;
            if (oParam.ApportionAddOn != AppConst.DecimalNull)
                paramApportionAddOn.Value = oParam.ApportionAddOn;
            else
                paramApportionAddOn.Value = System.DBNull.Value;
            if (oParam.UnitCost != AppConst.DecimalNull)
                paramUnitCost.Value = oParam.UnitCost;
            else
                paramUnitCost.Value = System.DBNull.Value;
            if (oParam.OrderQty != AppConst.IntNull)
                paramOrderQty.Value = oParam.OrderQty;
            else
                paramOrderQty.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramPOSysNo);
            cmd.Parameters.Add(paramProductSysNo);
            cmd.Parameters.Add(paramQuantity);
            cmd.Parameters.Add(paramWeight);
            cmd.Parameters.Add(paramOrderPrice);
            cmd.Parameters.Add(paramApportionAddOn);
            cmd.Parameters.Add(paramUnitCost);
            cmd.Parameters.Add(paramOrderQty);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
		public int DeleteItem(int poSysNo, int productSysNo)
		{
			string sql = " delete from po_item where posysno = " + poSysNo + " and productsysno = " + productSysNo;
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);

		}

        //批量删除采购项
        public int DeleteItems(int poSysNo, string productSysNos)
        {
            string sql = " delete from po_item where posysno = " + poSysNo + " and productsysno in (" + productSysNos+ ")";
            SqlCommand cmd = new SqlCommand(sql);
            return SqlHelper.ExecuteNonQuery(cmd);

        }
	}
}
