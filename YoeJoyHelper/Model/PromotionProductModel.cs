using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;

namespace YoeJoyHelper.Model
{
    /// <summary>
    /// 主页的促销商品
    /// </summary>
    public class PromotionProductModelForHome
    {
        public string ProductSysNo { get; set; }
        public string LargeImgPath { get; set; }
        public int ProductPrice { get; set; }
        public string PromotionWord { get; set; }
    }

    public class PromotionProductService
    {

        private static readonly string GetHomePromotionProductsSqlCmdTemplate = @"select top 4 p.PromotionWord,pm.PromotionName,pip.ProductSysNo,p.ProductName,CONVERT(float,pp.CurrentPrice) as price ,pis.product_limg from Promotion_Item_Group pig,Promotion_Item_Product pip,Promotion_Master pm,Product p,Product_Price pp,Product_Images pis
  where pm.Status=0 and pig.Status=0 and pm.SysNo=pig.PromotionSysNo and pig.SysNo=pip.PromotionItemGroupSysNo and pip.ProductSysNo=p.SysNo and pip.ProductSysNo=pp.ProductSysNo and pip.ProductSysNo=pis.product_sysNo and pis.status=1 and pis.orderNum=1 order by pip.OrderNum";

        public static List<PromotionProductModelForHome> GetHomePromotionProducts()
        {
            DataTable data = new SqlDBHelper().ExecuteQuery(GetHomePromotionProductsSqlCmdTemplate);
            int rowCount = data.Rows.Count;
            List<PromotionProductModelForHome> promos = new List<PromotionProductModelForHome>();
            for (int i = 0; i < rowCount; i++)
            {
                promos.Add(new PromotionProductModelForHome()
                {
                    PromotionWord = data.Rows[i]["PromotionWord"].ToString().Trim(),
                    LargeImgPath = data.Rows[i]["product_limg"].ToString().Trim(),
                    ProductSysNo = data.Rows[i]["ProductSysNo"].ToString().Trim(),
                    ProductPrice=int.Parse(data.Rows[i]["price"].ToString().Trim()),
                });
            }

            return promos;
        }
    }
}
