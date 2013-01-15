using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YoeJoyHelper.Model;

namespace YoeJoyHelper
{
    public class ProductDetailHelper
    {

        private ProductDetailModel productDetail;

        private ProductDetailHelper()
        {
            throw new NotImplementedException("禁止对ProductDetailHelper类声明无参的构造函数");
        }

        public ProductDetailHelper(int productSysNo)
        {
            string key = String.Format(DynomicCacheObjSettings.ProductBaiscInfoCacheSettings.CacheKey, productSysNo);
            int duration = DynomicCacheObjSettings.ProductBaiscInfoCacheSettings.CacheDuration;
            productDetail = CacheObj<ProductDetailModel>.GetCachedObj(key, duration, ProductDetailBasicService.GetProductDetailBasicInfo(productSysNo));
            //productDetail = ProductDetailBasicService.GetProductDetailBasicInfo(productSysNo);
        }

        /// <summary>
        /// 获取商品图片展示
        /// </summary>
        /// <returns></returns>
        public string GetProductDetailImages()
        {
            string productImgHTML = String.Empty;
            if (productDetail != null && productDetail.Images != null)
            {
                List<ProductDetailImg> imgs = productDetail.Images;
                string imgVirtualPathBase = YoeJoyConfig.ImgVirtualPathBase;

                StringBuilder strb = new StringBuilder("<ul class='clear'>");

                string imgItemHTMLTemplate = @"<li><a href='javascript:void(0)'>
                                    <img alt='样品缩略图' src='{0}'></a></li>";


                foreach (ProductDetailImg img in imgs)
                {
                    strb.Append(String.Format(imgItemHTMLTemplate, String.Concat(imgVirtualPathBase, img.LargeImg)));
                }

                strb.Append("</ul>");
                productImgHTML = strb.ToString();
            }

            return productImgHTML;
        }

        /// <summary>
        /// 获取商品名称
        /// </summary>
        /// <returns></returns>
        public string GetProductNameInfo()
        {
            if (productDetail != null)
            {
                return String.Format(@"<h1>{0}<span>{1}</span></h1><input type='hidden' id='productBriefName' value='{2}'/>", productDetail.ProductBriefName, productDetail.PromotionWord, productDetail.ProductBriefName);
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// 商品基本信息
        /// </summary>
        /// <returns></returns>
        public string GetProductBaiscInfo()
        {
            if (productDetail != null)
            {
                return String.Format(@"<div class='buyInfo'>
                    <p>
                        商品编号：{0}</p>
                    <p>
                        商品评论：共<a class='link1' href='#'>{1}</a>条</p>
                    <hr size='0'>
                    <p>
                        市场价：<span class='yprice'>¥{2}</span></p>
                    <p>
                        攸怡价：¥<span class='yyj'>{3}</span>&nbsp;&nbsp;节省了{4}元&nbsp;&nbsp;&nbsp;<a class='link1'
                            href='javascript:void(0);'>价格举报</a></p>
                    <p>
                        积分：{5}分</p>
                    <hr size='0'>
                    <p id='inventoryStatus'>
                        库存状况：有货</p>
                    <p>
                        <a class='link1' href='help.html'>运费说明</a></p>
                    <div class='buyApp'>
                        <h6 class='attrib'>
                            颜色：
                            <label class='selected'>
                                黑色</label>
                            <label>
                                白色</label>
                        </h6>
                        <h6 class='selected'>
                            已选择：<span class='mast0'>黑色</span>
                        </h6>
                        <div>
                             <input id='limitQty' value='{6}' type='hidden'/>
                            <h6>
                                我要买：<a class='sub' href='javascript:void(0)'>-</a>
                                <input class='num' maxlength='3' value='1' type='text'>
                                <a class='add' href='javascript:void(0)'>+</a>个</h6>
                            <h5>
                                <a class='bt5' href='../Shopping/ShoppingCart.aspx?Cmd=Add&ItemID={7}'>立即购买</a> <a id='btnAddToCart' class='bt7' href='javascript:void(0);'>
                                    加入购物车</a> <a class='link1' href='javascript:void(0);'>加入收藏</a></h5>
                        </div>
                    </div>
                </div>", productDetail.ProductID, "0",
                       productDetail.ProductBaiscPrice, productDetail.ProductCurrentPrice,
                       ((float.Parse(productDetail.ProductBaiscPrice) - (float.Parse(productDetail.ProductCurrentPrice)))).ToString("0.00"),
                       productDetail.Point, productDetail.LimitedQty,productDetail.SysNo);
            }
            else
            {
                return String.Empty;
            }
        }
        /// <summary>
        /// 商品介绍
        /// </summary>
        /// <returns></returns>
        public string GetProductLongDescription()
        {
            if (productDetail != null)
            {
                return productDetail.ProductDescriptionLong;
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// 商品包装清单
        /// </summary>
        /// <returns></returns>
        public string GetProductPackageList()
        {
            if (productDetail != null)
            {
                return productDetail.PackageList;
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// 商品参数
        /// </summary>
        /// <returns></returns>
        public string GetProductAttrSummery()
        {
            if (productDetail != null)
            {
                return productDetail.ProductAttrSummery;
            }
            else
            {
                return String.Empty;
            }
        }
    }
}
