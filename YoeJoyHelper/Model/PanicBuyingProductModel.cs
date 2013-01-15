using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Configuration;

namespace YoeJoyHelper.Model
{
    public class PanicBuyingProductModelForHome
    {
        public string ProductSysNo { get; set; }
        public string PromotionWord { get; set; }
        public int ProductPrice { get; set; }
        public DateTime EndTime { get; set; }
        public string CoverImg { get; set; }
        public int C1SysNo { get; set; }
        public int C2SysNo { get; set; }
        public int C3SysNo { get; set; }
        public string BriefName { get; set; }
        public string BaiscPrice { get; set; }
    }

    public class PanicBuyingProductService
    {
        private static readonly string GetHomeLastedPanicProductSqlCmdTemplate = @"select top 2 sc.ProductSysNo,p.ProductName,p.PromotionWord,p.PromotionWord,CONVERT(float,pp.CurrentPrice)as price,pis.product_limg,sc.EndTime,p.C1SysNo,p.C2SysNo,p.C3SysNo,p.BriefName,CONVERT(float,pp.BasicPrice) as basicPrice from Sale_CountDown sc left join 
  Product p on sc.ProductSysNo=p.SysNo left join
  Product_Price pp on sc.ProductSysNo=pp.ProductSysNo left join 
  Product_Images pis on sc.ProductSysNo=pis.product_sysNo
  where sc.Status=1 and pis.status=1 and pis.orderNum=1 and p.Status=1 order by sc.SysNo DESC";

        public static List<PanicBuyingProductModelForHome> GetHomePanicProduct()
        {
            DataTable data = new SqlDBHelper().ExecuteQuery(GetHomeLastedPanicProductSqlCmdTemplate);
            int rowCount = data.Rows.Count;
            if (rowCount > 0)
            {
                List<PanicBuyingProductModelForHome> panicProducts = new List<PanicBuyingProductModelForHome>();
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    panicProducts.Add(new PanicBuyingProductModelForHome()
                    {
                        ProductSysNo = data.Rows[i]["ProductSysNo"].ToString().Trim(),
                        PromotionWord = data.Rows[i]["PromotionWord"].ToString().Trim(),
                        C1SysNo = int.Parse(data.Rows[i]["C1SysNo"].ToString().Trim()),
                        C2SysNo = int.Parse(data.Rows[i]["C2SysNo"].ToString().Trim()),
                        C3SysNo = int.Parse(data.Rows[i]["C3SysNo"].ToString().Trim()),
                        ProductPrice = int.Parse(data.Rows[i]["price"].ToString().Trim()),
                        EndTime = DateTime.Parse(data.Rows[i]["EndTime"].ToString().Trim()),
                        CoverImg = data.Rows[i]["product_limg"].ToString().Trim(),
                        BaiscPrice = data.Rows[i]["basicPrice"].ToString().Trim(),
                        BriefName = data.Rows[i]["BriefName"].ToString().Trim(),
                    });
                }
                return panicProducts;
            }
            else
            {
                return null;
            }
        }
    }
}
