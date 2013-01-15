using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Text;

using Icson.Utils;
using Icson.Objects.Online;

namespace Icson.DBAccess.Online
{
  public  class FeedBackShowListDac
    {
        public FeedBackShowListDac()
        { 
        }
      public int Insert(FeedBackShowListInfo oParam)
      {
          string sql = @"INSERT INTO FeedbackShowList
                            (
                            FeedBackSysNo, Type, CreateTime
                            )
                            VALUES (
                            @FeedBackSysNo, @Type, @CreateTime
                            );set @SysNo = SCOPE_IDENTITY();";
          SqlCommand cmd = new SqlCommand(sql);

          SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
          SqlParameter paramFeedBackSysNo = new SqlParameter("@FeedBackSysNo", SqlDbType.Int, 4);
          SqlParameter paramType = new SqlParameter("@Type", SqlDbType.Int, 4);
          SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
          paramSysNo.Direction = ParameterDirection.Output;
          if (oParam.FeedBackSysNo != AppConst.IntNull)
              paramFeedBackSysNo.Value = oParam.FeedBackSysNo;
          else
              paramFeedBackSysNo.Value = System.DBNull.Value;
          if (oParam.Type != AppConst.IntNull)
              paramType.Value = oParam.Type;
          else
              paramType.Value = System.DBNull.Value;
          if (oParam.CreateTime != AppConst.DateTimeNull)
              paramCreateTime.Value = oParam.CreateTime;
          else
              paramCreateTime.Value = System.DBNull.Value;

          cmd.Parameters.Add(paramSysNo);
          cmd.Parameters.Add(paramFeedBackSysNo);
          cmd.Parameters.Add(paramType);
          cmd.Parameters.Add(paramCreateTime);

          return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
      }
      public int Delete(int FeedBacksysno)
      {
          string sql = "delete from FeedBackShowList where FeedBacksysno = " + FeedBacksysno;
          SqlCommand cmd = new SqlCommand(sql);
          return SqlHelper.ExecuteNonQuery(cmd);

      }
    }
}
