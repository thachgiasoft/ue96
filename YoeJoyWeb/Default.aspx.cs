using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YoeJoyHelper;

namespace YoeJoyWeb
{
    public partial class Default : System.Web.UI.Page
    {
        /// <summary>
        /// 商品促销
        /// </summary>
        protected string PromoHTML { get; set; }
        /// <summary>
        /// 限时抢购
        /// </summary>
        protected string PanicBuyingHTML { get; set; }
        /// <summary>
        /// 新品上市
        /// </summary>
        protected string InComingProducts { get; set; }
        /// <summary>
        /// 主页中间广告(幻灯片)
        /// </summary>
        protected string HomeBrandsHTML { get; set; }
        /// <summary>
        /// 公告
        /// </summary>
        protected string HomeWebBulletinListHTML { get; set; }
        /// <summary>
        /// 菜单下面的广告
        /// </summary>
        protected string ADBelowHomeNavHTML { get; set; }
        /// <summary>
        /// 公告栏下面的广告
        /// </summary>
        protected string ADBelowNewsHTML { get; set; }
        /// <summary>
        /// 主页中间靠左的广告
        /// </summary>
        protected string ADCenterLeft { get; set; }
        /// <summary>
        /// 主页中间靠右的广告
        /// </summary>
        protected string ADCenterRight { get; set; }
        /// <summary>
        /// 主页大类1的左侧图片广告
        /// </summary>
        protected string ADCategoryLeftOne { get; set; }
        /// <summary>
        /// 主页大类2的左侧图片广告
        /// </summary>
        protected string ADCategoryLeftTwo { get; set; }
        /// <summary>
        /// 主页大类1
        /// </summary>
        protected string CategoryProductsOneHTML { get; set; }
        /// <summary>
        /// 主页大类2
        /// </summary>
        protected string CategoryProductsTwoHTML { get; set; }
        /// <summary>
        /// 大类1品牌
        /// </summary>
        protected string CategoryProductsBrandsOneHTML { get; set; }
        /// <summary>
        /// 大类2品牌
        /// </summary>
        protected string CategoryProductsBrandsTwoHTML { get; set; }
        /// <summary>
        /// 大类1 用户热评
        /// </summary>
        protected string CategoryProductsHotCommentedOneHTML { get; set; }
        /// <summary>
        /// 大类2 用户热评
        /// </summary>
        protected string CategoryProductsHotCommentedTwoHTML { get; set; }
        /// <summary>
        /// 大类1 热销商品
        /// </summary>
        protected string CategoryProductBestSaledOneHTML { get; set; }
        /// <summary>
        /// 大类2 热销商品
        /// </summary>
        protected string CategoryProductBestSaledTwoHTML { get; set; }
        /// <summary>
        /// 首页中间的品牌推荐商品
        /// </summary>
        protected string HomePromotionBrandsProductHTML { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            ((Site)this.Master).IsHomePage = true;
            if (!IsPostBack)
            {
                CategoryNavigation1.IsHomePage = true;
                HomeWebBulletinListHTML = WebBulletinHelper.GetWebBulletinListForHome(5);
                HomeBrandsHTML = BrandsHelper.GetBrandsForHomeWrapper(8);
                InComingProducts = InComingProductHelper.GetInComingProductForHomeWrapper();
                CategoryProductsOneHTML = FrontProductsHelper.GetHomeCategoryOneProductsDisplayHTMLWrapper(YoeJoyConfig.HomeDisplayCategoryID1);
                CategoryProductsTwoHTML = FrontProductsHelper.GetHomeCategoryOneProductsDisplayHTMLWrapper(YoeJoyConfig.HomeDisplayCategoryID2);
                CategoryProductsBrandsOneHTML = BrandsHelper.GetBrandsForCategoryOneProductsWrapper(YoeJoyConfig.HomeDisplayCategoryID1);
                CategoryProductsBrandsTwoHTML = BrandsHelper.GetBrandsForCategoryOneProductsWrapper(YoeJoyConfig.HomeDisplayCategoryID2);
                PromoHTML = FrontProductsHelper.GetHomePromotionProductsHTML();
                PanicBuyingHTML = PanicBuyingHelper.GetPanicProductsForHomeWrapper();
                CategoryProductsHotCommentedOneHTML = FrontProductsHelper.GetHomeHotCommentedProductHTMLWrapper(int.Parse(YoeJoyConfig.HomeDisplayCategoryID1),1, 2);
                CategoryProductsHotCommentedTwoHTML = FrontProductsHelper.GetHomeHotCommentedProductHTMLWrapper(int.Parse(YoeJoyConfig.HomeDisplayCategoryID2), 3 ,4);
                CategoryProductBestSaledOneHTML = FrontProductsHelper.GetHomeBestSaledProductHTMLWrapper(int.Parse(YoeJoyConfig.HomeDisplayCategoryID1),1, 3);
                CategoryProductBestSaledTwoHTML = FrontProductsHelper.GetHomeBestSaledProductHTMLWrapper(int.Parse(YoeJoyConfig.HomeDisplayCategoryID2),4, 6);
                HomePromotionBrandsProductHTML = FrontProductsHelper.GetHomePromotionBrandsProductsWrapper();
            }
        }
    }
}