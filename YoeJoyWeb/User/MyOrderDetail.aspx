<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Payment.Master" AutoEventWireup="true"
    CodeBehind="MyOrderDetail.aspx.cs" Inherits="YoeJoyWeb.User.MyOrderDetail" %>

<%@ Register TagPrefix="sc1" Namespace="YoeJoyWeb.Controls" Assembly="YoeJoyWeb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link type="text/css" rel="Stylesheet" href="../static/css/base.css" />
    <link type="text/css" rel="Stylesheet" href="../static/css/process1.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
    <div id="content">
        <div id="stutes">
            <div class="l">
                <b>订单号：20120804</b><b>状态：</b> <em>等待付款</em> <a href="#">支付</a>
            </div>
            <div class="r">
                <a class="print" href="#">打印订单</a> <a href="#">修改</a> <a href="#">取消</a>
            </div>
            <p class="tips">
                尊敬的客户，我们还未收到该订单的款项，请您尽快付款（<a href="#">在线支付帮助</a>），如果您已经付款，请务必填写<a href="#">付款确认</a></p>
            <p>
                该订单会为您保留24小时（从下单之日算起），24小时之后如果还未付款，系统将自动取消该订单。</p>
        </div>
        <div class="group1">
            <div>
                <p>
                    <span class="mem0">提交订单</span><br>
                    2012-09-17<br>
                    15:30:30</p>
            </div>
            <div>
                <p>
                    <span class="mem0">等待付款</span></p>
            </div>
            <div>
                <p>
                    <span>商品出库</span></p>
            </div>
            <div>
                <p>
                    <span>等待收货</span></p>
            </div>
            <div>
                <p>
                    <span>完成</span></p>
            </div>
        </div>
        <script language="javascript" type="text/javascript">
            function SelectShipType(obj, type) {
                var items = document.getElementById("ShipType").getElementsByTagName("input");

                for (var i = 0; i < items.length; i++) {
                    var item = items[i];
                    if (item.type == 'radio') {
                        if (item == obj) {
                            item.checked = true;
                        }
                        else
                            item.checked = false;
                    }

                }
            }
        </script>
        <script language="javascript" type="text/javascript">
            function SelectPayType(obj, type) {
                var items = document.getElementById("PayType").getElementsByTagName("input");

                for (var i = 0; i < items.length; i++) {
                    var item = items[i];
                    if (item.type == 'radio') {
                        if (item == obj) {
                            item.checked = true;
                        }
                        else
                            item.checked = false;
                    }

                }

                if (document.all("divTenpayQQ") != null) {
                    if (type == "1") {
                        document.all("divTenpayQQ").style.visibility = "hidden";
                    }
                    else {
                        document.all("divTenpayQQ").style.visibility = "hidden";
                    }
                }
            }
        </script>
        <div id="incontents">
            <div class="shoppingstep">
                <div class="shoppingstep_title">
                    <div class="shoppingstep_title_in">
                        <span class="shoppingstep_current">订单明细</span> <a href="../Account/OrderManagement.html"
                            style="font-size: 12px; color: blue; float: right; position: relative; top: 8px;">
                            回到订单管理中心</a><div class="fixed">
                            </div>
                    </div>
                </div>
                <div class="shoppingstep_content">
                    <table cellpadding="0" cellspacing="0" width="100%" border="1" bordercolor="#b1d0ff"
                        style="border-collapse: collapse; display: none;">
                        <tr>
                            <td>
                                <table cellspacing="0" cellpadding="0" width="98%" align="center" border="0">
                                    <tr height="40">
                                        <td>
                                            <strong><a href="../Account/OrderManagement.aspx" class="STYLE1">回到订单管理中心</a>&nbsp;</strong>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td bgcolor="#ffffff">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <div class="shoppingstep_content_panel">
                        <div class="shoppingstep_content_panel_title">
                            <table id="Table5" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr height="25">
                                    <td>
                                        <strong>SO#:
                                            <asp:Label ID="lblSOID" runat="server"></asp:Label></strong>
                                    </td>
                                    <td>
                                        <b>订购日期：
                                            <asp:Label ID="lblSODate" runat="server"></asp:Label></b>
                                    </td>
                                    <td>
                                        <b>状态：
                                            <asp:Label ID="lblStatus" runat="server"></asp:Label></b>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="shoppingstep_content_panel_content">
                        </div>
                    </div>
                    <div class="shoppingstep_content_panel" id="trSOAlipay" runat="server">
                        <div class="shoppingstep_content_panel_title">
                            <asp:Label ID="Label1" runat="server">支付宝交易号:</asp:Label>
                        </div>
                        <div class="shoppingstep_content_panel_content">
                            <asp:TextBox ID="txtTradeNo" runat="server"></asp:TextBox>&nbsp;<asp:Button ID="btnEnter"
                                runat="server" Text=" 提 交 " OnClick="btnEnter_Click" /><asp:Label ID="lblResult"
                                    runat="server" ForeColor="red"></asp:Label>
                        </div>
                    </div>
                    <asp:Panel ID="PanelShow" runat="server">
                        <div class="shoppingstep_content_panel">
                            <div class="addresspanel">
                                <div>
                                    订货人</div>
                                <ul>
                                    <li>名称：<asp:Label ID="lblName" runat="server"></asp:Label></li>
                                    <li>&nbsp;</li>
                                    <li>省(自治区,直辖市)：<asp:Label ID="lblProvince" runat="server"></asp:Label></li>
                                    <li>城市：<asp:Label ID="lblCity" runat="server"></asp:Label></li>
                                    <li>县/区：<asp:Label ID="lblDistrict" runat="server"></asp:Label></li>
                                    <li>详细地址：<asp:Label ID="lblAddr" runat="server"></asp:Label></li>
                                    <li>邮政编码：<asp:Label ID="lblZip" runat="server"></asp:Label></li>
                                    <li>电话：<asp:Label ID="lblPhone" runat="server"></asp:Label></li>
                                    <li>手机：<asp:Label ID="lblCellPhone" runat="server"></asp:Label></li>
                                </ul>
                            </div>
                            <div class="addresspanel">
                                <div>
                                    收货人</div>
                                <ul>
                                    <li>名称：<asp:Label ID="lblReceiveName" runat="server"></asp:Label></li>
                                    <li>联系人：<asp:Label ID="lblReceiveContact" runat="server"></asp:Label></li>
                                    <li>省(自治区,直辖市)：<asp:Label ID="lblReceiveProvince" runat="server"></asp:Label></li>
                                    <li>城市：<asp:Label ID="lblReceiveCity" runat="server"></asp:Label></li>
                                    <li>县/区：<asp:Label ID="lblReceiveDistrict" runat="server"></asp:Label></li>
                                    <li>详细地址：<asp:Label ID="lblReceiveAddr" runat="server"></asp:Label></li>
                                    <li>邮政编码：<asp:Label ID="lblReceiveZip" runat="server"></asp:Label></li>
                                    <li>电话：<asp:Label ID="lblReceivePhone" runat="server"></asp:Label></li>
                                    <li>手机：<asp:Label ID="lblReceiveCellPhone" runat="server"></asp:Label></li>
                                </ul>
                            </div>
                            <div class="fixed">
                            </div>
                        </div>
                        <div class="shoppingstep_content_panel">
                            <div class="shoppingstep_content_panel_title">
                                其它信息
                            </div>
                            <div class="shoppingstep_content_panel_content">
                                配送方式：<asp:Label ID="lblShipType" runat="server"></asp:Label>
                                付款方式：<asp:Label ID="lblPayType" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="shoppingstep_content_panel">
                            <div class="shoppingstep_content_panel_title">
                                用户留言
                            </div>
                            <div class="shoppingstep_content_panel_content">
                                <asp:Label ID="lblMemo" runat="server"></asp:Label>
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="PanelUpdate" runat="server">
                        <div class="shoppingstep_content_panel">
                            <div class="addressupdatepanel">
                                <a href="../Account/AddressManagement.html" style="font-size: 12px; color: blue;
                                    float: right; position: relative; top: 8px; right: 20px;">我的收货地址</a>
                                <asp:DataList ID="DataList1" runat="server" DataKeyField="SysNo" OnItemDataBound="DataList1_ItemDataBound"
                                    CellPadding="5" Width="840px" OnItemCommand="DataList1_ItemCommand">
                                    <ItemTemplate>
                                        <table width="98%" border="1" cellspacing="0" cellpadding="0" bordercolor="#b1d0ff"
                                            style="border-collapse: collapse">
                                            <tr>
                                                <td rowspan="6" width="30">
                                                    &nbsp;<asp:ImageButton ID="btnSelect" runat="server" CommandName="Select" ImageUrl="../Images/site/UnSelectedIcon.jpg" />
                                                </td>
                                                <td align="left" colspan="3" style="padding-left: 10px">
                                                    <asp:Label ID="lblIsDefault" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "IsDefault")%>'
                                                        Visible="false"></asp:Label>
                                                    <asp:Label ID="lblBrief" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Brief")%>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="80" align="right">
                                                    联系方式：
                                                </td>
                                                <td align="left">
                                                    &nbsp;&nbsp;
                                                    <asp:Label ID="lblName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Name")%>'></asp:Label>,
                                                    <asp:Label ID="lblContact" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Contact")%>'></asp:Label>,
                                                    <asp:Label ID="lblPhone" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Phone")%>'></asp:Label>,
                                                    <asp:Label ID="Label2" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "CellPhone")%>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="80" align="right">
                                                    收货地址：
                                                </td>
                                                <td align="left">
                                                    &nbsp;&nbsp;
                                                    <asp:Label ID="lblAreaName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "AreaName")%>'></asp:Label>
                                                    <asp:Label ID="lblAddress" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Address")%>'></asp:Label>,
                                                    <asp:Label ID="lblZip" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Zip")%>'></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:DataList>
                            </div>
                            <div class="addressupdatepanel">
                                <div>
                                    收货人</div>
                                <ul>
                                    <li>名称：<asp:TextBox ID="txtRname" runat="server"></asp:TextBox></li>
                                    <li>
                                        <sc1:Area ID="RArea" runat="server" AutoPostBack="False">
                                        </sc1:Area>
                                    </li>
                                    <li>详细地址：<asp:TextBox ID="txtRaddress" runat="server" Width="350px"></asp:TextBox></li>
                                    <li>邮政编码：<asp:TextBox ID="txtRzip" runat="server"></asp:TextBox></li>
                                    <li>电话：<asp:TextBox ID="txtRPhone" runat="server"></asp:TextBox></li>
                                    <li>手机：<asp:TextBox ID="txtRCellPhone" runat="server"></asp:TextBox></li>
                                    <li>
                                        <asp:Label ID="lblRErrMsg" runat="server" ForeColor="Red" EnableViewState="False"></asp:Label></li>
                                </ul>
                            </div>
                            <div class="fixed">
                            </div>
                        </div>
                        <div class="shoppingstep_content_panel">
                            <div class="shoppingstep_content_panel_title">
                                其它信息
                            </div>
                            <div class="shoppingstep_content_panel_content">
                                配送方式：<div class="" id="ShipType">
                                    <table border="0" width="100%">
                                        <tr>
                                            <td height="10" align="left">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblFreeShipFeeTip" runat="server" ForeColor="Red" Font-Size="X-Large"></asp:Label>
                                                <asp:DataGrid ID="dgShipType" runat="server" AutoGenerateColumns="False" DataKeyField="SysNo"
                                                    Width="98%" OnItemCommand="dgShipType_ItemCommand" CssClass="GridShoppingStyle">
                                                    <HeaderStyle HorizontalAlign="center" />
                                                    <Columns>
                                                        <asp:TemplateColumn>
                                                            <ItemTemplate>
                                                                <asp:RadioButton ID="rdoSelectShipType" GroupName="SelectShipType" runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle BorderWidth="1px" />
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="ShipTypeName" HeaderText="配送方式">
                                                            <ItemStyle BorderWidth="1px" Width="60px" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="ShipPrice" HeaderText="运费">
                                                            <ItemStyle HorizontalAlign="Right" BorderWidth="1px" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="DiscountShipPrice" HeaderText="免除运费">
                                                            <ItemStyle HorizontalAlign="Right" BorderWidth="1px" ForeColor="Red" Width="60px" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Period" HeaderText="在途时间">
                                                            <ItemStyle BorderWidth="1px" HorizontalAlign="Left" Width="100px" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="ShipTypeDesc" HeaderText="详细介绍">
                                                            <ItemStyle BorderWidth="1px" HorizontalAlign="Left" />
                                                        </asp:BoundColumn>
                                                    </Columns>
                                                    <FooterStyle CssClass="GridShoppingFooterStyle" />
                                                    <SelectedItemStyle CssClass="GridShoppingSelectedRowStyle" />
                                                    <ItemStyle CssClass="GridShoppingRowStyle" />
                                                    <PagerStyle CssClass="GridShoppingPagerStyle" />
                                                    <AlternatingItemStyle CssClass="GridShoppingAlternatingRowStyle" />
                                                    <HeaderStyle CssClass="GridShoppingHeaderStyle" />
                                                </asp:DataGrid>
                                                <br />
                                                <div id="divDeliveryTime" runat="server" visible="false">
                                                    <table border="1" bordercolor="#b1d0ff" cellpadding="0" cellspacing="0" style="border-collapse: collapse;
                                                        text-align: center" width="98%" visible="true">
                                                        <tr>
                                                            <td bgcolor="#b1d0ff">
                                                                <div align="center">
                                                                    <strong><span class="fonts2">指定送货时段</span></strong></div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <table border="0" cellpadding="0" cellspacing="0" style="text-align: center" width="100%">
                                                                    <tr>
                                                                        <td width="34">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <table border="0" cellpadding="0" cellspacing="0" style="border-collapse: collapse"
                                                                                width="100%">
                                                                                <tr>
                                                                                    <td height="20">
                                                                                        <table border="0" cellpadding="0" cellspacing="0" height="20" style="text-align: center"
                                                                                            width="43%">
                                                                                            <tr>
                                                                                                <td align="right" width="65">
                                                                                                    <strong class="fonts1">送货时间：</strong>
                                                                                                </td>
                                                                                                <td align="left" width="102">
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="padding-left: 5px">
                                                                <p align="left">
                                                                    <strong class="fonts1">说 明：</strong><span class="fonts">为了避免您长时间等待收货，MMMbuy.cn 提供一日两次的送货服务，您可以根据您的需要设定送货时段。如果您没有指定送货时间MMMbuy.cn
                                                                        将在您的订单通过审核后的<br />
                                                                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;第二个工作日将货品送达。<a class="STYLE4"
                                                                            href="../Service/OneDayTwoSend.html" target="_blank">了解详情</a></span></p>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <br />
                                                <asp:CheckBox ID="chkIsPremium" runat="server" Font-Bold="true" ForeColor="#000000"
                                                    Text="我要购买货品运输保险" TextAlign="right" Visible="false" />
                                                <asp:Label ID="Label2" runat="server" EnableViewState="False" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="10" align="left">
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                付款方式：
                                <div class="" id="PayType">
                                    <table align="center" width="100%">
                                        <tr>
                                            <td height="10" align="left">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DataGrid ID="dgPayType" runat="server" Width="100%" DataKeyField="SysNo" AutoGenerateColumns="False"
                                                    GridLines="Vertical" OnItemCommand="dgPayType_ItemCommand" CssClass="GridShoppingStyle"
                                                    BorderWidth="1px">
                                                    <HeaderStyle HorizontalAlign="center" />
                                                    <Columns>
                                                        <asp:TemplateColumn ItemStyle-HorizontalAlign="center" ItemStyle-Width="30px" ItemStyle-BorderWidth="1px">
                                                            <ItemTemplate>
                                                                <asp:RadioButton ID="rdoSelectPayType" GroupName="SelectPayType" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="PayTypeName" HeaderText="支付方式" ItemStyle-HorizontalAlign="center"
                                                            ItemStyle-Width="100px" ItemStyle-BorderWidth="1px"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Period" HeaderText="到帐时间" ItemStyle-HorizontalAlign="center"
                                                            ItemStyle-Width="80px" ItemStyle-BorderWidth="1px"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="PayTypeDesc" HeaderText="详细介绍" ItemStyle-Width="680px"
                                                            ItemStyle-BorderWidth="1px" ItemStyle-HorizontalAlign="left"></asp:BoundColumn>
                                                    </Columns>
                                                    <FooterStyle CssClass="GridShoppingFooterStyle" />
                                                    <SelectedItemStyle CssClass="GridShoppingSelectedRowStyle" />
                                                    <ItemStyle CssClass="GridShoppingRowStyle" />
                                                    <PagerStyle CssClass="GridShoppingPagerStyle" />
                                                    <AlternatingItemStyle CssClass="GridShoppingAlternatingRowStyle" />
                                                    <HeaderStyle CssClass="GridShoppingHeaderStyle" />
                                                </asp:DataGrid>
                                            </td>
                                        </tr>
                                        <tr visible="false">
                                            <td>
                                                <div id="divTenpayQQ" runat="server" visible="false">
                                                    <table border="1" bordercolor="#b1d0ff" cellpadding="0" cellspacing="0" style="border-collapse: collapse;
                                                        text-align: center; width: 55%;" visible="true">
                                                        <tr>
                                                            <td style="width: 600px;">
                                                                请在此输入财付通账号：<asp:TextBox ID="txtTenpayQQ" runat="server"></asp:TextBox>，即可使用财付券享受折扣&nbsp;
                                                                <br />
                                                                您还没有财付券？<a href="http://portal.tenpay.com/christmas/index.html?posid=10&actid=121&opid=8&whoid=10"
                                                                    target="_blank"><font color="red">马上免费领取&gt;&gt;</font></a><br />
                                                                <span style="color: #ff0000">(请在生成订单后立即去财付通支付，否则无法使用财付券)</span>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;<asp:Label ID="Label3" runat="server" EnableViewState="False" Font-Bold="True"
                                                    Font-Size="Medium" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="10" align="left">
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                        <div class="shoppingstep_content_panel">
                            <div class="shoppingstep_content_panel_title">
                                用户留言
                            </div>
                            <div class="shoppingstep_content_panel_content">
                                <asp:TextBox ID="txtMessage" runat="server" Style="padding: 5px;" Width="100%" TextMode="MultiLine"
                                    Height="70px"></asp:TextBox>
                            </div>
                        </div>
                    </asp:Panel>
                    <div class="shoppingstep_content_panel">
                        <div class="shoppingstep_content_panel_content">
                            <asp:DataGrid ID="dgItem" runat="server" AutoGenerateColumns="False" Width="100%"
                                OnItemDataBound="dgItem_ItemDataBound" CssClass="GridShoppingStyle">
                                <HeaderStyle HorizontalAlign="center"></HeaderStyle>
                                <Columns>
                                    <asp:BoundColumn Visible="False" DataField="productsysno" HeaderText="productsysno">
                                        <ItemStyle BorderWidth="1px" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="productName" HeaderText="商品名称">
                                        <ItemStyle BorderWidth="1px" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Price" HeaderText="单价">
                                        <ItemStyle BorderWidth="1px" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="weight" HeaderText="单件重量(g)">
                                        <ItemStyle BorderWidth="1px" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="quantity" HeaderText="购买数量">
                                        <ItemStyle BorderWidth="1px" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="expectqty" HeaderText="期望购买数量">
                                        <ItemStyle BorderWidth="1px" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn HeaderText="金额小计" DataField="SubTotalAmt">
                                        <ItemStyle BorderWidth="1px" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn HeaderText="重量小计(g)" DataField="SubTotalWeight">
                                        <ItemStyle BorderWidth="1px" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Warranty" HeaderText="保修">
                                        <ItemStyle BorderWidth="1px" />
                                    </asp:BoundColumn>
                                </Columns>
                                <FooterStyle CssClass="GridShoppingFooterStyle" />
                                <SelectedItemStyle CssClass="GridShoppingSelectedRowStyle" />
                                <ItemStyle CssClass="GridShoppingRowStyle" />
                                <PagerStyle CssClass="GridShoppingPagerStyle" />
                                <AlternatingItemStyle CssClass="GridShoppingAlternatingRowStyle" />
                                <HeaderStyle CssClass="GridShoppingHeaderStyle" />
                            </asp:DataGrid>
                            <div id="trSRData" runat="server">
                                <asp:DataGrid ID="dgSaleRule" AutoGenerateColumns="False" Width="100%" runat="server"
                                    OnItemDataBound="dgSaleRule_ItemDataBound" CssClass="GridShoppingStyle">
                                    <ItemStyle Height="22px"></ItemStyle>
                                    <HeaderStyle Height="22px" CssClass="DataGridHead"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundColumn DataField="SaleRuleName" HeaderText="促销规则"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Discount" HeaderText="优惠金额"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Times" HeaderText="使用次数"></asp:BoundColumn>
                                        <asp:BoundColumn HeaderText="金额小计" DataField="SubTotal"></asp:BoundColumn>
                                    </Columns>
                                    <FooterStyle CssClass="GridShoppingFooterStyle" />
                                    <SelectedItemStyle CssClass="GridShoppingSelectedRowStyle" />
                                    <ItemStyle CssClass="GridShoppingRowStyle" />
                                    <PagerStyle CssClass="GridShoppingPagerStyle" />
                                    <AlternatingItemStyle CssClass="GridShoppingAlternatingRowStyle" />
                                    <HeaderStyle CssClass="GridShoppingHeaderStyle" />
                                </asp:DataGrid>
                            </div>
                        </div>
                        <div class="shoppingstep_content_panel_content_right">
                            <table id="Table4" cellspacing="0" cellpadding="0" border="0">
                                <tr>
                                    <td class="tdbg">
                                    </td>
                                    <td>
                                    </td>
                                    <td class="tdbg">
                                        现金支付合计：
                                    </td>
                                    <td>
                                        <asp:Label ID="lblSOAmt" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdbg">
                                    </td>
                                    <td>
                                    </td>
                                    <td class="tdbg">
                                        积分支付合计：
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPointPay" runat="server" ForeColor="SteelBlue"></asp:Label>
                                    </td>
                                </tr>
                                <tr id="trSaleRule" runat="server">
                                    <td class="tdbg">
                                    </td>
                                    <td>
                                    </td>
                                    <td class="tdbg">
                                        促销优惠：
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDiscountAmt" runat="server" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdbg">
                                        商品总重(含礼品)：
                                    </td>
                                    <td>
                                        <asp:Label ID="lblSOWeight" runat="server"></asp:Label>g
                                    </td>
                                    <td class="tdbg">
                                        运费：
                                    </td>
                                    <td>
                                        <asp:Label ID="lblShipPrice" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdbg">
                                    </td>
                                    <td>
                                    </td>
                                    <td class="tdbg">
                                        货运保险费：
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPremiumAmt" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdbg">
                                    </td>
                                    <td>
                                    </td>
                                    <td class="tdbg">
                                        小计：
                                    </td>
                                    <td>
                                        <asp:Label ID="lblSubSum" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr id="trPayPrice" runat="server">
                                    <td class="tdbg">
                                    </td>
                                    <td>
                                    </td>
                                    <td class="tdbg">
                                        手续费：
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPayPrice" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdbg">
                                    </td>
                                    <td>
                                    </td>
                                    <td class="tdbg">
                                        去零头：
                                    </td>
                                    <td>
                                        <asp:Label ID="lblChange" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdbg">
                                        订单可得积分：
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPointAmt" runat="server"></asp:Label>
                                    </td>
                                    <td class="tdbg">
                                        应付款总计：
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTotalMoney" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr height="0" style="visibility: hidden">
                                    <td class="tdbg">
                                    </td>
                                    <td>
                                    </td>
                                    <td class="tdbg">
                                        免运费支付：
                                    </td>
                                    <td>
                                        <asp:Label ID="lblFreeShipFeePay" runat="server" ForeColor="SteelBlue"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <div class="fixed">
                            </div>
                        </div>
                    </div>
                    <div>
                        <asp:Label ID="lblmsg" runat="server" ForeColor="Red" EnableViewState="False"></asp:Label></div>
                    <div style="text-align: center; padding: 5px 0 5px 0;">
                        <div style="margin-top: 10px; text-align: center;">
                            <asp:Button ID="btnCancel" runat="server" Text="作废订单" OnClick="btnCancel_Click" CssClass="ButtonAccount3" />
                            <asp:Button ID="btnUpdate" runat="server" Text="修改订单" OnClick="btnUpdate_Click" CssClass="ButtonAccount3" />
                            <div class="fixed">
                            </div>
                        </div>
                    </div>
                    <table id="Table1" cellspacing="0" cellpadding="0" width="96%" align="center" bgcolor="#ffffff"
                        border="0">
                        <tr height="20">
                            <td align="left">
                                <strong><a href="MyOrder.aspx" class="STYLE1">回到订单管理中心</a>&nbsp;</strong>
                            </td>
                        </tr>
                    </table>
                </div>
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
