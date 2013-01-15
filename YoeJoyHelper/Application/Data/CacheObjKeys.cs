using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoeJoyHelper
{

    internal class CacheObjSetting
    {
        internal string CacheKey { get; set; }
        internal int CacheDuration { get; set; }
    }

    /// <summary>
    /// 定义所有后台静态对象的cache设定
    /// </summary>
    internal sealed class StaticCacheObjSettings
    {
        internal static readonly CacheObjSetting SiteTopCategoryNavigationCacheSetting = new CacheObjSetting() { CacheKey = "SiteTopCategoryNavigation", CacheDuration = 7200 };
        internal static readonly CacheObjSetting SiteHomeInComingProductsCacheSetting = new CacheObjSetting() { CacheKey = "HomeInComingProducts", CacheDuration = 3600 };
        internal static readonly CacheObjSetting SiteHomeBrandsCacheSetting = new CacheObjSetting() { CacheKey = "HomeBrands", CacheDuration = 7200 };
        internal static readonly CacheObjSetting SiteHomeWebBulletinListCacheSetting = new CacheObjSetting() { CacheKey = "HomeWebBulletinList", CacheDuration = 7200 };
        internal static readonly CacheObjSetting SiteHomePromoProductListCacheSetting = new CacheObjSetting() { CacheKey = "HomePromoProductList", CacheDuration = 7200 };
        internal static readonly CacheObjSetting SiteHomePanicProductListCacheSetting = new CacheObjSetting() { CacheKey = "HomePanicProductList", CacheDuration = 3600 };
        internal static readonly CacheObjSetting SiteHomePromotionBrandsProduct = new CacheObjSetting() { CacheKey = "SiteHomePromotionBrandsProduct", CacheDuration = 7200 };
        internal static readonly CacheObjSetting SearchHotCommentedProduct = new CacheObjSetting() { CacheKey = "SearchHotCommentedProduct", CacheDuration = 7200 };
    }

    /// <summary>
    /// 定义所有后台动态对象的cache设定
    /// </summary>
    internal sealed class DynomicCacheObjSettings
    {
        internal static readonly CacheObjSetting SiteADCacheSetting = new CacheObjSetting() { CacheKey = "SiteAD/{0}", CacheDuration = 14400 };
        internal static readonly CacheObjSetting SiteSubCategoryNavigationCacheSetting = new CacheObjSetting() { CacheKey = "SubCategoryNavigation/{0}", CacheDuration = 14400 };
        internal static readonly CacheObjSetting HomeCategoryOneProductDisplayCacheSettings = new CacheObjSetting() { CacheKey = "HomeCategoryOneProducts/{0}", CacheDuration = 14400 };
        internal static readonly CacheObjSetting HomeCategoryOneProductBrandsDisplayCacheSettings = new CacheObjSetting() { CacheKey = "HomeCategoryOneProductsBrands/{0}", CacheDuration = 14400 };
        internal static readonly CacheObjSetting CategoryOneProductsDisplayCacheSettings = new CacheObjSetting() { CacheKey = "CategoryOneProducts/{0}", CacheDuration = 14400 };
        internal static readonly CacheObjSetting CategoryOneEmptyInventoryProductsCacheSettings = new CacheObjSetting() { CacheKey = "CategoryOneEmptyInventoryProducts/{0}", CacheDuration = 14400 };
        internal static readonly CacheObjSetting CategoryOneLastedDiscountProductsCacheSettings = new CacheObjSetting() { CacheKey = "CategoryOneLastedDiscountProducts/{0}", CacheDuration = 14400 };
        internal static readonly CacheObjSetting CategoryOneWeeklyBestSaledProductsCacheSettings = new CacheObjSetting() { CacheKey = "CategoryOneWeeklyBestSaledProducts/{0}", CacheDuration = 14400 };
        internal static readonly CacheObjSetting ProductBaiscInfoCacheSettings = new CacheObjSetting() { CacheKey = "ProductDetailBasic/{0}", CacheDuration = 3600 };
        internal static readonly CacheObjSetting C3ProductFilterCacheSettings = new CacheObjSetting() { CacheKey = "C3ProductFilter/{0}", CacheDuration = 3600 };
        internal static readonly CacheObjSetting C3BestSaledProductsCacheSettings = new CacheObjSetting() { CacheKey = "C3BestSaledProducts/{0}", CacheDuration = 3600 };
        internal static readonly CacheObjSetting C3HotCommentedProductsCacheSettings = new CacheObjSetting() { CacheKey = "C3HotCommentedProducts/{0}", CacheDuration = 3600 };
        internal static readonly CacheObjSetting HomeHotCommentedProductsCacheSettings = new CacheObjSetting() { CacheKey = "HomeHotCommentedProducts/{0}", CacheDuration = 3600 };
        internal static readonly CacheObjSetting HomeBestSaledProductsCacheSettings = new CacheObjSetting() { CacheKey = "HomeBestSaledProducts/{0}", CacheDuration = 3600 };
        internal static readonly CacheObjSetting CategoryTwoWeeklyBestSaledProductsCacheSettings = new CacheObjSetting() { CacheKey = "CategoryTwoWeeklyBestSaledProducts/{0}", CacheDuration = 14400 };
        internal static readonly CacheObjSetting CategoryTwoProductsDisplayCacheSettings = new CacheObjSetting() { CacheKey = "CategoryTwoProducts/{0}", CacheDuration = 14400 };
        internal static readonly CacheObjSetting CategoryTwoEmptyInventoryProductsCacheSettings = new CacheObjSetting() { CacheKey = "CategoryTwoEmptyInventoryProducts/{0}", CacheDuration = 14400 };
    }

    /// <summary>
    /// 定义所有的共享缓存对象的cache设定
    /// 因此共享的缓存对象设定的过期时间要远远小于页面的部分缓存
    /// </summary>
    internal sealed class SharedCacheObjSettings
    {
        internal static readonly CacheObjSetting CategoryOneListCacheSettings = new CacheObjSetting() { CacheKey = "C1List", CacheDuration = 3600 };
        internal static readonly CacheObjSetting CategoryTwoListCacheSettings = new CacheObjSetting() { CacheKey = "C2List", CacheDuration = 3600 };
        internal static readonly CacheObjSetting CategoryThreeListCacheSettings = new CacheObjSetting() { CacheKey = "C3List", CacheDuration = 3600 };

        #region 备用代码
        //internal static readonly CacheObjSetting CategoryOneHashCacheSettings = new CacheObjSetting() { CacheKey = "C1Hash", CacheDuration = 3600 };
        //internal static readonly CacheObjSetting CategoryTwoHashCacheSettings = new CacheObjSetting() { CacheKey = "C2Hash", CacheDuration = 3600 };
        //internal static readonly CacheObjSetting CategoryThreeHashCacheSettings = new CacheObjSetting() { CacheKey = "C3Hash", CacheDuration = 3600 };
        #endregion
    }
}
