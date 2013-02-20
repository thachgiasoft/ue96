using System;
using System.Data;
using System.Collections;
using System.Text;
using System.Timers;

using Icson.Utils;
using System.Transactions;

using Icson.Objects;
using Icson.Objects.Basic;
using Icson.Objects.Online;
using Icson.Objects.Sale;

using Icson.DBAccess;
using Icson.DBAccess.Online;

using Icson.BLL.Basic;
using Icson.BLL.Sale;

namespace Icson.BLL.Online
{
    /// <summary>
    /// Summary description for OnlineListManager.
    /// </summary>
    public class OnlineListManager
    {
        private OnlineListManager()
        {
        }
        private static OnlineListManager _instance;
        public static OnlineListManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new OnlineListManager();
            }
            return _instance;
        }

        public string onlineShowLimit = "and Product.Status = " + ((int)AppEnum.ProductStatus.Show).ToString() + " and (Product_Price.ClearanceSale=1 or Product_Price.currentprice>=IsNull(Product_Price.unitcost,0))";
        private int MaxDesLen = 150;

        private void map(OnlineListInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.ListArea = Util.TrimIntNull(tempdr["ListArea"]);
            oParam.ProductSysNo = Util.TrimIntNull(tempdr["ProductSysNo"]);
            oParam.CreateUserSysNo = Util.TrimIntNull(tempdr["CreateUserSysNo"]);
            oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
            oParam.ListOrder = Util.TrimNull(tempdr["ListOrder"]);
        }
        public void Insert(OnlineListInfo oParam)
        {
            string sql = "select top 1 sysno from onlinelist where listarea=" + oParam.ListArea + " and productsysno=" + oParam.ProductSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                throw new BizException("there is the same product in the list area you selected");

            new OnlineListDac().Insert(oParam);
        }
        public void Delete(int sysno)
        {
            new OnlineListDac().Delete(sysno);
        }

        public DataSet GetOnlineListDs(Hashtable paramHash, int topNumber, bool isOnlineShow, bool isQtyShow)
        {
            string sql = @" select @top
								OnlineList.SysNo as OnlineListSysNo, Product.SysNo as SysNo,Product.C3SysNo as C3SysNo, ProductId, ProductName, ProductDesc, ListOrder, OnlineList.ProductSysNo, left(productdesc,150) as productdescription,
								AvailableQty+VirtualQty as OnlineQty, Product.Status, UserName as CreateUserName,
								OnlineList.CreateTime, Product_Price.*,Category1.C1ID, Category2.C2ID, Category3.C3ID
							from 
								OnlineList(nolock),
								Product(nolock),
								Product_Price(nolock),
								Sys_User(nolock),
								Inventory(nolock),
								Category3(nolock),
								Category2(nolock),
								Category1(nolock)
							where
								OnlineList.productsysno = Product.sysno
							and Category3.sysno=Product.C3SysNo
							and Category2.sysno=Category3.C2SysNo
							and Category1.sysno=Category2.C1SysNo
							and OnlineList.CreateUserSysNo = Sys_User.SysNo
							and Inventory.ProductSysNo = OnlineList.ProductSysNo
							and OnlineList.ProductSysNo = Product_Price.ProductSysNo
							@onlineShowLimit
							@isqtyshow";
            if (isOnlineShow)
                sql = sql.Replace("@onlineShowLimit", onlineShowLimit);
            else
                sql = sql.Replace("@onlineShowLimit", "");

            if (isQtyShow)
                sql = sql.Replace("@isqtyshow", " and AvailableQty+VirtualQty>0 ");
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
                            sb.Append("OnlineList.ProductSysNo = ").Append(item.ToString());
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

            return SqlHelper.ExecuteDataSet(sql);
        }

        #region Category Mold

        public string GetCategoryNav(int c1SysNo)
        {
            StringBuilder sb = new StringBuilder(2000);

            sb.Append("	<div style='padding-top:5px'><table width='100%' border='0' cellspacing='0' cellpadding='0' >");
            sb.Append("		<tr>");
            sb.Append("			<td align='center'><img src='../images/ItemCategory.gif' width='188' height='34'></td>");
            sb.Append("		</tr>");
            sb.Append("	</table></div>");
            sb.Append("	<table width='188' border='0' cellpadding='0' cellspacing='0' class='channel_tdbg'");
            sb.Append("		 style='BORDER-COLLAPSE: collapse'>");
            sb.Append("		<tr>");
            sb.Append("			<td width='100%' align='center' valign='top'>");

            if (c1SysNo != AppConst.IntNull)
                sb.Append(GetCategoryNavMold(c1SysNo));
            else
            {
                CategoryManager cm = CategoryManager.GetInstance();

                StringBuilder sbAll = new StringBuilder(5000);

                SortedList sl = new SortedList(10);
                foreach (Category1Info item in cm.GetC1Hash().Values)
                {
                    if (item.Status == (int)AppEnum.BiStatus.Valid)
                        sl.Add(item.C1ID, item);
                }

                int index = 0;
                foreach (Category1Info item in sl.Values)
                {
                    string categoryStr = GetCategoryNavMold(item.SysNo);

                    if (index != 0)
                    {
                        sbAll.Append("<br>");
                    }
                    sbAll.Append(categoryStr);
                }
                sb.Append(sbAll.ToString());
            }
            sb.Append("         </td></tr>");
            sb.Append("  </table>");

            return sb.ToString();
        }

        private string GetCategoryNavMold(int c1SysNo)
        {

            CategoryManager cm = CategoryManager.GetInstance();

            Category1Info c1 = cm.GetC1Hash()[c1SysNo] as Category1Info;


            StringBuilder sb = new StringBuilder(2000);


            sb.Append("<table width='100%' border='0' align='center' cellpadding='0' cellspacing='0' >");
            sb.Append("		<tr>");
            sb.Append("			<td height='30'>");

            sb.Append("				<p><a href='../Items/Firstcategory.aspx?ID=").Append(c1.C1ID).Append("'><span class='m-4'><strong>").Append(c1.C1Name).Append("</strong></span></a><br>");
            sb.Append("				</p>");
            sb.Append("			</td>");
            sb.Append("		</tr>");
            sb.Append("		<tr>");
            sb.Append("			<td valign='top' align='right'>");
            sb.Append("				<table width='100%' border='0' cellspacing='0' cellpadding='0'>");
            sb.Append("					<tr>");
            sb.Append("						<td valign='top' align='left'>");
            sb.Append("							<table width='100%' border='0' cellpadding='0' cellspacing='0'>");
            sb.Append("								<tr>");
            sb.Append("									<td width='9%'><br></td> ");
            sb.Append("									<td width='91%' valign='top'>");

            int c2Index = 0;
            SortedList c2SL = new SortedList(cm.GetC2Hash().Count);
            foreach (Category2Info c2Item in cm.GetC2Hash().Values)
            {
                c2SL.Add(c2Item.C2ID, c2Item);
            }
            foreach (Category2Info c2Item in c2SL.Values)
            {
                if (c2Item.C1SysNo == c1SysNo && c2Item.Status == (int)AppEnum.BiStatus.Valid)
                {
                    if (c2Index != 0)
                    {
                        sb.Append("										<br>");
                        sb.Append("										<table width='100%' border='0' cellspacing='0' cellpadding='0'>");
                        sb.Append("											<tr>");
                        sb.Append("												<td height='8'><img src='../images/include/one.pix.jpg' width='1' height='1'></td>");
                        sb.Append("											</tr>");
                        sb.Append("										</table>");
                    }
                    sb.Append("										<strong><span class='ct2'>" + c2Item.C2Name + "</span><br></strong>");

                    int c3Index = 0;
                    SortedList c3SL = new SortedList(cm.GetC3Hash().Count);
                    foreach (Category3Info c3Item in cm.GetC3Hash().Values)
                    {
                        c3SL.Add(c3Item.C3ID, c3Item);
                    }
                    foreach (Category3Info c3Item in c3SL.Values)
                    {
                        if (c3Item.C2SysNo == c2Item.SysNo && c3Item.Status == (int)AppEnum.BiStatus.Valid)
                        {
                            if (c3Index != 0)
                            {
                                sb.Append("、");
                            }
                            sb.Append("										<a href='ThirdCategory.aspx?ID=").Append(c3Item.SysNo.ToString()).Append("'>").Append(c3Item.C3Name).Append("</a>");

                            c3Index++;
                        }
                    }
                    c2Index++;
                }
            }
            sb.Append("									</td>");
            sb.Append("								</tr>");
            sb.Append("							</table>");
            sb.Append("						</td>");
            sb.Append("					</tr>");
            sb.Append("				</table>");
            sb.Append("			</td>");
            sb.Append("		</tr>");
            sb.Append("	</table>");

            return sb.ToString();
        }

        public string GetFirstCategoryNav(int c1SysNo, int c2SysNo)
        {
            CategoryManager cm = CategoryManager.GetInstance();
            Category1Info c1 = cm.GetC1Hash()[c1SysNo] as Category1Info;

            int c2Index = 0;
            SortedList c2SL = new SortedList(cm.GetC2Hash().Count);
            foreach (Category2Info c2Item in cm.GetC2Hash().Values)
            {
                c2SL.Add(c2Item.C2ID, c2Item);
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("<div id=fl_dnpj class=panel2>");
            sb.Append("<div class=panel_title>");
            //sb.Append(  "<img src='../images/site/main/left/tt_dnyj.png' alt='" + c1.C1Name + "' />");
            sb.Append(c1.C1Name);
            sb.Append("</div>");
            sb.Append("<div class=panel_content>");
            sb.Append("<div class=c_dnpj>");
            foreach (Category2Info c2Item in c2SL.Values)
            {
                if (c2Item.C1SysNo == c1SysNo && c2Item.Status == (int)AppEnum.BiStatus.Valid)
                {
                    sb.Append("<span style='cursor:hand' onclick=\"return ShowThirdCategory('SecondCategory" + c2Item.SysNo + "');\"><strong>" + c2Item.C2Name + "</strong></span>");
                    c2Index++;
                    if (c2SysNo == 0 && c2Index == 1)
                    {
                        sb.Append("<div id='SecondCategory" + c2Item.SysNo + "' style='DISPLAY:' class=c_dnpj_sl>");
                    }
                    else if (c2SysNo == c2Item.SysNo)
                    {
                        sb.Append("<div id='SecondCategory" + c2Item.SysNo + "' style='DISPLAY:' class=c_dnpj_sl>");
                    }
                    else
                    {
                        sb.Append("<div id='SecondCategory" + c2Item.SysNo + "' style='DISPLAY: none' class=c_dnpj_sl>");
                    }
                    //int c3Index = 0;
                    SortedList c3SL = new SortedList(cm.GetC3Hash().Count);
                    foreach (Category3Info c3Item in cm.GetC3Hash().Values)
                    {
                        c3SL.Add(c3Item.C3ID, c3Item);
                    }
                    foreach (Category3Info c3Item in c3SL.Values)
                    {
                        if (c3Item.C2SysNo == c2Item.SysNo && c3Item.Status == (int)AppEnum.BiStatus.Valid)
                        {
                            sb.Append("<span><a href='../Habiliment/ThirdCategory.aspx?ID=" + c3Item.SysNo + "'>" + c3Item.C3Name + "</a></span>");
                        }
                    }
                    sb.Append("</div>");
                }
            }
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");

            return sb.ToString();
        }

        public string GetC2BrandNav(int c2sysno)
        {
            string sql = @"select c3.sysno as c3sysno,c3.c3name,a1.sysno as a1sysno,a1.attribute1name,a2.sysno as a2sysno,a2.attribute2name,opn.sysno as a2optionsysno,opn.attribute2optionname from category2 c2 inner join category3 c3 on c3.c2sysno=c2.sysno
                            inner join Category_Attribute1 a1 on c3.sysno=a1.c3sysno 
                            inner join Category_Attribute2 a2 on a2.a1sysno=a1.sysno
                            inner join Category_Attribute2_Option opn on opn.attribute2sysno=a2.sysno
                            where c3.status=0 and a1.status=0 and a2.status=0 and opn.status=0 and c2.sysno=@c2sysno and attribute2name = '品牌' 
                            order by c3.c3id,a1.ordernum,a2.ordernum,opn.ordernum ";
            sql = sql.Replace("@c2sysno", c2sysno.ToString());
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return "";

            CategoryManager cm = CategoryManager.GetInstance();
            Category2Info c2 = cm.GetC2Hash()[c2sysno] as Category2Info;
            Category1Info c1 = cm.GetC1Hash()[c2.C1SysNo] as Category1Info;

            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=category>");
            sb.Append("<div class=category_img></div>");
            sb.Append("<div class=category_title><a href='../Items/Default.aspx'>首页</a> --> <a href='../Items/FirstCategory.aspx?ID=" + c1.SysNo + "'>" + c1.C1Name + "</a> --> <a href='../Items/SecondCategory.aspx?ID=" + c2.SysNo + "'><span class=color_f60>" + c2.C2Name + "</span></a></div>");
            sb.Append("<div class=category_content>");

            int orignC3SysNo = 0;
            int c3sysno = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                c3sysno = Util.TrimIntNull(dr["c3sysno"]);
                if (orignC3SysNo != c3sysno)
                {
                    if (orignC3SysNo != 0)
                    {
                        sb.Append("</li>");
                        sb.Append("</ul>");
                        //sb.Append("<br clear=all />");
                        sb.Append("<div style='clear:both'></div>");
                        sb.Append("</div>");
                    }

                    sb.Append("<div class=category_li2>");
                    sb.Append("<ul>");
                    sb.Append("<li><a href='../Items/ThirdCategory.aspx?ID=" + c3sysno + "'>" + Util.TrimNull(dr["c3name"]) + ":</a></li>");
                    sb.Append("<li><a href='../Items/ThirdCategory.aspx?ID=" + c3sysno + "'>所有品牌</a>");
                }
                sb.Append("<a href='../Items/ThirdCategory.aspx?ID=" + c3sysno + "&Brand=" + Util.TrimNull(dr["a2optionsysno"]) + "'>" + Util.TrimNull(dr["attribute2optionname"]) + "</a>");
                orignC3SysNo = c3sysno;
            }
            sb.Append("</li>");
            sb.Append("</ul>");
            //sb.Append(      "<br clear=all />");
            sb.Append("<div style='clear:both'></div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");

            return sb.ToString();
        }

        public string GetFirstCategoryNav(int c3sysno)
        {
            CategoryManager cm = CategoryManager.GetInstance();
            Category3Info c3 = cm.GetC3Hash()[c3sysno] as Category3Info;
            int c2sysno = c3.C2SysNo;
            Category2Info c2 = cm.GetC2Hash()[c2sysno] as Category2Info;
            int c1sysno = c2.C1SysNo;
            return GetFirstCategoryNav(c1sysno, c2sysno);
        }

        #endregion

        #region Brand
        public DataSet GetBrandSearchDs(int c3Sysno, int ManufacturerSysNo)
        {
            string sql = @"select productid,
								product.sysno, productname,PromotionWord, productmode, availableqty+virtualqty as onlineqty,
								product_price.*,product.createtime,
                                case when product.createtime >= DATEADD(day, -5, getdate()) then '9999999' else product.avgdailyclick end as avgdailyclick 
							from
								product(nolock), inventory(nolock), product_price(nolock) 
							where
								product.sysno = inventory.productsysno
							and product.sysno = product_price.productsysno 

							and product.ManufacturerSysNo = @ManufacturerSysNo";
//                            and product.SysNo not in (select productsysno from sale_countdown where status = 1 )
//							@onlineShowLimit
            sql = sql.Replace("@ManufacturerSysNo", ManufacturerSysNo.ToString());

            if (c3Sysno != AppConst.IntNull && c3Sysno > 0)
                sql += " and product.c3Sysno = " + c3Sysno;

            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return null;
            else
                return ds;

        }
        #endregion

        #region ThirdCategory
        public string Get3rdCategory(int c3Sysno, int manufacturerSysNo, bool isSecondhand, int orderby)
        {
            string sql = @"select
								product.sysno, productname, productmode, availableqty+virtualqty as onlineqty,
								product_price.*,product.createtime
							from
								product(nolock), inventory(nolock), product_price(nolock)
							where
								product.sysno = inventory.productsysno
							and product.sysno = product_price.productsysno 
							@onlineShowLimit
							and product.C3SysNo = @c3sysno";

            sql = sql.Replace("@onlineShowLimit", this.onlineShowLimit);
            sql = sql.Replace("@c3sysno", c3Sysno.ToString());

            if (manufacturerSysNo != AppConst.IntNull)
                sql += " and product.manufacturerSysNo = " + manufacturerSysNo;
            if (isSecondhand)
            {
                //sql += " and productid like '%R%'";
                sql += " and product.producttype<>1";
            }
            else
            {
                //sql += " and productid not like '%R%' and productid not like '%B%' ";
                sql += " and product.producttype<>1 and product.producttype<>2 ";
            }

            if (orderby == 0)
            {
                sql += " order by productname";
            }
            else if (orderby == 1)
            {
                sql += " order by onlineqty desc";
            }
            else if (orderby == 2)
            {
                sql += " order by currentprice";
            }
            else if (orderby == 3)
            {
                sql += " order by currentprice desc";
            }
            else if (orderby == 4)
            {
                sql += " order by manufacturersysno";
            }
            else
            {
                sql += " order by newid()";
            }

            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return "";

            StringBuilder sb = new StringBuilder(2000);
            string begin = @"<table width='96%'  align='center' border='1' cellpadding='0' cellspacing='4' bordercolor='#CCCCCC' style='BORDER-COLLAPSE: collapse'>
								<tr bgcolor='#EEEEEE'>
									<td width='90' height='15'>商品名称</td>
									<td width='19%' align=center>型号</td>
									<td width='8%' align=center>当前价格</td>
									<td width='7%' align=center>现金抵用券</td>
									<td width='4%' align=center>积分</td>
									<td width='7%' align=center>库存状况</td>
									<td width='9%' align=center>购买</td>
									<td width='10%' align=center>比较</td>
								</tr>";
            sb.Append(begin);

            int index = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (index % 2 == 0)
                    sb.Append(" <tr>");
                else
                    sb.Append(" <tr bgcolor=#f5f5f5>");

                sb.Append("		<td height='15' weight=90><a href='../Items/ItemDetail.aspx?ItemID=").Append(dr["SysNo"].ToString()).Append("' target='_blank'><strong>").Append(dr["ProductName"].ToString()).Append("</strong></a></td>");
                sb.Append("		<td align=center>").Append(dr["productmode"].ToString()).Append("</td>");
                sb.Append("     <td align=center>").Append(((decimal)dr["currentprice"] + (decimal)dr["cashRebate"]).ToString(AppConst.DecimalFormat)).Append("</td>");
                if (Util.TrimDecimalNull(dr["cashRebate"]) > 0)
                    sb.Append("     <td align=center>").Append(((decimal)dr["cashRebate"]).ToString(AppConst.DecimalFormat)).Append("</td>");
                else
                    sb.Append("     <td align=center>---</td>");

                if (Util.TrimIntNull(dr["point"]) > 0)
                    sb.Append("     <td align=center>赠送:").Append((int)dr["point"]).Append("积分</td>");
                else
                    sb.Append("     <td align=center>---</td>");

                if (Util.TrimIntNull(dr["onlineqty"]) > 0)
                {
                    sb.Append("     <td align=center>有</td>");
                    sb.Append("     <td align=center><a href='javascript:AddToCart(").Append(dr["SysNo"].ToString()).Append(")'><font class=m-3>加入购物车</font></a></td>");
                }
                else
                {
                    sb.Append("     <td align=center>无</td>");
                    sb.Append("     <td align=center class=m-3><a href='../Account/ItemArrivedTellMe.aspx?ItemID=").Append(dr["sysno"].ToString()).Append(" '><font color=red>到货通知我</font></a></td>");
                }

                sb.Append("     <td align=center><input type='checkbox' name='chk").Append(dr["sysno"].ToString()).Append("'></td>");
                sb.Append(" </tr>");
                index++;
            }

            sb.Append("</table>");

            return sb.ToString();

        }

        //        public DataSet Get3rdCategorySearchDs(int c3Sysno, int manufacturerSysNo, bool isSecondhand, int orderby, string[] arrayFilter, Hashtable htFilter)
        //        {
        //            string sql = @"select productid,
        //								product.sysno, productname,PromotionWord, productmode, availableqty+virtualqty as onlineqty,
        //								product_price.*,product.createtime,
        //                                case when product.createtime >= DATEADD(day, -5, getdate()) then '9999999' else product.avgdailyclick end as avgdailyclick 
        //							from
        //								product(nolock), inventory(nolock), product_price(nolock) 
        //							where
        //								product.sysno = inventory.productsysno
        //							and product.sysno = product_price.productsysno 
        //							@onlineShowLimit
        //							and product.C3SysNo = @c3sysno";

        //            if (arrayFilter != null && arrayFilter.GetLength(0) > 0)
        //            {
        //                for (int i = 0; i < arrayFilter.Length; i++)
        //                {
        //                    if (arrayFilter[i] != null && arrayFilter[i].Length > 0 && arrayFilter[i].IndexOf(";") > 0)
        //                    {
        //                        string[] arTemp = arrayFilter[i].Split(';');
        //                        sql += " and exists(select productsysno from product_attribute2(nolock) where product_attribute2.attribute2optionsysno='" + arTemp[1].Trim() + "' and product_attribute2.productsysno=product.sysno)";
        //                    }
        //                }
        //            }

        //            if (htFilter != null && htFilter.Count > 0)
        //            {
        //                foreach (string key in htFilter.Keys)
        //                {
        //                    sql += " and exists(select productsysno from product_attribute2(nolock) where product_attribute2.attribute2optionsysno='" + key + "' and product_attribute2.productsysno=product.sysno)";
        //                }
        //            }

        //            sql = sql.Replace("@onlineShowLimit", this.onlineShowLimit);
        //            sql = sql.Replace("@c3sysno", c3Sysno.ToString());

        //            if (manufacturerSysNo != AppConst.IntNull)
        //                sql += " and product.manufacturerSysNo = " + manufacturerSysNo;
        //            if (isSecondhand)
        //            {
        //                sql += " and product.producttype=1";
        //            }
        //            else
        //            {
        //                sql += " and product.producttype<>1 and product.producttype<>2 ";
        //            }

        //            if (orderby == 0)
        //            {
        //                sql += " order by productname";
        //            }
        //            else if (orderby == 1)
        //            {
        //                sql += " order by onlineqty desc";
        //            }
        //            else if (orderby == 2)
        //            {
        //                sql += " order by currentprice";
        //            }
        //            else if (orderby == 3)
        //            {
        //                sql += " order by currentprice desc";
        //            }
        //            else if (orderby == 4)
        //            {
        //                sql += " order by manufacturersysno";
        //            }
        //            else
        //            {
        //                sql += " order by product.ordernum,avgdailyclick desc, product.sysno desc";
        //            }

        //            DataSet ds = SqlHelper.ExecuteDataSet(sql);

        //            if (!Util.HasMoreRow(ds))
        //                return null;
        //            else
        //                return ds;

        //        }

        public DataSet Get3rdCategorySearchDs(int c3Sysno, int manufacturerSysNo, bool isSecondhand, int orderby, string[] arrayFilter, Hashtable htFilter)
        {
            string sql = @"select productid,
								product.sysno, productname,PromotionWord, productmode, availableqty+virtualqty as onlineqty,
								product_price.*,product.createtime,
                                case when product.createtime >= DATEADD(day, -5, getdate()) then '9999999' else product.avgdailyclick end as avgdailyclick 
							from
								product(nolock), inventory(nolock), product_price(nolock) 
							where
								product.sysno = inventory.productsysno
							and product.sysno = product_price.productsysno 
and product.SysNo not in (select productsysno from sale_countdown where status = 1 )
							@onlineShowLimit
							and product.C3SysNo = @c3sysno";

            //注：加上 and product.SysNo not in (select productsysno from sale_countdown where status = 1 ) 限时特卖中的商品不在其它栏目显示

            if (arrayFilter != null && arrayFilter.GetLength(0) > 0)
            {
                for (int i = 0; i < arrayFilter.Length; i++)
                {
                    if (arrayFilter[i] != null && arrayFilter[i].Length > 0 && arrayFilter[i].IndexOf(";") > 0)
                    {
                        string[] arTemp = arrayFilter[i].Split(';');
                        sql += " and exists(select productsysno from product_attribute2(nolock) where product_attribute2.attribute2optionsysno='" + arTemp[1].Trim() + "' and product_attribute2.productsysno=product.sysno)";
                    }
                }
            }

            if (htFilter != null && htFilter.Count > 0)
            {
                foreach (string key in htFilter.Keys)
                {
                    sql += " and exists(select productsysno from product_attribute2(nolock) where product_attribute2.attribute2optionsysno='" + key + "' and product_attribute2.productsysno=product.sysno)";
                }
            }

            sql = sql.Replace("@onlineShowLimit", this.onlineShowLimit);
            sql = sql.Replace("@c3sysno", c3Sysno.ToString());

            if (manufacturerSysNo != AppConst.IntNull)
                sql += " and product.manufacturerSysNo = " + manufacturerSysNo;
            if (isSecondhand)
            {
                sql += " and product.producttype=1";
            }
            else
            {
                sql += " and product.producttype<>1 and product.producttype<>2 ";
            }

            //不显示子商品
            sql += " and product.product2ndtype < 2 ";

            if (orderby == 0)
            {
                sql += " order by productname";
            }
            else if (orderby == 1)
            {
                sql += " order by onlineqty desc";
            }
            else if (orderby == 2)
            {
                sql += " order by currentprice";
            }
            else if (orderby == 3)
            {
                sql += " order by currentprice desc";
            }
            else if (orderby == 4)
            {
                sql += " order by manufacturersysno";
            }
            else
            {
                sql += " order by product.ordernum,avgdailyclick desc, product.sysno desc";
            }

            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return null;
            else
                return ds;

        }

        public string Get3rdCategorySearch(int c3Sysno, int manufacturerSysNo, bool isSecondhand, int orderby, string[] arrayFilter, Hashtable htFilter)
        {
            string sql = @"select
								product.sysno, productname,PromotionWord, productmode, availableqty+virtualqty as onlineqty,
								product_price.*,product.createtime,
                                case when product.createtime >= DATEADD(day, -5, getdate()) then '9999999' else product.avgdailyclick end as avgdailyclick 
							from
								product(nolock), inventory(nolock), product_price(nolock) 
							where
								product.sysno = inventory.productsysno
							and product.sysno = product_price.productsysno 
							@onlineShowLimit
							and product.C3SysNo = @c3sysno";

            if (arrayFilter != null && arrayFilter.GetLength(0) > 0)
            {
                for (int i = 0; i < arrayFilter.Length; i++)
                {
                    if (arrayFilter[i] != null && arrayFilter[i].Length > 0 && arrayFilter[i].IndexOf(";") > 0)
                    {
                        string[] arTemp = arrayFilter[i].Split(';');
                        sql += " and exists(select productsysno from product_attribute2(nolock) where product_attribute2.attribute2optionsysno='" + arTemp[1].Trim() + "' and product_attribute2.productsysno=product.sysno)";
                    }
                }
            }

            if (htFilter != null && htFilter.Count > 0)
            {
                foreach (string key in htFilter.Keys)
                {
                    sql += " and exists(select productsysno from product_attribute2(nolock) where product_attribute2.attribute2optionsysno='" + key + "' and product_attribute2.productsysno=product.sysno)";
                }
            }

            sql = sql.Replace("@onlineShowLimit", this.onlineShowLimit);
            sql = sql.Replace("@c3sysno", c3Sysno.ToString());

            if (manufacturerSysNo != AppConst.IntNull)
                sql += " and product.manufacturerSysNo = " + manufacturerSysNo;
            if (isSecondhand)
            {
                sql += " and product.producttype=1";
            }
            else
            {
                sql += " and product.producttype<>1 and product.producttype<>2 ";
            }

            if (orderby == 0)
            {
                sql += " order by productname";
            }
            else if (orderby == 1)
            {
                sql += " order by onlineqty desc";
            }
            else if (orderby == 2)
            {
                sql += " order by currentprice";
            }
            else if (orderby == 3)
            {
                sql += " order by currentprice desc";
            }
            else if (orderby == 4)
            {
                sql += " order by manufacturersysno";
            }
            else
            {
                sql += " order by product.ordernum,avgdailyclick desc, product.sysno desc";
            }

            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return "";

            StringBuilder sb = new StringBuilder(2000);
            string begin = @"<table width='96%'  align='center' border='1' cellpadding='0' cellspacing='4' bordercolor='#CCCCCC' style='BORDER-COLLAPSE: collapse'>
								<tr bgcolor='#EEEEEE'>
                                    <td width='10%' align=center>对比</td>
									<td width='90' height='15'>商品名称</td>
									<td width='19%' align=center>型号</td>
									<td width='8%' align=center>当前价格</td>
									<td width='7%' align=center>现金抵用券</td>
									<td width='4%' align=center>积分</td>
									<td width='7%' align=center>库存状况</td>
									<td width='9%' align=center>购买</td>									
								</tr>";
            sb.Append(begin);

            int index = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (index % 2 == 0)
                    sb.Append(" <tr>");
                else
                    sb.Append(" <tr bgcolor=#f5f5f5>");

                sb.Append("     <td align=center><input type='checkbox' value='" + dr["sysno"].ToString() + "' name='chkCompare").Append(dr["sysno"].ToString()).Append("'></td>");

                sb.Append("		<td height='15' weight=90><a href='../Items/ItemDetail.aspx?ItemID=").Append(dr["SysNo"].ToString()).Append("' target='_blank'><strong>").Append(dr["ProductName"].ToString() + "<font color=red>" + Util.TrimNull(dr["PromotionWord"]) + "</font>").Append("</strong></a></td>");
                sb.Append("		<td align=center>").Append(dr["productmode"].ToString()).Append("</td>");
                sb.Append("     <td align=center>").Append(((decimal)dr["currentprice"] + (decimal)dr["cashRebate"]).ToString(AppConst.DecimalFormat)).Append("</td>");
                if (Util.TrimDecimalNull(dr["cashRebate"]) > 0)
                    sb.Append("     <td align=center>").Append(((decimal)dr["cashRebate"]).ToString(AppConst.DecimalFormat)).Append("</td>");
                else
                    sb.Append("     <td align=center>---</td>");

                if (Util.TrimIntNull(dr["point"]) > 0)
                    sb.Append("     <td align=center>赠送:").Append((int)dr["point"]).Append("积分</td>");
                else
                    sb.Append("     <td align=center>---</td>");

                if (Util.TrimIntNull(dr["onlineqty"]) > 0)
                {
                    sb.Append("     <td align=center>有</td>");
                    sb.Append("     <td align=center><a href='javascript:AddToCart(").Append(dr["SysNo"].ToString()).Append(")'><font class=m-3>加入购物车</font></a></td>");
                }
                else
                {
                    sb.Append("     <td align=center>无</td>");
                    sb.Append("     <td align=center class=m-3><a href='../Account/ItemArrivedTellMe.aspx?ItemID=").Append(dr["sysno"].ToString()).Append(" '><font color=red>到货通知我</font></a></td>");
                }
                sb.Append(" </tr>");
                index++;
            }

            sb.Append("</table>");

            return sb.ToString();

        }

        //        public DataSet Get3rdCategoryDataSetSearch(int c3Sysno, int manufacturerSysNo, bool isSecondhand, int orderby, string[] arrayFilter, Hashtable htFilter)
        //        {
        //            string sql = @"select productID,
        //								product.sysno, productname,PromotionWord, productmode, availableqty+virtualqty as onlineqty,
        //								product_price.*,product.createtime,
        //                                case when product.createtime >= DATEADD(day, -5, getdate()) then '9999999' else product.avgdailyclick end as avgdailyclick 
        //							from
        //								product(nolock), inventory(nolock), product_price(nolock) 
        //							where
        //								product.sysno = inventory.productsysno
        //							and product.sysno = product_price.productsysno 
        //							@onlineShowLimit
        //							and product.C3SysNo = @c3sysno";

        //            if (arrayFilter != null && arrayFilter.GetLength(0) > 0)
        //            {
        //                for (int i = 0; i < arrayFilter.Length; i++)
        //                {
        //                    if (arrayFilter[i] != null && arrayFilter[i].Length > 0 && arrayFilter[i].IndexOf(";") > 0)
        //                    {
        //                        string[] arTemp = arrayFilter[i].Split(';');
        //                        sql += " and exists(select productsysno from product_attribute2(nolock) where product_attribute2.attribute2optionsysno='" + arTemp[1].Trim() + "' and product_attribute2.productsysno=product.sysno)";
        //                    }
        //                }
        //            }

        //            if (htFilter != null && htFilter.Count > 0)
        //            {
        //                foreach (string key in htFilter.Keys)
        //                {
        //                    sql += " and exists(select productsysno from product_attribute2(nolock) where product_attribute2.attribute2optionsysno='" + key + "' and product_attribute2.productsysno=product.sysno)";
        //                }
        //            }

        //            sql = sql.Replace("@onlineShowLimit", this.onlineShowLimit);
        //            sql = sql.Replace("@c3sysno", c3Sysno.ToString());

        //            if (manufacturerSysNo != AppConst.IntNull)
        //                sql += " and product.manufacturerSysNo = " + manufacturerSysNo;
        //            if (isSecondhand)
        //            {
        //                sql += " and product.producttype=1";
        //            }
        //            else
        //            {
        //                sql += " and product.producttype<>1 and product.producttype<>2 ";
        //            }

        //            if (orderby == 0)
        //            {
        //                sql += " order by productname";
        //            }
        //            else if (orderby == 1)
        //            {
        //                sql += " order by onlineqty desc";
        //            }
        //            else if (orderby == 2)
        //            {
        //                sql += " order by currentprice";
        //            }
        //            else if (orderby == 3)
        //            {
        //                sql += " order by currentprice desc";
        //            }
        //            else if (orderby == 4)
        //            {
        //                sql += " order by manufacturersysno";
        //            }
        //            else
        //            {
        //                sql += " order by product.ordernum,avgdailyclick desc, product.sysno desc";
        //            }

        //            DataSet ds = SqlHelper.ExecuteDataSet(sql);

        //            if (!Util.HasMoreRow(ds))
        //                return null;

        //            return ds;

        //        }

        public DataSet Get3rdCategoryDataSetSearch(int c3Sysno, int manufacturerSysNo, bool isSecondhand, int orderby, string[] arrayFilter, Hashtable htFilter)
        {
            string sql = @"select productID,
								product.sysno, productname,PromotionWord, productmode, availableqty+virtualqty as onlineqty,
								product_price.*,product.createtime,
                                case when product.createtime >= DATEADD(day, -5, getdate()) then '9999999' else product.avgdailyclick end as avgdailyclick 
							from
								product(nolock), inventory(nolock), product_price(nolock) 
							where
								product.sysno = inventory.productsysno
							and product.sysno = product_price.productsysno 
							@onlineShowLimit
							and product.C3SysNo = @c3sysno";

            if (arrayFilter != null && arrayFilter.GetLength(0) > 0)
            {
                for (int i = 0; i < arrayFilter.Length; i++)
                {
                    if (arrayFilter[i] != null && arrayFilter[i].Length > 0 && arrayFilter[i].IndexOf(";") > 0)
                    {
                        string[] arTemp = arrayFilter[i].Split(';');
                        sql += " and exists(select productsysno from product_attribute2(nolock) where product_attribute2.attribute2optionsysno='" + arTemp[1].Trim() + "' and product_attribute2.productsysno=product.sysno)";
                    }
                }
            }

            if (htFilter != null && htFilter.Count > 0)
            {
                foreach (string key in htFilter.Keys)
                {
                    sql += " and exists(select productsysno from product_attribute2(nolock) where product_attribute2.attribute2optionsysno='" + key + "' and product_attribute2.productsysno=product.sysno)";
                }
            }

            sql = sql.Replace("@onlineShowLimit", this.onlineShowLimit);
            sql = sql.Replace("@c3sysno", c3Sysno.ToString());

            if (manufacturerSysNo != AppConst.IntNull)
                sql += " and product.manufacturerSysNo = " + manufacturerSysNo;
            if (isSecondhand)
            {
                sql += " and product.producttype=1";
            }
            else
            {
                sql += " and product.producttype<>1 and product.producttype<>2 ";
            }

            if (orderby == 0)
            {
                sql += " order by productname";
            }
            else if (orderby == 1)
            {
                sql += " order by onlineqty desc";
            }
            else if (orderby == 2)
            {
                sql += " order by currentprice";
            }
            else if (orderby == 3)
            {
                sql += " order by currentprice desc";
            }
            else if (orderby == 4)
            {
                sql += " order by manufacturersysno";
            }
            else
            {
                sql += " order by product.ordernum,avgdailyclick desc, product.sysno desc";
            }

            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return null;

            return ds;

        }


        public string Get3rdCategoryWithPic(int c3Sysno, int manufacturerSysNo, bool isSecondhand, int orderby, int currentPage)
        {
            string sql = @"select
								product.sysno, productid, left(productid,10)as picid, productname, PromotionWord,
								left(productdesc,150) as productdescription, 
								availableqty+virtualqty as onlineqty,
								product_price.*
							from
								product(nolock), inventory(nolock), product_price(nolock)
							where
								product.sysno = inventory.productsysno
							and product.sysno = product_price.productsysno 
							@onlineShowLimit
							and product.C3SysNo = @c3sysno";

            sql = sql.Replace("@onlineShowLimit", this.onlineShowLimit);
            sql = sql.Replace("@c3sysno", c3Sysno.ToString());

            if (manufacturerSysNo != AppConst.IntNull)
                sql += " and product.manufacturerSysNo = " + manufacturerSysNo;

            if (isSecondhand)
            {
                sql += " and product.productype=1";
            }
            else
            {
                sql += " and product.producttype<>1 and product.producttype<>2 ";
            }

            if (orderby == 0)
            {
                sql += " order by productname";
            }
            else if (orderby == 1)
            {
                sql += " order by onlineqty desc";
            }
            else if (orderby == 2)
            {
                sql += " order by currentprice";
            }
            else if (orderby == 3)
            {
                sql += " order by currentprice desc";
            }
            else if (orderby == 4)
            {
                sql += " order by manufacturersysno";
            }


            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return "";

            string seperator = @"
						<table width='100%' border='0' align='center' cellpadding='0' cellspacing='0'>
							 <tr>
  								<TD width='100%' bgcolor='#999999' heigth='1' colspan='2'><img src='../Image/spacer.gif'></td>
							 </tr>
						</table>";
            //<tr> 
            //	<td height='3''><img src='../images/dot_line02.gif' width='1' height='1'></td>
            //</tr>
            //<tr> 
            //		<td height='1' background='../images/dot_line02.gif'><img src='../images/dot_line02.gif' width='148' height='1'></td>
            //	</tr>
            //		<tr> 
            //			<td height='3'><img src='../images/dot_line02.gif' width='1' height='1'></td>
            //		</tr>

            int pageSize = 10;
            int totalPage = ds.Tables[0].Rows.Count / pageSize;
            if (ds.Tables[0].Rows.Count % pageSize != 0)
                totalPage += 1;

            if (currentPage > totalPage)
                currentPage = 1;

            StringBuilder sb = new StringBuilder(2000);
            //sb.Append(seperator);
            int rowindex = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (rowindex >= (currentPage - 1) * pageSize && rowindex < currentPage * pageSize)
                {
                    sb.Append(Get3rdCategoryWithPicMold(dr, AppConst.StringNull));
                    sb.Append(seperator);
                }
                rowindex++;
            }
            return sb.ToString();
        }

        //		//Category3 search options 列出搜索选项
        //		public DataSet Get3rdCategorySearchOptions(int c3Sysno,int AttributeType)
        //		{
        //			string sql = @"select C3SysNo,SysNo as AttributeSysNo,AttributeID + '_AttributeOptionSysNo' as AttributeID,AttributeName from category_attribute 
        //							where C3SysNo=@c3sysno and Status='0' and AttributeType=@AttributeType order by OrderNum";
        //            sql = sql.Replace("@c3sysno",c3Sysno.ToString());
        //			sql = sql.Replace("@AttributeType",AttributeType.ToString());
        //
        //			DataSet ds = SqlHelper.ExecuteDataSet(sql);
        //			if(Util.HasMoreRow(ds))
        //			{
        //				StringBuilder sb = new StringBuilder();
        //				sb.Append(sql + ";");
        //				foreach(DataRow dr in ds.Tables[0].Rows)
        //				{
        //					sb.Append(@"select SysNo as AttributeOptionSysNo,AttributeOptionName from category_attribute_option where AttributeSysNo='"+ dr["AttributeSysNo"].ToString().Trim() +"' and Status='0' order by OrderNum;");
        //				}
        //
        //				sql = sb.ToString().Substring(0,sb.Length-1);
        //				return SqlHelper.ExecuteDataSet(sql);
        //			}
        //			else
        //			{
        //				return null;
        //			}
        //		}
        //		public DataSet Get3rdCategorySearchOptions(int c3Sysno,int Attribute2Type)
        //		{
        //			string sql = @"select b.sysno as attribute2sysno,b.attribute2id,b.attribute2name from category_attribute1 a inner join category_attribute2 b 
        //						   on a.sysno=b.a1sysno and a.c3sysno=@c3Sysno and a.status='0' and b.status='0' and b.attribute2type=@attribute2type
        //						   order by a.ordernum,b.ordernum";
        //			sql = sql.Replace("@c3sysno",c3Sysno.ToString());
        //			sql = sql.Replace("@attribute2type",Attribute2Type.ToString());
        //
        //			DataSet ds = SqlHelper.ExecuteDataSet(sql);
        //			if(Util.HasMoreRow(ds))
        //			{
        //				StringBuilder sb = new StringBuilder();
        //				sb.Append(sql + ";");
        //				foreach(DataRow dr in ds.Tables[0].Rows)
        //				{
        //					//sb.Append(@"select sysno as attribute2optionsysno,attribute2optionname from category_attribute2_option where attribute2sysno='"+ dr["attribute2sysno"].ToString().Trim() +"' and status='0' order by ordernum;");
        //					sb.Append(@"select a.sysno as attribute2optionsysno,a.attribute2optionname,count(a.sysno) as itemCount,a.ordernum from category_attribute2_option a,product b,product_attribute2 c where a.attribute2sysno='"+ dr["attribute2sysno"].ToString().Trim() +"' and a.status='0' and a.sysno=c.attribute2optionsysno and b.sysno=c.productsysno group by a.sysno,a.attribute2optionname,a.ordernum order by a.ordernum;");
        //				}
        //
        //				sql = sb.ToString().Substring(0,sb.Length-1);
        //				return SqlHelper.ExecuteDataSet(sql);
        //			}
        //			else
        //			{
        //				return null;
        //			}
        //		}

        /// <summary>
        /// Category3 search options 列出搜索选项
        /// </summary>
        /// <param name="c3Sysno"></param>
        /// <param name="Attribute2Type"></param>
        /// <param name="isSecondhand"></param>
        /// <param name="arrayFilter">第一分类筛选条件</param>
        /// <param name="htFilter">第一分类筛选条件</param>
        /// <param name="RangeFrom">价格区间From</param>
        /// <param name="RangeTo">价格区间To</param>
        /// <returns></returns>
        // viewC3PA_xxx 替换成　product_attribute2
        public DataSet Get3rdCategorySearchOptions(int c3Sysno, int Attribute2Type, bool isSecondhand, string[] arrayFilter, Hashtable htFilter, int RangeFrom, int RangeTo)
        {
            string viewName = "";
            if (c3Sysno <= 100)
            {
                viewName = "viewC3PA_100";
            }
            else if (c3Sysno <= 150)
            {
                viewName = "viewC3PA_150";
            }
            else if (c3Sysno <= 200)
            {
                viewName = "viewC3PA_200";
            }
            else if (c3Sysno <= 300)
            {
                viewName = "viewC3PA_300";
            }
            else if (c3Sysno <= 500)
            {
                viewName = "viewC3PA_500";
            }
            else
            {
                viewName = "viewC3PA_999";
            }

            string sql = @"select b.sysno as attribute2sysno,b.attribute2id,b.attribute2name from category_attribute1 a(nolock) inner join category_attribute2 b(nolock) 
						   on a.sysno=b.a1sysno and a.c3sysno=@c3Sysno and a.status='0' and b.status='0' and b.attribute2type=@attribute2type
						   order by a.ordernum,b.ordernum";
            string sExists = "";
            if (arrayFilter != null && arrayFilter.GetLength(0) > 0)
            {
                for (int i = 0; i < arrayFilter.Length; i++)
                {
                    if (arrayFilter[i] != null && arrayFilter[i].Length > 0 && arrayFilter[i].IndexOf(";") > 0)
                    {
                        string[] arTemp = arrayFilter[i].Split(';');
                        sExists += " and exists(select product_attribute2.productsysno from product_attribute2(nolock) where product_attribute2.attribute2optionsysno='" + arTemp[1].Trim() + "' and product_attribute2.productsysno=product.sysno)";
                    }
                }
            }

            if (htFilter != null && htFilter.Count > 0)
            {
                foreach (string key in htFilter.Keys)
                {
                    sExists += " and exists(select product_attribute2.productsysno from product_attribute2(nolock) where product_attribute2.attribute2optionsysno='" + key.Trim() + "' and product_attribute2.productsysno=product.sysno)";
                }
            }

            if (RangeFrom >= 0)
            {
                sExists += " and exists(select product_price.productsysno from product_price(nolock) where product_price.currentprice >= " + RangeFrom.ToString().Trim() + " and product_price.productsysno=product.sysno)";
            }

            if (RangeTo > 0)
            {
                sExists += " and exists(select product_price.productsysno from product_price(nolock) where product_price.currentprice < " + RangeTo.ToString().Trim() + " and product_price.productsysno=product.sysno)";
            }

            Category3Info c3Info = CategoryManager.GetInstance().GetC3Hash()[c3Sysno] as Category3Info;
            if (c3Info.C3Type == 1) //鞋类商品只统计可订购商品
            {
                sExists += " and exists(select inventory.productsysno from inventory(nolock) where inventory.AvailableQty+inventory.VirtualQty>0 and inventory.productsysno=product.sysno)";
            }

            sql = sql.Replace("@c3Sysno", c3Sysno.ToString());
            sql = sql.Replace("@attribute2type", Attribute2Type.ToString());

            sql = sql.Replace("product_attribute2", viewName);

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(sql + ";");
                //foreach (DataRow dr in ds.Tables[0].Rows)
                //{
                //    //sb.Append(@"select sysno as attribute2optionsysno,attribute2optionname from category_attribute2_option where attribute2sysno='"+ dr["attribute2sysno"].ToString().Trim() +"' and status='0' order by ordernum;");
                //    sb.Append("select category_attribute2_option.sysno as attribute2optionsysno,category_attribute2_option.attribute2optionname,count(category_attribute2_option.sysno) as itemCount,category_attribute2_option.ordernum from category_attribute2_option(nolock),product(nolock),product_attribute2(nolock),product_price(nolock) where product.sysno=product_price.productsysno ");
                //    sb.Append(this.onlineShowLimit);
                //    if (isSecondhand)
                //    {
                //        sb.Append(" and product.producttype=1");
                //    }
                //    else
                //    {
                //        //sb.Append(" and product.productid not like '%R%' and product.productid not like '%B%' ");
                //        sb.Append(" and product.producttype<>1 and product.producttype<>2 "); //normal product
                //    }
                //    sb.Append(@" and category_attribute2_option.attribute2sysno='" + dr["attribute2sysno"].ToString().Trim() + "' and category_attribute2_option.sysno=product_attribute2.attribute2optionsysno and product.sysno=product_attribute2.productsysno " + sExists + " group by category_attribute2_option.sysno,category_attribute2_option.attribute2optionname,category_attribute2_option.ordernum order by category_attribute2_option.ordernum;");
                //}
                //foreach (DataRow dr in ds.Tables[0].Rows)
                //{
                //    //sb.Append(@"select sysno as attribute2optionsysno,attribute2optionname from category_attribute2_option where attribute2sysno='"+ dr["attribute2sysno"].ToString().Trim() +"' and status='0' order by ordernum;");
                //    sb.Append("select distinct category_attribute2_option.sysno as attribute2optionsysno,category_attribute2_option.attribute2optionname,'0' as itemCount,category_attribute2_option.ordernum from category_attribute2_option(nolock),product(nolock),product_attribute2(nolock),product_price(nolock) where product.sysno=product_price.productsysno ");
                //    sb.Append(this.onlineShowLimit);
                //    if (isSecondhand)
                //    {
                //        sb.Append(" and product.producttype=1");
                //    }
                //    else
                //    {
                //        sb.Append(" and product.producttype<>1 and product.producttype<>2 "); //normal product
                //    }
                //    sb.Append(@" and category_attribute2_option.attribute2sysno='" + dr["attribute2sysno"].ToString().Trim() + "' and category_attribute2_option.sysno=product_attribute2.attribute2optionsysno and product.sysno=product_attribute2.productsysno " + sExists + "  order by category_attribute2_option.ordernum;");
                //}

                //sql = sb.ToString().Substring(0, sb.Length - 1);

                string a2sysnoList = "";
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    a2sysnoList += Util.TrimNull(dr["attribute2sysno"]) + ",";
                }
                a2sysnoList = a2sysnoList.Substring(0, a2sysnoList.Length - 1);

                sb.Remove(0, sb.Length);

                //sb.Append("select category_attribute1.ordernum,category_attribute2.ordernum,category_attribute2.sysno as attribute2sysno, category_attribute2_option.sysno as attribute2optionsysno,category_attribute2_option.attribute2optionname,count(category_attribute2_option.sysno) as itemCount,category_attribute2_option.ordernum");
                //sb.Append(" from category_attribute1(nolock),category_attribute2(nolock), category_attribute2_option(nolock),product(nolock),product_attribute2(nolock),product_price(nolock) ");
                //sb.Append(" where category_attribute1.sysno=category_attribute2.a1sysno and category_attribute2.sysno=category_attribute2_option.attribute2sysno and product.sysno=product_price.productsysno ");
                sb.Append("select category_attribute1.ordernum,category_attribute2.ordernum,category_attribute2.sysno as attribute2sysno, category_attribute2_option.sysno as attribute2optionsysno,category_attribute2_option.attribute2optionname,count(category_attribute2_option.sysno) as itemCount,category_attribute2_option.ordernum");
                sb.Append(" from category_attribute1(nolock),category_attribute2(nolock), category_attribute2_option(nolock),product(nolock),product_attribute2(nolock) ");
                sb.Append(" where category_attribute1.sysno=category_attribute2.a1sysno and category_attribute2.sysno=category_attribute2_option.attribute2sysno ");
                sb.Append(" and Product.Status = " + ((int)AppEnum.ProductStatus.Show).ToString());
                //sb.Append(this.onlineShowLimit);
                if (isSecondhand)
                {
                    sb.Append(" and product.producttype=1");
                }
                else
                {
                    sb.Append(" and product.producttype<>1 and product.producttype<>2 "); //normal product
                }
                sb.Append(@" and category_attribute2_option.attribute2sysno in (" + a2sysnoList + ") and category_attribute2_option.sysno=product_attribute2.attribute2optionsysno and product.sysno=product_attribute2.productsysno " + sExists);
                sb.Append(" group by category_attribute1.ordernum,category_attribute2.ordernum,category_attribute2.sysno,category_attribute2_option.sysno,category_attribute2_option.attribute2optionname,category_attribute2_option.ordernum");
                sb.Append(" order by category_attribute1.ordernum,category_attribute2.ordernum,category_attribute2_option.ordernum;");

                sql = sql.Replace("product_attribute2", viewName);

                DataSet ds2 = SqlHelper.ExecuteDataSet(sb.ToString());

                int rowIndex = 0;
                int rowCount = ds2.Tables[0].Rows.Count;
                int attribute2sysno = 0;
                DataTable dt = new DataTable();
                foreach (DataRow dr in ds2.Tables[0].Rows)
                {
                    if (rowIndex < rowCount)
                    {
                        if (Util.TrimIntNull(dr["attribute2sysno"]) != attribute2sysno)
                        {
                            dt = new DataTable("tbl" + dr["attribute2sysno"]);
                            dt.Columns.Add("attribute2optionsysno");
                            dt.Columns.Add("attribute2optionname");
                            dt.Columns.Add("itemCount");
                            dt.Columns.Add("ordernum");
                        }
                        DataRow drTemp = dt.NewRow();
                        drTemp["attribute2optionsysno"] = dr["attribute2optionsysno"];
                        drTemp["attribute2optionname"] = dr["attribute2optionname"];
                        drTemp["itemCount"] = dr["itemCount"];
                        dt.Rows.Add(drTemp);

                        attribute2sysno = Util.TrimIntNull(dr["attribute2sysno"]);

                        if (rowIndex < rowCount - 1)
                        {
                            if (Util.TrimIntNull(ds2.Tables[0].Rows[rowIndex + 1]["attribute2sysno"]) != Util.TrimIntNull(ds2.Tables[0].Rows[rowIndex]["attribute2sysno"]))
                            {
                                ds.Tables.Add(dt);
                            }
                        }
                        else if (rowIndex == rowCount - 1)
                        {
                            ds.Tables.Add(dt);
                        }
                    }
                    rowIndex++;
                }

                return ds;
            }
            else
            {
                return null;
            }
        }

        //Category3 search function
        public string Get3rdCategoryWithPicSearch(int c3Sysno, int manufacturerSysNo, bool isSecondhand, int orderby, int currentPage, string[] arrayFilter, Hashtable htFilter)
        {
            string sql = @"select	product.sysno, productid, left(productid,10)as picid, productname, 
								left(productdesc,150) as productdescription, 
								availableqty+virtualqty as onlineqty,
								product_price.* 
							from
								product(nolock), inventory(nolock), product_price(nolock)
							where
								product.sysno = inventory.productsysno 
							and product.sysno = product_price.productsysno 
							@onlineShowLimit
							and product.C3SysNo = @c3sysno";

            if (arrayFilter != null && arrayFilter.GetLength(0) > 0)
            {
                for (int i = 0; i < arrayFilter.Length; i++)
                {
                    if (arrayFilter[i] != null && arrayFilter[i].Length > 0 && arrayFilter[i].IndexOf(";") > 0)
                    {
                        string[] arTemp = arrayFilter[i].Split(';');
                        sql += " and exists(select productsysno from product_attribute2(nolock) where product_attribute2.attribute2optionsysno='" + arTemp[1].Trim() + "' and product_attribute2.productsysno=product.sysno)";
                    }
                }
            }

            if (htFilter != null && htFilter.Count > 0)
            {
                foreach (string key in htFilter.Keys)
                {
                    sql += " and exists(select productsysno from product_attribute2(nolock) where product_attribute2.attribute2optionsysno='" + key + "' and product_attribute2.productsysno=product.sysno)";
                }
            }

            sql = sql.Replace("@onlineShowLimit", this.onlineShowLimit);
            sql = sql.Replace("@c3sysno", c3Sysno.ToString());

            if (manufacturerSysNo != AppConst.IntNull)
                sql += " and product.manufacturerSysNo = " + manufacturerSysNo;

            if (isSecondhand)
            {
                sql += " and product.producttype=1";
            }
            else
            {
                //sql += " and productid not like '%R%' and productid not like '%B%' ";
                sql += " and product.producttype<>1 and product.producttype<>2 ";  //normal product
            }

            if (orderby == 0)
            {
                sql += " order by productname";
            }
            else if (orderby == 1)
            {
                sql += " order by onlineqty desc";
            }
            else if (orderby == 2)
            {
                sql += " order by currentprice";
            }
            else if (orderby == 3)
            {
                sql += " order by currentprice desc";
            }
            else if (orderby == 4)
            {
                sql += " order by manufacturersysno";
            }
            else
            {
                sql += " order by product.sysno desc";
            }

            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return "";

            string seperator = @"
						<table width='100%' border='0' align='center' cellpadding='0' cellspacing='0'>
							 <tr>
  								<TD width='100%' bgcolor='#999999' heigth='1' colspan='2'><img src='../Image/spacer.gif'></td>
							 </tr>
						</table>";

            int pageSize = 10;
            int totalPage = ds.Tables[0].Rows.Count / pageSize;
            if (ds.Tables[0].Rows.Count % pageSize != 0)
                totalPage += 1;

            if (currentPage > totalPage)
                currentPage = 1;

            StringBuilder sb = new StringBuilder(2000);
            //sb.Append(seperator);
            int rowindex = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (rowindex >= (currentPage - 1) * pageSize && rowindex < currentPage * pageSize)
                {
                    sb.Append(Get3rdCategoryWithPicMold(dr, AppConst.StringNull));
                    sb.Append(seperator);
                }
                rowindex++;
            }
            return sb.ToString();
        }

        public string Get3rdCategoryWithPicSearchNew(int c3Sysno, int manufacturerSysNo, bool isSecondhand, int orderby, int currentPage, string[] arrayFilter, Hashtable htFilter)
        {
            string sql = @"select product.sysno, productid, left(productid,10)as picid, productname,PromotionWord,VirtualArriveTime,
								--left(productdesc,150) as productdescription, 
                                masterproductsysno,
								availableqty+virtualqty as onlineqty,availableqty,
								product_price.currentprice,product_price.point,product_price.cashrebate,product_price.discount --product_price.* 
                                --case when product.createtime >= DATEADD(day, -5, getdate()) then '9999999' else product.avgdailyclick end as avgdailyclick
							from
								product(nolock), inventory(nolock), product_price(nolock)
							where
								product.sysno = inventory.productsysno 
							and product.sysno = product_price.productsysno 
							@onlineShowLimit
							and product.C3SysNo = @c3sysno";

            if (arrayFilter != null && arrayFilter.GetLength(0) > 0)
            {
                for (int i = 0; i < arrayFilter.Length; i++)
                {
                    if (arrayFilter[i] != null && arrayFilter[i].Length > 0 && arrayFilter[i].IndexOf(";") > 0)
                    {
                        string[] arTemp = arrayFilter[i].Split(';');
                        sql += " and exists(select productsysno from product_attribute2(nolock) where product_attribute2.attribute2optionsysno='" + arTemp[1].Trim() + "' and product_attribute2.productsysno=product.sysno)";
                    }
                }
            }

            if (htFilter != null && htFilter.Count > 0)
            {
                foreach (string key in htFilter.Keys)
                {
                    sql += " and exists(select productsysno from product_attribute2(nolock) where product_attribute2.attribute2optionsysno='" + key + "' and product_attribute2.productsysno=product.sysno)";
                }
            }

            sql = sql.Replace("@onlineShowLimit", this.onlineShowLimit);
            sql = sql.Replace("@c3sysno", c3Sysno.ToString());

            if (manufacturerSysNo != AppConst.IntNull)
                sql += " and product.manufacturerSysNo = " + manufacturerSysNo;

            if (isSecondhand)
            {
                sql += " and product.producttype=1";
            }
            else
            {
                sql += " and product.producttype<>1 and product.producttype<>2 ";
            }

            if (orderby == 0)
            {
                sql += " order by productname";
            }
            else if (orderby == 1)
            {
                sql += " order by onlineqty desc";
            }
            else if (orderby == 2)
            {
                sql += " order by currentprice";
            }
            else if (orderby == 3)
            {
                sql += " order by currentprice desc";
            }
            else if (orderby == 4)
            {
                sql += " order by manufacturersysno";
            }
            else
            {
                sql += " order by product.ordernum,avgdailyclick desc, product.sysno desc";
            }

            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return "";

            Category3Info c3Info = CategoryManager.GetInstance().GetC3Hash()[c3Sysno] as Category3Info;
            if (c3Info.C3Type == 1)
            {
                Hashtable ht = new Hashtable(50);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    int productsysno = Util.TrimIntNull(ds.Tables[0].Rows[i]["sysno"]);
                    int masterproductsysno = Util.TrimIntNull(ds.Tables[0].Rows[i]["masterproductsysno"]);
                    if (masterproductsysno == 0 || masterproductsysno == AppConst.IntNull)
                    {
                        masterproductsysno = productsysno;
                    }

                    if (Util.TrimIntNull(ds.Tables[0].Rows[i]["onlineqty"]) > 0)  //可订购
                    {
                        if (!ht.ContainsKey(masterproductsysno))
                            ht.Add(masterproductsysno, null);
                        else
                        {
                            ds.Tables[0].Rows.RemoveAt(i);
                            i--;
                        }
                    }
                    else
                    {
                        ds.Tables[0].Rows.RemoveAt(i);
                        i--;
                    }
                }
            }

            int pageSize = 10;
            int totalRowCount = ds.Tables[0].Rows.Count;
            int totalPage = totalRowCount / pageSize;
            if (ds.Tables[0].Rows.Count % pageSize != 0)
                totalPage += 1;

            if (currentPage > totalPage)
                currentPage = 1;
            int rowindex = 0;
            StringBuilder sb = new StringBuilder(2000);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (rowindex >= (currentPage - 1) * pageSize && rowindex < currentPage * pageSize)
                {
                    sb.Append("<div class=fl_page_li>");
                    sb.Append("<table width='100%' border=0 cellspacing=0 cellpadding=0>");
                    sb.Append("<tr>");
                    sb.Append("<td class=flpl_ce><INPUT name='chkCompare" + Util.TrimNull(dr["SysNo"]) + "' type='checkbox' value='" + Util.TrimNull(dr["SysNo"]) + "' /></td>");
                    sb.Append("<td class=flpl_img><a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + dr["SysNo"].ToString() + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr["ProductID"]))).Append("' alt='查看大图' width='80' height='60' border='0'></a>").Append("</td>");
                    sb.Append("<td class=flpl_txt><a href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(dr["sysno"].ToString()) + "' target='_blank'>" + Util.TrimNull(dr["productname"]) + "<font color=red>" + Util.TrimNull(dr["PromotionWord"]) + "</font>" + "</a>");
                    //<div>人气指数：8<br />
                    //客户评价：<img src="images/site/main/center/star1.gif" /><img src="images/site/main/center/star1.gif" /><img src="images/site/main/center/star1.gif" /><img src="images/site/main/center/star1.gif" /><br />
                    if (Util.TrimIntNull(dr["point"].ToString()) > 0)
                    {
                        sb.Append("<div>赠送积分:" + Util.TrimIntNull(dr["point"].ToString()) + "</strong></div>");
                    }
                    sb.Append("</td>");
                    sb.Append("<td class=flpl_price>价格:<span class=color_f60>" + Util.ToMoney((decimal)dr["currentprice"] + (decimal)dr["cashrebate"]).ToString(AppConst.DecimalFormatWithCurrency) + "</span>");

                    if (Util.TrimIntNull(dr["availableqty"]) > 0)
                        sb.Append("<h4>1日内发货</h4>");
                    else if (Util.TrimIntNull(dr["availableqty"]) <= 0 && Util.TrimIntNull(dr["OnlineQty"]) > 0)
                    {
                        if (Util.TrimIntNull(dr["VirtualArriveTime"]) != AppConst.IntNull)
                        {
                            sb.Append("<h4>" + AppEnum.GetVirtualArriveTime(Util.TrimIntNull(dr["VirtualArriveTime"])) + "发货</h4>");
                        }
                        else
                        {
                            sb.Append("<h4>" + AppEnum.GetVirtualArriveTime((int)AppEnum.VirtualArriveTime.OneToThree) + "发货</h4>");
                        }
                    }
                    else
                        sb.Append("<h4>暂缺货</h4>");
                    sb.Append("</td>");
                    sb.Append("<td class=flpl_btn>");
                    if (Util.TrimIntNull(dr["OnlineQty"]) > 0)
                    {
                        if (c3Info.C3Type == 1) //鞋类商品，连接到商品详细页面
                        {
                            sb.Append("<a href='../Items/ItemDetail.aspx?ItemID=" + dr["SysNo"].ToString() + "' target='_blank'><img src='../images/site/main/center/btn_add2cart.png' border='0'></a>");
                        }
                        else
                        {
                            sb.Append("<a href=\"javascript:AddToCart('" + dr["SysNo"].ToString() + "')\"><img src='../images/site/main/center/btn_add2cart.png' border='0'></a>");
                        }
                    }
                    else
                        sb.Append("<a href='../Account/ItemArrivedTellMe.aspx?ItemID=").Append(dr["sysno"].ToString()).Append("'><img src='../images/site/main/center/btn_quehuo.png' border='0'></a>");
                    sb.Append("</td>");

                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("</div>");
                }
                rowindex++;
            }

            return sb.ToString();
        }

        public string GetC3SecondHandListString(int c3sysno)
        {
            string sql = @"select top 5 p.sysno,p.productname,pp.currentprice 
                           from product p(nolock) inner join product_price pp(nolock) on p.sysno=pp.productsysno 
                           inner join inventory inv(nolock) on inv.productsysno=p.sysno 
                           where p.c3sysno = @c3sysno and p.producttype=@producttype and p.status=@productstatus and (inv.AvailableQty+inv.VirtualQty > 0) 
                           order by p.sysno desc";
            sql = sql.Replace("@c3sysno", c3sysno.ToString());
            sql = sql.Replace("@producttype", Convert.ToString((int)AppEnum.ProductType.SecondHand));
            sql = sql.Replace("@productstatus", Convert.ToString((int)AppEnum.ProductStatus.Show));

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return "";

            StringBuilder sb = new StringBuilder();
            sb.Append("<div id=fl_cjwt class=panel>");
            sb.Append("<div class=panel_title>");
            sb.Append("<div class=panel_more><a href='#'><img src='../images/site/main/left/more.png' /></a></div>");
            sb.Append("<img src='../images/site/main/left/tt_espm.png' alt='二手拍卖' /></div>");
            sb.Append("<div class=panel_content>");
            sb.Append("<div class=c_espm>");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sb.Append("<div><a href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimNull(dr["sysno"]) + "' target='_blank'>" + Util.TrimNull(dr["productname"]) + "</a> <br /><span>拍卖价 " + Util.ToMoney(dr["currentprice"].ToString()) + "元</span></div>");
            }
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");

            return sb.ToString();
        }

        public string GetC2SecondHandListString(int c2sysno)
        {
            string sql = @"select top 5 p.sysno,p.productname,p.PromotionWord,pp.currentprice from product p(nolock) inner join product_price pp(nolock) on p.sysno=pp.productsysno 
                           inner join category3 c3(nolock) on p.c3sysno=c3.sysno inner join category2 c2 on c3.c2sysno = c2.sysno 
                           where c2.sysno = @c2sysno and p.producttype=@producttype and p.status=@productstatus
                           order by p.sysno desc";
            sql = sql.Replace("@c2sysno", c2sysno.ToString());
            sql = sql.Replace("@producttype", Convert.ToString((int)AppEnum.ProductType.SecondHand));
            sql = sql.Replace("@productstatus", Convert.ToString((int)AppEnum.ProductStatus.Show));

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return "";

            StringBuilder sb = new StringBuilder();
            sb.Append("<div id=fl_cjwt class=panel>");
            sb.Append("<div class=panel_title>");
            sb.Append("<div class=panel_more><a href='#'><img src='../images/site/main/left/more.png' /></a></div>");
            sb.Append("<img src='../images/site/main/left/tt_espm.png' alt='二手拍卖' /></div>");
            sb.Append("<div class=panel_content>");
            sb.Append("<div class=c_espm>");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sb.Append("<div><a href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimNull(dr["sysno"]) + "' target='_blank'>" + Util.TrimNull(dr["productname"]) + "<font color=red>" + Util.TrimNull(dr["PromotionWord"]) + "</font>" + "</a> <br /><span>拍卖价 " + Util.ToMoney(dr["currentprice"].ToString()) + "元</span></div>");
            }
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");

            return sb.ToString();
        }

        public string Get3rdCategoryWithPicMold(DataRow dr, string manuID)
        {
            StringBuilder sb = new StringBuilder(2000);

            sb.Append("<table width='100%' border='0' cellpadding='5' cellspacing='0' align=center>");
            sb.Append("	<tr class='m-1'> ");
            sb.Append("		<td width='130' valign='middle' align='center'>");
            if (manuID != AppConst.StringNull)
                sb.Append("<img src='../images/Items/logo/" + manuID + ".gif' border=0><br>");
            sb.Append("			<a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + dr["SysNo"].ToString() + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr["ProductID"]))).Append("' alt='点击查看更多图片' width='80' height='60' border='0'></a></td>");
            sb.Append("		<td  valign='top'>");
            if (Util.TrimIntNull(dr["OnlineQty"]) < 1)
                sb.Append("<img src='../images/Items/warning.gif' border=0>");
            sb.Append("			<span class='m-2'><strong><a href='../Items/ItemDetail.aspx?ItemID=").Append(dr["SysNo"].ToString()).Append("' target='_blank' title='查看更多信息'>").Append(dr["productname"].ToString() + "<font color=red>" + Util.TrimNull(dr["PromotionWord"]) + "</font>").Append("</a></strong></span>");
            if (Util.TrimIntNull(dr["OnlineQty"]) < 1)
                sb.Append("<FONT color='#ff0000'>暂缺货</font>");
            sb.Append("			<br><span class='s-1'>").Append(dr["productdescription"].ToString()).Append("... ...</span>");
            sb.Append("		</td>");
            sb.Append("		<td width=150 bgcolor='#F7F7F7' valign='top' rowspan=2><a href='ItemDetail.aspx?ItemID=" + dr["SysNo"].ToString() + "'><img src='../Image/but01-learnMore.gif' border='0' width='146'class='' height='20'></a>");
            if (Util.TrimIntNull(dr["OnlineQty"]) > 0)
                sb.Append("						<a href=\"javascript:AddToCart('" + dr["SysNo"].ToString() + "')\"><img src='../images/Items/carticon.gif' width='146' height='20' border='0'></a>");
            else
                sb.Append("						<a href='../Account/ItemArrivedTellMe.aspx?ItemID=").Append(dr["sysno"].ToString()).Append(" '><img src='../images/notice.gif'  height='20' border='0' border='0'></a>");


            sb.Append("	</td></tr>");
            sb.Append("	<tr> ");
            sb.Append("		<td valign='top' align=center>");
            //<br>&nbsp;<input type='checkbox' name='chk").Append(dr["sysno"].ToString()).Append("'>&nbsp;&nbsp;选取比较
            //if( manuID == AppConst.StringNull )
            //sb.Append("		<input type='checkbox' name='chk").Append(dr["sysno"].ToString()).Append("'></td>");
            //else
            //sb.Append("		&nbsp;");
            sb.Append("		<td>");
            sb.Append("			<table width='100%' border='0' cellspacing='0' cellpadding='0'>");
            sb.Append("				<tr> ");
            sb.Append("					<td width='44%'>");

            string pricestring = GetPrice(dr, true);
            sb.Append(pricestring);

            sb.Append("					</td>");
            //sb.Append("					<td ");
            //sb.Append("						<span class='font9'><img src='../images/arr2.gif' width='10' height='10'><a href='ItemDetail.aspx?ItemID="+dr["SysNo"].ToString()+"'>更多信息&gt;&gt;</a></span>");
            //sb.Append("						<br><span class='font9'><img src='../images/arr2.gif' width='10' height='10'><a href='ItemReview.aspx?ItemID="+dr["SysNo"].ToString()+"'>查看评论&gt;&gt;</a></span>");
            //sb.Append("					</td>");
            //sb.Append("					<td >");
            //if ( Util.TrimIntNull(dr["OnlineQty"])>0)
            //	sb.Append("						<a href=\"javascript:AddToCart('"+dr["SysNo"].ToString()+"')\"><img src='../images/Items/AddToCart.gif' width='89' height='25' border='0'></a>");
            //else
            //	sb.Append("						<a href='../Account/ItemArrivedTellMe.aspx?ItemID=").Append(dr["sysno"].ToString()).Append(" '><img src='../images/callme.gif' width='89' height='25' border='0'></a></td>");

            //sb.Append("				</td></tr>");
            sb.Append("			</table>");

            sb.Append("		</td>");

            sb.Append("	</tr>");
            sb.Append("</table>");

            return sb.ToString();
        }
        #endregion

        #region top sale for CGA
        public string GetTopSaleForCGA()
        {
            //			<table width="100%" border="0" cellspacing="0" cellpadding="0">
            //				<tr bgcolor="#FFF8AD">
            //					<td width="12%" height="21"><div align="center" class="red">01</div>
            //					</td>
            //					<td width="88%" height="21"><a href="#">罗技 极速战斧 游戏手柄（用于PS机）</a></td>
            //				</tr>
            //				<tr>
            //					<td height="21"><div align="center" class="red">02</div>
            //					</td>
            //					<td height="21"><a href="#">罗技 极速战斧 游戏手柄（用于PS机）</a></td>
            //				</tr>
            //				
            //			</table>
            Hashtable ht = new Hashtable(4);
            ht.Add("ListArea", (int)AppEnum.OnlineListArea.Newcome);
            //			ht.Add("OrderBy", " newid()");

            DataSet ds = this.GetOnlineListDs(ht, 8, true, true);
            if (!Util.HasMoreRow(ds))
                return "";

            StringBuilder sb = new StringBuilder(2000);
            sb.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0'>");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string Num = "";
                if (i != 9)
                {
                    Num = "0" + Convert.ToString(i + 1);
                }
                else
                {
                    Num = "10";
                }

                if (i % 2 == 0)
                {
                    sb.Append("<tr bgcolor='#FFF8AD'>");
                }
                else
                { }

                sb.Append("<td width='12%' height='21'><div align='center' class='red'>" + Num.ToString() + "</div>");
                sb.Append("</td>");
                sb.Append("<td width='88%' height='21'><a href='Items/ItemDetail.aspx?ItemID=" + ds.Tables[0].Rows[i]["SysNo"].ToString() + "'>" + ds.Tables[0].Rows[i]["ProductName"].ToString() + "</a></td>");
                sb.Append("</tr>");

            }

            sb.Append("</table>");

            return sb.ToString();
        }
        public string GetSPForCGA()
        {
            //int index=0;
            Hashtable ht = new Hashtable(5);
            ht.Add("ListArea", (int)AppEnum.OnlineListArea.DefaultTop2);
            ht.Add("OrderBy", "newid()");

            DataSet ds = this.GetOnlineListDs(ht, 2, true, true);

            if (!Util.HasMoreRow(ds))
                return "";

            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sb.Append("<table width='100%' border='0' cellspacing='0' cellpadding='5' Height='100%'>");
                sb.Append(" 	<tr>");
                sb.Append("			<td width='39%'>");
                sb.Append("				<table width='100' border='0' cellpadding='0' cellspacing='1'>");
                sb.Append("<tr>");
                sb.Append("						<td bgcolor='#ffffff'><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr["ProductID"]))).Append("' alt='' width='80' height='60' border='0'></td>");
                sb.Append("					</tr>");
                sb.Append("				</table>");
                sb.Append("			</td>");
                sb.Append("			<td width='61%' valign='top'><a href='Items/ItemDetail.aspx?ItemID=" + dr["SysNo"].ToString() + "' class='black'>" + dr["ProductName"].ToString() + "</a>");
                sb.Append("			<br>");
                //				sb.Append("			原价：" + "" + "元");
                //				sb.Append("			<br>");
                //				sb.Append("			特价：<font color='#ff0000' size='4'><strong>" + "" +  "86.00</strong></font>元<br>");
                sb.Append(this.GetPriceOnSaleForCGA(dr));
                sb.Append("			<br>");
                sb.Append("				<a href=\"javascript:AddToCartForCGA('" + dr["SysNo"].ToString() + "')\"><img src='images/button_2.gif'  border='0'></a>");
                sb.Append("         </td>");
                sb.Append("		</tr>");
                sb.Append("</table>");
                //sb.Append("<table width='95%' border='0' cellspacing='0' cellpadding='0'>");
                //sb.Append("		<tr>");
                //sb.Append("			<td background='images/line_3.gif'><img name='' src='' width='0' height='0' alt='></td>");
                //sb.Append("		</tr>");
                //sb.Append("</table>");
            }
            return sb.ToString();
        }

        #endregion

        #region new product
        private string GetNewcomeMold(DataRow dr)
        {
            StringBuilder sb = new StringBuilder(500);

            sb.Append("<table width='96%' border='0' align='center' cellpadding='0' cellspacing='0'>");
            sb.Append("	<tr>");
            sb.Append("		<td>");
            sb.Append("			<table width='100%' border='0' cellspacing='0' cellpadding='0'>");
            sb.Append("				<tr>");
            sb.Append("					<td width='26%'><a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + dr["ProductSysNo"].ToString() + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr["ProductID"]))).Append("' alt='点击查看更多图片' width='80' height='60' border='0'></a></td>");
            sb.Append("                 <td width='74%'><a href='../Items/ItemDetail.aspx?ItemID=").Append(dr["SysNo"].ToString()).Append("' target='_blank'><strong>").Append(dr["ProductName"].ToString()).Append("</strong></a></td>");
            sb.Append("             </tr>");
            sb.Append("         </table>");
            sb.Append("		</td>");
            sb.Append(" </tr>");
            sb.Append(" <tr>");
            sb.Append("		<td>");

            sb.Append(GetPrice(dr, false));

            sb.Append("		</td>");
            sb.Append(" </tr>");
            sb.Append("</table>");

            return sb.ToString();
        }


        public string GetNewcomeList()
        {
            Hashtable ht = new Hashtable(4);
            ht.Add("ListArea", (int)AppEnum.OnlineListArea.Newcome);
            ht.Add("OrderBy", " newid()");



            DataSet ds = this.GetOnlineListDs(ht, AppConst.IntNull, true, true);
            if (!Util.HasMoreRow(ds))
                return "";

            StringBuilder sb = new StringBuilder(2000);

            string seperator = @"
						<table width='95%' border='0' align='center' cellpadding='0' cellspacing='0'>
							<tr> 
								<td height='5'><img src='../images/dot_line02.gif' width='1' height='1'></td>
							</tr>
							<tr> 
								<td height='1' background='../images/dot_line02.gif'><img src='../images/dot_line02.gif' width='148' height='1'></td>
							</tr>
							<tr> 
								<td height='5'><img src='../images/dot_line02.gif' width='1' height='1'></td>
							</tr>
						</table>";

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sb.Append(GetNewcomeMold(dr));
                sb.Append(seperator);
            }

            return sb.ToString();

        }


        #endregion

        #region 5460 list
        private string Get5460Mold(DataRow dr)
        {
            StringBuilder sb = new StringBuilder(500);
            string PName;
            PName = dr["ProductName"].ToString();
            if (dr["ProductName"].ToString().Length > 40)
                PName = dr["ProductName"].ToString().Substring(0, 30);

            sb.Append("<td valign=top>");
            sb.Append("<table width='150' border='0' align='center' cellpadding='0' cellspacing='0'>");
            sb.Append("	<tr>");
            sb.Append("		<td>");
            sb.Append("			<table width='100%' border='0' cellspacing='0' cellpadding='0'>");
            sb.Append("				<tr>");
            //sb.Append("					<td width='26%'><a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?linksource=5460a&ItemID="+dr["ProductSysNo"].ToString()+"')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr["ProductID"]))).Append("' alt='点击查看更多图片' width='80' height='60' border='0'></a></td>");
            //sb.Append("					<td width='26%'><a href='../Items/ItemDetail.aspx?linksource=5460a&ItemID=").Append(dr["SysNo"].ToString()).Append("' target='_blank'>").Append("<img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr["ProductID"]))).Append("' alt='点击查看' width='80' height='60' border='0'></a></td>");
            //sb.Append("                 <td width='74%'><a href='../Items/ItemDetail.aspx?linksource=5460a&ItemID=").Append(dr["SysNo"].ToString()).Append("' target='_blank'><strong>").Append(dr["ProductName"].ToString()).Append("</strong></a></td>");
            sb.Append("					<td '><a href='../Items/ItemDetail.aspx?linksource=5460a&ItemID=").Append(dr["SysNo"].ToString()).Append("' target='_blank'>").Append("<img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr["ProductID"]))).Append("' alt='点击查看' width='80' height='60' border='0'></a><br><a href='../Items/ItemDetail.aspx?linksource=5460a&ItemID=").Append(dr["SysNo"].ToString()).Append("' target='_blank'>").Append(PName).Append("</a></td>");
            //sb.Append("                 <td '><a href='../Items/ItemDetail.aspx?linksource=5460a&ItemID=").Append(dr["SysNo"].ToString()).Append("' target='_blank'><strong>").Append(dr["ProductName"].ToString()).Append("</strong></a></td>");
            sb.Append("             </tr>");



            sb.Append("         </table>");
            sb.Append("		</td>");
            sb.Append(" </tr>");
            sb.Append(" <tr>");
            sb.Append("		<td>");

            sb.Append(GetPrice(dr, false));

            sb.Append("		</td>");
            sb.Append(" </tr>");
            sb.Append("</table>");

            sb.Append("</td>");
            return sb.ToString();
        }


        public string Get5460List()
        {
            Hashtable ht = new Hashtable(4);
            ht.Add("ListArea", (int)AppEnum.OnlineListArea.MovSto);
            ht.Add("OrderBy", " newid()");

            DataSet ds = this.GetOnlineListDs(ht, AppConst.IntNull, true, true);
            if (!Util.HasMoreRow(ds))
                return "";

            StringBuilder sb = new StringBuilder(2000);

            //string seperator = @"
            //			<table width='95%' border='0' align='center' cellpadding='0' cellspacing='0'>
            //				<tr> 
            //					<td height='5'><img src='../images/dot_line02.gif' width='1' height='1'></td>
            //				</tr>
            //				<tr> 
            //					<td height='1' background='../images/dot_line02.gif'><img src='../images/dot_line02.gif' width='148' height='1'></td>
            //				</tr>
            //				<tr> 
            //					<td height='5'><img src='../images/dot_line02.gif' width='1' height='1'></td>
            //				</tr>
            //			</table>";

            sb.Append("<table width='96%' border='0' align='center' cellpadding='0' cellspacing='0'>");
            sb.Append("	<tr>");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sb.Append(Get5460Mold(dr));
                //sb.Append(seperator);
            }

            sb.Append(" </tr>");
            sb.Append("</table>");

            return sb.ToString();
        }
        #endregion

        #region top 10
        public string GetCategoryTopSale(int listArea)
        {
            Hashtable ht = new Hashtable(5);
            ht.Add("ListArea", listArea);
            ht.Add("OrderBy", "OnlineList.SysNo desc");

            DataSet ds = this.GetOnlineListDs(ht, 10, true, false);

            if (!Util.HasMoreRow(ds))
                return "";

            StringBuilder sb = new StringBuilder(500);
            int index = 1;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sb.Append("<tr>");
                sb.Append("<td width=12 valign=top>").Append(index).Append(".");
                sb.Append("<td><a href='../Items/ItemDetail.aspx?ItemID=").Append(Util.TrimIntNull(dr["sysno"]).ToString()).Append("' target='_blank'><font style='FONT-SIZE: 8pt;'>").Append(Util.TrimNull(dr["ProductName"])).Append("</font></a><br>");
                index++;
            }

            string str1 = @"
				<table width='188' cellpadding='0' cellspacing='0' border='0'>
					<tr>
						<td background='img/DOT_BG.gif'><img src='../images/top10.gif''></td>
					</tr>
					<tr>
						<td style='WIDTH: 100%' class='channel_tdbg'>
							<table   cellSpacing='0'cellPadding='0' width='100%' border='0'>
							<tr><td width=100%>
								<table width=100% cellspacing=0 cellpadding=0 class='tbl'>
							";

            string str2 = @"
								</td></tr></table>
								</table>
						</td>
					</tr>
				</table>";

            return str1 + sb.ToString() + str2;
        }

        #endregion

        public string GetCategoryProduct(int areaList)
        {
            int index = 0;
            Hashtable ht = new Hashtable(5);
            ht.Add("ListArea", areaList);
            ht.Add("OrderBy", "newid()");

            DataSet ds = this.GetOnlineListDs(ht, 10, true, true);

            if (!Util.HasMoreRow(ds))
                return "";

            StringBuilder sb = new StringBuilder();
            sb.Append("<table width=100% border=0>");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sb.Append(" <tr>");
                sb.Append("  <td>");
                sb.Append(GetCategoryProductMold(dr, index));
                sb.Append("  </td>");
                sb.Append(" </tr>");
                index++;
            }
            sb.Append("</table>");
            return sb.ToString();

        }

        public string GetCategoryProductForCGA(int areaList)
        {
            //			<table width="100%" border="0" cellspacing="0" cellpadding="5">
            //				<tr>
            //					<td width="39%"><table width="100" border="0" cellpadding="0" cellspacing="1">
            //							<tr>
            //								<td bgcolor="#ffffff"><img src="images/pro_1.gif" width="101" height="92"></td>
            //							</tr>
            //						</table>
            //					</td>
            //					<td width="61%" valign="top"><a href="#" class="black">飞利浦 运动型耳机</a>
            //						<br>
            //						原价：100元
            //						<br>
            //						特价：<font color="#ff0000" size="4"><strong>86.00</strong></font>元<br>
            //						<br>
            //						<input name="imageField3322" type="image" src="images/button_2.gif" width="58" height="21"
            //							border="0"> <input name="imageField23222" type="image" src="images/button_2.gif" width="58" height="21"
            //							border="0"></td>
            //				</tr>
            //			</table>
            //			<table width="95%" border="0" cellspacing="0" cellpadding="0">
            //				<tr>
            //					<td background="images/line_3.gif"><img name="" src="" width="0" height="0" alt=""></td>
            //				</tr>
            //			</table>
            //int index=0;
            Hashtable ht = new Hashtable(5);
            ht.Add("ListArea", areaList);
            ht.Add("OrderBy", "newid()");

            DataSet ds = this.GetOnlineListDs(ht, 3, true, true);

            if (!Util.HasMoreRow(ds))
                return "";

            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sb.Append("<table width='100%' border='0' cellspacing='0' cellpadding='5' Height='150'>");
                sb.Append(" 	<tr>");
                sb.Append("			<td width='39%'>");
                sb.Append("				<table width='100' border='0' cellpadding='0' cellspacing='1'>");
                sb.Append("<tr>");
                sb.Append("						<td bgcolor='#ffffff'><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr["ProductID"]))).Append("' alt='' width='80' height='60' border='0'></td>");
                sb.Append("					</tr>");
                sb.Append("				</table>");
                sb.Append("			</td>");
                sb.Append("			<td width='61%' valign='top'><a href='Items/ItemDetail.aspx?ItemID=" + dr["SysNo"].ToString() + "' class='black'>" + dr["ProductName"].ToString() + "</a>");
                sb.Append("			<br>");
                //				sb.Append("			原价：" + "" + "元");
                //				sb.Append("			<br>");
                //				sb.Append("			特价：<font color='#ff0000' size='4'><strong>" + "" +  "86.00</strong></font>元<br>");
                sb.Append(this.GetPriceOnSaleForCGA(dr));
                sb.Append("			<br>");
                sb.Append("				<a href=\"javascript:AddToCartForCGA('" + dr["SysNo"].ToString() + "')\"><img src='images/button_2.gif'  border='0'></a>");
                sb.Append("         </td>");
                sb.Append("		</tr>");
                sb.Append("</table>");
                //				sb.Append("<table width='95%' border='0' cellspacing='0' cellpadding='0'>");
                //                sb.Append("		<tr>");
                //				sb.Append("			<td background='images/line_3.gif'><img name='' src='' width='0' height='0' alt='></td>");
                //				sb.Append("		</tr>");
                //				sb.Append("</table>");
            }

            return sb.ToString();
        }

        private string GetCategoryProductMold(DataRow dr, int index)
        {
            StringBuilder sb = new StringBuilder();
            if (index != 0)
            {
                sb.Append("<table width='99%' border='0' align='center' cellpadding='0' cellspacing='0'>");
                sb.Append("<tr>");
                sb.Append("	<TD width='100%' bgcolor='#999999' heigth='1' colspan='2'><img src='../Image/spacer.gif'></td> ");
                sb.Append("</tr>");
                //sb.Append("	<tr>");			
                //sb.Append("	 <td height='1'><img src='../images/dot_line02.gif' width='1' height='1'></td>");
                //sb.Append(" </tr>");
                //sb.Append(" <tr>");
                //sb.Append("	 <td height='1' background='../images/dot_line02.gif'><img src='../images/dot_line02.gif' width='148' height='1'></td>");
                //sb.Append(" </tr>");
                //sb.Append(" <tr>");
                //sb.Append("	 <td height='3'><img src='../images/dot_line02.gif' width='1' height='1'></td>");
                //sb.Append(" </tr>");
                sb.Append("</table>");
            }
            sb.Append("<table width='100%' border='0' cellspacing='8' cellpadding='0'>");
            sb.Append(" <tr>");
            sb.Append("  <td width='50%'>");
            sb.Append("   <table width='99%' height='100%' border='0' cellpadding='0' cellspacing='0'>");
            sb.Append("	   <tr class='m-1'>");
            sb.Append("		<td width='113' valign='middle'>");
            sb.Append("		 <a href=\"JavaScript:openDialog('DisplayPhoto.aspx?ItemID=" + dr["SysNo"].ToString() + "');\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr["ProductID"]))).Append("' alt='查看大图' width='80' height='60' border='0'></a>");//.Append("<br><font color='#999999'>").Append(dr["productid"].ToString()).Append("</font>");
            sb.Append("		</td>");
            sb.Append("		<td width='481' valign='top'>");
            sb.Append("		 <span class='m-2'><strong>");
            sb.Append("		  <a href='ItemDetail.aspx?ItemID=" + dr["SysNo"].ToString() + "' target='_blank' title='查看详细资料'>" + dr["ProductName"].ToString() + "</a>");
            sb.Append("		 </strong></span>");
            sb.Append("		 <br>");
            string ShortDes = dr["ProductDesc"].ToString().Trim();
            if (ShortDes.Length > MaxDesLen)
                ShortDes = ShortDes.Substring(0, MaxDesLen - 4) + "...";
            sb.Append("		 <span class='s-1'>" + ShortDes);
            sb.Append("		 </span>");
            sb.Append("     </td>");
            sb.Append("	   </tr>");
            sb.Append("    <tr>");
            sb.Append("     <td valign='top'>&nbsp;</td>");
            sb.Append("     <td>");
            sb.Append("		 <table width='100%' border='0' cellspacing='0' cellpadding='0'>");
            sb.Append("		  <tr>");
            sb.Append("		   <td width='50%'>");
            sb.Append("			<table width='100%' border='0' cellspacing='0' cellpadding='0'>");
            sb.Append("			 <tr>");
            sb.Append("			  <td width='50%'>");
            sb.Append(GetPrice(dr, true));
            sb.Append("			  </td>");
            sb.Append("	          <td width='26%'>");
            sb.Append("			   <!--span class='font9'><img src='../images/arr2.gif' width='10' height='10'><a href='ItemDetail.aspx?ItemID=" + dr["SysNo"].ToString() + "'>更多信息&gt;&gt;</a></span>");
            sb.Append("			   <br><span class='font9'><img src='../images/arr2.gif' width='10' height='10'><a href='ItemReview.aspx?ItemID=" + dr["SysNo"].ToString() + "'>查看评论&gt;&gt;</a></span-->");
            sb.Append("			  </td>");
            sb.Append("			  <td width='24%'>");
            if ((int)dr["OnlineQty"] > 0)
                sb.Append("	<span class='font9'><a href='ItemDetail.aspx?ItemID=" + dr["SysNo"].ToString() + "'><span class='buyfont'>阅读详细资料&nbsp;</span><img src='../Images/Items/greybubblearrow.gif'align='middle'  border=0></a></span><!--a href=\"javascript:AddToCart('" + dr["SysNo"].ToString() + "')\"><img src='../images/Items/AddToCart.gif' width='89' height='25' border='0'></a-->");
            else
                sb.Append("			   <a href='../Account/ItemArrivedTellMe.aspx?ItemID=" + dr["SysNo"].ToString() + "'><img src='../images/notify.gif' width='89' height='25' border='0'></a>");
            sb.Append("           </td>");
            sb.Append("			 </tr>");
            sb.Append("			</table>");
            sb.Append("		   </td>");
            sb.Append("		  </tr>");
            sb.Append("		 </table>");
            sb.Append("		</td>");
            sb.Append("	   </tr>");
            sb.Append("	  </table>");
            sb.Append("  </td>");
            sb.Append(" </tr>");
            sb.Append("</table>");
            return sb.ToString();
        }

        public string GetDefaultUp()
        {
            Hashtable ht = new Hashtable(5);
            ht.Add("ListArea", (int)AppEnum.OnlineListArea.DefaultUp);
            ht.Add("OrderBy", "newid()");

            DataSet ds = this.GetOnlineListDs(ht, 6, true, true);

            return GetDefaultProduct(ds);
        }

        public string GetDefaultUpForCGA()
        {
            Hashtable ht = new Hashtable(5);
            ht.Add("ListArea", (int)AppEnum.OnlineListArea.DefaultUp);
            ht.Add("OrderBy", "newid()");

            DataSet ds = this.GetOnlineListDs(ht, 4, true, true);

            return GetDefaultProductForCGA(ds);
        }
        public string GetDefaultDown()
        {
            Hashtable ht = new Hashtable(5);
            ht.Add("OrderBy", "Category1.C1ID, Category2.C2ID, Category3.C3ID");
            //DataSet dsCountdown = CountdownManager.GetInstance().GetCountdownOnlineDsDefault(ht, 36, true, true);
            //if (Util.HasMoreRow(dsCountdown))
            //{
            //    return GetDefaultProduct(dsCountdown);
            //}
            //else
            //{
            //    ht.Add("ListArea", (int)AppEnum.OnlineListArea.DefaultDown);
            //    DataSet ds = this.GetOnlineListDs(ht, 36, true, true);
            //    return GetDefaultProduct(ds);
            //}
            ht.Add("ListArea", (int)AppEnum.OnlineListArea.DefaultDown);
            DataSet ds = this.GetOnlineListDs(ht, 36, true, true);
            return GetDefaultProduct(ds);
        }

        public string GetDefaultTop2()
        {
            Hashtable ht = new Hashtable(5);
            ht.Add("ListArea", (int)AppEnum.OnlineListArea.DefaultTop2);
            ht.Add("OrderBy", "newid()");

            DataSet ds = this.GetOnlineListDs(ht, 2, true, true);

            if (!Util.HasMoreRow(ds))
                return "";
            else
            {
                string temp = GetDefaultProduct(ds);

                StringBuilder sb = new StringBuilder();
                sb.Append("<table width='96%' border='0' align='center' cellpadding='0' cellspacing='0'>");
                sb.Append(" <TR>");
                sb.Append("  <TD width='8'><IMG SRC='../images/Items/2_01.gif' WIDTH='8' HEIGHT='39' ALT=''></TD>");
                sb.Append("  <TD width='103'><IMG SRC='../images/Items/2_02.gif' WIDTH='103' HEIGHT='39' ALT=''></TD>");
                sb.Append("  <TD width='265'><IMG SRC='../images/Items/2_03.gif' WIDTH='265' HEIGHT='39' ALT=''></TD>");
                sb.Append("  <TD width='83'><img src='../images/Items/2_04.gif' width='83' height='39' alt=''></TD>");
                sb.Append("	 <TD COLSPAN='2'><IMG SRC='../images/Items/2_05.gif' WIDTH='121' HEIGHT='39' ALT=''></TD>");
                sb.Append(" </TR>");
                sb.Append("</table>");
                sb.Append("<table width='96%' border='0' align='center' cellpadding='0' cellspacing='0'>");
                sb.Append(" <tr>");
                sb.Append("  <td width='8'><IMG SRC='../images/Items/2_06.gif' WIDTH='8' HEIGHT='52' ALT=''></td>");
                sb.Append("  <td width='368'><IMG SRC='../images/Items/2_07.gif' WIDTH='368' HEIGHT='52' ALT=''></td>");
                sb.Append("  <td width='83'><IMG SRC='../images/Items/2_08.gif' WIDTH='83' HEIGHT='52' ALT=''></td>");
                sb.Append("  <td width='113'><IMG SRC='../images/Items/2_09.gif' WIDTH='113' HEIGHT='52' ALT=''></td>");
                sb.Append("	 <td background='../images/Items/2_top_bg.gif'>&nbsp;</td>");
                sb.Append("  <td width='8'><IMG SRC='../images/Items/2_10.gif' WIDTH='8' HEIGHT='52' ALT=''></td>");
                sb.Append(" </tr>");
                sb.Append("</table>");
                sb.Append("<table width='96%' border='0' align='center' cellpadding='0' cellspacing='0'>");
                sb.Append("	<tr>");
                sb.Append("	 <td width='8' height='118' background='../images/Items/1_11.gif'>&nbsp;</td>");
                sb.Append("  <td>" + temp + "</td>");
                sb.Append("  <td width='8' background='../images/Items/1_11-.gif'>&nbsp;</td>");
                sb.Append("	</tr>");
                sb.Append("</table>");
                sb.Append("<table width='96%' border='0' align='center' cellpadding='0' cellspacing='0'>");
                sb.Append(" <tr>");
                sb.Append("  <td width='8' height='8'><IMG SRC='../images/Items/1_13.gif' WIDTH='8' HEIGHT='8' ALT=''></td>");
                sb.Append("  <td background='../images/Items/1_14.gif'><IMG SRC='../images/Items/1_14.gif' WIDTH='4' HEIGHT='8' ALT=''></td>");
                sb.Append("  <td width='8' background='../images/Items/1_11-.gif'><IMG SRC='../images/Items/1_15.gif' WIDTH='8' HEIGHT='8' ALT=''></td>");
                sb.Append(" </tr>");
                sb.Append("</table>");
                return sb.ToString();
            }
        }

        private string GetDefaultProduct(DataSet ds)
        {
            if (!Util.HasMoreRow(ds))
                return "";

            int index = 0;
            StringBuilder sb = new StringBuilder(5000);


            string begin = @"<table width='100%' height='1' border='0' cellpadding='0' cellspacing='0'>
								<tr> 
									<!--td width='1' bgcolor='#ccccccc'><img src='../images/one.pix.jpg' width='1' height='1'></td-->
									<td>";

            sb.Append(begin);

            string str1 = "";
            string str2 = "";
            string str3 = "";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (index % 3 == 0)
                    str1 = GetDefaultProductMold(dr);
                if (index % 3 == 1)
                    str2 = GetDefaultProductMold(dr);
                if (index % 3 == 2)
                    str3 = GetDefaultProductMold(dr);

                if (index % 3 == 2)
                {
                    if (index != 2)
                    {
                        string seperator = @"<table width='100%' border='0' align='center' cellpadding='0' cellspacing='0'>
											<tr> 
											<td height='1' background='../Images/Default/default.dotline.gif'><img src='../Images/Default/default.dotline.gif' width='148' height='1'></td>
											</tr>
										</table>";
                        sb.Append(seperator);
                    }
                    sb.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0'>");
                    sb.Append("	<tr>");
                    sb.Append("		<td width='33%' height='130' valign='top'>").Append(str1).Append("</td>");
                    sb.Append("		<td width='1' background='../image/bj_x1.gif'><img src='../images/one.pix.jpg' width='1' height='1'></td>");
                    sb.Append("     <td width='33%' valign='top'>").Append(str2).Append("</td>");
                    sb.Append("		<td width='1' background='../image/bj_x1.gif'><img src='../images/one.pix.jpg' width='1' height='1'></td>");
                    sb.Append("     <td width='33%' valign='top'>").Append(str3).Append("</td>");
                    sb.Append(" </tr>");
                    sb.Append("</table>");

                    str1 = str2 = str3 = "";
                }
                index++;
            }
            if (str1 != "")
            {
                string seperator = @"<table width='100%' border='0' align='center' cellpadding='0' cellspacing='0'>
										<tr> 
										<td height='1' background='../Images/Default/default.dotline.gif'><img src='../Images/Default/default.dotline.gif' width='148' height='1'></td>
										</tr>
									</table>";
                sb.Append(seperator);

                sb.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0'>");
                sb.Append("	<tr>");
                sb.Append("		<td width='33% height='130' valign='top'>").Append(str1).Append("</td>");
                sb.Append("		<td width='1' background='../image/bj_x1.gif' width='1' ><img src='../images/one.pix.jpg' width='1' height='1'></td>");
                sb.Append("     <td width='33%' valign='top'>").Append(str2).Append("</td>");
                sb.Append("		<td width='1' background='../image/bj_x1.gif' width='1' ><img src='../images/one.pix.jpg' width='1' height='1'></td>");
                sb.Append("     <td width='33%' valign='top'>").Append("&nbsp;").Append("</td>");
                sb.Append(" </tr>");
                sb.Append("</table>");

            }

            string end = @"			</td>
									<!--td width='1' bgcolor='#ccccccc'><img src='../images/one.pix.jpg' width='1' height='1'></td-->
								</tr>
								<!--tr> 
									<td height='1' bgcolor='#ccccccc' colspan='3'><img src='../images/one.pix.jpg' width='1' height='1'></td>
								</tr-->
							</table>";
            sb.Append(end);

            return sb.ToString();

        }

        private string GetDefaultProductMold(DataRow dr)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table width='100%' height='100%' border='0' cellpadding='0' cellspacing='2' class='tbl' >");
            sb.Append("	<tr class='m-1'>");
            sb.Append("		<td colspan='2' valign='top'>");
            sb.Append("				<a href='../Items/ItemDetail.aspx?ItemID=").Append(dr["SysNo"].ToString()).Append("' target='_blank'>").Append(dr["ProductName"].ToString()).Append("</a>");
            sb.Append("		</td>");
            sb.Append("	</tr>");
            sb.Append("	<tr class='m-1'>");
            sb.Append("		<td width='85' rowspan='2'  valign='middle'>");
            sb.Append("			<div align='center'>");
            sb.Append("				<a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + dr["SysNo"].ToString() + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr["ProductID"]))).Append("' alt='查看大图' width='80' height='60' border='0'></a>");
            if (Util.TrimDecimalNull(dr["CurrentPrice"]) < Util.TrimDecimalNull(dr["BasicPrice"]))
                sb.Append("				");
            sb.Append("			</div>");
            sb.Append("		</td>");
            sb.Append("		<td valign='top'>");
            //sb.Append("			<div>");
            sb.Append(this.GetPriceOnSale(dr));
            //sb.Append("			<br><a href='../Items/ItemDetail.aspx?ItemID=").Append(dr["SysNo"].ToString()).Append("' target='_blank'>详细介绍...</a><br>");
            //sb.Append("			</div>");
            sb.Append("		</td>");
            sb.Append("	</tr>");
            sb.Append(" <tr>");
            sb.Append("		<td >");
            sb.Append("			<table width='100%' border='0' cellspacing='0' cellpadding='0'>");
            sb.Append("				<tr>");
            sb.Append("					<td width='50%' height='32' align='right' valign='middle'><a href='javascript:AddToCart(").Append(dr["SysNo"].ToString()).Append(")'> <span class='buyfont'>我要购买&nbsp;</span><img src='../Images/Items/greybubblearrow.gif'align='middle'  border=0></a></td>");
            sb.Append("				</tr>");
            sb.Append("			</table>");
            sb.Append("		</td>");
            sb.Append("	</tr>");
            sb.Append("</table>");

            return sb.ToString();
        }

        private string GetDefaultProductForCGA(DataSet ds)
        {
            if (!Util.HasMoreRow(ds))
                return "";

            int index = 0;
            StringBuilder sb = new StringBuilder(5000);


            string begin = @"<table width='100%' height='1' border='0' cellpadding='0' cellspacing='0'>
								<tr> 
									<!--td width='1' bgcolor='#ccccccc'><img src='images/one.pix.jpg' width='1' height='1'></td-->
									<td>";

            sb.Append(begin);

            string str1 = "";
            string str2 = "";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (index % 2 == 0)
                    str1 = GetDefaultProductMoldForCGA(dr);
                if (index % 2 == 1)
                    str2 = GetDefaultProductMoldForCGA(dr);

                if (index != 0 && index % 2 == 1)
                {
                    if (index != 1)
                    {
                        string seperator = @"<table width='100%' border='0' align='center' cellpadding='0' cellspacing='0'>
											<tr> 
											<td height='1' background='Images/Default/default.dotline.gif'><img src='Images/Default/default.dotline.gif' width='148' height='1'></td>
											</tr>
										</table>";
                        sb.Append(seperator);
                    }
                    sb.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0'>");
                    sb.Append("	<tr>");
                    sb.Append("		<td width='50%' height='130' valign='top'>").Append(str1).Append("</td>");
                    sb.Append("		<td width='1' background='image/bj_x1.gif'><img src='images/one.pix.jpg' width='1' height='1'></td>");
                    sb.Append("     <td width='50%' valign='top'>").Append(str2).Append("</td>");
                    sb.Append(" </tr>");
                    sb.Append("</table>");

                    str1 = str2 = "";
                }
                index++;
            }
            if (str1 != "")
            {
                string seperator = @"<table width='100%' border='0' align='center' cellpadding='0' cellspacing='0'>
										<tr> 
										<td height='1' background='Images/Default/default.dotline.gif'><img src='Images/Default/default.dotline.gif' width='148' height='1'></td>
										</tr>
									</table>";
                sb.Append(seperator);

                sb.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0'>");
                sb.Append("	<tr>");
                sb.Append("		<td width='50% height='130' valign='top'>").Append(str1).Append("</td>");
                sb.Append("		<td width='1' background='image/bj_x1.gif' width='1' ><img src='images/one.pix.jpg' width='1' height='1'></td>");
                sb.Append("     <td width='50%' valign='top'>").Append("&nbsp;").Append("</td>");
                sb.Append(" </tr>");
                sb.Append("</table>");

            }

            string end = @"			</td>
									<!--td width='1' bgcolor='#ccccccc'><img src='images/one.pix.jpg' width='1' height='1'></td-->
								</tr>
								<!--tr> 
									<td height='1' bgcolor='#ccccccc' colspan='3'><img src='images/one.pix.jpg' width='1' height='1'></td>
								</tr-->
							</table>";
            sb.Append(end);

            return sb.ToString();

        }

        private string GetDefaultProductMoldForCGA(DataRow dr)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table width='100%' height='100%' border='0' cellpadding='0' cellspacing='4' class='tbl' >");
            sb.Append("	<tr class='m-1'>");
            sb.Append("		<td width='85' rowspan='2'  valign='middle'>");
            sb.Append("			<div align='center'>");
            //sb.Append("				<a href=\"javascript:openDialog('~/Items/DisplayPhoto.aspx?ItemID="+dr["SysNo"].ToString()+"')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr["ProductID"]))).Append("' alt='查看大图' width='80' height='60' border='0'></a>");
            sb.Append("				<img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr["ProductID"]))).Append("' width='80' height='60' border='0'>");
            if (Util.TrimDecimalNull(dr["CurrentPrice"]) < Util.TrimDecimalNull(dr["BasicPrice"]))
                sb.Append("				");
            sb.Append("			</div>");
            sb.Append("		</td>");
            sb.Append("		<td valign='top'>");
            sb.Append("			<div>");
            sb.Append("				<a href='Items/ItemDetail.aspx?ItemID=").Append(dr["SysNo"].ToString()).Append("' target='_blank'>").Append(dr["ProductName"].ToString()).Append("</a><br>");

            sb.Append(this.GetPriceOnSaleForCGA(dr));
            sb.Append("			<br><a href='Items/ItemDetail.aspx?ItemID=").Append(dr["SysNo"].ToString()).Append("' target='_blank'>详细介绍...</a><br>");
            sb.Append("			</div>");
            sb.Append("		</td>");
            sb.Append("	</tr>");
            sb.Append(" <tr>");
            sb.Append("		<td >");
            sb.Append("			<table width='100%' border='0' cellspacing='0' cellpadding='0'>");
            sb.Append("				<tr>");
            sb.Append("					<td width='50%' height='32' align='right' valign='middle'><a href='javascript:AddToCartForCGA(").Append(dr["SysNo"].ToString()).Append(")'><img src='Images/button_2.gif'align='middle'  border=0></a></td>");
            sb.Append("				</tr>");
            sb.Append("			</table>");
            sb.Append("		</td>");
            sb.Append("	</tr>");
            sb.Append("</table>");

            return sb.ToString();
        }

        public Hashtable Get3rdProductSysNoHash(int c3SysNo, int brandSysNo, bool isSecondhand)
        {
            string sql = @"select product.sysno from product(nolock), inventory(nolock), product_price(nolock) 
							where
							product.sysno = inventory.productsysno and product.sysno = product_price.productsysno 
							" + onlineShowLimit;
            sql += " and product.c3sysno = " + c3SysNo;
            if (brandSysNo != AppConst.IntNull)
                sql += " and manufacturerSysNo = " + brandSysNo;
            if (isSecondhand)
            {
                sql += " and product.productype=1"; //二手
            }
            else
            {
                sql += " and product.productype<>1 and product.productype<>2"; //非二手和坏品
            }

            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return null;

            Hashtable ht = new Hashtable(30);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ht.Add(Util.TrimIntNull(dr["sysno"]), null);
            }
            return ht;
        }

        public Hashtable Get3rdProductSysNoHashSearch(int c3SysNo, int brandSysNo, bool isSecondhand, string[] arrayFilter, Hashtable htFilter)
        {
            string sql = @"select product.sysno,product.masterproductsysno,inventory.AvailableQty+inventory.VirtualQty as onlineqty from product(nolock), inventory(nolock), product_price(nolock)
							where
							product.sysno = inventory.productsysno and product.sysno = product_price.productsysno 
							" + onlineShowLimit;
            sql += " and product.c3sysno = " + c3SysNo;

            if (arrayFilter != null && arrayFilter.GetLength(0) > 0)
            {
                for (int i = 0; i < arrayFilter.Length; i++)
                {
                    if (arrayFilter[i] != null && arrayFilter[i].Length > 0 && arrayFilter[i].IndexOf(";") > 0)
                    {
                        string[] arTemp = arrayFilter[i].Split(';');
                        sql += " and exists(select productsysno from product_attribute2(nolock) where product_attribute2.attribute2optionsysno='" + arTemp[1].Trim() + "' and product_attribute2.productsysno=product.sysno)";
                    }
                }
            }

            if (htFilter != null && htFilter.Count > 0)
            {
                foreach (string key in htFilter.Keys)
                {
                    sql += " and exists(select productsysno from product_attribute2(nolock) where product_attribute2.attribute2optionsysno='" + key + "' and product_attribute2.productsysno=product.sysno)";
                }
            }

            if (brandSysNo != AppConst.IntNull)
                sql += " and manufacturerSysNo = " + brandSysNo;
            if (isSecondhand)
            {
                sql += " and product.producttype=1";
            }
            else
            {
                sql += " and product.producttype<>1 and product.producttype<>2";
            }

            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return null;

            Hashtable ht = new Hashtable(50);

            Category3Info c3Info = CategoryManager.GetInstance().GetC3Hash()[c3SysNo] as Category3Info;
            if (c3Info.C3Type == 1)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    int productsysno = Util.TrimIntNull(ds.Tables[0].Rows[i]["sysno"]);
                    int masterproductsysno = Util.TrimIntNull(ds.Tables[0].Rows[i]["masterproductsysno"]);
                    if (masterproductsysno == 0 || masterproductsysno == AppConst.IntNull)
                    {
                        masterproductsysno = productsysno;
                    }

                    if (Util.TrimIntNull(ds.Tables[0].Rows[i]["onlineqty"]) > 0)  //可订购
                    {
                        if (!ht.ContainsKey(masterproductsysno))
                            ht.Add(masterproductsysno, null);
                        else
                        {
                            ds.Tables[0].Rows.RemoveAt(i);
                            i--;
                        }
                    }
                    else
                    {
                        ds.Tables[0].Rows.RemoveAt(i);
                        i--;
                    }
                }
            }
            ht.Clear();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ht.Add(Util.TrimIntNull(dr["sysno"]), null);
            }
            return ht;
        }

        #region PriceMold
        public string GetPrice(DataRow dr, bool isShowPointOnly)
        {
            return GetPrice(dr, isShowPointOnly, false);
        }
        public string GetPriceOnSale(DataRow dr)
        {
            return GetPrice(dr, true, true);
        }

        public string GetPriceOnSaleForCGA(DataRow dr)
        {
            return GetPriceForCGA(dr, true, true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="isShowPointOnly">是否需要显示pointonly(没有足够的宽度显示)s</param>
        /// <param name="isOnSale"></param>
        /// <returns></returns>
        private string GetPrice(DataRow dr, bool isShowPointOnly, bool isOnSale)
        {
            string currentPriceTag, basicPriceTag;
            basicPriceTag = "市场价";
            if (isOnSale)
            {
                currentPriceTag = "优惠价";
            }
            else
            {
                currentPriceTag = "ORS商城价";
            }
            StringBuilder sb = new StringBuilder();
            if ((int)dr["PointType"] != (int)AppEnum.ProductPayType.PointPayOnly)
            {
                if ((decimal)dr["basicPrice"] > (decimal)dr["currentPrice"] && isOnSale)
                {
                    sb.Append("		   <img src='../images/arr2.gif'> ");
                    sb.Append("		   ").Append(basicPriceTag).Append("：<span class='moneyD'>￥" + Util.TrimDecimalNull(dr["basicPrice"]).ToString(AppConst.DecimalFormat) + "</span><br>");
                }
                sb.Append("		   <img src='../images/arr2.gif'>");
                sb.Append("		   ").Append(currentPriceTag).Append("：<span class='icson''><strong>￥" + (Util.TrimDecimalNull(dr["currentPrice"]) + Util.TrimDecimalNull(dr["cashrebate"])).ToString(AppConst.DecimalFormat) + "</strong></span>");
                if ((int)dr["PointType"] == (int)AppEnum.ProductPayType.MoneyPayOnly && isShowPointOnly)
                    sb.Append("		   <span class='font9'>(</span><span class='m-3'>不可积分兑换</span><span class='font9'>)</span>");
            }
            else
            {
                sb.Append("		   <img src='../images/arr2.gif' >");
                sb.Append("		   积分兑换：<span class='font10'><strong>" + (Util.TrimDecimalNull(dr["currentPrice"]) * AppConst.ExchangeRate).ToString(AppConst.DecimalToInt) + "</strong></span>");
            }
            if (Util.TrimDecimalNull(dr["cashRebate"]) > 0 && (int)dr["PointType"] != (int)AppEnum.ProductPayType.PointPayOnly)
            {
                sb.Append("		   <br><img src='../images/arr2.gif' >");
                sb.Append("		   现金抵用券：<span class='font10'><strong>￥" + Util.TrimDecimalNull(dr["cashrebate"]).ToString(AppConst.DecimalFormat) + "</strong></span>");
            }
            if ((int)dr["Point"] > 0)
            {
                sb.Append("	        <br><img src='../images/arr2.gif' >");
                sb.Append("			赠送积分：<span class='m-3'>" + dr["Point"].ToString() + "</span>");
            }
            return sb.ToString();
        }

        private string GetPriceForCGA(DataRow dr, bool isShowPointOnly, bool isOnSale)
        {
            string currentPriceTag, basicPriceTag;
            basicPriceTag = "市场价";
            if (isOnSale)
            {
                currentPriceTag = "优惠价";
            }
            else
            {
                currentPriceTag = "ORS商城价";
            }
            StringBuilder sb = new StringBuilder();
            if ((int)dr["PointType"] != (int)AppEnum.ProductPayType.PointPayOnly)
            {
                if ((decimal)dr["basicPrice"] > (decimal)dr["currentPrice"] && isOnSale)
                {
                    sb.Append("		   <img src='images/arr2.gif'> ");
                    sb.Append("		   ").Append(basicPriceTag).Append("：<span class='moneyD'>￥" + Util.TrimDecimalNull(dr["basicPrice"]).ToString(AppConst.DecimalFormat) + "</span><br>");
                }
                sb.Append("		   <img src='images/arr2.gif'>");
                sb.Append("		   ").Append(currentPriceTag).Append("：<span class='icson''><strong>￥" + (Util.TrimDecimalNull(dr["currentPrice"]) + Util.TrimDecimalNull(dr["cashrebate"])).ToString(AppConst.DecimalFormat) + "</strong></span>");
                if ((int)dr["PointType"] == (int)AppEnum.ProductPayType.MoneyPayOnly && isShowPointOnly)
                    sb.Append("		   <span class='font9'>(</span><span class='m-3'>不可积分兑换</span><span class='font9'>)</span>");
            }
            else
            {
                sb.Append("		   <img src='images/arr2.gif' >");
                sb.Append("		   积分兑换：<span class='font10'><strong>" + (Util.TrimDecimalNull(dr["currentPrice"]) * AppConst.ExchangeRate).ToString(AppConst.DecimalToInt) + "</strong></span>");
            }
            if (Util.TrimDecimalNull(dr["cashRebate"]) > 0 && (int)dr["PointType"] != (int)AppEnum.ProductPayType.PointPayOnly)
            {
                sb.Append("		   <br><img src='images/arr2.gif' >");
                sb.Append("		   现金抵用券：<span class='font10'><strong>￥" + Util.TrimDecimalNull(dr["cashrebate"]).ToString(AppConst.DecimalFormat) + "</strong></span>");
            }
            if ((int)dr["Point"] > 0)
            {
                sb.Append("	        <br><img src='images/arr2.gif' >");
                sb.Append("			赠送积分：<span class='m-3'>" + dr["Point"].ToString() + "</span>");
            }
            return sb.ToString();
        }
        #endregion


        private DataSet GetC3ByManufacturerSysNo(int manuSysNo, int productType)
        {
            string sql = @"select 
							distinct category3.*
						from
							category3(nolock), product(nolock), product_price(nolock)
						where
							category3.sysno = product.c3sysno
						and product.manufacturersysno = @manuSysNo
						and product.productType = @productType
						and category3.status = @c3status
						@onlineShowLimit";
            sql = sql.Replace("@c3status", ((int)AppEnum.BiStatus.Valid).ToString());
            sql = sql.Replace("@manuSysNo", manuSysNo.ToString());
            sql = sql.Replace("@productType", productType.ToString());
            sql = sql.Replace("@onlineShowLimit", onlineShowLimit);

            return SqlHelper.ExecuteDataSet(sql);
        }

        public string GetCategoryNavBrand(int manuSysNo)
        {
            DataSet ds = GetC3ByManufacturerSysNo(manuSysNo, (int)AppEnum.ProductType.Normal);
            if (!Util.HasMoreRow(ds))
                return "";

            StringBuilder sb = new StringBuilder();

            //由ds中的有效c3sysno获取 c3和c2
            Hashtable c3BrandHash = new Hashtable();
            Hashtable c2BrandHash = new Hashtable();

            CategoryManager cm = CategoryManager.GetInstance();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Category3Info oC3 = cm.GetC3Hash()[Util.TrimIntNull(dr["sysno"])] as Category3Info;
                c3BrandHash.Add(oC3.SysNo, oC3);

                if (!c2BrandHash.ContainsKey(oC3.C2SysNo))
                {
                    Category2Info oC2 = cm.GetC2Hash()[oC3.C2SysNo] as Category2Info;
                    c2BrandHash.Add(oC2.SysNo, oC2);
                }
            }

            if (c3BrandHash.Count == 0)
                return "";

            int c2Index = 1;
            foreach (Category2Info c2Item in c2BrandHash.Values)
            {
                if (c2Index % 2 != 0)
                    sb.Append("<tr>");
                sb.Append(" <td width=50%>【" + c2Item.C2Name + "】");
                foreach (Category3Info c3Item in c3BrandHash.Values)
                {
                    if (c3Item.C2SysNo == c2Item.SysNo)
                    {
                        sb.Append("<a href='BrandZone.aspx?brandSysNo=").Append(manuSysNo.ToString()).Append("&c3SysNo=").Append(c3Item.SysNo.ToString()).Append("'>").Append(c3Item.C3Name).Append("</a>");
                        sb.Append(" | ");
                    }
                }
                sb.Remove(sb.Length - 2, 2);//除去最后一个"|"
                sb.Append(" </td>");
                if (c2Index % 2 == 0)
                    sb.Append("</tr>");
                c2Index++;
            }

            return sb.ToString();
        }

        /// <summary>
        /// 品牌专区精品推荐
        /// </summary>
        /// <param name="producerSysNo"></param>
        /// <returns></returns>
        public string GetBrandRecommendProduct(int manuSysNo)
        {
            Hashtable ht = new Hashtable(1);
            ht.Add("ListArea", (int)AppEnum.OnlineListArea.AOpenTop1);

            DataSet ds = this.GetOnlineListDs(ht, 1, true, false);

            if (!Util.HasMoreRow(ds))
                return "";

            ManufacturerInfo oManu = ManufacturerManager.GetInstance().Load(manuSysNo);

            string strProduct = this.Get3rdCategoryWithPicMold(ds.Tables[0].Rows[0], oManu.ManufacturerID);
            StringBuilder sb = new StringBuilder();
            sb.Append("<TABLE cellSpacing=0 cellPadding=0 width=96% align=center border=0>");
            sb.Append(" <TR>");
            sb.Append("  <TD width=8><IMG height=37 src='../images/Items/1_06.gif' width=8></TD>");
            sb.Append("  <TD background='../images/Items/1_09.gif'><IMG height=37 src='../images/Items/BrandZone_Main.gif' width=213></TD>");
            sb.Append("  <TD width=8><IMG height=37 src='../images/Items/1_10.gif' width=8></TD>");
            sb.Append(" </TR>");
            sb.Append("</TABLE>");
            sb.Append("<TABLE cellSpacing=0 cellPadding=0 width=96% align=center border=0>");
            sb.Append(" <TR>");
            sb.Append("  <TD width=8 background='../images/Items/1_11.gif'>&nbsp;</TD>");
            sb.Append("  <TD>" + strProduct + "</TD>");
            sb.Append("  <TD width=8 background='../images/Items/1_11-.gif'>&nbsp;</TD>");
            sb.Append(" </TR>");
            sb.Append("</TABLE>");
            sb.Append("<TABLE cellSpacing=0 cellPadding=0 width=96% align=center border=0>");
            sb.Append(" <TR>");
            sb.Append("  <TD width=8><IMG height=8 src='../images/Items/1_13.gif' width=8></TD>");
            sb.Append("  <TD background='../images/Items/1_14.gif'><IMG height=8 src='../images/Items/1_14.gif' width=4></TD>");
            sb.Append("  <TD width=8 background='../images/Items/1_11-.gif'><IMG height=8 src='../images/Items/1_15.gif' width=8></TD>");
            sb.Append(" </TR>");
            sb.Append("</TABLE>");
            return sb.ToString();
        }

        private string getPager(int currentPage, int totalPage, string pageLink)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table width=100% border=0 cellspacing=0 cellspadding=3>");
            sb.Append(" <tr>");
            sb.Append("  <td align=right>");
            if (currentPage > 1)
                sb.Append("   <a href='" + pageLink + "&Page=" + (currentPage - 1) + "'><img src='../images/Items/btn_prev.gif' border=0 height=25 width=68></a>");
            if (currentPage < totalPage)
                sb.Append("   <a href='" + pageLink + "&Page=" + (currentPage + 1) + "'><img src='../images/Items/btn_next.gif' border=0 height=25 width=68></a>");
            sb.Append("  &nbsp;</td>");
            sb.Append("  <td width=60>");
            sb.Append("   共" + totalPage + "页&nbsp;&nbsp;&nbsp;");
            sb.Append("  </td>");
            sb.Append(" </tr>");
            sb.Append("</table>");
            return sb.ToString();
        }
        /// <summary>
        /// 品牌专区商品列表
        /// </summary>
        /// <param name="c3Sysno"></param>
        /// <param name="brandSysno"></param>
        /// <param name="isSecondhand"></param>
        /// <param name="orderby"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public string GetBrandZoneMain(int c3SysNo, int brandSysNo, int pageSize, int currentPage)
        {
            if (brandSysNo == AppConst.IntNull)
                return "";

            string sql = @"select
								product.sysno, productid, left(productid,10)as picid, productname, 
								left(productdesc,150) as productdescription, 
								availableqty+virtualqty as onlineqty,
								product_price.*
							from
								product(nolock), inventory(nolock), product_price(nolock)
							where
								product.sysno = inventory.productsysno
							and product.sysno = product_price.productsysno 
							@onlineShowLimit
							and product.manufacturerSysNo = @manufacturerSysNo
                            and product.producttype<>1 and product.producttype<>2 ";
            //and productid not like '%R' and productid not like '%B'";

            sql = sql.Replace("@onlineShowLimit", this.onlineShowLimit);
            sql = sql.Replace("@c3sysno", c3SysNo.ToString());
            sql = sql.Replace("@manufacturerSysNo", brandSysNo.ToString());

            if (c3SysNo != AppConst.IntNull)
                sql += " and product.c3sysno = " + c3SysNo;

            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return "";

            ManufacturerInfo oManu = ManufacturerManager.GetInstance().Load(brandSysNo);

            string seperator = @"
						<table width='90%' border='0' align='center' cellpadding='0' cellspacing='0'>
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
            int totalPage = ds.Tables[0].Rows.Count / pageSize;
            if (ds.Tables[0].Rows.Count % pageSize != 0)
                totalPage += 1;
            if (currentPage > totalPage)
                currentPage = 1;
            string linkStr = "BrandZone.aspx?brandSysNo=" + brandSysNo;
            if (c3SysNo != AppConst.IntNull)
                linkStr += "&c3Sysno=" + c3SysNo;
            string pager = getPager(currentPage, totalPage, linkStr);
            StringBuilder sb = new StringBuilder(2000);
            //sb.Append(pager);
            sb.Append(seperator);
            int rowindex = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (rowindex >= (currentPage - 1) * pageSize && rowindex < currentPage * pageSize)
                {
                    sb.Append(Get3rdCategoryWithPicMold(dr, oManu.ManufacturerID));
                    sb.Append(seperator);
                }
                rowindex++;
            }
            sb.Append(pager);
            return sb.ToString();
        }


        /// <summary>
        /// 品牌专区新品速递
        /// </summary>
        /// <returns></returns>
        public string GetBrandZoneNew(int listArea)
        {
            Hashtable ht = new Hashtable(5);
            ht.Add("ListArea", listArea);
            ht.Add("OrderBy", " OnlineList.CreateTime Desc");

            DataSet ds = this.GetOnlineListDs(ht, 5, true, true);

            if (!Util.HasMoreRow(ds))
                return "";

            StringBuilder sb = new StringBuilder(2000);

            string seperator = @"
						<table width='90%' border='0' align='center' cellpadding='0' cellspacing='0'>
							<tr> 
								<td height='5'><img src='../images/dot_line02.gif' width='1' height='1'></td>
							</tr>
							<tr> 
								<td height='1' background='../images/dot_line02.gif'><img src='../images/dot_line02.gif' width='148' height='1'></td>
							</tr>
							<tr> 
								<td height='5'><img src='../images/dot_line02.gif' width='1' height='1'></td>
							</tr>
						</table>";

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sb.Append(GetNewcomeMold(dr));
                sb.Append(seperator);
            }

            return sb.ToString();
        }

        public string GetAProducer(int manuSysNo, int productType)
        {
            DataSet ds = GetC3ByManufacturerSysNo(manuSysNo, productType);

            if (!Util.HasMoreRow(ds))
                return "";

            StringBuilder sb = new StringBuilder();

            sb.Append("<table width='100%' border='0' cellspacing='2' cellpadding='0'>\n");

            for (int i = 0; i < ds.Tables[0].Rows.Count; i += 2)
            {
                sb.Append("<tr>\n");
                sb.Append("<td width=5%></td>");
                sb.Append("<td width=45% align=left>");
                sb.Append("<li><a href='ThirdCategory.aspx?ID=" + ds.Tables[0].Rows[i]["SysNo"] + "&producer="
                    + manuSysNo + "&type=" + productType + "'> " + ds.Tables[0].Rows[i]["c3Name"] + "</a></li>");
                sb.Append("</td>");

                sb.Append("<td width=45% align=left>");
                if (i + 1 == ds.Tables[0].Rows.Count)
                {
                    sb.Append("</td></tr>\n");
                    break;
                }
                sb.Append("<li><a href='ThirdCategory.aspx?ID=" + ds.Tables[0].Rows[i + 1]["SysNo"] + "&producer="
                    + manuSysNo + "&type=" + productType + "'> " + ds.Tables[0].Rows[i + 1]["c3Name"] + "</a></li>");
                sb.Append("</td>\n");
                sb.Append("</tr>\n");
            }
            sb.Append("</table>\n");

            return sb.ToString();
        }


        public string GetProductRemarkMold(DataRow dr)
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append("<table width='100%' border='0' align='center' cellpadding='0' cellspacing='0'>");
            //sb.Append(" <tr>");
            //sb.Append("  <td height='6' align='right' bgcolor='#ff6633'><img src='../images/one.pix.jpg' width='1' height='1'></td>");
            //sb.Append(" </tr>");
            //sb.Append("</table>");
            //sb.Append("<table width='100%' border='1' cellpadding='0' cellspacing='0' bordercolor='#cccccc' style='BORDER-COLLAPSE: collapse'>");
            //sb.Append(" <tr>");
            //sb.Append("  <td width='22%'><div align='center'>昵称："+dr["CustomerID"].ToString()+"<br></div></td>");
            //sb.Append("  <td width='23%'><div align='center'>编号："+dr["CustomerSysNo"].ToString()+"</div></td>");
            //sb.Append("  <td width='26%'><div align='center'>评分等级： <img src='../images/Items/stars-blue-"+dr["Score"].ToString()+"-0.gif' width='55' height='12'></div></td>");
            //sb.Append("  <td width='29%'><div align='center'>发表时间："+((DateTime)dr["CreateTime"]).ToLongDateString()+"</div></td>");
            //sb.Append("	</tr>");
            //sb.Append(" <tr>");
            //sb.Append("	 <td colspan='4'>&nbsp;标题："+dr["Title"].ToString()+"</td>");
            //sb.Append(" </tr>");
            //sb.Append(" <tr>");
            //sb.Append("  <td colspan='4'>&nbsp;评价："+dr["Remark"].ToString()+"</td>");
            //sb.Append("	</tr>");
            //sb.Append("</table>");
            //sb.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0'>");
            //sb.Append(" <tr>");
            //sb.Append("  <td height='10'><img src='../images/one.pix.jpg' width='1' height='1'></td>");
            //sb.Append(" </tr>");
            //sb.Append("</table>");

            sb.Append("<table width='100%' border='0' cellpadding='0' cellspacing='0'>");
            sb.Append("<tr bgcolor='#F3F3F3'>");
            sb.Append("	<td colspan='2' height='25'>&nbsp;&nbsp;<strong>" + dr["CustomerID"].ToString() + "</strong>&nbsp;&nbsp;&nbsp;" + ((DateTime)dr["CreateTime"]).ToLongDateString() + "</td>");
            sb.Append("</tr>");
            sb.Append("<tr bgcolor='#F9F9F9'>");
            sb.Append("	<td width='82%' height='25'>&nbsp;&nbsp;<strong>" + dr["Title"].ToString() + "</strong></td>");
            sb.Append("	<td width='18%'><img src='../images/Items/stars-blue-" + dr["Score"].ToString() + "-0.gif' width='55' height='12'></td>");
            sb.Append("</tr>");
            sb.Append("<tr bgcolor='#F9F9F9'>");
            sb.Append("	<td colspan='2'>&nbsp;&nbsp;" + dr["Remark"].ToString() + "</td>");
            sb.Append("</tr>");
            sb.Append("</table>");
            return sb.ToString();
        }

        public string GetProductRemarkList(int productSysNo, string productName)
        {
            string sql = @"select top 5
									customerid, product_remark.*
								from 
									product_remark(nolock), customer(nolock)
								where
									product_remark.customersysno = customer.sysno
								and product_remark.productsysno = @productSysNo
								and product_remark.status = @status
								order by product_remark.sysno desc";
            sql = sql.Replace("@productSysNo", productSysNo.ToString());
            sql = sql.Replace("@status", ((int)AppEnum.BiStatus.Valid).ToString());
            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return "";

            StringBuilder sb = new StringBuilder();
            sb.Append("<table width='100%' border='0' cellpadding='0' cellspacing='0'>");
            sb.Append(" <tr>");
            sb.Append("  <td width='16'><img src='../images/Items/liuyan.gif'></td>");
            //sb.Append("  <td width='16'><img src='../images/Items/remark_l.gif' width='16' height='35'></td>");
            //sb.Append("	 <td background='../images/Items/remark_bg.gif'>关于<span class='m-1'><strong><font color='#ffffff'>"+productName+"</font></strong></span>的客户评价<br></td>");
            //sb.Append("  <td width='16'><img src='../images/Items/remark_r.gif' width='16' height='35'></td>");
            sb.Append(" </tr>");
            sb.Append("</table>");
            //sb.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0'>");
            //sb.Append(" <tr>");
            //sb.Append("  <td width='79%'>&nbsp;</td>");
            //sb.Append("  <td width='21%'>&nbsp;</td>");
            //sb.Append(" </tr>");
            //sb.Append("</table>");	

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sb.Append(GetProductRemarkMold(dr));
            }

            sb.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0'>");
            sb.Append(" <tr>");
            sb.Append("  <td width='64%'>&nbsp;</td>");
            sb.Append("  <td width='19%'>&nbsp;</td>");
            sb.Append("  <td width='17%'>&nbsp;</td>");
            sb.Append(" </tr>");
            sb.Append(" <tr>");
            sb.Append("  <td>&nbsp;</td>");
            sb.Append("  <td>&nbsp;</td>");
            sb.Append("  <td><a href='ItemReview.aspx?ItemID=" + productSysNo + "'><img src='../images/Items/readReview.gif'  border='0'></a></td>");
            sb.Append(" </tr>");
            sb.Append("</table>");
            return sb.ToString();
        }

        public string GetItemRemarkList(int productSysNo, string productName)
        {
            string sql = @"select top 5
									customerid, product_remark.*
								from 
									product_remark(nolock), customer(nolock)
								where
									product_remark.customersysno = customer.sysno
								and product_remark.productsysno = @productSysNo
								and product_remark.status = @status
								order by product_remark.sysno desc";
            sql = sql.Replace("@productSysNo", productSysNo.ToString());
            sql = sql.Replace("@status", ((int)AppEnum.BiStatus.Valid).ToString());
            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return "";

            StringBuilder sb = new StringBuilder();

            sb.Append("<div class=panelr>");
            sb.Append("<div class=panelr_title>");
            sb.Append("<div class=panelr_more><a href='../Items/ItemReview.aspx?ItemID=" + productSysNo + "'>查看所有评论</a></div>");
            sb.Append("<img src='../images/site/main/center/tt_khpr.png' alt='客户评论' /></div>");
            sb.Append("<div class=panelr_content>");
            sb.Append("<div id=dr_commentary>");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sb.Append("<div class=dr_com_block>"); //<!--单个评论区块-->
                sb.Append("<div class=dr_com_b_q>");
                sb.Append("<div class=dr_com_b_q_score>评分:");
                for (int i = 1; i <= Util.TrimIntNull(dr["Score"]); i++)
                {
                    sb.Append("<img src='../images/site/main/center/star1.gif' />");
                }
                sb.Append("</div>");
                sb.Append("<div class=dr_com_b_q_title>" + Util.TrimNull(dr["Title"]) + "</div>");
                sb.Append("<div class=dr_com_b_q_content>" + Util.TrimNull(dr["Remark"]) + "</div>");
                sb.Append("<div class=dr_com_b_q_time>" + Util.TrimNull(dr["CustomerID"]) + " 评论于: " + Util.TrimNull(dr["CreateTime"]) + "</div>");
                sb.Append("</div>");
                sb.Append("</div>");

                sb.Append("<div class=dr_com_b_foot><span class=link_oran></span></div>");
            }

            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");

            return sb.ToString();
        }

        public string GetItemRemarkListString(int productSysNo, int currentPage)
        {
            string sql = @"select top 200
									customerid, product_remark.*
								from 
									product_remark(nolock), customer(nolock)
								where
									product_remark.customersysno = customer.sysno
								and product_remark.productsysno = @productSysNo
								and product_remark.status = @status
								order by product_remark.sysno desc";
            sql = sql.Replace("@productSysNo", productSysNo.ToString());
            sql = sql.Replace("@status", ((int)AppEnum.BiStatus.Valid).ToString());
            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return "";

            int pageSize = 10;
            int totalRowCount = ds.Tables[0].Rows.Count;
            int totalPage = totalRowCount / pageSize;
            if (ds.Tables[0].Rows.Count % pageSize != 0)
                totalPage += 1;

            if (currentPage > totalPage)
                currentPage = 1;
            int rowindex = 0;
            StringBuilder sb = new StringBuilder(2000);
            sb.Append("<div class=panelr>");
            sb.Append("<div class=panelr_title>");
            sb.Append("<img src='../images/site/main/center/tt_khpr.png' alt='客户评论' /></div>");
            sb.Append("<div class=panelr_content>");
            sb.Append("<div id=dr_commentary>");

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (rowindex >= (currentPage - 1) * pageSize && rowindex < currentPage * pageSize)
                {
                    sb.Append("<div class=dr_com_block>"); //<!--单个评论区块-->
                    sb.Append("<div class=dr_com_b_q>");
                    sb.Append("<div class=dr_com_b_q_score>评分:");
                    for (int i = 1; i <= Util.TrimIntNull(dr["Score"]); i++)
                    {
                        sb.Append("<img src='../images/site/main/center/star1.gif' />");
                    }
                    sb.Append("</div>");
                    sb.Append("<div class=dr_com_b_q_title>" + Util.TrimNull(dr["Title"]) + "</div>");
                    sb.Append("<div class=dr_com_b_q_content>" + Util.TrimNull(dr["Remark"]) + "</div>");
                    sb.Append("<div class=dr_com_b_q_time>" + Util.TrimNull(dr["CustomerID"]) + " 评论于: " + Util.TrimNull(dr["CreateTime"]) + "</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("<div class=dr_com_b_foot><span class=link_oran></span></div>");
                }
                rowindex++;
            }

            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");

            return sb.ToString();
        }

        public Hashtable GetItemRemarkSysNoHash(int productSysNo)
        {
            string sql = "select top 200 sysno from product_remark(nolock) where productsysno=" + productSysNo + " order by sysno desc";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            Hashtable ht = new Hashtable();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ht.Add(Util.TrimIntNull(dr["sysno"]), null);
            }
            return ht;
        }

        public DataSet GetSameCateoryProduct(int productSysno, int c3sysno)
        {
            string sql = @"select top 10
							product.sysno, productname, productmode, currentprice, cashrebate, pointtype, productid
						from
							product(nolock), product_price(nolock), inventory(nolock)
						where
							product.sysno <> @productSysNo
						and product.c3sysno = @c3sysno
						and product.sysno = product_price.productsysno
						and product.sysno = inventory.productsysno
						@onlineShowLimit order by newid()";
            sql = sql.Replace("@productSysNo", productSysno.ToString());
            sql = sql.Replace("@c3sysno", c3sysno.ToString());
            sql = sql.Replace("@onlineShowLimit", onlineShowLimit);

            return SqlHelper.ExecuteDataSet(sql);
        }

        public string GetFeaturedProduct()
        {
            Hashtable ht = new Hashtable(5);
            ht.Add("ListArea", (int)AppEnum.OnlineListArea.FeturedProduct);
            ht.Add("OrderBy", "newid()");

            DataSet ds = this.GetOnlineListDs(ht, 10, true, false);

            if (!Util.HasMoreRow(ds))
                return "";

            StringBuilder sb = new StringBuilder(2000);

            string begin = @"<table width='100%' border='1' cellpadding='0' cellspacing='0' bordercolor='#cccccc' style='BORDER-COLLAPSE: collapse'>
								
									<td>

<table cellSpacing='0' cellPadding='0' width='100%' border='0'>
									<tr>
										<td width='26'><IMG height='28' src='../Images/jingpin.gif' width='190' border='0'></td>
									</tr>
								</table>
<table width='100%' border='0' cellpadding='0' cellspacing='0' 
											class='bgup' style='BORDER-COLLAPSE: collapse'>
											<tr>
												<td height='240' valign='top'><br>";
            sb.Append(begin);


            string seperator = @"<div style='padding-top:5px '> 
									<table width='90%' border='0' align='center' cellpadding='0' cellspacing='0'>
										<tr> 
											<td height='5'><img src='../images/dot_line02.gif' width='1' height='1'></td>
										</tr>
										<tr> 
											<td height='1'><img src='../images/dot_line02.gif' width='148' height='1'></td>
										</tr>
										<tr> 
											<td height='5'><img src='../images/dot_line02.gif' width='1' height='1'></td>
										</tr>
									</table>
								</div>";



            int index = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                if (index != 0)
                    sb.Append(seperator);

                sb.Append(GetFeaturedProductMold(dr));
                index++;
            }

            string end = @"						</td>
											</tr>
										</table>
									</td>
								</tr>
							</table>";

            sb.Append(end);

            return sb.ToString();
        }

        private string GetFeaturedProductMold(DataRow dr)
        {
            StringBuilder sb = new StringBuilder(500);
            sb.Append("<table width='96%' border='0' align='center' cellpadding='0' cellspacing='0'>");
            sb.Append("	<tr>");
            sb.Append("		<td>");
            sb.Append("			<table width='100%' border='0' cellspacing='0' cellpadding='0'>");
            sb.Append("				<tr>");
            sb.Append("					<td width='26%'><a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + dr["SysNo"].ToString() + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr["ProductID"]))).Append("' alt='点击查看更多图片' width='80' height='60' border='0'></a></td>");
            sb.Append("					<td width='74%'><a href='../Items/ItemDetail.aspx?ItemID=").Append(dr["SysNo"].ToString()).Append("' target='_blank'><strong>").Append(dr["ProductName"].ToString()).Append("</strong></a></td>");
            sb.Append("				</tr>");
            sb.Append("			</table>");
            sb.Append("		</td>");
            sb.Append("	</tr>");
            sb.Append("	<tr>");
            sb.Append("		<td>");
            sb.Append(GetPrice(dr, false));
            sb.Append("			<br><span class='font9'><img src='../images/arr2.gif' width='10' height='10'><a href='../Items/ItemDetail.aspx?ItemID=").Append(dr["SysNo"].ToString()).Append("' target='_blank'>&nbsp;更多信息&gt;&gt;</a></span>");
            sb.Append("		</td>");
            sb.Append(" </tr>");
            sb.Append("</table>");

            return sb.ToString();
        }

        public DataSet GetOnlineSearchResult(string keyWord, string searchType, int c1SysNo, int orderby)
        {
            string sql = @"select  
							product.sysno, productid, productname, productmode,VirtualArriveTime,PromotionWord,
							product_price.*, availableqty+virtualqty as onlineqty,availableqty,ManufacturerSysNo,category2.sysno c2sysno,category2.C2Name,Manufacturer.ManufacturerName
						from 
							product(nolock), product_price(nolock), inventory(nolock) ,category3(nolock),category2(nolock),category1(nolock),Manufacturer(nolock) 
						where 
								product.sysno = product_price.productsysno
							and product.sysno = inventory.productsysno 
                            and product.c3sysno = category3.sysno
                            and category3.c2sysno = category2.sysno 
                            and category2.c1sysno = category1.sysno 
                            and product_price.currentprice > 0 
                            and product.ManufacturerSysNo = Manufacturer.sysno
							@onlineShowLimit";
            sql = sql.Replace("@onlineShowLimit", onlineShowLimit);

            if (c1SysNo > 0)
            {
                sql += " and category1.sysno = " + c1SysNo;
            }

            if (searchType == "or" || searchType == "and")
            {
                string[] keyWords = keyWord.Split(' ');
                sql += " and (";
                for (int i = 0; i < keyWords.Length; i++)
                {
                    if (i != 0)
                        sql += " " + searchType + " ";
                    sql += "productname like " + Util.ToSqlLikeString(keyWords[i]);
                }
                sql += ")";
            }
            else //whole 或者其他未知的。
            {
                string[] keys = keyWord.Split(' ');
                if (keys.Length == 1)
                {
                    sql += " and (productid like " + Util.ToSqlLikeString(keyWord) + " or productname like " + Util.ToSqlLikeString(keyWord);
                }
                else
                {
                    sql += " and (productid like " + Util.ToSqlLikeString(keyWord) + " or ( 1=1 ";
                    for (int i = 0; i < keys.Length; i++)
                    {
                        sql += " and productname like " + Util.ToSqlLikeString(keys[i]);
                    }
                    sql += ")";
                }
                //try
                //{
                //    int productsysno = Convert.ToInt32(keyWord);
                //    sql += " or product.sysno = " + productsysno;
                //}
                //catch
                //{}
                sql += ")";
            }

            if (orderby == 0)
            {
                sql += " order by productname";
            }
            else if (orderby == 1)
            {
                sql += " order by onlineqty desc";
            }
            else if (orderby == 2)
            {
                sql += " order by currentprice";
            }
            else if (orderby == 3)
            {
                sql += " order by currentprice desc";
            }
            else if (orderby == 4)
            {
                sql += " order by manufacturersysno";
            }
            else
            {
                sql += " order by product.sysno desc";
            }

            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetOnlineSearchResult(string keyWord, string searchType, int c1SysNo, int c2SysNo, int manufacturerSysNo, int orderby)
        {
            string sql = @"select  
							product.sysno, productid, productname, productmode,VirtualArriveTime,PromotionWord,
							product_price.currentprice,product_price.cashrebate,product_price.point, 
                            availableqty+virtualqty as onlineqty,availableqty,ManufacturerSysNo,category2.sysno c2sysno,category2.C2Name,Manufacturer.ManufacturerName
						from 
							product(nolock), product_price(nolock), inventory(nolock) ,category3(nolock),category2(nolock),category1(nolock),Manufacturer(nolock) 
						where 
								product.sysno = product_price.productsysno
							and product.sysno = inventory.productsysno 
                            and product.c3sysno = category3.sysno
                            and category3.c2sysno = category2.sysno 
                            and category2.c1sysno = category1.sysno                             
                            and product.ManufacturerSysNo = Manufacturer.sysno
                            and product_price.currentprice > 0 
							@onlineShowLimit";
            sql = sql.Replace("@onlineShowLimit", onlineShowLimit);

            if (c1SysNo > 0)
                sql += " and category1.sysno = " + c1SysNo;

            if (c2SysNo > 0)
                sql += " and category2.sysno = " + c2SysNo;

            if (manufacturerSysNo > 0)
                sql += " and Manufacturer.SysNo = " + manufacturerSysNo;

            if (searchType == "or" || searchType == "and")
            {
                string[] keyWords = keyWord.Split(' ');
                sql += " and (";
                for (int i = 0; i < keyWords.Length; i++)
                {
                    if (i != 0)
                        sql += " " + searchType + " ";
                    sql += "productname like " + Util.ToSqlLikeString(keyWords[i]);
                }
                sql += ")";
            }
            else //whole 或者其他未知的。
            {
                string[] keys = keyWord.Split(' ');
                if (keys.Length == 1)
                {
                    sql += " and (productid = " + Util.ToSqlString(keyWord) + " or productname like " + Util.ToSqlLikeString(keyWord);
                }
                else
                {
                    sql += " and (productid = " + Util.ToSqlString(keyWord) + " or ( 1=1 ";
                    for (int i = 0; i < keys.Length; i++)
                    {
                        sql += " and productname like " + Util.ToSqlLikeString(keys[i]);
                    }
                    sql += ")";
                }
                sql += ")";
            }

            if (orderby == 0)
            {
                sql += " order by productname";
            }
            else if (orderby == 1)
            {
                sql += " order by onlineqty desc";
            }
            else if (orderby == 2)
            {
                sql += " order by currentprice";
            }
            else if (orderby == 3)
            {
                sql += " order by currentprice desc";
            }
            else if (orderby == 4)
            {
                sql += " order by manufacturersysno";
            }
            else
            {
                sql += " order by product.sysno desc";
            }

            return SqlHelper.ExecuteDataSet(sql);
        }

        public Hashtable GetOnlineSearchResultProductSysNoHash(string keyWord, string searchType, int c1SysNo)
        {
            string sql = @"select  
						product.sysno
					from 
						product(nolock), product_price(nolock), inventory(nolock) ,category3(nolock),category2(nolock),category1(nolock) 
					where 
							product.sysno = product_price.productsysno
						and product.sysno = inventory.productsysno 
                        and product.c3sysno = category3.sysno
                        and category3.c2sysno = category2.sysno 
                        and category2.c1sysno = category1.sysno 
						@onlineShowLimit";

            sql = sql.Replace("@onlineShowLimit", onlineShowLimit);

            if (c1SysNo > 0)
            {
                sql += " and category1.sysno = " + c1SysNo;
            }

            if (searchType == "or" || searchType == "and")
            {
                string[] keyWords = keyWord.Split(' ');
                sql += " and (";
                for (int i = 0; i < keyWords.Length; i++)
                {
                    if (i != 0)
                        sql += " " + searchType + " ";
                    sql += "productname like " + Util.ToSqlLikeString(keyWords[i]);
                }
                sql += ")";
            }
            else //all 或者其他未知的。
            {
                //sql += " and (productid like " + Util.ToSqlLikeString(keyWord) + " or productname like " + Util.ToSqlLikeString(keyWord);
                //try
                //{
                //    int productsysno = Convert.ToInt32(keyWord);
                //    sql += " or product.sysno = " + productsysno;
                //}
                //catch
                //{ }
                //sql += ")";

                string[] keys = keyWord.Split(' ');
                if (keys.Length == 1)
                {
                    sql += " and (productid like " + Util.ToSqlLikeString(keyWord) + " or productname like " + Util.ToSqlLikeString(keyWord);
                }
                else
                {
                    sql += " and (productid like " + Util.ToSqlLikeString(keyWord) + " or ( 1=1 ";
                    for (int i = 0; i < keys.Length; i++)
                    {
                        sql += " and productname like " + Util.ToSqlLikeString(keys[i]);
                    }
                    sql += ")";
                }
                sql += ")";
            }

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;

            Hashtable ht = new Hashtable(30);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ht.Add(Util.TrimIntNull(dr["sysno"]), null);
            }
            return ht;
        }

        public string GetOnlineSearchResultString(DataSet ds, int currentPage)
        {
            int pageSize = 10;
            int totalRowCount = ds.Tables[0].Rows.Count;
            int totalPage = totalRowCount / pageSize;
            if (ds.Tables[0].Rows.Count % pageSize != 0)
                totalPage += 1;

            if (currentPage > totalPage)
                currentPage = 1;
            int rowindex = 0;
            StringBuilder sb = new StringBuilder(2000);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (rowindex >= (currentPage - 1) * pageSize && rowindex < currentPage * pageSize)
                {
                    sb.Append("<div class=fl_page_li>");
                    sb.Append("<table width='100%' border=0 cellspacing=0 cellpadding=0>");
                    sb.Append("<tr>");
                    //sb.Append("<td class=flpl_ce><INPUT name='chkCompare" + Util.TrimNull(dr["SysNo"]) + "' type='checkbox' value='" + Util.TrimNull(dr["SysNo"]) + "' /></td>");
                    sb.Append("<td class=flpl_ce> </td>");
                    sb.Append("<td class=flpl_img><a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + dr["SysNo"].ToString() + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr["ProductID"]))).Append("' alt='查看大图' width='80' height='60' border='0'></a>").Append("</td>");
                    sb.Append("<td class=flpl_txt><a href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(dr["sysno"].ToString()) + "' target='_blank'>" + Util.TrimNull(dr["productname"]) + "<font color=red>" + Util.TrimNull(dr["PromotionWord"]) + "</font></a>");
                    //<div>人气指数：8<br />
                    //客户评价：<img src="images/site/main/center/star1.gif" /><img src="images/site/main/center/star1.gif" /><img src="images/site/main/center/star1.gif" /><img src="images/site/main/center/star1.gif" /><br />
                    if (Util.TrimIntNull(dr["point"].ToString()) > 0)
                    {
                        sb.Append("<div>赠送积分:" + Util.TrimIntNull(dr["point"].ToString()) + "</strong></div>");
                    }
                    sb.Append("</td>");
                    sb.Append("<td class=flpl_price>价格:<span class=color_f60>" + (Util.ToMoney(dr["currentprice"].ToString()) + Util.ToMoney(dr["cashrebate"].ToString())).ToString(AppConst.DecimalFormatWithCurrency) + "</span>");

                    if (Util.TrimIntNull(dr["availableqty"]) > 0)
                        sb.Append("<h4>1日内发货</h4>");
                    else if (Util.TrimIntNull(dr["availableqty"]) <= 0 && Util.TrimIntNull(dr["OnlineQty"]) > 0)
                    {
                        if (Util.TrimIntNull(dr["VirtualArriveTime"]) != AppConst.IntNull)
                        {
                            sb.Append("<h4>" + AppEnum.GetVirtualArriveTime(Util.TrimIntNull(dr["VirtualArriveTime"])) + "发货</h4>");
                        }
                        else
                        {
                            sb.Append("<h4>1-3日发货</h4>");
                        }

                    }
                    else
                        sb.Append("<h4>暂缺货</h4>");
                    sb.Append("</td>");
                    sb.Append("<td class=flpl_btn>");
                    if (Util.TrimIntNull(dr["OnlineQty"]) > 0)
                        sb.Append("<a href=\"javascript:AddToCart('" + dr["SysNo"].ToString() + "')\"><img src='../images/site/main/center/btn_add2cart.png' border='0'></a>");
                    else
                        sb.Append("<a href='../Account/ItemArrivedTellMe.aspx?ItemID=").Append(dr["sysno"].ToString()).Append("'><img src='../images/site/main/center/btn_quehuo.png' border='0'></a>");
                    sb.Append("</td>");

                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("</div>");
                }
                rowindex++;
            }

            return sb.ToString();
        }

        /// <summary>
        /// 最近10天点击率最高的10个产品
        /// </summary>
        /// <returns></returns>
        public DataSet GetClickTop10Product()
        {
            //            string sql = @"select top 10 
            //							product.sysno, productid, productname, productmode, productdesc,VirtualArriveTime,PromotionWord,
            //							product_price.*, availableqty+virtualqty as onlineqty,availableqty, ManufacturerSysNo
            //						from 
            //							product(nolock), product_price(nolock), inventory(nolock), 
            //							(
            //                            select productsysno, sum(clickcount) as clicknum from product_dailyclick(nolock)
            //                            where clickdate > @clickdate
            //                            group by productsysno
            //                            ) as b
            //						where 
            //								product.sysno = product_price.productsysno
            //							and product.sysno = inventory.productsysno
            //							and product.sysno = b.productsysno
            //							@onlineShowLimit";
            //            sql = sql.Replace("@onlineShowLimit", onlineShowLimit);
            //            sql = sql.Replace("@clickdate", Util.ToSqlString(DateTime.Now.AddDays(-10).ToString(AppConst.DateFormat)));

            //            sql += " order by clicknum desc";

            string sql = @"select top 10 
            							product.sysno, productid, productname, productmode, productdesc,VirtualArriveTime,PromotionWord,
            							product_price.*, availableqty+virtualqty as onlineqty,availableqty, ManufacturerSysNo
            						from 
            							product(nolock), product_price(nolock), inventory(nolock)            							
            						where 
            								product.sysno = product_price.productsysno
            							and product.sysno = inventory.productsysno
            							@onlineShowLimit and product_price.currentprice > 0 ";
            sql = sql.Replace("@onlineShowLimit", onlineShowLimit);
            sql += " order by product.AvgDailyClick desc";

            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetRefurbishedCatalogList()
        {
            string sql = @"select distinct category3.*
						from
							category3(nolock), product(nolock), inventory(nolock),
							product_price(nolock)
						where
							category3.sysno = product.c3sysno
						and product.sysno = inventory.productsysno
						and product.sysno = product_price.productsysno
						and productid like '%r%'
						and availableqty+virtualqty>0
						and category3.status = @c3status
						@onlineShowLimit";
            sql = sql.Replace("@c3status", ((int)AppEnum.BiStatus.Valid).ToString());
            sql = sql.Replace("@onlineShowLimit", onlineShowLimit);
            return SqlHelper.ExecuteDataSet(sql);
        }
        public DataSet GetRefurbishedManufacturerList()
        {
            string sql = @"select distinct Manufacturer.*
						from
							Manufacturer(nolock), product(nolock), inventory(nolock),
							product_price(nolock)
						where
							Manufacturer.sysno = product.ManufacturerSysNo
						and product.sysno = inventory.productsysno
						and product.sysno = product_price.productsysno
						and productid like '%r%'
						and availableqty+virtualqty>0
						@onlineShowLimit";
            sql = sql.Replace("@onlineShowLimit", onlineShowLimit);
            return SqlHelper.ExecuteDataSet(sql);
        }
        public DataSet GetWishList(int customerSysNo)
        {
            string sql = @"select 
							wishlist.sysno, product.sysno as productsysno,
							productname, product_price.currentprice, availableqty+virtualqty as onlineqty 
						from
							wishlist(nolock), product(nolock), inventory(nolock), product_price(nolock)
						where
							product.sysno = inventory.productsysno
						and product.sysno = product_price.productsysno
						and product.sysno = wishlist.productsysno
						and wishlist.customersysno = @customerSysNo
						@onlineShowLimit";
            sql = sql.Replace("@onlineShowLimit", onlineShowLimit);
            sql = sql.Replace("@customerSysNo", customerSysNo.ToString());
            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetCartDs(Hashtable ht)
        {
            //ht 是productsysno的集合。
            if (ht == null || ht.Count == 0)
                return null;
            int i = 0;
            StringBuilder sb = new StringBuilder(20);
            foreach (int productsysno in ht.Keys)
            {
                if (i != 0)
                    sb.Append(",");
                sb.Append(productsysno.ToString());
                i++;
            }
            string sql = @"select 
								product.sysno, productname,PromotionWord,VirtualArriveTime,
								product_price.*,
								availableqty+virtualqty as onlineqty,availableqty
							from
								product(nolock), product_price(nolock), inventory(nolock)
							where
								product.sysno = product_price.productsysno
							and product.sysno = inventory.productsysno 
							@onlineShowLimit
							and product.sysno in (@productsysnoS)";
            sql = sql.Replace("@onlineShowLimit", onlineShowLimit);
            sql = sql.Replace("@productsysnoS", sb.ToString());
            return SqlHelper.ExecuteDataSet(sql);
        }

        /// <summary>
        /// GetCartDs的增强版
        /// For UE96
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataTable NewGetCartDs(Hashtable ht)
        {
            //ht 是productsysno的集合。
            if (ht == null || ht.Count == 0)
                return null;
            int i = 0;
            StringBuilder sb = new StringBuilder(20);
            foreach (int productsysno in ht.Keys)
            {
                if (i != 0)
                    sb.Append(",");
                sb.Append(productsysno.ToString());
                i++;
            }
            string sql = @"select 
								product.sysno, C1SysNo,C2SysNo,C3SysNo,Weight,product_price.LimitedQty,
                                productname,PromotionWord,VirtualArriveTime,
								product_price.CurrentPrice,
                                product_price.Point,
                                product_price.BasicPrice,
								availableqty+virtualqty as onlineqty,availableqty,
                                product_simg,product.Weight
							from
								product(nolock), product_price(nolock), inventory(nolock),Product_Images (nolock)
							where
								product.sysno = product_price.productsysno
							and product.sysno = inventory.productsysno 
                            and product.SysNo=Product_Images.product_sysNo
							@onlineShowLimit
                            and Product_Images.status=1
                            and Product_Images.orderNum=1
							and product.sysno in (@productsysnoS)";
            sql = sql.Replace("@onlineShowLimit", onlineShowLimit);
            sql = sql.Replace("@productsysnoS", sb.ToString());
            return SqlHelper.ExecuteDataTable(sql);
        }

        /// <summary>
        /// 获取购物车主商品当前对应赠品集合
        /// </summary>
        /// <param name="ht">购物车主商品SysNo的集合</param>
        /// <returns></returns>
        public DataSet GetCartGiftDs(Hashtable ht)
        {
            if (ht == null || ht.Count == 0)
                return null;
            int i = 0;
            StringBuilder sb = new StringBuilder(20);
            foreach (int productsysno in ht.Keys)
            {
                if (i != 0)
                    sb.Append(",");
                sb.Append(productsysno.ToString());
                i++;
            }
            //            string sql = @"select 
            //								sale_gift.sysno,sale_gift.productsysno as parentsysno,product.sysno as giftsysno, product.productname as giftname,product.weight,sale_gift.productsysno as mastersysno,
            //								p.productname as mastername,availableqty+virtualqty as onlineqty,sale_gift.listOrder
            //							from
            //								product
            //							inner join product_price on product_price.productsysno = product.sysno
            //							inner join inventory on inventory.productsysno = product.sysno
            //							inner join sale_gift on sale_gift.giftsysno = product.sysno
            //							inner join product p on p.sysno = sale_gift.productsysno
            //							where
            //								sale_gift.status = "+(int)AppEnum.BiStatus.Valid
            //                        +@"		and sale_gift.productsysno in (@productsysnoS)
            //								and p.Status = "+(int)AppEnum.ProductStatus.Show+@" and (Product_Price.ClearanceSale=1 or Product_Price.currentprice>=IsNull(Product_Price.unitcost,0))
            //								and product.status = "+(int)AppEnum.ProductStatus.Valid
            //                        +@"	order by sale_gift.listorder";

            string sql = @"select 
								sale_gift.sysno,sale_gift.productsysno as parentsysno,product.sysno as giftsysno, product.productname as giftname,product.weight,sale_gift.productsysno as mastersysno,
								p.productname as mastername,availableqty+virtualqty as onlineqty,sale_gift.listOrder
							from
								product(nolock)
							inner join product_price(nolock) on product_price.productsysno = product.sysno
							inner join inventory(nolock) on inventory.productsysno = product.sysno
							inner join sale_gift(nolock) on sale_gift.giftsysno = product.sysno
							inner join product p(nolock) on p.sysno = sale_gift.productsysno
							where
								sale_gift.status = " + (int)AppEnum.BiStatus.Valid
                        + @"		and sale_gift.productsysno in (@productsysnoS) and (availableqty+virtualqty) > 0
								and p.Status = " + (int)AppEnum.ProductStatus.Show + @" and (Product_Price.ClearanceSale=1 or Product_Price.currentprice>=IsNull(Product_Price.unitcost,0))
								and product.status = " + (int)AppEnum.ProductStatus.Valid
                        + @"	order by sale_gift.listorder";
            sql = sql.Replace("@productsysnoS", sb.ToString());
            return SqlHelper.ExecuteDataSet(sql);
        }

        /// <summary>
        /// 显示对应商品所有在线显示的有效的销售规则
        /// </summary>
        /// <param name="productSysNo"></param>
        /// <returns></returns>
        public string GetSaleRuleOnlineList(int productSysNo)
        {
            DataSet ds = SaleManager.GetInstance().GetSROnlineList(productSysNo);
            StringBuilder sb = new StringBuilder();
            if (Util.HasMoreRow(ds))
            {
                Hashtable srHash = new Hashtable(5);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (!srHash.ContainsKey((int)dr["SaleRuleSysNo"]))
                    {
                        Hashtable itemHash = new Hashtable(5);
                        srHash.Add((int)dr["SaleRuleSysNo"], itemHash);
                    }
                    ((Hashtable)srHash[(int)dr["SaleRuleSysNo"]]).Add((int)dr["ProductSysNo"], dr);
                }
                sb.Append("<table width='440px' align=center valign=top border=0 cellpadding=3 cellspacing=0>");
                int i = 0;
                foreach (Hashtable itemHash in srHash.Values)
                {
                    if (i == 2) //最多只显示2个优惠套装
                    {
                        break;
                    }
                    int num = 0;
                    i++;
                    //sb.Append("<tr><td align=center colspan=2><b>优惠套装" + i.ToString() + "</b></td></tr>"); 
                    decimal subTotal = 0m;
                    decimal currentCost = 0m;
                    string strProductSysNo = "";
                    string strQuantity = "";
                    foreach (DataRow dr in itemHash.Values)
                    {
                        if (num == 0)
                            sb.Append("<tr><td align=center colspan=2><b>" + Util.TrimNull(dr["salerulename"]) + "</b></td></tr>");
                        sb.Append("<tr><td align=left width=90%><a href=ItemDetail.aspx?ItemID=" + dr["productsysno"] + " target='_blank'>" + dr["ProductName"].ToString() + "</a>" + "</td><td align=left width=10%> x" + dr["Quantity"].ToString() + "</td></tr>");
                        subTotal += (int)dr["Quantity"] * (decimal)dr["Discount"];
                        currentCost += (int)dr["Quantity"] * (decimal)dr["currentprice"];
                        strProductSysNo += dr["ProductSysNo"].ToString() + ",";
                        strQuantity += dr["Quantity"].ToString() + ",";
                        num++;
                    }
                    strProductSysNo = strProductSysNo.TrimEnd(',');
                    strQuantity = strQuantity.TrimEnd(',');
                    sb.Append("<tr><td align=center colspan=2>原价" + currentCost.ToString(AppConst.DecimalFormatWithCurrency) + "，<font color='blue'>节省</font><font color='red'><b>" + subTotal.ToString(AppConst.DecimalFormatWithCurrency) + "元</b></font>，<font color='blue'>套装价</font><font color='red'><b>" + (currentCost - subTotal).ToString(AppConst.DecimalFormatWithCurrency) + "元</b></font></td></tr>");
                    sb.Append("<tr><td align=center colspan=2><a href=\"javascript:promotionBuy('" + strProductSysNo + "','" + strQuantity + "');\"><img src='../images/site/main/center/btn_add2cart.gif' /></a>");
                    sb.Append("</td></tr>");
                }
                sb.Append("</table>");
            }
            return sb.ToString();
        }

        public string GetOnlineGift(int productSysNo)
        {
            //            string sql = @"select 
            //								top 1 p.productname,p.sysno,left(p.productdesc,150) as productdesc,p.productid
            //							from 
            //								product p
            //							inner join sale_gift sg on sg.giftsysno = p.sysno
            //							inner join inventory on inventory.productsysno = p.sysno
            //							where 
            //								sg.productsysno = "+productSysNo+" and p.status = "+(int)AppEnum.ProductStatus.Valid+" and sg.status = "+(int)AppEnum.BiStatus.Valid
            //                    +"		and (inventory.availableqty+inventory.virtualqty)>0"
            //                    +"		order by sg.listorder";

            string sql = @"select 
								--top 1 p.productname,p.sysno,left(p.productdesc,150) as productdesc,p.productid
                                p.productname,p.sysno,left(p.productdesc,150) as productdesc,p.productid
							from 
								product p(nolock)
							inner join sale_gift sg(nolock) on sg.giftsysno = p.sysno
							inner join inventory as inv1(nolock) on inv1.productsysno = p.sysno 
                            inner join inventory as inv2(nolock) on inv2.productsysno = sg.giftsysno 
							where 
								sg.productsysno = " + productSysNo + " and p.status = " + (int)AppEnum.ProductStatus.Valid + " and sg.status = " + (int)AppEnum.BiStatus.Valid
                    + "		and (inv1.availableqty+inv1.virtualqty)>0 and (inv2.availableqty+inv2.virtualqty)>0 "
                    + "		order by sg.listorder";

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            StringBuilder sb = new StringBuilder();
            if (Util.HasMoreRow(ds))
            {
                //DataRow dr = ds.Tables[0].Rows[0];
                sb.Append("<table width=100% cellpadding=3 cellspacing=0 border=0>");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    sb.Append(" <tr>");
                    sb.Append("  <td rowspan=2>");
                    sb.Append("   <a href=\"javascript:openDialog('DisplayPhoto.aspx?ItemID=" + dr["SysNo"].ToString() + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr["ProductID"]))).Append("' alt='点击查看更多图片' width='80' height='60' border='0'></a>");
                    sb.Append("  </td>");
                    sb.Append("  <td>" + dr["ProductName"].ToString() + "</td>");
                    sb.Append(" </tr>");
                    sb.Append(" <tr>");
                    sb.Append("  <td>" + dr["ProductDesc"].ToString() + "</td>");
                    sb.Append(" </tr>");
                }
                sb.Append("</table>");
            }
            return sb.ToString();
        }

        public DataSet GetProductNotifyDs(int customerSysNo)
        {
            string sql = @"select 
								product_notify.sysno as notifysysno, product_notify.status as notifystatus, 
								product.sysno, productname, productmode,
								product_price.*,
								availableqty+virtualqty as onlineqty
							from
								product_notify(nolock), product(nolock), product_price(nolock), inventory(nolock)
							where
								product.sysno = product_notify.productsysno
							and	product.sysno = product_price.productsysno
							and product.sysno = inventory.productsysno
							@onlineShowLimit
							and product_notify.customersysno = @customerSysNo
							";
            sql = sql.Replace("@onlineShowLimit", onlineShowLimit);
            sql = sql.Replace("@customerSysNo", customerSysNo.ToString());
            return SqlHelper.ExecuteDataSet(sql);
        }

        #region Partner Query
        public DataSet GetPartnerOrderDs(Hashtable paramHash)
        {
            //Query Order Table
            StringBuilder sb = new StringBuilder();
            sb.Append(@"select 
							convert(nvarchar(10), orderDate, 120) as soDate,count(*) as soCount,
							sum(case when status = 4 then 1 else 0 end) as outSoCount,
							sum(case when status = 4 then soamt else 0 end) as outSoAmt
							from so_master ");
            sb.Append(@"	where status >= 0 and salesmansysno = " + AppConfig.PartnerSysNo);
            if (paramHash.ContainsKey("StartDate"))
            {
                sb.Append(" and orderdate >=" + "cast(" + Util.ToSqlString(paramHash["StartDate"] as string) + " as datetime)");
            }
            if (paramHash.ContainsKey("EndDate"))
            {
                sb.Append("	and orderdate<=" + "cast(" + Util.ToSqlEndDate(paramHash["EndDate"] as string) + " as datetime)");
            }
            sb.Append(@"	group by convert(nvarchar(10), orderDate, 120) 
							order by convert(nvarchar(10), orderDate, 120) ASC");
            DataSet dsOrder = SqlHelper.ExecuteDataSet(sb.ToString());

            //Query Linksource Table
            StringBuilder sbVisit = new StringBuilder();
            sbVisit.Append(@"select 
								convert(nvarchar(10), countDate, 120) as countDate,visitcount
							 from 
								linksource
							 where 
								urlsource = '" + AppConfig.PartnerLinkSource + "'");
            if (paramHash.ContainsKey("StartDate"))
            {
                sbVisit.Append(" and countDate >=" + "cast(" + Util.ToSqlString(paramHash["StartDate"] as string) + " as datetime)");
            }
            if (paramHash.ContainsKey("EndDate"))
            {
                sbVisit.Append("	and countDate<=" + "cast(" + Util.ToSqlEndDate(paramHash["EndDate"] as string) + " as datetime)");
            }
            sbVisit.Append(" order by convert(nvarchar(10), countDate, 120) asc");
            DataSet dsVisit = SqlHelper.ExecuteDataSet(sbVisit.ToString());

            //Query Customer Table 
            StringBuilder sbCus = new StringBuilder();
            sbCus.Append(@"select 
								convert(nvarchar(10), RegisterTime, 120) as RegisterTime,count(*) as RegisterCount
						   from 
								customer
						   where 
								1=1");
            if (paramHash.ContainsKey("StartDate"))
            {
                sbCus.Append(" and RegisterTime >=" + "cast(" + Util.ToSqlString(paramHash["StartDate"] as string) + " as datetime)");
            }
            if (paramHash.ContainsKey("EndDate"))
            {
                sbCus.Append("	and RegisterTime<=" + "cast(" + Util.ToSqlEndDate(paramHash["EndDate"] as string) + " as datetime)");
            }
            sbCus.Append(" group by convert(nvarchar(10), RegisterTime, 120)");
            sbCus.Append(" order by convert(nvarchar(10), RegisterTime, 120) asc");
            DataSet dsCus = SqlHelper.ExecuteDataSet(sbCus.ToString());

            //Merge Tables
            dsOrder.Tables[0].Columns.Add(new DataColumn("visitcount"));
            dsOrder.Tables[0].Columns.Add(new DataColumn("RegisterCount"));

            if (dsVisit.Tables[0].Rows != null && dsVisit.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drVisit in dsVisit.Tables[0].Rows)
                {
                    DataRow[] drOrders = dsOrder.Tables[0].Select("soDate='" + drVisit["countDate"] + "'");
                    if (drOrders != null && drOrders.Length > 0)
                    {
                        drOrders[0]["visitcount"] = drVisit["visitcount"];
                    }
                    else
                    {
                        DataRow drOrder = dsOrder.Tables[0].NewRow();
                        drOrder["soDate"] = drVisit["countDate"];
                        drOrder["visitcount"] = drVisit["visitcount"];
                        dsOrder.Tables[0].Rows.Add(drOrder);
                    }
                }
            }

            if (dsCus.Tables[0].Rows != null && dsCus.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drCus in dsCus.Tables[0].Rows)
                {
                    DataRow[] drOrders = dsOrder.Tables[0].Select("soDate='" + drCus["RegisterTime"] + "'");
                    if (drOrders != null && drOrders.Length > 0)
                    {
                        drOrders[0]["RegisterCount"] = drCus["RegisterCount"];
                    }
                    else
                    {
                        DataRow drOrder = dsOrder.Tables[0].NewRow();
                        drOrder["soDate"] = drCus["RegisterTime"];
                        drOrder["RegisterCount"] = drCus["RegisterCount"];
                        dsOrder.Tables[0].Rows.Add(drOrder);
                    }
                }
            }

            dsOrder.Tables[0].Select("", "soDate asc");
            return dsOrder;
        }
        #endregion

        #region All Item List For Smarter
        public string GetAllItemList()
        {
            string sql = @"select
								product.sysno,productname
							from
								product,product_price
							where
								product.sysno = product_price.productsysno
								@onlineShowLimit";
            sql = sql.Replace("@onlineShowLimit", onlineShowLimit);
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            StringBuilder sb = new StringBuilder();
            if (Util.HasMoreRow(ds))
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    sb.Append("<a href='ItemDetail.aspx?ItemID=");
                    sb.Append(dr["sysno"].ToString());
                    sb.Append("' target='_blank'>");
                    sb.Append(dr["productname"] as string);
                    sb.Append("</a><br>");
                }
            }
            return sb.ToString();
        }
        #endregion

        public string GetAllItemListForAdways(int C1SysNo)
        {
            string sql = @"select
								product.sysno,productname,product_price.currentprice,c1.c1name,c2.c2name,c3.c3name,manufacturer.manufacturername,(inventory.AvailableQty+inventory.VirtualQty) as OnlineCount
							from
								product(nolock),product_price(nolock),category1 as c1(nolock),category2 as c2(nolock),category3 as c3(nolock),manufacturer(nolock),inventory(nolock)
							where
								product.sysno = product_price.productsysno and product.manufacturersysno=manufacturer.sysno
                                and c2.c1sysno=c1.sysno and c3.c2sysno=c2.sysno and product.c3sysno=c3.sysno 
                                and product.sysno = inventory.productsysno @c1
								@onlineShowLimit order by c1.sysno,c2.sysno,c3.sysno,manufacturer.manufacturername";

            if (C1SysNo != 0)
            {
                sql = sql.Replace("@c1", " and c1.sysno=" + C1SysNo);
            }
            else
            {
                sql = sql.Replace("@c1", "");
            }

            sql = sql.Replace("@onlineShowLimit", onlineShowLimit);
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            StringBuilder sb = new StringBuilder();
            sb.Append("<table>");
            if (Util.HasMoreRow(ds))
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    sb.Append("<tr>");
                    sb.Append("<td>" + Util.TrimNull(dr["sysno"]) + "</td>");
                    sb.Append("<td>" + Util.TrimNull(dr["c1name"]) + "</td>");
                    sb.Append("<td>" + Util.TrimNull(dr["c2name"]) + "</td>");
                    sb.Append("<td>" + Util.TrimNull(dr["c3name"]) + "</td>");
                    sb.Append("<td>" + Util.TrimNull(dr["manufacturername"]) + "</td>");
                    sb.Append("<td><a href='http://www.icson.com/Items/ItemDetail.aspx?ItemID=");
                    sb.Append(dr["sysno"].ToString());
                    sb.Append("' target='_blank'>");
                    sb.Append(Util.TrimNull(dr["productname"]));
                    sb.Append("</a></td>");
                    sb.Append("<td>" + Util.ToMoney(dr["currentprice"].ToString()).ToString() + "</td>");
                    if (Util.TrimIntNull(dr["OnlineCount"].ToString()) > 0)
                    {
                        sb.Append("<td>有货</td>");
                    }
                    else
                    {
                        sb.Append("<td>缺货</td>");
                    }
                    sb.Append("</tr>");
                }
            }
            sb.Append("</table>");
            return sb.ToString();
        }

        #region Import
        public void Import()
        {
            if (!AppConfig.IsImportable)
                throw new BizException("Is Importable is false");

            /*  do not  use the following code after Data Pour in */
            string sql = " select top 1 * from onlinelist ";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                throw new BizException("the table online list is not empty");

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                string sql1 = @"select 
								1 as sysno, listarea, 'zzz' as listorder, c.newsysno as productsysno, b.newsysno as createusersysno,
								createtime
							from 
								ipp2003..web_daily_list as a, 
								ippconvert..sys_user as b,
								ippconvert..productbasic as c
							where 
								a.createusersysno = b.oldsysno
							and a.productsysno = c.oldsysno";
                DataSet ds1 = SqlHelper.ExecuteDataSet(sql1);
                foreach (DataRow dr1 in ds1.Tables[0].Rows)
                {
                    OnlineListInfo oInfo = new OnlineListInfo();
                    map(oInfo, dr1);
                    switch (oInfo.ListArea)
                    {
                        case 0:
                            oInfo.ListArea = (int)AppEnum.OnlineListArea.DefaultUp;
                            break;
                        case 1:
                            oInfo.ListArea = (int)AppEnum.OnlineListArea.DefaultDown;
                            break;
                        case 2:
                            oInfo.ListArea = (int)AppEnum.OnlineListArea.FeturedProduct;
                            break;
                        case 3:
                            oInfo.ListArea = (int)AppEnum.OnlineListArea.DefaultTop2;
                            break;
                        default:
                            break;
                    }
                    new OnlineListDac().Insert(oInfo);
                }

                //借用sysno保存原来的类别信息
                string sql2 = @"select 
									categorysysno as sysno, listarea, 'zzz' as listorder, c.newsysno as productsysno, b.newsysno as createusersysno,
									createtime
								from 
									ipp2003..web_index_list as a, 
									ippconvert..sys_user as b,
									ippconvert..productbasic as c
								where 
									a.createusersysno = b.oldsysno
								and a.productsysno = c.oldsysno";
                DataSet ds2 = SqlHelper.ExecuteDataSet(sql2);
                foreach (DataRow dr2 in ds2.Tables[0].Rows)
                {
                    OnlineListInfo oInfo = new OnlineListInfo();
                    map(oInfo, dr2);
                    if (oInfo.SysNo == 1)
                    {
                        if (oInfo.ListArea == 0)
                            oInfo.ListArea = (int)AppEnum.OnlineListArea.Hardware;
                        else if (oInfo.ListArea == 1)
                            oInfo.ListArea = (int)AppEnum.OnlineListArea.HardwareTopSale;
                        else
                            throw new BizException("error1");
                    }
                    else if (oInfo.SysNo == 2)
                    {
                        if (oInfo.ListArea == 0)
                            oInfo.ListArea = (int)AppEnum.OnlineListArea.Digital;
                        else if (oInfo.ListArea == 1)
                            oInfo.ListArea = (int)AppEnum.OnlineListArea.DigitalTopSale;
                        else
                            throw new BizException("error");
                    }
                    else if (oInfo.SysNo == 4)
                    {
                        if (oInfo.ListArea == 0)
                            oInfo.ListArea = (int)AppEnum.OnlineListArea.Accessory;
                        else if (oInfo.ListArea == 1)
                            oInfo.ListArea = (int)AppEnum.OnlineListArea.AccessoryTopSale;
                        else
                            throw new BizException("error");
                    }
                    else
                        throw new BizException("error");
                    new OnlineListDac().Insert(oInfo);
                }

                string sql3 = @"select 
									1 as sysno, 0 as listarea, 'zzz' as listorder, c.newsysno as productsysno, b.newsysno as createusersysno,
									createtime
								from 
									ipp2003..web_new_list as a, 
									ippconvert..sys_user as b,
									ippconvert..productbasic as c
								where 
									a.createusersysno = b.oldsysno
								and a.productsysno = c.oldsysno
								";
                DataSet ds3 = SqlHelper.ExecuteDataSet(sql3);
                foreach (DataRow dr3 in ds3.Tables[0].Rows)
                {
                    OnlineListInfo oInfo = new OnlineListInfo();
                    map(oInfo, dr3);
                    oInfo.ListArea = (int)AppEnum.OnlineListArea.Newcome;
                    new OnlineListDac().Insert(oInfo);
                }

                string sql4 = @"select 
									1 as sysno, 0 as ListArea, 'zzz' as listorder, c.newsysno as productsysno, b.newsysno as createusersysno,
									createtime
								from 
									ipp2003..web_subindex_list as a, 
									ippconvert..sys_user as b,
									ippconvert..productbasic as c
								where 
									a.createusersysno = b.oldsysno
								and a.productsysno = c.oldsysno";
                DataSet ds4 = SqlHelper.ExecuteDataSet(sql4);
                foreach (DataRow dr4 in ds4.Tables[0].Rows)
                {
                    OnlineListInfo oInfo = new OnlineListInfo();
                    map(oInfo, dr4);
                    oInfo.ListArea = (int)AppEnum.OnlineListArea.Audio;
                    new OnlineListDac().Insert(oInfo);
                }


                string sql5 = @"select 
									1 as sysno, ListArea, 'zzz' as listorder, c.newsysno as productsysno, b.newsysno as createusersysno,
									createtime
								from 
									ipp2003..BrandZone_List as a, 
									ippconvert..sys_user as b,
									ippconvert..productbasic as c
								where 
									a.createusersysno = b.oldsysno
								and a.productsysno = c.oldsysno";
                DataSet ds5 = SqlHelper.ExecuteDataSet(sql5);
                foreach (DataRow dr5 in ds5.Tables[0].Rows)
                {
                    OnlineListInfo oInfo = new OnlineListInfo();
                    map(oInfo, dr5);
                    if (Util.TrimIntNull(dr5["ListArea"]) == 0) //recommend
                        oInfo.ListArea = (int)AppEnum.OnlineListArea.AOpenTop1;
                    else//new product
                        oInfo.ListArea = (int)AppEnum.OnlineListArea.AOpenNew;
                    new OnlineListDac().Insert(oInfo);
                }


                scope.Complete();
            }

        }
        #endregion

        public DataSet GetSecondHandShowC1()
        {
            string sql = @"select distinct c1.sysno as c1sysno,c1name,c1.c1id from product(nolock)
                             inner join Product_Price pp(nolock) on  product.sysno=pp.ProductSysNo
                             inner join Category3 c3(nolock) on product.c3sysno=c3.sysno
                             left join inventory(nolock) on product.sysno = inventory.productsysno
                             left join Category2 c2(nolock) on c3.c2sysno=c2.sysno
        		             left join Category1 c1(nolock) on c2.c1sysno=c1.sysno
                             where @Status @OnlineQty  @ProductType  order by c1id";

            sql = sql.Replace("@Status", "Product.Status=" + (int)AppEnum.ProductStatus.Show);
            sql = sql.Replace("@OnlineQty", "and inventory.AvailableQty+inventory.VirtualQty>=1");
            sql = sql.Replace("@ProductType", "and ProductType=1");

            return SqlHelper.ExecuteDataSet(sql);
        }
        public DataSet GetSecondHandShowC2(int c1sysno)
        {
            string sql = @"select distinct c2.sysno as c2sysno,c2name,c2.c2id from product(nolock)
                              inner join Product_Price pp(nolock) on  product.sysno=pp.ProductSysNo
                              inner join Category3 c3(nolock) on product.c3sysno=c3.sysno
                              left join inventory on product.sysno = inventory.productsysno
                              left join Category2 c2(nolock) on c3.c2sysno=c2.sysno
        		              left join Category1 c1(nolock) on c2.c1sysno=c1.sysno
                              where @Status @OnlineQty  @ProductType @c1sysno  order by c2id";
            if (c1sysno == 0)
                sql = sql.Replace("@c1sysno", "");
            else
            {
                sql = sql.Replace("@c1sysno", "and c1.sysno=" + c1sysno);
            }
            sql = sql.Replace("@Status", "Product.Status=" + (int)AppEnum.ProductStatus.Show);
            sql = sql.Replace("@OnlineQty", "and inventory.AvailableQty+inventory.VirtualQty>=1");
            sql = sql.Replace("@ProductType", "and ProductType=1");
            return SqlHelper.ExecuteDataSet(sql);
        }
        public string GetSecondHandCategoryNav(int c1sysno)
        {
            CategoryManager cm = CategoryManager.GetInstance();
            DataSet c1ds = OnlineListManager.GetInstance().GetSecondHandShowC1();
            if (!Util.HasMoreRow(c1ds))
            {
                throw new BizException("暂时没有在线销售的二手商品！");
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("<div id='fl_dnpj' class='panel2'>");
            sb.Append("<div class='panel_title'>");
            sb.Append("二手商品分类");
            sb.Append("</div>");
            sb.Append("<div class='panel_content'>");
            sb.Append("<div class='c_dnpj'>");
            foreach (DataRow dr in c1ds.Tables[0].Rows)
            {
                sb.Append("<span style='cursor:hand' onclick=\"ShowSecondCategory('FirstCategory" + Util.TrimIntNull(dr["c1sysno"]) + "');\">" + Util.TrimNull(dr["c1name"]) + "</span>");

                if (c1sysno == Util.TrimIntNull(dr["c1sysno"]))
                {
                    sb.Append("<div id='FirstCategory" + Util.TrimIntNull(dr["c1sysno"]) + "' style='DISPLAY:' class='c_dnpj_sl'>");
                }
                else
                {
                    sb.Append("<div id='FirstCategory" + Util.TrimIntNull(dr["c1sysno"]) + "' style='DISPLAY:none' class='c_dnpj_sl'>");
                }

                DataSet c2ds = OnlineListManager.GetInstance().GetSecondHandShowC2(Util.TrimIntNull(dr["c1sysno"]));
                foreach (DataRow c2dr in c2ds.Tables[0].Rows)
                {
                    sb.Append("<span><a href='../Items/Secondhandlist.aspx?ID=" + Util.TrimIntNull(c2dr["c2sysno"]) + "'>" + Util.TrimNull(c2dr["c2name"]) + "</a></span>");
                }
                sb.Append("</div>");
            }
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");

            return sb.ToString();
        }


        public DataSet GetNewProductsShowC1(int days)
        {
            string sql = @"select distinct c1.sysno as c1sysno,c1name,c1.c1id from product(nolock)
                             inner join Product_Price pp(nolock) on  product.sysno=pp.ProductSysNo
                             inner join Category3 c3(nolock) on product.c3sysno=c3.sysno
                             left join inventory(nolock) on product.sysno = inventory.productsysno
                             left join Category2 c2(nolock) on c3.c2sysno=c2.sysno
        		             left join Category1 c1(nolock) on c2.c1sysno=c1.sysno
                             where @Status @OnlineQty  @ProductType @days order by c1id";

            sql = sql.Replace("@Status", "Product.Status=" + (int)AppEnum.ProductStatus.Show);
            sql = sql.Replace("@OnlineQty", "and inventory.AvailableQty+inventory.VirtualQty>=1");
            sql = sql.Replace("@ProductType", "and ProductType=" + (int)AppEnum.ProductType.Normal);
            if (days > 0)
            {
                sql = sql.Replace("@days", "and (DATEDIFF(d, Product.CreateTime, GETDATE()) <=" + days + ")");
            }
            else
            {
                sql = sql.Replace("@days", "");
            }

            return SqlHelper.ExecuteDataSet(sql);
        }
        public DataSet GetNewProductsShowC2(int days, int c1sysno)
        {
            string sql = @"select distinct c2.sysno as c2sysno,c2name,c2.c2id from product(nolock)
                             inner join Product_Price pp(nolock) on  product.sysno=pp.ProductSysNo
                             inner join Category3 c3(nolock) on product.c3sysno=c3.sysno
                             left join inventory(nolock) on product.sysno = inventory.productsysno
                             left join Category2 c2(nolock) on c3.c2sysno=c2.sysno
        		             left join Category1 c1(nolock) on c2.c1sysno=c1.sysno
                             where @Status @OnlineQty  @ProductType @days @c1sysno order by c2id";

            sql = sql.Replace("@Status", "Product.Status=" + (int)AppEnum.ProductStatus.Show);
            sql = sql.Replace("@OnlineQty", "and inventory.AvailableQty+inventory.VirtualQty>=1");
            sql = sql.Replace("@ProductType", "and ProductType=" + (int)AppEnum.ProductType.Normal);
            if (days > 0)
            {
                sql = sql.Replace("@days", "and (DATEDIFF(d, Product.CreateTime, GETDATE()) <=" + days + ")");
            }
            else
            {
                sql = sql.Replace("@days", "");
            }
            if (c1sysno == 0)
                sql = sql.Replace("@c1sysno", "");
            else
            {
                sql = sql.Replace("@c1sysno", "and c1.sysno=" + c1sysno);
            }

            return SqlHelper.ExecuteDataSet(sql);
        }

        public string GetNewProductsCategoryNav(int c1sysno, int days)
        {
            DataSet c1ds = OnlineListManager.GetInstance().GetNewProductsShowC1(days);
            if (!Util.HasMoreRow(c1ds))
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("<div id='fl_dnpj' class='panel2'>");
            sb.Append("<div class='panel_title'>");
            sb.Append("新品分类");
            sb.Append("</div>");
            sb.Append("<div class='panel_content'>");
            sb.Append("<div class='c_dnpj'>");
            foreach (DataRow dr in c1ds.Tables[0].Rows)
            {
                sb.Append("<span style='cursor:hand' onclick=\"ShowSecondCategory('FirstCategory" + Util.TrimIntNull(dr["c1sysno"]) + "');\">" + Util.TrimNull(dr["c1name"]) + "</span>");

                if (c1sysno == Util.TrimIntNull(dr["c1sysno"]))
                {
                    sb.Append("<div id='FirstCategory" + Util.TrimIntNull(dr["c1sysno"]) + "' style='DISPLAY:' class='c_dnpj_sl'>");
                }
                else
                {
                    sb.Append("<div id='FirstCategory" + Util.TrimIntNull(dr["c1sysno"]) + "' style='DISPLAY:none' class='c_dnpj_sl'>");
                }
                DataSet c2ds = OnlineListManager.GetInstance().GetNewProductsShowC2(days, Util.TrimIntNull(dr["c1sysno"]));

                foreach (DataRow c2dr in c2ds.Tables[0].Rows)
                {
                    sb.Append("<span><a href='../Items/NewProducts.aspx?ID=" + Util.TrimIntNull(c2dr["c2sysno"]) + "'>" + Util.TrimNull(c2dr["c2name"]) + "</a></span>");
                }
                sb.Append("</div>");
            }
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");

            return sb.ToString();
        }

        /// <summary>
        /// 根据网站搜索结果的中类生成菜单
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="keyword"></param>
        /// <param name="searchType"></param>
        /// <param name="category1"></param>
        /// <param name="c2"></param>
        /// <param name="ma"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public string GetSearchLeftMenuNav(DataSet ds, string keyword, string searchType, int category1, int c2, int ma, int orderby)
        {
            Hashtable hsCategory = new Hashtable();
            Hashtable hsCategoryUrl = new Hashtable();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string c2Name = dr["C2Name"].ToString();
                if (hsCategory.ContainsKey(c2Name))
                {
                    int count = Util.TrimIntNull(hsCategory[c2Name].ToString());
                    hsCategory[c2Name] = count + 1;
                }
                else
                {
                    hsCategory.Add(c2Name, 1);//Response.Redirect("../Items/ItemQuery.aspx?Type=All&Key=" + Server.UrlEncode(txtKeyWord.Text.Trim()) + "&Cat=" + category1);
                    string strUrl = "ItemQuery.aspx?Type=All&Key=" + keyword + "&Cat=" + category1 + "&c2=" + dr["C2SysNo"].ToString();
                    if (ma > 0)
                        strUrl += "&ma=" + ma;
                    hsCategoryUrl.Add(c2Name, strUrl);
                }
            }

            Hashtable hsManuf = new Hashtable();
            Hashtable hsManuUrl = new Hashtable();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string ManufacturerName = dr["ManufacturerName"].ToString();
                if (hsManuf.ContainsKey(ManufacturerName))
                {
                    int count = Util.TrimIntNull(hsManuf[ManufacturerName].ToString());
                    hsManuf[ManufacturerName] = count + 1;
                }
                else
                {
                    hsManuf.Add(ManufacturerName, 1);
                    string strUrl = "ItemQuery.aspx?Type=All&Key=" + keyword + "&Cat=" + category1 + "&Ma=" + dr["ManufacturerSysNo"].ToString();
                    if (c2 > 0)
                        strUrl += "&c2=" + c2;
                    hsManuUrl.Add(ManufacturerName, strUrl);
                }
            }

            string leftmenuCatstr = "";
            if (hsCategory.Count > 0)
            {
                int index = 0;
                StringBuilder sb = new StringBuilder();
                sb.Append("<div id='fl_dnpj' class='panel2'>");
                sb.Append("     <div class='panel_title'>");
                sb.Append("         类别");
                sb.Append("     </div>");
                sb.Append("     <div class='panel_content'>");
                sb.Append("         <div id='CateDiv' class='c_dnpj'>");
                foreach (string key in hsCategory.Keys)
                {
                    index++;
                    if (index < 6)
                        sb.Append("<div style='DISPLAY:block' class='c_dnpj_sl2'>");
                    else if (index == 6)
                    {
                        sb.Append("<span style='cursor:hand;color:#f60;' onclick=\"ShowMoreMenu('c2url');\">更多...</span>");
                        sb.Append("<div id='c2url_" + index.ToString() + "' style='DISPLAY:none' class='c_dnpj_sl2'>");
                    }
                    else
                    {
                        sb.Append("<div  id='c2url_" + index.ToString() + "' style='DISPLAY:none' class='c_dnpj_sl2'>");
                    }
                    sb.Append("<span><a href='" + hsCategoryUrl[key].ToString() + "'>" + key + " (" + hsCategory[key].ToString() + ")" + "</a></span>");

                    sb.Append("</div>");
                }
                sb.Append("         </div>");
                sb.Append("     </div>");
                sb.Append("</div>");

                leftmenuCatstr = sb.ToString();
            }


            string leftmenuManustr = "";

            if (hsManuf.Count > 0)
            {
                int index = 0;
                StringBuilder sb = new StringBuilder();
                sb.Append("<div id='fl_dnpj' class='panel2'>");
                sb.Append("     <div class='panel_title'>");
                sb.Append("         品牌");
                sb.Append("     </div>");
                sb.Append("     <div class='panel_content'>");
                sb.Append("         <div id='ManuDiv' class='c_dnpj'>");
                foreach (string key in hsManuf.Keys)
                {
                    index++;
                    if (index < 6)
                        sb.Append("<div style='DISPLAY:block' class='c_dnpj_sl2'>");
                    else if (index == 6)
                    {
                        sb.Append("<span style='cursor:hand;color:#f60;' onclick=\"ShowMoreMenu('maurl');\">更多...</span>");
                        sb.Append("<div  id='maurl_" + index.ToString() + "' style='DISPLAY:none' class='c_dnpj_sl2'>");
                    }
                    else
                    {
                        sb.Append("<div  id='maurl_" + index.ToString() + "' style='DISPLAY:none' class='c_dnpj_sl2'>");
                    }
                    sb.Append("<span><a href='" + hsManuUrl[key].ToString() + "'>" + key + " (" + hsManuf[key].ToString() + ")" + "</a></span>");

                    sb.Append("</div>");
                }
                sb.Append("         </div>");
                sb.Append("     </div>");
                sb.Append("</div>");

                leftmenuManustr = sb.ToString();
            }


            return leftmenuCatstr + leftmenuManustr;
        }

        /// <summary>
        /// 根据网站搜索结果的大类、中类生成菜单
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="keyword"></param>
        /// <param name="category1"></param>
        /// <param name="c2"></param>
        /// <param name="ma"></param>
        /// <returns></returns>
        public string GetSearchLeftMenuNav(DataSet ds, string keyword, int category1, int c2, int ma)
        {
            string menustr = "";
            string sqlcate = @"
                            select category1.sysno c1sysno,category1.c1name,category2.sysno c2sysno,category2.C2Name,count(*) countnum
                            from 
	                            product(nolock), category3(nolock),category2(nolock),category1(nolock)
                            where 
                                product.c3sysno = category3.sysno
                                and category3.c2sysno = category2.sysno 
                                and category2.c1sysno = category1.sysno 
                                and product.sysno in (@instr)
                            group by category1.c1name,category2.c2name,category1.sysno,category2.sysno 
                            order by category1.sysno,category2.sysno";

            string sqlmanu = @"
                    SELECT dbo.Manufacturer.SysNo, dbo.Manufacturer.ManufacturerName, COUNT(*) 
                          AS countnum
                    FROM dbo.Product INNER JOIN
                          dbo.Manufacturer ON 
                          dbo.Product.ManufacturerSysNo = dbo.Manufacturer.SysNo
                    WHERE dbo.Product.SysNo in(@instr)
                    GROUP BY dbo.Manufacturer.SysNo, dbo.Manufacturer.ManufacturerName
                    order by dbo.Manufacturer.SysNo";


            if (Util.HasMoreRow(ds))
            {
                string productsysnostr = "";
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    productsysnostr += dr["SysNo"].ToString() + ",";
                }
                productsysnostr = productsysnostr.TrimEnd(',');
                sqlcate = sqlcate.Replace("@instr", productsysnostr);
                sqlmanu = sqlmanu.Replace("@instr", productsysnostr);

                DataSet cateDs = SqlHelper.ExecuteDataSet(sqlcate);
                if (Util.HasMoreRow(cateDs))
                {
                    int index = 0;
                    int lastC1 = AppConst.IntNull;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<div id='fl_dnpj' class='panel2'>");
                    sb.Append("     <div class='panel_title'>");
                    sb.Append("         类别");
                    sb.Append("     </div>");
                    sb.Append("     <div class='panel_content'>");
                    sb.Append("         <div class='c_dnpj'>");
                    foreach (DataRow dr in cateDs.Tables[0].Rows)
                    {
                        index++;
                        int currC1 = Util.TrimIntNull(dr["c1sysno"].ToString());
                        string c1name = dr["c1name"].ToString();
                        int currtC2 = Util.TrimIntNull(dr["c2sysno"].ToString());
                        int countnum = Util.TrimIntNull(dr["countnum"].ToString());
                        if (currC1 == lastC1)
                        {
                            string strUrl = "ItemQuery.aspx?Type=All&Key=" + keyword + "&Cat=" + category1 + "&c2=" + currtC2.ToString();
                            if (ma > 0)
                                strUrl += "&ma=" + ma;
                            sb.Append("<span><a href='" + strUrl + "'>" + dr["c2name"].ToString() + " (" + countnum + ")" + "</a></span>");
                        }
                        else
                        {
                            if (index != 1)
                            {
                                sb.Append("</div>");
                            }
                            lastC1 = currC1;
                            string strUrl = "ItemQuery.aspx?Type=All&Key=" + keyword + "&Cat=" + category1 + "&c2=" + currtC2.ToString();
                            if (ma > 0)
                                strUrl += "&ma=" + ma;
                            sb.Append("<span style='cursor:hand;' onclick=\"ShowSecondCategory('C1_" + currC1 + "');\">" + c1name + "</span>");
                            sb.Append("<div id='C1_" + currC1.ToString() + "' style='DISPLAY:none' class='c_dnpj_sl'>");
                            sb.Append("<span><a href='" + strUrl + "'>" + dr["c2name"].ToString() + " (" + countnum + ")" + "</a></span>");
                        }
                    }
                    sb.Append("         </div>");
                    sb.Append("     </div>");
                    sb.Append("   </div>");
                    sb.Append("</div>");

                    menustr += sb.ToString();
                }

                DataSet manuDs = SqlHelper.ExecuteDataSet(sqlmanu);
                if (Util.HasMoreRow(manuDs))
                {
                    int index = 0;
                    int lastMa = AppConst.IntNull;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<div id='fl_dnpj2' class='panel2'>");
                    sb.Append("     <div class='panel_title'>");
                    sb.Append("         品牌");
                    sb.Append("     </div>");
                    sb.Append("     <div class='panel_content'>");
                    sb.Append("         <div class='c_dnpj'>");
                    sb.Append("              <div id='madiv' class='c_dnpj_sl2'>");
                    foreach (DataRow dr in manuDs.Tables[0].Rows)
                    {
                        index++;
                        int currMaNo = Util.TrimIntNull(dr["SysNo"].ToString());
                        string maName = dr["ManufacturerName"].ToString();
                        int countnum = Util.TrimIntNull(dr["countnum"].ToString());
                        lastMa = currMaNo;
                        if (index == 15)
                        {
                            sb.Append("<span style='cursor:hand;color:#f60;' onclick=\"ShowSecondCategory('maurl_morediv');\">" + "更多..." + "</span>");
                            sb.Append("<div  id='maurl_morediv' style='DISPLAY:none' class='c_dnpj_sl2'>");
                        }
                        //sb.Append("<div id='Ma_" + currMaNo.ToString() + "' style='DISPLAY:' class='c_dnpj'>");
                        string strUrl = "ItemQuery.aspx?Type=All&Key=" + keyword + "&Cat=" + category1 + "&ma=" + currMaNo.ToString();
                        if (c2 > 0)
                            strUrl += "&c2=" + c2;
                        sb.Append("<span><a href='" + strUrl + "'>" + maName + " (" + countnum + ")" + "</a></span>");
                        if (index > 14 && index == manuDs.Tables[0].Rows.Count)
                            sb.Append("</div>");
                    }
                    sb.Append("         </div>");
                    sb.Append("     </div>");
                    sb.Append("   </div>");
                    sb.Append("</div>");

                    menustr += sb.ToString();
                }
            }

            return menustr;
        }

        /// <summary>
        /// 根据网站搜索结果的大类中类品牌 获取商品SysNo
        /// </summary>
        /// <param name="keyWord"></param>
        /// <param name="searchType"></param>
        /// <param name="c1SysNo"></param>
        /// <param name="c2SysNo"></param>
        /// <param name="manufacturerSysNo"></param>
        /// <returns></returns>
        public Hashtable GetOnlineSearchResultProductSysNoHash(string keyWord, string searchType, int c1SysNo, int c2SysNo, int manufacturerSysNo)
        {
            string sql = @"select  
							product.sysno
						from 
							product(nolock), product_price(nolock), inventory(nolock) ,category3(nolock),category2(nolock),category1(nolock),Manufacturer(nolock) 
						where 
								product.sysno = product_price.productsysno
							and product.sysno = inventory.productsysno 
                            and product.c3sysno = category3.sysno
                            and category3.c2sysno = category2.sysno 
                            and category2.c1sysno = category1.sysno 
                            and product.ManufacturerSysNo = Manufacturer.sysno
                            and product_price.currentprice > 0                             
							@onlineShowLimit";
            sql = sql.Replace("@onlineShowLimit", onlineShowLimit);

            if (c1SysNo > 0)
                sql += " and category1.sysno = " + c1SysNo;

            if (c2SysNo > 0)
                sql += " and category2.sysno = " + c2SysNo;

            if (manufacturerSysNo > 0)
                sql += " and Manufacturer.SysNo = " + manufacturerSysNo;

            if (searchType == "or" || searchType == "and")
            {
                string[] keyWords = keyWord.Split(' ');
                sql += " and (";
                for (int i = 0; i < keyWords.Length; i++)
                {
                    if (i != 0)
                        sql += " " + searchType + " ";
                    sql += "productname like " + Util.ToSqlLikeString(keyWords[i]);
                }
                sql += ")";
            }
            else //whole 或者其他未知的。
            {
                string[] keys = keyWord.Split(' ');
                if (keys.Length == 1)
                {
                    sql += " and (productid = " + Util.ToSqlString(keyWord) + " or productname like " + Util.ToSqlLikeString(keyWord);
                }
                else
                {
                    sql += " and (productid = " + Util.ToSqlString(keyWord) + " or ( 1=1 ";
                    for (int i = 0; i < keys.Length; i++)
                    {
                        sql += " and productname like " + Util.ToSqlLikeString(keys[i]);
                    }
                    sql += ")";
                }
                sql += ")";
            }

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;

            Hashtable ht = new Hashtable(30);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ht.Add(Util.TrimIntNull(dr["sysno"]), null);
            }
            return ht;
        }

        public DataSet GetC3ProductShowList(Hashtable paramHash)
        {
            string sql = @"select product.*
                         from product(nolock)
                        left join Product_Price(nolock) on Product_Price.productsysno=product.sysno
                        left join Inventory(nolock) on Inventory.productsysno=product.sysno
                        where product.producttype<>1 and product.producttype<>2 and product.C3SysNo = @c3sysno " + onlineShowLimit + "order by product.orderNum";
            //where product.productid not like '%R%' and productid not like '%B%'and product.C3SysNo = @c3sysno " + onlineShowLimit + "order by product.orderNum";

            if (paramHash.ContainsKey("c3sysno"))
                sql = sql.Replace("@c3sysno", Util.ToSqlString(paramHash["c3sysno"].ToString()));
            else
                sql = sql.Replace("@c3sysno", "");
            return SqlHelper.ExecuteDataSet(sql);
        }


        public string GetCountDownCategoryNav(int c1sysno)
        {
            DataSet c1ds = OnlineListManager.GetInstance().GetCountDownShowC1();
            if (!Util.HasMoreRow(c1ds))
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("<div id='fl_dnpj' class='panel2'>");
            sb.Append("<div class='panel_title'>");
            sb.Append("产品分类");
            sb.Append("</div>");
            sb.Append("<div class='panel_content'>");
            sb.Append("<div class='c_dnpj'>");
            foreach (DataRow dr in c1ds.Tables[0].Rows)
            {
                sb.Append("<span style='cursor:hand' onclick=\"ShowSecondCategory('FirstCategory" + Util.TrimIntNull(dr["c1sysno"]) + "');\">" + Util.TrimNull(dr["c1name"]) + "</span>");

                if (c1sysno == Util.TrimIntNull(dr["c1sysno"]))
                {
                    sb.Append("<div id='FirstCategory" + Util.TrimIntNull(dr["c1sysno"]) + "' style='DISPLAY:' class='c_dnpj_sl'>");
                }
                else
                {
                    sb.Append("<div id='FirstCategory" + Util.TrimIntNull(dr["c1sysno"]) + "' style='DISPLAY:none' class='c_dnpj_sl'>");
                }
                DataSet c2ds = OnlineListManager.GetInstance().GetCountDownShowC2(Util.TrimIntNull(dr["c1sysno"]));

                foreach (DataRow c2dr in c2ds.Tables[0].Rows)
                {
                    sb.Append("<span><a href='../Items/countdown.aspx?ID=" + Util.TrimIntNull(c2dr["c2sysno"]) + "'>" + Util.TrimNull(c2dr["c2name"]) + "</a></span>");
                }
                sb.Append("</div>");
            }
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");

            return sb.ToString();
        }

        public DataSet GetCountDownShowC1()
        {
            string sql = @"select distinct c1.sysno as c1sysno,c1name,c1.c1id 
                           from Sale_CountDown sc(nolock)
                           left join Product(nolock) on sc.productsysno=product.sysno
                           left join Category3 c3(nolock) on product.c3sysno=c3.sysno
                           left join Category2 c2(nolock) on c3.c2sysno=c2.sysno
    		               left join Category1 c1(nolock) on c2.c1sysno=c1.sysno
                           left join  Inventory (nolock)ON sc.productsysno = Inventory.ProductSysNo
                           where 1=1 @CountDownStatus  @Status  order by c1id";

            sql = sql.Replace("@CountDownStatus", "and sc.status=" + (int)AppEnum.CountdownStatus.Running);
            sql = sql.Replace("@Status", " and Product.Status=" + (int)AppEnum.ProductStatus.Show);
            //sql = sql.Replace("@OnlineQty", "and inventory.AvailableQty+inventory.VirtualQty>=1");

            return SqlHelper.ExecuteDataSet(sql);
        }
        public DataSet GetCountDownShowC2(int c1sysno)
        {
            string sql = @"select distinct c2.sysno as c2sysno,c2name,c2.c2id ,c1id
                           from Sale_CountDown sc(nolock)
                           inner join Product(nolock) on sc.productsysno=product.sysno
                           inner join Category3 c3(nolock) on product.c3sysno=c3.sysno
                           left join Category2 c2(nolock) on c3.c2sysno=c2.sysno
    		               left join Category1 c1(nolock) on c2.c1sysno=c1.sysno
                           left join  Inventory (nolock)ON sc.productsysno = Inventory.ProductSysNo
                           where 1=1 @CountDownStatus @c1sysno @Status  order by c1id";


            if (c1sysno == 0)
                sql = sql.Replace("@c1sysno", "");
            else
            {
                sql = sql.Replace("@c1sysno", "and c1.sysno=" + c1sysno);
            }
            sql = sql.Replace("@CountDownStatus", "and sc.status=" + (int)AppEnum.CountdownStatus.Running);
            sql = sql.Replace("@Status", " and Product.Status=" + (int)AppEnum.ProductStatus.Show);
            //sql = sql.Replace("@OnlineQty", "and inventory.AvailableQty+inventory.VirtualQty>=1");

            return SqlHelper.ExecuteDataSet(sql);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productSysNo"></param>
        /// <returns></returns>
        public DataSet GetProductSizeStockDs(int productSysNo)
        {
            string sql = @"select product.sysno as productsysno,size2.Size2Name as productsizename 
                       from product(nolock) 
                       inner join size2(nolock) on product.productsize=size2.sysno 
                       inner join inventory (nolock) on product.sysno=inventory.productsysno 
                       inner join product_price (nolock) on product.sysno=product_price.productsysno 
                       where inventory.AvailableQty+inventory.VirtualQty > 0 ";
            sql += onlineShowLimit;
            sql += " and product.masterproductsysno=(select masterproductsysno from product where sysno=@productsysno) ";
            sql += " order by size2.Size2Name";
            sql = sql.Replace("@productsysno", productSysNo.ToString());
            return SqlHelper.ExecuteDataSet(sql);
        }

        /// <summary>
        /// 根据日平均点击数进行排行
        /// </summary>
        /// <param name="paramHash">类别：Category1 eg,1. Category1String eg, "1,2". Category2 eg,1. Category3 eg,1.</param>
        /// <param name="top">显示行数</param>
        /// <returns></returns>
        public DataSet GetProductAvgDailyClickDs(Hashtable paramHash, int top)
        {
            string sql = @"select @Top p.sysno,p.productid,p.productname,pp.currentprice,su.username,
                           inv.accountqty,inv.availableqty+inv.virtualqty as onlineqty,p.avgdailyclick,p.createtime from product p 
                           inner join product_price pp on p.sysno=pp.productsysno
                           inner join inventory inv on p.sysno=inv.productsysno
                           inner join category3 c3 on p.c3sysno=c3.sysno
                           inner join category2 c2 on c3.c2sysno=c2.sysno
                           inner join category1 c1 on c2.c1sysno=c1.sysno 
                           inner join sys_user su on p.ppmusersysno=su.sysno 
                           where p.status=1 @FilterCategory 
                           order by p.avgdailyclick desc";

            sql = sql.Replace("@Top", " top " + top.ToString());

            if (paramHash.ContainsKey("Category1"))
            {
                int c1sysno = (int)paramHash["Category1"];
                sql = sql.Replace("@FilterCategory", " and c1.sysno=" + c1sysno);
            }
            if (paramHash.ContainsKey("Category1String"))
            {
                string c1sysnos = "(" + paramHash["Category1String"].ToString() + ")";
                sql = sql.Replace("@FilterCategory", " and c1.sysno in " + c1sysnos);
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

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;
            return ds;
        }

        public DataSet GetBrandByC1(int C1SysNo, int Top)
        {

            DataSet ds = new DataSet();

            string sql = @"
select DISTINCT @TOP m.ManufacturerName, m.ManufacturerID,m.BriefName,m.SysNo from Manufacturer m
	left join product p on p.ManufacturerSysNo = m.Sysno
	left join Category3 c3 on c3.SysNo = p.C3SysNo
left join Category2 c2 on c2.SysNo = c3.C2SysNo
left join Category1 c1 on c1.SysNo = c2.C1SysNo
where c1.SysNo = @C1SysNo and m.Status = 0
";
            if (Top == 0)
            {
                sql = sql.Replace("@TOP", "");
            }
            else
            {
                sql = sql.Replace("@TOP", " top " + Top.ToString());
            }

            sql = sql.Replace("@C1SysNo", C1SysNo.ToString());

            ds = SqlHelper.ExecuteDataSet(sql);

            if (Util.HasMoreRow(ds))
            {
                return ds;
            }

            return null;
        }


    }
}