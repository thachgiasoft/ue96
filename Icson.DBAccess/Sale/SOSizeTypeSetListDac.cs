using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Icson.Utils;
using Icson.Objects.Sale;


namespace Icson.DBAccess.Sale
{
    public class SOSizeTypeSetListDac
    {
        public int Insert(SOSizeTypeSetListInfo oParam)
        {
            string sql = @"INSERT INTO SO_SizeType_SetList
                            (
                            ItemID, CreateUserSysNo, CreateTime, SizeType
                            )
                            VALUES (
                            @ItemID, @CreateUserSysNo, @CreateTime, @SizeType
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramItemID = new SqlParameter("@ItemID", SqlDbType.NVarChar, 50);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
            SqlParameter paramSizeType = new SqlParameter("@SizeType", SqlDbType.Int, 4);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.ItemID != AppConst.StringNull)
                paramItemID.Value = oParam.ItemID;
            else
                paramItemID.Value = System.DBNull.Value;
            if (oParam.CreateUserSysNo != AppConst.IntNull)
                paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
            else
                paramCreateUserSysNo.Value = System.DBNull.Value;
            if (oParam.CreateTime != AppConst.DateTimeNull)
                paramCreateTime.Value = oParam.CreateTime;
            else
                paramCreateTime.Value = System.DBNull.Value;
            if (oParam.SizeType != AppConst.IntNull)
                paramSizeType.Value = oParam.SizeType;
            else
                paramSizeType.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramItemID);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramCreateTime);
            cmd.Parameters.Add(paramSizeType);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }
    }
}
