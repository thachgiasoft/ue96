using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Transactions;
using Icson.BLL.Basic;
using Icson.DBAccess;
using Icson.DBAccess.Sale;
using Icson.Objects;
using Icson.Objects.Basic;
using Icson.Utils;

namespace Icson.BLL.Sale
{
    public class FreeShipFeeManager
    {
        public FreeShipFeeManager()
        {
        }

        private static FreeShipFeeManager _instance;
        public static FreeShipFeeManager GetInstance()
        {
            if (_instance == null)
                _instance = new FreeShipFeeManager();
            return _instance;
        }

        public int InsertCustomerFreeShipFeeLog(CustomerFreeShipFeeLogInfo oParam)
        {
            return new FreeShipFeeDac().Insert(oParam);
        }

        private void map(CustomerFreeShipFeeLogInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.CustomerSysNo = Util.TrimIntNull(tempdr["CustomerSysNo"]);
            oParam.FreeShipFeeLogType = Util.TrimIntNull(tempdr["FreeShipFeeLogType"]);
            oParam.FreeShipFeeAmount = Util.TrimDecimalNull(tempdr["FreeShipFeeAmount"]);
            oParam.CreateTime = Util.TrimDateNull(tempdr["CreateTime"]);
            oParam.Memo = Util.TrimNull(tempdr["Memo"]);
            oParam.LogCheck = Util.TrimNull(tempdr["LogCheck"]);
        }

        public int InsertCustomerCommend(CustomerCommendInfo oParam)
        {
            return new FreeShipFeeDac().Insert(oParam);
        }

        public int UpdateCustomerCommendStatus(string CommendEmail,int CommendStatus)
        {
            string sql = "update customer_commend set commendstatus=" + CommendStatus + " where commendemail=" + Util.ToSqlString(CommendEmail);
            return SqlHelper.ExecuteNonQuery(sql);
        }

        public CustomerCommendInfo LoadCustomerCommend(string CommendEmail)
        {
            string sql = "select * from customer_commend where commendemail=" + Util.ToSqlString(CommendEmail);
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if(!Util.HasMoreRow(ds))
                return null;

            CustomerCommendInfo oInfo = new CustomerCommendInfo();
            map(oInfo,ds.Tables[0].Rows[0]);
            return oInfo;
        }

        public DataSet GetCommendEmailDS(int CustomerSysNo,int CommendStatus)
        {
            string sql = "select * from customer_commend where customersysno=" + CustomerSysNo + " and commendstatus=" + CommendStatus + " order by CommendTime";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if(Util.HasMoreRow(ds))
                return ds;
            else
                return null;
        }

        private void map(CustomerCommendInfo oParam, DataRow tempdr)
        {
            oParam.SysNo = Util.TrimIntNull(tempdr["SysNo"]);
            oParam.CustomerSysNo = Util.TrimIntNull(tempdr["CustomerSysNo"]);
            oParam.FriendName = Util.TrimNull(tempdr["FriendName"]);
            oParam.CommendEmail = Util.TrimNull(tempdr["CommendEmail"]);
            oParam.CommendTime = Util.TrimDateNull(tempdr["CommendTime"]);
            oParam.CommendStatus = Util.TrimIntNull(tempdr["CommendStatus"]);
        }

        public void SetFreeShipFee(int CustomerSysNo, decimal increment, int freeShipFeeLogType, string freeShipFeeLogMemo)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                int rowsAffected = new FreeShipFeeDac().SetFreeShipFee(CustomerSysNo, increment);
                if (rowsAffected != 1)
                    throw new BizException("�ͻ����˷�������ʧ�ܣ�������Ϊ�˷����㡣");

                if (increment != 0)
                {
                    CustomerFreeShipFeeLogInfo oLog = new CustomerFreeShipFeeLogInfo(CustomerSysNo,freeShipFeeLogType,increment,freeShipFeeLogMemo);
                    oLog.LogCheck = oLog.CalcLogCheck();

                    if (1 != new FreeShipFeeDac().Insert(oLog))
                        throw new BizException("�������˷����ʧ��");
                }

                scope.Complete();
            }
        }

        public void CommendCustomerEmailVerified(int CustomerSysNo,string CommendEmail)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                CustomerCommendInfo oInfo = LoadCustomerCommend(CommendEmail);
                if(oInfo == null)
                    throw new BizException("");
                decimal increment = 5; //�Ƽ��ĺ���ͨ��Email��֤����5���˷�
                int rowsAffected = new FreeShipFeeDac().SetFreeShipFee(CustomerSysNo, increment);
                if (rowsAffected != 1)
                    throw new BizException("�ͻ��˷�������ʧ�ܣ�������Ϊ�˷����㡣");

                UpdateCustomerCommendStatus(CommendEmail, (int) AppEnum.CommendStatus.Registered); //�����Ƽ�״̬

                if (increment != 0)
                {
                    int freeShipFeeLogType = (int)AppEnum.FreeShipFeeLogType.CustomerRegister;
                    string freeShipFeeLogMemo = "�Ƽ��ͻ�ע�����˷� - " + CommendEmail;
                    CustomerFreeShipFeeLogInfo oLog = new CustomerFreeShipFeeLogInfo(CustomerSysNo, freeShipFeeLogType, increment, freeShipFeeLogMemo);
                    oLog.LogCheck = oLog.CalcLogCheck();

                    if (1 != new FreeShipFeeDac().Insert(oLog))
                        throw new BizException("�����˷����ʧ��");
                }

                scope.Complete();
            }
        }

        public decimal GetValidFreeShipFee(int CustomerSysNo)
        {
            string sql = "select validfreeshipfee from customer where sysno=" + CustomerSysNo;
            return Util.ToMoney(SqlHelper.ExecuteScalar(sql).ToString());
        }

        /// <summary>
        /// ��ע��ͻ���δ���Ƽ���Email
        /// </summary>
        public bool VerifyValidEmail(string Email)
        {
            string sql = "select email from customer where email=" + Util.ToSqlString(Email) +
                         " union all select commendemail as email from customer_commend where commendemail=" +
                         Util.ToSqlString(Email);
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if(Util.HasMoreRow(ds))
                return false;
            else
                return true;
        }
        /// <summary>
        /// ��ע��ͻ���δ���Ƽ���Email�б�(�������Ƽ�ǰ���ж�)
        /// </summary>
        public bool VerifyValidEmailList(Hashtable ht)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select commendemail as email from customer_commend where commendstatus=").Append((int)AppEnum.CommendStatus.Origin).Append( " and ( 1<>1 ");
            foreach(string email in ht.Keys)
            {   
                sb.Append(" or commendemail=").Append(Util.ToSqlString(email));
            }
            string sql = sb + " )";
            DataSet ds = SqlHelper.ExecuteDataSet(sql);
            if (Util.HasMoreRow(ds))
                return false;
            else
                return true;
        }

        public void CommendCustomerEmailList(int CustomerSysNo,decimal FreeShipFee,Hashtable ht,string EmailBody)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = TransactionManager.DefaultTimeout;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                if (!VerifyValidEmailList(ht))
                    throw new BizException("�����ظ��Ƽ�!");

                CustomerInfo cInfo = CustomerManager.GetInstance().Load(CustomerSysNo);
                string CustomerName = cInfo.CustomerName;
                string mailSubject = CustomerName + " �����Ƽ�ORS�̳���";
                foreach (string email in ht.Keys)
                {
                    UpdateCustomerCommendStatus(email, (int) AppEnum.CommendStatus.Origin);
                                        
                    string mailBody = "@FriendName<br>" + EmailBody + "<br>@CustomerName<br>" + DateTime.Now.ToString(AppConst.DateFormat);
                    mailBody = mailBody.Replace("@FriendName", ht[email].ToString());
                    mailBody = mailBody.Replace("@CustomerName", CustomerName);

                    EmailInfo oEmail = new EmailInfo(email, mailSubject, mailBody);
                    EmailManager.GetInstance().InsertEmail(oEmail);
                }

                SetFreeShipFee(CustomerSysNo, FreeShipFee, (int)AppEnum.FreeShipFeeLogType.CommendCustomer, "�Ƽ�����Email");

                scope.Complete();
            }
        }
    }
}