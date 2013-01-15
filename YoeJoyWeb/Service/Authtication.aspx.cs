using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YoeJoyHelper;
using YoeJoyHelper.Security;
using YoeJoyHelper.Extension;

namespace YoeJoyWeb.Service
{
    public partial class Authtication : System.Web.UI.Page
    {

        private string Token
        {
            get
            {
                if (Request.QueryString["token"] == null)
                {
                    return String.Empty;
                }
                else
                {
                    return Request.QueryString["token"].ToString().Trim();
                }
            }
        }

        private string From
        {
            get
            {
                if (Request.QueryString["from"] == null)
                {
                    return String.Empty;
                }
                else
                {
                    return Request.QueryString["from"].ToString().Trim();
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var responseURL = From;
            if (String.IsNullOrEmpty(Token))
            {
                Response.Redirect(responseURL);
            }
            else
            {
                string[] tokenValue = DESProvider.DecryptString(Token).Split(',');
                string name = tokenValue[0].Trim();
                string password = tokenValue[1].Trim();
                string msg = String.Empty;
                bool result = CustomerHelper.CustomerLogin(Context, name, password, out msg);
                if (result)
                {
                    Response.Redirect(responseURL);
                }
                else
                {
                    string loginURL = YoeJoyConfig.SiteBaseURL + "User/Login.aspx";
                    Response.Write(@"<script type='text/javascript'>alert('" + msg + "');window.location.href = '" + loginURL + "';</script>");
                }
            }

        }
    }
}