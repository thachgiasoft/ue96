using System;

using Icson.Utils;

namespace Icson.Objects.Purchase
{
	/// <summary>
	/// Summary description for POApportionSubject.
	/// </summary>
	public class POApportionSubjectInfo : IComparable
	{
		public POApportionSubjectInfo()
		{
			Init();
		}
		
		public int SysNo;
		public string SubjectName;
		public string ListOrder;
		public int Status;
		public void Init()
		{
			SysNo = AppConst.IntNull;
			SubjectName = AppConst.StringNull;
			ListOrder = AppConst.StringNull;
			Status = AppConst.IntNull;
		}
		#region IComparable Members

		public int CompareTo(object obj)
		{
			POApportionSubjectInfo b = obj as POApportionSubjectInfo;
			int result = String.Compare(this.ListOrder, b.ListOrder);
			if ( result >= 0)
			{
				return 1;
			}
			else
			{
				return -1;
			}
		}

		#endregion
	}
}
