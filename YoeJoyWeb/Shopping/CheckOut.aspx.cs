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

using Icson.Objects;
using Icson.Utils;
using Icson.Objects.Basic;
using Icson.Objects.Online;
using Icson.Objects.Sale;
using Icson.BLL;
using Icson.BLL.Basic;
using Icson.BLL.Online;
using Icson.BLL.Sale;

namespace YoeJoyWeb.Shopping
{
    public partial class CheckOut : SecurityPageBase
    {
        protected SOInfo soInfo = new SOInfo();
        protected SOInfo oldSoInfo = new SOInfo();
        protected ShipTypeInfo shipTypeInfo = new ShipTypeInfo();
        protected PayTypeInfo payTypeInfo = new PayTypeInfo();
        protected IcsonSessionInfo oSession;
        protected string ppLink = "";
        //private bool HasEnoughAvailableQty = true;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            base.CheckProfile(Context);
            oSession = CommonUtility.GetUserSession(Context);

            this.GetLastOneSo();

            this.GetSOInfo();
            //btnOK.Attributes["onclick"] = "OKClicked()"; 
            if (!IsPostBack)
            {
                if (soInfo.ShipTypeSysNo == 1)
                {
                    trSignByOther.Visible = true;
                }
                else
                {
                    trSignByOther.Visible = false;
                }

                SaleManager.GetInstance().CalcSO(soInfo);
                this.BindSO();

                //客户是否存在未出库可以合并的订单
                trIsMergeSO.Visible = false;
                int AreaSysNo = SaleManager.GetInstance().GetCustomerUnOutstockSOReceiveArea(oSession.sCustomer.SysNo);
                if (AreaSysNo != AppConst.IntNull && AreaSysNo == soInfo.ReceiveAreaSysNo)
                {
                    trIsMergeSO.Visible = true;
                }
                //ucSelectDeliveryTime.BindDeliveryDateTime(HasEnoughAvailableQty, DateTime.Now, 12);
                if (chkInvoice.Checked == true)
                    vatInfo.Style.Add("display", "block");
                else
                    vatInfo.Style.Add("display", "none");

                //if (soInfo.SOAmt < 10)
                //{
                trPP.Visible = false;
                //}

                if (trPP.Visible && soInfo.SOAmt >= 10)
                {
                    int ppMaxNum = Util.DecimalToU_Int32(soInfo.SOAmt) / 100 + 1;
                    if (ppMaxNum > 10)
                        ppMaxNum = 10;

                    string time = DateTime.Now.ToString("yyyyMMddmmssms");
                    string key = "";
                    string md5 = Util.GetMD5(time + ppMaxNum.ToString() + key).ToUpper();
                    ppLink = "<a href='http://club.pchome.net/union/icson/?time=" + time + "&maxNum=" + ppMaxNum.ToString() + "&mac=" + md5 + "' target='_blank' style='text-decoration: underline;font-weight:bolder;color: #ff0033'>获得校验码</a></td>";
                }

                if (soInfo.CouponAmt > 0)
                {
                    int ppNum = Util.DecimalToU_Int32(soInfo.SOAmt) / 100 + 1;
                    if (soInfo.CouponAmt > ppNum * 2)
                        soInfo.CouponAmt = ppNum * 2;
                }
            }
        }



        private void GetSOInfo()
        {
            //如果session中有已生成的订单，清除之
            if (oSession.sSO != null && oSession.sSO.SysNo != AppConst.IntNull)
                oSession.sSO = null;

            this.soInfo = oSession.sSO;
            this.InitSOItem();
        }

        private void InitSOItem()
        {
            soInfo.ItemHash.Clear();

            Hashtable cartHash = CartManager.GetInstance().GetCartHash();
            if (cartHash.Count == 0)
                Response.Redirect("ShoppingCart.aspx");
            Hashtable sysNoHash = new Hashtable(5);
            string productList = "";
            foreach (CartInfo cartInfo in cartHash.Values)
            {
                sysNoHash.Add(cartInfo.ProductSysNo, null);
                productList += cartInfo.ProductSysNo + ",";
            }

            Hashtable soProductHash = SaleManager.GetInstance().LoadSOProducts(sysNoHash, (int)AppEnum.SOItemType.ForSale);
            //加入主商品
            foreach (SOItemInfo soItem in soProductHash.Values)
            {
                soItem.Quantity = ((CartInfo)cartHash[soItem.ProductSysNo]).Quantity;
                soItem.ExpectQty = ((CartInfo)cartHash[soItem.ProductSysNo]).ExpectQty;
                soItem.DiscountAmt = 0m;
                soItem.ProductType = (int)AppEnum.SOItemType.ForSale;

                //VIP客户，单品价格增加5%
                if (oSession.sCustomer.CustomerType == (int)AppEnum.CustomerType.VIP)
                {
                    soItem.Price += soItem.Price * 0.05m;
                    soItem.OrderPrice += soItem.OrderPrice * 0.05m;
                }

                soInfo.ItemHash.Add(soItem.ProductSysNo, soItem);
            }
            //处理赠品
            //------------------------------------------------------------------2008-08-18--------------------------------------------------------------------
            if (oSession.GiftHash.Count != 0)
            {
                sysNoHash.Clear();
                foreach (int sysNo in oSession.GiftHash.Keys)
                {
                    sysNoHash.Add(sysNo, null);
                }
                Hashtable saleGiftHash = SaleManager.GetInstance().GetSaleGiftHash(sysNoHash);
                if (saleGiftHash.Count != 0)
                {
                    Hashtable giftHash = new Hashtable(2);
                    foreach (SaleGiftInfo sgInfo in saleGiftHash.Values)
                    {
                        //设置主商品的赠品编号
                        ((SOItemInfo)soInfo.ItemHash[sgInfo.ProductSysNo]).GiftSysNo = sgInfo.GiftSysNo;
                        //记录赠品种类和数量

                        if (!giftHash.ContainsKey(sgInfo.GiftSysNo))
                            giftHash.Add(sgInfo.GiftSysNo, ((SOItemInfo)soInfo.ItemHash[sgInfo.ProductSysNo]).Quantity);
                        else
                        {
                            int qty = (int)giftHash[sgInfo.GiftSysNo];
                            qty += ((SOItemInfo)soInfo.ItemHash[sgInfo.ProductSysNo]).Quantity;
                            giftHash[sgInfo.GiftSysNo] = qty;
                        }
                    }
                    soProductHash.Clear();
                    soProductHash = SaleManager.GetInstance().LoadSOProducts(giftHash, (int)AppEnum.SOItemType.Gift);
                    //加入赠品
                    foreach (SOItemInfo giftItem in soProductHash.Values)
                    {
                        giftItem.Quantity = (int)giftHash[giftItem.ProductSysNo];
                        giftItem.ExpectQty = (int)giftHash[giftItem.ProductSysNo];
                        giftItem.Price = 0m;
                        giftItem.DiscountAmt = 0m;
                        giftItem.ProductType = (int)AppEnum.SOItemType.Gift;
                        soInfo.ItemHash.Add(giftItem.ProductSysNo, giftItem);
                    }
                }
            }
            //---------------------------------------------------------------------------------------------------------------------------------------------------
        }

        private void BindSO()
        {
            try
            {
                this.BindItemGrid();
                this.BindSaleRuleGrid();
                this.BindMasterInfo();

                this.BindShipTypeGrid();

                this.BindPayTypeGrid();
            }
            catch (BizException be)
            {
                this.ShowMsg(be.Message);
            }
            catch (Exception ex)
            {
                ErrorLog.GetInstance().Write(ex.Message);
            }
        }

        private void BindMasterInfo()
        {
            //收货人信息
            lblName.Text = soInfo.ReceiveName;
            lblContact.Text = soInfo.ReceiveContact;
            lblCellPhone.Text = soInfo.ReceiveCellPhone;
            lblPhone.Text = soInfo.ReceivePhone;
            lblAddress.Text = soInfo.ReceiveAddress;
            lblZip.Text = soInfo.ReceiveZip;
            AreaInfo ai = ASPManager.GetInstance().LoadArea(soInfo.ReceiveAreaSysNo);
            lblAreaName.Text = ai.GetWholeName();

            //trDeliveryTime.Visible = false;
            //int ShipTypeSysNo = soInfo.ShipTypeSysNo;
            //if (ShipTypeSysNo == 1) //如果其他快递方式也能支持上下午送到，再去掉判断条件
            //{
            //    int AreaSysNo = soInfo.ReceiveAreaSysNo;
            //    if (ASPManager.IsAreaTimeliness(ShipTypeSysNo, AreaSysNo))
            //    {
            //        trDeliveryTime.Visible = true;
            //    }
            //}


            //decimal[] i ={soInfo.CashPay,
            // soInfo.PayPrice,
            // soInfo.ShipPrice,
            // soInfo.FreeShipFeePay,
            // soInfo.PremiumAmt,
            // soInfo.VATEMSFee};   

            //-----------------------------------//
            if (soInfo.FreeShipFeePay == AppConst.IntNull)
                soInfo.FreeShipFeePay = 0;

            if (soInfo.CouponAmt == AppConst.IntNull)
                soInfo.CouponAmt = 0;
            //-----------------------------------//

            //订单金额
            this.txtPointPay.Text = soInfo.PointPay.ToString();
            this.lblPayPrice.Text = soInfo.PayPrice.ToString(AppConst.DecimalFormatWithCurrency);
            this.lblShipPrice.Text = soInfo.ShipPrice.ToString(AppConst.DecimalFormatWithCurrency);
            this.lblPremiumAmt.Text = soInfo.PremiumAmt.ToString(AppConst.DecimalFormatWithCurrency);
            //this.lblSaleRuleTotal.Text = soInfo.DiscountAmt.ToString(AppConst.DecimalFormatWithCurrency);



            this.lblSaleRuleTotal.Text = Convert.ToDecimal(soInfo.DiscountAmt + soInfo.CouponAmt).ToString(AppConst.DecimalFormatWithCurrency);

            this.lblCashPay.Text = soInfo.CashPay.ToString(AppConst.DecimalFormatWithCurrency);
            decimal total = soInfo.GetTotalAmt();
            decimal subTotal = total - soInfo.PayPrice;
            decimal trucTotal = SaleManager.GetInstance().GetEndMoney(soInfo);
            decimal change = total - trucTotal;
            this.lblSubSum.Text = subTotal.ToString(AppConst.DecimalFormatWithCurrency);
            this.lblTotalMoney.Text = trucTotal.ToString(AppConst.DecimalFormatWithCurrency);
            this.lblChange.Text = change.ToString(AppConst.DecimalFormatWithCurrency);
            this.lblPointAmt.Text = soInfo.PointAmt.ToString();
            this.lblSOWeight.Text = soInfo.GetTotalWeight().ToString();
            //手续费大于0显示
            if (soInfo.PayPrice > 0)
                this.trHandlePrice.Visible = true;
            else
                this.trHandlePrice.Visible = false;
            //订单折扣大于0显示
            //if (soInfo.DiscountAmt > 0)
            //    this.trSaleRuleTotal.Visible = true;
            //else
            //    this.trSaleRuleTotal.Visible = false;
            if ((soInfo.DiscountAmt + soInfo.CouponAmt) > 0)
                this.trSaleRuleTotal.Visible = true;
            else
                this.trSaleRuleTotal.Visible = false;

            //报价措施
            if (soInfo.ShipTypeSysNo != AppConst.IntNull)
            {
                ShipTypeInfo stInfo = ASPManager.GetInstance().LoadShipType(soInfo.ShipTypeSysNo);
                this.lblShipTypeName.Text = stInfo.ShipTypeName;
            }
            if (soInfo.PayTypeSysNo != AppConst.IntNull)
            {
                PayTypeInfo ptInfo = ASPManager.GetInstance().LoadPayType(soInfo.PayTypeSysNo);
                this.lblPayTypeName.Text = ptInfo.PayTypeName;
            }

            //积分支付范围
            int minPoint = SaleManager.GetInstance().GetMinPoint(soInfo);
            int maxPoint = SaleManager.GetInstance().GetMaxPoint(soInfo);
            int validScore = oSession.sCustomer.ValidScore;
            if (validScore == 0)
            {
                trPointPay.Visible = false;
            }
            else
            {
                trPointPay.Visible = true;
                if (validScore < maxPoint)
                    maxPoint = validScore;
                this.lblThisPoint.Text = "您当前有" + oSession.sCustomer.ValidScore + "积分，本单的积分支付范围：" + minPoint + "～" + maxPoint + "， 请输入您本次想支付的积分：";
            }

            CustomerInfo cInfo = CustomerManager.GetInstance().Load(soInfo.CustomerSysNo);
            if ((cInfo.CustomerType == (int)AppEnum.CustomerType.Enterprice && soInfo.SOAmt >= 2000) || cInfo.CustomerType == (int)AppEnum.CustomerType.VIP || soInfo.SOAmt < 2000)
            {
                vatnote.Visible = false;
                dgItem.Columns[dgItem.Columns.Count - 1].Visible = false;
            }
            else
            {
                vatnote.Visible = true;
            }
        }

        private void BindItemGrid()
        {
            DataTable dt = new DataTable("SOItemTable");
            dt.Columns.Add(new DataColumn("ProductSysNo", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("ProductName", typeof(System.String)));
            dt.Columns.Add(new DataColumn("PriceShow", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Weight", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("Quantity", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("LineTotal", typeof(System.String)));
            dt.Columns.Add(new DataColumn("LineWeight", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("InStock", typeof(System.String)));
            dt.Columns.Add(new DataColumn("DeliveryTimeliness", typeof(System.String)));
            dt.Columns.Add(new DataColumn("ExpectQty", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("IsCanVat", typeof(System.String)));

            bool HasExpectQty = false;
            bool HasNotCanVat = false;

            if (soInfo.ItemHash.Count != 0)
            {
                Hashtable sysNoHash = new Hashtable(5);
                foreach (SOItemInfo soItem in soInfo.ItemHash.Values)
                {
                    DataRow dr = dt.NewRow();


                    dr["ProductSysNo"] = soItem.ProductSysNo;
                    if (soItem.PointType == (int)AppEnum.ProductPayType.PointPayOnly)
                    {
                        dr["PriceShow"] = Convert.ToInt32(soItem.Price * AppConst.ExchangeRate).ToString() + "积分（仅积分支付）";
                        dr["LineTotal"] = Convert.ToInt32(soItem.Price * AppConst.ExchangeRate * soItem.Quantity).ToString() + "积分";
                    }
                    else
                    {
                        dr["PriceShow"] = soItem.Price.ToString(AppConst.DecimalFormatWithCurrency);
                        dr["LineTotal"] = ((decimal)(soItem.Price * soItem.Quantity)).ToString(AppConst.DecimalFormatWithCurrency);
                    }
                    dr["Weight"] = soItem.Weight;
                    dr["Quantity"] = soItem.Quantity;
                    dr["LineWeight"] = soItem.Quantity * soItem.Weight;
                    dr["ExpectQty"] = soItem.ExpectQty;
                    dr["IsCanVat"] = soItem.IsCanVat == 0 ? "<font color=red><strong>否</strong></font>" : "是";
                    dt.Rows.Add(dr);
                    sysNoHash.Add(dr["ProductSysNo"], null);

                    if (soItem.ExpectQty > soItem.Quantity)
                        HasExpectQty = true;
                    else
                        dr["ExpectQty"] = dr["Quantity"];

                    if (soItem.IsCanVat == (int)AppEnum.YNStatus.No)
                        HasNotCanVat = true;
                }
                DataSet dsPB = ProductManager.GetInstance().GetProductBoundleWithInventory(sysNoHash);
                foreach (DataRow dr in dt.Rows)
                {
                    foreach (DataRow drPB in dsPB.Tables[0].Rows)
                    {
                        if ((int)dr["ProductSysNo"] == (int)drPB["SysNo"])
                        {
                            dr["ProductName"] = drPB["ProductName"] + " " + drPB["size2name"];
                            if ((int)dr["Quantity"] <= (int)drPB["OnlineQty"])
                                dr["InStock"] = "有库存";
                            else
                                dr["InStock"] = "<font color=red>库存不足</font>";

                            if ((int)dr["Quantity"] <= (int)drPB["AvailableQty"])
                                dr["DeliveryTimeliness"] = "1日内";
                            else
                            {
                                if (Util.TrimIntNull(drPB["VirtualArriveTime"]) != AppConst.IntNull)
                                {
                                    dr["DeliveryTimeliness"] = AppEnum.GetVirtualArriveTime(Util.TrimIntNull(drPB["VirtualArriveTime"])) + "内";
                                }
                                else
                                {
                                    dr["DeliveryTimeliness"] = AppEnum.GetVirtualArriveTime((int)AppEnum.VirtualArriveTime.OneToThree) + "内";
                                }
                                //HasEnoughAvailableQty = false;
                            }

                            break;
                        }
                    }
                }
            }
            this.dgItem.DataSource = dt;
            this.dgItem.DataBind();

            if (HasExpectQty)
                dgItem.Columns[5].Visible = true;
            else
                dgItem.Columns[5].Visible = false;

            chkInvoice.Attributes.Add("onclick", "checkBoxChecked(0);");
            if (HasNotCanVat)
                chkInvoice.Attributes.Add("onclick", "checkBoxChecked(1);");
        }

        private void BindSaleRuleGrid()
        {
            DataTable dt = new DataTable("SaleRuleTable");
            dt.Columns.Add(new DataColumn("SaleRuleName", typeof(System.String)));
            dt.Columns.Add(new DataColumn("DiscountAmt", typeof(System.String)));
            dt.Columns.Add(new DataColumn("DiscountTime", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("SubTotal", typeof(System.String)));
            if (soInfo.SaleRuleHash.Count != 0)
            {
                foreach (SOSaleRuleInfo srInfo in soInfo.SaleRuleHash.Values)
                {
                    DataRow dr = dt.NewRow();
                    dr["SaleRuleName"] = srInfo.SaleRuleName;
                    dr["DiscountAmt"] = srInfo.Discount.ToString(AppConst.DecimalFormatWithCurrency);
                    dr["DiscountTime"] = srInfo.Times;
                    dr["SubTotal"] = ((decimal)(srInfo.Discount * srInfo.Times)).ToString(AppConst.DecimalFormatWithCurrency);
                    dt.Rows.Add(dr);
                }
            }
            this.dgSaleRule.DataSource = dt;
            this.dgSaleRule.DataBind();
            if (dt.Rows.Count == 0)
                this.trSRData.Visible = false;
            else
                this.trSRData.Visible = true;
        }

        /// <summary>
        /// 信息提示
        /// </summary>
        /// <param name="msg"></param>
        private void ShowMsg(string msg)
        {

            this.lblmsg.Text = msg;

            //去除HTML标签只保留文本信息
            msg = System.Text.RegularExpressions.Regex.Replace(msg, @"<(?!/?p(\s|>)+)[^>]*?>", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            Page.ClientScript.RegisterStartupScript(GetType(), null, "alert('" + msg + "');", true);
        }

        /// <summary>
        /// 确认无误提交订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckLimitedQtyProduct())
                    return;
                bool saveSOInfo = this.SaveSO();
                if (saveSOInfo == false)
                    return;
                soInfo.SalesManSysNo = AppConst.IcsonSalesMan;
                ////记录浩方订单
                //if (Session["LinkSource"] != null && Session["LinkSource"] as string == AppConfig.PartnerLinkSource)
                //{
                //    soInfo.SalesManSysNo = int.Parse(AppConfig.PartnerSysNo);
                //}
                //检验客户一分钟内是否有订单生成，防止重复下订单
                if (!SaleManager.GetInstance().SOCreatePreCheck(soInfo.CustomerSysNo))
                {
                    this.ShowMsg("请不要在1分钟之内重复下单。请到帐户中心查看您的订单");
                    return;
                }

                if (chkInvoice.Checked == true)
                {
                    if (chkInvoice.Checked == true)
                        vatInfo.Style.Add("display", "block");
                    else
                        vatInfo.Style.Add("display", "none");
                    if (txtCompanyName.Text.Trim() == AppConst.StringNull)
                    {
                        this.ShowMsg("开票公司名称不能为空！");
                        return;
                    }
                    if (txtCompanyAddr.Text.Trim() == AppConst.StringNull)
                    {
                        this.ShowMsg("开票公司地址不能为空！");
                        return;
                    }
                    if (txtCompanyPhone.Text.Trim() == AppConst.StringNull)
                    {
                        this.ShowMsg("开票公司电话不能为空！");
                        return;
                    }
                    if (txtBankAndAccount.Text.Trim() == AppConst.StringNull)
                    {
                        this.ShowMsg("开票开户银行及帐号不能为空！");
                        return;
                    }
                    if (txtTaxNum.Text.Trim() == AppConst.StringNull)
                    {
                        this.ShowMsg("开票公司税务登记号不能为空！");
                        return;
                    }



                    soInfo.IsVAT = (int)AppEnum.YNStatus.Yes;
                    soInfo.RequestInvoiceTime = DateTime.Now;
                    soInfo.RequestInvoiceType = (int)AppEnum.InvoiceType.SpecialVATInvoice;
                    soInfo.InvoiceStatus = (int)AppEnum.SOInvoiceStatus.InvoiceAbsent;
                }

                #region
                bool payTapyCheck = false;
                bool shipTypeCheck = false;

                foreach (DataGridItem dgItem in this.dgShipType.Items)
                {
                    RadioButton rdo = dgItem.FindControl("rdoSelectShipType") as RadioButton;
                    if (rdo.Checked)
                    {
                        soInfo.ShipTypeSysNo = Int32.Parse(dgShipType.DataKeys[dgItem.ItemIndex].ToString());
                        shipTypeCheck = true;
                        break;
                    }
                }
                if (chkIsPremium.Checked)
                {
                    soInfo.IsPremium = (int)AppEnum.YNStatus.Yes;
                }
                else
                    soInfo.IsPremium = (int)AppEnum.YNStatus.No;

                if (!shipTypeCheck)
                {
                    ShowMsg("您还没有选择配送方式");
                    return;
                }

                foreach (DataGridItem dgItem in this.dgPayType.Items)
                {
                    RadioButton rdo = dgItem.FindControl("rdoSelectPayType") as RadioButton;
                    if (rdo.Checked)
                    {
                        soInfo.PayTypeSysNo = Int32.Parse(dgPayType.DataKeys[dgItem.ItemIndex].ToString());
                        payTapyCheck = true;
                        break;
                    }
                }
                if (!payTapyCheck)
                {
                    this.ShowMsg("您还没有选择支付方式");
                    return;
                }


                soInfo.IsPremium = (int)AppEnum.YNStatus.No;
                soInfo.IsVAT = (int)AppEnum.YNStatus.No;
                soInfo.IsWholeSale = (int)AppEnum.YNStatus.No;
                soInfo.VATEMSFee = 0;
                soInfo.VatInfo.VATEMSFee = 0;
                soInfo.FreeShipFeePay = 0;




                #endregion

                soInfo.DeliveryDate = DateTime.Today;
                oSession.sSO = soInfo;
                SaleManager.GetInstance().CreateSO(soInfo);



                //if (soInfo.CouponCode.Length > 0 && soInfo.CouponAmt > 0 && soInfo.CouponType == 1) //宽带山
                //{
                //    try
                //    {
                //        string url = "http://club.pchome.net/union/icson/checkHashCode.php?action=1&code=" + soInfo.CouponCode;
                //        System.Net.WebClient wc = new System.Net.WebClient();
                //        System.IO.Stream stream = wc.OpenRead(url);
                //        wc.Dispose();
                //        stream.Close();
                //    }
                //    catch { }
                //}

                oSession.sCustomer = CustomerManager.GetInstance().Load(oSession.sCustomer.SysNo);
                try
                {
                    LogInfo log = new LogInfo();
                    log.OptIP = Request.UserHostAddress;
                    log.OptTime = DateTime.Now;
                    log.TicketSysNo = soInfo.SysNo;
                    log.TicketType = (int)AppEnum.LogType.Sale_SO_Create;
                    log.OptUserSysNo = soInfo.SalesManSysNo;
                    LogManager.GetInstance().Write(log);
                    SaleManager.GetInstance().SendEmail(soInfo, (int)AppEnum.SOEmailType.CreateSO);
                }
                catch (Exception ex)
                {
                    ErrorLog.GetInstance().Write(ex.Message);
                }
                //更新增票信息
                if (chkInvoice.Checked == true)
                {
                    try
                    {
                        SOVATInfo vat = new SOVATInfo();
                        vat.SOSysNo = soInfo.SysNo;
                        vat.BankAccount = txtBankAndAccount.Text.Trim();
                        vat.TaxNum = txtTaxNum.Text.Trim();
                        vat.CompanyAddress = txtCompanyAddr.Text.Trim();
                        vat.CompanyName = txtCompanyName.Text.Trim();
                        vat.CompanyPhone = txtCompanyPhone.Text.Trim();
                        vat.CustomerSysNo = oSession.sCustomer.SysNo;
                        vat.CreateTime = DateTime.Now;
                        vat.Memo = txtOthers.Text.Trim();
                        SaleManager.GetInstance().InsertSOVAT(vat);
                    }
                    catch
                    {
                        this.ShowMsg("增票信息更新失败，请联系MMMbuy.cn ");
                    }
                }

                //订单生成成功，清除购物车以及赠品信息
                CartManager.GetInstance().ClearCart();
                oSession.GiftHash.Clear();
                //转到支付页面
                PayTypeInfo ptInfo = ASPManager.GetInstance().LoadPayType(soInfo.PayTypeSysNo);
                if (ptInfo.PaymentPage != string.Empty)
                {
                    //Response.Redirect(ptInfo.PaymentPage + "?id=" + soInfo.SysNo.ToString() + "&sono=" + soInfo.SOID + "&soamt=" + SaleManager.GetInstance().GetEndMoney(soInfo).ToString(AppConst.DecimalFormat) + "&sodate=" + soInfo.OrderDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    try
                    {
                        //if (Request.Cookies["EqifaInfo"] != null && Request.Cookies["EqifaInfo"].Value != null && Request.Cookies["EqifaInfo"].Value.Length > 0)
                        //{
                        //    string merchant_id = "icson"; //广告主在EQIFA的帐号名
                        //    string eqifa_user_id = oSession.sCustomer.CustomerID; //会员帐号名称
                        //    string eqifa_user_name = oSession.sCustomer.CustomerName; //会员真实姓名
                        //    string eqifa_o_cd = soInfo.SysNo.ToString(); //用户订单号

                        //    string eqifa_p_cd = "";                        //一种商品编号
                        //    string eqifa_p_cnt = "1";			           //一种商品数量
                        //    string eqifa_p_price = soInfo.SOAmt.ToString();//一种商品价格
                        //    string eqifa_p_ccd = "";	                   //一种商品分类编号

                        //    string eqifa_info = Request.Cookies["EqifaInfo"].Value;
                        //    eqifa_info = Server.UrlDecode(eqifa_info);

                        //    if (eqifa_info.Replace("'", "''").Length != 0)
                        //    {
                        //        string eqifa_log = "http://service.eqifa.com/purchase_cps.aspx?a_id=" + eqifa_info;
                        //        eqifa_log += "&m_id=" + merchant_id + "&mbr_id=" + eqifa_user_id + "(" + eqifa_user_name + ")";
                        //        eqifa_log += "&o_cd=" + eqifa_o_cd + "&p_cd=" + eqifa_p_cd;
                        //        eqifa_log += "&p_cnt=" + eqifa_p_cnt + "&p_price=" + eqifa_p_price + "&p_ccd=" + eqifa_p_ccd;
                        //        eqifa_log = "<script src='" + eqifa_log + "'></script>";

                        //        //Response.Write(eqifa_log);
                        //        //Response.Write("<script src='http://icson-mis.3322.org/Temp.aspx?id=" + soInfo.SOID + "'></script>");

                        //        EqifaLogInfo eInfo = new EqifaLogInfo();
                        //        eInfo.SOSysNo = soInfo.SysNo;
                        //        eInfo.ProductSysNo = 1;
                        //        eInfo.Quantity = 1;
                        //        eInfo.Price = soInfo.SOAmt;
                        //        eInfo.EqifaLog = eqifa_log;

                        //        EqifaLogManager.GetInstance().Insert(eInfo);
                        //    }
                        //}
                        if (Request.Cookies["LSInfo"] != null && Request.Cookies["LSInfo"].Value != null && Request.Cookies["LSInfo"].Value.Length > 0)
                        {
                            EqifaLogInfo eInfo = new EqifaLogInfo();
                            eInfo.SOSysNo = soInfo.SysNo;
                            eInfo.ProductSysNo = 1;
                            eInfo.Quantity = 1;
                            eInfo.Price = soInfo.SOAmt;
                            eInfo.EqifaLog = Request.Cookies["LSInfo"].Value;

                            EqifaLogManager.GetInstance().Insert(eInfo);
                        }
                        else if (Session["LinkSource"] != null && Session["LinkSource"].ToString().Length > 0 && Session["LinkSource"].ToString().Trim().IndexOf("www.ICSON.com") == -1)
                        {
                            EqifaLogInfo eInfo = new EqifaLogInfo();
                            eInfo.SOSysNo = soInfo.SysNo;
                            eInfo.ProductSysNo = 1;
                            eInfo.Quantity = 1;
                            eInfo.Price = soInfo.SOAmt;
                            eInfo.EqifaLog = Session["LinkSource"].ToString().Trim();

                            EqifaLogManager.GetInstance().Insert(eInfo);
                        }
                    }
                    finally
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", "<script>location.href='" + ptInfo.PaymentPage + "?id=" + soInfo.SysNo.ToString() + "&sono=" + soInfo.SOID + "&soamt=" + SaleManager.GetInstance().GetEndMoney(soInfo).ToString(AppConst.DecimalFormat) + "&sodate=" + soInfo.OrderDate.ToString("yyyy-MM-dd HH:mm:ss") + "'</script>");
                    }
                }
                else
                {
                    Response.Redirect("../CustomError.aspx?msg=页面转向时发生错误！但是订单已经成功生成，订单号是：" + soInfo.SOID + "！");
                }
            }
            catch (BizException be)
            {
                if (be.Message.IndexOf("inventory: qty is not enough") != -1)
                {
                    this.ShowMsg("库存不足，请返回购物车重试");
                }
                else if (be.Message.IndexOf("inventory_stock: qty is not enough") != -1)
                {
                    this.ShowMsg("库存不足，请返回购物车重试");
                }
                else if (be.Message.IndexOf("客户积分更新失败") != -1)
                {
                    this.ShowMsg("您的积分不足，不能支付");
                }
                else
                {
                    this.ShowMsg(be.Message);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.GetInstance().Write("订单生成失败:" + ex.Message);
                this.ShowMsg("订单生成失败，请重试或者电话联系MMMbuy.cn ");
            }
        }

        private bool SaveSO()
        {
            //soInfo.ExpectDeliveryDate = ucSelectDeliveryTime.DeliveryDate;
            //soInfo.ExpectDeliveryTimeSpan = ucSelectDeliveryTime.TimeSpan;
            soInfo.Memo = this.txtMemo.Text.Trim();
            soInfo.SignByOther = Int32.Parse(rblSignByOther.SelectedValue);
            if (trIsMergeSO.Visible && Int32.Parse(rblIsMergeSO.SelectedValue) == (int)AppEnum.YNStatus.Yes)
                soInfo.IsMergeSO = (int)AppEnum.YNStatus.Yes;
            else
                soInfo.IsMergeSO = (int)AppEnum.YNStatus.No;

            if (this.txtPointPay.Text.Trim() != string.Empty)
                soInfo.PointPay = int.Parse(this.txtPointPay.Text.Trim());
            else
                soInfo.PointPay = 0;
            if (soInfo.PointPay > oSession.sCustomer.ValidScore)
            {
                this.ShowMsg("您没有足够的有效积分,请重新输入积分");
                return false;
            }
            //if (soInfo.PointPay < SaleManager.GetInstance().GetMinPoint(soInfo) || soInfo.PointPay > SaleManager.GetInstance().GetMaxPoint(soInfo))
            //{
            //    this.ShowMsg("您支付的积分不在本单积分支付范围中,请重新输入积分");
            //    return false;
            //}

            SaleManager.GetInstance().CalcSO(soInfo);
            return true;
        }

        private bool CheckLimitedQtyProduct()
        {
            DataTable dt = new DataTable("SOItemTable");
            dt.Columns.Add(new DataColumn("ProductSysNo", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("ProductName", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Quantity", typeof(System.Int32)));

            if (soInfo.ItemHash.Count != 0)
            {
                Hashtable sysNoHash = new Hashtable(5);
                string productList = "";
                foreach (SOItemInfo soItem in soInfo.ItemHash.Values)
                {
                    DataRow dr = dt.NewRow();
                    dr["ProductSysNo"] = soItem.ProductSysNo;
                    dr["Quantity"] = soItem.Quantity;
                    dt.Rows.Add(dr);
                    sysNoHash.Add(dr["ProductSysNo"], null);
                    productList += dr["ProductSysNo"].ToString() + ",";
                }
                DataSet dsPB = ProductManager.GetInstance().GetProductBoundleWithInventoryPrice(sysNoHash);
                foreach (DataRow dr in dt.Rows)
                {
                    foreach (DataRow drPB in dsPB.Tables[0].Rows)
                    {
                        if ((int)dr["ProductSysNo"] == (int)drPB["SysNo"])
                        {
                            if (Util.TrimIntNull(dr["Quantity"].ToString()) > Util.TrimIntNull(drPB["onlineqty"]))
                            {
                                this.ShowMsg(Util.TrimNull(drPB["ProductName"]) + " 库存不足，请返回购物车！");
                                return false;
                            }

                            int LimitedQty = Util.TrimIntNull(drPB["LimitedQty"].ToString());
                            if (LimitedQty > 0 && LimitedQty < 999)
                            {
                                if (Util.TrimIntNull(dr["Quantity"].ToString()) > LimitedQty)
                                {
                                    string msg = Util.TrimNull(drPB["productname"]) + "，每天限购一次，每次限购" +
                                                 Util.TrimIntNull(drPB["limitedqty"].ToString()) +
                                                 "个<br />请回到 <a href='ShoppingCart.aspx'>购物车</a> 修改限购商品！";
                                    this.ShowMsg(msg);
                                    btnOK.Enabled = false;
                                    return false;
                                }
                            }
                            break;
                        }
                    }
                }

                //判断限购商品
                productList = productList.Substring(0, productList.Length - 1);
                string sCheckLimitedQtyProduct = SaleManager.GetInstance().SOCheckLimitedQtyProduct(soInfo.CustomerSysNo, productList);
                if (sCheckLimitedQtyProduct.Length > 0)
                {
                    this.ShowMsg(sCheckLimitedQtyProduct);
                    btnOK.Enabled = false;
                    return false;
                }
            }

            return true;
        }

        protected void txtSubmitAdways_Click(object sender, EventArgs e)
        {
            //string result = VerifyAdwaysAccount(oSession.sCustomer.CustomerID, adwaysID, adwaysEmail);
            //if (result.Equals("0"))
            //{
            //    this.ShowMsg("您提供的易价网用户尚未注册，请注册后再使用！");
            //    return;
            //}
            //else if (result.Equals("-1"))
            //{
            //    this.ShowMsg("易价网通讯失败，但不影响您完成此次订单！");
            //    return;
            //}
        }

        private string VerifyAdwaysAccount(string CustomerID, string AdwaysID, string AdwaysEmail)
        {
            try
            {
                string url = "http://club.pchome.net/union/icson/checkHashCode.php?code=" + txtPPCode.Text.Trim();
                System.Net.WebClient wc = new System.Net.WebClient();
                System.IO.Stream stream = wc.OpenRead(url);
                System.Xml.XmlReader xmlread = new System.Xml.XmlTextReader(stream);

                string ppResult = stream.ToString();

                stream.Close();
                wc.Dispose();

                while (xmlread.Read())
                {
                    if (xmlread.Name.ToUpper().Equals("RESULT"))
                    {
                        break;
                    }
                }

                string result = xmlread.GetAttribute(0).ToString();
                xmlread.Close();
                return result;
            }
            catch
            {
                return "-1";
            }
        }

        protected void btnSubmitPP_Click(object sender, EventArgs e)
        {
            try
            {
                string url = "http://club.pchome.net/union/icson/checkHashCode.php?action=0&code=" + txtPPCode.Text.Trim();
                System.Net.WebClient wc = new System.Net.WebClient();
                System.IO.Stream stream = wc.OpenRead(url);

                System.IO.StreamReader sr = new System.IO.StreamReader(stream);

                string ppResult = sr.ReadToEnd().Trim();

                //* 返回错误代码 * -1 : 没有参数或参数不正确 * -2 ： 指定优惠券不存在 * -3 : 指定优惠券已失效 * -4 : 指定优惠券已被使用 * -5 : 数据库错误

                if (ppResult.Length >= 2)
                {
                    string errCode = ppResult.Substring(0, 2);
                    if (errCode == "-1")
                    {
                        lblmsg.Text = "没有参数或参数不正确，但不影响您完成此次订单！";
                    }
                    else if (errCode == "-2")
                    {
                        lblmsg.Text = "指定优惠券不存在，但不影响您完成此次订单！";
                    }
                    else if (errCode == "-3")
                    {
                        lblmsg.Text = "指定优惠券已失效，但不影响您完成此次订单！";
                    }
                    else if (errCode == "-4")
                    {
                        lblmsg.Text = "指定优惠券已被使用，但不影响您完成此次订单！";
                    }
                    else if (errCode == "-5")
                    {
                        lblmsg.Text = "数据库错误，但不影响您完成此次订单！";
                    }
                    else
                    {
                        string[] result = ppResult.Split('\n');
                        string pp = result[0].Trim();
                        string time = result[1].Trim();
                        string mac = result[2].Trim();

                        string key = "";
                        string md5 = Util.GetMD5(time + pp + key).ToUpper();

                        if (md5 == mac.ToUpper())
                        {
                            //有效PP值：pp\n发送时间\n校验码 
                            this.soInfo.CouponCode = this.txtPPCode.Text.Trim();  //优惠券代码
                            this.soInfo.CouponType = 1; //PP合作为1
                            decimal dPP = decimal.Parse(pp);
                            if (dPP > 10)
                                dPP = 10;

                            if (this.soInfo.SOAmt / 100 + 1 >= Int32.Parse(pp))
                            {
                                this.soInfo.CouponAmt = dPP * 2;
                            }
                            else
                            {
                                this.soInfo.CouponAmt = decimal.Parse(Convert.ToString(Util.DecimalToU_Int32(soInfo.SOAmt) / 100 * 2 + 2));
                            }

                            SaleManager.GetInstance().CalcSO(soInfo);
                            this.BindSO();
                        }
                        else
                        {
                            lblmsg.Text = "校验错误，但不影响您完成此次订单！";
                        }
                    }
                }

                stream.Close();
                wc.Dispose();
                //lblmsg.Text = ppResult;
            }
            catch
            {
                lblmsg.Text = "和宽带山通讯失败，但不影响您完成此次订单！";
            }
        }

        protected void dgItem_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            //Icson.Utils.DataGridUtil.GetInstance().setgridstyle(dgItem, e);
        }

        #region 配送方式与支付方式

        private void GetLastOneSo()
        {

            DataSet ds = new DataSet();

            ds = SaleManager.GetInstance().GetSOOnlineDs(oSession.sCustomer.SysNo, 1);

            if (Util.HasMoreRow(ds))
            {
                if (ds.Tables[0].Rows.Count == 1)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    int soSysNo = int.Parse(dr["sysno"].ToString());

                    oldSoInfo = SaleManager.GetInstance().LoadSOMaster(soSysNo);

                    shipTypeInfo = (ShipTypeInfo)ASPManager.GetInstance().GetShipTypeHash()[oldSoInfo.ShipTypeSysNo];

                    payTypeInfo = (PayTypeInfo)ASPManager.GetInstance().GetPayTypeHash()[oldSoInfo.PayTypeSysNo];

                }
                else
                {
                    Response.Redirect("ShoppingCart.aspx");
                }
            }

        }

        /// <summary>
        /// 支付方式列表绑定
        /// </summary>
        private void BindPayTypeGrid()
        {
            DataTable dt = new DataTable("PayTypeTable");
            dt.Columns.Add(new DataColumn("SysNo", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("PayTypeName", typeof(System.String)));
            dt.Columns.Add(new DataColumn("PayTypeDesc", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Period", typeof(System.String)));
            dt.Columns.Add(new DataColumn("OrderNumber", typeof(System.String)));

            SortedList payTypeList = ASPManager.GetInstance().GetPayTypeListByShipType(soInfo.ShipTypeSysNo);
            if (payTypeList != null && payTypeList.Count != 0)
            {
                foreach (PayTypeInfo ptInfo in payTypeList.Keys)
                {
                    if (ptInfo.IsOnlineShow == (int)AppEnum.YNStatus.Yes)
                    {
                        DataRow dr = dt.NewRow();
                        dr["SysNo"] = ptInfo.SysNo;
                        dr["PayTypeName"] = ptInfo.PayTypeName;
                        dr["PayTypeDesc"] = ptInfo.PayTypeDesc;
                        dr["Period"] = ptInfo.Period;
                        dr["OrderNumber"] = ptInfo.OrderNumber;
                        dt.Rows.Add(dr);
                    }
                }
            }
            //淘宝代理,增加支付宝免手续费的方式
            if (oSession.sCustomer.CustomerType == (int)AppEnum.CustomerType.Taobao)
            {
                PayTypeInfo ptInfo = ASPManager.GetInstance().GetPayTypeHash()[9] as PayTypeInfo;
                DataRow dr = dt.NewRow();
                dr["SysNo"] = ptInfo.SysNo;
                dr["PayTypeName"] = ptInfo.PayTypeName;
                dr["PayTypeDesc"] = ptInfo.PayTypeDesc;
                dr["Period"] = ptInfo.Period;
                dr["OrderNumber"] = ptInfo.OrderNumber;
                dt.Rows.Add(dr);
            }
            this.dgPayType.DataSource = dt;
            this.dgPayType.DataBind();

            //bool check = true;

            if (dgPayType.Items.Count != 0)
            {
                foreach (DataGridItem dgItem in dgPayType.Items)
                {
                    RadioButton rdo = dgItem.FindControl("rdoSelectPayType") as RadioButton;
                    int PayTypeSysNo = Int32.Parse(dgPayType.DataKeys[dgItem.ItemIndex].ToString());
                    if (PayTypeSysNo == 8) //财付通账号输入
                    {
                        rdo.Attributes.Add("onclick", "SelectPayType(this,1);");
                    }
                    else
                    {
                        rdo.Attributes.Add("onclick", "SelectPayType(this,0);");
                    }

                    ////货到付款方式按钮初始设置
                    //if (PayTypeSysNo == 1)
                    //{
                    //    foreach (DataGridItem shipItem in this.dgShipType.Items)
                    //    {
                    //        RadioButton rdoShip = shipItem.FindControl("rdoSelectShipType") as RadioButton;



                    //        if (rdoShip.Checked)
                    //        {

                    //            check = false;

                    //            soInfo.ShipTypeSysNo = Int32.Parse(dgShipType.DataKeys[shipItem.ItemIndex].ToString());

                    //            if (soInfo.ShipTypeSysNo == 1 || soInfo.ShipTypeSysNo == 7)
                    //            {
                    //                rdo.Enabled = true;
                    //            }
                    //            else 
                    //            {
                    //                rdo.Enabled = false;
                    //                rdo.Checked = false;
                    //            }
                    //        }
                    //    }

                    //    if(check)
                    //        rdo.Enabled=false;

                    //}

                }
            }
        }

        /// <summary>
        /// 支付方式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgPayType_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.Item.Cells[1].Text == payTypeInfo.PayTypeName)
                {
                    DataGridItem item = sender as DataGridItem;

                    RadioButton rdobtn = e.Item.FindControl("rdoSelectPayType") as RadioButton;

                    rdobtn.Checked = true;
                }

            }
        }

        /// <summary>
        /// 关于支付的
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgPayType_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                try
                {
                    soInfo = oSession.sSO;
                    soInfo.PayTypeSysNo = (int)dgPayType.DataKeys[e.Item.ItemIndex];
                    oSession.sSO = soInfo;
                }
                catch (BizException be)
                {
                    ErrorLog.GetInstance().Write(be.Message);
                }
                catch (Exception ex)
                {
                    ErrorLog.GetInstance().Write(ex.Message);
                }
            }
        }

        /// <summary>
        /// 关于配送的
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgShipType_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                try
                {
                    soInfo = oSession.sSO;
                    soInfo.ShipTypeSysNo = (int)dgShipType.DataKeys[e.Item.ItemIndex];
                    soInfo.IsPremium = (int)AppEnum.YNStatus.No;

                    oSession.sSO = soInfo;

                    //BindPayTypeGrid();
                }
                catch (BizException be)
                {
                    ErrorLog.GetInstance().Write(be.Message);
                }
                catch (Exception ex)
                {
                    ErrorLog.GetInstance().Write(ex.Message);
                }
            }
        }

        /// <summary>
        /// 行绑定选择默认配送方式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgShipType_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.Item.Cells[1].Text == shipTypeInfo.ShipTypeName)
                {
                    DataGridItem item = sender as DataGridItem;

                    RadioButton rdobtn = e.Item.FindControl("rdoSelectShipType") as RadioButton;

                    rdobtn.Checked = true;
                }
            }
        }

        /// <summary>
        /// 配送方式列表绑定
        /// </summary>
        private void BindShipTypeGrid()
        {
            DataTable dt = new DataTable("ShipTypeTable");
            dt.Columns.Add(new DataColumn("SysNo", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("ShipTypeName", typeof(System.String)));
            dt.Columns.Add(new DataColumn("ShipPrice", typeof(System.String)));
            dt.Columns.Add(new DataColumn("DiscountShipPrice", typeof(System.String)));
            dt.Columns.Add(new DataColumn("ShipTypeDesc", typeof(System.String)));
            dt.Columns.Add(new DataColumn("PremiumAmt", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Period", typeof(System.String)));
            dt.Columns.Add(new DataColumn("OrderNumber", typeof(System.String)));

            SortedList shipTypeList = ASPManager.GetInstance().GetShipTypeListByArea(soInfo.ReceiveAreaSysNo);

            AreaInfo areaInfo = ASPManager.GetInstance().LoadArea(soInfo.ReceiveAreaSysNo);

            if (shipTypeList != null && shipTypeList.Count != 0)
            {
                foreach (ShipTypeInfo stInfo in shipTypeList.Keys)
                {
                    if (stInfo.IsOnlineShow == (int)AppEnum.YNStatus.Yes)
                    {

                        try
                        {
                            #region

                            //欧州直购 仅能使用 欧州直购配送方式 作废
                            //if (ASPManager.GetInstance().IsAbroadChannel(soInfo.ItemHash))
                            //{
                            //    if (stInfo.ShipTypeID == "050")
                            //    {
                            //        if (Session["HasLimitedShipProduct"] != null)
                            //        {
                            //            if (Session["HasLimitedShipProduct"].ToString().IndexOf("1") >= 0)
                            //            {
                            //                if (stInfo.SysNo == 2 || stInfo.SysNo == 3)  //EMS，邮局普包，所有限运商品都不可以运输
                            //                {
                            //                    //lblmsg.Text = Session["HasLimitedShipProductInfo1"].ToString() + " 属邮局普包和EMS限运商品，如需使用邮局普包或EMS，请从购物车中删除限运商品！ <a href='http://www.icson.com/Service/NewsDetail.aspx?Type=Bulletin&ID=73' target='_blank'>查看限运说明</a>";
                            //                    continue;
                            //                }
                            //            }
                            //            if (Session["HasLimitedShipProduct"].ToString().IndexOf("2") >= 0) //圆通航空限运
                            //            {
                            //                if (stInfo.SysNo == 7)
                            //                {
                            //                    if (",1,201,131,403,814,1144,1323,1718,1591,2858,3225,2329,2621,".IndexOf("," + areaInfo.ProvinceSysNo.ToString() + ",") < 0)
                            //                    {
                            //                        //lblmsg.Text = Session["HasLimitedShipProductInfo2"].ToString() + " 属圆通限运商品，如需使用圆通，请从购物车中删除限运商品！ <a href='http://www.icson.com/Service/NewsDetail.aspx?Type=Bulletin&ID=73' target='_blank'>查看限运说明</a>";
                            //                        continue;
                            //                    }
                            //                }
                            //            }
                            //        }

                            //        DataRow dr = dt.NewRow();
                            //        dr["SysNo"] = stInfo.SysNo;
                            //        dr["ShipTypeName"] = stInfo.ShipTypeName;
                            //        dr["ShipTypeDesc"] = stInfo.ShipTypeDesc;
                            //        dr["Period"] = stInfo.Period;
                            //        decimal tempShipPrice = ASPManager.GetInstance().GetShipPrice(soInfo.GetTotalWeight(), soInfo.SOAmt, stInfo.SysNo, soInfo.ReceiveAreaSysNo);
                            //        dr["ShipPrice"] = tempShipPrice.ToString(AppConst.DecimalFormatWithCurrency);


                            //        //判断每天只能免除一次运费
                            //        //bool hasUserdOneDayOneFreeShipFee = SaleManager.GetInstance().HasUserdOneDayOneFreeShipFee(AppConst.IntNull,soInfo.CustomerSysNo,DateTime.Now);
                            //        //if (hasUserdOneDayOneFreeShipFee)
                            //        //{
                            //        //    dr["DiscountShipPrice"] = "￥0.00";
                            //        //    lblFreeShipFeeTip.Text = "尊敬的客户,每位客户每天只能参加一次免运费活动,请您谅解!";
                            //        //}
                            //        //else
                            //        //{
                            //        //    lblFreeShipFeeTip.Text = "";                                
                            //        //    //MMbuy商城快递配送范围内的（包括杭州，苏州，扬州，上海市区），满100元免运费，最高免50元
                            //        //    //上海郊区，订单1000元以内收费5元，满1000元免运费
                            //        //    //圆通快递，江浙沪皖地区，满200元免运费，最高免50元
                            //        //    //圆通快递，非江浙沪皖地区，不免运费
                            //        //    //EMS、顺丰快递不免运费。

                            //        //    //上海市区及分站快递自提
                            //        //    if ((stInfo.SysNo == 1 || stInfo.SysNo == 8 || stInfo.SysNo == 9 || stInfo.SysNo == 10 || stInfo.SysNo == 11 || stInfo.SysNo == 13 || stInfo.SysNo == 17) && areaInfo.LocalCode < 5 && soInfo.SOAmt >= 100)  //MMbuy商城快递(非上海郊区)、订单金额满100
                            //        //    {
                            //        //        if (tempShipPrice <= 50)
                            //        //        {
                            //        //            dr["DiscountShipPrice"] = "-" + tempShipPrice.ToString(AppConst.DecimalFormatWithCurrency);
                            //        //        }
                            //        //        else
                            //        //        {
                            //        //            dr["DiscountShipPrice"] = "-￥50.00";
                            //        //        }
                            //        //    }
                            //        //    //上海郊区MMbuy商城快递
                            //        //    else if (stInfo.SysNo == 1 && areaInfo.LocalCode > 4)
                            //        //    {
                            //        //        if (soInfo.SOAmt < 1000)
                            //        //        {
                            //        //            dr["DiscountShipPrice"] = "-" + Convert.ToDecimal(tempShipPrice - 5).ToString(AppConst.DecimalFormatWithCurrency);
                            //        //        }
                            //        //        else
                            //        //        {
                            //        //            dr["DiscountShipPrice"] = "-" + tempShipPrice.ToString(AppConst.DecimalFormatWithCurrency);
                            //        //        }
                            //        //    }
                            //        //    //江浙沪皖，圆通满200免运费
                            //        //    else if (soInfo.SOAmt >= 200 && stInfo.SysNo == 7 && (areaInfo.ProvinceSysNo == 1 || areaInfo.ProvinceSysNo == 2621 || areaInfo.ProvinceSysNo == 1591 || areaInfo.ProvinceSysNo == 3225))
                            //        //    {
                            //        //        if (tempShipPrice <= 50)
                            //        //        {
                            //        //            dr["DiscountShipPrice"] = "-" + tempShipPrice.ToString(AppConst.DecimalFormatWithCurrency);
                            //        //        }
                            //        //        else
                            //        //        {
                            //        //            dr["DiscountShipPrice"] = "-￥50.00";
                            //        //        }
                            //        //    }
                            //        //    else
                            //        //    {
                            //        //        dr["DiscountShipPrice"] = "￥0.00";
                            //        //    }
                            //        //}

                            //        dr["PremiumAmt"] = ASPManager.GetInstance().GetPremuimAmt(soInfo.SOAmt, stInfo.SysNo).ToString(AppConst.DecimalFormatWithCurrency);
                            //        dr["OrderNumber"] = stInfo.OrderNumber;
                            //        dt.Rows.Add(dr);

                            //    }
                            //}
                            //else
                            //{
                            #endregion

                            if (stInfo.ShipTypeID != "050")
                            {
                                if (Session["HasLimitedShipProduct"] != null)
                                {
                                    if (Session["HasLimitedShipProduct"].ToString().IndexOf("1") >= 0)
                                    {
                                        if (stInfo.SysNo == 2 || stInfo.SysNo == 3)  //EMS，邮局普包，所有限运商品都不可以运输
                                        {
                                            //lblmsg.Text = Session["HasLimitedShipProductInfo1"].ToString() + " 属邮局普包和EMS限运商品，如需使用邮局普包或EMS，请从购物车中删除限运商品！ <a href='http://www.icson.com/Service/NewsDetail.aspx?Type=Bulletin&ID=73' target='_blank'>查看限运说明</a>";
                                            continue;
                                        }
                                    }
                                    if (Session["HasLimitedShipProduct"].ToString().IndexOf("2") >= 0) //圆通航空限运
                                    {
                                        if (stInfo.SysNo == 7)
                                        {
                                            if (",1,201,131,403,814,1144,1323,1718,1591,2858,3225,2329,2621,".IndexOf("," + areaInfo.ProvinceSysNo.ToString() + ",") < 0)
                                            {
                                                //lblmsg.Text = Session["HasLimitedShipProductInfo2"].ToString() + " 属圆通限运商品，如需使用圆通，请从购物车中删除限运商品！ <a href='http://www.icson.com/Service/NewsDetail.aspx?Type=Bulletin&ID=73' target='_blank'>查看限运说明</a>";
                                                continue;
                                            }
                                        }
                                    }
                                }

                                DataRow dr = dt.NewRow();
                                dr["SysNo"] = stInfo.SysNo;
                                dr["ShipTypeName"] = stInfo.ShipTypeName;
                                dr["ShipTypeDesc"] = stInfo.ShipTypeDesc;
                                dr["Period"] = stInfo.Period;
                                decimal tempShipPrice = ASPManager.GetInstance().GetShipPrice(soInfo.GetTotalWeight(), soInfo.SOAmt, stInfo.SysNo, soInfo.ReceiveAreaSysNo);
                                dr["ShipPrice"] = tempShipPrice.ToString(AppConst.DecimalFormatWithCurrency);

                                #region

                                //判断每天只能免除一次运费
                                //bool hasUserdOneDayOneFreeShipFee = SaleManager.GetInstance().HasUserdOneDayOneFreeShipFee(AppConst.IntNull,soInfo.CustomerSysNo,DateTime.Now);
                                //if (hasUserdOneDayOneFreeShipFee)
                                //{
                                //    dr["DiscountShipPrice"] = "￥0.00";
                                //    lblFreeShipFeeTip.Text = "尊敬的客户,每位客户每天只能参加一次免运费活动,请您谅解!";
                                //}
                                //else
                                //{
                                //    lblFreeShipFeeTip.Text = "";                                
                                //    //MMbuy商城快递配送范围内的（包括杭州，苏州，扬州，上海市区），满100元免运费，最高免50元
                                //    //上海郊区，订单1000元以内收费5元，满1000元免运费
                                //    //圆通快递，江浙沪皖地区，满200元免运费，最高免50元
                                //    //圆通快递，非江浙沪皖地区，不免运费
                                //    //EMS、顺丰快递不免运费。

                                //    //上海市区及分站快递自提
                                //    if ((stInfo.SysNo == 1 || stInfo.SysNo == 8 || stInfo.SysNo == 9 || stInfo.SysNo == 10 || stInfo.SysNo == 11 || stInfo.SysNo == 13 || stInfo.SysNo == 17) && areaInfo.LocalCode < 5 && soInfo.SOAmt >= 100)  //MMbuy商城快递(非上海郊区)、订单金额满100
                                //    {
                                //        if (tempShipPrice <= 50)
                                //        {
                                //            dr["DiscountShipPrice"] = "-" + tempShipPrice.ToString(AppConst.DecimalFormatWithCurrency);
                                //        }
                                //        else
                                //        {
                                //            dr["DiscountShipPrice"] = "-￥50.00";
                                //        }
                                //    }
                                //    //上海郊区MMbuy商城快递
                                //    else if (stInfo.SysNo == 1 && areaInfo.LocalCode > 4)
                                //    {
                                //        if (soInfo.SOAmt < 1000)
                                //        {
                                //            dr["DiscountShipPrice"] = "-" + Convert.ToDecimal(tempShipPrice - 5).ToString(AppConst.DecimalFormatWithCurrency);
                                //        }
                                //        else
                                //        {
                                //            dr["DiscountShipPrice"] = "-" + tempShipPrice.ToString(AppConst.DecimalFormatWithCurrency);
                                //        }
                                //    }
                                //    //江浙沪皖，圆通满200免运费
                                //    else if (soInfo.SOAmt >= 200 && stInfo.SysNo == 7 && (areaInfo.ProvinceSysNo == 1 || areaInfo.ProvinceSysNo == 2621 || areaInfo.ProvinceSysNo == 1591 || areaInfo.ProvinceSysNo == 3225))
                                //    {
                                //        if (tempShipPrice <= 50)
                                //        {
                                //            dr["DiscountShipPrice"] = "-" + tempShipPrice.ToString(AppConst.DecimalFormatWithCurrency);
                                //        }
                                //        else
                                //        {
                                //            dr["DiscountShipPrice"] = "-￥50.00";
                                //        }
                                //    }
                                //    else
                                //    {
                                //        dr["DiscountShipPrice"] = "￥0.00";
                                //    }
                                //}

                                #endregion

                                dr["PremiumAmt"] = ASPManager.GetInstance().GetPremuimAmt(soInfo.SOAmt, stInfo.SysNo).ToString(AppConst.DecimalFormatWithCurrency);
                                dr["OrderNumber"] = stInfo.OrderNumber;
                                dt.Rows.Add(dr);

                            }
                        }
                        catch (BizException be)
                        {
                            this.ShowMsg(be.Message);
                        }
                        catch (Exception ex)
                        {
                            ErrorLog.GetInstance().Write(ex.Message);
                        }
                    }
                }
            }
            if (soInfo.CashPay > 1940)
            {
                DataView dv = new DataView(dt);
                dv.RowFilter = "SysNo <> 20";
                this.dgShipType.DataSource = dv;
                this.dgShipType.DataBind();
            }
            else
            {
                this.dgShipType.DataSource = dt;
                this.dgShipType.DataBind();
            }
            if (dgShipType.Items.Count != 0)
            {
                divDeliveryTime.Visible = false;
                foreach (DataGridItem dgItem in dgShipType.Items)
                {
                    RadioButton rdo = dgItem.FindControl("rdoSelectShipType") as RadioButton;

                    int ShipTypeSysNo = Int32.Parse(this.dgShipType.DataKeys[dgItem.ItemIndex].ToString());
                    if (ShipTypeSysNo == 1) //如果其他快递方式也能支持上下午送到，再去掉判断条件
                    {
                        int AreaSysNo = soInfo.ReceiveAreaSysNo;
                        if (ASPManager.IsAreaTimeliness(ShipTypeSysNo, AreaSysNo))
                        {
                            rdo.Attributes.Add("onclick", "SelectShipType(this,1);");
                            if (dgItem.ItemIndex == 0)
                            {
                                divDeliveryTime.Visible = true;
                            }
                        }
                        else
                        {
                            rdo.Attributes.Add("onclick", "SelectShipType(this,0);");
                        }
                    }
                    else
                    {
                        rdo.Attributes.Add("onclick", "SelectShipType(this,0);");
                    }

                    //if (soInfo.ShipTypeSysNo == AppConst.IntNull)
                    //{
                    //    if (dgItem.ItemIndex == 0)
                    //    {
                    //        //rdo.Checked = true;
                    //        //break;
                    //    }
                    //}
                    //else if ((int)dgShipType.DataKeys[dgItem.ItemIndex] == soInfo.ShipTypeSysNo)
                    //{
                    //    //rdo.Checked = true;
                    //    //break;
                    //}
                }
            }
            dgShipType.ItemCommand += new DataGridCommandEventHandler(dgShipType_ItemCommand);
        }

        /// <summary>
        /// 配送方式更改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdoSelectShipType_CheckedChanged(object sender, EventArgs e)
        {

            RadioButton rdoBtn = (RadioButton)sender;

            if (rdoBtn.Checked)
            {

                DataGridItem dgItem = (DataGridItem)rdoBtn.Parent.Parent;//.parent为cells 再.parent为DataGridItem

                int ShipTypeSysNo = dgItem.DataSetIndex;//获取索引

                soInfo.ShipTypeSysNo = ShipTypeSysNo;

                DataGrid dgShipType2 = (DataGrid)dgItem.Parent.Parent;//.parent为ChildTable 再.parent为DataGrid

                ShipTypeSysNo = int.Parse(dgShipType.DataKeys[ShipTypeSysNo].ToString());//根据索引取sysno

                //当配送方式为易讯快递1或者圆通快递7时“货到付款”可用，其他不可用

                BindPayTypeGrid();


                //foreach (DataGridItem item in this.dgPayType.Items)
                //{
                //    RadioButton rdo = item.FindControl("rdoSelectPayType") as RadioButton;

                //    string payType = item.Cells[1].Text;

                //    if (payType=="货到付款")
                //    {
                //        if (ShipTypeSysNo == 1 || ShipTypeSysNo == 7)
                //        {
                //            rdo.Enabled = true;
                //        }
                //        else 
                //        {
                //            rdo.Checked = false;
                //            rdo.Enabled = false;
                //        }


                //    }

                //}
            }
        }


        #endregion
    }
}