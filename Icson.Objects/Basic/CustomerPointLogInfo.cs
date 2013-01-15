using System;
using System.Text;

using Icson.Utils;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// Summary description for CustomerPointLog.
	/// </summary>
	public class CustomerPointLogInfo
	{
		public CustomerPointLogInfo()
		{
			Init();
		}

		public CustomerPointLogInfo(
			int customerSysNo, int pointLogType,
			int pointAmount, string memo)
		{
			this.CustomerSysNo = customerSysNo;
			this.PointLogType = pointLogType;
			this.PointAmount = pointAmount;
			this.CreateTime = DateTime.Now;
			this.Memo = memo;
			//this.LogCheck = this.CalcLogCheck();插入的时候，会计算的。
		}
		
		public int SysNo;
		public int CustomerSysNo;
		public int PointLogType;
		public int PointAmount;
		public DateTime CreateTime;
		public string Memo;
		public string LogCheck;
		public void Init()
		{
			SysNo = AppConst.IntNull;
			CustomerSysNo = AppConst.IntNull;
			PointLogType = AppConst.IntNull;
			PointAmount = AppConst.IntNull;
			CreateTime = AppConst.DateTimeNull;
			Memo = AppConst.StringNull;
			LogCheck = AppConst.StringNull;
		}

		public string CalcLogCheck()
		{
			StringBuilder sb = new StringBuilder(20);
			sb.Append(CustomerSysNo.ToString()).Append(PointLogType.ToString()).Append(CreateTime.ToString(AppConst.DateFormatLong)).Append(PointAmount.ToString()).Append(Memo);
			return Util.GetMD5(sb.ToString());
		}

	}
}
