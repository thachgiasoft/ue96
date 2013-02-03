<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Site.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="YoeJoyWeb.Default" %>

<%@ Register Src="Controls/CategoryNavigation.ascx" TagName="CategoryNavigation"
    TagPrefix="uc1" %>
<%@ Register Src="Controls/OnlineStaticAD.ascx" TagName="OnlineStaticAD" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link type="text/css" rel="Stylesheet" href="../static/css/head.css" />
    <link href="static/css/ui-lightness/jquery-ui-1.10.0.custom.min.css" rel="stylesheet"/>
	<script src="static/js/prod/jquery-ui-1.10.0.custom.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="LeftTopModule" runat="server">
    <uc1:CategoryNavigation ID="CategoryNavigation1" runat="server" />
    <div style="margin-top: 422px;">
        <uc2:OnlineStaticAD ID="ADBelowNavigation" ADPositionID="2" ADCSSClass="ad" Width="208"
            Height="80" runat="server" />
    </div>
    <div id="weibo">
        <h3 class="title">
            会员互动</h3>
        <div class="group">
            <h4>
                新浪微博</h4>
            <dl>
                <dt>
                    <img src="../static/images/logotemp.png" width="67" /></dt>
                <dd>
                    <p>
                        <em>Ue96攸怡商城</em><span>V</span></p>
                    <p>
                        <a href="javascript:void(0);">+加关注</a></p>
                </dd>
            </dl>
            <div class="external">
                <p>
                    微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容微博内容</p>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="RightTopModule" runat="server">
    <div id="security">
        <h3 class="title">
            服务与保障</h3>
        <ul>
            <li class="col1">
                <p>
                    <strong>正品行货 满299免运费</strong></p>
                <p>
                    <span>免运费限江浙沪</span></p>
            </li>
            <li class="col2">
                <p>
                    <strong>48小时价格保护</strong></p>
                <p>
                    <span>降价可退差额</span></p>
            </li>
            <li class="col3">
                <p>
                    <strong>24小时7天 随时接听</strong></p>
                <p>
                    <span>贴心服务</span></p>
            </li>
            <li class="col4">
                <p>
                    <strong>7天无理由退货</strong></p>
                <p>
                    <span>15天换货</span></p>
            </li>
        </ul>
    </div>
    <dl id="notes">
        <dt><a class="adone sel" href="#"><span>公告<i></i></span> </a><a class="adtwo" href="#">
            <span>动态<i></i></span> </a></dt>
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
    <div id="panic">
        <h3>
            限时抢购</h3>
        <div class="scrollItem">
            <%=PanicBuyingHTML%>
            <span class="prev scrollbt"><a href="javascript:void(0);"></a></span><span class="next scrollbt">
                <a href="javascript:void(0);"></a></span>
        </div>
    </div>
    <dl class="Promotions">
        <dt><a href="#"><b>新品上市</b></a><a href="#"><b>促销商品</b></a><a href="#"><b>会员特享</b></a><a
            href="#"><b>猜你喜欢</b></a> </dt>
        <dd>
            <%=PromoHTML %>
            <%=InComingProducts %>
        </dd>
    </dl>
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
                    <img src="static/images/ipevo_logo.png" width="100">
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
                    <h3>
                        <a class="Header" href="#">
                            <img src="static/images/it.png" /></a>
                    </h3>
                    <div class="group">
                        <uc2:OnlineStaticAD ID="C1LeftAD1" ADPositionID="6" Width="208" Height="278" ADCSSClass="Header"
                            runat="server" />
                        <p>
                            <a href="http://www.ue96.com/Pages/SubProductList2.aspx?c1=232&amp;c2=271">酷炫才是潮人必备大杀器</a></p>
                        <p>
                            <a href="http://www.ue96.com/Pages/SubProductList2.aspx?c1=232&amp;c2=397">那些匪夷所思的好玩意儿</a></p>
                        <p>
                            <a href="http://www.ue96.com/Pages/SubProductList2.aspx?c1=232&amp;c2=376">生活就是要讲究品质才行</a></p>
                        <p>
                            <a href="http://www.ue96.com/Pages/SubProductList2.aspx?c1=232&amp;c2=539">和“烦躁”“压力”“忧郁”说北北</a></p>
                        <p>
                            <a href="http://www.ue96.com/Pages/SubProductList2.aspx?c1=232&amp;c2=233">快把枯燥的上班变成享受</a></p>
                    </div>
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
                        <%--<span><a class="sel" href="#">用户评论</a></span>--%>
                        <span><a href="#" class="sel">销量排行</a></span>
                    </h2>
                    <div id="phoneCon1">
                        <%--<%=CategoryProductsHotCommentedOneHTML%>--%>
                        <%=CategoryProductBestSaledOneHTML%>
                    </div>
                </div>
            </div>
        </div>
        <div style="margin-top: 10px;" class="ThreeRow">
            <div class="bigLeft">
                <div class="It">
                    <h3>
                        <a class="Header" href="#">
                            <img src="static/images/home.png"></a>
                    </h3>
                    <div class="group">
                        <uc2:OnlineStaticAD ID="C1LeftAD2" ADPositionID="7" ADCSSClass="Header" Width="208"
                            Height="278" runat="server" />
                        <p>
                            <a href="Pages/BrandProductList1.aspx?bid=24">澳洲顶级抗衰老护肤品</a></p>
                        <p>
                            <a href="pages/dg01.aspx">绵羊油并不是羊的脂肪</a></p>
                        <p>
                            <a href="pages/dg02.aspx">美容达人必备护肤品</a></p>
                        <p>
                            <a href="pages/dg03.aspx">把脸洗干净了再谈护肤</a></p>
                        <p>
                            <a href="pages/dg04.aspx">重回固体时代</a></p>
                    </div>
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
                        <%--<span><a class="sel" href="#">用户评论</a></span>--%>
                        <span><a href="#">销量排行</a></span>
                    </h2>
                    <div id="phoneCon2">
                        <%--<%=CategoryProductsHotCommentedTwoHTML%>--%>
                        <%=CategoryProductBestSaledTwoHTML%>
                    </div>
                </div>
            </div>
        </div>
        <div class="centerAdWrapper">
            <a class="ad1" href="#">
                <img src="../static/images/ad3.jpg"></a><a class="ad1" href="#"><img src="../static/images/ad02.jpg"></a>
        </div>
        <div id="scheme" class="ThreeRow">
            <div class="bigLeft">
                <div class="It">
                    <h3>
                        出行解决方案</h3>
                    <div class="group">
                        <a class="ad" href="product.html">
                            <img src="../static/images/tsj.jpg" width="206" height="290"></a>
                    </div>
                </div>
                <div class="sort">
                    <ul id="sort2" class="sortHeader">
                        <li><a href="#">分类</a></li>
                        <li><a href="#">分类</a></li>
                        <li><a href="#">分类</a></li>
                        <li><a href="#">分类</a></li>
                    </ul>
                    <div class="main">
                        <div class="sort2Con">
                            <ul class="product sortContent">
                                <li>
                                    <div>
                                        <h3>
                                            <a href="#">
                                                <img alt="产品图片" src="../static/images/product.jpg" width="100" height="100"></a></h3>
                                        <p>
                                            <a class="name" title="商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end" href="product.html">
                                                商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end</a></p>
                                        <p>
                                            <span class="adText">促销促销促销促销品名称商品品名称商品品名称商品销促销促销促销促销end</span>
                                        </p>
                                        <p class="price">
                                            <b>¥1500</b><span>¥500</span></p>
                                    </div>
                                </li>
                                <li>
                                    <div>
                                        <h3>
                                            <a href="#">
                                                <img alt="产品图片" src="../static/images/product.jpg" width="100" height="100"></a></h3>
                                        <p>
                                            <a class="name" title="商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end" href="product.html">
                                                商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end</a></p>
                                        <p>
                                            <span class="adText">促销促销促销促销品名称商品品名称商品品名称商品销促销促销促销促销end</span>
                                        </p>
                                        <p class="price">
                                            <b>¥1500</b><span>¥500</span></p>
                                    </div>
                                </li>
                                <li>
                                    <div>
                                        <h3>
                                            <a href="#">
                                                <img alt="产品图片" src="../static/images/product.jpg" width="100" height="100"></a></h3>
                                        <p>
                                            <a class="name" title="商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end" href="product.html">
                                                商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end</a></p>
                                        <p>
                                            <span class="adText">促销促销促销促销品名称商品品名称商品品名称商品销促销促销促销促销end</span>
                                        </p>
                                        <p class="price">
                                            <b>¥1500</b><span>¥500</span></p>
                                    </div>
                                </li>
                                <li>
                                    <div>
                                        <h3>
                                            <a href="#">
                                                <img alt="产品图片" src="../static/images/product.jpg" width="100" height="100"></a></h3>
                                        <p>
                                            <a class="name" title="商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end" href="product.html">
                                                商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end</a></p>
                                        <p>
                                            <span class="adText">促销促销促销促销品名称商品品名称商品品名称商品销促销促销促销促销end</span>
                                        </p>
                                        <p class="price">
                                            <b>¥1500</b><span>¥500</span></p>
                                    </div>
                                </li>
                                <li>
                                    <div>
                                        <h3>
                                            <a href="#">
                                                <img alt="产品图片" src="../static/images/product.jpg" width="100" height="100"></a></h3>
                                        <p>
                                            <a class="name" title="商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end" href="product.html">
                                                商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end</a></p>
                                        <p>
                                            <span class="adText">促销促销促销促销品名称商品品名称商品品名称商品销促销促销促销促销end</span>
                                        </p>
                                        <p class="price">
                                            <b>¥1500</b><span>¥500</span></p>
                                    </div>
                                </li>
                                <li>
                                    <div>
                                        <h3>
                                            <a href="#">
                                                <img alt="产品图片" src="../static/images/product.jpg" width="100" height="100"></a></h3>
                                        <p>
                                            <a class="name" title="商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end" href="product.html">
                                                商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end</a></p>
                                        <p>
                                            <span class="adText">促销促销促销促销品名称商品品名称商品品名称商品销促销促销促销促销end</span>
                                        </p>
                                        <p class="price">
                                            <b>¥1500</b><span>¥500</span></p>
                                    </div>
                                </li>
                            </ul>
                        </div>
                        <div class="sort2Con">
                            <ul class="product sortContent">
                                <li>
                                    <div>
                                        <h3>
                                            <a href="#">
                                                <img alt="产品图片" src="../static/images/product.jpg" width="100" height="100"></a></h3>
                                        <p>
                                            <a class="name" title="商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end" href="product.html">
                                                2end</a></p>
                                        <p>
                                            <span class="adText">促销促销促销促销品名称商品品名称商品品名称商品销促销促销促销促销end</span>
                                        </p>
                                        <p class="price">
                                            <b>¥1500</b><span>¥500</span></p>
                                    </div>
                                </li>
                                <li>
                                    <div>
                                        <h3>
                                            <a href="#">
                                                <img alt="产品图片" src="../static/images/product.jpg" width="100" height="100"></a></h3>
                                        <p>
                                            <a class="name" title="商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end" href="product.html">
                                                商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end</a></p>
                                        <p>
                                            <span class="adText">促销促销促销促销品名称商品品名称商品品名称商品销促销促销促销促销end</span>
                                        </p>
                                        <p class="price">
                                            <b>¥1500</b><span>¥500</span></p>
                                    </div>
                                </li>
                                <li>
                                    <div>
                                        <h3>
                                            <a href="#">
                                                <img alt="产品图片" src="../static/images/product.jpg" width="100" height="100"></a></h3>
                                        <p>
                                            <a class="name" title="商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end" href="product.html">
                                                商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end</a></p>
                                        <p>
                                            <span class="adText">促销促销促销促销品名称商品品名称商品品名称商品销促销促销促销促销end</span>
                                        </p>
                                        <p class="price">
                                            <b>¥1500</b><span>¥500</span></p>
                                    </div>
                                </li>
                                <li>
                                    <div>
                                        <h3>
                                            <a href="#">
                                                <img alt="产品图片" src="../static/images/product.jpg" width="100" height="100"></a></h3>
                                        <p>
                                            <a class="name" title="商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end" href="product.html">
                                                商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end</a></p>
                                        <p>
                                            <span class="adText">促销促销促销促销品名称商品品名称商品品名称商品销促销促销促销促销end</span>
                                        </p>
                                        <p class="price">
                                            <b>¥1500</b><span>¥500</span></p>
                                    </div>
                                </li>
                                <li>
                                    <div>
                                        <h3>
                                            <a href="#">
                                                <img alt="产品图片" src="../static/images/product.jpg" width="100" height="100"></a></h3>
                                        <p>
                                            <a class="name" title="商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end" href="product.html">
                                                商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end</a></p>
                                        <p>
                                            <span class="adText">促销促销促销促销品名称商品品名称商品品名称商品销促销促销促销促销end</span>
                                        </p>
                                        <p class="price">
                                            <b>¥1500</b><span>¥500</span></p>
                                    </div>
                                </li>
                                <li>
                                    <div>
                                        <h3>
                                            <a href="#">
                                                <img alt="产品图片" src="../static/images/product.jpg" width="100" height="100"></a></h3>
                                        <p>
                                            <a class="name" title="商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end" href="product.html">
                                                商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end</a></p>
                                        <p>
                                            <span class="adText">促销促销促销促销品名称商品品名称商品品名称商品销促销促销促销促销end</span>
                                        </p>
                                        <p class="price">
                                            <b>¥1500</b><span>¥500</span></p>
                                    </div>
                                </li>
                            </ul>
                        </div>
                        <div class="sort2Con">
                            <ul class="product sortContent">
                                <li>
                                    <div>
                                        <h3>
                                            <a href="#">
                                                <img alt="产品图片" src="../static/images/product.jpg" width="100" height="100"></a></h3>
                                        <p>
                                            <a class="name" title="商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end" href="product.html">
                                                3end</a></p>
                                        <p>
                                            <span class="adText">促销促销促销促销品名称商品品名称商品品名称商品销促销促销促销促销end</span>
                                        </p>
                                        <p class="price">
                                            <b>¥1500</b><span>¥500</span></p>
                                    </div>
                                </li>
                                <li>
                                    <div>
                                        <h3>
                                            <a href="#">
                                                <img alt="产品图片" src="../static/images/product.jpg" width="100" height="100"></a></h3>
                                        <p>
                                            <a class="name" title="商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end" href="product.html">
                                                商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end</a></p>
                                        <p>
                                            <span class="adText">促销促销促销促销品名称商品品名称商品品名称商品销促销促销促销促销end</span>
                                        </p>
                                        <p class="price">
                                            <b>¥1500</b><span>¥500</span></p>
                                    </div>
                                </li>
                                <li>
                                    <div>
                                        <h3>
                                            <a href="#">
                                                <img alt="产品图片" src="../static/images/product.jpg" width="100" height="100"></a></h3>
                                        <p>
                                            <a class="name" title="商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end" href="product.html">
                                                商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end</a></p>
                                        <p>
                                            <span class="adText">促销促销促销促销品名称商品品名称商品品名称商品销促销促销促销促销end</span>
                                        </p>
                                        <p class="price">
                                            <b>¥1500</b><span>¥500</span></p>
                                    </div>
                                </li>
                                <li>
                                    <div>
                                        <h3>
                                            <a href="#">
                                                <img alt="产品图片" src="../static/images/product.jpg" width="100" height="100"></a></h3>
                                        <p>
                                            <a class="name" title="商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end" href="product.html">
                                                商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end</a></p>
                                        <p>
                                            <span class="adText">促销促销促销促销品名称商品品名称商品品名称商品销促销促销促销促销end</span>
                                        </p>
                                        <p class="price">
                                            <b>¥1500</b><span>¥500</span></p>
                                    </div>
                                </li>
                                <li>
                                    <div>
                                        <h3>
                                            <a href="#">
                                                <img alt="产品图片" src="../static/images/product.jpg" width="100" height="100"></a></h3>
                                        <p>
                                            <a class="name" title="商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end" href="product.html">
                                                商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end</a></p>
                                        <p>
                                            <span class="adText">促销促销促销促销品名称商品品名称商品品名称商品销促销促销促销促销end</span>
                                        </p>
                                        <p class="price">
                                            <b>¥1500</b><span>¥500</span></p>
                                    </div>
                                </li>
                                <li>
                                    <div>
                                        <h3>
                                            <a href="#">
                                                <img alt="产品图片" src="../static/images/product.jpg" width="100" height="100"></a></h3>
                                        <p>
                                            <a class="name" title="商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end" href="product.html">
                                                商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end</a></p>
                                        <p>
                                            <span class="adText">促销促销促销促销品名称商品品名称商品品名称商品销促销促销促销促销end</span>
                                        </p>
                                        <p class="price">
                                            <b>¥1500</b><span>¥500</span></p>
                                    </div>
                                </li>
                            </ul>
                        </div>
                        <div class="sort2Con">
                            <ul class="product sortContent">
                                <li>
                                    <div>
                                        <h3>
                                            <a href="#">
                                                <img alt="产品图片" src="../static/images/product.jpg" width="100" height="100"></a></h3>
                                        <p>
                                            <a class="name" title="商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end" href="product.html">
                                                4end</a></p>
                                        <p>
                                            <span class="adText">促销促销促销促销品名称商品品名称商品品名称商品销促销促销促销促销end</span>
                                        </p>
                                        <p class="price">
                                            <b>¥1500</b><span>¥500</span></p>
                                    </div>
                                </li>
                                <li>
                                    <div>
                                        <h3>
                                            <a href="#">
                                                <img alt="产品图片" src="../static/images/product.jpg" width="100" height="100"></a></h3>
                                        <p>
                                            <a class="name" title="商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end" href="product.html">
                                                商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end</a></p>
                                        <p>
                                            <span class="adText">促销促销促销促销品名称商品品名称商品品名称商品销促销促销促销促销end</span>
                                        </p>
                                        <p class="price">
                                            <b>¥1500</b><span>¥500</span></p>
                                    </div>
                                </li>
                                <li>
                                    <div>
                                        <h3>
                                            <a href="#">
                                                <img alt="产品图片" src="../static/images/product.jpg" width="100" height="100"></a></h3>
                                        <p>
                                            <a class="name" title="商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end" href="product.html">
                                                商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end</a></p>
                                        <p>
                                            <span class="adText">促销促销促销促销品名称商品品名称商品品名称商品销促销促销促销促销end</span>
                                        </p>
                                        <p class="price">
                                            <b>¥1500</b><span>¥500</span></p>
                                    </div>
                                </li>
                                <li>
                                    <div>
                                        <h3>
                                            <a href="#">
                                                <img alt="产品图片" src="../static/images/product.jpg" width="100" height="100"></a></h3>
                                        <p>
                                            <a class="name" title="商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end" href="product.html">
                                                商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end</a></p>
                                        <p>
                                            <span class="adText">促销促销促销促销品名称商品品名称商品品名称商品销促销促销促销促销end</span>
                                        </p>
                                        <p class="price">
                                            <b>¥1500</b><span>¥500</span></p>
                                    </div>
                                </li>
                                <li>
                                    <div>
                                        <h3>
                                            <a href="#">
                                                <img alt="产品图片" src="../static/images/product.jpg" width="100" height="100"></a></h3>
                                        <p>
                                            <a class="name" title="商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end" href="product.html">
                                                商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end</a></p>
                                        <p>
                                            <span class="adText">促销促销促销促销品名称商品品名称商品品名称商品销促销促销促销促销end</span>
                                        </p>
                                        <p class="price">
                                            <b>¥1500</b><span>¥500</span></p>
                                    </div>
                                </li>
                                <li>
                                    <div>
                                        <h3>
                                            <a href="#">
                                                <img alt="产品图片" src="../static/images/product.jpg" width="100" height="100"></a></h3>
                                        <p>
                                            <a class="name" title="商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end" href="product.html">
                                                商品名称商品品名称商品品名称商品名称商品名称商品名称商品名称商品商品名称end</a></p>
                                        <p>
                                            <span class="adText">促销促销促销促销品名称商品品名称商品品名称商品销促销促销促销促销end</span>
                                        </p>
                                        <p class="price">
                                            <b>¥1500</b><span>¥500</span></p>
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <div class="band">
                        <span>品牌旗舰店</span>
                        <%=HomeBrandsHTML%>
                    </div>
                </div>
            </div>
            <div class="right">
                <h3>
                    个性解决方案</h3>
                <a href="product.html">
                    <img src="../static/images/ddys.jpg" width="274" height="291"></a>
                <div>
                    <em>主题晚宴</em><span>解决方案</span></div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content10" ContentPlaceHolderID="BackupContent2" runat="server">
</asp:Content>
<asp:Content ID="Content11" ContentPlaceHolderID="ScriptContent" runat="server">
    <script type="text/javascript">
       
        $(function () {
            $("#progressbar0").progressbar({
                value:50
            });
            // Hover states on the static widgets
            $("#dialog-link, #icons li").hover(
                function () {
                    $(this).addClass("ui-state-hover");
                },
                function () {
                    $(this).removeClass("ui-state-hover");
                }
            );

        });

        $(function () {
            $("#progressbar1").progressbar({
                value: 15
            });
            // Hover states on the static widgets
            $("#dialog-link, #icons li").hover(
                function () {
                    $(this).addClass("ui-state-hover");
                },
                function () {
                    $(this).removeClass("ui-state-hover");
                }
            );

        });



        $(function () {

            $("#panicBuyContents").children("li").each(function (index) {
                var $this = $(this);
                var startTime = new Date();
                var $endTime = $this.children(".time").children("input").val();
                $endTime = $endTime.replace(/\-/g, '/');
                $endTime = Date.parse($endTime);
                var remainSecond = (($endTime - startTime.getTime()) / 1000);
                var $timeDiv = $this.children("h4[class='time']");
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
