using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Collections.Generic;

namespace LAjitControls.JQGridView
{
    public class ColModelCollection : IEnumerator
    {
        private List<ColModel> m_cmItems;
        private string m_ColNames;

        private int m_Count;

        public int Count
        {
            get { return m_cmItems.Count; }
        }

        public ColModelCollection()
        {
            m_cmItems = new List<ColModel>();
        }

        public string ColumnNames
        {
            get
            {
                if (m_ColNames == null)
                {
                    m_ColNames = GridHelper.ToColNameCSV(m_cmItems);
                }
                return m_ColNames;
            }
        }

        /// <summary>
        /// Generates the ColModel for the entire collection.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return GridHelper.ToCSV(m_cmItems,true);
        }

        public virtual IEnumerator GetEnumerator()
        {
            return new ColModelCollection();
        }

        private int index = -1;
        public bool MoveNext()
        {
            index++;
            if (index < m_cmItems.Count)
            {
                return true;
            }
            else
            {
                index = -1;
                return false;
            }
        }

        public object Current
        {
            get
            {
                if (index <= -1)
                {
                    throw new InvalidOperationException();
                }
                return m_cmItems[index];
            }
        }
        public void Reset()
        {
            index = -1;
        }

        #region IList Members

        public int Add(ColModel value)
        {
            m_cmItems.Add(value);
            return m_cmItems.Count - 1;
        }

        public void Clear()
        {
            m_cmItems.Clear();
        }

        public bool Contains(ColModel value)
        {
            return m_cmItems.Contains(value);
        }

        public int IndexOf(ColModel value)
        {
            return m_cmItems.IndexOf(value);
        }

        public void Insert(int index, object value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool IsFixedSize
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public bool IsReadOnly
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public void Remove(object value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void RemoveAt(int index)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ColModel this[int index]
        {
            get
            {
                return m_cmItems[index];
            }
            set
            {
                m_cmItems[index] = value;
            }
        }

        #endregion
    }
}
