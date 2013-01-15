
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
                    if (oCustomer.DwellAreaSysNo != 1)//�����new user��Ĭ��ֵ��
                        scArea.AreaSysNo = oCustomer.DwellAreaSysNo;
                    txtDwellAddress.Text = oCustomer.DwellAddress;
                    txtZip.Text = oCustomer.DwellZip;
                    txtFax.Text = oCustomer.Fax;
                    lblEmail.Text = oCustomer.Email + "(�޸���Ҫȡ����֤)";
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
                lblErrMsg.Text = "����������������";
                return;
            }

            if (txtNickname.Text.Trim() == "")
            {
                lblErrMsg.Text = "�����������ǳƣ�";
                return;
            }

            if (txtDwellAddress.Text.Trim() == "")
            {
                lblErrMsg.Text = "��������ϵ��ַ��";
                return;
            }

            if (txtPhone.Text.Trim() == "")
            {
                lblErrMsg.Text = "��������ϵ�绰��";
                return;
            }

            if (scArea.DistrictSysNo == AppConst.IntNull)
            {
                lblErrMsg.Text = "��ѡ�����(����ʡ���С�����)";
                return;
            }

            if (txtCellPhone.Text.Trim() != "")
            {
                if (!Util.IsCellNumber(txtCellPhone.Text.Trim()))
                {
                    lblErrMsg.Text = "�ֻ������ʽ����ȷ�������Բ�����";
                    return;
                }
            }

            if (txtZip.Text.Trim() == "")
            {
                lblErrMsg.Text = "�������������룡";
                return;
            }

            if (txtZip.Text.Trim() != "")
            {
                if (!Util.IsZipCode(txtZip.Text.Trim()))
                {
                    lblErrMsg.Text = "���������ʽ����ȷ��";
                    return;
                }
            }

            if (String.IsNullOrEmpty(txtBirthDay.Text.ToString()))
            {
                lblErrMsg.Text = "���ղ���Ϊ��";
                return;
            }

            if (!Util.IsEmailAddress(txtEmail.Text.Trim()))
            {
                lblErrMsg.Text = "Email��ַ��ʽ����ȷ";
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
                string url = "../CustomError.aspx?msg=" + Server.UrlEncode("ȱ��ע����Ϣ�����ܸ��»����ע�ᣬ���¼");
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
                lblErrMsg.Text = "���¸�����Ϣ�ɹ�";
            }
            catch (BizException exp)
            {
                lblErrMsg.Text = exp.Message;
            }
            catch (Exception exp)
            {
                ErrorLog.GetInstance().Write(exp.ToString());
                string url = "../CustomError.aspx?msg=" + Server.UrlEncode("�����û���Ϣʧ��");
                Response.Redirect(url);
            }

            if (this.opt == "New" && isOK)
            {

                //��ע���û�����ϵ��������ΪĬ�ϵ��ջ���ַ��ӵ��ջ���ַ����
                CustomerAddressInfo caInfo = new CustomerAddressInfo();
                caInfo.CustomerSysNo = oCustomer.SysNo;
                caInfo.Brief = "�ջ���ַ1";
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