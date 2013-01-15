using System;
using System.Collections.Generic;
using System.Collections;
using System.Transactions;
using System.Text;
using System.Data;

using Icson.DBAccess;
using Icson.DBAccess.Sale;
using Icson.Objects.Sale;
using Icson.Objects;
using Icson.BLL.Basic;
using Icson.BLL.Sale;
using Icson.Utils;

namespace Icson.BLL.Sale
{
    public class SRManager
    {
        private SRManager()
        {
        }
        private static SRManager _instance;
        public static SRManager GetInstance()
        {
            if (_instance == null)
                _instance = new SRManager();
            return _instance;
        }



        private void map(SRInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.SRID = Util.TrimNull(tempdr["SRID"]);
            oParam.SOSysNo = Util.TrimIntNull(tempdr["SOSysNo"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
            oParam.ReturnType = Util.TrimIntNull(tempdr["ReturnType"]);
            oParam.StockSysNo = Util.TrimIntNull(tempdr["StockSysNo"]);
            oParam.CreateUserSysNo = Util.TrimIntNull(tempdr["CreateUserSysNo"]);
            oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
            oParam.ReceiveUserSysNo = Util.TrimIntNull(tempdr["ReceiveUserSysNo"]);
            oParam.ReceiveTime = Util.TrimDateNull(tempdr["ReceiveTime"]);
            oParam.InstockTime = Util.TrimDateNull(tempdr["InstockTime"]);
            oParam.InstockUserSysNo = Util.TrimIntNull(tempdr["InstockUserSysNo"]);
            oParam.ShelveTime = Util.TrimDateNull(tempdr["ShelveTime"]);
            oParam.ShelveUserSysNo = Util.TrimIntNull(tempdr["ShelveUserSysNo"]);
            oParam.UpdateUserSysNo = Util.TrimIntNull(tempdr["UpdateUserSysNo"]);
            oParam.UpdateTime = Util.TrimDateNull(tempdr["UpdateTime"]);
            oParam.Note = Util.TrimNull(tempdr["Note"]);
        }
        private void map(SRItemInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.SRSysNo = Util.TrimIntNull(tempdr["SRSysNo"]);
            oParam.ProductSysNo = Util.TrimIntNull(tempdr["ProductSysNo"]);
            oParam.Quantity = Util.TrimIntNull(tempdr["Quantity"]);
        }

        private string BuildSRID(int sysNo)
        {
            string sysNoStr = sysNo.ToString();
            int idLen = 10;
            string srid = "R6";
            for (int i = 0; i < (idLen - sysNoStr.Length - 2); i++)
            {
                srid += "0";
            }
            srid += sysNoStr;
            return srid;
        }
        private int InsertSRMaster(SRInfo oParam)
        {
            oParam.SRID = this.BuildSRID(oParam.SysNo);
            return new SRDac().InsertMaster(oParam);
        }
        private void InsertSRItem(SRItemInfo oParam, SRInfo srInfo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                new SRDac().InsertItem(oParam);
                scope.Complete();
            }
        }

        public SRInfo Load(int sysno)
        {
            try
            {
                SRInfo oInfo = new SRInfo();
                string sql = "select * from SR_Master (NOLOCK) where sysno=" + sysno;
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (Util.HasMoreRow(ds))
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    map(oInfo, dr);
                    string itemSql = @"select SR_Item.* 
                                       from  SR_Master (NOLOCK)
                                       inner join  SR_Item (NOLOCK) on SR_Item.SRSysNo = SR_Master.SysNo 
                                       where SR_Master.sysno=" + sysno;
                    DataSet itemds = SqlHelper.ExecuteDataSet(itemSql);
                    if (Util.HasMoreRow(itemds))
                    {
                        foreach (DataRow itemdr in itemds.Tables[0].Rows)
                        {
                            SRItemInfo oSRItem = new SRItemInfo();
                            map(oSRItem, itemdr);
                            oInfo.ItemHash.Add(Util.TrimIntNull(itemdr["SysNo"]), oSRItem);
                        }
                    }
                }
                return oInfo;
            }
            catch
            {
                throw new BizException("Load SRInfo Error!");
            }

        }

        public DataSet GetSRItemList(int sysno)
        {
            string sql = @"select SR_Item.* ,Product.ProductName ,Product.ProductID,Product.SysNo as ProductSysNo
                            from SR_Master (NOLOCK) 
                            inner join SR_Item (NOLOCK) on SR_Master.SysNo = SR_Item.SRSysNo
							inner join Product (NOLOCK) on Product.sysno = SR_Item.ProductSysNo 
							where SR_Master.SysNo = " + sysno;
            return SqlHelper.ExecuteDataSet(sql);
        }
        public void CreateSR(SRInfo srInfo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                //加入退货单主项
                srInfo.SysNo = SequenceDac.GetInstance().Create("SR_Sequence");
                this.InsertSRMaster(srInfo);

                //加入退货单商品明细
                int itemCount = 0;
                foreach (SRItemInfo item in srInfo.ItemHash.Values)
                {
                    if (item.Quantity > 0)  //排除数量为零的情况
                    {
                        item.SRSysNo = srInfo.SysNo;
                        this.InsertSRItem(item, srInfo);

                        itemCount++;
                    }
                }
                if (itemCount == 0)  //无退货商品
                {
                    throw new BizException("您选择的退货商品数量为0！");
                }
                scope.Complete();
            }
        }

        public DataSet GetSRDs(Hashtable paramHash)
        {
            string sql = @"select distinct SR_Master.*,SO_Master.CustomerSysNo,Customer.CustomerID,Customer.CustomerName, su1.username as CreateUserName, su2.username as ReceiveUserName,
                        su4.userName as InstockUserName,su5.userName as ShelveUserName,SO_Master.SOID
                       from SR_Master 
                       left join SO_Master(NOLOCK) on SO_Master.sysno=SR_Master.sosysno
                       left join sys_user su1 on su1.sysno=SR_Master.CreateUserSysNo
                       left join sys_user su2 on su2.sysno=SR_Master.ReceiveUserSysNo
                       left join sys_user su4 on su4.sysno=SR_Master.InstockUserSysNo
                       left join sys_user su5 on su5.sysno=SR_Master.ShelveUserSysNo
                       left join Customer on Customer.sysno=SO_Master.customersysno
                       left join  SR_Item on SR_Item.srsysno=SR_Master.sysno
                       where 1=1
                        ";

            if (paramHash != null && paramHash.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string key in paramHash.Keys)
                {
                    object item = paramHash[key];
                    sb.Append(" and ");
                    if (key == "CreateTimeFrom")
                    {
                        sb.Append("SR_Master.CreateTime").Append(" >= ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "CreateTimeTo")
                    {
                        sb.Append("SR_Master.CreateTime").Append(" <= ").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "SOID")
                    {
                        sb.Append("SO_Master.soid").Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                    }
                    else if (key == "SRID")
                    {
                        sb.Append("SR_Master.SRID").Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                    }
                    else if (key == "CustomerID")
                    {
                        sb.Append("Customer.CustomerID").Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                    }
                    else if (key == "ProductSysNo")
                    {
                        sb.Append("SR_Item.productSysNo").Append(" = ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "SRStatus")
                    {
                        sb.Append("Sr_Master.Status").Append(" = ").Append(Util.ToSqlString(item.ToString()));
                    }

                }
                sql += sb.ToString();
            }
            else
            {
                sql = sql.Replace("select distinct ", "select distinct top 50 ");
            }

            sql += "order by SR_Master.sysno desc";

            return SqlHelper.ExecuteDataSet(sql);

        }

        public void UpdateSRMaster(SRInfo oParam)
        {
            new SRDac().UpdateMaster(oParam);
        }

        public void UpdateSRItem(SRItemInfo oParam)
        {
            new SRDac().UpdateItem(oParam);
        }
        public void AbandonSR(SRInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                //更新退货单状态
                oParam.Status = (int)AppEnum.SRStatus.Abandon;
                this.UpdateSRMaster(oParam);

                scope.Complete();
            }
        }
        public void CancelAbandonSR(SRInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                //更新退货单状态
                oParam.Status = (int)AppEnum.SRStatus.Origin;
                this.UpdateSRMaster(oParam);

                scope.Complete();
            }
        }
        public void ReceiveSR(SRInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                //更新订单状态
                if (oParam.ReturnType == (int)AppEnum.SRReturnType.PartlyReturn)
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("SysNo", oParam.SOSysNo);
                    ht.Add("Status", (int)AppEnum.SOStatus.PartlyReturn);
                    SaleManager.GetInstance().UpdateSOMaster(ht);
                }
                else if (oParam.ReturnType == (int)AppEnum.SRReturnType.Return)
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("SysNo", oParam.SOSysNo);
                    ht.Add("Status", (int)AppEnum.SOStatus.Return);
                    SaleManager.GetInstance().UpdateSOMaster(ht);
                }
                SOInfo soInfo = SaleManager.GetInstance().LoadSO(oParam.SOSysNo);

                //更新SOItemReturnQty
                foreach (SRItemInfo sritem in oParam.ItemHash.Values)
                {
                    foreach (SOItemInfo item in soInfo.ItemHash.Values)
                    {
                        if (item.ProductSysNo == sritem.ProductSysNo)
                        {
                            SaleManager.GetInstance().UpdateSOItemReturnQty(oParam.SOSysNo, item.ProductSysNo, sritem.Quantity);
                        }
                    }
                }

                //更新退货单
                this.UpdateSRMaster(oParam);

                scope.Complete();
            }
        }

        public void CancelReceiveSR(SRInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                //更新订单状态
                Hashtable ht = new Hashtable();
                ht.Add("SysNo", oParam.SOSysNo);
                ht.Add("Status", (int)AppEnum.SOStatus.OutStock);
                SaleManager.GetInstance().UpdateSOMaster(ht);

                SOInfo soInfo = SaleManager.GetInstance().LoadSO(oParam.SOSysNo);

                //更新SOItemReturnQty
                foreach (SRItemInfo sritem in oParam.ItemHash.Values)
                {
                    foreach (SOItemInfo item in soInfo.ItemHash.Values)
                    {
                        if (item.ProductSysNo == sritem.ProductSysNo)
                        {
                            SaleManager.GetInstance().UpdateSOItemReturnQty(oParam.SOSysNo, item.ProductSysNo, 0);
                        }
                    }
                }

                //更新退货单
                this.UpdateSRMaster(oParam);
                scope.Complete();
            }
        }

        public void InStockSR(SRInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                //更新退货单
                this.UpdateSRMaster(oParam);

                //更新商品财务库存信息
                foreach (SRItemInfo item in oParam.ItemHash.Values)
                {
                    InventoryManager.GetInstance().SetInStockQty2(oParam.StockSysNo, item.ProductSysNo, item.Quantity);
                }

                SOInfo soInfo = SaleManager.GetInstance().LoadSO(oParam.SOSysNo);

                //返还客户支付积分
                if (soInfo.Status == (int)AppEnum.SOStatus.Return)//全部退货返还,部分退货不返还积分
                {
                    PointManager.GetInstance().SetScore(soInfo.CustomerSysNo, soInfo.PointPay, (int)AppEnum.PointLogType.ReturnProduct, soInfo.SysNo.ToString());
                }

                //修改订单赠送的积分
                int soPointAmt = 0;
                int PointAmt = 0;

                if (soInfo.Status == (int)AppEnum.SOStatus.PartlyReturn)
                {
                    foreach (SOItemInfo oItem in soInfo.ItemHash.Values)
                    {
                        if (oItem.ReturnQty > 0 && oItem.Point > 0)
                        {
                            PointAmt += oItem.ReturnQty * oItem.Point;
                        }
                    }
                    soPointAmt = soInfo.PointAmt - PointAmt;
                }
                else if (soInfo.Status == (int)AppEnum.SOStatus.Return)
                {
                    soPointAmt = 0;
                }

                Hashtable ht = new Hashtable();
                ht.Add("SysNo", soInfo.SysNo);
                ht.Add("PointAmt", soPointAmt);
                SaleManager.GetInstance().UpdateSOMaster(ht);

                //扣除赠送积分
                SalePointDelayInfo spInfo = PointManager.GetInstance().LoadValid(soInfo.SysNo);
                if (spInfo != null)
                {
                    if (spInfo.Status == (int)AppEnum.TriStatus.Handled)//积分已经加入客户账户，扣除对应积分
                    {
                        PointManager.GetInstance().SetScore(soInfo.CustomerSysNo, soPointAmt * (-1), (int)AppEnum.PointLogType.ReturnProduct, soInfo.SysNo.ToString());
                    }
                    //更新积分赠送记录状态
                    spInfo.Status = (int)AppEnum.TriStatus.Abandon;
                    PointManager.GetInstance().UpdatePointDelay(spInfo);
                }

                scope.Complete();
            }
        }

        public void CancelInStockSR(SRInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                //更新退货单
                this.UpdateSRMaster(oParam);

                //更新商品财务库存信息
                foreach (SRItemInfo item in oParam.ItemHash.Values)
                {
                    InventoryManager.GetInstance().SetInStockQty2(oParam.StockSysNo, item.ProductSysNo, -1 * item.Quantity);
                }

                SOInfo soInfo = SaleManager.GetInstance().LoadSO(oParam.SOSysNo);

                //扣除客户支付积分
                if (soInfo.Status == (int)AppEnum.SOStatus.Return)//全部退货扣除积分,部分退货不扣除积分
                {
                    PointManager.GetInstance().SetScore(soInfo.CustomerSysNo, soInfo.PointPay * (-1), (int)AppEnum.PointLogType.CancelReturn, soInfo.SysNo.ToString());
                }

                //如果有积分赠送则添加pointDelay,并修改订单赠送积分
                int PointAmt = 0;
                foreach (SOItemInfo oItem in soInfo.ItemHash.Values)
                {
                    if (oItem.ReturnQty > 0 && oItem.Point > 0)
                    {
                        PointAmt += oItem.ReturnQty * oItem.Point;
                    }
                }
                if (PointAmt > 0)
                {
                    int soPointAmt = soInfo.PointAmt + PointAmt;
                    Hashtable ht = new Hashtable();
                    ht.Add("SysNo", soInfo.SysNo);
                    ht.Add("PointAmt", soPointAmt);
                    SaleManager.GetInstance().UpdateSOMaster(ht);
                }

                SalePointDelayInfo spInfo = new SalePointDelayInfo();
                spInfo.SOSysNo = soInfo.SysNo;
                spInfo.CreateTime = DateTime.Now;
                spInfo.Status = (int)AppEnum.TriStatus.Origin;
                PointManager.GetInstance().InsertPointDelay(spInfo);


                scope.Complete();
            }
        }

        public void ShelveSR(SRInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                //更新退货单
                this.UpdateSRMaster(oParam);

                //更新商品可用库存信息
                foreach (SRItemInfo item in oParam.ItemHash.Values)
                {
                    InventoryManager.GetInstance().SetAvailableQty2(oParam.StockSysNo, item.ProductSysNo, item.Quantity);
                }

                scope.Complete();
            }
        }

        public void CancelShelveSR(SRInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                //更新退货单
                this.UpdateSRMaster(oParam);

                //更新商品可用库存信息
                foreach (SRItemInfo item in oParam.ItemHash.Values)
                {
                    InventoryManager.GetInstance().SetAvailableQty2(oParam.StockSysNo, item.ProductSysNo, -1 * item.Quantity);
                }

                scope.Complete();
            }
        }

        public bool CheckValidSRExist(int sosysno)
        {
            string sql = @"select * from sr_master where sosysno= " + sosysno + " and status <>" + (int)AppEnum.SRStatus.Abandon;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds) && ds.Tables[0].Rows.Count > 0)
                return true;
            else
                return false;
        }

    }
}
