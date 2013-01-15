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
using Icson.Objects.Sale;
using Icson.BLL;
using Icson.BLL.Online;
using Icson.BLL.Basic;

namespace YoeJoyWeb.Shopping
{
    public partial class PreCheckOut : SecurityPageBase
    {

        protected IcsonSessionInfo oSession;
        protected SOInfo soInfo = new SOInfo();

        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckProfile(Context);
            // Put user code to initialize the page here
            oSession = CommonUtility.GetUserSession(Context);
            if (oSession.sCustomer.DwellAreaSysNo == AppConst.IntNull && oSession.sCustomer.ReceiveAreaSysNo == AppConst.IntNull)
            {
                tabInfo.Visible = true;
                lnkInfo.NavigateUrl = "../User/MyAddress.aspx";
            }
            else
            {
                tabInfo.Visible = false;
                //this.soInfo = oSession.sSO;
                //soInfo.VATEMSFee = 0;
                //soInfo.VatInfo.VATEMSFee = 0;
                //oSession.sSO = this.soInfo;
                //Response.Redirect("CheckOut.aspx");
                Response.Redirect("Comfirm.aspx");
            }
        }
    }
}