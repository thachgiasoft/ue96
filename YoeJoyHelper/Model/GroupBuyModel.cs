using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Sql;

using YoeJoyHelper.Extension;

namespace YoeJoyHelper.Model
{
    /// <summary>
    /// 团购单的状态
    /// 初始为UnReviewed，未审批
    /// Rejected为取消团购单
    /// Overdue为过期团购单
    /// NeedMorePurchaser为尚未凑够购买人数的团购单
    /// Approved为开始团购活动的正常团购单
    /// </summary>
    public enum TicketStatus
    {
        All = 0,
        UnReviewed = 1,
        Rejected = 2,
        Overdue = 3,
        NeedMorePurchaser = 4,
        Approved = 5,
    }


    /// <summary>
    /// 团购类型定义
    /// </summary>
    public class GroupBuyTicketModel
    {
        /// <summary>
        /// 团购单ID
        /// </summary>
        public string GroupTicketId { get; set; }

        /// <summary>
        /// 参与团购地点
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 团购标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 团购图片
        /// </summary>
        public string Img { get; set; }

        /// <summary>
        /// 成交量
        /// </summary>
        public int Trunover { get; set; }

        /// <summary>
        /// 团购开始时间
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 团购截止时间
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 团购单创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 团购商品ID
        /// </summary>
        public int RelatedProductSysNo { get; set; }

        /// <summary>
        /// 团购商品总数
        /// </summary>
        public int ProductionTotalCount { get; set; }

        /// <summary>
        /// 团购单状态
        /// true表示当前团购有效
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 团购单价
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// 最低购买人数限制
        /// </summary>
        public int CountLimit { get; set; }

    }

    /// <summary>
    /// 团购商品服务类
    /// </summary>
    public class GroupBuyService
    {

        private static readonly string AddGroupBuyTicketSqlCmdTemplate = @"INSERT INTO [mmbuy].[dbo].[Product_GroupBuyTicket]
           ([location]
           ,[title]
           ,[img]
           ,[start_date]
           ,[end_date]
           ,[total_count]
           ,[product_sysNo]
           ,[price]
           ,[count_limit])
     VALUES
           ('{0}'
           ,'{1}'
           ,'{2}'
           ,'{3}'
           ,'{4}'
           ,'{5}'
           ,{6}
           ,{7}
           ,{8})";

        private static readonly string GetGroupBuyTicketsSqlCmdTemplate = @"SELECT [groupid]
      ,[location]
      ,[title]
      ,[img]
      ,[status]
      ,[total_count]
      ,[product_sysNo]
      ,[price]
      ,[count_limit]
      ,CONVERT(varchar(12),create_date,111)as create_date
      ,CONVERT(varchar(12),start_date,111) as start_date
      ,CONVERT(varchar(12),end_date,111) as end_date
  FROM [mmbuy].[dbo].[Product_GroupBuyTicket] {0} ORDER BY {1} {2}";

        private static readonly string GetTicketDetailSqlCmdTemplate = @"SELECT groupid
    ,title
    ,img
    ,location
    ,Product_GroupBuyTicket.status
    ,product_sysNo
    ,ProductName
    ,Product.ProductDescLong
    ,Product_GroupBuyTicket.price
    ,count_limit
    ,total_count
    ,AvailableQty
    ,(AllocatedQty-InitialAllocatedQty)as saledQty
    ,(total_count-(AllocatedQty-InitialAllocatedQty))as leftQty
    ,CONVERT(varchar(12),create_date,111)as create_date
    ,CONVERT(varchar(12),start_date,111) as start_date
    ,CONVERT(varchar(12),end_date,111) as end_date
    ,DATEDIFF(DAY,GETDATE(),end_date)as leftDate
    FROM Product_GroupBuyTicket LEFT JOIN Inventory ON Inventory.ProductSysNo=product_sysNo 
    LEFT JOIN Product ON Product.SysNo=product_sysNo
    WHERE
    groupid={0}";


        private static readonly string GetInitialAllocatedQtySqlCmd = @"SELECT AllocatedQty as saledBefore FROM Inventory where ProductSysNo={0}";

        private static readonly string UpdateTicketStatusSqlCmd = @"Update Product_GroupBuyTicket SET status={0} where groupid={1}";

        private static readonly string UpdateInitialAllocatedQtyAndStatusSqlCmd = @"Update Product_GroupBuyTicket SET status={0},InitialAllocatedQty={1} where groupid={2}";

        /// <summary>
        /// 添加一个团购单
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public static bool AddGroupBuyTicket(GroupBuyTicketModel ticket)
        {
            bool result = false;
            string sqlCmd = String.Format(AddGroupBuyTicketSqlCmdTemplate,
                ticket.Location,
                ticket.Title,
                ticket.Img,
                ticket.StartDate,
                ticket.EndDate,
                ticket.ProductionTotalCount,
                ticket.RelatedProductSysNo,
                ticket.Price,
                ticket.CountLimit);
            SqlDBHelper dbHelper = new SqlDBHelper();
            if (dbHelper.ExecuteNonQuery(sqlCmd) > 0)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 获取团购单
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="city"></param>
        /// <param name="productSysNo"></param>
        /// <param name="status"></param>
        /// <param name="createDate"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static DataTable GetTickets(string id, string title, string city, string productSysNo, int status, string createDate, string startDate, string endDate, string sortOrder = "groupid", string orderDire = "ASC")
        {
            string condition = "WHERE";
            if (id.IsSafeString())
            {
                condition += String.Concat(" groupid=", int.Parse(id));
            }
            if (title.IsSafeString())
            {
                condition += GroupBuyUtility.CombineMutlQueryCondtion(condition, String.Concat(" title LIKE '%", title, "%'"));
            }
            if (city.IsSafeString())
            {
                condition += GroupBuyUtility.CombineMutlQueryCondtion(condition, String.Concat(" location='", city, "'"));
            }
            if (productSysNo.IsSafeString())
            {
                condition += GroupBuyUtility.CombineMutlQueryCondtion(condition, String.Concat(" product_sysNo=", int.Parse(productSysNo)));
            }
            if (status != 0)
            {
                condition += GroupBuyUtility.CombineMutlQueryCondtion(condition, String.Concat(" status=", status));
            }
            if (createDate.IsSafeString())
            {
                condition += GroupBuyUtility.CombineMutlQueryCondtion(condition, String.Concat(" create_date LIKE '%", createDate.Trim(), "'"));
            }
            if (startDate.IsSafeString())
            {
                condition += GroupBuyUtility.CombineMutlQueryCondtion(condition, String.Concat(" start_date = '", startDate.Trim(), "'"));
            }
            if (endDate.IsSafeString())
            {
                condition += GroupBuyUtility.CombineMutlQueryCondtion(condition, String.Concat(" end_date = '", endDate.Trim(), "'"));
            }
            string finalSqlCmd = String.Format(GetGroupBuyTicketsSqlCmdTemplate, String.Empty, sortOrder, orderDire);
            if (condition != "WHERE")
            {
                finalSqlCmd = String.Format(GetGroupBuyTicketsSqlCmdTemplate, condition, sortOrder, orderDire);
            }
            SqlDBHelper dbHelper = new SqlDBHelper();
            DataTable data = dbHelper.ExecuteQuery(finalSqlCmd);
            return data;
        }

        /// <summary>
        /// 通过ID 获得团购单的详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DataTable GetTicketById(string id)
        {
            int groupId = int.Parse(id);
            string sqlCmd = String.Format(GetTicketDetailSqlCmdTemplate, groupId);
            SqlDBHelper dbHelper = new SqlDBHelper();
            DataTable data = dbHelper.ExecuteQuery(sqlCmd);
            return data;
        }

        /// <summary>
        /// 更改团购单状态
        /// 并且初始化团购单商品在库存中已经用掉的库存
        /// 所以团购单的团购销量等于团购开始活动中的已用库存量减去
        /// 团购开始时库存表中的已用库存量
        /// </summary>
        /// <param name="gid"></param>
        /// <param name="status"></param>
        /// <param name="productSysNo"></param>
        /// <returns></returns>
        public static bool UpdateTicketStatus(string gid, int status, int productSysNo)
        {
            bool result = false;
            TicketStatus statusTag = (TicketStatus)status;
            switch (statusTag)
            {
                case TicketStatus.Approved:
                    {
                        result=UpdateTicketInitialAllocatedQtyAndStatus(gid, status, productSysNo);
                        break;
                    }
                case TicketStatus.NeedMorePurchaser:
                    {
                        result=UpdateTicketStatus(gid, status);
                        break;
                    }
                case TicketStatus.Overdue:
                    {
                        result=UpdateTicketStatus(gid, status);
                        break;
                    }
                case TicketStatus.UnReviewed:
                    {
                        result=UpdateTicketStatus(gid, status);
                        break;
                    }
                case TicketStatus.Rejected:
                    {
                        result=UpdateTicketStatus(gid, status);
                        break;
                    }
                case TicketStatus.All:
                    {
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            return result;
        }

        #region

        private static bool UpdateTicketStatus(string gid, int status)
        {
            bool result = false;
            SqlDBHelper dbHelper = new SqlDBHelper();
            string updateStatusSqlTxt = String.Format(UpdateTicketStatusSqlCmd, status, gid);
            try
            {
                if (dbHelper.ExecuteNonQuery(updateStatusSqlTxt) > 0)
                {
                    result = true;
                }
            }
            catch
            {

            }
            return result;
        }

        private static bool UpdateTicketInitialAllocatedQtyAndStatus(string gid, int status, int productSysNo)
        {
            bool result = false;
            string getInitialAllocatedQtySqlTxt = String.Format(GetInitialAllocatedQtySqlCmd, productSysNo);
            SqlDBHelper dbHelper = new SqlDBHelper();
            try
            {
                DataTable data1 = dbHelper.ExecuteQuery(getInitialAllocatedQtySqlTxt);
                if (data1.Rows.Count > 0)
                {
                    int initialAllocatedQty = int.Parse(data1.Rows[0]["saledBefore"].ToString());
                    string updateInitialAllocatedQtySqlTxt = String.Format(UpdateInitialAllocatedQtyAndStatusSqlCmd, status, initialAllocatedQty, gid);
                    if (dbHelper.ExecuteNonQuery(updateInitialAllocatedQtySqlTxt) > 0)
                    {
                        result = true;
                    }
                }
            }
            catch
            {

            }
            return result;
        }

        #endregion
    }

}