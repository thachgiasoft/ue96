using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YoeJoyHelper;
using System.Collections;

using Icson.Utils;
using Icson.Objects.Online;
using Icson.BLL;
using Icson.BLL.Basic;
using Icson.BLL.Online;
using Icson.Objects;
using Icson.Objects.Sale;
using Icson.Objects.Basic;
using Icson.BLL.Sale;

namespace YoeJoyWeb.Controls
{
    public partial class OnlieShoppingCart : System.Web.UI.UserControl
    {
        /// <summary>
        /// 未登录状态下的购物车HTML代码
        /// 或者购物车为空时
        /// </summary>
        protected static string EmptyShoppingCartHTML = @"<div id='chart'>
        <span>购物车:<b><a href='javascript:void(0);'>0</a></b> 件
        </span>
        <a class='chartBt' href='javascript:void(0);'></a>
        <a class='js' href='javascript:void(0);'>结算</a>
    </div><div id='chartContent'>
        <h5><span></span></h5>
        <div id='myShoppingCart' class='shopping'>您的购物车中还没有商品，快去选购吧！</div>
    </div>";

        protected string OnlieShoppingCartHTML { get; set; }

        protected Hashtable newHt = new Hashtable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (CommonUtility.GetUserSession(Context).sCustomer != null)
            {
                Hashtable ht = new Hashtable();

                if (newHt.Count == 0)
                {
                    ht = CartManager.GetInstance().GetCartHash();
                }
                else
                {
                    ht = newHt;
                }

                if (ht == null || ht.Count == 0)
                {
                    OnlieShoppingCartHTML = EmptyShoppingCartHTML;
                }
                else
                {
                    OnlieShoppingCartHTML = CustomerHelper.GetCustomerShoppingCartShortCuts(ht);
                }
            }
            else
            {
                OnlieShoppingCartHTML = EmptyShoppingCartHTML;
            }
        }
    }
}