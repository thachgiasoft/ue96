using System;
using Icson.Utils;

namespace Icson.Objects.RMA
{
    /// <summary>
    /// Summary description for RMARegisterInfo.
    /// </summary>
    public class RMARegisterInfo
    {
        public RMARegisterInfo()
        {
            Init();
        }

        public int SysNo;
        public int ProductSysNo;
        public int RequestType;
        public string CustomerDesc;
        public DateTime CheckTime;
        public string CheckDesc;
        public int NewProductStatus;
        public int RevertStatus;
        public int OutBoundStatus;
        public int ReturnStatus;
        public string ResponseDesc;
        public DateTime ResponseTime;
        public string PMDunDesc;
        public DateTime PMDunTime;
        public int RefundStatus;
        public int NextHandler;
        public string Memo;
        public int Status;
        public string ProductNo;
        public int ProductIDSysNo;
        public int NewProductIDSysNo;
        public int IsWithin7Days;
        public int IsRecommendRefund;
        public int IsContactCustomer;
        public string RefundInfo;

        public int OwnBy;
        public int Location;
        public decimal Cost;

        public int RMAReason;
        public int CheckUserSysNo;
        public int ResponseUserSysNo;
        public int CloseUserSysNo;
        public DateTime CloseTime;

        public int RevertAuditUserSysNo;
        public DateTime RevertAuditTime;
        public string RevertAuditMemo;
        public int RevertProductSysNo;
        public int IsHaveInvoice;
        public int IsFullAccessory;
        public int IsFullPackage;
        public int ReceiveStockSysNo;
        public string AttachmentInfo;

        public string InspectionResultType;
        public string VendorRepairResultType;
        public int OutBoundWithInvoice;
        public string ResponseProductNo;
        public int RevertStockSysNo;

        public string SOItemPODesc;

        public int RegisterReceiveType;
        public int RefundAuditUserSysNo;
        public DateTime RefundAuditTime;
        public string RefundAuditMemo;

        public int ShiftStatus;
        public int SetShiftUserSysNo;
        public DateTime SetShiftTime;

        public int CheckRepairResult;
        public string CheckRepairNote;
        public int CheckRepairUserSysNo;
        public DateTime CheckRepairTime;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            ProductSysNo = AppConst.IntNull;
            RequestType = AppConst.IntNull;
            CustomerDesc = AppConst.StringNull;
            CheckTime = AppConst.DateTimeNull;
            CheckDesc = AppConst.StringNull;
            NewProductStatus = AppConst.IntNull;
            RevertStatus = AppConst.IntNull;
            OutBoundStatus = AppConst.IntNull;
            ReturnStatus = AppConst.IntNull;
            ResponseDesc = AppConst.StringNull;
            ResponseTime = AppConst.DateTimeNull;
            PMDunDesc = AppConst.StringNull;
            PMDunTime = AppConst.DateTimeNull;
            RefundStatus = AppConst.IntNull;
            NextHandler = AppConst.IntNull;
            Memo = AppConst.StringNull;
            Status = AppConst.IntNull;
            ProductNo = AppConst.StringNull;
            ProductIDSysNo = AppConst.IntNull;
            NewProductIDSysNo = AppConst.IntNull;
            IsWithin7Days = AppConst.IntNull;
            IsRecommendRefund = AppConst.IntNull;
            RefundInfo = AppConst.StringNull;

            OwnBy = AppConst.IntNull;
            Location = AppConst.IntNull;
            Cost = AppConst.DecimalNull;
            RMAReason = AppConst.IntNull;
            CheckUserSysNo = AppConst.IntNull;
            ResponseUserSysNo = AppConst.IntNull;
            CloseUserSysNo = AppConst.IntNull;
            CloseTime = AppConst.DateTimeNull;

            RevertAuditUserSysNo = AppConst.IntNull;
            RevertAuditTime = AppConst.DateTimeNull;
            RevertAuditMemo = AppConst.StringNull;
            RevertProductSysNo = AppConst.IntNull;
            IsHaveInvoice = AppConst.IntNull;
            IsFullAccessory = AppConst.IntNull;
            IsFullPackage = AppConst.IntNull;
            IsContactCustomer = AppConst.IntNull;
            ReceiveStockSysNo = AppConst.IntNull;
            AttachmentInfo = AppConst.StringNull;

            InspectionResultType = AppConst.StringNull;
            VendorRepairResultType = AppConst.StringNull;
            OutBoundWithInvoice = AppConst.IntNull;
            ResponseProductNo = AppConst.StringNull;
            RevertStockSysNo = AppConst.IntNull;

            SOItemPODesc = AppConst.StringNull;
            RegisterReceiveType = AppConst.IntNull;

            RefundAuditUserSysNo = AppConst.IntNull;
            RefundAuditTime = AppConst.DateTimeNull;
            RefundAuditMemo = AppConst.StringNull;

            ShiftStatus = AppConst.IntNull;
            SetShiftUserSysNo = AppConst.IntNull;
            SetShiftTime = AppConst.DateTimeNull;

            CheckRepairResult = AppConst.IntNull;
            CheckRepairNote = AppConst.StringNull;
            CheckRepairUserSysNo = AppConst.IntNull;
            CheckRepairTime = AppConst.DateTimeNull;

        }
    }
}