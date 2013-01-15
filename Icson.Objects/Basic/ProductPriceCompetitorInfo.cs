using System;
using System.Collections.Generic;
using System.Text;

using Icson.Utils;

namespace Icson.Objects.Basic
{
   public class ProductPriceCompetitorInfo
    {
       public ProductPriceCompetitorInfo()
		{
			Init();
		}
        public int SysNo;
        public int ProductSysNo;
        public decimal CompetitorPrice1;
        public DateTime ImportTime1;
        public decimal CompetitorPrice2;
        public DateTime ImportTime2;
        public decimal CompetitorPrice3;
        public DateTime ImportTime3;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            ProductSysNo = AppConst.IntNull;
            CompetitorPrice1 = AppConst.DecimalNull;
            ImportTime1 = AppConst.DateTimeNull;
            CompetitorPrice2 = AppConst.DecimalNull;
            ImportTime2 = AppConst.DateTimeNull;
            CompetitorPrice3 = AppConst.DecimalNull;
            ImportTime3 = AppConst.DateTimeNull;
        }
    }
}
