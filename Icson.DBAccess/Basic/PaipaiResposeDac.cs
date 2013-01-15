using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Icson.Objects.Basic;
using Icson.Utils;
namespace Icson.DBAccess.Basic
{
   public class PaipaiResposeDac
    {
       public int Insert(PaipaiResponseInfo oParam)
       {
           string sql = @"INSERT INTO Paipai_Response
                            (
                            ProductSysNo, PaipaiItemID, CreateTime
                            )
                            VALUES (
                            @ProductSysNo, @PaipaiItemID, @CreateTime
                            );set @SysNo = SCOPE_IDENTITY();";
           SqlCommand cmd = new SqlCommand(sql);

           SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
           SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int, 4);
           SqlParameter paramPaipaiItemID = new SqlParameter("@PaipaiItemID", SqlDbType.NVarChar, 50);
           SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
           paramSysNo.Direction = ParameterDirection.Output;
           if (oParam.ProductSysNo != AppConst.IntNull)
               paramProductSysNo.Value = oParam.ProductSysNo;
           else
               paramProductSysNo.Value = System.DBNull.Value;
           if (oParam.PaipaiItemID != AppConst.StringNull)
               paramPaipaiItemID.Value = oParam.PaipaiItemID;
           else
               paramPaipaiItemID.Value = System.DBNull.Value;
           if (oParam.CreateTime != AppConst.DateTimeNull)
               paramCreateTime.Value = oParam.CreateTime;
           else
               paramCreateTime.Value = System.DBNull.Value;

           cmd.Parameters.Add(paramSysNo);
           cmd.Parameters.Add(paramProductSysNo);
           cmd.Parameters.Add(paramPaipaiItemID);
           cmd.Parameters.Add(paramCreateTime);

           return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
       }
    }
}
