<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Site.Master" AutoEventWireup="true"
    CodeBehind="BrandProductList1.aspx.cs" Inherits="YoeJoyWeb.Pages.BrandProductList1" %>

<%@ Register Src="../Controls/CategoryNavigation.ascx" TagName="CategoryNavigation"
    TagPrefix="uc1" %>
<%@ Register Src="../Controls/SubCategoryNavigation.ascx" TagName="SubCategoryNavigation"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link type="text/css" rel="Stylesheet" href="../static/css/layout.css" />
    <link type="text/css" rel="Stylesheet" href="../static/css/class.css" />
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
            您在：<a href="../Default.aspx">首页</a>〉品牌：<%=BrandName%>
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="LeftBigModule" runat="server">
    <div class="mix">
        <!--左边区域-->
        <div class="l_module">
            <!--三级商品分类-->
            <uc2:SubCategoryNavigation ID="SubCategoryNavigation1" runat="server" />
            <!--产品热评Begin-->
            <div id="hotComments" class="l_class area">
                <div class="title">
                    <div class="mem0">
                    </div>
                    <h3>
                        产品热评</h3>
                    <div class="mem1">
                    </div>
                </div>
                <%=SearchHotCommentProductsHTML %>
            </div>
            <!--产品热评End-->
        </div>
        <!--右内容区Begin-->
        <div class="r_module">
            <!--筛选条件Begin-->
            <div id="screening">
                <div class="title">
                    "<%=BrandName %>"<span id="resultNum"></span></div>
                <%=Search1C3Filter %>
                <div class="more">
                    <div class="item0">
                    </div>
                    <div class="item1">
                        <a href="javascript:void(0)">更多筛选</a></div>
                </div>
            </div>
            <iframe id="ProductIframe" frameborder="0" scrolling="no" width="982px" height="1150px">
            </iframe>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="RightBigModule" runat="server">
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

            var bid = YoeJoy.Site.Utility.GetQueryString("bid");
            var bName = YoeJoy.Site.Utility.GetQueryString("bName");
            var serachDetailBaseURL = "BrandProductList2.aspx?c1=";
            var productIframeURL = "BrandProductsResult1.aspx?bid=" + bid;
            $("#ProductIframe").attr({ "src": productIframeURL });

            $(".attr strong span").each(function (index) {
                $(this).click(function () {
                    var c1 = $(this).children("input").eq(0).val();
                    var c2 = $(this).children("input").eq(1).val();
                    var c3 = $(this).children("input").eq(2).val();
                    var searchDetailURL = serachDetailBaseURL + c1 + "&c2=" + c2 + "&c3=" + c3 + "&bid=" + bid + "&bName=" + escape(bName);
                    window.location.href = searchDetailURL;
                });
            });

        });
    </script>
</asp:Content>
