using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Text;

using Icson.Utils;
using Icson.Objects.Finance;

namespace Icson.DBAccess.Finance
{
    /// <summary>
    /// Summary description for POPayDac.
    /// </summary>
    public class POPayDac
    {

        public POPayDac()
        {
        }

        public int InsertMaster(POPayInfo oParam)
        {
            string sql = @"INSERT INTO Finance_POPay
                            (
                            POSysNo, CurrencySysNo, POAmt, 
                            AlreadyPayAmt, PayStatus, InvoiceStatus, Note
                            )
                            VALUES (
                            @POSysNo, @CurrencySysNo, @POAmt, 
                            @AlreadyPayAmt, @PayStatus, @InvoiceStatus, @Note
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramPOSysNo = new SqlParameter("@POSysNo", SqlDbType.Int, 4);
            SqlParameter paramCurrencySysNo = new SqlParameter("@CurrencySysNo", SqlDbType.Int, 4);
            SqlParameter paramPOAmt = new SqlParameter("@POAmt", SqlDbType.Decimal, 9);
            SqlParameter paramAlreadyPayAmt = new SqlParameter("@AlreadyPayAmt", SqlDbType.Decimal, 9);
            SqlParameter paramPayStatus = new SqlParameter("@PayStatus", SqlDbType.Int, 4);
            SqlParameter paramInvoiceStatus = new SqlParameter("@InvoiceStatus", SqlDbType.Int, 4);
            SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.NVarChar, 500);

            paramSysNo.Direction = ParameterDirection.Output;

            if (oParam.POSysNo != AppConst.IntNull)
                paramPOSysNo.Value = oParam.POSysNo;
            else
                paramPOSysNo.Value = System.DBNull.Value;
            if (oParam.CurrencySysNo != AppConst.IntNull)
                paramCurrencySysNo.Value = oParam.CurrencySysNo;
            else
                paramCurrencySysNo.Value = System.DBNull.Value;
            if (oParam.POAmt != AppConst.DecimalNull)
                paramPOAmt.Value = oParam.POAmt;
            else
                paramPOAmt.Value = System.DBNull.Value;
            if (oParam.AlreadyPayAmt != AppConst.DecimalNull)
                paramAlreadyPayAmt.Value = oParam.AlreadyPayAmt;
            else
                paramAlreadyPayAmt.Value = System.DBNull.Value;
            if (oParam.PayStatus != AppConst.IntNull)
                paramPayStatus.Value = oParam.PayStatus;
            else
                paramPayStatus.Value = System.DBNull.Value;
            if (oParam.InvoiceStatus != AppConst.IntNull)
                paramInvoiceStatus.Value = oParam.InvoiceStatus;
            else
                paramInvoiceStatus.Value = System.DBNull.Value;
            if (oParam.Note != AppConst.StringNull)
                paramNote.Value = oParam.Note;
            else
                paramNote.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramPOSysNo);
            cmd.Parameters.Add(paramCurrencySysNo);
            cmd.Parameters.Add(paramPOAmt);
            cmd.Parameters.Add(paramAlreadyPayAmt);
            cmd.Parameters.Add(paramPayStatus);
            cmd.Parameters.Add(paramInvoiceStatus);
            cmd.Parameters.Add(paramNote);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);

        }
        public int UpdateMaster(Hashtable paramHash)
        {
            StringBuilder sb = new StringBuilder(200);
            sb.Append("UPDATE Finance_POPay SET ");

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
        public int DeleteMaster(int poSysNo)
        {
            string sql = "delete from finance_popay where posysno = " + poSysNo;
            SqlCommand cmd = new SqlCommand(sql);
            return SqlHelper.ExecuteNonQuery(cmd);
        }


        public int InsertItem(POPayItemInfo oParam)
        {
            string sql = @"INSERT INTO Finance_POPay_Item
                            (
                            POSysNo, PayStyle, PayAmt, CreateTime, 
                            CreateUserSysNo, EstimatePayTime, ReferenceID, PayTime, 
                            PayUserSysNo, Note, Status, IsPrintPOPayBill, 
                            RequestUserSysNo, RequestTime, AuditUserSysNo, AuditTime, 
                            VoucherTime
                            )
                            VALUES (
                            @POSysNo, @PayStyle, @PayAmt, @CreateTime, 
                            @CreateUserSysNo, @EstimatePayTime, @ReferenceID, @PayTime, 
                            @PayUserSysNo, @Note, @Status, @IsPrintPOPayBill, 
                            @RequestUserSysNo, @RequestTime, @AuditUserSysNo, @AuditTime, 
                            @VoucherTime
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramPOSysNo = new SqlParameter("@POSysNo", SqlDbType.Int, 4);
            SqlParameter paramPayStyle = new SqlParameter("@PayStyle", SqlDbType.Int, 4);
            SqlParameter paramPayAmt = new SqlParameter("@PayAmt", SqlDbType.Decimal, 9);
            SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramEstimatePayTime = new SqlParameter("@EstimatePayTime", SqlDbType.DateTime);
            SqlParameter paramReferenceID = new SqlParameter("@ReferenceID", SqlDbType.NVarChar, 20);
            SqlParameter paramPayTime = new SqlParameter("@PayTime", SqlDbType.DateTime);
            SqlParameter paramPayUserSysNo = new SqlParameter("@PayUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.NVarChar, 200);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramIsPrintPOPayBill = new SqlParameter("@IsPrintPOPayBill", SqlDbType.Int, 4);
            SqlParameter paramRequestUserSysNo = new SqlParameter("@RequestUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramRequestTime = new SqlParameter("@RequestTime", SqlDbType.DateTime);
            SqlParameter paramAuditUserSysNo = new SqlParameter("@AuditUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramAuditTime = new SqlParameter("@AuditTime", SqlDbType.DateTime);
            SqlParameter paramVoucherTime = new SqlParameter("@VoucherTime", SqlDbType.DateTime);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.POSysNo != AppConst.IntNull)
                paramPOSysNo.Value = oParam.POSysNo;
            else
                paramPOSysNo.Value = System.DBNull.Value;
            if (oParam.PayStyle != AppConst.IntNull)
                paramPayStyle.Value = oParam.PayStyle;
            else
                paramPayStyle.Value = System.DBNull.Value;
            if (oParam.PayAmt != AppConst.DecimalNull)
                paramPayAmt.Value = oParam.PayAmt;
            else
                paramPayAmt.Value = System.DBNull.Value;
            if (oParam.CreateTime != AppConst.DateTimeNull)
                paramCreateTime.Value = oParam.CreateTime;
            else
                paramCreateTime.Value = System.DBNull.Value;
            if (oParam.CreateUserSysNo != AppConst.IntNull)
                paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
            else
                paramCreateUserSysNo.Value = System.DBNull.Value;
            if (oParam.EstimatePayTime != AppConst.DateTimeNull)
                paramEstimatePayTime.Value = oParam.EstimatePayTime;
            else
                paramEstimatePayTime.Value = System.DBNull.Value;
            if (oParam.ReferenceID != AppConst.StringNull)
                paramReferenceID.Value = oParam.ReferenceID;
            else
                paramReferenceID.Value = System.DBNull.Value;
            if (oParam.PayTime != AppConst.DateTimeNull)
                paramPayTime.Value = oParam.PayTime;
            else
                paramPayTime.Value = System.DBNull.Value;
            if (oParam.PayUserSysNo != AppConst.IntNull)
                paramPayUserSysNo.Value = oParam.PayUserSysNo;
            else
                paramPayUserSysNo.Value = System.DBNull.Value;
            if (oParam.Note != AppConst.StringNull)
                paramNote.Value = oParam.Note;
            else
                paramNote.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;
            if (oParam.IsPrintPOPayBill != AppConst.IntNull)
                paramIsPrintPOPayBill.Value = oParam.IsPrintPOPayBill;
            else
                paramIsPrintPOPayBill.Value = System.DBNull.Value;
            if (oParam.RequestUserSysNo != AppConst.IntNull)
                paramRequestUserSysNo.Value = oParam.RequestUserSysNo;
            else
                paramRequestUserSysNo.Value = System.DBNull.Value;
            if (oParam.RequestTime != AppConst.DateTimeNull)
                paramRequestTime.Value = oParam.RequestTime;
            else
                paramRequestTime.Value = System.DBNull.Value;
            if (oParam.AuditUserSysNo != AppConst.IntNull)
                paramAuditUserSysNo.Value = oParam.AuditUserSysNo;
            else
                paramAuditUserSysNo.Value = System.DBNull.Value;
            if (oParam.AuditTime != AppConst.DateTimeNull)
                paramAuditTime.Value = oParam.AuditTime;
            else
                paramAuditTime.Value = System.DBNull.Value;
            if (oParam.VoucherTime != AppConst.DateTimeNull)
                paramVoucherTime.Value = oParam.VoucherTime;
            else
                paramVoucherTime.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramPOSysNo);
            cmd.Parameters.Add(paramPayStyle);
            cmd.Parameters.Add(paramPayAmt);
            cmd.Parameters.Add(paramCreateTime);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramEstimatePayTime);
            cmd.Parameters.Add(paramReferenceID);
            cmd.Parameters.Add(paramPayTime);
            cmd.Parameters.Add(paramPayUserSysNo);
            cmd.Parameters.Add(paramNote);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramIsPrintPOPayBill);
            cmd.Parameters.Add(paramRequestUserSysNo);
            cmd.Parameters.Add(paramRequestTime);
            cmd.Parameters.Add(paramAuditUserSysNo);
            cmd.Parameters.Add(paramAuditTime);
            cmd.Parameters.Add(paramVoucherTime);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }
        public int UpdateItem(Hashtable paramHash)
        {
            StringBuilder sb = new StringBuilder(200);
            sb.Append("UPDATE Finance_POPay_Item SET ");

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
                        if ((DateTime)item == AppConst.DateTimeNull)
                            sb.Append(key).Append(" = null ");
                        else
                            sb.Append(key).Append(" = cast(").Append(Util.ToSqlString(((DateTime)item).ToString(AppConst.DateFormatLong))).Append(" as DateTime)");
                    }
                }
            }

            sb.Append(" WHERE SysNo=").Append(paramHash["SysNo"]);

            return SqlHelper.ExecuteNonQuery(sb.ToString());
        }

        public int DeleteItem(int poPayItemSysNo)
        {
            string sql = "delete from finance_popay_item where sysno = " + poPayItemSysNo;
            SqlCommand cmd = new SqlCommand(sql);
            return SqlHelper.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// 异常付款单记录申请
        /// </summary>
        /// <param name="oParam"></param>
        /// <returns></returns>
        public int InsertPOPayItemErrRequest(POPayItemErrRequestInfo oParam)
        {
            string sql = @"INSERT INTO Fin_POPay_Item_ErrRequest
                            (
                            POPayItemSysNo, RequestUserSysNo, RequestTime, RequestUserNote, 
                            ErrMsgNote, TLAuditUserSysNo, TLAuditTime, TLNote, 
                            LastAuditUserSysNo, LastAuditTime, LastAuditNote, Status
                            )
                            VALUES (
                            @POPayItemSysNo, @RequestUserSysNo, @RequestTime, @RequestUserNote, 
                            @ErrMsgNote, @TLAuditUserSysNo, @TLAuditTime, @TLNote, 
                            @LastAuditUserSysNo, @LastAuditTime, @LastAuditNote, @Status
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramPOPayItemSysNo = new SqlParameter("@POPayItemSysNo", SqlDbType.Int, 4);
            SqlParameter paramRequestUserSysNo = new SqlParameter("@RequestUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramRequestTime = new SqlParameter("@RequestTime", SqlDbType.DateTime);
            SqlParameter paramRequestUserNote = new SqlParameter("@RequestUserNote", SqlDbType.NVarChar, 200);
            SqlParameter paramErrMsgNote = new SqlParameter("@ErrMsgNote", SqlDbType.Text, 0);
            SqlParameter paramTLAuditUserSysNo = new SqlParameter("@TLAuditUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramTLAuditTime = new SqlParameter("@TLAuditTime", SqlDbType.DateTime);
            SqlParameter paramTLNote = new SqlParameter("@TLNote", SqlDbType.NVarChar, 200);
            SqlParameter paramLastAuditUserSysNo = new SqlParameter("@LastAuditUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramLastAuditTime = new SqlParameter("@LastAuditTime", SqlDbType.DateTime);
            SqlParameter paramLastAuditNote = new SqlParameter("@LastAuditNote", SqlDbType.NVarChar, 200);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.POPayItemSysNo != AppConst.IntNull)
                paramPOPayItemSysNo.Value = oParam.POPayItemSysNo;
            else
                paramPOPayItemSysNo.Value = System.DBNull.Value;
            if (oParam.RequestUserSysNo != AppConst.IntNull)
                paramRequestUserSysNo.Value = oParam.RequestUserSysNo;
            else
                paramRequestUserSysNo.Value = System.DBNull.Value;
            if (oParam.RequestTime != AppConst.DateTimeNull)
                paramRequestTime.Value = oParam.RequestTime;
            else
                paramRequestTime.Value = System.DBNull.Value;
            if (oParam.RequestUserNote != AppConst.StringNull)
                paramRequestUserNote.Value = oParam.RequestUserNote;
            else
                paramRequestUserNote.Value = System.DBNull.Value;
            if (oParam.ErrMsgNote != AppConst.StringNull)
                paramErrMsgNote.Value = oParam.ErrMsgNote;
            else
                paramErrMsgNote.Value = System.DBNull.Value;
            if (oParam.TLAuditUserSysNo != AppConst.IntNull)
                paramTLAuditUserSysNo.Value = oParam.TLAuditUserSysNo;
            else
                paramTLAuditUserSysNo.Value = System.DBNull.Value;
            if (oParam.TLAuditTime != AppConst.DateTimeNull)
                paramTLAuditTime.Value = oParam.TLAuditTime;
            else
                paramTLAuditTime.Value = System.DBNull.Value;
            if (oParam.TLNote != AppConst.StringNull)
                paramTLNote.Value = oParam.TLNote;
            else
                paramTLNote.Value = System.DBNull.Value;
            if (oParam.LastAuditUserSysNo != AppConst.IntNull)
                paramLastAuditUserSysNo.Value = oParam.LastAuditUserSysNo;
            else
                paramLastAuditUserSysNo.Value = System.DBNull.Value;
            if (oParam.LastAuditTime != AppConst.DateTimeNull)
                paramLastAuditTime.Value = oParam.LastAuditTime;
            else
                paramLastAuditTime.Value = System.DBNull.Value;
            if (oParam.LastAuditNote != AppConst.StringNull)
                paramLastAuditNote.Value = oParam.LastAuditNote;
            else
                paramLastAuditNote.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramPOPayItemSysNo);
            cmd.Parameters.Add(paramRequestUserSysNo);
            cmd.Parameters.Add(paramRequestTime);
            cmd.Parameters.Add(paramRequestUserNote);
            cmd.Parameters.Add(paramErrMsgNote);
            cmd.Parameters.Add(paramTLAuditUserSysNo);
            cmd.Parameters.Add(paramTLAuditTime);
            cmd.Parameters.Add(paramTLNote);
            cmd.Parameters.Add(paramLastAuditUserSysNo);
            cmd.Parameters.Add(paramLastAuditTime);
            cmd.Parameters.Add(paramLastAuditNote);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        /// <summary>
        /// 异常付款单记录修改
        /// </summary>
        /// <param name="oParam"></param>
        /// <returns></returns>
        public int UpdatePOPayItemErrRequest(POPayItemErrRequestInfo oParam)
        {
            string sql = @"UPDATE Fin_POPay_Item_ErrRequest SET 
                            POPayItemSysNo=@POPayItemSysNo, RequestUserSysNo=@RequestUserSysNo, 
                            RequestTime=@RequestTime, RequestUserNote=@RequestUserNote, 
                            ErrMsgNote=@ErrMsgNote, TLAuditUserSysNo=@TLAuditUserSysNo, 
                            TLAuditTime=@TLAuditTime, TLNote=@TLNote, 
                            LastAuditUserSysNo=@LastAuditUserSysNo, LastAuditTime=@LastAuditTime, 
                            LastAuditNote=@LastAuditNote, Status=@Status
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramPOPayItemSysNo = new SqlParameter("@POPayItemSysNo", SqlDbType.Int, 4);
            SqlParameter paramRequestUserSysNo = new SqlParameter("@RequestUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramRequestTime = new SqlParameter("@RequestTime", SqlDbType.DateTime);
            SqlParameter paramRequestUserNote = new SqlParameter("@RequestUserNote", SqlDbType.NVarChar, 200);
            SqlParameter paramErrMsgNote = new SqlParameter("@ErrMsgNote", SqlDbType.Text, 0);
            SqlParameter paramTLAuditUserSysNo = new SqlParameter("@TLAuditUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramTLAuditTime = new SqlParameter("@TLAuditTime", SqlDbType.DateTime);
            SqlParameter paramTLNote = new SqlParameter("@TLNote", SqlDbType.NVarChar, 200);
            SqlParameter paramLastAuditUserSysNo = new SqlParameter("@LastAuditUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramLastAuditTime = new SqlParameter("@LastAuditTime", SqlDbType.DateTime);
            SqlParameter paramLastAuditNote = new SqlParameter("@LastAuditNote", SqlDbType.NVarChar, 200);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.POPayItemSysNo != AppConst.IntNull)
                paramPOPayItemSysNo.Value = oParam.POPayItemSysNo;
            else
                paramPOPayItemSysNo.Value = System.DBNull.Value;
            if (oParam.RequestUserSysNo != AppConst.IntNull)
                paramRequestUserSysNo.Value = oParam.RequestUserSysNo;
            else
                paramRequestUserSysNo.Value = System.DBNull.Value;
            if (oParam.RequestTime != AppConst.DateTimeNull)
                paramRequestTime.Value = oParam.RequestTime;
            else
                paramRequestTime.Value = System.DBNull.Value;
            if (oParam.RequestUserNote != AppConst.StringNull)
                paramRequestUserNote.Value = oParam.RequestUserNote;
            else
                paramRequestUserNote.Value = System.DBNull.Value;
            if (oParam.ErrMsgNote != AppConst.StringNull)
                paramErrMsgNote.Value = oParam.ErrMsgNote;
            else
                paramErrMsgNote.Value = System.DBNull.Value;
            if (oParam.TLAuditUserSysNo != AppConst.IntNull)
                paramTLAuditUserSysNo.Value = oParam.TLAuditUserSysNo;
            else
                paramTLAuditUserSysNo.Value = System.DBNull.Value;
            if (oParam.TLAuditTime != AppConst.DateTimeNull)
                paramTLAuditTime.Value = oParam.TLAuditTime;
            else
                paramTLAuditTime.Value = System.DBNull.Value;
            if (oParam.TLNote != AppConst.StringNull)
                paramTLNote.Value = oParam.TLNote;
            else
                paramTLNote.Value = System.DBNull.Value;
            if (oParam.LastAuditUserSysNo != AppConst.IntNull)
                paramLastAuditUserSysNo.Value = oParam.LastAuditUserSysNo;
            else
                paramLastAuditUserSysNo.Value = System.DBNull.Value;
            if (oParam.LastAuditTime != AppConst.DateTimeNull)
                paramLastAuditTime.Value = oParam.LastAuditTime;
            else
                paramLastAuditTime.Value = System.DBNull.Value;
            if (oParam.LastAuditNote != AppConst.StringNull)
                paramLastAuditNote.Value = oParam.LastAuditNote;
            else
                paramLastAuditNote.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramPOPayItemSysNo);
            cmd.Parameters.Add(paramRequestUserSysNo);
            cmd.Parameters.Add(paramRequestTime);
            cmd.Parameters.Add(paramRequestUserNote);
            cmd.Parameters.Add(paramErrMsgNote);
            cmd.Parameters.Add(paramTLAuditUserSysNo);
            cmd.Parameters.Add(paramTLAuditTime);
            cmd.Parameters.Add(paramTLNote);
            cmd.Parameters.Add(paramLastAuditUserSysNo);
            cmd.Parameters.Add(paramLastAuditTime);
            cmd.Parameters.Add(paramLastAuditNote);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// 异常付款单记录修改
        /// </summary>
        /// <param name="paramHash"></param>
        /// <returns></returns>
        public int UpdatePOPayItemErrRequest(Hashtable paramHash)
        {
            return UtilDac.GetInstance().Update(paramHash, "Fin_POPay_Item_ErrRequest");
        }
    }
}
