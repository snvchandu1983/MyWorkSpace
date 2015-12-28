using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace LAjitControls.JQGridView
{
    public class XMLReader
    {
        private Hashtable m_htFieldList;

        public string Root
        {
            get { return Convert.ToString(m_htFieldList["root"]); }
            set { m_htFieldList["root"] = value; }
        }

        public string Row
        {
            get { return Convert.ToString(m_htFieldList["row"]); }
            set { m_htFieldList["row"] = value; }
        }

        public string Page
        {
            get { return Convert.ToString(m_htFieldList["page"]); }
            set { m_htFieldList["page"] = value; }
        }

        public string Total
        {
            get { return Convert.ToString(m_htFieldList["total"]); }
            set { m_htFieldList["total"] = value; }
        }

        public string Records
        {
            get { return Convert.ToString(m_htFieldList["records "]); }
            set { m_htFieldList["records "] = value; }
        }

        public string RepeatItems
        {
            get { return Convert.ToString(m_htFieldList["repeatitems"]); }
            set { m_htFieldList["repeatitems"] = value; }
        }


        public string ShowAddButton
        {
            get { return Convert.ToString(m_htFieldList["total"]); }
            set { m_htFieldList["total"] = value; }
        }

        public string Cell
        {
            get { return Convert.ToString(m_htFieldList["cell"]); }
            set { m_htFieldList["cell"] = value; }
        }

        public string Id
        {
            get { return Convert.ToString(m_htFieldList["id"]); }
            set { m_htFieldList["id"] = value; }
        }

        public string UserData
        {
            get { return Convert.ToString(m_htFieldList["userdata"]); }
            set { m_htFieldList["userdata"] = value; }
        }

        public XMLReader()
        {
            m_htFieldList = new Hashtable();
        }

        public override string ToString()
        {
            return GridHelper.ToCSV(m_htFieldList);
        }
    }
}
