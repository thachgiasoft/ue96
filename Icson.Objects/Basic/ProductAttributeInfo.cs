using System;
using System.Collections;
using Icson.Utils;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// Summary description for ProductAttributeInfo.
	/// </summary>
	public class ProductAttributeInfo
	{
		public ProductAttributeInfo()
		{
			InitAttributeList();
		}
		
		public int SysNo = AppConst.IntNull;
		public int ProductSysNo = AppConst.IntNull;
		public string A1 = AppConst.StringNull;
		public string A2 = AppConst.StringNull;
		public string A3 = AppConst.StringNull;
		public string A4 = AppConst.StringNull;
		public string A5 = AppConst.StringNull;
		public string A6 = AppConst.StringNull;
		public string A7 = AppConst.StringNull;
		public string A8 = AppConst.StringNull;
		public string A9 = AppConst.StringNull;
		public string A10 = AppConst.StringNull;
		public string A11 = AppConst.StringNull;
		public string A12 = AppConst.StringNull;
		public string A13 = AppConst.StringNull;
		public string A14 = AppConst.StringNull;
		public string A15 = AppConst.StringNull;
		public string A16 = AppConst.StringNull;
		public string A17 = AppConst.StringNull;
		public string A18 = AppConst.StringNull;
		public string A19 = AppConst.StringNull;
		public string A20 = AppConst.StringNull;
		public string A21 = AppConst.StringNull;
		public string A22 = AppConst.StringNull;
		public string A23 = AppConst.StringNull;
		public string A24 = AppConst.StringNull;
		public string A25 = AppConst.StringNull;
		public string A26 = AppConst.StringNull;
		public string A27 = AppConst.StringNull;
		public string A28 = AppConst.StringNull;
		public string A29 = AppConst.StringNull;
		public string A30 = AppConst.StringNull;

		private SortedList AttributeList = new SortedList();


		private void InitAttributeList()
		{
			AttributeList.Add("A1",A1);
			AttributeList.Add("A2",A2);
			AttributeList.Add("A3",A3);
			AttributeList.Add("A4",A4);
			AttributeList.Add("A5",A5);
			AttributeList.Add("A6",A6);
			AttributeList.Add("A7",A7);
			AttributeList.Add("A8",A8);
			AttributeList.Add("A9",A9);
			AttributeList.Add("A10",A10);
			AttributeList.Add("A11",A11);
			AttributeList.Add("A12",A12);
			AttributeList.Add("A13",A13);
			AttributeList.Add("A14",A14);
			AttributeList.Add("A15",A15);
			AttributeList.Add("A16",A16);
			AttributeList.Add("A17",A17);
			AttributeList.Add("A18",A18);
			AttributeList.Add("A19",A19);
			AttributeList.Add("A20",A20);
			AttributeList.Add("A21",A21);
			AttributeList.Add("A22",A22);
			AttributeList.Add("A23",A23);
			AttributeList.Add("A24",A24);
			AttributeList.Add("A25",A25);
			AttributeList.Add("A26",A26);
			AttributeList.Add("A27",A27);
			AttributeList.Add("A28",A28);
			AttributeList.Add("A29",A29);
			AttributeList.Add("A30",A30);
		}

		public SortedList GetAttributeList()
		{
			return AttributeList;
		}

		public void SetSLAttribute(string paramID,string paramValue)
		{
			AttributeList.SetByIndex(AttributeList.IndexOfKey(paramID),paramValue);
		}
		
		private string GetAttributeValue(string paramAttributeID)
		{
			return (string)AttributeList.GetByIndex(AttributeList.IndexOfKey(paramAttributeID));
		}

		public void GetValuesFromSL()
		{
			this.A1 = GetAttributeValue("A1");
			this.A2 = GetAttributeValue("A2");
			this.A3 = GetAttributeValue("A3");
			this.A4 = GetAttributeValue("A4");
			this.A5 = GetAttributeValue("A5");
			this.A6 = GetAttributeValue("A6");
			this.A7 = GetAttributeValue("A7");
			this.A8 = GetAttributeValue("A8");
			this.A9 = GetAttributeValue("A9");
			this.A10 = GetAttributeValue("A10");
			this.A11 = GetAttributeValue("A11");
			this.A12 = GetAttributeValue("A12");
			this.A13 = GetAttributeValue("A13");
			this.A14 = GetAttributeValue("A14");
			this.A15 = GetAttributeValue("A15");
			this.A16 = GetAttributeValue("A16");
			this.A17 = GetAttributeValue("A17");
			this.A18 = GetAttributeValue("A18");
			this.A19 = GetAttributeValue("A19");
			this.A20 = GetAttributeValue("A20");
			this.A21 = GetAttributeValue("A21");
			this.A22 = GetAttributeValue("A22");
			this.A23 = GetAttributeValue("A23");
			this.A24 = GetAttributeValue("A24");
			this.A25 = GetAttributeValue("A25");
			this.A26 = GetAttributeValue("A26");
			this.A27 = GetAttributeValue("A27");
			this.A28 = GetAttributeValue("A28");
			this.A29 = GetAttributeValue("A29");
			this.A30 = GetAttributeValue("A30");
		}
	}
}
