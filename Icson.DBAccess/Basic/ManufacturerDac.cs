using System;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects.Basic;

namespace Icson.DBAccess.Basic
{
	/// <summary>
	/// Summary description for ManufacturerDac.
	/// </summary>
	public class ManufacturerDac
	{
		
		public ManufacturerDac()
		{
		}

		public int Insert(ManufacturerInfo oParam)
		{
			string sql = @"INSERT INTO Manufacturer
                            (
                            SysNo, ManufacturerID, ManufacturerName, BriefName, 
                            Note, Status
                            )
                            VALUES (
                            @SysNo, @ManufacturerID, @ManufacturerName, @BriefName, 
                            @Note, @Status
                            )";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramManufacturerID = new SqlParameter("@ManufacturerID", SqlDbType.NVarChar,20);
			SqlParameter paramManufacturerName = new SqlParameter("@ManufacturerName", SqlDbType.NVarChar,100);
			SqlParameter paramBriefName = new SqlParameter("@BriefName", SqlDbType.NVarChar,50);
			SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.NVarChar,2000);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);

			paramSysNo.Value = oParam.SysNo;
			paramManufacturerID.Value = oParam.ManufacturerID;
			paramManufacturerName.Value = oParam.ManufacturerName;
			paramBriefName.Value = oParam.BriefName;
			if ( oParam.Note != AppConst.StringNull)
				paramNote.Value = oParam.Note;
			else
				paramNote.Value = System.DBNull.Value;
			paramStatus.Value = oParam.Status;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramManufacturerID);
			cmd.Parameters.Add(paramManufacturerName);
			cmd.Parameters.Add(paramBriefName);
			cmd.Parameters.Add(paramNote);
			cmd.Parameters.Add(paramStatus);

			return SqlHelper.ExecuteNonQuery(cmd);
		}
		public int Update(ManufacturerInfo oParam)
		{
			string sql = @"UPDATE Manufacturer SET 
                            ManufacturerID=@ManufacturerID, 
                            ManufacturerName=@ManufacturerName, BriefName=@BriefName, 
                            Note=@Note, Status=@Status
                            WHERE SysNo=@SysNo";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramManufacturerID = new SqlParameter("@ManufacturerID", SqlDbType.NVarChar,20);
			SqlParameter paramManufacturerName = new SqlParameter("@ManufacturerName", SqlDbType.NVarChar,100);
			SqlParameter paramBriefName = new SqlParameter("@BriefName", SqlDbType.NVarChar,50);
			SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.NVarChar,2000);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);

			paramSysNo.Value = oParam.SysNo;
			paramManufacturerID.Value = oParam.ManufacturerID;
			paramManufacturerName.Value = oParam.ManufacturerName;
			paramBriefName.Value = oParam.BriefName;
			if ( oParam.Note != AppConst.StringNull)
				paramNote.Value = oParam.Note;
			else
				paramNote.Value = System.DBNull.Value;
			paramStatus.Value = oParam.Status;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramManufacturerID);
			cmd.Parameters.Add(paramManufacturerName);
			cmd.Parameters.Add(paramBriefName);
			cmd.Parameters.Add(paramNote);
			cmd.Parameters.Add(paramStatus);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

        public int Deletes(string SysNos)
        {
            string sql = "delete from Manufacturer where sysno in(" + SysNos + ")";
            return SqlHelper.ExecuteNonQuery(sql);
        }
	}
}
