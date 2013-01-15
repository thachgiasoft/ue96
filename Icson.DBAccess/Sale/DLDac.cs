using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using Icson.Objects.Sale;
using Icson.Utils;

namespace Icson.DBAccess.Sale
{
    public class DLDac
    {
        public int Insert(DLInfo oParam)
        {
            string sql = @"INSERT INTO DL_Master
                            (
                            SysNo, FreightUserSysNo, CreateUserSysNo, CreateTime, 
                            ConfirmUserSysNo, ConfirmTime, IncomeUserSysNo, IncomeTime, 
                            UpdateFreightManUserSysNo, UpdateFreightManTime, VoucherUserSysNo, VoucherID, 
                            VoucherTime, HasPaidQty, HasPaidAmt, CODQty, 
                            CODAmt, Type, Status, IsSendSMS, 
                            IsAllow
                            )
                            VALUES (
                            @SysNo, @FreightUserSysNo, @CreateUserSysNo, @CreateTime, 
                            @ConfirmUserSysNo, @ConfirmTime, @IncomeUserSysNo, @IncomeTime, 
                            @UpdateFreightManUserSysNo, @UpdateFreightManTime, @VoucherUserSysNo, @VoucherID, 
                            @VoucherTime, @HasPaidQty, @HasPaidAmt, @CODQty, 
                            @CODAmt, @Type, @Status, @IsSendSMS, 
                            @IsAllow
                            )";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramFreightUserSysNo = new SqlParameter("@FreightUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
            SqlParameter paramConfirmUserSysNo = new SqlParameter("@ConfirmUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramConfirmTime = new SqlParameter("@ConfirmTime", SqlDbType.DateTime);
            SqlParameter paramIncomeUserSysNo = new SqlParameter("@IncomeUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramIncomeTime = new SqlParameter("@IncomeTime", SqlDbType.DateTime);
            SqlParameter paramUpdateFreightManUserSysNo = new SqlParameter("@UpdateFreightManUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramUpdateFreightManTime = new SqlParameter("@UpdateFreightManTime", SqlDbType.DateTime);
            SqlParameter paramVoucherUserSysNo = new SqlParameter("@VoucherUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramVoucherID = new SqlParameter("@VoucherID", SqlDbType.NVarChar, 50);
            SqlParameter paramVoucherTime = new SqlParameter("@VoucherTime", SqlDbType.DateTime);
            SqlParameter paramHasPaidQty = new SqlParameter("@HasPaidQty", SqlDbType.Int, 4);
            SqlParameter paramHasPaidAmt = new SqlParameter("@HasPaidAmt", SqlDbType.Decimal, 9);
            SqlParameter paramCODQty = new SqlParameter("@CODQty", SqlDbType.Int, 4);
            SqlParameter paramCODAmt = new SqlParameter("@CODAmt", SqlDbType.Decimal, 9);
            SqlParameter paramType = new SqlParameter("@Type", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramIsSendSMS = new SqlParameter("@IsSendSMS", SqlDbType.Int, 4);
            SqlParameter paramIsAllow = new SqlParameter("@IsAllow", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.FreightUserSysNo != AppConst.IntNull)
                paramFreightUserSysNo.Value = oParam.FreightUserSysNo;
            else
                paramFreightUserSysNo.Value = System.DBNull.Value;
            if (oParam.CreateUserSysNo != AppConst.IntNull)
                paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
            else
                paramCreateUserSysNo.Value = System.DBNull.Value;
            if (oParam.CreateTime != AppConst.DateTimeNull)
                paramCreateTime.Value = oParam.CreateTime;
            else
                paramCreateTime.Value = System.DBNull.Value;
            if (oParam.ConfirmUserSysNo != AppConst.IntNull)
                paramConfirmUserSysNo.Value = oParam.ConfirmUserSysNo;
            else
                paramConfirmUserSysNo.Value = System.DBNull.Value;
            if (oParam.ConfirmTime != AppConst.DateTimeNull)
                paramConfirmTime.Value = oParam.ConfirmTime;
            else
                paramConfirmTime.Value = System.DBNull.Value;
            if (oParam.IncomeUserSysNo != AppConst.IntNull)
                paramIncomeUserSysNo.Value = oParam.IncomeUserSysNo;
            else
                paramIncomeUserSysNo.Value = System.DBNull.Value;
            if (oParam.IncomeTime != AppConst.DateTimeNull)
                paramIncomeTime.Value = oParam.IncomeTime;
            else
                paramIncomeTime.Value = System.DBNull.Value;
            if (oParam.UpdateFreightManUserSysNo != AppConst.IntNull)
                paramUpdateFreightManUserSysNo.Value = oParam.UpdateFreightManUserSysNo;
            else
                paramUpdateFreightManUserSysNo.Value = System.DBNull.Value;
            if (oParam.UpdateFreightManTime != AppConst.DateTimeNull)
                paramUpdateFreightManTime.Value = oParam.UpdateFreightManTime;
            else
                paramUpdateFreightManTime.Value = System.DBNull.Value;
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
            if (oParam.HasPaidQty != AppConst.IntNull)
                paramHasPaidQty.Value = oParam.HasPaidQty;
            else
                paramHasPaidQty.Value = System.DBNull.Value;
            if (oParam.HasPaidAmt != AppConst.DecimalNull)
                paramHasPaidAmt.Value = oParam.HasPaidAmt;
            else
                paramHasPaidAmt.Value = System.DBNull.Value;
            if (oParam.CODQty != AppConst.IntNull)
                paramCODQty.Value = oParam.CODQty;
            else
                paramCODQty.Value = System.DBNull.Value;
            if (oParam.CODAmt != AppConst.DecimalNull)
                paramCODAmt.Value = oParam.CODAmt;
            else
                paramCODAmt.Value = System.DBNull.Value;
            if (oParam.Type != AppConst.IntNull)
                paramType.Value = oParam.Type;
            else
                paramType.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;
            if (oParam.IsSendSMS != AppConst.IntNull)
                paramIsSendSMS.Value = oParam.IsSendSMS;
            else
                paramIsSendSMS.Value = System.DBNull.Value;
            if (oParam.IsAllow != AppConst.IntNull)
                paramIsAllow.Value = oParam.IsAllow;
            else
                paramIsAllow.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramFreightUserSysNo);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramCreateTime);
            cmd.Parameters.Add(paramConfirmUserSysNo);
            cmd.Parameters.Add(paramConfirmTime);
            cmd.Parameters.Add(paramIncomeUserSysNo);
            cmd.Parameters.Add(paramIncomeTime);
            cmd.Parameters.Add(paramUpdateFreightManUserSysNo);
            cmd.Parameters.Add(paramUpdateFreightManTime);
            cmd.Parameters.Add(paramVoucherUserSysNo);
            cmd.Parameters.Add(paramVoucherID);
            cmd.Parameters.Add(paramVoucherTime);
            cmd.Parameters.Add(paramHasPaidQty);
            cmd.Parameters.Add(paramHasPaidAmt);
            cmd.Parameters.Add(paramCODQty);
            cmd.Parameters.Add(paramCODAmt);
            cmd.Parameters.Add(paramType);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramIsSendSMS);
            cmd.Parameters.Add(paramIsAllow);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int Update(DLInfo oParam)
        {
            string sql = @"UPDATE DL_Master SET 
                            FreightUserSysNo=@FreightUserSysNo, CreateUserSysNo=@CreateUserSysNo, 
                            CreateTime=@CreateTime, ConfirmUserSysNo=@ConfirmUserSysNo, 
                            ConfirmTime=@ConfirmTime, IncomeUserSysNo=@IncomeUserSysNo, 
                            IncomeTime=@IncomeTime, UpdateFreightManUserSysNo=@UpdateFreightManUserSysNo, 
                            UpdateFreightManTime=@UpdateFreightManTime, VoucherUserSysNo=@VoucherUserSysNo, 
                            VoucherID=@VoucherID, VoucherTime=@VoucherTime, 
                            HasPaidQty=@HasPaidQty, HasPaidAmt=@HasPaidAmt, 
                            CODQty=@CODQty, CODAmt=@CODAmt, 
                            Type=@Type, Status=@Status, 
                            IsSendSMS=@IsSendSMS, IsAllow=@IsAllow
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramFreightUserSysNo = new SqlParameter("@FreightUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
            SqlParameter paramConfirmUserSysNo = new SqlParameter("@ConfirmUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramConfirmTime = new SqlParameter("@ConfirmTime", SqlDbType.DateTime);
            SqlParameter paramIncomeUserSysNo = new SqlParameter("@IncomeUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramIncomeTime = new SqlParameter("@IncomeTime", SqlDbType.DateTime);
            SqlParameter paramUpdateFreightManUserSysNo = new SqlParameter("@UpdateFreightManUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramUpdateFreightManTime = new SqlParameter("@UpdateFreightManTime", SqlDbType.DateTime);
            SqlParameter paramVoucherUserSysNo = new SqlParameter("@VoucherUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramVoucherID = new SqlParameter("@VoucherID", SqlDbType.NVarChar, 50);
            SqlParameter paramVoucherTime = new SqlParameter("@VoucherTime", SqlDbType.DateTime);
            SqlParameter paramHasPaidQty = new SqlParameter("@HasPaidQty", SqlDbType.Int, 4);
            SqlParameter paramHasPaidAmt = new SqlParameter("@HasPaidAmt", SqlDbType.Decimal, 9);
            SqlParameter paramCODQty = new SqlParameter("@CODQty", SqlDbType.Int, 4);
            SqlParameter paramCODAmt = new SqlParameter("@CODAmt", SqlDbType.Decimal, 9);
            SqlParameter paramType = new SqlParameter("@Type", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramIsSendSMS = new SqlParameter("@IsSendSMS", SqlDbType.Int, 4);
            SqlParameter paramIsAllow = new SqlParameter("@IsAllow", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.FreightUserSysNo != AppConst.IntNull)
                paramFreightUserSysNo.Value = oParam.FreightUserSysNo;
            else
                paramFreightUserSysNo.Value = System.DBNull.Value;
            if (oParam.CreateUserSysNo != AppConst.IntNull)
                paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
            else
                paramCreateUserSysNo.Value = System.DBNull.Value;
            if (oParam.CreateTime != AppConst.DateTimeNull)
                paramCreateTime.Value = oParam.CreateTime;
            else
                paramCreateTime.Value = System.DBNull.Value;
            if (oParam.ConfirmUserSysNo != AppConst.IntNull)
                paramConfirmUserSysNo.Value = oParam.ConfirmUserSysNo;
            else
                paramConfirmUserSysNo.Value = System.DBNull.Value;
            if (oParam.ConfirmTime != AppConst.DateTimeNull)
                paramConfirmTime.Value = oParam.ConfirmTime;
            else
                paramConfirmTime.Value = System.DBNull.Value;
            if (oParam.IncomeUserSysNo != AppConst.IntNull)
                paramIncomeUserSysNo.Value = oParam.IncomeUserSysNo;
            else
                paramIncomeUserSysNo.Value = System.DBNull.Value;
            if (oParam.IncomeTime != AppConst.DateTimeNull)
                paramIncomeTime.Value = oParam.IncomeTime;
            else
                paramIncomeTime.Value = System.DBNull.Value;
            if (oParam.UpdateFreightManUserSysNo != AppConst.IntNull)
                paramUpdateFreightManUserSysNo.Value = oParam.UpdateFreightManUserSysNo;
            else
                paramUpdateFreightManUserSysNo.Value = System.DBNull.Value;
            if (oParam.UpdateFreightManTime != AppConst.DateTimeNull)
                paramUpdateFreightManTime.Value = oParam.UpdateFreightManTime;
            else
                paramUpdateFreightManTime.Value = System.DBNull.Value;
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
            if (oParam.HasPaidQty != AppConst.IntNull)
                paramHasPaidQty.Value = oParam.HasPaidQty;
            else
                paramHasPaidQty.Value = System.DBNull.Value;
            if (oParam.HasPaidAmt != AppConst.DecimalNull)
                paramHasPaidAmt.Value = oParam.HasPaidAmt;
            else
                paramHasPaidAmt.Value = System.DBNull.Value;
            if (oParam.CODQty != AppConst.IntNull)
                paramCODQty.Value = oParam.CODQty;
            else
                paramCODQty.Value = System.DBNull.Value;
            if (oParam.CODAmt != AppConst.DecimalNull)
                paramCODAmt.Value = oParam.CODAmt;
            else
                paramCODAmt.Value = System.DBNull.Value;
            if (oParam.Type != AppConst.IntNull)
                paramType.Value = oParam.Type;
            else
                paramType.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;
            if (oParam.IsSendSMS != AppConst.IntNull)
                paramIsSendSMS.Value = oParam.IsSendSMS;
            else
                paramIsSendSMS.Value = System.DBNull.Value;
            if (oParam.IsAllow != AppConst.IntNull)
                paramIsAllow.Value = oParam.IsAllow;
            else
                paramIsAllow.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramFreightUserSysNo);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramCreateTime);
            cmd.Parameters.Add(paramConfirmUserSysNo);
            cmd.Parameters.Add(paramConfirmTime);
            cmd.Parameters.Add(paramIncomeUserSysNo);
            cmd.Parameters.Add(paramIncomeTime);
            cmd.Parameters.Add(paramUpdateFreightManUserSysNo);
            cmd.Parameters.Add(paramUpdateFreightManTime);
            cmd.Parameters.Add(paramVoucherUserSysNo);
            cmd.Parameters.Add(paramVoucherID);
            cmd.Parameters.Add(paramVoucherTime);
            cmd.Parameters.Add(paramHasPaidQty);
            cmd.Parameters.Add(paramHasPaidAmt);
            cmd.Parameters.Add(paramCODQty);
            cmd.Parameters.Add(paramCODAmt);
            cmd.Parameters.Add(paramType);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramIsSendSMS);
            cmd.Parameters.Add(paramIsAllow);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int Insert(DLItemInfo oParam)
        {
            string sql = @"INSERT INTO DL_Item
                            (
                            DLSysNo, ItemID, ItemType, PayType, 
                            PayAmt, Status
                            )
                            VALUES (
                            @DLSysNo, @ItemID, @ItemType, @PayType, 
                            @PayAmt, @Status
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramDLSysNo = new SqlParameter("@DLSysNo", SqlDbType.Int, 4);
            SqlParameter paramItemID = new SqlParameter("@ItemID", SqlDbType.NVarChar, 50);
            SqlParameter paramItemType = new SqlParameter("@ItemType", SqlDbType.Int, 4);
            SqlParameter paramPayType = new SqlParameter("@PayType", SqlDbType.Int, 4);
            SqlParameter paramPayAmt = new SqlParameter("@PayAmt", SqlDbType.Decimal, 9);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.DLSysNo != AppConst.IntNull)
                paramDLSysNo.Value = oParam.DLSysNo;
            else
                paramDLSysNo.Value = System.DBNull.Value;
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

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramDLSysNo);
            cmd.Parameters.Add(paramItemID);
            cmd.Parameters.Add(paramItemType);
            cmd.Parameters.Add(paramPayType);
            cmd.Parameters.Add(paramPayAmt);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Update(DLItemInfo oParam)
        {
            string sql = @"UPDATE DL_Item SET 
                            DLSysNo=@DLSysNo, ItemID=@ItemID, 
                            ItemType=@ItemType, PayType=@PayType, 
                            PayAmt=@PayAmt, Status=@Status
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramDLSysNo = new SqlParameter("@DLSysNo", SqlDbType.Int, 4);
            SqlParameter paramItemID = new SqlParameter("@ItemID", SqlDbType.NVarChar, 50);
            SqlParameter paramItemType = new SqlParameter("@ItemType", SqlDbType.Int, 4);
            SqlParameter paramPayType = new SqlParameter("@PayType", SqlDbType.Int, 4);
            SqlParameter paramPayAmt = new SqlParameter("@PayAmt", SqlDbType.Decimal, 9);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.DLSysNo != AppConst.IntNull)
                paramDLSysNo.Value = oParam.DLSysNo;
            else
                paramDLSysNo.Value = System.DBNull.Value;
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

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramDLSysNo);
            cmd.Parameters.Add(paramItemID);
            cmd.Parameters.Add(paramItemType);
            cmd.Parameters.Add(paramPayType);
            cmd.Parameters.Add(paramPayAmt);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int UpdateMaster(Hashtable paramHash)
        {
            StringBuilder sb = new StringBuilder(200);
            sb.Append("UPDATE DL_Master SET ");

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