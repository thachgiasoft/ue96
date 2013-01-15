using System;
using System.Collections.Generic;
using System.Text;

using Icson.Utils;

namespace Icson.Objects.Sale
{
  public  class WhProductShelvingInspectionInfo
    {
      public WhProductShelvingInspectionInfo()
      {
          Init();
      }
        public int SysNo;
        public int WorkType;
        public int BillType;
        public int BillSysNo;
        public int AllocatedUserSysNo;
        public int RealUserSysNo;
        public int UpdateUserSysNo;
        public DateTime UpdateTime;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            WorkType = AppConst.IntNull;
            BillType = AppConst.IntNull;
            BillSysNo = AppConst.IntNull;
            AllocatedUserSysNo = AppConst.IntNull;
            RealUserSysNo = AppConst.IntNull;
            UpdateUserSysNo = AppConst.IntNull;
            UpdateTime = AppConst.DateTimeNull;
        }
    }
}
