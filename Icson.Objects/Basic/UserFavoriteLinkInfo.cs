using System;

using Icson.Utils;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// Summary description for UserFavoriteLinkInfo.
	/// </summary>
	public class UserFavoriteLinkInfo : IComparable
	{
		public UserFavoriteLinkInfo()
		{
			Init();
		}
		
		public int SysNo;
		public int UserSysNo;
		public string LinkName;
		public string LinkAhref;
		public DateTime CreateTime;
		public void Init()
		{
			SysNo = AppConst.IntNull;
			UserSysNo = AppConst.IntNull;
			LinkName = AppConst.StringNull;
			LinkAhref = AppConst.StringNull;
			CreateTime = AppConst.DateTimeNull;
		}

		public override string ToString()
		{
			return "<a href=\"" + LinkAhref + "\">" + LinkName + "</a>";
		}
		#region IComparable Members

		public int CompareTo(object obj)
		{
			UserFavoriteLinkInfo b = obj as UserFavoriteLinkInfo;
			if ( this.SysNo > b.SysNo )
				return 1;
			else 
				return -1;
		}

		#endregion
	}
}
