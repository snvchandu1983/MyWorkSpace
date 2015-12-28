using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace LAjitControls.JQGridView
{
    public class EditRules
    {
        private Hashtable m_htFieldList;

        public bool EditHidden
        {
            get { return Convert.ToBoolean(m_htFieldList["edithidden"]); }
            set { m_htFieldList["edithidden"] = value; }
        }

        public bool Required
        {
            get { return Convert.ToBoolean(m_htFieldList["required"]); }
            set { m_htFieldList["required"] = value; }
        }

        public bool Number
        {
            get { return Convert.ToBoolean(m_htFieldList["number"]); }
            set { m_htFieldList["number"] = value; }
        }

        public bool Integer
        {
            get { return Convert.ToBoolean(m_htFieldList["integer"]); }
            set { m_htFieldList["integer"] = value; }
        }

        public bool Email
        {
            get { return Convert.ToBoolean(m_htFieldList["email"]); }
            set { m_htFieldList["email"] = value; }
        }

        public bool Url
        {
            get { return Convert.ToBoolean(m_htFieldList["url"]); }
            set { m_htFieldList["url"] = value; }
        }

        public bool Date
        {
            get { return Convert.ToBoolean(m_htFieldList["date"]); }
            set { m_htFieldList["date"] = value; }
        }

        public bool Time
        {
            get { return Convert.ToBoolean(m_htFieldList["time"]); }
            set { m_htFieldList["time"] = value; }
        }

        public int MinValue
        {
            get { return Convert.ToInt32(m_htFieldList["minValue"]); }
            set { m_htFieldList["minValue"] = value; }
        }

        public int MaxValue
        {
            get { return Convert.ToInt32(m_htFieldList["maxValue"]); }
            set { m_htFieldList["maxValue"] = value; }
        }

        public bool Custom
        {
            get { return Convert.ToBoolean(m_htFieldList["custom"]); }
            set { m_htFieldList["custom"] = value; }
        }

        public string CustomFunction
        {
            get { return Convert.ToString(m_htFieldList["custom_func"]); }
            set { m_htFieldList["custom_func"] = value; }
        }

        public EditRules()
        {
            m_htFieldList = new Hashtable();
        }

        public override string ToString()
        {
            return GridHelper.ToCSV(m_htFieldList);
        }
    }
}
