using System;
using System.Data;
using System.Collections;
using System.Text;

using Icson.Utils;
using System.Transactions;

using Icson.Objects;
using Icson.DBAccess;
using Icson.DBAccess.Solution;
using Icson.Objects.Solution;

namespace Icson.BLL.Solution
{
    public class PrjManager
    {
        private static PrjManager _instance;
        public static PrjManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new PrjManager();
            }
            return _instance;
        }

        public string onlineShowLimit = "and Product.Status = " + ((int)AppEnum.ProductStatus.Show).ToString() + " and (Product_Price.ClearanceSale=1 or Product_Price.currentprice>=IsNull(Product_Price.unitcost,0))";

        #region Prj_Type
        private void mapPrjType(PrjTypeInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.SlnSysNo = Util.TrimIntNull(tempdr["SlnSysNo"]);
            oParam.ID = Util.TrimNull(tempdr["ID"]);
            oParam.Name = Util.TrimNull(tempdr["Name"]);
            oParam.Title = Util.TrimNull(tempdr["Title"]);
            oParam.Description = Util.TrimNull(tempdr["Description"]);
            oParam.OrderNum = Util.TrimIntNull(tempdr["OrderNum"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
        }

        public int InsertPrjType(PrjTypeInfo oParam)
        {
            string sql = "select * from prj_type where slnsysno = " + oParam.SlnSysNo + " and ID=" + Util.ToSqlString(oParam.ID);
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                throw new BizException("the same ID exists already");
            return new PrjDac().Insert(oParam);
        }

        public void UpdatePrjType(PrjTypeInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                new PrjDac().Update(oParam);
                scope.Complete();
            }
        }

        public PrjTypeInfo LoadPrjType(int SysNo)
        {
            string sql = "select * from prj_type where sysno=" + SysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            PrjTypeInfo oParam = new PrjTypeInfo();
            if (Util.HasMoreRow(ds))
                mapPrjType(oParam, ds.Tables[0].Rows[0]);
            else
                oParam = null;
            return oParam;
        }

        public PrjTypeInfo LoadPrjType(int SlnSysNo, int OrderNum)
        {
            string sql = @"select * from prj_type where slnsysno=@slnsysno and ordernum=@ordernum";
            sql = sql.Replace("@slnsysno", SlnSysNo.ToString()).Replace("@ordernum", OrderNum.ToString());
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            PrjTypeInfo oParam = new PrjTypeInfo();
            if (Util.HasMoreRow(ds))
                mapPrjType(oParam, ds.Tables[0].Rows[0]);
            else
                oParam = null;
            return oParam;
        }

        public int GetPrjTypeNewOrderNum(PrjTypeInfo oParam)
        {
            return new PrjDac().GetPrjTypeNewOrderNum(oParam);
        }

        public SortedList GetPrjTypeList(int SlnSysNo)
        {
            string sql = @"select * from prj_type where slnsysno = @slnsysno order by ordernum";
            sql = sql.Replace("@slnsysno", SlnSysNo.ToString());
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;

            SortedList sl = new SortedList(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PrjTypeInfo item = new PrjTypeInfo();
                mapPrjType(item, dr);
                sl.Add(item, null);
            }
            return sl;
        }

        public DataSet GetPrjTypeDs(int SlnSysNo, int Status)
        {
            string sql = @"select * from prj_type where slnsysno = @slnsysno ";
            if (Status > -2)
            {
                sql += " and status = @status";
            }
            sql += " order by ordernum";
            sql = sql.Replace("@slnsysno", SlnSysNo.ToString());
            sql = sql.Replace("@status", Status.ToString());
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;
            else
                return ds;
        }

        public void MoveTop(PrjTypeInfo oParam)
        {
            if (oParam.OrderNum == 1)
            {
                throw new BizException("it's the top one already");
            }
            SortedList sl = GetPrjTypeList(oParam.SlnSysNo);

            if (sl == null)
            {
                throw new BizException("no item for this solution");
            }

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                PrjDac o = new PrjDac();

                foreach (PrjTypeInfo item in sl.Keys)
                {
                    if (item.OrderNum < oParam.OrderNum)
                    {
                        item.OrderNum = item.OrderNum + 1;
                        o.SetOrderNum(item);
                    }
                }
                oParam.OrderNum = 1;
                o.SetOrderNum(oParam);

                scope.Complete();
            }
        }

        public void MoveUp(PrjTypeInfo oParam)
        {
            if (oParam.OrderNum == 1)
            {
                throw new BizException("it's the first one, can't be moved up");
            }
            SortedList sl = GetPrjTypeList(oParam.SlnSysNo);
            if (sl == null)
            {
                throw new BizException("no items");
            }

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                PrjDac o = new PrjDac();

                foreach (PrjTypeInfo item in sl.Keys)
                {
                    if (item.OrderNum == oParam.OrderNum - 1)
                    {
                        item.OrderNum += 1;
                        o.SetOrderNum(item);
                    }
                }
                oParam.OrderNum -= 1;
                o.SetOrderNum(oParam);

                scope.Complete();
            }
        }

        public void MoveDown(PrjTypeInfo oParam)
        {
            SortedList sl = GetPrjTypeList(oParam.SlnSysNo);
            if (sl == null)
            {
                throw new BizException("no items");
            }

            if (oParam.OrderNum == sl.Count)
            {
                throw new BizException("it's the last one, can't be moved down");
            }

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                PrjDac o = new PrjDac();

                foreach (PrjTypeInfo item in sl.Keys)
                {
                    if (item.OrderNum == oParam.OrderNum + 1)
                    {
                        item.OrderNum -= 1;
                        o.SetOrderNum(item);
                    }
                }
                oParam.OrderNum += 1;
                o.SetOrderNum(oParam);

                scope.Complete();
            }
        }

        public void MoveBottom(PrjTypeInfo oParam)
        {
            SortedList sl = GetPrjTypeList(oParam.SlnSysNo);
            if (sl == null)
            {
                throw new BizException("no items");
            }

            if (oParam.OrderNum == sl.Count)
            {
                throw new BizException("it's the last one, can't be moved down");
            }

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                PrjDac o = new PrjDac();

                foreach (PrjTypeInfo item in sl.Keys)
                {
                    if (item.OrderNum > oParam.OrderNum)
                    {
                        item.OrderNum = item.OrderNum - 1;
                        o.SetOrderNum(item);
                    }
                }
                oParam.OrderNum = sl.Count;
                o.SetOrderNum(oParam);

                scope.Complete();
            }
        }
        #endregion

        #region Prj_Master
        private void mapPrjMaster(PrjMasterInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.SlnSysNo = Util.TrimIntNull(tempdr["SlnSysNo"]);
            oParam.PrjTypeSysNo = Util.TrimIntNull(tempdr["PrjTypeSysNo"]);
            oParam.ID = Util.TrimNull(tempdr["ID"]);
            oParam.Name = Util.TrimNull(tempdr["Name"]);
            oParam.Title = Util.TrimNull(tempdr["Title"]);
            oParam.Description = Util.TrimNull(tempdr["Description"]);
            oParam.SysUserSysNo = Util.TrimIntNull(tempdr["SysUserSysNo"]);
            oParam.DateStamp = Util.TrimDateNull(tempdr["DateStamp"]);
            oParam.OrderNum = Util.TrimIntNull(tempdr["OrderNum"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
        }

        public int InsertPrjMaster(PrjMasterInfo oParam)
        {
            string sql = "select * from prj_master where slnsysno = " + oParam.SlnSysNo + " and ID=" + Util.ToSqlString(oParam.ID);
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                throw new BizException("the same ID exists already");
            return new PrjDac().Insert(oParam);
        }

        public void UpdatePrjMaster(PrjMasterInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                new PrjDac().Update(oParam);
                scope.Complete();
            }
        }

        public PrjMasterInfo LoadPrjMaster(int SysNo)
        {
            string sql = "select * from prj_master where sysno=" + SysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            PrjMasterInfo oParam = new PrjMasterInfo();
            if (Util.HasMoreRow(ds))
                mapPrjMaster(oParam, ds.Tables[0].Rows[0]);
            else
                oParam = null;
            return oParam;
        }

        public PrjMasterInfo LoadPrjMaster(int SlnSysNo, int OrderNum)
        {
            string sql = @"select * from prj_master where slnsysno=@slnsysno and ordernum=@ordernum";
            sql = sql.Replace("@slnsysno", SlnSysNo.ToString()).Replace("@ordernum", OrderNum.ToString());
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            PrjMasterInfo oParam = new PrjMasterInfo();
            if (Util.HasMoreRow(ds))
                mapPrjMaster(oParam, ds.Tables[0].Rows[0]);
            else
                oParam = null;
            return oParam;
        }

        public int GetPrjMasterNewOrderNum(PrjMasterInfo oParam)
        {
            return new PrjDac().GetPrjMasterNewOrderNum(oParam);
        }

        public SortedList GetPrjMasterList(int SlnSysNo)
        {
            string sql = @"select * from prj_master where slnsysno = @slnsysno order by ordernum";
            sql = sql.Replace("@slnsysno", SlnSysNo.ToString());
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;

            SortedList sl = new SortedList(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PrjMasterInfo item = new PrjMasterInfo();
                mapPrjMaster(item, dr);
                sl.Add(item, null);
            }
            return sl;
        }

        public DataSet GetPrjMasterDs(int SlnSysNo)
        {
            string sql = @"select prj_master.*,prj_type.name as prjtypename,sys_user.username 
                           from prj_master inner join prj_type on prj_master.prjtypesysno = prj_type.sysno 
                           inner join sys_user on prj_master.sysusersysno = sys_user.sysno 
                           where prj_master.slnsysno = @slnsysno order by prj_master.ordernum";
            sql = sql.Replace("@slnsysno", SlnSysNo.ToString());
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;
            else
                return ds;
        }

        public void MoveTop(PrjMasterInfo oParam)
        {
            if (oParam.OrderNum == 1)
            {
                throw new BizException("it's the top one already");
            }
            SortedList sl = GetPrjMasterList(oParam.SlnSysNo);

            if (sl == null)
            {
                throw new BizException("no item for this solution");
            }

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                PrjDac o = new PrjDac();

                foreach (PrjMasterInfo item in sl.Keys)
                {
                    if (item.OrderNum < oParam.OrderNum)
                    {
                        item.OrderNum = item.OrderNum + 1;
                        o.SetOrderNum(item);
                    }
                }
                oParam.OrderNum = 1;
                o.SetOrderNum(oParam);

                scope.Complete();
            }
        }

        public void MoveUp(PrjMasterInfo oParam)
        {
            if (oParam.OrderNum == 1)
            {
                throw new BizException("it's the first one, can't be moved up");
            }
            SortedList sl = GetPrjMasterList(oParam.SlnSysNo);
            if (sl == null)
            {
                throw new BizException("no items");
            }

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                PrjDac o = new PrjDac();

                foreach (PrjMasterInfo item in sl.Keys)
                {
                    if (item.OrderNum == oParam.OrderNum - 1)
                    {
                        item.OrderNum += 1;
                        o.SetOrderNum(item);
                    }
                }
                oParam.OrderNum -= 1;
                o.SetOrderNum(oParam);

                scope.Complete();
            }
        }

        public void MoveDown(PrjMasterInfo oParam)
        {
            SortedList sl = GetPrjMasterList(oParam.SlnSysNo);
            if (sl == null)
            {
                throw new BizException("no items");
            }

            if (oParam.OrderNum == sl.Count)
            {
                throw new BizException("it's the last one, can't be moved down");
            }

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                PrjDac o = new PrjDac();

                foreach (PrjMasterInfo item in sl.Keys)
                {
                    if (item.OrderNum == oParam.OrderNum + 1)
                    {
                        item.OrderNum -= 1;
                        o.SetOrderNum(item);
                    }
                }
                oParam.OrderNum += 1;
                o.SetOrderNum(oParam);

                scope.Complete();
            }
        }

        public void MoveBottom(PrjMasterInfo oParam)
        {
            SortedList sl = GetPrjMasterList(oParam.SlnSysNo);
            if (sl == null)
            {
                throw new BizException("no items");
            }

            if (oParam.OrderNum == sl.Count)
            {
                throw new BizException("it's the last one, can't be moved down");
            }

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                PrjDac o = new PrjDac();

                foreach (PrjMasterInfo item in sl.Keys)
                {
                    if (item.OrderNum > oParam.OrderNum)
                    {
                        item.OrderNum = item.OrderNum - 1;
                        o.SetOrderNum(item);
                    }
                }
                oParam.OrderNum = sl.Count;
                o.SetOrderNum(oParam);

                scope.Complete();
            }
        }
        #endregion

        #region Prj_Item
        private void mapPrjItem(PrjItemInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.PrjSysNo = Util.TrimIntNull(tempdr["PrjSysNo"]);
            oParam.SlnItemSysNo = Util.TrimIntNull(tempdr["SlnItemSysNo"]);
            oParam.DefaultProductSysNo = Util.TrimIntNull(tempdr["DefaultProductSysNo"]);
            oParam.DefaultQty = Util.TrimIntNull(tempdr["DefaultQty"]);
            oParam.IsShowPic = Util.TrimIntNull(tempdr["IsShowPic"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
        }

        public int InsertPrjItem(PrjItemInfo oParam)
        {
            string sql = "select * from prj_item where prjsysno = " + oParam.PrjSysNo + " and SlnItemSysNo=" + oParam.SlnItemSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                throw new BizException("the same exists already");
            return new PrjDac().Insert(oParam);
        }

        public int InsertUpdatePrjItem(PrjItemInfo oParam)
        {
            string sql = "select * from prj_item where prjsysno = " + oParam.PrjSysNo + " and SlnItemSysNo=" + oParam.SlnItemSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                oParam.SysNo = Util.TrimIntNull(ds.Tables[0].Rows[0]["sysno"]);
                return new PrjDac().Update(oParam);
            }
            else
            {
                return new PrjDac().Insert(oParam);
            }
        }

        public void UpdatePrjItem(PrjItemInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                new PrjDac().Update(oParam);
                scope.Complete();
            }
        }

        public PrjItemInfo LoadPrjItem(int SysNo)
        {
            string sql = "select * from prj_item where sysno=" + SysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            PrjItemInfo oParam = new PrjItemInfo();
            if (Util.HasMoreRow(ds))
                mapPrjItem(oParam, ds.Tables[0].Rows[0]);
            else
                oParam = null;
            return oParam;
        }

        public SortedList GetPrjItemList(int PrjSysNo)
        {
            string sql = @"select * from prj_item where prjsysno = @prjysno";
            sql = sql.Replace("@prjysno", PrjSysNo.ToString());
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;

            SortedList sl = new SortedList(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                PrjItemInfo item = new PrjItemInfo();
                mapPrjItem(item, dr);
                sl.Add(item, null);
            }
            return sl;
        }

        public DataSet GetPrjItemDs(int PrjSysNo, int Status)
        {
            string sql = @"select prj_item.*,sln_item.id as slnitemid,sln_item.name as slnitemname 
                           from prj_item inner join sln_item on prj_item.slnitemsysno=sln_item.sysno 
                           where sln_item.status=0 and prj_item.prjsysno=@prjsysno and prj_item.status=@status 
                           order by sln_item.ordernum";
            sql = sql.Replace("@prjsysno", PrjSysNo.ToString());
            sql = sql.Replace("@status", Status.ToString());
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;
            else
                return ds;
        }

        public void UpdatePrjItemList(Hashtable ht)
        {
            if (ht == null || ht.Count == 0)
                return;
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                int i = 0;
                foreach (PrjItemInfo oParam in ht.Values)
                {
                    if (i == 0)
                    {
                        string sql = "update prj_item set status=-1 where prjsysno=" + oParam.PrjSysNo;
                        SqlHelper.ExecuteNonQuery(sql);
                        i++;
                    }

                    InsertUpdatePrjItem(oParam);
                }
                scope.Complete();
            }
        }

        public DataSet GetPrjItemDs(int PrjSysNo)
        {
            string sql = @"select sln_item.sysno as slnitemsysno,sln_item.name,sln_item.title,sln_item.ordernum,
                           prj_item.prjsysno,prj_item.sysno as prjitemsysno,prj_item.defaultproductsysno,prj_item.defaultqty,prj_item.isshowpic,prj_item_filter.filter 
                           from sln_item inner join prj_item on prj_item.slnitemsysno=sln_item.sysno inner join prj_item_filter on prj_item.sysno=prj_item_filter.prjitemsysno 
                           where sln_item.status=0 and prj_item.status=0
                           order by sln_item.ordernum";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;
            return ds;
        }
        #endregion

        #region prj_item_filter
        private void mapPrjItemFilterInfo(PrjItemFilterInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.PrjItemSysNo = Util.TrimIntNull(tempdr["PrjItemSysNo"]);
            oParam.Filter = Util.TrimNull(tempdr["Filter"]);
            oParam.PriceFrom = Util.TrimDecimalNull(tempdr["PriceFrom"]);
            oParam.PriceTo = Util.TrimDecimalNull(tempdr["PriceTo"]);
        }

        public PrjItemFilterInfo LoadPrjItemFilter(int PrjItemSysNo)
        {
            string sql = "select * from prj_item_filter where prjitemsysno=" + PrjItemSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            PrjItemFilterInfo oParam = new PrjItemFilterInfo();
            if (Util.HasMoreRow(ds))
                mapPrjItemFilterInfo(oParam, ds.Tables[0].Rows[0]);
            else
                oParam = null;
            return oParam;
        }

        public int InsertUpdatePrjItemFilter(PrjItemFilterInfo oParam)
        {
            string sql = "select * from prj_item_filter where prjitemsysno = " + oParam.PrjItemSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                oParam.SysNo = Util.TrimIntNull(ds.Tables[0].Rows[0]["sysno"]);
                return new PrjDac().Update(oParam);
            }
            else
            {
                return new PrjDac().Insert(oParam);
            }
        }
        #endregion

        //step1:category3, step2:attribute2, step3:option
        public DataSet GetPrjItemAttributeOptionDs(int SlnItemSysNo)
        {
            StringBuilder sb = new StringBuilder();
            string sql = @"select category3.sysno as c3sysno,category3.c3name from sln_item 
                           inner join sln_item_c3 on sln_item.sysno=sln_item_c3.slnitemsysno 
                           inner join category3 on category3.sysno=sln_item_c3.c3sysno 
                           where sln_item.status=0 and sln_item_c3.status=0 and category3.status=0 
                           and slnitemsysno=@slnitemsysno";
            sql = sql.Replace("@slnitemsysno", SlnItemSysNo.ToString());
            sb.Append(sql + ";");

            DataSet dsC3 = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(dsC3))
                return null;

            foreach (DataRow drC3 in dsC3.Tables[0].Rows)
            {
                sql = @"select category_attribute2.sysno as attribute2sysno,category_attribute2.attribute2name as attribute2name from sln_item_c3 
                           inner join sln_item_c3_attr2 on sln_item_c3.sysno=sln_item_c3_attr2.slnitemc3sysno  
                           inner join category_attribute2 on sln_item_c3_attr2.c3attr2sysno = category_attribute2.sysno 
                           inner join category_attribute1 on category_attribute1.sysno=category_attribute2.a1sysno 
                           where sln_item_c3.status=0 and sln_item_c3_attr2.status=0 and category_attribute2.status=0 
                           and sln_item_c3.slnitemsysno=@slnitemsysno  
                           and sln_item_c3.c3sysno=@c3sysno 
                           order by category_attribute2.attribute2type,category_attribute1.ordernum,category_attribute2.ordernum";
                sql = sql.Replace("@slnitemsysno", SlnItemSysNo.ToString());
                sql = sql.Replace("@c3sysno", Util.TrimNull(drC3["c3sysno"]));

                sb.Append(sql + ";");
                DataSet dsA2 = SqlHelper.ExecuteDataSet(sql);
                if (!Util.HasMoreRow(dsA2))
                {
                    continue;
                }
                else
                {
                    foreach (DataRow drA2 in dsA2.Tables[0].Rows)
                    {
                        sql = @"select sysno as attribute2optionsysno,attribute2optionname from category_attribute2_option 
                                where attribute2sysno=@attribute2sysno and status=0 order by ordernum";
                        sql = sql.Replace("@attribute2sysno", Util.TrimNull(drA2["attribute2sysno"]));

                        sb.Append(sql + ";");
                    }
                }
            }
            sql = sb.ToString();
            return SqlHelper.ExecuteDataSet(sql.Substring(0, sql.Length - 1));
        }

        /// <summary>
        /// 根据filter字符串,转换成sql
        /// </summary>
        /// <param name="Filter"></param>
        /// <param name="HasOnlineQty">在线数量是否>0</param>
        /// <returns></returns>
        public string GetFilterSql(string Filter, bool HasOnlineQty, decimal PriceFrom, decimal PriceTo)
        {
            if (Filter == null || Filter.Length == 0)
            {
                return "";
            }

            string sql = @"select
								product.sysno as productsysno,product.productid, productname, productmode, availableqty+virtualqty as onlineqty, product.producttype,
								product_price.currentprice,product.createtime,product_attribute2_summary.summary 
							from 
								product, inventory, product_price ,product_attribute2_summary 
							where 
								product.sysno = inventory.productsysno 
							and product.sysno = product_price.productsysno 
                            and product.sysno = product_attribute2_summary.productsysno 
							@onlineShowLimit";
            if (HasOnlineQty)
            {
                sql += " and availableqty+virtualqty > 0 ";
            }

            if (PriceFrom > 0)
            {
                sql += " and product_price.currentprice >= " + PriceFrom.ToString();
            }

            if (PriceTo > 0)
            {
                sql += " and product_price.currentprice <= " + PriceTo.ToString();
            }

            string[] c3Filter = Filter.Split('|');  //Multi category3
            string c3Temp = "";
            for (int i = 0; i < c3Filter.Length; i++)
            {
                string[] a2Filter = c3Filter[i].Split('&');  //Multi attribute2
                string a2Temp = "";
                for (int j = 0; j < a2Filter.Length; j++)
                {
                    string[] optionFilter = a2Filter[j].Split(',');  //Multi option
                    string optionTemp = "";
                    for (int k = 0; k < optionFilter.Length; k++)
                    {
                        optionTemp += " or exists(select productsysno from product_attribute2 where product_attribute2.attribute2optionsysno='" + optionFilter[k] + "' and product_attribute2.productsysno=product.sysno) ";
                    }
                    optionTemp = optionTemp.Substring(3);
                    optionTemp = " ( " + optionTemp + " ) ";

                    a2Temp += " and " + optionTemp;
                }
                a2Temp = " ( " + a2Temp.Substring(4) + " ) ";

                c3Temp += " or " + a2Temp;
            }
            c3Temp = " ( " + c3Temp.Substring(3) + " ) ";

            sql += " and " + c3Temp;

            sql = sql.Replace("@onlineShowLimit", this.onlineShowLimit);
            //sql += " and productid not like '%R' and productid not like '%B' ";
            sql += " and product.producttype<>1 and product.producttype<>2 ";
            sql += " order by currentprice";

            return sql;
        }

        /// <summary>
        /// 获得符合筛选条件的商品dataset
        /// </summary>
        /// <param name="Filter"></param>
        /// <param name="HasOnelineQty">在线数量是否>0</param>
        /// <returns></returns>
        public DataSet GetFilterProductDs(string Filter, bool HasOnelineQty, decimal PriceFrom, decimal PriceTo)
        {
            string sql = GetFilterSql(Filter, HasOnelineQty, PriceFrom, PriceTo);
            if (sql.Length == 0)
                return null;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;
            return ds;
        }

        /// <summary>
        /// 获得project detail dataset, table 0:project item list, table 1:project item 1 products, table 2:....
        /// </summary>
        /// <param name="PrjSysNo"></param>
        /// <returns></returns>
        public DataSet GetPrjDetailDs(int PrjSysNo)
        {
            string sql = @"select sln_item.sysno as slnitemsysno,sln_item.name,sln_item.title,sln_item.ordernum,
                           prj_item.prjsysno,prj_item.sysno as prjitemsysno,prj_item.defaultproductsysno,prj_item.defaultqty,prj_item.isshowpic,
                           prj_item_filter.filter,prj_item_filter.pricefrom,prj_item_filter.priceto 
                           from sln_item inner join prj_item on prj_item.slnitemsysno=sln_item.sysno inner join prj_item_filter on prj_item.sysno=prj_item_filter.prjitemsysno 
                           where sln_item.status=0 and prj_item.status=0 and prj_item.prjsysno=@prjsysno 
                           order by sln_item.ordernum";
            sql = sql.Replace("@prjsysno", PrjSysNo.ToString());
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;

            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sb.Append(GetFilterSql(Util.TrimNull(dr["Filter"]), true, Util.TrimDecimalNull(dr["pricefrom"].ToString()), Util.TrimDecimalNull(dr["priceto"].ToString())) + ";");
            }

            sql += ";" + sb.ToString().Substring(0, sb.Length - 1);

            return SqlHelper.ExecuteDataSet(sql);
        }

        /// <summary>
        /// 获得project default detail dataset
        /// </summary>
        /// <param name="PrjSysNo"></param>
        /// <returns></returns>
        public DataSet GetPrjDefaultDetailDs(int PrjSysNo)
        {
            string sql = @"SELECT Sln_Item.SlnSysNo, Sln_Item.Name AS slnitemname, 
                                    Prj_Item.PrjSysNo, Prj_Item.DefaultQty, Prj_Item.IsShowPic, Prj_Master.Name AS prjname, Prj_Master.Title AS prjtitle,
                                    Product.SysNo as ProductSysNo, Product.ProductID, Product.ProductName, 
                                    Product_Price.CurrentPrice
                                    FROM Sln_Item INNER JOIN
                                    Prj_Master INNER JOIN
                                    Prj_Item ON Prj_Master.SysNo = Prj_Item.PrjSysNo ON 
                                    Sln_Item.SysNo = Prj_Item.SlnItemSysNo INNER JOIN
                                    Product_Price INNER JOIN
                                    Product ON Product_Price.ProductSysNo = Product.SysNo ON 
                                    Prj_Item.DefaultProductSysNo = Product.SysNo
                                    WHERE (Sln_Item.Status = 0) AND (Prj_Master.Status = 0) AND 
                                    (Prj_Item.Status = 0) AND (Prj_Master.SysNo=@prjsysno )
                                    ORDER BY Prj_Master.OrderNum, Sln_Item.OrderNum";
            sql = sql.Replace("@prjsysno", PrjSysNo.ToString());
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;

            return ds;
        }

        /// <summary>
        /// 获得project list推荐默认的详细, table 0:project list, table 1: project 1 detail, table 2:.....
        /// </summary>
        /// <param name="PrjTypeSysNo"></param>
        /// <returns></returns>
        public DataSet GetPrjListDefaultDetailDs(int PrjTypeSysNo)
        {
            string sql = @"select top 1 sysno as prjsysno,name as prjname,title as prjtitle,description as prjdescription from prj_master where status=0 and prjtypesysno=" + PrjTypeSysNo.ToString() + " order by ordernum";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;

            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string sqlTemp = @"SELECT Sln_Item.SlnSysNo, Sln_Item.Name AS slnitemname, 
                                    Prj_Item.PrjSysNo, Prj_Item.DefaultQty, Prj_Item.IsShowPic, Prj_Master.Name AS prjname, Prj_Master.Title AS prjtitle,
                                    Product.SysNo as ProductSysNo, Product.ProductID, Product.ProductName, Product.PromotionWord,
                                    Product_Price.CurrentPrice
                                    FROM Sln_Item INNER JOIN
                                    Prj_Master INNER JOIN
                                    Prj_Item ON Prj_Master.SysNo = Prj_Item.PrjSysNo ON 
                                    Sln_Item.SysNo = Prj_Item.SlnItemSysNo INNER JOIN
                                    Product_Price INNER JOIN
                                    Product ON Product_Price.ProductSysNo = Product.SysNo ON 
                                    Prj_Item.DefaultProductSysNo = Product.SysNo 
                                    WHERE (Sln_Item.Status = 0) AND (Prj_Master.Status = 0) AND 
                                    (Prj_Item.Status = 0) AND (Prj_Master.SysNo=@prjsysno )                                     
                                    ORDER BY Prj_Master.OrderNum, Sln_Item.OrderNum";
                sqlTemp = sqlTemp.Replace("@prjsysno", Util.TrimNull(dr["prjsysno"].ToString()));
                sb.Append(sqlTemp + ";");
            }
            sql += ";" + sb.ToString().Substring(0, sb.Length - 1);
            return SqlHelper.ExecuteDataSet(sql);
        }

        /// <summary>
        /// 根据SysNo获得project 推荐默认的详细, table 0:project list, table 1: project 1 detail
        /// </summary>
        /// <param name="PrjTypeSysNo"></param>
        /// <returns></returns>
        public DataSet GetPrjDetailDsBySysNo(int InputPrjSysNo)
        {
            string sql = @"select top 1 sysno as prjsysno,name as prjname,title as prjtitle,description as prjdescription from prj_master where status=0 and sysno=" + InputPrjSysNo.ToString() + " order by ordernum";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;

            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string sqlTemp = @"SELECT Sln_Item.SlnSysNo, Sln_Item.Name AS slnitemname, 
                                    Prj_Item.PrjSysNo, Prj_Item.DefaultQty, Prj_Item.IsShowPic, Prj_Master.Name AS prjname, Prj_Master.Title AS prjtitle,
                                    Product.SysNo as ProductSysNo, Product.ProductID, Product.ProductName, Product.PromotionWord,
                                    Product_Price.CurrentPrice
                                    FROM Sln_Item INNER JOIN
                                    Prj_Master INNER JOIN
                                    Prj_Item ON Prj_Master.SysNo = Prj_Item.PrjSysNo ON 
                                    Sln_Item.SysNo = Prj_Item.SlnItemSysNo INNER JOIN
                                    Product_Price INNER JOIN
                                    Product ON Product_Price.ProductSysNo = Product.SysNo ON 
                                    Prj_Item.DefaultProductSysNo = Product.SysNo 
                                    WHERE (Sln_Item.Status = 0) AND (Prj_Master.Status = 0) AND 
                                    (Prj_Item.Status = 0) AND (Prj_Master.SysNo=@prjsysno )                                     
                                    ORDER BY Prj_Master.OrderNum, Sln_Item.OrderNum";
                sqlTemp = sqlTemp.Replace("@prjsysno", Util.TrimNull(dr["prjsysno"].ToString()));
                sb.Append(sqlTemp + ";");
            }
            sql += ";" + sb.ToString().Substring(0, sb.Length - 1);
            return SqlHelper.ExecuteDataSet(sql);
        }

        /// <summary>
        /// 获得project detail html for ProjectDetail.aspx
        /// </summary>
        /// <param name="PrjSysNo"></param>
        /// <returns></returns>
        public string GetPrjDetailHtml(int PrjSysNo)
        {
            DataSet ds = GetPrjDetailDs(PrjSysNo);
            if (ds == null)
                return "";

            StringBuilder sb = new StringBuilder();
            StringBuilder sbNav = new StringBuilder(); //Navigate to item
            StringBuilder sbDiv = new StringBuilder(); //Div for product's summary

            sbNav.Append("    <tr>");
            sbNav.Append("        <td align=center colspan=4>");
            sbNav.Append("             <table>");
            sbNav.Append("                 <tr>");

            int i = 1;
            foreach (DataRow drItem in ds.Tables[0].Rows)  //CPU, motherboard, memory
            {
                if (ds.Tables.Count > i && ds.Tables[i].Rows.Count > 0)
                {
                    sbNav.Append("             <td class=title><a href=#item" + i.ToString() + ">" + Util.TrimNull(drItem["name"]) + "</a></td>");

                    sb.Append("<tr><td align=center class=title colspan=4><a name=\"item" + i.ToString() + "\">" + Util.TrimNull(drItem["Name"]) + "</a></td></tr>");
                    int j = 0;

                    foreach (DataRow drProduct in ds.Tables[i].Rows)
                    {
                        bool IsDefaultProduct = false;
                        if (Util.TrimIntNull(drProduct["ProductSysNo"].ToString()) == Util.TrimIntNull(drItem["DefaultProductSysNo"].ToString()))
                        {
                            IsDefaultProduct = true;
                        }

                        sb.Append("        <tr>");
                        sb.Append("            <td class=td>");
                        sb.Append("			   <a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + drProduct["ProductSysNo"].ToString() + "')\"><img id=imgProduct" + drProduct["ProductSysNo"].ToString() + " src='").Append(AppConfig.PicturePath + "small/" + Util.TrimNull(drProduct["ProductID"]) + ".jpg").Append("' alt='点击查看大图' width='80' height='60' border='0'></a>");
                        sb.Append("            </td>");
                        sb.Append("            <td>");
                        sb.Append("            <input id=rdoProduct" + Util.TrimIntNull(drProduct["ProductSysNo"]).ToString()); //不显示图片 radio id= "rdoProduct" + ProductSysNo
                        if (Util.TrimIntNull(drItem["IsShowPic"]) == 0)  //是否显示图片
                        {
                            sb.Append("_" + Util.TrimNull(drProduct["ProductID"]));  //显示图片 radio id= "rdoProduct" + ProductSysNo + "_" + ProductID
                        }
                        sb.Append("            name=rdoItem" + Util.TrimIntNull(drItem["prjitemsysno"]).ToString() + " type=radio value=" + Util.TrimIntNull(drProduct["ProductSysNo"]).ToString());
                        if (j == 0 || IsDefaultProduct)
                        {
                            sb.Append("        checked ");
                        }
                        sb.Append("            onclick=\"CalTotal();\" />");

                        sbDiv.Append("<div style=\"display: none;\" id=div" + Util.TrimIntNull(drProduct["ProductSysNo"]).ToString() + ">" + Util.TrimNull(drProduct["Summary"]) + "</div>");

                        sb.Append("            <a id=lblProduct" + Util.TrimIntNull(drProduct["ProductSysNo"].ToString()).ToString() + " href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(drProduct["ProductSysNo"]).ToString() + "' target='_blank'>" + Util.TrimNull(drProduct["ProductName"]) + "</a>");
                        sb.Append("            </td>");
                        sb.Append("            <td class=td>");
                        sb.Append("            <span id=spanProductPrice" + Util.TrimIntNull(drProduct["ProductSysNo"]).ToString() + " class=icson><strong>￥" + Util.ToMoney(drProduct["CurrentPrice"].ToString()) + "</strong></span>");
                        sb.Append("            <input id=txtProductPrice" + Util.TrimIntNull(drProduct["ProductSysNo"]).ToString() + " value=\"" + Util.ToMoney(drProduct["CurrentPrice"].ToString()).ToString() + "\" type=text style=\"width:0px;visibility:hidden\" />");
                        sb.Append("            </td>");
                        sb.Append("            <td class=td>");
                        sb.Append("            <select");
                        if (j == 0 || IsDefaultProduct)
                        {
                            sb.Append("        style=\"visibility:visible\"");
                        }
                        else
                        {
                            sb.Append("        style=\"visibility:hidden\"");
                        }
                        sb.Append("            onchange=\"CalTotal();\" ");
                        sb.Append("            name=selQty" + Util.TrimIntNull(drProduct["ProductSysNo"]).ToString() + " id=selQty" + Util.TrimIntNull(drProduct["ProductSysNo"]).ToString() + ">");
                        if (IsDefaultProduct)
                        {
                            sb.Append("            <option value=1 ");
                            if (Util.TrimIntNull(drItem["DefaultQty"].ToString()) == 1)
                            {
                                sb.Append("        selected='selected'");
                            }
                            sb.Append("            >1</option><option value=2");
                            if (Util.TrimIntNull(drItem["DefaultQty"].ToString()) == 2)
                            {
                                sb.Append("        selected='selected'");
                            }
                            sb.Append("            >2</option><option value=3");
                            if (Util.TrimIntNull(drItem["DefaultQty"].ToString()) == 3)
                            {
                                sb.Append("        selected='selected'");
                            }
                            sb.Append("            >3</option><option value=4");
                            if (Util.TrimIntNull(drItem["DefaultQty"].ToString()) == 4)
                            {
                                sb.Append("        selected='selected'");
                            }
                            sb.Append("            >4</option>");
                        }
                        else
                        {
                            sb.Append("            <option selected='selected' value=1>1</option><option value=2>2</option><option value=3>3</option><option value=4>4</option>");
                        }
                        sb.Append("            </select>");
                        sb.Append("            </td>");
                        sb.Append("        </tr>");
                        j++;
                    }
                    sb.Append("            <tr>");
                    sb.Append("                <td class=td>");
                    sb.Append("                &nbsp;");
                    sb.Append("                </td>");
                    sb.Append("                <td class=td>");
                    sb.Append("                <input ");
                    if (Util.TrimNull(drItem["DefaultProductSysNo"]) == "0")
                    {
                        sb.Append("            checked ");
                    }
                    sb.Append("                id=rdoNoProduct" + Util.TrimIntNull(drItem["prjitemsysno"]).ToString() + " name=rdoItem" + Util.TrimIntNull(drItem["prjitemsysno"]).ToString() + " type=radio value=0 ");
                    sb.Append("                onclick=\"CalTotal();\" />");
                    sb.Append("                <label>不选择" + Util.TrimNull(drItem["Name"]) + "</label>");
                    sb.Append("                </td>");
                    sb.Append("                <td class=td>");
                    sb.Append("                <span class=icson><strong>[￥0.00]" + "</strong></span>");
                    sb.Append("                </td>");
                    sb.Append("                <td class=td>");
                    sb.Append("                &nbsp;");
                    sb.Append("                </td>");
                    sb.Append("            </tr>");
                }
                i++;
            }

            sbNav.Append("                 </tr>");
            sbNav.Append("             </table>");
            sbNav.Append("        </td>");
            sbNav.Append("    </tr>");

            PrjMasterInfo oInfo = LoadPrjMaster(PrjSysNo);

            string ProjectTitle = "<tr><td align=center class=ct2 colspan=4>" + Util.TrimNull(oInfo.Title) + "</td></tr>";
            string ColumnName = "<tr><td class=icson align=center>图片</td><td class=icson align=center>名称</td><td class=icson align=center>价格</td><td class=icson align=center>数量</td></tr>";
            sb.Insert(0, "<table align=center width=90% class=specification>" + ProjectTitle + sbNav.ToString() + ColumnName);
            sb.Append("</table>");

            return sb.ToString();// +sbDiv.ToString();
        }

        /// <summary>
        /// 获得project detail for PrjItemDefaultOpt.aspx in IAS
        /// </summary>
        /// <param name="PrjSysNo"></param>
        /// <returns></returns>
        public string GetPrjDetailHtmlIas(int PrjSysNo)
        {
            DataSet ds = GetPrjDetailDs(PrjSysNo);
            if (ds == null)
                return "";

            StringBuilder sb = new StringBuilder();
            StringBuilder sbNav = new StringBuilder(); //Navigate to item

            sbNav.Append("    <tr>");
            sbNav.Append("        <td align=center colspan=3>");
            sbNav.Append("             <table>");
            sbNav.Append("                 <tr>");

            int i = 1;
            foreach (DataRow drItem in ds.Tables[0].Rows)  //CPU, motherboard, memory
            {
                if (ds.Tables.Count > i && ds.Tables[i].Rows.Count > 0)
                {
                    sbNav.Append("             <td class=title><a href=#item" + i.ToString() + ">" + Util.TrimNull(drItem["name"]) + "</a></td>");

                    sb.Append("<tr><td align=center class=title colspan=3><a name=\"item" + i.ToString() + "\">" + Util.TrimNull(drItem["Name"]) + "</a>&nbsp;&nbsp;");
                    sb.Append(" <input id=chkItem" + Util.TrimNull(drItem["prjitemsysno"]) + " name=chkItem" + Util.TrimNull(drItem["prjitemsysno"]));
                    if (Util.TrimIntNull(drItem["isshowpic"].ToString()) == 0)
                    {
                        sb.Append(" checked ");
                    }
                    sb.Append(" type=checkbox />组合图片元素</td></tr>");

                    int j = 0;
                    foreach (DataRow drProduct in ds.Tables[i].Rows)
                    {
                        bool IsDefaultProduct = false;
                        if (Util.TrimIntNull(drProduct["ProductSysNo"].ToString()) == Util.TrimIntNull(drItem["DefaultProductSysNo"].ToString()))
                        {
                            IsDefaultProduct = true;
                        }
                        sb.Append("        <tr>");
                        sb.Append("            <td>");
                        sb.Append("            <input id=rdoProduct" + Util.TrimIntNull(drProduct["ProductSysNo"]).ToString() + " name=rdoItem" + Util.TrimIntNull(drItem["prjitemsysno"]).ToString() + " type=radio value=" + Util.TrimIntNull(drItem["PrjItemSysNo"].ToString()).ToString() + "_" + Util.TrimIntNull(drProduct["ProductSysNo"]).ToString());
                        if (j == 0 || IsDefaultProduct)
                        {
                            sb.Append("        checked ");
                        }
                        sb.Append("            onclick=\"CalTotal();\" />");
                        sb.Append("            <a id=lblProduct" + Util.TrimIntNull(drProduct["ProductSysNo"].ToString()).ToString() + " href=http://www.icson.com/Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(drProduct["ProductSysNo"]).ToString() + " target='_blank'>" + Util.TrimNull(drProduct["ProductName"]) + "</a>");
                        sb.Append("            </td>");
                        sb.Append("            <td class=td>");
                        sb.Append("            <span id=spanProductPrice" + Util.TrimIntNull(drProduct["ProductSysNo"]).ToString() + " class=icson><strong>￥" + Util.ToMoney(drProduct["CurrentPrice"].ToString()) + "</strong></span>");
                        sb.Append("            <input id=txtProductPrice" + Util.TrimIntNull(drProduct["ProductSysNo"]).ToString() + " value=\"" + Util.ToMoney(drProduct["CurrentPrice"].ToString()).ToString() + "\" type=text style=\"width:0px;visibility:hidden\" />");
                        sb.Append("            </td>");
                        sb.Append("            <td class=td>");
                        sb.Append("            <select");
                        if (j == 0 || IsDefaultProduct)
                        {
                            sb.Append("        style=\"visibility:visible\"");
                        }
                        else
                        {
                            sb.Append("        style=\"visibility:hidden\"");
                        }
                        sb.Append("            onchange=\"CalTotal();\" ");
                        sb.Append("            name=selQty" + Util.TrimIntNull(drProduct["ProductSysNo"]).ToString() + " id=selQty" + Util.TrimIntNull(drProduct["ProductSysNo"]).ToString() + ">");
                        if (IsDefaultProduct)
                        {
                            sb.Append("            <option value=1 ");
                            if (Util.TrimIntNull(drItem["DefaultQty"].ToString()) == 1)
                            {
                                sb.Append("        selected='selected'");
                            }
                            sb.Append("            >1</option><option value=2");
                            if (Util.TrimIntNull(drItem["DefaultQty"].ToString()) == 2)
                            {
                                sb.Append("        selected='selected'");
                            }
                            sb.Append("            >2</option><option value=3");
                            if (Util.TrimIntNull(drItem["DefaultQty"].ToString()) == 3)
                            {
                                sb.Append("        selected='selected'");
                            }
                            sb.Append("            >3</option><option value=4");
                            if (Util.TrimIntNull(drItem["DefaultQty"].ToString()) == 4)
                            {
                                sb.Append("        selected='selected'");
                            }
                            sb.Append("            >4</option>");
                        }
                        else
                        {
                            sb.Append("            <option selected='selected' value=1>1</option><option value=2>2</option><option value=3>3</option><option value=4>4</option>");
                        }
                        sb.Append("            </select>");
                        sb.Append("            </td>");
                        sb.Append("        </tr>");
                        j++;
                    }
                    sb.Append("            <tr>");
                    sb.Append("                <td class=td>");
                    sb.Append("                <input");
                    if (Util.TrimNull(drItem["DefaultProductSysNo"]) == "0")
                    {
                        sb.Append("            checked ");
                    }
                    sb.Append("                id=rdoNoProduct" + Util.TrimIntNull(drItem["prjitemsysno"]).ToString() + " name=rdoItem" + Util.TrimIntNull(drItem["prjitemsysno"]).ToString() + " type=radio value=" + Util.TrimIntNull(drItem["PrjItemSysNo"].ToString()).ToString() + "_0");
                    sb.Append("                onclick=\"CalTotal();\" />");
                    sb.Append("                <label>不选择" + Util.TrimNull(drItem["Name"]) + "</label>");
                    sb.Append("                </td>");
                    sb.Append("                <td class=td>");
                    sb.Append("                <span class=icson><strong>[￥0.00]" + "</strong></span>");
                    sb.Append("                </td>");
                    sb.Append("                <td class=td>");
                    sb.Append("                &nbsp;");
                    sb.Append("                </td>");
                    sb.Append("            </tr>");
                }
                i++;
            }

            sbNav.Append("                 </tr>");
            sbNav.Append("             </table>");
            sbNav.Append("        </td>");
            sbNav.Append("    </tr>");

            PrjMasterInfo oInfo = LoadPrjMaster(PrjSysNo);

            string ProjectTitle = "<tr><td align=center class=ct2 colspan=3>" + Util.TrimNull(oInfo.Title) + "</td></tr>";
            string ColumnName = "<tr><td class=icson align=center>名称</td><td class=icson align=center>价格</td><td class=icson align=center>数量</td></tr>";
            sb.Insert(0, "<table align=center width=90% class=specification>" + ProjectTitle + sbNav.ToString() + ColumnName);
            sb.Append("</table>");

            return sb.ToString();
        }
    }
}