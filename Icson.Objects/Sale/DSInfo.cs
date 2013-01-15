using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

using Icson.Utils;
namespace Icson.Objects.Sale
{
    /// <summary>
    /// Delivery Settlement ≈‰ÀÕΩ·À„µ•
    /// </summary>
    public class DSInfo
    {
        public DSInfo()
        {
            Init();
        }

        public Hashtable ItemHash = new Hashtable();

        public int SysNo;
        public int DLSysNo;
        public int FreightUserSysNo;
        public decimal ARAmt;
        public decimal IncomeAmt;
        public int CreateUserSysNo;
        public DateTime CreateTime;
        public int SettlementUserSysNo;
        public DateTime SettlementTime;
        public int VoucherUserSysNo;
        public string VoucherID;
        public DateTime VoucherTime;
        public int AbandonUserSysNo;
        public DateTime AbandonTime;
        public int Status;
        public int AuditUserSysNo;
        public DateTime AuditTime;
        public decimal PosFee;
        public decimal Cheque;
        public decimal Cash;
        public decimal PosGoods;
        public decimal Remittance;
        public DateTime RemittanceDate;
        public string Memo;
        public int RemittanceType;
        public int IsReceiveVoucher;
        public decimal PosGoodsCash;
        public int AccAuditUserSysNo;
        public DateTime AccAuditTime;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            DLSysNo = AppConst.IntNull;
            FreightUserSysNo = AppConst.IntNull;
            ARAmt = AppConst.DecimalNull;
            IncomeAmt = AppConst.DecimalNull;
            CreateUserSysNo = AppConst.IntNull;
            CreateTime = AppConst.DateTimeNull;
            SettlementUserSysNo = AppConst.IntNull;
            SettlementTime = AppConst.DateTimeNull;
            VoucherUserSysNo = AppConst.IntNull;
            VoucherID = AppConst.StringNull;
            VoucherTime = AppConst.DateTimeNull;
            AbandonUserSysNo = AppConst.IntNull;
            AbandonTime = AppConst.DateTimeNull;
            Status = AppConst.IntNull;
            AuditUserSysNo = AppConst.IntNull;
            AuditTime = AppConst.DateTimeNull;
            PosFee = AppConst.DecimalNull;
            Cheque = AppConst.DecimalNull;
            Cash = AppConst.DecimalNull;
            PosGoods = AppConst.DecimalNull;
            Remittance = AppConst.DecimalNull;
            RemittanceDate = AppConst.DateTimeNull;
            Memo = AppConst.StringNull;
            RemittanceType = AppConst.IntNull;
            IsReceiveVoucher = AppConst.IntNull;
            PosGoodsCash = AppConst.DecimalNull;
            AccAuditUserSysNo = AppConst.IntNull;
            AccAuditTime = AppConst.DateTimeNull;
        }
    }
}
