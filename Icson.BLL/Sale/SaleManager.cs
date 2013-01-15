using System;
using System.Data;
using System.Collections;
using System.Text;
using System.IO;
using System.Web;

using Icson.Utils;
using Icson.Objects;
using Icson.Objects.Sale;
using Icson.Objects.Basic;
using Icson.Objects.Finance;
using System.Transactions;
using Icson.DBAccess;
using Icson.DBAccess.Basic;
using Icson.DBAccess.Sale;
using Icson.BLL.Basic;
using Icson.BLL.Finance;
using Icson.Objects.ImportData;
using Icson.DBAccess.ImportData;
using Icson.BLL.RMA;
using Icson.Objects.RMA;
using Icson.BLL.Promotion;

namespace Icson.BLL.Sale
{
	/// <summary>
	/// Summary description for SaleManager.
	/// </summary>
	public class SaleManager
	{
		private SaleManager()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		private static SaleManager _instance;
		private  ArrayList StackSRList=new ArrayList(1);//用于计算最佳salerule组合，堆栈，存放salerulesysno
		private static Hashtable AllValidSRList= new Hashtable(50);
		private ArrayList assembleList=new ArrayList(1);//记录所有可能的salerule组合，存放hashtable
		public static Hashtable allocatedManHash=new Hashtable(); //在webconfig中记录 usersysno:ratio;usersysno2:radio... 

		public static SaleManager GetInstance()
		{
			if ( _instance == null )
			{
				_instance = new SaleManager();
                //_instance.InitAllocatedMap();                
			}
			return _instance;
		}

		#region Map Zone
		private void map(SaleRuleInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.SaleRuleName = Util.TrimNull(tempdr["SaleRuleName"]);
			oParam.Status = Util.TrimIntNull(tempdr["Status"]);
			oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
		}

		private void map(SaleRuleItemInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.SaleRuleSysNo = Util.TrimIntNull(tempdr["SaleRuleSysNo"]);
			oParam.ProductSysNo = Util.TrimIntNull(tempdr["ProductSysNo"]);
			oParam.Quantity = Util.TrimIntNull(tempdr["Quantity"]);
			oParam.Discount = Util.TrimDecimalNull(tempdr["Discount"]);
			oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
		}

		private void map(SaleGiftInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.ProductSysNo = Util.TrimIntNull(tempdr["ProductSysNo"]);
			oParam.GiftSysNo = Util.TrimIntNull(tempdr["GiftSysNo"]);
			oParam.ListOrder = Util.TrimNull(tempdr["ListOrder"]);
			oParam.Status = Util.TrimIntNull(tempdr["Status"]);
			oParam.CreateUserSysNo = Util.TrimIntNull(tempdr["CreateUserSysNo"]);
			oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
			oParam.AbandonUserSysNo = Util.TrimIntNull(tempdr["AbandonUserSysNo"]);
			oParam.AbandonTime = Util.TrimDateNull(tempdr["AbandonTime"]);
		}

        private void map(SOInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.SOID = Util.TrimNull(tempdr["SOID"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
            oParam.CustomerSysNo = Util.TrimIntNull(tempdr["CustomerSysNo"]);
            oParam.StockSysNo = Util.TrimIntNull(tempdr["StockSysNo"]);
            oParam.OrderDate = Util.TrimDateNull(tempdr["OrderDate"]);
            oParam.DeliveryDate = Util.TrimDateNull(tempdr["DeliveryDate"]);
            oParam.SalesManSysNo = Util.TrimIntNull(tempdr["SalesManSysNo"]);
            oParam.IsWholeSale = Util.TrimIntNull(tempdr["IsWholeSale"]);
            oParam.IsPremium = Util.TrimIntNull(tempdr["IsPremium"]);
            oParam.PremiumAmt = Util.TrimDecimalNull(tempdr["PremiumAmt"]);
            oParam.ShipTypeSysNo = Util.TrimIntNull(tempdr["ShipTypeSysNo"]);
            oParam.ShipPrice = Util.TrimDecimalNull(tempdr["ShipPrice"]);
            oParam.FreeShipFeePay = Util.TrimDecimalNull(tempdr["FreeShipFeePay"]);
            oParam.PayTypeSysNo = Util.TrimIntNull(tempdr["PayTypeSysNo"]);
            oParam.PayPrice = Util.TrimDecimalNull(tempdr["PayPrice"]);
            oParam.SOAmt = Util.TrimDecimalNull(tempdr["SOAmt"]);
            oParam.DiscountAmt = Util.TrimDecimalNull(tempdr["DiscountAmt"]);

            oParam.CouponType = Util.TrimIntNull(tempdr["CouponType"]);
            oParam.CouponCode = Util.TrimNull(tempdr["CouponCode"]);
            oParam.CouponAmt = Util.TrimDecimalNull(tempdr["CouponAmt"]);

            oParam.PointAmt = Util.TrimIntNull(tempdr["PointAmt"]);
            oParam.CashPay = Util.TrimDecimalNull(tempdr["CashPay"]);
            oParam.PointPay = Util.TrimIntNull(tempdr["PointPay"]);
            oParam.ReceiveAreaSysNo = Util.TrimIntNull(tempdr["ReceiveAreaSysNo"]);
            oParam.ReceiveContact = Util.TrimNull(tempdr["ReceiveContact"]);
            oParam.ReceiveName = Util.TrimNull(tempdr["ReceiveName"]);
            oParam.ReceivePhone = Util.TrimNull(tempdr["ReceivePhone"]);
            oParam.ReceiveCellPhone = Util.TrimNull(tempdr["ReceiveCellPhone"]);
            oParam.ReceiveAddress = Util.TrimNull(tempdr["ReceiveAddress"]);
            oParam.ReceiveZip = Util.TrimNull(tempdr["ReceiveZip"]);
            oParam.AllocatedManSysNo = Util.TrimIntNull(tempdr["AllocatedManSysNo"]);
            oParam.FreightUserSysNo = Util.TrimIntNull(tempdr["FreightUserSysNo"]);
            oParam.SetDeliveryManTime = Util.TrimDateNull(tempdr["SetDeliveryManTime"]);
            oParam.UpdateUserSysNo = Util.TrimIntNull(tempdr["UpdateUserSysNo"]);
            oParam.UpdateTime = Util.TrimDateNull(tempdr["UpdateTime"]);
            oParam.AuditUserSysNo = Util.TrimIntNull(tempdr["AuditUserSysNo"]);
            oParam.AuditTime = Util.TrimDateNull(tempdr["AuditTime"]);
            oParam.ManagerAuditUserSysNo = Util.TrimIntNull(tempdr["ManagerAuditUserSysNo"]);
            oParam.ManagerAuditTime = Util.TrimDateNull(tempdr["ManagerAuditTime"]);
            oParam.OutUserSysNo = Util.TrimIntNull(tempdr["OutUserSysNo"]);
            oParam.OutTime = Util.TrimDateNull(tempdr["OutTime"]);
            oParam.CheckQtyUserSysNo = Util.TrimIntNull(tempdr["CheckQtyUserSysNo"]);
            oParam.CheckQtyTime = Util.TrimDateNull(tempdr["CheckQtyTime"]);
            oParam.Memo = Util.TrimNull(tempdr["Memo"]);
            oParam.Note = Util.TrimNull(tempdr["Note"]);
            oParam.InvoiceNote = Util.TrimNull(tempdr["InvoiceNote"]);
            oParam.IsVAT = Util.TrimIntNull(tempdr["IsVAT"]);
            oParam.IsPrintPackageCover = Util.TrimIntNull(tempdr["IsPrintPackageCover"]);
            oParam.DeliveryMemo = Util.TrimNull(tempdr["DeliveryMemo"]);
            oParam.VATEMSFee = Util.TrimDecimalNull(tempdr["VATEMSFee"]);

            oParam.ExpectDeliveryDate = Util.TrimDateNull(tempdr["ExpectDeliveryDate"]);
            oParam.ExpectDeliveryTimeSpan = Util.TrimIntNull(tempdr["ExpectDeliveryTimeSpan"]);
            oParam.AuditDeliveryDate = Util.TrimDateNull(tempdr["AuditDeliveryDate"]);
            oParam.AuditDeliveryTimeSpan = Util.TrimIntNull(tempdr["AuditDeliveryTimeSpan"]);
            oParam.SentDate = Util.TrimDateNull(tempdr["SentDate"]);
            oParam.SentTimeSpan = Util.TrimIntNull(tempdr["SentTimeSpan"]);

            oParam.SignByOther = Util.TrimIntNull(tempdr["SignByOther"]);
            oParam.HasServiceProduct = Util.TrimIntNull(tempdr["HasServiceProduct"]);

            oParam.CSUserSysNo = Util.TrimIntNull(tempdr["CSUserSysNo"]);
            oParam.HasExpectQty = Util.TrimIntNull(tempdr["HasExpectQty"]);
            oParam.IsMergeSO = Util.TrimIntNull(tempdr["IsMergeSO"]);

            oParam.InvoiceStatus = Util.TrimIntNull(tempdr["InvoiceStatus"]);
            oParam.InvoiceTime = Util.TrimDateNull(tempdr["InvoiceTime"]);
            oParam.InvoiceUpdateUserSysNo = Util.TrimIntNull(tempdr["InvoiceUpdateUserSysNo"]);
            oParam.AbandonInvoiceTime = Util.TrimDateNull(tempdr["AbandonInvoiceTime"]);
            oParam.RequestInvoiceType = Util.TrimIntNull(tempdr["RequestInvoiceType"]);
            oParam.RequestInvoiceTime = Util.TrimDateNull(tempdr["RequestInvoiceTime"]);

            oParam.InvoiceType = Util.TrimIntNull(tempdr["InvoiceType"]);
            oParam.IsLarge = Util.TrimIntNull(tempdr["IsLarge"]);
            oParam.PosFee = Util.TrimDecimalNull(tempdr["PosFee"]);
            oParam.DLSysNo = Util.TrimIntNull(tempdr["DLSysNo"]);
        }

		private void map(SOItemInfo oParam, DataRow tempdr)
		{
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.SOSysNo = Util.TrimIntNull(tempdr["SOSysNo"]);
            oParam.ProductSysNo = Util.TrimIntNull(tempdr["ProductSysNo"]);
            oParam.Quantity = Util.TrimIntNull(tempdr["Quantity"]);
            oParam.Weight = Util.TrimIntNull(tempdr["Weight"]);
            oParam.OrderPrice = Util.TrimDecimalNull(tempdr["OrderPrice"]);
            oParam.Price = Util.TrimDecimalNull(tempdr["Price"]);
            oParam.Cost = Util.TrimDecimalNull(tempdr["Cost"]);
            oParam.Point = Util.TrimIntNull(tempdr["Point"]);
            oParam.PointType = Util.TrimIntNull(tempdr["PointType"]);
            oParam.DiscountAmt = Util.TrimDecimalNull(tempdr["DiscountAmt"]);
            oParam.Warranty = Util.TrimNull(tempdr["Warranty"]);
            oParam.ProductType = Util.TrimIntNull(tempdr["ProductType"]);
            oParam.GiftSysNo = Util.TrimIntNull(tempdr["GiftSysNo"]);
            oParam.BaseProductType = Util.TrimIntNull(tempdr["BaseProductType"]);
            oParam.ExpectQty = Util.TrimIntNull(tempdr["ExpectQty"]);
		}

		private void map(SOVATInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.SOSysNo = Util.TrimIntNull(tempdr["SOSysNo"]);
			oParam.CustomerSysNo = Util.TrimIntNull(tempdr["CustomerSysNo"]);
			oParam.CompanyName = Util.TrimNull(tempdr["CompanyName"]);
			oParam.TaxNum = Util.TrimNull(tempdr["TaxNum"]);
			oParam.CompanyAddress = Util.TrimNull(tempdr["CompanyAddress"]);
			oParam.CompanyPhone = Util.TrimNull(tempdr["CompanyPhone"]);
			oParam.BankAccount = Util.TrimNull(tempdr["BankAccount"]);
			oParam.Memo = Util.TrimNull(tempdr["Memo"]);
			oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
			oParam.VATEMSFee = Util.TrimDecimalNull(tempdr["VATEMSFee"]);
		}

		private void map(SOSaleRuleInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.SOSysNo = Util.TrimIntNull(tempdr["SOSysNo"]);
			oParam.SaleRuleSysNo = Util.TrimIntNull(tempdr["SaleRuleSysNo"]);
			oParam.SaleRuleName = Util.TrimNull(tempdr["SaleRuleName"]);
			oParam.Discount = Util.TrimDecimalNull(tempdr["Discount"]);
			oParam.Times = Util.TrimIntNull(tempdr["Times"]);
			oParam.Note = Util.TrimNull(tempdr["Note"]);
			oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
		}
		#endregion

		#region Load Zone
		public SaleRuleInfo LoadSaleRule(int saleRuleSysNo)
		{
			string sql = "select * from salerule_master where sysno = "+saleRuleSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			SaleRuleInfo oSaleRule = new SaleRuleInfo();
			if(Util.HasMoreRow(ds))
			{
				map(oSaleRule,ds.Tables[0].Rows[0]);
				string sqlItem = "select * from salerule_item where salerulesysno ="+oSaleRule.SysNo;
				DataSet dsItem = SqlHelper.ExecuteDataSet(sqlItem);
				if(Util.HasMoreRow(dsItem))
				{
					foreach(DataRow dr in dsItem.Tables[0].Rows)
					{
						SaleRuleItemInfo oSaleRuleItem = new SaleRuleItemInfo();
						map(oSaleRuleItem,dr);
						oSaleRule.ItemHash.Add(oSaleRuleItem.ProductSysNo,oSaleRuleItem);
					}
				}
			}
			return oSaleRule;
		}

		public SaleRuleInfo LoadSaleRuleMaster(int saleRuleSysNo)
		{
			string sql = "select * from salerule_master where sysno ="+saleRuleSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			SaleRuleInfo oSaleRule = new SaleRuleInfo();
			if(Util.HasMoreRow(ds))
				map(oSaleRule,ds.Tables[0].Rows[0]);
			return oSaleRule;
		}

		public SaleRuleItemInfo LoadSaleRuleItem(int saleRuleItemSysNo)
		{
			string sql = "select * from salerule_item where sysno ="+saleRuleItemSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			SaleRuleItemInfo oSaleRuleItem = new SaleRuleItemInfo();
			if(Util.HasMoreRow(ds))
				map(oSaleRuleItem,ds.Tables[0].Rows[0]);
			return oSaleRuleItem;
		}
		
		public SOInfo LoadSO(int soSysNo)
		{
			string sql = "select * from so_master where sysno ="+soSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			SOInfo oSaleOrder = new SOInfo();
			if(Util.HasMoreRow(ds))
			{
                bool HasService = false;
				map(oSaleOrder,ds.Tables[0].Rows[0]);
				//load soitems
				string sqlItem = "select * from so_item where sosysno ="+oSaleOrder.SysNo;
				DataSet dsItem = SqlHelper.ExecuteDataSet(sqlItem);
				if(Util.HasMoreRow(dsItem))
				{
					foreach(DataRow dr in dsItem.Tables[0].Rows)
					{
						SOItemInfo oSOItem = new SOItemInfo();
						map(oSOItem,dr);
						oSaleOrder.ItemHash.Add(oSOItem.ProductSysNo,oSOItem);
                        if (oSOItem.BaseProductType == (int)AppEnum.ProductType.Service)
                        {
                            HasService = true;
                        }
					}
				}
				//load sosalerules
				string sqlSaleRule = "select * from so_salerule where sosysno ="+oSaleOrder.SysNo;
				DataSet dsSaleRule = SqlHelper.ExecuteDataSet(sqlSaleRule);
				if(Util.HasMoreRow(dsSaleRule))
				{
					foreach(DataRow dr in dsSaleRule.Tables[0].Rows)
					{
						SOSaleRuleInfo oSOSaleRule = new SOSaleRuleInfo();
						map(oSOSaleRule,dr);
						oSaleOrder.SaleRuleHash.Add(oSOSaleRule.SysNo,oSOSaleRule);
					}
				}
				//load vat
				if(oSaleOrder.IsVAT==(int)AppEnum.YNStatus.Yes)
				{
					string sqlVAT = "select * from so_valueadded_invoice where sosysno="+oSaleOrder.SysNo;
					DataSet dsVAT = SqlHelper.ExecuteDataSet(sqlVAT);
					if(Util.HasMoreRow(dsVAT))
					{
						map(oSaleOrder.VatInfo,dsVAT.Tables[0].Rows[0]);
					}
				}

                //load adways
                //SOAdwaysInfo adwaysInfo = LoadSOAdwaysBySOSysNo(oSaleOrder.SysNo);
                //if (adwaysInfo != null)
                //{
                //    oSaleOrder.AdwaysInfo = adwaysInfo;
                //}

                //load service product
                if (HasService)
                {
                    oSaleOrder.ServiceInfo = LoadSOService(oSaleOrder.SysNo);
                }
			}
			else 
				oSaleOrder = null;
			return oSaleOrder;
		}

		public SOInfo LoadSOMaster(int soSysNo)
		{
			string sql = "select * from so_master where sysno ="+soSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			SOInfo oSaleOrder = new SOInfo();
			if(Util.HasMoreRow(ds))
				map(oSaleOrder,ds.Tables[0].Rows[0]);
			else 
				oSaleOrder = null;
			return oSaleOrder;
		}

		public SOItemInfo LoadSOItem(int soItemSysNo)
		{
			string sql = "select * from so_item where sysno ="+soItemSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			SOItemInfo oSaleOrderItem = new SOItemInfo();
			if(Util.HasMoreRow(ds))
				map(oSaleOrderItem,ds.Tables[0].Rows[0]);
			else
				oSaleOrderItem = null;
			return oSaleOrderItem;
		}		

		public SOItemInfo LoadSOProduct(int productSysNo)
		{
			string sql = @"select null as sysno,null as sosysno,p.sysno as productsysno,null as quantity,null as expectqty ,p.weight,pp.currentprice as price,pp.unitcost as cost,pp.currentprice as orderprice,
						   pp.pointtype,null as discountamt,p.warranty,pp.point,null as producttype,null as giftsysno,p.producttype as baseproducttype
						   from product p
						   left join product_price pp on pp.productsysno = p.sysno
						   where p.sysno = "+productSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			SOItemInfo itemInfo = new SOItemInfo();
			if(Util.HasMoreRow(ds))
			{
				this.map(itemInfo,ds.Tables[0].Rows[0]);
			}
			else
				itemInfo = null;
			return itemInfo;
		}

		public Hashtable LoadSOProducts(Hashtable sysNoHash,int soItemType)
		{
			Hashtable productHash = new Hashtable(5);
			if(sysNoHash.Count!=0)
			{
				string strSysNo = "(";
				foreach(int sysNo in sysNoHash.Keys)
				{
					strSysNo += "'"+sysNo+"',";
				}
				strSysNo = strSysNo.TrimEnd(',');
				strSysNo += ")";
				string sql = @"select 
									null as sysno,null as sosysno,p.sysno as productsysno,null as quantity,null as expectqty ,p.weight,pp.currentprice as price,pp.unitcost as cost,pp.currentprice as orderprice,
									pp.pointtype,null as discountamt,p.warranty,pp.point,null as producttype,null as giftsysno,p.producttype as baseproducttype
								from 
									product p 
								left join product_price pp on pp.productsysno = p.sysno
								where 
									p.sysno in " + strSysNo;
				if(soItemType==(int)AppEnum.SOItemType.ForSale)
					sql += " and (pp.clearancesale = "+(int)AppEnum.YNStatus.Yes+" or pp.currentprice>=pp.unitcost) and p.status="+(int)AppEnum.ProductStatus.Show;
				else
					sql += " and p.status="+(int)AppEnum.ProductStatus.Valid;
				DataSet ds = SqlHelper.ExecuteDataSet(sql);
				if(Util.HasMoreRow(ds))
				{
					foreach(DataRow dr in ds.Tables[0].Rows)
					{
						SOItemInfo itemInfo = new SOItemInfo();
						this.map(itemInfo,dr);
						productHash.Add(itemInfo.ProductSysNo,itemInfo);
					}
				}
			}
			return productHash;
		}

		public SOVATInfo LoadSOVAT(int soSysNo)
		{
			string sql = "select * from so_valueadded_invoice where sosysno = "+soSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			SOVATInfo oVAT = new SOVATInfo();
			if(Util.HasMoreRow(ds))
				map(oVAT,ds.Tables[0].Rows[0]);
			return oVAT;
		}
		#endregion

		#region Insert Zone
		public int InsertSaleRuleMaster(SaleRuleInfo oParam)
		{			
			return new SaleRuleDac().InsertMaster(oParam);
		}

		public int InsertSaleRuleItem(SaleRuleItemInfo oParam)
		{	
			string sqlItemExist = "select * from salerule_item where SaleRuleSysNo="+oParam.SaleRuleSysNo+" and ProductSysNo="+oParam.ProductSysNo;
			DataSet dsItemExist = SqlHelper.ExecuteDataSet(sqlItemExist);
			if(Util.HasMoreRow(dsItemExist))
				throw new BizException("The same saleruleitem exists already");
			return new SaleRuleDac().InsertItem(oParam);
		}
        //更新促销商品
        public int UpdateSaleRuleItem(SaleRuleItemInfo oParam)
        {
            return new SaleRuleDac().UpdateItem(oParam);
        }

        //批量删除促销商品
        public int DeleteSaleRuleItemBatch(string sysNos)
        {
            return new SaleRuleDac().DeleteItemBatch(sysNos);
        }

        //获取促销商品信息
        public SaleRuleItemInfo GetSaleRuleItem(int itemSysNo)
        {
            SaleRuleItemInfo oRuleItem = new SaleRuleItemInfo();
            string sql = "select * from salerule_item where SysNo=" + itemSysNo.ToString();
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return oRuleItem;
            map(oRuleItem, ds.Tables[0].Rows[0]);
            return oRuleItem;
        }

		private int InsertSOMaster(SOInfo oParam)
		{	
			oParam.SysNo = SequenceDac.GetInstance().Create("SO_Sequence");
			oParam.SOID = this.BuildSOID(oParam.SysNo);
			return new SODac().InsertMaster(oParam);
		}

		private void InsertSOItem(SOItemInfo oParam,SOInfo soInfo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				new SODac().InsertItem(oParam);
				InventoryManager.GetInstance().SetOrderQty(soInfo.StockSysNo,oParam.ProductSysNo,oParam.Quantity);
				scope.Complete();
            }
		}

		public int InsertSOVAT(SOVATInfo oParam)
		{
			return new SOVATDac().Insert(oParam);
		}

		private int InsertSOSaleRule(SOSaleRuleInfo oParam)
		{
			return new SOSaleRuleDac().Insert(oParam);
		}

		public int InsertSaleGift(SaleGiftInfo oParam)
		{
			return new SaleGiftDac().Insert(oParam);
		}

		#endregion

		#region Import Zone
		public void ImportSaleRule()
		{
			if(!AppConfig.IsImportable)
				throw new BizException("IsImportable is false");
			string sqlmasterchk = "select top 1  * from SaleRule_Master";
			DataSet dsmasterchk = SqlHelper.ExecuteDataSet(sqlmasterchk);
			if(Util.HasMoreRow(dsmasterchk))
				throw new BizException("The SaleRule_Master is not empty");
			string sqlitemchk = "select top 1 * from SaleRule_Item";
			DataSet dsitemchk = SqlHelper.ExecuteDataSet(sqlitemchk);
			if(Util.HasMoreRow(dsitemchk))
				throw new BizException("The SaleRule_Item is not empty");
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				string sqlSearch = "select SysNo,Description as SaleRuleName,status from ipp2003..salerule_master";
				string sqlCreateUser = "select sysno from sys_user where username = '黄翔'";
				int CreateUserSysNo = Util.TrimIntNull(SqlHelper.ExecuteDataSet(sqlCreateUser).Tables[0].Rows[0][0]);
				DataSet ds1 = SqlHelper.ExecuteDataSet(sqlSearch);
				foreach(DataRow dr in ds1.Tables[0].Rows)
				{
					SaleRuleInfo oSaleRule = new SaleRuleInfo();
					oSaleRule.SaleRuleName = Util.TrimNull(dr["SaleRuleName"]);
					oSaleRule.Status = Util.TrimIntNull(dr["Status"]);
					oSaleRule.CreateUserSysNo = CreateUserSysNo;
					string sqlMasterExist = "select * from salerule_master where sysno="+oSaleRule.SysNo;
					DataSet dsMasterExist =  SqlHelper.ExecuteDataSet(sqlMasterExist);
					if(Util.HasMoreRow(dsMasterExist))
						throw new BizException("The same salerule exists already");
					ImportInfo oSaleRuleMasterConvert = new ImportInfo();
					oSaleRuleMasterConvert.OldSysNo = Util.TrimIntNull(dr["SysNo"]);
					this.InsertSaleRuleMaster(oSaleRule);
					oSaleRuleMasterConvert.NewSysNo = oSaleRule.SysNo;
					new ImportDac().Insert(oSaleRuleMasterConvert,"SaleRuleMaster");
					string sqlItem = @"select srm.newsysno as salerulesysno,pb.newsysno as productsysno,Quantity,discountAmt as discount 
									  from ipp2003..salerule_item si
									  inner join ippconvert..productbasic pb on pb.oldsysno = si.productsysno
									  inner join ippconvert..SaleRuleMaster srm on srm.oldsysno = si.salerulesysno
									  where si.SaleRuleSysNo ="+dr["SysNo"];
					DataSet ds3 = SqlHelper.ExecuteDataSet(sqlItem);
					if(Util.HasMoreRow(ds3))
					{
						foreach(DataRow dri in ds3.Tables[0].Rows)
						{
							SaleRuleItemInfo oSaleRuleItem = new SaleRuleItemInfo();
							oSaleRuleItem.SaleRuleSysNo = Util.TrimIntNull(dri["SaleRuleSysNo"]);
							oSaleRuleItem.ProductSysNo = Util.TrimIntNull(dri["ProductSysNo"]);
							oSaleRuleItem.Quantity = Util.TrimIntNull(dri["Quantity"]);
							oSaleRuleItem.Discount = Util.TrimDecimalNull(dri["Discount"]);
							this.InsertSaleRuleItem(oSaleRuleItem);
						}
					}
				}
				scope.Complete();
            }
		}

        public void ImportSO()
		{			
			if(!AppConfig.IsImportable)
				throw new BizException("IsImportable is false");
			string sqlMasterChk = "select top 1  * from SO_Master";
			DataSet dsMasterChk = SqlHelper.ExecuteDataSet(sqlMasterChk);
			if(Util.HasMoreRow(dsMasterChk))
				throw new BizException("The SO_Master is not empty");
			string sqlItemChk = "select top 1 * from so_item";
			DataSet dsItemChk = SqlHelper.ExecuteDataSet(sqlItemChk);
			if(Util.HasMoreRow(dsItemChk))
				throw new BizException("The SO_Item is not empty");
			string sqlCheckVAT = @"select top 1 * from so_valueAdded_invoice";
			DataSet dsCheckVAT = SqlHelper.ExecuteDataSet(sqlCheckVAT);
			if(Util.HasMoreRow(dsCheckVAT))
				throw new BizException("The VAT is not empty");
			string sqlCheckSaleRule = @"select top 1 * from so_salerule";
			DataSet dsCheckSaleRule = SqlHelper.ExecuteDataSet(sqlCheckSaleRule);
			if(Util.HasMoreRow(dsCheckSaleRule))
				throw new BizException("The SO_SaleRule is not empty");
			string sqlDataCleanUp = @"--整理so数据
									  --删除无主项的soitem
									  delete from ipp2003..so_item where sysno in (select si.sysno from ipp2003..so_item si left join ipp2003..so_master sm on si.sosysno = sm.sysno where sm.sysno is null)
									  --删除同一主项同一商品的重复明细
									  delete from ipp2003..so_item where sysno not in (select min(sysno) as sysno from ipp2003..so_item group by sosysno,productsysno having count(*)>1) 
									  and sosysno in (select sosysno from ipp2003..so_item group by sosysno,productsysno having count(*)>1) 
									  and productsysno in (select productsysno from ipp2003..so_item group by sosysno,productsysno having count(*)>1) 
									  --更新订单仓库编号0-->1
									  update ipp2003..so_master set warehousesysno = 1 where warehousesysno = 0
									  --更新订单收货区域编号：如果为空则更新为客户注册信息中的收货地区编号
									  update ipp2003..so_master set receiveareasysno = wi.shipareasysno from ipp2003..webuser_item wi where wi.webusersysno = ipp2003..so_master.sysno 
									  and (ipp2003..so_master.receiveareasysno is null or ipp2003..so_master.receiveareasysno = 1) and wi.shipareasysno is not null ";
			new ProductSaleTrendDac().Exec(sqlDataCleanUp);
			string sqlCheckSource = @"select * from ipp2003..so_item si left join ipp2003..so_master sm on si.sosysno = sm.sysno where sm.sysno is null;
									  select sosysno,productsysno,count(*) as number from ipp2003..so_item group by sosysno,productsysno having count(*)>1;
									  select soid,count(*) as number from ipp2003..so_master group by soid having count(*)>1;
									  select * from ipp2003..so_master sm left join ipp2003..stock s on s.sysno = sm.warehousesysno where s.sysno is null;";
			DataSet dsCheckSource = SqlHelper.ExecuteDataSet(sqlCheckSource);
			foreach(DataTable dt in dsCheckSource.Tables)
			{
				if(Util.HasMoreRow(dt))
					throw new BizException("The DataSource is uncorrect");
			}
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				//import master
				string sqlDefaultUser = @"select sysno from customer where CustomerID = 'UnknownUser'";
				DataSet dsDefaultUser = SqlHelper.ExecuteDataSet(sqlDefaultUser);
				string sqlDefaultPayType = @"select sysno from paytype where paytypeid = '0005'";
				DataSet dsDefaultPayType = SqlHelper.ExecuteDataSet(sqlDefaultPayType);
				string sqlMaster = @"select sm.sysno,soid,status,outtime,sodate as orderdate,deliverytime as DeliveryDate,ispremium,PremiumAmt,(case when sm.customsysno=-999 then "
									 +dsDefaultUser.Tables[0].Rows[0][0]+@" else sm.customsysno end) as customersysno, shipprice, payprice, soamt, st.newsysno as shiptypesysno, 
									 isnull(pt.newsysno,"+dsDefaultPayType.Tables[0].Rows[0][0]+@") as paytypesysno,isprintinvoice as isvat,suwh.newsysno as outusersysno,receiveaddress,
									 receivecontact, receivename,receivephone, receivezip, suft.newsysno as freightusersysno, audittime, suat.newsysno as auditusersysno, 
									 leaderaudittime as manageraudittime,sula.newsysno as  Managerauditusersysno,updatetime,suud.newsysno as updateusersysno,stock.newsysno as stocksysno,
									 memo,note as invoicenote,suam.newsysno as allocatedmansysno,iswholesale,pointpay,cashamt as cashpay,isnull(susa.newsysno,0) as SalesManSysNo,PointAmt,
									 isPrintPackageCover,deliverymemo,discountAmt,null as note, area.newsysno as receiveareasysno, null as receivecellphone
									 from ipp2003..so_master sm
									 left join ippconvert..shiptype st on st.oldsysno = sm.shiptypesysno 
									 left join ippconvert..paytype pt on pt.oldsysno = sm.paytypesysno
									 left join ippconvert..sys_user suwh on suwh.oldsysno = sm.warehouseusersysno
									 left join ippconvert..sys_user suft on suft.oldsysno = sm.freightusersysno
									 left join ippconvert..sys_user suat on suat.oldsysno = sm.auditusersysno
									 left join ippconvert..sys_user sula on sula.oldsysno = sm.leaderauditsysno
									 left join ippconvert..sys_user suud on suud.oldsysno = sm.updateusersysno
									 left join ippconvert..stock stock on stock.oldsysno = sm.warehousesysno
									 left join ippconvert..sys_user suam on suam.oldsysno = sm.allocatedmansysno
									 left join ippconvert..sys_user susa on susa.oldsysno = sm.salesmansysno
									 left join ippconvert..area area on area.oldsysno = sm.receiveareasysno";
				DataSet dsMaster = SqlHelper.ExecuteDataSet(sqlMaster);
				if(Util.HasMoreRow(dsMaster))
				{
					foreach(DataRow dr in dsMaster.Tables[0].Rows)
					{
						SOInfo oSOMaster = new SOInfo();
						this.map(oSOMaster,dr);
						new SODac().InsertMaster(oSOMaster);						
					}
				}
				//import item
				string sqlItem = @"select 1 as sysno,sosysno,pb.newsysno as productsysno,orderprice,price,(case when cost=-999 then 0 else isnull(cost,0) end) as cost,quantity,weight,
								   (case when returnpoint = -999 then 0 else isnull(returnpoint,0) end) as point,
								   (case when ltrim(rtrim(replace(warranty,CHAR(13)+char(10),'')))='' then '无' else isnull(warranty,'无') end) as warranty ,pointstatus as pointtype,discountamt,
								   getdate() as createtime,0 as producttype,null as giftsysno
								   from ipp2003..so_item si
								   left join ippConvert..productbasic pb on pb.oldsysno = si.productsysno";
				DataSet dsItem = SqlHelper.ExecuteDataSet(sqlItem);
				if(Util.HasMoreRow(dsItem))
				{
					foreach(DataRow drItem in dsItem.Tables[0].Rows)
					{
						SOItemInfo oSOItem = new SOItemInfo();
						this.map(oSOItem,drItem);
						new SODac().InsertItem(oSOItem);
					}
				}
				//import VAT
				string sqlVATCleanUp = @"--更新信息不全的增票记录
										 update ipp2003..sale_value_added_invoice set taxsignnumber = '信息不全' where ltrim(rtrim(replace(taxsignnumber,CHAR(13)+char(10),''))) = ''
			  							 update ipp2003..sale_value_added_invoice set companyaddress = '信息不全' where ltrim(rtrim(replace(companyaddress,CHAR(13)+char(10),''))) = ''
										 update ipp2003..sale_value_added_invoice set companyphone = '信息不全' where ltrim(rtrim(replace(companyphone,CHAR(13)+char(10),''))) = ''
										 update ipp2003..sale_value_added_invoice set bankAndAccount = '信息不全' where ltrim(rtrim(replace(bankAndAccount,CHAR(13)+char(10),''))) = ''
										 update ipp2003..sale_value_added_invoice set companyname = '信息不全' where ltrim(rtrim(replace(companyname,CHAR(13)+char(10),''))) = ''";
				new ProductSaleTrendDac().Exec(sqlVATCleanUp);				
				string sqlVAT = @"select 1 as sysno,sosysno,customsysno as customersysno,CompanyName,TaxSignNumber as TaxNum,CompanyAddress,CompanyPhone,BankAndAccount as bankaccount,Memo,getdate() as createtime
								  from ipp2003..sale_value_added_invoice sva";
				DataSet dsVAT =  SqlHelper.ExecuteDataSet(sqlVAT);
				if(Util.HasMoreRow(dsVAT))
				{
					foreach(DataRow drVAT in dsVAT.Tables[0].Rows)
					{
						SOVATInfo oVAT = new SOVATInfo();
						map(oVAT,drVAT);
						InsertSOVAT(oVAT);
					}
				}
				//import SaleRule				
				string sqlSaleRule = @"select 1 as sysno,sosysno,discountdescription as salerulename,discountamt as discount,discounttimes as times,memo as note,
									   srm.newsysno as salerulesysno,getdate() as createtime
									   from ipp2003..so_discount sd
									   inner join ippconvert..salerulemaster srm on srm.oldsysno = sd.salerulesysno";
				DataSet dsSaleRule = SqlHelper.ExecuteDataSet(sqlSaleRule);
				if(Util.HasMoreRow(dsSaleRule))
				{
					foreach(DataRow drSaleRule in dsSaleRule.Tables[0].Rows)
					{
						SOSaleRuleInfo oSaleRule = new SOSaleRuleInfo();
						map(oSaleRule,drSaleRule);
						InsertSOSaleRule(oSaleRule);
					}
				}
				//insert sequence
				string sqlMaxSysNo = @"select max(sysno) as sysno from so_master";
				DataSet dsMaxSysNo = SqlHelper.ExecuteDataSet(sqlMaxSysNo);
				int n = 0;
				while(n<Util.TrimIntNull(dsMaxSysNo.Tables[0].Rows[0][0]))
				{
					n = SequenceDac.GetInstance().Create("SO_Sequence");
				}
				scope.Complete();
            }
		}
		#endregion

		#region Update Zone
		public void UpdateSaleRuleMaster(SaleRuleInfo oParam)
		{
			new SaleRuleDac().UpdateMaster(oParam);
		}
		public void UpdateSaleGift(SaleGiftInfo oParam)
		{
			new SaleGiftDac().Update(oParam);
		}

        //设为无效，模拟删除
        public void SetInValid(string sysNos,int userSysNo,int status)
        {
            new SaleGiftDac().SetInValid(sysNos, userSysNo, status);
        }

        //更新赠品信息，不仅仅是作废
        public void UpdateAllGiftInfo(SaleGiftInfo oParam)
        {
            new SaleGiftDac().UpdateGift(oParam);
        }

		public void UpdateSOMaster(SOInfo oParam)
		{
			new SODac().UpdateSOMaster(oParam);
		}
		public void UpdateSOMaster(Hashtable paramHash)
		{
			new SODac().UpdateSOMaster(paramHash);
		}
		public void UpdateSOItem(SOItemInfo oParam)
		{
			new SODac().UpdateSOItem(oParam);
		}
		public void UpdateSOItem(Hashtable paramHash)
		{
			new SODac().UpdateSOItem(paramHash);
		}
		private void UpdateSOVAT(SOVATInfo vatInfo)
		{
			new SOVATDac().Update(vatInfo);
		}

		public void UpdateSOSaleRule(SOInfo soInfo)
		{
			
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				//清空原先SOSaleRule
				new SODac().DeleteSOSaleRule(soInfo.SysNo);
				if(soInfo.SaleRuleHash.Count!=0)
				{
					foreach(SOSaleRuleInfo srInfo in soInfo.SaleRuleHash.Values)
					{
						new SODac().InsertSOSaleRule(srInfo);
					}
				}
				scope.Complete();
            }
		}
		public void UpdateSO(SOInfo oParam)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				//更新主项
				SOInfo originInfo = this.LoadSO(oParam.SysNo);
				this.UpdateSOMaster(oParam);
				//判断item数量
				if(originInfo.ItemHash.Count!=oParam.ItemHash.Count)
					throw new BizException("SessionInfo expired,please reload this SaleOrder.");
				if(oParam.ItemHash.Count>0)
				{
					foreach(SOItemInfo item in oParam.ItemHash.Values)
					{
						if(originInfo.ItemHash.ContainsKey(item.ProductSysNo))
						{
							SOItemInfo originItem = (SOItemInfo)originInfo.ItemHash[item.ProductSysNo];
							//判断如果明细有变化就更新
							if(originItem.Cost!=item.Cost||originItem.Price!=item.Price||originItem.Quantity!=item.Quantity||originItem.Warranty!=item.Warranty
								||originItem.DiscountAmt!=item.DiscountAmt||originItem.GiftSysNo!=item.GiftSysNo||originItem.Weight!=item.Weight||originItem.Point!=item.Point)
							{
								//更新明细
                                if (oParam.IsWholeSale == 0) //零售的价格，以下订单的时候为准 2007-07-03
                                {
                                    item.Price = originItem.Price;
                                }
								this.UpdateSOItem(item);
							}
							//更新库存
							if(item.Quantity!=originItem.Quantity)
								InventoryManager.GetInstance().SetOrderQty(oParam.StockSysNo,item.ProductSysNo,(item.Quantity-originItem.Quantity));
						}
						else
							throw new BizException("SessionInfo expired,please reload this SaleOrder.");
					}
				}
				//判断SaleRule是否有变化
				if(originInfo.SaleRuleHash.Count==oParam.SaleRuleHash.Count)
				{
					foreach(SOSaleRuleInfo originSR in originInfo.SaleRuleHash.Values)
					{
						if(!oParam.SaleRuleHash.ContainsValue(originSR))
						{
							this.UpdateSOSaleRule(oParam);
							break;
						}
					}
				}
				else
				{
					//更新销售规则
					this.UpdateSOSaleRule(oParam);
				}
				//更新客户收货信息和积分
				Hashtable paramHash = new Hashtable();
				paramHash.Add("SysNo",oParam.CustomerSysNo);
				paramHash.Add("ReceiveAddress",oParam.ReceiveAddress);
				paramHash.Add("ReceiveContact",oParam.ReceiveContact);
				paramHash.Add("ReceiveName",oParam.ReceiveName);
				paramHash.Add("ReceivePhone",oParam.ReceivePhone);
				paramHash.Add("ReceiveCellPhone",oParam.ReceiveCellPhone);
				paramHash.Add("ReceiveZip",oParam.ReceiveZip);
				paramHash.Add("ReceiveAreaSysNo",oParam.ReceiveAreaSysNo);

				CustomerManager.GetInstance().Update(paramHash);

				PointManager.GetInstance().SetScore(oParam.CustomerSysNo,(originInfo.PointPay-oParam.PointPay), (int)AppEnum.PointLogType.UpdateSO, oParam.SysNo.ToString());
                //FreeShipFeeManager.GetInstance().SetFreeShipFee(oParam.CustomerSysNo,(originInfo.FreeShipFeePay - oParam.FreeShipFeePay),(int)AppEnum.FreeShipFeeLogType.UpdateSO,oParam.SysNo.ToString());
				scope.Complete();
            }
		}

		public void UpdateSOStatus(int soSysNo, int newStatus, int userSysNo)
		{
			/*	manager cancel__________
			 *							|
			 *							|--------waiting manager audit -----|
			 *							|				|					|
			 *							|--------waiting pay----------------|
			 *							|									|
			 *  customer cancel ---- origin-------------------------------waiting out stock ---单向---> out stock
			 *							/ \_____________________<--取消出库单向______________________________|
			 *						   /	
			 *						employee cancel
			 */
			Hashtable paramHash = new Hashtable();
			paramHash.Add("SysNo", soSysNo);
			paramHash.Add("Status",newStatus);
			int currentStatus = getCurrentSOStatus(soSysNo);
		    switch( newStatus )
			{
				case (int)AppEnum.SOStatus.ManagerCancel:
					if(currentStatus != (int)AppEnum.SOStatus.WaitingManagerAudit)
						throw new BizException("updateSOStatus: the current status is not WaitingManagerAudit, operation of ManagerCancel failed");
					break;
				case (int)AppEnum.SOStatus.EmployeeCancel:
					if(currentStatus != (int)AppEnum.SOStatus.Origin)
						throw new BizException("updateSOStatus: the current status is not Orgin, operation of EmployeeCancel failed");
					break;
				case (int)AppEnum.SOStatus.CustomerCancel:
					if(currentStatus != (int)AppEnum.SOStatus.Origin)
						throw new BizException("订单已经不是待审核状态，不能作废，请联系网站客服，谢谢");
					break;
				case (int)AppEnum.SOStatus.Origin:
					//已出库的订单不能回到Orign状态   -- //任何一个状态都可以到Orgin???
                    //if(currentStatus == (int)AppEnum.SOStatus.OutStock)
                    //    throw new BizException("updateSOStatus: the current status is outstock, operation of CancelAudit failed");
					break;
				case (int)AppEnum.SOStatus.WaitingOutStock:
					if(currentStatus==(int)AppEnum.SOStatus.Origin)
                    {
						paramHash.Add("AuditUserSysNo",userSysNo);
						paramHash.Add("AuditTime",DateTime.Now);
					}
					else if(currentStatus==(int)AppEnum.SOStatus.WaitingPay)
					{
					}
					else if(currentStatus==(int)AppEnum.SOStatus.WaitingManagerAudit)
					{
						paramHash.Add("ManagerAuditUserSysNo",userSysNo);
						paramHash.Add("ManagerAuditTime",DateTime.Now);
					}
					else
					{
						throw new BizException("updateSOStatus: the current status is not Orgin/WaitingPay/WaitingManagerAudit, operation of Audit(to waiting outstock) failed");
					}
					break;
				case (int)AppEnum.SOStatus.WaitingManagerAudit:
					if(currentStatus ==(int)AppEnum.SOStatus.Origin)
					{
						paramHash.Add("AuditUserSysNo",userSysNo);
						paramHash.Add("AuditTime",DateTime.Now);
					}
					else 
					{
						throw new BizException("updateSOStatus: the current status is not Orgin, operation of Audit(to waiting manager audit) failed");
					}
					break;
				case (int)AppEnum.SOStatus.WaitingPay:
					if(currentStatus == (int)AppEnum.SOStatus.Origin)
					{
						paramHash.Add("AuditUserSysNo",userSysNo);
						paramHash.Add("AuditTime",DateTime.Now);
					}
					else if(currentStatus==(int)AppEnum.SOStatus.WaitingManagerAudit)
					{
						paramHash.Add("ManagerAuditUserSysNo",userSysNo);
						paramHash.Add("ManagerAuditTime",DateTime.Now);
					}
					else 
					{
						throw new BizException("updateSOStatus: the current status is not Orgin, operation of Audit(to waiting pay) failed");
					}
					break;
				case (int)AppEnum.SOStatus.OutStock:
					if(currentStatus==(int)AppEnum.SOStatus.WaitingOutStock)
					{
						paramHash.Add("OutUserSysNo",userSysNo);
						paramHash.Add("OutTime",DateTime.Now);
					}
					else 
					{
						throw new BizException("updateSOStatus: the current status is not waiting out stock, operation of outstock failed");
					}
					break;
				default:
					throw new BizException("Unknown SOStatus");
			}
			if ( userSysNo != AppConst.IntNull )
			{
				paramHash.Add("UpdateUserSysNo",userSysNo);
				paramHash.Add("UpdateTime",DateTime.Now);
			}
			this.UpdateSOMaster(paramHash);
		}
		#endregion

		#region Search Zone
		public DataSet GetSRList(Hashtable paramHash)
		{
			string sql = @"select sm.sysno,salerulename,sm.status,su.username,sm.createtime,isnull(sum(quantity*discount),0) as discountAmt
						  from salerule_master sm
						  left join salerule_item si on si.salerulesysno = sm.sysno
						  left join sys_user su on su.sysno = sm.createusersysno
						  left join product p on p.sysno = si.productsysno
						  where 1=1	";
			if (paramHash!=null&&paramHash.Count > 0 )
			{
				StringBuilder sb = new StringBuilder();
				foreach(string key in paramHash.Keys)
				{
					sb.Append(" and ");
					object item = paramHash[key];
					if( key == "ProductID")
					{
						sb.Append("ProductID like ").Append(Util.ToSqlLikeString(item.ToString()));
					}
					else if( key == "Status")
					{
						sb.Append("sm.Status = ").Append(item.ToString());
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
			}
			else
			{
				sql = sql.Replace("select","select top 50 ");
			}
			sql += " group by sm.sysno,salerulename,sm.status,su.username,sm.createtime";
			sql += " order by sm.createtime desc ";
			return SqlHelper.ExecuteDataSet(sql);
		}
		
		public DataSet GetSRItemList(int paramSysNo)
		{
			string sql = @"select si.sysno,productid,productname,quantity,discount
						   from salerule_item si
						   inner join product p on p.sysno = si.productsysno
						   where salerulesysno = " + paramSysNo;
			return SqlHelper.ExecuteDataSet(sql);
		}

		public DataSet GetSaleGiftList(Hashtable paramHash)
		{
			string sql = @"select sg.sysno,pp.productname as MasterName,pp.sysno as MasterSysNo,pg.sysno as GiftSysNo,pg.productname as GiftName,listorder,sg.status,su.username,sg.createtime
						   from sale_gift sg
						   left join sys_user su on su.sysno = sg.createusersysno
						   left join product pp on pp.sysno = sg.productsysno
						   left join product pg on pg.sysno = sg.giftsysno
                           inner join category3 on category3.sysno=pp.c3sysno
                           inner join category2 on category3.c2sysno = category2.sysno 
						   inner join category1 on category2.c1sysno = category1.sysno
						   where sg.Status=0 ";
			if(paramHash!=null&&paramHash.Count>0)
			{
				StringBuilder sb = new StringBuilder();
				foreach(string key in paramHash.Keys)
				{
					object item = paramHash[key];
					sb.Append(" and ");
                    if (key == "ProductSysNo")
                        sb.Append("ProductSysNo=").Append(item.ToString());
                    else if (key == "SysNo")
                        sb.Append("sg.sysno!=").Append(item.ToString());
                    else if (key == "GiftSysNo")
                        sb.Append("GiftSysNo=").Append(item.ToString());
                    else if (key == "MasterProductList")
                        sb.Append("sg.ProductSysNo in(").Append(item.ToString()).Append(")");
                    else if (key == "Category")
                        sb.Append(item.ToString());
                    else if (key == "C3")
                        sb.Append("pp." + item.ToString());
                    else if (key == "KeyWords")
                    {
                        string[] Keys = (Util.TrimNull(item.ToString())).Split(' ');
                        if (Keys.Length == 1)
                        {
                            sb.Append("pp.productname like " + Util.ToSqlLikeString(item.ToString()));
                        }
                        else
                        {
                            string t = "1=1";
                            for (int i = 0; i < Keys.Length; i++)
                            {
                                t += " and pp.productname like " + Util.ToSqlLikeString(Keys[i]);
                            }

                            sb.Append(t);
                        }

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
            //else
            //{
            //    sql = sql.Replace("select","select top 50");
            //}
			return SqlHelper.ExecuteDataSet(sql);
		}

		public Hashtable GetSaleGiftHash(Hashtable sysNoHash)
		{
			Hashtable sgInfoHash = new Hashtable(2);
			if(sysNoHash.Count!=0)
			{
				string strSysNo = "(";
				foreach(int sysNo in sysNoHash.Keys)
				{
					strSysNo += "'"+sysNo+"',";
				}
				strSysNo = strSysNo.TrimEnd(',');
				strSysNo += ")";
				string sql = @"select 
								*
							from 
								sale_gift
							where
								status = "+(int)AppEnum.BiStatus.Valid+" and sysno in "+strSysNo;
				DataSet ds = SqlHelper.ExecuteDataSet(sql);
				if(Util.HasMoreRow(ds))
				{
					foreach(DataRow dr in ds.Tables[0].Rows)
					{
						SaleGiftInfo sgInfo = new SaleGiftInfo();
						this.map(sgInfo,dr);
						sgInfoHash.Add(sgInfo.SysNo,sgInfo);
					}
				}
			}
			return sgInfoHash;
		}
		/// <summary>
		/// 获取传入商品对应的在线显示的销售规则
		/// </summary>
		/// <param name="productSysNo"></param>
		/// <returns></returns>
		public DataSet GetSROnlineList(int productSysNo)
		{
			string sql = @"select 
								si.salerulesysno,sm.salerulename,sm.status ,si.productsysno,p.productname,si.quantity,si.discount,pp.currentprice
							from 
								salerule_item si
							inner join salerule_master sm on sm.sysno = si.salerulesysno and sm.status = 0
							inner join product p on p.sysno = si.productsysno
							inner join product_price pp on pp.productsysno = si.productsysno
							inner join 
								(select 
									distinct salerule_master.sysno 
								from 
									salerule_master 
								inner join salerule_item on salerule_item.salerulesysno = salerule_master.sysno
								where 
									salerule_item.productsysno = "+productSysNo.ToString()+@") a on a.sysno = sm.sysno
							left join 
								(select 
									distinct salerule_master.sysno
								from 
									salerule_item
								inner join salerule_master on salerule_item.salerulesysno = salerule_master.sysno
								inner join product on product.sysno = salerule_item.productsysno 
								inner join product_price pp on pp.productsysno = salerule_item .productsysno
								where 
									product.status<>"+(int)AppEnum.ProductStatus.Show+" or (pp.clearancesale = "+(int)AppEnum.YNStatus.No+@" and pp.currentprice<pp.unitcost)) b on b.sysno = sm.sysno
							where
								sm.status = "+(int)AppEnum.BiStatus.Valid+" and b.sysno is null";
			return SqlHelper.ExecuteDataSet(sql);
		}

		public DataSet GetProductList(Hashtable paramHash)
		{
			string sql = @"select p.sysno,p.productid,p.productname,p.weight,(isnull(i.availableqty,0)+isnull(i.virtualqty,0)) as onlineqty,isnull(pp.currentprice,0) as price,
						   sum(isnull(pgift.sysno,0)) as ifhasgift
						   from product p
						   left join inventory i on p.sysno = i.productsysno
						   left join product_price pp on pp.productSysNo = p.sysno
						   left join sale_gift sg on sg.productSysNo = p.sysno
						   left join product pgift on pgift.sysno = sg.giftsysno and pgift.status="+(int)AppEnum.ProductStatus.Valid
						 //+" where p.status <>" + (int)AppEnum.ProductStatus.Abandon +" and (sg.sysno is null or sg.status="+(int)AppEnum.BiStatus.Valid+")";
						   +" where p.status <>" + (int)AppEnum.ProductStatus.Abandon +" and (sg.sysno is null or sg.status<>-2)";
			if(paramHash.Count>0)
			{
				StringBuilder sb = new StringBuilder();
				foreach(object key in paramHash.Keys)
				{
					sb.Append(" and ");
					if((string)key=="ProductName")
					{
						sb.Append(" p.productname like ").Append( Util.ToSqlLikeString(paramHash[key].ToString()) );
					}
					else if((string)key=="ProductID")
					{
						sb.Append(" p.productid like ").Append( Util.ToSqlLikeStringR(paramHash[key].ToString()) );
					}
					else if ( (string)key == "ProductType")
					{
						sb.Append(" p.producttype = ").Append(paramHash[key].ToString());
					}
				}
				sql += sb.ToString();
			}
			else
			{
				sql = sql.Replace("select", "select top 50");
			}
			sql += " group by p.sysno,p.productid,p.productname,p.weight,(isnull(i.availableqty,0)+isnull(i.virtualqty,0)),isnull(pp.currentprice,0)";			 
			
			return SqlHelper.ExecuteDataSet(sql);
		}

		public DataSet GetGiftList(int productSysNo)
		{
			string sql = @"select p.sysno,p.productid,p.productname,p.status,p.weight,(i.availableqty+i.virtualqty) as onlineqty
						   from sale_gift sg
						   inner join product p on p.sysno = sg.giftsysno
						   inner join inventory i on i.productsysno = p.sysno
						   where sg.status = "+(int)AppEnum.BiStatus.Valid+" and p.status ="+(int)AppEnum.ProductStatus.Valid+" and sg.productsysno = "+productSysNo;
			return SqlHelper.ExecuteDataSet(sql);
		}

//		public Hashtable GetDistinctVATList(int customerSysNo)
//		{
//			string sql = @"select distinct null as sosysno,null as customersysno,companyname,taxnum,companyaddress,companyphone,bankaccount,null as memo,null as createtime
//						   from so_valueadded_invoice
//						   where customersysno ="+customerSysNo;
//			DataSet ds = SqlHelper.ExecuteDataSet(sql);			
//			DataTable dt = ds.Tables[0];
//			DataColumn dc = new DataColumn("SysNo",typeof(System.Int32));
//			dt.Columns.Add(dc);
//			Hashtable ht = new Hashtable();
//			int n = 0 ;
//			if(dt.Rows.Count>0)
//			{
//				foreach(DataRow dr in dt.Rows)
//				{
//					dr["SysNo"] = n;
//					n++;
//					SOVATInfo vatInfo = new SOVATInfo();
//					this.map(vatInfo,dr);
//					ht.Add(vatInfo.SysNo,vatInfo);
//				}
//			}
//			return ht;
//		}

		//2007-01-21 lucky
		public Hashtable GetDistinctVATList(int customerSysNo)
		{
			string sql = @"select distinct null as sosysno,null as customersysno,companyname,taxnum,companyaddress,companyphone,bankaccount,null as memo,null as createtime,VATEMSFee 
						   from so_valueadded_invoice
						   where customersysno ="+customerSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);			
			DataTable dt = ds.Tables[0];
			DataColumn dc = new DataColumn("SysNo",typeof(System.Int32));
			dt.Columns.Add(dc);
			Hashtable ht = new Hashtable();
			int n = 0 ;
			if(dt.Rows.Count>0)
			{
				foreach(DataRow dr in dt.Rows)
				{
					dr["SysNo"] = n;
					n++;
					SOVATInfo vatInfo = new SOVATInfo();
					this.map(vatInfo,dr);
					ht.Add(vatInfo.SysNo,vatInfo);
				}
			}
			return ht;
		}

		public DataSet GetSOList(Hashtable paramHash)
		{
			string sql = @"select sm.sysno,sm.soid,cm.customername,sm.receivename,sm.receivephone, sm.pointpay,sm.pointamt as pointget,
						   sm.orderdate as ordertime,sm.outtime,suat.username as allocatedman,suad.username as auditedman,suud.username as updatedman,sm.status as sostatus,pt.paytypename,fs.status as soincomestatus, 
							isPayWhenRecv, CashPay+PayPrice+ShipPrice-FreeShipFeePay+PremiumAmt-DiscountAmt-CouponAmt as TotalAmount,ShipPrice,FreeShipFeePay,HasExpectQty 
						   from so_master sm 
                           inner join area on sm.receiveareasysno=area.sysno 
						   left join sys_user suat on suat.sysno = sm.allocatedmansysno
						   left join sys_user suad on suad.sysno = sm.AuditUserSysNo
						   left join sys_user suud on suud.sysno = sm.UpdateUserSysNo
						   left join customer cm on cm.sysno = sm.customersysno
						   left join finance_soincome fs on fs.ordersysno = sm.sysno and fs.status>" + (int)AppEnum.SOIncomeStatus.Abandon+" and fs.ordertype="+(int)AppEnum.SOIncomeOrderType.SO
						+@" left join paytype pt on pt.sysno = sm.paytypesysno
						   where 1=1";
			if(paramHash.Count>0)
			{
				StringBuilder sb = new StringBuilder();
				foreach(string key in paramHash.Keys)
				{
					object item = paramHash[key];
					sb.Append(" and ");
					if(key=="ProductID")
					{
						sb.Append("sm.sysno in ");
						sb.Append("(select so_master.sysno from so_master ");
						sb.Append(" inner join so_item on so_item.sosysno=so_master.sysno ");
						sb.Append(" inner join product on product.sysno = so_item.productsysno");
						sb.Append(" where product.productid =").Append(Util.ToSqlString(item.ToString())).Append(")");
					}
					else if(key=="ShipAddress")
						sb.Append("sm.receiveaddress").Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
					else if(key=="ReceiveName")
						sb.Append("sm.receivename").Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
					else if(key=="ReceivePhone")
						sb.Append("sm.receivephone").Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
					else if(key=="SOStatus")
						sb.Append("sm.status").Append("=").Append(item.ToString());
					else if(key=="SOIncomeStatus")
						sb.Append("fs.status").Append("=").Append(item.ToString());
					else if(key=="StartDate")
					{
						sb.Append("sm.orderdate").Append(">=").Append(Util.ToSqlString(item.ToString()));
					}
					else if(key=="EndDate")
					{
						sb.Append("sm.orderdate").Append("<=").Append(Util.ToSqlEndDate(item.ToString()));
					}
					else if(key=="PayTypeSysNo")
					{
						sb.Append("sm.paytypesysno").Append("=").Append(item.ToString());
					}
                    else if (key=="HasService")
                    {
                        if (Convert.ToInt32(item.ToString()) == (int)AppEnum.YNStatus.Yes)
                        {
                            //sb.Append(" exists( select sosysno from so_service where so_service.sosysno = sm.sysno) ");
                            sb.Append(" exists( select sosysno from [SO_Item] where [SO_Item].sosysno = sm.sysno and [SO_Item].[BaseProductType]="+ (int)AppEnum.ProductType.Service +") ");
                        }
                        else if (Convert.ToInt32(item.ToString()) == (int)AppEnum.YNStatus.No)
                        {
                            //sb.Append(" not exists( select sosysno from so_service where so_service.sosysno = sm.sysno) ");
                            sb.Append(" not exists( select sosysno from [SO_Item] where [SO_Item].sosysno = sm.sysno and [SO_Item].[BaseProductType]="+ (int)AppEnum.ProductType.Service +")");
                        }
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
                    else if(key == "TotalAmt")
                    {
                        sb.Append("(CashPay+PayPrice+ShipPrice-FreeShipFeePay+PremiumAmt-DiscountAmt-CouponAmt)").Append("=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "HasExpectQty")
                    {
                        sb.Append("sm.HasExpectQty").Append("=").Append(item.ToString());
                    }
                    else if (key == "IsVat")
                    {
                        sb.Append("sm.IsVat").Append("=").Append(item.ToString());
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
				sql.Replace("select","select top 50");
			sql += " order by sm.sysno desc";
			return SqlHelper.ExecuteDataSet(sql);
		}

        public DataSet GetSOOutStockSummaryList(Hashtable paramHash)
        {
            string sql = @"select 'so' as type,sm.sysno,sm.sysno as sosysno,sm.soid,cm.customername,sm.receivename,sm.receivephone, sm.pointpay,sm.pointamt as pointget,sm.soamt,
						   sm.orderdate as ordertime,sm.outtime,suat.username as allocatedman,suad.username as auditedman,suud.username as updatedman,
                            sm.status as sostatus,pt.paytypename,fs.status as soincomestatus, 
							isPayWhenRecv, CashPay+PayPrice+ShipPrice-FreeShipFeePay+PremiumAmt-DiscountAmt-CouponAmt as TotalAmount,ShipPrice,FreeShipFeePay,CashPay,PayPrice,PremiumAmt,DiscountAmt,
                            st.shiptypename,su.username as freightman,sm.setdeliverymantime,sm.memo,fsv.voucherid,fs.ordertype,sm.pointamt 
						   from so_master sm(nolock)
						   left join sys_user suat(nolock) on suat.sysno = sm.allocatedmansysno
						   left join sys_user suad(nolock) on suad.sysno = sm.AuditUserSysNo
						   left join sys_user suud(nolock) on suud.sysno = sm.UpdateUserSysNo
						   left join customer cm(nolock) on cm.sysno = sm.customersysno
						   left join shiptype st(nolock) on st.sysno = sm.shiptypesysno 
						   left join sys_user su(nolock) on su.sysno = sm.freightusersysno 
						   left join finance_soincome fs(nolock) on fs.ordersysno = sm.sysno and fs.status>" + (int)AppEnum.SOIncomeStatus.Abandon + " and fs.ordertype=" + (int)AppEnum.SOIncomeOrderType.SO
                     + @"   left join finance_soincome_voucher fsv(nolock) on fsv.fsisysno = fs.sysno 
                           left join paytype pt(nolock) on pt.sysno = sm.paytypesysno
						   where 1=1";
            string sqlRO = @"select 'ro' as type,rm.sysno,sm.sysno as sosysno,rm.roid,cm.customername,rm.receivename,rm.receivephone,(-rm.originpointamt+rm.pointamt) as pointpay,rm.originpointamt as pointget,(-rm.cashamt + (-rm.originpointamt+rm.pointamt)/10.0) as soamt, 
                           rm.createtime as ordertime,rm.returntime as outtime,sucr.username as allocatedman,suad.username as auditedman,sure.username as updatedman,
                           sm.status as sostatus,pt.paytypename,fs.status as soincomestatus, 
						   isPayWhenRecv,-rm.cashamt as TotalAmount, '0' as ShipPrice,'0' as FreeShipFeePay,-rm.cashamt as CashPay,'0' as PayPrice,'0' as PremiumAmt,'0' as DiscountAmt,'0' as CouponAmt,
                           st.shiptypename,su.username as freightman,sm.setdeliverymantime,sm.memo,fsv.voucherid,fs.ordertype,(-rm.originpointamt) as pointamt 
						   from ro_master rm(nolock)
                           inner join rma_master rma(nolock) on rm.rmasysno = rma.sysno
                           inner join so_master sm(nolock) on rma.sosysno = sm.sysno
						   left join sys_user sucr(nolock) on sucr.sysno = rm.createusersysno
						   left join sys_user suad(nolock) on suad.sysno = rm.auditusersysno 
						   left join sys_user sure(nolock) on sure.sysno = rm.returnusersysno 
						   left join customer cm(nolock) on cm.sysno = sm.customersysno
						   left join shiptype st(nolock) on st.sysno = sm.shiptypesysno 
						   left join sys_user su(nolock) on su.sysno = sm.freightusersysno 
						   left join finance_soincome fs(nolock) on fs.ordersysno = rm.sysno and fs.status>" + (int)AppEnum.SOIncomeStatus.Abandon + " and fs.ordertype=" + (int)AppEnum.SOIncomeOrderType.RO
                     + @"  left join finance_soincome_voucher fsv(nolock) on fsv.fsisysno = fs.sysno 
                           left join paytype pt(nolock) on pt.sysno = sm.paytypesysno
						   where rm.status = " + (int)AppEnum.ROStatus.Returned;

            string sqlRR = @"select 'rr' as type,rr.sysno,sm.sysno as sosysno,rr.refundid,cm.customername,'' as receivename,'' as receivephone,(-rr.orgpointamt+rr.pointamt) as pointpay,rr.orgpointamt as pointget,(-rr.cashamt + (-rr.orgpointamt+rr.pointamt)/10.0) as soamt, 
                           rr.createtime as ordertime,rr.refundtime as outtime,sucr.username as allocatedman,suad.username as auditedman,sure.username as updatedman,
                           sm.status as sostatus,pt.paytypename,fs.status as soincomestatus, 
						   isPayWhenRecv,-rr.cashamt as TotalAmount, '0' as ShipPrice,'0' as FreeShipFeePay,-rr.cashamt as CashPay,'0' as PayPrice,'0' as PremiumAmt,'0' as DiscountAmt,'0' as CouponAmt,
                           st.shiptypename,su.username as freightman,sm.setdeliverymantime,sm.memo,fsv.voucherid,fs.ordertype,(-rr.orgpointamt) as pointamt 
						   from rma_refund rr(nolock) 
                           inner join so_master sm(nolock) on rr.sosysno = sm.sysno
						   left join sys_user sucr(nolock) on sucr.sysno = rr.createusersysno
						   left join sys_user suad(nolock) on suad.sysno = rr.auditusersysno 
						   left join sys_user sure(nolock) on sure.sysno = rr.refundusersysno 
						   left join customer cm(nolock) on cm.sysno = sm.customersysno
						   left join shiptype st(nolock) on st.sysno = sm.shiptypesysno 
						   left join sys_user su(nolock) on su.sysno = sm.freightusersysno 
						   left join finance_soincome fs(nolock) on fs.ordersysno = rr.sysno and fs.status>" + (int)AppEnum.SOIncomeStatus.Abandon + " and fs.ordertype=" + (int)AppEnum.SOIncomeOrderType.RO
                     + @"  left join finance_soincome_voucher fsv(nolock) on fsv.fsisysno = fs.sysno 
                           left join paytype pt(nolock) on pt.sysno = sm.paytypesysno
						   where rr.status = " + (int)AppEnum.RMARefundStatus.Refunded;

            if (paramHash.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sbRO = new StringBuilder();
                StringBuilder sbRR = new StringBuilder();

                foreach (string key in paramHash.Keys)
                {
                    object item = paramHash[key];
                    sb.Append(" and ");
                    sbRO.Append(" and ");
                    sbRR.Append(" and ");
                    if (key == "ProductID")
                    {
                        sb.Append("sm.sysno in ");
                        sb.Append("(select so_master.sysno from so_master(nolock) ");
                        sb.Append(" inner join so_item(nolock) on so_item.sosysno=so_master.sysno ");
                        sb.Append(" inner join product(nolock) on product.sysno = so_item.productsysno");
                        sb.Append(" where product.productid =").Append(Util.ToSqlString(item.ToString())).Append(")");
                    }
                    else if (key == "ShipAddress")
                    {
                        sb.Append("sm.receiveaddress").Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                        sbRO.Append("sm.receiveaddress").Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                        sbRR.Append("sm.receiveaddress").Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                    }
                    else if (key == "ReceiveName")
                    {
                        sb.Append("sm.receivename").Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                        sbRO.Append("sm.receivename").Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                        sbRR.Append("sm.receivename").Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                    }
                    else if (key == "ReceivePhone")
                    {
                        sb.Append("sm.receivephone").Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                        sbRO.Append("sm.receivephone").Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                        sbRR.Append("sm.receivephone").Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                    }
                    else if (key == "SOStatus")
                    {
                        sb.Append("sm.status").Append("=").Append(item.ToString());
                        sbRO.Append("sm.status").Append("=").Append(item.ToString());
                        sbRR.Append("sm.status").Append("=").Append(item.ToString());
                    }
                    else if (key == "SOIncomeStatus")
                    {
                        sb.Append("fs.status").Append("=").Append(item.ToString());
                        sbRO.Append("fs.status").Append("=").Append(item.ToString());
                        sbRR.Append("fs.status").Append("=").Append(item.ToString());
                    }
                    else if (key == "StartDate")
                    {
                        sb.Append("sm.outtime").Append(">=").Append(Util.ToSqlString(item.ToString()));
                        //sbRO.Append(" and rm.returntime").Append(">=").Append(Util.ToSqlString(item.ToString()));
                        sbRO.Append("sm.outtime").Append(">=").Append(Util.ToSqlString(item.ToString()));
                        sbRR.Append("sm.outtime").Append(">=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "EndDate")
                    {
                        sb.Append("sm.outtime").Append("<=").Append(Util.ToSqlEndDate(item.ToString()));
                        //sbRO.Append(" and rm.returntime").Append("<=").Append(Util.ToSqlEndDate(item.ToString()));
                        sbRO.Append("sm.outtime").Append("<=").Append(Util.ToSqlEndDate(item.ToString()));
                        sbRR.Append("sm.outtime").Append("<=").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "DateFrom")
                    {
                        sb.Append("sm.SetdeliveryManTime >= ").Append(Util.ToSqlString(item.ToString()));
                        sbRO.Append("sm.SetdeliveryManTime >= ").Append(Util.ToSqlString(item.ToString()));
                        sbRR.Append("sm.SetdeliveryManTime >= ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "DateTo")
                    {
                        sb.Append("sm.SetdeliveryManTime <= ").Append(Util.ToSqlString(item.ToString()));
                        sbRO.Append("sm.SetdeliveryManTime <= ").Append(Util.ToSqlString(item.ToString()));
                        sbRR.Append("sm.SetdeliveryManTime <= ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "PayTypeSysNo")
                    {
                        sb.Append("sm.paytypesysno").Append("=").Append(item.ToString());
                        sbRO.Append("sm.paytypesysno").Append("=").Append(item.ToString());
                        sbRR.Append("sm.paytypesysno").Append("=").Append(item.ToString());
                    }
                    else if (key == "TotalAmount")
                    {
                        sb.Append("(sm.CashPay+sm.PayPrice+sm.ShipPrice-sm.FreeShipFeePay+sm.PremiumAmt-sm.DiscountAmt-sm.CouponAmt)").Append("=").Append(item.ToString());
                        sbRO.Append("(sm.CashPay+sm.PayPrice+sm.ShipPrice-sm.FreeShipFeePay+sm.PremiumAmt-sm.DiscountAmt-sm.CouponAmt)").Append("=").Append(item.ToString());
                        sbRR.Append("(sm.CashPay+sm.PayPrice+sm.ShipPrice-sm.FreeShipFeePay+sm.PremiumAmt-sm.DiscountAmt-sm.CouponAmt)").Append("=").Append(item.ToString());
                    }
                    else if (key == "VoucherID")
                    {
                        sb.Append("fsv.voucherid ").Append("=").Append(Util.ToSqlString(item.ToString()));
                        sbRO.Append("fsv.voucherid ").Append("=").Append(Util.ToSqlString(item.ToString()));
                        sbRR.Append("fsv.voucherid ").Append("=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (item is int)
                    {
                        sb.Append(key).Append("=").Append(item.ToString());
                        sbRO.Append(key).Append("=").Append(item.ToString());
                        sbRR.Append(key).Append("=").Append(item.ToString());
                    }
                    else if (item is string)
                    {
                        sb.Append(key).Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                        sbRO.Append(key).Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                        sbRR.Append(key).Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                    }
                }
                sql += sb.ToString();
                sqlRO += sbRO.ToString();
                sqlRR += sbRR.ToString();
            }
            else
            {
                sql.Replace("select", "select top 50");
                sqlRO.Replace("select", "select top 50");
                sqlRR.Replace("select", "select top 50");
            }
            sql = sql + " union all " + sqlRO + " union all " + sqlRR;
            sql = "select * from (" + sql + " ) as v order by v.sosysno";

            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetSOOutStockList(Hashtable paramHash)
        {
//            string sql = @"select sm.sysno,sm.soid,st.shiptypename,pt.paytypename,sm.orderdate,sm.audittime,sm.deliverydate,sm.status,fs.status as paystatus,sm.isvat,
//                           sm.auditdeliverydate,sm.auditdeliverytimespan,
//						   (case when sum(p.islarge)>0 then 'Large' else 'Normal' end) as islarge,(case when sum((case when (i.availableqty+si.quantity)<0 then 1 else 0 end))>0 
//						   then  'Wait for replacements' when sum((case when (i.availableqty+si.quantity)>=0 and (its.availableqty+si.quantity)<0 then 1 else 0 end))>0 then 'Wait for transfer' 
//						   else 'Yes' end)   as isEnough ,c.customername
//						   from so_master sm 
//						   inner join so_item si on si.sosysno = sm.sysno
//						   left join finance_soincome fs on fs.ordersysno = sm.sysno and fs.ordertype = " + (int)AppEnum.SOIncomeOrderType.SO + " and fs.status>" + (int)AppEnum.SOIncomeStatus.Abandon
//                        + @" inner join product p on p.sysno = si.productsysno
//						   left join inventory i on i.productsysno = si.productsysno
//						   left join inventory_stock its on its.productsysno = si.productsysno and its.stocksysno = sm.stocksysno
//						   left join shiptype st on st.sysno = sm.shiptypesysno
//						   left join paytype pt on pt.sysno = sm.paytypesysno
//						   inner join customer c on c.sysno = sm.customersysno 
//                           left join area on sm.receiveareasysno = area.sysno
//						   where sm.status = " + (int)AppEnum.SOStatus.WaitingOutStock;
            string sql = @"select sm.sysno,sm.soid,st.shiptypename,pt.paytypename,sm.orderdate,sm.audittime,sm.deliverydate,sm.status,fs.status as paystatus,sm.isvat,
                           sm.auditdeliverydate,sm.auditdeliverytimespan,HasServiceProduct,
						   (case when sum(p.islarge)>0 then 'Large' else 'Normal' end) as islarge,(case when sum((case when (i.accountqty - si.quantity)<0 then 1 else 0 end))>0 
						   then  'Wait for replacements' 
						   else 'Yes' end)   as isEnough ,c.customername
						   from so_master sm 
						   inner join so_item si on si.sosysno = sm.sysno
						   left join finance_soincome fs on fs.ordersysno = sm.sysno and fs.ordertype = " + (int)AppEnum.SOIncomeOrderType.SO + " and fs.status>" + (int)AppEnum.SOIncomeStatus.Abandon
                        + @" inner join product p on p.sysno = si.productsysno
						   left join inventory i on i.productsysno = si.productsysno
						   left join inventory_stock its on its.productsysno = si.productsysno and its.stocksysno = sm.stocksysno
						   left join shiptype st on st.sysno = sm.shiptypesysno
						   left join paytype pt on pt.sysno = sm.paytypesysno
						   inner join customer c on c.sysno = sm.customersysno 
                           left join area on sm.receiveareasysno = area.sysno
						   where sm.status = " + (int)AppEnum.SOStatus.WaitingOutStock;

            if (paramHash.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string key in paramHash.Keys)
                {
                    object item = paramHash[key];
                    sb.Append(" and ");
                    if (key == "EndDate")
                    {
                        sb.Append("sm.DeliveryDate").Append("<=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "ShipTypeSysNo")
                    {
                        sb.Append("sm.shiptypesysno").Append((string)paramHash["CompareOpt"]).Append(item.ToString());
                    }
                    else if (key == "AuditDeliveryDate")
                    {
                        sb.Append("sm.auditdeliverydate").Append("=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "AuditDeliveryTimeSpan")
                    {
                        sb.Append("sm.auditdeliverytimespan").Append("=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "StockSysNo")
                    {
                        sb.Append("sm.stocksysno").Append("=").Append(item.ToString());
                    }
                    else if (key == "CompareOpt")
                    {
                        sb.Append(" 1=1 ");
                    }
                    else if(key == "DistrictSysNo")
                    {
                        sb.Append("area.sysno").Append("=").Append(item.ToString());
                    }
                    else if(key == "CitySysNo")
                    {
                        sb.Append("area.CitySysNo").Append("=").Append(item.ToString());
                    }
                    else if(key == "ProvinceSysNo")
                    {
                        sb.Append("area.ProvinceSysNo").Append("=").Append(item.ToString());
                    }
                    else if (key == "LocalCode")
                    {
                        if (item.ToString().Equals("-1"))
                        {
                            sb.Append("area.localcode <> 5").Append(" and area.localcode <> 6");
                        }
                        else if (item.ToString().Equals("0"))
                        {
                            sb.Append(" ( area.localcode = 5 or area.localcode = 6 )");  //5,6 郊区件
                        }
                        else
                        {
                            sb.Append("area.localcode").Append("=").Append(item.ToString());
                        }
                    }
                    else if (key == "HasServiceProduct")
                    {
                        sb.Append("sm.HasServiceProduct").Append("=").Append(item.ToString());
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
            sql += " group by sm.sysno,sm.soid,st.shiptypename,pt.paytypename,sm.orderdate,sm.audittime,sm.deliverydate,sm.auditdeliverydate,sm.auditdeliverytimespan,sm.HasServiceProduct,sm.status,fs.status,sm.isvat,c.customername";
            sql += " order by sm.audittime";
            return SqlHelper.ExecuteDataSet(sql);
        }
		
		public DataSet GetSOPackageCoverList(Hashtable paramHash)
		{
			string sql = @"select sm.sysno,sm.soid,sm.receiveaddress,customer.customername,sm.CashPay+sm.PayPrice+sm.ShipPrice-sm.FreeShipFeePay+sm.PremiumAmt-sm.DiscountAmt as TotalAmount,pt.paytypename,sm.outtime,fs.status as incomestatus,sm.isvat,
						   (case when a.sysno is null then 'Normal' else 'Large' end) as islarge,su.username as freightman
						   from so_master sm 
                           inner join area on sm.receiveareasysno=area.sysno 
						   inner join customer on customer.sysno = sm.customersysno
						   left join finance_soincome fs on fs.ordersysno = sm.sysno and fs.status>=0 and fs.ordertype = "+(int)AppEnum.SOIncomeOrderType.SO
						+@" 
							left join paytype pt on pt.sysno = sm.paytypesysno
							left join 
							(select distinct sm.sysno
							 from so_master sm
							 inner join so_item si on si.sosysno = sm.sysno
							 inner join product p on p.sysno = si.productsysno
							 where sm.status="+(int)AppEnum.SOStatus.OutStock+" and p.islarge ="+(int)AppEnum.YNStatus.Yes+" ) a on a.sysno = sm.sysno "
						+" left join Sys_user su on su.sysno = sm.freightusersysno ";						  
			if(paramHash.Count>0)
			{
				StringBuilder sb = new StringBuilder();
				foreach(string key in paramHash.Keys)
				{
					object item = paramHash[key];
					sb.Append(" and ");
					if(key=="StartDate")
					{
						sb.Append("sm.outtime").Append(">=").Append(Util.ToSqlString(item.ToString()));
					}
					else if(key=="EndDate")
					{
						sb.Append("sm.outtime").Append("<=").Append(Util.ToSqlString(item.ToString()));
					}
					else if(key=="ProductID")
					{
						string idStr = @" inner join 
										  (select distinct sm.sysno 
										   from so_master sm 
										   inner join so_item si on si.sosysno = sm.sysno
										   inner join product p on p.sysno = si.productsysno
										   where sm.status="+(int)AppEnum.SOStatus.OutStock+" and p.productid like "+Util.ToSqlLikeString(item.ToString())
							+" ) b on b.sysno = sm.sysno";
						sql += idStr;
						sb.Append("1=1");
					}
					else if(key=="IsVAT")
					{
						sb.Append("sm.isvat").Append(" = ").Append(item.ToString());
					}
					else if(key=="IsLarge")
					{
						if((int)item==(int)AppEnum.YNStatus.Yes)
							sb.Append("a.sysno is not null");
						else
							sb.Append("a.sysno is null");
					}
					else if(key=="IsPrintPackageCover")
					{
						sb.Append("sm.IsPrintPackageCover").Append(" = ").Append(item.ToString());
					}
					else if(key=="UnAllocated")
					{
						sb.Append("sm.freightusersysno is null");
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
					else if(key=="ReceiveAddress")
					{
						sb.Append("sm.receiveaddress").Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
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
				sql += " where sm.status="+(int)AppEnum.SOStatus.OutStock;
				sql += sb.ToString();
			}
			else
				sql += " where sm.status="+(int)AppEnum.SOStatus.OutStock;
			sql += " order by sm.outtime desc";
			return SqlHelper.ExecuteDataSet(sql);
		}

		public int GetSOSysNofromID(string soID)
		{
			string sql = "select sysno from so_master where soid = "+Util.ToSqlString(soID);
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if(Util.HasMoreRow(ds))
				return (int)ds.Tables[0].Rows[0][0];
			else
				return AppConst.IntNull;
		}
		#endregion

		#region Delete Zone
		public void DeleteSRItem(int paramSysNo)
		{
			new SaleRuleDac().DeleteItem(paramSysNo);
		}		
		#endregion

		#region SO Biz
		public void CalcSO(SOInfo soInfo)
		{
			int pointAmt = 0;
			decimal soAmt = 0m;
			if(soInfo.ItemHash.Count>0)
			{
                if (soInfo.CouponAmt == AppConst.DecimalNull)
                    soInfo.CouponAmt = 0;

				this.CalcItemPrice(soInfo);
				foreach(SOItemInfo soItem in soInfo.ItemHash.Values)
				{
					pointAmt += soItem.Point*soItem.Quantity;
					soAmt += soItem.Price*soItem.Quantity;
				}
				soInfo.PointAmt = pointAmt;
				soInfo.SOAmt = Util.ToMoney(soAmt);
				this.CalcShipPrice(soInfo);
				this.CalcPremiumAmt(soInfo);
				this.CalcSaleRule(soInfo);
				//如果本单支付积分不足最低积分，将本单支付积分自动更新为最低积分
				//如果本单支付积分超出最大值，将本单支付积分自动更新为最大值
				//此处不对客户帐户积分做判断，留在后续的update中进行
				int minPointPay = this.GetMinPoint(soInfo);
				int maxPointPay = this.GetMaxPoint(soInfo);
				if(minPointPay>soInfo.PointPay)
					soInfo.PointPay = minPointPay;
				else if(maxPointPay<soInfo.PointPay)
					soInfo.PointPay = maxPointPay;

				soInfo.CashPay = Util.ToMoney(soInfo.GetCashPay());				
				this.CalcPayPrice(soInfo);
				this.CalcVATEMSFee(soInfo);
			}
			else
			{
				soInfo.PremiumAmt = 0;
				soInfo.PayPrice = 0;
				soInfo.CashPay = 0;
				soInfo.ShipPrice = 0;
                soInfo.CouponAmt = 0;
			    soInfo.FreeShipFeePay = 0;
				soInfo.DiscountAmt = 0;
				soInfo.PointAmt = 0;
				soInfo.SOAmt= 0;
				soInfo.PointPay = 0;
				soInfo.SaleRuleHash.Clear();
				soInfo.VATEMSFee = 0;
			}
		}

		private void CalcItemPrice(SOInfo soInfo)
		{
			//如果是手工批发订单，则不采取商品设定的批发价格策略
			if(soInfo.IsWholeSale==(int)AppEnum.YNStatus.Yes)
			{
				//批发不给积分
				foreach(SOItemInfo item in soInfo.ItemHash.Values)
				{
					item.Point = 0;
				}
				return;
			}
			Hashtable priceHash = ProductManager.GetInstance().GetSOItemPriceList(soInfo);
			foreach(SOItemInfo item in soInfo.ItemHash.Values)
			{
				if(item.ProductType==(int)AppEnum.SOItemType.ForSale)
				{
					foreach(ProductPriceInfo priceInfo in priceHash.Values)
					{
						if(priceInfo.ProductSysNo==item.ProductSysNo)
						{
							//商品有优惠价格策略
							if(priceInfo.IsWholeSale==(int)AppEnum.YNStatus.Yes)
							{
								if(item.Quantity>=priceInfo.Q1&&item.Quantity<priceInfo.Q2)
									item.Price = priceInfo.P1;
								else if(item.Quantity>=priceInfo.Q2&&item.Quantity<priceInfo.Q3)
									item.Price = priceInfo.P2;
								else if(item.Quantity>=priceInfo.Q3)
									item.Price = priceInfo.P3;

                                item.Point = priceInfo.Point;
							}
                            //审核订单保存时,价格不作修改,保持下订单时的价格和积分
                            //else
                            //{
                            //    item.Price = priceInfo.CurrentPrice;
                            //    item.Point = priceInfo.Point;
                            //}
							break;
						}
					}
				}
				else//是赠品则价格为0、积分为0
				{
					item.Price = 0;
					item.Point = 0;
				}				
			}
		}

        private void CalcShipPrice(SOInfo soInfo)
        {
            if (soInfo.ShipTypeSysNo != AppConst.IntNull)
            {
                //if (soInfo.AdwaysInfo != null && !soInfo.AdwaysInfo.AdwaysID.Equals(AppConst.StringNull) && soInfo.AdwaysInfo.AdwaysID.Length > 0)
                //{
                //    soInfo.ShipPrice = 0m;
                //}
                //else
                //{
                //    soInfo.ShipPrice = Util.ToMoney(ASPManager.GetInstance().GetShipPrice(soInfo.GetTotalWeight(), soInfo.SOAmt, soInfo.ShipTypeSysNo, soInfo.ReceiveAreaSysNo));
                //}
                soInfo.ShipPrice = Util.ToMoney(ASPManager.GetInstance().GetShipPrice(soInfo.GetTotalWeight(), soInfo.SOAmt, soInfo.ShipTypeSysNo, soInfo.ReceiveAreaSysNo));
                CustomerInfo oCustomer = CustomerManager.GetInstance().Load(soInfo.CustomerSysNo);
                if (oCustomer.CustomerType != (int)AppEnum.CustomerType.Personal)
                {
                    return;
                }

                //ORS商城快递配送范围内的（包括杭州，南京，苏州，扬州，上海市区），满100元免运费，最高免50元
                //其他区域（包括上海郊区），满200元免运费，最高免50元。EMS、顺丰快递不免运费。
                AreaInfo areaInfo = ASPManager.GetInstance().LoadArea(soInfo.ReceiveAreaSysNo);
                if ((soInfo.ShipTypeSysNo == 1 || soInfo.ShipTypeSysNo == 8 || soInfo.ShipTypeSysNo == 9 || soInfo.ShipTypeSysNo == 10 || soInfo.ShipTypeSysNo == 11 || soInfo.ShipTypeSysNo == 13 || soInfo.ShipTypeSysNo == 17) && areaInfo.LocalCode < 5 && soInfo.SOAmt >= 100)  //ORS商城快递(非上海郊区)、订单金额满100
                {
                    if (soInfo.ShipPrice <= 50)
                    {
                        soInfo.ShipPrice = 0m;
                    }
                    else
                    {
                        soInfo.ShipPrice = soInfo.ShipPrice - 50;
                    }
                }
                else if (soInfo.SOAmt >= 200 && soInfo.ShipTypeSysNo != 2 && soInfo.ShipTypeSysNo != 12)
                {
                    if (soInfo.ShipPrice <= 50)
                    {
                        soInfo.ShipPrice = 0m;
                    }
                    else
                    {
                        soInfo.ShipPrice = soInfo.ShipPrice - 50;
                    }
                }

                //if (soInfo.SOAmt >= 50 && soInfo.ShipTypeSysNo != 2) //订单金额大于50￥，非EMS
                //{
                //    if (soInfo.ShipPrice <= 50)
                //    {
                //        soInfo.ShipPrice = 0m;
                //    }
                //    else
                //    {
                //        soInfo.ShipPrice = soInfo.ShipPrice - 50;
                //    }
                //}
            }
            else
                soInfo.ShipPrice = 0m;
        }

        public decimal CalcShipPriceOrigin(SOInfo soInfo)
        {
            if (soInfo.ShipTypeSysNo != AppConst.IntNull)
            {   
                return Util.ToMoney(ASPManager.GetInstance().GetShipPrice(soInfo.GetTotalWeight(), soInfo.SOAmt, soInfo.ShipTypeSysNo, soInfo.ReceiveAreaSysNo));
            }
            else
                return 0m;
        }

		private void CalcPremiumAmt(SOInfo soInfo)
		{
			if(soInfo.IsPremium==(int)AppEnum.YNStatus.Yes)
				soInfo.PremiumAmt = Util.ToMoney(ASPManager.GetInstance().GetPremuimAmt(soInfo.SOAmt,soInfo.ShipTypeSysNo));
			else
				soInfo.PremiumAmt = 0;
		}

		public void CalcPayPrice(SOInfo soInfo)
		{
			if(soInfo.PayTypeSysNo!=AppConst.IntNull)
			{
				PayTypeInfo ptInfo = ASPManager.GetInstance().LoadPayType(soInfo.PayTypeSysNo);
				//soInfo.PayPrice = Util.ToMoney(ptInfo.PayRate*(soInfo.CashPay+soInfo.ShipPrice+soInfo.DiscountAmt+soInfo.PremiumAmt));
                soInfo.PayPrice = Util.ToMoney(ptInfo.PayRate * (soInfo.CashPay + soInfo.ShipPrice - soInfo.FreeShipFeePay - soInfo.DiscountAmt + soInfo.PremiumAmt));
			}
			else
				soInfo.PayPrice = 0m;
		}

		public void CalcVATEMSFee(SOInfo soInfo)
		{
			if(soInfo.IsVAT == (int)AppEnum.YNStatus.Yes)
			{
				soInfo.VATEMSFee = Util.ToMoney(AppConfig.VATEMSFee);
				soInfo.VatInfo.VATEMSFee = Util.ToMoney(AppConfig.VATEMSFee);
			}
			else
			{
				soInfo.VATEMSFee = 0m;
				soInfo.VatInfo.VATEMSFee = 0;
			}
		}

		private int getCurrentSOStatus(int soSysNo)
		{
			int status = AppConst.IntNull;
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				string sql = @"select status from so_master where sysno = " + soSysNo;
				DataSet ds = SqlHelper.ExecuteDataSet(sql);
				if(Util.HasMoreRow(ds))
					status = Util.TrimIntNull(ds.Tables[0].Rows[0]["Status"]);
				scope.Complete();
            }
			return status;
		}

		public void DeleteSOItem(SOInfo soInfo,int productSysNo)
		{
			//验证赠品数量
			if(((SOItemInfo)soInfo.ItemHash[productSysNo]).ProductType==(int)AppEnum.SOItemType.ForSale&&((SOItemInfo)soInfo.ItemHash[productSysNo]).GiftSysNo!=AppConst.IntNull)
			{
				int giftQty = 0;
				int masterQty = 0;
				foreach(SOItemInfo item in soInfo.ItemHash.Values)
				{
					if(item.ProductType==(int)AppEnum.SOItemType.ForSale&&item.ProductSysNo!=productSysNo&&item.GiftSysNo==((SOItemInfo)soInfo.ItemHash[productSysNo]).GiftSysNo)
						masterQty += item.Quantity;
					else if(item.ProductSysNo==((SOItemInfo)soInfo.ItemHash[productSysNo]).GiftSysNo)
						giftQty = item.Quantity;
				}
				if(masterQty<giftQty)
					throw new BizException("There is too many gifts,please remove some gifts first");
			}
			if(soInfo.SysNo!=AppConst.IntNull)//判断是否是修改已生成的订单
			{
				TransactionOptions options = new TransactionOptions();
                options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                options.Timeout = TransactionManager.DefaultTimeout;

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
                {
				    if(getCurrentSOStatus(soInfo.SysNo)!=(int)AppEnum.SOStatus.Origin)
					    throw new BizException("The SOStatus is not origin now,can't be edited");
				    int rowsAffected = new SODac().DeleteSOItem((SOItemInfo)soInfo.ItemHash[productSysNo]);
				    if(rowsAffected!=1)
					    throw new BizException("Delete item failed");
				    SOItemInfo itemInfo = (SOItemInfo)soInfo.ItemHash[productSysNo];
				    InventoryManager.GetInstance().SetOrderQty(soInfo.StockSysNo,itemInfo.ProductSysNo,-1*itemInfo.Quantity);
				    soInfo.ItemHash.Remove(productSysNo);

                    //hawkins 2010-4-20
                    if (soInfo.CouponCode != "")
                    {
                        soInfo.CouponAmt = 0;
                        Icson.Objects.Promotion.CouponInfo oCoupon = Icson.BLL.Promotion.CouponManager.GetInstance().LoadCouponByPwd(soInfo.CouponCode);
                        if (oCoupon != null)
                        {
                            string errstr = Icson.BLL.Promotion.CouponManager.GetInstance().CheckCouponSOByUpdate(oCoupon, soInfo);
                            if (errstr == "")
                                soInfo.CouponAmt = oCoupon.CouponAmt;
                        }
                    }
                    //============

				    this.CalcSO(soInfo);
				    this.UpdateSO(soInfo);
				    scope.Complete();
                }
			}
			else
				soInfo.ItemHash.Remove(productSysNo);
		}

		public void AddSOItem(SOItemInfo itemInfo,SOInfo soInfo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				//验证赠品数量
				if(itemInfo.ProductType==(int)AppEnum.SOItemType.Gift)
				{
					int masterQty = 0;
					int giftQty = 0;
					foreach(SOItemInfo item in soInfo.ItemHash.Values)
					{
						if(item.ProductType==(int)AppEnum.SOItemType.ForSale&&item.GiftSysNo==itemInfo.ProductSysNo)
							masterQty += item.Quantity;
						else if(item.ProductType==(int)AppEnum.SOItemType.Gift&&item.ProductSysNo==itemInfo.ProductSysNo)
							giftQty = item.Quantity;
					}
					if(masterQty<(giftQty+itemInfo.Quantity))
						throw new BizException("There is too many gifts,can't add any more'");
				}
				if(soInfo.SysNo!=AppConst.IntNull)
				{
					if( getCurrentSOStatus(soInfo.SysNo)!=(int)AppEnum.SOStatus.Origin )
						throw new BizException("The SOStatus is not origin now,can't be edited");
					if(!soInfo.ItemHash.ContainsKey(itemInfo.ProductSysNo))//订单中不存在该商品，插入新Item
					{
						if(this.IfExistSOItem(itemInfo))
							throw new BizException("The same product already exists in this Sale Order.");
						this.InsertSOItem(itemInfo,soInfo);
					}
				}
				//更新Info
				if(soInfo.ItemHash.ContainsKey(itemInfo.ProductSysNo))
				{
					SOItemInfo tempItem = (SOItemInfo)soInfo.ItemHash[itemInfo.ProductSysNo];
					if(tempItem.ProductType!=itemInfo.ProductType)
						throw new BizException("This product already exists in the order with a different type");
					itemInfo.SysNo = tempItem.SysNo;
					itemInfo.Quantity += tempItem.Quantity;
					soInfo.ItemHash.Remove(itemInfo.ProductSysNo);
				}
				soInfo.ItemHash.Add(itemInfo.ProductSysNo,itemInfo);
				this.CalcSO(soInfo);
				if(soInfo.SysNo!=AppConst.IntNull)
					this.UpdateSO(soInfo);
				scope.Complete();
            }
		}

		public void AddSOGiftItem(SOItemInfo giftInfo,int parentSysNo,SOInfo soInfo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {		
				SOItemInfo parentItem = (SOItemInfo)soInfo.ItemHash[parentSysNo];
				//删除原先赠品
				if(parentItem.GiftSysNo!=AppConst.IntNull&&soInfo.ItemHash.ContainsKey(parentItem.GiftSysNo))
				{
					SOItemInfo originGift = (SOItemInfo)soInfo.ItemHash[parentItem.GiftSysNo];
					if(originGift.Quantity<=parentItem.Quantity)
						this.DeleteSOItem(soInfo,originGift.ProductSysNo);
					else
					{
						this.UpdateSOItemPriceAndQty(soInfo,originGift.ProductSysNo,(originGift.Quantity-parentItem.Quantity),originGift.Price);
					}
				}
				//更新主商品的GiftSysNo
				parentItem.GiftSysNo = giftInfo.ProductSysNo;					
				giftInfo.ProductType = (int)AppEnum.SOItemType.Gift;
				giftInfo.Price = 0;//赠品价格写死为0
				giftInfo.DiscountAmt = 0m;
				//增加赠品
				this.AddSOItem(giftInfo,soInfo);
				if(soInfo.SysNo!=AppConst.IntNull)
				{
					if(getCurrentSOStatus(soInfo.SysNo)!=(int)AppEnum.SOStatus.Origin)
						throw new BizException("The SOStatus is not origin now,can't be edited");
					Hashtable paramHash = new Hashtable();
					paramHash.Add("SysNo",parentItem.SysNo);
					paramHash.Add("GiftSysNo",parentItem.GiftSysNo);
					this.UpdateSOItem(paramHash);					
				}
				scope.Complete();
            }
		}

		public void UpdateSOItemPriceAndQty(SOInfo soInfo,int productSysNo,int qty,decimal price)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				SOItemInfo item = new SOItemInfo();
				if(soInfo.ItemHash.ContainsKey(productSysNo))
					item =  (SOItemInfo)soInfo.ItemHash[productSysNo];
				else
					throw new BizException("No such product in this order");
				//检查赠品数量
				int masterQty = 0;
				int giftQty = 0;
				if(item.ProductType==(int)AppEnum.SOItemType.ForSale&&item.GiftSysNo!=AppConst.IntNull&&item.Quantity>qty)//在减少主商品数量的时候需要检验
				{
					foreach(SOItemInfo itemInfo in soInfo.ItemHash.Values)
					{
						if(itemInfo.ProductType==(int)AppEnum.SOItemType.ForSale&&itemInfo.ProductSysNo!=item.ProductSysNo&&itemInfo.GiftSysNo==item.GiftSysNo)
							masterQty += itemInfo.Quantity;
						else if(itemInfo.ProductType==(int)AppEnum.SOItemType.Gift&&itemInfo.ProductSysNo==item.GiftSysNo)
							giftQty = itemInfo.Quantity;
					}
					if((masterQty+qty)<giftQty)
						throw new BizException("There is too many gifts,please remove some gifts first");
				}
				else if(item.ProductType==(int)AppEnum.SOItemType.Gift&&item.Quantity<qty)//在增加赠品数量的时候需要检验
				{
					foreach(SOItemInfo itemInfo in soInfo.ItemHash.Values)
					{
						if(itemInfo.ProductType==(int)AppEnum.SOItemType.ForSale&&itemInfo.GiftSysNo==item.ProductSysNo)
							masterQty += itemInfo.Quantity;
					}
					if(masterQty<(giftQty+qty))
						throw new BizException("Too many gifts added");
				}
				((SOItemInfo)soInfo.ItemHash[productSysNo]).Quantity = qty;
                ((SOItemInfo)soInfo.ItemHash[productSysNo]).ExpectQty = qty;
				((SOItemInfo)soInfo.ItemHash[productSysNo]).Price = price;
                //hawkins 2010-4-20
                if (soInfo.CouponCode != "")
                {
                    soInfo.CouponAmt = 0;
                    Icson.Objects.Promotion.CouponInfo oCoupon = Icson.BLL.Promotion.CouponManager.GetInstance().LoadCouponByPwd(soInfo.CouponCode);
                    if (oCoupon != null)
                    {
                        string errstr = Icson.BLL.Promotion.CouponManager.GetInstance().CheckCouponSOByUpdate(oCoupon, soInfo);
                        if (errstr == "")
                            soInfo.CouponAmt = oCoupon.CouponAmt;
                    }
                }

				SaleManager.GetInstance().CalcSO(soInfo);
				if(soInfo.SysNo!=AppConst.IntNull)
				{
					SaleManager.GetInstance().UpdateSO(soInfo);
				}
				scope.Complete();
            }
		}

		public void CreateSO(SOInfo soInfo)
		{
            try
            {
                TransactionOptions options = new TransactionOptions();
                options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                options.Timeout = TransactionManager.DefaultTimeout;

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    bool HasService = false;
                    int HasExpectQty = (int)AppEnum.YNStatus.No;
                    //修改逻辑，当用户提交订单后需要支付，而不是审核。
                    //soInfo.Status = (int)AppEnum.SOStatus.Origin;
                    soInfo.Status = (int)AppEnum.SOStatus.WaitingPay;
                    soInfo.IsPrintPackageCover = (int)AppEnum.YNStatus.No;
                    soInfo.OrderDate = DateTime.Now;
                    
                    //此处写死为上海分仓，日后有分仓需要，再开放选择
                    StockInfo shStock = StockManager.GetInstance().Load("0001");
                    soInfo.StockSysNo = shStock.SysNo;

                    //hawkins 2010-4-20
                    if (soInfo.CouponType != AppConst.IntNull)
                    {
                        Icson.Objects.Promotion.CouponInfo oCoupon = CouponManager.GetInstance().LoadCouponByPwd(soInfo.CouponCode);
                        if (oCoupon != null)
                            CouponManager.GetInstance().CheckCouponSO(oCoupon, soInfo);
                        else
                            throw new BizException("优惠券信息丢失");
                    }
                    //====================

                    //加入订单主项
                    this.InsertSOMaster(soInfo);

                    //加入订单商品明细
                    int itemCount = 0;
                    foreach (SOItemInfo item in soInfo.ItemHash.Values)
                    {
                        if (item.Quantity > 0)  //排除数量为零的情况
                        {
                            item.SOSysNo = soInfo.SysNo;
                            this.InsertSOItem(item, soInfo);
                            if (item.BaseProductType == (int) AppEnum.ProductType.Service)
                            {
                                HasService = true;
                            }
                            if (item.ExpectQty > item.Quantity)
                            {
                                HasExpectQty = (int)AppEnum.YNStatus.Yes;
                            }
                            itemCount++;
                        }
                    }
                    if (itemCount == 0)  //无订购商品
                    {
                        throw new BizException("您选择订购的商品数量为零！");
                    }

                    //加入订单销售规则
                    if (soInfo.SaleRuleHash.Count > 0)
                    {
                        foreach (SOSaleRuleInfo srInfo in soInfo.SaleRuleHash.Values)
                        {
                            srInfo.SOSysNo = soInfo.SysNo;
                            this.InsertSOSaleRule(srInfo);
                        }
                    }
                    //加入增票信息
                    //if(soInfo.IsVAT==(int)AppEnum.YNStatus.Yes)
                    //{
                    //	soInfo.VatInfo.SOSysNo = soInfo.SysNo;
                    //	this.InsertSOVAT(soInfo.VatInfo);
                    //}
                    //更新客户信息
                    Hashtable paramHash = new Hashtable();
                    paramHash.Add("SysNo", soInfo.CustomerSysNo);
                    paramHash.Add("ReceiveAddress", soInfo.ReceiveAddress);
                    paramHash.Add("ReceiveContact", soInfo.ReceiveContact);
                    paramHash.Add("ReceiveName", soInfo.ReceiveName);
                    paramHash.Add("ReceivePhone", soInfo.ReceivePhone);
                    paramHash.Add("ReceiveZip", soInfo.ReceiveZip);
                    paramHash.Add("ReceiveAreaSysNo", soInfo.ReceiveAreaSysNo);
                    paramHash.Add("ReceiveCellPhone", soInfo.ReceiveCellPhone);
                    CustomerManager.GetInstance().Update(paramHash);

                    PointManager.GetInstance().SetScore(soInfo.CustomerSysNo, soInfo.PointPay * -1, (int)AppEnum.PointLogType.CreateOrder, soInfo.SysNo.ToString());
                    //if(soInfo.FreeShipFeePay > 0)
                    //{
                    //    FreeShipFeeManager.GetInstance().SetFreeShipFee(soInfo.CustomerSysNo,soInfo.FreeShipFeePay * -1,(int)AppEnum.FreeShipFeeLogType.CreateOrder,soInfo.SysNo.ToString());
                    //}
                    //更新分配审单人
                    //soInfo.AllocatedManSysNo = this.GetAllocatedMan(soInfo.SysNo);
                    soInfo.AllocatedManSysNo = UserRatioManager.GetInstance().GetAllocatedMan(soInfo.SysNo);
                    Hashtable paramSOHash = new Hashtable();
                    paramSOHash.Add("SysNo", soInfo.SysNo);
                    paramSOHash.Add("AllocatedManSysNo", soInfo.AllocatedManSysNo);

                    //更新是否有期望订购数量
                    paramSOHash.Add("HasExpectQty", HasExpectQty);
                    this.UpdateSOMaster(paramSOHash);

                    //易价网合作
                    //if (soInfo.AdwaysInfo != null && soInfo.AdwaysInfo.AdwaysID.Length > 0)
                    //{
                    //    SOAdwaysInfo adwaysInfo = new SOAdwaysInfo();
                    //    adwaysInfo.AdwaysID = soInfo.AdwaysInfo.AdwaysID;
                    //    adwaysInfo.AdwaysEmail = soInfo.AdwaysInfo.AdwaysEmail;
                    //    adwaysInfo.ShipPrice = soInfo.AdwaysInfo.ShipPrice;
                    //    adwaysInfo.CustomerSysNo = soInfo.CustomerSysNo;
                    //    adwaysInfo.SOSysNo = soInfo.SysNo;
                    //    this.InsertSOAdways(adwaysInfo);
                    //}

                    //hawkins 2010-4-20
                    if (soInfo.CouponType != AppConst.IntNull)
                    {
                        CouponManager.GetInstance().UseCoupon(soInfo.CouponCode);
                    }
                    //===============

                    if (HasService)
                    {
                        soInfo.ServiceInfo.SOSysNo = soInfo.SysNo;
                        this.InsertSOService(soInfo.ServiceInfo);
                    }

                    scope.Complete();
                }
            }
            catch(Exception ex)
		    {
			    soInfo.SysNo = AppConst.IntNull;
			    throw ex;
            }
		}

		public void AuditSO(SOInfo soInfo,bool isForce)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                //1 查看订单当前状态
                int currentStatus = this.getCurrentSOStatus(soInfo.SysNo);
                if (currentStatus != (int)AppEnum.SOStatus.Origin)
                    throw new BizException("audit so: the current status is not Origin, operation of Audit SO failed");

				if(soInfo.IsWholeSale == (int)AppEnum.YNStatus.Yes) //是批发则转为等待经理审核状态
					soInfo.Status = (int)AppEnum.SOStatus.WaitingManagerAudit;
				else
				{
					PayTypeInfo ptInfo = ASPManager.GetInstance().LoadPayType(soInfo.PayTypeSysNo);
					if(ptInfo.SysNo == 6) //帐期也转为等待经理审核状态
						soInfo.Status = (int)AppEnum.SOStatus.WaitingManagerAudit;
					else
						calcWaitingStatus(soInfo,isForce);
				}

                UpdateSOHasServiceProduct(soInfo.SysNo);  //审核时更新是否包括服务类的商品

				UpdateSOStatus(soInfo.SysNo, soInfo.Status, soInfo.AuditUserSysNo);

				scope.Complete();
            }
		}

		public void PaySO(int soSysNo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				int currentStatus = getCurrentSOStatus(soSysNo);
				if ( currentStatus == (int)AppEnum.SOStatus.Origin || currentStatus == (int)AppEnum.SOStatus.OutStock)
				{
					//如果是Orgin，则不需要处理。
					//如果是OutStock, 会产生soIncome, 就不需要处理了。
				}
				else if ( currentStatus == (int)AppEnum.SOStatus.WaitingPay)
				{
					Hashtable ht = new Hashtable(2);
					ht.Add("SysNo", soSysNo);
					ht.Add("Status", (int)AppEnum.SOStatus.WaitingOutStock);
					new SODac().UpdateSOMaster(ht);
				}
                else if (currentStatus == (int)AppEnum.SOStatus.WaitingManagerAudit)
                {
                    //待经理审核，状态不变
                }
                else
                {
                    throw new BizException("payso: the current status is not orgin or waitingmanageraudit or waiting pay, pay so failed");
                }

				scope.Complete();
            }

		}
        public void UnPaySO(int soSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                int currentStatus = getCurrentSOStatus(soSysNo);
                if (currentStatus == (int)AppEnum.SOStatus.Origin || currentStatus == (int)AppEnum.SOStatus.WaitingManagerAudit || currentStatus == (int)AppEnum.SOStatus.Return)
                {
                    //如果是Orgin/Waiting Manager Audit/Return，则不需要处理。
                }
                else if (currentStatus == (int)AppEnum.SOStatus.WaitingOutStock)
                {
                    Hashtable ht = new Hashtable(2);
                    ht.Add("SysNo", soSysNo);
                    ht.Add("Status", (int)AppEnum.SOStatus.WaitingPay);
                    new SODac().UpdateSOMaster(ht);
                }
                else
                {
                    //employee cancel, manager cancel, customer cancel, waiting pay, outstock
                    throw new BizException("unpayso: the so current status is not orgin or waiting manager audit, un pay so failed");
                }

                scope.Complete();
            }

        }

		public void CancelAuditSO(SOInfo soInfo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                ////更新订单状态
                //1 查看订单当前状态
                //2 设置更新值，更新status
                int currentStatus = this.getCurrentSOStatus(soInfo.SysNo);
                if (currentStatus != (int)AppEnum.SOStatus.WaitingManagerAudit
                    && currentStatus != (int)AppEnum.SOStatus.WaitingOutStock
                    && currentStatus != (int)AppEnum.SOStatus.WaitingPay
                    )
                    throw new BizException("cancel audit so: the current status is not WaitingManagerAudit/WaitingOutStock/WaitingPay, operation of Cancel Audit SO failed");

				soInfo.Status = (int)AppEnum.SOStatus.Origin;
				UpdateSOStatus(soInfo.SysNo, soInfo.Status, soInfo.UpdateUserSysNo);

				scope.Complete();
            }
		}

		public void ManagerAuditSO(SOInfo soInfo,bool isForce)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                //1 查看订单当前状态
                int currentStatus = this.getCurrentSOStatus(soInfo.SysNo);
                if (currentStatus != (int)AppEnum.SOStatus.WaitingManagerAudit)
                    throw new BizException("manager audit so: the current status is not WaitingManagerAudit, operation of Manager Audit SO failed");

				calcWaitingStatus(soInfo,isForce);
				UpdateSOStatus(soInfo.SysNo, soInfo.Status, soInfo.ManagerAuditUserSysNo);
				scope.Complete();
            }
		}

		public void OutStock(SOInfo soInfo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                ////更新订单状态
                //1 查看订单当前状态
                //2 设置更新值，更新status
                int currentStatus = this.getCurrentSOStatus(soInfo.SysNo);

                if (currentStatus != (int)AppEnum.SOStatus.WaitingOutStock)
                    throw new BizException("outstock so: the current status is not WaitingOutStock, operation of OutStock SO failed");

				//this.UpdateSO(soInfo);
                soInfo.Status = (int)AppEnum.SOStatus.OutStock;
				//更新订单状态
				UpdateSOStatus(soInfo.SysNo, soInfo.Status, soInfo.OutUserSysNo);
				//更新商品库存
				foreach(SOItemInfo item in soInfo.ItemHash.Values)
				{
					InventoryManager.GetInstance().SetSOOutStockQty(soInfo.StockSysNo,item.ProductSysNo,item.Quantity);
				}
				//更新订单销售明细成本
				this.SetSOItemCost(soInfo.SysNo);

				//如果无有效的收款单-->生成soincome(normal, origin)
				SOIncomeInfo soIncome = SOIncomeManager.GetInstance().LoadValid((int)AppEnum.SOIncomeOrderType.SO,soInfo.SysNo);
				if(soIncome==null)//无有效收款单，生成收款单
				{	
					soIncome = new SOIncomeInfo();
					soIncome.OrderType = (int)AppEnum.SOIncomeOrderType.SO;
					soIncome.OrderSysNo = soInfo.SysNo;
					soIncome.OrderAmt = soIncome.IncomeAmt = Util.TruncMoney(soInfo.GetTotalAmt());
					soIncome.IncomeStyle = (int)AppEnum.SOIncomeStyle.Normal;
					soIncome.IncomeUserSysNo = soInfo.OutUserSysNo;
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

				//如果有积分赠送则添加pointDelay
				if(soInfo.PointAmt>0)
				{
					SalePointDelayInfo spInfo = new SalePointDelayInfo();
					spInfo.SOSysNo = soInfo.SysNo;
					spInfo.CreateTime = DateTime.Now;
					spInfo.Status = (int)AppEnum.TriStatus.Origin;
					PointManager.GetInstance().InsertPointDelay(spInfo);
				}

                //检测订单检货员，如果有记录则更新，没有则添加

                WhProductShelvingInspectionInfo oWhpsi = new WhProductShelvingInspectionInfo();
                Hashtable ht = new Hashtable();
                ht.Add("BillSysNo", soInfo.SysNo);
                ht.Add("WorkType", (int)AppEnum.WhWorkType.ProductInspection);
                ht.Add("BillType", (int)AppEnum.WhWorkBillType.SO);
                ht.Add("top", "select top 1 ");

                int whpsiSysNo = WhProductShelvingInspectionManager.GetInstance().GetSysNo(ht);

                if (whpsiSysNo == 0)
                {
                    oWhpsi.BillSysNo = soInfo.SysNo;
                    oWhpsi.WorkType = (int)AppEnum.WhWorkType.ProductInspection;
                    oWhpsi.BillType = (int)AppEnum.WhWorkBillType.SO;
                    oWhpsi.AllocatedUserSysNo = UserRatioManager.GetInstance().GetSOInspectionAllocatedMan(soInfo.SysNo);
                    oWhpsi.RealUserSysNo = oWhpsi.AllocatedUserSysNo;
                    oWhpsi.UpdateUserSysNo = 33;//IAS系统
                    oWhpsi.UpdateTime = DateTime.Now;
                    WhProductShelvingInspectionManager.GetInstance().Insert(oWhpsi);
                }
                else
                {
                    oWhpsi.SysNo = whpsiSysNo;
                    oWhpsi.BillSysNo = soInfo.SysNo;
                    oWhpsi.WorkType = (int)AppEnum.WhWorkType.ProductInspection;
                    oWhpsi.BillType = (int)AppEnum.WhWorkBillType.SO;
                    oWhpsi.AllocatedUserSysNo = UserRatioManager.GetInstance().GetSOInspectionAllocatedMan(soInfo.SysNo);
                    oWhpsi.RealUserSysNo = oWhpsi.AllocatedUserSysNo;
                    oWhpsi.UpdateUserSysNo = 33;//IAS系统
                    oWhpsi.UpdateTime = DateTime.Now;
                    WhProductShelvingInspectionManager.GetInstance().Update(oWhpsi);
                }

				scope.Complete();
            }
		}
		
		public void CancelOutStock(SOInfo soInfo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {	
				//作废未确认的有效收款单, 如果已经财务确认会抛出异常
				SOIncomeManager.GetInstance().SOCancelOutStock(soInfo.SysNo);
				
				//检查是否存在有效的rma单，存在就不允许取消出库
				if(RMAManager.GetInstance().IfExistValidRMA(soInfo.SysNo))
					throw new BizException("有对应此SO的有效RMA单存在，不能作废订单");

                //查看是否是前月出库的订单，不允许隔月取消出库，隔月取消只能在4天内取消
                if (soInfo.OutTime.Month != DateTime.Now.Month && soInfo.OutTime <= DateTime.Now.AddDays(-4))
                    throw new BizException("非本月订单，不允许隔月取消！");

                ////更新订单状态
                //1 查看订单当前状态
                //2 设置更新值，更新status
                int currentStatus = this.getCurrentSOStatus(soInfo.SysNo);
                if (currentStatus != (int)AppEnum.SOStatus.OutStock)
                    throw new BizException("cancel outstock so: the current status is not OutStock, operation of Cancel OutStock SO failed");

                soInfo.Status = (int)AppEnum.SOStatus.Origin;
				//更新订单状态
				UpdateSOStatus(soInfo.SysNo, soInfo.Status, soInfo.UpdateUserSysNo);
				//更新商品库存
				foreach(SOItemInfo item in soInfo.ItemHash.Values)
				{
					InventoryManager.GetInstance().SetSOOutStockQty(soInfo.StockSysNo,item.ProductSysNo,-1*item.Quantity);
				}
				//查看是否有赠送积分
				if(soInfo.PointAmt>0)
				{
					SalePointDelayInfo spInfo  =  PointManager.GetInstance().LoadValid(soInfo.SysNo);
					if(spInfo!=null)
					{
						if(spInfo.Status==(int)AppEnum.TriStatus.Handled)//积分已经加入客户账户，则先扣除对应积分
						{
							PointManager.GetInstance().SetScore(soInfo.CustomerSysNo, soInfo.PointAmt*(-1), (int)AppEnum.PointLogType.CancelOutstock,soInfo.SysNo.ToString());
						}
						//更新积分赠送记录状态
						spInfo.Status = (int)AppEnum.TriStatus.Abandon;
						PointManager.GetInstance().UpdatePointDelay(spInfo);
					}
				}
				scope.Complete();
            }
		}

		public void AbandonSO_Employee(SOInfo soInfo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				if(this.getCurrentSOStatus(soInfo.SysNo)!=(int)AppEnum.SOStatus.Origin)
					throw new BizException("The status is not origin now ,please reload this so.");
				soInfo.Status = (int)AppEnum.SOStatus.EmployeeCancel;
				this.AbandonSO(soInfo);	
				scope.Complete();
            }
		}

		public void AbandonSO_Manager(SOInfo soInfo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				if(this.getCurrentSOStatus(soInfo.SysNo)!=(int)AppEnum.SOStatus.WaitingManagerAudit)
					throw new BizException("The status is not waitingmanageraudit now ,please reload this so.");
				soInfo.Status = (int)AppEnum.SOStatus.ManagerCancel;
				this.AbandonSO(soInfo);
				scope.Complete();
            }
		}

		public void AbandonSO_Customer(SOInfo soInfo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				if(this.getCurrentSOStatus(soInfo.SysNo)!=(int)AppEnum.SOStatus.Origin)
					throw new BizException("您的订单已经过处理，不能直接作废，请联系ORS商城客服");
				soInfo.Status = (int)AppEnum.SOStatus.CustomerCancel;
				this.AbandonSO(soInfo);
				scope.Complete();
            }
		}
		
		/// <summary>
		/// 批量作废过期SO
		/// </summary>
		/// <param name="paramHash"></param>
		public void AbandonSOExpired(Hashtable paramHash)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				if(paramHash.Count!=0)
				{
					foreach(int sosysno in paramHash.Keys)
					{
						SOInfo soInfo = this.LoadSO(sosysno);
						this.AbandonSO_Employee(soInfo);
					}
				}
				scope.Complete();
            }
		}

		public void CancelAbandonSO(SOInfo soInfo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                int currentStatus = this.getCurrentSOStatus(soInfo.SysNo);
                if (currentStatus != (int)AppEnum.SOStatus.CustomerCancel && currentStatus != (int)AppEnum.SOStatus.EmployeeCancel && currentStatus != (int)AppEnum.SOStatus.ManagerCancel)
                    throw new BizException("cancel abandon so: the current status is not CustomerCancel/EmployeeCancel/ManagerCancel, operation of Cancel Abandon SO failed");

				//更新订单状态
				soInfo.Status = (int)AppEnum.SOStatus.Origin;
				this.UpdateSOStatus(soInfo.SysNo, soInfo.Status, soInfo.UpdateUserSysNo);
				//更新库存
				foreach(SOItemInfo item in soInfo.ItemHash.Values)
				{
					InventoryManager.GetInstance().SetOrderQty(soInfo.StockSysNo,item.ProductSysNo,item.Quantity);
				}
				//扣除客户支付积分
				PointManager.GetInstance().SetScore(soInfo.CustomerSysNo, soInfo.PointPay*-1, (int)AppEnum.PointLogType.CancelAbandonSO,soInfo.SysNo.ToString());
                //扣除客户支付免运费
                //FreeShipFeeManager.GetInstance().SetFreeShipFee(soInfo.CustomerSysNo, soInfo.FreeShipFeePay * -1, (int)AppEnum.FreeShipFeeLogType.CancelAbandonSO, soInfo.SysNo.ToString());
				scope.Complete();
            }
		}

		public void SetDeliveryDate(SOInfo soInfo)
		{
			Hashtable paramHash = new Hashtable();
			paramHash.Add("SysNo",soInfo.SysNo);
			paramHash.Add("DeliveryDate",soInfo.DeliveryDate);
			this.UpdateSOMaster(paramHash);
		}
		
		/// <summary>
		/// 检查该客户是否在一分钟内生成过订单，防止重复下单
		/// </summary>
		/// <param name="customerSysNo"></param>
		/// <returns></returns>
		public bool SOCreatePreCheck(int customerSysNo)
		{
			string sql = @"select * from so_master where OrderDate>"+Util.ToSqlString(DateTime.Now.AddMinutes(-1).ToString(AppConst.DateFormatLong))+" and customersysno = "+customerSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if(Util.HasMoreRow(ds))
				return false;
			else
				return true;
		}

        /// <summary>
        /// 检查限购商品，一天内一个用户只能购买一次限购数量范围内的商品
        /// </summary>
        /// <param name="customerSysNo"></param>
        /// <param name="productList"></param>
        /// <returns></returns>
        public string SOCheckLimitedQtyProduct(int customerSysNo, string productList)
        {
            string sql = @"select si.productsysno,p.productname,pp.limitedqty from so_master sm inner join so_item si on sm.sysno=si.sosysno 
                        inner join product p on si.productsysno=p.sysno
                        inner join product_price pp on pp.productsysno=p.sysno 
                        where sm.status>=0 and sm.customersysno=@customerSysNo and si.productsysno in(@Products) 
                        and orderdate > DateAdd(day,-1,getdate()) 
                        and pp.limitedqty > 0 and pp.limitedqty < 999";
            sql = sql.Replace("@customerSysNo", customerSysNo.ToString());
            sql = sql.Replace("@Products", productList);
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return "";
            else
            {
                string sReturn = "";
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    sReturn += Util.TrimNull(dr["productname"]) + "，每天限购一次，每次限购" + Util.TrimIntNull(dr["limitedqty"].ToString()) + "个<br>";
                }
                sReturn += "请回到 <a href='../Shopping/ShoppingCart.aspx'>购物车</a> 去除限购商品！";
                return sReturn;
            }
        }

		private void AbandonSO(SOInfo soInfo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				//处理收款单
				SOIncomeManager.GetInstance().SOAbandon(soInfo.SysNo);
				//更新订单状态
				this.UpdateSOStatus(soInfo.SysNo, soInfo.Status, soInfo.UpdateUserSysNo);
				//释放订单所占库存
				foreach(SOItemInfo item in soInfo.ItemHash.Values)
				{
					InventoryManager.GetInstance().SetOrderQty(soInfo.StockSysNo,item.ProductSysNo,-1*item.Quantity);
				}
				//返还客户支付积分
				PointManager.GetInstance().SetScore(soInfo.CustomerSysNo, soInfo.PointPay, (int)AppEnum.PointLogType.AbandonSO, soInfo.SysNo.ToString());
                //返还客户支付免运费
                //FreeShipFeeManager.GetInstance().SetFreeShipFee(soInfo.CustomerSysNo,soInfo.FreeShipFeePay,(int)AppEnum.FreeShipFeeLogType.AbandonSO,soInfo.SysNo.ToString());
				scope.Complete();
            }
		}

		public void SetDeliveryMen(int[] soSysNos,int freightManSysNo,string deliveryMemo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				for(int i=0;i<soSysNos.Length;i++)
				{
					Hashtable updateHash = new Hashtable();
					updateHash.Add("SysNo",soSysNos[i]);
					updateHash.Add("freightusersysno",freightManSysNo);
					updateHash.Add("DeliveryMemo",deliveryMemo);
					updateHash.Add("SetDeliveryManTime",System.DateTime.Now);
					this.UpdateSOMaster(updateHash);
				}
				scope.Complete();
            }
		}

        public void SetDeliveryMen(int[] soSysNos, int freightManSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                for (int i = 0; i < soSysNos.Length; i++)
                {
                    Hashtable updateHash = new Hashtable();
                    updateHash.Add("SysNo", soSysNos[i]);
                    updateHash.Add("freightusersysno", freightManSysNo);
                    updateHash.Add("SetDeliveryManTime", System.DateTime.Now);
                    this.UpdateSOMaster(updateHash);
                }
                scope.Complete();
            }
        }

        //批量设置配送人
        public void SetDeliveryMenBatch(string soSysNos,int freightManSysNo)
        {
            new SODac().SetDeliveryMenBatch(soSysNos,freightManSysNo);
        }

		private bool IfExistSOItem(SOItemInfo itemInfo)
		{
			string sql = "select * from so_item where sosysno ="+itemInfo.SOSysNo+" and productsysno="+itemInfo.ProductSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if(Util.HasMoreRow(ds))
				return true;
			else
				return false;
		}

		private bool CheckGiftQty(SOInfo soInfo)
		{
			Hashtable giftHash = new Hashtable();
			Hashtable parentHash = new Hashtable();
			foreach(SOItemInfo item in soInfo.ItemHash.Values)
			{
				if(item.ProductType==(int)AppEnum.SOItemType.ForSale)
					parentHash.Add(item.ProductSysNo,item);
				else if(item.ProductSysNo==(int)AppEnum.SOItemType.Gift)
					giftHash.Add(item.ProductSysNo,item);
			}
			foreach(SOItemInfo pitem in parentHash.Values)
			{
				if(pitem.GiftSysNo!=AppConst.IntNull)
				{
					foreach(SOItemInfo gitem in giftHash.Values)
					{
						if(gitem.ProductSysNo==pitem.GiftSysNo)
						{
							gitem.Quantity -= pitem.Quantity;
							break;
						}
					}
				}
			}
			foreach(SOItemInfo leftGift in giftHash.Values)
			{
				if(leftGift.Quantity>0)
				{
					return false;
				}
			}
			return true;
		}

		public void InitAllocatedMap()
		{
			string ratioStr = AppConfig.SOAllocatedMap; // 23:4;34:5
            if (ratioStr.ToString() == "") return;
			string[] ratioGroup = ratioStr.Split(';');

			//转换为方便使用
			Hashtable ht = new Hashtable(ratioGroup.Length);

			int ratioTotal = 0;
			for(int i=0; i<ratioGroup.Length; i++)
			{
				string[] temp = ratioGroup[i].Split(':');
				string userID = temp[0];
				int ratio = Convert.ToInt32(temp[1]);
				UserInfo user = SysManager.GetInstance().LoadUser(userID);
				ht.Add(user, ratio);
				ratioTotal += ratio;
			}

			//按比例随机map人员
			allocatedManHash.Clear();
			foreach(UserInfo user in ht.Keys)
			{
				int ratio = (int)ht[user];
				for(int i=0; i< ratio; i++ )
				{
					bool found = false;
					do
					{
						System.Random oRandom = new System.Random(RandomString.GetNewSeed());
						int tempkey = oRandom.Next(ratioTotal);
						if ( !allocatedManHash.ContainsKey(tempkey) )
						{
							found = true;
							allocatedManHash.Add(tempkey, user);
						}
					} while( !found );
				}
			}
		}

        /// <summary>
        /// 获取订单预分配审单人
        /// </summary>
        /// <returns></returns>
        private int GetAllocatedMan(int soSysNo)
        {
            if (allocatedManHash == null || allocatedManHash.Count == 0)
                throw new BizException("allocated man hash is empty");
            int key = soSysNo % allocatedManHash.Count;
            return ((UserInfo)allocatedManHash[key]).SysNo;
        }

        public Hashtable GetAllocatedMenHash()
        {
            return allocatedManHash;
        }

		public void SaveVAT(SOVATInfo vatInfo)
		{
			if(vatInfo.SOSysNo==AppConst.IntNull)
				throw new BizException("The SO doesn't exist,can't save vat");
			string sql = @"select * from so_valueAdded_invoice where sosysno ="+vatInfo.SOSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if(Util.HasMoreRow(ds))
			{
                this.UpdateSOVAT(vatInfo);
			}
			else
			{
				this.InsertSOVAT(vatInfo);
			}
		}

		/// <summary>
		/// 根据订单的支付方式和现在的支付情况计算 订单的状态
		/// 如果是货到付款，或者款到发货并且款已清，就置为"待出库"
		/// 否则置为"待支付"
		/// </summary>
		/// <param name="soInfo"></param>
		/// <param name="isForce"></param>
		private void calcWaitingStatus(SOInfo soInfo,bool isForce)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				//检验支付方式
				PayTypeInfo ptInfo = ASPManager.GetInstance().LoadPayType(soInfo.PayTypeSysNo);
				if(ptInfo.IsPayWhenRecv==(int)AppEnum.YNStatus.Yes)//货到付款可以直接出库
					soInfo.Status = (int)AppEnum.SOStatus.WaitingOutStock;
				else
				{
                    //检验支付信息
                    SOIncomeInfo soIncome = SOIncomeManager.GetInstance().LoadValid((int)AppEnum.SOIncomeOrderType.SO, soInfo.SysNo);
                    if (soIncome == null)//没有有效支付记录
                        soInfo.Status = (int)AppEnum.SOStatus.WaitingPay;
                    //else if( soIncome.IncomeAmt<soInfo.GetTotalAmt() && !isForce )//支付金额不足
                    //    throw new BizException("calcWaitingStatus: soincome is less than so total amt, please check or do force audit");
                    else if (soIncome.IncomeAmt != soInfo.GetTotalAmt() && !isForce)//支付金额不等
                        throw new BizException("calcWaitingStatus: soincome is not equal so total amt, please link AC or do force audit");
                    else
                        soInfo.Status = (int)AppEnum.SOStatus.WaitingOutStock;
				}
				scope.Complete();
            }
		}

		public DataSet CheckItemAccountQty(SOInfo soInfo)
		{
			string sql = @"select si.productsysno,(case when isnull(i.accountqty,0)<si.quantity then "+(int)AppEnum.YNStatus.No+" else "+(int)AppEnum.YNStatus.Yes
						 +" end) as Inventory,(case when isnull(s.accountqty,0)<si.quantity then "+(int)AppEnum.YNStatus.No+" else "+(int)AppEnum.YNStatus.Yes
						 +@" end) as stock
						   from so_item si 
						   left join inventory i on i.productsysno = si.productsysno
						   left join inventory_stock s on s.productsysno = si.productsysno and s.stocksysno = "+soInfo.StockSysNo
						   +" where sosysno = "+soInfo.SysNo;
			return SqlHelper.ExecuteDataSet(sql);
		}
	
		public decimal GetEndMoney(SOInfo soInfo)
		{
			PayTypeInfo ptInfo = ASPManager.GetInstance().LoadPayType(soInfo.PayTypeSysNo);
			decimal amt = soInfo.GetTotalAmt();
			if(ptInfo!=null&&ptInfo.IsPayWhenRecv==(int)AppEnum.YNStatus.Yes)
			{
				amt = Util.TruncMoney(amt);
			}
			return amt;
		}	

		public int GetMinPoint(SOInfo soInfo)
		{
			int minPoint = 0;
			foreach(SOItemInfo soItem in soInfo.ItemHash.Values)
			{
				if(soItem.PointType==(int)AppEnum.ProductPayType.PointPayOnly)
				{
					minPoint += Convert.ToInt32(Util.TruncMoney(soItem.Price*soItem.Quantity+soItem.DiscountAmt)*AppConst.ExchangeRate);
				}
			}
			return minPoint;
		}

		public int GetMaxPoint(SOInfo soInfo)
		{
			int maxPoint = 0;
			foreach(SOItemInfo soItem in soInfo.ItemHash.Values)
			{
				if(soItem.PointType==(int)AppEnum.ProductPayType.PointPayOnly||soItem.PointType==(int)AppEnum.ProductPayType.BothSupported)
				{
					maxPoint += Convert.ToInt32(Util.TruncMoney(soItem.Price*soItem.Quantity+soItem.DiscountAmt)*AppConst.ExchangeRate);
				}
			}
			return maxPoint;
		}

		#region Invoice Print
		

		public SOInvoiceInfo GetSOInvoice(int soSysNo)
		{
			SOInvoiceInfo soInvoice = new SOInvoiceInfo();
			SOInfo soInfo = SaleManager.GetInstance().LoadSO(soSysNo);
			if(soInfo == null)
			{
				throw new BizException("Sale Order Doesn't Exist!!!");
			}
			if(soInfo.ItemHash == null||soInfo.ItemHash.Count == 0)
			{
				throw new BizException("No Product In This Sale Order!!!");
			}
			UserInfo employee = SysManager.GetInstance().LoadUser(soInfo.OutUserSysNo);
			CustomerInfo customer = CustomerManager.GetInstance().Load(soInfo.CustomerSysNo);
			if(employee != null)
			{
				soInvoice.WarehouseUserCode = employee.UserID;
			}
			else            
			{
				soInvoice.WarehouseUserCode = "";
			}
			soInvoice.CustomerName = customer.CustomerName;
			soInvoice.ReceiveName = soInfo.ReceiveName;
			soInvoice.ReceiveContact = soInfo.ReceiveContact;
			//在上海的地址前，加上区
			AreaInfo area = ASPManager.GetInstance().LoadArea(soInfo.ReceiveAreaSysNo);
			//if(area.CityName.IndexOf("上海市")!=-1)
				soInvoice.ReceiveAddress = area.ProvinceName+area.CityName+area.DistrictName + "," +soInfo.ReceiveAddress;
			//else
               // soInvoice.ReceiveAddress = soInfo.ReceiveAddress;
			soInvoice.ReceivePhone = soInfo.ReceivePhone;
			soInvoice.ReceiveCellPhone = soInfo.ReceiveCellPhone;
			soInvoice.InvoiceNote = soInfo.InvoiceNote;
			SOIncomeInfo income = SOIncomeManager.GetInstance().LoadValid((int)AppEnum.SOIncomeOrderType.SO,soInfo.SysNo);
			if(income!=null&&income.IncomeStyle==(int)AppEnum.SOIncomeStyle.Advanced)
				soInvoice.InvoiceNote = "已付款; "+soInvoice.InvoiceNote;
			soInvoice.CustomerSysNo = soInfo.CustomerSysNo;
			PayTypeInfo ptInfo = ASPManager.GetInstance().LoadPayType(soInfo.PayTypeSysNo);
			soInvoice.PayTypeName = ptInfo.PayTypeName;
			soInvoice.SOID = soInfo.SOID;
			soInvoice.SOSysNo = soInfo.SysNo;
			soInvoice.OutStockID = soInfo.SOID;
			ShipTypeInfo stInfo = ASPManager.GetInstance().LoadShipType(soInfo.ShipTypeSysNo);
			soInvoice.ShipTypeName = stInfo.ShipTypeName;
			soInvoice.IsVAT = soInfo.IsVAT;			
			this.InitPageList(soInfo,soInvoice);
			soInvoice.TotalPage = soInvoice.ItemPageHash.Count;
			soInvoice.TotalWeight = soInfo.GetTotalWeight();
			//Icson Change
			soInvoice.DeliveryMemo = soInfo.DeliveryMemo;
            if (soInfo.AuditDeliveryDate != AppConst.DateTimeNull)
            {
                if (soInfo.AuditDeliveryTimeSpan == 1)
                {
                    soInvoice.AuditDeliveryDateTime = soInfo.AuditDeliveryDate.ToString(AppConst.DateFormat) + " 上午";
                }
                else
                {
                    soInvoice.AuditDeliveryDateTime = soInfo.AuditDeliveryDate.ToString(AppConst.DateFormat) + " 下午";
                }
            }

		    return soInvoice;
		}

		private void InitPageList(SOInfo soInfo,SOInvoiceInfo invoice)
		{
            invoice.HasServiceProduct = false;
			int index = 0;
			SOInvoicePageInfo page = new SOInvoicePageInfo();
			invoice.ItemPageHash.Add(index++,page);
			if(soInfo.ItemHash.Count>0)
			{
				Hashtable sysNoHash = new Hashtable();
				foreach(SOItemInfo item in soInfo.ItemHash.Values)
				{
					sysNoHash.Add(item.ProductSysNo,item.ProductSysNo);
				}
				Hashtable idHash = ProductManager.GetInstance().GetProductBoundle(sysNoHash);
				Hashtable posHash = InventoryManager.GetInstance().GetInventoryStockBoundle(sysNoHash,soInfo.StockSysNo);
				foreach(SOItemInfo item in soInfo.ItemHash.Values)
				{
					SOInvoicePageItemInfo printItem = new SOInvoicePageItemInfo();
					foreach(ProductBasicInfo pbInfo in idHash.Values)
					{
						if(pbInfo.SysNo == item.ProductSysNo)
						{
                            printItem.ProductSysNo = pbInfo.SysNo;
							printItem.ProductID = pbInfo.ProductID;
							printItem.ProductName = pbInfo.ProductName;

                            if (item.BaseProductType == (int)AppEnum.ProductType.Service)
                                invoice.HasServiceProduct = true;

							break;
						}
					}
					foreach(InventoryStockInfo isInfo in posHash.Values)
					{
						if(isInfo.ProductSysNo == item.ProductSysNo)
						{
							if ( isInfo.Position1 != "00-000-000" )
							{
								//printItem.ProductName += "(捡:" + isInfo.Position1 + ")";
                                printItem.ProductName += "(捡:<font size=4>" + isInfo.Position1 + "</font>)";
							}
							else if ( isInfo.Position2 != "00-000-000" )
							{
								//printItem.ProductName += "(捡:" + isInfo.Position2 + ")";
                                printItem.ProductName += "(捡:<font size=4>" + isInfo.Position2 + "</font>)";
							}
							break;
						}
					}
					printItem.Quantity = item.Quantity;
					printItem.Weight = item.Weight;
					printItem.Price = item.Price;
					printItem.SubTotal = item.Quantity*item.Price;
					printItem.IsPoint = false;

					printItem.IsSOItem = true;
					printItem.Warranty = item.Warranty;

					if(page.AddItem(printItem) == true)
					{
						continue;
					}
					else
					{
						page = new SOInvoicePageInfo();
						invoice.ItemPageHash.Add(index++,page);
						page.AddItem(printItem);						
					}
				}
			}
			if(soInfo.ShipPrice != 0)
			{
				SOInvoicePageItemInfo printItem = new SOInvoicePageItemInfo();
				printItem.ProductID = "运费";
				printItem.SubTotal = soInfo.ShipPrice;
				printItem.IsSOItem = false;
				if(page.AddItem(printItem) == false)
				{
					page = new SOInvoicePageInfo();
					invoice.ItemPageHash.Add(index++,page);
					page.AddItem(printItem);
				}
			}

            if (soInfo.FreeShipFeePay != 0)
            {
                SOInvoicePageItemInfo printItem = new SOInvoicePageItemInfo();
                printItem.ProductID = "免运费金额";
                printItem.SubTotal = -soInfo.FreeShipFeePay;
                printItem.IsSOItem = false;
                if (page.AddItem(printItem) == false)
                {
                    page = new SOInvoicePageInfo();
                    invoice.ItemPageHash.Add(index++, page);
                    page.AddItem(printItem);
                }
            }

			if(soInfo.DiscountAmt!=0)
			{
				SOInvoicePageItemInfo printItem = new SOInvoicePageItemInfo();
				printItem.ProductID = "促销优惠";
				printItem.SubTotal = -soInfo.DiscountAmt;
				printItem.IsSOItem = false;
				if(page.AddItem(printItem) == false)
				{
					page = new SOInvoicePageInfo();
					invoice.ItemPageHash.Add(index++,page);
					page.AddItem(printItem);
				}
			}

			if(soInfo.PremiumAmt != 0)
			{
				SOInvoicePageItemInfo printItem = new SOInvoicePageItemInfo();
				printItem.ProductID = "保价费";
				printItem.SubTotal = soInfo.PremiumAmt;
				printItem.IsSOItem = false;
				if(page.AddItem(printItem) == false)
				{
					page = new SOInvoicePageInfo();
					invoice.ItemPageHash.Add(index++,page);
					page.AddItem(printItem);
				}
			}

			if(soInfo.PayPrice != 0)
			{
				SOInvoicePageItemInfo printItem = new SOInvoicePageItemInfo();
				printItem.ProductID = "付款手续费";
				printItem.SubTotal = soInfo.PayPrice;
				printItem.IsSOItem = false;
				if(page.AddItem(printItem) == false)
				{
					page = new SOInvoicePageInfo();
					invoice.ItemPageHash.Add(index++,page);
					page.AddItem(printItem);
				}
			}

			if(soInfo.GetTotalAmt() != this.GetEndMoney(soInfo))
			{
				SOInvoicePageItemInfo printItem = new SOInvoicePageItemInfo();
				printItem.ProductID = "去零头";
				printItem.SubTotal = soInfo.GetTotalAmt()-this.GetEndMoney(soInfo);
				printItem.IsSOItem = false;
				if(page.AddItem(printItem) == false)
				{
					page = new SOInvoicePageInfo();
					invoice.ItemPageHash.Add(index++,page);
					page.AddItem(printItem);
				}
			}
			if(soInfo.PointPay != 0)
			{
				SOInvoicePageItemInfo printItem= new SOInvoicePageItemInfo();
				printItem.ProductID="本单积分抵扣";
				printItem.SubTotal=Convert.ToDecimal(soInfo.PointPay)/AppConst.ExchangeRate*(-1);
				printItem.IsSOItem = false;
				if(page.AddItem(printItem) == false)
				{
					page = new SOInvoicePageInfo();
					invoice.ItemPageHash.Add(index++,page);
					page.AddItem(printItem);
				}
			}

			if(soInfo.PointAmt != 0)
			{
				SOInvoicePageItemInfo printItem = new SOInvoicePageItemInfo();
				printItem.ProductID = "本单可得积分";
				printItem.SubTotal = Convert.ToDecimal(soInfo.PointAmt);
				printItem.IsSOItem= false;
				printItem.IsPoint = true;
				if(page.AddItem(printItem) == false)
				{
					page = new SOInvoicePageInfo();
					invoice.ItemPageHash.Add(index++,page);
					page.AddItem(printItem);
				}
			}
		}
		/// <summary>
		/// 打印发票输出的收货地址
		/// </summary>
		/// <returns></returns>
		public string GetReceiveAddress(SOInfo soInfo)
		{
			string areaName = null;
			AreaInfo area = ASPManager.GetInstance().LoadArea(soInfo.ReceiveAreaSysNo);
			if(area != null)
			{
				areaName = area.CityName;
				if(areaName == null || areaName.Length == 0)
				{
					areaName = area.ProvinceName;
					if(areaName == null || areaName.Length == 0)
					{
						areaName = "";
					}
				}
				//上海市，如果所在区不是空，打印所在区名称
				if ( area.CityName == "上海市" && area.DistrictName != "") 
				{
					areaName += "," + area.DistrictName;
				}
			}		
			if(areaName != null && areaName.Length > 0)
			{
				areaName += ",";
			}		
			areaName += soInfo.ReceiveAddress;
			return areaName;
		}		
		#endregion

		public void SendEmail(SOInfo soInfo,int sendType)
		{
			//mail标题
			string mailsubject = "您在ORS商城的订单SO#" + soInfo.SOID;

			//取要发送的email地址
			if (soInfo.CustomerSysNo==AppConst.IntNull) return;
			CustomerInfo cInfo = CustomerManager.GetInstance().Load(soInfo.CustomerSysNo);
			string mailaddress = Util.TrimNull(cInfo.Email);

			//如果没有填写email，不进行处理
			if( mailaddress == "" || mailaddress == null ) return;
			#region get templet

			//获取模板
			string fileName = AppConfig.SOMailTemplet;

			FileStream aFile;
			try
			{
				aFile = new FileStream(fileName, FileMode.Open, FileAccess.Read);
			}
			catch
			{
				return;
			}
			StreamReader sr = new StreamReader(aFile, Encoding.Default);

			string templet = sr.ReadToEnd();
			sr.Close();
			#endregion

			#region head templet

			AreaInfo dwellArea = ASPManager.GetInstance().LoadArea(cInfo.DwellAreaSysNo);     //订货人
			AreaInfo receiveArea = ASPManager.GetInstance().LoadArea(soInfo.ReceiveAreaSysNo);

			string headTemplet = GetString( templet, "_beginHead", "_endHead");


			if (sendType==(int)AppEnum.SOEmailType.CreateSO) // 下订单
			{
				mailsubject += " -- 订单生成";
				//headTemplet = Rep( headTemplet, "_email_type_picture", "http://www.baby1one.com.cn/images/ordermail/confirm1.gif");
			}
			else if (sendType==(int)AppEnum.SOEmailType.AuditSO) // 订单审核完成
			{
				mailsubject += " -- 订单审核通过";
				//headTemplet = Rep( headTemplet, "_email_type_picture", "http://www.baby1one.com.cn/images/ordermail/confirm2.gif");
			}
			else if (sendType==(int)AppEnum.SOEmailType.OutStock)  // 订单出货完成
			{
				mailsubject += " -- 订单已经出库待配送";
				//headTemplet = Rep( headTemplet, "_email_type_picture", "http://www.baby1one.com.cn/images/ordermail/confirm3.gif");
			}
			else if (sendType==(int)AppEnum.SOEmailType.AddDelayPoint) //获得积分
			{
				mailsubject += " -- 订单已获得积分";
				//headTemplet = Rep( headTemplet, "_email_type_picture", "http://www.baby1one.com.cn/images/ordermail/confirm4.gif");
			}
            else if (sendType == (int)AppEnum.SOEmailType.AbandonSO)
            {
                mailsubject += " -- 订单被作废";
                //headTemplet = Rep( headTemplet, "_email_type_picture", "http://www.baby1one.com.cn/images/ordermail/confirm4.gif");
            }

			headTemplet = Rep( headTemplet, "_order_id", soInfo.SOID);
			headTemplet = Rep( headTemplet, "_order_time", soInfo.OrderDate.ToLongDateString());

			headTemplet = Rep( headTemplet, "_orderMan",  cInfo.CustomerName);
			headTemplet = Rep( headTemplet, "_orderProvince", dwellArea.ProvinceName);
			headTemplet = Rep( headTemplet, "_orderCity", dwellArea.CityName);
			headTemplet = Rep( headTemplet, "_orderDistrict", dwellArea.DistrictName);
			headTemplet = Rep( headTemplet, "_orderAddress", cInfo.DwellAddress);
			headTemplet = Rep( headTemplet, "_orderZip", cInfo.DwellZip);
			headTemplet = Rep( headTemplet, "_orderPhone", cInfo.Phone);

			headTemplet = Rep( headTemplet, "_recvMan", soInfo.ReceiveName);
			headTemplet = Rep( headTemplet, "_recvContact", soInfo.ReceiveContact);
			headTemplet = Rep( headTemplet, "_recvProvince", receiveArea.ProvinceName);
			headTemplet = Rep( headTemplet, "_recvCity", receiveArea.CityName);
			headTemplet = Rep( headTemplet, "_recvDistrict", receiveArea.DistrictName);
			headTemplet = Rep( headTemplet, "_recvAddress", soInfo.ReceiveAddress);
			headTemplet = Rep( headTemplet, "_recvZip", soInfo.ReceiveZip);
			headTemplet = Rep( headTemplet, "_recvPhone", soInfo.ReceivePhone);

			
			#endregion

			#region bodyTemplet

			//1 get body, from body we get body templet
			//2 get rowTemplet
			string body = GetString(templet, "_beginBody", "_endBody");
			string bodyTemplet = RepField(body, "_beginRow", "_endRow", "_bodyRows");

			string rowTemplet = GetString( templet, "_beginRow", "_endRow");
			string rawRow;
			
			
			string bodyRows = "";

			Hashtable sysNoHash = new Hashtable();
			foreach(SOItemInfo itemInfo in soInfo.ItemHash.Values)
			{
				sysNoHash.Add(itemInfo.ProductSysNo,null);				
			}
			Hashtable pbHash = ProductManager.GetInstance().GetProductBoundle(sysNoHash);
			foreach(SOItemInfo item in soInfo.ItemHash.Values)
			{
				rawRow = rowTemplet;
				foreach(ProductBasicInfo pbInfo in pbHash.Values)
				{
					if(pbInfo.SysNo == item.ProductSysNo)
					{
						rawRow = Rep( rawRow, "_productId", pbInfo.ProductID);
						rawRow = Rep( rawRow, "_productName", pbInfo.ProductName);
						break;
					}
				}
				rawRow = Rep( rawRow, "_productQty", item.Quantity.ToString());

				
				if( item.ProductType==(int)AppEnum.SOItemType.ForSale )
				{
					rawRow = Rep( rawRow, "_productPrice", item.Price.ToString(AppConst.DecimalFormat));
					rawRow = Rep( rawRow, "_productLine", ((decimal)(item.Quantity * item.Price)).ToString(AppConst.DecimalFormat) );
				}
				else
				{
					rawRow = Rep( rawRow, "_productPrice", "赠品");
					rawRow = Rep( rawRow, "_productLine", "");
				}
				bodyRows += rawRow;
			}			

			bodyTemplet = Rep( bodyTemplet, "_bodyRows", bodyRows);
			
			//没有使用salerule，则不显示salerule列表
			if(soInfo.DiscountAmt==0)
			{
				
				bodyTemplet = Rep(bodyTemplet,"_saleRule","");
			}
			else
			{
				StringBuilder salerulebody = new StringBuilder(500);
				salerulebody.Append("<table width=609 border=0 cellpadding=2 cellspacing=1 bgcolor='#cccccc'>\n");
				salerulebody.Append("<tr bgcolor='#666666'>\n");		
				salerulebody.Append("<td height=16 colspan=4 class='order'><strong>促销优惠</strong></td></tr>");			
				salerulebody.Append("<tr bgcolor='#fffff9'>\n")	;
				salerulebody.Append("<td width='40%'><strong>规则描述</strong></td>\n");
				salerulebody.Append("<td width='20%'>单次折扣</td>\n");			
				salerulebody.Append("<td width='20%'>使用次数</td>\n");	
				salerulebody.Append("<td width='20%'>小计</td>\n");			
				salerulebody.Append("</tr>\n");			
						
			
				foreach(SOSaleRuleInfo srInfo in soInfo.SaleRuleHash.Values)
				{
					salerulebody.Append("<tr bgcolor='#fffff9'>\n");
					salerulebody.Append("<td width='40%'>"+srInfo.SaleRuleName+"</td>\n");
					salerulebody.Append("<td width='20%'>"+srInfo.Discount.ToString(AppConst.DecimalFormat)+"</td>\n");
					salerulebody.Append("<td width='20%'>"+srInfo.Times+"</td>\n");
					salerulebody.Append("<td width='20%'>"+ ((decimal)(srInfo.Discount*srInfo.Times)).ToString(AppConst.DecimalFormat)+"</td>\n");
					salerulebody.Append("</tr>\n");																														   
				}
				salerulebody.Append("</table>\n");
				bodyTemplet = Rep( bodyTemplet,"_saleRule",salerulebody.ToString());
				
			}
			bodyTemplet = Rep( bodyTemplet, "_user_get_point", soInfo.PointAmt.ToString());
			#endregion

			#region footTemplet

			string footTemplet = GetString(templet, "_beginFoot", "_endFoot");
			

			ShipTypeInfo stInfo = ASPManager.GetInstance().LoadShipType(soInfo.ShipTypeSysNo);
			PayTypeInfo ptInfo = ASPManager.GetInstance().LoadPayType(soInfo.PayTypeSysNo);

			footTemplet = Rep( footTemplet, "_shipType", stInfo.ShipTypeName);
			footTemplet = Rep( footTemplet, "_payType", ptInfo.PayTypeName);
			footTemplet = Rep( footTemplet, "_shipTime", stInfo.Period);

			//取出费用
			decimal cashamt = soInfo.SOAmt-(decimal)soInfo.PointPay/AppConst.ExchangeRate;
			decimal shipprice = soInfo.ShipPrice;
			decimal premiumamt = soInfo.PremiumAmt;
			decimal payprice = soInfo.PayPrice;
			decimal discountamt=soInfo.DiscountAmt;
			decimal totalamt = SaleManager.GetInstance().GetEndMoney(soInfo);
			int weight = 0;
			foreach(SOItemInfo oitem in soInfo.ItemHash.Values)
			{
				weight += oitem.Weight*oitem.Quantity;
			}

			StringBuilder footbody = new StringBuilder(500);

			footbody.Append("<tr bgcolor='#fffff9'>");
			footbody.Append("<td width='26%'><strong>现金支付总值(含税) </strong></td>");
			footbody.Append("<td><strong>" + cashamt.ToString(AppConst.DecimalFormat) + "</td>");
			footbody.Append("</tr>");
            
			if ( soInfo.PointPay>0)
			{
				footbody.Append("<tr bgcolor='#fffff9'>");
				footbody.Append("<td><strong>积分支付总值(含税)</strong></td>");
				footbody.Append("<td><strong>" + soInfo.PointPay + "</strong></td>");
				footbody.Append("</tr>");
			}

			footbody.Append("<tr bgcolor='#fffff9'>");
			footbody.Append("<td><strong>商品总重(含礼品)</strong></td>");
			footbody.Append("<td><strong>" + weight + "g</strong></td>");
			footbody.Append("</tr>");

			footbody.Append("<tr bgcolor='#fffff9'>");
			footbody.Append("<td><strong>运输费用</strong></td>");
			footbody.Append("<td><strong>" + soInfo.ShipPrice.ToString(AppConst.DecimalFormat) + "</strong></td>");
			footbody.Append("</tr>");

			footbody.Append("<tr bgcolor='#fffff9'>");
			footbody.Append("<td><strong>保险费</strong></td>");
			footbody.Append("<td><strong>" + soInfo.PremiumAmt.ToString(AppConst.DecimalFormat) + "</strong></td>");
			footbody.Append("</tr>");

			//如果没有手续费则不显示
			if( payprice != 0 )
			{
				footbody.Append("<tr bgcolor='#fffff9'>");
				footbody.Append("<td><strong>手续费</strong></td>");
				footbody.Append("<td><strong>" + soInfo.PayPrice.ToString(AppConst.DecimalFormat) + "</strong></td>");
				footbody.Append("</tr>");
			}

			if ( soInfo.DiscountAmt!=0 )
			{
				
				footbody.Append("<tr bgcolor='#fffff9'>");
				footbody.Append("<td><strong>促销优惠</strong></td>");
				footbody.Append("<td><strong>" + soInfo.DiscountAmt.ToString(AppConst.DecimalFormat) + "</strong></td>");
				footbody.Append("</tr>");
			}

			footbody.Append("<tr bgcolor='#fffff9'>");
			footbody.Append("<td><strong>去零头</strong></td>");
			footbody.Append("<td><strong>" + ((decimal)(totalamt - cashamt - shipprice - premiumamt - payprice-discountamt)).ToString(AppConst.DecimalFormat) + "</strong></td>");
			footbody.Append("</tr>");

			footbody.Append("<tr>");
			footbody.Append("<td><strong>总价</strong></td>");
			footbody.Append("<td><strong>" + totalamt.ToString(AppConst.DecimalFormat) + "</strong></td>");
			footbody.Append("</tr>");

			footTemplet = Rep( footTemplet, "_footbody", footbody.ToString());
			#endregion

			EmailInfo oMail = new EmailInfo();
			oMail.MailAddress = mailaddress;
			oMail.MailSubject = mailsubject;
			oMail.MailBody = headTemplet+bodyTemplet+footTemplet;
			oMail.Status = (int)AppEnum.TriStatus.Origin;
			EmailManager.GetInstance().InsertEmail(oMail);
		}

		private string Rep(string org, string source, string target)
		{
			return Rep( org, source, target, 0);
		}

		private string Rep( string org, string source, string target, int len)
		{
			if ( len != 0 )
			{
				if ( target.Length>len )
				{
					target = target.Substring(0, len);
				}
			}
			return org.Replace(source, target);
		}

		private string RepField(string org, string beginTag, string endTag, string block)
		{
			int beginIndex, endIndex;
			string result;

			beginIndex = org.IndexOf( beginTag, 0 );
			endIndex	= org.IndexOf( endTag, 0 );
			if ( endIndex <= beginIndex )
			{
				return org;
			}
			else
			{
				result = org.Substring(0, beginIndex) + org.Substring( endIndex );
				result = result.Replace( endTag, block);
				return result;
			}
		}

		private string GetString(string org, string beginTag, string endTag)
		{
			return GetString(org, beginTag, endTag, "");
		}

		private string GetString(string org, string beginTag, string endTag, string repBegin)
		{
			int beginIndex, endIndex;
			string result;
			beginIndex	= org.IndexOf( beginTag, 0);
			endIndex		= org.IndexOf( endTag, 0);
			if (endIndex <= beginIndex)
			{
				return "";
			}
			else
			{
				result	= org.Substring( beginIndex, endIndex-beginIndex );
				result	= result.Replace( beginTag, repBegin);
				return result;
			}
		}

        private string BuildSOID(int sysNo)
		{
			string sysNoStr = sysNo.ToString();
			int idLen = 10;
			string soid = "1";
			for(int i=0;i<(idLen-sysNoStr.Length-1);i++)
			{
				soid += "0";
			}
			soid += sysNoStr;
			return soid;
		}
		#region Sale Rule
		public void CalcSaleRule(SOInfo soInfo)
		{
			this.GetBestSaleRule(soInfo);
            this.GetBestSaleRuleFor99Bill(soInfo);
		}

		/// <summary>
		///查找订单so可以使用的所有salerule
		/// </summary>
		/// <param name="SO"></param>
		/// <returns>返回有效salerule的系统编号sysno列表</returns>
		public ArrayList GetValidSaleRule(SOInfo SO)
		{
			ArrayList validSRList=new ArrayList(1);
			if (SO==null)
				return null;
			
			if(AllValidSRList==null||AllValidSRList.Count==0)
				return null;

			Hashtable itemList= InitItemList(SO);
			foreach(int salerulesysno in AllValidSRList.Keys)
			{				
				if(CanApplySaleRule(itemList,salerulesysno))
					validSRList.Add(salerulesysno);				
			}
			return validSRList;

		}

		//初始化用于计算salerule组合的商品列表
		private Hashtable InitItemList(SOInfo SO)
		{
			Hashtable itemList= new Hashtable(50);
			foreach(SOItemInfo soitem in SO.ItemHash.Values)
			{
				itemList.Add(soitem.ProductSysNo,soitem.Quantity);
			}
			return itemList;
		}

		//检查某一条salerule是否能用于商品列表
		public bool CanApplySaleRule(Hashtable ItemList,int SaleRuleSysNo)
		{
			//获得该salerule的明细
			SaleRuleInfo sr=(SaleRuleInfo)AllValidSRList[SaleRuleSysNo];
			Hashtable saleruleList=sr.ItemHash;
			if(saleruleList==null||saleruleList.Count==0)
				return false;
			
			foreach(SaleRuleItemInfo sri in saleruleList.Values)
			{				
				//salerule中只要有一个商品在订单中不满足条件，该salerule对此商品列表无效
				if(!IsExistEnoughProduct(ItemList,sri.ProductSysNo,sri.Quantity))
					return false;
			}
			return true;

		}

		//检查某一条salerule中的商品是否在订单中存在，如果存在，数量是否够，符合条件返回true
		private bool IsExistEnoughProduct(Hashtable ItemList,int ProductSysNo,int Quantity )
		{
			if(ItemList.Contains(ProductSysNo)&&(int)ItemList[ProductSysNo]>=Quantity)
				return true;
			else return false;
		}

		/// <summary>
		/// 计算最佳salerule组合
		/// </summary>
		/// <param name="SO"></param>
		/// <return></return>
		public void GetBestSaleRule(SOInfo SO)
		{
			if(SO.IsWholeSale==0)//零售才可享受SaleRule
			{
				//初始化arraylist
				if(assembleList!=null && assembleList.Count>0)
					assembleList.Clear();

				//初始化所有有效的salerule
				AllValidSRList= this.GetAllValidSR();

				//获得有效的salerule列表
				ArrayList validSaleRuleList=GetValidSaleRule(SO);
				if(validSaleRuleList==null)
				{
					SO.DiscountAmt = 0m;
					return ;
				}
				//初始化定购商品列表，包括商品sysno，购买数量
				Hashtable itemList=InitItemList(SO);
				//初始化堆栈stackSRList
				if(StackSRList!=null&&StackSRList.Count>0)
					StackSRList.Clear();
				//初始化SO中原有的salerule组合
				if(SO.SaleRuleHash!=null&&SO.SaleRuleHash.Count>0 )
					SO.SaleRuleHash.Clear();
				//搜索所有可能的salerule组合
				SearchAllPath(itemList,validSaleRuleList,0);
				//找出最优的salerule组合
				SaleRuleInfo sr=new SaleRuleInfo();
				Hashtable ht=new Hashtable(50);
				decimal mindiscount=0;//记录最小折扣
				int optimize=0;//记录最佳组合
				for(int i=0;i<assembleList.Count;i++)
				{
					ht=(Hashtable)assembleList[i];
					decimal discount=0;
					foreach(int key in ht.Keys)
					{
						sr=(SaleRuleInfo)AllValidSRList[key];
						//获得此salerule的总折扣
						discount+=sr.GetTotalDiscount()*(int)ht[key];
					}
					if(discount<mindiscount) //discount<0
					{
						mindiscount=discount;
						optimize=i;
					}
					else
					{
						mindiscount=discount;
					}
				}
				//将discount记录到so中
				SO.DiscountAmt=mindiscount;				
				//将最优组合assembleList[optimize]保存为SOSaleRuleInfo类,存入so
			
				ht=(Hashtable)assembleList[optimize];

				foreach(int key in ht.Keys)
				{
					SOSaleRuleInfo ssr= new SOSaleRuleInfo();
					sr=(SaleRuleInfo)AllValidSRList[key];
					ssr.SaleRuleName=sr.SaleRuleName;
					ssr.Times=(int)ht[key];
					ssr.Discount=sr.GetTotalDiscount();
					ssr.Note=sr.GetSRNote();
					ssr.SOSysNo=SO.SysNo;
					ssr.SaleRuleSysNo=sr.SysNo;
					SO.SaleRuleHash.Add(ssr.SaleRuleSysNo,ssr);
				}
				//根据最佳salerule组合计算对订单每个商品的discountamt
				foreach(SOItemInfo item in SO.ItemHash.Values)
                {
                    item.DiscountAmt = GetProductDiscount(ht, item.ProductSysNo);
                }
			}
			else//批发清空原先SaleRule
			{
				SO.DiscountAmt = 0;
				SO.SaleRuleHash.Clear();
				foreach(SOItemInfo item in SO.ItemHash.Values)
				{
					item.DiscountAmt = 0;
				}
			}
		}

        public void GetBestSaleRuleFor99Bill(SOInfo SO)
        {
            if (SO.OrderDate < DateTime.Parse("2008-12-09"))
            {
                if (SO.PayTypeSysNo == 10 || SO.PayTypeSysNo == 16)  //快钱账户、快钱网银支付
                {
                    decimal discount = 0;
                    Hashtable ht = PromotionManager.GetInstance().GetPromotionProductSysNoHash(16);  //促销sysno=16
                    if (ht != null)
                    {
                        foreach (SOItemInfo item in SO.ItemHash.Values)
                        {
                            if (ht.ContainsKey(item.ProductSysNo))
                            {
                                if (Util.TrimDecimalNull(ht[item.ProductSysNo]) > 0)
                                {
                                    if (item.DiscountAmt <= item.Price * (1 - Util.TrimDecimalNull(ht[item.ProductSysNo])))
                                    {
                                        item.DiscountAmt = item.Price * (1 - Util.TrimDecimalNull(ht[item.ProductSysNo]));
                                        discount += item.Price * (1 - Util.TrimDecimalNull(ht[item.ProductSysNo])) * item.Quantity;
                                    }
                                }
                            }
                        }
                        if (discount > SO.DiscountAmt)
                            SO.DiscountAmt = discount;
                    }
                }
            }
        }

		public decimal GetProductDiscount(Hashtable SRList,int ProductSysNo)
		{
			SaleRuleInfo sr=new SaleRuleInfo();
			SaleRuleItemInfo sri=new SaleRuleItemInfo();
			decimal discount=0m;
			foreach(int key in SRList.Keys)
			{
				sr=(SaleRuleInfo)AllValidSRList[key];
				Hashtable ht=sr.ItemHash;
				if(ht.ContainsKey(ProductSysNo))
				{
					sri=(SaleRuleItemInfo)ht[ProductSysNo];
					discount+=sri.Discount*sri.Quantity*(int)SRList[key];
				}
			}
			return discount;
		}

		/// <summary>
		/// 搜索所有的salerule组合，递归实现
		/// </summary>
		/// <param name="itemList">当前的商品列表</param>
		/// <param name="ValidSaleRuleList">有效的salerule列表</param>
		/// <param name="StartPoint">应用salerule的起点</param>
		public void SearchAllPath(Hashtable itemList,ArrayList ValidSRList,int StartPoint)
		{
			bool isroot=true;
			int i;
			for(i=StartPoint;i<ValidSRList.Count;i++)
			{
				isroot=true;
				if(CanApplySaleRule(itemList,Convert.ToInt32(ValidSRList[i])))
				{
					isroot=false;
					Hashtable item=PushSaleRule(Convert.ToInt32(ValidSRList[i]),itemList);//压入堆栈，同时修改itemlist，减去应用该salerule的商品
					SearchAllPath(item,ValidSRList,i);
				}
			}
			if(i==ValidSRList.Count)
			{
				//记录路径，即堆栈中的salerule，计算discount
				if(isroot==true)
				{
					Hashtable saleruleList= new Hashtable(50);//记录一个可用的salerule组合，形式salerulesysno，使用次数
					for(int j=0;j<StackSRList.Count;j++)
					{
						int key=(int)StackSRList[j];
						if(saleruleList.ContainsKey(key))
						{
							int quantity=(int)saleruleList[key];
							saleruleList.Remove(key);
							saleruleList.Add(key,quantity+1);
						}
						else
						{
							saleruleList.Add(key,1);
						}
					}
					assembleList.Add(saleruleList);
				}
				PopSaleRule();//弹出栈顶的salerule
			}
		}

		/// <summary>
		/// 将salerule压入堆栈，同时修改商品列表，从商品列表中去除salerule的商品数量
		/// </summary>
		/// <param name="SaleRuleSysNo"></param>
		/// <param name="itemList"></param>
		/// <return>返回商品列表</return>
		public Hashtable PushSaleRule(int SaleRuleSysNo,Hashtable itemList)
		{
			//压栈
			StackSRList.Add(SaleRuleSysNo);
			SaleRuleInfo sr=(SaleRuleInfo)AllValidSRList[SaleRuleSysNo];
			Hashtable ht=sr.ItemHash;
			Hashtable item=new Hashtable(50);
			foreach(int key in itemList.Keys)
			{
				item.Add(key,itemList[key]);
			}
			
			foreach(SaleRuleItemInfo sri in ht.Values)
			{
				item[sri.ProductSysNo]=Convert.ToInt32(item[sri.ProductSysNo])-sri.Quantity;
			}
			return item;

		}

		public void PopSaleRule()
		{
			
			if(StackSRList!=null&&StackSRList.Count>0)
				StackSRList.RemoveAt(StackSRList.Count-1);
		
		}

        public Hashtable GetAllValidSR()
        {
            Hashtable SRList = new Hashtable(50);
            string sql = @"select sr.SaleRuleName,sr.Status,sr.CreateTime as srCreateTime,sri.* 
                            from SaleRule_Master sr(nolock) 
                            inner join SaleRule_Item sri(nolock) on sr.SysNo=sri.SaleRuleSysNo
                            where sr.Status = 0 
                            and sri.SaleRuleSysNo not in(select distinct SaleRuleSysNo from SaleRule_Item a(nolock) inner join Product b(nolock) on a.ProductSysNo = b.SysNo where b.Status<>1)
                            order by sr.SysNo,sri.ProductSysNo ";

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                int saleRuleSysNo = 0;
                int iRowIndex = 0;
                int iRowCount = ds.Tables[0].Rows.Count;

                SaleRuleInfo srInfo = null;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (saleRuleSysNo != Util.TrimIntNull(dr["SaleRuleSysNo"]))
                    {
                        srInfo = new SaleRuleInfo();
                        srInfo.SysNo = Util.TrimIntNull(dr["SaleRuleSysNo"]);
                        srInfo.SaleRuleName = Util.TrimNull(dr["SaleRuleName"]);
                        srInfo.Status = Util.TrimIntNull(dr["Status"]);
                        srInfo.CreateTime = Util.TrimDateNull(dr["srCreateTime"]);

                        saleRuleSysNo = Util.TrimIntNull(dr["SaleRuleSysNo"]);
                    }

                    SaleRuleItemInfo sriInfo = new SaleRuleItemInfo();
                    sriInfo.SysNo = Util.TrimIntNull(dr["SysNo"]);
                    sriInfo.SaleRuleSysNo = Util.TrimIntNull(dr["SaleRuleSysNo"]);
                    sriInfo.ProductSysNo = Util.TrimIntNull(dr["ProductSysNo"]);
                    sriInfo.Quantity = Util.TrimIntNull(dr["Quantity"]);
                    sriInfo.Discount = Util.TrimDecimalNull(dr["Discount"]);
                    sriInfo.CreateTime = Util.TrimDateNull(dr["CreateTime"]);

                    srInfo.ItemHash.Add(sriInfo.ProductSysNo, sriInfo);

                    iRowIndex++;

                    if (iRowIndex == iRowCount)
                    {
                        SRList.Add(srInfo.SysNo, srInfo);
                    }
                    else if (Util.TrimIntNull(dr["SaleRuleSysNo"]) != Util.TrimIntNull(ds.Tables[0].Rows[iRowIndex]["SaleRuleSysNo"]))
                    {
                        SRList.Add(srInfo.SysNo, srInfo);
                    }
                }
            }
            return SRList;
        }

		/// <summary>
		/// 根据商品sysno获得该商品在有效salerule中的最大折扣（折扣为负）
		/// </summary>
		/// <param name="ProductSysNo"></param>
		/// <returns></returns>
		public decimal GetMaxDiscountByProductSysNo(int ProductSysNo)
		{
			string sql = @"select min(discount) from salerule_item sri 
                                join salerule_master srm  on sri.salerulesysno = srm.sysno
                                where srm.status=0 and productsysno="+ProductSysNo.ToString();
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			object discount= ds.Tables[0].Rows[0][0];
			decimal discountamt;
			if(discount==System.DBNull.Value)
				discountamt=0;
			else discountamt=(decimal)discount;
			return discountamt;
		}
		#endregion

		#endregion	

		#region InvoicePrint

		#endregion

		public DataSet GetSOOnlineDs(int customerSysNo)
		{
			string sql = @"select
								so_master.sysno, soid, orderdate, cashpay+premiumamt+shipprice+payprice-freeshipfeepay-DiscountAmt-CouponAmt as totalcash,
								pointpay, pointamt, so_master.status, so_master.memo, ispaywhenrecv, paymentpage, isNet, doNo
							from
								so_master
							inner join paytype on so_master.paytypesysno = paytype.sysno
							left join do_master on so_master.sysno = do_master.sosysno
							where
								so_master.customersysno =" + customerSysNo;
			sql += " order by so_master.sysno desc";
			return SqlHelper.ExecuteDataSet(sql);
		}

        public DataSet GetSOOnlineDs(int customerSysNo,int top)
		{
			string sql = @"select @top 
								so_master.sysno, soid, orderdate, cashpay+premiumamt+shipprice+payprice-freeshipfeepay-DiscountAmt-CouponAmt as totalcash,
								pointpay, pointamt, so_master.status, so_master.memo, ispaywhenrecv, paymentpage, isNet, doNo
							from
								so_master
							inner join paytype on so_master.paytypesysno = paytype.sysno
							left join do_master on so_master.sysno = do_master.sosysno
							where
								so_master.customersysno =" + customerSysNo;
			sql += " order by so_master.sysno desc";
            sql = sql.Replace("@top", " top " + top);
			return SqlHelper.ExecuteDataSet(sql);
		}

		public string GetSOOnlineString(DataSet ds, int currentPage, int pageSize)
		{
			StringBuilder sb = new StringBuilder(1000);

			if ( !Util.HasMoreRow(ds))
			{
				sb.Append("<tr>");
				sb.Append("<td bgcolor=#FFFFFF colspan=4>");
				sb.Append("没有查到符合条件的历史纪录！");
				sb.Append("</td>");
				sb.Append("</tr>");
			}
			else
			{
				int totalPage = ds.Tables[0].Rows.Count / pageSize;
				if ( ds.Tables[0].Rows.Count % pageSize != 0)
					totalPage++;
				if ( currentPage > totalPage)
					currentPage = 1;
				int i = 1;
				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					if ( i > (currentPage-1)*pageSize && i<= currentPage*pageSize)
					{
						int soSysNo = Util.TrimIntNull(dr["sysno"]);
						string soID = Util.TrimNull(dr["soid"]);
						//sb.Append("<table cellspacing=\"0\" cellpadding=\"0\" width=\"790\" border=\"0\">");
						//sb.Append("	<tr>");
						//sb.Append("		<td>");
						sb.Append("			<table id=\"Table2\" bgcolor=\"#999999\" cellspacing=\"1\" cellpadding=\"2\" width=\"780\">");
						sb.Append("				<tr>");
						sb.Append("					<td width=70 height=\"25\" align=\"right\" bgcolor=\"#F0F0F0\">订单编号：</td>");
						sb.Append("					<td width=60 bgcolor=\"#FFFFFF\">").Append(Util.TrimNull(dr["soid"])).Append("</td>");
						sb.Append("					<td width=70 align=\"right\" bgcolor=\"#F0F0F0\">订购日期：</td>");
						sb.Append("					<td width=80 bgcolor=\"#FFFFFF\">").Append(Util.TrimDateNull(dr["OrderDate"]).ToString(AppConst.DateFormat)).Append("</td>");
						sb.Append("					<td width=70 align=\"right\" bgcolor=\"#F0F0F0\">现金总额：</td>");

						decimal totalCash = Util.TrimDecimalNull(dr["totalCash"]);
						if ( Util.TrimIntNull(dr["IsPayWhenRecv"]) == (int)AppEnum.YNStatus.Yes )
							totalCash = Util.TruncMoney(totalCash);
						sb.Append("					<td width=60 bgcolor=\"#FFFFFF\">").Append(totalCash.ToString(AppConst.DecimalFormat)).Append("</td>");

						sb.Append("					<td width=70 align=\"right\" bgcolor=\"#F0F0F0\">使用积分：</td>");
						sb.Append("					<td width=30 bgcolor=\"#FFFFFF\">").Append(Util.TrimIntNull(dr["PointPay"])).Append("</td>");
						sb.Append("					<td width=70 align=\"right\" bgcolor=\"#F0F0F0\">可得积分：</td>");
						sb.Append("					<td width=30 bgcolor=\"#FFFFFF\">").Append(Util.TrimIntNull(dr["PointAmt"])).Append("</td>");
						sb.Append("					<td width=70 align=\"right\" bgcolor=\"#F0F0F0\">运单编号：</td>");
						sb.Append("					<td width=80 bgcolor=\"#FFFFFF\">").Append(Util.TrimNull(dr["doNo"])).Append("</td>");
						sb.Append("				</tr>");
						sb.Append("				<tr>");
						sb.Append("					<td height=\"25\" align=\"right\" bgcolor=\"#F0F0F0\">订单状态：</td>");
						sb.Append("					<td bgcolor=\"#FFFFFF\">").Append(AppEnum.GetSOStatus(Util.TrimIntNull(dr["status"]))).Append("</td>");
						sb.Append("					<td align=\"middle\" colspan=\"10\" bgcolor=\"#FFFFFF\">");

						int soStatus = Util.TrimIntNull(dr["Status"]);
						bool isPay = NetPayManager.GetInstance().IsPayed( soSysNo );
						if ( soStatus == (int)AppEnum.SOStatus.Origin && !isPay )
                            sb.Append("						<a href='../Shopping/OrderDetail.aspx?action=cancel&ID=").Append(soSysNo.ToString()).Append("'>作废订单</a>&nbsp;&nbsp;");

						if ( soStatus == (int)AppEnum.SOStatus.Origin || soStatus == (int)AppEnum.SOStatus.WaitingPay || soStatus == (int)AppEnum.SOStatus.WaitingManagerAudit)
						{
							if ( Util.TrimNull(dr["paymentpage"]) != AppConst.StringNull && !isPay && Util.TrimIntNull(dr["IsNet"]) == (int)AppEnum.YNStatus.Yes)
								sb.Append("<a href='../Shopping/").Append( Util.TrimNull(dr["paymentpage"]) ).Append("?id=" + soSysNo.ToString() + "&sono=" + soID + "&soamt=" + totalCash.ToString("#####0.00") + "'>支付货款</a>&nbsp;&nbsp;");
						}

                        DataTable dt= RMARequestManager.GetInstance().GetRMABySO(soSysNo);
                        if (dt!=null)
                        {
                            sb.Append("						<a href='../Account/RMAQuery.aspx?Type=single&ID=").Append(soSysNo.ToString()).Append("'>查看返修信息</a>&nbsp;&nbsp;");
 
                        }
						sb.Append("						<a href='../Shopping/OrderDetail.aspx?ID=" + soSysNo.ToString() + "'>查看订单明细</a>&nbsp;&nbsp;&nbsp;");
						sb.Append("					</td>");
						sb.Append("				</tr>");
						sb.Append("				<tr>");
						sb.Append("					<td height=\"25\" align=\"right\" bgcolor=\"#F0F0F0\">备注信息：</td>");
						sb.Append("					<td colspan=\"11\" bgcolor=\"#FFFFFF\">");
						sb.Append("						<asp:Label id=\"lblRemark\" runat=\"server\">");
						//sb.Append("			<%#DataBinder.Eval(Container.DataItem, \"Memo\")%>&nbsp");
                        sb.Append(			            Util.TrimNull(dr["Memo"]) + "&nbsp");
						sb.Append("		</asp:Label></td>");
						sb.Append("				</tr>");
						sb.Append("</table><br>");
					}
					i++;
				}
			}
			return sb.ToString();
		}

        public string GetRecentOrderString(int customerSysNo)
        {
            DataSet ds = GetSOOnlineDs(customerSysNo,5);
            if(!Util.HasMoreRow(ds))
                return "";
            StringBuilder sb = new StringBuilder();
            sb.Append("<table width='100%' border=1 cellpadding=3 cellspacing=0 bordercolor='#b1d0ff' style='border-collapse:collapse'>");
            sb.Append(  "<tr class=c_gwc_l2>");
            sb.Append(      "<td width=80>订单号</td>");
            sb.Append(      "<td width=80>订购日期</td>");
            sb.Append(      "<td width=60>金额</td>");
            sb.Append(      "<td width=120>订单状态</td>");
            sb.Append(      "<td width=80>运单号</td>");
            sb.Append(      "<td width=90>积分说明</td>");
            sb.Append(      "<td width=70>返修查询</td>");
            sb.Append(  "</tr>");

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sb.Append("<tr>");
                sb.Append("<td><a href='../Shopping/OrderDetail.aspx?ID=" + Util.TrimNull(dr["sysno"]) + "'>" +
                          Util.TrimNull(dr["soid"]) + "</a></td>");
                sb.Append("<td>" + Util.TrimDateNull(dr["OrderDate"]).ToString(AppConst.DateFormat) + "</td>");
                decimal totalCash = Util.TrimDecimalNull(dr["totalCash"]);
                if (Util.TrimIntNull(dr["IsPayWhenRecv"]) == (int) AppEnum.YNStatus.Yes)
                    totalCash = Util.TruncMoney(totalCash);
                sb.Append("<td>" + totalCash.ToString(AppConst.DecimalFormat) + "</td>");

                if (Util.TrimIntNull(dr["status"]) != (int) AppEnum.SOStatus.OutStock)
                {
                    if (NetPayManager.GetInstance().IsPayed(Util.TrimIntNull(dr["sysno"])) && Util.TrimIntNull(dr["status"]) != (int) AppEnum.SOStatus.WaitingPay)
                    {
                        sb.Append("<td>已支付，" + AppEnum.GetSOStatus(Util.TrimIntNull(dr["status"])) + "</td>");
                    }
                    else
                    {
                        sb.Append("<td>" + AppEnum.GetSOStatus(Util.TrimIntNull(dr["status"])) + "</td>");
                    }
                }
                else
                {
                    sb.Append("<td>" + AppEnum.GetSOStatus(Util.TrimIntNull(dr["status"])) + "</td>");
                }
                sb.Append(  "<td>" + Util.TrimNull(dr["doNo"]) + "</td>");
                if (Util.TrimIntNull(dr["PointAmt"]) > 0)
                {
                    sb.Append("<td>" + Util.TrimIntNull(dr["PointAmt"]) + "分，订单成功三日后加至账户</td>");
                }
                else
                {
                    sb.Append("<td>0</td>");
                }
                if (RMARequestManager.GetInstance().GetRMABySO(Util.TrimIntNull(dr["sysno"]))!=null)
                {
                    sb.Append("<td><a href='../Account/RMAQuery.aspx?Type=single&ID=").Append(Util.TrimIntNull(dr["sysno"])).Append("'>查看返修信息</a></td>");
                }
                else
                {
                    sb.Append("<td></td>");
                }

              sb.Append("</tr>");
            }
            sb.Append("</table>");
            return sb.ToString();
        }

	    public DataSet GetFreightMenDs(Hashtable paramHash)
		{
            string sql = @" select 
								so_master.sysno, soid, receivename, receiveaddress, receivephone, receivecellphone,so_master.Status as SOStatus,
								so_master.IsVat,so_master.invoicenote, so_master.deliverymemo, username, area.districtname,area.localcode,
								cashpay+premiumamt+shipprice+payprice-freeshipfeepay-DiscountAmt as totalcash, ispaywhenrecv, paytypename,
                                so_master.auditdeliverydate,so_master.auditdeliverytimespan,so_master.SetDeliveryManTime,sys_user.username as freightusername
							from 
								so_master,
								sys_user,
								area,
								paytype
							where so_master.status=@status 
							and	so_master.paytypesysno = paytype.sysno
							and so_master.freightusersysno *= sys_user.sysno
							and so_master.receiveareasysno = area.sysno
							 ";

            string sql1 = " order by receiveareasysno,so_master.sysno ";

            sql = sql.Replace("@status",((int)AppEnum.SOStatus.OutStock).ToString());  //必须是已出库销售单

            if (paramHash != null && paramHash.Count != 0)
            {
                StringBuilder sb = new StringBuilder(100);
                foreach (string key in paramHash.Keys)
                {

                    object item = paramHash[key];
                    if (key == "DateFrom")
                    {
                        sb.Append(" and ");
                        sb.Append("SetdeliveryManTime >= ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "DateTo")
                    {
                        sb.Append(" and ");
                        sb.Append("SetdeliveryManTime <= ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "AuditDeliveryDate")
                    {
                        sb.Append(" and ");
                        sb.Append("AuditDeliveryDate = ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "AuditDeliveryTimeSpan")
                    {
                        sb.Append(" and ");
                        sb.Append("AuditDeliveryTimeSpan = ").Append(item.ToString());
                    }
                    else if (key == "SOSysNo")
                    {
                        sb.Append(" and ");
                        sb.Append("so_master.sysno = ").Append(item.ToString());
                    }
                    else if (key == "orderby")
                    {
                        sql1 = " order by SetDeliveryManTime desc ";
                    }
                    else if (item is int)
                    {
                        sb.Append(" and ");
                        sb.Append(key).Append("=").Append(item.ToString());
                    }
                    else if (item is string)
                    {
                        sb.Append(" and ");
                        sb.Append(key).Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                    }
                }
                sql += sb.ToString();
            }
            else
            {
                sql = sql.Replace("select", "select top 50");
            }

            sql += sql1;

            return SqlHelper.ExecuteDataSet(sql);
		}
		
		/// <summary>
		/// 取当前库存成本作为销售成本
		/// </summary>
		/// <param name="soSysNo"></param>
		private void SetSOItemCost(int soSysNo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				string sql = @"update 
								so_item
							set 
								so_item.cost = product_price.unitcost
							from 
								product_price
							where 
								product_price.productSysno = so_item.productsysno 
								and so_item.sosysno = " + soSysNo.ToString();
				new ProductSaleTrendDac().Exec(sql);
			scope.Complete();
            }
		}

        public int InsertSOService(SOServiceInfo oParam)
        {
            return new SOServiceDac().Insert(oParam);
        }

        public int UpdateSOService(SOServiceInfo oParam)
        {
            return new SOServiceDac().Update(oParam);
        }

        public int UpdateSOHasServiceProduct(int soSysNo)
        {
            string sql = "select sosysno from SO_Item where sosysno=" + soSysNo + " and BaseProductType=" + (int)AppEnum.ProductType.Service;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (Util.HasMoreRow(ds))
            {
                sql = "update so_master set HasServiceProduct=" + (int)AppEnum.YNStatus.Yes + " where sysno=" + soSysNo;
            }
            else
            {
                sql = "update so_master set HasServiceProduct=" + (int)AppEnum.YNStatus.No + " where sysno=" + soSysNo;
            }

            return SqlHelper.ExecuteNonQuery(sql);
        }

        private void map(SOServiceInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.SOSysNo = Util.TrimIntNull(tempdr["SOSysNo"]);
            oParam.ServiceAddress = Util.TrimNull(tempdr["ServiceAddress"]);
            oParam.ServiceReceiveName = Util.TrimNull(tempdr["ServiceReceiveName"]);
            oParam.ServicePhone = Util.TrimNull(tempdr["ServicePhone"]);
            oParam.ServiceExpectTime = Util.TrimNull(tempdr["ServiceExpectTime"]);
            oParam.ServiceMemo = Util.TrimNull(tempdr["ServiceMemo"]);
            oParam.ServiceAgreedTime = Util.TrimNull(tempdr["ServiceAgreedTime"]);
            oParam.ServiceActualTime = Util.TrimNull(tempdr["ServiceActualTime"]);
            oParam.ServiceQuality1 = Util.TrimIntNull(tempdr["ServiceQuality1"]);
            oParam.ServiceQuality2 = Util.TrimIntNull(tempdr["ServiceQuality2"]);
            oParam.ServiceQuality3 = Util.TrimIntNull(tempdr["ServiceQuality3"]);
            oParam.ServiceEvaluation1 = Util.TrimNull(tempdr["ServiceEvaluation1"]);
            oParam.ServiceEvaluation2 = Util.TrimNull(tempdr["ServiceEvaluation2"]);
            oParam.ServiceEvaluation3 = Util.TrimNull(tempdr["ServiceEvaluation3"]);
        }

        public SOServiceInfo LoadSOService(int SOSysNo)
        {
            string sql = "select * from so_service where sosysno =" + SOSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            SOServiceInfo oInfo = new SOServiceInfo();
            if (Util.HasMoreRow(ds))
                map(oInfo, ds.Tables[0].Rows[0]);
            else
                oInfo = null;
            return oInfo;
        }

        public SOItemPOInfo LoadSOItemPO(int SOItemSysNo, int POSysNo, int ProductIDSysNo)
        {
            string sql = "select * from SO_Item_PO where soitemsysno= " + SOItemSysNo + " and posysno = " + POSysNo + " and productidsysno=" + ProductIDSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            SOItemPOInfo oInfo = new SOItemPOInfo();
            if (Util.HasMoreRow(ds))
                map(oInfo, ds.Tables[0].Rows[0]);
            else
                oInfo = null;
            return oInfo;
        }

        public Hashtable LoadSOItemPOList(int SOItemSysNo)
        {
            string sql = "select * from SO_Item_PO where soitemsysno= " + SOItemSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                Hashtable ht = new Hashtable();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    SOItemPOInfo oInfo = new SOItemPOInfo();
                    map(oInfo, dr);
                    ht.Add(oInfo,null);
                }
                return ht;
            }
            else
                return null;
        }

	    public int InsertSOItemPO(SOItemPOInfo oParam)
        {
            return new SODac().InsertSOItemPO(oParam);
        }

        public int UpdateSOItemPO(SOItemPOInfo oParam)
        {
            return new SODac().UpdateSOItemPO(oParam);
        }

        private void map(SOItemPOInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.SOItemSysNo = Util.TrimIntNull(tempdr["SOItemSysNo"]);
            oParam.POSysNo = Util.TrimIntNull(tempdr["POSysNo"]);
            oParam.ProductIDSysNo = Util.TrimIntNull(tempdr["ProductIDSysNo"]);
        }

        public void InsertSOItemPOs(string[] ItemPOs)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                string soItemSysNoList = "";
                foreach (string ItemPO in ItemPOs)
                {
                    int indexComma = ItemPO.IndexOf(",");
                    soItemSysNoList += Int32.Parse(ItemPO.Substring(0, indexComma)) + ",";
                }

                if(soItemSysNoList.Length > 0)
                {
                    soItemSysNoList = soItemSysNoList.Substring(0, soItemSysNoList.Length - 1);
                    string sql = "delete from so_item_po where soitemsysno in(" + soItemSysNoList + ")";
                    SqlHelper.ExecuteNonQuery(sql);
                }

                foreach (string ItemPO in ItemPOs)
                {
                    int indexComma = ItemPO.IndexOf(",");
                    int lastIndexComma = ItemPO.LastIndexOf(",");
                    SOItemPOInfo oInfo = new SOItemPOInfo();
                    oInfo.SOItemSysNo = Int32.Parse(ItemPO.Substring(0, indexComma));
                    oInfo.POSysNo = Int32.Parse(ItemPO.Substring(indexComma + 1,(lastIndexComma-indexComma - 1)));
                    oInfo.ProductIDSysNo = Int32.Parse(ItemPO.Substring(lastIndexComma + 1));

                    if (LoadSOItemPO(oInfo.SOItemSysNo, oInfo.POSysNo, oInfo.ProductIDSysNo) == null)
                    {
                        this.InsertSOItemPO(oInfo);
                    }
                }
                scope.Complete();
            }
        }

        public DataSet GetSODetail(int SOSysNo)
        {
            string sql = @"SELECT * FROM [SO_Master] INNER JOIN [SO_Item] ON [SO_Master].sysno = [SO_Item].[SOSysNo] 
                           INNER JOIN [Product] ON [SO_Item].[ProductSysNo]=[Product].sysno 
                           LEFT OUTER JOIN SO_Item_PO ON [SO_Item].[SysNo] = [SO_Item_PO].soitemsysno 
                           LEFT OUTER JOIN [PO] ON [SO_Item_PO].posysno = po.sysno inner join Vendor on PO.vendorsysno = vendor.sysno 
                           WHERE [SO_Master].[SysNo] = ";
            sql += SOSysNo.ToString();
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;
            return ds;
        }

        public int InsertSOAdways(SOAdwaysInfo oParam)
        {
            return new SOAdwaysDac().Insert(oParam);
        }

        public int UpdateSOAdways(SOAdwaysInfo oParam)
        {
            return new SOAdwaysDac().Update(oParam);
        }

        private void map(SOAdwaysInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.CustomerSysNo = Util.TrimIntNull(tempdr["CustomerSysNo"]);
            oParam.AdwaysID = Util.TrimNull(tempdr["AdwaysID"]);
            oParam.AdwaysEmail = Util.TrimNull(tempdr["AdwaysEmail"]);
            oParam.SOSysNo = Util.TrimIntNull(tempdr["SOSysNo"]);
            oParam.ShipPrice = Util.TrimDecimalNull(tempdr["ShipPrice"]);
        }

        public SOAdwaysInfo LoadSOAdwaysBySOSysNo(int SOSysNo)
        {
            string sql = @"select * from so_adways where sosysno=" + SOSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            SOAdwaysInfo oInfo = new SOAdwaysInfo();
            if (Util.HasMoreRow(ds))
                map(oInfo, ds.Tables[0].Rows[0]);
            else
                oInfo = null;
            return oInfo;
        }

        public SOAdwaysInfo LoadSOAdways(int CustomerSysNo)
        {
            string sql = @"select top 1 * from so_adways where customersysno=" + CustomerSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            SOAdwaysInfo oInfo = new SOAdwaysInfo();
            if (Util.HasMoreRow(ds))
                map(oInfo, ds.Tables[0].Rows[0]);
            else
                oInfo = null;
            return oInfo;
        }

        public SOAdwaysInfo LoadSOAdways(string AdwaysID)
        {
            string sql = @"select top 1 * from so_adways where adwaysid=" + Util.ToSqlString(AdwaysID);
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            SOAdwaysInfo oInfo = new SOAdwaysInfo();
            if (Util.HasMoreRow(ds))
                map(oInfo, ds.Tables[0].Rows[0]);
            else
                oInfo = null;
            return oInfo;
        }

        //当天已出库和所有未出库的订单
        public string GetSORecent(int CustomerSysNo)
        {
            string sql = "select sysno,soid,soamt,ReceiveName,";
            sql += "case when status=" + (int) AppEnum.SOStatus.Origin + " then '" + AppEnum.GetSOStatus((int) AppEnum.SOStatus.Origin) +"'";
            sql += "     when status=" + (int)AppEnum.SOStatus.WaitingOutStock + " then '" + AppEnum.GetSOStatus((int)AppEnum.SOStatus.WaitingOutStock) + "'";
            sql += "     when status=" + (int)AppEnum.SOStatus.WaitingPay + " then '" + AppEnum.GetSOStatus((int)AppEnum.SOStatus.WaitingPay) + "'";
            sql += "     when status=" + (int)AppEnum.SOStatus.WaitingManagerAudit + " then '" + AppEnum.GetSOStatus((int)AppEnum.SOStatus.WaitingManagerAudit) + "'";
            sql += "     when status=" + (int)AppEnum.SOStatus.OutStock + " then '" + AppEnum.GetSOStatus((int)AppEnum.SOStatus.OutStock) + "'";
            sql += " end as statusname from so_master where customersysno=@customersysno ";
            sql += " and ( (status in(@status1) )  or (status=@status2 and outtime > @today) ) order by ReceiveName";

            sql = sql.Replace("@customersysno", CustomerSysNo.ToString());
            string status1 = ((int) AppEnum.SOStatus.Origin).ToString() + "," +
                            ((int) AppEnum.SOStatus.WaitingOutStock).ToString() + "," +
                            ((int) AppEnum.SOStatus.WaitingPay).ToString() + "," +
                            ((int) AppEnum.SOStatus.WaitingManagerAudit).ToString();
            string status2 = ((int) AppEnum.SOStatus.OutStock).ToString();
            sql = sql.Replace("@status1", status1);
            sql = sql.Replace("@status2", status2);
            sql = sql.Replace("@today", Util.ToSqlString(DateTime.Now.ToString(AppConst.DateFormat)));

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if(Util.HasMoreRow(ds))
            {
                string returnString = "";
                foreach(DataRow dr in ds.Tables[0].Rows)
                {
                    returnString += "<img src='../images_dbc/list_header.gif'/>"+Util.TrimNull(dr["receivename"]);
                    returnString += " , <a href=\"javascript:openWindowL('SODetail_DBC.aspx?SysNo=" + dr["SysNo"].ToString() + "')\">" + dr["SOID"] + "</a>";
                    returnString += " , " + Util.TrimNull(dr["statusname"]);
                    returnString += " , " + Util.ToMoney(dr["soamt"].ToString()) + "<br>";
                }
                return returnString;
            }
            else
            {
                return "";
            }
        }

        public int GetCustomerUnOutstockSOReceiveArea(int customerSysNo)
        {
            string sql = "select top 1 ReceiveAreaSysNo from so_master where customersysno=" + customerSysNo + " and status in(@status) order by sysno desc";
            string status = ((int)AppEnum.SOStatus.Origin).ToString() + "," +
                            ((int)AppEnum.SOStatus.WaitingOutStock).ToString() + "," +
                            ((int)AppEnum.SOStatus.WaitingPay).ToString() + "," +
                            ((int)AppEnum.SOStatus.WaitingManagerAudit).ToString();
            sql = sql.Replace("@status", status);
            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (Util.HasMoreRow(ds))
                return Util.TrimIntNull(ds.Tables[0].Rows[0][0].ToString());
            else
                return AppConst.IntNull;
        }

        private decimal CalcShipPriceTemp(SOInfo soInfo)
        {
            return Util.ToMoney(ASPManager.GetInstance().GetShipPrice(soInfo.GetTotalWeight(), soInfo.SOAmt, soInfo.ShipTypeSysNo, soInfo.ReceiveAreaSysNo));
        }

        public void CalcShipPriceForAdways(int from, int to)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                StringBuilder sb = new StringBuilder();
                string sql = "select sosysno from so_adways where sysno>=@from and sysno<=@to order by sosysno";
                sql = sql.Replace("@from", from.ToString()).Replace("@to", to.ToString());
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                foreach(DataRow dr in ds.Tables[0].Rows)
                {
                    int sosysno = Util.TrimIntNull(dr["sosysno"]);
                    SOInfo so = LoadSO(sosysno);
                    decimal shipprice = CalcShipPriceTemp(so);
                    decimal SOAmt = so.SOAmt;
                    int TotalWeight = so.GetTotalWeight();

                    string c3names = "";
                    foreach (SOItemInfo soItem in so.ItemHash.Values)
                    {
                        sql = "select c3name from product inner join category3 on product.c3sysno = category3.sysno where product.sysno = " + soItem.ProductSysNo;
                        c3names += SqlHelper.ExecuteScalar(sql).ToString() + ",";
                    }
                    c3names = c3names.Substring(0, c3names.Length - 1);

                    sb.Append("insert into so_shipprice(sosysno,shipprice,totalamt,totalweight,c3names) values('" + sosysno + "','" + shipprice + "','"+ SOAmt +"','"+ TotalWeight +"','"+ c3names +"');");
                }
                SqlHelper.ExecuteNonQuery(sb.ToString());
                scope.Complete();
            }
        }

        public void CalcShipPriceAll(int from, int to)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                StringBuilder sb = new StringBuilder();
                string sql = "select top 1000 sysno as sosysno from so_master where sysno>=@from order by sysno";
                sql = sql.Replace("@from", from.ToString()).Replace("@to", to.ToString());
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int sosysno = Util.TrimIntNull(dr["sosysno"]);
                    SOInfo so = LoadSO(sosysno);
                    if (so != null)
                    {
                        decimal shipprice = CalcShipPriceTemp(so);
                        decimal SOAmt = so.SOAmt;
                        int TotalWeight = 0;//so.GetTotalWeight();

                        sb.Append("insert into so_shipprice2(sosysno,shipprice,totalamt,totalweight) values('" + sosysno +"','" + shipprice + "','" + SOAmt + "','" + TotalWeight + "');");
                    }
                }
                SqlHelper.ExecuteNonQuery(sb.ToString());
                scope.Complete();
            }
        }

        //支付宝交易号
        #region 支付宝交易号
        public int InsertSOAlipay(SOAlipayInfo oParam)
        {
            return new SOAlipayDac().Insert(oParam);
        }

        public int UpdateSOAlipay(SOAlipayInfo oParam)
        {
            return new SOAlipayDac().Update(oParam);
        }

        public SOAlipayInfo LoadSOAlipay(int SOSysNo)
        {
            string sql = "select * from so_alipay where sosysno=" + SOSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            SOAlipayInfo oInfo = new SOAlipayInfo();
            if (Util.HasMoreRow(ds))
                map(oInfo, ds.Tables[0].Rows[0]);
            else
                oInfo = null;
            return oInfo;
        }

	    private void map(SOAlipayInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.SOSysNo = Util.TrimIntNull(tempdr["SOSysNo"]);
            oParam.AlipayTradeNo = Util.TrimNull(tempdr["AlipayTradeNo"]);
        }
        #endregion

        public void AutoCreateNullSO()
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                Random r = new Random();
                for(int i = 0;i < r.Next(1,AppConfig.AutoCreateSOCount);i++)
                {
                    SequenceDac.GetInstance().Create("SO_Sequence");
                }
                scope.Complete();
            }
        }
        
        private void map(SOWeightInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.SOSysNo = Util.TrimIntNull(tempdr["SOSysNo"]);
            oParam.Weight = Util.TrimDecimalNull(tempdr["Weight"]);
            oParam.ShipPriceCustomer = Util.TrimDecimalNull(tempdr["ShipPriceCustomer"]);
            oParam.ShipPriceVendor = Util.TrimDecimalNull(tempdr["ShipPriceVendor"]);
            oParam.CreateUserSysNo = Util.TrimIntNull(tempdr["CreateUserSysNo"]);
            oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
        }

        public SOWeightInfo LoadSOWeight(int SOSysNo)
        {
            string sql = "select * from so_weight where sosysno=" + SOSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                SOWeightInfo oInfo = new SOWeightInfo();
                map(oInfo, ds.Tables[0].Rows[0]);
                return oInfo;
            }
            else
                return null;
        }

        public void SOWeightImportToDB(ArrayList al)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                string SOSysNoList = "";
                for (int i = 0; i < al.Count; i++)
                {
                    SOWeightInfo oInfo = (SOWeightInfo)al[i];
                    SOSysNoList += oInfo.SOSysNo + ",";
                }

                SOSysNoList = SOSysNoList.Substring(0, SOSysNoList.Length - 1);
                int invalidstatus = (int)AppEnum.BiStatus.InValid;
                string sql = "update so_weight set status=" + invalidstatus + " where sosysno in(" + SOSysNoList + ")";
                SqlHelper.ExecuteNonQuery(sql);

                for (int i = 0; i < al.Count; i++)
                {
                    SOWeightInfo oInfo = (SOWeightInfo)al[i];
                    new SOWeightDac().Insert(oInfo);
                }
                scope.Complete();
            }
        }

        public DataSet GetWeightList(Hashtable paramhash)
        {
            string sql = @"select sw.sysno,sw.weight,sw.status as status,shippricecustomer,shippricevendor,st.shiptypename as shiptypename,su.username as username,sm.status as sostatus,sm.soid as soid,sm.ShipPrice as shipprice,sm.sysno as sosysno,sw.createtime as createtime,
                            sum(si.Quantity*si.weight) as SOTotalWeight
                           from so_weight sw 
                           left join sys_user su on  sw.createusersysno=su.sysno 
                           left join so_master sm on sw.sosysno=sm.sysno
                           left join shiptype st on st.sysno=sm.shiptypesysno
                           left join so_item si on sm.sysno=si.sosysno 
                           where 1=1";
            if (paramhash.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string key in paramhash.Keys)
                {
                    object item = paramhash[key];
                    sb.Append(" and ");

                    if (key == "StartDate")
                    { 
                        sb.Append("sw.CreateTime>=").Append(Util.ToSqlString(item.ToString())); 
                    }
                    else if (key == "EndDate")
                    {
                        sb.Append("sw.CreateTime<=").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "SoID")
                    {
                        sb.Append("sm.soid like").Append(Util.ToSqlLikeString(item.ToString()));
                    }
                    else if (key == "ShipTypeSysNo")
                    {
                        sb.Append("sm.shiptypesysno").Append("=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "Weight")
                    {
                        sb.Append(key).Append(item.ToString());
                    }
                    else if (key == "StatusSysNo")
                    {
                        sb.Append("sw.status").Append("=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "SOStatusSysNo")
                    {
                        sb.Append("sm.status").Append("=").Append(Util.ToSqlString(item.ToString()));
                    }
                    
                }
                sql += sb;

            }
            else
                sql.Replace("select", "select top 50");
            sql += "group by sm.sysno,sw.weight,shippricecustomer,shippricevendor,st.shiptypename,su.username,sm.status,sm.soid, sm.ShipPrice,sw.sysno,sw.status,sw.createtime";
            sql += " order by sw.sysno desc";
            return SqlHelper.ExecuteDataSet(sql);
 
        }

        public DataSet GetSOProductIDSysNoList(Hashtable paramhash)
        {
            string sql = @"select SO_Item_PO.ProductIDSysno, SO_Item.SoSysno,SO_Item.ProductSysno,SO_Item_PO.POSysNo
                           from SO_Item 
                           left join SO_Item_PO on SO_Item.sysno = SO_Item_PO.SOItemSysno
                           where 1=1";
            if (paramhash.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string key in paramhash.Keys)
                {
                    object item = paramhash[key];
                    sb.Append(" and ");

                    if (key == "ProductIDSysno")
                    {
                        sb.Append("ProductIDSysno=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if(key == "SOItemSysNo")
                    {
                        sb.Append("SOItemSysNo=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "SOSysNo")
                    {
                        sb.Append("SO_Item.SOSysNo=").Append(Util.ToSqlString(item.ToString()));
                    }
                }
                sql += sb;
            }
            
            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetProductSOList(Hashtable paramhash)
        {
            string sql = @"select SO_Item_PO.ProductIDSysno, SO_Item.SoSysno,SO_Item.ProductSysno
                           from SO_Item_PO 
                           left join SO_Item on SO_Item.sysno = SO_Item_PO.SOItemSysno
                           where 1=1";
            if (paramhash.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string key in paramhash.Keys)
                {
                    object item = paramhash[key];
                    sb.Append(" and ");

                    if (key == "ProductIDSysno")
                    {
                        sb.Append("ProductIDSysno=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "SoID")
                    {
                        sb.Append("SO_Item.SoSysno=").Append(Util.ToSqlString(item.ToString()));
                    }
                }
                sql += sb;
            }

            return SqlHelper.ExecuteDataSet(sql);
        }

        public int UpdateShipPrice()
        {
            string sql = @"select top 500 sw.*,sm.shiptypesysno as shiptypesysno,sototalweight,sm.ReceiveAreaSysNo
                           from so_weight sw
                           inner join so_master sm on sw.sosysno=sm.sysno
                           inner join
                           (select 
                                 sosysno,sum(si.Quantity*si.weight) as SOTotalWeight
                             from
                                 so_master sm
                            inner join so_item si on sm.sysno=si.sosysno
                            group by sosysno
                            ) as a on sw.sosysno=a.sosysno
                           where (shippriceCustomer=0 and shippricevendor=0) and sw.status=0 order by createtime";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            int count = 0;

            if (!Util.HasMoreRow(ds))
                throw new BizException("没有要更新运费的记录！");
            else
            {
                TransactionOptions options = new TransactionOptions();
                options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                options.Timeout = TransactionManager.DefaultTimeout;
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        decimal ShipPriceCustomer = 0;
                        decimal ShipPriceVendor = 0;
                        ShipPriceCustomer = ASPManager.GetInstance().GetShipPrice(Util.TrimDecimalNull(dr["sototalweight"].ToString()), Util.TrimIntNull(dr["shiptypesysno"].ToString()), Util.TrimIntNull(dr["ReceiveAreaSysNo"].ToString()));
                        if (Util.TrimIntNull(dr["shiptypesysno"].ToString()) == 005)//申通快递
                        {
                            ShipPriceVendor = ASPManager.GetInstance().GetShipPrice(Util.TrimDecimalNull(dr["weight"].ToString()) * 1000, 014, Util.TrimIntNull(dr["ReceiveAreaSysNo"].ToString()));
                        }
                        else if (Util.TrimIntNull(dr["shiptypeSysNo"].ToString()) == 007)//圆通快递
                        {
                            if (Util.TrimDecimalNull(dr["weight"].ToString()) <1) //小于等于1KG以圆通资料价格计算
                            {
                                ShipPriceVendor = ASPManager.GetInstance().GetShipPrice(Util.TrimDecimalNull(dr["weight"].ToString()) * 1000, 016, Util.TrimIntNull(dr["ReceiveAreaSysNo"].ToString()));
                            }
                            else //大于1KG以圆通物品价格计算
                            {
                                ShipPriceVendor = ASPManager.GetInstance().GetShipPrice(Util.TrimDecimalNull(dr["weight"].ToString()) * 1000, 015, Util.TrimIntNull(dr["ReceiveAreaSysNo"].ToString()));
                            }
                        }
                        else if (Util.TrimIntNull(dr["shiptypeSysNo"].ToString())==003)//邮局普包
                        {
                            ShipPriceVendor = ASPManager.GetInstance().GetShipPrice(Util.TrimDecimalNull(dr["weight"].ToString()) * 1000, 003, Util.TrimIntNull(dr["ReceiveAreaSysNo"].ToString()));
                        }
                        else if (Util.TrimIntNull(dr["shiptypeSysNo"].ToString()) == 002)//邮局EMS
                        {
                            ShipPriceVendor = ASPManager.GetInstance().GetShipPrice(Util.TrimDecimalNull(dr["weight"].ToString()) * 1000, 002, Util.TrimIntNull(dr["ReceiveAreaSysNo"].ToString()))*Util.TrimDecimalNull(0.57);
                        }
                        else if (Util.TrimIntNull(dr["shiptypeSysNo"].ToString()) == 012)//顺风快递
                        {
                            ShipPriceVendor = ASPManager.GetInstance().GetShipPrice(Util.TrimDecimalNull(dr["weight"].ToString()) * 1000, 012, Util.TrimIntNull(dr["ReceiveAreaSysNo"].ToString()));
                        }
                        else if (Util.TrimIntNull(dr["shiptypeSysNo"].ToString()) == 001)//ORS商城快递
                        {
                            AreaInfo oaInfo = ASPManager.GetInstance().LoadArea(Util.TrimIntNull(dr["ReceiveAreaSysNo"]));
                            if (oaInfo.LocalCode == 1)
                            {
                                ShipPriceVendor = Util.TrimDecimalNull(4);
                            }
                            else if (oaInfo.LocalCode == 2)
                            {
                                ShipPriceVendor = Util.TrimDecimalNull(5); 
                            }
                            else if (oaInfo.LocalCode == 3)
                            {
                                ShipPriceVendor = Util.TrimDecimalNull(5.5); 
                            }
                            else if (oaInfo.LocalCode == 4)
                            {
                                ShipPriceVendor = Util.TrimDecimalNull(6.5);
                            }
                            else if (oaInfo.LocalCode == 5||oaInfo.LocalCode==6)
                            {
                                ShipPriceVendor = Util.TrimDecimalNull(15); 
                            }
                        }
                        //自提
                        else if (Util.TrimIntNull(dr["shiptypeSysNo"].ToString()) == 008 || Util.TrimIntNull(dr["shiptypeSysNo"].ToString()) == 011 || Util.TrimIntNull(dr["shiptypeSysNo"].ToString()) == 009 || Util.TrimIntNull(dr["shiptypeSysNo"].ToString()) == 010 || Util.TrimIntNull(dr["shiptypeSysNo"].ToString())==013)
                        {
                            ShipPriceVendor = 0;
                        }
                        string sql2 = "update so_weight set ShipPriceCustomer=" + ShipPriceCustomer + ",ShipPriceVendor=" + ShipPriceVendor + " where sysno=" + Util.TrimIntNull(dr["sysno"]);
                        SqlHelper.ExecuteNonQuery(sql2);
                        count++;
                    }
                    scope.Complete();
                }

            }
            return count;
        }

        public int UpdateLatestShipPrice()
        {
            string sql = @"select sw.*,sm.shiptypesysno as shiptypesysno,sototalweight,sm.ReceiveAreaSysNo
                           from so_weight sw
                           inner join so_master sm on sw.sosysno=sm.sysno
                           inner join
                           (select 
                                 sosysno,sum(si.Quantity*si.weight) as SOTotalWeight
                             from
                                 so_master sm
                            inner join so_item si on sm.sysno=si.sosysno
                            group by sosysno
                            ) as a on sw.sosysno=a.sosysno
                           where shippriceCustomer=0 and shippricevendor=0 and createtime in (select top 1 createtime from so_weight order by createtime desc) and sw.status=0";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            int count = 0;

            if (!Util.HasMoreRow(ds))
                throw new BizException("没有要更新运费的记录！");
            else
            {
                TransactionOptions options = new TransactionOptions();
                options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                options.Timeout = TransactionManager.DefaultTimeout;
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        decimal ShipPriceCustomer = 0;
                        decimal ShipPriceVendor = 0;
                        ShipPriceCustomer = ASPManager.GetInstance().GetShipPrice(Util.TrimDecimalNull(dr["sototalweight"].ToString()), Util.TrimIntNull(dr["shiptypesysno"].ToString()), Util.TrimIntNull(dr["ReceiveAreaSysNo"].ToString()));
                        if (Util.TrimIntNull(dr["shiptypesysno"].ToString()) == 005)//申通快递
                        {
                            ShipPriceVendor = ASPManager.GetInstance().GetShipPrice(Util.TrimDecimalNull(dr["weight"].ToString()) * 1000, 014, Util.TrimIntNull(dr["ReceiveAreaSysNo"].ToString()));
                        }
                        else if (Util.TrimIntNull(dr["shiptypeSysNo"].ToString()) == 007)//圆通快递
                        {
                            ShipPriceVendor = ASPManager.GetInstance().GetShipPrice(Util.TrimDecimalNull(dr["weight"].ToString()) * 1000, 015, Util.TrimIntNull(dr["ReceiveAreaSysNo"].ToString()));
                        }
                        else if (Util.TrimIntNull(dr["shiptypeSysNo"].ToString()) == 003)//邮局普包
                        {
                            ShipPriceVendor = ASPManager.GetInstance().GetShipPrice(Util.TrimDecimalNull(dr["weight"].ToString()) * 1000, 003, Util.TrimIntNull(dr["ReceiveAreaSysNo"].ToString()));
                        }
                        else if (Util.TrimIntNull(dr["shiptypeSysNo"].ToString()) == 002)//邮局EMS
                        {
                            ShipPriceVendor = ASPManager.GetInstance().GetShipPrice(Util.TrimDecimalNull(dr["weight"].ToString()) * 1000, 002, Util.TrimIntNull(dr["ReceiveAreaSysNo"].ToString())) * Util.TrimDecimalNull(0.57);
                        }
                        else if (Util.TrimIntNull(dr["shiptypeSysNo"].ToString()) == 012)//顺风快递
                        {
                            ShipPriceVendor = ASPManager.GetInstance().GetShipPrice(Util.TrimDecimalNull(dr["weight"].ToString()) * 1000, 012, Util.TrimIntNull(dr["ReceiveAreaSysNo"].ToString()));
                        }
                        else if (Util.TrimIntNull(dr["shiptypeSysNo"].ToString()) == 001)//ORS商城快递
                        {
                            AreaInfo oaInfo = ASPManager.GetInstance().LoadArea(Util.TrimIntNull(dr["ReceiveAreaSysNo"]));
                            if (oaInfo.LocalCode == 1)
                            {
                                ShipPriceVendor = Util.TrimDecimalNull(4);
                            }
                            else if (oaInfo.LocalCode == 2)
                            {
                                ShipPriceVendor = Util.TrimDecimalNull(5);
                            }
                            else if (oaInfo.LocalCode == 3)
                            {
                                ShipPriceVendor = Util.TrimDecimalNull(5.5);
                            }
                            else if (oaInfo.LocalCode == 4)
                            {
                                ShipPriceVendor = Util.TrimDecimalNull(6.5);
                            }
                            else if (oaInfo.LocalCode == 5 || oaInfo.LocalCode == 6)
                            {
                                ShipPriceVendor = Util.TrimDecimalNull(15);
                            }
                        }
                        //自提
                        else if (Util.TrimIntNull(dr["shiptypeSysNo"].ToString()) == 008 || Util.TrimIntNull(dr["shiptypeSysNo"].ToString()) == 011 || Util.TrimIntNull(dr["shiptypeSysNo"].ToString()) == 009 || Util.TrimIntNull(dr["shiptypeSysNo"].ToString()) == 010 || Util.TrimIntNull(dr["shiptypeSysNo"].ToString()) == 013)
                        {
                            ShipPriceVendor = 0;
                        }
                        string sql2 = "update so_weight set ShipPriceCustomer=" + ShipPriceCustomer + ",ShipPriceVendor=" + ShipPriceVendor + " where sysno=" + Util.TrimIntNull(dr["sysno"]);
                        SqlHelper.ExecuteNonQuery(sql2);
                        count++;
                    }
                    scope.Complete();
                }

            }
            return count;
        }
        
        public int GetSOSysNobyDONo(string DONo)
        {
            string sql = @"select sosysno from do_master where dono=" + Util.ToSqlString(DONo.ToString());
            DataSet ds=SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return AppConst.IntNull;
            else if (ds.Tables[0].Rows.Count > 1)
            {
                throw new BizException(DONo + "对应多个订单号");
            }

            else
                return (int)ds.Tables[0].Rows[0][0];
        }

        public DataSet GetProductTrackSNList(Hashtable paramHash)
        {
            string sql = @"select sm.sysno as sosysno,sm.soid,p.productname,sm.receivename,sm.receivephone,sm.receivecellphone,
                            sm.outtime,pi.sysno as productidsysno,pi.producttracksn,su.username as auditusername,pi.note
                             from so_master sm(nolock) 
                            inner join so_item si(nolock) on sm.sysno=si.sosysno 
                            inner join sys_user su(nolock) on su.sysno=sm.auditusersysno
                            inner join product p(nolock) on si.productsysno=p.sysno 
                            inner join so_item_po sp(nolock) on si.sysno=sp.soitemsysno 
                            inner join product_id pi(nolock) on sp.productidsysno=pi.sysno 
                            inner join tracksn_base tb(nolock) on tb.c3sysno=p.c3sysno and tb.manufacturersysno=p.manufacturersysno 
                            where sm.status=@sostatus and tb.status=@tbstatus ";

            sql = sql.Replace("@sostatus", ((int)AppEnum.SOStatus.OutStock).ToString());
            //sql = sql.Replace("@pistatus", ((int)AppEnum.BiStatus.Valid).ToString());
            sql = sql.Replace("@tbstatus", ((int)AppEnum.BiStatus.Valid).ToString());

            bool SelectNullTrackSN = false;
            bool NoEndDate = true;

            if (paramHash.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string key in paramHash.Keys)
                {
                    object item = paramHash[key];
                    sb.Append(" and ");
                    if (key == "ProductID")
                        sb.Append("p.productid").Append("=").Append(Util.ToSqlString(item.ToString()));
                    else if (key == "ReceiveName")
                        sb.Append("sm.receivename").Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                    else if (key == "ReceivePhone")
                        sb.Append("sm.receivephone").Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                    else if (key == "StartDate")
                        sb.Append("sm.outtime").Append(">=").Append(Util.ToSqlString(item.ToString()));
                    else if (key == "EndDate")
                    {
                        sb.Append("sm.outtime").Append("<=").Append(Util.ToSqlEndDate(item.ToString()));
                        NoEndDate = false;
                    }
                    else if (key == "AuditUserSysNo")
                        sb.Append("sm.AuditUserSysNo").Append("=").Append(item.ToString());
                    else if (key == "HasTrackSN")
                    {
                        if (item.ToString() == ((int)AppEnum.YNStatus.Yes).ToString())
                        {
                            sb.Append("pi.ProductTrackSN is not null");
                        }
                        else
                        {
                            sb.Append("pi.ProductTrackSN is null");
                            SelectNullTrackSN = true;
                        }
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

            if (NoEndDate && SelectNullTrackSN)  //不设置出库截止日期并且选择未跟踪的
            {
                //ORS商城快递、普通快递 出库3天，EMS(shiptypesysno=2) 出库5天，邮政普包(shiptypesysno=3) 出库 7天
                sql += " and sm.outtime < ( case when shiptypesysno=2 then '" + DateTime.Now.AddDays(-5).ToString(AppConst.DateFormat) + "' when shiptypesysno=3 then  '" + DateTime.Now.AddDays(-7).ToString(AppConst.DateFormat) + "' else  '" + DateTime.Now.AddDays(-3).ToString(AppConst.DateFormat) + "' end)";
            }

            sql += " order by sm.outtime desc";
            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetMasertProductDs(Hashtable paramHash)
        {
            string sql = @"select Product.sysno,productid as MasterProductID,productname as MasterProductName
                        from Product
                        inner join category3 on category3.sysno=Product.c3sysno
                        inner join category2 on category3.c2sysno = category2.sysno 
						inner join category1 on category2.c1sysno = category1.sysno
                        where 1=1 @Status @MasterProductSysNo @Category @C3 @KeyWords
                      ";
            sql = sql.Replace("@Status", " and product.status=" + ((int)AppEnum.ProductStatus.Show).ToString());
            if (paramHash.ContainsKey("MasterProductSysNo"))
            {
                sql = sql.Replace("@MasterProductSysNo", " and Product.SysNo=" + paramHash["MasterProductSysNo"].ToString());
            }
            else
            {
                sql = sql.Replace("@MasterProductSysNo", " ");
            }
            if (paramHash.ContainsKey("Category"))
            {
                sql = sql.Replace("@Category", " and " + (string)paramHash["Category"]);
            }
            else
            {
                sql = sql.Replace("@Category", "");
            }

            if (paramHash.ContainsKey("C3"))
            {
                sql = sql.Replace("@C3", " and " + (string)paramHash["C3"]);
            }
            else
            {
                sql = sql.Replace("@C3", "");
            }
            if (paramHash.ContainsKey("KeyWords"))
            {
                string[] Keys = (Util.TrimNull(paramHash["KeyWords"].ToString())).Split(' ');
                if (Keys.Length == 1)
                {
                    sql = sql.Replace("@KeyWords", "and (productid like " + Util.ToSqlLikeString(paramHash["KeyWords"].ToString()) + " or productname like " + Util.ToSqlLikeString(paramHash["KeyWords"].ToString()) + ")");
                }
                else
                {
                    string t = "";
                    t += " and (productid like " + Util.ToSqlLikeString(paramHash["KeyWords"].ToString()) + " or ( 1=1 ";
                    for (int i = 0; i < Keys.Length; i++)
                    {
                        t += " and productname like " + Util.ToSqlLikeString(Keys[i]);
                    }
                    t += "))";
                    sql = sql.Replace("@KeyWords", t);
                }

            }
            else
            {
                sql = sql.Replace("@KeyWords", "");
            }
            sql += "order by Product.createtime desc";

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            return ds;

        }

        private void map(SOUserAuditRatioInfo oParam, DataRow tempdr)
        {
            oParam.UserSysNo = Util.TrimIntNull(tempdr["UserSysNo"]);
            oParam.Ratio = Util.TrimIntNull(tempdr["Ratio"]);
        }

        public DataSet GetSOUserAuditRatioDs(Hashtable paramHash)
        {
            string sql = @"select sua.*,su.username from SO_User_AuditRatio sua,sys_user su where sua.usersysno=su.sysno  @UserSysNo";
            if (paramHash != null && paramHash.ContainsKey("UserSysNo"))
            {
                sql = sql.Replace("@UserSysNo", "and sua.usersysno=" + Util.ToSqlString(paramHash["UserSysNo"].ToString()));
            }
            else
                sql = sql.Replace("@UserSysNo", " ");
            return SqlHelper.ExecuteDataSet(sql);
        }

        public SOUserAuditRatioInfo LoadSOUserAuditRatio(int sysNo)
        {
            string sql = "select * from SO_User_AuditRatio where sysno=" + sysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                SOUserAuditRatioInfo oInfo = new SOUserAuditRatioInfo();
                map(oInfo, ds.Tables[0].Rows[0]);
                return oInfo;
            }
            else
                return null;
        }

        public void InsertSoUserAuditRatio(SOUserAuditRatioInfo oInfo)
        {
            new SOUserAuditRatioDac().Insert(oInfo);
        }
        public void UpdateSoUserAuditRatio(SOUserAuditRatioInfo oInfo)
        {
            new SOUserAuditRatioDac().Update(oInfo);
        }
        public void DeleteSoUserAudit(SOUserAuditRatioInfo oInfo)
        {
            string sql = "delete from SO_User_AuditRatio where usersysno=" + oInfo.UserSysNo;
            SqlHelper.ExecuteDataSet(sql);
        }
        public int GetSoUserAuditTotalRatio()
        {
            string sql = "select isnull(sum(Ratio),0) as TotalRatio from SO_User_AuditRatio ";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds) && ds.Tables[0].Rows.Count > 0)
                return (int)ds.Tables[0].Rows[0][0];

            else
                return AppConst.IntNull;
        }

        public decimal GetSOReturnAmt(int sosysno)
        {
            decimal returnAmt = 0;
            string sql = @"SELECT SUM(ReturnQty * Price) AS returnamt
                        FROM SO_Item
                        WHERE SOSysNo=" + sosysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds) && ds.Tables[0].Rows.Count > 0)
            {
                returnAmt = Util.TrimDecimalNull(ds.Tables[0].Rows[0][0]);
            }
            return returnAmt;

        }

        public DataSet GetSOInvoiceList(Hashtable paramHash)
        {
            string sql = @"select sm.sysno,sm.soid,sm.outtime,sm.isvat,sm.InvoiceStatus,sm.InvoiceTime,sm.AbandonInvoiceTime,sm.RequestInvoiceType,
                            sm.InvoiceType,sm.Status, su.username as InvoiceUpdateUser,sm.RequestInvoiceTime
						   from so_master sm 
                           left join Sys_user su on su.sysno = sm.InvoiceUpdateUserSysNo 
                           where 1=1";
            StringBuilder sb = new StringBuilder();

            if (paramHash.Count > 0)
            {
                foreach (string key in paramHash.Keys)
                {
                    object item = paramHash[key];
                    sb.Append(" and ");
                    if (key == "OutTimeFrom")
                    {
                        sb.Append("sm.outtime").Append(">=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "OutTimeTo")
                    {
                        sb.Append("sm.outtime").Append("<=").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "InvoiceTimeFrom")
                    {
                        sb.Append("sm.InvoiceTime").Append(">=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "InvoiceTimeTo")
                    {
                        sb.Append("sm.InvoiceTime").Append("<=").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "IsVAT")
                    {
                        sb.Append("sm.isvat").Append(" = ").Append(item.ToString());
                    }
                    else if (key == "SOStatus")
                    {
                        sb.Append("sm.status").Append(" = ").Append(item.ToString());
                    }
                    else if (key == "InvoiceType")
                    {

                        sb.Append("sm.InvoiceType").Append(" = ").Append(item.ToString());
                    }
                    else if (key == "InvoiceStatus")
                    {
                        sb.Append(" sm.InvoiceStatus").Append(" = ").Append(item.ToString());
                    }
                    else if (key == "Compare")
                    {

                        sb.Append("sm.RequestInvoiceTime").Append(item.ToString()).Append("sm.outtime");
                    }
                    else if (key == "SOAmt")
                    {
                        sb.Append("sm.soamt").Append(item.ToString());
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
            sql += " order by sm.sysno desc";
            return SqlHelper.ExecuteDataSet(sql);
        }

        public void UpdateSOItemReturnQty(int sosysno, int productSysNo, int returnQty)
        {
            string sql = @"update So_Item Set ReturnQty=" + returnQty + " where sosysno=" + sosysno + " and productsysno=" + productSysNo;
            SqlHelper.ExecuteDataSet(sql);
        }

        public int GetSOItemQty(int sosysno)
        {
            int soItemQty = 0;
            string sql = @"select sum(Quantity) from so_item where so_item.sosysno=" + sosysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                soItemQty = Util.TrimIntNull(ds.Tables[0].Rows[0][0]);
            }
            return soItemQty;
        }

        public DataSet GetSOSizeSetList()
        {
            string sql = @"select sss.*,su.username from SO_SizeType_SetList sss 
                           left join sys_user su on su.sysno=sss.createusersysno 
                            where sss.createtime>=" + Util.ToSqlString(DateTime.Now.Date.ToShortDateString()) +
                            " and  not exists (select 1 from SO_SizeType_SetList s2 where ((s2.itemid=sss.itemid and s2.createtime> sss.createtime) or (s2.createtime=sss.createtime and s2.sysno>sss.sysno) )) order by createtime desc";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            return ds;
        }

        public void InsertSOSizeType(SOSizeTypeSetListInfo oParam)
        {
            new SOSizeTypeSetListDac().Insert(oParam);
        }

        /// <summary>
        /// 判断是否使用过每天一次的免运费活动
        /// 运费为0, 配送方式不是上海自提,状态为非作废状态
        /// </summary>
        /// <param name="CustomerSysNo"></param>
        /// <returns></returns>
        public bool HasUserdOneDayOneFreeShipFee(int soSysNo, int CustomerSysNo, DateTime OrderDate)
        {
            string sql = @"select count(sysno) from @so_master 
                           where customersysno=@customerSysNo 
                                 and shipprice=0 and shiptypesysno <> 8 and status not in(-1,-2,-3) and convert(nvarchar(10), orderdate, 120)=@orderdate ";
            sql = sql.Replace("@customerSysNo", CustomerSysNo.ToString()).Replace("@orderdate", Util.ToSqlString(OrderDate.ToString(AppConst.DateFormat)));
            string viewSOMaster = "viewSOMasterCustomer_" + Convert.ToString(CustomerSysNo % 10);
            sql = sql.Replace("@so_master", viewSOMaster);
            if (soSysNo != AppConst.IntNull)
            {
                sql += " and sysno < " + soSysNo;
            }
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.TrimIntNull(ds.Tables[0].Rows[0][0]) > 0)
                return true;
            else
                return false;
        }

        //===========================
        public void UpdateSOInvoiceInfo(int sosysno, int usersysno, int InvoiceTypeSysNo)
        {
            int InvoiceStatus = (int)AppEnum.SOInvoiceStatus.InvoiceComplete;
            DateTime InvoiceTime = DateTime.Now;
            int InvoiceUpdateUserSysNo = usersysno;

            string sql = @"update SO_Master set InvoiceStatus=" + InvoiceStatus + ",InvoiceTime=" + Util.ToSqlString(InvoiceTime.ToString()) + ", InvoiceUpdateUserSysNo=" + InvoiceUpdateUserSysNo + ",InvoiceType=" + InvoiceTypeSysNo + " where sysno =" + sosysno;
            SqlHelper.ExecuteDataSet(sql);


        }

        public void AbandonSOInvoice(int sosysno, int usersysno)
        {
            int InvoiceStatus = (int)AppEnum.SOInvoiceStatus.InvoiceAbandon;
            DateTime AbandonInvoiceTime = DateTime.Now;
            int InvoiceUpdateUserSysNo = usersysno;

            string sql = @"update SO_Master set InvoiceStatus=" + InvoiceStatus + ",AbandonInvoiceTime=" + Util.ToSqlString(AbandonInvoiceTime.ToString()) + ",InvoiceUpdateUserSysNo=" + InvoiceUpdateUserSysNo + " where sysno =" + sosysno;
            SqlHelper.ExecuteDataSet(sql);

        }

        public void UpdateSOInvoiceType(int soSysNo, int InvoiceTypeSysNo)
        {
            string sql = @"update SO_Master set InvoiceType=" + InvoiceTypeSysNo + " where sysno =" + soSysNo;
            SqlHelper.ExecuteDataSet(sql);
        }

        public void UpdateSOInvoiceStatus(int soSysNo, int InvoiceStatusSysNo, int IsVAT)
        {
            string sql = @"update SO_Master set InvoiceStatus=" + InvoiceStatusSysNo + ",IsVAT=" + IsVAT + ",RequestInvoiceTime=" + Util.ToSqlString(DateTime.Now.ToString()) + " where sysno =" + soSysNo;
            SqlHelper.ExecuteDataSet(sql);
        }

        public void UpdateSORequestInvoiceType(int soSysNo, int RequestInvoiceTypeSysNo)
        {
            string sql = @"update SO_Master set IsVAT=" + (int)AppEnum.YNStatus.Yes + " ,RequestInvoiceType=" + RequestInvoiceTypeSysNo + " ,InvoiceStatus=" + (int)AppEnum.SOInvoiceStatus.InvoiceAbsent + " where sysno =" + soSysNo;
            SqlHelper.ExecuteDataSet(sql);
        }


        public bool CheckVATExist(int sosysno)
        {
            bool Result = true;
            string sql = @"select *
						   from so_valueadded_invoice
						   where sosysno =" + sosysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                Result = false;
            return Result;


        }


    }
}