using System;
using Icson.Utils;

namespace Icson.Objects.Finance
{
	/// <summary>
	/// Summary description for NetPayInfo.
	/// </summary>
	public class NetPayInfo
	{
		public NetPayInfo()
		{
			Init();
		}
		
		public int SysNo;
		public int SOSysNo;
		public int PayTypeSysNo;
		public decimal PayAmount;
		public int Source;
		public DateTime InputTime;
		public int InputUserSysNo;
		public int ApproveUserSysNo;
		public DateTime ApproveTime;
		public string Note;
		public int Status;
		public void Init()
		{
			SysNo = AppConst.IntNull;
			SOSysNo = AppConst.IntNull;
			PayTypeSysNo = AppConst.IntNull;
			PayAmount = AppConst.DecimalNull;
			Source = AppConst.IntNull;
			InputTime = AppConst.DateTimeNull;
			InputUserSysNo = AppConst.IntNull;
			ApproveUserSysNo = AppConst.IntNull;
			ApproveTime = AppConst.DateTimeNull;
			Note = AppConst.StringNull;
			Status = AppConst.IntNull;
		}
	}
}
