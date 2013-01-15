using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YoeJoyHelper;
using Icson.Utils;
using Icson.Objects.Online;
using Icson.BLL.Online;

namespace YoeJoyWeb.Master
{
    public partial class Payment : System.Web.UI.MasterPage
    {

        protected string UserIDTxt
        {
            get
            {
                IcsonSessionInfo oSession = CommonUtility.GetUserSession(Context);
                if (oSession.sCustomer == null)
                {
                    return String.Empty;
                }
                else
                {
                    return oSession.sCustomer.CustomerID;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}