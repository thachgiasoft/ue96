using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Icson.Utils;
using Icson.Objects.Sale;
using Icson.DBAccess.Sale;

namespace Icson.BLL.Sale
{
    public class EqifaLogManager
    {
        public EqifaLogManager()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        private static EqifaLogManager _instance = new EqifaLogManager();

        public static EqifaLogManager GetInstance()
		{
			if(_instance == null)
			{
                _instance = new EqifaLogManager();
			}
			return _instance;
		}

        private void map(EqifaLogInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.SOSysNo = Util.TrimIntNull(tempdr["SOSysNo"]);
            oParam.ProductSysNo = Util.TrimIntNull(tempdr["ProductSysNo"]);
            oParam.Quantity = Util.TrimIntNull(tempdr["Quantity"]);
            oParam.Price = Util.TrimDecimalNull(tempdr["Price"]);
            oParam.EqifaLog = Util.TrimNull(tempdr["EqifaLog"]);
        }

        public int Insert(EqifaLogInfo oParam)
        {
            return new EqifaLogDac().Insert(oParam);
        }
    }
}