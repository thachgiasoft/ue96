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
    public partial class GetBrandProductList1Item : System.Web.UI.Page
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

        protected int BrandID
        {
            get
            {
                if (Request.QueryString["bid"] == null)
                {
                    return 0;
                }
                else
                {
                    return int.Parse(Request.QueryString["bid"].ToString());
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
                Response.Write(BrandProductHelper.GetBrandProductList1HTML(OrderTag, StartIndex, BrandID, Order));
            }
        }
    }
}