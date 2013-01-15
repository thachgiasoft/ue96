using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Transactions;
using Icson.BLL.Basic;
using Icson.Utils;
using Icson.Objects;
using Icson.Objects.Basic;
using Icson.Objects.Online;
using Icson.DBAccess;
using Icson.DBAccess.Online;
using Icson.BLL.Promotion;

namespace Icson.BLL.Online
{
    public class OnlineListProductManager
    {
        private OnlineListProductManager()
        {
        }
        private static OnlineListProductManager _instance;
        public static OnlineListProductManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new OnlineListProductManager();
            }
            return _instance;
        }

        public string onlineShowLimit = "and Product.Status = " + ((int)AppEnum.ProductStatus.Show).ToString() + " and (Product_Price.ClearanceSale=1 or Product_Price.currentprice>=IsNull(Product_Price.unitcost,0))";
        private int MaxDesLen = 150;

        private void map(OnlineListProductInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.OnlineAreaType = Util.TrimIntNull(tempdr["OnlineAreaType"]);
            oParam.OnlineRecommendType = Util.TrimIntNull(tempdr["OnlineRecommendType"]);
            oParam.CategorySysNo = Util.TrimIntNull(tempdr["CategorySysNo"]);
            oParam.ProductSysNo = Util.TrimIntNull(tempdr["ProductSysNo"]);
            oParam.CreateUserSysNo = Util.TrimIntNull(tempdr["CreateUserSysNo"]);
            oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
            oParam.ListOrder = Util.TrimIntNull(tempdr["ListOrder"]);
        }

        public int Insert(OnlineListProductInfo oParam)
        {
            if (LoadOnlineListProduct(oParam.OnlineAreaType, oParam.OnlineRecommendType, oParam.CategorySysNo, oParam.ProductSysNo) != null)
            {
                throw new BizException("duplicated!");
            }
            return new OnlineListProductDac().Insert(oParam);
        }

        public int Update(OnlineListProductInfo oParam)
        {
            return new OnlineListProductDac().Update(oParam);
        }

        public OnlineListProductInfo LoadOnlineListProduct(int OnlineAreaType, int OnlineRecommendType, int CategorySysNo, int ProductSysNo)
        {
            string sql = "select * from onlinelistproduct(nolock) where onlineareatype=" + OnlineAreaType + " and onlinerecommendtype=" + OnlineRecommendType + " and categorysysno=" + CategorySysNo + " and productsysno=" + ProductSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                OnlineListProductInfo oInfo = new OnlineListProductInfo();
                map(oInfo, ds.Tables[0].Rows[0]);
                return oInfo;
            }
            else
            {
                return null;
            }
        }

        public void Delete(int sysno)
        {
            new OnlineListProductDac().Delete(sysno);
        }

        public DataSet GetOnlineListProductDs(Hashtable paramHash, int topNumber, bool isOnlineShow, bool isQtyShow)
        {
            string sql = @" select @top
								OnlineListProduct.SysNo as OnlineListProductSysNo, OnlineListProduct.OnlineAreaType,OnlineListProduct.OnlineRecommendType,
                                Product.SysNo as SysNo,Product.C3SysNo as C3SysNo,ProductId, ProductName, Product.BriefName,PromotionWord, --ProductDesc,
                                ListOrder,left(productdesc," + MaxDesLen + ") as productdescription,";
            sql += @"		AvailableQty+VirtualQty as OnlineQty, Product.Status, UserName as CreateUserName,
								OnlineListProduct.CreateTime, Product_Price.*,Category1.C1ID, Category2.C2ID, Category3.C3ID
							from 
								OnlineListProduct(nolock),
								Product(nolock),
								Product_Price(nolock),
								Sys_User(nolock),
								Inventory(nolock),
								Category3(nolock),
								Category2(nolock),
								Category1(nolock)
							where
								OnlineListProduct.productsysno = Product.sysno
							and Category3.sysno=Product.C3SysNo
							and Category2.sysno=Category3.C2SysNo
							and Category1.sysno=Category2.C1SysNo
							and OnlineListProduct.CreateUserSysNo = Sys_User.SysNo
							and Inventory.ProductSysNo = OnlineListProduct.ProductSysNo
							and OnlineListProduct.ProductSysNo = Product_Price.ProductSysNo
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
                        sqlOrderBy = "order by newid()";
                        sb.Append(" and ");
                        if (key == "ProductSysNo")
                        {
                            sb.Append("OnlineListProduct.ProductSysNo = ").Append(item.ToString());
                        }
                        else if (key == "OnlineAreaType")
                        {
                            sb.Append("OnlineListProduct.OnlineAreaType=").Append(item.ToString());
                        }
                        else if (key == "OnlineRecommendType")
                        {
                            sb.Append("OnlineListProduct.OnlineRecommendType=").Append(item.ToString());
                        }
                        else if (key == "CategorySysNo")
                        {
                            sb.Append("OnlineListProduct.CategorySysNo=").Append(item.ToString());
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

        public string GetHomePageDiscount()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div id=fc_tj class=panelc>");
            sb.Append("<div class=panelc_title>");
            //sb.Append(    "<div class=panelc_more><a href='#'><img src='../images/site/main/left/more.png' /></a></div>");
            sb.Append("<img src='../images/site/main/center/tt_tj.png' alt='特价商品' />");
            sb.Append("</div>");
            sb.Append("<div class=panelc_content>");
            sb.Append("<div class=c_tj>");

            Hashtable ht = new Hashtable();
            ht.Add("OnlineAreaType", (int)AppEnum.OnlineAreaType.HomePage);
            ht.Add("OnlineRecommendType", (int)AppEnum.OnlineRecommendType.Discount);
            DataSet ds = GetOnlineListProductDs(ht, 6, true, true);
            if (!Util.HasMoreRow(ds))
                return "";

            int rowcount = 1;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (rowcount < 3)
                {
                    sb.Append("<div class=c_tj_li>");
                }
                else
                {
                    sb.Append("<div class='c_tj_li tj_bo'>");
                }
                sb.Append("<div class=c_tj_img>").Append("<a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + dr["SysNo"].ToString() + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr["ProductID"]), true)).Append("' alt='查看大图' width=80 height=60 border='0'></a>").Append("</div>");
                sb.Append("<div class=c_tj_p>").Append("<a href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(dr["sysno"].ToString()) + "' target='_blank'>" + Util.TrimNull(dr["productname"]) + "<font color=red>" + Util.TrimNull(dr["PromotionWord"]) + "</font>" + "</a><br />");
                if (Util.ToMoney(dr["basicprice"].ToString()) > Util.ToMoney(dr["currentprice"].ToString()))
                {
                    sb.Append("<span class=c_tj_pr>市场价：" + Util.ToMoney(dr["basicprice"].ToString()).ToString(AppConst.DecimalFormatWithCurrency) + "<br/></span>");
                }
                sb.Append("<span class=c_tj_pr>ORS商城价：" + (Util.ToMoney(dr["currentprice"].ToString()) + Util.ToMoney(dr["CashRebate"].ToString())).ToString(AppConst.DecimalFormatWithCurrency) + "</span><br />");
                if (Util.TrimIntNull(dr["point"].ToString()) > 0)
                {
                    sb.Append("<span class=c_tj_pr>赠送积分:" + Util.TrimIntNull(dr["point"].ToString())).Append("</span>");
                }
                sb.Append("</div>");
                sb.Append("</div>");

                if (rowcount % 2 == 0 && rowcount < 5)
                {
                    sb.Append("<hr />");
                }
                else if (rowcount == 6)
                {
                    sb.Append("<br clear=all />");
                }

                rowcount++;
            }

            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            return sb.ToString();
        }

        public string GetHomePageFeatured()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div id=fc_tj class=panelc>");
            sb.Append("<div class=panelc_title>");
            //sb.Append(    "<div class=panelc_more><a href='#'><img src='../images/site/main/left/more.png' /></a></div>");
            sb.Append("<img src='../images/site/main/center/tt_tjsp.png' alt='推荐商品' />");
            sb.Append("</div>");
            sb.Append("<div class=panelc_content>");
            sb.Append("<div class=c_tj>");

            Hashtable ht = new Hashtable();
            ht.Add("OnlineAreaType", (int)AppEnum.OnlineAreaType.HomePage);
            ht.Add("OnlineRecommendType", (int)AppEnum.OnlineRecommendType.Featured);
            DataSet ds = GetOnlineListProductDs(ht, 8, true, true);
            if (!Util.HasMoreRow(ds))
                return "";

            int rowcount = 1;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (rowcount < 3)
                {
                    sb.Append("<div class=c_tj_li>");
                }
                else
                {
                    sb.Append("<div class='c_tj_li tj_bo'>");
                }
                sb.Append("<div class=c_tj_img>").Append("<a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + dr["SysNo"].ToString() + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr["ProductID"]), true)).Append("' alt='查看大图' width=80 height=60 border='0'></a>").Append("</div>");
                sb.Append("<div class=c_tj_p>").Append("<a href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(dr["sysno"].ToString()) + "' target='_blank'>" + Util.TrimNull(dr["productname"]) + "<font color=red>" + Util.TrimNull(dr["PromotionWord"]) + "</font>" + "</a><br />");
                if (Util.ToMoney(dr["basicprice"].ToString()) > Util.ToMoney(dr["currentprice"].ToString()))
                {
                    sb.Append("<span class=c_tj_pr>市场价：" + Util.ToMoney(dr["basicprice"].ToString()).ToString(AppConst.DecimalFormatWithCurrency) + "<br/></span>");
                }
                sb.Append("<span class=c_tj_pr>ORS商城价：" + (Util.ToMoney(dr["currentprice"].ToString()) + Util.ToMoney(dr["CashRebate"].ToString())).ToString(AppConst.DecimalFormatWithCurrency) + "</span><br />");
                if (Util.TrimIntNull(dr["point"].ToString()) > 0)
                {
                    sb.Append("<span class=c_tj_pr>赠送积分:" + Util.TrimIntNull(dr["point"].ToString())).Append("</span>");
                }
                sb.Append("</div>");
                sb.Append("</div>");

                if (rowcount % 2 == 0 && rowcount < 7)
                {
                    sb.Append("<hr />");
                }
                else if (rowcount == 8)
                {
                    sb.Append("<br clear=all />");
                }

                rowcount++;
            }

            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            return sb.ToString();
        }

        //add by judy
        public string GetPromotionTopic()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div id=fl_cxzt class=panelr> ");
            sb.Append("<div class=panelr_title>");
            sb.Append("<img src='../images/site/main/right/tt_cxzt.png' alt='促销主题'/></div>");
            sb.Append("<div class=panelr_content>");
            sb.Append("<div class=c_cxzt>");
            Hashtable ht = new Hashtable();
            ht.Add("OnlineAreaType", (int)AppEnum.OnlineAreaType.HomePage);
            ht.Add("OnlineRecommendType", (int)AppEnum.OnlineRecommendType.PromotionTopic);
            DataSet ds = GetOnlineListProductDs(ht, 10, true, true);
            if (!Util.HasMoreRow(ds))
                return "";

            int i = 1;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sb.Append("<div class=c_cxzt_li>");
                sb.Append("<a href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(dr["sysno"].ToString()) + "' target='_blank'>" + Util.TrimNull(dr["BriefName"]) + "</a></br>");
                sb.Append("<span class=c_cxzt_pr>ORS商城价：" + Util.ToMoney(dr["currentprice"].ToString()).ToString(AppConst.DecimalFormatWithCurrency));
                sb.Append("</span></div>");
                i++;
            }
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");

            return sb.ToString();
        }
        //add by judy
        public string GetPopularProduct()
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            ht.Add("OnlineAreaType", (int)AppEnum.OnlineAreaType.HomePage);
            ht.Add("OnlineRecommendType", (int)AppEnum.OnlineRecommendType.PopularProduct);
            ht.Add("OrderBy", "OnlineListProduct.CreateTime desc");
            DataSet ds = GetOnlineListProductDs(ht, 2, true, true);
            if (!Util.HasMoreRow(ds))
                return "";

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sb.Append("<a href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(dr["sysno"].ToString()) + "' target='_blank'>" + Util.TrimNull(dr["BriefName"]) + "&nbsp;&nbsp;<font color='red'>特价" + Util.ToMoney(dr["currentprice"].ToString()).ToString(AppConst.DecimalFormatWithCurrency) + "</font></a>");
                sb.Append("&nbsp;&nbsp;&nbsp;&nbsp;");
            }
            return sb.ToString();
        }

        public string GetHomePagePromotion()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div id=ml_sale>");
            sb.Append("<div id=ml_sale_title><img src='../images/site/sale_title.jpg' alt='' border=0 /></div>");
            sb.Append("<div id=ml_sale_table>");
            sb.Append("<ul>");

            Hashtable ht = new Hashtable();
            ht.Add("OnlineAreaType", (int)AppEnum.OnlineAreaType.HomePage);
            ht.Add("OnlineRecommendType", (int)AppEnum.OnlineRecommendType.Promotion);
            DataSet ds = GetOnlineListProductDs(ht, 4, true, true);
            if (!Util.HasMoreRow(ds))
                return "";

            int i = 1;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sb.Append("<li><table width='100%' border=0 cellspacing=0 cellpadding=0>");
                sb.Append("<tr><td width='15' valign=top>" + i + ".</td>");
                sb.Append("");
                sb.Append("<td><a href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(dr["sysno"]) + "' target='_blank'>" + Util.TrimNull(dr["productname"]) + "</a></td></tr><tr><td>&nbsp;</td>");
                sb.Append("<td class=ml_sale_price1>市场价：" + Util.ToMoney(dr["basicprice"].ToString()).ToString(AppConst.DecimalFormatWithCurrency) + "</td></tr><tr><td>&nbsp;</td>");
                sb.Append("<td class=ml_sale_price2>ORS商城价：" + Util.ToMoney(dr["currentprice"].ToString()).ToString(AppConst.DecimalFormatWithCurrency) + "</td></tr></table></li>");
                i++;
            }
            sb.Append("</ul>");
            sb.Append("</div>");
            sb.Append("</div>");

            return sb.ToString();
        }

        public string GetC1FeaturedProducts(int c1SysNo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div id=mr_jstj>");
            sb.Append("<div class=mr_title>");
            sb.Append("<div class=mr_title_more><img src='../images/site/list_style4.gif' /><a href='#' target='_blank'>更多</a></div>");
            sb.Append("<img src='../images/site/jstj_title.gif' width=173 height=27 />");
            sb.Append("</div>");
            sb.Append("<div id=mr_jstj_table>");

            Hashtable ht = new Hashtable();
            ht.Add("OnlineAreaType", (int)AppEnum.OnlineAreaType.FirstCategory);
            ht.Add("OnlineRecommendType", (int)AppEnum.OnlineRecommendType.Featured);
            DataSet ds = GetOnlineListProductDs(ht, 12, true, true);
            if (!Util.HasMoreRow(ds))
                return "";

            int rowcount = 1;

            sb.Append("<table width='100%' border='0' align='center' cellpadding='18' cellspacing='0'>");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (rowcount % 4 == 1)
                {
                    sb.Append("<tr>");
                }

                if (rowcount < 4)
                {
                    sb.Append("<td class=yztj_td1 valign=top>");
                }
                else if (rowcount == 4 || rowcount == 8)
                {
                    sb.Append("<td class=yztj_td2 valign=top>");
                }
                else if (rowcount < 8)
                {
                    sb.Append("<td class=yztj_td1 valign=top>");
                }
                else if (rowcount < 12)
                {
                    sb.Append("<td class=yztj_td3 valign=top>");
                }
                else
                {
                    sb.Append("<td valign=top>");
                }
                sb.Append("<div>").Append("<a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + dr["SysNo"].ToString() + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr["ProductID"]))).Append("' alt='查看大图' width='80' height='60' border='0'></a>").Append("<br />");
                sb.Append("<a href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(dr["sysno"]) + "' target='_blank'>" + Util.TrimNull(dr["productname"]) + "</a><br />");
                sb.Append("           <strong class=title_focus>ORS商城价:" + Util.ToMoney(dr["currentprice"].ToString()).ToString(AppConst.DecimalFormatWithCurrency) + "<br />");
                if (Util.TrimIntNull(dr["point"].ToString()) > 0)
                {
                    sb.Append("赠送积分:<strong>" + Util.TrimIntNull(dr["point"].ToString()) + "<strong><br />");
                }
                //sb.Append(             "相关评论:");
                sb.Append("</div>");
                sb.Append("</td>");

                if (rowcount % 4 == 0)
                {
                    sb.Append("</tr>");
                }

                rowcount++;
            }

            sb.Append("</table>");
            sb.Append("</div>");
            sb.Append("</div>");

            return sb.ToString();
        }

        //Judy 2008-4-9 修改 精品推荐
        public string GetC2FeaturedProducts(int c2SysNo)
        {
            Hashtable ht = new Hashtable();
            ht.Add("OnlineAreaType", (int)AppEnum.OnlineAreaType.SecondCategory);
            ht.Add("CategorySysNo", c2SysNo);
            ht.Add("OnlineRecommendType", (int)AppEnum.OnlineRecommendType.Featured);
            DataSet ds = GetOnlineListProductDs(ht, 4, true, true);
            if (!Util.HasMoreRow(ds))
            {
                string sql = @"select top 4 Product.SysNo as SysNo,Product.C3SysNo as C3SysNo,ProductId, ProductName,PromotionWord, ProductDesc, 
                                left(productdesc,150) as productdescription,AvailableQty+VirtualQty as OnlineQty, Product.Status,
								Product_Price.*,Category1.C1ID, Category2.C2ID, Category3.C3ID
							from 
								Product(nolock),
								Product_Price(nolock),
								Inventory(nolock),
								Category3(nolock),
								Category2(nolock),
								Category1(nolock)
							where Category3.sysno=Product.C3SysNo
							and Category2.sysno=Category3.C2SysNo
							and Category1.sysno=Category2.C1SysNo
			                and Product_Price.ProductSysNo = Product.SysNo 
							and Inventory.ProductSysNo = Product.SysNo
							and Product.Status = 1 and (Product_Price.ClearanceSale=1 or Product_Price.currentprice>=IsNull(Product_Price.unitcost,0))
						    and AvailableQty+VirtualQty>0 and category2.sysno=@c2sysno order by product.sysno desc";
                sql = sql.Replace("@c2sysno", c2SysNo.ToString());
                ds = SqlHelper.ExecuteDataSet(sql);
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=panelc>");
            sb.Append("<div class=panelc_title>");
            sb.Append("<img src='../images/site/main/center/tt_jptj.png' alt='精品推荐' /></div>");
            sb.Append("<div class=panelc_content>");
            sb.Append("<div class=c_jptj>");
            sb.Append("<table width='100%'>");
            int rowcount = 1;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (rowcount % 2 == 1)
                {
                    sb.Append("<tr >");
                }

                sb.Append("<td width='270px' style='padding:5px'>");
                sb.Append("<a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + dr["SysNo"].ToString() + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr["ProductID"]))).Append("' alt='查看大图' width='80' height='60' border='0'></a><br>");
                sb.Append("<a href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(dr["sysno"]) + "' target='_blank'>" + Util.TrimNull(dr["productname"]) + "<font color=red>" + Util.TrimNull(dr["PromotionWord"]) + "</font>" + "</a><br />");
                sb.Append("<span class=color_f60>" + Util.ToMoney(dr["currentprice"].ToString()).ToString(AppConst.DecimalFormatWithCurrency) + "元</span>");
                sb.Append("</td>");
                if (rowcount % 2 == 0)
                {
                    sb.Append("</tr>");
                }
                rowcount++;

            }
            sb.Append("</tr></table>");
            //foreach(DataRow dr in ds.Tables[0].Rows)
            //{
            //    sb.Append("<div class=c_jptj_li>");
            //    sb.Append(  "<div class=c_jptj_img>").Append("<a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + dr["SysNo"].ToString() + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr["ProductID"]))).Append("' alt='查看大图' width='80' height='60' border='0'></a>").Append("</div>");
            //    sb.Append(  "<div class=c_jptj_txt><a href='../Items/ItemDetail.aspx?ItemID="+ Util.TrimIntNull(dr["sysno"]) +"' target='_blank'>"+ Util.TrimNull(dr["productname"]) +"</a><br />");
            //    sb.Append(  "<span class=color_f60>"+ Util.ToMoney(dr["currentprice"].ToString()).ToString(AppConst.DecimalFormatWithCurrency) +"元</span></div>");
            //    sb.Append("</div>");
            //}
            sb.Append("<br clear=all />");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");

            return sb.ToString();
        }

        /// <summary>
        /// 新品推荐(用于网站中类商品页面显示) 优先精品推荐，如果没有则显示新品
        /// </summary>
        /// <param name="c2SysNo"></param>
        /// <param name="IsRiorityFeatured">是否优先显示精品</param>
        /// <returns></returns>
        public string GetC2NewProducts(int c2SysNo, bool IsRiorityFeatured)
        {
            DataSet ds = new DataSet();
            if (IsRiorityFeatured)
            {
                Hashtable ht = new Hashtable();
                ht.Add("OnlineAreaType", (int)AppEnum.OnlineAreaType.SecondCategory);
                ht.Add("CategorySysNo", c2SysNo);
                ht.Add("OnlineRecommendType", (int)AppEnum.OnlineRecommendType.Featured);
                ds = GetOnlineListProductDs(ht, 4, true, true);
            }
            if (!Util.HasMoreRow(ds))
            {
                ds = NewProducts(c2SysNo, 30, 4);
                if (!Util.HasMoreRow(ds))
                    ds = NewProducts(c2SysNo, 0, 4);
            }

            if (Util.HasMoreRow(ds))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<div class=panelc>");
                sb.Append("<div class=panelc_title>");
                sb.Append("<img src='../images/site/main/center/tt_jptj.png' alt='精品/新品推荐' /></div>");
                sb.Append("<div class=panelc_content>");
                sb.Append("<div class=c_jptj>");
                sb.Append("<table width='100%'>");
                int rowcount = 1;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (rowcount % 2 == 1)
                    {
                        sb.Append("<tr >");
                    }

                    sb.Append("<td width='270px' style='padding:5px'>");
                    sb.Append("<a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + dr["SysNo"].ToString() + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr["ProductID"]))).Append("' alt='查看大图' width='80' height='60' border='0'></a><br>");
                    sb.Append("<a href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(dr["sysno"]) + "' target='_blank'>" + Util.TrimNull(dr["productname"]) + "<font color=red>" + Util.TrimNull(dr["PromotionWord"]) + "</font>" + "</a><br />");
                    sb.Append("<span class=color_f60>" + Util.ToMoney(dr["currentprice"].ToString()).ToString(AppConst.DecimalFormatWithCurrency) + "元</span>");
                    sb.Append("</td>");
                    if (rowcount % 2 == 0)
                    {
                        sb.Append("</tr>");
                    }
                    rowcount++;

                }
                sb.Append("</tr></table>");
                //foreach(DataRow dr in ds.Tables[0].Rows)
                //{
                //    sb.Append("<div class=c_jptj_li>");
                //    sb.Append(  "<div class=c_jptj_img>").Append("<a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + dr["SysNo"].ToString() + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr["ProductID"]))).Append("' alt='查看大图' width='80' height='60' border='0'></a>").Append("</div>");
                //    sb.Append(  "<div class=c_jptj_txt><a href='../Items/ItemDetail.aspx?ItemID="+ Util.TrimIntNull(dr["sysno"]) +"' target='_blank'>"+ Util.TrimNull(dr["productname"]) +"</a><br />");
                //    sb.Append(  "<span class=color_f60>"+ Util.ToMoney(dr["currentprice"].ToString()).ToString(AppConst.DecimalFormatWithCurrency) +"元</span></div>");
                //    sb.Append("</div>");
                //}
                sb.Append("<br clear=all />");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</div>");

                return sb.ToString();
            }
            else
                return string.Empty;

        }

        //Judy 2008-4-9 修改
        public string GetC2HotSaleProducts(int c2SysNo)
        {
            Hashtable ht = new Hashtable();
            ht.Add("OnlineAreaType", (int)AppEnum.OnlineAreaType.SecondCategory);
            ht.Add("CategorySysNo", c2SysNo);
            ht.Add("OnlineRecommendType", (int)AppEnum.OnlineRecommendType.HotSale);
            DataSet ds = GetOnlineListProductDs(ht, 8, true, true);
            if (!Util.HasMoreRow(ds))
            {
                string sql = @"select top 8 Product.SysNo as SysNo,Product.C3SysNo as C3SysNo,ProductId, ProductName,PromotionWord, ProductDesc, 
                                left(productdesc,150) as productdescription,AvailableQty+VirtualQty as OnlineQty, Product.Status,
								Product_Price.*,Category1.C1ID, Category2.C2ID, Category3.C3ID
							from 
								Product(nolock),
								Product_Price(nolock),
								Inventory(nolock),
                                Product_SaleTrend(nolock),
								Category3(nolock),
								Category2(nolock),
								Category1(nolock) 
							where Category3.sysno=Product.C3SysNo
							and Category2.sysno=Category3.C2SysNo
							and Category1.sysno=Category2.C1SysNo
							and Inventory.ProductSysNo = Product.SysNo
			                and Product_Price.ProductSysNo = Product.SysNo 
                            and Product_SaleTrend.ProductSysNo = Product.SysNo 
							and Product.Status = 1 and (Product_Price.ClearanceSale=1 or Product_Price.currentprice>=IsNull(Product_Price.unitcost,0))
						    and AvailableQty+VirtualQty>0 and category2.sysno=@c2sysno order by Product_SaleTrend.m1 desc";
                sql = sql.Replace("@c2sysno", c2SysNo.ToString());
                ds = SqlHelper.ExecuteDataSet(sql);
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=panelc>");
            sb.Append("<div class=panelc_title>");
            sb.Append("<img src='../images/site/main/center/tt_tbtj.png' alt='特别推荐' /></div>");
            sb.Append("<div class=panelc_content>");
            sb.Append("<div class=c_jptj>");
            int rowcount = 1;
            sb.Append("<table>");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (rowcount % 2 == 1)
                {
                    sb.Append("<tr>");
                }
                sb.Append("<td width='270px' style='padding:5px'>");
                sb.Append("<a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + dr["SysNo"].ToString() + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr["ProductID"]))).Append("' alt='查看大图' width='80' height='60' border='0'></a>").Append("<br>");
                sb.Append("<a href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(dr["sysno"]) + "' target='_blank'>" + Util.TrimNull(dr["productname"]) + "<font color=red>" + Util.TrimNull(dr["PromotionWord"]) + "</font>" + "</a><br />");
                sb.Append("<span class=color_f60>" + Util.ToMoney(dr["currentprice"].ToString()).ToString(AppConst.DecimalFormatWithCurrency) + "元</span>");
                sb.Append("</td>");
                if (rowcount % 2 == 0)
                {
                    sb.Append("</tr>");
                }
                rowcount++;
            }
            sb.Append("</table>");
            sb.Append("<br clear=all />");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");

            return sb.ToString();
        }

        public string GetC2HotClickProducts(int c2SysNo)
        {
            //            string sql = @"select top 8 p.sysno,p.productname,
            //                            case
            //                                when datediff(day,p.createtime,getdate()) = 0 then sum(dc.clickcount) 
            //                                when p.createtime <  @fromdate then sum(dc.clickcount)/(datediff(day,@fromdate,getdate()))  
            //                                when p.createtime >= @fromdate then sum(dc.clickcount)/(datediff(day,p.createtime,getdate()))
            //                            end as clicknum
            //                            from product_dailyclick dc 
            //                            inner join product p on dc.productsysno = p.sysno 
            //                            inner join category3 c3 on p.c3sysno=c3.sysno
            //                            inner join category2 c2 on c3.c2sysno=c2.sysno
            //                            inner join category1 c1 on c2.c1sysno=c1.sysno
            //                            where p.status=@status and c2.sysno=@c2sysno and dc.clickdate >= '@fromdate'
            //                            group by p.sysno,p.productname,p.createtime 
            //                            order by clicknum desc ";
            string sql = @"select top 8 p.sysno,p.productname,p.PromotionWord,p.avgdailyclick as clicknum from product p(nolock) 
                            inner join category3 c3(nolock) on p.c3sysno=c3.sysno 
                            inner join category2 c2(nolock) on c3.c2sysno=c2.sysno 
                            where p.status=@status and c2.sysno=@c2sysno 
                            group by p.sysno,p.productname,p.PromotionWord,p.avgdailyclick 
                            order by p.avgdailyclick desc ";

            sql = sql.Replace("@status", ((int)AppEnum.ProductStatus.Show).ToString());
            //string fromdate = DateTime.Now.AddMonths(-2).ToString(AppConst.DateFormat);
            //sql = sql.Replace("@fromdate", fromdate);
            sql = sql.Replace("@c2sysno", c2SysNo.ToString());

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return "";

            StringBuilder sb = new StringBuilder();
            sb.Append("<div id=fl_ztlb class=panelr>");
            sb.Append("<div class=panelr_title>");
            sb.Append("<img src='../images/site/main/right/tt_rqphb.png' alt='人气排行榜' /></div>");
            sb.Append("<div class=panelr_content>");
            sb.Append("<div class=c_zxdc>");

            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (i < ds.Tables[0].Rows.Count - 1)
                {
                    sb.Append("<div class=c_rq_li1><a href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(dr["sysno"]) + "' target='_blank'>" + Util.TrimNull(dr["productname"]) + "<font color=red>" + Util.TrimNull(dr["PromotionWord"]) + "</font>" + "</a></div>");
                }
                else
                {
                    sb.Append("<div class=c_rq_li1-2><a href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(dr["sysno"]) + "' target='_blank'>" + Util.TrimNull(dr["productname"]) + "<font color=red>" + Util.TrimNull(dr["PromotionWord"]) + "</font>" + "</a></div>");
                }
                i++;
            }
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");

            return sb.ToString();
        }

        private int GetOnlineC1_C2NewOrderNum(int C1SysNo)
        {
            string sql = "select (IsNull(max(oc.ordernum),0)+1) from OnlineC1_C2 oc(nolock) inner join Category2 c2(nolock) on oc.c2sysno=c2.sysno where c2.c1sysno=" + C1SysNo;
            return Int32.Parse(SqlHelper.ExecuteScalar(sql).ToString());
        }

        public int InsertOnlineC1_C2(int C2SysNo)
        {
            Category2Info c2 = CategoryManager.GetInstance().GetC2Hash()[C2SysNo] as Category2Info;
            int newOrderNum = GetOnlineC1_C2NewOrderNum(c2.C1SysNo);
            string sql = "Insert Into OnlineC1_C2(C2SysNo,OrderNum) values('" + C2SysNo + "','" + newOrderNum + "')";
            return SqlHelper.ExecuteNonQuery(sql);
        }

        private int GetOnlineC1_ProductNewOrderNum(int ProductSysNo)
        {
            string sql = "select c3.c2sysno from product p(nolock) inner join category3 c3 on p.c3sysno=c3.sysno inner join category2 c2 on c3.c2sysno=c2.sysno where p.sysno=" + ProductSysNo;
            int c2SysNo = Int32.Parse(SqlHelper.ExecuteScalar(sql).ToString());
            sql = "select count(cp.productsysno) as pCount from onlineC1_product cp inner join product p on cp.ProductSysNo=p.SysNo inner join category3 c3 on p.c3sysno=c3.sysno inner join category2 c2 on c3.c2sysno=c2.sysno where c2.sysno=" + c2SysNo;
            int pCount = Int32.Parse(SqlHelper.ExecuteScalar(sql).ToString());
            return pCount + 1;
        }

        public void InsertOnlineC1_Product(int ProductSysNo, string ProductBriefName)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                string sql = "select c3.c2sysno from product p(nolock) inner join category3 c3 on p.c3sysno=c3.sysno inner join category2 c2 on c3.c2sysno=c2.sysno where p.sysno=" + ProductSysNo;
                int c2SysNo = Int32.Parse(SqlHelper.ExecuteScalar(sql).ToString());
                sql = "select count(*) from onlineC1_C2 where c2SysNo=" + c2SysNo;
                int c2Count = Int32.Parse(SqlHelper.ExecuteScalar(sql).ToString());
                if (c2Count == 0)
                    InsertOnlineC1_C2(c2SysNo);
                //else
                //{
                //    sql = "select count(oc1p.productsysno) from OnlineC1_Product oc1p inner join product p(nolock) on oc1p.productsysno=p.sysno inner join category3 c3 on p.c3sysno=c3.sysno where c3.c2sysno=" + c2SysNo;
                //    int c2productCount = Int32.Parse(SqlHelper.ExecuteScalar(sql).ToString());
                //    if (c2productCount >= 6)
                //        throw new BizException("每个中类最多只能展示6个商品，请删除部分商品后再添加");
                //}
                int newOrderNum = GetOnlineC1_ProductNewOrderNum(ProductSysNo);
                sql = "insert into onlineC1_Product(ProductSysNo,ProductBriefName,OrderNum) values('" + ProductSysNo + "','" + ProductBriefName + "','" + newOrderNum + "')";
                SqlHelper.ExecuteNonQuery(sql);
                scope.Complete();
            }
        }

        public DataSet GetOnlineC1_ProductDs(int c1SysNo)
        {
            string sql = @"select Product.SysNo as SysNo,Product.C3SysNo as C3SysNo,Category2.SysNo as C2SysNo,ProductId, ProductName,OnlineC1_Product.ProductBriefName,Product.BriefName,
                                AvailableQty+VirtualQty as OnlineQty, Product.Status,Product_Attribute2_Summary.SummaryMain,
								Product_Price.*,Category1.C1ID,Category1.C1Name, Category2.C2ID,Category2.C2Name, Category3.C3ID,Category3.C3Name
							from 
								Product(nolock),
								Product_Price(nolock),
								Inventory(nolock),
                                OnlineC1_Product(nolock),
								Category3(nolock),
								Category2(nolock),
								Category1(nolock),
                                OnlineC1_C2(nolock),
                                Product_Attribute2_Summary(nolock) 
							where Category3.sysno=Product.C3SysNo
							and Category2.sysno=Product.C2SysNo
                            and Category2.sysno=OnlineC1_C2.C2SysNo
							and Category1.sysno=Product.C1SysNo
							and Inventory.ProductSysNo = Product.SysNo
			                and Product_Price.ProductSysNo = Product.SysNo 
                            and Product_Attribute2_Summary.ProductSysNo = Product.SysNo
                            and OnlineC1_Product.ProductSysNo = Product.SysNo 
							and Product.Status = 1 and (Product_Price.ClearanceSale=1 or Product_Price.currentprice>=IsNull(Product_Price.unitcost,0))
						    and AvailableQty+VirtualQty>0 and category1.sysno=@c1sysno order by OnlineC1_C2.OrderNum,OnlineC1_Product.OrderNum";
            sql = sql.Replace("@c1sysno", c1SysNo.ToString());
            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetOnlineC1_ProductDs(string c1SysNoGather)
        {
            string sql = @"select Product.SysNo as SysNo,Product.C3SysNo as C3SysNo,Category2.SysNo as C2SysNo,ProductId, ProductName,OnlineC1_Product.ProductBriefName,Product.BriefName,
                                AvailableQty+VirtualQty as OnlineQty, Product.Status,Product_Attribute2_Summary.SummaryMain,
								Product_Price.*,Category1.C1ID,Category1.C1Name, Category2.C2ID,Category2.C2Name, Category3.C3ID,Category3.C3Name
							from 
								Product(nolock),
								Product_Price(nolock),
								Inventory(nolock),
                                OnlineC1_Product(nolock),
								Category3(nolock),
								Category2(nolock),
								Category1(nolock),
                                OnlineC1_C2(nolock),
                                Product_Attribute2_Summary(nolock) 
							where Category3.sysno=Product.C3SysNo
							and Category2.sysno=Product.C2SysNo
                            and Category2.sysno=OnlineC1_C2.C2SysNo
							and Category1.sysno=Product.C1SysNo
							and Inventory.ProductSysNo = Product.SysNo
			                and Product_Price.ProductSysNo = Product.SysNo 
                            and Product_Attribute2_Summary.ProductSysNo = Product.SysNo
                            and OnlineC1_Product.ProductSysNo = Product.SysNo 
							and Product.Status = 1 and (Product_Price.ClearanceSale=1 or Product_Price.currentprice>=IsNull(Product_Price.unitcost,0))
						    and AvailableQty+VirtualQty>0 and category1.sysno in @c1sysno order by OnlineC1_C2.OrderNum,OnlineC1_Product.OrderNum";
            sql = sql.Replace("@c1sysno", "(" + c1SysNoGather.ToString() + ")");
            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetOnlineC1_C2_ProductDs(int c2SysNo)
        {
            string sql = @"select Product.SysNo as SysNo,Product.C3SysNo as C3SysNo,ProductId, ProductName,OnlineC1_Product.ProductBriefName,
                                AvailableQty+VirtualQty as OnlineQty, Product.Status,
								Product_Price.*,Category1.C1ID,Category1.C1Name, Category2.C2ID,Category2.C2Name, Category3.C3ID,Category3.C3Name
							from 
								Product(nolock),
								Product_Price(nolock),
								Inventory(nolock),
                                OnlineC1_Product(nolock),
								Category3(nolock),
								Category2(nolock),
								Category1(nolock),
                                OnlineC1_C2(nolock) 
							where Category3.sysno=Product.C3SysNo
							and Category2.sysno=Product.C2SysNo
                            and Category2.sysno=OnlineC1_C2.C2SysNo
							and Category1.sysno=Product.C1SysNo
							and Inventory.ProductSysNo = Product.SysNo
			                and Product_Price.ProductSysNo = Product.SysNo 
                            and OnlineC1_Product.ProductSysNo = Product.SysNo 
							and Product.Status = 1 and (Product_Price.ClearanceSale=1 or Product_Price.currentprice>=IsNull(Product_Price.unitcost,0))
						    and AvailableQty+VirtualQty>0 and category2.sysno=@c2sysno order by OnlineC1_C2.OrderNum,OnlineC1_Product.OrderNum";
            sql = sql.Replace("@c2sysno", c2SysNo.ToString());
            return SqlHelper.ExecuteDataSet(sql);
        }

        private void map(OnlineC1_C2Info oParam, DataRow tempdr)
        {
            oParam.C2SysNo = Util.TrimIntNull(tempdr["C2SysNo"]);
            oParam.OrderNum = Util.TrimIntNull(tempdr["OrderNum"]);
        }

        public OnlineC1_C2Info LoadOnlineC1_C2(int C2SysNo)
        {
            string sql = "select * from onlineC1_C2(nolock) where c2sysno=" + C2SysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                OnlineC1_C2Info o = new OnlineC1_C2Info();
                map(o, ds.Tables[0].Rows[0]);
                return o;
            }
            else
                return null;
        }

        public SortedList GetOnlineC1_C2List(int C2SysNo)
        {
            Category2Info c2 = CategoryManager.GetInstance().GetC2Hash()[C2SysNo] as Category2Info;
            int c1SysNo = c2.C1SysNo;
            string sql = @"select * from OnlineC1_C2 oc(nolock) inner join category2 c2(nolock) on oc.c2sysno=c2.sysno where c2.c1sysno=" + c1SysNo + " order by ordernum";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;

            SortedList sl = new SortedList(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                OnlineC1_C2Info item = new OnlineC1_C2Info();
                map(item, dr);
                sl.Add(item, null);
            }
            return sl;
        }

        public void MoveTop(OnlineC1_C2Info oParam)
        {
            if (oParam.OrderNum == 1)
            {
                throw new BizException("it's the top one already");
            }
            SortedList sl = GetOnlineC1_C2List(oParam.C2SysNo);

            if (sl == null)
            {
                throw new BizException("no item for this solution");
            }

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                OnlineC1_C2Dac o = new OnlineC1_C2Dac();

                foreach (OnlineC1_C2Info item in sl.Keys)
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

        public void MoveUp(OnlineC1_C2Info oParam)
        {
            if (oParam.OrderNum == 1)
            {
                throw new BizException("it's the first one, can't be moved up");
            }
            SortedList sl = GetOnlineC1_C2List(oParam.C2SysNo);
            if (sl == null)
            {
                throw new BizException("no items");
            }

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                OnlineC1_C2Dac o = new OnlineC1_C2Dac();

                foreach (OnlineC1_C2Info item in sl.Keys)
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

        public void MoveDown(OnlineC1_C2Info oParam)
        {
            SortedList sl = GetOnlineC1_C2List(oParam.C2SysNo);
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
                OnlineC1_C2Dac o = new OnlineC1_C2Dac();

                foreach (OnlineC1_C2Info item in sl.Keys)
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

        public void MoveBottom(OnlineC1_C2Info oParam)
        {
            SortedList sl = GetOnlineC1_C2List(oParam.C2SysNo);
            if (sl == null)
            {
                throw new BizException("no items");
            }

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                OnlineC1_C2Dac o = new OnlineC1_C2Dac();

                foreach (OnlineC1_C2Info item in sl.Keys)
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

        public OnlineC1_ProductInfo LoadOnlineC1_Product(int ProductSysNo)
        {
            string sql = "select * from onlineC1_Product(nolock) where productsysno=" + ProductSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                OnlineC1_ProductInfo o = new OnlineC1_ProductInfo();
                map(o, ds.Tables[0].Rows[0]);
                return o;
            }
            else
                return null;
        }

        private void map(OnlineC1_ProductInfo oParam, DataRow tempdr)
        {
            oParam.ProductSysNo = Util.TrimIntNull(tempdr["ProductSysNo"]);
            oParam.ProductBriefName = Util.TrimNull(tempdr["ProductBriefName"]);
            oParam.OrderNum = Util.TrimIntNull(tempdr["OrderNum"]);
        }

        public SortedList GetOnlineC1_ProductList(int ProductSysNo)
        {
            ProductBasicInfo pb = ProductManager.GetInstance().LoadBasic(ProductSysNo);
            Category3Info c3 = CategoryManager.GetInstance().GetC3Hash()[pb.C3SysNo] as Category3Info;
            Category2Info c2 = CategoryManager.GetInstance().GetC2Hash()[c3.C2SysNo] as Category2Info;

            string sql = @"select op.* from OnlineC1_Product op(nolock) inner join product p(nolock) on op.productsysno=p.sysno 
                           inner join category3 c3(nolock) on p.c3sysno=c3.sysno inner join category2 c2(nolock) on c3.c2sysno=c2.sysno 
                           where c2.sysno=" + c2.SysNo + " order by op.ordernum";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;

            SortedList sl = new SortedList(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                OnlineC1_ProductInfo item = new OnlineC1_ProductInfo();
                map(item, dr);
                sl.Add(item, null);
            }
            return sl;
        }

        public void MoveTop(OnlineC1_ProductInfo oParam)
        {
            if (oParam.OrderNum == 1)
            {
                throw new BizException("it's the top one already");
            }
            SortedList sl = GetOnlineC1_ProductList(oParam.ProductSysNo);

            if (sl == null)
            {
                throw new BizException("no item for this solution");
            }

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                OnlineC1_ProductDac o = new OnlineC1_ProductDac();

                foreach (OnlineC1_ProductInfo item in sl.Keys)
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

        public void MoveUp(OnlineC1_ProductInfo oParam)
        {
            if (oParam.OrderNum == 1)
            {
                throw new BizException("it's the first one, can't be moved up");
            }
            SortedList sl = GetOnlineC1_ProductList(oParam.ProductSysNo);
            if (sl == null)
            {
                throw new BizException("no items");
            }

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                OnlineC1_ProductDac o = new OnlineC1_ProductDac();

                foreach (OnlineC1_ProductInfo item in sl.Keys)
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

        public void MoveDown(OnlineC1_ProductInfo oParam)
        {
            SortedList sl = GetOnlineC1_ProductList(oParam.ProductSysNo);
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
                OnlineC1_ProductDac o = new OnlineC1_ProductDac();

                foreach (OnlineC1_ProductInfo item in sl.Keys)
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

        public void MoveBottom(OnlineC1_ProductInfo oParam)
        {
            SortedList sl = GetOnlineC1_ProductList(oParam.ProductSysNo);
            if (sl == null)
            {
                throw new BizException("no items");
            }

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                OnlineC1_ProductDac o = new OnlineC1_ProductDac();

                foreach (OnlineC1_ProductInfo item in sl.Keys)
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

        public void Delete(OnlineC1_C2Info oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                MoveBottom(oParam);
                new OnlineC1_C2Dac().Delete(oParam);
                DataSet ds = GetOnlineC1_C2_ProductDs(oParam.C2SysNo);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    OnlineC1_ProductInfo o = LoadOnlineC1_Product(Util.TrimIntNull(dr["SysNo"]));
                    new OnlineC1_ProductDac().Delete(o);
                }
                scope.Complete();
            }
        }

        public void Delete(OnlineC1_ProductInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                MoveBottom(oParam);
                new OnlineC1_ProductDac().Delete(oParam);
                scope.Complete();
            }
        }

        public string GetOnlineC1_ProductsDiv2(int c1SysNo)
        {
            DataSet ds = GetOnlineC1_ProductDs(c1SysNo);
            if (!Util.HasMoreRow(ds))
                return "";
            StringBuilder sb = new StringBuilder();
            sb.Append("<div id=fc_cplb class=panelc>");
            sb.Append("<div class=panelc_title>");
            sb.Append("<img src='../images/site/main/center/tt_cplb.png' alt='产品类别' /></div>");
            sb.Append("<div class=panelc_content>");
            sb.Append("<div class=c_cplb>");

            int c2SysNo = 0;
            int i = 0;
            int j = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Util.TrimIntNull(dr["C2SysNo"].ToString()) != c2SysNo)
                {
                    if (i == 1)
                    {
                        sb.Append("</div>");
                        sb.Append("<div class=c_li_intro>");
                        sb.Append("<a href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(ds.Tables[0].Rows[i - 1]["SysNo"].ToString()) + "' target='_blank'>" + Util.TrimNull(ds.Tables[0].Rows[i - 1]["ProductBriefName"]) + " " + Util.ToMoney(ds.Tables[0].Rows[i - 1]["CurrentPrice"].ToString()) + "</a>");
                        sb.Append("<br/ ><br />" + Util.TrimNull(ds.Tables[0].Rows[i - 1]["SummaryMain"].ToString()));
                        sb.Append("</div>");
                        sb.Append("</div>");
                    }
                    else if (i > 1)
                    {
                        sb.Append("</div>");
                        sb.Append("<div class=c_li_intro>");
                        sb.Append("<a href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(ds.Tables[0].Rows[i]["SysNo"].ToString()) + "' target='_blank'>" + Util.TrimNull(ds.Tables[0].Rows[i]["ProductBriefName"]) + " " + Util.ToMoney(ds.Tables[0].Rows[i]["CurrentPrice"].ToString()) + "</a>");
                        sb.Append("<br/ ><br />" + Util.TrimNull(ds.Tables[0].Rows[i]["SummaryMain"].ToString()));
                        sb.Append("</div>");
                        sb.Append("</div>");
                    }
                    if (j == 0)
                        i++;
                    else
                        i = j;

                    c2SysNo = Util.TrimIntNull(dr["C2SysNo"].ToString());
                    sb.Append("<div class=c_cplb_li>");
                    sb.Append("<div class=c_li_title>");
                    sb.Append("<div class=c_li_title_t2></div>");
                    sb.Append("<STRONG><a href='../Items/SecondCategory.aspx?ID=" + c2SysNo + "'>" + Util.TrimNull(dr["C2Name"]) + "</a></STRONG></div>");
                    sb.Append("<div class=c_li_img2><a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + dr["SysNo"].ToString() + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr["ProductID"]))).Append("' alt='查看大图' width='80' height='60' border='0'></a></div>");
                    sb.Append("<div class=c_li_content>");
                }
                else
                {
                    sb.Append("<span><a href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(dr["SysNo"].ToString()) + "' target='_blank'>" + Util.TrimNull(dr["ProductBriefName"]) + " " + Util.ToMoney(Util.TrimDecimalNull(dr["CurrentPrice"].ToString())) + "</a></span>");
                }
                j++;
            }

            sb.Append("</div>");
            sb.Append("<div class=c_li_intro>");
            sb.Append("<a href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(ds.Tables[0].Rows[i]["SysNo"].ToString()) + "' target='_blank'>" + Util.TrimNull(ds.Tables[0].Rows[i]["ProductBriefName"]) + " " + Util.ToMoney(ds.Tables[0].Rows[i]["CurrentPrice"].ToString()) + "</a>");
            sb.Append("<br /><br />" + Util.TrimNull(ds.Tables[0].Rows[i]["SummaryMain"].ToString()));
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");

            return sb.ToString();
        }

        public string GetOnlineC1_ProductsDiv(int c1SysNo)
        {
            DataSet ds = GetOnlineC1_ProductDs(c1SysNo);
            if (!Util.HasMoreRow(ds))
                return "";
            StringBuilder sb = new StringBuilder();
            sb.Append("<div id=fc_cplb class=panelc>");
            sb.Append("<div class=panelc_title>");
            sb.Append("<img src='../images/site/main/center/tt_cplb.png' alt='产品类别' /></div>");
            sb.Append("<div class=panelc_content>");
            sb.Append("<div class=c_cplb>");

            int c2SysNo = 0;
            int i = 0;
            int j = 0;
            int k = 0; //每个中类最多显示6个商品
            int iRowCount = ds.Tables[0].Rows.Count;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Util.TrimIntNull(dr["C2SysNo"].ToString()) != c2SysNo)  //带图片显示商品
                {
                    k = 1;
                    if (i == 1)
                    {
                        sb.Append("</div>");
                        sb.Append("<div class=c_li_intro>");
                        sb.Append("<table>");
                        sb.Append("<tr><td colspan=2><a href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(ds.Tables[0].Rows[i - 1]["SysNo"].ToString()) + "' target='_blank'><strong>" + Util.TrimNull(ds.Tables[0].Rows[i - 1]["BriefName"]) + "</strong></a>&nbsp;&nbsp;&nbsp;<font color='#ff6600'>￥" + (Util.ToMoney(ds.Tables[0].Rows[i - 1]["CurrentPrice"].ToString()) + Util.ToMoney(ds.Tables[0].Rows[i - 1]["CashRebate"].ToString())).ToString() + "</font></td></tr>");
                        sb.Append("<tr><td width=120><a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + ds.Tables[0].Rows[i - 1]["SysNo"] + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(ds.Tables[0].Rows[i - 1]["ProductID"]))).Append("' alt='查看大图' width='120' height='90' border='0'></a></td><td width=100% align=left valign=top>" + Util.TrimNull(ds.Tables[0].Rows[i - 1]["SummaryMain"].ToString()) + "</td></tr>");
                        sb.Append("</table>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                    }
                    else if (i > 1)
                    {
                        sb.Append("</div>");
                        sb.Append("<div class=c_li_intro>");
                        sb.Append("<table>");
                        sb.Append("<tr><td colspan=2><a href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(ds.Tables[0].Rows[i]["SysNo"].ToString()) + "' target='_blank'><strong>" + Util.TrimNull(ds.Tables[0].Rows[i]["BriefName"]) + "</strong></a>&nbsp;&nbsp;&nbsp;<font color='#ff6600'>￥" + Util.ToMoney(ds.Tables[0].Rows[i]["CurrentPrice"].ToString()) + "</font></td></tr>");
                        sb.Append("<tr><td width=120><a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + ds.Tables[0].Rows[i]["SysNo"] + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(ds.Tables[0].Rows[i]["ProductID"]))).Append("' alt='查看大图' width='120' height='90' border='0'></a></td><td width=100% align=left valign=top>" + Util.TrimNull(ds.Tables[0].Rows[i]["SummaryMain"].ToString()) + "</td></tr>");
                        sb.Append("</table>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                    }
                    if (j == 0)
                        i++;
                    else
                        i = j;

                    c2SysNo = Util.TrimIntNull(dr["C2SysNo"].ToString());
                    sb.Append("<div class=c_cplb_li>");
                    sb.Append("<div class=c_li_title>");
                    sb.Append("<div class=c_li_title_t2></div>");
                    sb.Append("<STRONG><a href='../Items/SecondCategory.aspx?ID=" + c2SysNo + "'>" + Util.TrimNull(dr["C2Name"]) + "</a></STRONG></div>");
                    sb.Append("<div class=c_li_content>");
                }
                else //不带图片显示商品
                {
                    k++;
                    if (k <= 6)
                    {
                        sb.Append("<span align=left><img src='../Images/site/dot.gif' />&nbsp;<a href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(dr["SysNo"].ToString()) + "' target='_blank'><font color='#000000'>" + Util.TrimNull(dr["BriefName"]) + "</font></a>&nbsp;&nbsp;&nbsp;<font color='#ff6600'>￥" + Util.ToMoney(Util.TrimDecimalNull(dr["CurrentPrice"].ToString())) + "</font></span>");
                    }
                }
                j++;
            }

            sb.Append("</div>");
            if (iRowCount > 1)
            {
                sb.Append("<div class=c_li_intro>");
                sb.Append("<table>");
                sb.Append("<tr><td colspan=2><a href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(ds.Tables[0].Rows[i]["SysNo"].ToString()) + "' target='_blank'><strong>" + Util.TrimNull(ds.Tables[0].Rows[i]["BriefName"]) + "</strong></a>&nbsp;&nbsp;&nbsp;<font color='#ff6600'>￥" + Util.ToMoney(ds.Tables[0].Rows[i]["CurrentPrice"].ToString()) + "</font></td></tr>");
                sb.Append("<tr><td width=120><a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + ds.Tables[0].Rows[i]["SysNo"].ToString() + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(ds.Tables[0].Rows[i]["ProductID"]))).Append("' alt='查看大图' width='120' height='90' border='0'></a></td><td width=100% align=left valign=top>" + Util.TrimNull(ds.Tables[0].Rows[i]["SummaryMain"].ToString()) + "</td></tr>");
                sb.Append("</table>");
                sb.Append("</div>");
            }
            sb.Append("</div>");

            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");

            return sb.ToString();
        }

        public DataSet GetSecondHandOnLineProductDs(int c2sysno)
        {
            string sql = @"select @top30 Product.sysno as sysno,ProductID,ProductName,BriefName, Product_Price.CurrentPrice,Product_Price.point
                              from product(nolock)
                              inner join Product_Price(nolock) on  product.sysno=Product_Price.ProductSysNo
                              inner join Category3 c3(nolock) on product.c3sysno=c3.sysno
                              left join inventory(nolock) on product.sysno = inventory.productsysno
                              left join Category2 c2(nolock) on c3.c2sysno=c2.sysno
        		              left join Category1 c1(nolock) on c2.c1sysno=c1.sysno
                              where @Status @OnlineQty @c2sysno @ProductType  order by c2id";
            if (c2sysno == 0)
            {
                sql = sql.Replace("@c2sysno", " ");
                sql = sql.Replace("@top30", " top 30");
            }
            else
            {
                sql = sql.Replace("@c2sysno", "and c2.sysno=" + c2sysno);
                sql = sql.Replace("@top30", " ");
            }
            sql = sql.Replace("@Status", " Product.Status=" + (int)AppEnum.ProductStatus.Show);
            sql = sql.Replace("@OnlineQty", "and inventory.AvailableQty+inventory.VirtualQty>=1");
            sql = sql.Replace("@ProductType", "and ProductType=1");

            return SqlHelper.ExecuteDataSet(sql);
        }

        public string GetSecondHandListByC2(int c2sysno, string c1name, string c2name)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div id=fc_tj class=panelc1>");
            sb.Append("<div class=panelc_title_sh>");
            if (c2sysno == 0)
            {
                sb.Append("推荐二手商品");
            }
            else
            {
                sb.Append("二手商品分类 > " + c1name + " > " + c2name);
            }
            sb.Append("</div>");
            sb.Append("<div class=panelc_content>");
            sb.Append("<div class=c_tj>");

            DataSet ds = GetSecondHandOnLineProductDs(c2sysno);
            if (!Util.HasMoreRow(ds))
                return "";

            int rowcount = 1;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (rowcount < 4)
                {
                    sb.Append("<div class=c_tj_li>");
                }
                else
                {
                    sb.Append("<div class='c_tj_li tj_bo'>");
                }
                sb.Append("<div class=c_tj_img>").Append("<a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + dr["SysNo"].ToString() + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr["ProductID"]), true)).Append("' alt='查看大图' width=80 height=60 border='0'></a>").Append("</div>");
                sb.Append("<div class=c_tj_p>").Append("<a href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(dr["sysno"].ToString()) + "' target='_blank'>" + Util.TrimNull(dr["productname"]) + "</a><br />");
                sb.Append("<span class=c_tj_pr>ORS商城价：" + Util.ToMoney(dr["currentprice"].ToString()).ToString(AppConst.DecimalFormatWithCurrency) + "</span><br />");
                if (Util.TrimIntNull(dr["point"].ToString()) > 0)
                {
                    sb.Append("赠送积分：" + Util.TrimIntNull(dr["point"].ToString()));
                }
                sb.Append("</div>");
                sb.Append("</div>");

                if (rowcount % 3 == 0 && rowcount < ds.Tables[0].Rows.Count)
                {
                    sb.Append("<hr />");
                }
                else if (rowcount == ds.Tables[0].Rows.Count)
                {
                    sb.Append("<br clear=all />");
                }
                rowcount++;
            }
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            return sb.ToString();
        }

        public string GetProductRelatedShowListDiv(int MasterProductSysNo)
        {
            string sql = @"select top 6 ProductID,ProductName,BriefName, Product_Price.CurrentPrice,Product_Price.point,RelatedProductSysNo
                              from product_Related(nolock)
                              inner join Product(nolock) on product.sysno=product_Related.RelatedProductSysNo
                              inner join Product_Price(nolock) on  product.sysno=Product_Price.ProductSysNo
                              left join inventory(nolock) on product.sysno = inventory.productsysno

                              where product_Related.MasterProductSysNo=" + MasterProductSysNo + " @Status @ProductStatus @OnlineQty  ";
            sql = sql.Replace("@Status", "and product_Related.Status=" + (int)AppEnum.BiStatus.Valid);
            sql = sql.Replace("@ProductStatus", "and Product.Status=" + (int)AppEnum.ProductStatus.Show);
            sql = sql.Replace("@OnlineQty", "and inventory.AvailableQty+inventory.VirtualQty>=1");
            sql += "order by product_Related.createtime desc";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return "";
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=panelr>");
            sb.Append("<div class=panelr_title> <img src='../images/site/main/center/tt_xgtj.png' alt='相关商品推荐'/></div>");
            sb.Append("<div class=panelr_content>");
            sb.Append("<div  id=dl_p_cm>");
            sb.Append("<table width='100%' border=0 cellspacing=0 cellpadding=0 style='padding:5px'>");
            sb.Append("<tr>");
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (i % 3 != 0 || i == 0)
                {
                    sb.Append("<td valign=top width=30% style='padding-top:5px;padding-botton:5px;'>");
                    sb.Append("<a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + dr["RelatedProductSysNo"].ToString() + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr["ProductID"].ToString()), true)).Append("'alt='查看大图'  border='0'></a>");
                    sb.Append("<br/>");
                    if (Util.TrimNull(dr["BriefName"].ToString()) != AppConst.StringNull)
                    {
                        sb.Append("<a target='_blank' href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(dr["RelatedProductSysNo"].ToString()) + "'>" + Util.TrimNull(dr["BriefName"]) + "</a><br />");
                    }
                    else
                    {
                        sb.Append("<a target='_blank' href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(dr["RelatedProductSysNo"].ToString()) + "'>" + Util.TrimNull(dr["productname"]) + "</a><br />");
                    }
                    sb.Append("<span class=c_tj_pr>ORS商城售价：￥" + Util.ToMoney(Util.TrimDecimalNull(dr["CurrentPrice"].ToString())));
                    if (Util.TrimIntNull(dr["point"].ToString()) > 0)
                    {
                        sb.Append("<br>赠送积分：" + Util.TrimIntNull(dr["point"].ToString()));
                    }
                    sb.Append("</span></div>");
                    sb.Append("</td>");
                    i++;
                }
                else if (i % 3 == 0 && i > 0)
                {
                    sb.Append("</tr><tr>");
                    sb.Append("<td valign=top width=30% style='padding-top:5px;padding-botton:5px;'>");
                    sb.Append("<a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + dr["RelatedProductSysNo"].ToString() + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr["ProductID"].ToString()), true)).Append("'alt='查看大图' border='0'></a>");
                    sb.Append("<br/>");
                    if (Util.TrimNull(dr["BriefName"].ToString()) != AppConst.StringNull)
                    {
                        sb.Append("<a target='_blank' href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(dr["RelatedProductSysNo"].ToString()) + "'>" + Util.TrimNull(dr["BriefName"]) + "</a><br />");
                    }
                    else
                    {
                        sb.Append("<a target='_blank' href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(dr["RelatedProductSysNo"].ToString()) + "'>" + Util.TrimNull(dr["productname"]) + "</a><br />");
                    }
                    sb.Append("<span class=c_tj_pr>ORS商城售价：￥" + Util.ToMoney(Util.TrimDecimalNull(dr["CurrentPrice"].ToString())));
                    if (Util.TrimIntNull(dr["point"].ToString()) > 0)
                    {
                        sb.Append("<br>赠送积分：" + Util.TrimIntNull(dr["point"].ToString()));
                    }
                    sb.Append("</span></div>");
                    sb.Append("</td>");
                    i++;
                }

            }
            sb.Append("</tr>");
            sb.Append("</table>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");

            return sb.ToString();
        }

        public DataSet NewProducts(int c2Sysno, int days)
        {
            string Sql;
            if (c2Sysno == 0)
            {
                Sql = @"SELECT Product.SysNo AS sysno, Product.ProductID, Product.ProductName, Product.BriefName,Product.PromotionWord,  
                           Product_Price.CurrentPrice,Product_Price.CashRebate, Product_Price.Point, c3.C2SysNo FROM NewProducts(nolock) 
                           INNER JOIN product(nolock) on NewProducts.sysno=Product.SysNo
                           INNER JOIN Product_Price(nolock) ON NewProducts.SysNo = Product_Price.ProductSysNo 
                           LEFT OUTER JOIN Category3 AS c3(nolock) ON Product.C3SysNo = c3.SysNo INNER JOIN
                           Inventory (nolock)ON NewProducts.SysNo = Inventory.ProductSysNo INNER JOIN
                           Category2 AS c2(nolock) ON c3.C2SysNo = c2.SysNo INNER JOIN
                           Category1 AS c1(nolock) ON c2.C1SysNo = c1.SysNo
                           where @Status @OnlineQty  @ProductType @days @c2sysno";
            }
            else
            {
                Sql = @"SELECT Product.SysNo AS sysno, Product.ProductID, Product.ProductName, Product.BriefName,Product.PromotionWord,  
                           Product_Price.CurrentPrice,Product_Price.CashRebate, Product_Price.Point, c3.C2SysNo FROM Product(nolock) INNER JOIN
                           Product_Price(nolock) ON Product.SysNo = Product_Price.ProductSysNo INNER JOIN
                           Category3 AS c3(nolock) ON Product.C3SysNo = c3.SysNo INNER JOIN
                           Inventory(nolock) ON Product.SysNo = Inventory.ProductSysNo INNER JOIN
                           Category2 AS c2(nolock) ON c3.C2SysNo = c2.SysNo INNER JOIN
                           Category1 AS c1(nolock) ON c2.C1SysNo = c1.SysNo
                           where @Status @OnlineQty  @ProductType @days @c2sysno";
            }

            if (days > 0)
            {
                Sql = Sql.Replace("@days", "and (DATEDIFF(d, Product.CreateTime, GETDATE()) <=" + days + ")");
            }
            else
            {
                Sql = Sql.Replace("@days", "");
            }

            if (c2Sysno == 0)
            {
                Sql = Sql.Replace("@c2sysno", "");
            }
            else
            {
                Sql = Sql.Replace("@c2sysno", "and c2.sysno=" + c2Sysno);
            }
            Sql = Sql.Replace("@Status", "Product.Status=" + (int)AppEnum.ProductStatus.Show);
            Sql = Sql.Replace("@OnlineQty", "and inventory.AvailableQty+inventory.VirtualQty>=1");
            Sql = Sql.Replace("@ProductType", "and ProductType=" + (int)AppEnum.ProductType.Normal);

            Sql += " ORDER BY Product.CreateTime DESC";
            return SqlHelper.ExecuteDataSet(Sql);
        }


        public DataSet NewProducts(int c2Sysno, int days, int showRowsCount)
        {
            string Sql;
            if (c2Sysno == 0)
            {
                Sql = @"@SELECT Product.SysNo AS sysno, Product.ProductID, Product.ProductName, Product.BriefName,Product.PromotionWord,  
                           Product_Price.CurrentPrice,Product_Price.CashRebate, Product_Price.Point, c3.C2SysNo FROM NewProducts(nolock) 
                           INNER JOIN product(nolock) on NewProducts.sysno=Product.SysNo
                           INNER JOIN Product_Price(nolock) ON NewProducts.SysNo = Product_Price.ProductSysNo 
                           LEFT OUTER JOIN Category3 AS c3(nolock) ON Product.C3SysNo = c3.SysNo LEFT OUTER JOIN
                           Inventory (nolock)ON NewProducts.SysNo = Inventory.ProductSysNo LEFT OUTER JOIN
                           Category2 AS c2(nolock) ON c3.C2SysNo = c2.SysNo LEFT OUTER JOIN
                           Category1 AS c1(nolock) ON c2.C1SysNo = c1.SysNo
                           where @Status @OnlineQty  @ProductType @days @c2sysno";
            }
            else
            {
                Sql = @"@SELECT Product.SysNo AS sysno, Product.ProductID, Product.ProductName, Product.BriefName,Product.PromotionWord,  
                           Product_Price.CurrentPrice,Product_Price.CashRebate, Product_Price.Point, c3.C2SysNo FROM Product(nolock) INNER JOIN
                           Product_Price(nolock) ON Product.SysNo = Product_Price.ProductSysNo INNER JOIN
                           Category3 AS c3(nolock) ON Product.C3SysNo = c3.SysNo LEFT OUTER JOIN
                           Inventory(nolock) ON Product.SysNo = Inventory.ProductSysNo LEFT OUTER JOIN
                           Category2 AS c2(nolock) ON c3.C2SysNo = c2.SysNo LEFT OUTER JOIN
                           Category1 AS c1(nolock) ON c2.C1SysNo = c1.SysNo
                           where @Status @OnlineQty  @ProductType @days @c2sysno";
            }

            if (days > 0)
            {
                Sql = Sql.Replace("@days", "and (DATEDIFF(d, Product.CreateTime, GETDATE()) <=" + days + ")");
            }
            else
            {
                Sql = Sql.Replace("@days", "");
            }

            if (c2Sysno == 0)
            {
                Sql = Sql.Replace("@c2sysno", "");
            }
            else
            {
                Sql = Sql.Replace("@c2sysno", "and c2.sysno=" + c2Sysno);
            }
            if (showRowsCount != 0 && showRowsCount != AppConst.IntNull)
                Sql = Sql.Replace("@SELECT", "SELECT Top " + showRowsCount);
            else
                Sql = Sql.Replace("@SELECT", "SELECT");
            Sql = Sql.Replace("@Status", "Product.Status=" + (int)AppEnum.ProductStatus.Show);
            Sql = Sql.Replace("@OnlineQty", "and inventory.AvailableQty+inventory.VirtualQty>=1");
            Sql = Sql.Replace("@ProductType", "and ProductType=" + (int)AppEnum.ProductType.Normal);

            Sql += " ORDER BY Product.CreateTime DESC";
            return SqlHelper.ExecuteDataSet(Sql);
        }

        /// <summary>
        /// 获取厂商新品
        /// </summary>
        /// <param name="manufacturerSysNo"></param>
        /// <param name="days"></param>
        /// <param name="showRowsCount"></param>
        /// <returns></returns>
        public DataSet GetNewProductsByManufacturer(int manufacturerSysNo, int days, int showRowsCount)
        {
            string Sql;

            Sql = @"@SELECT Product.SysNo AS sysno, Product.ProductID, Product.ProductName, Product.BriefName,Product.PromotionWord,Product.ManufacturerSysNo,  
                           Product_Price.CurrentPrice,Product_Price.CashRebate, Product_Price.Point
                           FROM Product(nolock) 
                           INNER JOIN  Product_Price(nolock) ON Product.SysNo = Product_Price.ProductSysNo
                           INNER JOIN  Category3 AS c3(nolock) ON Product.C3SysNo = c3.SysNo                            
                           LEFT OUTER JOIN  Inventory(nolock) ON Product.SysNo = Inventory.ProductSysNo 
                           WHERE @Status @OnlineQty  @ProductType @days @ManufacturerSysNo";


            if (days > 0)
            {
                Sql = Sql.Replace("@days", "and (DATEDIFF(d, Product.CreateTime, GETDATE()) <=" + days + ")");
            }
            else
            {
                Sql = Sql.Replace("@days", "");
            }

            if (manufacturerSysNo != 0 && manufacturerSysNo != AppConst.IntNull)
            {
                Sql = Sql.Replace("@ManufacturerSysNo", " AND Product.ManufacturerSysNo = " + manufacturerSysNo);
            }
            else
                Sql = Sql.Replace("@ManufacturerSysNo", "");

            if (showRowsCount != 0 && showRowsCount != AppConst.IntNull)
                Sql = Sql.Replace("@SELECT", "SELECT Top " + showRowsCount);
            else
                Sql = Sql.Replace("@SELECT", "SELECT");
            Sql = Sql.Replace("@Status", "Product.Status=" + (int)AppEnum.ProductStatus.Show);
            Sql = Sql.Replace("@OnlineQty", "and inventory.AvailableQty+inventory.VirtualQty>=1");
            Sql = Sql.Replace("@ProductType", "and ProductType=" + (int)AppEnum.ProductType.Normal);

            Sql += " ORDER BY Product.CreateTime DESC";
            return SqlHelper.ExecuteDataSet(Sql);
        }

        /// <summary>
        /// 获取厂商商品
        /// </summary>
        /// <param name="manufacturerSysNo"></param>
        /// <param name="days"></param>
        /// <param name="showRowsCount"></param>
        /// <returns></returns>
        public DataSet GetProductsByManufacturer(int manufacturerSysNo, int showRowsCount, int orderby)
        {
            string Sql;

            Sql = @"@SELECT Product.SysNo AS sysno, Product.ProductID, Product.ProductName, Product.BriefName,Product.PromotionWord,Product.ManufacturerSysNo,  
                           Product_Price.CurrentPrice,Product_Price.CashRebate, Product_Price.Point, c3.C2SysNo 
                           FROM Product(nolock) 
                           INNER JOIN  Product_Price(nolock) ON Product.SysNo = Product_Price.ProductSysNo
                           INNER JOIN  Category3 AS c3(nolock) ON Product.C3SysNo = c3.SysNo                            
                           LEFT OUTER JOIN  Inventory(nolock) ON Product.SysNo = Inventory.ProductSysNo 
                           WHERE @Status @OnlineQty  @ProductType  @ManufacturerSysNo";


            if (manufacturerSysNo != 0 && manufacturerSysNo != AppConst.IntNull)
            {
                Sql = Sql.Replace("@ManufacturerSysNo", " AND Product.ManufacturerSysNo = " + manufacturerSysNo);
            }
            else
                Sql = Sql.Replace("@ManufacturerSysNo", "");

            if (showRowsCount != 0 && showRowsCount != AppConst.IntNull)
                Sql = Sql.Replace("@SELECT", "SELECT Top " + showRowsCount);
            else
                Sql = Sql.Replace("@SELECT", "SELECT");
            Sql = Sql.Replace("@Status", "Product.Status=" + (int)AppEnum.ProductStatus.Show);
            Sql = Sql.Replace("@OnlineQty", "and inventory.AvailableQty+inventory.VirtualQty>=1");
            Sql = Sql.Replace("@ProductType", "and ProductType=" + (int)AppEnum.ProductType.Normal);

            if (orderby == 0)
            {
                Sql += " order by productname";
            }
            else if (orderby == 1)
            {
                Sql += " order by onlineqty desc";
            }
            else if (orderby == 2)
            {
                Sql += " order by currentprice";
            }
            else if (orderby == 3)
            {
                Sql += " order by currentprice desc";
            }
            else if (orderby == 4)
            {
                Sql += " order by manufacturersysno";
            }
            else
            {
                Sql += " order by product.sysno desc";
            }

            Sql += " ORDER BY Product.SysNo DESC";
            return SqlHelper.ExecuteDataSet(Sql);
        }

        public string NewProductsPage(int c2Sysno, int days, int pageSize, int currentPage, string c1name, string c2name)
        {
            DataSet Ds = NewProducts(c2Sysno, days);
            if (!Util.HasMoreRow(Ds))
                return "";
            int TotalPage = Ds.Tables[0].Rows.Count / pageSize;
            if (Ds.Tables[0].Rows.Count % pageSize != 0)
                TotalPage += 1;
            if (currentPage > TotalPage)
                currentPage = 1;
            int RowIndex = 0;
            int RowCount = 1;
            StringBuilder Result = new StringBuilder();
            Result.Append("<div id=fc_tj class=panelc1>");
            Result.Append("<div class=panelc_title_sh>");
            if (c2Sysno == 0)
            {
                Result.Append("推荐新品");
            }
            else
            {
                Result.Append(c1name + " > " + c2name);
            }
            Result.Append("</div>");
            Result.Append("<div class=panelc_content>");
            Result.Append("<div class=c_tj>");

            //Result.Append("<div id=fc_tj class=panelc1>");
            //Result.Append("<div class=panelc_content>");
            //Result.Append("<div class=c_tj>");

            foreach (DataRow dr in Ds.Tables[0].Rows)
            {
                if (RowCount > 30)
                {
                    break;
                }
                if (RowIndex >= (currentPage - 1) * pageSize && RowIndex < currentPage * pageSize)
                {
                    if (RowCount < 4)
                    {
                        Result.Append("<div class=c_tj_li>");
                    }
                    else
                    {
                        Result.Append("<div class='c_tj_li tj_bo'>");
                    }
                    Result.Append("<div class=c_tj_img>").Append("<a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + dr["SysNo"].ToString() + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr["ProductID"]), true)).Append("' alt='查看大图' width=80 height=60 border='0'></a>").Append("</div>");
                    Result.Append("<div class=c_tj_p>").Append("<a href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(dr["sysno"].ToString()) + "' target='_blank'>" + Util.TrimNull(dr["BriefName"]) + "<font color=red>" + Util.TrimNull(dr["PromotionWord"]) + "</font>" + "</a><br />");
                    Result.Append("<span class=c_tj_pr>ORS商城价：" + Util.ToMoney((decimal)dr["currentprice"] + (decimal)dr["cashrebate"]).ToString(AppConst.DecimalFormatWithCurrency) + "</span><br />");
                    if (Util.TrimIntNull(dr["point"].ToString()) > 0)
                    {
                        Result.Append("赠送积分：" + Util.TrimIntNull(dr["point"].ToString()));
                    }
                    Result.Append("</div>");
                    Result.Append("</div>");

                    if (RowCount % 3 == 0 && RowCount < Ds.Tables[0].Rows.Count && RowCount % 30 != 0)
                    {
                        Result.Append("<hr />");
                    }
                    else if (RowCount == Ds.Tables[0].Rows.Count)
                    {
                        Result.Append("<br clear=all />");
                    }
                    RowCount++;
                }
                RowIndex++;
            }
            Result.Append("</div>");
            Result.Append("</div>");
            Result.Append("</div>");
            return Result.ToString();
        }

        public Hashtable NewProductsHash(int c2Sysno, int days)
        {
            DataSet Ds = NewProducts(c2Sysno, days);
            if (!Util.HasMoreRow(Ds))
                return null;
            Hashtable Result = new Hashtable();
            foreach (DataRow dr in Ds.Tables[0].Rows)
            {
                Result.Add(Util.TrimIntNull(dr["sysno"]), null);
            }
            return Result;
        }



        public string GetPromotionProductDs(int promotionMasterSysNo)
        {
            Hashtable ht = new Hashtable();
            ht.Add("PromotionMasterSysNo", promotionMasterSysNo);
            ht.Add("status", (int)AppEnum.BiStatus.Valid);
            ht.Add("MasterStatus", (int)AppEnum.BiStatus.Valid);
            DataSet ds = PromotionManager.GetInstance().GetPromotionGroupDs(ht);
            if (!Util.HasMoreRow(ds))
                return "";

            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                DataSet dsProduct = GetPromotionProductShowListDs(Util.TrimIntNull(dr["SysNo"]));
                //if (Util.HasMoreRow(dsProduct))
                //    return "";

                sb.Append("<div class=panelr_promotion>");
                sb.Append("<div class=panelr_title_promotion>" + Util.TrimNull(dr["GroupName"].ToString()) + " </div>");
                sb.Append("<div>");
                sb.Append("<div>");
                sb.Append("<table width='100%' border=0 cellspacing=0 cellpadding=0 style='padding:5px'>");
                sb.Append("<tr>");
                int i = 0;
                foreach (DataRow dr1 in dsProduct.Tables[0].Rows)
                {
                    if (i % 4 != 0 || i == 0)
                    {
                        sb.Append("<td valign=top width=245px style='padding-top:5px;padding-botton:5px;'>");
                        sb.Append("<table>");
                        sb.Append("<tr><td><a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + dr1["sysno"].ToString() + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr1["ProductID"].ToString()), false)).Append("'alt='查看大图'  border='0'></a></td></tr>");
                        sb.Append("<tr><td  class=promotion_pn><span>ORS商城售价：￥" + Util.ToMoney(Util.TrimDecimalNull((decimal)dr1["CurrentPrice"] + (decimal)dr1["cashrebate"])));
                        if (Util.TrimIntNull(dr1["onlineQty"]) < 1)
                        {
                            sb.Append("<font color=blue>[已售完]</font>");
                        }
                        if (Util.TrimIntNull(dr1["point"].ToString()) > 0)
                        {
                            sb.Append("</span></td></tr><tr><td class=promotion_pn><span >赠送积分：" + Util.TrimIntNull(dr1["point"].ToString()));
                        }
                        if (Util.TrimDecimalNull(dr1["PromotionDiscount"].ToString()) > 0)
                        {
                            sb.Append("</span></td></tr><tr><td class=promotion_pn><span >活动折扣：" + Util.TrimNull(dr1["PromotionDiscount"].ToString()).Substring(2, 2) + " 折");
                        }

                        sb.Append("</span></td></tr>");
                        sb.Append("<tr><td align=left><span><a target='_blank' href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(dr1["sysno"].ToString()) + "'>" + Util.TrimNull(dr1["productname"]) + "<font color=red>" + Util.TrimNull(dr1["PromotionWord"]) + "</font></a></span>");
                        sb.Append("</td></tr></table></td>");
                        i++;
                    }
                    else if (i % 4 == 0 && i > 0 && i < dsProduct.Tables[0].Rows.Count)
                    {
                        sb.Append("</tr><tr><td colspan=4><hr></td></tr><tr>");
                        sb.Append("<td valign=top width=245px style='padding-top:5px;padding-botton:5px;'>");
                        sb.Append("<table>");
                        sb.Append("<tr><td><a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + dr1["sysno"].ToString() + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr1["ProductID"].ToString()), false)).Append("'alt='查看大图'  border='0'></a></td></tr>");
                        sb.Append("<tr><td class=promotion_pn><span>ORS商城售价：￥" + Util.ToMoney(Util.TrimDecimalNull((decimal)dr1["CurrentPrice"] + (decimal)dr1["cashrebate"])));
                        if (Util.TrimIntNull(dr1["onlineQty"]) < 1)
                        {
                            sb.Append("<font color=blue>[已售完]</font>");
                        }
                        if (Util.TrimIntNull(dr1["point"].ToString()) > 0)
                        {
                            sb.Append("</span></td></tr><tr><td class=promotion_pn><span >赠送积分：" + Util.TrimIntNull(dr1["point"].ToString()));
                        }
                        if (Util.TrimDecimalNull(dr1["PromotionDiscount"].ToString()) > 0)
                        {
                            sb.Append("</span></td></tr><tr><td class=promotion_pn><span >活动折扣：" + Util.TrimNull(dr1["PromotionDiscount"].ToString()).Substring(2, 2) + " 折");
                        }
                        sb.Append("</span></td></tr>");
                        sb.Append("<tr><td align=left><span><a target='_blank' href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(dr1["sysno"].ToString()) + "'>" + Util.TrimNull(dr1["productname"]) + "<font color=red>" + Util.TrimNull(dr1["PromotionWord"]) + "</font></a><br /></span>");
                        sb.Append("</td></tr></table></td>");
                        i++;

                    }
                    else if (i % 4 == 0 && i > 0 && i == dsProduct.Tables[0].Rows.Count)
                    {
                        sb.Append("</tr><tr>");
                        sb.Append("<td valign=top width=245px style='padding-top:5px;padding-botton:5px;'>");
                        sb.Append("<table>");
                        sb.Append("<tr><td><a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + dr1["sysno"].ToString() + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr1["ProductID"].ToString()), false)).Append("'alt='查看大图'  border='0'></a></td></tr>");
                        sb.Append("<tr><td class=promotion_pn><span>ORS商城售价：￥" + Util.ToMoney(Util.TrimDecimalNull((decimal)dr1["CurrentPrice"] + (decimal)dr1["cashrebate"])));
                        if (Util.TrimIntNull(dr1["onlineQty"]) < 1)
                        {
                            sb.Append("<font color=blue>[已售完]</font>");
                        }
                        if (Util.TrimIntNull(dr1["point"].ToString()) > 0)
                        {
                            sb.Append("</span></td></tr><tr><td class=promotion_pn><span>赠送积分：" + Util.TrimIntNull(dr1["point"].ToString()));
                        }
                        if (Util.TrimDecimalNull(dr1["PromotionDiscount"].ToString()) > 0)
                        {
                            sb.Append("</span></td></tr><tr><td class=promotion_pn><span>活动折扣：" + Util.TrimNull(dr1["PromotionDiscount"].ToString()).Substring(2, 2) + " 折");
                        }
                        sb.Append("</span></td></tr>");
                        sb.Append("<tr><td align=left><span><a target='_blank' href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(dr1["sysno"].ToString()) + "'>" + "<font color=red>" + Util.TrimNull(dr1["productname"]) + Util.TrimNull(dr1["PromotionWord"]) + "</font></a><br /></span>");
                        sb.Append("</td></tr></table></td>");
                        i++;
                    }
                }
                sb.Append("</tr>");
                sb.Append("</table>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</div>");

            }
            return sb.ToString();
        }

        public DataSet GetPromotionProductShowListDs(int PromotionGroupSysNo)
        {
            string sql = @"select product.sysno,product.ProductID as ProductID,product.ProductName,Product.BriefName,Product.PromotionWord,
                           Product_Price.CurrentPrice, Product_Price.Point ,Product_Price.CashRebate,isnull(pp.PromotionDiscount,0) as PromotionDiscount,
                           (inventory.AvailableQty+inventory.VirtualQty) as OnlineQty  
                          from Promotion_Item_Product pp(nolock) 
                          inner join product(nolock) on pp.productsysno=product.sysno
                          inner join product_Price(nolock) on pp.productsysno=Product_Price.productsysno
                          inner join Inventory(nolock) ON pp.productsysno = Inventory.ProductSysNo
                          where @promotionGroupSysNo @Status @OnlineQty order by pp.ordernum";

            sql = sql.Replace("@promotionGroupSysNo", " pp.PromotionItemGroupSysNo=" + PromotionGroupSysNo);
            sql = sql.Replace("@Status", "and Product.Status=" + (int)AppEnum.ProductStatus.Show);
            //sql = sql.Replace("@OnlineQty", "and inventory.AvailableQty+inventory.VirtualQty>=1");
            sql = sql.Replace("@OnlineQty", "");
            return SqlHelper.ExecuteDataSet(sql);
        }


        public string GetHtmlPromotionProductDs(int promotionMasterSysNo)
        {
            Hashtable ht = new Hashtable();
            ht.Add("PromotionMasterSysNo", promotionMasterSysNo);
            ht.Add("status", (int)AppEnum.BiStatus.Valid);
            ht.Add("MasterStatus", (int)AppEnum.BiStatus.InValid);
            DataSet ds = PromotionManager.GetInstance().GetPromotionGroupDs(ht);
            if (!Util.HasMoreRow(ds))
                return "";

            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                DataSet dsProduct = GetHtmlPromotionProductShowListDs(Util.TrimIntNull(dr["SysNo"]));

                sb.Append("<div class=panelr_promotion>");
                sb.Append("<div class=panelr_title_promotion>" + Util.TrimNull(dr["GroupName"].ToString()) + " </div>");
                sb.Append("<div>");
                sb.Append("<div>");
                sb.Append("<table width='100%' border=0 cellspacing=0 cellpadding=0 style='padding:5px'>");
                sb.Append("<tr>");
                int i = 0;
                foreach (DataRow dr1 in dsProduct.Tables[0].Rows)
                {
                    if (i % 4 != 0 || i == 0)
                    {
                        sb.Append("<td valign=top width=245px style='padding-top:5px;padding-botton:5px;'>");
                        sb.Append("<table>");
                        if (Util.TrimIntNull(dr1["OnlineQty"]) <= 0)
                        {
                            sb.Append("<tr><td><img src=../Images/Promotion/wang.gif></td></tr>");
                        }
                        else
                        {
                            sb.Append("<tr><td><a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + dr1["sysno"].ToString() + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr1["ProductID"].ToString()), true)).Append("'alt='查看大图'  border='0'></a></td></tr>");

                        }
                        sb.Append("<tr><td  class=promotion_pn><span>ORS商城售价：￥" + Util.ToMoney(Util.TrimDecimalNull((decimal)dr1["CurrentPrice"] + (decimal)dr1["cashrebate"])));
                        if (Util.TrimIntNull(dr1["point"].ToString()) > 0)
                        {
                            sb.Append("</span></td></tr><tr><td class=promotion_pn><span >赠送积分：" + Util.TrimIntNull(dr1["point"].ToString()));
                        }
                        if (Util.TrimDecimalNull(dr1["PromotionDiscount"].ToString()) > 0)
                        {
                            sb.Append("</span></td></tr><tr><td class=promotion_pn><span >活动折扣：" + Util.TrimNull(dr1["PromotionDiscount"].ToString()).Substring(2, 2) + " 折");
                        }

                        sb.Append("</span></td></tr>");
                        sb.Append("<tr><td align=left><span><a target='_blank' href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(dr1["sysno"].ToString()) + "'>" + Util.TrimNull(dr1["productname"]) + "<font color=red>" + Util.TrimNull(dr1["PromotionWord"]) + "</font></a></span>");
                        sb.Append("</td></tr></table></td>");
                        i++;
                    }
                    else if (i % 4 == 0 && i > 0 && i < dsProduct.Tables[0].Rows.Count)
                    {
                        sb.Append("</tr><tr><td colspan=4><hr></td></tr><tr>");
                        sb.Append("<td valign=top width=245px style='padding-top:5px;padding-botton:5px;'>");
                        sb.Append("<table id=tbl" + i + ">");
                        if (Util.TrimIntNull(dr1["OnlineQty"]) <= 0)
                        {
                            sb.Append("<tr><td><img src=../Images/Promotion/wang.gif></td></tr>");
                        }
                        else
                        {
                            sb.Append("<tr><td><a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + dr1["sysno"].ToString() + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr1["ProductID"].ToString()), true)).Append("'alt='查看大图'  border='0'></a></td></tr>");

                        } sb.Append("<tr><td class=promotion_pn><span>ORS商城售价：￥" + Util.ToMoney(Util.TrimDecimalNull((decimal)dr1["CurrentPrice"] + (decimal)dr1["cashrebate"])));
                        if (Util.TrimIntNull(dr1["point"].ToString()) > 0)
                        {
                            sb.Append("</span></td></tr><tr><td class=promotion_pn><span >赠送积分：" + Util.TrimIntNull(dr1["point"].ToString()));
                        }
                        if (Util.TrimDecimalNull(dr1["PromotionDiscount"].ToString()) > 0)
                        {
                            sb.Append("</span></td></tr><tr><td class=promotion_pn><span >活动折扣：" + Util.TrimNull(dr1["PromotionDiscount"].ToString()).Substring(2, 2) + " 折");
                        }
                        sb.Append("</span></td></tr>");
                        sb.Append("<tr><td align=left><span><a target='_blank' href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(dr1["sysno"].ToString()) + "'>" + Util.TrimNull(dr1["productname"]) + "<font color=red>" + Util.TrimNull(dr1["PromotionWord"]) + "</font></a><br /></span>");
                        sb.Append("</td></tr></table></td>");
                        i++;

                    }
                    else if (i % 4 == 0 && i > 0 && i == dsProduct.Tables[0].Rows.Count)
                    {
                        sb.Append("</tr><tr>");
                        sb.Append("<td valign=top width=245px style='padding-top:5px;padding-botton:5px;'>");
                        sb.Append("<table>");
                        if (Util.TrimIntNull(dr1["OnlineQty"]) <= 0)
                        {
                            sb.Append("<tr><td><img src=../Images/Promotion/wang.gif></td></tr>");
                        }
                        else
                        {
                            sb.Append("<tr><td><a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + dr1["sysno"].ToString() + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr1["ProductID"].ToString()), true)).Append("'alt='查看大图'  border='0'></a></td></tr>");
                        }
                        sb.Append("<tr><td class=promotion_pn><span>ORS商城售价：￥" + Util.ToMoney(Util.TrimDecimalNull((decimal)dr1["CurrentPrice"] + (decimal)dr1["cashrebate"])));
                        if (Util.TrimIntNull(dr1["point"].ToString()) > 0)
                        {
                            sb.Append("</span></td></tr><tr><td class=promotion_pn><span>赠送积分：" + Util.TrimIntNull(dr1["point"].ToString()));
                        }
                        if (Util.TrimDecimalNull(dr1["PromotionDiscount"].ToString()) > 0)
                        {
                            sb.Append("</span></td></tr><tr><td class=promotion_pn><span>活动折扣：" + Util.TrimNull(dr1["PromotionDiscount"].ToString()).Substring(2, 2) + " 折");
                        }
                        sb.Append("</span></td></tr>");
                        sb.Append("<tr><td align=left><span><a target='_blank' href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(dr1["sysno"].ToString()) + "'>" + "<font color=red>" + Util.TrimNull(dr1["productname"]) + Util.TrimNull(dr1["PromotionWord"]) + "</font></a><br /></span>");

                        sb.Append("</td></tr></table></td>");
                        i++;
                    }
                }
                sb.Append("</tr>");
                sb.Append("</table>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</div>");

            }
            return sb.ToString();
        }

        public DataSet GetHtmlPromotionProductShowListDs(int PromotionGroupSysNo)
        {
            string sql = @"select product.sysno,product.ProductID as ProductID,product.ProductName,Product.BriefName,Product.PromotionWord,Product_Price.CurrentPrice, Product_Price.Point ,Product_Price.CashRebate,isnull(pp.PromotionDiscount,0) as PromotionDiscount,(inventory.AvailableQty+inventory.VirtualQty) as OnlineQty 
                          from Promotion_Item_Product pp(nolock) 
                          inner join product(nolock) on pp.productsysno=product.sysno
                          inner join product_Price(nolock) on pp.productsysno=Product_Price.productsysno
                          inner join Inventory(nolock) ON pp.productsysno = Inventory.ProductSysNo
                          where @promotionGroupSysNo @Status  order by pp.ordernum";

            sql = sql.Replace("@promotionGroupSysNo", " pp.PromotionItemGroupSysNo=" + PromotionGroupSysNo);
            sql = sql.Replace("@Status", "and Product.Status=" + (int)AppEnum.ProductStatus.Show);
            return SqlHelper.ExecuteDataSet(sql);
        }

        public string GetAccountCenterPowerfulSale()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div id=fc_tj class=account_panner>");
            sb.Append("<div class=panelc_title>");
            //sb.Append(    "<div class=panelc_more><a href='#'><img src='../images/site/main/left/more.png' /></a></div>");
            sb.Append("<img src='../images/site/main/center/tt_jbrm.png' alt='劲爆热卖' />");
            sb.Append("</div>");
            sb.Append("<div class=panelc_content>");
            sb.Append("<div class=c_tj>");

            Hashtable ht = new Hashtable();
            ht.Add("OnlineAreaType", (int)AppEnum.OnlineAreaType.AccountCenter);
            ht.Add("OnlineRecommendType", (int)AppEnum.OnlineRecommendType.PowerfulSale);
            DataSet ds = GetOnlineListProductDs(ht, 4, true, true);
            if (!Util.HasMoreRow(ds))
                return "";

            int rowcount = 1;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (rowcount < 5)
                {
                    sb.Append("<div class=c_tj_li2>");
                }
                else
                {
                    sb.Append("<div class='c_tj_li tj_bo'>");
                }
                sb.Append("<div class=c_tj_img>").Append("<a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + dr["SysNo"].ToString() + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr["ProductID"]), true)).Append("' alt='查看大图' width=80 height=60 border='0'></a>").Append("</div>");
                sb.Append("<div class=c_tj_p>").Append("<a href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(dr["sysno"].ToString()) + "' target='_blank'>" + Util.TrimNull(dr["BriefName"]) + "<font color=red>" + Util.TrimNull(dr["PromotionWord"]) + "</font>" + "</a><br />");
                sb.Append("<span class=c_tj_pr>ORS商城价：" + (Util.ToMoney(dr["currentprice"].ToString()) + Util.ToMoney(dr["CashRebate"].ToString())).ToString(AppConst.DecimalFormatWithCurrency) + "</span><br />");
                if (Util.TrimIntNull(dr["point"].ToString()) > 0)
                {
                    sb.Append("<span class=c_tj_pr>赠送积分:" + Util.TrimIntNull(dr["point"].ToString())).Append("</span>");
                }
                sb.Append("</div>");
                sb.Append("</div>");

                if (rowcount % 4 == 0 && rowcount > 4)
                {
                    sb.Append("<hr />");
                }
                else if (rowcount == 8)
                {
                    sb.Append("<br clear=all />");
                }

                rowcount++;
            }

            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            return sb.ToString();
        }


        public DataSet GetProductNotifySameCategoryHotSaleDs(int customerSysNo)
        {
            string sql = @"select top 4 Product.sysno as sysno,ProductID,ProductName,BriefName,PromotionWord, Product_Price.CurrentPrice,Product_Price.point,product_price.CashRebate
                              from product(nolock)
                              inner join Product_Price(nolock) on  product.sysno=Product_Price.ProductSysNo
                              left join inventory(nolock) on product.sysno = inventory.productsysno
                              where @Status @OnlineQty  @ProductType and c3sysno in 
                                (select top 1 c3sysno from product inner join Product_Notify pn on pn.productsysno=product.sysno where pn.customersysno=" + customerSysNo + " @onlineShowLimit order by pn.createtime desc) order by isnull(AvgDailyClick,0)";


            sql = sql.Replace("@Status", " Product.Status=" + (int)AppEnum.ProductStatus.Show);
            sql = sql.Replace("@OnlineQty", "and inventory.AvailableQty+inventory.VirtualQty>=1");
            sql = sql.Replace("@ProductType", "and ProductType=" + (int)AppEnum.ProductType.Normal);
            sql = sql.Replace("@onlineShowLimit", onlineShowLimit);


            return SqlHelper.ExecuteDataSet(sql);
        }

        public string GetProductNotifySameCategoryHotSale(int customerSysNo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div id=fc_tj class=account_panner>");
            sb.Append("<div class=panelc_title>");
            //sb.Append(    "<div class=panelc_more><a href='#'><img src='../images/site/main/left/more.png' /></a></div>");
            sb.Append("<img src='../images/site/main/center/tt_tlcx.png' alt='同类热销' />");
            sb.Append("</div>");
            sb.Append("<div class=panelc_content>");
            sb.Append("<div class=c_tj>");

            DataSet ds = GetProductNotifySameCategoryHotSaleDs(customerSysNo);
            if (!Util.HasMoreRow(ds))
                return "";

            int rowcount = 1;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (rowcount < 5)
                {
                    sb.Append("<div class=c_tj_li2>");
                }
                else
                {
                    sb.Append("<div class='c_tj_li tj_bo'>");
                }
                sb.Append("<div class=c_tj_img>").Append("<a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + dr["SysNo"].ToString() + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr["ProductID"]), true)).Append("' alt='查看大图' width=80 height=60 border='0'></a>").Append("</div>");
                sb.Append("<div class=c_tj_p>").Append("<a href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(dr["sysno"].ToString()) + "' target='_blank'>" + Util.TrimNull(dr["BriefName"]) + "<font color=red>" + Util.TrimNull(dr["PromotionWord"]) + "</font>" + "</a><br />");
                sb.Append("<span class=c_tj_pr>ORS商城价：" + (Util.ToMoney(dr["currentprice"].ToString()) + Util.ToMoney(dr["CashRebate"].ToString())).ToString(AppConst.DecimalFormatWithCurrency) + "</span><br />");
                if (Util.TrimIntNull(dr["point"].ToString()) > 0)
                {
                    sb.Append("<span class=c_tj_pr>赠送积分:" + Util.TrimIntNull(dr["point"].ToString())).Append("</span>");
                }
                sb.Append("</div>");
                sb.Append("</div>");

                if (rowcount % 4 == 0 && rowcount > 4)
                {
                    sb.Append("<hr />");
                }
                else if (rowcount == 8)
                {
                    sb.Append("<br clear=all />");
                }

                rowcount++;
            }

            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            return sb.ToString();
        }

        public string GetShoppingCartExcellentRecommend()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div id=fc_tj class=shoppingcart_panner>");
            sb.Append("<div class=panelc_title>");
            //sb.Append(    "<div class=panelc_more><a href='#'><img src='../images/site/main/left/more.png' /></a></div>");
            sb.Append("<img src='../images/site/main/center/tt_cztj.png' alt='超值推荐' />");
            sb.Append("</div>");
            sb.Append("<div class=panelc_content>");
            sb.Append("<div class=c_tj>");

            Hashtable ht = new Hashtable();
            ht.Add("OnlineAreaType", (int)AppEnum.OnlineAreaType.ShoppingCart);
            ht.Add("OnlineRecommendType", (int)AppEnum.OnlineRecommendType.ExcellentRecommend);
            DataSet ds = GetOnlineListProductDs(ht, 4, true, true);
            if (!Util.HasMoreRow(ds))
                return "";

            int rowcount = 1;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (rowcount < 5)
                {
                    sb.Append("<div class=c_tj_li3>");
                }
                else
                {
                    sb.Append("<div class='c_tj_li tj_bo'>");
                }
                sb.Append("<div class=c_tj_img>").Append("<a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + dr["SysNo"].ToString() + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr["ProductID"]), true)).Append("' alt='查看大图' width=80 height=60 border='0'></a>").Append("</div>");
                sb.Append("<div class=c_tj_p>").Append("<a href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(dr["sysno"].ToString()) + "' target='_blank'>" + Util.TrimNull(dr["BriefName"]) + "<font color=red>" + Util.TrimNull(dr["PromotionWord"]) + "</font>" + "</a><br />");
                sb.Append("<span class=c_tj_pr>ORS商城价：" + (Util.ToMoney(dr["currentprice"].ToString()) + Util.ToMoney(dr["CashRebate"].ToString())).ToString(AppConst.DecimalFormatWithCurrency) + "</span><br />");
                if (Util.TrimIntNull(dr["point"].ToString()) > 0)
                {
                    sb.Append("<span class=c_tj_pr>赠送积分:" + Util.TrimIntNull(dr["point"].ToString())).Append("</span>");
                }
                sb.Append("</div>");
                sb.Append("</div>");

                if (rowcount % 4 == 0 && rowcount > 4)
                {
                    sb.Append("<hr />");
                }
                else if (rowcount == 8)
                {
                    sb.Append("<br clear=all />");
                }

                rowcount++;
            }

            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            return sb.ToString();
        }

//        public DataSet GetC3RecommendDivDs(int c3sysno, int productsysno)
//        {
//            string sql = @"select top 3 product.sysno as sysno, ProductID,ProductName,BriefName, Product_Price.CurrentPrice,Product_Price.point
//                              from product(nolock)
//                              inner join Product_Price(nolock) on  product.sysno=Product_Price.ProductSysNo
//                              left join inventory(nolock) on product.sysno = inventory.productsysno
//                              where c3sysno=" + c3sysno + " and product.sysno <> " + productsysno + "   @ProductStatus @OnlineQty @producttype ";
//            sql = sql.Replace("@ProductStatus", " and Product.Status=" + (int)AppEnum.ProductStatus.Show);
//            sql = sql.Replace("@OnlineQty", " and inventory.AvailableQty+inventory.VirtualQty>=1");
//            sql = sql.Replace("@producttype", " and Product.Producttype=" + (int)AppEnum.ProductType.Normal);
//            sql += "order by product.OrderNum ";
//            DataSet ds = SqlHelper.ExecuteDataSet(sql);
//            if (!Util.HasMoreRow(ds))
//                return null;
//            else
//                return ds;
//        }
        public DataSet GetC3RecommendDivDs(int c3sysno, int productsysno)
        {
            string sql = @"select top 3 product.sysno as sysno, ProductID,ProductName,BriefName, Product_Price.CurrentPrice,Product_Price.point
                              from product(nolock)
                              inner join Product_Price(nolock) on  product.sysno=Product_Price.ProductSysNo
                              left join inventory(nolock) on product.sysno = inventory.productsysno
                              where c3sysno=" + c3sysno + " and product.sysno <> " + productsysno + "   @ProductStatus @OnlineQty @producttype ";
            sql = sql.Replace("@ProductStatus", " and Product.Status=" + (int)AppEnum.ProductStatus.Show);
            sql = sql.Replace("@OnlineQty", " and inventory.AvailableQty+inventory.VirtualQty>=1");
            sql = sql.Replace("@producttype", " and Product.Producttype=" + (int)AppEnum.ProductType.Normal);

            //不显示子商品
            sql += " and product.product2ndtype < 2 ";

            sql += "order by product.OrderNum ";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;
            else
                return ds;
        }

        public string GetC3RecommendDiv(int c3sysno, int productsysno)
        {
            string sql = @"select top 3 product.sysno as sysno, ProductID,ProductName,BriefName, Product_Price.CurrentPrice,Product_Price.point
                              from product(nolock)
                              inner join Product_Price(nolock) on  product.sysno=Product_Price.ProductSysNo
                              left join inventory(nolock) on product.sysno = inventory.productsysno
                              where c3sysno=" + c3sysno + " and product.sysno <> " + productsysno + "   @ProductStatus @OnlineQty @producttype ";
            sql = sql.Replace("@ProductStatus", " and Product.Status=" + (int)AppEnum.ProductStatus.Show);
            sql = sql.Replace("@OnlineQty", " and inventory.AvailableQty+inventory.VirtualQty>=1");
            sql = sql.Replace("@producttype", " and Product.Producttype=" + (int)AppEnum.ProductType.Normal);
            sql += "order by product.OrderNum ";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return "";
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=panelr>");
            sb.Append("<div class=panelr_title> <img src='../images/site/main/center/tt_tltj.png' alt='同类推荐'/></div>");
            sb.Append("<div class=panelr_content>");
            sb.Append("<div  id=dl_p_cm>");
            sb.Append("<table width='100%' border=0 cellspacing=0 cellpadding=0 style='padding:5px'>");
            sb.Append("<tr>");
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (i % 3 != 0 || i == 0)
                {
                    sb.Append("<td valign=top width=30% style='padding-top:5px;padding-botton:5px;'>");
                    sb.Append("<a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + dr["sysno"].ToString() + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr["ProductID"].ToString()), true)).Append("'alt='查看大图'  border='0'></a>");
                    sb.Append("<br/>");
                    if (Util.TrimNull(dr["BriefName"].ToString()) != AppConst.StringNull)
                    {
                        sb.Append("<a target='_blank' href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(dr["sysno"].ToString()) + "'>" + Util.TrimNull(dr["BriefName"]) + "</a><br />");
                    }
                    else
                    {
                        sb.Append("<a target='_blank' href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(dr["sysno"].ToString()) + "'>" + Util.TrimNull(dr["productname"]) + "</a><br />");
                    }
                    sb.Append("<span class=c_tj_pr>ORS商城售价：￥" + Util.ToMoney(Util.TrimDecimalNull(dr["CurrentPrice"].ToString())));
                    if (Util.TrimIntNull(dr["point"].ToString()) > 0)
                    {
                        sb.Append("<br/>赠送积分:" + Util.TrimIntNull(dr["point"].ToString()));
                    }
                    sb.Append("</span></div>");
                    sb.Append("</td>");
                    i++;
                }
                else if (i % 3 == 0 && i > 0)
                {
                    sb.Append("</tr><tr>");
                    sb.Append("<td valign=top width=30% style='padding-top:5px;padding-botton:5px;'>");
                    sb.Append("<a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + dr["sysno"].ToString() + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr["ProductID"].ToString()), true)).Append("'alt='查看大图' border='0'></a>");
                    sb.Append("<br/>");
                    if (Util.TrimNull(dr["BriefName"].ToString()) != AppConst.StringNull)
                    {
                        sb.Append("<a target='_blank' href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(dr["sysno"].ToString()) + "'>" + Util.TrimNull(dr["BriefName"]) + "</a><br />");
                    }
                    else
                    {
                        sb.Append("<a target='_blank' href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(dr["sysno"].ToString()) + "'>" + Util.TrimNull(dr["productname"]) + "</a><br />");
                    }
                    sb.Append("<span class=c_tj_pr>ORS商城售价：￥" + Util.ToMoney(Util.TrimDecimalNull(dr["CurrentPrice"].ToString())));
                    if (Util.TrimIntNull(dr["point"].ToString()) > 0)
                    {
                        sb.Append("<BR/>赠送积分:" + Util.TrimIntNull(dr["point"].ToString()));
                    }
                    sb.Append("</span></div>");
                    sb.Append("</td>");
                    i++;
                }

            }
            sb.Append("</tr>");
            sb.Append("</table>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");

            return sb.ToString();
        }


        public Hashtable CountDownProductsHash(int c2Sysno)
        {
            DataSet Ds = CountDownProducts(c2Sysno);
            if (!Util.HasMoreRow(Ds))
                return null;
            Hashtable Result = new Hashtable();
            foreach (DataRow dr in Ds.Tables[0].Rows)
            {
                Result.Add(Util.TrimIntNull(dr["sysno"]), null);
            }
            return Result;
        }
        public DataSet CountDownProducts(int c2Sysno)
        {
            string Sql;
            Sql = @"select product.sysno,product.ProductID as ProductID,product.ProductName,Product.BriefName,Product.PromotionWord,pp.BasicPrice as BasicPrice,
                      sc.CountDownCurrentPrice,sc.SnapShotCurrentPrice,sc.CountDownPoint,sc.SnapShotPoint,sc.CountDownCashRebate,sc.SnapShotCashRebate,(inventory.AvailableQty+inventory.VirtualQty) as onlineQty
                         from Sale_CountDown sc(nolock)
                         left join Product(nolock) on sc.productsysno=product.sysno
                         left join Category3 c3(nolock) on product.c3sysno=c3.sysno
                         left join Category2 c2(nolock) on c3.c2sysno=c2.sysno
		                 left join Category1 c1(nolock) on c2.c1sysno=c1.sysno
                         left join  Inventory (nolock)ON sc.productsysno = Inventory.ProductSysNo
                         left join Product_price pp (nolock) on pp.productsysno=sc.productsysno
                        where 1=1 @CountDownStatus @c2sysno  @Status ";

            Sql = Sql.Replace("@CountDownStatus", "and sc.status=" + (int)AppEnum.CountdownStatus.Running);
            Sql = Sql.Replace("@Status", " and Product.Status=" + (int)AppEnum.ProductStatus.Show);
            //Sql = Sql.Replace("@OnlineQty", "and inventory.AvailableQty+inventory.VirtualQty>=1");

            if (c2Sysno == 0)
            {
                Sql = Sql.Replace("@c2sysno", "");
                //Sql = Sql.Replace("select", "select top 15");
            }
            else
            {
                Sql = Sql.Replace("@c2sysno", "and c2.sysno=" + c2Sysno);
            }

            //Sql += " ORDER BY newid();";
            return SqlHelper.ExecuteDataSet(Sql);
        }

        public string CountDownProductsPage(int c2Sysno, int pageSize, int currentPage, string c1name, string c2name)
        {
            DataSet Ds = CountDownProducts(c2Sysno);
            if (!Util.HasMoreRow(Ds))
                return "";
            int TotalPage = Ds.Tables[0].Rows.Count / pageSize;
            if (Ds.Tables[0].Rows.Count % pageSize != 0)
                TotalPage += 1;
            if (currentPage > TotalPage)
                currentPage = 1;
            int RowIndex = 0;
            int RowCount = 1;
            StringBuilder Result = new StringBuilder();
            Result.Append("<div id=fc_tj class=panelc1>");
            Result.Append("<div class=panelc_title_sh>");
            if (c2Sysno == 0)
            {
                Result.Append("推荐商品");
            }
            else
            {
                Result.Append(c1name + " > " + c2name);
            }
            Result.Append("</div>");
            Result.Append("<div class=panelc_content>");
            Result.Append("<div class=c_tj>");

            //Result.Append("<div id=fc_tj class=panelc1>");
            //Result.Append("<div class=panelc_content>");
            //Result.Append("<div class=c_tj>");

            foreach (DataRow dr in Ds.Tables[0].Rows)
            {
                if (RowCount > 15)
                {
                    break;
                }
                if (RowIndex >= (currentPage - 1) * pageSize && RowIndex < currentPage * pageSize)
                {
                    if (RowCount < 4)
                    {
                        Result.Append("<div class=c_tj_li>");
                    }
                    else
                    {
                        Result.Append("<div class='c_tj_li tj_bo'>");
                    }

                    Result.Append("<div class=c_tj_img>").Append("<a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + dr["SysNo"].ToString() + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr["ProductID"]), true)).Append("' alt='查看大图' width=80 height=60 border='0'></a>").Append("</div>");
                    Result.Append("<div class=c_tj_p>").Append("<a href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(dr["sysno"].ToString()) + "' target='_blank'>" + Util.TrimNull(dr["BriefName"]) + "<font color=red>" + Util.TrimNull(dr["PromotionWord"]) + "</font>" + "</a><br />");
                    Result.Append("<span class=c_tj_pr>市场价：" + Util.ToMoney((decimal)dr["BasicPrice"]) + "</span><br />");
                    Result.Append("<span class=c_tj_pr3>抢购价：" + Util.ToMoney((decimal)dr["CountDownCurrentPrice"]) + "</span><br />");

                    if (Util.TrimIntNull(dr["CountDownPoint"].ToString()) > 0)
                    {
                        Result.Append("<span class=c_tj_pr>赠送积分：" + Util.TrimIntNull(dr["CountDownPoint"].ToString())).Append("</span><br />");
                    }
                    if (Util.TrimIntNull(dr["onlineQty"].ToString()) <= 0)
                    {
                        Result.Append("<span class=c_tj_pr1>抢购完").Append("</span><br />");
                    }
                    else
                    {
                        Result.Append("<span class=c_tj_pr2>热卖中……").Append("</span><br />");
                    }
                    Result.Append("</div>");
                    Result.Append("</div>");

                    if (RowCount % 3 == 0 && RowCount < Ds.Tables[0].Rows.Count && RowCount % 15 != 0)
                    {
                        Result.Append("<hr />");
                    }
                    else if (RowCount == Ds.Tables[0].Rows.Count)
                    {
                        Result.Append("<br clear=all />");
                    }
                    RowCount++;
                }
                RowIndex++;
            }
            Result.Append("</div>");
            Result.Append("</div>");
            Result.Append("</div>");
            return Result.ToString();
        }


        public DataSet GetTop4DailyClickProductDs(int c2sysno)
        {
            string Sql = @"select top 4 product.sysno,product.ProductID as ProductID,product.ProductName,Product.BriefName,Product.PromotionWord, 
                            (inventory.AvailableQty+inventory.VirtualQty) as onlineQty,pp.basicprice,pp.currentprice,pp.CashRebate,pp.Point
                         from Product (nolock)
                         left join Category3 c3(nolock) on product.c3sysno=c3.sysno
                         left join Category2 c2(nolock) on c3.c2sysno=c2.sysno
                         left join  Inventory (nolock)ON product.sysno = Inventory.ProductSysNo
                         left join Product_price pp (nolock) on pp.productsysno=product.sysno
                        where 1=1  @c2sysno  @Status @OnlineQty";
            Sql = Sql.Replace("@c2sysno", " and c2.sysno=" + c2sysno);
            Sql = Sql.Replace("@Status", " and Product.Status=" + (int)AppEnum.ProductStatus.Show);
            Sql = Sql.Replace("@OnlineQty", "and inventory.AvailableQty+inventory.VirtualQty>=1");

            return SqlHelper.ExecuteDataSet(Sql);


        }

//        public DataSet GetProductListByRecommendTypeDs(string RecommendTypeList,string Top)
//        {
//            string sql = @"SELECT Tops Product.SysNo, Product.ProductName, Product.PromotionWord, Product.ProductID, 
//                              Product_Price.CurrentPrice, Product_Price.BasicPrice, 
//                              Product_Price.CashRebate, Product_Price.Point, 
//                              OnlineListProduct.OnlineRecommendType, 
//                              OnlineListProduct.OnlineAreaType, OnlineListProduct.ListOrder,
//                              Inventory.AccountQty, Inventory.AvailableQty, Inventory.VirtualQty
//                        FROM Product(nolock) INNER JOIN
//                              Product_Price(nolock) ON 
//                              Product.SysNo = Product_Price.ProductSysNo INNER JOIN
//                              Inventory(nolock) ON Product.SysNo = Inventory.ProductSysNo INNER JOIN
//                              OnlineListProduct(nolock) ON Product.SysNo = OnlineListProduct.ProductSysNo
//                        where product.status=1 and availableqty+virtualqty > 0
//                              and OnlineAreaType = 1 and OnlineRecommendType in(" + RecommendTypeList + ")";
//            sql += " order by OnlineRecommendType,newid()";
//            sql = sql.Replace("Tops", "top " + Top.ToString());
//            DataSet ds = SqlHelper.ExecuteDataSet(sql);
//            if (!Util.HasMoreRow(ds))
//                return null;
//            else
//                return ds;
//        }

        public DataSet GetProductListByRecommendTypeDs(string RecommendTypeList, string Top)
        {
            string sql = @"SELECT Tops Product.SysNo, Product.ProductName, Product.PromotionWord, Product.ProductID, 
                              Product_Price.CurrentPrice, Product_Price.BasicPrice, 
                              Product_Price.CashRebate, Product_Price.Point, 
                              OnlineListProduct.OnlineRecommendType, 
                              OnlineListProduct.OnlineAreaType, OnlineListProduct.ListOrder,
                              Inventory.AccountQty, Inventory.AvailableQty, Inventory.VirtualQty
                        FROM Product(nolock) INNER JOIN
                              Product_Price(nolock) ON 
                              Product.SysNo = Product_Price.ProductSysNo INNER JOIN
                              Inventory(nolock) ON Product.SysNo = Inventory.ProductSysNo INNER JOIN
                              OnlineListProduct(nolock) ON Product.SysNo = OnlineListProduct.ProductSysNo
                        where product.status=1 and availableqty+virtualqty > 0
and product.SysNo not in (select productsysno from sale_countdown where status = 1 )
                              and OnlineAreaType = 1 and OnlineRecommendType in(" + RecommendTypeList + ")";
            sql += " order by listorder ";

            //注：加上 and product.SysNo not in (select productsysno from sale_countdown where status = 1 ) 限时特卖中的商品不在其它栏目显示

            //            sql += " order by OnlineRecommendType,listorder,newid()";

            sql = sql.Replace("Tops", "top " + Top.ToString());
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;
            else
                return ds;
        }

        
        public string[] GetTabsProductList(string RecommendTypeList)
        {
            string sql = @"SELECT Product.SysNo, Product.ProductName, Product.PromotionWord, Product.ProductID, 
                              Product_Price.CurrentPrice, Product_Price.BasicPrice, 
                              Product_Price.CashRebate, Product_Price.Point, 
                              OnlineListProduct.OnlineRecommendType, 
                              OnlineListProduct.OnlineAreaType, OnlineListProduct.ListOrder,
                              Inventory.AccountQty, Inventory.AvailableQty, Inventory.VirtualQty
                        FROM Product(nolock) INNER JOIN
                              Product_Price(nolock) ON 
                              Product.SysNo = Product_Price.ProductSysNo INNER JOIN
                              Inventory(nolock) ON Product.SysNo = Inventory.ProductSysNo INNER JOIN
                              OnlineListProduct(nolock) ON Product.SysNo = OnlineListProduct.ProductSysNo
                        where product.status=1 and availableqty+virtualqty > 0
                              and OnlineAreaType = 1 and OnlineRecommendType in(" + RecommendTypeList + ")";
            sql += " order by OnlineRecommendType,newid()";

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;

            string[] tab = new string[RecommendTypeList.Split(',').Length];
            int tabIndex = -1; //记录第几个tab
            int rowIndex = 0;  //所有记录的序号
            int rowCount = ds.Tables[0].Rows.Count; //所有记录数
            int recommendType = AppConst.IntNull;
            int rowSubIndex = 0;  //每个tab的记录序号
            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (recommendType != Util.TrimIntNull(dr["OnlineRecommendType"]))
                {
                    recommendType = Util.TrimIntNull(dr["OnlineRecommendType"]);
                    tabIndex++;
                    sb.Remove(0, sb.Length);
                    sb.Append("<div class=c_tj>");
                    rowSubIndex = 1;
                }
                if (rowSubIndex < 3)
                {
                    sb.Append("<div class=c_tj_li>");
                }
                else if (rowSubIndex < 9)
                {
                    sb.Append("<div class='c_tj_li tj_bo'>");
                }
                else  //一个tab里超过8个商品
                {
                    rowSubIndex++;
                    rowIndex++;
                    continue;
                }

                sb.Append("<div class=c_tj_img>").Append("<a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + dr["SysNo"].ToString() + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr["ProductID"]), true)).Append("' alt='查看大图' width=80 height=60 border='0'></a>").Append("</div>");
                sb.Append("<div class=c_tj_p>").Append("<a href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(dr["sysno"].ToString()) + "' target='_blank'>" + Util.TrimNull(dr["productname"]) + "<font color=red>" + Util.TrimNull(dr["PromotionWord"]) + "</font>" + "</a><br />");
                if (Util.ToMoney(dr["basicprice"].ToString()) > Util.ToMoney(dr["currentprice"].ToString()))
                {
                    sb.Append("<span class=c_tj_pr>市场价：" + Util.ToMoney(dr["basicprice"].ToString()).ToString(AppConst.DecimalFormatWithCurrency) + "</span><br/>");
                }
                sb.Append("<span class=c_tj_pr>ORS商城价：" + (Util.ToMoney(dr["currentprice"].ToString()) + Util.ToMoney(dr["CashRebate"].ToString())).ToString(AppConst.DecimalFormatWithCurrency) + "</span><br />");
                if (Util.TrimIntNull(dr["point"].ToString()) > 0)
                {
                    sb.Append("<span class=c_tj_pr>赠送积分:" + Util.TrimIntNull(dr["point"].ToString())).Append("</span>");
                }
                sb.Append("</div>");
                sb.Append("</div>");

                if (rowSubIndex % 2 == 0 && rowSubIndex < 7)
                {
                    sb.Append("<br/><hr />");
                }
                else if (rowSubIndex == 8)
                {
                    sb.Append("<br clear=all />");
                    sb.Append("</div>");
                    tab[tabIndex] = sb.ToString();
                    rowSubIndex++;
                    rowIndex++;
                    continue;
                }

                if (rowIndex + 1 == rowCount) //最后一行
                {
                    sb.Append("<br clear=all />");
                    sb.Append("</div>");
                    tab[tabIndex] = sb.ToString();
                }
                else if (Util.TrimIntNull(ds.Tables[0].Rows[rowIndex + 1]["OnlineRecommendType"]) != Util.TrimIntNull(ds.Tables[0].Rows[rowIndex]["OnlineRecommendType"]))
                {
                    sb.Append("<br clear=all />");
                    sb.Append("</div>");
                    tab[tabIndex] = sb.ToString();
                }

                rowSubIndex++;
                rowIndex++;
            }

            return tab;
        }

        public string GetTabProductList(int OnlineRecommendType)
        {
            StringBuilder sb = new StringBuilder();

            Hashtable ht = new Hashtable();
            ht.Add("OnlineAreaType", (int)AppEnum.OnlineAreaType.HomePage);
            ht.Add("OnlineRecommendType", OnlineRecommendType);
            DataSet ds = GetOnlineListProductDs(ht, 8, true, true);
            if (!Util.HasMoreRow(ds))
                return "";

            sb.Append("<div class=c_tj>");

            int rowcount = 1;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (rowcount < 3)
                {
                    sb.Append("<div class=c_tj_li>");
                }
                else
                {
                    sb.Append("<div class='c_tj_li tj_bo'>");
                }

                sb.Append("<div class=c_tj_img>").Append("<a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + dr["SysNo"].ToString() + "')\"><img src='").Append(ProductBasicInfo.GetSmallPic(AppConfig.PicturePath, Util.TrimNull(dr["ProductID"]), true)).Append("' alt='查看大图' width=80 height=60 border='0'></a>").Append("</div>");
                sb.Append("<div class=c_tj_p>").Append("<a href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(dr["sysno"].ToString()) + "' target='_blank'>" + Util.TrimNull(dr["productname"]) + "<font color=red>" + Util.TrimNull(dr["PromotionWord"]) + "</font>" + "</a><br />");
                if (Util.ToMoney(dr["basicprice"].ToString()) > Util.ToMoney(dr["currentprice"].ToString()))
                {
                    sb.Append("<span class=c_tj_pr>市场价：" + Util.ToMoney(dr["basicprice"].ToString()).ToString(AppConst.DecimalFormatWithCurrency) + "</span><br/>");
                }
                sb.Append("<span class=c_tj_pr>ORS商城价：" + (Util.ToMoney(dr["currentprice"].ToString()) + Util.ToMoney(dr["CashRebate"].ToString())).ToString(AppConst.DecimalFormatWithCurrency) + "</span><br />");
                if (Util.TrimIntNull(dr["point"].ToString()) > 0)
                {
                    sb.Append("<span class=c_tj_pr>赠送积分:" + Util.TrimIntNull(dr["point"].ToString())).Append("</span>");
                }
                sb.Append("</div>");
                sb.Append("</div>");

                if (rowcount % 2 == 0 && rowcount < 7)
                {
                    sb.Append("<br/><hr />");
                }
                else if (rowcount == 8)
                {
                    sb.Append("<br clear=all />");
                }

                rowcount++;
            }
            sb.Append("</div>");
            return sb.ToString();
        }


    }
}