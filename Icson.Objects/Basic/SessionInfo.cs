using System;
using System.Collections;

using Icson.Objects.Stock;
using Icson.Objects.Purchase;
using Icson.Objects.Sale;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// Summary description for SessionInfo.
	/// </summary>
	public class SessionInfo
	{
		public SessionInfo()
		{
			User = new UserInfo();
		}
		public UserInfo User;
		public string IpAddress;
		public Hashtable PrivilegeHt;
		public SortedList FavoriteLinkList;

		public LendInfo sLend;
		public AdjustInfo sAdjust;
		public ShiftInfo sShift;
		public TransferInfo sTransfer;

		public POInfo sPO;

		public SOInfo sSO;
		public CustomerInfo sCustomer;
		public RMAInfo sRMA;
	}
}