using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Icson.Objects.Online;
using Icson.Utils;

namespace Icson.DBAccess.Online
{
    public class OnlineC1_C2Dac
    {
        public int Insert(OnlineC1_C2Info oParam)
        {
            string sql = @"INSERT INTO OnlineC1_C2
                            (
                            C2SysNo, OrderNum
                            )
                            VALUES (
                            @C2SysNo, @OrderNum
                            )";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramC2SysNo = new SqlParameter("@C2SysNo", SqlDbType.Int, 4);
            SqlParameter paramOrderNum = new SqlParameter("@OrderNum", SqlDbType.Int, 4);

            if (oParam.C2SysNo != AppConst.IntNull)
                paramC2SysNo.Value = oParam.C2SysNo;
            else
                paramC2SysNo.Value = System.DBNull.Value;
            if (oParam.OrderNum != AppConst.IntNull)
                paramOrderNum.Value = oParam.OrderNum;
            else
                paramOrderNum.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramC2SysNo);
            cmd.Parameters.Add(paramOrderNum);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int Update(OnlineC1_C2Info oParam)
        {
            string sql = @"UPDATE OnlineC1_C2 SET 
                            C2SysNo=@C2SysNo, OrderNum=@OrderNum
                            WHERE C2SysNo=@C2SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramC2SysNo = new SqlParameter("@C2SysNo", SqlDbType.Int, 4);
            SqlParameter paramOrderNum = new SqlParameter("@OrderNum", SqlDbType.Int, 4);

            if (oParam.C2SysNo != AppConst.IntNull)
                paramC2SysNo.Value = oParam.C2SysNo;
            else
                paramC2SysNo.Value = System.DBNull.Value;
            if (oParam.OrderNum != AppConst.IntNull)
                paramOrderNum.Value = oParam.OrderNum;
            else
                paramOrderNum.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramC2SysNo);
            cmd.Parameters.Add(paramOrderNum);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int Delete(OnlineC1_C2Info oParam)
        {
            string sql = "DELETE OnlineC1_C2 where C2SysNo=@C2SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramC2SysNo = new SqlParameter("@C2SysNo", SqlDbType.Int, 4);
            if (oParam.C2SysNo != AppConst.IntNull)
                paramC2SysNo.Value = oParam.C2SysNo;
            else
                paramC2SysNo.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramC2SysNo);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int SetOrderNum(OnlineC1_C2Info oParam)
        {
            string sql = "update onlineC1_C2 set ordernum = " + oParam.OrderNum + " where c2sysno = " + oParam.C2SysNo;
            SqlCommand cmd = new SqlCommand(sql);
            return SqlHelper.ExecuteNonQuery(cmd);
        }
    }
}
