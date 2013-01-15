using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YoeJoyHelper;
using Icson.Objects;
using Icson.Utils;
using YoeJoyHelper.Security;

namespace YoeJoyWeb
{
    public partial class UserLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string name = Request["name"].ToString().Trim();
                string password = Request["pass"].ToString().Trim();
                string external = Request["extern"].ToString().Trim();

                bool autoLogin = false;
                if (String.Equals(external, "autoLogin"))
                {
                    autoLogin = true;
                }

                string msg = String.Empty;

                bool result = CustomerHelper.CustomerLogin(Context, name, password, out msg);

                if (result)
                {
                    System.Web.HttpCookie mycookie = new System.Web.HttpCookie("LoginInfo");	//申明新的COOKIE变量
                    mycookie.Domain = YoeJoyConfig.SiteBaseURL;
                    mycookie.Expires = DateTime.Now.AddYears(1);
                    mycookie.Value = name + "," + DateTime.Now.ToString(AppConst.DateFormatLong);
                    Response.Cookies.Add(mycookie);
                    //添加自动登录的cookie
                    if (autoLogin)
                    {
                        string cookieValue = String.Concat(name, ",", password);
                        string encriptValue = DESProvider.EncryptString(cookieValue);
                        var cookie = new System.Web.HttpCookie("LocalSession", encriptValue);
                        cookie.Expires = DateTime.Now.AddDays(7);
                        Response.Cookies.Add(cookie);
                    }
                }
                Response.Write(JsonContentTransfomer<object>.GetJsonContent(new { IsSuccess = result, Msg = msg }));
            }
        }
    }
}