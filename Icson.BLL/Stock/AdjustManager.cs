using System;
using System.Data;
using System.Collections;
using System.Text;

using Icson.Utils;
using System.Transactions;
using Icson.Objects.Stock;
using Icson.DBAccess;
using Icson.DBAccess.Stock;
using Icson.BLL.Basic;
using Icson.Objects.Basic;
using Icson.Objects;

namespace Icson.BLL.Stock
{
	/// <summary>
	/// Summary description for AdjustManager.
	/// </summary>
	public class AdjustManager
	{
		private AdjustManager()
		{
		}

		private static AdjustManager _instance;
		public static AdjustManager GetInstance()
		{
			if( _instance == null )
			{
				_instance = new AdjustManager();
			}
			return _instance;
		}
		public DataSet GetAdjustListDs(Hashtable paramHash)
		{
			string sql = @" select st.*, b.username as CreateUserName, c.username as AuditUserName, d.username as OutUserName 
							from st_adjust st, sys_user b, sys_user c, sys_user d 
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
						sb.Append(" exists ( select top 1 sysno from st_adjust_item where st.sysno=st_adjust_item.adjustsysno and productsysno = ").Append(Util.SafeFormat(item.ToString())).Append(" ) ");
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
		public AdjustInfo Load(int adjustSysNo)
		{
			AdjustInfo masterInfo;

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				string masterSql = "select * from st_adjust where sysno = " + adjustSysNo;
				DataSet masterDs = SqlHelper.ExecuteDataSet(masterSql);
				if ( !Util.HasMoreRow(masterDs))
					throw new BizException("there is no this adjust sysno");

				masterInfo = new AdjustInfo();
				map(masterInfo, masterDs.Tables[0].Rows[0]);

				string itemSql = "select * from st_adjust_item where adjustsysno=" + adjustSysNo;
				DataSet itemDs = SqlHelper.ExecuteDataSet(itemSql);
				if ( Util.HasMoreRow(itemDs))
				{
					foreach(DataRow dr in itemDs.Tables[0].Rows)
					{
						AdjustItemInfo item = new AdjustItemInfo();
						map(item, dr);
						masterInfo.itemHash.Add(item.ProductSysNo, item);
					}
				}	
				scope.Complete();
            }

			return masterInfo;

		}
		#region map
		private void map(AdjustInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.AdjustID = Util.TrimNull(tempdr["AdjustID"]);
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
		private void map(AdjustItemInfo oParam, DataRow tempdr)
		{
			oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
			oParam.AdjustSysNo = Util.TrimIntNull(tempdr["AdjustSysNo"]);
			oParam.ProductSysNo = Util.TrimIntNull(tempdr["ProductSysNo"]);
			oParam.AdjustQty = Util.TrimIntNull(tempdr["AdjustQty"]);
			oParam.AdjustCost = Util.TrimDecimalNull(tempdr["AdjustCost"]);
		}
		#endregion

		#region  getID ��getCurrentStatus
		private string getAdjustID(int sysNo)
		{
			return "56" + sysNo.ToString().PadLeft(8,'0');
		}
		private int getCurrentStatus(int adjustSysNo)
		{
			int status = AppConst.IntNull;

			string sql = "select status from st_adjust where sysno =" + adjustSysNo;
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			
			if ( Util.HasMoreRow(ds))
				status = Util.TrimIntNull( ds.Tables[0].Rows[0]["status"] );

			return status;
		}
		#endregion

		public void Create(AdjustInfo oParam)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				oParam.SysNo = SequenceDac.GetInstance().Create("St_Adjust_Sequence");
				oParam.AdjustID = getAdjustID(oParam.SysNo);
				//���������¼
				int rowsAffected = new AdjustDac().InsertMaster(oParam);
				if(rowsAffected != 1)
					throw new BizException("insert adjust master error");
				foreach( AdjustItemInfo item in oParam.itemHash.Values)
				{
					item.AdjustSysNo = oParam.SysNo;

					rowsAffected = new AdjustDac().InsertItem(item);
					if ( rowsAffected != 1)
						throw new BizException("insert adjust item error");
					if ( item.AdjustQty < 0 )
					{//��Ҫռ�ÿ��
						InventoryManager.GetInstance().SetAvailableQty(oParam.StockSysNo, item.ProductSysNo, -1*item.AdjustQty);
					}
					
				}

				scope.Complete();
            }
		}

		public void UpdateMaster(AdjustInfo oParam)
		{
			//������Ը���note
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				
				//�����ǳ�ʼ״̬
				if ( getCurrentStatus(oParam.SysNo) != (int)AppEnum.AdjustStatus.Origin )
					throw new BizException("status is not origin now,  update failed");

				//���� ����, ��ע
				Hashtable ht = new Hashtable(3);
				ht.Add("SysNo", oParam.SysNo);
				ht.Add("Note", oParam.Note);
				if ( 1!=new AdjustDac().UpdateMaster(ht))
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

				//�����ǳ�ʼ״̬
				if ( getCurrentStatus(masterSysNo) != (int)AppEnum.AdjustStatus.Origin )
					throw new BizException("status is not origin now ,  verify failed");

				//���� ���š�״̬�������
				Hashtable ht = new Hashtable(4);

				ht.Add("SysNo", masterSysNo);
				ht.Add("Status", (int)AppEnum.AdjustStatus.Verified);
				ht.Add("AuditTime", DateTime.Now);
				ht.Add("AuditUserSysNo", userSysNo);
				if ( 1!=new AdjustDac().UpdateMaster(ht))
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

				AdjustInfo masterInfo = Load(masterSysNo);
				
				//�����ǳ�ʼ״̬
				if ( masterInfo.Status != (int)AppEnum.AdjustStatus.Origin )
					throw new BizException("status is not origin now ,  abandon failed");

				//���� ���š�״̬
				Hashtable ht = new Hashtable(4);
				ht.Add("SysNo", masterSysNo);
				ht.Add("Status", (int)AppEnum.AdjustStatus.Abandon);
				if ( 1!=new AdjustDac().UpdateMaster(ht))
					throw new BizException("expected one-row update failed, abandon failed ");

				//ȡ����available������ռ��
				foreach(AdjustItemInfo item in masterInfo.itemHash.Values)
				{
					if ( item.AdjustQty < 0 ) // <0 �Ż������ɵ�ʱ��ռ��
					{
						InventoryManager.GetInstance().SetAvailableQty(masterInfo.StockSysNo, item.ProductSysNo, item.AdjustQty);
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

				AdjustInfo masterInfo = Load(masterSysNo);
				
				//�����Ƿ���״̬
				if ( masterInfo.Status != (int)AppEnum.AdjustStatus.Abandon )
					throw new BizException("status is not abandon now ,  cancel abandon failed");

				//���� ���š�״̬
				Hashtable ht = new Hashtable(4);
				ht.Add("SysNo", masterSysNo);
				ht.Add("Status", (int)AppEnum.AdjustStatus.Origin);
				if ( 1!=new AdjustDac().UpdateMaster(ht))
					throw new BizException("expected one-row update failed, cancel abandon failed ");

				//���Ӷ�available������ռ��
				foreach(AdjustItemInfo item in masterInfo.itemHash.Values)
				{
					if ( item.AdjustQty < 0 )
					{
						InventoryManager.GetInstance().SetAvailableQty(masterInfo.StockSysNo, item.ProductSysNo, -1*item.AdjustQty);
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

				AdjustInfo masterInfo = Load(masterSysNo);
				
				//�����������״̬
				if ( masterInfo.Status != (int)AppEnum.AdjustStatus.Verified )
					throw new BizException("status is not verified now,  cancel verify failed");

				//���� ���š�״̬
				Hashtable ht = new Hashtable(4);
				ht.Add("SysNo", masterInfo.SysNo);
				ht.Add("Status", (int)AppEnum.AdjustStatus.Origin);
				ht.Add("AuditTime", DateTime.Now);
				ht.Add("AuditUserSysNo", userSysNo);
				if ( 1!=new AdjustDac().UpdateMaster(ht))
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

				AdjustInfo masterInfo = Load(masterSysNo);
				
				//�����������
				if ( masterInfo.Status != (int)AppEnum.AdjustStatus.Verified )
					throw new BizException("status is not verify now,  outstock failed");


				//���� ���š�״̬
				Hashtable ht = new Hashtable(4);
				ht.Add("SysNo", masterInfo.SysNo);
				ht.Add("Status", (int)AppEnum.AdjustStatus.OutStock);
				ht.Add("OutTime", DateTime.Now);
				ht.Add("OutUserSysNo", userSysNo);
				if ( 1!=new AdjustDac().UpdateMaster(ht))
					throw new BizException("expected one-row update failed, outstock failed ");

				
				foreach(AdjustItemInfo item in masterInfo.itemHash.Values)
				{
					//cost�趨
					ProductPriceInfo priceInfo = ProductManager.GetInstance().LoadPrice(item.ProductSysNo);
					item.AdjustCost = priceInfo.UnitCost;
					new AdjustDac().UpdateItemCost(item.SysNo, item.AdjustCost);
					//����趨
					if ( item.AdjustQty < 0 )
					{
						InventoryManager.GetInstance().SetOutStockQty(masterInfo.StockSysNo, item.ProductSysNo, -1*item.AdjustQty);
					}
					else
					{
						InventoryManager.GetInstance().SetInStockQty(masterInfo.StockSysNo, item.ProductSysNo, item.AdjustQty);
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

				AdjustInfo masterInfo = Load(masterSysNo);
				
				//�������ѳ���
				if ( masterInfo.Status != (int)AppEnum.AdjustStatus.OutStock )
					throw new BizException("status is not outstock now,  cancel outstock failed");


				//���� ���š�״̬
				Hashtable ht = new Hashtable(4);
				ht.Add("SysNo", masterInfo.SysNo);
				ht.Add("Status", (int)AppEnum.AdjustStatus.Origin);
				ht.Add("OutTime", DateTime.Now);
				ht.Add("OutUserSysNo", userSysNo);
				if ( 1!=new AdjustDac().UpdateMaster(ht))
					throw new BizException("expected one-row update failed, cancel outstock failed ");

				
				foreach(AdjustItemInfo item in masterInfo.itemHash.Values)
				{
					//cost�趨
					//ȡ�����⣬���趨cost�ˣ�Ӱ���С

					//����趨
					if ( item.AdjustQty < 0 )
					{
						InventoryManager.GetInstance().SetOutStockQty(masterInfo.StockSysNo, item.ProductSysNo, item.AdjustQty);
					}
					else
					{
						InventoryManager.GetInstance().SetInStockQty(masterInfo.StockSysNo, item.ProductSysNo, -1*item.AdjustQty);
					}

				}
				scope.Complete();
            }
		}
		public void UpdateItemQty(AdjustInfo masterInfo, AdjustItemInfo itemInfo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				
				//�����ǳ�ʼ
				if ( getCurrentStatus(masterInfo.SysNo) != (int)AppEnum.AdjustStatus.Origin )
					throw new BizException("status is not origin now,  update item qty failed");

				//��ȡ������ֵ
				AdjustItemInfo oldItemInfo = masterInfo.itemHash[itemInfo.ProductSysNo] as AdjustItemInfo;

				
				/* 
				 * old     new
				 * >0      >0    ����Ҫ����
				 * <0      <0    new - old = delt
				 * <0      >0    0 - old = delt
				 * >0      <0    new - 0 = delt
				 */
				int newAllocatedQty =0;
				int oldQty = oldItemInfo.AdjustQty;
				int newQty = itemInfo.AdjustQty;
				int deltQty = newQty - oldQty;

				if ( oldQty <0 && newQty <0 )
					newAllocatedQty = newQty - oldQty;
				else if ( oldQty < 0 && newQty > 0 )
					newAllocatedQty = 0 - oldQty;
				else if ( oldQty >0 && newQty < 0 )
					newAllocatedQty = newQty - 0;


				//���±���ϸ ( ������ӱ���ϸ������ԣ���Ҫ�����ﴦ��һ�£�
				itemInfo.SysNo = oldItemInfo.SysNo;
				itemInfo.AdjustSysNo = oldItemInfo.AdjustSysNo;
				//��ȡ�ɱ�����Ҫ�Ĵ�ӡ������ã���Ϊ�����ʱ���ǻ���µ�
				ProductPriceInfo priceInfo = ProductManager.GetInstance().LoadPrice(itemInfo.ProductSysNo);
				itemInfo.AdjustCost = priceInfo.UnitCost;

				if ( 1 != new AdjustDac().UpdateItemQtyCost(itemInfo.SysNo, deltQty, itemInfo.AdjustCost))
					throw new BizException("expected one-row update failed, update item qty failed");
				

				//����ռ�úͿ��ÿ��
				//////////////////////////////////////////////////////////////////////////
				if ( newAllocatedQty != 0)
					InventoryManager.GetInstance().SetAvailableQty(masterInfo.StockSysNo, itemInfo.ProductSysNo, -1*newAllocatedQty);

				//���� itemInfo �� masterInfo ע:���ݿ���³ɹ��Ժ�Ÿ�����
				masterInfo.itemHash.Remove(itemInfo.ProductSysNo);
				masterInfo.itemHash.Add(itemInfo.ProductSysNo, itemInfo);

				scope.Complete();
            }	
		}

		public void InsertItem(AdjustInfo masterInfo, AdjustItemInfo itemInfo)
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
				
				//�����ǳ�ʼ
				if ( getCurrentStatus(masterInfo.SysNo) != (int)AppEnum.AdjustStatus.Origin )
					throw new BizException("status is not origin now,  add item failed");

				//��ȡ�ɱ�����Ҫ�Ĵ�ӡ������ã���Ϊ�����ʱ���ǻ���µ�
				ProductPriceInfo priceInfo = ProductManager.GetInstance().LoadPrice(itemInfo.ProductSysNo);
				itemInfo.AdjustCost = priceInfo.UnitCost;

				//����item
				if ( 1 != new AdjustDac().InsertItem(itemInfo))
					throw new BizException("expected one-row update failed, add item failed");

				//���¿��
				if ( itemInfo.AdjustQty < 0 )
				{
					InventoryManager.GetInstance().SetAvailableQty(masterInfo.StockSysNo, itemInfo.ProductSysNo, -1*itemInfo.AdjustQty);
				}

				//���� itemInfo �� masterInfo
				masterInfo.itemHash.Add(itemInfo.ProductSysNo, itemInfo);

				scope.Complete();
            }
		}
		public void DeleteItem(AdjustInfo masterInfo, int itemProductSysNo)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				
				//�����ǳ�ʼ
				if ( getCurrentStatus(masterInfo.SysNo) != (int)AppEnum.AdjustStatus.Origin )
					throw new BizException("status is not origin now,  delete item failed");

				//��ȡ������ֵ
				AdjustItemInfo oldItemInfo = masterInfo.itemHash[itemProductSysNo] as AdjustItemInfo;
				int deltQty = -1*oldItemInfo.AdjustQty;

				//����item
				if ( 1 != new AdjustDac().DeleteItem(oldItemInfo.SysNo))
					throw new BizException("expected one-row update failed, delete item qty failed");

				//���¿��
				if ( oldItemInfo.AdjustQty < 0)
				{
					InventoryManager.GetInstance().SetAvailableQty(masterInfo.StockSysNo, itemProductSysNo, -1*deltQty);
				}

				//���� masterInfo
				masterInfo.itemHash.Remove(itemProductSysNo);

				scope.Complete();
            }
		}

		public void Import()
		{
			/* �漰������ 
			 * 1 ������¼�Ĵ���
			 * 2 ���Ĵ���
			 * 3 ״̬�Ĵ���
			 * 4 ���е���id�Ķ�Ӧ�����Ҫ�ر�ע��
			 */
			if ( !AppConfig.IsImportable)
				throw new BizException("Is Importable is false");
			
			string sql = " select top 1 sysno from st_adjust";
			DataSet ds = SqlHelper.ExecuteDataSet(sql);
			if ( Util.HasMoreRow(ds) )
				throw new BizException("the table adjust is not empty");

			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

				string sql_old = @"select 
								old.sysno, old.adjustid, stock_con.newsysno as stocksysno,
								create_con.newsysno as createusersysno,
								audit_con.newsysno as auditusersysno,
								audit_con.newsysno as outusersysno,
								createtime, audittime,audittime as outtime,
								auditstatus, note, '1' as status
							from 
								ipp2003..st_adjust as old, 
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
					int newStatus = (int)AppEnum.AdjustStatus.Origin;
					int auditStatus = Util.TrimIntNull(dr["auditStatus"]);
					if ( auditStatus == -1 )
						newStatus = (int)AppEnum.AdjustStatus.Abandon;
					else if ( auditStatus == 0 )
						newStatus = (int)AppEnum.AdjustStatus.Origin;
					else if ( auditStatus == 1)
						newStatus = (int)AppEnum.AdjustStatus.OutStock;
					else
						throw new BizException("old adjust status out of range");

					
					AdjustInfo  oInfo = new AdjustInfo();
					map(oInfo, dr);
					oInfo.Status = newStatus;

					if ( new AdjustDac().ImportMaster(oInfo)!= 1)
					{
						throw new BizException("master expected one row error");
					}

					//insert item
					string sql_item = @"select '0' as sysno,
										AdjustSysNo, con_product.newsysno as productsysno, adjustqty, adjustcost
									from 
										ipp2003..St_Adjust_Item sti, ippconvert..productbasic as con_product
									where sti.productsysno = con_product.oldsysno and AdjustSysNo=" + oInfo.SysNo;

					DataSet ds_item = SqlHelper.ExecuteDataSet(sql_item);
					if ( !Util.HasMoreRow(ds_item) )
						continue;
					foreach(DataRow drItem in ds_item.Tables[0].Rows)
					{
						AdjustItemInfo oItem = new AdjustItemInfo();
						map(oItem, drItem);

						if ( oItem.AdjustCost == 0 )
							oItem.AdjustCost = AppConst.IntNull;

						int resultitem = new AdjustDac().InsertItem(oItem);
						if ( resultitem < 1 )
							throw new BizException("insert item row < 1");

						//�������
						if ( oInfo.Status == (int)AppEnum.AdjustStatus.Origin && oItem.AdjustQty < 0 )
						{
							InventoryManager.GetInstance().SetAvailableQty(oInfo.StockSysNo, oItem.ProductSysNo, -1*oItem.AdjustQty);
						}
					}
				}

				string sqlMaxSysNo = "select top 1 sysno from ipp2003..st_adjust order by sysno desc";
				DataSet dsMax = SqlHelper.ExecuteDataSet(sqlMaxSysNo);
				if ( !Util.HasMoreRow(dsMax))
					throw new BizException("got max sysno error");
				int maxSysNo = Util.TrimIntNull(dsMax.Tables[0].Rows[0]["sysno"]);
				// ���Զ����ɵ�sysno�Ŀǰ����󵥾ݺ�
				int newSysNo;
				do 
				{
					newSysNo = SequenceDac.GetInstance().Create("St_Adjust_Sequence");
				} while( newSysNo < maxSysNo);
				
				
			scope.Complete();
            }

		}
	}
}
