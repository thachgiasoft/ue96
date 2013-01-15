using System;
using System.Collections.Generic;
using System.Text;


using Icson.Utils;

namespace Icson.Objects.Online
{
    public class TopicInfo
    {
        public TopicInfo()
        {
            Init();
        }
        public void Init()
        {
            this.ImageList = new List<TopicImageInfo>();
            this.ReplayList = new List<TopicReplyInfo>();
        }

        protected int _sysNo = AppConst.IntNull;
        protected AppEnum.TopicType _topicType;
        protected string _title = String.Empty;
        protected string _topicContent = String.Empty;
        protected bool _isTop;
        protected bool _isDigest;
        protected int _referenceType;
        protected int _referenceSysNo;
        protected int _totalRemarkCount;
        protected int _totalUsefulRemarkCount;
        protected int _score = 5; // 默认值					
        protected int _totalComplainCount;
        protected AppEnum.TopicStatus _status;
        protected int _createCustomerSysNo;
        protected DateTime _createDate;
        protected int _lastEditUserSysNo;
        protected DateTime _lastEditDate;
        protected string _referenceName = String.Empty;

        protected int _customerRank;
        protected string _customerID;
        protected string _customerName;


        #region Public Properties
        public string ReferenceName
        {
            get { return _referenceName; }
            set { _referenceName = value; }
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

        public int SysNo
        {
            get { return _sysNo; }
            set { _sysNo = value; }
        }
        public AppEnum.TopicType TopicType
        {
            get { return _topicType; }
            set { _topicType = value; }
        }
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        public string TopicContent
        {
            get { return _topicContent; }
            set { _topicContent = value; }
        }
        public bool IsTop
        {
            get { return _isTop; }
            set { _isTop = value; }
        }
        public bool IsDigest
        {
            get { return _isDigest; }
            set { _isDigest = value; }
        }
        public int ReferenceType
        {
            get { return _referenceType; }
            set { _referenceType = value; }
        }
        public int ReferenceSysNo
        {
            get { return _referenceSysNo; }
            set { _referenceSysNo = value; }
        }
        public int TotalRemarkCount
        {
            get { return _totalRemarkCount; }
            set { _totalRemarkCount = value; }
        }
        public int TotalUsefulRemarkCount
        {
            get { return _totalUsefulRemarkCount; }
            set { _totalUsefulRemarkCount = value; }
        }
        public int Score
        {
            get { return _score; }
            set { _score = value; }
        }
        public int TotalComplainCount
        {
            get { return _totalComplainCount; }
            set { _totalComplainCount = value; }
        }
        public AppEnum.TopicStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }
        public int CreateCustomerSysNo
        {
            get { return _createCustomerSysNo; }
            set { _createCustomerSysNo = value; }
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
        #endregion
        private string _createUsername;

        public string CreateUsername
        {
            get { return _createUsername; }
            set { _createUsername = value; }
        }

        private List<TopicReplyInfo> _replayList;

        /// <summary>
        /// 该 Topic 的回复列表
        /// </summary>
        public List<TopicReplyInfo> ReplayList
        {
            get { return _replayList; }
            set { _replayList = value; }
        }

        private List<TopicImageInfo> _imageList;

        /// <summary>
        /// 该 Topic 的图片列表
        /// </summary>
        public List<TopicImageInfo> ImageList
        {
            get { return _imageList; }
            set { _imageList = value; }
        }
    }
}