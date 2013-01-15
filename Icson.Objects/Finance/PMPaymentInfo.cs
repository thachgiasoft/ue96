using System;
using System.Collections;
using Icson.Utils;

namespace Icson.Objects.Finance
{
    public class PMPaymentInfo
    {
        public PMPaymentInfo()
        {
            Init();    
        }

        public int SysNo;
        public DateTime PayDate;
        public int PMSysNo;
        public decimal PayAmt;
        public decimal StockAmt;
        public DateTime DateStamp;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            PayDate = AppConst.DateTimeNull;
            PMSysNo = AppConst.IntNull;
            PayAmt = AppConst.DecimalNull;
            StockAmt = AppConst.DecimalNull;
            DateStamp = AppConst.DateTimeNull;
        }
    }
}
