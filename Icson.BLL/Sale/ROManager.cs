using System;
using System.Collections;
using System.Data;
using System.Text;
using Icson.Utils;
using Icson.Objects;
using Icson.Objects.Basic;
using Icson.Objects.Sale;
using Icson.Objects.Finance;
using Icson.DBAccess;
using Icson.DBAccess.Basic;
using Icson.DBAccess.Sale;
using System.Transactions;
using Icson.BLL.Basic;
using Icson.BLL.Finance;

namespace Icson.BLL.Sale
{
    /// <summary>
    /// Summary description for ROManager.
    /// </summary>
    public class ROManager
    {
        public ROManager()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        private static ROManager _instance = new ROManager();

        public static ROManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ROManager();
            }
            return _instance;
        }

        private void map(ROInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.ROID = Util.TrimNull(tempdr["ROID"]);
            oParam.RMASysNo = Util.TrimIntNull(tempdr["RMASysNo"]);
            oParam.StockSysNo = Util.TrimIntNull(tempdr["StockSysNo"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
            oParam.OriginCashAmt = Util.TrimDecimalNull(tempdr["OriginCashAmt"]);
            oParam.OriginPointAmt = Util.TrimIntNull(tempdr["OriginPointAmt"]);
            oParam.RedeemAmt = Util.TrimDecimalNull(tempdr["RedeemAmt"]);
            oParam.CashAmt = Util.TrimDecimalNull(tempdr["CashAmt"]);
            oParam.PointAmt = Util.TrimIntNull(tempdr["PointAmt"]);
            oParam.CreateUserSysNo = Util.TrimIntNull(tempdr["CreateUserSysNo"]);
            oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
            oParam.AuditUserSysNo = Util.TrimIntNull(tempdr["AuditUserSysNo"]);
            oParam.AuditTime = Util.TrimDateNull(tempdr["AuditTime"]);
            oParam.ReturnUserSysNo = Util.TrimIntNull(tempdr["ReturnUserSysNo"]);
            oParam.ReturnTime = Util.TrimDateNull(tempdr["ReturnTime"]);
            oParam.ReceiveName = Util.TrimNull(tempdr["ReceiveName"]);
            oParam.ReceiveAddress = Util.TrimNull(tempdr["ReceiveAddress"]);
            oParam.ReceivePhone = Util.TrimNull(tempdr["ReceivePhone"]);
            oParam.Note = Util.TrimNull(tempdr["Note"]);
        }

        private void map(ROItemInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.ROSysNo = Util.TrimIntNull(tempdr["ROSysNo"]);
            oParam.ProductSysNo = Util.TrimIntNull(tempdr["ProductSysNo"]);
            oParam.ReturnPriceType = Util.TrimIntNull(tempdr["ReturnPriceType"]);
            oParam.ReturnType = Util.TrimIntNull(tempdr["ReturnType"]);
            oParam.ReturnSysNo = Util.TrimIntNull(tempdr["ReturnSysNo"]);
            oParam.Quantity = Util.TrimIntNull(tempdr["Quantity"]);
            oParam.Price = Util.TrimDecimalNull(tempdr["Price"]);
            oParam.Cost = Util.TrimDecimalNull(tempdr["Cost"]);
            oParam.Weight = Util.TrimIntNull(tempdr["Weight"]);
            oParam.Point = Util.TrimIntNull(tempdr["Point"]);
            oParam.RefundCash = Util.TrimDecimalNull(tempdr["RefundCash"]);
            oParam.RefundPoint = Util.TrimIntNull(tempdr["RefundPoint"]);
        }

        public ROInfo LoadRO(int roSysNo)
        {
            string sqlMaster = @"select * from ro_master where sysno =" + roSysNo;
            DataSet dsMaster = SqlHelper.ExecuteDataSet(sqlMaster);
            ROInfo oRO = new ROInfo();
            if (Util.HasMoreRow(dsMaster))
            {
                map(oRO, dsMaster.Tables[0].Rows[0]);
                string sqlItem = @"select * from ro_item where rosysno =" + roSysNo;
                DataSet dsItem = SqlHelper.ExecuteDataSet(sqlItem);
                if (Util.HasMoreRow(dsItem))
                {
                    foreach (DataRow dr in dsItem.Tables[0].Rows)
                    {
                        ROItemInfo oROItem = new ROItemInfo();
                        map(oROItem, dr);
                        oRO.ItemHash.Add(oROItem.SysNo, oROItem);
                    }
                }
            }
            else
                oRO = null;
            return oRO;
        }

        public ROInfo LoadROfromRMA(int rmaSysNo)
        {
            string sqlMaster = @"select * from ro_master where status <> " + (int)AppEnum.RMAStatus.Abandon + " and rmasysno =" + rmaSysNo;
            DataSet dsMaster = SqlHelper.ExecuteDataSet(sqlMaster);
            ROInfo oRO = new ROInfo();
            if (Util.HasMoreRow(dsMaster))
            {
                map(oRO, dsMaster.Tables[0].Rows[0]);
                string sqlItem = @"select * from ro_item where rosysno =" + rmaSysNo;
                DataSet dsItem = SqlHelper.ExecuteDataSet(sqlItem);
                if (Util.HasMoreRow(dsItem))
                {
                    foreach (DataRow dr in dsItem.Tables[0].Rows)
                    {
                        ROItemInfo oROItem = new ROItemInfo();
                        map(oROItem, dr);
                        oRO.ItemHash.Add(oROItem.SysNo, oROItem);
                    }
                }
            }
            else
                oRO = null;
            return oRO;
        }

        public void Import()
        {
            string sql = @"select top 1 * from ro_master;
						   select top 1 * from ro_item;";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            foreach (DataTable dt in ds.Tables)
            {
                if (Util.HasMoreRow(dt))
                    throw new BizException("The target is not empty");
            }
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                //Insert Master
                string sqlDataCleanUp = @"--更新空的退货单创建人为审核人
										  update ipp2003..ut_master set utusersysno = approversysno where utusersysno  =-999";
                new ProductSaleTrendDac().Exec(sqlDataCleanUp);
                string sqlMaster = @"select um.sysno,utid as roid,rmasysno,stock.newsysno as stocksysno,redeemamt,cashamt as origincashamt,pointamt as originpointamt,
									 (cashamt-redeemamt/10) as cashamt,(pointamt+redeemamt) as pointamt,suct.newsysno as createusersysno,uttime as createtime,
									 suap.newsysno as auditusersysno ,approvetime as audittime,receiveaddress,receivephone,memo as note,um.status,um.customname as receivename
									 from ipp2003..ut_master um
									 left join ippconvert..stock stock on stock.oldsysno = um.warehousesysno
									 left join ippconvert..sys_user suct on suct.oldsysno = um.utusersysno
									 left join ippconvert..sys_user suap on suap.oldsysno = um.approversysno";
                DataSet dsMaster = SqlHelper.ExecuteDataSet(sqlMaster);
                if (Util.HasMoreRow(dsMaster))
                {
                    foreach (DataRow dr in dsMaster.Tables[0].Rows)
                    {
                        ROInfo oRO = new ROInfo();
                        oRO.SysNo = Util.TrimIntNull(dr["SysNo"]);
                        oRO.ROID = Util.TrimNull(dr["ROID"]);
                        oRO.RMASysNo = Util.TrimIntNull(dr["RMASysNo"]);
                        oRO.StockSysNo = Util.TrimIntNull(dr["StockSysNo"]);
                        switch (Util.TrimIntNull(dr["Status"]))
                        {
                            case -1://作废
                                oRO.Status = (int)AppEnum.ROStatus.Abandon;
                                break;
                            case 0://未审核，可修改
                                oRO.Status = (int)AppEnum.ROStatus.Origin;
                                break;
                            case 1://待审核
                                oRO.Status = (int)AppEnum.ROStatus.Origin;
                                break;
                            case 2://已审核
                                oRO.Status = (int)AppEnum.ROStatus.Audited;
                                break;
                            case 3://已退货
                                oRO.Status = (int)AppEnum.ROStatus.Returned;
                                break;
                        }
                        oRO.OriginCashAmt = Util.TrimDecimalNull(dr["OriginCashAmt"]);
                        oRO.OriginPointAmt = Util.TrimIntNull(dr["OriginPointAmt"]);
                        oRO.RedeemAmt = Util.TrimDecimalNull(dr["RedeemAmt"]);
                        oRO.CashAmt = Util.TrimDecimalNull(dr["CashAmt"]);
                        oRO.PointAmt = Util.TrimIntNull(dr["PointAmt"]);
                        oRO.CreateUserSysNo = Util.TrimIntNull(dr["CreateUserSysNo"]);
                        oRO.CreateTime = Util.TrimDateNull(dr["CreateTime"]);
                        oRO.AuditUserSysNo = Util.TrimIntNull(dr["AuditUserSysNo"]);
                        oRO.AuditTime = Util.TrimDateNull(dr["AuditTime"]);
                        oRO.ReceiveName = Util.TrimNull(dr["ReceiveName"]);
                        oRO.ReceiveAddress = Util.TrimNull(dr["ReceiveAddress"]);
                        oRO.ReceivePhone = Util.TrimNull(dr["ReceivePhone"]);
                        oRO.Note = Util.TrimNull(dr["Note"]);
                        new RODac().InsertMaster(oRO);
                    }
                }
                //Insert Item
                string sqlItem = @"select 1 as sysno ,utsysno as rosysno,pb.newsysno as productsysno,2 as returntype,pb.newsysno as returnsysno,quantity,price,cost,weight,returnpoint as point,
								   isnull(refundcash,0) as refundcash,isnull(refundpoint,0) as refundpoint
								   from ipp2003..ut_item ui
								   left join ippconvert..productbasic pb on pb.oldsysno = ui.productsysno";
                DataSet dsItem = SqlHelper.ExecuteDataSet(sqlItem);
                if (Util.HasMoreRow(dsItem))
                {
                    foreach (DataRow dr in dsItem.Tables[0].Rows)
                    {
                        ROItemInfo oROItem = new ROItemInfo();
                        map(oROItem, dr);
                        new RODac().InsertItem(oROItem);
                    }
                }
                //Insert Sequence
                string sqlMaxSysNo = @"select max(sysno) as sysno from rma_master";
                DataSet dsMaxSysNo = SqlHelper.ExecuteDataSet(sqlMaxSysNo);
                int n = 0;
                while (n < Util.TrimIntNull(dsMaxSysNo.Tables[0].Rows[0][0]))
                {
                    n = SequenceDac.GetInstance().Create("RO_Sequence");
                }
                scope.Complete();
            }
        }

        public void AddRO(ROInfo roInfo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                roInfo.SysNo = SequenceDac.GetInstance().Create("RO_Sequence");
                roInfo.ROID = this.BuildROID(roInfo.SysNo);
                new RODac().InsertMaster(roInfo);
                if (roInfo.ItemHash.Count > 0)
                {
                    foreach (ROItemInfo roItem in roInfo.ItemHash.Values)
                    {
                        roItem.ROSysNo = roInfo.SysNo;
                        new RODac().InsertItem(roItem);
                    }
                }
                else
                    throw new BizException("Can't create an empty RO'");
                scope.Complete();
            }
        }

        public ROInfo BuildROFromRMA(RMAInfo rmaInfo)
        {
            ROInfo roInfo = new ROInfo();
            SOInfo soInfo = SaleManager.GetInstance().LoadSO(rmaInfo.SOSysNo);
            roInfo.RMASysNo = rmaInfo.SysNo;
            roInfo.CreateTime = DateTime.Now;
            roInfo.CreateUserSysNo = rmaInfo.RMAUserSysNo;
            roInfo.StockSysNo = soInfo.StockSysNo;
            roInfo.Status = (int)AppEnum.ROStatus.Origin;
            roInfo.ReceiveName = soInfo.ReceiveName;
            roInfo.ReceivePhone = soInfo.ReceivePhone;
            roInfo.ReceiveAddress = soInfo.ReceiveAddress;
            roInfo.PointAmt = 0;
            roInfo.CashAmt = 0m;
            roInfo.RedeemAmt = 0m;
            Hashtable leftHash = RMAManager.GetInstance().GetLeftRMAQty(rmaInfo.SOSysNo);
            decimal originCashAmt = 0m;
            int originPointAmt = 0;
            foreach (RMAItemInfo rmaItem in rmaInfo.ItemHash.Values)
            {
                if (rmaItem.RMAType == (int)AppEnum.RMAType.Return)
                {
                    ROItemInfo roItem = new ROItemInfo();
                    roItem.ProductSysNo = rmaItem.ProductSysNo;
                    roItem.Quantity = rmaItem.RMAQty;
                    if (roItem.Quantity > (int)leftHash[(int)roItem.ProductSysNo])
                        throw new BizException("Too many item(" + roItem.ProductSysNo + ") to return");
                    foreach (SOItemInfo soItem in soInfo.ItemHash.Values)
                    {
                        if (soItem.ProductSysNo == rmaItem.ProductSysNo)
                        {
                            roItem.Price = soItem.Price;
                            roItem.Point = soItem.Point;
                            roItem.Cost = soItem.Cost;
                            roItem.Weight = soItem.Weight;
                            roItem.RefundPoint = soItem.Point;
                            roItem.RefundCash = soItem.Price;
                            roItem.ReturnPriceType = (int)AppEnum.ReturnPriceType.InputPrice;
                            roItem.ReturnType = (int)AppEnum.ReturnType.SecondHand;
                            originCashAmt += roItem.Price * roItem.Quantity;
                            originPointAmt += roItem.Point * roItem.Quantity;
                            break;
                        }
                    }
                    roInfo.ItemHash.Add(roItem.ProductSysNo, roItem);
                }
            }
            roInfo.OriginPointAmt = originPointAmt;
            roInfo.OriginCashAmt = originCashAmt;

            return roInfo;
        }

        public void UpdateROMaster(Hashtable paramHash)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                new RODac().UpdateMaster(paramHash);
                scope.Complete();
            }
        }

        public void UpdateROMaster(ROInfo roInfo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                Hashtable paramHash = new Hashtable();
                paramHash.Add("AuditTime", roInfo.AuditTime);
                paramHash.Add("AuditUserSysNo", roInfo.AuditUserSysNo);
                paramHash.Add("CashAmt", roInfo.CashAmt);
                paramHash.Add("Note", roInfo.Note);
                paramHash.Add("OriginCashAmt", roInfo.OriginCashAmt);
                paramHash.Add("OriginPointAmt", roInfo.OriginPointAmt);
                paramHash.Add("PointAmt", roInfo.PointAmt);
                paramHash.Add("ReceiveName", roInfo.ReceiveName);
                paramHash.Add("ReceiveAddress", roInfo.ReceiveAddress);
                paramHash.Add("ReceivePhone", roInfo.ReceivePhone);
                paramHash.Add("RedeemAmt", roInfo.RedeemAmt);
                paramHash.Add("ReturnTime", roInfo.ReturnTime);
                paramHash.Add("ReturnUserSysNo", roInfo.ReturnUserSysNo);
                paramHash.Add("RMASysNo", roInfo.RMASysNo);
                paramHash.Add("ROID", roInfo.ROID);
                paramHash.Add("Status", roInfo.Status);
                paramHash.Add("StockSysNo", roInfo.StockSysNo);
                paramHash.Add("SysNo", roInfo.SysNo);
                this.UpdateROMaster(paramHash);
                scope.Complete();
            }
        }

        public void UpdateROItem(ROItemInfo roItem)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                new RODac().UpdateItem(roItem);
                scope.Complete();
            }
        }

        public void UpdateRO(ROInfo roInfo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                this.UpdateROMaster(roInfo);
                foreach (ROItemInfo roItem in roInfo.ItemHash.Values)
                {
                    this.UpdateROItem(roItem);
                }
                scope.Complete();
            }
        }

        public void AuditRO(ROInfo roInfo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                if (this.GetROCurrentStatus(roInfo.SysNo) != (int)AppEnum.ROStatus.Origin)
                    throw new BizException("This RO is not origin now ,can't be audited");
                roInfo.Status = (int)AppEnum.ROStatus.Audited;
                this.CalcRO(roInfo);
                this.UpdateRO(roInfo);
                scope.Complete();
            }
        }

        public void CancelAuditRO(ROInfo roInfo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                if (this.GetROCurrentStatus(roInfo.SysNo) != (int)AppEnum.ROStatus.Audited)
                    throw new BizException("This RO is not audited now ,can't cancel audit");
                roInfo.Status = (int)AppEnum.ROStatus.Origin;
                Hashtable paramHash = new Hashtable(4);
                paramHash.Add("Status", roInfo.Status);
                paramHash.Add("ReturnTime", roInfo.AuditTime);
                paramHash.Add("ReturnUserSysNo", roInfo.AuditUserSysNo);
                paramHash.Add("SysNo", roInfo.SysNo);
                this.UpdateROMaster(paramHash);
                scope.Complete();
            }
        }

        public void ReturnRO(ROInfo roInfo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                if (this.GetROCurrentStatus(roInfo.SysNo) != (int)AppEnum.ROStatus.Audited)
                    throw new BizException("This RO is not Audited now ,can't be returned");
                roInfo.Status = (int)AppEnum.ROStatus.Returned;
                //update ro status
                Hashtable paramHash = new Hashtable(4);
                paramHash.Add("Status", roInfo.Status);
                paramHash.Add("ReturnTime", roInfo.ReturnTime);
                paramHash.Add("ReturnUserSysNo", roInfo.ReturnUserSysNo);
                paramHash.Add("SysNo", roInfo.SysNo);
                this.UpdateROMaster(paramHash);
                //update inventory
                foreach (ROItemInfo roItem in roInfo.ItemHash.Values)
                {
                    //UnitCostManager.GetInstance().SetCost(roItem.ProductSysNo, roItem.Quantity, roItem.Cost); //Serious bug 2007-07-21
                    //退货时不影响一手商品的成本，只需要修改二手商品的成本
                    UnitCostManager.GetInstance().SetCost(roItem.ReturnSysNo, roItem.Quantity, roItem.Cost);
                    InventoryManager.GetInstance().SetInStockQty(roInfo.StockSysNo, roItem.ReturnSysNo, roItem.Quantity);
                }
                //update customer score
                RMAInfo oRMA = RMAManager.GetInstance().LoadMaster(roInfo.RMASysNo);
                //先将销售单所得积分加上
                PointManager.GetInstance().DoDelayPointSingle(oRMA.SOSysNo);
                //然后减去退货对应积分
                if (roInfo.PointAmt != 0)
                {
                    PointManager.GetInstance().SetScore(oRMA.CustomerSysNo, roInfo.PointAmt * (-1), (int)AppEnum.PointLogType.ReturnProduct, roInfo.SysNo.ToString());
                }
                //生成收款单
                //如果无有效的收款单-->生成soincome(normal, origin)
                SOIncomeInfo soIncome = SOIncomeManager.GetInstance().LoadValid((int)AppEnum.SOIncomeOrderType.RO, roInfo.SysNo);
                if (soIncome == null)//无有效收款单，生成收款单
                {
                    soIncome = new SOIncomeInfo();
                    soIncome.OrderType = (int)AppEnum.SOIncomeOrderType.RO;
                    soIncome.OrderSysNo = roInfo.SysNo;
                    soIncome.OrderAmt = soIncome.IncomeAmt = Util.TruncMoney(roInfo.CashAmt + roInfo.RedeemAmt);
                    soIncome.IncomeStyle = (int)AppEnum.SOIncomeStyle.Normal;
                    soIncome.IncomeUserSysNo = roInfo.ReturnUserSysNo;
                    soIncome.IncomeTime = DateTime.Now;
                    soIncome.Status = (int)AppEnum.SOIncomeStatus.Origin;
                    SOIncomeManager.GetInstance().Insert(soIncome);
                }
                scope.Complete();
            }
        }

        public void CancelReturnRO(ROInfo roInfo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                if (this.GetROCurrentStatus(roInfo.SysNo) != (int)AppEnum.ROStatus.Returned)
                    throw new BizException("This RO is not returned now ,can't cancel return");
                roInfo.Status = (int)AppEnum.ROStatus.Audited;
                //update ro status
                Hashtable paramHash = new Hashtable(4);
                paramHash.Add("Status", roInfo.Status);
                paramHash.Add("ReturnTime", roInfo.ReturnTime);
                paramHash.Add("ReturnUserSysNo", roInfo.ReturnUserSysNo);
                paramHash.Add("SysNo", roInfo.SysNo);
                this.UpdateROMaster(paramHash);
                //update inventory
                foreach (ROItemInfo roItem in roInfo.ItemHash.Values)
                {
                    //UnitCostManager.GetInstance().SetCost(roItem.ProductSysNo, roItem.Quantity*(-1), roItem.Cost);
                    UnitCostManager.GetInstance().SetCost(roItem.ReturnSysNo, roItem.Quantity * (-1), roItem.Cost);
                    InventoryManager.GetInstance().SetInStockQty(roInfo.StockSysNo, roItem.ReturnSysNo, roItem.Quantity * (-1));
                }
                //update customer score
                if (roInfo.PointAmt != 0)
                {
                    RMAInfo oRMA = RMAManager.GetInstance().LoadMaster(roInfo.RMASysNo);
                    PointManager.GetInstance().SetScore(oRMA.CustomerSysNo, roInfo.PointAmt, (int)AppEnum.PointLogType.CancelReturn, roInfo.SysNo.ToString());
                }
                //abandon soincome
                SOIncomeManager.GetInstance().ROCancelReturn(roInfo.SysNo);
                scope.Complete();
            }
        }

        public void AbandonRO(ROInfo roInfo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                if (this.GetROCurrentStatus(roInfo.SysNo) != (int)AppEnum.ROStatus.Origin)
                    throw new BizException("This RO is not origin ,can't abandon");
                roInfo.Status = (int)AppEnum.ROStatus.Abandon;
                Hashtable paramHash = new Hashtable(4);
                paramHash.Add("Status", roInfo.Status);
                paramHash.Add("ReturnTime", roInfo.CreateTime);
                paramHash.Add("ReturnUserSysNo", roInfo.CreateUserSysNo);
                paramHash.Add("SysNo", roInfo.SysNo);
                this.UpdateROMaster(paramHash);
                scope.Complete();
            }
        }

        private string BuildROID(int sysNo)
        {
            string sysNoStr = sysNo.ToString();
            int idLen = 10;
            string roid = "9";
            for (int i = 0; i < (idLen - sysNoStr.Length - 1); i++)
            {
                roid += "0";
            }
            roid += sysNoStr;
            return roid;
        }

        private void CalcRO(ROInfo roInfo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                RMAInfo rmaInfo = RMAManager.GetInstance().LoadMaster(roInfo.RMASysNo);
                if (rmaInfo == null || (rmaInfo.Status != (int)AppEnum.RMAStatus.Handled && rmaInfo.Status != (int)AppEnum.RMAStatus.Closed))
                    throw new BizException("Related RMA Sheet load error");
                SOInfo soInfo = SaleManager.GetInstance().LoadSO(rmaInfo.SOSysNo);
                if (soInfo == null || soInfo.Status != (int)AppEnum.SOStatus.OutStock)
                    throw new BizException("Related SO load error");
                decimal cashPayRate = soInfo.CashPay / soInfo.SOAmt;//获取销售单商品金额的现金支付比例
                decimal refundCash = 0m;
                int refundPoint = 0;
                foreach (ROItemInfo roItem in roInfo.ItemHash.Values)
                {
                    roItem.RefundCash = roItem.Price * cashPayRate * roItem.Quantity;
                    roItem.RefundPoint = (roItem.Point - Convert.ToInt32(decimal.Round(roItem.Price * (1 - cashPayRate), 1) * AppConst.ExchangeRate)) * roItem.Quantity;
                    refundCash += roItem.RefundCash;
                    refundPoint += roItem.RefundPoint;
                }
                roInfo.CashAmt = Util.ToMoney(refundCash);
                roInfo.PointAmt = refundPoint;
                CustomerInfo cmInfo = CustomerManager.GetInstance().Load(soInfo.CustomerSysNo);
                if ((cmInfo.ValidScore - refundPoint) < 0)
                {
                    roInfo.RedeemAmt = Convert.ToDecimal(cmInfo.ValidScore + refundPoint) / AppConst.ExchangeRate;
                }
                else
                {
                    roInfo.RedeemAmt = 0m;
                }
                scope.Complete();
            }
        }

        private int GetROCurrentStatus(int sysno)
        {
            int status = AppConst.IntNull;
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                string sql = "select status from ro_master where sysno=" + sysno;
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (Util.HasMoreRow(ds))
                    status = (int)ds.Tables[0].Rows[0][0];
                scope.Complete();
            }
            return status;
        }

        //		public DataSet GetROList(Hashtable paramHash)
        //		{
        //			string sql = @"select
        //								ro.sysno , ro.roid,ro.rmasysno,rma.rmaid,rma.sosysno,so.soid,ro.createtime,ro.status,customer.customerid
        //						   from 
        //								ro_master ro 
        //						   inner join rma_master rma on rma.sysno = ro.rmasysno
        //						   inner join so_master so on so.sysno = rma.sosysno
        //						   inner join customer on customer.sysno = so.customersysno";
        //			if(paramHash.Count>0)
        //			{
        //				StringBuilder sb = new StringBuilder();
        //				sb.Append(" where 1=1 ");
        //				foreach(string key in paramHash.Keys)
        //				{
        //					sb.Append(" and ");
        //					object item = paramHash[key];
        //					if(key=="StartDate")
        //					{
        //						sb.Append("ro.createtime").Append(">=").Append(Util.ToSqlString(item.ToString()));
        //					}
        //					else if(key=="EndDate")
        //					{
        //						sb.Append("ro.createtime").Append("<=").Append(Util.ToSqlEndDate(item.ToString()));
        //					}
        //					else if(key=="Status")
        //					{
        //						sb.Append("ro.status").Append("=").Append(item.ToString());
        //					}
        //					else if ( item is int)
        //					{
        //						sb.Append(key).Append("=" ).Append(item.ToString());
        //					}
        //					else if ( item is string)
        //					{
        //						sb.Append(key).Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
        //					}
        //				}
        //				sql += sb.ToString();
        //			}
        //			else
        //			{
        //				sql.Replace("select","select top 50");
        //			}
        //			sql += " order by ro.sysno desc";
        //			return SqlHelper.ExecuteDataSet(sql);
        //		}

        public DataSet GetROList(Hashtable paramHash)
        {
            string sql = "";
            if (!paramHash.ContainsKey("ProductID"))
            {
                sql = @"select
								ro.sysno , ro.roid,ro.rmasysno,rma.rmaid,rma.sosysno,so.soid,ro.createtime,ro.status,customer.customerid, ro.OriginCashAmt, ro.CashAmt
						   from 
								ro_master ro 
						   inner join rma_master rma on rma.sysno = ro.rmasysno
						   inner join so_master so on so.sysno = rma.sosysno
						   inner join customer on customer.sysno = so.customersysno";
            }
            else
            {
                sql = @"select
								ro.sysno , ro.roid,ro.rmasysno,rma.rmaid,rma.sosysno,so.soid,ro.createtime,ro.status,customer.customerid, ro.OriginCashAmt, ro.CashAmt
						   from 
								ro_master ro 
						   inner join ro_item ri on ro.sysno = ri.rosysno 
						   inner join product p on ri.productsysno = p.sysno and p.productid=@productid 
						   inner join rma_master rma on rma.sysno = ro.rmasysno
						   inner join so_master so on so.sysno = rma.sosysno 
						   inner join customer on customer.sysno = so.customersysno";
                sql = sql.Replace("@productid", "'" + paramHash["ProductID"].ToString() + "'");
            }
            if (paramHash.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" where 1=1 ");
                foreach (string key in paramHash.Keys)
                {
                    sb.Append(" and ");
                    object item = paramHash[key];
                    if (key == "StartDate")
                    {
                        sb.Append("ro.createtime").Append(">=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "EndDate")
                    {
                        sb.Append("ro.createtime").Append("<=").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "Status")
                    {
                        sb.Append("ro.status").Append("=").Append(item.ToString());
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
                sql.Replace("select", "select top 50");
            }
            sql += " order by ro.sysno desc";
            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetROSummaryList(Hashtable paramHash)
        {
            string sql = @"SELECT RO_Item.SysNo,RO_Master.SysNo AS ROSysNo,RO_Master.ROID,RO_Master.RMASysNo,RMA_Master.RMAID,
                            SO_Master.SysNo AS SOSysNo,SO_Master.SOID,RO_Master.ReturnTime,RO_Master.Status,
                            RO_Item.RefundCash,RO_Item.Cost * RO_Item.Quantity AS SumCost,RO_Item.Quantity,
                            RO_Item.ProductSysNo,Product.ProductName,Sys_User.UserName,RO_Item.ReturnType,
                            (-RO_Master.originpointamt+RO_Master.pointamt) as pointpay,RO_Master.originpointamt as pointget,
                            (-RO_Master.cashamt + (-RO_Master.originpointamt+RO_Master.pointamt)/10.0) as soamt, 
                            (-RO_Master.originpointamt) as pointamt,-RO_Master.cashamt as TotalAmount,fsv.voucherid
                            FROM RO_Master 
                            INNER JOIN RMA_Master ON RMA_Master.SysNo = RO_Master.RMASysNo 
                            INNER JOIN SO_Master ON RMA_Master.SOSysNo = SO_Master.SysNo
                            INNER JOIN RO_Item ON RO_Master.SysNo = RO_Item.ROSysNo 
                            INNER JOIN Product ON Product.SysNo = RO_Item.ProductSysNo 
                            INNER JOIN Sys_User ON Sys_User.SysNo = Product.PMUserSysNo 
                            INNER JOIN Category3 ON Category3.SysNo = Product.C3SysNo 
                            INNER JOIN Category2 ON Category3.C2SysNo = Category2.SysNo
                            INNER JOIN Category1 ON Category2.C1SysNo = Category1.SysNo 
                            left join finance_soincome fs on fs.ordersysno = ro_master.sysno and fs.status>" + (int)AppEnum.SOIncomeStatus.Abandon + " and fs.ordertype=" + (int)AppEnum.SOIncomeOrderType.RO
                      + @"  left join finance_soincome_voucher fsv on fsv.fsisysno = fs.sysno ";

            if (paramHash.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" where 1=1 ");
                foreach (string key in paramHash.Keys)
                {
                    sb.Append(" and ");
                    object item = paramHash[key];
                    if (key == "StartDate")
                    {
                        sb.Append("ro_master.returntime").Append(">=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "EndDate")
                    {
                        sb.Append("ro_master.returntime").Append("<=").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "Status")
                    {
                        sb.Append("ro_master.status").Append("=").Append(item.ToString());
                    }
                    else if (key == "Category")
                    {
                        sb.Append(item.ToString());
                    }
                    else if (key == "PPMUserSysNo")
                    {
                        sb.Append("sys_user.sysno").Append("=").Append(item.ToString());
                    }
                    else if (key == "ReturnType")
                    {
                        sb.Append("ro_item.returntype").Append("=").Append(item.ToString());
                    }
                    else if (key == "VoucherID")
                    {
                        sb.Append("fsv.voucherid ").Append("=").Append(Util.ToSqlString(item.ToString()));
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
                sql.Replace("select", "select top 50");
            }
            sql += " ORDER BY RO_Master.ReturnTime DESC";
            return SqlHelper.ExecuteDataSet(sql);
        }

        #region Print RO Invoice
        /// <summary>
        /// 获取ro发票
        /// </summary>
        /// <param name="roSysNo"></param>
        /// <returns></returns>
        public ROInvoiceInfo GetROInvoice(int roSysNo)
        {
            ROInvoiceInfo invoice = new ROInvoiceInfo();
            ROInfo ro = this.LoadRO(roSysNo);
            RMAInfo rma = RMAManager.GetInstance().Load(ro.RMASysNo);
            SOInfo so = SaleManager.GetInstance().LoadSO(rma.SOSysNo);
            IcsonInfo son = new IcsonInfo();
            invoice.AuditUserSysNo = ro.AuditUserSysNo;
            invoice.CompanyAddress = son.CompanyAddress;
            invoice.InvoiceNote = so.InvoiceNote;
            invoice.SOID = so.SOID;
            PayTypeInfo ptInfo = ASPManager.GetInstance().LoadPayType(so.PayTypeSysNo);
            invoice.PayTypeName = ptInfo.PayTypeName;
            invoice.ReceiveAddress = ro.ReceiveAddress;
            CustomerInfo customer = CustomerManager.GetInstance().Load(so.CustomerSysNo);
            invoice.CustomerName = customer.CustomerName;
            invoice.CustomerSysNo = customer.SysNo;
            invoice.ReceiveName = ro.ReceiveName;
            invoice.ReceivePhone = ro.ReceivePhone;
            invoice.ROID = ro.ROID;
            invoice.ROSysNo = ro.SysNo;
            ShipTypeInfo stInfo = ASPManager.GetInstance().LoadShipType(so.ShipTypeSysNo);
            invoice.ShipTypeName = stInfo.ShipTypeName;
            UserInfo employee = SysManager.GetInstance().LoadUser(so.OutUserSysNo);
            if (employee != null)
            {
                invoice.WarehouseUserCode = employee.UserID;
            }
            else
            {
                invoice.WarehouseUserCode = "";
            }
            this.InitPageList(ro, invoice);
            invoice.TotalPage = invoice.ItemHash.Count;
            invoice.TotalWeight = ro.GetTotalWeight();
            return invoice;
        }

        private void InitPageList(ROInfo ro, ROInvoiceInfo invoice)
        {
            int index = 0;
            ROInvoicePageInfo page = new ROInvoicePageInfo();
            invoice.ItemHash.Add(index++, page);
            Hashtable sysHash = new Hashtable(5);
            foreach (ROItemInfo item in ro.ItemHash.Values)
            {
                sysHash.Add(item.ProductSysNo, null);
            }
            Hashtable pbHash = ProductManager.GetInstance().GetProductBoundle(sysHash);
            foreach (ROItemInfo item in ro.ItemHash.Values)
            {
                ROInvoicePageItemInfo printItem = new ROInvoicePageItemInfo();
                printItem.pk = ((ProductBasicInfo)pbHash[item.ProductSysNo]).ProductID;  //add by lucky 2008/04/16
                printItem.ProductID = ((ProductBasicInfo)pbHash[item.ProductSysNo]).ProductID;
                printItem.ProductName = ((ProductBasicInfo)pbHash[item.ProductSysNo]).ProductName;
                printItem.Quantity = item.Quantity * (-1);
                printItem.Weight = item.Weight;
                printItem.Total = (-1) * item.RefundCash;
                printItem.Price = (decimal)(item.RefundCash / item.Quantity);
                printItem.isRoItem = true;
                if (page.AddItem(printItem) == true)
                {
                    continue;
                }
                else
                {
                    page = new ROInvoicePageInfo();
                    page.AddItem(printItem);
                    invoice.ItemHash.Add(index++, page);
                }
                sysHash.Add(item.ProductSysNo, null);
            }

            if (ro.PointAmt != 0)
            {
                ROInvoicePageItemInfo printItem = new ROInvoicePageItemInfo();
                printItem.pk = "影响积分"; //add by lucky 2008/04/16
                printItem.ProductID = "影响积分";
                printItem.Total = Convert.ToDecimal(ro.PointAmt);
                printItem.isRoItem = false;
                printItem.isPoint = true;
                if (page.AddItem(printItem) == false)
                {
                    page = new ROInvoicePageInfo();
                    page.AddItem(printItem);
                    invoice.ItemHash.Add(index++, page);
                }
            }

            if (ro.RedeemAmt != 0)
            {
                ROInvoicePageItemInfo printItem = new ROInvoicePageItemInfo();
                printItem.pk = "补积分"; //add by lucky 2008/04/16
                printItem.ProductID = "补积分";
                printItem.Total = ro.RedeemAmt;
                printItem.isRoItem = false;
                if (page.AddItem(printItem) == false)
                {
                    page = new ROInvoicePageInfo();
                    page.AddItem(printItem);
                    invoice.ItemHash.Add(index++, page);
                }
            }
        }
        #endregion
    }
}
