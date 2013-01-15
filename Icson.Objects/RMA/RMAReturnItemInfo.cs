using System;
using Icson.Utils;

namespace Icson.Objects.RMA
{
    /// <summary>
    /// Summary description for RMAReturnItemInfo.
    /// </summary>
    public class RMAReturnItemInfo
    {
        public RMAReturnItemInfo()
        {
            Init();
        }

        public int SysNo;
        public int ReturnSysNo;
        public int RegisterSysNo;
        public int TargetProductSysNo;
        public decimal Cost;

        public int ReturnProductType;
        public int AuditStatus;
        public int AuditUserSysNo;
        public DateTime AuditTime;
        public string AuditMemo;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            ReturnSysNo = AppConst.IntNull;
            RegisterSysNo = AppConst.IntNull;
            TargetProductSysNo = AppConst.IntNull;
            Cost = AppConst.DecimalNull;
            ReturnProductType = AppConst.IntNull;
            AuditStatus = AppConst.IntNull;
            AuditUserSysNo = AppConst.IntNull;
            AuditTime = AppConst.DateTimeNull;
            AuditMemo = AppConst.StringNull;
        }


    }
}
