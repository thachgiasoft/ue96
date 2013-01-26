using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Icson.Objects.Basic;
using Icson.Objects.Online;
using Icson.Objects;
using Icson.Objects.Finance;
using Icson.Utils;
using Icson.BLL;
using Icson.BLL.Online;
using Icson.BLL.Sale;
using Icson.BLL.Finance;
using Icson.BLL.RMA;
using System.Text;
using YoeJoyHelper;
using System.Data;
using YoeJoyHelper.Security;

namespace YoeJoyWeb.User
{
    public partial class MyOrder : SecurityPageBase
    {
        IcsonSessionInfo oS;

        protected string OrderListHTML { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

            base.CheckProfile(Context);

            oS = CommonUtility.GetUserSession(Context);
            OrderListHTML=CustomerHelper.GetCustomOrderList(oS.sCustomer.SysNo,0);
        }
    }
}