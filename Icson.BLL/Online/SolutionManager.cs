using System;
using System.Data;
using System.Collections;
using System.Text;

using Icson.Utils;
using System.Transactions;

using Icson.DBAccess;
using Icson.DBAccess.Online;
using Icson.Objects;
using Icson.Objects.Online;

namespace Icson.BLL.Online
{
    public class SolutionManager
    {
        private static SolutionManager _instance;
        public static SolutionManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SolutionManager();
            }
            return _instance;
        }

        public string onlineShowLimit = "and Product.Status = " + ((int)AppEnum.ProductStatus.Show).ToString() + " and (Product_Price.ClearanceSale=1 or Product_Price.currentprice>=IsNull(Product_Price.unitcost,0))";

    }
}