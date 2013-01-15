using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using Icson.Objects.Sale;
using Icson.Utils;
namespace Icson.DBAccess.Sale
{
    public class SOServiceDac
    {
        public SOServiceDac()
        {}

        public int Insert(SOServiceInfo oParam)
        {
            string sql = @"INSERT INTO SO_Service
                            (
                            SOSysNo, ServiceAddress, ServiceReceiveName, ServicePhone, 
                            ServiceExpectTime, ServiceMemo, ServiceAgreedTime, ServiceActualTime, 
                            ServiceQuality1, ServiceQuality2, ServiceQuality3, ServiceEvaluation1, 
                            ServiceEvaluation2, ServiceEvaluation3
                            )
                            VALUES (
                            @SOSysNo, @ServiceAddress, @ServiceReceiveName, @ServicePhone, 
                            @ServiceExpectTime, @ServiceMemo, @ServiceAgreedTime, @ServiceActualTime, 
                            @ServiceQuality1, @ServiceQuality2, @ServiceQuality3, @ServiceEvaluation1, 
                            @ServiceEvaluation2, @ServiceEvaluation3
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int, 4);
            SqlParameter paramServiceAddress = new SqlParameter("@ServiceAddress", SqlDbType.NVarChar, 200);
            SqlParameter paramServiceReceiveName = new SqlParameter("@ServiceReceiveName", SqlDbType.NVarChar, 50);
            SqlParameter paramServicePhone = new SqlParameter("@ServicePhone", SqlDbType.NVarChar, 100);
            SqlParameter paramServiceExpectTime = new SqlParameter("@ServiceExpectTime", SqlDbType.NVarChar, 100);
            SqlParameter paramServiceMemo = new SqlParameter("@ServiceMemo", SqlDbType.NVarChar, 500);
            SqlParameter paramServiceAgreedTime = new SqlParameter("@ServiceAgreedTime", SqlDbType.NVarChar, 100);
            SqlParameter paramServiceActualTime = new SqlParameter("@ServiceActualTime", SqlDbType.NVarChar, 100);
            SqlParameter paramServiceQuality1 = new SqlParameter("@ServiceQuality1", SqlDbType.Int, 4);
            SqlParameter paramServiceQuality2 = new SqlParameter("@ServiceQuality2", SqlDbType.Int, 4);
            SqlParameter paramServiceQuality3 = new SqlParameter("@ServiceQuality3", SqlDbType.Int, 4);
            SqlParameter paramServiceEvaluation1 = new SqlParameter("@ServiceEvaluation1", SqlDbType.NVarChar, 500);
            SqlParameter paramServiceEvaluation2 = new SqlParameter("@ServiceEvaluation2", SqlDbType.NVarChar, 500);
            SqlParameter paramServiceEvaluation3 = new SqlParameter("@ServiceEvaluation3", SqlDbType.NVarChar, 500);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.SOSysNo != AppConst.IntNull)
                paramSOSysNo.Value = oParam.SOSysNo;
            else
                paramSOSysNo.Value = System.DBNull.Value;
            if (oParam.ServiceAddress != AppConst.StringNull)
                paramServiceAddress.Value = oParam.ServiceAddress;
            else
                paramServiceAddress.Value = System.DBNull.Value;
            if (oParam.ServiceReceiveName != AppConst.StringNull)
                paramServiceReceiveName.Value = oParam.ServiceReceiveName;
            else
                paramServiceReceiveName.Value = System.DBNull.Value;
            if (oParam.ServicePhone != AppConst.StringNull)
                paramServicePhone.Value = oParam.ServicePhone;
            else
                paramServicePhone.Value = System.DBNull.Value;
            if (oParam.ServiceExpectTime != AppConst.StringNull)
                paramServiceExpectTime.Value = oParam.ServiceExpectTime;
            else
                paramServiceExpectTime.Value = System.DBNull.Value;
            if (oParam.ServiceMemo != AppConst.StringNull)
                paramServiceMemo.Value = oParam.ServiceMemo;
            else
                paramServiceMemo.Value = System.DBNull.Value;
            if (oParam.ServiceAgreedTime != AppConst.StringNull)
                paramServiceAgreedTime.Value = oParam.ServiceAgreedTime;
            else
                paramServiceAgreedTime.Value = System.DBNull.Value;
            if (oParam.ServiceActualTime != AppConst.StringNull)
                paramServiceActualTime.Value = oParam.ServiceActualTime;
            else
                paramServiceActualTime.Value = System.DBNull.Value;
            if (oParam.ServiceQuality1 != AppConst.IntNull)
                paramServiceQuality1.Value = oParam.ServiceQuality1;
            else
                paramServiceQuality1.Value = System.DBNull.Value;
            if (oParam.ServiceQuality2 != AppConst.IntNull)
                paramServiceQuality2.Value = oParam.ServiceQuality2;
            else
                paramServiceQuality2.Value = System.DBNull.Value;
            if (oParam.ServiceQuality3 != AppConst.IntNull)
                paramServiceQuality3.Value = oParam.ServiceQuality3;
            else
                paramServiceQuality3.Value = System.DBNull.Value;
            if (oParam.ServiceEvaluation1 != AppConst.StringNull)
                paramServiceEvaluation1.Value = oParam.ServiceEvaluation1;
            else
                paramServiceEvaluation1.Value = System.DBNull.Value;
            if (oParam.ServiceEvaluation2 != AppConst.StringNull)
                paramServiceEvaluation2.Value = oParam.ServiceEvaluation2;
            else
                paramServiceEvaluation2.Value = System.DBNull.Value;
            if (oParam.ServiceEvaluation3 != AppConst.StringNull)
                paramServiceEvaluation3.Value = oParam.ServiceEvaluation3;
            else
                paramServiceEvaluation3.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramSOSysNo);
            cmd.Parameters.Add(paramServiceAddress);
            cmd.Parameters.Add(paramServiceReceiveName);
            cmd.Parameters.Add(paramServicePhone);
            cmd.Parameters.Add(paramServiceExpectTime);
            cmd.Parameters.Add(paramServiceMemo);
            cmd.Parameters.Add(paramServiceAgreedTime);
            cmd.Parameters.Add(paramServiceActualTime);
            cmd.Parameters.Add(paramServiceQuality1);
            cmd.Parameters.Add(paramServiceQuality2);
            cmd.Parameters.Add(paramServiceQuality3);
            cmd.Parameters.Add(paramServiceEvaluation1);
            cmd.Parameters.Add(paramServiceEvaluation2);
            cmd.Parameters.Add(paramServiceEvaluation3);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Update(SOServiceInfo oParam)
        {
            string sql = @"UPDATE SO_Service SET 
                            SOSysNo=@SOSysNo, ServiceAddress=@ServiceAddress, 
                            ServiceReceiveName=@ServiceReceiveName, ServicePhone=@ServicePhone, 
                            ServiceExpectTime=@ServiceExpectTime, ServiceMemo=@ServiceMemo, 
                            ServiceAgreedTime=@ServiceAgreedTime, ServiceActualTime=@ServiceActualTime, 
                            ServiceQuality1=@ServiceQuality1, ServiceQuality2=@ServiceQuality2, 
                            ServiceQuality3=@ServiceQuality3, ServiceEvaluation1=@ServiceEvaluation1, 
                            ServiceEvaluation2=@ServiceEvaluation2, ServiceEvaluation3=@ServiceEvaluation3
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int, 4);
            SqlParameter paramServiceAddress = new SqlParameter("@ServiceAddress", SqlDbType.NVarChar, 200);
            SqlParameter paramServiceReceiveName = new SqlParameter("@ServiceReceiveName", SqlDbType.NVarChar, 50);
            SqlParameter paramServicePhone = new SqlParameter("@ServicePhone", SqlDbType.NVarChar, 100);
            SqlParameter paramServiceExpectTime = new SqlParameter("@ServiceExpectTime", SqlDbType.NVarChar, 100);
            SqlParameter paramServiceMemo = new SqlParameter("@ServiceMemo", SqlDbType.NVarChar, 500);
            SqlParameter paramServiceAgreedTime = new SqlParameter("@ServiceAgreedTime", SqlDbType.NVarChar, 100);
            SqlParameter paramServiceActualTime = new SqlParameter("@ServiceActualTime", SqlDbType.NVarChar, 100);
            SqlParameter paramServiceQuality1 = new SqlParameter("@ServiceQuality1", SqlDbType.Int, 4);
            SqlParameter paramServiceQuality2 = new SqlParameter("@ServiceQuality2", SqlDbType.Int, 4);
            SqlParameter paramServiceQuality3 = new SqlParameter("@ServiceQuality3", SqlDbType.Int, 4);
            SqlParameter paramServiceEvaluation1 = new SqlParameter("@ServiceEvaluation1", SqlDbType.NVarChar, 500);
            SqlParameter paramServiceEvaluation2 = new SqlParameter("@ServiceEvaluation2", SqlDbType.NVarChar, 500);
            SqlParameter paramServiceEvaluation3 = new SqlParameter("@ServiceEvaluation3", SqlDbType.NVarChar, 500);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.SOSysNo != AppConst.IntNull)
                paramSOSysNo.Value = oParam.SOSysNo;
            else
                paramSOSysNo.Value = System.DBNull.Value;
            if (oParam.ServiceAddress != AppConst.StringNull)
                paramServiceAddress.Value = oParam.ServiceAddress;
            else
                paramServiceAddress.Value = System.DBNull.Value;
            if (oParam.ServiceReceiveName != AppConst.StringNull)
                paramServiceReceiveName.Value = oParam.ServiceReceiveName;
            else
                paramServiceReceiveName.Value = System.DBNull.Value;
            if (oParam.ServicePhone != AppConst.StringNull)
                paramServicePhone.Value = oParam.ServicePhone;
            else
                paramServicePhone.Value = System.DBNull.Value;
            if (oParam.ServiceExpectTime != AppConst.StringNull)
                paramServiceExpectTime.Value = oParam.ServiceExpectTime;
            else
                paramServiceExpectTime.Value = System.DBNull.Value;
            if (oParam.ServiceMemo != AppConst.StringNull)
                paramServiceMemo.Value = oParam.ServiceMemo;
            else
                paramServiceMemo.Value = System.DBNull.Value;
            if (oParam.ServiceAgreedTime != AppConst.StringNull)
                paramServiceAgreedTime.Value = oParam.ServiceAgreedTime;
            else
                paramServiceAgreedTime.Value = System.DBNull.Value;
            if (oParam.ServiceActualTime != AppConst.StringNull)
                paramServiceActualTime.Value = oParam.ServiceActualTime;
            else
                paramServiceActualTime.Value = System.DBNull.Value;
            if (oParam.ServiceQuality1 != AppConst.IntNull)
                paramServiceQuality1.Value = oParam.ServiceQuality1;
            else
                paramServiceQuality1.Value = System.DBNull.Value;
            if (oParam.ServiceQuality2 != AppConst.IntNull)
                paramServiceQuality2.Value = oParam.ServiceQuality2;
            else
                paramServiceQuality2.Value = System.DBNull.Value;
            if (oParam.ServiceQuality3 != AppConst.IntNull)
                paramServiceQuality3.Value = oParam.ServiceQuality3;
            else
                paramServiceQuality3.Value = System.DBNull.Value;
            if (oParam.ServiceEvaluation1 != AppConst.StringNull)
                paramServiceEvaluation1.Value = oParam.ServiceEvaluation1;
            else
                paramServiceEvaluation1.Value = System.DBNull.Value;
            if (oParam.ServiceEvaluation2 != AppConst.StringNull)
                paramServiceEvaluation2.Value = oParam.ServiceEvaluation2;
            else
                paramServiceEvaluation2.Value = System.DBNull.Value;
            if (oParam.ServiceEvaluation3 != AppConst.StringNull)
                paramServiceEvaluation3.Value = oParam.ServiceEvaluation3;
            else
                paramServiceEvaluation3.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramSOSysNo);
            cmd.Parameters.Add(paramServiceAddress);
            cmd.Parameters.Add(paramServiceReceiveName);
            cmd.Parameters.Add(paramServicePhone);
            cmd.Parameters.Add(paramServiceExpectTime);
            cmd.Parameters.Add(paramServiceMemo);
            cmd.Parameters.Add(paramServiceAgreedTime);
            cmd.Parameters.Add(paramServiceActualTime);
            cmd.Parameters.Add(paramServiceQuality1);
            cmd.Parameters.Add(paramServiceQuality2);
            cmd.Parameters.Add(paramServiceQuality3);
            cmd.Parameters.Add(paramServiceEvaluation1);
            cmd.Parameters.Add(paramServiceEvaluation2);
            cmd.Parameters.Add(paramServiceEvaluation3);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
    }
}
