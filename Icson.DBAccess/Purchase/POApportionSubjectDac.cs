using System;
using System.Data;
using System.Data.SqlClient;

using Icson.Utils;
using Icson.Objects.Purchase;

namespace Icson.DBAccess.Purchase
{
	/// <summary>
	/// Summary description for POApportionSubjectDac.
	/// </summary>
	public class POApportionSubjectDac
	{
		
		public POApportionSubjectDac()
		{
		}
		public int Insert(POApportionSubjectInfo oParam)
		{
			string sql = @"INSERT INTO PO_Apportion_Subject
                            (
                            SysNo, SubjectName, ListOrder, Status
                            )
                            VALUES (
                            @SysNo, @SubjectName, @ListOrder, @Status
                            )";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramSubjectName = new SqlParameter("@SubjectName", SqlDbType.NVarChar,200);
			SqlParameter paramListOrder = new SqlParameter("@ListOrder", SqlDbType.NVarChar,10);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);

			if ( oParam.SysNo != AppConst.IntNull)
				paramSysNo.Value = oParam.SysNo;
			else
				paramSysNo.Value = System.DBNull.Value;
			if ( oParam.SubjectName != AppConst.StringNull)
				paramSubjectName.Value = oParam.SubjectName;
			else
				paramSubjectName.Value = System.DBNull.Value;
			if ( oParam.ListOrder != AppConst.StringNull)
				paramListOrder.Value = oParam.ListOrder;
			else
				paramListOrder.Value = System.DBNull.Value;
			if ( oParam.Status != AppConst.IntNull)
				paramStatus.Value = oParam.Status;
			else
				paramStatus.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramSubjectName);
			cmd.Parameters.Add(paramListOrder);
			cmd.Parameters.Add(paramStatus);

			return SqlHelper.ExecuteNonQuery(cmd);
		}
		public int Update(POApportionSubjectInfo oParam)
		{
			string sql = @"UPDATE PO_Apportion_Subject SET 
                            SubjectName=@SubjectName, 
                            ListOrder=@ListOrder, Status=@Status
                            WHERE SysNo=@SysNo";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramSubjectName = new SqlParameter("@SubjectName", SqlDbType.NVarChar,200);
			SqlParameter paramListOrder = new SqlParameter("@ListOrder", SqlDbType.NVarChar,10);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);

			if ( oParam.SysNo != AppConst.IntNull)
				paramSysNo.Value = oParam.SysNo;
			else
				paramSysNo.Value = System.DBNull.Value;
			if ( oParam.SubjectName != AppConst.StringNull)
				paramSubjectName.Value = oParam.SubjectName;
			else
				paramSubjectName.Value = System.DBNull.Value;
			if ( oParam.ListOrder != AppConst.StringNull)
				paramListOrder.Value = oParam.ListOrder;
			else
				paramListOrder.Value = System.DBNull.Value;
			if ( oParam.Status != AppConst.IntNull)
				paramStatus.Value = oParam.Status;
			else
				paramStatus.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramSubjectName);
			cmd.Parameters.Add(paramListOrder);
			cmd.Parameters.Add(paramStatus);

			return SqlHelper.ExecuteNonQuery(cmd);
		}
	}
}
