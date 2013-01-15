using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Icson.Objects.Promotion;
using Icson.Utils;

namespace Icson.DBAccess.Promotion
{
  public  class PromotionItemGroupDac
    {
      public int Insert(PromotionItemGroupInfo oParam)
      {
          string sql = @"INSERT INTO Promotion_Item_Group
                            (
                            PromotionSysNo, GroupName, OrderNum, Status
                            )
                            VALUES (
                            @PromotionSysNo, @GroupName, @OrderNum, @Status
                            );set @SysNo = SCOPE_IDENTITY();";
          SqlCommand cmd = new SqlCommand(sql);

          SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
          SqlParameter paramPromotionSysNo = new SqlParameter("@PromotionSysNo", SqlDbType.Int, 4);
          SqlParameter paramGroupName = new SqlParameter("@GroupName", SqlDbType.Char, 10);
          SqlParameter paramOrderNum = new SqlParameter("@OrderNum", SqlDbType.Int, 4);
          SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
          paramSysNo.Direction = ParameterDirection.Output;
          if (oParam.PromotionSysNo != AppConst.IntNull)
              paramPromotionSysNo.Value = oParam.PromotionSysNo;
          else
              paramPromotionSysNo.Value = System.DBNull.Value;
          if (oParam.GroupName != AppConst.StringNull)
              paramGroupName.Value = oParam.GroupName;
          else
              paramGroupName.Value = System.DBNull.Value;
          if (oParam.OrderNum != AppConst.IntNull)
              paramOrderNum.Value = oParam.OrderNum;
          else
              paramOrderNum.Value = System.DBNull.Value;
          if (oParam.Status != AppConst.IntNull)
              paramStatus.Value = oParam.Status;
          else
              paramStatus.Value = System.DBNull.Value;

          cmd.Parameters.Add(paramSysNo);
          cmd.Parameters.Add(paramPromotionSysNo);
          cmd.Parameters.Add(paramGroupName);
          cmd.Parameters.Add(paramOrderNum);
          cmd.Parameters.Add(paramStatus);

          return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
      }

      public int Update(PromotionItemGroupInfo oParam)
      {
          string sql = @"UPDATE Promotion_Item_Group SET 
                            PromotionSysNo=@PromotionSysNo, GroupName=@GroupName, 
                            OrderNum=@OrderNum, Status=@Status
                            WHERE SysNo=@SysNo";
          SqlCommand cmd = new SqlCommand(sql);

          SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
          SqlParameter paramPromotionSysNo = new SqlParameter("@PromotionSysNo", SqlDbType.Int, 4);
          SqlParameter paramGroupName = new SqlParameter("@GroupName", SqlDbType.Char, 10);
          SqlParameter paramOrderNum = new SqlParameter("@OrderNum", SqlDbType.Int, 4);
          SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);

          if (oParam.SysNo != AppConst.IntNull)
              paramSysNo.Value = oParam.SysNo;
          else
              paramSysNo.Value = System.DBNull.Value;
          if (oParam.PromotionSysNo != AppConst.IntNull)
              paramPromotionSysNo.Value = oParam.PromotionSysNo;
          else
              paramPromotionSysNo.Value = System.DBNull.Value;
          if (oParam.GroupName != AppConst.StringNull)
              paramGroupName.Value = oParam.GroupName;
          else
              paramGroupName.Value = System.DBNull.Value;
          if (oParam.OrderNum != AppConst.IntNull)
              paramOrderNum.Value = oParam.OrderNum;
          else
              paramOrderNum.Value = System.DBNull.Value;
          if (oParam.Status != AppConst.IntNull)
              paramStatus.Value = oParam.Status;
          else
              paramStatus.Value = System.DBNull.Value;

          cmd.Parameters.Add(paramSysNo);
          cmd.Parameters.Add(paramPromotionSysNo);
          cmd.Parameters.Add(paramGroupName);
          cmd.Parameters.Add(paramOrderNum);
          cmd.Parameters.Add(paramStatus);

          return SqlHelper.ExecuteNonQuery(cmd);
      }
    } 
}
