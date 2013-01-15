using System;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects.Basic;

namespace Icson.DBAccess.Basic
{
    /// <summary>
    /// Summary description for ColorDac.
    /// </summary>
    public class ColorDac
    {

        public ColorDac()
        {
        }

        public int Insert(Color1Info oParam)
        {
            string sql = @"INSERT INTO Color1
                            (
                            SysNo, Color1ID, Color1Name, Status
                            )
                            VALUES (
                            @SysNo, @Color1ID, @Color1Name, @Status
                            )";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramColor1ID = new SqlParameter("@Color1ID", SqlDbType.NVarChar, 20);
            SqlParameter paramColor1Name = new SqlParameter("@Color1Name", SqlDbType.NVarChar, 200);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);

            paramSysNo.Value = oParam.SysNo;
            paramColor1ID.Value = oParam.Color1ID;
            paramColor1Name.Value = oParam.Color1Name;
            paramStatus.Value = oParam.Status;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramColor1ID);
            cmd.Parameters.Add(paramColor1Name);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
        public int Insert(Color2Info oParam)
        {
            string sql = @"INSERT INTO Color2
                            (
                            SysNo, Color1SysNo, Color2ID, Color2Name, 
                            Status
                            )
                            VALUES (
                            @SysNo, @Color1SysNo, @Color2ID, @Color2Name, 
                            @Status
                            )";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramColor1SysNo = new SqlParameter("@Color1SysNo", SqlDbType.Int, 4);
            SqlParameter paramColor2ID = new SqlParameter("@Color2ID", SqlDbType.NVarChar, 20);
            SqlParameter paramColor2Name = new SqlParameter("@Color2Name", SqlDbType.NVarChar, 200);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);

            paramSysNo.Value = oParam.SysNo;
            paramColor1SysNo.Value = oParam.Color1SysNo;
            paramColor2ID.Value = oParam.Color2ID;
            paramColor2Name.Value = oParam.Color2Name;
            paramStatus.Value = oParam.Status;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramColor1SysNo);
            cmd.Parameters.Add(paramColor2ID);
            cmd.Parameters.Add(paramColor2Name);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
        
        public int Update(Color1Info oParam)
        {
            string sql = @"UPDATE Color1 SET 
                            Color1ID=@Color1ID, 
                            Color1Name=@Color1Name, Status=@Status
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramColor1ID = new SqlParameter("@Color1ID", SqlDbType.NVarChar, 20);
            SqlParameter paramColor1Name = new SqlParameter("@Color1Name", SqlDbType.NVarChar, 200);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);

            paramSysNo.Value = oParam.SysNo;
            paramColor1ID.Value = oParam.Color1ID;
            paramColor1Name.Value = oParam.Color1Name;
            paramStatus.Value = oParam.Status;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramColor1ID);
            cmd.Parameters.Add(paramColor1Name);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int Update(Color2Info oParam)
        {
            string sql = @"UPDATE Color2 SET 
                            Color1SysNo=@Color1SysNo, 
                            Color2ID=@Color2ID, Color2Name=@Color2Name, 
                            Status=@Status
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramColor1SysNo = new SqlParameter("@Color1SysNo", SqlDbType.Int, 4);
            SqlParameter paramColor2ID = new SqlParameter("@Color2ID", SqlDbType.NVarChar, 20);
            SqlParameter paramColor2Name = new SqlParameter("@Color2Name", SqlDbType.NVarChar, 200);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);

            paramSysNo.Value = oParam.SysNo;
            paramColor1SysNo.Value = oParam.Color1SysNo;
            paramColor2ID.Value = oParam.Color2ID;
            paramColor2Name.Value = oParam.Color2Name;
            paramStatus.Value = oParam.Status;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramColor1SysNo);
            cmd.Parameters.Add(paramColor2ID);
            cmd.Parameters.Add(paramColor2Name);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
    }
}