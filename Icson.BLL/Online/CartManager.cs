using System;
using System.Data;
using System.Collections;
using System.Text;

using Icson.Utils;
using Icson.Objects.Online;
using Icson.Objects.Basic;
using Icson.BLL.Basic;


namespace Icson.BLL.Online
{
    /// <summary>
    /// 购物车的CartManager类
    /// （单例模式生成）.
    /// </summary>
    public class CartManager
    {
        private CartManager()
        {

        }
        private static CartManager _instance;
        public static CartManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new CartManager();
            }
            return _instance;
        }

        /// <summary>
        /// 购物车里的商品
        /// 在客户端以cookie的形式保存
        /// 在服务端以HashTable的形式保存
        /// 在服务端，对购物车的各种操作以hashtable的形式进行
        /// </summary>
        /// <returns></returns>
        public Hashtable GetCartHash()
        {
            //get cart string;
            //build cart hash;
            //增加期望订购数量，ProductSysNo1,Quantity1,ExpectQty1;ProductSysNo2,Quantity2,ExpectQty2
            Hashtable ht = new Hashtable(5);

            string cartCookieValue = CookieUtil.GetDESEncryptedCookieValue("cart");
            if (cartCookieValue == null)
                return ht;
            string[] cartItems = cartCookieValue.Split(';');
            try
            {
                for (int i = 0; i < cartItems.Length; i++)
                {
                    string cartItem = cartItems[i];
                    string[] cartValues = cartItem.Split(',');
                    CartInfo oCart = new CartInfo();
                    oCart.ProductSysNo = Convert.ToInt32(cartValues[0]);
                    oCart.Quantity = Convert.ToInt32(cartValues[1]);
                    if (cartValues.Length > 2)
                    {
                        oCart.ExpectQty = Convert.ToInt32(cartValues[2]);
                    }
                    else
                    {
                        oCart.ExpectQty = Convert.ToInt32(cartValues[1]);
                    }
                    if (oCart.Quantity > 99)
                        oCart.Quantity = 99;
                    ht.Add(oCart.ProductSysNo, oCart);
                }
            }
            catch
            {

            }
            return ht;
        }

        /// <summary>
        /// 添加商品到购物车
        /// </summary>
        /// <param name="newItemHt"></param>
        public void AddToCart(Hashtable newItemHt)
        {
            if (newItemHt == null || newItemHt.Count == 0)
                return;

            Hashtable ht = GetCartHash();
            foreach (CartInfo item in newItemHt.Values)
            {
                if (ht.ContainsKey(item.ProductSysNo)) //如果存在，数量相加
                {
                    CartInfo originInfo = (CartInfo)ht[item.ProductSysNo];
                    item.Quantity += originInfo.Quantity;
                    item.ExpectQty += originInfo.ExpectQty;
                    ht[item.ProductSysNo] = item;
                }
                else
                {
                    ht.Add(item.ProductSysNo, item);
                }
            }
            SaveCart(ht);
        }

        /// <summary>
        /// 从购物车中删除一个商品
        /// </summary>
        /// <param name="productSysNo"></param>
        public void DeleteFromCart(int productSysNo)
        {
            Hashtable ht = GetCartHash();
            if (ht.ContainsKey(productSysNo))
                ht.Remove(productSysNo);
            SaveCart(ht);
        }

        public void UpdateCart(CartInfo oCart)
        {
            Hashtable ht = GetCartHash();
            if (ht.ContainsKey(oCart.ProductSysNo))
            {
                ht.Remove(oCart.ProductSysNo);
                ht.Add(oCart.ProductSysNo, oCart);
            }
            SaveCart(ht);
        }

        /// <summary>
        /// 清空购物车
        /// </summary>
        public void ClearCart()
        {
            SaveCart(null);
        }

        /// <summary>
        /// 保存购物车的信息
        /// </summary>
        /// <param name="ht"></param>
        private void SaveCart(Hashtable ht)
        {
            if (ht == null || ht.Count == 0)
            {
                CookieUtil.SetDESEncryptedCookie("cart", "", DateTime.MaxValue);
                return;
            }
            int i = 0;
            StringBuilder sb = new StringBuilder(200);
            foreach (CartInfo item in ht.Values)
            {
                if (i != 0)
                    sb.Append(";");
                sb.Append(item.ProductSysNo.ToString() + "," + item.Quantity.ToString() + "," + item.ExpectQty.ToString());
                i++;
            }
            CookieUtil.SetDESEncryptedCookie("cart", sb.ToString(), DateTime.MaxValue);
        }

        public void GetCartBriefInfo(Hashtable ht, ref int cartProductCount, ref decimal cartMoneyTotal)
        {
            DataSet ds = OnlineListManager.GetInstance().GetCartDs(ht);
            if (!Util.HasMoreRow(ds))
                return;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ProductPriceInfo oProductPrice = new ProductPriceInfo();

                oProductPrice.ProductSysNo = Util.TrimIntNull(dr["ProductSysNo"]);
                oProductPrice.CurrentPrice = Util.TrimDecimalNull(dr["CurrentPrice"]);
                oProductPrice.PointType = Util.TrimIntNull(dr["PointType"]);
                oProductPrice.IsWholeSale = Util.TrimIntNull(dr["IsWholeSale"]);
                oProductPrice.Q1 = Util.TrimIntNull(dr["Q1"]);
                oProductPrice.P1 = Util.TrimDecimalNull(dr["P1"]);
                oProductPrice.Q2 = Util.TrimIntNull(dr["Q2"]);
                oProductPrice.P2 = Util.TrimDecimalNull(dr["P2"]);
                oProductPrice.Q3 = Util.TrimIntNull(dr["Q3"]);
                oProductPrice.P3 = Util.TrimDecimalNull(dr["P3"]);

                CartInfo oCart = ht[oProductPrice.ProductSysNo] as CartInfo;

                cartProductCount += oCart.Quantity;
                cartMoneyTotal += oCart.Quantity * oProductPrice.GetRealPrice(oCart.Quantity);
            }
        }

        public DataTable BuildTable(Hashtable ht)
        {
            return null;
        }
    }
}