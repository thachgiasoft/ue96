using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Icson.Objects.Review;
using Icson.Utils;

namespace Icson.DBAccess.Review
{
    public class ReviewReplyDac
    {
        public int Insert(ReviewReplyInfo oParam)
        {
            string sql = @"INSERT INTO Review_Reply
                            (
                            ReviewSysNo, ReplyContent, Status, CreateUserType, 
                            CreateUserSysNo, CreateDate, LastEditUserSysNo, LastEditDate
                            )
                            VALUES (
                            @ReviewSysNo, @ReplyContent, @Status, @CreateUserType, 
                            @CreateUserSysNo, @CreateDate, @LastEditUserSysNo, @LastEditDate
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramReviewSysNo = new SqlParameter("@ReviewSysNo", SqlDbType.Int, 4);
            SqlParameter paramReplyContent = new SqlParameter("@ReplyContent", SqlDbType.NVarChar, 1000);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramCreateUserType = new SqlParameter("@CreateUserType", SqlDbType.Int, 4);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateDate = new SqlParameter("@CreateDate", SqlDbType.DateTime);
            SqlParameter paramLastEditUserSysNo = new SqlParameter("@LastEditUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramLastEditDate = new SqlParameter("@LastEditDate", SqlDbType.DateTime);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.ReviewSysNo != AppConst.IntNull)
                paramReviewSysNo.Value = oParam.ReviewSysNo;
            else
                paramReviewSysNo.Value = System.DBNull.Value;
            if (oParam.ReplyContent != AppConst.StringNull)
                paramReplyContent.Value = oParam.ReplyContent;
            else
                paramReplyContent.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;
            if (oParam.CreateUserType != AppConst.IntNull)
                paramCreateUserType.Value = oParam.CreateUserType;
            else
                paramCreateUserType.Value = System.DBNull.Value;
            if (oParam.CreateUserSysNo != AppConst.IntNull)
                paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
            else
                paramCreateUserSysNo.Value = System.DBNull.Value;
            if (oParam.CreateDate != AppConst.DateTimeNull)
                paramCreateDate.Value = oParam.CreateDate;
            else
                paramCreateDate.Value = System.DBNull.Value;
            if (oParam.LastEditUserSysNo != AppConst.IntNull)
                paramLastEditUserSysNo.Value = oParam.LastEditUserSysNo;
            else
                paramLastEditUserSysNo.Value = System.DBNull.Value;
            if (oParam.LastEditDate != AppConst.DateTimeNull)
                paramLastEditDate.Value = oParam.LastEditDate;
            else
                paramLastEditDate.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramReviewSysNo);
            cmd.Parameters.Add(paramReplyContent);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramCreateUserType);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramCreateDate);
            cmd.Parameters.Add(paramLastEditUserSysNo);
            cmd.Parameters.Add(paramLastEditDate);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Update(ReviewReplyInfo oParam)
        {
            string sql = @"UPDATE Review_Reply SET 
                            ReviewSysNo=@ReviewSysNo, ReplyContent=@ReplyContent, 
                            Status=@Status, CreateUserType=@CreateUserType, 
                            CreateUserSysNo=@CreateUserSysNo, CreateDate=@CreateDate, 
                            LastEditUserSysNo=@LastEditUserSysNo, LastEditDate=@LastEditDate
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramReviewSysNo = new SqlParameter("@ReviewSysNo", SqlDbType.Int, 4);
            SqlParameter paramReplyContent = new SqlParameter("@ReplyContent", SqlDbType.NVarChar, 1000);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramCreateUserType = new SqlParameter("@CreateUserType", SqlDbType.Int, 4);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateDate = new SqlParameter("@CreateDate", SqlDbType.DateTime);
            SqlParameter paramLastEditUserSysNo = new SqlParameter("@LastEditUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramLastEditDate = new SqlParameter("@LastEditDate", SqlDbType.DateTime);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.ReviewSysNo != AppConst.IntNull)
                paramReviewSysNo.Value = oParam.ReviewSysNo;
            else
                paramReviewSysNo.Value = System.DBNull.Value;
            if (oParam.ReplyContent != AppConst.StringNull)
                paramReplyContent.Value = oParam.ReplyContent;
            else
                paramReplyContent.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;
            if (oParam.CreateUserType != AppConst.IntNull)
                paramCreateUserType.Value = oParam.CreateUserType;
            else
                paramCreateUserType.Value = System.DBNull.Value;
            if (oParam.CreateUserSysNo != AppConst.IntNull)
                paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
            else
                paramCreateUserSysNo.Value = System.DBNull.Value;
            if (oParam.CreateDate != AppConst.DateTimeNull)
                paramCreateDate.Value = oParam.CreateDate;
            else
                paramCreateDate.Value = System.DBNull.Value;
            if (oParam.LastEditUserSysNo != AppConst.IntNull)
                paramLastEditUserSysNo.Value = oParam.LastEditUserSysNo;
            else
                paramLastEditUserSysNo.Value = System.DBNull.Value;
            if (oParam.LastEditDate != AppConst.DateTimeNull)
                paramLastEditDate.Value = oParam.LastEditDate;
            else
                paramLastEditDate.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramReviewSysNo);
            cmd.Parameters.Add(paramReplyContent);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramCreateUserType);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramCreateDate);
            cmd.Parameters.Add(paramLastEditUserSysNo);
            cmd.Parameters.Add(paramLastEditDate);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
    }
}
