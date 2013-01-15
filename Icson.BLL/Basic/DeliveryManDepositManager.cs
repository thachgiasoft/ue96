using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using System.Collections;

using Icson.Utils;
using Icson.Objects;
using Icson.Objects.Basic;
using Icson.DBAccess;
using Icson.DBAccess.Basic;

namespace Icson.BLL.Basic
{
    public class DeliveryManDepositManager
    {
        private static DeliveryManDepositManager _instance;
        public static DeliveryManDepositManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DeliveryManDepositManager();
            }
            return _instance;
        }


        private void map(DeliveryManDepositInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.UserSysNo = Util.TrimIntNull(tempdr["UserSysNo"]);
            oParam.Deposit = Util.TrimDecimalNull(tempdr["Deposit"]);
            oParam.Arrearage = Util.TrimDecimalNull(tempdr["Arrearage"]);
            oParam.PayDate = Util.TrimDateNull(tempdr["PayDate"]);
            oParam.IsAllow = Util.TrimIntNull(tempdr["IsAllow"]);
            oParam.CreateUserSysNo = Util.TrimIntNull(tempdr["CreateUserSysNo"]);
            oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
            oParam.UpdateUserSysNo = Util.TrimIntNull(tempdr["UpdateUserSysNo"]);
            oParam.UpdateTime = Util.TrimDateNull(tempdr["UpdateTime"]);
        }

        public DeliveryManDepositInfo Load(int sysno)
        {
            string sql = "select * from DeliveryMan_Deposit where sysno =" + sysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            DeliveryManDepositInfo oInfo = new DeliveryManDepositInfo();
            if (Util.HasMoreRow(ds))
            {
                map(oInfo, ds.Tables[0].Rows[0]);
                return oInfo;
            }
            else
                return null;

        }

        public DataSet GetDeliveryManDepositByUserSysNo(int usersysno)
        {
            string sql = "select * from DeliveryMan_Deposit where UserSysNo =" + usersysno;
            return SqlHelper.ExecuteDataSet(sql);

        }

        public bool IsAllowDelivery(int usersysno)
        {
            bool tag = true;
            string sql = "select IsAllow from DeliveryMan_Deposit where usersysno=" + usersysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds) && Util.TrimIntNull(ds.Tables[0].Rows[0][0]) != AppConst.IntNull && Util.TrimIntNull(ds.Tables[0].Rows[0][0]) == (int)AppEnum.YNStatus.No)
                tag = false;

            return tag;

        }

        public decimal GetCredit(int usersysno)
        {
            decimal credit = 0;
            DataSet ds = GetDeliveryManDepositByUserSysNo(usersysno);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Util.TrimDecimalNull(dr["Deposit"]) == AppConst.DecimalNull)
                    throw new BizException("该配送员未交押金不能发货");
                if (Util.TrimIntNull(dr["IsAllow"]) == (int)AppEnum.YNStatus.No)
                    throw new BizException("该配送员帐号已被冻结，不能发货");
                if (Util.TrimDateNull(dr["PayDate"]) <= DateTime.Today.Date.AddMonths(-1))
                {
                    if (Util.TrimDecimalNull(dr["Arrearage"]) != AppConst.DecimalNull)
                    {
                        credit = Util.TrimDecimalNull(dr["Deposit"].ToString()) - Util.TrimDecimalNull(dr["Arrearage"].ToString()) + AppConfig.FMMonthCredit;
                    }

                    else
                        credit = Util.TrimDecimalNull(dr["Deposit"].ToString()) + AppConfig.FMMonthCredit;
                }
                else
                {
                    if (Util.TrimDecimalNull(dr["Arrearage"]) != AppConst.DecimalNull)
                    {
                        credit = Util.TrimDecimalNull(dr["Deposit"].ToString()) - Util.TrimDecimalNull(dr["Arrearage"].ToString());
                    }
                    credit = Util.TrimDecimalNull(dr["Deposit"].ToString());
                }
            }
            return credit;

        }

        public void Insert(DeliveryManDepositInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                new DeliveryManDepositDac().Insert(oParam);
                scope.Complete();
            }

        }

        public void Update(DeliveryManDepositInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                new DeliveryManDepositDac().Update(oParam);
                scope.Complete();
            }

        }


        public DataSet GetDMDDs(Hashtable paramHash)
        {
//            string sql = @" select su.username,su.DepartmentSysNo,su.userid,su.sysno as usersysno,dmd.SysNo,dmd.Deposit,dmd.PayDate,dmd.IsAllow,dmd.Arrearage
//                            from
//                                sys_user su
//                                left join DeliveryMan_Deposit dmd on dmd.usersysno=su.sysno
//								
//							where 1=1  and su.status=" + (int)AppEnum.BiStatus.Valid + " and DepartmentSysNo in (21,23) ";

            string sql = @" select su.username,su.StationSysNo,su.userid,su.sysno as usersysno,dmd.SysNo,dmd.Deposit,dmd.PayDate,dmd.IsAllow,dmd.Arrearage
                            from
                                sys_user su
                                left join DeliveryMan_Deposit dmd on dmd.usersysno=su.sysno
								
							where 1=1  and su.status=" + (int)AppEnum.BiStatus.Valid + " and su.flag="+((int)AppEnum.UserFlag.Sender).ToString();

            string sql1 = " order by dmd.usersysno ";
            if (paramHash != null && paramHash.Count != 0)
            {
                StringBuilder sb = new StringBuilder(100);
                foreach (string key in paramHash.Keys)
                {
                    object item = paramHash[key];
                    if (key == "UserID")
                    {
                        sb.Append(" and ");
                        sb.Append("su.UserID = ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "UserName")
                    {
                        sb.Append(" and ");
                        sb.Append("su.UserName like ").Append(Util.ToSqlLikeString(item.ToString()));
                    }
                    else if (key == "IsAllow")
                    {
                        sb.Append(" and ");
                        sb.Append("dmd.IsAllow = ").Append(item.ToString());
                    }
                    //else if (key == "DepartmentSysNo")
                    //{
                    //    sb.Append(" and ");
                    //    sb.Append(" su.DepartmentSysNo = ").Append(item.ToString());
                    //}
                    else if (key == "StationSysNo")
                    {
                        sb.Append(" and ");
                        sb.Append(" su.StationSysNo = ").Append(item.ToString());
                    }

                    else if (item is int)
                    {
                        sb.Append(" and ");
                        sb.Append(key).Append("=").Append(item.ToString());
                    }
                    else if (item is string)
                    {
                        sb.Append(" and ");
                        sb.Append(key).Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                    }
                }
                sql += sb.ToString();
            }

            sql += sql1;

            return SqlHelper.ExecuteDataSet(sql);
        }
    }
}
