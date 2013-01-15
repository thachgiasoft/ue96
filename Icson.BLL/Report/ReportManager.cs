using System;
using System.Data;
using System.Collections;
using System.Transactions;

using Icson.Utils;
using Icson.Objects;
using Icson.Objects.Sale;
using Icson.DBAccess;
using Icson.BLL.Basic;
using System.Text;

namespace Icson.BLL.Report
{
    /// <summary>
    /// Summary description for ReportManager.
    /// </summary>
    public class ReportManager
    {
        private ReportManager()
        {
        }

        private static ReportManager _instance;
        public static ReportManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ReportManager();
            }
            return _instance;
        }

        public DataSet GetSaleAnalysisDs(Hashtable paramHash)
        {
            string sql = @"select @top
								productid, productname, totalNumber, accountqty, availableqty+virtualqty as onlineqty,
								currentprice, totalCost, totalPoint, totalSale, totalSale-totalCost as GrossProfit, 
								case when totalsale =0 then -1.0000 else Round((totalSale-totalCost)/totalSale,4) end as Rate, username as ppmusername
							from product
								inner join product_price on product.sysno = product_price.productsysno
								inner join inventory on product.sysno = inventory.productsysno
								inner join manufacturer on product.manufacturersysno = manufacturer.sysno
								inner join category3 on product.c3sysno = category3.sysno 
								inner join category2 on category3.c2sysno = category2.sysno 
								inner join category1 on category2.c1sysno = category1.sysno
								inner join 
									(
										select
											productsysno, 
											sum(quantity*cost) as totalCost,sum(quantity * point) as totalPoint, sum(quantity*(price- so_item.discountamt)) as totalSale, sum(quantity) as totalNumber
										from 
											so_master 
											inner join so_item on so_master.sysno = so_item.sosysno
										where
											so_master.status = @status
										and outtime>=@datefrom and outtime<=@dateto
											@iswholesale
										group by productsysno
									) as a on product.sysno = a.productsysno
								inner join sys_user on product.pmusersysno = sys_user.sysno
								where 1=1  @ppmusersysno @productsysno @manufacturersysno @category";

            //处理日期
            string datefrom = paramHash["DateFrom"] as string;
            string dateto = paramHash["DateTo"] as string;
            sql = sql.Replace("@datefrom", "cast(" + Util.ToSqlString(datefrom) + " as datetime)");
            sql = sql.Replace("@dateto", "cast(" + Util.ToSqlEndDate(dateto) + " as datetime)");

            //设置订单状态
            int status = (int)AppEnum.SOStatus.OutStock;
            sql = sql.Replace("@status", status.ToString());

            //处理商品编号
            if (paramHash.ContainsKey("ProductSysNo"))
            {
                int productSysNo = (int)paramHash["ProductSysNo"];
                sql = sql.Replace("@productsysno", " and product.sysno = " + productSysNo.ToString());
            }
            else
            {
                sql = sql.Replace("@productsysno", "");
            }

            //处理厂商
            if (paramHash.ContainsKey("ManufacturerSysNo"))
            {
                int manufacturerSysNo = (int)paramHash["ManufacturerSysNo"];
                sql = sql.Replace("@manufacturersysno", " and manufacturer.sysno = " + manufacturerSysNo.ToString());
            }
            else
            {
                sql = sql.Replace("@manufacturersysno", "");
            }

            //处理TOP10， 100 等
            if (paramHash.ContainsKey("TOP"))
            {
                int top = (int)paramHash["TOP"];
                sql = sql.Replace("@top", " top " + top.ToString());
            }
            else
            {
                sql = sql.Replace("@top", "");
            }

            if (paramHash.ContainsKey("Category"))
            {
                sql = sql.Replace("@category", " and " + (string)paramHash["Category"]);
            }
            else
            {
                sql = sql.Replace("@category", "");
            }


            //处理是否批发
            if (paramHash.ContainsKey("IsWholeSale"))
            {
                int iswholesale = (int)paramHash["IsWholeSale"];
                sql = sql.Replace("@iswholesale", " and iswholesale = " + iswholesale.ToString());
            }
            else
            {
                sql = sql.Replace("@iswholesale", "");
            }
            //处理pm
            if (paramHash.ContainsKey("PPMUserSysNo"))
            {
                int ppmUserSysNo = (int)paramHash["PPMUserSysNo"];
                sql = sql.Replace("@ppmusersysno", " and pmusersysno = " + ppmUserSysNo.ToString());
            }
            else
            {
                sql = sql.Replace("@ppmusersysno", "");
            }

            if (paramHash.ContainsKey("OrderBy"))
            {
                sql += " order by " + Util.SafeFormat((string)paramHash["OrderBy"] + " desc");
            }
            
            return SqlHelper.ExecuteDataSet(sql);
        }
        public DataSet GetSaleReportDs(Hashtable paramHash)
        {
            //datefrom dateto stock iswholesale
            string sql = @"
							select 
									a.ddd, a.ordernumber, b.outnumber, 
									saleamt, saleamt_p, miscamt, 
									shipprice,freeshipfeepay, payprice, premiumamt,
									cost, cost_p
							from
								(
									select 
										convert(@groupby, orderdate, 120) as ddd, count(orderdate) as ordernumber
									from 
										so_master 
									where
										(orderdate>=@datefrom and orderdate<=@dateto) @iswholesale @stocksysno	
										group by convert(@groupby, orderdate, 120)
								) as a 
								left join
								(
									select 
										convert(@groupby, outtime, 120) as ddd, count(outtime) as outnumber
									from 
										so_master 
									where
										(outtime>=@datefrom and orderdate<=@dateto)	and status=@status @iswholesale @stocksysno
										group by convert(@groupby, outtime, 120)
								) as b
								on a.ddd = b.ddd
								left join
								(
									select
										convert(@groupby, outtime, 120) as ddd,
										sum(cashpay-discountamt-couponamt) as saleamt,
										sum(cashpay-discountamt-couponamt+pointpay/10.0) as saleamt_p,
										sum(payprice+shipprice-freeshipfeepay+premiumamt) as miscamt,
										sum(shipprice) as shipprice,
                                        sum(freeshipfeepay) as freeshipfeepay,
										sum(payprice) as payprice,
										sum(premiumamt) as premiumamt,
										sum(items.cost) as cost, 
										sum(items.cost_p) as cost_p
									from
										so_master inner join
											(
											select 
												sosysno, 
												sum(cost*quantity) as cost, sum((cost+ point/10.0)*quantity) as cost_p
											from 
												so_master inner join so_item on so_master.sysno = so_item.sosysno
												where 
													so_master.status = @status
												and	( (orderdate>=@datefrom and orderdate<=@dateto) or (outtime>=@datefrom and orderdate<=@dateto) ) @iswholesale @stocksysno
												group by so_item.sosysno
											) as items
										on so_master.sysno = items.sosysno @iswholesale @stocksysno
									group by convert(@groupby, outtime, 120)
								) as c
								on a.ddd = c.ddd

							order by a.ddd";

            string datefrom = paramHash["DateFrom"] as string;
            string dateto = paramHash["DateTo"] as string;
            int status = (int)AppEnum.SOStatus.OutStock;
            string groupby = paramHash["GroupBy"] as string;
            if (groupby == "Day")
            {
                sql = sql.Replace("@groupby", "nvarchar(10)");
            }
            else
            {
                sql = sql.Replace("@groupby", "nvarchar(7)");
            }

            sql = sql.Replace("@datefrom", "cast(" + Util.ToSqlString(datefrom) + " as datetime)");
            sql = sql.Replace("@dateto", "cast(" + Util.ToSqlEndDate(dateto) + " as datetime)");
            sql = sql.Replace("@status", status.ToString());
            if (paramHash.ContainsKey("IsWholeSale"))
            {
                int iswholesale = (int)paramHash["IsWholeSale"];
                sql = sql.Replace("@iswholesale", " and iswholesale = " + iswholesale.ToString());
            }
            else
            {
                sql = sql.Replace("@iswholesale", "");
            }
            if (paramHash.ContainsKey("StockSysNo"))
            {
                int stocksysno = (int)paramHash["StockSysNo"];
                sql = sql.Replace("@stocksysno", " and stocksysno = " + stocksysno.ToString());
            }
            else
            {
                sql = sql.Replace("@stocksysno", "");
            }

            return SqlHelper.ExecuteDataSet(sql);

        }

        public DataSet GetInventoryReportDs(Hashtable paramHash)
        {
            if (!paramHash.ContainsKey("StockSysNo"))
            {
                return null;
            }

//            string sqlStr = @"select ivs.stocksysno,stock.stockname,
//								p.SysNo as ProductSysNo,p.productID,p.productName,isnull(iv.accountQty,0) as accountQty,
//								isnull(iv.availableQty,0) as availableQty,isnull(iv.purchaseQty,0) as purchaseQty,
//								pp.unitcost,(isnull(pp.unitcost,0)*isnull(iv.accountQty,0)) as amount,
//								isnull(ivs.position1,'00-000-000') as position1,isnull(ivs.position2,'00-000-000') as position2 from inventory iv
//								inner join product p on p.sysno = iv.productsysno
//								inner join product_price pp on iv.productsysno = pp.productsysno
//								inner join category3 on p.c3sysno = category3.sysno 
//								inner join category2 on category3.c2sysno = category2.sysno 
//								inner join category1 on category2.c1sysno = category1.sysno
//								inner join Inventory_Stock ivs on ivs.productsysno=p.sysno
//                                inner join stock on ivs.stocksysno=stock.sysno 
//								where 
//								@PPM
//								and @ProductSysNo
//                                and @StockSysNo
//								and @C3SysNo
//								and @C2SysNo
//								and @C1SysNo 
//                                and @InventoryReportType
//								and @ShowZero ";
            string sqlStr = @"select ivs.stocksysno,stock.stockname,
								p.SysNo as ProductSysNo,p.productID,p.productName,isnull(ivs.accountQty,0) as accountQty,
								isnull(ivs.availableQty,0) as availableQty,isnull(ivs.purchaseQty,0) as purchaseQty,
								pp.unitcost,(isnull(pp.unitcost,0)*isnull(ivs.accountQty,0)) as amount,
								isnull(ivs.position1,'00-000-000') as position1,isnull(ivs.position2,'00-000-000') as position2 from inventory iv
								inner join product p on p.sysno = iv.productsysno
								inner join product_price pp on iv.productsysno = pp.productsysno
								inner join category3 on p.c3sysno = category3.sysno 
								inner join category2 on category3.c2sysno = category2.sysno 
								inner join category1 on category2.c1sysno = category1.sysno
								inner join Inventory_Stock ivs on ivs.productsysno=p.sysno
                                inner join stock on ivs.stocksysno=stock.sysno 
								where 
								@PPM
								and @ProductSysNo
                                and @StockSysNo
								and @C3SysNo
								and @C2SysNo
								and @C1SysNo 
                                and @InventoryReportType
								and @ShowZero 
                                and @ProductType";

            if (paramHash.ContainsKey("ProductSysNo"))
            {
                sqlStr = sqlStr.Replace("@ProductSysNo", " p.sysno =" + paramHash["ProductSysNo"] as string);
            }
            else
            {
                sqlStr = sqlStr.Replace("@ProductSysNo", " 1=1 ");
            }
            if(paramHash.ContainsKey("StockSysNo"))
            {
                sqlStr = sqlStr.Replace("@StockSysNo", " stock.sysno=" + paramHash["StockSysNo"] as string);
            }
            else
            {
                sqlStr = sqlStr.Replace("@StockSysNo", " 1=1 ");
            }
            if (paramHash.ContainsKey("PPM"))
            {
                sqlStr = sqlStr.Replace("@PPM", " p.ppmusersysno = " + paramHash["PPM"] as string);
            }
            else
            {
                sqlStr = sqlStr.Replace("@PPM", " 1=1 ");
            }
            if (paramHash.ContainsKey("ShowZero"))
            {
                sqlStr = sqlStr.Replace("@ShowZero", " 1=1 ");
            }
            else
            {
                //sqlStr = sqlStr.Replace("@ShowZero", " iv.accountQty>0 ");
                sqlStr = sqlStr.Replace("@ShowZero", " ivs.accountQty>0 ");
            }
            if (paramHash.ContainsKey("C3SysNo"))
            {
                sqlStr = sqlStr.Replace("@C3SysNo", " category3.sysno = " + paramHash["C3SysNo"] as string);
            }
            else
            {
                sqlStr = sqlStr.Replace("@C3SysNo", " 1=1 ");
            }
            if (paramHash.ContainsKey("C2SysNo"))
            {
                sqlStr = sqlStr.Replace("@C2SysNo", " category2.sysno = " + paramHash["C2SysNo"] as string);
            }
            else
            {
                sqlStr = sqlStr.Replace("@C2SysNo", " 1=1 ");
            }
            if (paramHash.ContainsKey("C1SysNo"))
            {
                sqlStr = sqlStr.Replace("@C1SysNo", " category1.sysno = " + paramHash["C1SysNo"] as string);
            }
            else
            {
                sqlStr = sqlStr.Replace("@C1SysNo", " 1=1 ");
            }

            if (paramHash.ContainsKey("InventoryReportType"))
            {
                string TypeFilterString = GetInventoryReportTypeFilterString(Int32.Parse(paramHash["InventoryReportType"].ToString()));
                sqlStr = sqlStr.Replace("@InventoryReportType", TypeFilterString);
            }
            else
            {
                sqlStr = sqlStr.Replace("@InventoryReportType", " 1=1 ");
            }

            if (paramHash.ContainsKey("ProductType"))
            {
                sqlStr = sqlStr.Replace("@ProductType", " p.ProductType = " + paramHash["ProductType"] as string);
            }
            else
            {
                sqlStr = sqlStr.Replace("@ProductType", " 1=1 ");
            }

            sqlStr = sqlStr + " order by position1,position2";
            return SqlHelper.ExecuteDataSet(sqlStr);
        }

        public DataSet GetPMTodayFundsReport(Hashtable paramHash)
        {
            //pm的占用资金 = 库存-当日帐期资金
            //其中当日帐期计算可以用：采购单金额X（付款日期-今天日期）/ （付款日期-采购单日期）； 
            //已付款或者付款日期小于今天日期的订单，不在计算范围内；当日付款的，不做计算。

            int PMSysNo = Int32.Parse(paramHash["PMSysNo"].ToString());
            string TodayDate = paramHash["TodayDate"].ToString();

            string sql = @"select v2.sysno,v2.username,isnull(v1.pmtotalamt,0) as pmtotalamt,isnull(v2.pmstockfunds,0) as pmstockfunds,
                            (isnull(v2.pmstockfunds,0) - isnull(v1.pmtotalamt,0)) as pmfunds from 
                            (select sys_user.sysno,sys_user.username, 
                            sum( TotalAmt * (cast(datediff(day,getdate(),po_master.paydate) as decimal(5,2)) / cast(datediff(day,po_master.intime,po_master.paydate) as decimal(5,2)))) as PMTotalAmt 
                            from po_master inner join sys_user on sys_user.sysno=po_master.createusersysno and sys_user.status=0 @Filterpmsysno 
                            inner join finance_popay on po_master.sysno=finance_popay.posysno @Filterpaystatus 
                            and po_master.paydate > po_master.intime and po_master.paydate > getdate()  
                            and po_master.paydate is not null and po_master.intime is not null 
                            group by sys_user.sysno,sys_user.username)  v1 right outer join 
                            (select sys_user.sysno,sys_user.username, sum((isnull(product_price.unitcost,0)*isnull(inventory.accountQty,0))) as PMStockFunds
                            from inventory inner join product_price on inventory.productsysno=product_price.productsysno 
                            inner join product on inventory.productsysno=product.sysno inner join sys_user on product.PMUsersysno=sys_user.sysno and sys_user.status=0 @Filterpmsysno 
                            group by sys_user.sysno,sys_user.username) v2 
                            on v1.sysno = v2.sysno";
            int UnPay = (int)AppEnum.POPayStatus.UnPay;
            sql = sql.Replace("@Filterpaystatus", " and finance_popay.paystatus=" + UnPay.ToString());
            if (PMSysNo > 0)
            {
                sql = sql.Replace("@Filterpmsysno", " and sys_user.sysno=" + PMSysNo.ToString());
            }
            else
            {
                sql = sql.Replace("@Filterpmsysno", "");
            }

            sql = sql.Replace("getdate()", "'" + TodayDate + "'");
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;
            return ds;
        }

        public string GetPMFundsReport(Hashtable paramHash)
        {
            StringBuilder ReportTable = new StringBuilder();
            int PMSysNo = Int32.Parse(paramHash["PMSysNo"].ToString());
            string DateFrom = paramHash["DateFrom"].ToString();
            string DateTo = paramHash["DateTo"].ToString();
            StringBuilder sbSql = new StringBuilder();

            string sql = @"select distinct sys_user.sysno as pmsysno,sys_user.username from pmpayment 
                           inner join sys_user on pmpayment.pmsysno=sys_user.sysno and sys_user.status=0 @Filterpmsysno order by sys_user.sysno";
            if (PMSysNo > 0)
            {
                sql = sql.Replace("@Filterpmsysno", " and sys_user.sysno=" + PMSysNo.ToString());
            }
            else
            {
                sql = sql.Replace("@Filterpmsysno", "");
            }
            DataSet ds = SqlHelper.ExecuteDataSet(sql); //纪录所有pmsysno,username
            if (!Util.HasMoreRow(ds))
                return "no records";
            int PMCount = ds.Tables[0].Rows.Count;
            if (PMCount == 0)
                return "no records";
            sbSql.Append("select pmpayment.paydate, ");
            int i = 0;
            ReportTable.Append("<table class=specification><tr><td width=40>序号</td><td width=60>日期</td>");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sbSql.Append("'" + Util.TrimNull(dr["username"]) + "'=sum(case username when '" + Util.TrimNull(dr["username"]) + "' then (stockamt-payamt) else 0 end)");
                if (i < PMCount - 1)
                {
                    sbSql.Append(",");
                }
                ReportTable.Append("<td width=80 align=right>" + Util.TrimNull(dr["username"]) + "</td>");
                i++;
            }
            ReportTable.Append("<td width=80 align=right>Total</td></tr>");
            sbSql.Append("from pmpayment inner join sys_user on pmpayment.pmsysno=sys_user.sysno @Filterdatefrom @Filterdateto group by pmpayment.paydate order by pmpayment.paydate");
            sbSql = sbSql.Replace("@Filterdatefrom", " and pmpayment.paydate >= '" + DateFrom + "'");
            sbSql = sbSql.Replace("@Filterdateto", " and pmpayment.paydate <= '" + DateTo + "'");

            decimal[] dTotalCol = new decimal[ds.Tables[0].Rows.Count];
            DataSet ds2 = SqlHelper.ExecuteDataSet(sbSql.ToString());  //纪录所有占用资金
            if (!Util.HasMoreRow(ds))
                return "no records";
            int DateCount = ds2.Tables[0].Rows.Count;
            if (DateCount == 0)
                return "no records";
            decimal[] dTotalRow = new decimal[ds2.Tables[0].Rows.Count];
            decimal dTotal = 0;
            i = 0;
            foreach (DataRow dr in ds2.Tables[0].Rows)
            {
                ReportTable.Append("<td>" + Convert.ToString(i + 1) + "</td><td>" + Convert.ToDateTime(dr["paydate"]).ToString(AppConst.DateFormat) + "</td>");
                dTotalRow[i] = 0;
                for (int j = 0; j < PMCount; j++)
                {
                    ReportTable.Append("<td align=right>" + Util.ToMoney(dr[j + 1].ToString()) + "</td>");
                    dTotalRow[i] += Util.ToMoney(dr[j + 1].ToString());
                    dTotalCol[j] += Util.ToMoney(dr[j + 1].ToString());
                    dTotal += Util.ToMoney(dr[j + 1].ToString());
                }
                ReportTable.Append("<td align=right>" + Util.ToMoney(dTotalRow[i]) + "</td></tr>");
                i++;
            }

            ReportTable.Append("<tr><td>&nbsp;</td><td>Avg</td>");

            for (int j = 0; j < PMCount; j++)
            {
                ReportTable.Append("<td align=right>" + Util.ToMoney(dTotalCol[j] / DateCount) + "</td>");
            }
            ReportTable.Append("<td align=right>" + Util.ToMoney(dTotal / DateCount) + "</td></tr></table>");

            return ReportTable.ToString();
        }

        public string GetPMPayDateReport(Hashtable paramHash)
        {
            StringBuilder ReportTable = new StringBuilder();
            string DateFrom = Convert.ToDateTime(paramHash["DateFrom"].ToString()).ToString("yyyy-MM-dd");
            string DateTo = Convert.ToDateTime(paramHash["DateTo"].ToString()).ToString("yyyy-MM-dd");
            int iDateDiff = Util.DateDiff(Convert.ToDateTime(DateFrom), Convert.ToDateTime(DateTo));
            if (iDateDiff < 0)
                return "";
            int PMSysNo = Int32.Parse(paramHash["PMSysNo"].ToString());
            int VendorSysNo = Int32.Parse(paramHash["VendorSysNo"].ToString());
            int PayStatusSysNo = Int32.Parse(paramHash["PayStatusSysNo"].ToString());
            string Type = paramHash["Type"].ToString().Trim();
            string sql = "";
            string curDate = "";
            StringBuilder sbSQL = new StringBuilder();
            DataSet ds = new DataSet();
            decimal total = 0;
            int i = 1; //记录行数
            int j = 1; //记录列数
            switch (Type)
            {
                case "pm":
                    {
                        if (DateFrom == DateTo && PMSysNo > 0)  //统计指定PM某一天,显示具体的采购单
                        {
                            sql = @"select sys_user.sysno as usersysno,sys_user.username,po_master.sysno as posysno,po_master.poid,vendor.vendorname,
                                convert(nvarchar(10), po_master.intime, 120) as intime,po_master.totalamt 
                                from po_master inner join sys_user on po_master.createusersysno = sys_user.sysno @Filterpmsysno 
                                inner join vendor on po_master.vendorsysno = vendor.sysno 
                                inner join finance_popay on po_master.sysno=finance_popay.posysno @Filterpaystatus
                                and convert(nvarchar(10), po_master.paydate, 120)='@paydate' and po_master.intime is not null order by po_master.sysno";
                            sql = sql.Replace("@Filterpmsysno", " and sys_user.sysno=" + PMSysNo.ToString());
                            sql = sql.Replace("@paydate", DateFrom);
                            if (PayStatusSysNo != AppConst.IntNull)
                            {
                                sql = sql.Replace("@Filterpaystatus", " and finance_popay.paystatus=" + PayStatusSysNo.ToString());
                            }
                            else
                            {
                                sql = sql.Replace("@Filterpaystatus", "");
                            }
                            ds = SqlHelper.ExecuteDataSet(sql);
                            if (!Util.HasMoreRow(ds))
                                return "";
                            ReportTable.Append("<table class=specification width=540><tr><td width=40>序号</td><td width=50>PM</td><td width=60>采购单号</td><td width=200>供应商</td><td width=100>入库时间</td><td width=100 align=right>金额</td></tr>");
                            total = 0;
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                total += Convert.ToDecimal(dr["totalamt"].ToString());
                                ReportTable.Append("<tr><td>" + i.ToString() + "</td><td>" + Util.TrimNull(dr["username"]) + "</td><td><a href=\"javascript:openWindowS2('../Purchase/POSheet.aspx?sysno=" + dr["posysno"].ToString() + "&opt=view')\">" + Util.TrimNull(dr["posysno"]) + "</a></td><td>" + Util.TrimNull(dr["vendorname"]) + "</td><td>" + Util.TrimNull(dr["intime"]) + "</td><td align=right>" + Util.ToMoney(dr["totalamt"].ToString()) + "</td></tr>");
                                i++;
                            }
                            ReportTable.Append("<tr><td colspan=5 align=right>Total: </td><td align=right><strong>" + Util.ToMoney(total) + "</strong></td></tr>");
                            ReportTable.Append("</table>");
                        }
                        else //统计一段时间
                        {
                            sbSQL.Append("select username,");
                            for (j = 0; j <= iDateDiff; j++)
                            {
                                curDate = Convert.ToDateTime(DateFrom).AddDays(j).ToString("yyyy-MM-dd");
                                sbSQL.Append("'" + curDate + "'=sum(case datestring when '" + curDate + "' then pmtotalamt else 0 end),");
                            }
                            sbSQL.Append(@" v.sysno as pmsysno from (select convert(nvarchar(10), po_master.paydate, 120) as datestring, sys_user.sysno,sys_user.username,sum(po_master.totalamt) as pmtotalamt
                                        from po_master inner join sys_user on po_master.createusersysno=sys_user.sysno 
                                        inner join finance_popay on po_master.sysno=finance_popay.posysno 
                                        where po_master.intime is not null and convert(nvarchar(10), po_master.paydate, 120)>='@datefrom' 
                                        and convert(nvarchar(10), po_master.paydate, 120)<='@dateto' @Filterpmsysno @Filterpaystatus  
                                        group by convert(nvarchar(10), po_master.paydate, 120),sys_user.sysno,sys_user.username 
                                        ) v group by v.sysno,v.username");
                            sql = sbSQL.ToString().Replace("@datefrom", DateFrom).Replace("@dateto", DateTo);

                            if (PMSysNo > 0)
                            {
                                sql = sql.Replace("@Filterpmsysno", " and sys_user.sysno=" + PMSysNo.ToString());
                            }
                            else
                            {
                                sql = sql.Replace("@Filterpmsysno", "");
                            }

                            if (PayStatusSysNo != AppConst.IntNull)
                            {
                                sql = sql.Replace("@Filterpaystatus", " and finance_popay.paystatus=" + PayStatusSysNo.ToString());
                            }
                            else
                            {
                                sql = sql.Replace("@Filterpaystatus", "");
                            }

                            ds = SqlHelper.ExecuteDataSet(sql);
                            if (!Util.HasMoreRow(ds))
                                return "";
                            ReportTable.Append("<table class=specification><tr><td width=40>序号</td><td width=60>PM</td>");
                            for (j = 0; j <= iDateDiff; j++)
                            {
                                curDate = Convert.ToDateTime(DateFrom).AddDays(j).ToString("yyyy-MM-dd");
                                ReportTable.Append("<td width=80 align=right>" + curDate + "</td>");
                            }
                            ReportTable.Append("<td width=80 align=right>Total</td></tr>");
                            total = 0;
                            decimal[] dTotalCol = new decimal[iDateDiff + 1];
                            decimal[] dTotalRow = new decimal[ds.Tables[0].Rows.Count];
                            i = 1;
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                ReportTable.Append("<tr><td>" + i.ToString() + "</td><td>" + Util.TrimNull(dr["username"]) + "</td>");
                                for (j = 0; j <= iDateDiff; j++)
                                {
                                    total += Convert.ToDecimal(dr[j + 1].ToString());
                                    dTotalCol[j] += Convert.ToDecimal(dr[j + 1].ToString());
                                    dTotalRow[i - 1] += Convert.ToDecimal(dr[j + 1].ToString());
                                    ReportTable.Append("<td align=right><a href=\"javascript:openWindowS2('PMPayDateReport.aspx?paystatus=" + PayStatusSysNo.ToString() + "&type=pm&pmsysno=" + Util.TrimNull(dr["pmsysno"]) + "&datefrom=" + Convert.ToDateTime(DateFrom).AddDays(j).ToString("yyyy-MM-dd") + "&dateto=" + Convert.ToDateTime(DateFrom).AddDays(j).ToString("yyyy-MM-dd") + "')\">" + Util.ToMoney(dr[j + 1].ToString()) + "</a></td>");
                                }
                                ReportTable.Append("<td align=right><strong>" + Util.ToMoney(dTotalRow[i - 1]) + "</strong></td>");
                                i++;
                                ReportTable.Append("</tr>");
                            }
                            ReportTable.Append("<tr><td colspan=2 align=right>Total: </td>");
                            for (j = 0; j <= iDateDiff; j++)
                            {
                                ReportTable.Append("<td align=right><strong>" + Util.ToMoney(dTotalCol[j]) + "</strong></td>");
                            }
                            ReportTable.Append("<td align=right><strong>" + Util.ToMoney(total) + "</strong></td>");

                            ReportTable.Append("</tr>");
                            ReportTable.Append("</table>");
                        }
                        break;
                    }
                case "vendor":
                    {
                        if (DateFrom == DateTo && VendorSysNo > 0)  //统计指定供应商某一天,显示具体的采购单
                        {
                            sql = @"select vendor.sysno as vendorsysno,vendor.vendorname,po_master.sysno as posysno,po_master.poid,sys_user.username,
                                convert(nvarchar(10), po_master.intime, 120) as intime,po_master.totalamt 
                                from po_master inner join vendor on po_master.vendorsysno = vendor.sysno @Filtervendorsysno 
                                inner join sys_user on po_master.createusersysno = sys_user.sysno 
                                inner join finance_popay on po_master.sysno=finance_popay.posysno @Filterpaystatus 
                                and convert(nvarchar(10), po_master.paydate, 120)='@paydate' and po_master.intime is not null order by po_master.sysno";
                            sql = sql.Replace("@Filtervendorsysno", " and vendor.sysno=" + VendorSysNo.ToString());
                            sql = sql.Replace("@paydate", DateFrom);
                            if (PayStatusSysNo != AppConst.IntNull)
                            {
                                sql = sql.Replace("@Filterpaystatus", " and finance_popay.paystatus=" + PayStatusSysNo.ToString());
                            }
                            else
                            {
                                sql = sql.Replace("@Filterpaystatus", "");
                            }
                            ds = SqlHelper.ExecuteDataSet(sql);
                            if (!Util.HasMoreRow(ds))
                                return "no records";
                            ReportTable.Append("<table class=specification width=540><tr><td width=40>序号</td><td width=200>供应商</td><td width=60>采购单号</td><td width=60>PM</td><td width=100>入库时间</td><td width=100 align=right>金额</td></tr>");
                            total = 0;
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                total += Convert.ToDecimal(dr["totalamt"].ToString());
                                ReportTable.Append("<tr><td>" + i.ToString() + "</td><td>" + Util.TrimNull(dr["vendorname"]) + "</td><td><a href=\"javascript:openWindowS2('../Purchase/POSheet.aspx?sysno=" + dr["posysno"].ToString() + "&opt=view')\">" + Util.TrimNull(dr["posysno"]) + "</a></td><td>" + Util.TrimNull(dr["username"]) + "</td><td>" + Util.TrimNull(dr["intime"]) + "</td><td align=right>" + Util.ToMoney(dr["totalamt"].ToString()) + "</td></tr>");
                                i++;
                            }
                            ReportTable.Append("<tr><td colspan=5 align=right>Total: </td><td align=right><strong>" + Util.ToMoney(total) + "</strong></td></tr>");
                            ReportTable.Append("</table>");
                        }
                        else //统计一段时间
                        {
                            sbSQL.Append("select vendorname,");
                            for (j = 0; j <= iDateDiff; j++)
                            {
                                curDate = Convert.ToDateTime(DateFrom).AddDays(j).ToString("yyyy-MM-dd");
                                sbSQL.Append("'" + curDate + "'=sum(case datestring when '" + curDate + "' then pmtotalamt else 0 end),");
                            }

                            sbSQL.Append(@" v.sysno as vendorsysno from (select convert(nvarchar(10), po_master.paydate, 120) as datestring, vendor.sysno,vendor.vendorname,sum(po_master.totalamt) as pmtotalamt
                                        from po_master inner join vendor on po_master.vendorsysno=vendor.sysno 
                                        inner join finance_popay on po_master.sysno=finance_popay.posysno 
                                        where po_master.intime is not null and convert(nvarchar(10), po_master.paydate, 120)>='@datefrom' 
                                        and convert(nvarchar(10), po_master.paydate, 120)<='@dateto' @Filtervendorsysno @Filterpaystatus  
                                        group by convert(nvarchar(10), po_master.paydate, 120),vendor.sysno,vendor.vendorname 
                                        ) v group by v.sysno,v.vendorname");
                            sql = sbSQL.ToString().Replace("@datefrom", DateFrom).Replace("@dateto", DateTo);

                            if (PMSysNo > 0)
                            {
                                sql = sql.Replace("@Filtervendorsysno", " and vendor.sysno=" + PMSysNo.ToString());
                            }
                            else
                            {
                                sql = sql.Replace("@Filtervendorsysno", "");
                            }

                            if (PayStatusSysNo != AppConst.IntNull)
                            {
                                sql = sql.Replace("@Filterpaystatus", " and finance_popay.paystatus=" + PayStatusSysNo.ToString());
                            }
                            else
                            {
                                sql = sql.Replace("@Filterpaystatus", "");
                            }

                            ds = SqlHelper.ExecuteDataSet(sql);
                            if (!Util.HasMoreRow(ds))
                                return "no records";
                            ReportTable.Append("<table class=specification><tr><td width=40>序号</td><td width=200>供应商</td>");
                            for (j = 0; j <= iDateDiff; j++)
                            {
                                curDate = Convert.ToDateTime(DateFrom).AddDays(j).ToString("yyyy-MM-dd");
                                ReportTable.Append("<td width=80 align=right>" + curDate + "</td>");
                            }
                            ReportTable.Append("<td width=80 align=right>Total</td></tr>");
                            total = 0;
                            decimal[] dTotalCol = new decimal[iDateDiff + 1];
                            decimal[] dTotalRow = new decimal[ds.Tables[0].Rows.Count];
                            i = 1;
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                ReportTable.Append("<tr><td>" + i.ToString() + "</td><td>" + Util.TrimNull(dr["vendorname"]) + "</td>");
                                for (j = 0; j <= iDateDiff; j++)
                                {
                                    total += Convert.ToDecimal(dr[j + 1].ToString());
                                    dTotalCol[j] += Convert.ToDecimal(dr[j + 1].ToString());
                                    dTotalRow[i - 1] += Convert.ToDecimal(dr[j + 1].ToString());
                                    ReportTable.Append("<td align=right><a href=\"javascript:openWindowS2('PMPayDateReport.aspx?paystatus=" + PayStatusSysNo.ToString() + "&type=vendor&vendorsysno=" + Util.TrimNull(dr["vendorsysno"]) + "&datefrom=" + Convert.ToDateTime(DateFrom).AddDays(j).ToString("yyyy-MM-dd") + "&dateto=" + Convert.ToDateTime(DateFrom).AddDays(j).ToString("yyyy-MM-dd") + "')\">" + Util.ToMoney(dr[j + 1].ToString()) + "</a></td>");
                                }
                                ReportTable.Append("<td align=right><strong>" + Util.ToMoney(dTotalRow[i - 1]) + "</strong></td>");
                                i++;
                                ReportTable.Append("</tr>");
                            }
                            ReportTable.Append("<tr><td colspan=2 align=right>Total: </td>");
                            for (j = 0; j <= iDateDiff; j++)
                            {
                                ReportTable.Append("<td align=right><strong>" + Util.ToMoney(dTotalCol[j]) + "</strong></td>");
                            }
                            ReportTable.Append("<td align=right><strong>" + Util.ToMoney(total) + "</strong></td>");

                            ReportTable.Append("</tr>");
                            ReportTable.Append("</table>");
                        }
                        break;
                    }
                case "both":
                    {
                        if (PMSysNo > 0 && VendorSysNo > 0)
                        {
                            sql = @"select sys_user.sysno as usersysno,sys_user.username,po_master.sysno as posysno,po_master.poid,vendor.vendorname,
                                    convert(nvarchar(10), po_master.intime, 120) as intime,po_master.totalamt 
                                    from po_master inner join sys_user on po_master.createusersysno = sys_user.sysno and sys_user.sysno=@Filterpmsysno 
                                    inner join vendor on po_master.vendorsysno = vendor.sysno and vendor.sysno=@Filtervendorsysno 
                                    inner join finance_popay on po_master.sysno=finance_popay.posysno @Filterpaystatus
                                    and convert(nvarchar(10), po_master.paydate, 120)>='@datefrom' and convert(nvarchar(10), po_master.paydate, 120)<='@dateto' 
                                    and po_master.intime is not null order by convert(nvarchar(10), po_master.paydate, 120)";
                            sql = sql.Replace("@Filterpmsysno", PMSysNo.ToString()).Replace("@Filtervendorsysno", VendorSysNo.ToString()).Replace("@datefrom", DateFrom).Replace("@dateto", DateTo);
                            if (PayStatusSysNo != AppConst.IntNull)
                            {
                                sql = sql.Replace("@Filterpaystatus", " and finance_popay.paystatus=" + PayStatusSysNo.ToString());
                            }
                            else
                            {
                                sql = sql.Replace("@Filterpaystatus", "");
                            }
                            ds = SqlHelper.ExecuteDataSet(sql);
                            if (!Util.HasMoreRow(ds))
                                return "no records";
                            ReportTable.Append("<table class=specification width=560><tr><td width=40>序号</td><td width=50>PM</td><td width=200>供应商</td><td width=60>采购单号</td><td width=100>入库时间</td><td width=100 align=right>金额</td></tr>");
                            total = 0;
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                total += Convert.ToDecimal(dr["totalamt"].ToString());
                                ReportTable.Append("<tr><td>" + i.ToString() + "</td><td>" + Util.TrimNull(dr["username"]) + "</td><td>" + Util.TrimNull(dr["vendorname"]) + "</td><td><a href=\"javascript:openWindowS2('../Purchase/POSheet.aspx?sysno=" + dr["posysno"].ToString() + "&opt=view')\">" + Util.TrimNull(dr["posysno"]) + "</a></td><td>" + Util.TrimNull(dr["intime"]) + "</td><td align=right>" + Util.ToMoney(dr["totalamt"].ToString()) + "</td></tr>");
                                i++;
                            }
                            ReportTable.Append("<tr><td colspan=5 align=right>Total: </td><td align=right><strong>" + Util.ToMoney(total) + "</strong></td></tr>");
                            ReportTable.Append("</table>");
                        }
                        else
                        {
                            return "please select pm and vendor";
                        }
                        break;
                    }
            }
            return ReportTable.ToString();
        }

        public DataSet GetPriceChangeReport(Hashtable paramHash)
        {
            string DateFrom = paramHash["DateFrom"].ToString();
            string DateTo = paramHash["DateTo"].ToString();
            int PMSysNo = Int32.Parse(paramHash["PMSysNo"].ToString());
            DateTo = Convert.ToDateTime(DateTo).AddDays(1).ToString(AppConst.DateFormat);
            string sql = @"select sys_user.username,product.sysno as productsysno,product.productid,product.productname,
                            product_price.currentprice,product_status.pricetime,product_price.lastorderprice,
                            case when product.status=@showstatus then '是' else '否' end as statusname
                            from product inner join product_price on product.sysno=product_price.productsysno 
                            inner join product_status on product.sysno=product_status.productsysno 
                            inner join sys_user on product_status.priceusersysno=sys_user.sysno @Filterpmsysno 
                            @Filterdatefrom @Filterdateto
                            order by sys_user.sysno,product.productid";
            sql = sql.Replace("@showstatus", ((int)AppEnum.ProductStatus.Show).ToString());
            sql = sql.Replace("@Filterdatefrom", " and product_status.pricetime >='" + DateFrom + "'");
            sql = sql.Replace("@Filterdateto", " and product_status.pricetime < '" + DateTo + "'");
            if (PMSysNo > 0)
            {
                sql = sql.Replace("@Filterpmsysno", " and sys_user.sysno=" + PMSysNo.ToString());
            }
            else
            {
                sql = sql.Replace("@Filterpmsysno", "");
            }
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;
            return ds;
        }

        public DataSet GetPurchaseCostReport(Hashtable paramHash)
        {   
            int PMSysNo = Int32.Parse(paramHash["PMSysNo"].ToString());
            string sql = @"select p.sysno,p.productid,p.productname,pp.currentprice,pp.unitcost,pp.lastorderprice,su.username from product p inner join product_price pp on p.sysno=pp.productsysno
                           inner join sys_user su on p.ppmusersysno=su.sysno where p.status=1 @Filterpmsysno
                           order by p.createtime desc";
            
            if (PMSysNo > 0)
            {
                sql = sql.Replace("@Filterpmsysno", " and su.sysno=" + PMSysNo.ToString());
            }
            else
            {
                sql = sql.Replace("@Filterpmsysno", "");
            }
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;
            return ds;
        }

        //计算每日平均点击率
        public DataSet GetProductAvgDailyClickReport(Hashtable paramHash)
        {
            int PMSysNo = Int32.Parse(paramHash["PMUserSysNo"].ToString());
            string sql = @"select p.sysno,p.productid,p.productname,pp.currentprice,su.username,
                           inv.accountqty,inv.availableqty+inv.virtualqty as onlineqty,p.avgdailyclick,p.createtime from product p 
                           inner join product_price pp on p.sysno=pp.productsysno
                           inner join inventory inv on p.sysno=inv.productsysno
                           inner join category3 c3 on p.c3sysno=c3.sysno
                           inner join category2 c2 on c3.c2sysno=c2.sysno
                           inner join category1 c1 on c2.c1sysno=c1.sysno 
                           inner join sys_user su on p.ppmusersysno=su.sysno 
                           where p.status=1 @Filterpmsysno @FilterCategory @FilterProduct
                           order by p.avgdailyclick desc";

            if (PMSysNo > 0)
            {
                sql = sql.Replace("@Filterpmsysno", " and su.sysno=" + PMSysNo.ToString());
            }
            else
            {
                sql = sql.Replace("@Filterpmsysno", "");
            }

            if (paramHash.ContainsKey("Category1"))
            {
                int c1sysno = (int)paramHash["Category1"];
                sql = sql.Replace("@FilterCategory", " and c1.sysno=" + c1sysno);
            }
            if (paramHash.ContainsKey("Category2"))
            {
                int c2sysno = (int)paramHash["Category2"];
                sql = sql.Replace("@FilterCategory", " and c2.sysno=" + c2sysno);
            }
            if (paramHash.ContainsKey("Category3"))
            {
                int c3sysno = (int)paramHash["Category3"];
                sql = sql.Replace("@FilterCategory", " and c3.sysno=" + c3sysno);
            }
            sql = sql.Replace("@FilterCategory", "");
            if(paramHash.ContainsKey("ProductSysNo"))
            {
                int productSysno = (int)paramHash["ProductSysNo"];
                sql = sql.Replace("@FilterProduct", " and p.sysno=" + productSysno);
            }
            else
            {
                sql = sql.Replace("@FilterProduct","");
            }

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;
            return ds;
        }


        //计算每日点击率
        public DataSet  GetProductDailyClickReport(Hashtable paramHash)
        {
            StringBuilder ReportTable = new StringBuilder();
            string DateFrom = Convert.ToDateTime(paramHash["DateFrom"].ToString()).ToString("yyyy-MM-dd");
            string DateTo = Convert.ToDateTime(paramHash["DateTo"].ToString()).ToString("yyyy-MM-dd");
            int iDateDiff = Util.DateDiff(Convert.ToDateTime(DateFrom), Convert.ToDateTime(DateTo));
            if (iDateDiff < 0)
                return null;
            string sql = "";
            string curDate = "";
            StringBuilder sbSQL = new StringBuilder();

            int j;
            sbSQL.Append("select p.sysno,p.productid as 商品ID,p.productname as 商品名称,avg(ClickCount) as 平均数量,");

            for (j = 0; j <= iDateDiff; j++)
            {
                curDate = Convert.ToDateTime(DateFrom).AddDays(j).ToString("yyyy-MM-dd");
                sbSQL.Append("'" + curDate + "'=sum(case pd.ClickDate when '" + curDate + "' then ClickCount else 0 end),");
            }

            sql = @"su.username as PM
                           from product p 
                           inner join product_price pp on p.sysno=pp.productsysno 
                           inner join inventory inv on p.sysno=inv.productsysno
                           inner join category3 c3 on p.c3sysno=c3.sysno
                           inner join category2 c2 on c3.c2sysno=c2.sysno
                           inner join category1 c1 on c2.c1sysno=c1.sysno 
                           inner join sys_user su on p.ppmusersysno=su.sysno 
                           inner join Product_DailyClick pd on p.sysno=pd.productsysno 
                           where p.status=1 @Filterpmsysno @FilterCategory @FilterProduct @DateFrom @DateTo @ProdutNameKeyWords
                           group by p.sysno,p.productid,p.productname,pp.currentprice,su.username
                           ";


            if (paramHash.ContainsKey("PMUserSysNo"))
            {
                sql = sql.Replace("@Filterpmsysno", " and su.sysno=" + Util.TrimIntNull(paramHash["PMUserSysNo"]));
            }
            else
            {
                sql = sql.Replace("@Filterpmsysno", "");
            }

            if (paramHash.ContainsKey("Category1"))
            {
                int c1sysno = (int)paramHash["Category1"];
                sql = sql.Replace("@FilterCategory", " and c1.sysno=" + c1sysno);
            }
            if (paramHash.ContainsKey("Category2"))
            {
                int c2sysno = (int)paramHash["Category2"];
                sql = sql.Replace("@FilterCategory", " and c2.sysno=" + c2sysno);
            }
            if (paramHash.ContainsKey("Category3"))
            {
                int c3sysno = (int)paramHash["Category3"];
                sql = sql.Replace("@FilterCategory", " and c3.sysno=" + c3sysno);
            }
            sql = sql.Replace("@FilterCategory", "");
            if (paramHash.ContainsKey("ProductSysNo"))
            {
                int productSysno = (int)paramHash["ProductSysNo"];
                sql = sql.Replace("@FilterProduct", " and p.sysno=" + productSysno);
            }
            else
            {
                sql = sql.Replace("@FilterProduct", "");
            }
            if (paramHash.ContainsKey("DateFrom"))
            {
                sql = sql.Replace("@DateFrom", "and pd.ClickDate>= " + Util.ToSqlString(paramHash["DateFrom"].ToString()));
            }
            else
            {
                sql = sql.Replace("@DateFrom", "");
            }
            if (paramHash.ContainsKey("DateTo"))
            {
                sql = sql.Replace("@DateTo", "and pd.ClickDate<=" + Util.ToSqlString(paramHash["DateTo"].ToString()));
            }
            else
            {
                sql = sql.Replace("@DateTo", "");
            }
            if (paramHash.ContainsKey("ProdutNameKeyWords"))
            {
                string[] KeyWords = (Util.TrimNull(paramHash["ProdutNameKeyWords"].ToString())).Split(' ');
                if (KeyWords.Length == 1)
                {
                    sql = sql.Replace("@ProdutNameKeyWords", "and p.productname like" + Util.ToSqlLikeString(paramHash["ProdutNameKeyWords"].ToString()));
                }
                else
                {
                    string t = "";
                    for (int i = 0; i < KeyWords.Length; i++)
                    {
                        t += "and p.productname like" + Util.ToSqlLikeString(KeyWords[i]);
                    }
                    sql = sql.Replace("@ProdutNameKeyWords", t);
                }
            }
            else
            {
                sql = sql.Replace("@ProdutNameKeyWords", "");
            }
            sbSQL.Append(sql);
            DataSet ds = SqlHelper.ExecuteDataSet(sbSQL.ToString());
            if (!Util.HasMoreRow(ds))
                return null;
            return ds;

        } 
        public DataSet GetPMSaleReport(Hashtable paramHash)
        {
            int j = 0;

            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select v.username as PM,");

            //处理日期
            string datefrom = paramHash["DateFrom"] as string;
            string dateto = paramHash["DateTo"] as string;

            string groupby = paramHash["GroupBy"] as string;
            if (groupby == "Day")
            {
                int iDateDiff = Util.DateDiff(Convert.ToDateTime(datefrom), Convert.ToDateTime(dateto));
                for (j = 0; j <= iDateDiff; j++)
                {
                    string curDate = Convert.ToDateTime(datefrom).AddDays(j).ToString("yyyy-MM-dd");
                    string dispayDate = Convert.ToDateTime(datefrom).AddDays(j).ToString("MM-dd");
                    sbSql.Append("'" + dispayDate + "销售额'=convert(nvarchar,cast(sum(case datestring when '" + curDate + "' then totalSale else 0 end) as money),1) ,");
                    sbSql.Append("'" + dispayDate + "毛利'=convert(nvarchar,cast(sum(case datestring when '" + curDate + "' then totalProfit else 0 end) as money),1),");
                    sbSql.Append("'" + dispayDate + "毛利率'=convert(nvarchar,cast(sum(case datestring when '" + curDate + "' then rate*100 else 0 end) as money),1) + '%',");
                }
            }
            else
            {
                DateTime startMonth = Convert.ToDateTime(Convert.ToDateTime(datefrom).ToString("yyyy-MM") + "-01");
                DateTime endMonth = Convert.ToDateTime(Convert.ToDateTime(dateto).AddMonths(1).ToString("yyyy-MM") + "-01");
                DateTime curMonth = startMonth;
                while (curMonth < endMonth)
                {
                    string curDate = curMonth.ToString("yyyy-MM");
                    string dispayDate = curMonth.ToString("yyyy-MM");
                    sbSql.Append("'" + dispayDate + "销售额'=convert(nvarchar,cast(sum(case datestring when '" + curDate + "' then totalSale else 0 end) as money),1) ,");
                    sbSql.Append("'" + dispayDate + "毛利'=convert(nvarchar,cast(sum(case datestring when '" + curDate + "' then totalProfit else 0 end) as money),1),");
                    sbSql.Append("'" + dispayDate + "毛利率'=convert(nvarchar,cast(sum(case datestring when '" + curDate + "' then rate*100 else 0 end) as money),1) + '%',");

                    curMonth = curMonth.AddMonths(1);
                }
            }
            sbSql.Remove(sbSql.Length - 1, 1);
            sbSql.Append(@" from (
                           select convert(@groupby, outtime, 120) as datestring,sys_user.sysno as pmusersysno,sys_user.username,
				           sum(quantity*(cost + point/@exchangerate)) as totalCost, sum(quantity*(price - so_item.discountamt)) as totalSale, 
                           sum(quantity*(price - so_item.discountamt)) - sum(quantity*(cost + point/@exchangerate)) as totalProfit,                           
                           case when sum(quantity*(price - so_item.discountamt)) = 0 then -1.0000 else round( (sum(quantity*(price - so_item.discountamt)) - sum(quantity*(cost + point/@exchangerate)) ) / sum(quantity*(price - so_item.discountamt)) ,4) end as rate,
                           sum(quantity) as totalNumber 
			               from so_master 
				                inner join so_item on so_master.sysno = so_item.sosysno 
                                inner join product on so_item.productsysno = product.sysno 
				                inner join sys_user on product.pmusersysno = sys_user.sysno 
								inner join manufacturer on product.manufacturersysno = manufacturer.sysno
                                inner join category3 on product.c3sysno = category3.sysno 
								inner join category2 on category3.c2sysno = category2.sysno 
								inner join category1 on category2.c1sysno = category1.sysno
			                where
				                so_master.status = @status
			                and outtime>=@datefrom and outtime<@dateto 
				                @iswholesale @category @ppmusersysno @manufacturersysno
			                group by sys_user.sysno,username,convert(@groupby, outtime, 120) ) v group by v.pmusersysno,v.username");

            string sql = sbSql.ToString();
            sql = sql.Replace("@datefrom", "cast(" + Util.ToSqlString(datefrom) + " as datetime)");
            sql = sql.Replace("@dateto", "cast(" + Util.ToSqlEndDate(dateto) + " as datetime)");
            sql = sql.Replace("@exchangerate", Int32.Parse(AppConst.ExchangeRate.ToString()).ToString());

            if (groupby == "Day")
            {
                sql = sql.Replace("@groupby", "nvarchar(10)");
            }
            else
            {
                sql = sql.Replace("@groupby", "nvarchar(7)");
            }

            //设置订单状态
            int status = (int)AppEnum.SOStatus.OutStock;
            sql = sql.Replace("@status", status.ToString());

            //处理是否批发
            if (paramHash.ContainsKey("IsWholeSale"))
            {
                int iswholesale = (int)paramHash["IsWholeSale"];
                sql = sql.Replace("@iswholesale", " and iswholesale = " + iswholesale.ToString());
            }
            else
            {
                sql = sql.Replace("@iswholesale", "");
            }
            if (paramHash.ContainsKey("Category"))
            {
                sql = sql.Replace("@category", " and " + (string)paramHash["Category"]);
            }
            else
            {
                sql = sql.Replace("@category", "");
            }
            //处理pm
            if (paramHash.ContainsKey("PPMUserSysNo"))
            {
                int ppmUserSysNo = (int)paramHash["PPMUserSysNo"];
                sql = sql.Replace("@ppmusersysno", " and pmusersysno = " + ppmUserSysNo.ToString());
            }
            else
            {
                sql = sql.Replace("@ppmusersysno", "");
            }
            //处理厂商
            if (paramHash.ContainsKey("ManufacturerSysNo"))
            {
                int manufacturerSysNo = (int)paramHash["ManufacturerSysNo"];
                sql = sql.Replace("@manufacturersysno", " and manufacturer.sysno = " + manufacturerSysNo.ToString());
            }
            else
            {
                sql = sql.Replace("@manufacturersysno", "");
            }

            return SqlHelper.ExecuteDataSet(sql);
        }

        private string GetInventoryReportTypeFilterString(int InventoryReportTypeSysNo)
        {
            DataSet ds = GetInventoryReportTypeDs(InventoryReportTypeSysNo);
            if (!Util.HasMoreRow(ds))
                return " 1=1 ";
            string sReturn = "";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sReturn += " or " + Util.TrimNull(dr["categorytype"]) + " = '" + Util.TrimNull(dr["categorysysno"]) + "'";
            }
            sReturn = " ( 1=0 " + sReturn + " ) ";
            return sReturn;
        }

        public DataSet GetInventoryReportTypeDs(int InventoryReportTypeSysNo)
        {
            string sql = "select categorytype,categorysysno,categoryname from inventory_report where status=0 and inventoryreporttype = " + InventoryReportTypeSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;
            return ds;
        }

        public DataSet GetWarehouseWorkload(Hashtable paramHash)
        {
            //是否出过库，用是否有出库校验人员来判断（如果用已出库的状态来判断，会把退货的减掉）
            string datefrom = "";
            if (paramHash.ContainsKey("DateFrom"))
                datefrom = Util.ToSqlString(paramHash["DateFrom"].ToString());

            string dateto = "";
            if (paramHash.ContainsKey("DateTo"))
                dateto = Util.ToSqlEndDate(paramHash["DateTo"].ToString());

            int postatus = (int)AppEnum.POStatus.InStock;

            StringBuilder sb = new StringBuilder();

            //入库商品总数量 tbl0
            sb.Append(@"select c.sysno,c.username,sum(abs(b.quantity)) as inquantity from po_master a inner join po_item b on a.sysno=b.posysno
                        inner join sys_user c on a.inusersysno = c.sysno 
                        where 1=1 @postatus @instockdatefrom @instockdateto 
                        group by c.sysno,c.username;");
            //入库商品总批次 tbl1
            sb.Append(@"select count(b.sysno) as inpoitemquantity from po_master a inner join po_item b on a.sysno=b.posysno
                        where 1=1 @postatus @instockdatefrom @instockdateto;");

            //打单 tbl2
            sb.Append(@"select count(*) as SOCount from so_master a where a.checkqtyusersysno > 0 @outstockdatefrom @outstockdateto;");
            //发票 tbl3
            sb.Append(@"select count(*) as InvoiceCount from so_master a where a.checkqtyusersysno > 0 and a.isvat = 1 @outstockdatefrom @outstockdateto;");
            //出库商品总数量 tbl4
            sb.Append(@"select sum(b.quantity) as SOProductCount from so_master a inner join so_item b on a.sysno=b.sosysno where a.checkqtyusersysno > 0 @outstockdatefrom @outstockdateto;");
            //出库商品总批次 tbl5
            sb.Append(@"select count(b.sysno) as SOItemCount from so_master a inner join so_item b on a.sysno=b.sosysno where a.checkqtyusersysno > 0 @outstockdatefrom @outstockdateto;");
            //打包单量 tbl6
            sb.Append(@"select b.sysno,b.username,c.shiptypename, count(a.sysno) as outquantity 
                        from so_master a inner join sys_user b on a.checkqtyusersysno = b.sysno 
                        inner join shiptype c on a.shiptypesysno = c.sysno 
                        where 1=1 @outstockdatefrom @outstockdateto
                        group by b.sysno,b.username,c.shiptypename
                        order by username,shiptypename");

            string sql = sb.ToString();
            sql = sql.Replace("@postatus", " and a.status=" + postatus);
            if (datefrom.Length > 0)
            {
                sql = sql.Replace("@instockdatefrom", " and a.intime >= " + datefrom);
                sql = sql.Replace("@outstockdatefrom", " and a.outtime >= " + datefrom);
            }
            else
            {
                sql = sql.Replace("@instockdatefrom", "");
                sql = sql.Replace("@outstockdatefrom", "");
            }

            if (dateto.Length > 0)
            {
                sql = sql.Replace("@instockdateto", " and a.intime <= " + dateto);
                sql = sql.Replace("@outstockdateto", " and a.outtime <= " + dateto);
            }
            else
            {
                sql = sql.Replace("@instockdateto", "");
                sql = sql.Replace("@outstockdateto", "");
            }

            return SqlHelper.ExecuteDataSet(sql);
        }

//        public DataSet GetPointReport(Hashtable paramHash)
//        {
//            string DateFrom = paramHash["DateFrom"].ToString();
//            string DateTo = paramHash["DateTo"].ToString();
//            string Type = "";
//            if (paramHash.ContainsKey("Type"))
//                Type = paramHash["Type"].ToString();

//            string sql = @"select pointlogtype, sum(pointamount) as sumpoint from customer_pointlog 
//                           where CreateTime >= @datefrom and CreateTime <= @dateto @type 
//                           group by pointlogtype order by sum(pointamount) desc";
//            sql = sql.Replace("@datefrom", Util.ToSqlString(DateFrom));
//            sql = sql.Replace("@dateto", Util.ToSqlEndDate(DateTo));
//            if (Type.Length > 0)
//                sql = sql.Replace("@type", " and pointlogtype=" + Type);
//            else
//                sql = sql.Replace("@type", "");

//            return SqlHelper.ExecuteDataSet(sql);
//        }
        public DataSet GetPointReport(Hashtable paramHash)
        {
            string DateFrom = paramHash["DateFrom"].ToString();
            string DateTo = paramHash["DateTo"].ToString();
            string sql = @"select PointLogType, sum(PointAmount) as sumpoint,PMUserSysNo from Customer_PointRequest 
                           where AddTime >= @datefrom and AddTime <= @dateto @type @PMUserSysNo
                           group by PointLogType,PMUserSysNo order by sum(pointamount),PMUserSysNo desc";
            sql = sql.Replace("@datefrom", Util.ToSqlString(DateFrom));
            sql = sql.Replace("@dateto", Util.ToSqlEndDate(DateTo));
            if (paramHash.Contains("PointLogType"))
                sql = sql.Replace("@type", " and PointLogType=" + Util.ToSqlString(paramHash["PointLogType"].ToString()));
            else
                sql = sql.Replace("@type", "");

            if (paramHash.Contains("PMUserSysNo"))
                sql = sql.Replace("@PMUserSysNo", "and PMUserSysNo=" + Util.ToSqlString(paramHash["PMUserSysNo"].ToString()));
            else
                sql = sql.Replace("@PMUserSysNo", "");
            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetPointView(Hashtable paramHash)
        {
            string DateFrom = paramHash["DateFrom"].ToString();
            string DateTo = paramHash["DateTo"].ToString();
            string Type = paramHash["Type"].ToString();

            string sql = @"select customersysno,customername,pointlogtype, pointamount,createtime,memo 
                            from customer_pointlog cp inner join customer c on cp.customersysno=c.sysno
                            where CreateTime >= @datefrom and CreateTime <= @dateto and pointlogtype = @type
                            order by cp.sysno";
            sql = sql.Replace("@datefrom", Util.ToSqlString(DateFrom));
            sql = sql.Replace("@dateto", Util.ToSqlEndDate(DateTo));
            sql = sql.Replace("@type",Type);

            return SqlHelper.ExecuteDataSet(sql);
        }

        /// <summary>
        /// 获取RMA每月清单
        /// </summary>
        /// <param name="paramHash"></param>
        /// <returns></returns>
        public DataSet GetRMAMonthList(Hashtable paramHash)
        {
            string sql = null;
            if (paramHash.ContainsKey("ListType"))
            {
                if (paramHash["ListType"].ToString() == "1")
                {//本月退货入库物品清单
                    sql = @"select distinct
						    rma_request.sosysno,
						    customername,
						    productid, productname, rma_register.sysno as registersysno, rma_register.status as requestStatus,
						    rma_register.* , rma_request.recvtime  ,customer.VIPRank 
							from
								rma_register (nolock)
								inner join rma_request_item (nolock) on rma_register.sysno = rma_request_item.registersysno
								inner join rma_request (nolock) on rma_request.sysno = rma_request_item.requestsysno					
								inner join customer (nolock) on rma_request.customersysno = customer.sysno
								inner join product (nolock) on rma_register.productsysno = product.sysno 
                                inner join ( select rma_return_item.registersysno from rma_return_item (nolock) 
								              inner join rma_return (nolock) on rma_return.sysno = rma_return_item.returnsysno 
                                              where rma_return.returntime between '" + paramHash["DateStart"] + "' and " + Util.ToSqlEndDate(paramHash["DateEnd"].ToString()) + " and rma_return.status=" + (int)AppEnum.RMAReturnStatus.Returned + ") as returnlist on  rma_register.sysno= returnlist.registersysno  order by registersysno desc ";
                }
                else if (paramHash["ListType"].ToString() == "2")
                {//本月退款未入库清单
                    sql = @"select distinct
						    rma_request.sosysno,
						    customername,
						    productid, productname, rma_register.sysno as registersysno, rma_register.status as requestStatus,
						    rma_register.* , rma_request.recvtime  ,customer.VIPRank 
							from
								rma_register (nolock) 
								inner join rma_request_item (nolock) on rma_register.sysno = rma_request_item.registersysno
								inner join rma_request (nolock) on rma_request.sysno = rma_request_item.requestsysno					
								inner join customer (nolock) on rma_request.customersysno = customer.sysno
								inner join product (nolock) on rma_register.productsysno = product.sysno 
                                inner join ( select rma_refund_item.registersysno from rma_refund_item (nolock)
					            inner join rma_refund (nolock) on rma_refund.sysno = rma_refund_item.refundsysno 
                                where rma_refund.refundtime between '" + paramHash["DateStart"] + "' and " + Util.ToSqlEndDate(paramHash["DateEnd"].ToString()) + " and rma_refund.status=" + (int)AppEnum.RMARefundStatus.Refunded + ") as refundlist on  rma_register.sysno= refundlist.registersysno and (rma_register.returnstatus<>" + (int)AppEnum.RMAReturnStatus.Returned + "  or rma_register.returnstatus is null)order by registersysno desc ";
                }
                else if (paramHash["ListType"].ToString() == "3")
                {//当前供货商所欠物品清单
                    sql = @"select 
						    rma_request.sosysno,
						    customername,
						    productid, productname, rma_register.sysno as registersysno, rma_register.status as requestStatus,
						    rma_register.* , rma_request.recvtime  ,customer.VIPRank 
							from
								rma_register (nolock) 
								inner join rma_request_item (nolock) on rma_register.sysno = rma_request_item.registersysno
								inner join rma_request (nolock) on rma_request.sysno = rma_request_item.requestsysno					
								inner join customer (nolock) on rma_request.customersysno = customer.sysno
								inner join product (nolock) on rma_register.productsysno = product.sysno 
                                inner join ( select rma_outbound_item.registersysno from rma_outbound_item (nolock)  
								              inner join rma_outbound (nolock) on rma_outbound.sysno = rma_outbound_item.outboundsysno 
                                              where rma_outbound.outtime between '" + paramHash["DateStart"] + "' and " + Util.ToSqlEndDate(paramHash["DateEnd"].ToString()) + " and (rma_outbound.status<>" + (int)AppEnum.RMAOutBoundStatus.Responsed + " or rma_outbound.status is null)) as outboundlist on  rma_register.sysno= outboundlist.registersysno  where  rma_register.outboundstatus=" + (int)AppEnum.RMAOutBoundStatus.SendAlready + "   order by registersysno desc ";

                }
                else if (paramHash["ListType"].ToString() == "4")
                {//本月发新品未入库清单
                    sql = @"select distinct
						    rma_request.sosysno,
						    customername,
						    productid, productname, rma_register.sysno as registersysno, rma_register.status as requestStatus,
						    rma_register.* , rma_request.recvtime  ,customer.VIPRank 
							from
								rma_register (nolock)
								inner join rma_request_item (nolock) on rma_register.sysno = rma_request_item.registersysno
								inner join rma_request (nolock) on rma_request.sysno = rma_request_item.requestsysno					
								inner join customer (nolock) on rma_request.customersysno = customer.sysno
								inner join product (nolock) on rma_register.productsysno = product.sysno 
                                inner join ( select rma_revert_item.registersysno from rma_revert_item (nolock) 
								              inner join rma_revert (nolock) on rma_revert.sysno = rma_revert_item.revertsysno 
                                              where rma_revert.outtime between '" + paramHash["DateStart"] + "' and " + Util.ToSqlEndDate(paramHash["DateEnd"].ToString()) + ") as revertnewlist on  rma_register.sysno= revertnewlist.registersysno where  rma_register.NewProductStatus<>" + (int)AppEnum.NewProductStatus.Origin + "  and (rma_register.returnstatus!=" + (int)AppEnum.RMAReturnStatus.Returned + " or rma_register.returnstatus is null ) order by registersysno desc ";

                }
                else if (paramHash["ListType"].ToString() == "5")
                {//超时未结束的物品清单
                    sql = @"select distinct
                                  rma_request.sosysno,
                                  customername,
                                  productid, productname, rma_register.sysno as registersysno, rma_register.status as requestStatus,
                                  rma_register.* , rma_request.recvtime  ,customer.VIPRank 
                                  from
                                      rma_register (nolock) 
                                      inner join rma_request_item (nolock) on rma_register.sysno = rma_request_item.registersysno
                                      inner join rma_request (nolock) on rma_request.sysno = rma_request_item.requestsysno	
                                      inner join customer (nolock) on rma_request.customersysno = customer.sysno
                                      inner join product (nolock) on rma_register.productsysno = product.sysno	
                                      left join rma_revert_item (nolock) on rma_register.sysno = rma_revert_item.registersysno
                                      left join rma_revert (nolock) on rma_revert.sysno = rma_revert_item.revertsysno
                                      left join rma_refund_item (nolock) on rma_refund_item.registersysno = rma_register.sysno
                                      left join rma_refund (nolock) on rma_refund.sysno = rma_refund_item.refundsysno			

                                      where rma_request.recvtime between '" + paramHash["DateStart"] + "' and  " + Util.ToSqlEndDate(paramHash["DateEnd"].ToString()) + " ";
                    sql += @" and (rma_refund.refundtime>rma_request.recvtime+21 
                                               or (refundtime is null and (rma_revert.outtime>rma_request.recvtime+21 or rma_revert.outtime is null)))
                                               and rma_request.recvtime<='" + DateTime.Now.AddDays(-21) + "' order by registersysno desc";

                }
            }


            return SqlHelper.ExecuteDataSet(sql);

        }

        public DataSet GetRMARate(Hashtable paramHash)
        {
            string sql = @"select product.sysno,product.productid,productname,isnull(saletotal ,0 ) as saleqty,isnull(rmatotal , 0) as rmaqty ,isnull(refundtotal , 0) as refundqty ,
                                  sys_user.username as PM , manufacturer.manufacturername ,product.status , vendor.vendorname as lastvendor
							from product (nolock)
							inner join
							(select productsysno , sum(quantity) as saletotal
							from v_so_master (nolock) inner join v_so_item (nolock) on v_so_master.sysno = v_so_item.sosysno
							where v_so_master.status = @sostatus @outtimeFrom @outtimeTo
							group by productsysno
                             
							) as so on product.sysno = so.productsysno  
                      		                  					
                            left join
							(select productsysno , count(rma_register.sysno) as rmatotal
							from rma_register (nolock)
                            inner join rma_request_item (nolock) on rma_request_item.registersysno = rma_register.sysno
                            inner join rma_request (nolock) on rma_request_item.requestsysno = rma_request.sysno
							where @registerstatus @unrefund @recvtimeFrom @recvtimeTo  @IsRejectRMA
							group by productsysno
							) as rma on product.sysno = rma.productsysno
                            left join
                            (select productsysno , count(rma_register.sysno) as refundtotal
                             from rma_register (nolock)
                             inner join rma_refund_item (nolock) on rma_refund_item.registersysno = rma_register.sysno
                             inner join rma_refund (nolock) on rma_refund.sysno = rma_refund_item.refundsysno
                             inner join rma_request_item (nolock) on rma_request_item.registersysno = rma_register.sysno
                             inner join rma_request (nolock) on rma_request_item.requestsysno = rma_request.sysno
                             where @registerstatus and refundstatus=@refundstatus @refundtimeFrom @refundtimeTo  @IsRejectRMA
                             group by productsysno
                            ) as refund on refund.productsysno = product.sysno
                           inner join Category3 (nolock) on Product.C3SysNo = Category3.SysNo
                           inner join Category2 (nolock) on Category3.C2SysNo = Category2.SysNo
                           inner join Category1 (nolock) on Category2.C1SysNo = Category1.SysNo
                           inner join manufacturer (nolock) on product.manufacturersysno = manufacturer.sysno
                           inner join sys_user (nolock) on product.pmusersysno = sys_user.sysno
                           inner join product_lastpoinfo (nolock) on product.sysno = product_lastpoinfo.productsysno
                           inner join vendor (nolock) on product_lastpoinfo.lastvendorsysno = vendor.sysno
							where 1=1 @product @pstatus @ptype @manufacturer @category @PM
                           order by product.productid
                          ";

            sql = sql.Replace("@sostatus", ((int)AppEnum.SOStatus.OutStock).ToString());
            sql = sql.Replace("@registerstatus", "( rma_register.status = " + ((int)AppEnum.RMARequestStatus.Handling).ToString() + " or rma_register.status = " + ((int)AppEnum.RMARequestStatus.Closed).ToString() + ") ");
            sql = sql.Replace("@unrefund", " and ( refundstatus is null or refundstatus=" + ((int)AppEnum.RMARefundStatus.Abandon).ToString() + ")");
            sql = sql.Replace("@refundstatus", ((int)AppEnum.RMARefundStatus.Refunded).ToString());

            if (paramHash.ContainsKey("DateFrom"))
            {
                sql = sql.Replace("@outtimeFrom", " and outtime>=" + Util.ToSqlString(paramHash["DateFrom"].ToString()));
                sql = sql.Replace("@recvtimeFrom", " and recvtime>=" + Util.ToSqlString(paramHash["DateFrom"].ToString()));
                sql = sql.Replace("@refundtimeFrom", " and refundtime>=" + Util.ToSqlString(paramHash["DateFrom"].ToString()));
            }
            else
            {
                sql = sql.Replace("@outtimeFrom", "");
                sql = sql.Replace("@recvtimeFrom", "");
                sql = sql.Replace("@refundtimeFrom", "");
            }

            if (paramHash.ContainsKey("DateTo"))
            {
                sql = sql.Replace("@outtimeTo", " and outtime<=" + Util.ToSqlEndDate(paramHash["DateTo"].ToString()));
                sql = sql.Replace("@recvtimeTo", " and recvtime<=" + Util.ToSqlEndDate(paramHash["DateTo"].ToString()));
                sql = sql.Replace("@refundtimeTo", " and refundtime<=" + Util.ToSqlEndDate(paramHash["DateTo"].ToString()));
            }
            else
            {
                sql = sql.Replace("@recvtimeTo", "");
                sql = sql.Replace("@refundtimeTo", "");
            }


            if (paramHash.ContainsKey("ProductSysNo"))
                sql = sql.Replace("@product", "and  product.sysno=" + paramHash["ProductSysNo"].ToString());
            else
                sql = sql.Replace("@product", "");

            if (paramHash.ContainsKey("ProductStatus"))
                sql = sql.Replace("@pstatus", " and product.status=" + paramHash["ProductStatus"].ToString());
            else
                sql = sql.Replace("@pstatus", "");

            if (paramHash.ContainsKey("ProductType"))
                sql = sql.Replace("@ptype", " and product.producttype=" + paramHash["ProductType"].ToString());
            else
                sql = sql.Replace("@ptype", "");

            if (paramHash.ContainsKey("Manufacturer"))
                sql = sql.Replace("@manufacturer", " and Manufacturer.ManufacturerName like " + Util.ToSqlLikeString(paramHash["Manufacturer"].ToString()));
            else
                sql = sql.Replace("@manufacturer", "");

            if (paramHash.ContainsKey("Category"))
                sql = sql.Replace("@category", " and " + paramHash["Category"].ToString());
            else
                sql = sql.Replace("@category", "");

            if (paramHash.ContainsKey("PM"))
                sql = sql.Replace("@PM", " and product.pmusersysno = " + paramHash["PM"].ToString());
            else
                sql = sql.Replace("@PM", "");
            if (paramHash.ContainsKey("IsRejectRMA"))
                sql = sql.Replace("@IsRejectRMA", "and RMA_Request.IsRejectRMA =" + paramHash["IsRejectRMA"].ToString());
            else
                sql = sql.Replace("@IsRejectRMA", "");

            return SqlHelper.ExecuteDataSet(sql);

        }
        public DataSet GetRMARateRevised(Hashtable paramHash)
        {
            string sql = @"select sysno,productid,productname,sum(saleqty) as saleqty , sum(rmaqty) as rmaqty ,sum(refundqty) as refundqty,sum(NewRevertQty) as NewRevertQty,
                                  PM , manufacturername ,status ,lastvendor from 
                          (
                            select product.sysno,product.productid,productname,isnull(saletotal ,0 ) as saleqty,isnull(rmatotal , 0) as rmaqty ,isnull(refundtotal , 0) as refundqty ,isnull(NewRevertTotal , 0) as NewRevertQty ,
                                  sys_user.username as PM , manufacturer.manufacturername ,product.status , vendor.vendorname as lastvendor
							from product (nolock)
							inner join
							(select productsysno , sum(quantity) as saletotal
							from v_so_master (nolock) inner join v_so_item (nolock) on v_so_master.sysno = v_so_item.sosysno
							where v_so_master.status = @sostatus @outtimeFrom @outtimeTo
							group by productsysno
                             
							) as so on product.sysno = so.productsysno  
                      		                  					
                            left join
							(select productsysno , count(rma_register.sysno) as rmatotal
							from rma_register (nolock)
                            inner join rma_request_item (nolock) on rma_request_item.registersysno = rma_register.sysno
                            inner join rma_request (nolock) on rma_request_item.requestsysno = rma_request.sysno
							where @registerstatus @unrefund @recvtimeFrom @recvtimeTo  @IsRejectRMA @UnNewProductStatus
							group by productsysno
							) as rma on product.sysno = rma.productsysno
                            left join
							(select productsysno , count(rma_register.sysno) as NewRevertTotal
							from rma_register (nolock)
                            inner join rma_request_item (nolock) on rma_request_item.registersysno = rma_register.sysno
                            inner join rma_request (nolock) on rma_request_item.requestsysno = rma_request.sysno
							where @registerstatus @unrefund @recvtimeFrom @recvtimeTo  @IsRejectRMA @NewProductStatus
							group by productsysno
							) as NewRevert on product.sysno = NewRevert.productsysno
                            left join
                            (select productsysno , count(rma_register.sysno) as refundtotal
                             from rma_register (nolock)
                             inner join rma_refund_item (nolock) on rma_refund_item.registersysno = rma_register.sysno
                             inner join rma_refund (nolock) on rma_refund.sysno = rma_refund_item.refundsysno
                             inner join rma_request_item (nolock) on rma_request_item.registersysno = rma_register.sysno
                             inner join rma_request (nolock) on rma_request_item.requestsysno = rma_request.sysno
                             where @registerstatus and refundstatus=@refundstatus @refundtimeFrom @refundtimeTo  @IsRejectRMA
                             group by productsysno
                            ) as refund on refund.productsysno = product.sysno
                           inner join Category3 (nolock) on Product.C3SysNo = Category3.SysNo
                           inner join Category2 (nolock) on Category3.C2SysNo = Category2.SysNo
                           inner join Category1 (nolock) on Category2.C1SysNo = Category1.SysNo
                           inner join manufacturer (nolock) on product.manufacturersysno = manufacturer.sysno
                           inner join sys_user (nolock) on product.pmusersysno = sys_user.sysno
                           inner join product_lastpoinfo (nolock) on product.sysno = product_lastpoinfo.productsysno
                           inner join vendor (nolock) on product_lastpoinfo.lastvendorsysno = vendor.sysno
						   where 1=1 @product @pstatus @ptype @manufacturer @category @PM
                           union all
                           (                         
                             select product.sysno,product.productid,productname,0 as saleqty,isnull(rmatotal , 0) as rmaqty ,isnull(refundtotal , 0) as refundqty ,isnull(NewRevertTotal , 0) as NewRevertQty ,
                                  sys_user.username as PM , manufacturer.manufacturername ,product.status , vendor.vendorname as lastvendor
							from product (nolock) 
                            
                            inner join
							(select productsysno , sum(quantity) as saletotal
							from v_so_master (nolock) inner join v_so_item (nolock) on v_so_master.sysno = v_so_item.sosysno
							where v_so_master.status = @sostatus @timeFrom  @opration  @timeTo
							group by productsysno
                            having 
                              productsysno not in 
                              ( select productsysno 
							    from v_so_master (nolock) inner join v_so_item (nolock) on v_so_master.sysno = v_so_item.sosysno
							    where v_so_master.status = @sostatus @outtimeFrom @outtimeTo
							  )
                             
							) as so on product.sysno = so.productsysno                   		                  					
                            left join
							( select productsysno , count(rma_register.sysno) as rmatotal
							  from rma_register (nolock)
                              inner join rma_request_item (nolock) on rma_request_item.registersysno = rma_register.sysno
                              inner join rma_request (nolock) on rma_request_item.requestsysno = rma_request.sysno
							  where @registerstatus @unrefund @recvtimeFrom @recvtimeTo  @IsRejectRMA @UnNewProductStatus
							  group by productsysno
							) as rma  on product.sysno = rma.productsysno

                            left join
							(select productsysno , count(rma_register.sysno) as NewRevertTotal
							from rma_register (nolock)
                            inner join rma_request_item (nolock) on rma_request_item.registersysno = rma_register.sysno
                            inner join rma_request (nolock) on rma_request_item.requestsysno = rma_request.sysno
							where @registerstatus @unrefund @recvtimeFrom @recvtimeTo  @IsRejectRMA @NewProductStatus
							group by productsysno
							) as NewRevert on product.sysno = NewRevert.productsysno

                             left join
                            (select productsysno , count(rma_register.sysno) as refundtotal
                             from rma_register (nolock)
                             inner join rma_refund_item (nolock) on rma_refund_item.registersysno = rma_register.sysno
                             inner join rma_refund (nolock) on rma_refund.sysno = rma_refund_item.refundsysno
                             inner join rma_request_item (nolock) on rma_request_item.registersysno = rma_register.sysno
                             inner join rma_request (nolock) on rma_request_item.requestsysno = rma_request.sysno
                             where @registerstatus and refundstatus=@refundstatus @refundtimeFrom @refundtimeTo  @IsRejectRMA
                             group by productsysno
                            ) as refund  on refund.productsysno = product.sysno
                           inner join Category3 (nolock) on Product.C3SysNo = Category3.SysNo
                           inner join Category2 (nolock) on Category3.C2SysNo = Category2.SysNo
                           inner join Category1 (nolock) on Category2.C1SysNo = Category1.SysNo
                           inner join manufacturer (nolock) on product.manufacturersysno = manufacturer.sysno
                           inner join sys_user (nolock) on product.pmusersysno = sys_user.sysno
                           inner join product_lastpoinfo (nolock) on product.sysno = product_lastpoinfo.productsysno
                           inner join vendor (nolock) on product_lastpoinfo.lastvendorsysno = vendor.sysno
						   where 1=1  @product @pstatus @ptype @manufacturer @category @PM  and (rmatotal>0  or refundtotal>0 or NewRevertTotal>0) 
                           
                            ) )   c   group by c.sysno,c.productid,c.productname,c.pm,c.manufacturername,c.status,c.lastvendor";

            sql = sql.Replace("@sostatus", ((int)AppEnum.SOStatus.OutStock).ToString());
            sql = sql.Replace("@registerstatus", "( rma_register.status = " + ((int)AppEnum.RMARequestStatus.Handling).ToString() + " or rma_register.status = " + ((int)AppEnum.RMARequestStatus.Closed).ToString() + ") ");
            sql = sql.Replace("@unrefund", " and ( refundstatus is null or refundstatus=" + ((int)AppEnum.RMARefundStatus.Abandon).ToString() + ")");
            sql = sql.Replace("@refundstatus", ((int)AppEnum.RMARefundStatus.Refunded).ToString());
            sql = sql.Replace("@NewProductStatus", " and rma_register.NewProductStatus=" + ((int)AppEnum.NewProductStatus.NewProduct).ToString());
            sql = sql.Replace("@UnNewProductStatus", " and isnull(rma_register.NewProductStatus,-999999)!=" + ((int)AppEnum.NewProductStatus.NewProduct).ToString());

            if (paramHash.ContainsKey("DateFrom"))
            {
                sql = sql.Replace("@outtimeFrom", " and outtime>=" + Util.ToSqlString(paramHash["DateFrom"].ToString()));
                sql = sql.Replace("@timeFrom", " and outtime<" + Util.ToSqlString(paramHash["DateFrom"].ToString()));
                if (paramHash.ContainsKey("DateTo"))
                {
                    sql = sql.Replace("@opration", "or");

                }
                sql = sql.Replace("@recvtimeFrom", " and recvtime>=" + Util.ToSqlString(paramHash["DateFrom"].ToString()));
                sql = sql.Replace("@refundtimeFrom", " and refundtime>=" + Util.ToSqlString(paramHash["DateFrom"].ToString()));
            }
            else
            {
                sql = sql.Replace("@timeFrom", "");
                sql = sql.Replace("@opration", "");
                sql = sql.Replace("@outtimeFrom", "");
                sql = sql.Replace("@recvtimeFrom", "");
                sql = sql.Replace("@refundtimeFrom", "");
            }

            if (paramHash.ContainsKey("DateTo"))
            {
                sql = sql.Replace("@outtimeTo", "    and outtime<=" + Util.ToSqlEndDate(paramHash["DateTo"].ToString()));
                sql = sql.Replace("@timeTo", "       outtime>" + Util.ToSqlEndDate(paramHash["DateTo"].ToString()));
                sql = sql.Replace("@recvtimeTo", "   and recvtime<=" + Util.ToSqlEndDate(paramHash["DateTo"].ToString()));
                sql = sql.Replace("@refundtimeTo", " and refundtime<=" + Util.ToSqlEndDate(paramHash["DateTo"].ToString()));
            }
            else
            {

                sql = sql.Replace("@recvtimeTo", "");
                sql = sql.Replace("@refundtimeTo", "");
            }


            if (paramHash.ContainsKey("ProductSysNo"))
                sql = sql.Replace("@product", "and  product.sysno=" + paramHash["ProductSysNo"].ToString());
            else
                sql = sql.Replace("@product", "");

            if (paramHash.ContainsKey("ProductStatus"))
                sql = sql.Replace("@pstatus", " and product.status=" + paramHash["ProductStatus"].ToString());
            else
                sql = sql.Replace("@pstatus", "");

            if (paramHash.ContainsKey("ProductType"))
                sql = sql.Replace("@ptype", " and product.producttype=" + paramHash["ProductType"].ToString());
            else
                sql = sql.Replace("@ptype", "");

            if (paramHash.ContainsKey("Manufacturer"))
                sql = sql.Replace("@manufacturer", " and Manufacturer.ManufacturerName like " + Util.ToSqlLikeString(paramHash["Manufacturer"].ToString()));
            else
                sql = sql.Replace("@manufacturer", "");

            if (paramHash.ContainsKey("Category"))
                sql = sql.Replace("@category", " and " + paramHash["Category"].ToString());
            else
                sql = sql.Replace("@category", "");

            if (paramHash.ContainsKey("PM"))
                sql = sql.Replace("@PM", " and product.pmusersysno = " + paramHash["PM"].ToString());
            else
                sql = sql.Replace("@PM", "");
            if (paramHash.ContainsKey("IsRejectRMA"))
                sql = sql.Replace("@IsRejectRMA", "and RMA_Request.IsRejectRMA =" + paramHash["IsRejectRMA"].ToString());
            else
                sql = sql.Replace("@IsRejectRMA", "");

            return SqlHelper.ExecuteDataSet(sql);

        }

        public DataSet GetRMAReportByCategory(Hashtable paramHash)
        {
            string sql = @"select @Category as Category , productqty , refundcash , refundpoint 
                            from
   						       (  select @Category , count(RMA_Register.SysNo) as productqty , 
                                         sum(RMA_Refund_Item.RefundCash) as refundcash , sum(RMA_Refund_Item.RefundPoint) as refundpoint 
                                         
                                  from   RMA_Refund (nolock)
                                  inner  join RMA_Refund_Item (nolock) on RMA_Refund.SysNo = RMA_Refund_Item.RefundSysNo
                                  inner  join RMA_Register (nolock) on RMA_Refund_Item.registerSysNo = RMA_Register.SysNo
                                  inner  join rma_request_item (nolock) on rma_request_item.registersysno = rma_register.sysno
                                  inner  join rma_request (nolock) on rma_request_item.requestsysno = rma_request.sysno
                                  inner  join Product (nolock) on RMA_Register.ProductSysNo = Product.SysNo
                                  inner  join Category3 (nolock) on Product.C3SysNo = Category3.SysNo
                                  inner  join Category2 (nolock) on Category3.C2SysNo = Category2.SysNo
                                  inner  join Category1 (nolock) on Category2.C1SysNo = Category1.SysNo
                                  where  RMA_Refund.status = @RefundStatus
                                  and    RMA_Refund.RefundTime >= @DateFrom
                                  and    RMA_Refund.RefundTime <= @DateTo
                                         @CSysNo
                                         @IsRejectRMA
                                  group by @Category
   					           )  as a  order by productqty desc";
            sql = sql.Replace("@RefundStatus", ((int)AppEnum.RMARefundStatus.Refunded).ToString());
            sql = sql.Replace("@DateFrom", Util.ToSqlString(paramHash["DateFrom"].ToString()));
            sql = sql.Replace("@DateTo", Util.ToSqlEndDate(paramHash["DateTo"].ToString()));
            sql = sql.Replace("@Category", paramHash["Category"].ToString());

            if (paramHash.ContainsKey("CategorySysNo"))
                sql = sql.Replace("@CSysNo", " and " + paramHash["CategorySysNo"].ToString());
            else
                sql = sql.Replace("@CSysNo", "");
            if (paramHash.ContainsKey("IsRejectRMA"))
                sql = sql.Replace("@IsRejectRMA", "and RMA_Request.IsRejectRMA =" + paramHash["IsRejectRMA"].ToString());
            else
                sql = sql.Replace("@IsRejectRMA", "");


            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetRMAReportByDate(Hashtable paramHash)
        {
            string sql = @"
							CREATE TABLE #t(ddd nvarchar(10) PRIMARY KEY)

							Declare @date datetime
							Set @date=@DateFrom
							While convert(@groupby, @date , 120) <= convert(@groupby, @DateTo , 120) 
							Begin
							Insert Into #t values(convert(@groupby, @date , 120))
							Set @date=DateAdd(@dateadd,1,@date)
							End;

							select 
									X.ddd,productqty , RefundPoint ,refundcashpay ,refundBankPay,
                                    refundtransferpointpay
							from
								#t as X								
								left join
								(
									select
										convert(@groupby, refundtime, 120) as ddd,
										count(RMA_Refund_Item.SysNo) as productqty               									
									from
										RMA_Refund (nolock)
                                        inner  join RMA_Refund_Item (nolock) on RMA_Refund.SysNo = RMA_Refund_Item.RefundSysNo
                                        
                                        inner  join rma_request_item (nolock) on rma_request_item.registersysno =RMA_Refund_Item.registersysno
                                        inner  join rma_request (nolock) on rma_request_item.requestsysno = rma_request.sysno
                                        
                                    where
                                         RMA_Refund.status = @RefundStatus
                                    and  RMA_Refund.RefundTime >= @DateFrom
                                    and  RMA_Refund.RefundTime <= @DateTo
                                    @IsRejectRMA
										 group by convert(@groupby, refundtime, 120)
								) as c on X.ddd = c.ddd  
                              left join 
                                 (
                                   select 
                                        convert(@groupby, refundtime, 120) as ddd,
                                        sum(case when RMA_Refund.RefundPayType = @CashPay then RMA_Refund_item.refundcash end) as  refundcashpay,
                                        sum(case when RMA_Refund.RefundPayType = @AlipayPay then RMA_Refund_item.refundcash end) as  refundBankPay,
                                        sum(case when RMA_Refund.RefundPayType = @TransferPointPayType then RMA_Refund_item.refundcash end) as  refundtransferpointpay,
                                        sum(RMA_Refund_Item.RefundPoint) as  RefundPoint
                                   	from
										RMA_Refund  (nolock)
                                        inner  join RMA_Refund_Item (nolock) on RMA_Refund.SysNo = RMA_Refund_Item.RefundSysNo                                    
                                        inner  join rma_request_item (nolock) on rma_request_item.registersysno =RMA_Refund_Item.registersysno
                                        inner  join rma_request (nolock) on rma_request_item.requestsysno = rma_request.sysno
                                                                            
                                    where
                                       RMA_Refund.status = @RefundStatus
                                    
                                    and  RMA_Refund.RefundTime >= @DateFrom
                                    and  RMA_Refund.RefundTime <= @DateTo 
                                    @IsRejectRMA
										 group by convert(@groupby, refundtime, 120)
                                 ) as d on d.ddd=X.ddd   

							order by X.ddd;
						
							drop table #t";
            // RMA_Refund.CashAmt
            // sum(RMA_Refund.PointAmt)

            sql = sql.Replace("@RefundStatus", ((int)AppEnum.RMARefundStatus.Refunded).ToString());
            sql = sql.Replace("@DateFrom", Util.ToSqlString(paramHash["DateFrom"].ToString()));
            sql = sql.Replace("@DateTo", Util.ToSqlEndDate(paramHash["DateTo"].ToString()));
            sql = sql.Replace("@CashPay", ((int)(AppEnum.RMARefundPayType.CashRefund)).ToString());
            sql = sql.Replace("@AlipayPay", ((int)(AppEnum.RMARefundPayType.AlipayRefund)).ToString());
            sql = sql.Replace("@TransferPointPayType", ((int)(AppEnum.RMARefundPayType.TransferPointRefund)).ToString());
            string groupby = paramHash["GroupBy"] as string;
            if (groupby == "Day")
            {
                sql = sql.Replace("@groupby", "nvarchar(10)");
                sql = sql.Replace("@dateadd", "d");
            }
            else
            {
                sql = sql.Replace("@groupby", "nvarchar(7)");
                sql = sql.Replace("@dateadd", "m");
            }
            if (paramHash.ContainsKey("IsRejectRMA"))
                sql = sql.Replace("@IsRejectRMA", "and RMA_Request.IsRejectRMA =" + paramHash["IsRejectRMA"].ToString());
            else
                sql = sql.Replace("@IsRejectRMA", "");

            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetRMAReportByPM(Hashtable paramHash)
        {
            string sql = @"select  Sys_User.UserName , productqty , refundcash ,refundpoint 
                           from
                              (
									select  Product.PMUserSysNo , count(rma_refund_item.sysno) as productqty , 
											sum(rma_refund_item.refundcash) as refundcash ,
											sum(rma_refund_item.refundpoint) as refundpoint 
									from   RMA_Refund (nolock)
									inner  join RMA_Refund_Item (nolock) on RMA_Refund_Item.RefundSysNo = RMA_Refund.SysNo
									inner  join RMA_Register (nolock) on RMA_Refund_Item.RegisterSysNo = RMA_Register.SysNo
									inner  join Product (nolock) on Product.SysNo = RMA_Register.ProductSysNo									
									where  RMA_Refund.Status = @RefundStatus 
									and    RMA_Refund.RefundTime >= @DateFrom
									and    RMA_Refund.RefundTime <= @DateTo
									group  by  Product.PMUserSysNo 
                              ) as a
							  inner  join Sys_User (nolock) on a.PMUserSysNo = Sys_User.SysNo
                              order by productqty";


            sql = sql.Replace("@RefundStatus", ((int)AppEnum.RMARefundStatus.Refunded).ToString());
            sql = sql.Replace("@DateFrom", Util.ToSqlString(paramHash["DateFrom"].ToString()));
            sql = sql.Replace("@DateTo", Util.ToSqlEndDate(paramHash["DateTo"].ToString()));

            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetRMAReportByProduct(Hashtable paramHash)
        {
            string sql = @"select  Product.productname ,Product.ProductID, productqty ,refundcash ,refundpoint
                           from
                           (
                               select RMA_Register.ProductSysNo , count(RMA_Register.sysno) as productqty ,
                                      sum(RMA_Refund_Item.RefundCash) as refundcash , sum(RMA_Refund_Item.RefundPoint) as refundpoint                                       
                               from   RMA_Refund (nolock)
                               inner  join RMA_Refund_Item (nolock) on RMA_Refund.SysNo = RMA_Refund_Item.RefundSysNo
                               inner  join RMA_Register (nolock) on RMA_Refund_Item.registerSysNo = RMA_Register.SysNo
                               
                               inner join rma_request_item (nolock) on rma_request_item.registersysno = rma_register.sysno
                               inner join rma_request (nolock) on rma_request_item.requestsysno = rma_request.sysno

                               where  RMA_Refund.status = @RefundStatus
                               and    RMA_Refund.RefundTime >= @DateFrom
                               and    RMA_Refund.RefundTime <= @DateTo
                               @IsRejectRMA
                               group  by RMA_Register.ProductSysNo                                      
                           ) as a
                           inner join Product (nolock) on a.ProductSysNo = Product.SysNo
                           inner join Category3 (nolock) on Product.C3SysNo = Category3.SysNo
                           inner join Category2 (nolock) on Category3.C2SysNo = Category2.SysNo
                           inner join Category1 (nolock) on Category2.C1SysNo = Category1.SysNo
                           where 1=1 @ProductSysNo  @Category @PM  order by productqty
                           ";

            sql = sql.Replace("@RefundStatus", ((int)AppEnum.RMARefundStatus.Refunded).ToString());
            sql = sql.Replace("@DateFrom", Util.ToSqlString(paramHash["DateFrom"].ToString()));
            sql = sql.Replace("@DateTo", Util.ToSqlEndDate(paramHash["DateTo"].ToString()));

            if (paramHash.ContainsKey("ProductSysNo"))
                sql = sql.Replace("@ProductSysNo", " and a.ProductSysNo = " + paramHash["ProductSysNo"].ToString());
            else
                sql = sql.Replace("@ProductSysNo", "");

            if (paramHash.ContainsKey("Category"))
                sql = sql.Replace("@Category", " and " + paramHash["Category"].ToString());
            else
                sql = sql.Replace("@Category", "");

            if (paramHash.ContainsKey("PM"))
                sql = sql.Replace("@PM", " and Product.PMUserSysNo = " + paramHash["PM"].ToString());
            else
                sql = sql.Replace("@PM", "");
            if (paramHash.ContainsKey("IsRejectRMA"))
                sql = sql.Replace("@IsRejectRMA", "and RMA_Request.IsRejectRMA =" + paramHash["IsRejectRMA"].ToString());
            else
                sql = sql.Replace("@IsRejectRMA", "");


            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetRMAReportByManufacturer(Hashtable paramHash)
        {
            string sql = @"select Manufacturer.BriefName as manufacturername , productqty , refundcash , refundpoint
                           from
                           (
                               select Product.ManufacturerSysNo as msysno , count(RMA_Register.SysNo) as productqty ,
                                      sum(RMA_Refund_Item.RefundCash) as refundcash , sum(RMA_Refund_Item.RefundPoint) as refundpoint                                     
                               from   RMA_Refund (nolock)
                               inner  join  RMA_Refund_Item (nolock) on RMA_Refund.SysNo = RMA_Refund_Item.RefundSysNo
                               inner  join  RMA_Register (nolock) on RMA_Refund_Item.registerSysNo = RMA_Register.SysNo
                               inner  join  Product (nolock) on RMA_Register.ProductSysNo = Product.SysNo
                               inner  join  Category3 (nolock) on Category3.SysNo = Product.C3SysNo
                               inner  join  Category2 (nolock) on Category2.SysNo = Category3.C2SysNo
                               inner  join  Category1 (nolock) on Category1.SysNo = Category2.C1SysNo
                               where  RMA_Refund.status = @RefundStatus
                               and    RMA_Refund.RefundTime >= @DateFrom
                               and    RMA_Refund.RefundTime <= @DateTo
                                      @PM  @Category
                               group  by  Product.ManufacturerSysNo
                           )  as a
                           inner join Manufacturer (nolock) on a.msysno = Manufacturer.SysNo ";

            sql = sql.Replace("@RefundStatus", ((int)AppEnum.RMARefundStatus.Refunded).ToString());
            sql = sql.Replace("@DateFrom", Util.ToSqlString(paramHash["DateFrom"].ToString()));
            sql = sql.Replace("@DateTo", Util.ToSqlEndDate(paramHash["DateTo"].ToString()));

            if (paramHash.ContainsKey("PM"))
                sql = sql.Replace("@PM", " and Product.PMUserSysNo = " + paramHash["PM"].ToString());
            else
                sql = sql.Replace("@PM", "");

            if (paramHash.ContainsKey("Category"))
                sql = sql.Replace("@Category", " and " + paramHash["Category"].ToString());
            else
                sql = sql.Replace("@Category", "");

            return SqlHelper.ExecuteDataSet(sql);

        }

        public DataSet GetEqifaList(Hashtable paramHash)
        {
            string sql = @"select el.sosysno,sm.soamt,sm.status,el.eqifalog
						   from Eqifa_Log el 
                           left join so_master sm on sm.sysno = el.sosysno
                           left join Area  on sm.receiveareasysno = area.sysno
						   where  1=1";
            if (paramHash.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string key in paramHash.Keys)
                {
                    object item = paramHash[key];
                    sb.Append(" and ");
                   
                    if (key == "SOStatus")
                        sb.Append("sm.status").Append("=").Append(item.ToString());
                    else if (key == "StartDate")
                    {
                        sb.Append("sm.orderdate").Append(">=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "EndDate")
                    {
                        sb.Append("sm.orderdate").Append("<=").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "SoamtFrom")
                    {
                        sb.Append("sm.soamt").Append(">=").Append(item.ToString());
                    }
                    else if (key == "SoamtTo")
                    {
                        sb.Append("sm.soamt").Append("<=").Append(item.ToString());
                    }
                    else if (key == "DistrictSysNo")
                    {
                        sb.Append("area.sysno").Append("=").Append(item.ToString());
                    }
                    else if (key == "CitySysNo")
                    {
                        sb.Append("area.CitySysNo").Append("=").Append(item.ToString());
                    }
                    else if (key == "ProvinceSysNo")
                    {
                        sb.Append("area.ProvinceSysNo").Append("=").Append(item.ToString());
                    }
                    else if (key == "LS")
                    {
                        sb.Append("el.eqifalog like '%").Append(item.ToString()).Append("%'");
                    }
                   
             
                }
                sql += sb.ToString();
            }
            else
                sql.Replace("select", "select top 50");
            sql += " order by sm.sysno desc";
            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetRMAUserCloseRateList(int type,Hashtable paramHash)
        {
            string sql = @"select @datediff as daycount, count(a.sysno) as itemcount
                          from rma_register a
                         inner join rma_request_item b on a.sysno=b.registersysno
                         inner join rma_request c on  b.requestsysno= c.sysno 
                         inner join so_master sm(nolock) on sm.sysno=c.sosysno 
                         @inneritem
                         @innermaster
                         where c.recvtime is not null and c.status<>-1  and  @dealtime ";

            if (paramHash.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string key in paramHash.Keys)
                {
                    object item = paramHash[key];
                    sb.Append(" and ");
                    if (key == "ReceiveStartDate")
                        sb.Append("c.recvtime").Append(">=").Append(Util.ToSqlString(item.ToString()));
                    else if (key == "ReceiveEndDate")
                    {
                        sb.Append("c.recvtime").Append("<=").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    if (type == 1)//退款
                    {
                        if (key == "HandleDay")
                        {
                            sb.Append("datediff(day,c.recvtime,e.refundtime)+1").Append(item.ToString());
                        }
                    }
                    else if (type == 2)//发货
                    {
                        if (key == "HandleDay")
                        {
                            sb.Append(" datediff(day,c.recvtime,e.outtime)+1").Append(item.ToString());
                        }
                    }
                    if (key == "UsedDay")
                    {
                        sb.Append("DateDiff(day,isnull(sm.AuditDeliveryDate,sm.OutTime),isnull(c.CustomerSendTime,c.createtime))").Append(item.ToString());
                    }
                    if (key == "RefundStartDate")
                    {
                        sb.Append("e.refundtime").Append(">=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "RefundEndDate")
                    {
                        sb.Append("e.refundtime").Append("<=").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "RevertStartDate")
                    {
                        sb.Append("e.outtime").Append(">=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "RevertEndDate")
                    {
                        sb.Append("e.outtime").Append("<=").Append(Util.ToSqlEndDate(item.ToString()));
                    }

                }
                sql += sb.ToString();
            }
            sql += "group by @datediff  order by @datediff ";
            if (type == 1)//退款
            {
                sql = sql.Replace("@datediff", "datediff(day,c.recvtime,e.refundtime)+1");
                sql = sql.Replace("@inneritem", "inner join rma_refund_item d on a.sysno=d.registersysno");
                sql = sql.Replace("@innermaster", "inner join rma_refund  e on d.refundsysno=e.sysno");
                sql = sql.Replace("@dealtime", "e.refundtime is not null and e.status<>-1 ");
            }
            else if (type == 2)//发货
            {
                sql = sql.Replace("@datediff", "datediff(day,c.recvtime,e.outtime)+1 ");
                sql = sql.Replace("@inneritem", " inner join RMA_Revert_Item d on a.sysno=d.registersysno");
                sql = sql.Replace("@innermaster", " inner join RMA_Revert  e on d.revertsysno=e.sysno");
                sql = sql.Replace("@dealtime", " e.OutTime is not null and e.status<>-1 ");
            }
            return SqlHelper.ExecuteDataSet(sql);

        }

       public DataSet GetRMAUserClosedList(int type,Hashtable paramHash)
        {
            string sql = @"select @datediff as daycount,a.sysno as sysno ,c.recvtime as recvtime,product.productname as productname
                         from rma_register a
                         inner join rma_request_item b on a.sysno=b.registersysno
                         inner join rma_request c on  b.requestsysno= c.sysno
                         inner join Product on a.productsysno=product.sysno
                         inner join so_master sm(nolock) on c.sosysno=sm.sysno 
                         @inneritem  @innermaster
                         where c.recvtime is not null and c.status<>-1
                         and  @dealtime";

            if (paramHash.Count>0)
            {
                StringBuilder sb=new StringBuilder();
                foreach(string key in paramHash.Keys)
                {
                    object item =paramHash[key];
                    sb.Append(" and ");
                    if (key=="ReceiveStartDate")
                        sb.Append("c.recvtime").Append(">=").Append(Util.ToSqlString(item.ToString()));
                    else if (key == "ReceiveEndDate")
                    {
                        sb.Append("c.recvtime").Append("<=").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    if (type == 1)//退款
                    {
                        if (key == "HandleDay")
                        {
                            sb.Append("datediff(day,c.recvtime,e.refundtime)+1").Append(item.ToString());
                        }
                    }
                    else if (type == 2)//发货
                    {                        
                        if (key == "HandleDay")
                        {
                            sb.Append("datediff(day,c.recvtime,e.outtime)+1").Append(item.ToString());
                        }
                    }
                    if (key == "UsedDay")
                    {
                        sb.Append("DateDiff(day,isnull(sm.AuditDeliveryDate,sm.OutTime),isnull(c.CustomerSendTime,c.createtime))").Append(item.ToString());
                    }
                    if (key =="RefundStartDate")
                    {
                        sb.Append("e.refundtime").Append(">=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key=="RefundEndDate")
                    {
                        sb.Append("e.refundtime").Append("<=").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "RevertStartDate")
                    {
                        sb.Append("e.outtime").Append(">=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "RevertEndDate")
                    {
                        sb.Append("e.outtime").Append("<=").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    
                }
                sql+=sb.ToString();
            }
            sql += "order by @datediff ";
            if (type == 1)//退款
            {
                sql = sql.Replace("@datediff", "datediff(day,c.recvtime,e.refundtime)+1");
                sql = sql.Replace("@inneritem", "inner join rma_refund_item d on a.sysno=d.registersysno");
                sql = sql.Replace("@innermaster", "inner join rma_refund  e on d.refundsysno=e.sysno");
                sql = sql.Replace("@dealtime", "e.refundtime is not null and e.status<>-1 ");
            }
            else if (type == 2)//发货
            {
                sql = sql.Replace("@datediff", "datediff(day,c.recvtime,e.outtime)+1 ");
                sql = sql.Replace("@inneritem", " inner join RMA_Revert_Item d on a.sysno=d.registersysno");
                sql = sql.Replace("@innermaster", " inner join RMA_Revert  e on d.revertsysno=e.sysno");
                sql = sql.Replace("@dealtime", " e.OutTime is not null and e.status<>-1 ");
            }
            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetInvoiceReport(Hashtable ht)
        {
            //采购：进项
            string sql1 = @"SELECT '进项' as InvoiceType, su.username, c3.c3name,SUM(pi.Quantity) as totalqty,
                            SUM(pi.Quantity * pi.OrderPrice) as totalamt
                            FROM Finance_POPay fp(nolock) INNER JOIN
                                  PO_Master po(nolock) ON fp.POSysNo = po.SysNo INNER JOIN
                                  PO_Item pi(nolock) ON po.SysNo = pi.POSysNo INNER JOIN
                                  Product p(nolock) ON pi.ProductSysNo = p.SysNo       
                                  inner join category3 c3(nolock) on p.c3sysno=c3.sysno
                                  left join sys_user su(nolock) on su.sysno=p.pmusersysno 
                            WHERE po.intime>=@datefrom and po.intime<@dateto 
                                  and po.poinvoicetype=@poinvoicetype and @pm and po.status=@postatus 
                            GROUP BY su.username,c3.c3name
                            order by su.username,c3.c3name";
            //销售：销项
            string sql2 = @"SELECT '销项' as InvoiceType, su.username,c3.c3name,sum(si.quantity) as totalqty,
                            sum(si.quantity*si.price) as totalamt 
                            from so_master sm(nolock) 
                            inner join so_item si(nolock) on sm.sysno=si.sosysno
                            inner join product p(nolock) on si.productsysno=p.sysno
                            inner join category3 c3(nolock) on p.c3sysno=c3.sysno
                            left join sys_user su(nolock) on su.sysno=p.pmusersysno 
                            where sm.status=@sostatus and sm.isvat=@vatstatus 
                                  and sm.outtime>=@datefrom and sm.outtime<@dateto 
                                  and @pm
                            GROUP BY su.username,c3.c3name
                            order by su.username,c3.c3name";

            sql1 = sql1.Replace("@poinvoicetype", ((int)AppEnum.POInvoiceType.ValueAddedInvoice).ToString());
            sql1 = sql1.Replace("@postatus", ((int)AppEnum.POStatus.InStock).ToString());
            sql2 = sql2.Replace("@sostatus", ((int)AppEnum.SOStatus.OutStock).ToString());
            sql2 = sql2.Replace("@vatstatus", ((int)AppEnum.YNStatus.Yes).ToString());
     
            string datefrom = ht["DateFrom"] as string;
            string dateto = ht["DateTo"] as string;
            sql1 = sql1.Replace("@datefrom", "cast(" + Util.ToSqlString(datefrom) + " as datetime)");
            sql1 = sql1.Replace("@dateto", "cast(" + Util.ToSqlEndDate(dateto) + " as datetime)");
            sql2 = sql2.Replace("@datefrom", "cast(" + Util.ToSqlString(datefrom) + " as datetime)");
            sql2 = sql2.Replace("@dateto", "cast(" + Util.ToSqlEndDate(dateto) + " as datetime)");

            if (Int32.Parse(ht["PMSysNo"].ToString()) > 0)
            {
                sql1 = sql1.Replace("@pm", " su.sysno=" + ht["PMSysNo"]);
                sql2 = sql2.Replace("@pm", " su.sysno=" + ht["PMSysNo"]);
            }
            else
            {
                sql1 = sql1.Replace("@pm", " 1=1");
                sql2 = sql2.Replace("@pm", " 1=1");
            }

            DataSet ds = SqlHelper.ExecuteDataSet(sql1 + ";" + sql2);
            if (Util.HasMoreRow(ds))
            {   
                return ds;
            }
            else
                return null;
        }

        public DataSet GetAPMWorkload(Hashtable paramHash)
        {
            string datefrom = "";
            if (paramHash.ContainsKey("DateFrom"))
                datefrom = Util.ToSqlString(paramHash["DateFrom"].ToString());

            string dateto = "";
            if (paramHash.ContainsKey("DateTo"))
                dateto = Util.ToSqlEndDate(paramHash["DateTo"].ToString());


            StringBuilder sb = new StringBuilder();

            //客户反馈 tbl0
            sb.Append(@"select b.sysno,b.username,count(a.sysno) as inquantity 
                        from FeedBack a
                        inner join sys_user b on a.updateusersysno = b.sysno 
                        where 1=1 @UpdateTimeFrom @UpdateTimeTo
                        group by b.sysno,b.username;");
            //低价举报 tbl1
            sb.Append(@"select b.sysno,b.username, count(b.sysno) as inquantity 
                        from Price_Report a
                        inner join sys_user b on a.AuditUserSysNo = b.sysno 
                        where 1=1 @AuditTimeFrom @AuditTimeTo
                        group by b.sysno,b.username;");
            //采购单低价对比 tbl2
            sb.Append(@"select b.sysno,b.username, count(b.sysno) as inquantity 
                        from Product_Price_Market a 
                        inner join sys_user b on a.CreateUserSysNo = b.sysno 
                        where 1=1 @CreateTimeFrom @CreateTimeTo
                        group by b.sysno,b.username;");
           
            //商品评论 tbl3
            sb.Append(@"select b.sysno,b.username, count(b.sysno) as inquantity 
                        from Review_Master a 
                        inner join sys_user b on a.LastEditUserSysNo = b.sysno 
                        where 1=1 @LastEditDateFrom @LastEditDateTo
                        group by b.sysno,b.username;");


            string sql = sb.ToString();
            if (datefrom.Length > 0)
            {
                sql = sql.Replace("@UpdateTimeFrom", " and a.UpdateTime >= " + datefrom);
                sql = sql.Replace("@AuditTimeFrom", " and a.AuditTime >= " + datefrom);
                sql = sql.Replace("@CreateTimeFrom", " and a.CreateTime >= " + datefrom);
                sql = sql.Replace("@LastEditDateFrom", " and a.LastEditDate >= " + datefrom);

            }
            else
            {
                sql = sql.Replace("@UpdateTimeFrom", "");
                sql = sql.Replace("@AuditTimeFrom", "");
                sql = sql.Replace("@CreateTimeFrom", "");
                sql = sql.Replace("@LastEditDateFrom", "");

            }

            if (dateto.Length > 0)
            {
                sql = sql.Replace("@UpdateTimeTo", " and a.UpdateTime <= " + dateto);
                sql = sql.Replace("@AuditTimeTo", " and a.AuditTime <= " + dateto);
                sql = sql.Replace("@CreateTimeTo", " and a.CreateTime <= " + dateto);
                sql = sql.Replace("@LastEditDateTo", " and a.LastEditDate <= " + dateto);
            }
            else
            {
                sql = sql.Replace("@UpdateTimeTo", "");
                sql = sql.Replace("@AuditTimeTo", "");
                sql = sql.Replace("@CreateTimeTo", "");
                sql = sql.Replace("@LastEditDateTo", "");
            }

            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetSOAuditWordLoad(Hashtable paramHash)
        {
            string datefrom = "";
            if (paramHash.ContainsKey("DateFrom"))
                datefrom = Util.ToSqlString(paramHash["DateFrom"].ToString());

            string dateto = "";
            if (paramHash.ContainsKey("DateTo"))
                dateto = Util.ToSqlEndDate(paramHash["DateTo"].ToString());

            StringBuilder sb = new StringBuilder();

            //订单审核 tbl0
            sb.Append(@"select b.sysno,b.username,count(a.sysno) as inquantity 
                        from so_master a
                        inner join sys_user b on a.AuditUserSysNo = b.sysno 
                        where 1=1 @AuditTimeFrom @AuditTimeTo
                        group by b.sysno,b.username;");

            string sql = sb.ToString();
            if (datefrom.Length > 0)
            {
                sql = sql.Replace("@AuditTimeFrom", " and a.AuditTime >= " + datefrom);

            }
            else
            {
                sql = sql.Replace("@AuditTimeFrom", "");

            }

            if (dateto.Length > 0)
            {
                sql = sql.Replace("@AuditTimeTo", " and a.AuditTime <= " + dateto);
            }
            else
            {
                sql = sql.Replace("@AuditTimeTo", "");
            }
            return SqlHelper.ExecuteDataSet(sql);

 
        }

        public DataSet GetRMAReportDs(Hashtable paramHash)
        {
            //datefrom dateto stock iswholesale
            string sql = @"
							select 
									a.date, a.rmaCreatenumber, b.rmaRecvenumber, 
									c.rmaOutboundOutnumber,d.rmaOutboundResponsenumber,e.rmaRevertOutnumber,f.rmaRefundnumber,g.rmaClosenumber,
                                    h.rmaChecknumber,i.rmaReverChecktnumber
							from
								(
									select 
										convert(nvarchar(10), RMA_Request.CreateTime, 120) as date, count(RMA_Request.CreateTime) as rmaCreatenumber
										from Rma_Register 
	                                    inner join rma_request_item (NOLOCK) on rma_register.sysno = rma_request_item.registersysno
	                                    inner join rma_request (NOLOCK) on rma_request.sysno = rma_request_item.requestsysno
									where
										(rma_request.createtime>=@datefrom and rma_request.createtime<=@dateto) and rma_request.status<>-1	
										group by convert(nvarchar(10), RMA_Request.CreateTime, 120)
								) as a 
								left join
								(
									select 
										convert(nvarchar(10), RMA_Request.RecvTime, 120) as date, count(RMA_Request.RecvTime) as rmaRecvenumber
										from Rma_Register 
	                                    inner join rma_request_item (NOLOCK) on rma_register.sysno = rma_request_item.registersysno
	                                    inner join rma_request (NOLOCK) on rma_request.sysno = rma_request_item.requestsysno
									where
										(rma_request.RecvTime>=@datefrom and rma_request.RecvTime<=@dateto) and rma_request.status<>-1	
										group by convert(nvarchar(10), rma_request.RecvTime, 120)
								) as b
								on a.date = b.date
								left join
								(
									select 
										convert(nvarchar(10), RMA_OutBound.OutTime, 120) as date, count(RMA_OutBound.OutTime) as rmaOutboundOutnumber
										from Rma_Register 
	                                    inner join RMA_OutBound_Item (NOLOCK) on rma_register.sysno = RMA_OutBound_Item.registersysno
	                                    inner join RMA_OutBound (NOLOCK) on RMA_OutBound.sysno = RMA_OutBound_Item.OutBoundSysNo
									where
										(RMA_OutBound.OutTime>=@datefrom and RMA_OutBound.OutTime<=@dateto) and RMA_OutBound.status<>-1	
										group by convert(nvarchar(10), RMA_OutBound.OutTime, 120)
								) as c
								on a.date = c.date
	                            left join
								(
									select 
										convert(nvarchar(10), Rma_Register.ResponseTime, 120) as date, count(Rma_Register.ResponseTime) as rmaOutboundResponsenumber
										from Rma_Register 
	                                    inner join RMA_OutBound_Item (NOLOCK) on rma_register.sysno = RMA_OutBound_Item.registersysno
	                                    inner join RMA_OutBound (NOLOCK) on RMA_OutBound.sysno = RMA_OutBound_Item.OutBoundSysNo
									where
										(Rma_Register.ResponseTime>=@datefrom and Rma_Register.ResponseTime<=@dateto) and RMA_OutBound.status<>-1	
										group by convert(nvarchar(10), Rma_Register.ResponseTime, 120)
								) as d
								on a.date = d.date
                                left join
								(
									select 
										convert(nvarchar(10), RMA_Revert.OutTime, 120) as date, count(RMA_Revert.OutTime) as rmaRevertOutnumber
										from Rma_Register 
	                                    inner join RMA_Revert_Item (NOLOCK) on rma_register.sysno = RMA_Revert_Item.registersysno
	                                    inner join RMA_Revert (NOLOCK) on RMA_Revert.sysno = RMA_Revert_Item.RevertSysNo
									where
										(RMA_Revert.OutTime>=@datefrom and RMA_Revert.OutTime<=@dateto) and RMA_Revert.status<>-1	
										group by convert(nvarchar(10), RMA_Revert.OutTime, 120)
								) as e
								on a.date = e.date
                              left join
								(
									select 
										convert(nvarchar(10), RMA_Refund.RefundTime, 120) as date, count(RMA_Refund.RefundTime) as rmaRefundnumber
										from Rma_Register 
	                                    inner join RMA_Refund_Item (NOLOCK) on rma_register.sysno = RMA_Refund_Item.registersysno
	                                    inner join RMA_Refund (NOLOCK) on RMA_Refund.sysno = RMA_Refund_Item.RefundSysNo
									where
										(RMA_Refund.RefundTime>=@datefrom and RMA_Refund.RefundTime<=@dateto) and RMA_Refund.status<>-1	
										group by convert(nvarchar(10), RMA_Refund.RefundTime, 120)
								) as f
								on a.date = f.date
                                left join
								(
									select 
										convert(nvarchar(10), Rma_Register.CloseTime, 120) as date, count(Rma_Register.CloseTime) as rmaClosenumber
										from Rma_Register 
									where
										(Rma_Register.CloseTime>=@datefrom and Rma_Register.CloseTime<=@dateto) and Status<>-1	
										group by convert(nvarchar(10), Rma_Register.CloseTime, 120)
								) as g
								on a.date = g.date
                                left join
								(
									select 
										convert(nvarchar(10), Rma_Register.CheckTime, 120) as date, count(Rma_Register.CheckTime) as rmaChecknumber
										from Rma_Register 
									where
										(Rma_Register.CheckTime>=@datefrom and Rma_Register.CheckTime<=@dateto) and Status<>-1	
										group by convert(nvarchar(10), Rma_Register.CheckTime, 120)
								) as h
                                on a.date = h.date
                                 left join
								(
									select 
										convert(nvarchar(10), RMA_Revert.CheckQtyTime, 120) as date, count(RMA_Revert.CheckQtyTime) as rmaReverChecktnumber
										from Rma_Register 
	                                    inner join RMA_Revert_Item (NOLOCK) on rma_register.sysno = RMA_Revert_Item.registersysno
	                                    inner join RMA_Revert (NOLOCK) on RMA_Revert.sysno = RMA_Revert_Item.RevertSysNo
									where
										(RMA_Revert.CheckQtyTime>=@datefrom and RMA_Revert.CheckQtyTime<=@dateto) and RMA_Revert.status<>-1	
										group by convert(nvarchar(10), RMA_Revert.CheckQtyTime, 120)
								) as i
								on a.date = i.date
							
							order by a.date";

            string datefrom = paramHash["DateFrom"] as string;
            string dateto = paramHash["DateTo"] as string;

            string Dateto15 = DateTime.Now.AddDays(-15).ToString(AppConst.DateFormat);
            string Dateto30 = DateTime.Now.AddDays(-30).ToString(AppConst.DateFormat);

            sql = sql.Replace("@datefrom", "cast(" + Util.ToSqlString(datefrom) + " as datetime)");
            sql = sql.Replace("@dateto", "cast(" + Util.ToSqlEndDate(dateto) + " as datetime)");
            sql=sql.Replace("@OutBoundStatus",((int)AppEnum.RMAOutBoundStatus.SendAlready).ToString());

            sql = sql.Replace("@Dateto15", Dateto15);

            return SqlHelper.ExecuteDataSet(sql);

        }


        public DataSet GetSaleReportByCategoryDs(Hashtable paramHash)
        {

            string sql = @" SELECT @date as date, Category1.c1name as c1name,@c2name,@c3name, SUM(SO_Item.Quantity) AS itemQuantity,SUM((SO_Item.Price-SO_Item.DiscountAmt)*SO_Item.Quantity) AS saleAmt,
                                sum(so_item.cost*so_item.quantity) as cost,sum((so_item.cost+ so_item.point/10.0)*so_item.quantity) as cost_p,sum((SO_Item.Price-SO_Item.DiscountAmt)*SO_Item.Quantity+PointPay/10.0) as saleamt_p, 
                                sum((SO_Item.Price-SO_Item.DiscountAmt-cost)*SO_Item.Quantity) as profit,sum((SO_Item.Price-SO_Item.DiscountAmt-cost-so_item.point/10.0)*SO_Item.Quantity) as profit_P
                                FROM SO_Item
                                    INNER JOIN SO_Master ON SO_Master.SysNo = SO_Item.SOSysNo
                                    INNER JOIN Product ON Product.SysNo = SO_Item.ProductSysNo
                                    inner join Manufacturer on Product.ManufacturerSysNo= Manufacturer.sysno                                  
                                    inner join Category3 on Product.c3sysno=Category3.sysno
                                    inner join Category2 on Category2.sysno=Category3.c2sysno
                                    inner join Category1 on Category1.sysno=Category2.c1sysno
                                    WHERE SO_Master.Status = @Status @Category @DateFrom @DateTo @iswholesale @StockSysNo @PMSysNo
                                    GROUP BY @groupby, @MasterSearch
                                    Order by @dd, @Orderby";


            string datefrom = paramHash["DateFrom"] as string;
            string dateto = paramHash["DateTo"] as string;
            string groupby = paramHash["GroupBy"] as string;
            if (groupby == "Day")
            {
                sql = sql.Replace("@groupby", "convert(nvarchar(10), outtime, 120)");
                sql = sql.Replace("@date", "convert(nvarchar(10), outtime, 120)");
                sql = sql.Replace("@dd", "convert(nvarchar(10), outtime, 120)");
            }
            else if (groupby == "Month")
            {
                sql = sql.Replace("@groupby", "convert(nvarchar(7), outtime, 120)");
                sql = sql.Replace("@date", "convert(nvarchar(7), outtime, 120)");
                sql = sql.Replace("@dd", "convert(nvarchar(7), outtime, 120)");
            }
            else
            {
                sql = sql.Replace("@groupby,", " ");
                sql = sql.Replace("@date", "null");
                sql = sql.Replace("@dd,", " ");
            }
            
            int status = (int)AppEnum.SOStatus.OutStock;
            sql = sql.Replace("@DateFrom", " and outtime>=cast(" + Util.ToSqlString(datefrom) + " as datetime)");
            sql = sql.Replace("@DateTo", " and outtime<=cast(" + Util.ToSqlEndDate(dateto) + " as datetime)");
            sql = sql.Replace("@Status", status.ToString());
            if (paramHash.ContainsKey("IsWholeSale"))
            {
                int iswholesale = (int)paramHash["IsWholeSale"];
                sql = sql.Replace("@iswholesale", " and iswholesale = " + iswholesale.ToString());
            }
            else
            {
                sql = sql.Replace("@iswholesale", "");
            }
            if (paramHash.ContainsKey("StockSysNo"))
            {
                int stocksysno = (int)paramHash["StockSysNo"];
                sql = sql.Replace("@StockSysNo", " and stocksysno = " + stocksysno.ToString());
            }
            else
            {
                sql = sql.Replace("@StockSysNo", "");
            }
            if (paramHash.ContainsKey("Category"))
            {

                sql = sql.Replace("@Category", " and " + paramHash["Category"].ToString());
            }
            else
            {
                sql = sql.Replace("@Category", "");
            }
            if (paramHash.ContainsKey("MasterSearch"))
            {
                sql = sql.Replace("@MasterSearch", paramHash["MasterSearch"].ToString());

            }
            else
            {
                sql = sql.Replace("@MasterSearch", "");
            }
            if (paramHash.ContainsKey("c2name"))
            {
                sql = sql.Replace("@c2name", paramHash["c2name"].ToString());

            }
            else
            {
                sql = sql.Replace("@c2name", "");
            }

            if (paramHash.ContainsKey("c3name"))
            {
                sql = sql.Replace("@c3name", paramHash["c3name"].ToString());
            }
            else
            {
                sql = sql.Replace("@c3name", "");
            }
            if (paramHash.ContainsKey("Orderby"))
            {
                sql = sql.Replace("@Orderby", paramHash["Orderby"].ToString()+" desc");

            }
            else
            {
                sql = sql.Replace("@Orderby", "");
            }
            if (paramHash.ContainsKey("PMSysNo"))
            {
                sql = sql.Replace("@PMSysNo", "and product.PMUserSysNo=" + paramHash["PMSysNo"].ToString());
            }
            else
            {
                sql = sql.Replace("@PMSysNo", "");
            }
            return SqlHelper.ExecuteDataSet(sql);

        }

        public DataSet GetSaleReportByManufacturerDs(Hashtable paramHash)
        {

            string sql = @"  SELECT @date as date, Manufacturer.sysno,Manufacturer.Briefname, SUM(SO_Item.Quantity) AS itemQuantity,SUM((SO_Item.Price-SO_Item.DiscountAmt)*SO_Item.Quantity) AS saleAmt,
                                sum(so_item.cost*so_item.quantity) as cost,sum((so_item.cost+ so_item.point/10.0)*so_item.quantity) as cost_p,sum((SO_Item.Price-SO_Item.DiscountAmt)*SO_Item.Quantity+PointPay/10.0) as saleamt_p,
                                sum((SO_Item.Price-SO_Item.DiscountAmt-cost)*SO_Item.Quantity) as profit,sum((SO_Item.Price-SO_Item.DiscountAmt-cost-so_item.point/10.0)*SO_Item.Quantity) as profit_P
                                    FROM SO_Item
                                    INNER JOIN SO_Master ON SO_Master.SysNo = SO_Item.SOSysNo
                                    INNER JOIN Product ON Product.SysNo = SO_Item.ProductSysNo
                                    inner join Manufacturer on Product.ManufacturerSysNo= Manufacturer.sysno  
                                    inner join Category3 on Product.c3sysno=Category3.sysno
                                    inner join Category2 on Category2.sysno=Category3.c2sysno
                                    inner join Category1 on Category1.sysno=Category2.c1sysno
                                    WHERE SO_Master.Status = @Status @DateFrom @DateTo @iswholesale @StockSysNo @ManufacturerSysNo @Category @PMSysNo
                                    GROUP BY @groupby, @MasterSearch
                                    Order by @dd, @Orderby";



            string datefrom = paramHash["DateFrom"] as string;
            string dateto = paramHash["DateTo"] as string;
            int status = (int)AppEnum.SOStatus.OutStock;
            string groupby = paramHash["GroupBy"] as string;
            if (groupby == "Day")
            {
                sql = sql.Replace("@groupby", "convert(nvarchar(10), outtime, 120)");
                sql = sql.Replace("@date", "convert(nvarchar(10), outtime, 120)");
                sql = sql.Replace("@dd", "convert(nvarchar(10), outtime, 120)");
            }
            else if (groupby == "Month")
            {
                sql = sql.Replace("@groupby", "convert(nvarchar(7), outtime, 120)");
                sql = sql.Replace("@date", "convert(nvarchar(7), outtime, 120)");
                sql = sql.Replace("@dd", "convert(nvarchar(7), outtime, 120)");
            }
            else
            {
                sql = sql.Replace("@groupby,", " ");
                sql = sql.Replace("@date", "null");
                sql = sql.Replace("@dd,", " ");
            }
            sql = sql.Replace("@DateFrom", " and outtime>=cast(" + Util.ToSqlString(datefrom) + " as datetime)");
            sql = sql.Replace("@DateTo", " and outtime<=cast(" + Util.ToSqlEndDate(dateto) + " as datetime)");
            sql = sql.Replace("@Status", status.ToString());
            if (paramHash.ContainsKey("IsWholeSale"))
            {
                int iswholesale = (int)paramHash["IsWholeSale"];
                sql = sql.Replace("@iswholesale", " and iswholesale = " + iswholesale.ToString());
            }
            else
            {
                sql = sql.Replace("@iswholesale", "");
            }
            if (paramHash.ContainsKey("StockSysNo"))
            {
                int stocksysno = (int)paramHash["StockSysNo"];
                sql = sql.Replace("@StockSysNo", " and stocksysno = " + stocksysno.ToString());
            }
            else
            {
                sql = sql.Replace("@StockSysNo", "");
            }
            if (paramHash.ContainsKey("ManufacturerSysNo"))
            {

                sql = sql.Replace("@ManufacturerSysNo", " and Manufacturer.sysno =" + paramHash["ManufacturerSysNo"].ToString());
            }
            else
            {
                sql = sql.Replace("@ManufacturerSysNo", "");
            }
            if (paramHash.ContainsKey("MasterSearch"))
            {
                sql = sql.Replace("@MasterSearch", paramHash["MasterSearch"].ToString());

            }
            else
            {
                sql = sql.Replace("@MasterSearch", "");
            }
            if (paramHash.ContainsKey("Orderby"))
            {
                sql = sql.Replace("@Orderby", paramHash["Orderby"].ToString()+" desc");

            }
            else
            {
                sql = sql.Replace("@Orderby", "");
            }
            if (paramHash.ContainsKey("Category"))
            {

                sql = sql.Replace("@Category", " and " + paramHash["Category"].ToString());
            }
            else
            {
                sql = sql.Replace("@Category", "");
            }
            if (paramHash.ContainsKey("PMSysNo"))
            {
                sql = sql.Replace("@PMSysNo", "and product.PMUserSysNo=" + paramHash["PMSysNo"].ToString());
            }
            else
            {
                sql = sql.Replace("@PMSysNo", "");
            }
            return SqlHelper.ExecuteDataSet(sql);

        }
    }
}