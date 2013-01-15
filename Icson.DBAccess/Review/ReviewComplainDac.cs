using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Icson.Objects.Review;
using Icson.Utils;

namespace Icson.DBAccess.Review
{
    public class ReviewComplainDac
    {
        public int Insert(ReviewComplainInfo oParam)
        {
            string sql = @"INSERT INTO Review_Complain
                            (
                            ReviewSysNo, ComplainContent, CreateUserType, CreateUserSysNo, 
                            CreateDate
                            )
                            VALUES (
                            @ReviewSysNo, @ComplainContent, @CreateUserType, @CreateUserSysNo, 
                            @CreateDate
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramReviewSysNo = new SqlParameter("@ReviewSysNo", SqlDbType.Int, 4);
            SqlParameter paramComplainContent = new SqlParameter("@ComplainContent", SqlDbType.NVarChar, 1000);
            SqlParameter paramCreateUserType = new SqlParameter("@CreateUserType", SqlDbType.Int, 4);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateDate = new SqlParameter("@CreateDate", SqlDbType.DateTime);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.ReviewSysNo != AppConst.IntNull)
                paramReviewSysNo.Value = oParam.ReviewSysNo;
            else
                paramReviewSysNo.Value = System.DBNull.Value;
            if (oParam.ComplainContent != AppConst.StringNull)
                paramComplainContent.Value = oParam.ComplainContent;
            else
                paramComplainContent.Value = System.DBNull.Value;
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
            cmd.Parameters.Add(paramComplainContent);
            cmd.Parameters.Add(paramCreateUserType);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramCreateDate);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Update(ReviewComplainInfo oParam)
        {
            string sql = @"UPDATE Review_Complain SET 
                            ReviewSysNo=@ReviewSysNo, ComplainContent=@ComplainContent, 
                            CreateUserType=@CreateUserType, CreateUserSysNo=@CreateUserSysNo, 
                            CreateDate=@CreateDate
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramReviewSysNo = new SqlParameter("@ReviewSysNo", SqlDbType.Int, 4);
            SqlParameter paramComplainContent = new SqlParameter("@ComplainContent", SqlDbType.NVarChar, 1000);
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
            if (oParam.ComplainContent != AppConst.StringNull)
                paramComplainContent.Value = oParam.ComplainContent;
            else
                paramComplainContent.Value = System.DBNull.Value;
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
            cmd.Parameters.Add(paramComplainContent);
            cmd.Parameters.Add(paramCreateUserType);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramCreateDate);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
    }
}