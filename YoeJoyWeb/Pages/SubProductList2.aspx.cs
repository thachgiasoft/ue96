using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YoeJoyHelper;

namespace YoeJoyWeb
{
    public partial class SubProductList2 : System.Web.UI.Page
    {

        protected int C1SysNo
        {
            get
            {
                if (Request.QueryString["c1"] == null)
                {
                    return 0;
                }
                else
                {
                    return int.Parse(Request.QueryString["c1"].ToString().Trim());
                }
            }
        }

        protected int C2SysNo
        {
            get
            {
                if (Request.QueryString["c2"] == null)
                {
                    return 0;
                }
                else
                {
                    return int.Parse(Request.QueryString["c2"].ToString().Trim());
                }
            }
        }

        /// <summary>
        /// 大类商品
        /// </summary>
        protected string C2ProductsDisplayHTML { get; set; }
        /// <summary>
        /// 清库产品
        /// </summary>
        protected string C2EmptyInventoryProductsHTML { get; set; }
        /// <summary>
        /// 本周销量排行
        /// </summary>
        protected string C2WeeklyBestSaledProductsHTML { get; set; }

        /// <summary>
        /// 二类幻灯片广告
        /// </summary>
        protected string C2SlideAdHTML { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            ((Site)this.Master).IsHomePage = false;
            if (!IsPostBack)
            {
                SubCategoryNavigation1.C1SysNo = C1SysNo;
                C2WeeklyBestSaledProductsHTML = FrontProductsHelper.GetC2WeeklyBestSaledProductsHTMLWrapper(C1SysNo,C2SysNo);
                C2EmptyInventoryProductsHTML = FrontProductsHelper.GetC2EmptyInventoryProductsHTMLWrapper(C1SysNo,C2SysNo);
                C2ProductsDisplayHTML = FrontProductsHelper.GetC2ProductsDisplayHTMLWrapper(C1SysNo, C2SysNo);
                C2SlideAdHTML = ADHelper.GetSlideAdWrapper(C2SysNo);
            }
        }
    }
}