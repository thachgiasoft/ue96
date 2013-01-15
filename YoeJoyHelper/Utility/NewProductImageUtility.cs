using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Configuration;

namespace YoeJoyHelper
{
    public struct ProductImgRecord
    {
        public string ProductSysNo;
        public string ProductName;
        public string Id;
        public string LargeImg;
        public string SmallImg;
        public int Status;
        public int OrderNum;
    }
    /// <summary>
    /// 用于上传图片的类
    /// </summary>
    public class ProductImage
    {
        public string LargeImg { get; set; }
        public string SmallImg { get; set; }
    }

    public class NewProductImageUtility
    {
        private static readonly string AddProductImgSqlCmdTemplate = @"INSERT INTO [mmbuy].[dbo].[Product_Images]
           ([product_sysNo]
           ,[product_limg]
           ,[status]
           ,[product_simg]
           ,[orderNum])
     VALUES
            ({0}
           ,'{1}'
           ,{2}
           ,'{3}'
           ,{4})";

        private static readonly string GetProductImgsSqlCmdTemplate = @"select p.ProductName,pimg.id,pimg.product_limg,pimg.product_simg,
            pimg.orderNum,pimg.status 
            from Product_Images pimg left join Product p on pimg.product_sysNo=p.SysNo 
            where pimg.product_sysNo={0} order by pimg.orderNum";

        private static readonly string GetSingleImgRecordSqlCmdTemplate = @"select p.SysNo,p.ProductName,pimg.id,pimg.product_limg,pimg.product_simg,
            pimg.orderNum,pimg.status 
            from Product_Images pimg left join Product p on pimg.product_sysNo=p.SysNo 
            where pimg.id={0}";

        private static readonly string UpdateProductImgSqlCmdTemplate = @"UPDATE [mmbuy].[dbo].[Product_Images]
   SET [product_limg] = '{0}'
      ,[status] = {1}
      ,[product_simg] = '{2}'
      ,[orderNum] = {3}
 WHERE [id]={4}";



        public static Dictionary<int, string> BindImgStatus()
        {
            Dictionary<int, string> statusDic = new Dictionary<int, string>();
            statusDic.Add(0, "无效");
            statusDic.Add(1, "有效");
            return statusDic;
        }

        /// <summary>
        /// 添加商品图片
        /// </summary>
        /// <param name="producySysNo"></param>
        /// <param name="LImgPath"></param>
        /// <param name="SImgPath"></param>
        /// <param name="orderNum"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static bool AddProductImg(string producySysNo, string LImgPath, string SImgPath, int orderNum, int status)
        {
            string sqlCmd = String.Format(AddProductImgSqlCmdTemplate, producySysNo, LImgPath, status, SImgPath, orderNum);
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
        /// 获取当前商品的所有图片
        /// </summary>
        /// <param name="productSysNo"></param>
        /// <returns></returns>
        public static DataTable GetProductImgs(string productSysNo)
        {
            string sqlCmd = String.Format(GetProductImgsSqlCmdTemplate, productSysNo);
            DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
            return data;
        }

        /// <summary>
        /// 获取单条图片记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ProductImgRecord GetSingleImgRecord(string id)
        {
            DataTable data = new SqlDBHelper().ExecuteQuery(String.Format(GetSingleImgRecordSqlCmdTemplate, id));
            ProductImgRecord record = new ProductImgRecord()
            {
                ProductSysNo = data.Rows[0]["SysNo"].ToString(),
                Id = data.Rows[0]["id"].ToString(),
                ProductName = data.Rows[0]["ProductName"].ToString().Trim(),
                LargeImg = data.Rows[0]["product_limg"].ToString().Trim(),
                SmallImg = data.Rows[0]["product_simg"].ToString().Trim(),
                OrderNum = int.Parse(data.Rows[0]["OrderNum"].ToString().Trim()),
                Status = int.Parse(data.Rows[0]["Status"].ToString().Trim()),
            };
            return record;
        }

        /// <summary>
        /// 修改单条图片记录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="LImgPath"></param>
        /// <param name="SImgPath"></param>
        /// <param name="status"></param>
        /// <param name="orderNum"></param>
        /// <returns></returns>
        public static bool UpdateProductImg(string id, string LImgPath, string SImgPath, int status, int orderNum)
        {
            try
            {
                if (new SqlDBHelper().ExecuteNonQuery(String.Format(UpdateProductImgSqlCmdTemplate, LImgPath, status, SImgPath, orderNum,id)) > 0)
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

    }
}