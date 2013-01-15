using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using Icson.BLL.Purchase;
using Icson.Objects.Purchase;
using Icson.Objects.Sale;
using Icson.Objects.Basic;
using Icson.DBAccess;
using Icson.DBAccess.Basic;
using Icson.DBAccess.Sale;
using Icson.Utils;
using System.Transactions;
using Icson.Objects;

namespace Icson.BLL.Sale
{
	/// <summary>
	/// Summary description for RMAManager.
	/// </summary>
	public class RMAManager
	{
		public RMAManager()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		private static RMAManager _instance;
		public static RMAManager GetInstance()
		{
			if(_instance==null)
			{
				_instance = new RMAManager();
			}
			return _instance;
		}
		private void map(RMAInfo oParam,DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.RMAID = Util.TrimNull(tempdr["RMAID"]);
			oParam.SOSysNo = Util.TrimIntNull(tempdr["SOSysNo"]);
			oParam.CustomerSysNo = Util.TrimIntNull(tempdr["CustomerSysNo"]);
			oParam.Status = Util.TrimIntNull(tempdr["Status"]);
			oParam.AuditUserSysNo = Util.TrimIntNull(tempdr["AuditUserSysNo"]);
			oParam.AuditTime = Util.TrimDateNull(tempdr["AuditTime"]);
			oParam.ReceiveUserSysNo = Util.TrimIntNull(tempdr["ReceiveUserSysNo"]);
			oParam.ReceiveTime = Util.TrimDateNull(tempdr["ReceiveTime"]);
			oParam.CloseUserSysNo = Util.TrimIntNull(tempdr["CloseUserSysNo"]);
			oParam.CloseTime = Util.TrimDateNull(tempdr["CloseTime"]);
			oParam.RMAUserSysNo = Util.TrimIntNull(tempdr["RMAUserSysNo"]);
			oParam.RMATime = Util.TrimDateNull(tempdr["RMATime"]);
			oParam.LastUserSysNo = Util.TrimIntNull(tempdr["LastUserSysNo"]);
			oParam.UserChangedTime = Util.TrimDateNull(tempdr["UserChangedTime"]);
			oParam.RMANote = Util.TrimNull(tempdr["RMANote"]);
			oParam.CCNote = Util.TrimNull(tempdr["CCNote"]);
			oParam.SubmitInfo = Util.TrimNull(tempdr["SubmitInfo"]);
			oParam.ReceiveInfo = Util.TrimNull(tempdr["ReceiveInfo"]);
			oParam.UserStatus = Util.TrimIntNull(tempdr["UserStatus"]);
			oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
		}
		private void map(RMAItemInfo oParam,DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.RMASysNo = Util.TrimIntNull(tempdr["RMASysNo"]);
			oParam.ProductSysNo = Util.TrimIntNull(tempdr["ProductSysNo"]);
			oParam.RMAType = Util.TrimIntNull(tempdr["RMAType"]);
			oParam.RMAQty = Util.TrimIntNull(tempdr["RMAQty"]);
			oParam.RMADesc = Util.TrimNull(tempdr["RMADesc"]);
		}
		public RMAInfo LoadMaster(int rmaSysNo)
		{
			string sql = "select * from RMA_Master where sysno ="+rmaSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if(!Util.HasMoreRow(ds))
				return null;
			RMAInfo oRMA = new RMAInfo();
			this.map(oRMA,ds.Tables[0].Rows[0]);
			return oRMA;
		}
		public RMAItemInfo LoadSingleItem(int rmaItemSysNo)
		{
			string sql = "select * from RMA_Item where sysno ="+rmaItemSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if(!Util.HasMoreRow(ds))
				throw new BizException("This RMA Item doesn't exist");
			RMAItemInfo oRMAItem = new RMAItemInfo();
			this.map(oRMAItem,ds.Tables[0].Rows[0]);
			return oRMAItem;
		}
		public RMAInfo Load(int rmaSysNo)
		{
//            TransactionOptions options = new TransactionOptions();
//            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
//            options.Timeout = TransactionManager.DefaultTimeout;

//            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
//            {
//                RMAInfo oRMA = this.LoadMaster(rmaSysNo);
//                if (oRMA != null)
//                {
//                    string sql = "select * from RMA_Item where rmasysno =" + rmaSysNo;
//                    DataSet ds = SqlHelper.ExecuteDataSet(sql);
//                    if (Util.HasMoreRow(ds))
//                    {
//                        foreach (DataRow dr in ds.Tables[0].Rows)
//                        {
//                            RMAItemInfo oRMAItem = new RMAItemInfo();
//                            this.map(oRMAItem, dr);

//                            string SOItemPODesc = "";
//                            sql = @"select pm.sysno as posysno,pm.vendorsysno from RMA_Item ri 
//                                    inner join RMA_Master rm on ri.RMASysNo=rm.sysno 
//                                    inner join so_master sm on rm.sosysno=sm.sysno 
//                                    inner join so_item si on sm.sysno=si.sosysno and si.productsysno=ri.productsysno 
//                                    inner join so_item_po sip on si.sysno=sip.soitemsysno 
//                                    inner join po_master pm on sip.posysno=pm.sysno
//                                     where rm.sysno=" + rmaSysNo + " and ri.productsysno=" + Util.TrimIntNull(dr["ProductSysNo"]);
//                            DataSet dsSOItemPO = SqlHelper.ExecuteDataSet(sql);
//                            foreach (DataRow drSOItemPO in dsSOItemPO.Tables[0].Rows)
//                            {
//                                int POSysNo = Util.TrimIntNull(drSOItemPO["posysno"].ToString());
//                                int VendorSysNo = Util.TrimIntNull(drSOItemPO["vendorsysno"].ToString());
//                                SOItemPODesc += "采购单号:<a href=\"javascript:openWindowS2('../Purchase/POSheet.aspx?sysno=" + POSysNo + "&opt=view')" + "\" >" + POSysNo + "</a>, " + "供应商编号:<a href=\"javascript:openWindowS2('../Basic/VendorOpt.aspx?sysno=" + VendorSysNo + "&opt=update')" + "\">" + VendorSysNo + "</a><br>";
//                            }
//                            oRMAItem.SOItemPODesc = SOItemPODesc;

//                            oRMA.ItemHash.Add(oRMAItem.ProductSysNo, oRMAItem);
//                        }
//                    }
//                }
//                scope.Complete();
//                return oRMA;
//            }

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                RMAInfo oRMA = this.LoadMaster(rmaSysNo);
                if (oRMA != null)
                {
                    string sql = "select * from RMA_Item where rmasysno =" + rmaSysNo;
                    DataSet ds = SqlHelper.ExecuteDataSet(sql);
                    if (Util.HasMoreRow(ds))
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            RMAItemInfo oRMAItem = new RMAItemInfo();
                            this.map(oRMAItem, dr);

                            string SOItemPODesc = "";
                            sql = @"select isnull(pm.sysno,0) as posysno,isnull(pm.vendorsysno,0) as vendorsysno,isnull(pid.sysno,0) as productidsysno from RMA_Item ri 
                                    inner join RMA_Master rm on ri.RMASysNo=rm.sysno 
                                    inner join so_master sm on rm.sosysno=sm.sysno 
                                    inner join so_item si on sm.sysno=si.sosysno and si.productsysno=ri.productsysno 
                                    inner join so_item_po sip on si.sysno=sip.soitemsysno 
                                    left join po_master pm on sip.posysno=pm.sysno 
                                    left join product_id pid on sip.productidsysno=pid.sysno 
                                    where rm.sysno=" + rmaSysNo + " and ri.productsysno=" + Util.TrimIntNull(dr["ProductSysNo"]);
                            DataSet dsSOItemPO = SqlHelper.ExecuteDataSet(sql);
                            foreach (DataRow drSOItemPO in dsSOItemPO.Tables[0].Rows)
                            {
                                int POSysNo = Util.TrimIntNull(drSOItemPO["posysno"].ToString());
                                int VendorSysNo = Util.TrimIntNull(drSOItemPO["vendorsysno"].ToString());
                                int ProductIDSysNo = Util.TrimIntNull(drSOItemPO["productidsysno"].ToString());
                                if (POSysNo > 0)
                                {
                                    SOItemPODesc += "采购单号:<a href=\"javascript:openWindowS2('../Purchase/POSheet.aspx?sysno=" + POSysNo + "&opt=view')" + "\" >" + POSysNo + "</a>, " + "供应商编号:<a href=\"javascript:openWindowS2('../Basic/VendorOpt.aspx?sysno=" + VendorSysNo + "&opt=update')" + "\">" + VendorSysNo + "</a><br>";
                                }
                                else if (ProductIDSysNo > 0)
                                {
                                    SOItemPODesc += "商品序列号:<a href=\"javascript:openWindowS2('../Basic/ProductID.aspx?sysno=" + ProductIDSysNo + "')" + "\" >" + ProductIDSysNo + "</a><br>";
                                }
                            }
                            oRMAItem.SOItemPODesc = SOItemPODesc;
                            oRMA.ItemHash.Add(oRMAItem.ProductSysNo, oRMAItem);
                        }
                    }
                }
                scope.Complete();
                return oRMA;
            }
		}
		private void InsertMaster(RMAInfo oRMA)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				new RMADac().InsertMaster(oRMA);
				scope.Complete();
            }
		}
		private void InsertItem(RMAItemInfo oRMAItem)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				new RMADac().InsertItem(oRMAItem);
				scope.Complete();
            }
		}

		public string GetRMASnapShot(RMAInfo oParam)
		{
			StringBuilder sb = new StringBuilder();
			if(oParam.ItemHash.Count>0)
			{
				sb.Append("<table class=\"GridViewStyle\" cellspacing=\"0\" rules=\"all\" border=\"1\" id=\"Table99\" style=\"width: 100%;border-collapse: collapse;\">");
				sb.Append("	<tr class=\"GridViewHeaderStyle\">");
				sb.Append("	 <td>"+oParam.RMAID+"</td>");
				sb.Append("	 <td>商品名称</td>");
				sb.Append("  <td>返修数量</td>");
				sb.Append("	 <td>返修类型</td>");
				sb.Append("  <td>返修原因</td>");
				sb.Append(" </tr>");
				Hashtable SysnoHt = new Hashtable();				
				foreach(RMAItemInfo item in oParam.ItemHash.Values)
				{
					SysnoHt.Add(item.ProductSysNo,item.ProductSysNo);
				}
				Hashtable PbHt = Icson.BLL.Basic.ProductManager.GetInstance().GetProductBoundle(SysnoHt);
                int i = 1;
				foreach(RMAItemInfo item in oParam.ItemHash.Values)
				{
                    if ((i % 2) != 0 )
                    {
                        sb.Append("<tr class=\"GridViewRowStyle\" onmouseout=\"this.style.backgroundColor=currentcolor,this.style.fontWeight=&quot;&quot;;\" onmouseover=\"currentcolor=this.style.backgroundColor;this.style.backgroundColor=&quot;#d8e3e7&quot;,this.style.fontWeight=&quot;&quot;;jquery_Tools_showthis(this);\">");
                    }
                    else
                    {
                        sb.Append("<tr class=\"GridViewAlternatingRowStyle\" onmouseout=\"this.style.backgroundColor=currentcolor,this.style.fontWeight=&quot;&quot;;\" onmouseover=\"currentcolor=this.style.backgroundColor;this.style.backgroundColor=&quot;#d8e3e7&quot;,this.style.fontWeight=&quot;&quot;;jquery_Tools_showthis(this);\">");
                    }
                    i++;
                    foreach(ProductBasicInfo PbInfo in PbHt.Values)
					{
						if(PbInfo.SysNo==item.ProductSysNo)
						{
							sb.Append("<td>");
							sb.Append(PbInfo.ProductID);
							sb.Append("</td>");
							sb.Append("<td>");
							sb.Append(PbInfo.ProductName);
							sb.Append("</td>");
							break;
						}
					}
					sb.Append(" <td>"+item.RMAQty.ToString()+"</td>");
					sb.Append(" <td>"+AppEnum.GetRMAType(item.RMAType)+"</td>");
					sb.Append(" <td>"+item.RMADesc+"</td>");
					sb.Append("</tr>");
				}
				sb.Append("</table>");
			}
			return sb.ToString();
		}

		public void FixData()
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				string sql = @"select rm.sysno,pb.newsysno as productsysno ,ri.rmatype,ri.rmaquantity
								from ipp2003..rma_item ri
								inner join ippconvert..productbasic pb on pb.oldsysno = ri.productsysno
								inner join ipp2003..rma_master rm on rm.sysno = ri.rmasysno
								where rm.status = 0";
				DataSet ds = SqlHelper.ExecuteDataSet(sql);
				if(Util.HasMoreRow(ds))
				{
					foreach(DataRow dr in ds.Tables[0].Rows)
					{
						string updateSql = @"update rma_item 
												set rmatype="+dr["RMAType"].ToString()+",rmaqty="+dr["RMAQuantity"].ToString()
											+"	where rmasysno="+dr["SysNo"].ToString()+" and productsysno="+dr["ProductSysNo"].ToString();
						new ProductSaleTrendDac().Exec(updateSql);
					}
				}
				scope.Complete();
            }
		}

		public void Import()
		{
			string sql = @"select top 1 * from rma_master;
						   select top 1 * from rma_item;";
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			foreach(DataTable dt in ds.Tables)
			{
				if(Util.HasMoreRow(dt))
					throw new BizException("The target is not empty");
			}
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				//Insert RMA_Master
				string sqlMaster = @"select rm.sysno, rmaid, sosysno, customsysno as customersysno, approvetime as audittime, suau.newsysno as auditusersysno, status, rmatime as createtime,
									 note as rmanote,note as ccnote
									 from ipp2003..rma_master rm
									 left join ippconvert..sys_user suau on suau.oldsysno = rm.approversysno";
				DataSet dsMaster = SqlHelper.ExecuteDataSet(sqlMaster);
				if(Util.HasMoreRow(dsMaster))
				{
					foreach(DataRow dr in dsMaster.Tables[0].Rows)
					{
						RMAInfo oRMA = new RMAInfo();
						oRMA.SysNo = Util.TrimIntNull(dr["SysNo"]);
						oRMA.RMAID = Util.TrimNull(dr["RMAID"]);
						oRMA.SOSysNo = Util.TrimIntNull(dr["SOSysNo"]);
						oRMA.CustomerSysNo = Util.TrimIntNull(dr["CustomerSysNo"]);
						oRMA.AuditTime = Util.TrimDateNull(dr["AuditTime"]);
						oRMA.AuditUserSysNo = Util.TrimIntNull(dr["AuditUserSysNo"]);
						switch((int)dr["Status"]) 
						{
							case -1:
								oRMA.Status = (int)AppEnum.RMAStatus.Abandon;
								break;
							case 0:
								oRMA.Status = (int)AppEnum.RMAStatus.Origin;
								break;
							case 1:
								oRMA.Status = (int)AppEnum.RMAStatus.Closed;
								break;
						}
						oRMA.CreateTime = Util.TrimDateNull(dr["CreateTime"]);
						oRMA.RMANote = Util.TrimNull(dr["RMANote"]);
						oRMA.CCNote = Util.TrimNull(dr["CCNote"]);
						oRMA.UserStatus = 1;
						new RMADac().InsertMaster(oRMA);
					}
				}
				//Insert RMA_Item
				string sqlItem = @"select 1 as sysno,rmasysno,case realrmatype when 3 then "+(int)AppEnum.RMAType.Overrule+@" when 1 then "+(int)AppEnum.RMAType.Return
								+@" when 2 then "+(int)AppEnum.RMAType.Maintain+@" end as rmatype, realrmaquantity as rmaqty,
								   realrmareason as rmadesc,pb.newsysno as productsysno 
								   from ipp2003..rma_item ri
								   inner join ippconvert..productbasic pb on pb.oldsysno = ri.productsysno";
				DataSet dsItem = SqlHelper.ExecuteDataSet(sqlItem);
				if(Util.HasMoreRow(dsItem))
				{
					foreach(DataRow dr in dsItem.Tables[0].Rows)
					{
						RMAItemInfo oRMAItem = new RMAItemInfo();
						this.map(oRMAItem,dr);
						new RMADac().InsertItem(oRMAItem);
					}
				}			
				//Insert SnapShot
				string sqlSnapShot = @"select rmasysno,rmaid,case rmatype when 0 then "+(int)AppEnum.RMAType.Unsure+@" when 1 then "+(int)AppEnum.RMAType.Return
									+@" when 2 then "+(int)AppEnum.RMAType.Maintain+@" end as rmatype, rmaquantity as rmaqty,
									   rmareason as rmadesc,pb.newsysno as productsysno,pl.productname,p.productid
									   from ipp2003..rma_item ri
									   inner join ippconvert..productbasic pb on pb.oldsysno = ri.productsysno
									   inner join ipp2003..product_language pl on pl.productsysno = ri.productsysno and pl.languageid = 'cn'
									   inner join ipp2003..product p on p.sysno = ri.productsysno
									   inner join ipp2003..rma_master rm on rm.sysno = ri.rmasysno
									   order by rmasysno";
				DataSet dsSnapShot = SqlHelper.ExecuteDataSet(sqlSnapShot);
				if(Util.HasMoreRow(dsSnapShot))
				{
					StringBuilder sb = new StringBuilder();
					int tempSysNo = 0;
					foreach(DataRow dr in dsSnapShot.Tables[0].Rows)
					{
						if(tempSysNo!=(int)dr["RMASysNo"])
						{
							if(tempSysNo!=0)
							{
								sb.Append("</table>");
								Hashtable updateht = new Hashtable();
								updateht.Add("SysNo",tempSysNo);
								updateht.Add("SubmitInfo",sb.ToString());
								updateht.Add("ReceiveInfo",sb.ToString());
								new RMADac().UpdateMaster(updateht);
								sb.Remove(0,sb.Length);
							}
							sb.Append("<table width='100%' border='1' cellpadding='0' cellspacing='0'>");
							sb.Append("	<tr>");
							sb.Append("	 <td>"+dr["RMAID"].ToString()+"</td>");
							sb.Append("	 <td>商品名称</td>");
							sb.Append("  <td>返修数量</td>");
							sb.Append("	 <td>返修类型</td>");
							sb.Append("  <td>返修原因</td>");
							sb.Append(" </tr>");							
						}
						sb.Append("<tr>");
						sb.Append(" <td>"+dr["ProductID"].ToString()+"</td>");
						sb.Append(" <td>"+dr["ProductName"].ToString()+"</td>");
						sb.Append(" <td>"+dr["RMAQty"].ToString()+"</td>");
						sb.Append(" <td>"+AppEnum.GetRMAType(Util.TrimIntNull(dr["RMAType"]))+"</td>");
						sb.Append(" <td>"+dr["RMADesc"].ToString()+"</td>");
						sb.Append("</tr>");		
						tempSysNo = (int)dr["RMASysNo"];
					}
				}	
				//Insert Sequence
				string sqlMaxSysNo = @"select max(sysno) as sysno from rma_master";
				DataSet dsMaxSysNo = SqlHelper.ExecuteDataSet(sqlMaxSysNo);
				int n = 0;
				while(n<Util.TrimIntNull(dsMaxSysNo.Tables[0].Rows[0][0]))
				{
					n = SequenceDac.GetInstance().Create("RMA_Sequence");
				}
				scope.Complete();
            }
		}

		public void AbandonRMA(RMAInfo oRMA)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				int currentStatus = this.GetRMACurrentStatus(oRMA.SysNo);
				if(currentStatus!=(int)AppEnum.RMAStatus.Origin&&currentStatus!=(int)AppEnum.RMAStatus.Received)
					throw new BizException("此单已经过处理，不能作废");
				Hashtable paramHash = new Hashtable(2);
				paramHash.Add("LastUserSysNo",oRMA.LastUserSysNo);
				paramHash.Add("Status",oRMA.Status);
				paramHash.Add("SysNo",oRMA.SysNo);
				RMAManager.GetInstance().UpdateRMAMaster(paramHash);
				scope.Complete();
            }
		}

		public void UpdateRMA(RMAInfo rmaInfo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				this.UpdateRMAMaster(rmaInfo);
				if(rmaInfo.ItemHash.Count>0)
				{
					foreach(RMAItemInfo item in rmaInfo.ItemHash.Values)
					{
						this.UpdateRMAItem(item);
					}
				}
				scope.Complete();
            }
		}

		public void UpdateRMAMaster(RMAInfo rmaInfo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				Hashtable paramHash = new Hashtable();
				paramHash.Add("CCNote",rmaInfo.CCNote);
				paramHash.Add("LastUserSysNo",rmaInfo.LastUserSysNo);
				paramHash.Add("RMANote",rmaInfo.RMANote);
				paramHash.Add("SysNo",rmaInfo.SysNo);
				paramHash.Add("UserChangedTime",rmaInfo.UserChangedTime);
				paramHash.Add("UserStatus",rmaInfo.UserStatus);
				this.UpdateRMAMaster(paramHash);
				scope.Complete();
            }
		}

		public void UpdateRMAMaster(Hashtable paramHash)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				new RMADac().UpdateMaster(paramHash);
				scope.Complete();
            }
		}
		
		private void UpdateRMAItem(RMAItemInfo item)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				new RMADac().UpdateItem(item);
				scope.Complete();
            }
		}

	    public RMAInfo BuildRMAfromSO(int soSysNo)
		{
			if(!this.IfExistOpenedRMA(soSysNo))
			{
				RMAInfo rmaInfo = new RMAInfo();
				SOInfo soInfo  = SaleManager.GetInstance().LoadSO(soSysNo);
				if(soInfo!=null&&soInfo.Status==(int)AppEnum.SOStatus.OutStock)
				{
					rmaInfo.SOSysNo = soInfo.SysNo;
					rmaInfo.CustomerSysNo = soInfo.CustomerSysNo;
					if(soInfo.ItemHash.Count>0)
					{
						foreach(SOItemInfo soItem in soInfo.ItemHash.Values)
						{
							RMAItemInfo rmaItem = new RMAItemInfo();
							rmaItem.ProductSysNo = soItem.ProductSysNo;
							rmaItem.RMAType = (int)AppEnum.RMAType.Unsure;
							rmaItem.RMAQty = soItem.Quantity;
							rmaInfo.ItemHash.Add(rmaItem.ProductSysNo,rmaItem);

						    string SOItemPODesc = "";
						    Hashtable ht = SaleManager.GetInstance().LoadSOItemPOList(soItem.SysNo);
                            if(ht != null)
                            {
                                foreach(SOItemPOInfo soItemPO in ht.Keys)
                                {
                                    if (soItemPO.POSysNo > 0)
                                    {
                                        POInfo poInfo = PurchaseManager.GetInstance().LoadPO(soItemPO.POSysNo);
                                        int VendorSysNo = poInfo.VendorSysNo;
                                        SOItemPODesc += "采购单号:<a href=\"javascript:openWindowS2('../Purchase/POSheet.aspx?sysno=" + soItemPO.POSysNo + "&opt=view')" + "\" >" + soItemPO.POSysNo + "</a>, " + "供应商编号:<a href=\"javascript:openWindowS2('../Basic/VendorOpt.aspx?sysno=" + VendorSysNo + "&opt=update')" + "\">" + VendorSysNo + "</a><br>";
                                    }
                                    else if(soItemPO.ProductIDSysNo > 0)
                                    {
                                        SOItemPODesc += "商品序列号:<a href=\"javascript:openWindowS2('../Basic/ProductID.aspx?sysno=" + soItemPO.ProductIDSysNo + "')" + "\" >" + soItemPO.ProductIDSysNo + "</a><br>";
                                    }
                                }
                            }
						    rmaItem.SOItemPODesc = SOItemPODesc;
						}
					}
				}
				else 
					rmaInfo = null;
				return rmaInfo;
			}
			else 
				throw new BizException("本销售单已经存在一张保修单在处理中，其间您不能再提交新的保修申请，请联系ORS商城客服");
		}

		public RMAInfo BuildRMAfromSO(string soID)
		{
			int sysno = SaleManager.GetInstance().GetSOSysNofromID(soID);
			if(sysno!=AppConst.IntNull)
				return this.BuildRMAfromSO(sysno);
			else
				return null;
		}

		/// <summary>
		/// 新增rma单
		/// </summary>
		/// <param name="rmaInfo"></param>
		public void AddRMA(RMAInfo rmaInfo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				if(!this.IfExistOpenedRMA(rmaInfo.SOSysNo))
				{
					Hashtable leftHash = this.GetLeftRMAQty(rmaInfo.SOSysNo);
					foreach(RMAItemInfo item in rmaInfo.ItemHash.Values)
					{
						if(item.RMAQty>(int)leftHash[item.ProductSysNo])
							throw new BizException("您填写的保修商品数量超过销售单的实际购买数量，请确认需保修的商品均为此单购买");
					}
					rmaInfo.SysNo = SequenceDac.GetInstance().Create("RMA_Sequence");
					rmaInfo.RMAID = this.BuildRMAID(rmaInfo.SysNo);
					rmaInfo.SubmitInfo = this.GetRMASnapShot(rmaInfo);
					rmaInfo.Status = (int)AppEnum.RMAStatus.Origin;
					this.InsertMaster(rmaInfo);
					foreach(RMAItemInfo rmaItem in rmaInfo.ItemHash.Values)
					{
						rmaItem.RMASysNo = rmaInfo.SysNo;
						this.InsertItem(rmaItem);
					}
				}
				else
					throw new BizException("本销售单已经存在一张保修单在处理中，其间您不能再提交新的保修申请，请联系ORS商城客服");
				scope.Complete();
            }
		}

		public void DeleteRMAItem(int sysno)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				new RMADac().DeleteItem(sysno);
				scope.Complete();
            }
		}

		public void HandleRMA(RMAInfo rmaInfo,SessionInfo sInfo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				if(this.GetRMACurrentStatus(rmaInfo.SysNo)!=(int)AppEnum.RMAStatus.Received)
					throw new BizException("This rma sheet is not received now,can't be handled");
				Hashtable paramHash = new Hashtable(4);
				paramHash.Add("Status",rmaInfo.Status);
				paramHash.Add("RMATime",rmaInfo.RMATime);
				paramHash.Add("RMAUserSysNo",rmaInfo.RMAUserSysNo);
				paramHash.Add("SysNo",rmaInfo.SysNo);
				this.UpdateRMAMaster(paramHash);
				//如果有退货类型的商品，自动生成退货单
				foreach(RMAItemInfo rmaItem in rmaInfo.ItemHash.Values)
				{
					if(rmaItem.RMAType==(int)AppEnum.RMAType.Return)
					{
						ROInfo roInfo = ROManager.GetInstance().BuildROFromRMA(rmaInfo);
						ROManager.GetInstance().AddRO(roInfo);
						LogManager.GetInstance().Write(new LogInfo(roInfo.SysNo,(int)AppEnum.LogType.Sale_RO_Create,sInfo));
						break;
					}
				}
				LogManager.GetInstance().Write(new LogInfo(rmaInfo.SysNo,(int)AppEnum.LogType.Sale_RMA_Handle,sInfo));
				scope.Complete();
            }
		}

		public void CancelHandleRMA(RMAInfo rmaInfo,SessionInfo sInfo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				if(this.GetRMACurrentStatus(rmaInfo.SysNo)!=(int)AppEnum.RMAStatus.Handled)
					throw new BizException("This rma sheet is not handled now,can't cancel handle");
				//如果存在退货单，则作废
				ROInfo roInfo = ROManager.GetInstance().LoadROfromRMA(rmaInfo.SysNo);
				if(roInfo!=null)
				{
					ROManager.GetInstance().AbandonRO(roInfo);
					LogManager.GetInstance().Write(new LogInfo(roInfo.SysNo,(int)AppEnum.LogType.Sale_RO_Abandon,sInfo));
				}
				Hashtable paramHash = new Hashtable(4);
				paramHash.Add("Status",rmaInfo.Status);
				paramHash.Add("RMATime",rmaInfo.RMATime);
				paramHash.Add("RMAUserSysNo",rmaInfo.RMAUserSysNo);
				paramHash.Add("SysNo",rmaInfo.SysNo);
				this.UpdateRMAMaster(paramHash);	
				LogManager.GetInstance().Write(new LogInfo(rmaInfo.SysNo,(int)AppEnum.LogType.Sale_RMA_CancelHandle,sInfo));
				scope.Complete();
            }
		}
//		public DataSet GetRMAList(Hashtable paramHash)
//		{
//			string sql = @"select 
//								rm.sysno,rm.rmaid,rm.sosysno,rm.createtime,rm.status,rm.userstatus,su.username as lasthandler,sm.soid
//						  from 
//								rma_master rm 
//						  inner join so_master sm on sm.sysno = rm.sosysno
//						  left join sys_user su on su.sysno = rm.lastusersysno";						  
//			if(paramHash.Count>0)
//			{
//				StringBuilder sb = new StringBuilder();
//				sb.Append(" where 1=1 ");
//				foreach(string key in paramHash.Keys)
//				{
//					object item = paramHash[key];
//					sb.Append(" and ");
//					if(key=="StartDate")
//					{
//						sb.Append("rm.createtime").Append(">=").Append(Util.ToSqlString(item.ToString()));
//					}
//					else if(key=="EndDate")
//					{
//						sb.Append("rm.createtime").Append("<=").Append(Util.ToSqlEndDate(item.ToString()));
//					}
//					else if(key=="Status")
//					{
//						sb.Append("rm.status").Append("=").Append(item.ToString());
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
//				sql.Replace("select","select top 50");
//			sql += " order by rm.sysno desc";
//			return SqlHelper.ExecuteDataSet(sql);
//		}

        public DataSet GetRMAList(Hashtable paramHash)
        {
            string sql = "";
            if (!paramHash.ContainsKey("ProductID"))
            {
                sql = @"select 
								distinct rm.sysno,rm.rmaid,rm.sosysno,rm.createtime,rm.receivetime,rm.status,rm.userstatus,su.username as lasthandler,sm.soid,sm.AuditDeliveryDate,sm.OutTime
						  from 
								rma_master rm inner join rma_item ri on rm.sysno=ri.rmasysno 
						  inner join so_master sm on sm.sysno = rm.sosysno
						  left join sys_user su on su.sysno = rm.lastusersysno";
            }
            else
            {
                sql = @"select 
								distinct rm.sysno,rm.rmaid,rm.sosysno,rm.createtime,rm.receivetime,rm.status,rm.userstatus,su.username as lasthandler,sm.soid,sm.AuditDeliveryDate,sm.OutTime
						  from 
								rma_master rm 
						  inner join rma_item ri on rm.sysno = ri.rmasysno 
						  inner join product p on ri.productsysno = p.sysno and p.productid = @productid 
						  inner join so_master sm on sm.sysno = rm.sosysno
						  left join sys_user su on su.sysno = rm.lastusersysno ";

                sql = sql.Replace("@productid", "'" + paramHash["ProductID"].ToString() + "'");
            }

            if (paramHash.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" where 1=1 ");
                foreach (string key in paramHash.Keys)
                {
                    object item = paramHash[key];
                    sb.Append(" and ");
                    if (key == "StartDate")
                    {
                        sb.Append("rm.createtime").Append(">=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "EndDate")
                    {
                        sb.Append("rm.createtime").Append("<=").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "ReceiveStartDate")
                    {
                        sb.Append("rm.receivetime").Append(">=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "ReceiveEndDate")
                    {
                        sb.Append("rm.receivetime").Append("<=").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "Status")
                    {
                        sb.Append("rm.status").Append("=").Append(item.ToString());
                    }
                    else if (key == "RMAType")
                    {
                        sb.Append("ri.RmaType").Append("=").Append(item.ToString());
                    }
                    else if (key == "ProductSysNo")
                    {
                        sb.Append("ri.productsysno").Append("=").Append(item.ToString());
                    }
                    else if (key == "UsedDate")
                    {
                        sb.Append("DATEDIFF(day,sm.AuditDeliveryDate,rm.createtime)").Append("<=").Append(item.ToString());
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
                sql.Replace("select", "select top 50");
            sql += " order by rm.sysno desc";
            return SqlHelper.ExecuteDataSet(sql);
        }

		/// <summary>
		/// 获取客户可以申请RMA的订单
		/// </summary>
		/// <param name="customerSysNo"></param>
		/// <returns></returns>
		public Hashtable GetSOforRMA(int customerSysNo)
		{
			string sql = @"select 
								sm.sysno,sm.soid
							from 
								so_master sm
							left join rma_master rm on rm.sosysno = sm.sysno and rm.status<>"+(int)AppEnum.RMAStatus.Abandon+" and rm.status<>"+(int)AppEnum.RMAStatus.Closed
						+@"	where
								sm.status="+(int)AppEnum.SOStatus.OutStock+" and sm.customersysno= "+customerSysNo+" and rm.sosysno is null";
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			Hashtable idHash = new Hashtable(5);
			if(Util.HasMoreRow(ds))
			{
				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					idHash.Add((int)dr["SysNo"],dr["SOID"].ToString());
				}
			}
			return idHash;
		}
		
		/// <summary>
		/// 获取客户提交过的RMA单号
		/// </summary>
		/// <param name="customerSysNo"></param>
		/// <returns></returns>
		public Hashtable GetRMAIDs(int customerSysNo)
		{
			string sql = @"select rma.sysno ,rma.rmaid from rma_master rma where rma.customerSysNo="+customerSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			Hashtable idHash = new Hashtable(5);
			if(Util.HasMoreRow(ds))
			{
				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					idHash.Add((int)dr["SysNo"],dr["RMAID"].ToString());
				}
			}
			return idHash;
		}
		
		/// <summary>
		/// 获取销售单剩余RMA数量
		/// </summary>
		/// <param name="soSysNo"></param>
		/// <returns></returns>
		public Hashtable GetLeftRMAQty(int soSysNo)
		{
			Hashtable tempHash = new Hashtable(5);
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {	
			
				string sql = @"select
								si.productsysno ,(si.quantity-sum(isnull(ri.quantity,0))) as leftQty
							from
								so_item si
							left join rma_master rma on rma.sosysno = si.sosysno
							left join ro_master rm on rm.rmasysno = rma.sysno and rm.status = "+(int)AppEnum.ROStatus.Returned
					+@"	left join ro_item ri on ri.rosysno = rm.sysno and ri.productsysno = si.productsysno
							where
								si.sosysno = "+soSysNo
					+"	group by si.productsysno,si.quantity";
				DataSet ds = SqlHelper.ExecuteDataSet(sql);
				if(Util.HasMoreRow(ds))
				{
					foreach(DataRow dr in ds.Tables[0].Rows)
					{
						tempHash.Add((int)dr["ProductSysNo"],(int)dr["LeftQty"]);
					}
				}
				scope.Complete();
            }
			return tempHash;
		}

		private bool IfExistOpenedRMA(int soSysNo)
		{
			bool ifExist;
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {	
				string sql = "select sysno from rma_master where status>="+(int)AppEnum.RMAStatus.Origin+" and status<"+(int)AppEnum.RMAStatus.Closed+" and sosysno="+soSysNo;
				DataSet ds = SqlHelper.ExecuteDataSet(sql);
				if(Util.HasMoreRow(ds))
					ifExist = true;
				else
					ifExist = false;
				scope.Complete();
            }
			return ifExist;
		}

		public bool IfExistValidRMA(int soSysNo)
		{
			bool ifExist;
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {	
				string sql = "select sysno from rma_master where status>="+(int)AppEnum.RMAStatus.Origin+" and sosysno="+soSysNo;
				DataSet ds = SqlHelper.ExecuteDataSet(sql);
				if(Util.HasMoreRow(ds))
					ifExist = true;
				else
					ifExist = false;
				scope.Complete();
            }
			return ifExist;
		}
		private string BuildRMAID(int sysNo)
		{
			string sysNoStr = sysNo.ToString();
			int idLen = 10;
			string rmaid = "8";
			for(int i=0;i<(idLen-sysNoStr.Length-1);i++)
			{
				rmaid += "0";
			}
			rmaid += sysNoStr;
			return rmaid;
		}

		private int GetRMACurrentStatus(int sysno)
		{
			int status = AppConst.IntNull;
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {	
				string sql = "select status from rma_master where sysno="+sysno;
				DataSet ds = SqlHelper.ExecuteDataSet(sql);
				if(Util.HasMoreRow(ds))
					status = (int)ds.Tables[0].Rows[0][0];
				scope.Complete();
            }
			return status;
		}
	}
}
