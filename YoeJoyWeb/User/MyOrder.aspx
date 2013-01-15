<%@ Page Title="" Language="C#" MasterPageFile="~/Master/User.Master" AutoEventWireup="true"
    CodeBehind="MyOrder.aspx.cs" Inherits="YoeJoyWeb.User.MyOrder" %>

<%@ Register Assembly="DBCSoft.Component" Namespace="DBCSoft.Component" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavContentPlaceHolder" runat="server">
    <div id="position">
        <span>
            <img src="../static/images/f4.jpg" />您在:</span> <b><a href="../Default.aspx">首页</a></b>
        <span>&gt;</span> <span><b><a href="MyProfile.aspx">用户中心</a></b></span><span>&gt;</span>
        <span>我的订单</span>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="RightBigModule" runat="server">
    <div class="myOrders">
        <h2 class="titles">
            <b>我的订单</b></h2>
        <div class="OrdersSearch">
            <div class="l">
                <select>
                    <option>一周内订单</option>
                </select>
                <select>
                    <option>全部订单</option>
                </select>
            </div>
            <div class="r">
                <input type="text" />
                <a href="#">查询</a>
            </div>
        </div>
    </div>
    <div class="Order_List">
        <asp:DataList ID="DataList1" runat="server" Width="100%">
            <ItemTemplate>
                <table id="Table2" class="order">
                    <tr>
                        <th class="orderNum" rowspan="3">
                            <p>
                                订单编号：
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("soid") %>'></asp:Label></p>
                            <p>
                                运单编号：<asp:Label ID="Label6" runat="server" Text='<%# Bind("doNo")%>'></asp:Label></p>
                            <p>
                                订购日期：<asp:Label ID="Label2" runat="server" Text='<%# Bind("OrderDate") %>'></asp:Label></p>
                            <p>
                                付款方式：在线支付</p>
                            <p>
                                订单总计：
                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("totalCash") %>'></asp:Label></p>
                            <p>
                                使用积分：<asp:Label ID="Label4" runat="server" Text='<%# Bind("PointPay")%>'></asp:Label></p>
                            <p>
                                可得积分：
                                <asp:Label ID="Label5" runat="server" Text='<%# Bind("PointAmt")%>'></asp:Label></p>
                        </th>
                        <td colspan="3">
                            <h2 class="ordertitlts">
                                <span class="l"><b>
                                    <asp:Label ID="Label7" runat="server" Text='<%# Bind("statusName")%>'></asp:Label></b></span>
                                <span class="r">
                                    <asp:Label ID="Label8" runat="server" ForeColor="blue" Text='<%# Bind("opt") %>'></asp:Label></span>
                            </h2>
                        </td>
                    </tr>
                    <tr class="orderList">
                        <td>
                            <img src="../static/images/plsp.jpg">
                        </td>
                        <td>
                            <p>
                                <a href="#">雪之恋 手搓麻署(绿茶牛奶)180g/袋(台湾地区)</a></p>
                            <p>
                                <b>¥199900.00</b>X2</p>
                        </td>
                        <th>
                            <a class="reBuy" href="#">再次购买</a><br>
                            <a class="reBuy" href="#">发表评论</a>
                        </th>
                    </tr>
                    <tr class="orderList last">
                        <td>
                            <img src="../static/images/plsp.jpg">
                        </td>
                        <td>
                            <p>
                                <a href="#">雪之恋 手搓麻署(绿茶牛奶)180g/袋(台湾地区)</a></p>
                            <p>
                                <b>¥199900.00</b>X2</p>
                        </td>
                        <th>
                            <a class="reBuy" href="#">再次购买</a><br>
                            <a class="reBuy" href="#">发表评论</a>
                        </th>
                    </tr>
                    <asp:Label ID="lblRemark" runat="server" Text='<%#  Bind("Memo") %>'>
                    </asp:Label>
                </table>
                <br />
            </ItemTemplate>
        </asp:DataList>
        <div class="navStyle">
            <cc1:NavigatorBar ID="NavigatorBar1" runat="server" ImagePath="../images/pageimages/"
                NavMode="number" OnPageIndexChanged="NavigatorBar1_PageIndexChanged">
            </cc1:NavigatorBar>
            <div class="fixed">
            </div>
        </div>
    </div>
    <div class="Order_Description">
        <table width="100%" border="0" cellspacing="5" cellpadding="5">
            <tr>
                <td width="5" id="b2">
                    <a name="#status"></a>
                </td>
            </tr>
            <tr>
                <td style="padding-left: 10px;">
                    您的订单分别会有<font color="red"><strong>"待审核，待支付，待出库，已出库，客户作废，员工作废"</strong></font>这六种状态，分别说明如下：
                </td>
            </tr>
            <tr>
                <td style="padding-left: 10px;">
                    <strong><font color="red">1. “待审核”：</font></strong>
                </td>
            </tr>
            <tr>
                <td style="padding-left: 30px;">
                    我们的审核人员还没有开始审核，最迟当天（工作时间内）会审核，请您耐心等待。
                </td>
            </tr>
            <tr>
                <td style="padding-left: 10px;">
                    <strong><font color="red">2. “待支付”：</font></strong>
                </td>
            </tr>
            <tr>
                <td style="padding-left: 30px;">
                    您还没有支付货款， 或您已经通过网上支付、邮局、银行电汇支付货款，我们还没有收到。款到后即可发货。<br />
                </td>
            </tr>
            <tr>
                <td style="padding-left: 10px;">
                    <strong><font color="red">3. “待出库”：</font></strong>
                </td>
            </tr>
            <tr>
                <td style="padding-left: 30px;">
                    我们已经收到您的货款（货到付款除外），并且已经通过业务人员的审核，仓库正在备货、核查、包装。<br />
                </td>
            </tr>
            <tr>
                <td style="padding-left: 10px;">
                    <strong><font color="red">4. “已出库”：</font></strong>
                </td>
            </tr>
            <tr>
                <td style="padding-left: 30px;">
                    仓库正在安排配送，请您耐心等待。<br />
                </td>
            </tr>
            <tr>
                <td style="padding-left: 10px;">
                    <strong><font color="red">5. “客户作废”：</font></strong>
                </td>
            </tr>
            <tr>
                <td style="padding-left: 30px;">
                    若您的订单还是“待审核”状态，您可以在“帐户中心 > 订单管理”中作废您的订单。<br />
                </td>
            </tr>
            <tr>
                <td style="padding-left: 10px;">
                    <strong><font color="red">6. “员工作废”：</font></strong>
                </td>
            </tr>
            <tr>
                <td style="padding-left: 30px; padding-right: 20px;">
                    <ul>
                        <li>1) 您来电要求作废订单</li>
                        <li>2) 由于您的订单重复、地址不详等原因，攸怡员工将您的订单作废</li>
                        <li>3) 由于拆单、合并订单、修改订单等原因，将原订单先作废</li>
                        <li>4) 根据一定的逾期规则，系统自动作废您的订单</li>
                    </ul>
                    <ul style="background: white; padding: 20px; margin-top: 10px;">
                        <li><font color="">逾期规则如下：</font></li>
                        <li>a. 自提订单：保留三个工作日（以订单到达自提点时间算起）</li>
                        <li>b. 网上支付订单：保留一个工作日（以订单生成时间算起，三个工作日后如我们还未收到您的货款，系统将自动作废您的订单）</li>
                        <li>c. 邮局汇款、银行电汇订单：保留一个工作日（以订单生成时间算起，七个工作日后如我们还未收到您的货款，系统将自动作废您的订单）</li>
                        <li><strong>若还有其他疑问，欢迎您致电 400-808-9196 垂询或给我们发送电子邮件。</strong></li>
                    </ul>
                </td>
            </tr>
            <tr>
                <td style="padding-left: 10px;">
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BackupContent1" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BackupContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
</asp:Content>
