<%@ Page Title="" Language="C#" MasterPageFile="~/Master/User.Master" AutoEventWireup="true"
    CodeBehind="MyHistory.aspx.cs" Inherits="YoeJoyWeb.User.MyHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavContentPlaceHolder" runat="server">
<div id="position">
        <span>
            <img src="../static/images/f4.jpg" />您在:</span> <b><a href="../Default.aspx">首页</a></b>
        <span>&gt;</span> <span><b><a href="MyProfile.aspx">用户中心</a></b></span><span>&gt;</span>
        <span>浏览历史</span>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="RightBigModule" runat="server">
    <div class="history2">
        <h2 class="titles">
            <b>浏览历史</b><asp:Button ID="btnClearAll" runat="server" Text="清空所有浏览历史" 
                onclick="btnClearAll_Click" /></h2>
        <table class="historyDetail" cellspacing="0" cellpadding="0">
            <tbody>
            <%=UserBrowserHistoryProductsAllHTML%>
            </tbody>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BackupContent1" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BackupContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
</asp:Content>
