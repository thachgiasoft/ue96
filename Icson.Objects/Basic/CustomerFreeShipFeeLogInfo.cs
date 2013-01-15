using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Basic
{
    public class CustomerFreeShipFeeLogInfo
    {
        public int SysNo;
        public int CustomerSysNo;
        public int FreeShipFeeLogType;
        public decimal FreeShipFeeAmount;
        public DateTime CreateTime;
        public string Memo;
        public string LogCheck;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            CustomerSysNo = AppConst.IntNull;
            FreeShipFeeLogType = AppConst.IntNull;
            FreeShipFeeAmount = AppConst.DecimalNull;
            CreateTime = AppConst.DateTimeNull;
            Memo = AppConst.StringNull;
            LogCheck = AppConst.StringNull;
        }

        public CustomerFreeShipFeeLogInfo(int customerSysNo, int freeShipFeeLogType,decimal freeShipFeeAmount, string memo)
		{
			this.CustomerSysNo = customerSysNo;
            this.FreeShipFeeLogType = freeShipFeeLogType;
            this.FreeShipFeeAmount = freeShipFeeAmount;
			this.CreateTime = DateTime.Now;
			this.Memo = memo;
			//this.LogCheck = this.CalcLogCheck();插入的时候，会计算的。
		}
		
		public string CalcLogCheck()
		{
			StringBuilder sb = new StringBuilder(20);
			sb.Append(CustomerSysNo.ToString()).Append(FreeShipFeeLogType.ToString()).Append(CreateTime.ToString(AppConst.DateFormatLong)).Append(FreeShipFeeAmount.ToString()).Append(Memo);
			return Util.GetMD5(sb.ToString());
		}
    }
}
