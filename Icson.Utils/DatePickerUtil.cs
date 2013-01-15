using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Icson.Utils
{
    public class DatePickerUtil
    {
        public DatePickerUtil()
        {

        }

        private static DatePickerUtil _instance;
        public static DatePickerUtil GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DatePickerUtil();
            }
            return _instance;
        }

        public void setDatePickerBox(TextBox tb)
        {
            string jsString = "WdatePicker();";
            tb.Attributes["onfocus"] = jsString;
            tb.CssClass = "Wdate";
        }
    }
}
