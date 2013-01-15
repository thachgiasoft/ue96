using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Data.Sql;
using System.Data;

using Icson.Utils;
using Icson.Objects;
using Icson.Objects.Promotion;
using Icson.BLL.Promotion;
using YoeJoyHelper.Extension;

namespace YoeJoyHelper
{
    public class NewPromotionUtility
    {
        private static readonly string GetPromotionGroupSqlCmdTemplate = @"SELECT [SysNo]
      ,[PromotionSysNo]
      ,[GroupName]
      ,[OrderNum]
      ,[Status]
  FROM [mmbuy].[dbo].[Promotion_Item_Group] {0}";

        private static readonly string GetGroupProductsSqlCmdTemplate = @"select pip.SysNo, 
    pip.ProductSysNo,
    p.ProductName,
    pip.PromotionDiscount ,
    pp.CurrentPrice,
    pp.Discount,pip.OrderNum
    from Promotion_Item_Product pip left join Product p on pip.ProductSysNo=p.SysNo
    left join Product_Price pp on pp.ProductSysNo=pip.ProductSysNo 
    where pip.PromotionItemGroupSysNo={0}";

        private static readonly string RemoveProductSqlCmdTemplate = @"delete from Promotion_Item_Product where sysNo={0}";

        private static readonly string AddProductSqlCmdTemplate = @"INSERT INTO [mmbuy].[dbo].[Promotion_Item_Product]
           ([PromotionItemGroupSysNo]
           ,[ProductSysNo]
           ,[PromotionDiscount]
           ,[OrderNum]
           ,[PromotionPrice])
     VALUES
           ({0}
           ,{1}
           ,{2}
           ,{3}
           ,{4})";

        private static readonly string CheckProductExistedForGroupSqlCmdTemplate = @"select SysNo from Promotion_Item_Product where SysNo={0} AND ProductSysNo={1}";

        private static readonly string GetAllGroupItemsSqlCmdTemplate = @"select SysNo,GroupName from Promotion_Item_Group where status=0";

        private static readonly string GetBindedGroupSqlCmdTemplate = @"select SysNo,GroupName from Promotion_Item_Group where PromotionSysNo={0} and Status=0";

        private static readonly string[] UpdateGroupBindSqlCmdTemplate = new string[2]
            {
             @"update Promotion_Item_Group set PromotionSysNo=0 where PromotionSysNo={0};",
             @"update Promotion_Item_Group set PromotionSysNo={0} where SysNo={1}",
            };

        private static readonly string UpdatePromotionSqlCmdTemplate = @"update Promotion_Master 
            set PromotionName='{0}', PromotionNote='{1}', CreateTime='{2}', Status={3} where sysNo={4}";

        /// <summary>
        /// 获得促销单
        /// </summary>
        /// <param name="keyWord"></param>
        /// <param name="status"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static DataSet GetPromotionSearchResult(string keyWord, int status, string startDate, string endDate)
        {
            Hashtable ht = new Hashtable();
            ht.Add("KeyWords", keyWord);
            ht.Add("Status", status);
            ht.Add("DateFrom", startDate);
            ht.Add("DateTo", endDate);

            DataSet data = PromotionManager.GetInstance().GetPromotionDs(ht);
            return data;
        }

        /// <summary>
        /// 添加促销单
        /// </summary>
        /// <param name="title"></param>
        /// <param name="desc"></param>
        /// <param name="status"></param>
        /// <param name="createUserSysNo"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public static bool AddNewPromotionTicket(string title, string desc, int status = 0, int createUserSysNo = 403)
        {
            bool result = false;

            PromotionMasterInfo pmInfo = new PromotionMasterInfo()
            {
                PromotionName = title,
                PromotionNote = desc,
                CreateTime = DateTime.Now,
                Status = status,
                CreateUserSysNo = createUserSysNo,
            };

            try
            {

                PromotionManager.GetInstance().InsertMaster(pmInfo);
                result = true;
                return result;
            }
            catch
            {
                return result;
            }
        }

        /// <summary>
        /// 获取促销商品组
        /// </summary>
        /// <param name="name"></param>
        /// <param name="orderNum"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static DataTable GetPromotionGroup(string name, string orderNum, int status)
        {
            string sqlCmd = String.Empty;
            string condition = "WHERE";
            if (name.IsSafeString())
            {
                condition += String.Concat(" GroupName LIKE '%", name, "%'");
            }
            if (orderNum.IsSafeString())
            {
                condition += GroupBuyUtility.CombineMutlQueryCondtion(condition, String.Concat(" OrderNum", orderNum));
            }
            if (status != -999999)
            {
                condition += GroupBuyUtility.CombineMutlQueryCondtion(condition, String.Concat(" Status=", status));
            }
            sqlCmd = (condition == "WHERE") ? String.Format(GetPromotionGroupSqlCmdTemplate, String.Empty) : String.Format(GetPromotionGroupSqlCmdTemplate, condition);
            SqlDBHelper dbHelper = new SqlDBHelper();
            DataTable data = dbHelper.ExecuteQuery(sqlCmd);
            return data;
        }

        /// <summary>
        /// 获取一个Group下面所有的商品
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static DataTable GetGroupProducts(string groupId)
        {
            string sqlCmd = String.Format(GetGroupProductsSqlCmdTemplate, groupId);
            DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
            return data;
        }

        /// <summary>
        /// 移除组商品
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public static bool RemoveProduct(string sysNo)
        {
            string sqlCmd = String.Format(RemoveProductSqlCmdTemplate, sysNo);
            if (new SqlDBHelper().ExecuteNonQuery(sqlCmd) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 添加促销组的商品
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="productId"></param>
        /// <param name="discount"></param>
        /// <param name="orderNum"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public static bool AddProduct(string groupId, string productId, string discount, string orderNum, string price)
        {
            string sqlCmd = String.Format(AddProductSqlCmdTemplate, groupId, productId, discount, orderNum, price);
            if (new SqlDBHelper().ExecuteNonQuery(sqlCmd) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 在插入时 判断同一个group是否有同样的商品
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static bool IsProductExistedForGroup(string groupId, string productId)
        {
            string sqlCmd = String.Format(CheckProductExistedForGroupSqlCmdTemplate, groupId, productId);
            DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
            if (data.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 通过ID 
        /// 获得promotion对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static PromotionMasterInfo GetPromotionInfoById(string id)
        {
            int sysNo = int.Parse(id);
            return PromotionManager.GetInstance().Load(sysNo);
        }

        /// <summary>
        /// 初始化一个促销单的状态
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static Dictionary<int, string> GetPromotionStatus(int status)
        {
            Dictionary<int, string> list = new Dictionary<int, string>();
            if (status < 0)
            {
                list.Add(status, "无效");
                list.Add(0, "有效");
            }
            else
            {
                list.Add(status, "有效");
                list.Add(-1, "无效");
            }
            return list;
        }

        /// <summary>
        /// 获取所有的有效的促销组的sortedlist集合
        /// </summary>
        /// <returns></returns>
        public static SortedList GetPromotionGroups()
        {
            SortedList list = new SortedList();
            DataTable data = new SqlDBHelper().ExecuteQuery(GetAllGroupItemsSqlCmdTemplate);
            for (int i = 0; i < data.Rows.Count; i++)
            {
                int sysNo = int.Parse(data.Rows[i]["sysNo"].ToString());
                string groupNme = data.Rows[i]["GroupName"].ToString().Trim();
                list.Add(sysNo, groupNme);
            }
            return list;
        }

        /// <summary>
        /// 获取当前促销单已经绑定的促销商品组
        /// </summary>
        /// <param name="promotionId"></param>
        /// <returns></returns>
        public static string GetBindedGroup(string promotionId)
        {
            DataTable data = new SqlDBHelper().ExecuteQuery(String.Format(GetBindedGroupSqlCmdTemplate, promotionId));
            if (data.Rows.Count > 0)
            {
                return data.Rows[0]["GroupName"].ToString();
            }
            else
            {
                return "当前促销未绑定商品";
            }
        }

        /// <summary>
        /// 更新促销单的商品组绑定
        /// </summary>
        /// <param name="promotionid"></param>
        /// <param name="newgroupId"></param>
        /// <returns></returns>
        public static bool UpdatePromotionGroupBind(string promotionid, string newgroupId, bool isPromotionBinded)
        {
            bool result;
            try
            {
                string sqlCmd1 = String.Format(UpdateGroupBindSqlCmdTemplate[0], promotionid);
                string sqlCmd2 = String.Format(UpdateGroupBindSqlCmdTemplate[1], promotionid, newgroupId);
                if (isPromotionBinded)
                {
                    string[] sqlCmds = new string[2]
                    {
                        sqlCmd1,sqlCmd2,
                    };
                    SqlDBHelper helper = new SqlDBHelper();
                    if (helper.ExecuteTransaction(sqlCmds))
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
                    string[] sqlCmds = new string[1]
                    {
                        sqlCmd2,
                    };
                    SqlDBHelper helper = new SqlDBHelper();
                    if (helper.ExecuteTransaction(sqlCmds))
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 更新促销单
        /// </summary>
        /// <param name="pmInfo"></param>
        /// <returns></returns>
        public static bool UpdatePromotionTicket(PromotionMasterInfo pmInfo)
        {
            string sqlCmd = String.Format(UpdatePromotionSqlCmdTemplate, pmInfo.PromotionName, pmInfo.PromotionNote, pmInfo.CreateTime, pmInfo.Status, pmInfo.SysNo);
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

    }
}