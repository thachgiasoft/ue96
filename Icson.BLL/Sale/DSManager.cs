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
using Icson.Objects.RMA;
using Icson.Objects.Stock;


namespace Icson.BLL.Sale
{
    /// <summary>
    /// Delivery Settlement 配送结算单
    /// </summary>
    public class DSManager
    {
        private DSManager()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        private static DSManager _instance;
        public static DSManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DSManager();
            }
            return _instance;
        }

        private void map(DSInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.DLSysNo = Util.TrimIntNull(tempdr["DLSysNo"]);
            oParam.FreightUserSysNo = Util.TrimIntNull(tempdr["FreightUserSysNo"]);
            oParam.ARAmt = Util.TrimDecimalNull(tempdr["ARAmt"]);
            oParam.IncomeAmt = Util.TrimDecimalNull(tempdr["IncomeAmt"]);
            oParam.CreateUserSysNo = Util.TrimIntNull(tempdr["CreateUserSysNo"]);
            oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
            oParam.SettlementUserSysNo = Util.TrimIntNull(tempdr["SettlementUserSysNo"]);
            oParam.SettlementTime = Util.TrimDateNull(tempdr["SettlementTime"]);
            oParam.VoucherUserSysNo = Util.TrimIntNull(tempdr["VoucherUserSysNo"]);
            oParam.VoucherID = Util.TrimNull(tempdr["VoucherID"]);
            oParam.VoucherTime = Util.TrimDateNull(tempdr["VoucherTime"]);
            oParam.AbandonUserSysNo = Util.TrimIntNull(tempdr["AbandonUserSysNo"]);
            oParam.AbandonTime = Util.TrimDateNull(tempdr["AbandonTime"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
            oParam.AuditUserSysNo = Util.TrimIntNull(tempdr["AuditUserSysNo"]);
            oParam.AuditTime = Util.TrimDateNull(tempdr["AuditTime"]);
            oParam.PosFee = Util.TrimDecimalNull(tempdr["PosFee"]);
            oParam.Cheque = Util.TrimDecimalNull(tempdr["Cheque"]);
            oParam.Cash = Util.TrimDecimalNull(tempdr["Cash"]);
            oParam.PosGoods = Util.TrimDecimalNull(tempdr["PosGoods"]);
            oParam.Remittance = Util.TrimDecimalNull(tempdr["Remittance"]);
            oParam.RemittanceDate = Util.TrimDateNull(tempdr["RemittanceDate"]);
            oParam.Memo = Util.TrimNull(tempdr["Memo"]);
            oParam.RemittanceType = Util.TrimIntNull(tempdr["RemittanceType"]);
            oParam.IsReceiveVoucher = Util.TrimIntNull(tempdr["IsReceiveVoucher"]);
            oParam.PosGoodsCash = Util.TrimDecimalNull(tempdr["PosGoodsCash"]);
            oParam.AccAuditUserSysNo = Util.TrimIntNull(tempdr["AccAuditUserSysNo"]);
            oParam.AccAuditTime = Util.TrimDateNull(tempdr["AccAuditTime"]);
        }
        private void map(DSItemInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.DSSysNo = Util.TrimIntNull(tempdr["DSSysNo"]);
            oParam.ItemID = Util.TrimNull(tempdr["ItemID"]);
            oParam.ItemType = Util.TrimIntNull(tempdr["ItemType"]);
            oParam.PayType = Util.TrimIntNull(tempdr["PayType"]);
            oParam.PayAmt = Util.TrimDecimalNull(tempdr["PayAmt"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
            oParam.PosFee = Util.TrimDecimalNull(tempdr["PosFee"]);
            oParam.DLSysNo = Util.TrimIntNull(tempdr["DLSysNo"]);
            oParam.IsPos = Util.TrimIntNull(tempdr["IsPos"]);
            oParam.PosNo = Util.TrimNull(tempdr["PosNo"]);
            oParam.PosDate = Util.TrimDateNull(tempdr["PosDate"]);
        }

        private void InsertDSMaster(DSInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                oParam.SysNo = SequenceDac.GetInstance().Create("DS_Sequence");
                new DSDac().Insert(oParam);
                scope.Complete();
            }
        }

        public void UpdateDSMaster(DSInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                new DSDac().Update(oParam);
                scope.Complete();
            }
        }

        public void UpdateDSItem(DSItemInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                new DSDac().Update(oParam);
                scope.Complete();
            }
        }

        public DSInfo LoadDSMaster(int sysno)
        {
            string sql = "select * from DS_Master where sysno =" + sysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            DSInfo oInfo = new DSInfo();
            if (Util.HasMoreRow(ds))
            {
                map(oInfo, ds.Tables[0].Rows[0]);
                return oInfo;
            }
            else
                return null;

        }

        public DSItemInfo LoadDSItem(int sysno)
        {
            string sql = "select * from DS_Item where sysno =" + sysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            DSItemInfo oItemInfo = new DSItemInfo();
            if (Util.HasMoreRow(ds))
            {
                map(oItemInfo, ds.Tables[0].Rows[0]);
                return oItemInfo;
            }
            else
                return null;

        }


        public void UpdateDSMaster(Hashtable paramHash)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                new DSDac().UpdateMaster(paramHash);
                scope.Complete();
            }
        }

        private void InsertDSItem(DSItemInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                new DSDac().Insert(oParam);
                scope.Complete();
            }
        }

        public void UpdateItemPosFee(int dssysno)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                string sql = @" update DS_Item set PosFee=null,IsPos=null  where DSSysNo=" + dssysno;
                SqlHelper.ExecuteDataSet(sql);
                scope.Complete();
            }
        }


        public void CreateDS(DSInfo dsInfo)
        {
            try
            {
                TransactionOptions options = new TransactionOptions();
                options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                options.Timeout = TransactionManager.DefaultTimeout;

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    dsInfo.Status = (int)AppEnum.DSStatus.Origin;
                    dsInfo.CreateTime = DateTime.Now;

                    //加入配送结算单主项
                    this.InsertDSMaster(dsInfo);

                    //加入配送结算单明细
                    decimal ARAmt = 0; //应收款合计

                    foreach (DSItemInfo item in dsInfo.ItemHash.Values)
                    {
                        item.DSSysNo = dsInfo.SysNo;
                        item.Status = (int)AppEnum.BiStatus.Valid;
                        this.InsertDSItem(item);

                        if (item.PayType == 1) //货到付款
                        {
                            ARAmt += item.PayAmt;
                        }
                    }

                    dsInfo.ARAmt = ARAmt;

                    UpdateDSMaster(dsInfo);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                dsInfo.SysNo = AppConst.IntNull;
                throw ex;
            }
        }

        public void CreateDS(string dlsysnolist, int usersysno, DSInfo oDSInfo)
        {
            try
            {
                TransactionOptions options = new TransactionOptions();
                options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                options.Timeout = TransactionManager.DefaultTimeout;

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    decimal ARAmt = 0;
                    decimal IncomeAmt = 0;
                    int FreightUser = 0;
                    oDSInfo.ItemHash.Clear();
                    string NullFreightItem = "";
                    string[] DLSysNoList = dlsysnolist.Split(',');
                    int i = 0;

                    for (i = 0; i < DLSysNoList.Length; i++)
                    {
                        int DLSysNo = Int32.Parse(DLSysNoList[i]);
                        DLInfo oDLInfo = DLManager.GetInstance().Load(DLSysNo);
                        if (oDLInfo == null)
                        {
                            throw new BizException(DLSysNo + "对应的配送单不存在");
                        }
                        else if (oDLInfo.Status != (int)AppEnum.DLStatus.StockConfirmed)
                        {

                            throw new BizException(DLSysNo + "配送单不是确认发货状态，不能生成结算单");
                        }
                        else if (oDLInfo.Status == (int)AppEnum.DLStatus.DSConfirmed)
                        {
                            throw new BizException(DLSysNo + "此单已生成结算单，不能重复生成");
                        }

                        FreightUser = oDLInfo.FreightUserSysNo;
                        foreach (DLItemInfo oItem in oDLInfo.ItemHash.Values)
                        {
                            if (oItem.Status == (int)AppEnum.BiStatus.Valid)
                            {
                                if (oItem.ItemType == (int)AppEnum.DLItemType.SaleOrder)
                                {
                                    int sosysno = Convert.ToInt32(oItem.ItemID.Substring(1));
                                    SOInfo oSOInfo = SaleManager.GetInstance().LoadSOMaster(sosysno);
                                    if (oSOInfo.FreightUserSysNo == AppConst.IntNull)
                                    {
                                        NullFreightItem += oSOInfo.SOID + ",";

                                    }
                                    else if (NullFreightItem == "" && oSOInfo.FreightUserSysNo == oDLInfo.FreightUserSysNo && oSOInfo.DLSysNo == oDLInfo.SysNo)
                                    {
                                        if (oSOInfo.Status == (int)AppEnum.SOStatus.OutStock)
                                        {
                                            DSItemInfo oDSItem = new DSItemInfo();
                                            oDSItem.ItemID = oItem.ItemID;
                                            oDSItem.ItemType = oItem.ItemType;
                                            oDSItem.PayType = oItem.PayType;
                                            oDSItem.DLSysNo = oItem.DLSysNo;

                                            if (oSOInfo.PayTypeSysNo == 1) //货到付款去零头
                                            {
                                                oDSItem.PayAmt = Util.TruncMoney(oSOInfo.GetTotalAmt());
                                                IncomeAmt += oDSItem.PayAmt;
                                            }
                                            else
                                            {
                                                oDSItem.PayAmt = 0;
                                                IncomeAmt += oItem.PayAmt;
                                            }
                                            oDSInfo.ItemHash.Add(oDSItem, oDSItem);
                                            ARAmt += oDSItem.PayAmt;

                                        }
                                        else if (oSOInfo.Status == (int)AppEnum.SOStatus.PartlyReturn)
                                        {
                                            DSItemInfo oDSItem = new DSItemInfo();
                                            oDSItem.ItemID = oItem.ItemID;
                                            oDSItem.ItemType = oItem.ItemType;
                                            oDSItem.PayType = oItem.PayType;
                                            oDSItem.DLSysNo = oItem.DLSysNo;

                                            decimal returnAmt = 0;
                                            SOInfo oSOInfo2 = SaleManager.GetInstance().LoadSO(oSOInfo.SysNo);
                                            foreach (SOItemInfo oSOItem in oSOInfo2.ItemHash.Values)
                                            {
                                                if (oSOItem.ReturnQty != AppConst.IntNull)
                                                {
                                                    returnAmt += oSOItem.ReturnQty * oSOItem.Price;
                                                }
                                            }
                                            if (oSOInfo.PayTypeSysNo == 1) //货到付款去零头
                                            {
                                                oDSItem.PayAmt = Util.TruncMoney(oSOInfo.GetTotalAmt() - returnAmt);
                                                IncomeAmt += Util.TruncMoney(oSOInfo.GetTotalAmt() - returnAmt);

                                            }
                                            else
                                            {
                                                oDSItem.PayAmt = 0;
                                                IncomeAmt += oItem.PayAmt;

                                            }
                                            oDSInfo.ItemHash.Add(oDSItem, oDSItem);
                                            ARAmt += oDSItem.PayAmt;
                                        }
                                    }
                                }
                                else if (oItem.ItemType == (int)AppEnum.DLItemType.RMARequest)
                                {
                                    int FreightUserSysNo = RMARequestManager.GetInstance().GetFreightUserSysNofromID(oItem.ItemID);
                                    int RequestDLSysNo = RMARequestManager.GetInstance().GetDLSysNofromID(oItem.ItemID);

                                    if (FreightUserSysNo == AppConst.IntNull)
                                    {
                                        NullFreightItem += oItem.ItemID + ",";

                                    }
                                    else if (NullFreightItem == "" && FreightUserSysNo == oDLInfo.FreightUserSysNo && RequestDLSysNo == oDLInfo.SysNo)
                                    {
                                        DSItemInfo oDSItem = new DSItemInfo();
                                        oDSItem.ItemID = oItem.ItemID;
                                        oDSItem.ItemType = oItem.ItemType;
                                        oDSItem.PayType = oItem.PayType;
                                        oDSItem.PayAmt = oItem.PayAmt;
                                        oDSItem.DLSysNo = oItem.DLSysNo;

                                        oDSInfo.ItemHash.Add(oDSItem, oDSItem);
                                        ARAmt += oItem.PayAmt;
                                        IncomeAmt += oItem.PayAmt;

                                    }
                                }
                                else if (oItem.ItemType == (int)AppEnum.DLItemType.RMAOutbound)
                                {
                                    int FreightUserSysNo = RMAOutBoundManager.GetInstance().GetFreightUserSysNoByID(oItem.ItemID);
                                    int OutboundDLSysNo = RMAOutBoundManager.GetInstance().GetDLSysNoSysNoByID(oItem.ItemID);

                                    if (FreightUserSysNo == AppConst.IntNull)
                                    {
                                        NullFreightItem += oItem.ItemID + ",";
                                    }
                                    else if (NullFreightItem == "" && FreightUserSysNo == oDLInfo.FreightUserSysNo && OutboundDLSysNo == oDLInfo.SysNo)
                                    {
                                        DSItemInfo oDSItem = new DSItemInfo();
                                        oDSItem.ItemID = oItem.ItemID;
                                        oDSItem.ItemType = oItem.ItemType;
                                        oDSItem.PayType = oItem.PayType;
                                        oDSItem.DLSysNo = oItem.DLSysNo;

                                        oDSItem.PayAmt = 0;
                                        oDSInfo.ItemHash.Add(oDSItem, oDSItem);
                                        ARAmt += 0;
                                        IncomeAmt += 0;

                                    }
                                }
                                else if (oItem.ItemType == (int)AppEnum.DLItemType.RMARevert)
                                {
                                    int FreightUserSysNo = RMARevertManager.GetInstance().GetFreightUserSysNofromID(oItem.ItemID);
                                    int RevertDLSysNo = RMARevertManager.GetInstance().GetDLSysNofromID(oItem.ItemID);

                                    if (FreightUserSysNo == AppConst.IntNull)
                                    {
                                        NullFreightItem += oItem.ItemID + ",";
                                    }

                                    else if (NullFreightItem == "" && FreightUserSysNo == oDLInfo.FreightUserSysNo && RevertDLSysNo == oDLInfo.SysNo)
                                    {
                                        DSItemInfo oDSItem = new DSItemInfo();
                                        oDSItem.ItemID = oItem.ItemID;
                                        oDSItem.ItemType = oItem.ItemType;
                                        oDSItem.PayType = oItem.PayType;
                                        oDSItem.DLSysNo = oItem.DLSysNo;

                                        oDSItem.PayAmt = 0;
                                        oDSInfo.ItemHash.Add(oDSItem, oDSItem);
                                        ARAmt += 0;
                                        IncomeAmt += 0;
                                    }

                                }
                                else if (oItem.ItemType == (int)AppEnum.DLItemType.RMASendAccessory)
                                {
                                    int FreightUserSysNo = RMASendAccessoryManager.GetInstance().GetFreightUserSysNoByID(oItem.ItemID);
                                    int SendAccessoryDLSysNo = RMASendAccessoryManager.GetInstance().GetDLSysNoByID(oItem.ItemID);

                                    if (FreightUserSysNo == AppConst.IntNull)
                                    {
                                        NullFreightItem += oItem.ItemID + ",";
                                    }
                                    else if (NullFreightItem == "" && FreightUserSysNo == oDLInfo.FreightUserSysNo && SendAccessoryDLSysNo == oDLInfo.SysNo)
                                    {
                                        DSItemInfo oDSItem = new DSItemInfo();
                                        oDSItem.ItemID = oItem.ItemID;
                                        oDSItem.ItemType = oItem.ItemType;
                                        oDSItem.PayType = oItem.PayType;
                                        oDSItem.DLSysNo = oItem.DLSysNo;
                                        oDSItem.PayAmt = 0;
                                        oDSInfo.ItemHash.Add(oDSItem, oDSItem);
                                        ARAmt += 0;
                                        IncomeAmt += 0;
                                    }
                                }

                                else if (oItem.ItemType == (int)AppEnum.DLItemType.StShift)
                                {
                                    ShiftInfo oInfo = ShiftManager.GetInstance().Load(Int32.Parse(oItem.ItemID.Substring(2)));
                                    int FreightUserSysNo = oInfo.FreightUserSysNo;
                                    int stShiftDLSysNo = oInfo.DLSysNo;
                                    if (FreightUserSysNo == AppConst.IntNull)
                                    {
                                        NullFreightItem += oItem.ItemID + ",";
                                    }

                                    else if (NullFreightItem == "" && FreightUserSysNo == oDLInfo.FreightUserSysNo && stShiftDLSysNo == oDLInfo.SysNo)
                                    {
                                        DSItemInfo oDSItem = new DSItemInfo();
                                        oDSItem.ItemID = oItem.ItemID;
                                        oDSItem.ItemType = oItem.ItemType;
                                        oDSItem.PayType = oItem.PayType;
                                        oDSItem.DLSysNo = oItem.DLSysNo;
                                        oDSItem.PayAmt = 0;
                                        oDSInfo.ItemHash.Add(oDSItem, oDSItem);
                                        ARAmt += 0;
                                        IncomeAmt += 0;
                                    }
                                }

                            }

                        }
                        oDLInfo.Status = (int)AppEnum.DLStatus.DSConfirmed;
                        DLManager.GetInstance().UpdateDLMaster(oDLInfo);

                    }
                    if (NullFreightItem != "")
                    {
                        throw new BizException(NullFreightItem + "没有配送人，请重新设置！");
                    }

                    if (oDSInfo.ItemHash.Count <= 0)
                    {
                        throw new BizException("配送单中没有配送成功的单据");
                    }
                    oDSInfo.CreateTime = DateTime.Now;
                    oDSInfo.CreateUserSysNo = usersysno;
                    oDSInfo.FreightUserSysNo = FreightUser;
                    oDSInfo.ARAmt = ARAmt;
                    oDSInfo.IncomeAmt = IncomeAmt;
                    DSManager.GetInstance().CreateDS(oDSInfo);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新配送结算单的凭证
        /// </summary>
        /// <param name="dsSysNoList"></param>
        /// <param name="voucherID"></param>
        /// <param name="voucherUserSysNo"></param>
        /// <param name="isConfirm">是否同时确认收款</param>
        public void UpdateDSVoucherID(string dsSysNoList, string voucherID, int voucherUserSysNo, bool isConfirm)
        {
            try
            {
                TransactionOptions options = new TransactionOptions();
                options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                options.Timeout = TransactionManager.DefaultTimeout;

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    string sql = "update ds_master set voucherID=" + Util.ToSqlString(voucherID) + ",voucherUserSysno=" + voucherUserSysNo + ",voucherTime=" + Util.ToSqlString(DateTime.Now.ToString());
                    if (isConfirm)
                    {
                        sql += ",settlementUserSysNo=" + voucherUserSysNo + ",settlementTime=" + Util.ToSqlString(DateTime.Now.ToString());
                        sql += ",status=" + (int)AppEnum.DSStatus.AccountConfirmed;
                    }

                    sql += " where sysno in(" + dsSysNoList + ")";
                    SqlHelper.ExecuteNonQuery(sql);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 审核结算单
        /// </summary>
        /// <param name="oDSInfo"></param>

        public void AuditDS(DSInfo oDSInfo)
        {
            try
            {
                TransactionOptions options = new TransactionOptions();
                options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                options.Timeout = TransactionManager.DefaultTimeout;

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    string sql = "select * from DS_Item where DSSysNo=" + oDSInfo.SysNo + " and IsPos=" + (int)AppEnum.YNStatus.Yes + " and (PosNo is null or PosDate is null)";
                    DataSet ds = SqlHelper.ExecuteDataSet(sql);
                    if (Util.HasMoreRow(ds))
                        throw new BizException("刷卡终端号及刷卡日期没有填写完整！");
                    UpdateDSMaster(oDSInfo);
                    decimal Remittance = 0;
                    decimal PosGoods = 0;
                    decimal Cheque = 0;
                    decimal PosGoodsCash = 0;
                    if (oDSInfo.Remittance != AppConst.DecimalNull)
                        Remittance = oDSInfo.Remittance;
                    if (oDSInfo.PosGoods != AppConst.DecimalNull)
                        PosGoods = oDSInfo.PosGoods;
                    if (oDSInfo.Cheque != AppConst.DecimalNull)
                        Cheque = oDSInfo.Cheque;
                    if (oDSInfo.PosGoodsCash != AppConst.DecimalNull)
                        PosGoodsCash = oDSInfo.PosGoodsCash;
                    decimal Arrearage = Remittance + PosGoods - PosGoodsCash + Cheque - oDSInfo.ARAmt;
                    if (Arrearage != 0)
                    {
                        DeliveryManArrearageLogInfo oDMArrearageLog = new DeliveryManArrearageLogInfo();
                        oDMArrearageLog.UserSysNo = oDSInfo.FreightUserSysNo;
                        oDMArrearageLog.DSSysNo = oDSInfo.SysNo;
                        oDMArrearageLog.Arrearage = Arrearage;
                        oDMArrearageLog.ArrearageChange = Arrearage;
                        oDMArrearageLog.CreateTime = DateTime.Now;
                        oDMArrearageLog.CreateUserSysNo = oDSInfo.AuditUserSysNo;
                        if (Arrearage < 0)
                            oDMArrearageLog.ArrearageLogType = (int)AppEnum.DeliveryManArrearageLogType.PayMentOfGoodsLack;
                        else if (Arrearage > 0)
                            oDMArrearageLog.ArrearageLogType = (int)AppEnum.DeliveryManArrearageLogType.PayMentofGoodsExcessive;
                        DeliveryManager.GetInstance().InsertDeliveryManArrearageLog(oDMArrearageLog);
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///取消审核结算单
        /// </summary>
        /// <param name="oDSInfo"></param>

        public void CancelAuditDS(DSInfo oDSInfo)
        {
            try
            {
                TransactionOptions options = new TransactionOptions();
                options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                options.Timeout = TransactionManager.DefaultTimeout;

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    string sql = "delete DeliveryMan_ArrearageLog where DSSysNo=" + oDSInfo.SysNo;
                    SqlHelper.ExecuteNonQuery(sql);

                    UpdateDSMaster(oDSInfo);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AbandonDS(DSInfo dsInfo)
        {
            try
            {
                TransactionOptions options = new TransactionOptions();
                options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                options.Timeout = TransactionManager.DefaultTimeout;

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    //更新DS状态
                    DSManager.GetInstance().UpdateDSMaster(dsInfo);

                    //更新DL状态
                    string sql = @"update DL_Master set Status =" + (int)AppEnum.DLStatus.StockConfirmed + " where DL_Master.sysno in (select distinct dlsysno from ds_item where dssysno=" + dsInfo.SysNo + ")";
                    SqlHelper.ExecuteNonQuery(sql);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                dsInfo.SysNo = AppConst.IntNull;
                throw ex;
            }
        }

        public DataSet GetDSDs(int dlsysno)
        {
            string sql = @"select * from ds_master where dlsysno=" + dlsysno + "and status<>" + (int)AppEnum.DSStatus.Abandon;
            return SqlHelper.ExecuteDataSet(sql);

        }

        public DataSet GetDSDs(Hashtable paramHash)
        {
            string sql = @" select ds.*,su.username as createusername,su1.username as abandonusername,su2.username as FreightUserName
                            from
								ds_master ds(nolock) 
                                left join sys_user su on su.sysno=ds.CreateUserSysNo
                                left join sys_user su1 on su1.sysno=ds.AbandonUserSysNo
                                left join sys_user su2 on ds.FreightUserSysNo=su2.sysno
							where 1=1 ";

            if (paramHash != null && paramHash.Count != 0)
            {
                StringBuilder sb = new StringBuilder(100);
                foreach (string key in paramHash.Keys)
                {
                    object item = paramHash[key];

                    if (key == "SysNo")
                    {
                        sb.Append(" and ");
                        sb.Append(" ds.sysno = ").Append(item.ToString());
                    }
                    else if (key == "Status")
                    {
                        sb.Append(" and ");
                        sb.Append(" ds.status = ").Append(item.ToString());
                    }
                    else if (key == "RemittanceDateFrom")
                    {
                        sb.Append(" and ");
                        sb.Append(" ds.RemittanceDate>=").Append(item.ToString());
                    }
                    else if (key == "RemittanceDateTo")
                    {
                        sb.Append(" and ");
                        sb.Append(" ds.RemittanceDate<=").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "DLSysNo")
                    {
                        sb.Append(" and ");
                        sb.Append(" exists (select * from ds_item where ds_item.DLSysNo=").Append(item.ToString()).Append(" and ds_item.dssysno=ds.sysno)");
                    }
                    else if (key == "PosNo")
                    {
                        sb.Append(" and ");
                        sb.Append(" exists (select * from ds_item where ds_item.PosNo=").Append(item.ToString()).Append(" and ds_item.dssysno=ds.sysno)");
                    }
                    else if (key == "PosDateFrom")
                    {
                        sb.Append(" and ");
                        sb.Append(" exists (select * from ds_item where ds_item.PosDate>=").Append(item.ToString()).Append(" and ds_item.dssysno=ds.sysno)");
                    }
                    else if (key == "PosDateTo")
                    {
                        sb.Append(" and ");
                        sb.Append(" exists (select * from ds_item where ds_item.PosDate<=").Append(Util.ToSqlEndDate(item.ToString())).Append(" and ds_item.dssysno=ds.sysno)");
                    }
                    else if (key == "DateFrom")
                    {
                        sb.Append(" and ");
                        sb.Append("ds.createtime >= ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "DateTo")
                    {
                        sb.Append(" and ");
                        sb.Append("ds.createtime <= ").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "FreightUserSysNo")
                    {
                        sb.Append(" and ");
                        sb.Append(" ds.FreightUserSysNo = ").Append(item.ToString());
                    }
                    else if (key == "CreateUserSysNo")
                    {
                        sb.Append(" and ");
                        sb.Append(" ds.CreateUserSysNo= ").Append(item.ToString());
                    }
                    else if (key == "BranchSysNo")
                    {
                        sb.Append(" and ");
                        sb.Append(" su2.BranchSysNo= ").Append(item.ToString());
                    }
                    else if (key == "IsReceiveVoucher")
                    {
                        sb.Append(" and ");
                        sb.Append(" isnull(ds.IsReceiveVoucher," + (int)AppEnum.YNStatus.No + ")=").Append(item.ToString());
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
            sql += " order by ds.sysno desc";

            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetDSItemDs(int dsSysNo, int status)
        {
            string sql = "select * from ds_item where dssysno=" + dsSysNo;
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

        public DSInfo LoadMaster(int sysno)
        {
            string sql = "select * from DS_Master where sysno =" + sysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            DSInfo oInfo = new DSInfo();
            if (Util.HasMoreRow(ds))
            {
                map(oInfo, ds.Tables[0].Rows[0]);
                return oInfo;
            }
            else
                return null;

        }

        public DataSet GetSOFreightMenDs(Hashtable paramHash)
        {
            string sql = @"select sys_user.username as freightusername,paytype.IsPayWhenRecv,so_master.receivename,receiveaddress, receivephone, receivecellphone,
                           area.districtname,area.localcode,so_master.status AS SOStatus,SO_MASter.sysno as sysno, paytypename,so_master.PayTypeSysNo as PayTypeSysNo,
								cashpay+premiumamt+shipprice+payprice-freeshipfeepay-DiscountAmt as totalcash,so_master.PosFee,SO_MASter.IsLarge as IsLarge,DS_Item.IsPos as IsPos
                          from DS_Item,
                               DS_Master,
                               so_master,
                               area,
                               sys_user,
                               paytype
                               where 1=1 and  @dsmstatus and @dsitemstatus 
                               and DS_Item.ItemID=so_master.soid
                               and DS_Master.FreightUserSysNo=sys_user.sysno
                               and DS_Item.PayType=paytype.sysno
                               and DS_Master.sysno=DS_Item.dssysno
                               and so_master.receiveareasysno = area.sysno";

            string sql1 = " order by receiveareasysno,so_master.sysno ";

            sql = sql.Replace("@dsmstatus", " DS_Master.status <>" + (int)AppEnum.DSStatus.Abandon);  //必须是非作废的配送单
            sql = sql.Replace("@dsitemstatus", " DS_Item.status =" + (int)AppEnum.BiStatus.Valid);


            if (paramHash != null && paramHash.Count != 0)
            {
                StringBuilder sb = new StringBuilder(100);
                foreach (string key in paramHash.Keys)
                {

                    object item = paramHash[key];

                    if (key == "DSSysNo")
                    {
                        sb.Append(" and ");
                        sb.Append("DS_Item.DSSysNo = ").Append(item.ToString());
                    }

                    else if (key == "orderby")
                    {
                        sql1 = " order by DS_Item.sysno desc ";
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
                           area.localcode,sys_user.username as freightusername,rma_request.Contact,rma_request.Phone,rma_request.IsLarge as IsLarge,DS_Item.IsPos as IsPos,DS_Item.PosFee as PosFee
                          from rma_request,area ,sys_user,DS_Item,DS_Master
                          where  AreaSysNo=area.sysno  and @dsmstatus and @dsitemstatus
                                and DS_Item.ItemID=rma_request.RequestID
                                and DS_Master.sysno=DS_Item.dssysno
                                and DS_Master.FreightUserSysNo=sys_user.sysno ";


            string sql1 = "";

            sql = sql.Replace("@dsmstatus", " DS_Master.status <>" + (int)AppEnum.DSStatus.Abandon);  //必须是非作废的配送单
            sql = sql.Replace("@dsitemstatus", " DS_Item.status =" + (int)AppEnum.BiStatus.Valid);


            if (paramHash != null && paramHash.Count != 0)
            {
                StringBuilder sb = new StringBuilder(100);
                foreach (string key in paramHash.Keys)
                {

                    object item = paramHash[key];

                    if (key == "DSSysNo")
                    {
                        sb.Append(" and ");
                        sb.Append("DS_Item.DSSysNo = ").Append(item.ToString());
                    }

                    else if (key == "orderby")
                    {
                        sql1 = " order by DS_Item.sysno desc ";
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
                           sys_user.username as freightusername,rma_revert.Contact,rma_revert.Phone,rma_revert.IsLarge as IsLarge,DS_Item.IsPos as IsPos
                          from rma_revert,area ,sys_user,DS_Item,DS_Master
                          where rma_revert.status=1 and AddressAreaSysNo=area.sysno and @dsmstatus and @dsitemstatus
                          and DS_Item.ItemID=rma_revert.RevertID
                          and DS_Master.sysno=DS_Item.dssysno                          
                          and DS_Master.FreightUserSysNo=sys_user.sysno ";

            string sql1 = "";

            sql = sql.Replace("@dsmstatus", " DS_Master.status <>" + (int)AppEnum.DSStatus.Abandon);  //必须是非作废的配送单
            sql = sql.Replace("@dsitemstatus", " DS_Item.status =" + (int)AppEnum.BiStatus.Valid);


            if (paramHash != null && paramHash.Count != 0)
            {
                StringBuilder sb = new StringBuilder(100);
                foreach (string key in paramHash.Keys)
                {

                    object item = paramHash[key];

                    if (key == "DSSysNo")
                    {
                        sb.Append(" and ");
                        sb.Append("DS_Item.DSSysNo = ").Append(item.ToString());
                    }

                    else if (key == "orderby")
                    {
                        sql1 = " order by DS_Item.sysno desc ";
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
                           sys_user.username as freightusername,RMA_OutBound.IsLarge as IsLarge,DS_Item.IsPos as IsPos
                          from RMA_OutBound,area ,sys_user,DS_Item,DS_Master
                          where RMA_OutBound.status=1 and AreaSysNo=area.sysno and @dsmstatus and @dsitemstatus
                          and DS_Item.ItemID=RMA_OutBound.OutBoundID
                          and DS_Master.sysno=DS_Item.dssysno
                          and DS_Master.FreightUserSysNo=sys_user.sysno ";


            string sql1 = "";
            sql = sql.Replace("@dsmstatus", " DS_Master.status <>" + (int)AppEnum.DSStatus.Abandon);  //必须是非作废的配送单
            sql = sql.Replace("@dsitemstatus", " DS_Item.status =" + (int)AppEnum.BiStatus.Valid);


            if (paramHash != null && paramHash.Count != 0)
            {
                StringBuilder sb = new StringBuilder(100);
                foreach (string key in paramHash.Keys)
                {

                    object item = paramHash[key];

                    if (key == "DSSysNo")
                    {
                        sb.Append(" and ");
                        sb.Append("DS_Item.DSSysNo = ").Append(item.ToString());
                    }

                    else if (key == "orderby")
                    {
                        sql1 = " order by DS_Item.sysno desc ";
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
                            area.DistrictName,area.localcode,sys_user.username as freightusername,RMA_SendAccessory.IsLarge as IsLarge,DS_Item.IsPos as IsPos
                          from RMA_SendAccessory,area ,sys_user,DS_Item,DS_Master
                          where AreaSysNo=area.sysno and @RMASendAccessoryStatus  and @dsmstatus and @dsitemstatus
                          and DS_Item.ItemID=RMA_SendAccessory.SendAccessoryID
                          and DS_Master.FreightUserSysNo=sys_user.sysno 
                          and DS_Master.sysno=DS_Item.dssysno";


            string sql1 = "";
            sql = sql.Replace("@dsmstatus", " DS_Master.status <>" + (int)AppEnum.DSStatus.Abandon);  //必须是非作废的配送单
            sql = sql.Replace("@dsitemstatus", " DS_Item.status =" + (int)AppEnum.BiStatus.Valid);
            sql = sql.Replace("@RMASendAccessoryStatus", "RMA_SendAccessory.status=" + (int)AppEnum.RMASendAccessoryStatus.Sent);


            if (paramHash != null && paramHash.Count != 0)
            {
                StringBuilder sb = new StringBuilder(100);
                foreach (string key in paramHash.Keys)
                {

                    object item = paramHash[key];

                    if (key == "DSSysNo")
                    {
                        sb.Append(" and ");
                        sb.Append("DS_Item.DsSysNo = ").Append(item.ToString());
                    }

                    else if (key == "orderby")
                    {
                        sql1 = " order by DS_Item.sysno desc ";
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
                          from St_Shift,sys_user,DS_Item,DS_Master,Stock
                          where 1=1 and @StShiftStatus and @dsitemstatus
                          and DS_Item.ItemID=St_Shift.ShiftID
                          and St_Shift.StockSysNoB=stock.sysno
                          and DS_Master.FreightUserSysNo=sys_user.sysno 
                          and DS_Master.sysno=DS_Item.dlsysno";


            string sql1 = "";
            //sql = sql.Replace("@dsmstatus", " DS_Master.status <>" + (int)AppEnum.DLStatus.Abandon);  //必须是非作废的结算单
            sql = sql.Replace("@dsitemstatus", " DS_Item.status =" + (int)AppEnum.BiStatus.Valid);
            sql = sql.Replace("@StShiftStatus", "St_Shift.status>=" + (int)AppEnum.ShiftStatus.OutStock);


            if (paramHash != null && paramHash.Count != 0)
            {
                StringBuilder sb = new StringBuilder(100);
                foreach (string key in paramHash.Keys)
                {

                    object item = paramHash[key];

                    if (key == "DSSysNo")
                    {
                        sb.Append(" and ");
                        sb.Append("DS_Item.DSSysNo = ").Append(item.ToString());
                    }

                    else if (key == "orderby")
                    {
                        sql1 = " order by DS_Item.sysno desc ";
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
        public decimal GetUsedCredit(int usersysno)
        {
            decimal userdCredit1 = 0;
            decimal userdCredit2 = 0;
            decimal userdCredit = 0;
            //已生成结算单，结算单未审核
            string sql = @"select sum(IncomeAmt) from DS_Master ds where ds.status=" + (int)AppEnum.DSStatus.Origin + " and ds.FreightUserSysNo=" + usersysno;
            DataSet ds1 = SqlHelper.ExecuteDataSet(sql);

            if (Util.HasMoreRow(ds1) && ds1.Tables[0].Rows[0][0].ToString() != AppConst.StringNull)
                userdCredit1 = decimal.Parse(ds1.Tables[0].Rows[0][0].ToString());

            //已确认发货的配送单,未生成结算单
            string sql1 = @"select sum(HasPaidAmt+CODAmt) from dl_master where sysno in (
                            select distinct dl.sysno
                            from dl_master dl
                            inner join DL_Item dli on dl.sysno=dli.dlsysno
                            inner join so_master sm on sm.soid=dli.itemid 
                            where dl.status=" + (int)AppEnum.DLStatus.StockConfirmed + " and dli.ItemType=" + (int)AppEnum.DLItemType.SaleOrder +
                             " and sm.FreightUserSysNo=dl.FreightUserSysNo and dl.sysno not in (select dlsysno from ds_master ds where ds.status <>" + (int)AppEnum.DSStatus.Abandon + " and ds.FreightUserSysNo=dl.FreightUserSysNo) and dl.FreightUserSysNo=" + usersysno + ")";

            DataSet ds2 = SqlHelper.ExecuteDataSet(sql1);
            if (Util.HasMoreRow(ds2) && ds2.Tables[0].Rows[0][0].ToString() != AppConst.StringNull)
                userdCredit2 = decimal.Parse(ds2.Tables[0].Rows[0][0].ToString());

            userdCredit = userdCredit1 + userdCredit2;
            return userdCredit;

        }

        public DataSet GetDSCreateUserList(DateTime dtCreateFrom, int CreateUserStatus)
        {
            string sql = "select distinct su.sysno as createusersysno,su.username as createusername from sys_user su inner join DS_Master ds on su.sysno=ds.CreateUserSysNo where 1=1";
            if (dtCreateFrom != AppConst.DateTimeNull)
            {
                sql += " and ds.createtime >= " + Util.ToSqlString(dtCreateFrom.ToString(AppConst.DateFormat));
            }
            if (CreateUserStatus != AppConst.IntNull)
            {
                sql += " and su.status=" + Util.ToSqlString(CreateUserStatus.ToString());
            }
            sql += " order by su.username";
            return SqlHelper.ExecuteDataSet(sql);
        }

        public void UpdatePosInfo(string itemsysnoList, string PosNo, string PosDate)
        {
            string sql = "update DS_Item set PosNo=" + Util.ToSqlString(PosNo) + ",PosDate=" + Util.ToSqlString(PosDate) + " where sysno in (" + itemsysnoList + ") and IsPos=" + (int)AppEnum.YNStatus.Yes;
            SqlHelper.ExecuteNonQuery(sql);
        }

        public void DeletePosInfo(string itemsysnoList, int DSSysNo)
        {
            try
            {
                TransactionOptions options = new TransactionOptions();
                options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                options.Timeout = TransactionManager.DefaultTimeout;

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    string sql = "update DS_Item set IsPos=null,PosNo=null,PosDate=null where sysno in (" + itemsysnoList + ") and IsPos=" + (int)AppEnum.YNStatus.Yes;
                    SqlHelper.ExecuteNonQuery(sql);

                    string sql1 = " update DS_Master set PosGoods= (select sum(PayAmt) from DS_Item where ispos=" + (int)AppEnum.YNStatus.Yes + " and dssysno=" + DSSysNo + ") where sysno=" + DSSysNo;
                    SqlHelper.ExecuteNonQuery(sql1);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdatePaperVoucher(string dlsysnoList, int IsReceiveVoucher)
        {
            try
            {
                TransactionOptions options = new TransactionOptions();
                options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                options.Timeout = TransactionManager.DefaultTimeout;

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    string sql = " update DS_Master set IsReceiveVoucher=" + IsReceiveVoucher + " where sysno in (" + dlsysnoList + ")";
                    SqlHelper.ExecuteNonQuery(sql);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
