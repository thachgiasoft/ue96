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
using Icson.BLL.RMA;
using Icson.BLL.Stock;

namespace Icson.BLL.Sale
{
    public class DLManager
    {
        private DLManager()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        private static DLManager _instance;
        public static DLManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DLManager();
            }
            return _instance;
        }


        private void map(DLInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.FreightUserSysNo = Util.TrimIntNull(tempdr["FreightUserSysNo"]);
            oParam.CreateUserSysNo = Util.TrimIntNull(tempdr["CreateUserSysNo"]);
            oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
            oParam.ConfirmUserSysNo = Util.TrimIntNull(tempdr["ConfirmUserSysNo"]);
            oParam.ConfirmTime = Util.TrimDateNull(tempdr["ConfirmTime"]);
            oParam.IncomeUserSysNo = Util.TrimIntNull(tempdr["IncomeUserSysNo"]);
            oParam.IncomeTime = Util.TrimDateNull(tempdr["IncomeTime"]);
            oParam.UpdateFreightManUserSysNo = Util.TrimIntNull(tempdr["UpdateFreightManUserSysNo"]);
            oParam.UpdateFreightManTime = Util.TrimDateNull(tempdr["UpdateFreightManTime"]);
            oParam.VoucherUserSysNo = Util.TrimIntNull(tempdr["VoucherUserSysNo"]);
            oParam.VoucherID = Util.TrimNull(tempdr["VoucherID"]);
            oParam.VoucherTime = Util.TrimDateNull(tempdr["VoucherTime"]);
            oParam.HasPaidQty = Util.TrimIntNull(tempdr["HasPaidQty"]);
            oParam.HasPaidAmt = Util.TrimDecimalNull(tempdr["HasPaidAmt"]);
            oParam.CODQty = Util.TrimIntNull(tempdr["CODQty"]);
            oParam.CODAmt = Util.TrimDecimalNull(tempdr["CODAmt"]);
            oParam.Type = Util.TrimIntNull(tempdr["Type"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
            oParam.IsSendSMS = Util.TrimIntNull(tempdr["IsSendSMS"]);
            oParam.IsAllow = Util.TrimIntNull(tempdr["IsAllow"]);
        }

        private void map(DLItemInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.DLSysNo = Util.TrimIntNull(tempdr["DLSysNo"]);
            oParam.ItemID = Util.TrimNull(tempdr["ItemID"]);
            oParam.ItemType = Util.TrimIntNull(tempdr["ItemType"]);
            oParam.PayType = Util.TrimIntNull(tempdr["PayType"]);
            oParam.PayAmt = Util.TrimDecimalNull(tempdr["PayAmt"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
        }

        public DLInfo Load(int sysno)
        {
            string sql = "select * from DL_Master where sysno =" + sysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            DLInfo oInfo = new DLInfo();
            if (Util.HasMoreRow(ds))
            {
                map(oInfo, ds.Tables[0].Rows[0]);

                //load dlitems
                string sqlItem = "select * from dl_item where dlsysno =" + oInfo.SysNo;
                DataSet dsItem = SqlHelper.ExecuteDataSet(sqlItem);
                if (Util.HasMoreRow(dsItem))
                {
                    foreach (DataRow dr in dsItem.Tables[0].Rows)
                    {
                        DLItemInfo oDLItem = new DLItemInfo();
                        map(oDLItem, dr);
                        oInfo.ItemHash.Add(oDLItem.SysNo, oDLItem);
                    }
                }
                return oInfo;
            }
            else
                return null;

        }
        public DLInfo LoadDLMaster(int sysno)
        {
            string sql = "select * from DL_Master where sysno =" + sysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            DLInfo oInfo = new DLInfo();
            if (Util.HasMoreRow(ds))
            {
                map(oInfo, ds.Tables[0].Rows[0]);
                return oInfo;
            }
            else
                return null;

        }
        public DLItemInfo LoadDLItem(int sysno)
        {
            string sql = "select * from DL_Item where sysno =" + sysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            DLItemInfo oInfo = new DLItemInfo();
            if (Util.HasMoreRow(ds))
                map(oInfo, ds.Tables[0].Rows[0]);
            else
                oInfo = null;
            return oInfo;
        }

        private void InsertDLMaster(DLInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                oParam.SysNo = SequenceDac.GetInstance().Create("DL_Sequence");
                new DLDac().Insert(oParam);
                scope.Complete();
            }
        }

        public void UpdateDLMaster(DLInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                new DLDac().Update(oParam);
                scope.Complete();
            }
        }

        public void UpdateDLMaster(Hashtable paramHash)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                new DLDac().UpdateMaster(paramHash);
                scope.Complete();
            }
        }

        private void InsertDLItem(DLItemInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                new DLDac().Insert(oParam);
                scope.Complete();
            }
        }

        public void AddNewDLItem(DLItemInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                string sql = "select * from dl_item where dlsysno=" + oParam.DLSysNo + " and ItemID=" + Util.ToSqlString(oParam.ItemID);
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (Util.HasMoreRow(ds)) //先判断是否存在，存在就修改为valid状态
                {
                    oParam.SysNo = Util.TrimIntNull(ds.Tables[0].Rows[0][0].ToString());
                    oParam.Status = (int)AppEnum.BiStatus.Valid;
                    new DLDac().Update(oParam);
                }
                else
                {
                    new DLDac().Insert(oParam);
                }
                CalcDLMaster(oParam.DLSysNo);
                scope.Complete();
            }
        }

        public void DeleteDLItem(DLItemInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                oParam.Status = (int)AppEnum.BiStatus.InValid;
                new DLDac().Update(oParam);
                CalcDLMaster(oParam.DLSysNo);
                scope.Complete();
            }
        }

        public void CalcDLMaster(int SysNo)
        {
            string sql1 = "select * from dl_master where sysno=" + SysNo;
            DataSet ds1 = SqlHelper.ExecuteDataSet(sql1);
            DLInfo dlInfo = new DLInfo();
            if (Util.HasMoreRow(ds1))
            {
                map(dlInfo, ds1.Tables[0].Rows[0]);
            }
            else
            {
                return;
            }

            dlInfo.ItemHash.Clear();

            string sql2 = "select * from dl_item where dlsysno=" + SysNo + " and status=@valid";
            sql2 = sql2.Replace("@valid", ((int)AppEnum.BiStatus.Valid).ToString());
            DataSet ds2 = SqlHelper.ExecuteDataSet(sql2);
            if (Util.HasMoreRow(ds2))
            {
                foreach (DataRow dr in ds2.Tables[0].Rows)
                {
                    DLItemInfo item = new DLItemInfo();
                    map(item, dr);
                    dlInfo.ItemHash.Add(item.SysNo, item);
                }
            }

            try
            {
                TransactionOptions options = new TransactionOptions();
                options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                options.Timeout = TransactionManager.DefaultTimeout;

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    int HasPaidQty = 0;
                    decimal HasPaidAmt = 0;
                    int CODQty = 0;
                    decimal CODAmt = 0;

                    foreach (DLItemInfo item in dlInfo.ItemHash.Values)
                    {
                        if (item.PayType == 1) //货到付款
                        {
                            CODQty++;
                            CODAmt += item.PayAmt;
                        }
                        else
                        {
                            HasPaidQty++;
                            HasPaidAmt += item.PayAmt;
                        }
                    }

                    dlInfo.HasPaidQty = HasPaidQty;
                    dlInfo.HasPaidAmt = HasPaidAmt;
                    dlInfo.CODQty = CODQty;
                    dlInfo.CODAmt = CODAmt;

                    UpdateDLMaster(dlInfo);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                dlInfo.SysNo = AppConst.IntNull;
                throw ex;
            }
        }

        public void UpdateDLItem(DLItemInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                new DLDac().Update(oParam);
                scope.Complete();
            }
        }

        public void CreateDL(DLInfo dlInfo)
        {
            try
            {
                TransactionOptions options = new TransactionOptions();
                options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                options.Timeout = TransactionManager.DefaultTimeout;

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    dlInfo.Status = (int)AppEnum.DLStatus.Origin;
                    dlInfo.CreateTime = DateTime.Now;

                    //加入配送表主项
                    this.InsertDLMaster(dlInfo);

                    //加入配送表明细
                    int HasPaidQty = 0;
                    decimal HasPaidAmt = 0;
                    int CODQty = 0;
                    decimal CODAmt = 0;

                    foreach (DLItemInfo item in dlInfo.ItemHash.Values)
                    {
                        item.DLSysNo = dlInfo.SysNo;
                        item.Status = (int)AppEnum.BiStatus.Valid;
                        this.InsertDLItem(item);

                        if (item.PayType == 1) //货到付款
                        {
                            CODQty++;
                            CODAmt += item.PayAmt;
                        }
                        else
                        {
                            HasPaidQty++;
                            HasPaidAmt += item.PayAmt;
                        }
                    }

                    dlInfo.HasPaidQty = HasPaidQty;
                    dlInfo.HasPaidAmt = HasPaidAmt;
                    dlInfo.CODQty = CODQty;
                    dlInfo.CODAmt = CODAmt;

                    UpdateDLMaster(dlInfo);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                dlInfo.SysNo = AppConst.IntNull;
                throw ex;
            }
        }

        public int GetDLItemSysNo(int FreightUserSysNo, string itemid)
        {
            string sql = @"select top 1 dl_item.sysno from dl_master,dl_item where dl_master.sysno=dl_item.dlsysno and FreightUserSysNo=" + FreightUserSysNo + " and itemid=" + Util.ToSqlString(itemid) + " and dl_item.status=" + (int)AppEnum.BiStatus.Valid + " and dl_master.status=" + (int)AppEnum.DLStatus.StockConfirmed + " order by dl_item.sysno desc";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                return Int32.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            else
                return AppConst.IntNull;

        }

        public DataSet GetDLDs(Hashtable paramHash)
        {
            string sql = @" select dl.sysno,dl.FreightUserSysNo,fu.username as freightusername,su.username as createusername, 
                            dl.createtime,dl.haspaidqty,dl.haspaidamt,dl.codqty,dl.codamt,dl.status,dl.IsAllow,dl.IsSendSMS 
                            from
								dl_master dl(nolock) 
                                left join sys_user fu(nolock) on dl.freightusersysno=fu.sysno 
                                left join sys_user su(nolock) on dl.createusersysno=su.sysno
							where 1=1 ";

            string sql1 = " order by dl.sysno desc ";
            if (paramHash != null && paramHash.Count != 0)
            {
                StringBuilder sb = new StringBuilder(100);
                foreach (string key in paramHash.Keys)
                {
                    object item = paramHash[key];
                    if (key == "DateFrom")
                    {
                        sb.Append(" and ");
                        sb.Append("dl.createtime >= ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "DateTo")
                    {
                        sb.Append(" and ");
                        sb.Append("dl.createtime <= ").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "FreightUserSysNo")
                    {
                        sb.Append(" and ");
                        sb.Append("dl.freightusersysno = ").Append(item.ToString());
                    }
                    else if (key == "SysNo")
                    {
                        sb.Append(" and ");
                        sb.Append(" dl.sysno = ").Append(item.ToString());
                    }
                    else if (key == "Status")
                    {
                        sb.Append(" and ");
                        sb.Append(" dl.status = ").Append(item.ToString());
                    }
                    else if (key == "CreateUserSysNo")
                    {
                        sb.Append(" and ");
                        sb.Append(" dl.CreateUserSysNo= ").Append(item.ToString());
                    }
                    else if (key == "BranchSysNo")
                    {
                        sb.Append(" and ");
                        sb.Append(" fu.BranchSysNo= ").Append(item.ToString());
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
            else
            {
                sql = sql.Replace("select", "select top 50");
            }

            sql += sql1;

            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetDLItemDs(int dlSysNo, int status)
        {
            string sql = "select * from dl_item where dlsysno=" + dlSysNo;
            if (status == (int)AppEnum.BiStatus.Valid || status == (int)AppEnum.BiStatus.InValid)
            {
                sql += " and status=" + status;
            }
            sql += " order by sysno desc";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                return ds;
            else
                return null;
        }

        public DataSet GetDLItemDs(Hashtable paramHash)
        {
            string sql = @" select * from dl_item 	where 1=1 ";

            if (paramHash != null && paramHash.Count != 0)
            {
                StringBuilder sb = new StringBuilder(100);
                foreach (string key in paramHash.Keys)
                {
                    object item = paramHash[key];
                    if (key == "SysNo")
                    {
                        sb.Append(" and ");
                        sb.Append("SysNo = ").Append(item.ToString());
                    }

                    else if (key == "DLSysNo")
                    {
                        sb.Append(" and ");
                        sb.Append("DLSysNo = ").Append(item.ToString());
                    }
                    else if (key == "ItemID")
                    {
                        sb.Append(" and ");
                        sb.Append(" ItemID = ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "Status")
                    {
                        sb.Append(" and ");
                        sb.Append(" status = ").Append(item.ToString());
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
            else
            {
                sql = sql.Replace("select", "select top 50");
            }


            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetSOFreightMenDs(Hashtable paramHash)
        {
            string sql = @"select sys_user.username as freightusername,paytype.IsPayWhenRecv,so_master.receivename,receiveaddress, receivephone, receivecellphone,
                           area.districtname,area.localcode,so_master.status AS SOStatus,SO_MASter.sysno as sysno, paytypename,so_master.PayTypeSysNo as PayTypeSysNo,
								cashpay+premiumamt+shipprice+payprice-freeshipfeepay-DiscountAmt as totalcash,so_master.IsLarge as IsLarge
                          from DL_Item,
                               DL_Master,
                               so_master,
                               area,
                               sys_user,
                               paytype
                               where 1=1 and  @dlmstatus and @dlitemstatus 
                               and DL_Item.ItemID=so_master.soid
                               and DL_Master.FreightUserSysNo=sys_user.sysno
                               and DL_Item.PayType=paytype.sysno
                               and DL_Master.sysno=DL_Item.dlsysno
                               and so_master.receiveareasysno = area.sysno";

            string sql1 = " order by receiveareasysno,so_master.sysno ";

            sql = sql.Replace("@dlmstatus", " DL_Master.status <>" + (int)AppEnum.DLStatus.Abandon);  //必须是非作废的配送单
            sql = sql.Replace("@dlitemstatus", " DL_Item.status =" + (int)AppEnum.BiStatus.Valid);


            if (paramHash != null && paramHash.Count != 0)
            {
                StringBuilder sb = new StringBuilder(100);
                foreach (string key in paramHash.Keys)
                {

                    object item = paramHash[key];

                    if (key == "DLSysNo")
                    {
                        sb.Append(" and ");
                        sb.Append("DL_Item.DLSysNo = ").Append(item.ToString());
                    }

                    else if (key == "orderby")
                    {
                        sql1 = " order by DL_Item.sysno desc ";
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
            else
            {
                sql = sql.Replace("select", "select top 50");
            }

            sql += sql1;

            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetRequestFreightMenDs(Hashtable paramHash)
        {
            string sql = @"select rma_request.sysno,rma_request.RequestID,rma_request.Address,rma_request.DoorGetFee,area.DistrictName,
                           area.localcode,sys_user.username as freightusername,rma_request.Contact,rma_request.Phone,rma_request.IsLarge as IsLarge
                          from rma_request,area ,sys_user,DL_Item,DL_Master
                          where  AreaSysNo=area.sysno  and @dlmstatus and @dlitemstatus
                                and DL_Item.ItemID=rma_request.RequestID
                                and DL_Master.sysno=DL_Item.dlsysno
                                and DL_Master.FreightUserSysNo=sys_user.sysno ";


            string sql1 = "";

            sql = sql.Replace("@dlmstatus", " DL_Master.status <>" + (int)AppEnum.DLStatus.Abandon);  //必须是非作废的配送单
            sql = sql.Replace("@dlitemstatus", " DL_Item.status =" + (int)AppEnum.BiStatus.Valid);


            if (paramHash != null && paramHash.Count != 0)
            {
                StringBuilder sb = new StringBuilder(100);
                foreach (string key in paramHash.Keys)
                {

                    object item = paramHash[key];

                    if (key == "DLSysNo")
                    {
                        sb.Append(" and ");
                        sb.Append("DL_Item.DLSysNo = ").Append(item.ToString());
                    }

                    else if (key == "orderby")
                    {
                        sql1 = " order by DL_Item.sysno desc ";
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
            else
            {
                sql = sql.Replace("select", "select top 50");
            }

            sql += sql1;
            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetRevertFreightMenDs(Hashtable paramHash)
        {
            string sql = @"select rma_revert.sysno,rma_revert.RevertID,rma_revert.Address,area.DistrictName,area.localcode,
                           sys_user.username as freightusername,rma_revert.Contact,rma_revert.Phone,rma_revert.IsLarge as IsLarge
                          from rma_revert,area ,sys_user,DL_Item,DL_Master
                          where rma_revert.status=1 and AddressAreaSysNo=area.sysno and @dlmstatus and @dlitemstatus
                          and DL_Item.ItemID=rma_revert.RevertID
                          and DL_Master.sysno=DL_Item.dlsysno                          
                          and DL_Master.FreightUserSysNo=sys_user.sysno ";

            string sql1 = "";

            sql = sql.Replace("@dlmstatus", " DL_Master.status <>" + (int)AppEnum.DLStatus.Abandon);  //必须是非作废的配送单
            sql = sql.Replace("@dlitemstatus", " DL_Item.status =" + (int)AppEnum.BiStatus.Valid);


            if (paramHash != null && paramHash.Count != 0)
            {
                StringBuilder sb = new StringBuilder(100);
                foreach (string key in paramHash.Keys)
                {

                    object item = paramHash[key];

                    if (key == "DLSysNo")
                    {
                        sb.Append(" and ");
                        sb.Append("DL_Item.DLSysNo = ").Append(item.ToString());
                    }

                    else if (key == "orderby")
                    {
                        sql1 = " order by DL_Item.sysno desc ";
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
            else
            {
                sql = sql.Replace("select", "select top 50");
            }

            sql += sql1;
            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetOutBoundFreightMenDs(Hashtable paramHash)
        {
            string sql = @"select RMA_OutBound.sysno,RMA_OutBound.OutBoundID,RMA_OutBound.Address,area.DistrictName,area.localcode,RMA_OutBound.Contact,RMA_OutBound.Phone,
                           sys_user.username as freightusername,RMA_OutBound.IsLarge as IsLarge
                          from RMA_OutBound,area ,sys_user,DL_Item,DL_Master
                          where RMA_OutBound.status=1 and AreaSysNo=area.sysno and @dlmstatus and @dlitemstatus
                          and DL_Item.ItemID=RMA_OutBound.OutBoundID
                          and DL_Master.sysno=DL_Item.dlsysno
                          and DL_Master.FreightUserSysNo=sys_user.sysno ";


            string sql1 = "";
            sql = sql.Replace("@dlmstatus", " DL_Master.status <>" + (int)AppEnum.DLStatus.Abandon);  //必须是非作废的配送单
            sql = sql.Replace("@dlitemstatus", " DL_Item.status =" + (int)AppEnum.BiStatus.Valid);


            if (paramHash != null && paramHash.Count != 0)
            {
                StringBuilder sb = new StringBuilder(100);
                foreach (string key in paramHash.Keys)
                {

                    object item = paramHash[key];

                    if (key == "DLSysNo")
                    {
                        sb.Append(" and ");
                        sb.Append("DL_Item.DLSysNo = ").Append(item.ToString());
                    }

                    else if (key == "orderby")
                    {
                        sql1 = " order by DL_Item.sysno desc ";
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
            else
            {
                sql = sql.Replace("select", "select top 50");
            }


            sql += sql1;
            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetSendAccessoryFreightMenDs(Hashtable paramHash)
        {
            string sql = @"select RMA_SendAccessory.sysno,RMA_SendAccessory.SendAccessoryID,RMA_SendAccessory.Address,RMA_SendAccessory.Contact,RMA_SendAccessory.Phone,
                            area.DistrictName,area.localcode,sys_user.username as freightusername,RMA_SendAccessory.IsLarge as IsLarge
                          from RMA_SendAccessory,area ,sys_user,DL_Item,DL_Master
                          where AreaSysNo=area.sysno and @RMASendAccessoryStatus  and @dlmstatus and @dlitemstatus
                          and DL_Item.ItemID=RMA_SendAccessory.SendAccessoryID
                          and DL_Master.FreightUserSysNo=sys_user.sysno 
                          and DL_Master.sysno=DL_Item.dlsysno";


            string sql1 = "";
            sql = sql.Replace("@dlmstatus", " DL_Master.status <>" + (int)AppEnum.DLStatus.Abandon);  //必须是非作废的配送单
            sql = sql.Replace("@dlitemstatus", " DL_Item.status =" + (int)AppEnum.BiStatus.Valid);
            sql = sql.Replace("@RMASendAccessoryStatus", "RMA_SendAccessory.status=" + (int)AppEnum.RMASendAccessoryStatus.Sent);


            if (paramHash != null && paramHash.Count != 0)
            {
                StringBuilder sb = new StringBuilder(100);
                foreach (string key in paramHash.Keys)
                {

                    object item = paramHash[key];

                    if (key == "DLSysNo")
                    {
                        sb.Append(" and ");
                        sb.Append("DL_Item.DLSysNo = ").Append(item.ToString());
                    }

                    else if (key == "orderby")
                    {
                        sql1 = " order by DL_Item.sysno desc ";
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
            else
            {
                sql = sql.Replace("select", "select top 50");
            }


            sql += sql1;
            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetStShiftFreightMenDs(Hashtable paramHash)
        {
            string sql = @"select St_Shift.sysno,St_Shift.ShiftID,sys_user.username as freightusername,St_Shift.IsLarge as IsLarge,Stock.StockName,Stock.Address,Stock.Contact,Stock.Phone
                          from St_Shift,sys_user,DL_Item,DL_Master,Stock
                          where 1=1 and @StShiftStatus  and @dlmstatus and @dlitemstatus
                          and DL_Item.ItemID=St_Shift.ShiftID
                          and St_Shift.StockSysNoB=stock.sysno
                          and DL_Master.FreightUserSysNo=sys_user.sysno 
                          and DL_Master.sysno=DL_Item.dlsysno";


            string sql1 = "";
            sql = sql.Replace("@dlmstatus", " DL_Master.status <>" + (int)AppEnum.DLStatus.Abandon);  //必须是非作废的配送单
            sql = sql.Replace("@dlitemstatus", " DL_Item.status =" + (int)AppEnum.BiStatus.Valid);
            sql = sql.Replace("@StShiftStatus", "St_Shift.status>=" + (int)AppEnum.ShiftStatus.OutStock);


            if (paramHash != null && paramHash.Count != 0)
            {
                StringBuilder sb = new StringBuilder(100);
                foreach (string key in paramHash.Keys)
                {

                    object item = paramHash[key];

                    if (key == "DLSysNo")
                    {
                        sb.Append(" and ");
                        sb.Append("DL_Item.DLSysNo = ").Append(item.ToString());
                    }

                    else if (key == "orderby")
                    {
                        sql1 = " order by DL_Item.sysno desc ";
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
            else
            {
                sql = sql.Replace("select", "select top 50");
            }


            sql += sql1;
            return SqlHelper.ExecuteDataSet(sql);
        }
        public void DeleteItemRelated(int sysno, DLItemInfo dlItem, int DLItemType, int updateUser)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                Hashtable ht = new Hashtable();
                ht.Add("SysNo", sysno);
                ht.Add("FreightUserSysNo", AppConst.IntNull);
                ht.Add("SetDeliveryManTime", DateTime.Now);
                ht.Add("DLSysNo", dlItem.DLSysNo);

                if (DLItemType == (int)AppEnum.DLItemType.SaleOrder)
                    SaleManager.GetInstance().UpdateSOMaster(ht);
                else if (DLItemType == (int)AppEnum.DLItemType.RMARequest)
                    RMARequestManager.GetInstance().SetDeliveryMen(ht);
                else if (DLItemType == (int)AppEnum.DLItemType.RMARevert)
                    RMARevertManager.GetInstance().SetDeliveryMen(ht);
                else if (DLItemType == (int)AppEnum.DLItemType.RMAOutbound)
                    RMAOutBoundManager.GetInstance().SetDeliveryMen(ht);
                else if (DLItemType == (int)AppEnum.DLItemType.RMASendAccessory)
                    RMASendAccessoryManager.GetInstance().SetDeliveryMen(ht);
                else if (DLItemType == (int)AppEnum.DLItemType.StShift)
                    ShiftManager.GetInstance().UpdateShiftMaster(ht);

                DeliveryManSetListInfo oInfo = new DeliveryManSetListInfo();
                oInfo.ItemID = dlItem.ItemID;
                oInfo.SetUserSysNo = updateUser;
                oInfo.FreightUserSysNo = AppConst.IntNull;
                oInfo.CreateTime = DateTime.Now;
                oInfo.DLSysNo = dlItem.DLSysNo;
                DeliveryManager.GetInstance().InsertDeliveryMenSetList(oInfo);

                DLManager.GetInstance().DeleteDLItem(dlItem);

                scope.Complete();
            }
        }

        public void AddItemRelated(int sysno, int freightUserSysNo, DLItemInfo dlItem, int DLItemType, int updateUser)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                Hashtable ht = new Hashtable();
                ht.Add("SysNo", sysno);
                ht.Add("FreightUserSysNo", freightUserSysNo);
                ht.Add("SetDeliveryManTime", DateTime.Now);
                ht.Add("DLSysNo", dlItem.DLSysNo);


                if (DLItemType == (int)AppEnum.DLItemType.SaleOrder)
                    SaleManager.GetInstance().UpdateSOMaster(ht);
                else if (DLItemType == (int)AppEnum.DLItemType.RMARequest)
                    RMARequestManager.GetInstance().SetDeliveryMen(ht);
                else if (DLItemType == (int)AppEnum.DLItemType.RMARevert)
                    RMARevertManager.GetInstance().SetDeliveryMen(ht);
                else if (DLItemType == (int)AppEnum.DLItemType.RMAOutbound)
                    RMAOutBoundManager.GetInstance().SetDeliveryMen(ht);
                else if (DLItemType == (int)AppEnum.DLItemType.RMASendAccessory)
                    RMASendAccessoryManager.GetInstance().SetDeliveryMen(ht);
                else if (DLItemType == (int)AppEnum.DLItemType.StShift)
                    ShiftManager.GetInstance().UpdateShiftMaster(ht);

                DeliveryManSetListInfo oInfo = new DeliveryManSetListInfo();
                oInfo.ItemID = dlItem.ItemID;
                oInfo.SetUserSysNo = updateUser;
                oInfo.FreightUserSysNo = freightUserSysNo;
                oInfo.CreateTime = DateTime.Now;
                oInfo.DLSysNo = dlItem.DLSysNo;
                DeliveryManager.GetInstance().InsertDeliveryMenSetList(oInfo);


                DLManager.GetInstance().AddNewDLItem(dlItem);

                scope.Complete();
            }
        }

        public void ChangeFreight(DLInfo oDLMasterInfo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                //更新DL主表信息
                UpdateDLMaster(oDLMasterInfo);

                DLInfo oDLInfo = Load(oDLMasterInfo.SysNo);


                //更新各单据配送人信息

                DeliveryManSetListInfo deliveryInfo = new DeliveryManSetListInfo();
                deliveryInfo.SetUserSysNo = oDLMasterInfo.UpdateFreightManUserSysNo;
                deliveryInfo.FreightUserSysNo = oDLMasterInfo.FreightUserSysNo;
                deliveryInfo.CreateTime = DateTime.Now;
                deliveryInfo.DLSysNo = oDLMasterInfo.SysNo;
                foreach (DLItemInfo oItem in oDLInfo.ItemHash.Values)
                {
                    if (oItem.ItemType == (int)AppEnum.DLItemType.SaleOrder)
                    {
                        Hashtable ht = new Hashtable();
                        ht.Add("freightusersysno", oDLMasterInfo.FreightUserSysNo);
                        ht.Add("SetDeliveryManTime", System.DateTime.Now);
                        ht.Add("DLSysNo", oDLMasterInfo.SysNo);
                        ht.Add("SysNo", Int32.Parse(oItem.ItemID.Substring(1)));
                        SaleManager.GetInstance().UpdateSOMaster(ht);

                    }
                    else if (oItem.ItemType == (int)AppEnum.DLItemType.RMARequest)
                    {
                        Hashtable ht = new Hashtable();
                        ht.Add("freightusersysno", oDLMasterInfo.FreightUserSysNo);
                        ht.Add("SetDeliveryManTime", System.DateTime.Now);
                        ht.Add("DLSysNo", oDLMasterInfo.SysNo);
                        ht.Add("SysNo", Int32.Parse(oItem.ItemID.Substring(2)));
                        RMARequestManager.GetInstance().SetDeliveryMen(ht);
                    }
                    else if (oItem.ItemType == (int)AppEnum.DLItemType.RMARevert)
                    {
                        Hashtable ht = new Hashtable();
                        ht.Add("freightusersysno", oDLMasterInfo.FreightUserSysNo);
                        ht.Add("SetDeliveryManTime", System.DateTime.Now);
                        ht.Add("DLSysNo", oDLMasterInfo.SysNo);
                        ht.Add("SysNo", Int32.Parse(oItem.ItemID.Substring(2)));
                        RMARevertManager.GetInstance().SetDeliveryMen(ht);
                    }
                    else if (oItem.ItemType == (int)AppEnum.DLItemType.RMAOutbound)
                    {
                        Hashtable ht = new Hashtable();
                        ht.Add("freightusersysno", oDLMasterInfo.FreightUserSysNo);
                        ht.Add("SetDeliveryManTime", System.DateTime.Now);
                        ht.Add("DLSysNo", oDLMasterInfo.SysNo);
                        ht.Add("SysNo", Int32.Parse(oItem.ItemID.Substring(2)));
                        RMAOutBoundManager.GetInstance().SetDeliveryMen(ht);
                    }
                    else if (oItem.ItemType == (int)AppEnum.DLItemType.RMASendAccessory)
                    {
                        Hashtable ht = new Hashtable();
                        ht.Add("freightusersysno", oDLMasterInfo.FreightUserSysNo);
                        ht.Add("SetDeliveryManTime", System.DateTime.Now);
                        ht.Add("DLSysNo", oDLMasterInfo.SysNo);
                        ht.Add("SysNo", Int32.Parse(oItem.ItemID.Substring(2)));
                        RMASendAccessoryManager.GetInstance().SetDeliveryMen(ht);
                    }

                    deliveryInfo.ItemID = oItem.ItemID;
                    DeliveryManager.GetInstance().InsertDeliveryMenSetList(deliveryInfo);

                }
                scope.Complete();
            }
        }

        public DataSet GetDLCreateUserList(DateTime dtCreateFrom, int CreateUserStatus)
        {
            string sql = "select distinct su.sysno as createusersysno,su.username as createusername from sys_user su inner join DL_Master dl on su.sysno=dl.CreateUserSysNo where 1=1";
            if (dtCreateFrom != AppConst.DateTimeNull)
            {
                sql += " and dl.createtime >= " + Util.ToSqlString(dtCreateFrom.ToString(AppConst.DateFormat));
            }
            if (CreateUserStatus != AppConst.IntNull)
            {
                sql += " and su.status=" + Util.ToSqlString(CreateUserStatus.ToString());
            }
            sql += " order by su.username";
            return SqlHelper.ExecuteDataSet(sql);
        }
    }
}