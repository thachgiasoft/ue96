<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Payment.Master" AutoEventWireup="true"
    CodeBehind="ShoppingCart.aspx.cs" Inherits="YoeJoyWeb.Shopping.ShoppingCart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link type="text/css" rel="Stylesheet" href="../static/css/base.css" />
    <link type="text/css" rel="Stylesheet" href="../static/css/process1.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
    <div id="content">
        <div id="process">
            <img src="../static/images/step1.png" />
        </div>
        <div id="FreeDesc">
            <h2>
                <input checked="checked" type="checkbox" />
                <span>商品每满<em>100元</em>免<em>10kg</em></span> <span>商品每满<em>100元</em>免<em>10kg</em></span>
                <a href="#">运费说明</a>
            </h2>
            <asp:DataGrid ID="dgCart" runat="server" AutoGenerateColumns="False" DataKeyField="SysNo"
                Width="100%" OnItemDataBound="dgCart_ItemDataBound1">
                <Columns>
                    <asp:BoundColumn DataField="sysno" Visible="False"></asp:BoundColumn>
                    <asp:BoundColumn DataField="quantity" Visible="False"></asp:BoundColumn>
                    <asp:BoundColumn DataField="productlink" HeaderText="商品名称">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Width="190px" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="currentpriceshow" HeaderText="攸怡价">
                        <HeaderStyle HorizontalAlign="Center" Width="80px" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="PointStatus" HeaderText="积分兑换" Visible="False">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle ForeColor="Green" HorizontalAlign="Center" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="point" HeaderText="赠送积分">
                        <HeaderStyle HorizontalAlign="Center" Width="55px" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundColumn>
                    <asp:TemplateColumn HeaderText="限购数量">
                        <ItemTemplate>
                            <asp:Label ID="lblLimitedQty" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"LimitedQty") %>'></asp:Label></ItemTemplate>
                        <HeaderStyle Width="55px" />
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="购买数量">
                        <HeaderStyle HorizontalAlign="Center" Width="80px" />
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <div style="padding: 5px; width: 70px;">
                                <asp:TextBox ID="txtQuantity" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Quantity")%>'
                                    ToolTip="当您选购产品大于1时，请在此修改购买数量" Width="20px"></asp:TextBox>
                                <asp:LinkButton ToolTip="当您选购产品大于1时，请先填入具体数量，再点击＂更新数量＂按钮进行更新" ID="cmdUpdate" runat="server"
                                    CommandName="cmdUpdate" CssClass="button02" OnClick="cmdUpdate_Click"><span>修改数量</span></asp:LinkButton>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="期望购买数量">
                        <HeaderStyle HorizontalAlign="Center" Width="80px" />
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:TextBox ID="txtExpectQty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ExpectQty")%>'
                                ToolTip="当您期望购买数量大于限购数量时，请在此填写期望购买数量，我们的客服人员将与您联系" Width="20px"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:BoundColumn DataField="linetotal" HeaderText="小计">
                        <HeaderStyle HorizontalAlign="Center" Width="60px" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="DeliveryTimeliness" HeaderText="发货时间">
                        <HeaderStyle HorizontalAlign="Center" Width="55px" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Operation" HeaderText="去除商品">
                        <HeaderStyle HorizontalAlign="Center" Width="55px" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundColumn>
                </Columns>
                <FooterStyle CssClass="GridShoppingFooterStyle" />
                <SelectedItemStyle CssClass="GridShoppingSelectedRowStyle" />
                <ItemStyle CssClass="GridShoppingRowStyle" VerticalAlign="Middle" />
                <PagerStyle CssClass="GridShoppingPagerStyle" />
                <AlternatingItemStyle CssClass="GridShoppingAlternatingRowStyle" VerticalAlign="Middle" />
                <HeaderStyle CssClass="GridShoppingHeaderStyle" />
            </asp:DataGrid>
            <asp:DataGrid ID="dgSaleRule" runat="server" AutoGenerateColumns="False" Width="60%">
                <ItemStyle Height="22"></ItemStyle>
                <HeaderStyle Height="22" CssClass="DataGridHead"></HeaderStyle>
                <Columns>
                    <asp:BoundColumn DataField="SaleRuleName" HeaderText="优惠套装"></asp:BoundColumn>
                    <asp:BoundColumn DataField="DiscountAmt" HeaderText="节省金额" ItemStyle-HorizontalAlign="Right">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="DiscountTime" HeaderText="使用次数" ItemStyle-HorizontalAlign="Right">
                    </asp:BoundColumn>
                    <asp:BoundColumn HeaderText="金额小计" DataField="SubTotal" ItemStyle-HorizontalAlign="Right"
                        ItemStyle-Width="100px"></asp:BoundColumn>
                </Columns>
            </asp:DataGrid>
            <asp:DataGrid ID="dgGiftCart" runat="server" AutoGenerateColumns="False" CssClass="DataGridBorder"
                DataKeyField="SysNo" Width="100%" BackColor="White" BorderColor="#d7d7d7" BorderWidth="1px"
                CellPadding="3">
                <Columns>
                    <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center" HeaderText="选择" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="30" Visible="false">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="True" />
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:BoundColumn DataField="GiftName" HeaderStyle-HorizontalAlign="Center" HeaderText="赠品名称">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="MasterName" HeaderStyle-HorizontalAlign="Center" HeaderText="商品名称">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Quantity" HeaderStyle-HorizontalAlign="Center" HeaderText="赠送数量"
                        ItemStyle-Width="60" ItemStyle-HorizontalAlign="Right"></asp:BoundColumn>
                    <asp:BoundColumn DataField="Weight" HeaderStyle-HorizontalAlign="Center" HeaderText="单品重量"
                        ItemStyle-Width="60" ItemStyle-HorizontalAlign="Right"></asp:BoundColumn>
                </Columns>
                <FooterStyle CssClass="GridSFooterStyle" />
                <PagerStyle CssClass="GridSPagerStyle" />
                <HeaderStyle CssClass="GridVHeaderStyle" />
            </asp:DataGrid>
            <asp:Panel ID="panelMessage" runat="server" Width="99.75%" Height="150" BorderColor="#d7d7d7"
                BorderWidth="1">
                <div align="center">
                    &nbsp;</div>
                <div align="center">
                    &nbsp;</div>
                <div align="center">
                    &nbsp;</div>
                <div align="center">
                    <font size="4"></font>&nbsp;</div>
                <div align="center">
                    <font color="red" size="4"><strong>购物车好空啊，您不打算买点什么吗？</strong></font></div>
            </asp:Panel>
            <div id="c_gwc_btn" runat="server" style="text-align: left; padding-top: 10px; padding-left: 1px;">
                <div>
                    <asp:Label ID="lblExpectQty" runat="server" EnableViewState="False" Font-Bold="True"
                        ForeColor="Red" Visible="False">您的期望购买数量超过了限购数量，系统会纪录您的期望购买数量，UE96 客服人员将与您联系！</asp:Label></div>
                <div>
                    <asp:Label ID="lblIsSameChannel" runat="server" EnableViewState="False" Font-Bold="True"
                        ForeColor="Red" Visible="False">您购买商品中含有欧洲直发商品与国内商品，为了不影响你的收货时间，请分开下单！</asp:Label></div>
                <div>
                    <asp:Label ID="lblMessage" runat="server" EnableViewState="False" Font-Bold="True"
                        ForeColor="Red" /></div>
                <div>
                    <asp:Label ID="lblPoint" runat="server">积分支付额度：</asp:Label>
                    <asp:Label ID="lblPointAmt" runat="server" ForeColor="#FF6633" Width="40"></asp:Label></div>
            </div>
            <div id="comfirm">
                <div class="l">
                    <input checked="checked" type="checkbox">
                    全选 <a href="#">批量删除</a> <a href="#">
                        <asp:ImageButton ID="imgEmpty" runat="server" ImageUrl="~/static/images/emptycart.JPG"
                            OnClick="imgEmpty_Click" /></a>
                </div>
                <div class="r">
                    <b>商品总价:</b> <strong>
                        <asp:Label ID="lblAmtTxt" runat="server">￥</asp:Label><asp:Label ID="lblAmt" runat="server"></asp:Label></strong>
                    <b>元</b> <a href="javascript:void(0);">
                        <asp:ImageButton ID="imgCheckOut" runat="server" ImageUrl="~/static/images/comfirm.jpg"
                            OnClick="imgCheckOut_Click" />
                    </a>
                </div>
                <asp:ImageButton ID="imgContinue" runat="server" Style="padding-right: 10px;" ImageUrl="../images/shopping/contineshopping.gif"
                    OnClick="imgContinue_Click" Visible="false" />
            </div>
            <asp:TextBox ID="txtIsCheck" runat="server" Enabled="False" Visible="False" Width="16px">0</asp:TextBox>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ModuleContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BackupContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="BackupContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
</asp:Content>
