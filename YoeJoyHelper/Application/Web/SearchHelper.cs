using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YoeJoyHelper.Model;

namespace YoeJoyHelper
{
    public class SearchHelper
    {
        /// <summary>
        /// 初始化Search1的筛选项
        /// </summary>
        /// <param name="keyWords"></param>
        /// <returns></returns>
        public static string InitSearch1C3ProductFilter(string keyWords)
        {
            string serach1C3ProductFilterHTML = String.Empty;

            List<C3ProductSerach1Filter> search1Filters = Search1ProductService.GetSearch1C3Names(keyWords);
            if (search1Filters != null)
            {
                StringBuilder strb = new StringBuilder(@"<div class='attr'> <em>产品类别：
    </em><a class='selected' href='javascript:void(0);'>全部</a><strong>".Trim());

                string filterItemHTMLTemplate = @"
                            <span><a href='javascript:void(0);'>{0}</a><input type='hidden' value='{1}'/><input type='hidden' value='{2}'/><input type='hidden' value='{3}'/></span>".Trim();

                foreach (C3ProductSerach1Filter filter in search1Filters)
                {
                    strb.Append(String.Format(filterItemHTMLTemplate, filter.C3Name, filter.C1SysNo, filter.C2SysNo, filter.C3SysNo));
                }

                strb.Append(@"</strong></div>".Trim());
                serach1C3ProductFilterHTML = strb.ToString();
            }
            return serach1C3ProductFilterHTML;
        }

        /// <summary>
        /// 初始化Serach1商品列表表尾
        /// </summary>
        /// <param name="keyWords"></param>
        /// <returns></returns>
        public static string InitSearch1C3ProductListFooter(string keyWords)
        {
            string productListFooterHTML = String.Empty;

            int productTotalCount = Search1ProductService.GetSearch1C3ProductTotalCount(keyWords);
            StringBuilder strb = new StringBuilder("<div id='turnPage'>");
            if (productTotalCount > 0)
            {
                int pagedCount = int.Parse(YoeJoyConfig.ProductListPagedCount);
                int totalPageCount = (productTotalCount <= pagedCount) ? 1 : ((productTotalCount/pagedCount)+(productTotalCount % pagedCount));

                string bottomNavHTMLTemplate = @"<a id='prev10' class='prev10' href='javascript:void(0)'/></a>
                        <a id='prev' class='prev' href='javascript:void(0)'></a><em id='pageNumNav' class='pageNum'>{0}</em>
                        <a id='next' class='next' href='javascript:void(0)'></a><a id='next10' class='next10' href='javascript:void(0)'></a>
                        <span>共{1}页&nbsp;&nbsp;到第</span>
                        <input id='txtIndex' class='in' type='text' />
                        <span>页</span>
                        <input id='btnLocate' class='butt' value='确定' type='button' />
                        <input type='hidden' id='totalPageCount' value='{2}'/>
                       <input type='hidden' id='pageSeed' value='{3}'/>
                       <input type='hidden' id='totalProductCount' value='{4}'/>";

                string bottomNavHTML = String.Empty;
                string bottomNavItemHTMLTemplate = @"<a href='javascript:void(0)'>{0}</a>";

                if (totalPageCount > 1)
                {
                    string bottomNavItenHTML = String.Empty;
                    for (int i = 1; i <= totalPageCount; i++)
                    {
                        bottomNavItenHTML += String.Format(bottomNavItemHTMLTemplate, i);
                    }
                    bottomNavHTML = String.Format(bottomNavHTMLTemplate, bottomNavItenHTML, totalPageCount, totalPageCount, pagedCount, productTotalCount);
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

        public static string GetSearch1ProductListHTML(YoeJoyEnum.ProductListSortedOrder orderOption, int startIndex, string keyWords,string order)
        {
            string productListHTML = String.Empty;
            StringBuilder strb = new StringBuilder("<ul>");

            string baseURL = YoeJoyConfig.SiteBaseURL;
            int pagedCount = int.Parse(YoeJoyConfig.ProductListPagedCount);

            List<FrontDsiplayProduct> products = Search1ProductService.GetPagedSearch1Products(orderOption,keyWords,order);
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

                startIndex = (startIndex - 1)*pagedCount;
                var pagedProductList = products.Skip(startIndex).Take(pagedCount);

                foreach (FrontDsiplayProduct product in pagedProductList)
                {
                    string imgPath = YoeJoyConfig.ImgVirtualPathBase + product.ImgPath;
                    string deeplink = YoeJoyConfig.SiteBaseURL + "pages/product.aspx?c1=" + product.C1SysNo + "&c2=" + product.C2SysNo + "&c3=" + product.C3SysNo + "&pid=" + product.ProductSysNo;
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
        /// 初始化Serach2商品列表表尾
        /// </summary>
        /// <param name="keyWords"></param>
        /// <returns></returns>
        public static string InitSearch2C3ProductListFooter(int c3SysNo, string attribution2IdStr, string keyWords)
        {
            string productListFooterHTML = String.Empty;

            int productTotalCount = Search2ProductService.GetSearch2C3ProductTotalCount(c3SysNo, attribution2IdStr, keyWords);
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
                       <input type='hidden' id='pageSeed' value='{3}'/>
                       <input type='hidden' id='totalProductCount' value='{4}'/>";

                string bottomNavHTML = String.Empty;
                string bottomNavItemHTMLTemplate = @"<a href='javascript:void(0)'>{0}</a>";

                if (totalPageCount > 1)
                {
                    string bottomNavItenHTML = String.Empty;
                    for (int i = 1; i <= totalPageCount; i++)
                    {
                        bottomNavItenHTML += String.Format(bottomNavItemHTMLTemplate, i);
                    }
                    bottomNavHTML = String.Format(bottomNavHTMLTemplate, bottomNavItenHTML, totalPageCount, totalPageCount, pagedCount, productTotalCount);
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
        /// 获得Search2的分页商品
        /// </summary>
        /// <param name="orderOption"></param>
        /// <param name="startIndex"></param>
        /// <param name="keyWords"></param>
        /// <returns></returns>
        public static string GetSearch2ProductListHTML(YoeJoyEnum.ProductListSortedOrder orderOption, int startIndex, int c3SysNo, int c1SysNo, int c2SysNo, string attribution2Ids, string keyWords,string order)
        {
            string productListHTML = String.Empty;
            StringBuilder strb = new StringBuilder("<ul>");

            string baseURL = YoeJoyConfig.SiteBaseURL;
            int pagedCount = int.Parse(YoeJoyConfig.ProductListPagedCount);

            List<FrontDsiplayProduct> products = Search2ProductService.GetPagedProductList(c3SysNo, orderOption, attribution2Ids, keyWords, order);
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

                startIndex = (startIndex - 1) * pagedCount;
                var pagedProductList = products.Skip(startIndex).Take(pagedCount);

                foreach (FrontDsiplayProduct product in pagedProductList)
                {
                    string imgPath = YoeJoyConfig.ImgVirtualPathBase + product.ImgPath;
                    string deeplink = YoeJoyConfig.SiteBaseURL + "pages/product.aspx?c1=" + product.C1SysNo + "&c2=" + product.C2SysNo + "&c3=" + product.C3SysNo + "&pid=" + product.ProductSysNo;
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
        /// 获取搜索结果中第一个商品的大类ID
        /// 来生成右侧导航
        /// </summary>
        /// <param name="keyWords"></param>
        /// <returns></returns>
        public static int GetSearchResultC1SysNo(string keyWords)
        {
            int c1SysNo = Search1ProductService.GetSearchC1SysNo(keyWords);
            return c1SysNo;
        }
    }
}
