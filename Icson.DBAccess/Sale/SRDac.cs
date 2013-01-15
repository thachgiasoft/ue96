using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Icson.Objects.Sale;
using Icson.Utils;
namespace Icson.DBAccess.Sale
{
    public class SRDac
    {
        public int InsertMaster(SRInfo oParam)
        {
            string sql = @"INSERT INTO SR_Master
                            (
                            SysNo, SRID, SOSysNo, Status, 
                            ReturnType, StockSysNo, CreateUserSysNo, CreateTime, 
                            ReceiveUserSysNo, ReceiveTime, InstockTime, InstockUserSysNo, 
                            ShelveTime, ShelveUserSysNo, UpdateUserSysNo, UpdateTime, 
                            Note
                            )
                            VALUES (
                            @SysNo, @SRID, @SOSysNo, @Status, 
                            @ReturnType, @StockSysNo, @CreateUserSysNo, @CreateTime, 
                            @ReceiveUserSysNo, @ReceiveTime, @InstockTime, @InstockUserSysNo, 
                            @ShelveTime, @ShelveUserSysNo, @UpdateUserSysNo, @UpdateTime, 
                            @Note
                            )";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramSRID = new SqlParameter("@SRID", SqlDbType.Char, 10);
            SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramReturnType = new SqlParameter("@ReturnType", SqlDbType.Int, 4);
            SqlParameter paramStockSysNo = new SqlParameter("@StockSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
            SqlParameter paramReceiveUserSysNo = new SqlParameter("@ReceiveUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramReceiveTime = new SqlParameter("@ReceiveTime", SqlDbType.DateTime);
            SqlParameter paramInstockTime = new SqlParameter("@InstockTime", SqlDbType.DateTime);
            SqlParameter paramInstockUserSysNo = new SqlParameter("@InstockUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramShelveTime = new SqlParameter("@ShelveTime", SqlDbType.DateTime);
            SqlParameter paramShelveUserSysNo = new SqlParameter("@ShelveUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramUpdateUserSysNo = new SqlParameter("@UpdateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramUpdateTime = new SqlParameter("@UpdateTime", SqlDbType.DateTime);
            SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.NVarChar, 500);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.SRID != AppConst.StringNull)
                paramSRID.Value = oParam.SRID;
            else
                paramSRID.Value = System.DBNull.Value;
            if (oParam.SOSysNo != AppConst.IntNull)
                paramSOSysNo.Value = oParam.SOSysNo;
            else
                paramSOSysNo.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;
            if (oParam.ReturnType != AppConst.IntNull)
                paramReturnType.Value = oParam.ReturnType;
            else
                paramReturnType.Value = System.DBNull.Value;
            if (oParam.StockSysNo != AppConst.IntNull)
                paramStockSysNo.Value = oParam.StockSysNo;
            else
                paramStockSysNo.Value = System.DBNull.Value;
            if (oParam.CreateUserSysNo != AppConst.IntNull)
                paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
            else
                paramCreateUserSysNo.Value = System.DBNull.Value;
            if (oParam.CreateTime != AppConst.DateTimeNull)
                paramCreateTime.Value = oParam.CreateTime;
            else
                paramCreateTime.Value = System.DBNull.Value;
            if (oParam.ReceiveUserSysNo != AppConst.IntNull)
                paramReceiveUserSysNo.Value = oParam.ReceiveUserSysNo;
            else
                paramReceiveUserSysNo.Value = System.DBNull.Value;
            if (oParam.ReceiveTime != AppConst.DateTimeNull)
                paramReceiveTime.Value = oParam.ReceiveTime;
            else
                paramReceiveTime.Value = System.DBNull.Value;
            if (oParam.InstockTime != AppConst.DateTimeNull)
                paramInstockTime.Value = oParam.InstockTime;
            else
                paramInstockTime.Value = System.DBNull.Value;
            if (oParam.InstockUserSysNo != AppConst.IntNull)
                paramInstockUserSysNo.Value = oParam.InstockUserSysNo;
            else
                paramInstockUserSysNo.Value = System.DBNull.Value;
            if (oParam.ShelveTime != AppConst.DateTimeNull)
                paramShelveTime.Value = oParam.ShelveTime;
            else
                paramShelveTime.Value = System.DBNull.Value;
            if (oParam.ShelveUserSysNo != AppConst.IntNull)
                paramShelveUserSysNo.Value = oParam.ShelveUserSysNo;
            else
                paramShelveUserSysNo.Value = System.DBNull.Value;
            if (oParam.UpdateUserSysNo != AppConst.IntNull)
                paramUpdateUserSysNo.Value = oParam.UpdateUserSysNo;
            else
                paramUpdateUserSysNo.Value = System.DBNull.Value;
            if (oParam.UpdateTime != AppConst.DateTimeNull)
                paramUpdateTime.Value = oParam.UpdateTime;
            else
                paramUpdateTime.Value = System.DBNull.Value;
            if (oParam.Note != AppConst.StringNull)
                paramNote.Value = oParam.Note;
            else
                paramNote.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramSRID);
            cmd.Parameters.Add(paramSOSysNo);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramReturnType);
            cmd.Parameters.Add(paramStockSysNo);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramCreateTime);
            cmd.Parameters.Add(paramReceiveUserSysNo);
            cmd.Parameters.Add(paramReceiveTime);
            cmd.Parameters.Add(paramInstockTime);
            cmd.Parameters.Add(paramInstockUserSysNo);
            cmd.Parameters.Add(paramShelveTime);
            cmd.Parameters.Add(paramShelveUserSysNo);
            cmd.Parameters.Add(paramUpdateUserSysNo);
            cmd.Parameters.Add(paramUpdateTime);
            cmd.Parameters.Add(paramNote);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
        public int InsertItem(SRItemInfo oParam)
        {
            string sql = @"INSERT INTO SR_Item
                            (
                            SRSysNo, ProductSysNo, Quantity
                            )
                            VALUES (
                            @SRSysNo, @ProductSysNo, @Quantity
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramSRSysNo = new SqlParameter("@SRSysNo", SqlDbType.Int, 4);
            SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int, 4);
            SqlParameter paramQuantity = new SqlParameter("@Quantity", SqlDbType.Int, 4);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.SRSysNo != AppConst.IntNull)
                paramSRSysNo.Value = oParam.SRSysNo;
            else
                paramSRSysNo.Value = System.DBNull.Value;
            if (oParam.ProductSysNo != AppConst.IntNull)
                paramProductSysNo.Value = oParam.ProductSysNo;
            else
                paramProductSysNo.Value = System.DBNull.Value;
            if (oParam.Quantity != AppConst.IntNull)
                paramQuantity.Value = oParam.Quantity;
            else
                paramQuantity.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramSRSysNo);
            cmd.Parameters.Add(paramProductSysNo);
            cmd.Parameters.Add(paramQuantity);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int UpdateMaster(SRInfo oParam)
        {
            string sql = @"UPDATE SR_Master SET 
                            SRID=@SRID, SOSysNo=@SOSysNo, 
                            Status=@Status, ReturnType=@ReturnType, 
                            StockSysNo=@StockSysNo, CreateUserSysNo=@CreateUserSysNo, 
                            CreateTime=@CreateTime, ReceiveUserSysNo=@ReceiveUserSysNo, 
                            ReceiveTime=@ReceiveTime, InstockTime=@InstockTime, 
                            InstockUserSysNo=@InstockUserSysNo, ShelveTime=@ShelveTime, 
                            ShelveUserSysNo=@ShelveUserSysNo, UpdateUserSysNo=@UpdateUserSysNo, 
                            UpdateTime=@UpdateTime, Note=@Note
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramSRID = new SqlParameter("@SRID", SqlDbType.Char, 10);
            SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramReturnType = new SqlParameter("@ReturnType", SqlDbType.Int, 4);
            SqlParameter paramStockSysNo = new SqlParameter("@StockSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
            SqlParameter paramReceiveUserSysNo = new SqlParameter("@ReceiveUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramReceiveTime = new SqlParameter("@ReceiveTime", SqlDbType.DateTime);
            SqlParameter paramInstockTime = new SqlParameter("@InstockTime", SqlDbType.DateTime);
            SqlParameter paramInstockUserSysNo = new SqlParameter("@InstockUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramShelveTime = new SqlParameter("@ShelveTime", SqlDbType.DateTime);
            SqlParameter paramShelveUserSysNo = new SqlParameter("@ShelveUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramUpdateUserSysNo = new SqlParameter("@UpdateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramUpdateTime = new SqlParameter("@UpdateTime", SqlDbType.DateTime);
            SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.NVarChar, 500);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.SRID != AppConst.StringNull)
                paramSRID.Value = oParam.SRID;
            else
                paramSRID.Value = System.DBNull.Value;
            if (oParam.SOSysNo != AppConst.IntNull)
                paramSOSysNo.Value = oParam.SOSysNo;
            else
                paramSOSysNo.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;
            if (oParam.ReturnType != AppConst.IntNull)
                paramReturnType.Value = oParam.ReturnType;
            else
                paramReturnType.Value = System.DBNull.Value;
            if (oParam.StockSysNo != AppConst.IntNull)
                paramStockSysNo.Value = oParam.StockSysNo;
            else
                paramStockSysNo.Value = System.DBNull.Value;
            if (oParam.CreateUserSysNo != AppConst.IntNull)
                paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
            else
                paramCreateUserSysNo.Value = System.DBNull.Value;
            if (oParam.CreateTime != AppConst.DateTimeNull)
                paramCreateTime.Value = oParam.CreateTime;
            else
                paramCreateTime.Value = System.DBNull.Value;
            if (oParam.ReceiveUserSysNo != AppConst.IntNull)
                paramReceiveUserSysNo.Value = oParam.ReceiveUserSysNo;
            else
                paramReceiveUserSysNo.Value = System.DBNull.Value;
            if (oParam.ReceiveTime != AppConst.DateTimeNull)
                paramReceiveTime.Value = oParam.ReceiveTime;
            else
                paramReceiveTime.Value = System.DBNull.Value;
            if (oParam.InstockTime != AppConst.DateTimeNull)
                paramInstockTime.Value = oParam.InstockTime;
            else
                paramInstockTime.Value = System.DBNull.Value;
            if (oParam.InstockUserSysNo != AppConst.IntNull)
                paramInstockUserSysNo.Value = oParam.InstockUserSysNo;
            else
                paramInstockUserSysNo.Value = System.DBNull.Value;
            if (oParam.ShelveTime != AppConst.DateTimeNull)
                paramShelveTime.Value = oParam.ShelveTime;
            else
                paramShelveTime.Value = System.DBNull.Value;
            if (oParam.ShelveUserSysNo != AppConst.IntNull)
                paramShelveUserSysNo.Value = oParam.ShelveUserSysNo;
            else
                paramShelveUserSysNo.Value = System.DBNull.Value;
            if (oParam.UpdateUserSysNo != AppConst.IntNull)
                paramUpdateUserSysNo.Value = oParam.UpdateUserSysNo;
            else
                paramUpdateUserSysNo.Value = System.DBNull.Value;
            if (oParam.UpdateTime != AppConst.DateTimeNull)
                paramUpdateTime.Value = oParam.UpdateTime;
            else
                paramUpdateTime.Value = System.DBNull.Value;
            if (oParam.Note != AppConst.StringNull)
                paramNote.Value = oParam.Note;
            else
                paramNote.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramSRID);
            cmd.Parameters.Add(paramSOSysNo);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramReturnType);
            cmd.Parameters.Add(paramStockSysNo);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramCreateTime);
            cmd.Parameters.Add(paramReceiveUserSysNo);
            cmd.Parameters.Add(paramReceiveTime);
            cmd.Parameters.Add(paramInstockTime);
            cmd.Parameters.Add(paramInstockUserSysNo);
            cmd.Parameters.Add(paramShelveTime);
            cmd.Parameters.Add(paramShelveUserSysNo);
            cmd.Parameters.Add(paramUpdateUserSysNo);
            cmd.Parameters.Add(paramUpdateTime);
            cmd.Parameters.Add(paramNote);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
        public int UpdateItem(SRItemInfo oParam)
        {
            string sql = @"UPDATE SR_Item SET 
                            SRSysNo=@SRSysNo, ProductSysNo=@ProductSysNo, 
                            Quantity=@Quantity
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramSRSysNo = new SqlParameter("@SRSysNo", SqlDbType.Int, 4);
            SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int, 4);
            SqlParameter paramQuantity = new SqlParameter("@Quantity", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.SRSysNo != AppConst.IntNull)
                paramSRSysNo.Value = oParam.SRSysNo;
            else
                paramSRSysNo.Value = System.DBNull.Value;
            if (oParam.ProductSysNo != AppConst.IntNull)
                paramProductSysNo.Value = oParam.ProductSysNo;
            else
                paramProductSysNo.Value = System.DBNull.Value;
            if (oParam.Quantity != AppConst.IntNull)
                paramQuantity.Value = oParam.Quantity;
            else
                paramQuantity.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramSRSysNo);
            cmd.Parameters.Add(paramProductSysNo);
            cmd.Parameters.Add(paramQuantity);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
    }
}
