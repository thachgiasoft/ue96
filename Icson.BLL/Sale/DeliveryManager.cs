using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

using Icson.Objects;
using Icson.Objects.Sale;
using Icson.DBAccess.Sale;
using Icson.DBAccess;
using Icson.Utils;

namespace Icson.BLL.Sale
{
    public class DeliveryManager
    {
        private static DeliveryManager _instance = new DeliveryManager();
        public static DeliveryManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DeliveryManager();
            }
            return _instance;
        }

        private void map(DeliveryManSetListInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.ItemID = Util.TrimNull(tempdr["ItemID"]);
            oParam.SetUserSysNo = Util.TrimIntNull(tempdr["SetUserSysNo"]);
            oParam.FreightUserSysNo = Util.TrimIntNull(tempdr["FreightUserSysNo"]);
            oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
            oParam.DLSysNo = Util.TrimIntNull(tempdr["DLSysNo"]);
        }

        private void map(DeliveryDelayListInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.CauseType = Util.TrimIntNull(tempdr["CauseType"]);
            oParam.BillID = Util.TrimNull(tempdr["BillID"]);
            oParam.FreightUserSysNo = Util.TrimIntNull(tempdr["FreightUserSysNo"]);
            oParam.SetDeliveryManTime = Util.TrimDateNull(tempdr["SetDeliveryManTime"]);
            oParam.UpdateUserSysNo = Util.TrimIntNull(tempdr["UpdateUserSysNo"]);
            oParam.UpdateTime = Util.TrimDateNull(tempdr["UpdateTime"]);
            oParam.ReviewBackNote = Util.TrimNull(tempdr["ReviewBackNote"]);
            oParam.ReviewCauseType = Util.TrimIntNull(tempdr["ReviewCauseType"]);
            oParam.ReviewBackUserSysNo = Util.TrimIntNull(tempdr["ReviewBackUserSysNo"]);
            oParam.ReviewBackTime = Util.TrimDateNull(tempdr["ReviewBackTime"]);
        }


        private void map(DeliveryManArrearageLogInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.UserSysNo = Util.TrimIntNull(tempdr["UserSysNo"]);
            oParam.DSSysNo = Util.TrimIntNull(tempdr["DSSysNo"]);
            oParam.Arrearage = Util.TrimDecimalNull(tempdr["Arrearage"]);
            oParam.ArrearageLogType = Util.TrimIntNull(tempdr["ArrearageLogType"]);
            oParam.ArrearageChange = Util.TrimDecimalNull(tempdr["ArrearageChange"]);
            oParam.Memo = Util.TrimNull(tempdr["Memo"]);
            oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
            oParam.CreateUserSysNo = Util.TrimIntNull(tempdr["CreateUserSysNo"]);
        }

        public DeliveryDelayListInfo Load(int SysNo)
        {
            string sql = "select * from DeliveryDelayList where sysno =" + SysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            DeliveryDelayListInfo oInfo = new DeliveryDelayListInfo();
            if (Util.HasMoreRow(ds))
                map(oInfo, ds.Tables[0].Rows[0]);
            else
                oInfo = null;
            return oInfo;
        }

        public DeliveryManArrearageLogInfo LoadDMALog(int SysNo)
        {
            string sql = "select * from DeliveryMan_ArrearageLog where sysno=" + SysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            DeliveryManArrearageLogInfo oInfo = new DeliveryManArrearageLogInfo();
            if (Util.HasMoreRow(ds))
                map(oInfo, ds.Tables[0].Rows[0]);
            else
                oInfo = null;
            return oInfo;
        }

        public void InsertDeliveryMenSetList(DeliveryManSetListInfo oParam)
        {
            new DeliveryDac().Insert(oParam);
        }
        public DataSet GetDeliveryManListDs(Hashtable paramHash)
        {
            string sql = @" select dl.sysno,dl.itemid,dl.createtime,dl.dlsysno,su.username,su2.username as setusername,su.phone
                          from DeliveryManSetList dl
                          left join Sys_User su on dl.FreightUserSysNo=su.sysno
                          left join sys_user su2 on dl.SetUserSysNo=su2.sysno   
                          where 1=1";

            if (paramHash != null || paramHash.Count == 0)
            {
                StringBuilder sb = new StringBuilder();

                foreach (string key in paramHash.Keys)
                {
                    object item = paramHash[key];
                    sb.Append(" and ");
                    if (key == "DateFrom")
                    {
                        sb.Append("dl.createtime >=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "DateTo")
                    {
                        sb.Append("dl.createtime <=").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "ItemID")
                    {
                        sb.Append("dl.itemid like").Append(Util.ToSqlLikeString(item.ToString()));
                    }
                    else if (key == "FreightManSysNo")
                    {
                        sb.Append("dl.FreightUserSysNo=").Append(Util.ToSqlString(item.ToString()));
                    }
                }
                sql = sql + sb;

            }
            else sql = sql.Replace("select", "select top 50");
            sql += " order by Createtime Desc";
            return SqlHelper.ExecuteDataSet(sql);
        }


        public void InsertDeliveryDelayList(DeliveryDelayListInfo oParam)
        {
            string sql = "select * from DeliveryDelayList where FreightUserSysNo=" + oParam.FreightUserSysNo + " and SetDeliveryManTime=" + Util.ToSqlString(oParam.SetDeliveryManTime.ToString()) + " and BillID=" + Util.ToSqlString(oParam.BillID);
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                throw new BizException("相同订单号、配送人、设置时间的记录已存在，不能重复添加！");
            new DeliveryDelayDac().Insert(oParam);
        }
        public void UpdateDeliveryDelayList(DeliveryDelayListInfo oParam)
        {
            new DeliveryDelayDac().Update(oParam);
        }

        public DataSet GetDeliveryDelayListDs(Hashtable paramHash)
        {
            string sql = @"select DeliveryDelayList.*,sys_user.username as FreigthManName,su.username as updateusername
                       from DeliveryDelayList,sys_user ,sys_user su
                       where DeliveryDelayList.FreightUserSysNo=sys_user.sysno and su.sysno=DeliveryDelayList.UpdateUserSysNo";

            if (paramHash != null || paramHash.Count == 0)
            {
                StringBuilder sb = new StringBuilder();

                foreach (string key in paramHash.Keys)
                {
                    object item = paramHash[key];
                    sb.Append(" and ");
                    if (key == "DateFrom")
                    {
                        sb.Append("DeliveryDelayList.UpdateTime >=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "DateTo")
                    {
                        sb.Append("DeliveryDelayList.UpdateTime <=").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "FreightUserSysNo")
                    {
                        sb.Append("DeliveryDelayList.FreightUserSysNo=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "BillID")
                    {
                        sb.Append("DeliveryDelayList.BillID like").Append(Util.ToSqlLikeString(item.ToString()));
                    }
                    else if (key == "CauseType")
                    {
                        sb.Append("DeliveryDelayList.CauseType=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "BranchSysNo")
                    {
                        sb.Append("sys_user.BranchSysNo=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "IsReviewBack")
                    {
                        if ((int)item == (int)AppEnum.YNStatus.Yes)
                            sb.Append("isnull(DeliveryDelayList.ReviewCauseType," + AppConst.IntNull + ")> 0");
                        else if ((int)item == (int)AppEnum.YNStatus.No)
                            sb.Append("isnull(DeliveryDelayList.ReviewCauseType," + AppConst.IntNull + ")< 0");
                    }
                    else if (key == "IsDifferent")
                    {
                        if ((int)item == (int)AppEnum.YNStatus.Yes)
                            sb.Append("isnull(DeliveryDelayList.ReviewCauseType," + AppConst.IntNull + ")<> isnull(DeliveryDelayList.CauseType," + AppConst.IntNull + ")");
                        else if ((int)item == (int)AppEnum.YNStatus.No)
                            sb.Append("isnull(DeliveryDelayList.ReviewCauseType," + AppConst.IntNull + ")= isnull(DeliveryDelayList.CauseType," + AppConst.IntNull + ")");
                    }


                }
                sql = sql + sb;

            }
            else sql = sql.Replace("select", "select top 50");
            sql += " order by UpdateTime Desc";
            return SqlHelper.ExecuteDataSet(sql);

        }

        public void DeleteDeliveryDelayList(int paramSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                DeliveryDelayListInfo oInfo = Load(paramSysNo);
                if (oInfo.ReviewBackNote != AppConst.StringNull || oInfo.ReviewCauseType != AppConst.IntNull)
                {
                    throw new BizException("已有回访记录不能删除");
                }
                else
                {
                    string sql = "delete from DeliveryDelayList where sysno=" + paramSysNo;
                    SqlHelper.ExecuteDataSet(sql);
                }
                scope.Complete();
            }
        }

        public void InsertDeliveryManArrearageLog(DeliveryManArrearageLogInfo oParam)
        {
            new DeliveryManArrearageLogDac().Insert(oParam);
        }
        public void UpdateDeliveryManArrearageLog(DeliveryManArrearageLogInfo oParam)
        {
            new DeliveryManArrearageLogDac().Update(oParam);
        }

        public void DeleteDeliveryManArrearageLog(int DLSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                string sql = "delete from DeliveryMan_ArrearageLog where dlsysno=" + DLSysNo;
                SqlHelper.ExecuteDataSet(sql);

                scope.Complete();
            }
        }

        public decimal FMTotalArrearage(int UserSysNo)
        {
            string sql = "select sum(ArrearageChange) from DeliveryMan_ArrearageLog where usersysno=" + UserSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (Util.HasMoreRow(ds) && Util.TrimDecimalNull(ds.Tables[0].Rows[0][0]) != AppConst.DecimalNull)
                return Util.ToMoney(ds.Tables[0].Rows[0][0].ToString());
            else
                return 0;

        }

        public DataSet GetFMArrearageLogDs(int usersysno)
        {
            string sql = @"select DeliveryMan_ArrearageLog.*,su.username from DeliveryMan_ArrearageLog 
                         left join sys_user su on DeliveryMan_ArrearageLog.CreateUserSysNo=su.sysno
                         where usersysno=" + usersysno + " order by DeliveryMan_ArrearageLog.sysno DESC";
            return SqlHelper.ExecuteDataSet(sql);
        }
    }
}
