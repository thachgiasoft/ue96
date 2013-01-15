using System;
using System.Data;
using System.Collections;
using System.Text;

using Icson.Utils;
using System.Transactions;
using Icson.Objects;
using Icson.Objects.Stock;
using Icson.DBAccess;
using Icson.DBAccess.Stock;
using Icson.BLL.Basic;
using Icson.BLL.Finance;
using Icson.Objects.Basic;

namespace Icson.BLL.Stock
{
	/// <summary>
	/// Summary description for TransferManager.
	/// </summary>
	public class TransferManager
	{
		private TransferManager()
		{
		}

		private static TransferManager _instance;
		public static TransferManager GetInstance()
		{
			if( _instance == null )
			{
				_instance = new TransferManager();
			}
			return _instance;
		}

		public DataSet GetTransferListDs(Hashtable paramHash)
		{
			string sql = @" select st.*, b.username as CreateUserName, c.username as AuditUserName, d.username as OutUserName 
							from st_transfer st, sys_user b, sys_user c, sys_user d 
							where 
								st.createusersysno *= b.sysno
								and st.auditusersysno *= c.sysno
								and st.outusersysno *= d.sysno ";
			if ( paramHash != null && paramHash.Count != 0 )
			{
				StringBuilder sb = new StringBuilder(100);
				foreach(string key in paramHash.Keys)
				{
					sb.Append(" and ");
					object item = paramHash[key];
					if ( key == "DateFrom")
					{
						sb.Append("CreateTime >= ").Append(Util.ToSqlString(item.ToString()));
					}
					else if ( key == "DateTo")
					{
						sb.Append("CreateTime <= ").Append(Util.ToSqlEndDate(item.ToString()));
					}
					else if ( key == "ProductSysNo")
					{
						sb.Append(" exists ( select top 1 sysno from st_transfer_item where st.sysno=st_transfer_item.transfersysno and productsysno = ").Append(Util.SafeFormat(item.ToString())).Append(" ) ");
					}
					else if ( key == "Status" )
					{
						sb.Append("st.Status = ").Append(item.ToString());
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

			sql += " order by st.sysno desc";
			return SqlHelper.ExecuteDataSet(sql);
		}
		public TransferInfo Load(int transferSysNo)
		{
			TransferInfo masterInfo;

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				string masterSql = "select * from st_transfer where sysno = " + transferSysNo;
				DataSet masterDs = SqlHelper.ExecuteDataSet(masterSql);
				if ( !Util.HasMoreRow(masterDs))
					throw new BizException("there is no this transfer sysno");

				masterInfo = new TransferInfo();
				map(masterInfo, masterDs.Tables[0].Rows[0]);

				string itemSql = "select * from st_transfer_item where transfersysno=" + transferSysNo;
				DataSet itemDs = SqlHelper.ExecuteDataSet(itemSql);
				if ( Util.HasMoreRow(itemDs))
				{
					foreach(DataRow dr in itemDs.Tables[0].Rows)
					{
						TransferItemInfo item = new TransferItemInfo();
						map(item, dr);
						masterInfo.itemHash.Add(item.ProductSysNo, item);
					}
				}	
				scope.Complete();
            }

			return masterInfo;
		}

		#region map
		private void map(TransferInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.TransferID = Util.TrimNull(tempdr["TransferID"]);
			oParam.StockSysNo = Util.TrimIntNull(tempdr["StockSysNo"]);
			oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
			oParam.CreateUserSysNo = Util.TrimIntNull(tempdr["CreateUserSysNo"]);
			oParam.AuditTime = Util.TrimDateNull(tempdr["AuditTime"]);
			oParam.AuditUserSysNo = Util.TrimIntNull(tempdr["AuditUserSysNo"]);
			oParam.OutTime = Util.TrimDateNull(tempdr["OutTime"]);
			oParam.OutUserSysNo = Util.TrimIntNull(tempdr["OutUserSysNo"]);
			oParam.Status = Util.TrimIntNull(tempdr["Status"]);
			oParam.Note = Util.TrimNull(tempdr["Note"]);
		}
		private void map(TransferItemInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.TransferSysNo = Util.TrimIntNull(tempdr["TransferSysNo"]);
			oParam.ProductSysNo = Util.TrimIntNull(tempdr["ProductSysNo"]);
			oParam.TransferType = Util.TrimIntNull(tempdr["TransferType"]);
			oParam.TransferQty = Util.TrimIntNull(tempdr["TransferQty"]);
			oParam.TransferCost = Util.TrimDecimalNull(tempdr["TransferCost"]);
		}
		#endregion

		#region  getID 、getCurrentStatus
		private string getTransferID(int sysNo)
		{
			return "58" + sysNo.ToString().PadLeft(8,'0');
		}
		private int getCurrentStatus(int transferSysNo)
		{
			int status = AppConst.IntNull;

			string sql = "select status from st_transfer where sysno =" + transferSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			
			if ( Util.HasMoreRow(ds))
				status = Util.TrimIntNull( ds.Tables[0].Rows[0]["status"] );

			return status;
		}
		private bool isBalance(TransferInfo oMaster)
		{
			if ( oMaster.itemHash.Count == 0)
				return true;
			decimal sourceTotal, targetTotal;
			sourceTotal = targetTotal = 0;
			foreach(TransferItemInfo item in oMaster.itemHash.Values)
			{
				if ( item.TransferType == (int)AppEnum.TransferItemType.Source )
				{
					sourceTotal += item.TransferQty * item.TransferCost;
				}
				else
				{
					targetTotal += item.TransferQty * item.TransferCost;
				}
			}
			if ( Math.Abs(sourceTotal-targetTotal) < 0.09M )
				return true;
			else
				return false;
		}
		#endregion

		public void Create(TransferInfo oParam)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				oParam.SysNo = SequenceDac.GetInstance().Create("St_Transfer_Sequence");
				oParam.TransferID = getTransferID(oParam.SysNo);
				//建立主表记录
				int rowsAffected = new TransferDac().InsertMaster(oParam);
				if(rowsAffected != 1)
					throw new BizException("insert transfer master error");
				foreach( TransferItemInfo item in oParam.itemHash.Values)
				{
					item.TransferSysNo = oParam.SysNo;
					
					//更新source的成本
					if ( item.TransferType == (int)AppEnum.TransferItemType.Source) 
					{
						ProductPriceInfo priceInfo = ProductManager.GetInstance().LoadPrice(item.ProductSysNo);
						item.TransferCost = priceInfo.UnitCost;
					}

					rowsAffected = new TransferDac().InsertItem(item);
					if ( rowsAffected != 1)
						throw new BizException("insert transfer item error");
					if ( item.TransferType == (int)AppEnum.TransferItemType.Source )
					{//需要占用库存
						InventoryManager.GetInstance().SetAvailableQty(oParam.StockSysNo, item.ProductSysNo, item.TransferQty);
					}
					
				}

				scope.Complete();
            }
		}
		public void UpdateMaster(TransferInfo oParam)
		{
			//主项可以更新note
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				
				//必须是初始状态
				if ( getCurrentStatus(oParam.SysNo) != (int)AppEnum.TransferStatus.Origin )
					throw new BizException("status is not origin now,  update failed");

				//设置 单号, 备注
				Hashtable ht = new Hashtable(3);
				ht.Add("SysNo", oParam.SysNo);
				ht.Add("Note", oParam.Note);
				if ( 1!=new TransferDac().UpdateMaster(ht))
					throw new BizException("expected one-row update failed, update failed ");
				
				scope.Complete();
            }
		}

		public void Verify(int masterSysNo, int userSysNo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				TransferInfo oMaster = Load(masterSysNo);

				//必须是初始状态
				if ( oMaster.Status != (int)AppEnum.TransferStatus.Origin )
					throw new BizException("status is not origin now ,  verify failed");

				if ( !isBalance(oMaster))
					throw new BizException("source cost != target cost");

				//设置 单号、状态和审核人
				Hashtable ht = new Hashtable(4);

				ht.Add("SysNo", masterSysNo);
				ht.Add("Status", (int)AppEnum.TransferStatus.Verified);
				ht.Add("AuditTime", DateTime.Now);
				ht.Add("AuditUserSysNo", userSysNo);
				if ( 1!=new TransferDac().UpdateMaster(ht))
					throw new BizException("expected one-row update failed, verify failed ");

				
				scope.Complete();
            }
		}

		public void Abandon(int masterSysNo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				TransferInfo masterInfo = Load(masterSysNo);
				
				//必须是初始状态
				if ( masterInfo.Status != (int)AppEnum.TransferStatus.Origin )
					throw new BizException("status is not origin now ,  abandon failed");

				//设置 单号、状态
				Hashtable ht = new Hashtable(4);
				ht.Add("SysNo", masterSysNo);
				ht.Add("Status", (int)AppEnum.TransferStatus.Abandon);
				if ( 1!=new TransferDac().UpdateMaster(ht))
					throw new BizException("expected one-row update failed, abandon failed ");

				//取消对available数量的占用
				foreach(TransferItemInfo item in masterInfo.itemHash.Values)
				{
					if ( item.TransferType == (int)AppEnum.TransferItemType.Source )
					{
						InventoryManager.GetInstance().SetAvailableQty(masterInfo.StockSysNo, item.ProductSysNo, -1*item.TransferQty);
					}
				}
				scope.Complete();
            }
		}

		public void CancelAbandon(int masterSysNo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				TransferInfo masterInfo = Load(masterSysNo);
				
				//必须是废弃状态
				if ( masterInfo.Status != (int)AppEnum.TransferStatus.Abandon )
					throw new BizException("status is not abandon now ,  cancel abandon failed");

				//设置 单号、状态
				Hashtable ht = new Hashtable(4);
				ht.Add("SysNo", masterSysNo);
				ht.Add("Status", (int)AppEnum.TransferStatus.Origin);
				if ( 1!=new TransferDac().UpdateMaster(ht))
					throw new BizException("expected one-row update failed, cancel abandon failed ");

				//增加对available数量的占用
				foreach(TransferItemInfo item in masterInfo.itemHash.Values)
				{
					if ( item.TransferType == (int)AppEnum.TransferItemType.Source )
					{
						InventoryManager.GetInstance().SetAvailableQty(masterInfo.StockSysNo, item.ProductSysNo, item.TransferQty);
					}
				}
				scope.Complete();
            }
		}

		public void CancelVerify(int masterSysNo, int userSysNo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				TransferInfo masterInfo = Load(masterSysNo);
				
				//必须是已审核状态
				if ( masterInfo.Status != (int)AppEnum.TransferStatus.Verified )
					throw new BizException("status is not verified now,  cancel verify failed");

				//设置 单号、状态
				Hashtable ht = new Hashtable(4);
				ht.Add("SysNo", masterInfo.SysNo);
				ht.Add("Status", (int)AppEnum.TransferStatus.Origin);
				ht.Add("AuditTime", DateTime.Now);
				ht.Add("AuditUserSysNo", userSysNo);
				if ( 1!=new TransferDac().UpdateMaster(ht))
					throw new BizException("expected one-row update failed, cancel verify failed ");
				
				scope.Complete();
            }
		}

		public void OutStock(int masterSysNo, int userSysNo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				TransferInfo masterInfo = Load(masterSysNo);
				
				//必须是已审核
				if ( masterInfo.Status != (int)AppEnum.TransferStatus.Verified )
					throw new BizException("status is not verify now,  outstock failed");

				if ( !isBalance(masterInfo))
					throw new BizException("source cost != target cost");

				//设置 单号、状态
				Hashtable ht = new Hashtable(4);
				ht.Add("SysNo", masterInfo.SysNo);
				ht.Add("Status", (int)AppEnum.TransferStatus.OutStock);
				ht.Add("OutTime", DateTime.Now);
				ht.Add("OutUserSysNo", userSysNo);
				if ( 1!=new TransferDac().UpdateMaster(ht))
					throw new BizException("expected one-row update failed, outstock failed ");

				foreach(TransferItemInfo item in masterInfo.itemHash.Values)
				{
					//库存设定
					if ( item.TransferType == (int)AppEnum.TransferItemType.Source )
					{
                        UnitCostManager.GetInstance().SetCost(item.ProductSysNo, -1*item.TransferQty, item.TransferCost);
						InventoryManager.GetInstance().SetOutStockQty(masterInfo.StockSysNo, item.ProductSysNo, item.TransferQty);
					}
					else
					{
                        UnitCostManager.GetInstance().SetCost(item.ProductSysNo, item.TransferQty, item.TransferCost);
						InventoryManager.GetInstance().SetInStockQty(masterInfo.StockSysNo, item.ProductSysNo, item.TransferQty);
					}
				}

			scope.Complete();
            }
		}

		public void CancelOutStock(int masterSysNo, int userSysNo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				TransferInfo masterInfo = Load(masterSysNo);
				
				//必须是已出库
				if ( masterInfo.Status != (int)AppEnum.TransferStatus.OutStock )
					throw new BizException("status is not outstock now,  cancel outstock failed");


				//设置 单号、状态
				Hashtable ht = new Hashtable(4);
				ht.Add("SysNo", masterInfo.SysNo);
				ht.Add("Status", (int)AppEnum.TransferStatus.Origin);
				ht.Add("OutTime", DateTime.Now);
				ht.Add("OutUserSysNo", userSysNo);
				if ( 1!=new TransferDac().UpdateMaster(ht))
					throw new BizException("expected one-row update failed, cancel outstock failed ");

				
				foreach(TransferItemInfo item in masterInfo.itemHash.Values)
				{
					//库存设定
					if ( item.TransferType == (int)AppEnum.TransferItemType.Source )
					{
                        UnitCostManager.GetInstance().SetCost(item.ProductSysNo, item.TransferQty, item.TransferCost);
						InventoryManager.GetInstance().SetOutStockQty(masterInfo.StockSysNo, item.ProductSysNo, -1*item.TransferQty);
					}
					else
					{
                        UnitCostManager.GetInstance().SetCost(item.ProductSysNo, -1*item.TransferQty, item.TransferCost);
						InventoryManager.GetInstance().SetInStockQty(masterInfo.StockSysNo, item.ProductSysNo, -1*item.TransferQty);
					}
				}
				scope.Complete();
            }
		}

		/// <summary>
		/// 更新数量和成本
		/// 如果是souce， 成本从当前数据库重新取
		/// </summary>
		/// <param name="masterInfo"></param>
		/// <param name="itemInfo"></param>
		public void UpdateItem(TransferInfo masterInfo, TransferItemInfo itemInfo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				
				//必须是初始
				if ( getCurrentStatus(masterInfo.SysNo) != (int)AppEnum.TransferStatus.Origin )
					throw new BizException("status is not origin now,  update item qty failed");

				//获取数量差值
				TransferItemInfo oldItemInfo = masterInfo.itemHash[itemInfo.ProductSysNo] as TransferItemInfo;
				int deltQty = itemInfo.TransferQty - oldItemInfo.TransferQty;

				//更新表单明细 ( 如果增加表单明细项的属性，需要在这里处理一下）
				itemInfo.SysNo = oldItemInfo.SysNo;
				itemInfo.TransferSysNo = oldItemInfo.TransferSysNo;
				itemInfo.TransferType = oldItemInfo.TransferType;

				//更新source的成本更新库存
				if ( itemInfo.TransferType == (int)AppEnum.TransferItemType.Source)
				{
					ProductPriceInfo priceInfo = ProductManager.GetInstance().LoadPrice(itemInfo.ProductSysNo);
					itemInfo.TransferCost = priceInfo.UnitCost;
					InventoryManager.GetInstance().SetAvailableQty(masterInfo.StockSysNo, itemInfo.ProductSysNo, deltQty);
				}

				//更新item到数据库
				if ( 1 != new TransferDac().UpdateItem(itemInfo.SysNo, deltQty, itemInfo.TransferCost))
					throw new BizException("expected one-row update failed, update item qty failed");

				//更新 itemInfo 到 masterInfo 注:数据库更新成功以后才更新类
				masterInfo.itemHash.Remove(itemInfo.ProductSysNo);
				masterInfo.itemHash.Add(itemInfo.ProductSysNo, itemInfo);

				scope.Complete();
            }	
		}

		public void InsertItem(TransferInfo masterInfo, TransferItemInfo itemInfo)
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
				if ( getCurrentStatus(masterInfo.SysNo) != (int)AppEnum.TransferStatus.Origin )
					throw new BizException("status is not origin now,  add item failed");

				//更新库存
				if ( itemInfo.TransferType == (int)AppEnum.TransferItemType.Source )
				{
					ProductPriceInfo priceInfo = ProductManager.GetInstance().LoadPrice(itemInfo.ProductSysNo);
					itemInfo.TransferCost = priceInfo.UnitCost;
					InventoryManager.GetInstance().SetAvailableQty(masterInfo.StockSysNo, itemInfo.ProductSysNo, itemInfo.TransferQty);
				}

				//插入item到数据库
				if ( 1 != new TransferDac().InsertItem(itemInfo))
					throw new BizException("expected one-row update failed, add item failed");

				//插入itemInfo 到 masterInfo
				masterInfo.itemHash.Add(itemInfo.ProductSysNo, itemInfo);

				scope.Complete();
            }
		}

		public void DeleteItem(TransferInfo masterInfo, int itemProductSysNo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				
				//必须是初始
				if ( getCurrentStatus(masterInfo.SysNo) != (int)AppEnum.TransferStatus.Origin )
					throw new BizException("status is not origin now,  delete item failed");

				//获取数量差值
				TransferItemInfo oldItemInfo = masterInfo.itemHash[itemProductSysNo] as TransferItemInfo;
				int deltQty = -1*oldItemInfo.TransferQty;

				//更新item
				if ( 1 != new TransferDac().DeleteItem(oldItemInfo.SysNo))
					throw new BizException("expected one-row update failed, delete item qty failed");

				//更新库存
				if ( oldItemInfo.TransferType == (int)AppEnum.TransferItemType.Source)
				{
					InventoryManager.GetInstance().SetAvailableQty(masterInfo.StockSysNo, itemProductSysNo, deltQty);
				}

				//更新 masterInfo
				masterInfo.itemHash.Remove(itemProductSysNo);

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
			
			string sql = " select top 1 sysno from st_transfer";
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds) )
				throw new BizException("the table transfer is not empty");

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				string sql_old = @"select 
								old.sysno, old.transferid, stock_con.newsysno as stocksysno,
								create_con.newsysno as createusersysno,
								audit_con.newsysno as auditusersysno,
								audit_con.newsysno as outusersysno,
								createtime, audittime,audittime as outtime,
								auditstatus, note, '1' as status
							from 
								ipp2003..st_transfer as old, 
								ippconvert..sys_user as create_con,
								ippconvert..sys_user as audit_con,
								ippconvert..stock as stock_con
							where 
								old.createusersysno *= create_con.oldsysno and
								old.auditusersysno *= audit_con.oldsysno and
								old.stocksysno = stock_con.oldsysno
								order by old.sysno";
				DataSet ds_old = SqlHelper.ExecuteDataSet(sql_old);
				if ( !Util.HasMoreRow(ds_old) )
					return;
				foreach(DataRow dr in ds_old.Tables[0].Rows)
				{
					/* newStatus	audit	
					 * abandon		-1		
					 * origin		0		
					 * verified		1		
					 */	
					int newStatus = (int)AppEnum.TransferStatus.Origin;
					int auditStatus = Util.TrimIntNull(dr["auditStatus"]);
					if ( auditStatus == -1 )
						newStatus = (int)AppEnum.TransferStatus.Abandon;
					else if ( auditStatus == 0 )
						newStatus = (int)AppEnum.TransferStatus.Origin;
					else if ( auditStatus == 1)
						newStatus = (int)AppEnum.TransferStatus.OutStock;
					else
						throw new BizException("old transfer status out of range");

					
					TransferInfo  oInfo = new TransferInfo();
					map(oInfo, dr);
					oInfo.Status = newStatus;

					if ( new TransferDac().InsertMaster(oInfo)!= 1)
					{
						throw new BizException("master expected one row error");
					}

					//insert item
					string sql_item = @"select '0' as sysno,
										TransferSysNo, con_product.newsysno as productsysno, transferqty, transfercost, transferstatus as transfertype
									from 
										ipp2003..St_Transfer_Item sti, ippconvert..productbasic as con_product
									where sti.productsysno = con_product.oldsysno and TransferSysNo=" + oInfo.SysNo;

					DataSet ds_item = SqlHelper.ExecuteDataSet(sql_item);
					if ( !Util.HasMoreRow(ds_item) )
						continue;
					foreach(DataRow drItem in ds_item.Tables[0].Rows)
					{
						TransferItemInfo oItem = new TransferItemInfo();
						map(oItem, drItem);

						if ( oItem.TransferCost == 0 )
							oItem.TransferCost = AppConst.IntNull;

						int resultitem = new TransferDac().InsertItem(oItem);
						if ( resultitem < 1 )
							throw new BizException("insert item row < 1");

						if ( oInfo.Status == (int)AppEnum.TransferStatus.Origin && oItem.TransferType == (int)AppEnum.TransferItemType.Source )
						{
							InventoryManager.GetInstance().SetAvailableQty(oInfo.StockSysNo, oItem.ProductSysNo, oItem.TransferQty);
						}
					}
				}

				string sqlMaxSysNo = "select top 1 sysno from ipp2003..st_transfer order by sysno desc";
				DataSet dsMax = SqlHelper.ExecuteDataSet(sqlMaxSysNo);
				if ( !Util.HasMoreRow(dsMax))
					throw new BizException("got max sysno error");
				int maxSysNo = Util.TrimIntNull(dsMax.Tables[0].Rows[0]["sysno"]);
				// 将自动生成的sysno填到目前的最大单据号
				int newSysNo;
				do 
				{
					newSysNo = SequenceDac.GetInstance().Create("St_Transfer_Sequence");
				} while( newSysNo < maxSysNo);
				
				
			scope.Complete();
            }

		}

        public int GetTransferSysNofromID(string transferID)
        {
            string sql = "select sysno from St_Transfer where transferID = " + Util.ToSqlString(transferID);
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                return (int)ds.Tables[0].Rows[0][0];
            else
                return AppConst.IntNull;
        }
	}
}
