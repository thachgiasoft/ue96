<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Payment.Master" AutoEventWireup="true"
    CodeBehind="ShoppingCart.aspx.cs" Inherits="YoeJoyWeb.Shopping.ShoppingCart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link type="text/css" rel="Stylesheet" href="../static/css/base.css" />
    <link type="text/css" rel="Stylesheet" href="../static/css/process1.css" />
    <meta http-equiv="Expires " content="0 " />
    <meta http-equiv="Cache-Control " content="no-cache " />
    <meta http-equiv="Pragma " content="no-cache " />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
    <div id="content">
        <div id="process">
            <img src="../static/images/step1.png" />
            <ul>
                <li>1.查看购物车</li>
                <li>2.核对订单信息</li>
                <li>3.成功提交订单</li>
                <li>4.评论商品</li>
            </ul>
        </div>
        <div id="FreeDesc">
            <h2>
                <input checked="checked" type="checkbox" />
                <span>商品每满<em>100元</em>免<em>10kg</em></span> <span>商品每满<em>100元</em>免<em>10kg</em></span>
                <a href="#">运费说明</a>
            </h2>
            <%=ShoppingCartHTML %>
        </div>
        <div id="comfirm">
            <div class="l">
                <input type="checkbox" checked="checked" />
                全选
                <a href="#">批量删除</a>
                <a href="javascript:void(0);" id="btnClear">清空购物车</a>
            </div>
            <div class="r">
                <b>商品总价:</b>
                <strong id="bottomPrice"></strong>
                <b>元</b>
                <a href="javascript:void(0);" id="btnConfirm">
                    <img src="../static/images/comfirm.jpg" /></a>
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
    <script type="text/javascript">
        $(function () {

            var totalPrice = $("#totalPrice").val();
            $("#bottomPrice").html(totalPrice);

            $("#btnClear").click(function () {
                YoeJoy.Site.ShoppingCart.MainCart.ClearCartItems();
            });

            $("#btnConfirm").click(function () {
                YoeJoy.Site.ShoppingCart.MainCart.GoShopping();
            });

        });
    </script>
</asp:Content>
