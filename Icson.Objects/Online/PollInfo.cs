using System;
using System.Collections;

using Icson.Utils;

namespace Icson.Objects.Online
{
	/// <summary>
	/// Summary description for PollInfo.
	/// </summary>
	public class PollInfo
	{
		public PollInfo()
		{
			Init();
		}

		public Hashtable itemHash = new Hashtable(5); //key itemsysno, value polliteminfo
		public int SysNo;
		public string PollName;
		public int PollCount;
		public int Status;
		public void Init()
		{
			SysNo = AppConst.IntNull;
			PollName = AppConst.StringNull;
			PollCount = AppConst.IntNull;
			Status = AppConst.IntNull;
		}
	}
}
