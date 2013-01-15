using System;
using System.Data;
using System.Collections;
using System.Text;

using Icson.Utils;
using System.Transactions;
using Icson.Objects;
using Icson.Objects.Basic;
using Icson.Objects.ImportData;
using Icson.DBAccess;
using Icson.DBAccess.ImportData;
using Icson.DBAccess.Basic;

namespace Icson.BLL.Basic
{
    /// <summary>
    /// Summary description for ASPManager. A(Area)S(Ship)P(Pay)
    /// </summary>
    public class ASPManager : IInitializable
    {
        private ASPManager()
        {
        }
        private static ASPManager _instance;
        public static ASPManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ASPManager();
                _instance.Init();
                SyncManager.GetInstance().RegisterLastVersion((int)AppEnum.Sync.ASP);
            }
            return _instance;
        }

        #region six locker
        private static object areaLock = new object();
        private static object payTypeLock = new object();
        private static object shipTypeLock = new object();
        private static object shipAreaLock = new object();
        private static object shipAreaPriceLock = new object();
        private static object shipPayLock = new object();
        private static object freightManAreaLock = new object();
        private static object payPlusChargeLock = new object();
        private static object branchUserLock = new object();
        #endregion

        #region six HASH
        private static Hashtable areaHash = new Hashtable(10);
        private static Hashtable payTypeHash = new Hashtable(10);
        private static Hashtable shipTypeHash = new Hashtable(10);
        private static Hashtable shipAreaHash = new Hashtable(10);
        private static Hashtable shipAreaPriceHash = new Hashtable(10);
        private static Hashtable shipPayHash = new Hashtable(10);
        private static Hashtable freightManAreaHash = new Hashtable(10);
        private static Hashtable payPlusChargeHash = new Hashtable(10);
        private static Hashtable branchUserHash = new Hashtable(10);

        public Hashtable GetAreaHash()
        {
            return areaHash;
        }
        public Hashtable GetPayTypeHash()
        {
            return payTypeHash;
        }
        public Hashtable GetShipTypeHash()
        {
            return shipTypeHash;
        }
        public Hashtable GetShipAreaHash()
        {
            return shipAreaHash;
        }
        public Hashtable GetShipAreaPriceHash()
        {
            return shipAreaPriceHash;
        }
        public Hashtable GetShipPayHash()
        {
            return shipPayHash;
        }
        public Hashtable GetFreightManAreaHash()
        {
            return freightManAreaHash;
        }
        public Hashtable GetPayPlusChargeHash()
        {
            return payPlusChargeHash;
        }

        public Hashtable GetBranchUserHash()
        {
            return branchUserHash;
        }
        #endregion

        #region Map
        private void Map(AreaInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.ProvinceSysNo = Util.TrimIntNull(tempdr["ProvinceSysNo"]);
            oParam.CitySysNo = Util.TrimIntNull(tempdr["CitySysNo"]);
            oParam.ProvinceName = Util.TrimNull(tempdr["ProvinceName"]);
            oParam.CityName = Util.TrimNull(tempdr["CityName"]);
            oParam.DistrictName = Util.TrimNull(tempdr["DistrictName"]);
            oParam.OrderNumber = Util.TrimNull(tempdr["OrderNumber"]);
            oParam.IsLocal = Util.TrimIntNull(tempdr["IsLocal"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
            oParam.LocalCode = Util.TrimIntNull(tempdr["LocalCode"]);
        }
        private void Map(ShipTypeInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.ShipTypeID = Util.TrimNull(tempdr["ShipTypeID"]);
            oParam.ShipTypeName = Util.TrimNull(tempdr["ShipTypeName"]);
            oParam.ShipTypeDesc = Util.TrimNull(tempdr["ShipTypeDesc"]);
            oParam.Period = Util.TrimNull(tempdr["Period"]);
            oParam.Provider = Util.TrimNull(tempdr["Provider"]);
            oParam.PremiumRate = Util.TrimDecimalNull(tempdr["PremiumRate"]);
            oParam.PremiumBase = Util.TrimDecimalNull(tempdr["PremiumBase"]);
            oParam.FreeShipBase = Util.TrimDecimalNull(tempdr["FreeShipBase"]);
            oParam.OrderNumber = Util.TrimNull(tempdr["OrderNumber"]);
            oParam.IsOnlineShow = Util.TrimIntNull(tempdr["IsOnlineShow"]);
            oParam.IsWithPackFee = Util.TrimIntNull(tempdr["IsWithPackFee"]);
            oParam.StatusQueryType = Util.TrimIntNull(tempdr["StatusQueryType"]);
            oParam.StatusQueryUrl = Util.TrimNull(tempdr["StatusQueryUrl"]);
        }
        private void Map(PayTypeInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.PayTypeID = Util.TrimNull(tempdr["PayTypeID"]);
            oParam.PayTypeName = Util.TrimNull(tempdr["PayTypeName"]);
            oParam.PayTypeDesc = Util.TrimNull(tempdr["PayTypeDesc"]);
            oParam.Period = Util.TrimNull(tempdr["Period"]);
            oParam.PaymentPage = Util.TrimNull(tempdr["PaymentPage"]);
            oParam.PayRate = Util.TrimDecimalNull(tempdr["PayRate"]);
            oParam.IsNet = Util.TrimIntNull(tempdr["IsNet"]);
            oParam.IsPayWhenRecv = Util.TrimIntNull(tempdr["IsPayWhenRecv"]);
            oParam.OrderNumber = Util.TrimNull(tempdr["OrderNumber"]);
            oParam.IsOnlineShow = Util.TrimIntNull(tempdr["IsOnlineShow"]);
        }
        private void Map(ShipAreaPriceInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.ShipTypeSysNo = Util.TrimIntNull(tempdr["ShipTypeSysNo"]);
            oParam.AreaSysNo = Util.TrimIntNull(tempdr["AreaSysNo"]);
            oParam.BaseWeight = Util.TrimIntNull(tempdr["BaseWeight"]);
            oParam.TopWeight = Util.TrimIntNull(tempdr["TopWeight"]);
            oParam.UnitWeight = Util.TrimIntNull(tempdr["UnitWeight"]);
            oParam.UnitPrice = Util.TrimDecimalNull(tempdr["UnitPrice"]);
            oParam.MaxPrice = Util.TrimDecimalNull(tempdr["MaxPrice"]);
        }

        private void Map(ShipAreaInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.ShipTypeSysNo = Util.TrimIntNull(tempdr["ShipTypeSysNo"]);
            oParam.AreaSysNo = Util.TrimIntNull(tempdr["AreaSysNo"]);
        }
        private void Map(ShipPayInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.ShipTypeSysNo = Util.TrimIntNull(tempdr["ShipTypeSysNo"]);
            oParam.PayTypeSysNo = Util.TrimIntNull(tempdr["PayTypeSysNo"]);
        }

        private void Map(FreightManAreaInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.FreightUserSysNo = Util.TrimIntNull(tempdr["FreightUserSysNo"]);
            oParam.AreaSysNo = Util.TrimIntNull(tempdr["AreaSysNo"]);
        }
        //private void Map(PayPlusChargeInfo oParam, DataRow tempdr)
        //{
        //    oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
        //    oParam.BankName = Util.TrimNull(tempdr["BankName"]);
        //    oParam.InstallmentNum = Util.TrimIntNull(tempdr["InstallmentNum"]);
        //    oParam.PlusRate = Util.TrimDecimalNull(tempdr["PlusRate"]);
        //    oParam.Status = Util.TrimIntNull(tempdr["Status"]);
        //}
        #endregion

        #region Init
        public void Init()
        {
            InitArea();
            InitPayType();
            InitShipType();
            InitShipAreaUn();
            InitShipPayUn();
            InitShipAreaPrice();
            InitFreightManArea();
            //InitPayPlusCharge();
            InitBranchUser();
        }
        public void InitArea()
        {
            lock (areaLock)
            {
                if (areaHash != null)
                    areaHash.Clear();
                string sql = "select * from area order by ProvinceName,CityName,DistrictName";
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (!Util.HasMoreRow(ds))
                    return;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    AreaInfo item = new AreaInfo();
                    Map(item, dr);
                    if (areaHash == null)
                    {
                        areaHash = new Hashtable(1000);
                    }
                    areaHash.Add(item.SysNo, item);
                }
            }
        }
        public void InitPayType()
        {
            lock (payTypeLock)
            {
                payTypeHash.Clear();
                string sql = "select * from PayType order by OrderNumber";
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (!Util.HasMoreRow(ds))
                    return;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    PayTypeInfo item = new PayTypeInfo();
                    Map(item, dr);
                    payTypeHash.Add(item.SysNo, item);
                }
            }
        }
        public void InitShipType()
        {
            lock (shipTypeLock)
            {
                if (shipTypeHash != null)
                    shipTypeHash.Clear();
                string sql = "select * from shiptype";
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (!Util.HasMoreRow(ds))
                    return;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ShipTypeInfo item = new ShipTypeInfo();
                    Map(item, dr);
                    shipTypeHash.Add(item.SysNo, item);
                }
            }
        }
        public void InitShipAreaUn()
        {
            lock (shipAreaLock)
            {
                shipAreaHash.Clear();
                string sql = "select * from shiptype_area_un";
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (!Util.HasMoreRow(ds))
                    return;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ShipAreaInfo item = new ShipAreaInfo();
                    Map(item, dr);
                    shipAreaHash.Add(item.SysNo, item);
                }
            }
        }
        public void InitShipPayUn()
        {
            lock (shipPayLock)
            {
                shipPayHash.Clear();
                string sql = "select * from shiptype_paytype_un";
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (!Util.HasMoreRow(ds))
                    return;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ShipPayInfo item = new ShipPayInfo();
                    Map(item, dr);
                    shipPayHash.Add(item.SysNo, item);
                }
            }
        }
        public void InitFreightManArea()
        {
            lock (freightManAreaLock)
            {
                freightManAreaHash.Clear();
                string sql = "select * from FreightMan_Area";
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (!Util.HasMoreRow(ds))
                    return;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    FreightManAreaInfo item = new FreightManAreaInfo();

                    Map(item, dr);
                    freightManAreaHash.Add(item.SysNo, item);
                }
            }
        }
        public void InitShipAreaPrice()
        {
            lock (shipAreaPriceLock)
            {
                shipAreaPriceHash.Clear();
                string sql = "select * from shiptype_area_price";
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (!Util.HasMoreRow(ds))
                    return;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ShipAreaPriceInfo item = new ShipAreaPriceInfo();
                    Map(item, dr);
                    shipAreaPriceHash.Add(item.SysNo, item);
                }
            }
        }
        //public void InitPayPlusCharge()
        //{
        //    lock (payPlusChargeLock)
        //    {
        //        payPlusChargeHash.Clear();
        //        string sql = "select * from PayPlusCharge order by BankName,InstallmentNum";
        //        DataSet ds = SqlHelper.ExecuteDataSet(sql);
        //        if (!Util.HasMoreRow(ds))
        //            return;
        //        foreach (DataRow dr in ds.Tables[0].Rows)
        //        {
        //            PayPlusChargeInfo item = new PayPlusChargeInfo();
        //            Map(item, dr);
        //            payPlusChargeHash.Add(item.SysNo, item);
        //        }
        //    }
        //}


        public void InitBranchUser()
        {
            lock (branchUserLock)
            {
                branchUserHash.Clear();
                string sql = "select * from Sys_User where stationsysno>0 and status=" + (int)AppEnum.BiStatus.Valid;
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (!Util.HasMoreRow(ds))
                    return;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    UserInfo item = new UserInfo();
                    item = SysManager.GetInstance().LoadUser(Util.TrimIntNull(dr["sysno"]));
                    branchUserHash.Add(item.SysNo, item);
                }
            }
        }

        #endregion

        #region Area Section
        public void ImportArea()
        {
            if (!AppConfig.IsImportable)
                throw new BizException("Is Importable is false");

            /*  do not  use the following code after Data Pour in */
            string sql = " select top 1 * from area ";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                throw new BizException("the table area is not empty");

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                string sql1 = "select * from ipp2003..area where countrySysNo = 1 and provinceSysNo = 0"; //通用省份 and SysNo <> 36";
                DataSet ds1 = SqlHelper.ExecuteDataSet(sql1);
                foreach (DataRow dr1 in ds1.Tables[0].Rows)
                {
                    AreaInfo oProvince = new AreaInfo();
                    oProvince.ProvinceName = Util.TrimNull(dr1["ProvinceName"]);
                    oProvince.IsLocal = Util.TrimIntNull(dr1["IsLocal"]);
                    oProvince.Status = Util.TrimIntNull(dr1["Status"]);
                    oProvince.OrderNumber = "zzz";
                    this.InsertArea(oProvince);

                    //insert into convert table
                    ImportInfo oProvinceConvert = new ImportInfo();
                    oProvinceConvert.OldSysNo = Util.TrimIntNull(dr1["SysNo"]);
                    oProvinceConvert.NewSysNo = oProvince.SysNo;
                    new ImportDac().Insert(oProvinceConvert, "Area");

                    string sql2 = "select * from ipp2003..area where provinceSysNo = " + Util.TrimIntNull(dr1["SysNo"]) + " and citySysNo = 0 ";
                    DataSet ds2 = SqlHelper.ExecuteDataSet(sql2);
                    foreach (DataRow dr2 in ds2.Tables[0].Rows)
                    {
                        AreaInfo oCity = new AreaInfo();
                        oCity.ProvinceSysNo = oProvince.SysNo;
                        oCity.ProvinceName = Util.TrimNull(dr1["ProvinceName"]);
                        oCity.CityName = Util.TrimNull(dr2["CityName"]);
                        oCity.IsLocal = Util.TrimIntNull(dr2["IsLocal"]);
                        oCity.Status = Util.TrimIntNull(dr2["Status"]);
                        oCity.OrderNumber = "zzz";
                        this.InsertArea(oCity);

                        //insert into convert table
                        ImportInfo oCityConvert = new ImportInfo();
                        oCityConvert.OldSysNo = Util.TrimIntNull(dr2["SysNo"]);
                        oCityConvert.NewSysNo = oCity.SysNo;
                        new ImportDac().Insert(oCityConvert, "Area");

                        string sql3 = "select * from ipp2003..area where citySysNo = " + Util.TrimIntNull(dr2["SysNo"]);
                        DataSet ds3 = SqlHelper.ExecuteDataSet(sql3);
                        foreach (DataRow dr3 in ds3.Tables[0].Rows)
                        {
                            AreaInfo oDistrict = new AreaInfo();

                            oDistrict.ProvinceSysNo = oCity.ProvinceSysNo;
                            oDistrict.CitySysNo = oCity.SysNo;
                            oDistrict.ProvinceName = Util.TrimNull(dr3["ProvinceName"]);
                            oDistrict.CityName = Util.TrimNull(dr3["CityName"]);
                            oDistrict.DistrictName = Util.TrimNull(dr3["DistrictName"]);
                            oDistrict.IsLocal = Util.TrimIntNull(dr3["IsLocal"]);
                            oDistrict.Status = Util.TrimIntNull(dr3["Status"]);
                            oDistrict.OrderNumber = "zzz";
                            this.InsertArea(oDistrict);

                            //insert into convert table
                            ImportInfo oDistrictConvert = new ImportInfo();
                            oDistrictConvert.OldSysNo = Util.TrimIntNull(dr3["SysNo"]);
                            oDistrictConvert.NewSysNo = oDistrict.SysNo;
                            new ImportDac().Insert(oDistrictConvert, "Area");
                        }
                    }
                }
                scope.Complete();
            }
        }
        public int InsertArea(AreaInfo oParam)
        {
            if (!AppConfig.IsImportable)
                foreach (AreaInfo item in areaHash.Values)
                {
                    if (item.GetWholeName() == oParam.GetWholeName())
                        throw new BizException("area duplicated");
                }
            oParam.SysNo = SequenceDac.GetInstance().Create("Area_Sequence");
            int result = new ASPDac().InsertArea(oParam);
            SyncManager.GetInstance().SetDbLastVersion((int)AppEnum.Sync.ASP);

            areaHash.Add(oParam.SysNo, oParam);
            return result;
        }
        public int UpdateArea(AreaInfo oParam)
        {
            if (!areaHash.ContainsKey(oParam.SysNo))
                throw new BizException("the area does not exist, update failed");

            int result = new ASPDac().UpdateArea(oParam);
            SyncManager.GetInstance().SetDbLastVersion((int)AppEnum.Sync.ASP);

            areaHash.Remove(oParam.SysNo);
            areaHash.Add(oParam.SysNo, oParam);

            return result;
        }
        public AreaInfo LoadArea(int paramSysNo)
        {
            if (!areaHash.ContainsKey(paramSysNo))
                throw new BizException("load area info error");
            return (AreaInfo)areaHash[paramSysNo];
        }
        public int GetAreaType(AreaInfo oParam)
        {
            if (oParam.CitySysNo != AppConst.IntNull)
                return (int)AppEnum.AreaType.District;
            else if (oParam.ProvinceSysNo != AppConst.IntNull)
                return (int)AppEnum.AreaType.City;
            else
                return (int)AppEnum.AreaType.Province;
        }
        public SortedList GetProvinceList(bool isValidOnly)
        {
            SortedList sl = null;
            foreach (AreaInfo item in areaHash.Values)
            {
                if (this.GetAreaType(item) == (int)AppEnum.AreaType.Province)
                {
                    if ((isValidOnly && item.Status == (int)AppEnum.BiStatus.Valid) || !isValidOnly)
                    {
                        if (sl == null)
                            sl = new SortedList(30);
                        sl.Add(item, null);
                    }
                }
            }
            return sl;
        }
        public SortedList GetCityList(int provinceSysNo, bool isValidOnly)
        {
            SortedList sl = null;
            foreach (AreaInfo item in areaHash.Values)
            {
                if (this.GetAreaType(item) == (int)AppEnum.AreaType.City && item.ProvinceSysNo == provinceSysNo)
                {
                    if ((isValidOnly && item.Status == (int)AppEnum.BiStatus.Valid) || !isValidOnly)
                    {
                        if (sl == null)
                            sl = new SortedList(30);
                        sl.Add(item, null);
                    }
                }
            }
            return sl;
        }
        public SortedList GetDistrictList(int citySysNo, bool isValidOnly)
        {
            SortedList sl = null;
            foreach (AreaInfo item in areaHash.Values)
            {
                if (this.GetAreaType(item) == (int)AppEnum.AreaType.District && item.CitySysNo == citySysNo)
                {
                    if ((isValidOnly && item.Status == (int)AppEnum.BiStatus.Valid) || !isValidOnly)
                    {
                        if (sl == null)
                            sl = new SortedList(30);
                        sl.Add(item, null);
                    }
                }
            }
            return sl;
        }
        /// <summary>
        /// 根据深度获得本地区对应深度的地区码
        /// </summary>
        /// <param name="depth"></param>
        /// <returns></returns>
        public int GetAreaSysNo(int depth, AreaInfo oArea)
        {
            if (depth == this.GetAreaType(oArea))
            {
                return oArea.SysNo;
            }
            switch (depth)
            {
                case (int)AppEnum.AreaType.Province:
                    return oArea.ProvinceSysNo;
                case (int)AppEnum.AreaType.City:
                    return oArea.CitySysNo;
                case (int)AppEnum.AreaType.District:
                    return oArea.SysNo;
                default:
                    return 0;
            }
        }

        #endregion

        #region Pay Type Section
        public void ImportPayType()
        {
            if (!AppConfig.IsImportable)
                throw new BizException("Is Importable is false");

            /*  do not  use the following code after Data Pour in */
            string sql = @"select * from PayType";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                throw new BizException("the table paytype is not empty");

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                string sql1 = @"select pt.*, ptl.paytypename, period, note from ipp2003..pay_type as pt, ipp2003..pay_type_language as ptl
					where pt.sysno = ptl.paytypesysno and ptl.languageid='cn'";
                DataSet ds1 = SqlHelper.ExecuteDataSet(sql1);
                foreach (DataRow dr1 in ds1.Tables[0].Rows)
                {
                    PayTypeInfo oPayType = new PayTypeInfo();
                    oPayType.PayTypeID = Util.TrimNull(dr1["PayTypeID"]);
                    oPayType.PayTypeName = Util.TrimNull(dr1["PayTypeName"]);
                    oPayType.PayTypeDesc = Util.TrimNull(dr1["Note"]);
                    oPayType.Period = Util.TrimNull(dr1["Period"]);
                    oPayType.PaymentPage = Util.TrimNull(dr1["PaymentPage"]);
                    oPayType.PayRate = Util.TrimDecimalNull(dr1["ServiceRate"]);
                    oPayType.IsNet = Util.TrimIntNull(dr1["IsNet"]);
                    oPayType.IsPayWhenRecv = Util.TrimIntNull(dr1["IsPayWhenReceive"]);
                    oPayType.OrderNumber = "zzz";
                    oPayType.IsOnlineShow = Util.TrimIntNull(dr1["sstatus"]);

                    this.InsertPayType(oPayType);

                    //insert into convert table
                    ImportInfo oImport = new ImportInfo();
                    oImport.OldSysNo = Util.TrimIntNull(dr1["SysNo"]);
                    oImport.NewSysNo = oPayType.SysNo;
                    new ImportDac().Insert(oImport, "PayType");

                }
                scope.Complete();
            }
        }
        public int InsertPayType(PayTypeInfo oParam)
        {
            string sql = "select * from paytype where paytypeid = " + Util.ToSqlString(oParam.PayTypeID);
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                throw new BizException("the same paytypeid exists");

            oParam.SysNo = SequenceDac.GetInstance().Create("PayType_Sequence");
            int result = new ASPDac().InsertPayType(oParam);
            SyncManager.GetInstance().SetDbLastVersion((int)AppEnum.Sync.ASP);

            if (payTypeHash == null)
                payTypeHash = new Hashtable(10);
            payTypeHash.Add(oParam.SysNo, oParam);
            return result;
        }
        public int UpdatePayType(PayTypeInfo oParam)
        {
            string sql = "select * from paytype where paytypeid=" + Util.ToSqlString(oParam.PayTypeID) + " and sysno <>" + oParam.SysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                throw new BizException("the same paytypeid exists");

            if (payTypeHash == null || !payTypeHash.ContainsKey(oParam.SysNo))
                throw new BizException("the pay type does not exist, update failed");

            int result = new ASPDac().UpdatePayType(oParam);
            SyncManager.GetInstance().SetDbLastVersion((int)AppEnum.Sync.ASP);

            payTypeHash.Remove(oParam.SysNo);
            payTypeHash.Add(oParam.SysNo, oParam);

            return result;
        }
        public PayTypeInfo LoadPayType(int paramSysNo)
        {
            if (payTypeHash == null || !payTypeHash.ContainsKey(paramSysNo))
                return null;
            return (PayTypeInfo)payTypeHash[paramSysNo];
        }
        public SortedList GetPayTypeList(bool isOnlineShow)
        {
            if (payTypeHash == null)
                return null;

            SortedList sl = null;
            foreach (PayTypeInfo item in payTypeHash.Values)
            {
                if ((isOnlineShow && item.IsOnlineShow == (int)AppEnum.YNStatus.Yes) || !isOnlineShow)
                {
                    if (sl == null)
                        sl = new SortedList(10);
                    sl.Add(item, null);
                }
            }
            return sl;
        }
        public SortedList GetPayTypeListIsPayAdvanced(bool isNet)
        {
            if (payTypeHash == null)
                return null;

            SortedList sl = null;
            foreach (PayTypeInfo item in payTypeHash.Values)
            {
                if (item.IsPayWhenRecv == (int)AppEnum.YNStatus.No)
                {
                    if ((item.IsNet == (int)AppEnum.YNStatus.Yes && isNet) ||
                        (item.IsNet == (int)AppEnum.YNStatus.No && !isNet))
                    {
                        if (sl == null)
                            sl = new SortedList(10);
                        sl.Add(item, null);
                    }
                }
            }
            return sl;
        }
        public SortedList GetPayTypeListByShipType(int shipTypeSysNo)
        {
            Hashtable htPayTypeByShipTypeUn = new Hashtable(5);
            foreach (ShipPayInfo item in shipPayHash.Values)
            {
                if (item.ShipTypeSysNo == shipTypeSysNo
                    && !htPayTypeByShipTypeUn.ContainsKey(item.ShipTypeSysNo))
                    htPayTypeByShipTypeUn.Add(item.PayTypeSysNo, null);
            }
            SortedList sl = new SortedList(5);
            foreach (PayTypeInfo item in payTypeHash.Values)
            {
                if (!htPayTypeByShipTypeUn.ContainsKey(item.SysNo))
                    sl.Add(item, null);
            }
            return sl;
        }
        #endregion

        #region Ship Type Section
        public void ImportShipType()
        {
            if (!AppConfig.IsImportable)
                throw new BizException("Is Importable is false");
            /*  do not  use the following code after Data Pour in */
            string sql = @"select * from ShipType";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                throw new BizException("the table ship type is not empty");

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                string sql1 = @"select st.*, st.withpackfee as iswithpackfee, stl.shiptypename, stl.company, stl.period from ipp2003..ship_type as st, ipp2003..ship_type_language as stl
							where st.sysno = stl.shiptypesysno and stl.languageid = 'cn'";
                DataSet ds1 = SqlHelper.ExecuteDataSet(sql1);
                foreach (DataRow dr1 in ds1.Tables[0].Rows)
                {
                    ShipTypeInfo oShipType = new ShipTypeInfo();


                    oShipType.ShipTypeID = Util.TrimNull(dr1["ShipTypeID"]);
                    oShipType.ShipTypeName = Util.TrimNull(dr1["ShipTypeName"]);
                    oShipType.ShipTypeDesc = "";
                    oShipType.Period = Util.TrimNull(dr1["Period"]);
                    oShipType.Provider = Util.TrimNull(dr1["Company"]);
                    oShipType.PremiumRate = Util.TrimDecimalNull(dr1["PremiumRate"]);
                    oShipType.PremiumBase = Util.TrimDecimalNull(dr1["PremiumBase"]);
                    oShipType.FreeShipBase = 0;
                    oShipType.OrderNumber = "zzz";
                    oShipType.IsOnlineShow = Util.TrimIntNull(dr1["sstatus"]);
                    oShipType.IsWithPackFee = Util.TrimIntNull(dr1["IsWithPackFee"]);
                    oShipType.StatusQueryType = Util.TrimIntNull(dr1["StatusQueryType"]);
                    oShipType.StatusQueryUrl = Util.TrimNull(dr1["StatusQueryUrl"]);

                    this.InsertShipType(oShipType);

                    //insert into convert table
                    ImportInfo oImport = new ImportInfo();
                    oImport.OldSysNo = Util.TrimIntNull(dr1["SysNo"]);
                    oImport.NewSysNo = oShipType.SysNo;
                    new ImportDac().Insert(oImport, "ShipType");

                }
                scope.Complete();
            }
        }
        public int InsertShipType(ShipTypeInfo oParam)
        {
            string sql = "select * from shiptype where shiptypeid = " + Util.ToSqlString(oParam.ShipTypeID);
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                throw new BizException("the same ship type id exists");

            oParam.SysNo = SequenceDac.GetInstance().Create("ShipType_Sequence");
            int result = new ASPDac().InsertShipType(oParam);
            SyncManager.GetInstance().SetDbLastVersion((int)AppEnum.Sync.ASP);

            if (shipTypeHash == null)
                shipTypeHash = new Hashtable(10);
            shipTypeHash.Add(oParam.SysNo, oParam);
            return result;
        }
        public int UpdateShipType(ShipTypeInfo oParam)
        {
            string sql = "select * from shiptype where shiptypeid=" + Util.ToSqlString(oParam.ShipTypeID) + " and sysno <>" + oParam.SysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                throw new BizException("the same ship type id exists");

            if (shipTypeHash == null || !shipTypeHash.ContainsKey(oParam.SysNo))
                throw new BizException("the ship type does not exist, update failed");

            int result = new ASPDac().UpdateShipType(oParam);
            SyncManager.GetInstance().SetDbLastVersion((int)AppEnum.Sync.ASP);

            shipTypeHash.Remove(oParam.SysNo);
            shipTypeHash.Add(oParam.SysNo, oParam);

            return result;
        }
        public ShipTypeInfo LoadShipType(int paramSysNo)
        {
            if (shipTypeHash == null || !shipTypeHash.ContainsKey(paramSysNo))
                return null;
            return (ShipTypeInfo)shipTypeHash[paramSysNo];
        }
        public SortedList GetShipTypeList(bool isOnlineShow)
        {
            if (shipTypeHash == null)
                return null;

            SortedList sl = null;
            foreach (ShipTypeInfo item in shipTypeHash.Values)
            {
                if ((isOnlineShow && item.IsOnlineShow == (int)AppEnum.YNStatus.Yes) || !isOnlineShow)
                {
                    if (sl == null)
                        sl = new SortedList(30);
                    sl.Add(item, null);
                }
            }
            return sl;
        }
        public SortedList GetShipTypeListByArea(int areaSysNo)
        {
            /* 取当前area
             * 看这个area，地区，市，省是否有否的关系，如果有，记录相应的shiptype（否的）
             * 从全部的shiptype中过滤之
             */
            AreaInfo oArea = areaHash[areaSysNo] as AreaInfo;
            if (oArea == null)
                return null;

            Hashtable htShipTypeUnByArea = new Hashtable(5);

            foreach (ShipAreaInfo item in shipAreaHash.Values)
            {
                if (item.AreaSysNo == oArea.SysNo
                    || item.AreaSysNo == oArea.CitySysNo
                    || item.AreaSysNo == oArea.ProvinceSysNo)
                {
                    if (!htShipTypeUnByArea.ContainsKey(item.ShipTypeSysNo))
                        htShipTypeUnByArea.Add(item.ShipTypeSysNo, null);
                }
            }

            SortedList sl = new SortedList(5);
            foreach (ShipTypeInfo item in shipTypeHash.Values)
            {
                if (!htShipTypeUnByArea.ContainsKey(item.SysNo))
                    sl.Add(item, null);
            }
            return sl;
        }

        public decimal GetShipPrice(int weight, decimal soAmt, int shipTypeSysNo, int areaSysNo)
        {
            if (weight == 0)
                return 0m;
            //取出对应地区信息
            AreaInfo area = this.LoadArea(areaSysNo);
            //取出对应运送方式
            ShipTypeInfo stInfo = this.LoadShipType(shipTypeSysNo);
            if (stInfo == null)
                throw new BizException("No ShipType selected");
            //达到免运费金额，则免除运费
            if (stInfo.FreeShipBase > 0 && stInfo.FreeShipBase < soAmt)
                return 0m;
            decimal shipPrice = 0m;
            //考虑包装重量和成本
            decimal packCost = 0m;
            //if(stInfo.IsWithPackFee==(int)AppEnum.YNStatus.Yes)
            //{
            //	if(weight>0&&weight<=1000)
            //	{
            //		weight += 250;
            //		packCost = 2m;
            //	}
            //	else if(weight>1000&&weight<=3000)
            //	{
            //		weight += 750;
            //		packCost = 4m;
            //	}
            //	else if(weight>3000)
            //	{
            //		weight += 1250;
            //		packCost = 8m;
            //	}
            //}
            //挂号费，邮政普包和邮政快递所有
            decimal rp = 0m;
            if (stInfo.ShipTypeID == "000" || stInfo.ShipTypeID == "003")
            {
                rp = 5m;
            }
            shipPrice += (packCost + rp);
            //取出对应运送方式和地区信息的所有价格策略
            Hashtable sapHash = new Hashtable();
            Hashtable allHash = this.GetShipAreaPriceHash();
            //取出对应运送方式和地区的最底层的价格策略
            for (int i = this.GetAreaType(area); i >= 0; i--)
            {
                foreach (ShipAreaPriceInfo sapInfo in allHash.Values)
                {
                    if (sapInfo.AreaSysNo == this.GetAreaSysNo(i, area) && sapInfo.ShipTypeSysNo == shipTypeSysNo)
                    {
                        sapHash.Add(sapInfo.SysNo, sapInfo);
                    }
                }
                if (sapHash.Count > 0)
                    break;
            }
            //没有任何匹配该地区和运送方式的价格策略,返回0
            if (sapHash.Count == 0)
                return 0m;
            //计算分布在各价格区间内的重量对应的运费
            int minBaseWeight = weight;
            int maxTopWeight = 0;
            int totalWeight = 0;
            foreach (ShipAreaPriceInfo sapInfo in sapHash.Values)
            {
                if (minBaseWeight > sapInfo.BaseWeight)
                    minBaseWeight = sapInfo.BaseWeight;
                if (maxTopWeight < sapInfo.TopWeight)
                    maxTopWeight = sapInfo.TopWeight;
                decimal priceItem = 0m;
                if (weight > sapInfo.BaseWeight)
                {
                    int weightItem = 0;
                    if (weight > sapInfo.TopWeight)
                        weightItem = sapInfo.TopWeight - sapInfo.BaseWeight;
                    else
                        weightItem = weight - sapInfo.BaseWeight;
                    int units = weightItem / sapInfo.UnitWeight;
                    if (weightItem % sapInfo.UnitWeight != 0)
                        units++;
                    priceItem = sapInfo.UnitPrice * units;
                    if (priceItem > sapInfo.MaxPrice && sapInfo.MaxPrice > 0)
                        priceItem = sapInfo.MaxPrice;
                    totalWeight += weightItem;
                }
                shipPrice += priceItem;
            }
            if (minBaseWeight > 0 || maxTopWeight < weight || totalWeight != weight)
                throw new BizException("The weight is out of range");

            return Util.TruncMoney(shipPrice);
        }

        /// <summary>
        /// 用户各快递公司的运费结算，计算出客户实际应支付的运费及与快递公司结算的运费
        /// </summary>
        /// <param name="weight"></param>
        /// <param name="shipTypeSysNo"></param>
        /// <param name="areaSysNo"></param>
        /// <returns></returns>

        public decimal GetShipPrice(decimal weight, int shipTypeSysNo, int areaSysNo)
        {
            if (weight == 0)
                return 0m;
            //取出对应地区信息
            AreaInfo area = this.LoadArea(areaSysNo);
            //取出对应运送方式
            ShipTypeInfo stInfo = this.LoadShipType(shipTypeSysNo);
            if (stInfo == null)
                throw new BizException("No ShipType selected");
            decimal shipPrice = 0m;
            //考虑包装重量和成本
            decimal packCost = 0m;
            //挂号费，邮政普包和邮政快递所有
            decimal rp = 0m;
            if (stInfo.ShipTypeID == "000" || stInfo.ShipTypeID == "003")
            {
                rp = 5m;
            }
            shipPrice += (packCost + rp);
            //取出对应运送方式和地区信息的所有价格策略
            Hashtable sapHash = new Hashtable();
            Hashtable allHash = this.GetShipAreaPriceHash();
            //取出对应运送方式和地区的最底层的价格策略
            for (int i = this.GetAreaType(area); i >= 0; i--)
            {
                foreach (ShipAreaPriceInfo sapInfo in allHash.Values)
                {
                    if (sapInfo.AreaSysNo == this.GetAreaSysNo(i, area) && sapInfo.ShipTypeSysNo == shipTypeSysNo)
                    {
                        sapHash.Add(sapInfo.SysNo, sapInfo);
                    }
                }
                if (sapHash.Count > 0)
                    break;
            }
            //没有任何匹配该地区和运送方式的价格策略,返回0
            if (sapHash.Count == 0)
                return 0m;
            //计算分布在各价格区间内的重量对应的运费
            decimal minBaseWeight = weight;
            decimal maxTopWeight = 0;
            decimal totalWeight = 0;
            foreach (ShipAreaPriceInfo sapInfo in sapHash.Values)
            {
                if (minBaseWeight > sapInfo.BaseWeight)
                    minBaseWeight = sapInfo.BaseWeight;
                if (maxTopWeight < sapInfo.TopWeight)
                    maxTopWeight = sapInfo.TopWeight;
                decimal priceItem = 0m;
                if (weight > sapInfo.BaseWeight)
                {
                    decimal weightItem = 0;
                    if (weight > sapInfo.TopWeight)
                        weightItem = sapInfo.TopWeight - sapInfo.BaseWeight;
                    else
                        weightItem = weight - sapInfo.BaseWeight;
                    decimal a = weightItem / sapInfo.UnitWeight;
                    string[] aa = new string[2];
                    string s = a.ToString();
                    aa = s.Split('.');
                    int units = int.Parse(aa[0]);
                    if (weightItem % sapInfo.UnitWeight != 0)
                        units++;
                    priceItem = sapInfo.UnitPrice * units;
                    if (priceItem > sapInfo.MaxPrice && sapInfo.MaxPrice > 0)
                        priceItem = sapInfo.MaxPrice;
                    totalWeight += weightItem;
                }
                shipPrice += priceItem;
            }
            if (minBaseWeight > 0 || maxTopWeight < weight || totalWeight != weight)
                throw new BizException("The weight is out of range");

            return Util.TruncMoney(shipPrice);
        }


        public decimal GetPremuimAmt(decimal soAmt, int shipTypeSysNo)
        {
            decimal premiumAmt = 0m;
            ShipTypeInfo stInfo = this.LoadShipType(shipTypeSysNo);
            if (soAmt > stInfo.PremiumBase)
                premiumAmt = soAmt * stInfo.PremiumRate;
            return Util.TruncMoney(premiumAmt);
        }
        #endregion

        #region Ship Type & Area Section
        public void ImportShipAreaUn()
        {
            if (!AppConfig.IsImportable)
                throw new BizException("Is Importable is false");

            /*  do not  use the following code after Data Pour in */
            string sql = @"select * from ShipType_Area_Un";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                throw new BizException("the table ShipType_Area is not empty");

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                string sql1 = @"select shiptype_convert.newsysno as ShipTypeSysNo, area_convert.newsysno as AreaSysNo from 
							ipp2003..area_ship as Old,
							ippconvert..area as area_convert,
							ippconvert..shiptype as shiptype_convert
							where
							Old.ShipTypeSysNo = shiptype_convert.oldsysno
							and Old.AreaSysNo = area_convert.oldsysno";
                DataSet ds1 = SqlHelper.ExecuteDataSet(sql1);
                foreach (DataRow dr1 in ds1.Tables[0].Rows)
                {
                    ShipAreaInfo oShipArea = new ShipAreaInfo();

                    oShipArea.ShipTypeSysNo = Util.TrimIntNull(dr1["ShipTypeSysNo"]);
                    oShipArea.AreaSysNo = Util.TrimIntNull(dr1["AreaSysNo"]);

                    this.InsertShipAreaUn(oShipArea);

                }
                scope.Complete();
            }
        }
        public int InsertShipAreaUn(ShipAreaInfo oParam)
        {
            string sql = "select * from shiptype_area_un where ShipTypeSysNo = " + oParam.ShipTypeSysNo + " and AreaSysNo = " + oParam.AreaSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                throw new BizException("this Area" + oParam.AreaSysNo + " shiptype_area_un record exists");

            int result = new ASPDac().InsertShipArea(oParam);
            SyncManager.GetInstance().SetDbLastVersion((int)AppEnum.Sync.ASP);

            if (shipAreaHash == null)
                shipAreaHash = new Hashtable(10);
            shipAreaHash.Add(oParam.SysNo, oParam);
            return result;
        }
        public int DeleteShipAreaUn(int paramSysNo)
        {
            if (shipAreaHash == null || !shipAreaHash.ContainsKey(paramSysNo))
                throw new BizException("the un ShipType Area record does not exist, delete failed");

            int result = new ASPDac().DeleteShipArea(paramSysNo);
            SyncManager.GetInstance().SetDbLastVersion((int)AppEnum.Sync.ASP);

            shipAreaHash.Remove(paramSysNo);

            return result;
        }
        #endregion

        #region Ship Type & Pay Type Section
        public void ImportShipPayUn()
        {
            if (!AppConfig.IsImportable)
                throw new BizException("Is Importable is false");

            /*  do not  use the following code after Data Pour in */
            string sql = @"select * from ShipType_PayType_Un";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                throw new BizException("the table ShipType_PayType_Un is not empty");

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                string sql1 = @"select shiptype_convert.newsysno as ShipTypeSysNo, paytype_convert.newsysno as PayTypeSysNo from 
							ipp2003..ship_pay as old,
							ippconvert..shiptype as shiptype_convert,
							ippconvert..paytype as paytype_convert
							where
							old.shiptypesysno = shiptype_convert.oldsysno
							and old.paytypesysno = paytype_convert.oldsysno";
                DataSet ds1 = SqlHelper.ExecuteDataSet(sql1);
                foreach (DataRow dr1 in ds1.Tables[0].Rows)
                {
                    ShipPayInfo oShipPay = new ShipPayInfo();

                    oShipPay.ShipTypeSysNo = Util.TrimIntNull(dr1["ShipTypeSysNo"]);
                    oShipPay.PayTypeSysNo = Util.TrimIntNull(dr1["PayTypeSysNo"]);

                    this.InsertShipPayUn(oShipPay);

                }
                scope.Complete();
            }
        }
        public int InsertShipPayUn(ShipPayInfo oParam)
        {
            string sql = "select * from shiptype_paytype_un where ShipTypeSysNo = " + oParam.ShipTypeSysNo + " and PayTypeSysNo = " + oParam.PayTypeSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                throw new BizException("this shiptype_paytype_un record exists");

            int result = new ASPDac().InsertShipPay(oParam);
            SyncManager.GetInstance().SetDbLastVersion((int)AppEnum.Sync.ASP);

            if (shipPayHash == null)
                shipPayHash = new Hashtable(10);
            shipPayHash.Add(oParam.SysNo, oParam);
            return result;
        }
        public int DeleteShipPayUn(int paramSysNo)
        {
            if (shipPayHash == null || !shipPayHash.ContainsKey(paramSysNo))
                throw new BizException("the un ShipType PayType record does not exist, delete failed");

            int result = new ASPDac().DeleteShipPay(paramSysNo);
            SyncManager.GetInstance().SetDbLastVersion((int)AppEnum.Sync.ASP);

            shipPayHash.Remove(paramSysNo);

            return result;
        }
        #endregion

        #region Ship Type & Area Price Section
        public void ImportShipAreaPrice()
        {
            if (!AppConfig.IsImportable)
                throw new BizException("Is Importable is false");

            /*  do not  use the following code after Data Pour in */
            string sql = @"select * from ShipType_Area_Price";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                throw new BizException("the table ShipType_Area_Price is not empty");

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                string sql1 = @"select shiptype_convert.newsysno as shiptypesysno, area_convert.newsysno as areasysno, 
								baseweight as bw1, baseprice as bp1, oldmaster.unitweight as uw1, oldmaster.unitprice as up1, maxprice,
								baseline as bw2, topline as tw2, olditem.unitweight as uw2, olditem.unitprice as up2

								from ipp2003..area_ship_price as oldmaster, ipp2003..area_ship_price_item as olditem,
								ippconvert..area as area_convert, ippconvert..shiptype as shiptype_convert
								where oldmaster.sysno *= olditem.shippricesysno
								and oldmaster.shiptypesysno = shiptype_convert.oldsysno
								and oldmaster.areasysno = area_convert.oldsysno
								order by shiptypesysno, areasysno, bw2";

                /*
                    分成主表和明细表

                    原来计算价格的规则

                    如果主表的baseWeight==0, 就直接返回BasePrice

                    如果重量小于主表的BaseWeight，就直接返回baseprice

                    如果重量大于主表的baseweight
                    ｛
                        如果没有分段，就使用主表中的unitweight，和unitprice
                        否则
                    ｝
                 */
                int lastShipTypeSysNo, lastAreaSysNo;
                lastShipTypeSysNo = lastAreaSysNo = AppConst.IntNull;
                int lastTopWeight = 900000000;

                DataSet ds1 = SqlHelper.ExecuteDataSet(sql1);
                foreach (DataRow dr1 in ds1.Tables[0].Rows)
                {
                    if (Util.TrimIntNull(dr1["bw2"]) == AppConst.IntNull)
                    {
                        //没有分段的

                        //新的区段，但是没有分段。
                        if (lastTopWeight != 900000000)
                            throw new BizException("上限区段不完整" + lastShipTypeSysNo + ", " + lastAreaSysNo);

                        ShipAreaPriceInfo oShipAreaPrice = new ShipAreaPriceInfo();
                        ShipAreaPriceInfo oShipAreaPrice2 = new ShipAreaPriceInfo();

                        oShipAreaPrice.ShipTypeSysNo = Util.TrimIntNull(dr1["ShipTypeSysNo"]);
                        oShipAreaPrice.AreaSysNo = Util.TrimIntNull(dr1["AreaSysNo"]);
                        oShipAreaPrice.BaseWeight = 0;
                        oShipAreaPrice.TopWeight = Util.TrimIntNull(dr1["bw1"]);
                        oShipAreaPrice.UnitWeight = Util.TrimIntNull(dr1["bw1"]);
                        oShipAreaPrice.UnitPrice = Util.TrimDecimalNull(dr1["bp1"]);
                        oShipAreaPrice.MaxPrice = Util.TrimDecimalNull(dr1["maxprice"]);

                        oShipAreaPrice2.ShipTypeSysNo = Util.TrimIntNull(dr1["ShipTypeSysNo"]);
                        oShipAreaPrice2.AreaSysNo = Util.TrimIntNull(dr1["AreaSysNo"]);
                        oShipAreaPrice2.BaseWeight = Util.TrimIntNull(dr1["bw1"]); ;
                        oShipAreaPrice2.TopWeight = 900000000;
                        oShipAreaPrice2.UnitWeight = Util.TrimIntNull(dr1["uw1"]);
                        oShipAreaPrice2.UnitPrice = Util.TrimDecimalNull(dr1["up1"]);
                        oShipAreaPrice2.MaxPrice = Util.TrimDecimalNull(dr1["maxprice"]);

                        lastShipTypeSysNo = oShipAreaPrice.ShipTypeSysNo;
                        lastAreaSysNo = oShipAreaPrice.AreaSysNo;
                        lastTopWeight = oShipAreaPrice2.TopWeight;

                        new ASPDac().InsertShipAreaPrice(oShipAreaPrice);
                        new ASPDac().InsertShipAreaPrice(oShipAreaPrice2);

                    }
                    else
                    {
                        //有分段
                        if (Util.TrimIntNull(dr1["ShipTypeSysNo"]) != lastShipTypeSysNo
                            || Util.TrimIntNull(dr1["AreaSysNo"]) != lastAreaSysNo) //新的区段
                        {
                            if (lastTopWeight != 900000000)
                                throw new BizException("上限区段不完整" + lastShipTypeSysNo + ", " + lastAreaSysNo);

                            ShipAreaPriceInfo oShipAreaPrice = new ShipAreaPriceInfo();

                            oShipAreaPrice.ShipTypeSysNo = Util.TrimIntNull(dr1["ShipTypeSysNo"]);
                            oShipAreaPrice.AreaSysNo = Util.TrimIntNull(dr1["AreaSysNo"]);
                            oShipAreaPrice.BaseWeight = 0;
                            oShipAreaPrice.TopWeight = Util.TrimIntNull(dr1["bw1"]);
                            oShipAreaPrice.UnitWeight = Util.TrimIntNull(dr1["bw1"]);
                            oShipAreaPrice.UnitPrice = Util.TrimDecimalNull(dr1["bp1"]);
                            oShipAreaPrice.MaxPrice = Util.TrimDecimalNull(dr1["maxprice"]);

                            lastShipTypeSysNo = oShipAreaPrice.ShipTypeSysNo;
                            lastAreaSysNo = oShipAreaPrice.AreaSysNo;
                            lastTopWeight = oShipAreaPrice.TopWeight;

                            new ASPDac().InsertShipAreaPrice(oShipAreaPrice);
                        }

                        ShipAreaPriceInfo oShipAreaPrice2 = new ShipAreaPriceInfo();
                        oShipAreaPrice2.ShipTypeSysNo = Util.TrimIntNull(dr1["ShipTypeSysNo"]);
                        oShipAreaPrice2.AreaSysNo = Util.TrimIntNull(dr1["AreaSysNo"]);
                        oShipAreaPrice2.BaseWeight = Util.TrimIntNull(dr1["bw2"]);
                        if (oShipAreaPrice2.BaseWeight != lastTopWeight)
                            throw new BizException("重量不连续" + oShipAreaPrice2.ShipTypeSysNo + ", " + oShipAreaPrice2.AreaSysNo);

                        oShipAreaPrice2.TopWeight = Util.TrimIntNull(dr1["tw2"]);

                        oShipAreaPrice2.UnitWeight = Util.TrimIntNull(dr1["uw2"]);
                        oShipAreaPrice2.UnitPrice = Util.TrimDecimalNull(dr1["up2"]);
                        oShipAreaPrice2.MaxPrice = Util.TrimDecimalNull(dr1["maxprice"]);

                        lastTopWeight = oShipAreaPrice2.TopWeight;

                        new ASPDac().InsertShipAreaPrice(oShipAreaPrice2);

                    }
                    this.InitShipAreaPrice();
                }
                scope.Complete();
            }
        }
        public int InsertShipAreaPrice(ShipAreaPriceInfo oParam)
        {

            int result = new ASPDac().InsertShipAreaPrice(oParam);
            SyncManager.GetInstance().SetDbLastVersion((int)AppEnum.Sync.ASP);

            if (shipAreaPriceHash == null)
                shipAreaPriceHash = new Hashtable(10);
            shipAreaPriceHash.Add(oParam.SysNo, oParam);
            return result;
        }
        public int DeleteShipAreaPrice(int paramSysNo)
        {
            if (shipAreaPriceHash == null || !shipAreaPriceHash.ContainsKey(paramSysNo))
                throw new BizException("the ShipType Area Price record does not exist, delete failed");

            int result = new ASPDac().DeleteShipAreaPrice(paramSysNo);
            SyncManager.GetInstance().SetDbLastVersion((int)AppEnum.Sync.ASP);

            shipAreaPriceHash.Remove(paramSysNo);

            return result;
        }
        #endregion

        #region DeliveryTime-Workday
        public static bool IsAreaTimeliness(int ShipTypeSysNo, int AreaSysNo)
        {
            string sql = "select count(*) from area_timeliness where shiptypesysno=" + ShipTypeSysNo + " and areasysno=" +
                         AreaSysNo + " and status=" + (int)AppEnum.BiStatus.Valid;
            int rowCount = Util.TrimIntNull(SqlHelper.ExecuteScalar(sql).ToString());
            if (rowCount > 0)
                return true;
            else
                return false;
        }

        private static void Map(WorkdayInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.Name = Util.TrimNull(tempdr["Name"]);
            oParam.Date = Util.TrimDateNull(tempdr["Date"]);
            oParam.TimeSpan = Util.TrimIntNull(tempdr["TimeSpan"]);
            oParam.Week = Util.TrimIntNull(tempdr["Week"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
        }

        public static SortedList GetDeliveryTime(bool HasEnoughAvailableQty, DateTime OrderTime, int Top)
        {
            string sToday = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime dt11 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " 11:30");  //上午截止11:30

            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
                dt11 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " 11:30");  //周六上午截止11:30
            else if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                dt11 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " 00:01");  //周日上午截止00:01


            DateTime dt17 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59");  //下午截止23:59
            if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                dt17 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " 18:00");  //周日下午截止18:00

            DateTime dt15 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " 18:00");  //周日下午截止18:00

            int iTimeSpan = 0;
            if (OrderTime.CompareTo(dt11) <= 0)  //当天11:30前
            {
                iTimeSpan = 1;
            }
            else if (OrderTime.CompareTo(dt17) <= 0)  //当天23:59前
            {
                iTimeSpan = 2;
            }
            else  //算到第二天上午
            {
                sToday = Convert.ToDateTime(sToday).AddDays(1).ToString(AppConst.DateFormat);
                iTimeSpan = 1;
            }

            string sql = "select @top * from workday where sysno >= (select sysno from workday where [date]='" + sToday + "' and timespan='" + iTimeSpan + "') order by sysno";
            sql = sql.Replace("@top", " top " + Convert.ToString(Top + 10));

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;

            int iRowCount = 0;
            SortedList sl = new SortedList();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (iRowCount == 0 || Util.TrimIntNull(dr["status"]) == (int)AppEnum.BiStatus.Valid)
                {
                    WorkdayInfo oInfo = new WorkdayInfo();
                    Map(oInfo, dr);
                    sl.Add(oInfo, null);

                    iRowCount++;
                    if (iRowCount == Top)
                        break;
                }
            }
            if (Util.TrimIntNull(ds.Tables[0].Rows[0]["status"]) != (int)AppEnum.BiStatus.Valid)  //计算时段为非工作时间
                sl.RemoveAt(0);
            if (!HasEnoughAvailableQty)  //可用数量不足推迟半天,
            {
                sl.RemoveAt(0);
            }
            sl.RemoveAt(0);

            if (!HasEnoughAvailableQty) //周六下午和周日下的订单，需要到下周二发货
            {
                if ((DateTime.Now.DayOfWeek == DayOfWeek.Saturday && DateTime.Now > dt15) || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                {
                    WorkdayInfo tempInfo = (WorkdayInfo)sl.GetKey(0);
                    if (tempInfo.Date.DayOfWeek == DayOfWeek.Sunday)
                        sl.RemoveAt(0);

                    tempInfo = (WorkdayInfo)sl.GetKey(0);
                    if (tempInfo.Date.DayOfWeek == DayOfWeek.Sunday)
                        sl.RemoveAt(0);

                    tempInfo = (WorkdayInfo)sl.GetKey(0);
                    if (tempInfo.Date.DayOfWeek == DayOfWeek.Monday)
                        sl.RemoveAt(0);

                    tempInfo = (WorkdayInfo)sl.GetKey(0);
                    if (tempInfo.Date.DayOfWeek == DayOfWeek.Monday)
                        sl.RemoveAt(0);
                }
            }

            return sl;
        }

        public static int InsertWorkday(WorkdayInfo oParam)
        {
            return new WorkdayDac().Insert(oParam);
        }

        public static DataSet GetOneWorkday(WorkdayInfo oParam)
        {
            string sql = "select * from workday where date=" + Util.ToSqlString(oParam.Date.ToString(AppConst.DateFormat)) + " and TimeSpan=" + oParam.TimeSpan;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                return ds;
            else
                return null;
        }

        public static WorkdayInfo LoadWorkday(int sysno)
        {
            string sql = "select * from workday where sysno=" + sysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                WorkdayInfo oInfo = new WorkdayInfo();
                Map(oInfo, ds.Tables[0].Rows[0]);
                return oInfo;
            }
            else
                return null;
        }

        public static int UpdateWorkday(WorkdayInfo oParam)
        {
            return new WorkdayDac().Update(oParam);
        }

        public static DataSet GetWorkdayDs(Hashtable paramHash)
        {
            string sql = "select * from workday where 1=1 ";
            StringBuilder sb = new StringBuilder();
            if (paramHash.Count > 0)
            {
                foreach (string key in paramHash.Keys)
                {
                    object item = paramHash[key];
                    sb.Append(" and ");
                    if (key == "datefrom")
                    {
                        sb.Append("[date]").Append(">=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "dateto")
                    {
                        sb.Append("[date]").Append("<=").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "Status")
                    {
                        sb.Append("status").Append("=").Append(item.ToString());
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
            }
            sql += sb.ToString() + " order by [date],timespan";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                return ds;
            else
                return null;
        }

        public static void AddWorkdayPeriod(string datefrom, string dateto)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                DateTime dtfrom = Convert.ToDateTime(datefrom);
                DateTime dtto = Convert.ToDateTime(dateto);

                string sql = "select top 1 * from workday where [date] = '" + dtfrom.AddDays(-1).ToString(AppConst.DateFormat) + "' order by [date] desc";
                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                if (!Util.HasMoreRow(ds))
                {
                    throw new BizException("请按顺序添加工作日");
                }

                int dayCount = Util.DateDiff(dtfrom, dtto);
                for (int i = 0; i <= dayCount; i++)
                {
                    DateTime curDay = dtfrom.AddDays(i);
                    WorkdayInfo oInfo = new WorkdayInfo();
                    int weekid = Util.GetWeekID(curDay.DayOfWeek);
                    string weekname = Util.GetWeekName(weekid);
                    oInfo.Name = curDay.ToString(AppConst.DateFormat) + " " + weekname + " 上午";
                    oInfo.Date = curDay;
                    oInfo.TimeSpan = 1;
                    oInfo.Week = weekid;
                    if (weekid != 7)
                        oInfo.Status = (int)AppEnum.BiStatus.Valid;
                    else
                        oInfo.Status = (int)AppEnum.BiStatus.InValid;

                    if (GetOneWorkday(oInfo) != null)
                        throw new BizException("重复的工作日");
                    InsertWorkday(oInfo);

                    oInfo.Name = curDay.ToString(AppConst.DateFormat) + " " + weekname + " 下午";
                    oInfo.TimeSpan = 2;
                    if (GetOneWorkday(oInfo) != null)
                        throw new BizException("重复的工作日");
                    InsertWorkday(oInfo);
                }
                scope.Complete();
            }
        }
        public bool IsLocal(int areasysno)
        {
            bool result = true;
            string sql = @"select IsLocal from area where sysno=" + areasysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Util.TrimIntNull(dr["IsLocal"]) == (int)AppEnum.YNStatus.Yes)
                    result = true;
                else
                    result = false;
            }
            return result;
        }

        public DataSet GetAreaDs(Hashtable paramHash)
        {
            string sql = @"select * from Area where 1=1  @AreaSysNo  @AreaType ";
            if (paramHash.ContainsKey("AreaType"))
            {
                sql = sql.Replace("@AreaType", "and " + paramHash["AreaType"].ToString());
            }
            else
            {
                sql = sql.Replace("@AreaType", "");
            }
            if (paramHash.ContainsKey("AreaSysNo"))
            {
                sql = sql.Replace("@AreaSysNo", "and " + paramHash["AreaSysNo"].ToString());
            }
            else
            {
                sql = sql.Replace("@AreaSysNo", "");
            }
            return SqlHelper.ExecuteDataSet(sql);
        }
        #endregion

        #region  PayPlusCharge Section
        //public int InsertPayPlusCharge(PayPlusChargeInfo oParam)
        //{
        //    string sql = "select * from paypluscharge where BankName = " + Util.ToSqlString(oParam.BankName) + " and InstallmentNum =" + oParam.InstallmentNum;
        //    DataSet ds = SqlHelper.ExecuteDataSet(sql);
        //    if (Util.HasMoreRow(ds))
        //        throw new BizException("已经存在同一银行的相同分期数");

        //    oParam.SysNo = SequenceDac.GetInstance().Create("PayPlusCharge_Sequence");
        //    int result = new ASPDac().InsertPayPlusCharge(oParam);
        //    SyncManager.GetInstance().SetDbLastVersion((int)AppEnum.Sync.ASP);

        //    if (payPlusChargeHash == null)
        //        payPlusChargeHash = new Hashtable(10);
        //    payPlusChargeHash.Add(oParam.SysNo, oParam);
        //    return result;
        //}

        //public int UpdatePayPlusCharge(PayPlusChargeInfo oParam)
        //{
        //    string sql = "select * from paypluscharge where BankName=" + Util.ToSqlString(oParam.BankName) + " and InstallmentNum =" + oParam.InstallmentNum + " and sysno <>" + oParam.SysNo;
        //    DataSet ds = SqlHelper.ExecuteDataSet(sql);
        //    if (Util.HasMoreRow(ds))
        //        throw new BizException("已经存在同一银行的相同分期数");

        //    if (payPlusChargeHash == null || !payPlusChargeHash.ContainsKey(oParam.SysNo))
        //        throw new BizException("更新失败");

        //    int result = new ASPDac().UpdatePayPlusCharge(oParam);
        //    SyncManager.GetInstance().SetDbLastVersion((int)AppEnum.Sync.ASP);

        //    payPlusChargeHash.Remove(oParam.SysNo);
        //    payPlusChargeHash.Add(oParam.SysNo, oParam);

        //    return result;
        //}
        //public PayPlusChargeInfo LoadPayPlusCharge(int paramSysNo)
        //{
        //    if (payPlusChargeHash == null || !payPlusChargeHash.ContainsKey(paramSysNo))
        //        return null;
        //    return (PayPlusChargeInfo)payPlusChargeHash[paramSysNo];
        //}
        //public SortedList GetPayPlusChargeList(bool isValidOnly)
        //{
        //    if (payPlusChargeHash == null)
        //        return null;

        //    SortedList sl = null;
        //    foreach (PayPlusChargeInfo item in payPlusChargeHash.Values)
        //    {
        //        if ((isValidOnly && item.Status == (int)AppEnum.BiStatus.Valid) || !isValidOnly)
        //        {
        //            if (sl == null)
        //                sl = new SortedList(20);
        //            sl.Add(item, null);
        //        }
        //    }
        //    return sl;
        //}

        #endregion

        public void InsertFreightManArea(string FreightMenList, string AreaSysNoList)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                string[] FreightMen = FreightMenList.Split(',');
                string[] AreaSysNo = AreaSysNoList.Split(',');

                for (int i = 0; i < FreightMen.Length; i++)
                {
                    for (int j = 0; j < AreaSysNo.Length; j++)
                    {
                        string sql = "select * from FreightMan_Area where FreightUserSysNo = " + Int32.Parse(FreightMen[i]) + " and AreaSysNo = " + Int32.Parse(AreaSysNo[j]);
                        DataSet ds = SqlHelper.ExecuteDataSet(sql);
                        if (!Util.HasMoreRow(ds))
                        {
                            FreightManAreaInfo oParam = new FreightManAreaInfo();
                            oParam.FreightUserSysNo = Int32.Parse(FreightMen[i]);
                            oParam.AreaSysNo = Int32.Parse(AreaSysNo[j]);
                            int result = new ASPDac().InsertFreightManArea(oParam);
                            SyncManager.GetInstance().SetDbLastVersion((int)AppEnum.Sync.ASP);

                            if (freightManAreaHash == null)
                                freightManAreaHash = new Hashtable(10);
                            freightManAreaHash.Add(oParam.SysNo, oParam);

                        }

                    }
                }
                scope.Complete();

            }


        }
        public int DeleteFreightManArea(int paramSysNo)
        {
            if (freightManAreaHash == null || !freightManAreaHash.ContainsKey(paramSysNo))
                throw new BizException("delete failed");

            int result = new ASPDac().DeleteFreightManArea(paramSysNo);
            SyncManager.GetInstance().SetDbLastVersion((int)AppEnum.Sync.ASP);

            freightManAreaHash.Remove(paramSysNo);

            return result;
        }

        public bool CheckFreightManByArea(int areaSysNo, int freightUserSysNo)
        {
            //先判断该快递有没有设置匹配区域
            //判断该区域的省市区是否在匹配范围内

            bool tag = true;


            foreach (FreightManAreaInfo item in freightManAreaHash.Values)
            {
                AreaInfo oArea = ASPManager.GetInstance().LoadArea(item.AreaSysNo);
                if (item.FreightUserSysNo == freightUserSysNo)
                {
                    tag = false;

                    if (areaSysNo == oArea.SysNo || areaSysNo == oArea.CitySysNo || areaSysNo == oArea.ProvinceSysNo)
                    {
                        tag = true;
                        break;
                    }
                }
            }
            return tag;


        }

        public void UpdateBranchUser(int usersysno, int branchsysno)
        {
            UserInfo oUser = SysManager.GetInstance().LoadUser(usersysno);

            if (branchsysno != AppConst.IntNull)
            {
                if (branchUserHash == null)
                    branchUserHash = new Hashtable(10);
                if (!branchUserHash.ContainsKey(oUser.SysNo))
                    branchUserHash.Add(oUser.SysNo, oUser);

            }
            else
            {
                if (branchUserHash != null && branchUserHash.ContainsKey(oUser.SysNo))
                    branchUserHash.Remove(oUser.SysNo);
            }

            SyncManager.GetInstance().SetDbLastVersion((int)AppEnum.Sync.ASP);
        }
    }
}