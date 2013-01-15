<%@ Page Title="" Language="C#" MasterPageFile="~/Master/User.Master" AutoEventWireup="true"
    CodeBehind="MyProfileDetail.aspx.cs" Inherits="YoeJoyWeb.User.MyProfileDetail" %>

<%@ Register Src="../Controls/CustomerDetailInfo.ascx" TagName="CustomerDetailInfo"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="NavContentPlaceHolder" runat="server">
    <div id="position">
        <span>
            <img src="../static/images/f4.jpg" />您在:</span> <b><a href="../Default.aspx">首页</a></b>
        <span>&gt;</span> <span><b><a href="MyProfile.aspx">用户中心</a></b></span><span>&gt;</span>
        <span>编辑个人资料</span>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="RightBigModule" runat="server">
    <uc1:CustomerDetailInfo ID="CustomerDetailInfo1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BackupContent1" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BackupContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
</asp:Content>
