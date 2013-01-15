<%@ Page Title="" Language="C#" MasterPageFile="~/Master/User.Master" AutoEventWireup="true"
    CodeBehind="MyAddress.aspx.cs" Inherits="YoeJoyWeb.User.MyAddress" %>

<%@ Register TagPrefix="sc1" Namespace="YoeJoyWeb.Controls" Assembly="YoeJoyWeb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavContentPlaceHolder" runat="server">
<div id="position">
        <span>
            <img src="../static/images/f4.jpg" />您在:</span> <b><a href="../Default.aspx">首页</a></b>
        <span>&gt;</span> <span><b><a href="MyProfile.aspx">用户中心</a></b></span><span>&gt;</span>
        <span>收货地址管理</span>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="RightBigModule" runat="server">
    <div class="myaddress">
        <h2 class="titles">
            <b>收货地址管理</b></h2>
        <asp:DataList ID="DataList1" runat="server" DataKeyField="SysNo" OnItemCommand="DataList1_ItemCommand"
            OnItemDataBound="DataList1_ItemDataBound" Width="100%" CellSpacing="0" CellPadding="0">
            <HeaderTemplate>
                <thead>
                    <tr>
                        <th>
                            地址标注
                        </th>
                        <th>
                            用户姓名
                        </th>
                        <th>
                            收货人姓名
                        </th>
                        <th>
                            固定电话
                        </th>
                        <th>
                            手机号码
                        </th>
                        <th>
                            城市
                        </th>
                        <th>
                            地址
                        </th>
                        <th>
                            邮编
                        </th>
                        <th>
                            传真
                        </th>
                        <th>
                            操作
                        </th>
                    </tr>
                </thead>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <asp:Label ID="lblIsDefault" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "IsDefault")%>'></asp:Label>
                        <asp:Label ID="lblBrief" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Brief")%>'></asp:Label>
                        <asp:LinkButton ID="lnkbtnSetDefault" Style="font-weight: 700;" Font-Underline="false"
                            runat="server" Text=" 设为默认 " CommandName="SetDefault" ForeColor="black">
                        </asp:LinkButton>
                    </td>
                    <td>
                        <asp:Label ID="lblName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Name")%>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblContact" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Contact")%>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblPhone" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Phone")%>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "CellPhone")%>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblAreaName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "AreaName")%>'><</asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblAddress" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Address")%>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblZip" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Zip")%>'></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Fax")%>'></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="lnkbtnUpdate" runat="server" CommandName="Update" AlternateText="编 辑"
                            ImageUrl="../static/images/edit.jpg" />
                    </td>
                    <td>
                        <asp:ImageButton ID="lnkbtnDelte" runat="server" CommandName="Delete" AlternateText="删 除"
                            ImageUrl="../static/images/del.jpg" />
                    </td>
                </tr>
            </ItemTemplate>
        </asp:DataList>
        <br />
        <p>
            <asp:LinkButton ID="lntbtnAdd" runat="server" OnClick="lntbtnAdd_Click"> + 添加收货地址</asp:LinkButton></p>
        <div id="AddressInfo" class="NewAddressContent" runat="server">
            <asp:Label ID="lblSysNo" runat="server" Visible="False">0</asp:Label>
            <asp:Label ID="lblIsDefault" runat="server" Visible="False">-1</asp:Label>
            <asp:Label ID="lblCustomerSysNo" runat="server" Visible="False">0</asp:Label>
            <p>
                <label>
                    地址标注</label><asp:TextBox ID="txtBrief" runat="server"></asp:TextBox><span>例如在家里，公司，最多4个汉字或者8个字符</span></p>
            <p>
                <label>
                    姓名：
                </label>
                <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
            </p>
            <p>
                <label>
                    <b>*</b>选择地区</label><sc1:Area ID="scArea" runat="server" AutoPostBack="False">
                    </sc1:Area>
            </p>
            <p>
                <label>
                    <b>*</b>详细地址</label><asp:TextBox ID="txtAddress" runat="server"></asp:TextBox></p>
            <p>
                <label>
                    邮政编码</label><asp:TextBox ID="txtZip" runat="server"></asp:TextBox></p>
            <p>
                <label>
                    <b>*</b>收货人</label><asp:TextBox ID="txtContact" runat="server"></asp:TextBox></p>
            <p>
                <label>
                    <b>*</b>手机</label><asp:TextBox ID="txtCellPhone" runat="server"></asp:TextBox>
                <strong>电话</strong><asp:TextBox ID="txtPhone" runat="server"></asp:TextBox><strong>传真</strong>
                <asp:TextBox ID="txtFax" runat="server"></asp:TextBox>
            </p>
            <p>
                <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="保存地址" />
                <asp:Button ID="btnCancel" runat="server" Text="取消" OnClick="btnCancel_Click" /></p>
            <p>
                <asp:Label ID="lblErrMsg" runat="server" EnableViewState="False" ForeColor="Red"></asp:Label></p>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BackupContent1" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BackupContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <script type="text/javascript">
        $(function () {

            $("#RightBigModule_DataList1").children("tbody").eq(0).remove();
            $("#RightBigModule_DataList1").children("tbody").eq(0).children("tr:even").remove();

        });
    </script>
</asp:Content>
