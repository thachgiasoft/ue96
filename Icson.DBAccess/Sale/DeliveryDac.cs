using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects;
using Icson.Objects.Sale;
namespace Icson.DBAccess.Sale
{
   public class DeliveryDac
    {
       public int Insert(DeliveryManSetListInfo oParam)
       {
           string sql = @"INSERT INTO DeliveryManSetList
                            (
                            ItemID, FreightUserSysNo, CreateTime
                            )
                            VALUES (
                            @ItemID, @FreightUserSysNo, @CreateTime
                            );set @SysNo = SCOPE_IDENTITY();";
           SqlCommand cmd = new SqlCommand(sql);

           SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
           SqlParameter paramItemID = new SqlParameter("@ItemID", SqlDbType.NVarChar, 50);
           SqlParameter paramFreightUserSysNo = new SqlParameter("@FreightUserSysNo", SqlDbType.Int, 4);
           SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
           paramSysNo.Direction = ParameterDirection.Output;
           if (oParam.ItemID != AppConst.StringNull)
               paramItemID.Value = oParam.ItemID;
           else
               paramItemID.Value = System.DBNull.Value;
           if (oParam.FreightUserSysNo != AppConst.IntNull)
               paramFreightUserSysNo.Value = oParam.FreightUserSysNo;
           else
               paramFreightUserSysNo.Value = System.DBNull.Value;
           if (oParam.CreateTime != AppConst.DateTimeNull)
               paramCreateTime.Value = oParam.CreateTime;
           else
               paramCreateTime.Value = System.DBNull.Value;

           cmd.Parameters.Add(paramSysNo);
           cmd.Parameters.Add(paramItemID);
           cmd.Parameters.Add(paramFreightUserSysNo);
           cmd.Parameters.Add(paramCreateTime);

           return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
       }
    }
}
