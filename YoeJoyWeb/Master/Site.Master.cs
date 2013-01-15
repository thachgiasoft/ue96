using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YoeJoyHelper;
using YoeJoyHelper.Extension;

using Icson.Utils;
using Icson.Objects.Online;
using Icson.BLL.Online;

namespace YoeJoyWeb
{
    public partial class Site : System.Web.UI.MasterPage
    {
        //判断是否是主页
        public bool IsHomePage { get; set; }

        protected string LeftTopDivTag { get; set; }

        protected string SiteBaseURLSearch
        {
            get
            {
                return String.Concat(YoeJoyConfig.SiteBaseURL, "Pages/Search.aspx");
            }
        }

        protected string SiteBaseURL
        {
            get
            {
                return YoeJoyConfig.SiteBaseURL;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            checkAutoLogin();
            if (IsHomePage)
            {
                LeftTopDivTag = "<div class='left'>";
            }
            else
            {
                LeftTopDivTag = "<div class='left ItemSort1' id='ItemSortCon'>";
            }
        }

        /// <summary>
        /// 检测自动登录
        /// </summary>
        private void checkAutoLogin()
        {
            if (Request.Cookies["LocalSession"] != null)
            {
                if (CommonUtility.GetUserSession(Context).sCustomer == null)
                {
                    string authticationUrl = SiteBaseURL + "Service/Authtication.aspx";
                    string token = Request.Cookies["LocalSession"].Value.GetUrlEncodeStr();
                    string from = Request.RawUrl;
                    Response.Redirect(authticationUrl+"?token="+token+"&from="+from);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }

    }
}