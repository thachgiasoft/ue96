using System;

using Icson.Utils;

namespace Icson.Objects.Basic
{
    /// <summary>
    /// Summary description for Category2Info.
    /// </summary>
    public class Size2Info : IComparable
    {
        public Size2Info()
        {
            Init();
        }

        private int _sysno;
        private int _Size1sysno;
        private string _Size2id;
        private string _Size2name;
        private int _status;
        private string _Size2Img;

        //public void Init()
        //{
        //    _sysno = AppConst.IntNull;
        //    _Size1sysno = AppConst.IntNull;
        //    _Size2id = AppConst.StringNull;
        //    _Size2name = AppConst.StringNull;
        //    _status = AppConst.IntNull;
        //}

        //public int SysNo
        //{
        //    get
        //    {
        //        return _sysno;
        //    }
        //    set
        //    {
        //        _sysno = value;
        //    }
        //}
        //public int Size1SysNo
        //{
        //    get
        //    {
        //        return _Size1sysno;
        //    }
        //    set
        //    {
        //        _Size1sysno = value;
        //    }
        //}
        //public string Size2ID
        //{
        //    get
        //    {
        //        return _Size2id;
        //    }
        //    set
        //    {
        //        _Size2id = value;
        //    }
        //}
        //public string Size2Name
        //{
        //    get
        //    {
        //        return _Size2name;
        //    }
        //    set
        //    {
        //        _Size2name = value;
        //    }
        //}
        //public int Status
        //{
        //    get
        //    {
        //        return _status;
        //    }
        //    set
        //    {
        //        _status = value;
        //    }
        //}
        public int SysNo;
        public int Size1SysNo;
        public string Size2ID;
        public string Size2Name;
        public int Status;
        public string Size2Img;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            Size1SysNo = AppConst.IntNull;
            Size2ID = AppConst.StringNull;
            Size2Name = AppConst.StringNull;
            Status = AppConst.IntNull;
            Size2Img = AppConst.StringNull;
        }

        public override string ToString()
        {
            return "[" + this.Size2ID + "] " + this.Size2Name;
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            Size2Info b = obj as Size2Info;

            if (this.Status < b.Status)
                return 1;
            else if (this.Status > b.Status)
                return -1;
            else
            {
                int result = String.Compare(this.Size2ID, b.Size2ID);
                if (result > 0)
                    return 1;
                else
                    return -1;
            }
        }
        #endregion
    }
}