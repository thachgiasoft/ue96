using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Icson.BLL.Finance;
using Icson.Objects.Finance;
using Icson.Utils;
using Icson.Objects.Basic;
using Icson.Objects.Sale;
using Icson.Objects.Online;
using Icson.BLL;
using Icson.BLL.Basic;
using Icson.BLL.Sale;
using Icson.BLL.Online;
using Icson.Objects;
using YoeJoyHelper;
using YoeJoyHelper.Security;
using System.Data;
using System.Collections;

namespace YoeJoyWeb.User
{
    public partial class MyOrderDetail : SecurityPageBase
    {
        protected SOInfo soInfo = new SOInfo();
        protected int soSysNo = AppConst.IntNull;
        protected IcsonSessionInfo oSession;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            // Put user code to initialize the page here

            this.btnCancel.Attributes.Add("onclick", "return confirm('您确认要作废订单吗？')");

            if (Request.QueryString["ID"] != null && Util.IsNaturalNumber(Request.QueryString["ID"]))
                soSysNo = int.Parse(Request.QueryString["ID"]);
            else
                this.ShowError("此订单不存在，请重试");
            oSession = CommonUtility.GetUserSession(Context);
            soInfo = SaleManager.GetInstance().LoadSO(soSysNo);
            if (soInfo.CustomerSysNo != oSession.sCustomer.SysNo)
                this.ShowError("抱歉，您无权查看他人订单");

            if (Request.QueryString["action"] == "update")
            {
                PanelShow.Visible = false;
                PanelUpdate.Visible = true;
                btnUpdate.Visible = true;

            }
            else
            {
                PanelShow.Visible = true;
                PanelUpdate.Visible = false;
                btnUpdate.Visible = false;
            }
            if (soInfo.PayTypeSysNo == 13) //支付宝即时到帐付款
            {
                trSOAlipay.Visible = true;
            }
            else
            {
                trSOAlipay.Visible = false;
            }
            lblResult.Text = "";
            lblResult.Visible = false;
            if (!Page.IsPostBack)
            {

                this.BindPage();
                BindShipTypeGrid();
                BindPayTypeGrid();
                BindAddressList();
            }
        }
        private void BindAddressList()
        {
            int customerSysNo = oSession.sCustomer.SysNo;
            DataSet ds = CustomerManager.GetInstance().GetCustomerAddressDs(customerSysNo);
            //if (!Util.HasMoreRow(ds))
            //    lblSysNo.Text = "0";
            DataList1.DataSource = ds;
            DataList1.DataBind();
        }

        protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lbl1 = e.Item.FindControl("lblIsDefault") as Label;
                Label lbl2 = e.Item.FindControl("lblBrief") as Label;
                ImageButton imgbtn = e.Item.FindControl("btnSelect") as ImageButton;

                int IsDefault = Int32.Parse(lbl1.Text);
                if (IsDefault == (int)AppEnum.BiStatus.Valid)
                {
                    lbl2.Text = lbl2.Text + "[默认]";
                    imgbtn.ImageUrl = "../Images/site/SelectedIcon.jpg";

                    int sysno = Int32.Parse(DataList1.DataKeys[e.Item.ItemIndex].ToString());
                    // lblSysNo.Text = sysno.ToString();

                    CustomerAddressInfo o = CustomerManager.GetInstance().LoadCustomerAddress(sysno);
                    txtRname.Text = o.Name;

                    txtRCellPhone.Text = o.CellPhone;
                    txtRPhone.Text = o.Phone;
                    if (oSession.sCustomer.DwellAreaSysNo != 1)//这个是new user的默认值。
                        RArea.AreaSysNo = o.AreaSysNo;
                    txtRaddress.Text = o.Address;
                    txtRzip.Text = o.Zip;
                }
                else
                {
                    imgbtn.ImageUrl = "../Images/site/UnSelectedIcon.jpg";
                }
            }
        }
        protected void DataList1_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.SelectedItem)
            {
                if (e.CommandName == "Select")
                {
                    foreach (DataListItem item in DataList1.Items)
                    {
                        if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.SelectedItem)
                        {
                            ImageButton imgbtn = item.FindControl("btnSelect") as ImageButton;
                            if (item.ItemIndex == e.Item.ItemIndex)
                            {
                                imgbtn.ImageUrl = "../Images/site/SelectedIcon.jpg";
                            }
                            else
                            {
                                imgbtn.ImageUrl = "../Images/site/UnSelectedIcon.jpg";
                            }
                        }
                    }

                    int sysno = Int32.Parse(DataList1.DataKeys[e.Item.ItemIndex].ToString());
                    //lblSysNo.Text = sysno.ToString();
                    CustomerAddressInfo o = CustomerManager.GetInstance().LoadCustomerAddress(sysno);
                    txtRname.Text = o.Name;

                    txtRCellPhone.Text = o.CellPhone;
                    txtRPhone.Text = o.Phone;
                    if (oSession.sCustomer.DwellAreaSysNo != 1)//这个是new user的默认值。
                        RArea.AreaSysNo = o.AreaSysNo;
                    txtRaddress.Text = o.Address;
                    txtRzip.Text = o.Zip;
                }
            }
        }
        private void BindPage()
        {
            //Bind CustomerInfo
            this.lblName.Text = oSession.sCustomer.CustomerName;
            this.lblAddr.Text = oSession.sCustomer.DwellAddress;
            this.lblPhone.Text = oSession.sCustomer.Phone;
            this.lblCellPhone.Text = oSession.sCustomer.CellPhone;
            this.lblZip.Text = oSession.sCustomer.DwellZip;
            AreaInfo area = ASPManager.GetInstance().LoadArea(oSession.sCustomer.DwellAreaSysNo);
            this.lblProvince.Text = area.ProvinceName;
            this.lblCity.Text = area.CityName;
            this.lblDistrict.Text = area.DistrictName;

            //修改部分显示

            txtRname.Text = soInfo.ReceiveName;

            txtRCellPhone.Text = soInfo.ReceiveCellPhone;
            txtRPhone.Text = soInfo.ReceivePhone;
            if (oSession.sCustomer.DwellAreaSysNo != 1)//这个是new user的默认值。
                RArea.AreaSysNo = soInfo.ReceiveAreaSysNo;
            txtRaddress.Text = soInfo.ReceiveAddress;
            txtRzip.Text = soInfo.ReceiveZip;

            //Bind SO
            //Bind SOMaster
            this.lblSOID.Text = soInfo.SOID;
            this.lblReceiveName.Text = soInfo.ReceiveName;
            this.lblReceiveAddr.Text = soInfo.ReceiveAddress;
            this.lblReceivePhone.Text = soInfo.ReceivePhone;
            this.lblReceiveCellPhone.Text = soInfo.ReceiveCellPhone;
            this.lblReceiveZip.Text = soInfo.ReceiveZip;
            area = ASPManager.GetInstance().LoadArea(soInfo.ReceiveAreaSysNo);
            this.lblReceiveProvince.Text = area.ProvinceName;
            this.lblReceiveCity.Text = area.CityName;
            this.lblReceiveDistrict.Text = area.DistrictName;
            ShipTypeInfo stInfo = ASPManager.GetInstance().LoadShipType(soInfo.ShipTypeSysNo);
            PayTypeInfo ptInfo = ASPManager.GetInstance().LoadPayType(soInfo.PayTypeSysNo);
            this.lblShipType.Text = stInfo.ShipTypeName;
            this.lblPayType.Text = ptInfo.PayTypeName;
            this.lblPointAmt.Text = soInfo.PointAmt.ToString();
            this.lblPointPay.Text = soInfo.PointPay.ToString();
            this.lblFreeShipFeePay.Text = Util.ToMoney(soInfo.FreeShipFeePay).ToString();
            this.lblPremiumAmt.Text = soInfo.PremiumAmt.ToString(AppConst.DecimalFormatWithCurrency);
            this.lblShipPrice.Text = soInfo.ShipPrice.ToString(AppConst.DecimalFormatWithCurrency);
            this.lblPayPrice.Text = soInfo.PayPrice.ToString(AppConst.DecimalFormatWithCurrency);
            if (soInfo.PayPrice > 0)
                this.trPayPrice.Visible = true;
            else
                this.trPayPrice.Visible = false;
            this.lblSOAmt.Text = soInfo.SOAmt.ToString(AppConst.DecimalFormatWithCurrency);
            //this.lblDiscountAmt.Text = soInfo.DiscountAmt.ToString(AppConst.DecimalFormatWithCurrency);
            this.lblDiscountAmt.Text = Convert.ToDecimal(soInfo.DiscountAmt + soInfo.CouponAmt).ToString(AppConst.DecimalFormatWithCurrency);
            //if(soInfo.DiscountAmt>0)
            //    this.trSaleRule.Visible = true;
            //else
            //    this.trSaleRule.Visible = false;
            if ((soInfo.DiscountAmt + soInfo.CouponAmt) > 0)
                this.trSaleRule.Visible = true;
            else
                this.trSaleRule.Visible = false;

            decimal total = soInfo.GetTotalAmt();
            decimal subTotal = total - soInfo.PayPrice;
            decimal endMoney = SaleManager.GetInstance().GetEndMoney(soInfo);
            decimal change = total - endMoney;
            this.lblSubSum.Text = subTotal.ToString(AppConst.DecimalFormatWithCurrency);
            this.lblTotalMoney.Text = endMoney.ToString(AppConst.DecimalFormatWithCurrency);
            this.lblChange.Text = change.ToString(AppConst.DecimalFormatWithCurrency);
            this.lblSOWeight.Text = soInfo.GetTotalWeight().ToString();
            this.lblStatus.Text = AppEnum.GetSOStatus(soInfo.Status);
            this.lblSODate.Text = soInfo.OrderDate.ToString(AppConst.DateFormat);
            if (soInfo.Memo == "")
            {
                lblMemo.Text = "没有留言！";
                txtMessage.Text = "没有留言！";
            }
            else
            {
                this.lblMemo.Text = soInfo.Memo;
                txtMessage.Text = soInfo.Memo;
            }
            //Bind SOItem Table
            this.BindSOItem();
            //Bind SaleRule Table
            this.BindSaleRule();
            //Set visible
            if (soInfo.Status == (int)AppEnum.SOStatus.Origin)
                this.btnCancel.Visible = true;
            else
                this.btnCancel.Visible = false;

            if (soInfo.HasExpectQty == (int)AppEnum.YNStatus.Yes)
                this.dgItem.Columns[5].Visible = true;
            else
                this.dgItem.Columns[5].Visible = false;
        }

        private void BindSOItem()
        {
            DataTable dt = new DataTable("SOItemTable");
            dt.Columns.Add(new DataColumn("ProductSysNo", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("ProductName", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Price", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Quantity", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("Weight", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("SubTotalAmt", typeof(System.String)));
            dt.Columns.Add(new DataColumn("SubTotalWeight", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("ExpectQty", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("Warranty", typeof(System.String)));

            if (soInfo.ItemHash.Count != 0)
            {
                Hashtable sysNoHash = new Hashtable(5);
                foreach (SOItemInfo soItem in soInfo.ItemHash.Values)
                {
                    DataRow dr = dt.NewRow();
                    dr["ProductSysNo"] = soItem.ProductSysNo;
                    if (soItem.PointType == (int)AppEnum.ProductPayType.PointPayOnly)
                    {
                        dr["Price"] = Convert.ToInt32(soItem.Price * AppConst.ExchangeRate).ToString() + "积分（仅积分支付）";
                        dr["SubTotalAmt"] = Convert.ToInt32(soItem.Price * AppConst.ExchangeRate * soItem.Quantity).ToString() + "积分";
                    }
                    else
                    {
                        dr["Price"] = soItem.Price.ToString(AppConst.DecimalFormatWithCurrency);
                        dr["SubTotalAmt"] = ((decimal)(soItem.Price * soItem.Quantity)).ToString(AppConst.DecimalFormatWithCurrency);
                    }
                    dr["Quantity"] = soItem.Quantity.ToString();

                    if (soItem.ExpectQty > 0)
                    {
                        dr["ExpectQty"] = soItem.ExpectQty.ToString();
                    }
                    else
                    {
                        dr["ExpectQty"] = dr["Quantity"];
                    }

                    dr["Weight"] = soItem.Weight.ToString();
                    dr["SubTotalWeight"] = soItem.Weight * soItem.Quantity;
                    dr["Warranty"] = Util.TrimNull(soItem.Warranty);
                    dt.Rows.Add(dr);
                    sysNoHash.Add(soItem.ProductSysNo, null);
                }
                Hashtable pbHash = ProductManager.GetInstance().GetProductBoundle(sysNoHash);
                foreach (ProductBasicInfo pbInfo in pbHash.Values)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if ((int)dr["ProductSysNo"] == pbInfo.SysNo)
                        {
                            if (pbInfo.ProductSize > 0)
                                dr["ProductName"] = pbInfo.ProductName + ((Size2Info)CategoryManager.GetInstance().GetSize2Hash()[pbInfo.ProductSize]).ToString();
                            else
                                dr["ProductName"] = pbInfo.ProductName;
                            break;
                        }
                    }
                }
            }
            dgItem.DataSource = dt;
            dgItem.DataBind();
        }

        private void BindSaleRule()
        {
            DataTable dt = new DataTable("SaleRuleTable");
            dt.Columns.Add(new DataColumn("SaleRuleName", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Discount", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Times", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("SubTotal", typeof(System.String)));
            if (soInfo.SaleRuleHash.Count != 0)
            {
                foreach (SOSaleRuleInfo srInfo in soInfo.SaleRuleHash.Values)
                {
                    DataRow dr = dt.NewRow();
                    dr["SaleRuleName"] = srInfo.SaleRuleName;
                    dr["Discount"] = srInfo.Discount.ToString(AppConst.DecimalFormatWithCurrency);
                    dr["Times"] = srInfo.Times.ToString();
                    dr["SubTotal"] = ((decimal)(srInfo.Discount * srInfo.Times)).ToString(AppConst.DecimalFormatWithCurrency);
                    dt.Rows.Add(dr);
                }
            }
            dgSaleRule.DataSource = dt;
            dgSaleRule.DataBind();
            if (dt.Rows.Count != 0)
                this.trSRData.Visible = true;
            else
                this.trSRData.Visible = false;
        }

        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            try
            {
                SaleManager.GetInstance().AbandonSO_Customer(soInfo);
                oSession.sCustomer = CustomerManager.GetInstance().Load(oSession.sCustomer.SysNo);
                this.BindPage();
            }
            catch (BizException be)
            {
                lblmsg.Text = be.Message;
                ErrorLog.GetInstance().Write(be.Message);
            }
            catch (Exception ex)
            {
                ErrorLog.GetInstance().Write(ex.Message);
            }
        }

        protected void btnEnter_Click(object sender, EventArgs e)
        {
            if (txtTradeNo.Text != string.Empty)
            {
                int Result = 0;
                bool IsAdd = true;
                int SOSysNo = Int32.Parse(lblSOID.Text.Substring(1));
                SOAlipayInfo oInfo = SaleManager.GetInstance().LoadSOAlipay(SOSysNo);
                if (oInfo == null)
                {
                    oInfo = new SOAlipayInfo();
                    oInfo.SOSysNo = SOSysNo;
                    oInfo.AlipayTradeNo = txtTradeNo.Text.Trim();
                    Result = SaleManager.GetInstance().InsertSOAlipay(oInfo);
                    lblResult.Text = "输入支付宝交易号完成!";
                }
                else
                {
                    IsAdd = false;
                    oInfo.AlipayTradeNo = txtTradeNo.Text.Trim();
                    Result = SaleManager.GetInstance().UpdateSOAlipay(oInfo);
                    lblResult.Text = "更新支付宝交易号完成!";
                }

                if (Result == 1)
                {
                    SOInfo order = SaleManager.GetInstance().LoadSOMaster(SOSysNo);
                    if (order != null)
                    {
                        if (IsAdd)
                        {
                            NetPayInfo netpay = new NetPayInfo();

                            netpay.SOSysNo = SOSysNo;
                            netpay.PayTypeSysNo = order.PayTypeSysNo;
                            netpay.InputTime = DateTime.Now;
                            netpay.Source = (int)AppEnum.NetPaySource.Bank;
                            netpay.PayAmount = order.GetTotalAmt();
                            netpay.Status = (int)AppEnum.NetPayStatus.Origin;
                            netpay.Note = "支付宝即时到帐付款";
                            NetPayManager.GetInstance().Insert(netpay);
                        }
                    }
                    lblResult.Visible = true;
                }
            }
        }

        protected void dgItem_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            //Icson.Utils.DataGridUtil.GetInstance().setgridstyle(dgItem, e);
        }

        protected void dgSaleRule_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            //Icson.Utils.DataGridUtil.GetInstance().setgridstyle(dgItem, e);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            //以下是修改订单信息。
            try
            {
                soInfo.SysNo = soSysNo;
                soInfo.ReceiveAddress = txtRaddress.Text.Trim();
                soInfo.ReceiveName = txtRname.Text.Trim();
                soInfo.ReceivePhone = txtRPhone.Text.Trim();
                soInfo.ReceiveCellPhone = txtRCellPhone.Text.Trim();
                soInfo.ReceiveZip = txtRzip.Text.Trim();
                soInfo.UpdateTime = DateTime.Now;
                int ShipTypeSysNo = 0; ;
                foreach (DataGridItem dgItem in dgShipType.Items)
                {
                    RadioButton rdo = dgItem.FindControl("rdoSelectShipType") as RadioButton;
                    if (rdo.Checked)
                    {
                        ShipTypeSysNo = Int32.Parse(dgShipType.DataKeys[dgItem.ItemIndex].ToString());
                        break;
                    }
                }
                soInfo.ShipTypeSysNo = ShipTypeSysNo;
                int PayTypeSysNo = 0;
                foreach (DataGridItem dgItem in dgPayType.Items)
                {
                    RadioButton rdo = dgItem.FindControl("rdoSelectPayType") as RadioButton;
                    if (rdo.Checked)
                    {
                        PayTypeSysNo = Int32.Parse(dgPayType.DataKeys[dgItem.ItemIndex].ToString());
                        break;
                    }
                }
                soInfo.PayTypeSysNo = PayTypeSysNo;
                soInfo.ReceiveAreaSysNo = this.RArea.AreaSysNo;
                //soInfo.IsPremium = this.chkPremium.Checked ? (int)AppEnum.YNStatus.Yes : (int)AppEnum.YNStatus.No;
                //soInfo.IsWholeSale = this.chkWholeSale.Checked ? (int)AppEnum.YNStatus.Yes : (int)AppEnum.YNStatus.No;
                //soInfo.IsVAT = this.chkVAT.Checked ? (int)AppEnum.YNStatus.Yes : (int)AppEnum.YNStatus.No;
                //soInfo.PointPay = int.Parse(txtPointPay.Text.Trim());
                //soInfo.FreeShipFeePay = decimal.Parse(txtFreeShipFeePay.Text.Trim());
                //soInfo.Note = txtNote.Text.Trim();
                //soInfo.Note = lblNote.Text.Trim();
                //soInfo.InvoiceNote = txtInvoiceNote.Text.Trim();
                soInfo.Memo = txtMessage.Text.Trim();  //备注信息
                //if (soInfo.ExpectDeliveryDate == AppConst.DateTimeNull)
                //{
                //    soInfo.ExpectDeliveryDate = ucAuditDeliveryTime.DeliveryDate;
                //    soInfo.ExpectDeliveryTimeSpan = ucAuditDeliveryTime.TimeSpan;
                //}
                //soInfo.AuditDeliveryDate = ucAuditDeliveryTime.DeliveryDate;
                //soInfo.AuditDeliveryTimeSpan = ucAuditDeliveryTime.TimeSpan;
                SaleManager.GetInstance().CalcSO(soInfo);
                //lblCalShipPrice.Text = Util.ToMoney(SaleManager.GetInstance().CalcShipPriceOrigin(soInfo)).ToString();
                //if (!this.chkAutoShipPrice.Checked)
                //{
                //    soInfo.ShipPrice = Util.ToMoney(txtShipPrice.Text.Trim());
                //    soInfo.CashPay = Util.ToMoney(soInfo.GetCashPay());
                SaleManager.GetInstance().CalcPayPrice(soInfo);
                //}
                if (soInfo.SysNo != AppConst.IntNull)
                {
                    //soInfo.UpdateUserSysNo = this.sInfo.User.SysNo;
                    soInfo.UpdateTime = DateTime.Now;
                    SaleManager.GetInstance().UpdateSO(soInfo);
                    //LogManager.GetInstance().Write(new LogInfo(soInfo.SysNo, (int)AppEnum.LogType.Sale_SO_Update, this.oSession));
                    //this.GetCustomerInfo(true);
                }
                Assert(lblmsg, "Save OK", 1);
            }
            catch (BizException be)
            {
                Assert(lblmsg, be.Message, -1);
            }
            catch
            {
                Assert(lblmsg, "Save SO failed", -1);
            }


        }

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
        protected void dgShipType_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            Icson.Utils.DataGridUtil.GetInstance().setgridstyle(dgShipType, e);
        }
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
        protected void dgPayType_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            Icson.Utils.DataGridUtil.GetInstance().setgridstyle(dgPayType, e);
        }



        private void InitSOMaster()
        {
            SortedList shipTypeList = ASPManager.GetInstance().GetShipTypeListByArea(oSession.sSO.ReceiveAreaSysNo);
            //默认选中第一种在线显示的运送方式
            if (shipTypeList != null && shipTypeList.Count != 0)
            {
                foreach (ShipTypeInfo stInfo in shipTypeList.Keys)
                {
                    if (stInfo.IsOnlineShow == (int)AppEnum.YNStatus.Yes)
                    {
                        oSession.sSO.ShipTypeSysNo = stInfo.SysNo;
                        break;
                    }
                }
            }
        }
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
                            //        dr["PremiumAmt"] = ASPManager.GetInstance().GetPremuimAmt(soInfo.SOAmt, stInfo.SysNo).ToString(AppConst.DecimalFormatWithCurrency);
                            //        dr["OrderNumber"] = stInfo.OrderNumber;
                            //        dt.Rows.Add(dr);

                            //    }
                            //}
                            //else
                            //{
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
                                dr["PremiumAmt"] = ASPManager.GetInstance().GetPremuimAmt(soInfo.SOAmt, stInfo.SysNo).ToString(AppConst.DecimalFormatWithCurrency);
                                dr["OrderNumber"] = stInfo.OrderNumber;
                                dt.Rows.Add(dr);

                            }
                            //}
                        }
                        catch (BizException be)
                        {
                            this.lblRErrMsg.Text = be.Message;
                        }
                        catch (Exception ex)
                        {
                            ErrorLog.GetInstance().Write(ex.Message);
                        }
                    }
                }
            }
            this.dgShipType.DataSource = dt;
            this.dgShipType.DataBind();
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

                    if (soInfo.ShipTypeSysNo == AppConst.IntNull)
                    {
                        if (dgItem.ItemIndex == 0)
                        {
                            rdo.Checked = true;
                            //break;
                        }
                    }
                    else if ((int)dgShipType.DataKeys[dgItem.ItemIndex] == soInfo.ShipTypeSysNo)
                    {
                        rdo.Checked = true;
                        //break;
                    }
                }
            }
        }
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

                    if (soInfo.PayTypeSysNo == AppConst.IntNull)
                    {
                        if (dgItem.ItemIndex == 0)
                        {
                            rdo.Checked = true;
                            //break;
                            //dgPayType.SelectedIndex = dgItem.ItemIndex;
                        }
                    }
                    else if ((int)dgPayType.DataKeys[dgItem.ItemIndex] == soInfo.PayTypeSysNo)
                    {
                        rdo.Checked = true;
                        //dgPayType.SelectedIndex = dgItem.ItemIndex;
                        //break;
                    }
                }
            }
        }

    }
}