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
    public partial class BrandProductsResult1 : System.Web.UI.Page
    {

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
                    return int.Parse(Request.QueryString["bid"]);
                }
            }
        }
        protected string C3ProductListFooterHTML { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C3ProductListFooterHTML = BrandProductHelper.InitBrandProductList1Footer(BrandID);
            }
        }
    }
}