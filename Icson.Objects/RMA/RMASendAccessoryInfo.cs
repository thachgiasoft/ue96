using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.RMA
{
   
  public class RMASendAccessoryInfo
    {
      public RMASendAccessoryInfo()
      {
          Init();
      }
      public int SysNo;
      public int SOSysNo;
      public int CustomerSysNo;
      public string SendAccessoryID;
      public DateTime CreateTime;
      public int CreateUserSysNo;
      public int AreaSysNo;
      public string Address;
      public string Contact;
      public string Phone;
      public int Status;
      public int UpdateUserSysNo;
      public DateTime UpdateTime;
      public DateTime AuditTime;
      public int AuditUserSysNo;
      public DateTime SendTime;
      public int SendStockSynNo;
      public int ShipTypeSysNo;
      public int SendUserSysNo;
      public string Memo;
      public int IsPrintPackage;
      public string PackageID;
      public int FreightUserSysNo;
      public DateTime SetDeliveryManTime;

        public Hashtable ItemHash = new Hashtable();//key sendaccessoryitemsysno , value sendaccessoryiteminfo;

      public void Init()
      {
          SysNo = AppConst.IntNull;
          SOSysNo = AppConst.IntNull;
          CustomerSysNo = AppConst.IntNull;
          SendAccessoryID = AppConst.StringNull;
          CreateTime = AppConst.DateTimeNull;
          CreateUserSysNo = AppConst.IntNull;
          AreaSysNo = AppConst.IntNull;
          Address = AppConst.StringNull;
          Contact = AppConst.StringNull;
          Phone = AppConst.StringNull;
          Status = AppConst.IntNull;
          UpdateUserSysNo = AppConst.IntNull;
          UpdateTime = AppConst.DateTimeNull;
          AuditTime = AppConst.DateTimeNull;
          AuditUserSysNo = AppConst.IntNull;
          SendTime = AppConst.DateTimeNull;
          SendStockSynNo = AppConst.IntNull;
          ShipTypeSysNo = AppConst.IntNull;
          SendUserSysNo = AppConst.IntNull;
          Memo = AppConst.StringNull;
          IsPrintPackage = AppConst.IntNull;
          PackageID = AppConst.StringNull;
          FreightUserSysNo = AppConst.IntNull;
          SetDeliveryManTime = AppConst.DateTimeNull;
      }
    }
}
