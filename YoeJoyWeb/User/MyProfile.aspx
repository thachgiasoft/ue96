<%@ Page Title="" Language="C#" MasterPageFile="~/Master/User.Master" AutoEventWireup="true"
    CodeBehind="MyProfile.aspx.cs" Inherits="YoeJoyWeb.User.MyProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="NavContentPlaceHolder" runat="server">
    <div id="position">
        <span>
            <img src="../static/images/f4.jpg" />您在:</span> <b><a href="../Default.aspx">首页</a></b>
        <span>&gt;</span> <span>用户中心</span>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="RightBigModule" runat="server">
    <!--用户基本信息-->
    <%=MyProfileHTML%>
    <!--近期订单-->
    <div class="item1">
        <h2>
            <b>近期订单(2)</b> <a href="#">查看所有订单</a>
        </h2>
        <table border="0" cellspacing="0" cellpadding="0" width="100%">
            <thead>
                <tr>
                    <th>
                        订单编号
                    </th>
                    <th>
                        下单日期
                    </th>
                    <th>
                        收货人
                    </th>
                    <th>
                        付款方式
                    </th>
                    <th>
                        总金额
                    </th>
                    <th>
                        状态
                    </th>
                    <th class="operation" width="102">
                        操作
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>
                        <a class="link1" href="javascript:void(0)">5646456</a>
                    </td>
                    <td>
                        2012-12-02
                    </td>
                    <td>
                        购物狂
                    </td>
                    <td>
                        在线支付
                    </td>
                    <td>
                        <span class="mast">¥10000.00</span>
                    </td>
                    <td>
                        待付
                    </td>
                    <td class="operation">
                        <a class="link1" href="javascript:void(0)">支付</a><a class="link1" href="javascript:void(0)">订单详情</a>
                    </td>
                </tr>
                <tr>
                    <td>
                        <a class="link1" href="javascript:void(0)">5646456</a>
                    </td>
                    <td>
                        2012-12-02
                    </td>
                    <td>
                        购物狂
                    </td>
                    <td>
                        在线支付
                    </td>
                    <td>
                        <span class="mast">¥10000.00</span>
                    </td>
                    <td>
                        待付
                    </td>
                    <td class="operation">
                        <a class="link1" href="javascript:void(0)">支付</a><a class="link1" href="javascript:void(0)">订单详情</a>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <!--近期收藏-->
    <div class="item1 collection">
        <h2>
            <b>近期收藏(2)</b> <a href="#">查看所有收藏</a>
        </h2>
        <div class="mem">
            <input type="checkbox">
            <span>全选</span><a class="link1" href="javascript:void(0)">删除</a></div>
        <table border="0" cellspacing="0" cellpadding="0" width="100%">
            <tbody>
                <tr>
                    <td width="20">
                        <input type="checkbox">
                    </td>
                    <td width="132">
                        <a href="../products/product.html">
                            <img src="../static/images/sp.jpg" width="90"></a>
                    </td>
                    <td>
                        <a class="name" title="Wyeth/惠氏 进口奶源 S-26金装幼儿乐 幼儿配方奶粉3段（1-3岁）1200g/盒" href="../products/product.html">
                            Wyeth/惠氏 进口奶源 S-26金装幼儿乐 幼儿配方奶粉3段（1-3岁）1200g/盒</a> <span class="adText">进口奶源 S-26金装幼儿乐促销促销促销促销促销促销促销</span>
                    </td>
                    <td class="price">
                        <b>¥1500</b><span>¥500</span>
                    </td>
                    <td>
                        <a class="link1" href="javascript:void(0)">加入购物车</a>
                    </td>
                </tr>
                <tr>
                    <td width="20">
                        <input type="checkbox">
                    </td>
                    <td width="132">
                        <a href="../products/product.html">
                            <img src="../static/images/sp.jpg" width="90"></a>
                    </td>
                    <td>
                        <a class="name" title="Wyeth/惠氏 进口奶源 S-26金装幼儿乐 幼儿配方奶粉3段（1-3岁）1200g/盒" href="../products/product.html">
                            Wyeth/惠氏 进口奶源 S-26金装幼儿乐 幼儿配方奶粉3段（1-3岁）1200g/盒</a> <span class="adText">进口奶源 S-26金装幼儿乐促销促销促销促销促销促销促销</span>
                    </td>
                    <td class="price">
                        <b>¥1500</b><span>¥500</span>
                    </td>
                    <td>
                        <a class="link1" href="javascript:void(0)">加入购物车</a>
                    </td>
                </tr>
            </tbody>
        </table>
        <div class="mem">
            <input type="checkbox">
            <span>全选</span><a class="link1" href="javascript:void(0)">删除</a></div>
    </div>
    <!--待评论商品-->
    <div class="item1 comments">
        <h2>
            <b>待评论商品(2)</b> <a href="#">更多〉〉</a>
        </h2>
        <div class="list0">
            <div class="show0">
                <div class="mem0">
                    <img src="../static/images/sp.jpg" width="90"></div>
                <div class="mem1">
                    <a class="name" title="Wyeth/惠氏 进口奶源 S-26金装幼儿乐 幼儿配方奶粉3段（1-3岁）1200g/盒" href="../products/product.html">
                        Wyeth/惠氏 进口奶源 S-26金装幼儿乐 幼儿配方奶粉3段（1-3岁）1200g/盒</a>
                    <p class="slave">
                        购买日期：2012-08-09</p>
                    <a class="bt1" href="javascript:void(0);">立刻评论</a></div>
            </div>
            <div class="show0">
                <div class="mem0">
                    <img src="../static/images/sp.jpg" width="90"></div>
                <div class="mem1">
                    <a class="name" title="Wyeth/惠氏 进口奶源 S-26金装幼儿乐 幼儿配方奶粉3段（1-3岁）1200g/盒" href="../products/product.html">
                        Wyeth/惠氏 进口奶源 S-26金装幼儿乐 幼儿配方奶粉3段（1-3岁）1200g/盒</a>
                    <p class="slave">
                        购买日期：2012-08-09</p>
                    <a class="bt1" href="javascript:void(0);">立刻评论</a></div>
            </div>
        </div>
    </div>
    <!--浏览记录-->
    <div class="history">
        <h2 class="titles">
            <b>您的浏览历史记录</b></h2>
        <div class="recentView">
            <div class="l">
                <h2>
                    我最近浏览的商品</h2>
                <%=MyRecentBroswerHistoryHTML %>
            </div>
            <div class="r">
                <div id="newFocus">
                    <h2>
                        看了您最近看过商品的顾客同时看了</h2>
                    <div id="newFocus">
                        <p id="NumTitle">
                        </p>
                        <div class="test">
                            <span class="prev"><a href="#"></a></span><span class="next"><a href="#"></a></span>
                        </div>
                        <ul class="focusContent">
                            <li id="animate"><span class="item"><a href="#">
                                <img src="../static/images/sp02.jpg"></a> <a class="name" title="Wyeth/惠氏 进口奶源 S-26金装幼儿乐 幼儿配方奶粉3段（1-3岁）1200g/盒"
                                    href="product.html">Wyeth/惠氏 进口奶源 S-26金装幼儿乐 幼儿配方奶粉3段（1-3岁）1200g/盒</a> <strong class="adText">
                                        Wyeth/惠氏 进口奶源 S-26金装幼儿乐 幼儿配方奶粉</strong>
                                <p class="price">
                                    <b>¥1500</b><span>¥500</span></p>
                                <p>
                                    <a href="#">评论:1000条</a></p>
                            </span><span class="item"><a href="#">
                                <img src="../static/images/sp02.jpg"></a> <a class="name" title="Wyeth/惠氏 进口奶源 S-26金装幼儿乐 幼儿配方奶粉3段（1-3岁）1200g/盒"
                                    href="product.html">Wyeth/惠氏 进口奶源 S-26金装幼儿乐 幼儿配方奶粉3段（1-3岁）1200g/盒</a> <strong class="adText">
                                        Wyeth/惠氏 进口奶源 S-26金装幼儿乐 幼儿配方奶粉</strong>
                                <p class="price">
                                    <b>¥1500</b><span>¥500</span></p>
                                <p>
                                    <a href="#">评论:1000条</a></p>
                            </span><span class="item"><a href="#">
                                <img src="../static/images/sp02.jpg"></a> <a class="name" title="Wyeth/惠氏 进口奶源 S-26金装幼儿乐 幼儿配方奶粉3段（1-3岁）1200g/盒"
                                    href="product.html">Wyeth/惠氏 进口奶源 S-26金装幼儿乐 幼儿配方奶粉3段（1-3岁）1200g/盒</a> <strong class="adText">
                                        Wyeth/惠氏 进口奶源 S-26金装幼儿乐 幼儿配方奶粉</strong>
                                <p class="price">
                                    <b>¥1500</b><span>¥500</span></p>
                                <p>
                                    <a href="#">评论:1000条</a></p>
                            </span><span class="item"><a href="#">
                                <img src="../static/images/sp02.jpg"></a> <a class="name" title="Wyeth/惠氏 进口奶源 S-26金装幼儿乐 幼儿配方奶粉3段（1-3岁）1200g/盒"
                                    href="product.html">Wyeth/惠氏 进口奶源 S-26金装幼儿乐 幼儿配方奶粉3段（1-3岁）1200g/盒</a> <strong class="adText">
                                        Wyeth/惠氏 进口奶源 S-26金装幼儿乐 幼儿配方奶粉</strong>
                                <p class="price">
                                    <b>¥1500</b><span>¥500</span></p>
                                <p>
                                    <a href="#">评论:1000条</a></p>
                            </span><span class="item"><a href="#">
                                <img src="../static/images/sp02.jpg"></a> <a class="name" title="Wyeth/惠氏 进口奶源 S-26金装幼儿乐 幼儿配方奶粉3段（1-3岁）1200g/盒"
                                    href="product.html">Wyeth/惠氏 进口奶源 S-26金装幼儿乐 幼儿配方奶粉3段（1-3岁）1200g/盒</a> <strong class="adText">
                                        Wyeth/惠氏 进口奶源 S-26金装幼儿乐 幼儿配方奶粉</strong>
                                <p class="price">
                                    <b>¥1500</b><span>¥500</span></p>
                                <p>
                                    <a href="#">评论:1000条</a></p>
                            </span><span class="item"><a href="#">
                                <img src="../static/images/sp02.jpg"></a> <a class="name" title="Wyeth/惠氏 进口奶源 S-26金装幼儿乐 幼儿配方奶粉3段（1-3岁）1200g/盒"
                                    href="product.html">Wyeth/惠氏 进口奶源 S-26金装幼儿乐 幼儿配方奶粉3段（1-3岁）1200g/盒</a> <strong class="adText">
                                        Wyeth/惠氏 进口奶源 S-26金装幼儿乐 幼儿配方奶粉</strong>
                                <p class="price">
                                    <b>¥1500</b><span>¥500</span></p>
                                <p>
                                    <a href="#">评论:1000条</a></p>
                            </span></li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BackupContent1" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BackupContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
</asp:Content>
