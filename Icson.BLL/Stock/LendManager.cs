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

namespace Icson.BLL.Stock
{
	/// <summary>
	/// Summary description for LendManager.
	/// </summary>
	public class LendManager
	{
		private LendManager()
		{
		}
		private static LendManager _instance;
		public static LendManager GetInstance()
		{
			if( _instance == null )
			{
				_instance = new LendManager();
			}
			return _instance;
		}

		public DataSet GetLendListDs(Hashtable paramHash)
		{
			string sql = @" select st.*, a.username as LendUserName, b.username as CreateUserName, c.username as AuditUserName, d.username as OutUserName 
							from st_lend st, sys_user a, sys_user b, sys_user c, sys_user d 
							where st.lendusersysno *= a.sysno
								and st.createusersysno *= b.sysno
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
						sb.Append(" exists ( select top 1 sysno from st_lend_item where st.sysno=st_lend_item.lendsysno and productsysno = ").Append(Util.SafeFormat(item.ToString())).Append(" ) ");
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

		public LendInfo Load(int lendSysNo)
		{
			LendInfo masterInfo;

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				string masterSql = "select * from st_lend where sysno = " + lendSysNo;
				DataSet masterDs = SqlHelper.ExecuteDataSet(masterSql);
				if ( !Util.HasMoreRow(masterDs))
					throw new BizException("there is no this lend sysno");

				masterInfo = new LendInfo();
				map(masterInfo, masterDs.Tables[0].Rows[0]);

				string itemSql = "select * from st_lend_item where lendsysno=" + lendSysNo;
				DataSet itemDs = SqlHelper.ExecuteDataSet(itemSql);
				if ( Util.HasMoreRow(itemDs))
				{
					foreach(DataRow dr in itemDs.Tables[0].Rows)
					{
						LendItemInfo item = new LendItemInfo();
						map(item, dr);
						masterInfo.itemHash.Add(item.ProductSysNo, item);
					}
				}

				//因为还货记录的pk不是product，所以用sysno 做key
				string returnSql = "select * from st_lend_return where lendsysno=" +lendSysNo;
				DataSet returnDs = SqlHelper.ExecuteDataSet(returnSql);
				if ( Util.HasMoreRow(returnDs))
				{
					foreach(DataRow dr in returnDs.Tables[0].Rows)
					{
						LendReturnInfo item = new LendReturnInfo();
						map(item, dr);
						masterInfo.returnHash.Add(item.SysNo, item);
					}
				}
					
				scope.Complete();
            }

			return masterInfo;

		}

		#region map
		private void map(LendInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.LendID = Util.TrimNull(tempdr["LendID"]);
			oParam.StockSysNo = Util.TrimIntNull(tempdr["StockSysNo"]);
			oParam.LendUserSysNo = Util.TrimIntNull(tempdr["LendUserSysNo"]);
			oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
			oParam.CreateUserSysNo = Util.TrimIntNull(tempdr["CreateUserSysNo"]);
			oParam.AuditTime = Util.TrimDateNull(tempdr["AuditTime"]);
			oParam.AuditUserSysNo = Util.TrimIntNull(tempdr["AuditUserSysNo"]);
			oParam.OutTime = Util.TrimDateNull(tempdr["OutTime"]);
			oParam.OutUserSysNo = Util.TrimIntNull(tempdr["OutUserSysNo"]);
			oParam.Status = Util.TrimIntNull(tempdr["Status"]);
			oParam.Note = Util.TrimNull(tempdr["Note"]);
		}
		private void map(LendItemInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.LendSysNo = Util.TrimIntNull(tempdr["LendSysNo"]);
			oParam.ProductSysNo = Util.TrimIntNull(tempdr["ProductSysNo"]);
			oParam.LendQty = Util.TrimIntNull(tempdr["LendQty"]);
			oParam.ExpectReturnTime = Util.TrimDateNull(tempdr["ExpectReturnTime"]);
		}
		private void map(LendReturnInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.LendSysNo = Util.TrimIntNull(tempdr["LendSysNo"]);
			oParam.ProductSysNo = Util.TrimIntNull(tempdr["ProductSysNo"]);
			oParam.ReturnQty = Util.TrimIntNull(tempdr["ReturnQty"]);
			oParam.ReturnTime = Util.TrimDateNull(tempdr["ReturnTime"]);
		}
		#endregion

		#region  getID 、getCurrentStatus
		private string getLendID(int sysNo)
		{
			return "55" + sysNo.ToString().PadLeft(8,'0');
		}
		private int getCurrentStatus(int lendSysNo)
		{
			int status = AppConst.IntNull;

			string sql = "select status from st_lend where sysno =" + lendSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			
			if ( Util.HasMoreRow(ds))
				status = Util.TrimIntNull( ds.Tables[0].Rows[0]["status"] );

			return status;
		}
		private int CalcReturnStatus(LendInfo oParam)
		{
			if ( oParam.itemHash.Count == 0)
				throw new BizException("no lend item record");
			if ( oParam.returnHash.Count == 0)
				return (int)AppEnum.LendStatus.OutStock;

			//oParam.itemHash.Count > oParam.returnHash.Count 如果增加商品种类判断可以优化，暂时不做了

			foreach(LendItemInfo itemInfo in oParam.itemHash.Values)
			{
				int lendqty = itemInfo.LendQty;
				int returnqty = 0;
				foreach(LendReturnInfo returnInfo in oParam.returnHash.Values)
				{
					if ( itemInfo.ProductSysNo == returnInfo.ProductSysNo)
						returnqty += returnInfo.ReturnQty;
				}
				if ( lendqty > returnqty )
					return (int)AppEnum.LendStatus.ReturnPartly;
			}
			return (int)AppEnum.LendStatus.ReturnAll;
		}
		#endregion

		public void Create(LendInfo oParam)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				oParam.SysNo = SequenceDac.GetInstance().Create("St_Lend_Sequence");
				oParam.LendID = getLendID(oParam.SysNo);
				//建立主表记录
				int rowsAffected = new LendDac().InsertMaster(oParam);
				if(rowsAffected != 1)
					throw new BizException("insert lend master error");
				foreach( LendItemInfo item in oParam.itemHash.Values)
				{
					item.LendSysNo = oParam.SysNo;

					rowsAffected = new LendDac().InsertItem(item);
					if ( rowsAffected != 1)
						throw new BizException("insert lend item error");
					InventoryManager.GetInstance().SetAvailableQty(oParam.StockSysNo, item.ProductSysNo, item.LendQty);
				}

				scope.Complete();
            }
		}
		public void UpdateMaster(LendInfo oParam)
		{
			//主项可以更新 lenduser, note
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				
				//必须是初始状态
				if ( getCurrentStatus(oParam.SysNo) != (int)AppEnum.LendStatus.Origin )
					throw new BizException("status is not origin now,  update failed");

				//设置 单号, 可以修改借货人、备注
				Hashtable ht = new Hashtable(3);
				ht.Add("SysNo", oParam.SysNo);
				ht.Add("LendUserSysNo", oParam.LendUserSysNo);
				ht.Add("Note", oParam.Note);
				if ( 1!=new LendDac().UpdateMaster(ht))
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

				//必须是初始状态
				if ( getCurrentStatus(masterSysNo) != (int)AppEnum.LendStatus.Origin )
					throw new BizException("status is not origin now ,  verify failed");

				//设置 单号、状态和审核人
				Hashtable ht = new Hashtable(4);

				ht.Add("SysNo", masterSysNo);
				ht.Add("Status", (int)AppEnum.LendStatus.Verified);
				ht.Add("AuditTime", DateTime.Now);
				ht.Add("AuditUserSysNo", userSysNo);
				if ( 1!=new LendDac().UpdateMaster(ht))
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

				LendInfo masterInfo = Load(masterSysNo);
				
				//必须是初始状态
				if ( masterInfo.Status != (int)AppEnum.LendStatus.Origin )
					throw new BizException("status is not origin now ,  abandon failed");

				//设置 单号、状态
				Hashtable ht = new Hashtable(4);
				ht.Add("SysNo", masterSysNo);
				ht.Add("Status", (int)AppEnum.LendStatus.Abandon);
				if ( 1!=new LendDac().UpdateMaster(ht))
					throw new BizException("expected one-row update failed, abandon failed ");

				//取消对available数量的占用
				foreach(LendItemInfo item in masterInfo.itemHash.Values)
				{
					InventoryManager.GetInstance().SetAvailableQty(masterInfo.StockSysNo, item.ProductSysNo, -1*item.LendQty);

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

				LendInfo masterInfo = Load(masterSysNo);
				
				//必须是废弃状态
				if ( masterInfo.Status != (int)AppEnum.LendStatus.Abandon )
					throw new BizException("status is not abandon now ,  cancel abandon failed");

				//设置 单号、状态
				Hashtable ht = new Hashtable(4);
				ht.Add("SysNo", masterSysNo);
				ht.Add("Status", (int)AppEnum.LendStatus.Origin);
				if ( 1!=new LendDac().UpdateMaster(ht))
					throw new BizException("expected one-row update failed, cancel abandon failed ");

				//增加对available数量的占用
				foreach(LendItemInfo item in masterInfo.itemHash.Values)
				{
					InventoryManager.GetInstance().SetAvailableQty(masterInfo.StockSysNo, item.ProductSysNo, item.LendQty);

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

				LendInfo masterInfo = Load(masterSysNo);
				
				//必须是已审核状态
				if ( masterInfo.Status != (int)AppEnum.LendStatus.Verified )
					throw new BizException("status is not verified now,  cancel verify failed");

				//设置 单号、状态
				Hashtable ht = new Hashtable(4);
				ht.Add("SysNo", masterInfo.SysNo);
				ht.Add("Status", (int)AppEnum.LendStatus.Origin);
				ht.Add("AuditTime", DateTime.Now);
				ht.Add("AuditUserSysNo", userSysNo);
				if ( 1!=new LendDac().UpdateMaster(ht))
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

				LendInfo masterInfo = Load(masterSysNo);
				
				//必须是已审核
				if ( masterInfo.Status != (int)AppEnum.LendStatus.Verified )
					throw new BizException("status is not verify now,  outstock failed");


				//设置 单号、状态
				Hashtable ht = new Hashtable(4);
				ht.Add("SysNo", masterInfo.SysNo);
				ht.Add("Status", (int)AppEnum.LendStatus.OutStock);
				ht.Add("OutTime", DateTime.Now);
				ht.Add("OutUserSysNo", userSysNo);
				if ( 1!=new LendDac().UpdateMaster(ht))
					throw new BizException("expected one-row update failed, outstock failed ");

				//库存设定
				foreach(LendItemInfo item in masterInfo.itemHash.Values)
				{
					InventoryManager.GetInstance().SetOutStockQty(masterInfo.StockSysNo, item.ProductSysNo, item.LendQty);

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

				LendInfo masterInfo = Load(masterSysNo);
				
				//必须是已出库
				if ( masterInfo.Status != (int)AppEnum.LendStatus.OutStock )
					throw new BizException("status is not outstock now,  cancel outstock failed");


				//设置 单号、状态
				Hashtable ht = new Hashtable(4);
				ht.Add("SysNo", masterInfo.SysNo);
				ht.Add("Status", (int)AppEnum.LendStatus.Origin);
				ht.Add("OutTime", DateTime.Now);
				ht.Add("OutUserSysNo", userSysNo);
				if ( 1!=new LendDac().UpdateMaster(ht))
					throw new BizException("expected one-row update failed, cancel outstock failed ");

				//库存设定
				foreach(LendItemInfo item in masterInfo.itemHash.Values)
				{
					InventoryManager.GetInstance().SetOutStockQty(masterInfo.StockSysNo, item.ProductSysNo, -1*item.LendQty);

				}
				scope.Complete();
            }
		}

		public void UpdateItemQty(LendInfo masterInfo, LendItemInfo itemInfo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				
				//必须是初始
				if ( getCurrentStatus(masterInfo.SysNo) != (int)AppEnum.LendStatus.Origin )
					throw new BizException("status is not origin now,  update item qty failed");

				//获取数量差值
				LendItemInfo oldItemInfo = masterInfo.itemHash[itemInfo.ProductSysNo] as LendItemInfo;
				int deltQty = itemInfo.LendQty - oldItemInfo.LendQty;

				//更新表单明细 ( 如果增加表单明细项的属性，需要在这里处理一下）
				itemInfo.SysNo = oldItemInfo.SysNo;
				itemInfo.LendSysNo = oldItemInfo.LendSysNo;

				if ( 1 != new LendDac().UpdateItem(itemInfo.SysNo, deltQty, itemInfo.ExpectReturnTime))
					throw new BizException("expected one-row update failed, update item qty failed");


				//更新库存
				InventoryManager.GetInstance().SetAvailableQty(masterInfo.StockSysNo, itemInfo.ProductSysNo, deltQty);

				//更新 itemInfo 到 masterInfo 注:数据库更新成功以后才更新类
				masterInfo.itemHash.Remove(itemInfo.ProductSysNo);
				masterInfo.itemHash.Add(itemInfo.ProductSysNo, itemInfo);

				scope.Complete();
            }
			
		}

		public void InsertItem(LendInfo masterInfo, LendItemInfo itemInfo)
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
				if ( getCurrentStatus(masterInfo.SysNo) != (int)AppEnum.LendStatus.Origin )
					throw new BizException("status is not origin now,  add item failed");

				//获取数量差值

				//更新item
				if ( 1 != new LendDac().InsertItem(itemInfo))
					throw new BizException("expected one-row update failed, add item failed");

				//更新库存
				InventoryManager.GetInstance().SetAvailableQty(masterInfo.StockSysNo, itemInfo.ProductSysNo, itemInfo.LendQty);

				//更新 itemInfo 到 masterInfo
				masterInfo.itemHash.Add(itemInfo.ProductSysNo, itemInfo);

				scope.Complete();
            }
		}
		public void DeleteItem(LendInfo masterInfo, int itemProductSysNo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				
				//必须是初始
				if ( getCurrentStatus(masterInfo.SysNo) != (int)AppEnum.LendStatus.Origin )
					throw new BizException("status is not origin now,  delete item failed");

				//获取数量差值
				LendItemInfo oldItemInfo = masterInfo.itemHash[itemProductSysNo] as LendItemInfo;
				int deltQty = -1*oldItemInfo.LendQty;

				//更新item
				if ( 1 != new LendDac().DeleteItem(oldItemInfo.SysNo))
					throw new BizException("expected one-row update failed, delete item qty failed");

				//更新库存
				InventoryManager.GetInstance().SetAvailableQty(masterInfo.StockSysNo, itemProductSysNo, deltQty);

				//更新 masterInfo
				masterInfo.itemHash.Remove(itemProductSysNo);

				scope.Complete();
            }
		}


		public void InsertReturn(LendInfo masterInfo, LendReturnInfo returnInfo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				
				//必须是出库状态
				if ( getCurrentStatus(masterInfo.SysNo) != (int)AppEnum.LendStatus.OutStock 
					&& getCurrentStatus(masterInfo.SysNo) != (int)AppEnum.LendStatus.ReturnPartly)
					throw new BizException("status is not outstock or returnpartly now,  return failed");

				//获取数量差值
				LendItemInfo oldItemInfo = masterInfo.itemHash[returnInfo.ProductSysNo] as LendItemInfo;
				int alreadyReturnQty = 0;
				foreach(LendReturnInfo item in masterInfo.returnHash.Values)
				{
					if ( item.ProductSysNo == returnInfo.ProductSysNo )
					{
						alreadyReturnQty += item.ReturnQty;
					}
				}
				if ( oldItemInfo.LendQty - alreadyReturnQty < returnInfo.ReturnQty )
					throw new BizException("you don't need to return so much");

				//更新item
				if ( 1 != new LendDac().InsertReturn(returnInfo))
					throw new BizException("expected one-row update failed, update item qty failed");

				//更新库存
				InventoryManager.GetInstance().SetInStockQty(masterInfo.StockSysNo, returnInfo.ProductSysNo, returnInfo.ReturnQty);

				//更新 returnInfo 到 masterInfo
				masterInfo.returnHash.Add(returnInfo.SysNo, returnInfo);

				//设置单号、状态
				Hashtable ht = new Hashtable(2);
				ht.Add("SysNo", masterInfo.SysNo);
				ht.Add("Status", CalcReturnStatus(masterInfo));
				if ( 1!=new LendDac().UpdateMaster(ht))
					throw new BizException("expected one-row update failed, cancel outstock failed ");

				scope.Complete();
            }

		}
		public void DeleteReturn(LendInfo masterInfo, int returnSysNo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				
				//必须是出库状态
				if ( getCurrentStatus(masterInfo.SysNo) != (int)AppEnum.LendStatus.ReturnAll 
					&& getCurrentStatus(masterInfo.SysNo) != (int)AppEnum.LendStatus.ReturnPartly)
					throw new BizException("status is not return or returnpartly now,  delete return failed");

				//获取数量差值
				LendReturnInfo oldReturnInfo = masterInfo.returnHash[returnSysNo] as LendReturnInfo;
				int deltQty = -1*oldReturnInfo.ReturnQty;

				//更新item
				if ( 1 != new LendDac().DeleteReturn(returnSysNo))
					throw new BizException("expected one-row update failed, delete return failed");

				//更新库存
				InventoryManager.GetInstance().SetInStockQty(masterInfo.StockSysNo, oldReturnInfo.ProductSysNo, deltQty);

				//更新 returnInfo 到 masterInfo
				masterInfo.returnHash.Remove(oldReturnInfo.SysNo);

				//设置单号、状态
				Hashtable ht = new Hashtable(2);
				ht.Add("SysNo", masterInfo.SysNo);
				ht.Add("Status", CalcReturnStatus(masterInfo));
				if ( 1!=new LendDac().UpdateMaster(ht))
					throw new BizException("expected one-row update failed, cancel outstock failed ");

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
			
			string sql = " select top 1 sysno from st_lend";
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds) )
				throw new BizException("the table lend is not empty");

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				string sql_old = @"select 
								old.sysno, old.lendid, stock_con.newsysno as stocksysno,
								lender_con.newsysno as lendusersysno,
								create_con.newsysno as createusersysno,
								audit_con.newsysno as auditusersysno,
								out_con.newsysno as outusersysno,
								createtime, audittime,outstocktime as outtime,
								productstatus, auditstatus, memo as note, '1' as status
							from 
								ipp2003..st_lend as old, 
								ippconvert..sys_user as lender_con,
								ippconvert..sys_user as create_con,
								ippconvert..sys_user as audit_con,
								ippconvert..sys_user as out_con,
								ippconvert..stock as stock_con
							where 
								old.employeesysno = lender_con.oldsysno and
								old.createusersysno = create_con.oldsysno and
								old.auditusersysno = audit_con.oldsysno and
								old.outstockusersysno = out_con.oldsysno and
								old.stocksysno = stock_con.oldsysno
								order by old.sysno";
				DataSet ds_old = SqlHelper.ExecuteDataSet(sql_old);
				if ( !Util.HasMoreRow(ds_old) )
					return;
				foreach(DataRow dr in ds_old.Tables[0].Rows)
				{
					/* newStatus	aduit	product
					 * abandon		-1		n/a
					 * origin		0		n/a
					 * verified		1		n/a
					 * outstock		n/a		1
					 * returnpartly	n/a		2
					 * returnall	n/a		3
					 */	
					int newStatus = (int)AppEnum.LendStatus.Origin;
					int auditStatus = Util.TrimIntNull(dr["auditStatus"]);
					int productStatus = Util.TrimIntNull(dr["productStatus"]);
					if ( auditStatus == -1 )
						newStatus = (int)AppEnum.LendStatus.Abandon;
					if ( auditStatus == 0 )
						newStatus = (int)AppEnum.LendStatus.Origin;
					if ( auditStatus == 1)
						newStatus = (int)AppEnum.LendStatus.Verified;
					if ( productStatus == 1)
						newStatus = (int)AppEnum.LendStatus.OutStock;
					if ( productStatus == 2)
						newStatus = (int)AppEnum.LendStatus.ReturnPartly;
					if ( productStatus == 3)
						newStatus = (int)AppEnum.LendStatus.ReturnAll;

					
					LendInfo  oInfo = new LendInfo();
					map(oInfo, dr);
					oInfo.Status = newStatus;

					if ( new LendDac().ImportMaster(oInfo)!= 1)
					{
						throw new BizException("master expected one row error");
					}

					//insert item
					string sql_item = @"select '0' as sysno,
										LendSysNo, con_product.newsysno as productsysno, lendqty, returnqty, restoretime as ExpectReturnTime
									from 
										ipp2003..St_Lend_Item sli, ippconvert..productbasic as con_product
									where sli.productsysno = con_product.oldsysno and LendSysNo=" + oInfo.SysNo;

					DataSet ds_item = SqlHelper.ExecuteDataSet(sql_item);
					if ( !Util.HasMoreRow(ds_item) )
						continue;
					foreach(DataRow drItem in ds_item.Tables[0].Rows)
					{
						LendItemInfo oItem = new LendItemInfo();
						map(oItem, drItem);
						int resultitem = new LendDac().InsertItem(oItem);
						if ( resultitem < 1 )
							throw new BizException("insert item row < 1");

						
						if ( Util.TrimIntNull(drItem["returnqty"])!= 0)
						{
							LendReturnInfo oReturn = new LendReturnInfo();
							oReturn.LendSysNo = oItem.LendSysNo;
							oReturn.ProductSysNo = oItem.ProductSysNo;
							oReturn.ReturnTime = oInfo.OutTime.AddDays(30);
							oReturn.ReturnQty = Util.TrimIntNull(drItem["returnqty"]);
							int resultReturn = new LendDac().InsertReturn(oReturn);
							if ( resultReturn != 1)
								throw new BizException("return item one-row expected error");
						}

						
						if ( oItem.LendQty - Util.TrimIntNull(drItem["returnqty"]) != 0 )
							InventoryManager.GetInstance().SetImportLendQty(oInfo.StockSysNo, oItem.ProductSysNo, oItem.LendQty-Util.TrimIntNull(drItem["returnqty"]));

						//调整库存
						if ( oInfo.Status == (int)AppEnum.LendStatus.Origin || oInfo.Status == (int)AppEnum.LendStatus.Verified)
						{
							InventoryManager.GetInstance().SetAvailableQty(oInfo.StockSysNo, oItem.ProductSysNo, oItem.LendQty);
						}
					}
				}

				string sqlMaxSysNo = "select top 1 sysno from ipp2003..st_lend order by sysno desc";
				DataSet dsMax = SqlHelper.ExecuteDataSet(sqlMaxSysNo);
				if ( !Util.HasMoreRow(dsMax))
					throw new BizException("got max sysno error");
				int maxSysNo = Util.TrimIntNull(dsMax.Tables[0].Rows[0]["sysno"]);
				// 将自动生成的sysno填到目前的最大单据号
				int newSysNo;
				do 
				{
					newSysNo = SequenceDac.GetInstance().Create("St_Lend_Sequence");
				} while( newSysNo < maxSysNo);
				
				
			scope.Complete();
            }

		}
	}
}
