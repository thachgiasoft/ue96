using System;

using Icson.Utils;

namespace Icson.Objects.Basic
{
    /// <summary>
    /// Summary description for Category1Info.
    /// </summary>
    public class Size1Info : IComparable
    {
        public Size1Info()
        {
            Init();
        }

        //private int _sysno;
        //private string _Size1id;
        //private string _Size1name;
        //private int _status;

        //public void Init()
        //{
        //    _sysno = AppConst.IntNull;
        //    _Size1id = AppConst.StringNull;
        //    _Size1name = AppConst.StringNull;
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
        //public string Size1ID
        //{
        //    get
        //    {
        //        return _Size1id;
        //    }
        //    set
        //    {
        //        _Size1id = value;
        //    }
        //}
        //public string Size1Name
        //{
        //    get
        //    {
        //        return _Size1name;
        //    }
        //    set
        //    {
        //        _Size1name = value;
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
        //public override string ToString()
        //{
        //    return "[" + this._Size1id + "] " + this._Size1name;
        //}

        //#region IComparable Members

        //public int CompareTo(object obj)
        //{
        //    Size1Info b = obj as Size1Info;
        //    if (this.Status < b.Status)
        //        return 1;
        //    else if (this.Status > b.Status)
        //        return -1;
        //    else
        //    {
        //        int result = String.Compare(this.Size1ID, b.Size1ID);
        //        if (result > 0)
        //            return 1;
        //        else
        //            return -1;
        //    }
        //}

        //#endregion

        public int SysNo;
        public string Size1ID;
        public string Size1Name;
        public int Status;
        public string Size1Type;

        public void Init()
        {
            SysNo = AppConst.IntNull;
            Size1ID = AppConst.StringNull;
            Size1Name = AppConst.StringNull;
            Status = AppConst.IntNull;
            Size1Type = AppConst.StringNull;
        }

        public override string ToString()
        {
            return "[" + this.Size1ID + "] " + this.Size1Name;
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            Size1Info b = obj as Size1Info;
            if (this.Status < b.Status)
                return 1;
            else if (this.Status > b.Status)
                return -1;
            else
            {
                int result = String.Compare(this.Size1ID, b.Size1ID);
                if (result > 0)
                    return 1;
                else
                    return -1;
            }
        }

        #endregion
    }
}