using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YoeJoyHelper;
using YoeJoyHelper.Security;

namespace YoeJoyWeb.User
{
    public partial class MyHistory : SecurityPageBase
    {

        protected string UserBrowserHistoryProductsAllHTML { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckProfile(Context);
            UserBrowserHistoryProductsAllHTML = CustomerHelper.GetCustomerBrowserHistoryProductsAllHTML();
        }

        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            CustomerHelper.ClearCustomerBrowserHistoryProductsAll();
        }
    }
}