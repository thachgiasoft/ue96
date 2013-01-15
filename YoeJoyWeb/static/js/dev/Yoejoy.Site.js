/**
* YoeJoy Admin.js file
*/

/** ************************************************************************************************** */

YoeJoy.Site = new function () {

    var _this = this;

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

};
