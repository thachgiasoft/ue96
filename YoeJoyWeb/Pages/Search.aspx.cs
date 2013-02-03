using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YoeJoyHelper.Extension;
using YoeJoyHelper;

namespace YoeJoyWeb.Pages
{
    public partial class Search : System.Web.UI.Page
    {

        protected string KeyWords
        {
            get
            {
                if (Request.QueryString["q"] == null)
                {
                    return String.Empty;
                }
                else
                {
                    return Request.QueryString["q"].ToString().GetUrlDecodeStr();
                }
            }
        }
        
        protected string Search1C3Filter { get; set; }

        protected string SearchHotCommentProductsHTML { get; set; }

        protected string Research { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            ((Site)this.Master).IsHomePage = false;
            if (!IsPostBack)
            {
                Research = SearchHelper.relevancesearch(KeyWords);
                SubCategoryNavigation1.C1SysNo = SearchHelper.GetSearchResultC1SysNo(KeyWords);
                Search1C3Filter = SearchHelper.InitSearch1C3ProductFilter(KeyWords);
                SearchHotCommentProductsHTML = FrontProductsHelper.GetSearchHotCommentedProductsHTMLWrapper();
            }
        }
    }
}