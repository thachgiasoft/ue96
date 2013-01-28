using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using YoeJoyHelper.Security;
using System.Collections;
using YoeJoyHelper.Model;
using System.Globalization;
using System.Data;
using System.Threading;

using Icson.Utils;
using Icson.Objects;
using Icson.Objects.Basic;
using Icson.Objects.Online;
using Icson.Objects.Sale;
using Icson.BLL;
using Icson.BLL.Online;
using Icson.BLL.Basic;
using Icson.BLL.Sale;
using Icson.Objects.Finance;
using Icson.BLL.Finance;
using Icson.BLL.RMA;
using System.Text;


namespace YoeJoyHelper
{
    /// <summary>
    /// 用户的helper类
    /// </summary>
    public class CustomerHelper
    {
        /// <summary>
        /// 注册新用户
        /// </summary>
        public static bool RegisterNewCustomer(HttpContext context, NewRegisterCustomerModel customer, out string msg)
        {
            bool isSuccess = false;
            msg = String.Empty;

            string customerID = customer.CustomerID.Trim();
            string password1 = customer.PassWordInput1.Trim();
            string password2 = customer.PassWordInput2.Trim();
            string customerEmail = customer.CustomerEmail.Trim();

            if (customerID == "")
            {
                msg += "请输入用户名！<br />";
            }
            else if (!CommonUtility.IsValidNum(customerID, "^[\u4e00-\u9fa5a-zA-Z]+$"))//原需求只允许中英文
            {
                msg += "用户名只能包含中英文字符！<br />";
            }
            else if (customerID.Length < 3 || customerID.Length > 20)
            {
                msg += "用户名长度必须大于等于3个字符！<br />";
            }
            if (password1 == "")
            {
                msg += "请输入密码！<br />";
            }
            else if (!CommonUtility.IsValidNum(password1, "[a-zA-Z0-9]+$"))//原需求只允许英文数字组合
            {
                msg += "密码只能是英文数字组合！<br />";
            }
            else if (password1.Length < 6 || password1.Length > 20)
            {
                msg += "密码长度必须大于等于6个字符！<br />";
            }
            else if (password2 == "")
            {
                msg += "请输入确认密码！<br />";
            }
            else if (password2 != password1)
            {
                msg += "请确保两次输入的密码一致！<br />";
            }

            if (customerEmail == "")
            {
                msg += "请输入电子邮箱！<br />";
            }
            else if (!Util.IsEmailAddress(customerEmail))
            {
                msg += "请正确输入电子邮箱地址！";
            }

            try
            {

                //定义一个用户对象并赋值
                CustomerInfo oCustomer = new CustomerInfo();
                //-----基础的三个信息，用户名，密码，邮箱---//
                oCustomer.CustomerID = customerID;
                //DESC加密用户密码
                oCustomer.Pwd = DESProvider.EncryptString(password1, YoeJoyConfig.DESCEncryptKey);
                oCustomer.Email = customerEmail;

                //---其他信息---//
                oCustomer.EmailStatus = (int)AppEnum.EmailStatus.Origin;
                oCustomer.Status = (int)AppEnum.BiStatus.Valid;
                oCustomer.DwellAreaSysNo = AppConst.IntNull;
                oCustomer.ReceiveAreaSysNo = AppConst.IntNull;

                oCustomer.CustomerRank = (int)AppEnum.CustomerRank.Ordinary;
                oCustomer.IsManualRank = (int)AppEnum.YNStatus.No;
                oCustomer.CustomerType = (int)AppEnum.CustomerType.Personal;

                oCustomer.RegisterTime = DateTime.Now;
                oCustomer.LastLoginTime = DateTime.Now;
                oCustomer.LastLoginIP = context.Request.UserHostAddress;

                oCustomer.ValidScore = 0;
                oCustomer.TotalScore = 0;
                oCustomer.ValidFreeShipFee = 0;
                oCustomer.TotalFreeShipFee = 0;


                //注册操作
                CustomerManager.GetInstance().Insert(oCustomer);

                IcsonSessionInfo oSession = (IcsonSessionInfo)context.Session["IcsonSessionInfo"];
                if (oSession == null)
                {
                    oSession = new IcsonSessionInfo();
                    context.Session["IcsonSessionInfo"] = oSession;
                }
                //指定当前用户为注册的用户
                oSession.sCustomer = oCustomer;

                isSuccess = true;
            }
            catch (BizException exp)
            {
                msg = exp.Message;
            }
            catch (Exception ex)
            {
                ErrorLog.GetInstance().Write(ex.ToString());
                string url = "../CustomError.aspx?msg=" + context.Server.UrlEncode("用户注册失败！");
                context.Response.Redirect(url);
            }

            if (isSuccess)
            {
                //Response.Redirect("../Customer/NewCustomer.aspx?Type=success");
                msg += "注册成功";
                //lblErrmsg.Text = "恭喜您，注册成功！<br/>";
                //lblErrmsg.Text += "<a href='../Account/AccountCenter.aspx'><span style='color:#FF298F'>请点击进入用户中心！</ span></ a>";

                //Response.Redirect("../Account/AccountCenter.aspx");
            }
            return isSuccess;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool CustomerLogin(HttpContext context, string name, string password, out string msg)
        {
            bool isSuccess = false;
            msg = String.Empty;

            if (name == "")
            {
                msg = "请输入用户名！";
                return isSuccess;
            }

            if (password == "")
            {
                msg = "密码不能为空！";
                return isSuccess;
            }

            CustomerInfo oCustomer = null;
            oCustomer = CustomerManager.GetInstance().Load(name);
            string encryptPassword = DESProvider.EncryptString(password, YoeJoyConfig.DESCEncryptKey);
            //string encryptPassword1 = DESProvider.DecryptString(oCustomer.Pwd, YoeJoyConfig.DESCEncryptKey);
            if (oCustomer == null)
            {
                msg = "用户不存在";
                return isSuccess;
            }

            if (oCustomer.Pwd != encryptPassword)
            {
                msg = "密码不正确";
                return isSuccess;
            }
            else if (oCustomer.Status != (int)AppEnum.BiStatus.Valid)
            {
                msg = "用户名已经作废";
                return isSuccess;
            }
            else
            {
                //初始化会员级别，删除过期会员级别
                //NewPointManager.GetInstance().DelOverDueRank(oCustomer.SysNo);
                //oCustomer = CustomerManager.GetInstance().Load(name);
                //NewPointManager.GetInstance().InitRank(oCustomer.SysNo, oCustomer.CustomerRank);
                //NewPointManager.GetInstance().DelOverDueRank(oCustomer.SysNo);
                oCustomer = CustomerManager.GetInstance().Load(name);

                System.Web.HttpCookie mycookie = new System.Web.HttpCookie("LoginInfo");	//申明新的COOKIE变量
                mycookie.Domain = YoeJoyConfig.SiteBaseURL;
                mycookie.Expires = DateTime.Now.AddYears(1);
                mycookie.Value = name + "," + DateTime.Now.ToString(AppConst.DateFormatLong);
                context.Response.Cookies.Add(mycookie);

                IcsonSessionInfo oSession = CommonUtility.GetUserSession(context);
                oSession.sCustomer = oCustomer;

                Hashtable ht = new Hashtable(5);
                ht.Add("SysNo", oCustomer.SysNo);
                ht.Add("LastLoginIP", context.Request.UserHostAddress);
                ht.Add("LastLoginTime", DateTime.Now);
                CustomerManager.GetInstance().Update(ht);

                //if (oCustomer.IsManualRank != (int)AppEnum.YNStatus.Yes)
                //{
                //    int customerRank = CustomerManager.GetInstance().SetRank(oCustomer.SysNo);
                //    oSession.sCustomer.CustomerRank = customerRank;
                //}

                isSuccess = true;

            }

            return isSuccess;
        }

        /// <summary>
        /// 获得用户的基本信息
        /// </summary>
        /// <param name="cInfo"></param>
        /// <returns></returns>
        public static string GetCustomerBasicInfo(CustomerInfo cInfo)
        {
            string customInfoHTML = String.Empty;

            StringBuilder strb = new StringBuilder("<div id='myInfo'>");

            //            string profileHTMLTemplate = @"
            //        <div class='l'>
            //            <img src='../static/images/tx02.jpg'>
            //            <a href='#'>编辑头像</a>
            //        </div>
            //        <div class='r'>
            //            <h2>
            //                <em>您好，{0}</em><strong>高级会员</strong><em>[</em><b>{1}</b><em>]</em> <span>上一次登录时间：{2}</span>
            //            </h2>
            //            <ul class='infoDetail'>
            //                <li>等待付款订单(0)</li>
            //                <li>等待收货订单(0)</li>
            //                <li><a href='#'>等待评价商品(10)</a></li>
            //                <li>攸怡积分:<b>{3}</b></li>
            //                <li>冻结积分:<b>32</b></li>
            //                <li><a href='#'>优惠券:<b>2</b></a></li>
            //                <li><a href='#'>站内信(<b>2</b>)</a></li>
            //            </ul>
            //        </div>";

            string profileHTMLTemplate = @"
        <div class='l'>
            <img src='../static/images/tx02.jpg'>
            <a href='#'>编辑头像</a>
        </div>
        <div class='r'>
            <h2>
                <em>您好，{0}</em><strong>{1}</strong><em>[</em><b>{2}</b><em>]</em> <span>上一次登录时间：{3}</span>
            </h2>
            <ul class='infoDetail'>
                <li>等待付款订单(0)</li>
                <li>等待收货订单(0)</li>
                <li><a href='#'>等待评价商品(0)</a></li>
                <li>攸怡积分:<b>{4}</b></li>
            </ul>
        </div>";

            string emailStatus = String.Empty;
            if (cInfo.EmailStatus == (int)AppEnum.EmailStatus.Origin)
            {
                emailStatus = "未验证";
            }
            else
            {
                emailStatus = "已验证";
            }

            strb.Append(String.Format(profileHTMLTemplate, cInfo.CustomerID, emailStatus, AppEnum.GetCustomerRank(cInfo.CustomerRank), cInfo.LastLoginTime, cInfo.ValidScore > 0 ? cInfo.ValidScore : 0));

            strb.Append("</div>");
            customInfoHTML = strb.ToString();
            return customInfoHTML;
        }

        /// <summary>
        /// 获得用户的购物车
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public static string GetCustomerShoppingCart(Hashtable ht)
        {
            int productCount = 0;
            float productTotalPrice = 0;
            string siteBaseURL = YoeJoyConfig.SiteBaseURL;
            List<FrontDsiplayProduct> products = CustomerShoppingCartService.GetShoppingCartProducts(ht);
            string shoppingCartHTML = String.Empty;
            if (products != null)
            {

                productCount = products.Count;
                StringBuilder strb = new StringBuilder();

                string liHTML = @"<p class='l'>
                        <a href='{0}'>
                            <img alt='{1}' src='{2}' width='30' height='30'></a><a class='goodsName'
                                href='{3}'>{4}</a><b>￥{5}</b>
                    </p>
                    <div class='r'>
                        <a class='sub' href='javascript:void(0)'>-</a>
                        <input class='num' maxlength='3' value='1' type='text'/>
                        <a class='add' href='javascript:void(0)'>+</a>
                        <p onClick='javascript:DeleteShoppingCartItem(this);'>
                            删除 <input type='hidden' value='{6}'/></p>
                    </div>";

                foreach (FrontDsiplayProduct product in products)
                {
                    string deeplink = YoeJoyConfig.SiteBaseURL + "Pages/Product.aspx?c1=" + product.C1SysNo + "&c2=" + product.C2SysNo + "&c3=" + product.C3SysNo + "&pid=" + product.ProductSysNo;
                    string image = YoeJoyConfig.ImgVirtualPathBase + product.ImgPath;
                    strb.Append(String.Format(liHTML, deeplink, product.ProductBriefName, image, deeplink, product.ProductBriefName, product.Price, product.ProductSysNo));
                    productTotalPrice += float.Parse(product.Price);
                }

                shoppingCartHTML = strb.ToString();
            }

            string shoppingCartHTMLWrapper1 = String.Format(@"<div id='chart'>
        <span>购物车:<b><a href='{0}Shopping/ShoppingCart.aspx'>{1}</a></b> 件
        </span>
        <a class='chartBt' href='javascript:void(0);'></a>
        <a class='js' href='{2}Shopping/ShoppingCart.aspx'>结算</a>
    </div>
    <div id='chartContent'>
        <h5><span></span></h5>
        <div id='myShoppingCart' class='shopping'>", siteBaseURL, productCount, siteBaseURL);


            string shoppingCartHTMLWrapper2 = String.Format(@"</div>
        <div class='payNow'>
            <div class='l'>
                共<b><a href='{0}Shopping/ShoppingCart.aspx'>{1}</a></b>件商品
            </div>
            <div class='r'>
                <p>
                    合计：<b id='CartTotalPrice'>￥{2}</b></p>
                <a href='{3}Shopping/ShoppingCart.aspx'>
                    <img alt='结算' src='../static/images/jsbt.png' width='61' height='25' /></a>
            </div>
        </div>
    </div>", siteBaseURL, productCount, productTotalPrice.ToString("0.00"), siteBaseURL);


            return String.Concat(shoppingCartHTMLWrapper1, shoppingCartHTML, shoppingCartHTMLWrapper2);
        }

        /// <summary>
        /// 获得用户分页的收藏列表
        /// </summary>
        /// <param name="customerSysNo"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static string GetCustomerWishListProducts(int customerSysNo, int startIndex)
        {
            string wishListHTML = String.Empty;

            int pageCount = int.Parse(YoeJoyConfig.WishListPagedCount);
            List<WishListModule> wishList = WishListService.GetCustomerWishList(customerSysNo, startIndex, pageCount);

            if (wishList != null)
            {
                StringBuilder strb = new StringBuilder();

                ///TODO: Add logic to impelemnt wishlist UI.

                wishListHTML = strb.ToString();
            }

            return wishListHTML;
        }

        /// <summary>
        /// 设置用户的浏览记录
        /// </summary>
        /// <param name="productSysNo"></param>
        public static void SetCustomerBrowserHistory(int productSysNo)
        {
            // 如果product已经存在于cookie中，就不用更新了
            // 更新Cookie
            // 更新Session

            string cookie = CookieUtil.GetDESEncryptedCookieValue(CookieUtil.Cookie_BrowseHistory);

            string newCookie = productSysNo.ToString();

            if (cookie != null)
            {
                string[] productArray = cookie.Split(',');

                int index = 0; // 新的字符串的个数
                for (int i = 0; i < productArray.Length; i++)
                {
                    if (productArray[i] != productSysNo.ToString())
                    {
                        newCookie += ",";
                        try
                        {
                            Convert.ToInt32(productArray[i]);
                        }
                        catch
                        {
                            //出错重置为当前product
                            newCookie = productSysNo.ToString();
                            break;
                        }
                        newCookie += productArray[i];
                        index++;
                    }

                    if (index == 9)
                        break;
                }
            }
            CookieUtil.SetDESEncryptedCookie(CookieUtil.Cookie_BrowseHistory, newCookie, DateTime.MaxValue);
        }

        /// <summary>
        /// 获取在商品吧详细页的用户浏览历史
        /// </summary>
        /// <returns></returns>
        public static string GetProductDetailBrowserHistoryProductsHTML()
        {
            string productDetailBroswerHistoryHTML = String.Empty;

            string cookie = CookieUtil.GetDESEncryptedCookieValue(CookieUtil.Cookie_BrowseHistory);
            ArrayList al = CustomerBrowserHistoryProductService.GetBrowseHistoryList(cookie);

            if (al != null)
            {
                StringBuilder strb = new StringBuilder("<ul class='group'>");

                string liHTML = @"<li><a class='photo' href='{0}'>
                        <img alt='{1}' src='{2}' width='60' height='60'></a>
                        <div>
                            <a class='name' title='{3}' href='{4}'>{5}</a>
                            <span class='adText'>{6}</span>
                        </div>
                        <p class='price'>
                            <b>¥{7}</b><span>¥{8}</span></p>
                    </li>";

                var products = al.ToArray().Take(4);

                foreach (CustomerBrowserHistoryProduct product in products)
                {
                    string thumbImg = YoeJoyConfig.ImgVirtualPathBase + product.SmallImg;
                    string deeplink = YoeJoyConfig.SiteBaseURL + "Pages/Product.aspx?c1=" + product.C1SysNo + "&c2=" + product.C2SysNo + "&c3=" + product.C3SysNo + "&pid=" + product.ProductSysNo;
                    strb.Append(String.Format(liHTML, deeplink, product.ProductBriefName, thumbImg, product.ProductBriefName, deeplink, product.ProductBriefName, product.PromotionWord, product.CurrentPrice, product.StandardPrice));
                }

                strb.Append("</ul>");
                productDetailBroswerHistoryHTML = strb.ToString();
            }
            return productDetailBroswerHistoryHTML;
        }

        /// <summary>
        /// 获得用户中心的最近浏览的商品历史记录
        /// </summary>
        /// <returns></returns>
        public static string GetProfileCenterBorwserHistoryProductsHTML()
        {
            string productDetailBroswerHistoryHTML = String.Empty;

            string cookie = CookieUtil.GetDESEncryptedCookieValue(CookieUtil.Cookie_BrowseHistory);
            ArrayList al = CustomerBrowserHistoryProductService.GetBrowseHistoryList(cookie);

            if (al != null)
            {
                StringBuilder strb = new StringBuilder("<ul class='recentLIst'>");

                string liHTML = @"<li><a class='photo' href='{0}' title='{1}'>
                        <img src='{2}' alt='{3}' 
                            width='60' height='60'></a>
                        <div>
                            <a class='name' title='{4}' href='{5}'>
                                {6}</a> <span class='adText'>{7}</span>
                        </div>
                        <p class='price'>
                            <b>¥{8}</b><span>¥{9}</span></p>
                    </li>";

                var products = al.ToArray().Take(3);

                foreach (CustomerBrowserHistoryProduct product in products)
                {
                    string thumbImg = YoeJoyConfig.ImgVirtualPathBase + product.SmallImg;
                    string deeplink = YoeJoyConfig.SiteBaseURL + "Pages/Product.aspx?c1=" + product.C1SysNo + "&c2=" + product.C2SysNo + "&c3=" + product.C3SysNo + "&pid=" + product.ProductSysNo;
                    strb.Append(String.Format(liHTML, deeplink, product.ProductBriefName, thumbImg, product.ProductBriefName, product.ProductBriefName, deeplink, product.ProductBriefName, product.PromotionWord, product.CurrentPrice, product.StandardPrice));
                }

                strb.Append("</ul>");
                productDetailBroswerHistoryHTML = strb.ToString();
            }
            return productDetailBroswerHistoryHTML;
        }

        /// <summary>
        /// 获得用户的所有浏览商品记录
        /// </summary>
        /// <returns></returns>
        public static string GetCustomerBrowserHistoryProductsAllHTML()
        {
            string productDetailBroswerHistoryHTML = String.Empty;

            string cookie = CookieUtil.GetDESEncryptedCookieValue(CookieUtil.Cookie_BrowseHistory);
            ArrayList al = CustomerBrowserHistoryProductService.GetBrowseHistoryList(cookie);

            if (al != null)
            {
                StringBuilder strb = new StringBuilder();

                string liHTML = @"<tr>
                    <td>
                        <span>2012-09-09 21:30</span>
                    </td>
                    <td>
                        <a href='{0}'>
                            <img src='{1}'></a> <em><b>{2}</b><br>
                                <span>攸怡价：<strong>￥{3}</strong> 市场价：<i>￥{4}</i> </span></em>
                    </td>
                    <td style='text-align: left;'>
                        <a href='#'>
                            <img src='../static/images/history.jpg'></a><br>
                        <a href='#'>
                            <img src='../static/images/adda.jpg'></a><br>
                        <a href='#'>清除浏览历史</a>
                    </td>
                </tr>";

                var products = al.ToArray();

                foreach (CustomerBrowserHistoryProduct product in products)
                {
                    string thumbImg = YoeJoyConfig.ImgVirtualPathBase + product.LargeImg;
                    string deeplink = YoeJoyConfig.SiteBaseURL + "Pages/Product.aspx?c1=" + product.C1SysNo + "&c2=" + product.C2SysNo + "&c3=" + product.C3SysNo + "&pid=" + product.ProductSysNo;
                    strb.Append(String.Format(liHTML, deeplink, thumbImg, product.ProductBriefName, product.CurrentPrice, product.StandardPrice));
                }
                productDetailBroswerHistoryHTML = strb.ToString();
            }
            return productDetailBroswerHistoryHTML;
        }

        /// <summary>
        /// 清除用户的所有cookie
        /// </summary>
        public static void ClearCustomerBrowserHistoryProductsAll()
        {
            CookieUtil.SetDESEncryptedCookie(CookieUtil.Cookie_BrowseHistory, String.Empty);
        }

        /// <summary>
        /// 获得用户的订单列表
        /// </summary>
        /// <param name="customSysNo"></param>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        public static string GetCustomOrderList(int customSysNo, int pageNum)
        {
            string orderListHTML = String.Empty;
            int pageCount = int.Parse(YoeJoyConfig.MyOrderProductListPagedCount);
            List<OrderModuel> orderList = OrderService.GetMyOrder(customSysNo, pageNum, pageCount);
            if (orderList != null)
            {
                StringBuilder strb = new StringBuilder();

                string tabRowHTMLTemplate = @"
                             <tr class='orderList'>
                                <td>
                                	<img src='{0}'>
                                </td>
                                <td>
                                	<p><a href='{1}'>{2}</a></p>
                                    <p><b>¥{3}</b>X{4}</p>
                                </td>
                                <th>
                                	<a class='reBuy' href='{5}'>再次购买</a><br>
                                	<a class='reBuy' href='{6}'>发表评论</a>
                                </th>
                            </tr>";

                foreach (var orderListItem in orderList)
                {

                    strb.Append(@"<table class='order' cellSpacing='0' cellPadding='0'><tbody>");

                    string orderDetailDeeplink = String.Concat(YoeJoyConfig.SiteBaseURL, "MyOrderDetail.aspx?ID=", orderListItem.SysNo);
                    string orderUpdateDeeplink = String.Concat(YoeJoyConfig.SiteBaseURL, "MyOrderDetail.aspx?action=update&ID=", orderListItem.SysNo);
                    string orderCancelDeeplink = String.Concat(YoeJoyConfig.SiteBaseURL, "MyOrderDetail.aspx?action=cancel&ID=", orderListItem.SysNo);

                    bool IsNet = (orderListItem.IsNet == (int)AppEnum.YNStatus.Yes) ? true : false;
                    bool IsPayWhenRecv = (orderListItem.IsPayWhenEcv == (int)AppEnum.YNStatus.Yes) ? true : false;

                    strb.Append("<tr><th class='orderNum' rowSpan='3'>");

                    strb.Append("<p>订单编号：<a href='" + orderDetailDeeplink + "'>" + orderListItem.SysNo + "</a></p>");
                    strb.Append("<p>订购日期：" + orderListItem.OrderDate.ToString("yyyy-mm-dd") + "</p>");
                    if (IsNet)
                    {
                        strb.Append("<p>付款方式：在线支付</p>");
                    }
                    if (IsPayWhenRecv)
                    {
                        strb.Append("<p>订单总计：¥" + orderListItem.TotalCash.ToString("#####0.00") + "</p>");
                    }
                    strb.Append("<p>使用积分：" + orderListItem.PointPay + "</p>");
                    strb.Append("<p>可获积分：" + orderListItem.Pointamt + "</p>");
                    strb.Append("<p><a href='" + orderDetailDeeplink + "'>订单详情</a></p>");

                    strb.Append("</th><td colSpan='3'><h2 class='ordertitlts'>");
                    strb.Append(AppEnum.GetSOStatus(Util.TrimIntNull(orderListItem.Status)));
                    strb.Append("<span class='r'>");

                    bool isPayed = NetPayManager.GetInstance().IsPayed(orderListItem.SysNo);

                    if (orderListItem.Status == (int)AppEnum.SOStatus.Origin && !isPayed)
                    {
                        strb.Append("<a href='MyOrderDetail.aspx?action=cancel&ID=" + orderListItem.SysNo + "'>作废订单</a>");
                        strb.Append("<a href='MyOrderDetail.aspx?action=update&ID=" + orderListItem.SysNo + "'>修改订单</a>");
                    }
                    if (orderListItem.Status == (int)AppEnum.SOStatus.Origin || orderListItem.Status == (int)AppEnum.SOStatus.WaitingPay || orderListItem.Status == (int)AppEnum.SOStatus.WaitingManagerAudit)
                    {
                        if (Util.TrimNull(orderListItem.PagementPage) != AppConst.StringNull && !isPayed && Util.TrimIntNull(orderListItem.IsNet) == (int)AppEnum.YNStatus.Yes)
                        {
                            strb.Append("<a href='../Shopping/" + Util.TrimNull(orderListItem.PagementPage) + "?id=" + orderListItem.SysNo + "&sono=" + orderListItem.SoId + "&soamt=" + orderListItem.TotalCash.ToString("#####0.00") + "'>支付货款</a>");
                        }
                    }

                    DataTable dt = RMARequestManager.GetInstance().GetRMABySO(orderListItem.SysNo);
                    if (dt != null)
                    {
                        strb.Append("<a href='../Account/RMAQuery.aspx?Type=single&ID=" + orderListItem.SysNo + "'>查看返修信息</a>");
                    }
                    strb.Append( "<a href='MyOrderDetail.aspx?ID=" + orderListItem.SysNo + "'>查看订单明细</a>");
                    if (Util.TrimNull(orderListItem.Memo) != String.Empty)
                    {
                        strb.Append("<tr><td height=25px align=right bgcolor=#E7F9F9>备注信息：</td><td colspan=5 bgcolor=#ffffff>" + orderListItem.Memo + "</td></tr>");
                    }
                    strb.Append(" </span></h2></td>");
                    strb.Append("</tr>");

                    if (orderListItem.ProductList != null)
                    {
                        foreach (var productListItem in orderListItem.ProductList)
                        {
                            string deeplink = String.Concat(YoeJoyConfig.SiteBaseURL, "Pages/Product.aspx?c1=", productListItem.C1SysNo, "&c2=", productListItem.C2SysNo, "&c3=", productListItem.C3SysNo, "&pid=", productListItem.ProductSysNo);
                            string image=String.Concat(YoeJoyConfig.ImgVirtualPathBase,productListItem.ImgPath);
                            strb.Append(String.Format(tabRowHTMLTemplate,image,deeplink,productListItem.ProductBriefName,productListItem.Cost,productListItem.Quantity,deeplink,deeplink));
                        }
                    }

                    strb.Append("</tbody></table>");
                }
                orderListHTML = strb.ToString();
            }
            return orderListHTML;
        }

    }
}
