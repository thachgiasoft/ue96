
using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using System.Collections;
using Icson.Objects;
using Icson.Utils;


using Icson.Objects.Basic;
using Icson.Objects.Online;

using Icson.BLL;
using Icson.BLL.Basic;

namespace YoeJoyWeb.Controls
{
    /// <summary>
    ///		Summary description for Register1.
    /// </summary>
    public partial class CustomerDetailInfo : System.Web.UI.UserControl
    {
        private string opt;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            IcsonSessionInfo oSession = (IcsonSessionInfo)Session["IcsonSessionInfo"];
            if (oSession == null)
            {
                oSession = new IcsonSessionInfo();
                Session["IcsonSessionInfo"] = oSession;
            }

            if (oSession.sCustomer == null || oSession.sCustomer.SysNo == AppConst.IntNull)
            {
                string url = "login.aspx";
                Response.Redirect(url);
            }

            if (Request.QueryString["Type"] != null)
            {
                this.opt = Request.QueryString["Type"].ToString();
            }

            CustomerInfo oCustomer = oSession.sCustomer;
            if (oCustomer.EmailStatus == (int)AppEnum.EmailStatus.Confirmed)
            {
                lblEmail.Visible = true;
                txtEmail.Visible = false;
            }
            else
            {
                lblEmail.Visible = false;
                txtEmail.Visible = true;
            }

            if (!Page.IsPostBack)
            {
                lbUserId.Text = oSession.sCustomer.CustomerID;
                DatePickerUtil.GetInstance().setDatePickerBox(txtBirthDay);
                scArea.BindArea();
                if (this.opt != "reg")
                {

                    txtName.Text = oCustomer.CustomerName;
                    rblGeneder.SelectedIndex = rblGeneder.Items.IndexOf(rblGeneder.Items.FindByValue(oCustomer.Gender.ToString().Trim()));
                    txtCellPhone.Text = oCustomer.CellPhone;
                    txtPhone.Text = oCustomer.Phone;
                    if (oCustomer.DwellAreaSysNo != 1)//这个是new user的默认值。
                        scArea.AreaSysNo = oCustomer.DwellAreaSysNo;
                    txtDwellAddress.Text = oCustomer.DwellAddress;
                    txtZip.Text = oCustomer.DwellZip;
                    txtFax.Text = oCustomer.Fax;
                    lblEmail.Text = oCustomer.Email + "(修改需要取消认证)";
                    txtBirthDay.Text = oCustomer.BirthDay;
                    txtNickname.Text = oCustomer.NickName;
                }
                txtEmail.Text = oCustomer.Email;

            }
        }


        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion
        protected void btnSave_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (txtName.Text.Trim() == "")
            {
                lblErrMsg.Text = "请输入您的姓名！";
                return;
            }

            if (txtNickname.Text.Trim() == "")
            {
                lblErrMsg.Text = "请输入您的昵称！";
                return;
            }

            if (txtDwellAddress.Text.Trim() == "")
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

            if (String.IsNullOrEmpty(txtBirthDay.Text.ToString()))
            {
                lblErrMsg.Text = "生日不能为空";
                return;
            }

            if (!Util.IsEmailAddress(txtEmail.Text.Trim()))
            {
                lblErrMsg.Text = "Email地址格式不正确";
                return;
            }

            IcsonSessionInfo oSession = (IcsonSessionInfo)Session["IcsonSessionInfo"];
            if (oSession == null)
            {
                oSession = new IcsonSessionInfo();
                Session["IcsonSessionInfo"] = oSession;
            }

            if (oSession.sCustomer == null || oSession.sCustomer.SysNo == AppConst.IntNull)
            {
                string url = "../CustomError.aspx?msg=" + Server.UrlEncode("缺少注册信息，不能更新或继续注册，请登录");
                Response.Redirect(url);
            }

            bool isOK = false;
            CustomerInfo oCustomer = oSession.sCustomer;
            try
            {
                oCustomer.CustomerName = txtName.Text.Trim();
                oCustomer.NickName = txtNickname.Text.Trim();
                oCustomer.Gender = int.Parse(rblGeneder.SelectedItem.Value.Trim());
                oCustomer.Phone = txtPhone.Text.Trim();
                oCustomer.CellPhone = txtCellPhone.Text.Trim();
                oCustomer.Fax = txtFax.Text.Trim();
                oCustomer.DwellAreaSysNo = scArea.AreaSysNo;
                oCustomer.DwellZip = txtZip.Text.Trim();
                oCustomer.DwellAddress = txtDwellAddress.Text.Trim();
                oCustomer.Email = txtEmail.Text.Trim();
                oCustomer.BirthDay = txtBirthDay.Text.Trim();

                Hashtable ht = new Hashtable(20);

                ht.Add("SysNo", oCustomer.SysNo);
                ht.Add("CustomerName", oCustomer.CustomerName);
                ht.Add("Nickname", oCustomer.NickName);
                ht.Add("Gender", oCustomer.Gender);
                ht.Add("Phone", oCustomer.Phone);
                ht.Add("CellPhone", oCustomer.CellPhone);
                ht.Add("Fax", oCustomer.Fax);
                ht.Add("DwellAreaSysNo", oCustomer.DwellAreaSysNo);
                ht.Add("DwellZip", oCustomer.DwellZip);
                ht.Add("DwellAddress", oCustomer.DwellAddress);
                ht.Add("Email", oCustomer.Email);
                ht.Add("Birthday", oCustomer.BirthDay);
                CustomerManager.GetInstance().Update(ht);

                isOK = true;
                lblErrMsg.Text = "更新个人信息成功";
            }
            catch (BizException exp)
            {
                lblErrMsg.Text = exp.Message;
            }
            catch (Exception exp)
            {
                ErrorLog.GetInstance().Write(exp.ToString());
                string url = "../CustomError.aspx?msg=" + Server.UrlEncode("更新用户信息失败");
                Response.Redirect(url);
            }

            if (this.opt == "New" && isOK)
            {

                //新注册用户，联系人资料作为默认的收货地址添加到收货地址表中
                CustomerAddressInfo caInfo = new CustomerAddressInfo();
                caInfo.CustomerSysNo = oCustomer.SysNo;
                caInfo.Brief = "收货地址1";
                caInfo.Contact = txtName.Text.Trim();
                caInfo.Name = txtName.Text.Trim();
                caInfo.Phone = txtPhone.Text.Trim();
                caInfo.CellPhone = txtCellPhone.Text.Trim();
                caInfo.Fax = txtFax.Text.Trim();
                caInfo.AreaSysNo = scArea.AreaSysNo;
                caInfo.Zip = txtZip.Text.Trim();
                caInfo.Address = txtDwellAddress.Text.Trim();
                caInfo.IsDefault = (int)AppEnum.BiStatus.Valid;
                caInfo.UpdateTime = DateTime.Now;
                CustomerManager.GetInstance().InsertCustomerAddress(caInfo);

                Response.Redirect("../Account/AddressManagement.html");
                //Response.Redirect("Register2.aspx?opt=reg");
            }
        }
    }
}