using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Finance
{
    public class SOIncomeVoucherInfo
    {
        public int SysNo;
        public int FSISysNo;
        public string VoucherID;
        public DateTime VoucherTime;
        public int SysUserSysNo;
        public DateTime DateStamp;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            FSISysNo = AppConst.IntNull;
            VoucherID = AppConst.StringNull;
            VoucherTime = AppConst.DateTimeNull;
            SysUserSysNo = AppConst.IntNull;
            DateStamp = AppConst.DateTimeNull;
        }
    }
}