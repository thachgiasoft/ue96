using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YoeJoyHelper;
using System.Globalization;
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

namespace YoeJoyWeb.Service
{
    public partial class ShoppingCartService : System.Web.UI.Page
    {

        /// <summary>
        /// 购物车的服务指令
        /// </summary>
        protected string Cmd
        {
            get
            {
                if (Request["cmd"] == null)
                {
                    return String.Empty;
                }
                else
                {
                    string command = Request.QueryString["cmd"].ToString().Trim().ToLower(CultureInfo.InvariantCulture);
                    return command;
                }
            }
        }

        /// <summary>
        /// 放入购物车的商品ID
        /// </summary>
        protected int ProductSysNo
        {
            get
            {
                if (Request["pid"] == null)
                {
                    return 0;
                }
                else
                {
                    return int.Parse(Request["pid"].ToString().Trim());
                }
            }
        }

        /// <summary>
        /// 放入购物车的商品数量
        /// </summary>
        protected int ProductQty
        {
            get
            {
                if (Request["qtp"] == null)
                {
                    return 1;
                }
                else
                {
                    return int.Parse(Request["qtp"].ToString().Trim());
                }
            }
        }

        /// <summary>
        /// 空的购物车初始HTML代码
        /// </summary>
        private static readonly string emptyShoppingCartHTML = @"<div id='chart'>
        <span>购物车:<b><a href='javascript:void(0);'>0</a></b> 件
        </span>
        <a class='chartBt' href='javascript:void(0);'></a>
        <a class='js' href='javascript:void(0);'>结算</a>
    </div><div id='chartContent'>
        <h5><span></span></h5>
        <div id='myShoppingCart' class='shopping'>您的购物车中还没有商品，快去选购吧！</div>
    </div>";

        protected Hashtable newHt = new Hashtable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (CommonUtility.GetUserSession(Context).sCustomer != null)
            {
                if (!IsPostBack)
                {
                    string msg = String.Empty;
                    bool result = false;
                    try
                    {
                        if (ProductSysNo != 0)
                        {
                            switch (Cmd)
                            {
                                //添加商品到购物车
                                case "add":
                                    {
                                        CartInfo oInfo = new CartInfo()
                                        {
                                            ProductSysNo = ProductSysNo,
                                            Quantity = ProductQty,
                                            ExpectQty = ProductQty,
                                        };
                                        newHt = CartManager.GetInstance().GetCartHash();
                                        newHt.Add(ProductSysNo, oInfo);
                                        Hashtable ht = new Hashtable();
                                        ht.Add(ProductSysNo, oInfo);
                                        CartManager.GetInstance().AddToCart(ht);
                                        result = true;
                                        msg = "成功添加";
                                        Response.Write(JsonContentTransfomer<object>.GetJsonContent(new { IsSuccess = result, Msg = msg }));
                                        break;
                                    }
                                //删除单个购物车中的商品
                                case "delete":
                                    {
                                        CartManager.GetInstance().DeleteFromCart(ProductSysNo);
                                        msg = "删除成功";
                                        result = true;
                                        Response.Write(JsonContentTransfomer<object>.GetJsonContent(new { IsSuccess = result, Msg = msg }));
                                        break;
                                    }
                                //默认什么都不做
                                default:
                                    {
                                        break;
                                    }
                            }
                        }
                        else
                        {
                            //浏览购物车
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

                                Response.Write(emptyShoppingCartHTML);
                            }
                            else
                            {
                                Response.Write(CustomerHelper.GetCustomerShoppingCart(ht));
                            }
                        }
                    }
                    catch
                    {
                        msg = "用户请求的操作失败";
                        Response.Write(JsonContentTransfomer<object>.GetJsonContent(new { IsSuccess = result, Msg = msg }));
                    }
                }
            }
            else
            {
                Response.Write(JsonContentTransfomer<object>.GetJsonContent(new { IsSuccess = false, Msg = "请先登录" }));
            }
        }


    }
}
