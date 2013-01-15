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
	/// Summary description for FeedBackDac.
	/// </summary>
	public class FeedBackDac
	{
		
		public FeedBackDac()
		{
		}
		public int Insert(FeedBackInfo oParam)
		{
			string sql = @"INSERT INTO FeedBack
                            (
                            CustomerSysNo,
                            Subject, Suggest, NickName, 
                            Email, Phone, Memo, Note, 
                            CreateTime, UpdateTime,UpdateUserSysNo, Status,Sosysno
                            )
                            VALUES (
                            @CustomerSysNo,
                            @Subject, @Suggest, @NickName, 
                            @Email, @Phone, @Memo, @Note, 
                            @CreateTime, @UpdateTime,@UpdateUserSysNo, @Status,@Sosysno
                            );set @SysNo = SCOPE_IDENTITY();";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
            SqlParameter paramCustomerSysNo = new SqlParameter("@CustomerSysNo", SqlDbType.Int, 4);
			SqlParameter paramSubject = new SqlParameter("@Subject", SqlDbType.NVarChar,250);
			SqlParameter paramSuggest = new SqlParameter("@Suggest", SqlDbType.NVarChar,2000);
			SqlParameter paramNickName = new SqlParameter("@NickName", SqlDbType.NVarChar,50);
			SqlParameter paramEmail = new SqlParameter("@Email", SqlDbType.NVarChar,50);
			SqlParameter paramPhone = new SqlParameter("@Phone", SqlDbType.NVarChar,50);
			SqlParameter paramMemo = new SqlParameter("@Memo", SqlDbType.NVarChar,500);
			SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.NVarChar,500);
			SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
			SqlParameter paramUpdateTime = new SqlParameter("@UpdateTime", SqlDbType.DateTime);
            SqlParameter paramUpdateUserSysNo = new SqlParameter("@UpdateUserSysNo", SqlDbType.Int,4);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);
            SqlParameter paramSosysno = new SqlParameter("@Sosysno", SqlDbType.Int, 4);
			paramSysNo.Direction = ParameterDirection.Output;

            if (oParam.CustomerSysNo != AppConst.IntNull)
                paramCustomerSysNo.Value = oParam.CustomerSysNo;
            else
                paramCustomerSysNo.Value = System.DBNull.Value;

			if ( oParam.Subject != AppConst.StringNull)
				paramSubject.Value = oParam.Subject;
			else
				paramSubject.Value = System.DBNull.Value;
			if ( oParam.Suggest != AppConst.StringNull)
				paramSuggest.Value = oParam.Suggest;
			else
				paramSuggest.Value = System.DBNull.Value;
			if ( oParam.NickName != AppConst.StringNull)
				paramNickName.Value = oParam.NickName;
			else
				paramNickName.Value = System.DBNull.Value;
			if ( oParam.Email != AppConst.StringNull)
				paramEmail.Value = oParam.Email;
			else
				paramEmail.Value = System.DBNull.Value;
			if ( oParam.Phone != AppConst.StringNull)
				paramPhone.Value = oParam.Phone;
			else
				paramPhone.Value = System.DBNull.Value;
			if ( oParam.Memo != AppConst.StringNull)
				paramMemo.Value = oParam.Memo;
			else
				paramMemo.Value = System.DBNull.Value;
			if ( oParam.Note != AppConst.StringNull)
				paramNote.Value = oParam.Note;
			else
				paramNote.Value = System.DBNull.Value;
			if ( oParam.CreateTime != AppConst.DateTimeNull)
				paramCreateTime.Value = oParam.CreateTime;
			else
				paramCreateTime.Value = System.DBNull.Value;
			if ( oParam.UpdateTime != AppConst.DateTimeNull)
				paramUpdateTime.Value = oParam.UpdateTime;
			else
				paramUpdateTime.Value = System.DBNull.Value;
            if (oParam.UpdateUserSysNo != AppConst.IntNull)
                paramUpdateUserSysNo.Value = oParam.UpdateUserSysNo;
            else
                paramUpdateUserSysNo.Value = System.DBNull.Value;
			if ( oParam.Status != AppConst.IntNull)
				paramStatus.Value = oParam.Status;
			else
				paramStatus.Value = System.DBNull.Value;
            if (oParam.Sosysno != AppConst.IntNull)
                paramSosysno.Value = oParam.Sosysno;
			else
                paramSosysno.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramCustomerSysNo);
			cmd.Parameters.Add(paramSubject);
			cmd.Parameters.Add(paramSuggest);
			cmd.Parameters.Add(paramNickName);
			cmd.Parameters.Add(paramEmail);
			cmd.Parameters.Add(paramPhone);
			cmd.Parameters.Add(paramMemo);
			cmd.Parameters.Add(paramNote);
			cmd.Parameters.Add(paramCreateTime);
			cmd.Parameters.Add(paramUpdateTime);
            cmd.Parameters.Add(paramUpdateUserSysNo);
			cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramSosysno);
			return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);

		}

		public int Update(Hashtable paramHash)
		{
			StringBuilder sb = new StringBuilder(200);
			sb.Append("UPDATE FeedBack SET ");

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
