<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Site.Master" AutoEventWireup="true"
    CodeBehind="Product.aspx.cs" Inherits="YoeJoyWeb.Pages.Product" %>

<%@ Register Src="../Controls/CategoryNavigation.ascx" TagName="CategoryNavigation"
    TagPrefix="uc1" %>
<%@ Register Src="../Controls/SubCategoryNavigation.ascx" TagName="SubCategoryNavigation"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link type="text/css" rel="Stylesheet" href="../static/css/layout.css" />
    <link type="text/css" rel="Stylesheet" href="../static/css/product.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="LeftTopModule" runat="server">
    <uc1:CategoryNavigation ID="CategoryNavigation1" IsHomePage="false" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="RightTopModule" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MiddleTopModule" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="SiteNavModule" runat="server">
    <div id="breadNav">
        <p>
            您在：<a href="../Default.aspx">首页</a>〉<a id='c1NavLink' href="javascript:void(0);"></a>〉<a
                href="javascript:void(0);" id="c2NavLink"></a> 〉<a href="javascript:void(0);" id="c3NavLink"></a>〉<span
                    id="productNav"></span></p>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="LeftBigModule" runat="server">
    <div class="mix area">
        <div class="l_module">
            <uc2:SubCategoryNavigation ID="SubCategoryNavigation1" runat="server" />
            <div id="guessLike" class="box0 area">
                <div class="title">
                    <div class="mem0">
                    </div>
                    <h3>
                        猜你喜欢</h3>
                    <div class="mem1">
                    </div>
                </div>
                <%=ProductRelatedGuessYouLikeHTML%>
            </div>
            <div id="record" class="box0 area">
                <div class="title">
                    <div class="mem0">
                    </div>
                    <h3>
                        浏览记录</h3>
                    <div class="mem1">
                    </div>
                </div>
                <%=BrowserHistoryProductListHTML%>
            </div>
        </div>
        <div class="r_mix">
            <!--商品购买区Begin-->
            <div id="buyArea">
                <%=ProductNameInfoHTML %>
                <div id="photoShow">
                    <div class="bigPhoto">
                    </div>
                    <div style="left: 6px; top: 207px; display: none;" class="magnifier">
                    </div>
                    <div class="smallPhoto scrollItem">
                        <div class="prev">
                            <a href="javascript:"></a>
                        </div>
                        <div class="photoItem member1">
                            <%=ProductDetailImagesHTML %>
                        </div>
                        <div class="next">
                            <a style="display: inline-block;" href="javascript:"></a>
                        </div>
                    </div>
                </div>
                <div class="bigWindow">
                </div>
                <!--商品照片展示End-->
                <!--商品购买信息区Begin-->
                <%=ProductBaiscInfoHTML%>
                <p class="share">
                    分享到：<a class="sina" href="javascript:void(0);">新浪微博</a><a class="qq" href="javascript:void(0);">腾迅微博</a><a
                        class="kaixin" href="javascript:void(0);">开心网</a></p>
                <!--商品购买信息区End-->
            </div>
            <!--商品购买区End-->
            <!--浏览过该商品的用户还看过begin-->
            <div id="alsoSeen">
                <p class="title">
                    浏览过该商品的用户还看过</p>
                <%=ProductAlsoSeenHTML%>
            </div>
            <!--浏览过该商品的用户还看过end-->
            <!--商品综合信息区域Begin-->
            <ul class="introMenu">
                <li class="selected">商品介绍</li>
                <li>规格参数</li>
                <li>包装清单</li>
                <li>商品评价</li>
                <li>商品问答</li>
            </ul>
            <!--商品综合信息Begin-->
            <div id="introduce">
                <!--商品介绍Begin-->
                <div class="group selected">
                    <%=ProductLongDescriptionHTML%>
                </div>
                <!--商品介绍End-->
                <!--规格参数Begin-->
                <div class="group" id="productAttrSummery">
                    <%=ProductAttrSummeryHTML%>
                </div>
                <!--规格参数End-->
                <!--包装清单Begin-->
                <div class="group">
                    <%=ProductPackageListHTML%></div>
                <!--包装清单End-->
                <!--商品评价Begin-->
                <div id="comments" class="group">
                    <h3>
                        商品评价</h3>
                    <div class="comments-head clear">
                        <dl class="left clear">
                            <dt>
                                <p class="mast" align="center">
                                    <strong>90</strong>%</p>
                                <p class="slave" align="center">
                                    好评度</p>
                            </dt>
                            <dd>
                                <p>
                                    共<span class="mast0">364</span>人评价</p>
                                <ul>
                                    <li style='background: url("../../static/../static/images/showbg.png") no-repeat -49px -380px;'
                                        class="progressBar"><em>好评</em><span>(97%)</span></li>
                                    <li style='background: url("../../static/../static/images/showbg.png") no-repeat -139px -380px;'
                                        class="progressBar"><em>中评</em><span>(3%)</span></li>
                                    <li style='background: url("../../static/../static/images/showbg.png") no-repeat -94px -380px;'
                                        class="progressBar"><em>差评</em><span>(50%)</span></li>
                                </ul>
                            </dd>
                        </dl>
                        <div class="center">
                            <p>
                                发表评价即可获得积分,前五位评价用户可获得多倍积分:<a class="link1" href="help.html">详见积分规则</a></p>
                            <ul class="clear">
                                <li>1．<a class="link1" href="javascript:void(0)">会员甲</a><span class="slave">+10</span></li>
                                <li>2．<a class="link1" href="javascript:void(0)">会员乙</a><span class="slave">+10</span></li>
                                <li>3．<a class="link1" href="javascript:void(0)">会员丙</a><span class="slave">+10</span></li>
                                <li>4．<a class="link1" href="javascript:void(0)">会员丁</a><span class="slave">+10</span></li>
                            </ul>
                        </div>
                        <div class="right">
                            <p align="center">
                                我在攸怡购买过该商品</p>
                            <p align="center">
                                <a class="bt1" href="javascript:void(0)">我要评价</a></p>
                            <p align="center">
                                <a class="link1" href="comments.html">查看全部评价</a></p>
                        </div>
                    </div>
                    <ul class="menu clear">
                        <li class="selected"><a href="javascript:">全部评价(8233)</a></li>
                        <li><a href="javascript:">好评(8025)</a></li>
                        <li><a href="javascript:">中评(173)</a></li>
                        <li><a href="javascript:">差评(35)</a></li>
                    </ul>
                    <div class="allComment">
                        <div class="comment selected">
                            <div class="item clear">
                                <div class="left">
                                    <p align="center">
                                        <a class="photo" href="all-comment.html">
                                            <img alt="salfjk头像" src="../static/../static/images/tx02.jpg" width="46" height="46"></a></p>
                                    <p align="center">
                                        <a class="link0" href="all-comment.html">salfjk</a></p>
                                    <p align="center">
                                        三星会员</p>
                                    <p class="slave" align="center">
                                        (河北)</p>
                                    <p class="slave" align="center">
                                        购买日期</p>
                                    <p class="slave" align="center">
                                        2012-01-20</p>
                                </div>
                                <div class="right">
                                    <div class="title">
                                        <p>
                                            <img alt="精华评论" src="../static/../static/images/jing.png" width="18" height="18"><a
                                                href="comment.html">机子很不错</a><img alt="一星级" src="../static/../static/images/pf1.png"
                                                    width="16" height="16"><img alt="一星级" src="../static/../static/images/pf1.png" width="16"
                                                        height="16"><img alt="一星级" src="../static/../static/images/pf1.png" width="16" height="16"><img
                                                            alt="一星级" src="../static/../static/images/pf1.png" width="16" height="16"><img alt="一星级"
                                                                src="../static/../static/images/pf1.png" width="16" height="16"><span class="slave">2012-05-23
                                                                    13:23:30</span>
                                        </p>
                                        <hr>
                                    </div>
                                    <div class="allContent">
                                        <div class="argument">
                                            <p>
                                                <em>优点：</em>灯光很亮！</p>
                                            <p>
                                                <em>不足：</em>暂时还没发现缺点哦！</p>
                                            <p>
                                                <em>购买心得：</em>比较新颖好看！</p>
                                            <p align="right">
                                                <button class="bt6">
                                                    回复</button>
                                            </p>
                                            <div id="reply" class="replyBox">
                                            </div>
                                        </div>
                                        <ul class="reply">
                                            <li>
                                                <p>
                                                    <strong class="slave">53</strong><a class="link0" href="javascript:">adfjak</a><span
                                                        class="slave">回复说：</span><em class="slave">2012-05-30&nbsp;13:03:45</em></p>
                                                <p class="content">
                                                    用暖光灯，开灯时效果很好。灯罩与屋顶有1cm左右的间距，屋顶上映出温馨的光环，底盘上也不会映出节能灯。安装方便。不知道时间长了，灯罩里是否会积灰尘。</p>
                                            </li>
                                            <li>
                                                <p>
                                                    <strong class="slave">52</strong><a class="link0" href="javascript:">adfjak</a><span
                                                        class="slave">回复说：</span><em class="slave">2012-05-30&nbsp;13:03:45</em></p>
                                                <p class="content">
                                                    用暖光灯，开灯时效果很好。灯罩与屋顶有1cm左右的间距，屋顶上映出温馨的光环，底盘上也不会映出节能灯。安装方便。不知道时间长了，灯罩里是否会积灰尘。</p>
                                            </li>
                                            <li>
                                                <p>
                                                    <strong class="slave">51</strong><a class="link0" href="javascript:">adfjak</a><span
                                                        class="slave">回复说：</span><em class="slave">2012-05-30&nbsp;13:03:45</em></p>
                                                <p class="content">
                                                    用暖光灯，开灯时效果很好。灯罩与屋顶有1cm左右的间距，屋顶上映出温馨的光环，底盘上也不会映出节能灯。安装方便。不知道时间长了，灯罩里是否会积灰尘。</p>
                                            </li>
                                        </ul>
                                        <p align="center">
                                            <a href="comment.html">查看全部53条回复〉〉</a></p>
                                    </div>
                                </div>
                            </div>
                            <div class="item clear">
                                <div class="left">
                                    <p align="center">
                                        <a class="photo" href="all-comment.html">
                                            <img alt="salfjk头像" src="../static/../static/images/tx02.jpg" width="46" height="46"></a></p>
                                    <p align="center">
                                        <a class="link0" href="all-comment.html">salfjk</a></p>
                                    <p align="center">
                                        三星会员</p>
                                    <p class="slave" align="center">
                                        (河北)</p>
                                    <p class="slave" align="center">
                                        购买日期</p>
                                    <p class="slave" align="center">
                                        2012-01-20</p>
                                </div>
                                <div class="right">
                                    <div class="title">
                                        <p>
                                            <img alt="精华评论" src="../static/../static/images/jing.png" width="18" height="18"><a
                                                href="comment.html">机子很不错</a><img alt="一星级" src="../static/../static/images/pf1.png"
                                                    width="16" height="16"><img alt="一星级" src="../static/../static/images/pf1.png" width="16"
                                                        height="16"><img alt="一星级" src="../static/../static/images/pf1.png" width="16" height="16"><img
                                                            alt="一星级" src="../static/../static/images/pf1.png" width="16" height="16"><img alt="一星级"
                                                                src="../static/../static/images/pf1.png" width="16" height="16"><span class="slave">2012-05-23
                                                                    13:23:30</span>
                                        </p>
                                        <hr>
                                    </div>
                                    <div class="allContent">
                                        <div class="argument">
                                            <p>
                                                <em>优点：</em>灯光很亮！</p>
                                            <p>
                                                <em>不足：</em>暂时还没发现缺点哦！</p>
                                            <p>
                                                <em>购买心得：</em>比较新颖好看！</p>
                                            <p align="right">
                                                <button class="bt6">
                                                    回复</button>
                                            </p>
                                            <div id="reply" class="replyBox">
                                            </div>
                                        </div>
                                        <ul class="reply">
                                            <li>
                                                <p>
                                                    <strong class="slave">1</strong><a class="link0" href="javascript:">adfjak</a><span
                                                        class="slave">回复说：</span><em class="slave">2012-05-30&nbsp;13:03:45</em></p>
                                                <p class="content">
                                                    用暖光灯，开灯时效果很好。灯罩与屋顶有1cm左右的间距，屋顶上映出温馨的光环，底盘上也不会映出节能灯。安装方便。不知道时间长了，灯罩里是否会积灰尘。</p>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                            <div class="item clear">
                                <div class="left">
                                    <p align="center">
                                        <a class="photo" href="all-comment.html">
                                            <img alt="salfjk头像" src="../static/../static/images/tx02.jpg" width="46" height="46"></a></p>
                                    <p align="center">
                                        <a class="link0" href="all-comment.html">salfjk</a></p>
                                    <p align="center">
                                        三星会员</p>
                                    <p class="slave" align="center">
                                        (河北)</p>
                                    <p class="slave" align="center">
                                        购买日期</p>
                                    <p class="slave" align="center">
                                        2012-01-20</p>
                                </div>
                                <div class="right">
                                    <div class="title">
                                        <p>
                                            <img alt="精华评论" src="../static/../static/images/jing.png" width="18" height="18"><a
                                                href="comment.html">机子很不错</a><img alt="一星级" src="../static/../static/images/pf1.png"
                                                    width="16" height="16"><img alt="一星级" src="../static/../static/images/pf1.png" width="16"
                                                        height="16"><img alt="一星级" src="../static/../static/images/pf1.png" width="16" height="16"><img
                                                            alt="一星级" src="../static/../static/images/pf1.png" width="16" height="16"><img alt="一星级"
                                                                src="../static/../static/images/pf1.png" width="16" height="16"><span class="slave">2012-05-23
                                                                    13:23:30</span>
                                        </p>
                                        <hr>
                                    </div>
                                    <div class="allContent">
                                        <div class="argument">
                                            <p>
                                                <em>优点：</em>灯光很亮！</p>
                                            <p>
                                                <em>不足：</em>暂时还没发现缺点哦！</p>
                                            <p>
                                                <em>购买心得：</em>比较新颖好看！</p>
                                            <p align="right">
                                                <button class="bt6">
                                                    回复</button>
                                            </p>
                                            <div id="reply" class="replyBox">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="comment">
                            <div class="item clear">
                                <div class="left">
                                    <p align="center">
                                        <a class="photo" href="all-comment.html">
                                            <img alt="salfjk头像" src="../static/../static/images/tx02.jpg" width="46" height="46"></a></p>
                                    <p align="center">
                                        <a class="link0" href="all-comment.html">salfjk</a></p>
                                    <p align="center">
                                        三星会员</p>
                                    <p class="slave" align="center">
                                        (河北)</p>
                                    <p class="slave" align="center">
                                        购买日期</p>
                                    <p class="slave" align="center">
                                        2012-01-20</p>
                                </div>
                                <div class="right">
                                    <div class="title">
                                        <p>
                                            <img alt="精华评论" src="../static/../static/images/jing.png" width="18" height="18"><a
                                                href="comment.html">机子很不错</a><img alt="一星级" src="../static/../static/images/pf1.png"
                                                    width="16" height="16"><img alt="一星级" src="../static/../static/images/pf1.png" width="16"
                                                        height="16"><img alt="一星级" src="../static/../static/images/pf1.png" width="16" height="16"><img
                                                            alt="一星级" src="../static/../static/images/pf1.png" width="16" height="16"><img alt="一星级"
                                                                src="../static/../static/images/pf1.png" width="16" height="16"><span class="slave">2012-05-23
                                                                    13:23:30</span>
                                        </p>
                                        <hr>
                                    </div>
                                    <div class="allContent">
                                        <div class="argument">
                                            <p>
                                                <em>优点：</em>灯光很亮！</p>
                                            <p>
                                                <em>不足：</em>暂时还没发现缺点哦！</p>
                                            <p>
                                                <em>购买心得：</em>比较新颖好看！</p>
                                            <p align="right">
                                                <button class="bt6">
                                                    回复</button>
                                            </p>
                                            <div id="reply" class="replyBox">
                                            </div>
                                        </div>
                                        <ul class="reply">
                                            <li>
                                                <p>
                                                    <strong class="slave">1</strong><a class="link0" href="javascript:">adfjak</a><span
                                                        class="slave">回复说：</span><em class="slave">2012-05-30&nbsp;13:03:45</em></p>
                                                <p class="content">
                                                    用暖光灯，开灯时效果很好。灯罩与屋顶有1cm左右的间距，屋顶上映出温馨的光环，底盘上也不会映出节能灯。安装方便。不知道时间长了，灯罩里是否会积灰尘。</p>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="comment">
                            <div class="item clear">
                                <div class="left">
                                    <p align="center">
                                        <a class="photo" href="all-comment.html">
                                            <img alt="salfjk头像" src="../static/../static/images/tx02.jpg" width="46" height="46"></a></p>
                                    <p align="center">
                                        <a class="link0" href="all-comment.html">salfjk</a></p>
                                    <p align="center">
                                        三星会员</p>
                                    <p class="slave" align="center">
                                        (河北)</p>
                                    <p class="slave" align="center">
                                        购买日期</p>
                                    <p class="slave" align="center">
                                        2012-01-20</p>
                                </div>
                                <div class="right">
                                    <div class="title">
                                        <p>
                                            <img alt="精华评论" src="../static/../static/images/jing.png" width="18" height="18"><a
                                                href="comment.html">机子很不错</a><img alt="一星级" src="../static/../static/images/pf1.png"
                                                    width="16" height="16"><img alt="一星级" src="../static/../static/images/pf1.png" width="16"
                                                        height="16"><img alt="一星级" src="../static/../static/images/pf1.png" width="16" height="16"><img
                                                            alt="一星级" src="../static/../static/images/pf1.png" width="16" height="16"><img alt="一星级"
                                                                src="../static/../static/images/pf1.png" width="16" height="16"><span class="slave">2012-05-23
                                                                    13:23:30</span>
                                        </p>
                                        <hr>
                                    </div>
                                    <div class="allContent">
                                        <div class="argument">
                                            <p>
                                                <em>优点：</em>灯光很亮！</p>
                                            <p>
                                                <em>不足：</em>暂时还没发现缺点哦！</p>
                                            <p>
                                                <em>购买心得：</em>比较新颖好看！</p>
                                            <p align="right">
                                                <button class="bt6">
                                                    回复</button>
                                            </p>
                                            <div id="reply" class="replyBox">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="comment">
                            <div class="item clear">
                                <div class="left">
                                    <p align="center">
                                        <a class="photo" href="all-comment.html">
                                            <img alt="salfjk头像" src="../static/../static/images/tx02.jpg" width="46" height="46"></a></p>
                                    <p align="center">
                                        <a class="link0" href="all-comment.html">salfjk</a></p>
                                    <p align="center">
                                        三星会员</p>
                                    <p class="slave" align="center">
                                        (河北)</p>
                                    <p class="slave" align="center">
                                        购买日期</p>
                                    <p class="slave" align="center">
                                        2012-01-20</p>
                                </div>
                                <div class="right">
                                    <div class="title">
                                        <p>
                                            <img alt="精华评论" src="../static/../static/images/jing.png" width="18" height="18"><a
                                                href="comment.html">机子很不错</a><img alt="一星级" src="../static/../static/images/pf1.png"
                                                    width="16" height="16"><img alt="一星级" src="../static/../static/images/pf1.png" width="16"
                                                        height="16"><img alt="一星级" src="../static/../static/images/pf1.png" width="16" height="16"><img
                                                            alt="一星级" src="../static/../static/images/pf1.png" width="16" height="16"><img alt="一星级"
                                                                src="../static/../static/images/pf1.png" width="16" height="16"><span class="slave">2012-05-23
                                                                    13:23:30</span>
                                        </p>
                                        <hr>
                                    </div>
                                    <div class="allContent">
                                        <div class="argument">
                                            <p>
                                                <em>优点：</em>灯光很亮！</p>
                                            <p>
                                                <em>不足：</em>暂时还没发现缺点哦！</p>
                                            <p>
                                                <em>购买心得：</em>比较新颖好看！</p>
                                            <p align="right">
                                                <button class="bt6">
                                                    回复</button>
                                            </p>
                                            <div id="reply" class="replyBox">
                                            </div>
                                        </div>
                                        <ul class="reply">
                                            <li>
                                                <p>
                                                    <strong class="slave">53</strong><a class="link0" href="javascript:">adfjak</a><span
                                                        class="slave">回复说：</span><em class="slave">2012-05-30&nbsp;13:03:45</em></p>
                                                <p class="content">
                                                    用暖光灯，开灯时效果很好。灯罩与屋顶有1cm左右的间距，屋顶上映出温馨的光环，底盘上也不会映出节能灯。安装方便。不知道时间长了，灯罩里是否会积灰尘。</p>
                                            </li>
                                            <li>
                                                <p>
                                                    <strong class="slave">51</strong><a class="link0" href="javascript:">adfjak</a><span
                                                        class="slave">回复说：</span><em class="slave">2012-05-30&nbsp;13:03:45</em></p>
                                                <p class="content">
                                                    用暖光灯，开灯时效果很好。灯罩与屋顶有1cm左右的间距，屋顶上映出温馨的光环，底盘上也不会映出节能灯。安装方便。不知道时间长了，灯罩里是否会积灰尘。</p>
                                            </li>
                                        </ul>
                                        <p align="center">
                                            <a href="comment.html">查看全部53条回复〉〉</a></p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <p id="turnPage1" align="right">
                        <a class="currPage" href="javascript:">1</a><a href="javascript:">2</a><a href="javascript:">3</a><a
                            href="javascript:">4</a><span>...</span><a href="javascript:">14</a><a class="next"
                                href="javascript:">下一页</a></p>
                    <p class="fastBuy" align="center">
                        <span>我要买：</span><a class="sub" href="javascript:void(0)">-</a>
                        <input class="num" maxlength="3" value="1" type="text" />
                        <a class="add" href="javascript:void(0)">+</a><span>个</span><a class="bt4" href="javascript:void(0);">加入购物车</a></p>
                </div>
                <!--商品评价End-->
                <!--商品问答Begin-->
                <div class="group qa">
                    <p class="alignLine">
                        <span>关于产品价格，保质期，促销活动的留言具有时效性，回复仅供参考只在一定时间内有效。</span><a class="rColumn link1" href="javascript:">我要提问</a></p>
                    <div class="singleQA">
                        <dl class="question">
                            <dt><span></span><em>这款巧克力，送手提袋吗？</em> </dt>
                            <dd>
                                <p align="right">
                                    提问者：<em>208800230**</em></p>
                                <p align="right">
                                    2012-11-02&nbsp;05:20:30</p>
                            </dd>
                        </dl>
                        <dl class="answer">
                            <dt><span></span><em>您好，由于部分商品包装更换较为频繁和商品批次不同，具体请您以收到的商品实物为准，但同时我们承诺到货的商品均为正品行货，请放心使用，感谢您支持1号店，祝您购物愉快！</em>
                            </dt>
                            <dd>
                                <p align="right">
                                    提问者：<em>208800230**</em></p>
                                <p align="right">
                                    2012-11-02&nbsp;05:20:30</p>
                            </dd>
                        </dl>
                        <p class="mem" align="right">
                            <span>共有1条解答</span><a class="bt3" href="javascript:">我来解答</a></p>
                    </div>
                    <div class="singleQA">
                        <dl class="question">
                            <dt><span></span><em>这款巧克力，送手提袋吗？</em> </dt>
                            <dd>
                                <p align="right">
                                    提问者：<em>208800230**</em></p>
                                <p align="right">
                                    2012-11-02&nbsp;05:20:30</p>
                            </dd>
                        </dl>
                        <dl class="answer">
                            <dt><span></span><em>您好，由于部分商品包装更换较为频繁和商品批次不同，具体请您以收到的商品实物为准，但同时我们承诺到货的商品均为正品行货，请放心使用，感谢您支持1号店，祝您购物愉快！</em>
                            </dt>
                            <dd>
                                <p align="right">
                                    提问者：<em>208800230**</em></p>
                                <p align="right">
                                    2012-11-02&nbsp;05:20:30</p>
                            </dd>
                        </dl>
                        <p class="mem" align="right">
                            <span>共有1条解答</span><a class="bt3" href="javascript:">我来解答</a></p>
                    </div>
                    <p id="turnPage1" align="right">
                        <a class="currPage" href="javascript:">1</a><a href="javascript:">2</a><a href="javascript:">3</a><a
                            href="javascript:">4</a><span>...</span><a href="javascript:">14</a><a class="next"
                                href="javascript:">下一页</a></p>
                    <p class="fastBuy" align="center">
                        <span>我要买：</span><a class="sub" href="javascript:void(0)">-</a>
                        <input class="num" maxlength="3" value="1" type="text">
                        <a class="add" href="javascript:void(0)">+</a><span>个</span><a class="bt4" href="javascript:void(0);">加入购物车</a></p>
                </div>
                <!--商品问答End-->
            </div>
            <!--商品综合信息End-->
            <!--商品综合信息区域End-->
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="RightBigModule" runat="server">
    <div id="cartCheck" class="popbox">
        <div class="tBorder">
        </div>
        <div class="content">
            <h6 class="alignLine">
                1件商品加入购物车<a class="rColumn close">关闭<b>X</b></a></h6>
            <div class="group0">
                <div class="item0" id="cartCheckImg">
                    <img alt="顶级音乐大屏智能手机！Motorola 摩托罗拉 XT550交响黑 WCDMA" src="../static/images/sp.jpg"
                        width="130" height="130"></div>
                <div class="item1">
                    <div class="member0" id="cartCheckDetail">
                        <h2>
                            顶级音乐大屏智能手机！Motorola 摩托罗拉 XT550交响黑 WCDMA</h2>
                        <p>
                            加入数量：<span>1</span></p>
                        <p>
                            总计金额：<span>¥5298</span></p>
                    </div>
                    <div class="member1">
                        <a class="close" href="javascript:void(0)">继续购物</a>&nbsp;&nbsp;<a class="settlement"
                            href="../Shopping/ShoppingCart.aspx">去结算</a></div>
                </div>
            </div>
            <!--购买了此商品的用户还买了Begin-->
            <div class="group1">
                <h5>
                    购买了此商品的用户还买了：</h5>
                <div class="item0 scrollItem">
                    <div class="member0 dot">
                    </div>
                    <div class="prev">
                        <a href="javascript:void(0);"></a>
                    </div>
                    <div class="member1 hxzsc">
                        <%=ProductAlsoBuyHTML%>
                    </div>
                    <div class="next">
                        <a href="javascript:void(0);"></a>
                    </div>
                </div>
            </div>
            <!--购买了此商品的用户还买了End-->
        </div>
        <div class="bBorder">
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="BackupContent1" runat="server">
    <div id="fComment" class="popbox">
        <div class="tBorder">
        </div>
        <div class="content">
            <h6 class="alignLine">
                发表评论<a class="rColumn close">关闭<b>X</b></a></h6>
            <p class="level">
                <em>评星级：</em><span><a class="selected" href="javascript:"></a><a href="javascript:"></a><a
                    href="javascript:"></a><a href="javascript:"></a><a href="javascript:"></a></span></p>
            <p>
                <em>标题：</em><input name="title" type="text"></p>
            <p>
                <em>优点：</em><input name="good" type="text"></p>
            <p>
                <em>不足：</em><input name="bad" type="text"></p>
            <p>
                <em>购买心得：</em></p>
            <p>
                <textarea></textarea></p>
            <p class="button">
                <button name="submit">
                    提交评论</button></p>
        </div>
        <div class="bBorder">
        </div>
    </div>
    <div id="questions" class="popbox qaBox">
        <div class="tBorder">
        </div>
        <div class="content">
            <h6 class="alignLine">
                我要提问<a class="rColumn close">关闭<b>X</b></a></h6>
            <br>
            <p>
                <em>问题：</em></p>
            <p>
                <textarea></textarea></p>
            <br>
            <p align="center">
                <button>
                    我要提问</button></p>
            <br>
            <p class="mast0">
                提问小贴士</p>
            <p>
                1、攸怡鼓励您对已有的产品问题做出解答，一个问题只能回答一次。</p>
            <p>
                2、问答字数限制：10～100个字以内。
            </p>
            <p>
                3、若您提交信息出现与产品无关的冗余信息或涉及广告、比价、重复反馈、不实 评论、恶意评论、粗口、危害国家安全等不当言论时，或经攸怡查实您存在自问自答等作弊行为，攸怡有权予以删除其内容。</p>
            <br>
        </div>
        <div class="bBorder">
        </div>
    </div>
    <div id="answer" class="popbox qaBox">
        <div class="tBorder">
        </div>
        <div class="content">
            <h6 class="alignLine">
                我要回答<a class="rColumn close">关闭<b>X</b></a></h6>
            <br>
            <p>
                <em>问题：</em>这款巧克力，送手提袋吗？</p>
            <br>
            <p>
                <em>解答：</em></p>
            <p>
                <textarea></textarea></p>
            <br>
            <p align="center">
                <button>
                    我来解答</button></p>
            <br>
            <p class="mast0">
                提问小贴士</p>
            <p>
                1、攸怡鼓励您对已有的产品问题做出解答，一个问题只能回答一次。</p>
            <p>
                2、问答字数限制：10～100个字以内。
            </p>
            <p>
                3、若您提交信息出现与产品无关的冗余信息或涉及广告、比价、重复反馈、不实 评论、恶意评论、粗口、危害国家安全等不当言论时，或经攸怡查实您存在自问自答等作弊行为，攸怡有权予以删除其内容。</p>
            <br />
        </div>
        <div class="bBorder">
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="HomeMiddleContent" runat="server">
</asp:Content>
<asp:Content ID="Content10" ContentPlaceHolderID="BackupContent2" runat="server">
</asp:Content>
<asp:Content ID="Content11" ContentPlaceHolderID="ScriptContent" runat="server">
    <script type="text/javascript">

        function RefreshOnlineShoppingCart() {
            var shoppingCartServiceURL = YoeJoy.Site.Utility.GetSiteBaseURL(false) + "/Service/ShoppingCartService.aspx?cmd=view&random=" + Math.random();
            //var shoppingCartServiceURL = YoeJoy.Site.Utility.GetSiteBaseURL(false) + "/Service/ShoppingCartService.aspx?cmd=view";
            $.get(shoppingCartServiceURL, function (data) {
                $("#count").empty().append(data);
                ReCartBindHoverEvent();
            });
        };

        function ReCartBindHoverEvent() {

            var char = $('#count img');
            var charf = $('#count');
            var charContent = $('#chartContent');
            var car = $('#chart img');

            char.hover(function () {
                charContent.css('display', 'block');
                car.attr({ 'src': '../static/images/gwcbt1.png' });
            }, function () {
            });

            charf.hover(function () {
            }, function () {
                charContent.css('display', 'none');
                car.attr({ 'src': '../static/images/gwcbt0.png' });
            });

            charContent.hover(function () {
                charContent.show();
                car.attr({ 'src': '../static/images/gwcbt1.png' });
            }, function () {
                charContent.hide();
                car.attr({ 'src': '../static/images/gwcbt0.png' });
            });
        };

        $(function () {

            var c1 = YoeJoy.Site.Utility.GetQueryString("c1");
            var c2 = YoeJoy.Site.Utility.GetQueryString("c2");
            var c3 = YoeJoy.Site.Utility.GetQueryString("c3");
            var pid = YoeJoy.Site.Utility.GetQueryString("pid");

            var $productBriefName = $("#productBriefName").val();

            var $c1Name = $("#foodImport").children("h2").eq(0).children("b").eq(0).html();
            $("#breadNav").children("p").children("a").eq(1).html($c1Name);
            $("#breadNav").children("p").children("a").eq(1).click(function (event) {
                window.location.href = $("#siteBaseURL").val() + "Pages/SubProductList1.aspx?c1=" + c1;
            });
            var $c2Name = $(".listOut li input[value=" + c2 + "]").siblings("h3").text();
            $("#breadNav").children("p").children("a").eq(2).click(function (event) {
                window.location.href = $("#siteBaseURL").val() + "Pages/SubProductList2.aspx?c1=" + c1 + "&c2=" + c2;
            });
            $("#breadNav").children("p").children("a").eq(2).html($c2Name);
            var $c3Name = $(".listOut li p a input[value=" + c3 + "]").siblings("input").val();
            $("#breadNav").children("p").children("a").eq(3).html($c3Name);
            $("#breadNav").children("p").children("a").eq(3).click(function (event) {
                window.location.href = $("#siteBaseURL").val() + "Pages/SubProductList3.aspx?c1=" + c1 + "&c2=" + c2 + "&c3=" + c3;
            });
            $("#breadNav").children("p").children("span").eq(0).text($productBriefName);

            $("#btnAddToCart").click(function (event) {
                var shoppingCartServiceURL = "../Service/ShoppingCartService.aspx?cmd=add";
                var params = "pid=" + pid + "&qtp=1";
                $.post(shoppingCartServiceURL, params, function (data) {
                    var result = YoeJoy.Site.Utility.GetJsonStr(data);
                    if (result.IsSuccess) {
                        //alert(result.Msg);
                        RefreshOnlineShoppingCart();
                        return;
                    }
                    else {
                        alert(result.Msg);
                    }
                });
            });

        });
    </script>
</asp:Content>
