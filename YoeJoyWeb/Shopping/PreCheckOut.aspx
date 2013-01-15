<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Payment.Master" AutoEventWireup="true"
    CodeBehind="PreCheckOut.aspx.cs" Inherits="YoeJoyWeb.Shopping.PreCheckOut" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link type="text/css" rel="Stylesheet" href="../static/css/base.css" />
    <link type="text/css" rel="Stylesheet" href="../static/css/process1.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
    <div id="content">
        <table id="tabInfo" runat="server" width="85%" align="center" border="0" cellpadding="3"
            cellspacing="1">
            <tr>
                <td align="center" height="500" valign="middle" style="font-size: 20px">
                    <b>抱歉，您的所在地信息尚未填写，请您先将信息补充完整<br>
                        <br>
                        <asp:HyperLink ID="lnkInfo" runat="server">
								            <font color="blue">填写信息请点击</font></asp:HyperLink></b>
                </td>
            </tr>
        </table>
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
