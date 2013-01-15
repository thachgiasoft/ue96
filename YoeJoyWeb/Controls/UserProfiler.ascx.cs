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
        private static readonly string profileHeaderTxtWithoutLogin =@"<h6>
        [&nbsp;<a href='"+YoeJoyConfig.SiteBaseURL+@"User/Login.aspx?from={0}'> 登录 </a>&nbsp;]&nbsp;&nbsp;&nbsp; [&nbsp;<a href='User/Login.aspx?act=register'>
            注册新用户 </a>&nbsp;]
    </h6>";
        private static readonly string profileHeaderTxtWithLogin = @"<h6>
        &nbsp;欢迎 &nbsp;&nbsp;&nbsp;&nbsp; &nbsp{0}&nbsp;</h6>";

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
                    ProfilerHeadHTML =String.Format(profileHeaderTxtWithoutLogin,Context.Request.RawUrl.GetUrlEncodeStr());
                }
            }
        }
    }
}