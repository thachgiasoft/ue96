using System;
using Icson.Utils;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// ¼¯³ÉcategoryAttributeInfo name ºÍ productAttributeInfo value
	/// </summary>
	public class AttributeInfo : IComparable
	{
		public AttributeInfo()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		
		public int SysNo = AppConst.IntNull;
		public string AttributeId = AppConst.StringNull;
		public string AttributeName = AppConst.StringNull;
		public string AttributeValue = AppConst.StringNull;
		public int OrderNum = AppConst.IntNull;
		public int Status = AppConst.IntNull;
		//public string AttributeType = AppConst.StringNull;
	
		#region IComparable Members

		public int CompareTo(object obj)
		{
			// TODO:  Add AttributeInfo.CompareTo implementation
			AttributeInfo ai = obj as AttributeInfo;
			if(ai.OrderNum<this.OrderNum)
				return 1;
			else
				return -1;
		}

		#endregion
	}
}
