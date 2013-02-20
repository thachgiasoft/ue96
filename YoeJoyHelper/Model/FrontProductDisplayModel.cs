using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;
using Icson.Objects;
using Icson.Utils;
using System.Collections;

namespace YoeJoyHelper.Model
{
    #region 前台商品展示的模型定义
    /// <summary>
    /// 前台展示的商品
    /// </summary>
    public class FrontDsiplayProduct
    {
        public string ProductSysNo { get; set; }
        public int C1SysNo { get; set; }
        public int C2SysNo { get; set; }
        public int C3SysNo { get; set; }
        public string BaiscPrice { get; set; }
        public string Price { get; set; }
        public string ImgPath { get; set; }
        public string ProductPromotionWord { get; set; }
        public string ProductBriefName { get; set; }
        public bool IsCanPurchase { get; set; }
        /// <summary>
        /// 限购数量
        /// </summary>
        public int LimitQty { get; set; }
        /// <summary>
        /// 可用库存
        /// </summary>
        public int AvailableQty { get; set; }
        /// <summary>
        /// 商品重量
        /// </summary>
        public float Weight { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int Point { get; set; }
    }

    /// <summary>
    /// 前台展示的清库商品
    /// </summary>
    public class EmptyInventoryProduct
    {
        public string ProductSysNo { get; set; }
        public int C2SysNo { get; set; }
        public int C3SysNo { get; set; }
        public string Price { get; set; }
        public string ProductPromotionWord { get; set; }
        public string ProductBriefName { get; set; }
        public string BaiscPrice { get; set; }
    }

    /// <summary>
    /// 大类页面最新降价商品
    /// </summary>
    public class C1LastestDiscountProduct
    {
        public string ProductSysNo { get; set; }
        public int C2SysNo { get; set; }
        public int C3SysNo { get; set; }
        public string Price { get; set; }
        public string ProductPromotionWord { get; set; }
        public string ImgPath { get; set; }
        public string DiscountRate { get; set; }
        public string ProductBriefName { get; set; }
        public string BaiscPrice { get; set; }
    }

    /// <summary>
    /// 大类每周销量排行商品
    /// </summary>
    public class C1WeeklyBestSaledProduct
    {
        public string ProductSysNo { get; set; }
        public int C2SysNo { get; set; }
        public int C3SysNo { get; set; }
        public string Price { get; set; }
        public string ProductPromotionWord { get; set; }
        public string ImgPath { get; set; }
        public string ProductBriefName { get; set; }
        public string BaiscPrice { get; set; }
    }

    /// <summary>
    /// 商品Filter选项
    /// </summary>
    public struct C3ProductAttributionOption
    {
        public int OptionSysNo;
        public string OptionName;
    }

    /// <summary>
    /// 商品Filter
    /// </summary>
    public class C3ProductAttribution
    {
        public int A1SysNo { get; set; }
        public int A2SysNo { get; set; }
        public string A2Name { get; set; }
        public List<C3ProductAttributionOption> Options { get; set; }
    }


    /// <summary>
    /// 相关搜索
    /// </summary>
    public class Research
    {
        public string rulest1 { get; set; }
        public string rulest2 { get; set; }
        public string rulest3 { get; set; }
        public string rulest4 { get; set; }
    }
    /// <summary>
    /// Serach1初值的分类
    /// </summary>
    public class C3ProductSerach1Filter
    {
        public int C1SysNo { get; set; }
        public int C2SysNo { get; set; }
        public int C3SysNo { get; set; }
        public string C3Name { get; set; }
    }

    /// <summary>
    /// 用户浏览记录中的商品
    /// </summary>
    public class CustomerBrowserHistoryProduct
    {
        public string ProductSysNo { get; set; }
        public int C1SysNo { get; set; }
        public int C2SysNo { get; set; }
        public int C3SysNo { get; set; }
        public string ProductBriefName { get; set; }
        public string LargeImg { get; set; }
        public string SmallImg { get; set; }
        public string CurrentPrice { get; set; }
        public string StandardPrice { get; set; }
        public string PromotionWord { get; set; }
    }

    /// <summary>
    /// 首页推荐品牌商品
    /// </summary>
    public class HomePromotionBrandsC3Info
    {
        public string C3Name { get; set; }
        public int C1SysNo { get; set; }
        public int C2SysNo { get; set; }
        public int C3SysNo { get; set; }
    }

    #endregion

    #region 前台商品展示的模型服务
    public class HomeCategoryOneProductService
    {
        private static readonly string GetCategoryTwoIDsSqlCmdTemplate = @"select top 5 c1c2.C2SysNo,c2.C2Name 
  from OnlineC1_C2 c1c2 left join Category2 c2 on c1c2.C2SysNo=c2.SysNo
  where c2.C1SysNo={0} 
  order by c1c2.OrderNum";

        private static readonly string GetCategoryTwoProductsSqlCmdTemplate = @"select op.ProductSysNo,op.ProductBriefName,op.OrderNum,CONVERT(float,pp.CurrentPrice)as Price,pimg.product_limg,p.C1SysNo,p.C2SysNo,p.C3SysNo,p.PromotionWord,p.BriefName,CONVERT(float,pp.BasicPrice) as basicPrice from OnlineC1_Product as op 
  left join Product as p on op.ProductSysNo=p.SysNo
  left join Product_Images pimg on op.ProductSysNo=pimg.product_sysNo
  left join Product_Price pp on op.ProductSysNo=pp.ProductSysNo
  left join Inventory inv on op.ProductSysNo=inv.ProductSysNo
  where
  (inv.AvailableQty+inv.VirtualQty>0) and
  pimg.orderNum=1 and 
  p.Status=1 and
  (pp.ClearanceSale=1 or pp.currentprice>=IsNull(pp.unitcost,0)) and
  p.C2SysNo={0}
  order by op.OrderNum";

        /// <summary>
        /// 获取前台指定的大类商品中
        /// 需要在首页展示的所有小类的ID和名称
        /// </summary>
        /// <param name="categoryOneId"></param>
        /// <returns></returns>
        public static Dictionary<int, string> GetCategoryTwoIDs(string categoryOneId)
        {
            try
            {
                string sqlCmd = String.Format(GetCategoryTwoIDsSqlCmdTemplate, categoryOneId);
                DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {
                    Dictionary<int, string> C2ProductsDic = new Dictionary<int, string>();
                    for (int i = 0; i < rowCount; i++)
                    {
                        C2ProductsDic.Add(int.Parse(data.Rows[i]["C2SysNo"].ToString().Trim()), data.Rows[i]["C2Name"].ToString().Trim());
                    }
                    return C2ProductsDic;
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
        /// 获取需要在前台大类展示的商品
        /// </summary>
        /// <param name="categoryTwoId"></param>
        /// <returns></returns>
        public static List<FrontDsiplayProduct> GetHomeCategoryOneDisplayProducts(int categoryTwoId)
        {
            try
            {
                string sqlCmd = String.Format(GetCategoryTwoProductsSqlCmdTemplate, categoryTwoId);
                DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {
                    List<FrontDsiplayProduct> C2Products = new List<FrontDsiplayProduct>();
                    for (int i = 0; i < rowCount; i++)
                    {
                        C2Products.Add(new FrontDsiplayProduct()
                        {
                            C1SysNo = int.Parse(data.Rows[i]["C1SysNo"].ToString().Trim()),
                            C2SysNo = int.Parse(data.Rows[i]["C2SysNo"].ToString().Trim()),
                            C3SysNo = int.Parse(data.Rows[i]["C3SysNo"].ToString().Trim()),
                            ImgPath = data.Rows[i]["product_limg"].ToString().Trim(),
                            Price = data.Rows[i]["Price"].ToString().Trim(),
                            ProductSysNo = data.Rows[i]["ProductSysNo"].ToString().Trim(),
                            ProductPromotionWord = data.Rows[i]["PromotionWord"].ToString().Trim(),
                            ProductBriefName = data.Rows[i]["BriefName"].ToString().Trim(),
                            BaiscPrice = data.Rows[i]["basicPrice"].ToString().Trim(),
                        });
                    }
                    return C2Products;
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

    public class C1DisplayProductService
    {

        private static readonly string GetC2IDSqlCmdTemplate = @"select distinct p.C2SysNo,c2.C2Name from OnlineListProduct olp
  left join Product p on olp.CategorySysNo=p.C1SysNo
  left join Category1 c1 on olp.CategorySysNo=c1.SysNo
  left join Category2 c2 on p.C2SysNo=c2.SysNo
  where olp.CategorySysNo={0} and c1.Status=0 and c2.Status=0
  and olp.OnlineAreaType={1}
  and olp.OnlineRecommendType={2}";

        private static readonly string GetC2DisplayProductsSqlCmdTemplate = @"select top 8 olp.ProductSysNo as sysno,p.ProductName,p.PromotionWord,CONVERT(float,pp.CurrentPrice) as price,pimg.product_limg,p.C3SysNo,p.BriefName,CONVERT(float,pp.BasicPrice) as basicPrice from OnlineListProduct olp
  left join Product p on olp.ProductSysNo=p.SysNo
  left join Product_Price pp on olp.ProductSysNo=pp.ProductSysNo
  left join Product_Images pimg on olp.ProductSysNo=pimg.product_sysNo
  where p.Status=1 
  and pimg.orderNum=1 
  and olp.OnlineAreaType={0} 
  and olp.OnlineRecommendType={1}
  and olp.CategorySysNo=p.C1SysNo 
  and p.C2SysNo={2}
  order by olp.ListOrder ASC";

        /// <summary>
        /// 获得要在大类页面下
        /// 展示的二类推荐商品的CategoryId
        /// </summary>
        /// <param name="c1SysNo"></param>
        /// <returns></returns>
        public static Dictionary<int, string> GetC2DsiplayIDs(int c1SysNo)
        {
            try
            {
                string sqlCmd = String.Format(GetC2IDSqlCmdTemplate, c1SysNo, (int)AppEnum.OnlineAreaType.FirstCategory, (int)AppEnum.OnlineRecommendType.ExcellentRecommend);
                DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {
                    Dictionary<int, string> C2ProductsDic = new Dictionary<int, string>();
                    for (int i = 0; i < rowCount; i++)
                    {
                        C2ProductsDic.Add(int.Parse(data.Rows[i]["C2SysNo"].ToString().Trim()), data.Rows[i]["C2Name"].ToString().Trim());
                    }
                    return C2ProductsDic;
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
        /// 获得要在大类页面下
        /// 展示的二类推荐商品
        /// </summary>
        /// <param name="c2SysNo"></param>
        /// <returns></returns>
        public static List<FrontDsiplayProduct> GetC2DsiplayProducts(int c2SysNo)
        {
            try
            {
                int onlineAreaType = (int)AppEnum.OnlineAreaType.FirstCategory;
                int onlineRecommendType = (int)AppEnum.OnlineRecommendType.ExcellentRecommend;
                string sqlCmd = String.Format(GetC2DisplayProductsSqlCmdTemplate, onlineAreaType, onlineRecommendType, c2SysNo);
                DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {
                    List<FrontDsiplayProduct> C2Products = new List<FrontDsiplayProduct>();
                    for (int i = 0; i < rowCount; i++)
                    {
                        C2Products.Add(new FrontDsiplayProduct()
                        {
                            C3SysNo = int.Parse(data.Rows[i]["C3SysNo"].ToString().Trim()),
                            ImgPath = data.Rows[i]["product_limg"].ToString().Trim(),
                            Price = data.Rows[i]["price"].ToString().Trim(),
                            ProductSysNo = data.Rows[i]["sysno"].ToString().Trim(),
                            ProductPromotionWord = data.Rows[i]["PromotionWord"].ToString().Trim(),
                            ProductBriefName = data.Rows[i]["BriefName"].ToString().Trim(),
                            BaiscPrice = data.Rows[i]["basicPrice"].ToString().Trim(),
                        });
                    }
                    return C2Products;
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

    public class C2DisplayProductService
    {
        private static readonly string GetC3IDSqlCmdTemplate = @"select distinct p.C3SysNo,c3.C3Name from OnlineListProduct olp
  left join Product p on olp.CategorySysNo=p.C2SysNo
  left join Category2 c2 on olp.CategorySysNo=c2.SysNo
  left join Category3 c3 on p.C3SysNo=c3.SysNo
  where olp.CategorySysNo={0} and c2.Status=0 and c3.Status=0
 and olp.OnlineAreaType={1}
 and olp.OnlineRecommendType={2}";

        private static readonly string GetC3DisplayProductsSqlCmdTemplate = @"select top 8 olp.ProductSysNo as sysno,p.ProductName,p.PromotionWord,CONVERT(float,pp.CurrentPrice) as price,pimg.product_limg,p.C3SysNo,p.BriefName,CONVERT(float,pp.BasicPrice) as basicPrice from OnlineListProduct olp
  left join Product p on olp.ProductSysNo=p.SysNo
  left join Product_Price pp on olp.ProductSysNo=pp.ProductSysNo
  left join Product_Images pimg on olp.ProductSysNo=pimg.product_sysNo
  where p.Status=1 
  and pimg.orderNum=1 
  and olp.OnlineAreaType={0} 
  and olp.OnlineRecommendType={1}
  and olp.CategorySysNo=p.C2SysNo 
  and p.C3SysNo={2}
  order by olp.ListOrder ASC";

        /// <summary>
        /// 获得要在二类页面下
        /// 展示的三类推荐商品的CategoryId
        /// </summary>
        /// <param name="c1SysNo"></param>
        /// <returns></returns>
        public static Dictionary<int, string> GetC3DsiplayIDs(int c2SysNo)
        {
            try
            {
                string sqlCmd = String.Format(GetC3IDSqlCmdTemplate, c2SysNo, (int)AppEnum.OnlineAreaType.SecondCategory, (int)AppEnum.OnlineRecommendType.ExcellentRecommend);
                DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {
                    Dictionary<int, string> C2ProductsDic = new Dictionary<int, string>();
                    for (int i = 0; i < rowCount; i++)
                    {
                        C2ProductsDic.Add(int.Parse(data.Rows[i]["C3SysNo"].ToString().Trim()), data.Rows[i]["C3Name"].ToString().Trim());
                    }
                    return C2ProductsDic;
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
        /// 获得要在二类页面下
        /// 展示的三类推荐商品
        /// </summary>
        /// <param name="c2SysNo"></param>
        /// <returns></returns>
        public static List<FrontDsiplayProduct> GetC3DsiplayProducts(int c3SysNo)
        {
            try
            {
                int onlineAreaType = (int)AppEnum.OnlineAreaType.SecondCategory;
                int onlineRecommendType = (int)AppEnum.OnlineRecommendType.ExcellentRecommend;
                string sqlCmd = String.Format(GetC3DisplayProductsSqlCmdTemplate, onlineAreaType, onlineRecommendType, c3SysNo);
                DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {
                    List<FrontDsiplayProduct> C3Products = new List<FrontDsiplayProduct>();
                    for (int i = 0; i < rowCount; i++)
                    {
                        C3Products.Add(new FrontDsiplayProduct()
                        {
                            ImgPath = data.Rows[i]["product_limg"].ToString().Trim(),
                            Price = data.Rows[i]["price"].ToString().Trim(),
                            ProductSysNo = data.Rows[i]["sysno"].ToString().Trim(),
                            ProductPromotionWord = data.Rows[i]["PromotionWord"].ToString().Trim(),
                            ProductBriefName = data.Rows[i]["BriefName"].ToString().Trim(),
                            BaiscPrice = data.Rows[i]["basicPrice"].ToString().Trim(),
                        });
                    }
                    return C3Products;
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

    public class C1EmptyInventoryProductService
    {
        private static readonly string GetC1EmptyInventoryProductsSqlCmdTemplate = @"select top 4 olp.ProductSysNo,p.PromotionWord,CONVERT(float,pp.CurrentPrice)as price,
  p.C2SysNo,p.C3SysNo,pimg.product_simg,p.BriefName,CONVERT(float,pp.BasicPrice) as baiscPrice
  from OnlineListProduct olp
  left join Product p on olp.ProductSysNo=p.SysNo
  left join Product_Price pp on olp.ProductSysNo=pp.ProductSysNo
  left join Product_Images pimg on olp.ProductSysNo=pimg.product_sysNo
  where olp.CategorySysNo={0}
  and p.Status=1
  and olp.OnlineAreaType={1}
  and olp.OnlineRecommendType={2}
  and pimg.status=1
  and pimg.orderNum=1
  order by olp.ListOrder";

        /// <summary>
        /// 获得清库商品模块的4商品
        /// </summary>
        /// <param name="c1SysNo"></param>
        /// <returns></returns>
        public static List<C1WeeklyBestSaledProduct> GetGetC1EmptyInventoryProducts(int c1SysNo)
        {
            try
            {
                int onlineAreaType = (int)AppEnum.OnlineAreaType.FirstCategory;
                int onlineRecommendType = (int)AppEnum.OnlineRecommendType.Promotion;
                string sqlCmd = String.Format(GetC1EmptyInventoryProductsSqlCmdTemplate, c1SysNo, onlineAreaType, onlineRecommendType);
                DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {
                    List<C1WeeklyBestSaledProduct> products = new List<C1WeeklyBestSaledProduct>();
                    for (int i = 0; i < rowCount; i++)
                    {
                        products.Add(new C1WeeklyBestSaledProduct()
                        {
                            C2SysNo = int.Parse(data.Rows[i]["C2SysNo"].ToString().Trim()),
                            C3SysNo = int.Parse(data.Rows[i]["C3SysNo"].ToString().Trim()),
                            Price = data.Rows[i]["price"].ToString().Trim(),
                            ProductSysNo = data.Rows[i]["ProductSysNo"].ToString().Trim(),
                            ProductPromotionWord = data.Rows[i]["PromotionWord"].ToString().Trim(),
                            ImgPath = data.Rows[i]["product_simg"].ToString().Trim(),
                            ProductBriefName = data.Rows[i]["BriefName"].ToString().Trim(),
                            BaiscPrice = data.Rows[i]["baiscPrice"].ToString().Trim(),
                        });
                    }
                    return products;
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

    public class C2EmptyInventoryProductService
    {
        private static readonly string GetC2EmptyInventoryProductsSqlCmdTemplate = @"select top 4 olp.ProductSysNo,p.PromotionWord,CONVERT(float,pp.CurrentPrice)as price,
  p.C2SysNo,p.C3SysNo,pimg.product_simg,p.BriefName,CONVERT(float,pp.BasicPrice) as baiscPrice
  from OnlineListProduct olp
  left join Product p on olp.ProductSysNo=p.SysNo
  left join Product_Price pp on olp.ProductSysNo=pp.ProductSysNo
  left join Product_Images pimg on olp.ProductSysNo=pimg.product_sysNo
  where olp.CategorySysNo={0}
  and p.Status=1
  and olp.OnlineAreaType={1}
  and olp.OnlineRecommendType={2}
  and pimg.status=1
  and pimg.orderNum=1
  order by olp.ListOrder";

        /// <summary>
        /// 获得而类下面清库商品模块的4商品
        /// </summary>
        /// <param name="c1SysNo"></param>
        /// <returns></returns>
        public static List<C1WeeklyBestSaledProduct> GetGetC2EmptyInventoryProducts(int c2SysNo)
        {
            try
            {
                int onlineAreaType = (int)AppEnum.OnlineAreaType.SecondCategory;
                int onlineRecommendType = (int)AppEnum.OnlineRecommendType.Promotion;
                string sqlCmd = String.Format(GetC2EmptyInventoryProductsSqlCmdTemplate, c2SysNo, onlineAreaType, onlineRecommendType);
                DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {
                    List<C1WeeklyBestSaledProduct> products = new List<C1WeeklyBestSaledProduct>();
                    for (int i = 0; i < rowCount; i++)
                    {
                        products.Add(new C1WeeklyBestSaledProduct()
                        {
                            C3SysNo = int.Parse(data.Rows[i]["C3SysNo"].ToString().Trim()),
                            Price = data.Rows[i]["price"].ToString().Trim(),
                            ProductSysNo = data.Rows[i]["ProductSysNo"].ToString().Trim(),
                            ProductPromotionWord = data.Rows[i]["PromotionWord"].ToString().Trim(),
                            ImgPath = data.Rows[i]["product_simg"].ToString().Trim(),
                            ProductBriefName = data.Rows[i]["BriefName"].ToString().Trim(),
                            BaiscPrice = data.Rows[i]["baiscPrice"].ToString().Trim(),
                        });
                    }
                    return products;
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

    public class C1LastedDisCountProductService
    {
        private static readonly string GetC1LastedDiscountProductsSqlCmdTemplate = @"select top 5 olp.ProductSysNo,p.PromotionWord,pimg.product_simg,convert(float,pp.CurrentPrice) as price,CONVERT(int,(1-(pp.CurrentPrice/pp.BasicPrice))*100.00)as discountRate,p.C2SysNo,p.C3SysNo from OnlineListProduct olp
 left join Product p on olp.ProductSysNo=p.SysNo
 left join Product_Price pp on olp.ProductSysNo=pp.ProductSysNo
 left join Product_Images pimg on olp.ProductSysNo=pimg.product_sysNo
 where olp.CategorySysNo={0} 
 and p.Status=1
 and olp.OnlineAreaType={1}
 and olp.OnlineRecommendType={2}
 and pimg.status=1
 and pimg.orderNum=1
 order by olp.ListOrder";

        public static List<C1LastestDiscountProduct> GetC1LastestDiscountProduct(int c1SysNo)
        {
            try
            {
                int onlineAreaType = (int)AppEnum.OnlineAreaType.FirstCategory;
                int onlineRecommendType = (int)AppEnum.OnlineRecommendType.PopularProduct;
                string sqlCmd = String.Format(GetC1LastedDiscountProductsSqlCmdTemplate, c1SysNo, onlineAreaType, onlineRecommendType);
                DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {
                    List<C1LastestDiscountProduct> products = new List<C1LastestDiscountProduct>();
                    for (int i = 0; i < rowCount; i++)
                    {
                        products.Add(new C1LastestDiscountProduct()
                        {
                            C2SysNo = int.Parse(data.Rows[i]["C2SysNo"].ToString().Trim()),
                            C3SysNo = int.Parse(data.Rows[i]["C3SysNo"].ToString().Trim()),
                            Price = data.Rows[i]["price"].ToString().Trim(),
                            ProductSysNo = data.Rows[i]["ProductSysNo"].ToString().Trim(),
                            ProductPromotionWord = data.Rows[i]["PromotionWord"].ToString().Trim(),
                            DiscountRate = data.Rows[i]["discountRate"].ToString().Trim(),
                            ImgPath = data.Rows[i]["product_simg"].ToString().Trim(),
                        });
                    }
                    return products;
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

    public class C1WeeklyBestSaledProductService
    {
        private static readonly string GetC1WeeklyBestSaledProductsSqlCmdTemplate = @"select olp.ProductSysNo,p.PromotionWord,CONVERT(float,pp.CurrentPrice)as price,
  p.C2SysNo,p.C3SysNo,pimg.product_simg,p.BriefName,CONVERT(float,pp.BasicPrice) as baiscPrice
  from OnlineListProduct olp
  left join Product p on olp.ProductSysNo=p.SysNo
  left join Product_Price pp on olp.ProductSysNo=pp.ProductSysNo
  left join Product_Images pimg on olp.ProductSysNo=pimg.product_sysNo
  where olp.CategorySysNo={0}
  and p.Status=1
  and olp.OnlineAreaType={1}
  and olp.OnlineRecommendType={2}
  and pimg.status=1
  and pimg.orderNum=1
  order by olp.ListOrder";

        public static List<C1WeeklyBestSaledProduct> GetC1WeeklyBestSaledProduct(int c1SysNo)
        {
            try
            {
                int onlineAreaType = (int)AppEnum.OnlineAreaType.FirstCategory;
                int onlineRecommendType = (int)AppEnum.OnlineRecommendType.PromotionTopic;
                string sqlCmd = String.Format(GetC1WeeklyBestSaledProductsSqlCmdTemplate, c1SysNo, onlineAreaType, onlineRecommendType);
                DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {
                    List<C1WeeklyBestSaledProduct> products = new List<C1WeeklyBestSaledProduct>();
                    for (int i = 0; i < rowCount; i++)
                    {
                        products.Add(new C1WeeklyBestSaledProduct()
                        {
                            C2SysNo = int.Parse(data.Rows[i]["C2SysNo"].ToString().Trim()),
                            C3SysNo = int.Parse(data.Rows[i]["C3SysNo"].ToString().Trim()),
                            Price = data.Rows[i]["price"].ToString().Trim(),
                            ProductSysNo = data.Rows[i]["ProductSysNo"].ToString().Trim(),
                            ProductPromotionWord = data.Rows[i]["PromotionWord"].ToString().Trim(),
                            ImgPath = data.Rows[i]["product_simg"].ToString().Trim(),
                            BaiscPrice = data.Rows[i]["baiscPrice"].ToString().Trim(),
                            ProductBriefName = data.Rows[i]["BriefName"].ToString().Trim(),
                        });
                    }
                    return products;
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

    public class C2WeeklyBestSaledProductService
    {
        private static readonly string GetC2WeeklyBestSaledProductsSqlCmdTemplate = @"select olp.ProductSysNo,p.PromotionWord,CONVERT(float,pp.CurrentPrice)as price,
  p.C2SysNo,p.C3SysNo,pimg.product_simg,p.BriefName,CONVERT(float,pp.BasicPrice) as baiscPrice
  from OnlineListProduct olp
  left join Product p on olp.ProductSysNo=p.SysNo
  left join Product_Price pp on olp.ProductSysNo=pp.ProductSysNo
  left join Product_Images pimg on olp.ProductSysNo=pimg.product_sysNo
  where olp.CategorySysNo={0}
  and p.Status=1
  and olp.OnlineAreaType={1}
  and olp.OnlineRecommendType={2}
  and pimg.status=1
  and pimg.orderNum=1
  order by olp.ListOrder";

        public static List<C1WeeklyBestSaledProduct> GetC2WeeklyBestSaledProduct(int c2SysNo)
        {
            try
            {
                int onlineAreaType = (int)AppEnum.OnlineAreaType.SecondCategory;
                int onlineRecommendType = (int)AppEnum.OnlineRecommendType.PromotionTopic;
                string sqlCmd = String.Format(GetC2WeeklyBestSaledProductsSqlCmdTemplate, c2SysNo, onlineAreaType, onlineRecommendType);
                DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {
                    List<C1WeeklyBestSaledProduct> products = new List<C1WeeklyBestSaledProduct>();
                    for (int i = 0; i < rowCount; i++)
                    {
                        products.Add(new C1WeeklyBestSaledProduct()
                        {
                            C3SysNo = int.Parse(data.Rows[i]["C3SysNo"].ToString().Trim()),
                            Price = data.Rows[i]["price"].ToString().Trim(),
                            ProductSysNo = data.Rows[i]["ProductSysNo"].ToString().Trim(),
                            ProductPromotionWord = data.Rows[i]["PromotionWord"].ToString().Trim(),
                            ImgPath = data.Rows[i]["product_simg"].ToString().Trim(),
                            BaiscPrice = data.Rows[i]["baiscPrice"].ToString().Trim(),
                            ProductBriefName = data.Rows[i]["BriefName"].ToString().Trim(),
                        });
                    }
                    return products;
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

    public class C3BestSaledProductService
    {
        private static readonly string GetC1WeeklyBestSaledProductsSqlCmdTemplate = @"select top 3 olp.ProductSysNo,p.PromotionWord,CONVERT(float,pp.CurrentPrice)as price,
  p.C2SysNo,p.C3SysNo,pimg.product_simg,p.BriefName,CONVERT(float,pp.BasicPrice) as baiscPrice
  from OnlineListProduct olp
  left join Product p on olp.ProductSysNo=p.SysNo
  left join Product_Price pp on olp.ProductSysNo=pp.ProductSysNo
  left join Product_Images pimg on olp.ProductSysNo=pimg.product_sysNo
  where olp.CategorySysNo={0}
  and p.Status=1
  and olp.OnlineAreaType={1}
  and olp.OnlineRecommendType={2}
  and pimg.status=1
  and pimg.orderNum=1
  order by olp.ListOrder";

        public static List<C1WeeklyBestSaledProduct> GetC3BestSaledProduct(int c3SysNo)
        {
            try
            {
                int onlineAreaType = (int)AppEnum.OnlineAreaType.ThirdCategory;
                int onlineRecommendType = (int)AppEnum.OnlineRecommendType.ExcellentRecommend;
                string sqlCmd = String.Format(GetC1WeeklyBestSaledProductsSqlCmdTemplate, c3SysNo, onlineAreaType, onlineRecommendType);
                DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {
                    List<C1WeeklyBestSaledProduct> products = new List<C1WeeklyBestSaledProduct>();
                    for (int i = 0; i < rowCount; i++)
                    {
                        products.Add(new C1WeeklyBestSaledProduct()
                        {
                            C2SysNo = int.Parse(data.Rows[i]["C2SysNo"].ToString().Trim()),
                            C3SysNo = int.Parse(data.Rows[i]["C3SysNo"].ToString().Trim()),
                            Price = data.Rows[i]["price"].ToString().Trim(),
                            ProductSysNo = data.Rows[i]["ProductSysNo"].ToString().Trim(),
                            ProductPromotionWord = data.Rows[i]["PromotionWord"].ToString().Trim(),
                            ImgPath = data.Rows[i]["product_simg"].ToString().Trim(),
                            BaiscPrice = data.Rows[i]["baiscPrice"].ToString().Trim(),
                            ProductBriefName = data.Rows[i]["BriefName"].ToString().Trim(),
                        });
                    }
                    return products;
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

    public class C3HotCommentProductService
    {
        private static readonly string GetC1WeeklyBestSaledProductsSqlCmdTemplate = @"select top 5 olp.ProductSysNo,p.PromotionWord,CONVERT(float,pp.CurrentPrice)as price,
  p.C2SysNo,p.C3SysNo,pimg.product_simg,p.BriefName,CONVERT(float,pp.BasicPrice) as baiscPrice
  from OnlineListProduct olp
  left join Product p on olp.ProductSysNo=p.SysNo
  left join Product_Price pp on olp.ProductSysNo=pp.ProductSysNo
  left join Product_Images pimg on olp.ProductSysNo=pimg.product_sysNo
  where olp.CategorySysNo={0}
  and p.Status=1
  and olp.OnlineAreaType={1}
  and olp.OnlineRecommendType={2}
  and pimg.status=1
  and pimg.orderNum=1
  order by olp.ListOrder";

        public static List<C1WeeklyBestSaledProduct> GetC3HotCommentedProduct(int c3SysNo)
        {
            try
            {
                int onlineAreaType = (int)AppEnum.OnlineAreaType.ThirdCategory;
                int onlineRecommendType = (int)AppEnum.OnlineRecommendType.PowerfulSale;
                string sqlCmd = String.Format(GetC1WeeklyBestSaledProductsSqlCmdTemplate, c3SysNo, onlineAreaType, onlineRecommendType);
                DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {
                    List<C1WeeklyBestSaledProduct> products = new List<C1WeeklyBestSaledProduct>();
                    for (int i = 0; i < rowCount; i++)
                    {
                        products.Add(new C1WeeklyBestSaledProduct()
                        {
                            C2SysNo = int.Parse(data.Rows[i]["C2SysNo"].ToString().Trim()),
                            C3SysNo = int.Parse(data.Rows[i]["C3SysNo"].ToString().Trim()),
                            Price = data.Rows[i]["price"].ToString().Trim(),
                            ProductSysNo = data.Rows[i]["ProductSysNo"].ToString().Trim(),
                            ProductPromotionWord = data.Rows[i]["PromotionWord"].ToString().Trim(),
                            ImgPath = data.Rows[i]["product_simg"].ToString().Trim(),
                            BaiscPrice = data.Rows[i]["baiscPrice"].ToString().Trim(),
                            ProductBriefName = data.Rows[i]["BriefName"].ToString().Trim(),
                        });
                    }
                    return products;
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
    
       
    public class C3ProductListSerivice
    {
        private static readonly string getPagedProductListItemsSqlCmdTemplate = @"select distinct top {0} p.SysNo,p.ProductName,p.PromotionWord,CONVERT(float,pp.CurrentPrice) as price,pimg.product_limg,pp.LimitedQty,p.IsCanPurchase,p.BriefName,CONVERT(float,pp.BasicPrice) as baiscPrice,p.CreateTime
  from Product p
  left join Product_Price pp on p.SysNo=pp.ProductSysNo
  left join Product_Images pimg on p.SysNo=pimg.product_sysNo
  left join Product_Attribute2 pa2 on p.SysNo=pa2.ProductSysNo
  where p.Status=1 
  and (pimg.orderNum=1 and pimg.status=1)
  and p.C3SysNo={1}
  {3}
  and p.SysNo not in 
  (select top {2} p.SysNo from Product p where p.Status=1 and p.C3SysNo={4} {5} {6})
  {7} {8}";
        private static readonly string GetProductSysno = @"select max(tb.oidCount) as maxoidCount from ( SELECT ProductSysNo,count(ProductSysNo) as oidCount FROM Product_Attribute2 WHERE {0} GROUP BY ProductSysNo ) as tb";
        private static readonly string GetProductname = @"SELECT ProductSysNo FROM Product_Attribute2 WHERE{0} GROUP BY ProductSysNo having count(ProductSysNo) = {1} ";
        private static readonly string getC3ProductAttributionNameSqlCmdTemplate = @"select ca1.SysNo as A1SysNo,ca2.SysNo as A2SysNo,ca2.Attribute2Name as A2Name from Category_Attribute2 ca2
  left join Category_Attribute1 ca1 on ca2.A1SysNo=ca1.SysNo
  where ca1.C3SysNo={0} and Attribute2Type=1 and ca1.Status=0 and ca2.Status=0
  order by ca1.OrderNum ";

        private static readonly string getC3ProductAttributionOptionSqlCmdTemplate = @"select ca2o.SysNo,ca2o.Attribute2OptionName from Category_Attribute2 ca2 
  left join Category_Attribute2_Option ca2o on ca2.SysNo=ca2o.Attribute2SysNo
  where ca2.SysNo={0}";

        private static readonly string getProductListItemTotalCountSqlCmdTemplate = @"select COUNT(distinct p.SysNo)as totalCount from Product p
  left join Product_Attribute2 pa2 on p.SysNo=pa2.ProductSysNo
  where p.Status=1
  {0}
  and p.C3SysNo={1}";

        public static string getSysno(string arrtibutionFilterSqlCmd, string attribution2IdStr)
        {
            string sqlcom = string.Empty;
            string searchesql = string.Empty;

            if (arrtibutionFilterSqlCmd != "")
            {//修改Bug，就是查询一次。然后去符合筛选项最多的外键，再根据关系去查出需要的主键。就OK了。
                string SQLNo = string.Format(GetProductSysno, arrtibutionFilterSqlCmd);
                DataTable datea = new SqlDBHelper().ExecuteQuery(SQLNo);
                int SYScount = 0;
                int count;
                List<int> proSYSNO = new List<int>();
                DataTable da = null;
                if (datea.Rows[0]["maxoidCount"].ToString() == "")
                {
                    count = 0;
                }
                else
                {
                    count = Convert.ToInt32(datea.Rows[0]["maxoidCount"].ToString());
                }
                string[] cot = attribution2IdStr.Split(',');
                if (cot.Count() == 1)
                {
                    string SQLProductname = string.Format(GetProductname, arrtibutionFilterSqlCmd, count);
                    da = new SqlDBHelper().ExecuteQuery(SQLProductname);

                    SYScount = da.Rows.Count;
                }
                else
                {
                    if (count > 1)
                    {
                        string SQLProductname = string.Format(GetProductname, arrtibutionFilterSqlCmd, count);
                        da = new SqlDBHelper().ExecuteQuery(SQLProductname);

                        SYScount = da.Rows.Count;
                    }
                }

                if (SYScount > 0)
                {

                    for (int i = 0; i < SYScount; i++)
                    {
                        proSYSNO.Add(int.Parse(da.Rows[i]["ProductSysNo"].ToString()));

                    }
                }
                string SYSNO = string.Empty;
                for (int y = 0; y < proSYSNO.Count(); y++)
                {

                    SYSNO += proSYSNO[y].ToString() + ",";
                }
                if (SYSNO == "")
                {
                    sqlcom = "and p.SYSNO in ( " + 0 + " )";
                }
                else
                {
                    searchesql = SYSNO.Remove(SYSNO.Length - 1);
                    sqlcom = "and p.SYSNO in ( " + searchesql + " )";
                }


            }
            return sqlcom;
        }

        

        /// <summary>
        /// 获取分页的商品列表
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="pagedCount"></param>
        /// <param name="c3SysNo"></param>
        /// <param name="orderByOption"></param>
        /// <returns></returns>
        public static List<FrontDsiplayProduct> GetPagedProductList(int startIndex, int pagedCount, int c3SysNo, YoeJoyEnum.ProductListSortedOrder orderByOption, string attribution2IdStr, string order)
        {

            string orderByStr = YoeJoySystemDic.ProductListSortedOrderDic[orderByOption];
            string orderByStr1 = orderByStr;
            string arrtibutionFilterSqlCmd = String.Empty;
            if (attribution2IdStr != null)
            {
                arrtibutionFilterSqlCmd = " Attribute2OptionSysNo in ( " + attribution2IdStr + " )";
            }
            switch (orderByOption)
            {
                case YoeJoyEnum.ProductListSortedOrder.Default:
                    {
                        break;
                    }
                case YoeJoyEnum.ProductListSortedOrder.Price:
                    {
                        orderByStr1 = orderByStr1.Replace("price", "pp.CurrentPrice");
                        break;
                    }
                default:
                    {
                        break;
                    }
            }




            
            try
            {

            string sqlcom= getSysno(arrtibutionFilterSqlCmd, attribution2IdStr);

            

                

                string sqlCmd = String.Format(getPagedProductListItemsSqlCmdTemplate, pagedCount, c3SysNo, startIndex, sqlcom, c3SysNo, orderByStr1, order, orderByStr, order);
                DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {
                    List<FrontDsiplayProduct> products = new List<FrontDsiplayProduct>();
                    for (int i = 0; i < rowCount; i++)
                    {
                        products.Add(new FrontDsiplayProduct()
                        {
                            C3SysNo = c3SysNo,
                            ImgPath = data.Rows[i]["product_limg"].ToString().Trim(),
                            Price = data.Rows[i]["price"].ToString().Trim(),
                            ProductSysNo = data.Rows[i]["SysNo"].ToString().Trim(),
                            ProductPromotionWord = data.Rows[i]["PromotionWord"].ToString().Trim(),
                            IsCanPurchase = (int.Parse(data.Rows[i]["IsCanPurchase"].ToString().Trim()) == 1) ? true : false,
                            LimitQty = int.Parse(data.Rows[i]["LimitedQty"].ToString().Trim()),
                            BaiscPrice = data.Rows[i]["baiscPrice"].ToString().Trim(),
                            ProductBriefName = data.Rows[i]["BriefName"].ToString().Trim(),
                        });
                    }
                    return products;
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
        /// 获得该类别下的商品总数
        /// </summary>
        /// <param name="c3SysNo"></param>
        /// <returns></returns>
        public static int GetPagedProductListItemTotalCount(int c3SysNo, string attribution2IdStr)
        {
         
            string arrtibutionFilterSqlCmd = string.Empty;
            if (attribution2IdStr != null)
            {
                arrtibutionFilterSqlCmd = " Attribute2OptionSysNo in ( " + attribution2IdStr + " )";
            }
           arrtibutionFilterSqlCmd=  getSysno(arrtibutionFilterSqlCmd, attribution2IdStr);

            string sqlCmd = String.Format(getProductListItemTotalCountSqlCmdTemplate, arrtibutionFilterSqlCmd, c3SysNo);
            try
            {
                DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {
                    return int.Parse(data.Rows[0]["totalCount"].ToString().Trim());
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获得商品筛选项
        /// </summary>
        /// <param name="c3SysNo"></param>
        /// <returns></returns>
        public static List<C3ProductAttribution> GetC3ProductAttribution(int c3SysNo)
        {
            string sqlCmd = String.Format(getC3ProductAttributionNameSqlCmdTemplate, c3SysNo);
            try
            {
                DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {
                    List<C3ProductAttribution> c3ProductAttributions = new List<C3ProductAttribution>();
                    for (int i = 0; i < rowCount; i++)
                    {
                        C3ProductAttribution c3ProductAttribution = new C3ProductAttribution()
                        {
                            A1SysNo = int.Parse(data.Rows[i]["A1SysNo"].ToString().Trim()),
                            A2SysNo = int.Parse(data.Rows[i]["A2SysNo"].ToString().Trim()),
                            A2Name = data.Rows[i]["A2Name"].ToString().Trim(),
                        };
                        string sqlCmd2 = String.Format(getC3ProductAttributionOptionSqlCmdTemplate, c3ProductAttribution.A2SysNo);
                        DataTable data2 = new SqlDBHelper().ExecuteQuery(sqlCmd2);
                        int rowCount2 = data2.Rows.Count;
                        if (rowCount2 > 0)
                        {
                            c3ProductAttribution.Options = new List<C3ProductAttributionOption>();
                            for (int j = 0; j < rowCount2; j++)
                            {
                                C3ProductAttributionOption option = new C3ProductAttributionOption()
                                {
                                    OptionSysNo = int.Parse(data2.Rows[j]["SysNo"].ToString().Trim()),
                                    OptionName = data2.Rows[j]["Attribute2OptionName"].ToString().Trim(),
                                };
                                c3ProductAttribution.Options.Add(option);
                            }
                        }
                        else
                        {
                            c3ProductAttribution.Options = null;
                        }
                        c3ProductAttributions.Add(c3ProductAttribution);
                    }
                    return c3ProductAttributions;
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

    public class Search1ProductService
    {
        
       private static readonly string getDetailSearch = @"select distinct p.C1SysNo from Product p 
 left join Category3 c3 on p.C3SysNo=c3.SysNo
 where p.Status=1
 and c3.Status=0
 and p.C3SysNo={0}";
       private static readonly string getDetailSearch1 = @" select top 4 * from Brand where C1SysNo={0}";

           private static readonly string getSearch = @"select distinct p.C1SysNo from Product p 
 left join Category3 c3 on p.C3SysNo=c3.SysNo
 where p.Status=1
 and c3.Status=0
 and ( p.BriefName like ('%{0}%') or p.PromotionWord like ('%{1}%') or  p.PromotionWord like ('%{0}%') or  p.BriefName like ('%{1}%') )";

           private static readonly string getSearchrulest = @" select top 2 * from Brand where C1SysNo={0} union
select top 2 * from Brand where C1SysNo={1}";
           private static readonly string getSearchrulest11 = @" select top 4 * from Brand where C1SysNo={0} union
select top 2 * from Brand where C1SysNo={1}"; 
        private static readonly string getSearch1C3NamesSqlCmdTemplate1 = @"select distinct p.C1SysNo,p.C2SysNo,p.C3SysNo,c3.C3Name from Product p 
 left join Category3 c3 on p.C3SysNo=c3.SysNo
 where p.Status=1
 and c3.Status=0
 and ( p.BriefName like ('%{0}%') or p.PromotionWord like ('%{1}%'）or  p.PromotionWord like ('%{0}%')or  p.BriefName like ('%{1}%') )";
        
        private static readonly string getSearch1C3NamesSqlCmdTemplate = @"select distinct p.C1SysNo,p.C2SysNo,p.C3SysNo,c3.C3Name from Product p 
 left join Category3 c3 on p.C3SysNo=c3.SysNo
 where p.Status=1
 and c3.Status=0
 and ( p.BriefName like ('%{0}%') or p.PromotionWord like ('%{0}%' ))";
        private static readonly string getSearch1C3ProductTotalCountSqlCmdTemplate = @"select COUNT(distinct p.SysNo)as totalCount from Product p
 where p.Status=1 and 
( p.BriefName like ('%{0}%') or p.PromotionWord like ('%{1}%') or  p.PromotionWord like ('%{0}%')or p.BriefName like ('%{1}%') )";

        //        private static readonly string getSearch1ProductsSqlCmdTemplate = @"select distinct p.SysNo,p.ProductName,p.PromotionWord,CONVERT(float,pp.CurrentPrice) as price,pimg.product_limg,p.C1SysNo,p.C2SysNo,p.C3SysNo,p.BriefName,CONVERT(float,pp.BasicPrice) as baiscPrice,p.IsCanPurchase,p.CreateTime,pp.LimitedQty from Product p
        //  left join Product_Price pp on p.SysNo=pp.ProductSysNo
        //  left join Product_Images pimg on p.SysNo=pimg.product_sysNo 
        //  left join Brand b on p.C1SysNo=b.C1SysNo
        //  where p.Status=1  
        //  and (pimg.orderNum=1 and pimg.status=1) 
        //  and (p.PromotionWord like ('%{0}%') or p.BriefName like ('%{1}%') or b.BrandName like ('%{2}%') {3}) {4} {5}";

        private static readonly string getSearch1ProductsSqlCmdTemplate = @"select distinct p.SysNo,p.ProductName,p.PromotionWord,CONVERT(float,pp.CurrentPrice) as price,pimg.product_limg,p.C1SysNo,p.C2SysNo,p.C3SysNo,p.BriefName,CONVERT(float,pp.BasicPrice) as baiscPrice,p.IsCanPurchase,p.CreateTime,pp.LimitedQty from Product p
  left join Product_Price pp on p.SysNo=pp.ProductSysNo
  left join Product_Images pimg on p.SysNo=pimg.product_sysNo 
  where p.Status=1  
  and (pimg.orderNum=1 and pimg.status=1) 
  and (p.PromotionWord like ('%{0}%') or p.BriefName like ('%{1}%')  {3}) {4} {5}";


        private static readonly string getSearchC1SysNo = @"select top 1 p.C1SysNo from Product p
 left join Category1 c1 on p.C1SysNo=c1.SysNo
 where p.Status=1 
 and c1.Status=0
 and (p.PromotionWord like ('%{0}%') or p.BriefName like ('%{1}%') or b.BrandName like ('%{2}%') {3})";


        /// <summary>
        /// 获得相关搜索
        /// </summary>
        /// <returns></returns>
        public static List<Research> GetSearch(string keyWords)
        {

            List<Research> filters = new List<Research>();
            string childSearchSqlCmd = String.Empty;
            string sqlCmd = string.Empty;
            string[] keyWordsArray = keyWords.Split(' ');
            if (keyWordsArray.Count() == 1)
            {
                sqlCmd = String.Format(getSearch, keyWordsArray[0].Trim(), keyWordsArray[0].Trim());
            }
            else
            {
                sqlCmd = String.Format(getSearch, keyWordsArray[0].Trim(), keyWordsArray[1].Trim(), keyWordsArray[0].Trim(), keyWordsArray[1].Trim());

            }
            try
            {
                DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
                string searchrulest = string.Empty;
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {
                    if (rowCount == 1)
                    {
                         searchrulest = String.Format(getSearchrulest11, data.Rows[0]["C1SysNo"].ToString(),0);
                    }
                    if (rowCount == 2)
                    {
                         searchrulest = String.Format(getSearchrulest, data.Rows[0]["C1SysNo"].ToString(), data.Rows[1]["C1SysNo"].ToString());
                    }
                    if (rowCount == 0)
                    {

                    }

                   

                    DataTable da = new SqlDBHelper().ExecuteQuery(searchrulest);

                    int rowss = da.Rows.Count;
                    if (rowss > 0)
                    {

                      
                            filters.Add(new Research()
                            {
                                rulest1 = da.Rows[0]["BrandName"].ToString().Trim(),
                                rulest2 = da.Rows[1]["BrandName"].ToString().Trim(),
                                rulest3 = da.Rows[2]["BrandName"].ToString().Trim(),
                                rulest4 = da.Rows[3]["BrandName"].ToString().Trim(),
                            });
                        


                     
                    }
                    else
                    {
                        return null;
                    }
                }
                return filters;
            }
            catch
            {
                return null;
            }
      
        }

        //Detail搜索
        public static List<Research> GetSecondSearch(int C3SYSNO)
        {

            List<Research> filters = new List<Research>();
            string childSearchSqlCmd = String.Empty;
            string sqlCmd = string.Empty;

            sqlCmd = String.Format(getDetailSearch, C3SYSNO);
           
            try
            {
                DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
                string searchrulest = string.Empty;
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {
                   
                        searchrulest = String.Format(getDetailSearch1, data.Rows[0]["C1SysNo"].ToString(), 0);
                  

                    DataTable da = new SqlDBHelper().ExecuteQuery(searchrulest);

                    int rowss = da.Rows.Count;
                    if (rowss > 0)
                    {


                        filters.Add(new Research()
                        {
                            rulest1 = da.Rows[0]["BrandName"].ToString().Trim(),
                            rulest2 = da.Rows[1]["BrandName"].ToString().Trim(),
                            rulest3 = da.Rows[2]["BrandName"].ToString().Trim(),
                            rulest4 = da.Rows[3]["BrandName"].ToString().Trim(),
                        });




                    }
                    else
                    {
                        return null;
                    }
                }
                return filters;
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// 获得Serach1的商品小类
        /// </summary>
        /// <returns></returns>
        public static List<C3ProductSerach1Filter> GetSearch1C3Names(string keyWords)
        {
            string childSearchSqlCmd = String.Empty;
            string sqlCmd = string.Empty;
            string[] keyWordsArray = keyWords.Split(' ');
            if (keyWordsArray.Count() == 1)
            {
                sqlCmd = String.Format(getSearch1C3NamesSqlCmdTemplate, keyWordsArray[0].Trim(), keyWordsArray[0].Trim());
            }
            else
            {
                 sqlCmd = String.Format(getSearch1C3NamesSqlCmdTemplate1, keyWordsArray[0].Trim(), keyWordsArray[1].Trim(), keyWordsArray[0].Trim(), keyWordsArray[1].Trim());

            }
            try
            {
                DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {
                    List<C3ProductSerach1Filter> filters = new List<C3ProductSerach1Filter>();
                    for (int j = 0; j < rowCount; j++)
                    {
                        filters.Add(new C3ProductSerach1Filter()
                        {
                            C1SysNo = int.Parse(data.Rows[j]["C1SysNo"].ToString().Trim()),
                            C2SysNo = int.Parse(data.Rows[j]["C2SysNo"].ToString().Trim()),
                            C3SysNo = int.Parse(data.Rows[j]["C3SysNo"].ToString().Trim()),
                            C3Name = data.Rows[j]["C3Name"].ToString().Trim(),
                        });
                    }
                    return filters;
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
        /// 返回search1的结果总数
        /// </summary>
        /// <param name="keyWords"></param>
        /// <returns></returns>
        public static int GetSearch1C3ProductTotalCount(string keyWords)
        {
            string childSearchSqlCmd = String.Empty;
            string sqlCmd=string.Empty;
            string[] keyWordsArray = keyWords.Split(' ');
            if (keyWordsArray.Count() == 1)
            {
                sqlCmd = String.Format(getSearch1C3NamesSqlCmdTemplate, keyWordsArray[0].Trim(), keyWordsArray[0].Trim());
            }
            else
            {
                sqlCmd = String.Format(getSearch1C3NamesSqlCmdTemplate1, keyWordsArray[0].Trim(), keyWordsArray[1].Trim(), keyWordsArray[0].Trim(), keyWordsArray[1].Trim());

            }
            try
            {
                DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {
                    return int.Parse(data.Rows[0]["totalCount"].ToString().Trim());
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取搜索结果中第一个商品的大类ID
        /// </summary>
        /// <param name="keyWords"></param>
        /// <returns></returns>
        public static int GetSearchC1SysNo(string keyWords)
        {
            string childSearchSqlCmd = String.Empty;
            string[] keyWordsArray = keyWords.Split(' ');
            if (keyWordsArray.Length > 1)
            {
                for (int i = 1; i < keyWordsArray.Length; i++)
                {
                    childSearchSqlCmd += String.Format(" or p.PromotionWord like ('%{0}%') or p.BriefName like ('%{1}%') or b.BrandName like ('%{2}%') ", keyWordsArray[i].Trim(), keyWordsArray[i].Trim(), keyWordsArray[i].Trim());
                }
            }
            string sqlCmd = String.Format(getSearchC1SysNo, keyWordsArray[0].Trim(), keyWordsArray[0].Trim(), keyWordsArray[0].Trim(), childSearchSqlCmd);
            try
            {
                DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {
                    return int.Parse(data.Rows[0]["C1SysNo"].ToString().Trim());
                }
                else
                {
                    return int.Parse(new SqlDBHelper().ExecuteQuery(@"select top 1 c1.SysNo from Category1 c1
 where c1.Status=0").Rows[0]["SysNo"].ToString().Trim());
                }
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 取得分页的搜索结果集合
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="pagedCount"></param>
        /// <param name="c3SysNo"></param>
        /// <param name="orderByOption"></param>
        /// <param name="attribution2IdStr"></param>
        /// <returns></returns>
        public static List<FrontDsiplayProduct> GetPagedSearch1Products(YoeJoyEnum.ProductListSortedOrder orderByOption, string keyWords, string order = "DESC")
        {
            string orderByStr = YoeJoySystemDic.ProductListSortedOrderDic[orderByOption];
            string orderByStr1 = orderByStr;
            switch (orderByOption)
            {
                case YoeJoyEnum.ProductListSortedOrder.Default:
                    {
                        break;
                    }
                case YoeJoyEnum.ProductListSortedOrder.Price:
                    {
                        orderByStr1 = orderByStr1.Replace("price", "pp.CurrentPrice");
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            string childSearchSqlCmd = String.Empty;
            string[] keyWordsArray = keyWords.Split(' ');
            if (keyWordsArray.Length > 1)
            {
                for (int i = 1; i < keyWordsArray.Length; i++)
                {
                    childSearchSqlCmd += String.Format(" or p.PromotionWord like ('%{0}%') or p.BriefName like ('%{1}%') ", keyWordsArray[i].Trim(), keyWordsArray[i].Trim(), keyWordsArray[i].Trim());
                }
            }
            string sqlCmd = String.Format(getSearch1ProductsSqlCmdTemplate, keyWordsArray[0].Trim(), keyWordsArray[0].Trim(), keyWordsArray[0].Trim(), childSearchSqlCmd, orderByStr, order);
            try
            {
                DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {
                    List<FrontDsiplayProduct> products = new List<FrontDsiplayProduct>();
                    for (int i = 0; i < rowCount; i++)
                    {
                        products.Add(new FrontDsiplayProduct()
                        {
                            C1SysNo = int.Parse(data.Rows[i]["C1SysNo"].ToString().Trim()),
                            C2SysNo = int.Parse(data.Rows[i]["C2SysNo"].ToString().Trim()),
                            C3SysNo = int.Parse(data.Rows[i]["C3SysNo"].ToString().Trim()),
                            ImgPath = data.Rows[i]["product_limg"].ToString().Trim(),
                            Price = data.Rows[i]["price"].ToString().Trim(),
                            ProductSysNo = data.Rows[i]["SysNo"].ToString().Trim(),
                            ProductPromotionWord = data.Rows[i]["PromotionWord"].ToString().Trim(),
                            ProductBriefName = data.Rows[i]["BriefName"].ToString().Trim(),
                            BaiscPrice = data.Rows[i]["baiscPrice"].ToString().Trim(),
                            IsCanPurchase = (int.Parse(data.Rows[i]["IsCanPurchase"].ToString().Trim()) == 1) ? true : false,
                            LimitQty = int.Parse(data.Rows[i]["LimitedQty"].ToString().Trim()),
                        });
                    }
                    return products;
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

    public class Search2ProductService
    {
        private static readonly string getSearch2ProductListItemTotalCountSqlCmdTemplate = @"select COUNT(distinct p.SysNo)as totalCount from Product p
  left join Product_Attribute2 pa2 on p.SysNo=pa2.ProductSysNo
  where p.Status=1
  {0}
  and (p.PromotionWord like ('%{1}%') or p.BriefName like ('%{2}%')  {3})
  and p.C3SysNo={4}";

        private static readonly string getSearch2C3ProductsSqlCmdTemplate = @"select distinct p.SysNo,p.ProductName,p.PromotionWord,CONVERT(float,pp.CurrentPrice) as price,pimg.product_limg,p.C1SysNo,p.C2SysNo,p.C3SysNo,p.BriefName,CONVERT(float,pp.BasicPrice) as baiscPrice,p.IsCanPurchase,p.CreateTime,pp.LimitedQty from Product p
  left join Product_Price pp on p.SysNo=pp.ProductSysNo
  left join Product_Images pimg on p.SysNo=pimg.product_sysNo 
  left join Product_Attribute2 pa2 on p.SysNo=pa2.ProductSysNo
  where p.Status=1  
  and (pimg.orderNum=1 and pimg.status=1) 
  {0}
  and p.C3Sysno={1} {2}";

        private static readonly string GetProductSysno = @"select max(tb.oidCount) as maxoidCount from ( SELECT ProductSysNo,count(ProductSysNo) as oidCount FROM Product_Attribute2 WHERE {0} GROUP BY ProductSysNo ) as tb";
        private static readonly string GetProductname = @"SELECT ProductSysNo FROM Product_Attribute2 WHERE {0} GROUP BY ProductSysNo having count(ProductSysNo) = {1} ";
        public static string getSysno(string arrtibutionFilterSqlCmd, string attribution2IdStr)
        {
            string sqlcom = string.Empty;
            string searchesql = string.Empty;

            if (arrtibutionFilterSqlCmd != "")
            {//修改Bug，就是查询一次。然后去符合筛选项最多的外键，再根据关系去查出需要的主键。就OK了。
                string SQLNo = string.Format(GetProductSysno, arrtibutionFilterSqlCmd);
                DataTable datea = new SqlDBHelper().ExecuteQuery(SQLNo);
                int SYScount = 0;
                int count;
                List<int> proSYSNO = new List<int>();
                DataTable da = null;
                if (datea.Rows[0]["maxoidCount"].ToString() == "")
                {
                    count = 0;
                }
                else
                {
                    count = Convert.ToInt32(datea.Rows[0]["maxoidCount"].ToString());
                }
                string[] cot = attribution2IdStr.Split(',');
                if (cot.Count() == 1)
                {
                    string SQLProductname = string.Format(GetProductname, arrtibutionFilterSqlCmd, count);
                    da = new SqlDBHelper().ExecuteQuery(SQLProductname);

                    SYScount = da.Rows.Count;
                }
                else
                {
                    if (count > 1)
                    {
                        string SQLProductname = string.Format(GetProductname, arrtibutionFilterSqlCmd, count);
                        da = new SqlDBHelper().ExecuteQuery(SQLProductname);

                        SYScount = da.Rows.Count;
                    }
                }

                if (SYScount > 0)
                {

                    for (int i = 0; i < SYScount; i++)
                    {
                        proSYSNO.Add(int.Parse(da.Rows[i]["ProductSysNo"].ToString()));

                    }
                }
                string SYSNO = string.Empty;
                for (int y = 0; y < proSYSNO.Count(); y++)
                {

                    SYSNO += proSYSNO[y].ToString() + ",";
                }
                if (SYSNO == "")
                {
                    sqlcom = "and p.SYSNO in ( " + 0 + " )";
                }
                else
                {
                    searchesql = SYSNO.Remove(SYSNO.Length - 1);
                    sqlcom = "and p.SYSNO in ( " + searchesql + " )";
                }


            }
            return sqlcom;
        }
        /// <summary>
        /// 获得满足search2条件的商品总数
        /// </summary>
        /// <param name="c3SysNo"></param>
        /// <param name="attribution2IdStr"></param>
        /// <param name="keyWords"></param>
        /// <returns></returns>
        public static int GetSearch2C3ProductTotalCount(int c3SysNo, string attribution2IdStr, string keyWords)
        {
            string arrtibutionFilterSqlCmd = String.Empty;
            if (attribution2IdStr != null)
            {
                arrtibutionFilterSqlCmd = "and pa2.Attribute2OptionSysNo in ( " + attribution2IdStr + " )";
            }
            string childSearchSqlCmd = String.Empty;
            string[] keyWordsArray = keyWords.Split(' ');
            if (keyWordsArray.Length > 1)
            {
                for (int i = 1; i < keyWordsArray.Length; i++)
                {
                    childSearchSqlCmd += String.Format(" or p.PromotionWord like ('%{0}%') or p.BriefName like ('%{1}%') ) ", keyWordsArray[i].Trim(), keyWordsArray[i].Trim());
                }
            }
            string sqlCmd = String.Format(getSearch2ProductListItemTotalCountSqlCmdTemplate, arrtibutionFilterSqlCmd, keyWordsArray[0].Trim(),  keyWordsArray[0].Trim(), childSearchSqlCmd, c3SysNo);
            try
            {
                DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {
                    return int.Parse(data.Rows[0]["totalCount"].ToString().Trim());
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获得满足search2条件的分页的商品
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="pagedCount"></param>
        /// <param name="c3SysNo"></param>
        /// <param name="orderByOption"></param>
        /// <param name="attribution2IdStr"></param>
        /// <returns></returns>
        public static List<FrontDsiplayProduct> GetPagedProductList(int c3SysNo, YoeJoyEnum.ProductListSortedOrder orderByOption, string attribution2IdStr, string keyWords, string order = "DESC")
        {
            string orderByStr = YoeJoySystemDic.ProductListSortedOrderDic[orderByOption];
            string orderByStr1 = orderByStr;
            string arrtibutionFilterSqlCmd = String.Empty;
            if (attribution2IdStr != null)
            {
                arrtibutionFilterSqlCmd = " Attribute2OptionSysNo in ( " + attribution2IdStr + " )";
            }
            string sqlcom = getSysno(arrtibutionFilterSqlCmd, attribution2IdStr);

            string childSearchSqlCmd = String.Empty;
            //string[] keyWordsArray = keyWords.Split(' ');
            //if (keyWordsArray.Length > 1)
            //{
            //    for (int i = 1; i < keyWordsArray.Length; i++)
            //    {
            //        childSearchSqlCmd += String.Format(" or p.PromotionWord like ('%{0}%') or p.BriefName like ('%{1}%')", keyWordsArray[i].Trim(),  keyWordsArray[i].Trim());
            //    }
            //}


            switch (orderByOption)
            {
                case YoeJoyEnum.ProductListSortedOrder.Default:
                    {
                        break;
                    }
                case YoeJoyEnum.ProductListSortedOrder.Price:
                    {
                        orderByStr1 = orderByStr1.Replace("price", "pp.CurrentPrice");
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            string sqlCmd = String.Format(getSearch2C3ProductsSqlCmdTemplate, sqlcom, c3SysNo, orderByStr);
            try
            {
                DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {
                    List<FrontDsiplayProduct> products = new List<FrontDsiplayProduct>();
                    for (int i = 0; i < rowCount; i++)
                    {
                        products.Add(new FrontDsiplayProduct()
                        {
                            C1SysNo = int.Parse(data.Rows[i]["C1SysNo"].ToString().Trim()),
                            C2SysNo = int.Parse(data.Rows[i]["C2SysNo"].ToString().Trim()),
                            C3SysNo = int.Parse(data.Rows[i]["C3SysNo"].ToString().Trim()),
                            ImgPath = data.Rows[i]["product_limg"].ToString().Trim(),
                            Price = data.Rows[i]["price"].ToString().Trim(),
                            ProductSysNo = data.Rows[i]["SysNo"].ToString().Trim(),
                            ProductPromotionWord = data.Rows[i]["PromotionWord"].ToString().Trim(),
                            ProductBriefName = data.Rows[i]["BriefName"].ToString().Trim(),
                            BaiscPrice = data.Rows[i]["baiscPrice"].ToString().Trim(),
                            IsCanPurchase = (int.Parse(data.Rows[i]["IsCanPurchase"].ToString().Trim()) == 1) ? true : false,
                            LimitQty = int.Parse(data.Rows[i]["LimitedQty"].ToString().Trim()),
                        });
                    }
                    return products;
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

    public class CustomerBrowserHistoryProductService
    {
        private static readonly string getCustomerBrowserHistoryProductAllSqlCmdTemplate = @"select p.SysNo,p.C1SysNo,p.C2SysNo,p.C3SysNo,p.BriefName,p.PromotionWord,CONVERT(float,pp.CurrentPrice) as currentprice,CONVERT(float,pp.BasicPrice) as basicprice,pimgs.product_limg,pimgs.product_simg from Product p
  left join Product_Price pp on p.SysNo=pp.ProductSysNo
  left join Product_Images pimgs on p.SysNo=pimgs.product_sysNo
  where p.SysNo in ({0})
  and p.Status=1
  and pimgs.status=1
  and pimgs.orderNum=1";

        /// <summary>
        /// 获得用户的浏览过的商品信息
        /// </summary>
        /// <param name="productSysNoList"></param>
        /// <returns></returns>
        public static ArrayList GetBrowseHistoryList(string sysNoList)
        {
            if (sysNoList == null || sysNoList == string.Empty)
            {
                return null;
            }
            try
            {
                string sqlCmd = String.Format(getCustomerBrowserHistoryProductAllSqlCmdTemplate, sysNoList);

                DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);

                if (!Util.HasMoreRow(data))
                    return null;

                //将数据库获取的数据放入Hashtable
                Hashtable ht = new Hashtable(10);
                foreach (DataRow dr in data.Rows)
                {
                    CustomerBrowserHistoryProduct cbHisProduct = new CustomerBrowserHistoryProduct()
                    {
                        ProductSysNo = dr["SysNo"].ToString().Trim(),
                        C1SysNo = int.Parse(dr["C1SysNo"].ToString().Trim()),
                        C2SysNo = int.Parse(dr["C2SysNo"].ToString().Trim()),
                        C3SysNo = int.Parse(dr["C3SysNo"].ToString().Trim()),
                        ProductBriefName = dr["BriefName"].ToString().Trim(),
                        PromotionWord = dr["PromotionWord"].ToString().Trim(),
                        LargeImg = dr["product_limg"].ToString().Trim(),
                        SmallImg = dr["product_simg"].ToString().Trim(),
                        CurrentPrice = dr["currentprice"].ToString().Trim(),
                        StandardPrice = dr["basicprice"].ToString().Trim(),
                    };
                    ht.Add(cbHisProduct.ProductSysNo.ToString(), cbHisProduct);
                }

                //根据次序进行排序
                ArrayList al = new ArrayList(10);
                string[] sysArr = sysNoList.Split(',');
                for (int i = 0; i < sysArr.Length; i++)
                {
                    if (ht.Contains(sysArr[i])) //有可能商品Status变化,所以ht会比cookie中的少
                        al.Add(ht[sysArr[i]]);
                }

                return al;
            }
            catch
            {
                return null;
            }
        }
    }

    public class HomePromotionProductService
    {
        private static readonly string getHomePromotionProductsSqlCmdTemplate = @"select top 4 olp.ProductSysNo,p.C1SysNo,p.C2SysNo,p.C3SysNo,p.ProductName,p.PromotionWord,CONVERT(float,pp.CurrentPrice) as price,pimg.product_limg,p.BriefName,CONVERT(float,pp.BasicPrice) as basicPrice from OnlineListProduct olp 
  left join Product p on olp.ProductSysNo=p.SysNo 
  left join Product_Price pp on olp.ProductSysNo=pp.ProductSysNo
  left join Product_Images pimg on olp.ProductSysNo=pimg.product_sysNo
  where p.Status=1 and pimg.orderNum=1 and olp.OnlineAreaType={0} and olp.OnlineRecommendType={1} and olp.CategorySysNo=0
  order by olp.ListOrder ASC";

        /// <summary>
        /// 获得首页的促销商品
        /// </summary>
        /// <returns></returns>
        public static List<FrontDsiplayProduct> GetHomePromotionProducts()
        {
            int onlineAreaType = (int)AppEnum.OnlineAreaType.HomePage;
            int onlineRecommendType = (int)AppEnum.OnlineRecommendType.Discount;
            string sqlCmd = String.Format(getHomePromotionProductsSqlCmdTemplate, onlineAreaType, onlineRecommendType);
            DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
            int count = data.Rows.Count;
            if (count > 0)
            {
                List<FrontDsiplayProduct> products = new List<FrontDsiplayProduct>();
                for (int i = 0; i < count; i++)
                {
                    products.Add(new FrontDsiplayProduct()
                    {
                        ProductSysNo = data.Rows[i]["ProductSysNo"].ToString().Trim(),
                        ProductPromotionWord = data.Rows[i]["PromotionWord"].ToString().Trim(),
                        Price = data.Rows[i]["price"].ToString().Trim(),
                        ImgPath = data.Rows[i]["product_limg"].ToString().Trim(),
                        C1SysNo = int.Parse(data.Rows[i]["C1SysNo"].ToString().Trim()),
                        C2SysNo = int.Parse(data.Rows[i]["C2SysNo"].ToString().Trim()),
                        C3SysNo = int.Parse(data.Rows[i]["C3SysNo"].ToString().Trim()),
                        BaiscPrice = data.Rows[i]["basicPrice"].ToString().Trim(),
                        ProductBriefName = data.Rows[i]["BriefName"].ToString().Trim(),
                    });
                }
                return products;
            }
            else
            {
                return null;
            }
        }

    }

    public class HomeHotCommentedProductService
    {
        private static readonly string getHomePromotionProductsSqlCmdTemplate = @"select top 4 olp.ProductSysNo,p.C1SysNo,p.C2SysNo,p.C3SysNo,p.ProductName,p.PromotionWord,CONVERT(float,pp.CurrentPrice) as price,pimg.product_simg,p.BriefName,CONVERT(float,pp.BasicPrice) as basicPrice from OnlineListProduct olp 
  left join Product p on olp.ProductSysNo=p.SysNo 
  left join Product_Price pp on olp.ProductSysNo=pp.ProductSysNo
  left join Product_Images pimg on olp.ProductSysNo=pimg.product_sysNo
  where p.Status=1 and pimg.orderNum=1 and olp.OnlineAreaType={0} and olp.OnlineRecommendType={1} and olp.CategorySysNo=0
and olp.ListOrder between {2} and {3}
  order by olp.ListOrder ASC";

        /// <summary>
        /// 获得首页的热评商品
        /// </summary>
        /// <returns></returns>
        public static List<FrontDsiplayProduct> GetHomeHotCommentedProducts(int startIndex, int endIndex)
        {
            int onlineAreaType = (int)AppEnum.OnlineAreaType.HomePage;
            int onlineRecommendType = (int)AppEnum.OnlineRecommendType.PowerfulSale;
            string sqlCmd = String.Format(getHomePromotionProductsSqlCmdTemplate, onlineAreaType, onlineRecommendType, startIndex, endIndex);
            DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
            int count = data.Rows.Count;
            if (count > 0)
            {
                List<FrontDsiplayProduct> products = new List<FrontDsiplayProduct>();
                for (int i = 0; i < count; i++)
                {
                    products.Add(new FrontDsiplayProduct()
                    {
                        ProductSysNo = data.Rows[i]["ProductSysNo"].ToString().Trim(),
                        ProductPromotionWord = data.Rows[i]["PromotionWord"].ToString().Trim(),
                        Price = data.Rows[i]["price"].ToString().Trim(),
                        ImgPath = data.Rows[i]["product_simg"].ToString().Trim(),
                        C1SysNo = int.Parse(data.Rows[i]["C1SysNo"].ToString().Trim()),
                        C2SysNo = int.Parse(data.Rows[i]["C2SysNo"].ToString().Trim()),
                        C3SysNo = int.Parse(data.Rows[i]["C3SysNo"].ToString().Trim()),
                        BaiscPrice = data.Rows[i]["basicPrice"].ToString().Trim(),
                        ProductBriefName = data.Rows[i]["BriefName"].ToString().Trim(),
                    });
                }
                return products;
            }
            else
            {
                return null;
            }
        }
    }

    public class HomeBestSaledProductService
    {
        private static readonly string getHomePromotionProductsSqlCmdTemplate = @"select top 3 olp.ProductSysNo,p.C1SysNo,p.C2SysNo,p.C3SysNo,p.ProductName,p.PromotionWord,CONVERT(float,pp.CurrentPrice) as price,pimg.product_simg,p.BriefName,CONVERT(float,pp.BasicPrice) as basicPrice from OnlineListProduct olp 
  left join Product p on olp.ProductSysNo=p.SysNo 
  left join Product_Price pp on olp.ProductSysNo=pp.ProductSysNo
  left join Product_Images pimg on olp.ProductSysNo=pimg.product_sysNo
  where p.Status=1 and pimg.orderNum=1 and olp.OnlineAreaType={0} and olp.OnlineRecommendType={1} and olp.CategorySysNo=0
and olp.ListOrder between {2} and {3}
  order by olp.ListOrder ASC";

        /// <summary>
        /// 获得首页的热评商品
        /// </summary>
        /// <returns></returns>
        public static List<FrontDsiplayProduct> GetHomeBestSaledProducts(int startIndex, int endIndex)
        {
            int onlineAreaType = (int)AppEnum.OnlineAreaType.HomePage;
            int onlineRecommendType = (int)AppEnum.OnlineRecommendType.PromotionTopic;
            string sqlCmd = String.Format(getHomePromotionProductsSqlCmdTemplate, onlineAreaType, onlineRecommendType, startIndex, endIndex);
            DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
            int count = data.Rows.Count;
            if (count > 0)
            {
                List<FrontDsiplayProduct> products = new List<FrontDsiplayProduct>();
                for (int i = 0; i < count; i++)
                {
                    products.Add(new FrontDsiplayProduct()
                    {
                        ProductSysNo = data.Rows[i]["ProductSysNo"].ToString().Trim(),
                        ProductPromotionWord = data.Rows[i]["PromotionWord"].ToString().Trim(),
                        Price = data.Rows[i]["price"].ToString().Trim(),
                        ImgPath = data.Rows[i]["product_simg"].ToString().Trim(),
                        C1SysNo = int.Parse(data.Rows[i]["C1SysNo"].ToString().Trim()),
                        C2SysNo = int.Parse(data.Rows[i]["C2SysNo"].ToString().Trim()),
                        C3SysNo = int.Parse(data.Rows[i]["C3SysNo"].ToString().Trim()),
                        BaiscPrice = data.Rows[i]["basicPrice"].ToString().Trim(),
                        ProductBriefName = data.Rows[i]["BriefName"].ToString().Trim(),
                    });
                }
                return products;
            }
            else
            {
                return null;
            }
        }
    }

    public class HomePromotionBrandsService
    {
        private static readonly string getHomePromotionBrandsC3InfoSqlCmd = @"select distinct top 3 c3.C3Name,p.C1SysNo,p.C2SysNo,p.C3SysNo from OnlineListProduct op
  left join Product p on op.ProductSysNo=p.SysNo
  left join Product_Price pp on op.ProductSysNo=pp.ProductSysNo
  left join Category3 c3 on p.C3SysNo=c3.SysNo
  where p.Status=1
  and c3.Status=0
  and op.OnlineAreaType={0}
  and op.CategorySysNo=0
  and op.OnlineRecommendType={1}";

        private static readonly string getHomePromotionBrandsProductSqlCmd = @"select distinct op.ProductSysNo,p.BriefName,p.PromotionWord,
  p.C1SysNo,p.C2SysNo,p.C3SysNo,CONVERT(float,pp.BasicPrice) as baiscPrice,CONVERT(float,pp.CurrentPrice) as price,
  pimgs.product_limg,op.ListOrder
  from OnlineListProduct op
  left join Product p on op.ProductSysNo=p.SysNo
  left join Product_Brand pb on op.ProductSysNo=pb.ProductSysNo
  left join Product_Price pp on op.ProductSysNo=pp.ProductSysNo
  left join Category3 c3 on p.C3SysNo=c3.SysNo
  left join Product_Images pimgs on op.ProductSysNo=pimgs.product_sysNo
  where p.Status=1
  and c3.Status=0
  and op.OnlineAreaType={0}
  and op.CategorySysNo=0
  and op.OnlineRecommendType={1}
  and pimgs.orderNum=1
  and pimgs.status=1
  and p.C3SysNo={2} order by op.ListOrder ASC";

        /// <summary>
        /// 获得首页品牌推荐的商品种类信息
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns></returns>
        public static List<HomePromotionBrandsC3Info> GetHomePromptionBrandsC3Info()
        {
            int onlineAreaType = (int)AppEnum.OnlineAreaType.HomePage;
            int onlineRecommendType = (int)AppEnum.OnlineRecommendType.PromotionBrand;
            string sqlCmd = String.Format(getHomePromotionBrandsC3InfoSqlCmd, onlineAreaType, onlineRecommendType);
            DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
            int count = data.Rows.Count;
            if (count > 0)
            {
                List<HomePromotionBrandsC3Info> c3Info = new List<HomePromotionBrandsC3Info>();
                for (int i = 0; i < count; i++)
                {
                    c3Info.Add(new HomePromotionBrandsC3Info()
                    {
                        C3Name = data.Rows[i]["C3Name"].ToString().Trim(),
                        C1SysNo = int.Parse(data.Rows[i]["C1SysNo"].ToString().Trim()),
                        C2SysNo = int.Parse(data.Rows[i]["C2SysNo"].ToString().Trim()),
                        C3SysNo = int.Parse(data.Rows[i]["C3SysNo"].ToString().Trim()),
                    });
                }
                return c3Info;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 推荐品牌的商品
        /// </summary>
        /// <param name="brandId"></param>
        /// <param name="c3SysNo"></param>
        /// <returns></returns>
        public static List<FrontDsiplayProduct> GetHomePromotionBrandsProducts(int c3SysNo)
        {
            int onlineAreaType = (int)AppEnum.OnlineAreaType.HomePage;
            int onlineRecommendType = (int)AppEnum.OnlineRecommendType.PromotionBrand;
            string sqlCmd = String.Format(getHomePromotionBrandsProductSqlCmd, onlineAreaType, onlineRecommendType, c3SysNo);
            DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
            int count = data.Rows.Count;
            if (count > 0)
            {
                List<FrontDsiplayProduct> products = new List<FrontDsiplayProduct>();
                for (int i = 0; i < count; i++)
                {
                    products.Add(new FrontDsiplayProduct()
                    {
                        ProductSysNo = data.Rows[i]["ProductSysNo"].ToString().Trim(),
                        ProductPromotionWord = data.Rows[i]["PromotionWord"].ToString().Trim(),
                        Price = data.Rows[i]["price"].ToString().Trim(),
                        ImgPath = data.Rows[i]["product_limg"].ToString().Trim(),
                        C1SysNo = int.Parse(data.Rows[i]["C1SysNo"].ToString().Trim()),
                        C2SysNo = int.Parse(data.Rows[i]["C2SysNo"].ToString().Trim()),
                        C3SysNo = int.Parse(data.Rows[i]["C3SysNo"].ToString().Trim()),
                        BaiscPrice = data.Rows[i]["baiscPrice"].ToString().Trim(),
                        ProductBriefName = data.Rows[i]["BriefName"].ToString().Trim(),
                    });
                }
                return products;
            }
            else
            {
                return null;
            }
        }
    }

    public class SearchHotCommentedProductService
    {
        private static readonly string getSearchHotCommentedSqlCmdTemplate = @"select top 5 olp.ProductSysNo,p.PromotionWord,CONVERT(float,pp.CurrentPrice)as price,
  p.C1SysNo,p.C2SysNo,p.C3SysNo,pimg.product_simg,p.BriefName,CONVERT(float,pp.BasicPrice) as baiscPrice
  from OnlineListProduct olp
  left join Product p on olp.ProductSysNo=p.SysNo
  left join Product_Price pp on olp.ProductSysNo=pp.ProductSysNo
  left join Product_Images pimg on olp.ProductSysNo=pimg.product_sysNo
  where olp.CategorySysNo=0
  and p.Status=1
  and olp.OnlineAreaType={0}
  and olp.OnlineRecommendType={1}
  and pimg.status=1
  and pimg.orderNum=1
  order by olp.ListOrder";

        /// <summary>
        /// 搜索页 产品热评
        /// </summary>
        /// <returns></returns>
        public static List<FrontDsiplayProduct> GetSearchHotCommentedProduct()
        {

            string sqlCmd = String.Format(getSearchHotCommentedSqlCmdTemplate, (int)AppEnum.OnlineAreaType.Search, (int)AppEnum.OnlineRecommendType.PowerfulSale);
            try
            {
                DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
                int count = data.Rows.Count;
                if (count > 0)
                {
                    List<FrontDsiplayProduct> products = new List<FrontDsiplayProduct>();
                    for (int i = 0; i < count; i++)
                    {
                        products.Add(new FrontDsiplayProduct()
                        {
                            C1SysNo = int.Parse(data.Rows[i]["C1SysNo"].ToString().Trim()),
                            C2SysNo = int.Parse(data.Rows[i]["C2SysNo"].ToString().Trim()),
                            C3SysNo = int.Parse(data.Rows[i]["C3SysNo"].ToString().Trim()),
                            Price = data.Rows[i]["price"].ToString().Trim(),
                            ProductSysNo = data.Rows[i]["ProductSysNo"].ToString().Trim(),
                            ProductPromotionWord = data.Rows[i]["PromotionWord"].ToString().Trim(),
                            ImgPath = data.Rows[i]["product_simg"].ToString().Trim(),
                            BaiscPrice = data.Rows[i]["baiscPrice"].ToString().Trim(),
                            ProductBriefName = data.Rows[i]["BriefName"].ToString().Trim(),
                        });
                    };
                    return products;
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

    public class BrandProductService
    {
        private static readonly string getBrandProductC1SysNoSqlCmdTemplate = @"select top 1 p.C1SysNo from Product_Brand pb
left join Product p on pb.ProductSysNo=p.SysNo
 left join Category1 c1 on p.C1SysNo=c1.SysNo
 left join Brand b on p.C1SysNo=b.C1SysNo
 where p.Status=1 
 and c1.Status=0
 and pb.BrandSysNo={0}";

        private static readonly string getBrandProductC3NamesSqlCmdTemplate = @" select distinct p.C1SysNo,p.C2SysNo,p.C3SysNo,c3.C3Name from Product_Brand pb 
 left join Product p on pb.ProductSysNo=p.SysNo
 left join Category3 c3 on p.C3SysNo=c3.SysNo
 left join Brand b on p.C1SysNo=b.C1SysNo
 where p.Status=1
 and c3.Status=0
and pb.BrandSysNo={0}";

        private static readonly string getBrandProductC3TotalCount1SqlCmdTemplate = @"select COUNT(distinct p.SysNo)as totalCount from Product_Brand pb 
left join Product p on pb.ProductSysNo=p.SysNo
 where p.Status=1 and 
 pb.BrandSysNo={0}";

        private static readonly string geBrandProducts1SqlCmdTemplate = @"select distinct p.SysNo,p.ProductName,p.PromotionWord,CONVERT(float,pp.CurrentPrice) as price,pimg.product_limg,p.C1SysNo,p.C2SysNo,p.C3SysNo,p.BriefName,CONVERT(float,pp.BasicPrice) as baiscPrice,p.IsCanPurchase,p.CreateTime,pp.LimitedQty from Product p
  left join Product_Price pp on p.SysNo=pp.ProductSysNo
  left join Product_Images pimg on p.SysNo=pimg.product_sysNo 
  left join Product_Brand pb on pb.ProductSysNo=p.SysNo
  where p.Status=1  
  and (pimg.orderNum=1 and pimg.status=1) 
  and pb.BrandSysNo={0} {1} {2}";

        private static readonly string getBrandProductC3TotalCount2SqlCmdTemplate = @"select COUNT(distinct p.SysNo)as totalCount from Product_Brand pb
    left join Product_Attribute2 pa2 on pb.ProductSysNo=pa2.ProductSysNo
    left join Product p on pb.ProductSysNo=p.SysNo
  where p.Status=1
  {0}
  and pb.BrandSysNo={1}
  and p.C3SysNo={2}";

        private static readonly string geBrandProducts2SqlCmdTemplate = @"select distinct p.SysNo,p.ProductName,p.PromotionWord,CONVERT(float,pp.CurrentPrice) as price,pimg.product_limg,p.C1SysNo,p.C2SysNo,p.C3SysNo,p.BriefName,CONVERT(float,pp.BasicPrice) as baiscPrice,p.IsCanPurchase,p.CreateTime,pp.LimitedQty from Product p
  left join Product_Price pp on p.SysNo=pp.ProductSysNo
  left join Product_Images pimg on p.SysNo=pimg.product_sysNo 
  left join Product_Brand pb on pb.ProductSysNo=p.SysNo
  where p.Status=1  
  and (pimg.orderNum=1 and pimg.status=1) 
  {0}
  and pb.BrandSysNo={1}
  and p.C3Sysno={2} {3} {4}";

        /// <summary>
        /// 获得品牌商品的大类ID
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        public static int GetBrandProductC1SysNo(int bId)
        {
            string sqlCmd = String.Format(getBrandProductC1SysNoSqlCmdTemplate, bId);
            try
            {
                DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {
                    return int.Parse(data.Rows[0]["C1SysNo"].ToString().Trim());
                }
                else
                {
                    return int.Parse(new SqlDBHelper().ExecuteQuery(@"select top 1 c1.SysNo from Category1 c1
 where c1.Status=0").Rows[0]["SysNo"].ToString().Trim());
                }
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获得品牌商品的三类名称
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        public static List<C3ProductSerach1Filter> GetBrandProductC3Names(int bId)
        {
            string sqlCmd = String.Format(getBrandProductC3NamesSqlCmdTemplate, bId);
            try
            {
                DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {
                    List<C3ProductSerach1Filter> filters = new List<C3ProductSerach1Filter>();
                    for (int j = 0; j < rowCount; j++)
                    {
                        filters.Add(new C3ProductSerach1Filter()
                        {
                            C1SysNo = int.Parse(data.Rows[j]["C1SysNo"].ToString().Trim()),
                            C2SysNo = int.Parse(data.Rows[j]["C2SysNo"].ToString().Trim()),
                            C3SysNo = int.Parse(data.Rows[j]["C3SysNo"].ToString().Trim()),
                            C3Name = data.Rows[j]["C3Name"].ToString().Trim(),
                        });
                    }
                    return filters;
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
        /// 获得所有品牌商品的数量
        /// </summary>
        /// <param name="bId"></param>
        /// <returns></returns>
        public static int GetBrandC3ProductTotalCount1(int bId)
        {
            string sqlCmd = String.Format(getBrandProductC3TotalCount1SqlCmdTemplate, bId);
            try
            {
                DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {
                    return int.Parse(data.Rows[0]["totalCount"].ToString().Trim());
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获得所有的品牌商品
        /// </summary>
        /// <param name="orderByOption"></param>
        /// <param name="bId"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static List<FrontDsiplayProduct> GetBrandProductsList1(YoeJoyEnum.ProductListSortedOrder orderByOption, int bId, string order = "DESC")
        {
            string orderByStr = YoeJoySystemDic.ProductListSortedOrderDic[orderByOption];
            string orderByStr1 = orderByStr;
            switch (orderByOption)
            {
                case YoeJoyEnum.ProductListSortedOrder.Default:
                    {
                        break;
                    }
                case YoeJoyEnum.ProductListSortedOrder.Price:
                    {
                        orderByStr1 = orderByStr1.Replace("price", "pp.CurrentPrice");
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            string sqlCmd = String.Format(geBrandProducts1SqlCmdTemplate, bId, orderByStr, order);
            try
            {
                DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {
                    List<FrontDsiplayProduct> products = new List<FrontDsiplayProduct>();
                    for (int i = 0; i < rowCount; i++)
                    {
                        products.Add(new FrontDsiplayProduct()
                        {
                            C1SysNo = int.Parse(data.Rows[i]["C1SysNo"].ToString().Trim()),
                            C2SysNo = int.Parse(data.Rows[i]["C2SysNo"].ToString().Trim()),
                            C3SysNo = int.Parse(data.Rows[i]["C3SysNo"].ToString().Trim()),
                            ImgPath = data.Rows[i]["product_limg"].ToString().Trim(),
                            Price = data.Rows[i]["price"].ToString().Trim(),
                            ProductSysNo = data.Rows[i]["SysNo"].ToString().Trim(),
                            ProductPromotionWord = data.Rows[i]["PromotionWord"].ToString().Trim(),
                            ProductBriefName = data.Rows[i]["BriefName"].ToString().Trim(),
                            BaiscPrice = data.Rows[i]["baiscPrice"].ToString().Trim(),
                            IsCanPurchase = (int.Parse(data.Rows[i]["IsCanPurchase"].ToString().Trim()) == 1) ? true : false,
                            LimitQty = int.Parse(data.Rows[i]["LimitedQty"].ToString().Trim()),
                        });
                    }
                    return products;
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
        /// 获得品牌商品的一个1个三类商品总数
        /// </summary>
        /// <param name="c3SysNo"></param>
        /// <param name="attribution2IdStr"></param>
        /// <param name="keyWords"></param>
        /// <returns></returns>
        public static int getBrandProductC3TotalCount2(int c3SysNo, string attribution2IdStr, int bId)
        {
            string arrtibutionFilterSqlCmd = String.Empty;
            if (attribution2IdStr != null)
            {
                arrtibutionFilterSqlCmd = "and pa2.Attribute2OptionSysNo in ( " + attribution2IdStr + " )";
            }
            string sqlCmd = String.Format(getBrandProductC3TotalCount2SqlCmdTemplate, arrtibutionFilterSqlCmd, bId, c3SysNo);
            try
            {
                DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {
                    return int.Parse(data.Rows[0]["totalCount"].ToString().Trim());
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获得品牌商品的一个1个三类商品
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="pagedCount"></param>
        /// <param name="c3SysNo"></param>
        /// <param name="orderByOption"></param>
        /// <param name="attribution2IdStr"></param>
        /// <returns></returns>
        public static List<FrontDsiplayProduct> GetBrandProductsList2(int c3SysNo, YoeJoyEnum.ProductListSortedOrder orderByOption, string attribution2IdStr, int bId, string order = "DESC")
        {
            string orderByStr = YoeJoySystemDic.ProductListSortedOrderDic[orderByOption];
            string orderByStr1 = orderByStr;
            string arrtibutionFilterSqlCmd = String.Empty;
            if (attribution2IdStr != null)
            {
                arrtibutionFilterSqlCmd = "and pa2.Attribute2OptionSysNo in ( " + attribution2IdStr + " )";
            }
            switch (orderByOption)
            {
                case YoeJoyEnum.ProductListSortedOrder.Default:
                    {
                        break;
                    }
                case YoeJoyEnum.ProductListSortedOrder.Price:
                    {
                        orderByStr1 = orderByStr1.Replace("price", "pp.CurrentPrice");
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            string sqlCmd = String.Format(geBrandProducts2SqlCmdTemplate, arrtibutionFilterSqlCmd, bId, c3SysNo, orderByStr, order);
            try
            {
                DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {
                    List<FrontDsiplayProduct> products = new List<FrontDsiplayProduct>();
                    for (int i = 0; i < rowCount; i++)
                    {
                        products.Add(new FrontDsiplayProduct()
                        {
                            C1SysNo = int.Parse(data.Rows[i]["C1SysNo"].ToString().Trim()),
                            C2SysNo = int.Parse(data.Rows[i]["C2SysNo"].ToString().Trim()),
                            C3SysNo = int.Parse(data.Rows[i]["C3SysNo"].ToString().Trim()),
                            ImgPath = data.Rows[i]["product_limg"].ToString().Trim(),
                            Price = data.Rows[i]["price"].ToString().Trim(),
                            ProductSysNo = data.Rows[i]["SysNo"].ToString().Trim(),
                            ProductPromotionWord = data.Rows[i]["PromotionWord"].ToString().Trim(),
                            ProductBriefName = data.Rows[i]["BriefName"].ToString().Trim(),
                            BaiscPrice = data.Rows[i]["baiscPrice"].ToString().Trim(),
                            IsCanPurchase = (int.Parse(data.Rows[i]["IsCanPurchase"].ToString().Trim()) == 1) ? true : false,
                            LimitQty = int.Parse(data.Rows[i]["LimitedQty"].ToString().Trim()),
                        });
                    }
                    return products;
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
