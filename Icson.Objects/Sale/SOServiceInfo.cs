using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Sale
{
    public class SOServiceInfo
    {
        public SOServiceInfo()
		{
			Init();
		}

        public int SysNo;
        public int SOSysNo;
        public string ServiceAddress;
        public string ServiceReceiveName;
        public string ServicePhone;
        public string ServiceExpectTime;
        public string ServiceMemo;
        public string ServiceAgreedTime;
        public string ServiceActualTime;
        public int ServiceQuality1;
        public int ServiceQuality2;
        public int ServiceQuality3;
        public string ServiceEvaluation1;
        public string ServiceEvaluation2;
        public string ServiceEvaluation3;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            SOSysNo = AppConst.IntNull;
            ServiceAddress = AppConst.StringNull;
            ServiceReceiveName = AppConst.StringNull;
            ServicePhone = AppConst.StringNull;
            ServiceExpectTime = AppConst.StringNull;
            ServiceMemo = AppConst.StringNull;
            ServiceAgreedTime = AppConst.StringNull;
            ServiceActualTime = AppConst.StringNull;
            ServiceQuality1 = AppConst.IntNull;
            ServiceQuality2 = AppConst.IntNull;
            ServiceQuality3 = AppConst.IntNull;
            ServiceEvaluation1 = AppConst.StringNull;
            ServiceEvaluation2 = AppConst.StringNull;
            ServiceEvaluation3 = AppConst.StringNull;
        }
    }
}
