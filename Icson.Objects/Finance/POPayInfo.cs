using System;
using System.Collections;

using Icson.Utils;

namespace Icson.Objects.Finance
{
    /// <summary>
    /// Summary description for POPayInfo.
    /// </summary>
    public class POPayInfo
    {
        public POPayInfo()
        {
            Init();
        }

        Hashtable itemHash = new Hashtable(5);

        public int SysNo;
        public int POSysNo;
        public int CurrencySysNo;
        public decimal POAmt;
        public decimal AlreadyPayAmt;
        public int PayStatus;
        public int InvoiceStatus;
        public DateTime InvoiceTime;
        public string Note;
        public void Init()
        {
            SysNo = AppConst.IntNull;
            POSysNo = AppConst.IntNull;
            CurrencySysNo = AppConst.IntNull;
            POAmt = AppConst.DecimalNull;
            AlreadyPayAmt = AppConst.DecimalNull;
            PayStatus = AppConst.IntNull;
            InvoiceStatus = AppConst.IntNull;
            InvoiceTime = AppConst.DateTimeNull;
            Note = AppConst.StringNull;
        }


    }
}
