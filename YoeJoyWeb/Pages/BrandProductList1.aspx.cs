using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YoeJoyHelper;
using YoeJoyHelper.Extension;

namespace YoeJoyWeb.Pages
{
    public partial class BrandProductList1 : System.Web.UI.Page
    {

        protected string BrandName
        {
            get
            {
                if (Request.QueryString["bName"] == null)
                {
                    return String.Empty;
                }
                else
                {
                    return Request.QueryString["bName"].ToString().GetUrlDecodeStr();
                }
            }
        }

        protected int BrandID
        {
            get
            {
                if (Request.QueryString["bid"] == null)
                {
                    return 0;
                }
                else
                {
                    return int.Parse(Request.QueryString["bid"].ToString().Trim());
                }
            }
        }

        protected string Search1C3Filter { get; set; }

        protected string SearchHotCommentProductsHTML { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            ((Site)this.Master).IsHomePage = false;
            if (!IsPostBack)
            {
                SubCategoryNavigation1.C1SysNo = BrandProductHelper.GetBrandProductsC1SysNo(BrandID);
                Search1C3Filter = BrandProductHelper.GetBrandProductsC3Names(BrandID);
                SearchHotCommentProductsHTML = FrontProductsHelper.GetSearchHotCommentedProductsHTMLWrapper();
            }
        }
    }
}