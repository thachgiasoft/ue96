using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using Icson.Utils;
using Icson.Objects;
using Icson.Objects.Sale;

namespace Icson.DBAccess.Sale
{
    public class SearchKeyTrackDac
    {
        public int Insert(SearchKeyTrackInfo oParam)
        {
            string sql = @"INSERT INTO SearchKeyTrack
                            (
                            CustomerID, Keyword, SearchTime
                            )
                            VALUES (
                            @CustomerID, @Keyword, @SearchTime
                            );set @SysNo = SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramCustomerID = new SqlParameter("@CustomerID", SqlDbType.NVarChar, 50);
            SqlParameter paramKeyword = new SqlParameter("@Keyword", SqlDbType.NVarChar, 200);
            SqlParameter paramSearchTime = new SqlParameter("@SearchTime", SqlDbType.DateTime);
            paramSysNo.Direction = ParameterDirection.Output;
            if (oParam.CustomerID != AppConst.StringNull)
                paramCustomerID.Value = oParam.CustomerID;
            else
                paramCustomerID.Value = System.DBNull.Value;
            if (oParam.Keyword != AppConst.StringNull)
                paramKeyword.Value = oParam.Keyword;
            else
                paramKeyword.Value = System.DBNull.Value;
            if (oParam.SearchTime != AppConst.DateTimeNull)
                paramSearchTime.Value = oParam.SearchTime;
            else
                paramSearchTime.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramCustomerID);
            cmd.Parameters.Add(paramKeyword);
            cmd.Parameters.Add(paramSearchTime);

            return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
        }

        public int Update(SearchKeyTrackInfo oParam)
        {
            string sql = @"UPDATE SearchKeyTrack SET 
                            CustomerID=@CustomerID, Keyword=@Keyword, 
                            SearchTime=@SearchTime
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramCustomerID = new SqlParameter("@CustomerID", SqlDbType.NVarChar, 50);
            SqlParameter paramKeyword = new SqlParameter("@Keyword", SqlDbType.NVarChar, 200);
            SqlParameter paramSearchTime = new SqlParameter("@SearchTime", SqlDbType.DateTime);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.CustomerID != AppConst.StringNull)
                paramCustomerID.Value = oParam.CustomerID;
            else
                paramCustomerID.Value = System.DBNull.Value;
            if (oParam.Keyword != AppConst.StringNull)
                paramKeyword.Value = oParam.Keyword;
            else
                paramKeyword.Value = System.DBNull.Value;
            if (oParam.SearchTime != AppConst.DateTimeNull)
                paramSearchTime.Value = oParam.SearchTime;
            else
                paramSearchTime.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramCustomerID);
            cmd.Parameters.Add(paramKeyword);
            cmd.Parameters.Add(paramSearchTime);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
    }
}
