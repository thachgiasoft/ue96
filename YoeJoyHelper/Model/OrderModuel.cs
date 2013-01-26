using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;
using Icson.Objects.Basic;
using Icson.Objects.Online;
using Icson.Objects;
using Icson.Objects.Finance;
using Icson.Utils;
using Icson.BLL;
using Icson.BLL.Online;
using Icson.BLL.Sale;
using Icson.BLL.Finance;
using Icson.BLL.RMA;
using System.Data;
using System.Data.Sql;

namespace YoeJoyHelper.Model
{
    /// <summary>
    /// 订单商品模型类
    /// </summary>
    public class OrderProductModuel
    {
        public string ProductSysNo { get; set; }
        public int C1SysNo { get; set; }
        public int C2SysNo { get; set; }
        public int C3SysNo { get; set; }
        public string Cost { get; set; }
        public string ImgPath { get; set; }
        public string ProductBriefName { get; set; }
        public int Quantity { get; set; }
    }

    /// <summary>
    /// 我的订单模型类
    /// </summary>
    public class OrderModuel
    {
        public int SysNo { get; set; }
        public int SoId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalCash { get; set; }
        public int PointPay { get; set; }
        public int Pointamt { get; set; }
        public int Status { get; set; }
        public string Memo { get; set; }
        public int IsPayWhenEcv { get; set; }
        public string PagementPage { get; set; }
        public int IsNet { get; set; }
        public int DoNo { get; set; }
        public List<OrderProductModuel> ProductList { get; set; }
    }

    /// <summary>
    /// 订单服务
    /// </summary>
    public class OrderService
    {

        private static readonly string getPagedOrderListSqlCmdTemplate = @"select top {0} so_master.sysno, soid, orderdate, cashpay+premiumamt+shipprice+payprice-freeshipfeepay-DiscountAmt-CouponAmt as totalcash,
pointpay, pointamt, so_master.status, so_master.memo, ispaywhenrecv, paymentpage, isNet, doNo
from so_master inner join paytype on so_master.paytypesysno = paytype.sysno
left join do_master on so_master.sysno = do_master.sosysno
where SO_Master.SysNo not in (select top {1} sm.SysNo from SO_Master sm where sm.CustomerSysNo={2} order by sm.SysNo desc)  
and so_master.customersysno = {3} order by so_master.sysno desc";

        private static readonly string getTotalOrderListNumSqlCmdTemplate = @"select COUNT (so_master.sysno) as totalCount from so_master where so_master.customersysno ={0}";

        private static readonly string getOrderProductListSqlCmdTemplate = @"select si.ProductSysNo,CONVERT(float,si.Cost) as cost,si.Quantity,p.BriefName,pimgs.product_simg,p.C1SysNo,p.C2SysNo,p.C3SysNo,p.BriefName,p.PromotionWord from SO_Item si
left join Product p on si.ProductSysNo=p.SysNo
left join Product_Images pimgs on si.ProductSysNo=pimgs.product_sysNo 
where pimgs.orderNum=1 and pimgs.status=1 and si.SOSysNo={0}";

        /// <summary>
        /// 获得用户的订单总数
        /// </summary>
        /// <param name="customerSysno"></param>
        /// <returns></returns>
        public static int GetOrderListTotalNum(int customerSysno)
        {
            try
            {
                DataTable data = new SqlDBHelper().ExecuteQuery(String.Format(getTotalOrderListNumSqlCmdTemplate, customerSysno));
                if(data.Rows.Count>0)
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
        /// 获取用户的订单和订单商品
        /// </summary>
        /// <param name="customerSysno"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        public static List<OrderModuel> GetMyOrder(int customerSysno, int pageIndex, int pageCount)
        {
            try
            {
                SqlDBHelper dbHelper = new SqlDBHelper();
                string sql1 = String.Format(getPagedOrderListSqlCmdTemplate, pageCount, customerSysno, pageIndex, customerSysno);
                DataTable data1 = dbHelper.ExecuteQuery(sql1);
                if (data1.Rows.Count > 0)
                {
                    int rowCount1 = data1.Rows.Count;
                    List<OrderModuel> orderList=new List<OrderModuel>();
                    for (int i = 0; i < rowCount1; i++)
                    {
                        orderList.Add(new OrderModuel()
                        {
                            SysNo=int.Parse(data1.Rows[i]["sysno"].ToString().Trim()),
                            SoId = int.Parse(data1.Rows[i]["soid"].ToString().Trim()), 
                            OrderDate = DateTime.Parse(data1.Rows[i]["orderdate"].ToString().Trim()),
                            TotalCash =decimal.Parse(data1.Rows[i]["totalcash"].ToString().Trim()),
                            Status = int.Parse(data1.Rows[i]["status"].ToString().Trim()),
                            Memo = data1.Rows[i]["memo"].ToString().Trim(),
                            PointPay = int.Parse(data1.Rows[i]["pointpay"].ToString().Trim()),
                            Pointamt = int.Parse(data1.Rows[i]["pointamt"].ToString().Trim()),
                            IsNet = int.Parse(data1.Rows[i]["isNet"].ToString().Trim()),
                            PagementPage = data1.Rows[i]["paymentpage"].ToString().Trim(),
                            IsPayWhenEcv = int.Parse(data1.Rows[i]["ispaywhenrecv"].ToString().Trim()),
                            DoNo = data1.Rows[i]["doNo"].Equals(DBNull.Value)?0:int.Parse(data1.Rows[i]["paymentpage"].ToString().Trim()),
                        });
                    }
                    foreach (var orderListItem in orderList)
                    {
                        string sql2 = String.Format(getOrderProductListSqlCmdTemplate, orderListItem.SysNo);
                        DataTable data2 = dbHelper.ExecuteQuery(sql2);
                        orderListItem.ProductList = new List<OrderProductModuel>();
                        if (data2.Rows.Count > 0)
                        {
                            int rowCount2 = data2.Rows.Count;
                            for(int j=0;j<rowCount2;j++)
                            {
                                orderListItem.ProductList.Add(new OrderProductModuel()
                                {
                                    ProductSysNo = data2.Rows[j]["ProductSysNo"].ToString().Trim(),
                                    Cost = data2.Rows[j]["cost"].ToString().Trim(),
                                    C1SysNo = int.Parse(data2.Rows[j]["C1SysNo"].ToString().Trim()),
                                    C2SysNo = int.Parse(data2.Rows[j]["C2SysNo"].ToString().Trim()),
                                    C3SysNo = int.Parse(data2.Rows[j]["C3SysNo"].ToString().Trim()),
                                    ImgPath = data2.Rows[j]["product_simg"].ToString().Trim(),
                                    Quantity = int.Parse(data2.Rows[j]["Quantity"].ToString().Trim()),
                                    ProductBriefName = data2.Rows[j]["BriefName"].ToString().Trim(),
                                });
                            }
                        }
                    }
                    return orderList;
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
