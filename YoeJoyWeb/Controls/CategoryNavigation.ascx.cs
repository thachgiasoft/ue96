using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YoeJoyHelper.Model;
using YoeJoyHelper;

namespace YoeJoyWeb.Controls
{
    public partial class CategoryNav : System.Web.UI.UserControl
    {
        protected string CategoryNavHTML { get; set; }

        public bool IsHomePage { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            string categoryMiddleContent = new CategoryHelper().InitCategoryNavigation();
            if (IsHomePage)
            {
                CategoryNavHTML = String.Format("<div id='Menu'>{0}</div>", categoryMiddleContent);
            }
            else
            {
                CategoryNavHTML = String.Format("<div id='ItemSort'> <span>全部商品分类<b>+</b></span> </div><div id='Menu' class='classMenu'>{0}</div>", categoryMiddleContent);
            }
        }
    }
}