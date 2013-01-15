using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YoeJoyHelper;
using YoeJoyHelper.Extension;

namespace YoeJoyWeb
{
    public partial class GetSearch1ProductListItem : System.Web.UI.Page
    {

        protected int StartIndex
        {
            get
            {
                if (Request.QueryString["startIndex"] == null)
                {
                    return 0;
                }
                else
                {
                    return int.Parse(Request.QueryString["startIndex"].ToString().Trim());
                }
            }
        }

        protected YoeJoyEnum.ProductListSortedOrder OrderTag
        {
            get
            {
                if (Request.QueryString["orderBy"] == null)
                {
                    return YoeJoyEnum.ProductListSortedOrder.Default;
                }
                else
                {
                    return (YoeJoyEnum.ProductListSortedOrder)int.Parse(Request.QueryString["orderBy"].ToString().Trim());
                }
            }
        }

        protected string KeyWords
        {
            get
            {
                if (Request.QueryString["q"] == null)
                {
                    return String.Empty;
                }
                else
                {
                    return Request.QueryString["q"].ToString().GetUrlDecodeStr();
                }
            }
        }

        protected string Order
        {
            get
            {
                if (Request.QueryString["order"] == null)
                {
                    return "DESC";
                }
                else
                {
                    return Request.QueryString["order"].ToString().Trim();
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Response.Write(SearchHelper.GetSearch1ProductListHTML(OrderTag, StartIndex, KeyWords, Order));
            }
        }
    }
}