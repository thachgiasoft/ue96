using System;
using System.Collections;

using Icson.Utils;

namespace Icson.Objects.Sale
{
	/// <summary>
	/// Summary description for SOInvoicePageInfo.
	/// </summary>
	public class SOInvoicePageInfo
	{
		public SOInvoicePageInfo()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		//The Total Weight For This Item
		public int TotalWeight;

		public int ItemLineCount;
		        
		public Hashtable ItemHash = new Hashtable(AppConst.PageMaxLine);

		/// <summary>
		/// 增加一个Item
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool AddItem(SOInvoicePageItemInfo itemInfo)
		{
			int count;
			int realLength; 
			//如果是产品行，去掉不可见的字符的长度<font style='FONT-SIZE: 7pt'>xxx</font>
			if ( itemInfo.IsSOItem )
			{
				if ( itemInfo.ProductName.IndexOf("<font style='FONT-SIZE:")!=-1)
				{
					realLength = itemInfo.ProductName.Length - 36;
				}
				else 
				{
					realLength = itemInfo.ProductName.Length;
				}
				
				count = realLength/AppConst.NameMaxLength;
				if(realLength%AppConst.NameMaxLength != 0)
				{
					count++;
				}
			}
			else
			{//运杂费
				count = 1;
			}
            
			if(ItemLineCount + count > AppConst.PageMaxLine)
			{
				return false;
			}
			ItemLineCount += count;
			ItemHash.Add(itemInfo.ProductID,itemInfo);        
			return true;
		}

		//The Total Money For This Item
		public decimal TotalMoney
		{
			get
			{
				decimal totalMoney = 0;
				if(this.ItemHash.Count<=0)
				{
					return 0;
				}                
				foreach(SOInvoicePageItemInfo item in ItemHash.Values)
				{
					if ( ! item.IsPoint )
						totalMoney += item.SubTotal;
				}				
				return Util.TruncMoney(totalMoney);
			}
		}
	}
}
