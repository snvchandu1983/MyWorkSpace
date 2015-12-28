using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace LAjitControls.JQGridView
{
    public class GridFooter
    {
        private Hashtable m_htFieldList;

        public bool ShowDeleteButton
        {
            get { return Convert.ToBoolean(m_htFieldList[""]); }
            set { m_htFieldList[""] = value; }
        }

        public bool ShowSearchButton
        {
            get { return Convert.ToBoolean(m_htFieldList[""]); }
            set { m_htFieldList[""] = value; }
        }

        public bool ShowEditButton
        {
            get { return Convert.ToBoolean(m_htFieldList[""]); }
            set { m_htFieldList[""] = value; }
        }


        public bool ShowAddButton
        {
            get { return Convert.ToBoolean(m_htFieldList[""]); }
            set { m_htFieldList[""] = value; }
        }

        public GridFooter()
        {

            m_htFieldList = new Hashtable();
        }

        public override string ToString()
        {
            return GridHelper.ToCSV(m_htFieldList);
        }

    }
}
