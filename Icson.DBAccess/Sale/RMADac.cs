using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using Icson.Objects.Sale;
using Icson.Utils;

namespace Icson.DBAccess.Sale
{
	/// <summary>
	/// Summary description for RMADac.
	/// </summary>
	public class RMADac
	{
		public RMADac()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		
		public int InsertMaster(RMAInfo oParam)
		{
			string sql = @"INSERT INTO RMA_Master
                           (
                           SysNo, RMAID, SOSysNo, CustomerSysNo, 
                           Status, AuditUserSysNo, AuditTime, ReceiveUserSysNo, 
                           ReceiveTime, CloseUserSysNo, CloseTime, RMAUserSysNo, RMATime,
						   LastUserSysNo, UserChangedTime, RMANote, CCNote, 
						   SubmitInfo, ReceiveInfo, UserStatus
                           )
                           VALUES (
                           @SysNo, @RMAID, @SOSysNo, @CustomerSysNo, 
                           @Status, @AuditUserSysNo, @AuditTime, @ReceiveUserSysNo, 
                           @ReceiveTime, @CloseUserSysNo, @CloseTime, @RMAUserSysNo, @RMATime,
						   @LastUserSysNo, @UserChangedTime, @RMANote, @CCNote, 
                           @SubmitInfo, @ReceiveInfo, @UserStatus
                           )";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramRMAID = new SqlParameter("@RMAID", SqlDbType.NVarChar,20);
			SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int,4);
			SqlParameter paramCustomerSysNo = new SqlParameter("@CustomerSysNo", SqlDbType.Int,4);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);
			SqlParameter paramAuditUserSysNo = new SqlParameter("@AuditUserSysNo", SqlDbType.Int,4);
			SqlParameter paramAuditTime = new SqlParameter("@AuditTime", SqlDbType.DateTime);
			SqlParameter paramReceiveUserSysNo = new SqlParameter("@ReceiveUserSysNo", SqlDbType.Int,4);
			SqlParameter paramReceiveTime = new SqlParameter("@ReceiveTime", SqlDbType.DateTime);
			SqlParameter paramCloseUserSysNo = new SqlParameter("@CloseUserSysNo", SqlDbType.Int,4);
			SqlParameter paramCloseTime = new SqlParameter("@CloseTime", SqlDbType.DateTime);
			SqlParameter paramRMAUserSysNo = new SqlParameter("@RMAUserSysNo", SqlDbType.Int);
			SqlParameter paramRMATime = new SqlParameter("@RMATime", SqlDbType.DateTime);
			SqlParameter paramLastUserSysNo = new SqlParameter("@LastUserSysNo", SqlDbType.Int,4);
			SqlParameter paramUserChangedTime = new SqlParameter("@UserChangedTime", SqlDbType.DateTime);
			SqlParameter paramRMANote = new SqlParameter("@RMANote", SqlDbType.NVarChar,500);
			SqlParameter paramCCNote = new SqlParameter("@CCNote", SqlDbType.NVarChar,500);
			SqlParameter paramSubmitInfo = new SqlParameter("@SubmitInfo", SqlDbType.NText);
			SqlParameter paramReceiveInfo = new SqlParameter("@ReceiveInfo", SqlDbType.NText);
			SqlParameter paramUserStatus = new SqlParameter("@UserStatus", SqlDbType.Int,4);

			if ( oParam.SysNo != AppConst.IntNull)
				paramSysNo.Value = oParam.SysNo;
			else
				paramSysNo.Value = System.DBNull.Value;
			if ( oParam.RMAID != AppConst.StringNull)
				paramRMAID.Value = oParam.RMAID;
			else
				paramRMAID.Value = System.DBNull.Value;
			if ( oParam.SOSysNo != AppConst.IntNull)
				paramSOSysNo.Value = oParam.SOSysNo;
			else
				paramSOSysNo.Value = System.DBNull.Value;
			if ( oParam.CustomerSysNo != AppConst.IntNull)
				paramCustomerSysNo.Value = oParam.CustomerSysNo;
			else
				paramCustomerSysNo.Value = System.DBNull.Value;
			if ( oParam.Status != AppConst.IntNull)
				paramStatus.Value = oParam.Status;
			else
				paramStatus.Value = System.DBNull.Value;
			if ( oParam.AuditUserSysNo != AppConst.IntNull)
				paramAuditUserSysNo.Value = oParam.AuditUserSysNo;
			else
				paramAuditUserSysNo.Value = System.DBNull.Value;
			if ( oParam.AuditTime != AppConst.DateTimeNull)
				paramAuditTime.Value = oParam.AuditTime;
			else
				paramAuditTime.Value = System.DBNull.Value;
			if ( oParam.ReceiveUserSysNo != AppConst.IntNull)
				paramReceiveUserSysNo.Value = oParam.ReceiveUserSysNo;
			else
				paramReceiveUserSysNo.Value = System.DBNull.Value;
			if ( oParam.ReceiveTime != AppConst.DateTimeNull)
				paramReceiveTime.Value = oParam.ReceiveTime;
			else
				paramReceiveTime.Value = System.DBNull.Value;
			if ( oParam.CloseUserSysNo != AppConst.IntNull)
				paramCloseUserSysNo.Value = oParam.CloseUserSysNo;
			else
				paramCloseUserSysNo.Value = System.DBNull.Value;
			if ( oParam.CloseTime != AppConst.DateTimeNull)
				paramCloseTime.Value = oParam.CloseTime;
			else
				paramCloseTime.Value = System.DBNull.Value;
			if ( oParam.RMAUserSysNo != AppConst.IntNull)
				paramRMAUserSysNo.Value = oParam.RMAUserSysNo;
			else
				paramRMAUserSysNo.Value = System.DBNull.Value;
			if ( oParam.RMATime != AppConst.DateTimeNull)
				paramRMATime.Value = oParam.RMATime;
			else
				paramRMATime.Value = System.DBNull.Value;
			if ( oParam.LastUserSysNo != AppConst.IntNull)
				paramLastUserSysNo.Value = oParam.LastUserSysNo;
			else
				paramLastUserSysNo.Value = System.DBNull.Value;
			if ( oParam.UserChangedTime != AppConst.DateTimeNull)
				paramUserChangedTime.Value = oParam.UserChangedTime;
			else
				paramUserChangedTime.Value = System.DBNull.Value;
			if ( oParam.RMANote != AppConst.StringNull)
				paramRMANote.Value = oParam.RMANote;
			else
				paramRMANote.Value = System.DBNull.Value;
			if ( oParam.CCNote != AppConst.StringNull)
				paramCCNote.Value = oParam.CCNote;
			else
				paramCCNote.Value = System.DBNull.Value;
			if ( oParam.SubmitInfo != AppConst.StringNull)
				paramSubmitInfo.Value = oParam.SubmitInfo;
			else
				paramSubmitInfo.Value = System.DBNull.Value;
			if ( oParam.ReceiveInfo != AppConst.StringNull)
				paramReceiveInfo.Value = oParam.ReceiveInfo;
			else
				paramReceiveInfo.Value = System.DBNull.Value;
			if ( oParam.UserStatus != AppConst.IntNull)
				paramUserStatus.Value = oParam.UserStatus;
			else
				paramUserStatus.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramRMAID);
			cmd.Parameters.Add(paramSOSysNo);
			cmd.Parameters.Add(paramCustomerSysNo);
			cmd.Parameters.Add(paramStatus);
			cmd.Parameters.Add(paramAuditUserSysNo);
			cmd.Parameters.Add(paramAuditTime);
			cmd.Parameters.Add(paramReceiveUserSysNo);
			cmd.Parameters.Add(paramReceiveTime);
			cmd.Parameters.Add(paramCloseUserSysNo);
			cmd.Parameters.Add(paramCloseTime);
			cmd.Parameters.Add(paramRMAUserSysNo);
			cmd.Parameters.Add(paramRMATime);
			cmd.Parameters.Add(paramLastUserSysNo);
			cmd.Parameters.Add(paramUserChangedTime);
			cmd.Parameters.Add(paramRMANote);
			cmd.Parameters.Add(paramCCNote);
			cmd.Parameters.Add(paramSubmitInfo);
			cmd.Parameters.Add(paramReceiveInfo);
			cmd.Parameters.Add(paramUserStatus);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public int UpdateMaster(Hashtable paramHash)
		{
			StringBuilder sb = new StringBuilder(200);
			sb.Append("UPDATE RMA_Master SET ");

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

		public int InsertItem(RMAItemInfo oParam)
		{
			string sql = @"INSERT INTO RMA_Item
                           (
                           RMASysNo, ProductSysNo, RMAType, 
                           RMAQty, RMADesc
                           )
                           VALUES (
                           @RMASysNo, @ProductSysNo, @RMAType, 
                           @RMAQty, @RMADesc
                           )";

			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramRMASysNo = new SqlParameter("@RMASysNo", SqlDbType.Int,4);
			SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int,4);
			SqlParameter paramRMAType = new SqlParameter("@RMAType", SqlDbType.Int,4);
			SqlParameter paramRMAQty = new SqlParameter("@RMAQty", SqlDbType.Int,4);
			SqlParameter paramRMADesc = new SqlParameter("@RMADesc", SqlDbType.NVarChar,500);

			if ( oParam.RMASysNo != AppConst.IntNull)
				paramRMASysNo.Value = oParam.RMASysNo;
			else
				paramRMASysNo.Value = System.DBNull.Value;
			if ( oParam.ProductSysNo != AppConst.IntNull)
				paramProductSysNo.Value = oParam.ProductSysNo;
			else
				paramProductSysNo.Value = System.DBNull.Value;
			if ( oParam.RMAType != AppConst.IntNull)
				paramRMAType.Value = oParam.RMAType;
			else
				paramRMAType.Value = System.DBNull.Value;
			if ( oParam.RMAQty != AppConst.IntNull)
				paramRMAQty.Value = oParam.RMAQty;
			else
				paramRMAQty.Value = System.DBNull.Value;
			if ( oParam.RMADesc != AppConst.StringNull)
				paramRMADesc.Value = oParam.RMADesc;
			else
				paramRMADesc.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramRMASysNo);
			cmd.Parameters.Add(paramProductSysNo);
			cmd.Parameters.Add(paramRMAType);
			cmd.Parameters.Add(paramRMAQty);
			cmd.Parameters.Add(paramRMADesc);

			return SqlHelper.ExecuteNonQuery(cmd);
		}
		public int UpdateItem(RMAItemInfo oParam)
		{
			string sql = @"UPDATE RMA_Item SET 
                           RMASysNo=@RMASysNo, 
                           ProductSysNo=@ProductSysNo, RMAType=@RMAType, 
                           RMAQty=@RMAQty, RMADesc=@RMADesc
                           WHERE SysNo=@SysNo";

			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramRMASysNo = new SqlParameter("@RMASysNo", SqlDbType.Int,4);
			SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int,4);
			SqlParameter paramRMAType = new SqlParameter("@RMAType", SqlDbType.Int,4);
			SqlParameter paramRMAQty = new SqlParameter("@RMAQty", SqlDbType.Int,4);
			SqlParameter paramRMADesc = new SqlParameter("@RMADesc", SqlDbType.NVarChar,500);

			if ( oParam.SysNo != AppConst.IntNull)
				paramSysNo.Value = oParam.SysNo;
			else
				paramSysNo.Value = System.DBNull.Value;
			if ( oParam.RMASysNo != AppConst.IntNull)
				paramRMASysNo.Value = oParam.RMASysNo;
			else
				paramRMASysNo.Value = System.DBNull.Value;
			if ( oParam.ProductSysNo != AppConst.IntNull)
				paramProductSysNo.Value = oParam.ProductSysNo;
			else
				paramProductSysNo.Value = System.DBNull.Value;
			if ( oParam.RMAType != AppConst.IntNull)
				paramRMAType.Value = oParam.RMAType;
			else
				paramRMAType.Value = System.DBNull.Value;
			if ( oParam.RMAQty != AppConst.IntNull)
				paramRMAQty.Value = oParam.RMAQty;
			else
				paramRMAQty.Value = System.DBNull.Value;
			if ( oParam.RMADesc != AppConst.StringNull)
				paramRMADesc.Value = oParam.RMADesc;
			else
				paramRMADesc.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramRMASysNo);
			cmd.Parameters.Add(paramProductSysNo);
			cmd.Parameters.Add(paramRMAType);
			cmd.Parameters.Add(paramRMAQty);
			cmd.Parameters.Add(paramRMADesc);

			return SqlHelper.ExecuteNonQuery(cmd);
		}	
		public int DeleteItem(int sysno)
		{
			string sql = @"delete from rma_item where sysno = @SysNo";
			SqlCommand cmd = new SqlCommand(sql);
			SqlParameter paramSysNo = new SqlParameter("@SysNo",SqlDbType.Int,4);
			if( sysno!=AppConst.IntNull)
				paramSysNo.Value = sysno;
			cmd.Parameters.Add(paramSysNo);
			
			return SqlHelper.ExecuteNonQuery(cmd);
		}
	}
}
