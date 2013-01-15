using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YoeJoyHelper;

namespace YoeJoyWeb.Pages
{
    public partial class Product : System.Web.UI.Page
    {

        protected int C1SysNo
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

        protected int C2SysNo
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

        protected int C3SysNo
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

        protected int ProductSysNo
        {
            get
            {
                if (Request.QueryString["pid"] == null)
                {
                    return 0;
                }
                else
                {
                    return int.Parse(Request.QueryString["pid"].ToString().Trim());
                }
            }
        }

        protected string ProductNameInfoHTML { get; set; }
        protected string ProductDetailImagesHTML { get; set; }
        protected string ProductBaiscInfoHTML { get; set; }
        protected string ProductLongDescriptionHTML { get; set; }
        protected string ProductPackageListHTML { get; set; }
        protected string ProductAttrSummeryHTML { get; set; }
        protected string ProductRelatedGuessYouLikeHTML { get; set; }
        protected string ProductAlsoSeenHTML { get; set; }
        protected string ProductAlsoBuyHTML { get; set; }
        protected string BrowserHistoryProductListHTML { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            ((Site)this.Master).IsHomePage = false;
            if (!IsPostBack)
            {
                SubCategoryNavigation1.C1SysNo = C1SysNo;
                ProductDetailHelper helper = new ProductDetailHelper(ProductSysNo);
                ProductDetailImagesHTML = helper.GetProductDetailImages();
                ProductNameInfoHTML = helper.GetProductNameInfo();
                ProductBaiscInfoHTML = helper.GetProductBaiscInfo();
                ProductLongDescriptionHTML = helper.GetProductLongDescription();
                ProductPackageListHTML=helper.GetProductPackageList();
                ProductAttrSummeryHTML = helper.GetProductAttrSummery();
                ProductRelatedGuessYouLikeHTML = FrontProductsHelper.GetProductGuessYouLikeHTML(C1SysNo, C2SysNo, C3SysNo, ProductSysNo);
                ProductAlsoSeenHTML=FrontProductsHelper.GetProductAlsoSeenHTML(C1SysNo, C2SysNo, C3SysNo, ProductSysNo);
                ProductAlsoBuyHTML = FrontProductsHelper.GetProductAlsoBuyInCartCheck(C1SysNo, C2SysNo, C3SysNo, ProductSysNo);
                //添加最近浏览记录
                CustomerHelper.SetCustomerBrowserHistory(ProductSysNo);
                BrowserHistoryProductListHTML = CustomerHelper.GetProductDetailBrowserHistoryProductsHTML();
            }
        }
    }
}