using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using Icson.Objects.Sale;
using Icson.Utils;

namespace Icson.DBAccess.Sale
{
    public class DSDac
    {
        public int Insert(DSInfo oParam)
        {
            string sql = @"INSERT INTO DS_Master
                            (
                            SysNo, DLSysNo, FreightUserSysNo, ARAmt, 
                            IncomeAmt, CreateUserSysNo, CreateTime, SettlementUserSysNo, 
                            SettlementTime, VoucherUserSysNo, VoucherID, VoucherTime, 
                            AbandonUserSysNo, AbandonTime, Status, AuditUserSysNo, 
                            AuditTime, PosFee, Cheque, Cash, 
                            PosGoods, Remittance, RemittanceDate, Memo, 
                            RemittanceType, IsReceiveVoucher, PosGoodsCash, AccAuditUserSysNo, 
                            AccAuditTime
                            )
                            VALUES (
                            @SysNo, @DLSysNo, @FreightUserSysNo, @ARAmt, 
                            @IncomeAmt, @CreateUserSysNo, @CreateTime, @SettlementUserSysNo, 
                            @SettlementTime, @VoucherUserSysNo, @VoucherID, @VoucherTime, 
                            @AbandonUserSysNo, @AbandonTime, @Status, @AuditUserSysNo, 
                            @AuditTime, @PosFee, @Cheque, @Cash, 
                            @PosGoods, @Remittance, @RemittanceDate, @Memo, 
                            @RemittanceType, @IsReceiveVoucher, @PosGoodsCash, @AccAuditUserSysNo, 
                            @AccAuditTime
                            )";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramDLSysNo = new SqlParameter("@DLSysNo", SqlDbType.Int, 4);
            SqlParameter paramFreightUserSysNo = new SqlParameter("@FreightUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramARAmt = new SqlParameter("@ARAmt", SqlDbType.Decimal, 9);
            SqlParameter paramIncomeAmt = new SqlParameter("@IncomeAmt", SqlDbType.Decimal, 9);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
            SqlParameter paramSettlementUserSysNo = new SqlParameter("@SettlementUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramSettlementTime = new SqlParameter("@SettlementTime", SqlDbType.DateTime);
            SqlParameter paramVoucherUserSysNo = new SqlParameter("@VoucherUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramVoucherID = new SqlParameter("@VoucherID", SqlDbType.NVarChar, 50);
            SqlParameter paramVoucherTime = new SqlParameter("@VoucherTime", SqlDbType.DateTime);
            SqlParameter paramAbandonUserSysNo = new SqlParameter("@AbandonUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramAbandonTime = new SqlParameter("@AbandonTime", SqlDbType.DateTime);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramAuditUserSysNo = new SqlParameter("@AuditUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramAuditTime = new SqlParameter("@AuditTime", SqlDbType.DateTime);
            SqlParameter paramPosFee = new SqlParameter("@PosFee", SqlDbType.Decimal, 9);
            SqlParameter paramCheque = new SqlParameter("@Cheque", SqlDbType.Decimal, 9);
            SqlParameter paramCash = new SqlParameter("@Cash", SqlDbType.Decimal, 9);
            SqlParameter paramPosGoods = new SqlParameter("@PosGoods", SqlDbType.Decimal, 9);
            SqlParameter paramRemittance = new SqlParameter("@Remittance", SqlDbType.Decimal, 9);
            SqlParameter paramRemittanceDate = new SqlParameter("@RemittanceDate", SqlDbType.DateTime);
            SqlParameter paramMemo = new SqlParameter("@Memo", SqlDbType.NVarChar, 500);
            SqlParameter paramRemittanceType = new SqlParameter("@RemittanceType", SqlDbType.Int, 4);
            SqlParameter paramIsReceiveVoucher = new SqlParameter("@IsReceiveVoucher", SqlDbType.Int, 4);
            SqlParameter paramPosGoodsCash = new SqlParameter("@PosGoodsCash", SqlDbType.Decimal, 9);
            SqlParameter paramAccAuditUserSysNo = new SqlParameter("@AccAuditUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramAccAuditTime = new SqlParameter("@AccAuditTime", SqlDbType.DateTime);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.DLSysNo != AppConst.IntNull)
                paramDLSysNo.Value = oParam.DLSysNo;
            else
                paramDLSysNo.Value = System.DBNull.Value;
            if (oParam.FreightUserSysNo != AppConst.IntNull)
                paramFreightUserSysNo.Value = oParam.FreightUserSysNo;
            else
                paramFreightUserSysNo.Value = System.DBNull.Value;
            if (oParam.ARAmt != AppConst.DecimalNull)
                paramARAmt.Value = oParam.ARAmt;
            else
                paramARAmt.Value = System.DBNull.Value;
            if (oParam.IncomeAmt != AppConst.DecimalNull)
                paramIncomeAmt.Value = oParam.IncomeAmt;
            else
                paramIncomeAmt.Value = System.DBNull.Value;
            if (oParam.CreateUserSysNo != AppConst.IntNull)
                paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
            else
                paramCreateUserSysNo.Value = System.DBNull.Value;
            if (oParam.CreateTime != AppConst.DateTimeNull)
                paramCreateTime.Value = oParam.CreateTime;
            else
                paramCreateTime.Value = System.DBNull.Value;
            if (oParam.SettlementUserSysNo != AppConst.IntNull)
                paramSettlementUserSysNo.Value = oParam.SettlementUserSysNo;
            else
                paramSettlementUserSysNo.Value = System.DBNull.Value;
            if (oParam.SettlementTime != AppConst.DateTimeNull)
                paramSettlementTime.Value = oParam.SettlementTime;
            else
                paramSettlementTime.Value = System.DBNull.Value;
            if (oParam.VoucherUserSysNo != AppConst.IntNull)
                paramVoucherUserSysNo.Value = oParam.VoucherUserSysNo;
            else
                paramVoucherUserSysNo.Value = System.DBNull.Value;
            if (oParam.VoucherID != AppConst.StringNull)
                paramVoucherID.Value = oParam.VoucherID;
            else
                paramVoucherID.Value = System.DBNull.Value;
            if (oParam.VoucherTime != AppConst.DateTimeNull)
                paramVoucherTime.Value = oParam.VoucherTime;
            else
                paramVoucherTime.Value = System.DBNull.Value;
            if (oParam.AbandonUserSysNo != AppConst.IntNull)
                paramAbandonUserSysNo.Value = oParam.AbandonUserSysNo;
            else
                paramAbandonUserSysNo.Value = System.DBNull.Value;
            if (oParam.AbandonTime != AppConst.DateTimeNull)
                paramAbandonTime.Value = oParam.AbandonTime;
            else
                paramAbandonTime.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;
            if (oParam.AuditUserSysNo != AppConst.IntNull)
                paramAuditUserSysNo.Value = oParam.AuditUserSysNo;
            else
                paramAuditUserSysNo.Value = System.DBNull.Value;
            if (oParam.AuditTime != AppConst.DateTimeNull)
                paramAuditTime.Value = oParam.AuditTime;
            else
                paramAuditTime.Value = System.DBNull.Value;
            if (oParam.PosFee != AppConst.DecimalNull)
                paramPosFee.Value = oParam.PosFee;
            else
                paramPosFee.Value = System.DBNull.Value;
            if (oParam.Cheque != AppConst.DecimalNull)
                paramCheque.Value = oParam.Cheque;
            else
                paramCheque.Value = System.DBNull.Value;
            if (oParam.Cash != AppConst.DecimalNull)
                paramCash.Value = oParam.Cash;
            else
                paramCash.Value = System.DBNull.Value;
            if (oParam.PosGoods != AppConst.DecimalNull)
                paramPosGoods.Value = oParam.PosGoods;
            else
                paramPosGoods.Value = System.DBNull.Value;
            if (oParam.Remittance != AppConst.DecimalNull)
                paramRemittance.Value = oParam.Remittance;
            else
                paramRemittance.Value = System.DBNull.Value;
            if (oParam.RemittanceDate != AppConst.DateTimeNull)
                paramRemittanceDate.Value = oParam.RemittanceDate;
            else
                paramRemittanceDate.Value = System.DBNull.Value;
            if (oParam.Memo != AppConst.StringNull)
                paramMemo.Value = oParam.Memo;
            else
                paramMemo.Value = System.DBNull.Value;
            if (oParam.RemittanceType != AppConst.IntNull)
                paramRemittanceType.Value = oParam.RemittanceType;
            else
                paramRemittanceType.Value = System.DBNull.Value;
            if (oParam.IsReceiveVoucher != AppConst.IntNull)
                paramIsReceiveVoucher.Value = oParam.IsReceiveVoucher;
            else
                paramIsReceiveVoucher.Value = System.DBNull.Value;
            if (oParam.PosGoodsCash != AppConst.DecimalNull)
                paramPosGoodsCash.Value = oParam.PosGoodsCash;
            else
                paramPosGoodsCash.Value = System.DBNull.Value;
            if (oParam.AccAuditUserSysNo != AppConst.IntNull)
                paramAccAuditUserSysNo.Value = oParam.AccAuditUserSysNo;
            else
                paramAccAuditUserSysNo.Value = System.DBNull.Value;
            if (oParam.AccAuditTime != AppConst.DateTimeNull)
                paramAccAuditTime.Value = oParam.AccAuditTime;
            else
                paramAccAuditTime.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramDLSysNo);
            cmd.Parameters.Add(paramFreightUserSysNo);
            cmd.Parameters.Add(paramARAmt);
            cmd.Parameters.Add(paramIncomeAmt);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramCreateTime);
            cmd.Parameters.Add(paramSettlementUserSysNo);
            cmd.Parameters.Add(paramSettlementTime);
            cmd.Parameters.Add(paramVoucherUserSysNo);
            cmd.Parameters.Add(paramVoucherID);
            cmd.Parameters.Add(paramVoucherTime);
            cmd.Parameters.Add(paramAbandonUserSysNo);
            cmd.Parameters.Add(paramAbandonTime);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramAuditUserSysNo);
            cmd.Parameters.Add(paramAuditTime);
            cmd.Parameters.Add(paramPosFee);
            cmd.Parameters.Add(paramCheque);
            cmd.Parameters.Add(paramCash);
            cmd.Parameters.Add(paramPosGoods);
            cmd.Parameters.Add(paramRemittance);
            cmd.Parameters.Add(paramRemittanceDate);
            cmd.Parameters.Add(paramMemo);
            cmd.Parameters.Add(paramRemittanceType);
            cmd.Parameters.Add(paramIsReceiveVoucher);
            cmd.Parameters.Add(paramPosGoodsCash);
            cmd.Parameters.Add(paramAccAuditUserSysNo);
            cmd.Parameters.Add(paramAccAuditTime);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
        public int Update(DSInfo oParam)
        {
            string sql = @"UPDATE DS_Master SET 
                            DLSysNo=@DLSysNo, FreightUserSysNo=@FreightUserSysNo, 
                            ARAmt=@ARAmt, IncomeAmt=@IncomeAmt, 
                            CreateUserSysNo=@CreateUserSysNo, CreateTime=@CreateTime, 
                            SettlementUserSysNo=@SettlementUserSysNo, SettlementTime=@SettlementTime, 
                            VoucherUserSysNo=@VoucherUserSysNo, VoucherID=@VoucherID, 
                            VoucherTime=@VoucherTime, AbandonUserSysNo=@AbandonUserSysNo, 
                            AbandonTime=@AbandonTime, Status=@Status, 
                            AuditUserSysNo=@AuditUserSysNo, AuditTime=@AuditTime, 
                            PosFee=@PosFee, Cheque=@Cheque, 
                            Cash=@Cash, PosGoods=@PosGoods, 
                            Remittance=@Remittance, RemittanceDate=@RemittanceDate, 
                            Memo=@Memo, RemittanceType=@RemittanceType, 
                            IsReceiveVoucher=@IsReceiveVoucher, PosGoodsCash=@PosGoodsCash, 
                            AccAuditUserSysNo=@AccAuditUserSysNo, AccAuditTime=@AccAuditTime
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramDLSysNo = new SqlParameter("@DLSysNo", SqlDbType.Int, 4);
            SqlParameter paramFreightUserSysNo = new SqlParameter("@FreightUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramARAmt = new SqlParameter("@ARAmt", SqlDbType.Decimal, 9);
            SqlParameter paramIncomeAmt = new SqlParameter("@IncomeAmt", SqlDbType.Decimal, 9);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
            SqlParameter paramSettlementUserSysNo = new SqlParameter("@SettlementUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramSettlementTime = new SqlParameter("@SettlementTime", SqlDbType.DateTime);
            SqlParameter paramVoucherUserSysNo = new SqlParameter("@VoucherUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramVoucherID = new SqlParameter("@VoucherID", SqlDbType.NVarChar, 50);
            SqlParameter paramVoucherTime = new SqlParameter("@VoucherTime", SqlDbType.DateTime);
            SqlParameter paramAbandonUserSysNo = new SqlParameter("@AbandonUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramAbandonTime = new SqlParameter("@AbandonTime", SqlDbType.DateTime);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramAuditUserSysNo = new SqlParameter("@AuditUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramAuditTime = new SqlParameter("@AuditTime", SqlDbType.DateTime);
            SqlParameter paramPosFee = new SqlParameter("@PosFee", SqlDbType.Decimal, 9);
            SqlParameter paramCheque = new SqlParameter("@Cheque", SqlDbType.Decimal, 9);
            SqlParameter paramCash = new SqlParameter("@Cash", SqlDbType.Decimal, 9);
            SqlParameter paramPosGoods = new SqlParameter("@PosGoods", SqlDbType.Decimal, 9);
            SqlParameter paramRemittance = new SqlParameter("@Remittance", SqlDbType.Decimal, 9);
            SqlParameter paramRemittanceDate = new SqlParameter("@RemittanceDate", SqlDbType.DateTime);
            SqlParameter paramMemo = new SqlParameter("@Memo", SqlDbType.NVarChar, 500);
            SqlParameter paramRemittanceType = new SqlParameter("@RemittanceType", SqlDbType.Int, 4);
            SqlParameter paramIsReceiveVoucher = new SqlParameter("@IsReceiveVoucher", SqlDbType.Int, 4);
            SqlParameter paramPosGoodsCash = new SqlParameter("@PosGoodsCash", SqlDbType.Decimal, 9);
            SqlParameter paramAccAuditUserSysNo = new SqlParameter("@AccAuditUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramAccAuditTime = new SqlParameter("@AccAuditTime", SqlDbType.DateTime);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.DLSysNo != AppConst.IntNull)
                paramDLSysNo.Value = oParam.DLSysNo;
            else
                paramDLSysNo.Value = System.DBNull.Value;
            if (oParam.FreightUserSysNo != AppConst.IntNull)
                paramFreightUserSysNo.Value = oParam.FreightUserSysNo;
            else
                paramFreightUserSysNo.Value = System.DBNull.Value;
            if (oParam.ARAmt != AppConst.DecimalNull)
                paramARAmt.Value = oParam.ARAmt;
            else
                paramARAmt.Value = System.DBNull.Value;
            if (oParam.IncomeAmt != AppConst.DecimalNull)
                paramIncomeAmt.Value = oParam.IncomeAmt;
            else
                paramIncomeAmt.Value = System.DBNull.Value;
            if (oParam.CreateUserSysNo != AppConst.IntNull)
                paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
            else
                paramCreateUserSysNo.Value = System.DBNull.Value;
            if (oParam.CreateTime != AppConst.DateTimeNull)
                paramCreateTime.Value = oParam.CreateTime;
            else
                paramCreateTime.Value = System.DBNull.Value;
            if (oParam.SettlementUserSysNo != AppConst.IntNull)
                paramSettlementUserSysNo.Value = oParam.SettlementUserSysNo;
            else
                paramSettlementUserSysNo.Value = System.DBNull.Value;
            if (oParam.SettlementTime != AppConst.DateTimeNull)
                paramSettlementTime.Value = oParam.SettlementTime;
            else
                paramSettlementTime.Value = System.DBNull.Value;
            if (oParam.VoucherUserSysNo != AppConst.IntNull)
                paramVoucherUserSysNo.Value = oParam.VoucherUserSysNo;
            else
                paramVoucherUserSysNo.Value = System.DBNull.Value;
            if (oParam.VoucherID != AppConst.StringNull)
                paramVoucherID.Value = oParam.VoucherID;
            else
                paramVoucherID.Value = System.DBNull.Value;
            if (oParam.VoucherTime != AppConst.DateTimeNull)
                paramVoucherTime.Value = oParam.VoucherTime;
            else
                paramVoucherTime.Value = System.DBNull.Value;
            if (oParam.AbandonUserSysNo != AppConst.IntNull)
                paramAbandonUserSysNo.Value = oParam.AbandonUserSysNo;
            else
                paramAbandonUserSysNo.Value = System.DBNull.Value;
            if (oParam.AbandonTime != AppConst.DateTimeNull)
                paramAbandonTime.Value = oParam.AbandonTime;
            else
                paramAbandonTime.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;
            if (oParam.AuditUserSysNo != AppConst.IntNull)
                paramAuditUserSysNo.Value = oParam.AuditUserSysNo;
            else
                paramAuditUserSysNo.Value = System.DBNull.Value;
            if (oParam.AuditTime != AppConst.DateTimeNull)
                paramAuditTime.Value = oParam.AuditTime;
            else
                paramAuditTime.Value = System.DBNull.Value;
            if (oParam.PosFee != AppConst.DecimalNull)
                paramPosFee.Value = oParam.PosFee;
            else
                paramPosFee.Value = System.DBNull.Value;
            if (oParam.Cheque != AppConst.DecimalNull)
                paramCheque.Value = oParam.Cheque;
            else
                paramCheque.Value = System.DBNull.Value;
            if (oParam.Cash != AppConst.DecimalNull)
                paramCash.Value = oParam.Cash;
            else
                paramCash.Value = System.DBNull.Value;
            if (oParam.PosGoods != AppConst.DecimalNull)
                paramPosGoods.Value = oParam.PosGoods;
            else
                paramPosGoods.Value = System.DBNull.Value;
            if (oParam.Remittance != AppConst.DecimalNull)
                paramRemittance.Value = oParam.Remittance;
            else
                paramRemittance.Value = System.DBNull.Value;
            if (oParam.RemittanceDate != AppConst.DateTimeNull)
                paramRemittanceDate.Value = oParam.RemittanceDate;
            else
                paramRemittanceDate.Value = System.DBNull.Value;
            if (oParam.Memo != AppConst.StringNull)
                paramMemo.Value = oParam.Memo;
            else
                paramMemo.Value = System.DBNull.Value;
            if (oParam.RemittanceType != AppConst.IntNull)
                paramRemittanceType.Value = oParam.RemittanceType;
            else
                paramRemittanceType.Value = System.DBNull.Value;
            if (oParam.IsReceiveVoucher != AppConst.IntNull)
                paramIsReceiveVoucher.Value = oParam.IsReceiveVoucher;
            else
                paramIsReceiveVoucher.Value = System.DBNull.Value;
            if (oParam.PosGoodsCash != AppConst.DecimalNull)
                paramPosGoodsCash.Value = oParam.PosGoodsCash;
            else
                paramPosGoodsCash.Value = System.DBNull.Value;
            if (oParam.AccAuditUserSysNo != AppConst.IntNull)
                paramAccAuditUserSysNo.Value = oParam.AccAuditUserSysNo;
            else
                paramAccAuditUserSysNo.Value = System.DBNull.Value;
            if (oParam.AccAuditTime != AppConst.DateTimeNull)
                paramAccAuditTime.Value = oParam.AccAuditTime;
            else
                paramAccAuditTime.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramDLSysNo);
            cmd.Parameters.Add(paramFreightUserSysNo);
            cmd.Parameters.Add(paramARAmt);
            cmd.Parameters.Add(paramIncomeAmt);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramCreateTime);
            cmd.Parameters.Add(paramSettlementUserSysNo);
            cmd.Parameters.Add(paramSettlementTime);
            cmd.Parameters.Add(paramVoucherUserSysNo);
            cmd.Parameters.Add(paramVoucherID);
            cmd.Parameters.Add(paramVoucherTime);
            cmd.Parameters.Add(paramAbandonUserSysNo);
            cmd.Parameters.Add(paramAbandonTime);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramAuditUserSysNo);
            cmd.Parameters.Add(paramAuditTime);
            cmd.Parameters.Add(paramPosFee);
            cmd.Parameters.Add(paramCheque);
            cmd.Parameters.Add(paramCash);
            cmd.Parameters.Add(paramPosGoods);
            cmd.Parameters.Add(paramRemittance);
            cmd.Parameters.Add(paramRemittanceDate);
            cmd.Parameters.Add(paramMemo);
            cmd.Parameters.Add(paramRemittanceType);
            cmd.Parameters.Add(paramIsReceiveVoucher);
            cmd.Parameters.Add(paramPosGoodsCash);
            cmd.Parameters.Add(paramAccAuditUserSysNo);
            cmd.Parameters.Add(paramAccAuditTime);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int Insert(DSItemInfo oParam)
        {
            string sql = @"INSERT INTO DS_Item
                            (
                            DSSysNo, ItemID, ItemType, PayType, 
                            PayAmt, Status, PosFee, DLSysNo, 
                            IsPos
                            )
                            VALUES (
                            @DSSysNo, @ItemID, @ItemType, @PayType, 
                            @PayAmt, @Status, @PosFee, @DLSysNo, 
                            @IsPos
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramDSSysNo = new SqlParameter("@DSSysNo", SqlDbType.Int, 4);
            SqlParameter paramItemID = new SqlParameter("@ItemID", SqlDbType.NVarChar, 50);
            SqlParameter paramItemType = new SqlParameter("@ItemType", SqlDbType.Int, 4);
            SqlParameter paramPayType = new SqlParameter("@PayType", SqlDbType.Int, 4);
            SqlParameter paramPayAmt = new SqlParameter("@PayAmt", SqlDbType.Decimal, 9);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramPosFee = new SqlParameter("@PosFee", SqlDbType.Decimal, 9);
            SqlParameter paramDLSysNo = new SqlParameter("@DLSysNo", SqlDbType.Int, 4);
            SqlParameter paramIsPos = new SqlParameter("@IsPos", SqlDbType.Int, 4);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.DSSysNo != AppConst.IntNull)
                paramDSSysNo.Value = oParam.DSSysNo;
            else
                paramDSSysNo.Value = System.DBNull.Value;
            if (oParam.ItemID != AppConst.StringNull)
                paramItemID.Value = oParam.ItemID;
            else
                paramItemID.Value = System.DBNull.Value;
            if (oParam.ItemType != AppConst.IntNull)
                paramItemType.Value = oParam.ItemType;
            else
                paramItemType.Value = System.DBNull.Value;
            if (oParam.PayType != AppConst.IntNull)
                paramPayType.Value = oParam.PayType;
            else
                paramPayType.Value = System.DBNull.Value;
            if (oParam.PayAmt != AppConst.DecimalNull)
                paramPayAmt.Value = oParam.PayAmt;
            else
                paramPayAmt.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;
            if (oParam.PosFee != AppConst.DecimalNull)
                paramPosFee.Value = oParam.PosFee;
            else
                paramPosFee.Value = System.DBNull.Value;
            if (oParam.DLSysNo != AppConst.IntNull)
                paramDLSysNo.Value = oParam.DLSysNo;
            else
                paramDLSysNo.Value = System.DBNull.Value;
            if (oParam.IsPos != AppConst.IntNull)
                paramIsPos.Value = oParam.IsPos;
            else
                paramIsPos.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramDSSysNo);
            cmd.Parameters.Add(paramItemID);
            cmd.Parameters.Add(paramItemType);
            cmd.Parameters.Add(paramPayType);
            cmd.Parameters.Add(paramPayAmt);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramPosFee);
            cmd.Parameters.Add(paramDLSysNo);
            cmd.Parameters.Add(paramIsPos);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }
        public int Update(DSItemInfo oParam)
        {
            string sql = @"UPDATE DS_Item SET 
                            DSSysNo=@DSSysNo, ItemID=@ItemID, 
                            ItemType=@ItemType, PayType=@PayType, 
                            PayAmt=@PayAmt, Status=@Status, 
                            PosFee=@PosFee, DLSysNo=@DLSysNo, 
                            IsPos=@IsPos
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramDSSysNo = new SqlParameter("@DSSysNo", SqlDbType.Int, 4);
            SqlParameter paramItemID = new SqlParameter("@ItemID", SqlDbType.NVarChar, 50);
            SqlParameter paramItemType = new SqlParameter("@ItemType", SqlDbType.Int, 4);
            SqlParameter paramPayType = new SqlParameter("@PayType", SqlDbType.Int, 4);
            SqlParameter paramPayAmt = new SqlParameter("@PayAmt", SqlDbType.Decimal, 9);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramPosFee = new SqlParameter("@PosFee", SqlDbType.Decimal, 9);
            SqlParameter paramDLSysNo = new SqlParameter("@DLSysNo", SqlDbType.Int, 4);
            SqlParameter paramIsPos = new SqlParameter("@IsPos", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.DSSysNo != AppConst.IntNull)
                paramDSSysNo.Value = oParam.DSSysNo;
            else
                paramDSSysNo.Value = System.DBNull.Value;
            if (oParam.ItemID != AppConst.StringNull)
                paramItemID.Value = oParam.ItemID;
            else
                paramItemID.Value = System.DBNull.Value;
            if (oParam.ItemType != AppConst.IntNull)
                paramItemType.Value = oParam.ItemType;
            else
                paramItemType.Value = System.DBNull.Value;
            if (oParam.PayType != AppConst.IntNull)
                paramPayType.Value = oParam.PayType;
            else
                paramPayType.Value = System.DBNull.Value;
            if (oParam.PayAmt != AppConst.DecimalNull)
                paramPayAmt.Value = oParam.PayAmt;
            else
                paramPayAmt.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;
            if (oParam.PosFee != AppConst.DecimalNull)
                paramPosFee.Value = oParam.PosFee;
            else
                paramPosFee.Value = System.DBNull.Value;
            if (oParam.DLSysNo != AppConst.IntNull)
                paramDLSysNo.Value = oParam.DLSysNo;
            else
                paramDLSysNo.Value = System.DBNull.Value;
            if (oParam.IsPos != AppConst.IntNull)
                paramIsPos.Value = oParam.IsPos;
            else
                paramIsPos.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramDSSysNo);
            cmd.Parameters.Add(paramItemID);
            cmd.Parameters.Add(paramItemType);
            cmd.Parameters.Add(paramPayType);
            cmd.Parameters.Add(paramPayAmt);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramPosFee);
            cmd.Parameters.Add(paramDLSysNo);
            cmd.Parameters.Add(paramIsPos);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int UpdateMaster(Hashtable paramHash)
        {
            StringBuilder sb = new StringBuilder(200);
            sb.Append("UPDATE DS_Master SET ");

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
    }
}