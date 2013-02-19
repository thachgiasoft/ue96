<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchResult2.aspx.cs"
    Inherits="YoeJoyWeb.SearchResult2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link type="text/css" rel="Stylesheet" href="../static/css/base.css" />
    <link type="text/css" rel="Stylesheet" href="../static/css/layout.css" />
    <link type="text/css" rel="Stylesheet" href="../static/css/class.css" />
    <script type="text/javascript" src="../static/js/dev/YoeJoy.Namespace.js"></script>
    <script type="text/javascript" src="../static/js/dev/jquery-1.9.0.js"></script>
    <script type="text/javascript" src="../static/js/dev/jquery.js"></script>
    <script type="text/javascript" src="../static/js/dev/usercustom.js"></script>
    <script type="text/javascript" src="../static/js/dev/Yoejoy.Site.js"></script>
</head>
<body>
    <div id="showList">
        <!--排序条件Begin-->
        <div class="sort">
            <div class="item0">
                <div>
                    <span>排序：</span>
                    <select id="orderSelect">
                        <option value="1">销量从高到低</option>
                        <option value="2">销量从低到高</option>
                        <option value="3">价格从高到低</option>
                        <option value="4">价格从低到高</option>
                        <option value="5">评论从高到低</option>
                        <option value="6">评论从低到高</option>
                        <option selected="selected" value="0">默认排序</option>
                    </select>
                </div>
                <ul id="orderBy">
                    <li><a href="javascript:void(0)">销量</a><input type="hidden" value="3" /></li>
                    <li><a href="javascript:void(0)">价格</a><input type="hidden" value="2" /></li>
                    <li><a href="javascript:void(0)">评论</a><input type="hidden" value="5" /></li>
                    <li><a href="javascript:void(0)">上架时间</a><input type="hidden" value="4" /></li>
                </ul>
            </div>
            <div class="item1">
            </div>
        </div>
        <!--排序条件End-->
        <!--显示商品列表Begin-->
        <div id="productList" class="list">
        </div>
    </div>
    <!--商品列表页码-->
    <%=C3ProductListFooterHTML%>
    <script type="text/javascript">

        var c1 = YoeJoy.Site.Utility.GetQueryString("c1");
        var c2 = YoeJoy.Site.Utility.GetQueryString("c2");
        var c3 = YoeJoy.Site.Utility.GetQueryString("c3");
        var attrIds = YoeJoy.Site.Utility.GetQueryString("attrIds");
        //分页起始标识
        var startIndex = 1;
        //总页数
        var totalPageCount = parseInt($("#totalPageCount").val());
        //当前页码标识
        var currentPageIndex = 1;
        var handlerBaseURL = "../Service/GetSearch2ProductListItem.aspx";
        var order = "DESC";
        var orderOption = 1;
        var pageSeed = parseInt($("#pageSeed").val());
        var keyWords = YoeJoy.Site.Utility.GetQueryString("q");
        var totalProductCount = parseInt($("#totalProductCount").val());
        var IsAscOrder = false;

        function getProductListItem(callbackHandler) {
            //Use random number in query string to avoid the Ajax get handler browser cache
            var handlerURL = handlerBaseURL + "?c1=" + c1 + "&c2=" + c2 + "&c3=" + c3 + "&startIndex=" + currentPageIndex + "&orderBy=" + orderOption + "&attrIds=" + attrIds + "&order=" + order + "&q=" + escape(keyWords) + "&random=" + Math.random();
            //var handlerURL = handerBaseURL + "?c1=" + c1 + "&c2=" + c2 + "&c3=" + c3 + "&startIndex=" + currentPageIndex + "&orderBy=" + orderOption + "&attrIds=" + attributionIds;
            $.get(handlerURL, function (data) {
                $("#productList").empty().append(data);
                callbackHandler();
            });
        };


        function locateToPage(pageNum) {
            var stepSeed = Math.abs((pageNum - currentPageIndex));
            if (pageNum > currentPageIndex) {
                currentPageIndex = currentPageIndex + stepSeed
                startIndex = startIndex + stepSeed * pageSeed;
            }
            else {
                currentPageIndex = currentPageIndex - stepSeed
                startIndex = startIndex - stepSeed * pageSeed;
            }
            getProductListItem(function () {
                pageNum--;
                $("#pageNumNav").children("a").removeClass("current");
                $("#pageNumNav").children("a").eq(pageNum).addClass("current");
            });
        };

        function changeOrderByList(orderTag) {
            orderTag = parseInt(orderTag);
            startIndex = 1;
            currentPageIndex = 1;
            $("#orderBy").children("li").removeClass("selected");
            if (orderTag == 0) {
                order = "DESC";
                orderOption = 1;
            }
            else if (orderTag == 1) {
                order = "DESC";
                orderOption = 3;
                $("#orderBy").children("li").eq(0).addClass("selected");
            }
            else if (orderTag == 2) {
                order = "ASC";
                orderOption = 3;
                $("#orderBy").children("li").eq(0).addClass("selected");
            }
            else if (orderTag == 3) {
                order = "DESC";
                orderOption = 2;
                $("#orderBy").children("li").eq(1).addClass("selected").css('background-position', '39px -21px');
            }
            else if (orderTag == 4) {
                order = "ASC";
                orderOption = 2;
                $("#orderBy").children("li").eq(1).addClass("selected").css('background-position', '39px -44px');
            }
            getProductListItem(function () {
                $("#pageNumNav").children("a").eq(0).addClass("current");
            });
        };

        function changeOrderByTab(orderTag) {
            switch (orderTag) {
                case "3":
                    {
                        if (IsAscOrder) {
                            changeOrderByList(2);
                            $('#orderSelect option').filter(function () {
                                return $(this).val() == 2
                            }).attr('selected', 'true').siblings().removeAttr('selected');
                        }
                        else {
                            changeOrderByList(1);
                            $('#orderSelect option').filter(function () {
                                return $(this).val() == 1
                            }).attr('selected', 'true').siblings().removeAttr('selected');
                        }
                        return;
                    }
                case "2":
                    {
                        if (IsAscOrder) {
                            changeOrderByList(4);
                            $('#orderSelect option').filter(function () {
                                return $(this).val() == 4
                            }).attr('selected', 'true').siblings().removeAttr('selected');
                        }
                        else {
                            changeOrderByList(3);
                            $('#orderSelect option').filter(function () {
                                return $(this).val() == 3
                            }).attr('selected', 'true').siblings().removeAttr('selected');
                        }
                        return;
                    }
                case "4":
                    {
                        order = "DESC";
                        orderOption = 4;
                        getProductListItem(function () {
                            $("#pageNumNav").children("a").eq(0).addClass("current");
                        });
                    }
                default:
                    {
                        return;
                    }
            }
        };

        $(function () {

            var _$parentWindow = $(window.parent.document);
            _$parentWindow.find("#resultNum").text("找到" + totalProductCount + "件相关商品")

            getProductListItem(function () {
                $("#pageNumNav").children("a").eq(0).addClass("current");
            });

            $("#prev").click(function () {
                if (currentPageIndex > 1) {
                    locateToPage(currentPageIndex - 1);
                }
            });

            $("#next").click(function () {
                if (currentPageIndex < totalPageCount) {
                    locateToPage(currentPageIndex + 1);
                }
            });

            $("#pageNumNav").children("a").each(function (index) {
                $(this).click(function (event) {
                    locateToPage(index + 1);
                });
            });

            $("#btnLocate").click(function () {
                var pageNumInput = parseInt($("#txtIndex").val());
                if (pageNumInput == '') {
                    return;
                }
                else {
                    if (pageNumInput > totalPageCount || pageNumInput <= 0) {
                        alert("输入值越界");
                    }
                    else if (pageNumInput == currentPageIndex) {
                        return;
                    }
                    else {
                        locateToPage(pageNumInput);
                    }
                }
            });

            $("#orderSelect").change(function (event) {
                var $option = $(this);
                var optionValue = $option.val();
                changeOrderByList(optionValue);
            });

            $("#orderBy li").each(function (index) {
                $(this).click(function (event) {
                    $(this).siblings().removeClass("selected");
                    $(this).addClass('selected');
                    var orderOption = $(this).children("input").val();
                    if ($(this).hasClass("asc")) {
                        $(this).css('background-position', '39px -44px');
                        $(this).removeClass("asc");
                        IsAscOrder = false;
                    }
                    else {
                        $(this).css('background-position', '39px -21px');
                        $(this).addClass("asc");
                        IsAscOrder = true;
                    }
                    changeOrderByTab(orderOption);
                });
            });

        });

    </script>
</body>
</html>
