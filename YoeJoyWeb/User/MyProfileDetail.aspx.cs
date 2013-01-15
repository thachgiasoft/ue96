using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YoeJoyHelper.Security;

namespace YoeJoyWeb.User
{
    public partial class MyProfileDetail : SecurityPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckProfile(Context);
        }
    }
}