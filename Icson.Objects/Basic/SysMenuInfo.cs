using System;
using System.Collections.Generic;
using System.Text;
using Icson.Utils;

namespace Icson.Objects.Basic
{
    public class SysMenuInfo : IComparable
    {
        public SysMenuInfo()
        {
            Init();
        }

        public int SysNo;
        public int MenuID;
        public int ParentID;
        public int OrderNum;
        public int SubOrder;
        public string Name;
        public string Description;
        public string href;
        public string help;
        public string Icon;
        public string Privilege;

        private void Init()
        {
            SysNo = AppConst.IntNull;
            MenuID = AppConst.IntNull;
            ParentID = AppConst.IntNull;
            OrderNum = AppConst.IntNull;
            SubOrder = AppConst.IntNull;
            Name = AppConst.StringNull;
            Description = AppConst.StringNull;
            href = AppConst.StringNull;
            help = AppConst.StringNull;
            Icon = AppConst.StringNull;
            Privilege = AppConst.StringNull;
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            SysMenuInfo b = obj as SysMenuInfo;
            return String.Compare(this.MenuID.ToString(), b.MenuID.ToString());
        }

        #endregion

    }
}
