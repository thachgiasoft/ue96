<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Payment.Master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="YoeJoyWeb.User.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link type="text/css" rel="Stylesheet" href="../static/css/base.css" />
    <link type="text/css" rel="Stylesheet" href="../static/css/layout.css" />
    <link type="text/css" rel="Stylesheet" href="../static/css/login.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="http://qzonestyle.gtimg.cn/qzone/openapi/qc_loader.js"
        data-appid="100346020" data-redirecturi="http://www.ue96.com/Default.aspx"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
    <div id="login_global" class="clear">
        <!--登录区-->
        <div id="loginTab" class="login mastW">
            <h3 class="title">
                攸怡用户登入</h3>
            <div class="group">
                <p class="input">
                    <em>账户名：</em><input id="txtID" class="default" type="text"></p>
                <p class="input">
                    <em>密码：</em><input id="txtPass" type="password"><a class="link3" href="#">找回密码</a></p>
                <p>
                    <em></em>
                    <input type="checkbox" id="cbRememberName" /><span>记住账户名</span><input type="checkbox"
                        id="cbAutoLogin" /><span>自动登录</span></p>
                <p class="button">
                    <input id="btnLogin" value="登入" type="button" /><span id="qqLoginBtn"></span></p>
                <h5 class="joint">
                    使用合作网站账号登录攸怡：</h5>
                <ul class="clear">
                    <li class="163"><a class="link1" href="#">网易</a></li>
                    <li class="renren"><a class="link1" href="#">人人网</a></li>
                    <li class="sina"><a class="link1" href="#">新浪微博</a></li>
                    <li class="kaixin"><a class="link1" href="#">开心网</a></li>
                    <li class="douban"><a class="link1" href="#">豆瓣</a></li>
                </ul>
            </div>
        </div>
        <!--注册区-->
        <div id="registerTable" class="register slaveW">
            <h3>
                注册成为攸怡用户</h3>
            <div class="group">
                <p>
                    <em></em><strong>请填写以下注册信息，均为必填项：</strong></p>
                <p class="input">
                    <em>邮箱：</em><input id="txtEmail" name="邮箱" type="text" /></p>
                <p class="note">
                    <span>邮箱可作为登入账号，并用于找回密码，接收订单通知等。</span><strong></strong></p>
                <p class="input">
                    <em>用户名：</em><input id="txtRegisterName" name="用户名" type="text" /></p>
                <p class="note">
                    <span>4-20位字符，可由中文、字母、数字及特殊字符组成。</span><strong></strong></p>
                <p class="input">
                    <em>密码：</em><input id="password" name="密码" type="password" /></p>
                <p class="note">
                    <span>6-20位字符，可由字母、数字或符号的组合。</span><strong></strong></p>
                <p class="input">
                    <em>确认密码：</em><input id="password1" name="确认密码" type="password" /></p>
                <p class="note">
                    <span>请再次输入密码。</span><strong></strong></p>
                <p class="input code">
                    <em>验证码：</em><input id="txtCaptcha" name="验证码" type="text" /><img id="imgCaptcha"
                        alt="验证码" src="../Service/Captcha.aspx" width="69" height="27" /><span class="slave">看不清？</span><a
                            id="btnRefreshCaptcha" class="link1" href="javascript:void(0)">换一张</a></p>
                <p class="note">
                    <span>请输入图片中的字符，不区分大小写。</span></p>
                <p class="button">
                    <input id="btnRegister" value="同意以下协议并注册" type="button" /></p>
                <textarea>          《攸怡服务协议》（以下简称“本协议”）是由攸怡网站的运营方（以下简称“攸怡或攸怡网站”）在提供域名为www.ue96.com的网络运营服务时与攸怡的使用者（以下简称“用户”）达成的关于使用攸怡网站服务的各项条款、条件和规则。

如果您访问攸怡网站或在攸怡网站购物，或以任何行为实际使用、享受攸怡的服务，即表示您接受了本协议，并同意受本协议各项条款的约束。如果您不同意本协议中的任何内容，您可以选择不使用本网站。

本协议包括协议正文及所有攸怡网站已经发布的或将来可能发布或更新的各类规则，所有规则为本协议不可分割的组成部分，与本协议正文具有同等法律效力。您应当仔细阅读本协议的正文内容及其所属各类规则，对于本协议中以加粗字体显示的内容，您应重点阅读。

攸怡网站有权根据需要不时地制订、修改本协议及/或各类规则，并以网站公示的方式进行公告，不再单独通知您。修订后的协议或将来可能发布或更新的各类规则一经在网站公布后，立即自动生效。如您不同意相关修订，应当立即停止使用攸怡网站服务。您继续使用攸怡网站服务，即表示您接受经修订的协议或规则。
        </textarea>
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
    <script type="text/javascript">

        $(function () {

            $("#welcomeContent").css({ "display": "none" });

            if (YoeJoy.Site.Cookie.GetCookieValue("name") == null || YoeJoy.Site.Cookie.GetCookieValue("name") == undefined) {
            }
            else {
                $("#txtID").val(YoeJoy.Site.Cookie.GetCookieValue("name").toString());
            }

            var from = YoeJoy.Site.Utility.GetQueryString("from");

            //QQ 登录
            QC.Login({
                btnId: "qqLoginBtn"	//插入按钮的节点id
            }, function (reqData, opts) {
            }, function (reqData,opts) { });

            //刷新验证码
            $("#btnRefreshCaptcha").click(function (event) {
                $("#imgCaptcha").removeAttr("src");
                $("#imgCaptcha").attr({ "src": "../Service/Captcha.aspx?random=" + Math.random() });
            });

            //用户登录
            $("#loginTab #btnLogin").click(function () {
                var registerHandlerURL = "../Service/UserLogin.aspx";
                var name = $("#txtID").val();
                var password = $("#txtPass").val();
                var extern = "empty";

                var IsCbRememberNameClicked = $("#cbRememberName").attr("checked") == undefined ? false : true;
                var IsCbAutoLogineClicked = $("#cbAutoLogin").attr("checked") == undefined ? false : true;

                if (IsCbAutoLogineClicked) {
                    extern = "autoLogin";
                    IsCbRememberNameClicked = false;
                }

                if (IsCbRememberNameClicked) {
                    YoeJoy.Site.Cookie.AddCookie("name", name, 7 * 24);
                }

                $.post(registerHandlerURL, { "name": name, "pass": password, "extern": extern }, function (data) {
                    var result = YoeJoy.Site.Utility.GetJsonStr(data);
                    if (result.IsSuccess) {
                        if (from == null || from == undefined || from == '') {
                            window.location.href = "../Default.aspx";
                        }
                        else {
                            window.location.href = from;
                        }
                    }
                    else {
                        alert(result.Msg);
                    }
                });

            });

            //用户注册
            $("#registerTable #btnRegister").click(function () {
                var registerHandlerURL = "../Service/UserRegister.aspx";
                var name = $("#txtRegisterName").val();
                var pass1 = $("#password").val();
                var pass2 = $("#password1").val();
                var email = $("#txtEmail").val();

                var captchaInput = $("#txtCaptcha").val();

                var captcha = YoeJoy.Site.Cookie.GetCookieValue("captcha").toString();

                if (captchaInput == captcha) {

                    $.post(registerHandlerURL, { "name": name, "pass1": pass1, "pass2": pass2, "email": email }, function (data) {
                        var result = YoeJoy.Site.Utility.GetJsonStr(data);
                        if (result.IsSuccess) {
                            window.location.href = "MyProfile.aspx";
                        }
                        else {
                            alert(result.Msg);
                        }
                    });
                }
                else {
                    alert("请检查你的验证码!");
                }

            });

        });
    </script>
</asp:Content>
