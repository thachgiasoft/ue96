using System;
using System.Data;
using System.Collections;
using System.Text;
using System.IO;

using Icson.Utils;
using System.Transactions;

using Icson.Objects;
using Icson.DBAccess;
using Icson.DBAccess.Solution;
using Icson.Objects.Solution;
using Icson.BLL.Review;

namespace Icson.BLL.Solution
{
    public class DIYManager
    {
        private static DIYManager _instance;
        public static DIYManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DIYManager();
            }
            return _instance;
        }

        public string onlineShowLimit = "and Product.Status = " + ((int)AppEnum.ProductStatus.Show).ToString() + " and (Product_Price.ClearanceSale=1 or Product_Price.currentprice>=IsNull(Product_Price.unitcost,0))";

        /// <summary>
        /// SolutionDetail.aspx 显示html
        /// </summary>
        /// <param name="SlnSysNo">solution sysno</param>
        /// <param name="PrjTypeOrderNum">project type ordernum</param>
        /// <param name="ProjectPicPath">project pic path</param>
        /// <returns></returns>
        public string GetDIYSolutionDetail(int SlnSysNo, int PrjTypeOrderNum, string ProjectPicPath, int InputPrjSysNo, int Is53KF)
        {
            StringBuilder sbTotal = new StringBuilder();   //
            StringBuilder sbHeader = new StringBuilder();  //项目类型导航
            StringBuilder sbLeft = new StringBuilder();    //项目导航
            StringBuilder sbMain = new StringBuilder();    //具体项目

            //找出第一个valid的prjtypeordernum
            string sql = "select top 1 ordernum as prjtypeordernum from prj_type where slnsysno=@slnsysno and status=0 order by ordernum";
            sql = sql.Replace("@slnsysno", SlnSysNo.ToString());
            int FirstPrjTypeOrderNum = Int32.Parse(SqlHelper.ExecuteScalar(sql).ToString());

            if (PrjTypeOrderNum == 0)
            {
                PrjTypeOrderNum = FirstPrjTypeOrderNum;
            }
            bool IsFirstPrjTypeOrderNum = false;
            if (PrjTypeOrderNum == FirstPrjTypeOrderNum)
            {
                IsFirstPrjTypeOrderNum = true;
            }

            PrjTypeInfo oPrjTypeInfo = PrjManager.GetInstance().LoadPrjType(SlnSysNo, PrjTypeOrderNum);
            if (oPrjTypeInfo == null)
                return "";
            int PrjTypeSysNo = oPrjTypeInfo.SysNo;

            sbHeader.Append("<table width=100% border=0 cellspacing=0 cellpadding=0>");
            sbHeader.Append("    <tr>");
            if (IsFirstPrjTypeOrderNum)
            {
                sbHeader.Append("    <td><img src='images/diyimages/diytitle01.jpg' width=251 height=54></td>");
            }
            else
            {
                sbHeader.Append("    <td><img src='images/diyimages/diytitle07.jpg' width=251 height=54></td>");
            }

            SortedList slType = PrjManager.GetInstance().GetPrjTypeList(SlnSysNo);  //获得solution下project type list => 导航
            foreach (PrjTypeInfo oTypeInfo in slType.Keys)
            {
                if (oTypeInfo.Status == (int)AppEnum.BiStatus.Valid)
                {
                    bool IsCurrentPrjTypeOrderNum = false;
                    if (PrjTypeOrderNum == oTypeInfo.OrderNum)
                    {
                        IsCurrentPrjTypeOrderNum = true;
                    }

                    if (IsFirstPrjTypeOrderNum)
                    {
                        if (!IsCurrentPrjTypeOrderNum)
                        {
                            sbHeader.Append("<td><img src='images/diyimages/diytitle03.jpg' width=10 height=54 border=0></td>");
                        }
                    }
                    else
                    {
                        if (FirstPrjTypeOrderNum != oTypeInfo.OrderNum)
                        {
                            if (IsCurrentPrjTypeOrderNum)
                            {
                                sbHeader.Append("<td><img src='images/diyimages/diytitle08.jpg' width=10 height=54 border=0></td>");
                            }
                            else
                            {
                                sbHeader.Append("<td><img src='images/diyimages/diytitle03.jpg' width=10 height=54 border=0></td>");
                            }
                        }
                    }

                    if (IsCurrentPrjTypeOrderNum)
                    {
                        sbHeader.Append("<td valign=top background='images/diyimages/diytitle02bg.jpg'>");
                    }
                    else
                    {
                        sbHeader.Append("<td valign=top background='images/diyimages/diytitle03bg.jpg'>");
                    }

                    sbHeader.Append("    <table width=150 height=40 border=0 cellpadding=0 cellspacing=0>");
                    sbHeader.Append("        <tr>");
                    sbHeader.Append("            <td height=14></td>");
                    sbHeader.Append("        </tr>");
                    sbHeader.Append("        <tr>");
                    sbHeader.Append("            <td align=center><a href=SolutionDetail.aspx?ID=" + SlnSysNo.ToString() + "&Order=" + oTypeInfo.OrderNum.ToString() + "><strong style='font-size: 14px;'><font color='#FFFFFF'>" + Util.TrimNull(oTypeInfo.Name) + "</font></strong></a></td>");
                    sbHeader.Append("        </tr>");
                    sbHeader.Append("    </table>");
                    sbHeader.Append("</td>");
                    if (IsCurrentPrjTypeOrderNum)
                    {
                        sbHeader.Append("<td><img src='images/diyimages/diytitle02.jpg' width=10 height=54 border=0></td>");
                    }
                    else
                    {
                        sbHeader.Append("<td><img src='images/diyimages/diytitle04.jpg' width=10 height=54 border=0></td>");
                    }
                }
            }
            sbHeader.Append("        <td width=100% align=right background='images/diyimages/diytitle05.jpg'><img src='images/diyimages/diytitle06.jpg' width=8 height=54></td>");
            sbHeader.Append("    </tr>");
            sbHeader.Append("</table>");

            DataSet dsLeft = SlnManager.GetInstance().GetSlnLeftNavDs(SlnSysNo);

            sbLeft.Append("<table valign=top width=100% border=0 cellspacing=0 cellpadding=0>");
            sbLeft.Append("    <tr>");
            sbLeft.Append("        <td><img src='images/diyimages/diyside01.jpg' width=158 height=29></td>");
            sbLeft.Append("        <td width=100% align=right background='images/diyimages/diyside02.jpg'><img src='images/diyimages/diyside03.jpg' width=7 height=29></td>");
            sbLeft.Append("    </tr>");
            int i = 1;
            foreach (DataRow drPrjType in dsLeft.Tables[0].Rows)
            {
                if (Util.HasMoreRow(dsLeft.Tables[i]))
                {
                    sbLeft.Append("    <tr>");
                    sbLeft.Append("        <td colspan=2 style='border-top-width: 1px;border-right-width: 1px;border-left-width: 1px;border-right-style: solid;border-left-style: solid;border-right-color: #b6b6b6;border-left-color: #b6b6b6;padding: 10px;'>");
                    sbLeft.Append("             <table width=100% border=0 cellspacing=0 cellpadding=2>");
                    sbLeft.Append("                 <tr>");
                    sbLeft.Append("                    <td style='border-bottom-width: 1px;border-bottom-style: dashed;border-bottom-color: b0b3b5;'><strong><font color='#5fb2cf'>" + Util.TrimNull(drPrjType["prjtypename"]) + "</font></strong></td>");
                    sbLeft.Append("                 </tr>");

                    foreach (DataRow drPrj in dsLeft.Tables[i].Rows)
                    {
                        if (Util.TrimIntNull(drPrj["prjsysno"].ToString()) == InputPrjSysNo)
                        {
                            sbLeft.Append("         <tr style='background-color:InfoBackground'>");
                        }
                        else
                        {
                            sbLeft.Append("         <tr>");
                        }
                        sbLeft.Append("                 <td style='border-bottom-width: 1px;border-bottom-style: dashed;border-bottom-color: b0b3b5;'><img src='images/diyimages/arrow.jpg' width=14 height=16><a href='../DIY/SolutionDetail.aspx?ID=" + SlnSysNo.ToString() + "&Order=" + Util.TrimNull(drPrjType["PrjTypeOrderNum"]) + "&PID=" + Util.TrimNull(drPrj["prjsysno"]) + "#P" + Util.TrimNull(drPrj["prjsysno"]) + "'>" + Util.TrimNull(drPrj["prjname"]) + "</a></td>");
                        sbLeft.Append("             </tr>");
                    }
                    sbLeft.Append("             </table>");
                    sbLeft.Append("        </td>");
                    sbLeft.Append("    </tr>");
                }
                i++;
            }
            sbLeft.Append("    <tr>");
            sbLeft.Append("         <td height=2 background='images/diyimages/diyline02.jpg'><img src='images/diyimages/diyline01.jpg' width=9 height=9></td>");
            sbLeft.Append("         <td height=2 align=right background='images/diyimages/diyline02.jpg'><img src='images/diyimages/diyline03.jpg' width=9 height=9></td>");
            sbLeft.Append("    </tr>");
            sbLeft.Append("</table>");

            DataSet dsMain = new DataSet();
            if (InputPrjSysNo == 0)
            {
                dsMain = PrjManager.GetInstance().GetPrjListDefaultDetailDs(PrjTypeSysNo);
            }
            else
                dsMain = PrjManager.GetInstance().GetPrjDetailDsBySysNo(InputPrjSysNo);

            if (Util.HasMoreRow(dsMain))
            {
                sbMain.Append("<table width=100% border=0 cellspacing=0 cellpadding=2 valign=top>");
                int RowCount = dsMain.Tables[0].Rows.Count;
                int PageSize = 3;
                int PageCount = 1;
                int CurrentRow = 1;
                //decimal widthPercent = 0;
                //if (PageSize < RowCount)
                //{
                //    PageCount = (RowCount / PageSize) + 1;
                //    widthPercent = Util.TrimDecimalNull(100) / Util.TrimDecimalNull(PageSize);
                //}
                //else
                //{
                //    widthPercent = Util.TrimDecimalNull(100) / Util.TrimDecimalNull(RowCount);
                //}

                for (int m = 1; m <= PageCount; m++)
                {
                    sbMain.Append("    <tr>");
                    for (int n = 1; n <= PageSize; n++)
                    {
                        if (CurrentRow > RowCount)
                        {
                            //sbMain.Append("    <td valign=top width=" + widthPercent.ToString("##") + "%></td>");
                            CurrentRow++;
                            continue;
                        }
                        int PrjSysNo = Util.TrimIntNull(dsMain.Tables[0].Rows[CurrentRow - 1]["prjsysno"].ToString());
                        decimal TotalPrice = 0;
                        StringBuilder sbImage = new StringBuilder();

                        sbMain.Append("        <td valign=top width=50%>");
                        sbMain.Append("            <table width=100% valign=top border=0 cellspacing=0 cellpadding=0>");
                        sbMain.Append("                <tr>");
                        sbMain.Append("                    <td width=100% background='images/diyimages/diyline05.jpg'><img src='images/diyimages/diyline04.jpg' width=9 height=9></td>");
                        sbMain.Append("                    <td align=right background='images/diyimages/diyline05.jpg'><img src='images/diyimages/diyline06.jpg' width=9 height=9></td>");
                        sbMain.Append("                </tr>");
                        sbMain.Append("                <tr>");
                        sbMain.Append("                    <td colspan=2 align=center bgcolor='#f1f1f1' style='border-top-width: 1px;border-right-width: 1px;border-left-width: 1px;border-right-style: solid;border-left-style: solid;border-right-color: #b6b6b6;border-left-color: #b6b6b6;padding: 8px;'>");
                        if (PrjSysNo == InputPrjSysNo)
                        {
                            sbMain.Append("<a name='#P" + Util.TrimNull(dsMain.Tables[0].Rows[CurrentRow - 1]["prjsysno"]) + "'><strong style='font-size: 16px;color:red;'>" + Util.TrimNull(dsMain.Tables[0].Rows[CurrentRow - 1]["prjname"].ToString()) + "</strong></a>");
                        }
                        else
                        {
                            sbMain.Append("<strong style='font-size: 14px;'><a name='#P" + Util.TrimNull(dsMain.Tables[0].Rows[CurrentRow - 1]["prjsysno"]) + "'>" + Util.TrimNull(dsMain.Tables[0].Rows[CurrentRow - 1]["prjname"].ToString()) + "</a></strong>");
                        }
                        sbMain.Append("                    </td>");
                        sbMain.Append("                </tr>");
                        sbMain.Append("                <tr>");
                        sbMain.Append("                    <td height=105 colspan=2 align=center style='border-top-width: 1px;border-right-width: 1px;border-left-width: 1px;border-right-style: solid;border-left-style: solid;border-right-color: #b6b6b6;border-left-color: #b6b6b6;padding: 2px;'>");
                        bool PrjectPicExist = false;
                        string ProjectPicFile = ProjectPicPath + PrjSysNo.ToString() + ".jpg";
                        if (File.Exists(ProjectPicFile))
                        {
                            PrjectPicExist = true;
                        }
                        if (PrjectPicExist)  //project pic 存在就显示其project的图片,不存在就显示商品组合图片
                        {
                            ProjectPicFile = AppConfig.PicturePath + "project/" + PrjSysNo.ToString() + ".jpg";
                            sbMain.Append("<img src='" + ProjectPicFile + "' />");
                        }
                        else
                        {
                            sbMain.Append("##Image##");
                        }
                        sbMain.Append("                    </td>");
                        sbMain.Append("                </tr>");
                        if (Util.TrimNull(dsMain.Tables[0].Rows[CurrentRow - 1]["prjdescription"]).Length > 0)
                        {
                            sbMain.Append("            <tr>");
                            sbMain.Append("                <td bgcolor='#f1f1f1' colspan=2 style='border-top-width: 1px;border-right-width: 1px;border-left-width: 1px;border-right-style: solid;border-left-style: solid;border-right-color: #b6b6b6;border-left-color: #b6b6b6;padding: 10px;'>");
                            sbMain.Append("                    <table valign=top height=150 width=100% border=0 cellspacing=0 cellpadding=2>");
                            sbMain.Append("                        <tr>");
                            sbMain.Append("                            <td>");
                            sbMain.Append(Util.TrimNull(dsMain.Tables[0].Rows[CurrentRow - 1]["prjdescription"]));
                            sbMain.Append("                            </td>");
                            sbMain.Append("                        </tr>");
                            sbMain.Append("                    </table>");
                            sbMain.Append("                </td>");
                            sbMain.Append("            </tr>");
                        }
                        sbMain.Append("                <tr>");
                        sbMain.Append("                    <td colspan=2 style='border-top-width: 1px;border-right-width: 1px;border-left-width: 1px;border-right-style: solid;border-left-style: solid;border-right-color: #b6b6b6;border-left-color: #b6b6b6;padding: 10px;'>");
                        sbMain.Append("                        <table valign=top height=900 width=100% border=0 cellspacing=0 cellpadding=2>");
                        foreach (DataRow dr in dsMain.Tables[CurrentRow].Rows)
                        {
                            sbMain.Append("                        <tr>");
                            sbMain.Append("                            <td colspan=2 style='border-bottom-width: 1px;border-bottom-style: dashed;border-bottom-color: b0b3b5;'><strong><font color='#FF6600'>" + Util.TrimNull(dr["slnitemname"].ToString()) + "</font></strong><br>" + "<a target='_blank' href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimNull(dr["productsysno"]) + "'>" + Util.TrimNull(dr["productname"].ToString()) + "<font color=red>" + Util.TrimNull(dr["PromotionWord"]) + "</font>" + "</a>");

                            if (Util.TrimIntNull(dr["defaultqty"].ToString()) > 1)
                            {
                                sbMain.Append(" <strong>(*" + dr["defaultqty"].ToString() + ")</strong>");
                            }
                            sbMain.Append("                            </td>");
                            sbMain.Append("                        </tr>");
                            if (!PrjectPicExist && Util.TrimIntNull(dr["isshowpic"].ToString()) == 0)
                            {
                                sbImage.Append("<img src=" + AppConfig.PicturePath + "small/" + Util.TrimNull(dr["productid"]) + ".jpg />");
                            }
                            TotalPrice += Util.ToMoney(dr["currentprice"].ToString()) * Util.ToMoney(dr["defaultqty"].ToString());
                        }
                        if (!PrjectPicExist)
                        {
                            sbMain.Replace("##Image##", sbImage.ToString());
                        }
                        sbMain.Append("                            <tr>");
                        sbMain.Append("                                <td colspan=2 align=center style='border-bottom-width: 1px;border-bottom-style: dashed;border-bottom-color: b0b3b5;'><strong><font color='#FF6600'>推荐标配总价: ￥" + TotalPrice.ToString() + " </font></strong></td>");
                        sbMain.Append("                            </tr>");
                        sbMain.Append("                            <tr>");
                        sbMain.Append("                                <td width=49% align=right style='border-bottom-width: 1px;border-bottom-style: dashed;border-bottom-color: b0b3b5;'><a href='../DIY/ProjectBuy.aspx?ID=" + PrjSysNo.ToString() + "'><img style='CURSOR: hand' border=0 src='images/diyimages/buynowbt.jpg' width=74 height=23></a></td>");
                        sbMain.Append("                                <td width=51% height=40 style='border-bottom-width: 1px;border-bottom-style: dashed;border-bottom-color: b0b3b5;'><a href='../DIY/ProjectDetail.aspx?ID=" + PrjSysNo.ToString() + "'><img style='CURSOR: hand' border=0 src='images/diyimages/choicebt.jpg' width=68 height=23></a></td>");
                        sbMain.Append("                            </tr>");
                        sbMain.Append("                        </table>");
                        sbMain.Append("                    </td>");
                        sbMain.Append("                </tr>");
                        sbMain.Append("                <tr>");
                        sbMain.Append("                    <td height=2 background='images/diyimages/diyline02.jpg'><img src='images/diyimages/diyline01.jpg' width=9 height=9></td>");
                        sbMain.Append("                    <td height=2 align=right background='images/diyimages/diyline02.jpg'><img src='images/diyimages/diyline03.jpg' width=9 height=9></td>");
                        sbMain.Append("                </tr>");
                        sbMain.Append("            </table>");
                        sbMain.Append("        </td>");


                        //客户评论
                        sbMain.Append("        <td valign=top width=50%>");

                        if (Is53KF == 1)
                        {
                            sbMain.Append("            <table width=100%  text-align=center border=0 cellspacing=0 cellpadding=0 >");
                            sbMain.Append("                <tr><td align=center>");
                            sbMain.Append("<div id='c_53kf_diy' style='padding-bottom:3px;'> ");
                            sbMain.Append("<div class='c_53kf'> ");
                            sbMain.Append("<span id='c_53kf2_diy'>");
                            sbMain.Append("<!--script>document.write(\"<scr\"+\"ipt language=\"javascript\" src=\"http://www.53kf.com/kf.php?arg=stonebu&style=1&keyword='+escape(document.referrer)+'\'></scr'+'ipt>');</script--><a href='#' onclick=\"window.open('http://www.53kf.com/company.php?arg=stonebu&style=2','_blank','height=473,width=703,fullscreen=3,top=200,left=200,status=no,toolbar=no,menubar=no,resizable=no,scrollbars=no,location=no,titlebar=no,fullscreen=no');\"><img src=\" http://www.53kf.com/kfimg.php?arg=stonebu&style=2&on_img=http://www.icson.com/items/image/online_diy2.jpg&off_img=http://www.icson.com/items/image/offline_diy2.jpg \" border=\"0\"></a>");

                            sbMain.Append("</span>");
                            sbMain.Append("</div>");
                            sbMain.Append("</div>");
                            sbMain.Append("                </td></tr>");
                            sbMain.Append("                </table>");
                        }


                        sbMain.Append("            <table width=100% valign=top border=0 cellspacing=0 cellpadding=0>");
                        sbMain.Append("                <tr>");
                        sbMain.Append("                    <td width=100% background='images/diyimages/diyline05.jpg'><img src='images/diyimages/diyline04.jpg' width=9 height=9></td>");
                        sbMain.Append("                    <td align=right background='images/diyimages/diyline05.jpg'><img src='images/diyimages/diyline06.jpg' width=9 height=9></td>");
                        sbMain.Append("                </tr>");
                        sbMain.Append("                <tr>");
                        sbMain.Append("                    <td colspan=2 align=center bgcolor='#f1f1f1' style='border-top-width: 1px;border-right-width: 1px;border-left-width: 1px;border-right-style: solid;border-left-style: solid;border-right-color: #b6b6b6;border-left-color: #b6b6b6;padding: 8px; font-size:14px;'>");
                        sbMain.Append("                     <strong>客户评论</strong></td>");
                        sbMain.Append("                </tr>");

                        sbMain.Append("            <tr>");
                        sbMain.Append("                <td height=20 colspan=2 align=right style='border-top-width: 1px;border-right-width: 1px;border-left-width: 1px;border-right-style: solid;border-left-style: solid;border-right-color: #b6b6b6;border-left-color: #b6b6b6;padding: 2px;'>");
                        sbMain.Append("                    <table valign=top height=20 width=100% border=0 cellspacing=0 cellpadding=2>");
                        sbMain.Append("                        <tr>");
                        sbMain.Append("                            <td>");
                        sbMain.Append("<tr><td><a href='../Review/Remark.aspx?PID=" + Util.TrimNull(dsMain.Tables[0].Rows[CurrentRow - 1]["prjsysno"]) + "'><font color=green>发表评论</font></a></td><tr>");
                        sbMain.Append("                            </td>");
                        sbMain.Append("                        </tr>");
                        sbMain.Append("                    </table>");
                        sbMain.Append("                </td>");
                        sbMain.Append("            </tr>");

                        sbMain.Append("                        <tr>");
                        sbMain.Append("                            <td colspan=2 style='border-bottom-width: 1px;border-bottom-style: dashed;border-bottom-color: b0b3b5;border-right-width: 1px;border-left-width: 1px;border-right-style: solid;border-left-style: solid;border-right-color: #b6b6b6;border-left-color: #b6b6b6;padding: 2px;'>");
                        sbMain.Append("                              <table  valign=top width=100% border=0 cellspacing=0 cellpadding=2>");
                        sbMain.Append(ReviewManager.GetInstance().GetDIYReviewDiv(Util.TrimIntNull(dsMain.Tables[0].Rows[CurrentRow - 1]["prjsysno"])));
                        sbMain.Append("                              </table>");
                        sbMain.Append("</td>");
                        sbMain.Append("</tr>");


                        sbMain.Append("                <tr>");
                        sbMain.Append("                    <td height=2 background='images/diyimages/diyline02.jpg'><img src='images/diyimages/diyline01.jpg' width=9 height=9></td>");
                        sbMain.Append("                    <td height=2 align=right background='images/diyimages/diyline02.jpg'><img src='images/diyimages/diyline03.jpg' width=9 height=9></td>");
                        sbMain.Append("                </tr>");
                        sbMain.Append("            </table>");
                        sbMain.Append("        </td>");






                        CurrentRow++;

                    }
                    sbMain.Append("</tr>");
                    if (m < PageCount)
                    {
                        sbMain.Append("<tr>");
                        sbMain.Append("    <td colspan=" + PageSize.ToString() + "><hr style='border: dotted;' color=black size=1></td>");
                        sbMain.Append("</tr>");
                    }
                }

                //    sbMain.Append("    <tr>");
                //    for (int j = 0; j < dsMain.Tables[0].Rows.Count; j++)
                //    {
                //        decimal widthPercent = Util.TrimDecimalNull(100) / Util.TrimDecimalNull(dsMain.Tables[0].Rows.Count);

                //        int PrjSysNo = Util.TrimIntNull(dsMain.Tables[0].Rows[j]["prjsysno"].ToString());
                //        decimal TotalPrice = 0;
                //        StringBuilder sbImage = new StringBuilder();

                //        sbMain.Append("        <td valign=top width="+ widthPercent.ToString("##") +"%>");
                //        sbMain.Append("            <table width=100% valign=top border=0 cellspacing=0 cellpadding=0>");
                //        sbMain.Append("                <tr>");
                //        sbMain.Append("                    <td width=100% background='images/diyimages/diyline05.jpg'><img src='images/diyimages/diyline04.jpg' width=9 height=9></td>");
                //        sbMain.Append("                    <td align=right background='images/diyimages/diyline05.jpg'><img src='images/diyimages/diyline06.jpg' width=9 height=9></td>");
                //        sbMain.Append("                </tr>");
                //        sbMain.Append("                <tr>");
                //        sbMain.Append("                    <td colspan=2 align=center bgcolor='#f1f1f1' style='border-top-width: 1px;border-right-width: 1px;border-left-width: 1px;border-right-style: solid;border-left-style: solid;border-right-color: #b6b6b6;border-left-color: #b6b6b6;padding: 10px;'><strong style='font-size: 14px;'>" + Util.TrimNull(dsMain.Tables[0].Rows[j]["prjname"].ToString()) + "</strong></td>");
                //        sbMain.Append("                </tr>");
                //        sbMain.Append("                <tr>");
                //        sbMain.Append("                    <td height=110 colspan=2 align=center style='border-top-width: 1px;border-right-width: 1px;border-left-width: 1px;border-right-style: solid;border-left-style: solid;border-right-color: #b6b6b6;border-left-color: #b6b6b6;padding: 2px;'>");
                //        bool PrjectPicExist = false;
                //        string ProjectPicFile = ProjectPicPath + PrjSysNo.ToString() + ".jpg";
                //        if (File.Exists(ProjectPicFile))
                //        {
                //            PrjectPicExist = true;
                //        }
                //        if (PrjectPicExist)  //project pic 存在就显示其project的图片,不存在就显示商品组合图片
                //        {
                //            ProjectPicFile = AppConfig.PicturePath + "project/" + PrjSysNo.ToString() + ".jpg";
                //            sbMain.Append("<img src='" + ProjectPicFile + "' />");
                //        }
                //        else
                //        {
                //            sbMain.Append("##Image##");
                //        }
                //        sbMain.Append("                    </td>");
                //        sbMain.Append("                </tr>");
                //        sbMain.Append("                <tr>");
                //        sbMain.Append("                    <td colspan=2 style='border-top-width: 1px;border-right-width: 1px;border-left-width: 1px;border-right-style: solid;border-left-style: solid;border-right-color: #b6b6b6;border-left-color: #b6b6b6;padding: 10px;'>");
                //        sbMain.Append("                        <table width=100% border=0 cellspacing=0 cellpadding=2>");
                //        foreach (DataRow dr in dsMain.Tables[j + 1].Rows)
                //        {
                //            sbMain.Append("                            <tr>");
                //            sbMain.Append("                                <td colspan=2 style='border-bottom-width: 1px;border-bottom-style: dashed;border-bottom-color: b0b3b5;'><strong><font color='#FF6600'>" + Util.TrimNull(dr["slnitemname"].ToString()) + "</font></strong><br>" + "<a target='_blank' href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimNull(dr["productsysno"]) + "'>" + Util.TrimNull(dr["productname"].ToString()) + "</a>");

                //            if (Util.TrimIntNull(dr["defaultqty"].ToString()) > 1)
                //            {
                //                sbMain.Append(" <strong>(*" + dr["defaultqty"].ToString() + ")</strong>");
                //            }
                //            sbMain.Append("                   </td>");
                //            sbMain.Append("               </tr>");
                //            if (!PrjectPicExist && Util.TrimIntNull(dr["isshowpic"].ToString()) == 0)
                //            {
                //                sbImage.Append("<img src=" + AppConfig.PicturePath + "small/" + Util.TrimNull(dr["productid"]) + ".jpg />");
                //            }
                //            TotalPrice += Util.ToMoney(dr["currentprice"].ToString()) * Util.ToMoney(dr["defaultqty"].ToString());
                //        }
                //        if (!PrjectPicExist)
                //        {
                //            sbMain.Replace("##Image##", sbImage.ToString());
                //        }
                //        sbMain.Append("                            <tr>");
                //        sbMain.Append("                                <td colspan=2 align=center style='border-bottom-width: 1px;border-bottom-style: dashed;border-bottom-color: b0b3b5;'><strong><font color='#FF6600'>推荐标配总价: ￥"+ TotalPrice.ToString() +" </font></strong></td>");
                //        sbMain.Append("                            </tr>");
                //        sbMain.Append("                            <tr>");
                //        sbMain.Append("                                <td width=49% align=right style='border-bottom-width: 1px;border-bottom-style: dashed;border-bottom-color: b0b3b5;'><a href='../DIY/ProjectBuy.aspx?ID=" + PrjSysNo.ToString() + "'><img style='CURSOR: hand' border=0 src='images/diyimages/buynowbt.jpg' width=74 height=23></a></td>");
                //        sbMain.Append("                                <td width=51% height=40 style='border-bottom-width: 1px;border-bottom-style: dashed;border-bottom-color: b0b3b5;'><a href='../DIY/ProjectDetail.aspx?ID=" + PrjSysNo.ToString() + "'><img style='CURSOR: hand' border=0 src='images/diyimages/choicebt.jpg' width=68 height=23></a></td>");
                //        sbMain.Append("                            </tr>");
                //        sbMain.Append("                        </table>");
                //        sbMain.Append("                    </td>");
                //        sbMain.Append("                </tr>");
                //        sbMain.Append("                <tr>");
                //        sbMain.Append("                    <td height=2 background='images/diyimages/diyline02.jpg'><img src='images/diyimages/diyline01.jpg' width=9 height=9></td>");
                //        sbMain.Append("                    <td height=2 align=right background='images/diyimages/diyline02.jpg'><img src='images/diyimages/diyline03.jpg' width=9 height=9></td>");
                //        sbMain.Append("                </tr>");
                //        sbMain.Append("            </table>");
                //        sbMain.Append("        </td>");
                //    } //project end
            }

            //sbMain.Append("    </tr>");
            sbMain.Append("</table>");

            sbTotal.Append(sbHeader.ToString());
            sbTotal.Append("<TABLE width=100% border=0 align=center cellpadding=5 cellspacing=0>");
            sbTotal.Append("    <TR>");
            sbTotal.Append("        <TD width=160 valign=top>");
            sbTotal.Append(sbLeft.ToString());
            sbTotal.Append("        </TD>");
            sbTotal.Append("        <TD valign=top>");
            sbTotal.Append(sbMain.ToString());
            sbTotal.Append("        </TD>");
            sbTotal.Append("    </TR>");
            sbTotal.Append("<TABLE>");
            return sbTotal.ToString();
        }

        public string GetPrjDetailHeaderHtml(int PrjSysNo)
        {
            PrjMasterInfo oMaster = PrjManager.GetInstance().LoadPrjMaster(PrjSysNo);
            if (oMaster == null)
                return "";

            StringBuilder sbHeader = new StringBuilder();  //
            sbHeader.Append("<table border=0 cellspacing=0 cellpadding=0>");
            sbHeader.Append("    <tr>");
            sbHeader.Append("        <td width=33% background='images/diyimages/diytitle12.jpg'><img src='images/diyimages/diytitle11.jpg' width=251 height=54></td>");
            sbHeader.Append("        <td width=200 align=center valign=top background='images/diyimages/diytitle12.jpg'>");
            sbHeader.Append("            <table width=250 border=0 cellspacing=0 cellpadding=0>");
            sbHeader.Append("                <tr>");
            sbHeader.Append("                    <td height=18></td>");
            sbHeader.Append("                </tr>");
            sbHeader.Append("                <tr>");
            sbHeader.Append("                    <td align=center><strong style='font-size: 14px;font color:#000000;'>" + Util.TrimNull(oMaster.Name) + "</strong></td>");
            sbHeader.Append("                </tr>");
            sbHeader.Append("            </table>");
            sbHeader.Append("        </td>");
            sbHeader.Append("        <td width=13><img src='images/diyimages/diytitle13.jpg' width=13 height=54></td>");
            sbHeader.Append("        <td width=100% align=right background='images/diyimages/diytitle05.jpg'><img src='images/diyimages/diytitle06.jpg' width=8 height=54></td>");
            sbHeader.Append("    </tr>");
            sbHeader.Append("</table>");
            return sbHeader.ToString();
        }

        /// <summary>
        /// 获得project detail html for ProjectDetail.aspx
        /// </summary>
        /// <param name="PrjSysNo"></param>
        /// <returns></returns>
        public string GetPrjDetailHtml(int PrjSysNo, string SlnItemPicPath)
        {
            DataSet ds = PrjManager.GetInstance().GetPrjDetailDs(PrjSysNo);
            if (ds == null)
                return "";

            StringBuilder sbTotal = new StringBuilder();

            StringBuilder sbNav = new StringBuilder(); //Navigate to item
            StringBuilder sbMain = new StringBuilder(); //Main

            sbNav.Append("<table width=100% border=0 cellspacing=0 cellpadding=0>");
            sbNav.Append("    <tr>");
            sbNav.Append("        <td background='images/diyimages/diymenu05.jpg'><img src='images/diyimages/diymenu01.jpg' width=14 height=14></td>");
            sbNav.Append("        <td align=right background='images/diyimages/diymenu05.jpg'><img src='images/diyimages/diymenu02.jpg' width=14 height=14></td>");
            sbNav.Append("    </tr>");
            sbNav.Append("    <tr>");
            sbNav.Append("        <td colspan=2 style='border-top-width: 1px;border-right-width: 1px;border-left-width: 1px;border-right-style: solid;border-left-style: solid;border-right-color: #b6b6b6;border-left-color: #b6b6b6;padding: 5px;'>");
            sbNav.Append("            <table width=100% border=0 cellspacing=0 cellpadding=0>");

            sbMain.Append("<table width=100% border=0 cellspacing=0 cellpadding=0>");
            sbMain.Append("   <tr>");
            sbMain.Append("       <td width=12% background='images/diyimages/diyside02.jpg'><img src='images/diyimages/diymain01.jpg' width=83 height=29></td>");
            sbMain.Append("       <td width=70% align=center background='images/diyimages/diyside02.jpg'><img src='images/diyimages/diymain02.jpg' width=45 height=29></td>");
            sbMain.Append("       <td width=11% align=center background='images/diyimages/diyside02.jpg'><img src='images/diyimages/diymain03.jpg' width=45 height=29></td>");
            sbMain.Append("       <td width=6% align=center background='images/diyimages/diyside02.jpg'><img src='images/diyimages/diymain04.jpg' width=45 height=29></td>");
            sbMain.Append("       <td width=1% align=right background='images/diyimages/diyside02.jpg'><img src='images/diyimages/diyside03.jpg' width=7 height=29></td>");
            sbMain.Append("   </tr>");

            sbMain.Append("   <tr>");
            sbMain.Append("       <td height=96 colspan=5 valign=top style='border-top-width: 1px;border-right-width: 1px;border-left-width: 1px;border-right-style: solid;border-left-style: solid;border-right-color: #b6b6b6;border-left-color: #b6b6b6;'>");
            sbMain.Append("           <table width=100% border=1 cellpadding=3 cellspacing=0 bordercolor='#FFFFFF'>");

            int i = 1;
            foreach (DataRow drItem in ds.Tables[0].Rows)  //CPU, motherboard, memory
            {
                if (ds.Tables.Count > i && ds.Tables[i].Rows.Count > 0)
                {
                    if (File.Exists(SlnItemPicPath + Util.TrimNull(drItem["slnitemsysno"]) + ".jpg"))
                    {
                        sbNav.Append("<td align=center><a href=#item" + i.ToString() + "><img src='" + AppConfig.PicturePath + "slnitem/" + Util.TrimNull(drItem["slnitemsysno"]) + ".jpg' border=0 width=40></a></td>");
                        if (i < (ds.Tables.Count - 1))
                        {
                            sbNav.Append("<td><img src='images/diyimages/menubtline.jpg' width=3 height=56></td>");
                        }
                    }
                    sbMain.Append("       <tr>");
                    sbMain.Append("           <td colspan=4 align=center bgcolor='#e2f3c7'><strong><a name=\"item" + i.ToString() + "\">" + Util.TrimNull(drItem["Name"]) + "</a></strong></td>");
                    sbMain.Append("       </tr>");

                    int j = 0;
                    foreach (DataRow drProduct in ds.Tables[i].Rows)
                    {
                        if (Util.TrimIntNull(drProduct["ProductType"]) == (int)AppEnum.ProductType.SecondHand)
                        {
                            continue;
                        }

                        bool IsDefaultProduct = false;
                        if (Util.TrimIntNull(drProduct["ProductSysNo"].ToString()) == Util.TrimIntNull(drItem["DefaultProductSysNo"].ToString()))
                        {
                            IsDefaultProduct = true;
                        }
                        sbMain.Append("   <tr>");
                        sbMain.Append("       <td bgcolor='#f1f1f1'><a href=\"javascript:openDialog('../Items/DisplayPhoto.aspx?ItemID=" + drProduct["ProductSysNo"].ToString() + "')\"><img id=imgProduct" + drProduct["ProductSysNo"].ToString() + " src='" + AppConfig.PicturePath + "small/" + Util.TrimNull(drProduct["ProductID"]) + ".jpg' alt='点击查看大图' width=80 height=60 border=0></a></td>");
                        //sbMain.Append("       <td width=60% bgcolor='#f1f1f1'><input name=radiobutton type=radio value=radiobutton checked>");
                        sbMain.Append("       <td bgcolor='#f1f1f1'>");
                        sbMain.Append("       <input id=rdoProduct" + Util.TrimIntNull(drProduct["ProductSysNo"]).ToString()); //不显示图片 radio id= "rdoProduct" + ProductSysNo
                        if (Util.TrimIntNull(drItem["IsShowPic"]) == 0)  //是否显示图片
                        {
                            sbMain.Append("_" + Util.TrimNull(drProduct["ProductID"]));  //显示图片 radio id= "rdoProduct" + ProductSysNo + "_" + ProductID
                        }
                        sbMain.Append("       name=rdoItem" + Util.TrimIntNull(drItem["prjitemsysno"]).ToString() + " type=radio value=" + Util.TrimIntNull(drProduct["ProductSysNo"]).ToString());
                        if (j == 0 || IsDefaultProduct)
                        {
                            sbMain.Append("       checked ");
                        }
                        sbMain.Append("       onclick=\"CalTotal();\" />");
                        sbMain.Append("       <a title=查看更多信息 id=lblProduct" + Util.TrimIntNull(drProduct["ProductSysNo"].ToString()).ToString() + " href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimIntNull(drProduct["ProductSysNo"]).ToString() + "' target='_blank'>" + Util.TrimNull(drProduct["ProductName"]) + "</a>");
                        sbMain.Append("       </td>");
                        sbMain.Append("       <td align=center bgcolor='#f1f1f1'>");
                        sbMain.Append("       <span id=spanProductPrice" + Util.TrimIntNull(drProduct["ProductSysNo"]).ToString() + "><strong><font color='#FF6600'>￥" + Util.ToMoney(drProduct["CurrentPrice"].ToString()) + "</font></strong></span>");
                        sbMain.Append("       <input id=txtProductPrice" + Util.TrimIntNull(drProduct["ProductSysNo"]).ToString() + " value=\"" + Util.ToMoney(drProduct["CurrentPrice"].ToString()).ToString() + "\" type=text style=\"width:0px;visibility:hidden\" />");
                        sbMain.Append("       <BR></td>");
                        sbMain.Append("       <td align=center bgcolor='#f1f1f1'>");
                        sbMain.Append("            <select");
                        if (j == 0 || IsDefaultProduct)
                        {
                            sbMain.Append("        style=\"visibility:visible\"");
                        }
                        else
                        {
                            sbMain.Append("        style=\"visibility:hidden\"");
                        }
                        sbMain.Append("            onchange=\"CalTotal();\" ");
                        sbMain.Append("            name=selQty" + Util.TrimIntNull(drProduct["ProductSysNo"]).ToString() + " id=selQty" + Util.TrimIntNull(drProduct["ProductSysNo"]).ToString() + ">");
                        if (IsDefaultProduct)
                        {
                            sbMain.Append("            <option value=1 ");
                            if (Util.TrimIntNull(drItem["DefaultQty"].ToString()) == 1)
                            {
                                sbMain.Append("        selected='selected'");
                            }
                            sbMain.Append("            >1</option><option value=2");
                            if (Util.TrimIntNull(drItem["DefaultQty"].ToString()) == 2)
                            {
                                sbMain.Append("        selected='selected'");
                            }
                            sbMain.Append("            >2</option><option value=3");
                            if (Util.TrimIntNull(drItem["DefaultQty"].ToString()) == 3)
                            {
                                sbMain.Append("        selected='selected'");
                            }
                            sbMain.Append("            >3</option><option value=4");
                            if (Util.TrimIntNull(drItem["DefaultQty"].ToString()) == 4)
                            {
                                sbMain.Append("        selected='selected'");
                            }
                            sbMain.Append("            >4</option>");
                        }
                        else
                        {
                            sbMain.Append("            <option selected='selected' value=1>1</option><option value=2>2</option><option value=3>3</option><option value=4>4</option>");
                        }
                        sbMain.Append("            </select>");
                        sbMain.Append("            </td>");
                        sbMain.Append("   </tr>");
                        j++;
                    }
                    sbMain.Append("       <tr>");
                    sbMain.Append("           <td bgcolor='#f1f1f1'>&nbsp;</td>");
                    sbMain.Append("           <td bgcolor='#f1f1f1'>");
                    sbMain.Append("           <input ");
                    if (Util.TrimNull(drItem["DefaultProductSysNo"]) == "0")
                    {
                        sbMain.Append("       checked ");
                    }
                    sbMain.Append("           id=rdoNoProduct" + Util.TrimIntNull(drItem["prjitemsysno"]).ToString() + " name=rdoItem" + Util.TrimIntNull(drItem["prjitemsysno"]).ToString() + " type=radio value=0 ");
                    sbMain.Append("           onclick=\"CalTotal();\" />");
                    sbMain.Append("           <label>不选择" + Util.TrimNull(drItem["Name"]) + "</label>");
                    sbMain.Append("           </td>");
                    sbMain.Append("           <td align=center bgcolor='#f1f1f1'><STRONG><font color='#FF6600'>￥0.00</font></STRONG></td>");
                    sbMain.Append("           <td bgcolor='#f1f1f1'>&nbsp;</td>");
                    sbMain.Append("       </tr>");
                }
                i++;
            }
            sbMain.Append("           </table>");
            sbMain.Append("       </td>");
            sbMain.Append("   </tr>");
            sbMain.Append("   <tr>");
            sbMain.Append("       <td height=2 background='images/diyimages/diyline02.jpg'><img src='images/diyimages/diyline01.jpg' width=9 height=9></td>");
            sbMain.Append("       <td height=2 colspan=4 align=right background='images/diyimages/diyline02.jpg'><img src='images/diyimages/diyline03.jpg' width=9 height=9></td>");
            sbMain.Append("   </tr>");
            sbMain.Append("</table>");

            sbNav.Append("                </tr>");
            sbNav.Append("            </table>");
            sbNav.Append("        </td>");
            sbNav.Append("    </tr>");
            sbNav.Append("    <tr>");
            sbNav.Append("        <td background='images/diyimages/diymenu06.jpg'><img src='images/diyimages/diymenu03.jpg' width=14 height=14></td>");
            sbNav.Append("        <td align=right background='images/diyimages/diymenu06.jpg'><img src='images/diyimages/diymenu04.jpg' width=14 height=14></td>");
            sbNav.Append("    </tr>");
            sbNav.Append("</table>");

            sbTotal.Append(sbNav.ToString());
            sbTotal.Append("<table width=100% border=0 cellspacing=0 cellpadding=0>");
            sbTotal.Append("    <tr>");
            sbTotal.Append("        <td height=5></td>");
            sbTotal.Append("    </tr>");
            sbTotal.Append("</table>");

            sbTotal.Append(sbMain.ToString());
            return sbTotal.ToString();
        }

        public DataSet GetDIYCommendDs()
        {
            string sql = @"select m.sysno,m.name,sum(i.defaultqty * p.currentprice) as totalPrice from prj_master m inner join prj_item i on m.sysno=i.prjsysno 
                            inner join product_price p on i.defaultproductsysno = p.productsysno 
                            where  m.status=0 and i.status=0 and m.sysno in (1,2,3,4,5,6,7,8,9,10,11)
                            group by m.sysno,m.name";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                return ds;
            else
                return null;
        }
    }
}