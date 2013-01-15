<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Payment.Master" AutoEventWireup="true"
    CodeBehind="Comfirm.aspx.cs" Inherits="YoeJoyWeb.Shopping.Comfirm" %>

<%@ Register TagPrefix="sc1" Namespace="YoeJoyWeb.Controls" Assembly="YoeJoyWeb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link type="text/css" rel="Stylesheet" href="../static/css/base.css" />
    <link type="text/css" rel="Stylesheet" href="../static/css/process1.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
    <script type="text/javascript">
        function SelectAddress(obj) {
            for (var i = 0; i < document.Form1.elements.length; i++) {
                var item = document.Form1.elements[i];
                if (item.type == 'radio') {
                    if (item == obj) {
                        item.checked = true;
                    }
                    else
                        item.checked = false;
                }
            }
        }
    </script>
    <div id="content">
        <div id="process">
            <img src="../static/images/step2.png" />
        </div>
        <div class="consigneeInfo">
            <h2 class="titles">
                收货地址
            </h2>
            <div class="shoppingstep_content">
                <table width="900" align="center">
                    <tr>
                        <td height="10" align="left">
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            &nbsp;&nbsp;&nbsp;&nbsp;<strong>请选择您本订单的收货地址</strong>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 840px; padding-left: 40px">
                            <asp:DropDownList ID="ddlAddresses" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlAddresses_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td height="10">
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="height: 22px">
                            &nbsp;&nbsp;&nbsp;&nbsp;<strong>收货地址如需调整，请直接在下面表格中修改</strong>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="width: 840px">
                            <table width="96.8%" border="1" cellspacing="0" cellpadding="0" bordercolor="#b1d0ff"
                                style="border-collapse: collapse">
                                <tr>
                                    <td style="height: 30px" width="80">
                                        <div align="right">
                                            地址类型：</div>
                                    </td>
                                    <td align="left">
                                        &nbsp;&nbsp;<asp:TextBox ID="txtBrief" runat="server"></asp:TextBox>&nbsp; 例如：公司、家庭，便于您选择收货地址
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 30px" width="80">
                                        <div align="right">
                                            <asp:Label ID="lblSysNo" runat="server" Visible="False">1</asp:Label>
                                            <asp:Label ID="lblIsDefault" runat="server" Visible="False">-1</asp:Label>
                                            <asp:Label ID="lblCustomerSysNo" runat="server" Visible="False">0</asp:Label>收货人姓名：
                                        </div>
                                    </td>
                                    <td style="height: 30px" align="left">
                                        &nbsp;&nbsp;<asp:TextBox ID="txtName" runat="server"></asp:TextBox><font face="宋体">
                                            <span class="font9"><span id="custnamemsg"><font color="#ff0000">*</font></span> 请填写准确的姓名</span></font>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 30px">
                                        <div align="right">
                                            联系人：</div>
                                    </td>
                                    <td style="height: 30px" align="left">
                                        &nbsp;&nbsp;<asp:TextBox ID="txtContact" runat="server"></asp:TextBox><font face="宋体">
                                            <span class="font9"><span id="Span1"><font color="#ff0000">*</font></span> 请填写准确的联系人信息</span></font>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="height: 30px">
                                        <div align="right">
                                            <font face="宋体">手机：</font></div>
                                    </td>
                                    <td align="left" style="height: 25px">
                                        &nbsp;&nbsp;<asp:TextBox ID="txtCellPhone" runat="server"></asp:TextBox><span style="color: #ff0000">(请填写准确的手机号码，以便接收订单相关的免费短信)</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="height: 30px">
                                        <div align="right">
                                            联系电话：</div>
                                    </td>
                                    <td align="left" style="height: 30px">
                                        &nbsp;&nbsp;<asp:TextBox ID="txtPhone" runat="server"></asp:TextBox>
                                        <span class="font9"><span id="phonemsg"><font color="#ff0000">*</font></span> 可以填写多个联系电话，中间用","隔开</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="height: 30px" valign="middle">
                                        <div align="right">
                                            地区：</div>
                                    </td>
                                    <td align="left" style="height: 30px">
                                        <sc1:Area ID="scArea" runat="server" AutoPostBack="False">
                                        </sc1:Area>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 30px">
                                        <div align="right">
                                            联系地址：</div>
                                    </td>
                                    <td align="left" style="height: 30px">
                                        &nbsp;&nbsp;<asp:TextBox ID="txtAddress" runat="server" Width="400px"></asp:TextBox>&nbsp;
                                        <span class="font9"><span id="addressmsg"><font color="#ff0000">*</font></span> 请尽量详细填写该地址信息！</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 30px">
                                        <div align="right">
                                            邮编：</div>
                                    </td>
                                    <td align="left" style="height: 30px">
                                        &nbsp;&nbsp;<asp:TextBox ID="txtZip" runat="server"></asp:TextBox>
                                        <span style="color: #ff0000">*</span> 请填写准确的邮编
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 30px">
                                        <div align="right">
                                            传真：</div>
                                    </td>
                                    <td align="left" style="height: 30px">
                                        &nbsp;&nbsp;<asp:TextBox ID="txtFax" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2" style="height: 30px">
                                        <div align="left">
                                            &nbsp;&nbsp;<strong>注意：</strong>带<font color="#ff0000">*</font>的栏目必须填写！</div>
                                    </td>
                                </tr>
                            </table>
                            <asp:Label ID="lblErrMsg" runat="server" EnableViewState="False" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 840px; text-align: center;">
                            <div style="margin: 10px 0 0 320px;">
                                <asp:ImageButton ID="ImageButton1" Style="padding-right: 10px;" OnClick="btnShoppingCart_Click"
                                    runat="server" ImageUrl="~/static/images/gwc.jpg" />
                                <asp:ImageButton ID="imgnext" OnClick="btnNext_Click" runat="server" ImageUrl="~/static/images/next.jpg" />
                                <div class="fixed">
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td height="10" align="left">
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ModuleContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BackupContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="BackupContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
</asp:Content>
