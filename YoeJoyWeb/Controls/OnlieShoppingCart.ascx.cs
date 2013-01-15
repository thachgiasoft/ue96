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
        <img alt='购物车' src='../static/images/gwcbt0.png' width='39' height='32'>
        <a href='javascript:void(0);''>结算</a>
    </div><div id='chartContent'>
        <img alt='背景' src='../static/images/gwctop.png' width='374' height='18'>
        <div id='myShoppingCart' class='shopping'>您的购物车中还没有商品，赶紧选购吧！</div>
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
                    OnlieShoppingCartHTML = CustomerHelper.GetCustomerShoppingCart(ht);
                }
            }
            else
            {
                OnlieShoppingCartHTML = EmptyShoppingCartHTML;
            }
        }
    }
}