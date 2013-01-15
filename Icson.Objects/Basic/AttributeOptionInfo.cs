using System;
using Icson.Utils;

namespace Icson.Objects.Basic
{
	public class AttributeOptionInfo : IComparable
	{
		public AttributeOptionInfo()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		
		public int SysNo = AppConst.IntNull;
		public int AttributeSysNo = AppConst.IntNull;
		public string AttributeOptionName = AppConst.StringNull;
		public int OrderNum = AppConst.IntNull;
		public int IsCommend = AppConst.IntNull;
		public int Status = AppConst.IntNull;
		public string AttributeID = AppConst.StringNull;
		public string AttributeName = AppConst.StringNull;
	
		#region IComparable Members

		public int CompareTo(object obj)
		{
			// TODO:  Add AttributeInfo.CompareTo implementation
			AttributeOptionInfo ai = obj as AttributeOptionInfo;
			if(ai.OrderNum<this.OrderNum)
				return 1;
			else
				return -1;
		}

		#endregion
	}
}
