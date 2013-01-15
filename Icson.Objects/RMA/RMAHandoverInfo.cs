using System;
using System.Collections.Generic;
using System.Collections;

using Icson.Utils;

namespace Icson.Objects.RMA
{
  public class RMAHandoverInfo
    {

      public RMAHandoverInfo()
      {
          Init();
      }
        public int SysNo;
        public string HandoverID;
        public DateTime CreateTime;
        public int CreateUserSysNo;
        public DateTime UpdateTime;
        public int UpdateUserSysNo;
        public DateTime OutStockTime;
        public int OutStockUserSysNo;
        public DateTime ReceiveTime;
        public int ReceiveUserSysNo;
        public int FromLocation;
        public int ToLocation;
        public int Status;
        public Hashtable itemHash = new Hashtable(5); //key registerSysNo, value regiserInfo;


        public void Init()
        {
            SysNo = AppConst.IntNull;
            HandoverID = AppConst.StringNull;
            CreateTime = AppConst.DateTimeNull;
            CreateUserSysNo = AppConst.IntNull;
            UpdateTime = AppConst.DateTimeNull;
            UpdateUserSysNo = AppConst.IntNull;
            OutStockTime = AppConst.DateTimeNull;
            OutStockUserSysNo = AppConst.IntNull;
            ReceiveTime = AppConst.DateTimeNull;
            ReceiveUserSysNo = AppConst.IntNull;
            FromLocation = AppConst.IntNull;
            ToLocation = AppConst.IntNull;
            Status = AppConst.IntNull;
            itemHash.Clear();
        }
    }
}
