using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Globalization;
using YoeJoyHelper;

namespace YoeJoyWeb
{
    /// <summary>
    /// 请不要移除这张页面
    /// 这张页面是给后台程序使用
    /// word paser控件上传图片使用
    /// 考虑到网站的结构和路径问题
    /// 直接放在了主页目录下
    /// </summary>
    public partial class upload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string fname = Request.Form["UserName"];
            int len = Request.ContentLength;

            if (Request.Files.Count > 0)
            {
                DateTime timeNow = DateTime.Now;
                string uploadPath = "upload/" + timeNow.ToString("yyyyMM") + "/" + timeNow.ToString("dd") + "/";

                string folder = YoeJoyConfig.ImgPhysicalPathBase + "/products/" + uploadPath;
                //自动创建目录
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                HttpPostedFile file = Request.Files.Get(0);
                string ext = Path.GetExtension(file.FileName).ToLower();
                //只支持图片上传
                if (ext == ".jpg"
                    || ext == ".png"
                    || ext == ".gif"
                    || ext == ".bmp")
                {
                    //在这里我们将会根据时间重新生成一个名称
                    string time = DateTime.Now.ToString("HHmmssffff") + ext;
                    time = Guid.NewGuid() + time;
                    string filePath = Path.Combine(folder, time);

                    file.SaveAs(filePath);
                    Response.Write(YoeJoyConfig.ImgVirtualPathBase+uploadPath + time);
                }
            }
        }
    }
}
