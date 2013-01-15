using System;
using System.Collections.Generic;
using System.Text;

namespace Icson.Utils
{
    public class SpecialWindow
    {

        /// <summary>
        /// 显示消息提示对话框
        /// </summary>
        /// <param name="page">当前页面指针，一般为"this"</param>
        /// <param name="msg">提示信息</param>
        public static void ShowAlert(System.Web.UI.Page page, string msg)
        {
            System.Text.StringBuilder Builder = new System.Text.StringBuilder();
            Builder.Append("<script language='javascript' defer>");
            Builder.AppendFormat("alert('{0}');", msg);
            Builder.Append("</script>");
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", Builder.ToString());
        }
        /// <summary>
        /// 打开大小不可变模式窗口
        /// </summary>
        /// <param name="page">当前页面指针，一般为"this"</param>
        /// <param name="PageUrl">打开的模式窗口显示的网页地址</param>
        /// <param name="Width">打开的模式窗口的宽度</param>
        /// <param name="Height">打开的模式窗口的高度</param>
        public static void OpenFixModalDialog(System.Web.UI.Page page, String PageUrl, int Width, int Height)
        {
            System.Text.StringBuilder Builder = new System.Text.StringBuilder();
            Builder.Append("<script language='javascript' defer>");
            Builder.AppendFormat("window.showModalDialog('{0}',null,'dialogWidth:{1}px;dialogHeight:{2}px;help:no;unadorned:no;resizable:no;status:no');", PageUrl, Width, Height);
            Builder.Append("</script>");
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", Builder.ToString());

        }
        /// <summary>
        /// 打开大小可变模式窗口
        /// </summary>
        /// <param name="page">当前页面指针，一般为"this"</param>
        /// <param name="PageUrl">打开的模式窗口显示的网页地址</param>
        /// <param name="Width">打开的模式窗口的宽度</param>
        /// <param name="Height">打开的模式窗口的高度</param>
        public static void OpenSizeableModalDialog(System.Web.UI.Page page, String PageUrl, int Width, int Height)
        {
            System.Text.StringBuilder Builder = new System.Text.StringBuilder();
            Builder.Append("<script language='javascript' defer>");
            Builder.AppendFormat("window.showModalDialog('{0}',null,'dialogWidth:{1}px;dialogHeight:{2}px;help:no;unadorned:no;resizable:yes;status:no');", PageUrl, Width, Height);
            Builder.Append("</script>");
            //page.ClientScript.RegisterStartupScript(page.GetType(), "message", Builder.ToString());
            page.Response.Write(Builder.ToString());
        }
        /// <summary>
        /// 打开悬浮提示窗口
        /// </summary>
        /// <param name="page">页面指针 一般输入"this"</param>
        /// <param name="message">显示的消息</param>
        /// <param name="Width">窗口宽度</param>
        /// <param name="height">窗口高度</param>
        public static void OpenFloatDialog(System.Web.UI.Page page, string message, int Width, int height)
        {
            System.Text.StringBuilder Builder = new System.Text.StringBuilder();
            Builder.Append("<script type='text/javascript' language='javascript' defer>");
            //   Builder.Append("var msgw,msgh,bordercolor; ");
            Builder.AppendLine("function ShowBDDialog(){ ");
            Builder.AppendLine("bordercolor='#66ccff';titlecolor='#99CCFF';");
            Builder.AppendLine("var sWidth,sHeight; sWidth=document.body.offsetWidth; sHeight=document.body.offsetHeight;");
            Builder.AppendLine("var bgObj=document.createElement('div'); ");
            Builder.AppendLine(" bgObj.setAttribute('id','bgDiv'); ");
            Builder.AppendLine("bgObj.style.position='absolute'; ");
            Builder.AppendLine("bgObj.style.top='0'; bgObj.style.background='#dcdcdc';");
            Builder.AppendLine("bgObj.style.filter='progid:DXImageTransform.Microsoft.Alpha(style=3,opacity=25,finishOpacity=75';");
            Builder.AppendLine("bgObj.style.opacity='0.6'; ");
            Builder.AppendLine("bgObj.style.left='0';");
            Builder.AppendLine("bgObj.style.width=sWidth + 'px'; ");
            Builder.AppendLine("bgObj.style.height=sHeight + 'px';");
            Builder.AppendLine("document.body.appendChild(bgObj); ");
            Builder.AppendLine("var msgObj=document.createElement('div')");
            Builder.AppendLine("msgObj.setAttribute('id','msgDiv');");
            Builder.AppendLine("msgObj.setAttribute('align','center');");
            Builder.AppendLine("msgObj.style.position='absolute';msgObj.style.background='white'; ");
            Builder.AppendLine("msgObj.style.font='12px/1.6em Verdana, Geneva, Arial, Helvetica, sans-serif';");
            Builder.AppendLine("msgObj.style.border='1px solid ' + bordercolor;");
            Builder.AppendFormat("msgObj.style.width='{0} '+ 'px'; ", Width);
            Builder.AppendFormat("msgObj.style.height='{0}' + 'px';", height);
            Builder.AppendFormat("msgObj.style.top=(document.documentElement.scrollTop + (sHeight-'{0}')/2) + 'px';", height);
            Builder.AppendFormat("msgObj.style.left=(sWidth-'{0}')/2 + 'px';", Width);
            Builder.AppendLine("var title=document.createElement('h4');");
            Builder.AppendLine("title.setAttribute('id','msgTitle');");
            Builder.AppendLine("title.setAttribute('align','right');");
            Builder.AppendLine("title.style.margin='0'; ");
            Builder.AppendLine("title.style.padding='3px'; title.style.background=bordercolor; ");
            Builder.AppendLine("title.style.filter='progid:DXImageTransform.Microsoft.Alpha(startX=20, startY=20, finishX=100, finishY=100,style=1,opacity=75,finishOpacity=100);'; ");
            Builder.AppendLine("title.style.opacity='0.75'; ");
            Builder.AppendLine("title.style.border='1px solid ' + bordercolor;title.innerHTML='<a style=font-size:small href=#>关闭</a>'; ");
            Builder.AppendLine("title.onclick=function(){ document.body.removeChild(bgObj);document.getElementById('msgDiv').removeChild(title); document.body.removeChild(msgObj);} ");
            Builder.AppendLine("document.body.appendChild(msgObj); ");
            Builder.AppendLine("document.getElementById('msgDiv').appendChild(title);");
            Builder.AppendLine("var txt=document.createElement('p');");
            Builder.AppendFormat("txt.style.height='{0}';", height);
            Builder.AppendFormat("txt.style.width='{0}';", Width);
            Builder.AppendLine(" txt.style.margin='1em 0' ");
            Builder.AppendLine("txt.setAttribute('id','msgTxt');");
            Builder.AppendFormat("txt.innerHTML='{0}'; ", message);
            Builder.AppendLine("document.getElementById('msgDiv').appendChild(txt);return false;}");
            Builder.AppendLine(" ShowBDDialog(); </script>");
            page.Response.Write(Builder.ToString());
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", "<script language='javscript'>ShowBDDialog();</" + "script>");
        }
        /// <summary>
        /// 打开悬浮弹出窗口
        /// </summary>
        /// <param name="page">页面指针 一般输入"this"</param>
        /// <param name="url">打开的页面的url</param>
        /// <param name="Width">窗口宽度</param>
        /// <param name="height">窗口高度</param>
        public static void OpenFloatModalWindow(System.Web.UI.Page page, string url, int Width, int height)
        {
            System.Text.StringBuilder Builder = new System.Text.StringBuilder();
            Builder.Append("<script type='text/javascript' language='javascript' defer>");
            //   Builder.Append("var msgw,msgh,bordercolor; ");
            Builder.AppendLine("function ShowBDDialog(){ ");
            Builder.AppendLine("bordercolor='#66ccff';titlecolor='#99CCFF';");
            Builder.AppendLine("var sWidth,sHeight; sWidth=document.body.offsetWidth; sHeight=document.body.offsetHeight;");
            Builder.AppendLine("var bgObj=document.createElement('div'); ");
            Builder.AppendLine(" bgObj.setAttribute('id','bgDiv'); ");
            Builder.AppendLine("bgObj.style.position='absolute'; ");
            Builder.AppendLine("bgObj.style.top='0'; bgObj.style.background='#dcdcdc';");
            Builder.AppendLine("bgObj.style.filter='progid:DXImageTransform.Microsoft.Alpha(style=3,opacity=25,finishOpacity=75';");
            Builder.AppendLine("bgObj.style.opacity='0.6'; ");
            Builder.AppendLine("bgObj.style.left='0';");
            Builder.AppendLine("bgObj.style.width=sWidth + 'px'; ");
            Builder.AppendLine("bgObj.style.height=sHeight + 'px';");
            Builder.AppendLine("document.body.appendChild(bgObj); ");
            Builder.AppendLine("var msgObj=document.createElement('div')");
            Builder.AppendLine("msgObj.setAttribute('id','msgDiv');");
            Builder.AppendLine("msgObj.setAttribute('align','center');");
            Builder.AppendLine("msgObj.style.position='absolute';msgObj.style.background='white'; ");
            Builder.AppendLine("msgObj.style.font='12px/1.6em Verdana, Geneva, Arial, Helvetica, sans-serif';");
            Builder.AppendLine("msgObj.style.border='1px solid ' + bordercolor;");
            Builder.AppendFormat("msgObj.style.width='{0} '+ 'px'; ", Width);
            Builder.AppendFormat("msgObj.style.height='{0}' + 'px';", height);
            Builder.AppendFormat("msgObj.style.top=(document.documentElement.scrollTop + (sHeight-'{0}')/2) + 'px';", height);
            Builder.AppendFormat("msgObj.style.left=(sWidth-'{0}')/2 + 'px';", Width);
            Builder.AppendLine("var title=document.createElement('h4');");
            Builder.AppendLine("title.setAttribute('id','msgTitle');");
            Builder.AppendLine("title.setAttribute('align','right');");
            Builder.AppendLine("title.style.margin='0'; ");
            Builder.AppendLine("title.style.padding='3px'; title.style.background=bordercolor; ");
            Builder.AppendLine("title.style.filter='progid:DXImageTransform.Microsoft.Alpha(startX=20, startY=20, finishX=100, finishY=100,style=1,opacity=75,finishOpacity=100);'; ");
            Builder.AppendLine("title.style.opacity='0.75'; ");
            Builder.AppendLine("title.style.border='1px solid ' + bordercolor;title.innerHTML='<a style=font-size:small href=#>关闭</a>'; ");
            Builder.AppendLine("title.onclick=function(){ document.body.removeChild(bgObj);document.getElementById('msgDiv').removeChild(title); document.body.removeChild(msgObj);} ");
            Builder.AppendLine("document.body.appendChild(msgObj); ");
            Builder.AppendLine("document.getElementById('msgDiv').appendChild(title);");
            Builder.AppendLine("var txt=document.createElement('iframe');");
            Builder.AppendFormat("txt.style.height='{0}';", height);
            Builder.AppendFormat("txt.style.width='{0}';", Width);
            Builder.AppendLine(" txt.style.margin='1em 0' ");
            Builder.AppendLine("txt.setAttribute('id','msgTxt');");
            Builder.AppendFormat("txt.src='{0}'; ", url);
            Builder.AppendLine("document.getElementById('msgDiv').appendChild(txt);return false;}");
            Builder.AppendLine(" ShowBDDialog(); </script>");
            page.Response.Write(Builder.ToString());

            page.ClientScript.RegisterStartupScript(page.GetType(), "message", "<script language='javscript'>ShowBDDialog();</" + "script>");
        }

        public static string pp(System.Web.UI.Page page, string url, int Width, int height)
        {
            System.Text.StringBuilder Builder = new System.Text.StringBuilder();
            Builder.Append("<script type='text/javascript' language='javascript' defer>");
            //   Builder.Append("var msgw,msgh,bordercolor; ");
            Builder.AppendLine("function ShowBDDialog(){ ");
            Builder.AppendLine("bordercolor='#66ccff';titlecolor='#99CCFF';");
            Builder.AppendLine("var sWidth,sHeight; sWidth=document.body.offsetWidth; sHeight=document.body.offsetHeight;");
            Builder.AppendLine("var bgObj=document.createElement('div'); ");
            Builder.AppendLine(" bgObj.setAttribute('id','bgDiv'); ");
            Builder.AppendLine("bgObj.style.position='absolute'; ");
            Builder.AppendLine("bgObj.style.top='0'; bgObj.style.background='#dcdcdc';");
            Builder.AppendLine("bgObj.style.filter='progid:DXImageTransform.Microsoft.Alpha(style=3,opacity=25,finishOpacity=75';");
            Builder.AppendLine("bgObj.style.opacity='0.6'; ");
            Builder.AppendLine("bgObj.style.left='0';");
            Builder.AppendLine("bgObj.style.width=sWidth + 'px'; ");
            Builder.AppendLine("bgObj.style.height=sHeight + 'px';");
            Builder.AppendLine("document.body.appendChild(bgObj); ");
            Builder.AppendLine("var msgObj=document.createElement('div')");
            Builder.AppendLine("msgObj.setAttribute('id','msgDiv');");
            Builder.AppendLine("msgObj.setAttribute('align','center');");
            Builder.AppendLine("msgObj.style.position='absolute';msgObj.style.background='white'; ");
            Builder.AppendLine("msgObj.style.font='12px/1.6em Verdana, Geneva, Arial, Helvetica, sans-serif';");
            Builder.AppendLine("msgObj.style.border='1px solid ' + bordercolor;");
            Builder.AppendFormat("msgObj.style.width='{0} '+ 'px'; ", Width);
            Builder.AppendFormat("msgObj.style.height='{0}' + 'px';", height);
            Builder.AppendFormat("msgObj.style.top=(document.documentElement.scrollTop + (sHeight-'{0}')/2) + 'px';", height);
            Builder.AppendFormat("msgObj.style.left=(sWidth-'{0}')/2 + 'px';", Width);
            Builder.AppendLine("var title=document.createElement('h4');");
            Builder.AppendLine("title.setAttribute('id','msgTitle');");
            Builder.AppendLine("title.setAttribute('align','right');");
            Builder.AppendLine("title.style.margin='0'; ");
            Builder.AppendLine("title.style.padding='3px'; title.style.background=bordercolor; ");
            Builder.AppendLine("title.style.filter='progid:DXImageTransform.Microsoft.Alpha(startX=20, startY=20, finishX=100, finishY=100,style=1,opacity=75,finishOpacity=100);'; ");
            Builder.AppendLine("title.style.opacity='0.75'; ");
            Builder.AppendLine("title.style.border='1px solid ' + bordercolor;title.innerHTML='<a style=font-size:small href=#>关闭</a>'; ");
            Builder.AppendLine("title.onclick=function(){ document.body.removeChild(bgObj);document.getElementById('msgDiv').removeChild(title); document.body.removeChild(msgObj);} ");
            Builder.AppendLine("document.body.appendChild(msgObj); ");
            Builder.AppendLine("document.getElementById('msgDiv').appendChild(title);");
            Builder.AppendLine("var txt=document.createElement('iframe');");
            Builder.AppendFormat("txt.style.height='{0}';", height);
            Builder.AppendFormat("txt.style.width='{0}';", Width);
            Builder.AppendLine(" txt.style.margin='1em 0' ");
            Builder.AppendLine("txt.setAttribute('id','msgTxt');");
            Builder.AppendFormat("txt.src='{0}'; ", url);
            Builder.AppendLine("document.getElementById('msgDiv').appendChild(txt);return false;}");
            Builder.AppendLine(" ShowBDDialog(); </script>");
            //Label tx = (Label)page.FindControl("Label1");
            //tx.Text = "xxx";
            return Builder.ToString();
        }


        public static string ShowWindow(string title, int width, int height, string url)
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();


            str.Append("<div id='formDiv' style='display:none;Z-INDEX:100;FILTER:alpha(Opacity=40);LEFT:0px;WIDTH:100%;POSITION:absolute;TOP:0px;HEIGHT:100%;BACKGROUND-COLOR:black'>\n");
            str.Append("<table width='100%' height='100%' cellpadding='0' cellspacing='0' border='0'>\n");
            str.Append("<tr>\n");
            str.Append("<td></td>\n");
            str.Append("</tr>\n");
            str.Append("</table>\n");
            str.Append("</div>\n");

            str.Append("<div id='arltDiv' align='center' style='border-color:#1e4d9d; border-width:1px; border-style:solid; display:none;Z-INDEX: 101;WIDTH: " + width.ToString() + "px; POSITION: absolute;top:10px; HEIGHT: " + height.ToString() + "px'>\n");
            str.Append("<table width='100%' height='100%' border='0' cellpadding='0' cellspacing='0' align='center' >\n");
            str.Append("<tr style='background-image:url(\"../images/datagridstyle/head_bg.gif\");'>\n");
            str.Append("<td align='left' valign='middle' style='width:100px; height:25px; font-size:13px; font-weight:bold; color:#151515; font-family: 宋体'>&nbsp;&nbsp;<b id='title'>" + title + "</b></td>\n");
            str.Append("<td align='right' valign='middle'><input class='window_btn' type='button' value='关闭' style=\"background-image:url('../images_dbc/btn_window.jpg')\" onclick='hideDiv()'>&nbsp;</td>\n");
            str.Append("</tr>\n");
            str.Append("<tr style='background-color:white;height:1px'>\n");
            str.Append("<td colspan='2'></td>\n");
            str.Append("</tr>\n");
            str.Append("<tr>\n");
            str.Append("<td  colspan='2' bgcolor='#ffffff' align='center' width='100%'>\n");
            str.Append("<iframe id='ifUrl' width='100%' height='100%' scrolling='yes' src='" + url + "' frameborder='0' marginheight='0' marginwidth='0' style='border:0px'></iframe>");
            str.Append("</td></tr>\n");

            str.Append("</table>\n");
            str.Append("</div>\n");

            str.Append("<script type='text/javascript'>\n");
            str.Append("var width_c=" + width.ToString() + ";\n");
            str.Append("var height_c=" + height.ToString() + ";\n");
            str.Append("var sw = document.body.offsetWidth;\n");
            str.Append("var sh = document.body.offsetHeight;\n");
            str.Append("var left = (sw - width_c)/2;\n");
            str.Append("var top = (sh - height_c)/2;\n");

            str.Append("document.getElementById('arltDiv').style.top=top;\n");
            str.Append("document.getElementById('arltDiv').style.left=left;\n");

            str.Append("function hideDiv(){\n");
            str.Append("document.getElementById('formDiv').style.display='none';\n");
            str.Append("document.getElementById('arltDiv').style.display='none';\n");
            str.Append("}\n");

            str.Append("function imageUp1(img){\n");
            str.Append("img.src='../Images/b_close_1.gif';");
            str.Append("}\n");

            str.Append("function imageUp2(img){\n");
            str.Append(" img.src='../Images/b_close_2.gif';");
            str.Append("}\n");

            str.Append("</script>\n");
            return str.ToString();

            //HttpContext.Current.Response.Write(str);
        }

    }
}
