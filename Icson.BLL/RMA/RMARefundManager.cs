using System;
using System.Data;
using System.Collections;
using System.Text;

using Icson.Utils;
using System.Transactions;

using Icson.Objects;
using Icson.Objects.RMA;

using Icson.DBAccess;
using Icson.DBAccess.RMA;
using Icson.DBAccess.Finance;

using Icson.Objects.Basic;
using Icson.Objects.Sale;
using Icson.Objects.Finance;

using Icson.BLL.Basic;
using Icson.BLL.Sale;
using Icson.BLL.Finance;

namespace Icson.BLL.RMA
{
    /// <summary>
    /// Summary description for RMARefundManager.
    /// </summary>
    public class RMARefundManager
    {
        private RMARefundManager()
        {
        }

        private static RMARefundManager _instance;
        public static RMARefundManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new RMARefundManager();
            }
            return _instance;
        }

        private string getRefundID(int sysno)
        {
            return "R3" + sysno.ToString().PadLeft(8, '0');
        }

        private int getCurrentStatus(int sysno)
        {
            string sql = "select status from rma_refund where sysno = " + sysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
            {
                throw new BizException("no such sysno");
            }
            return Util.TrimIntNull(ds.Tables[0].Rows[0]["Status"]);
        }



        public SortedList GetWaitingSOList()
        {
            string sql = @"select 
								distinct sosysno
							from 
								rma_register (NOLOCK), rma_request (NOLOCK), rma_request_item (NOLOCK), product (NOLOCK), customer (NOLOCK)
							where
								rma_register.sysno = rma_request_item.registersysno
							and rma_request.sysno = rma_request_item.requestsysno
							and rma_register.productsysno = product.sysno
							and rma_register.refundstatus = @0
							and rma_request.customersysno = customer.sysno
							and rma_register.sysno not in
							(
								select rma_refund_item.registersysno
								from
									rma_refund (NOLOCK), rma_refund_item (NOLOCK)
								where
									rma_refund.sysno = rma_refund_item.refundsysno
								and rma_refund.status <> @1
							)";

            sql = sql.Replace("@0", ((int)AppEnum.RMARefundStatus.WaitingRefund).ToString());
            sql = sql.Replace("@1", ((int)AppEnum.RMARefundStatus.Abandon).ToString());

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;

            SortedList sl = new SortedList(5);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sl.Add(Util.TrimIntNull(dr["sosysno"]), null);
            }
            return sl;
        }

        public DataSet GetRefundWaitingDs(int soSysNo)
        {
            string sql = @"select 
								registerSysNo, sosysno, customername, productid, productname 
							from 
								rma_register (NOLOCK), rma_request (NOLOCK), rma_request_item (NOLOCK), product (NOLOCK), customer (NOLOCK)
							where
								rma_register.sysno = rma_request_item.registersysno
							and rma_request.sysno = rma_request_item.requestsysno
							and rma_register.productsysno = product.sysno
							and rma_register.refundstatus = @0
							and rma_request.customersysno = customer.sysno
							and rma_register.sysno not in
							(
								select rma_refund_item.registersysno
								from
									rma_refund (NOLOCK), rma_refund_item (NOLOCK)
								where
									rma_refund.sysno = rma_refund_item.refundsysno
								and rma_refund.status <> @1
							)";
            sql = sql.Replace("@0", ((int)AppEnum.RMARefundStatus.WaitingRefund).ToString());
            sql = sql.Replace("@1", ((int)AppEnum.RMARefundStatus.Abandon).ToString());

            if (soSysNo != AppConst.IntNull)
            {
                sql += " and sosysno = " + soSysNo;
            }

            return SqlHelper.ExecuteDataSet(sql);
        }

        public void Create(RMARefundInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                //1 get register item with the same so sysno
                //2 load sosysno,
                //3 foreach registersysno calc so, set some default value
                //4 calc master

                string sql_register = @"select rma_register.sysno, productsysno
										from
											rma_request (NOLOCK), rma_request_item (NOLOCK), rma_register (NOLOCK)
										where
											rma_request.sysno = rma_request_item.requestsysno
										and rma_request.sosysno = @sosysno
										and rma_register.refundstatus = @0
										and rma_register.sysno = rma_request_item.registersysno
										and rma_register.sysno not in
										(
											select rma_refund_item.registersysno
											from
												rma_refund (NOLOCK), rma_refund_item (NOLOCK)
											where
												rma_refund.sysno = rma_refund_item.refundsysno
											and rma_refund.status <> @1
										)";
                sql_register = sql_register.Replace("@sosysno", oParam.SOSysNo.ToString());
                sql_register = sql_register.Replace("@0", ((int)AppEnum.RMARefundStatus.WaitingRefund).ToString());
                sql_register = sql_register.Replace("@1", ((int)AppEnum.RMARefundStatus.Abandon).ToString());

                DataSet ds_register = SqlHelper.ExecuteDataSet(sql_register);
                if (!Util.HasMoreRow(ds_register))
                    throw new BizException("no valid register item can be inserted into refund sheet");

                SOInfo oSO = SaleManager.GetInstance().LoadSO(oParam.SOSysNo);

                //get something from so
                oParam.CustomerSysNo = oSO.CustomerSysNo;

                //set ship price default 设置缺省 赔偿运费
                oParam.CompensateShipPrice = 0;

                foreach (DataRow dr in ds_register.Tables[0].Rows)
                {
                    RMARefundItemInfo refundItem = new RMARefundItemInfo();
                    //refundItem.RefundSysNo 等插入的时候赋值
                    refundItem.RegisterSysNo = Util.TrimIntNull(dr["sysno"]);

                    int productSysNo = Util.TrimIntNull(dr["productSysNo"]);
                    SOItemInfo soItem = oSO.ItemHash[productSysNo] as SOItemInfo;

                    refundItem.OrgPrice = soItem.Price;
                    refundItem.UnitDiscount = Decimal.Round(soItem.DiscountAmt / soItem.Quantity, 2);
                    refundItem.ProductValue = refundItem.OrgPrice + refundItem.UnitDiscount;

                    refundItem.OrgPoint = soItem.Point;
                    refundItem.RefundPrice = refundItem.ProductValue; //缺省设置为全额
                    refundItem.PointType = soItem.PointType;
                    refundItem.RefundPriceType = (int)AppEnum.ReturnPriceType.TenPercentsOff;  //缺省设置为10%折价退款
                    //refundItem.RefundCash = 0;
                    //refundItem.RefundPoint = 0;
                    oParam.ItemHash.Add(refundItem.RegisterSysNo, refundItem);
                }

                Calc(oParam);

                //inster master
                oParam.SysNo = SequenceDac.GetInstance().Create("RMA_Refund_Sequence");
                oParam.RefundID = getRefundID(oParam.SysNo);
                oParam.RefundPayType = (int)AppEnum.RMARefundPayType.CashRefund;  //默认为现金退款
                new RMARefundDac().InsertMaster(oParam);

                //insert item;
                foreach (RMARefundItemInfo oItem in oParam.ItemHash.Values)
                {
                    oItem.RefundSysNo = oParam.SysNo;
                    new RMARefundDac().InsertItem(oItem);
                }

                scope.Complete();
            }
        }

        public void ReCalc(RMARefundInfo oRefund)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                //必须是初始状态
                if (getCurrentStatus(oRefund.SysNo) != (int)AppEnum.RMARefundStatus.WaitingRefund)
                    throw new BizException("status is not origin now,  re calc failed");

                Calc(oRefund);
                //update master;

                Hashtable htMaster = new Hashtable(5);
                htMaster.Add("SysNo", oRefund.SysNo);
                htMaster.Add("CompensateShipPrice", oRefund.CompensateShipPrice);
                htMaster.Add("OrgCashAmt", oRefund.OrgCashAmt);
                htMaster.Add("OrgPointAmt", oRefund.OrgPointAmt);
                htMaster.Add("DeductPointFromAccount", oRefund.DeductPointFromAccount);
                htMaster.Add("DeductPointFromCurrentCash", oRefund.DeductPointFromCurrentCash);
                htMaster.Add("CashAmt", oRefund.CashAmt);
                htMaster.Add("PointAmt", oRefund.PointAmt);

                new RMARefundDac().UpdateMaster(htMaster);

                foreach (RMARefundItemInfo oItem in oRefund.ItemHash.Values)
                {
                    Hashtable htItem = new Hashtable(5);

                    htItem.Add("SysNo", oItem.SysNo);

                    htItem.Add("OrgPrice", oItem.OrgPrice);
                    htItem.Add("UnitDiscount", oItem.UnitDiscount);
                    htItem.Add("ProductValue", oItem.ProductValue);
                    htItem.Add("RefundPrice", oItem.RefundPrice);
                    htItem.Add("PointType", oItem.PointType);
                    htItem.Add("RefundCash ", oItem.RefundCash);
                    htItem.Add("RefundPoint", oItem.RefundPoint);
                    htItem.Add("RefundPriceType", oItem.RefundPriceType);

                    new RMARefundDac().UpdateItem(htItem);
                }

                scope.Complete();
            }

        }

        //计算RefundCost，为了计算SalesReport
        public DataRow calcRefundCost(int sysno)
        {
            string sql = @"select V_SO_Item.Cost , V_SO_Item.Point
                           from  RMA_Refund_Item (NOLOCK)
                           inner join RMA_Register (NOLOCK) on RMA_Register.SysNo = RMA_Refund_Item.RegisterSysNo                          
                           inner join RMA_Refund (NOLOCK) on RMA_Refund_Item.RefundSysNo = RMA_Refund.SysNo                         
                           inner join V_SO_Item (NOLOCK) on (V_SO_Item.sosysno = RMA_Refund.sosysno and V_SO_Item.ProductSysNo = RMA_Register.ProductSysNo)
                           where RMA_Refund_Item.SysNo = " + sysno;

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                DataRow dr = ds.Tables[0].Rows[0];
                return dr;
            }
            else
                return null;
        }

        private void Calc(RMARefundInfo oRefund)
        {
            //清空
            oRefund.OrgPointAmt = 0;
            oRefund.OrgCashAmt = 0;


            SOInfo oSO = SaleManager.GetInstance().LoadSO(oRefund.SOSysNo);

            #region calc money/point radio

            decimal pointOnly = 0; //仅积分支付部分对应的价值
            decimal cashOnly = 0; //仅现金支付部分对应的价值

            foreach (SOItemInfo soItem in oSO.ItemHash.Values)
            {
                if (soItem.PointType == (int)AppEnum.ProductPayType.PointPayOnly)
                {
                    pointOnly += soItem.Price * soItem.Quantity + soItem.DiscountAmt;
                }
                else if (soItem.PointType == (int)AppEnum.ProductPayType.MoneyPayOnly)
                {
                    cashOnly += soItem.Price * soItem.Quantity + soItem.DiscountAmt;
                }
            }
            //计算可混合支付部分中，用户支付的比例。
            //货款是SOAmt = CashPay + PointPay(单位为积分，int), 如果直接+，积分是需要转换的。
            //CashPay中去除cashOnly == 混合部分用户支付的现金。
            // ----------------------------
            // | cashOnly     | pointOnly |
            // |              |__________ |
            // |--------------|           |
            // |              |    B      |
            // |              |___________|
            // |       A      | 
            // |              |
            // |              |
            // |              |
            // |______________|

            // 可以认为左边一块是CashPay, 右边一块是PointPay
            // 我们要求的比例是 a/(a+b) = (cashPay-cashOnly)/(soAmt-pointOnly-cashOnly);

            //如果oSO.SOAmt-pointOnly-cashOnly=0 ，可能是MoneyPayOnly或者PointPayOnly，这种情况在下面的计算中
            //不需要用到SOCashPointRate的值。所以设定值为0。 
            if ((oSO.SOAmt - pointOnly - cashOnly) == 0)
                oRefund.SOCashPointRate = 0;
            else
                oRefund.SOCashPointRate = Decimal.Round((oSO.CashPay - cashOnly) / (oSO.SOAmt - pointOnly - cashOnly), 4);
                //oRefund.SOCashPointRate = Decimal.Round((oSO.CashPay - cashOnly) / (oSO.SOAmt - oSO.promotionValue - pointOnly - cashOnly), 4);  //这里在分母的地方补充了-oSO.promotionValue
            #endregion

            foreach (RMARefundItemInfo oItem in oRefund.ItemHash.Values)
            {
                //根据 point type 计算 应该退的现金和金额

                if ((oItem.ProductValue + 20) < oItem.RefundPrice)
                    throw new BizException("退款金额不能大于商品价格+20元运费");

                if (oItem.PointType == (int)AppEnum.ProductPayType.BothSupported)
                {
                    oItem.RefundCash = oItem.RefundPrice * oRefund.SOCashPointRate;
                    oItem.RefundPoint = Convert.ToInt32(Decimal.Round(oItem.RefundPrice * (1 - oRefund.SOCashPointRate) * AppConst.ExchangeRate, 0))
                                    - oItem.OrgPoint;
                }
                else if (oItem.PointType == (int)AppEnum.ProductPayType.MoneyPayOnly)
                {
                    oItem.RefundCash = oItem.RefundPrice;
                    oItem.RefundPoint = -1 * oItem.OrgPoint;
                }
                else
                {
                    oItem.RefundCash = 0m;
                    oItem.RefundPoint = Convert.ToInt32(Decimal.Round(oItem.RefundPrice * AppConst.ExchangeRate, 0)) - oItem.OrgPoint;
                }

                oRefund.OrgCashAmt += oItem.RefundCash;
                oRefund.OrgPointAmt += oItem.RefundPoint;
            }

            oRefund.DeductPointFromAccount = 0;
            oRefund.DeductPointFromCurrentCash = 0;
            if (oRefund.OrgPointAmt < 0)
            {
                CustomerInfo oCustomer = CustomerManager.GetInstance().Load(oRefund.CustomerSysNo);
                if (oRefund.OrgPointAmt * -1 < oCustomer.ValidScore)
                {
                    oRefund.DeductPointFromAccount = oRefund.OrgPointAmt * -1;
                }
                else
                {
                    oRefund.DeductPointFromAccount = oCustomer.ValidScore;
                    oRefund.DeductPointFromCurrentCash = Decimal.Round((oRefund.OrgPointAmt * -1 - oCustomer.ValidScore) / AppConst.ExchangeRate, 2);
                }
            }
            else
            {
            }

            oRefund.CashAmt = oRefund.OrgCashAmt - oRefund.DeductPointFromCurrentCash + oRefund.CompensateShipPrice;
            oRefund.PointAmt = oRefund.OrgPointAmt + oRefund.DeductPointFromAccount + Convert.ToInt32(oRefund.DeductPointFromCurrentCash * AppConst.ExchangeRate);
        }

        private void map(RMARefundInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.RefundID = Util.TrimNull(tempdr["RefundID"]);
            oParam.SOSysNo = Util.TrimIntNull(tempdr["SOSysNo"]);
            oParam.CustomerSysNo = Util.TrimIntNull(tempdr["CustomerSysNo"]);
            oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
            oParam.CreateUserSysNo = Util.TrimIntNull(tempdr["CreateUserSysNo"]);
            oParam.AuditTime = Util.TrimDateNull(tempdr["AuditTime"]);
            oParam.AuditUserSysNo = Util.TrimIntNull(tempdr["AuditUserSysNo"]);
            oParam.RefundTime = Util.TrimDateNull(tempdr["RefundTime"]);
            oParam.RefundUserSysNo = Util.TrimIntNull(tempdr["RefundUserSysNo"]);
            oParam.CompensateShipPrice = Util.TrimDecimalNull(tempdr["CompensateShipPrice"]);
            oParam.SOCashPointRate = Util.TrimDecimalNull(tempdr["SOCashPointRate"]);
            oParam.OrgCashAmt = Util.TrimDecimalNull(tempdr["OrgCashAmt"]);
            oParam.OrgPointAmt = Util.TrimIntNull(tempdr["OrgPointAmt"]);
            oParam.DeductPointFromAccount = Util.TrimIntNull(tempdr["DeductPointFromAccount"]);
            oParam.DeductPointFromCurrentCash = Util.TrimDecimalNull(tempdr["DeductPointFromCurrentCash"]);
            oParam.CashAmt = Util.TrimDecimalNull(tempdr["CashAmt"]);
            oParam.PointAmt = Util.TrimIntNull(tempdr["PointAmt"]);
            oParam.RefundPayType = Util.TrimIntNull(tempdr["RefundPayType"]);
            oParam.Note = Util.TrimNull(tempdr["Note"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
        }

        private void map(RMARefundItemInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.RefundSysNo = Util.TrimIntNull(tempdr["RefundSysNo"]);
            oParam.RegisterSysNo = Util.TrimIntNull(tempdr["RegisterSysNo"]);
            oParam.OrgPrice = Util.TrimDecimalNull(tempdr["OrgPrice"]);
            oParam.UnitDiscount = Util.TrimDecimalNull(tempdr["UnitDiscount"]);
            oParam.ProductValue = Util.TrimDecimalNull(tempdr["ProductValue"]);
            oParam.OrgPoint = Util.TrimIntNull(tempdr["OrgPoint"]);
            oParam.RefundPrice = Util.TrimDecimalNull(tempdr["RefundPrice"]);
            oParam.PointType = Util.TrimIntNull(tempdr["PointType"]);
            oParam.RefundCash = Util.TrimDecimalNull(tempdr["RefundCash"]);
            oParam.RefundPoint = Util.TrimIntNull(tempdr["RefundPoint"]);
            oParam.RefundCost = Util.TrimDecimalNull(tempdr["RefundCost"]);

        }
        public RMARefundInfo Load(int sysno)
        {

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                string sql = "select * from rma_refund (NOLOCK) where sysno = " + sysno;
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (!Util.HasMoreRow(ds))
                    throw new BizException("does not exist such sysno");

                RMARefundInfo oMaster = new RMARefundInfo();

                map(oMaster, ds.Tables[0].Rows[0]);

                string sql_item = "select * from rma_refund_item (NOLOCK) where refundsysno =" + oMaster.SysNo;
                DataSet ds_item = SqlHelper.ExecuteDataSet(sql_item);
                if (Util.HasMoreRow(ds_item))
                {
                    foreach (DataRow dr in ds_item.Tables[0].Rows)
                    {
                        RMARefundItemInfo oItem = new RMARefundItemInfo();
                        map(oItem, dr);
                        oMaster.ItemHash.Add(oItem.RegisterSysNo, oItem);
                    }
                }

                scope.Complete();

                return oMaster;

            }

        }
        /// <summary>
        /// SoIncome使用,可按客户搜索该客户的全部RO单
        /// </summary>
        /// <param name="RefundID"></param>
        /// <returns></returns>
        public RMARefundInfo LoadRMARefund(string RefundID)
        {
            string sql = "select * from rma_refund (nolock) where RefundID = '" + RefundID + "'";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            RMARefundInfo oMaster = new RMARefundInfo();
            if (Util.HasMoreRow(ds))
            {

                map(oMaster, ds.Tables[0].Rows[0]);

                string sql_item = "select * from rma_refund_item (nolock) where refundsysno =" + oMaster.SysNo;
                DataSet ds_item = SqlHelper.ExecuteDataSet(sql_item);
                if (Util.HasMoreRow(ds_item))
                {
                    foreach (DataRow dr in ds_item.Tables[0].Rows)
                    {
                        RMARefundItemInfo oItem = new RMARefundItemInfo();
                        map(oItem, dr);
                        oMaster.ItemHash.Add(oItem.RegisterSysNo, oItem);
                    }
                }
            }
            else
            {
                oMaster = null;
            }
            return oMaster;
        }
        /// <summary>
        /// SoIncome使用,可按客户搜索该客户的全部RO单
        /// </summary>
        /// <param name="sysno"></param>
        /// <returns></returns>
        public RMARefundInfo LoadRMARefund(int sysno)
        {
            string sql = "select * from rma_refund (nolock) where sysno = " + sysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            RMARefundInfo oMaster = new RMARefundInfo();
            if (Util.HasMoreRow(ds))
            {
                map(oMaster, ds.Tables[0].Rows[0]);

                string sql_item = "select * from rma_refund_item (nolock) where refundsysno =" + oMaster.SysNo;
                DataSet ds_item = SqlHelper.ExecuteDataSet(sql_item);
                if (Util.HasMoreRow(ds_item))
                {
                    foreach (DataRow dr in ds_item.Tables[0].Rows)
                    {
                        RMARefundItemInfo oItem = new RMARefundItemInfo();
                        map(oItem, dr);
                        oMaster.ItemHash.Add(oItem.RegisterSysNo, oItem);
                    }
                }
            }
            else
            {
                oMaster = null;
            }
            return oMaster;
        }
        public DataSet GetRefundItemDs(int refundSysNo)
        {
            string sql = @"select 
								rma_refund_item.*, productname, productid ,rma_register.productsysno ,rma_refund_item.refundpricetype,rma_register.refundInfo
							from rma_refund (NOLOCK), rma_refund_item (NOLOCK), product (NOLOCK), rma_register (NOLOCK)
							where
								rma_refund.sysno = rma_refund_item.refundsysno
							and rma_refund_item.registersysno = rma_register.sysno
							and rma_register.productsysno = product.sysno and rma_refund.sysno=" + refundSysNo;
            return SqlHelper.ExecuteDataSet(sql);

        }

        public DataSet GetRefundDs(Hashtable paramHash)
        {
            string sql = @" select 
								rma.*, a.username as CreateName, b.username as AuditName, c.username as RefundName, customername ,customer.VIPRank  
							from 
								rma_refund rma (NOLOCK), sys_user a (NOLOCK), sys_user b (NOLOCK), sys_user c (NOLOCK), customer (NOLOCK) 
							where 
								rma.createusersysno *= a.sysno
							and rma.auditusersysno *= b.sysno
							and rma.refundusersysno *= c.sysno
							and rma.customersysno = customer.sysno
                             ";

            if (paramHash != null && paramHash.Count != 0)
            {
                StringBuilder sb = new StringBuilder(100);
                foreach (string key in paramHash.Keys)
                {
                    sb.Append(" and ");
                    object item = paramHash[key];
                    if (key == "DateFrom")
                    {
                        sb.Append("rma.CreateTime >= ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "DateTo")
                    {
                        sb.Append("rma.CreateTime <= ").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "RefundFrom")
                    {
                        sb.Append("rma.RefundTime >=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "RefundTo")
                    {
                        sb.Append("rma.RefundTime <=").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "SoSysNo")
                    {
                        sb.Append(" rma.sosysno in ( select top 1 sysno from v_so_master where sysno = " + Util.SafeFormat(item.ToString()) + " or soid=" + Util.ToSqlString(item.ToString()) + ")");
                    }
                    else if (key == "CustomerSysNo")
                    {
                        sb.Append("rma.CustomerSysNo = ").Append(Util.SafeFormat(item.ToString()));
                    }
                    else if (key == "Status")
                    {
                        sb.Append("rma.Status = ").Append(item.ToString());
                    }
                    else if (key == "ProductSysNo")
                    {
                        sb.Append(" exists (select top 1 rma_register.sysno from rma_register , rma_refund_item where rma_register.sysno = rma_refund_item.registersysno and rma_refund_item.refundsysno = rma.sysno and productsysno = ").Append(item.ToString()).Append(" )");
                    }
                    else if (key == "IsVIP")
                    {
                        if (Util.TrimIntNull(item) == (int)AppEnum.YNStatus.Yes)
                            sb.Append(" (").Append("customer.VIPRank").Append("=").Append((int)AppEnum.CustomerVIPRank.AutoVIP).Append(" or ").Append("customer.VIPRank").Append("=").Append((int)AppEnum.CustomerVIPRank.ManualVIP).Append(" ) ");
                        else if (Util.TrimIntNull(item) == (int)AppEnum.YNStatus.No)
                            sb.Append(" (").Append("customer.VIPRank").Append("=").Append((int)AppEnum.CustomerVIPRank.AutoNonVIP).Append(" or ").Append("customer.VIPRank").Append("=").Append((int)AppEnum.CustomerVIPRank.ManualNonVIP).Append(" )");
                    }
                    else if (key == "RefundPayType")
                    {
                        sb.Append("rma.RefundPayType in( ").Append(item.ToString() + ")");
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

            sql += " order by rma.sysno desc";
            return SqlHelper.ExecuteDataSet(sql);
        }

        public void UpdateMaster(Hashtable paramHt)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                //必须是初始状态
                if (getCurrentStatus((int)paramHt["SysNo"]) != (int)AppEnum.RMARefundStatus.WaitingRefund)
                    throw new BizException("status is not waiting refund now,  update failed");

                if (1 != new RMARefundDac().UpdateMaster(paramHt))
                    throw new BizException("expected one-row update failed, update failed ");

                scope.Complete();
            }
        }

        public void UpdateMasterMemo(Hashtable paramHt)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                if (1 != new RMARefundDac().UpdateMaster(paramHt))
                    throw new BizException("expected one-row update failed, update failed ");

                scope.Complete();
            }
        }

        public void UpdateVoucher(Hashtable paramHt)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                if (1 != new RMARefundDac().UpdateMaster(paramHt))
                    throw new BizException("expected one-row update failed, update failed ");

                SOIncomeInfo soIncome = SOIncomeManager.GetInstance().LoadValid((int)AppEnum.SOIncomeOrderType.RO, Util.TrimIntNull(paramHt["SysNo"]));
                SOIncomeVoucherInfo oVInfo = new SOIncomeVoucherInfo();
                oVInfo.FSISysNo = soIncome.SysNo;
                SOIncomeVoucherInfo oParam = SOIncomeManager.GetInstance().LoadSOIncomeVoucher(oVInfo);

                oParam.VoucherID = paramHt["VoucherID"].ToString();
                oParam.VoucherTime = Util.TrimDateNull(paramHt["VoucherTime"].ToString());
                oParam.DateStamp = DateTime.Now;
                SOIncomeManager.GetInstance().UpdateSOIncomeVoucher(oParam);


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

                //必须是初始状态
                if (getCurrentStatus(masterSysNo) != (int)AppEnum.RMARefundStatus.WaitingRefund)
                    throw new BizException("status is not origin now,  abandon failed");

                Hashtable ht = new Hashtable(5);
                ht.Add("SysNo", masterSysNo);
                ht.Add("Status", (int)AppEnum.RMARefundStatus.Abandon);

                if (1 != new RMARefundDac().UpdateMaster(ht))
                    throw new BizException("expected one-row update failed, update failed ");

                //改变Register中相应的RefundStatus
                string sql = "select RegisterSysNo from RMA_Refund_Item where RefundSysNo =" + masterSysNo;
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (Util.HasMoreRow(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Hashtable htRegister = new Hashtable();
                        htRegister.Add("SysNo", Util.TrimIntNull(dr["RegisterSysNo"]));
                        htRegister.Add("RefundStatus", (int)AppEnum.RMARefundStatus.Abandon);
                        RMARegisterManager.GetInstance().UpdateRegister(htRegister);
                    }
                }

                scope.Complete();
            }

        }

        public void Audit(int masterSysNo, int userSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                //必须是初始状态
                if (getCurrentStatus(masterSysNo) != (int)AppEnum.RMARefundStatus.WaitingRefund)
                    throw new BizException("status is not origin now,  audit failed");

                Hashtable ht = new Hashtable(5);
                ht.Add("SysNo", masterSysNo);
                ht.Add("AuditUserSysNo", userSysNo);
                ht.Add("AuditTime", DateTime.Now);
                ht.Add("Status", (int)AppEnum.RMARefundStatus.Audited);

                if (1 != new RMARefundDac().UpdateMaster(ht))
                    throw new BizException("expected one-row update failed, update failed ");

//                string sql = @"select
//									registersysno
//								from
//									rma_refund (NOLOCK), rma_refund_item (NOLOCK)
//								where
//									rma_refund.sysno = rma_refund_item.refundsysno and rma_refund.sysno=" + masterSysNo;
//                DataSet ds = SqlHelper.ExecuteDataSet(sql);
//                if (Util.HasMoreRow(ds))
//                {
//                    //调用Register的更新
//                    foreach (DataRow dr in ds.Tables[0].Rows)
//                    {

//                        Hashtable htRegister = new Hashtable(2);
//                        htRegister.Add("SysNo", Util.TrimIntNull(dr["registersysno"]));
//                        htRegister.Add("RefundStatus", (int)AppEnum.RMARefundStatus.Audited);
//                        RMARegisterManager.GetInstance().UpdateRegister(htRegister);
//                    }
//                }
                string sql = @"select
                                   rma_refund_item.sysno ,registersysno ,revertstatus
                                from
                                   rma_refund_item (NOLOCK)
                                   inner join rma_register (NOLOCK) on rma_refund_item.registersysno = rma_register.sysno
                                where
                                   rma_refund_item.refundsysno =" + masterSysNo;
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (Util.HasMoreRow(ds))
                {
                    //调用Register的更新
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {

                        Hashtable htItem = new Hashtable(2);
                        DataRow drCost = calcRefundCost(Util.TrimIntNull(dr["SysNo"]));
                        htItem.Add("SysNo", Util.TrimIntNull(dr["SysNo"]));
                        htItem.Add("RefundCost", Util.TrimDecimalNull(drCost["Cost"]));
                        htItem.Add("RefundCostPoint", Util.TrimDecimalNull(drCost["Point"]) / AppConst.ExchangeRate);

                        new RMARefundDac().UpdateItem(htItem);

                        Hashtable htRegister = new Hashtable(2);
                        htRegister.Add("SysNo", Util.TrimIntNull(dr["registersysno"]));
                        htRegister.Add("RefundStatus", (int)AppEnum.RMARefundStatus.Audited);
                        htRegister.Add("Cost", Util.TrimDecimalNull(drCost["Cost"]));
                        RMARegisterManager.GetInstance().UpdateRegister(htRegister);
                    }
                }



                scope.Complete();
            }

        }

        public void CancelAudit(int masterSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                //必须是初始状态
                if (getCurrentStatus(masterSysNo) != (int)AppEnum.RMARefundStatus.Audited)
                    throw new BizException("status is not audited now,  audit failed");

                Hashtable ht = new Hashtable(5);
                ht.Add("SysNo", masterSysNo);
                ht.Add("Status", (int)AppEnum.RMARefundStatus.WaitingRefund);

                if (1 != new RMARefundDac().UpdateMaster(ht))
                    throw new BizException("expected one-row update failed, update failed ");

                string sql = @"select
									registersysno
								from
									rma_refund (NOLOCK), rma_refund_item (NOLOCK)
								where
									rma_refund.sysno = rma_refund_item.refundsysno and rma_refund.sysno=" + masterSysNo;
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (Util.HasMoreRow(ds))
                {
                    //调用Register的更新
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {

                        Hashtable htRegister = new Hashtable(2);
                        htRegister.Add("SysNo", Util.TrimIntNull(dr["registersysno"]));
                        htRegister.Add("RefundStatus", (int)AppEnum.RMARefundStatus.WaitingRefund);
                        RMARegisterManager.GetInstance().UpdateRegister(htRegister);
                    }
                }

                scope.Complete();
            }
        }

        public void Refund(int masterSysNo, int userSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                //必须是初始状态
                RMARefundInfo oInfo = Load(masterSysNo);
                if (oInfo.Status != (int)AppEnum.RMARefundStatus.Audited)
                    throw new BizException("status is not audit now,  refund failed");

                //更新用户积分
                int affectedPoint = 0;
                affectedPoint = -1 * oInfo.DeductPointFromAccount + oInfo.PointAmt;
                if (oInfo.RefundPayType == (int)AppEnum.RMARefundPayType.TransferPointRefund)
                {
                    affectedPoint += Convert.ToInt32(Decimal.Round(oInfo.CashAmt * AppConst.ExchangeRate, 0));
                }

                if (affectedPoint != 0)
                {
                    //写积分日志
                    //CustomerManager.GetInstance().SetCustomerPoint(oInfo.CustomerSysNo, affectedPoint, PointLogType.ReturnProduct, oInfo.SysNo.ToString(), "");
                    PointManager.GetInstance().SetScore(oInfo.CustomerSysNo, affectedPoint, (int)AppEnum.PointLogType.ReturnProduct , oInfo.SysNo.ToString());
                }

                Hashtable ht = new Hashtable(5);
                ht.Add("SysNo", masterSysNo);
                ht.Add("RefundUserSysNo", userSysNo);
                ht.Add("RefundTime", DateTime.Now);
                ht.Add("Status", (int)AppEnum.RMARefundStatus.Refunded);

                if (1 != new RMARefundDac().UpdateMaster(ht))
                    throw new BizException("expected one-row update failed, update failed ");

                string sql = @"select
                                   rma_refund_item.sysno ,registersysno ,revertstatus
                                from
                                   rma_refund_item (NOLOCK)
                                   inner join rma_register (NOLOCK) on rma_refund_item.registersysno = rma_register.sysno
                                where
                                   rma_refund_item.refundsysno =" + masterSysNo;
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (Util.HasMoreRow(ds))
                {
                    //调用Register的更新
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (Util.TrimIntNull(dr["revertstatus"]) == (int)AppEnum.RMARevertStatus.Reverted)
                            throw new BizException("已经发还货物给客户，退款失败！请与系统管理员联系。");

                        Hashtable htRegister = new Hashtable(2);
                        htRegister.Add("SysNo", Util.TrimIntNull(dr["registersysno"]));
                        htRegister.Add("RefundStatus", (int)AppEnum.RMARefundStatus.Refunded);
                        htRegister.Add("OwnBy", (int)AppEnum.RMAOwnBy.Icson);
                        RMARegisterManager.GetInstance().UpdateRegister(htRegister);
                    }
                }

                //如果无有效的收款单-->生成soincome(ro, origin)
                SOIncomeInfo soIncome = SOIncomeManager.GetInstance().LoadValid((int)AppEnum.SOIncomeOrderType.RO, oInfo.SysNo);
                if (soIncome == null)//无有效收款单，生成收款单
                {
                    soIncome = new SOIncomeInfo();
                    soIncome.OrderType = (int)AppEnum.SOIncomeOrderType.RO;
                    soIncome.OrderSysNo = oInfo.SysNo;

                    decimal affectedCash = 0m;
                    if (oInfo.RefundPayType != (int)AppEnum.RMARefundPayType.TransferPointRefund)
                        affectedCash = -1 * oInfo.CashAmt;
                    soIncome.OrderAmt = soIncome.IncomeAmt = affectedCash;

                    soIncome.IncomeStyle = (int)AppEnum.SOIncomeStyle.RO;
                    soIncome.IncomeUserSysNo = userSysNo;
                    soIncome.IncomeTime = DateTime.Now;
                    soIncome.Status = (int)AppEnum.SOIncomeStatus.Origin;
                    SOIncomeManager.GetInstance().Insert(soIncome);
                    LogInfo log = new LogInfo();
                    log.OptIP = AppConst.SysIP;
                    log.OptUserSysNo = AppConst.SysUser;
                    log.OptTime = DateTime.Now;
                    log.TicketType = (int)AppEnum.LogType.Finance_SOIncome_Add;
                    log.TicketSysNo = soIncome.SysNo;
                    LogManager.GetInstance().Write(log);
                }

                scope.Complete();
            }
        }

        public void CancelRefund(int masterSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                //必须是已退款状态
                RMARefundInfo oInfo = Load(masterSysNo);
                if (oInfo.Status != (int)AppEnum.RMARefundStatus.Refunded)
                    throw new BizException("status is not refunded now,  cancel refund failed");

                DataSet ds = RMARefundManager.GetInstance().GetRefundItemDs(masterSysNo);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    DataRow drRR = RMARegisterManager.GetInstance().GetRegisterRow(Util.TrimIntNull(dr["RegisterSysNo"]));
                    if (Util.TrimIntNull(drRR["Status"]) != (int)AppEnum.RMARequestStatus.Handling)
                    {
                        throw new BizException("单件状态不是处理中，无法进行取消退款操作");
                    }
                }
               
                //作废未确认的有效收款单, 如果已经财务确认会抛出异常
                SOIncomeManager.GetInstance().ROCancelReturn(masterSysNo);

                //更新用户积分
                int affectedPoint = 0;
                affectedPoint = -1 * oInfo.DeductPointFromAccount + oInfo.PointAmt;
                if (oInfo.RefundPayType == (int)AppEnum.RMARefundPayType.TransferPointRefund)
                {
                    affectedPoint += Convert.ToInt32(Decimal.Round(oInfo.CashAmt * AppConst.ExchangeRate, 0));
                }

                if (affectedPoint != 0)
                {
                    //写积分日志
                    //CustomerManager.GetInstance().SetCustomerPoint(oInfo.CustomerSysNo, -1 * affectedPoint, PointLogType.ReturnProduct, oInfo.SysNo.ToString(), "");
                    PointManager.GetInstance().SetScore(oInfo.CustomerSysNo, -1*affectedPoint, (int)AppEnum.PointLogType.ReturnProduct , oInfo.SysNo.ToString());
                }

                Hashtable ht = new Hashtable(5);
                ht.Add("SysNo", masterSysNo);
                ht.Add("Status", (int)AppEnum.RMARefundStatus.Audited);

                if (1 != new RMARefundDac().UpdateMaster(ht))
                    throw new BizException("expected one-row update failed, update failed ");

                string sql = @"select
									registersysno
								from
									rma_refund_item (NOLOCK)
								where
									rma_refund_item.refundsysno =" + masterSysNo;
                DataSet dss = SqlHelper.ExecuteDataSet(sql);
                if (Util.HasMoreRow(dss))
                {
                    //调用Register的更新
                    foreach (DataRow dr in dss.Tables[0].Rows)
                    {
                        Hashtable htRegister = new Hashtable(2);
                        htRegister.Add("SysNo", Util.TrimIntNull(dr["registersysno"]));
                        //htRegister.Add("RefundStatus", (int)AppEnum.RMARefundStatus.WaitingRefund);
                        htRegister.Add("RefundStatus", (int)AppEnum.RMARefundStatus.Audited);  //取消退款后，状态变为已审核
                        htRegister.Add("OwnBy", (int)AppEnum.RMAOwnBy.Customer);
                        RMARegisterManager.GetInstance().UpdateRegister(htRegister);
                    }
                }
                scope.Complete();
            }
        }
        /// <summary>
        /// 由于SalesReport中增加了RefundCash和RefundCost，表RMA_Refund_Item中增加了字段：RefundCost和RefundCostPoint
        /// 对于老数据，需要将这两个字段的数据补上
        /// </summary>
        public void SetRefundCost()
        {
            string sql = "select sysno from RMA_Refund_Item (NOLOCK)";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Hashtable ht = new Hashtable(3);
                    DataRow drCost = calcRefundCost(Util.TrimIntNull(dr["sysno"]));
                    if (drCost != null)
                    {
                        ht.Add("SysNo", Util.TrimIntNull(dr["sysno"]));
                        ht.Add("RefundCost", Util.TrimDecimalNull(drCost["Cost"]));
                        ht.Add("RefundCostPoint", Util.TrimIntNull(drCost["Point"]));
                        new RMARefundDac().UpdateItem(ht);
                    }
                }
            }
        }

        #region Print RO Invoice
        /// <summary>
        /// 获取ro发票
        /// </summary>
        /// <param name="roSysNo"></param>
        /// <returns></returns>
        public ROInvoiceInfo GetROInvoice(int refundSysNo)
        {
            ROInvoiceInfo invoice = new ROInvoiceInfo();
            RMARefundInfo oRma = this.Load(refundSysNo);

            IcsonInfo icson = new IcsonInfo();
            invoice.AuditUserSysNo = oRma.AuditUserSysNo;
            invoice.CompanyAddress = icson.CompanyAddress;
            //invoice.InvoiceNote = so.InvoiceNote;
            invoice.SOID = oRma.SOSysNo.ToString();
            invoice.ROID = oRma.RefundID;

            CustomerInfo customer = CustomerManager.GetInstance().Load(oRma.CustomerSysNo);
            invoice.CustomerName = customer.CustomerName;
            invoice.CustomerSysNo = customer.SysNo;
            invoice.ReceiveName = customer.ReceiveName;
            invoice.ReceivePhone = customer.ReceivePhone;

            this.InitPageList(oRma, invoice);
            invoice.TotalPage = invoice.ItemHash.Count;
            invoice.TotalWeight = 0;

            return invoice;
        }

        private void InitPageList(RMARefundInfo oRma, ROInvoiceInfo invoice)
        {
            int index = 0;
            ROInvoicePageInfo page = new ROInvoicePageInfo();
            invoice.ItemHash.Add(index++, page);
            DataSet ds = GetRefundItemDs(oRma.SysNo);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ROInvoicePageItemInfo printItem = new ROInvoicePageItemInfo();
                printItem.pk = dr["registersysno"].ToString();
                printItem.ProductID = Util.TrimNull(dr["ProductID"]);
                printItem.ProductName = Util.TrimNull(dr["ProductName"]);

                decimal refundCash = Util.TrimDecimalNull(dr["RefundCash"]);
                if (oRma.RefundPayType == (int)AppEnum.RMARefundPayType.TransferPointRefund)
                    refundCash = 0;

                printItem.Quantity = -1;
                printItem.Weight = 0;
                printItem.Total = (-1) * refundCash;
                printItem.Price = refundCash;
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
            }

            if (oRma.DeductPointFromCurrentCash != 0)
            {
                ROInvoicePageItemInfo printItem = new ROInvoicePageItemInfo();
                printItem.pk = "赎回积分";
                printItem.ProductID = "赎回积分";
                printItem.Total = Convert.ToDecimal(oRma.DeductPointFromCurrentCash);
                printItem.isRoItem = false;
                //				printItem.isPoint = true;
                if (page.AddItem(printItem) == false)
                {
                    page = new ROInvoicePageInfo();
                    page.AddItem(printItem);
                    invoice.ItemHash.Add(index++, page);
                }
            }

            if (oRma.CompensateShipPrice != 0)
            {
                ROInvoicePageItemInfo printItem = new ROInvoicePageItemInfo();
                printItem.pk = "补偿运费";
                printItem.ProductID = "补偿运费";
                printItem.Total = Convert.ToDecimal(-1 * oRma.CompensateShipPrice);
                printItem.isRoItem = false;
                if (page.AddItem(printItem) == false)
                {
                    page = new ROInvoicePageInfo();
                    page.AddItem(printItem);
                    invoice.ItemHash.Add(index++, page);
                }
            }

            if (oRma.PointAmt != 0)
            {
                ROInvoicePageItemInfo printItem = new ROInvoicePageItemInfo();
                printItem.pk = "影响积分";
                printItem.ProductID = "影响积分";

                int affectedPoint = oRma.PointAmt;
                if (oRma.RefundPayType == (int)AppEnum.RMARefundPayType.TransferPointRefund)
                    affectedPoint += Convert.ToInt32(Decimal.Round(oRma.CashAmt * AppConst.ExchangeRate, 0));

                printItem.Total = affectedPoint;
                printItem.isRoItem = false;
                printItem.isPoint = true;
                if (page.AddItem(printItem) == false)
                {
                    page = new ROInvoicePageInfo();
                    page.AddItem(printItem);
                    invoice.ItemHash.Add(index++, page);
                }
            }

        }

     
        #endregion

        #region Import

        //        public void ImportRefund()
        //        {
        //            string sqlExamRefund = "select top 1 * from RMA_Refund";
        //            DataSet dsExamRefund = SqlHelper.ExecuteDataSet(sqlExamRefund);
        //            if(Util.HasMoreRow(dsExamRefund))
        //                throw new BizException("The RMA_Refund is not empty!");
        //            string sqlExamRefundItem = "select top 1 * from RMA_Refund_Item";
        //            DataSet dsExamRefundItem = SqlHelper.ExecuteDataSet(sqlExamRefundItem);
        //            if(Util.HasMoreRow(dsExamRefundItem))
        //                throw new BizException("The RMA_Refund is not empty!");

        //            string sqlExamReturn = "select top 1 * from RMA_Return";
        //            DataSet dsExamReturn = SqlHelper.ExecuteDataSet(sqlExamReturn);
        //            if(!Util.HasMoreRow(dsExamReturn))
        //                throw new BizException("Please Import RMA_Return at first!");

        //            TransactionOptions options = new TransactionOptions();
        //            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
        //            options.Timeout = TransactionManager.DefaultTimeout;

        //            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
        //            {

        //                string sqlMaster = "select * from ipp3..RO_Master";
        //                DataSet dsMaster = SqlHelper.ExecuteDataSet(sqlMaster);
        //                foreach(DataRow drMaster in dsMaster.Tables[0].Rows)
        //                {	
        //                    if(Util.TrimIntNull(drMaster["SysNo"]) == 3346)
        //                    {
        //                         string a = drMaster["SysNo"].ToString();
        //                    }
        //                    RMARefundInfo oRefund = new RMARefundInfo();
        //                    try
        //                    {						
        //                        oRefund.SysNo = SequenceDac.GetInstance().Create("RMA_Refund_Sequence");
        //                        oRefund.RefundID = this.getRefundID(oRefund.SysNo);
        //                        string sqlGetSO ;
        //                        //如果ro中没有RMA申请单信息，则从ipp2003中获取sosysno和customersysno信息
        //                        if(drMaster["RMASysNo"].ToString() == AppConst.StringNull)					
        //                            sqlGetSO = @"select ipp2003..UT_Master.SOSysNo , ipp2003..UT_Master.CustomSysNo as CustomerSysNo
        //                                     from   ipp3..RO_Master
        //                                     inner  join ipp2003..UT_Master on ipp2003..UT_Master.UTID = ipp3..RO_Master.ROID                                            
        //                                     where  ipp3..RO_Master.SysNo = " + drMaster["SysNo"].ToString();											
        //                        else   //如果ro中包含rma申请单信息，则从rma申请单中读取sosysno和customersysno信息
        //                            sqlGetSO = "select SOSysNo , CustomerSysNo  from RMA_Master where SysNo = " + drMaster["RMASysNo"].ToString();

        //                        DataSet dsGetSO = SqlHelper.ExecuteDataSet(sqlGetSO);
        //                        DataRow drGetSO = dsGetSO.Tables[0].Rows[0];
        //                        oRefund.SOSysNo = Util.TrimIntNull(drGetSO["SOSysNo"]);
        //                        oRefund.CustomerSysNo = Util.TrimIntNull(drGetSO["CustomerSysNo"]);
        //                        oRefund.CreateTime = Util.TrimDateNull(drMaster["CreateTime"]);
        //                        oRefund.CreateUserSysNo = Util.TrimIntNull(drMaster["CreateUserSysNo"]);
        //                        oRefund.RefundTime = Util.TrimDateNull(drMaster["ReturnTime"]);
        //                        oRefund.RefundUserSysNo = Util.TrimIntNull(drMaster["ReturnUserSysNo"]);
        //                        oRefund.AuditTime = Util.TrimDateNull(drMaster["AuditTime"]);
        //                        oRefund.AuditUserSysNo = Util.TrimIntNull(drMaster["AuditUserSysNo"]);
        //                        oRefund.CompensateShipPrice = 0m;											

        //                        decimal cashamt =0m ;
        //                        int orgpoint = 0;
        //                        int refundpoint = 0;

        //                        string sqlItem = "select * from ipp3..RO_Item where ROSysNO = " + drMaster["SysNo"].ToString();
        //                        DataSet dsItem = SqlHelper.ExecuteDataSet(sqlItem);
        //                        if(Util.HasMoreRow(dsItem))
        //                        {
        //                            foreach(DataRow drItem in dsItem.Tables[0].Rows)
        //                            {
        //                                try
        //                                {
        //                                    string RMASysNo = drMaster["RMASysNo"].ToString();
        //                                    if(drMaster["RMASysNO"].ToString() == AppConst.StringNull)
        //                                        RMASysNo = drMaster["SysNo"].ToString();

        //                                    string sqlRegister = "select RegisterSysNo from ippconvert..rma_import where rmasysno =" + RMASysNo + " and ProductSysNo = " + drItem["ProductSysNo"].ToString();
        //                                    DataSet dsRegister = SqlHelper.ExecuteDataSet(sqlRegister);
        //                                    if(Util.HasMoreRow(dsRegister))
        //                                    {
        //                                        foreach(DataRow drRegister in dsRegister.Tables[0].Rows)
        //                                        {
        //                                            RMARefundItemInfo oRefundItem = new RMARefundItemInfo();
        //                                            oRefundItem.RefundSysNo = Util.TrimIntNull(oRefund.SysNo);
        //                                            oRefundItem.RegisterSysNo = Util.TrimIntNull(drRegister["RegisterSysNo"]);
        //                                            oRefundItem.OrgPrice = Util.TrimDecimalNull(drItem["Price"]);
        //                                            oRefundItem.UnitDiscount = 0 ;
        //                                            oRefundItem.ProductValue = oRefundItem.OrgPrice;
        //                                            oRefundItem.OrgPoint = Util.TrimIntNull(drItem["Point"])*(-1);
        //                                            oRefundItem.RefundCash = Util.TrimDecimalNull(drItem["RefundCash"]) / Util.TrimIntNull(drItem["Quantity"]);
        //                                            oRefundItem.RefundPoint = Util.TrimIntNull(drItem["RefundPoint"])/Util.TrimIntNull(drItem["Quantity"])*(-1) ;
        //                                            oRefundItem.RefundPrice = oRefundItem.RefundCash + Convert.ToDecimal(oRefundItem.RefundPoint)/AppConst.ExchangeRate;
        //                                            if(Util.TrimDecimalNull(drItem["RefundCash"]) == 0)
        //                                                oRefundItem.PointType = (int)AppEnum.ProductPayType.PointPayOnly;
        //                                            else if(Util.TrimIntNull(drItem["RefundPoint"]) == 0)
        //                                                oRefundItem.PointType = (int)AppEnum.ProductPayType.MoneyPayOnly;
        //                                            else
        //                                                oRefundItem.PointType = (int)AppEnum.ProductPayType.BothSupported;
        //                                            new RMARefundDac().InsertItem(oRefundItem);

        //                                            //更新相应的Register中RefundStatus的值
        //                                            Hashtable ht = new Hashtable();
        //                                            ht.Add("SysNo" ,oRefundItem.RegisterSysNo );
        //                                            ht.Add("Status" ,  Util.TrimIntNull(drMaster["Status"]));										
        //                                            RMARegisterManager.GetInstance().UpdateRegister(ht);
        //                                            cashamt += oRefundItem.RefundCash;											
        //                                            refundpoint += oRefundItem.RefundPoint;
        //                                            orgpoint += oRefundItem.OrgPoint;
        //                                        }
        //                                    }
        //                                }
        //                                catch
        //                                {
        //                                    throw new BizException("Import RefundItem Error! RO_Item.SysNo = " + drItem["SysNo"].ToString());
        //                                }


        //                            }
        //                        }

        //                        oRefund.OrgCashAmt = cashamt;
        //                        oRefund.OrgPointAmt = refundpoint - orgpoint;

        //                        oRefund.DeductPointFromAccount = Util.TrimIntNull(drMaster["PointAmt"]) - Convert.ToInt32(Util.TrimDecimalNull(drMaster["RedeemAmt"])*AppConst.ExchangeRate);
        //                        oRefund.DeductPointFromCurrentCash = Util.TrimDecimalNull(drMaster["RedeemAmt"])*(-1);


        //                        oRefund.CashAmt = oRefund.OrgCashAmt + oRefund.DeductPointFromCurrentCash;
        //                        oRefund.PointAmt = oRefund.OrgPointAmt + Convert.ToInt32(Util.TrimDecimalNull(drMaster["RedeemAmt"])*AppConst.ExchangeRate);

        //                        SOInfo oSO = SaleManager.GetInstance().LoadSO(oRefund.SOSysNo);

        //                        if ( oSO.SOAmt == 0)
        //                            oRefund.SOCashPointRate = 1;
        //                        else
        //                            oRefund.SOCashPointRate = oSO.CashPay/oSO.SOAmt;//获取销售单商品金额的现金支付比例

        //                        if(Util.TrimDecimalNull(drMaster["CashAmt"]) == 0)
        //                            oRefund.RefundPayType = 1;
        //                        else
        //                            oRefund.RefundPayType = 0;
        //                        oRefund.Note = drMaster["Note"].ToString();
        //                        if(Util.TrimIntNull(drMaster["Status"]) == (int)AppEnum.ROStatus.Abandon)
        //                            oRefund.Status = (int)AppEnum.RMARefundStatus.Abandon;
        //                        else if(Util.TrimIntNull(drMaster["Status"]) == (int)AppEnum.ROStatus.Returned)
        //                            oRefund.Status = (int)AppEnum.RMARefundStatus.Refunded;
        //                        else if(Util.TrimIntNull(drMaster["Status"]) == (int)AppEnum.ROStatus.Audited)
        //                            oRefund.Status = (int)AppEnum.RMARefundStatus.Audited;
        //                        else
        //                            oRefund.Status = (int)AppEnum.RMARefundStatus.WaitingRefund;
        //                        new RMARefundDac().InsertMaster(oRefund);

        //                        //update corresponding finance_soincome info
        //                        string sqlfs = "select sysno from finance_soincome where ordertype = @ordertype and ordersysno =@ordersysno";
        //                        sqlfs = sqlfs.Replace("@ordertype" , ((int)AppEnum.SOIncomeOrderType.RO).ToString());
        //                        sqlfs = sqlfs.Replace("@ordersysno" , drMaster["SysNo"].ToString());
        //                        DataSet dsfs = SqlHelper.ExecuteDataSet(sqlfs);
        //                        if(Util.HasMoreRow(dsfs))
        //                        {
        //                            foreach(DataRow drfs in dsfs.Tables[0].Rows)
        //                            {							
        //                                Hashtable htfs = new Hashtable();
        //                                htfs.Add("SysNo" , Util.TrimIntNull(drfs["sysno"]));
        //                                htfs.Add("OrderSysNo" , oRefund.SysNo);
        //                                new SOIncomeDac().Update(htfs);
        //                            }						    
        //                        }

        //                    }
        //                    catch
        //                    {
        //                        throw new BizException("Import Refund Error! RO_Master.SysNo=" + drMaster["SysNo"].ToString());
        //                    }
        //                }

        //                scope.Complete();
        //            }		
        //        }

        #endregion
    }
}
