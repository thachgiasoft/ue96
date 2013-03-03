<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Payment.Master" AutoEventWireup="true" CodeBehind="CheckOut_new.aspx.cs" Inherits="YoeJoyWeb.Shopping.CheckOut_new" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link type="text/css" rel="Stylesheet" href="../static/css/base.css" />
    <link type="text/css" rel="Stylesheet" href="../static/css/process1.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
    <div id="content">
        <div id="process">
            <img src="../static/images/step2.png" />
            <ul>
                <li>1.查看购物车</li>
                <li>2.核对订单信息</li>
                <li>3.成功提交订单</li>
                <li>4.评论商品</li>
            </ul>
        </div>
        <div class="consigneeInfo">
        	<h2 class="titles"><span>请填写收货人信息</span><a class="modify" href="#">[修改]</a></h2>
	            <div id="addAdress">
            	<p class="myNowAddress" style="display: block;">
                    <input type="radio" checked="checked">
                    <label>姓名 上海 闵行区 七莘路2288弄25栋302室 200010</label>    	
                    <span>常用地址</span>
                </p>
                <a class="addNewAdress" style="margin-top: 5px; display: none;" href="#">添加新地址</a>                
                <div id="NewAdressContent" style="display: none;">
                	<p>
                    	<label>
                        	<span>*</span>
                            收货人：
                         </label>
                         <input type="text"/>
                   	</p>
                    
                	<p>
                    	<label>
                        	<span>*</span>
                            收货地址：
                         </label>
                         <select>
                         	<option>上海市</option>
                         </select>
                         <select>
                         	<option>上海市</option>
                         </select>
                   	</p>
                    
                	<p>
                    	<label>
                        	<span>*</span>
                            详细地址
                         </label>
                         <input style="width: 402px;" type="text">
                   	</p>
                    
                	<p>
                    	<label>
                        	<span>*</span>
                            手机号码：
                         </label>
                         <input type="text">
                         <em>或者 固定电话:</em>
                         <input type="text">
                   	</p>
                    
                    <p>
                    	<input type="checkbox" checked="checked">
                        设为常用地址
                    </p>
                    <a style="margin-top: 10px;" href="#"><img class="save" src="../static/images/dd2.jpg"></a>
                </div>
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
