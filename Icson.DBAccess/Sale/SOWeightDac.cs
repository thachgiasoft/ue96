using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects.Sale;

namespace Icson.DBAccess.Sale
{
    public class SOWeightDac
    {
        public int Insert(SOWeightInfo oParam)
        {
            string sql = @"INSERT INTO SO_Weight
                            (
                            SOSysNo, Weight, ShipPriceCustomer, ShipPriceVendor, 
                            CreateUserSysNo, CreateTime, Status
                            )
                            VALUES (
                            @SOSysNo, @Weight, @ShipPriceCustomer, @ShipPriceVendor, 
                            @CreateUserSysNo, @CreateTime, @Status
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int, 4);
            SqlParameter paramWeight = new SqlParameter("@Weight", SqlDbType.Decimal, 9);
            SqlParameter paramShipPriceCustomer = new SqlParameter("@ShipPriceCustomer", SqlDbType.Decimal, 9);
            SqlParameter paramShipPriceVendor = new SqlParameter("@ShipPriceVendor", SqlDbType.Decimal, 9);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.SOSysNo != AppConst.IntNull)
                paramSOSysNo.Value = oParam.SOSysNo;
            else
                paramSOSysNo.Value = System.DBNull.Value;
            if (oParam.Weight != AppConst.DecimalNull)
                paramWeight.Value = oParam.Weight;
            else
                paramWeight.Value = System.DBNull.Value;
            if (oParam.ShipPriceCustomer != AppConst.DecimalNull)
                paramShipPriceCustomer.Value = oParam.ShipPriceCustomer;
            else
                paramShipPriceCustomer.Value = System.DBNull.Value;
            if (oParam.ShipPriceVendor != AppConst.DecimalNull)
                paramShipPriceVendor.Value = oParam.ShipPriceVendor;
            else
                paramShipPriceVendor.Value = System.DBNull.Value;
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
            cmd.Parameters.Add(paramSOSysNo);
            cmd.Parameters.Add(paramWeight);
            cmd.Parameters.Add(paramShipPriceCustomer);
            cmd.Parameters.Add(paramShipPriceVendor);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramCreateTime);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Update(SOWeightInfo oParam)
        {
            string sql = @"UPDATE SO_Weight SET 
                            SOSysNo=@SOSysNo, Weight=@Weight, 
                            ShipPriceCustomer=@ShipPriceCustomer, ShipPriceVendor=@ShipPriceVendor, 
                            CreateUserSysNo=@CreateUserSysNo, CreateTime=@CreateTime, 
                            Status=@Status
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int, 4);
            SqlParameter paramWeight = new SqlParameter("@Weight", SqlDbType.Decimal, 9);
            SqlParameter paramShipPriceCustomer = new SqlParameter("@ShipPriceCustomer", SqlDbType.Decimal, 9);
            SqlParameter paramShipPriceVendor = new SqlParameter("@ShipPriceVendor", SqlDbType.Decimal, 9);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.SOSysNo != AppConst.IntNull)
                paramSOSysNo.Value = oParam.SOSysNo;
            else
                paramSOSysNo.Value = System.DBNull.Value;
            if (oParam.Weight != AppConst.DecimalNull)
                paramWeight.Value = oParam.Weight;
            else
                paramWeight.Value = System.DBNull.Value;
            if (oParam.ShipPriceCustomer != AppConst.DecimalNull)
                paramShipPriceCustomer.Value = oParam.ShipPriceCustomer;
            else
                paramShipPriceCustomer.Value = System.DBNull.Value;
            if (oParam.ShipPriceVendor != AppConst.DecimalNull)
                paramShipPriceVendor.Value = oParam.ShipPriceVendor;
            else
                paramShipPriceVendor.Value = System.DBNull.Value;
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
            cmd.Parameters.Add(paramSOSysNo);
            cmd.Parameters.Add(paramWeight);
            cmd.Parameters.Add(paramShipPriceCustomer);
            cmd.Parameters.Add(paramShipPriceVendor);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramCreateTime);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
    }
}
