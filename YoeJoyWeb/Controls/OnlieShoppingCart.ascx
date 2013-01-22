<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OnlieShoppingCart.ascx.cs"
    Inherits="YoeJoyWeb.Controls.OnlieShoppingCart" %>
<div id="count">
    <%=OnlieShoppingCartHTML%>
</div>


<div style="display: none;" id="chartContent">
        <h5><span></span></h5>
        <div class="shopping">
          <p class="l"> <a href="product.html"><img alt="产品" src="images/char.jpg" width="30" height="30"></a><a class="goodsName" href="product.html">全脂牛奶全脂牛奶全脂牛奶全脂牛奶</a><b>￥15000.00</b> </p>
          <div class="r"> <a class="sub" href="javascript:void(0)">-</a>
            <input class="num" maxLength="3" value="1" type="text">
            <a class="add" href="javascript:void(0)">+</a>
            <p>删除</p>
          </div>
        </div>
        <div class="payNow">
          <div class="l"> 共<b>12</b>件商品 </div>
          <div class="r">
            <p>合计：<b>￥1900.9</b></p>
            <a href="process1.html"><img alt="结算" src="images/jsbt.png" width="61" height="25"></a> </div>
        </div>
      </div>