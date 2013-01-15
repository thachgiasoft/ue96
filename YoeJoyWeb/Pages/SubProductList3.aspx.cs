using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YoeJoyHelper;

namespace YoeJoyWeb
{
    public partial class SubProductList3 : System.Web.UI.Page
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

        protected int C3SysNo
        {
            get
            {
                if (Request.QueryString["c3"] == null)
                {
                    return 0;
                }
                else
                {
                    return int.Parse(Request.QueryString["c3"].ToString().Trim());
                }
            }
        }

        protected string C3ProductFilterHTML { get; set; }
        /// <summary>
        /// 热卖推荐
        /// </summary>
        protected string C3BestSaledProductHTML { get; set; }
        /// <summary>
        /// 产品热评
        /// </summary>
        protected string C3HotCommentProductHTML { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            ((Site)this.Master).IsHomePage = false;
            if (!IsPostBack)
            {
                SubCategoryNavigation1.C1SysNo = C1SysNo;
                C3ProductFilterHTML = FrontProductsHelper.InitC3ProductFilterWrapper(C3SysNo);
                C3BestSaledProductHTML = FrontProductsHelper.GetC3BestSaledProductHTMLWrapper(C1SysNo,C2SysNo,C3SysNo);
                C3HotCommentProductHTML = FrontProductsHelper.GetC3HotCommentedProductHTMLWrapper(C1SysNo, C2SysNo, C3SysNo);
            }
        }
    }
}