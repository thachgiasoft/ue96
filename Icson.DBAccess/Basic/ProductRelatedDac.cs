using System;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects.Basic;

namespace Icson.DBAccess.Basic
{
    public class ProductRelatedDac
    {
        public ProductRelatedDac()
        {}

        public int Insert(ProductRelatedInfo oParam)
        {
            string sql = @"INSERT INTO Product_Related
                            (
                            MasterProductSysNo, RelatedProductSysNo, CreateUserSysNo, CreateTime, 
                            Status
                            )
                            VALUES (
                            @MasterProductSysNo, @RelatedProductSysNo, @CreateUserSysNo, @CreateTime, 
                            @Status
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramMasterProductSysNo = new SqlParameter("@MasterProductSysNo", SqlDbType.Int, 4);
            SqlParameter paramRelatedProductSysNo = new SqlParameter("@RelatedProductSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.MasterProductSysNo != AppConst.IntNull)
                paramMasterProductSysNo.Value = oParam.MasterProductSysNo;
            else
                paramMasterProductSysNo.Value = System.DBNull.Value;
            if (oParam.RelatedProductSysNo != AppConst.IntNull)
                paramRelatedProductSysNo.Value = oParam.RelatedProductSysNo;
            else
                paramRelatedProductSysNo.Value = System.DBNull.Value;
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
            cmd.Parameters.Add(paramMasterProductSysNo);
            cmd.Parameters.Add(paramRelatedProductSysNo);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramCreateTime);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Update(ProductRelatedInfo oParam)
        {
            string sql = @"UPDATE Product_Related SET 
                            MasterProductSysNo=@MasterProductSysNo, RelatedProductSysNo=@RelatedProductSysNo, 
                            CreateUserSysNo=@CreateUserSysNo, CreateTime=@CreateTime, 
                            Status=@Status
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramMasterProductSysNo = new SqlParameter("@MasterProductSysNo", SqlDbType.Int, 4);
            SqlParameter paramRelatedProductSysNo = new SqlParameter("@RelatedProductSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.MasterProductSysNo != AppConst.IntNull)
                paramMasterProductSysNo.Value = oParam.MasterProductSysNo;
            else
                paramMasterProductSysNo.Value = System.DBNull.Value;
            if (oParam.RelatedProductSysNo != AppConst.IntNull)
                paramRelatedProductSysNo.Value = oParam.RelatedProductSysNo;
            else
                paramRelatedProductSysNo.Value = System.DBNull.Value;
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
            cmd.Parameters.Add(paramMasterProductSysNo);
            cmd.Parameters.Add(paramRelatedProductSysNo);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramCreateTime);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
    }
}