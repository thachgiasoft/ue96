using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using Icson.Utils;
using Icson.Objects.RMA;

namespace Icson.DBAccess.RMA
{
	/// <summary>
	/// Summary description for RMARevertDac.
	/// </summary>
	public class RMARevertDac
	{
		
		public RMARevertDac()
		{
			
		}

		public int InsertRevert(RMARevertInfo oParam)
		{
			string sql = @"INSERT INTO RMA_Revert
                            (
                            SysNo, RevertID, CustomerSysNo, CreateTime, 
                            CreateUserSysNo, ZIP, Address, Contact, 
                            Phone, ShipType, OutTime, OutUserSysNo, 
                            Note, Status ,SOSysNo ,LocationStatus,AddressAreaSysNo,PackageID,FreightUserSysNo, SetDeliveryManTime,CheckQtyUserSysNo, 
                            CheckQtyTime,IsPrintPackageCover,IsConfirmAddress
                            )
                            VALUES (
                            @SysNo, @RevertID, @CustomerSysNo, @CreateTime, 
                            @CreateUserSysNo, @ZIP, @Address, @Contact, 
                            @Phone, @ShipType, @OutTime, @OutUserSysNo, 
                            @Note, @Status ,@SOSysNo , @LocationStatus ,@AddressAreaSysNo,@PackageID ,@FreightUserSysNo, @SetDeliveryManTime,@CheckQtyUserSysNo,
                            @CheckQtyTime, @IsPrintPackageCover, @IsConfirmAddress
                            )";
			
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramRevertID = new SqlParameter("@RevertID", SqlDbType.NVarChar,10);
			SqlParameter paramCustomerSysNo = new SqlParameter("@CustomerSysNo", SqlDbType.Int,4);
			SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
			SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int,4);
			SqlParameter paramZIP = new SqlParameter("@ZIP", SqlDbType.NVarChar,10);
			SqlParameter paramAddress = new SqlParameter("@Address", SqlDbType.NVarChar,200);
			SqlParameter paramContact = new SqlParameter("@Contact", SqlDbType.NVarChar,50);
			SqlParameter paramPhone = new SqlParameter("@Phone", SqlDbType.NVarChar,50);
			SqlParameter paramShipType = new SqlParameter("@ShipType", SqlDbType.Int,4);
			SqlParameter paramOutTime = new SqlParameter("@OutTime", SqlDbType.DateTime);
			SqlParameter paramOutUserSysNo = new SqlParameter("@OutUserSysNo", SqlDbType.Int,4);
			SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.NVarChar,200);
			SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int,4);
			SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int,4);
            SqlParameter paramLocationStatus = new SqlParameter("@LocationStatus", SqlDbType.Int, 4);
            SqlParameter paramAddressAreaSysNo = new SqlParameter("@AddressAreaSysNo",SqlDbType.Int,4);
            SqlParameter paramPackageID = new SqlParameter("@PackageID", SqlDbType.NVarChar, 50);
            SqlParameter paramFreightUserSysNo = new SqlParameter("@FreightUserSysNo", SqlDbType.Int,4);
            SqlParameter paramSetDeliveryManTime = new SqlParameter("@SetDeliveryManTime", SqlDbType.DateTime);
            SqlParameter paramCheckQtyUserSysNo = new SqlParameter("@CheckQtyUserSysNo", SqlDbType.Int, 4);
            SqlParameter paramCheckQtyTime = new SqlParameter("@CheckQtyTime", SqlDbType.DateTime);
            SqlParameter paramIsPrintPackageCover = new SqlParameter("@IsPrintPackageCover", SqlDbType.Int, 4);
            SqlParameter paramIsConfirmAddress = new SqlParameter("@IsConfirmAddress", SqlDbType.Int, 4);

			if ( oParam.SysNo != AppConst.IntNull)
				paramSysNo.Value = oParam.SysNo;
			else
				paramSysNo.Value = System.DBNull.Value;
			if ( oParam.RevertID != AppConst.StringNull)
				paramRevertID.Value = oParam.RevertID;
			else
				paramRevertID.Value = System.DBNull.Value;
			if ( oParam.CustomerSysNo != AppConst.IntNull)
				paramCustomerSysNo.Value = oParam.CustomerSysNo;
			else
				paramCustomerSysNo.Value = System.DBNull.Value;
			if ( oParam.CreateTime != AppConst.DateTimeNull)
				paramCreateTime.Value = oParam.CreateTime;
			else
				paramCreateTime.Value = System.DBNull.Value;
			if ( oParam.CreateUserSysNo != AppConst.IntNull)
				paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
			else
				paramCreateUserSysNo.Value = System.DBNull.Value;
			if ( oParam.ZIP != AppConst.StringNull)
				paramZIP.Value = oParam.ZIP;
			else
				paramZIP.Value = System.DBNull.Value;
			if ( oParam.Address != AppConst.StringNull)
				paramAddress.Value = oParam.Address;
			else
				paramAddress.Value = System.DBNull.Value;
			if ( oParam.Contact != AppConst.StringNull)
				paramContact.Value = oParam.Contact;
			else
				paramContact.Value = System.DBNull.Value;
			if ( oParam.Phone != AppConst.StringNull)
				paramPhone.Value = oParam.Phone;
			else
				paramPhone.Value = System.DBNull.Value;
			if ( oParam.ShipType != AppConst.IntNull)
				paramShipType.Value = oParam.ShipType;
			else
				paramShipType.Value = System.DBNull.Value;
			if ( oParam.OutTime != AppConst.DateTimeNull)
				paramOutTime.Value = oParam.OutTime;
			else
				paramOutTime.Value = System.DBNull.Value;
			if ( oParam.OutUserSysNo != AppConst.IntNull)
				paramOutUserSysNo.Value = oParam.OutUserSysNo;
			else
				paramOutUserSysNo.Value = System.DBNull.Value;
			if ( oParam.Note != AppConst.StringNull)
				paramNote.Value = oParam.Note;
			else
				paramNote.Value = System.DBNull.Value;
			if ( oParam.Status != AppConst.IntNull)
				paramStatus.Value = oParam.Status;
			else
				paramStatus.Value = System.DBNull.Value;
			if ( oParam.SOSysNo != AppConst.IntNull)
				paramSOSysNo.Value = oParam.SOSysNo;
			else
				paramSOSysNo.Value = System.DBNull.Value;
            if (oParam.LocationStatus != AppConst.IntNull)
                paramLocationStatus.Value = oParam.LocationStatus;
            else
                paramLocationStatus.Value = System.DBNull.Value;
            if (oParam.AddressAreaSysNo != AppConst.IntNull)
                paramAddressAreaSysNo.Value = oParam.AddressAreaSysNo;
            else
                paramAddressAreaSysNo.Value = System.DBNull.Value;
            if (oParam.PackageID != AppConst.StringNull)

                paramPackageID.Value = oParam.PackageID;
            else
                paramPackageID.Value = System.DBNull.Value;

            if (oParam.FreightUserSysNo != AppConst.IntNull)
                paramFreightUserSysNo.Value = oParam.FreightUserSysNo;
            else
                paramFreightUserSysNo.Value = System.DBNull.Value;
            
            if (oParam.SetDeliveryManTime != AppConst.DateTimeNull)
                paramSetDeliveryManTime.Value = oParam.SetDeliveryManTime;
            else
                paramSetDeliveryManTime.Value = System.DBNull.Value;

            if (oParam.CheckQtyUserSysNo != AppConst.IntNull)
                paramCheckQtyUserSysNo.Value = oParam.CheckQtyUserSysNo;
            else
                paramCheckQtyUserSysNo.Value = System.DBNull.Value;

            if (oParam.CheckQtyTime != AppConst.DateTimeNull)
                paramCheckQtyTime.Value = oParam.CheckQtyTime;
            else
                paramCheckQtyTime.Value = System.DBNull.Value;

            if (oParam.IsPrintPackageCover != AppConst.IntNull)
                paramIsPrintPackageCover.Value = oParam.IsPrintPackageCover;
            else
                paramIsPrintPackageCover.Value = System.DBNull.Value;

            if (oParam.IsConfirmAddress != AppConst.IntNull)
                paramIsConfirmAddress.Value = oParam.IsConfirmAddress;
            else
                paramIsConfirmAddress.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramRevertID);
			cmd.Parameters.Add(paramCustomerSysNo);
			cmd.Parameters.Add(paramCreateTime);
			cmd.Parameters.Add(paramCreateUserSysNo);
			cmd.Parameters.Add(paramZIP);
			cmd.Parameters.Add(paramAddress);
			cmd.Parameters.Add(paramContact);
			cmd.Parameters.Add(paramPhone);
			cmd.Parameters.Add(paramShipType);
			cmd.Parameters.Add(paramOutTime);
			cmd.Parameters.Add(paramOutUserSysNo);
			cmd.Parameters.Add(paramNote);
			cmd.Parameters.Add(paramStatus);
			cmd.Parameters.Add(paramSOSysNo);
            cmd.Parameters.Add(paramLocationStatus);
            cmd.Parameters.Add(paramAddressAreaSysNo);
            cmd.Parameters.Add(paramPackageID);
            cmd.Parameters.Add(paramFreightUserSysNo);
            cmd.Parameters.Add(paramSetDeliveryManTime);
            cmd.Parameters.Add(paramCheckQtyUserSysNo);
            cmd.Parameters.Add(paramCheckQtyTime);
            cmd.Parameters.Add(paramIsPrintPackageCover);
            cmd.Parameters.Add(paramIsConfirmAddress);

			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public int UpdateRevert(Hashtable paramHash)
		{
			StringBuilder sb = new StringBuilder(200);
			sb.Append("UPDATE RMA_Revert SET ");

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

		public int InsertRevertItem(RMARevertItemInfo oParam)
		{
			string sql = @"INSERT INTO RMA_Revert_Item
                            (
                            RevertSysNo, RegisterSysNo, StockSysNo ,Cost
                            )
                            VALUES (
                             @RevertSysNo, @RegisterSysNo, @StockSysNo ,@Cost
                            );set @SysNo = SCOPE_IDENTITY();";			
			SqlCommand cmd = new SqlCommand(sql);

			SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int,4);
			SqlParameter paramRevertSysNo = new SqlParameter("@RevertSysNo", SqlDbType.Int,4);
			SqlParameter paramRegisterSysNo = new SqlParameter("@RegisterSysNo", SqlDbType.Int,4);
			SqlParameter paramStockSysNo = new SqlParameter("@StockSysNo", SqlDbType.Int,4);
            SqlParameter paramCost       = new SqlParameter("@Cost",SqlDbType.Decimal,9);

			paramSysNo.Direction = ParameterDirection.Output;

			if ( oParam.SysNo != AppConst.IntNull)
				paramSysNo.Value = oParam.SysNo;
			else
				paramSysNo.Value = System.DBNull.Value;
			if ( oParam.RevertSysNo != AppConst.IntNull)
				paramRevertSysNo.Value = oParam.RevertSysNo;
			else
				paramRevertSysNo.Value = System.DBNull.Value;
			if ( oParam.RegisterSysNo != AppConst.IntNull)
				paramRegisterSysNo.Value = oParam.RegisterSysNo;
			else
				paramRegisterSysNo.Value = System.DBNull.Value;
			if ( oParam.StockSysNo != AppConst.IntNull)
				paramStockSysNo.Value = oParam.StockSysNo;
			else
				paramStockSysNo.Value = System.DBNull.Value;
            if (oParam.Cost != AppConst.DecimalNull)
                paramCost.Value = oParam.Cost;
            else
                paramCost.Value = System.DBNull.Value;

			cmd.Parameters.Add(paramSysNo);
			cmd.Parameters.Add(paramRevertSysNo);
			cmd.Parameters.Add(paramRegisterSysNo);
			cmd.Parameters.Add(paramStockSysNo);
            cmd.Parameters.Add(paramCost);

			return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
		}
        public int UpdateRevertItem(Hashtable paramHash)
        {
            StringBuilder sb = new StringBuilder(200);
            sb.Append("UPDATE RMA_Revert_Item SET ");

            if (paramHash != null && paramHash.Count != 0)
            {
                int index = 0;
                foreach (string key in paramHash.Keys)
                {
                    object item = paramHash[key];
                    if (key.ToLower() == "sysno")
                        continue;

                    if (index != 0)
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
                        sb.Append(key).Append(" = cast(").Append(Util.ToSqlString(((DateTime)item).ToString(AppConst.DateFormatLong))).Append(" as DateTime)");
                    }
                   
                }
            }

            sb.Append(" WHERE SysNo=").Append(paramHash["SysNo"]);

            return SqlHelper.ExecuteNonQuery(sb.ToString());
        }

		public int DeleteItem(int sysno)
		{
		    string sql = "delete from RMA_Revert_Item where sysno = " + sysno ;
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);
		}

        public int DeleteItem(int registersysno, int revertsysno)
        {
            string sql = "delete from RMA_Revert_Item where registersysno = " + registersysno + "and RevertSysNo ="+revertsysno;
            SqlCommand cmd = new SqlCommand(sql);
            return SqlHelper.ExecuteNonQuery(cmd);
        }
		public int RevertCancelOut(int sysno)
		{
		    string sql = @"update RMA_Revert set OutTime=null ,OutUserSysNo=null where sysno = " + sysno;
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);
		}

		public int SetRevertStock(int itemsysno,int stocksysno)
		{
		    string sql = @"Update RMA_Revert_Item set StockSysNo= @stocksysno where sysno = " + itemsysno;
			if (stocksysno == AppConst.IntNull)
				sql = sql.Replace("@stocksysno" , "null");
			else
				sql = sql.Replace("@stocksysno" , stocksysno.ToString());
			SqlCommand cmd = new SqlCommand(sql);
			return SqlHelper.ExecuteNonQuery(cmd);
		}
	}
}