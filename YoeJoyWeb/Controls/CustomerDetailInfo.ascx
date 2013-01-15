<%@ Control Language="c#" Inherits="YoeJoyWeb.Controls.CustomerDetailInfo" CodeBehind="CustomerDetailInfo.ascx.cs" %>
<%@ Register TagPrefix="sc1" Namespace="YoeJoyWeb.Controls" Assembly="YoeJoyWeb" %>
<script type="text/javascript" src="../Controls/My97DatePicker/WdatePicker.js"></script>
<div class="fillInfo">
    <h2 class="titles">
        <b>编辑个人资料</b></h2>
    <p>
        <label>
            用户名:</label><span><asp:Label ID="lbUserId" runat="server"></asp:Label></span></p>
    <p>
        <label>
            您的Email:</label>
        <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox><asp:Label ID="lblEmail"
            runat="server"></asp:Label></p>
    <p>
        <label>
            <b>*</b>昵称:</label><asp:TextBox ID="txtNickname" runat="server"></asp:TextBox></p>
    <p>
        <label>
            <b>*</b>真实姓名</label><asp:TextBox ID="txtName" runat="server"></asp:TextBox></p>
    <p>
        <label>
            <b>*</b>手机:</label><asp:TextBox ID="txtCellPhone" runat="server"></asp:TextBox></p>
    <p>
        <label>
            <b>*</b>电话:</label><asp:TextBox ID="txtPhone" runat="server"></asp:TextBox></p>
    <p>
        <label>
            <b>*</b>性别:</label><asp:RadioButtonList ID="rblGeneder" runat="server">
                <asp:ListItem Text="男" Value="1"></asp:ListItem>
                <asp:ListItem Text="女" Value="0"></asp:ListItem>
            </asp:RadioButtonList>
    </p>
    <p>
        <label>
            <b>*</b>生日:</label>
        <asp:TextBox ID="txtBirthDay" runat="server"></asp:TextBox><strong>生日确定后，不能修改</strong>
    </p>
    <p>
        <label><b>*</b>
            居住地:</label>
            <sc1:Area ID="scArea" runat="server" AutoPostBack="false">
            </sc1:Area>
    </p>
    <p>
        <label>
            您的地址:</label><asp:TextBox ID="txtDwellAddress" runat="server" Width="430px"></asp:TextBox></p>
    <p>
        <label><b>*</b>
            您的邮编:</label><asp:TextBox ID="txtZip" runat="server"></asp:TextBox></p>
    <p>
        <label><b>*</b>
            您的传真:</label>
        <asp:TextBox ID="txtFax" runat="server"></asp:TextBox></p>
    <p>
        <asp:ImageButton ID="btnSave" ImageUrl="../static/images/save.jpg" runat="server"
            OnClick="btnSave_Click" AlternateText="确认修改" />
    </p>
    <p>
        <asp:Label ID="lblErrMsg" runat="server" ForeColor="Red" EnableViewState="False"></asp:Label>
    </p>
</div>
