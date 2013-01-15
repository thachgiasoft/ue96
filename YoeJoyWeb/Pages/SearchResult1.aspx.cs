using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YoeJoyHelper;
using YoeJoyHelper.Extension;

namespace YoeJoyWeb
{
    public partial class SearchResult1 : System.Web.UI.Page
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
        protected string C3ProductListFooterHTML { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C3ProductListFooterHTML = SearchHelper.InitSearch1C3ProductListFooter(KeyWords);
            }
        }
    }
}