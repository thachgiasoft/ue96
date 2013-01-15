using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YoeJoyHelper;

namespace YoeJoyWeb.Controls
{
    public partial class SubCategoryNavigation : System.Web.UI.UserControl
    {

        public int C1SysNo { get; set; }

        protected string SubNavigationHTML { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SubNavigationHTML = new CategoryHelper().InitSubCategoryNavigationWrapper(C1SysNo);
            }
        }
    }
}