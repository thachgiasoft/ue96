using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Transactions;
using System.Collections;
using Icson.BLL.Purchase;
using Icson.DBAccess;
using Icson.DBAccess.Basic;
using Icson.Objects;
using Icson.Objects.Basic;
using Icson.Objects.Purchase;
using Icson.Utils;

namespace Icson.BLL.Basic
{
    public class ProductIDManager
    {
        private ProductIDManager()
        {
        }
        private static ProductIDManager _instance;
        public static ProductIDManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ProductIDManager();
            }
            return _instance;
        }


        private void map(ProductIDInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.ProductSysNo = Util.TrimIntNull(tempdr["ProductSysNo"]);
            oParam.POSysNo = Util.TrimIntNull(tempdr["POSysNo"]);
            oParam.OrderNum = Util.TrimIntNull(tempdr["OrderNum"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
            oParam.ProductSN = Util.TrimNull(tempdr["ProductSN"]);
            oParam.ProductTrackSN = Util.TrimNull(tempdr["ProductTrackSN"]);
            oParam.Note = Util.TrimNull(tempdr["Note"]);
        }

        public int Insert(ProductIDInfo oParam)
        {
            return new ProductIDDac().Insert(oParam);
        }

        public int Update(ProductIDInfo oParam)
        {
            return new ProductIDDac().Update(oParam);
        }

        public ProductIDInfo Load(int SysNo)
        {
            string sql = "select * from product_id where sysno=" + SysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                ProductIDInfo oInfo = new ProductIDInfo();
                map(oInfo, ds.Tables[0].Rows[0]);
                return oInfo;
            }
            else
                return null;
        }

        public void GenerateProductIDsByPO(int POSysNo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                POInfo po = PurchaseManager.GetInstance().LoadPO(POSysNo);

                if (po != null)
                {
                    GenerateProductIDsByPO(po);
                }

                scope.Complete();
            }
        }

        public void GenerateProductIDsByPO(POInfo PO)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                if (PO != null)
                {
                    AbandonProductIDsByPO(PO.SysNo);
                    foreach (POItemInfo item in PO.itemHash.Values)
                    {
                        for (int i = 1; i <= item.Quantity; i++)
                        {
                            ProductIDInfo pidInfo = new ProductIDInfo();
                            pidInfo.ProductSysNo = item.ProductSysNo;
                            pidInfo.POSysNo = PO.SysNo;
                            pidInfo.OrderNum = i;
                            pidInfo.Status = (int)AppEnum.BiStatus.Valid;
                            this.Insert(pidInfo);
                        }
                    }
                }

                scope.Complete();
            }
        }

        public int AbandonProductIDsByPO(int POSysNo)
        {
            string sql = "update Product_ID set status=@status where posysno=" + POSysNo;
            sql = sql.Replace("@status", ((int)AppEnum.BiStatus.InValid).ToString());
            return SqlHelper.ExecuteNonQuery(sql);
        }

        public DataSet GetProductIDDetailInfo(int SysNo)
        {
            string sql = @"select pid.sysno as productidsysno,pid.productsn,pid.producttracksn,pid.productsysno,p.productid,p.productname,pid.note,
                            pid.posysno,su.username,poi.orderprice,po.intime,
                            po.vendorsysno,v.vendorname,v.contact as vendorcontact,v.phone as vendorphone,v.address as vendoraddress,
                            v.warrantycontact,v.warrantyphone,v.warrantyaddress,v.RMAPosition  
                            from product_id pid inner join po_master po on pid.posysno=po.sysno inner join po_item poi on po.sysno=poi.posysno 
                            left join vendor v on po.vendorsysno=v.sysno 
                            left join sys_user su on po.createusersysno=su.sysno inner join product p on p.sysno = pid.productsysno 
                            where pid.sysno=" + SysNo;
            return SqlHelper.ExecuteDataSet(sql);
        }
        public void ProductIDSNImportToDB(ArrayList al)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                for (int i = 0; i < al.Count; i++)
                {
                    ProductIDInfo oInfo = (ProductIDInfo)al[i];
                    string sql = "update Product_ID set ProductSN=" +Util.ToSqlString(oInfo.ProductSN) + " where sysno =" + oInfo.SysNo;
                    SqlHelper.ExecuteNonQuery(sql);
                }
                scope.Complete();
            }
        }

        public DataSet GetProductIDSNDs(Hashtable paramHash)
        {
            string sql = @"select * from Product_ID where 1=1 @SysNo @ProductIDSN";
            if (paramHash.ContainsKey("ProductIDSysNo"))
                sql = sql.Replace("@SysNo", "and sysno=" + Util.ToSqlString(paramHash["ProductIDSysNo"].ToString()));
            else
                sql = sql.Replace("@SysNo", "");

            if (paramHash.ContainsKey("ProductIDSN"))
                sql = sql.Replace("@ProductIDSN", "and ProductSN like" + Util.ToSqlLikeString(paramHash["ProductIDSN"].ToString()));
            else
                sql = sql.Replace("@ProductIDSN", "");

            return SqlHelper.ExecuteDataSet(sql);
            
        }
    }
}