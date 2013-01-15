using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections;

namespace YoeJoyHelper.Model
{
    //关联类型
    public enum ProductMappingType : int
    {
        [Description("同级关联")]
        TheSameLevel = 1,
        [Description("主子关联")]
        MasterLevel = 2,
    }

    /// <summary>
    /// 商品关联模型
    /// 允许多对多的类型
    /// </summary>
    public class ProductMappingModel
    {
        public string MappingId { get; set; }
        //商品ID
        public int P1SysNo { get; set; }
        //关联商品ID
        public int P2SysNo { get; set; }
        //关联类型
        public ProductMappingType MappingType { get; set; }
        //关联索引
        public string MappingIndex { get; set; }
    }


    public class ProductMappingService
    {

        private static readonly string getPMInfoSqlCmdTemplate = @"select pm.MId,p.ProductName,pm.M_Index from Product_Mapping pm 
  left join Product p on pm.P2SysNo=p.SysNo
  where pm.P1SysNo={0} and pm.M_Type={1} {2}";

        private static readonly string removeProductMappingSqlCmdTemplate = @"delete from [mmbuy].[dbo].[Product_Mapping] where [MId]={0}";

        private static readonly string getRelatedProductSysnoFromC2SqlCmdTemplate = @"select p.SysNo from Product p 
  where p.C3SysNo<>{0} and p.Status=1 and p.C2SysNo={1}";

        private static readonly string getRelatedProductSysnoFromC3SqlCmdTemplate = @"select p.SysNo from Product p 
  where p.SysNo<>{0} and p.Status=1 and p.C3SysNo={1}";

        private static readonly string getRelatedProductSqlCmdTemplate = @"select top {0} p.SysNo,p.PromotionWord,CONVERT(float,pp.CurrentPrice) as price,pimg.product_limg,p.C2SysNo,p.C3SysNo,p.BriefName,CONVERT(float,pp.BasicPrice) as baiscPrice from Product p
  left join Product_Price pp on p.SysNo=pp.ProductSysNo 
  left join Product_Images pimg on p.SysNo=pimg.product_sysNo
  where p.Status=1 and p.SysNo in ({1}) and pimg.orderNum=1 and pimg.status=1";

        /// <summary>
        /// 添加商品关联
        /// </summary>
        /// <param name="mappingModle"></param>
        /// <returns></returns>
        public static bool AddProductMapping(ProductMappingModel mappingModle)
        {
            bool result = false;

            try
            {
                SqlParameter[] paras = new SqlParameter[5];
                paras[0] = new SqlParameter("@Product1SysNo", SqlDbType.Int, 4);
                paras[0].Value = mappingModle.P1SysNo;
                paras[0].Direction = ParameterDirection.Input;
                paras[1] = new SqlParameter("@Product2SysNo", SqlDbType.Int, 4);
                paras[1].Value = mappingModle.P2SysNo;
                paras[1].Direction = ParameterDirection.Input;
                paras[2] = new SqlParameter("@MappingType", SqlDbType.Int, 4);
                paras[2].Value = (int)mappingModle.MappingType;
                paras[2].Direction = ParameterDirection.Input;
                paras[3] = new SqlParameter("@MappingIndex", SqlDbType.VarChar, 8000);
                paras[3].Value = mappingModle.MappingIndex;
                paras[3].Direction = ParameterDirection.Input;
                paras[4] = new SqlParameter("@result", SqlDbType.Bit, 1);
                paras[4].Direction = ParameterDirection.Output;
                object outputValue;
                if (new SqlDBHelper().ExecuteStoredProcedure("AddProductMapping", paras, "@result", out outputValue) > 0)
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
            }
            catch
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 查询关联商品
        /// </summary>
        /// <param name="p1SysNo"></param>
        /// <param name="pMType"></param>
        /// <param name="mIndex"></param>
        /// <returns></returns>
        public static DataTable GetProductMappingInfo(int p1SysNo, ProductMappingType pMType, string mIndex)
        {
            string mIndexSerachSqlCmd = String.IsNullOrEmpty(mIndex) ? String.Empty : String.Format("and pm.M_Index like '%{0}%'", mIndex);
            string sqlCmd = String.Format(getPMInfoSqlCmdTemplate, p1SysNo, (int)pMType, mIndexSerachSqlCmd);
            return new SqlDBHelper().ExecuteQuery(sqlCmd);
        }

        /// <summary>
        /// 删除商品关联
        /// </summary>
        /// <param name="mappingId"></param>
        /// <returns></returns>
        public static bool RemoveProductMapping(string mappingId)
        {
            try
            {
                string sqlCmd = String.Format(removeProductMappingSqlCmdTemplate, mappingId);
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
        /// 在同一个子类中随机获得关联的商品(猜你喜欢)
        /// </summary>
        /// <param name="c3SysNo"></param>
        /// <param name="productSysNo"></param>
        /// <param name="topNum"></param>
        /// <returns></returns>
        public static List<FrontDsiplayProduct> GetRelatedProductFromC3(int c3SysNo, int productSysNo, int topNum = 5)
        {
            string sqlCmd1 = String.Format(getRelatedProductSysnoFromC3SqlCmdTemplate, productSysNo, c3SysNo);
            try
            {
                DataTable data1 = new SqlDBHelper().ExecuteQuery(sqlCmd1);
                int rowCount1 = data1.Rows.Count;
                if (rowCount1 > 0)
                {
                    List<int> pkList = new List<int>();
                    for (int i = 0; i < rowCount1; i++)
                    {
                        pkList.Add(int.Parse(data1.Rows[i]["SysNo"].ToString().Trim()));
                    }
                    List<int> randomPKList = new List<int>();
                    if (pkList.Count < topNum)
                    {
                        randomPKList = pkList;
                    }
                    else
                    {
                        randomPKList = GetRandomPKList(pkList, topNum);
                    }
                    string sqlCmd = String.Format(getRelatedProductSqlCmdTemplate, topNum, GetPKStr(randomPKList));
                    DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
                    int rowCount = data.Rows.Count;
                    if (rowCount > 0)
                    {
                        List<FrontDsiplayProduct> products = new List<FrontDsiplayProduct>();
                        for (int j = 0; j < rowCount; j++)
                        {
                            products.Add(new FrontDsiplayProduct()
                            {
                                ImgPath = data.Rows[j]["product_limg"].ToString().Trim(),
                                Price = data.Rows[j]["price"].ToString().Trim(),
                                ProductSysNo = data.Rows[j]["Sysno"].ToString().Trim(),
                                ProductPromotionWord = data.Rows[j]["PromotionWord"].ToString().Trim(),
                                BaiscPrice = data.Rows[j]["baiscPrice"].ToString().Trim(),
                                ProductBriefName = data.Rows[j]["BriefName"].ToString().Trim(),
                            });
                        }
                        return products;
                    }
                    else
                    {
                        return null;
                    }
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
        /// 在同一个二类中随机获得关联的商品(浏览过该商品的用户还看过)
        /// </summary>
        /// <param name="c3SysNo"></param>
        /// <param name="productSysNo"></param>
        /// <param name="topNum"></param>
        /// <returns></returns>
        public static List<FrontDsiplayProduct> GetRelatedProductFromC2(int c2SysNo, int c3SysNo, int topNum = 5)
        {
            string sqlCmd1 = String.Format(getRelatedProductSysnoFromC2SqlCmdTemplate, c3SysNo, c2SysNo);
            try
            {
                DataTable data1 = new SqlDBHelper().ExecuteQuery(sqlCmd1);
                int rowCount1 = data1.Rows.Count;
                if (rowCount1 > 0)
                {
                    List<int> pkList = new List<int>();
                    for (int i = 0; i < rowCount1; i++)
                    {
                        pkList.Add(int.Parse(data1.Rows[i]["SysNo"].ToString().Trim()));
                    }
                    List<int> randomPKList = new List<int>();
                    if (pkList.Count < topNum)
                    {
                        randomPKList = pkList;
                    }
                    else
                    {
                        randomPKList = GetRandomPKList(pkList, topNum);
                    }
                    string sqlCmd = String.Format(getRelatedProductSqlCmdTemplate, topNum, GetPKStr(randomPKList));
                    DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
                    int rowCount = data.Rows.Count;
                    if (rowCount > 0)
                    {
                        List<FrontDsiplayProduct> products = new List<FrontDsiplayProduct>();
                        for (int j = 0; j < rowCount; j++)
                        {
                            products.Add(new FrontDsiplayProduct()
                            {
                                ImgPath = data.Rows[j]["product_limg"].ToString().Trim(),
                                Price = data.Rows[j]["price"].ToString().Trim(),
                                ProductSysNo = data.Rows[j]["Sysno"].ToString().Trim(),
                                ProductPromotionWord = data.Rows[j]["PromotionWord"].ToString().Trim(),
                                C2SysNo = int.Parse(data.Rows[j]["C2SysNo"].ToString().Trim()),
                                C3SysNo = int.Parse(data.Rows[j]["C3SysNo"].ToString().Trim()),
                                BaiscPrice = data.Rows[j]["baiscPrice"].ToString().Trim(),
                                ProductBriefName = data.Rows[j]["BriefName"].ToString().Trim(),
                            });
                        }
                        return products;
                    }
                    else
                    {
                        return null;
                    }
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
        /// 获得指定长度的随机主键
        /// </summary>
        /// <param name="pkList"></param>
        /// <param name="listLength"></param>
        /// <returns></returns>
        private static List<int> GetRandomPKList(List<int> pkList, int listLength)
        {
            List<int> randomList = new List<int>();
            Random random = new Random();
            for (int i = 0; i < listLength; i++)
            {
                randomList.Add(pkList[random.Next(pkList.Count)]);
            }
            return randomList;
        }

        /// <summary>
        /// 获得主键查询条件的String
        /// </summary>
        /// <param name="randomList"></param>
        /// <param name="listLength"></param>
        /// <returns></returns>
        private static string GetPKStr(List<int> randomList)
        {
            string str = String.Empty;
            for (int i = 0; i < randomList.Count; i++)
            {
                if (i == (randomList.Count - 1))
                {
                    str += randomList[i].ToString().Trim();
                }
                else
                {
                    str = str + randomList[i].ToString().Trim() + " , ";
                }
            }
            return str;
        }

    }
}