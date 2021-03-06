﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Site.Master" AutoEventWireup="true"
    CodeBehind="SubProductList1.aspx.cs" Inherits="YoeJoyWeb.SubProductList1" %>

<%@ Register Src="../Controls/CategoryNavigation.ascx" TagName="CategoryNavigation"
    TagPrefix="uc1" %>
<%@ Register Src="../Controls/SubCategoryNavigation.ascx" TagName="SubCategoryNavigation"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link type="text/css" rel="Stylesheet" href="../static/css/productSort.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="LeftTopModule" runat="server">
    <uc1:CategoryNavigation ID="CategoryNavigation1" runat="server" IsHomePage="false" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="RightTopModule" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MiddleTopModule" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="SiteNavModule" runat="server">
    <div id="position">
        <span>您在 :</span> <b><a href="../Default.aspx">首页</a></b> <span>&gt;</span> <span
            id="c1"></span>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="LeftBigModule" runat="server">
    <div class="left">
        <uc2:SubCategoryNavigation ID="SubCategoryNavigation1" runat="server" />
        <dl class="ranking">
            <dt>
                <h3 class="title">
                    本周销量排行</h3>
            </dt>
            <dd>
                <%=C1WeeklyBestSaledProductsHTML%>
            </dd>
        </dl>
    </div>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="RightBigModule" runat="server">
    <div class="bigRight">
        <div class="clear">
            <a class="ad" href="http://www.ue96.com/Pages/SubProductList3.aspx?c1=232&amp;c2=539&amp;c3=540"
                target="_blank">
                <img src="http://www.ue96.com/up/products/ad09.jpg">
            </a>
            <%--<%=C1SlideAdHTML%>--%>
            <%--<dl id="Discount">
                <dt><i></i><b>清库产品</b><strong></strong></dt>
                <%=C1EmptyInventoryProductsHTML%>
            </dl>--%>
            <div id="limit">
                <h3>
                    限时抢购</h3>
                <h4>
                    <span>剩余</span>
                    <img alt="钟" src="../static/images/time.png" width="15" height="18">
                    <b>23</b> <em>小时</em> <b>55</b> <em>分</em> <b>33</b> <em>秒</em>
                </h4>
                <a class="photo" href="#">
                    <img alt="商品图片" src="../static/images/sp.jpg" width="80" height="80"></a>
                <p>
                    <a class="name" title="商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end" href="product.html">
                        商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end</a></p>
                <p>
                    <span class="adText">促销促销促销促销品名称商品品名称商品品名称商品销促销促销促销促销end</span></p>
                <h5>
                    <em class="price" align="right"><b>¥1500</b><span>¥2500</span></em> <a class="buying"
                        href="process1.html">立即抢购</a>
                </h5>
            </div>
        </div>
        <%=C1ProductsDisplayHTML%>
    </div>
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="BackupContent1" runat="server">
</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="HomeMiddleContent" runat="server">
</asp:Content>
<asp:Content ID="Content10" ContentPlaceHolderID="BackupContent2" runat="server">
</asp:Content>
<asp:Content ID="Content11" ContentPlaceHolderID="ScriptContent" runat="server">
    <script type="text/javascript">
        $(function () {

            $(".mainShow").each(function (index) {
                $(this).children("div").eq(0).css({ "width": "100%", "border": "none" });
                $(this).children("div").eq(1).css({ "display": "none" });
            });
            var $c1Name = $("#foodImport").children("h4").html();
            $("#position").children("span[id='c1']").html($c1Name);
        });
    </script>
</asp:Content>
