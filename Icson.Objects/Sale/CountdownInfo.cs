using System;

using Icson.Utils;

namespace Icson.Objects.Sale
{
    /// <summary>
    /// Summary description for CountdownInfo.
    /// </summary>
    public class CountdownInfo
    {
        public CountdownInfo()
        {
            Init();
        }

        public int SysNo;
        public int CreateUserSysNo;
        public DateTime CreateTime;
        public int ProductSysNo;
        public DateTime StartTime;
        public DateTime EndTime;
        public decimal CountDownCurrentPrice;
        public decimal CountDownCashRebate;
        public int CountDownPoint;
        public int CountDownQty;
        public decimal SnapShotCurrentPrice;
        public decimal SnapShotCashRebate;
        public int SnapShotPoint;
        public int AffectedVirtualQty;
        public int Status;
        public int Type;
        public void Init()
        {
            SysNo = AppConst.IntNull;
            CreateUserSysNo = AppConst.IntNull;
            CreateTime = AppConst.DateTimeNull;
            ProductSysNo = AppConst.IntNull;
            StartTime = AppConst.DateTimeNull;
            EndTime = AppConst.DateTimeNull;
            CountDownCurrentPrice = AppConst.DecimalNull;
            CountDownCashRebate = AppConst.DecimalNull;
            CountDownPoint = AppConst.IntNull;
            CountDownQty = AppConst.IntNull;
            SnapShotCurrentPrice = AppConst.DecimalNull;
            SnapShotCashRebate = AppConst.DecimalNull;
            SnapShotPoint = AppConst.IntNull;
            AffectedVirtualQty = AppConst.IntNull;
            Status = AppConst.IntNull;
            Type = AppConst.IntNull;
        }
    }
}
