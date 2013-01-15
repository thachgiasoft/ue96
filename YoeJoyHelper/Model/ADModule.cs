using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.ComponentModel;

namespace YoeJoyHelper.Model
{
    /// <summary>
    /// 广告的模型类
    /// </summary>
    public class ADModule
    {
        public string ADSysno { get; set; }
        public int Status { get; set; }
        public string ADName { get; set; }
        public int PostionId { get; set; }
        public string ADImg { get; set; }
        public string ADLink { get; set; }
        public int OrderNum { get; set; }
    }

    /// <summary>
    /// 前端页面的AD模型类
    /// </summary>
    public class ADModuleForSite
    {
        public string ADName { get; set; }
        public string ADImg { get; set; }
        public string ADLink { get; set; }
    }


    public class ADService
    {
        private static readonly string AddNewADSqlCmdTemplate = @"INSERT INTO [mmbuy].[dbo].[AD]
           ([ADName]
           ,[PositionID]
           ,[Status]
           ,[ADImg]
           ,[ADLink]
           ,[OrderNum])
     VALUES
           ('{0}'
           ,{1}
           ,{2}
           ,'{3}'
           ,'{4}'
           ,{5})";

        private static readonly string UpdateADSqlCmdTemplate = @"UPDATE [mmbuy].[dbo].[AD]
   SET [ADName] = '{0}'
      ,[PositionID] = {1}
      ,[Status] = {2}
      ,[ADImg] = '{3}'
      ,[ADLink] = '{4}'
      ,[OrderNum]='{5}'
 WHERE [ADSysNo]={6}";

        private static readonly string GetAdGroupByPositionSqlCmdTemplate = @"SELECT [ADSysNo]
      ,[ADName]
      ,[PositionID]
      ,[Status]
      ,[ADImg]
      ,[ADLink]
      ,[OrderNum]
  FROM [mmbuy].[dbo].[AD] WHERE [PositionID] = {0} ORDER BY ADSysNo DESC";

        private static readonly string GetAdByIdSqlCmdTemplate = @"SELECT [ADSysNo]
      ,[ADName]
      ,[PositionID]
      ,[Status]
      ,[ADImg]
      ,[ADLink]
      ,[OrderNum]
  FROM [mmbuy].[dbo].[AD] WHERE [ADSysNo] = {0}";

        private static readonly string GetSiteAdByPostionSqlCmdTemplate = @"SELECT TOP 1 [ADName]
      ,[ADImg]
      ,[ADLink]
      ,[OrderNum]
  FROM [mmbuy].[dbo].[AD] WHERE [PositionID] = {0} AND [Status]=1";

        private static readonly string getSlideAdByPostionSqlCmdTemplate = @"SELECT [ADName]
      ,[ADImg]
      ,[ADLink]
      ,[OrderNum]
  FROM [mmbuy].[dbo].[AD] WHERE [PositionID] = {0} AND [Status]=1 ORDER BY [OrderNum] ASC";

        private static readonly string GetSiteAdForSlideShowSqlCmdTemplate = @"SELECT [ADName]
      ,[ADImg]
      ,[ADLink]
  FROM [mmbuy].[dbo].[AD] WHERE [PositionID] = {0} AND [Status]=1 ORDER BY [OrderNum]";

        /// <summary>
        /// 添加一个新的广告
        /// </summary>
        /// <returns></returns>
        public static bool AddNewAD(ADModule ad)
        {
            string sqlCmd = String.Format(AddNewADSqlCmdTemplate, ad.ADName, ad.PostionId, ad.Status, ad.ADImg, ad.ADLink, ad.OrderNum);
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
        /// 修改一个广告
        /// </summary>
        /// <param name="ad"></param>
        /// <returns></returns>
        public static bool UpdateAd(ADModule ad)
        {
            string sqlCmd = String.Format(UpdateADSqlCmdTemplate, ad.ADName, ad.PostionId, ad.Status, ad.ADImg, ad.ADLink, ad.OrderNum, ad.ADSysno);
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
        /// 根据PositionId获取相应位置的所有广告
        /// </summary>
        /// <param name="positionId"></param>
        /// <returns></returns>
        public static DataTable GetAdGroupByPosition(int positionId)
        {
            string sqlCmd = String.Format(GetAdGroupByPositionSqlCmdTemplate, positionId);
            DataTable data = new SqlDBHelper().ExecuteQuery(sqlCmd);
            return data;
        }

        /// <summary>
        /// 根据广告ID获得广告信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ADModule GetAdById(string id)
        {
            DataTable data = new SqlDBHelper().ExecuteQuery(String.Format(GetAdByIdSqlCmdTemplate, id));
            ADModule ad = new ADModule()
            {
                ADSysno = id.ToString().Trim(),
                ADName = data.Rows[0]["ADName"].ToString().Trim(),
                Status = int.Parse(data.Rows[0]["Status"].ToString().Trim()),
                OrderNum = int.Parse(data.Rows[0]["OrderNum"].ToString().Trim()),
                ADImg = data.Rows[0]["ADImg"].ToString().Trim(),
                ADLink = data.Rows[0]["ADLink"].ToString().Trim(),
                PostionId = int.Parse(data.Rows[0]["PositionID"].ToString().Trim()),
            };
            return ad;
        }

        /// <summary>
        /// 获得前端页面的广告
        /// </summary>
        /// <param name="positionId"></param>
        /// <returns></returns>
        public static ADModuleForSite GetHomeAdByPosition(int positionId)
        {
            try
            {
                DataTable data = new SqlDBHelper().ExecuteQuery(String.Format(GetSiteAdByPostionSqlCmdTemplate, positionId));
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {
                    ADModuleForSite ad = new ADModuleForSite()
                    {
                        ADName = data.Rows[0]["ADName"].ToString().Trim(),
                        ADImg = data.Rows[0]["ADImg"].ToString().Trim(),
                        ADLink = data.Rows[0]["ADLink"].ToString().Trim(),
                    };
                    return ad;
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
        /// 获得幻灯片广告
        /// </summary>
        /// <param name="positionId"></param>
        /// <returns></returns>
        public static List<ADModuleForSite> GetSlideAdByPosition(int positionId)
        {
            try
            {
                DataTable data = new SqlDBHelper().ExecuteQuery(String.Format(getSlideAdByPostionSqlCmdTemplate, positionId));
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {
                    List<ADModuleForSite> ads = new List<ADModuleForSite>();
                    for (int i = 0; i < rowCount; i++)
                    {
                        ads.Add(new ADModuleForSite()
                        {
                            ADName = data.Rows[i]["ADName"].ToString().Trim(),
                            ADImg = data.Rows[i]["ADImg"].ToString().Trim(),
                            ADLink = data.Rows[i]["ADLink"].ToString().Trim(),
                        });
                    }
                    return ads;
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
        /// 获得首页幻灯片的广告
        /// </summary>
        /// <param name="positionId"></param>
        /// <returns></returns>
        public static List<ADModuleForSite> GetHomeAdForSlideShow(int positionId)
        {
            DataTable data = new SqlDBHelper().ExecuteQuery(String.Format(GetSiteAdForSlideShowSqlCmdTemplate, positionId));

            int count = data.Rows.Count;
            if (count > 0)
            {
                List<ADModuleForSite> list = new List<ADModuleForSite>();
                for (int i = 0; i < count; i++)
                {
                    list.Add(new ADModuleForSite()
                    {
                        ADName = data.Rows[i]["ADName"].ToString().Trim(),
                        ADImg = data.Rows[i]["ADImg"].ToString().Trim(),
                        ADLink = data.Rows[i]["ADLink"].ToString().Trim(),
                    });
                }
                return list;
            }
            else
            {
                return null;
            }
        }

    }
}