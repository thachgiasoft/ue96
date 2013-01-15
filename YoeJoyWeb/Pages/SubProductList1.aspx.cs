using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YoeJoyHelper;

namespace YoeJoyWeb
{
    public partial class SubProductList1 : System.Web.UI.Page
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

        /// <summary>
        /// 大类商品
        /// </summary>
        protected string C1ProductsDisplayHTML { get; set; }
        /// <summary>
        /// 清库产品
        /// </summary>
        protected string C1EmptyInventoryProductsHTML { get; set; }
        /// <summary>
        /// 本周销量排行
        /// </summary>
        protected string C1WeeklyBestSaledProductsHTML { get; set; }

        /// <summary>
        /// 大类幻灯片广告
        /// </summary>
        protected string C1SlideAdHTML { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            ((Site)this.Master).IsHomePage = false;
            if (!IsPostBack)
            {
                SubCategoryNavigation1.C1SysNo = C1SysNo;
                C1WeeklyBestSaledProductsHTML = FrontProductsHelper.GetC1WeeklyBestSaledProductsHTMLWrapper(C1SysNo);
                C1EmptyInventoryProductsHTML = FrontProductsHelper.GetC1EmptyInventoryProductsHTML(C1SysNo);
                C1ProductsDisplayHTML = FrontProductsHelper.GetC1ProductsDisplayHTMLWrapper(C1SysNo);
                C1SlideAdHTML = ADHelper.GetSlideAdWrapper(C1SysNo);
            }
        }
    }
}