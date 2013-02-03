using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace YoeJoyHelper
{

    /// <summary>
    /// YoeJoy的系统枚举常量
    /// </summary>
    public sealed class YoeJoyEnum
    {
        /// <summary>
        /// 商品浏览排序
        /// YoeJoy商城使用
        /// 默认排序
        ///价格
        ///销量
        ///上架时间
        ///评价
        /// </summary>
        public enum ProductListSortedOrder : int
        {
            Default = 1,
            Price = 2,
            SaledAmount = 3,
            OnlieDate = 4,
            Comment = 5,
        }

    }

    /// <summary>
    /// YoeJoy的系统字典对象
    /// </summary>
    public sealed class YoeJoySystemDic
    {
        public static readonly Dictionary<YoeJoyEnum.ProductListSortedOrder, string> ProductListSortedOrderDic = new Dictionary<YoeJoyEnum.ProductListSortedOrder, string>()
        {
            {YoeJoyEnum.ProductListSortedOrder.Default,"order by p.SysNo"},
            {YoeJoyEnum.ProductListSortedOrder.Price,"order by price"},
            {YoeJoyEnum.ProductListSortedOrder.SaledAmount,"order by p.SysNo"},
            {YoeJoyEnum.ProductListSortedOrder.OnlieDate,"order by p.CreateTime"},
            {YoeJoyEnum.ProductListSortedOrder.Comment,"order by p.SysNo"}
        };
    }

    /// <summary>
    /// YoeJoy的相关配置
    /// </summary>
    public sealed class YoeJoyConfig
    {
        public static readonly string ImgPhysicalPathBase = ConfigurationManager.AppSettings["ImagePhyicalPath"].ToString().Trim();
        public static readonly string ImgVirtualPathBase = ConfigurationManager.AppSettings["ImageVitrualPath"].ToString().Trim();
        public static readonly string HomeDisplayCategoryID1 = ConfigurationManager.AppSettings["HomeDisplayCategoryID1"].ToString().Trim();
        public static readonly string HomeDisplayCategoryID2 = ConfigurationManager.AppSettings["HomeDisplayCategoryID2"].ToString().Trim();
        public static readonly string SiteBaseURL = ConfigurationManager.AppSettings["SiteBaseURL"].ToString().Trim();
        public static readonly string ProductListPagedCount = ConfigurationManager.AppSettings["ProductListPagedCount"].ToString().Trim();
        public static readonly string DESCEncryptKey = ConfigurationManager.AppSettings["DESCEncryptKey"].ToString().Trim();
        public static readonly string WishListPagedCount = ConfigurationManager.AppSettings["WishListPagedCount"].ToString().Trim();
        public static readonly string AlipayReturnURL = ConfigurationManager.AppSettings["AlipayReturnURL"].ToString().Trim();
        public static readonly string AlipayNotifyURL = ConfigurationManager.AppSettings["AlipayNotifyURL"].ToString().Trim();
        public static readonly string AlipaySellerEmailAddress = ConfigurationManager.AppSettings["AlipaySellerEmailURL"].ToString().Trim();
        public static readonly string MyOrderProductListPagedCount = ConfigurationManager.AppSettings["MyOrderProductListPagedCount"].ToString().Trim();
    }
}