using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Text;

using Icson.Utils;
using Icson.Objects.Finance;

namespace Icson.DBAccess.Finance
{
	/// <summary>
	/// Summary description for NetPayDac.
	/// </summary>
	public class NetPayDac
	{
		
		public NetPayDac()
		{
		}
		public int Insert(NetPayInfo oParam)
		{
			string sql = @"INSERT INTO Finance_NetPay
                            (
                            SOSysNo, PayTypeSysNo, PayAmount, 
                            Source, InputTime, InputUserSysNo, ApproveUserSysNo, 
                            ApproveTime, Note, Status
                            )
                            VALUES (
                            @SOSysNo, @PayTypeSysNo, @PayAmount, 
                            @Source, @InputTime, @InputUserSysNo, @ApproveUserSysNo, 
                            @ApproveTime, @Note, @Status
                            );set @SysNo = SCOPE_IDENTITY();";

			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int,4);
			SqlParameter paramPayTypeSysNo = new SqlParameter("@PayTypeSysNo", SqlDbType.Int,4);
			SqlParameter paramPayAmount = new SqlParameter("@PayAmount", SqlDbType.Decimal,9);
			SqlParameter paramSource = new SqlParameter("@Source", SqlDbType.Int,4);
			SqlParameter paramInputTime = new SqlParameter("@InputTime", SqlDbType.DateTime);
			SqlParameter paramInputUserSysNo = new SqlParameter("@InputUserSysNo", SqlDbType.Int,4);
			SqlParameter paramApproveUserSysNo = new SqlParameter("@ApproveUserSysNo", SqlDbType.Int,4);
			SqlParameter paramApproveTime = new SqlParameter("@ApproveTime", SqlDbType.DateTime);
			SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.NVarChar,200);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);

			paramSysNo.Direction = ParameterDirection.Output;

			if ( oParam.SOSysNo != AppConst.IntNull)
				paramSOSysNo.Value = oParam.SOSysNo;
			else
				paramSOSysNo.Value = System.DBNull.Value;
			if ( oParam.PayTypeSysNo != AppConst.IntNull)
				paramPayTypeSysNo.Value = oParam.PayTypeSysNo;
			else
				paramPayTypeSysNo.Value = System.DBNull.Value;
			if ( oParam.PayAmount != AppConst.DecimalNull)
				paramPayAmount.Value = oParam.PayAmount;
			else
				paramPayAmount.Value = System.DBNull.Value;
			if ( oParam.Source != AppConst.IntNull)
				paramSource.Value = oParam.Source;
			else
				paramSource.Value = System.DBNull.Value;
			if ( oParam.InputTime != AppConst.DateTimeNull)
				paramInputTime.Value = oParam.InputTime;
			else
				paramInputTime.Value = System.DBNull.Value;
			if ( oParam.InputUserSysNo != AppConst.IntNull)
				paramInputUserSysNo.Value = oParam.InputUserSysNo;
			else
				paramInputUserSysNo.Value = System.DBNull.Value;
			if ( oParam.ApproveUserSysNo != AppConst.IntNull)
				paramApproveUserSysNo.Value = oParam.ApproveUserSysNo;
			else
				paramApproveUserSysNo.Value = System.DBNull.Value;
			if ( oParam.ApproveTime != AppConst.DateTimeNull)
				paramApproveTime.Value = oParam.ApproveTime;
			else
				paramApproveTime.Value = System.DBNull.Value;
			if ( oParam.Note != AppConst.StringNull)
				paramNote.Value = oParam.Note;
			else
				paramNote.Value = System.DBNull.Value;
			if ( oParam.Status != AppConst.IntNull)
				paramStatus.Value = oParam.Status;
			else
				paramStatus.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramSOSysNo);
			cmd.Parameters.Add(paramPayTypeSysNo);
			cmd.Parameters.Add(paramPayAmount);
			cmd.Parameters.Add(paramSource);
			cmd.Parameters.Add(paramInputTime);
			cmd.Parameters.Add(paramInputUserSysNo);
			cmd.Parameters.Add(paramApproveUserSysNo);
			cmd.Parameters.Add(paramApproveTime);
			cmd.Parameters.Add(paramNote);
			cmd.Parameters.Add(paramStatus);

			return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);

		}

		public int Update(Hashtable paramHash)
		{
			StringBuilder sb = new StringBuilder(200);
			sb.Append("UPDATE Finance_NetPay SET ");

			if ( paramHash != null && paramHash.Count != 0 )
			{
				int index = 0;
				foreach(string key in paramHash.Keys)
				{
					object item = paramHash[key];
					if ( key.ToLower()=="sysno" )
						continue;

					if ( index != 0 )
						sb.Append(",");
					index++;

					
					if (item is int || item is decimal)
					{
						sb.Append(key).Append("=").Append(item.ToString());
					}
					else if (item is string)
					{
						sb.Append(key).Append(" = ").Append(Util.ToSqlString(item.ToString()));
					}
					else if (item is DateTime)
					{
						sb.Append(key).Append(" = cast(").Append( Util.ToSqlString(((DateTime)item).ToString(AppConst.DateFormatLong))).Append(" as DateTime)");
					}
				}
			}

			sb.Append(" WHERE SysNo=").Append(paramHash["SysNo"]);

			return SqlHelper.ExecuteNonQuery(sb.ToString());
		}
	}
}
