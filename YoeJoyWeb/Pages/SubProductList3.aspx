﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Site.Master" AutoEventWireup="true"
    CodeBehind="SubProductList3.aspx.cs" Inherits="YoeJoyWeb.SubProductList3" %>

<%@ Register Src="../Controls/CategoryNavigation.ascx" TagName="CategoryNavigation"
    TagPrefix="uc1" %>
<%@ Register Src="../Controls/SubCategoryNavigation.ascx" TagName="SubCategoryNavigation"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link type="text/css" rel="Stylesheet" href="../static/css/layout.css" />
    <link type="text/css" rel="Stylesheet" href="../static/css/class.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="LeftTopModule" runat="server">
    <uc1:CategoryNavigation ID="CategoryNavigation1" runat="server" IsHomePage="false" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="RightTopModule" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MiddleTopModule" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="SiteNavModule" runat="server">
    <div id="breadNav">
        <p>
            您在：<a href="../Default.aspx">首页</a>〉<a id='c1NavLink' href="javascript:void(0);"></a>〉<a
                id="c2NavLink" href="javascript:void(0);"></a>〉 <span id="c2NavLink"></span>
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="LeftBigModule" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="RightBigModule" runat="server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="BackupContent1" runat="server">
    <div class="mix">
        <div class="l_module">
            <uc2:SubCategoryNavigation ID="SubCategoryNavigation1" runat="server" />
            <dl id="Discount">
                <dt>
                    <h3 class="title">
                        清库产品</h3>
                </dt>
                <dd>
                    <h2>
                        <a href="product.html">
                            <img src="../static/images/sps1.jpg"></a></h2>
                    <p>
                        <a class="name" title="宏碁i5 独显 4G内存 直降400低价疯抢中！" href="product.html">宏碁i5 独显 4G内存 直降400低价疯抢中！</a>
                        <em class="price"><b>¥1500</b><span>¥500</span></em>
                    </p>
                </dd>
                <dd>
                    <h2>
                        <a href="product.html">
                            <img src="../static/images/sps1.jpg"></a></h2>
                    <p>
                        <a class="name" title="宏碁i5 独显 4G内存 直降400低价疯抢中！" href="product.html">宏碁i5 独显 4G内存 直降400低价疯抢中！</a>
                        <em class="price"><b>¥1500</b><span>¥500</span></em>
                    </p>
                </dd>
                <dd>
                    <h2>
                        <a href="product.html">
                            <img src="../static/images/sps1.jpg"></a></h2>
                    <p>
                        <a class="name" title="宏碁i5 独显 4G内存 直降400低价疯抢中！" href="product.html">宏碁i5 独显 4G内存 直降400低价疯抢中！</a>
                        <em class="price"><b>¥1500</b><span>¥500</span></em>
                    </p>
                </dd>
                <dd>
                    <h2>
                        <a href="product.html">
                            <img src="../static/images/sps1.jpg"></a></h2>
                    <p>
                        <a class="name" title="宏碁i5 独显 4G内存 直降400低价疯抢中！" href="product.html">宏碁i5 独显 4G内存 直降400低价疯抢中！</a>
                        <em class="price"><b>¥1500</b><span>¥500</span></em>
                    </p>
                </dd>
            </dl>
            <div id="hotComments" class="l_class area">
                    <h3 class="title">
                        产品热评</h3>
                <%=C3HotCommentProductHTML%>
            </div>
        </div>
        <div class="r_mix">
            <!--热卖推荐Begin-->
            <div id="recommend">
                <div class="group">
                    <img class="pTitle" src="../static/images/recommend.png">
                    <%=C3BestSaledProductHTML%>
                </div>
            </div>
            <!--热卖推荐End-->
            <div id="groupBuy">
                <h3 class="title">
                    团购</h3>
                <div class="group">
                    <h5 align="center">
                        <a href="#">
                            <img src="../static/images/dishes.jpg" width="100"></a></h5>
                    <p>
                        <a class="name" href="#">商品名称商品名称商品名称商品名称商品名称商品名称 </a>
                    </p>
                    <p class="join">
                        <a href="Grounp2.html">参团</a><em>¥1000</em><span>市场价:2000</span></p>
                    <p class="item">
                        <em class="time"><span>还剩</span><strong>10</strong><span>天</span><strong>10</strong><span>小时</span></em>
                        <b class="number"><span>余</span><strong>125</strong><span>份</span></b>
                    </p>
                </div>
            </div>
            <!--筛选条件Begin-->
            <div id="screening">
                <div class="title">
                </div>
                <%=C3ProductFilterHTML%>
                <div class="more">
                    <div class="item0">
                    </div>
                    <div class="item1">
                        <a href="javascript:void(0)">更多筛选</a></div>
                </div>
            </div>
            <iframe id="ProductIframe" frameborder="0" scrolling="no" width="1001px" height="1150px">
            </iframe>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="HomeMiddleContent" runat="server">
</asp:Content>
<asp:Content ID="Content10" ContentPlaceHolderID="BackupContent2" runat="server">
</asp:Content>
<asp:Content ID="Content11" ContentPlaceHolderID="ScriptContent" runat="server">
    <script type="text/javascript">

        function getAttributionIds() {
            var productFilterCount = $("#screening").children("div[class='attr']").length;
            var selectedAIds = new Array();
            for (var i = 0; i < productFilterCount; i++) {
                var attrId = $("#screening").children("div[class='attr']").eq(i).children("input[class='selectedValue']").eq(0).val();
                if (attrId != "0") {
                    selectedAIds[i] = attrId;
                }
            }
            return YoeJoy.Site.Utility.ReplaceEmptyItem(selectedAIds);
        }

        $(function () {

            var c1 = YoeJoy.Site.Utility.GetQueryString("c1");
            var c2 = YoeJoy.Site.Utility.GetQueryString("c2");
            var c3 = YoeJoy.Site.Utility.GetQueryString("c3");

            var $c1Name = $("#foodImport").children("h4").html();
            $("#breadNav").children("p").children("a").eq(1).html($c1Name);
            $("#breadNav").children("p").children("a").eq(1).click(function (event) {
                window.location.href = $("#siteBaseURL").val() + "Pages/SubProductList1.aspx?c1=" + c1;
                window.location.target = "_parent";
            });
            var $c2Name = $(".listOut li input[value=" + c2 + "]").siblings("h3").text();
            $("#breadNav").children("p").children("a").eq(2).html($c2Name);
            $("#breadNav").children("p").children("a").eq(2).click(function (event) {
                window.location.href = $("#siteBaseURL").val() + "Pages/SubProductList2.aspx?c1=" + c1 + "&c2=" + c2;
                window.location.target = "_parent";
            });
            var $c3Name = $(".listOut li p a input[value=" + c3 + "]").siblings("input").val();
            $("#breadNav").children("p").children("span").eq(0).html($c3Name);
            $("#screening").children(".title").eq(0).html($c3Name + "－商品筛选");

            var productListBaseURL = "ProductList.aspx?c1=" + c1 + "&c2=" + c2 + "&c3=" + c3 + "&attrIds=";
            $("#ProductIframe").attr({ "src": productListBaseURL });

            $("#screening").children("div[class='attr']").each(function (index) {

                var $attrItem = $(this);
                $attrItem.children(".all").click(function (event) {
                    $attrItem.children("input[class='selectedValue']").val("0");
                    var aIds = getAttributionIds();
                    var url = productListBaseURL + aIds;
                    $("#ProductIframe").attr({ "src": url });
                });

                $attrItem.children("strong").eq(0).children("span").each(function (index1) {
                    var $attrItemSpan = $(this);
                    $attrItemSpan.click(function (event) {
                        var selectedAId = $attrItemSpan.children("input").val();
                        $attrItem.children("input[class='selectedValue']").val(selectedAId);
                        var aIds = getAttributionIds();
                        var url = productListBaseURL + aIds;
                        $("#ProductIframe").attr({ "src": url });
                    });
                });

            });

        });
    </script>
</asp:Content>
