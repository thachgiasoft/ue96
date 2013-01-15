using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;
namespace Icson.Objects.RMA
{
   public class PreSaleRefundInfo
    {
      public PreSaleRefundInfo()
       {
           Init();
       }
       public int SysNo;
       public string RefundID;
       public int SOSysNo;
       public int CustomerSysNo;
       public DateTime CreateTime;
       public int CreateUserSysNo;
       public DateTime AuditTime;
       public int AuditUserSysNo;
       public DateTime RefundTime;
       public int RefundUserSysNo;
       public int UpdateUserSysNo;
       public DateTime UpdateTime;
       public int RefundPayType;
       public decimal RefundAmt;
       public int RefundCause;
       public string RefundCauseNote;
       public string CustomerAccNote;
       public string RefundNote;
       public int Status;
       public string VoucherID;
       public DateTime VoucherTime;
       public DateTime ACCAuditTime;
       public int ACCAuditUserSysNo;

       public void Init()
       {
           SysNo = AppConst.IntNull;
           RefundID = AppConst.StringNull;
           SOSysNo = AppConst.IntNull;
           CustomerSysNo = AppConst.IntNull;
           CreateTime = AppConst.DateTimeNull;
           CreateUserSysNo = AppConst.IntNull;
           AuditTime = AppConst.DateTimeNull;
           AuditUserSysNo = AppConst.IntNull;
           RefundTime = AppConst.DateTimeNull;
           RefundUserSysNo = AppConst.IntNull;
           UpdateUserSysNo = AppConst.IntNull;
           UpdateTime = AppConst.DateTimeNull;
           RefundPayType = AppConst.IntNull;
           RefundAmt = AppConst.DecimalNull;
           RefundCause = AppConst.IntNull;
           RefundCauseNote = AppConst.StringNull;
           CustomerAccNote = AppConst.StringNull;
           RefundNote = AppConst.StringNull;
           Status = AppConst.IntNull;
           VoucherID = AppConst.StringNull;
           VoucherTime = AppConst.DateTimeNull;
           ACCAuditTime = AppConst.DateTimeNull;
           ACCAuditUserSysNo = AppConst.IntNull;
       }
    }
}
