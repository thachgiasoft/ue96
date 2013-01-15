using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Online
{
    public class CustomerTopicInfo
	{
		public CustomerTopicInfo()
        {
            Init();
        }
		public int SysNo;
		public int CustomerSysNo;
		public int TopicCount;
		public int DigestCount;
		public int ReplyCount;
		public int RemarkCount;
		public int LastEditUserSysNo;
		public DateTime LastEditDate;

		public void Init()
		{
			SysNo = AppConst.IntNull;
			CustomerSysNo = AppConst.IntNull;
			TopicCount = AppConst.IntNull;
			DigestCount = AppConst.IntNull;
			ReplyCount = AppConst.IntNull;
			RemarkCount = AppConst.IntNull;
			LastEditUserSysNo = AppConst.IntNull;
			LastEditDate = AppConst.DateTimeNull;
		}
	}
}
