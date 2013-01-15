using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Icson.Objects.Review;
using Icson.Utils;

namespace Icson.DBAccess.Review
{
    public class ReviewDac
    {
        public ReviewDac()
        {}

        public int Insert(ReviewInfo oParam)
        {
            string sql = @"INSERT INTO Review_Master
                            (
                            SysNo, ReviewType, Title, Content1, 
                            Content2, Content3, Score, OwnedType, 
                            UnderstandingType, NickName, ReferenceType, ReferenceSysNo, 
                            IsTop, IsGood, TotalRemarkCount, TotalHelpfulRemarkCount, 
                            TotalComplainCount, Status, CreateCustomerSysNo, CreateDate, 
                            LastEditUserSysNo, LastEditDate,CustomerIP
                            )
                            VALUES (
                            @SysNo, @ReviewType, @Title, @Content1, 
                            @Content2, @Content3, @Score, @OwnedType, 
                            @UnderstandingType, @NickName, @ReferenceType, @ReferenceSysNo, 
                            @IsTop, @IsGood, @TotalRemarkCount, @TotalHelpfulRemarkCount, 
                            @TotalComplainCount, @Status, @CreateCustomerSysNo, @CreateDate, 
                            @LastEditUserSysNo, @LastEditDate,@CustomerIP
                            )";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramReviewType = new SqlParameter("@ReviewType", SqlDbType.Int, 4);
            SqlParameter paramTitle = new SqlParameter("@Title", SqlDbType.NVarChar, 200);
            SqlParameter paramContent1 = new SqlParameter("@Content1", SqlDbType.NVarChar, 1000);
            SqlParameter paramContent2 = new SqlParameter("@Content2", SqlDbType.NVarChar, 1000);
            SqlParameter paramContent3 = new SqlParameter("@Content3", SqlDbType.NVarChar, 1000);
            SqlParameter paramScore = new SqlParameter("@Score", SqlDbType.Int, 4);
            SqlParameter paramOwnedType = new SqlParameter("@OwnedType", SqlDbType.Int, 4);
            SqlParameter paramUnderstandingType = new SqlParameter("@UnderstandingType", SqlDbType.Int, 4);
            SqlParameter paramNickName = new SqlParameter("@NickName", SqlDbType.NVarChar, 50);
            SqlParameter paramReferenceType = new SqlParameter("@ReferenceType", SqlDbType.Int, 4);
            SqlParameter paramReferenceSysNo = new SqlParameter("@ReferenceSysNo", SqlDbType.Int, 4);
            SqlParameter paramIsTop = new SqlParameter("@IsTop", SqlDbType.Int, 4);
            SqlParameter paramIsGood = new SqlParameter("@IsGood", SqlDbType.Int, 4);
            SqlParameter paramTotalRemarkCount = new SqlParameter("@TotalRemarkCount", SqlDbType.Int, 4);
            SqlParameter paramTotalHelpfulRemarkCount = new SqlParameter("@TotalHelpfulRemarkCount", SqlDbType.Int, 4);
            SqlParameter paramTotalComplainCount = new SqlParameter("@TotalComplainCount", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramCreateCustomerSysNo = new SqlParameter("@CreateCustomerSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateDate = new SqlParameter("@CreateDate", SqlDbType.DateTime);
            SqlParameter paramLastEditUserSysNo = new SqlParameter("@LastEditUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramLastEditDate = new SqlParameter("@LastEditDate", SqlDbType.DateTime);
            SqlParameter paramCustomerIP = new SqlParameter("@CustomerIP", SqlDbType.NVarChar, 30);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.ReviewType != AppConst.IntNull)
                paramReviewType.Value = oParam.ReviewType;
            else
                paramReviewType.Value = System.DBNull.Value;
            if (oParam.Title != AppConst.StringNull)
                paramTitle.Value = oParam.Title;
            else
                paramTitle.Value = System.DBNull.Value;
            if (oParam.Content1 != AppConst.StringNull)
                paramContent1.Value = oParam.Content1;
            else
                paramContent1.Value = System.DBNull.Value;
            if (oParam.Content2 != AppConst.StringNull)
                paramContent2.Value = oParam.Content2;
            else
                paramContent2.Value = System.DBNull.Value;
            if (oParam.Content3 != AppConst.StringNull)
                paramContent3.Value = oParam.Content3;
            else
                paramContent3.Value = System.DBNull.Value;
            if (oParam.Score != AppConst.IntNull)
                paramScore.Value = oParam.Score;
            else
                paramScore.Value = System.DBNull.Value;
            if (oParam.OwnedType != AppConst.IntNull)
                paramOwnedType.Value = oParam.OwnedType;
            else
                paramOwnedType.Value = System.DBNull.Value;
            if (oParam.UnderstandingType != AppConst.IntNull)
                paramUnderstandingType.Value = oParam.UnderstandingType;
            else
                paramUnderstandingType.Value = System.DBNull.Value;
            if (oParam.NickName != AppConst.StringNull)
                paramNickName.Value = oParam.NickName;
            else
                paramNickName.Value = System.DBNull.Value;
            if (oParam.ReferenceType != AppConst.IntNull)
                paramReferenceType.Value = oParam.ReferenceType;
            else
                paramReferenceType.Value = System.DBNull.Value;
            if (oParam.ReferenceSysNo != AppConst.IntNull)
                paramReferenceSysNo.Value = oParam.ReferenceSysNo;
            else
                paramReferenceSysNo.Value = System.DBNull.Value;
            if (oParam.IsTop != AppConst.IntNull)
                paramIsTop.Value = oParam.IsTop;
            else
                paramIsTop.Value = System.DBNull.Value;
            if (oParam.IsGood != AppConst.IntNull)
                paramIsGood.Value = oParam.IsGood;
            else
                paramIsGood.Value = System.DBNull.Value;
            if (oParam.TotalRemarkCount != AppConst.IntNull)
                paramTotalRemarkCount.Value = oParam.TotalRemarkCount;
            else
                paramTotalRemarkCount.Value = System.DBNull.Value;
            if (oParam.TotalHelpfulRemarkCount != AppConst.IntNull)
                paramTotalHelpfulRemarkCount.Value = oParam.TotalHelpfulRemarkCount;
            else
                paramTotalHelpfulRemarkCount.Value = System.DBNull.Value;
            if (oParam.TotalComplainCount != AppConst.IntNull)
                paramTotalComplainCount.Value = oParam.TotalComplainCount;
            else
                paramTotalComplainCount.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;
            if (oParam.CreateCustomerSysNo != AppConst.IntNull)
                paramCreateCustomerSysNo.Value = oParam.CreateCustomerSysNo;
            else
                paramCreateCustomerSysNo.Value = System.DBNull.Value;
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

            if (oParam.CustomerIP != AppConst.StringNull)
                paramCustomerIP.Value = oParam.CustomerIP;
            else
                paramCustomerIP.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramReviewType);
            cmd.Parameters.Add(paramTitle);
            cmd.Parameters.Add(paramContent1);
            cmd.Parameters.Add(paramContent2);
            cmd.Parameters.Add(paramContent3);
            cmd.Parameters.Add(paramScore);
            cmd.Parameters.Add(paramOwnedType);
            cmd.Parameters.Add(paramUnderstandingType);
            cmd.Parameters.Add(paramNickName);
            cmd.Parameters.Add(paramReferenceType);
            cmd.Parameters.Add(paramReferenceSysNo);
            cmd.Parameters.Add(paramIsTop);
            cmd.Parameters.Add(paramIsGood);
            cmd.Parameters.Add(paramTotalRemarkCount);
            cmd.Parameters.Add(paramTotalHelpfulRemarkCount);
            cmd.Parameters.Add(paramTotalComplainCount);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramCreateCustomerSysNo);
            cmd.Parameters.Add(paramCreateDate);
            cmd.Parameters.Add(paramLastEditUserSysNo);
            cmd.Parameters.Add(paramLastEditDate);
            cmd.Parameters.Add(paramCustomerIP);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int Update(ReviewInfo oParam)
        {
            string sql = @"UPDATE Review_Master SET 
                            ReviewType=@ReviewType, Title=@Title, 
                            Content1=@Content1, Content2=@Content2, 
                            Content3=@Content3, Score=@Score, 
                            OwnedType=@OwnedType, UnderstandingType=@UnderstandingType, 
                            NickName=@NickName, ReferenceType=@ReferenceType, 
                            ReferenceSysNo=@ReferenceSysNo, IsTop=@IsTop, 
                            IsGood=@IsGood, TotalRemarkCount=@TotalRemarkCount, 
                            TotalHelpfulRemarkCount=@TotalHelpfulRemarkCount, TotalComplainCount=@TotalComplainCount, 
                            Status=@Status, CreateCustomerSysNo=@CreateCustomerSysNo, 
                            CreateDate=@CreateDate, LastEditUserSysNo=@LastEditUserSysNo, 
                            LastEditDate=@LastEditDate
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramReviewType = new SqlParameter("@ReviewType", SqlDbType.Int, 4);
            SqlParameter paramTitle = new SqlParameter("@Title", SqlDbType.NVarChar, 200);
            SqlParameter paramContent1 = new SqlParameter("@Content1", SqlDbType.NVarChar, 1000);
            SqlParameter paramContent2 = new SqlParameter("@Content2", SqlDbType.NVarChar, 1000);
            SqlParameter paramContent3 = new SqlParameter("@Content3", SqlDbType.NVarChar, 1000);
            SqlParameter paramScore = new SqlParameter("@Score", SqlDbType.Int, 4);
            SqlParameter paramOwnedType = new SqlParameter("@OwnedType", SqlDbType.Int, 4);
            SqlParameter paramUnderstandingType = new SqlParameter("@UnderstandingType", SqlDbType.Int, 4);
            SqlParameter paramNickName = new SqlParameter("@NickName", SqlDbType.NVarChar, 50);
            SqlParameter paramReferenceType = new SqlParameter("@ReferenceType", SqlDbType.Int, 4);
            SqlParameter paramReferenceSysNo = new SqlParameter("@ReferenceSysNo", SqlDbType.Int, 4);
            SqlParameter paramIsTop = new SqlParameter("@IsTop", SqlDbType.Int, 4);
            SqlParameter paramIsGood = new SqlParameter("@IsGood", SqlDbType.Int, 4);
            SqlParameter paramTotalRemarkCount = new SqlParameter("@TotalRemarkCount", SqlDbType.Int, 4);
            SqlParameter paramTotalHelpfulRemarkCount = new SqlParameter("@TotalHelpfulRemarkCount", SqlDbType.Int, 4);
            SqlParameter paramTotalComplainCount = new SqlParameter("@TotalComplainCount", SqlDbType.Int, 4);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramCreateCustomerSysNo = new SqlParameter("@CreateCustomerSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateDate = new SqlParameter("@CreateDate", SqlDbType.DateTime);
            SqlParameter paramLastEditUserSysNo = new SqlParameter("@LastEditUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramLastEditDate = new SqlParameter("@LastEditDate", SqlDbType.DateTime);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.ReviewType != AppConst.IntNull)
                paramReviewType.Value = oParam.ReviewType;
            else
                paramReviewType.Value = System.DBNull.Value;
            if (oParam.Title != AppConst.StringNull)
                paramTitle.Value = oParam.Title;
            else
                paramTitle.Value = System.DBNull.Value;
            if (oParam.Content1 != AppConst.StringNull)
                paramContent1.Value = oParam.Content1;
            else
                paramContent1.Value = System.DBNull.Value;
            if (oParam.Content2 != AppConst.StringNull)
                paramContent2.Value = oParam.Content2;
            else
                paramContent2.Value = System.DBNull.Value;
            if (oParam.Content3 != AppConst.StringNull)
                paramContent3.Value = oParam.Content3;
            else
                paramContent3.Value = System.DBNull.Value;
            if (oParam.Score != AppConst.IntNull)
                paramScore.Value = oParam.Score;
            else
                paramScore.Value = System.DBNull.Value;
            if (oParam.OwnedType != AppConst.IntNull)
                paramOwnedType.Value = oParam.OwnedType;
            else
                paramOwnedType.Value = System.DBNull.Value;
            if (oParam.UnderstandingType != AppConst.IntNull)
                paramUnderstandingType.Value = oParam.UnderstandingType;
            else
                paramUnderstandingType.Value = System.DBNull.Value;
            if (oParam.NickName != AppConst.StringNull)
                paramNickName.Value = oParam.NickName;
            else
                paramNickName.Value = System.DBNull.Value;
            if (oParam.ReferenceType != AppConst.IntNull)
                paramReferenceType.Value = oParam.ReferenceType;
            else
                paramReferenceType.Value = System.DBNull.Value;
            if (oParam.ReferenceSysNo != AppConst.IntNull)
                paramReferenceSysNo.Value = oParam.ReferenceSysNo;
            else
                paramReferenceSysNo.Value = System.DBNull.Value;
            if (oParam.IsTop != AppConst.IntNull)
                paramIsTop.Value = oParam.IsTop;
            else
                paramIsTop.Value = System.DBNull.Value;
            if (oParam.IsGood != AppConst.IntNull)
                paramIsGood.Value = oParam.IsGood;
            else
                paramIsGood.Value = System.DBNull.Value;
            if (oParam.TotalRemarkCount != AppConst.IntNull)
                paramTotalRemarkCount.Value = oParam.TotalRemarkCount;
            else
                paramTotalRemarkCount.Value = System.DBNull.Value;
            if (oParam.TotalHelpfulRemarkCount != AppConst.IntNull)
                paramTotalHelpfulRemarkCount.Value = oParam.TotalHelpfulRemarkCount;
            else
                paramTotalHelpfulRemarkCount.Value = System.DBNull.Value;
            if (oParam.TotalComplainCount != AppConst.IntNull)
                paramTotalComplainCount.Value = oParam.TotalComplainCount;
            else
                paramTotalComplainCount.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;
            if (oParam.CreateCustomerSysNo != AppConst.IntNull)
                paramCreateCustomerSysNo.Value = oParam.CreateCustomerSysNo;
            else
                paramCreateCustomerSysNo.Value = System.DBNull.Value;
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
            cmd.Parameters.Add(paramReviewType);
            cmd.Parameters.Add(paramTitle);
            cmd.Parameters.Add(paramContent1);
            cmd.Parameters.Add(paramContent2);
            cmd.Parameters.Add(paramContent3);
            cmd.Parameters.Add(paramScore);
            cmd.Parameters.Add(paramOwnedType);
            cmd.Parameters.Add(paramUnderstandingType);
            cmd.Parameters.Add(paramNickName);
            cmd.Parameters.Add(paramReferenceType);
            cmd.Parameters.Add(paramReferenceSysNo);
            cmd.Parameters.Add(paramIsTop);
            cmd.Parameters.Add(paramIsGood);
            cmd.Parameters.Add(paramTotalRemarkCount);
            cmd.Parameters.Add(paramTotalHelpfulRemarkCount);
            cmd.Parameters.Add(paramTotalComplainCount);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramCreateCustomerSysNo);
            cmd.Parameters.Add(paramCreateDate);
            cmd.Parameters.Add(paramLastEditUserSysNo);
            cmd.Parameters.Add(paramLastEditDate);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int UpdateReviewMaster(Hashtable paramHash)
        {
            StringBuilder sb = new StringBuilder(200);
            sb.Append("UPDATE Review_Master SET ");

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