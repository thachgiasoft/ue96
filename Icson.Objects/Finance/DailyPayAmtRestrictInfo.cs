using System;

using Icson.Utils;

namespace Icson.Objects.Finance
{
    public class DailyPayAmtRestrictInfo
    {
        public DailyPayAmtRestrictInfo()
        {
            Init();
        }

        public int SysNo;
        public decimal TopPublicPayAmt;
        public decimal AllocatedPublicAmt;
        public decimal TopPrivatePayAmt;
        public decimal AllocatedPrivateAmt;
        public int CreateUserSysNo;
        public DateTime CreateTime;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            TopPublicPayAmt = AppConst.DecimalNull;
            AllocatedPublicAmt = AppConst.DecimalNull;
            TopPrivatePayAmt = AppConst.DecimalNull;
            AllocatedPrivateAmt = AppConst.DecimalNull;
            CreateUserSysNo = AppConst.IntNull;
            CreateTime = AppConst.DateTimeNull;
        }
    }
}
