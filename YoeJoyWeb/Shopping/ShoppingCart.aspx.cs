using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Data.Sql;
using YoeJoyHelper;
using YoeJoyHelper.Security;

using Icson.Utils;
using Icson.Objects.Online;
using Icson.BLL;
using Icson.BLL.Basic;
using Icson.BLL.Online;
using Icson.Objects;
using Icson.Objects.Sale;
using Icson.Objects.Basic;
using Icson.BLL.Sale;

namespace YoeJoyWeb.Shopping
{
    public partial class ShoppingCart : SecurityPageBase
    {
        protected IcsonSessionInfo oSession;
        protected SOInfo soInfo = new SOInfo();

        protected string ShoppingCartHTML { get; set; }

        protected Hashtable newHt = new Hashtable();

        protected void Page_Load(object sender, System.EventArgs e)
        {
            base.CheckProfile(Context);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            oSession = CommonUtility.GetUserSession(Context);
            Hashtable ht = CartManager.GetInstance().GetCartHash();
            ShoppingCartHTML = CustomerHelper.GetCustomerShoppingCart(ht);
        }
    }
}