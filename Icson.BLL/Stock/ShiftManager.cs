using System;
using System.Data;
using System.Collections;
using System.Text;

using Icson.Objects;
using Icson.Objects.Basic;
using Icson.Utils;
using System.Transactions;
using Icson.Objects.Stock;
using Icson.DBAccess;
using Icson.DBAccess.Stock;
using Icson.BLL.Basic;
using Icson.BLL.RMA;

namespace Icson.BLL.Stock
{
    /// <summary>
    /// Summary description for ShiftManager.
    /// </summary>
    public class ShiftManager
    {
        private ShiftManager()
        {
        }

        private static ShiftManager _instance;
        public static ShiftManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ShiftManager();
            }
            return _instance;
        }

        public DataSet GetShiftListDs(Hashtable paramHash)
        {
            string sql = @" select st.*, b.username as CreateUserName, c.username as AuditUserName, d.username as OutUserName, e.username as InUserName,
							stocka.stockname as stocknameA, stockb.stockname as stocknameB 
							from st_shift st, sys_user b, sys_user c, sys_user d , sys_user e, stock as stocka, stock as stockb
							where 
								st.createusersysno *= b.sysno
								and st.auditusersysno *= c.sysno
								and st.outusersysno *= d.sysno
								and st.inusersysno *= e.sysno
								and st.stocksysnoa = stocka.sysno
								and st.stocksysnob = stockb.sysno";
            if (paramHash != null && paramHash.Count != 0)
            {
                StringBuilder sb = new StringBuilder(100);
                foreach (string key in paramHash.Keys)
                {
                    sb.Append(" and ");
                    object item = paramHash[key];
                    if (key == "DateFrom")
                    {
                        sb.Append("CreateTime >= ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "DateTo")
                    {
                        sb.Append("CreateTime <= ").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "ProductSysNo")
                    {
                        sb.Append(" exists ( select top 1 sysno from st_shift_item where st.sysno=st_shift_item.shiftsysno and productsysno = ").Append(Util.SafeFormat(item.ToString())).Append(" ) ");
                    }
                    else if (key == "Status")
                    {
                        sb.Append("st.Status = ").Append(item.ToString());
                    }
                    else if (item is int)
                    {
                        sb.Append(key).Append("=").Append(item.ToString());
                    }
                    else if (item is string)
                    {
                        sb.Append(key).Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                    }
                }
                sql += sb.ToString();
            }
            else
            {
                sql = sql.Replace("select", "select top 50");
            }

            sql += " order by st.sysno desc";
            return SqlHelper.ExecuteDataSet(sql);
        }

        public ShiftInfo Load(int shiftSysNo)
        {
            ShiftInfo masterInfo;

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                string masterSql = "select * from st_shift where sysno = " + shiftSysNo;
                DataSet masterDs = SqlHelper.ExecuteDataSet(masterSql);
                if (!Util.HasMoreRow(masterDs))
                    throw new BizException("there is no this shift sysno");

                masterInfo = new ShiftInfo();
                map(masterInfo, masterDs.Tables[0].Rows[0]);

                string itemSql = "select * from st_shift_item where shiftsysno=" + shiftSysNo;
                DataSet itemDs = SqlHelper.ExecuteDataSet(itemSql);
                if (Util.HasMoreRow(itemDs))
                {
                    foreach (DataRow dr in itemDs.Tables[0].Rows)
                    {
                        ShiftItemInfo item = new ShiftItemInfo();
                        map(item, dr);
                        masterInfo.itemHash.Add(item.ProductSysNo, item);
                    }
                }
                scope.Complete();
            }

            return masterInfo;
        }

        #region map
        private void map(ShiftInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.ShiftID = Util.TrimNull(tempdr["ShiftID"]);
            oParam.StockSysNoA = Util.TrimIntNull(tempdr["StockSysNoA"]);
            oParam.StockSysNoB = Util.TrimIntNull(tempdr["StockSysNoB"]);
            oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
            oParam.CreateUserSysNo = Util.TrimIntNull(tempdr["CreateUserSysNo"]);
            oParam.AuditTime = Util.TrimDateNull(tempdr["AuditTime"]);
            oParam.AuditUserSysNo = Util.TrimIntNull(tempdr["AuditUserSysNo"]);
            oParam.OutTime = Util.TrimDateNull(tempdr["OutTime"]);
            oParam.OutUserSysNo = Util.TrimIntNull(tempdr["OutUserSysNo"]);
            oParam.InTime = Util.TrimDateNull(tempdr["InTime"]);
            oParam.InUserSysNo = Util.TrimIntNull(tempdr["InUserSysNo"]);
            oParam.CheckQtyUserSysNo = Util.TrimIntNull(tempdr["CheckQtyUserSysNo"]);
            oParam.CheckQtyTime = Util.TrimDateNull(tempdr["CheckQtyTime"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
            oParam.Note = Util.TrimNull(tempdr["Note"]);
            oParam.DLSysNo = Util.TrimIntNull(tempdr["DLSysNo"]);
            oParam.SetDeliveryManTime = Util.TrimDateNull(tempdr["SetDeliveryManTime"]);
            oParam.FreightUserSysNo = Util.TrimIntNull(tempdr["FreightUserSysNo"]);
            oParam.IsLarge = Util.TrimIntNull(tempdr["IsLarge"]);

        }

        private void map(ShiftItemInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.ShiftSysNo = Util.TrimIntNull(tempdr["ShiftSysNo"]);
            oParam.ProductSysNo = Util.TrimIntNull(tempdr["ProductSysNo"]);
            oParam.ShiftQty = Util.TrimIntNull(tempdr["ShiftQty"]);
        }

        private void map(ShiftItemProductIDInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.StShiftItemSysNo = Util.TrimIntNull(tempdr["StShiftItemSysNo"]);
            oParam.ProductIDSysNo = Util.TrimIntNull(tempdr["ProductIDSysNo"]);
        }
        #endregion

        #region  getID 、getCurrentStatus
        private string getShiftID(int sysNo)
        {
            return "57" + sysNo.ToString().PadLeft(8, '0');
        }
        private int getCurrentStatus(int shiftSysNo)
        {
            int status = AppConst.IntNull;

            string sql = "select status from st_shift where sysno =" + shiftSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (Util.HasMoreRow(ds))
                status = Util.TrimIntNull(ds.Tables[0].Rows[0]["status"]);

            return status;
        }
        #endregion

        public void Create(ShiftInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                oParam.SysNo = SequenceDac.GetInstance().Create("St_Shift_Sequence");
                oParam.ShiftID = getShiftID(oParam.SysNo);
                //建立主表记录
                int rowsAffected = new ShiftDac().InsertMaster(oParam);
                if (rowsAffected != 1)
                    throw new BizException("insert shift master error");
                foreach (ShiftItemInfo item in oParam.itemHash.Values)
                {
                    item.ShiftSysNo = oParam.SysNo;

                    rowsAffected = new ShiftDac().InsertItem(item);
                    if (rowsAffected != 1)
                        throw new BizException("insert shift item error");
                    InventoryManager.GetInstance().SetAvailableQty(oParam.StockSysNoA, item.ProductSysNo, item.ShiftQty);
                }

                scope.Complete();
            }
        }

        public void UpdateMaster(ShiftInfo oParam)
        {
            //主项可以更新note
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                //必须是初始状态
                if (getCurrentStatus(oParam.SysNo) != (int)AppEnum.ShiftStatus.Origin)
                    throw new BizException("status is not origin now,  update failed");

                //设置 单号, 备注
                Hashtable ht = new Hashtable(3);
                ht.Add("SysNo", oParam.SysNo);
                ht.Add("Note", oParam.Note);
                if (1 != new ShiftDac().UpdateMaster(ht))
                    throw new BizException("expected one-row update failed, update failed ");

                scope.Complete();
            }
        }

        public void Verify(int masterSysNo, int userSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                //必须是初始状态
                if (getCurrentStatus(masterSysNo) != (int)AppEnum.ShiftStatus.Origin)
                    throw new BizException("status is not origin now ,  verify failed");

                //设置 单号、状态和审核人
                Hashtable ht = new Hashtable(4);

                ht.Add("SysNo", masterSysNo);
                ht.Add("Status", (int)AppEnum.ShiftStatus.Verified);
                ht.Add("AuditTime", DateTime.Now);
                ht.Add("AuditUserSysNo", userSysNo);
                if (1 != new ShiftDac().UpdateMaster(ht))
                    throw new BizException("expected one-row update failed, verify failed ");

                //更新RMA单件移库状态
                Hashtable Registerht = new Hashtable();
                Registerht.Add("ShiftSysNo", masterSysNo);
                DataSet ds = GetRMARegisterByShiftSysNo(Registerht);
                if (Util.HasMoreRow(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string sql = @"update RMA_Register set ShiftStatus=" + (int)AppEnum.ShiftStatus.Verified + "where sysno=" + Util.TrimIntNull(dr["registersysno"]);
                        SqlHelper.ExecuteNonQuery(sql);
                    }

                }

                scope.Complete();
            }
        }

        public void Abandon(int masterSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                ShiftInfo masterInfo = Load(masterSysNo);

                //必须是初始状态
                if (masterInfo.Status != (int)AppEnum.ShiftStatus.Origin)
                    throw new BizException("status is not origin now ,  abandon failed");

                //设置 单号、状态
                Hashtable ht = new Hashtable(4);
                ht.Add("SysNo", masterSysNo);
                ht.Add("Status", (int)AppEnum.ShiftStatus.Abandon);
                if (1 != new ShiftDac().UpdateMaster(ht))
                    throw new BizException("expected one-row update failed, abandon failed ");

                //取消对available数量的占用
                foreach (ShiftItemInfo item in masterInfo.itemHash.Values)
                {
                    InventoryManager.GetInstance().SetAvailableQty(masterInfo.StockSysNoA, item.ProductSysNo, -1 * item.ShiftQty);
                }

                //设置RMA单件的状态
                Hashtable Registerht = new Hashtable();
                Registerht.Add("ShiftSysNo", masterSysNo);
                DataSet ds = GetRMARegisterByShiftSysNo(Registerht);
                if (Util.HasMoreRow(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string sql = @"update RMA_Register set ShiftStatus=" + (int)AppEnum.ShiftStatus.Abandon + "where sysno=" + Util.TrimIntNull(dr["registersysno"]);
                        SqlHelper.ExecuteNonQuery(sql);
                    }

                }
                scope.Complete();
            }
        }

        public void CancelAbandon(int masterSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                ShiftInfo masterInfo = Load(masterSysNo);

                //必须是废弃状态
                if (masterInfo.Status != (int)AppEnum.ShiftStatus.Abandon)
                    throw new BizException("status is not abandon now ,  cancel abandon failed");

                //设置 单号、状态
                Hashtable ht = new Hashtable(4);
                ht.Add("SysNo", masterSysNo);
                ht.Add("Status", (int)AppEnum.ShiftStatus.Origin);
                if (1 != new ShiftDac().UpdateMaster(ht))
                    throw new BizException("expected one-row update failed, cancel abandon failed ");

                //增加对available数量的占用
                foreach (ShiftItemInfo item in masterInfo.itemHash.Values)
                {
                    InventoryManager.GetInstance().SetAvailableQty(masterInfo.StockSysNoA, item.ProductSysNo, item.ShiftQty);
                }
                //设置RMA单件的状态
                Hashtable Registerht = new Hashtable();
                Registerht.Add("ShiftSysNo", masterSysNo);
                DataSet ds = GetRMARegisterByShiftSysNo(Registerht);
                if (Util.HasMoreRow(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string sql = @"update RMA_Register set ShiftStatus=" + (int)AppEnum.ShiftStatus.Origin + "where sysno=" + Util.TrimIntNull(dr["registersysno"]);
                        SqlHelper.ExecuteNonQuery(sql);
                    }

                }
                scope.Complete();
            }
        }

        public void CancelVerify(int masterSysNo, int userSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                ShiftInfo masterInfo = Load(masterSysNo);

                //必须是已审核状态
                if (masterInfo.Status != (int)AppEnum.ShiftStatus.Verified)
                    throw new BizException("status is not verified now,  cancel verify failed");

                //设置 单号、状态
                Hashtable ht = new Hashtable(4);
                ht.Add("SysNo", masterInfo.SysNo);
                ht.Add("Status", (int)AppEnum.ShiftStatus.Origin);
                ht.Add("AuditTime", DateTime.Now);
                ht.Add("AuditUserSysNo", userSysNo);
                if (1 != new ShiftDac().UpdateMaster(ht))
                    throw new BizException("expected one-row update failed, cancel verify failed ");

                //设置RMA单件的状态
                Hashtable Registerht = new Hashtable();
                Registerht.Add("ShiftSysNo", masterSysNo);
                DataSet ds = GetRMARegisterByShiftSysNo(Registerht);
                if (Util.HasMoreRow(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string sql = @"update RMA_Register set ShiftStatus=" + (int)AppEnum.ShiftStatus.Origin + "where sysno=" + Util.TrimIntNull(dr["registersysno"]);
                        SqlHelper.ExecuteNonQuery(sql);
                    }

                }
                scope.Complete();
            }
        }

        public void OutStock(int masterSysNo, int userSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                ShiftInfo masterInfo = Load(masterSysNo);

                //必须是已审核
                if (masterInfo.Status != (int)AppEnum.ShiftStatus.Verified)
                    throw new BizException("status is not verify now,  outstock failed");

                //设置 单号、状态
                Hashtable ht = new Hashtable(4);
                ht.Add("SysNo", masterInfo.SysNo);
                ht.Add("Status", (int)AppEnum.ShiftStatus.OutStock);
                ht.Add("OutTime", DateTime.Now);
                ht.Add("OutUserSysNo", userSysNo);
                if (1 != new ShiftDac().UpdateMaster(ht))
                    throw new BizException("expected one-row update failed, outstock failed ");

                foreach (ShiftItemInfo item in masterInfo.itemHash.Values)
                {
                    InventoryManager.GetInstance().SetShiftOutStockQty(masterInfo.StockSysNoA, masterInfo.StockSysNoB, item.ProductSysNo, item.ShiftQty);
                }

                //设置RMA单件的状态
                Hashtable Registerht = new Hashtable();
                Registerht.Add("ShiftSysNo", masterInfo.SysNo);
                DataSet ds = GetRMARegisterByShiftSysNo(Registerht);
                if (Util.HasMoreRow(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string sql = @"update RMA_Register set ShiftStatus=" + (int)AppEnum.ShiftStatus.OutStock + "where sysno=" + Util.TrimIntNull(dr["registersysno"]);
                        SqlHelper.ExecuteNonQuery(sql);
                    }

                }

                scope.Complete();
            }
        }

        public void CancelOutStock(int masterSysNo, int userSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                ShiftInfo masterInfo = Load(masterSysNo);

                //必须是已出库
                if (masterInfo.Status != (int)AppEnum.ShiftStatus.OutStock)
                    throw new BizException("status is not outstock now,  cancel outstock failed");

                //设置 单号、状态
                Hashtable ht = new Hashtable(4);
                ht.Add("SysNo", masterInfo.SysNo);
                ht.Add("Status", (int)AppEnum.ShiftStatus.Origin);
                ht.Add("OutTime", DateTime.Now);
                ht.Add("OutUserSysNo", userSysNo);
                if (1 != new ShiftDac().UpdateMaster(ht))
                    throw new BizException("expected one-row update failed, cancel outstock failed ");

                foreach (ShiftItemInfo item in masterInfo.itemHash.Values)
                {
                    //库存设定
                    InventoryManager.GetInstance().SetShiftOutStockQty(masterInfo.StockSysNoA, masterInfo.StockSysNoB, item.ProductSysNo, -1 * item.ShiftQty);
                }

                //设置RMA单件的状态
                Hashtable Registerht = new Hashtable();
                Registerht.Add("ShiftSysNo", masterSysNo);
                DataSet ds = GetRMARegisterByShiftSysNo(Registerht);
                if (Util.HasMoreRow(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string sql = @"update RMA_Register set ShiftStatus=" + (int)AppEnum.ShiftStatus.Origin + "where sysno=" + Util.TrimIntNull(dr["registersysno"]);
                        SqlHelper.ExecuteNonQuery(sql);
                    }

                }

                scope.Complete();
            }
        }

        public void InStock(int masterSysNo, int userSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                ShiftInfo masterInfo = Load(masterSysNo);

                //必须是已出库
                if (masterInfo.Status != (int)AppEnum.ShiftStatus.OutStock)
                    throw new BizException("status is not outstock now,  instock failed");

                //设置 单号、状态
                Hashtable ht = new Hashtable(4);
                ht.Add("SysNo", masterInfo.SysNo);
                ht.Add("Status", (int)AppEnum.ShiftStatus.InStock);
                ht.Add("InTime", DateTime.Now);
                ht.Add("InUserSysNo", userSysNo);
                if (1 != new ShiftDac().UpdateMaster(ht))
                    throw new BizException("expected one-row update failed, instock failed ");

                foreach (ShiftItemInfo item in masterInfo.itemHash.Values)
                {
                    //库存设定
                    InventoryManager.GetInstance().SetShiftInStockQty(masterInfo.StockSysNoA, masterInfo.StockSysNoB, item.ProductSysNo, item.ShiftQty);
                }

                //设置RMA单件的状态
                Hashtable Registerht = new Hashtable();
                Registerht.Add("ShiftSysNo", masterSysNo);
                DataSet ds = GetRMARegisterByShiftSysNo(Registerht);
                if (Util.HasMoreRow(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string sql = @"update RMA_Register set ShiftStatus=" + (int)AppEnum.ShiftStatus.InStock + "where sysno=" + Util.TrimIntNull(dr["registersysno"]);
                        SqlHelper.ExecuteNonQuery(sql);
                    }

                }

                scope.Complete();
            }
        }

        /// <summary>
        /// 移库单中如果有二手商品将会邮件通知相关的PM
        /// </summary>
        /// <param name="masterSysNo">移库单号</param>
        public void InstockSecondHandProductEmailNotify(int shiftSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                string sql = @"select ssi.shiftsysno, p.sysno as productsysno,p.productid,p.productname,ssi.shiftqty,su.username,su.email 
                        from st_shift_item ssi(nolock) inner join product p(nolock) on ssi.productsysno=p.sysno 
                        inner join sys_user su(nolock) on p.pmusersysno=su.sysno 
                        where p.producttype=@secondhandtype and ssi.shiftsysno=@shiftsysno 
                        order by su.sysno";
                sql = sql.Replace("@secondhandtype", ((int)AppEnum.ProductType.SecondHand).ToString());
                sql = sql.Replace("@shiftsysno", shiftSysNo.ToString());
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (Util.HasMoreRow(ds))
                {
                    string mailAddress = "";
                    string mailSubject = "";
                    string mailBody = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        mailAddress = Util.TrimNull(dr["email"]);
                        mailSubject = "二手商品移库单入库通知，移库单号：" + shiftSysNo + "，商品号：" + Util.TrimNull(dr["productid"]);
                        mailBody = "<html><body>商品号：" + Util.TrimNull(dr["productid"]) + "<br>商品名称：" + Util.TrimNull(dr["productname"]) + "<br>数量：" + Util.TrimNull(dr["shiftqty"]) + "</body></html>";
                        EmailInfo oEmail = new EmailInfo(mailAddress, mailSubject, mailBody);
                        EmailManager.GetInstance().InsertEmail(oEmail);
                    }
                }
                scope.Complete();
            }
        }

        public void CancelInStock(int masterSysNo, int userSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                ShiftInfo masterInfo = Load(masterSysNo);

                //必须是已出库
                if (masterInfo.Status != (int)AppEnum.ShiftStatus.InStock)
                    throw new BizException("status is not instock now,  cancel instock failed");

                //RMA发货单已发货，则不能取消入库
                Hashtable Registerht = new Hashtable();
                Registerht.Add("ShiftSysNo", masterInfo.SysNo);
                Registerht.Add("RevertStatus", (int)AppEnum.RMARevertStatus.Reverted);
                DataSet ds = GetRMARegisterByShiftSysNo(Registerht);
                if (Util.HasMoreRow(ds))
                {
                    throw new BizException("存在已出库的发货单，不能取消取出库 ");
                }
                else
                {
                    Hashtable Registerht2 = new Hashtable();
                    Registerht2.Add("ShiftSysNo", masterSysNo);
                    DataSet ds2 = GetRMARegisterByShiftSysNo(Registerht2);
                    if (Util.HasMoreRow(ds2))
                    {
                        foreach (DataRow dr in ds2.Tables[0].Rows)
                        {
                            string sql = @"update RMA_Register set ShiftStatus=" + (int)AppEnum.ShiftStatus.OutStock + "where sysno=" + Util.TrimIntNull(dr["registersysno"]);
                            SqlHelper.ExecuteNonQuery(sql);
                        }

                    }

                }

                //设置 单号、状态
                Hashtable ht = new Hashtable(4);
                ht.Add("SysNo", masterInfo.SysNo);
                ht.Add("Status", (int)AppEnum.ShiftStatus.OutStock);
                ht.Add("InTime", DateTime.Now);
                ht.Add("InUserSysNo", userSysNo);
                if (1 != new ShiftDac().UpdateMaster(ht))
                    throw new BizException("expected one-row update failed, instock failed ");

                foreach (ShiftItemInfo item in masterInfo.itemHash.Values)
                {
                    //库存设定
                    InventoryManager.GetInstance().SetShiftInStockQty(masterInfo.StockSysNoA, masterInfo.StockSysNoB, item.ProductSysNo, -1 * item.ShiftQty);
                }
                scope.Complete();
            }
        }

        public void UpdateItemQty(ShiftInfo masterInfo, ShiftItemInfo itemInfo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                //必须是初始
                if (getCurrentStatus(masterInfo.SysNo) != (int)AppEnum.ShiftStatus.Origin)
                    throw new BizException("status is not origin now,  update item qty failed");

                //获取数量差值
                ShiftItemInfo oldItemInfo = masterInfo.itemHash[itemInfo.ProductSysNo] as ShiftItemInfo;
                int deltQty = itemInfo.ShiftQty - oldItemInfo.ShiftQty;

                //更新表单明细 ( 如果增加表单明细项的属性，需要在这里处理一下）
                itemInfo.SysNo = oldItemInfo.SysNo;
                itemInfo.ShiftSysNo = oldItemInfo.ShiftSysNo;

                if (1 != new ShiftDac().UpdateItemQty(itemInfo.SysNo, deltQty))
                    throw new BizException("expected one-row update failed, update item qty failed");


                //更新库存
                InventoryManager.GetInstance().SetAvailableQty(masterInfo.StockSysNoA, itemInfo.ProductSysNo, deltQty);

                //更新 itemInfo 到 masterInfo 注:数据库更新成功以后才更新类
                masterInfo.itemHash.Remove(itemInfo.ProductSysNo);
                masterInfo.itemHash.Add(itemInfo.ProductSysNo, itemInfo);

                scope.Complete();
            }
        }

        public void InsertItem(ShiftInfo masterInfo, ShiftItemInfo itemInfo)
        {
            if (masterInfo.itemHash.ContainsKey(itemInfo.ProductSysNo))
            {
                throw new BizException("item duplicated!");
            }

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                //必须是初始
                if (getCurrentStatus(masterInfo.SysNo) != (int)AppEnum.ShiftStatus.Origin)
                    throw new BizException("status is not origin now,  add item failed");

                //获取数量差值

                //更新item
                if (1 != new ShiftDac().InsertItem(itemInfo))
                    throw new BizException("expected one-row update failed, add item failed");

                //更新库存
                InventoryManager.GetInstance().SetAvailableQty(masterInfo.StockSysNoA, itemInfo.ProductSysNo, itemInfo.ShiftQty);

                //更新 itemInfo 到 masterInfo
                masterInfo.itemHash.Add(itemInfo.ProductSysNo, itemInfo);

                scope.Complete();
            }
        }
        public void DeleteItem(ShiftInfo masterInfo, int itemProductSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                //必须是初始
                if (getCurrentStatus(masterInfo.SysNo) != (int)AppEnum.ShiftStatus.Origin)
                    throw new BizException("status is not origin now,  delete item failed");

                //获取数量差值
                ShiftItemInfo oldItemInfo = masterInfo.itemHash[itemProductSysNo] as ShiftItemInfo;
                int deltQty = -1 * oldItemInfo.ShiftQty;

                //更新item
                if (1 != new ShiftDac().DeleteItem(oldItemInfo.SysNo))
                    throw new BizException("expected one-row update failed, delete item qty failed");

                //更新库存
                InventoryManager.GetInstance().SetAvailableQty(masterInfo.StockSysNoA, itemProductSysNo, deltQty);

                //更新 masterInfo
                masterInfo.itemHash.Remove(itemProductSysNo);

                //更新RMA移库信息信息
                Hashtable ht = new Hashtable();
                ht.Add("ShiftSysNo", masterInfo.SysNo);
                ht.Add("RevertProductSysNo", itemProductSysNo);
                DataSet ds = GetRMARegisterByShiftSysNo(ht);
                if (Util.HasMoreRow(ds))
                {
                    RMARegisterManager.GetInstance().DeleteRegisterShift(ds);
                }
                scope.Complete();
            }
        }

        public ShiftItemProductIDInfo LoadShiftItemProductID(int StShiftItemSysNo, int ProductIDSysNo)
        {
            string sql = "select * from St_Shift_Item_ProductID where stshiftitemsysno= " + StShiftItemSysNo + " and productidsysno=" + ProductIDSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            ShiftItemProductIDInfo oInfo = new ShiftItemProductIDInfo();
            if (Util.HasMoreRow(ds))
                map(oInfo, ds.Tables[0].Rows[0]);
            else
                oInfo = null;
            return oInfo;
        }

        public int InsertShiftItemProductID(ShiftItemProductIDInfo oParam)
        {
            return new ShiftDac().InsertShiftItemProductID(oParam);
        }

        public void InsertShiftItemProductIDs(string[] ItemProductIDs)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                string ItemProductIDList = "";
                foreach (string ItemProductID in ItemProductIDs)
                {
                    int indexComma = ItemProductID.IndexOf(",");
                    ItemProductIDList += Int32.Parse(ItemProductID.Substring(0, indexComma)) + ",";
                }

                if (ItemProductIDList.Length > 0)
                {
                    ItemProductIDList = ItemProductIDList.Substring(0, ItemProductIDList.Length - 1);
                    string sql = "delete from st_shift_item_productid where stshiftitemsysno in(" + ItemProductIDList + ")";
                    SqlHelper.ExecuteNonQuery(sql);
                }

                foreach (string ItemProductID in ItemProductIDs)
                {
                    int indexComma = ItemProductID.IndexOf(",");
                    int lastIndexComma = ItemProductID.LastIndexOf(",");
                    ShiftItemProductIDInfo oInfo = new ShiftItemProductIDInfo();
                    oInfo.StShiftItemSysNo = Int32.Parse(ItemProductID.Substring(0, indexComma));
                    oInfo.ProductIDSysNo = Int32.Parse(ItemProductID.Substring(lastIndexComma + 1));

                    if (LoadShiftItemProductID(oInfo.StShiftItemSysNo, oInfo.ProductIDSysNo) == null)
                    {
                        this.InsertShiftItemProductID(oInfo);
                    }
                }
                scope.Complete();
            }
        }

        public void Import()
        {
            /* 涉及的问题 
             * 1 还货记录的处理
             * 2 库存的处理
             * 3 状态的处理
             * 4 还有单据id的对应，这个要特别注意
             */
            if (!AppConfig.IsImportable)
                throw new BizException("Is Importable is false");

            string sql = " select top 1 sysno from st_shift";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                throw new BizException("the table shift is not empty");

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                string sql_old = @"select 
								old.sysno, old.shiftid, stocka_con.newsysno as stocksysnoa, stockb_con.newsysno as stocksysnob,
								create_con.newsysno as createusersysno,
								audit_con.newsysno as auditusersysno,
								out_con.newsysno as outusersysno,
								in_con.newsysno as inusersysno,
								createtime, audittime,outstocktime as outtime,instocktime as intime,
								auditstatus, productstatus, note, '1' as status
							from 
								ipp2003..st_shift as old, 
								ippconvert..sys_user as create_con,
								ippconvert..sys_user as audit_con,
								ippconvert..sys_user as out_con,
								ippconvert..sys_user as in_con,
								ippconvert..stock as stocka_con,
								ippconvert..Stock as stockb_con
							where 
								old.createusersysno *= create_con.oldsysno and
								old.auditusersysno *= audit_con.oldsysno and
								old.outstockusersysno *= out_con.oldsysno and
								old.instockusersysno *= in_con.oldsysno and
								old.stocksysnoa = stocka_con.oldsysno and
								old.stocksysnob = stockb_con.oldsysno
								order by old.sysno";
                DataSet ds_old = SqlHelper.ExecuteDataSet(sql_old);
                if (!Util.HasMoreRow(ds_old))
                    return;
                foreach (DataRow dr in ds_old.Tables[0].Rows)
                {
                    /* newStatus	aduit	product
                     * abandon		-1		n/a
                     * origin		0		n/a
                     * verified		1		n/a
                     * 
                     * StillInA		n/a		0
                     * OnTheRoad	n/a		1
                     * AlreadyInB	n/a		2
                     */
                    int newStatus = (int)AppEnum.ShiftStatus.Origin;
                    int auditStatus = Util.TrimIntNull(dr["auditStatus"]);
                    int productStatus = Util.TrimIntNull(dr["productStatus"]);
                    if (auditStatus == -1)
                        newStatus = (int)AppEnum.ShiftStatus.Abandon;
                    if (auditStatus == 0)
                        newStatus = (int)AppEnum.ShiftStatus.Origin;
                    if (auditStatus == 0)
                        newStatus = (int)AppEnum.ShiftStatus.Verified;
                    if (productStatus == 1)
                        newStatus = (int)AppEnum.ShiftStatus.OutStock;
                    if (productStatus == 2)
                        newStatus = (int)AppEnum.ShiftStatus.InStock;


                    ShiftInfo oInfo = new ShiftInfo();
                    map(oInfo, dr);
                    oInfo.Status = newStatus;

                    if (new ShiftDac().InsertMaster(oInfo) != 1)
                    {
                        throw new BizException("master expected one row error");
                    }

                    //insert item
                    string sql_item = @"select '0' as sysno,
										ShiftSysNo, con_product.newsysno as productsysno, shiftqty
									from 
										ipp2003..St_Shift_Item si, ippconvert..productbasic as con_product
									where si.productsysno = con_product.oldsysno and ShiftSysNo=" + oInfo.SysNo;

                    DataSet ds_item = SqlHelper.ExecuteDataSet(sql_item);
                    if (!Util.HasMoreRow(ds_item))
                        continue;
                    foreach (DataRow drItem in ds_item.Tables[0].Rows)
                    {
                        ShiftItemInfo oItem = new ShiftItemInfo();
                        map(oItem, drItem);

                        int resultitem = new ShiftDac().InsertItem(oItem);
                        if (resultitem < 1)
                            throw new BizException("insert item row < 1");

                        //调整库存
                        if (oInfo.Status == (int)AppEnum.ShiftStatus.Origin || oInfo.Status == (int)AppEnum.ShiftStatus.Verified)
                        {
                            InventoryManager.GetInstance().SetAvailableQty(oInfo.StockSysNoA, oItem.ProductSysNo, oItem.ShiftQty);
                        }
                    }
                }

                string sqlMaxSysNo = "select top 1 sysno from ipp2003..st_shift order by sysno desc";
                DataSet dsMax = SqlHelper.ExecuteDataSet(sqlMaxSysNo);
                if (!Util.HasMoreRow(dsMax))
                    throw new BizException("got max sysno error");
                int maxSysNo = Util.TrimIntNull(dsMax.Tables[0].Rows[0]["sysno"]);
                // 将自动生成的sysno填到目前的最大单据号
                int newSysNo;
                do
                {
                    newSysNo = SequenceDac.GetInstance().Create("St_Shift_Sequence");
                } while (newSysNo < maxSysNo);


                scope.Complete();
            }
        }

        public int GetShiftSysNofromID(string shiftID)
        {
            string sql = "select sysno from St_Shift where shiftID = " + Util.ToSqlString(shiftID);
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                return (int)ds.Tables[0].Rows[0][0];
            else
                return AppConst.IntNull;
        }

        public DataSet GetRAMWaitingShiftList()
        {
            string sql = @"select product.sysno,product.productid,product.productname,count(RevertProductSysNo) as ShiftQty ,RMA_Register.sysno as registersysno from RMA_Register 
                         left join Product on RMA_Register.RevertProductSysNo=product.sysno
                         Group by RMA_Register.RevertProductSysNo,product.sysno,product.productid,product.productname,RMA_Register.shiftstatus,RMA_Register.sysno,RMA_Register.revertstatus
                         having 1=1 and RMA_Register.shiftstatus=@registershiftstatus and @Revertstatus";

            sql = sql.Replace("@registershiftstatus", ((int)AppEnum.ShiftStatus.RMAWaitingShift).ToString());
            sql = sql.Replace("@Revertstatus", "(RMA_Register.revertstatus=" + ((int)AppEnum.RMARevertStatus.WaitingAudit) + "or RMA_Register.revertstatus=" + ((int)AppEnum.RMARevertStatus.WaitingRevert) + ")");

            return SqlHelper.ExecuteDataSet(sql);
        }
        public DataSet GetRAMShiftList(string RegisterSysNo)
        {
            string sql = @"select product.sysno as productsysno,count(RevertProductSysNo) as ShiftQty  from RMA_Register 
                         left join Product on RMA_Register.RevertProductSysNo=product.sysno
                         left join RMA_Shift on RMA_Shift.registersysno=RMA_Register.sysno
                         where RMA_Register.sysno in (" + RegisterSysNo + ")" + " Group by product.sysno ";

            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetRMARegisterByShiftSysNo(Hashtable paramHash)
        {
            string sql = @"select RMA_Register.sysno as registersysno,RMA_Shift.sysno as RMAShiftSysNo from RMA_Register 
                             left join RMA_Shift on RMA_Shift.RegisterSysNo=RMA_Register.sysno
                             where 1=1 @ShiftSysNo @RevertProductSysNo @RevertStatus";

            if (paramHash.ContainsKey("ShiftSysNo"))
                sql = sql.Replace("@ShiftSysNo", " and RMA_Shift.ShiftSysNo=" + paramHash["ShiftSysNo"].ToString());
            else
                sql = sql.Replace("@ShiftSysNo", "");

            if (paramHash.ContainsKey("RevertProductSysNo"))
                sql = sql.Replace("@RevertProductSysNo", " and RMA_Register.RevertProductSysNo=" + paramHash["RevertProductSysNo"].ToString());
            else
                sql = sql.Replace("@RevertProductSysNo", "");

            if (paramHash.ContainsKey("RevertStatus"))
                sql = sql.Replace("@RevertStatus", " and RMA_Register.RevertStatus=" + paramHash["RevertStatus"].ToString());
            else
                sql = sql.Replace("@RevertStatus", "");


            return SqlHelper.ExecuteDataSet(sql);
        }

        public bool IsFromRMA(int ShiftSysNo)
        {
            string sql = @"select * from RMA_Shift where ShiftSysNo=" + ShiftSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (Util.HasMoreRow(ds))
                return true;
            else
                return false;
        }

        public int UpdateShiftMaster(Hashtable ht)
        {
            return UtilDac.GetInstance().Update(ht, "St_Shift");
        }

        public int GetFreightUserSysNoByID(string ShiftID)
        {
            string sql = @"select FreightUserSysNo
                           from  St_Shift (nolock)
                           where St_Shift.ShiftID='" + ShiftID + "'";

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                return (int)ds.Tables[0].Rows[0][0];
            else
                return AppConst.IntNull;
        }

    }
}