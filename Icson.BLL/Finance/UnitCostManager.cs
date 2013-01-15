using System;

using Icson.Utils;
using System.Transactions;

using Icson.Objects.Basic;

using Icson.DBAccess.Basic;

using Icson.BLL.Basic;


namespace Icson.BLL.Finance
{
	/// <summary>
	/// Summary description for UnitCostManager.
	/// </summary>
	public class UnitCostManager
	{
		private UnitCostManager()
		{
		}
		private static UnitCostManager _instance;
		public static UnitCostManager GetInstance()
		{
			if( _instance == null )
			{
				_instance = new UnitCostManager();
			}
			return _instance;
		}
		/// <summary>
		/// 在修改库存以前完成，否则当前库存数就不对了。
		/// </summary>
		/// <param name="productSysNo">商品编号</param>
		/// <param name="hereQty">本次数量</param>
		/// <param name="hereCost">本次成本</param>
		public void SetCost(int productSysNo, int hereQty, decimal hereCost)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				//获取该商品的当前成本
				InventoryManager.GetInstance().InitInventory(productSysNo);
				InventoryInfo oInv = InventoryManager.GetInstance().LoadInventory(productSysNo);
				ProductPriceInfo oPrice = ProductManager.GetInstance().LoadPrice(productSysNo);

				int curQty = oInv.AccountQty;
				decimal curCost = oPrice.UnitCost;

				//如果当前成本等于本次成本不用更新了
                if (curCost == hereCost)
                {
                    scope.Complete();
                    return;
                }

				decimal newCost = AppConst.DecimalNull;

				//如果数量为零或者计算出来的成本为负，用本次成本更新
				if ( curQty + hereQty == 0 )
					newCost = hereCost;
				else
				{
					newCost = Decimal.Round( (curQty*curCost + hereQty*hereCost) / ( curQty+hereQty)*1.0M, 2);
					if ( newCost < 0 )
						newCost = hereCost;
				}

				//如果计算出来相等也不用更新
                if (newCost == curCost)
                {
                    scope.Complete();
                    return;
                }

				if ( newCost == AppConst.DecimalNull )
					throw new BizException("calc cost error");

				//更新成本到数据库
				if ( 1!=new ProductPriceDac().UpdateCost(productSysNo, newCost))
					throw new BizException("expected one-row update failed, cancel verify failed ");
				scope.Complete();
            }
		}
	}
}