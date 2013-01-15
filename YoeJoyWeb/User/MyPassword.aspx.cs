using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YoeJoyHelper;
using YoeJoyHelper.Security;
using Icson.Utils;
using Icson.Objects.Online;
using Icson.Objects.Basic;
using System.Collections;

using Icson.BLL.Basic;

namespace YoeJoyWeb.User
{
    public partial class MyPassword : SecurityPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckProfile(Context);
        }

        protected void btnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            if (txtOld.Text.Trim() == "")
            {
                lblErrMsg.Text = "请输入旧密码！";
                return;
            }
            if (txtNew0.Text.Trim() == "" || txtNew1.Text.Trim() == "")
            {
                lblErrMsg.Text = "新的密码不能为空！";
                return;
            }
            if (txtNew0.Text.Trim() != txtNew1.Text.Trim())
            {
                lblErrMsg.Text = "密码输入不一致！";
                return;
            }

            IcsonSessionInfo oSession = CommonUtility.GetUserSession(Context);
            if (oSession.sCustomer == null || oSession.sCustomer.SysNo == AppConst.IntNull)
            {
                Response.Redirect("Login.aspx");
            }

            if (txtOld.Text.Trim() !=DESProvider.DecryptString(oSession.sCustomer.Pwd))
            {
                lblErrMsg.Text = "您输入的旧密码与您的旧密码不一致，不能修改。";
            }
            else
            {
                //更新数据库中的用户密码
                Hashtable ht = new Hashtable(2);
                ht.Add("SysNo", oSession.sCustomer.SysNo);
                ht.Add("Pwd",DESProvider.EncryptString(txtNew0.Text.Trim()));
                CustomerManager.GetInstance().Update(ht);

                //更新session中的密码
                oSession.sCustomer.Pwd = txtNew0.Text.Trim();

                lblErrMsg.Text = "修改成功！";
            }
        }
    }
}