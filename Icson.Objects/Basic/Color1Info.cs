using System;

using Icson.Utils;

namespace Icson.Objects.Basic
{
    /// <summary>
    /// Summary description for Category1Info.
    /// </summary>
    public class Color1Info : IComparable
    {
        public Color1Info()
        {
            Init();
        }

        private int _sysno;
        private string _color1id;
        private string _color1name;
        private int _status;

        public void Init()
        {
            _sysno = AppConst.IntNull;
            _color1id = AppConst.StringNull;
            _color1name = AppConst.StringNull;
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
        public string Color1ID
        {
            get
            {
                return _color1id;
            }
            set
            {
                _color1id = value;
            }
        }
        public string Color1Name
        {
            get
            {
                return _color1name;
            }
            set
            {
                _color1name = value;
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
            return "[" + this._color1id + "] " + this._color1name;
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            Color1Info b = obj as Color1Info;
            if (this.Status < b.Status)
                return 1;
            else if (this.Status > b.Status)
                return -1;
            else
            {
                int result = String.Compare(this.Color1ID, b.Color1ID);
                if (result > 0)
                    return 1;
                else
                    return -1;
            }
        }

        #endregion
    }
}