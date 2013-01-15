using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Icson.Objects.Review;
using Icson.Utils;

namespace Icson.DBAccess.Review
{
    public class ReviewRemarkDac
    {
        public int Insert(ReviewRemarkInfo oParam)
        {
            string sql = @"INSERT INTO Review_Remark
                            (
                            ReviewSysNo, IsHelpful, CreateUserType, CreateUserSysNo, 
                            CreateDate
                            )
                            VALUES (
                            @ReviewSysNo, @IsHelpful, @CreateUserType, @CreateUserSysNo, 
                            @CreateDate
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramReviewSysNo = new SqlParameter("@ReviewSysNo", SqlDbType.Int, 4);
            SqlParameter paramIsHelpful = new SqlParameter("@IsHelpful", SqlDbType.Int, 4);
            SqlParameter paramCreateUserType = new SqlParameter("@CreateUserType", SqlDbType.Int, 4);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateDate = new SqlParameter("@CreateDate", SqlDbType.DateTime);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.ReviewSysNo != AppConst.IntNull)
                paramReviewSysNo.Value = oParam.ReviewSysNo;
            else
                paramReviewSysNo.Value = System.DBNull.Value;
            if (oParam.IsHelpful != AppConst.IntNull)
                paramIsHelpful.Value = oParam.IsHelpful;
            else
                paramIsHelpful.Value = System.DBNull.Value;
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

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramReviewSysNo);
            cmd.Parameters.Add(paramIsHelpful);
            cmd.Parameters.Add(paramCreateUserType);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramCreateDate);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Update(ReviewRemarkInfo oParam)
        {
            string sql = @"UPDATE Review_Remark SET 
                            ReviewSysNo=@ReviewSysNo, IsHelpful=@IsHelpful, 
                            CreateUserTyp=@CreateUserType, CreateUserSysNo=@CreateUserSysNo, 
                            CreateDate=@CreateDate
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramReviewSysNo = new SqlParameter("@ReviewSysNo", SqlDbType.Int, 4);
            SqlParameter paramIsHelpful = new SqlParameter("@IsHelpful", SqlDbType.Int, 4);
            SqlParameter paramCreateUserType = new SqlParameter("@CreateUserType", SqlDbType.Int, 4);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateDate = new SqlParameter("@CreateDate", SqlDbType.DateTime);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.ReviewSysNo != AppConst.IntNull)
                paramReviewSysNo.Value = oParam.ReviewSysNo;
            else
                paramReviewSysNo.Value = System.DBNull.Value;
            if (oParam.IsHelpful != AppConst.IntNull)
                paramIsHelpful.Value = oParam.IsHelpful;
            else
                paramIsHelpful.Value = System.DBNull.Value;
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

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramReviewSysNo);
            cmd.Parameters.Add(paramIsHelpful);
            cmd.Parameters.Add(paramCreateUserType);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramCreateDate);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
    }
}
