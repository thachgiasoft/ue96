using System;
using System.Data;
using System.Collections;

using Icson.Utils;
using System.Transactions;
using Icson.Objects;
using Icson.Objects.Basic;
using Icson.Objects.RMA;

using Icson.DBAccess;
using Icson.DBAccess.RMA;

using Icson.BLL.Sale;
using Icson.BLL.Basic;
using Icson.BLL.Stock;


namespace Icson.BLL.RMA
{
    /// <summary>
    /// Summary description for RMARegisterManager.
    /// </summary>
    public class RMARegisterManager
    {
        private RMARegisterManager()
        {

        }
        private static RMARegisterManager _instance;
        public static RMARegisterManager GetInstance()
        {
            if (_instance == null)
                _instance = new RMARegisterManager();
            return _instance;
        }

        public void InsertRegister(RMARegisterInfo oInfo)
        {
            oInfo.IsWithin7Days = (int)AppEnum.YNStatus.No;
            oInfo.IsRecommendRefund = (int)AppEnum.RecommendRefund.No;
            oInfo.NewProductStatus = (int)AppEnum.NewProductStatus.Origin;
            new RMARegisterDac().Insert(oInfo);
        }



        public RMARegisterInfo Load(int sysno)
        {
            RMARegisterInfo oRegister = new RMARegisterInfo();
            string sql = "select * from RMA_Register (NOLOCK) where sysno = " + sysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                map(oRegister, ds.Tables[0].Rows[0]);
            }
            return oRegister;
        }


        public void UpdateRegister(Hashtable paramHash)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                string alarm = AppConst.StringNull;
                RMARegisterInfo oRegister = new RMARegisterInfo();
                DataRow dr = this.GetRegisterRow(Util.TrimIntNull(paramHash["SysNo"]));
                this.map(oRegister, dr);

                //如果是作废订单，需要判断是否已退款、已退货、已发货、已送修。
                if (paramHash.ContainsKey("Status"))
                {
                    if (Util.TrimIntNull(paramHash["Status"]) == (int)AppEnum.RMARequestStatus.Abandon)
                    {
                        //OutBound不涉及到库存以及财务的操作，是否已经存在处理中或者处理完毕的OutBound，对作废没有影响
                        //if(oRegister.OutBoundStatus==(int)AppEnum.RMAOutBoundStatus.PartlyResponsed || oRegister.OutBoundStatus == (int)AppEnum.RMAOutBoundStatus.Responsed || oRegister.OutBoundStatus == (int)AppEnum.RMAOutBoundStatus.SendAlready)
                        //	alarm += "RegisterSysNo：" + paramHash["sysno"].ToString() + "存在处理中的OutBound送修单！" + "<br>";
                        if (oRegister.RefundStatus == (int)AppEnum.RMARefundStatus.Refunded)
                            alarm += "RegisterSysNo：" + paramHash["SysNo"].ToString() + "已经进行了Refund退款操作！请先取消退款，再作废该RMA单！" + "<br>";
                        if (oRegister.ReturnStatus == (int)AppEnum.RMAReturnStatus.Returned)
                            alarm += "RegisterSysNo：" + paramHash["SysNo"].ToString() + "已经进行了Return退货入库操作！请先取消退货入库，再作废该RMA单！" + "<br>";
                        if (oRegister.RevertStatus == (int)AppEnum.RMARevertStatus.Reverted)
                            alarm += "RegisterSysNo：" + paramHash["SysNo"].ToString() + "已经进行了Revert发货出库操作！请先取消发货出库，再作废该RMA单！" + "<br>";
                        else
                        {
                            if (oRegister.RevertStatus != (int)AppEnum.RMARevertStatus.Abandon && oRegister.NewProductStatus != (int)AppEnum.NewProductStatus.Origin)
                                alarm += "RegisterSysNo：" + paramHash["SysNo"].ToString() + "设置WaitingRevert不是非调换，耗掉库存,请先取消WaitingRevert，再作废该RMA单！" + "<br>";
                        }
                    }
                    //if (Util.TrimIntNull(paramHash["Status"]) == (int)AppEnum.RMARequestStatus.Closed)
                    //{
                    //    if (oRegister.OwnBy != (int)AppEnum.RMAOwnBy.Origin || oRegister.Location != (int)AppEnum.RMALocation.Origin)
                    //        alarm += "RegisterSysNo：" + paramHash["SysNo"].ToString() + "只有在处理完成后才能Close！请确认是否发还货物给客户或者退货入库。";
                    //}
                }
                if (alarm != AppConst.StringNull)
                {
                    throw new BizException(alarm + "操作失败！");
                }
                new RMARegisterDac().Update(paramHash);
                scope.Complete();
            }
        }

        public void SetToCC(int registerSysNo)
        {
            Hashtable ht = new Hashtable(5);
            ht.Add("SysNo", registerSysNo);
            ht.Add("NextHandler", (int)AppEnum.RMANextHandler.CC);
            this.UpdateRegister(ht);
        }

        public void SetToRMA(int registerSysNo)
        {
            Hashtable ht = new Hashtable(5);
            ht.Add("SysNo", registerSysNo);
            ht.Add("NextHandler", (int)AppEnum.RMANextHandler.RMA);
            this.UpdateRegister(ht);
        }
        public void SetTo(int registerSysNo, int HandlerNext)
        {
            Hashtable ht = new Hashtable(5);
            ht.Add("SysNo", registerSysNo);
            ht.Add("NextHandler", HandlerNext);
            this.UpdateRegister(ht);
        }

        public void CloseRegister(int registerSysNo, int closeUserSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                string sql = "select top 1 sysno from rma_register (NOLOCK) where status = @0 and sysno = @1";
                sql = sql.Replace("@0", ((int)AppEnum.RMARequestStatus.Handling).ToString());
                sql = sql.Replace("@1", registerSysNo.ToString());

                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (!Util.HasMoreRow(ds))
                    throw new BizException("this record's status can't allow such operation");

                Hashtable ht = new Hashtable(5);
                ht.Add("SysNo", registerSysNo);
                ht.Add("Status", (int)AppEnum.RMARequestStatus.Closed);
                ht.Add("CloseUserSysNo", closeUserSysNo);
                ht.Add("CloseTime", DateTime.Now);

                this.UpdateRegister(ht);

                //查找本registerSysNo具有相同requestsysno的其他明细的状态。用来判断是否可以全部关闭。
                string sql2 = @"select b.requestsysno, c.status 
								from 
									rma_request_item as a (NOLOCK), rma_request_item as b (NOLOCK), rma_register as c (NOLOCK)
								where 
									a.requestsysno = b.requestsysno 
								and b.registersysno = c.sysno
								and a.registerSysNo =" + registerSysNo;

                DataSet ds2 = SqlHelper.ExecuteDataSet(sql2);
                if (Util.HasMoreRow(ds2))
                {
                    bool isAllClosed = true;
                    int requestSysNo = AppConst.IntNull;
                    foreach (DataRow dr2 in ds2.Tables[0].Rows)
                    {
                        if (requestSysNo == AppConst.IntNull)
                        {
                            requestSysNo = Util.TrimIntNull(dr2["requestSysNo"]);
                        }
                        if (Util.TrimIntNull(dr2["status"]) == (int)AppEnum.RMARequestStatus.Handling)
                        {
                            isAllClosed = false;
                            break;
                        }
                    }
                    if (isAllClosed)
                    {
                        Hashtable htRequest = new Hashtable(5);
                        htRequest.Add("SysNo", requestSysNo);
                        htRequest.Add("Status", (int)AppEnum.RMARequestStatus.Closed);
                        RMARequestManager.GetInstance().UpdateRequest(htRequest);
                    }
                }
                scope.Complete();
            }
        }

        public void CloseAllRegister(int returnSysNo, int closeUserSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                DataSet dsRegisterSysNo = RMAReturnManager.GetInstance().GetRegisterSysNo(returnSysNo);
                foreach (DataRow dr in dsRegisterSysNo.Tables[0].Rows)
                {
                    RMARegisterInfo oInfo = RMARegisterManager.GetInstance().Load(Util.TrimIntNull(dr["RegisterSysNo"]));

                    if (oInfo.RefundStatus == (int)AppEnum.RMARefundStatus.Abandon || oInfo.RefundStatus == (int)AppEnum.RMARefundStatus.Refunded || oInfo.RefundStatus == AppConst.IntNull)//退款状态为已作废或已退款或不需退款的才能关闭
                    {
                        RMARegisterManager.GetInstance().CloseRegister((int)dr["RegisterSysNo"], closeUserSysNo);
                    }
                }
                scope.Complete();
            }
        }
        public void ReOpenRegister(int registerSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                string sql = "select top 1 sysno from rma_register (NOLOCK) where status = @0 and sysno = @1";
                sql = sql.Replace("@0", ((int)AppEnum.RMARequestStatus.Closed).ToString());
                sql = sql.Replace("@1", registerSysNo.ToString());

                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (!Util.HasMoreRow(ds))
                    throw new BizException("this recored's status can't allow such operation");

                Hashtable ht = new Hashtable(5);
                ht.Add("SysNo", registerSysNo);
                ht.Add("Status", (int)AppEnum.RMARequestStatus.Handling);

                this.UpdateRegister(ht);

                string sql2 = @"select b.requestsysno, c.status 
								from 
									rma_request_item as a (NOLOCK), rma_request_item as b (NOLOCK), rma_register as c (NOLOCK)
								where 
									a.requestsysno = b.requestsysno 
								and b.registersysno = c.sysno
								and a.registerSysNo = " + registerSysNo;

                DataSet ds2 = SqlHelper.ExecuteDataSet(sql2);
                if (Util.HasMoreRow(ds2))
                {
                    bool isAllClosed = true;
                    int requestSysNo = AppConst.IntNull;
                    foreach (DataRow dr2 in ds2.Tables[0].Rows)
                    {
                        if (requestSysNo == AppConst.IntNull)
                        {
                            requestSysNo = Util.TrimIntNull(dr2["requestSysNo"]);
                        }
                        if (Util.TrimIntNull(dr2["status"]) == (int)AppEnum.RMARequestStatus.Handling)
                        {
                            isAllClosed = false;
                            break;
                        }
                    }
                    if (isAllClosed)
                    {
                        Hashtable htRequest = new Hashtable(5);
                        htRequest.Add("SysNo", requestSysNo);
                        htRequest.Add("Status", (int)AppEnum.RMARequestStatus.Closed);
                        RMARequestManager.GetInstance().UpdateRequest(htRequest);
                    }
                }

                scope.Complete();
            }
        }

        public void SetWaitingReturn(int registerSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                string sql = "select top 1 sysno,revertstatus,OutBoundStatus,CheckRepairResult from rma_register (NOLOCK) where (returnstatus is null or returnstatus =" + (int)AppEnum.RMAReturnStatus.Abandon + ")and status in ( "
                    + (int)AppEnum.RMARequestStatus.Handling + ") and sysno = " + registerSysNo;

                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (!Util.HasMoreRow(ds))
                    throw new BizException("this recored's return status or status can't allow such operation");

                DataRow dr = ds.Tables[0].Rows[0];

                if (Util.TrimIntNull(dr["CheckRepairResult"]) == AppConst.IntNull)
                {
                    throw new BizException("未检测，不能设置待入库！");
                }

                Hashtable ht = new Hashtable(5);
                ht.Add("SysNo", registerSysNo);
                ht.Add("ReturnStatus", (int)AppEnum.RMAReturnStatus.WaitingReturn);

                this.UpdateRegister(ht);
                scope.Complete();
            }
        }

        public void CancelWaitingReturn(int registerSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                string sql = @"select top 1 sysno from rma_register (NOLOCK) where returnstatus=@3 and status in (@0)
								and sysno not in
								(
									select registersysno from rma_return, rma_return_item
									where rma_return.sysno = rma_return_item.returnsysno and rma_return.status = @2
								)  and sysno = " + registerSysNo;

                sql = sql.Replace("@0", ((int)AppEnum.RMARequestStatus.Handling).ToString());
                sql = sql.Replace("@2", ((int)AppEnum.RMAReturnStatus.WaitingReturn).ToString());
                sql = sql.Replace("@3", ((int)AppEnum.RMAReturnStatus.WaitingReturn).ToString());


                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (!Util.HasMoreRow(ds))
                    throw new BizException("this recored's return status or status can't allow such operation");

                Hashtable ht = new Hashtable(5);
                ht.Add("SysNo", registerSysNo);
                ht.Add("ReturnStatus", AppConst.IntNull);

                this.UpdateRegister(ht);
                scope.Complete();
            }
        }

        public void SetWaitingRefund(int registerSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                string sql = "select top 1 sysno,revertstatus from rma_register (NOLOCK) where (refundstatus is null or refundstatus=" + (int)AppEnum.RMARefundStatus.Abandon + ") and status in ( "
                    + (int)AppEnum.RMARequestStatus.Handling + ") and sysno = " + registerSysNo;

                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (!Util.HasMoreRow(ds))
                    throw new BizException("this recored's refund status or status can't allow such operation");

                DataRow dr = ds.Tables[0].Rows[0];

                if (Util.TrimIntNull(dr["revertstatus"]) == (int)AppEnum.RMARevertStatus.WaitingRevert)
                {
                    throw new BizException("待发还状态，不能再设置待退款");
                }
                else if (Util.TrimIntNull(dr["revertstatus"]) == (int)AppEnum.RMARevertStatus.Reverted)
                {
                    throw new BizException("已发还状态，不能再设置待退款");
                }

                Hashtable ht = new Hashtable(5);
                ht.Add("SysNo", registerSysNo);
                ht.Add("RefundStatus", (int)AppEnum.RMARefundStatus.WaitingRefund);

                this.UpdateRegister(ht);
                scope.Complete();
            }
        }

        public void CancelWaitingRefund(int registerSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                string sql = @"select top 1 sysno from rma_register (NOLOCK) where refundstatus=@3 and status in (@0)
								and sysno not in
								(
									select registersysno from rma_refund (NOLOCK), rma_refund_item (NOLOCK)
									where rma_refund.sysno = rma_refund_item.refundsysno and rma_refund.status = @2
								) and sysno = " + registerSysNo;
                sql = sql.Replace("@0", ((int)AppEnum.RMARequestStatus.Handling).ToString());
                sql = sql.Replace("@2", ((int)AppEnum.RMARefundStatus.WaitingRefund).ToString());
                sql = sql.Replace("@3", ((int)AppEnum.RMARefundStatus.WaitingRefund).ToString());


                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (!Util.HasMoreRow(ds))
                    throw new BizException("this recored's refund status or status can't allow such operation");

                Hashtable ht = new Hashtable(5);
                ht.Add("SysNo", registerSysNo);
                ht.Add("RefundStatus", AppConst.IntNull);

                this.UpdateRegister(ht);
                scope.Complete();
            }
        }

        ///// <summary>
        ///// 退还二手商品时，要根据当前商品编号返回对应二手商品的编号。
        ///// 
        ///// </summary>
        ///// <param name="productSysNo"></param>
        ///// <returns></returns>
        //public int getSecondHandProductSysNo(int productSysNo)
        //{
        //    //validate ProductID
        //    string sql=AppConst.StringNull;
        //    string productSql = "select ProductID from product P (nolock) where SysNo = "+productSysNo+"";
        //    DataSet ds = SqlHelper.ExecuteDataSet(productSql);
        //    if (Util.HasMoreRow(ds))
        //    {
        //        if (ds.Tables[0].Rows[0]["ProductID"].ToString().ToUpper().EndsWith("R"))
        //        {
        //            //二手调二手时对于多R的产品先去掉R
        //            string initialProductID = RMAReturnManager.GetInstance().GetInitialProductID(ds.Tables[0].Rows[0]["ProductID"].ToString()+'R');
        //            sql = "select P.SysNo from product P (nolock) where P.ProductID='"+initialProductID+"'";
        //        }
        //        else
        //        {
        //            sql = "select P.SysNo from product P (nolock), (select ProductID from Product (nolock) where SysNo=" + productSysNo + ") N where P.ProductID=N.ProductID+'R'";
        //        }

        //    }

        //    //
        //    int newProductSysNo = -1;
        //    DataSet newds = SqlHelper.ExecuteDataSet(sql);
        //    if (newds.Tables[0].Rows.Count > 0)
        //    {
        //        newProductSysNo = (int)newds.Tables[0].Rows[0]["SysNo"];
        //    }
        //    else
        //    {
        //        throw new BizException("编号为" + productSysNo.ToString() + "的商品没有二手商品。");
        //    }
        //    return newProductSysNo;
        //}
        /// <summary>
        /// 用户购买的二手产品换新品时找到此二手品对应的新品的SysNo
        /// </summary>
        /// <param name="registerSysNo"></param>
        /// <param name="newProductStatusValue"></param>
        /// <param name="revertProductSysNo"></param>
        public int getNewForSecondProductSysNo(string ProductID, int ProductSysNo)
        {
            string initiProductID = RMAReturnManager.GetInstance().GetInitialProductID(ProductID);
            string sql = "select P.SysNo from product P where P.ProductID='" + initiProductID + "'";

            int newProductSysNo = -1;
            DataSet newds = SqlHelper.ExecuteDataSet(sql);
            if (newds.Tables[0].Rows.Count > 0)
            {
                newProductSysNo = (int)newds.Tables[0].Rows[0]["SysNo"];
            }
            else
            {
                throw new BizException("编号为" + ProductSysNo.ToString() + "的商品库存中没有新的商品。");
            }

            return newProductSysNo;
        }

        public void SetWaitingRevert(int registerSysNo, int newProductStatusValue, int revertProductSysNo, int revertStockSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                string sql = "select top 1 sysno , productsysno,outboundstatus,refundstatus,returnstatus,InspectionResultType,CheckRepairResult  from rma_register (NOLOCK) where revertstatus is null and status in ( "
                    + (int)AppEnum.RMARequestStatus.Handling + ") and sysno = " + registerSysNo;
                int newProductSysNo = -1;  //退还给用户的商品编号，如果是新品，就是原商品编号，如果是二手，需要通过一定关系获取。

                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (!Util.HasMoreRow(ds))
                    throw new BizException("this recored's revert status or status can't allow such operation");

                DataRow dr = ds.Tables[0].Rows[0];

                if (Util.TrimIntNull(dr["refundstatus"]) == (int)AppEnum.RMARefundStatus.WaitingRefund)
                {
                    throw new BizException("待退款状态，不能再设置待发还");
                }
                else if (Util.TrimIntNull(dr["refundstatus"]) == (int)AppEnum.RMARefundStatus.Refunded)
                {
                    throw new BizException("已退款状态，不能再设置待发还");
                }

                string InspectionResultType = Util.TrimNull(dr["InspectionResultType"]);

                if (InspectionResultType != "人为损坏，建议发还" && InspectionResultType != "产品无故障" && InspectionResultType != "产品过保，建议发还" && InspectionResultType != "非我方产品，建议发还")
                {
                    if (newProductStatusValue == (int)AppEnum.NewProductStatus.Origin)
                    {
                        if (Util.TrimIntNull(dr["OutBoundStatus"]) != (int)AppEnum.RMAOutBoundStatus.Responsed)
                        {
                            throw new BizException("送修未返还，不能创建非换货的发货单");
                        }
                        else if (Util.TrimIntNull(dr["CheckRepairResult"]) == AppConst.IntNull)
                        {
                            throw new BizException("送修返还未检测，不能发货！");
                        }
                        else if (Util.TrimIntNull(dr["CheckRepairResult"]) == (int)AppEnum.YNStatus.No)
                        {
                            throw new BizException("送修返还检测有故障，不能发货！");
                        }
                    }
                }
                if (InspectionResultType == "")
                {
                    throw new BizException("没有检测结果，不能发货！");
                }

                newProductSysNo = Util.TrimIntNull(dr["productsysno"]);  //默认是修复后返还原产品或者返还新品id

                if (newProductStatusValue != (int)AppEnum.NewProductStatus.Origin)
                {
                    //占用库存  //仅退还新品或者二手品或者调非当前Case产品时会占用对应商品的库存
                    if (revertProductSysNo != AppConst.IntNull) newProductSysNo = revertProductSysNo;  //在调新品的时候需要上面查询到的产品sysno。
                    InventoryManager.GetInstance().SetAvailableQty(revertStockSysNo, newProductSysNo, 1);
                }

                Hashtable ht = new Hashtable(5);
                ht.Add("SysNo", registerSysNo);
                ht.Add("RevertProductSysNo", newProductSysNo);
                if (newProductStatusValue == (int)AppEnum.NewProductStatus.OtherProduct)
                    ht.Add("RevertStatus", (int)AppEnum.RMARevertStatus.WaitingAudit);
                else
                    ht.Add("RevertStatus", (int)AppEnum.RMARevertStatus.WaitingRevert);

                ht.Add("NewProductStatus", newProductStatusValue);

                if (Util.TrimIntNull(revertStockSysNo) != AppConst.IntNull)
                    ht.Add("RevertStockSysNo", revertStockSysNo);

                this.UpdateRegister(ht);

                scope.Complete();
            }
        }

        public void SetWaitingShift(int registerSysNo, int SetShiftUserSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                RMARegisterInfo oRegister = RMARegisterManager.GetInstance().Load(registerSysNo);

                if (oRegister.ShiftStatus != AppConst.IntNull && oRegister.ShiftStatus != (int)AppEnum.ShiftStatus.Abandon)
                {
                    throw new BizException("已设置待移库，不能重复设置！");
                }
                else if (oRegister.RevertStatus == AppConst.IntNull || oRegister.RevertStatus == (int)AppEnum.RMARevertStatus.Abandon)
                {
                    throw new BizException("未设置待发还，不能设置待移库！");
                }
                else if (oRegister.RevertStatus == (int)AppEnum.RMARevertStatus.Reverted)
                {
                    throw new BizException("已发还，不能设置待移库！");
                }
                else if (oRegister.NewProductStatus == (int)AppEnum.NewProductStatus.Origin)
                {
                    throw new BizException("非换货，不能设置待移库！");
                }
                else if (oRegister.RevertProductSysNo == AppConst.IntNull)
                {
                    throw new BizException("发还的商品编号不能为空！");
                }


                Hashtable ht = new Hashtable(5);
                ht.Add("SysNo", registerSysNo);
                ht.Add("ShiftStatus", (int)AppEnum.ShiftStatus.RMAWaitingShift);
                ht.Add("SetShiftUserSysNo", SetShiftUserSysNo);
                ht.Add("SetShiftTime", DateTime.Now);

                this.UpdateRegister(ht);
                scope.Complete();
            }
        }

        public void CancelWaitingShift(int registerSysNo, int SetShiftUserSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                RMARegisterInfo oRegister = RMARegisterManager.GetInstance().Load(registerSysNo);

                //string sql = @"select * from RMA_Shift left join St_Shift on RMA_Shift.shiftssyno=St_Shift.sysno where RMA_Shift.RegisterSysNo=" + registerSysNo;

                //DataSet ds = SqlHelper.ExecuteDataSet(sql);
                Hashtable ht = new Hashtable(5);
                if (oRegister.ShiftStatus == (int)AppEnum.ShiftStatus.RMAWaitingShift)
                {
                    ht.Add("SysNo", registerSysNo);
                    ht.Add("ShiftStatus", AppConst.IntNull);
                    ht.Add("SetShiftUserSysNo", SetShiftUserSysNo);
                    ht.Add("SetShiftTime", DateTime.Now);
                }
                else
                {
                    throw new BizException("发货单或移库单不允许此操作！");
                }

                this.UpdateRegister(ht);
                scope.Complete();
            }
        }
        public void UpdateRevertNewProductStatus(int registerSysNo, int newProductStatusValue, int revertProductSysNo, int revertStockSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                string sql = "select top 1 sysno , productsysno,outboundstatus,refundstatus,returnstatus,InspectionResultType,CheckRepairResult,ShiftStatus  from rma_register (NOLOCK) where revertstatus=0 and status in ( "
                    + (int)AppEnum.RMARequestStatus.Handling + ") and sysno = " + registerSysNo;

                int newProductSysNo = -1;  //退还给用户的商品编号，如果是新品，就是原商品编号，如果是二手，需要通过一定关系获取。

                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (!Util.HasMoreRow(ds))
                    throw new BizException("this recored's revert status or status can't allow such operation");

                DataRow dr = ds.Tables[0].Rows[0];

                if (Util.TrimIntNull(dr["refundstatus"]) == (int)AppEnum.RMARefundStatus.WaitingRefund)
                {
                    throw new BizException("待退款状态，不能再设置待发还");
                }
                else if (Util.TrimIntNull(dr["refundstatus"]) == (int)AppEnum.RMARefundStatus.Refunded)
                {
                    throw new BizException("已退款状态，不能再设置待发还");
                }

                if (Util.TrimIntNull(dr["ShiftStatus"]) != AppConst.IntNull && Util.TrimIntNull(dr["ShiftStatus"]) != (int)AppEnum.ShiftStatus.Abandon && newProductStatusValue == (int)AppEnum.NewProductStatus.Origin)
                {
                    throw new BizException("已设置待移库，不能修改为非换货类型");
                }
                string InspectionResultType = Util.TrimNull(dr["InspectionResultType"]);

                if (InspectionResultType != "人为损坏，建议发还" && InspectionResultType != "产品无故障" && InspectionResultType != "产品过保，建议发还" && InspectionResultType != "非我方产品，建议发还")
                {
                    if (newProductStatusValue == (int)AppEnum.NewProductStatus.Origin)
                    {
                        if (Util.TrimIntNull(dr["OutBoundStatus"]) != (int)AppEnum.RMAOutBoundStatus.Responsed)
                        {
                            throw new BizException("送修未返还，不能创建非换货的发货单");
                        }
                        else if (Util.TrimIntNull(dr["CheckRepairResult"]) == AppConst.IntNull)
                        {
                            throw new BizException("送修返还未检测，不能设置非换货！");
                        }
                        else if (Util.TrimIntNull(dr["CheckRepairResult"]) == (int)AppEnum.YNStatus.No)
                        {
                            throw new BizException("送修返还检测有故障，不能设置非换货！");
                        }

                    }
                }

                newProductSysNo = Util.TrimIntNull(dr["productsysno"]);  //默认是修复后返还原产品或者返还新品id

                if (newProductStatusValue != (int)AppEnum.NewProductStatus.Origin)
                {
                    //占用库存  //仅退还新品或者二手品或者调非当前Case产品时会占用对应商品的库存
                    if (revertProductSysNo != AppConst.IntNull) newProductSysNo = revertProductSysNo;  //在调新品的时候需要上面查询到的产品sysno。
                    InventoryManager.GetInstance().SetAvailableQty(revertStockSysNo, newProductSysNo, 1);
                }

                Hashtable ht = new Hashtable(5);
                ht.Add("SysNo", registerSysNo);
                ht.Add("RevertProductSysNo", newProductSysNo);
                if (newProductStatusValue == (int)AppEnum.NewProductStatus.OtherProduct)
                    ht.Add("RevertStatus", (int)AppEnum.RMARevertStatus.WaitingAudit);
                else
                    ht.Add("RevertStatus", (int)AppEnum.RMARevertStatus.WaitingRevert);

                ht.Add("NewProductStatus", newProductStatusValue);

                if (Util.TrimIntNull(revertStockSysNo) != AppConst.IntNull)
                    ht.Add("RevertStockSysNo", revertStockSysNo);

                this.UpdateRegister(ht);

                scope.Complete();

            }
        }
        public void CancelWaitingRevert(int registerSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                int newProductStatusValue = (int)AppEnum.NewProductStatus.Origin;
                int newProductSysNo = -1;
                string sql = @"select top 1 sysno , productsysno ,ShiftStatus, newproductstatus,revertproductsysno,RevertStockSysNo from rma_register (NOLOCK) where revertstatus in (@3,@4) and status in (@0)
								and sysno not in
								(
									select registersysno from rma_revert (NOLOCK), rma_revert_item (NOLOCK)
									where rma_revert.sysno = rma_revert_item.revertsysno and rma_revert.status = @2
								) and sysno = " + registerSysNo;
                sql = sql.Replace("@0", ((int)AppEnum.RMARequestStatus.Handling).ToString());
                sql = sql.Replace("@2", ((int)AppEnum.RMARevertStatus.WaitingRevert).ToString());
                sql = sql.Replace("@3", ((int)AppEnum.RMARevertStatus.WaitingRevert).ToString());
                sql = sql.Replace("@4", ((int)AppEnum.RMARevertStatus.WaitingAudit).ToString());  //加@4的原因是在用户审核时设置拒绝时也可以调用本方法CancelWaitingRevert


                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (!Util.HasMoreRow(ds))
                    throw new BizException("this recored's revert status or status can't allow such operation");


                DataRow dr = ds.Tables[0].Rows[0];
                int ShiftStatus = Util.TrimIntNull(dr["ShiftStatus"]);
                if (ShiftStatus != AppConst.IntNull && ShiftStatus != (int)AppEnum.ShiftStatus.Abandon && ShiftStatus != (int)AppEnum.ShiftStatus.InStock)
                    throw new BizException("存在有效的移库单状态，请先取消！");

                newProductStatusValue = Util.TrimIntNull(dr["newproductstatus"]);
                if (newProductStatusValue != (int)AppEnum.NewProductStatus.Origin)
                {
                    //取消占用库存
                    newProductSysNo = Util.TrimIntNull(dr["RevertProductSysNo"]);
                    //if (newProductStatusValue == (int)AppEnum.NewProductStatus.SecondHand) newProductSysNo = getSecondHandProductSysNo(newProductSysNo);
                    InventoryManager.GetInstance().SetAvailableQty(Util.TrimIntNull(dr["RevertStockSysNo"]), newProductSysNo, -1);
                }

                Hashtable ht = new Hashtable(5);
                ht.Add("SysNo", registerSysNo);
                ht.Add("RevertStatus", AppConst.IntNull);
                ht.Add("NewProductStatus", (int)AppEnum.NewProductStatus.Origin);
                ht.Add("RevertProductSysNo", AppConst.IntNull);
                if (Util.TrimIntNull(dr["RevertStockSysNo"]) != AppConst.IntNull)
                    ht.Add("RevertStockSysNo", AppConst.IntNull);

                this.UpdateRegister(ht);

                scope.Complete();
            }
        }

        //RMA单件处理中心明细页面上对发货的审核
        public void RevertAudit(Hashtable para, bool isAuditPass)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                int registerSysNo = (int)para["SysNo"];
                if (!isAuditPass) CancelWaitingRevert(registerSysNo);

                this.UpdateRegister(para);
                scope.Complete();
            }
        }

        public void SetWaitingOutBound(int registerSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                string sql = "select top 1 sysno from rma_register (NOLOCK) where outboundstatus is null and status in ( "
                    + (int)AppEnum.RMARequestStatus.Handling + ") and sysno = " + registerSysNo;

                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (!Util.HasMoreRow(ds))
                    throw new BizException("this recored's outbound status or status can't allow such operation");

                Hashtable ht = new Hashtable(5);
                ht.Add("SysNo", registerSysNo);
                ht.Add("OutBoundStatus", (int)AppEnum.RMAOutBoundStatus.Origin);

                this.UpdateRegister(ht);
                scope.Complete();
            }
        }

        public void CancelWaitingOutBound(int registerSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                string sql = @"select top 1 sysno from rma_register (NOLOCK) where outboundstatus=@3 and status in (@0)
								and sysno not in
								(
									select registersysno from rma_outbound (NOLOCK), rma_outbound_item (NOLOCK)
									where rma_outbound.sysno = rma_outbound_item.outboundsysno and rma_outbound.status = @2
								) and sysno = " + registerSysNo;
                sql = sql.Replace("@0", ((int)AppEnum.RMARequestStatus.Handling).ToString());
                sql = sql.Replace("@2", ((int)AppEnum.RMAOutBoundStatus.Origin).ToString());
                sql = sql.Replace("@3", ((int)AppEnum.RMAOutBoundStatus.Origin).ToString());


                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (!Util.HasMoreRow(ds))
                    throw new BizException("this recored's outbound status or status can't allow such operation");

                Hashtable ht = new Hashtable(5);
                ht.Add("SysNo", registerSysNo);
                ht.Add("OutBoundStatus", AppConst.IntNull);

                this.UpdateRegister(ht);
                scope.Complete();
            }
        }

        public void SetResponse(int registerSysNo, string responseDesc)
        {
            //1 是否可以response
            //2 partly response or all response
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                string sql = @"select top 1 sysno,location from rma_register where outboundstatus=@3 and status in (@0)
								and sysno in
								(
									select registersysno from rma_outbound (NOLOCK), rma_outbound_item (NOLOCK)
									where rma_outbound.sysno = rma_outbound_item.outboundsysno and rma_outbound.status in (@2,@4)
								) and sysno = " + registerSysNo;
                sql = sql.Replace("@0", ((int)AppEnum.RMARequestStatus.Handling).ToString());
                sql = sql.Replace("@2", ((int)AppEnum.RMAOutBoundStatus.SendAlready).ToString());
                sql = sql.Replace("@3", ((int)AppEnum.RMAOutBoundStatus.SendAlready).ToString());
                sql = sql.Replace("@4", ((int)AppEnum.RMAOutBoundStatus.PartlyResponsed).ToString());

                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (!Util.HasMoreRow(ds))
                    throw new BizException("this record's outbound status or status can't allow such operation");
                else
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    if (Util.TrimIntNull(dr["location"]) != (int)AppEnum.RMALocation.Vendor)
                        throw new BizException("操作失败，该商品已经退货入库或者尚未送修。");
                }

                //获取在同一个outbound 主表的 其他register, 判断response状态，然后设置主表的状态。
                //这里需要排除自己单据的状态。
                //SQL注释：从a获取outboundsysno,通过b获取所有rma_register。
                //这里是返还，因此主表状态可能是部分返还或者全部返还。
                //如果送修单中只有一个产品的话，
                string sql2 = @"select rma_register.outboundstatus, b.outboundsysno
								from
									rma_outbound_item a (NOLOCK), rma_outbound_item b (NOLOCK), rma_register (NOLOCK),rma_outbound e(NOLOCK)
								where
									a.outboundsysno = b.outboundsysno
								and a.registersysno = @0
								and b.registersysno = rma_register.sysno and rma_register.sysno!=@0
                                and e.sysno=b.outboundsysno and e.status <> @status";

                sql2 = sql2.Replace("@0", registerSysNo.ToString());
                sql2 = sql2.Replace("@status", ((int)AppEnum.RMAOutBoundStatus.Abandon).ToString());
                bool isAllResponsed = true;
                int outboundSysNo = AppConst.IntNull;

                DataSet ds2 = SqlHelper.ExecuteDataSet(sql2);
                if (Util.HasMoreRow(ds2))
                {
                    foreach (DataRow dr2 in ds2.Tables[0].Rows)
                    {
                        //因为outbound sysno 都是一样的，所以只获取一次
                        if (outboundSysNo == AppConst.IntNull)
                        {
                            outboundSysNo = Util.TrimIntNull(dr2["outboundsysno"]);
                        }
                        int outBoundStatus = Util.TrimIntNull(dr2["outboundstatus"]);
                        if (outBoundStatus == (int)AppEnum.RMAOutBoundStatus.SendAlready)
                        {
                            isAllResponsed = false;
                            break;
                        }
                    }
                }
                else  //送修单只有一个送修产品
                {
                    isAllResponsed = true;
                    string sqlReadBoundSysNo = "select outboundsysno from rma_outbound_item (nolock),rma_outbound (nolock) where registersysno={0} and rma_outbound.sysno=rma_outbound_item.outboundsysno and rma_outbound.status<> @status";
                    sqlReadBoundSysNo = sqlReadBoundSysNo.Replace("@status", ((int)AppEnum.RMAOutBoundStatus.Abandon).ToString());

                    DataSet tempDS = SqlHelper.ExecuteDataSet(String.Format(sqlReadBoundSysNo, registerSysNo));
                    if (Util.HasMoreRow(tempDS))
                        outboundSysNo = Util.TrimIntNull(tempDS.Tables[0].Rows[0]["outboundsysno"]);
                    else
                        throw new BizException("异常：送修单明细中没有Registersysno为" + registerSysNo.ToString() + "的单据。");
                }

                int outStatus = (int)AppEnum.RMAOutBoundStatus.Responsed;
                if (!isAllResponsed)
                    outStatus = (int)AppEnum.RMAOutBoundStatus.PartlyResponsed;

                Hashtable htOutBound = new Hashtable(5);
                htOutBound.Add("SysNo", outboundSysNo);
                htOutBound.Add("Status", outStatus);
                new RMAOutBoundDac().UpdateMaster(htOutBound);   //这里没有调用biz中对应名称方法，因为里面判断必须出于Orign状态，而这里的状态已经是送修。

                Hashtable ht = new Hashtable(5);
                ht.Add("SysNo", registerSysNo);
                ht.Add("OutBoundStatus", (int)AppEnum.RMAOutBoundStatus.Responsed);
                ht.Add("ResponseTime", DateTime.Now);
                ht.Add("ResponseDesc", responseDesc);
                ht.Add("Location", (int)AppEnum.RMALocation.Icson);

                this.UpdateRegister(ht);

                RMARegisterInfo reg = new RMARegisterInfo();
                map(reg, GetRegisterRow(registerSysNo));
                InventoryManager.GetInstance().SetInStockQty(reg.ReceiveStockSysNo, reg.ProductSysNo, 1);

                scope.Complete();
            }
        }

        public void CancelResponse(int registerSysNo)
        {
            //1 是否可以cancel response
            //2 partly response or all sendalready

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                string sql = @"select top 1 sysno,location from rma_register (NOLOCK) where outboundstatus=@3 and status in (@0)
								and sysno in
								(
									select registersysno from rma_outbound (NOLOCK), rma_outbound_item (NOLOCK)
									where rma_outbound.sysno = rma_outbound_item.outboundsysno and rma_outbound.status in (@2,@4)
								) and sysno = " + registerSysNo;
                sql = sql.Replace("@0", ((int)AppEnum.RMARequestStatus.Handling).ToString());
                sql = sql.Replace("@2", ((int)AppEnum.RMAOutBoundStatus.PartlyResponsed).ToString());
                sql = sql.Replace("@4", ((int)AppEnum.RMAOutBoundStatus.Responsed).ToString());
                sql = sql.Replace("@3", ((int)AppEnum.RMAOutBoundStatus.Responsed).ToString());


                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (!Util.HasMoreRow(ds))
                    throw new BizException("this record's outbound status or status can't allow such operation");
                else
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    if (Util.TrimIntNull(dr["location"]) != (int)AppEnum.RMALocation.Icson)
                        throw new BizException("操作失败，该商品已经入库或者送修尚未返还。");
                }

                //获取在同一个outbound 主表的 其他register, 判断response状态，然后设置主表的状态。
                //这里是取消返还，即没有从vendor那里拿到产品，因此主表状态可能是送修或者部分返还,选择主表的状态为非作废的
                //                string sql2 = @"select rma_register.outboundstatus, b.outboundsysno
                //								from
                //									rma_outbound_item a (NOLOCK), rma_outbound_item b (NOLOCK), rma_register (NOLOCK)
                //								where
                //									a.outboundsysno = b.outboundsysno
                //								and a.registersysno = @0
                //								and b.registersysno = rma_register.sysno  and rma_register.sysno!=@0";
                //                sql2 = sql2.Replace("@0", registerSysNo.ToString());

                string sql2 = @"select rma_register.outboundstatus, b.outboundsysno
								from
									rma_outbound_item a (NOLOCK), rma_outbound_item b (NOLOCK), rma_register (NOLOCK),rma_outbound e(NOLOCK)
								where
									a.outboundsysno = b.outboundsysno
								and a.registersysno = @0
								and b.registersysno = rma_register.sysno and rma_register.sysno!=@0
                                and e.sysno=b.outboundsysno and e.status <> @status";

                sql2 = sql2.Replace("@0", registerSysNo.ToString());
                sql2 = sql2.Replace("@status", ((int)AppEnum.RMAOutBoundStatus.Abandon).ToString());

                bool isAllInVendor = true;
                int outboundSysNo = AppConst.IntNull;

                DataSet ds2 = SqlHelper.ExecuteDataSet(sql2);
                if (Util.HasMoreRow(ds2))
                {
                    foreach (DataRow dr2 in ds2.Tables[0].Rows)
                    {
                        //因为outbound sysno 都是一样的，所以只获取一次
                        if (outboundSysNo == AppConst.IntNull)
                        {
                            outboundSysNo = Util.TrimIntNull(dr2["outboundsysno"]);
                        }
                        int outBoundStatus = Util.TrimIntNull(dr2["outboundstatus"]);
                        if (outBoundStatus == (int)AppEnum.RMAOutBoundStatus.Responsed)
                        {
                            isAllInVendor = false;
                            break;
                        }
                    }

                }
                else  //只有一个送修件
                {
                    isAllInVendor = true;
                    //string sqlReadBoundSysNo = "select outboundsysno from rma_outbound_item (nolock) where registersysno={0}";
                    string sqlReadBoundSysNo = "select outboundsysno from rma_outbound_item (nolock),rma_outbound (nolock) where registersysno={0} and rma_outbound.sysno=rma_outbound_item.outboundsysno and rma_outbound.status<> @status";
                    sqlReadBoundSysNo = sqlReadBoundSysNo.Replace("@status", ((int)AppEnum.RMAOutBoundStatus.Abandon).ToString());

                    DataSet tempDS = SqlHelper.ExecuteDataSet(String.Format(sqlReadBoundSysNo, registerSysNo));
                    if (Util.HasMoreRow(tempDS)) outboundSysNo = Util.TrimIntNull(tempDS.Tables[0].Rows[0]["outboundsysno"]);
                    else throw new BizException("异常：送修单明细中没有Registersysno为" + registerSysNo.ToString() + "的单据。");

                }

                int outStatus = (int)AppEnum.RMAOutBoundStatus.SendAlready;
                if (!isAllInVendor)
                    outStatus = (int)AppEnum.RMAOutBoundStatus.PartlyResponsed;

                Hashtable htOutBound = new Hashtable(5);
                htOutBound.Add("SysNo", outboundSysNo);
                htOutBound.Add("Status", outStatus);
                new RMAOutBoundDac().UpdateMaster(htOutBound);


                Hashtable ht = new Hashtable(5);
                ht.Add("SysNo", registerSysNo);
                ht.Add("OutBoundStatus", (int)AppEnum.RMAOutBoundStatus.SendAlready);
                ht.Add("Location", (int)AppEnum.RMALocation.Vendor);

                this.UpdateRegister(ht);

                RMARegisterInfo reg = new RMARegisterInfo();
                map(reg, GetRegisterRow(registerSysNo));
                InventoryManager.GetInstance().SetInStockQty(reg.ReceiveStockSysNo, reg.ProductSysNo, -1);

                scope.Complete();
            }
        }


        /// <summary>
        /// Regist相关信息比较多，很难用Map来实现。暂时用Row的形式，日后慢慢优化
        /// </summary>
        /// <param name="registerSysNo"></param>
        /// <returns></returns>
        public DataRow GetRegisterRow(int registerSysNo)
        {
            string sql = @"select 
								rma_request.sosysno,rma_request.note as requestnote,
								customername, rma_request.recvTime,rma_request.ReceiveType as ReceiveType,
								productid, productname, rma_register.sysno as registersysno,
								rma_register.*
							from
								rma_register (NOLOCK), customer (NOLOCK), rma_request_item (NOLOCK), rma_request (NOLOCK), product (NOLOCK)
							where
								rma_register.sysno = rma_request_item.registersysno
							and rma_request_item.requestsysno = rma_request.sysno
							and rma_request.customersysno = customer.sysno
							and rma_register.productsysno = product.sysno and rma_register.sysno = " + registerSysNo;

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                return ds.Tables[0].Rows[0];
            else
                return null;
        }

        public DataSet GetRegisterDs(Hashtable paramHash)
        {
            string sql = @"@select 
							rma_request.sosysno,
							customername,
							productid, productname,product.sysno as productsysno,rma_register.sysno as registersysno, rma_register.status as requestStatus,
							rma_register.* , rma_request.recvtime  ,customer.VIPRank  
							from
								rma_register (NOLOCK) 

								inner join rma_request_item (NOLOCK) on rma_register.sysno = rma_request_item.registersysno
								inner join rma_request (NOLOCK) on rma_request.sysno = rma_request_item.requestsysno
                                inner join so_master(NOLOCK) on rma_request.sosysno = so_master.sysno 
								inner join customer (NOLOCK) on rma_request.customersysno = customer.sysno
								inner join product (NOLOCK) on rma_register.productsysno = product.sysno
                                inner join category3 (NOLOCK) on product.c3sysno = category3.sysno
                                
                                

								left join 
                                (
                                select registersysno, outtime from
                                    rma_outbound (NOLOCK) inner join rma_outbound_item (NOLOCK) on rma_outbound.sysno = rma_outbound_item.outboundsysno
                                    and rma_outbound.status <>-1
                                ) as outbound on rma_register.sysno = outbound.registersysno

                                left join
                                (
                                 select registersysno , refundtime from
                                 rma_refund (NOLOCK) inner  join rma_refund_item (NOLOCK) on rma_refund.sysno = rma_refund_item.refundsysno
                                 and rma_refund.status <> -1
                                ) as refund on rma_register.sysno = refund.registersysno

                                left join
                                (
                                 select registersysno , returntime from
                                 rma_return (NOLOCK) inner join rma_return_item (NOLOCK) on rma_return.sysno = rma_return_item.returnsysno
                                 and rma_return.status <> -1
                                ) as rmareturn on rma_register.sysno = rmareturn.registersysno

                                left join
                                (
                                 select registersysno , outtime as reverttime from
                                 rma_revert (NOLOCK) inner join rma_revert_item (NOLOCK) on rma_revert.sysno = rma_revert_item.revertsysno
                                 and rma_revert.status <> -1
                                ) as rmarevert on rma_register.sysno = rmarevert.registersysno

                                left join RMA_Shift on rma_register.sysno = RMA_Shift.registersysno


							where
							1=1
                            @RegisterSysNo  @ProductSysNo @RequestType @sosysno  @CreateFrom  @CreateTo  @CheckFrom  @CheckTo  @RevertStatus
                            @NewProductStatus  @OutBoundStatus  @ReturnStatus  @NextHandler  @Status  @ResponseFrom  @ResponseTo @RecvFrom @RecvTo
							@OutBoundFrom @OutBoundTo @UnRecv @UnCheck @UnResponse @UnOutBound @RefundStatus  @RefundFrom  @RefundTo  @UnRefund
                            @ReturnFrom  @ReturnTo  @UnReturn @IsWithin7Days @RevertFrom @RevertTo @UnRevert @IsVIP @PMUserSysNo @Category  
                            @RefundInfo @RMAReason @IsRecommendRefund @UsedDate @NextContactTimeFrom @NextContactTimeTo @IsContact
                            @ContactCustomerUserSysNo @InspectionResultType @RMARegisterReceiveType @ShiftStatus @SetShiftTimeFrom @SetShiftTimeTo @ShiftSysNo";
            sql += "order by rma_request.sysno desc";
            
            //去除left join RMA_Shift
            //sql = sql.Replace("left join RMA_Shift on rma_register.sysno = RMA_Shift.registersysno", "");
            
            if (paramHash.ContainsKey("RegisterSysNo"))
                sql = sql.Replace("@RegisterSysNo", " and rma_register.sysno=" + paramHash["RegisterSysNo"].ToString());
            else
                sql = sql.Replace("@RegisterSysNo", "");

            if (paramHash.ContainsKey("ProductSysNo"))
                sql = sql.Replace("@ProductSysNo", " and rma_register.productsysno=" + paramHash["ProductSysNo"].ToString());
            else
                sql = sql.Replace("@ProductSysNo", "");

            if (paramHash.ContainsKey("RequestType"))
                sql = sql.Replace("@RequestType", " and rma_register.RequestType=" + paramHash["RequestType"].ToString());
            else
                sql = sql.Replace("@RequestType", "");

            if (paramHash.ContainsKey("So"))
            {
                int sysno = SaleManager.GetInstance().GetSOSysNofromID(paramHash["So"].ToString());
                if (sysno != AppConst.IntNull)
                    sql = sql.Replace("@sosysno", " and rma_request.sosysno=" + sysno.ToString());
                else
                    sql = sql.Replace("@sosysno", " and rma_request.sosysno=" + paramHash["So"].ToString());
            }
            else
                sql = sql.Replace("@sosysno", "");

            if (paramHash.ContainsKey("CreateFrom"))
                sql = sql.Replace("@CreateFrom", " and rma_request.createtime>=" + Util.ToSqlString(paramHash["CreateFrom"].ToString()));
            else
                sql = sql.Replace("@CreateFrom", "");

            if (paramHash.ContainsKey("CreateTo"))
                sql = sql.Replace("@CreateTo", " and rma_request.createtime <=" + Util.ToSqlEndDate(paramHash["CreateTo"].ToString()));
            else
                sql = sql.Replace("@CreateTo", "");

            if (paramHash.ContainsKey("CheckFrom"))
                sql = sql.Replace("@CheckFrom", " and rma_register.checktime >=" + Util.ToSqlString(paramHash["CheckFrom"].ToString()));
            else
                sql = sql.Replace("@CheckFrom", "");

            if (paramHash.ContainsKey("CheckTo"))
                sql = sql.Replace("@CheckTo", " and rma_register.checktime <=" + Util.ToSqlEndDate(paramHash["CheckTo"].ToString()));
            else
                sql = sql.Replace("@CheckTo", "");

            if (paramHash.ContainsKey("RevertStatus"))
                sql = sql.Replace("@RevertStatus", " and rma_register.revertstatus =" + paramHash["RevertStatus"].ToString());
            else
                sql = sql.Replace("@RevertStatus", "");

            if (paramHash.ContainsKey("NewProductStatus"))
                sql = sql.Replace("@NewProductStatus", " and rma_register.newproductstatus =" + paramHash["NewProductStatus"].ToString());
            else
                sql = sql.Replace("@NewProductStatus", "");

            if (paramHash.ContainsKey("OutBoundStatus"))
                sql = sql.Replace("@OutBoundStatus", " and rma_register.outboundstatus = " + paramHash["OutBoundStatus"].ToString());
            else
                sql = sql.Replace("@OutBoundStatus", "");

            if (paramHash.ContainsKey("ReturnStatus"))
                sql = sql.Replace("@ReturnStatus", " and rma_register.returnstatus = " + paramHash["ReturnStatus"].ToString());
            else
                sql = sql.Replace("@ReturnStatus", "");

            if (paramHash.ContainsKey("NextHandler"))
                sql = sql.Replace("@NextHandler", " and rma_register.nexthandler=" + paramHash["NextHandler"].ToString());
            else
                sql = sql.Replace("@NextHandler", "");

            if (paramHash.ContainsKey("Status"))
                sql = sql.Replace("@Status", " and rma_register.status=" + paramHash["Status"].ToString());
            else
                sql = sql.Replace("@Status", "");

            if (paramHash.ContainsKey("ResponseFrom"))
                sql = sql.Replace("@ResponseFrom", " and rma_register.responsetime>=" + Util.ToSqlString(paramHash["ResponseFrom"].ToString()));
            else
                sql = sql.Replace("@ResponseFrom", "");

            if (paramHash.ContainsKey("ResponseTo"))
                sql = sql.Replace("@ResponseTo", " and rma_register.responsetime <=" + Util.ToSqlEndDate(paramHash["ResponseTo"].ToString()));
            else
                sql = sql.Replace("@ResponseTo", "");

            if (paramHash.ContainsKey("RecvFrom"))
                sql = sql.Replace("@RecvFrom", " and rma_request.Recvtime>=" + Util.ToSqlString(paramHash["RecvFrom"].ToString()));
            else
                sql = sql.Replace("@RecvFrom", "");

            if (paramHash.ContainsKey("RecvTo"))
                sql = sql.Replace("@RecvTo", " and rma_request.Recvtime <=" + Util.ToSqlEndDate(paramHash["RecvTo"].ToString()));
            else
                sql = sql.Replace("@RecvTo", "");

            if (paramHash.ContainsKey("OutBoundFrom"))
                sql = sql.Replace("@OutBoundFrom", " and outbound.Outtime>=" + Util.ToSqlString(paramHash["OutBoundFrom"].ToString()));
            else
                sql = sql.Replace("@OutBoundFrom", "");
            if (paramHash.ContainsKey("OutBoundTo"))
                sql = sql.Replace("@OutBoundTo", " and outbound.Outtime <=" + Util.ToSqlEndDate(paramHash["OutBoundTo"].ToString()));
            else
                sql = sql.Replace("@OutBoundTo", "");

            if (paramHash.ContainsKey("UnRecv"))
                sql = sql.Replace("@UnRecv", " and rma_request.RecvTime is null");
            else
                sql = sql.Replace("@UnRecv", "");

            if (paramHash.ContainsKey("UnCheck"))
                sql = sql.Replace("@UnCheck", " and rma_register.checktime is null");
            else
                sql = sql.Replace("@UnCheck", "");

            if (paramHash.ContainsKey("UnResponse"))
                sql = sql.Replace("@UnResponse", " and rma_register.responsetime is null");
            else
                sql = sql.Replace("@UnResponse", "");

            if (paramHash.ContainsKey("UnOutBound"))
                sql = sql.Replace("@UnOutBound", " and outbound.Outtime is null");
            else
                sql = sql.Replace("@UnOutBound", "");

            if (paramHash.ContainsKey("RefundStatus"))
                sql = sql.Replace("@RefundStatus", " and rma_register.refundstatus = " + paramHash["RefundStatus"].ToString());
            else
                sql = sql.Replace("@RefundStatus", "");

            if (paramHash.ContainsKey("RefundFrom"))
                sql = sql.Replace("@RefundFrom", " and refund.refundtime >=" + Util.ToSqlString(paramHash["RefundFrom"].ToString()));
            else
                sql = sql.Replace("@RefundFrom", "");

            if (paramHash.ContainsKey("RefundTo"))
                sql = sql.Replace("@RefundTo", " and refund.refundtime <=" + Util.ToSqlEndDate(paramHash["RefundTo"].ToString()));
            else
                sql = sql.Replace("@RefundTo", "");

            if (paramHash.ContainsKey("UnRefund"))
                sql = sql.Replace("@UnRefund", " and refund.refundtime is null");
            else
                sql = sql.Replace("@UnRefund", "");

            if (paramHash.ContainsKey("ReturnFrom"))
                sql = sql.Replace("@ReturnFrom", " and rmareturn.returntime >=" + Util.ToSqlString(paramHash["ReturnFrom"].ToString()));
            else
                sql = sql.Replace("@ReturnFrom", "");

            if (paramHash.ContainsKey("ReturnTo"))
                sql = sql.Replace("@ReturnTo", " and rmareturn.returntime <=" + Util.ToSqlEndDate(paramHash["ReturnTo"].ToString()));
            else
                sql = sql.Replace("@ReturnTo", "");

            if (paramHash.ContainsKey("UnReturn"))
                sql = sql.Replace("@UnReturn", " and rmareturn.returntime is null");
            else
                sql = sql.Replace("@UnReturn", "");

            if (paramHash.ContainsKey("RevertFrom"))
                sql = sql.Replace("@RevertFrom", " and rmarevert.reverttime >=" + Util.ToSqlString(paramHash["RevertFrom"].ToString()));
            else
                sql = sql.Replace("@RevertFrom", "");

            if (paramHash.ContainsKey("RevertTo"))
                sql = sql.Replace("@RevertTo", " and rmarevert.reverttime <=" + Util.ToSqlEndDate(paramHash["RevertTo"].ToString()));
            else
                sql = sql.Replace("@RevertTo", "");

            if (paramHash.ContainsKey("UnRevert"))
                sql = sql.Replace("@UnRevert", " and rmarevert.reverttime is null");
            else
                sql = sql.Replace("@UnRevert", "");

            if (paramHash.ContainsKey("IsWithin7Days"))
                sql = sql.Replace("@IsWithin7Days", " and rma_register.iswithin7days = " + Util.TrimNull(paramHash["IsWithin7Days"]));
            else
                sql = sql.Replace("@IsWithin7Days", "");

            if (paramHash.ContainsKey("IsVIP"))
            {
                if (Util.TrimIntNull(paramHash["IsVIP"]) == (int)AppEnum.YNStatus.Yes)
                {
                    sql = sql.Replace("@IsVIP", "and (customer.VIPRank=" + (int)AppEnum.CustomerVIPRank.AutoVIP + " or customer.VIPRank=" + (int)AppEnum.CustomerVIPRank.ManualVIP + ") ");
                }
                else
                {
                    sql = sql.Replace("@IsVIP", "and (customer.VIPRank=" + (int)AppEnum.CustomerVIPRank.AutoNonVIP + " or customer.VIPRank=" + (int)AppEnum.CustomerVIPRank.ManualNonVIP + ") ");
                }
            }
            else
            {
                sql = sql.Replace("@IsVIP", "");
            }

            if (paramHash.ContainsKey("PMUserSysNo"))
                sql = sql.Replace("@PMUserSysNo", "and product.PMUserSysNo = " + paramHash["PMUserSysNo"].ToString() + "");
            else
                sql = sql.Replace("@PMUserSysNo", "");

            if (paramHash.ContainsKey("Category"))
                sql = sql.Replace("@Category", "and category3.sysno=" + paramHash["Category"].ToString() + "");
            else
                sql = sql.Replace("@Category", "");

            if (paramHash.ContainsKey("RMAReason"))
                sql = sql.Replace("@RMAReason", "and rma_register.RMAReason=" + Util.TrimIntNull(paramHash["RMAReason"]) + "");
            else
                sql = sql.Replace("@RMAReason", "");


            if (paramHash.ContainsKey("IsRecommendRefund"))
                sql = sql.Replace("@IsRecommendRefund", " and rma_register.IsRecommendRefund=" + Util.TrimIntNull(paramHash["IsRecommendRefund"]) + "");
            else
                sql = sql.Replace("@IsRecommendRefund", "");

            if (paramHash.ContainsKey("ShiftStatus"))
                sql = sql.Replace("@ShiftStatus", " and rma_register.ShiftStatus=" + Util.TrimIntNull(paramHash["ShiftStatus"]) + "");
            else
                sql = sql.Replace("@ShiftStatus", "");

            if (paramHash.ContainsKey("SetShiftTimeFrom"))
                sql = sql.Replace("@SetShiftTimeFrom", " and  isnull(rma_register.SetShiftTime,'" + AppConst.DateTimeNull + "') >=" + Util.ToSqlString(paramHash["SetShiftTimeFrom"].ToString()));
            else
                sql = sql.Replace("@SetShiftTimeFrom", "");

            if (paramHash.ContainsKey("SetShiftTimeTo"))
                sql = sql.Replace("@SetShiftTimeTo", " and isnull(rma_register.SetShiftTime,'" + AppConst.DateTimeNull + "') <=" + Util.ToSqlEndDate(paramHash["SetShiftTimeTo"].ToString()));
            else
                sql = sql.Replace("@SetShiftTimeTo", "");

            if (paramHash.ContainsKey("ShiftSysNo"))
            {
                int sysno = ShiftManager.GetInstance().GetShiftSysNofromID(paramHash["ShiftSysNo"].ToString());
                if (sysno != AppConst.IntNull)
                    sql = sql.Replace("@ShiftSysNo", " and RMA_Shift.ShiftSysNo=" + sysno.ToString());
                else
                    sql = sql.Replace("@ShiftSysNo", " and RMA_Shift.ShiftSysNo=" + paramHash["ShiftSysNo"].ToString());
            }
            else
                sql = sql.Replace("@ShiftSysNo", "");


            //正常退款=0，非正常退款=1 ，所有=ALL
            string RefundInfo = paramHash["RefundInfo"].ToString();
            if (RefundInfo.Equals("ALL"))
            {
                sql = sql.Replace("@RefundInfo", "");
            }
            else
            {
                if (RefundInfo.Equals("0")) sql = sql.Replace("@RefundInfo", " and rma_register.refundstatus=" + Convert.ToString((int)AppEnum.RMARefundStatus.Refunded) + " and rma_register.IsRecommendRefund=" + Convert.ToString((int)AppEnum.RecommendRefund.Yes));
                else sql = sql.Replace("@RefundInfo", " and rma_register.refundstatus=" + Convert.ToString((int)AppEnum.RMARefundStatus.Refunded) + " and rma_register.IsRecommendRefund=" + Convert.ToString((int)AppEnum.RecommendRefund.No));
            }

            if (paramHash.ContainsKey("UsedDate"))
            {
                sql = sql.Replace("@UsedDate", " and DATEDIFF(day,so_master.AuditDeliveryDate,rma_request.createtime)<=" + Util.TrimNull(paramHash["UsedDate"]));
            }
            else
            {
                sql = sql.Replace("@UsedDate", "");
            }


            if (paramHash.ContainsKey("NextContactTimeFrom"))
            {
                sql = sql.Replace("@NextContactTimeFrom", "and exists(select NextContactTime from rma_contactCustomer  where NextContactTime>=" + Util.ToSqlString(paramHash["NextContactTimeFrom"].ToString()) + "and rma_contactCustomer.sysno in (SELECT max(sysno) as sysno FROM RMA_ContactCustomer GROUP BY RegisterSysNo) and rma_contactCustomer.RegisterSysNo=rma_register.sysno)");
            }
            else
            {
                sql = sql.Replace("@NextContactTimeFrom", "");
            }

            if (paramHash.ContainsKey("NextContactTimeTo"))
            {
                sql = sql.Replace("@NextContactTimeTo", "and exists(select NextContactTime from rma_contactCustomer  where NextContactTime<=" + Util.ToSqlString(paramHash["NextContactTimeTo"].ToString()) + "and rma_contactCustomer.sysno in (SELECT max(sysno) as sysno FROM RMA_ContactCustomer GROUP BY RegisterSysNo) and rma_contactCustomer.RegisterSysNo=rma_register.sysno)");
            }
            else
            {
                sql = sql.Replace("@NextContactTimeTo", "");
            }

            if (paramHash.ContainsKey("IsContact"))
            {

                if (Util.TrimIntNull(paramHash["IsContact"]) == (int)AppEnum.YNStatus.No)
                    sql = sql.Replace("@IsContact", " and exists( select * from RMA_ContactCustomer  where rma_contactCustomer.nextContactTime is null and rma_contactCustomer.sysno in (SELECT max(sysno) as sysno FROM RMA_ContactCustomer GROUP BY RegisterSysNo) and  rma_contactCustomer.RegisterSysNo=rma_register.sysno)");
                else if (Util.TrimIntNull(paramHash["IsContact"]) == (int)AppEnum.YNStatus.Yes)
                    sql = sql.Replace("@IsContact", " and exists( select * from RMA_ContactCustomer  where rma_contactCustomer.nextContactTime is not null and rma_contactCustomer.sysno in (SELECT max(sysno) as sysno FROM RMA_ContactCustomer GROUP BY RegisterSysNo) and  rma_contactCustomer.RegisterSysNo=rma_register.sysno)");
            }
            else
            {
                sql = sql.Replace("@IsContact", "");
            }


            if (paramHash.ContainsKey("ContactCustomerUserSysNo"))
            {
                sql = sql.Replace("@ContactCustomerUserSysNo", " and exists( select * from RMA_ContactCustomer rc where  rc.ContactUserSysNo= " + Util.ToSqlString(paramHash["ContactCustomerUserSysNo"].ToString()) + "  and rc.sysno in (SELECT max(sysno) as sysno FROM RMA_ContactCustomer GROUP BY RegisterSysNo) and  rc.RegisterSysNo=rma_register.sysno)");
            }
            else
            {
                sql = sql.Replace("@ContactCustomerUserSysNo", "");
            }

            if (paramHash.ContainsKey("InspectionResultType"))
            {
                sql = sql.Replace("@InspectionResultType", " and rma_register.InspectionResultType like" + Util.ToSqlLikeString(paramHash["InspectionResultType"].ToString()));
            }
            else
            {
                sql = sql.Replace("@InspectionResultType", "");
            }
            if (paramHash.ContainsKey("RMARegisterReceiveType"))
            {
                sql = sql.Replace("@RMARegisterReceiveType", " and isnull(rma_register.RegisterReceiveType,'" + AppConst.IntNull + "') " + paramHash["RMARegisterReceiveType"].ToString());
            }
            else
            {
                sql = sql.Replace("@RMARegisterReceiveType", "");
            }

            if (paramHash == null || paramHash.Count == 0)
                sql = sql.Replace("@select", "select top 50");
            else
                sql = sql.Replace("@select", "select");




            return SqlHelper.ExecuteDataSet(sql);
        }


        private void map(RMARegisterInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.ProductSysNo = Util.TrimIntNull(tempdr["ProductSysNo"]);
            oParam.RequestType = Util.TrimIntNull(tempdr["RequestType"]);
            oParam.CustomerDesc = Util.TrimNull(tempdr["CustomerDesc"]);
            oParam.CheckTime = Util.TrimDateNull(tempdr["CheckTime"]);
            oParam.CheckDesc = Util.TrimNull(tempdr["CheckDesc"]);
            oParam.NewProductStatus = Util.TrimIntNull(tempdr["NewProductStatus"]);
            oParam.RevertStatus = Util.TrimIntNull(tempdr["RevertStatus"]);
            oParam.OutBoundStatus = Util.TrimIntNull(tempdr["OutBoundStatus"]);
            oParam.ReturnStatus = Util.TrimIntNull(tempdr["ReturnStatus"]);
            oParam.ResponseDesc = Util.TrimNull(tempdr["ResponseDesc"]);
            oParam.ResponseTime = Util.TrimDateNull(tempdr["ResponseTime"]);
            oParam.RefundStatus = Util.TrimIntNull(tempdr["RefundStatus"]);
            oParam.NextHandler = Util.TrimIntNull(tempdr["NextHandler"]);
            oParam.Memo = Util.TrimNull(tempdr["Memo"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
            oParam.ProductNo = Util.TrimNull(tempdr["ProductNo"]);
            oParam.ProductIDSysNo = Util.TrimIntNull(tempdr["ProductIDSysNo"]);
            oParam.NewProductIDSysNo = Util.TrimIntNull(tempdr["NewProductIDSysNo"]);
            oParam.IsWithin7Days = Util.TrimIntNull(tempdr["IsWithin7Days"]);
            oParam.IsRecommendRefund = Util.TrimIntNull(tempdr["IsRecommendRefund"]);
            oParam.RefundInfo = Util.TrimNull(tempdr["RefundInfo"]);
            oParam.OwnBy = Util.TrimIntNull(tempdr["OwnBy"]);
            oParam.Location = Util.TrimIntNull(tempdr["Location"]);
            oParam.Cost = Util.TrimDecimalNull(tempdr["Cost"]);
            oParam.RMAReason = Util.TrimIntNull(tempdr["RMAReason"]);
            oParam.CheckUserSysNo = Util.TrimIntNull(tempdr["CheckUserSysNo"]);
            oParam.ResponseUserSysNo = Util.TrimIntNull(tempdr["ResponseUserSysNo"]);
            oParam.CloseUserSysNo = Util.TrimIntNull(tempdr["CloseUserSysNo"]);
            oParam.CloseTime = Util.TrimDateNull(tempdr["CloseTime"]);
            oParam.RevertAuditUserSysNo = Util.TrimIntNull(tempdr["RevertAuditUserSysNo"]);
            oParam.RevertAuditTime = Util.TrimDateNull(tempdr["RevertAuditTime"]);
            oParam.RevertAuditMemo = Util.TrimNull(tempdr["RevertAuditMemo"]);
            oParam.RevertProductSysNo = Util.TrimIntNull(tempdr["RevertProductSysNo"]);
            oParam.IsHaveInvoice = Util.TrimIntNull(tempdr["IsHaveInvoice"]);
            oParam.IsFullAccessory = Util.TrimIntNull(tempdr["IsFullAccessory"]);
            oParam.IsFullPackage = Util.TrimIntNull(tempdr["IsFullPackage"]);
            oParam.ReceiveStockSysNo = Util.TrimIntNull(tempdr["ReceiveStockSysNo"]);
            oParam.AttachmentInfo = Util.TrimNull(tempdr["AttachmentInfo"]);
            oParam.InspectionResultType = Util.TrimNull(tempdr["InspectionResultType"]);
            oParam.VendorRepairResultType = Util.TrimNull(tempdr["VendorRepairResultType"]);
            oParam.OutBoundWithInvoice = Util.TrimIntNull(tempdr["OutBoundWithInvoice"]);
            oParam.ResponseProductNo = Util.TrimNull(tempdr["ResponseProductNo"]);
            oParam.RevertStockSysNo = Util.TrimIntNull(tempdr["RevertStockSysNo"]);
            oParam.PMDunDesc = Util.TrimNull(tempdr["PMDunDesc"]);
            oParam.PMDunTime = Util.TrimDateNull(tempdr["PMDunTime"]);
            oParam.IsContactCustomer = Util.TrimIntNull(tempdr["IsContactCustomer"]);
            oParam.RegisterReceiveType = Util.TrimIntNull(tempdr["RegisterReceiveType"]);
            oParam.RefundAuditUserSysNo = Util.TrimIntNull(tempdr["RefundAuditUserSysNo"]);
            oParam.RefundAuditTime = Util.TrimDateNull(tempdr["RefundAuditTime"]);
            oParam.RefundAuditMemo = Util.TrimNull(tempdr["RefundAuditMemo"]);
            oParam.ShiftStatus = Util.TrimIntNull(tempdr["ShiftStatus"]);
            oParam.SetShiftUserSysNo = Util.TrimIntNull(tempdr["SetShiftUserSysNo"]);
            oParam.SetShiftTime = Util.TrimDateNull(tempdr["SetShiftTime"]);
            oParam.CheckRepairResult = Util.TrimIntNull(tempdr["CheckRepairResult"]);
            oParam.CheckRepairNote = Util.TrimNull(tempdr["CheckRepairNote"]);
            oParam.CheckRepairUserSysNo = Util.TrimIntNull(tempdr["CheckRepairUserSysNo"]);
            oParam.CheckRepairTime = Util.TrimDateNull(tempdr["CheckRepairTime"]);
        }

        public void ConvertRegisterBoundleHash(Hashtable ht)
        {
            if (ht == null || ht.Count == 0)
                return;

            int index = 0;
            string boundle = "";

            foreach (int rSysNo in ht.Keys)
            {
                if (index != 0)
                    boundle += ",";

                boundle += rSysNo.ToString();
                index++;
            }

            string sql = "select * from rma_register (NOLOCK) where sysno in (@0)";
            sql = sql.Replace("@0", boundle);
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            ht.Clear();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                RMARegisterInfo oR = new RMARegisterInfo();
                map(oR, dr);
                ht.Add(oR.SysNo, oR);
            }
        }

        public DataSet SearchRMAStore(Hashtable paramHash)
        {
            string sql = @"select a.* , product.productid , product.productname,b.*
                           from
                           (
								select product.sysno , 
								count(product.sysno) as total ,
								isnull(sum(case when (outboundstatus = @outboundstatus ) then 1 end) , 0) as outbounded ,
								isnull(sum(case when revertstatus = @revertstatus then 1 end) , 0) as reverted ,
								isnull(sum(case when returnstatus = @returnstatus then 1 end) , 0) as returned ,
                                isnull(sum(case when revertstatus = @revertstatus and newproductstatus <> @newproductstatus then 1 end) , 0) as newproduct ,
								isnull(sum(case when returnstatus = @returnstatus and targetproduct.productid = product.productid then 1 end) , 0) as new ,
                                isnull(sum(case when returnstatus = @returnstatus and (targetproduct.productid = product.productid+'r' or targetproduct.productid = product.productid + 'R') then 1 end) , 0) as secondhand ,
                                isnull(sum(case when returnstatus = @returnstatus and (targetproduct.productid = product.productid + 'b' or targetproduct.productid = product.productid + 'B') then 1 end) , 0) as badproduct ,
                                isnull(sum(rma_register.cost) ,0) as cost 
								from rma_register (NOLOCK) 
                                inner join rma_request_item (NOLOCK) on rma_request_item.registerSysNo = rma_register.sysno
                                inner join rma_request (NOLOCK) on rma_request.sysno = rma_request_item.requestsysno
								inner join product (NOLOCK) on rma_register.productsysno = product.sysno
                                left join rma_return_item (NOLOCK) on rma_return_item.registersysno = rma_register.sysno
                                left join product as targetproduct (NOLOCK) on rma_return_item.targetproductsysno = targetproduct.sysno
								where rma_register.status = @registerstatus and rma_request.recvtime is not null  @productsysno @ownby
								          group by product.sysno
                           ) as a 
                           inner join product on product.sysno = a.sysno 
                           left join 
                           (
                                select productsysno, sum(case when stocksysnoB=@instocksysno and status=@instockstatus then shiftqty else '0' end) as shiftin,
                                sum( case when stocksysnoA=@outstocksysno and (status=@outstockstatus or status=@instockstatus) then shiftqty else '0' end) shiftout 
                                from st_shift ss inner join st_shift_item si on ss.sysno=si.shiftsysno
                                group by productsysno
                            ) 
                            as b 
                            on product.sysno=b.productsysno";

            sql = sql.Replace("@outboundstatus", ((int)AppEnum.RMAOutBoundStatus.SendAlready).ToString());
            sql = sql.Replace("@revertstatus", ((int)AppEnum.RMARevertStatus.Reverted).ToString());
            sql = sql.Replace("@returnstatus", ((int)AppEnum.RMAReturnStatus.Returned).ToString());
            sql = sql.Replace("@registerstatus", ((int)AppEnum.RMARequestStatus.Handling).ToString());
            sql = sql.Replace("@newproductstatus", ((int)AppEnum.NewProductStatus.Origin).ToString());

            int StockSysNo = Util.TrimIntNull(paramHash["StockSysNo"].ToString());
            sql = sql.Replace("@instocksysno", StockSysNo.ToString());
            sql = sql.Replace("@outstocksysno", StockSysNo.ToString());
            sql = sql.Replace("@instockstatus", ((int)AppEnum.ShiftStatus.InStock).ToString());
            sql = sql.Replace("@outstockstatus", ((int)AppEnum.ShiftStatus.OutStock).ToString());

            if (paramHash.ContainsKey("ProductSysNo"))
                sql = sql.Replace("@productsysno", " and product.sysno = " + paramHash["ProductSysNo"].ToString());
            else
                sql = sql.Replace("@productsysno", "");

            if (paramHash.ContainsKey("OwnBy"))
                sql = sql.Replace("@ownby", " and ownby=" + paramHash["OwnBy"].ToString());
            else
                sql = sql.Replace("@ownby", "");

            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetRMAInventoryDetail(Hashtable paramHash)
        {
            string sql = @"select rr.SysNo,rr.OutBoundStatus,rr.RevertStatus,
                           rr.RefundStatus,rr.ReturnStatus,rr.Cost,rr.NewProductStatus,
                           product.ProductID,product.ProductName
                           from rma_register  as rr (NOLOCK)
                           inner join rma_request_item (NOLOCK) on rma_request_item.registerSysNo = rr.sysno
                           inner join rma_request (NOLOCK) on rma_request.sysno = rma_request_item.requestsysno
                           inner join product (NOLOCK) on rr.ProductSysNo = product.SysNo
                           where 1=1 @SysNo @productsysno @ownby and rma_request.recvtime is not null and  rr.Status=" + (int)AppEnum.RMARequestStatus.Handling + "";
            sql += "order by rr.OutBoundStatus,rr.SysNo desc";

            if (paramHash.ContainsKey("ProductSysNo"))
                sql = sql.Replace("@productsysno", "and product.sysno = " + paramHash["ProductSysNo"].ToString());
            else
                sql = sql.Replace("@productsysno", "");
            if (paramHash.ContainsKey("OwnBy"))
                sql = sql.Replace("@ownby", " and ownby=" + paramHash["OwnBy"].ToString());
            else
                sql = sql.Replace("@ownby", "");

            if (paramHash.ContainsKey("RegisterSysNo"))
                sql = sql.Replace("@SysNo", " and rr.SysNo=" + Util.TrimNull(paramHash["RegisterSysNo"]));
            else
                sql = sql.Replace("@SysNo", "");

            return SqlHelper.ExecuteDataSet(sql);
        }
        public string GetSoSysNoFromRegisterSysno(int sysno)
        {
            string sql = @"select rma_request.sosysno
                           from rma_register (NOLOCK)
                           inner join rma_request_item (NOLOCK) on rma_register.sysno = rma_request_item.registersysno
                           inner join rma_request (NOLOCK) on rma_request_item.requestsysno = rma_request.sysno
                           where rma_register.sysno =" + sysno;

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                throw new Exception("Get SoSysNo Error!");

            DataRow dr = ds.Tables[0].Rows[0];
            return dr["sosysno"].ToString();

        }

        public void Set7Days(Hashtable paramHash)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                string sql = @"select top 1 sysno from rma_register (NOLOCK)
                           where status=@status";

                sql = sql.Replace("@status", ((int)AppEnum.RMARequestStatus.Orgin).ToString());

                DataSet ds = SqlHelper.ExecuteDataSet(sql);

                if (!Util.HasMoreRow(ds))
                    throw new BizException("this recored's status can't allow such operation");

                Hashtable ht = new Hashtable();
                ht.Add("SysNo", paramHash["SysNo"].ToString());
                ht.Add("IsWithin7Days", (int)AppEnum.YNStatus.Yes);
                new RMARegisterDac().Update(ht);

                scope.Complete();
            }
        }

        public void CancelSet7Days(Hashtable paramHash)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                string sql = @"select top 1 sysno from rma_register (NOLOCK)
                           where status=@status";

                sql = sql.Replace("@status", ((int)AppEnum.RMARequestStatus.Orgin).ToString());

                DataSet ds = SqlHelper.ExecuteDataSet(sql);

                if (!Util.HasMoreRow(ds))
                    throw new BizException("this recored's status can't allow such operation");

                Hashtable ht = new Hashtable();
                ht.Add("SysNo", paramHash["SysNo"].ToString());
                ht.Add("IsWithin7Days", (int)AppEnum.YNStatus.No);
                new RMARegisterDac().Update(ht);

                scope.Complete();
            }
        }

        public int GetOutBoundSysNoByRegister(int RMARegisterSysNo)
        {
            string sql = "select outboundsysno from rma_outbound_item(NOLOCK),rma_outbound (NOLOCK) where registersysno=" + RMARegisterSysNo + "and rma_outbound.status<> @status and rma_outbound.sysno=rma_outbound_Item.outboundsysno";
            sql = sql.Replace("@status", ((int)AppEnum.RMAOutBoundStatus.Abandon).ToString());
            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return AppConst.IntNull;

            DataRow dr = ds.Tables[0].Rows[0];

            return Util.TrimIntNull(dr["outboundsysno"]);

        }

        public int GetRevertSysNoByRegister(int RMARegisterSysNo)
        {
            string sql = "select revertsysno from rma_revert_item (NOLOCK) where registersysno=" + RMARegisterSysNo;

            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return AppConst.IntNull;

            DataRow dr = ds.Tables[0].Rows[0];

            return Util.TrimIntNull(dr["revertsysno"]);
        }

        public int GetReturnSysNoByRegister(int RMARegisterSysNo)
        {
            string sql = @"select returnsysno 
                           from rma_register
                           inner join rma_return_item (NOLOCK) on rma_register.sysno = rma_return_item.registersysno
                           inner join rma_return (NOLOCK) on rma_return_item.returnsysno = rma_return.sysno                           
                           where rma_return.status=rma_register.returnstatus and rma_register.sysno=" + RMARegisterSysNo;


            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return AppConst.IntNull;

            DataRow dr = ds.Tables[0].Rows[0];

            return Util.TrimIntNull(dr["returnsysno"]);
        }

        public int GetRefundSysNoByRegister(int RMARegisterSysNo)
        {
            string sql = "select refundsysno from rma_refund_item (NOLOCK) where registersysno=" + RMARegisterSysNo;

            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return AppConst.IntNull;

            DataRow dr = ds.Tables[0].Rows[0];

            return Util.TrimIntNull(dr["refundsysno"]);
        }

        public int GetRequestSysNoByRegister(int RMARegisterSysNo)
        {
            string sql = "select requestsysno from rma_request_item (NOLOCK) where registersysno=" + RMARegisterSysNo;

            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return AppConst.IntNull;

            DataRow dr = ds.Tables[0].Rows[0];

            return Util.TrimIntNull(dr["requestsysno"]);
        }

        public int GetShiftSysNoByRegister(int RMARegisterSysNo)
        {
            string sql = "select ShiftSysNo from RMA_Shift (NOLOCK) where registersysno=" + RMARegisterSysNo;

            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return AppConst.IntNull;

            DataRow dr = ds.Tables[0].Rows[0];

            return Util.TrimIntNull(dr["ShiftSysNo"]);
        }


        /// <summary>
        /// 根据RMA_Register的id，获取MasterList打印替换信息。
        /// </summary>
        /// <param name="registerSysNo"></param>
        /// <param name="values"></param>
        public void GetMasterListInfo(int registerSysNo, Hashtable values)
        {
            DataRow row = GetRegisterRow(registerSysNo);  //这个方法是单件处理中心明细页面调用的方法
            int soSysNo = -1;
            string charSelected = "■";
            string charNoSelected = "□";
            int newProductStatus;
            int refundStatus;
            int returnStatus;

            if (row != null)
            {

                soSysNo = Util.TrimIntNull(row["SOSysNo"]);

                values.Add("_productID", Util.TrimNull(row["ProductID"]));
                values.Add("_reisterSysNo", registerSysNo);
                values.Add("_sOSysNo", soSysNo);

                //设置RMA申请信息
                getMasterListInfoRequest(registerSysNo, values);

                values.Add("_productName", Util.TrimNull(row["ProductName"]));
                values.Add("_customerDesc", Util.TrimNull(row["CustomerDesc"]));
                values.Add("_checkDesc", Util.TrimNull(row["CheckDesc"]));
                values.Add("_checkUser", getUserName(Util.TrimIntNull(row["CheckUserSysNo"])));
                values.Add("_checkDate", getDateString(Util.TrimDateNull(row["CheckTime"])));

                values.Add("_responseUser", getUserName(Util.TrimIntNull(row["ResponseUserSysNo"])));
                values.Add("_responseDate", getDateString(Util.TrimDateNull(row["ResponseTime"])));


                //是否退货,这里参考的是退款标志
                refundStatus = Util.TrimIntNull(row["refundStatus"]);
                if (refundStatus == (int)AppEnum.RMARefundStatus.Refunded)
                {
                    values.Add("_returnYes", charSelected);
                    values.Add("_returnNo", charNoSelected);
                }
                else
                {
                    values.Add("_returnYes", charNoSelected);
                    values.Add("_returnNo", charSelected);
                }

                //是否发新品
                newProductStatus = Util.TrimIntNull(row["newProductStatus"]);
                if (newProductStatus != (int)AppEnum.NewProductStatus.Origin)
                {
                    values.Add("_returnNewYes", charSelected);
                    values.Add("_returnNewNo", charNoSelected);
                }
                else
                {
                    values.Add("_returnNewYes", charNoSelected);
                    values.Add("_returnNewNo", charSelected);
                }

                //是否退货入库
                returnStatus = Util.TrimIntNull(row["returnStatus"]);
                if (returnStatus == (int)AppEnum.RMAReturnStatus.Returned)
                {
                    values.Add("_InStockYes", charSelected);
                    values.Add("_InStockNo", charNoSelected);
                }
                else
                {
                    values.Add("_InStockYes", charNoSelected);
                    values.Add("_InStockNo", charSelected);
                }

                //设置退货信息
                getMasterListInfoReturn(registerSysNo, values);
                //设置发货信息
                getMasterListInfoRevert(registerSysNo, values);

                //设置结案人。结案日期信息
                if (Util.TrimIntNull(row["Status"]) == (int)AppEnum.RMARequestStatus.Closed)
                {
                    values.Add("_closeUser", getUserName(Util.TrimIntNull(row["CloseUserSysNo"])));
                    if (Util.TrimDateNull(row["CloseTime"]) != AppConst.DateTimeNull) values.Add("_closeDate", Util.TrimDateNull(row["CloseTime"]).ToString(AppConst.DateFormat));
                    else values.Add("_closeDate", Blank);
                }
                else
                {
                    values.Add("_closeUser", Blank);
                    values.Add("_closeDate", Blank);
                }

            }
        }

        #region 供GetMasterListInfo方法调用的多个小方法。
        //分隔信息之间的空格。
        private const string Blank = "&nbsp; &nbsp;&nbsp;&nbsp; &nbsp;";

        /// <summary>
        /// 根据RMA_Register的id，获取MasterList打印替换信息，这里包括request的部分信息。
        /// </summary>
        /// <param name="registerSysNo"></param>
        /// <param name="values"></param>
        private void getMasterListInfoRequest(int registerSysNo, Hashtable values)
        {
            string sql = "select R.RecvTime, R.Note ReceiveNote,R.RecvUserSysNo"
                         + " from RMA_Request R (nolock)"
                         + " join RMA_Request_Item I (nolock) ON I.RequestSysNo=R.SysNo"
                         + " where I.RegisterSysNo={0}";
            DataRow row = null;
            DataSet ds = SqlHelper.ExecuteDataSet(String.Format(sql, registerSysNo));
            if (Util.HasMoreRow(ds))
            {
                row = ds.Tables[0].Rows[0];
                values.Add("_dateRequestReceiveDate", getDateString(Util.TrimDateNull(row["RecvTime"])));

                values.Add("_requestNote", Util.TrimNull(row["ReceiveNote"]));
                values.Add("_requestReceiveUser", getUserName(Util.TrimIntNull(row["RecvUserSysNo"])));
            }
            else
            {
                values.Add("_dateRequestReceiveDate", Blank);
                values.Add("_requestNote", Blank);
                values.Add("_requestReceiveUser", Blank);
            }
        }


        /// <summary>
        /// 根据RMA_Register的id，获取MasterList打印替换信息，这里包括return的部分信息。
        /// </summary>
        /// <param name="registerSysNo"></param>
        /// <param name="values"></param>
        private void getMasterListInfoReturn(int registerSysNo, Hashtable values)
        {
            string sql = "select T.CreateUserSysNo, T.ReturnUserSysNo,T.ReturnTime"
                        + " from RMA_Return T (nolock) "
                         + " join RMA_Return_Item I (nolock) ON I.ReturnSysNo=T.SysNo"
                         + " where I.RegisterSysNo={0}";
            DataRow row = null;
            DataSet ds = SqlHelper.ExecuteDataSet(String.Format(sql, registerSysNo));
            if (Util.HasMoreRow(ds))
            {
                row = ds.Tables[0].Rows[0];
                values.Add("_returnInStockDate", getDateString(Util.TrimDateNull(row["ReturnTime"])));
                values.Add("_returnCreateUser", getUserName(Util.TrimIntNull(row["CreateUserSysNo"])));
                values.Add("_returnInStockUser", getUserName(Util.TrimIntNull(row["ReturnUserSysNo"])));
            }
            else
            {
                values.Add("_returnInStockDate", Blank);
                values.Add("_returnCreateUser", Blank);
                values.Add("_returnInStockUser", Blank);
            }
        }


        /// <summary>
        /// 根据RMA_Register的id，获取MasterList打印替换信息，这里包括revert的部分信息。
        /// </summary>
        /// <param name="registerSysNo"></param>
        /// <param name="values"></param>
        private void getMasterListInfoRevert(int registerSysNo, Hashtable values)
        {
            string sql = "select R.CreateUserSysNo, R.OutUserSysNo,R.OutTime,R.PackageID"
                         + ",S.ShipTypeName"
                         + " from RMA_Revert R (nolock) "
                         + " join RMA_Revert_Item I (nolock) ON I.RevertSysNo=R.SysNo"
                         + " left join shipType S (nolock) on S.SysNo=R.ShipType"
                         + " where I.RegisterSysNo={0}";
            DataRow row = null;
            string packageID = "";
            DataSet ds = SqlHelper.ExecuteDataSet(String.Format(sql, registerSysNo));
            if (Util.HasMoreRow(ds))
            {
                row = ds.Tables[0].Rows[0];
                values.Add("_shipType", Util.TrimNull(row["ShipTypeName"]));
                packageID = Util.TrimNull(row["PackageID"]);
                if (packageID != AppConst.StringNull) packageID = "," + packageID;
                values.Add("_packageID", packageID);
                values.Add("_revertOutStockDate", getDateString(Util.TrimDateNull(row["OutTime"])));
                values.Add("_revertCreateUser", getUserName(Util.TrimIntNull(row["CreateUserSysNo"])));
                values.Add("_revertOutStockUser", getUserName(Util.TrimIntNull(row["OutUserSysNo"])));
            }
            else
            {
                values.Add("_shipType", Blank);
                values.Add("_packageID", Blank);
                values.Add("_revertOutStockDate", Blank);
                values.Add("_revertCreateUser", Blank);
                values.Add("_revertOutStockUser", Blank);
            }
        }

        /// <summary>
        /// 根据userSysNo获取用户的名称。
        /// </summary>
        /// <param name="userSysNo"></param>
        /// <returns></returns>
        private string getUserName(int userSysNo)
        {
            string userName = Blank;
            if (userSysNo != AppConst.IntNull)
            {
                UserInfo user = SysManager.GetInstance().LoadUser(userSysNo);
                if (user != null) userName = user.UserName;
            }
            return userName;
        }

        /// <summary>
        /// 返回日期
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private string getDateString(DateTime date)
        {
            string dateStr = Blank;
            if (date != AppConst.DateTimeNull) dateStr = date.ToString(AppConst.DateFormat);
            return dateStr;
        }

        /// <summary>
        /// 用于前台网站RMA查询进度明细显示
        /// </summary>
        /// <param name="sysno"></param>
        /// <returns></returns>
        public DataSet GetRMARegisterInfo(int RMARegisterSysNo)
        {

            string sql = @"select rma_request.RecvTime,rma_register.IsHaveInvoice as IsHaveInvoiceShow ,rma_register.IsFullAccessory as IsFullAccessoryShow ,rma_register.IsFullPackage as IsFullPackageShow,
                                  rma_register.CheckTime,rma_register.ResponseTime,rma_register.OutBoundStatus ,
                                  RMA_Revert.OutTime,RMA_Revert.Address,ShipType.ShipTypeName as ShipType, RMA_Revert.PackageID,rma_register.sysno as RMAControlNumber
                           from rma_register (NOLOCK)
                           inner join rma_request_item (NOLOCK) on rma_register.sysno = rma_request_item.registersysno
                           inner join rma_request (NOLOCK) on rma_request_item.requestsysno = rma_request.sysno
                           left join RMA_OutBound_Item (NOLOCK) on rma_register.sysno = RMA_OutBound_Item.registersysno
                           left join RMA_OutBound (NOLOCK) on RMA_OutBound_Item.outboundsysno = RMA_OutBound.sysno   
                           left join RMA_Revert_Item (NOLOCK) on rma_register.sysno = RMA_Revert_Item.registersysno
                           left join RMA_Revert (NOLOCK) on RMA_Revert_Item.revertsysno = RMA_Revert.sysno  
                           left join ShipType (NOLOCK) on ShipType.SysNo = RMA_Revert.ShipType
                          
                           where rma_register.sysno =" + RMARegisterSysNo;

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return ds;
            ds.Tables[0].Columns.Add("IsHaveInvoice");
            ds.Tables[0].Columns.Add("IsFullAccessory");
            ds.Tables[0].Columns.Add("IsFullPackage");
            ds.Tables[0].Columns.Add("Status");
            ds.Tables[0].Columns.Add("RevertID");

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Util.TrimIntNull(dr["IsHaveInvoiceShow"]) != AppConst.IntNull)
                {
                    if (Util.TrimIntNull(dr["IsHaveInvoiceShow"]) == (int)AppEnum.YNStatus.Yes)
                        dr["IsHaveInvoice"] = "有发票";
                    else
                        dr["IsHaveInvoice"] = "无发票";
                }
                if (Util.TrimIntNull(dr["IsFullAccessoryShow"]) != AppConst.IntNull)
                {
                    if (Util.TrimIntNull(dr["IsFullAccessoryShow"]) == (int)AppEnum.YNStatus.Yes)
                        dr["IsFullAccessory"] = "附件完整";
                    else
                        dr["IsFullAccessory"] = "附件不完整";
                }
                if (Util.TrimIntNull(dr["IsFullPackageShow"]) != AppConst.IntNull)
                {
                    if (Util.TrimIntNull(dr["IsFullPackageShow"]) == (int)AppEnum.YNStatus.Yes)
                        dr["IsFullPackage"] = "包裹完整";
                    else
                        dr["IsFullPackage"] = "包裹不完整";
                }

                //if (Util.TrimIntNull(dr["OutBoundStatus"]) != AppConst.IntNull)
                //    dr["Status"] = AppEnum.GetRMAOutBoundStatus(dr["OutBoundStatus"]);
                if (dr["ShipType"].ToString().Length > 0 && dr["ShipType"].ToString().Substring(0, 4) == "ORS商城快递")
                    dr["RevertID"] = dr["RMAControlNumber"];
                else
                    dr["RevertID"] = dr["PackageID"];
                dr["Status"] = GetStatusValue(RMARegisterSysNo);


            }
            return ds;
        }
        /// <summary>
        /// 获得返修状态值
        /// </summary>
        /// <returns></returns>
        public int GetStatusValue(int registersysno)
        {
            int intStatus = 0;
            string sql = @"select  rma_request.RecvTime ,rma_register.Status ,
                                   rma_register.CheckTime ,rma_register.OutBoundStatus ,rma_register.ResponseTime ,
                                   rma_register.RevertStatus ,rma_register.RefundStatus
                           from rma_register (NOLOCK)
                           inner join rma_request_item (NOLOCK) on rma_register.sysno = rma_request_item.registersysno
                           inner join rma_request (NOLOCK) on rma_request_item.requestsysno = rma_request.sysno
                                                
                           where rma_register.sysno =" + registersysno;

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return 0;

            if (Util.TrimIntNull(ds.Tables[0].Rows[0]["Status"]) == (int)AppEnum.RMARequestStatus.Orgin)
                intStatus = 0;//没有收货
            if (Util.TrimIntNull(ds.Tables[0].Rows[0]["Status"]) == (int)AppEnum.RMARequestStatus.Handling)
                intStatus = 1;//已收货
            if (Util.TrimIntNull(ds.Tables[0].Rows[0]["Status"]) == (int)AppEnum.RMARequestStatus.Abandon)
                intStatus = 6;//已作废
            if (Util.TrimDateNull(ds.Tables[0].Rows[0]["CheckTime"]) != AppConst.DateTimeNull)
                intStatus = 2;//已检测送修
            if (Util.TrimDateNull(ds.Tables[0].Rows[0]["ResponseTime"]) != AppConst.DateTimeNull && (Util.TrimIntNull(ds.Tables[0].Rows[0]["OutBoundStatus"]) == (int)AppEnum.RMAOutBoundStatus.Responsed || Util.TrimIntNull(ds.Tables[0].Rows[0]["OutBoundStatus"]) == (int)AppEnum.RMAOutBoundStatus.PartlyResponsed))
                intStatus = 3;//已送修返回
            if (Util.TrimIntNull(ds.Tables[0].Rows[0]["RefundStatus"]) == (int)AppEnum.RMARefundStatus.Refunded)
                intStatus = 4;//已退款（处理结束）
            if (Util.TrimIntNull(ds.Tables[0].Rows[0]["RevertStatus"]) == (int)AppEnum.RMARevertStatus.Reverted)
                intStatus = 5;//已发还（处理结束）

            return intStatus;
        }
        /// <summary>
        /// 用于前台网站RMA查询进度显示的状态值
        /// </summary>
        /// <param name="statusvalue"></param>
        /// <returns></returns>
        public string GetStatusName(int statusvalue)
        {
            string statusname = null;
            switch (statusvalue)
            {
                case 0:
                    statusname = "没有收货";
                    break;
                case 1:
                    statusname = "已收货";
                    break;
                case 2:
                    statusname = "已检测送修";
                    break;
                case 3:
                    statusname = "已送修返回";
                    break;
                case 4:
                    statusname = "已退款（处理结束）";
                    break;
                case 5:
                    statusname = "已发还（处理结束）";
                    break;
                case 6:
                    statusname = "已作废";
                    break;
                default:
                    break;
            }

            return statusname;
        }

        #endregion

        //同一种产品的二手品分品相，可能有多个不同的产品，因此返回某个产品的所有二手品产品。
        public DataSet GetSecondHandProductInfo(string productID)
        {
            //productID = productID.Substring(0, 10) + "R"; old
            productID = productID.Substring(0, 7) + "R";
            string sql = @"select product.sysno,product.productID 
                            from product (nolock)
                            left join Inventory on Inventory.ProductSysNo=product.sysno
                            where productid like " + Util.ToSqlLikeStringR(productID) + "and Inventory.AvailableQty>0";
            return SqlHelper.ExecuteDataSet(sql);
        }

        public int GetRegisterUsedDays(int RegisterSysNo)
        {
            string sql = @"select DateDiff(day,isnull(so_master.AuditDeliveryDate,so_master.OutTime),isnull(rma_request.CustomerSendTime,rma_request.createtime)) as UsedDays 
                            from rma_register inner join rma_request_item on rma_register.sysno=rma_request_item.registersysno 
                            inner join rma_request on rma_request.sysno = rma_request_item.requestsysno 
                            inner join so_master on rma_request.sosysno = so_master.sysno where rma_register.sysno=" + RegisterSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                return Util.TrimIntNull(ds.Tables[0].Rows[0][0]);
            else
                return 0;
        }

        public DataSet GetContactCustomerDs(int RegisterSysNo)
        {
            string sql = @"select rc.* ,su.username
                           from RMA_ContactCustomer rc
                           left join sys_user su on rc.ContactUserSysNo=su.sysno
                           where rc.RegisterSysNo =" + RegisterSysNo + " order by rc.sysno";
            return SqlHelper.ExecuteDataSet(sql);

        }

        public void InsertContactCustomer(RMAContactCustomerInfo oInfo)
        {
            new RMAContactCustomerDac().Insert(oInfo);
        }

        public DataSet GetRMAContactUserList(DateTime dtCreateFrom, string dtNextContactTimeFrom, string dtNextContactTimeTo, int CreateUserStatus)
        {
            string sql = "select distinct su.sysno as ContactuserSysno,su.username as Contactusername from sys_user su inner join RMA_ContactCustomer rc on su.sysno=rc.contactUserSysNo where 1=1";
            if (dtCreateFrom != AppConst.DateTimeNull)
            {
                sql += " and rc.Createtime >= " + Util.ToSqlString(dtCreateFrom.ToString(AppConst.DateFormat));
            }
            if (dtNextContactTimeFrom != AppConst.StringNull)
            {
                sql += " and rc.NextContactTimeFrom >= " + Util.ToSqlString(dtNextContactTimeFrom.ToString());
            }
            else if (dtNextContactTimeTo != AppConst.StringNull)
            {
                sql += " and rc.NextContactTimeTo <=" + Util.ToSqlEndDate(dtNextContactTimeFrom.ToString());
            }
            if (CreateUserStatus != AppConst.IntNull)
            {
                sql += " and su.status=" + Util.ToSqlString(CreateUserStatus.ToString());
            }
            sql += " order by su.username";
            return SqlHelper.ExecuteDataSet(sql);
        }

        public string GetRegisterRMAPosition(int SysNo)
        {
            string sql = @"select v.RMAPosition from rma_register rr inner join product_id pid on rr.productidsysno=pid.sysno  inner join po_master po on pid.posysno=po.sysno inner join po_item poi on po.sysno=poi.posysno 
                            left join vendor v on po.vendorsysno=v.sysno where rr.sysno=" + SysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                return ds.Tables[0].Rows[0][0].ToString();
            else
                return "";
        }

        public DataSet GetRMAInspectionResultType()
        {
            string sql = "select distinct InspectionResultType from RMA_Register  where 1=1 and InspectionResultType is not null";
            return SqlHelper.ExecuteDataSet(sql);
        }
        public void UpdateRegisterReceiveType(int registerSysNo, int RegisterReceiveType)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                string sql = "select top 1 sysno , productsysno,outboundstatus,refundstatus,returnstatus,revertstatus,InspectionResultType  from rma_register (NOLOCK) where status in ( "
                    + (int)AppEnum.RMARequestStatus.Handling + ") and sysno = " + registerSysNo;

                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (!Util.HasMoreRow(ds))
                    throw new BizException("this recored's revert status or status can't allow such operation");

                DataRow dr = ds.Tables[0].Rows[0];

                if (Util.TrimIntNull(dr["refundstatus"]) != AppConst.IntNull && Util.TrimIntNull(dr["refundstatus"]) != (int)AppEnum.RMARefundStatus.Abandon)
                {
                    throw new BizException("已设置退款，不能再设置单件收货类型");
                }
                else if (Util.TrimIntNull(dr["outboundstatus"]) != AppConst.IntNull && Util.TrimIntNull(dr["outboundstatus"]) != (int)AppEnum.RMAOutBoundStatus.Abandon)
                {
                    throw new BizException("已设置送修，不能再设置单件收货类型");
                }
                else if (Util.TrimIntNull(dr["returnstatus"]) != AppConst.IntNull && Util.TrimIntNull(dr["returnstatus"]) != (int)AppEnum.RMAReturnStatus.Abandon)
                {
                    throw new BizException("已设置退货入库，不能再设置单件收货类型");
                }
                else if (Util.TrimIntNull(dr["revertstatus"]) != AppConst.IntNull && Util.TrimIntNull(dr["revertstatus"]) != (int)AppEnum.RMARevertStatus.Abandon)
                {
                    throw new BizException("已设置发还，不能再设置单件收货类型");
                }

                Hashtable ht = new Hashtable(5);
                ht.Add("SysNo", registerSysNo);
                ht.Add("RegisterReceiveType", RegisterReceiveType);
                if (RegisterReceiveType == (int)AppEnum.RMARegisterRecieveType.HostGood)
                {
                    ht.Add("InspectionResultType", "产品无故障");
                }
                this.UpdateRegister(ht);

                scope.Complete();

            }
        }

        public DataSet GetRMARegisterByProductIDSysNo(Hashtable paramHash)
        {
            string sql = @"select RMA_Register.sysno from RMA_Register 
                         left join RMA_Request_Item ri on ri.registersysno=RMA_Register.sysno
                         left join RMA_Request on ri.requestsysno=RMA_Request.sysno
                         where RMA_Register.status<>-1 and RMA_Request.status<>-1 and ProductIDSysNo=@ProductIDSysNo  and RMA_Request.sosysno=@SosysNo and RMA_Register.ProductSysNo=@ProductSysNo";

            int ProductIDSysNo = Util.TrimIntNull(paramHash["ProductIDSysNo"]);
            int SosysNo = Util.TrimIntNull(paramHash["SoSysNo"]);
            int ProductSysNo = Util.TrimIntNull(paramHash["ProductSysNo"]);
            sql = sql.Replace("@ProductIDSysNo", ProductIDSysNo.ToString());
            sql = sql.Replace("@SosysNo", SosysNo.ToString());
            sql = sql.Replace("@ProductSysNo", ProductSysNo.ToString());
            return SqlHelper.ExecuteDataSet(sql);
        }


        public void InsertRMAShift(RMAShiftInfo oParam)
        {
            new RMAShiftDac().Insert(oParam);
        }
        public void UpdateRegisterShift(string RegisterSysNo, int RMAShiftType, int ShiftSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                string sql = @"update RMA_Register set ShiftStatus=" + (int)AppEnum.ShiftStatus.Origin + "where sysno in (" + RegisterSysNo + ")";
                SqlHelper.ExecuteNonQuery(sql);

                string[] registerSysno = RegisterSysNo.Split(',');
                for (int i = 0; i < registerSysno.Length; i++)
                {
                    RMAShiftInfo oInfo = new RMAShiftInfo();
                    oInfo.RegisterSysNo = Int32.Parse(registerSysno[i]);
                    oInfo.RMAShiftType = RMAShiftType;
                    oInfo.ShiftSysNo = ShiftSysNo;
                    RMARegisterManager.GetInstance().InsertRMAShift(oInfo);
                }
                scope.Complete();
            }

        }


        public void DeleteRegisterShift(DataSet ds)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string sql = @"update RMA_Register set ShiftStatus=" + (int)AppEnum.ShiftStatus.RMAWaitingShift + "where sysno=" + Util.TrimIntNull(dr["registersysno"]);
                    SqlHelper.ExecuteNonQuery(sql);

                    string sql2 = @"delete RMA_Shift where sysno =" + Util.TrimIntNull(dr["RMAShiftSysNo"]);
                    SqlHelper.ExecuteNonQuery(sql2);
                }

                scope.Complete();
            }

        }

        /// <summary>
        /// 判断是否有收获情况选项全部选中
        /// </summary>
        /// <param name="sysno"></param>
        /// <returns></returns>
        public bool GetSetRecInfo(int sysno)
        {
            int countNum = 0;
            string sql = @"select IsHaveInvoice,IsFullAccessory,IsFullPackage
                                       from rma_register (nolock) 
                                       where sysno = " + sysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
            {
                throw new BizException("当前记录错误");
            }
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Util.TrimIntNull(dr["IsHaveInvoice"]) == AppConst.IntNull || Util.TrimIntNull(dr["IsFullAccessory"]) == AppConst.IntNull || Util.TrimIntNull(dr["IsFullPackage"]) == AppConst.IntNull)
                    countNum = countNum + 1;
            }
            if (countNum > 0)
                return true;
            else
                return false;
        }

        //设置交接状态
        public void UpdateRegisterHandoverStatus(string RegisterSysNo, int handoverStatus)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                string sql1 = @"select rr.sysno from RMA_Request rr inner join RMA_Request_Item ri on rr.sysno=ri.requestsysno where registersysno in (" + RegisterSysNo + ") and rr.RecvTime IS NULL";
                DataSet ds = SqlHelper.ExecuteDataSet(sql1);
                if (Util.HasMoreRow(ds) && ds.Tables[0].Rows.Count > 0)
                    throw new BizException("存在未收货的单件，不能设置待交接");

                string sql = "";
                if (handoverStatus == AppConst.IntNull)
                    sql = @"update RMA_Register set HandoverStatus=null where sysno in (" + RegisterSysNo + ")";
                else
                    sql = @"update RMA_Register set HandoverStatus=" + handoverStatus + "where sysno in (" + RegisterSysNo + ")";
                SqlHelper.ExecuteNonQuery(sql);

                scope.Complete();
            }

        }

        public int GetHandoverSysNoByRegister(int RMARegisterSysNo)
        {
            string sql = "select handoversysno from RMA_Handover_Item(NOLOCK),RMA_Handover (NOLOCK) where registersysno=" + RMARegisterSysNo + "and RMA_Handover.status<> @status and RMA_Handover.sysno=RMA_Handover_Item.handoversysno";
            sql = sql.Replace("@status", ((int)AppEnum.RMAHandoverStatus.Abandon).ToString());
            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return AppConst.IntNull;

            DataRow dr = ds.Tables[0].Rows[0];

            return Util.TrimIntNull(dr["handoversysno"]);

        }

        //设置交接取消接收
        public void HandoverCancelReceive(int registerSysNo)
        {
            //1 是否可以cancel receive
            //2 partly receive or all outstock

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                string sql = @"select top 1 rr.sysno as registersysno,rr.location,rh.fromlocation,rh.tolocation from rma_register rr 
                             inner join rma_handover_item ri on ri .registersysno=rr.sysno
                             inner join rma_handover rh on rh.sysno=ri.handoversysno
                            where rr.handoverstatus=@3 and rh.status in (@2,@4) and rr.status in (@0) and rr.sysno=" + registerSysNo;

                sql = sql.Replace("@0", ((int)AppEnum.RMARequestStatus.Handling).ToString());
                sql = sql.Replace("@2", ((int)AppEnum.RMAHandoverStatus.PartlyReceived).ToString());
                sql = sql.Replace("@4", ((int)AppEnum.RMAHandoverStatus.FullReceived).ToString());
                sql = sql.Replace("@3", ((int)AppEnum.RMAHandoverStatus.FullReceived).ToString());


                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (!Util.HasMoreRow(ds))
                    throw new BizException("this record's handover status or status can't allow such operation");
                else
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    if (Util.TrimIntNull(dr["location"]) != Util.TrimIntNull(dr["tolocation"]))
                        throw new BizException("操作失败，该商品已经当前的位置与交接单的接收分站不符，不能取消接收");
                }

                //获取在同一个handover 主表的 其他register, 判断receive状态，然后设置主表的状态。

                string sql2 = @"select rma_register.handoverstatus, b.handoversysno
								from
									rma_handover_item a (NOLOCK), rma_handover_item b (NOLOCK), rma_register (NOLOCK),rma_handover e(NOLOCK)
								where
									a.handoversysno = b.handoversysno
								and a.registersysno = @0
								and b.registersysno = rma_register.sysno and rma_register.sysno!=@0
                                and e.sysno=b.handoversysno and e.status <> @status";

                sql2 = sql2.Replace("@0", registerSysNo.ToString());
                sql2 = sql2.Replace("@status", ((int)AppEnum.RMAHandoverStatus.Abandon).ToString());

                bool isAllCancelReceive = true;
                int handoverSysNo = AppConst.IntNull;

                DataSet ds2 = SqlHelper.ExecuteDataSet(sql2);
                if (Util.HasMoreRow(ds2))
                {
                    foreach (DataRow dr2 in ds2.Tables[0].Rows)
                    {
                        //因为handoverSysNo 都是一样的，所以只获取一次
                        if (handoverSysNo == AppConst.IntNull)
                        {
                            handoverSysNo = Util.TrimIntNull(dr2["handoversysno"]);
                        }
                        int handoverStatus = Util.TrimIntNull(dr2["handoverstatus"]);
                        if (handoverStatus == (int)AppEnum.RMAHandoverStatus.FullReceived)
                        {
                            isAllCancelReceive = false;
                            break;
                        }
                    }

                }
                else  //只有一个交接件
                {
                    isAllCancelReceive = true;
                    string sqlReadHandoverSysNo = "select handoversysno from rma_handover_item (nolock),rma_handover (nolock) where registersysno={0} and rma_handover.sysno=rma_handover_item.handoversysno and rma_handover.status<> @status";
                    sqlReadHandoverSysNo = sqlReadHandoverSysNo.Replace("@status", ((int)AppEnum.RMAHandoverStatus.Abandon).ToString());

                    DataSet tempDS = SqlHelper.ExecuteDataSet(String.Format(sqlReadHandoverSysNo, registerSysNo));
                    if (Util.HasMoreRow(tempDS)) handoverSysNo = Util.TrimIntNull(tempDS.Tables[0].Rows[0]["handoversysno"]);
                    else throw new BizException("异常：交接单明细中没有Registersysno为" + registerSysNo.ToString() + "的单据。");

                }

                int handoverstatus = (int)AppEnum.RMAHandoverStatus.OutStock;
                if (!isAllCancelReceive)
                    handoverstatus = (int)AppEnum.RMAHandoverStatus.PartlyReceived;

                Hashtable htHandover = new Hashtable(5);
                htHandover.Add("SysNo", handoverSysNo);
                htHandover.Add("Status", handoverstatus);
                new RMAHandoverDac().UpdateMaster(htHandover);

                Hashtable ht = new Hashtable(5);
                ht.Add("SysNo", registerSysNo);
                ht.Add("HandoverStatus", (int)AppEnum.RMAHandoverStatus.OutStock);
                DataRow dr1 = ds.Tables[0].Rows[0];
                ht.Add("Location", Util.TrimIntNull(dr1["Fromlocation"]));

                this.UpdateRegister(ht);


                scope.Complete();
            }
        }

        //设置交接接收
        public void HandoverReceive(int registerSysNo)
        {
            //1 是否可以receive
            //2 partly receive or all receive
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                //                string sql = @"select top 1 sysno,location from rma_register where handoverstatus=@3 and status in (@0)
                //								and sysno in
                //								(
                //									select registersysno from rma_handover (NOLOCK), rma_handover_item (NOLOCK)
                //									where rma_handover.sysno = rma_handover_item.handoversysno and rma_handover.status in (@2,@4)
                //								) and sysno = " + registerSysNo;

                string sql = @"select top 1 rr.sysno as registersysno,rr.location,rh.fromlocation,rh.tolocation from rma_register rr 
                             inner join rma_handover_item ri on ri .registersysno=rr.sysno
                             inner join rma_handover rh on rh.sysno=ri.handoversysno
                            where rr.handoverstatus=@3 and rh.status in (@2,@4) and rr.status in (@0) and rr.sysno=" + registerSysNo;
                sql = sql.Replace("@0", ((int)AppEnum.RMARequestStatus.Handling).ToString());
                sql = sql.Replace("@2", ((int)AppEnum.RMAHandoverStatus.OutStock).ToString());
                sql = sql.Replace("@3", ((int)AppEnum.RMAHandoverStatus.OutStock).ToString());
                sql = sql.Replace("@4", ((int)AppEnum.RMAHandoverStatus.PartlyReceived).ToString());

                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (!Util.HasMoreRow(ds))
                    throw new BizException("this record's handover status or status  can't allow such operation");
                else
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    if (Util.TrimIntNull(dr["location"]) != Util.TrimIntNull(dr["fromlocation"]))
                        throw new BizException("操作失败，该商品目前的位置与交接单的起始位置不相符，不能接收！");
                }

                //获取在同一个handover 主表的 其他register, 判断receive状态，然后设置主表的状态。
                //这里需要排除自己单据的状态。
                //SQL注释：从a获取handoversysno,通过b获取所有rma_register。
                //这里是接收，因此主表状态可能是部分接收或者全部接收。
                //如果接收单中只有一个产品的话，
                string sql2 = @"select rma_register.handoverstatus, b.handoversysno
								from
									rma_handover_item a (NOLOCK), rma_handover_item b (NOLOCK), rma_register (NOLOCK),rma_handover e(NOLOCK)
								where
									a.handoversysno = b.handoversysno
								and a.registersysno = @0
								and b.registersysno = rma_register.sysno and rma_register.sysno!=@0
                                and e.sysno=b.handoversysno and e.status <> @status";

                sql2 = sql2.Replace("@0", registerSysNo.ToString());
                sql2 = sql2.Replace("@status", ((int)AppEnum.RMAHandoverStatus.Abandon).ToString());
                bool isAllReceive = true;
                int handoversysno = AppConst.IntNull;

                DataSet ds2 = SqlHelper.ExecuteDataSet(sql2);
                if (Util.HasMoreRow(ds2))
                {
                    foreach (DataRow dr2 in ds2.Tables[0].Rows)
                    {
                        //因为handoversysno 都是一样的，所以只获取一次
                        if (handoversysno == AppConst.IntNull)
                        {
                            handoversysno = Util.TrimIntNull(dr2["handoversysno"]);
                        }
                        int handoverstatus = Util.TrimIntNull(dr2["handoverstatus"]);
                        if (handoverstatus == (int)AppEnum.RMAHandoverStatus.OutStock)
                        {
                            isAllReceive = false;
                            break;
                        }
                    }
                }
                else  //接收单只有一个产品
                {
                    isAllReceive = true;
                    string sqlhandoverstatus = "select handoversysno from rma_handover_item (nolock),rma_handover (nolock) where registersysno={0} and rma_handover.sysno=rma_handover_item.handoversysno and rma_handover.status<> @status";
                    sqlhandoverstatus = sqlhandoverstatus.Replace("@status", ((int)AppEnum.RMAHandoverStatus.Abandon).ToString());

                    DataSet tempDS = SqlHelper.ExecuteDataSet(String.Format(sqlhandoverstatus, registerSysNo));
                    if (Util.HasMoreRow(tempDS))
                        handoversysno = Util.TrimIntNull(tempDS.Tables[0].Rows[0]["handoversysno"]);
                    else
                        throw new BizException("异常：交接单明细中没有Registersysno为" + registerSysNo.ToString() + "的单据。");
                }

                int handoverStatus = (int)AppEnum.RMAHandoverStatus.FullReceived;
                if (!isAllReceive)
                    handoverStatus = (int)AppEnum.RMAHandoverStatus.PartlyReceived;

                Hashtable htHandover = new Hashtable(5);
                htHandover.Add("SysNo", handoversysno);
                htHandover.Add("Status", handoverStatus);
                new RMAHandoverDac().UpdateMaster(htHandover);

                Hashtable ht = new Hashtable(5);
                ht.Add("SysNo", registerSysNo);
                ht.Add("HandoverStatus", (int)AppEnum.RMAHandoverStatus.FullReceived);

                DataRow dr1 = ds.Tables[0].Rows[0];
                ht.Add("Location", Util.TrimIntNull(dr1["tolocation"]));

                this.UpdateRegister(ht);

                scope.Complete();
            }
        }

    }
}