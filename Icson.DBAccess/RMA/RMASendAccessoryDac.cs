using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

using Icson.Utils;
using Icson.Objects.RMA;

namespace Icson.DBAccess.RMA
{
  public class RMASendAccessoryDac
    {
      public int Insert(RMASendAccessoryInfo oParam)
      {
          string sql = @"INSERT INTO RMA_SendAccessory
                            (
                            SOSysNo, CustomerSysNo, SendAccessoryID, CreateTime, 
                            CreateUserSysNo, AreaSysNo, Address, Contact, 
                            Phone, Status, UpdateUserSysNo, UpdateTime, 
                            AuditTime, AuditUserSysNo, SendTime, SendStockSynNo, 
                            ShipTypeSysNo, SendUserSysNo, Memo, IsPrintPackage, 
                            PackageID, FreightUserSysNo, SetDeliveryManTime
                            )
                            VALUES (
                            @SOSysNo, @CustomerSysNo, @SendAccessoryID, @CreateTime, 
                            @CreateUserSysNo, @AreaSysNo, @Address, @Contact, 
                            @Phone, @Status, @UpdateUserSysNo, @UpdateTime, 
                            @AuditTime, @AuditUserSysNo, @SendTime, @SendStockSynNo, 
                            @ShipTypeSysNo, @SendUserSysNo, @Memo, @IsPrintPackage, 
                            @PackageID, @FreightUserSysNo, @SetDeliveryManTime
                            );set @SysNo = SCOPE_IDENTITY();";
          SqlCommand cmd = new SqlCommand(sql);

          SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
          SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int, 4);
          SqlParameter paramCustomerSysNo = new SqlParameter("@CustomerSysNo", SqlDbType.Int, 4);
          SqlParameter paramSendAccessoryID = new SqlParameter("@SendAccessoryID", SqlDbType.NVarChar, 10);
          SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
          SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
          SqlParameter paramAreaSysNo = new SqlParameter("@AreaSysNo", SqlDbType.Int, 4);
          SqlParameter paramAddress = new SqlParameter("@Address", SqlDbType.NVarChar, 200);
          SqlParameter paramContact = new SqlParameter("@Contact", SqlDbType.NVarChar, 50);
          SqlParameter paramPhone = new SqlParameter("@Phone", SqlDbType.NVarChar, 50);
          SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
          SqlParameter paramUpdateUserSysNo = new SqlParameter("@UpdateUserSysNo", SqlDbType.Int, 4);
          SqlParameter paramUpdateTime = new SqlParameter("@UpdateTime", SqlDbType.DateTime);
          SqlParameter paramAuditTime = new SqlParameter("@AuditTime", SqlDbType.DateTime);
          SqlParameter paramAuditUserSysNo = new SqlParameter("@AuditUserSysNo", SqlDbType.Int, 4);
          SqlParameter paramSendTime = new SqlParameter("@SendTime", SqlDbType.DateTime);
          SqlParameter paramSendStockSynNo = new SqlParameter("@SendStockSynNo", SqlDbType.Int, 4);
          SqlParameter paramShipTypeSysNo = new SqlParameter("@ShipTypeSysNo", SqlDbType.Int, 4);
          SqlParameter paramSendUserSysNo = new SqlParameter("@SendUserSysNo", SqlDbType.Int, 4);
          SqlParameter paramMemo = new SqlParameter("@Memo", SqlDbType.NVarChar, 200);
          SqlParameter paramIsPrintPackage = new SqlParameter("@IsPrintPackage", SqlDbType.Int, 4);
          SqlParameter paramPackageID = new SqlParameter("@PackageID", SqlDbType.NVarChar, 50);
          SqlParameter paramFreightUserSysNo = new SqlParameter("@FreightUserSysNo", SqlDbType.Int, 4);
          SqlParameter paramSetDeliveryManTime = new SqlParameter("@SetDeliveryManTime", SqlDbType.DateTime);
          paramSysNo.Direction = ParameterDirection.Output;
          if (oParam.SOSysNo != AppConst.IntNull)
              paramSOSysNo.Value = oParam.SOSysNo;
          else
              paramSOSysNo.Value = System.DBNull.Value;
          if (oParam.CustomerSysNo != AppConst.IntNull)
              paramCustomerSysNo.Value = oParam.CustomerSysNo;
          else
              paramCustomerSysNo.Value = System.DBNull.Value;
          if (oParam.SendAccessoryID != AppConst.StringNull)
              paramSendAccessoryID.Value = oParam.SendAccessoryID;
          else
              paramSendAccessoryID.Value = System.DBNull.Value;
          if (oParam.CreateTime != AppConst.DateTimeNull)
              paramCreateTime.Value = oParam.CreateTime;
          else
              paramCreateTime.Value = System.DBNull.Value;
          if (oParam.CreateUserSysNo != AppConst.IntNull)
              paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
          else
              paramCreateUserSysNo.Value = System.DBNull.Value;
          if (oParam.AreaSysNo != AppConst.IntNull)
              paramAreaSysNo.Value = oParam.AreaSysNo;
          else
              paramAreaSysNo.Value = System.DBNull.Value;
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
          if (oParam.Status != AppConst.IntNull)
              paramStatus.Value = oParam.Status;
          else
              paramStatus.Value = System.DBNull.Value;
          if (oParam.UpdateUserSysNo != AppConst.IntNull)
              paramUpdateUserSysNo.Value = oParam.UpdateUserSysNo;
          else
              paramUpdateUserSysNo.Value = System.DBNull.Value;
          if (oParam.UpdateTime != AppConst.DateTimeNull)
              paramUpdateTime.Value = oParam.UpdateTime;
          else
              paramUpdateTime.Value = System.DBNull.Value;
          if (oParam.AuditTime != AppConst.DateTimeNull)
              paramAuditTime.Value = oParam.AuditTime;
          else
              paramAuditTime.Value = System.DBNull.Value;
          if (oParam.AuditUserSysNo != AppConst.IntNull)
              paramAuditUserSysNo.Value = oParam.AuditUserSysNo;
          else
              paramAuditUserSysNo.Value = System.DBNull.Value;
          if (oParam.SendTime != AppConst.DateTimeNull)
              paramSendTime.Value = oParam.SendTime;
          else
              paramSendTime.Value = System.DBNull.Value;
          if (oParam.SendStockSynNo != AppConst.IntNull)
              paramSendStockSynNo.Value = oParam.SendStockSynNo;
          else
              paramSendStockSynNo.Value = System.DBNull.Value;
          if (oParam.ShipTypeSysNo != AppConst.IntNull)
              paramShipTypeSysNo.Value = oParam.ShipTypeSysNo;
          else
              paramShipTypeSysNo.Value = System.DBNull.Value;
          if (oParam.SendUserSysNo != AppConst.IntNull)
              paramSendUserSysNo.Value = oParam.SendUserSysNo;
          else
              paramSendUserSysNo.Value = System.DBNull.Value;
          if (oParam.Memo != AppConst.StringNull)
              paramMemo.Value = oParam.Memo;
          else
              paramMemo.Value = System.DBNull.Value;
          if (oParam.IsPrintPackage != AppConst.IntNull)
              paramIsPrintPackage.Value = oParam.IsPrintPackage;
          else
              paramIsPrintPackage.Value = System.DBNull.Value;
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

          cmd.Parameters.Add(paramSysNo);
          cmd.Parameters.Add(paramSOSysNo);
          cmd.Parameters.Add(paramCustomerSysNo);
          cmd.Parameters.Add(paramSendAccessoryID);
          cmd.Parameters.Add(paramCreateTime);
          cmd.Parameters.Add(paramCreateUserSysNo);
          cmd.Parameters.Add(paramAreaSysNo);
          cmd.Parameters.Add(paramAddress);
          cmd.Parameters.Add(paramContact);
          cmd.Parameters.Add(paramPhone);
          cmd.Parameters.Add(paramStatus);
          cmd.Parameters.Add(paramUpdateUserSysNo);
          cmd.Parameters.Add(paramUpdateTime);
          cmd.Parameters.Add(paramAuditTime);
          cmd.Parameters.Add(paramAuditUserSysNo);
          cmd.Parameters.Add(paramSendTime);
          cmd.Parameters.Add(paramSendStockSynNo);
          cmd.Parameters.Add(paramShipTypeSysNo);
          cmd.Parameters.Add(paramSendUserSysNo);
          cmd.Parameters.Add(paramMemo);
          cmd.Parameters.Add(paramIsPrintPackage);
          cmd.Parameters.Add(paramPackageID);
          cmd.Parameters.Add(paramFreightUserSysNo);
          cmd.Parameters.Add(paramSetDeliveryManTime);

          return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
      }
      public int Update(RMASendAccessoryInfo oParam)
      {
          string sql = @"UPDATE RMA_SendAccessory SET 
                            SOSysNo=@SOSysNo, CustomerSysNo=@CustomerSysNo, 
                            SendAccessoryID=@SendAccessoryID, CreateTime=@CreateTime, 
                            CreateUserSysNo=@CreateUserSysNo, AreaSysNo=@AreaSysNo, 
                            Address=@Address, Contact=@Contact, 
                            Phone=@Phone, Status=@Status, 
                            UpdateUserSysNo=@UpdateUserSysNo, UpdateTime=@UpdateTime, 
                            AuditTime=@AuditTime, AuditUserSysNo=@AuditUserSysNo, 
                            SendTime=@SendTime, SendStockSynNo=@SendStockSynNo, 
                            ShipTypeSysNo=@ShipTypeSysNo, SendUserSysNo=@SendUserSysNo, 
                            Memo=@Memo, IsPrintPackage=@IsPrintPackage, 
                            PackageID=@PackageID, FreightUserSysNo=@FreightUserSysNo, 
                            SetDeliveryManTime=@SetDeliveryManTime
                            WHERE SysNo=@SysNo";
          SqlCommand cmd = new SqlCommand(sql);

          SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
          SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int, 4);
          SqlParameter paramCustomerSysNo = new SqlParameter("@CustomerSysNo", SqlDbType.Int, 4);
          SqlParameter paramSendAccessoryID = new SqlParameter("@SendAccessoryID", SqlDbType.NVarChar, 10);
          SqlParameter paramCreateTime = new SqlParameter("@CreateTime", SqlDbType.DateTime);
          SqlParameter paramCreateUserSysNo = new SqlParameter("@CreateUserSysNo", SqlDbType.Int, 4);
          SqlParameter paramAreaSysNo = new SqlParameter("@AreaSysNo", SqlDbType.Int, 4);
          SqlParameter paramAddress = new SqlParameter("@Address", SqlDbType.NVarChar, 200);
          SqlParameter paramContact = new SqlParameter("@Contact", SqlDbType.NVarChar, 50);
          SqlParameter paramPhone = new SqlParameter("@Phone", SqlDbType.NVarChar, 50);
          SqlParameter paramStatus = new SqlParameter("@Status", SqlDbType.Int, 4);
          SqlParameter paramUpdateUserSysNo = new SqlParameter("@UpdateUserSysNo", SqlDbType.Int, 4);
          SqlParameter paramUpdateTime = new SqlParameter("@UpdateTime", SqlDbType.DateTime);
          SqlParameter paramAuditTime = new SqlParameter("@AuditTime", SqlDbType.DateTime);
          SqlParameter paramAuditUserSysNo = new SqlParameter("@AuditUserSysNo", SqlDbType.Int, 4);
          SqlParameter paramSendTime = new SqlParameter("@SendTime", SqlDbType.DateTime);
          SqlParameter paramSendStockSynNo = new SqlParameter("@SendStockSynNo", SqlDbType.Int, 4);
          SqlParameter paramShipTypeSysNo = new SqlParameter("@ShipTypeSysNo", SqlDbType.Int, 4);
          SqlParameter paramSendUserSysNo = new SqlParameter("@SendUserSysNo", SqlDbType.Int, 4);
          SqlParameter paramMemo = new SqlParameter("@Memo", SqlDbType.NVarChar, 200);
          SqlParameter paramIsPrintPackage = new SqlParameter("@IsPrintPackage", SqlDbType.Int, 4);
          SqlParameter paramPackageID = new SqlParameter("@PackageID", SqlDbType.NVarChar, 50);
          SqlParameter paramFreightUserSysNo = new SqlParameter("@FreightUserSysNo", SqlDbType.Int, 4);
          SqlParameter paramSetDeliveryManTime = new SqlParameter("@SetDeliveryManTime", SqlDbType.DateTime);

          if (oParam.SysNo != AppConst.IntNull)
              paramSysNo.Value = oParam.SysNo;
          else
              paramSysNo.Value = System.DBNull.Value;
          if (oParam.SOSysNo != AppConst.IntNull)
              paramSOSysNo.Value = oParam.SOSysNo;
          else
              paramSOSysNo.Value = System.DBNull.Value;
          if (oParam.CustomerSysNo != AppConst.IntNull)
              paramCustomerSysNo.Value = oParam.CustomerSysNo;
          else
              paramCustomerSysNo.Value = System.DBNull.Value;
          if (oParam.SendAccessoryID != AppConst.StringNull)
              paramSendAccessoryID.Value = oParam.SendAccessoryID;
          else
              paramSendAccessoryID.Value = System.DBNull.Value;
          if (oParam.CreateTime != AppConst.DateTimeNull)
              paramCreateTime.Value = oParam.CreateTime;
          else
              paramCreateTime.Value = System.DBNull.Value;
          if (oParam.CreateUserSysNo != AppConst.IntNull)
              paramCreateUserSysNo.Value = oParam.CreateUserSysNo;
          else
              paramCreateUserSysNo.Value = System.DBNull.Value;
          if (oParam.AreaSysNo != AppConst.IntNull)
              paramAreaSysNo.Value = oParam.AreaSysNo;
          else
              paramAreaSysNo.Value = System.DBNull.Value;
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
          if (oParam.Status != AppConst.IntNull)
              paramStatus.Value = oParam.Status;
          else
              paramStatus.Value = System.DBNull.Value;
          if (oParam.UpdateUserSysNo != AppConst.IntNull)
              paramUpdateUserSysNo.Value = oParam.UpdateUserSysNo;
          else
              paramUpdateUserSysNo.Value = System.DBNull.Value;
          if (oParam.UpdateTime != AppConst.DateTimeNull)
              paramUpdateTime.Value = oParam.UpdateTime;
          else
              paramUpdateTime.Value = System.DBNull.Value;
          if (oParam.AuditTime != AppConst.DateTimeNull)
              paramAuditTime.Value = oParam.AuditTime;
          else
              paramAuditTime.Value = System.DBNull.Value;
          if (oParam.AuditUserSysNo != AppConst.IntNull)
              paramAuditUserSysNo.Value = oParam.AuditUserSysNo;
          else
              paramAuditUserSysNo.Value = System.DBNull.Value;
          if (oParam.SendTime != AppConst.DateTimeNull)
              paramSendTime.Value = oParam.SendTime;
          else
              paramSendTime.Value = System.DBNull.Value;
          if (oParam.SendStockSynNo != AppConst.IntNull)
              paramSendStockSynNo.Value = oParam.SendStockSynNo;
          else
              paramSendStockSynNo.Value = System.DBNull.Value;
          if (oParam.ShipTypeSysNo != AppConst.IntNull)
              paramShipTypeSysNo.Value = oParam.ShipTypeSysNo;
          else
              paramShipTypeSysNo.Value = System.DBNull.Value;
          if (oParam.SendUserSysNo != AppConst.IntNull)
              paramSendUserSysNo.Value = oParam.SendUserSysNo;
          else
              paramSendUserSysNo.Value = System.DBNull.Value;
          if (oParam.Memo != AppConst.StringNull)
              paramMemo.Value = oParam.Memo;
          else
              paramMemo.Value = System.DBNull.Value;
          if (oParam.IsPrintPackage != AppConst.IntNull)
              paramIsPrintPackage.Value = oParam.IsPrintPackage;
          else
              paramIsPrintPackage.Value = System.DBNull.Value;
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

          cmd.Parameters.Add(paramSysNo);
          cmd.Parameters.Add(paramSOSysNo);
          cmd.Parameters.Add(paramCustomerSysNo);
          cmd.Parameters.Add(paramSendAccessoryID);
          cmd.Parameters.Add(paramCreateTime);
          cmd.Parameters.Add(paramCreateUserSysNo);
          cmd.Parameters.Add(paramAreaSysNo);
          cmd.Parameters.Add(paramAddress);
          cmd.Parameters.Add(paramContact);
          cmd.Parameters.Add(paramPhone);
          cmd.Parameters.Add(paramStatus);
          cmd.Parameters.Add(paramUpdateUserSysNo);
          cmd.Parameters.Add(paramUpdateTime);
          cmd.Parameters.Add(paramAuditTime);
          cmd.Parameters.Add(paramAuditUserSysNo);
          cmd.Parameters.Add(paramSendTime);
          cmd.Parameters.Add(paramSendStockSynNo);
          cmd.Parameters.Add(paramShipTypeSysNo);
          cmd.Parameters.Add(paramSendUserSysNo);
          cmd.Parameters.Add(paramMemo);
          cmd.Parameters.Add(paramIsPrintPackage);
          cmd.Parameters.Add(paramPackageID);
          cmd.Parameters.Add(paramFreightUserSysNo);
          cmd.Parameters.Add(paramSetDeliveryManTime);

          return SqlHelper.ExecuteNonQuery(cmd);
      }
      public int InsertItem(RMASendAccessoryItemInfo oParam)
      {
          string sql = @"INSERT INTO RMA_SendAccessory_Item
                            (
                            SendAccessorySysNo, SOSysNo, ProductSysNo, ProductIDSysNo, 
                            NewProductSysNo, NewProductQty, NewProductIDSysNo, AccessoryType, 
                            AccessoryName, SOItemPODesc, Note
                            )
                            VALUES (
                            @SendAccessorySysNo, @SOSysNo, @ProductSysNo, @ProductIDSysNo, 
                            @NewProductSysNo, @NewProductQty, @NewProductIDSysNo, @AccessoryType, 
                            @AccessoryName, @SOItemPODesc, @Note
                            );set @SysNo = SCOPE_IDENTITY();";
          SqlCommand cmd = new SqlCommand(sql);

          SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
          SqlParameter paramSendAccessorySysNo = new SqlParameter("@SendAccessorySysNo", SqlDbType.Int, 4);
          SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int, 4);
          SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int, 4);
          SqlParameter paramProductIDSysNo = new SqlParameter("@ProductIDSysNo", SqlDbType.Int, 4);
          SqlParameter paramNewProductSysNo = new SqlParameter("@NewProductSysNo", SqlDbType.Int, 4);
          SqlParameter paramNewProductQty = new SqlParameter("@NewProductQty", SqlDbType.Int, 4);
          SqlParameter paramNewProductIDSysNo = new SqlParameter("@NewProductIDSysNo", SqlDbType.Int, 4);
          SqlParameter paramAccessoryType = new SqlParameter("@AccessoryType", SqlDbType.Int, 4);
          SqlParameter paramAccessoryName = new SqlParameter("@AccessoryName", SqlDbType.NVarChar, 100);
          SqlParameter paramSOItemPODesc = new SqlParameter("@SOItemPODesc", SqlDbType.NVarChar, 200);
          SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.NVarChar, 200);
          paramSysNo.Direction = ParameterDirection.Output;
          if (oParam.SendAccessorySysNo != AppConst.IntNull)
              paramSendAccessorySysNo.Value = oParam.SendAccessorySysNo;
          else
              paramSendAccessorySysNo.Value = System.DBNull.Value;
          if (oParam.SOSysNo != AppConst.IntNull)
              paramSOSysNo.Value = oParam.SOSysNo;
          else
              paramSOSysNo.Value = System.DBNull.Value;
          if (oParam.ProductSysNo != AppConst.IntNull)
              paramProductSysNo.Value = oParam.ProductSysNo;
          else
              paramProductSysNo.Value = System.DBNull.Value;
          if (oParam.ProductIDSysNo != AppConst.IntNull)
              paramProductIDSysNo.Value = oParam.ProductIDSysNo;
          else
              paramProductIDSysNo.Value = System.DBNull.Value;
          if (oParam.NewProductSysNo != AppConst.IntNull)
              paramNewProductSysNo.Value = oParam.NewProductSysNo;
          else
              paramNewProductSysNo.Value = System.DBNull.Value;
          if (oParam.NewProductQty != AppConst.IntNull)
              paramNewProductQty.Value = oParam.NewProductQty;
          else
              paramNewProductQty.Value = System.DBNull.Value;
          if (oParam.NewProductIDSysNo != AppConst.IntNull)
              paramNewProductIDSysNo.Value = oParam.NewProductIDSysNo;
          else
              paramNewProductIDSysNo.Value = System.DBNull.Value;
          if (oParam.AccessoryType != AppConst.IntNull)
              paramAccessoryType.Value = oParam.AccessoryType;
          else
              paramAccessoryType.Value = System.DBNull.Value;
          if (oParam.AccessoryName != AppConst.StringNull)
              paramAccessoryName.Value = oParam.AccessoryName;
          else
              paramAccessoryName.Value = System.DBNull.Value;
          if (oParam.SOItemPODesc != AppConst.StringNull)
              paramSOItemPODesc.Value = oParam.SOItemPODesc;
          else
              paramSOItemPODesc.Value = System.DBNull.Value;
          if (oParam.Note != AppConst.StringNull)
              paramNote.Value = oParam.Note;
          else
              paramNote.Value = System.DBNull.Value;

          cmd.Parameters.Add(paramSysNo);
          cmd.Parameters.Add(paramSendAccessorySysNo);
          cmd.Parameters.Add(paramSOSysNo);
          cmd.Parameters.Add(paramProductSysNo);
          cmd.Parameters.Add(paramProductIDSysNo);
          cmd.Parameters.Add(paramNewProductSysNo);
          cmd.Parameters.Add(paramNewProductQty);
          cmd.Parameters.Add(paramNewProductIDSysNo);
          cmd.Parameters.Add(paramAccessoryType);
          cmd.Parameters.Add(paramAccessoryName);
          cmd.Parameters.Add(paramSOItemPODesc);
          cmd.Parameters.Add(paramNote);

          return SqlHelper.ExecuteNonQuery(cmd, out oParam.SysNo);
      }
      public int UpdateItem(RMASendAccessoryItemInfo oParam)
      {
          string sql = @"UPDATE RMA_SendAccessory_Item SET 
                            SendAccessorySysNo=@SendAccessorySysNo, SOSysNo=@SOSysNo, 
                            ProductSysNo=@ProductSysNo, ProductIDSysNo=@ProductIDSysNo, 
                            NewProductSysNo=@NewProductSysNo, NewProductQty=@NewProductQty, 
                            NewProductIDSysNo=@NewProductIDSysNo, AccessoryType=@AccessoryType, 
                            AccessoryName=@AccessoryName, SOItemPODesc=@SOItemPODesc, 
                            Note=@Note
                            WHERE SysNo=@SysNo";
          SqlCommand cmd = new SqlCommand(sql);

          SqlParameter paramSysNo = new SqlParameter("@SysNo", SqlDbType.Int, 4);
          SqlParameter paramSendAccessorySysNo = new SqlParameter("@SendAccessorySysNo", SqlDbType.Int, 4);
          SqlParameter paramSOSysNo = new SqlParameter("@SOSysNo", SqlDbType.Int, 4);
          SqlParameter paramProductSysNo = new SqlParameter("@ProductSysNo", SqlDbType.Int, 4);
          SqlParameter paramProductIDSysNo = new SqlParameter("@ProductIDSysNo", SqlDbType.Int, 4);
          SqlParameter paramNewProductSysNo = new SqlParameter("@NewProductSysNo", SqlDbType.Int, 4);
          SqlParameter paramNewProductQty = new SqlParameter("@NewProductQty", SqlDbType.Int, 4);
          SqlParameter paramNewProductIDSysNo = new SqlParameter("@NewProductIDSysNo", SqlDbType.Int, 4);
          SqlParameter paramAccessoryType = new SqlParameter("@AccessoryType", SqlDbType.Int, 4);
          SqlParameter paramAccessoryName = new SqlParameter("@AccessoryName", SqlDbType.NVarChar, 100);
          SqlParameter paramSOItemPODesc = new SqlParameter("@SOItemPODesc", SqlDbType.NVarChar, 200);
          SqlParameter paramNote = new SqlParameter("@Note", SqlDbType.NVarChar, 200);

          if (oParam.SysNo != AppConst.IntNull)
              paramSysNo.Value = oParam.SysNo;
          else
              paramSysNo.Value = System.DBNull.Value;
          if (oParam.SendAccessorySysNo != AppConst.IntNull)
              paramSendAccessorySysNo.Value = oParam.SendAccessorySysNo;
          else
              paramSendAccessorySysNo.Value = System.DBNull.Value;
          if (oParam.SOSysNo != AppConst.IntNull)
              paramSOSysNo.Value = oParam.SOSysNo;
          else
              paramSOSysNo.Value = System.DBNull.Value;
          if (oParam.ProductSysNo != AppConst.IntNull)
              paramProductSysNo.Value = oParam.ProductSysNo;
          else
              paramProductSysNo.Value = System.DBNull.Value;
          if (oParam.ProductIDSysNo != AppConst.IntNull)
              paramProductIDSysNo.Value = oParam.ProductIDSysNo;
          else
              paramProductIDSysNo.Value = System.DBNull.Value;
          if (oParam.NewProductSysNo != AppConst.IntNull)
              paramNewProductSysNo.Value = oParam.NewProductSysNo;
          else
              paramNewProductSysNo.Value = System.DBNull.Value;
          if (oParam.NewProductQty != AppConst.IntNull)
              paramNewProductQty.Value = oParam.NewProductQty;
          else
              paramNewProductQty.Value = System.DBNull.Value;
          if (oParam.NewProductIDSysNo != AppConst.IntNull)
              paramNewProductIDSysNo.Value = oParam.NewProductIDSysNo;
          else
              paramNewProductIDSysNo.Value = System.DBNull.Value;
          if (oParam.AccessoryType != AppConst.IntNull)
              paramAccessoryType.Value = oParam.AccessoryType;
          else
              paramAccessoryType.Value = System.DBNull.Value;
          if (oParam.AccessoryName != AppConst.StringNull)
              paramAccessoryName.Value = oParam.AccessoryName;
          else
              paramAccessoryName.Value = System.DBNull.Value;
          if (oParam.SOItemPODesc != AppConst.StringNull)
              paramSOItemPODesc.Value = oParam.SOItemPODesc;
          else
              paramSOItemPODesc.Value = System.DBNull.Value;
          if (oParam.Note != AppConst.StringNull)
              paramNote.Value = oParam.Note;
          else
              paramNote.Value = System.DBNull.Value;

          cmd.Parameters.Add(paramSysNo);
          cmd.Parameters.Add(paramSendAccessorySysNo);
          cmd.Parameters.Add(paramSOSysNo);
          cmd.Parameters.Add(paramProductSysNo);
          cmd.Parameters.Add(paramProductIDSysNo);
          cmd.Parameters.Add(paramNewProductSysNo);
          cmd.Parameters.Add(paramNewProductQty);
          cmd.Parameters.Add(paramNewProductIDSysNo);
          cmd.Parameters.Add(paramAccessoryType);
          cmd.Parameters.Add(paramAccessoryName);
          cmd.Parameters.Add(paramSOItemPODesc);
          cmd.Parameters.Add(paramNote);

          return SqlHelper.ExecuteNonQuery(cmd);
      }
      public int DeleteItem(int paramSysNo)
      {
          string sql = "delete from RMA_SendAccessory_Item where sysno=" + paramSysNo;
          SqlCommand cmd = new SqlCommand(sql);
          return SqlHelper.ExecuteNonQuery(cmd);
      }

      public int Update(Hashtable paramHash)
      {
          StringBuilder sb = new StringBuilder(200);
          sb.Append("UPDATE RMA_SendAccessory SET ");

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

    }
}
