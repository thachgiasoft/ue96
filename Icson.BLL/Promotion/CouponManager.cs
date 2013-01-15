using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Security.Cryptography;
using System.Transactions;

using Icson.Objects;
using Icson.Objects.Promotion;
using Icson.DBAccess.Promotion;
using Icson.DBAccess;
using Icson.Utils;
using Icson.Objects.Sale;

namespace Icson.BLL.Promotion
{
    public class CouponManager
    {
        private CouponManager()
        {

        }

        private static CouponManager _instance;
        public static CouponManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new CouponManager();
            }
            return _instance;
        }

        #region map zone
        private void map(CouponInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.CouponID = Util.TrimNull(tempdr["CouponID"]);
            oParam.CouponName = Util.TrimNull(tempdr["CouponName"]);
            oParam.CouponCode = Util.TrimNull(tempdr["CouponCode"]);
            oParam.CouponAmt = Util.TrimDecimalNull(tempdr["CouponAmt"]);
            oParam.SaleAmt = Util.TrimDecimalNull(tempdr["SaleAmt"]);
            oParam.CouponType = Util.TrimIntNull(tempdr["CouponType"]);
            oParam.ValidTimeFrom = Util.TrimDateNull(tempdr["ValidTimeFrom"]);
            oParam.ValidTimeTo = Util.TrimDateNull(tempdr["ValidTimeTo"]);
            oParam.MaxUseDegree = Util.TrimIntNull(tempdr["MaxUseDegree"]);
            oParam.UsedDegree = Util.TrimIntNull(tempdr["UsedDegree"]);
            oParam.CreateUserSysNo = Util.TrimIntNull(tempdr["CreateUserSysNo"]);
            oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
            oParam.AuditUserSysNo = Util.TrimIntNull(tempdr["AuditUserSysNo"]);
            oParam.AuditTime = Util.TrimDateNull(tempdr["AuditTime"]);
            oParam.UsedTime = Util.TrimDateNull(tempdr["UsedTime"]);
            oParam.BatchNo = Util.TrimIntNull(tempdr["BatchNo"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
            oParam.CategorySysNoCom = Util.TrimNull(tempdr["CategorySysNoCom"]);
            oParam.ProductSysNoCom = Util.TrimNull(tempdr["ProductSysNoCom"]);
            oParam.ManufactorySysNoCom = Util.TrimNull(tempdr["ManufactorySysNoCom"]);
            oParam.UseAreaSysNoCom = Util.TrimNull(tempdr["UseAreaSysNoCom"]);
            oParam.UseCustomerSysNo = Util.TrimIntNull(tempdr["UseCustomerSysNo"]);
            oParam.UseCustomerGradeCom = Util.TrimNull(tempdr["UseCustomerGradeCom"]);
        }
        #endregion

        public int GetMaxBatchNo()
        {
            string sql = "Select ISNULL( max(BatchNo),0) BatchNo from Coupon";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                return Util.TrimIntNull(ds.Tables[0].Rows[0]["BatchNo"]);
            }
            else
                return 0;
        }

        public void CreateCoupon(CouponInfo oriInfo, int qty)
        {
            string sql = @"SELECT couponcode FROM Coupon";
            DataSet couponDs = SqlHelper.ExecuteDataSet(sql);
            ArrayList pwdList = new ArrayList();
            if (Util.HasMoreRow(couponDs))
            {
                foreach (DataRow dr in couponDs.Tables[0].Rows)
                {
                    pwdList.Add(dr["couponcode"].ToString());
                }
            }

            if (oriInfo.CouponCode != "")
            {
                int count = 0;
                if (!pwdList.Contains(oriInfo.CouponCode))
                {
                    sql = @"SELECT  MAX(SUBSTRING(CouponID, 2,9)) MaxCouponID
                                FROM    Coupon";
                    DataSet dsMaxNo = SqlHelper.ExecuteDataSet(sql);
                    if (Util.HasMoreRow(dsMaxNo))
                    {
                        if (dsMaxNo.Tables[0].Rows.Count == 1)
                        {
                            foreach (DataRow dr in dsMaxNo.Tables[0].Rows)
                            {
                                if (dr["MaxCouponID"] != null && dr["MaxCouponID"].ToString() != string.Empty)
                                {
                                    count = int.Parse(dr["MaxCouponID"].ToString());
                                    qty += int.Parse(dr["MaxCouponID"].ToString());
                                }
                            }
                        }
                    }

                    oriInfo.CouponID = oriInfo.CouponType.ToString() + (count + 1).ToString().PadLeft(9, '0');
                    if (qty - count > 1)
                        throw new BizException("指定密码的优惠券一次只能生成一个");

                    CouponDac couponDac = new CouponDac();
                    TransactionOptions options = new TransactionOptions();
                    options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                    options.Timeout = TransactionManager.DefaultTimeout;

                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
                    {
                        oriInfo.SysNo = SequenceDac.GetInstance().Create("Coupon_Sequence");
                        oriInfo.CreateTime = DateTime.Now;
                        couponDac.Insert(oriInfo);
                        scope.Complete();
                    }
                }
                else
                    throw new BizException("您指定要生成的优惠券密码已经被使用，请使用其它密码再生成");
            }
            else
            {
                PasswordGenerator pwdGen = new PasswordGenerator();
                int count = 0;

                sql = @"SELECT  MAX(SUBSTRING(CouponID, 2,9)) MaxCouponID
                                FROM    Coupon";
                DataSet dsMaxNo = SqlHelper.ExecuteDataSet(sql);
                if (Util.HasMoreRow(dsMaxNo))
                {
                    if (dsMaxNo.Tables[0].Rows.Count == 1)
                    {
                        foreach (DataRow dr in dsMaxNo.Tables[0].Rows)
                        {
                            if (dr["MaxCouponID"] != null && dr["MaxCouponID"].ToString() != string.Empty)
                            {
                                count = int.Parse(dr["MaxCouponID"].ToString());
                                qty += int.Parse(dr["MaxCouponID"].ToString());
                            }
                        }
                    }
                }

                //生成优惠券

                ArrayList newList = new ArrayList();
                while (count < qty)
                {
                    string pwd = pwdGen.Generate();
                    if (!pwdList.Contains(pwd))
                    {
                        count++;
                        CouponInfo oCoupon = new CouponInfo();
                        oCoupon.CouponName = oriInfo.CouponName;
                        oCoupon.CouponID = oriInfo.CouponType.ToString() + count.ToString().PadLeft(9, '0');
                        oCoupon.CouponCode = pwd;
                        oCoupon.CouponAmt = oriInfo.CouponAmt;
                        oCoupon.SaleAmt = oriInfo.SaleAmt;
                        oCoupon.CouponType = oriInfo.CouponType;
                        oCoupon.ValidTimeFrom = oriInfo.ValidTimeFrom;
                        oCoupon.ValidTimeTo = oriInfo.ValidTimeTo;
                        oCoupon.MaxUseDegree = oriInfo.MaxUseDegree;
                        oCoupon.UsedDegree = oriInfo.UsedDegree;
                        oCoupon.CreateTime = oriInfo.CreateTime;
                        oCoupon.CreateUserSysNo = oriInfo.CreateUserSysNo;
                        oCoupon.BatchNo = oriInfo.BatchNo;
                        oCoupon.Status = oriInfo.Status;
                        oCoupon.CategorySysNoCom = oriInfo.CategorySysNoCom;
                        oCoupon.ProductSysNoCom = oriInfo.ProductSysNoCom;
                        oCoupon.ManufactorySysNoCom = oriInfo.ManufactorySysNoCom;
                        oCoupon.UseAreaSysNoCom = oriInfo.UseAreaSysNoCom;
                        oCoupon.UseCustomerSysNo = oriInfo.UseCustomerSysNo;
                        oCoupon.UseCustomerGradeCom = oriInfo.UseCustomerGradeCom;

                        pwdList.Add(pwd);
                        newList.Add(oCoupon);
                    }
                }

                if (newList.Count > 0)
                {
                    CouponDac couponDac = new CouponDac();
                    foreach (CouponInfo oCoupon in newList)
                    {
                        TransactionOptions options = new TransactionOptions();
                        options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                        options.Timeout = TransactionManager.DefaultTimeout;

                        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
                        {
                            oCoupon.SysNo = SequenceDac.GetInstance().Create("Coupon_Sequence");
                            oCoupon.CreateTime = DateTime.Now;
                            couponDac.Insert(oCoupon);
                            scope.Complete();
                        }
                    }
                }
            }
        }

        public void AuditCoupon(ArrayList sysNoList, int auditUserSysNo)
        {
            string sql = "Update Coupon Set Status = " + (int)AppEnum.CouponStatus.Activation + ",AuditUserSysNo=" + auditUserSysNo + ",AuditTime=" + Util.ToSqlString(DateTime.Now.ToString(AppConst.DateFormatLong)) + @"
                          Where Status=" + (int)AppEnum.CouponStatus.Origin + " AND SysNo IN " + Util.ToSqlInString(sysNoList);
            SqlHelper.ExecuteNonQuery(sql);
        }

        public void CancelAuditCoupon(ArrayList sysNoList, int auditUserSysNo)
        {
            string sql = "Update Coupon Set Status = " + (int)AppEnum.CouponStatus.Origin + ",AuditUserSysNo=" + auditUserSysNo + ",AuditTime=" + Util.ToSqlString(DateTime.Now.ToString(AppConst.DateFormatLong)) + @"
                          Where Status=" + (int)AppEnum.CouponStatus.Activation + " AND SysNo IN " + Util.ToSqlInString(sysNoList);
            SqlHelper.ExecuteNonQuery(sql);
        }

        public void AbandonCoupon(ArrayList sysNoList, int optUserSysNo)
        {
            string sql = "Update Coupon Set Status = " + (int)AppEnum.CouponStatus.Abandon + @"
                          Where Status=" + (int)AppEnum.CouponStatus.Origin + " AND SysNo IN " + Util.ToSqlInString(sysNoList);
            SqlHelper.ExecuteNonQuery(sql);
        }

        public void CancelAbandonCoupon(ArrayList sysNoList, int optUserSysNo)
        {
            string sql = "Update Coupon Set Status = " + (int)AppEnum.CouponStatus.Origin + @"
                          Where Status=" + (int)AppEnum.CouponStatus.Abandon + " AND SysNo IN " + Util.ToSqlInString(sysNoList);
            SqlHelper.ExecuteNonQuery(sql);
        }


        /// <summary>
        /// 检测优惠券是否能使用于SO
        /// </summary>
        /// <param name="oCoupon"></param>
        /// <param name="soInfo"></param>
        public void CheckCouponSO(CouponInfo oCoupon, SOInfo soInfo)
        {
            //有效期
            if (oCoupon.ValidTimeTo < DateTime.Now)
            {
                throw new BizException("优惠券已经过期，不能使用");
            }
            else if (oCoupon.ValidTimeFrom > DateTime.Now)
            {
                throw new BizException("优惠券尚未生效，不能使用");
            }
            //状态
            else if (oCoupon.Status != (int)AppEnum.CouponStatus.Activation && oCoupon.Status != (int)AppEnum.CouponStatus.PartlyUsed)
            {
                throw new BizException("此优惠券没有被激活或已经使用，不能使用");
            }
            //使用次数
            else if (oCoupon.MaxUseDegree == oCoupon.UsedDegree)
            {
                throw new BizException("该优惠券已达到最高使用次数，不能再使用");
            }

            DataSet soItemDS = new DataSet();  //销售单的明细商品（仅主商品），增加大、中、小类，增加品牌项
            if (soInfo.ItemHash.Count > 0)
            {
                string sysNoStr = "";
                foreach (SOItemInfo soItem in soInfo.ItemHash.Values)
                {
                    if (soItem.ProductType == (int)AppEnum.SOItemType.ForSale)
                    {
                        sysNoStr += soItem.ProductSysNo + ",";
                    }
                }
                sysNoStr = sysNoStr.TrimEnd(',');
                if (sysNoStr != "")
                {
                    string sql = @"
SELECT  Product.SysNo,Product.ManufacturerSysNo,Product.C3SysNo,Category3.C2SysNo,Category2.C1SysNo
FROM    Product (NOLOCK)
        JOIN Category3 (NOLOCK) ON Category3.SysNo = product.C3SysNo
        JOIN Category2 (NOLOCK) ON Category2.SysNo = Category3.C2SysNo
        JOIN Category1 (NOLOCK) ON Category1.SysNo = Category2.C1SysNo
WHERE Product.SysNo IN(" + sysNoStr + ")";
                    soItemDS = SqlHelper.ExecuteDataSet(sql);
                    if (Util.HasMoreRow(soItemDS))
                    {
                        soItemDS = ConvertSOItemDS(soItemDS, soInfo);
                    }
                }
            }
            else
            {
                throw new BizException("没有购买任何商品");
            }

            #region 优惠券类型 的类别/商品/品牌 检测

            if (oCoupon.CouponType == (int)AppEnum.CouponType.Category)
            {
                string cstr = oCoupon.CategorySysNoCom;
                string[] cList = cstr.Split(',');
                if (cList.Length > 0)
                {
                    ArrayList c1List = new ArrayList();
                    ArrayList c2List = new ArrayList();
                    ArrayList c3List = new ArrayList();

                    for (int i = 0; i < cList.Length; i++)
                    {
                        string[] cx = cList[i].ToString().Split('_');
                        switch (cx[0].ToString())
                        {
                            case "c1":
                            case "C1":
                                c1List.Add(Util.TrimIntNull(cx[1]));
                                break;
                            case "c2":
                            case "C2":
                                c2List.Add(Util.TrimIntNull(cx[1]));
                                break;
                            case "c3":
                            case "C3":
                                c3List.Add(Util.TrimIntNull(cx[1]));
                                break;
                            default:
                                break;
                        }
                    }

                    decimal categorybuyAmt = 0;
                    bool IsContainsCategory = false;

                    foreach (DataRow dr in soItemDS.Tables[0].Rows)
                    {
                        int c1SysNo = Util.TrimIntNull(dr["C1SysNo"]);
                        int c2SysNo = Util.TrimIntNull(dr["C2SysNo"]);
                        int c3SysNo = Util.TrimIntNull(dr["C3SysNo"]);
                        int currqty = Util.TrimIntNull(dr["Qty"]);
                        decimal currprice = Util.TrimDecimalNull(dr["Price"]);
                        bool isInCategory = false;

                        if (c1List.Contains(c1SysNo))
                        {
                            IsContainsCategory = true;
                            isInCategory = true;
                        }
                        else if (c2List.Contains(c2SysNo))
                        {
                            IsContainsCategory = true;
                            isInCategory = true;
                        }
                        else if (c3List.Contains(c3SysNo))
                        {
                            IsContainsCategory = true;
                            isInCategory = true;
                        }

                        if (isInCategory == true)
                            categorybuyAmt += currprice * currqty;
                    }
                    if (IsContainsCategory == false)
                        throw new BizException("您使用的是类别优惠券，只能购买相应的类别商品时才能使用");
                    if (categorybuyAmt < oCoupon.SaleAmt)
                        throw new BizException("您使用的是类别优惠券，符合优惠类别的购买商品的金额总和不满足使用要求");
                }
            }
            else if (oCoupon.CouponType == (int)AppEnum.CouponType.Product)
            {
                string pstr = oCoupon.ProductSysNoCom;

                decimal productbuyAmt = 0;
                bool IsContainsProduct = false;

                foreach (DataRow dr in soItemDS.Tables[0].Rows)
                {
                    int productsysno = Util.TrimIntNull(dr["SysNo"]);
                    int currqty = Util.TrimIntNull(dr["Qty"]);
                    decimal currprice = Util.TrimDecimalNull(dr["Price"]);
                    bool isInProduct = false;

                    if (pstr.Contains(productsysno.ToString()))
                    {
                        IsContainsProduct = true;
                        isInProduct = true;
                    }

                    if (isInProduct == true)
                        productbuyAmt += currprice * currqty;
                }
                if (IsContainsProduct == false)
                    throw new BizException("您使用的是商品优惠券，只能购买相应的商品时才能使用");
                if (productbuyAmt < oCoupon.SaleAmt)
                    throw new BizException("您使用的是商品优惠券，符合优惠商品的购买商品的金额总和不满足使用要求");
            }
            else if (oCoupon.CouponType == (int)AppEnum.CouponType.Manufactory)
            {
                string mstr = oCoupon.ManufactorySysNoCom;

                decimal manufactorybuyAmt = 0;
                bool IsContainsManufactory = false;

                foreach (DataRow dr in soItemDS.Tables[0].Rows)
                {
                    int manufacturerSysNo = Util.TrimIntNull(dr["ManufacturerSysNo"]);
                    int currqty = Util.TrimIntNull(dr["Qty"]);
                    decimal currprice = Util.TrimDecimalNull(dr["Price"]);
                    bool isInManufacturer = false;

                    if (mstr.Contains(manufacturerSysNo.ToString()))
                    {
                        IsContainsManufactory = true;
                        isInManufacturer = true;
                    }

                    if (isInManufacturer == true)
                        manufactorybuyAmt += currprice * currqty;
                }
                if (IsContainsManufactory == false)
                    throw new BizException("您使用的是品牌优惠券，只能购买相应的品牌商品时才能使用");
                if (manufactorybuyAmt < oCoupon.SaleAmt)
                    throw new BizException("您使用的是品牌优惠券，符合优惠品牌的购买商品的金额总和不满足使用要求");
            }
            else if (oCoupon.CouponType == (int)AppEnum.CouponType.ALL)
            {
                decimal productbuyAmt = 0;
                foreach (DataRow dr in soItemDS.Tables[0].Rows)
                {
                    int currqty = Util.TrimIntNull(dr["Qty"]);
                    decimal currprice = Util.TrimDecimalNull(dr["Price"]);
                    productbuyAmt += currprice * currqty;
                }
                if (productbuyAmt < oCoupon.SaleAmt)
                    throw new BizException("您购买的商品的金额总和不满足优惠券使用要求");
            }
            #endregion

            //区域
            if (oCoupon.UseAreaSysNoCom != "")
            {
                Icson.Objects.Basic.AreaInfo oArea = Icson.BLL.Basic.ASPManager.GetInstance().LoadArea(soInfo.ReceiveAreaSysNo);
                bool isContainsArea = false;
                if (oArea != null)
                {
                    if (oCoupon.UseAreaSysNoCom.Contains(oArea.SysNo.ToString()))
                        isContainsArea = true;
                    else if (oCoupon.UseAreaSysNoCom.Contains(oArea.ProvinceSysNo.ToString()))
                        isContainsArea = true;
                    else if (oCoupon.UseAreaSysNoCom.Contains(oArea.CitySysNo.ToString()))
                        isContainsArea = true;
                }

                if (isContainsArea == false)
                    throw new BizException("您所在地区不符合优惠券的使用区域");
            }
            //客户
            if (oCoupon.UseCustomerSysNo != AppConst.IntNull)
            {
                if (soInfo.CustomerSysNo != oCoupon.UseCustomerSysNo)
                    throw new BizException("您不能使用该优惠券");
            }

            //客户等级
            if (oCoupon.UseCustomerGradeCom != "")
            {
                Icson.Objects.Basic.CustomerInfo oCustomer = Icson.BLL.Basic.CustomerManager.GetInstance().Load(soInfo.CustomerSysNo);
                if (!oCoupon.UseCustomerGradeCom.Contains(oCustomer.CustomerRank.ToString()))
                    throw new BizException("您的会员等级不能使用当前优惠券");
            }

        }

        /// <summary>
        /// 后台修改订单时重新检查优惠券
        /// </summary>
        /// <param name="oCoupon"></param>
        /// <param name="soInfo"></param>
        /// <returns></returns>
        public string CheckCouponSOByUpdate(CouponInfo oCoupon, SOInfo soInfo)
        {
            string result = "";
            DataSet soItemDS = new DataSet();  //销售单的明细商品（仅主商品），增加大、中、小类，增加品牌
            if (soInfo.ItemHash.Count > 0)
            {
                string sysNoStr = "";
                foreach (SOItemInfo soItem in soInfo.ItemHash.Values)
                {
                    if (soItem.ProductType == (int)AppEnum.SOItemType.ForSale)
                    {
                        sysNoStr += soItem.ProductSysNo + ",";
                    }
                }
                sysNoStr = sysNoStr.TrimEnd(',');
                if (sysNoStr != "")
                {
                    string sql = @"
SELECT  Product.SysNo,Product.ManufacturerSysNo,Product.C3SysNo,Category3.C2SysNo,Category2.C1SysNo
FROM    Product (NOLOCK)
        JOIN Category3 (NOLOCK) ON Category3.SysNo = product.C3SysNo
        JOIN Category2 (NOLOCK) ON Category2.SysNo = Category3.C2SysNo
        JOIN Category1 (NOLOCK) ON Category1.SysNo = Category2.C1SysNo
WHERE Product.SysNo IN(" + sysNoStr + ")";
                    soItemDS = SqlHelper.ExecuteDataSet(sql);
                    if (Util.HasMoreRow(soItemDS))
                    {
                        soItemDS = ConvertSOItemDS(soItemDS, soInfo);
                    }
                }
            }
            else
            {
                result = "没有订购任何商品";
                return result;
            }

            #region 优惠券类型 的类别/商品/品牌 检测

            if (oCoupon.CouponType == (int)AppEnum.CouponType.Category)
            {
                string cstr = oCoupon.CategorySysNoCom;
                string[] cList = cstr.Split(',');
                if (cList.Length > 0)
                {
                    ArrayList c1List = new ArrayList();
                    ArrayList c2List = new ArrayList();
                    ArrayList c3List = new ArrayList();

                    for (int i = 0; i < cList.Length; i++)
                    {
                        string[] cx = cList[i].ToString().Split('_');
                        switch (cx[0].ToString())
                        {
                            case "c1":
                            case "C1":
                                c1List.Add(Util.TrimIntNull(cx[1]));
                                break;
                            case "c2":
                            case "C2":
                                c2List.Add(Util.TrimIntNull(cx[1]));
                                break;
                            case "c3":
                            case "C3":
                                c3List.Add(Util.TrimIntNull(cx[1]));
                                break;
                            default:
                                break;
                        }
                    }

                    decimal categorybuyAmt = 0;
                    bool IsContainsCategory = false;

                    foreach (DataRow dr in soItemDS.Tables[0].Rows)
                    {
                        int c1SysNo = Util.TrimIntNull(dr["C1SysNo"]);
                        int c2SysNo = Util.TrimIntNull(dr["C2SysNo"]);
                        int c3SysNo = Util.TrimIntNull(dr["C3SysNo"]);
                        int currqty = Util.TrimIntNull(dr["Qty"]);
                        decimal currprice = Util.TrimDecimalNull(dr["Price"]);
                        bool isInCategory = false;

                        if (c1List.Contains(c1SysNo))
                        {
                            IsContainsCategory = true;
                            isInCategory = true;
                        }
                        else if (c2List.Contains(c2SysNo))
                        {
                            IsContainsCategory = true;
                            isInCategory = true;
                        }
                        else if (c3List.Contains(c3SysNo))
                        {
                            IsContainsCategory = true;
                            isInCategory = true;
                        }

                        if (isInCategory == true)
                            categorybuyAmt += currprice * currqty;
                    }
                    if (IsContainsCategory == false)
                    {
                        result = "使用的类别优惠券，只能针对优惠券指定的类别商品进行优惠";
                        return result;
                    }
                    if (categorybuyAmt < oCoupon.SaleAmt)
                    {
                        result = "优惠券指定的类别商品的购买总额没有达到优惠券需消费金额要求";
                        return result;
                    }
                }
            }
            else if (oCoupon.CouponType == (int)AppEnum.CouponType.Product)
            {
                string pstr = oCoupon.ProductSysNoCom;

                decimal productbuyAmt = 0;
                bool IsContainsProduct = false;

                foreach (DataRow dr in soItemDS.Tables[0].Rows)
                {
                    int productsysno = Util.TrimIntNull(dr["SysNo"]);
                    int currqty = Util.TrimIntNull(dr["Qty"]);
                    decimal currprice = Util.TrimDecimalNull(dr["Price"]);
                    bool isInProduct = false;

                    if (pstr.Contains(productsysno.ToString()))
                    {
                        IsContainsProduct = true;
                        isInProduct = true;
                    }

                    if (isInProduct == true)
                        productbuyAmt += currprice * currqty;
                }
                if (IsContainsProduct == false)
                {
                    result = "使用的商品优惠券，只能针对优惠券指定的商品进行优惠";
                    return result;
                }
                if (productbuyAmt < oCoupon.SaleAmt)
                {
                    result = "优惠券指定的商品的购买总额没有达到优惠券需消费金额要求";
                    return result;
                }
            }
            else if (oCoupon.CouponType == (int)AppEnum.CouponType.Manufactory)
            {
                string mstr = oCoupon.ManufactorySysNoCom;

                decimal manufactorybuyAmt = 0;
                bool IsContainsManufactory = false;

                foreach (DataRow dr in soItemDS.Tables[0].Rows)
                {
                    int manufacturerSysNo = Util.TrimIntNull(dr["ManufacturerSysNo"]);
                    int currqty = Util.TrimIntNull(dr["Qty"]);
                    decimal currprice = Util.TrimDecimalNull(dr["Price"]);
                    bool isInManufacturer = false;

                    if (mstr.Contains(manufacturerSysNo.ToString()))
                    {
                        IsContainsManufactory = true;
                        isInManufacturer = true;
                    }

                    if (isInManufacturer == true)
                        manufactorybuyAmt += currprice * currqty;
                }
                if (IsContainsManufactory == false)
                {
                    result = "使用的品牌优惠券，只能针对优惠券指定的品牌商品进行优惠";
                    return result;
                }
                if (manufactorybuyAmt < oCoupon.SaleAmt)
                {
                    result = "优惠券指定的品牌商品的购买总额没有达到优惠券需消费金额要求";
                    return result;
                }
            }
            else if (oCoupon.CouponType == (int)AppEnum.CouponType.ALL)
            {
                decimal productbuyAmt = 0;
                foreach (DataRow dr in soItemDS.Tables[0].Rows)
                {
                    int currqty = Util.TrimIntNull(dr["Qty"]);
                    decimal currprice = Util.TrimDecimalNull(dr["Price"]);
                    productbuyAmt += currprice * currqty;
                }
                if (productbuyAmt < oCoupon.SaleAmt)
                {
                    result = "购买商品的总额没有达到优惠券需消费金额要求";
                    return result;
                }
            }
            #endregion

            return result;
        }

        private DataSet ConvertSOItemDS(DataSet ds, SOInfo soInfo)
        {
            if (!Util.HasMoreRow(ds))
                return ds;

            ds.Tables[0].Columns.Add("Price");
            ds.Tables[0].Columns.Add("Qty");

            Hashtable hs = soInfo.ItemHash;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int key = Util.TrimIntNull(dr["SysNo"]);
                SOItemInfo item = (SOItemInfo)hs[key];
                dr["Price"] = item.Price;
                dr["Qty"] = item.Quantity;
            }

            return ds;
        }

        public bool CheckCouponValid(string couponCode)
        {
            string sql = @"select * from coupon
                           where couponcode = " + Util.ToSqlString(couponCode) + @"
                           and validTimeFrom <=" + Util.ToSqlString(DateTime.Now.ToString(AppConst.DateFormatLong)) + @"
                           and validtimeTo >= " + Util.ToSqlString(DateTime.Now.ToString(AppConst.DateFormatLong)) + @"
                           and status in(" + (int)AppEnum.CouponStatus.Activation + "," + (int)AppEnum.CouponStatus.PartlyUsed + ")";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds) && ds.Tables[0].Rows.Count == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void UseCoupon(string couponCode)
        {
            CouponInfo oCoupon = LoadCouponByPwd(couponCode);
            if (oCoupon != null)
            {
                if (oCoupon.ValidTimeTo < DateTime.Now)
                {
                    throw new BizException("优惠券已经过期，不能使用");
                }
                else if (oCoupon.ValidTimeFrom > DateTime.Now)
                {
                    throw new BizException("优惠券尚未生效，不能使用");
                }
                else if (oCoupon.Status != (int)AppEnum.CouponStatus.Activation && oCoupon.Status != (int)AppEnum.CouponStatus.PartlyUsed)
                {
                    throw new BizException("此优惠券不能使用");
                }
                if (oCoupon.MaxUseDegree - (oCoupon.UsedDegree + 1) == 0)
                    oCoupon.Status = (int)AppEnum.CouponStatus.FullUsed;
                else
                    oCoupon.Status = (int)AppEnum.CouponStatus.PartlyUsed;
                oCoupon.UsedDegree = oCoupon.UsedDegree + 1;
                oCoupon.UsedTime = DateTime.Now;
                UpdateCoupon(oCoupon);
            }
            else
            {
                throw new BizException("优惠券不存在，无法使用");
            }
        }


        public void ReleaseCoupon(string couponCode)
        {
            CouponInfo oCoupon = LoadCouponByPwd(couponCode);
            if (oCoupon != null)
            {
                oCoupon.Status = (int)AppEnum.CouponStatus.Origin;
                oCoupon.UsedTime = AppConst.DateTimeNull;
                UpdateCoupon(oCoupon);
            }
            else
            {
                throw new BizException("优惠券不存在，无法释放");
            }
        }

        public CouponInfo LoadCouponByPwd(string couponCode)
        {
            string sql = "select * from coupon where couponcode=" + Util.ToSqlString(couponCode);
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                CouponInfo oCoupon = new CouponInfo();
                map(oCoupon, ds.Tables[0].Rows[0]);
                return oCoupon;
            }
            else
            {
                return null;
            }
        }

        private void UpdateCoupon(CouponInfo oCoupon)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                new CouponDac().Update(oCoupon);
                scope.Complete();
            }
        }

        public void UpdateCoupon(ArrayList sysNoList, Hashtable paramHash)
        {
            if (paramHash != null && paramHash.Count > 0 && sysNoList.Count > 0)
            {
                StringBuilder sb = new StringBuilder(1000);
                sb.Append(" Update Coupon Set ");
                foreach (string key in paramHash.Keys)
                {
                    object item = paramHash[key];
                    if (key == "ValidTimeFrom")
                        sb.Append(" ValidTimeFrom = ").Append(Util.ToSqlString(item.ToString())).Append(",");
                    else if (key == "ValidTimeTo")
                        sb.Append(" ValidTimeTo = ").Append(Util.ToSqlEndDate(item.ToString())).Append(",");
                    else if (key == "MaxUseDegree")
                        sb.Append(" MaxUseDegree = ").Append(Util.ToSqlString(item.ToString())).Append(",");
                    else if (key == "UseCustomerGradeCom")
                        sb.Append(" UseCustomerGradeCom = ").Append(Util.ToSqlString(item.ToString())).Append(",");
                    else if (key == "UseCustomerSysNo")
                        sb.Append(" UseCustomerSysNo = ").Append(item.ToString()).Append(",");
                    else if (key == "UseAreaSysNoCom")
                        sb.Append(" UseAreaSysNoCom = ").Append(Util.ToSqlString(item.ToString())).Append(",");
                    else if (key == "CategorySysNoCom")
                        sb.Append(" CategorySysNoCom = ").Append(Util.ToSqlString(item.ToString())).Append(",");
                    else if (key == "ProductSysNoCom")
                        sb.Append(" ProductSysNoCom = ").Append(Util.ToSqlString(item.ToString())).Append(",");
                    else if (key == "ManufactorySysNoCom")
                        sb.Append(" ManufactorySysNoCom = ").Append(Util.ToSqlString(item.ToString())).Append(",");
                }

                string sql = sb.ToString();
                sql = sql.TrimEnd(',');
                sql += " WHERE Status = " + (int)AppEnum.CouponStatus.Origin + " AND SysNo IN" + Util.ToSqlInString(sysNoList);

                SqlHelper.ExecuteNonQuery(sql);
            }
        }

        public DataSet GetCoupon(Hashtable paramHash)
        {
            string sql = @" select coupon.*,customer.customername,a.username as createusername,b.username as auditusername from coupon 
                            left join customer on customer.sysno = coupon.usecustomersysno 
                            left join sys_user as a on a.sysno = coupon.createusersysno
                            left join sys_user as b on b.sysno = coupon.auditusersysno
                            where 1=1 ";
            if (paramHash != null && paramHash.Count != 0)
            {
                StringBuilder sb = new StringBuilder(100);
                foreach (string key in paramHash.Keys)
                {
                    sb.Append(" and ");
                    object item = paramHash[key];
                    if (key == "CouponID")
                    {
                        sb.Append(string.Format(" RIGHT(CouponID, {0}) ", item.ToString().Length)).Append(" = ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "ValidTimeFrom")
                    {
                        sb.Append(" ValidTimeFrom ").Append(" >=").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "ValidTimeTo")
                    {
                        sb.Append(" ValidTimeTo ").Append(" <= ").Append(Util.ToSqlEndDate(item.ToString()));
                    }
                    else if (key == "CreateTime")
                    {
                        sb.Append(" SUBSTRING(CONVERT(VARCHAR,CreateTime,20),1,10) ").Append(" =").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "Status")
                    {
                        sb.Append(" Coupon.Status ").Append(" = ").Append(item.ToString()); 
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
            sql += " order by couponID desc";
            return SqlHelper.ExecuteDataSet(sql);
        }

        public DataSet GetCustomerCoupon(int customerSysNo)
        {
            string sql = @"
SELECT  *
FROM    Coupon (NOLOCK)
WHERE   Coupon.UseCustomerSysNo = @customersysno
        AND @status 
";
            sql = sql.Replace("@customersysno", customerSysNo.ToString());
            sql = sql.Replace("@status", "Status IN(" + (int)AppEnum.CouponStatus.Activation + "," + (int)AppEnum.CouponStatus.PartlyUsed + ")");

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                ds.Tables[0].Columns.Add("UseNote");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int coupontype = Util.TrimIntNull(dr["CouponType"]);
                    string saleamt = Util.TrimDecimalNull(dr["SaleAmt"]).ToString(AppConst.DecimalFormat);
                    string typecom = "";
                    if (coupontype == (int)AppEnum.CouponType.Category)
                    {
                        typecom = dr["CategorySysNoCom"].ToString();
                        if (typecom != "")
                        {
                            string usenote = "";
                            string[] cList = typecom.TrimStart(',').TrimEnd(',').Split(',');
                            if (cList.Length > 0)
                            {
                                ArrayList c1List = new ArrayList();
                                ArrayList c2List = new ArrayList();
                                ArrayList c3List = new ArrayList();

                                for (int i = 0; i < cList.Length; i++)
                                {
                                    string[] cx = cList[i].ToString().Split('_');
                                    switch (cx[0].ToString())
                                    {
                                        case "c1":
                                        case "C1":
                                            c1List.Add(Util.TrimIntNull(cx[1]));
                                            break;
                                        case "c2":
                                        case "C2":
                                            c2List.Add(Util.TrimIntNull(cx[1]));
                                            break;
                                        case "c3":
                                        case "C3":
                                            c3List.Add(Util.TrimIntNull(cx[1]));
                                            break;
                                        default:
                                            break;
                                    }
                                }

                                string sql2 = "";
                                if (c1List.Count > 0)
                                {
                                    sql2 += "Select C1Name CName From Category1 Where SysNo IN" + Util.ToSqlInString(c1List);
                                }
                                if (c2List.Count > 0)
                                {
                                    if (sql2 != "")
                                    {
                                        sql2 += " UNION Select C2Name CName From Category2 Where SysNo IN" + Util.ToSqlInString(c2List);
                                    }
                                    else
                                    {
                                        sql2 += " Select C2Name CName From Category2 Where SysNo IN" + Util.ToSqlInString(c2List);
                                    }
                                }
                                if (c3List.Count > 0)
                                {
                                    if (sql2 != "")
                                    {
                                        sql2 += " UNION Select C3Name CName From Category3 Where SysNo IN" + Util.ToSqlInString(c3List);
                                    }
                                    else
                                    {
                                        sql2 += "Select C3Name CName From Category3 Where SysNo IN" + Util.ToSqlInString(c3List); ;
                                    }
                                }

                                if (sql2 != "")
                                {
                                    DataSet dsc = SqlHelper.ExecuteDataSet(sql2);
                                    if (Util.HasMoreRow(dsc))
                                    {
                                        usenote += "此优惠券需购买以下类别中的至少一种商品：";
                                        foreach (DataRow drc in dsc.Tables[0].Rows)
                                        {
                                            usenote += drc["CName"].ToString() + "&nbsp;&nbsp;";
                                        }
                                    }
                                }

                            }
                            //if (usenote != "")
                            //    usenote += "<br />";
                            //usenote += "购买商品总额需满" + saleamt + "元";
                            dr["UseNote"] = usenote;
                        }
                        else
                        {
                            dr["UseNote"] = "";
                        }
                    }
                    else if (coupontype == (int)AppEnum.CouponType.Product)
                    {
                        typecom = dr["ProductSysNoCom"].ToString();
                        if (typecom != "")
                        {
                            string sql2 = "select sysno,productid from product where sysno in (" + typecom.TrimStart(',').TrimEnd(',') + ")";
                            string usenote = "";
                            DataSet tempds = SqlHelper.ExecuteDataSet(sql2);
                            if (Util.HasMoreRow(tempds))
                            {
                                usenote += "此优惠券需购买以下商品中的至少一种：<br />";
                                foreach (DataRow drr in tempds.Tables[0].Rows)
                                {
                                    usenote += drr["productid"].ToString() + "&nbsp;&nbsp;";
                                }
                            }
                            //if (usenote != "")
                            //    usenote += "<br />";
                            //usenote += "购买商品总额需满" + saleamt + "元";
                            dr["UseNote"] = usenote;
                        }
                        else
                        {
                            dr["UseNote"] = "";
                        }
                    }
                    else if (coupontype == (int)AppEnum.CouponType.Manufactory)
                    {
                        typecom = dr["ManufactorySysNoCom"].ToString();
                        if (typecom != "")
                        {
                            string sql2 = "SELECT SysNo, COALESCE(Manufacturer.BriefName, Manufacturer.ManufacturerName) ManufacturerName FROM Manufacturer WHERE SysNo IN (" + typecom.TrimStart(',').TrimEnd(',') + ")";
                            string usenote = "";
                            DataSet tempds = SqlHelper.ExecuteDataSet(sql2);
                            if (Util.HasMoreRow(tempds))
                            {
                                usenote += "此优惠券需购买以下品牌中的至少一种商品：<br />";
                                foreach (DataRow drr in tempds.Tables[0].Rows)
                                {
                                    usenote += drr["ManufacturerName"].ToString() + "&nbsp;&nbsp;";
                                }
                            }
                            //if (usenote != "")
                            //    usenote += "<br />";
                            //usenote += "购买商品总额需满" + saleamt + "元";
                            dr["UseNote"] = usenote;
                        }
                        else
                        {
                            dr["UseNote"] = "";
                        }
                    }
                }

                return ds;
            }
            else
                return null;

        }

        public DataSet GetCustomerUsedCoupon(int customerSysNo)
        {
            string sql = @"
SELECT  Coupon.*,SO_Master.SOID,SO_Master.SysNo SOSysNo
FROM    Coupon (NOLOCK)
JOIN    SO_Master (NOLOCK) ON SO_Master.CouponCode = Coupon.CouponCode  
WHERE   Coupon.UseCustomerSysNo = @customersysno
        AND SO_Master.CustomerSysNo = @customersysno
        AND @status 
";
            sql = sql.Replace("@customersysno", customerSysNo.ToString());
            sql = sql.Replace("@status", "Coupon.Status IN(" + (int)AppEnum.CouponStatus.PartlyUsed + "," + (int)AppEnum.CouponStatus.FullUsed + ")");

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                return ds;
            else
                return null;
        }

        public bool IsReceiveCouponByCustomer(int customerSysNo, int couponType, int batchNo)
        {
            if (batchNo == AppConst.IntNull && couponType == AppConst.IntNull)
                return false;

            string sql = "select top 1 * from coupon where status = " + (int)AppEnum.CouponStatus.Activation + " and UseCustomerSysNo IS NULL ";
            if (couponType != AppConst.IntNull)
                sql += " and coupontype = " + couponType;
            if (batchNo != AppConst.IntNull)
                sql += " and batchno = " + batchNo;

            sql += " order by sysno ";

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                string sql1 = "Update coupon set UseCustomerSysNo =" + customerSysNo + " Where sysno =" + Util.TrimIntNull(ds.Tables[0].Rows[0]["SysNo"]);
                if (1 == SqlHelper.ExecuteNonQuery(sql1))
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        public string RequestCouponCode(int BatchNo)
        {
            string sql = @"
SELECT TOP 1
        Coupon.CouponCode
FROM    Coupon
WHERE   Coupon.Status = " + (int)AppEnum.CouponStatus.Activation + @"
        AND Coupon.UseCustomerSysNo IS NULL
        AND Coupon.BatchNo = " + BatchNo + @"
        AND NOT EXISTS ( SELECT CouponRequest.CouponCode
                         FROM   CouponRequest
                         WHERE  CouponRequest.CouponCode = Coupon.CouponCode AND CouponRequest.Status <> " + (int)AppEnum.CouponRequestStatus.Abandon + @" )
ORDER BY Coupon.SysNo
";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                return ds.Tables[0].Rows[0][0].ToString();
            else
                return "";
        }

        public DataSet GetBatchDes()
        {
            string sql = "SELECT Distinct BatchNo,( '['+ convert(nvarchar(10), BatchNo)+'] '+CouponName) BatchName FROM Coupon ORDER BY BatchNo DESC";
            return SqlHelper.ExecuteDataSet(sql);
        }

        #region CouponRequest

        private void map(CouponRequestInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.CustomerSysNo = Util.TrimIntNull(tempdr["CustomerSysNo"]);
            oParam.CouponCode = Util.TrimNull(tempdr["CouponCode"]);
            oParam.RequestUserSysNo = Util.TrimIntNull(tempdr["RequestUserSysNo"]);
            oParam.RequestTime = Util.TrimDateNull(tempdr["RequestTime"]);
            oParam.AuditUserSysNo = Util.TrimIntNull(tempdr["AuditUserSysNo"]);
            oParam.AuditTime = Util.TrimDateNull(tempdr["AuditTime"]);
            oParam.SOSysNo = Util.TrimIntNull(tempdr["SOSysNo"]);
            oParam.BatchNo = Util.TrimIntNull(tempdr["BatchNo"]);
            oParam.Note = Util.TrimNull(tempdr["Note"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
            oParam.EXECSql = Util.TrimNull(tempdr["EXECSql"]);
        }

        public CouponRequestInfo LoadCouponRequest(int SysNo)
        {
            string sql = "select * from CouponRequest where sysno = " + SysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;
            CouponRequestInfo oInfo = new CouponRequestInfo();
            map(oInfo, ds.Tables[0].Rows[0]);
            return oInfo;
        }

        public void InsertCouponRequest(CouponRequestInfo paramInfo)
        {
            if (1 != new CouponRequestDac().Insert(paramInfo))
                throw new BizException("插入客户优惠券申请记录失败");
        }

        public void UpdateCouponRequest(CouponRequestInfo paramInfo)
        {
            if (1 != new CouponRequestDac().Update(paramInfo))
                throw new BizException("更新客户优惠券申请记录失败");
        }

        public void AuditCouponRequest(ArrayList a, int auditUserSysNo)
        {
            if (a.Count > 0)
            {
                try
                {
                    for (int i = 0; i < a.Count; i++)
                    {
                        int sysno = int.Parse(a[i].ToString());
                        AuditCouponRequest(sysno, auditUserSysNo);
                    }
                }
                catch
                {
                    throw new BizException("审核失败，请重试");
                }
            }
        }


        public void AuditCouponRequest(int couponRequestSysNo, int auditUserSysNo)
        {
            CouponRequestInfo oCouponRequest = LoadCouponRequest(couponRequestSysNo);
            if (oCouponRequest == null)
                throw new BizException("当前优惠券申请记录不存在");
            CouponInfo oCoupon = LoadCouponByPwd(oCouponRequest.CouponCode);
            if (oCoupon == null)
                throw new BizException("当前优惠券不存在");
            if (oCoupon.UseCustomerSysNo != AppConst.IntNull)
                throw new BizException("当前优惠券已经被使用");

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                oCouponRequest.Status = (int)AppEnum.CouponRequestStatus.Audited;
                oCouponRequest.AuditUserSysNo = auditUserSysNo;
                oCouponRequest.AuditTime = DateTime.Now;
                UpdateCouponRequest(oCouponRequest);

                oCoupon.UseCustomerSysNo = oCouponRequest.CustomerSysNo;
                UpdateCoupon(oCoupon);

                scope.Complete();
            }
        }

        public void CancelAuditCouponRequest(ArrayList a, int auditUserSysNo)
        {
            if (a.Count > 0)
            {
                try
                {
                    for (int i = 0; i < a.Count; i++)
                    {
                        int sysno = int.Parse(a[i].ToString());
                        CancelAuditCouponRequest(sysno, auditUserSysNo);
                    }
                }
                catch
                {
                    throw new BizException("取消审核失败，请重试");
                }
            }
        }

        public void CancelAuditCouponRequest(int couponRequestSysNo, int auditUserSysNo)
        {
            CouponRequestInfo oCouponRequest = LoadCouponRequest(couponRequestSysNo);
            if (oCouponRequest == null)
                throw new BizException("当前优惠券申请记录不存在");
            CouponInfo oCoupon = LoadCouponByPwd(oCouponRequest.CouponCode);
            if (oCoupon == null)
                throw new BizException("当前优惠券不存在");
            if (oCoupon.Status != (int)AppEnum.CouponStatus.Origin && oCoupon.Status != (int)AppEnum.CouponStatus.Activation)
                throw new BizException("当前使用的优惠券已经被使用或作废，不能取消审核");

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                oCouponRequest.Status = (int)AppEnum.CouponRequestStatus.Origin;
                oCouponRequest.AuditUserSysNo = auditUserSysNo;
                oCouponRequest.AuditTime = DateTime.Now;
                UpdateCouponRequest(oCouponRequest);

                string updatecouponsql = "UPDATE Coupon set UseCustomerSysNo = NULL WHERE SysNo = " + oCoupon.SysNo;
                if (1 != SqlHelper.ExecuteNonQuery(updatecouponsql))
                    throw new BizException("更新到优惠券使用客户失败");

                scope.Complete();
            }
        }

        public void AbandonCouponRequest(ArrayList a, int AbandonUserSysNo)
        {
            string sql = "Update CouponRequest Set Status = " + (int)AppEnum.CouponRequestStatus.Abandon + " , AuditUserSysNo=" + AbandonUserSysNo + " , AuditTime =" + Util.ToSqlString(DateTime.Now.ToString(AppConst.DateFormatLong)) + @" 
                          Where Status=" + (int)AppEnum.CouponRequestStatus.Origin + " AND SysNo IN" + Util.ToSqlInString(a);
            SqlHelper.ExecuteNonQuery(sql);
        }

        public void AbandonCouponRequest(int couponRequestSysNo)
        {
            string sql = "Update CouponRequest Set Status = " + (int)AppEnum.CouponRequestStatus.Abandon + @"
                          Where Status=" + (int)AppEnum.CouponRequestStatus.Origin + " AND SysNo =" + couponRequestSysNo;
            if (1 != SqlHelper.ExecuteNonQuery(sql))
                throw new BizException("作废优惠券申请记录失败");
        }

        /// <summary>
        /// 获取客户优惠券申请记录DS
        /// </summary>
        /// <param name="paramHash"></param>
        /// <returns></returns>
        public DataSet GetCouponRequestDs(Hashtable paramHash)
        {
            string sql = @"
SELECT  CouponRequest.*,
        Customer.CustomerID,
        Customer.CustomerName,
        requestuser.UserName RequestUserName,
        audituser.UserName AuditUserName,
        SO_Master.SOID,
        SO_Master.SOAmt,
        Coupon.CouponID,
        Coupon.ValidTimeTo,
        Coupon.CouponName,
        Coupon.CouponAmt,
        Coupon.SaleAmt,
        Coupon.CouponType,
        Coupon.BatchNo,
        Coupon.Status CouponStatus
FROM    CouponRequest
        INNER JOIN Customer ON Customer.SysNo = CouponRequest.CustomerSysNo
        INNER JOIN Coupon ON Coupon.CouponCode = CouponRequest.CouponCode
        LEFT JOIN SO_Master ON SO_Master.SysNo = CouponRequest.SOSysNo
        LEFT JOIN Sys_User requestuser ON requestuser.SysNo = CouponRequest.RequestUserSysNo
        LEFT JOIN Sys_User audituser ON audituser.SysNo = CouponRequest.AuditUserSysNo
";
            if (paramHash != null && paramHash.Count > 0)
            {
                StringBuilder sb = new StringBuilder(1000);
                sb.Append(" WHERE 1=1 ");
                foreach (string key in paramHash.Keys)
                {
                    object item = paramHash[key];
                    sb.Append(" AND ");

                    if (key == "CustomerID")
                        sb.Append(" Customer.CustomerID ").Append(" = ").Append(Util.ToSqlString(item.ToString()));
                    else if (key == "Status")
                        sb.Append(" CouponRequest.Status ").Append(" = ").Append(item.ToString());
                    else if (key == "BatchNo")
                        sb.Append(" CouponRequest.BatchNo ").Append(" = ").Append(item.ToString());
                    else if (key == "SOID")
                        sb.Append(" SO_Master.SOID ").Append(" = ").Append(Util.ToSqlString(item.ToString()));
                    else if (key == "RequestTimeFrom")
                        sb.Append(" CouponRequest.RequestTime ").Append(" >= ").Append(Util.ToSqlString(item.ToString()));
                    else if (key == "RequestTimeTo")
                        sb.Append(" CouponRequest.RequestTime ").Append(" <= ").Append(Util.ToSqlEndDate(item.ToString()));
                    else if (key == "CouponType")
                        sb.Append(" Coupon.CouponType ").Append(" = ").Append(item.ToString());
                    else if (key == "CouponID")
                        sb.Append(" Coupon.CouponID ").Append(" = ").Append(Util.ToSqlString(item.ToString()));
                    else if (key == "OrderBy")
                        sb.Append(" 1=1 ");
                    else if (item is int)
                        sb.Append(key).Append(" = ").Append(item.ToString());
                    else if (item is string)
                        sb.Append(key).Append(" = ").Append(Util.ToSqlString(item.ToString()));
                    else if (item is DateTime)
                        sb.Append(key).Append(" = cast(").Append(Util.ToSqlString(((DateTime)item).ToString(AppConst.DateFormatLong))).Append(" as DateTime )");
                    else if (item is DBNull)
                        sb.Append(key).Append(" = null ");
                }

                sql += sb.ToString();
            }

            if(paramHash.ContainsKey("OrderBy"))
                sql += " ORDER BY "+paramHash["OrderBy"].ToString();
            return SqlHelper.ExecuteDataSet(sql);
        }

        /// <summary>
        /// 仅用于获取需要赠送优惠券的客户时使用
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataSet GetCustomerBySql(string sql)
        {
            return SqlHelper.ExecuteDataSet(sql);
        }

        /// <summary>
        /// 批量申请优惠券
        /// </summary>
        /// <param name="batchNo">批次号</param>
        /// <param name="alist">申请优惠券的CustomerSysNo列表</param>
        /// <param name="note">备注</param>
        /// <param name="requestUserSysNo">申请操作人</param>
        /// <param name="execsqlstr">查询批量客户的sql语句，没有传""，且一批客户中数据库只存一次execsql</param>
        public void RequestCouponBatch(int batchNo, ArrayList alist, string note, int requestUserSysNo,string execsqlstr)
        {
            int count = alist.Count;
            if (count <= 0)
                throw new BizException("没有需要申请优惠券的客户");

            string sql = @"
SELECT  TOP " + count + @"
        Coupon.CouponCode
FROM    Coupon
WHERE   Coupon.Status = " + (int)AppEnum.CouponStatus.Activation + @"
        AND Coupon.UseCustomerSysNo IS NULL
        AND Coupon.BatchNo = " + batchNo + @"
        AND NOT EXISTS ( SELECT CouponRequest.CouponCode
                         FROM   CouponRequest
                         WHERE  CouponRequest.CouponCode = Coupon.CouponCode AND CouponRequest.Status <> " + (int)AppEnum.CouponRequestStatus.Abandon + @" )
ORDER BY Coupon.SysNo
";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                throw new BizException("当前批次没有可以使用的优惠券");

            int couponcount = ds.Tables[0].Rows.Count;
            if (couponcount < count)
                throw new BizException("当前申请优惠券的数量是 " + count + " 张,但系统只有 " + couponcount + " 张可以使用，请先补充该批次可以使用的优惠券" + (count - couponcount) + " 张后再操作");

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                for (int i = 0; i < count; i++)
                {
                    CouponRequestInfo oInfo = new CouponRequestInfo();
                    oInfo.CustomerSysNo = Util.TrimIntNull(alist[i].ToString());
                    oInfo.CouponCode = ds.Tables[0].Rows[i][0].ToString();
                    oInfo.RequestUserSysNo = requestUserSysNo;
                    oInfo.RequestTime = DateTime.Now;
                    oInfo.BatchNo = batchNo;
                    oInfo.Note = note;
                    oInfo.Status = (int)AppEnum.CouponRequestStatus.Origin;
                    if (i == 0)
                        oInfo.EXECSql = execsqlstr;

                    CouponManager.GetInstance().InsertCouponRequest(oInfo);
                }

                scope.Complete();
            }
        }

        /// <summary>
        /// 单个申请优惠券
        /// </summary>
        /// <param name="batchNo"></param>
        /// <param name="customerSysNo"></param>
        /// <param name="soSysNo"></param>
        /// <param name="?"></param>
        public void RequestCoupon(int batchNo, int customerSysNo, int requestUserSysNo, int soSysNo, string note)
        {
            CouponRequestInfo oInfo = new CouponRequestInfo();
            oInfo.CustomerSysNo = customerSysNo;
            string couponcode = RequestCouponCode(batchNo);
            if (couponcode == "")
                throw new BizException("获取优惠券密码失败");
            oInfo.CouponCode = couponcode;
            oInfo.RequestUserSysNo = requestUserSysNo;
            oInfo.RequestTime = DateTime.Now;
            oInfo.BatchNo = batchNo;
            oInfo.SOSysNo = soSysNo;
            oInfo.Note = note;
            oInfo.Status = (int)AppEnum.CouponRequestStatus.Origin;

            InsertCouponRequest(oInfo);
        }

        #endregion End CouponRequest
    }

    #region PasswordGenerator
    public class PasswordGenerator
    {
        public PasswordGenerator()
        {
            Minimum = DefaultMinimum;
            Maximum = DefaultMaximum;
            ConsecutiveCharacters = false;
            RepeatCharacters = true;
            ExcludeSymbols = false;
            Exclusions = null;
            rng = new RNGCryptoServiceProvider();
        }

        protected int GetCryptographicRandomNumber(int lBound, int uBound)
        {
            // 假定 lBound >= 0 && lBound < uBound
            // 返回一个 int >= lBound and < uBound
            uint urndnum;
            byte[] rndnum = new Byte[4];

            if (lBound == uBound - 1)
            {
                // 只有iBound返回的情况 
                return lBound;
            }

            uint xcludeRndBase = (uint.MaxValue - (uint.MaxValue % (uint)(uBound - lBound)));
            do
            {
                rng.GetBytes(rndnum);
                urndnum = BitConverter.ToUInt32(rndnum, 0);
            } while (urndnum >= xcludeRndBase);
            return (int)(urndnum % (uBound - lBound)) + lBound;
        }

        protected char GetRandomCharacter()
        {
            int upperBound = pwdCharArray.GetUpperBound(0);
            if (ExcludeSymbols)
            {
                upperBound = UBoundDigit;
            }

            int randomCharPosition = GetCryptographicRandomNumber(pwdCharArray.GetLowerBound(0), upperBound);
            char randomChar = pwdCharArray[randomCharPosition];
            return randomChar;
        }

        public string Generate()
        {
            // 得到minimum 和 maximum 之间随机的长度

            //int pwdLength = GetCryptographicRandomNumber(Minimum, Maximum);
            int pwdLength = 12;//指定密码长度为12位

            StringBuilder pwdBuffer = new StringBuilder();
            pwdBuffer.Capacity = Maximum;
            // 产生随机字符
            char lastCharacter, nextCharacter;
            // 初始化标记

            lastCharacter = nextCharacter = '\n';

            for (int i = 0; i < pwdLength; i++)
            {
                nextCharacter = GetRandomCharacter();
                if (false == ConsecutiveCharacters)
                {
                    while (lastCharacter == nextCharacter)
                    {
                        nextCharacter = GetRandomCharacter();
                    }
                }

                if (false == RepeatCharacters)
                {
                    string temp = pwdBuffer.ToString();
                    int duplicateIndex = temp.IndexOf(nextCharacter);

                    while (-1 != duplicateIndex)
                    {
                        nextCharacter = GetRandomCharacter();
                        duplicateIndex = temp.IndexOf(nextCharacter);
                    }
                }

                if ((null != Exclusions))
                {
                    while (-1 != Exclusions.IndexOf(nextCharacter))
                    {
                        nextCharacter = GetRandomCharacter();
                    }
                }
                pwdBuffer.Append(nextCharacter);
                lastCharacter = nextCharacter;
            }


            if (null != pwdBuffer)
            {
                return pwdBuffer.ToString();
            }
            else
            {
                return String.Empty;
            }
        }


        public bool ConsecutiveCharacters
        {
            get { return hasConsecutive; }
            set { hasConsecutive = value; }
        }

        public bool ExcludeSymbols
        {
            get { return hasSymbols; }
            set { hasSymbols = value; }
        }

        public string Exclusions
        {
            get { return exclusionSet; }
            set { exclusionSet = value; }
        }

        public int Maximum
        {
            get { return maxSize; }
            set
            {
                maxSize = value;
                if (minSize >= maxSize)
                {
                    maxSize = DefaultMaximum;
                }
            }
        }

        public int Minimum
        {
            get { return minSize; }
            set
            {
                minSize = value;
                if (DefaultMinimum > minSize)
                {
                    minSize = DefaultMinimum;
                }
            }
        }

        public bool RepeatCharacters
        {
            get { return hasRepeating; }
            set { hasRepeating = value; }
        }


        private const int DefaultMaximum = 12;
        private const int DefaultMinimum = 11;
        private const int UBoundDigit = 61;
        private string exclusionSet;
        private bool hasConsecutive;
        private bool hasRepeating;
        private bool hasSymbols;
        private int maxSize;
        private int minSize;
        private char[] pwdCharArray = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789".ToCharArray();
        private RNGCryptoServiceProvider rng;
    }
    #endregion


}
