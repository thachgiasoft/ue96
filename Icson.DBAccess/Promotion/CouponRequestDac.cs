using System;
using System.Collections.Generic;
using System.Text;
using Icson.Objects;
using Icson.Utils;
using Icson.Objects.Promotion;
using System.Data;
using System.Data.SqlClient;

namespace Icson.DBAccess.Promotion
{
    public class CouponRequestDac
    {
        public CouponRequestDac()
        {

        }

        public int Insert(CouponRequestInfo oParam)
        {
            string sql = @"INSERT INTO CouponRequest
                            (
                            CustomerSysNo, CouponCode, RequestUserSysNo, RequestTime, 
                            AuditUserSysNo, AuditTime, SOSysNo, BatchNo, 
                            Note, Status, EXECSql
                            )
                            VALUES (
                            @CustomerSysNo, @CouponCode, @RequestUserSysNo, @RequestTime, 
                            @AuditUserSysNo, @AuditTime, @SOSysNo, @BatchNo, 
                            @Note, @Status, @EXECSql
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramCustomerSysNo = new SqlParameter("@CustomerSysNo", SqlDbType.Int, 4);
            SqlParameter paramCouponCode = new SqlParameter("@CouponCode", SqlDbType.NVarChar, 20);
            SqlParameter paramRequestUserSysNo = new SqlParameter("@RequestUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramRequestTime = new SqlParameter("@RequestTime", SqlDbType.DateTime);
            SqlParameter paramAuditUserSysNo = new SqlParameter("@AuditUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramAuditTime = new SqlParameter("@AuditTime", SqlDbType.DateTime);
            SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int, 4);
            SqlParameter paramBatchNo = new SqlParameter("@BatchNo", SqlDbType.Int, 4);
            SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.NVarChar, 200);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramEXECSql = new SqlParameter("@EXECSql", SqlDbType.Text, 0);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.CustomerSysNo != AppConst.IntNull)
                paramCustomerSysNo.Value = oParam.CustomerSysNo;
            else
                paramCustomerSysNo.Value = System.DBNull.Value;
            if (oParam.CouponCode != AppConst.StringNull)
                paramCouponCode.Value = oParam.CouponCode;
            else
                paramCouponCode.Value = System.DBNull.Value;
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
            if (oParam.SOSysNo != AppConst.IntNull)
                paramSOSysNo.Value = oParam.SOSysNo;
            else
                paramSOSysNo.Value = System.DBNull.Value;
            if (oParam.BatchNo != AppConst.IntNull)
                paramBatchNo.Value = oParam.BatchNo;
            else
                paramBatchNo.Value = System.DBNull.Value;
            if (oParam.Note != AppConst.StringNull)
                paramNote.Value = oParam.Note;
            else
                paramNote.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;
            if (oParam.EXECSql != AppConst.StringNull)
                paramEXECSql.Value = oParam.EXECSql;
            else
                paramEXECSql.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramCustomerSysNo);
            cmd.Parameters.Add(paramCouponCode);
            cmd.Parameters.Add(paramRequestUserSysNo);
            cmd.Parameters.Add(paramRequestTime);
            cmd.Parameters.Add(paramAuditUserSysNo);
            cmd.Parameters.Add(paramAuditTime);
            cmd.Parameters.Add(paramSOSysNo);
            cmd.Parameters.Add(paramBatchNo);
            cmd.Parameters.Add(paramNote);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramEXECSql);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Update(CouponRequestInfo oParam)
        {
            string sql = @"UPDATE CouponRequest SET 
                            CustomerSysNo=@CustomerSysNo, CouponCode=@CouponCode, 
                            RequestUserSysNo=@RequestUserSysNo, RequestTime=@RequestTime, 
                            AuditUserSysNo=@AuditUserSysNo, AuditTime=@AuditTime, 
                            SOSysNo=@SOSysNo, BatchNo=@BatchNo, 
                            Note=@Note, Status=@Status, 
                            EXECSql=@EXECSql
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramCustomerSysNo = new SqlParameter("@CustomerSysNo", SqlDbType.Int, 4);
            SqlParameter paramCouponCode = new SqlParameter("@CouponCode", SqlDbType.NVarChar, 20);
            SqlParameter paramRequestUserSysNo = new SqlParameter("@RequestUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramRequestTime = new SqlParameter("@RequestTime", SqlDbType.DateTime);
            SqlParameter paramAuditUserSysNo = new SqlParameter("@AuditUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramAuditTime = new SqlParameter("@AuditTime", SqlDbType.DateTime);
            SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int, 4);
            SqlParameter paramBatchNo = new SqlParameter("@BatchNo", SqlDbType.Int, 4);
            SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.NVarChar, 200);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramEXECSql = new SqlParameter("@EXECSql", SqlDbType.Text, 0);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.CustomerSysNo != AppConst.IntNull)
                paramCustomerSysNo.Value = oParam.CustomerSysNo;
            else
                paramCustomerSysNo.Value = System.DBNull.Value;
            if (oParam.CouponCode != AppConst.StringNull)
                paramCouponCode.Value = oParam.CouponCode;
            else
                paramCouponCode.Value = System.DBNull.Value;
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
            if (oParam.SOSysNo != AppConst.IntNull)
                paramSOSysNo.Value = oParam.SOSysNo;
            else
                paramSOSysNo.Value = System.DBNull.Value;
            if (oParam.BatchNo != AppConst.IntNull)
                paramBatchNo.Value = oParam.BatchNo;
            else
                paramBatchNo.Value = System.DBNull.Value;
            if (oParam.Note != AppConst.StringNull)
                paramNote.Value = oParam.Note;
            else
                paramNote.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;
            if (oParam.EXECSql != AppConst.StringNull)
                paramEXECSql.Value = oParam.EXECSql;
            else
                paramEXECSql.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramCustomerSysNo);
            cmd.Parameters.Add(paramCouponCode);
            cmd.Parameters.Add(paramRequestUserSysNo);
            cmd.Parameters.Add(paramRequestTime);
            cmd.Parameters.Add(paramAuditUserSysNo);
            cmd.Parameters.Add(paramAuditTime);
            cmd.Parameters.Add(paramSOSysNo);
            cmd.Parameters.Add(paramBatchNo);
            cmd.Parameters.Add(paramNote);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramEXECSql);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

    }
}
