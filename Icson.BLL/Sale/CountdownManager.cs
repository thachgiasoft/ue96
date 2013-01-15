using System;
using System.Collections;
using System.Data;
using System.Text;
using Icson.DBAccess;
using Icson.DBAccess.Sale;
using Icson.Objects.Sale;
using System.Transactions;
using Icson.Utils;
using Icson.Objects;
using Icson.Objects.Basic;
using Icson.BLL.Basic;

//using Icson.Objects.Basic;
//using Icson.Objects.Sale;
//using Icson.DBAccess;
//using Icson.DBAccess.Sale;
//using Icson.BLL;
//using Icson.BLL.Basic;

namespace Icson.BLL.Sale
{
    /// <summary>
    /// Summary description for CountdownManager.
    /// </summary>
    public class CountdownManager
    {
        private CountdownManager()
        {
        }
        private static CountdownManager _instance;
        public static CountdownManager GetInstance()
        {
            if (_instance == null)
                _instance = new CountdownManager();
            return _instance;
        }

        public void Insert(CountdownInfo oParam)
        {
            //有效的记录，相同商品，时段不能重复
            //对于一维集合A(x,y), B(a,b), 已知x>y, a>b。判断AB是否重合。
            // AB不重合可以这样判断 (y<a || x>b), 这个否关系的表达式 ( y>=a && x<=b).
            // x=sale_countdown.StartTime
            // y=sale_countdown.EndTime
            // a=oParam.StartTime
            // b=oParam.EndTime
            string sql = "select top 1 sysno from sale_countdown where "
                + " status in (" + (int)AppEnum.CountdownStatus.Ready + "," + (int)AppEnum.CountdownStatus.Running + ")"
                + " and productsysno = " + oParam.ProductSysNo
                + " and (EndTime>=" + Util.ToSqlString(oParam.StartTime.ToString(AppConst.DateFormatLong))
                + " and StartTime<=" + Util.ToSqlString(oParam.EndTime.ToString(AppConst.DateFormatLong)) + ")";

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                throw new BizException("有效的记录，相同商品，时段不能重复");

            new CountdownDac().Insert(oParam);
        }
        public void Update(CountdownInfo oParam)
        {
            new CountdownDac().Update(oParam);
        }

        private int getCurrentStatus(int sysno)
        {
            string sql = "select status from sale_countdown where sysno=" + sysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                throw new BizException("no records use this sysno");
            return Util.TrimIntNull(ds.Tables[0].Rows[0]["Status"]);
        }
        private void map(CountdownInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.CreateUserSysNo = Util.TrimIntNull(tempdr["CreateUserSysNo"]);
            oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
            oParam.ProductSysNo = Util.TrimIntNull(tempdr["ProductSysNo"]);
            oParam.StartTime = Util.TrimDateNull(tempdr["StartTime"]);
            oParam.EndTime = Util.TrimDateNull(tempdr["EndTime"]);
            oParam.CountDownCurrentPrice = Util.TrimDecimalNull(tempdr["CountDownCurrentPrice"]);
            oParam.CountDownCashRebate = Util.TrimDecimalNull(tempdr["CountDownCashRebate"]);
            oParam.CountDownPoint = Util.TrimIntNull(tempdr["CountDownPoint"]);
            oParam.CountDownQty = Util.TrimIntNull(tempdr["CountDownQty"]);
            oParam.SnapShotCurrentPrice = Util.TrimDecimalNull(tempdr["SnapShotCurrentPrice"]);
            oParam.SnapShotCashRebate = Util.TrimDecimalNull(tempdr["SnapShotCashRebate"]);
            oParam.SnapShotPoint = Util.TrimIntNull(tempdr["SnapShotPoint"]);
            oParam.AffectedVirtualQty = Util.TrimIntNull(tempdr["AffectedVirtualQty"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
            oParam.Type = Util.TrimIntNull(tempdr["Type"]);
        }
        public CountdownInfo Load(int sysno)
        {
            string sql = "select * from sale_countdown where sysno = " + sysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                throw new BizException("no records use this sysno");
            CountdownInfo oInfo = new CountdownInfo();
            map(oInfo, ds.Tables[0].Rows[0]);
            return oInfo;
        }

        public void SetAbandon(int sysno)
        {
            //必须是Ready状态
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                int status = getCurrentStatus(sysno);
                if (status != (int)AppEnum.CountdownStatus.Ready)
                    throw new BizException("the current status not allow such opertion");

                Hashtable ht = new Hashtable(5);
                ht.Add("SysNo", sysno);
                ht.Add("Status", (int)AppEnum.CountdownStatus.Abandon);

                new CountdownDac().Update(ht);

                scope.Complete();
            }
        }

        public bool SetRunning(int sysno)
        {
            //必须是Ready状态，切换价格和库存
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                CountdownInfo oCountdown = Load(sysno);

                if (oCountdown.Status != (int)AppEnum.CountdownStatus.Ready)
                    throw new BizException("the current status not allow such opertion");

                oCountdown.Status = (int)AppEnum.CountdownStatus.Running;

                ProductPriceInfo oPrice = ProductManager.GetInstance().LoadPrice(oCountdown.ProductSysNo);

                oCountdown.SnapShotCurrentPrice = oPrice.CurrentPrice;
                oCountdown.SnapShotCashRebate = oPrice.CashRebate;
                oCountdown.SnapShotPoint = oPrice.Point;

                oPrice.CurrentPrice = oCountdown.CountDownCurrentPrice;
                oPrice.CashRebate = oCountdown.CountDownCashRebate;
                oPrice.Point = oCountdown.CountDownPoint;

                InventoryInfo oInventory = InventoryManager.GetInstance().LoadInventory(oCountdown.ProductSysNo);
                
                //oCountdown.AffectedVirtualQty = oInventory.AvailableQty + oInventory.VirtualQty + oInventory.ConsignQty - oCountdown.CountDownQty;
                oCountdown.AffectedVirtualQty = oInventory.AvailableQty + oInventory.VirtualQty + - oCountdown.CountDownQty;
                if (oCountdown.AffectedVirtualQty < 0)
                {
                    scope.Complete();
                    return false;
                }

                InventoryManager.GetInstance().SetVirtualQty(oCountdown.ProductSysNo, -1 * oCountdown.AffectedVirtualQty);
                ProductManager.GetInstance().UpdatePriceInfo(oPrice);
                new CountdownDac().Update(oCountdown);

                scope.Complete();
                return true;
            }
        }
        public void SetInterupt(int sysno)
        {
            //必须是Running，切换价格和库存
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                CountdownInfo oCountdown = Load(sysno);

                if (oCountdown.Status != (int)AppEnum.CountdownStatus.Running)
                    throw new BizException("the current status not allow such opertion");

                oCountdown.Status = (int)AppEnum.CountdownStatus.Interupt;


                ProductPriceInfo oPrice = ProductManager.GetInstance().LoadPrice(oCountdown.ProductSysNo);

                oPrice.CurrentPrice = oCountdown.SnapShotCurrentPrice;
                oPrice.CashRebate = oCountdown.SnapShotCashRebate;
                oPrice.Point = oCountdown.SnapShotPoint;

                InventoryManager.GetInstance().SetVirtualQty(oCountdown.ProductSysNo, oCountdown.AffectedVirtualQty);
                ProductManager.GetInstance().UpdatePriceInfo(oPrice);
                new CountdownDac().Update(oCountdown);

                scope.Complete();
            }
        }
        public void SetFinish(int sysno)
        {
            //必须是Running
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                CountdownInfo oCountdown = Load(sysno);

                if (oCountdown.Status != (int)AppEnum.CountdownStatus.Running)
                    throw new BizException("the current status not allow such opertion");

                oCountdown.Status = (int)AppEnum.CountdownStatus.Finish;

                ProductPriceInfo oPrice = ProductManager.GetInstance().LoadPrice(oCountdown.ProductSysNo);

                oPrice.CurrentPrice = oCountdown.SnapShotCurrentPrice;
                oPrice.CashRebate = oCountdown.SnapShotCashRebate;
                oPrice.Point = oCountdown.SnapShotPoint;

                InventoryManager.GetInstance().SetVirtualQty(oCountdown.ProductSysNo, oCountdown.AffectedVirtualQty);
                ProductManager.GetInstance().UpdatePriceInfo(oPrice);
                new CountdownDac().Update(oCountdown);

                scope.Complete();
            }
        }
        public DataSet GetCountdownDs(Hashtable paramHt)
        {
            string sql = @"select 
								a.*, productid, productname, username as createname 
							from 
								sale_countdown a
								inner join sys_user on a.createusersysno = sys_user.sysno
								inner join product on a.productsysno = product.sysno
							where 1=1";

            if (paramHt.ContainsKey("ProductSysNo"))
                sql = sql + " and a.productsysno= " + ((int)paramHt["ProductSysNo"]).ToString();
            if (paramHt.ContainsKey("DateFromCreate"))
                sql = sql + " and a.createtime>=" + Util.ToSqlString(paramHt["DateFromCreate"].ToString());
            if (paramHt.ContainsKey("DateToCreate"))
                sql = sql + " and a.createtime<=" + Util.ToSqlEndDate(paramHt["DateToCreate"].ToString());
            if (paramHt.ContainsKey("Status"))
                sql = sql + " and a.Status =" + ((int)paramHt["Status"]).ToString();
            if (paramHt.ContainsKey("Type"))
                sql = sql + " and a.Type =" + ((int)paramHt["Type"]).ToString();
            if (paramHt.Contains("pmusersysno"))
                sql = sql + " and product.pmusersysno=" + ((int)paramHt["pmusersysno"]).ToString();

            string dateFromCountdown = "1753-1-1"; //sql2000 最小的日期值
            string dateToCountdown = DateTime.MaxValue.ToString(AppConst.DateFormat);

            if (paramHt.ContainsKey("DateFromCountDown"))
                dateFromCountdown = paramHt["DateFromCountDown"].ToString();
            if (paramHt.ContainsKey("DateToCountDown"))
                dateToCountdown = paramHt["DateToCountDown"].ToString();

            // b>x && y>a
            // xy 表
            // ab 条件
            sql = sql + " and ( " + Util.ToSqlEndDate(dateToCountdown) + " > a.StartTime and a.EndTime > " + Util.ToSqlString(dateFromCountdown) + ")";

            sql = sql + " order by a.sysno desc";

            return SqlHelper.ExecuteDataSet(sql);

        }
        public void CountdownJob()
        {
            try
            {
                string sql = "select * from sale_countdown where status in ( " + (int)AppEnum.CountdownStatus.Ready + "," + (int)AppEnum.CountdownStatus.Running + ")";
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (!Util.HasMoreRow(ds))
                    return;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    CountdownInfo oCountdown = new CountdownInfo();
                    map(oCountdown, dr);
                    if (oCountdown.Status == (int)AppEnum.CountdownStatus.Ready && (oCountdown.StartTime < DateTime.Now && oCountdown.EndTime >= DateTime.Now))
                    {
                        if (!this.SetRunning(oCountdown.SysNo))
                            this.SetAbandon(oCountdown.SysNo);
                    }
                    if (oCountdown.Status == (int)AppEnum.CountdownStatus.Running && (oCountdown.EndTime < DateTime.Now))
                    {
                        if (oCountdown.Type == (int)AppEnum.CountdownType.OneTime)
                        {
                            this.SetFinish(oCountdown.SysNo);
                        }
                        else if (oCountdown.Type == (int)AppEnum.CountdownType.EveryDay)
                        {
                            this.SetFinish(oCountdown.SysNo);
                            oCountdown.StartTime = oCountdown.StartTime.AddDays(1);
                            oCountdown.EndTime = oCountdown.EndTime.AddDays(1);
                            oCountdown.Status = (int)AppEnum.CountdownStatus.Ready;
                            this.Update(oCountdown);
                        }
                    }
                }
            }
            catch
            {

            }
        }

        public DataSet GetCountdownOnlineDsDefault(Hashtable paramHash, int topNumber, bool isOnlineShow, bool isQtyShow)
        {
            string sql = @" select @top
								Sale_Countdown.SysNo as OnlineSysNo, Product.SysNo as SysNo,Product.C3SysNo as C3SysNo, ProductId, ProductName, ProductDesc, 
                                Sale_Countdown.ProductSysNo, left(productdesc,150) as productdescription,
								AvailableQty+VirtualQty as OnlineQty, Product.Status, UserName as CreateUserName,
								Sale_Countdown.CreateTime, Product_Price.*,Category1.C1ID, Category2.C2ID, Category3.C3ID
							from 
                                Sale_Countdown, 								
								Product,
								Product_Price,
								Sys_User,
								Inventory,
								Category3,
								Category2,
								Category1
							where 
							Sale_Countdown.productsysno = Product.sysno 
							and Category3.sysno=Product.C3SysNo
							and Category2.sysno=Category3.C2SysNo
							and Category1.sysno=Category2.C1SysNo
							and Sale_Countdown.CreateUserSysNo = Sys_User.SysNo
							and Inventory.ProductSysNo = Sale_Countdown.ProductSysNo
							and Sale_Countdown.ProductSysNo = Product_Price.ProductSysNo
							@onlineShowLimit
							@isqtyshow
                            and sale_countdown.status = @CountdownStatus
                            ";

            if (isOnlineShow)
                sql = sql.Replace("@onlineShowLimit", Icson.BLL.Online.OnlineListManager.GetInstance().onlineShowLimit);
            else
                sql = sql.Replace("@onlineShowLimit", "");

            if (isQtyShow)
                sql = sql.Replace("@isqtyshow", " and Inventory.AvailableQty+Inventory.VirtualQty>0 ");
            else
                sql = sql.Replace("@isqtyshow", "");

            string sqlOrderBy = "";


            if (paramHash != null && paramHash.Count != 0)
            {
                StringBuilder sb = new StringBuilder(100);
                foreach (string key in paramHash.Keys)
                {
                    object item = paramHash[key];
                    if (key == "OrderBy")
                    {
                        sqlOrderBy = " order by " + item.ToString();
                    }
                    else
                    {
                        sb.Append(" and ");
                        if (key == "ProductSysNo")
                        {
                            sb.Append("sale_countdown.ProductSysNo = ").Append(item.ToString());
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
                }
                sql += sb.ToString();
            }
            sql += sqlOrderBy;

            if (topNumber != AppConst.IntNull)
                sql = sql.Replace("@top", " top " + topNumber);
            else
                sql = sql.Replace("@top", "");

            sql = sql.Replace("@CountdownStatus", ((int)AppEnum.CountdownStatus.Running).ToString());

            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetCountdownOnlineDs()
        {
            string sql = @"select 
							sale_countdown.*, inventory.*,
							productid, productname
						from
							sale_countdown
							inner join product on sale_countdown.productsysno = product.sysno and sale_countdown.status = 1
							inner join inventory on sale_countdown.productsysno = inventory.productsysno
							inner join product_price on sale_countdown.productsysno = product_price.productsysno
						where
							sale_countdown.status = @CountdownStatus
							@OnlineShowLimit
						";
            sql = sql.Replace("@CountdownStatus", ((int)AppEnum.CountdownStatus.Running).ToString());
            sql = sql.Replace("@OnlineShowLimit", Icson.BLL.Online.OnlineListManager.GetInstance().onlineShowLimit);

            return SqlHelper.ExecuteDataSet(sql);
        }

        public String GetCountdownOnlineString(DataSet ds)
        {
            if (!Util.HasMoreRow(ds))
                return "";

            int index = 0;
            string seperator = @"
						<table width='100%' border='0' align='center' cellpadding='0' cellspacing='0'>
							<tr> 
								<td height='10'><img src='../images/dot_line02.gif' width='1' height='1'></td>
							</tr>
							<tr> 
								<td height='1' background='../images/dot_line02.gif'><img src='../images/dot_line02.gif' width='148' height='1'></td>
							</tr>
							<tr> 
								<td height='10'><img src='../images/dot_line02.gif' width='1' height='1'></td>
							</tr>
						</table>";


            StringBuilder sb = new StringBuilder(2000);
            sb.Append(seperator);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sb.Append(GetCountdownMold(dr, index));
                sb.Append(seperator);
                index++;
            }
            return sb.ToString();
        }

        public string GetCountdownMold(DataRow dr, int index)
        {
            StringBuilder sb = new StringBuilder(2000);

            sb.Append("<table width='100%' border='0' cellpadding='0' cellspacing='0' align=center>");
            sb.Append("	<tr class='m-1'> ");
            sb.Append("		<td width=100 valign='top'>");
            //sb.Append("			<a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + dr["ProductSysNo"].ToString() + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr["ProductID"]))).Append("' alt=\"" + System.Web.HttpUtility.HtmlEncode(Util.RemoveHtmlTag(dr["ProductName"].ToString())) + "\" width='80' height='60' border='0'></a>").Append("<br><font color='#999999'>").Append(dr["productid"].ToString()).Append("</font>").Append("</td>");
            sb.Append("			<a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + dr["ProductSysNo"].ToString() + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr["ProductID"]))).Append("' alt=\"查看大图\" width='80' height='60' border='0'></a>").Append("</td>");
            sb.Append("		<td width=40% align=left valign=middle>");
            sb.Append("			<span class='m-1'><a href='../Items/ItemDetail.aspx?ItemID=").Append(dr["ProductSysNo"].ToString()).Append("' target='_blank'>").Append(dr["productname"].ToString()).Append("</a></span>");
            sb.Append("		</td>");
            sb.Append("		<td width=150>");
            //-----snap shot price
            sb.Append(getPrice(dr, false));
            sb.Append("		</td>");
            sb.Append("		<td width=150>");
            //-----countdown price
            sb.Append(getPrice(dr, true));
            sb.Append("		</td>");
            //sb.Append("		<td>");
            //sb.Append(getStatus(dr, index));
            //-----status including time & qty
            //sb.Append("		</td>");

            sb.Append("	</tr>");
            sb.Append("</table>");

            return sb.ToString();
        }

        public string getPrice(DataRow dr, bool isCountdown)
        {
            string deleteLineClass = "middleLine";
            if (isCountdown)
                deleteLineClass = "font10";

            decimal currentPrice = Util.TrimDecimalNull(dr["SnapShotCurrentPrice"]);
            decimal cashRebate = Util.TrimDecimalNull(dr["SnapShotCashRebate"]);
            int point = Util.TrimIntNull(dr["SnapShotPoint"]);
            string currentPriceTag = "ORS商城价：";

            if (isCountdown)
            {
                currentPrice = Util.TrimDecimalNull(dr["CountdownCurrentPrice"]);
                cashRebate = Util.TrimDecimalNull(dr["CountdownCashRebate"]);
                point = Util.TrimIntNull(dr["CountdownPoint"]);
                currentPriceTag = "夜间价：";
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("<img src='../images/arr2.gif' width='10' height='10'>").Append(currentPriceTag).Append("<span class='").Append(deleteLineClass).Append("'>￥" + (currentPrice + cashRebate).ToString(AppConst.DecimalFormat) + "</span>");

            if (cashRebate > 0)
                sb.Append("<br><img src='../images/arr2.gif' width='10' height='10'>").Append("ORS商城礼券：").Append("<span class='").Append(deleteLineClass).Append("'>￥" + cashRebate.ToString(AppConst.DecimalFormat) + "</span>");

            if (point > 0)
                sb.Append("<br><img src='../images/arr2.gif' width='10' height='10'>").Append("赠送积分：").Append("<span class='").Append(deleteLineClass).Append("'>" + point.ToString() + "</span>");

            if (isCountdown)
            {
                //if (Util.TrimIntNull(dr["AvailableQty"]) + Util.TrimIntNull(dr["VirtualQty"]) + Util.TrimIntNull(dr["ConsignQty"]) > 0)
                if (Util.TrimIntNull(dr["AvailableQty"]) + Util.TrimIntNull(dr["VirtualQty"]) > 0)
                    sb.Append("						<br>&nbsp;&nbsp;&nbsp;<a href=\"javascript:AddToCart(" + dr["ProductSysNo"].ToString() + ")\"><span class='buyfont'>我要购买&nbsp;</span><img src='../Images/Items/greybubblearrow.gif'align='middle' border=0></a>");
                else
                    sb.Append("						<br>&nbsp;&nbsp;&nbsp;<a href='../Account/MyFavorite.aspx?Cmd=Add&ItemID=").Append(dr["ProductSysNo"].ToString()).Append(" '><img src='../images/notice.gif' border='0'></a>");
            }

            return sb.ToString();
        }

        public string getStatus(DataRow dr, int index)
        {
            StringBuilder sb = new StringBuilder(500);
            DateTime startTime = Util.TrimDateNull(dr["StartTime"]);
            DateTime endTime = Util.TrimDateNull(dr["EndTime"]);

            TimeSpan span = endTime - startTime;
            long tempSpan = span.Ticks / 5;
            string statusImg = "../Images/Countdown/";

            TimeSpan ts = endTime - DateTime.Now;
            int leftSecond = Convert.ToInt32(Decimal.Round(Convert.ToDecimal(ts.TotalSeconds), 0));
            if (leftSecond < 0)
                leftSecond = 0;

            int leftQty = Util.TrimIntNull(dr["AvailableQty"]) + Util.TrimIntNull(dr["VirtualQty"]);// +Util.TrimIntNull(dr["ConsignQty"]);

            if (leftQty <= 0 || leftSecond == 0)
            {
                statusImg += "CountDown_Status_4.gif";
            }
            else if (DateTime.Now < startTime.AddTicks(tempSpan))
            {
                statusImg += "CountDown_Status_1.gif";
            }
            else if (DateTime.Now >= startTime.AddTicks(tempSpan) && DateTime.Now < startTime.AddTicks(tempSpan * 4))
            {
                statusImg += "CountDown_Status_2.gif";
            }
            else if (DateTime.Now >= startTime.AddTicks(tempSpan * 4))
            {
                statusImg += "CountDown_Status_3.gif";
            }
            statusImg = "<img src=" + statusImg + ">";

            string id = "leftTime" + index.ToString();

            sb.Append("<script language=\"Javascript\">");
            sb.Append("initialTimeElement[" + index.ToString() + "] = " + leftSecond.ToString());
            sb.Append(";showTimeElement[" + index.ToString() + "] = '" + id + "'");
            sb.Append("</script>");

            sb.Append("<table bordor=0 cellSpacing=0 cellPadding=0 width=125");
            sb.Append("	<tr>");
            sb.Append("		<td colspan=2 align=center>");
            sb.Append(statusImg);
            sb.Append("		</td>");
            sb.Append("	</tr>");
            sb.Append("	<tr>");
            sb.Append("		<td align=center>时间</td>");
            sb.Append("		<td align=center>库存</td>");
            sb.Append("	</tr>");
            sb.Append("	<tr>");
            sb.Append("		<td align=center><font color=red><label id='" + id + "'></label></td></font>");
            sb.Append("		<td align=center><font color=red>" + leftQty.ToString() + "</td></font>");
            sb.Append("	</tr>");
            sb.Append("</table>");

            return sb.ToString();
        }

        public CountdownInfo GetCountdownRunningByProductSysNo(int productSysNo)
        {
            string sql = "select * from sale_countdown where productsysno = " + productSysNo + " and status=" + (int)AppEnum.CountdownStatus.Running;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;

            if (ds.Tables[0].Rows.Count != 1)
                throw new BizException("限时抢购策略重复!");

            CountdownInfo oCountdown = new CountdownInfo();
            map(oCountdown, ds.Tables[0].Rows[0]);
            return oCountdown;
        }

        public CountdownInfo GetCountdownReadyByProductSysNo(int productSysNo)
        {
            string sql = "select * from sale_countdown where productsysno = " + productSysNo + " and status=" + (int)AppEnum.CountdownStatus.Ready;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;

            if (ds.Tables[0].Rows.Count != 1)
                throw new BizException("限时抢购策略重复!");

            CountdownInfo oCountdown = new CountdownInfo();
            map(oCountdown, ds.Tables[0].Rows[0]);
            return oCountdown;
        }

        public int DeleteCountdown(int SysNo)
        {
            return new CountdownDac().Delete(SysNo);
        }
    }
}