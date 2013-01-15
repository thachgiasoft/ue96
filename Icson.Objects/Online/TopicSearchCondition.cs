using System;
using System.Collections.Generic;
using System.Text;

namespace Icson.Objects.Online
{
    #region Topic Search Conditions

    [Serializable]
    public class TopicSearchCondition
    {
        public DateTime? DateFrom = null;
        public DateTime? DateTo = null;
        public int? ProductSysNo = null;
        public string Category = string.Empty;
        public int? Status = null;
        public int? PMSysNo = null;
        public int? ProductStatus = null;
        public bool? IsTop = null;
        public bool? IsDigest = null;
        public bool? IsComplain = null;
        public int? TypeOfTopic = null;
        public string ScoreSign = string.Empty;
        public string CustomerName = string.Empty;
        public string CustomerId = string.Empty;

        public bool InEmpty
        {
            get
            {
                if (CustomerName != string.Empty)
                    return false;
                if (CustomerId != string.Empty)
                    return false;

                if (ScoreSign != string.Empty)
                    return false;
                if (TypeOfTopic != null)
                    return false;
                if (IsComplain != null)
                    return false;
                if (DateFrom != null)
                    return false;
                if (DateTo != null)
                    return false;
                if (ProductSysNo != null)
                    return false;
                if (Category != string.Empty)
                    return false;
                if (Status != null)
                    return false;
                if (PMSysNo != null)
                    return false;
                if (ProductStatus != null)
                    return false;
                if (IsTop != null)
                    return false;
                if (IsDigest != null)
                    return false;
                else
                    return true;
            }
        }
    }

    [Serializable]
    public class TopicReplySearchCondition
    {
        public DateTime? DateFrom = null;
        public DateTime? DateTo = null;
        public int? Status = null;
        public string CustomerId = string.Empty;

        public bool InEmpty
        {
            get
            {
                if (DateFrom != null)
                    return false;
                if (DateTo != null)
                    return false;
                if (CustomerId != string.Empty)
                    return false;
                if (Status != null)
                    return false;
                else
                    return true;
            }
        }
    }

    [Serializable]
    public class CustomerTopicSearchCondition
    {
        public int? CustomerSysNo = null;
        public string CustomerID = string.Empty;
        public int TopicCountSign = 0;
        public int? TopicCount = null;
        public int DigestCountSign = 0;
        public int? DigestCount = null;
        public int ReplyCountSign = 0;
        public int? ReplyCount = null;
        public int RemarkCountSign = 0;
        public int? RemarkCount = null;
        public int? Rights = null;

        public bool IsEmpty
        {
            get
            {
                if (CustomerSysNo != null)
                    return false;
                if (CustomerID != string.Empty)
                    return false;
                if (TopicCount != null)
                    return false;
                if (DigestCount != null)
                    return false;
                if (ReplyCount != null)
                    return false;
                if (RemarkCount != null)
                    return false;
                if (Rights != null)
                    return false;
                else
                    return true;

            }
        }
    }

    [Serializable]
    public class TopicImageSearchCondition
    {
        public DateTime? DateFrom = null;
        public DateTime? DateTo = null;
        public int? Status = null;
        public string CustomerId = string.Empty;

        public bool InEmpty
        {
            get
            {
                if (DateFrom != null)
                    return false;
                if (DateTo != null)
                    return false;
                if (CustomerId != string.Empty)
                    return false;
                if (Status != null)
                    return false;
                else
                    return true;
            }
        }
    }
    #endregion
}