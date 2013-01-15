<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Site.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="YoeJoyWeb.Default" %>

<%@ Register Src="Controls/CategoryNavigation.ascx" TagName="CategoryNavigation"
    TagPrefix="uc1" %>
<%@ Register Src="Controls/OnlineStaticAD.ascx" TagName="OnlineStaticAD" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link type="text/css" rel="Stylesheet" href="../static/css/head.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="LeftTopModule" runat="server">
    <uc1:CategoryNavigation ID="CategoryNavigation1" runat="server" />
    <div style="margin-top: 360px;">
        <uc2:OnlineStaticAD ID="ADBelowNavigation" ADPositionID="2" ADCSSClass="ad" Width="208"
            Height="80" runat="server" />
    </div>
    <div class="panic">
        <h3>
            <em></em><strong><b>限时抢购</b><span>Panic-Bu ying</span></strong> <i></i>
        </h3>
        <%=PanicBuyingHTML%>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="RightTopModule" runat="server">
    <a href="javascript:void(0);">
        <img src="static/images/help2.png" width="192" height="74" /></a>
    <dl id="notes">
        <dt><a class="adone sel" href="#"><span>公告</span> </a><a class="adtwo" href="#"><span>
            动态</span> </a></dt>
        <dd>
            <p style="display: block;">
                <%=HomeWebBulletinListHTML %>
            </p>
            <p style="display: none;">
                <%=HomeWebBulletinListHTML %>
            </p>
        </dd>
    </dl>
    <uc2:OnlineStaticAD ID="ADBelowNews" ADPositionID="3" ADCSSClass="ad" Width="192"
        Height="106" runat="server" />
    <a class="ad" href="http://www.ue96.com/pages/product.aspx?c1=2&amp;c2=611&amp;c3=612&amp;pid=29554"
        target="_blank">
        <img src="http://www.ue96.com/up/products/ad11.jpg">
    </a><a class="ad" href="http://www.ue96.com/Pages/SubProductList2.aspx?c1=633&amp;c2=634"
        target="_blank">
        <img src="http://www.ue96.com/up/products/ad12.jpg" height="262">
    </a>
    <%--<div class="GroupPurchase">
        <h5 class="mytitles">
            <em></em><strong><b>团购</b><span>Group-Buying</span></strong> <i></i>
        </h5>
        <div class="dd">
            <p class="day">
                <span>6</span> <span>6</span> <b>天</b> <span>6</span> <span>6</span> <b>时</b> <span>
                    6</span> <span>6</span> <b>分</b>
            </p>
            <p class="ad2">
                <a href="#">
                    <img src="static/images/dishes.jpg">
                </a>
            </p>
            <a class="word" href="#">商品名称商品名称商品名称商品名称商品名称商品名称 </a><a class="join" href="#">¥1000.0
            </a>
            <p class="hr">
            </p>
            <p class="day">
                <span>6</span> <span>6</span> <b>天</b> <span>6</span> <span>6</span> <b>时</b> <span>
                    6</span> <span>6</span> <b>分</b>
            </p>
            <p class="ad2">
                <a href="#">
                    <img src="static/images/dishes.jpg">
                </a>
            </p>
            <a class="word" href="#">商品名称商品名称商品名称商品名称商品名称商品名称 </a><a class="join" href="#">¥1000.0
            </a>
        </div>
    </div>--%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MiddleTopModule" runat="server">
    <dl id="focus">
        <dt><a style="display: none;" href="http://www.ue96.com/Pages/SubProductList3.aspx?c1=232&amp;c2=233&amp;c3=352">
            <img src="http://www.ue96.com/up/products/ad07.jpg" /></a> <a style="display: none;"
                href="http://www.ue96.com/Pages/SubProductList1.aspx?c1=8">
                <img src="http://www.ue96.com/up/products/ad04.jpg" /></a> <a style="display: inline-block;"
                    href="http://www.ue96.com/Pages/SubProductList1.aspx?c1=633">
                    <img src="http://www.ue96.com/up/products/ad03.jpg" /></a> <a style="display: none;"
                        href="http://www.ue96.com/Pages/SubProductList2.aspx?c1=2&amp;c2=620">
                        <img src="http://www.ue96.com/up/products/ad05.jpg" /></a> <a style="display: none;"
                            href="http://www.ue96.com/Pages/SubProductList1.aspx?c1=4">
                            <img src="http://www.ue96.com/up/products/ad06.jpg" /></a> </dt>
        <dd>
            <a style="opacity: 0.5;" href="http://www.ue96.com/Pages/SubProductList3.aspx?c1=232&amp;c2=233&amp;c3=352">
                趣味办公享生活</a> <a style="opacity: 0.5;" href="http://www.ue96.com/Pages/SubProductList1.aspx?c1=8">
                    LaFong Style</a> <a style="opacity: 1;" href="http://www.ue96.com/Pages/SubProductList1.aspx?c1=633">
                        爱美女人的秘密</a> <a style="opacity: 0.5;" href="http://www.ue96.com/Pages/SubProductList2.aspx?c1=2&amp;c2=620">
                            Apple精品周边</a> <a style="opacity: 0.5;" href="http://www.ue96.com/Pages/SubProductList1.aspx?c1=4">
                                礼品卡惠集中营</a>
        </dd>
    </dl>
    <dl class="Promotions">
        <dt><a class="prom" href="#">
            <img alt="图标" src="static/images/szj.png" width="8" height="8" />
            <b>商品促销</b> <span>promotions</span> </a><a class="more" href="#">
                <%--更多&gt;&gt; --%></a> </dt>
        <dd>
            <%=PromoHTML %>
        </dd>
    </dl>
    <dl class="Promotions">
        <!--新品上市-->
        <dt><a class="prom" href="#">
            <img alt="图标" src="static/images/szj.png" width="8" height="8" />
            <b>新品上市</b> <span>promotions</span> </a><a class="more" href="#">
                <%--更多&gt;&gt; --%></a> </dt>
        <dd>
            <%=InComingProducts%>
        </dd>
    </dl>
    <div style="margin-top: 7px;" class="band">
        <span>品牌旗舰店</span>
        <%=HomeBrandsHTML %>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="SiteNavModule" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="LeftBigModule" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="RightBigModule" runat="server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="BackupContent1" runat="server">
</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="HomeMiddleContent" runat="server">
    <div id="content">
        <div id="highgoods1" class="area">
            <a class="prev" href="javascript:void(0)"></a>
            <div style="left: 573px;" id="options" class="selectbj">
            </div>
            <a class="next" href="javascript:void(0)"></a>
            <div style="width: 3600px;" class="group">
                <div class="item">
                    <img src="static/images/spb3.jpg" width="100">
                    <ul>
                        <li><a href="http://www.ue96.com/pages/product.aspx?c1=2&amp;c2=620&amp;c3=625&amp;pid=29498">
                            <img style="margin-top: 60px;" src="static/images/1a.png" width="253" height="265"></a></li>
                        <li><a href="http://www.ue96.com/pages/product.aspx?c1=2&amp;c2=620&amp;c3=625&amp;pid=29477">
                            <img style="margin-top: 60px;" src="static/images/2a.png" width="278" height="265"></a></li>
                        <li><a href="http://www.ue96.com/pages/product.aspx?c1=2&amp;c2=620&amp;c3=625&amp;pid=29622">
                            <img style="margin-top: 60px;" src="static/images/3a.png" width="278" height="265"></a></li>
                    </ul>
                </div>
                <div class="item">
                    <img src="static/images/spb3.jpg" width="100">
                    <ul>
                        <li><a href="http://www.ue96.com/pages/product.aspx?c1=2&amp;c2=620&amp;c3=625&amp;pid=29618">
                            <img style="margin-top: 60px;" src="static/images/4a.png" width="278" height="265"></a></li>
                        <li><a href="http://www.ue96.com/pages/product.aspx?c1=2&amp;c2=620&amp;c3=625&amp;pid=29619">
                            <img style="margin-top: 60px;" src="static/images/5a.png" width="278" height="265"></a></li>
                        <li><a href="products/http://www.ue96.com/pages/product.aspx?c1=2&amp;c2=620&amp;c3=625&amp;pid=29620">
                            <img style="margin-top: 60px;" src="static/images/6a.png" width="278" height="265"></a></li>
                    </ul>
                </div>
                <div class="item">
                    <img src="static/images/spb3.jpg" width="100">
                    <ul>
                        <li><a href="http://www.ue96.com/pages/product.aspx?c1=2&amp;c2=620&amp;c3=625&amp;pid=29621">
                            <img style="margin-top: 60px;" src="static/images/7a.png" width="278" height="265"></a></li>
                        <li><a href="http://www.ue96.com/pages/product.aspx?c1=2&amp;c2=620&amp;c3=625&amp;pid=29627">
                            <img style="margin-top: 60px;" src="static/images/8a.png" width="278" height="265"></a></li>
                        <li><a href="http://www.ue96.com/pages/product.aspx?c1=2&amp;c2=620&amp;c3=622&amp;pid=29623">
                            <img style="margin-top: 60px;" src="static/images/9a.png" width="278" height="265"></a></li>
                    </ul>
                </div>
            </div>
        </div>
        <div id="highgoods2" class="area imgshow">
            <%=HomePromotionBrandsProductHTML %>
        </div>
        <div id="centerAdWrapper">
            <uc2:OnlineStaticAD ID="OnlineStaticAD1" ADPositionID="4" ADCSSClass="ad1" runat="server" />
            <uc2:OnlineStaticAD ID="OnlineStaticAD2" ADPositionID="5" ADCSSClass="ad1" runat="server" />
            <div style="clear: left;">
            </div>
        </div>
        <div class="ThreeRow">
            <div class="bigLeft">
                <div class="It">
                    <a class="Header" href="#">
                        <img src="static/images/it.png" /></a>
                    <uc2:OnlineStaticAD ID="C1LeftAD1" ADPositionID="6" Width="208" Height="278" ADCSSClass="Header"
                        runat="server" />
                    <p style="height: 66px; background-color: rgb(51, 0, 204);">
                    </p>
                    <p>
                        <a href="#">IT相关导购文</a></p>
                    <p>
                        <a href="#">IT相关导购文</a></p>
                    <p>
                        <a href="#">IT相关导购文</a></p>
                    <p>
                        <a href="#">IT相关导购文</a></p>
                    <p>
                        <a href="#">IT相关导购文</a></p>
                </div>
                <%=CategoryProductsOneHTML%>
            </div>
            <div class="right">
                <div class="brand">
                    <a class="h" href="#">
                        <img src="static/images/brand.gif" width="192" height="40"></a>
                    <%=CategoryProductsBrandsOneHTML%>
                </div>
                <div class="discus">
                    <h2 id="phone1">
                        <span><a class="sel" href="#">用户评论</a></span> <span><a href="#">销量排行</a></span>
                    </h2>
                    <div id="phoneCon1">
                        <%=CategoryProductsHotCommentedOneHTML%>
                        <%=CategoryProductBestSaledOneHTML%>
                    </div>
                </div>
            </div>
        </div>
        <div style="margin-top: 10px;" class="ThreeRow">
            <div class="bigLeft">
                <div class="It">
                    <a class="Header" href="#">
                        <img src="static/images/home.png"></a>
                    <uc2:OnlineStaticAD ID="C1LeftAD2" ADPositionID="7" ADCSSClass="Header" Width="208"
                        Height="278" runat="server" />
                    <p style="height: 66px; background-color: rgb(51, 0, 204);">
                    </p>
                    <p>
                        <a href="#">IT相关导购文</a></p>
                    <p>
                        <a href="#">IT相关导购文</a></p>
                    <p>
                        <a href="#">IT相关导购文</a></p>
                    <p>
                        <a href="#">IT相关导购文</a></p>
                    <p>
                        <a href="#">IT相关导购文</a></p>
                </div>
                <%=CategoryProductsTwoHTML%>
            </div>
            <div class="right">
                <div class="brand">
                    <a class="h" href="#">
                        <img src="static/images/brand.gif" /></a>
                    <%=CategoryProductsBrandsTwoHTML%>
                </div>
                <div class="discus">
                    <h2 id="phone2">
                        <span><a class="sel" href="#">用户评论</a></span> <span><a href="#">销量排行</a></span>
                    </h2>
                    <div id="phoneCon2">
                        <%=CategoryProductsHotCommentedTwoHTML%>
                        <%=CategoryProductBestSaledTwoHTML%>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content10" ContentPlaceHolderID="BackupContent2" runat="server">
</asp:Content>
<asp:Content ID="Content11" ContentPlaceHolderID="ScriptContent" runat="server">
    <script type="text/javascript">
        $(function () {

            $("#panicContentt").children("div[class='panicContent']").each(function (index) {
                var $this = $(this);
                var startTime = new Date();
                var $endTime = $this.children(".time").children("input").val();
                $endTime = $endTime.replace(/\-/g, '/');
                $endTime = Date.parse($endTime);
                var remainSecond = (($endTime - startTime.getTime()) / 1000);
                var $timeDiv = $this.children("h2[class='time']");
                var InterValObj = window.setInterval(function () {
                    if (remainSecond > 0) {
                        remainSecond = remainSecond - 1;
                        var second = Math.floor(remainSecond % 60);
                        var minite = Math.floor((remainSecond / 60) % 60);
                        var hour = Math.floor((remainSecond / 3600) % 24);
                        var day = Math.floor((remainSecond / 3600) / 24);
                        $timeDiv.html("<span>剩余 </span>&nbsp;<img alt='钟' src='static/images/time.png' width='15' height='18'/><b>"
                         + day + "</b><em>天</em><b>" + hour + "</b><em>小时</em><b>" + minite + "</b><em>分</em>" +
                                        "<b>" + second + "</b><em>秒</em>");
                    }
                    else {
                        window.clearInterval(InterValObj);
                        $timeDiv.html("<b>抢购结束</b>")
                    }
                }, 1000);
            });

        });
    </script>
</asp:Content>
