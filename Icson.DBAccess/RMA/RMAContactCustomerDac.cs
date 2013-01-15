using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using System.Text;


using Icson.Objects.RMA;
using Icson.Utils;
namespace Icson.DBAccess.RMA
{
  public  class RMAContactCustomerDac
    {
      public int Insert(RMAContactCustomerInfo oParam)
      {
          string sql = @"INSERT INTO RMA_ContactCustomer
                            (
                            RegisterSysNo, Content, ContactUserSysNo, CreateTime, 
                            NextContactTime
                            )
                            VALUES (
                            @RegisterSysNo, @Content, @ContactUserSysNo, @CreateTime, 
                            @NextContactTime
                            );set @SysNo = SCOPE_IDENTITY();";
          SqlCommand cmd = new SqlCommand(sql);

          SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
          SqlParameter paramRegisterSysNo = new SqlParameter("@RegisterSysNo", SqlDbType.Int, 4);
          SqlParameter paramContent = new SqlParameter("@Content", SqlDbType.NVarChar, 500);
          SqlParameter paramContactUserSysNo = new SqlParameter("@ContactUserSysNo", SqlDbType.Int, 4);
          SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
          SqlParameter paramNextContactTime = new SqlParameter("@NextContactTime", SqlDbType.DateTime);
          paramSysNo.Direction = ParameterDirection.Output;
          if (oParam.RegisterSysNo != AppConst.IntNull)
              paramRegisterSysNo.Value = oParam.RegisterSysNo;
          else
              paramRegisterSysNo.Value = System.DBNull.Value;
          if (oParam.Content != AppConst.StringNull)
              paramContent.Value = oParam.Content;
          else
              paramContent.Value = System.DBNull.Value;
          if (oParam.ContactUserSysNo != AppConst.IntNull)
              paramContactUserSysNo.Value = oParam.ContactUserSysNo;
          else
              paramContactUserSysNo.Value = System.DBNull.Value;
          if (oParam.CreateTime != AppConst.DateTimeNull)
              paramCreateTime.Value = oParam.CreateTime;
          else
              paramCreateTime.Value = System.DBNull.Value;
          if (oParam.NextContactTime != AppConst.DateTimeNull)
              paramNextContactTime.Value = oParam.NextContactTime;
          else
              paramNextContactTime.Value = System.DBNull.Value;

          cmd.Parameters.Add(paramSysNo);
          cmd.Parameters.Add(paramRegisterSysNo);
          cmd.Parameters.Add(paramContent);
          cmd.Parameters.Add(paramContactUserSysNo);
          cmd.Parameters.Add(paramCreateTime);
          cmd.Parameters.Add(paramNextContactTime);

          return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
      }
    }
}
