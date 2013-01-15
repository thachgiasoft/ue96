using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;

namespace YoeJoyHelper.Model
{
    /// <summary>
    /// 用户商品收藏模型类
    /// </summary>
    public class WishListModule
    {
        public string ProductSysNo { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public string CurrentPrice { get; set; }
        //是否无货
        public bool IsEmptyInventory { get; set; }
        public int C1SysNo { get; set; }
        public int C2SysNo { get; set; }
        public int C3Sysno { get; set; }
    }

    public class WishListService
    {

        private static readonly string getPagedCustomerWishListSqlCmdTemplate = @"select distinct top {0} wl.SysNo,wl.ProductSysNo,p.ProductName,pimg.product_simg,CONVERT(float,pp.CurrentPrice) as price, (inev.AvailableQty+inev.VirtualQty) as onlineQty,p.C1SysNo,p.C2SysNo,p.C3SysNo from WishList wl 
  left join Product p on wl.ProductSysNo=p.SysNo
  left join Product_Price pp on wl.ProductSysNo=pp.ProductSysNo
  left join Product_Images pimg on wl.ProductSysNo=pimg.product_sysNo
  left join Inventory inev on wl.ProductSysNo=inev.ProductSysNo
  where p.Status=1
  and (pimg.orderNum=1 and pimg.status=1)
  and wl.SysNo not in 
  (select top {1} wl.SysNo from WishList wl left join
  Product p on wl.ProductSysNo=p.SysNo where p.Status=1 and wl.CustomerSysNo={2} order by wl.SysNo DESC )
  and wl.CustomerSysNo={3}
  order by wl.SysNo DESC";

        /// <summary>
        /// 获得用户收藏的商品信息
        /// </summary>
        /// <param name="customerSysNo"></param>
        /// <returns></returns>
        public static List<WishListModule> GetCustomerWishList(int customerSysNo,int startIndex,int pageCount)
        {
            string sqlCmd = String.Format(getPagedCustomerWishListSqlCmdTemplate, pageCount, startIndex, customerSysNo, customerSysNo);
            try
            {
                DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {
                    List<WishListModule> wishList = new List<WishListModule>();
                    for (int i = 0; i < rowCount; i++)
                    {
                        wishList.Add(new WishListModule()
                        {
                            C1SysNo = int.Parse(data.Rows[i]["C1SysNo"].ToString().Trim()),
                            C2SysNo = int.Parse(data.Rows[i]["C2SysNo"].ToString().Trim()),
                            C3Sysno = int.Parse(data.Rows[i]["C3SysNo"].ToString().Trim()),
                            IsEmptyInventory = (int.Parse(data.Rows[i]["onlineQty"].ToString().Trim()) > 0) ? true : false,
                            ProductSysNo = data.Rows[i]["ProductSysNo"].ToString().Trim(),
                            ProductName = data.Rows[i]["ProductName"].ToString().Trim(),
                            ProductImage = data.Rows[i]["product_simg"].ToString().Trim(),
                            CurrentPrice = data.Rows[i]["price"].ToString().Trim(),
                        });
                    }
                    return wishList;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
