using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Text;

using Icson.Utils;
using Icson.Objects.Online;

namespace Icson.DBAccess.Online
{
	/// <summary>
	/// Summary description for PollDac.
	/// </summary>
	public class PollDac
	{
		
		public PollDac()
		{
		}

		public int Insert(PollInfo oParam)
		{
			string sql = @"INSERT INTO Poll
                            (
                            PollName, PollCount, Status
                            )
                            VALUES (
                            @PollName, @PollCount, @Status
                            );set @SysNo = SCOPE_IDENTITY();";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramPollName = new SqlParameter("@PollName", SqlDbType.NVarChar,200);
			SqlParameter paramPollCount = new SqlParameter("@PollCount", SqlDbType.Int,4);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);

			paramSysNo.Direction = ParameterDirection.Output;

			if ( oParam.PollName != AppConst.StringNull)
				paramPollName.Value = oParam.PollName;
			else
				paramPollName.Value = System.DBNull.Value;
			if ( oParam.PollCount != AppConst.IntNull)
				paramPollCount.Value = oParam.PollCount;
			else
				paramPollCount.Value = System.DBNull.Value;
			if ( oParam.Status != AppConst.IntNull)
				paramStatus.Value = oParam.Status;
			else
				paramStatus.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramPollName);
			cmd.Parameters.Add(paramPollCount);
			cmd.Parameters.Add(paramStatus);

			return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);

		}

		public int Update(Hashtable paramHash)
		{
			StringBuilder sb = new StringBuilder(200);
			sb.Append("UPDATE Poll SET ");

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
		public int InsertItem(PollItemInfo oParam)
		{
			string sql = @"INSERT INTO Poll_Item
                            (
                            PollSysNo, ItemName, ItemCount
                            )
                            VALUES (
                            @PollSysNo, @ItemName, @ItemCount
                            );set @SysNo = SCOPE_IDENTITY();";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramPollSysNo = new SqlParameter("@PollSysNo", SqlDbType.Int,4);
			SqlParameter paramItemName = new SqlParameter("@ItemName", SqlDbType.NVarChar,200);
			SqlParameter paramItemCount = new SqlParameter("@ItemCount", SqlDbType.Int,4);

			paramSysNo.Direction = ParameterDirection.Output;

			if ( oParam.PollSysNo != AppConst.IntNull)
				paramPollSysNo.Value = oParam.PollSysNo;
			else
				paramPollSysNo.Value = System.DBNull.Value;
			if ( oParam.ItemName != AppConst.StringNull)
				paramItemName.Value = oParam.ItemName;
			else
				paramItemName.Value = System.DBNull.Value;
			if ( oParam.ItemCount != AppConst.IntNull)
				paramItemCount.Value = oParam.ItemCount;
			else
				paramItemCount.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramPollSysNo);
			cmd.Parameters.Add(paramItemName);
			cmd.Parameters.Add(paramItemCount);

			return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);

		}
		public int UpdateItem(Hashtable paramHash)
		{
			StringBuilder sb = new StringBuilder(200);
			sb.Append("UPDATE Poll_Item SET ");

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
		public int DeleteItem(int itemSysNo)
		{
			string sql = "delete from poll_item where sysno = " + itemSysNo;
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);
		}
		public int DoPoll(int pollSysNo, int itemSysNo)
		{
			string sql = "update poll set pollcount = pollcount+1 where sysno =" + pollSysNo;
			sql += ";update poll_item set itemcount = itemcount+1 where sysno =" + itemSysNo;

			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);
		}
	}
}
