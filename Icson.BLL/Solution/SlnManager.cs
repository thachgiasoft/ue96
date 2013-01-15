using System;
using System.Data;
using System.Collections;
using System.Text;
using System.IO;

using Icson.Utils;
using System.Transactions;

using Icson.DBAccess;
using Icson.DBAccess.Solution;

using Icson.Objects;
using Icson.Objects.Solution;

namespace Icson.BLL.Solution
{
    public class SlnManager
    {
        private static SlnManager _instance;
        public static SlnManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SlnManager();
            }
            return _instance;
        }

        public string onlineShowLimit = "and Product.Status = " + ((int)AppEnum.ProductStatus.Show).ToString() + " and (Product_Price.ClearanceSale=1 or Product_Price.currentprice>=IsNull(Product_Price.unitcost,0))";

        #region sln_master
        private void mapSlnMaster(SlnMasterInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.ID = Util.TrimNull(tempdr["ID"]);
            oParam.Name = Util.TrimNull(tempdr["Name"]);
            oParam.Title = Util.TrimNull(tempdr["Title"]);
            oParam.Description = Util.TrimNull(tempdr["Description"]);
            oParam.SysUserSysNo = Util.TrimIntNull(tempdr["SysUserSysNo"]);
            oParam.DateStamp = Util.TrimDateNull(tempdr["DateStamp"]);
            oParam.OrderNum = Util.TrimIntNull(tempdr["OrderNum"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
        }

        public int InsertSlnMaster(SlnMasterInfo oParam)
        {
            string sql = "select * from sln_master where ID=" + Util.ToSqlString(oParam.ID);
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                throw new BizException("the same ID exists already");
            return new SlnDac().Insert(oParam);
        }

        public void UpdateSlnMaster(SlnMasterInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                new SlnDac().Update(oParam);
                scope.Complete();
            }
        }

        public SlnMasterInfo LoadSlnMaster(int SysNo)
        {
            string sql = "select * from sln_master where sysno=" + SysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            SlnMasterInfo oParam = new SlnMasterInfo();
            if (Util.HasMoreRow(ds))
                mapSlnMaster(oParam, ds.Tables[0].Rows[0]);
            else
                oParam = null;
            return oParam;
        }

        public DataSet GetSlnMaster(int SysNo)
        {
            string sql = "select sln_master.*,sys_user.username from sln_master inner join sys_user on sln_master.sysusersysno = sys_user.sysno and sln_master.sysno=" + SysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                return ds;
            else
                return null;
        }

        public DataSet GetSlnMasterList()
        {
            string sql = "select sln_master.*,sys_user.username from sln_master inner join sys_user on sln_master.sysusersysno = sys_user.sysno ";
            sql += " order by ordernum";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                return ds;
            else
                return null;
        }

        public int GetSlnMasterNewOrderNum()
        {
            return new SlnDac().GetSlnMasterNewOrderNum();
        }
        #endregion

        #region sln_item
        private void mapSlnItem(SlnItemInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.SlnSysNo = Util.TrimIntNull(tempdr["SlnSysNo"]);
            oParam.ID = Util.TrimNull(tempdr["ID"]);
            oParam.Name = Util.TrimNull(tempdr["Name"]);
            oParam.Title = Util.TrimNull(tempdr["Title"]);
            oParam.Description = Util.TrimNull(tempdr["Description"]);
            oParam.OrderNum = Util.TrimIntNull(tempdr["OrderNum"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
        }

        public int InsertSlnItem(SlnItemInfo oParam)
        {
            string sql = "select * from sln_item where slnsysno = "+ oParam.SlnSysNo +" and ID=" + Util.ToSqlString(oParam.ID);
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                throw new BizException("the same ID exists already");
            return new SlnDac().Insert(oParam);
        }

        public void UpdateSlnItem(SlnItemInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                new SlnDac().Update(oParam);
                scope.Complete();
            }
        }

        public SlnItemInfo LoadSlnItem(int SysNo)
        {
            string sql = "select * from sln_item where sysno=" + SysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            SlnItemInfo oParam = new SlnItemInfo();
            if (Util.HasMoreRow(ds))
                mapSlnItem(oParam, ds.Tables[0].Rows[0]);
            else
                oParam = null;
            return oParam;
        }

        public DataSet GetSlnItemDs(int SlnSysNo, bool IsDetail)
        {
            string sql = "";
            if (IsDetail)
            {
                sql = @"select sln_item.*,sln_item_c3.c3sysno,category3.c3name,sln_item_c3_attr2.c3attr2sysno as attribute2sysno,category_attribute2.attribute2name 
                           from sln_item 
                           left join sln_item_c3 on sln_item.sysno=sln_item_c3.slnitemsysno and sln_item_c3.status=0 
                           left join category3 on sln_item_c3.c3sysno=category3.sysno and category3.status=0 
                           left join sln_item_c3_attr2 on sln_item_c3.sysno = sln_item_c3_attr2.slnitemc3sysno and sln_item_c3_attr2.status=0 
                           left join category_attribute2 on sln_item_c3_attr2.c3attr2sysno = category_attribute2.sysno and category_attribute2.status=0 
                           and sln_item.slnsysno = @slnsysno 
                           order by sln_item.ordernum,category3.c3name,category_attribute2.ordernum ";
            }
            else
            {
                sql = @"select * from sln_item where sln_item.slnsysno = @slnsysno order by ordernum";
            }
            sql = sql.Replace("@slnsysno", SlnSysNo.ToString());

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                return ds;
            }
            else
            {
                return null;
            }
        }

        public DataSet GetSlnItemDs(int SlnSysNo, int Status)
        {
            string sql = @"select * from sln_item where sln_item.slnsysno = @slnsysno and sln_item.status=@status order by ordernum";

            sql = sql.Replace("@slnsysno", SlnSysNo.ToString());
            sql = sql.Replace("@status", Status.ToString());

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                return ds;
            }
            else
            {
                return null;
            }
        }

        public int GetSlnItemNewOrderNum(SlnItemInfo oParam)
        {
            return new SlnDac().GetSlnItemNewOrderNum(oParam);
        }

        public SortedList GetSlnItemList(int SlnSysNo)
        {
            string sql = @"select * from sln_item where slnsysno = @slnsysno order by ordernum";
            sql = sql.Replace("@slnsysno", SlnSysNo.ToString());
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;

            SortedList sl = new SortedList(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                SlnItemInfo item = new SlnItemInfo();
                mapSlnItem(item, dr);
                sl.Add(item, null);
            }
            return sl;
        }

        public void MoveTop(SlnItemInfo oParam)
        {
            if (oParam.OrderNum == 1)
            {
                throw new BizException("it's the top one already");
            }
            SortedList sl = GetSlnItemList(oParam.SlnSysNo);

            if (sl == null)
            {
                throw new BizException("no item for this solution");
            }

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                SlnDac o = new SlnDac();

                foreach (SlnItemInfo item in sl.Keys)
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

        public void MoveUp(SlnItemInfo oParam)
        {
            if (oParam.OrderNum == 1)
            {
                throw new BizException("it's the first one, can't be moved up");
            }
            SortedList sl = GetSlnItemList(oParam.SlnSysNo);
            if (sl == null)
            {
                throw new BizException("no items");
            }

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                SlnDac o = new SlnDac();

                foreach (SlnItemInfo item in sl.Keys)
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

        public void MoveDown(SlnItemInfo oParam)
        {
            SortedList sl = GetSlnItemList(oParam.SlnSysNo);
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
                SlnDac o = new SlnDac();

                foreach (SlnItemInfo item in sl.Keys)
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

        public void MoveBottom(SlnItemInfo oParam)
        {
            SortedList sl = GetSlnItemList(oParam.SlnSysNo);
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

                SlnDac o = new SlnDac();

                foreach (SlnItemInfo item in sl.Keys)
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
        #endregion

        #region sln_item_c3
        private void mapSlnItemC3(SlnItemC3Info oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.SlnItemSysNo = Util.TrimIntNull(tempdr["SlnItemSysNo"]);
            oParam.C3SysNo = Util.TrimIntNull(tempdr["C3SysNo"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
        }

        public int InsertSlnItemC3(SlnItemC3Info oParam)
        {
            string sql = "select * from sln_item_c3 where slnitemsysno = " + oParam.SlnItemSysNo + " and c3sysno = " + oParam.C3SysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                throw new BizException("the same c3 exists already");
            return new SlnDac().Insert(oParam);
        }

        public void UpdateSlnItemC3(SlnItemC3Info oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                new SlnDac().Update(oParam);
                scope.Complete();
            }
        }

        public SlnItemC3Info LoadSlnItemC3(int SysNo)
        {
            string sql = "select * from sln_item_c3 where sysno=" + SysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            SlnItemC3Info oParam = new SlnItemC3Info();
            if (Util.HasMoreRow(ds))
                mapSlnItemC3(oParam, ds.Tables[0].Rows[0]);
            else
                oParam = null;
            return oParam;
        }

        public DataSet GetSlnItemC3Ds(int SlnItemSysNo)
        {
            string sql = @"select sln_item_c3.*,category3.c3name 
                           from sln_item_c3 left join category3 on sln_item_c3.c3sysno=category3.sysno 
                           where slnitemsysno = @slnitemsysno";

            sql = sql.Replace("@slnitemsysno", SlnItemSysNo.ToString());

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                return ds;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region sln_item_c3_attr2
        public int InsertSlnItemC3Attr2(SlnItemC3Attr2Info oParam)
        {
            string sql = "select * from sln_item_c3_attr2 where slnitemc3sysno = " + oParam.SlnItemC3SysNo + " and c3attr2sysno = " + oParam.C3Attr2SysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                oParam.SysNo = Util.TrimIntNull(ds.Tables[0].Rows[0]["sysno"]);
                return new SlnDac().Update(oParam);
            }
            else
            {
                return new SlnDac().Insert(oParam);
            }
        }

        public void UpdateSlnItemC3Attr2(SlnItemC3Attr2Info oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                new SlnDac().Update(oParam);
                scope.Complete();
            }
        }

        public void UpdateSlnItemC3Attr2List(Hashtable ht)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                foreach (SlnItemC3Attr2Info oParam in ht.Values)
                {
                    //new SlnDac().Insert(oParam);
                    InsertSlnItemC3Attr2(oParam);
                }
                scope.Complete();
            }
        }

        private void mapSlnItemC3Attr2Info(SlnItemC3Attr2Info oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.SlnItemC3SysNo = Util.TrimIntNull(tempdr["SlnItemC3SysNo"]);
            oParam.C3Attr2SysNo = Util.TrimIntNull(tempdr["C3Attr2SysNo"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
        }

        public Hashtable GetSlnItemC2Attr2Hash(int SlnItemC3SysNo, int Status)
        {
            string sql = "select c3attr2sysno from sln_item_c3_attr2 where slnitemc3sysno=" + SlnItemC3SysNo + " and status=" + Status;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;

            Hashtable ht = new Hashtable(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ht.Add(Util.TrimIntNull(dr["c3attr2sysno"]), Util.TrimIntNull(dr["c3attr2sysno"]));
            }
            return ht;
        }

        public SortedList GetSlnItemC2Attr2List(int SlnItemC3SysNo)
        {
            string sql = "select * from sln_item_c3_attr2 where slnitemc3sysno=" + SlnItemC3SysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;

            SortedList sl = new SortedList(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                SlnItemC3Attr2Info oParam = new SlnItemC3Attr2Info();
                mapSlnItemC3Attr2Info(oParam, dr);
                sl.Add(oParam.C3Attr2SysNo, oParam);
            }
            return sl;
        }
        #endregion        

        /// <summary>
        /// SolutionDetail.aspx 显示html
        /// </summary>
        /// <param name="SlnSysNo">solution sysno</param>
        /// <param name="PrjTypeOrderNum">project type ordernum</param>
        /// <param name="ProjectPicPath">project pic path</param>
        /// <returns></returns>
        public string GetSlnDetailHtml(int SlnSysNo, int PrjTypeOrderNum, string ProjectPicPath)
        {
            //if prjtypeordernum==0, 找出第一个valid的prjtypeordernum
            if (PrjTypeOrderNum == 0)
            {
                string sql = "select top 1 ordernum as prjtypeordernum from prj_type where slnsysno=@slnsysno and status=0 order by ordernum";
                sql = sql.Replace("@slnsysno", SlnSysNo.ToString());
                PrjTypeOrderNum = Int32.Parse(SqlHelper.ExecuteScalar(sql).ToString());
            }

            PrjTypeInfo oPrjTypeInfo = PrjManager.GetInstance().LoadPrjType(SlnSysNo, PrjTypeOrderNum);
            if (oPrjTypeInfo == null)
                return "";

            int PrjTypeSysNo = oPrjTypeInfo.SysNo;

            StringBuilder sbTotal = new StringBuilder();   //total html
            StringBuilder sbNav = new StringBuilder();     //project type 导航 html
            StringBuilder sbDetail = new StringBuilder();  //同一类型的projects详细html

            sbTotal.Append("<table class=specification>");
            sbTotal.Append("    <tr>");
            sbTotal.Append("        <td>");
            sbNav.Append("              <table align=center>");
            sbNav.Append("                  <tr>");
            SortedList slType = PrjManager.GetInstance().GetPrjTypeList(SlnSysNo);  //获得solution下project type list => 导航
            foreach (PrjTypeInfo oTypeInfo in slType.Keys)
            {
                if (oTypeInfo.Status == (int)AppEnum.BiStatus.Valid)
                {
                    sbNav.Append("              <td>");
                    if (PrjTypeOrderNum == oTypeInfo.OrderNum)
                    {
                        sbNav.Append("              <a class=icson href='../Solution/SolutionDetail.aspx?ID=" + SlnSysNo.ToString() + "&Order=" + oTypeInfo.OrderNum + "'>" + Util.TrimNull(oTypeInfo.Name) + "</a>");
                    }
                    else
                    {
                        sbNav.Append("              <a href='../Solution/SolutionDetail.aspx?ID=" + SlnSysNo.ToString() + "&Order=" + oTypeInfo.OrderNum + "'>" + Util.TrimNull(oTypeInfo.Name) + "</a>");
                    }
                    sbNav.Append("              </td>");
                }
            }
            sbNav.Append("                  </tr>");
            sbNav.Append("              </table>");

            sbTotal.Append(sbNav.ToString());
            sbTotal.Append("        </td>");
            sbTotal.Append("    </tr>");
            sbTotal.Append("    <tr>");
            sbTotal.Append("        <td>");

            sbDetail.Append("           <table align=center>");
            sbDetail.Append("               <tr>");
            DataSet ds = PrjManager.GetInstance().GetPrjListDefaultDetailDs(PrjTypeSysNo);
            if (Util.HasMoreRow(ds))
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    int PrjSysNo = Util.TrimIntNull(ds.Tables[0].Rows[i]["prjsysno"].ToString());

                    sbDetail.Append("           <td width=320 valign=top class=specification>");
                    sbDetail.Append("               <table>");
                    sbDetail.Append("                   <tr>");
                    sbDetail.Append("                   <td align=center>");
                    bool PrjectPicExist = false;
                    string ProjectPicFile = ProjectPicPath + PrjSysNo.ToString() + ".jpg";
                    if (File.Exists(ProjectPicFile))
                    {
                        PrjectPicExist = true;
                    }

                    if (PrjectPicExist)  //project pic 存在就显示其project的图片,不存在就显示商品组合图片
                    {
                        ProjectPicFile = AppConfig.PicturePath + "project/" + PrjSysNo.ToString() + ".jpg";
                        sbDetail.Append("<img src='" + ProjectPicFile + "' />");
                    }
                    else
                    {
                        sbDetail.Append("##Image##");
                    }
                    sbDetail.Append("                   </td>");
                    sbDetail.Append("                   </tr>");
                    sbDetail.Append("                   <tr>");
                    sbDetail.Append("                       <td>");
                    sbDetail.Append("<span class=ct2>" + Util.TrimNull(ds.Tables[0].Rows[i]["prjname"].ToString()) + "<span>" + " (推荐标配)");
                    sbDetail.Append("                       </td>");
                    sbDetail.Append("                   <tr>");
                    decimal TotalPrice = 0;
                    StringBuilder sbImage = new StringBuilder();
                    foreach (DataRow dr in ds.Tables[i + 1].Rows)
                    {
                        sbDetail.Append("               <tr>");
                        sbDetail.Append("                   <td>");
                        sbDetail.Append("<span class=icson>" + Util.TrimNull(dr["slnitemname"].ToString()) + "</span><br>");
                        sbDetail.Append("<a target='_blank' href='../Items/ItemDetail.aspx?ItemID=" + Util.TrimNull(dr["productsysno"]) + "'>" + Util.TrimNull(dr["productname"].ToString()) + "</a>");
                        if (Util.TrimIntNull(dr["defaultqty"].ToString()) > 1)
                        {
                            sbDetail.Append(" <strong>(*" + dr["defaultqty"].ToString() + ")</strong>");
                        }
                        sbDetail.Append("                   </td>");
                        sbDetail.Append("               </tr>");
                        if (!PrjectPicExist && Util.TrimIntNull(dr["isshowpic"].ToString()) == 0)
                        {
                            sbImage.Append("<img src="+ AppConfig.PicturePath + "small/"+ Util.TrimNull(dr["productid"]) +".jpg />");
                        }
                        TotalPrice += Util.ToMoney(dr["currentprice"].ToString()) * Util.ToMoney(dr["defaultqty"].ToString());
                    }

                    if (!PrjectPicExist)
                    {
                        sbDetail.Replace("##Image##", sbImage.ToString());
                    }

                    sbDetail.Append("                   <tr>");
                    sbDetail.Append("                       <td align=center>");
                    sbDetail.Append("<span class=ct2>推荐标配总价: ￥" + Util.ToMoney(TotalPrice).ToString() +"<span>");
                    sbDetail.Append("                       </td>");
                    sbDetail.Append("                   <tr>");

                    sbDetail.Append("                   <tr>");
                    sbDetail.Append("                       <td align=center>");
                    sbDetail.Append("<a href='../Solution/ProjectBuy.aspx?ID=" + PrjSysNo.ToString() + "'><img style='CURSOR: hand' border='0' src='../Images/Items/AddToCart.gif' /></a>");
                    sbDetail.Append("                       </td>");
                    sbDetail.Append("                   <tr>");

                    sbDetail.Append("                   <tr>");
                    sbDetail.Append("                       <td align=center>");
                    sbDetail.Append("<a href='../Solution/ProjectDetail.aspx?ID=" + PrjSysNo.ToString() + "'><img style='CURSOR: hand' border='0' src='../Images/Items/selfconfigure.gif' /></a>");
                    sbDetail.Append("                       </td>");
                    sbDetail.Append("                   <tr>");

                    sbDetail.Append("                   <tr>");
                    sbDetail.Append("                       <td align=center>");
                    sbDetail.Append("<a href='../Solution/ProjectCommend.aspx?ID=" + PrjSysNo.ToString() + "'><img style='CURSOR: hand' border='0' src='../Images/Items/projectcommend.gif' /></a>");
                    sbDetail.Append("                       </td>");
                    sbDetail.Append("                   <tr>");

                    sbDetail.Append("               </table>");
                    sbDetail.Append("           <td>");
                }
            }
            sbDetail.Append("               </tr>");
            sbDetail.Append("           </table>");

            sbTotal.Append(sbDetail.ToString());
            sbTotal.Append("        </td>");
            sbTotal.Append("    </tr>");
            sbTotal.Append("</table>");

            return sbTotal.ToString();
        }

        public DataSet GetSlnLeftNavDs(int SlnSysNo)
        {
            StringBuilder sb = new StringBuilder();
            string sql = "select sysno as prjtypesysno,name as prjtypename,ordernum as prjtypeordernum from prj_type where status=0 and slnsysno=" + SlnSysNo.ToString() + " order by ordernum;";
            sb.Append(sql);
            SortedList sl = PrjManager.GetInstance().GetPrjTypeList(SlnSysNo);
            if (sl == null)
                return null;
            foreach (PrjTypeInfo oType in sl.Keys)
            {
                if (oType.Status == (int)AppEnum.BiStatus.Valid)
                {
                    sql = "select sysno as prjsysno,name as prjname from prj_master where status=0 and prjtypesysno=" + oType.SysNo.ToString() + " order by ordernum;";
                    sb.Append(sql);
                }
            }

            DataSet ds = SqlHelper.ExecuteDataSet(sb.ToString().Substring(0, sb.Length - 1));
            if (!Util.HasMoreRow(ds))
                return null;
            return ds;
        }
    }
}