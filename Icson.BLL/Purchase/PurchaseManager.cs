using System;
using System.Data;
using System.Collections;
using System.Text;
using System.Web;
using Icson.Utils;
using System.Transactions;
using Icson.Objects;
using Icson.Objects.Basic;
using Icson.Objects.Purchase;

using Icson.DBAccess;
using Icson.DBAccess.Purchase;

using Icson.BLL.Basic;
using Icson.BLL.Online;
using Icson.BLL.Finance;
using Icson.Objects.Finance;

namespace Icson.BLL.Purchase
{
	/// <summary>
	/// Summary description for PurchaseManager.
	/// </summary>
	public class PurchaseManager
	{
		private PurchaseManager()
		{
		}
		private static PurchaseManager _instance;
		public static PurchaseManager GetInstance()
		{
			if( _instance == null )
			{
				_instance = new PurchaseManager();
			}
			return _instance;
		}
		#region POBasket
		private void map(POBasketInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.CreateUserSysNo = Util.TrimIntNull(tempdr["CreateUserSysNo"]);
			oParam.ProductSysNo = Util.TrimIntNull(tempdr["ProductSysNo"]);
			oParam.Quantity = Util.TrimIntNull(tempdr["Quantity"]);
			oParam.OrderPrice = Util.TrimDecimalNull(tempdr["OrderPrice"]);
		}
		public void InsertPOBasket(POBasketInfo oParam)
		{
			string sql = "select * from po_basket where productsysno=" +oParam.ProductSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds))
				throw new BizException("the same product exists in yours or others' basket");

			new POBasketDac().Insert(oParam);
		}

        public void InsertPOBasketList(ArrayList al)
        {
            if ( al.Count == 0 )
				return;

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                for (int i = 0; i < al.Count; i++)
                {
                    POBasketInfo oInfo = (POBasketInfo)al[i];
                    POBasketInfo existsInfo = LoadBasket(oInfo.ProductSysNo, oInfo.CreateUserSysNo);
                    if (existsInfo == null)
                        InsertPOBasket(oInfo);
                    else
                        UpdatePOBasket(oInfo);
                }
                scope.Complete();
            }
        }

		public void UpdatePOBasket(POBasketInfo oParam)
		{//只能更改数量
			new POBasketDac().Update(oParam);
		}
		public void DeletePOBasket(int sysno, int userSysNo)
		{
			string sql = "select top 1 sysno from po_basket where createusersysno=" + userSysNo + " and sysno=" + sysno;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( !Util.HasMoreRow(ds))
				throw new BizException("delete failed, for this product not in your basket or been deleted already");
			new POBasketDac().Delete(sysno);
		}

        //批量删除采购蓝信息
        public void DeletePOBaskets(string SysNos)
        {
            new POBasketDac().Deletes(SysNos);
        }

		public void DeletePOBasketAfterPO(int productSysNo, int userSysNo)
		{
			new POBasketDac().Delete(productSysNo, userSysNo);
		}
		public SortedList GetPOBasketPPMList()
		{
			//这个ppm的list是动态更新的
			string sql = @"select distinct b.sysno, b.UserName from PO_Basket as a, Sys_User as b 
							where a.createusersysno = b.sysno";
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if (!Util.HasMoreRow(ds))
			{
				return null;
			}
			SortedList sl = new SortedList(5);
			foreach(DataRow dr in ds.Tables[0].Rows)
			{
				sl.Add(Util.TrimIntNull(dr["sysno"]), Util.TrimNull(dr["username"]));
			}
			return sl;
		}
		public DataSet GetPOBasketDs(int userSysNo)
		{
			string sql = @"select a.sysno,b.sysno as productsysno, b.productname, b.productid,a.quantity, a.orderprice, c.username, vendorname
							from PO_Basket as a, Product as b, Sys_User as c, vendor
							where a.productsysno = b.sysno and a.createusersysno = c.sysno and b.lastvendorsysno *= vendor.sysno";
			if ( userSysNo != AppConst.IntNull )
				sql += " and a.createusersysno = " + userSysNo;

			sql += " order by lastvendorsysno";

			return SqlHelper.ExecuteDataSet(sql);
		}

		public Hashtable GetPOBasketHash(int userSysNo)
		{
			if ( userSysNo == AppConst.IntNull )
				return null;
			string sql = @"select * from po_basket where createusersysno = " + userSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if (!Util.HasMoreRow(ds))
				return null;
			Hashtable ht = new Hashtable(5);
			foreach(DataRow dr in ds.Tables[0].Rows)
			{
				POBasketInfo item = new POBasketInfo();
				map(item, dr);
				ht.Add(item.SysNo, item);
			}
			return ht;
		}
		public POBasketInfo LoadBasket(int productSysNo, int userSysNo)
		{
			string sql = "select * from po_basket where productsysno =" + productSysNo + " and createusersysno =" + userSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( !Util.HasMoreRow(ds))
				return null;
			POBasketInfo oInfo = new POBasketInfo();
			map(oInfo, ds.Tables[0].Rows[0]);
			return oInfo;
		}
		#endregion
		public POInfo LoadPO(int sysno)
		{
			string sql = "select * from po_master where sysno = " + sysno;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( !Util.HasMoreRow(ds))
				return null;
			POInfo oMaster = new POInfo();
			map(oMaster, ds.Tables[0].Rows[0]);

			string sqlItem = "select * from po_item where posysno = " + oMaster.SysNo;
			DataSet dsItem = SqlHelper.ExecuteDataSet(sqlItem);
			if ( Util.HasMoreRow(dsItem))
			{
				foreach(DataRow drItem in dsItem.Tables[0].Rows)
				{
					POItemInfo oItem = new POItemInfo();
					map(oItem, drItem);
					oMaster.itemHash.Add(oItem.ProductSysNo, oItem);
				}
			}

			string sqlApportion = "select * from PO_Apportion where posysno = " + oMaster.SysNo;
			DataSet dsApportion = SqlHelper.ExecuteDataSet(sqlApportion);
			if ( Util.HasMoreRow(dsApportion))
			{
				foreach(DataRow drApportion in dsApportion.Tables[0].Rows)
				{
					POApportionInfo oApportion = new POApportionInfo();
					map(oApportion, drApportion);
					oMaster.apportionHash.Add(oApportion,null);

					string sqlApportionItem = "select * from po_apportion_item where apportionsysno = " + oApportion.SysNo;
					DataSet dsApportionItem = SqlHelper.ExecuteDataSet(sqlApportionItem);
					if ( Util.HasMoreRow(dsApportionItem))
					{
						foreach(DataRow drApportionItem in dsApportionItem.Tables[0].Rows)
						{
							POApportionItemInfo oApportionItem = new POApportionItemInfo();
							map(oApportionItem, drApportionItem);
							oApportion.itemHash.Add(oApportionItem.ProductSysNo, oApportionItem);
						}
					}
				}
			}
			return oMaster;
		}

        public POApportionInfo LoadApportion(int poSysNo)
        {
            string sql = "select * from po_apportion where posysno=" + poSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                POApportionInfo oInfo = new POApportionInfo();
                map(oInfo,ds.Tables[0].Rows[0]);
                return oInfo;
            }
            else
                return null;
        }

		public SortedList GetPOApportionSubjectList()
		{
			return GetPOApportionSubjectList(false);

		}
		public SortedList GetPOApportionSubjectList(bool isValidOnly)
		{
			Hashtable ht = GetPOApportionSubjectHash();
			if ( ht == null || ht.Count == 0)
				return null;
			SortedList sl = new SortedList(10);
			foreach(POApportionSubjectInfo oSubject in ht.Values)
			{
				if ( !isValidOnly || oSubject.Status == (int)AppEnum.BiStatus.Valid)
					sl.Add(oSubject,null);
			}
			return sl;
		}
		public Hashtable GetPOApportionSubjectHash()
		{
			string sql = "select * from po_apportion_subject";
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( !Util.HasMoreRow(ds))
				return null;
			Hashtable ht = new Hashtable(10);
			foreach(DataRow dr in ds.Tables[0].Rows)
			{
				POApportionSubjectInfo oSubject = new POApportionSubjectInfo();
				map(oSubject, dr);
				ht.Add(oSubject.SysNo,oSubject);
			}
			return ht;

		}
		private void map(POApportionSubjectInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.SubjectName = Util.TrimNull(tempdr["SubjectName"]);
			oParam.ListOrder = Util.TrimNull(tempdr["ListOrder"]);
			oParam.Status = Util.TrimIntNull(tempdr["Status"]);
		}
		private void map(POApportionInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.POSysNo = Util.TrimIntNull(tempdr["POSysNo"]);
			oParam.ApportionSubjectSysNo = Util.TrimIntNull(tempdr["ApportionSubjectSysNo"]);
			oParam.ApportionType = Util.TrimIntNull(tempdr["ApportionType"]);
			oParam.ExpenseAmt = Util.TrimDecimalNull(tempdr["ExpenseAmt"]);
		}
		private void map(POApportionItemInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.ApportionSysNo = Util.TrimIntNull(tempdr["ApportionSysNo"]);
			oParam.ProductSysNo = Util.TrimIntNull(tempdr["ProductSysNo"]);
		}
		private void map(POInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.POID = Util.TrimNull(tempdr["POID"]);
			oParam.VendorSysNo = Util.TrimIntNull(tempdr["VendorSysNo"]);
			oParam.StockSysNo = Util.TrimIntNull(tempdr["StockSysNo"]);
			oParam.ShipTypeSysNo = Util.TrimIntNull(tempdr["ShipTypeSysNo"]);
			oParam.PayTypeSysNo = Util.TrimIntNull(tempdr["PayTypeSysNo"]);
			oParam.CurrencySysNo = Util.TrimIntNull(tempdr["CurrencySysNo"]);
			oParam.ExchangeRate = Util.TrimDecimalNull(tempdr["ExchangeRate"]);
			oParam.TotalAmt = Util.TrimDecimalNull(tempdr["TotalAmt"]);
			oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
			oParam.CreateUserSysNo = Util.TrimIntNull(tempdr["CreateUserSysNo"]);
			oParam.AuditTime = Util.TrimDateNull(tempdr["AuditTime"]);
			oParam.AuditUserSysNo = Util.TrimIntNull(tempdr["AuditUserSysNo"]);
            oParam.ReceiveTime = Util.TrimDateNull(tempdr["ReceiveTime"]);
            oParam.ReceiveUserSysNo = Util.TrimIntNull(tempdr["ReceiveUserSysNo"]);
			oParam.InTime = Util.TrimDateNull(tempdr["InTime"]);
			oParam.InUserSysNo = Util.TrimIntNull(tempdr["InUserSysNo"]);
			oParam.IsApportion = Util.TrimIntNull(tempdr["IsApportion"]);
			oParam.ApportionTime = Util.TrimDateNull(tempdr["ApportionTime"]);
			oParam.ApportionUserSysNo = Util.TrimIntNull(tempdr["ApportionUserSysNo"]);
            oParam.PayDate = Util.TrimDateNull(tempdr["PayDate"]);
			oParam.Memo = Util.TrimNull(tempdr["Memo"]);
			oParam.Note = Util.TrimNull(tempdr["Note"]);
			oParam.Status = Util.TrimIntNull(tempdr["Status"]);
		    oParam.POType = Util.TrimIntNull(tempdr["POType"]);
		    oParam.POInvoiceType = Util.TrimIntNull(tempdr["POInvoiceType"]);
            oParam.ManagerAuditMemo = Util.TrimNull(tempdr["ManagerAuditMemo"]);
            oParam.ManagerAuditStatus = Util.TrimIntNull(tempdr["ManagerAuditStatus"]);
            oParam.POInvoiceDunDesc = Util.TrimNull(tempdr["POInvoiceDunDesc"]);
            oParam.InvoiceExpectReceiveDate = Util.TrimDateNull(tempdr["InvoiceExpectReceiveDate"]);
            oParam.POShipTypeSysNo = Util.TrimIntNull(tempdr["POShipTypeSysNo"]);
            oParam.DeliveryDate = Util.TrimDateNull(tempdr["DeliveryDate"]);
            oParam.CustomerSysNo = Util.TrimIntNull(tempdr["CustomerSysNo"]);
		}
        
		private void map(POItemInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.POSysNo = Util.TrimIntNull(tempdr["POSysNo"]);
			oParam.ProductSysNo = Util.TrimIntNull(tempdr["ProductSysNo"]);
			oParam.Quantity = Util.TrimIntNull(tempdr["Quantity"]);
			oParam.Weight = Util.TrimIntNull(tempdr["Weight"]);
			oParam.OrderPrice = Util.TrimDecimalNull(tempdr["OrderPrice"]);
			oParam.ApportionAddOn = Util.TrimDecimalNull(tempdr["ApportionAddOn"]);
			oParam.UnitCost = Util.TrimDecimalNull(tempdr["UnitCost"]);
            oParam.OrderQty = Util.TrimIntNull(tempdr["OrderQty"]);
		}
		public void InsertPOApportionSubject(POApportionSubjectInfo oParam)
		{
			string sql = "select top 1 sysno from po_apportion_subject where subjectname=" + Util.ToSqlString(oParam.SubjectName);
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds))
				throw new BizException("the subject name exists already");

			if ( oParam.SysNo == AppConst.IntNull )
			{
				string sql2 = "select isnull(max(sysno),0) as maxsysno from po_apportion_subject";
				DataSet ds2 = SqlHelper.ExecuteDataSet(sql2);
				oParam.SysNo = Util.TrimIntNull(ds2.Tables[0].Rows[0]["maxsysno"])+1;
			}
			new POApportionSubjectDac().Insert(oParam);
		}
		public void UpdatePOApportionSubject(POApportionSubjectInfo oParam)
		{
			string sql = "select top 1 sysno from po_apportion_subject where sysno <> " + oParam.SysNo + " and subjectname=" + Util.ToSqlString(oParam.SubjectName);
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds))
				throw new BizException("the subject name exists already");
			new POApportionSubjectDac().Update(oParam);
		}

		#region  getID 、getCurrentStatus
		private string getPOID(int sysNo)
		{
			return "2" + sysNo.ToString().PadLeft(9,'0');
		}
		public int getCurrentStatus(int poSysNo)
		{
			int status = AppConst.IntNull;

			string sql = "select status from po_master where sysno =" + poSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			
			if ( Util.HasMoreRow(ds))
				status = Util.TrimIntNull( ds.Tables[0].Rows[0]["status"] );

			return status;
		}
		#endregion
		public void CreatePOApportion(Hashtable htParam)
		{
			if ( htParam.Count == 0 )
				return;
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				foreach(POApportionInfo masterInfo in htParam.Keys)
				{
					masterInfo.SysNo = SequenceDac.GetInstance().Create("PO_Apportion_Sequence");
					int rowsAffected = new POApportionDac().Insert(masterInfo);
					if ( rowsAffected != 1)
						throw new BizException("insert po apportion master error");
					foreach(POApportionItemInfo itemInfo in masterInfo.itemHash.Values)
					{
						itemInfo.ApportionSysNo = masterInfo.SysNo;
						rowsAffected = new POApportionDac().InsertItem(itemInfo);
						if ( rowsAffected != 1)
							throw new BizException("insert po apportion item error");
					}

				}
				scope.Complete();
            }

		}
		public void InsertPOApportionItem(POInfo oPO, POApportionInfo oApportion, int productSysNo)
		{
			if ( oPO.Status != (int)AppEnum.POStatus.WaitingApportion )
				throw new BizException("the status is not waiting apportion, insert failed");
			
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				POApportionItemInfo oApportionItem = new POApportionItemInfo();
				oApportionItem.ApportionSysNo = oApportion.SysNo;
				oApportionItem.ProductSysNo = productSysNo;
				int rowsAffected = new POApportionDac().InsertItem(oApportionItem);
				if ( rowsAffected != 1)
					throw new BizException("insert po apportion master error");

				oApportion.itemHash.Add(oApportionItem.ProductSysNo, oApportionItem);

				scope.Complete();
            }

		}
		public void DeletePOApportionItem(POInfo oPO, POApportionInfo oApportion, int productSysNo)
		{
			if ( oPO.Status != (int)AppEnum.POStatus.WaitingApportion )
				throw new BizException("the status is not waiting apportion, insert failed");
			
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				int rowsAffected = new POApportionDac().DeleteItem(oApportion.SysNo, productSysNo);
				if ( rowsAffected != 1)
					throw new BizException("insert po apportion master error");

				oApportion.itemHash.Remove(productSysNo);

				scope.Complete();
            }

		}
		public void InsertPOApportionMaster(POInfo oMaster, POApportionInfo oApportionMaster)
		{
			foreach(POApportionInfo appMaster in oMaster.apportionHash.Keys)
			{
				if ( appMaster.ApportionSubjectSysNo == oApportionMaster.ApportionSubjectSysNo)
					throw new BizException("the same subject already exists");
			}

			if ( oMaster.Status != (int)AppEnum.POStatus.WaitingApportion )
				throw new BizException("the status is not waiting apportion, insert failed");

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				oApportionMaster.SysNo = SequenceDac.GetInstance().Create("PO_Apportion_Sequence");
				int rowsAffected = new POApportionDac().Insert(oApportionMaster);
				if ( rowsAffected != 1)
					throw new BizException("insert po apportion master error");

				oMaster.apportionHash.Add(oApportionMaster, null);

				scope.Complete();
            }

		}

		public void DeletePOApportionMaster(POInfo oMaster, int apportionMasterSysNo)
		{
			if ( oMaster.Status != (int)AppEnum.POStatus.WaitingApportion )
				throw new BizException("the status is not waiting apportion, insert failed");

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				string sql = "select top 1 sysno from po_apportion_item where apportionsysno = " + apportionMasterSysNo;
				DataSet ds = SqlHelper.ExecuteDataSet(sql);
				if ( Util.HasMoreRow(ds))
					throw new BizException("please delete apportion item first");

				int rowsAffected = new POApportionDac().DeleteMaster(apportionMasterSysNo);
				if ( rowsAffected != 1)
					throw new BizException("d po apportion master error");

				POApportionInfo oApportionMaster = null;
				foreach(POApportionInfo appMaster in oMaster.apportionHash.Keys)
				{
					if ( appMaster.SysNo == apportionMasterSysNo)
						oApportionMaster = appMaster;
				}

				oMaster.apportionHash.Remove(oApportionMaster);

				scope.Complete();
            }

		}

		public void ExportPOApportion(int poSysNo, int userSysNo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				POInfo masterInfo = LoadPO(poSysNo);

				if ( masterInfo.Status != (int)AppEnum.POStatus.WaitingApportion)
					throw new BizException("the status is not waiting apportion now, export failed");
				if ( masterInfo.apportionHash == null || masterInfo.apportionHash.Count == 0)
					throw new BizException("lack of apportion expense");

				//清除原来的apportion add on
				foreach(POItemInfo poItem in masterInfo.itemHash.Values)
				{
					poItem.ApportionAddOn = 0;
				}
				
				foreach(POApportionInfo apportionMaster in masterInfo.apportionHash.Keys)
				{
					int subTotalWeight = 0;
					int subTotalQuantity = 0;
					decimal subTotalMoney = 0;
					if ( apportionMaster.itemHash == null || apportionMaster.itemHash.Count == 0)
						throw new BizException("some of the apportion lack of items ");

					foreach(int productSysNo in apportionMaster.itemHash.Keys)
					{
						POItemInfo poItem = masterInfo.itemHash[productSysNo] as POItemInfo;
						if ( poItem == null )
							throw new BizException("po apportion item be not included in po item");
						subTotalWeight += poItem.Weight;
						subTotalMoney += poItem.Quantity*poItem.OrderPrice;
						subTotalQuantity += poItem.Quantity;
					}
					foreach(int productSysNo in apportionMaster.itemHash.Keys)
					{
						POItemInfo poItem = masterInfo.itemHash[productSysNo] as POItemInfo;
						if ( apportionMaster.ApportionType == (int)AppEnum.POApportionType.ByMoney )
						{
							if ( subTotalMoney != 0)
							{
								poItem.ApportionAddOn += apportionMaster.ExpenseAmt * (poItem.Quantity * poItem.OrderPrice / subTotalMoney ) / poItem.Quantity;
							}
						}
						if ( apportionMaster.ApportionType == (int)AppEnum.POApportionType.ByQuantity)
						{
							if ( subTotalQuantity != 0)
							{
								poItem.ApportionAddOn += apportionMaster.ExpenseAmt * (poItem.Quantity*1.0M / subTotalQuantity) /poItem.Quantity;
							}
						}
						if ( apportionMaster.ApportionType == (int)AppEnum.POApportionType.ByWeight)
						{
							if ( subTotalWeight != 0)
							{
								poItem.ApportionAddOn += apportionMaster.ExpenseAmt * (poItem.Weight*1.0M / subTotalWeight) /poItem.Quantity;
							}
						}
					}
				}

				foreach(POItemInfo poItem in masterInfo.itemHash.Values)
				{
					poItem.ApportionAddOn = Decimal.Round(poItem.ApportionAddOn,2);
					poItem.UnitCost = Decimal.Round(poItem.OrderPrice*masterInfo.ExchangeRate + poItem.ApportionAddOn,2);
					if( 1 != new PODac().UpdateItem(poItem))
						throw new BizException("update apportion add on error");
				}

				masterInfo.IsApportion = (int)AppEnum.YNStatus.Yes;
				masterInfo.ApportionUserSysNo = userSysNo;
				masterInfo.ApportionTime = DateTime.Now;

				Hashtable ht = new Hashtable(4);
				ht.Add("SysNo", masterInfo.SysNo);
				ht.Add("ApportionUserSysNo", masterInfo.ApportionUserSysNo);
				ht.Add("ApportionTime", masterInfo.ApportionTime);
				ht.Add("IsApportion", masterInfo.IsApportion);

				if( 1!= new PODac().UpdateMaster(ht))
					throw new BizException("update po master error");

				scope.Complete();
            }

		}
		
		public void CreatePO(POInfo oParam)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				if ( oParam.SysNo == AppConst.IntNull)
				{
					oParam.SysNo = SequenceDac.GetInstance().Create("PO_Sequence");
					oParam.POID = getPOID(oParam.SysNo);
				}

				oParam.TotalAmt = oParam.GetTotalAmt();

				//建立主表记录
				int rowsAffected = new PODac().InsertMaster(oParam);
				if(rowsAffected != 1)
					throw new BizException("insert po master error");
				foreach( POItemInfo item in oParam.itemHash.Values)
				{
					item.POSysNo = oParam.SysNo;
					item.UnitCost = Decimal.Round(item.OrderPrice * oParam.ExchangeRate + item.ApportionAddOn, 2);

					rowsAffected = new PODac().InsertItem(item);
					if ( rowsAffected != 1)
						throw new BizException("insert po item error");
				}

				scope.Complete();
            }
		}

        //添加采购信息基本信息
        public void CreateMasterPO(POInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                if (oParam.SysNo == AppConst.IntNull)
                {
                    oParam.SysNo = SequenceDac.GetInstance().Create("PO_Sequence");
                    oParam.POID = getPOID(oParam.SysNo);
                }

                oParam.TotalAmt = oParam.GetTotalAmt();

                //建立主表记录
                int rowsAffected = new PODac().InsertMaster(oParam);
                if (rowsAffected != 1)
                    throw new BizException("insert po master error");
                //foreach (POItemInfo item in oParam.itemHash.Values)
                //{
                //    item.POSysNo = oParam.SysNo;
                //    item.UnitCost = Decimal.Round(item.OrderPrice * oParam.ExchangeRate + item.ApportionAddOn, 2);

                //    rowsAffected = new PODac().InsertItem(item);
                //    if (rowsAffected != 1)
                //        throw new BizException("insert po item error");
                //}

                scope.Complete();
            }
        }

        //添加采购商品信息
        public void CreatePOItem(POInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                //if (oParam.SysNo == AppConst.IntNull)
                //{
                //    oParam.SysNo = SequenceDac.GetInstance().Create("PO_Sequence");
                //    oParam.POID = getPOID(oParam.SysNo);
                //}

                oParam.TotalAmt = oParam.GetTotalAmt();

                //建立主表记录
                //int rowsAffected = new PODac().InsertMaster(oParam);
                //if (rowsAffected != 1)
                //    throw new BizException("insert po master error");
                foreach (POItemInfo item in oParam.itemHash.Values)
                {
                    item.POSysNo = oParam.SysNo;
                    item.UnitCost = Decimal.Round(item.OrderPrice * oParam.ExchangeRate + item.ApportionAddOn, 2);

                    int rowsAffected = new PODac().InsertItem(item);
                    if (rowsAffected != 1)
                        throw new BizException("insert po item error");

                    //更新master total amt

                    oParam.TotalAmt = oParam.GetTotalAmt();
                    Hashtable ht = new Hashtable(2);
                    ht.Add("SysNo", oParam.SysNo);
                    ht.Add("TotalAmt", oParam.TotalAmt);
                    if (1 != new PODac().UpdateMaster(ht))
                        throw new BizException("expected one-row update failed, add item failed");
                }

                scope.Complete();
            }
        }

		public DataSet GetPOProductHistory(int productSysNo)
		{
			string sql = @"select 
							po_master.sysno, poid, vendor.sysno as VendorSysNo, vendorName, Quantity, OrderPrice, UnitCost, InTime
						from 
							po_master, po_item, vendor
						where
							po_master.sysno = po_item.posysno
						and	po_master.vendorsysno = vendor.sysno
						and	po_item.productsysno = @productsysno
						and po_master.status = @status
						order by intime desc";
			sql = sql.Replace("@productsysno", productSysNo.ToString());
			sql = sql.Replace("@status", ((int)AppEnum.POStatus.InStock).ToString());
			return SqlHelper.ExecuteDataSet(sql);
		}

		public DataSet GetPODs(Hashtable paramHash)
		{
		    bool AddUrgent = false;
            string sql0 = "select ";
			string sql = @" select1 
								po.sysno,poid,totalamt, vendor.vendorname, stock.stockname,
								con_create.username as createname,
								po.createtime,
								con_audit.username as auditname,
								po.audittime,
								con_in.username as inname,
								po.intime,
                                po.paydate,
								po.status 
							from 
								po_master po inner join vendor on po.vendorsysno = vendor.sysno
								inner join stock on po.stocksysno = stock.sysno
								inner join sys_user as con_create on po.createusersysno = con_create.sysno
								left join sys_user as con_audit on po.auditusersysno = con_audit.sysno
								left join sys_user as con_in on po.inusersysno = con_in.sysno 
                            where 1=1 ";
			if ( paramHash != null && paramHash.Count != 0 )
			{
				StringBuilder sb = new StringBuilder(100);
				foreach(string key in paramHash.Keys)
				{
					sb.Append(" and ");
					object item = paramHash[key];
					if ( key == "DateFromCreate")
					{
						sb.Append("CreateTime >= ").Append(Util.ToSqlString(item.ToString()));
					}
					else if ( key == "DateToCreate")
					{
						sb.Append("CreateTime <= ").Append(Util.ToSqlEndDate(item.ToString()));
					}
					else if ( key == "DateFromInStock")
					{
						sb.Append("InTime >= ").Append(Util.ToSqlString(item.ToString()));
					}
					else if ( key == "DateToInStock")
					{
						sb.Append("InTime <= ").Append(Util.ToSqlEndDate(item.ToString()));
					}
					else if ( key == "ProductSysNo")
					{
						sb.Append(" exists ( select top 1 sysno from po_item where po.sysno=po_item.posysno and productsysno = ").Append(Util.SafeFormat(item.ToString())).Append(" ) ");
					}
					else if ( key == "Status" )
					{
						sb.Append("po.Status = ").Append(item.ToString());
					}
                    else if(key == "POType")
                    {
                        sb.Append("po.potype = ").Append(item.ToString());
                        if (Int32.Parse(item.ToString()) == (int)AppEnum.POType.Urgent)
                            AddUrgent = true;
                    }
                    else if(key == "POInvoiceType")
                    {
                        sb.Append("po.POInvoiceType=").Append(item.ToString());
                    }
                    else if (key=="ShowNum")
					{
                        //if (item.ToString()=="1")
                        //{
                        //    sql0="select top 10";
                        //}
                        //else if(item.ToString()=="2")
                        //{
                        //    sql0="select top 50";
                        //}
                        //else if(item.ToString()=="3")
                        //{
                        //    sql0="select top 100";
                        //}
                        //else if
                        //{
                        //    sql0="select ";
                        //}
                        if (Util.IsInteger(item.ToString()))
                            sql0 = "select top " + item.ToString();
                        else
                            sql0 = "select top 50";
						sb.Append(" 1=1 ");
					}
					else if ( item is int)
					{
						sb.Append(key).Append("=" ).Append(item.ToString());
					}
					else if ( item is string)
					{
						sb.Append(key).Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
					}
				}
				sql += sb.ToString();
				sql = sql.Replace("select1", sql0);
			}
			else
			{
				sql = sql.Replace("select1", "select top 50");
			}

            if(AddUrgent)
            {
                sql += @" or po.sysno in (select distinct posysno from po_master pm inner join po_item as [pi] on pm.sysno=[pi].posysno 
                          where pm.status = 3 and [pi].productsysno in (
                                select distinct si.productsysno 
                                from so_master sm inner join so_item si on sm.sysno=si.sosysno and sm.status=1
                                inner join inventory inv on si.productsysno=inv.productsysno 
                                group by si.productsysno,inv.accountqty
                                having inv.accountqty<sum(si.quantity) )  )";
            }

		    sql += " order by po.sysno desc";
			return SqlHelper.ExecuteDataSet(sql);
		}

		public void UpdatePOMaster(POInfo oParam)
		{
			//主项可以更新note,memo,potype
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				//必须是初始状态
				if ( getCurrentStatus(oParam.SysNo) != (int)AppEnum.POStatus.Origin )
					throw new BizException("status is not origin now,  update failed");

				//设置 单号, 备注
				Hashtable ht = new Hashtable(3);
				ht.Add("SysNo", oParam.SysNo);
				ht.Add("Note", oParam.Note);
				ht.Add("Memo", oParam.Memo);
                ht.Add("POType",oParam.POType);
                ht.Add("POInvoiceType",oParam.POInvoiceType);
                ht.Add("DeliveryDate", oParam.DeliveryDate.ToString(AppConst.DateFormat));
                ht.Add("CustomerSysNo", oParam.CustomerSysNo);
                ht.Add("PayDate", oParam.PayDate);
				if ( 1!=new PODac().UpdateMaster(ht))
					throw new BizException("expected one-row update failed, update failed ");

				scope.Complete();
            }
		}

        public void UpdatePOMasterPayDate(POInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                Hashtable ht = new Hashtable(2);
                ht.Add("SysNo", oParam.SysNo);
                ht.Add("PayDate", oParam.PayDate);
                ht.Add("POInvoiceType", oParam.POInvoiceType);
                if (1 != new PODac().UpdateMaster(ht))
                    throw new BizException("expected one-row update failed, update failed ");

                scope.Complete();
            }
        }

		public void InsertPOItem(POInfo masterInfo, POItemInfo itemInfo)
		{
			if ( masterInfo.itemHash.ContainsKey(itemInfo.ProductSysNo))
			{
				throw new BizException("item duplicated!");
			}

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				
				//必须是初始
				if ( getCurrentStatus(masterInfo.SysNo) != (int)AppEnum.POStatus.Origin )
					throw new BizException("status is not origin now,  add item failed");

				//更新item
				if ( 1 != new PODac().InsertItem(itemInfo))
					throw new BizException("expected one-row update failed, add item failed");

				//更新 itemInfo 到 masterInfo
				masterInfo.itemHash.Add(itemInfo.ProductSysNo, itemInfo);

				//更新master total amt
				masterInfo.TotalAmt = masterInfo.GetTotalAmt();
				Hashtable ht = new Hashtable(2);
				ht.Add("SysNo", masterInfo.SysNo);
				ht.Add("TotalAmt", masterInfo.TotalAmt);
				if ( 1 != new PODac().UpdateMaster(ht))
					throw new BizException("expected one-row update failed, add item failed");

				scope.Complete();
            }
		}

		public void UpdatePOItem(POInfo masterInfo, POItemInfo itemInfo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				
				//必须是初始
				if ( getCurrentStatus(masterInfo.SysNo) != (int)AppEnum.POStatus.Origin )
					throw new BizException("status is not origin now,  update item failed");

				//更新item
				if ( 1 != new PODac().UpdateItem(itemInfo))
					throw new BizException("expected one-row update failed, update item failed");

				//更新 itemInfo 到 masterInfo
				masterInfo.itemHash.Remove(itemInfo.ProductSysNo);
				masterInfo.itemHash.Add(itemInfo.ProductSysNo, itemInfo);

				//更新master total amt
				masterInfo.TotalAmt = masterInfo.GetTotalAmt();
				Hashtable ht = new Hashtable(2);
				ht.Add("SysNo", masterInfo.SysNo);
				ht.Add("TotalAmt", masterInfo.TotalAmt);
				if ( 1 != new PODac().UpdateMaster(ht))
					throw new BizException("expected one-row update failed, update item failed");

				scope.Complete();
            }
		}

		public void DeletePOItem(POInfo masterInfo, int itemProductSysNo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				
				//必须是初始
				if ( getCurrentStatus(masterInfo.SysNo) != (int)AppEnum.POStatus.Origin )
					throw new BizException("status is not origin now,  delete item failed");

				//更新item
				if ( 1 != new PODac().DeleteItem(masterInfo.SysNo, itemProductSysNo))
					throw new BizException("expected one-row update failed, delete item failed");

				//更新 itemInfo 到 masterInfo
				masterInfo.itemHash.Remove(itemProductSysNo);

				//更新master total amt
				masterInfo.TotalAmt = masterInfo.GetTotalAmt();
				Hashtable ht = new Hashtable(2);
				ht.Add("SysNo", masterInfo.SysNo);
				ht.Add("TotalAmt", masterInfo.TotalAmt);
				if ( 1 != new PODac().UpdateMaster(ht))
					throw new BizException("expected one-row update failed, delete item failed");

				scope.Complete();
            }
		}

        public void DeletePOItems(POInfo masterInfo, string itemProductSysNos)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                //必须是初始
                if (getCurrentStatus(masterInfo.SysNo) != (int)AppEnum.POStatus.Origin)
                    throw new BizException("status is not origin now,  delete item failed");

                //更新item
                if (0 >=new PODac().DeleteItems(masterInfo.SysNo, itemProductSysNos))
                    throw new BizException("expected one-row update failed, delete item failed");

                //更新 itemInfo 到 masterInfo
                string[] _sysnos=itemProductSysNos.Split(',');
                //masterInfo.itemHash.Remove(itemProductSysNo);
                foreach (string item in _sysnos)
                {
                    if(Util.IsInteger(item))
                        masterInfo.itemHash.Remove(Convert.ToInt32(item));
                }

                //更新master total amt
                masterInfo.TotalAmt = masterInfo.GetTotalAmt();
                Hashtable ht = new Hashtable(2);
                ht.Add("SysNo", masterInfo.SysNo);
                ht.Add("TotalAmt", masterInfo.TotalAmt);
                if (1 != new PODac().UpdateMaster(ht))
                    throw new BizException("expected one-row update failed, delete item failed");

                scope.Complete();
            }
        }

        /// <summary>
        /// 审核待摊销
        /// </summary>
        /// <param name="masterSysNo"></param>
        /// <param name="userSysNo"></param>
        /// <param name="isForce">是否强制审核</param>
        public void VerifyApportion(int masterSysNo, int userSysNo, bool isForce)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                try
                {
                    //必须是初始状态
                    POInfo dbInfo = LoadPO(masterSysNo);
                    if (dbInfo == null)
                        throw new BizException("does not exist this po sysno");
                    if (dbInfo.Status != (int)AppEnum.POStatus.Origin)
                        throw new BizException("status is not origin now, verify apportion failed");
                    if (!isForce)
                    {
                        CheckPMPOAmt(dbInfo);
                    }

                    //设置 单号、状态和审核人
                    Hashtable ht = new Hashtable(4);

                    ht.Add("SysNo", masterSysNo);
                    ht.Add("Status", (int)AppEnum.POStatus.WaitingApportion);
                    ht.Add("AuditTime", DateTime.Now);
                    ht.Add("AuditUserSysNo", userSysNo);
                    if (1 != new PODac().UpdateMaster(ht))
                        throw new BizException("expected one-row update failed, verify failed ");

                    //设置采购在途数量
                    foreach (POItemInfo item in dbInfo.itemHash.Values)
                    {
                        InventoryManager.GetInstance().SetPurchaseQty(dbInfo.StockSysNo, item.ProductSysNo, item.Quantity);
                    }
                }
                catch (Exception ex)
                {
                    throw new BizException(ex.Message);
                }
                scope.Complete();
            }
        }

        /// <summary>
        /// 审核待收货
        /// </summary>
        /// <param name="masterSysNo"></param>
        /// <param name="userSysNo"></param>
        /// <param name="isForce">是否强制审核</param>
        public void VerifyReceive(int masterSysNo, int userSysNo, bool isForce)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                try
                {
                    //必须是初始状态
                    POInfo dbInfo = LoadPO(masterSysNo);
                    if (dbInfo == null)
                        throw new BizException("does not exist this po sysno");
                    if (dbInfo.Status != (int)AppEnum.POStatus.Origin
                        && dbInfo.Status != (int)AppEnum.POStatus.WaitingApportion)
                        throw new BizException("status is not origin or waiting apportion now, verify stock failed");
                    if (dbInfo.Status == (int)AppEnum.POStatus.WaitingApportion
                        && dbInfo.IsApportion == (int)AppEnum.YNStatus.No)
                        throw new BizException("missing apportion export");

                    if (!isForce)
                    {
                        CheckPMPOAmt(dbInfo);
                    }
                    //设置 单号、状态和审核人
                    Hashtable ht = new Hashtable(4);

                    ht.Add("SysNo", masterSysNo);
                    //ht.Add("Status", (int)AppEnum.POStatus.WaitingInStock);
                    ht.Add("Status", (int)AppEnum.POStatus.WaitingReceive);
                    ht.Add("AuditTime", DateTime.Now);
                    ht.Add("AuditUserSysNo", userSysNo);
                    if (1 != new PODac().UpdateMaster(ht))
                        throw new BizException("expected one-row update failed, verify failed ");

                    //设置采购在途数量
                    if (dbInfo.Status == (int)AppEnum.POStatus.Origin)
                    {
                        foreach (POItemInfo item in dbInfo.itemHash.Values)
                        {
                            InventoryManager.GetInstance().SetPurchaseQty(dbInfo.StockSysNo, item.ProductSysNo, item.Quantity);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new BizException(ex.Message);
                }
                scope.Complete();
            }
        }

        /// <summary>
        /// 返回PM当日已采购金额 与 当前PO采购金额的总和
        /// </summary>
        /// <param name="pmUserSysNo">po创建人</param>
        /// <param name="currentPOAmt">当前PO采购金额</param>
        /// <param name="poSysNo">当前修改PO的SysNo</param>
        /// <returns></returns>
        public decimal GetSumAmtByPMPOToday(int pmUserSysNo, decimal currentPOAmt, int poSysNo, DateTime createTime)
        {
            string sql = @"
select IsNull(sum(totalamt),0) sumamt 
from po_master 
where status > " + (int)AppEnum.POStatus.Origin + @"
      and createusersysno= " + pmUserSysNo + @" @SysNo      
      and createtime > " + Util.ToSqlString(createTime.ToString(AppConst.DateFormat)) + @"
      and createtime <= " + Util.ToSqlEndDate(createTime.ToString(AppConst.DateFormat));
            if (poSysNo == AppConst.IntNull)
                sql = sql.Replace("@SysNo", "");
            else
                sql = sql.Replace("@SysNo", " and sysno <> " + poSysNo);

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            return Util.TrimDecimalNull(ds.Tables[0].Rows[0]["sumamt"]) + currentPOAmt;
        }

        /// <summary>
        /// 检测当次PO的采购金额是否符合采购金额限制
        /// </summary>
        /// <param name="oParam">当前PO</param>
        /// <returns></returns>
        public void CheckPMPOAmt(POInfo oParam)
        {
            if (oParam != null)
            {
                PMPOAmtRestrictInfo oInfo = LoadPMPOAmtRestrict(AppConst.IntNull, oParam.CreateUserSysNo);
                if (oInfo != null)
                {
                    if (oParam.TotalAmt > 0)
                    {
                        if (oParam.TotalAmt > oInfo.EachPOAmtMax)
                        {
                            throw new BizException("当前审核的采购单金额超过每单采购金额上限");
                        }

                        decimal total = GetSumAmtByPMPOToday(oParam.CreateUserSysNo, oParam.TotalAmt, oParam.SysNo, oParam.CreateTime);
                        if (total > oInfo.DailyPOAmtMax)
                        {
                            throw new BizException("建单日采购金额总和超过每日采购金额上限");
                        }
                    }
                }
                else
                    throw new BizException("您还没建立采购金额限制，请通知PMD创建采购金额限制");
            }
            else
            {
                throw new BizException("信息丢失，请重试");
            }
        }

		public void CancelVerify(int masterSysNo, int userSysNo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				//不能有未作废的预付款
				string sqlPayItem = "select top 1 sysno from finance_popay_item where paystyle = " + (int)AppEnum.POPayItemStyle.Advanced
					+ " and status <> " + (int)AppEnum.POPayItemStatus.Abandon
					+ " and posysno = " + masterSysNo;
				DataSet dsPayItem = SqlHelper.ExecuteDataSet(sqlPayItem);
				if ( Util.HasMoreRow(dsPayItem))
					throw new BizException("cancel advanced pay bill first");
				//必须是初始状态
				POInfo dbInfo = LoadPO(masterSysNo);
				if ( dbInfo == null )
					throw new BizException("does not exist this po sysno");
				if ( dbInfo.Status != (int)AppEnum.POStatus.WaitingReceive 
					&& dbInfo.Status != (int)AppEnum.POStatus.WaitingApportion)
					throw new BizException("status is not waiting receive or waiting apportion now, cancel verify failed");
				


				//设置 单号、状态和审核人
				Hashtable ht = new Hashtable(4);

				ht.Add("SysNo", masterSysNo);
				ht.Add("Status", (int)AppEnum.POStatus.Origin);
				ht.Add("AuditTime", DateTime.Now);
				ht.Add("AuditUserSysNo", userSysNo);
				ht.Add("IsApportion", (int)AppEnum.YNStatus.No);
				if ( 1!=new PODac().UpdateMaster(ht))
					throw new BizException("expected one-row update failed, verify failed ");

				//设置采购在途数量
				foreach(POItemInfo item in dbInfo.itemHash.Values)
				{
					InventoryManager.GetInstance().SetPurchaseQty(dbInfo.StockSysNo, item.ProductSysNo, -1*item.Quantity);

					//清除摊销对成本的影响
					if ( item.ApportionAddOn != 0)
					{
						item.UnitCost = item.UnitCost - item.ApportionAddOn;
						item.ApportionAddOn = 0;
						new PODac().UpdateItem(item);
					}
				}
				//清除摊销的记录
				foreach(POApportionInfo oApportion in dbInfo.apportionHash.Keys)
				{
					foreach(POApportionItemInfo oApportionItem in oApportion.itemHash.Values)
					{
						if ( 1 != new POApportionDac().DeleteItem(oApportionItem.ApportionSysNo, oApportionItem.ProductSysNo) )
							throw new BizException("expected one-row update failed, delete apportion item failed ");
					}
					if ( 1 != new POApportionDac().DeleteMaster(oApportion.SysNo) )
						throw new BizException("expected one-row update failed, delete apportion master failed ");
				}

				
				scope.Complete();
            }
		}

        //采购单收货，锁定采购单，防止修改
        public void Receive(int masterSysNo, int userSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                //必须是初始状态
                POInfo dbInfo = LoadPO(masterSysNo);
                if (dbInfo == null)
                    throw new BizException("does not exist this po sysno");
                if (dbInfo.Status != (int)AppEnum.POStatus.WaitingReceive)
                    throw new BizException("status is not waiting receive now, receive failed");

                //设置 单号、状态和收货人
                Hashtable ht = new Hashtable(4);

                ht.Add("SysNo", masterSysNo);
                ht.Add("Status", (int)AppEnum.POStatus.WaitingInStock);
                ht.Add("ReceiveTime", DateTime.Now);
                ht.Add("ReceiveUserSysNo", userSysNo);

                //收货时更新付款日期
                VendorInfo vInfo = VendorManager.GetInstance().Load(dbInfo.VendorSysNo);
                if (vInfo != null || vInfo.APType != AppConst.IntNull)
                {
                    ht.Add("PayDate", PurchaseManager.GetInstance().CalcPOPayDate("new", DateTime.Now, vInfo.APType));
                }

                if (1 != new PODac().UpdateMaster(ht))
                    throw new BizException("expected one-row update failed, verify failed ");

                scope.Complete();
            }
        }

        //采购单收货，锁定采购单，防止修改
        public void CancelReceive(int masterSysNo, int userSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                //必须是初始状态
                POInfo dbInfo = LoadPO(masterSysNo);
                if (dbInfo == null)
                    throw new BizException("does not exist this po sysno");
                if (dbInfo.Status != (int)AppEnum.POStatus.WaitingInStock)
                    throw new BizException("status is not received now, cancel receive failed");

                //设置 单号、状态和收货人
                Hashtable ht = new Hashtable(4);
                ht.Add("SysNo", masterSysNo);
                ht.Add("Status", (int)AppEnum.POStatus.WaitingReceive);
                ht.Add("ReceiveTime", DateTime.Now);
                ht.Add("ReceiveUserSysNo", userSysNo);
                if (1 != new PODac().UpdateMaster(ht))
                    throw new BizException("expected one-row update failed, verify failed ");

                scope.Complete();
            }
        }

		public void InStock(int masterSysNo, int userSysNo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				//必须是初始状态
				POInfo dbInfo = LoadPO(masterSysNo);
				if ( dbInfo == null )
					throw new BizException("does not exist this po sysno");
				if ( dbInfo.Status != (int)AppEnum.POStatus.WaitingInStock ) 
					throw new BizException("status is not waiting instock now, in stock failed");

				//设置 单号、状态和审核人
				Hashtable ht = new Hashtable(4);

				ht.Add("SysNo", masterSysNo);
				ht.Add("Status", (int)AppEnum.POStatus.InStock);
				ht.Add("InTime", DateTime.Now);
				ht.Add("InUserSysNo", userSysNo);
				if ( 1!=new PODac().UpdateMaster(ht))
					throw new BizException("expected one-row update failed, verify failed ");

				//设置入库数量(包括清除采购在途), 成本, 最后一次采购价，最后一次采购商
				CurrencyInfo oCurrency = CurrencyManager.GetInstance().Load(dbInfo.CurrencySysNo);

				foreach(POItemInfo item in dbInfo.itemHash.Values)
				{
					UnitCostManager.GetInstance().SetCost(item.ProductSysNo, item.Quantity, item.UnitCost);
					InventoryManager.GetInstance().SetPOInStockQty(dbInfo.StockSysNo, item.ProductSysNo, item.Quantity);
					ProductManager.GetInstance().SetLastOrderPrice(item.ProductSysNo, Decimal.Round(item.OrderPrice*oCurrency.ExchangeRate,2));
					ProductManager.GetInstance().SetLastVendor(item.ProductSysNo, dbInfo.VendorSysNo);
				}

				//将没有支付的金额生成付款单
				POPayInfo  oPay = POPayManager.GetInstance().LoadPay(dbInfo.SysNo);

				//计算付款单的金额（poamt－已经支付的部分）
				decimal payItemAmt = 0;
				if ( oPay == null )
					payItemAmt = dbInfo.TotalAmt;
				else if ( oPay.POAmt - oPay.AlreadyPayAmt > 0)
					payItemAmt = oPay.POAmt - oPay.AlreadyPayAmt;

				if ( payItemAmt != 0)
				{
					POPayItemInfo oPayItem = new Icson.Objects.Finance.POPayItemInfo();
					oPayItem.POSysNo = dbInfo.SysNo;
					oPayItem.PayStyle = (int)AppEnum.POPayItemStyle.Normal;
					oPayItem.PayAmt =  payItemAmt;
					oPayItem.CreateTime = DateTime.Now;
					oPayItem.CreateUserSysNo = userSysNo;
					oPayItem.Note = "auto create during in stock";
					oPayItem.Status = (int)AppEnum.POPayItemStatus.Origin;
					POPayManager.GetInstance().InsertPOPayItem(oPayItem);
				}

				//生成到货通知邮件
				ProductNotifyManager.GetInstance().DoNotify(masterSysNo);

				scope.Complete();
            }
		}

		public void CancelInStock(int masterSysNo, int userSysNo)
		{
			//如果没有本入库以后的货卡记录是可以取消入库的。
			//处理财务部分
		}

		public void Abandon(int masterSysNo, int userSysNo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				//必须是初始状态
				POInfo dbInfo = LoadPO(masterSysNo);
				if ( dbInfo == null )
					throw new BizException("does not exist this po sysno");
				if ( dbInfo.Status != (int)AppEnum.POStatus.Origin ) 
					throw new BizException("status is not origin now, abandon failed");

				//设置 单号、状态和审核人
				Hashtable ht = new Hashtable(4);

				ht.Add("SysNo", masterSysNo);
				ht.Add("Status", (int)AppEnum.POStatus.Abandon);
				if ( 1!=new PODac().UpdateMaster(ht))
					throw new BizException("expected one-row update failed, verify failed ");

                ProductIDManager.GetInstance().AbandonProductIDsByPO(masterSysNo);

				scope.Complete();
            }
		}

		public void CancelAbandon(int masterSysNo, int userSysNo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            { 

				//必须是初始状态
				POInfo dbInfo = LoadPO(masterSysNo);
				if ( dbInfo == null )
					throw new BizException("does not exist this po sysno");
				if ( dbInfo.Status != (int)AppEnum.POStatus.Abandon ) 
					throw new BizException("status is not abandon now, cancel abandon failed");

				//设置 单号、状态和审核人
				Hashtable ht = new Hashtable(4);

				ht.Add("SysNo", masterSysNo);
				ht.Add("Status", (int)AppEnum.POStatus.Origin);
				if ( 1!=new PODac().UpdateMaster(ht))
					throw new BizException("expected one-row update failed, verify failed ");
				
				scope.Complete();
            }
		}

		/// <summary>
		/// Get product list which is waiting for purchaseing
		/// </summary>
		/// <returns></returns>
		public DataSet GetProductsToPurchase()
		{
			string sqlStr = @"select 
									productID,productname,accountQty,availableqty,(availableqty+virtualqty) as onlineqty,
									orderqty,sum(si.quantity) as outqty,purchaseqty,username
								from product
								inner join inventory on product.sysno = inventory.productsysno
								left join sys_user on product.ppmusersysno = sys_user.sysno
								inner join so_item si on si.productsysno = product.sysno
								inner join so_master sm on sm.sysno = si.sosysno and sm.status = 1
								group by productID,productname,accountQty,availableqty,(availableqty+virtualqty),orderqty,purchaseqty,username
								having accountqty<sum(si.quantity) 
								order by username,productID";

            //select 
            //    productID,productname,accountQty,availableqty,(availableqty+virtualqty) as onlineqty,
            //    orderqty,sum(si.quantity) as outqty,purchaseqty,username,sm.auditdeliverydate,case sm.auditdeliverytimespan when 1 then '上午' else '下午' end as auditdeliverytimeshow
            //from product
            //inner join inventory on product.sysno = inventory.productsysno
            //left join sys_user on product.ppmusersysno = sys_user.sysno
            //inner join so_item si on si.productsysno = product.sysno
            //inner join so_master sm on sm.sysno = si.sosysno and sm.status = 1
            //group by productID,productname,accountQty,availableqty,(availableqty+virtualqty),orderqty,purchaseqty,username,sm.auditdeliverydate,sm.auditdeliverytimespan 
            //having accountqty<sum(si.quantity) 
            //order by username,productID,sm.auditdeliverydate,sm.auditdeliverytimespan

			return SqlHelper.ExecuteDataSet(sqlStr);
		}

        public bool CheckPOIncludeWaitingOutStockProduct(int poSysNo)
        {
            string sql = @"select [pi].productsysno from po_item [pi] inner join so_item si on [pi].productsysno=si.productsysno 
                            inner join so_master sm on sm.sysno=si.sosysno and sm.status=@sostatus
                            inner join inventory inv on si.productsysno=inv.productsysno 
                            where posysno=@posysno 
                            group by [pi].productsysno,inv.accountqty
                            having inv.accountqty < sum(si.quantity)";
            sql = sql.Replace("@sostatus",((int)AppEnum.SOStatus.WaitingOutStock).ToString());
            sql = sql.Replace("@posysno", poSysNo.ToString());
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                return true;
            else
                return false;
        }

		public DataSet GetProductsBarcodes(string ProductSysNos)
		{
			string sqlStr = "select ProductSysNo,Barcode from Product_Barcode where ProductSysNo in("+ ProductSysNos +") order by ProductSysNo,Barcode";
			return SqlHelper.ExecuteDataSet(sqlStr);
		}

		public DataSet GetProductBarcodes(int ProductSysNo)
		{
			string sqlStr = "select a.ProductID,a.ProductName,b.ProductSysNo,b.Barcode from Product a, Product_Barcode b where a.SysNo = b.ProductSysNo and b.ProductSysNo = "+ ProductSysNo +" order by b.Barcode";
			return SqlHelper.ExecuteDataSet(sqlStr);
		}
        public DataSet GetProductBarcodeInfo(int ProductIDSysNo)
        {
            string sql = "select p_i.productsysno,i_s.position1 from product_id p_i inner join po_master po on p_i.posysno=po.sysno inner join inventory_stock i_s on po.stocksysno=i_s.stocksysno and p_i.productsysno=i_s.productsysno where p_i.sysno = " + ProductIDSysNo;
            return SqlHelper.ExecuteDataSet(sql);
        }

		public DataSet GetProductBarcodes(string ProductID)
		{
			string sqlStr = "select a.ProductID,a.ProductName,b.ProductSysNo,b.Barcode from Product a, Product_Barcode b where a.SysNo = b.ProductSysNo and a.ProductID = '"+ ProductID +"' order by b.Barcode";
			return SqlHelper.ExecuteDataSet(sqlStr);
		}

        public void PrintProductBarcode(int poSysNo)
        {
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=po.txt");
            HttpContext.Current.Response.Charset = "GB2312";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            HttpContext.Current.Response.ContentType = "application/vnd.ms-txt";

            System.IO.StringWriter tw = new System.IO.StringWriter();

            POInfo poInfo = LoadPO(poSysNo);
            int stockSysNo = poInfo.StockSysNo;

            string sql = @"select [pi].sysno,[pi].productsysno,inv.position1 as stockno from Product_ID [pi] 
                           left join Inventory_Stock inv on [pi].productsysno=inv.productsysno 
                           where [pi].status=@status and [pi].posysno=" + poSysNo + " and inv.stocksysno=" + stockSysNo + " order by [pi].sysno";
            sql = sql.Replace("@status", ((int)AppEnum.BiStatus.Valid).ToString());
            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            string sLine = "\"ID\",\"ProductID\",\"Quantity\",\"StockNo\"";

            tw.WriteLine(sLine);
            int i = 1;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sLine = "\"" + i + "\",\"" + Util.TrimIntNull(dr["productsysno"]) + "_" + Util.TrimIntNull(dr["sysno"]) + "\",\"1\",\"" + Util.TrimNull(dr["stockno"]) + "\"";
                tw.WriteLine(sLine);
                i++;
            }

            tw.Flush();
            tw.Close();
            HttpContext.Current.Response.Write(tw.ToString()); //Write the HTML back to the browser.
            HttpContext.Current.Response.End();
        }

        public void PrintProductIDBarcode(int ProductIDSysNo)
        {
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=po.txt");
            HttpContext.Current.Response.Charset = "GB2312";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            HttpContext.Current.Response.ContentType = "application/vnd.ms-txt";

            System.IO.StringWriter tw = new System.IO.StringWriter();

            string sql = "select p_i.productsysno,i_s.position1 from product_id p_i inner join po_master po on p_i.posysno=po.sysno inner join inventory_stock i_s on po.stocksysno=i_s.stocksysno and p_i.productsysno=i_s.productsysno where p_i.sysno = " + ProductIDSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return;

            string sLine = "\"ID\",\"ProductID\",\"Quantity\",\"StockNo\"";
            tw.WriteLine(sLine);
            sLine = "\"" + 1 + "\",\"" + Util.TrimNull(ds.Tables[0].Rows[0]["productsysno"]) + "_" + ProductIDSysNo + "\",\"1\",\"" + Util.TrimNull(ds.Tables[0].Rows[0]["position1"]) + "\"";
            tw.WriteLine(sLine);
            
            tw.Flush();
            tw.Close();
            HttpContext.Current.Response.Write(tw.ToString()); //Write the HTML back to the browser.
            HttpContext.Current.Response.End();
        }

	    #region Import
		public void ImportApportionSubject()
		{
			if ( !AppConfig.IsImportable)
				throw new BizException("Is Importable is false");
			
			string sql = " select top 1 sysno from po_apportion_subject";
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds) )
				throw new BizException("the table po apption subject is not empty");

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				string sqlSubject = "select sysno, ExpenseName as SubjectName, ExpenseID as ListOrder, Status from ipp2003..PO_Apportionment_Expense_Type";
				DataSet dsSubject = SqlHelper.ExecuteDataSet(sqlSubject);
				foreach(DataRow dr in dsSubject.Tables[0].Rows)
				{
					POApportionSubjectInfo oSubject = new POApportionSubjectInfo();
					map(oSubject, dr);
					if ( oSubject.ListOrder == AppConst.StringNull)
						oSubject.ListOrder = "z";
					this.InsertPOApportionSubject(oSubject);
				}
				POApportionSubjectInfo oExtSubject = new POApportionSubjectInfo();
				oExtSubject.SysNo = 0;
				oExtSubject.SubjectName = "导数据";
				oExtSubject.ListOrder = "zzz";
				oExtSubject.Status = (int)AppEnum.BiStatus.InValid;
				this.InsertPOApportionSubject(oExtSubject);
				
				scope.Complete();
            }
		}
		public void Import()
		{
			/* 涉及的问题 
			 * 1 还货记录的处理
			 * 2 库存的处理
			 * 3 状态的处理
			 * 4 还有单据id的对应，这个要特别注意
			 */
			if ( !AppConfig.IsImportable)
				throw new BizException("Is Importable is false");
			
			string sql = " select top 1 sysno from po_master";
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds) )
				throw new BizException("the table po master is not empty");

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				//因为po中有一些stock是错误的，对于有入库单的以入库单为准，没有入库单的设置成上海的就行了。
				int shanghaiStockSysNo = AppConst.IntNull;
				Hashtable htStock = StockManager.GetInstance().GetStockHash(false);
				foreach(StockInfo stockItem in htStock.Values)
				{
					if ( stockItem.StockName.IndexOf("上海")!=-1)
					{
						shanghaiStockSysNo = stockItem.SysNo;
						break;
					}
				}
				if ( shanghaiStockSysNo == AppConst.IntNull )
					throw new BizException("shanghai stock can't be find");

				/*
				 * 1 遍历所有的po
				 * 2 看该po是否有有效的入库记录，
				 *	 合计有效入库的数量，作为poitem中的数量；
				 *	 根据入库记录的采购价格和成本，作为poitem的采购价格、摊销和单位成本
				 *   如果有摊销金额，那么po的isapportion赋值
				 * 3 如果没有有效的入库记录，那么就用原来的poitem赋值当前的poitem，并且认为没有作摊销
				 * 4 如果在2中有摊销，到老的入库准备单中获取制单者和时间，按新的poitem的值进行反推的按数量摊销的摊销单
				 * 5 最后生成po
				 */

				string sqlPo = @"select 
								po.sysno, poid, vendor_con.newsysno as vendorsysno, user_con1.newsysno as createusersysno,
								isnull(purchasetime, createbaskettime) as createtime, user_con2.newsysno as auditusersysno,
								leaderapprovetime as audittime, po.status, shiptype_con.newsysno as shiptypesysno, paytype_con.newsysno as paytypesysno,
								po.note, po.memo, currency_con.newsysno as currencysysno, currency.exchangerate
								from 
								ipp2003..po_master po, 
								ippconvert..vendor vendor_con, 
								ippconvert..sys_user user_con1,
								ippconvert..sys_user user_con2,
								ippconvert..shiptype shiptype_con,
								ippconvert..paytype paytype_con,
								ippconvert..currency currency_con,
								ipp3..currency
								where 
								po.vendorsysno = vendor_con.oldsysno
								and po.createbasketusersysno = user_con1.oldsysno
								and po.leadersysno *= user_con2.oldsysno
								and po.shipviasysno = shiptype_con.oldsysno
								and po.paymenttypesysno = paytype_con.oldsysno
								and po.currencytype = currency_con.oldsysno
								and currency_con.newsysno = currency.sysno
								";
				DataSet dsPo = SqlHelper.ExecuteDataSet(sqlPo);
				if ( !Util.HasMoreRow(dsPo))
					return;
				foreach(DataRow dr in dsPo.Tables[0].Rows)
				{
					#region 从老po中提取的可以用于新po的值
					/* original po status           newstatus
					 * 0 shoppingbag				abandon
					 * 1 abolish shopping bag		abandon
					 * 2 unproved po				origin
					 * 3 abolished po				abandon
					 * 4 administrator proved po	waiting in stock
					 * 5 account proved po			waiting in stock
					 * 6 proved po					waiting in stock
					 * 8 prepared in-stock-bill		waiting apportion
					 * 9 part in warehouse			instock
					 * 10 full in warehouse			instock
					 */
					POInfo oPO = new POInfo();
					oPO.SysNo = Util.TrimIntNull(dr["sysno"]);
					oPO.POID = Util.TrimNull(dr["poid"]);
					oPO.VendorSysNo = Util.TrimIntNull(dr["vendorsysno"]);
					oPO.ShipTypeSysNo = Util.TrimIntNull(dr["shiptypesysno"]);
					oPO.PayTypeSysNo = Util.TrimIntNull(dr["paytypesysno"]);
					oPO.CurrencySysNo = Util.TrimIntNull(dr["currencysysno"]);
					oPO.ExchangeRate = Util.TrimDecimalNull(dr["exchangerate"]);
					oPO.CreateTime = Util.TrimDateNull(dr["CreateTime"]);
					oPO.CreateUserSysNo = Util.TrimIntNull(dr["CreateUserSysNo"]);
					oPO.AuditTime = Util.TrimDateNull(dr["AuditTime"]);
					oPO.AuditUserSysNo = Util.TrimIntNull(dr["AuditUserSysNo"]);
					oPO.Memo = Util.TrimNull(dr["Memo"]);
					oPO.Note = Util.TrimNull(dr["Note"]);

					#endregion

					/* orginal in_stock_master status
					 * -1 abolished
					 * 0 not in stock
					 * 2 in shelf
					 */
					#region 用老的poitem 来初始化新的poitem
					oPO.StockSysNo = shanghaiStockSysNo;

					int poStatus = Util.TrimIntNull(dr["status"]);
					switch(poStatus) 
					{
						case 0:
						case 1:
						case 3:
							oPO.Status = (int)AppEnum.POStatus.Abandon;
							break;
						case 2:
							oPO.Status = (int)AppEnum.POStatus.Origin;
							break;
						case 4:
						case 5:
						case 6:
							oPO.Status = (int)AppEnum.POStatus.WaitingInStock;
							break;
						case 8:
							oPO.Status = (int)AppEnum.POStatus.WaitingApportion;
							break;
						case 9:
						case 10:
							//将状态设置为等待入库，下面查到真正的入库单的时候再赋值为入库状态。
							oPO.Status = (int)AppEnum.POStatus.WaitingInStock;
							break;
						default:
							break;
					}

					string sqlPOItem = @"select b.newsysno as productsysno, cast(a.orderqty as int) as orderqty, a.foreignprice from ipp2003..po_item a, ippconvert..productbasic b
										where a.productsysno = b.oldsysno and posysno = " + oPO.SysNo;
					DataSet dsPOItem = SqlHelper.ExecuteDataSet(sqlPOItem);
					if ( Util.HasMoreRow(dsPOItem))
					{
							
						foreach(DataRow drPOItem in dsPOItem.Tables[0].Rows)
						{
							POItemInfo poItem = new POItemInfo();
							poItem.POSysNo = oPO.SysNo;
							poItem.ProductSysNo = Util.TrimIntNull(drPOItem["productsysno"]);
							poItem.Quantity = Util.TrimIntNull(drPOItem["orderqty"]);
							poItem.OrderPrice = Decimal.Round(Util.TrimDecimalNull(drPOItem["foreignprice"]),2);
							poItem.ApportionAddOn = 0;
							poItem.UnitCost = Decimal.Round(poItem.OrderPrice * oPO.ExchangeRate, 2);
							poItem.Weight = 0;
							oPO.itemHash.Add(poItem.ProductSysNo, poItem);
						}
					}
					#endregion

					//取最后一次入库的信息作为po的入库。
					string sqlInStockMaster = @"select 
													con_stock.newsysno as warehousesysno,
													ism.InStockTime, con_user.newsysno as InStockUserSysNo
												from 
													ipp2003..in_stock_master ism,
													ippconvert..stock con_stock,
													ippconvert..sys_user con_user
												where
													ism.warehousesysno = con_stock.oldsysno
												and	ism.InStockUserSysNo = con_user.oldsysno
												and (status=1 or status =2) and posysno ="
												+ oPO.SysNo + " order by ism.sysno desc";
					DataSet dsInStockMaster = SqlHelper.ExecuteDataSet(sqlInStockMaster);
					if ( Util.HasMoreRow( dsInStockMaster ))
					{
						#region 取有效的入库记录更新po, 生成item
						DataRow drInStockMaster = dsInStockMaster.Tables[0].Rows[0];
						oPO.InTime = Util.TrimDateNull(drInStockMaster["InStockTime"]);
						oPO.InUserSysNo = Util.TrimIntNull(drInStockMaster["InStockUserSysNo"]);
						oPO.StockSysNo = Util.TrimIntNull(drInStockMaster["WarehouseSysNo"]);
						oPO.Status = (int)AppEnum.POStatus.InStock;

						string sqlInStockItem = @"select b.newsysno as productsysno, cast(sum(realqty) as int) as quantity, a.productprice, a.foreignprice from ipp2003..in_stock_item as a, ippconvert..productbasic b
												where a.productsysno = b.oldsysno and instocksysno in
												( select distinct sysno from ipp2003..in_stock_master where (status = 1 or status=2) and posysno=" + oPO.SysNo + ")";
						sqlInStockItem += @"	group by b.newsysno, productprice, foreignprice
												order by b.newsysno";

						DataSet dsInStockItem = SqlHelper.ExecuteDataSet(sqlInStockItem);
						if ( Util.HasMoreRow(dsInStockItem))
						{
							//为了防止部分入库，要以入库单为准，所以要转移poitem（需要获取orderprice），然后清除poitem
							Hashtable htOldPOItem = new Hashtable(5);
							foreach(POItemInfo poItem in oPO.itemHash.Values)
							{
								htOldPOItem.Add(poItem.ProductSysNo, poItem);
							}
							oPO.itemHash.Clear();

							foreach(DataRow drInStockItem in dsInStockItem.Tables[0].Rows)
							{
								POItemInfo oPOItem = new POItemInfo();
								POItemInfo oOldPOItem = htOldPOItem[Util.TrimIntNull(drInStockItem["ProductSysNo"])] as POItemInfo;

								if ( oOldPOItem == null && oPO.SysNo == 1516)
								{
									oOldPOItem = new POItemInfo();
									oOldPOItem.OrderPrice = Decimal.Round(Util.TrimDecimalNull(drInStockItem["ProductPrice"]),2);
								}
								

								oPOItem.POSysNo = oPO.SysNo;
								oPOItem.ProductSysNo = Util.TrimIntNull(drInStockItem["ProductSysNo"]);
								oPOItem.Quantity = Util.TrimIntNull(drInStockItem["quantity"]);
								oPOItem.OrderPrice = oOldPOItem.OrderPrice;
								oPOItem.UnitCost = Decimal.Round(Util.TrimDecimalNull(drInStockItem["ProductPrice"]),2);

								decimal apportionAddOn = 0;

								if ( Math.Abs(oPOItem.UnitCost - oPOItem.OrderPrice)>= 0.05M )
								{
									apportionAddOn = oPOItem.UnitCost - oPOItem.OrderPrice*oPO.ExchangeRate;
								}
							
								
								oPOItem.ApportionAddOn = Decimal.Round(apportionAddOn,2);
								oPOItem.Weight = 0;

								if ( apportionAddOn != 0)
								{
									oPO.IsApportion = (int)AppEnum.YNStatus.Yes;
								}
								oPO.itemHash.Add(oPOItem.ProductSysNo, oPOItem);
							}
						}
						#endregion
					}



					Hashtable apportionHash = new Hashtable(10);
					#region 反推摊销单
					if ( oPO.IsApportion == AppConst.IntNull)
						oPO.IsApportion = (int)AppEnum.YNStatus.No;
					else
					{
						

						//只有1591以后的po才有入库准备单。估计是做ippversion2的时候没有导过来
						if ( oPO.SysNo < 1591 
							|| oPO.SysNo == 3446 || oPO.SysNo == 4234 ||oPO.SysNo==4235 ||oPO.SysNo==5261 
							|| oPO.SysNo == 5262 || oPO.SysNo == 5283 ||oPO.SysNo==5284 ||oPO.SysNo==5582) //3446确实没有作摊销，这是一个负采购
						{
							oPO.ApportionTime = oPO.CreateTime;
							oPO.ApportionUserSysNo = oPO.CreateUserSysNo;
						}
						else
						{
							string sqlPrepare = @"select user_con.newsysno as createusersysno, createtime 
												from ipp2003..in_stock_prepare_master a, ippconvert..sys_user as user_con
												where a.status<>-1 and a.creatorsysno = user_con.oldsysno and posysno=" + oPO.SysNo;
							DataSet dsPrepare = SqlHelper.ExecuteDataSet(sqlPrepare);
							if ( !Util.HasMoreRow(dsPrepare))
							{
								throw new BizException("need apportion, but missing prepare :" + oPO.SysNo);
							}
							DataRow drPrepare = dsPrepare.Tables[0].Rows[0];
							oPO.ApportionTime = Util.TrimDateNull(drPrepare["createtime"]);
							oPO.ApportionUserSysNo = Util.TrimIntNull(drPrepare["createusersysno"]);
						}

						int apportionIndex = -1;
						SortedList apportionSubjectList = GetPOApportionSubjectList();
						foreach(POItemInfo newPOItem in oPO.itemHash.Values)
						{
							POApportionInfo oPOApportion = new POApportionInfo();
							apportionHash.Add(oPOApportion, null);

							oPOApportion.POSysNo = oPO.SysNo;
								
							bool found = false;
							foreach(POApportionSubjectInfo itemSub in apportionSubjectList.Keys)
							{
								if ( itemSub.SysNo == apportionIndex )
								{
									found = true;
									break;
								}
							}
							if ( !found)
							{
								POApportionSubjectInfo newSubject = new POApportionSubjectInfo();
								newSubject.SysNo = apportionIndex;
								newSubject.SubjectName = "系统导入数据"+apportionIndex.ToString();
								newSubject.ListOrder = apportionIndex.ToString();
								newSubject.Status = (int)AppEnum.BiStatus.InValid;
								new POApportionSubjectDac().Insert(newSubject);
								apportionSubjectList = GetPOApportionSubjectList();
							}

							oPOApportion.ApportionSubjectSysNo = apportionIndex;
							apportionIndex--;
							//继续赋值

							oPOApportion.ApportionType = (int)AppEnum.POApportionType.ByQuantity;
							oPOApportion.ExpenseAmt = Decimal.Round(newPOItem.ApportionAddOn * newPOItem.Quantity,2);

							
							POApportionItemInfo oPOApportionItem = new POApportionItemInfo();
							oPOApportionItem.ApportionSysNo = oPOApportion.SysNo;
							oPOApportionItem.ProductSysNo = newPOItem.ProductSysNo;

							oPOApportion.itemHash.Add(oPOApportionItem.ProductSysNo, oPOApportionItem);

						}
						
					}
					#endregion

					CreatePO(oPO);
					CreatePOApportion(apportionHash);
					

					//处理采购在途的数量
					if ( oPO.Status == (int)AppEnum.POStatus.WaitingApportion 
						|| oPO.Status == (int)AppEnum.POStatus.WaitingInStock)
					{
						foreach(POItemInfo poitem2 in oPO.itemHash.Values)
						{
							InventoryManager.GetInstance().SetPurchaseQty(oPO.StockSysNo, poitem2.ProductSysNo, poitem2.Quantity);
						}

					}
				}
				
				#region 填充po的sysno
				string sqlMaxSysNo = "select top 1 sysno from ipp2003..po_master order by sysno desc";
				DataSet dsMax = SqlHelper.ExecuteDataSet(sqlMaxSysNo);
				if ( !Util.HasMoreRow(dsMax))
					throw new BizException("got max sysno error");
				int maxSysNo = Util.TrimIntNull(dsMax.Tables[0].Rows[0]["sysno"]);
				// 将自动生成的sysno填到目前的最大单据号
				int newSysNo;
				do 
				{
					newSysNo = SequenceDac.GetInstance().Create("PO_Sequence");
				} while( newSysNo < maxSysNo);
				#endregion
				
				
			scope.Complete();
            }

		}
		#endregion

        /// <summary>
        /// 商品是否第一次采购入库
        /// </summary>
        /// <param name="POSysNo"></param>
        /// <param name="ProductSysNo"></param>
        /// <returns></returns>
        public bool IsFirstPurchaseInStock(int POSysNo, int ProductSysNo)
        {
            string sql = @"select top 1 posysno from po_item inner join po_master on po_item.posysno=po_master.sysno 
                           where status=@status and productsysno=" + ProductSysNo + " order by intime";
            sql = sql.Replace("@status", ((int)AppEnum.POStatus.InStock).ToString());
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                if (Util.TrimIntNull(ds.Tables[0].Rows[0][0]) == POSysNo)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 更新PO的催票信息
        /// </summary>
        /// <param name="POSysNo"></param>
        /// <param name="ProductSysNo"></param>
        /// <returns></returns>
        public void UpdatePOInvoiceDunDesc(Hashtable oparam)
        {
            new PODac().UpdateMaster(oparam);
        }

        /// <summary>
        /// 计算付款日期
        /// </summary>
        /// <param name="poOperation">采购单操作（new 新建,audit 审核, receive 收货, instock 入库）</param>
        /// <param name="startDate">开始计算日期</param>
        /// <param name="apType">帐期类型</param>
        /// <returns></returns>
        public DateTime CalcPOPayDate(string poOperation,DateTime startDate, int apType)
        {
            DateTime dt = new DateTime();            
            switch (apType)
            {
                case 1:  //预付款
                    dt = startDate;
                    break;
                case 2:  //货到且票到
                    dt = startDate;
                    break;
                case 3:  //货到且票到1周
                    dt = startDate.AddDays(7);
                    break;
                case 4:  //货到且票到10天
                    dt = startDate.AddDays(10);
                    break;
                case 5:  //货到且票到2周
                    dt = startDate.AddDays(14);
                    break;
                case 6:  //货到且票到20天
                    dt = startDate.AddDays(20);
                    break;
                case 7:  //货到且票到一个月
                    dt = startDate.AddMonths(1);
                    break;
                case 8:  //货到且票到45天
                    dt = startDate.AddDays(45);
                    break;
                case 9:  //每月25日且票到
                    if (startDate.Day < 25) //25日之前的
                    {
                        dt = DateTime.Parse(startDate.Year.ToString() + "-" + startDate.Month.ToString() + "-25");
                    }
                    else //25日之后的下月25日
                    {
                        dt = DateTime.Parse(startDate.AddMonths(1).Year.ToString() + "-" + startDate.AddMonths(1).Month.ToString() + "-25");
                    }
                    break;
                case 10:  //每月10,25日且票到
                    if (startDate.Day < 10)  //10日之前的
                    {
                        dt = DateTime.Parse(startDate.Year.ToString() + "-" + startDate.Month.ToString() + "-10");
                    }
                    else if (startDate.Day < 25) //10-25日的
                    {
                        dt = DateTime.Parse(startDate.Year.ToString() + "-" + startDate.Month.ToString() + "-25");
                    }
                    else  //25日之后的下月10日
                    {
                        dt = DateTime.Parse(startDate.AddMonths(1).Year.ToString() + "-" + startDate.AddMonths(1).Month.ToString() + "-10");
                    }
                    break;
            }

            return DateTime.Parse(dt.ToString(AppConst.DateFormat));
        }

        /// <summary>
        /// 获取PM所在分组的小组组长(或主管)
        /// </summary>
        /// <param name="pmUserSysNo"></param>
        /// <returns></returns>
        public int GetTLUserSysNo(int pmUserSysNo)
        {
//            string sql = @"
//SELECT  PMUserSysNo
//FROM    dbo.PM_POAmtRestrict
//WHERE   IsPMD = " + (int)AppEnum.YNStatus.Yes + @"
//        AND PMGroupNo IN ( SELECT TOP 1
//                                    PMGroupNo
//                           FROM     dbo.PM_POAmtRestrict
//                           WHERE    PMUserSysNo = " + pmUserSysNo + @" )
//";
//            DataSet ds = SqlHelper.ExecuteDataSet(sql);
//            if (Util.HasMoreRow(ds))
//                return Util.TrimIntNull(ds.Tables[0].Rows[0]["PMUserSysNo"]);
//            else
//                return AppConst.IntNull;

            string sql = "select sysno from sys_user where pmgroupsysno in (select pmgroupsysno from sys_user where sysno=" + pmUserSysNo.ToString() + ") and flag=" + ((int)AppEnum.UserFlag.TL).ToString();
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                return Util.TrimIntNull(ds.Tables[0].Rows[0]["sysno"]);
            else
                return AppConst.IntNull;
        }

        public int GetPMDUserSysNo()
        {
            string sql = "select sysno from sys_user where flag=" + ((int)AppEnum.UserFlag.PMD).ToString();
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                return Util.TrimIntNull(ds.Tables[0].Rows[0]["sysno"]);
            else
                return AppConst.IntNull;
        }

        public DataSet GetPMPOAmtRestrict(Hashtable paramHash)
        {
            string sql = @"
select PMP.sysno,PMP.pmusersysno,PMP.DailyPOAmtMax,PMP.EachPOAmtMax,SU.Flag as IsPMD,SU.PMGroupSysNo as PMGroupNo,SU.UserName PMName,SU.Status  
from  PM_POAmtRestrict PMP
left join Sys_User SU on SU.SysNo = PMP.PMUserSysNo  
";
            if (paramHash != null && paramHash.Count != 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" Where 1=1 ");
                foreach (string key in paramHash.Keys)
                {
                    sb.Append(" AND ");
                    object item = paramHash[key];

                    if (key == "PMUserSysNo")
                        sb.Append(" PMP.PMUserSysNo = ").Append(item.ToString());
                    else if (item is int)
                        sb.Append(key).Append(" = ").Append(item.ToString());
                    else if (item is string)
                        sb.Append(key).Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                }
                sql += sb.ToString() + " Order by PMP.PMGroupNo ";
            }

            return SqlHelper.ExecuteDataSet(sql);

        }

        /// <summary>
        /// 获取PM采购金额限制(不知道SysNo时,使用AppConst.IntNull,但至少需要一个条件)
        /// </summary>
        /// <param name="SysNo">流水号</param>
        /// <param name="PMUserSysNo">PMUserSysNo</param>
        /// <returns></returns>
        public PMPOAmtRestrictInfo LoadPMPOAmtRestrict(int SysNo, int PMUserSysNo)
        {
            if (SysNo == AppConst.IntNull && PMUserSysNo == AppConst.IntNull)
            {
                throw new BizException("缺少参数");
            }
            string sql = "select top 1 * from PM_POAmtRestrict Where 1=1 ";
            if (SysNo != AppConst.IntNull)
            {
                sql += " and SysNo = " + SysNo;
            }
            else if (PMUserSysNo != AppConst.IntNull)
            {
                sql += " and PMUserSysNo = " + PMUserSysNo;
            }

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;
            else
            {
                PMPOAmtRestrictInfo oInfo = new PMPOAmtRestrictInfo();
                map(oInfo, ds.Tables[0].Rows[0]);
                return oInfo;
            }
        }

        private void map(PMPOAmtRestrictInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.PMUserSysNo = Util.TrimIntNull(tempdr["PMUserSysNo"]);
            oParam.PMGroupNo = Util.TrimIntNull(tempdr["PMGroupNo"]);
            oParam.IsPMD = Util.TrimIntNull(tempdr["IsPMD"]);
            oParam.DailyPOAmtMax = Util.TrimDecimalNull(tempdr["DailyPOAmtMax"]);
            oParam.EachPOAmtMax = Util.TrimDecimalNull(tempdr["EachPOAmtMax"]);
        }

        public void InsertPMPOAmtRestrict(PMPOAmtRestrictInfo oParam)
        {
            string sql = "select top 1 * from PM_POAmtRestrict where PMUserSysNo =" + oParam.PMUserSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                throw new BizException("已近存在该PM的相关采购金额限制，不能重复插入");
            }
            if (1 != new PMPOAmtRestrictDac().Insert(oParam))
            {
                throw new BizException("插入PM采购金额限制失败");
            }
        }

        public void UpdatePMPOAmtRestrict(PMPOAmtRestrictInfo oParam)
        {
            string sql = "select top 1 * from PM_POAmtRestrict where PMUserSysNo =" + oParam.PMUserSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
            {
                throw new BizException("不存在该PM的相关采购金额限制，更新失败");
            }
            if (1 != new PMPOAmtRestrictDac().Update(oParam))
            {
                throw new BizException("更新PM采购金额限制失败");
            }
        }

        public void DeletePMPOAmtRestrict(int sysNo)
        {
            string sql = "delete from PM_POAmtRestrict where SysNo =" + sysNo.ToString();
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
        }

	}
}