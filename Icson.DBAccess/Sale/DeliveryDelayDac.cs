using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects.Sale;

namespace Icson.DBAccess.Sale
{
    public class DeliveryDelayDac
    {
        public int Insert(DeliveryDelayListInfo oParam)
        {
            string sql = @"INSERT INTO DeliveryDelayList
                            (
                            CauseType, BillID, FreightUserSysNo, SetDeliveryManTime, 
                            UpdateUserSysNo, UpdateTime, ReviewBackNote, ReviewCauseType, 
                            ReviewBackUserSysNo, ReviewBackTime
                            )
                            VALUES (
                            @CauseType, @BillID, @FreightUserSysNo, @SetDeliveryManTime, 
                            @UpdateUserSysNo, @UpdateTime, @ReviewBackNote, @ReviewCauseType, 
                            @ReviewBackUserSysNo, @ReviewBackTime
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramCauseType = new SqlParameter("@CauseType", SqlDbType.Int, 4);
            SqlParameter paramBillID = new SqlParameter("@BillID", SqlDbType.NVarChar, 50);
            SqlParameter paramFreightUserSysNo = new SqlParameter("@FreightUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramSetDeliveryManTime = new SqlParameter("@SetDeliveryManTime", SqlDbType.DateTime);
            SqlParameter paramUpdateUserSysNo = new SqlParameter("@UpdateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramUpdateTime = new SqlParameter("@UpdateTime", SqlDbType.DateTime);
            SqlParameter paramReviewBackNote = new SqlParameter("@ReviewBackNote", SqlDbType.NVarChar, 500);
            SqlParameter paramReviewCauseType = new SqlParameter("@ReviewCauseType", SqlDbType.Int, 4);
            SqlParameter paramReviewBackUserSysNo = new SqlParameter("@ReviewBackUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramReviewBackTime = new SqlParameter("@ReviewBackTime", SqlDbType.DateTime);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.CauseType != AppConst.IntNull)
                paramCauseType.Value = oParam.CauseType;
            else
                paramCauseType.Value = System.DBNull.Value;
            if (oParam.BillID != AppConst.StringNull)
                paramBillID.Value = oParam.BillID;
            else
                paramBillID.Value = System.DBNull.Value;
            if (oParam.FreightUserSysNo != AppConst.IntNull)
                paramFreightUserSysNo.Value = oParam.FreightUserSysNo;
            else
                paramFreightUserSysNo.Value = System.DBNull.Value;
            if (oParam.SetDeliveryManTime != AppConst.DateTimeNull)
                paramSetDeliveryManTime.Value = oParam.SetDeliveryManTime;
            else
                paramSetDeliveryManTime.Value = System.DBNull.Value;
            if (oParam.UpdateUserSysNo != AppConst.IntNull)
                paramUpdateUserSysNo.Value = oParam.UpdateUserSysNo;
            else
                paramUpdateUserSysNo.Value = System.DBNull.Value;
            if (oParam.UpdateTime != AppConst.DateTimeNull)
                paramUpdateTime.Value = oParam.UpdateTime;
            else
                paramUpdateTime.Value = System.DBNull.Value;
            if (oParam.ReviewBackNote != AppConst.StringNull)
                paramReviewBackNote.Value = oParam.ReviewBackNote;
            else
                paramReviewBackNote.Value = System.DBNull.Value;
            if (oParam.ReviewCauseType != AppConst.IntNull)
                paramReviewCauseType.Value = oParam.ReviewCauseType;
            else
                paramReviewCauseType.Value = System.DBNull.Value;
            if (oParam.ReviewBackUserSysNo != AppConst.IntNull)
                paramReviewBackUserSysNo.Value = oParam.ReviewBackUserSysNo;
            else
                paramReviewBackUserSysNo.Value = System.DBNull.Value;
            if (oParam.ReviewBackTime != AppConst.DateTimeNull)
                paramReviewBackTime.Value = oParam.ReviewBackTime;
            else
                paramReviewBackTime.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramCauseType);
            cmd.Parameters.Add(paramBillID);
            cmd.Parameters.Add(paramFreightUserSysNo);
            cmd.Parameters.Add(paramSetDeliveryManTime);
            cmd.Parameters.Add(paramUpdateUserSysNo);
            cmd.Parameters.Add(paramUpdateTime);
            cmd.Parameters.Add(paramReviewBackNote);
            cmd.Parameters.Add(paramReviewCauseType);
            cmd.Parameters.Add(paramReviewBackUserSysNo);
            cmd.Parameters.Add(paramReviewBackTime);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }
        public int Update(DeliveryDelayListInfo oParam)
        {
            string sql = @"UPDATE DeliveryDelayList SET 
                            CauseType=@CauseType, BillID=@BillID, 
                            FreightUserSysNo=@FreightUserSysNo, SetDeliveryManTime=@SetDeliveryManTime, 
                            UpdateUserSysNo=@UpdateUserSysNo, UpdateTime=@UpdateTime, 
                            ReviewBackNote=@ReviewBackNote, ReviewCauseType=@ReviewCauseType, 
                            ReviewBackUserSysNo=@ReviewBackUserSysNo, ReviewBackTime=@ReviewBackTime
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramCauseType = new SqlParameter("@CauseType", SqlDbType.Int, 4);
            SqlParameter paramBillID = new SqlParameter("@BillID", SqlDbType.NVarChar, 50);
            SqlParameter paramFreightUserSysNo = new SqlParameter("@FreightUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramSetDeliveryManTime = new SqlParameter("@SetDeliveryManTime", SqlDbType.DateTime);
            SqlParameter paramUpdateUserSysNo = new SqlParameter("@UpdateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramUpdateTime = new SqlParameter("@UpdateTime", SqlDbType.DateTime);
            SqlParameter paramReviewBackNote = new SqlParameter("@ReviewBackNote", SqlDbType.NVarChar, 500);
            SqlParameter paramReviewCauseType = new SqlParameter("@ReviewCauseType", SqlDbType.Int, 4);
            SqlParameter paramReviewBackUserSysNo = new SqlParameter("@ReviewBackUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramReviewBackTime = new SqlParameter("@ReviewBackTime", SqlDbType.DateTime);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.CauseType != AppConst.IntNull)
                paramCauseType.Value = oParam.CauseType;
            else
                paramCauseType.Value = System.DBNull.Value;
            if (oParam.BillID != AppConst.StringNull)
                paramBillID.Value = oParam.BillID;
            else
                paramBillID.Value = System.DBNull.Value;
            if (oParam.FreightUserSysNo != AppConst.IntNull)
                paramFreightUserSysNo.Value = oParam.FreightUserSysNo;
            else
                paramFreightUserSysNo.Value = System.DBNull.Value;
            if (oParam.SetDeliveryManTime != AppConst.DateTimeNull)
                paramSetDeliveryManTime.Value = oParam.SetDeliveryManTime;
            else
                paramSetDeliveryManTime.Value = System.DBNull.Value;
            if (oParam.UpdateUserSysNo != AppConst.IntNull)
                paramUpdateUserSysNo.Value = oParam.UpdateUserSysNo;
            else
                paramUpdateUserSysNo.Value = System.DBNull.Value;
            if (oParam.UpdateTime != AppConst.DateTimeNull)
                paramUpdateTime.Value = oParam.UpdateTime;
            else
                paramUpdateTime.Value = System.DBNull.Value;
            if (oParam.ReviewBackNote != AppConst.StringNull)
                paramReviewBackNote.Value = oParam.ReviewBackNote;
            else
                paramReviewBackNote.Value = System.DBNull.Value;
            if (oParam.ReviewCauseType != AppConst.IntNull)
                paramReviewCauseType.Value = oParam.ReviewCauseType;
            else
                paramReviewCauseType.Value = System.DBNull.Value;
            if (oParam.ReviewBackUserSysNo != AppConst.IntNull)
                paramReviewBackUserSysNo.Value = oParam.ReviewBackUserSysNo;
            else
                paramReviewBackUserSysNo.Value = System.DBNull.Value;
            if (oParam.ReviewBackTime != AppConst.DateTimeNull)
                paramReviewBackTime.Value = oParam.ReviewBackTime;
            else
                paramReviewBackTime.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramCauseType);
            cmd.Parameters.Add(paramBillID);
            cmd.Parameters.Add(paramFreightUserSysNo);
            cmd.Parameters.Add(paramSetDeliveryManTime);
            cmd.Parameters.Add(paramUpdateUserSysNo);
            cmd.Parameters.Add(paramUpdateTime);
            cmd.Parameters.Add(paramReviewBackNote);
            cmd.Parameters.Add(paramReviewCauseType);
            cmd.Parameters.Add(paramReviewBackUserSysNo);
            cmd.Parameters.Add(paramReviewBackTime);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
    }
}
