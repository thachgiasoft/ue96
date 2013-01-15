using System;
using System.Data;
using System.Collections;
using System.Text;

using Icson.Utils;
using System.Transactions;
using Icson.Objects;
using Icson.Objects.Basic;
using Icson.DBAccess;
using Icson.DBAccess.Basic;

using Icson.Objects.Sale;
using Icson.BLL.Sale;

namespace Icson.BLL.Basic
{
    /// <summary>
    /// Summary description for CustomerManager.
    /// </summary>
    public class CustomerManager
    {
        private CustomerManager()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private static CustomerManager _instance;

        public static CustomerManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new CustomerManager();
            }
            return _instance;
        }

        private void map(CustomerInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.CustomerID = Util.TrimNull(tempdr["CustomerID"]);
            oParam.Pwd = Util.TrimNull(tempdr["Pwd"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
            oParam.CustomerName = Util.TrimNull(tempdr["CustomerName"]);
            oParam.Gender = Util.TrimIntNull(tempdr["Gender"]);
            oParam.Email = Util.TrimNull(tempdr["Email"]);
            oParam.Phone = Util.TrimNull(tempdr["Phone"]);
            oParam.CellPhone = Util.TrimNull(tempdr["CellPhone"]);
            oParam.Fax = Util.TrimNull(tempdr["Fax"]);
            oParam.DwellAreaSysNo = Util.TrimIntNull(tempdr["DwellAreaSysNo"]);
            oParam.DwellAddress = Util.TrimNull(tempdr["DwellAddress"]);
            oParam.DwellZip = Util.TrimNull(tempdr["DwellZip"]);
            oParam.ReceiveName = Util.TrimNull(tempdr["ReceiveName"]);
            oParam.ReceiveContact = Util.TrimNull(tempdr["ReceiveContact"]);
            oParam.ReceivePhone = Util.TrimNull(tempdr["ReceivePhone"]);
            oParam.ReceiveCellPhone = Util.TrimNull(tempdr["ReceiveCellPhone"]);
            oParam.ReceiveFax = Util.TrimNull(tempdr["ReceiveFax"]);
            oParam.ReceiveAreaSysNo = Util.TrimIntNull(tempdr["ReceiveAreaSysNo"]);
            oParam.ReceiveAddress = Util.TrimNull(tempdr["ReceiveAddress"]);
            oParam.ReceiveZip = Util.TrimNull(tempdr["ReceiveZip"]);
            oParam.TotalScore = Util.TrimIntNull(tempdr["TotalScore"]);
            oParam.ValidScore = Util.TrimIntNull(tempdr["ValidScore"]);
            oParam.CardNo = Util.TrimNull(tempdr["CardNo"]);
            oParam.Note = Util.TrimNull(tempdr["Note"]);
            oParam.EmailStatus = Util.TrimIntNull(tempdr["EmailStatus"]);
            oParam.RegisterTime = Util.TrimDateNull(tempdr["RegisterTime"]);

            oParam.LastLoginIP = Util.TrimNull(tempdr["LastLoginIP"]);
            oParam.LastLoginTime = Util.TrimDateNull(tempdr["LastLoginTime"]);
            oParam.CustomerRank = Util.TrimIntNull(tempdr["CustomerRank"]);
            oParam.IsManualRank = Util.TrimIntNull(tempdr["IsManualRank"]);
            oParam.CustomerType = Util.TrimIntNull(tempdr["CustomerType"]);

            oParam.TotalFreeShipFee = Util.TrimDecimalNull(tempdr["TotalFreeShipFee"]);
            oParam.ValidFreeShipFee = Util.TrimDecimalNull(tempdr["ValidFreeShipFee"]);
            oParam.RefCustomerSysNo = Util.TrimIntNull(tempdr["RefCustomerSysNo"]);
            oParam.NickName = Util.TrimNull(tempdr["Nickname"]);
            oParam.BirthDay = Util.TrimNull(tempdr["Birthday"]);
        }

        public CustomerInfo LoadByVip(string card)
        {
            string sql = "select * from Customer where cardno=" + Util.ToSqlString(card);
            return LoadBySql(sql);
        }
        public CustomerInfo Load(int sysNo)
        {
            string sql = "select * from Customer where sysno =" + sysNo;
            return LoadBySql(sql);
        }
        public CustomerInfo Load(string customerID)
        {
            string sql = "select * from Customer where customerID=" + Util.ToSqlString(customerID);
            return LoadBySql(sql);
        }
        private CustomerInfo LoadBySql(string sql)
        {
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;

            CustomerInfo oCustomer = new CustomerInfo();
            map(oCustomer, ds.Tables[0].Rows[0]);
            return oCustomer;
        }

        public int Insert(CustomerInfo oParam)
        {
            string sql = @"select case when CustomerID = " + Util.ToSqlString(oParam.CustomerID) + @" then 1  else 0 end as idcheck,
							case when email = " + Util.ToSqlString(oParam.Email) + @" then 1 else 0 end as emailcheck 
							from customer 
							where CustomerID = " + Util.ToSqlString(oParam.CustomerID)
                         + "	or Email = " + Util.ToSqlString(oParam.Email);
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                string errorStr = "";
                if (Util.TrimIntNull(ds.Tables[0].Rows[0]["idcheck"]) == 1)
                {
                    errorStr = "这个用户名已经被注册";
                }
                if (Util.TrimIntNull(ds.Tables[0].Rows[0]["emailcheck"]) == 1)
                {
                    if (errorStr != "")
                        errorStr += ",";
                    errorStr += "这个Email已经被注册";
                }
                if (errorStr == "")
                {
                    //可能有两rows的重复数据，理论上是不应该有的
                    errorStr = "用户名或Email已经被注册";
                }
                throw new BizException(errorStr);
            }

            oParam.SysNo = SequenceDac.GetInstance().Create("Customer_Sequence");

            return new CustomerDac().Insert(oParam);
        }

        public void Update(Hashtable ht)
        {//update customer
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                if (ht.ContainsKey("Email"))
                {
                    string sql = "select top 1 sysno from customer where sysno <>" + ht["SysNo"].ToString() + " and email=" + Util.ToSqlString(ht["Email"].ToString());
                    DataSet ds = SqlHelper.ExecuteDataSet(sql);
                    if (Util.HasMoreRow(ds))
                        throw new BizException("有其他用户在使用这个email，不能更新。");
                }

                if (1 != new CustomerDac().UpdateCustomer(ht))
                    throw new BizException("expected one-row update failed, update failed ");

                scope.Complete();
            }
        }

        public void TransferPoint(int sourceCustomerSysNo, int targetCustomerSysNo, int transferAmt)
        {
            if (transferAmt <= 0)
                throw new BizException("转移积分不能小于零");
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {

                //设置源用户积分和日志
                PointManager.GetInstance().SetScore(sourceCustomerSysNo, -1 * transferAmt, (int)AppEnum.PointLogType.TransferPoint, targetCustomerSysNo.ToString());
                //设置目标用户积分和日志
                PointManager.GetInstance().SetScore(targetCustomerSysNo, transferAmt, (int)AppEnum.PointLogType.TransferPoint, sourceCustomerSysNo.ToString());

                scope.Complete();
            }
        }

        public Hashtable GetCustomerByEmail(string email)
        {
            string sql = "select * from customer where email = " + Util.ToSqlString(email);
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                return null;
            Hashtable ht = new Hashtable(5);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                CustomerInfo oItem = new CustomerInfo();
                map(oItem, dr);
                ht.Add(oItem.SysNo, oItem);
            }
            return ht;
        }

        public DataSet GetCustomerDs(Hashtable paramHash)
        {
            string sql = @" select 
								*
							from 
								customer
							where
								1=1 ";
            if (paramHash != null && paramHash.Count != 0)
            {
                StringBuilder sb = new StringBuilder(100);
                foreach (string key in paramHash.Keys)
                {
                    sb.Append(" and ");
                    object item = paramHash[key];
                    if (key == "CustomerName")
                    {
                        string customer = "(customername like @customername or receivename like @customername or receivecontact like @customername)";
                        customer = customer.Replace("@customername", Util.ToSqlLikeString(item.ToString()));
                        sb.Append(customer);
                    }
                    else if (key == "Phone")
                    {
                        string phone = "(phone like @phone or cellphone like @phone or receivephone like @phone or receivecellphone like @phone)";
                        phone = phone.Replace("@phone", Util.ToSqlLikeString(item.ToString()));
                        sb.Append(phone);
                    }
                    else if (key == "Address")
                    {
                        string address = "(dwelladdress like @address or receiveaddress like @address)";
                        address = address.Replace("@address", Util.ToSqlLikeString(item.ToString()));
                        sb.Append(address);
                    }
                    else if (key == "DateFrom")
                    {
                        sb.Append(" registertime >= '" + item.ToString() + "'");
                    }
                    else if (key == "DateTo")
                    {
                        sb.Append(" registertime < '" + Convert.ToDateTime(item.ToString()).AddDays(1).ToString(AppConst.DateFormat) + "'");
                    }
                    else if (item is int)
                    {
                        sb.Append(key).Append("=").Append(item.ToString());
                    }
                    else if (item is string)
                    {
                        sb.Append(key).Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                    }
                }
                sql += sb.ToString();
            }
            else
            {
                sql = sql.Replace("select", "select top 50");
            }

            sql += " order by customer.sysno desc";
            return SqlHelper.ExecuteDataSet(sql);
        }

        public void ImportCustomer()
        {
            if (!AppConfig.IsImportable)
                throw new BizException("IsImportable is false");
            string sql = "select top 1  * from Customer";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                throw new BizException("The Customer is not empty");
            string sqlDataCleanUp = @"--删除无明细的主项,保留游客
									  declare @ret_value as int set @ret_value = 0 
									  begin tran 
									  delete from ipp2003..webuser_master where sysno in (select wm.sysno from ipp2003..webuser_master wm left join ipp2003..webuser_item wi on wi.webusersysno = wm.sysno
									  where wi.sysno is null) and sysno <> -100
									  if @@error<>0
										set @ret_value = -1
									  --删除非法用户名
									  delete from ipp2003..webuser_item where webusersysno in (select sysno from ipp2003..webuser_master where usercode like '%''%')
									  delete from ipp2003..webuser_master where usercode like '%''%'
									  --删除同一主项的重复明细
									  delete from ipp2003..webuser_item where webusersysno in (select webusersysno from ipp2003..webuser_item group by webusersysno having count(*)>1)
									  and sysno not in (select min(sysno) as sysno from ipp2003..webuser_item group by webusersysno having count(*)>1) 
									  if @@error<>0
										set @ret_value = -2
									  --更新同名主项的usercode=usercode+sysno，保留第一条不变
									  update ipp2003..webuser_master set usercode = (ltrim(rtrim(usercode)) +'('+ cast(sysno as nvarchar)+')') where sysno in (select wm.sysno
									  from ipp2003..webuser_master wm inner join (select ltrim(rtrim(usercode)) as usercode ,count(*) as num from ipp2003..webuser_master group by ltrim(rtrim(usercode))
									  having count(*)>1) a on ltrim(rtrim(wm.usercode)) = a.usercode) and sysno not in (select min(sysno) as sysno from ipp2003..webuser_master 
									  group by ltrim(rtrim(usercode)) having count(*)>1)
									  if @@error<>0
										set @ret_value = -3
									  if @ret_value <> 0
										rollback tran
									  else
										commit tran";
            new ProductSaleTrendDac().Exec(sqlDataCleanUp);
            string sqlCheckData = @"select * from ipp2003..webuser_Master wm left join ipp2003..webuser_item wi on wi.webusersysno = wm.sysno where wi.sysno is null and wm.sysno<> -100;
								    select ltrim(rtrim(wm.usercode)) as usercode,count(*) as number from ipp2003..webuser_master wm group by ltrim(rtrim(wm.usercode)) having count(*)>1;
									select wi.webusersysno,count(*) as number from ipp2003..webuser_item wi group by wi.webusersysno having count(*)>1;";
            DataSet dsCheckData = SqlHelper.ExecuteDataSet(sqlCheckData);
            foreach (DataTable dt in dsCheckData.Tables)
            {
                if (Util.HasMoreRow(dt))
                    throw new BizException("The DataSource is uncorrect");
            }
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                string sqlSearch = @"select wm.sysno,wm.usercode as CustomerID,(case when wm.userpassword = '' then '8888' else isnull(wm.userpassword,'8888') end) as pwd,
									 wm.status ,name as CustomerName,gender,email,phone,cellphone,fax,dwelladdress,zip  as dwellzip,shipname as receivename,
									 shipcontact as receivecontact,shipaddress as receiveaddress,shipzip as receivezip,
									 shipphone as receivephone,shipfax as receivefax,totalscore,validscore,wm.isactive as emailstatus,
									 cardnumber as cardno ,registertime ,null as receivecellphone,wi.memo as note,
									 dwellarea.newsysno as dwellareasysno, shiparea.newsysno as receiveareasysno
									 from ipp2003..webuser_master wm
									 inner join ipp2003..webuser_item wi on wi.webusersysno = wm.sysno
									 inner join ippconvert..Area dwellarea on dwellarea.oldsysno = wi.dwellareasysno
									 inner join ippconvert..Area shiparea on shiparea.oldsysno = wi.shipareasysno";
                DataSet dsSearch = SqlHelper.ExecuteDataSet(sqlSearch);
                if (Util.HasMoreRow(dsSearch))
                {
                    foreach (DataRow dr in dsSearch.Tables[0].Rows)
                    {
                        CustomerInfo oCustomer = new CustomerInfo();
                        this.map(oCustomer, dr);
                        new Icson.DBAccess.Basic.CustomerDac().Insert(oCustomer);
                    }
                }
                //insert sequence
                string sqlMaxSysNo = @"select max(sysno) as sysno from customer";
                DataSet dsMaxSysNo = SqlHelper.ExecuteDataSet(sqlMaxSysNo);
                int n = 0;
                while (n < Util.TrimIntNull(dsMaxSysNo.Tables[0].Rows[0][0]))
                {
                    n = SequenceDac.GetInstance().Create("Customer_Sequence");
                }
                //insert Guest
                string sqlInsertGuest = @"INSERT INTO Customer
										  (
										  SysNo,CustomerID, Pwd,CustomerName, Status, DwellAreaSysNo,
										  ReceiveAreaSysNo,TotalScore, ValidScore,EmailStatus
										  )
										  VALUES (
										  -100,'游客','','游客',0, 0, 
										  0, 0,0,0
										  )";
                new ProductSaleTrendDac().Exec(sqlInsertGuest);
                scope.Complete();
            }
        }

        public int SetRank(int customerSysNo)
        {
            int customerRank = (int)AppEnum.CustomerRank.Ordinary;

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                CustomerInfo oCustomer = this.Load(customerSysNo);

                //没有指定的用户，返回
                if (oCustomer == null)
                    throw new BizException("错误的用户");

                if (oCustomer.IsManualRank == (int)AppEnum.YNStatus.Yes)
                    return 0;

                string sql = @"SELECT 
									SUM( CASE WHEN outtime>@t1begin AND outtime<@t1end THEN (CashPay+PayPrice+ShipPrice-FreeShipFeePay+PremiumAmt-DiscountAmt-CouponAmt) ELSE 0 END) AS A1Money,
									SUM( CASE WHEN outtime>@t2begin AND outtime<@t2end THEN (CashPay+PayPrice+ShipPrice-FreeShipFeePay+PremiumAmt-DiscountAmt-CouponAmt) ELSE 0 END) AS A2Money,
									SUM( CASE WHEN outtime>@t3begin AND outtime<@t3end THEN (CashPay+PayPrice+ShipPrice-FreeShipFeePay+PremiumAmt-DiscountAmt-CouponAmt) ELSE 0 END) AS A3Money,
                                    SUM( CASE WHEN outtime>@t4begin AND outtime<@t4end THEN (CashPay+PayPrice+ShipPrice-FreeShipFeePay+PremiumAmt-DiscountAmt-CouponAmt) ELSE 0 END) AS A4Money,
                                    SUM( CASE WHEN outtime>@t5begin AND outtime<@t5end THEN (CashPay+PayPrice+ShipPrice-FreeShipFeePay+PremiumAmt-DiscountAmt-CouponAmt) ELSE 0 END) AS A5Money
								FROM so_master (NOLOCK)
								WHERE
									status = @SOStatus
								AND customerSysNo = @CustomerSysNo";

                DateTime t1begin, t1end, t2begin, t2end, t3begin, t3end, t4begin, t4end, t5begin, t5end;

                //t1 0-1个月    * 1
                //t2 2-3个月    * 0.9
                //t3 3-6个月    * 0.7
                //t4 6-12个月   * 0.5
                //t5 12-24个月  * 0.25

                t1end = DateTime.Now;
                t1begin = t1end.AddMonths(-1);
                t2end = t1begin;
                t2begin = t2end.AddMonths(-3);
                t3end = t2begin;
                t3begin = t3end.AddMonths(-6);
                t4end = t3begin;
                t4begin = t4end.AddMonths(-12);
                t5end = t4begin;
                t5begin = t5end.AddMonths(-24);

                sql = sql.Replace("@t1begin", Util.ToSqlString(t1begin.ToString(AppConst.DateFormatLong)));
                sql = sql.Replace("@t1end", Util.ToSqlString(t1end.ToString(AppConst.DateFormatLong)));
                sql = sql.Replace("@t2begin", Util.ToSqlString(t2begin.ToString(AppConst.DateFormatLong)));
                sql = sql.Replace("@t2end", Util.ToSqlString(t2end.ToString(AppConst.DateFormatLong)));
                sql = sql.Replace("@t3begin", Util.ToSqlString(t3begin.ToString(AppConst.DateFormatLong)));
                sql = sql.Replace("@t3end", Util.ToSqlString(t3end.ToString(AppConst.DateFormatLong)));
                sql = sql.Replace("@t4begin", Util.ToSqlString(t4begin.ToString(AppConst.DateFormat)));
                sql = sql.Replace("@t4end", Util.ToSqlString(t4end.ToString(AppConst.DateFormatLong)));
                sql = sql.Replace("@t5begin", Util.ToSqlString(t5begin.ToString(AppConst.DateFormat)));
                sql = sql.Replace("@t5end", Util.ToSqlString(t5end.ToString(AppConst.DateFormatLong)));

                sql = sql.Replace("@SOStatus", ((int)AppEnum.SOStatus.OutStock).ToString());
                sql = sql.Replace("@CustomerSysNo", customerSysNo.ToString());

                DataSet ds = SqlHelper.ExecuteDataSet(sql);
                decimal agedMoney = 0m; //无购买记录
                if (Util.HasMoreRow(ds))
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    decimal a1BuyMoney = Util.TrimDecimalNull(dr["a1money"]);
                    decimal a2BuyMoney = Util.TrimDecimalNull(dr["a2money"]);
                    decimal a3BuyMoney = Util.TrimDecimalNull(dr["a3money"]);
                    decimal a4BuyMoney = Util.TrimDecimalNull(dr["a4money"]);
                    decimal a5BuyMoney = Util.TrimDecimalNull(dr["a5money"]);

                    agedMoney = Decimal.Round(a1BuyMoney + a2BuyMoney * 0.9m + a3BuyMoney * 0.7m + a4BuyMoney * 0.5m + a5BuyMoney * 0.25m, 2);
                }

                if (agedMoney == 0)
                    customerRank = (int)AppEnum.CustomerRank.Ordinary;
                else if (agedMoney > 0 && agedMoney < 300)
                    customerRank = (int)AppEnum.CustomerRank.OneStar;
                else if (agedMoney >= 300 && agedMoney < 500)
                    customerRank = (int)AppEnum.CustomerRank.TwoStar;
                else if (agedMoney >= 500 && agedMoney < 1000)
                    customerRank = (int)AppEnum.CustomerRank.ThreeStar;
                else if (agedMoney >= 1000 && agedMoney < 2000)
                    customerRank = (int)AppEnum.CustomerRank.FourStar;
                else if (agedMoney >= 2000 && agedMoney < 3000)
                    customerRank = (int)AppEnum.CustomerRank.FiveStar;
                else if (agedMoney >= 3000 && agedMoney < 5000)
                    customerRank = (int)AppEnum.CustomerRank.Golden;
                else if (agedMoney >= 5000 && agedMoney < 15000)
                    customerRank = (int)AppEnum.CustomerRank.Diamond;
                else if (agedMoney >= 15000)
                    customerRank = (int)AppEnum.CustomerRank.VIP;

                Hashtable ht = new Hashtable(5);
                ht.Add("SysNo", customerSysNo);
                ht.Add("CustomerRank", customerRank);

                this.Update(ht);
                scope.Complete();
            }

            return customerRank;
        }

        public int InsertCustomerAddress(CustomerAddressInfo oParam)
        {
            return new CustomerDac().Insert(oParam);
        }

        public int UpdateCustomerAddress(CustomerAddressInfo oParam)
        {
            return new CustomerDac().Update(oParam);
        }

        public int DeleteCustomerAddress(int sysno)
        {
            string sql = "delete from customer_address where sysno=" + sysno;
            return SqlHelper.ExecuteNonQuery(sql);
        }

        public int CustomerAddressSetDefault(int sysno)
        {
            string sql = "update customer_address set IsDefault=" + (int)AppEnum.BiStatus.InValid + " where customersysno = (select customersysno from customer_address where sysno=" + sysno + ")";
            sql += ";update customer_address set IsDefault=" + (int)AppEnum.BiStatus.Valid + " where sysno=" + sysno;

            return SqlHelper.ExecuteNonQuery(sql);
        }

        private void map(CustomerAddressInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.CustomerSysNo = Util.TrimIntNull(tempdr["CustomerSysNo"]);
            oParam.Brief = Util.TrimNull(tempdr["Brief"]);
            oParam.Name = Util.TrimNull(tempdr["Name"]);
            oParam.Contact = Util.TrimNull(tempdr["Contact"]);
            oParam.Phone = Util.TrimNull(tempdr["Phone"]);
            oParam.CellPhone = Util.TrimNull(tempdr["CellPhone"]);
            oParam.Fax = Util.TrimNull(tempdr["Fax"]);
            oParam.Address = Util.TrimNull(tempdr["Address"]);
            oParam.Zip = Util.TrimNull(tempdr["Zip"]);
            oParam.AreaSysNo = Util.TrimIntNull(tempdr["AreaSysNo"]);
            oParam.IsDefault = Util.TrimIntNull(tempdr["IsDefault"]);
            oParam.UpdateTime = Util.TrimDateNull(tempdr["UpdateTime"]);
        }

        public DataSet GetCustomerAddressDs(int customerSysNo)
        {
            string sql = "select c.*,a.provincename + ' ' + a.cityname + ' ' + a.districtname as areaname from customer_address c left join area a on c.areasysno=a.sysno where customerSysNo=" + customerSysNo + " order by IsDefault Desc,UpdateTime";
            return SqlHelper.ExecuteDataSet(sql);
        }

        public CustomerAddressInfo LoadCustomerAddress(int sysno)
        {
            string sql = "select * from customer_address where sysno=" + sysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                CustomerAddressInfo o = new CustomerAddressInfo();
                map(o, ds.Tables[0].Rows[0]);
                return o;
            }
            else
                return null;
        }

        public DataSet LoadCustomerAddressByCustomer(int Customersysno)
        {
            string sql = "select * from customer_address where customersysno=" + Customersysno;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                return ds;
            }
            else
                return null;
        }

        public CustomerInfo LoadCustomerBySO(int soSysNo)
        {
            string sql = "select c.* from customer c inner join so_master so on c.sysno=so.customersysno where so.sysno=" + soSysNo;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
            {
                CustomerInfo oInfo = new CustomerInfo();
                map(oInfo, ds.Tables[0].Rows[0]);
                return oInfo;
            }
            else
                return null;
        }

        #region 客户增票信息

        private void map(CustomerVATInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.CustomerSysNo = Util.TrimIntNull(tempdr["CustomerSysNo"]);
            oParam.CompanyName = Util.TrimNull(tempdr["CompanyName"]);
            oParam.TaxNum = Util.TrimNull(tempdr["TaxNum"]);
            oParam.CompanyAddress = Util.TrimNull(tempdr["CompanyAddress"]);
            oParam.CompanyPhone = Util.TrimNull(tempdr["CompanyPhone"]);
            oParam.BankInfo = Util.TrimNull(tempdr["BankInfo"]);
            oParam.BankAccount = Util.TrimNull(tempdr["BankAccount"]);
            oParam.Image1 = Util.TrimNull(tempdr["Image1"]);
            oParam.Image2 = Util.TrimNull(tempdr["Image2"]);
            oParam.Image3 = Util.TrimNull(tempdr["Image3"]);
            oParam.Image4 = Util.TrimNull(tempdr["Image4"]);
            oParam.Memo = Util.TrimNull(tempdr["Memo"]);
            oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
            oParam.IsDefault = Util.TrimIntNull(tempdr["IsDefault"]);
            oParam.Status = Util.TrimIntNull(tempdr["Status"]);
        }

        public void InsertCustomerVATInfo(CustomerVATInfo oParam)
        {
            if (1 != new CustomerDac().Insert(oParam))
                throw new BizException("插入客户增票信息失败");
        }

        public void UpdateCustomerVATInfo(CustomerVATInfo oParam)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                if (oParam != null)
                {
                    //string sql = "select top 1 sysno from customer_vatinfo where sysno <>" + oParam.SysNo + " and BankAccount=" +Util.ToSqlString(oParam.BankAccount);
                    //DataSet ds = SqlHelper.ExecuteDataSet(sql);
                    //if (Util.HasMoreRow(ds))
                    //    throw new BizException("此银行账号已经被使用，请确认账号信息");
                    if (1 != new CustomerDac().Update(oParam))
                        throw new BizException("更新客户增票信息失败");
                }
                else
                {
                    throw new BizException("参数丢失，请重试");
                }

                scope.Complete();
            }
        }

        public void UpdateCustomerVATInfo(Hashtable paramHash)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                //if (ht.ContainsKey("BankAccount"))
                //{
                //    string sql = "select top 1 sysno from customer_vatinfo where sysno <>" + ht["SysNo"].ToString() + " and BankAccount=" + Util.ToSqlString(ht["BankAccount"].ToString());
                //    DataSet ds = SqlHelper.ExecuteDataSet(sql);
                //    if (Util.HasMoreRow(ds))
                //        throw new BizException("银行账号已经被使用，请确认账号信息");
                //}

                if (1 != new CustomerDac().UpdateCustomerVATInfo(paramHash))
                    throw new BizException("更新客户增票信息失败");

                scope.Complete();
            }
        }

        /// <summary>
        /// 加载客户增票信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <param name="customerSysNo">客户编号</param>
        /// <param name="isOnlyLoadDefault">是否只加载默认的增票</param>
        /// <returns></returns>
        public CustomerVATInfo LoadCustomerVATInfo(int sysNo, int customerSysNo, bool isOnlyLoadDefault)
        {
            string sql = " Select top 1 * From Customer_VATInfo Where 1=1 ";
            if (sysNo != AppConst.IntNull)
            {
                sql += " AND SysNo = " + sysNo;
            }

            if (customerSysNo != AppConst.IntNull)
            {
                sql += " AND CustomerSysNo =" + customerSysNo;
            }

            if (isOnlyLoadDefault)
            {
                sql += " AND IsDefault = " + (int)AppEnum.YNStatus.Yes;
            }

            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
            {
                return null;
            }
            else
            {
                CustomerVATInfo oInfo = new CustomerVATInfo();
                map(oInfo, ds.Tables[0].Rows[0]);
                return oInfo;
            }
        }

        public DataSet GetCustomerVATInfo(Hashtable paramHash)
        {
            string sql = "@Select * From Customer_VATInfo ";
            if (paramHash != null && paramHash.Count != 0)
            {
                StringBuilder sb = new StringBuilder(200);
                sb.Append(" Where 1=1 ");
                foreach (string key in paramHash.Keys)
                {
                    sb.Append(" AND ");
                    object item = paramHash[key];
                    if (key == "DateForm")
                    {
                        sb.Append(" CreateTime >= ").Append(Util.ToSqlString(item.ToString()));
                    }
                    else if (key == "DateEnd")
                    {
                        sb.Append(" CreateTime <= ").Append(Util.ToSqlEndDate(Util.TrimDateNull(item.ToString()).ToString(AppConst.DateFormat)));
                    }
                    else if (key == "isShowAbandon")
                    {
                        if (item.ToString() == ((int)AppEnum.YNStatus.No).ToString())
                            sb.Append(" Status <> ").Append((int)AppEnum.CustomerVATInfoStatus.Abandon);
                        else
                            sb.Append(" 1=1 ");
                    }
                    else if (key == "isOnlyShowDefault")
                    {
                        if (item.ToString() == ((int)AppEnum.YNStatus.Yes).ToString())
                        {
                            sb.Append(" IsDefault = ").Append((int)AppEnum.YNStatus.Yes);
                        }
                        else
                        {
                            sb.Append(" 1=1 ");
                        }
                    }
                    else if (item is int)
                    {
                        sb.Append(key).Append(" = ").Append(item.ToString());
                    }
                    else if (item is string)
                    {
                        sb.Append(key).Append(" like ").Append(Util.ToSqlLikeString(item.ToString()));
                    }
                }
                sql += sb.ToString();
                sql = sql.Replace("@Select", "Select");
            }
            else
            {
                sql = sql.Replace("@Select", "Select Top 50");
            }

            return SqlHelper.ExecuteDataSet(sql);
        }

        /// <summary>
        /// 作废客户增票信息
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public void AbandonCustomerVATInfo(int sysNo)
        {
            string sql = "Select * from Customer_VATInfo Where SysNo=" + sysNo + " and Status =" + (int)AppEnum.CustomerVATInfoStatus.Origin;// +" and IsDefault =" + (int)AppEnum.YNStatus.No;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                throw new BizException("当前增票信息不是初始状态，不能作废");

            Hashtable hs = new Hashtable(2);
            hs.Add("SysNo", sysNo);
            hs.Add("Status", (int)AppEnum.CustomerVATInfoStatus.Abandon);
            hs.Add("IsDefault", (int)AppEnum.YNStatus.No);

            if (1 != new CustomerDac().UpdateCustomerVATInfo(hs))
                throw new BizException("作废增票信息失败");
        }

        /// <summary>
        /// 取消作废客户增票信息
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public void CancelAbandonCustomerVATInfo(int sysNo)
        {
            string sql = "Select * from Customer_VATInfo Where SysNo=" + sysNo + " and Status =" + (int)AppEnum.CustomerVATInfoStatus.Abandon;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                throw new BizException("当前增票信息不存在或不是作废状态，不能取消作废");

            Hashtable hs = new Hashtable(2);
            hs.Add("SysNo", sysNo);
            hs.Add("Status", (int)AppEnum.CustomerVATInfoStatus.Origin);

            if (1 != new CustomerDac().UpdateCustomerVATInfo(hs))
                throw new BizException("取消作废增票信息失败");
        }

        /// <summary>
        /// 设置客户的默认增票信息
        /// </summary>
        /// <param name="sysNo"></param>
        /// <param name="customerSysNo"></param>
        public void SetDefaultCustomerVAT(int sysNo, int customerSysNo)
        {
            string sql = "Select * from Customer_VATInfo Where SysNo=" + sysNo + " and customerSysNo=" + customerSysNo + " and Status <>" + (int)AppEnum.CustomerVATInfoStatus.Abandon;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                throw new BizException("当前增票信息已经作废，不能设为默认增票信息");

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                //更新采购联系人
                sql = "Update Customer_VATInfo Set IsDefault = " + (int)AppEnum.YNStatus.No + " Where CustomerSysNo = " + customerSysNo + " AND IsDefault =" + (int)AppEnum.YNStatus.Yes;
                SqlHelper.ExecuteNonQuery(sql);
                sql = "Update Customer_VATInfo Set IsDefault = " + (int)AppEnum.YNStatus.Yes + " Where SysNo = " + sysNo + " AND CustomerSysNo = " + customerSysNo;
                SqlHelper.ExecuteNonQuery(sql);

                scope.Complete();
            }
        }

        /// <summary>
        /// 审核增票信息
        /// </summary>
        /// <param name="sysNo"></param>
        /// <param name="customerSysNo"></param>
        public void AuditCustomerVAT(int sysNo, int customerSysNo)
        {
            string sql = "Select * from Customer_VATInfo Where SysNo=" + sysNo + " and customerSysNo=" + customerSysNo + " and Status =" + (int)AppEnum.CustomerVATInfoStatus.Origin;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                throw new BizException("当前增票信息不是初始状态，不能审核");

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                //更新采购联系人
                sql = "Update Customer_VATInfo Set Status = " + (int)AppEnum.CustomerVATInfoStatus.Audited + " Where CustomerSysNo = " + customerSysNo + " AND SysNo =" + sysNo;
                if (1 != SqlHelper.ExecuteNonQuery(sql))
                {
                    throw new BizException("审核失败");
                }

                scope.Complete();
            }
        }

        /// <summary>
        /// 取消审核增票信息
        /// </summary>
        /// <param name="sysNo"></param>
        /// <param name="customerSysNo"></param>
        public void CancelAuditCustomerVAT(int sysNo, int customerSysNo)
        {
            string sql = "Select * from Customer_VATInfo Where SysNo=" + sysNo + " and customerSysNo=" + customerSysNo + " and Status =" + (int)AppEnum.CustomerVATInfoStatus.Audited;
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (!Util.HasMoreRow(ds))
                throw new BizException("当前增票信息不是审核状态，不能取消审核");

            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                //更新采购联系人
                sql = "Update Customer_VATInfo Set Status = " + (int)AppEnum.CustomerVATInfoStatus.Origin + " Where CustomerSysNo = " + customerSysNo + " AND SysNo =" + sysNo;
                if (1 != SqlHelper.ExecuteNonQuery(sql))
                {
                    throw new BizException("取消审核失败");
                }

                scope.Complete();
            }
        }

        /// <summary>
        ///  会员等级设置(如果大客户则需设置公司信息)
        /// </summary>
        /// <param name="customerHash">要更新的客户信息</param>
        /// <param name="customerVATInfo">增票信息(无增票设为null)</param>
        public void SetCustomer(Hashtable customerHash, CustomerVATInfo customerVATInfo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                if (customerHash.ContainsKey("CustomerType"))
                {
                    int customerType = Util.TrimIntNull(customerHash["CustomerType"].ToString());
                    if (customerType == (int)AppEnum.CustomerType.Enterprice || customerType == (int)AppEnum.CustomerType.VIP)
                    {
                        if (customerVATInfo != null)
                        {
                            CustomerVATInfo oldVat = LoadCustomerVATInfo(AppConst.IntNull, customerVATInfo.CustomerSysNo, true);
                            if (oldVat == null)
                            {
                                InsertCustomerVATInfo(customerVATInfo);
                            }
                            else
                            {
                                customerVATInfo.SysNo = oldVat.SysNo;
                                UpdateCustomerVATInfo(customerVATInfo);
                            }
                        }
                    }
                }

                Update(customerHash);


                scope.Complete();
            }
        }

        #endregion
    }
}