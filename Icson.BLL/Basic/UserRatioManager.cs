using System;
using System.Data;
using System.Collections;
using System.Text;

using Icson.Utils;
using System.Transactions;
using Icson.Objects.Sale;
using Icson.DBAccess;
using Icson.DBAccess.Sale;

using Icson.Objects;
using Icson.Objects.Basic;

namespace Icson.BLL.Basic
{
    /// <summary>
    /// Summary description for StockManager.
    /// </summary>
    public class UserRatioManager : IInitializable
    {
        private UserRatioManager()
        {
        }
        private static UserRatioManager _instance;
        public static UserRatioManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new UserRatioManager();
                _instance.Init();
                _instance.InspectionInit();
                SyncManager.GetInstance().RegisterLastVersion((int)AppEnum.Sync.UserRatio);
            }
            return _instance;
        }

        private static object ratioLock = new object();
        private static Hashtable ratioHash = new Hashtable(5);
        private static Hashtable allocatedMenHash = new Hashtable(5);
        private static Hashtable InspectionratioHash = new Hashtable(5);
        private static Hashtable allocatedInspectionMenHash = new Hashtable(5);



        public void Init()
        {
            lock (ratioLock)
            {
                int ratioTotal = 0;
                if (ratioHash != null)
                    ratioHash.Clear();
                if (allocatedMenHash != null)
                    allocatedMenHash.Clear();

                string sql = "select * from SO_User_AuditRatio";
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (!Util.HasMoreRow(ds))
                    return;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ratioTotal += Util.TrimIntNull(dr["Ratio"]);
                    if (!allocatedMenHash.ContainsKey(Util.TrimIntNull(dr["UserSysNo"])))
                    {
                        UserInfo user = SysManager.GetInstance().LoadUser(Util.TrimIntNull(dr["UserSysNo"]));
                        allocatedMenHash.Add(Util.TrimIntNull(dr["UserSysno"]), user);
                    }
                }

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    SOUserAuditRatioInfo item = new SOUserAuditRatioInfo();
                    map(item, dr);
                    if (ratioHash == null)
                    {
                        ratioHash = new Hashtable(5);
                    }
                    for (int i = 0; i < item.Ratio; i++)
                    {
                        bool found = false;
                        do
                        {
                            System.Random oRandom = new System.Random(RandomString.GetNewSeed());
                            int tempkey = oRandom.Next(ratioTotal);
                            if (!ratioHash.ContainsKey(tempkey))
                            {
                                found = true;
                                ratioHash.Add(tempkey, item.UserSysNo);
                            }
                        } while (!found);
                    }
                }
            }
        }

        //判断用户是否为销售单分配审核员
        public bool IsallocatedMen(int UserSysNo)
        {
            if (allocatedMenHash.ContainsKey(UserSysNo))
                return true;
            else
                return false;
        }

        public void InspectionInit()
        {
            lock (ratioLock)
            {
                int ratioTotal = 0;
                if (InspectionratioHash != null)
                    InspectionratioHash.Clear();
                if (allocatedInspectionMenHash != null)
                    allocatedInspectionMenHash.Clear();

                string sql = "select * from WH_User_WorkRatio where WorkType=" + (int)AppEnum.WhWorkType.ProductInspection + "and BillType=" + (int)AppEnum.WhWorkBillType.SO;
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (!Util.HasMoreRow(ds))
                    return;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ratioTotal += Util.TrimIntNull(dr["Ratio"]);
                    if (!allocatedInspectionMenHash.ContainsKey(Util.TrimIntNull(dr["UserSysNo"])))
                    {
                        UserInfo user = SysManager.GetInstance().LoadUser(Util.TrimIntNull(dr["UserSysNo"]));
                        allocatedInspectionMenHash.Add(Util.TrimIntNull(dr["UserSysno"]), user);
                    }
                }

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    WHUserWorkRatioInfo item = new WHUserWorkRatioInfo();
                    map(item, dr);
                    if (InspectionratioHash == null)
                    {
                        InspectionratioHash = new Hashtable(5);
                    }
                    for (int i = 0; i < item.Ratio; i++)
                    {
                        bool found = false;
                        do
                        {
                            System.Random oRandom = new System.Random(RandomString.GetNewSeed());
                            int tempkey = oRandom.Next(ratioTotal);
                            if (!InspectionratioHash.ContainsKey(tempkey))
                            {
                                found = true;
                                InspectionratioHash.Add(tempkey, item.UserSysNo);
                            }
                        } while (!found);
                    }
                }
            }
        }


        /// <summary>
        /// 获取订单预分配审单人
        /// </summary>
        /// <returns></returns>
        public int GetAllocatedMan(int soSysNo)
        {
            if (ratioHash == null || ratioHash.Count == 0)
            {
                //throw new BizException("当前无订单审核人员");
                return 33; //IAS系统
            }
            int key = soSysNo % ratioHash.Count;
            return Int32.Parse(ratioHash[key].ToString());
        }

        public Hashtable GetAllocatedMenHash()
        {
            return allocatedMenHash;
        }
        /// <summary>
        /// 获取订单预分配检货人
        /// </summary>
        /// <returns></returns>
        public int GetSOInspectionAllocatedMan(int soSysNo)
        {
            if (InspectionratioHash == null || InspectionratioHash.Count == 0)
            {
                //throw new BizException("当前无订单审核人员");
                return 33; //IAS系统
            }
            int key = soSysNo % InspectionratioHash.Count;
            return Int32.Parse(InspectionratioHash[key].ToString());
        }

        public Hashtable GetInspectionAllocatedMenHash()
        {
            return allocatedInspectionMenHash;
        }

        private void map(SOUserAuditRatioInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.UserSysNo = Util.TrimIntNull(tempdr["UserSysNo"]);
            oParam.Ratio = Util.TrimIntNull(tempdr["Ratio"]);
            oParam.AuditTimeSpan = Util.TrimIntNull(tempdr["AuditTimeSpan"]);
        }

        public DataSet GetSOUserAuditRatioDs(Hashtable paramHash)
        {
            string sql = @"select sua.*,su.username from SO_User_AuditRatio sua,sys_user su where sua.usersysno=su.sysno  @UserSysNo";
            if (paramHash != null && paramHash.ContainsKey("UserSysNo"))
            {
                sql = sql.Replace("@UserSysNo", "and sua.usersysno=" + Util.ToSqlString(paramHash["UserSysNo"].ToString()));
            }
            else
                sql = sql.Replace("@UserSysNo", " ");
            return SqlHelper.ExecuteDataSet(sql);
        }

        public SOUserAuditRatioInfo LoadSOUserAuditRatio(int userSysNo)
        {
            string sql = "select * from SO_User_AuditRatio where usersysno=" + userSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                SOUserAuditRatioInfo oInfo = new SOUserAuditRatioInfo();
                map(oInfo, ds.Tables[0].Rows[0]);
                return oInfo;
            }
            else
                return null;
        }

        public void InsertSOUserAuditRatio(SOUserAuditRatioInfo oInfo)
        {
            new SOUserAuditRatioDac().Insert(oInfo);

            SyncManager.GetInstance().SetDbLastVersion((int)AppEnum.Sync.UserRatio);
            ratioHash.Add(oInfo.UserSysNo, oInfo);
        }

        public void UpdateSOUserAuditRatio(SOUserAuditRatioInfo oInfo)
        {
            new SOUserAuditRatioDac().Update(oInfo);

            SyncManager.GetInstance().SetDbLastVersion((int)AppEnum.Sync.UserRatio);
            ratioHash.Remove(oInfo.UserSysNo);
            ratioHash.Add(oInfo.UserSysNo, oInfo);
        }

        public void DeleteSOUserAudit(SOUserAuditRatioInfo oInfo)
        {
            string sql = "delete from SO_User_AuditRatio where UserSysNo=" + oInfo.UserSysNo;
            SqlHelper.ExecuteDataSet(sql);

            SyncManager.GetInstance().SetDbLastVersion((int)AppEnum.Sync.UserRatio);
            ratioHash.Remove(oInfo.UserSysNo);
        }

        public int GetSOUserAuditTotalRatio()
        {
            string sql = "select isnull(sum(Ratio),0) as TotalRatio from SO_User_AuditRatio ";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds) && ds.Tables[0].Rows.Count > 0)
                return (int)ds.Tables[0].Rows[0][0];
            else
                return AppConst.IntNull;
        }

        public SOUserAuditRatioInfo Load(int paramSysno)
        {
            return ratioHash[paramSysno] as SOUserAuditRatioInfo;
        }

        private void map(WHUserWorkRatioInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.UserSysNo = Util.TrimIntNull(tempdr["UserSysNo"]);
            oParam.Ratio = Util.TrimIntNull(tempdr["Ratio"]);
            oParam.WorkType = Util.TrimIntNull(tempdr["WorkType"]);
            oParam.BillType = Util.TrimIntNull(tempdr["BillType"]);
            oParam.WorkTimeSpan = Util.TrimIntNull(tempdr["WorkTimeSpan"]);
        }


        public DataSet GetWhUserWorkRatioDs(Hashtable paramHash)
        {
            string sql = @"select wuw.*,su.username from WH_User_WorkRatio wuw,sys_user su where wuw.usersysno=su.sysno  @UserSysNo @WorkType @BillType";

            if (paramHash != null && paramHash.ContainsKey("UserSysNo"))
            {
                sql = sql.Replace("@UserSysNo", "and wuw.usersysno=" + Util.ToSqlString(paramHash["UserSysNo"].ToString()));
            }
            else
            {
                sql = sql.Replace("@UserSysNo", " ");
            }
            if (paramHash != null && paramHash.ContainsKey("WorkType"))
            {
                sql = sql.Replace("@WorkType", "and wuw.WorkType=" + Util.ToSqlString(paramHash["WorkType"].ToString()));
            }
            else
            {
                sql = sql.Replace("@WorkType", " ");
            }
            if (paramHash != null && paramHash.ContainsKey("BillType"))
            {
                sql = sql.Replace("@BillType", "and wuw.BillType=" + Util.ToSqlString(paramHash["BillType"].ToString()));
            }
            else
            {
                sql = sql.Replace("@BillType", " ");
            }

            sql = sql + " order by WorkType, BillType";
            return SqlHelper.ExecuteDataSet(sql);
        }

        public WHUserWorkRatioInfo LoadWhUserWorkRatio(int sysNo)
        {
            string sql = "select * from WH_User_WorkRatio where sysno=" + sysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                WHUserWorkRatioInfo oInfo = new WHUserWorkRatioInfo();
                map(oInfo, ds.Tables[0].Rows[0]);
                return oInfo;
            }
            else
                return null;
        }

        public void InsertWhUserWorkRatio(WHUserWorkRatioInfo oInfo)
        {
            new WHUserWorkRatioDac().Insert(oInfo);
        }
        public void UpdateWhUserWorkRatio(WHUserWorkRatioInfo oInfo)
        {
            new WHUserWorkRatioDac().Update(oInfo);
        }
        public void DeleteWhUserWorkRatio(WHUserWorkRatioInfo oInfo)
        {
            string sql = "delete from WH_User_WorkRatio where sysno=" + oInfo.SysNo;
            SqlHelper.ExecuteDataSet(sql);
        }
    }
}