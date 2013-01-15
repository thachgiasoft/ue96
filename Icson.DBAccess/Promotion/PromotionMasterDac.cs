using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Icson.Objects.Promotion;
using Icson.Utils;

namespace Icson.DBAccess.Promotion
{
  public  class PromotionMasterDac
    {
      public int Insert(PromotionMasterInfo oParam)
      {
          string sql = @"INSERT INTO Promotion_Master
                            (
                            PromotionName, PromotionNote, CreateUserSysNo, CreateTime, 
                            Status
                            )
                            VALUES (
                            @PromotionName, @PromotionNote, @CreateUserSysNo, @CreateTime, 
                            @Status
                            );set @SysNo = SCOPE_IDENTITY();";
          SqlCommand cmd = new SqlCommand(sql);

          SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
          SqlParameter paramPromotionName = new SqlParameter("@PromotionName", SqlDbType.NVarChar, 200);
          SqlParameter paramPromotionNote = new SqlParameter("@PromotionNote", SqlDbType.Text, 0);
          SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
          SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
          SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
          paramSysNo.Direction = ParameterDirection.Output;
          if (oParam.PromotionName != AppConst.StringNull)
              paramPromotionName.Value = oParam.PromotionName;
          else
              paramPromotionName.Value = System.DBNull.Value;
          if (oParam.PromotionNote != AppConst.StringNull)
              paramPromotionNote.Value = oParam.PromotionNote;
          else
              paramPromotionNote.Value = System.DBNull.Value;
          if (oParam.CreateUserSysNo != AppConst.IntNull)
              paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
          else
              paramCreateUserSysNo.Value = System.DBNull.Value;
          if (oParam.CreateTime != AppConst.DateTimeNull)
              paramCreateTime.Value = oParam.CreateTime;
          else
              paramCreateTime.Value = System.DBNull.Value;
          if (oParam.Status != AppConst.IntNull)
              paramStatus.Value = oParam.Status;
          else
              paramStatus.Value = System.DBNull.Value;

          cmd.Parameters.Add(paramSysNo);
          cmd.Parameters.Add(paramPromotionName);
          cmd.Parameters.Add(paramPromotionNote);
          cmd.Parameters.Add(paramCreateUserSysNo);
          cmd.Parameters.Add(paramCreateTime);
          cmd.Parameters.Add(paramStatus);

          return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
      }

      public int Update(PromotionMasterInfo oParam)
      {
          string sql = @"UPDATE Promotion_Master SET 
                            PromotionName=@PromotionName, PromotionNote=@PromotionNote, 
                            CreateUserSysNo=@CreateUserSysNo, CreateTime=@CreateTime, 
                            Status=@Status
                            WHERE SysNo=@SysNo";
          SqlCommand cmd = new SqlCommand(sql);

          SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
          SqlParameter paramPromotionName = new SqlParameter("@PromotionName", SqlDbType.NVarChar, 200);
          SqlParameter paramPromotionNote = new SqlParameter("@PromotionNote", SqlDbType.Text, 0);
          SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
          SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
          SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);

          if (oParam.SysNo != AppConst.IntNull)
              paramSysNo.Value = oParam.SysNo;
          else
              paramSysNo.Value = System.DBNull.Value;
          if (oParam.PromotionName != AppConst.StringNull)
              paramPromotionName.Value = oParam.PromotionName;
          else
              paramPromotionName.Value = System.DBNull.Value;
          if (oParam.PromotionNote != AppConst.StringNull)
              paramPromotionNote.Value = oParam.PromotionNote;
          else
              paramPromotionNote.Value = System.DBNull.Value;
          if (oParam.CreateUserSysNo != AppConst.IntNull)
              paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
          else
              paramCreateUserSysNo.Value = System.DBNull.Value;
          if (oParam.CreateTime != AppConst.DateTimeNull)
              paramCreateTime.Value = oParam.CreateTime;
          else
              paramCreateTime.Value = System.DBNull.Value;
          if (oParam.Status != AppConst.IntNull)
              paramStatus.Value = oParam.Status;
          else
              paramStatus.Value = System.DBNull.Value;

          cmd.Parameters.Add(paramSysNo);
          cmd.Parameters.Add(paramPromotionName);
          cmd.Parameters.Add(paramPromotionNote);
          cmd.Parameters.Add(paramCreateUserSysNo);
          cmd.Parameters.Add(paramCreateTime);
          cmd.Parameters.Add(paramStatus);

          return SqlHelper.ExecuteNonQuery(cmd);
      }
    }
}
