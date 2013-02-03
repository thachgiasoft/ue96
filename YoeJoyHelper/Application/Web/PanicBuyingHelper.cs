using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YoeJoyHelper.Model;
using System.Configuration;

using Icson.Utils;
using Icson.Objects.Sale;
using Icson.Objects.Basic;

using Icson.BLL;
using Icson.BLL.Basic;
using Icson.BLL.Sale;
using Icson.Objects;

namespace YoeJoyHelper
{
    /// <summary>
    /// 限时抢购的Helper类
    /// </summary>
    public class PanicBuyingHelper
    {
        public static string GetPanicProductsForHomeWrapper()
        {
            CacheObjSetting cacheSetting = StaticCacheObjSettings.SiteHomePanicProductListCacheSetting;
            string key = cacheSetting.CacheKey;
            int duration = cacheSetting.CacheDuration;
            string HomeInComingProductHTML = CacheObj<string>.GetCachedObj(key, duration, GetPanicProductsForHome());
            return HomeInComingProductHTML;
        }

        /// <summary>
        /// 主页限时抢购模块
        /// </summary>
        /// <returns></returns>
        public static string GetPanicProductsForHome()
        {
            string HomePanicHTML = String.Empty;
            List<PanicBuyingProductModelForHome> panicProducts = PanicBuyingProductService.GetHomePanicProduct();
            if (panicProducts != null)
            {
              
                string imageVitualPath = YoeJoyConfig.ImgVirtualPathBase;
                StringBuilder strb = new StringBuilder("<ul id='panicBuyContents'>");
                foreach (PanicBuyingProductModelForHome panic in panicProducts)
                {
                    
                    

                    string liTemplate = @"<li>
                    <h4 class='time'>
                        <span>还剩</span>
                        <img alt='钟' src='../static/images/time.png' width='15' height='18'/>
                        <b>23</b> <em>小时</em> <b>55</b> <em>分</em> <b>33</b> <em>秒</em>
                        <input type='hidden' class='buttonEndTime' value='{0}'/>
                    </h4>
                    <a href='{1}'>
                        <img alt='商品图片' src='{2}' width='100' height='100' /></a>
                    <div class='group'>
                        <a class='name' title='{3}' href='{4}'>
                            {5}</a> <span class='adText'>{6}</span>
                        <p class='price'>
                            <b>¥{7}</b><span>¥{8}</span></p>
                        <em>库存：</em><div id='{10}' style='height:8px; width:80px'></div><a class='buying' href='{9}'>
                            立即抢购</a>
                    </div>
                </li>";

                    string progressbar ="progressbar"+ panic.Count;
                    string imgURL = String.Concat(imageVitualPath, panic.CoverImg);
                    string deeplink = YoeJoyConfig.SiteBaseURL + "Pages/Product.aspx?c1=" + panic.C1SysNo + "&c2=" + panic.C2SysNo + "&c3=" + panic.C3SysNo + "&pid=" + panic.ProductSysNo;
                    strb.Append(String.Format(liTemplate, panic.EndTime.ToString("MM/dd/yyyy HH:mm:ss"), deeplink, imgURL, panic.BriefName, deeplink, panic.BriefName, panic.PromotionWord, panic.ProductPrice, panic.BaiscPrice, deeplink, progressbar));
                }
                strb.Append("</ul>");
                HomePanicHTML = strb.ToString();
            }
            return HomePanicHTML;
        }
    }
}
