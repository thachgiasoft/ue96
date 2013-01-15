<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Payment.Master" AutoEventWireup="true"
    CodeBehind="CheckOut.aspx.cs" Inherits="YoeJoyWeb.Shopping.CheckOut" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link type="text/css" rel="Stylesheet" href="../static/css/base.css" />
    <link type="text/css" rel="Stylesheet" href="../static/css/process1.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
    <script type="text/javascript">
        <!--
        function CheckTime() {
            var canfirst = document.getElementById("ucSelectDeliveryTime_txtCanFirst");
            if (canfirst.value == "false") {
                var date = document.getElementById("ucSelectDeliveryTime_ddlDeliveryDate");
                var time = document.getElementById("ucSelectDeliveryTime_ddlDeliveryTime");
                if (date.options[0].selected && time.options[0].selected) {
                    alert('您不能选择 ' + date.options[0].text + time.options[0].text);
                    time.options[1].selected = true;
                }
            }
        }

        function replaceurl(url) {
            if (url != "") {
                window.open(url);
                window.close();
                return false;
            }
        }

        function SetDisabled() {
            document.all["btnOK"].disabled = 1;
            return true;
        }

        function OKClicked() {
            SetDisabled();
            __doPostBack('btnOK', '');
        }

        function checkBoxChecked(NotVat) {
            var chkInvoice = document.getElementById("chkInvoice");
            var vatInfoDiv = document.getElementById("vatInfo");

            if (chkInvoice.checked == true) {
                if (NotVat == "1")
                    alert("存在不能开增票商品，不能开增票的商品金额只能开普票，敬请谅解！");
                vatInfoDiv.style.display = "block";
            }
            else {
                vatInfoDiv.style.display = "none";
            }
        }

        function SelectShipType(obj, type) {
            var dg = document.getElementById('<%=dgShipType.ClientID %>');
            var items = dg.getElementsByTagName("input");

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

        function SelectPayType(obj, type) {
            var dg = document.getElementById('<%=dgPayType.ClientID %>');
            var items = dg.getElementsByTagName("input");

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
         -->
    </script>
    <div id="content">
        <div id="process">
            <img src="../static/images/step2.png" />
        </div>
        <div class="consigneeInfo">
            <h2 class="titles">
                <span>收货人信息</span><a class="modify" href="../User/MyAddress.aspx">[修改]</a></h2>
            <div id="addAdress">
                <div>
                    联系方式：
                    <asp:Label ID="lblName" runat="server"></asp:Label>,
                    <asp:Label ID="lblContact" runat="server"></asp:Label>,
                    <asp:Label ID="lblPhone" runat="server"></asp:Label>,
                    <asp:Label ID="lblCellPhone" runat="server"></asp:Label></div>
                <div>
                    收货地址：
                    <asp:Label ID="lblAreaName" runat="server"></asp:Label>
                    <asp:Label ID="lblAddress" runat="server"></asp:Label>,
                    <asp:Label ID="lblZip" runat="server"></asp:Label></div>
            </div>
        </div>
        <div id="SentMethod">
            <h2 class="titles">
                配送方式
            </h2>
            <div class="model" style="display: block;">
                <asp:Label ID="lblShipTypeName" runat="server" Visible="false"></asp:Label>
                <asp:DataGrid ID="dgShipType" runat="server" CssClass="GridShoppingStyle" AutoGenerateColumns="False"
                    DataKeyField="SysNo" Width="98%" OnItemCommand="dgShipType_ItemCommand" OnItemDataBound="dgShipType_ItemDataBound">
                    <Columns>
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <asp:RadioButton ID="rdoSelectShipType" runat="server" OnCheckedChanged="rdoSelectShipType_CheckedChanged"
                                    AutoPostBack="True" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="ShipTypeName" HeaderText="配送方式">
                            <ItemStyle Width="60px" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ShipPrice" HeaderText="运费">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="DiscountShipPrice" HeaderText="免除运费">
                            <ItemStyle HorizontalAlign="Right" Width="60px" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Period" HeaderText="在途时间">
                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ShipTypeDesc" HeaderText="详细介绍">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                    </Columns>
                    <FooterStyle CssClass="GridShoppingFooterStyle" />
                    <SelectedItemStyle CssClass="GridShoppingSelectedRowStyle" />
                    <ItemStyle CssClass="GridShoppingRowStyle" />
                    <PagerStyle CssClass="GridShoppingPagerStyle" />
                    <AlternatingItemStyle CssClass="GridShoppingAlternatingRowStyle" />
                    <HeaderStyle CssClass="GridShoppingHeaderStyle" HorizontalAlign="Center" />
                </asp:DataGrid>
                <div id="divDeliveryTime" runat="server" visible="false">
                    <table border="1" cellpadding="0" cellspacing="0" style="border-collapse: collapse;
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
                                                        <table border="0" cellpadding="0" cellspacing="0" style="text-align: center" width="43%">
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
                <asp:CheckBox ID="chkIsPremium" runat="server" Font-Bold="true" ForeColor="#000000"
                    Text="我要购买货品运输保险" TextAlign="right" Visible="false" />
            </div>
        </div>
        <div id="PriceMethod">
            <h2 class="titles">
                <span>支付方式</span></h2>
            <div class="choiced" style="margin-top: 15px; margin-right: 15px; margin-bottom: 25px;
                margin-left: 15px;">
                <asp:Label ID="lblPayTypeName" runat="server" Visible="false"></asp:Label>
            </div>
            <div class="model" style="display: block;">
                <table align="center" width="100%">
                    <tr>
                        <td height="10" align="left">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DataGrid ID="dgPayType" runat="server" Width="100%" DataKeyField="SysNo" AutoGenerateColumns="False"
                                OnItemCommand="dgPayType_ItemCommand" CssClass="GridShoppingStyle" OnItemDataBound="dgPayType_ItemDataBound">
                                <HeaderStyle HorizontalAlign="center" />
                                <Columns>
                                    <asp:TemplateColumn ItemStyle-HorizontalAlign="center">
                                        <ItemTemplate>
                                            <asp:RadioButton ID="rdoSelectPayType" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="PayTypeName" HeaderText="支付方式" ItemStyle-HorizontalAlign="center"
                                        ItemStyle-Width="100px"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Period" HeaderText="到帐时间" ItemStyle-HorizontalAlign="center"
                                        ItemStyle-Width="80px"></asp:BoundColumn>
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
                                <table border="1" cellpadding="0" cellspacing="0" style="border-collapse: collapse;
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
                </table>
            </div>
        </div>
        <div id="InvoiceInfo">
            <h2 class="titles">
                订购商品项目核对
            </h2>
            <div id="modfiedGoods">
                <asp:Label ID="lblValidScore" runat="server" ForeColor="Red"></asp:Label>
                <asp:DataGrid ID="dgItem" runat="server" Width="100%" AutoGenerateColumns="False"
                    DataKeyField="productSysNo" OnItemDataBound="dgItem_ItemDataBound" CssClass="GridShoppingStyle">
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="productsysno"></asp:BoundColumn>
                        <asp:BoundColumn DataField="productName" HeaderText="商品名称">
                            <ItemStyle HorizontalAlign="Left" Width="400px" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="PriceShow" HeaderText="单价">
                            <ItemStyle HorizontalAlign="Right" Width="70px" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="weight" HeaderText="单件重量(g)">
                            <ItemStyle HorizontalAlign="Right" Width="70px" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="quantity" HeaderText="购买数量">
                            <ItemStyle HorizontalAlign="Right" Width="60px" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="expectqty" HeaderText="期望购买数量">
                            <ItemStyle HorizontalAlign="Right" Width="60px" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="LineTotal" HeaderText="金额小计">
                            <ItemStyle HorizontalAlign="Right" Width="70px" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="LineWeight" HeaderText="重量小计(g)">
                            <ItemStyle HorizontalAlign="Right" Width="70px" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="DeliveryTimeliness" HeaderText="发货时间">
                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="InStock" HeaderText="提示" Visible="False">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="IsCanVat" HeaderText="可开增票">
                            <ItemStyle HorizontalAlign="Center" Width="60px" />
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
                    <asp:DataGrid ID="dgSaleRule" runat="server" Width="100%" AutoGenerateColumns="False">
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
                </div>
                <div id="trPointPay" runat="server">
                    <asp:Label ID="lblThisPoint" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                    <asp:TextBox ID="txtPointPay" Style="text-align: right; width: 50px" runat="server"
                        Font-Bold="True" Wrap="False" Text="0"></asp:TextBox>&nbsp;&nbsp;注：100 积分兑换
                    1 元。<a href="../Service/Point.html" target="_blank">查看积分说明</a>
                </div>
                <p>
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="tdbg">
                            </td>
                            <td>
                            </td>
                            <td class="tdbg">
                                商品支付现金合计：
                            </td>
                            <td>
                                <asp:Label ID="lblCashPay" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                            </td>
                        </tr>
                        <tr runat="server" id="trYiJiaWang" visible="false">
                            <td class="tdbg">
                                易价网用户名：<asp:TextBox ID="txtAdwaysID" runat="server" Font-Bold="True" Width="50px"></asp:TextBox>
                                易价网Email：<asp:TextBox ID="txtAdwaysEmail" runat="server" Font-Bold="True" Width="50px"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                            <td class="tdbg">
                            </td>
                            <td>
                                <asp:Button ID="txtSubmitAdways" runat="server" Text="免运费" OnClick="txtSubmitAdways_Click" />
                            </td>
                        </tr>
                        <tr id="trPP" runat="server" visible="false">
                            <td class="tdbg">
                                宽带山校验码：
                            </td>
                            <td>
                                <asp:TextBox ID="txtPPCode" runat="server" Font-Bold="False" Width="136px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btnSubmitPP" runat="server" OnClick="btnSubmitPP_Click" Text="提交PP" />
                            </td>
                            <td>
                                <%=ppLink %>
                            </td>
                        </tr>
                        <tr id="trSaleRuleTotal" runat="server">
                            <td class="tdbg">
                            </td>
                            <td>
                            </td>
                            <td class="tdbg">
                                优惠：
                            </td>
                            <td>
                                <asp:Label ID="lblSaleRuleTotal" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdbg">
                                货物总重(含礼品)：
                            </td>
                            <td>
                                <asp:Label ID="lblSOWeight" runat="server"></asp:Label>
                            </td>
                            <td class="tdbg">
                                实际运费：
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
                        <tr id="trHandlePrice" runat="server">
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
                                去零头(限货到付款)：
                            </td>
                            <td>
                                <asp:Label ID="lblChange" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdbg">
                                本单可得积分：
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
                    </table>
                    <div class="fixed">
                    </div>
                </p>
            </div>
        </div>
        <div class="shoppingstep_content_panel">
            <div class="shoppingstep_content_panel_title">
                给攸怡 留言
            </div>
            <div class="shoppingstep_content_panel_content">
                <p>
                    如果您有什么需要关照攸怡 工作人员特别注意的事项，请在这里留言，我们会为您安排处理。</p>
            </div>
        </div>
        <div id="trSignByOther" runat="server" style="padding: 10px;">
            代收货品提示：送货时，如果快递无法与您取得联系，您是否同意由您的朋友、同事或家人为您代收货品：<asp:RadioButtonList ID="rblSignByOther"
                runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                <asp:ListItem Selected="True" Value="0">同意</asp:ListItem>
                <asp:ListItem Value="-1">不同意</asp:ListItem>
            </asp:RadioButtonList>
        </div>
        <div id="trIsMergeSO" runat="server" style="display: none;">
            <p>
                合并订单提示：您存在尚未出库订单，是否同意攸怡客服为您合并订单：<asp:RadioButtonList ID="rblIsMergeSO" runat="server"
                    RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <asp:ListItem Selected="True" Value="1">同意</asp:ListItem>
                    <asp:ListItem Value="0">不同意</asp:ListItem>
                </asp:RadioButtonList>
            </p>
        </div>
        <div class="shoppingstep_content_panel">
            <div class="shoppingstep_content_panel_title">
                购物票据注意事项
            </div>
            <div class="shoppingstep_content_panel_content">
                <p>
                    为确保客户得到良好的售后服务，攸怡会为所有售出商品开具加盖公章的机打商品保修单，客户凭销售单号码即可保修或退换；如需要普通商业零售发票的客户请给攸怡 留言，一张订单只能开具一张发票；开具发票的商品在保修或退换时必须出具原始发票，不能出具发票的售后服务申请攸怡将拒绝受理；商品保修单、发票会随同货物一起寄出。如未能及时申请索取发票，在货物发出后2周内可以申请补开发票，攸怡
                    将为您补开发票，发票以快递方式寄出，快递费用由申请人承担。</p>
                <div style="text-align: center;">
                    <asp:TextBox ID="txtMemo" runat="server" Width="90%" CssClass="inputsearch" TextMode="MultiLine"
                        Height="80px"></asp:TextBox>
                </div>
            </div>
        </div>
        <div id="vatnote" runat="server">
            <asp:CheckBox ID="chkInvoice" runat="server" Text="需要增值税发票："></asp:CheckBox>
            按照国家税务总局的要求，对于需要开具增值税发票的订单，请客户务必提供<font color="red">一般纳税人资格证书</font>的复印件。请您在成功提交订单之后，将一般纳税人资格证书传真至021-64699706。为了您的订单可以得到及时的处理，请在传真上<font
                color="red">标明订单号码。</font> 同时，请详细填写企业名称、地址电话、税号、开户银行和帐号，并注意所填信息须与税务登记证上的信息一致。<br />
            <font color="red">特别说明：增值税发票只针对有一般纳税人资格的企业，普通客户请勿勾选此单。</font></div>
        <div id="vatInfo" name="vatInfo" runat="server">
            <table id="TABLE1" height="100%" cellspacing="0" cellpadding="0" width="98%" border="0"
                runat="server" style="border-collapse: collapse;">
                <tr>
                    <td style="width: 99px; text-align: right;">
                        公司名称：
                    </td>
                    <td>
                        <asp:TextBox ID="txtCompanyName" runat="server" Width="350px"></asp:TextBox>
                    </td>
                    <td style="width: 99px; text-align: right;">
                        公司地址：
                    </td>
                    <td>
                        <asp:TextBox ID="txtCompanyAddr" runat="server" Width="350px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 99px; text-align: right;">
                        公司电话：
                    </td>
                    <td>
                        <asp:TextBox ID="txtCompanyPhone" runat="server" Width="350px"></asp:TextBox>
                    </td>
                    <td style="width: 99px; text-align: right;">
                        税务登记号：
                    </td>
                    <td>
                        <asp:TextBox ID="txtTaxNum" runat="server" Width="350px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 99px; text-align: right;">
                        开户银行及帐号：
                    </td>
                    <td>
                        <asp:TextBox ID="txtBankAndAccount" runat="server" Width="350px"></asp:TextBox>
                    </td>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 99px; height: 25px; text-align: right;">
                        其他说明：
                    </td>
                    <td colspan="3" style="height: 25px">
                        <asp:TextBox ID="txtOthers" runat="server" Width="95%"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <p>
                <asp:Label ID="lblmsg" runat="server" ForeColor="Red" Font-Bold="True" Font-Size="Medium"
                    EnableViewState="False"></asp:Label></p>
        </div>
        <div style="text-align: center; padding: 5px 0 5px 0;">
            <div style="margin: 10px 0 0 320px;">
                <asp:ImageButton ID="btnOK" OnClick="btnOK_Click" runat="server" ImageUrl="../static/images/tj.jpg" />
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
