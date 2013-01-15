using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using Icson.Objects.Sale;
using Icson.Utils;

namespace Icson.DBAccess.Sale
{
    public class SOAdwaysDac
    {
        public SOAdwaysDac()
        { }

        public int Insert(SOAdwaysInfo oParam)
        {
            string sql = @"INSERT INTO SO_Adways
                            (
                            CustomerSysNo, AdwaysID, AdwaysEmail, SOSysNo, 
                            ShipPrice
                            )
                            VALUES (
                            @CustomerSysNo, @AdwaysID, @AdwaysEmail, @SOSysNo, 
                            @ShipPrice
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramCustomerSysNo = new SqlParameter("@CustomerSysNo", SqlDbType.Int, 4);
            SqlParameter paramAdwaysID = new SqlParameter("@AdwaysID", SqlDbType.NVarChar, 50);
            SqlParameter paramAdwaysEmail = new SqlParameter("@AdwaysEmail", SqlDbType.NVarChar, 200);
            SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int, 4);
            SqlParameter paramShipPrice = new SqlParameter("@ShipPrice", SqlDbType.Decimal, 9);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.CustomerSysNo != AppConst.IntNull)
                paramCustomerSysNo.Value = oParam.CustomerSysNo;
            else
                paramCustomerSysNo.Value = System.DBNull.Value;
            if (oParam.AdwaysID != AppConst.StringNull)
                paramAdwaysID.Value = oParam.AdwaysID;
            else
                paramAdwaysID.Value = System.DBNull.Value;
            if (oParam.AdwaysEmail != AppConst.StringNull)
                paramAdwaysEmail.Value = oParam.AdwaysEmail;
            else
                paramAdwaysEmail.Value = System.DBNull.Value;
            if (oParam.SOSysNo != AppConst.IntNull)
                paramSOSysNo.Value = oParam.SOSysNo;
            else
                paramSOSysNo.Value = System.DBNull.Value;
            if (oParam.ShipPrice != AppConst.DecimalNull)
                paramShipPrice.Value = oParam.ShipPrice;
            else
                paramShipPrice.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramCustomerSysNo);
            cmd.Parameters.Add(paramAdwaysID);
            cmd.Parameters.Add(paramAdwaysEmail);
            cmd.Parameters.Add(paramSOSysNo);
            cmd.Parameters.Add(paramShipPrice);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Update(SOAdwaysInfo oParam)
        {
            string sql = @"UPDATE SO_Adways SET 
                            CustomerSysNo=@CustomerSysNo, AdwaysID=@AdwaysID, 
                            AdwaysEmail=@AdwaysEmail, SOSysNo=@SOSysNo, 
                            ShipPrice=@ShipPrice
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramCustomerSysNo = new SqlParameter("@CustomerSysNo", SqlDbType.Int, 4);
            SqlParameter paramAdwaysID = new SqlParameter("@AdwaysID", SqlDbType.NVarChar, 50);
            SqlParameter paramAdwaysEmail = new SqlParameter("@AdwaysEmail", SqlDbType.NVarChar, 200);
            SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int, 4);
            SqlParameter paramShipPrice = new SqlParameter("@ShipPrice", SqlDbType.Decimal, 9);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.CustomerSysNo != AppConst.IntNull)
                paramCustomerSysNo.Value = oParam.CustomerSysNo;
            else
                paramCustomerSysNo.Value = System.DBNull.Value;
            if (oParam.AdwaysID != AppConst.StringNull)
                paramAdwaysID.Value = oParam.AdwaysID;
            else
                paramAdwaysID.Value = System.DBNull.Value;
            if (oParam.AdwaysEmail != AppConst.StringNull)
                paramAdwaysEmail.Value = oParam.AdwaysEmail;
            else
                paramAdwaysEmail.Value = System.DBNull.Value;
            if (oParam.SOSysNo != AppConst.IntNull)
                paramSOSysNo.Value = oParam.SOSysNo;
            else
                paramSOSysNo.Value = System.DBNull.Value;
            if (oParam.ShipPrice != AppConst.DecimalNull)
                paramShipPrice.Value = oParam.ShipPrice;
            else
                paramShipPrice.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramCustomerSysNo);
            cmd.Parameters.Add(paramAdwaysID);
            cmd.Parameters.Add(paramAdwaysEmail);
            cmd.Parameters.Add(paramSOSysNo);
            cmd.Parameters.Add(paramShipPrice);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
    }
}
