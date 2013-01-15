<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Site.Master" AutoEventWireup="true"
    CodeBehind="SubProductList2_Test.aspx.cs" Inherits="YoeJoyWeb.SubProductList2_Test" %>

<%@ Register Src="../Controls/CategoryNavigation.ascx" TagName="CategoryNavigation"
    TagPrefix="uc1" %>
<%@ Register Src="../Controls/SubCategoryNavigation.ascx" TagName="SubCategoryNavigation"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .enableArrow
        {
            display: inline-block;
            cursor: pointer;
            background-image: url('../static/images/hy2.gif');
            background-repeat: no-repeat;
            width: 18px;
            height: 18px;
            margin: 0px 3px;
        }
        .disableArrow
        {
            display: inline-block;
            background-image: url('../static/images/qy1.gif');
            background-repeat: no-repeat;
            width: 18px;
            height: 18px;
            margin: 0px 3px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigationModuleContent" runat="server">
    <uc1:CategoryNavigation ID="CategoryNavigation1" runat="server" />
    <uc2:SubCategoryNavigation ID="SubCategoryNavigation1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PanicBuyingMdouleContent" runat="server">
    <div id="cprp" class="l_class">
        <img src="../static/images/cprp.png">
        <div class="item">
            <table border="0" cellspacing="0" cellpadding="0" width="180">
                <tbody>
                    <tr>
                        <td width="62">
                            <img src="../static/images/plsp.jpg">
                        </td>
                        <td valign="top" width="118" align="right">
                            <p align="left">
                                <a href="#">商品名称</a></p>
                            <a href="#"><span class="plnum">321</span></a>条
                        </td>
                    </tr>
                </tbody>
            </table>
            <p class="pltext" align="left">
                评论内容评论内容评论内容评论内容评论内容评论内容</p>
            会员:<span class="plname">l***o</span>
        </div>
        <div class="item">
            <table border="0" cellspacing="0" cellpadding="0" width="180">
                <tbody>
                    <tr>
                        <td width="62">
                            <img src="../static/images/plsp.jpg">
                        </td>
                        <td valign="top" width="118" align="right">
                            <p align="left">
                                <a href="#">商品名称</a></p>
                            <a href="#"><span class="plnum">321</span></a>条
                        </td>
                    </tr>
                </tbody>
            </table>
            <p class="pltext" align="left">
                评论内容评论内容评论内容评论内容评论内容评论内容</p>
            会员:<span class="plname">l***o</span>
        </div>
        <div class="item">
            <table border="0" cellspacing="0" cellpadding="0" width="180">
                <tbody>
                    <tr>
                        <td width="62">
                            <img src="../static/images/plsp.jpg">
                        </td>
                        <td valign="top" width="118" align="right">
                            <p align="left">
                                <a href="#">商品名称</a></p>
                            <a href="#"><span class="plnum">321</span></a>条
                        </td>
                    </tr>
                </tbody>
            </table>
            <p class="pltext" align="left">
                评论内容评论内容评论内容评论内容评论内容评论内容</p>
            会员:<span class="plname">l***o</span>
        </div>
        <div class="item">
            <table border="0" cellspacing="0" cellpadding="0" width="180">
                <tbody>
                    <tr>
                        <td width="62">
                            <img src="../static/images/plsp.jpg">
                        </td>
                        <td valign="top" width="118" align="right">
                            <p align="left">
                                <a href="#">商品名称</a></p>
                            <a href="#"><span class="plnum">321</span></a>条
                        </td>
                    </tr>
                </tbody>
            </table>
            <p class="pltext" align="left">
                评论内容评论内容评论内容评论内容评论内容评论内容</p>
            会员:<span class="plname">l***o</span>
        </div>
        <div class="item">
            <table border="0" cellspacing="0" cellpadding="0" width="180">
                <tbody>
                    <tr>
                        <td width="62">
                            <img src="../static/images/plsp.jpg">
                        </td>
                        <td valign="top" width="118" align="right">
                            <p align="left">
                                <a href="#">商品名称</a></p>
                            <a href="#"><span class="plnum">321</span></a>条
                        </td>
                    </tr>
                </tbody>
            </table>
            <p class="pltext" align="left">
                评论内容评论内容评论内容评论内容评论内容评论内容</p>
            会员:<span class="plname">l***o</span>
        </div>
        <img src="../static/images/yj02.png">
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TopLeftModuleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="TopRightMdouleContent" runat="server">
    <!--洋葱导航条-->
    <div class="ycdh">
        <a href="../Default.aspx"><b>首页</b></a>&gt;<a><span></span></a>&gt;<span></span></div>
    <!--热卖推荐-->
    <table class="rmtj" border="0" cellspacing="0" cellpadding="0" width="1000">
        <tbody>
            <tr>
                <td valign="top" width="38">
                    <img src="../static/images/rmtj.png">
                </td>
                <td class="list">
                    <ul>
                        <li><a href="#">
                            <p>
                                <img src="../static/images/sp02.jpg"><span class="price">1000元</span></p>
                            <span>商品名称</span></a></li>
                        <li><a href="#">
                            <p>
                                <img src="../static/images/sp02.jpg"><span class="price">1000元</span></p>
                            <span>商品名称</span></a></li>
                        <li><a href="#">
                            <p>
                                <img src="../static/images/sp02.jpg"><span class="price">1000元</span></p>
                            <span>商品名称</span></a></li>
                        <li><a href="#">
                            <p>
                                <img src="../static/images/sp02.jpg"><span class="price">1000元</span></p>
                            <span>商品名称</span></a></li>
                    </ul>
                </td>
            </tr>
        </tbody>
    </table>
    <!--筛选条件-->
    <div class="sxtj">
        <table border="0" cellspacing="0" cellpadding="0" width="792">
            <%=C3ProductFilterHTML%>
        </table>
    </div>
    <!--商品列-->
    <div class="splb">
        <!--排序条件-->
        <div class="pxtjitem">
            <ul id="listHeader">
                <li class="selected">默认排序<input type="hidden" value="1" /></li>
                <li>价格<input type="hidden" value="2" /></li>
                <li>销量<input type="hidden" value="3" /></li>
                <li>上架时间<input type="hidden" value="4" /></li>
                <li>评价<input type="hidden" value="5" /></li>
            </ul>
            <%=C3ProductListHeaderHTML%>
        </div>
        <div id="productList" class="list">
        </div>
    </div>
    <div class="fyitem1" align="right">
        <form id="Form1">
        <span class="prev prev0">上一页</span><span class="pagenum selected">1</span><a href="#"><span
            class="pagenum">2</span></a><a href="#"><span class="pagenum">3</span></a><a href="#"><span
                class="pagenum">4</span></a><span>...</span><a href="#"><span class="pagenum">8</span></a><a
                    href="#"><span class="next next1">下一页</span></a><span>共8页</span><span>到第</span>
        <input maxlength="2" width="2" type="text">
        <span>页</span>
        <input value="确定" type="button">
        </form>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="BodyContent" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ExtenalContent" runat="server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="ScriptContent" runat="server">
    <script type="text/javascript">

        function getQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) {
                return unescape(r[2]);
            }
            return null;
        }

        function getAttributionIds() {
            var productFilterCount = $("#filterTable").children("tr").length;
            var selectedAIds = new Array();
            for (var i = 0; i < productFilterCount; i++) {
                var attrId = $("#filterTable tr").eq(i).children("td").eq(1).children("ul").children("input").val();
                if (attrId != "0") {
                    selectedAIds[i] = attrId;
                }
            }
            return selectedAIds;
        }

        function getProductListItem(handerBaseURL, c1, c2, c3, totalPagedCount, callbackHandler) {
            var currentPageIndex = parseInt($("#currentPageIndex").val());
            var orderOption = $("#listHeader .selected").children("input").val();
            var handlerURL = handerBaseURL + "?c1=" + c1 + "&c2=" + c2 + "&c3=" + c3 + "&startIndex=" + currentPageIndex + "&pagedCount=" + totalPagedCount + "&orderBy=" + orderOption + "&attrIds=" + getAttributionIds() + "&random=" + Math.random();
            $.get(handlerURL, function (data) {
                $("#productList").empty().append(data);
                callbackHandler();
            });
        }

        $(function () {

            $("body").removeAttr("id").attr({ "id": "class" });
            var c1 = getQueryString("c1");
            var c2 = getQueryString("c2");
            var c3 = getQueryString("c3");
            var c1Page = "SubProductList1.aspx?c1=";
            var $c1Name = $(".flbt").html();
            var c3Name = $("#subCategoryMenu li input[value=" + c3 + "]").siblings("input").val();
            $(".ycdh a").eq(1).attr({ "href": c1Page + c1 }).children("span").html($c1Name);
            $(".ycdh span").eq(1).html(c3Name);

            var GetProductListItemSeriviceBaseURL = "../Service/GetProductListItem.aspx";
            var totalPagedCount = parseInt($("#totalPagedCount").val());

            getProductListItem(GetProductListItemSeriviceBaseURL, c1, c2, c3, totalPagedCount, function () {
                var pagedSeed = parseInt($("#pagedSeed").val());
                if (pagedSeed == totalPagedCount) {
                    $("#nextArrow").removeClass("enableArrow").addClass("disableArrow");
                }
            });

            $("#prevArrow").click(function () {
                if ($(this).hasClass("disableArrow")) {
                    return;
                }
                else {
                    var currentPageIndex = parseInt($("#currentPageIndex").val());
                    $("#currentPageIndex").val(currentPageIndex - totalPagedCount);
                    getProductListItem(GetProductListItemSeriviceBaseURL, c1, c2, c3, totalPagedCount, function () {
                        var pagedSeed = parseInt($("#pagedSeed").val());
                        $("#pagedSeed").val(pagedSeed - 1);
                        pagedSeed -= 1;
                        if (pagedSeed > 1) {
                            $("#prevArrow").removeClass().addClass("enableArrow");
                        }
                        else {
                            $("#prevArrow").removeClass().addClass("disableArrow");
                            $("#nextArrow").removeClass().addClass("enableArrow");
                        }
                    });
                }
            });

            $("#nextArrow").click(function () {
                if ($(this).hasClass("disableArrow")) {
                    return;
                }
                else {
                    var currentPageIndex = parseInt($("#currentPageIndex").val());
                    $("#currentPageIndex").val(currentPageIndex + totalPagedCount);
                    getProductListItem(GetProductListItemSeriviceBaseURL, c1, c2, c3, totalPagedCount, function () {
                        var pagedSeed = parseInt($("#pagedSeed").val());
                        $("#pagedSeed").val(pagedSeed + 1);
                        pagedSeed += 1;
                        if (pagedSeed < totalPagedCount) {
                            $("#nextArrow").removeClass().addClass("enableArrow");
                        }
                        else {
                            $("#nextArrow").removeClass().addClass("disableArrow");
                            $("#prevArrow").removeClass().addClass("enableArrow");
                        }
                    });
                }
            });

            $("#listHeader li").each(function (index) {
                $(this).click(function () {
                    $(this).siblings("li").removeClass("selected");
                    $(this).addClass("selected");
                    $("#currentPageIndex").val('1');

                    getProductListItem(GetProductListItemSeriviceBaseURL, c1, c2, c3, totalPagedCount, function () {

                        $("#pagedSeed").val('1');
                        if (totalPagedCount == 1) {
                            $("#prevArrow").removeClass().addClass("disableArrow");
                            $("#nextArrow").removeClass().addClass("disableArrow");
                        }
                        else {
                            $("#prevArrow").removeClass().addClass("disableArrow");
                            $("#nextArrow").removeClass().addClass("enableArrow");
                        }
                    });
                });
            });


            $("#filterTable tr").each(function (index) {
                $(this).children("td").eq(1).children("ul").children("li").each(function (index1) {
                    $(this).click(function () {
                        var a2id = $(this).children("input").val();
                        $(this).siblings("input").val(a2id);
                        //alert(getAttributionIds());
                        $("#currentPageIndex").val('1');
                        getProductListItem(GetProductListItemSeriviceBaseURL, c1, c2, c3, totalPagedCount, function () {

                            $("#pagedSeed").val('1');
                            if (totalPagedCount == 1) {
                                $("#prevArrow").removeClass().addClass("disableArrow");
                                $("#nextArrow").removeClass().addClass("disableArrow");
                            }
                            else {
                                $("#prevArrow").removeClass().addClass("disableArrow");
                                $("#nextArrow").removeClass().addClass("enableArrow");
                            }
                        });

                    });
                });
            });

        });
    </script>
</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="PopupContent" runat="server">
</asp:Content>
