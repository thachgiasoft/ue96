using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YoeJoyHelper.Model;

namespace YoeJoyHelper
{
    /// <summary>
    /// 前台商类品展示的Helper
    /// </summary>
    public class FrontProductsHelper
    {

        public static string GetHomeCategoryOneProductsDisplayHTMLWrapper(string categoryOneId)
        {
            CacheObjSetting cacheSetting = DynomicCacheObjSettings.HomeCategoryOneProductDisplayCacheSettings;
            string key = String.Format(cacheSetting.CacheKey, categoryOneId);
            int duration = cacheSetting.CacheDuration;
            string homeCategoryOneProductsDisplayHTML = CacheObj<string>.GetCachedObj(key, duration, GetHomeCategoryOneProductsDisplayHTML(categoryOneId));
            return homeCategoryOneProductsDisplayHTML;
        }

        public static string GetC1ProductsDisplayHTMLWrapper(int categoryOneId)
        {
            CacheObjSetting cacheSetting = DynomicCacheObjSettings.CategoryOneProductsDisplayCacheSettings;
            string key = String.Format(cacheSetting.CacheKey, categoryOneId);
            int duration = cacheSetting.CacheDuration;
            string c1ProductsDisplayHTML = CacheObj<string>.GetCachedObj(key, duration, GetC1ProductsDisplayHTML(categoryOneId));
            return c1ProductsDisplayHTML;
        }

        public static string GetC2ProductsDisplayHTMLWrapper(int c1SysNo, int c2SysNo)
        {
            CacheObjSetting cacheSetting = DynomicCacheObjSettings.CategoryTwoProductsDisplayCacheSettings;
            string key = String.Format(cacheSetting.CacheKey, c2SysNo);
            int duration = cacheSetting.CacheDuration;
            string c1ProductsDisplayHTML = CacheObj<string>.GetCachedObj(key, duration, GetC2ProductsDisplayHTML(c1SysNo, c2SysNo));
            return c1ProductsDisplayHTML;
        }

        public static string GetC1EmptyInventoryProductsHTMLWrapper(int categoryOneId)
        {
            CacheObjSetting cacheSetting = DynomicCacheObjSettings.CategoryOneEmptyInventoryProductsCacheSettings;
            string key = String.Format(cacheSetting.CacheKey, categoryOneId);
            int duration = cacheSetting.CacheDuration;
            string c1EmptyInventoryProductsHTML = CacheObj<string>.GetCachedObj(key, duration, GetC1EmptyInventoryProductsHTML(categoryOneId));
            return c1EmptyInventoryProductsHTML;
        }

        public static string GetC2EmptyInventoryProductsHTMLWrapper(int c1SysNo, int c2SysNo)
        {
            CacheObjSetting cacheSetting = DynomicCacheObjSettings.CategoryTwoEmptyInventoryProductsCacheSettings;
            string key = String.Format(cacheSetting.CacheKey, c2SysNo);
            int duration = cacheSetting.CacheDuration;
            string c1EmptyInventoryProductsHTML = CacheObj<string>.GetCachedObj(key, duration, GetC2EmptyInventoryProductsHTML(c1SysNo, c2SysNo));
            return c1EmptyInventoryProductsHTML;
        }

        public static string GetC1LastedDisCountProductsHTMLWrapper(int categoryOneId)
        {
            CacheObjSetting cacheSetting = DynomicCacheObjSettings.CategoryOneLastedDiscountProductsCacheSettings;
            string key = String.Format(cacheSetting.CacheKey, categoryOneId);
            int duration = cacheSetting.CacheDuration;
            string c1LastedDisCountProductsHTML = CacheObj<string>.GetCachedObj(key, duration, GetC1LastedDisCountProductsHTML(categoryOneId));
            return c1LastedDisCountProductsHTML;
        }

        public static string GetC1WeeklyBestSaledProductsHTMLWrapper(int categoryOneId)
        {
            CacheObjSetting cacheSetting = DynomicCacheObjSettings.CategoryOneWeeklyBestSaledProductsCacheSettings;
            string key = String.Format(cacheSetting.CacheKey, categoryOneId);
            int duration = cacheSetting.CacheDuration;
            string c1WeeklyBestSaledProductsHTML = CacheObj<string>.GetCachedObj(key, duration, GetC1WeeklyBestSaledProductsHTML(categoryOneId));
            return c1WeeklyBestSaledProductsHTML;
        }

        public static string GetC2WeeklyBestSaledProductsHTMLWrapper(int c1SysNo, int c2SyNo)
        {
            CacheObjSetting cacheSetting = DynomicCacheObjSettings.CategoryTwoWeeklyBestSaledProductsCacheSettings;
            string key = String.Format(cacheSetting.CacheKey, c2SyNo);
            int duration = cacheSetting.CacheDuration;
            string c1WeeklyBestSaledProductsHTML = CacheObj<string>.GetCachedObj(key, duration, GetC2WeeklyBestSaledProductsHTML(c1SysNo, c2SyNo));
            return c1WeeklyBestSaledProductsHTML;
        }

        public static string GetHomePromotionProductsHTMWrapper()
        {
            CacheObjSetting cacheSetting = StaticCacheObjSettings.SiteHomePromoProductListCacheSetting;
            string key = cacheSetting.CacheKey;
            int duration = cacheSetting.CacheDuration;
            string homePromotion = CacheObj<string>.GetCachedObj(key, duration, GetHomePromotionProductsHTML());
            return homePromotion;
        }

        public static string GetC3BestSaledProductHTMLWrapper(int c1sysNo, int c2SsyNo, int c3SysNo)
        {
            CacheObjSetting cacheSetting = DynomicCacheObjSettings.C3BestSaledProductsCacheSettings;
            string key = String.Format(cacheSetting.CacheKey, c3SysNo);
            int duration = cacheSetting.CacheDuration;
            string c3BestSaledProductHTML = CacheObj<string>.GetCachedObj(key, duration, GetC3BestSaledProductHTML(c1sysNo, c2SsyNo, c3SysNo));
            return c3BestSaledProductHTML;
        }

        public static string GetC3HotCommentedProductHTMLWrapper(int c1sysNo, int c2SsyNo, int c3SysNo)
        {
            CacheObjSetting cacheSetting = DynomicCacheObjSettings.C3HotCommentedProductsCacheSettings;
            string key = String.Format(cacheSetting.CacheKey, c3SysNo);
            int duration = cacheSetting.CacheDuration;
            string c3HotCommentedProductHTML = CacheObj<string>.GetCachedObj(key, duration, GetC3HotCommentedProductHTML(c1sysNo, c2SsyNo, c3SysNo));
            return c3HotCommentedProductHTML;
        }

        public static string GetHomeHotCommentedProductHTMLWrapper(int cacheKey, int startIndex, int endIndex)
        {
            CacheObjSetting cacheSetting = DynomicCacheObjSettings.HomeHotCommentedProductsCacheSettings;
            string key = String.Format(cacheSetting.CacheKey, cacheKey);
            int duration = cacheSetting.CacheDuration;
            string HomeHotCommentedProductHTML = CacheObj<string>.GetCachedObj(key, duration, GetHomeHotCommentedProductHTML(startIndex, endIndex));
            return HomeHotCommentedProductHTML;
        }

        public static string GetHomeBestSaledProductHTMLWrapper(int cacheKey, int startIndex, int endIndex)
        {
            CacheObjSetting cacheSetting = DynomicCacheObjSettings.HomeBestSaledProductsCacheSettings;
            string key = String.Format(cacheSetting.CacheKey, cacheKey);
            int duration = cacheSetting.CacheDuration;
            string HomeBestSaledProductHTML = CacheObj<string>.GetCachedObj(key, duration, GetHomeBestSaledProductHTML(startIndex, endIndex));
            return HomeBestSaledProductHTML;
        }

        public static string GetHomePromotionBrandsProductsWrapper()
        {
            CacheObjSetting cacheSetting = StaticCacheObjSettings.SiteHomePromotionBrandsProduct;
            string key = cacheSetting.CacheKey;
            int duration = cacheSetting.CacheDuration;
            string HomeBestSaledProductHTML = CacheObj<string>.GetCachedObj(key, duration, GetHomePromotionBrandsProductsHTML());
            return HomeBestSaledProductHTML;
        }

        public static string InitC3ProductFilterWrapper(int c3SysNo)
        {
            CacheObjSetting cacheSetting = DynomicCacheObjSettings.CategoryOneWeeklyBestSaledProductsCacheSettings;
            string key = String.Format(cacheSetting.CacheKey, c3SysNo);
            int duration = cacheSetting.CacheDuration;
            string c3ProductFilterHTML = CacheObj<string>.GetCachedObj(key, duration, InitC3ProductFilter(c3SysNo));
            return c3ProductFilterHTML;
        }

        public static string GetSearchHotCommentedProductsHTMLWrapper()
        {
            CacheObjSetting cacheSetting = StaticCacheObjSettings.SearchHotCommentedProduct;
            string key = cacheSetting.CacheKey;
            int duration = cacheSetting.CacheDuration;
            string searchHotCommentedProductsHTML = CacheObj<string>.GetCachedObj(key, duration, GetSearchHotCommentedProductsHTML());
            return searchHotCommentedProductsHTML;
        }

        /// <summary>
        /// 首页大类商品展示的HTML代码
        /// </summary>
        /// <param name="categoryOneId"></param>
        /// <returns></returns>
        public static string GetHomeCategoryOneProductsDisplayHTML(string categoryOneId)
        {
            string homeCategoryOneProductsDisplayHTML = String.Empty;
            Dictionary<int, string> c2IdDic = HomeCategoryOneProductService.GetCategoryTwoIDs(categoryOneId);
            if (c2IdDic != null)
            {
                StringBuilder strb = new StringBuilder("<div class='sort'>");

                strb.Append(@"<ul class='sortHeader'>");
                foreach (int key in c2IdDic.Keys)
                {
                    strb.Append(String.Concat("<li><a  href='#'>", c2IdDic[key].ToString().Trim(), "</a></li>"));
                }
                strb.Append("</ul>");
                strb.Append(@"<div class='main'>");
                string baseURL = YoeJoyConfig.SiteBaseURL;
                string deeplinkTemplate = "{0}Pages/Product.aspx?c1={1}&c2={2}&c3={3}&pid={4}";
                string productsItemHTMLTempate = @"<li>
                                    <div>
                                        <h3>
                                            <a href='{0}'>
                                                <img alt='产品图片' src='{1}' width='140' height='140' /></a></h3>
                                        <p>
                                            <a href='{2}' title='{3}' class='name'>{4}</a>
                                        </p>
                                        <p> <span class='adText'>
                                         {5}</span> </p>
                                        <p class='price'><b>¥{6}</b><span>¥{7}</span></p>
                                    </div>
                                </li>";
                string imageBasePath = YoeJoyConfig.ImgVirtualPathBase;
                foreach (int key in c2IdDic.Keys)
                {
                    List<FrontDsiplayProduct> products = HomeCategoryOneProductService.GetHomeCategoryOneDisplayProducts(key);
                    strb.Append(" <div class='sort1Con'><ul class='product sortContent'>");
                    if (products != null)
                    {
                        for (int i = 0; i < products.Count; i++)
                        {
                            FrontDsiplayProduct product = products[i];
                            string imagePath = imageBasePath + product.ImgPath;
                            string deeplink = String.Format(deeplinkTemplate, baseURL, product.C1SysNo, product.C2SysNo, product.C3SysNo, product.ProductSysNo);
                            strb.Append(String.Format(productsItemHTMLTempate, deeplink, imagePath, deeplink, product.ProductBriefName, product.ProductBriefName, product.ProductPromotionWord, product.Price, product.BaiscPrice));
                        }
                        strb.Append("</ul></div>");
                    }
                }
                strb.Append("</div>");
                strb.Append("</div>");
                homeCategoryOneProductsDisplayHTML = strb.ToString();
            }
            return homeCategoryOneProductsDisplayHTML;
        }

        /// <summary>
        /// 获得大类下的所有二类展示的商品
        /// </summary>
        /// <param name="categoryOneId"></param>
        /// <returns></returns>
        public static string GetC1ProductsDisplayHTML(int c1SysNo)
        {
            string c1ProductsDisplayHTML = String.Empty;
            Dictionary<int, string> c2IdDic = C1DisplayProductService.GetC2DsiplayIDs(c1SysNo);
            if (c2IdDic != null)
            {
                StringBuilder strb = new StringBuilder();

                string siteBaseURL = YoeJoyConfig.SiteBaseURL;
                string imageBasePath = YoeJoyConfig.ImgVirtualPathBase;

                string c2DeeplinkTemplate = siteBaseURL + "Pages/SubProductList2.aspx" + "?c1=" + c1SysNo + "&c2={0}";

                foreach (int key in c2IdDic.Keys)
                {
                    string c2Deeplink = String.Format(c2DeeplinkTemplate, key);
                    strb.Append(String.Format(@"<div class='chocolate'>
            <h2>
                <span>{0}</span> <a href='{1}'>更多&gt;&gt;</a>
            </h2>
            <div class='mainShow'>
                <div class='mainProduct'>", c2IdDic[key].ToString().Trim(), c2Deeplink));

                    strb.Append("<ul class='product'>");

                    List<FrontDsiplayProduct> c2Products = C1DisplayProductService.GetC2DsiplayProducts(key);
                    if (c2Products != null)
                    {
                        string productsItemHTMLTempate = @"<li>
                                    <div>
                                        <h3>
                                            <a href='{0}'>
                                                <img alt='产品图片' src='{1}' width='140' height='140'/></a></h3>
                                        <p>
                                            <a href='{2}' title='{3}' class='name'>{4}</a>
                                        </p>
                                        <p> <span class='adText'>
                                         {5}</span> </p>
                                        <p class='price'><b>¥{6}</b><span>¥{7}</span></p>
                                    </div>
                                </li>";

                        foreach (FrontDsiplayProduct c2Product in c2Products)
                        {
                            string imagePath = imageBasePath + c2Product.ImgPath;
                            string deeplink = YoeJoyConfig.SiteBaseURL + "pages/product.aspx?c1=" + c1SysNo + "&c2=" + key + "&c3=" + c2Product.C3SysNo + "&pid=" + c2Product.ProductSysNo;
                            strb.Append(String.Format(productsItemHTMLTempate, deeplink, imagePath, deeplink, c2Product.ProductBriefName, c2Product.ProductBriefName, c2Product.ProductPromotionWord, c2Product.Price, c2Product.BaiscPrice));
                        }

                    }
                    strb.Append("</ul></div>");
                    ADModuleForSite ad = ADService.GetHomeAdByPosition(key);
                    if (ad != null)
                    {
                        string c2AdHTMLTemplate = @"<div class='hot'><a href='{0}' target='_blank'><img width='192' height='360' src='{1}' alt='{2}'></img></a></div>";
                        strb.Append(String.Format(c2AdHTMLTemplate, ad.ADLink, String.Concat(imageBasePath, ad.ADImg), ad.ADName));
                    }
                    strb.Append("</div></div>");
                }
                c1ProductsDisplayHTML = strb.ToString();
            }
            return c1ProductsDisplayHTML;
        }

        /// <summary>
        /// 获得二类下的所有三类展示的商品
        /// </summary>
        /// <param name="categoryOneId"></param>
        /// <returns></returns>
        public static string GetC2ProductsDisplayHTML(int c1SysNo, int c2SysNo)
        {
            string c2ProductsDisplayHTML = String.Empty;
            Dictionary<int, string> c3IdDic = C2DisplayProductService.GetC3DsiplayIDs(c2SysNo);
            if (c3IdDic != null)
            {
                StringBuilder strb = new StringBuilder();

                string siteBaseURL = YoeJoyConfig.SiteBaseURL;
                string imageBasePath = YoeJoyConfig.ImgVirtualPathBase;

                string c3DeeplinkTemplate = siteBaseURL + "Pages/SubProductList3.aspx" + "?c1=" + c1SysNo + "&c2=" + c2SysNo + "&c3={0}";

                foreach (int key in c3IdDic.Keys)
                {
                    string c3Deeplink = String.Format(c3DeeplinkTemplate, key);
                    strb.Append(String.Format(@"<div class='chocolate'>
            <h2>
                <span>{0}</span> <a href='{1}'>更多&gt;&gt;</a>
            </h2>
            <div class='mainShow'>
                <div class='mainProduct'>", c3IdDic[key].ToString().Trim(), c3Deeplink));

                    strb.Append("<ul class='product'>");

                    List<FrontDsiplayProduct> c3Products = C2DisplayProductService.GetC3DsiplayProducts(key);
                    if (c3Products != null)
                    {
                        string productsItemHTMLTempate = @"<li>
                                    <div>
                                        <h3>
                                            <a href='{0}'>
                                                <img alt='产品图片' src='{1}' width='140' height='140'/></a></h3>
                                        <p>
                                            <a href='{2}' title='{3}' class='name'>{4}</a>
                                        </p>
                                        <p> <span class='adText'>
                                         {5}</span> </p>
                                        <p class='price'><b>¥{6}</b><span>¥{7}</span></p>
                                    </div>
                                </li>";

                        foreach (FrontDsiplayProduct c2Product in c3Products)
                        {
                            string imagePath = imageBasePath + c2Product.ImgPath;
                            string deeplink = YoeJoyConfig.SiteBaseURL + "pages/product.aspx?c1=" + c1SysNo + "&c2=" + c2SysNo + "&c3=" + key + "&pid=" + c2Product.ProductSysNo;
                            strb.Append(String.Format(productsItemHTMLTempate, deeplink, imagePath, deeplink, c2Product.ProductBriefName, c2Product.ProductBriefName, c2Product.ProductPromotionWord, c2Product.Price, c2Product.BaiscPrice));
                        }

                    }
                    strb.Append("</ul></div>");
                    List<ADModuleForSite> ads = ADService.GetSlideAdByPosition(key);
                    if (ads != null)
                    {
                        strb.Append("<div class='hot'>");
                        string c3AdHTMLTemplate = @"<a href='{0}' target='_blank'><img src='{1}' alt='{2}'></img></a>";
                        foreach (ADModuleForSite ad in ads)
                        {
                            strb.Append(String.Format(c3AdHTMLTemplate, ad.ADLink, String.Concat(imageBasePath, ad.ADImg), ad.ADName));
                        }
                        strb.Append("</div>");
                    }
                    strb.Append("</div></div>");
                }
                c2ProductsDisplayHTML = strb.ToString();
            }
            return c2ProductsDisplayHTML;
        }

        /// <summary>
        /// 获得大类的清库商品
        /// </summary>
        /// <param name="categoryOneId"></param>
        /// <returns></returns>
        public static string GetC1EmptyInventoryProductsHTML(int categoryOneId)
        {
            string c1EmptyInventoryProductsHTML = String.Empty;
            List<C1WeeklyBestSaledProduct> products = C1EmptyInventoryProductService.GetGetC1EmptyInventoryProducts(categoryOneId);
            StringBuilder strb = new StringBuilder();
            if (products != null)
            {
                string emptyInventoryItemHTML = @"<dd>
                    <h2>
                        <a href='{0}'>
                            <img src='{1}'></a></h2>
                        <p>
              <a class='name' title='{2}' href='{3}'>{4}</a>
              <em class='price'><b>¥{5}</b><span>¥{6}</span></em>
            </p>
                </dd>";

                foreach (C1WeeklyBestSaledProduct product in products)
                {
                    string thumbImg = YoeJoyConfig.ImgVirtualPathBase + product.ImgPath;
                    string deeplink = YoeJoyConfig.SiteBaseURL + "Pages/Product.aspx?c1=" + categoryOneId + "&c2=" + product.C2SysNo + "&c3=" + product.C3SysNo + "&pid=" + product.ProductSysNo;
                    strb.Append(String.Format(emptyInventoryItemHTML, deeplink, thumbImg, product.ProductBriefName, deeplink, product.ProductBriefName, product.Price, product.BaiscPrice));
                }
                c1EmptyInventoryProductsHTML = strb.ToString();
            }
            return c1EmptyInventoryProductsHTML;
        }

        /// <summary>
        /// 获得二类的清库商品
        /// </summary>
        /// <param name="categoryOneId"></param>
        /// <returns></returns>
        public static string GetC2EmptyInventoryProductsHTML(int c1SysNo, int c2SysNo)
        {
            string c1EmptyInventoryProductsHTML = String.Empty;
            List<C1WeeklyBestSaledProduct> products = C2EmptyInventoryProductService.GetGetC2EmptyInventoryProducts(c2SysNo);
            StringBuilder strb = new StringBuilder();
            if (products != null)
            {
                string emptyInventoryItemHTML = @"<dd>
                    <h2>
                        <a href='{0}'>
                            <img src='{1}'></a></h2>
                        <p>
              <a class='name' title='{2}' href='{3}'>{4}</a>
              <em class='price'><b>¥{5}</b><span>¥{6}</span></em>
            </p>
                </dd>";

                foreach (C1WeeklyBestSaledProduct product in products)
                {
                    string thumbImg = YoeJoyConfig.ImgVirtualPathBase + product.ImgPath;
                    string deeplink = YoeJoyConfig.SiteBaseURL + "Pages/Product.aspx?c1=" + c1SysNo + "&c2=" + c2SysNo + "&c3=" + product.C3SysNo + "&pid=" + product.ProductSysNo;
                    strb.Append(String.Format(emptyInventoryItemHTML, deeplink, thumbImg, product.ProductBriefName, deeplink, product.ProductBriefName, product.Price, product.BaiscPrice));
                }
                c1EmptyInventoryProductsHTML = strb.ToString();
            }
            return c1EmptyInventoryProductsHTML;
        }

        /// <summary>
        /// 获得大类的最新降价模块的商品
        /// </summary>
        /// <param name="categoryOneId"></param>
        /// <returns></returns>
        public static string GetC1LastedDisCountProductsHTML(int categoryOneId)
        {
            string c1LastedDisCountProductsHTML = String.Empty;
            List<C1LastestDiscountProduct> products = C1LastedDisCountProductService.GetC1LastestDiscountProduct(categoryOneId);
            StringBuilder strb = new StringBuilder("<ul class='s_item'>");
            if (products != null)
            {
                string imageBasePath = YoeJoyConfig.ImgVirtualPathBase;
                string c1LastedDisCountProductItemHTML = @"<li>
                <table cellspacing='0' cellpadding='0'>
                    <tbody>
                        <tr>
                            <td width='60'>
                                <a href='products/product.html?c1={0}&c2={1}&c3={2}&pid={3}'>
                                    <img src='{4}'></a>
                            </td>
                            <td valign='top' width='124'>
                                <a href='products/product.html?c1={5}&c2={6}&c3={7}&pid={8}'>{9}</a>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                ￥<span class='price'>{10}</span>
                            </td>
                            <td align='right'>
                                <span class='jjfd'>降价幅度：{11}%</span>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </li>";
                foreach (C1LastestDiscountProduct product in products)
                {
                    string thumbImg = imageBasePath + product.ImgPath;
                    strb.Append(String.Format(c1LastedDisCountProductItemHTML,
                        categoryOneId, product.C2SysNo, product.C3SysNo, product.ProductSysNo, thumbImg,
                        categoryOneId, product.C2SysNo, product.C3SysNo, product.ProductSysNo,
                        product.ProductPromotionWord, product.Price, product.DiscountRate));
                }
                strb.Append("</ul>");
                c1LastedDisCountProductsHTML = strb.ToString();
            }
            return c1LastedDisCountProductsHTML;
        }

        /// <summary>
        /// 大类页面本周销量排行
        /// </summary>
        /// <param name="categoryOneId"></param>
        /// <returns></returns>
        public static string GetC1WeeklyBestSaledProductsHTML(int categoryOneId)
        {
            string c1WeeklyBestSaledProductsHTML = String.Empty;
            List<C1WeeklyBestSaledProduct> products = C1WeeklyBestSaledProductService.GetC1WeeklyBestSaledProduct(categoryOneId);
            StringBuilder strb = new StringBuilder("<ul class='sellRanking'>");
            if (products != null)
            {
                string imageBasePath = YoeJoyConfig.ImgVirtualPathBase;
                string c1LastedDisCountProductItemHTML = @"<li>
                <em>{0}</em> <a class='productPic' href='{1}'><img src='{2}'></a>
                <div>
                  <a class='name' title='{3}' href='{4}'>{5}</a>
                  <span class='adText'>{6}</span>
                </div>
                <p class='price'><b>¥{7}</b><span>¥{8}</span></p>
              </li>";

                for (int i = 0; i < products.Count; i++)
                {
                    C1WeeklyBestSaledProduct product = products[i];
                    string thumbImg = imageBasePath + product.ImgPath;
                    string deeplink = YoeJoyConfig.SiteBaseURL + "Pages/Product.aspx?c1=" + categoryOneId + "&c2=" + product.C2SysNo + "&c3=" + product.C3SysNo + "&pid=" + product.ProductSysNo;
                    strb.Append(String.Format(c1LastedDisCountProductItemHTML, (i + 1), deeplink, thumbImg, product.ProductBriefName, deeplink, product.ProductBriefName, product.ProductPromotionWord, product.Price, product.BaiscPrice));
                }
                strb.Append("</ul>");
                c1WeeklyBestSaledProductsHTML = strb.ToString();
            }
            return c1WeeklyBestSaledProductsHTML;
        }

        /// <summary>
        /// 二类页面本周销量排行
        /// </summary>
        /// <param name="categoryOneId"></param>
        /// <returns></returns>
        public static string GetC2WeeklyBestSaledProductsHTML(int c1SysNo, int c2SysNo)
        {
            string c1WeeklyBestSaledProductsHTML = String.Empty;
            List<C1WeeklyBestSaledProduct> products = C2WeeklyBestSaledProductService.GetC2WeeklyBestSaledProduct(c2SysNo);
            StringBuilder strb = new StringBuilder("<ul class='sellRanking'>");
            if (products != null)
            {
                string imageBasePath = YoeJoyConfig.ImgVirtualPathBase;
                string c1LastedDisCountProductItemHTML = @"<li>
                <em>{0}</em> <a class='productPic' href='{1}'><img src='{2}'></a>
                <div>
                  <a class='name' title='{3}' href='{4}'>{5}</a>
                  <span class='adText'>{6}</span>
                </div>
                <p class='price'><b>¥{7}</b><span>¥{8}</span></p>
              </li>";

                for (int i = 0; i < products.Count; i++)
                {
                    C1WeeklyBestSaledProduct product = products[i];
                    string thumbImg = imageBasePath + product.ImgPath;
                    string deeplink = YoeJoyConfig.SiteBaseURL + "Pages/Product.aspx?c1=" + c1SysNo + "&c2=" + c2SysNo + "&c3=" + product.C3SysNo + "&pid=" + product.ProductSysNo;
                    strb.Append(String.Format(c1LastedDisCountProductItemHTML, (i + 1), deeplink, thumbImg, product.ProductBriefName, deeplink, product.ProductBriefName, product.ProductPromotionWord, product.Price, product.BaiscPrice));
                }
                strb.Append("</ul>");
                c1WeeklyBestSaledProductsHTML = strb.ToString();
            }
            return c1WeeklyBestSaledProductsHTML;
        }

        /// <summary>
        /// 获得商品列表的底部导航栏
        /// </summary>
        /// <param name="c3SysNo"></param>
        /// <param name="attribution2Ids"></param>
        /// <returns></returns>
        public static string InitC3ProductListFooter(int c3SysNo, string attribution2Ids)
        {
            string productListFooterHTML = String.Empty;

            int productTotalCount = C3ProductListSerivice.GetPagedProductListItemTotalCount(c3SysNo, attribution2Ids);
            StringBuilder strb = new StringBuilder("<div id='turnPage'>");
            if (productTotalCount > 0)
            {
                int pagedCount = int.Parse(YoeJoyConfig.ProductListPagedCount);
                int totalPageCount = (productTotalCount <= pagedCount) ? 1 : ((productTotalCount / pagedCount) + (productTotalCount % pagedCount));

                string bottomNavHTMLTemplate = @"<a id='prev10' class='prev10' href='javascript:void(0)'/></a>
                        <a id='prev' class='prev' href='javascript:void(0)'></a><em id='pageNumNav' class='pageNum'>{0}</em>
                        <a id='next' class='next' href='javascript:void(0)'></a><a id='next10' class='next10' href='javascript:void(0)'></a>
                        <span>共{1}页&nbsp;&nbsp;到第</span>
                        <input id='txtIndex' class='in' type='text' />
                        <span>页</span>
                        <input id='btnLocate' class='butt' value='确定' type='button' />
                        <input type='hidden' id='totalPageCount' value='{2}'/>
                       <input type='hidden' id='pageSeed' value='{3}'/>";

                string bottomNavHTML = String.Empty;
                string bottomNavItemHTMLTemplate = @"<a href='javascript:void(0)'>{0}</a>";

                if (totalPageCount > 1)
                {
                    string bottomNavItenHTML = String.Empty;
                    for (int i = 1; i <= totalPageCount; i++)
                    {
                        bottomNavItenHTML += String.Format(bottomNavItemHTMLTemplate, i);
                    }
                    bottomNavHTML = String.Format(bottomNavHTMLTemplate, bottomNavItenHTML, totalPageCount, totalPageCount, pagedCount);
                }
                else
                {
                    bottomNavHTML = String.Format(@"<input type='hidden' id='totalProductCount' value='{0}'/>", productTotalCount);
                }
                strb.Append(bottomNavHTML);
            }

            strb.Append("</div>");

            productListFooterHTML = strb.ToString();

            return productListFooterHTML;
        }

        /// <summary>
        /// 获得商品列表的筛选项目
        /// </summary>
        /// <param name="c3SysNo"></param>
        /// <returns></returns>
        public static string InitC3ProductFilter(int c3SysNo)
        {
            string C3ProductFilterHTML = String.Empty;

            List<C3ProductAttribution> c3Attrs = C3ProductListSerivice.GetC3ProductAttribution(c3SysNo);
            if (c3Attrs != null)
            {
                StringBuilder strb = new StringBuilder();

                string filterItemHTMLTemplate = @"<div class='attr'><input class='selectedValue' value='0' type='hidden'> <em class='attrName'>{0}：<input type='hidden' value='{1}'/></em>
                    <a class='all selected' href='javascript:'>全部<input type='hidden' value='0'/></a>
<strong>{2}</strong> </div>".Trim();

                foreach (C3ProductAttribution attr in c3Attrs)
                {
                    if (attr.Options != null)
                    {
                        StringBuilder strb2 = new StringBuilder();
                        string filterOptionHTMLTemplate = @"<span><a href='javascript:'>{0}</a><input type='hidden' value='{1}'/></span>";
                        foreach (C3ProductAttributionOption option in attr.Options)
                        {
                            strb2.Append(String.Format(filterOptionHTMLTemplate, option.OptionName, option.OptionSysNo));
                        }
                        strb.Append(String.Format(filterItemHTMLTemplate, attr.A2Name, attr.A2SysNo, strb2.ToString()));
                    }
                    else
                    {
                        strb.Append(String.Format(filterItemHTMLTemplate, attr.A2Name, attr.A2SysNo, String.Empty));
                    }
                }

                C3ProductFilterHTML = strb.ToString();
            }
            return C3ProductFilterHTML;
        }

        /// <summary>
        /// 获得商品列表
        /// </summary>
        /// <param name="orderOption"></param>
        /// <param name="startIndex"></param>
        /// <param name="pagedCount"></param>
        /// <param name="c3SysNo"></param>
        /// <param name="c1SysNo"></param>
        /// <param name="c2SysNo"></param>
        /// <returns></returns>
        public static string GetC3PageProductListHTML(YoeJoyEnum.ProductListSortedOrder orderOption, int startIndex, int c3SysNo, int c1SysNo, int c2SysNo, string attribution2Ids, string order)
        {
            string productListHTML = String.Empty;
            StringBuilder strb = new StringBuilder("<ul>");

            string baseURL = YoeJoyConfig.SiteBaseURL;
            int pagedCount = int.Parse(YoeJoyConfig.ProductListPagedCount);

            List<FrontDsiplayProduct> products = C3ProductListSerivice.GetPagedProductList((startIndex - 1), pagedCount, c3SysNo, orderOption, attribution2Ids, order);
            if (products != null)
            {
                string imageBaseURL = YoeJoyConfig.ImgVirtualPathBase;
                string productListItemHTMLTemplate1 = @"<li class='show1'>
                    <div class='group'>
                <div class='photo'><a href='{0}' target='_parent'><img class='photo' alt='{1}' src='{2}' width='190' height='190'></a></div>
                <a class='name' title='{3}' href='{4}' target='_parent'>{5}</a>
                <span class='adText'>{6}</span>
                <div class='mem0'>
                  <p class='price'><b>¥{7}</b><span>¥{8}</span></p>
                  <p align='right'>评论:1000条</p>
                </div>
                <div class='botton'><a class='ck' href='{9}' target='_parent'>查看详情</a></div>
              </div>
                </li>";

                string productListItemHTMLTemplate2 = @"<li class='show2'>
                    <div class='group'>
                <div class='photo'><a href='{0}' target='_parent'><img class='photo' alt='{1}' src='{2}' width='190' height='190'></a></div>
                <a class='name' title='{3}' href='{4}' target='_parent'>{5}</a>
                <span class='adText'>{6}</span>
                <div class='mem0'>
                  <p class='price'><b>¥{7}</b><span>¥{8}</span></p>
                  <p align='right'>评论:1000条</p>
                </div>
                <div class='botton'>
                  <p><a class='sub' href='javascript:void(0)'>-</a>
                    <input class='num' maxLength='3' value='1' type='text'>
                    <a class='add' href='javascript:void(0)'>+</a></p>
                  <a class='ck' href='process1.html'>直接购买</a></div>
              </div>
                </li>";
                foreach (FrontDsiplayProduct product in products)
                {
                    string imgPath = YoeJoyConfig.ImgVirtualPathBase + product.ImgPath;
                    string deeplink = YoeJoyConfig.SiteBaseURL + "pages/product.aspx?c1=" + c1SysNo + "&c2=" + c2SysNo + "&c3=" + c3SysNo + "&pid=" + product.ProductSysNo;
                    if (product.IsCanPurchase)
                    {
                        strb.Append(String.Format(productListItemHTMLTemplate2, deeplink, product.ProductBriefName, imgPath, product.ProductBriefName, deeplink, product.ProductBriefName, product.ProductPromotionWord, product.Price, product.BaiscPrice));
                    }
                    else
                    {
                        strb.Append(String.Format(productListItemHTMLTemplate1, deeplink, product.ProductBriefName, imgPath, product.ProductBriefName, deeplink, product.ProductBriefName, product.ProductPromotionWord, product.Price, product.BaiscPrice, deeplink));
                    }
                }
            }
            strb.Append("</ul>");

            productListHTML = strb.ToString();

            return productListHTML;
        }

        /// <summary>
        /// 获得首页促销商品的HTML
        /// </summary>
        /// <returns></returns>
        public static string GetHomePromotionProductsHTML()
        {
            string HomeInComingProductHTML = String.Empty;
            List<FrontDsiplayProduct> products = HomePromotionProductService.GetHomePromotionProducts();
            if (products != null)
            {
                string imageVitualPath = YoeJoyConfig.ImgVirtualPathBase;
                StringBuilder strb = new StringBuilder("<ul class='product products'>");
                foreach (FrontDsiplayProduct product in products)
                {
                    string productLink = String.Format("/Pages/Product.aspx?c1={0}&c2={1}&c3={2}&pid={3}", product.C1SysNo, product.C2SysNo, product.C3SysNo, product.ProductSysNo);
                    string innerHTML = @"<li>
                                        <h3>
                                            <a href='{0}'>
                                                <img alt='产品图片' src='{1}' width='140' height='140'/></a></h3>
                                        <p>
                                            <a href='{2}' title='{3}' class='name'>{4}</a>
                                        </p>
                                        <p> <span class='adText'>
                                         {5}</span> </p>
                                        <p class='price'><b>¥{6}</b><span>¥{7}</span></p>
                                </li>";
                    string imgURL = String.Concat(imageVitualPath, product.ImgPath);
                    strb.Append(String.Format(innerHTML, productLink, imgURL, productLink, product.ProductBriefName, product.ProductBriefName, product.ProductPromotionWord, product.Price, product.BaiscPrice));
                }
                strb.Append("</ul>");
                HomeInComingProductHTML = strb.ToString();
            }
            return HomeInComingProductHTML;
        }

        /// <summary>
        /// 获得浏览过该商品的用户还看过的商品
        /// </summary>
        /// <returns></returns>
        public static string GetProductAlsoSeenHTML(int c1SysNo, int c2SysNo, int c3SysNo, int productSysno)
        {
            string alsoSeenProductHTML = String.Empty;

            List<FrontDsiplayProduct> products = ProductMappingService.GetRelatedProductFromC3(c3SysNo, productSysno);
            if (products != null)
            {
                StringBuilder strb = new StringBuilder("<ul class='list'>");

                string liHTML = @"<li><a href='{0}'><img class='photo' alt='{1}' src='{2}' width='140' height='140'></a>
              <a class='name' title='{3}' href='{4}'>{5}</a>
              <span class='adText'>{6}</span>
              <p class='price'><b>¥{7}</b><span>¥{8}</span></p>
            </li>";

                foreach (FrontDsiplayProduct product in products)
                {
                    string imgPath = YoeJoyConfig.ImgVirtualPathBase + product.ImgPath;
                    string deeplink = YoeJoyConfig.SiteBaseURL + "pages/product.aspx?c1=" + c1SysNo + "&c2=" + c2SysNo + "&c3=" + c3SysNo + "&pid=" + product.ProductSysNo;
                    strb.Append(String.Format(liHTML, deeplink, product.ProductBriefName, imgPath, product.ProductBriefName, deeplink, product.ProductBriefName, product.ProductPromotionWord, product.Price, product.BaiscPrice));
                }
                strb.Append("</ul>");
                alsoSeenProductHTML = strb.ToString();
            }
            return alsoSeenProductHTML;
        }

        /// <summary>
        ///获得猜你喜欢的商品
        /// </summary>
        /// <param name="c1SysNo"></param>
        /// <param name="c2SysNo"></param>
        /// <param name="c3SysNo"></param>
        /// <param name="productSysno"></param>
        /// <returns></returns>
        public static string GetProductGuessYouLikeHTML(int c1SysNo, int c2SysNo, int c3SysNo, int productSysno)
        {
            string guessYouLikeProductHTML = String.Empty;

            List<FrontDsiplayProduct> products = ProductMappingService.GetRelatedProductFromC2(c2SysNo, c3SysNo);
            if (products != null)
            {
                StringBuilder strb = new StringBuilder("<ul class='group'>");

                string liHTML = @"<li><a class='photo' href='{0}'><img alt='{1}' src='{2}' width='60' height='60'></a>
              <div>
                <a class='name' title='{3}' href='{4}'>{5}</a>
                <span class='adText'>{6}</span>
              </div>
              <p class='price'><b>¥{7}</b><span>¥{8}</span></p>
            </li>";

                foreach (FrontDsiplayProduct product in products)
                {
                    string imgPath = YoeJoyConfig.ImgVirtualPathBase + product.ImgPath;
                    string deeplink = YoeJoyConfig.SiteBaseURL + "pages/product.aspx?c1=" + c1SysNo + "&c2=" + product.C2SysNo + "&c3=" + product.C3SysNo + "&pid=" + product.ProductSysNo;
                    strb.Append(String.Format(liHTML, deeplink, product.ProductBriefName, imgPath, product.ProductBriefName, deeplink, product.ProductBriefName, product.ProductPromotionWord, product.Price, product.BaiscPrice));
                }
                strb.Append("</ul>");
                guessYouLikeProductHTML = strb.ToString();
            }
            return guessYouLikeProductHTML;
        }

        /// <summary>
        /// 获得购物车中
        /// 购买了此商品的用户还购买了的商品
        /// </summary>
        /// <param name="c1SysNo"></param>
        /// <param name="c2SysNo"></param>
        /// <param name="c3SysNo"></param>
        /// <param name="productSysno"></param>
        /// <returns></returns>
        public static string GetProductAlsoBuyInCartCheck(int c1SysNo, int c2SysNo, int c3SysNo, int productSysno)
        {
            string alsoBuyProductHTML = String.Empty;

            List<FrontDsiplayProduct> products = ProductMappingService.GetRelatedProductFromC3(c3SysNo, productSysno, 6);
            if (products != null)
            {
                StringBuilder strb = new StringBuilder("<ul>");

                string liHTML = @"<li><a href='{0}'><img alt='{1}' src='{2}' width='118' height='118'></a>
                  <a class='name' title='{3}' href='{4}'>{5}</a>
                  <span class='adText'>{6}</span>
                  <p class='price'><b>¥{7}</b><span>¥{8}</span></p>
                </li>";

                foreach (FrontDsiplayProduct product in products)
                {
                    string imgPath = YoeJoyConfig.ImgVirtualPathBase + product.ImgPath;
                    string deeplink = YoeJoyConfig.SiteBaseURL + "pages/product.aspx?c1=" + c1SysNo + "&c2=" + product.C2SysNo + "&c3=" + product.C3SysNo + "&pid=" + product.ProductSysNo;
                    strb.Append(String.Format(liHTML, deeplink, product.ProductBriefName, imgPath, product.ProductBriefName, deeplink, product.ProductBriefName, product.ProductPromotionWord, product.Price, product.BaiscPrice));
                }
                strb.Append("</ul>");
                alsoBuyProductHTML = strb.ToString();
            }
            return alsoBuyProductHTML;
        }

        /// <summary>
        /// 三类商品页面热卖推荐
        /// </summary>
        /// <param name="c1SysNo"></param>
        /// <param name="c2SysNo"></param>
        /// <param name="c3SysNo"></param>
        /// <returns></returns>
        public static string GetC3BestSaledProductHTML(int c1SysNo, int c2SysNo, int c3SysNo)
        {
            string c3BestSaledProductHTML = String.Empty;

            List<C1WeeklyBestSaledProduct> products = C3BestSaledProductService.GetC3BestSaledProduct(c3SysNo);
            StringBuilder strb = new StringBuilder("<ul class='item'>");
            if (products != null)
            {
                string emptyInventoryItemHTML = @"<li>
              <h5 align='center'><a href='{0}'><img alt='{1}' src='{2}' width='100' height='100'></a></h5>
              <p><a class='name' title='{3}' href='{4}'>{5}</a>
              <span class='adText'>{6}</span></p>
              <p class='price'><b>¥{7}</b><span>¥{8}</span><a class='bt1' href='javascript:void(0);'>立即购买</a></p>
            </li>";

                foreach (C1WeeklyBestSaledProduct product in products)
                {
                    string thumbImg = YoeJoyConfig.ImgVirtualPathBase + product.ImgPath;
                    string deeplink = YoeJoyConfig.SiteBaseURL + "Pages/Product.aspx?c1=" + c1SysNo + "&c2=" + c2SysNo + "&c3=" + c3SysNo + "&pid=" + product.ProductSysNo;
                    strb.Append(String.Format(emptyInventoryItemHTML,deeplink,product.ProductBriefName, thumbImg, product.ProductBriefName, deeplink, product.ProductBriefName, product.ProductPromotionWord, product.Price, product.BaiscPrice));
                }
                strb.Append("</ul>");
                c3BestSaledProductHTML = strb.ToString();
            }
            return c3BestSaledProductHTML;
        }

        /// <summary>
        /// 三类商品产品热评
        /// </summary>
        /// <param name="c1SysNo"></param>
        /// <param name="c2SysNo"></param>
        /// <param name="c3SysNo"></param>
        /// <returns></returns>
        public static string GetC3HotCommentedProductHTML(int c1SysNo, int c2SysNo, int c3SysNo)
        {
            string c3HotCommentedProductHTML = String.Empty;

            List<C1WeeklyBestSaledProduct> products = C3HotCommentProductService.GetC3HotCommentedProduct(c3SysNo);
            StringBuilder strb = new StringBuilder("<div class='group'>");
            if (products != null)
            {
                string emptyInventoryItemHTML = @"<div class='item'>
                        <a class='photo' href='{0}'>
                            <img alt='{1}' src='{2}' width='60' height='60'></a>
                        <div>
                            <a class='name' title='{3}' href='{4}'>{5}</a>
                            <span class='adText'>{6}</span>
                        </div>
                        <p class='price'>
                            <b>¥{7}</b><span>¥{8}</span></p>
                        <p class='pltext' align='left'>
                            评论内容评论内容评论内容评论内容评论内容评论内容</p>
                        <p class='slave' align='right'>
                            会员:l***o</p>
                    </div>";

                foreach (C1WeeklyBestSaledProduct product in products)
                {
                    string thumbImg = YoeJoyConfig.ImgVirtualPathBase + product.ImgPath;
                    string deeplink = YoeJoyConfig.SiteBaseURL + "Pages/Product.aspx?c1=" + c1SysNo + "&c2=" + c2SysNo + "&c3=" + c3SysNo + "&pid=" + product.ProductSysNo;
                    strb.Append(String.Format(emptyInventoryItemHTML, deeplink, product.ProductBriefName, thumbImg, product.ProductBriefName, deeplink, product.ProductBriefName, product.ProductPromotionWord, product.Price, product.BaiscPrice));
                }
                strb.Append("</div>");
                c3HotCommentedProductHTML = strb.ToString();
            }
            return c3HotCommentedProductHTML;
        }

        /// <summary>
        /// 首页用户热评商品
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public static string GetHomeHotCommentedProductHTML(int startIndex, int endIndex)
        {
            string homeHotCommentedProductHTML = String.Empty;
            List<FrontDsiplayProduct> products = HomeHotCommentedProductService.GetHomeHotCommentedProducts(startIndex, endIndex);
            StringBuilder strb = new StringBuilder("<dl class='discsPhone'>");
            if (products != null)
            {
                string emptyInventoryItemHTML = @"<dt>
                                <h2>
                                    <a href='{0}'>
                                        <img alt='{1}' src='{2}' width='130' height='130'/></a>
                                </h2>
                                <p>
                                    <a class='name' title='{3}' href='{4}'>{5}</a> <i class='adText'>
                                        {6}</i> <span><b>332</b>条</span>
                                </p>
                            </dt>
                            <dd>
                                <p>
                                    <img src='../static/images/alert.png'/>评论内容评论内容评论内容评论内容评论内容评论内容</p>
                                <span><em>会员:</em>l***o</span>
                            </dd>";

                foreach (FrontDsiplayProduct product in products)
                {
                    string thumbImg = YoeJoyConfig.ImgVirtualPathBase + product.ImgPath;
                    string deeplink = YoeJoyConfig.SiteBaseURL + "Pages/Product.aspx?c1=" + product.C1SysNo + "&c2=" + product.C2SysNo + "&c3=" + product.C3SysNo + "&pid=" + product.ProductSysNo;
                    strb.Append(String.Format(emptyInventoryItemHTML, deeplink, product.ProductBriefName, thumbImg, product.ProductBriefName, deeplink, product.ProductBriefName, product.ProductPromotionWord, product.Price, product.BaiscPrice));
                }
                strb.Append("</dl>");
                homeHotCommentedProductHTML = strb.ToString();
            }

            return homeHotCommentedProductHTML;
        }

        /// <summary>
        /// 首页销量排行
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public static string GetHomeBestSaledProductHTML(int startIndex, int endIndex)
        {
            string homeHotCommentedProductHTML = String.Empty;
            List<FrontDsiplayProduct> products = HomeBestSaledProductService.GetHomeBestSaledProducts(startIndex, endIndex);
            StringBuilder strb = new StringBuilder("<dl class='discusSell'>");
            if (products != null)
            {
                string emptyInventoryItemHTML = @"<dd>
                                <em>{0}</em><img src='{1}' width='57' height='57'>
                                <div>
                                    <a class='name' href='{2}'>{3}</a> <i class='adText'>{4}</i>
                                </div>
                                <p class='price'>
                                    <b>¥{5}</b><span>¥{6}</span></p>
                            </dd>";

                for (int i = 0; i < products.Count; i++)
                {
                    var product = products[i];
                    string thumbImg = YoeJoyConfig.ImgVirtualPathBase + product.ImgPath;
                    string deeplink = YoeJoyConfig.SiteBaseURL + "Pages/Product.aspx?c1=" + product.C1SysNo + "&c2=" + product.C2SysNo + "&c3=" + product.C3SysNo + "&pid=" + product.ProductSysNo;
                    strb.Append(String.Format(emptyInventoryItemHTML, (i + 1), thumbImg, deeplink, product.ProductBriefName, product.ProductPromotionWord, product.Price, product.BaiscPrice));
                }

                strb.Append("</dl>");
                homeHotCommentedProductHTML = strb.ToString();
            }

            return homeHotCommentedProductHTML;
        }

        /// <summary>
        /// 获得首页中间品牌推荐的商品
        /// </summary>
        public static string GetHomePromotionBrandsProductsHTML()
        {
            string homePromotionBrandsProductsHTML = String.Empty;

            List<HomePromotionBrandsC3Info> c3Info = HomePromotionBrandsService.GetHomePromptionBrandsC3Info();
            if (c3Info != null)
            {
                StringBuilder strb = new StringBuilder();
                string siteBaseURL = YoeJoyConfig.SiteBaseURL;
                string imgBasePath = YoeJoyConfig.ImgVirtualPathBase;
                foreach (HomePromotionBrandsC3Info c3 in c3Info)
                {
                    strb.Append("<div class='item'>");
                    string c3Deeplnik = String.Concat(siteBaseURL, "Pages/SubProductList3.aspx?c1=", c3.C1SysNo, "&c2=", c3.C2SysNo, "&c3=", c3.C3SysNo);
                    strb.Append(String.Concat("<div class='slave0'><a href='", c3Deeplnik, "'>", c3.C3Name, "</a></div>"));
                    strb.Append("<div class='mem0'><img class='prev' src='../static/images/hg2prev.png' data-src='../static/images/hg2prev.png'></div>");
                    strb.Append("<div class='mem1'>");
                    List<FrontDsiplayProduct> products = HomePromotionBrandsService.GetHomePromotionBrandsProducts(c3.C3SysNo);
                    if (products != null)
                    {
                        strb.Append("<div class='scrollw'>");
                        string productHTMLTemplate = @"<div class='photo'>
            <a href='{0}'><img src='{1}' ></a></div>
            <div class='info'>
              <p class='nameItem'>
                <a class='name' title='{2}' href='{3}'>{4}</a>
                <span class='adText'>{5}</span>
              </p>
              <p class='price'><b>¥{6}</b><span>¥{7}</span></p></div>";

                        foreach (FrontDsiplayProduct product in products)
                        {
                            string productDeepLink = String.Concat(siteBaseURL, "Pages/Product.aspx?c1=", c3.C1SysNo, "&c2=", c3.C2SysNo, "&c3=", c3.C3SysNo, "&pid=", product.ProductSysNo);
                            string imagePath = String.Concat(imgBasePath, product.ImgPath);
                            strb.Append("<div class='scroll'>");

                            strb.Append(String.Format(productHTMLTemplate, productDeepLink, imagePath, product.ProductBriefName, productDeepLink,
                                product.ProductBriefName, product.ProductPromotionWord, product.Price, product.BaiscPrice));

                            strb.Append("</div>");
                        }
                        strb.Append("</div>");
                    }
                    strb.Append("</div>");
                    strb.Append("<div class='mem0'><img class='next' src='../static/images/hg2next.png' data-src='../static/images/hg2next.png'></div>");
                    strb.Append("</div>");
                }
                homePromotionBrandsProductsHTML = strb.ToString();
            }
            return homePromotionBrandsProductsHTML;
        }

        /// <summary>
        /// 搜索页 热评商品
        /// </summary>
        /// <returns></returns>
        public static string GetSearchHotCommentedProductsHTML()
        {
            string searchHotCommentedProductsHTML = String.Empty;
            List<FrontDsiplayProduct> products = SearchHotCommentedProductService.GetSearchHotCommentedProduct();
            StringBuilder strb = new StringBuilder("<div class='group'>");
            if (products != null)
            {
                string imageBasePath = YoeJoyConfig.ImgVirtualPathBase;
                string c1LastedDisCountProductItemHTML = @"<div class='item'>
                        <a class='photo' href='{0}l'>
                            <img alt='{1}' src='{2}' width='60' height='60'></a>
                        <div>
                            <a class='name' title='{3}' href='{4}'>{5}</a>
                            <span class='adText'>{6}</span>
                        </div>
                        <p class='price'>
                            <b>¥{7}</b><span>¥{8}</span></p>
                        <p class='pltext' align='left'>
                            评论内容评论内容评论内容评论内容评论内容评论内容</p>
                        <p class='slave' align='right'>
                            会员:l***o</p>
                    </div>";

                for (int i = 0; i < products.Count; i++)
                {
                    FrontDsiplayProduct product = products[i];
                    string thumbImg = imageBasePath + product.ImgPath;
                    string deeplink = YoeJoyConfig.SiteBaseURL + "Pages/Product.aspx?c1=" + product.C1SysNo + "&c2=" + product.C2SysNo + "&c3=" + product.C3SysNo + "&pid=" + product.ProductSysNo;
                    strb.Append(String.Format(c1LastedDisCountProductItemHTML, deeplink, product.ProductBriefName, thumbImg, product.ProductBriefName, deeplink, product.ProductBriefName, product.ProductPromotionWord, product.Price, product.BaiscPrice));
                }
                strb.Append("</div>");
                searchHotCommentedProductsHTML = strb.ToString();
            }
            return searchHotCommentedProductsHTML;
        }
    }
}
