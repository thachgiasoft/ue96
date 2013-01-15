using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Icson.Objects.Review;
using Icson.Utils;

namespace Icson.DBAccess.Review
{
    public class ReviewC3ItemScoreDac
    {
        public int Insert(ReviewC3ItemScoreInfo oParam)
        {
            string sql = @"INSERT INTO Review_C3ItemScore
                            (
                            ReviewSysNo, C3ReviewItemSysNo, Score, Weight
                            )
                            VALUES (
                            @ReviewSysNo, @C3ReviewItemSysNo, @Score, @Weight
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramReviewSysNo = new SqlParameter("@ReviewSysNo", SqlDbType.Int, 4);
            SqlParameter paramC3ReviewItemSysNo = new SqlParameter("@C3ReviewItemSysNo", SqlDbType.Int, 4);
            SqlParameter paramScore = new SqlParameter("@Score", SqlDbType.Int, 4);
            SqlParameter paramWeight = new SqlParameter("@Weight", SqlDbType.Int, 4);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.ReviewSysNo != AppConst.IntNull)
                paramReviewSysNo.Value = oParam.ReviewSysNo;
            else
                paramReviewSysNo.Value = System.DBNull.Value;
            if (oParam.C3ReviewItemSysNo != AppConst.IntNull)
                paramC3ReviewItemSysNo.Value = oParam.C3ReviewItemSysNo;
            else
                paramC3ReviewItemSysNo.Value = System.DBNull.Value;
            if (oParam.Score != AppConst.IntNull)
                paramScore.Value = oParam.Score;
            else
                paramScore.Value = System.DBNull.Value;
            if (oParam.Weight != AppConst.IntNull)
                paramWeight.Value = oParam.Weight;
            else
                paramWeight.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramReviewSysNo);
            cmd.Parameters.Add(paramC3ReviewItemSysNo);
            cmd.Parameters.Add(paramScore);
            cmd.Parameters.Add(paramWeight);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Update(ReviewC3ItemScoreInfo oParam)
        {
            string sql = @"UPDATE Review_C3ItemScore SET 
                            ReviewSysNo=@ReviewSysNo, C3ReviewItemSysNo=@C3ReviewItemSysNo, 
                            Score=@Score, Weight=@Weight
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramReviewSysNo = new SqlParameter("@ReviewSysNo", SqlDbType.Int, 4);
            SqlParameter paramC3ReviewItemSysNo = new SqlParameter("@C3ReviewItemSysNo", SqlDbType.Int, 4);
            SqlParameter paramScore = new SqlParameter("@Score", SqlDbType.Int, 4);
            SqlParameter paramWeight = new SqlParameter("@Weight", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.ReviewSysNo != AppConst.IntNull)
                paramReviewSysNo.Value = oParam.ReviewSysNo;
            else
                paramReviewSysNo.Value = System.DBNull.Value;
            if (oParam.C3ReviewItemSysNo != AppConst.IntNull)
                paramC3ReviewItemSysNo.Value = oParam.C3ReviewItemSysNo;
            else
                paramC3ReviewItemSysNo.Value = System.DBNull.Value;
            if (oParam.Score != AppConst.IntNull)
                paramScore.Value = oParam.Score;
            else
                paramScore.Value = System.DBNull.Value;
            if (oParam.Weight != AppConst.IntNull)
                paramWeight.Value = oParam.Weight;
            else
                paramWeight.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramReviewSysNo);
            cmd.Parameters.Add(paramC3ReviewItemSysNo);
            cmd.Parameters.Add(paramScore);
            cmd.Parameters.Add(paramWeight);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
    }
}
