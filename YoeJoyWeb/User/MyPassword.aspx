<%@ Page Title="" Language="C#" MasterPageFile="~/Master/User.Master" AutoEventWireup="true"
    CodeBehind="MyPassword.aspx.cs" Inherits="YoeJoyWeb.User.MyPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavContentPlaceHolder" runat="server">
    <div id="position">
        <span>
            <img src="../static/images/f4.jpg" />您在:</span> <b><a href="../Default.aspx">首页</a></b>
        <span>&gt;</span> <span><b><a href="MyProfile.aspx">用户中心</a></b></span><span>&gt;</span>
        <span>修改密码</span>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="RightBigModule" runat="server">
    <div class="changePassword">
        <h2 class="titles">
            <b>修改密码</b></h2>
        <p>
            <label>
                原密码：</label>
            <asp:TextBox ID="txtOld" runat="server" TextMode="Password"></asp:TextBox>
        </p>
        <p>
            <label>
                新密码：</label>
            <asp:TextBox ID="txtNew0" runat="server" TextMode="Password"></asp:TextBox>
        </p>
        <p>
            <label>
                确认密码：</label>
            <asp:TextBox ID="txtNew1" runat="server" TextMode="Password"></asp:TextBox>
        </p>
        <p class="comf">
            <a href="javascript:void(0);">
                <asp:ImageButton ID="btnSubmit" ImageUrl="../static/images/comfirm1.jpg" runat="server"
                    AlternateText="确 认" OnClick="btnSubmit_Click" /></a> <a href="#">
                        <img src="../static/images/cancel.jpg"></a>
        </p>
        <p>
            <asp:Label ID="lblErrMsg" runat="server" ForeColor="Red"></asp:Label>
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BackupContent1" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BackupContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
</asp:Content>
