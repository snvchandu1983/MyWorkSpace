using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace LAjitControls.JQGridView
{
    public enum FormatterType
    {
        integer, number, currency, date, mail, link, showlink, checkbox, select
    }

    public abstract class Formatter
    {
        private Hashtable m_htFieldList;

        //public IntegerFormatter IntegerFormatter
        //{
        //    get
        //    {
        //        return (IntegerFormatter)m_htFieldList["IntegerFormatter"];
        //    }
        //    set
        //    {
        //        m_htFieldList.Add("IntegerFormatter", value);//Overidden ToString.
        //    }
        //}

        public override string ToString()
        {
            if (m_htFieldList.Count > 1)
            {
                throw new Exception("Only a single formatter can be applied to a Column.");
            }
            else
            {
                return "formatoptions:" + m_htFieldList.GetEnumerator().Current.ToString();
            }
        }
    }

    public class IntegerFormatter : Formatter
    {
        private Hashtable m_htFieldList;

        public string ThousandsSeparator
        {
            set { m_htFieldList["thousandsSeparator"] = value; }
        }

        public string DefaultValue
        {
            set { m_htFieldList["defaultValue"] = value; }
        }

        public IntegerFormatter()
        {
            m_htFieldList = new Hashtable();
        }

        public override string ToString()
        {
            return GridHelper.ToCSV(m_htFieldList);
        }
    }

    public class NumberFormatter : Formatter
    {
        private Hashtable m_htFieldList;

        public string DecimalSeparator
        {
            set { m_htFieldList["decimalSeparator"] = value; }
        }

        public string ThousandsSeparator
        {
            set { m_htFieldList["thousandsSeparator"] = value; }
        }

        public short DecimalPlaces
        {
            set { m_htFieldList["decimalPlaces"] = value; }
        }

        public string DefaultValue
        {
            set { m_htFieldList["defaultValue"] = value; }
        }

        public NumberFormatter()
        {
            m_htFieldList = new Hashtable();
        }

        public override string ToString()
        {
            return GridHelper.ToCSV(m_htFieldList);
        }
    }

    public class CurrencyFormatter : Formatter
    {
        private Hashtable m_htFieldList;

        public string DecimalSeparator
        {
            set { m_htFieldList["decimalSeparator"] = value; }
        }

        public string ThousandsSeparator
        {
            set { m_htFieldList["thousandsSeparator"] = value; }
        }

        public string DecimalPlaces
        {
            set { m_htFieldList["decimalPlaces"] = value; }
        }

        public string DefaultValue
        {
            set { m_htFieldList["defaultValue"] = value; }
        }

        public string Prefix
        {
            set { m_htFieldList["prefix"] = value; }
        }

        public string Suffix
        {
            set { m_htFieldList["suffix"] = value; }
        }

        public CurrencyFormatter()
        {
            m_htFieldList = new Hashtable();
        }

        public override string ToString()
        {
            return GridHelper.ToCSV(m_htFieldList);
        }
    }

    public class DateFormatter : Formatter
    {
        private Hashtable m_htFieldList;

        public string SourceFormat
        {
            set { m_htFieldList["srcformat"] = value; }
        }

        public string NewFormat
        {
            set { m_htFieldList["newformat"] = value; }
        }

        public DateFormatter()
        {
            m_htFieldList = new Hashtable();
        }

        public override string ToString()
        {
            return GridHelper.ToCSV(m_htFieldList);
        }
    }

    public class LinkFormatter : Formatter
    {
        private Hashtable m_htFieldList;

        public string Target
        {
            set { m_htFieldList["target"] = value; }
        }
        public LinkFormatter()
        {
            m_htFieldList = new Hashtable();
        }

        public override string ToString()
        {
            return GridHelper.ToCSV(m_htFieldList);
        }
    }

    public class ShowLinkFormatter : Formatter
    {
        private Hashtable m_htFieldList;

        public string BaseLinkUrl
        {
            set { m_htFieldList["baseLinkUrl"] = value; }
        }

        public string ShowAction
        {
            set { m_htFieldList["showAction"] = value; }
        }

        public string AddParam
        {
            set { m_htFieldList["addParam"] = value; }
        }

        public string Target
        {
            set { m_htFieldList["target"] = value; }
        }

        public string IdName
        {
            set { m_htFieldList["idName"] = value; }
        }

        public ShowLinkFormatter()
        {
            m_htFieldList = new Hashtable();
        }

        public override string ToString()
        {
            return GridHelper.ToCSV(m_htFieldList);
        }
    }
}
