using System;
using System.Data;
using System.Collections;
using System.Text;

using Icson.Objects;
using Icson.Utils;
using System.Transactions;
using Icson.Objects.Basic;
using Icson.DBAccess;
using Icson.DBAccess.Basic;
using Icson.Objects.ImportData;
using Icson.DBAccess.ImportData;

namespace Icson.BLL.Basic
{
    /// <summary>
    /// Summary description for VendorManager.
    /// </summary>
    public class VendorManager
    {
        private VendorManager()
        {
        }
        private static VendorManager _instance;
        public static VendorManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new VendorManager();
            }
            return _instance;
        }
        public void ImportVendor()
        {
            if (!AppConfig.IsImportable)
                throw new BizException("Is Importable is false");

            /*  do not  use the following code after Data Pour in */
            string sql = " select top 1 * from Vendor ";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                throw new BizException("the table Vendor is not empty");

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                string sql1 = "select * from ipp2003..Vendor";
                DataSet ds1 = SqlHelper.ExecuteDataSet(sql1);
                foreach (DataRow dr1 in ds1.Tables[0].Rows)
                {
                    VendorInfo oInfo = new VendorInfo();

                    oInfo.VendorID = Util.TrimNull(dr1["VendorID"]);
                    oInfo.VendorName = Util.TrimNull(dr1["VendorName"]);
                    oInfo.EnglishName = Util.TrimNull(dr1["EnglishName"]);
                    oInfo.BriefName = Util.TrimNull(dr1["BriefName"]);

                    int vendorType = (int)AppEnum.VendorType.Other;
                    if (Util.TrimIntNull(dr1["VendorType"]) == 0)
                        vendorType = (int)AppEnum.VendorType.Manufacturer;
                    if (Util.TrimIntNull(dr1["VendorType"]) == 1)
                        vendorType = (int)AppEnum.VendorType.Agent;
                    if (Util.TrimIntNull(dr1["VendorType"]) == 2)
                        vendorType = (int)AppEnum.VendorType.Other;
                    oInfo.VendorType = vendorType;

                    oInfo.District = Util.TrimNull(dr1["Country"]) + Util.TrimNull(dr1["City"]);
                    oInfo.Address = Util.TrimNull(dr1["Address"]);
                    oInfo.Zip = Util.TrimNull(dr1["PostCode"]);
                    oInfo.Contact = Util.TrimNull(dr1["Contact"]);
                    oInfo.Phone = Util.TrimNull(dr1["Tel"]) + Util.TrimNull(dr1["Mobile"]); ;
                    oInfo.Fax = Util.TrimNull(dr1["Fax"]); ;
                    oInfo.Email = Util.TrimNull(dr1["Email"]); ;
                    oInfo.Site = Util.TrimNull(dr1["Web"]); ;
                    oInfo.Bank = Util.TrimNull(dr1["Bank"]); ;
                    oInfo.Account = Util.TrimNull(dr1["Account"]);
                    oInfo.TaxNo = Util.TrimNull(dr1["TaxNo"]);
                    oInfo.Comment = Util.TrimNull(dr1["Comment"]);
                    oInfo.Note = Util.TrimNull(dr1["Note"]);
                    oInfo.Status = Util.TrimIntNull(dr1["Status"]);

                    this.Insert(oInfo);

                    //insert into convert table
                    ImportInfo oVendorConvert = new ImportInfo();
                    oVendorConvert.OldSysNo = Util.TrimIntNull(dr1["SysNo"]);
                    oVendorConvert.NewSysNo = oInfo.SysNo;
                    new ImportDac().Insert(oVendorConvert, "Vendor");

                }
                scope.Complete();
            }
        }

        public int Insert(VendorInfo oParam)
        {
            //string sql = " select top 1 sysno from Vendor where VendorID = " + Util.ToSqlString(oParam.VendorID);
            //DataSet ds = SqlHelper.ExecuteDataSet(sql);
            //if (Util.HasMoreRow(ds))
            //    throw new BizException("the same Vendor ID exists already");

            oParam.SysNo = SequenceDac.GetInstance().Create("Vendor_Sequence");
            oParam.VendorID = oParam.SysNo.ToString();
            return new VendorDac().Insert(oParam);
        }
        public int Update(VendorInfo oParam)
        {
            string sql = " select top 1 sysno from Vendor where VendorID = " + Util.ToSqlString(oParam.VendorID) + " and sysno <> " + oParam.SysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                throw new BizException("the same Vendor ID exists already");

            return new VendorDac().Update(oParam);
        }

        //批量删除供应商
        public int Deletes(string SysNos)
        {
            return new VendorDac().Deletes(SysNos);
        }

        private void map(VendorInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.VendorID = Util.TrimNull(tempdr["VendorID"]);
            oParam.VendorName = Util.TrimNull(tempdr["VendorName"]);
            oParam.EnglishName = Util.TrimNull(tempdr["EnglishName"]);
            oParam.BriefName = Util.TrimNull(tempdr["BriefName"]);
            oParam.VendorType = Util.TrimIntNull(tempdr["VendorType"]);
            oParam.District = Util.TrimNull(tempdr["District"]);
            oParam.Address = Util.TrimNull(tempdr["Address"]);
            oParam.Zip = Util.TrimNull(tempdr["Zip"]);
            oParam.Contact = Util.TrimNull(tempdr["Contact"]);
            oParam.Phone = Util.TrimNull(tempdr["Phone"]);
            oParam.Fax = Util.TrimNull(tempdr["Fax"]);
            oParam.Email = Util.TrimNull(tempdr["Email"]);
            oParam.Site = Util.TrimNull(tempdr["Site"]);
            oParam.Bank = Util.TrimNull(tempdr["Bank"]);
            oParam.Account = Util.TrimNull(tempdr["Account"]);
            oParam.TaxNo = Util.TrimNull(tempdr["TaxNo"]);
            oParam.APType = Util.TrimIntNull(tempdr["APType"]);
            oParam.Comment = Util.TrimNull(tempdr["Comment"]);
            oParam.Note = Util.TrimNull(tempdr["Note"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
            //add by judy
            //--------------------------------------------------------------------------
            oParam.WarrantyAreaSysNo = Util.TrimIntNull(tempdr["WarrantyAreaSysNo"]);
            oParam.WarrantyAddress = Util.TrimNull(tempdr["WarrantyAddress"]);
            oParam.WarrantyZip = Util.TrimNull(tempdr["WarrantyZip"]);
            oParam.WarrantyContact = Util.TrimNull(tempdr["WarrantyContact"]);
            oParam.WarrantyPhone = Util.TrimNull(tempdr["WarrantyPhone"]);
            oParam.WarrantyFax = Util.TrimNull(tempdr["WarrantyFax"]);
            oParam.WarrantyEmail = Util.TrimNull(tempdr["WarrantyEmail"]);
            oParam.WarrantySite = Util.TrimNull(tempdr["WarrantySite"]);
            //--------------------------------------------------------------------------
            oParam.WarrantySelfSend = Util.TrimIntNull(tempdr["WarrantySelfSend"]);
            oParam.RMAPosition = Util.TrimNull(tempdr["RMAPosition"]);
            oParam.CooperateType = Util.TrimIntNull(tempdr["CooperateType"]);
        }

        public VendorInfo Load(int paramSysno)
        {
            string sql = "select * from Vendor where SysNo = " + paramSysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);

            if (!Util.HasMoreRow(ds))
                return null;
            VendorInfo oVendor = new VendorInfo();
            map(oVendor, ds.Tables[0].Rows[0]);
            return oVendor;
        }
        public DataSet GetVendorDs(Hashtable paramHash)
        {
            string sql = " select * from Vendor ";
            if (paramHash != null && paramHash.Count != 0)
            {
                StringBuilder sb = new StringBuilder(100);
                sb.Append(" where 1=1 ");
                foreach (string key in paramHash.Keys)
                {
                    sb.Append(" and ");
                    object item = paramHash[key];
                    if (key == "some key special")
                    {
                        //special deal
                    }
                    else if (key == "RMAPosition")
                    {
                        sb.Append(key).Append("=").Append(Util.ToSqlString(item.ToString()));
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
            {
                sql = sql.Replace("select", "select top 50");
            }


            sql += " order by sysno desc";
            return SqlHelper.ExecuteDataSet(sql);
        }

        public bool GetRMAPosition(int sysno,string RMAPosition)
        {
            string sql = "select * from Vendor where sysno <> " + sysno + "and RMAPosition=" + RMAPosition;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds) || ds.Tables[0].Rows.Count > 0)
                return true;
            else
                return false;
        }

    }
}
