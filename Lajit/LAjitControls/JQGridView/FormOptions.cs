using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace LAjitControls.JQGridView
{
    public class FormOptions
    {
        private Hashtable m_htFieldList;

        public string ElementPrefix
        {
            get { return Convert.ToString(m_htFieldList["elmprefix"]); }
            set { m_htFieldList["elmprefix"] = value; }
        }

        public string ElementSuffix
        {
            get { return Convert.ToString(m_htFieldList["elmsuffix"]); }
            set { m_htFieldList["elmsuffix"] = value; }
        }

        public string Label
        {
            get { return Convert.ToString(m_htFieldList["label"]); }
            set { m_htFieldList["label"] = value; }
        }

        public int RowPosition
        {
            get { return Convert.ToInt32(m_htFieldList["rowpos"]); }
            set { m_htFieldList["rowpos"] = value; }
        }

        public int ColumnPosition
        {
            get { return Convert.ToInt32(m_htFieldList["colpos"]); }
            set { m_htFieldList["colpos"] = value; }
        }

        public FormOptions()
        {
            m_htFieldList = new Hashtable();
        }

        public override string ToString()
        {
            return GridHelper.ToCSV(m_htFieldList);
        }
    }
}
