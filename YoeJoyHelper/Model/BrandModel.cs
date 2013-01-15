using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace YoeJoyHelper.Model
{
    /// <summary>
    /// 品牌
    /// </summary>
    public class BrandModel
    {
        public int BrandSysNo { get; set; }
        public string BrandName { get; set; }
        public string BrandLogo { get; set; }
        public int Status { get; set; }
        public int OrderNum { get; set; }
        /// <summary>
        /// 品牌旗舰店显示顺序
        /// </summary>
        public int RecommendOrderNum { get; set; }
        /// <summary>
        /// 分类列表显示顺序
        /// </summary>
        public int PromotionOrderNum { get; set; }
        public int C1SysNo { get; set; }
        public int C2SysNo { get; set; }
        public int C3SysNo { get; set; }
        /// <summary>
        /// 是否显示在首页中间品牌旗舰店
        /// </summary>
        public int IsRecommend { get; set; }
        /// <summary>
        /// 是否显示在大类的品牌推荐中
        /// </summary>
        public int IsPromoted { get; set; }
    }

    /// <summary>
    /// 主页的品牌模型类
    /// </summary>
    public class BrandForHome
    {
        public int BrandSysNo { get; set; }
        public string BrandName { get; set; }
        public string BrandLogo { get; set; }
        public int C1SysNo { get; set; }
    }

    public class BrandService
    {

        private static readonly string AddNewBrandSqlCmdTemplate = @"INSERT INTO [mmbuy].[dbo].[Brand]
           ([BrandName]
           ,[Status]
           ,[BrandIcon]
           ,[OrderNum]
           ,[C1SysNo]
           ,[C2SysNo]
           ,[C3SysNo]
           ,[IsRecommend]
           ,[IsPromoted]
           ,[RecommendOrderNum]
           ,[PromotionOrderNum])
     VALUES
           ('{0}'
            ,{1}
           ,'{2}',{3},'{4}','{5}','{6}',{7},{8},{9},{10})";

        private static readonly string UpdateBrandSqlCmdTemplate = @"UPDATE [mmbuy].[dbo].[Brand]
   SET [BrandName] = '{0}'
      ,[Status] = {1}
      ,[BrandIcon] = '{2}'
      ,[OrderNum]={3}
      ,[C1SysNo]={4}
      ,[C2SysNo]={5}
      ,[C3SysNo]={6}
      ,[IsRecommend]={7}
      ,[IsPromoted]={8}
      ,[RecommendOrderNum]={9}
      ,[PromotionOrderNum]={10}
 WHERE [BrandSysNo]={11}";

        private static readonly string GetAllBrandsSqlCmdTemplate = @"SELECT [BrandSysNo]
      ,[BrandName]
      ,[Status]
      ,[BrandIcon]
      ,[RecommendOrderNum]
      ,[IsRecommend]
      ,[IsPromoted]
      ,[PromotionOrderNum]
      ,[OrderNum]
  FROM [mmbuy].[dbo].[Brand]";

        private static readonly string GetProductBrandSqlCmdTemplate = @"select b.BrandSysNo,BrandName,BrandIcon from Brand b left join Product_brand pb on b.BrandSysNo=pb.BrandSysNo
  where pb.ProductSysNo={0} and b.Status=1";

        private static readonly string GetValidBrandsSqlCmdTemplate = @"SELECT [BrandSysNo]
      ,[BrandName]
      ,[Status]
      ,[BrandIcon]
  FROM [mmbuy].[dbo].[Brand] WHERE [Status]=1";

        private static readonly string GetSingleBrandsSqlCmdTemplate = @"SELECT [BrandSysNo]
      ,[BrandName]
      ,[Status]
      ,[BrandIcon]
      ,[OrderNum]
      ,[C1SysNo]
      ,[C2SysNo]
      ,[C3SysNo]
      ,[IsRecommend]
      ,[IsPromoted]
      ,[RecommendOrderNum]
      ,[PromotionOrderNum]
  FROM [mmbuy].[dbo].[Brand] WHERE [BrandSysNo]={0}";

        private static readonly string GetHomeCenterBradsSqlCmdTemplate = @"  select top {0} b.BrandSysNo,b.BrandName,b.BrandIcon,b.C1SysNo,b.RecommendOrderNum from Brand b 
  where b.Status=1 and b.IsRecommend=1 order by b.RecommendOrderNum ASC";

        private static readonly string GetHomeCategoryOneBrandsSqlCmdTemplate = @"  select top 8 b.BrandSysNo,b.BrandName,b.BrandIcon,b.C1SysNo,b.OrderNum from Brand b
  where b.Status=1 and b.C1SysNo={0} order by b.OrderNum ASC";

        private static readonly string GetHomeCategoryListBrandsSqlCmdTemplate = @"select top {0} b.BrandSysNo,b.BrandName,b.PromotionOrderNum from Brand as b 
  where b.Status=1 and b.IsPromoted=1 and b.C1SysNo={1} order by b.PromotionOrderNum ASC";


        /// <summary>
        /// 添加一个新的品牌
        /// </summary>
        /// <param name="brand"></param>
        /// <returns></returns>
        public static bool AddNewBrand(BrandModel brand)
        {
            string sqlCmd = String.Format(AddNewBrandSqlCmdTemplate, brand.BrandName, brand.Status, brand.BrandLogo, brand.OrderNum, brand.C1SysNo, brand.C2SysNo, brand.C3SysNo, brand.IsRecommend, brand.IsPromoted, brand.RecommendOrderNum, brand.PromotionOrderNum);
            try
            {
                if (new SqlDBHelper().ExecuteNonQuery(sqlCmd) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 更新一个品牌
        /// </summary>
        /// <param name="brand"></param>
        /// <returns></returns>
        public static bool UpdateBrand(BrandModel brand)
        {
            string sqlCmd = String.Format(UpdateBrandSqlCmdTemplate, brand.BrandName, brand.Status, brand.BrandLogo, brand.OrderNum, brand.C1SysNo, brand.C2SysNo, brand.C3SysNo, brand.IsRecommend, brand.IsPromoted, brand.RecommendOrderNum, brand.PromotionOrderNum, brand.BrandSysNo);
            try
            {
                if (new SqlDBHelper().ExecuteNonQuery(sqlCmd) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获得当前所有的品牌
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllBrands()
        {
            DataTable data = new SqlDBHelper().ExecuteQuery(GetAllBrandsSqlCmdTemplate);
            return data;
        }

        /// <summary>
        /// 关联商品和品牌
        /// </summary>
        /// <param name="brandSysNo"></param>
        /// <param name="productSysNo"></param>
        /// <returns></returns>
        public static bool MapBrandProduct(string brandSysNo, string productSysNo)
        {
            bool result;
            SqlParameter[] paras = new SqlParameter[3];
            paras[0] = new SqlParameter("@BrandSysNo", SqlDbType.Int, 4);
            paras[0].Value = brandSysNo;
            paras[0].Direction = ParameterDirection.Input;
            paras[1] = new SqlParameter("@ProductSysNo", SqlDbType.Int, 4);
            paras[1].Value = productSysNo;
            paras[1].Direction = ParameterDirection.Input;
            paras[2] = new SqlParameter("@result", SqlDbType.Bit, 1);
            paras[2].Direction = ParameterDirection.Output;
            object outputValue;
            if (new SqlDBHelper().ExecuteStoredProcedure("MappBrandProduct", paras, "@result", out outputValue) > 0)
            {
                if ((bool)outputValue)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 获得商品的品牌
        /// </summary>
        /// <returns></returns>
        public static BrandModel GetProductBrand(string productSysNo)
        {
            DataTable data = new SqlDBHelper().ExecuteQuery(String.Format(GetProductBrandSqlCmdTemplate, productSysNo));
            if (data.Rows.Count > 0)
            {
                BrandModel brand = new BrandModel()
                {
                    BrandName = data.Rows[0]["BrandName"].ToString().Trim(),
                    BrandSysNo = int.Parse(data.Rows[0]["BrandSysNo"].ToString().Trim()),
                    BrandLogo = data.Rows[0]["BrandIcon"].ToString().Trim(),
                };
                return brand;
            }
            else
            {
                BrandModel brand = new BrandModel()
                {
                    BrandName = String.Empty,
                    BrandSysNo = 0,
                    BrandLogo = String.Empty,
                };
                return brand;
            }
        }

        /// <summary>
        /// 获得品牌信息
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns></returns>
        public static BrandModel GetBrandById(string brandId)
        {
            DataTable data = new SqlDBHelper().ExecuteQuery(String.Format(GetSingleBrandsSqlCmdTemplate, brandId));
            BrandModel brand = new BrandModel()
            {
                BrandName = data.Rows[0]["BrandName"].ToString().Trim(),
                BrandSysNo = int.Parse(data.Rows[0]["BrandSysNo"].ToString().Trim()),
                BrandLogo = data.Rows[0]["BrandIcon"].ToString().Trim(),
                Status = int.Parse(data.Rows[0]["Status"].ToString().Trim()),
                OrderNum = int.Parse(data.Rows[0]["OrderNum"].ToString().Trim()),
                C1SysNo = int.Parse(data.Rows[0]["C1SysNo"].ToString().Trim()),
                C2SysNo = int.Parse(data.Rows[0]["C2SysNo"].ToString().Trim()),
                C3SysNo = int.Parse(data.Rows[0]["C3SysNo"].ToString().Trim()),
                IsRecommend = int.Parse(data.Rows[0]["IsRecommend"].ToString().Trim()),
                IsPromoted = int.Parse(data.Rows[0]["IsPromoted"].ToString().Trim()),
                PromotionOrderNum = int.Parse(data.Rows[0]["PromotionOrderNum"].ToString().Trim()),
                RecommendOrderNum = int.Parse(data.Rows[0]["RecommendOrderNum"].ToString().Trim()),
            };
            return brand;
        }

        /// <summary>
        /// 绑定品牌下来列表
        /// </summary>
        /// <returns></returns>
        public static SortedList<int, string> GetValidBrandList()
        {
            SortedList<int, string> list = new SortedList<int, string>();
            DataTable data = new SqlDBHelper().ExecuteQuery(GetValidBrandsSqlCmdTemplate);
            for (int i = 0; i < data.Rows.Count; i++)
            {
                list.Add(int.Parse(data.Rows[i]["BrandSysNo"].ToString().Trim()), data.Rows[i]["BrandName"].ToString().Trim());
            }
            return list;
        }

        /// <summary>
        /// 获得主页中间的品牌旗舰店的商标
        /// </summary>
        /// <param name="topNum"></param>
        /// <returns></returns>
        public static List<BrandForHome> GetHomeCenterBrands(int topNum=8)
        {
            try
            {
                DataTable data = new SqlDBHelper().ExecuteQuery(String.Format(GetHomeCenterBradsSqlCmdTemplate, topNum));
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {
                    List<BrandForHome> brands = new List<BrandForHome>();
                    for (int i = 0; i < rowCount; i++)
                    {
                        brands.Add(new BrandForHome()
                        {
                            BrandName = data.Rows[i]["BrandName"].ToString().Trim(),
                            BrandSysNo = int.Parse(data.Rows[i]["BrandSysNo"].ToString().Trim()),
                            BrandLogo = data.Rows[i]["BrandIcon"].ToString().Trim(),
                            C1SysNo = int.Parse(data.Rows[i]["C1SysNo"].ToString().Trim()),
                        });
                    }
                    return brands;
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
        /// 获得主页大类展示的商品商标
        /// </summary>
        /// <param name="topNum"></param>
        /// <returns></returns>
        public static List<BrandForHome> GetCategoryOneBrands(string c1SysNo)
        {
            try
            {
                DataTable data = new SqlDBHelper().ExecuteQuery(String.Format(GetHomeCategoryOneBrandsSqlCmdTemplate, c1SysNo));
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {
                    List<BrandForHome> brands = new List<BrandForHome>();
                    for (int i = 0; i < rowCount; i++)
                    {
                        brands.Add(new BrandForHome()
                        {
                            BrandName = data.Rows[i]["BrandName"].ToString().Trim(),
                            BrandSysNo = int.Parse(data.Rows[i]["BrandSysNo"].ToString().Trim()),
                            BrandLogo = data.Rows[i]["BrandIcon"].ToString().Trim(),
                            C1SysNo = int.Parse(data.Rows[i]["C1SysNo"].ToString().Trim()),
                        });
                    }
                    return brands;
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
        /// 获得商品分类列表中的商标
        /// </summary>
        /// <param name="c1SysNo"></param>
        /// <returns></returns>
        public static List<BrandForHome> GetCategoryListBrands(string c1SysNo,int topNum=12)
        {
            try
            {
                DataTable data = new SqlDBHelper().ExecuteQuery(String.Format(GetHomeCategoryListBrandsSqlCmdTemplate,topNum,c1SysNo));
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {
                    List<BrandForHome> brands = new List<BrandForHome>();
                    for (int i = 0; i < rowCount; i++)
                    {
                        brands.Add(new BrandForHome()
                        {
                            BrandName = data.Rows[i]["BrandName"].ToString().Trim(),
                            BrandSysNo = int.Parse(data.Rows[i]["BrandSysNo"].ToString().Trim()),
                        });
                    }
                    return brands;
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
}