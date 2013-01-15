using System;
using System.Collections;
using Icson.Utils;

namespace Icson.Objects.Sale
{
    /// <summary>
    /// Summary description for SOInfo.
    /// </summary>
    public class SOInfo
    {
        public SOInfo()
        {
            Init();
        }

        public Hashtable ItemHash = new Hashtable();
        public Hashtable SaleRuleHash = new Hashtable();
        public SOVATInfo VatInfo = new SOVATInfo();
        public SOServiceInfo ServiceInfo = new SOServiceInfo();
        public SOAdwaysInfo AdwaysInfo = new SOAdwaysInfo();

        public int SysNo;
        public string SOID;
        public int Status;
        public int CustomerSysNo;
        public int StockSysNo;
        public DateTime OrderDate;
        public DateTime DeliveryDate;
        public int SalesManSysNo;
        public int IsWholeSale;
        public int IsPremium;
        public decimal PremiumAmt;
        public int ShipTypeSysNo;
        public decimal ShipPrice;
        public decimal FreeShipFeePay;
        public int PayTypeSysNo;
        public decimal PayPrice;
        public decimal SOAmt;
        public decimal DiscountAmt;

        public int CouponType;
        public string CouponCode;
        public decimal CouponAmt;

        public int PointAmt;
        public decimal CashPay;
        public int PointPay;
        public int ReceiveAreaSysNo;
        public string ReceiveContact;
        public string ReceiveName;
        public string ReceivePhone;
        public string ReceiveCellPhone;
        public string ReceiveAddress;
        public string ReceiveZip;
        public int AllocatedManSysNo;
        public int FreightUserSysNo;
        public DateTime SetDeliveryManTime;
        public int UpdateUserSysNo;
        public DateTime UpdateTime;
        public int AuditUserSysNo;
        public DateTime AuditTime;
        public int ManagerAuditUserSysNo;
        public DateTime ManagerAuditTime;
        public int OutUserSysNo;
        public DateTime OutTime;
        public int CheckQtyUserSysNo;
        public DateTime CheckQtyTime;
        public string Memo;
        public string Note;
        public string InvoiceNote;
        public int IsVAT;
        public int IsPrintPackageCover;
        public string DeliveryMemo;
        public decimal VATEMSFee;

        public DateTime ExpectDeliveryDate;
        public int ExpectDeliveryTimeSpan;
        public DateTime AuditDeliveryDate;
        public int AuditDeliveryTimeSpan;
        public DateTime SentDate;
        public int SentTimeSpan;

        public int SignByOther;
        public int HasServiceProduct;
        public int PositiveSOSysNo;

        public int CSUserSysNo;
        public int HasExpectQty;
        public int IsMergeSO;

        public int InvoiceStatus;
        public DateTime InvoiceTime;
        public int InvoiceUpdateUserSysNo;
        public DateTime AbandonInvoiceTime;
        public int RequestInvoiceType;
        public DateTime RequestInvoiceTime;
        public int InvoiceType;
        public decimal PosFee;

        public int IsLarge;
        public int DLSysNo;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            SOID = AppConst.StringNull;
            Status = AppConst.IntNull;
            CustomerSysNo = AppConst.IntNull;
            StockSysNo = AppConst.IntNull;
            OrderDate = AppConst.DateTimeNull;
            DeliveryDate = AppConst.DateTimeNull;
            SalesManSysNo = AppConst.IntNull;
            IsWholeSale = AppConst.IntNull;
            IsPremium = AppConst.IntNull;
            PremiumAmt = AppConst.DecimalNull;
            ShipTypeSysNo = AppConst.IntNull;
            ShipPrice = AppConst.DecimalNull;
            FreeShipFeePay = AppConst.DecimalNull;
            PayTypeSysNo = AppConst.IntNull;
            PayPrice = AppConst.DecimalNull;
            SOAmt = AppConst.DecimalNull;
            DiscountAmt = AppConst.DecimalNull;

            CouponType = AppConst.IntNull;
            CouponCode = AppConst.StringNull;
            CouponAmt = AppConst.DecimalNull;

            PointAmt = AppConst.IntNull;
            CashPay = AppConst.DecimalNull;
            PointPay = AppConst.IntNull;
            ReceiveAreaSysNo = AppConst.IntNull;
            ReceiveContact = AppConst.StringNull;
            ReceiveName = AppConst.StringNull;
            ReceivePhone = AppConst.StringNull;
            ReceiveCellPhone = AppConst.StringNull;
            ReceiveAddress = AppConst.StringNull;
            ReceiveZip = AppConst.StringNull;
            AllocatedManSysNo = AppConst.IntNull;
            FreightUserSysNo = AppConst.IntNull;
            SetDeliveryManTime = AppConst.DateTimeNull;
            UpdateUserSysNo = AppConst.IntNull;
            UpdateTime = AppConst.DateTimeNull;
            AuditUserSysNo = AppConst.IntNull;
            AuditTime = AppConst.DateTimeNull;
            ManagerAuditUserSysNo = AppConst.IntNull;
            ManagerAuditTime = AppConst.DateTimeNull;
            OutUserSysNo = AppConst.IntNull;
            OutTime = AppConst.DateTimeNull;
            CheckQtyUserSysNo = AppConst.IntNull;
            CheckQtyTime = AppConst.DateTimeNull;
            Memo = AppConst.StringNull;
            Note = AppConst.StringNull;
            InvoiceNote = AppConst.StringNull;
            IsVAT = AppConst.IntNull;
            IsPrintPackageCover = AppConst.IntNull;
            DeliveryMemo = AppConst.StringNull;
            VATEMSFee = AppConst.DecimalNull;

            ExpectDeliveryDate = AppConst.DateTimeNull;
            ExpectDeliveryTimeSpan = AppConst.IntNull;
            AuditDeliveryDate = AppConst.DateTimeNull;
            AuditDeliveryTimeSpan = AppConst.IntNull;
            SentDate = AppConst.DateTimeNull;
            SentTimeSpan = AppConst.IntNull;

            SignByOther = AppConst.IntNull;
            HasServiceProduct = AppConst.IntNull;

            PositiveSOSysNo = AppConst.IntNull;

            CSUserSysNo = AppConst.IntNull;
            HasExpectQty = AppConst.IntNull;
            IsMergeSO = AppConst.IntNull;

            InvoiceStatus = AppConst.IntNull;
            InvoiceTime = AppConst.DateTimeNull;
            InvoiceUpdateUserSysNo = AppConst.IntNull;
            AbandonInvoiceTime = AppConst.DateTimeNull;
            RequestInvoiceType = AppConst.IntNull;
            RequestInvoiceTime = AppConst.DateTimeNull;
            InvoiceType = AppConst.IntNull;
            PosFee = AppConst.DecimalNull;

            DLSysNo = AppConst.IntNull;
            IsLarge = AppConst.IntNull;
        }

        public decimal GetTotalAmt()
        {
            //return (this.CashPay+this.PayPrice+this.ShipPrice+this.PremiumAmt-this.DiscountAmt);
            //return (this.CashPay+this.PayPrice+this.ShipPrice+this.PremiumAmt-this.DiscountAmt + this.VATEMSFee);
            //2007-09-05 lucky
            return (this.CashPay + this.PayPrice + this.ShipPrice - this.FreeShipFeePay + this.PremiumAmt - this.DiscountAmt + this.VATEMSFee - this.CouponAmt);
        }

        public decimal GetCashPay()
        {
            return (this.SOAmt - Convert.ToDecimal(this.PointPay) / AppConst.ExchangeRate);
        }

        /// <summary>
        /// 包含打包重量
        /// </summary>
        /// <returns></returns>
        public int GetTotalWeight()
        {
            int weight = 0;
            foreach (SOItemInfo item in this.ItemHash.Values)
            {
                weight += item.Quantity * item.Weight;
            }
            return weight;
        }
    }
}