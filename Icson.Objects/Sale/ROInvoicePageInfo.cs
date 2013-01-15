using System;
using System.Collections;

using Icson.Utils;

namespace Icson.Objects.Sale
{
	/// <summary>
	/// Summary description for ROInvoicePageInfo.
	/// </summary>
	public class ROInvoicePageInfo
	{
		public ROInvoicePageInfo()
		{
		}

		public Hashtable ItemHash = new Hashtable(AppConst.PageMaxLine);

		//The Total Weight For This Item
		private int totalWeight;

		//The Total Money For This Item
		public decimal TotalMoney
		{
			get
			{
				decimal totalMoney = 0;
				if(this.ItemHash.Count == 0)
				{
					return 0;
				}                
				foreach(ROInvoicePageItemInfo item in this.ItemHash.Values)
				{
					if ( ! item.isPoint)
						totalMoney += item.Total;
				}
                return Util.TruncMoney(totalMoney);  
                //return totalMoney; //不去零头，否则影响财务统计
			}
		}

		//The Total Weight For This Item
		public int ToTalWeight
		{
			get
			{
				if(this.ItemHash.Count == 0)
				{
					return 0;
				}
				foreach(ROInvoicePageItemInfo item in this.ItemHash.Values)
				{
					this.totalWeight += item.weight;
				}

				return this.totalWeight;
			}
		}

		private int itemLineCount;

		public int ItemLineCount
		{
			get
			{
				return itemLineCount;
			}
		}

        ///// <summary>
        ///// 增加一个Item
        ///// </summary>
        ///// <param name="item"></param>
        ///// <returns></returns>
        //public bool AddItem(ROInvoicePageItemInfo item)
        //{
        //    int count = item.ProductName.Length/AppConst.NameMaxLength;
        //    if(item.ProductName.Length%AppConst.NameMaxLength != 0)
        //    {
        //        count++;
        //    }
            
        //    if(itemLineCount + count > AppConst.NameMaxLength)
        //    {
        //        return false;
        //    }
        //    itemLineCount += count;
        //    this.ItemHash.Add(item.ProductID,item);        
        //    return true;
        //}

        /// <summary>
        /// 增加一个Item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool AddItem(ROInvoicePageItemInfo item)
        {
            int count = item.ProductName.Length / AppConst.NameMaxLength;
            if (item.ProductName.Length % AppConst.NameMaxLength != 0)
            {
                count++;
            }

            if (itemLineCount + count > AppConst.NameMaxLength)
            {
                return false;
            }
            itemLineCount += count;
            this.ItemHash.Add(item.pk, item);
            return true;
        }
	}
}
