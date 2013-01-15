using System;
using System.Collections.Generic;
using System.Text;

using Icson.Utils;
namespace Icson.Objects.Sale
{
    public class DeliveryDelayListInfo
    {
        public DeliveryDelayListInfo()
        {
            Init();
        }
        public int SysNo;
        public int CauseType;
        public string BillID;
        public int FreightUserSysNo;
        public DateTime SetDeliveryManTime;
        public int UpdateUserSysNo;
        public DateTime UpdateTime;
        public string ReviewBackNote;
        public int ReviewCauseType;
        public int ReviewBackUserSysNo;
        public DateTime ReviewBackTime;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            CauseType = AppConst.IntNull;
            BillID = AppConst.StringNull;
            FreightUserSysNo = AppConst.IntNull;
            SetDeliveryManTime = AppConst.DateTimeNull;
            UpdateUserSysNo = AppConst.IntNull;
            UpdateTime = AppConst.DateTimeNull;
            ReviewBackNote = AppConst.StringNull;
            ReviewCauseType = AppConst.IntNull;
            ReviewBackUserSysNo = AppConst.IntNull;
            ReviewBackTime = AppConst.DateTimeNull;
        }
    }
}
