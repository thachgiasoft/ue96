using System;
using System.Collections;

using Icson.Utils;

namespace Icson.Objects.Stock
{
    /// <summary>
    /// Summary description for ShiftInfo.
    /// </summary>
    public class ShiftInfo
    {
        public ShiftInfo()
        {
            Init();
        }

        public Hashtable itemHash = new Hashtable(5);

        public int SysNo;
        public string ShiftID;
        public int StockSysNoA;
        public int StockSysNoB;
        public DateTime CreateTime;
        public int CreateUserSysNo;
        public DateTime AuditTime;
        public int AuditUserSysNo;
        public DateTime OutTime;
        public int OutUserSysNo;
        public DateTime InTime;
        public int InUserSysNo;
        public int CheckQtyUserSysNo;
        public DateTime CheckQtyTime;
        public int Status;
        public string Note;
        public int DLSysNo;
        public DateTime SetDeliveryManTime;
        public int FreightUserSysNo;
        public int IsLarge;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            ShiftID = AppConst.StringNull;
            StockSysNoA = AppConst.IntNull;
            StockSysNoB = AppConst.IntNull;
            CreateTime = AppConst.DateTimeNull;
            CreateUserSysNo = AppConst.IntNull;
            AuditTime = AppConst.DateTimeNull;
            AuditUserSysNo = AppConst.IntNull;
            OutTime = AppConst.DateTimeNull;
            OutUserSysNo = AppConst.IntNull;
            InTime = AppConst.DateTimeNull;
            InUserSysNo = AppConst.IntNull;
            CheckQtyUserSysNo = AppConst.IntNull;
            CheckQtyTime = AppConst.DateTimeNull;
            Status = AppConst.IntNull;
            Note = AppConst.StringNull;
            DLSysNo = AppConst.IntNull;
            SetDeliveryManTime = AppConst.DateTimeNull;
            FreightUserSysNo = AppConst.IntNull;
            IsLarge = AppConst.IntNull;
        }
    }
}
