using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Configuration;
using Icson.Objects;

namespace YoeJoyHelper.Model
{

    public class InComingProductForHome
    {
        public string SysNo { get; set; }
        public int Price { get; set; }
        public string PromotionWord { get; set; }
        public string ImgCover { get; set; }
        public int C1SysNo { get; set; }
        public int C2SysNo { get; set; }
        public int C3SysNo { get; set; }
        public string BaiscPrice { get; set; }
        public string BriefName { get; set; }
    }

    public class InComingProductService
    {
        private static readonly string GetHomeInComingProductSqlCmdTemplate = @" select top 4 olp.ProductSysNo,p.C1SysNo,p.C2SysNo,p.C3SysNo,p.ProductName,p.PromotionWord,CONVERT(float,pp.CurrentPrice) as price,pimg.product_limg,p.BriefName,CONVERT(float,pp.BasicPrice) as basicPrice from OnlineListProduct olp 
  left join Product p on olp.ProductSysNo=p.SysNo 
  left join Product_Price pp on olp.ProductSysNo=pp.ProductSysNo
  left join Product_Images pimg on olp.ProductSysNo=pimg.product_sysNo
  where p.Status=1 and pimg.orderNum=1 and olp.OnlineAreaType={0} and olp.OnlineRecommendType={1} and olp.CategorySysNo=0
  order by olp.ListOrder ASC";

        public static List<InComingProductForHome> GetHomeInComingProduct()
        {
            int onlineAreaType = (int)AppEnum.OnlineAreaType.HomePage;
            int onlineRecommendType = (int)AppEnum.OnlineRecommendType.NewArrive;
            string sqlCmd = String.Format(GetHomeInComingProductSqlCmdTemplate, onlineAreaType, onlineRecommendType);
            DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
            int count = data.Rows.Count;
            if (count > 0)
            {
                List<InComingProductForHome> products = new List<InComingProductForHome>();
                for (int i = 0; i < count; i++)
                {
                    products.Add(new InComingProductForHome()
                    {
                        SysNo = data.Rows[i]["ProductSysNo"].ToString().Trim(),
                        PromotionWord = data.Rows[i]["PromotionWord"].ToString().Trim(),
                        Price = int.Parse(data.Rows[i]["price"].ToString().Trim()),
                        ImgCover = data.Rows[i]["product_limg"].ToString().Trim(),
                        C1SysNo = int.Parse(data.Rows[i]["C1SysNo"].ToString().Trim()),
                        C2SysNo = int.Parse(data.Rows[i]["C2SysNo"].ToString().Trim()),
                        C3SysNo = int.Parse(data.Rows[i]["C3SysNo"].ToString().Trim()),
                        BaiscPrice = data.Rows[i]["basicPrice"].ToString().Trim(),
                        BriefName = data.Rows[i]["BriefName"].ToString().Trim(),
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

}

