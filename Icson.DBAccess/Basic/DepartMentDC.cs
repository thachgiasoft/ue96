using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects.Basic;

namespace Icson.DBAccess.Basic
{
    public class DepartMentDC
    {
        public DepartMentDC()
        {
        }

        public int InsertOrUpdateDepart(OperationTypeInfo oParam,bool isAdd)
        {
            string sql = "";
            int _tmpInt = AppConst.IntNull;
            if (isAdd)
            {
                sql = @"INSERT INTO sys_OperationType
                            (
                            TypeName, TypeDescription, Status
                            )
                            VALUES (
                            @TypeName, @TypeDescription, @Status
                            );set @SysNo = SCOPE_IDENTITY();";
            }
            else
            {
                sql=@"UPDATE sys_OperationType
                            SET TypeName=@TypeName,TypeDescription=@TypeDescription,Status=@Status WHERE SysNo=@SysNo";
            }

            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramTypeName = new SqlParameter("@TypeName", SqlDbType.VarChar, 50);
            SqlParameter paramTypeDescription = new SqlParameter("@TypeDescription", SqlDbType.VarChar, 500);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);

            if (isAdd)
                paramSysNo.Direction = ParameterDirection.Output;
            else
            {
                if (oParam.SysNo != AppConst.IntNull)
                    paramSysNo.Value = oParam.SysNo;
                else
                    paramSysNo.Value = System.DBNull.Value;
            }

            if (oParam.TypeName != AppConst.StringNull)
                paramTypeName.Value = oParam.TypeName;
            else
                paramTypeName.Value = System.DBNull.Value;
            if (oParam.TypeDescription != AppConst.StringNull)
                paramTypeDescription.Value = oParam.TypeDescription;
            else
                paramTypeDescription.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramTypeName);
            cmd.Parameters.Add(paramTypeDescription);
            cmd.Parameters.Add(paramStatus);

            if (isAdd)
            {
                SqlHelper.ExecuteNonQuery(cmd, out _tmpInt);
                oParam.SysNo = _tmpInt;
                return 1;
            }
            else
                return SqlHelper.ExecuteNonQuery(cmd);            
        }


        public int DelPrivilegeDept(string strDelSql,int SysNo)
        {
            string sql = "delete from sys_operationtype_privilege where OperationTypeID=" + SysNo;
             return SqlHelper.ExecuteNonQuery(sql);
            

            //StringBuilder sb = new StringBuilder(200);
            //if(strDelSql.Trim()=="")
            //    sb.Append("delete from sys_operationtype_privilege where OperationTypeID=" + SysNo);
            //else
            //    sb.Append("delete from sys_operationtype_privilege where PrivilegeID in("+strDelSql+") and OperationTypeID=" + SysNo);

            //return SqlHelper.ExecuteNonQuery(sb.ToString().Trim());
        }

        public int DelDept(int SysNo)
        {
            string sql="delete from sys_operationtype_privilege where OperationTypeID="+SysNo;
            SqlHelper.ExecuteNonQuery(sql);

            sql = "delete from sys_operationtype where SysNo=" + SysNo;
            return SqlHelper.ExecuteNonQuery(sql);
        }

        public int InsertPrivilegeDept(int DeptID, int PrivilegeID)
        {
            string sql = @"INSERT INTO sys_OperationType_Privilege
                            (
                            OperationTypeID, PrivilegeID
                            )
                            VALUES (
                            @OperationTypeID, @PrivilegeID
                            )";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramOperationTypeID = new SqlParameter("@OperationTypeID", SqlDbType.Int, 4);
            SqlParameter paramPrivilegeID = new SqlParameter("@PrivilegeID", SqlDbType.Int, 4);


            if (DeptID != AppConst.IntNull)
                paramOperationTypeID.Value = DeptID;
            else
                paramOperationTypeID.Value = System.DBNull.Value;
            if (PrivilegeID != AppConst.IntNull)
                paramPrivilegeID.Value = PrivilegeID;
            else
                paramPrivilegeID.Value = System.DBNull.Value;


            cmd.Parameters.Add(paramOperationTypeID);
            cmd.Parameters.Add(paramPrivilegeID);

            return SqlHelper.ExecuteNonQuery(cmd);  
        }


    }
}
