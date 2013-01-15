using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YoeJoyHelper.Security;
using Icson.Utils;
using Icson.Objects;
using Icson.Objects.Online;
using Icson.Objects.Basic;
using Icson.BLL.Basic;
using YoeJoyHelper;
using System.Data;

namespace YoeJoyWeb.User
{
    public partial class MyAddress : SecurityPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindAddressList();
                scArea.BindArea();
                btnSubmit.Enabled = false;
                AddressInfo.Visible = false;
                if (Request.QueryString["Type"] != null && Request.QueryString["Type"] == "New")
                {
                    AddressInfo.Visible = true;
                    btnSubmit.Enabled = true;
                    btnSubmit.Text = "新增收货地址";
                }
            }
        }

        private void BindAddressList()
        {
            IcsonSessionInfo oSession = CommonUtility.GetUserSession(Context);
            if (oSession.sCustomer == null || oSession.sCustomer.SysNo == AppConst.IntNull)
            {
                Response.Redirect("login.aspx");
            }

            int customerSysNo = oSession.sCustomer.SysNo;
            lblCustomerSysNo.Text = customerSysNo.ToString();
            DataSet ds = CustomerManager.GetInstance().GetCustomerAddressDs(customerSysNo);
            DataList1.DataSource = ds;
            DataList1.DataBind();
        }

        protected void DataList1_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int sysno = Int32.Parse(DataList1.DataKeys[e.Item.ItemIndex].ToString());
                if (e.CommandName == "Update")
                {
                    CustomerAddressInfo o = CustomerManager.GetInstance().LoadCustomerAddress(sysno);

                    lblSysNo.Text = sysno.ToString();
                    lblIsDefault.Text = o.IsDefault.ToString();
                    txtBrief.Text = o.Brief;
                    txtName.Text = o.Name;
                    txtContact.Text = o.Contact;
                    txtCellPhone.Text = o.CellPhone;
                    txtPhone.Text = o.Phone;
                    txtAddress.Text = o.Address;
                    scArea.AreaSysNo = o.AreaSysNo;
                    txtZip.Text = o.Zip;
                    txtFax.Text = o.Fax;

                    btnSubmit.Text = "修改收货地址";
                    btnSubmit.Enabled = true;
                    AddressInfo.Visible = true;
                }
                else if (e.CommandName == "Delete")
                {
                    CustomerManager.GetInstance().DeleteCustomerAddress(sysno);
                    AddressInfo.Visible = false;
                }
                else if (e.CommandName == "SetDefault")
                {
                    CustomerManager.GetInstance().CustomerAddressSetDefault(sysno);
                    AddressInfo.Visible = false;
                }

                BindAddressList();
            }

        }

        protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lbl1 = e.Item.FindControl("lblIsDefault") as Label;
                Label lbl2 = e.Item.FindControl("lblBrief") as Label;
                LinkButton btn = e.Item.FindControl("lnkbtnSetDefault") as LinkButton;

                int IsDefault = Int32.Parse(lbl1.Text);
                if (IsDefault == (int)AppEnum.BiStatus.Valid)
                {
                    lbl2.Text = lbl2.Text + "&nbsp;&nbsp;<strong>[默认]</strong>";
                    btn.Visible = false;
                }
                else
                    btn.Visible = true;
            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            AddressInfo.Visible = true;
            if (txtName.Text == "" || txtName.Text == null)
            {
                lblErrMsg.Text = "请输入收货人姓名！";
                return;
            }
            if (txtContact.Text == "" || txtContact.Text == null)
            {
                lblErrMsg.Text = "请输入收货联系人信息！";
                return;
            }
            if (txtPhone.Text == null || txtPhone.Text == "")
            {
                lblErrMsg.Text = "请输入收货联系电话！";
                return;
            }
            if (txtAddress.Text == "" || txtAddress.Text == null)
            {
                lblErrMsg.Text = "请输入收货联系地址！";
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

            CustomerAddressInfo o = new CustomerAddressInfo();
            int sysno = Int32.Parse(lblSysNo.Text);
            o.SysNo = sysno;
            o.CustomerSysNo = Int32.Parse(lblCustomerSysNo.Text);
            o.Brief = txtBrief.Text.Trim();
            o.Name = txtName.Text.Trim();
            o.Contact = txtContact.Text.Trim();
            o.CellPhone = txtCellPhone.Text.Trim();
            o.Phone = txtPhone.Text.Trim();
            o.Address = txtAddress.Text.Trim();
            o.AreaSysNo = scArea.AreaSysNo;
            o.Zip = txtZip.Text.Trim();
            o.Fax = txtFax.Text.Trim();
            o.IsDefault = Int32.Parse(lblIsDefault.Text);
            o.UpdateTime = DateTime.Now;

            if (sysno > 0)
            {
                CustomerManager.GetInstance().UpdateCustomerAddress(o);
            }
            else
            {
                if (CustomerManager.GetInstance().LoadCustomerAddressByCustomer(o.CustomerSysNo) == null)
                {
                    o.IsDefault = (int)AppEnum.BiStatus.Valid;
                }
                CustomerManager.GetInstance().InsertCustomerAddress(o);
            }
            BindAddressList();

            btnSubmit.Enabled = false;
            AddressInfo.Visible = false;

        }

        protected void lntbtnAdd_Click(object sender, EventArgs e)
        {
            lblSysNo.Text = "0";
            lblIsDefault.Text = Convert.ToString((int)AppEnum.BiStatus.InValid);
            txtBrief.Text = "";
            txtPhone.Text = "";
            txtAddress.Text = "";
            txtZip.Text = "";
            txtFax.Text = "";

            btnSubmit.Enabled = true;
            btnSubmit.Text = "新增收货地址";
            AddressInfo.Visible = true;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            AddressInfo.Visible = false;
            txtBrief.Text = "";
            txtPhone.Text = "";
            txtAddress.Text = "";
            txtZip.Text = "";
            txtFax.Text = "";
        }
    }
}