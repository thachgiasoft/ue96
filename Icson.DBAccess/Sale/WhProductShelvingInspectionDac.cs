using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Icson.Objects.Sale;
using Icson.Utils;

namespace Icson.DBAccess.Sale
{
  public class WhProductShelvingInspectionDac
    {
      public int Insert(WhProductShelvingInspectionInfo oParam)
      {
          string sql = @"INSERT INTO Wh_ProductShelvingInspection
                            (
                            WorkType, BillType, BillSysNo, AllocatedUserSysNo, 
                            RealUserSysNo, UpdateUserSysNo, UpdateTime
                            )
                            VALUES (
                            @WorkType, @BillType, @BillSysNo, @AllocatedUserSysNo, 
                            @RealUserSysNo, @UpdateUserSysNo, @UpdateTime
                            );set @SysNo = SCOPE_IDENTITY();";
          SqlCommand cmd = new SqlCommand(sql);

          SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
          SqlParameter paramWorkType = new SqlParameter("@WorkType", SqlDbType.Int, 4);
          SqlParameter paramBillType = new SqlParameter("@BillType", SqlDbType.Int, 4);
          SqlParameter paramBillSysNo = new SqlParameter("@BillSysNo", SqlDbType.Int, 4);
          SqlParameter paramAllocatedUserSysNo = new SqlParameter("@AllocatedUserSysNo", SqlDbType.Int, 4);
          SqlParameter paramRealUserSysNo = new SqlParameter("@RealUserSysNo", SqlDbType.Int, 4);
          SqlParameter paramUpdateUserSysNo = new SqlParameter("@UpdateUserSysNo", SqlDbType.Int, 4);
          SqlParameter paramUpdateTime = new SqlParameter("@UpdateTime", SqlDbType.DateTime);
          paramSysNo.Direction = ParameterDirection.Output;
          if (oParam.WorkType != AppConst.IntNull)
              paramWorkType.Value = oParam.WorkType;
          else
              paramWorkType.Value = System.DBNull.Value;
          if (oParam.BillType != AppConst.IntNull)
              paramBillType.Value = oParam.BillType;
          else
              paramBillType.Value = System.DBNull.Value;
          if (oParam.BillSysNo != AppConst.IntNull)
              paramBillSysNo.Value = oParam.BillSysNo;
          else
              paramBillSysNo.Value = System.DBNull.Value;
          if (oParam.AllocatedUserSysNo != AppConst.IntNull)
              paramAllocatedUserSysNo.Value = oParam.AllocatedUserSysNo;
          else
              paramAllocatedUserSysNo.Value = System.DBNull.Value;
          if (oParam.RealUserSysNo != AppConst.IntNull)
              paramRealUserSysNo.Value = oParam.RealUserSysNo;
          else
              paramRealUserSysNo.Value = System.DBNull.Value;
          if (oParam.UpdateUserSysNo != AppConst.IntNull)
              paramUpdateUserSysNo.Value = oParam.UpdateUserSysNo;
          else
              paramUpdateUserSysNo.Value = System.DBNull.Value;
          if (oParam.UpdateTime != AppConst.DateTimeNull)
              paramUpdateTime.Value = oParam.UpdateTime;
          else
              paramUpdateTime.Value = System.DBNull.Value;

          cmd.Parameters.Add(paramSysNo);
          cmd.Parameters.Add(paramWorkType);
          cmd.Parameters.Add(paramBillType);
          cmd.Parameters.Add(paramBillSysNo);
          cmd.Parameters.Add(paramAllocatedUserSysNo);
          cmd.Parameters.Add(paramRealUserSysNo);
          cmd.Parameters.Add(paramUpdateUserSysNo);
          cmd.Parameters.Add(paramUpdateTime);

          return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
      }

      public int Update(WhProductShelvingInspectionInfo oParam)
      {
          string sql = @"UPDATE Wh_ProductShelvingInspection SET 
                            WorkType=@WorkType, BillType=@BillType, 
                            BillSysNo=@BillSysNo, AllocatedUserSysNo=@AllocatedUserSysNo, 
                            RealUserSysNo=@RealUserSysNo, UpdateUserSysNo=@UpdateUserSysNo, 
                            UpdateTime=@UpdateTime
                            WHERE SysNo=@SysNo";
          SqlCommand cmd = new SqlCommand(sql);

          SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
          SqlParameter paramWorkType = new SqlParameter("@WorkType", SqlDbType.Int, 4);
          SqlParameter paramBillType = new SqlParameter("@BillType", SqlDbType.Int, 4);
          SqlParameter paramBillSysNo = new SqlParameter("@BillSysNo", SqlDbType.Int, 4);
          SqlParameter paramAllocatedUserSysNo = new SqlParameter("@AllocatedUserSysNo", SqlDbType.Int, 4);
          SqlParameter paramRealUserSysNo = new SqlParameter("@RealUserSysNo", SqlDbType.Int, 4);
          SqlParameter paramUpdateUserSysNo = new SqlParameter("@UpdateUserSysNo", SqlDbType.Int, 4);
          SqlParameter paramUpdateTime = new SqlParameter("@UpdateTime", SqlDbType.DateTime);

          if (oParam.SysNo != AppConst.IntNull)
              paramSysNo.Value = oParam.SysNo;
          else
              paramSysNo.Value = System.DBNull.Value;
          if (oParam.WorkType != AppConst.IntNull)
              paramWorkType.Value = oParam.WorkType;
          else
              paramWorkType.Value = System.DBNull.Value;
          if (oParam.BillType != AppConst.IntNull)
              paramBillType.Value = oParam.BillType;
          else
              paramBillType.Value = System.DBNull.Value;
          if (oParam.BillSysNo != AppConst.IntNull)
              paramBillSysNo.Value = oParam.BillSysNo;
          else
              paramBillSysNo.Value = System.DBNull.Value;
          if (oParam.AllocatedUserSysNo != AppConst.IntNull)
              paramAllocatedUserSysNo.Value = oParam.AllocatedUserSysNo;
          else
              paramAllocatedUserSysNo.Value = System.DBNull.Value;
          if (oParam.RealUserSysNo != AppConst.IntNull)
              paramRealUserSysNo.Value = oParam.RealUserSysNo;
          else
              paramRealUserSysNo.Value = System.DBNull.Value;
          if (oParam.UpdateUserSysNo != AppConst.IntNull)
              paramUpdateUserSysNo.Value = oParam.UpdateUserSysNo;
          else
              paramUpdateUserSysNo.Value = System.DBNull.Value;
          if (oParam.UpdateTime != AppConst.DateTimeNull)
              paramUpdateTime.Value = oParam.UpdateTime;
          else
              paramUpdateTime.Value = System.DBNull.Value;

          cmd.Parameters.Add(paramSysNo);
          cmd.Parameters.Add(paramWorkType);
          cmd.Parameters.Add(paramBillType);
          cmd.Parameters.Add(paramBillSysNo);
          cmd.Parameters.Add(paramAllocatedUserSysNo);
          cmd.Parameters.Add(paramRealUserSysNo);
          cmd.Parameters.Add(paramUpdateUserSysNo);
          cmd.Parameters.Add(paramUpdateTime);

          return SqlHelper.ExecuteNonQuery(cmd);
      }
    }
}
