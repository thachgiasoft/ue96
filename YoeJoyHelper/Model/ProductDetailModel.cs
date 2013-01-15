using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;

namespace YoeJoyHelper.Model
{
    /// <summary>
    /// 商品详细模型类
    /// </summary>
    public class ProductDetailModel
    {
        public string ProductID { get; set; }
        public string SysNo { get; set; }
        public string ProductBriefName { get; set; }
        public string ProductBaiscPrice { get; set; }
        public string ProductCurrentPrice { get; set; }
        public string PromotionWord { get; set; }
        //详细说明
        public string ProductDescriptionLong { get; set; }
        //包装清单
        public string PackageList { get; set; }
        //一次限购数量
        public int LimitedQty { get; set; }
        //商品图片
        public List<ProductDetailImg> Images { get; set; }
        //规格参数
        public string ProductAttrSummery { get; set; }
        //积分
        public int Point { get; set; }
    }

    /// <summary>
    /// 商品详细图片展示
    /// </summary>
    public class ProductDetailImg
    {
        public string LargeImg { get; set; }
        public string ThumbnailImg { get; set; }
    }

    #region 商品详细信息服务类

    /// <summary>
    /// 商品详细基础服务类
    /// </summary>
    public class ProductDetailBasicService
    {

        private static readonly string getProductImgsSqlCmdTemplate = @"select pimg.product_limg,pimg.product_simg,
            pimg.orderNum,pimg.status from Product_Images pimg
            where pimg.product_sysNo={0} and pimg.status=1 and pimg.orderNum<>1 order by pimg.orderNum";

        private static readonly string getProductDetailInfoSqlCmdTemplate = @"select p.SysNo,p.BriefName,p.PromotionWord,
  CONVERT(float,pp.BasicPrice) as BasicPrice,CONVERT(float,pp.CurrentPrice) as CurrentPrice,
  p.ProductDescLong,p.PackageList,pp.LimitedQty, pas.SummaryMain,pp.Point,p.ProductID from Product p 
  left join Product_Price pp on p.SysNo=pp.ProductSysNo
  left join Inventory inve on p.SysNo=inve.ProductSysNo
  left join Product_Attribute2_Summary pas on p.SysNo=pas.ProductSysNo
  where p.SysNo={0}";

        /// <summary>
        /// 获得商品的全部展示图片
        /// </summary>
        /// <param name="productSysNo"></param>
        /// <returns></returns>
        public static List<ProductDetailImg> GetProductDetailImgs(int productSysNo)
        {
            try
            {
                string sqlCmd = String.Format(getProductImgsSqlCmdTemplate, productSysNo);
                DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {
                    List<ProductDetailImg> imgs = new List<ProductDetailImg>();
                    for (int i = 0; i < rowCount; i++)
                    {
                        imgs.Add(new ProductDetailImg()
                        {
                            ThumbnailImg = data.Rows[i]["product_simg"].ToString().Trim(),
                            LargeImg = data.Rows[i]["product_limg"].ToString().Trim(),
                        });
                    }
                    return imgs;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获得商品详细的基本信息
        /// </summary>
        /// <param name="productSysNo"></param>
        /// <returns></returns>
        public static ProductDetailModel GetProductDetailBasicInfo(int productSysNo)
        {
            string sqlCmd = String.Format(getProductDetailInfoSqlCmdTemplate, productSysNo);
            try
            {
                DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {
                    ProductDetailModel productDetail = new ProductDetailModel()
                    {
                        ProductID = data.Rows[0]["ProductID"].ToString().Trim(),
                        SysNo = data.Rows[0]["SysNo"].ToString().Trim(),
                        ProductBriefName = data.Rows[0]["BriefName"].ToString().Trim(),
                        ProductBaiscPrice = data.Rows[0]["BasicPrice"].ToString().Trim(),
                        ProductCurrentPrice = data.Rows[0]["CurrentPrice"].ToString().Trim(),
                        ProductDescriptionLong = data.Rows[0]["ProductDescLong"].ToString().Trim(),
                        PackageList = data.Rows[0]["PackageList"].ToString().Trim(),
                        LimitedQty = int.Parse(data.Rows[0]["LimitedQty"].ToString().Trim()),
                        PromotionWord = data.Rows[0]["PromotionWord"].ToString().Trim(),
                        Images = GetProductDetailImgs(productSysNo),
                        ProductAttrSummery = data.Rows[0]["SummaryMain"].ToString().Trim(),
                        Point = int.Parse(data.Rows[0]["Point"].ToString().Trim()),
                    };
                    return productDetail;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

    }

    #endregion





}
