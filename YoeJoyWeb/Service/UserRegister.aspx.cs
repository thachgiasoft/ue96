using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YoeJoyHelper;
using YoeJoyHelper.Model;
using Icson.Objects;
using Icson.Utils;

namespace YoeJoyWeb
{
    /// <summary>
    /// 用户注册的服务
    /// </summary>
    public partial class UserRegister : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                NewRegisterCustomerModel newRegisterCustomer = new NewRegisterCustomerModel()
                {
                    CustomerID = Request["name"].ToString().Trim(),
                    CustomerEmail = Request["email"].ToString().Trim(),
                    PassWordInput1 = Request["pass1"].ToString().Trim(),
                    PassWordInput2 = Request["pass2"].ToString().Trim(),
                };

               

                string msg = String.Empty;

                var result = CustomerHelper.RegisterNewCustomer(Context, newRegisterCustomer, out msg);

                if (result)
                {
                    System.Web.HttpCookie myCookie = new HttpCookie("LoginInfo");
                    myCookie.Domain = YoeJoyConfig.SiteBaseURL;
                    myCookie.Expires = DateTime.Now.AddYears(1);//Cookie过期时间
                    myCookie.Value = newRegisterCustomer.CustomerID + "," + DateTime.Now.ToString(AppConst.DateFormatLong);
                    Response.Cookies.Add(myCookie);//添加到页面cookie中
                }

                Response.Write(JsonContentTransfomer<object>.GetJsonContent(new {IsSuccess=result,Msg=msg}));
            }
        }
    }
}