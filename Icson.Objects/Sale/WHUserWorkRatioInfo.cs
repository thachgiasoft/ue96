using System;
using System.Collections.Generic;
using System.Text;

using Icson.Utils;
namespace Icson.Objects.Sale
{
    public class WHUserWorkRatioInfo
    {
        public WHUserWorkRatioInfo()
        {
            Init();
        }
        public int SysNo;
        public int UserSysNo;
        public int Ratio;
        public int WorkType;
        public int BillType;
        public int WorkTimeSpan;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            UserSysNo = AppConst.IntNull;
            Ratio = AppConst.IntNull;
            WorkType = AppConst.IntNull;
            BillType = AppConst.IntNull;
            WorkTimeSpan = AppConst.IntNull;
        }
    }
}
