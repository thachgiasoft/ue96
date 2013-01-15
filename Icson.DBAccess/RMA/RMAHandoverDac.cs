using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Text;

using Icson.Utils;
using Icson.Objects.RMA;

namespace Icson.DBAccess.RMA
{
   public class RMAHandoverDac
    {
       public RMAHandoverDac()
		{
		}
       public int InsertMaster(RMAHandoverInfo oParam)
       {
           string sql = @"INSERT INTO RMA_Handover
                            (
                            SysNo, HandoverID, CreateTime, CreateUserSysNo, 
                            UpdateTime, UpdateUserSysNo, OutStockTime, OutStockUserSysNo, 
                            ReceiveTime, ReceiveUserSysNo, FromLocation, ToLocation, 
                            Status
                            )
                            VALUES (
                            @SysNo, @HandoverID, @CreateTime, @CreateUserSysNo, 
                            @UpdateTime, @UpdateUserSysNo, @OutStockTime, @OutStockUserSysNo, 
                            @ReceiveTime, @ReceiveUserSysNo, @FromLocation, @ToLocation, 
                            @Status
                            )";
           SqlCommand cmd = new SqlCommand(sql);

           SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
           SqlParameter paramHandoverID = new SqlParameter("@HandoverID", SqlDbType.NVarChar, 50);
           SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
           SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
           SqlParameter paramUpdateTime = new SqlParameter("@UpdateTime", SqlDbType.DateTime);
           SqlParameter paramUpdateUserSysNo = new SqlParameter("@UpdateUserSysNo", SqlDbType.Int, 4);
           SqlParameter paramOutStockTime = new SqlParameter("@OutStockTime", SqlDbType.DateTime);
           SqlParameter paramOutStockUserSysNo = new SqlParameter("@OutStockUserSysNo", SqlDbType.Int, 4);
           SqlParameter paramReceiveTime = new SqlParameter("@ReceiveTime", SqlDbType.DateTime);
           SqlParameter paramReceiveUserSysNo = new SqlParameter("@ReceiveUserSysNo", SqlDbType.Int, 4);
           SqlParameter paramFromLocation = new SqlParameter("@FromLocation", SqlDbType.Int, 4);
           SqlParameter paramToLocation = new SqlParameter("@ToLocation", SqlDbType.Int, 4);
           SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);

           if (oParam.SysNo != AppConst.IntNull)
               paramSysNo.Value = oParam.SysNo;
           else
               paramSysNo.Value = System.DBNull.Value;
           if (oParam.HandoverID != AppConst.StringNull)
               paramHandoverID.Value = oParam.HandoverID;
           else
               paramHandoverID.Value = System.DBNull.Value;
           if (oParam.CreateTime != AppConst.DateTimeNull)
               paramCreateTime.Value = oParam.CreateTime;
           else
               paramCreateTime.Value = System.DBNull.Value;
           if (oParam.CreateUserSysNo != AppConst.IntNull)
               paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
           else
               paramCreateUserSysNo.Value = System.DBNull.Value;
           if (oParam.UpdateTime != AppConst.DateTimeNull)
               paramUpdateTime.Value = oParam.UpdateTime;
           else
               paramUpdateTime.Value = System.DBNull.Value;
           if (oParam.UpdateUserSysNo != AppConst.IntNull)
               paramUpdateUserSysNo.Value = oParam.UpdateUserSysNo;
           else
               paramUpdateUserSysNo.Value = System.DBNull.Value;
           if (oParam.OutStockTime != AppConst.DateTimeNull)
               paramOutStockTime.Value = oParam.OutStockTime;
           else
               paramOutStockTime.Value = System.DBNull.Value;
           if (oParam.OutStockUserSysNo != AppConst.IntNull)
               paramOutStockUserSysNo.Value = oParam.OutStockUserSysNo;
           else
               paramOutStockUserSysNo.Value = System.DBNull.Value;
           if (oParam.ReceiveTime != AppConst.DateTimeNull)
               paramReceiveTime.Value = oParam.ReceiveTime;
           else
               paramReceiveTime.Value = System.DBNull.Value;
           if (oParam.ReceiveUserSysNo != AppConst.IntNull)
               paramReceiveUserSysNo.Value = oParam.ReceiveUserSysNo;
           else
               paramReceiveUserSysNo.Value = System.DBNull.Value;
           if (oParam.FromLocation != AppConst.IntNull)
               paramFromLocation.Value = oParam.FromLocation;
           else
               paramFromLocation.Value = System.DBNull.Value;
           if (oParam.ToLocation != AppConst.IntNull)
               paramToLocation.Value = oParam.ToLocation;
           else
               paramToLocation.Value = System.DBNull.Value;
           if (oParam.Status != AppConst.IntNull)
               paramStatus.Value = oParam.Status;
           else
               paramStatus.Value = System.DBNull.Value;

           cmd.Parameters.Add(paramSysNo);
           cmd.Parameters.Add(paramHandoverID);
           cmd.Parameters.Add(paramCreateTime);
           cmd.Parameters.Add(paramCreateUserSysNo);
           cmd.Parameters.Add(paramUpdateTime);
           cmd.Parameters.Add(paramUpdateUserSysNo);
           cmd.Parameters.Add(paramOutStockTime);
           cmd.Parameters.Add(paramOutStockUserSysNo);
           cmd.Parameters.Add(paramReceiveTime);
           cmd.Parameters.Add(paramReceiveUserSysNo);
           cmd.Parameters.Add(paramFromLocation);
           cmd.Parameters.Add(paramToLocation);
           cmd.Parameters.Add(paramStatus);

           return SqlHelper.ExecuteNonQuery(cmd);
       }

       public int UpdateMaster(Hashtable paramHash)
       {
           StringBuilder sb = new StringBuilder(200);
           sb.Append("UPDATE RMA_Handover SET ");

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
                       if (item.ToString() == AppConst.IntNull.ToString() || item.ToString() == AppConst.DecimalNull.ToString())
                           sb.Append(key).Append(" = null");
                       else
                           sb.Append(key).Append(" = ").Append(item.ToString());
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


       public int InsertItem(int handoverSysNo, int registerSysNo)
       {
           string sql = @"INSERT INTO RMA_Handover_Item
                            (
                            HandoverSysNo, RegisterSysNo
                            )
                            VALUES (
                            @HandoverSysNo, @RegisterSysNo
                            );set @SysNo = SCOPE_IDENTITY();";
           SqlCommand cmd = new SqlCommand(sql);

           SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
           SqlParameter paramHandoverSysNo = new SqlParameter("@HandoverSysNo", SqlDbType.Int, 4);
           SqlParameter paramRegisterSysNo = new SqlParameter("@RegisterSysNo", SqlDbType.Int, 4);

           paramSysNo.Direction = ParameterDirection.Output;

           paramHandoverSysNo.Value = handoverSysNo;
           paramRegisterSysNo.Value = registerSysNo;

           cmd.Parameters.Add(paramSysNo);
           cmd.Parameters.Add(paramHandoverSysNo);
           cmd.Parameters.Add(paramRegisterSysNo);

           return SqlHelper.ExecuteNonQuery(cmd);
       }

       public int DeleteItem(int handoverSysNo, int registerSysNo)
       {
           string sql = "delete from RMA_Handover_Item where handoverSysNo = " + handoverSysNo + " and registerSysNo = " + registerSysNo;
           SqlCommand cmd = new SqlCommand(sql);
           return SqlHelper.ExecuteNonQuery(cmd);
       }
    }
}
