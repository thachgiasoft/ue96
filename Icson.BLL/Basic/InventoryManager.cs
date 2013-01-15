using System;
using System.Collections;
using System.Data;
using System.Text;

using Icson.Utils;
using System.Transactions;

using Icson.Objects;
using Icson.Objects.Basic;
using Icson.DBAccess;
using Icson.DBAccess.Basic;

namespace Icson.BLL.Basic
{
	/// <summary>
	/// Summary description for InventoryManager.
	/// </summary>
	public class InventoryManager
	{
		private InventoryManager()
		{
		}
		private static InventoryManager _instance;
		public static InventoryManager GetInstance()
		{
			if ( _instance == null )
			{
				_instance = new InventoryManager();
			}
			return _instance;
		}

		private string buildSql(int stockSysNo, int productSysNo, int qty, Hashtable htQty, Hashtable htCon)
		{
			StringBuilder sb = new StringBuilder(100);
			sb.Append("Update ");
			if ( stockSysNo == AppConst.IntNull )
			{
				//inventory
				sb.Append(" Inventory ");
			}
			else
			{
				//inventory stock
				sb.Append(" Inventory_Stock ");
			}
			sb.Append(" Set ");

			int i=0;
			foreach(string key in htQty.Keys)
			{
				if ( i != 0 )
					sb.Append(",");
				string val = (string)htQty[key];
				// orderqty = orderqty + qty
				// key      = key      val qty //val 和qty中间一定要加一个空格，因为两个-号就是sql的注释
				sb.Append(" ").Append(key).Append("=").Append(key).Append(val).Append(" ").Append(qty.ToString());
				i++;
			}

			sb.Append(" where productsysno = ").Append(productSysNo.ToString());
			if ( stockSysNo == AppConst.IntNull)
			{
				//inventory
			}
			else
			{
				//inventory stock
				sb.Append(" and stocksysno = ").Append(stockSysNo.ToString());
			}

			foreach(string key in htCon.Keys)
			{				
				string val = (string)htCon[key];
				// orderqty+qty>=0
				// key value qty >=0
				if ( ( val == "+" && qty<0 ) || (val == "-" && qty>0 ) ) //对应delt>0的时候不检查, 见文档
				{
					sb.Append(" and ");
					sb.Append(" ").Append(key).Append(val).Append(" ").Append(qty.ToString()).Append(">=0");
				}
			}
			return sb.ToString();
		}

		/// <summary>
		/// sale order create / cancel
		/// </summary>
		/// <param name="stockSysNo"></param>
		/// <param name="productSysNo"></param>
		/// <param name="qty"></param>
		public void SetOrderQty( int stockSysNo, int productSysNo,int qty)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				Hashtable htQty = new Hashtable(2);
				htQty.Add("OrderQty", "+");
				htQty.Add("AvailableQty", "-");

                Hashtable htCon = new Hashtable(2);
				htCon.Add("AvailableQty+VirtualQty", "-");
				htCon.Add("OrderQty", "+");

				string sql = buildSql(AppConst.IntNull, productSysNo, qty, htQty, htCon);
				if ( 1 != new InventoryDac().UpdateQty(sql))
					throw new BizException("inventory: qty is not enough");

				//stock
				htCon.Remove("AvailableQty+VirtualQty");
				string sql2 = buildSql(stockSysNo, productSysNo, qty, htQty, htCon);

				if ( 1 != new InventoryDac().UpdateQty(sql2))
					throw new BizException("inventory_stock: qty is not enough");

				scope.Complete();
            }
		}
		/// <summary>
		/// order out stock / cancel out stock
		/// </summary>
		/// <param name="stockSysNo"></param>
		/// <param name="productSysNo"></param>
		/// <param name="qty"></param>
		public void SetSOOutStockQty( int stockSysNo, int productSysNo,int qty)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				Hashtable htQty = new Hashtable(2);
				htQty.Add("OrderQty", "-");
				htQty.Add("AccountQty", "-");

				Hashtable htCon = new Hashtable(2);
				htCon.Add("OrderQty", "-");
				htCon.Add("AccountQty", "-");

				string sql = buildSql(AppConst.IntNull, productSysNo, qty, htQty, htCon);

				if ( 1 != new InventoryDac().UpdateQty(sql))  //更新总仓
					throw new BizException("inventory: qty is not enough");

				string sql2 = buildSql(stockSysNo, productSysNo, qty, htQty, htCon);

				if ( 1 != new InventoryDac().UpdateQty(sql2))  //更新分仓
					throw new BizException("inventory_stock: qty is not enough");

				scope.Complete();
            }
		}
		/// <summary>
		/// 仓库一般单据生成和作废
		/// </summary>
		/// <param name="stockSysNo"></param>
		/// <param name="productSysNo"></param>
		/// <param name="qty"></param>
		public void SetAvailableQty(int stockSysNo, int productSysNo, int qty)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				//初始化商品的库存信息
				this.InitInventory(productSysNo);

                StockInfo stock = StockManager.GetInstance().Load(stockSysNo);
                if (stock.StockType == (int)AppEnum.StockType.Normal)  //正常仓库才SetAvailableQty
                {
                    Hashtable htQty = new Hashtable(2);
                    htQty.Add("AllocatedQty", "+");
                    htQty.Add("AvailableQty", "-");

                    Hashtable htCon = new Hashtable(2);
                    htCon.Add("AllocatedQty", "+");

                    string sql = buildSql(AppConst.IntNull, productSysNo, qty, htQty, htCon);
                    //更新总仓
                    if (1 != new InventoryDac().UpdateQty(sql))
                        throw new BizException("inventory: qty is not enough");

                    //分仓增加avail的判断
                    htCon.Add("AvailableQty", "-");

                    string sql2 = buildSql(stockSysNo, productSysNo, qty, htQty, htCon);
                    if (1 != new InventoryDac().UpdateQty(sql2))
                        throw new BizException("inventory_stock: qty is not enough");
                }
				scope.Complete();
            }
		}

		/// <summary>
        /// 移库单出库和取消出库
		/// </summary>
		/// <param name="stockSysNoA"></param>
		/// <param name="stockSysNoB"></param>
		/// <param name="productSysNo"></param>
		/// <param name="qty"></param>
		public void SetShiftOutStockQty(int stockSysNoA, int stockSysNoB, int productSysNo, int qty)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				//初始化商品的库存信息
				this.InitInventory(productSysNo);

				//总仓
				//qty: 减财务库存和占用库存
				Hashtable htQty = new Hashtable(2);
				htQty.Add("AllocatedQty", "-");
				htQty.Add("AccountQty", "-");

				//con: 财务库存和占用库存>=0
				Hashtable htCon = new Hashtable(2);
				htCon.Add("AllocatedQty", "-");
				htCon.Add("AccountQty", "-");

                StockInfo stockA = StockManager.GetInstance().Load(stockSysNoA);

                if (stockA.StockType == (int)AppEnum.StockType.Normal)  //正常仓库出库，才影响总仓
                {
                    string sql = buildSql(AppConst.IntNull, productSysNo, qty, htQty, htCon);

                    if (1 != new InventoryDac().UpdateQty(sql))
                        throw new BizException("inventory: qty is not enough");
                }

				//A仓
				//qty: 加shiftout
				//con：和总仓一致
                if (stockA.StockType == (int)AppEnum.StockType.RMA)  //RMA仓库只判断和更新财务数量
                {
                    htQty.Clear();
                    htQty.Add("AccountQty", "-");

                    htCon.Clear();
                    htCon.Add("AccountQty", "-");
                }
                htQty.Add("ShiftOutQty", "+");
				string sql2 = buildSql(stockSysNoA, productSysNo, qty, htQty, htCon);
				if ( 1 != new InventoryDac().UpdateQty(sql2))
					throw new BizException("inventory_stock:商品编号-" + productSysNo.ToString() + " qty is not enough");

				//B仓
				//qty: 仅仅需要shiftin
				htQty.Clear();
				htQty.Add("ShiftInQty", "+");
				//con: 不需要
				htCon.Clear();

				string sql3 = buildSql(stockSysNoB, productSysNo, qty, htQty, htCon);
				if ( 1 != new InventoryDac().UpdateQty(sql3))
					throw new BizException("inventory_stock:商品编号-" + productSysNo.ToString() + " qty is not enough");

				scope.Complete();
            }
		}
		/// <summary>
		/// 移库单入库和取消入库
		/// </summary>
		/// <param name="stockSysNo"></param>
		/// <param name="productSysNo"></param>
		/// <param name="qty"></param>
		public void SetShiftInStockQty(int stockSysNoA, int stockSysNoB, int productSysNo, int qty)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				//初始化商品的库存信息
				this.InitInventory(productSysNo);

				//总仓
				//qty：加财务和可用
				Hashtable htQty = new Hashtable(2);
				htQty.Add("AvailableQty", "+");
				htQty.Add("AccountQty", "+");

				//con：财务库存>=0
				Hashtable htCon = new Hashtable(2);
				htCon.Add("AccountQty", "+");

                StockInfo stockB = StockManager.GetInstance().Load(stockSysNoB);  //正常仓库入库，才影响总仓
                if (stockB.StockType == (int)AppEnum.StockType.Normal)
                {
                    string sql = buildSql(AppConst.IntNull, productSysNo, qty, htQty, htCon);

                    if (1 != new InventoryDac().UpdateQty(sql))
                        throw new BizException("inventory: qty is not enough");
                }
				//B仓
				//con：增加分仓判断条件
                if (stockB.StockType == (int)AppEnum.StockType.Normal)
                {
                    htCon.Add("AvailableQty", "+"); //注:qty>0的时候，是不会判断的，详见buildSql函数
                }

                if (stockB.StockType == (int)AppEnum.StockType.RMA) //RMA仓库只更新财务数量
                {
                    htQty.Clear();
                    htQty.Add("AccountQty", "+");
                }

                //qty: 增加shifin的消减
                htQty.Add("ShiftInQty", "-");

				string sql2 = buildSql(stockSysNoB, productSysNo, qty, htQty, htCon);
				if ( 1 != new InventoryDac().UpdateQty(sql2))
					throw new BizException("inventory_stock: qty is not enough");

				//A仓
				//qty：仅仅需要shiftout 减
				htQty.Clear();
				htQty.Add("ShiftOutQty", "-");
				//con ：不需要条件
				htCon.Clear();

				string sql3 = buildSql(stockSysNoA, productSysNo, qty, htQty, htCon);
				if ( 1 != new InventoryDac().UpdateQty(sql3))
					throw new BizException("inventory_stock: qty is not enough");

				scope.Complete();
            }
		}

		/// <summary>
		/// 仓库一般单据出库和取消出库
		/// </summary>
		/// <param name="stockSysNo"></param>
		/// <param name="productSysNo"></param>
		/// <param name="qty"></param>
		public void SetOutStockQty(int stockSysNo, int productSysNo, int qty)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                StockInfo stock = StockManager.GetInstance().Load(stockSysNo);
                if (stock == null)
                    return;

				Hashtable htQty = new Hashtable(2);
				htQty.Add("AccountQty", "-");
                if (stock.StockType == (int)AppEnum.StockType.Normal) //正常仓库更新分配数量，RMA仓库就不更新了
				    htQty.Add("AllocatedQty", "-");

				Hashtable htCon = new Hashtable(2);
				htCon.Add("AccountQty", "-");
                if (stock.StockType == (int)AppEnum.StockType.Normal)  //正常仓库更新分配数量，RMA仓库就不更新了
                    htCon.Add("AllocatedQty", "-");

                if (stock.StockType == (int)AppEnum.StockType.Normal)  //正常仓库更新总仓，RMA暂时不设总仓
                {
                    string sql = buildSql(AppConst.IntNull, productSysNo, qty, htQty, htCon);

                    if (1 != new InventoryDac().UpdateQty(sql))
                        throw new BizException("inventory: qty is not enough");
                }

                string sql2 = buildSql(stockSysNo, productSysNo, qty, htQty, htCon);
				if ( 1 != new InventoryDac().UpdateQty(sql2))
					throw new BizException("inventory_stock: qty is not enough");

				scope.Complete();
            }
		}
		/// <summary>
		/// 仓库一般单据入库和取消入库
		/// </summary>
		/// <param name="stockSysNo"></param>
		/// <param name="productSysNo"></param>
		/// <param name="qty"></param>
		public void SetInStockQty(int stockSysNo, int productSysNo, int qty)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                StockInfo stock = StockManager.GetInstance().Load(stockSysNo);
                if (stock == null)
                    return;

				//初始化商品的库存信息
				this.InitInventory(productSysNo);

				Hashtable htQty = new Hashtable(2);
				htQty.Add("AccountQty", "+");

                if(stock.StockType == (int)AppEnum.StockType.Normal)   //正常仓库更新可用数量，RMA仓库就不更新了
                    htQty.Add("AvailableQty", "+");

				Hashtable htCon = new Hashtable(2);
				htCon.Add("AccountQty", "+");

                if (stock.StockType == (int)AppEnum.StockType.Normal)  //正常仓库更新总仓，RMA暂时不设总仓
                {
                    string sql = buildSql(AppConst.IntNull, productSysNo, qty, htQty, htCon);

                    if (1 != new InventoryDac().UpdateQty(sql))
                        throw new BizException("inventory: qty is not enough");
                }

                //分仓增加对avail的判断，注意：在buildSql里面如果qty>0,就不判断了。也就是说在取消入库的时候才会判断。
                if (stock.StockType == (int)AppEnum.StockType.Normal)  //正常仓库更新可用数量，RMA仓库就不更新了
                    htCon.Add("AvailableQty", "+");

				string sql2 = buildSql(stockSysNo, productSysNo, qty, htQty, htCon);
				if ( 1 != new InventoryDac().UpdateQty(sql2))
					throw new BizException("inventory_stock: qty is not enough");

				scope.Complete();
            }
		}
		/// <summary>
		/// 采购入库和取消入库
		/// </summary>
		/// <param name="stockSysNo"></param>
		/// <param name="productSysNo"></param>
		/// <param name="qty"></param>
		public void SetPOInStockQty(int stockSysNo, int productSysNo, int qty)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				//初始化商品的库存信息
				this.InitInventory(productSysNo);

				Hashtable htQty = new Hashtable(2);
				htQty.Add("AccountQty", "+");
				htQty.Add("AvailableQty", "+");
				htQty.Add("PurchaseQty", "-");

				Hashtable htCon = new Hashtable(2);
				htCon.Add("AccountQty", "+");

				string sql = buildSql(AppConst.IntNull, productSysNo, qty, htQty, htCon);

				if ( 1 != new InventoryDac().UpdateQty(sql))
					throw new BizException("inventory: qty is not enough");

				//分仓增加对avail的判断，注意：在buildSql里面如果qty>0,就不判断了。也就是说在取消入库的时候才会判断。
				htCon.Add("AvailableQty", "+");
				string sql2 = buildSql(stockSysNo, productSysNo, qty, htQty, htCon);
				if ( 1 != new InventoryDac().UpdateQty(sql2))
					throw new BizException("inventory_stock: qty is not enough");

				scope.Complete();
            }
		}
		public void SetVirtualQty(int productSysNo, int qty)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				//初始化商品的库存信息
				this.InitInventory(productSysNo);

				Hashtable htQty = new Hashtable(2);
				htQty.Add("VirtualQty", "+");

				Hashtable htCon = new Hashtable(2);

				string sql = buildSql(AppConst.IntNull, productSysNo, qty, htQty, htCon);

				if ( 1 != new InventoryDac().UpdateQty(sql))
					throw new BizException("inventory: qty is not enough");

				scope.Complete();
            }
		}

        //Add by Judy
        // --------------------------------------------------------------------
        public void SetSafeQty(int productSysNo, int qty, int StockSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                //更新安全库存

                string sql = @"Update Inventory_Stock Set SafeQty=" + qty + " where productsysno=" + productSysNo + " and stocksysno=" + StockSysNo;
                DataSet ds = SqlHelper.ExecuteDataSet(sql);

                //if (1 != new InventoryDac().UpdateQty(sql))


                scope.Complete();
            }
        }
        // --------------------------------------------------------------------

        public void SetSafeQty(int productSysNo, int qty)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                //初始化商品的库存信息

                string sql = @"Update Inventory_Stock Set SafeQty=" + qty+"where productsysno="+productSysNo;
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                
                //if (1 != new InventoryDac().UpdateQty(sql))

                scope.Complete();
            }
        }

        //Add by Judy
        public InventoryStockInfo LoadInventoryStock2(int productsysno)
        {
            string sql = "select * from Inventory_Stock where productsysno = " + productsysno;
            return LoadInventoryStock(sql);
        }

		public void SetPurchaseQty(int stockSysNo, int productSysNo, int qty)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				//初始化商品的库存信息
				this.InitInventory(productSysNo);

				Hashtable htQty = new Hashtable(2);
				htQty.Add("PurchaseQty", "+");

				Hashtable htCon = new Hashtable(2);

				string sql = buildSql(AppConst.IntNull, productSysNo, qty, htQty, htCon);

				if ( 1 != new InventoryDac().UpdateQty(sql))
					throw new BizException("inventory: qty is not enough");

				string sql2 = buildSql(stockSysNo, productSysNo, qty, htQty, htCon);
				if ( 1 != new InventoryDac().UpdateQty(sql2))
					throw new BizException("inventory_stock: qty is not enough");

				scope.Complete();
            }
		}
		/// <summary>
		/// 导入借货未还专用，因为原来借货不减财务库存，现在导数据就需要仅仅减掉财务库存
		/// </summary>
		/// <param name="stockSysNo"></param>
		/// <param name="productSysNo"></param>
		/// <param name="qty"></param>
		public void SetImportLendQty(int stockSysNo, int productSysNo, int qty)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				//初始化商品的库存信息
				this.InitInventory(productSysNo);

				//导入数据不判断正负
				string sql = @"
					Update Inventory Set
						AccountQty = AccountQty - @DeltQty
					Where ProductSysNo = @ProductSysNo";

				sql = sql.Replace("@DeltQty", qty.ToString());
				sql = sql.Replace("@ProductSysNo", productSysNo.ToString());

				if ( 1 != new InventoryDac().UpdateQty(sql))
					throw new BizException("inventory: qty is not enough");

				string sql2 = @"
					Update Inventory_Stock Set
						AccountQty = AccountQty - @DeltQty
					Where ProductSysNo = @ProductSysNo and StockSysNo = @StockSysNo";

				sql2 = sql2.Replace("@DeltQty", qty.ToString());
				sql2 = sql2.Replace("@ProductSysNo", productSysNo.ToString());
				sql2 = sql2.Replace("@StockSysNo", stockSysNo.ToString());

				scope.Complete();
            }
		}
		public InventoryInfo LoadInventory(int productSysNo)
		{
			string sql = "select * from Inventory where productsysno = " + productSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( !Util.HasMoreRow(ds))
				return null;
			if ( ds.Tables[0].Rows.Count != 1 )
				throw new BizException("expected one-row affected failed");
			InventoryInfo oInfo = new InventoryInfo();
			map(oInfo, ds.Tables[0].Rows[0]);
			return oInfo;
		}

		private InventoryStockInfo LoadInventoryStock(string sql)
		{
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( !Util.HasMoreRow(ds))
				return null;
			if ( ds.Tables[0].Rows.Count != 1 )
				throw new BizException("expected one-row affected failed");
			InventoryStockInfo oInfo = new InventoryStockInfo();
			map(oInfo, ds.Tables[0].Rows[0]);
			return oInfo;
		}
		public InventoryStockInfo LoadInventoryStock(int stockSysNo, int productSysNo)
		{
			string sql = "select * from Inventory_Stock where stocksysno = " + stockSysNo + " and productsysno = " + productSysNo;
			return LoadInventoryStock(sql);
		}
		public InventoryStockInfo LoadInventoryStock(int sysno)
		{
			string sql = "select * from Inventory_Stock where sysno = " + sysno;
			return LoadInventoryStock(sql);
		}

		//新建商品的时候需要初始化库存, 
		//但是有的商品是以前建立的，目前商品转换的时候计算成本也需要初始化库存。
		public void InitInventory(int productSysNo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				string sql = " select top 1 sysno from Inventory where ProductSysNo=" + productSysNo;
				DataSet ds = SqlHelper.ExecuteDataSet(sql);
				if ( !Util.HasMoreRow(ds))
				{
					new InventoryDac().InitInventory(productSysNo);
				}

				string sqlStock = "select sysno from stock where sysno not in (select Stock.sysno from Stock, Inventory_Stock where Stock.Sysno = Inventory_Stock.StockSysNo and productSysno=" + productSysNo + ")";
				DataSet dsStock = SqlHelper.ExecuteDataSet(sqlStock);
				if ( Util.HasMoreRow(dsStock))
				{
					foreach(DataRow dr in dsStock.Tables[0].Rows)
					{
						new InventoryDac().InitInventoryStock(productSysNo, Util.TrimIntNull(dr["sysno"]));
					}
				}
				scope.Complete();
            }
		}

		public void SetPosition(int sysno, string pos1, string pos2)
		{
			new InventoryDac().SetPosition(sysno, pos1, pos2);
		}
		public DataSet GetInventoryDs(Hashtable paramHash)
		{
			string sql = @" select inventory.*, productid, productname from inventory, product
							where inventory.productsysno = product.sysno ";
			if ( paramHash != null && paramHash.Count != 0 )
			{
				StringBuilder sb = new StringBuilder(100);
				foreach(string key in paramHash.Keys)
				{
					sb.Append(" and ");
					object item = paramHash[key];
					if ( key == "special key")
					{
						//sb.Append("CreateTime >= ").Append(Util.ToSqlString(item.ToString()));
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
				sql = sql.Replace("select", "select top 50");
			}
			return SqlHelper.ExecuteDataSet(sql);
		}

		public DataSet GetInventoryStockDs(Hashtable paramHash)
		{
			string sql = @" select a.*, productid, productname, stockname from inventory_stock a, product, stock
						where a.stocksysno = stock.sysno and a.productsysno = product.sysno";
			if ( paramHash != null && paramHash.Count != 0 )
			{
				StringBuilder sb = new StringBuilder(100);
				foreach(string key in paramHash.Keys)
				{
					sb.Append(" and ");
					object item = paramHash[key];
					if ( key == "special key")
					{
						//sb.Append("CreateTime >= ").Append(Util.ToSqlString(item.ToString()));
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
				sql = sql.Replace("select", "select top 50");
			}
			sql += " order by productsysno ";
			return SqlHelper.ExecuteDataSet(sql);
		}


		public DataSet GetProductAllocatedDs(int stockSysNo, int productSysNo)
		{
			string sql = @"select a.*
							from 
							(
								-- lend
								(
									select 
									'lend' as record_name, 
									productsysno as record_product, 
									createtime as record_time,
									lendid as record_code,
									st_lend.sysno as record_sysno,
									lendqty as record_qty
									from 
									st_lend inner join st_lend_item
									on st_lend.sysno = st_lend_item.lendsysno
									where @lendstatus  and stocksysno = @stocksysno and productsysno=@productsysno
								)
								union all
								-- transfer
								(
									select 
									'transfer' as record_name,
									productsysno as record_product, 
									createtime as record_time,
									transferid as record_code,
									st_transfer.sysno as record_sysno,
									transferqty record_qty
									from 
									st_transfer inner join st_transfer_item
									on st_transfer.sysno = st_transfer_item.transfersysno
									where transfertype = @transfersource and @transferstatus and stocksysno = @stocksysno and productsysno = @productsysno
								)
								union all
								-- shift
								(
									select 
									'shiftout' as record_name,
									productsysno as record_product, 
									createtime as record_time,
									shiftid as record_code,
									st_shift.sysno as record_sysno,
									shiftqty as record_qty
									from 
									st_shift inner join st_shift_item
									on st_shift.sysno = st_shift_item.shiftsysno
									where @shiftstatus and  stocksysnoA = @stocksysno and productsysno=@productsysno
								)
								union all
								-- adjust
								(
									select 
									'adjust' as record_name, 
									productsysno as record_product, 
									createtime as record_time,
									adjustid as record_code,
									st_adjust.sysno as record_sysno,
									adjustqty*(-1) as record_qty
									from 
									st_adjust inner join st_adjust_item
									on st_adjust.sysno = st_adjust_item.adjustsysno
									where @adjuststatus and stocksysno = @stocksysno and productsysno=@productsysno
								)
							)
							as a 
							order by record_time";
			
			sql = sql.Replace("@stocksysno", stockSysNo.ToString());
			sql = sql.Replace("@productsysno", productSysNo.ToString());

			sql = sql.Replace("@lendstatus", "(status=" + (int)AppEnum.LendStatus.Origin + " or status =" + (int)AppEnum.LendStatus.Verified + ")");
			sql = sql.Replace("@transfersource", ((int)AppEnum.TransferItemType.Source).ToString());
			sql = sql.Replace("@transferstatus", "(status=" + (int)AppEnum.TransferStatus.Origin + " or status =" + (int)AppEnum.TransferStatus.Verified + ")");
			sql = sql.Replace("@shiftstatus", "(status=" + (int)AppEnum.ShiftStatus.Origin + " or status =" + (int)AppEnum.ShiftStatus.Verified + ")");
			sql = sql.Replace("@adjuststatus", "(status=" + (int)AppEnum.AdjustStatus.Origin + " or status =" + (int)AppEnum.AdjustStatus.Verified + ")");


			return SqlHelper.ExecuteDataSet(sql);
		}
		public DataSet GetProductCardDs(int stockSysNo, int productSysNo)
		{
			string sql = @"select a.*
							from 
							(
							--instock
								(
									select 
									'po(instock)' as record_name,
									productsysno as record_product,  
									intime as record_time,
									poid as record_code,
									po_master.sysno as record_sysno,
									quantity as record_qty,
									stockSysNo
									from 
									po_master inner join po_item
									on po_master.sysno = po_item.posysno
									where status = @postatus and stocksysno = @stocksysno and productsysno=@productsysno
								)
								union all
								-- lend
								(
									select 
									'lend' as record_name, 
									productsysno as record_product, 
									outtime as record_time,
									lendid as record_code,
									st_lend.sysno as record_sysno,
									lendqty*(-1) as record_qty,
									stocksysno
									from 
									st_lend inner join st_lend_item
									on st_lend.sysno = st_lend_item.lendsysno
									where status >= @lendstatus  and stocksysno = @stocksysno and productsysno=@productsysno
								)
							    union all
								-- lend
								(
									select 
									'lend(return)' as record_name, 
									productsysno as record_product, 
									returntime as record_time,
									lendid as record_code,
									st_lend.sysno as record_sysno,
									returnqty as record_qty,
									stocksysno
									from 
									st_lend inner join st_lend_return
									on st_lend.sysno = st_lend_return.lendsysno
									where status >= @lendreturnstatus  and stocksysno = @stocksysno and productsysno=@productsysno
								)
								union all
								-- ro
								(
									select 
									'ro' as record_name,
									returnsysno as record_product, 
									returntime as record_time,
									roid as record_code,
									ro_master.sysno as record_sysno,
									quantity as record_qty,
									stocksysno
									from 
									ro_master inner join ro_item
									on ro_master.sysno = ro_item.rosysno
									where status = @rostatus and stocksysno = @stocksysno and returnsysno=@productsysno

								)
								union all
								-- so
								(
									select 
									'so' as record_name,
									productsysno as record_product,  
									outtime as record_time,
									soid as record_code,
									so_master.sysno as record_sysno,
									quantity*(-1) as record_qty,
									stocksysno
									from 
									so_master inner join so_item
									on so_master.sysno = so_item.sosysno
									where status = @sostatus and stocksysno = @stocksysno and productsysno=@productsysno
								)
								union all
								-- transfer
								(
									select 
									'transfer' as record_name,
									productsysno as record_product, 
									outtime as record_time,
									transferid as record_code,
									st_transfer.sysno as record_sysno,
									case transfertype when @transfersource then transferqty*(-1) else transferqty end as record_qty,
									stocksysno
									from 
									st_transfer inner join st_transfer_item
									on st_transfer.sysno = st_transfer_item.transfersysno
									where status = @transferstatus and stocksysno = @stocksysno and productsysno = @productsysno
								)
								union all
								-- shift out
								(
									select 
									'shiftout' as record_name,
									productsysno as record_product, 
									outtime as record_time,
									shiftid as record_code,
									st_shift.sysno as record_sysno,
									shiftqty*(-1) as record_qty,
									stocksysnoA as stocksysno
									from 
									st_shift inner join st_shift_item
									on st_shift.sysno = st_shift_item.shiftsysno
									where status >= @shiftoutstatus and  stocksysnoA = @stocksysno and productsysno=@productsysno
								)
								union all
								-- shift in
								(
									select 
									'shiftin' as record_name,
									productsysno as record_product, 
									intime as record_time,
									shiftid as record_code,
									st_shift.sysno as record_sysno,
									shiftqty as record_qty,
									stocksysnob as stocksysno
									from 
									st_shift inner join st_shift_item
									on st_shift.sysno = st_shift_item.shiftsysno
									where status = @shiftinstatus and  stocksysnoB = @stocksysno and productsysno=@productsysno
								)
								union all
								-- adjust
								(
									select 
									'adjust' as record_name, 
									productsysno as record_product, 
									outtime as record_time,
									adjustid as record_code,
									st_adjust.sysno as record_sysno,
									adjustqty as record_qty,
									stocksysno
									from 
									st_adjust inner join st_adjust_item
									on st_adjust.sysno = st_adjust_item.adjustsysno
									where status=@adjuststatus and stocksysno = @stocksysno and productsysno=@productsysno
								)
							)
							as a 
							order by record_time";
			
			sql = sql.Replace("@stocksysno", stockSysNo.ToString());
			sql = sql.Replace("@productsysno", productSysNo.ToString());
			sql = sql.Replace("@postatus", ((int)AppEnum.POStatus.InStock).ToString());
			sql = sql.Replace("@lendstatus", ((int)AppEnum.LendStatus.OutStock).ToString());	//>=
			sql = sql.Replace("@lendreturnstatus", ((int)AppEnum.LendStatus.ReturnPartly).ToString()); //  >=
			sql = sql.Replace("@rostatus", ((int)AppEnum.ROStatus.Returned).ToString());
			sql = sql.Replace("@sostatus", ((int)AppEnum.SOStatus.OutStock).ToString());
			sql = sql.Replace("@transfersource", ((int)AppEnum.TransferItemType.Source).ToString());
			sql = sql.Replace("@transferstatus", ((int)AppEnum.TransferStatus.OutStock).ToString());
			sql = sql.Replace("@shiftoutstatus", ((int)AppEnum.ShiftStatus.OutStock).ToString()); // >=
			sql = sql.Replace("@shiftinstatus", ((int)AppEnum.ShiftStatus.InStock).ToString());
			sql = sql.Replace("@adjuststatus", ((int)AppEnum.AdjustStatus.OutStock).ToString());
            
			return SqlHelper.ExecuteDataSet(sql);
		}
		public DataSet DoCheck(int StockSysNo)
		{
			string sql = @"
						select 
							inventory_stock.stocksysno, inventory_stock.productsysno, inventory_stock.accountqty as stock_accountqty, productcard.accountqty as productcard_accountqty,product.productname
						from
							inventory_stock, product,
							(select stocksysno, record_product as productsysno, sum(record_qty) as accountqty
								from 
								(
								--instock
									(
										select 
										'po(instock)' as record_name,
										productsysno as record_product,  
										intime as record_time,
										poid as record_code,
										po_master.sysno as record_sysno,
										quantity as record_qty,
										stockSysNo
										from 
										po_master inner join po_item
										on po_master.sysno = po_item.posysno
										where status = @postatus 
									)
									union all
									-- lend
									(
										select 
										'lend' as record_name, 
										productsysno as record_product, 
										outtime as record_time,
										lendid as record_code,
										st_lend.sysno as record_sysno,
										lendqty*(-1) as record_qty,
										stocksysno
										from 
										st_lend inner join st_lend_item
										on st_lend.sysno = st_lend_item.lendsysno
										where status >= @lendstatus  
									)
								union all
									-- lend
									(
										select 
										'lend(return)' as record_name, 
										productsysno as record_product, 
										returntime as record_time,
										lendid as record_code,
										st_lend.sysno as record_sysno,
										returnqty as record_qty,
										stocksysno
										from 
										st_lend inner join st_lend_return
										on st_lend.sysno = st_lend_return.lendsysno
										where status >= @lendreturnstatus  
									)
									union all
									-- ro
									(
										select 
										'ro' as record_name,
										returnsysno as record_product, 
										returntime as record_time,
										roid as record_code,
										ro_master.sysno as record_sysno,
										quantity as record_qty,
										stocksysno
										from 
										ro_master inner join ro_item
										on ro_master.sysno = ro_item.rosysno
										where status = @rostatus 
									)
									union all
									-- so
									(
										select 
										'so' as record_name,
										productsysno as record_product,  
										outtime as record_time,
										soid as record_code,
										so_master.sysno as record_sysno,
										quantity*(-1) as record_qty,
										stocksysno
										from 
										so_master inner join so_item
										on so_master.sysno = so_item.sosysno
										where status = @sostatus 
									)
									union all
									-- transfer
									(
										select 
										'transfer' as record_name,
										productsysno as record_product, 
										outtime as record_time,
										transferid as record_code,
										st_transfer.sysno as record_sysno,
										case transfertype when @transfersource then transferqty*(-1) else transferqty end as record_qty,
										stocksysno
										from 
										st_transfer inner join st_transfer_item
										on st_transfer.sysno = st_transfer_item.transfersysno
										where status = @transferstatus 
									)
									union all
									-- shift out
									(
										select 
										'shiftout' as record_name,
										productsysno as record_product, 
										outtime as record_time,
										shiftid as record_code,
										st_shift.sysno as record_sysno,
										shiftqty*(-1) as record_qty,
										stocksysnoA as stocksysno
										from 
										st_shift inner join st_shift_item
										on st_shift.sysno = st_shift_item.shiftsysno
										where status >= @shiftoutstatus   
									)
									union all
									-- shift in
									(
										select 
										'shiftin' as record_name,
										productsysno as record_product, 
										intime as record_time,
										shiftid as record_code,
										st_shift.sysno as record_sysno,
										shiftqty as record_qty,
										stocksysnob as stocksysno
										from 
										st_shift inner join st_shift_item
										on st_shift.sysno = st_shift_item.shiftsysno
										where status = @shiftinstatus 
									)
									union all
									-- adjust
									(
										select 
										'adjust' as record_name, 
										productsysno as record_product, 
										outtime as record_time,
										adjustid as record_code,
										st_adjust.sysno as record_sysno,
										adjustqty as record_qty,
										stocksysno
										from 
										st_adjust inner join st_adjust_item
										on st_adjust.sysno = st_adjust_item.adjustsysno
										where status=@adjuststatus
									)
								)
								as a group by stocksysno, record_product ) as productcard
							where
                            inventory_stock.stocksysno=@stocksysno 
							and	inventory_stock.stocksysno = productcard.stocksysno
							and inventory_stock.productsysno = productcard.productSysNo
							and inventory_stock.accountqty <> productcard.accountqty
                            and product.sysno=inventory_stock.productsysno
							";
            sql = sql.Replace("@stocksysno", StockSysNo.ToString());
			sql = sql.Replace("@postatus", ((int)AppEnum.POStatus.InStock).ToString());
			sql = sql.Replace("@lendstatus", ((int)AppEnum.LendStatus.OutStock).ToString());	//>=
			sql = sql.Replace("@lendreturnstatus", ((int)AppEnum.LendStatus.ReturnPartly).ToString()); //  >=
			sql = sql.Replace("@rostatus", ((int)AppEnum.ROStatus.Returned).ToString());
			sql = sql.Replace("@sostatus", ((int)AppEnum.SOStatus.OutStock).ToString());
			sql = sql.Replace("@transfersource", ((int)AppEnum.TransferItemType.Source).ToString());
			sql = sql.Replace("@transferstatus", ((int)AppEnum.TransferStatus.OutStock).ToString());
			sql = sql.Replace("@shiftoutstatus", ((int)AppEnum.ShiftStatus.OutStock).ToString()); // >=
			sql = sql.Replace("@shiftinstatus", ((int)AppEnum.ShiftStatus.InStock).ToString());
			sql = sql.Replace("@adjuststatus", ((int)AppEnum.AdjustStatus.OutStock).ToString());
			return SqlHelper.ExecuteDataSet(sql);
		}

		/// <summary>
        /// 目前仅仅用于获取库位
		/// </summary>
		/// <param name="productSysNoHt"></param>
		/// <param name="stockSysNo"></param>
		/// <returns></returns>
		public Hashtable GetInventoryStockBoundle(Hashtable productSysNoHt, int stockSysNo)
		{
			if ( productSysNoHt == null || productSysNoHt.Count == 0)
				return null;

			string sql = "select * from inventory_stock where stocksysno = " + stockSysNo + " and productsysno in (";
			int index = 0;
			foreach(int item in productSysNoHt.Keys)
			{
				if ( index != 0 )
					sql +=",";

				sql += item.ToString();
				
				index ++;
			}
			sql +=")";

			DataSet ds = SqlHelper.ExecuteDataSet(sql);

			if ( !Util.HasMoreRow(ds))
				return null;

			Hashtable ht = new Hashtable(5);
			foreach(DataRow dr in ds.Tables[0].Rows)
			{
				InventoryStockInfo item = new InventoryStockInfo();
				map(item, dr);
				ht.Add(item.ProductSysNo, item);
			}
			return ht;
		}

        /// <summary>
        /// 获取库位号
        /// </summary>
        /// <param name="productSysNo"></param>
        /// <param name="stockSysNo"></param>
        /// <returns></returns>
        public string GetInventoryStockBoundle(int productSysNo, int stockSysNo)
        {
            string sql = "select Position1 from inventory_stock where stocksysno = " + stockSysNo + " and productsysno = " + productSysNo;

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                return Util.TrimNull(ds.Tables[0].Rows[0][0]);
            else
                return "";
        }

        ///// <summary>
        ///// RMA仓库一般单据出库和取消出库(不更新总仓)
        ///// </summary>
        ///// <param name="stockSysNo"></param>
        ///// <param name="productSysNo"></param>
        ///// <param name="qty"></param>
        //public void SetOutStockQtyRMA(int stockSysNo, int productSysNo, int qty)
        //{
        //    TransactionOptions options = new TransactionOptions();
        //    options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
        //    options.Timeout = TransactionManager.DefaultTimeout;

        //    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
        //    {

        //        Hashtable htQty = new Hashtable(2);
        //        htQty.Add("AccountQty", "-");

        //        Hashtable htCon = new Hashtable(2);
        //        htCon.Add("AccountQty", "-");

        //        string sql2 = buildSql(stockSysNo, productSysNo, qty, htQty, htCon);
        //        if (1 != new InventoryDac().UpdateQty(sql2))
        //            throw new BizException("inventory_stock: qty is not enough");

        //        scope.Complete();
        //    }
        //}
        ///// <summary>
        ///// RMA仓库一般单据入库和取消入库(不更新总仓)
        ///// </summary>
        ///// <param name="stockSysNo"></param>
        ///// <param name="productSysNo"></param>
        ///// <param name="qty"></param>
        //public void SetInStockQtyRMA(int stockSysNo, int productSysNo, int qty)
        //{
        //    TransactionOptions options = new TransactionOptions();
        //    options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
        //    options.Timeout = TransactionManager.DefaultTimeout;

        //    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
        //    {

        //        //初始化商品的库存信息
        //        this.InitInventory(productSysNo);

        //        Hashtable htQty = new Hashtable(2);
        //        htQty.Add("AccountQty", "+");

        //        Hashtable htCon = new Hashtable(2);
        //        htCon.Add("AccountQty", "+");

        //        string sql2 = buildSql(stockSysNo, productSysNo, qty, htQty, htCon);
        //        if (1 != new InventoryDac().UpdateQty(sql2))
        //            throw new BizException("inventory_stock: qty is not enough");

        //        scope.Complete();
        //    }
        //}

        /// <summary>
        /// saleReturn  InStock / cancelInStock,只修改财务数量、分配数量
        /// </summary>
        /// <param name="stockSysNo"></param>
        /// <param name="productSysNo"></param>
        /// <param name="qty"></param>
        public void SetInStockQty2(int stockSysNo, int productSysNo, int qty)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                StockInfo stock = StockManager.GetInstance().Load(stockSysNo);
                if (stock == null)
                    return;

                //初始化商品的库存信息
                this.InitInventory(productSysNo);

                Hashtable htQty = new Hashtable(2);
                htQty.Add("AccountQty", "+");
                htQty.Add("AllocatedQty", "+");

                Hashtable htCon = new Hashtable(2);
                htCon.Add("AccountQty", "+");
                htCon.Add("AllocatedQty", "+");

                //更新总仓财务库存
                string sql = buildSql(AppConst.IntNull, productSysNo, qty, htQty, htCon);
                if (1 != new InventoryDac().UpdateQty(sql))
                    throw new BizException("inventory: qty is not enough");


                //更新分库财务库存
                string sql2 = buildSql(stockSysNo, productSysNo, qty, htQty, htCon);
                if (1 != new InventoryDac().UpdateQty(sql2))
                    throw new BizException("inventory_stock: qty is not enough");
                scope.Complete();
            }
        }

        /// <summary>
        /// saleReturn  上架，修改可用数量、分配数量
        /// </summary>
        /// <param name="stockSysNo"></param>
        /// <param name="productSysNo"></param>
        /// <param name="qty"></param>
        public void SetAvailableQty2(int stockSysNo, int productSysNo, int qty)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                StockInfo stock = StockManager.GetInstance().Load(stockSysNo);
                if (stock == null)
                    return;

                //初始化商品的库存信息
                this.InitInventory(productSysNo);

                Hashtable htQty = new Hashtable(2);
                htQty.Add("AvailableQty", "+");
                htQty.Add("AllocatedQty", "-");

                Hashtable htCon = new Hashtable(2);
                htCon.Add("AvailableQty", "+");
                htCon.Add("AllocatedQty", "-");

                //更新总仓财务库存
                string sql = buildSql(AppConst.IntNull, productSysNo, qty, htQty, htCon);
                if (1 != new InventoryDac().UpdateQty(sql))
                    throw new BizException(productSysNo + "inventory: qty is not enough");


                //更新分库财务库存
                string sql2 = buildSql(stockSysNo, productSysNo, qty, htQty, htCon);
                if (1 != new InventoryDac().UpdateQty(sql2))
                    throw new BizException(productSysNo + "inventory_stock: qty is not enough");
                scope.Complete();
            }
        }

		#region Import
		public void Import()
		{
			if ( !AppConfig.IsImportable)
				throw new BizException("Is Importable is false");

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				//如果inventory表不为空，就不能导入数据
				string sql = "select top 1 * from inventory";
				DataSet ds = SqlHelper.ExecuteDataSet(sql);
				if ( Util.HasMoreRow(ds) )
					throw new BizException("the table inventory is not empty");

				//在ipp3里面 available + allocated + order = account.
				//采购在途将用po导入的时候控制，初始化为零。
				//不采用AccountQty-AvailableQty-WebQty as AllocatedQty, 全部由单据来控制
				string sql2 = @"select conProduct.newSysNo as productSysNo, 
							AccountQty, 
							AvailableQty+WebQty-OrderQty as AvailableQty, 
							0 as AllocatedQty, 
							OrderQty, 0 as PurchaseQty, VirtualQty 
						from ipp2003..product_inventory as inv, ippconvert..productbasic as conproduct
						where inv.productsysno = conProduct.oldsysno";
				DataSet ds2 = SqlHelper.ExecuteDataSet(sql2);
				if ( !Util.HasMoreRow(ds2) )
					return;
				Hashtable ht = new Hashtable(1000);
				foreach(DataRow dr in ds2.Tables[0].Rows)
				{
					InventoryInfo item = new InventoryInfo();
					map(item, dr);
					ht.Add(item.ProductSysNo, item);
					new InventoryDac().Insert(item);
				}

				//如果inventory_stock表不为空，就不能导入数据
				string sql3 = "select top 1 * from inventory_stock";
				DataSet ds3 = SqlHelper.ExecuteDataSet(sql3);
				if ( Util.HasMoreRow(ds3) )
					throw new BizException("the table inventory_stock is not empty");

				string sql5 = "select sysno from stock where stockname='上海本部' ";
				DataSet ds5 = SqlHelper.ExecuteDataSet(sql5);
				if ( !Util.HasMoreRow(ds5))
					throw new BizException("stock sysno load error");
				int stockSysNo = Util.TrimIntNull(ds5.Tables[0].Rows[0]["sysno"]);

				string sql4 = @"select conStock.newSysno as StockSysno, conProduct.newSysNo as productSysNo, 
									AccountQty, AvailableQty+WebQty as AvailableQty, 
									0 as AllocatedQty, 
									ShiftInQty, ShiftOutQty,isnull(SafeQty,0) as SafeQty,Position1,Position2,
									0 as OrderQty, 0 as PurchaseQty
								from ipp2003..stock_inventory as inv, ippconvert..productbasic as conproduct, ippconvert..stock as conStock
								where inv.productsysno = conProduct.oldsysno and conStock.oldsysno = inv.stocksysno";
				DataSet ds4 = SqlHelper.ExecuteDataSet(sql4);
				if ( !Util.HasMoreRow(ds4) )
					return;
				foreach(DataRow dr in ds4.Tables[0].Rows)
				{
					InventoryStockInfo item = new InventoryStockInfo();
					map(item, dr);
					if ( item.StockSysNo == stockSysNo )
					{
						InventoryInfo item2 = ht[item.ProductSysNo] as InventoryInfo;
						if ( item2 == null )
							throw new Exception("this product has record in stock, but no records in inventory, impossible!");
						item.AvailableQty -= item2.OrderQty;
						item.OrderQty = item2.OrderQty;
					}
					new InventoryDac().Insert(item);
				}

				scope.Complete();
            }
		}
		private void map(InventoryInfo oParam, DataRow tempdr)
		{
			//oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.ProductSysNo = Util.TrimIntNull(tempdr["ProductSysNo"]);
			oParam.AccountQty = Util.TrimIntNull(tempdr["AccountQty"]);
			oParam.AvailableQty = Util.TrimIntNull(tempdr["AvailableQty"]);
			oParam.AllocatedQty = Util.TrimIntNull(tempdr["AllocatedQty"]);
			oParam.OrderQty = Util.TrimIntNull(tempdr["OrderQty"]);
			oParam.PurchaseQty = Util.TrimIntNull(tempdr["PurchaseQty"]);
			oParam.VirtualQty = Util.TrimIntNull(tempdr["VirtualQty"]);
          }
        private void map(InventoryStockInfo oParam, DataRow tempdr)
        {
            //oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.StockSysNo = Util.TrimIntNull(tempdr["StockSysNo"]);
            oParam.ProductSysNo = Util.TrimIntNull(tempdr["ProductSysNo"]);
            oParam.AccountQty = Util.TrimIntNull(tempdr["AccountQty"]);
            oParam.AvailableQty = Util.TrimIntNull(tempdr["AvailableQty"]);
            oParam.AllocatedQty = Util.TrimIntNull(tempdr["AllocatedQty"]);
            oParam.OrderQty = Util.TrimIntNull(tempdr["OrderQty"]);
            oParam.PurchaseQty = Util.TrimIntNull(tempdr["PurchaseQty"]);
            oParam.ShiftInQty = Util.TrimIntNull(tempdr["ShiftInQty"]);
            oParam.ShiftOutQty = Util.TrimIntNull(tempdr["ShiftOutQty"]);
            oParam.SafeQty = Util.TrimIntNull(tempdr["SafeQty"]);
            oParam.Position1 = Util.TrimNull(tempdr["Position1"]);
            oParam.Position2 = Util.TrimNull(tempdr["Position2"]);
            oParam.PosLastUpdateTime = Util.TrimDateNull(tempdr["PosLastUpdateTime"]);
        }
		
        #endregion
	}
}
