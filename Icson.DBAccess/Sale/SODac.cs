using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using Icson.Objects.Sale;
using Icson.Utils;

namespace Icson.DBAccess.Sale
{
    /// <summary>
    /// Summary description for SODac.
    /// </summary>
    public class SODac
    {
        public SODac()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public int InsertMaster(SOInfo oParam)
        {
            string sql = @"INSERT INTO SO_Master
                            (
                            SysNo, SOID, Status, CustomerSysNo, 
                            StockSysNo, OrderDate, DeliveryDate, SalesManSysNo, 
                            IsWholeSale, IsPremium, PremiumAmt, ShipTypeSysNo, 
                            ShipPrice, FreeShipFeePay, PayTypeSysNo, PayPrice, SOAmt, 
                            DiscountAmt,CouponType, CouponCode, CouponAmt, 
                            PointAmt, CashPay, PointPay, ReceiveAreaSysNo,
                            ReceiveContact, ReceiveName, ReceivePhone, ReceiveCellPhone, ReceiveAddress, 
                            ReceiveZip, AllocatedManSysNo, FreightUserSysNo, UpdateUserSysNo, 
                            UpdateTime, AuditUserSysNo, AuditTime, ManagerAuditUserSysNo, 
                            ManagerAuditTime, OutUserSysNo, OutTime,CheckQtyUserSysNo,CheckQtyTime, Memo, 
                            Note, InvoiceNote, IsVAT, IsPrintPackageCover, DeliveryMemo,VATEMSFee,
                            ExpectDeliveryDate,ExpectDeliveryTimeSpan,AuditDeliveryDate,AuditDeliveryTimeSpan,SentDate,SentTimeSpan,
                            SignByOther,HasServiceProduct,CSUserSysNo,HasExpectQty,IsMergeSO 
                            )
                            VALUES 
                            (
                            @SysNo, @SOID, @Status, @CustomerSysNo, 
                            @StockSysNo, @OrderDate, @DeliveryDate, @SalesManSysNo, 
                            @IsWholeSale, @IsPremium, @PremiumAmt, @ShipTypeSysNo, 
                            @ShipPrice, @FreeShipFeePay, @PayTypeSysNo, @PayPrice, @SOAmt, 
                            @DiscountAmt,@CouponType, @CouponCode, @CouponAmt, 
                            @PointAmt, @CashPay, @PointPay, @ReceiveAreaSysNo,
                            @ReceiveContact, @ReceiveName, @ReceivePhone, @ReceiveCellPhone, @ReceiveAddress, 
                            @ReceiveZip, @AllocatedManSysNo, @FreightUserSysNo, @UpdateUserSysNo, 
                            @UpdateTime, @AuditUserSysNo, @AuditTime, @ManagerAuditUserSysNo, 
                            @ManagerAuditTime, @OutUserSysNo, @OutTime, @CheckQtyUserSysNo,@CheckQtyTime,@Memo, 
                            @Note, @InvoiceNote, @IsVAT, @IsPrintPackageCover, @DeliveryMemo, @VATEMSFee,
                            @ExpectDeliveryDate,@ExpectDeliveryTimeSpan,@AuditDeliveryDate,@AuditDeliveryTimeSpan,@SentDate,@SentTimeSpan,
                            @SignByOther,@HasServiceProduct,@CSUserSysNo,@HasExpectQty,@IsMergeSO
                            )";

            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramSOID = new SqlParameter("@SOID", SqlDbType.NVarChar, 20);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramCustomerSysNo = new SqlParameter("@CustomerSysNo", SqlDbType.Int, 4);
            SqlParameter paramStockSysNo = new SqlParameter("@StockSysNo", SqlDbType.Int, 4);
            SqlParameter paramOrderDate = new SqlParameter("@OrderDate", SqlDbType.DateTime);
            SqlParameter paramDeliveryDate = new SqlParameter("@DeliveryDate", SqlDbType.DateTime);
            SqlParameter paramSalesManSysNo = new SqlParameter("@SalesManSysNo", SqlDbType.Int, 4);
            SqlParameter paramIsWholeSale = new SqlParameter("@IsWholeSale", SqlDbType.Int, 4);
            SqlParameter paramIsPremium = new SqlParameter("@IsPremium", SqlDbType.Int, 4);
            SqlParameter paramPremiumAmt = new SqlParameter("@PremiumAmt", SqlDbType.Decimal, 9);
            SqlParameter paramShipTypeSysNo = new SqlParameter("@ShipTypeSysNo", SqlDbType.Int, 4);
            SqlParameter paramShipPrice = new SqlParameter("@ShipPrice", SqlDbType.Decimal, 9);
            SqlParameter paramFreeShipFeePay = new SqlParameter("@FreeShipFeePay", SqlDbType.Decimal, 9);

            SqlParameter paramPayTypeSysNo = new SqlParameter("@PayTypeSysNo", SqlDbType.Int, 4);
            SqlParameter paramPayPrice = new SqlParameter("@PayPrice", SqlDbType.Decimal, 9);
            SqlParameter paramSOAmt = new SqlParameter("@SOAmt", SqlDbType.Decimal, 9);
            SqlParameter paramDiscountAmt = new SqlParameter("@DiscountAmt", SqlDbType.Decimal, 9);

            //hawkins 2010-4-20
            SqlParameter paramCouponType = new SqlParameter("@CouponType", SqlDbType.Int, 4);
            SqlParameter paramCouponCode = new SqlParameter("@CouponCode", SqlDbType.NVarChar, 50);
            SqlParameter paramCouponAmt = new SqlParameter("@CouponAmt", SqlDbType.Decimal, 9);
            //==================

            SqlParameter paramPointAmt = new SqlParameter("@PointAmt", SqlDbType.Int, 4);
            SqlParameter paramCashPay = new SqlParameter("@CashPay", SqlDbType.Decimal, 9);
            SqlParameter paramPointPay = new SqlParameter("@PointPay", SqlDbType.Int, 4);
            SqlParameter paramReceiveAreaSysNo = new SqlParameter("@ReceiveAreaSysNo", SqlDbType.Int, 4);
            SqlParameter paramReceiveContact = new SqlParameter("@ReceiveContact", SqlDbType.NVarChar, 20);
            SqlParameter paramReceiveName = new SqlParameter("@ReceiveName", SqlDbType.NVarChar, 20);
            SqlParameter paramReceivePhone = new SqlParameter("@ReceivePhone", SqlDbType.NVarChar, 100);
            SqlParameter paramReceiveCellPhone = new SqlParameter("@ReceiveCellPhone", SqlDbType.NVarChar, 100);
            SqlParameter paramReceiveAddress = new SqlParameter("@ReceiveAddress", SqlDbType.NVarChar, 200);
            SqlParameter paramReceiveZip = new SqlParameter("@ReceiveZip", SqlDbType.NVarChar, 20);
            SqlParameter paramAllocatedManSysNo = new SqlParameter("@AllocatedManSysNo", SqlDbType.Int, 4);
            SqlParameter paramFreightUserSysNo = new SqlParameter("@FreightUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramUpdateUserSysNo = new SqlParameter("@UpdateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramUpdateTime = new SqlParameter("@UpdateTime", SqlDbType.DateTime);
            SqlParameter paramAuditUserSysNo = new SqlParameter("@AuditUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramAuditTime = new SqlParameter("@AuditTime", SqlDbType.DateTime);
            SqlParameter paramManagerAuditUserSysNo = new SqlParameter("@ManagerAuditUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramManagerAuditTime = new SqlParameter("@ManagerAuditTime", SqlDbType.DateTime);
            SqlParameter paramOutUserSysNo = new SqlParameter("@OutUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramOutTime = new SqlParameter("@OutTime", SqlDbType.DateTime);
            SqlParameter paramCheckQtyUserSysNo = new SqlParameter("@CheckQtyUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCheckQtyTime = new SqlParameter("@CheckQtyTime", SqlDbType.DateTime);
            SqlParameter paramMemo = new SqlParameter("@Memo", SqlDbType.NVarChar, 1000);
            SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.NVarChar, 1000);
            SqlParameter paramInvoiceNote = new SqlParameter("@InvoiceNote", SqlDbType.NVarChar, 500);
            SqlParameter paramIsVAT = new SqlParameter("@IsVAT", SqlDbType.Int, 4);
            SqlParameter paramIsPrintPackageCover = new SqlParameter("@IsPrintPackageCover", SqlDbType.Int, 4);
            SqlParameter paramDeliveryMemo = new SqlParameter("@DeliveryMemo", SqlDbType.NVarChar, 200);
            SqlParameter paramVATEMSFee = new SqlParameter("@VATEMSFee", SqlDbType.Decimal, 9);

            SqlParameter paramExpectDeliveryDate = new SqlParameter("@ExpectDeliveryDate", SqlDbType.DateTime);
            SqlParameter paramExpectDeliveryTimeSpan = new SqlParameter("@ExpectDeliveryTimeSpan", SqlDbType.Int, 4);
            SqlParameter paramAuditDeliveryDate = new SqlParameter("@AuditDeliveryDate", SqlDbType.DateTime);
            SqlParameter paramAuditDeliveryTimeSpan = new SqlParameter("@AuditDeliveryTimeSpan", SqlDbType.Int, 4);
            SqlParameter paramSentDate = new SqlParameter("@SentDate", SqlDbType.DateTime);
            SqlParameter paramSentTimeSpan = new SqlParameter("@SentTimeSpan", SqlDbType.Int, 4);

            SqlParameter paramSignByOther = new SqlParameter("@SignByOther", SqlDbType.Int, 4);
            SqlParameter paramHasServiceProduct = new SqlParameter("@HasServiceProduct", SqlDbType.Int, 4);

            SqlParameter paramCSUserSysNo = new SqlParameter("@CSUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramHasExpectQty = new SqlParameter("@HasExpectQty", SqlDbType.Int, 4);
            SqlParameter paramIsMergeSO = new SqlParameter("@IsMergeSO", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.SOID != AppConst.StringNull)
                paramSOID.Value = oParam.SOID;
            else
                paramSOID.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;
            if (oParam.CustomerSysNo != AppConst.IntNull)
                paramCustomerSysNo.Value = oParam.CustomerSysNo;
            else
                paramCustomerSysNo.Value = System.DBNull.Value;
            if (oParam.StockSysNo != AppConst.IntNull)
                paramStockSysNo.Value = oParam.StockSysNo;
            else
                paramStockSysNo.Value = System.DBNull.Value;
            if (oParam.OrderDate != AppConst.DateTimeNull)
                paramOrderDate.Value = oParam.OrderDate;
            else
                paramOrderDate.Value = System.DBNull.Value;
            if (oParam.DeliveryDate != AppConst.DateTimeNull)
                paramDeliveryDate.Value = oParam.DeliveryDate;
            else
                paramDeliveryDate.Value = System.DBNull.Value;
            if (oParam.SalesManSysNo != AppConst.IntNull)
                paramSalesManSysNo.Value = oParam.SalesManSysNo;
            else
                paramSalesManSysNo.Value = System.DBNull.Value;
            if (oParam.IsWholeSale != AppConst.IntNull)
                paramIsWholeSale.Value = oParam.IsWholeSale;
            else
                paramIsWholeSale.Value = System.DBNull.Value;
            if (oParam.IsPremium != AppConst.IntNull)
                paramIsPremium.Value = oParam.IsPremium;
            else
                paramIsPremium.Value = System.DBNull.Value;
            if (oParam.PremiumAmt != AppConst.DecimalNull)
                paramPremiumAmt.Value = oParam.PremiumAmt;
            else
                paramPremiumAmt.Value = System.DBNull.Value;
            if (oParam.ShipTypeSysNo != AppConst.IntNull)
                paramShipTypeSysNo.Value = oParam.ShipTypeSysNo;
            else
                paramShipTypeSysNo.Value = System.DBNull.Value;
            if (oParam.ShipPrice != AppConst.DecimalNull)
                paramShipPrice.Value = oParam.ShipPrice;
            else
                paramShipPrice.Value = System.DBNull.Value;
            if (oParam.FreeShipFeePay != AppConst.DecimalNull)
                paramFreeShipFeePay.Value = oParam.FreeShipFeePay;
            else
                paramFreeShipFeePay.Value = System.DBNull.Value;

            if (oParam.PayTypeSysNo != AppConst.IntNull)
                paramPayTypeSysNo.Value = oParam.PayTypeSysNo;
            else
                paramPayTypeSysNo.Value = System.DBNull.Value;
            if (oParam.PayPrice != AppConst.DecimalNull)
                paramPayPrice.Value = oParam.PayPrice;
            else
                paramPayPrice.Value = System.DBNull.Value;
            if (oParam.SOAmt != AppConst.DecimalNull)
                paramSOAmt.Value = oParam.SOAmt;
            else
                paramSOAmt.Value = System.DBNull.Value;
            if (oParam.DiscountAmt != AppConst.DecimalNull)
                paramDiscountAmt.Value = oParam.DiscountAmt;
            else
                paramDiscountAmt.Value = System.DBNull.Value;

            //hawkins 2010-4-20
            if (oParam.CouponType != AppConst.IntNull)
                paramCouponType.Value = oParam.CouponType;
            else
                paramCouponType.Value = System.DBNull.Value;
            if (oParam.CouponCode != AppConst.StringNull)
                paramCouponCode.Value = oParam.CouponCode;
            else
                paramCouponCode.Value = System.DBNull.Value;
            if (oParam.CouponAmt != AppConst.DecimalNull)
                paramCouponAmt.Value = oParam.CouponAmt;
            else
                paramCouponAmt.Value = System.DBNull.Value;
            //==================

            if (oParam.PointAmt != AppConst.IntNull)
                paramPointAmt.Value = oParam.PointAmt;
            else
                paramPointAmt.Value = System.DBNull.Value;
            if (oParam.CashPay != AppConst.DecimalNull)
                paramCashPay.Value = oParam.CashPay;
            else
                paramCashPay.Value = System.DBNull.Value;
            if (oParam.PointPay != AppConst.IntNull)
                paramPointPay.Value = oParam.PointPay;
            else
                paramPointPay.Value = System.DBNull.Value;
            if (oParam.ReceiveAreaSysNo != AppConst.IntNull)
                paramReceiveAreaSysNo.Value = oParam.ReceiveAreaSysNo;
            else
                paramReceiveAreaSysNo.Value = System.DBNull.Value;
            if (oParam.ReceiveContact != AppConst.StringNull)
                paramReceiveContact.Value = oParam.ReceiveContact;
            else
                paramReceiveContact.Value = System.DBNull.Value;
            if (oParam.ReceiveName != AppConst.StringNull)
                paramReceiveName.Value = oParam.ReceiveName;
            else
                paramReceiveName.Value = System.DBNull.Value;
            if (oParam.ReceivePhone != AppConst.StringNull)
                paramReceivePhone.Value = oParam.ReceivePhone;
            else
                paramReceivePhone.Value = System.DBNull.Value;
            if (oParam.ReceiveCellPhone != AppConst.StringNull)
                paramReceiveCellPhone.Value = oParam.ReceiveCellPhone;
            else
                paramReceiveCellPhone.Value = System.DBNull.Value;
            if (oParam.ReceiveAddress != AppConst.StringNull)
                paramReceiveAddress.Value = oParam.ReceiveAddress;
            else
                paramReceiveAddress.Value = System.DBNull.Value;
            if (oParam.ReceiveZip != AppConst.StringNull)
                paramReceiveZip.Value = oParam.ReceiveZip;
            else
                paramReceiveZip.Value = System.DBNull.Value;
            if (oParam.AllocatedManSysNo != AppConst.IntNull)
                paramAllocatedManSysNo.Value = oParam.AllocatedManSysNo;
            else
                paramAllocatedManSysNo.Value = System.DBNull.Value;
            if (oParam.FreightUserSysNo != AppConst.IntNull)
                paramFreightUserSysNo.Value = oParam.FreightUserSysNo;
            else
                paramFreightUserSysNo.Value = System.DBNull.Value;
            if (oParam.UpdateUserSysNo != AppConst.IntNull)
                paramUpdateUserSysNo.Value = oParam.UpdateUserSysNo;
            else
                paramUpdateUserSysNo.Value = System.DBNull.Value;
            if (oParam.UpdateTime != AppConst.DateTimeNull)
                paramUpdateTime.Value = oParam.UpdateTime;
            else
                paramUpdateTime.Value = System.DBNull.Value;
            if (oParam.AuditUserSysNo != AppConst.IntNull)
                paramAuditUserSysNo.Value = oParam.AuditUserSysNo;
            else
                paramAuditUserSysNo.Value = System.DBNull.Value;
            if (oParam.AuditTime != AppConst.DateTimeNull)
                paramAuditTime.Value = oParam.AuditTime;
            else
                paramAuditTime.Value = System.DBNull.Value;
            if (oParam.ManagerAuditUserSysNo != AppConst.IntNull)
                paramManagerAuditUserSysNo.Value = oParam.ManagerAuditUserSysNo;
            else
                paramManagerAuditUserSysNo.Value = System.DBNull.Value;
            if (oParam.ManagerAuditTime != AppConst.DateTimeNull)
                paramManagerAuditTime.Value = oParam.ManagerAuditTime;
            else
                paramManagerAuditTime.Value = System.DBNull.Value;
            if (oParam.OutUserSysNo != AppConst.IntNull)
                paramOutUserSysNo.Value = oParam.OutUserSysNo;
            else
                paramOutUserSysNo.Value = System.DBNull.Value;
            if (oParam.OutTime != AppConst.DateTimeNull)
                paramOutTime.Value = oParam.OutTime;
            else
                paramOutTime.Value = System.DBNull.Value;
            if (oParam.CheckQtyUserSysNo != AppConst.IntNull)
                paramCheckQtyUserSysNo.Value = oParam.CheckQtyUserSysNo;
            else
                paramCheckQtyUserSysNo.Value = System.DBNull.Value;
            if (oParam.CheckQtyTime != AppConst.DateTimeNull)
                paramCheckQtyTime.Value = oParam.CheckQtyTime;
            else
                paramCheckQtyTime.Value = System.DBNull.Value;
            if (oParam.Memo != AppConst.StringNull)
                paramMemo.Value = oParam.Memo;
            else
                paramMemo.Value = System.DBNull.Value;
            if (oParam.Note != AppConst.StringNull)
                paramNote.Value = oParam.Note;
            else
                paramNote.Value = System.DBNull.Value;
            if (oParam.InvoiceNote != AppConst.StringNull)
                paramInvoiceNote.Value = oParam.InvoiceNote;
            else
                paramInvoiceNote.Value = System.DBNull.Value;
            if (oParam.IsVAT != AppConst.IntNull)
                paramIsVAT.Value = oParam.IsVAT;
            else
                paramIsVAT.Value = System.DBNull.Value;
            if (oParam.IsPrintPackageCover != AppConst.IntNull)
                paramIsPrintPackageCover.Value = oParam.IsPrintPackageCover;
            else
                paramIsPrintPackageCover.Value = System.DBNull.Value;
            if (oParam.DeliveryMemo != AppConst.StringNull)
                paramDeliveryMemo.Value = oParam.DeliveryMemo;
            else
                paramDeliveryMemo.Value = System.DBNull.Value;
            if (oParam.VATEMSFee != AppConst.DecimalNull)
                paramVATEMSFee.Value = oParam.VATEMSFee;
            else
                paramVATEMSFee.Value = System.DBNull.Value;

            if (oParam.ExpectDeliveryDate != AppConst.DateTimeNull)
                paramExpectDeliveryDate.Value = oParam.ExpectDeliveryDate;
            else
                paramExpectDeliveryDate.Value = System.DBNull.Value;
            if (oParam.ExpectDeliveryTimeSpan != AppConst.IntNull)
                paramExpectDeliveryTimeSpan.Value = oParam.ExpectDeliveryTimeSpan;
            else
                paramExpectDeliveryTimeSpan.Value = System.DBNull.Value;
            if (oParam.AuditDeliveryDate != AppConst.DateTimeNull)
                paramAuditDeliveryDate.Value = oParam.AuditDeliveryDate;
            else
                paramAuditDeliveryDate.Value = System.DBNull.Value;
            if (oParam.AuditDeliveryTimeSpan != AppConst.IntNull)
                paramAuditDeliveryTimeSpan.Value = oParam.AuditDeliveryTimeSpan;
            else
                paramAuditDeliveryTimeSpan.Value = System.DBNull.Value;
            if (oParam.SentDate != AppConst.DateTimeNull)
                paramSentDate.Value = oParam.SentDate;
            else
                paramSentDate.Value = System.DBNull.Value;
            if (oParam.SentTimeSpan != AppConst.IntNull)
                paramSentTimeSpan.Value = oParam.SentTimeSpan;
            else
                paramSentTimeSpan.Value = System.DBNull.Value;
            if (oParam.SignByOther != AppConst.IntNull)
                paramSignByOther.Value = oParam.SignByOther;
            else
                paramSignByOther.Value = System.DBNull.Value;
            if (oParam.HasServiceProduct != AppConst.IntNull)
                paramHasServiceProduct.Value = oParam.HasServiceProduct;
            else
                paramHasServiceProduct.Value = System.DBNull.Value;

            if (oParam.CSUserSysNo != AppConst.IntNull)
                paramCSUserSysNo.Value = oParam.CSUserSysNo;
            else
                paramCSUserSysNo.Value = System.DBNull.Value;

            if (oParam.HasExpectQty != AppConst.IntNull)
                paramHasExpectQty.Value = oParam.HasExpectQty;
            else
                paramHasExpectQty.Value = System.DBNull.Value;
            if (oParam.IsMergeSO != AppConst.IntNull)
                paramIsMergeSO.Value = oParam.IsMergeSO;
            else
                paramIsMergeSO.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramSOID);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramCustomerSysNo);
            cmd.Parameters.Add(paramStockSysNo);
            cmd.Parameters.Add(paramOrderDate);
            cmd.Parameters.Add(paramDeliveryDate);
            cmd.Parameters.Add(paramSalesManSysNo);
            cmd.Parameters.Add(paramIsWholeSale);
            cmd.Parameters.Add(paramIsPremium);
            cmd.Parameters.Add(paramPremiumAmt);
            cmd.Parameters.Add(paramShipTypeSysNo);
            cmd.Parameters.Add(paramShipPrice);
            cmd.Parameters.Add(paramFreeShipFeePay);
            cmd.Parameters.Add(paramPayTypeSysNo);
            cmd.Parameters.Add(paramPayPrice);
            cmd.Parameters.Add(paramSOAmt);
            cmd.Parameters.Add(paramDiscountAmt);

            //hawkins 2010-4-20
            cmd.Parameters.Add(paramCouponType);
            cmd.Parameters.Add(paramCouponCode);
            cmd.Parameters.Add(paramCouponAmt);
            //==================

            cmd.Parameters.Add(paramPointAmt);
            cmd.Parameters.Add(paramCashPay);
            cmd.Parameters.Add(paramPointPay);
            cmd.Parameters.Add(paramReceiveAreaSysNo);
            cmd.Parameters.Add(paramReceiveContact);
            cmd.Parameters.Add(paramReceiveName);
            cmd.Parameters.Add(paramReceivePhone);
            cmd.Parameters.Add(paramReceiveCellPhone);
            cmd.Parameters.Add(paramReceiveAddress);
            cmd.Parameters.Add(paramReceiveZip);
            cmd.Parameters.Add(paramAllocatedManSysNo);
            cmd.Parameters.Add(paramFreightUserSysNo);
            cmd.Parameters.Add(paramUpdateUserSysNo);
            cmd.Parameters.Add(paramUpdateTime);
            cmd.Parameters.Add(paramAuditUserSysNo);
            cmd.Parameters.Add(paramAuditTime);
            cmd.Parameters.Add(paramManagerAuditUserSysNo);
            cmd.Parameters.Add(paramManagerAuditTime);
            cmd.Parameters.Add(paramOutUserSysNo);
            cmd.Parameters.Add(paramOutTime);
            cmd.Parameters.Add(paramCheckQtyUserSysNo);
            cmd.Parameters.Add(paramCheckQtyTime);
            cmd.Parameters.Add(paramMemo);
            cmd.Parameters.Add(paramNote);
            cmd.Parameters.Add(paramInvoiceNote);
            cmd.Parameters.Add(paramIsVAT);
            cmd.Parameters.Add(paramIsPrintPackageCover);
            cmd.Parameters.Add(paramDeliveryMemo);
            cmd.Parameters.Add(paramVATEMSFee);

            cmd.Parameters.Add(paramExpectDeliveryDate);
            cmd.Parameters.Add(paramExpectDeliveryTimeSpan);
            cmd.Parameters.Add(paramAuditDeliveryDate);
            cmd.Parameters.Add(paramAuditDeliveryTimeSpan);
            cmd.Parameters.Add(paramSentDate);
            cmd.Parameters.Add(paramSentTimeSpan);

            cmd.Parameters.Add(paramSignByOther);
            cmd.Parameters.Add(paramHasServiceProduct);

            cmd.Parameters.Add(paramCSUserSysNo);
            cmd.Parameters.Add(paramHasExpectQty);
            cmd.Parameters.Add(paramIsMergeSO);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int InsertItem(SOItemInfo oParam)
        {
            string sql = @"INSERT INTO SO_Item
                            (
                            SOSysNo, ProductSysNo, Quantity, Weight, OrderPrice,
                            Price, Cost, Point, PointType, 
                            DiscountAmt, Warranty, ProductType, GiftSysNo, 
                            BaseProductType,ExpectQty
                            )
                            VALUES (
                            @SOSysNo, @ProductSysNo, @Quantity, @Weight, @OrderPrice,
                            @Price, @Cost, @Point, @PointType, 
                            @DiscountAmt, @Warranty, @ProductType, @GiftSysNo, 
                            @BaseProductType,@ExpectQty
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int, 4);
            SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int, 4);
            SqlParameter paramQuantity = new SqlParameter("@Quantity", SqlDbType.Int, 4);
            SqlParameter paramWeight = new SqlParameter("@Weight", SqlDbType.Int, 4);
            SqlParameter paramOrderPrice = new SqlParameter("@OrderPrice", SqlDbType.Decimal, 9);
            SqlParameter paramPrice = new SqlParameter("@Price", SqlDbType.Decimal, 9);
            SqlParameter paramCost = new SqlParameter("@Cost", SqlDbType.Decimal, 9);
            SqlParameter paramPoint = new SqlParameter("@Point", SqlDbType.Int, 4);
            SqlParameter paramPointType = new SqlParameter("@PointType", SqlDbType.Int, 4);
            SqlParameter paramDiscountAmt = new SqlParameter("@DiscountAmt", SqlDbType.Decimal, 9);
            SqlParameter paramWarranty = new SqlParameter("@Warranty", SqlDbType.NVarChar, 500);
            SqlParameter paramProductType = new SqlParameter("@ProductType", SqlDbType.Int, 4);
            SqlParameter paramGiftSysNo = new SqlParameter("@GiftSysNo", SqlDbType.Int, 4);
            SqlParameter paramBaseProductType = new SqlParameter("@BaseProductType", SqlDbType.Int, 4);
            SqlParameter paramExpectQty = new SqlParameter("@ExpectQty", SqlDbType.Int, 4);

            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.SOSysNo != AppConst.IntNull)
                paramSOSysNo.Value = oParam.SOSysNo;
            else
                paramSOSysNo.Value = System.DBNull.Value;
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

            if (oParam.Price != AppConst.DecimalNull)
                paramPrice.Value = oParam.Price;
            else
                paramPrice.Value = System.DBNull.Value;
            if (oParam.Cost != AppConst.DecimalNull)
                paramCost.Value = oParam.Cost;
            else
                paramCost.Value = System.DBNull.Value;
            if (oParam.Point != AppConst.IntNull)
                paramPoint.Value = oParam.Point;
            else
                paramPoint.Value = System.DBNull.Value;
            if (oParam.PointType != AppConst.IntNull)
                paramPointType.Value = oParam.PointType;
            else
                paramPointType.Value = System.DBNull.Value;
            if (oParam.DiscountAmt != AppConst.DecimalNull)
                paramDiscountAmt.Value = oParam.DiscountAmt;
            else
                paramDiscountAmt.Value = System.DBNull.Value;
            if (oParam.Warranty != AppConst.StringNull)
                paramWarranty.Value = oParam.Warranty;
            else
                paramWarranty.Value = System.DBNull.Value;
            if (oParam.ProductType != AppConst.IntNull)
                paramProductType.Value = oParam.ProductType;
            else
                paramProductType.Value = System.DBNull.Value;
            if (oParam.GiftSysNo != AppConst.IntNull)
                paramGiftSysNo.Value = oParam.GiftSysNo;
            else
                paramGiftSysNo.Value = System.DBNull.Value;
            if (oParam.BaseProductType != AppConst.IntNull)
                paramBaseProductType.Value = oParam.BaseProductType;
            else
                paramBaseProductType.Value = System.DBNull.Value;

            if (oParam.ExpectQty != AppConst.IntNull)
                paramExpectQty.Value = oParam.ExpectQty;
            else
                paramExpectQty.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramSOSysNo);
            cmd.Parameters.Add(paramProductSysNo);
            cmd.Parameters.Add(paramQuantity);
            cmd.Parameters.Add(paramWeight);
            cmd.Parameters.Add(paramOrderPrice);
            cmd.Parameters.Add(paramPrice);
            cmd.Parameters.Add(paramCost);
            cmd.Parameters.Add(paramPoint);
            cmd.Parameters.Add(paramPointType);
            cmd.Parameters.Add(paramDiscountAmt);
            cmd.Parameters.Add(paramWarranty);
            cmd.Parameters.Add(paramProductType);
            cmd.Parameters.Add(paramGiftSysNo);
            cmd.Parameters.Add(paramBaseProductType);
            cmd.Parameters.Add(paramExpectQty);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int UpdateSOMaster(SOInfo oParam)
        {
            string sql = @"UPDATE SO_Master SET 
                           Status=@Status,DeliveryDate=@DeliveryDate, SalesManSysNo=@SalesManSysNo, 
                           IsWholeSale=@IsWholeSale, IsPremium=@IsPremium, 
                           PremiumAmt=@PremiumAmt, ShipTypeSysNo=@ShipTypeSysNo, 
                           ShipPrice=@ShipPrice,FreeShipFeePay=@FreeShipFeePay, PayTypeSysNo=@PayTypeSysNo, 
                           PayPrice=@PayPrice, SOAmt=@SOAmt, 
                           DiscountAmt=@DiscountAmt, PointAmt=@PointAmt, 
                           CashPay=@CashPay, PointPay=@PointPay, ReceiveAreaSysNo=@ReceiveAreaSysNo,
                           ReceiveContact=@ReceiveContact, ReceiveName=@ReceiveName, 
                           ReceivePhone=@ReceivePhone, ReceiveCellPhone=@ReceiveCellPhone, ReceiveAddress=@ReceiveAddress, 
                           ReceiveZip=@ReceiveZip, AllocatedManSysNo=@AllocatedManSysNo, 
                           FreightUserSysNo=@FreightUserSysNo, UpdateUserSysNo=@UpdateUserSysNo, 
                           UpdateTime=@UpdateTime, AuditUserSysNo=@AuditUserSysNo, 
                           AuditTime=@AuditTime, ManagerAuditUserSysNo=@ManagerAuditUserSysNo, 
                           ManagerAuditTime=@ManagerAuditTime, OutUserSysNo=@OutUserSysNo, OutTime=@OutTime, 
                           CheckQtyUserSysNo=@CheckQtyUserSysNo,CheckQtyTime=@CheckQtyTime,
                           Memo=@Memo, Note=@Note, InvoiceNote=@InvoiceNote, IsVAT=@IsVAT, 
                           IsPrintPackageCover=@IsPrintPackageCover, DeliveryMemo=@DeliveryMemo, VATEMSFee=@VATEMSFee,
                           ExpectDeliveryDate=@ExpectDeliveryDate, ExpectDeliveryTimeSpan=@ExpectDeliveryTimeSpan, 
                           AuditDeliveryDate=@AuditDeliveryDate, AuditDeliveryTimeSpan=@AuditDeliveryTimeSpan, 
                           SentDate=@SentDate, SentTimeSpan=@SentTimeSpan, SignByOther=@SignByOther,HasServiceProduct=@HasServiceProduct,
                           CSUserSysNo=@CSUserSysNo,HasExpectQty=@HasExpectQty,IsMergeSO=@IsMergeSO  
                           WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramDeliveryDate = new SqlParameter("@DeliveryDate", SqlDbType.DateTime);
            SqlParameter paramSalesManSysNo = new SqlParameter("@SalesManSysNo", SqlDbType.Int, 4);
            SqlParameter paramIsWholeSale = new SqlParameter("@IsWholeSale", SqlDbType.Int, 4);
            SqlParameter paramIsPremium = new SqlParameter("@IsPremium", SqlDbType.Int, 4);
            SqlParameter paramPremiumAmt = new SqlParameter("@PremiumAmt", SqlDbType.Decimal, 9);
            SqlParameter paramShipTypeSysNo = new SqlParameter("@ShipTypeSysNo", SqlDbType.Int, 4);
            SqlParameter paramShipPrice = new SqlParameter("@ShipPrice", SqlDbType.Decimal, 9);
            SqlParameter paramFreeShipFeePay = new SqlParameter("@FreeShipFeePay", SqlDbType.Decimal, 9);
            SqlParameter paramPayTypeSysNo = new SqlParameter("@PayTypeSysNo", SqlDbType.Int, 4);
            SqlParameter paramPayPrice = new SqlParameter("@PayPrice", SqlDbType.Decimal, 9);
            SqlParameter paramSOAmt = new SqlParameter("@SOAmt", SqlDbType.Decimal, 9);
            SqlParameter paramDiscountAmt = new SqlParameter("@DiscountAmt", SqlDbType.Decimal, 9);
            SqlParameter paramPointAmt = new SqlParameter("@PointAmt", SqlDbType.Int, 4);
            SqlParameter paramCashPay = new SqlParameter("@CashPay", SqlDbType.Decimal, 9);
            SqlParameter paramPointPay = new SqlParameter("@PointPay", SqlDbType.Int, 4);
            SqlParameter paramReceiveAreaSysNo = new SqlParameter("@ReceiveAreaSysNo", SqlDbType.Int, 4);
            SqlParameter paramReceiveContact = new SqlParameter("@ReceiveContact", SqlDbType.NVarChar, 20);
            SqlParameter paramReceiveName = new SqlParameter("@ReceiveName", SqlDbType.NVarChar, 20);
            SqlParameter paramReceivePhone = new SqlParameter("@ReceivePhone", SqlDbType.NVarChar, 100);
            SqlParameter paramReceiveCellPhone = new SqlParameter("@ReceiveCellPhone", SqlDbType.NVarChar, 100);
            SqlParameter paramReceiveAddress = new SqlParameter("@ReceiveAddress", SqlDbType.NVarChar, 200);
            SqlParameter paramReceiveZip = new SqlParameter("@ReceiveZip", SqlDbType.NVarChar, 20);
            SqlParameter paramAllocatedManSysNo = new SqlParameter("@AllocatedManSysNo", SqlDbType.Int, 4);
            SqlParameter paramFreightUserSysNo = new SqlParameter("@FreightUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramUpdateUserSysNo = new SqlParameter("@UpdateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramUpdateTime = new SqlParameter("@UpdateTime", SqlDbType.DateTime);
            SqlParameter paramAuditUserSysNo = new SqlParameter("@AuditUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramAuditTime = new SqlParameter("@AuditTime", SqlDbType.DateTime);
            SqlParameter paramManagerAuditUserSysNo = new SqlParameter("@ManagerAuditUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramManagerAuditTime = new SqlParameter("@ManagerAuditTime", SqlDbType.DateTime);
            SqlParameter paramOutUserSysNo = new SqlParameter("@OutUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramOutTime = new SqlParameter("@OutTime", SqlDbType.DateTime);
            SqlParameter paramCheckQtyUserSysNo = new SqlParameter("@CheckQtyUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCheckQtyTime = new SqlParameter("@CheckQtyTime", SqlDbType.DateTime);
            SqlParameter paramMemo = new SqlParameter("@Memo", SqlDbType.NVarChar, 1000);
            SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.NVarChar, 1000);
            SqlParameter paramInvoiceNote = new SqlParameter("@InvoiceNote", SqlDbType.NVarChar, 500);
            SqlParameter paramIsVAT = new SqlParameter("@IsVAT", SqlDbType.Int, 4);
            SqlParameter paramIsPrintPackageCover = new SqlParameter("@IsPrintPackageCover", SqlDbType.Int, 4);
            SqlParameter paramDeliveryMemo = new SqlParameter("@DeliveryMemo", SqlDbType.NVarChar, 200);
            SqlParameter paramVATEMSFee = new SqlParameter("@VATEMSFee", SqlDbType.Decimal, 9);

            SqlParameter paramExpectDeliveryDate = new SqlParameter("@ExpectDeliveryDate", SqlDbType.DateTime);
            SqlParameter paramExpectDeliveryTimeSpan = new SqlParameter("@ExpectDeliveryTimeSpan", SqlDbType.Int, 4);
            SqlParameter paramAuditDeliveryDate = new SqlParameter("@AuditDeliveryDate", SqlDbType.DateTime);
            SqlParameter paramAuditDeliveryTimeSpan = new SqlParameter("@AuditDeliveryTimeSpan", SqlDbType.Int, 4);
            SqlParameter paramSentDate = new SqlParameter("@SentDate", SqlDbType.DateTime);
            SqlParameter paramSentTimeSpan = new SqlParameter("@SentTimeSpan", SqlDbType.Int, 4);

            SqlParameter paramSignByOther = new SqlParameter("@SignByOther", SqlDbType.Int, 4);
            SqlParameter paramHasServiceProduct = new SqlParameter("@HasServiceProduct", SqlDbType.Int, 4);

            SqlParameter paramCSUserSysNo = new SqlParameter("@CSUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramHasExpectQty = new SqlParameter("@HasExpectQty", SqlDbType.Int, 4);
            SqlParameter paramIsMergeSO = new SqlParameter("@IsMergeSO", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;
            if (oParam.DeliveryDate != AppConst.DateTimeNull)
                paramDeliveryDate.Value = oParam.DeliveryDate;
            else
                paramDeliveryDate.Value = System.DBNull.Value;
            if (oParam.SalesManSysNo != AppConst.IntNull)
                paramSalesManSysNo.Value = oParam.SalesManSysNo;
            else
                paramSalesManSysNo.Value = System.DBNull.Value;
            if (oParam.IsWholeSale != AppConst.IntNull)
                paramIsWholeSale.Value = oParam.IsWholeSale;
            else
                paramIsWholeSale.Value = System.DBNull.Value;
            if (oParam.IsPremium != AppConst.IntNull)
                paramIsPremium.Value = oParam.IsPremium;
            else
                paramIsPremium.Value = System.DBNull.Value;
            if (oParam.PremiumAmt != AppConst.DecimalNull)
                paramPremiumAmt.Value = oParam.PremiumAmt;
            else
                paramPremiumAmt.Value = System.DBNull.Value;
            if (oParam.ShipTypeSysNo != AppConst.IntNull)
                paramShipTypeSysNo.Value = oParam.ShipTypeSysNo;
            else
                paramShipTypeSysNo.Value = System.DBNull.Value;
            if (oParam.ShipPrice != AppConst.DecimalNull)
                paramShipPrice.Value = oParam.ShipPrice;
            else
                paramShipPrice.Value = System.DBNull.Value;
            if (oParam.FreeShipFeePay != AppConst.DecimalNull)
                paramFreeShipFeePay.Value = oParam.FreeShipFeePay;
            else
                paramFreeShipFeePay.Value = System.DBNull.Value;
            if (oParam.PayTypeSysNo != AppConst.IntNull)
                paramPayTypeSysNo.Value = oParam.PayTypeSysNo;
            else
                paramPayTypeSysNo.Value = System.DBNull.Value;
            if (oParam.PayPrice != AppConst.DecimalNull)
                paramPayPrice.Value = oParam.PayPrice;
            else
                paramPayPrice.Value = System.DBNull.Value;
            if (oParam.SOAmt != AppConst.DecimalNull)
                paramSOAmt.Value = oParam.SOAmt;
            else
                paramSOAmt.Value = System.DBNull.Value;
            if (oParam.DiscountAmt != AppConst.DecimalNull)
                paramDiscountAmt.Value = oParam.DiscountAmt;
            else
                paramDiscountAmt.Value = System.DBNull.Value;
            if (oParam.PointAmt != AppConst.IntNull)
                paramPointAmt.Value = oParam.PointAmt;
            else
                paramPointAmt.Value = System.DBNull.Value;
            if (oParam.CashPay != AppConst.DecimalNull)
                paramCashPay.Value = oParam.CashPay;
            else
                paramCashPay.Value = System.DBNull.Value;
            if (oParam.PointPay != AppConst.IntNull)
                paramPointPay.Value = oParam.PointPay;
            else
                paramPointPay.Value = System.DBNull.Value;
            if (oParam.ReceiveAreaSysNo != AppConst.IntNull)
                paramReceiveAreaSysNo.Value = oParam.ReceiveAreaSysNo;
            else
                paramReceiveAreaSysNo.Value = System.DBNull.Value;
            if (oParam.ReceiveContact != AppConst.StringNull)
                paramReceiveContact.Value = oParam.ReceiveContact;
            else
                paramReceiveContact.Value = System.DBNull.Value;
            if (oParam.ReceiveName != AppConst.StringNull)
                paramReceiveName.Value = oParam.ReceiveName;
            else
                paramReceiveName.Value = System.DBNull.Value;
            if (oParam.ReceivePhone != AppConst.StringNull)
                paramReceivePhone.Value = oParam.ReceivePhone;
            else
                paramReceivePhone.Value = System.DBNull.Value;
            if (oParam.ReceiveCellPhone != AppConst.StringNull)
                paramReceiveCellPhone.Value = oParam.ReceiveCellPhone;
            else
                paramReceiveCellPhone.Value = System.DBNull.Value;
            if (oParam.ReceiveAddress != AppConst.StringNull)
                paramReceiveAddress.Value = oParam.ReceiveAddress;
            else
                paramReceiveAddress.Value = System.DBNull.Value;
            if (oParam.ReceiveZip != AppConst.StringNull)
                paramReceiveZip.Value = oParam.ReceiveZip;
            else
                paramReceiveZip.Value = System.DBNull.Value;
            if (oParam.AllocatedManSysNo != AppConst.IntNull)
                paramAllocatedManSysNo.Value = oParam.AllocatedManSysNo;
            else
                paramAllocatedManSysNo.Value = System.DBNull.Value;
            if (oParam.FreightUserSysNo != AppConst.IntNull)
                paramFreightUserSysNo.Value = oParam.FreightUserSysNo;
            else
                paramFreightUserSysNo.Value = System.DBNull.Value;
            if (oParam.UpdateUserSysNo != AppConst.IntNull)
                paramUpdateUserSysNo.Value = oParam.UpdateUserSysNo;
            else
                paramUpdateUserSysNo.Value = System.DBNull.Value;
            if (oParam.UpdateTime != AppConst.DateTimeNull)
                paramUpdateTime.Value = oParam.UpdateTime;
            else
                paramUpdateTime.Value = System.DBNull.Value;
            if (oParam.AuditUserSysNo != AppConst.IntNull)
                paramAuditUserSysNo.Value = oParam.AuditUserSysNo;
            else
                paramAuditUserSysNo.Value = System.DBNull.Value;
            if (oParam.AuditTime != AppConst.DateTimeNull)
                paramAuditTime.Value = oParam.AuditTime;
            else
                paramAuditTime.Value = System.DBNull.Value;
            if (oParam.ManagerAuditUserSysNo != AppConst.IntNull)
                paramManagerAuditUserSysNo.Value = oParam.ManagerAuditUserSysNo;
            else
                paramManagerAuditUserSysNo.Value = System.DBNull.Value;
            if (oParam.ManagerAuditTime != AppConst.DateTimeNull)
                paramManagerAuditTime.Value = oParam.ManagerAuditTime;
            else
                paramManagerAuditTime.Value = System.DBNull.Value;
            if (oParam.OutUserSysNo != AppConst.IntNull)
                paramOutUserSysNo.Value = oParam.OutUserSysNo;
            else
                paramOutUserSysNo.Value = System.DBNull.Value;
            if (oParam.OutTime != AppConst.DateTimeNull)
                paramOutTime.Value = oParam.OutTime;
            else
                paramOutTime.Value = System.DBNull.Value;
            if (oParam.CheckQtyUserSysNo != AppConst.IntNull)
                paramCheckQtyUserSysNo.Value = oParam.CheckQtyUserSysNo;
            else
                paramCheckQtyUserSysNo.Value = System.DBNull.Value;
            if (oParam.CheckQtyTime != AppConst.DateTimeNull)
                paramCheckQtyTime.Value = oParam.CheckQtyTime;
            else
                paramCheckQtyTime.Value = System.DBNull.Value;
            if (oParam.Memo != AppConst.StringNull)
                paramMemo.Value = oParam.Memo;
            else
                paramMemo.Value = System.DBNull.Value;
            if (oParam.Note != AppConst.StringNull)
                paramNote.Value = oParam.Note;
            else
                paramNote.Value = System.DBNull.Value;
            if (oParam.InvoiceNote != AppConst.StringNull)
                paramInvoiceNote.Value = oParam.InvoiceNote;
            else
                paramInvoiceNote.Value = System.DBNull.Value;
            if (oParam.IsVAT != AppConst.IntNull)
                paramIsVAT.Value = oParam.IsVAT;
            else
                paramIsVAT.Value = System.DBNull.Value;
            if (oParam.IsPrintPackageCover != AppConst.IntNull)
                paramIsPrintPackageCover.Value = oParam.IsPrintPackageCover;
            else
                paramIsPrintPackageCover.Value = System.DBNull.Value;
            if (oParam.DeliveryMemo != AppConst.StringNull)
                paramDeliveryMemo.Value = oParam.DeliveryMemo;
            else
                paramDeliveryMemo.Value = System.DBNull.Value;

            if (oParam.VATEMSFee != AppConst.DecimalNull)
                paramVATEMSFee.Value = oParam.VATEMSFee;
            else
                paramVATEMSFee.Value = System.DBNull.Value;

            if (oParam.ExpectDeliveryDate != AppConst.DateTimeNull)
                paramExpectDeliveryDate.Value = oParam.ExpectDeliveryDate;
            else
                paramExpectDeliveryDate.Value = System.DBNull.Value;
            if (oParam.ExpectDeliveryTimeSpan != AppConst.IntNull)
                paramExpectDeliveryTimeSpan.Value = oParam.ExpectDeliveryTimeSpan;
            else
                paramExpectDeliveryTimeSpan.Value = System.DBNull.Value;
            if (oParam.AuditDeliveryDate != AppConst.DateTimeNull)
                paramAuditDeliveryDate.Value = oParam.AuditDeliveryDate;
            else
                paramAuditDeliveryDate.Value = System.DBNull.Value;
            if (oParam.AuditDeliveryTimeSpan != AppConst.IntNull)
                paramAuditDeliveryTimeSpan.Value = oParam.AuditDeliveryTimeSpan;
            else
                paramAuditDeliveryTimeSpan.Value = System.DBNull.Value;
            if (oParam.SentDate != AppConst.DateTimeNull)
                paramSentDate.Value = oParam.SentDate;
            else
                paramSentDate.Value = System.DBNull.Value;
            if (oParam.SentTimeSpan != AppConst.IntNull)
                paramSentTimeSpan.Value = oParam.SentTimeSpan;
            else
                paramSentTimeSpan.Value = System.DBNull.Value;
            if (oParam.SignByOther != AppConst.IntNull)
                paramSignByOther.Value = oParam.SignByOther;
            else
                paramSignByOther.Value = System.DBNull.Value;
            if (oParam.HasServiceProduct != AppConst.IntNull)
                paramHasServiceProduct.Value = oParam.HasServiceProduct;
            else
                paramHasServiceProduct.Value = System.DBNull.Value;

            if (oParam.CSUserSysNo != AppConst.IntNull)
                paramCSUserSysNo.Value = oParam.CSUserSysNo;
            else
                paramCSUserSysNo.Value = System.DBNull.Value;

            if (oParam.HasExpectQty != AppConst.IntNull)
                paramHasExpectQty.Value = oParam.HasExpectQty;
            else
                paramHasExpectQty.Value = System.DBNull.Value;
            if (oParam.IsMergeSO != AppConst.IntNull)
                paramIsMergeSO.Value = oParam.IsMergeSO;
            else
                paramIsMergeSO.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramDeliveryDate);
            cmd.Parameters.Add(paramSalesManSysNo);
            cmd.Parameters.Add(paramIsWholeSale);
            cmd.Parameters.Add(paramIsPremium);
            cmd.Parameters.Add(paramPremiumAmt);
            cmd.Parameters.Add(paramShipTypeSysNo);
            cmd.Parameters.Add(paramShipPrice);
            cmd.Parameters.Add(paramFreeShipFeePay);
            cmd.Parameters.Add(paramPayTypeSysNo);
            cmd.Parameters.Add(paramPayPrice);
            cmd.Parameters.Add(paramSOAmt);
            cmd.Parameters.Add(paramDiscountAmt);
            cmd.Parameters.Add(paramPointAmt);
            cmd.Parameters.Add(paramCashPay);
            cmd.Parameters.Add(paramPointPay);
            cmd.Parameters.Add(paramReceiveAreaSysNo);
            cmd.Parameters.Add(paramReceiveContact);
            cmd.Parameters.Add(paramReceiveName);
            cmd.Parameters.Add(paramReceivePhone);
            cmd.Parameters.Add(paramReceiveCellPhone);
            cmd.Parameters.Add(paramReceiveAddress);
            cmd.Parameters.Add(paramReceiveZip);
            cmd.Parameters.Add(paramAllocatedManSysNo);
            cmd.Parameters.Add(paramFreightUserSysNo);
            cmd.Parameters.Add(paramUpdateUserSysNo);
            cmd.Parameters.Add(paramUpdateTime);
            cmd.Parameters.Add(paramAuditUserSysNo);
            cmd.Parameters.Add(paramAuditTime);
            cmd.Parameters.Add(paramManagerAuditUserSysNo);
            cmd.Parameters.Add(paramManagerAuditTime);
            cmd.Parameters.Add(paramOutUserSysNo);
            cmd.Parameters.Add(paramOutTime);
            cmd.Parameters.Add(paramCheckQtyUserSysNo);
            cmd.Parameters.Add(paramCheckQtyTime);
            cmd.Parameters.Add(paramMemo);
            cmd.Parameters.Add(paramNote);
            cmd.Parameters.Add(paramInvoiceNote);
            cmd.Parameters.Add(paramIsVAT);
            cmd.Parameters.Add(paramIsPrintPackageCover);
            cmd.Parameters.Add(paramDeliveryMemo);
            cmd.Parameters.Add(paramVATEMSFee);

            cmd.Parameters.Add(paramExpectDeliveryDate);
            cmd.Parameters.Add(paramExpectDeliveryTimeSpan);
            cmd.Parameters.Add(paramAuditDeliveryDate);
            cmd.Parameters.Add(paramAuditDeliveryTimeSpan);
            cmd.Parameters.Add(paramSentDate);
            cmd.Parameters.Add(paramSentTimeSpan);

            cmd.Parameters.Add(paramSignByOther);
            cmd.Parameters.Add(paramHasServiceProduct);

            cmd.Parameters.Add(paramCSUserSysNo);
            cmd.Parameters.Add(paramHasExpectQty);
            cmd.Parameters.Add(paramIsMergeSO);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
        
        //批量设置配送人
        public int SetDeliveryMenBatch(string soSysNos, int freightManSysNo)
        {
            string sql = "update SO_Master set FreightUserSysNo=" + freightManSysNo.ToString() + ",SetDeliveryManTime=getdate() where sysno in (" + soSysNos + ")";
            return SqlHelper.ExecuteNonQuery(sql);
        }

        public int UpdateSOMaster(Hashtable paramHash)
        {
            StringBuilder sb = new StringBuilder(200);
            sb.Append("UPDATE SO_Master SET ");

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

        public int UpdateSOItem(SOItemInfo oParam)
        {
            string sql = @"UPDATE SO_Item SET 
                           Quantity=@Quantity, 
                           Weight=@Weight, OrderPrice=@OrderPrice,Price=@Price, 
                           Cost=@Cost, Point=@Point, 
                           PointType=@PointType, DiscountAmt=@DiscountAmt, 
                           Warranty=@Warranty, ProductType=@ProductType, 
                           GiftSysNo=@GiftSysNo,BaseProductType=@BaseProductType,ExpectQty=@ExpectQty 
                           WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramQuantity = new SqlParameter("@Quantity", SqlDbType.Int, 4);
            SqlParameter paramWeight = new SqlParameter("@Weight", SqlDbType.Int, 4);
            SqlParameter paramOrderPrice = new SqlParameter("@OrderPrice", SqlDbType.Decimal, 9);
            SqlParameter paramPrice = new SqlParameter("@Price", SqlDbType.Decimal, 9);
            SqlParameter paramCost = new SqlParameter("@Cost", SqlDbType.Decimal, 9);
            SqlParameter paramPoint = new SqlParameter("@Point", SqlDbType.Int, 4);
            SqlParameter paramPointType = new SqlParameter("@PointType", SqlDbType.Int, 4);
            SqlParameter paramDiscountAmt = new SqlParameter("@DiscountAmt", SqlDbType.Decimal, 9);
            SqlParameter paramWarranty = new SqlParameter("@Warranty", SqlDbType.NVarChar, 500);
            SqlParameter paramProductType = new SqlParameter("@ProductType", SqlDbType.Int, 4);
            SqlParameter paramGiftSysNo = new SqlParameter("@GiftSysNo", SqlDbType.Int, 4);
            SqlParameter paramBaseProductType = new SqlParameter("@BaseProductType", SqlDbType.Int, 4);
            SqlParameter paramExpectQty = new SqlParameter("@ExpectQty", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
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

            if (oParam.Price != AppConst.DecimalNull)
                paramPrice.Value = oParam.Price;
            else
                paramPrice.Value = System.DBNull.Value;
            if (oParam.Cost != AppConst.DecimalNull)
                paramCost.Value = oParam.Cost;
            else
                paramCost.Value = System.DBNull.Value;
            if (oParam.Point != AppConst.IntNull)
                paramPoint.Value = oParam.Point;
            else
                paramPoint.Value = System.DBNull.Value;
            if (oParam.PointType != AppConst.IntNull)
                paramPointType.Value = oParam.PointType;
            else
                paramPointType.Value = System.DBNull.Value;
            if (oParam.DiscountAmt != AppConst.DecimalNull)
                paramDiscountAmt.Value = oParam.DiscountAmt;
            else
                paramDiscountAmt.Value = System.DBNull.Value;
            if (oParam.Warranty != AppConst.StringNull)
                paramWarranty.Value = oParam.Warranty;
            else
                paramWarranty.Value = System.DBNull.Value;
            if (oParam.ProductType != AppConst.IntNull)
                paramProductType.Value = oParam.ProductType;
            else
                paramProductType.Value = System.DBNull.Value;
            if (oParam.GiftSysNo != AppConst.IntNull)
                paramGiftSysNo.Value = oParam.GiftSysNo;
            else
                paramGiftSysNo.Value = System.DBNull.Value;
            if (oParam.BaseProductType != AppConst.IntNull)
                paramBaseProductType.Value = oParam.BaseProductType;
            else
                paramBaseProductType.Value = System.DBNull.Value;

            if (oParam.ExpectQty != AppConst.IntNull)
                paramExpectQty.Value = oParam.ExpectQty;
            else
                paramExpectQty.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramQuantity);
            cmd.Parameters.Add(paramWeight);
            cmd.Parameters.Add(paramOrderPrice);
            cmd.Parameters.Add(paramPrice);
            cmd.Parameters.Add(paramCost);
            cmd.Parameters.Add(paramPoint);
            cmd.Parameters.Add(paramPointType);
            cmd.Parameters.Add(paramDiscountAmt);
            cmd.Parameters.Add(paramWarranty);
            cmd.Parameters.Add(paramProductType);
            cmd.Parameters.Add(paramGiftSysNo);
            cmd.Parameters.Add(paramBaseProductType);
            cmd.Parameters.Add(paramExpectQty);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
        public int UpdateSOItem(Hashtable paramHash)
        {
            StringBuilder sb = new StringBuilder(200);
            sb.Append("UPDATE SO_Item SET ");

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
        public int InsertSOSaleRule(SOSaleRuleInfo oParam)
        {
            string sql = @"INSERT INTO SO_SaleRule
                           (
                           SOSysNo, SaleRuleSysNo, SaleRuleName, 
                           Discount, Times, Note
                           )
                           VALUES (
                           @SOSysNo, @SaleRuleSysNo, @SaleRuleName, 
                           @Discount, @Times, @Note
                           )";

            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int, 4);
            SqlParameter paramSaleRuleSysNo = new SqlParameter("@SaleRuleSysNo", SqlDbType.Int, 4);
            SqlParameter paramSaleRuleName = new SqlParameter("@SaleRuleName", SqlDbType.NVarChar, 500);
            SqlParameter paramDiscount = new SqlParameter("@Discount", SqlDbType.Decimal, 9);
            SqlParameter paramTimes = new SqlParameter("@Times", SqlDbType.Int, 4);
            SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.NVarChar, 200);

            if (oParam.SOSysNo != AppConst.IntNull)
                paramSOSysNo.Value = oParam.SOSysNo;
            else
                paramSOSysNo.Value = System.DBNull.Value;
            if (oParam.SaleRuleSysNo != AppConst.IntNull)
                paramSaleRuleSysNo.Value = oParam.SaleRuleSysNo;
            else
                paramSaleRuleSysNo.Value = System.DBNull.Value;
            if (oParam.SaleRuleName != AppConst.StringNull)
                paramSaleRuleName.Value = oParam.SaleRuleName;
            else
                paramSaleRuleName.Value = System.DBNull.Value;
            if (oParam.Discount != AppConst.DecimalNull)
                paramDiscount.Value = oParam.Discount;
            else
                paramDiscount.Value = System.DBNull.Value;
            if (oParam.Times != AppConst.IntNull)
                paramTimes.Value = oParam.Times;
            else
                paramTimes.Value = System.DBNull.Value;
            if (oParam.Note != AppConst.StringNull)
                paramNote.Value = oParam.Note;
            else
                paramNote.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSOSysNo);
            cmd.Parameters.Add(paramSaleRuleSysNo);
            cmd.Parameters.Add(paramSaleRuleName);
            cmd.Parameters.Add(paramDiscount);
            cmd.Parameters.Add(paramTimes);
            cmd.Parameters.Add(paramNote);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
        public int DeleteSOSaleRule(int soSysNo)
        {
            string sql = @"Delete from SO_SaleRule where SOSysNo = @SOSysNo";
            SqlCommand cmd = new SqlCommand(sql);
            SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int, 4);
            if (soSysNo != AppConst.IntNull)
                paramSOSysNo.Value = soSysNo;
            else
                paramSOSysNo.Value = System.DBNull.Value;
            cmd.Parameters.Add(paramSOSysNo);
            return SqlHelper.ExecuteNonQuery(cmd);
        }
        public int DeleteSOItem(SOItemInfo itemInfo)
        {
            string sql = @"Delete from SO_Item where productsysno = @ProductSysNo and sosysno=@SOSysNo";
            SqlCommand cmd = new SqlCommand(sql);
            SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int, 4);
            SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int, 4);
            if (itemInfo.ProductSysNo != AppConst.IntNull)
                paramProductSysNo.Value = itemInfo.ProductSysNo;
            else
                paramProductSysNo.Value = System.DBNull.Value;
            if (itemInfo.SOSysNo != AppConst.IntNull)
                paramSOSysNo.Value = itemInfo.SOSysNo;
            else
                paramSOSysNo.Value = System.DBNull.Value;
            cmd.Parameters.Add(paramProductSysNo);
            cmd.Parameters.Add(paramSOSysNo);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int InsertSOItemPO(SOItemPOInfo oParam)
        {
            string sql = @"INSERT INTO SO_Item_PO
                            (
                            SOItemSysNo, POSysNo, ProductIDSysNo
                            )
                            VALUES (
                            @SOItemSysNo, @POSysNo, @ProductIDSysNo
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramSOItemSysNo = new SqlParameter("@SOItemSysNo", SqlDbType.Int, 4);
            SqlParameter paramPOSysNo = new SqlParameter("@POSysNo", SqlDbType.Int, 4);
            SqlParameter paramProductIDSysNo = new SqlParameter("@ProductIDSysNo", SqlDbType.Int, 4);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.SOItemSysNo != AppConst.IntNull)
                paramSOItemSysNo.Value = oParam.SOItemSysNo;
            else
                paramSOItemSysNo.Value = System.DBNull.Value;
            if (oParam.POSysNo != AppConst.IntNull)
                paramPOSysNo.Value = oParam.POSysNo;
            else
                paramPOSysNo.Value = System.DBNull.Value;
            if (oParam.ProductIDSysNo != AppConst.IntNull)
                paramProductIDSysNo.Value = oParam.ProductIDSysNo;
            else
                paramProductIDSysNo.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramSOItemSysNo);
            cmd.Parameters.Add(paramPOSysNo);
            cmd.Parameters.Add(paramProductIDSysNo);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }
        public int UpdateSOItemPO(SOItemPOInfo oParam)
        {
            string sql = @"UPDATE SO_Item_PO SET 
                            SOItemSysNo=@SOItemSysNo, POSysNo=@POSysNo, 
                            ProductIDSysNo=@ProductIDSysNo
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramSOItemSysNo = new SqlParameter("@SOItemSysNo", SqlDbType.Int, 4);
            SqlParameter paramPOSysNo = new SqlParameter("@POSysNo", SqlDbType.Int, 4);
            SqlParameter paramProductIDSysNo = new SqlParameter("@ProductIDSysNo", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.SOItemSysNo != AppConst.IntNull)
                paramSOItemSysNo.Value = oParam.SOItemSysNo;
            else
                paramSOItemSysNo.Value = System.DBNull.Value;
            if (oParam.POSysNo != AppConst.IntNull)
                paramPOSysNo.Value = oParam.POSysNo;
            else
                paramPOSysNo.Value = System.DBNull.Value;
            if (oParam.ProductIDSysNo != AppConst.IntNull)
                paramProductIDSysNo.Value = oParam.ProductIDSysNo;
            else
                paramProductIDSysNo.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramSOItemSysNo);
            cmd.Parameters.Add(paramPOSysNo);
            cmd.Parameters.Add(paramProductIDSysNo);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
    }
}