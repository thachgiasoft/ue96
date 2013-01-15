<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Site.Master" AutoEventWireup="true"
    CodeBehind="Article.aspx.cs" Inherits="YoeJoyWeb.Pages.Article" %>

<%@ Register Src="../Controls/CategoryNavigation.ascx" TagName="CategoryNavigation"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<link type="text/css" rel="Stylesheet" href="../static/css/news.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="LeftTopModule" runat="server">
    <uc1:CategoryNavigation ID="CategoryNavigation1" IsHomePage="false" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="RightTopModule" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MiddleTopModule" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="SiteNavModule" runat="server">
    <div id="position">
        <span>
            <img src="../static/images/f4.jpg">您在:</span><a href="../Default.aspx">首页</a>
        <span>&gt;</span><span>新闻中心</span>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="LeftBigModule" runat="server">
    <div class="left">
        <dl id="newsMenu">
            <dt>攸怡导购</dt>
            <dd>
                <a href="news09.html">绵羊油并不是羊的脂肪</a></dd>
            <dd>
                <a href="news10.html">美容达人必备护肤品</a></dd>
            <dd>
                <a href="news11.html">把脸洗干净了再谈护肤</a></dd>
            <dd>
                <a href="news12.html">重回固体时代</a></dd>
            <dt>攸怡动态</dt>
            <dd>
                <a href="news01.html">中国人什么时候会上网买菜？</a></dd>
            <dd>
                <a href="news02.html">手机购物成新奇消费模式：网购消费支出激增</a></dd>
            <dd>
                <a href="news03.html">工信部官员：中国网民数已达5.5亿</a></dd>
            <dd>
                <a href="news04.html">丽思·卡尔顿：是如何创造出忠诚顾客人均120万美元的终身消费的？</a></dd>
            <dd>
                <a href="news05.html">易观：第3季中国移动互联网市场规模达395亿</a></dd>
            <dt>攸怡公告</dt>
            <dd>
                <a href="news06.html">关于攸怡网商品结构调整的公告</a></dd>
            <dd>
                <a href="news07.html">警惕假冒攸怡网销售通告</a></dd>
            <dd>
                <a href="news08.html">关于增值税发票的公告</a></dd>
        </dl>
    </div>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="RightBigModule" runat="server">
    <div class="bigRight">
        <!-- 帖子标题信息 开始 -->
        <div id="t_title_info">
            <h2 class="t_title">
                中国人什么时候会上网买菜？</h2>
            <p class="t_info">
                <span><a href="http://www.36kr.com/p/200129.html" target="_blank">36氪</a></span>•
                <span>12月13日 10:12</span></p>
        </div>
        <!-- 帖子标题信息 结束 -->
        <!-- 帖子文字内容 开始 -->
        <div id="topic_content">
            <p style="text-indent: 2em;">
                <span>Farmigo 是美国的一家在线农产品销售平台。农场主可以在上面发布自己的农产品，而人们则可以在上面直接购买自己喜欢食材。Farmigo 承诺，新鲜的食材会在
                    48 小时内送至指定地点，而价格也要比超市便宜 20-30%。</span></p>
            <p style="text-indent: 2em;">
                <span>也就是说，Farmigo 自身并不销售农产品，仅起到一个平台中介的作用。他们吸引消费者的优点首先是食材的新鲜，其次是送货上门，最后还拥有价格优势。对于农户而言，Farmigo
                    是一个很好的销售渠道，拿到手的钱也不比出售给超市少，省心又省力。</span></p>
            <p style="text-indent: 2em;">
                <strong><span>农产品电商在我国是否可行呢？</span></strong></p>
            <p style="text-indent: 2em;">
                <span>作为入围 TechCrunch Disrupt 2011 创业者大赛决赛的企业，Farmigo 的目标是成为全美最大的农产品在线交易平台。Farmigo 模式在美国行得通与美国的人文经济环境有关。首先是农贸产品以农场的形式批量生产，其次大型超市与住宅区的距离较远，最后人们也习惯使用互联网进行消费购物。</span></p>
            <p style="text-indent: 2em;">
                <span>我国国情的确与美国存在差异，但这个差距在逐步减小。虽说我国农贸产品的生产方式以小农为主，但市场上活跃着的肉类品牌印证了农产品集团化生产销售的大趋势。阿里巴巴日前宣布，零售交易额突破
                    10000 亿，可见国人对于网购的接纳程度也正与发达国家接轨。</span></p>
            <p style="text-indent: 2em;">
                <span>在线销售农产品的网站在不少城市都出现过，但规模都不大。他们通常自己充当卖方，在价格上没有优势，目标客户是年轻的白领，实质上就是跑腿代购。亲自售卖的模式没有做大的可能性，再加上国人买菜并不麻烦，这类网站的存活实在会有些困难。</span></p>
            <p style="text-indent: 2em;">
                <strong><span>既然我国不具备 Farmigo 模式成功的第二点条件——买菜麻烦，那农产品电商存在的意义何在？</span></strong></p>
            <p style="text-indent: 2em;">
                <span>送菜上门优于驱车买菜的核心其实就是七个字：差异性竞争优势。从这点下手，能够挖掘的内容就不少，比如食品质量。随着经济水平的提高，不少家庭不再满足于一般的农产品，市场对于无化肥蔬果、农家牲畜的需求激增。在市场需求的号召下，有不少绿色农场应运而生，但这些农场目前的经营困境在于，缺乏公信力，缺乏大规模的销售渠道，所以通常难以实现盈利。</span></p>
            <p style="text-indent: 2em;">
                <span>如果市场上出现一个运作良好的“绿色”农产品电商平台，平台与一般本地市场的差异就在于可以保证所售商品的绿色健康，相信即使是 80 岁买菜的老奶奶，也会尝试通过这个平台买一些土鸡蛋给孙子吃。而对于绿色农场方面，借此平台的力量，不仅可以有更广阔的销售渠道，还能更有效地塑造起自己的品牌。正如消费者通过天猫购物，可以减少对商品品质的担忧。</span></p>
            <p style="text-indent: 2em;">
                <strong><span>运送和储存的难度会阻碍农产品电商平台的发展吗？</span></strong></p>
            <p style="text-indent: 2em;">
                <span>农产品具有不可忽视的特性，比一般商品不易保存，这也是为什么 Farmigo 选择了”社区支持型农业（CSA）“的路线，也就是当地农产品的需求由当地供应。运送和储存的难度是否会成为阻碍农产品电商的拦路虎？我认为这仅是一个价值的问题。假设一个北京人希望买到香格里拉新鲜采摘的野生松茸，一个拉萨人希望买到大连刚捕获的野生辽参，距离就不会是问题。</span></p>
            <p style="text-indent: 2em;">
                <span>农产品电商平台的出现需要一个比目前更为成熟的市场环境。曾经有一个说法，十年前的美国就是今天的中国。我相信这么一个成熟的市场环境不会让我们等十年。</span></p>
            <p style="text-align: center;">
                <img title="素菜.jpg" border="0" hspace="0" vspace="0" src="http://www.paidai.com/scripts/plugin/bdeditor/php/../../../../images/news/baidueditor/63931355364400.jpg"></p>
            <p style="text-indent: 2em;">
                <br>
            </p>
        </div>
        <!-- 帖子文字内容 结束 -->
    </div>
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="BackupContent1" runat="server">
</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="HomeMiddleContent" runat="server">
</asp:Content>
<asp:Content ID="Content10" ContentPlaceHolderID="BackupContent2" runat="server">
</asp:Content>
<asp:Content ID="Content11" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>
