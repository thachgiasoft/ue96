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

namespace YoeJoyWeb.Controls
{
    public partial class UserProfiler : System.Web.UI.UserControl
    {

        private static readonly string profileHeaderTxtWithoutLogin = @"<a href='" + YoeJoyConfig.SiteBaseURL + @"User/Login.aspx?from={0}'> 登录 </a>|<a href='User/Login.aspx?act=register'>
             免费注册 </a>";

        private static readonly string profileHeaderTxtWithLogin = @"<h6>
        &nbsp;欢迎 &nbsp;&nbsp;&nbsp;&nbsp; &nbsp{0}&nbsp;</h6>";

        /// <summary>
        /// 我的攸怡账户页面
        /// </summary>
        protected static string MyProfileURL
        {
            get
            {
                return String.Concat(YoeJoyConfig.SiteBaseURL, "User/MyProfile.aspx");
            }
        }

        /// <summary>
        /// 帮助中心页面
        /// </summary>
        protected static string HelprCenterURL
        {
            get
            {
                return String.Concat(YoeJoyConfig.SiteBaseURL, "help/help.aspx");
            }
        }

        /// <summary>
        /// 我的历史记录
        /// </summary>
        protected static string MyHistoryURL
        {
            get
            {
                return String.Concat(YoeJoyConfig.SiteBaseURL, "User/MyHistory.aspx");
            }
        }

        protected string ProfilerHeadHTML { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IcsonSessionInfo oSession = CommonUtility.GetUserSession(Context);
                if (oSession.sCustomer != null)
                {
                    ProfilerHeadHTML = String.Format(profileHeaderTxtWithLogin, oSession.sCustomer.CustomerID);
                }
                else
                {
                    ProfilerHeadHTML = String.Format(profileHeaderTxtWithoutLogin, Context.Request.RawUrl.GetUrlEncodeStr());
                }
            }
        }
    }
}