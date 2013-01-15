using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Icson.Objects.Online;
using Icson.Utils;

namespace Icson.DBAccess.Online
{
  public  class ShoppingGuideDac
    {
      public int Insert(ShoppingGuideInfo oParam)
      {
          string sql = @"INSERT INTO ShoppingGuide
                            (
                            Title, Url, Content, CreateUserSysNo, 
                            CreateTime, Status
                            )
                            VALUES (
                            @Title, @Url, @Content, @CreateUserSysNo, 
                            @CreateTime, @Status
                            );set @SysNo = SCOPE_IDENTITY();";
          SqlCommand cmd = new SqlCommand(sql);

          SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
          SqlParameter paramTitle = new SqlParameter("@Title", SqlDbType.NVarChar, 100);
          SqlParameter paramUrl = new SqlParameter("@Url", SqlDbType.NVarChar, 100);
          SqlParameter paramContent = new SqlParameter("@Content", SqlDbType.Text, 0);
          SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
          SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
          SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
          paramSysNo.Direction = ParameterDirection.Output;
          if (oParam.Title != AppConst.StringNull)
              paramTitle.Value = oParam.Title;
          else
              paramTitle.Value = System.DBNull.Value;
          if (oParam.Url != AppConst.StringNull)
              paramUrl.Value = oParam.Url;
          else
              paramUrl.Value = System.DBNull.Value;
          if (oParam.Content != AppConst.StringNull)
              paramContent.Value = oParam.Content;
          else
              paramContent.Value = System.DBNull.Value;
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
          cmd.Parameters.Add(paramTitle);
          cmd.Parameters.Add(paramUrl);
          cmd.Parameters.Add(paramContent);
          cmd.Parameters.Add(paramCreateUserSysNo);
          cmd.Parameters.Add(paramCreateTime);
          cmd.Parameters.Add(paramStatus);

          return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
      }
      public int Update(ShoppingGuideInfo oParam)
      {
          string sql = @"UPDATE ShoppingGuide SET 
                            Title=@Title, Url=@Url, 
                            Content=@Content, CreateUserSysNo=@CreateUserSysNo, 
                            CreateTime=@CreateTime, Status=@Status
                            WHERE SysNo=@SysNo";
          SqlCommand cmd = new SqlCommand(sql);

          SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
          SqlParameter paramTitle = new SqlParameter("@Title", SqlDbType.NVarChar, 100);
          SqlParameter paramUrl = new SqlParameter("@Url", SqlDbType.NVarChar, 100);
          SqlParameter paramContent = new SqlParameter("@Content", SqlDbType.Text, 0);
          SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
          SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
          SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);

          if (oParam.SysNo != AppConst.IntNull)
              paramSysNo.Value = oParam.SysNo;
          else
              paramSysNo.Value = System.DBNull.Value;
          if (oParam.Title != AppConst.StringNull)
              paramTitle.Value = oParam.Title;
          else
              paramTitle.Value = System.DBNull.Value;
          if (oParam.Url != AppConst.StringNull)
              paramUrl.Value = oParam.Url;
          else
              paramUrl.Value = System.DBNull.Value;
          if (oParam.Content != AppConst.StringNull)
              paramContent.Value = oParam.Content;
          else
              paramContent.Value = System.DBNull.Value;
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
          cmd.Parameters.Add(paramTitle);
          cmd.Parameters.Add(paramUrl);
          cmd.Parameters.Add(paramContent);
          cmd.Parameters.Add(paramCreateUserSysNo);
          cmd.Parameters.Add(paramCreateTime);
          cmd.Parameters.Add(paramStatus);

          return SqlHelper.ExecuteNonQuery(cmd);
      }
    }
}
