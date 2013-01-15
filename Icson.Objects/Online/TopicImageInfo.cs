using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Online
{
    public class TopicImageInfo
    {
        public TopicImageInfo()
        {
            Init();
        }

        protected int _sysno;

        protected int _TopicSysNo;
        protected string _ImageLink;
        protected string _ThumbnailLink;
        protected int _HitCount;
        protected AppEnum.TopicImageStatus _Status;
        protected int _CreateUserSysNo;
        protected DateTime _CreateDate;

        public void Init()
        {
            _sysno = AppConst.IntNull;
            _TopicSysNo = AppConst.IntNull;
            _ImageLink = AppConst.StringNull;
            _ThumbnailLink = AppConst.StringNull;
            _HitCount = AppConst.IntNull;
            _Status = AppEnum.TopicImageStatus.Normal;
            _CreateUserSysNo = AppConst.IntNull;
            _CreateDate = AppConst.DateTimeNull;
        }
        public int SysNo
        {
            get { return _sysno; }
            set { _sysno = value; }
        }
        public int TopicSysNo
        {
            get { return _TopicSysNo; }
            set { _TopicSysNo = value; }
        }
        public string ImageLink
        {
            get { return _ImageLink; }
            set { _ImageLink = value; }
        }
        public string ThumbnailLink
        {
            get { return _ThumbnailLink; }
            set { _ThumbnailLink = value; }
        }
        public int HitCount
        {
            get { return _HitCount; }
            set { _HitCount = value; }
        }
        public AppEnum.TopicImageStatus Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        public int CreateUserSysNo
        {
            get { return _CreateUserSysNo; }
            set { _CreateUserSysNo = value; }
        }
        public DateTime CreateDate
        {
            get { return _CreateDate; }
            set { _CreateDate = value; }
        }
    }
}