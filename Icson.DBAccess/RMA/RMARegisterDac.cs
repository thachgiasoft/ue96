using System;
using Icson.Utils;
using Icson.Objects;
using Icson.Objects.RMA;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Icson.DBAccess.RMA
{
    /// <summary>
    /// Summary description for RMARegisterDac.
    /// </summary>
    public class RMARegisterDac
    {
        public RMARegisterDac()
        {
        }

        public int Insert(RMARegisterInfo oParam)
        {
            string sql = @"INSERT INTO RMA_Register
                            (
                            SysNo, ProductSysNo, RequestType, CustomerDesc, 
                            CheckTime, CheckDesc, NewProductStatus, RevertStatus, 
                            OutBoundStatus, ReturnStatus, ResponseDesc, ResponseTime, 
                            RefundStatus, NextHandler, Memo, Status, 
                            ProductNo, ProductIDSysNo, NewProductIDSysNo, IsWithin7Days, 
                            IsRecommendRefund, RefundInfo, OwnBy, Location, 
                            Cost, RMAReason, CheckUserSysNo, ResponseUserSysNo, 
                            CloseUserSysNo, CloseTime, RevertAuditUserSysNo, RevertAuditTime, 
                            RevertAuditMemo, RevertProductSysNo, IsHaveInvoice, IsFullAccessory, 
                            IsFullPackage, ReceiveStockSysNo, AttachmentInfo, InspectionResultType, 
                            VendorRepairResultType, OutBoundWithInvoice, ResponseProductNo, RevertStockSysNo, 
                            PMDunDesc, PMDunTime, IsContactCustomer, RegisterReceiveType, 
                            RefundAuditUserSysNo, RefundAuditTime, RefundAuditMemo
                            )
                            VALUES (
                            @SysNo, @ProductSysNo, @RequestType, @CustomerDesc, 
                            @CheckTime, @CheckDesc, @NewProductStatus, @RevertStatus, 
                            @OutBoundStatus, @ReturnStatus, @ResponseDesc, @ResponseTime, 
                            @RefundStatus, @NextHandler, @Memo, @Status, 
                            @ProductNo, @ProductIDSysNo, @NewProductIDSysNo, @IsWithin7Days, 
                            @IsRecommendRefund, @RefundInfo, @OwnBy, @Location, 
                            @Cost, @RMAReason, @CheckUserSysNo, @ResponseUserSysNo, 
                            @CloseUserSysNo, @CloseTime, @RevertAuditUserSysNo, @RevertAuditTime, 
                            @RevertAuditMemo, @RevertProductSysNo, @IsHaveInvoice, @IsFullAccessory, 
                            @IsFullPackage, @ReceiveStockSysNo, @AttachmentInfo, @InspectionResultType, 
                            @VendorRepairResultType, @OutBoundWithInvoice, @ResponseProductNo, @RevertStockSysNo, 
                            @PMDunDesc, @PMDunTime, @IsContactCustomer, @RegisterReceiveType, 
                            @RefundAuditUserSysNo, @RefundAuditTime, @RefundAuditMemo
                            )";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int, 4);
            SqlParameter paramRequestType = new SqlParameter("@RequestType", SqlDbType.Int, 4);
            SqlParameter paramCustomerDesc = new SqlParameter("@CustomerDesc", SqlDbType.NVarChar, 500);
            SqlParameter paramCheckTime = new SqlParameter("@CheckTime", SqlDbType.DateTime);
            SqlParameter paramCheckDesc = new SqlParameter("@CheckDesc", SqlDbType.NVarChar, 500);
            SqlParameter paramNewProductStatus = new SqlParameter("@NewProductStatus", SqlDbType.Int, 4);
            SqlParameter paramRevertStatus = new SqlParameter("@RevertStatus", SqlDbType.Int, 4);
            SqlParameter paramOutBoundStatus = new SqlParameter("@OutBoundStatus", SqlDbType.Int, 4);
            SqlParameter paramReturnStatus = new SqlParameter("@ReturnStatus", SqlDbType.Int, 4);
            SqlParameter paramResponseDesc = new SqlParameter("@ResponseDesc", SqlDbType.NVarChar, 200);
            SqlParameter paramResponseTime = new SqlParameter("@ResponseTime", SqlDbType.DateTime);
            SqlParameter paramRefundStatus = new SqlParameter("@RefundStatus", SqlDbType.Int, 4);
            SqlParameter paramNextHandler = new SqlParameter("@NextHandler", SqlDbType.Int, 4);
            SqlParameter paramMemo = new SqlParameter("@Memo", SqlDbType.NVarChar, 200);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramProductNo = new SqlParameter("@ProductNo", SqlDbType.NVarChar, 50);
            SqlParameter paramProductIDSysNo = new SqlParameter("@ProductIDSysNo", SqlDbType.Int, 4);
            SqlParameter paramNewProductIDSysNo = new SqlParameter("@NewProductIDSysNo", SqlDbType.Int, 4);
            SqlParameter paramIsWithin7Days = new SqlParameter("@IsWithin7Days", SqlDbType.Int, 4);
            SqlParameter paramIsRecommendRefund = new SqlParameter("@IsRecommendRefund", SqlDbType.Int, 4);
            SqlParameter paramRefundInfo = new SqlParameter("@RefundInfo", SqlDbType.NVarChar, 200);
            SqlParameter paramOwnBy = new SqlParameter("@OwnBy", SqlDbType.Int, 4);
            SqlParameter paramLocation = new SqlParameter("@Location", SqlDbType.Int, 4);
            SqlParameter paramCost = new SqlParameter("@Cost", SqlDbType.Decimal, 9);
            SqlParameter paramRMAReason = new SqlParameter("@RMAReason", SqlDbType.Int, 4);
            SqlParameter paramCheckUserSysNo = new SqlParameter("@CheckUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramResponseUserSysNo = new SqlParameter("@ResponseUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCloseUserSysNo = new SqlParameter("@CloseUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCloseTime = new SqlParameter("@CloseTime", SqlDbType.DateTime);
            SqlParameter paramRevertAuditUserSysNo = new SqlParameter("@RevertAuditUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramRevertAuditTime = new SqlParameter("@RevertAuditTime", SqlDbType.DateTime);
            SqlParameter paramRevertAuditMemo = new SqlParameter("@RevertAuditMemo", SqlDbType.NVarChar, 100);
            SqlParameter paramRevertProductSysNo = new SqlParameter("@RevertProductSysNo", SqlDbType.Int, 4);
            SqlParameter paramIsHaveInvoice = new SqlParameter("@IsHaveInvoice", SqlDbType.Int, 4);
            SqlParameter paramIsFullAccessory = new SqlParameter("@IsFullAccessory", SqlDbType.Int, 4);
            SqlParameter paramIsFullPackage = new SqlParameter("@IsFullPackage", SqlDbType.Int, 4);
            SqlParameter paramReceiveStockSysNo = new SqlParameter("@ReceiveStockSysNo", SqlDbType.Int, 4);
            SqlParameter paramAttachmentInfo = new SqlParameter("@AttachmentInfo", SqlDbType.NVarChar, 500);
            SqlParameter paramInspectionResultType = new SqlParameter("@InspectionResultType", SqlDbType.NVarChar, 100);
            SqlParameter paramVendorRepairResultType = new SqlParameter("@VendorRepairResultType", SqlDbType.NVarChar, 100);
            SqlParameter paramOutBoundWithInvoice = new SqlParameter("@OutBoundWithInvoice", SqlDbType.Int, 4);
            SqlParameter paramResponseProductNo = new SqlParameter("@ResponseProductNo", SqlDbType.NVarChar, 50);
            SqlParameter paramRevertStockSysNo = new SqlParameter("@RevertStockSysNo", SqlDbType.Int, 4);
            SqlParameter paramPMDunDesc = new SqlParameter("@PMDunDesc", SqlDbType.NVarChar, 1000);
            SqlParameter paramPMDunTime = new SqlParameter("@PMDunTime", SqlDbType.DateTime);
            SqlParameter paramIsContactCustomer = new SqlParameter("@IsContactCustomer", SqlDbType.Int, 4);
            SqlParameter paramRegisterReceiveType = new SqlParameter("@RegisterReceiveType", SqlDbType.Int, 4);
            SqlParameter paramRefundAuditUserSysNo = new SqlParameter("@RefundAuditUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramRefundAuditTime = new SqlParameter("@RefundAuditTime", SqlDbType.DateTime);
            SqlParameter paramRefundAuditMemo = new SqlParameter("@RefundAuditMemo", SqlDbType.NVarChar, 100);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.ProductSysNo != AppConst.IntNull)
                paramProductSysNo.Value = oParam.ProductSysNo;
            else
                paramProductSysNo.Value = System.DBNull.Value;
            if (oParam.RequestType != AppConst.IntNull)
                paramRequestType.Value = oParam.RequestType;
            else
                paramRequestType.Value = System.DBNull.Value;
            if (oParam.CustomerDesc != AppConst.StringNull)
                paramCustomerDesc.Value = oParam.CustomerDesc;
            else
                paramCustomerDesc.Value = System.DBNull.Value;
            if (oParam.CheckTime != AppConst.DateTimeNull)
                paramCheckTime.Value = oParam.CheckTime;
            else
                paramCheckTime.Value = System.DBNull.Value;
            if (oParam.CheckDesc != AppConst.StringNull)
                paramCheckDesc.Value = oParam.CheckDesc;
            else
                paramCheckDesc.Value = System.DBNull.Value;
            if (oParam.NewProductStatus != AppConst.IntNull)
                paramNewProductStatus.Value = oParam.NewProductStatus;
            else
                paramNewProductStatus.Value = System.DBNull.Value;
            if (oParam.RevertStatus != AppConst.IntNull)
                paramRevertStatus.Value = oParam.RevertStatus;
            else
                paramRevertStatus.Value = System.DBNull.Value;
            if (oParam.OutBoundStatus != AppConst.IntNull)
                paramOutBoundStatus.Value = oParam.OutBoundStatus;
            else
                paramOutBoundStatus.Value = System.DBNull.Value;
            if (oParam.ReturnStatus != AppConst.IntNull)
                paramReturnStatus.Value = oParam.ReturnStatus;
            else
                paramReturnStatus.Value = System.DBNull.Value;
            if (oParam.ResponseDesc != AppConst.StringNull)
                paramResponseDesc.Value = oParam.ResponseDesc;
            else
                paramResponseDesc.Value = System.DBNull.Value;
            if (oParam.ResponseTime != AppConst.DateTimeNull)
                paramResponseTime.Value = oParam.ResponseTime;
            else
                paramResponseTime.Value = System.DBNull.Value;
            if (oParam.RefundStatus != AppConst.IntNull)
                paramRefundStatus.Value = oParam.RefundStatus;
            else
                paramRefundStatus.Value = System.DBNull.Value;
            if (oParam.NextHandler != AppConst.IntNull)
                paramNextHandler.Value = oParam.NextHandler;
            else
                paramNextHandler.Value = System.DBNull.Value;
            if (oParam.Memo != AppConst.StringNull)
                paramMemo.Value = oParam.Memo;
            else
                paramMemo.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;
            if (oParam.ProductNo != AppConst.StringNull)
                paramProductNo.Value = oParam.ProductNo;
            else
                paramProductNo.Value = System.DBNull.Value;
            if (oParam.ProductIDSysNo != AppConst.IntNull)
                paramProductIDSysNo.Value = oParam.ProductIDSysNo;
            else
                paramProductIDSysNo.Value = System.DBNull.Value;
            if (oParam.NewProductIDSysNo != AppConst.IntNull)
                paramNewProductIDSysNo.Value = oParam.NewProductIDSysNo;
            else
                paramNewProductIDSysNo.Value = System.DBNull.Value;
            if (oParam.IsWithin7Days != AppConst.IntNull)
                paramIsWithin7Days.Value = oParam.IsWithin7Days;
            else
                paramIsWithin7Days.Value = System.DBNull.Value;
            if (oParam.IsRecommendRefund != AppConst.IntNull)
                paramIsRecommendRefund.Value = oParam.IsRecommendRefund;
            else
                paramIsRecommendRefund.Value = System.DBNull.Value;
            if (oParam.RefundInfo != AppConst.StringNull)
                paramRefundInfo.Value = oParam.RefundInfo;
            else
                paramRefundInfo.Value = System.DBNull.Value;
            if (oParam.OwnBy != AppConst.IntNull)
                paramOwnBy.Value = oParam.OwnBy;
            else
                paramOwnBy.Value = System.DBNull.Value;
            if (oParam.Location != AppConst.IntNull)
                paramLocation.Value = oParam.Location;
            else
                paramLocation.Value = System.DBNull.Value;
            if (oParam.Cost != AppConst.DecimalNull)
                paramCost.Value = oParam.Cost;
            else
                paramCost.Value = System.DBNull.Value;
            if (oParam.RMAReason != AppConst.IntNull)
                paramRMAReason.Value = oParam.RMAReason;
            else
                paramRMAReason.Value = System.DBNull.Value;
            if (oParam.CheckUserSysNo != AppConst.IntNull)
                paramCheckUserSysNo.Value = oParam.CheckUserSysNo;
            else
                paramCheckUserSysNo.Value = System.DBNull.Value;
            if (oParam.ResponseUserSysNo != AppConst.IntNull)
                paramResponseUserSysNo.Value = oParam.ResponseUserSysNo;
            else
                paramResponseUserSysNo.Value = System.DBNull.Value;
            if (oParam.CloseUserSysNo != AppConst.IntNull)
                paramCloseUserSysNo.Value = oParam.CloseUserSysNo;
            else
                paramCloseUserSysNo.Value = System.DBNull.Value;
            if (oParam.CloseTime != AppConst.DateTimeNull)
                paramCloseTime.Value = oParam.CloseTime;
            else
                paramCloseTime.Value = System.DBNull.Value;
            if (oParam.RevertAuditUserSysNo != AppConst.IntNull)
                paramRevertAuditUserSysNo.Value = oParam.RevertAuditUserSysNo;
            else
                paramRevertAuditUserSysNo.Value = System.DBNull.Value;
            if (oParam.RevertAuditTime != AppConst.DateTimeNull)
                paramRevertAuditTime.Value = oParam.RevertAuditTime;
            else
                paramRevertAuditTime.Value = System.DBNull.Value;
            if (oParam.RevertAuditMemo != AppConst.StringNull)
                paramRevertAuditMemo.Value = oParam.RevertAuditMemo;
            else
                paramRevertAuditMemo.Value = System.DBNull.Value;
            if (oParam.RevertProductSysNo != AppConst.IntNull)
                paramRevertProductSysNo.Value = oParam.RevertProductSysNo;
            else
                paramRevertProductSysNo.Value = System.DBNull.Value;
            if (oParam.IsHaveInvoice != AppConst.IntNull)
                paramIsHaveInvoice.Value = oParam.IsHaveInvoice;
            else
                paramIsHaveInvoice.Value = System.DBNull.Value;
            if (oParam.IsFullAccessory != AppConst.IntNull)
                paramIsFullAccessory.Value = oParam.IsFullAccessory;
            else
                paramIsFullAccessory.Value = System.DBNull.Value;
            if (oParam.IsFullPackage != AppConst.IntNull)
                paramIsFullPackage.Value = oParam.IsFullPackage;
            else
                paramIsFullPackage.Value = System.DBNull.Value;
            if (oParam.ReceiveStockSysNo != AppConst.IntNull)
                paramReceiveStockSysNo.Value = oParam.ReceiveStockSysNo;
            else
                paramReceiveStockSysNo.Value = System.DBNull.Value;
            if (oParam.AttachmentInfo != AppConst.StringNull)
                paramAttachmentInfo.Value = oParam.AttachmentInfo;
            else
                paramAttachmentInfo.Value = System.DBNull.Value;
            if (oParam.InspectionResultType != AppConst.StringNull)
                paramInspectionResultType.Value = oParam.InspectionResultType;
            else
                paramInspectionResultType.Value = System.DBNull.Value;
            if (oParam.VendorRepairResultType != AppConst.StringNull)
                paramVendorRepairResultType.Value = oParam.VendorRepairResultType;
            else
                paramVendorRepairResultType.Value = System.DBNull.Value;
            if (oParam.OutBoundWithInvoice != AppConst.IntNull)
                paramOutBoundWithInvoice.Value = oParam.OutBoundWithInvoice;
            else
                paramOutBoundWithInvoice.Value = System.DBNull.Value;
            if (oParam.ResponseProductNo != AppConst.StringNull)
                paramResponseProductNo.Value = oParam.ResponseProductNo;
            else
                paramResponseProductNo.Value = System.DBNull.Value;
            if (oParam.RevertStockSysNo != AppConst.IntNull)
                paramRevertStockSysNo.Value = oParam.RevertStockSysNo;
            else
                paramRevertStockSysNo.Value = System.DBNull.Value;
            if (oParam.PMDunDesc != AppConst.StringNull)
                paramPMDunDesc.Value = oParam.PMDunDesc;
            else
                paramPMDunDesc.Value = System.DBNull.Value;
            if (oParam.PMDunTime != AppConst.DateTimeNull)
                paramPMDunTime.Value = oParam.PMDunTime;
            else
                paramPMDunTime.Value = System.DBNull.Value;
            if (oParam.IsContactCustomer != AppConst.IntNull)
                paramIsContactCustomer.Value = oParam.IsContactCustomer;
            else
                paramIsContactCustomer.Value = System.DBNull.Value;
            if (oParam.RegisterReceiveType != AppConst.IntNull)
                paramRegisterReceiveType.Value = oParam.RegisterReceiveType;
            else
                paramRegisterReceiveType.Value = System.DBNull.Value;
            if (oParam.RefundAuditUserSysNo != AppConst.IntNull)
                paramRefundAuditUserSysNo.Value = oParam.RefundAuditUserSysNo;
            else
                paramRefundAuditUserSysNo.Value = System.DBNull.Value;
            if (oParam.RefundAuditTime != AppConst.DateTimeNull)
                paramRefundAuditTime.Value = oParam.RefundAuditTime;
            else
                paramRefundAuditTime.Value = System.DBNull.Value;
            if (oParam.RefundAuditMemo != AppConst.StringNull)
                paramRefundAuditMemo.Value = oParam.RefundAuditMemo;
            else
                paramRefundAuditMemo.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramProductSysNo);
            cmd.Parameters.Add(paramRequestType);
            cmd.Parameters.Add(paramCustomerDesc);
            cmd.Parameters.Add(paramCheckTime);
            cmd.Parameters.Add(paramCheckDesc);
            cmd.Parameters.Add(paramNewProductStatus);
            cmd.Parameters.Add(paramRevertStatus);
            cmd.Parameters.Add(paramOutBoundStatus);
            cmd.Parameters.Add(paramReturnStatus);
            cmd.Parameters.Add(paramResponseDesc);
            cmd.Parameters.Add(paramResponseTime);
            cmd.Parameters.Add(paramRefundStatus);
            cmd.Parameters.Add(paramNextHandler);
            cmd.Parameters.Add(paramMemo);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramProductNo);
            cmd.Parameters.Add(paramProductIDSysNo);
            cmd.Parameters.Add(paramNewProductIDSysNo);
            cmd.Parameters.Add(paramIsWithin7Days);
            cmd.Parameters.Add(paramIsRecommendRefund);
            cmd.Parameters.Add(paramRefundInfo);
            cmd.Parameters.Add(paramOwnBy);
            cmd.Parameters.Add(paramLocation);
            cmd.Parameters.Add(paramCost);
            cmd.Parameters.Add(paramRMAReason);
            cmd.Parameters.Add(paramCheckUserSysNo);
            cmd.Parameters.Add(paramResponseUserSysNo);
            cmd.Parameters.Add(paramCloseUserSysNo);
            cmd.Parameters.Add(paramCloseTime);
            cmd.Parameters.Add(paramRevertAuditUserSysNo);
            cmd.Parameters.Add(paramRevertAuditTime);
            cmd.Parameters.Add(paramRevertAuditMemo);
            cmd.Parameters.Add(paramRevertProductSysNo);
            cmd.Parameters.Add(paramIsHaveInvoice);
            cmd.Parameters.Add(paramIsFullAccessory);
            cmd.Parameters.Add(paramIsFullPackage);
            cmd.Parameters.Add(paramReceiveStockSysNo);
            cmd.Parameters.Add(paramAttachmentInfo);
            cmd.Parameters.Add(paramInspectionResultType);
            cmd.Parameters.Add(paramVendorRepairResultType);
            cmd.Parameters.Add(paramOutBoundWithInvoice);
            cmd.Parameters.Add(paramResponseProductNo);
            cmd.Parameters.Add(paramRevertStockSysNo);
            cmd.Parameters.Add(paramPMDunDesc);
            cmd.Parameters.Add(paramPMDunTime);
            cmd.Parameters.Add(paramIsContactCustomer);
            cmd.Parameters.Add(paramRegisterReceiveType);
            cmd.Parameters.Add(paramRefundAuditUserSysNo);
            cmd.Parameters.Add(paramRefundAuditTime);
            cmd.Parameters.Add(paramRefundAuditMemo);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int Update(Hashtable paramHash)
        {
            StringBuilder sb = new StringBuilder(200);
            sb.Append("UPDATE RMA_Register SET ");

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

                    if (item is int)
                    {
                        if ((int)item == AppConst.IntNull)
                            sb.Append(key).Append("= null");
                        else
                            sb.Append(key).Append("=").Append(item.ToString());
                    }
                    else if (item is decimal)
                    {
                        if ((decimal)item == AppConst.DecimalNull)
                            sb.Append(key).Append("= null");
                        else
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

                sb.Append(" WHERE SysNo=").Append(paramHash["SysNo"]);
            }
            return SqlHelper.ExecuteNonQuery(sb.ToString());
        }
    }
}