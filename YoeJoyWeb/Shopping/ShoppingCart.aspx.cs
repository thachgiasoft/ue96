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
        bool checkGo = true;
        protected decimal RulesDiscountAmtAll = 0;
        private decimal xMoney = 0;

        protected Hashtable newHt = new Hashtable();

        protected void Page_Load(object sender, System.EventArgs e)
        {
            base.CheckProfile(Context);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            oSession = CommonUtility.GetUserSession(Context);

            if (Request.QueryString["checkGo"] != null)
            {
                checkGo = Boolean.Parse(Request.QueryString["checkGo"]);
            }
            else
            {
                checkGo = false;
            }

            if (checkGo)
            {
                GoShopping();
            }

            if (!Page.IsPostBack)
            {
                string action = AppConst.StringNull;

                if (Request.QueryString["Cmd"] == null || Request.QueryString["Cmd"].ToString() == "")
                {
                    BindData();
                    //添加绑定促销优惠信息
                    //InitSORule();
                    //BindSaleRuleGrid();
                }
                else
                {
                    BindData();
                    xMoney = 0;
                    action = Request.QueryString["Cmd"].ToString();

                    //取出传入的productsysno，这应该是一个逗号分隔的productsysno的集合
                    string productSysno = Request.QueryString["ItemID"];
                    //取出传入的对应productsysno的购买数量，应该是一个逗号分隔的quantity集合，如果为null，数量为1
                    string quantity = Request.QueryString["quantity"];

                    string[] arrProductSysno = productSysno.Split(',');
                    string[] arrQuantity = new string[arrProductSysno.Length];
                    if (quantity == null || quantity == "")//没有传入数量，默认为1
                    {
                        arrQuantity = null;
                    }
                    else
                    {
                        try
                        {
                            arrQuantity = quantity.Split(',');
                        }
                        catch
                        {
                            ShowError("商品数量错误");
                        }
                    }
                    CartInfo oInfo = new CartInfo();
                    switch (action.ToUpper())
                    {
                        case "DEL":     //删除商品
                            for (int i = 0; i < arrProductSysno.Length; i++)
                            {
                                if (arrProductSysno[i] != AppConst.StringNull)
                                {
                                    CartManager.GetInstance().DeleteFromCart(Int32.Parse(arrProductSysno[i]));
                                }
                            }
                            Response.Redirect(Request.Url.AbsolutePath);
                            break;
                        case "ADD":     //增加商品
                        case "MOVE":    //从收藏加移入商品
                            Hashtable newItemHt = new Hashtable(3);
                            for (int i = 0; i < arrProductSysno.Length; i++)
                            {
                                if (arrProductSysno[i] != AppConst.StringNull)
                                {
                                    int qty = 0;
                                    if (arrQuantity == null)
                                        qty = 1;
                                    else
                                        qty = Int32.Parse(arrQuantity[i]);


                                    oInfo.ProductSysNo = Int32.Parse(arrProductSysno[i]);
                                    oInfo.Quantity = qty;
                                    oInfo.ExpectQty = qty;
                                }
                            }

                            newHt = CartManager.GetInstance().GetCartHash();

                            //CartManager.GetInstance().AddToCart(newItemHt);

                            bool check = true;



                            foreach (DataGridItem item in dgCart.Items)
                            {

                                if (item.Cells[0].Text.Equals(oInfo.ProductSysNo.ToString()))
                                {
                                    check = false;
                                    int limite = Int32.Parse((item.FindControl("lblLimitedQty") as Label).Text);
                                    int que = Int32.Parse(item.Cells[1].Text);
                                    TextBox textBox = item.FindControl("txtQuantity") as TextBox;

                                    int queNow = Int32.Parse(textBox.Text);
                                    queNow += oInfo.Quantity;

                                    if (queNow > limite)
                                    {
                                        ShowMessage("您期望购买的数量已经超出限购量！");
                                        return;
                                    }

                                    if (queNow > que)
                                    {
                                        oInfo.Quantity = que;
                                        textBox.Text = oInfo.Quantity.ToString();

                                        ShowMessage("您期望购买的数量已经超出库存，这是我们目前的所有库存了！");

                                    }
                                    else
                                    {
                                        oInfo.Quantity = queNow;
                                    }

                                    CartManager.GetInstance().UpdateCart(oInfo);

                                    newHt.Remove(oInfo.ProductSysNo);
                                    newHt.Add(oInfo.ProductSysNo, oInfo);

                                    break;

                                }
                            }
                            if (check)
                            {
                                newHt.Add(oInfo.ProductSysNo, oInfo);
                                Hashtable ht = new Hashtable();
                                ht.Add(oInfo.ProductSysNo, oInfo);
                                CartManager.GetInstance().AddToCart(ht);

                            }



                            BindData();

                            Response.Redirect(Request.Url.AbsolutePath);
                            break;
                        default:        //浏览购车
                            break;
                    }
                }
                //进入购物车页面时，默认选中所有赠品
                if (dgGiftCart.Items.Count > 0)
                {
                    oSession.GiftHash.Clear();
                    foreach (DataGridItem item in dgGiftCart.Items)
                    {
                        CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");
                        chkSelect.Checked = true;
                        oSession.GiftHash.Add((int)dgGiftCart.DataKeys[item.ItemIndex], (int)dgGiftCart.DataKeys[item.ItemIndex]);
                    }
                }
                else
                {
                    oSession.GiftHash.Clear();
                }
            }


        }

        private void BindData()
        {
            DataTable dt = BuildCartTable();
            DataTable dtGift = BuildCartGiftTable();

            if (dt == null || dt.Rows.Count == 0)
            {
                panelMessage.Visible = true;
                //panelCart.Visible = false;
                dgCart.Visible = false;
                c_gwc_btn.Visible = false;

                return;
            }
            else
            {
                panelMessage.Visible = false;
                //panelCart.Visible = true;
                dgCart.Visible = true;
                c_gwc_btn.Visible = true;
            }


            dgCart.DataSourceID = "";
            dgCart.DataSource = dt;
            dgCart.DataBind();

            bool HasExpectQty = false;
            foreach (DataGridItem item in dgCart.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
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
                        return;

                    int productSysNo = (int)dgCart.DataKeys[(int)item.ItemIndex];
                    CartInfo oCart = ht[productSysNo] as CartInfo;
                    //((TextBox)e.Item.FindControl("txtQuantity")).Text= oCart.Quantity.ToString();
                    TextBox txtQty = (TextBox)item.FindControl("txtQuantity") as TextBox;
                    txtQty.Text = oCart.Quantity.ToString();

                    TextBox txtExpectQty = (TextBox)item.FindControl("txtExpectQty") as TextBox;
                    Label lblLimitedQty = (Label)item.FindControl("lblLimitedQty") as Label;
                    if (lblLimitedQty.Text != AppConst.StringNull && Int32.Parse(lblLimitedQty.Text) > 0 && Int32.Parse(lblLimitedQty.Text) < 99)
                    {
                        txtExpectQty.Text = oCart.ExpectQty.ToString();
                        //txtExpectQty.Visible = false;
                    }

                    if (oCart.ExpectQty != AppConst.IntNull && oCart.ExpectQty > oCart.Quantity)
                    {
                        txtExpectQty.Text = oCart.ExpectQty.ToString();
                    }
                    else
                    {
                        txtExpectQty.Text = oCart.Quantity.ToString();
                    }

                    LinkButton cmdUpdateQty = (LinkButton)item.FindControl("cmdUpdate") as LinkButton;
                    txtQty.Attributes.Add("onblur", "document.getElementById('" + cmdUpdateQty.ClientID + "').click();");  //onchange

                    if (Int32.Parse(txtExpectQty.Text) > Int32.Parse(txtQty.Text))
                    {
                        HasExpectQty = true;
                    }
                }
            }

            if (HasExpectQty)
            {
                dgCart.Columns[8].Visible = true;
                //lblExpectQty.Visible = true;
                //Response.Write("<div><script>alert('您的期望购买数量超过了限购数量，系统会纪录您的期望购买数量，MMMbuy.cn 客服人员将与您联系！');</script></div>");
                //Page.RegisterClientScriptBlock("a", "<script language='javascript'>alert('您的期望购买数量超过了限购数量，系统会纪录您的期望购买数量，MMMbuy.cn 客服人员将与您联系！')</script>");
                ClientScript.RegisterStartupScript(typeof(Page), "pagestartup", "<script language='javascript'>alert('您的期望购买数量超过了限购数量，系统会纪录您的期望购买数量，攸怡客服人员将与您联系！')</script>");
                //Page.RegisterStartupScript("a", "<script language='javascript'>alert('您的期望购买数量超过了限购数量，系统会纪录您的期望购买数量，MMMbuy.cn 客服人员将与您联系！')</script>");
            }
            else
            {
                dgCart.Columns[8].Visible = false;
                //lblExpectQty.Visible = false;
            }

            if (dtGift == null || dtGift.Rows.Count == 0)
            {
                //trGift.Visible = false;
                return;
            }
            else
                //trGift.Visible = true;

                dgGiftCart.DataSource = dtGift;
            dgGiftCart.DataBind();

            foreach (DataGridItem item in dgGiftCart.Items)
            {
                CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");
                if (oSession.GiftHash.Count != 0 && oSession.GiftHash.ContainsKey((int)dgGiftCart.DataKeys[item.ItemIndex]))
                    chkSelect.Checked = true;
                else
                    chkSelect.Checked = false;
            }
        }

        //private void InitSORule()
        //{
        //    soInfo = new SOInfo();
        //    soInfo.IsWholeSale = (int)AppEnum.YNStatus.No;
        //    Hashtable cartHash = CartManager.GetInstance().GetCartHash();

        //    Hashtable sysNoHash = new Hashtable(5);
        //    foreach (CartInfo cartInfo in cartHash.Values)
        //    {
        //        sysNoHash.Add(cartInfo.ProductSysNo, null);
        //    }
        //    Hashtable soProductHash = SaleManager.GetInstance().LoadSOProducts(sysNoHash, (int)AppEnum.SOItemType.ForSale);

        //    // 加入主商品
        //    foreach (SOItemInfo soItem in soProductHash.Values)
        //    {
        //        soItem.Quantity = ((CartInfo)cartHash[soItem.ProductSysNo]).Quantity;
        //        soItem.DiscountAmt = 0m;
        //        soItem.ProductType = (int)AppEnum.SOItemType.ForSale;
        //        soInfo.ItemHash.Add(soItem.ProductSysNo, soItem);
        //    }

        //    SaleManager.GetInstance().CalcSaleRule(soInfo);
        //}

        //private void BindSaleRuleGrid()
        //{
        //    DataTable dt = new DataTable("SaleRuleTable");
        //    dt.Columns.Add(new DataColumn("SaleRuleName", typeof(System.String)));
        //    dt.Columns.Add(new DataColumn("DiscountAmt", typeof(System.String)));
        //    dt.Columns.Add(new DataColumn("DiscountTime", typeof(System.Int32)));
        //    dt.Columns.Add(new DataColumn("SubTotal", typeof(System.String)));
        //    if (soInfo.SaleRuleHash.Count != 0)
        //    {
        //        decimal rulesDiscountAmt = 0;
        //        foreach (SOSaleRuleInfo srInfo in soInfo.SaleRuleHash.Values)
        //        {
        //            DataRow dr = dt.NewRow();
        //            dr["SaleRuleName"] = srInfo.SaleRuleName;
        //            dr["DiscountAmt"] = srInfo.Discount.ToString(AppConst.DecimalFormatWithCurrency);
        //            dr["DiscountTime"] = srInfo.Times;
        //            dr["SubTotal"] = ((decimal)(srInfo.Discount * srInfo.Times)).ToString(AppConst.DecimalFormatWithCurrency);
        //            dt.Rows.Add(dr);
        //            rulesDiscountAmt += (decimal)(srInfo.Discount * srInfo.Times);
        //        }
        //        RulesDiscountAmtAll = rulesDiscountAmt;
        //    }
        //    lblAmt.Text = (xMoney - RulesDiscountAmtAll).ToString(AppConst.DecimalFormat);
        //    this.dgSaleRule.DataSource = dt;
        //    this.dgSaleRule.DataBind();
        //    if (dt.Rows.Count == 0)
        //        this.dgSaleRule.Visible = false;
        //    else
        //        this.dgSaleRule.Visible = true;
        //}

        private DataTable BuildCartTable()
        {
            DataTable table = new DataTable("BuildCartTable");
            table.Columns.Add(new DataColumn("SysNo", typeof(int)));		//product			
            table.Columns.Add(new DataColumn("ProductName", typeof(string)));
            table.Columns.Add(new DataColumn("PromotionWord", typeof(string)));
            table.Columns.Add(new DataColumn("CurrentPrice", typeof(decimal)));
            table.Columns.Add(new DataColumn("Point", typeof(string)));
            table.Columns.Add(new DataColumn("Quantity", typeof(int)));
            table.Columns.Add(new DataColumn("InStock", typeof(string)));
            table.Columns.Add(new DataColumn("ProductLink", typeof(string)));
            table.Columns.Add(new DataColumn("CurrentPriceShow", typeof(string)));
            table.Columns.Add(new DataColumn("LineTotal", typeof(string)));
            table.Columns.Add(new DataColumn("Operation", typeof(string)));
            table.Columns.Add(new DataColumn("PointStatus", typeof(string)));
            table.Columns.Add(new DataColumn("CashRebate", typeof(string)));

            table.Columns.Add(new DataColumn("LimitedQty", typeof(int)));  //每单限购
            table.Columns.Add(new DataColumn("DeliveryTimeliness", typeof(string))); //发货时间
            table.Columns.Add(new DataColumn("ExpectQty", typeof(int)));  //期望订购数量

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
                return table;

            //if (ASPManager.GetInstance().IsAllChannel(ht))
            //{
            //    lblIsSameChannel.Visible = true;
            //    txtIsCheck.Text = "1";
            //}
            //else
            //{
            lblIsSameChannel.Visible = false;
            txtIsCheck.Text = "0";
            //}

            DataSet ds = OnlineListManager.GetInstance().GetCartDs(ht);
            // DataSet dspresend = OnlineListManager.GetInstance().GetPresendDs(ht);

            //暂时不处理批发对应的q1,q2,q3
            //decimal xMoney = 0; //总价值
            int xPoint;
            xPoint = buildTable(table, ht, ds);
            //xPoint += buildTable(table, ht, dspresend);

            if (xMoney > 0)
            {
                lblAmtTxt.Visible = true;
                if (RulesDiscountAmtAll > 0)
                {
                    lblAmt.Text = (xMoney - RulesDiscountAmtAll).ToString("0");
                }
                else
                {
                    lblAmt.Text = xMoney.ToString("0");
                }
            }
            else
            {
                lblAmtTxt.Visible = false;
            }

            if (xPoint > 0)
            {
                lblPoint.Visible = true;
                lblPointAmt.Text = xPoint.ToString();
            }
            else
            {
                lblPoint.Visible = false;
            }
            return table;
        }

        private int buildTable(DataTable table, Hashtable ht, DataSet ds)
        {
            int xPoint = 0;		//仅仅积分支付部分

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                DataRow row = table.NewRow();
                int productSysNo = Util.TrimIntNull(dr["sysno"]);
                row["Sysno"] = productSysNo;

                row["ProductName"] = Util.TrimNull(dr["productname"]);

                row["PromotionWord"] = Util.TrimNull(dr["PromotionWord"]);

                decimal currentPrice = Util.TrimDecimalNull(dr["currentprice"]);
                row["CurrentPrice"] = currentPrice.ToString(AppConst.DecimalFormat);

                decimal cashRebate = Util.TrimDecimalNull(dr["cashRebate"]);
                int pointType = Util.TrimIntNull(dr["PointType"]);
                if (cashRebate > 0 && pointType != (int)AppEnum.ProductPayType.PointPayOnly)
                    row["CashRebate"] = cashRebate.ToString(AppConst.DecimalFormat);
                else
                    row["CashRebate"] = "--";

                int point = Util.TrimIntNull(dr["point"]);
                if (point > 0 && pointType != (int)AppEnum.ProductPayType.PointPayOnly)
                {
                    row["Point"] = point.ToString();
                }
                else
                {
                    row["Point"] = "--";
                }

                CartInfo oCart = ht[productSysNo] as CartInfo;
                int LimitedQty = Util.TrimIntNull(dr["LimitedQty"]);
                if (LimitedQty > 0 && oCart.Quantity > LimitedQty)
                {
                    oCart.Quantity = LimitedQty;
                }
                row["Quantity"] = oCart.Quantity;
                if (oCart.Quantity <= Util.TrimIntNull(dr["OnlineQty"]))
                    row["InStock"] = "有";
                else
                {
                    //row["InStock"] = "<FONT color='red'><STRONG>无</STRONG></FONT>";
                    oCart.Quantity = Util.TrimIntNull(dr["OnlineQty"]);
                    row["Quantity"] = oCart.Quantity;
                    row["InStock"] = "有";
                }

                if (oCart.Quantity <= Util.TrimIntNull(dr["AvailableQty"]))
                {
                    row["DeliveryTimeliness"] = "1日内";
                }
                else
                {

                    if (Util.TrimIntNull(dr["VirtualArriveTime"]) != AppConst.IntNull)
                    {
                        row["DeliveryTimeliness"] = AppEnum.GetVirtualArriveTime(Util.TrimIntNull(dr["VirtualArriveTime"])) + "内";
                    }
                    else
                    {
                        row["DeliveryTimeliness"] = AppEnum.GetVirtualArriveTime((int)AppEnum.VirtualArriveTime.OneToThree) + "内";
                    }
                }

                if (LimitedQty > 0 && LimitedQty <= Util.TrimIntNull(dr["OnlineQty"]))
                {
                    row["LimitedQty"] = LimitedQty.ToString();
                }
                else
                {
                    row["LimitedQty"] = "99";//Util.TrimIntNull(dr["OnlineQty"]).ToString();
                }

                row["ProductLink"] = "<a href=\"javascript:openNewWindow('../Items/ItemDetail.aspx?ItemID=" + productSysNo.ToString() + "')\">" + Util.TrimNull(dr["productname"]) + "<font color=red>" + Util.TrimNull(dr["PromotionWord"]) + "</font>" + "</a>";


                if (Util.TrimNull(dr["size2name"]).Length > 0)
                {
                    row["ProductLink"] += "<br />[尺码：" + Util.TrimNull(dr["size2name"]) + "]";
                }

                string currentPriceShow = "";
                string linetotal = "";

                if (pointType != (int)AppEnum.ProductPayType.PointPayOnly)
                {
                    currentPriceShow = (currentPrice + cashRebate).ToString(AppConst.DecimalFormat);
                    linetotal = (currentPrice * oCart.Quantity).ToString(AppConst.DecimalFormat);
                }
                else
                {
                    currentPriceShow = Decimal.Round(currentPrice * AppConst.ExchangeRate, 0).ToString() + "积分<font color=red>(仅可积分兑换)</font>";
                    linetotal = (Decimal.Round(currentPrice * AppConst.ExchangeRate, 0) * oCart.Quantity).ToString() + "积分";
                }

                row["CurrentPriceShow"] = currentPriceShow;
                row["linetotal"] = linetotal;
                row["operation"] = "<a href='ShoppingCart.aspx?Cmd=del&ItemID=" + productSysNo + "'>删除</a>";

                row["ExpectQty"] = row["Quantity"];

                if (pointType == (int)AppEnum.ProductPayType.PointPayOnly)
                    xPoint += Convert.ToInt32(Decimal.Round(currentPrice * AppConst.ExchangeRate, 0) * oCart.Quantity);
                else
                    xMoney += currentPrice * oCart.Quantity;

                table.Rows.Add(row);
            }
            return xPoint;
        }

        private DataTable BuildCartGiftTable()
        {
            DataTable table = new DataTable("BuildGiftCartTable");
            DataColumn dc0 = new DataColumn("SysNo", typeof(System.Int32));
            DataColumn dc1 = new DataColumn("GiftName", typeof(System.String));
            DataColumn dc2 = new DataColumn("MasterName", typeof(System.String));
            DataColumn dc3 = new DataColumn("Quantity", typeof(System.Int32));
            DataColumn dc4 = new DataColumn("Weight", typeof(System.Int32));
            DataColumn dc5 = new DataColumn("ExpectQty", typeof(System.Int32));

            table.Columns.Add(dc0);
            table.Columns.Add(dc1);
            table.Columns.Add(dc2);
            table.Columns.Add(dc3);
            table.Columns.Add(dc4);
            table.Columns.Add(dc5);
            //------------------------------------------------------2008-08-18----------------------------------------------------------------------------
            //Hashtable ht = CartManager.GetInstance().GetCartHash();

            //if (ht == null || ht.Count == 0)
            //    return table;

            //DataSet ds = OnlineListManager.GetInstance().GetCartGiftDs(ht);
            //Hashtable topHash = new Hashtable(5);
            //foreach (DataRow dr in ds.Tables[0].Rows)
            //{
            //    if (!topHash.ContainsKey((int)dr["ParentSysNo"]))
            //        topHash.Add((int)dr["ParentSysNo"], dr);
            //    else
            //    {
            //        DataRow odr = (DataRow)topHash[(int)dr["ParentSysNo"]];
            //        if (string.Compare(dr["ListOrder"].ToString(), odr["ListOrder"].ToString()) < 0)
            //        {
            //            topHash.Remove((int)dr["ParentSysNo"]);
            //            topHash.Add((int)dr["ParentSysNo"], dr);
            //        }
            //    }
            //}
            //foreach (DataRow dr in ds.Tables[0].Rows)
            //{
            //    if (topHash.ContainsValue(dr))
            //    {
            //        DataRow drShow = table.NewRow();
            //        drShow["SysNo"] = (int)dr["SysNo"];
            //        drShow["GiftName"] = dr["GiftName"].ToString();
            //        drShow["MasterName"] = "<a href='../Items/ItemDetail.aspx?ItemID=" + dr["MasterSysNo"].ToString() + "' target='_blank'>" + dr["MasterName"].ToString() + "</a>";
            //        drShow["Weight"] = (int)dr["Weight"];
            //        foreach (CartInfo cartInfo in ht.Values)
            //        {
            //            if (cartInfo.ProductSysNo == (int)dr["MasterSysNo"])
            //            {
            //                drShow["Quantity"] = cartInfo.Quantity;
            //                drShow["ExpectQty"] = cartInfo.ExpectQty;
            //                break;
            //            }
            //        }
            //        table.Rows.Add(drShow);
            //    }
            //}
            //return table;
            //--------------------------------------------------------------------------------------------------------------------------------------------

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
                return table;

            DataSet ds = OnlineListManager.GetInstance().GetCartGiftDs(ht);
            List<DataRow> giftList = new List<DataRow>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                giftList.Add(dr);
            }
            foreach (DataRow dr in giftList)
            {
                DataRow drShow = table.NewRow();
                drShow["SysNo"] = (int)dr["SysNo"];
                drShow["GiftName"] = dr["GiftName"].ToString();
                drShow["MasterName"] = "<a href='../Items/ItemDetail.aspx?ItemID=" + dr["MasterSysNo"].ToString() + "' target='_blank'>" + dr["MasterName"].ToString() + "</a>";
                drShow["Weight"] = (int)dr["Weight"];
                foreach (CartInfo cartInfo in ht.Values)
                {
                    if (cartInfo.ProductSysNo == (int)dr["MasterSysNo"])
                    {
                        drShow["Quantity"] = cartInfo.Quantity;
                        drShow["ExpectQty"] = cartInfo.ExpectQty;
                        break;
                    }
                }
                table.Rows.Add(drShow);
            }
            return table;
        }

        private void dgCart_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            //bool HasExpectQty = false;
            switch (e.CommandName)
            {
                case "cmdUpdate":
                    int productSysNo = (int)dgCart.DataKeys[(int)e.Item.ItemIndex];
                    int quantity = AppConst.IntNull;
                    int expectQty = AppConst.IntNull;
                    try
                    {
                        quantity = Convert.ToInt32(((TextBox)e.Item.FindControl("txtQuantity")).Text);
                        if (quantity <= 0)
                        {
                            ShowMessage("数量必须大于零");
                            return;
                        }
                    }
                    catch
                    {
                        ShowMessage("商品数量必须是整数！");
                        return;
                    }

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
                        return;
                    DataSet ds = OnlineListManager.GetInstance().GetCartDs(ht);
                    string alert = "";

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["productSysNo"].ToString().Equals(productSysNo.ToString()))
                        {
                            ((TextBox)e.Item.FindControl("txtExpectQty")).Text = quantity.ToString();
                            expectQty = quantity;

                            int LimitedQty = Util.TrimIntNull(dr["LimitedQty"]);
                            if (LimitedQty > 0 && quantity > LimitedQty)
                            {
                                quantity = LimitedQty;
                                ((TextBox)e.Item.FindControl("txtQuantity")).Text = quantity.ToString();
                                //expectQty = LimitedQty;
                                //alert = "<script>alert('每单限购" + LimitedQty.ToString() + "件！');</script>";
                            }
                            else if (quantity > Int32.Parse(dr["OnlineQty"].ToString()))
                            {
                                quantity = Int32.Parse(dr["OnlineQty"].ToString());
                                ((TextBox)e.Item.FindControl("txtQuantity")).Text = quantity.ToString();

                                //alert = "<script>alert('您要订购的数量超过了我们的限购数量，如需订购更多数量，请拨打400-820-1878');</script>";
                                //Response.Write("<script>alert('您期望购买的数量超过了我们的库存，MMMbuy.cn 客服人员将与您联系');</script>");
                                this.dgCart.Columns[8].Visible = true;
                                //HasExpectQty = true;
                            }
                        }
                    }

                    CartInfo oCart = new CartInfo();
                    oCart.ProductSysNo = productSysNo;
                    oCart.Quantity = quantity;
                    if (expectQty > quantity)
                    {
                        oCart.ExpectQty = expectQty;
                    }
                    else
                    {
                        oCart.ExpectQty = quantity;
                        ((TextBox)e.Item.FindControl("txtExpectQty")).Text = quantity.ToString();
                    }

                    CartManager.GetInstance().UpdateCart(oCart);

                    //更新显示
                    BindData();
                    //Header1.BindHeadCart();
                    if (alert.Length > 10)
                    {
                        Response.Write(alert);
                        return;
                    }

                    break;
                default:
                    break;
            }

            Response.Redirect(Request.Url.AbsolutePath);

            //if (HasExpectQty)
            //{
            //    if (Request.Url.AbsolutePath.EndsWith(".aspx"))
            //        Response.Redirect(Request.Url.AbsolutePath + "?type=1");
            //    else
            //        Response.Redirect(Request.Url.AbsolutePath + "&type=1");
            //}
            //else
            //{
            //    Response.Redirect(Request.Url.AbsolutePath);
            //}
        }

        private void ShowMessage(string message)
        {
            lblMessage.Text = message;

            //去除HTML标签只保留文本内容
            message = System.Text.RegularExpressions.Regex.Replace(message, @"<(?!/?p(\s|>)+)[^>]*?>", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            Page.ClientScript.RegisterStartupScript(GetType(), null, "alert('" + message + "');", true);
        }

        private void btnRefresh_Click(object sender, System.EventArgs e)
        {
            Response.Redirect(Request.Url.AbsolutePath);
        }

        private void btnEmpty_Click(object sender, System.EventArgs e)
        {
            CartManager.GetInstance().ClearCart();
            Response.Redirect(Request.Url.AbsolutePath);
        }

        private void btnSaveToMyFavorite_Click(object sender, System.EventArgs e)
        {
            string ItemID = "";

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
                return;
            foreach (int item in ht.Keys)
            {
                ItemID += "&ItemID=" + item.ToString();
            }

            if (ItemID != "")
            {
                //打开收藏夹页面
                Response.Redirect("../Account/MyFavorite.aspx?Type=New" + ItemID);
            }
        }



        private void dgCart_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            //if(e.Item.ItemType==ListItemType.Item||e.Item.ItemType==ListItemType.AlternatingItem)
            //{
            //    Hashtable ht = CartManager.GetInstance().GetCartHash();
            //    if ( ht == null || ht.Count == 0)
            //        return;

            //    int productSysNo = (int)dgCart.DataKeys[(int)e.Item.ItemIndex];
            //    CartInfo oCart = ht[productSysNo] as CartInfo;
            //    //((TextBox)e.Item.FindControl("txtQuantity")).Text= oCart.Quantity.ToString();
            //    TextBox txtQty = (TextBox)e.Item.FindControl("txtQuantity") as TextBox;
            //    txtQty.Text = oCart.Quantity.ToString();

            //    ImageButton cmdUpdateQty = (ImageButton)e.Item.FindControl("cmdUpdate") as ImageButton;
            //    txtQty.Attributes.Add("onchange", "<script>document.getElementById('"+ cmdUpdateQty.ClientID +"').click();</script>");
            //}
        }

        protected void chkSelect_CheckedChanged(object sender, System.EventArgs e)
        {
            oSession.GiftHash.Clear();
            foreach (DataGridItem item in dgGiftCart.Items)
            {
                CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");
                int sysno = (int)dgGiftCart.DataKeys[item.ItemIndex];
                if (chkSelect.Checked)
                    oSession.GiftHash.Add(sysno, sysno);
            }
            BindData();
        }



        protected string GetShoppingCartExcellentRecommend()
        {
            //return OnlineListProductManager.GetInstance().GetShoppingCartExcellentRecommend();
            return "";
        }

        protected void dgCart_ItemDataBound1(object sender, DataGridItemEventArgs e)
        {
            //Icson.Utils.DataGridUtil.GetInstance().setgridstyle(dgCart, e);
        }

        //private int GetChannelID(int C1ID)
        //{
        //    CategoryManager cm = CategoryManager.GetInstance();

        //    int chsysno = cm.GetChannelSysNo(C1ID);

        //    return chsysno;

        //}

        public int GetFirstCategorySysNo(int c3sysno)
        {
            CategoryManager cm = CategoryManager.GetInstance();
            Category3Info c3 = cm.GetC3Hash()[c3sysno] as Category3Info;
            int c2sysno = c3.C2SysNo;

            Category2Info c2 = cm.GetC2Hash()[c2sysno] as Category2Info;
            int c1sysno = c2.C1SysNo;
            return c1sysno;
        }

        protected void imgEmpty_Click(object sender, ImageClickEventArgs e)
        {
            CartManager.GetInstance().ClearCart();
            Response.Redirect(Request.Url.AbsolutePath);
        }

        protected void imgContinue_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("../Default.aspx");
        }

        protected void imgCheckOut_Click(object sender, ImageClickEventArgs e)
        {
            if (txtIsCheck.Text == "1")
            {
                lblIsSameChannel.Text = "您无法下单：原因是您购买商品中含有欧洲直发商品与国内商品，为了不影响你的收货时间，请分开下单！";
                lblIsSameChannel.Visible = true;
                return;
            }

            foreach (DataGridItem item in this.dgCart.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    Label lblLimitedQty1 = item.FindControl("lblLimitedQty") as Label;   //限购数量
                    TextBox txtQuantity1 = item.FindControl("txtQuantity") as TextBox;   //订购数量
                    TextBox txtExpectQty = item.FindControl("txtExpectQty") as TextBox;  //期望订购数量

                    try
                    {
                        if (txtExpectQty.Text.Trim() != AppConst.StringNull)
                        {
                            int qtyTemp = Convert.ToInt32(txtExpectQty.Text);
                            if (qtyTemp <= 0)
                            {
                                ShowMessage("期望购买数量必须大于零");
                                return;
                            }
                        }
                    }
                    catch
                    {
                        ShowMessage("期望购买数量必须是整数！");
                        return;
                    }

                    int LimitedQty = Int32.Parse(lblLimitedQty1.Text);
                    int Quantity = Int32.Parse(txtQuantity1.Text);
                    int productSysNo = (int)dgCart.DataKeys[(int)item.ItemIndex];

                    if (txtExpectQty.Text != AppConst.StringNull)
                    {
                        int ExpectQty = Int32.Parse(txtExpectQty.Text);
                        if (ExpectQty > Quantity)
                        {
                            CartInfo oCart = new CartInfo();
                            oCart.ProductSysNo = productSysNo;
                            oCart.Quantity = Quantity;
                            oCart.ExpectQty = ExpectQty;
                            CartManager.GetInstance().UpdateCart(oCart);

                            //Response.Write("<script>alert('您期望购买的数量超过了我们的库存，MMMbuy.cn 客服人员将与您联系');</script>");
                        }
                        else if (ExpectQty < Quantity)
                        {
                            CartInfo oCart = new CartInfo();
                            oCart.ProductSysNo = productSysNo;
                            oCart.Quantity = Quantity;
                            oCart.ExpectQty = Quantity;
                            CartManager.GetInstance().UpdateCart(oCart);
                        }
                    }

                    if (Quantity > LimitedQty)
                    {
                        txtQuantity1.Text = lblLimitedQty1.Text;

                        CartInfo oCart = new CartInfo();
                        oCart.ProductSysNo = productSysNo;
                        oCart.ExpectQty = oCart.Quantity;
                        //oCart.ExpectQty = LimitedQty;
                        oCart.Quantity = LimitedQty;
                        CartManager.GetInstance().UpdateCart(oCart);
                        ShowMessage("购买数量超出了库存！");
                        //Response.Write("<script>alert('您要订购的数量超过了我们的限购数量，如需订购更多数量，请拨打400-820-1878');</script>");
                        //ShowMessage("您期望购买数量超过了我们的库存，请填写期望购买数量，MMMbuy.cn 客服人员将与您联系");
                        return;
                    }
                }
            }




            //商品数量不足的检查在结帐，因为这里检查也没有。

            GoShopping();
        }

        private void GoShopping()
        {
            DataSet ds = new DataSet();
            SOInfo oldSoInfo = null;
            try
            {
                ds = SaleManager.GetInstance().GetSOOnlineDs(oSession.sCustomer.SysNo, 1);
            }
            catch
            {
                checkGo = true;
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
                Response.Redirect("CheckOut.aspx");
            }

            else
                Response.Redirect("precheckout.aspx");
        }

        /// <summary>
        /// 购买数量修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdUpdate_Click(object sender, EventArgs e)
        {

            CartInfo cart = new CartInfo();

            LinkButton lbtn = sender as LinkButton;
            object o = lbtn.Parent.Parent;
            DataGridItem item = (DataGridItem)o;

            int ProductSysNo = (Int32.Parse(item.Cells[0].Text));

            TextBox textBox = lbtn.Parent.FindControl("txtQuantity") as TextBox;
            newHt = CartManager.GetInstance().GetCartHash();
            cart = (CartInfo)newHt[ProductSysNo];
            int count;
            try
            {
                count = Int32.Parse(textBox.Text);
                if (count < 1)
                {
                    ShowMessage("期望购买数量必须是正整数！");
                    textBox.Text = cart.Quantity.ToString();
                    return;
                }
            }
            catch
            {
                ShowMessage("期望购买数量必须是正整数！");
                textBox.Text = (cart.Quantity).ToString();
                return;

            }

            DataSet ds = OnlineListManager.GetInstance().GetCartDs(newHt);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {


                if (dr[0].ToString().Equals(cart.ProductSysNo.ToString()))
                {

                    //Int32.Parse(item.Cells[6].Text)

                    int limite = Int32.Parse((item.FindControl("lblLimitedQty") as Label).Text);


                    int que = Int32.Parse(dr["OnlineQty"].ToString());

                    if (count > limite)
                    {
                        textBox.Text = cart.Quantity.ToString();
                        ShowMessage("您期望购买的数量已经超出限购量，请重新选择！");
                        return;
                    }
                    else
                        cart.Quantity = count;

                    if (que <= 0)
                    {
                        textBox.Text = que.ToString();
                        ShowMessage("您要购买的" + dr["productName"] + "已无库存！");
                        return;
                    }


                    if (cart.Quantity > que)
                    {
                        cart.Quantity = que;
                        textBox.Text = cart.Quantity.ToString();

                        ShowMessage("您期望购买的数量已经超出库存，这是我们目前的所有库存了！");
                        return;
                    }
                }
            }


            if (String.IsNullOrEmpty(item.Cells[6].Text))
            {
                cart.ExpectQty = 0;
            }
            else
            {
                cart.ExpectQty = cart.Quantity;
            }

            CartManager.GetInstance().UpdateCart(cart);

            Response.Redirect(Request.Url.AbsolutePath);

            //newHt.Remove(cart.ProductSysNo);
            //newHt.Add(cart.ProductSysNo, cart);

            //BindData();

        }
    }
}