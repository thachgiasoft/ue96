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
    public partial class SearchResult2 : System.Web.UI.Page
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

        protected string C3ProductListFooterHTML { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                C3ProductListFooterHTML = SearchHelper.InitSearch2C3ProductListFooter(C3CategorySysId, Attribution2Ids, KeyWords);
            }
        }
    }
}