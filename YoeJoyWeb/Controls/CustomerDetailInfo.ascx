<%@ Control Language="c#" Inherits="YoeJoyWeb.Controls.CustomerDetailInfo" CodeBehind="CustomerDetailInfo.ascx.cs" %>
<%@ Register TagPrefix="sc1" Namespace="YoeJoyWeb.Controls" Assembly="YoeJoyWeb" %>
<script type="text/javascript" src="../Controls/My97DatePicker/WdatePicker.js"></script>
<div class="fillInfo">
    <h2 class="titles">
        <b>�༭��������</b></h2>
    <p>
        <label>
            �û���:</label><span><asp:Label ID="lbUserId" runat="server"></asp:Label></span></p>
    <p>
        <label>
            ����Email:</label>
        <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox><asp:Label ID="lblEmail"
            runat="server"></asp:Label></p>
    <p>
        <label>
            <b>*</b>�ǳ�:</label><asp:TextBox ID="txtNickname" runat="server"></asp:TextBox></p>
    <p>
        <label>
            <b>*</b>��ʵ����</label><asp:TextBox ID="txtName" runat="server"></asp:TextBox></p>
    <p>
        <label>
            <b>*</b>�ֻ�:</label><asp:TextBox ID="txtCellPhone" runat="server"></asp:TextBox></p>
    <p>
        <label>
            <b>*</b>�绰:</label><asp:TextBox ID="txtPhone" runat="server"></asp:TextBox></p>
    <p>
        <label>
            <b>*</b>�Ա�:</label><asp:RadioButtonList ID="rblGeneder" runat="server">
                <asp:ListItem Text="��" Value="1"></asp:ListItem>
                <asp:ListItem Text="Ů" Value="0"></asp:ListItem>
            </asp:RadioButtonList>
    </p>
    <p>
        <label>
            <b>*</b>����:</label>
        <asp:TextBox ID="txtBirthDay" runat="server"></asp:TextBox><strong>����ȷ���󣬲����޸�</strong>
    </p>
    <p>
        <label><b>*</b>
            ��ס��:</label>
            <sc1:Area ID="scArea" runat="server" AutoPostBack="false">
            </sc1:Area>
    </p>
    <p>
        <label>
            ���ĵ�ַ:</label><asp:TextBox ID="txtDwellAddress" runat="server" Width="430px"></asp:TextBox></p>
    <p>
        <label><b>*</b>
            �����ʱ�:</label><asp:TextBox ID="txtZip" runat="server"></asp:TextBox></p>
    <p>
        <label><b>*</b>
            ���Ĵ���:</label>
        <asp:TextBox ID="txtFax" runat="server"></asp:TextBox></p>
    <p>
        <asp:ImageButton ID="btnSave" ImageUrl="../static/images/save.jpg" runat="server"
            OnClick="btnSave_Click" AlternateText="ȷ���޸�" />
    </p>
    <p>
        <asp:Label ID="lblErrMsg" runat="server" ForeColor="Red" EnableViewState="False"></asp:Label>
    </p>
</div>
