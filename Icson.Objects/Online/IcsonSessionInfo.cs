using System;
using System.Collections;

using Icson.Utils;
using Icson.Objects.Basic;
using Icson.Objects.Sale;

namespace Icson.Objects.Online
{
	/// <summary>
	/// Summary description for IcsonSessionInfo.
	/// </summary>
	public class IcsonSessionInfo
	{
		public IcsonSessionInfo()
		{
			Init();
		}

		public void Init()
		{
			Catalog = AppConst.StringNull;
			sCustomer = null;
			sSO = null;
			sRMA = null;
			GiftHash = new Hashtable(5);
            BrowseHistoryList = new ArrayList(5);
		}

		public string Catalog; // ÏêÏ¸¼ûpage head
		public CustomerInfo sCustomer;

		public Hashtable GiftHash;
		public SOInfo sSO;
		public RMAInfo sRMA;
	    public ArrayList BrowseHistoryList;
	}
}
