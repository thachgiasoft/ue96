using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Transactions;
using Icson.DBAccess;
using Icson.DBAccess.Basic;
using Icson.DBAccess.Online;
using Icson.Objects.Basic;
using Icson.Utils;

namespace Icson.BLL.Online
{
    public class ProductReviewManager
    {
        private ProductReviewManager()
		{
		}
		private static ProductReviewManager _instance;
		public static ProductReviewManager GetInstance()
		{
			if( _instance == null )
			{
				_instance = new ProductReviewManager();
			}
			return _instance;
		}

        public static string GetProductReviewTableName(int productSysNo)
        {
            if (0 < productSysNo && productSysNo < 3000)
            {
                return "Product_Review1";
            }
            else if (3000 <= productSysNo && productSysNo < 6000)
            {
                return "Product_Review2";
            }
            else if (6000 <= productSysNo && productSysNo < 9000)
            {
                return "Product_Review3";
            }
            else
            {
                return "Product_Review4";
            }
        }

        private void map(Category3ReviewItemInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.C3SysNo = Util.TrimIntNull(tempdr["C3SysNo"]);
            oParam.ID = Util.TrimNull(tempdr["ID"]);
            oParam.Name = Util.TrimNull(tempdr["Name"]);
            oParam.Description = Util.TrimNull(tempdr["Description"]);
            oParam.Weight = Util.TrimIntNull(tempdr["Weight"]);
            oParam.OrderNum = Util.TrimIntNull(tempdr["OrderNum"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
        }

        public void Insert(Category3ReviewItemInfo oParam)
		{
            string _reviewID = "";
            string _tmpStr = "";
            int _tmpInt = -1;

            string sql = "select max(id) from Category3_ReviewItem where C3SysNo=" + oParam.C3SysNo.ToString();
            _tmpStr = SqlHelper.ExecuteScalar(sql).ToString();

            if (_tmpStr.Trim() == "")
                _tmpStr = "0";

            if (!Util.IsInteger(_tmpStr))
                throw new BizException("对不起，生成编号错误，不能添加");
            _tmpInt = Convert.ToInt32(_tmpStr);
            _tmpInt++;
            _reviewID = _tmpInt.ToString();

            oParam.ID = _reviewID;

			new ProductReviewDac().Insert(oParam);
		}

        public void Update(Category3ReviewItemInfo oParam)
        {
            new ProductReviewDac().Update(oParam);
        }

        public void Delete(int reviewSysNo)
        {
            new ProductReviewDac().Delete(reviewSysNo);
        }

        public int GetCatetory3ReviewItemNewOrderNum(Category3ReviewItemInfo oParam)
        {
            return new ProductReviewDac().GetCatetory3ReviewItemNewOrderNum(oParam);
        }

        public Category3ReviewItemInfo LoadCategory3ReviewItem(int paramSysNo)
        {
            string sql = "select * from category3_reviewitem where sysno=" + paramSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;
            Category3ReviewItemInfo item = new Category3ReviewItemInfo();
            map(item, ds.Tables[0].Rows[0]);
            return item;
        }

        public SortedList GetCategory3ReviewItemList(int paramC3SysNo)
        {
            string sql = " select * from category3_reviewitem where c3sysno = " + paramC3SysNo + " order by ordernum";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;
            SortedList sl = new SortedList(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Category3ReviewItemInfo item = new Category3ReviewItemInfo();
                map(item, dr);
                sl.Add(item, null);
            }
            return sl;
        }

        public void MoveTop(Category3ReviewItemInfo oParam)
        {
            if (oParam.OrderNum == 1)
            {
                throw new BizException("it's the top one already");
            }
            SortedList sl = GetCategory3ReviewItemList(oParam.C3SysNo);
            if (sl == null)
            {
                throw new BizException("no item for this third category");
            }

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                ProductReviewDac o = new ProductReviewDac();

                foreach (Category3ReviewItemInfo item in sl.Keys)
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
        public void MoveUp(Category3ReviewItemInfo oParam)
        {
            if (oParam.OrderNum == 1)
            {
                throw new BizException("it's the first one, can't be moved up");
            }
            SortedList sl = GetCategory3ReviewItemList(oParam.C3SysNo);
            if (sl == null)
            {
                throw new BizException("no attributes");
            }

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                ProductReviewDac o = new ProductReviewDac();

                foreach (Category3ReviewItemInfo item in sl.Keys)
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
        public void MoveDown(Category3ReviewItemInfo oParam)
        {
            SortedList sl = GetCategory3ReviewItemList(oParam.C3SysNo);
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

                ProductReviewDac o = new ProductReviewDac();

                foreach (Category3ReviewItemInfo item in sl.Keys)
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
        public void MoveBottom(Category3ReviewItemInfo oParam)
        {
            SortedList sl = GetCategory3ReviewItemList(oParam.C3SysNo);
            if (sl == null)
            {
                throw new BizException("no attributes");
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

                ProductReviewDac o = new ProductReviewDac();

                foreach (Category3ReviewItemInfo item in sl.Keys)
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
    }
}
