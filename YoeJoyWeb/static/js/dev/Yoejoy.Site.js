/**
* YoeJoy Admin.js file
*/

/** ************************************************************************************************** */

YoeJoy.Site = new function () {

    var _this = this;

    //Cookie的操作类
    _this.Cookie = new function () {

        var _this = this;

        _this.GetCookieObj = function (cookieName) {
            var cookieArray = document.cookie.split("; "); //得到分割的cookie名值对    
            var cookie = new Object();
            for (var i = 0; i < cookieArray.length; i++) {
                var arr = cookieArray[i].split("=");       //将名和值分开    
                if (arr[0] == name) {
                    return unescape(arr[1]);
                } //如果是指定的cookie，则返回它的值    
            }
            return "";
        };

        _this.DeleteAllCookies = function (cookieName) {
            document.cookie = name + "=;expires=" + (new Date(0)).toGMTString();
        };

        _this.GetCookieValue = function (objName) {
            var arrStr = document.cookie.split("; ");
            for (var i = 0; i < arrStr.length; i++) {
                var temp = arrStr[i].split("=");
                if (temp[0] == objName) {
                    return unescape(temp[1]);
                }
            }
        };

        _this.AddCookie = function (objName, objValue, objHours) {      //添加cookie
            var str = objName + "=" + escape(objValue);
            if (objHours > 0) {                               //为时不设定过期时间，浏览器关闭时cookie自动消失
                var date = new Date();
                var ms = objHours * 3600 * 1000;
                date.setTime(date.getTime() + ms);
                str += "; expires=" + date.toGMTString();
            }
            document.cookie = str;
        };

        _this.SetCookie = function (name, value)//两个参数，一个是cookie的名字，一个是值
        {
            var Days = 30; //此 cookie 将被保存 30 天
            var exp = new Date();    //new Date("December 31, 9998");
            exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);
            document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString();
        };

        _this.GetAllCookies = function (name)//取cookies函数        
        {
            var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
            if (arr != null)
            { return unescape(arr[2]); }
            else {
                return null;
            }
        };


        _this.DeleteCookie = function (name)//删除cookie
        {
            var exp = new Date();
            exp.setTime(exp.getTime() - 1);
            var cval = getCookie(name);
            if (cval != null) document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString();
        };
    };

    //实用工具类
    _this.Utility = new function () {

        var _this = this;

        //get querystring from URL
        _this.GetQueryString = function (para) {
            var reg = new RegExp("(^|&)" + para + "=([^&]*)(&|$)", "i");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) {
                return unescape(r[2]);
            }
            return null;
        }

        //remove empty item from JS array
        _this.ReplaceEmptyItem = function (arr) {
            for (var i = 0, len = arr.length; i < len; i++) {
                if (!arr[i] || arr[i] == '') {
                    arr.splice(i, 1);
                    len--;
                    i--;
                }
            }
            return arr;
        }

        //get current site base url
        _this.GetSiteBaseURL = function (isHttps) {
            if (isHttps) {
                return ("https://" + window.location.host);
            }
            else {
                return ("http://" + window.location.host);
            }
        }

        //get JSON format string
        _this.GetJsonStr = function (str) {
            var json = eval('(' + str + ')');
            return json;
        }

    };

    //购物车
    _this.ShoppingCart = new function () {

        var _this = this;

        //购物车
        _this.MainCart = new function () {

            var _this = this;

            //清空购物车
            _this.ClearCartItems = function () {
                var shoppingCartServiceURL = YoeJoy.Site.Utility.GetSiteBaseURL(false) + "/Service/ShoppingCartService.aspx?cmd=clear";
                var params = "pid=99999";
                $.post(shoppingCartServiceURL,params,function (data) {
                    var result = YoeJoy.Site.Utility.GetJsonStr(data);
                    if (result.IsSuccess) {
                        alert(result.Msg);
                    }
                    else {
                        alert(result.Msg);
                    }
                });
            };

        };

        //页面头部的购物车快捷方式
        _this.ShortCuts = new function () {

            var _this = this;

            //更新购物车信息
            _this.RefreshOnlineShoppingCartShortCuts = function () {
                var shoppingCartServiceURL = YoeJoy.Site.Utility.GetSiteBaseURL(false) + "/Service/ShoppingCartService.aspx?cmd=view&random=" + Math.random();
                //var shoppingCartServiceURL = YoeJoy.Site.Utility.GetSiteBaseURL(false) + "/Service/ShoppingCartService.aspx?cmd=view";
                $.get(shoppingCartServiceURL, function (data) {
                    $("#count").empty().append(data);
                    YoeJoy.Site.ShoppingCart.ShortCuts.ReBindCartHoverEvent();
                });
            };

            //重新绑定Hover方法，当页面部分刷新以后
            _this.ReBindCartHoverEvent = function () {

                var char = $('#count .chartBt');
                var charf = $('#count');
                var charContent = $('#chartContent');
                var car = $('#chart .chartBt');

                char.hover(function () {
                    charContent.css('display', 'block');
                    car.addClass('sel');
                }, function () {
                });

                charf.hover(function () {
                }, function () {
                    charContent.css('display', 'none');
                    car.removeClass('sel');
                });

                charContent.hover(function () {
                    charContent.show();
                    car.addClass('sel');
                }, function () {
                    charContent.hide();
                    car.removeClass('sel');
                });
            };

            //删除购物车的商品
            _this.DeleteCartItem = function (sender) {
                var $this = $(sender);
                var deleteHandlerBaseURL = YoeJoy.Site.Utility.GetSiteBaseURL(false) + "/Service/ShoppingCartService.aspx?cmd=delete";
                var productSysNo = $this.children("input").val();
                var params = "pid=" + productSysNo;
                $.post(deleteHandlerBaseURL, params, function (data) {
                    var result = YoeJoy.Site.Utility.GetJsonStr(data);
                    if (result.IsSuccess) {
                        YoeJoy.Site.ShoppingCart.ShortCuts.RefreshOnlineShoppingCartShortCuts();
                    }
                    else {
                        alert(result.Msg);
                    }
                });
            };

            //更新购物车商品数量
            _this.UpdateCartItem = function (productSysNo, qtyCount) {
                var updateHandlerBaseURL = YoeJoy.Site.Utility.GetSiteBaseURL(false) + "/Service/ShoppingCartService.aspx?cmd=update";
                var params = "pid=" + productSysNo + "&qty=" + qtyCount;
                $.post(updateHandlerBaseURL, params, function (data) {
                    var result = YoeJoy.Site.Utility.GetJsonStr(data);
                    if (result.IsSuccess) {
                        YoeJoy.Site.ShoppingCart.ShortCuts.RefreshOnlineShoppingCartShortCuts();
                    }
                    else {
                        alert(result.Msg);
                    }
                });
            };

            //购物车商品数量减一
            _this.DecreaseItemNum = function (sender) {
                var $this = $(sender);
                var $numBox = $(sender).siblings("input[class='num']");
                var currentQty = parseInt($numBox.val());
                if (currentQty == 1) {
                    alert("购物车商品数不能小于1");
                }
                else {
                    var newCount = currentQty - 1;
                    var productSysNo = $this.siblings("p").children("input").val();
                    YoeJoy.Site.ShoppingCart.ShortCuts.UpdateCartItem(productSysNo, newCount);
                }
            };

            //购物车商品数量加一
            _this.IncreaseItemNum = function (sender) {
                var $this = $(sender);
                var $numBox = $(sender).siblings("input[class='num']");
                var currentQty = parseInt($numBox.val());
                var limitQty = parseInt($(sender).siblings("input[class='limitQty']").val());
                var availableQty = parseInt($(sender).siblings("input[class='availableQty']").val());
                if (currentQty >= limitQty) {
                    alert("购物车商品数不能大于限购数量，本商品一次限购：" + limitQty + "件");
                }
                else if (currentQty >= availableQty) {
                    alert("该商品库存不足，请修改数量");
                }
                else {
                    var newCount = currentQty + 1;
                    var productSysNo = $this.siblings("p").children("input").val();
                    YoeJoy.Site.ShoppingCart.ShortCuts.UpdateCartItem(productSysNo, newCount);
                }
            };

        };

    };


};
