using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

using Icson.Utils;
using Icson.Objects.Online;
using Icson.BLL.Online;
using YoeJoyHelper.Extension;

namespace YoeJoyHelper.Security
{
    /// <summary>
    /// 安全页面基础类型
    /// 后台用户中心必须继承这个page类
    /// </summary>
    public class SecurityPageBase : System.Web.UI.Page
    {
        protected void CheckProfile(HttpContext context)
        {
            if (!IsPostBack)
            {
                IcsonSessionInfo oSession = CommonUtility.GetUserSession(context);
                if (oSession.sCustomer == null)
                {
                    string loginUrl = String.Concat(YoeJoyConfig.SiteBaseURL, "User/Login.aspx?from=",Context.Request.RawUrl.GetUrlEncodeStr());
                    Response.Redirect(loginUrl);
                }
            }
        }

        protected void ShowError(string message)
        {
            string url = "../CustomError.aspx?msg=" + Server.UrlEncode(message);
            Response.Redirect(url);
        }

        /// <summary>
        /// 设置输出显示
        /// </summary>
        /// <param name="lbl">显示信息的lab控件ID</param>
        /// <param name="msg">显示信息</param>
        /// <param name="status">信息类型：小于0为错误信息，大于0为正确信息，-1显示自定义错误信息。-2显示默认错误信息</param>
        protected static void Assert(Label lbl, string msg, int status)
        {
            Util.Assert(lbl, msg, status);
        }

        protected static bool Assert(Label lbl, ArrayList errorList)
        {
            return Util.Assert(lbl, errorList);
        }

    }
}
