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
		/// ���޸Ŀ����ǰ��ɣ�����ǰ������Ͳ����ˡ�
		/// </summary>
		/// <param name="productSysNo">��Ʒ���</param>
		/// <param name="hereQty">��������</param>
		/// <param name="hereCost">���γɱ�</param>
		public void SetCost(int productSysNo, int hereQty, decimal hereCost)
		{
			TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
				//��ȡ����Ʒ�ĵ�ǰ�ɱ�
				InventoryManager.GetInstance().InitInventory(productSysNo);
				InventoryInfo oInv = InventoryManager.GetInstance().LoadInventory(productSysNo);
				ProductPriceInfo oPrice = ProductManager.GetInstance().LoadPrice(productSysNo);

				int curQty = oInv.AccountQty;
				decimal curCost = oPrice.UnitCost;

				//�����ǰ�ɱ����ڱ��γɱ����ø�����
                if (curCost == hereCost)
                {
                    scope.Complete();
                    return;
                }

				decimal newCost = AppConst.DecimalNull;

				//�������Ϊ����߼�������ĳɱ�Ϊ�����ñ��γɱ�����
				if ( curQty + hereQty == 0 )
					newCost = hereCost;
				else
				{
					newCost = Decimal.Round( (curQty*curCost + hereQty*hereCost) / ( curQty+hereQty)*1.0M, 2);
					if ( newCost < 0 )
						newCost = hereCost;
				}

				//�������������Ҳ���ø���
                if (newCost == curCost)
                {
                    scope.Complete();
                    return;
                }

				if ( newCost == AppConst.DecimalNull )
					throw new BizException("calc cost error");

				//���³ɱ������ݿ�
				if ( 1!=new ProductPriceDac().UpdateCost(productSysNo, newCost))
					throw new BizException("expected one-row update failed, cancel verify failed ");
				scope.Complete();
            }
		}
	}
}