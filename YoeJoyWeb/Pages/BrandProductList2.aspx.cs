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
    public partial class BrandProductList2 : System.Web.UI.Page
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


        protected string Search2C3Filter { get; set; }

        protected string SearchHotCommentProductsHTML { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            ((Site)this.Master).IsHomePage = false;
            if (!IsPostBack)
            {
                SubCategoryNavigation1.C1SysNo = C1SysNo;
                Search2C3Filter = FrontProductsHelper.InitC3ProductFilterWrapper(C3SysNo);
                SearchHotCommentProductsHTML = FrontProductsHelper.GetSearchHotCommentedProductsHTMLWrapper();
            }
        }
    }
}