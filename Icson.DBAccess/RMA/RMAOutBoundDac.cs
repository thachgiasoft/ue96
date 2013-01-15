using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Text;

using Icson.Utils;
using Icson.Objects.RMA;

namespace Icson.DBAccess.RMA
{
	/// <summary>
	/// Summary description for RMAOutBoundDac.
	/// </summary>
	public class RMAOutBoundDac
	{
		
		public RMAOutBoundDac()
		{
		}


        public int InsertMaster(RMAOutBoundInfo oParam)
        {
            string sql = @"INSERT INTO RMA_OutBound
                            (
                            SysNo, OutBoundID, VendorSysNo, CreateTime, 
                            CreateUserSysNo, VendorName, Zip, Address, 
                            Contact, Phone, OutTime, OutUserSysNo, 
                            Note, Status, EOutBoundDate, EResponseDate, 
                            AreaSysNo, OutBoundInvoiceQty, ShipType, PackageID, 
                            CheckQtyUserSysNo, CheckQtyTime, FreightUserSysNo, SetDeliveryManTime
                            )
                            VALUES (
                            @SysNo, @OutBoundID, @VendorSysNo, @CreateTime, 
                            @CreateUserSysNo, @VendorName, @Zip, @Address, 
                            @Contact, @Phone, @OutTime, @OutUserSysNo, 
                            @Note, @Status, @EOutBoundDate, @EResponseDate, 
                            @AreaSysNo, @OutBoundInvoiceQty, @ShipType, @PackageID, 
                            @CheckQtyUserSysNo, @CheckQtyTime, @FreightUserSysNo, @SetDeliveryManTime
                            )";
            SqlCommand cmd = new SqlCommand(sql);

            SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
            SqlParameter paramOutBoundID = new SqlParameter("@OutBoundID", SqlDbType.NVarChar, 20);
            SqlParameter paramVendorSysNo = new SqlParameter("@VendorSysNo", SqlDbType.Int, 4);
            SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
            SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramVendorName = new SqlParameter("@VendorName", SqlDbType.NVarChar, 100);
            SqlParameter paramZip = new SqlParameter("@Zip", SqlDbType.NVarChar, 10);
            SqlParameter paramAddress = new SqlParameter("@Address", SqlDbType.NVarChar, 200);
            SqlParameter paramContact = new SqlParameter("@Contact", SqlDbType.NVarChar, 50);
            SqlParameter paramPhone = new SqlParameter("@Phone", SqlDbType.NVarChar, 50);
            SqlParameter paramOutTime = new SqlParameter("@OutTime", SqlDbType.DateTime);
            SqlParameter paramOutUserSysNo = new SqlParameter("@OutUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.NVarChar, 200);
            SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
            SqlParameter paramEOutBoundDate = new SqlParameter("@EOutBoundDate", SqlDbType.DateTime);
            SqlParameter paramEResponseDate = new SqlParameter("@EResponseDate", SqlDbType.DateTime);
            SqlParameter paramAreaSysNo = new SqlParameter("@AreaSysNo", SqlDbType.Int, 4);
            SqlParameter paramOutBoundInvoiceQty = new SqlParameter("@OutBoundInvoiceQty", SqlDbType.Int, 4);
            SqlParameter paramShipType = new SqlParameter("@ShipType", SqlDbType.Int, 4);
            SqlParameter paramPackageID = new SqlParameter("@PackageID", SqlDbType.NVarChar, 50);
            SqlParameter paramCheckQtyUserSysNo = new SqlParameter("@CheckQtyUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCheckQtyTime = new SqlParameter("@CheckQtyTime", SqlDbType.DateTime);
            SqlParameter paramFreightUserSysNo = new SqlParameter("@FreightUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramSetDeliveryManTime = new SqlParameter("@SetDeliveryManTime", SqlDbType.DateTime);

            if (oParam.SysNo != AppConst.IntNull)
                paramSysNo.Value = oParam.SysNo;
            else
                paramSysNo.Value = System.DBNull.Value;
            if (oParam.OutBoundID != AppConst.StringNull)
                paramOutBoundID.Value = oParam.OutBoundID;
            else
                paramOutBoundID.Value = System.DBNull.Value;
            if (oParam.VendorSysNo != AppConst.IntNull)
                paramVendorSysNo.Value = oParam.VendorSysNo;
            else
                paramVendorSysNo.Value = System.DBNull.Value;
            if (oParam.CreateTime != AppConst.DateTimeNull)
                paramCreateTime.Value = oParam.CreateTime;
            else
                paramCreateTime.Value = System.DBNull.Value;
            if (oParam.CreateUserSysNo != AppConst.IntNull)
                paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
            else
                paramCreateUserSysNo.Value = System.DBNull.Value;
            if (oParam.VendorName != AppConst.StringNull)
                paramVendorName.Value = oParam.VendorName;
            else
                paramVendorName.Value = System.DBNull.Value;
            if (oParam.ZIP != AppConst.StringNull)
                paramZip.Value = oParam.ZIP;
            else
                paramZip.Value = System.DBNull.Value;
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
            if (oParam.OutTime != AppConst.DateTimeNull)
                paramOutTime.Value = oParam.OutTime;
            else
                paramOutTime.Value = System.DBNull.Value;
            if (oParam.OutUserSysNo != AppConst.IntNull)
                paramOutUserSysNo.Value = oParam.OutUserSysNo;
            else
                paramOutUserSysNo.Value = System.DBNull.Value;
            if (oParam.Note != AppConst.StringNull)
                paramNote.Value = oParam.Note;
            else
                paramNote.Value = System.DBNull.Value;
            if (oParam.Status != AppConst.IntNull)
                paramStatus.Value = oParam.Status;
            else
                paramStatus.Value = System.DBNull.Value;
            if (oParam.EOutBoundDate != AppConst.DateTimeNull)
                paramEOutBoundDate.Value = oParam.EOutBoundDate;
            else
                paramEOutBoundDate.Value = System.DBNull.Value;
            if (oParam.EResponseDate != AppConst.DateTimeNull)
                paramEResponseDate.Value = oParam.EResponseDate;
            else
                paramEResponseDate.Value = System.DBNull.Value;
            if (oParam.AreaSysNo != AppConst.IntNull)
                paramAreaSysNo.Value = oParam.AreaSysNo;
            else
                paramAreaSysNo.Value = System.DBNull.Value;
            if (oParam.OutBoundInvoiceQty != AppConst.IntNull)
                paramOutBoundInvoiceQty.Value = oParam.OutBoundInvoiceQty;
            else
                paramOutBoundInvoiceQty.Value = System.DBNull.Value;
            if (oParam.ShipType != AppConst.IntNull)
                paramShipType.Value = oParam.ShipType;
            else
                paramShipType.Value = System.DBNull.Value;
            if (oParam.PackageID != AppConst.StringNull)
                paramPackageID.Value = oParam.PackageID;
            else
                paramPackageID.Value = System.DBNull.Value;
            if (oParam.CheckQtyUserSysNo != AppConst.IntNull)
                paramCheckQtyUserSysNo.Value = oParam.CheckQtyUserSysNo;
            else
                paramCheckQtyUserSysNo.Value = System.DBNull.Value;
            if (oParam.CheckQtyTime != AppConst.DateTimeNull)
                paramCheckQtyTime.Value = oParam.CheckQtyTime;
            else
                paramCheckQtyTime.Value = System.DBNull.Value;
            if (oParam.FreightUserSysNo != AppConst.IntNull)
                paramFreightUserSysNo.Value = oParam.FreightUserSysNo;
            else
                paramFreightUserSysNo.Value = System.DBNull.Value;
            if (oParam.SetDeliveryManTime != AppConst.DateTimeNull)
                paramSetDeliveryManTime.Value = oParam.SetDeliveryManTime;
            else
                paramSetDeliveryManTime.Value = System.DBNull.Value;

            cmd.Parameters.Add(paramSysNo);
            cmd.Parameters.Add(paramOutBoundID);
            cmd.Parameters.Add(paramVendorSysNo);
            cmd.Parameters.Add(paramCreateTime);
            cmd.Parameters.Add(paramCreateUserSysNo);
            cmd.Parameters.Add(paramVendorName);
            cmd.Parameters.Add(paramZip);
            cmd.Parameters.Add(paramAddress);
            cmd.Parameters.Add(paramContact);
            cmd.Parameters.Add(paramPhone);
            cmd.Parameters.Add(paramOutTime);
            cmd.Parameters.Add(paramOutUserSysNo);
            cmd.Parameters.Add(paramNote);
            cmd.Parameters.Add(paramStatus);
            cmd.Parameters.Add(paramEOutBoundDate);
            cmd.Parameters.Add(paramEResponseDate);
            cmd.Parameters.Add(paramAreaSysNo);
            cmd.Parameters.Add(paramOutBoundInvoiceQty);
            cmd.Parameters.Add(paramShipType);
            cmd.Parameters.Add(paramPackageID);
            cmd.Parameters.Add(paramCheckQtyUserSysNo);
            cmd.Parameters.Add(paramCheckQtyTime);
            cmd.Parameters.Add(paramFreightUserSysNo);
            cmd.Parameters.Add(paramSetDeliveryManTime);

            return SqlHelper.ExecuteNonQuery(cmd);
        }


		public int UpdateMaster(Hashtable paramHash)
		{
			StringBuilder sb = new StringBuilder(200);
			sb.Append("UPDATE RMA_OutBound SET ");

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


		public int InsertItem(int outboundSysNo, int registerSysNo)
		{
			string sql = @"INSERT INTO RMA_OutBound_Item
                            (
                            OutBoundSysNo, RegisterSysNo
                            )
                            VALUES (
                            @OutBoundSysNo, @RegisterSysNo
                            );set @SysNo = SCOPE_IDENTITY();";
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramOutBoundSysNo = new SqlParameter("@OutBoundSysNo", SqlDbType.Int,4);
			SqlParameter paramRegisterSysNo = new SqlParameter("@RegisterSysNo", SqlDbType.Int,4);

			paramSysNo.Direction = ParameterDirection.Output;

			paramOutBoundSysNo.Value = outboundSysNo;
			paramRegisterSysNo.Value = registerSysNo;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramOutBoundSysNo);
			cmd.Parameters.Add(paramRegisterSysNo);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public int DeleteItem(int outboundSysNo, int registerSysNo)
		{
			string sql = "delete from rma_outbound_item where outboundsysno = " + outboundSysNo + " and registerSysNo = " + registerSysNo;
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);
		}
	}
}
