using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Icson.Objects.Online;
using Icson.Utils;

namespace Icson.DBAccess.Online
{
    public class TopicItemDac
    {
        public int Insert(TopicItemInfo oParam)
        {
            string sql = @"INSERT INTO Topic_Item
                            (
                            TopicSysNo, C3ReviewItemSysNo, Score
                            )
                            VALUES (
                            @TopicSysNo, @C3ReviewItemSysNo, @Score
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramTopicSysNo = new SqlParameter("@TopicSysNo", SqlDbType.Int, 4);
            SqlParameter paramC3ReviewItemSysNo = new SqlParameter("@C3ReviewItemSysNo", SqlDbType.Int, 4);
            SqlParameter paramScore = new SqlParameter("@Score", SqlDbType.Int, 4);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.TopicSysNo != AppConst.IntNull)
                paramTopicSysNo.Value = oParam.TopicSysNo;
            else
                paramTopicSysNo.Value = System.DBNull.Value;
            if (oParam.C3ReviewItemSysNo != AppConst.IntNull)
                paramC3ReviewItemSysNo.Value = oParam.C3ReviewItemSysNo;
            else
                paramC3ReviewItemSysNo.Value = System.DBNull.Value;
            if (oParam.Score != AppConst.IntNull)
                paramScore.Value = oParam.Score;
            else
                paramScore.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramTopicSysNo);
            cmd.Parameters.Add(paramC3ReviewItemSysNo);
            cmd.Parameters.Add(paramScore);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Update(TopicItemInfo oParam)
        {
            string sql = @"UPDATE Topic_Item SET 
                            TopicSysNo=@TopicSysNo, C3ReviewItemSysNo=@C3ReviewItemSysNo, 
                            Score=@Score
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramTopicSysNo = new SqlParameter("@TopicSysNo", SqlDbType.Int, 4);
            SqlParameter paramC3ReviewItemSysNo = new SqlParameter("@C3ReviewItemSysNo", SqlDbType.Int, 4);
            SqlParameter paramScore = new SqlParameter("@Score", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.TopicSysNo != AppConst.IntNull)
                paramTopicSysNo.Value = oParam.TopicSysNo;
            else
                paramTopicSysNo.Value = System.DBNull.Value;
            if (oParam.C3ReviewItemSysNo != AppConst.IntNull)
                paramC3ReviewItemSysNo.Value = oParam.C3ReviewItemSysNo;
            else
                paramC3ReviewItemSysNo.Value = System.DBNull.Value;
            if (oParam.Score != AppConst.IntNull)
                paramScore.Value = oParam.Score;
            else
                paramScore.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramTopicSysNo);
            cmd.Parameters.Add(paramC3ReviewItemSysNo);
            cmd.Parameters.Add(paramScore);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
    }
}
