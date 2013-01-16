using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YoeJoyHelper;

namespace YoeJoyWeb.User
{
    public partial class Login : System.Web.UI.Page
    {

        protected string AuthticationForQQURL
        {
            get
            {
                return String.Concat(YoeJoyConfig.SiteBaseURL, "Service/qc_callback.html");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}