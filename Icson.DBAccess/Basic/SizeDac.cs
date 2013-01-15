using System;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects.Basic;

namespace Icson.DBAccess.Basic
{
    /// <summary>
    /// Summary description for SizeDac.
    /// </summary>
    public class SizeDac
    {

        public SizeDac()
        {
        }

        public int Insert(Size1Info oParam)
        {
            string sql = @"INSERT INTO Size1
                            (
                             Size1ID, Size1Name, Status
                            )
                            VALUES (
                             @Size1ID, @Size1Name, @Status
                            )";
            SqlCommand cmd = new SqlCommand(sql);

            
            SqlParameter paramSize1ID = new SqlParameter("@Size1ID", SqlDbType.NVarChar, 20);
            SqlParameter paramSize1Name = new SqlParameter("@Size1Name", SqlDbType.NVarChar, 200);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);

          
            paramSize1ID.Value = oParam.Size1ID;
            paramSize1Name.Value = oParam.Size1Name;
            paramStatus.Value = oParam.Status;

          
            cmd.Parameters.Add(paramSize1ID);
            cmd.Parameters.Add(paramSize1Name);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
        public int Insert(Size2Info oParam)
        {
            string sql = @"INSERT INTO Size2
                            (
                             Size1SysNo, Size2ID, Size2Name, 
                            Status
                            )
                            VALUES (
                             @Size1SysNo, @Size2ID, @Size2Name, 
                            @Status
                            )";
            SqlCommand cmd = new SqlCommand(sql);

           
            SqlParameter paramSize1SysNo = new SqlParameter("@Size1SysNo", SqlDbType.Int, 4);
            SqlParameter paramSize2ID = new SqlParameter("@Size2ID", SqlDbType.NVarChar, 20);
            SqlParameter paramSize2Name = new SqlParameter("@Size2Name", SqlDbType.NVarChar, 200);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);

          
            paramSize1SysNo.Value = oParam.Size1SysNo;
            paramSize2ID.Value = oParam.Size2ID;
            paramSize2Name.Value = oParam.Size2Name;
            paramStatus.Value = oParam.Status;

            
            cmd.Parameters.Add(paramSize1SysNo);
            cmd.Parameters.Add(paramSize2ID);
            cmd.Parameters.Add(paramSize2Name);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int Update(Size1Info oParam)
        {
            string sql = @"UPDATE Size1 SET 
                            Size1ID=@Size1ID, 
                            Size1Name=@Size1Name, Status=@Status
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramSize1ID = new SqlParameter("@Size1ID", SqlDbType.NVarChar, 20);
            SqlParameter paramSize1Name = new SqlParameter("@Size1Name", SqlDbType.NVarChar, 200);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);

            paramSysNo.Value = oParam.SysNo;
            paramSize1ID.Value = oParam.Size1ID;
            paramSize1Name.Value = oParam.Size1Name;
            paramStatus.Value = oParam.Status;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramSize1ID);
            cmd.Parameters.Add(paramSize1Name);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

        public int Update(Size2Info oParam)
        {
            string sql = @"UPDATE Size2 SET 
                            Size1SysNo=@Size1SysNo, 
                            Size2ID=@Size2ID, Size2Name=@Size2Name, 
                            Status=@Status
                            WHERE SysNo=@SysNo";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramSize1SysNo = new SqlParameter("@Size1SysNo", SqlDbType.Int, 4);
            SqlParameter paramSize2ID = new SqlParameter("@Size2ID", SqlDbType.NVarChar, 20);
            SqlParameter paramSize2Name = new SqlParameter("@Size2Name", SqlDbType.NVarChar, 200);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);

            paramSysNo.Value = oParam.SysNo;
            paramSize1SysNo.Value = oParam.Size1SysNo;
            paramSize2ID.Value = oParam.Size2ID;
            paramSize2Name.Value = oParam.Size2Name;
            paramStatus.Value = oParam.Status;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramSize1SysNo);
            cmd.Parameters.Add(paramSize2ID);
            cmd.Parameters.Add(paramSize2Name);
            cmd.Parameters.Add(paramStatus);

            return SqlHelper.ExecuteNonQuery(cmd);
        }
    }
}