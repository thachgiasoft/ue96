using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Icson.Utils;
using Icson.Objects;
using Icson.Objects.Online;
using Icson.Objects.Basic;
using Icson.Objects.Sale;
using Icson.BLL.Basic;

using Icson.BLL;
using Icson.BLL.Online;
using Icson.BLL.Sale;
using YoeJoyHelper;
using YoeJoyHelper.Security;

namespace YoeJoyWeb.Shopping
{
    public partial class Comfirm :SecurityPageBase
    {
        protected SOInfo soInfo = new SOInfo();
        protected IcsonSessionInfo oSession;
        protected CustomerAddressInfo oldAddress;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckProfile(Context);
            oSession = CommonUtility.GetUserSession(Context);
            if (!IsPostBack)
            {
                scArea.BindArea();
                BindAddressList();

                InitGift();
            }
        }

        public void InitGift()
        {
            DataTable dt = BuildCartGiftTable();
            if (Util.HasMoreRow(dt))
            {
                oSession.GiftHash.Clear();
                foreach (DataRow dr in dt.Rows)
                {
                    oSession.GiftHash.Add(Util.TrimIntNull(dr["sysno"]), Util.TrimIntNull(dr["sysno"]));
                }
            }
        }

        private DataTable BuildCartGiftTable()
        {
            DataTable table = new DataTable("BuildGiftCartTable");
            DataColumn dc0 = new DataColumn("SysNo", typeof(System.Int32));
            DataColumn dc1 = new DataColumn("Quantity", typeof(System.Int32));
            DataColumn dc2 = new DataColumn("ExpectQty", typeof(System.Int32));

            table.Columns.Add(dc0);
            table.Columns.Add(dc1);
            table.Columns.Add(dc2);

            Hashtable ht = CartManager.GetInstance().GetCartHash();

            if (ht == null || ht.Count == 0)
                return table;

            DataSet ds = OnlineListManager.GetInstance().GetCartGiftDs(ht);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                DataRow drShow = table.NewRow();
                drShow["SysNo"] = (int)dr["SysNo"];
                foreach (CartInfo cartInfo in ht.Values)
                {
                    if (cartInfo.ProductSysNo == (int)dr["MasterSysNo"])
                    {
                        drShow["Quantity"] = cartInfo.Quantity;
                        drShow["ExpectQty"] = cartInfo.Quantity;
                        break;
                    }
                }
                table.Rows.Add(drShow);
            }

            return table;
        }

        /// <summary>
        /// 收货地址绑定
        /// </summary>
        private void BindAddressList()
        {

            ddlAddresses.Items.Add(new ListItem("创建新地址", "create"));

            int customerSysNo = oSession.sCustomer.SysNo;
            DataSet ds = CustomerManager.GetInstance().GetCustomerAddressDs(customerSysNo);
            if (!Util.HasMoreRow(ds))
                lblSysNo.Text = "0";

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ddlAddresses.Items.Add(new ListItem(dr["AreaName"].ToString() + dr["Address"].ToString(), dr["sysno"].ToString()));
                if (Int32.Parse(dr["IsDefault"].ToString()) == 0)
                {
                    ddlAddresses.SelectedValue = dr["sysno"].ToString();

                    int sysno = Int32.Parse(ddlAddresses.SelectedValue);
                    lblSysNo.Text = sysno.ToString();
                    CustomerAddressInfo o = CustomerManager.GetInstance().LoadCustomerAddress(sysno);
                    txtBrief.Text = o.Brief;
                    txtName.Text = o.Name;
                    txtContact.Text = o.Contact;
                    txtCellPhone.Text = o.CellPhone;
                    txtPhone.Text = o.Phone;
                    scArea.AreaSysNo = o.AreaSysNo;
                    txtAddress.Text = o.Address;
                    txtZip.Text = o.Zip;
                    txtFax.Text = o.Fax;


                }

            }

            //DataList1.DataSource = ds;
            //DataList1.DataBind();
        }

        //protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
        //{
        //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //    {
        //        Label lbl1 = e.Item.FindControl("lblIsDefault") as Label;
        //        Label lbl2 = e.Item.FindControl("lblBrief") as Label;
        //        ImageButton imgbtn = e.Item.FindControl("btnSelect") as ImageButton;

        //        int IsDefault = Int32.Parse(lbl1.Text);
        //        if (IsDefault == (int)AppEnum.BiStatus.Valid)
        //        {
        //            lbl2.Text = lbl2.Text + "[默认]";
        //            imgbtn.ImageUrl = "../Images/site/SelectedIcon.jpg";

        //            int sysno = Int32.Parse(DataList1.DataKeys[e.Item.ItemIndex].ToString());
        //            lblSysNo.Text = sysno.ToString();

        //            CustomerAddressInfo o = CustomerManager.GetInstance().LoadCustomerAddress(sysno);
        //            txtBrief.Text = o.Brief;
        //            txtName.Text = o.Name;
        //            txtContact.Text = o.Contact;
        //            txtCellPhone.Text = o.CellPhone;
        //            txtPhone.Text = o.Phone;
        //            scArea.AreaSysNo = o.AreaSysNo;
        //            txtAddress.Text = o.Address;
        //            txtZip.Text = o.Zip;
        //            txtFax.Text = o.Fax;
        //        }
        //        else
        //        {
        //            imgbtn.ImageUrl = "../Images/site/UnSelectedIcon.jpg";
        //        }
        //    }
        //}

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim() == "")
            {
                lblErrMsg.Text = "请输入收货人姓名！";
                return;
            }

            if (txtAddress.Text.Trim() == "")
            {
                lblErrMsg.Text = "请输入联系地址！";
                return;
            }

            if (txtPhone.Text.Trim() == "")
            {
                lblErrMsg.Text = "请输入联系电话！";
                return;
            }

            if (scArea.DistrictSysNo == AppConst.IntNull)
            {
                lblErrMsg.Text = "请选择地区(包括省、市、区县)";
                return;
            }

            if (txtCellPhone.Text.Trim() != "")
            {
                if (!Util.IsCellNumber(txtCellPhone.Text.Trim()))
                {
                    lblErrMsg.Text = "手机号码格式不正确，您可以不输入";
                    return;
                }
            }

            if (txtZip.Text.Trim() == "")
            {
                lblErrMsg.Text = "请输入邮政编码！";
                return;
            }
            if (txtZip.Text.Trim() != "")
            {
                if (!Util.IsZipCode(txtZip.Text.Trim()))
                {
                    lblErrMsg.Text = "邮政编码格式不正确！";
                    return;
                }
            }

            string msgLimited1 = "";  //普包、EMS限运
            string msgLimited2 = "";  //圆通航空限运

            Hashtable cartHash = CartManager.GetInstance().GetCartHash();
            if (cartHash.Count == 0)
                Response.Redirect("ShoppingCart.aspx");
            Hashtable sysNoHash = new Hashtable(5);
            foreach (CartInfo cartInfo in cartHash.Values)
            {
                sysNoHash.Add(cartInfo.ProductSysNo, null);
            }

            //笔记本、打火机 是圆通航空限运，笔记本是普包、EMS不限运
            string LimitedShip = "";  //包含1，为盘片、音箱耳机、液体、打火机；包含2为笔记本、打火机，1为普包、EMS全部限运，2为圆通航空限运

            Hashtable ProductHash = ProductManager.GetInstance().LoadProducts(sysNoHash);
            foreach (ProductBasicInfo oInfo in ProductHash.Values)
            {
                //盘片、音箱耳机、液体、打火机, 笔记本、液晶显示器、液晶电视
                //if (",62,64,67,72,73,74,83,169,170,171,186,191,196,197,198,210,214,239,287,434,111,234,386,562,576,577,578,579,580,".IndexOf("," + oInfo.C3SysNo + ",") >= 0)
                //笔记本，液晶不再限运
                if (",62,64,67,72,73,74,83,169,170,171,186,191,196,197,198,210,214,239,287,434,576,577,578,579,580,".IndexOf("," + oInfo.C3SysNo + ",") >= 0)
                {
                    msgLimited1 += oInfo.ProductName + "<br />";
                    LimitedShip += "1";
                }
                if (",234,562,434,576,577,578,579,580,".IndexOf("," + oInfo.C3SysNo + ",") >= 0)  //笔记本、打火机
                {
                    LimitedShip += "2";
                }
                if (oInfo.C3SysNo == 234) //笔记本
                {
                    msgLimited2 += oInfo.ProductName + "<br />";
                }
            }
            if (LimitedShip.Length > 0)
            {
                Session["HasLimitedShipProduct"] = LimitedShip;
                Session["HasLimitedShipProductInfo1"] = msgLimited1;
                Session["HasLimitedShipProductInfo2"] = msgLimited2;
                //msgLimited += " 属限运商品，请从购物车中删除！ <a href='http://www.icson.com/Service/NewsDetail.aspx?Type=Bulletin&ID=35' target='_blank'>查看限运说明</a>";
            }
            else
            {
                Session["HasLimitedShipProduct"] = "0";
                Session["HasLimitedShipProductInfo1"] = "";
                Session["HasLimitedShipProductInfo2"] = "";
            }

            //if (scArea.ProvinceSysNo != 1 && scArea.ProvinceSysNo != 1591 && scArea.ProvinceSysNo != 3225 && scArea.ProvinceSysNo != 2621)
            //{
            //    if (msgLimited.Length > 0)
            //    {
            //        lblErrMsg.Text = msgLimited;
            //        return;
            //    }
            //}

            //是否是新增地址状态
            if (ddlAddresses.SelectedValue == "create")
            {
                ////将原来的默认地址取消默认
                //oldAddress.IsDefault = -1;
                //CustomerManager.GetInstance().UpdateCustomerAddress();

                //新地址保存
                CustomerAddressInfo o = new CustomerAddressInfo();
                o.CustomerSysNo = oSession.sCustomer.SysNo;
                o.Brief = txtBrief.Text.Trim();
                o.Name = txtName.Text.Trim();
                o.Contact = txtContact.Text.Trim();
                o.Address = txtAddress.Text.Trim();
                o.AreaSysNo = scArea.AreaSysNo;
                o.Phone = txtPhone.Text.Trim();
                o.CellPhone = txtCellPhone.Text.Trim();
                o.Zip = txtZip.Text.Trim();
                o.Fax = txtFax.Text.Trim();
                o.IsDefault = (int)AppEnum.BiStatus.InValid;
                o.UpdateTime = DateTime.Now;

                if (lblSysNo.Text.Trim() == "")//原来没有默认地址，就将新增的设置为默认地址
                    o.IsDefault = 0;

                CustomerManager.GetInstance().InsertCustomerAddress(o);

                //新地址存入当前用户信息
                oSession.sSO = new SOInfo();
                oSession.sSO.CustomerSysNo = o.CustomerSysNo;
                oSession.sSO.ReceiveAddress = o.Address;
                oSession.sSO.ReceiveAreaSysNo = o.AreaSysNo;
                oSession.sSO.ReceiveCellPhone = o.CellPhone;
                oSession.sSO.ReceiveContact = o.Contact;
                oSession.sSO.ReceiveName = o.Name;
                oSession.sSO.ReceivePhone = o.Phone;
                oSession.sSO.ReceiveZip = o.Zip;

                Response.Redirect("CheckOut4.aspx");

            }
            else
            {
                int sysno = Int32.Parse(ddlAddresses.SelectedValue);
                CustomerAddressInfo o = CustomerManager.GetInstance().LoadCustomerAddress(sysno);
                o.Brief = txtBrief.Text.Trim();
                o.Name = txtName.Text.Trim();
                o.Contact = txtContact.Text.Trim();
                o.Address = txtAddress.Text.Trim();
                o.AreaSysNo = scArea.AreaSysNo;
                o.Phone = txtPhone.Text.Trim();
                o.CellPhone = txtCellPhone.Text.Trim();
                o.Zip = txtZip.Text.Trim();
                o.Fax = txtFax.Text.Trim();
                CustomerManager.GetInstance().UpdateCustomerAddress(o);

                oSession.sSO = new SOInfo();
                oSession.sSO.CustomerSysNo = o.CustomerSysNo;
                oSession.sSO.ReceiveAddress = o.Address;
                oSession.sSO.ReceiveAreaSysNo = o.AreaSysNo;
                oSession.sSO.ReceiveCellPhone = o.CellPhone;
                oSession.sSO.ReceiveContact = o.Contact;
                oSession.sSO.ReceiveName = o.Name;
                oSession.sSO.ReceivePhone = o.Phone;
                oSession.sSO.ReceiveZip = o.Zip;

                Response.Redirect("CheckOut.aspx");
            }


            //if(lblSysNo.Text == "0")
            //{
            //    CustomerAddressInfo o = new CustomerAddressInfo();
            //    o.CustomerSysNo = oSession.sCustomer.SysNo;
            //    o.Brief = txtBrief.Text.Trim();
            //    o.Name = txtName.Text.Trim();
            //    o.Contact = txtContact.Text.Trim();
            //    o.Address = txtAddress.Text.Trim();
            //    o.AreaSysNo = scArea.AreaSysNo;
            //    o.Phone = txtPhone.Text.Trim();
            //    o.CellPhone = txtCellPhone.Text.Trim();
            //    o.Zip = txtZip.Text.Trim();
            //    o.Fax = txtFax.Text.Trim();
            //    o.IsDefault = (int) AppEnum.BiStatus.InValid;
            //    o.UpdateTime = DateTime.Now;
            //    CustomerManager.GetInstance().InsertCustomerAddress(o);

            //    oSession.sSO = new SOInfo();
            //    oSession.sSO.CustomerSysNo = o.CustomerSysNo;
            //    oSession.sSO.ReceiveAddress = o.Address;
            //    oSession.sSO.ReceiveAreaSysNo = o.AreaSysNo;
            //    oSession.sSO.ReceiveCellPhone = o.CellPhone;
            //    oSession.sSO.ReceiveContact = o.Contact;
            //    oSession.sSO.ReceiveName = o.Name;
            //    oSession.sSO.ReceivePhone = o.Phone;
            //    oSession.sSO.ReceiveZip = o.Zip;

            //    Response.Redirect("CheckOut2.aspx");
            //}
            //else 
            //{
            //    foreach (DataListItem item in DataList1.Items)
            //    {
            //        if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.SelectedItem)
            //        {
            //            ImageButton imgbtn = item.FindControl("btnSelect") as ImageButton;
            //            if (imgbtn.ImageUrl == "../Images/site/SelectedIcon.jpg")
            //            {
            //                int sysno = Int32.Parse(DataList1.DataKeys[item.ItemIndex].ToString());
            //                CustomerAddressInfo o = CustomerManager.GetInstance().LoadCustomerAddress(sysno);
            //                o.Brief = txtBrief.Text.Trim();
            //                o.Name = txtName.Text.Trim();
            //                o.Contact = txtContact.Text.Trim();
            //                o.Address = txtAddress.Text.Trim();
            //                o.AreaSysNo = scArea.AreaSysNo;
            //                o.Phone = txtPhone.Text.Trim();
            //                o.CellPhone = txtCellPhone.Text.Trim();
            //                o.Zip = txtZip.Text.Trim();
            //                o.Fax = txtFax.Text.Trim();
            //                CustomerManager.GetInstance().UpdateCustomerAddress(o);

            //                oSession.sSO = new SOInfo();
            //                oSession.sSO.CustomerSysNo = o.CustomerSysNo;
            //                oSession.sSO.ReceiveAddress = o.Address;
            //                oSession.sSO.ReceiveAreaSysNo = o.AreaSysNo;
            //                oSession.sSO.ReceiveCellPhone = o.CellPhone;
            //                oSession.sSO.ReceiveContact = o.Contact;
            //                oSession.sSO.ReceiveName = o.Name;
            //                oSession.sSO.ReceivePhone = o.Phone;
            //                oSession.sSO.ReceiveZip = o.Zip;

            //                Response.Redirect("CheckOut2.aspx");
            //                break;
            //            }
            ////        }
            //    }

            //    //没有默认的收获地址,新增的一个设为默认
            //    CustomerAddressInfo o2 = new CustomerAddressInfo();
            //    o2.CustomerSysNo = oSession.sCustomer.SysNo;
            //    o2.Brief = txtBrief.Text.Trim();
            //    o2.Name = txtName.Text.Trim();
            //    o2.Contact = txtContact.Text.Trim();
            //    o2.Address = txtAddress.Text.Trim();
            //    o2.AreaSysNo = scArea.AreaSysNo;
            //    o2.Phone = txtPhone.Text.Trim();
            //    o2.CellPhone = txtCellPhone.Text.Trim();
            //    o2.Zip = txtZip.Text.Trim();
            //    o2.Fax = txtFax.Text.Trim();
            //    o2.IsDefault = (int)AppEnum.BiStatus.Valid;
            //    o2.UpdateTime = DateTime.Now;
            //    CustomerManager.GetInstance().InsertCustomerAddress(o2);

            //    oSession.sSO = new SOInfo();
            //    oSession.sSO.CustomerSysNo = o2.CustomerSysNo;
            //    oSession.sSO.ReceiveAddress = o2.Address;
            //    oSession.sSO.ReceiveAreaSysNo = o2.AreaSysNo;
            //    oSession.sSO.ReceiveCellPhone = o2.CellPhone;
            //    oSession.sSO.ReceiveContact = o2.Contact;
            //    oSession.sSO.ReceiveName = o2.Name;
            //    oSession.sSO.ReceivePhone = o2.Phone;
            //    oSession.sSO.ReceiveZip = o2.Zip;

            //    Response.Redirect("CheckOut2.aspx");
            //}
        }

        protected void btnShoppingCart_Click(object sender, EventArgs e)
        {
            Response.Redirect("ShoppingCart.aspx");
        }

        //protected void DataList1_ItemCommand(object source, DataListCommandEventArgs e)
        //{
        //    if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.SelectedItem)
        //    {
        //        if (e.CommandName == "Select")
        //        {
        //            foreach (DataListItem item in DataList1.Items)
        //            {
        //                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.SelectedItem)
        //                {
        //                    ImageButton imgbtn = item.FindControl("btnSelect") as ImageButton;
        //                    if(item.ItemIndex == e.Item.ItemIndex)
        //                    {
        //                        imgbtn.ImageUrl = "../Images/site/SelectedIcon.jpg";
        //                    }
        //                    else
        //                    {
        //                        imgbtn.ImageUrl = "../Images/site/UnSelectedIcon.jpg";
        //                    }
        //                }
        //            }

        //            int sysno = Int32.Parse(DataList1.DataKeys[e.Item.ItemIndex].ToString());
        //            lblSysNo.Text = sysno.ToString();
        //            CustomerAddressInfo o = CustomerManager.GetInstance().LoadCustomerAddress(sysno);
        //            txtBrief.Text = o.Brief;
        //            txtName.Text = o.Name;
        //            txtContact.Text = o.Contact;
        //            txtCellPhone.Text = o.CellPhone;
        //            txtPhone.Text = o.Phone;
        //            scArea.AreaSysNo = o.AreaSysNo;
        //            txtAddress.Text = o.Address;
        //            txtZip.Text = o.Zip;
        //            txtFax.Text = o.Fax;
        //        }
        //    }
        //}

        //protected void lnkbtnNewAddress_Click(object sender, EventArgs e)
        //{
        //    foreach (DataListItem item in DataList1.Items)
        //    {
        //        if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.SelectedItem)
        //        {
        //            ImageButton imgbtn = item.FindControl("btnSelect") as ImageButton;
        //            imgbtn.ImageUrl = "../Images/site/UnSelectedIcon.jpg";
        //        }
        //    }

        //    lblSysNo.Text = "0";

        //    txtBrief.Text = "";
        //    txtName.Text = "";
        //    txtCellPhone.Text = "";
        //    txtPhone.Text = "";
        //    txtAddress.Text = "";
        //    scArea.AreaSysNo = AppConst.IntNull;
        //    txtZip.Text = "";
        //    txtFax.Text = "";
        //}

        /// <summary>
        /// 选择地址
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlAddresses_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlAddresses.SelectedValue == "create")//创建新地址
            {

                oldAddress = CustomerManager.GetInstance().LoadCustomerAddress(Int32.Parse(lblSysNo.Text));

                lblSysNo.Text = "0";

                txtBrief.Text = "";
                txtName.Text = "";
                txtCellPhone.Text = "";
                txtPhone.Text = "";
                txtAddress.Text = "";
                scArea.AreaSysNo = AppConst.IntNull;
                txtZip.Text = "";
                txtFax.Text = "";
            }
            else //选择其他地址
            {

                int sysno = Int32.Parse(ddlAddresses.SelectedValue);
                lblSysNo.Text = sysno.ToString();
                CustomerAddressInfo o = CustomerManager.GetInstance().LoadCustomerAddress(sysno);
                txtBrief.Text = o.Brief;
                txtName.Text = o.Name;
                txtContact.Text = o.Contact;
                txtCellPhone.Text = o.CellPhone;
                txtPhone.Text = o.Phone;
                scArea.AreaSysNo = o.AreaSysNo;
                txtAddress.Text = o.Address;
                txtZip.Text = o.Zip;
                txtFax.Text = o.Fax;


            }
        }

    }
}