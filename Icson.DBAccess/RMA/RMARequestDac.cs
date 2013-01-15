using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Icson.Objects.RMA;
using Icson.Utils;

namespace Icson.DBAccess.RMA
{
	/// <summary>
	/// Summary description for RMARequestDac.
	/// </summary>
	public class RMARequestDac
	{
		
		public RMARequestDac()
		{
			
		}
        public int InsertRequest(RMARequestInfo oParam)
        {
            string sql = @"INSERT INTO RMA_Request
                            (
                            SysNo, SOSysNo, RequestID, CreateTime, 
                            CustomerSysNo, CreateUserSysNo, CanDoorGet, DoorGetFee, 
                            Address, Contact, Phone, Zip, 
                            RecvTime, RecvUserSysNo, Note, Memo, 
                            Status, ETakeDate, AreaSysNo, CustomerSendTime, 
                            IsRejectRMA, FreightUserSysNo, SetDeliveryManTime, IsRevertAddress, 
                            RevertAddress, RevertAreaSysNo, RevertZip,RevertContact,RevertContactPhone,ReceiveType
                            )
                            VALUES (
                            @SysNo, @SOSysNo, @RequestID, @CreateTime, 
                            @CustomerSysNo, @CreateUserSysNo, @CanDoorGet, @DoorGetFee, 
                            @Address, @Contact, @Phone, @Zip, 
                            @RecvTime, @RecvUserSysNo, @Note, @Memo, 
                            @Status, @ETakeDate, @AreaSysNo, @CustomerSendTime, 
                            @IsRejectRMA, @FreightUserSysNo, @SetDeliveryManTime, @IsRevertAddress, 
                            @RevertAddress, @RevertAreaSysNo, @RevertZip,@RevertContact,@RevertContactPhone,@ReceiveType
                            )";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int, 4);
            SqlParameter paramRequestID = new SqlParameter("@RequestID", SqlDbType.NVarChar, 10);
            SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
            SqlParameter paramCustomerSysNo = new SqlParameter("@CustomerSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCanDoorGet = new SqlParameter("@CanDoorGet", SqlDbType.Int, 4);
            SqlParameter paramDoorGetFee = new SqlParameter("@DoorGetFee", SqlDbType.Decimal, 9);
            SqlParameter paramAddress = new SqlParameter("@Address", SqlDbType.NVarChar, 200);
            SqlParameter paramContact = new SqlParameter("@Contact", SqlDbType.NVarChar, 50);
            SqlParameter paramPhone = new SqlParameter("@Phone", SqlDbType.NVarChar, 50);
            SqlParameter paramZip = new SqlParameter("@Zip", SqlDbType.VarChar, 50);
            SqlParameter paramRecvTime = new SqlParameter("@RecvTime", SqlDbType.DateTime);
            SqlParameter paramRecvUserSysNo = new SqlParameter("@RecvUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.NVarChar, 200);
            SqlParameter paramMemo = new SqlParameter("@Memo", SqlDbType.NVarChar, 200);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramETakeDate = new SqlParameter("@ETakeDate", SqlDbType.DateTime);
            SqlParameter paramAreaSysNo = new SqlParameter("@AreaSysNo", SqlDbType.Int, 4);
            SqlParameter paramCustomerSendTime = new SqlParameter("@CustomerSendTime", SqlDbType.DateTime);
            SqlParameter paramIsRejectRMA = new SqlParameter("@IsRejectRMA", SqlDbType.Int, 4);
            SqlParameter paramFreightUserSysNo = new SqlParameter("@FreightUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramSetDeliveryManTime = new SqlParameter("@SetDeliveryManTime", SqlDbType.DateTime);
            SqlParameter paramIsRevertAddress = new SqlParameter("@IsRevertAddress", SqlDbType.Int, 4);
            SqlParameter paramRevertAddress = new SqlParameter("@RevertAddress", SqlDbType.NVarChar, 200);
            SqlParameter paramRevertAreaSysNo = new SqlParameter("@RevertAreaSysNo", SqlDbType.Int, 4);
            SqlParameter paramRevertZip = new SqlParameter("@RevertZip", SqlDbType.NVarChar, 50);
            SqlParameter paramRevertContact = new SqlParameter("@RevertContact", SqlDbType.NVarChar, 50);
            SqlParameter paramRevertContactPhone= new SqlParameter("@RevertContactPhone", SqlDbType.NVarChar, 50);
            SqlParameter paramReceiveType = new SqlParameter("@ReceiveType", SqlDbType.Int, 4);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.SOSysNo != AppConst.IntNull)
                paramSOSysNo.Value = oParam.SOSysNo;
            else
                paramSOSysNo.Value = System.DBNull.Value;
            if (oParam.RequestID != AppConst.StringNull)
                paramRequestID.Value = oParam.RequestID;
            else
                paramRequestID.Value = System.DBNull.Value;
            if (oParam.CreateTime != AppConst.DateTimeNull)
                paramCreateTime.Value = oParam.CreateTime;
            else
                paramCreateTime.Value = System.DBNull.Value;
            if (oParam.CustomerSysNo != AppConst.IntNull)
                paramCustomerSysNo.Value = oParam.CustomerSysNo;
            else
                paramCustomerSysNo.Value = System.DBNull.Value;
            if (oParam.CreateUserSysNo != AppConst.IntNull)
                paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
            else
                paramCreateUserSysNo.Value = System.DBNull.Value;
            if (oParam.CanDoorGet != AppConst.IntNull)
                paramCanDoorGet.Value = oParam.CanDoorGet;
            else
                paramCanDoorGet.Value = System.DBNull.Value;
            if (oParam.DoorGetFee != AppConst.DecimalNull)
                paramDoorGetFee.Value = oParam.DoorGetFee;
            else
                paramDoorGetFee.Value = System.DBNull.Value;
            if (oParam.Address != AppConst.StringNull)
                paramAddress.Value = oParam.Address;
            else
                paramAddress.Value = System.DBNull.Value;
            if (oParam.Contact != AppConst.StringNull)
                paramContact.Value = oParam.Contact;
            else
                paramContact.Value = System.DBNull.Value;
            if (oParam.Phone != AppConst.StringNull)
                paramPhone.Value = oParam.Phone;
            else
                paramPhone.Value = System.DBNull.Value;
            if (oParam.Zip != AppConst.StringNull)
                paramZip.Value = oParam.Zip;
            else
                paramZip.Value = System.DBNull.Value;
            if (oParam.RecvTime != AppConst.DateTimeNull)
                paramRecvTime.Value = oParam.RecvTime;
            else
                paramRecvTime.Value = System.DBNull.Value;
            if (oParam.RecvUserSysNo != AppConst.IntNull)
                paramRecvUserSysNo.Value = oParam.RecvUserSysNo;
            else
                paramRecvUserSysNo.Value = System.DBNull.Value;
            if (oParam.Note != AppConst.StringNull)
                paramNote.Value = oParam.Note;
            else
                paramNote.Value = System.DBNull.Value;
            if (oParam.Memo != AppConst.StringNull)
                paramMemo.Value = oParam.Memo;
            else
                paramMemo.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;
            if (oParam.ETakeDate != AppConst.DateTimeNull)
                paramETakeDate.Value = oParam.ETakeDate;
            else
                paramETakeDate.Value = System.DBNull.Value;
            if (oParam.AreaSysNo != AppConst.IntNull)
                paramAreaSysNo.Value = oParam.AreaSysNo;
            else
                paramAreaSysNo.Value = System.DBNull.Value;
            if (oParam.CustomerSendTime != AppConst.DateTimeNull)
                paramCustomerSendTime.Value = oParam.CustomerSendTime;
            else
                paramCustomerSendTime.Value = System.DBNull.Value;
            if (oParam.IsRejectRMA != AppConst.IntNull)
                paramIsRejectRMA.Value = oParam.IsRejectRMA;
            else
                paramIsRejectRMA.Value = System.DBNull.Value;
            if (oParam.FreightUserSysNo != AppConst.IntNull)
                paramFreightUserSysNo.Value = oParam.FreightUserSysNo;
            else
                paramFreightUserSysNo.Value = System.DBNull.Value;
            if (oParam.SetDeliveryManTime != AppConst.DateTimeNull)
                paramSetDeliveryManTime.Value = oParam.SetDeliveryManTime;
            else
                paramSetDeliveryManTime.Value = System.DBNull.Value;
            if (oParam.IsRevertAddress != AppConst.IntNull)
                paramIsRevertAddress.Value = oParam.IsRevertAddress;
            else
                paramIsRevertAddress.Value = System.DBNull.Value;
            if (oParam.RevertAddress != AppConst.StringNull)
                paramRevertAddress.Value = oParam.RevertAddress;
            else
                paramRevertAddress.Value = System.DBNull.Value;
            if (oParam.RevertAreaSysNo != AppConst.IntNull)
                paramRevertAreaSysNo.Value = oParam.RevertAreaSysNo;
            else
                paramRevertAreaSysNo.Value = System.DBNull.Value;
            if (oParam.RevertZip != AppConst.StringNull)
                paramRevertZip.Value = oParam.RevertZip;
            else
                paramRevertZip.Value = System.DBNull.Value;
            if (oParam.RevertContact != AppConst.StringNull)
                paramRevertContact.Value = oParam.RevertContact;
            else
                paramRevertContact.Value = System.DBNull.Value;
            if (oParam.RevertContactPhone != AppConst.StringNull)
                paramRevertContactPhone.Value = System.DBNull.Value;
            if (oParam.ReceiveType != AppConst.IntNull)
                paramReceiveType.Value = oParam.ReceiveType;
            else
                paramReceiveType.Value = System.DBNull.Value;
            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramSOSysNo);
            cmd.Parameters.Add(paramRequestID);
            cmd.Parameters.Add(paramCreateTime);
            cmd.Parameters.Add(paramCustomerSysNo);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramCanDoorGet);
            cmd.Parameters.Add(paramDoorGetFee);
            cmd.Parameters.Add(paramAddress);
            cmd.Parameters.Add(paramContact);
            cmd.Parameters.Add(paramPhone);
            cmd.Parameters.Add(paramZip);
            cmd.Parameters.Add(paramRecvTime);
            cmd.Parameters.Add(paramRecvUserSysNo);
            cmd.Parameters.Add(paramNote);
            cmd.Parameters.Add(paramMemo);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramETakeDate);
            cmd.Parameters.Add(paramAreaSysNo);
            cmd.Parameters.Add(paramCustomerSendTime);
            cmd.Parameters.Add(paramIsRejectRMA);
            cmd.Parameters.Add(paramFreightUserSysNo);
            cmd.Parameters.Add(paramSetDeliveryManTime);
            cmd.Parameters.Add(paramIsRevertAddress);
            cmd.Parameters.Add(paramRevertAddress);
            cmd.Parameters.Add(paramRevertAreaSysNo);
            cmd.Parameters.Add(paramRevertZip);
            cmd.Parameters.Add(paramRevertContact);
            cmd.Parameters.Add(paramRevertContactPhone);
            cmd.Parameters.Add(paramReceiveType);

            return SqlHelper.ExecuteNonQuery(cmd);
        }

		public int UpdateRequest(Hashtable paramHash)
		{
			StringBuilder sb = new StringBuilder(200);
			sb.Append("UPDATE RMA_Request SET ");

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
						if((DateTime)item == AppConst.DateTimeNull)
							sb.Append(key).Append(" = null ");
						else
						    sb.Append(key).Append(" = cast(").Append( Util.ToSqlString(((DateTime)item).ToString(AppConst.DateFormatLong))).Append(" as DateTime)");
					}
				}
			}

			sb.Append(" WHERE SysNo=").Append(paramHash["SysNo"]);

			return SqlHelper.ExecuteNonQuery(sb.ToString());
		}

		public int InsertRequestItem(RMARequestItemInfo oParam)
		{
			string sql = @"INSERT INTO RMA_Request_Item
                            (
                             RequestSysNo, RegisterSysNo
                            )
                            VALUES (
                            @RequestSysNo, @RegisterSysNo
                            );set @SysNo = SCOPE_IDENTITY();";			
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramRequestSysNo = new SqlParameter("@RequestSysNo", SqlDbType.Int,4);
			SqlParameter paramRegisterSysNo = new SqlParameter("@RegisterSysNo", SqlDbType.Int,4);

			paramSysNo.Direction = ParameterDirection.Output;

			if ( oParam.RequestSysNo != AppConst.IntNull)
				paramRequestSysNo.Value = oParam.RequestSysNo;
			else
				paramRequestSysNo.Value = System.DBNull.Value;
			if ( oParam.RegisterSysNo != AppConst.IntNull)
				paramRegisterSysNo.Value = oParam.RegisterSysNo;
			else
				paramRegisterSysNo.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramRequestSysNo);
			cmd.Parameters.Add(paramRegisterSysNo);

			return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
		}
		public int DeleteItem(int itemSysNo)
		{
			string sql = "delete from RMA_Request_Item where sysno = " + itemSysNo;
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public int CancelReceive(int sysno)
		{
			string sql = "update RMA_Request set RecvTime = null , RecvUserSysNo = null where sysno = " + sysno ;
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);
		}
	}
}