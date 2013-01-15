using System;
using System.Collections.Generic;
using System.Text;

using Icson.Utils;
namespace Icson.Objects.Complain
{
   public class ComplainInfo
    {
       public ComplainInfo()
       {
           Init();
       }

       public int SysNo;
       public int SoSysNo;
       public int CustomerSysno;
       public string Contact;
       public string ContactPhone;
       public int AreaSysNo;
       public string Address;
       public int CreateUserSysNo;
       public DateTime CreateTime;
       public int Status;
       public int LastUpdateUserSysNo;
       public DateTime LastUpdateTime;
       public int CurrentHandleUserSysNo;
       public int AbnormalType;
       public int AbnormalCauseType;
       public string CustomerNote;
       public string EmployeeNote;
       public string AuditNote;
       public int AuditUserSysNo;
       public DateTime AuditTime;
       public int Score;
       public string ReviewBackNote;
       public int LastReviewBackUserSysNo;
       public DateTime LastReviewTime;
       public int CloseUserSysNo;
       public DateTime CloseTime;

       public void Init()
       {
           SysNo = AppConst.IntNull;
           SoSysNo = AppConst.IntNull;
           CustomerSysno = AppConst.IntNull;
           Contact = AppConst.StringNull;
           ContactPhone = AppConst.StringNull;
           AreaSysNo = AppConst.IntNull;
           Address = AppConst.StringNull;
           CreateUserSysNo = AppConst.IntNull;
           CreateTime = AppConst.DateTimeNull;
           Status = AppConst.IntNull;
           LastUpdateUserSysNo = AppConst.IntNull;
           LastUpdateTime = AppConst.DateTimeNull;
           CurrentHandleUserSysNo = AppConst.IntNull;
           AbnormalType = AppConst.IntNull;
           AbnormalCauseType = AppConst.IntNull;
           CustomerNote = AppConst.StringNull;
           EmployeeNote = AppConst.StringNull;
           AuditNote = AppConst.StringNull;
           AuditUserSysNo = AppConst.IntNull;
           AuditTime = AppConst.DateTimeNull;
           Score = AppConst.IntNull;
           ReviewBackNote = AppConst.StringNull;
           LastReviewBackUserSysNo = AppConst.IntNull;
           LastReviewTime = AppConst.DateTimeNull;
           CloseUserSysNo = AppConst.IntNull;
           CloseTime = AppConst.DateTimeNull;
       }
    }
}
