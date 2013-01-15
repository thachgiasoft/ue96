using System;
using System.Collections.Generic;
using System.Text;

namespace Icson.Objects.Online
{
    public class TopicReplyInfo
    {
        protected int _sysNo;
        protected int _topicSysNo;
        protected string _replyContent = String.Empty;
        protected AppEnum.TopicReplyStatus _status;
        protected AppEnum.CreateUserType _createUserType;
        protected int _createUserSysNo;
        protected string _createUserName;
        protected DateTime _createDate;
        protected int _lastEditUserSysNo;
        protected string _lastEditUserName;
        protected DateTime _lastEditDate;
        protected int _customerRank;
        protected string _customerID;
        protected string _customerName;

        #region Public Properties
        public int SysNo
        {
            get { return _sysNo; }
            set { _sysNo = value; }
        }
        public string CustomerID
        {
            get { return _customerID; }
            set { _customerID = value; }
        }
        public string CustomerName
        {
            get { return _customerName; }
            set { _customerName = value; }
        }
        public int CustomerRank
        {
            get { return _customerRank; }
            set { _customerRank = value; }
        }
        public int TopicSysNo
        {
            get { return _topicSysNo; }
            set { _topicSysNo = value; }
        }
        public string ReplyContent
        {
            get { return _replyContent; }
            set { _replyContent = value; }
        }
        public AppEnum.TopicReplyStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }
        public AppEnum.CreateUserType CreateUserType
        {
            get { return _createUserType; }
            set { _createUserType = value; }
        }
        public int CreateUserSysNo
        {
            get { return _createUserSysNo; }
            set { _createUserSysNo = value; }
        }
        public DateTime CreateDate
        {
            get { return _createDate; }
            set { _createDate = value; }
        }
        public int LastEditUserSysNo
        {
            get { return _lastEditUserSysNo; }
            set { _lastEditUserSysNo = value; }
        }
        public DateTime LastEditDate
        {
            get { return _lastEditDate; }
            set { _lastEditDate = value; }
        }
        public string CreateUserName
        {
            get { return _createUserName; }
            set { _createUserName = value; }
        }
        public string LastEditUserName
        {
            get { return _lastEditUserName; }
            set { _lastEditUserName = value; }
        }
        #endregion
    }
}