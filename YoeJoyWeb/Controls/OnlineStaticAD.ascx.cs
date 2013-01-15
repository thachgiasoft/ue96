using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YoeJoyHelper;

namespace YoeJoyWeb.Controls
{
    /// <summary>
    /// 静态图片链接广告
    /// </summary>
    public partial class OnlineStaticAD : System.Web.UI.UserControl
    {

        protected string StaticADHTML { get; set; }

        public int ADPositionID { get; set; }
        public string ADCSSClass { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                StaticADHTML = ADHelper.GetSiteStaticAdWrapper(ADPositionID, ADCSSClass, Width, Height);
            }
        }
    }
}