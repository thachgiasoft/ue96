using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YoeJoyHelper;
using System.Globalization;
using System.Collections;
using System.Data;

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
        /// 1. add : 添加商品到购物车中
        /// 2. delete: 删除单个商品
        /// 3. clear: 清空购物车
        /// 4. updateCart： 更新购物车内某商品的数量
        /// 5. updateShortCuts: 更新购物车快捷方式内某商品的数量
        /// 6. viewCart : 查看购物车
        /// 7. viewShortCuts: 查看购物车快捷方式
        /// 8. goShopping: 付款，下一步填写订单详细信息
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
                if (Request["qty"] == null)
                {
                    return 1;
                }
                else
                {
                    return int.Parse(Request["qty"].ToString().Trim());
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
                            //更新购物车商品数量
                            case "update":
                                {
                                    CartInfo cart = new CartInfo();
                                    newHt = CartManager.GetInstance().GetCartHash();
                                    cart = (CartInfo)newHt[ProductSysNo];
                                    cart.Quantity = ProductQty;
                                    CartManager.GetInstance().UpdateCart(cart);
                                    msg = "修改成功";
                                    result = true;
                                    Response.Write(JsonContentTransfomer<object>.GetJsonContent(new { IsSuccess = result, Msg = msg }));
                                    break;
                                }
                            case "clear":
                                {
                                    CartManager.GetInstance().ClearCart();
                                    msg = "成功清空购物车";
                                    result = true;
                                    Response.Write(JsonContentTransfomer<object>.GetJsonContent(new { IsSuccess = result, Msg = msg }));
                                    break;
                                }
                            case "viewshortcuts":
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
                                        Response.Write(CustomerHelper.GetCustomerShoppingCartShortCuts(ht));
                                    }
                                    break;
                                }
                            //默认什么都不做
                            case "viewcart":
                                {
                                    break;
                                }
                            case "goshopping":
                                {
                                    goShopping();
                                    break;
                                }
                            default:
                                {
                                    break;
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

        private void goShopping()
        {
            DataSet ds = new DataSet();
            SOInfo oldSoInfo = null;
            var oSession = CommonUtility.GetUserSession(Context);
            try
            {
                ds = SaleManager.GetInstance().GetSOOnlineDs(oSession.sCustomer.SysNo, 1);
            }
            catch
            {
                Response.Redirect("../User/login.aspx?url=../Shopping/ShoppingCart.aspx");
            }
            //-------------------------库存判断--------------------------//

            //newHt = CartManager.GetInstance().GetCartHash();
            //DataSet ds2 = OnlineListManager.GetInstance().GetCartDs(newHt);
            //foreach (DataRow dr in ds2.Tables[0].Rows)
            //{
            //    int que = Int32.Parse(dr["OnlineQty"].ToString());

            //    if (que <= 0)
            //    {
            //        ShowMessage("您要购买的" + dr["productName"] + "已无库存！");
            //        return;
            //    }


            //}
            //------------------------------------------------------------//


            if (Util.HasMoreRow(ds))
            {
                if (ds.Tables[0].Rows.Count == 1)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    int soSysNo = int.Parse(dr["sysno"].ToString());

                    oldSoInfo = SaleManager.GetInstance().LoadSOMaster(soSysNo);
                }
            }


            if (oldSoInfo != null)
            {


                oSession.sSO = new SOInfo();
                oSession.sSO.CustomerSysNo = oSession.sCustomer.SysNo;
                oSession.sSO.ReceiveAddress = oldSoInfo.ReceiveAddress;
                oSession.sSO.ReceiveAreaSysNo = oldSoInfo.ReceiveAreaSysNo;
                oSession.sSO.ReceiveCellPhone = oldSoInfo.ReceiveCellPhone;
                oSession.sSO.ReceiveContact = oldSoInfo.ReceiveContact;
                oSession.sSO.ReceiveName = oldSoInfo.ReceiveName;
                oSession.sSO.ReceivePhone = oldSoInfo.ReceivePhone;
                oSession.sSO.ReceiveZip = oldSoInfo.ReceiveZip;

                oSession.sSO.IsPremium = (int)AppEnum.YNStatus.Yes;
                oSession.sSO.IsVAT = (int)AppEnum.YNStatus.No;
                oSession.sSO.IsWholeSale = (int)AppEnum.YNStatus.No;
                oSession.sSO.VATEMSFee = 0;
                oSession.sSO.VatInfo.VATEMSFee = 0;
                oSession.sSO.FreeShipFeePay = 0;
                oSession.sSO.PayTypeSysNo = oldSoInfo.PayTypeSysNo;
                oSession.sSO.ShipTypeSysNo = oldSoInfo.ShipTypeSysNo;
                var checkOutURL = YoeJoyConfig.SiteBaseURL + "Shopping/CheckOut.aspx";
                Response.Redirect(checkOutURL);
            }

            else
            {
                var precheckOutURL = YoeJoyConfig.SiteBaseURL + "Shopping/precheckout.aspx";
                Response.Redirect("precheckout.aspx");
            }
        }


    }
}
