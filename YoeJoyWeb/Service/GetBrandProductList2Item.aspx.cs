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
    public partial class GetBrandProductList2Item : System.Web.UI.Page
	{

        protected int C1CategorySysId
        {
            get
            {
                if (Request.QueryString["c1"] == null)
                {
                    return 0;
                }
                else
                {
                    return int.Parse(Request.QueryString["c1"].ToString().Trim());
                }
            }
        }

        protected int C2CategorySysId
        {
            get
            {
                if (Request.QueryString["c2"] == null)
                {
                    return 0;
                }
                else
                {
                    return int.Parse(Request.QueryString["c2"].ToString().Trim());
                }
            }
        }

        protected int C3CategorySysId
        {
            get
            {
                if (Request.QueryString["c3"] == null)
                {
                    return 0;
                }
                else
                {
                    return int.Parse(Request.QueryString["c3"].ToString().Trim());
                }
            }
        }

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

        protected string Attribution2Ids
        {
            get
            {
                if (Request.QueryString["attrIds"] == null || String.IsNullOrEmpty(Request.QueryString["attrIds"].ToString()))
                {
                    return null;
                }
                else
                {
                    return Request.QueryString["attrIds"].ToString();
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

		protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                Response.Write(BrandProductHelper.GetBrandProductList2HTML(OrderTag, StartIndex, C3CategorySysId, C1CategorySysId, C2CategorySysId, Attribution2Ids, BrandID,Order));
            }
		}
	}
}