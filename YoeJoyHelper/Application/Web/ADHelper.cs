using System;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Linq;
using System.Text;
using YoeJoyHelper.Model;

namespace YoeJoyHelper
{
    /// <summary>
    /// 广告的helper类
    /// </summary>
    public class ADHelper
    {
        public static string GetSlideAdWrapper(int positionId)
        {
            CacheObjSetting cacheSetting = DynomicCacheObjSettings.SiteADCacheSetting;
            string key = String.Format(cacheSetting.CacheKey, positionId);
            int duration = cacheSetting.CacheDuration;
            string slideAdHTML = CacheObj<string>.GetCachedObj(key, duration, GetSlideAd(positionId));
            return slideAdHTML;
        }

        public static string GetSiteStaticAdWrapper(int positionId, string cssClass, string width, string height)
        {
            CacheObjSetting cacheSetting = DynomicCacheObjSettings.SiteADCacheSetting;
            string key = String.Format(cacheSetting.CacheKey, positionId);
            int duration = cacheSetting.CacheDuration;
            string siteAdHTML = CacheObj<string>.GetCachedObj(key, duration, GetSiteStaticAd(positionId, cssClass, width, height));
            return siteAdHTML;
        }

        public static string GetSiteStaticAd(int positionId, string cssClass, string width, string height)
        {
            string siteAdHTML = String.Empty;
            string imageVitualPath = ConfigurationManager.AppSettings["ImageVitrualPath"].ToString();
            ADModuleForSite ad = ADService.GetHomeAdByPosition(positionId);
            if (ad != null)
            {
                string adImg = String.Concat(imageVitualPath, ad.ADImg);
                if (String.IsNullOrEmpty(width) || String.IsNullOrEmpty(height))
                {
                    siteAdHTML = String.Format("<a class='{0}' href='{1}' target='_blank' ><img src='{2}' alt='{3}' /></a>", cssClass, ad.ADLink, adImg, ad.ADName);
                }
                else
                {
                    siteAdHTML = String.Format("<a class='{0}' href='{1}' target='_blank' ><img width='{2}' height='{3}' src='{4}' alt='{5}' /></a>", cssClass, ad.ADLink, width, height, adImg, ad.ADName);
                }
            }
            return siteAdHTML;
        }

        public static string GetSlideAd(int positionId)
        {
            string slideAdHTML = String.Empty;

            List<ADModuleForSite> ads = ADService.GetSlideAdByPosition(positionId);
            if (ads != null)
            {
                StringBuilder strb = new StringBuilder("<div id='adShow'>");

                string adHTMLTemplate = @"<a href='{0}'>
                    <img alt='{1}' src='{2}' width='780' height='277'></a>";

                for (int j = 0; j < ads.Count; j++)
                {
                    string image = YoeJoyConfig.ImgVirtualPathBase + ads[j].ADImg;
                    if (j == 0)
                    {
                        strb.Append(String.Format( @"<a class='show' href='{0}'><img alt='{1}' src='{2}' width='780' height='277'></a>", ads[j].ADLink, ads[j].ADName, image));
                    }
                    else
                    {
                        strb.Append(String.Format(adHTMLTemplate, ads[j].ADLink, ads[j].ADName, image));
                    }
                }

                strb.Append("<div class='btItem'>");

                if (ads.Count > 0)
                {
                    for (int i = 1; i <= ads.Count; i++)
                    {
                        if (i == 1)
                        {
                            strb.Append(String.Format("<a class='selected' href='javascript:'>{0}</a>", i));
                        }
                        else
                        {
                            strb.Append(String.Format("<a href='javascript:'>{0}</a>", i));
                        }
                    }
                }

                strb.Append("</div>");
                strb.Append("</div>");
                slideAdHTML = strb.ToString();
            }
            return slideAdHTML;
        }

    }
}
