using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Transactions;

using Icson.DBAccess;
using Icson.Utils;
using Icson.DBAccess.Sale;
using Icson.Objects;
using Icson.Objects.Sale;

namespace Icson.BLL.Sale
{
    public class VoucherManager
    {
        private VoucherManager()
		{
		}
        private static VoucherManager _instance;
        public static VoucherManager GetInstance()
		{
			if ( _instance == null )
			{
                _instance = new VoucherManager();
			}
			return _instance;
		}

        #region 销售单<->凭证号
        public int InsertSOVoucher(SOVoucherInfo oParam)
        {
            return new SOVoucherDac().Insert(oParam);
        }

        public int UpdateSOVoucher(SOVoucherInfo oParam)
        {
            return new SOVoucherDac().Update(oParam);
        }

        private void map(SOVoucherInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.SOSysNo = Util.TrimIntNull(tempdr["SOSysNo"]);
            oParam.VoucherID = Util.TrimNull(tempdr["VoucherID"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
        }

        private SOVoucherInfo LoadSOVoucher(SOVoucherInfo oParam)
        {
            string sql = "select * from so_voucher where sosysno=" + oParam.SOSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                SOVoucherInfo newInfo = new SOVoucherInfo();
                map(newInfo, ds.Tables[0].Rows[0]);
                return newInfo;
            }
            else
            {
                return null;
            }
        }

        public Hashtable LoadSOVoucherList(string VoucherID)
        {
            string sql = "select * from so_voucher where voucherid = " + Util.ToSqlString(VoucherID);
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                Hashtable ht = new Hashtable();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    SOVoucherInfo oInfo = new SOVoucherInfo();
                    map(oInfo, dr);
                    ht.Add(oInfo, null);
                }
                return ht;
            }
            else
                return null;
        }

        public void InsertSOVoucherList(Hashtable ht)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                foreach (SOVoucherInfo oParam in ht.Keys)
                {
                    SOVoucherInfo newInfo = LoadSOVoucher(oParam);
                    if (newInfo == null)
                        InsertSOVoucher(oParam);
                    else
                    {
                        newInfo.VoucherID = oParam.VoucherID;
                        UpdateSOVoucher(newInfo);
                    }
                }
                scope.Complete();
            }
        }

        #endregion

        #region 退货单<->凭证号
        public int InsertROVoucher(ROVoucherInfo oParam)
        {
            return new ROVoucherDac().Insert(oParam);
        }

        public int UpdateROVoucher(ROVoucherInfo oParam)
        {
            return new ROVoucherDac().Update(oParam);
        }

        private void map(ROVoucherInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.ROSysNo = Util.TrimIntNull(tempdr["ROSysNo"]);
            oParam.VoucherID = Util.TrimNull(tempdr["VoucherID"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
        }

        private ROVoucherInfo LoadROVoucher(ROVoucherInfo oParam)
        {
            string sql = "select * from ro_voucher where ROsysno=" + oParam.ROSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                ROVoucherInfo newInfo = new ROVoucherInfo();
                map(newInfo, ds.Tables[0].Rows[0]);
                return newInfo;
            }
            else
            {
                return null;
            }
        }

        public Hashtable LoadROVoucherList(string VoucherID)
        {
            string sql = "select * from ro_voucher where voucherid = " + Util.ToSqlString(VoucherID);
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                Hashtable ht = new Hashtable();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ROVoucherInfo oInfo = new ROVoucherInfo();
                    map(oInfo, dr);
                    ht.Add(oInfo, null);
                }
                return ht;
            }
            else
                return null;
        }

        public void InsertROVoucherList(Hashtable ht)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                foreach (ROVoucherInfo oParam in ht.Keys)
                {
                    ROVoucherInfo newInfo = LoadROVoucher(oParam);
                    if (newInfo == null)
                        InsertROVoucher(oParam);
                    else
                    {
                        newInfo.VoucherID = oParam.VoucherID;
                        UpdateROVoucher(newInfo);
                    }
                }
                scope.Complete();
            }
        }

        #endregion
    }
}
