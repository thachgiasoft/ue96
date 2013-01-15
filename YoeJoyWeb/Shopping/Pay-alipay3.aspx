<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Payment.Master" AutoEventWireup="true"
    CodeBehind="Pay-alipay3.aspx.cs" Inherits="YoeJoyWeb.Shopping.Pay_alipay3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link type="text/css" rel="Stylesheet" href="../static/css/base.css" />
    <link type="text/css" rel="Stylesheet" href="../static/css/process1.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
    <div id="content">
        <div id="BookStatus">
            <h2>
                您的定订单已经生成，请尽快支付订单</h2>
            <p>
                <img src="../static/images/markes1.jpg">为了保证用时处理您的订单，请于下单<b><img src="../static/images/clock.jpg">23时23分24秒</b>内付，若逾期未付款订单将被取消，需重新下单</p>
            <table id="Table1" cellspacing="1" cellpadding="1" width="96%" align="center" border="0">
                <tr>
                    <td align="center" style="border-bottom: 1px solid #CB1732; font-family: 宋体">
                        <p>
                            <span style="font-size: 25PX; color: #cb1732">感谢您在本店购物！您的订单已提交成功，请记住您的订单号</span></p>
                        <p style="font-size: 18px;">
                            您的订单编号:<strong><asp:Label ID="lblSOID" runat="server" ForeColor="#cb1732"></asp:Label></strong></p>
                        <p style="font-size: 14px">
                            您选择的配送方式是:<span style="color: #cb1732; font-weight: bold;"><asp:Label ID="lblShipType"
                                runat="server" Font-Italic="True" Font-Bold="True"></asp:Label></span>, 您选择的支付方式是:<span
                                    style="color: #cb1732; font-weight: bold;"><asp:Label ID="lblPayType" runat="server"
                                        Font-Italic="True" Font-Bold="True"></asp:Label></span>, 订单金额是:<span style="color: #cb1732;
                                            font-weight: bold;"><asp:Label ID="lblSOAmt" runat="server"></asp:Label></span>。
                            <asp:Label ID="lblView" runat="server" Font-Italic="True" Font-Bold="True"></asp:Label>
                        </p>
                        <p style="font-size: 14px">
                            我们将为您保留订单1个工作日，如1日后攸怡仍然没有收到您的货款，系统将自动取消此订单。
                            <asp:Label ID="lblSOSysNo" runat="server" Visible="False"></asp:Label>
                        </p>
                        <p>
                            <font face="宋体">
                                <asp:Button ID="btnPayNow" Text="现在付款" CssClass="ButtonAccount5" runat="server" OnClick="btnPayNow_Click">
                                </asp:Button>&nbsp;
                                <input type="button" value="稍后付款" name="button2" onclick="go('../default.aspx')"
                                    class="ButtonAccount5" />
                            </font>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table id="Table2" cellspacing="0" cellpadding="0" width="100%" border="1" style="border-collapse: collapse;
                            text-align: center; border-color: #c9e0ee; visibility: hidden;">
                            <tbody>
                                <tr>
                                    <td bgcolor="#91c0dd">
                                        <font face="宋体">订单编号</font>
                                    </td>
                                    <td bgcolor="#91c0dd">
                                        <font face="宋体">订单总金额</font>
                                    </td>
                                    <td bgcolor="#91c0dd">
                                        <font face="宋体">定购日期</font>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <font face="宋体">
                                            <asp:Label ID="lblSOID_1" runat="server"></asp:Label></font>
                                    </td>
                                    <td>
                                        <font face="宋体"></font>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblSODate" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3">
                                        <input type="hidden" value="005626" name="mer_code" />
                                        <input type="hidden" value='005626' name="billno" />
                                        <input type="hidden" name="amount" />
                                        <input type="hidden" name="date" />
                                        <input type="hidden" name="OrderEncodeType" value="1" />
                                        <input type="hidden" name="RetEncodeType" value="2" />
                                        <input type="hidden" name="RetType" value="0" />
                                        <input type="hidden" name="SignMD5" />
                                        <input type="hidden" value="01" name="currency" />
                                        <input type="hidden" value="http://www.icson.com/Shopping/payresultfromAlipay3.aspx"
                                            name="merchanturl" />
                                        <input type="hidden" value="1" name="lang" />
                                        <input type="hidden" value=" " name="attach" />
                                    </td>
                                </tr>
                                <tr>
                                    <td height="31" colspan="3">
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </table>
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
