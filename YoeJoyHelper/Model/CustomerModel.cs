﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

using Icson.Utils;
using Icson.Objects;
using Icson.Objects.Basic;
using Icson.Objects.Online;
using Icson.Objects.Sale;
using Icson.BLL;
using Icson.BLL.Online;
using Icson.BLL.Basic;
using Icson.BLL.Sale;

namespace YoeJoyHelper.Model
{
    /// <summary>
    /// 新用户注册模型类
    /// </summary>
    public class NewRegisterCustomerModel
    {
        public string CustomerID { get; set; }
        public string PassWordInput1 { get; set; }
        public string PassWordInput2 { get; set; }
        public string CustomerEmail { get; set; }
    }

    /// <summary>
    /// 用户地址的模型类
    /// </summary>
    public class CustomerAddressModel
    {
        public string SysNo { get; set; }
        public string CustomerSysNo { get; set; }
        public string ContactName { get; set; }
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string CellPhone { get; set; }
    }

    public class CustomerShoppingCartService
    {
        /// <summary>
        /// 用户购物车中的商品
        /// </summary>
        /// <returns></returns>
        public static List<FrontDsiplayProduct> GetShoppingCartProducts(Hashtable ht)
        {
            try
            {
                DataTable data = OnlineListManager.GetInstance().NewGetCartDs(ht);
                int rowCount = data.Rows.Count;
                if (rowCount > 0)
                {

                    List<FrontDsiplayProduct> productList = new List<FrontDsiplayProduct>();

                    for (int i = 0; i < rowCount; i++)
                    {
                        productList.Add(new FrontDsiplayProduct()
                        {
                            ProductSysNo = data.Rows[i]["sysno"].ToString().Trim(),
                            ProductBriefName = data.Rows[i]["productname"].ToString().Trim(),
                            C1SysNo = int.Parse(data.Rows[i]["C1SysNo"].ToString().Trim()),
                            C2SysNo = int.Parse(data.Rows[i]["C2SysNo"].ToString().Trim()),
                            C3SysNo = int.Parse(data.Rows[i]["C3SysNo"].ToString().Trim()),
                            Price = float.Parse(data.Rows[i]["currentprice"].ToString().Trim()).ToString("0"),
                            ImgPath = data.Rows[i]["product_simg"].ToString().Trim(),
                            Weight =float.Parse(data.Rows[i]["Weight"].ToString().Trim()),
                            LimitQty=int.Parse(data.Rows[i]["LimitedQty"].ToString().Trim()),
                            AvailableQty = int.Parse(data.Rows[i]["AvailableQty"].ToString().Trim()),
                            Point = int.Parse(data.Rows[i]["Point"].ToString().Trim()),
                            BaiscPrice = float.Parse(data.Rows[i]["BasicPrice"].ToString().Trim()).ToString("0"),
                        });
                    }

                    return productList;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
    }


}
