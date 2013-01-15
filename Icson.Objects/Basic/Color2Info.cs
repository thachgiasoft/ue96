using System;

using Icson.Utils;

namespace Icson.Objects.Basic
{
    /// <summary>
    /// Summary description for Category2Info.
    /// </summary>
    public class Color2Info : IComparable
    {
        public Color2Info()
        {
            Init();
        }

        private int _sysno;
        private int _color1sysno;
        private string _color2id;
        private string _color2name;
        private int _status;

        public void Init()
        {
            _sysno = AppConst.IntNull;
            _color1sysno = AppConst.IntNull;
            _color2id = AppConst.StringNull;
            _color2name = AppConst.StringNull;
            _status = AppConst.IntNull;
        }

        public int SysNo
        {
            get
            {
                return _sysno;
            }
            set
            {
                _sysno = value;
            }
        }
        public int Color1SysNo
        {
            get
            {
                return _color1sysno;
            }
            set
            {
                _color1sysno = value;
            }
        }
        public string Color2ID
        {
            get
            {
                return _color2id;
            }
            set
            {
                _color2id = value;
            }
        }
        public string Color2Name
        {
            get
            {
                return _color2name;
            }
            set
            {
                _color2name = value;
            }
        }
        public int Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }
        public override string ToString()
        {
            return "[" + this._color2id + "] " + this._color2name;
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            Color2Info b = obj as Color2Info;

            if (this.Status < b.Status)
                return 1;
            else if (this.Status > b.Status)
                return -1;
            else
            {
                int result = String.Compare(this.Color2ID, b.Color2ID);
                if (result > 0)
                    return 1;
                else
                    return -1;
            }
        }
        #endregion
    }
}